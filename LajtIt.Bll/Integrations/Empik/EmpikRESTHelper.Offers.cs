using LajtIt.Dal;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class EmpikRESTHelper
    {
        public partial class Offers
        {
            #region Clesses
            public class OffersList
            {
                // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
                public class VolumePrice
                {
                    public double price { get; set; }
                    public int quantity_threshold { get; set; }
                    public object unit_discount_price { get; set; }
                    public double unit_origin_price { get; set; }
                }

                public class AllPrice
                {
                    public object channel_code { get; set; }
                    public object discount_end_date { get; set; }
                    public object discount_start_date { get; set; }
                    public double price { get; set; }
                    public object unit_discount_price { get; set; }
                    public double unit_origin_price { get; set; }
                    public List<VolumePrice> volume_prices { get; set; }
                }

                public class VolumePrice2
                {
                    public double price { get; set; }
                    public int quantity_threshold { get; set; }
                    public object unit_discount_price { get; set; }
                    public double unit_origin_price { get; set; }
                }

                public class ApplicablePricing
                {
                    public object channel_code { get; set; }
                    public object discount_end_date { get; set; }
                    public object discount_start_date { get; set; }
                    public double price { get; set; }
                    public object unit_discount_price { get; set; }
                    public double unit_origin_price { get; set; }
                    public List<VolumePrice2> volume_prices { get; set; }
                }

                public class LogisticClass
                {
                    public string code { get; set; }
                    public string label { get; set; }
                }

                public class ProductReference
                {
                    public string reference { get; set; }
                    public string reference_type { get; set; }
                }

                public class Offer
                {
                    public string ean { get { return product_references.Where(x => x.reference_type == "EAN").Select(x => x.reference).FirstOrDefault(); } }
                    public bool active { get; set; }
                    public List<AllPrice> all_prices { get; set; }
                    public bool allow_quote_requests { get; set; }
                    public ApplicablePricing applicable_pricing { get; set; }
                    public string category_code { get; set; }
                    public string category_label { get; set; }
                    public List<string> channels { get; set; }
                    public string currency_iso_code { get; set; }
                    public string description { get; set; }
                    public object discount { get; set; }
                    public LogisticClass logistic_class { get; set; }
                    public double min_shipping_price { get; set; }
                    public double min_shipping_price_additional { get; set; }
                    public string min_shipping_type { get; set; }
                    public string min_shipping_zone { get; set; }
                    public List<object> offer_additional_fields { get; set; }
                    public int offer_id { get; set; }
                    public double price { get; set; }
                    public object price_additional_info { get; set; }
                    public List<ProductReference> product_references { get; set; }
                    public string product_sku { get; set; }
                    public string product_title { get; set; }
                    public int quantity { get; set; }
                    public string shop_sku { get; set; }
                    public string state_code { get; set; }
                    public double total_price { get; set; }
                }

                public class Root
                {
                    public List<Offer> offers { get; set; }
                    public int total_count { get; set; }
                }


            }


            public class CSVOffer
            {
                public string file { get; set; }
                public string import_mode { get; set; }
                public bool with_products { get; set; }

            }
            #endregion

            public static async Task<object> Upload(string actionUrl)
            {
                try
                {
                    //Image newImage = Image.FromFile(@"Absolute Path of image");
                    //ImageConverter _imageConverter = new ImageConverter();
                    byte[] paramFileStream = File.ReadAllBytes(@"C:\Users\jacek\Downloads\empik_offers.xlsx");

                    var formContent = new MultipartFormDataContent
                    {
                        // Send form text values here
                        {new StringContent("import_mode"),"REPLACE"},
                        {new StringContent("with_products"),"0" },
                        // Send Image Here
                        {new StreamContent(new MemoryStream(paramFileStream)),"file","filename.csv"}
                    };

                    var myHttpClient = new HttpClient();
                    myHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("1e2ff244-9231-4b07-a1ac-d85dc3b7edca");
                    myHttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("multipart/form-data"));
                    myHttpClient.DefaultRequestHeaders.Add("Api-Key", "1e2ff244-9231-4b07-a1ac-d85dc3b7edca");
                    myHttpClient.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("pl-PL"));


                    var response = await myHttpClient.PostAsync(actionUrl.ToString(), formContent);
                    string stringContent = await response.Content.ReadAsStringAsync();

                    return response;
                }
                catch (Exception ex)
                {


                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania listy offers {0}, {1}", 1, ex.Message));
                    return null;
                }
            }


            public static void SetOffers2(Dal.Helper.Shop shop)
            {


                HttpWebRequest request = GetHttpWebRequest("/api/offers/imports", "POST");
                request.MediaType = "multipart/form-data input";
                request.ContentType = "multipart/form-data input";

                try
                {
                    string text = null;
                    CSVOffer offer = new CSVOffer();
                    offer.import_mode = "REPLACE";//NORMAL, PARTIAL_UPDATE, REPLACE
                    offer.with_products = false;
                    offer.file = EmpikRESTHelper.Offers.GetFile(shop);


                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(offer);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();




                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

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


                            Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania listy offers {0}, {1}", 1, text));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania listy offers {0}, {1}", 1, ex.Message));
                }
            }
            public static void SetOffers(Dal.Helper.Shop shop)
            {
                HttpWebRequest request = GetHttpWebRequest("/api/offers/imports", "POST");
                request.MediaType = "multipart/form-data";
                request.ContentType = "multipart/form-data";

                try
                {
                    string text = null;
                    CSVOffer offer = new CSVOffer();
                    offer.import_mode = "REPLACE";//NORMAL, PARTIAL_UPDATE, REPLACE
                    offer.with_products = false;
                    offer.file = EmpikRESTHelper.Offers.GetFile(shop);


                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(offer);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();




                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

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


                            Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania listy offers {0}, {1}", 1, text));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania listy offers {0}, {1}", 1, ex.Message));
                }
            }

            public static void GetOffers(Dal.Helper.Shop shop)
            {
                GetOffers(shop, 0);
            }
            private static void GetOffers(Dal.Helper.Shop shop, int page)
            {
                int max = 100;
                int offset = page * max;

                HttpWebRequest request = GetHttpWebRequest(String.Format("/api/offers?max={0}&offset={1}", max, offset), "GET");

                try
                {
                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    OffersList.Root offersRoot = json_serializer.Deserialize<OffersList.Root>(text);


                    SetOrders(shop, offersRoot);

                    if (offersRoot.total_count > offset + max)
                        GetOffers(shop, page + 1);

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


                            Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania listy offers {0}, {1}", 1, text));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania listy offers {0}, {1}", 1, ex.Message));
                }
            }

            private static void SetOrders(Dal.Helper.Shop shop, OffersList.Root offersRoot)
            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

                foreach(OffersList.Offer offer in offersRoot.offers)
                {
                    bool eanFound = pch.SetShopProductToProductCatalogByEan(shop, offer.ean, offer.product_sku);
                    if (!eanFound)
                        Bll.ErrorHandler.SendEmail(String.Format("Empik. Ean {0} w empik ale nie w katalogu", eanFound));

                }
            }


            //public static void SetOffers()
            //{
            //    HttpWebRequest request = GetHttpWebRequest("/api/offers", "POST");

            //    try
            //    {
            //        string text = null;
            //        using (WebResponse webResponse = request.GetResponse())
            //        {
            //            Stream responseStream = webResponse.GetResponseStream();
            //            StreamReader reader = new StreamReader(responseStream);
            //            text = reader.ReadToEnd();
            //        }
            //        RootOffer rootOffer = GetOffer();
            //       // var json_serializer = new JavaScriptSerializer();
            //        //RootOffer offersRoot = json_serializer.Deserialize<RootOffer>(text);

            //        //SetOrders(ordersRoot);
            //    }
            //    catch (WebException ex)
            //    {
            //        using (WebResponse response = ex.Response)
            //        {
            //            HttpWebResponse httpResponse = (HttpWebResponse)response;
            //            if (httpResponse == null)
            //            {
            //                Bll.ErrorHandler.SendError(ex, ex.Message);


            //            }
            //            Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
            //            using (Stream data = response.GetResponseStream())
            //            using (var reader = new StreamReader(data))
            //            {
            //                string text = reader.ReadToEnd();


            //                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania listy offers {0}, {1}", 1, text));
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania listy offers {0}, {1}", 1, ex.Message));
            //    }
            //}

            #region private
            //private static RootOffer GetOffer()
            //{
            //    RootOffer rootOffer = new RootOffer();

            //    rootOffer.offers = GetOffers();

            //    return rootOffer;
            //}

            //public static List<Offer> GetOffers()
            //{
            //    List<Offer> offers = new List<Offer>();

            //    offers.Add(GetOffer(null));

            //    return offers;
            //}

            //private static Offer GetOffer(Dal.ProductCatalogForShopFnResult product)
            //{
            //    Offer offer = new Offer()
            //    {
            //        allow_quote_requests = true,
            //        all_prices = GetAllPrices(product),
            //        available_ended = DateTime.Now.AddYears(1),
            //        available_started = DateTime.Now,
            //        description = "",
            //        //discount = GetDiscount(),
            //        internal_description = "",
            //        leadtime_to_ship = 0,
            //        logistic_class = "",
            //        max_order_quantity = 50,
            //        min_order_quantity = 1,
            //        min_quantity_alert = 1,
            //       // offer_additional_fields = GetAdditionalFields(),
            //        package_quantity = "",
            //        price = 0,
            //        price_additional_info = "",
            //        product_id = "",
            //        product_id_type = "",
            //        product_tax_code = "",
            //        quantity = "",
            //        shop_sku = "",
            //        state_code = "",
            //        update_delete = ""
            //    };
            //    return offer;
            //}

            //private static List<AllPrice> GetAllPrices(ProductCatalogForShopFnResult product)
            //{
            //    List<AllPrice> allrpices = new List<AllPrice>();
            //    AllPrice allPrice = new AllPrice()
            //    {

            //    };
            //    return allrpices;
            //}
            #endregion
        }

    }
}
