using LajtIt.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class ShopRestHelper
    {
        public class Payments
        {// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
            public class PlPL
            {
                public string translation_id { get; set; }
                public string payment_id { get; set; }
                public string title { get; set; }
                public string description { get; set; }
                public string vendor_description { get; set; }
                public string active { get; set; }
                public string notify { get; set; }
                public string notify_mail { get; set; }
                public string lang_id { get; set; }
            }

             

            public class Translations
            {
                public PlPL pl_PL { get; set; } 
            }

            public class List
            {
                public int payment_id { get; set; }
                public string name { get; set; }
                public string order { get; set; }
                public string install { get; set; }
                public string visible { get; set; }
                public Translations translations { get; set; }
                public List<string> currencies { get; set; }
            }

            public class RootPayment
            {
                public string count { get; set; }
                public int pages { get; set; }
                public int page { get; set; }
                public List<List> list { get; set; }
            }


            public static RootPayment GetPayments(Dal.Helper.Shop shop)
            {

         

                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, "/webapi/rest/payments", "GET");

                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    RootPayment rootPayment = json_serializer.Deserialize<RootPayment>(text);

                    return rootPayment;
                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop);
                    return null;
                }
            }

        }
    }
}
