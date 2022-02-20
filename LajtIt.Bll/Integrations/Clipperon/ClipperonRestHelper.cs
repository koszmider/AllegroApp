using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class ClipperonRestHelper
    {

        #region Token



        #endregion

        private static string GetUrl()
        {
            string url = "";
            switch (GetEnv())
            {
                case Dal.Helper.EnvirotmentEnum.Dev:
                    url = "https://api.sandbox.clipperon.com";
                    break;
                case Dal.Helper.EnvirotmentEnum.Prod:
                    url = "https://api.clipperon.com";
                    break;
            }
            return url;
        }
        private static Dal.Helper.EnvirotmentEnum GetEnv()
        {
             return Dal.Helper.EnvirotmentEnum.Prod;
            return Dal.Helper.Env;
        }
        private static string GetToken()
        {
            string token = "";
            switch (GetEnv())
            {
                case Dal.Helper.EnvirotmentEnum.Dev:
                    token = Bll.RESTHelper.Base64Encode(String.Format("{0}:{1}", "mform@amazon.com", "slF-W1Qx8MZp"));
                    break;
                case Dal.Helper.EnvirotmentEnum.Prod:
                    token = Bll.RESTHelper.Base64Encode(String.Format("{0}:{1}", "amazon@m-form.com", "8N95b.4J3"));

                    break;
            }
            return token;
        }
        private static HttpWebRequest GetHttpWebRequest(Dal.Helper.Shop shop, string url, string method)
        {
            //Dal.Shop s = Dal.DbHelper.Shop.GetShop((int)shop);


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("{0}{1}", GetUrl(), url));
            string clcs = GetToken();
            request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", clcs));


            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");
            request.Accept = "application/json";
            request.ContentType = "application/json";

            request.Method = method;
            return request;
        }
        //public static string GetTextFromWebResponse(HttpWebRequest request)
        //{
        //    string text;
        //    using (WebResponse webResponse = request.GetResponse())
        //    {
        //        Stream responseStream = webResponse.GetResponseStream();
        //        StreamReader reader = new StreamReader(responseStream);
        //        text = reader.ReadToEnd();

        //        Console.WriteLine(String.Format("X-Shop-Api-Calls {0}, X-Shop-Api-Bandwidth {1}, X-Shop-Api-Limit {2}",
        //            webResponse.Headers.Get("X-Shop-Api-Calls"),
        //            webResponse.Headers.Get("X-Shop-Api-Bandwidth"),
        //            webResponse.Headers.Get("X-Shop-Api-Limit")));

        //        string apiCalls = webResponse.Headers.Get("X-Shop-Api-Calls");

        //        if (apiCalls == null)
        //            return text;

        //        int calls = Int32.Parse(apiCalls);

        //        if (calls > 7)
        //        {

        //            Dal.SettingsHelper sh = new Dal.SettingsHelper();
        //            Dal.Settings s = sh.GetSetting("ApiCalls");

        //            System.Threading.Thread.Sleep(s.IntValue.Value);
        //        }
        //    }

        //    return text;
        //}




    }
}
