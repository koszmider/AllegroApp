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
        public class DeliveryMethod
        {
            public string id { get; set; }
            public string name { get; set; }
            public string paymentPolicy { get; set; }
        }

        public class DeliveryObject
        {
            public List<DeliveryMethod> deliveryMethods { get; set; }
        }
        public static List<DeliveryMethod> GetDeliveryMethods()
        { 
            try
            {

                HttpWebRequest request = GetHttpWebRequest("/sale/delivery-methods", "GET", null, null);


                string text = null;
                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }

                var json_serializer = new JavaScriptSerializer();
                DeliveryObject item = json_serializer.Deserialize<DeliveryObject>(text);

                return item.deliveryMethods;
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, "Błąd pobierania dostępnych metod dostaw");
                return null;
            }
        }


        #region create delivery
        public class DeliveryMethod2
        {
            public string id { get; set; } 
        }
        public class ItemRate
        {
            public string amount { get; set; }
            public string currency { get; set; }
        }
         

        public class ShippingTime
        {
            public string from { get; set; }
            public string to { get; set; }
        }

        public class Rate
        {
            public DeliveryMethod2 deliveryMethod { get; set; }
            public int maxQuantityPerPackage { get; set; }
            public  ItemRate firstItemRate { get; set; }
            public  ItemRate nextItemRate { get; set; }
            public ShippingTime shippingTime { get; set; }
        }

        public class DeliveryCreateObject
        {
            public string id { get; set; }
            public string name { get; set; }
            public List<Rate> rates { get; set; }
            public string lastModified { get; set; }
        }
        #endregion
        public static bool SetDeliveryMethod(int deliveryCostTypeId)
        {

            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.AllegroDeliveryCostType costType = oh.GetAllegroDeliveryCostType(deliveryCostTypeId);

            List<Dal.AllegoDelieryCostTypeUserFnResult> allegroDeliveryTypes = oh.GetAllegroDeliveryCostTypeForAllegro(deliveryCostTypeId);

            List<Dal.AllegroDeliveryCostsByIdResult> allegroDeliveryCosts = oh.GetAllegroDeliveryCosts(deliveryCostTypeId).Where(x=>x.AllegroDeliveryMethodId.HasValue).ToList();

            // pary Typ Cennika <-> Konto Allegro
            foreach(Dal.AllegoDelieryCostTypeUserFnResult allegroDeliveryType in allegroDeliveryTypes)
            {
                List<Rate> rates = new List<Rate>();

                
                // poszczególne pozycje cennika
                foreach (Dal.AllegroDeliveryCostsByIdResult allegroDeliveryCost in  allegroDeliveryCosts)
                {
                    Rate rate = new Rate()
                    {
                        deliveryMethod = new DeliveryMethod2() { id = allegroDeliveryCost.AllegroDeliveryMethodId.Value.ToString() },
                        firstItemRate = new ItemRate() { amount = allegroDeliveryCost.BaseCost.ToString().Replace(",","."), currency = "PLN" },
                        nextItemRate = new ItemRate() { amount = allegroDeliveryCost.NextItemCost.ToString().Replace(",", "."), currency = "PLN" },
                        maxQuantityPerPackage = allegroDeliveryCost.Quantity
                    };

                    rates.Add(rate);
                }

                DeliveryCreateObject dco = new DeliveryCreateObject()
                {
                    name = costType.Name.Substring(0, Math.Min(60, costType.Name.Length)),
                    rates = rates

                };
                if (allegroDeliveryType.AllegroShippingId.HasValue)
                    dco.id = allegroDeliveryType.AllegroShippingId.ToString();

                 
                string method = allegroDeliveryType.AllegroShippingId.HasValue ? "PUT" : "POST"; // aktualizacja/utwórz nowy
               
                    dco = SetDelivery(dco, method, allegroDeliveryType.UserId);
               
                if(dco==null)
                {
                    oh.SetAllegroDeliveryCostTypeUser(deliveryCostTypeId, allegroDeliveryType.UserId, null,  "Błąd");
                }
                else
                oh.SetAllegroDeliveryCostTypeUser(deliveryCostTypeId, allegroDeliveryType.UserId, Guid.Parse(dco.id),  "OK" );
            }
            oh.SetAllegroDeliveryPublished(deliveryCostTypeId);
            return true;

        }
        private static DeliveryCreateObject   SetDelivery(DeliveryCreateObject dco, string method, long userId)
        {
            try
            {
                string url = "/sale/shipping-rates{0}";
                if (method == "PUT")
                    url = String.Format(url, "/" + dco.id);
                else
                    url = String.Format(url, "");

                HttpWebRequest request = GetHttpWebRequest(url, method, null, userId);


                Stream dataStream = request.GetRequestStream();



                string jsonEncodedParams = Bll.RESTHelper.ToJson(dco);
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();



                WebResponse webResponse = request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();

                var json_serializer = new JavaScriptSerializer();
                DeliveryCreateObject item = json_serializer.Deserialize<DeliveryCreateObject>(text);

                return item;
            }
            catch (WebException ex)
            {

                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response; if (httpResponse == null)
                    { 
                        Bll.ErrorHandler.SendError(ex, ex.Message);
                        return null;

                    }
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();


                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd tworzenia cennika. Allegro.UserId: {0}, Nazwa: {1}, Info: {2}", userId, dco.name, text));
                    }
                }
                return null;
            }
        }
    }
}
