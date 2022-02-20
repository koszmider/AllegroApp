using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class AllegroRESTHelper
    {

        public class   Parameters
        {
            public class Options
            {
                public bool variantsAllowed { get; set; }
                public bool variantsEqual { get; set; }
            }

            public class Dictionary
            {
                public string id { get; set; }
                public string value { get; set; }
            }

            public class Restrictions
            {
                public bool multipleChoices { get; set; }
                public double? min { get; set; }
                public double? max { get; set; }
                public double? minLength { get; set; }
                public double? maxLength { get; set; }
                public bool? range { get; set; }
                public int? precision { get; set; }
            }

            public class Parameter
            { 
                public string id { get; set; }
                public string name { get; set; }
                public string type { get; set; }
                public bool required { get; set; }
                public string unit { get; set; }
                public Options options { get; set; }
                public List<Dictionary> dictionary { get; set; }
                public Restrictions restrictions { get; set; }
            }

            public class RootObject
            {
                public List<Parameter> parameters { get; set; }
            }

            public static List<Dictionary> GetDictionary(string categoryId, string fieldId)
            {
                Parameters.RootObject p = GetCategoryParameters(categoryId);

                var r = p.parameters.Where(x => x.id == fieldId).FirstOrDefault();

                if (r != null)
                    return r.dictionary;
                else
                    return null;
            }
        }

        public static Parameters.RootObject GetCategoryParameters(string categoryId)
        {
            return GetCategoryParameters(categoryId, "/sale/categories/{0}/parameters");

        }

        public static Parameters.RootObject GetCategoryProductParameters(string categoryId)
        {
            return GetCategoryParameters(categoryId, "/sale/categories/{0}/product-parameters");

        }
        private static Parameters.RootObject GetCategoryParameters(string categoryId, string url)
        {
            try
            {

                HttpWebRequest request = GetHttpWebRequest(String.Format(url, categoryId), "GET", null, null);


                string text = null;
                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }

                var json_serializer = new JavaScriptSerializer();
                Parameters.RootObject parameters = json_serializer.Deserialize<Parameters.RootObject>(text);
                return parameters;
            }

            catch (WebException ex)
            {

                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse == null)
                    {
                        Bll.ErrorHandler.SendError(ex, ex.Message);
                        return null;

                    }
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();


                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania parametrów dla kategorii {0}, {1}", categoryId, text));
                    }
                }
                return null;
            }

        }
    }
}
