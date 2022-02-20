using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class PgeRESTHelper
    {
        const Dal.Helper.Shop s = Dal.Helper.Shop.PGE;
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

        class T
        {
            public string refresh_token { get; set; }
        }

        public static string GetRefreshToken()
        {

            Dal.ShopHelper sh = new Dal.ShopHelper();
            Dal.Shop shop = Dal.DbHelper.Shop.GetShop((int)s);

            WebRequest request = WebRequest.Create("https://api-marketplace.morele.net/auth/refresh");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (SHA256 sha256 = new SHA256Managed())
            {

                try
                {
                    string clcs = Bll.RESTHelper.Base64Encode(String.Format("{0}:{1}", shop.ClientId, shop.ClientSecret));
                    request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", clcs));



                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(new T()
                    {
                        refresh_token = "refreshtoken"
                    });
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();



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
