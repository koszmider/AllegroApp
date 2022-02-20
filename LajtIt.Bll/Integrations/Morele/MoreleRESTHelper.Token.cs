using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class MoreleRESTHelper
    {
        const Dal.Helper.Shop s = Dal.Helper.Shop.Morele;
        public class TokenReturnObject
        {
            public string access_token { get; set; } 
            public string refresh_token { get; set; }
            public int expires_in { get; set; } 
        }



        #region Token
        public static string GetToken()
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            Dal.Shop shop = Dal.DbHelper.Shop.GetShop((int)s);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            WebRequest request = WebRequest.Create("https://api-marketplace.morele.net/auth/register");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            try
            {
                using (SHA256 sha256 = new SHA256Managed())
                {


                    string clcs = Bll.RESTHelper.Base64Encode(String.Format("{0}:{1}", shop.ClientId, shop.ClientSecret));
                    request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", clcs));

                    WebResponse webResponse = request.GetResponse();
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string text = reader.ReadToEnd();



                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = Int32.MaxValue;
                    TokenReturnObject token = serializer.Deserialize<TokenReturnObject>(text);


                    Dal.Shop ss = new Dal.Shop()
                    {
                        ShopId = (int)s,
                        Token = token.access_token,
                        TokenRefresh = token.refresh_token,
                        TokenCreateDate = DateTime.Now,
                        TokenEndDate = DateTime.Now.AddSeconds(token.expires_in)
                    };
                    sh.SetShopToken(ss);

                }
            }
            catch (WebException ex)
            {

                Bll.ShopRestHelper.ProcessException(ex, s, "Pusto");
            }
            return "";
        }

        public static void GetRT()
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            Dal.Shop shop = Dal.DbHelper.Shop.GetShop((int)s);

            var request = (HttpWebRequest)WebRequest.Create("https://api-marketplace.morele.net/auth/refresh");

            try
            {
                string clcs = Bll.RESTHelper.Base64Encode(String.Format("{0}:{1}", shop.ClientId, shop.ClientSecret));
                request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", clcs));

                var postData = "refresh_token=" + Uri.EscapeDataString(shop.TokenRefresh);

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                JavaScriptSerializer serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = Int32.MaxValue
                };
                TokenReturnObject token = serializer.Deserialize<TokenReturnObject>(responseString);

                Dal.Shop ss = new Dal.Shop()
                {
                    ShopId = (int)s,
                    Token = token.access_token,
                    TokenRefresh = token.refresh_token,
                    TokenCreateDate = DateTime.Now,
                    TokenEndDate = DateTime.Now.AddSeconds(token.expires_in)
                };
                sh.SetShopToken(ss);
            }
            catch (WebException ex)
            {
                Bll.ShopRestHelper.ProcessException(ex, s, "Pusto");
            }
        }

        public class T
        {
            public string refresh_token { get; set; }
         
        }
        public static string GetRefreshToken()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Dal.ShopHelper sh = new Dal.ShopHelper();
            Dal.Shop shop = Dal.DbHelper.Shop.GetShop((int)s);

            WebRequest request = WebRequest.Create("https://api-marketplace.morele.net/auth/refresh");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");


            using (SHA256 sha256 = new SHA256Managed())
            {

                try
                {
                    string clcs = Bll.RESTHelper.Base64Encode(String.Format("{0}:{1}", shop.ClientId, shop.ClientSecret));
                    request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", clcs));

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(new T()
                    {
                        refresh_token = shop.TokenRefresh
                    });
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();


                    WebResponse webResponse = request.GetResponse();
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string text = reader.ReadToEnd();

                    JavaScriptSerializer serializer = new JavaScriptSerializer
                    {
                        MaxJsonLength = Int32.MaxValue
                    };
                    TokenReturnObject token = serializer.Deserialize<TokenReturnObject>(text);

                    Dal.Shop ss = new Dal.Shop()
                    {
                        ShopId = (int)s,
                        Token = token.access_token,
                        TokenRefresh = token.refresh_token,
                        TokenCreateDate = DateTime.Now,
                        TokenEndDate = DateTime.Now.AddSeconds(token.expires_in)
                    };
                    sh.SetShopToken(ss);
                }
                catch (WebException ex)
                {

                    Bll.ShopRestHelper.ProcessException(ex, s, "Pusto");
                }

        }
            return "";
        }

        #endregion
    }
}
