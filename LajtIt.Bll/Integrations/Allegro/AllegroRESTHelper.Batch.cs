

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using LajtIt.Dal;
using static LajtIt.Bll.AllegroRESTHelper.DraftError;

namespace LajtIt.Bll
{
    public partial class AllegroRESTHelper
    {
        public static class Batch
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
            public class Tax
            {
                public string percentage { get; set; }
            }

            public class Payments
            {
                public string invoice { get; set; }
                public Tax tax { get; set; }
            }

            public class Modification
            {
                public Payments payments { get; set; }
            }

            public class Offer
            {
                public string id { get; set; }
            }

            public class OfferCriteria
            {
                public List<Offer> offers { get; set; }
                public string type { get; set; }
            }

            public class Root
            {
                public Modification modification { get; set; }
                public List<OfferCriteria> offerCriteria { get; set; }
            }

            public class Offer1
            {
                public string id { get; set; }
            }

            public class Error
            {
                public string code { get; set; }
                public object details { get; set; }
                public string message { get; set; }
                public object path { get; set; }
                public string userMessage { get; set; }
            }

            public class Task
            {
                public string field { get; set; }
                public DateTime finishedAt { get; set; }
                public string message { get; set; }
                public Offer1 offer { get; set; }
                public DateTime scheduledAt { get; set; }
                public string status { get; set; }
                public List<Error> errors { get; set; }
            }

            public class RootResult
            {
                public List<Task> tasks { get; set; }
            }

            public static void UpdateVat()
            {
                Dal.AllegroScan asc = new Dal.AllegroScan();
                List<Dal.AllegroUser> users = asc.GetAllegroMyUsers();
                foreach (Dal.AllegroUser user in users)
                {
                    List<Dal.AllegroItem> items = Dal.DbHelper.AllegroHelper.GetAllegroItems(user.UserId, "ACTIVE");

                    for (int i = 0; i < items.Count / 1000; i++)
                    UpdateVat(user.UserId, items.Select(x => x.ItemId).Skip(i*1000).Take(1000).ToArray(), 100);
                }
            }
            private static void UpdateVat(long userId, long[] itemIds, int limit)
            {
                List<OfferCriteria> criteria = new List<OfferCriteria>();
                criteria.Add(new OfferCriteria()
                {
                    offers = itemIds.Select(x => new Offer() { id = x.ToString() }).ToList(),
                    type = "CONTAINS_OFFERS"
                });

                Payments payments = new Payments()
                {
                    invoice = "VAT",
                    tax = new Tax()
                    { percentage = "23.00" }
                };
                Modification modification = new Modification()
                {
                    payments = payments
                };
                Root root = new Root();
                root.modification = modification;

                root.offerCriteria = criteria;

                Guid g = Guid.NewGuid();

                HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offer-modification-commands/{0}", g), "PUT", null, userId);
                Stream dataStream = request.GetRequestStream();



                string jsonEncodedParams = Bll.RESTHelper.ToJson(root);


                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();


                try
                {

                    WebResponse webResponse = request.GetResponse();
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string text = reader.ReadToEnd();
                    Console.WriteLine(text);
                    //var json_serializer = new JavaScriptSerializer();
                    //validationResult = json_serializer.Deserialize<Validation.RootObject>(text);
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

                            Bll.ErrorHandler.SendError(ex, String.Format(" {0} {1}", userId, text));


                        }
                    }
                }
            }
            public static void UpdateVatResult()
            {
                 
                HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offer-modification-commands/{0}/tasks", "aad9d242-e1a0-4912-8a79-d67c3a2fb4de"), "GET", null, 678165);
              


 
                try
                {

                    WebResponse webResponse = request.GetResponse();
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string text = reader.ReadToEnd();
                    Console.WriteLine(text);
                    //var json_serializer = new JavaScriptSerializer();
                    //validationResult = json_serializer.Deserialize<Validation.RootObject>(text);
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

                            Bll.ErrorHandler.SendError(ex, String.Format(" {0} {1}", 0, text));


                        }
                    }
                }
            }
        }
    }
}