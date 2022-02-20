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
    public partial class EmpikRESTHelper
    {
      
        private static HttpWebRequest GetHttpWebRequest(string url, string method)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://marketplace.empik.com{0}", url));
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, "1e2ff244-9231-4b07-a1ac-d85dc3b7edca");
            //request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");
            request.Headers.Add("Api-Key", "1e2ff244-9231-4b07-a1ac-d85dc3b7edca");
            request.Method = method;

            return request;
        }
 

    }
}
