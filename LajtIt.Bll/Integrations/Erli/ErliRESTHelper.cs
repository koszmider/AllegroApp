using LajtIt.Dal;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class ErliRESTHelper
    {
      
        private static HttpWebRequest GetHttpWebRequest(Dal.Helper.Shop shop, string url, string method)
        {

            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(String.Format("https://erli.pl/svc/shop-api{0}", url));
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer SaO6v4DWiHki:LZIxDs4HAvpRcUxC");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");
            request.Method = method;

            return request;
        }
 

    }
}
