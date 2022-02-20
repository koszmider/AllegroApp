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
    public partial class AllegroRESTHelper
    {
        #region Token
        public static string GetToken(string code, int userId)
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            Dal.AllegroUser au = asc.GetAllegroMyUsers().Where(x => x.UserId == userId).FirstOrDefault();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            WebRequest request = WebRequest.Create(
                String.Format(
                "https://allegro.pl/auth/oauth/token?{0}={1}&{2}={3}&{4}={5}",
                "grant_type",
                "authorization_code",
                "code",
                code,
                "redirect_uri",
                "https://lajtit.pl"

                ));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (SHA256 sha256 = new SHA256Managed())
            {


                string clcs = Bll.RESTHelper.Base64Encode(String.Format("{0}:{1}", au.ClientId, au.ClientSecret));
                request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", clcs));

                WebResponse webResponse = request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();

                Object response = Bll.RESTHelper.FromJson(text);


                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                TokenReturnObject token = serializer.Deserialize<TokenReturnObject>(text);

                Dal.AllegroUser allegroUser = new Dal.AllegroUser()
                {
                    UserId = userId,
                    Token = token.access_token,
                    TokenRefresh = token.refresh_token,
                    TokenCreateDate = DateTime.Now,
                    TokenEndDate = DateTime.Now.AddSeconds(token.expires_in)
                };
                asc.SetAllegroUserUpdateToken(allegroUser);

            }
            return "";
        }


        public static void GetRefreshToken()
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            List<Dal.AllegroUser> users = asc.GetAllegroMyUsers();
            foreach (Dal.AllegroUser user in users)
            {
                GetRefreshToken(user.UserId);
            }

        }
        public static string GetRefreshToken(long userId)
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            Dal.AllegroUser au = asc.GetAllegroMyUsers().Where(x => x.UserId == userId).FirstOrDefault();

            WebRequest request = WebRequest.Create(
                String.Format(
                "https://allegro.pl/auth/oauth/token?{0}={1}&{2}={3}&{4}={5}",
                "grant_type",
                "refresh_token",
                "refresh_token",
                au.TokenRefresh,
                "redirect_uri",
                "https://lajtit.pl"

                ));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (SHA256 sha256 = new SHA256Managed())
            {


                string clcs = Bll.RESTHelper.Base64Encode(String.Format("{0}:{1}", au.ClientId, au.ClientSecret));
                request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", clcs));

                WebResponse webResponse = request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();

                Object response = Bll.RESTHelper.FromJson(text);


                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                TokenReturnObject token = serializer.Deserialize<TokenReturnObject>(text);

                Dal.AllegroUser allegroUser = new Dal.AllegroUser()
                {
                    UserId = userId,
                    Token = token.access_token,
                    TokenRefresh = token.refresh_token,
                    TokenCreateDate = DateTime.Now,
                    TokenEndDate = DateTime.Now.AddSeconds(token.expires_in)
                };
                asc.SetAllegroUserUpdateToken(allegroUser);

            }
            return "";
        }

        #endregion
    }
}
