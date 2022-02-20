using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class ShopRestHelper
    {

        #region Token
        public class Token
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }
        }


        #endregion

        private static string GetUrl(Dal.Helper.Shop shop)
        {
            string url = "";
            switch(shop)
            {
                case Dal.Helper.Shop.Lajtitpl:
                    url = "https://lajtit.pl";
                    break;
                case Dal.Helper.Shop.OswietlenieTechniczne:
                    url = "http://sklep037329.shoparena.pl";
                    break;
            }
            return url;
        }
        private static HttpWebRequest GetHttpWebRequest(Dal.Helper.Shop shop, string url, string method)
        {  
            Dal.Shop s = Dal.DbHelper.Shop.GetShop((int)shop);

            bool expired = s.TokenEndDate.HasValue && s.TokenEndDate.Value.AddMinutes(-10) < DateTime.Now;

            if (s.Token == null || expired)
            {
                Login(shop);
                s = Dal.DbHelper.Shop.GetShop((int)shop);
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("{0}{1}", GetUrl(shop), url));
            if (s.Token != null)
                request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", s.Token));
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");
            request.Accept = "application/json";
            request.ContentType = "application/json";

            request.Method = method;
            return request;
        }
        public static string GetTextFromWebResponse(HttpWebRequest request)
        {
            string text;
            using (WebResponse webResponse = request.GetResponse())
            {
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                text = reader.ReadToEnd();

                Console.WriteLine(String.Format("X-Shop-Api-Calls {0}, X-Shop-Api-Bandwidth {1}, X-Shop-Api-Limit {2}, {3}",
                    webResponse.Headers.Get("X-Shop-Api-Calls"),
                    webResponse.Headers.Get("X-Shop-Api-Bandwidth"),
                    webResponse.Headers.Get("X-Shop-Api-Limit"),
                    request.RequestUri));

                string apiCalls = webResponse.Headers.Get("X-Shop-Api-Calls");

                if (apiCalls == null)
                    return text;

                int calls = Int32.Parse(apiCalls);

                if (calls > 7)
                {

                    Dal.SettingsHelper sh = new Dal.SettingsHelper();
                    Dal.Settings s = sh.GetSetting("ApiCalls");

                    System.Threading.Thread.Sleep(s.IntValue.Value);
                }
            }

            return text;
        }

        public static void Login(Dal.Helper.Shop shop)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("{0}/webapi/rest/auth", GetUrl(shop)));
                request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");
                request.Accept = "application/json";
                request.ContentType = "application/json";

                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(System.Configuration.ConfigurationManager.AppSettings["ShopWebApiLogin"]
                    + ":" + System.Configuration.ConfigurationManager.AppSettings[String.Format("ShopWebApiPwd{0}", shop.ToString())]));
                request.Headers.Add("Authorization", "Basic " + encoded);

                request.Method = "POST";



                string text = GetTextFromWebResponse(request);

                var json_serializer = new JavaScriptSerializer();


                Token token = json_serializer.Deserialize<Token>(text);


                Dal.ShopHelper sh = new Dal.ShopHelper();

                sh.SetToken(shop, token.access_token, token.expires_in);

            }

            catch (WebException ex)
            {

                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse == null)
                    {
                        Bll.ErrorHandler.SendError(ex, ex.Message);


                    }
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();


                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania kategorii {0} ", text));
                    }
                }
            }


        }
        private static Bulk.BulkResult ProcessException(WebException ex, Dal.Helper.Shop shop)
        {
            Bulk.BulkResult result = new Bulk.BulkResult()
            {
                errors = true
            };

          

            using (WebResponse response = ex.Response)
            {
                if (response != null)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse == null)
                    {
                        Bll.ErrorHandler.SendError(ex, ex.Message);
                    }
                    else
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);

                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();

                        Console.WriteLine(text);

                        var json_serializer = new JavaScriptSerializer();

                        result = json_serializer.Deserialize<Bulk.BulkResult>(text);

                        Bll.ErrorHandler.SendError(ex, String.Format("<div>Sklep: {0}, Błąd wykonania metody Bulk {1}</div>", shop.ToString(),
                            Bulk.BulkResultToString(result)));
                    }
                }
                else
                    Bll.ErrorHandler.SendError(ex, String.Format("<div>Sklep: {0}, Błąd wykonania metody Bulk </div>", shop.ToString()));

            }

            return result;
        }
        public static void ProcessException(WebException ex, Dal.Helper.Shop shop, params string[] arg)
        {
            using (WebResponse response = ex.Response)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                if (httpResponse == null)
                {
                    Bll.ErrorHandler.SendError(ex, ex.Message);
                }

                Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();

                    StringBuilder sb = new StringBuilder();
                    
                    foreach(string a in arg)
                    {
                        sb.Append(String.Format("{0}<br>", a));
                    }

                    Bll.ErrorHandler.SendError(ex, String.Format("Sklep: {0}, Błąd wykonania metody: {1}<br><br>{2}", shop.ToString(), text, sb.ToString()));
                }
            }
        }


    }
}
