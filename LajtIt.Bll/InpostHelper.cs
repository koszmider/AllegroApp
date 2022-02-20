using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public class InpostHelper
    {
        //int id = 3444;
       //string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJpc3MiOiJhcGktc2hpcHgtcGwuZWFzeXBhY2syNC5uZXQiLCJzdWIiOiJhcGktc2hpcHgtcGwuZWFzeXBhY2syNC5uZXQiLCJleHAiOjE1MzY2NTcyNjcsImlhdCI6MTUzNjY1NzI2NywianRpIjoiNThkYWYxYmMtYTRmMi00NWU5LWJiMTUtMDJiMmRlZGM1NDdkIn0.lzdS690nlvXnkMzCqYSsKoNTNOyaYh2g4KSyylfTlcJJ0UMio9s2x8F8df5-KaWla_Gp_qxchG4quexYdcWaGA";

        string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJpc3MiOiJhcGktc2hpcHgtcGwuZWFzeXBhY2syNC5uZXQiLCJzdWIiOiJhcGktc2hpcHgtcGwuZWFzeXBhY2syNC5uZXQiLCJleHAiOjE2MDM4NjkzODQsImlhdCI6MTYwMzg2OTM4NCwianRpIjoiNzVhNjYzNjUtYmEyZS00MzA2LTgzMDAtZWQ5YTI0ZDQ2NzA5In0.kJXvx0NtTU0ALsiVHmA9xlKmbpRLwUCWUiY_r8wUlJl16ipP0Gm-4A2HJEYKFpP9dRSoNcCM5-sznPX3oLnbJw";
        int id = 25823;
        public class SendingMethodBatchId
        {
            public string SendingMethod { get; set; }
            public int Id { get; set; }
            public int? InpostBatchId  { get; set; }
        }
        public class SearchRootObject
        {
            public string href { get; set; }
            public int id { get; set; }
            public object tracking_number { get; set; } 
            public string external_customer_id { get; set; } 
        }


        #region Item
        public class ItemCustomAttributes
        {
            public string sending_method { get; set; }
            public string target_point { get; set; }
        }

        public class ItemCod
        {
            public double amount { get; set; }
            public string currency { get; set; }
        }

        public class ItemInsurance
        {
            public double amount { get; set; }
            public string currency { get; set; }
        }

        public class ItemAddress
        {
            public int id { get; set; }
            public string street { get; set; }
            public string building_number { get; set; }
            public object line1 { get; set; }
            public object line2 { get; set; }
            public string city { get; set; }
            public string post_code { get; set; }
            public string country_code { get; set; }
        }

        public class ItemSender
        {
            public int id { get; set; }
            public string name { get; set; }
            public string company_name { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public ItemAddress address { get; set; }
        }

        public class ItemReceiver
        {
            public int id { get; set; }
            public string name { get; set; }
            public string company_name { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string address { get; set; }
        }

        public class ItemCarrier
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class ItemService
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class ItemSelectedOffer
        {
            public int id { get; set; }
            public string status { get; set; }
            public object expires_at { get; set; }
            public object rate { get; set; }
            public string currency { get; set; }
            public List<object> additional_services { get; set; }
            public ItemCarrier carrier { get; set; }
            public ItemService service { get; set; }
            public object unavailability_reasons { get; set; }
        }

        public class ItemCarrier2
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class ItemService2
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class ItemOffer
        {
            public int id { get; set; }
            public string status { get; set; }
            public object expires_at { get; set; }
            public object rate { get; set; }
            public string currency { get; set; }
            public List<object> additional_services { get; set; }
            public ItemCarrier2 carrier { get; set; }
            public ItemService2 service { get; set; }
            public object unavailability_reasons { get; set; }
        }

        public class ItemDetails
        {
        }

        public class ItemTransaction
        {
            public int id { get; set; }
            public string status { get; set; }
            public int offer_id { get; set; }
            public ItemDetails details { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }

        public class ItemDimensions
        {
            public double length { get; set; }
            public double width { get; set; }
            public double height { get; set; }
            public string unit { get; set; }
        }

        public class ItemWeight
        {
            public double amount { get; set; }
            public string unit { get; set; }
        }

        public class ItemParcel
        {
            public int id { get; set; }
            public string tracking_number { get; set; }
            public bool is_non_standard { get; set; }
            public string template { get; set; }
            public ItemDimensions dimensions { get; set; }
            public ItemWeight weight { get; set; }
        }

        public class ItemItem
        {
            //public string href { get; set; }
            public int id { get; set; }
            //public string status { get; set; }
            public string tracking_number { get; set; }
            //public string service { get; set; }
            //public string reference { get; set; }
            //public bool is_return { get; set; }
            //public int application_id { get; set; }
            //public object created_by_id { get; set; }
            //public object external_customer_id { get; set; }
            //public string sending_method { get; set; }
            //public string mpk { get; set; }
            //public string comments { get; set; }
            //public List<object> additional_services { get; set; }
            //public ItemCustomAttributes custom_attributes { get; set; }
            //public ItemCod cod { get; set; }
            //public ItemInsurance insurance { get; set; }
            //public ItemSender sender { get; set; }
            //public ItemReceiver receiver { get; set; }
            //public ItemSelectedOffer selected_offer { get; set; }
            //public List<ItemOffer> offers { get; set; }
            //public List<ItemTransaction> transactions { get; set; }
            //public List<ItemParcel> parcels { get; set; }
            //public DateTime created_at { get; set; }
            //public DateTime updated_at { get; set; }
        }

        

        public class ItemRootObject
        {
            public string href { get; set; }
            public int count { get; set; }
            public int page { get; set; }
            public int per_page { get; set; }
            public List<ItemItem> items { get; set; }
        }
        #endregion

        public class OrderTracking
        {
            public int OrderId { get; set; }
            public int ShipmentId { get; set; }
            public string TrackingNumber { get; set; }
            public string ServiceType { get; set; }
            public string InpostServiceCode { get; set; }

        }
        public class Address
        {
            public string id { get; set; }
            public string street { get; set; }
            public string building_number { get; set; }
            public string city { get; set; }
            public string post_code { get; set; }
            public string country_code { get; set; }
        }

        public class Receiver
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public Address address { get; set; }
        }

        public class Sender
        {
//            company_name String  Atrybut nie jest wymagany, wymagalność pojawia się w momencie kiedy nie zostanie przekazany atrybut first_name oraz last_name
//email String  Atrybut nie jest wymagany, możliwość przekazania pustego atrybutu.
//phone String  Atrybut jest wymagany.
//address Address Form Atrybut jest wymagany.
//first_name  String Atrybut nie jest wymagany, wymagalność pojawia się w momencie kiedy nie zostanie przekazany atrybut company_name, first_name lub last_name
//last_name   String Atrybut nie jest wymagany, wymagalność pojawia się w momencie kiedy nie zostanie przekazany atrybut company_name, first_name lub last_name


            public string company_name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public Address address { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; } 
        }

        public class Dimensions
        {
            public string length { get; set; }
            public string width { get; set; }
            public string height { get; set; }
            public string unit { get; set; }
        }

        public class Weight
        {
            public string amount { get; set; }
            public string unit { get; set; }
        }

        public class Parcel
        {
            public string id { get; set; }
            public string template { get; set; }
            public Dimensions dimensions { get; set; }
            public Weight weight { get; set; }
            public object tracking_number { get; set; }
            public bool is_non_standard { get; set; }
        }

        public class CustomAttributes
        {
            public string target_point { get; set; }
            public string dropoff_point { get; set; }
            public string allegro_transaction_id { get; set; }
            public string allegro_user_id { get; set; }
            public string sending_method { get; set; } 
        }

        public class Insurance
        {
            public int amount { get; set; }
            public string currency { get; set; }
        }

        public class Cod
        {
            public double amount { get; set; }
            public string currency { get; set; }
        }

        public class RootObject
        {
            public string mpk { get; set; }
            public string comments { get; set; }
            public string external_customer_id { get; set; }
            public Receiver receiver { get; set; }
            public Sender sender{ get; set; }
            public List<Parcel> parcels { get; set; }
            public CustomAttributes custom_attributes { get; set; }
            public Insurance insurance { get; set; }
            public Cod cod { get; set; }
            public string reference { get; set; }
            public string service { get; set; }
            public bool only_choice_of_offer { get; set; }
            public List<string> additional_services { get; set; }
        }

        public class LabelRootObject
        {
            public string format { get; set; }
            public string type { get; set; }
            public string[] shipment_ids { get; set; }

        }
        #region ShipmentResponse
        public class ShipmentResponseCustomAttributes
        {
            public string target_point { get; set; }
        }

        public class ShipmentResponseCod
        {
            public object amount { get; set; }
            public object currency { get; set; }
        }

        public class ShipmentResponseInsurance
        {
            public object amount { get; set; }
            public object currency { get; set; }
        }

        public class ShipmentResponseAddress
        {
            public int id { get; set; }
            public string street { get; set; }
            public string building_number { get; set; }
            public object line1 { get; set; }
            public object line2 { get; set; }
            public string city { get; set; }
            public string post_code { get; set; }
            public string country_code { get; set; }
        }


        public class ShipmentResponseSender
        {
            public int id { get; set; }
            public object name { get; set; }
            public string company_name { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public ShipmentResponseAddress address { get; set; }
        }

        public class ShipmentResponseReceiver
        {
            public int id { get; set; }
            public object name { get; set; }
            public object company_name { get; set; }
            public object first_name { get; set; }
            public object last_name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public object address { get; set; }
        }

        public class ShipmentResponseDimensions
        {
            public double length { get; set; }
            public double width { get; set; }
            public double height { get; set; }
            public string unit { get; set; }
        }

        public class ShipmentResponseWeight
        {
            public double amount { get; set; }
            public string unit { get; set; }
        }

        public class ShipmentResponseParcel
        {
            public int id { get; set; }
            public object tracking_number { get; set; }
            public bool is_non_standard { get; set; }
            public string template { get; set; }
            public Dimensions dimensions { get; set; }
            public Weight weight { get; set; }
        }

        public class ShipmentResponseRootObject
        {
            public string href { get; set; }
            public int id { get; set; }
            public string status { get; set; }
            public object tracking_number { get; set; }
            public string service { get; set; }
            public object reference { get; set; }
            public bool is_return { get; set; }
            public int application_id { get; set; }
            public object created_by_id { get; set; }
            public object external_customer_id { get; set; }
            public object sending_method { get; set; }
            public object mpk { get; set; }
            public string comments { get; set; }
            public List<object> additional_services { get; set; }
            public ShipmentResponseCustomAttributes custom_attributes { get; set; }
            public ShipmentResponseCod cod { get; set; }
            public object insurance { get; set; }
            public ShipmentResponseSender sender { get; set; }
            public ShipmentResponseReceiver receiver { get; set; }
            public object selected_offer { get; set; }
            public List<object> offers { get; set; }
            public List<object> transactions { get; set; }
            public List<ShipmentResponseParcel> parcels { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }
        #endregion
  
    
        #region Batch
        public class BatchCustomAttributes
        {
            public string target_point { get; set; }
            public string dropoff_point { get; set; }
            public string allegro_transaction_id { get; set; }
            public string allegro_user_id { get; set; }
            public string sending_method { get; set; }
        }

        public class BatchParcels
        {
            public string template { get; set; }
        }

        public class BatchAddress
        {
            public string line1 { get; set; }
            public string city { get; set; }
            public string post_code { get; set; }
            public string country_code { get; set; }
        }

        public class BatchReceiver
        {
            public string company_name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public BatchAddress address { get; set; }
        }

        public class BatchAddress2
        {
            public string line1 { get; set; }
            public string city { get; set; }
            public string post_code { get; set; }
            public string country_code { get; set; }
        }

        public class BatchSender
        {
            public string company_name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public BatchAddress2 address { get; set; }
        }

        public class BatchCod
        {
            public double amount { get; set; }
            public string currency { get; set; }
        }

        public class BatchInsurance
        {
            public int amount { get; set; }
            public string currency { get; set; }
        }

        public class BatchShipment
        {
            public int id { get; set; }
            public string service { get; set; }
            public string external_customer_id { get; set; }
            public BatchCustomAttributes custom_attributes { get; set; }
            public BatchParcels parcels { get; set; }
            public BatchReceiver receiver { get; set; }
            public BatchSender sender { get; set; }
            public BatchCod cod { get; set; }
            public BatchInsurance insurance { get; set; }
        }

        public class BatchRootObject
        {
            public bool only_choice_of_offer { get; set; }
            public List<BatchShipment> shipments { get; set; }
        }
        #endregion
        #region BatchResponse
        public class BatchResponseShipment
        {
            public string href { get; set; }
            public int id { get; set; }
            public string tracking_number { get; set; }
        }

        public class BatchResponseRootObject
        {
            public string href { get; set; }
            public int id { get; set; }
            public string status { get; set; }
            public List<BatchResponseShipment> shipments { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }
        #endregion

        //public string GetToken()
        //{
        //    Dal.AllegroScan asc = new Dal.AllegroScan();

        //    WebRequest request = WebRequest.Create(String.Format("https://api-shipx-pl.easypack24.net/v1/tracking/692321747893490111722023"));

        //    request.Method = "GET";
        //    request.ContentType = "application/json";

        //    //using (SHA256 sha256 = new SHA256Managed())
        //    //{


        //    //    string clcs = Base64Encode(String.Format("{0}:{1}", au.ClientId, au.ClientSecret));
        //    request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", token));

        //    WebResponse webResponse = request.GetResponse();
        //    Stream responseStream = webResponse.GetResponseStream();
        //    StreamReader reader = new StreamReader(responseStream);
        //    string text = reader.ReadToEnd();

        //    //Object response = FromJson(text);


        //    //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    //    serializer.MaxJsonLength = Int32.MaxValue;
        //    //    TokenReturnObject token = serializer.Deserialize<TokenReturnObject>(text);

        //    //    Dal.AllegroUser allegroUser = new Dal.AllegroUser()
        //    //    {
        //    //        UserId = userId,
        //    //        Token = token.access_token,
        //    //        TokenRefresh = token.refresh_token,
        //    //        TokenCreateDate = DateTime.Now,
        //    //        TokenEndDate = DateTime.Now.AddSeconds(token.expires_in)
        //    //    };
        //    //    asc.SetAllegroUserUpdateToken(allegroUser);

        //    //}
        //    return "";
        //}
        //public string Tracking()
        //{
        //    Dal.AllegroScan asc = new Dal.AllegroScan();

        //    WebRequest request = WebRequest.Create(String.Format("https://api-shipx-pl.easypack24.net/v1/tracking/692321747893490111722023"));

        //    request.Method = "GET";
        //    request.ContentType = "application/json";

        //    WebResponse webResponse = request.GetResponse();
        //    Stream responseStream = webResponse.GetResponseStream();
        //    StreamReader reader = new StreamReader(responseStream);
        //    string text = reader.ReadToEnd();


        //    return text;
        //}


        /// <summary>
        /// Tworzy przesyłkę
        /// </summary>
        /// <param name="seviceCode"></param>
        /// <param name="order"></param>
        /// <returns>Zwraca ShipmentId</returns>
        //public OrderTracking Shipment(string serviceCode, Dal.OrderInpostView order)
        //{
        //    Dal.AllegroScan asc = new Dal.AllegroScan();

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api-shipx-pl.easypack24.net/v1/organizations/{0}/shipments",id));

        //    request.Method = "POST";
        //    request.ContentType = "application/json";
        //    request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));
        //    request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");

        //    #region Parcel
        //    List<Parcel> parcels = new List<Parcel>();
        //    Parcel parcel = new Parcel()
        //    {
        //        id = "1",
        //        is_non_standard = false,
        //        template = order.InpostSizeType,
        //        tracking_number = null
        //    };
        //    parcels.Add(parcel);

        //    string phone = new string(order.Phone.Where(char.IsDigit).ToArray());
        //    phone = phone.Substring(phone.Length - 9, 9);

        //    Cod cod = null;

        //    if (order.InpostCod.HasValue)
        //        cod = new Cod()
        //        {
        //            amount = (double)order.InpostCod.Value,
        //            currency = "PLN"
        //        };
        //    CustomAttributes ca = new CustomAttributes()
        //    {
        //        target_point = order.InpostLockerCode,
        //        sending_method = GetServiceMethod(serviceCode) ,
        //        dropoff_point = String.IsNullOrEmpty(serviceCode) ? null : serviceCode,
        //    };
        //    //if (order.BuyFormId.HasValue)
        //    //{
        //    //    ca.allegro_transaction_id = order.BuyFormId.ToString();
        //    //}
        //    if (order.SellerUserId.HasValue)
        //    {
        //        ca.allegro_user_id = order.SellerUserId.ToString();
        //    }
        //    RootObject ro = new RootObject()
        //    {
        //        sender = new Sender()
        //        {
        //            company_name = "M-Form sp. z o.o.",
        //            address = new Address()
        //            {
        //                city = "Łódź",
        //                country_code = "PL",
        //                street = "Przewodnia",
        //                building_number = "16",
        //                post_code = "93-419"

        //            },
        //            email = Dal.Helper.MyEmail,
        //            phone = "600732000"
        //        },
        //        additional_services = null,
        //        cod = cod,
        //        comments = "komentarz",
        //        custom_attributes = ca,
        //        external_customer_id = null,
        //        insurance = null,
        //        mpk = null,
        //        receiver = new Receiver()
        //        {
        //            phone = phone,
        //            email = order.Email
        //        },
        //        service = order.InpostServiceType,
        //        parcels = parcels,
        //        reference = order.OrderId.ToString(),
        //        only_choice_of_offer = false


        //    };
        //    #endregion

        //   // ErrorHandler.SendEmail(ToJson(ro));
        //    Stream dataStream = request.GetRequestStream();
        //    string jsonEncodedParams = jsonEncodedParams = Bll.RESTHelper.ToJson(ro);
        //    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


        //    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();
        //    string text = "";
        //    try
        //    {
        //        HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();

        //        Stream responseStream = webResponse.GetResponseStream();

        //        StreamReader reader = new StreamReader(responseStream);

        //        text = reader.ReadToEnd();

        //        var json_serializer = new JavaScriptSerializer();
        //        ShipmentResponseRootObject shipment = json_serializer.Deserialize<ShipmentResponseRootObject>(text);




        //        OrderTracking ot = new OrderTracking()
        //        {
        //            ShipmentId = shipment.id,
        //            OrderId = order.OrderId,
        //            ServiceType = shipment.service,
        //            TrackingNumber = null,
        //            InpostServiceCode = serviceCode
        //        };
        //        Dal.OrderHelper oh = new Dal.OrderHelper();
        //        oh.SetOrderInpostShipmentId(order.OrderId, shipment.id.ToString(), serviceCode);

        //        return ot;
        //    }
        //    catch (WebException e)
        //    {
        //        using (WebResponse response = e.Response)
        //        {
        //            HttpWebResponse httpResponse = (HttpWebResponse)response;
        //            Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
        //            using (Stream data = response.GetResponseStream())
        //            using (var reader = new StreamReader(data))
        //            {
        //                string t = reader.ReadToEnd();
        //                ErrorHandler.SendError(e, String.Format("OrderId: {0}, {1}", order.OrderId, t));
        //                Console.WriteLine(t);
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Bll.ErrorHandler.SendEmail(text);
        //    }

        //    return null;

        //}

        public OrderTracking Shipment(string serviceCode, Dal.OrderInpostView order, Dal.OrderShipping os)
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api-shipx-pl.easypack24.net/v1/organizations/{0}/shipments", id));

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");

            #region Parcel
            List<Parcel> parcels = new List<Parcel>();
            List<Dal.OrderShippingParcel> orderShippingParcel = Dal.DbHelper.Orders.GetOrderShippingParcels(os.Id);

            int i = 1;
            foreach (Dal.OrderShippingParcel p in orderShippingParcel)
            {
                Parcel parcel = new Parcel()
                {
                    id = i.ToString(),
                    is_non_standard = false,
                    template = GetSize(p.Size),
                    tracking_number = null
                };
                parcels.Add(parcel);
                i += 1;
            }
            string phone = new string(order.Phone.Where(char.IsDigit).ToArray());
            if (phone.Length >= 9)
                phone = phone.Substring(phone.Length - 9, 9);

            Insurance insurance = null;
            Cod cod = null;



            if (order.InpostCod.HasValue)
            {
                cod = new Cod()
                {
                    amount = (double)order.InpostCod.Value,
                    currency = "PLN"
                };
                insurance = new Insurance()
                {
                    amount = ((int)order.InpostCod.Value<200?200: (int)order.InpostCod.Value) + 1,
                    currency = "PLN"
                };
            }
            CustomAttributes ca = new CustomAttributes()
            {
                target_point = order.InpostLockerCode,
                sending_method = GetServiceMethod(serviceCode),
                dropoff_point = String.IsNullOrEmpty(serviceCode) ? null : serviceCode,
            };
            //if (order.BuyFormId.HasValue)
            //{
            //    ca.allegro_transaction_id = order.BuyFormId.ToString();
            //}
            if (order.SellerUserId.HasValue)
            {
                ca.allegro_user_id = order.SellerUserId.ToString();
            }
            RootObject ro = new RootObject()
            {
                sender = new Sender()
                {
                    company_name = "M-Form sp. z o.o.",
                    address = new Address()
                    {
                        city = "Łódź",
                        country_code = "PL",
                        street = "Przewodnia",
                        building_number = "16",
                        post_code = "93-419"

                    },
                    email = Dal.Helper.MyEmail,
                    phone = "600732000"
                },
                additional_services = null,
                cod = cod,
              
                comments = "",
                custom_attributes = ca,
                external_customer_id = null,
                insurance =insurance,
                mpk = null,
                receiver = new Receiver()
                {
                    phone = phone,
                    email = order.Email,
       
                },
                service = order.InpostServiceType,
                parcels = parcels,
                reference = order.OrderId.ToString(),
                only_choice_of_offer = false


            };

            if (String.IsNullOrEmpty(order.InpostLockerCode))
            {
                ro.receiver.address = new Address()
                {
                    city = order.ShipmentCity,
                    post_code = order.ShipmentPostcode,
                    country_code = "PL",
                    building_number = ".",
                    street = order.ShipmentAddress,

                };
                ro.receiver.first_name = order.ShipmentFirstName;
                ro.receiver.last_name = order.ShipmentLastName;
                ro.receiver.name = order.ShipmentCompanyName;
            }
            #endregion

            // ErrorHandler.SendEmail(ToJson(ro));
            Stream dataStream = request.GetRequestStream();
            string jsonEncodedParams = jsonEncodedParams = Bll.RESTHelper.ToJson(ro);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


            byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            string text = "";
            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();

                Stream responseStream = webResponse.GetResponseStream();

                StreamReader reader = new StreamReader(responseStream);

                text = reader.ReadToEnd();

                var json_serializer = new JavaScriptSerializer();
                ShipmentResponseRootObject shipment = json_serializer.Deserialize<ShipmentResponseRootObject>(text);




                OrderTracking ot = new OrderTracking()
                {
                    ShipmentId = shipment.id,
                    OrderId = order.OrderId,
                    ServiceType = shipment.service,
                    TrackingNumber = null,
                    InpostServiceCode = serviceCode
                };
               // Dal.OrderHelper oh = new Dal.OrderHelper();
              //  oh.SetOrderInpostShipmentId(order.OrderId, shipment.id.ToString(), serviceCode);

                Dal.DbHelper.Orders.SetOrderShippingInternalId(os.Id, shipment.id.ToString());
                return ot;
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string t = reader.ReadToEnd();
                        ErrorHandler.SendError(e, String.Format("OrderId: {0}, {1}", order.OrderId, t));
                        Console.WriteLine(t);
                    }
                }
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendEmail(text);
            }

            return null;

        }

        private string GetSize(int? size)
        {
            switch(size)
            {
                case 1: return "small";
                case 2: return "medium";
                case 3: return "large";
                default: return "large";

            }
            return "A";
        }

        public void GetLabels(Dal.OrderInpostView ots, string trackingNumber, string actingUser)
        {

            string serviceType = ots.InpostServiceType;



            string ids = ots.InpostShipmentId;

            Dal.AllegroScan asc = new Dal.AllegroScan();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api-shipx-pl.easypack24.net/v1/organizations/{0}/shipments/labels",id));

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");

            #region Parcel

            LabelRootObject ro = new LabelRootObject()
            {
                format = "Pdf",
                shipment_ids = new string[] { ids },
                type = "A6"
            };
            #endregion

            Stream dataStream = request.GetRequestStream();
            string jsonEncodedParams  = Bll.RESTHelper.ToJson(ro);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(jsonEncodedParams);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            string saveLocation = null;
            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                saveLocation = CreatePdf(responseStream, trackingNumber);


            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                        ErrorHandler.SendError(new Exception("GetLabels"), text);
                    }
                }
            }


            //string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
            //path = String.Format(path, @"Shipping\Inpost\");
            ////DirectoryInfo di = new DirectoryInfo(path);



            ////string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
            //string fn = String.Format(@"{0}.pdf", trackingNumber);

            //string[] files =
            //   serviceFile.Select(x => x.Value).Distinct().ToArray();

            //if(files.Length==0)
            //{


            //    ErrorHandler.SendEmail("Brak plików z etykietami do wykonania operacji Merge");
            //    return 0;
            //}
            //Bll.PDF.MergeFiles(files,
            //String.Format(path, ""),
            //String.Format(path, fn),
            //iTextSharp.text.PageSize.A4);

            //Dal.OrderHelper oh = new Dal.OrderHelper();

            //int[] ordersInserted = ots.Select(x => x.OrderId).ToArray();
            //int batchId = oh.SetOrderExportBatch(Dal.Helper.ShippingCompany.InPost,
            //  ordersInserted,
            //  "Export automatyczny",
            //  actingUser,
            //  fn);
            //return batchId;


            //var json_serializer = new JavaScriptSerializer();
            //PromotionCreatedRootObject promotion = json_serializer.Deserialize<PromotionCreatedRootObject>(text);

        }

        //public bool ExportInpostBatch(string service_code, Dal.Order order, string actingUser)
        //{ 
        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    List<BatchShipment> shipments = new List<BatchShipment>();

        //    Dal.OrderInpostView o = oh.GetOrdersForInpost(order.OrderId);

        //    OrderTracking ot;

        //    //if(service_code==null)
        //    //{
        //    //    if (o.InpostServiceCode == null)
        //    //    {
        //    //        Dal.SettingsHelper sh = new Dal.SettingsHelper();
        //    //        Dal.Settings s = sh.GetSetting("INPOST_DEF");
        //    //        service_code = s.StringValue;
        //    //    }
        //    //    else
        //    //        service_code = o.InpostServiceCode;

        //    //}


        //    if (o.InpostShipmentId==null)
        //        ot = Shipment(service_code, o);
        //    else
        //    {
        //        ot = new OrderTracking()
        //        {
        //            OrderId = o.OrderId,
        //            ServiceType = o.InpostServiceType,
        //            ShipmentId = Int32.Parse(o.InpostShipmentId)
        //        };
        //    }
        //    SetTrackingNumber(ot, null);


        //    o = oh.GetOrdersForInpost(order.OrderId);

        //    if(o.ShipmentTrackingNumber!=null)
        //        Label(o.InpostShipmentId, o.ShipmentTrackingNumber);

           


        //    return true;

        //}
        public bool ExportInpostBatch(string service_code, Dal.OrderShipping order, string actingUser)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<BatchShipment> shipments = new List<BatchShipment>();

            Dal.OrderInpostView o = oh.GetOrdersForInpost(order.Id, order.OrderId);

            if (String.IsNullOrEmpty(o.Email))
                return false;


            OrderTracking ot;

            //if(service_code==null)
            //{
            //    if (o.InpostServiceCode == null)
            //    {
            //        Dal.SettingsHelper sh = new Dal.SettingsHelper();
            //        Dal.Settings s = sh.GetSetting("INPOST_DEF");
            //        service_code = s.StringValue;
            //    }
            //    else
            //        service_code = o.InpostServiceCode;

            //}


            //List<Dal.OrderShippingParcel> parcels = Dal.DbHelper.Orders.GetOrderShippingParcels(order.Id);

            //foreach (Dal.OrderShippingParcel parcel in parcels)
            {
                if (o.InpostShipmentId == null)
                    ot = Shipment(service_code, o, order);
                else
                {
                    ot = new OrderTracking()
                    {
                        OrderId = o.OrderId,
                        ServiceType = o.InpostServiceType,
                        ShipmentId = Int32.Parse(o.InpostShipmentId)
                    };
                }
                SetTrackingNumber(ot, order);


                o = oh.GetOrdersForInpost(order.Id, order.OrderId);

                if (o.ShipmentTrackingNumber != null)
                    Label(o.InpostShipmentId, o.ShipmentTrackingNumber);

            }


            return true;

        }
        private void SetTrackingNumber(OrderTracking ot, Dal.OrderShipping orderShipping)
        {
            try
            {
                Dal.AllegroScan asc = new Dal.AllegroScan();


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api-shipx-pl.easypack24.net/v1/organizations/{1}/shipments?id={0}",ot.ShipmentId,id));


                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));

                request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");

                Dal.OrderHelper oh = new Dal.OrderHelper();



                WebResponse webResponse = request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();

             

                var json_serializer = new JavaScriptSerializer();
                ItemRootObject itemRootObject = json_serializer.Deserialize<ItemRootObject>(text);
                if (itemRootObject != null)
                {
                    //ErrorHandler.SendEmail("SetOrderTrackingNumber itemRootObject != null" + text);
                    foreach (ItemItem item in itemRootObject.items)
                    {
                        //int orderId = ot.OrderId;
                        //oh.SetOrderTrackingNumber(orderId, item.tracking_number, item.id.ToString());

                        if (orderShipping != null)
                            Dal.DbHelper.Orders.SetOrderShippingTrackingNumber(orderShipping.Id, item.tracking_number);
                        //ErrorHandler.SendEmail("SetOrderTrackingNumber" + text);
                    }
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                        ErrorHandler.SendError(e, "SetTrackingNumbers" + text);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandler.SendError(e, "SetTrackingNumbers");
            }

        }
        //private void SetBatchResponseRootObject(BatchResponseRootObject brro, SendingMethodBatchId sm, string actingUser)
        //{
        //    string status = "generated";
        //    if (brro.shipments.Where(x => x.tracking_number == null || x.tracking_number == "").Count() > 0)
        //        status = "in_progress";

        //    Dal.OrderInpostBatch oib = new Dal.OrderInpostBatch()
        //    {
        //        BatchStatus = status,
        //        Id = brro.id,
        //        InsertDate = DateTime.Now,
        //        InsertUser = actingUser,
        //        SendingMethod = sm.SendingMethod
        //    };
        //    List<Dal.OrderInpost> ordersInpost = new List<Dal.OrderInpost>();

        //    foreach (BatchResponseShipment brs in brro.shipments)
        //    {

        //        SearchRootObject sro =   Search( brs.id);
        //        Dal.OrderInpost oi = new Dal.OrderInpost()
        //        {
        //            OrderInpostBatch = oib,
        //            Id = brs.id,
        //            OrderId = Convert.ToInt32(sro.external_customer_id),
        //            ShipmentId = brs.id.ToString(),
        //            TrackingNumber = brs.tracking_number,
        //            BatchStatus = brro.status
        //        };
        //        ordersInpost.Add(oi);
        //    }

        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    oh.SetOrderInpostBatch(ordersInpost, oib, sm.InpostBatchId);

        //    Dal.OrderInpostView oiv = oh.GetOrdersForInpost(ordersInpost[0].OrderId);//.Select(x => x.OrderId).ToArray());

        //    GetLabels(oiv, ordersInpost[0].TrackingNumber, actingUser);
        //}

        //private BatchResponseRootObject GetBatchResponseObject(BatchRootObject batch)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api-shipx-pl.easypack24.net/v1/organizations/{0}/batches",id));

        //    request.Method = "POST";
        //    request.ContentType = "application/json";
        //    request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));
        //    request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");



        //    Stream dataStream = request.GetRequestStream();
        //    string jsonEncodedParams =  ToJson(batch);
        //    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        //    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    try
        //    {
        //        HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
        //        Stream responseStream = webResponse.GetResponseStream(); 
        //        StreamReader reader = new StreamReader(responseStream);
        //        string text = reader.ReadToEnd();

        //        ErrorHandler.SendEmail(text);

        //        var json_serializer = new JavaScriptSerializer();
        //        BatchResponseRootObject brro = json_serializer.Deserialize<BatchResponseRootObject>(text);

        //        return brro;
        //    }
        //    catch (WebException e)
        //    {
        //        using (WebResponse response = e.Response)
        //        {
        //            HttpWebResponse httpResponse = (HttpWebResponse)response;
        //            Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
        //            using (Stream data = response.GetResponseStream())
        //            using (var reader = new StreamReader(data))
        //            {
        //                string text = reader.ReadToEnd();
        //                Console.WriteLine(text);
        //                ErrorHandler.SendError(new Exception("GetBatchResponseObject (BatchRootObject)"), jsonEncodedParams);
        //                ErrorHandler.SendError(new Exception("GetBatchResponseObject (BatchRootObject batch)"), text);
        //            }
        //        }
        //        return null;
        //    }
        //}
        //private void SetBatchResponseObject(object o)
        //{
        //    SendingMethodBatchId sm = o as SendingMethodBatchId;
        //    BatchResponseRootObject brro = GetBatchResponseObject(sm.Id);

        //    SetBatchResponseRootObject(brro, sm, "");
        //}
        //private BatchResponseRootObject GetBatchResponseObject(int batchId)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api-shipx-pl.easypack24.net/v1/batches/{0}", batchId));

        //    request.Method = "GET";
        //    request.ContentType = "application/json";
        //    request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));
        //    request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");


        //    try
        //    {
        //        HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
        //        Stream responseStream = webResponse.GetResponseStream();
        //        StreamReader reader = new StreamReader(responseStream);
        //        string text = reader.ReadToEnd();

        //        ErrorHandler.SendEmail(text);

        //        var json_serializer = new JavaScriptSerializer();
        //        BatchResponseRootObject brro = json_serializer.Deserialize<BatchResponseRootObject>(text);


        //        return brro;
        //    }
        //    catch (WebException e)
        //    {
        //        using (WebResponse response = e.Response)
        //        {
        //            HttpWebResponse httpResponse = (HttpWebResponse)response;
        //            Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
        //            using (Stream data = response.GetResponseStream())
        //            using (var reader = new StreamReader(data))
        //            {
        //                string text = reader.ReadToEnd();
        //                Console.WriteLine(text);
        //                ErrorHandler.SendError(new Exception("GetBatchResponseObject (int batchId)"), text);
        //            }
        //        }
        //        return null;
        //    }
        //}

        //private BatchShipment GetBatchShipment(Dal.OrderInpostView order, string seviceCode)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    BatchParcels parcel = new BatchParcels()
        //    { 
        //        template = order.InpostSizeType 
        //    };

        //    string phone = new string(order.Phone.Where(char.IsDigit).ToArray());
        //    phone = phone.Substring(phone.Length - 9, 9);

        //    BatchCod cod = null;

        //    if (order.InpostCod.HasValue)
        //        cod = new BatchCod()
        //        {
        //            amount = (double)order.InpostCod.Value,
        //            currency = "PLN"
        //        };
        //    BatchCustomAttributes ca = new BatchCustomAttributes()
        //    {
        //        target_point = order.InpostLockerCode,
        //        sending_method = GetServiceMethod(seviceCode),
        //        dropoff_point = String.IsNullOrEmpty(seviceCode) ? null : seviceCode,
        //    };
        //    if (order.BuyFormId.HasValue)
        //    {
        //        ca.allegro_transaction_id = order.BuyFormId.ToString();
        //    }
        //    if (order.SellerUserId.HasValue)
        //    {
        //        ca.allegro_user_id = order.SellerUserId.ToString();
        //    }
        //    BatchShipment ro = new BatchShipment()
        //    {
        //        cod = cod,
        //        custom_attributes = ca,
        //        insurance = null,
        //        receiver = new BatchReceiver()
        //        {
        //            phone = phone,
        //            email = order.Email
        //        },
        //        service = order.InpostServiceType,
        //        parcels = parcel,
        //        sender = new BatchSender()
        //        {
        //            address = new BatchAddress2()
        //            {
        //                city = order.CompanyCity,
        //                country_code = "PL",
        //                line1 = String.Format("{0} {1}", order.CompanyAddress, order.CompanyAddressNo),
        //                post_code = order.CompanyPostalCode

        //            },
        //            email = Dal.Helper.MyEmail,
        //            phone = "604688227"
        //        },
        //        id = order.OrderId,
        //        external_customer_id = order.OrderId.ToString()
        //    };
        //    return ro;
        //}

        private string GetServiceMethod(string seviceCode)
        {
            return String.IsNullOrEmpty(seviceCode) ? "dispatch_order" : "parcel_locker";
        }

        public SearchRootObject Search(int id)
        {
          
            HttpWebRequest request = (HttpWebRequest)WebRequest
                .Create(String.Format(
                    String.Format("https://api-shipx-pl.easypack24.net/v1/shipments/{0}", id)));

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");


            WebResponse webResponse = request.GetResponse();
            Stream responseStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string text = reader.ReadToEnd();


            var json_serializer = new JavaScriptSerializer();
            SearchRootObject sro = json_serializer.Deserialize<SearchRootObject>(text);

            

            return sro;
        }
        public string Search(string query)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest
                .Create(String.Format(
                    String.Format("https://api-shipx-pl.easypack24.net/v1/organizations/{1}/shipments?{0}", query,id)));

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");


            WebResponse webResponse = request.GetResponse();
            Stream responseStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string text = reader.ReadToEnd();


            return text;
        }






        private static string CreatePdf(Stream responseStream, string trackingNumber)
        {
            try
            {

                string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
                path = String.Format(path, @"Shipping\Inpost\{0}");

                string fileName = String.Format(@"{0}.pdf", trackingNumber);
                string saveLocation = String.Format(path, fileName);
                DirectoryInfo di = new DirectoryInfo(path);
                File.WriteAllBytes(saveLocation, Bll.Helper.StreamToByteArray(responseStream));

                return saveLocation;
            }
            catch (Exception ex)
            {
                ErrorHandler.SendError(ex, "CreatePdf");
                return null;
            }
        }
      
        
        internal void GetTracking()
        {
            //Dal.OrderHelper oh = new Dal.OrderHelper();
            //Dal.OrderInpostBatch oib = oh.GetOrderInpostBatch();

            //if (oib != null)
            //{
            //    SendingMethodBatchId sm = new SendingMethodBatchId();
            //    sm.Id = oib.Id;
            //    sm.SendingMethod = oib.SendingMethod;
            //    sm.InpostBatchId = oib.InpostBatchId;
            //    SetBatchResponseObject(sm);
            //}
        }



        /// <summary>
        /// Pobiera wiele etykiet na raz
        /// </summary>
        /// <param name="ids"></param>
        //public int GetLabels(List<OrderTracking> ots, string actingUser)
        //{
        //    System.Threading.Thread.Sleep(10000);
        //    SetTrackingNumbers(ots);


        //    System.Threading.Thread.Sleep(2000);

        //    string[] serviceTypes = ots.Select(x => x.ServiceType).Distinct().ToArray();

        //    Dictionary<string, string> serviceFile = new Dictionary<string, string>();

        //    foreach (string serviceType in serviceTypes)
        //    {
        //        string[] ids = ots.Where(x => x.ServiceType == serviceType).Select(x => x.ShipmentId.ToString()).ToArray();

        //        Dal.AllegroScan asc = new Dal.AllegroScan();

        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api-shipx-pl.easypack24.net/v1/organizations/{0}/shipments/labels",id));

        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));
        //        request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");

        //        #region Parcel

        //        LabelRootObject ro = new LabelRootObject()
        //        {
        //            format = "Pdf",
        //            shipment_ids = ids,
        //            type = "A6"
        //        };
        //        #endregion

        //        Stream dataStream = request.GetRequestStream();
        //        string jsonEncodedParams = jsonEncodedParams = ToJson(ro);
        //        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        //        byte[] byteArray = encoding.GetBytes(jsonEncodedParams);
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //        dataStream.Close();

        //        string fileName = null;
        //        try
        //        {
        //            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
        //            Stream responseStream = webResponse.GetResponseStream();
        //            fileName = CreatePdf(responseStream);

        //            //StreamReader reader = new StreamReader(responseStream);
        //            //string text = reader.ReadToEnd();
        //            serviceFile.Add(serviceType, fileName);

        //        }
        //        catch (WebException e)
        //        {
        //            using (WebResponse response = e.Response)
        //            {
        //                HttpWebResponse httpResponse = (HttpWebResponse)response;
        //                Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
        //                using (Stream data = response.GetResponseStream())
        //                using (var reader = new StreamReader(data))
        //                {
        //                    string text = reader.ReadToEnd();
        //                    Console.WriteLine(text);
        //                    ErrorHandler.SendError(new Exception("Create Pdf"), text);
        //                }
        //            }
        //        }

        //    }

        //    string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
        //    string fn = String.Format(@"Inpost_{0:yyyyMMddHHmmss}.pdf", DateTime.Now);

        //    Bll.PDF.MergeFiles(
        //       serviceFile.Select(x => x.Value).Distinct().ToArray(),
        //    String.Format(path, ""),
        //    String.Format(path, fn),
        //    iTextSharp.text.PageSize.A6);

        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    int[] ordersInserted = ots.Select(x => x.OrderId).ToArray();
        //    int batchId = oh.SetOrdersStatus(Dal.Helper.ShippingCompany.InPost,
        //      ordersInserted,
        //      Dal.Helper.OrderStatus.Exported,
        //      "Export automatyczny",
        //      actingUser,
        //      fn);
        //    return batchId;


        //    //var json_serializer = new JavaScriptSerializer();
        //    //PromotionCreatedRootObject promotion = json_serializer.Deserialize<PromotionCreatedRootObject>(text);

        //}
        //private static byte[] StrToByteArray(string str)
        //{
        //    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        //    return encoding.GetBytes(str);
        //}
        //public string Organization()
        //{
        //    WebRequest request = WebRequest.Create(String.Format("https://api-shipx-pl.easypack24.net/v1/organizations"));

        //    request.Method = "GET";
        //    request.ContentType = "application/json";

        //    request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));

        //    WebResponse webResponse = request.GetResponse();
        //    Stream responseStream = webResponse.GetResponseStream();
        //    StreamReader reader = new StreamReader(responseStream);
        //    string text = reader.ReadToEnd();

        //    //Object response = FromJson(text);


        //    //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    //    serializer.MaxJsonLength = Int32.MaxValue;
        //    //    TokenReturnObject token = serializer.Deserialize<TokenReturnObject>(text);

        //    //    Dal.AllegroUser allegroUser = new Dal.AllegroUser()
        //    //    {
        //    //        UserId = userId,
        //    //        Token = token.access_token,
        //    //        TokenRefresh = token.refresh_token,
        //    //        TokenCreateDate = DateTime.Now,
        //    //        TokenEndDate = DateTime.Now.AddSeconds(token.expires_in)
        //    //    };
        //    //    asc.SetAllegroUserUpdateToken(allegroUser);

        //    //}
        //    return "";
        //}
        //public int ExportInpost(string service_code,int[] orderIds, string actingUser, out int batchId)
        //{
        //    List<OrderTracking> ids = new List<OrderTracking>();

        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    List<Dal.OrderInpostView> orders = oh.GetOrdersForInpost() 
        //        .ToList();
        //    if (orderIds != null)
        //        orders = orders.Where(x => orderIds.Contains(x.OrderId)).ToList();

        //    foreach (Dal.OrderInpostView order in orders)
        //    {
        //        OrderTracking ot = Shipment(service_code, order);
        //        ids.Add(ot);
        //    }
        //    //ids.Clear();
        //    //ids.Add(new OrderTracking() { OrderId = 71130, ShipmentId = Convert.ToInt32("140731571"), TrackingNumber = null, ServiceType = "inpost_locker_allegro" });
        //    //ids.Add(new OrderTracking() { OrderId = 71111, ShipmentId = Convert.ToInt32("140714162"), TrackingNumber = null, ServiceType = "inpost_locker_allegro" });
        //    //ids.Add(new OrderTracking() { OrderId = 71120, ShipmentId = Convert.ToInt32("140714169"), TrackingNumber = null, ServiceType = "inpost_locker_allegro" });
        //    //ids.Add(new OrderTracking() { OrderId = 70948, ShipmentId = Convert.ToInt32("140714159"), TrackingNumber = null, ServiceType = "inpost_locker_standard" });
        //    //ids.Add(new OrderTracking() { OrderId = 70998, ShipmentId = Convert.ToInt32("140714160"), TrackingNumber = null, ServiceType = "inpost_locker_standard" });

        //    //if (orderIds != null)
        //    //    ids = ids.Where(x => orderIds.Contains(x.OrderId)).ToList();

        //    batchId = GetLabels(ids, actingUser);

        //    return ids.Count();

        //}


        //public void GetSingleLabel(Dal.Order order)
        //{

        //    string shipment = order.InpostShipmentId;


        //    string fn = Label(shipment);
        //    if(fn!=null)
        //    { 
        //    string fileName = String.Format(@"{0}\{1}", HttpContext.Current.Server.MapPath("/Files/ExportFiles"), fn);


        //    string contentType = "Application/pdf";




        //    HttpContext.Current. Response.ContentType = contentType;
        //    HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + fn);

        //    //Write the file directly to the HTTP content output stream.
        //    HttpContext.Current.Response.WriteFile(fileName);
        //    HttpContext.Current.Response.End();

        //    }
        //}


        /// <summary>
        /// Pobiera pojedynczą etykietę
        /// </summary>
        /// <param name="ids"></param>
        public string Label(string shipment, string trackingNumber)
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api-shipx-pl.easypack24.net/v1/shipments/{0}/label?format=pdf&type=A6", shipment));

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));

            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");

            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                return CreatePdf(responseStream, trackingNumber);

            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                    }
                }
                return null;
            }

            //var json_serializer = new JavaScriptSerializer();
            //PromotionCreatedRootObject promotion = json_serializer.Deserialize<PromotionCreatedRootObject>(text);

        }
    }
}