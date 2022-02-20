using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{


    public partial class MoreleRESTHelper
    {
        

        public static HttpWebRequest GetHttpWebRequest(string url, string method)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            Dal.Shop shop = Dal.DbHelper.Shop.GetShop((int)s);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api-marketplace.morele.net{0}", url));
                request.ContentType = "application/x-www-form-urlencoded";

                request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", shop.Token));
                request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");


                request.Method = method;
                return request;
            }
            catch (WebException ex)
            {

                Bll.ShopRestHelper.ProcessException(ex, s, "Pusto");
            }
            return null;





            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Method = method;
            //request.ContentType = "application/x-www-form-urlencoded";
           
            //    using (SHA256 sha256 = new SHA256Managed())
            //    {


            //        string clcs = Base64Encode(String.Format("{0}:{1}", shop.ClientId, shop.ClientSecret));
            //        request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", clcs));

            //        return request;

            //    }
    
        }



    }
}
