using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using LajtIt.Dal;
using static LajtIt.Bll.CeneoHelper;

namespace LajtIt.Bll
{
    public class EmagHelper
    {
        [XmlRoot(ElementName = "product")]
        public class Product
        {
            [XmlElement(ElementName = "id")]
            public string Id { get; set; }
            [XmlElement(ElementName = "category")]
            public string Category { get; set; }
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "part_number")]
            public string Part_number { get; set; }
            [XmlElement(ElementName = "ean", IsNullable =true)]
            public string Ean { get; set; }
            [XmlElement(ElementName = "brand")]
            public string Brand { get; set; }
            [XmlElement(ElementName = "images_url_1")]
            public string Images_url_1 { get; set; }
            [XmlElement(ElementName = "images_url_2")]
            public string Images_url_2 { get; set; }
            [XmlElement(ElementName = "images_url_3")]
            public string Images_url_3 { get; set; }
            [XmlElement(ElementName = "url")]
            public string Url { get; set; }
            [XmlElement(ElementName = "sale_price")]
            public string Sale_price { get; set; }
            [XmlElement(ElementName = "recommended_price")]
            public string Recommended_price { get; set; }
            [XmlElement(ElementName = "VAT")]
            public string VAT { get; set; }
            [XmlElement(ElementName = "stock")]
            public string Stock { get; set; }
            [XmlElement(ElementName = "weight")]
            public string Weight { get; set; }
            [XmlElement(ElementName = "warranty")]
            public string Warranty { get; set; }
            [XmlElement(ElementName = "handling_time")]
            public string Handling_time { get; set; }
            [XmlElement(ElementName = "description")]
            public string Description { get; set; }
        }

        [XmlRoot(ElementName = "shop")]
        public class Shop
        {
            [XmlElement(ElementName = "product")]
            public List<Product> Product { get; set; }
        }

        //internal void GenerateXMLFileHomebook(string utm, Dal.Helper.Shop s)
        //{ 
        //    List<Dal.ShopFnResult> products = Dal.DbHelper.ProductCatalog.GetProductCatalogForShop((int)s);

        //    Shop shop = new Shop();
        //    shop.Product = new List<Product>();

        //    products = products.Where(x => x.ShopProductId != null).ToList();

        //    NumberFormatInfo nfi = new NumberFormatInfo();
        //    nfi.NumberDecimalSeparator = ".";

        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources(Dal.Helper.ShopType.Homebook);

        //    foreach (Dal.ShopFnResult product in products)
        //    {
        //        int deliveryTime = Bll.Helper.GetSetting("SHOPMINDEL").IntValue.Value;
        //        int extId = sources.Where(x => x.DeliveryId == product.DeliveryId).Select(x => x.ExternalValue).FirstOrDefault();
        //        int shopDeliveryTimeId =
        //            Bll.ShopUpdateHelper.ClickShop.GetDeliveryTime(
        //                product.LeftQuantity,
        //                extId,
        //                deliveryTime);


        //        Product p = new Product()
        //        {
        //            Brand = product.SupplierName,
        //            Category = product.CategoryName,
        //            Description = Bll.OrderHelper.GetPreview(Dal.Helper.Shop.Homebook, product.ProductCatalogId, false).ToString(),
        //            Ean = product.Ean,
        //            Handling_time = shopDeliveryTimeId.ToString(),
        //            Id = product.ShopProductId.ToString(),
        //            Name = product.Name,
        //            Part_number = product.Code,
        //            Sale_price = String.Format(nfi, "{0:0.00}", product.PriceBruttoMinimum.Value),
        //            Stock = GetStock(product),
        //            Url = String.Format("http://lajtit.pl/pl/p/p/{0}?utm_source={1}&utm_medium=cpc", product.ShopProductId, utm),
        //            VAT = Dal.Helper.VAT.ToString(nfi),
        //            Warranty = "24",
        //            Images_url_1 = String.Format("http://static.lajtit.pl/ProductCatalog/{0}", product.ImageFullName)

        //        };
        //        shop.Product.Add(p);
        //    }
        //    string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];


        //    string saveLocation = String.Format(path, String.Format("{0}.xml", utm));


        //    XmlSerializer xmlserializer = new XmlSerializer(typeof(Shop));
        //    Utf8StringWriter stringWriter = new Utf8StringWriter();
        //    XmlWriter writer = XmlWriter.Create(stringWriter);

        //    xmlserializer.Serialize(writer, shop);

        //    string serializeXml = stringWriter.ToString();

        //    writer.Close();

        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(serializeXml);


        //    //if (File.Exists(saveLocation))
        //    //    File.Copy(saveLocation.Replace(".xml", String.Format("_{0}.xml", Guid.NewGuid())), saveLocation);
        //    doc.Save(saveLocation);

        //    if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Prod)
        //    {

        //        FtpHelper fh = new FtpHelper();
        //        fh.UploadFileToftp(saveLocation, "/");
        //    }
        //}
        //internal void GenerateXMLFile(string utm, Dal.Helper.Shop s)
        //{ 
        //    List<Dal.ShopFnResult> products = Dal.DbHelper.ProductCatalog.GetProductCatalogForShop((int)s);

        //    Shop shop = new Shop();
        //    shop.Product = new List<Product>();

        //    products = products.Where(x => x.ShopProductId!=null).ToList();
             
        //    NumberFormatInfo nfi = new NumberFormatInfo();
        //    nfi.NumberDecimalSeparator = ".";
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources(Dal.Helper.ShopType.Homebook);
             
        //    foreach (Dal.ShopFnResult product in products)
        //    {
        //        int deliveryTime = Bll.Helper.GetSetting("SHOPMINDEL").IntValue.Value;
        //        int extId = sources.Where(x => x.DeliveryId == product.DeliveryId).Select(x => x.ExternalValue).FirstOrDefault();
        //        int shopDeliveryTimeId = 
        //            Bll.ShopUpdateHelper.ClickShop. GetDeliveryTime(
        //                product.LeftQuantity,
        //                extId, 
        //                deliveryTime);


        //        Product p = new Product()
        //        {
        //            Brand = product.SupplierName,
        //            Category = product.CategoryName,
        //            Description = Bll.OrderHelper.GetPreview(s, product.ProductCatalogId, false).ToString(),
        //            Ean = product.Ean,
        //            Handling_time = shopDeliveryTimeId.ToString(),
        //            Id = product.ShopProductId.ToString(),
        //            Name = product.Name,
        //            Part_number = product.Code,
        //            Sale_price = String.Format(nfi, "{0:0.00}", product.PriceBruttoMinimum.Value / (1 + Dal.Helper.VAT)),
        //            Stock = GetStock(product),
        //            Url = String.Format("http://lajtit.pl/pl/p/p/{0}?utm_source={1}&utm_medium=cpc", product.ShopProductId, utm),
        //            VAT = Dal.Helper.VAT.ToString(nfi),
        //            Warranty = "24",
        //            Images_url_1 = String.Format("http://static.lajtit.pl/ProductCatalog/{0}", product.ImageFullName)

        //        };
        //        shop.Product.Add(p);
        //    }
        //    string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];


        //    string saveLocation = String.Format(path, String.Format("{0}.xml", utm));
            

        //    XmlSerializer xmlserializer = new XmlSerializer(typeof(Shop));
        //    Utf8StringWriter stringWriter = new Utf8StringWriter();
        //    XmlWriter writer = XmlWriter.Create(stringWriter);

        //    xmlserializer.Serialize(writer, shop);

        //    string serializeXml = stringWriter.ToString();

        //    writer.Close();

        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(serializeXml);


        //    //if (File.Exists(saveLocation))
        //    //    File.Copy(saveLocation.Replace(".xml", String.Format("_{0}.xml", Guid.NewGuid())), saveLocation);
        //    doc.Save(saveLocation);

        //    if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Prod)
        //    {

        //        FtpHelper fh = new FtpHelper();
        //        fh.UploadFileToftp(saveLocation, "/");
        //    }
        //}
        private static string GetStock(Dal.ShopFnResult item)
        {
            int quantity = 50;

            if (item.SupplierQuantity.HasValue && item.SupplierQuantity.Value > 0)
                quantity = item.SupplierQuantity.Value + item.LeftQuantity;
            else
            {
                if (item.IsOnStock && !item.IsAvailable)
                {
                    quantity = item.LeftQuantity;// + 10;
                }
            }
            return quantity.ToString();
        }
    }

    public partial class EmagRESTHelper
    {
        #region Klasy

        public class Product
        {
       

            public class Characteristic
            {
                public string id { get; set; }
                public string value { get; set; }
            }
            public class Image
            {
                public string url { get; set; }
                public int display_type { get; set; }
            }

            public class OfferDetails
            {
                public int id { get; set; }
                public int warranty_type { get; set; }
                public int supply_lead_time { get; set; }
            }

            public class Availability
            {
                public int warehouse_id { get; set; }
                public int id { get; set; }
            }

            public class Stock
            {
                public int warehouse_id { get; set; }
                public int value { get; set; }
            }

            public class Commission
            {
                public string type{ get; set; }
                public int value { get; set; }
            }

            public class HandlingTime
            {
                public int warehouse_id { get; set; }
                public int value { get; set; }
            }

            public class ErrorDescription
            {
                public string en_GB { get; set; }
                public string ro_RO { get; set; }
                public string bg_BG { get; set; }
                public string hu_HU { get; set; }
                public string pl_PL { get; set; }
            }

            public class UserMessage2
            {
                public string en_GB { get; set; }
                public string ro_RO { get; set; }
                public string bg_BG { get; set; }
                public string hu_HU { get; set; }
                public string pl_PL { get; set; }
            }

            public class SellerText
            {
                public string en_GB { get; set; }
                public string ro_RO { get; set; }
                public string bg_BG { get; set; }
                public string hu_HU { get; set; }
                public string pl_PL { get; set; }
            }

            public class UserMessage
            {
                public string error_code { get; set; }
                public ErrorDescription error_description { get; set; }
                public object doc_id { get; set; }
                public object reasons { get; set; }
                public object elements { get; set; }
                public object comment { get; set; }
                public object suggestion { get; set; }
                public UserMessage2 user_message { get; set; }
                public string error_level { get; set; }
                public bool automated_response { get; set; }
                public string section { get; set; }
                public SellerText seller_text { get; set; }
            }

            public class Error
            {
                public string message { get; set; }
                public List<UserMessage> user_message { get; set; }
            }

            public class ValidationStatu
            {
                public int value { get; set; }
                public string description { get; set; }
                public List<Error> errors { get; set; }
            }

            public class OfferValidationStatus
            {
                public int value { get; set; }
                public string description { get; set; }
                public object errors { get; set; }
            }

            public class Result
            {
                public int category_id { get; set; }
                public string id { get; set; }
                //public string brand { get; set; }
                //public object vendor_category_id { get; set; }
                //public string name { get; set; }
                //public string part_number { get; set; }
        //        public double sale_price { get; set; }
                //public string currency { get; set; }
                //public string description { get; set; }
                //public string url { get; set; }
                //public int warranty { get; set; }
                //public int general_stock { get; set; }
                //public string weight { get; set; }
         //       public int status { get; set; }
                //public object recommended_price { get; set; }
                //public List<Image> images { get; set; }
                public List<Characteristic> characteristics { get; set; }
                //public List<object> attachments { get; set; }
        //        public int vat_id { get; set; }
                //public object family { get; set; }
                //public List<object> start_date { get; set; }
                //public int estimated_stock { get; set; }
                //public object min_sale_price { get; set; }
                //public object max_sale_price { get; set; }
                //public OfferDetails offer_details { get; set; }
                //public List<object> offer_properties { get; set; }
         //       public List<Availability> availability { get; set; }
        //        public List<Stock> stock { get; set; }
        //        public List<HandlingTime> handling_time { get; set; }
                //public List<object> barcode { get; set; }
                //public List<string> ean { get; set; }
        //        public List<Commission> commission { get; set; }
                //public List<ValidationStatu> validation_status { get; set; }
                //public OfferValidationStatus offer_validation_status { get; set; }
                //public int ownership { get; set; }
            }

            public class RootObject
            {
                public bool isError { get; set; }
                public List<object> messages { get; set; }
                public List<Result> results { get; set; }
            }
        }
        #region Order
        public class Order
        {
            public class Customer
            {
                public int id { get; set; }
                public int mkt_id { get; set; }
                public string name { get; set; }
                public string company { get; set; }
                public string gender { get; set; }
                public string phone_1 { get; set; }
                public string phone_2 { get; set; }
                public string phone_3 { get; set; }
                public string registration_number { get; set; }
                public string code { get; set; }
                public string email { get; set; }
                public string billing_name { get; set; }
                public string billing_phone { get; set; }
                public string billing_country { get; set; }
                public string billing_suburb { get; set; }
                public string billing_city { get; set; }
                public int billing_locality_id { get; set; }
                public string billing_street { get; set; }
                public string billing_postal_code { get; set; }
                public string shipping_country { get; set; }
                public string shipping_suburb { get; set; }
                public string shipping_city { get; set; }
                public int shipping_locality_id { get; set; }
                public string shipping_postal_code { get; set; }
                public string shipping_contact { get; set; }
                public string shipping_phone { get; set; }
                public string created { get; set; }
                public string modified { get; set; }
                public string bank { get; set; }
                public string iban { get; set; }
                public int legal_entity { get; set; }
                public string fax { get; set; }
                public int is_vat_payer { get; set; }
                public string liable_person { get; set; }
                public string shipping_street { get; set; }
            }

            public class Product
            {
                public string ext_part_number { get; set; }
                public int retained_amount { get; set; }
                public string sale_price { get; set; }
                public string created { get; set; }
                public string modified { get; set; }
                public string original_price { get; set; }
                public int id { get; set; }
                public string product_id { get; set; }
                public string part_number { get; set; }
                public string currency { get; set; }
                public int quantity { get; set; }
                public string vat { get; set; }
                public int status { get; set; }
                public List<object> attachments { get; set; }
                public int initial_qty { get; set; }
                public int storno_qty { get; set; }
                public List<object> details { get; set; }
            }

            public class Result
            {
                public string vendor_name { get; set; }
                public int id { get; set; }
                public object parent_id { get; set; }
                public string date { get; set; }
                public string payment_mode { get; set; }
                public int payment_mode_id { get; set; }
                public string delivery_mode { get; set; }
                public string observation { get; set; }
                public int status { get; set; }
                public int payment_status { get; set; }
                public Customer customer { get; set; }
                public List<Product> products { get; set; }
                public int shipping_tax { get; set; }
                public List<object> vouchers { get; set; }
                public List<object> proforms { get; set; }
                public List<object> attachments { get; set; }
                public object cashed_co { get; set; }
                public int cashed_cod { get; set; }
                public object cancellation_request { get; set; }
                public int has_editable_products { get; set; }
                public string refunded_amount { get; set; }
                public int is_complete { get; set; }
                public object refund_status { get; set; }
                public string maximum_date_for_shipment { get; set; }
                public List<object> details { get; set; }
            }

            public class RootObject
            {
                public bool isError { get; set; }
                public List<object> messages { get; set; }
                public List<Result> results { get; set; }
            }
        }

        public static void SetProducts()
        {
            GetProduct(9947);
        }
        #endregion
        public class Characteristic
        { 
            public int id { get; set; }
            public string name { get; set; }
            public int type_id { get; set; }
            public int display_order { get; set; }
            public int is_mandatory { get; set; } 
            public int allow_new_value { get; set; }
            public int is_filter { get; set; }
            public List<string> values { get; set; }
        }
        public class Result
        {
            public int id { get; set; }
            public int? parent_id { get; set; }
            public List<Characteristic> characteristics { get; set; }
            public List<object> family_types { get; set; }
            public string name { get; set; }
            public bool is_ean_mandatory { get; set; }
            public int is_allowed { get; set; }
        }

        public class RootObject
        {
            public bool isError { get; set; }
            public List<object> messages { get; set; }
            public List<Result> results { get; set; }
        }

        #endregion
        
 

        private static string GetResponse(string url, string jsonEncodedParams)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(@"https://marketplace.emag.pl/api-3/{0}",url));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (SHA256 sha256 = new SHA256Managed())
            {


                try
                {
                    string clcs = Bll.RESTHelper.Base64Encode(String.Format("{0}:{1}", "kontakt@lajtit.pl", "R47SoFQm"));
                    request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", clcs));


                    //Category c = new Category() { id = 1232 };


                    Stream dataStream = request.GetRequestStream();



                    //string jsonEncodedParams = @"currentPage=10";
                    //QueryStringBuilder.BuildQueryString(c);
                    //ToJson(c);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();




                    HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string text = reader.ReadToEnd();

                    // RootObject response = FromJson(text);

                    return text;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public static void GetCategory()
        {
            List<Dal.ShopCategory> categories = new List<Dal.ShopCategory>();
            GetCategory(1, categories);


            Dal.ShopHelper sh = new Dal.ShopHelper();

            sh.SetCategories(Dal.Helper.ShopType.eMag, categories);
        }

        private static void GetCategory(int page, List<Dal.ShopCategory> categories)
        { 
            string jsonEncodedParams = String.Format("currentPage={0}", page);

            string response = GetResponse(@"category/read", jsonEncodedParams);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            RootObject rootObject = serializer.Deserialize<RootObject>(response);



            foreach (Result category in rootObject.results)
            {
                Dal.ShopCategory sc = new Dal.ShopCategory()
                {
                    ShopTypeId = (int)Dal.Helper.ShopType.eMag,
                    ShopCategoryId=category.id.ToString(),
                    Name = category.name,
                    CategoryOrder = 0,
                    IsActive = true,
                    Description = "",
                    SeoTitle = "",
                    SeoDescription = "",
                    SeoKeywords = "",
                    Permalink = "",
                    Url="",
                    IsPublished = true,
                    IsAllowed=category.is_allowed==1
                };

                if (category.parent_id.HasValue)
                    sc.CategoryParentId = category.parent_id.ToString();

                categories.Add(sc);
            }

            if (rootObject.results.Count == 100)
                GetCategory(page+1, categories);

        }
        public static Result  GetCategory(string id)
        {
            string jsonEncodedParams = String.Format("id={0}", id);

            string response = GetResponse(@"category/read", jsonEncodedParams);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            RootObject rootObject = serializer.Deserialize<RootObject>(response);

            return rootObject.results[0];

            foreach (Result category in rootObject.results)
            {
                Dal.ShopCategory sc = new Dal.ShopCategory()
                {
                    ShopTypeId = (int)Dal.Helper.ShopType.eMag,
                    ShopCategoryId = category.id.ToString(),
                    Name = category.name,
                    CategoryOrder = 0,
                    IsActive = true,
                    Description = "",
                    SeoTitle = "",
                    SeoDescription = "",
                    SeoKeywords = "",
                    Permalink = "",
                    Url = "",
                    IsPublished = true,
                    IsAllowed = category.is_allowed == 1
                };

                if (category.parent_id.HasValue)
                    sc.CategoryParentId = category.parent_id.ToString();

           
            }
 

        }
        public static Result GetProduct(int shopProductId)
        {
            string jsonEncodedParams1 = String.Format("id={0}", shopProductId);

            string response1 = GetResponse(@"product_offer/read", jsonEncodedParams1);

            JavaScriptSerializer serializer1 = new JavaScriptSerializer();
            serializer1.MaxJsonLength = Int32.MaxValue;
            Product.RootObject rootObject = serializer1.Deserialize<Product.RootObject>(response1);

           // jsonEncodedParams = String.Format(@"Array(Array(""id""=>""{0}"",""name""=>""{1}""))", shopProductId, rootObject.results[0].name);


            //Product.Result update = new Product.Result()
            //{
            //    id = shopProductId.ToString(),
            //    //characteristics=GetCharacteristics(shopProductId)
            //};

            //List<Product.Result> results = new List<Product.Result>();
            //results.Add(update);

            //string i = ToJson(update);
            string j = Bll.RESTHelper.ToJson(rootObject.results[0]);


            string response = GetResponse(@"product_offer/save", j);
             
         
            Product.RootObject rootObject111 = serializer1.Deserialize<Product.RootObject>(response);
            //Product.RootObject rootObject = serializer.Deserialize<Product.RootObject>(response);
             

            return null;
          


        }

        private static List<Product.Characteristic> GetCharacteristics(int shopProductId)
        {
            List<Product.Characteristic> characteristics = new List<Product.Characteristic>();

            characteristics.Add(
                new Product.Characteristic()
                {
                    id= "8341",
                    value="7"
                }
                );
            characteristics.Add(
                new Product.Characteristic()
                {
                    id= "2988",
                    value= "LED"
                }
                );
            return characteristics;
        } 
        public static void TestApi()
        {
            GetCategory();
        }


        public static void GetOrders()
        {
            string jsonEncodedParams = "";// String.Format("currentPage={0}", page);

            string response = GetResponse(@"order/read", jsonEncodedParams);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            Order.RootObject rootObject = serializer.Deserialize<Order.RootObject>(response);

            ProcessOrders(rootObject);

        }

        private static void ProcessOrders(Order.RootObject rootObject)
        {

            Dal.ShopHelper sh = new Dal.ShopHelper();


            string[] shopOrderNumbers = rootObject.results.Select(x => x.id.ToString()).ToArray();

            string[] orderToProcess = sh.InsertOrders(shopOrderNumbers, null, Dal.Helper.Shop.eMag, "system");


            ProcessOrders(orderToProcess, Dal.Helper.Shop.eMag, Dal.Helper.Shop.Lajtitpl);

        }

        private static void ProcessOrders(string[] orderToProcess, Dal.Helper.Shop shop, Dal.Helper.Shop shopExportedProducts)
        {

            foreach(string orderNumber in orderToProcess)
            {
                string jsonEncodedParams = String.Format("id={0}", orderNumber);

                string response = GetResponse(@"order/read", jsonEncodedParams);


                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                Order.RootObject r = serializer.Deserialize<Order.RootObject>(response);

                List<Dal.OrderProduct> products = new List<OrderProduct>();

                Dal.Order order = null;

                var eMag = r.results[0];

                Dal.ShopHelper sh = new Dal.ShopHelper();
                Dal.ShopOrder so = sh.GetShopOrder(shop, orderNumber);

                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

                try
                {
                    order = new Dal.Order()
                    {
                        InsertDate = DateTime.Parse(eMag.date),
                        ShippingCost = Convert.ToDecimal(eMag.shipping_tax, new System.Globalization.CultureInfo("en-us")),
                        ShippingCostVAT = Dal.Helper.VAT,
                        ShopId = (int)Dal.Helper.Shop.eMag,
                        OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                        ParDate = null,
                        CompanyId = Dal.Helper.DefaultCompanyId,
                         
                        DeliveryDate = DateTime.Parse(eMag.maximum_date_for_shipment),
                        Email = Dal.Helper.MyEmail,
                        ExternalUserId = eMag.customer.id,
                       
                        Phone = eMag.customer.phone_1,
                        Phone2 = eMag.customer.phone_2, 
                        ShipmentAddress = eMag.customer.shipping_street,
                        ShipmentCity = eMag.customer.shipping_city,
                        ShipmentFirstName = eMag.customer.name,
                        ShipmentPostcode = eMag.customer.shipping_postal_code,
                        ShipmentCompanyName="",
                        ShipmentLastName="",
                        ExternalOrderNumber= orderNumber
                    };

                    OrderPayment payment = null;
                    //switch (eMag.payment_mode_id)
                    //{
                    //    case 1:
                    //        order.ShippintTypeId = 4; break;
                    //    case 2:
                    //        order.ShippintTypeId = 3; break;
                    //    case 3:
                    //        order.ShippintTypeId = 3; 
                    //        if(eMag.payment_status ==1)
                    //        {
                    //            payment = new OrderPayment()
                    //            {
                    //                Amount = Decimal.Parse(eMag.cashed_co.ToString()),
                    //                InsertDate = order.InsertDate,
                    //                InsertUser = "system",
                    //                Order = order,
                    //                PaymentTypeId = 1
                    //            };
                    //        }
                    //        break;
                    //}

                    Dal.Invoice invoice = null;

                    if (eMag.customer.legal_entity == 1)
                    {
                        invoice = new Invoice()
                        {
                            Address = eMag.customer.billing_street,
                            City = eMag.customer.billing_city,
                            CompanyId = Dal.Helper.DefaultCompanyId,
                            CompanyName = eMag.customer.company,
                            Email = Dal.Helper.MyEmail,
                            ExcludeFromInvoiceReport = false,
                            InvoiceDate = order.InsertDate,
                            InvoiceTypeId = 2,
                            IsDeleted = false,
                            Nip = eMag.customer.code,
                            Postcode = eMag.customer.billing_postal_code,
                            IsLocked = false
                        };

                        order.Invoice = invoice;
                    }
                    foreach (Order.Product product in eMag.products)
                    {
                        Dal.ProductCatalog pc = pch.GetProductCatalogOnShopProductId(shopExportedProducts, product.product_id);

                        Dal.OrderProduct op = new OrderProduct()
                        {
                            ExternalProductId = product.id,
                            Order = order,
                            OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New,
                            Price = Convert.ToDecimal(product.sale_price, new System.Globalization.CultureInfo("en-us")) * (1 + Convert.ToDecimal(product.vat, new System.Globalization.CultureInfo("en-us"))),
                            ProductName = pc.Name,
                            ProductCatalogId = pc.ProductCatalogId,
                            ProductTypeId = 1,
                            Quantity = product.quantity,
                            Rebate = 0,
                            VAT = Dal.Helper.VAT

                        };
                        op.Price = Decimal.Round(op.Price, 2);
                        products.Add(op);
                    }
                    OrderStatusHistory osh = new OrderStatusHistory()
                    {
                        Comment = "",//String.Format("Dostawa: {0}\n\nUwagi: {1}", eMag., r.CustomerRemark),
                        InsertDate = DateTime.Now,
                        Order = order,
                        InsertUser = "system",
                        OrderStatusId = (int)Dal.Helper.OrderStatus.New
                        
                    };
                    so.IsProcessed = true;
                   
                    sh.SetNewOrder(so, invoice, products, shop, osh, payment, null);
                }
                catch (Exception ex)
                {
                    ErrorHandler.SendError(ex, String.Format("Błąd wczytywania zamówienia eMag {0}", orderNumber));

                }
            }
        }
    }
    /// <summary>
    ///  Helps up build a query string by converting an object into a set of named-values and making a
    ///  query string out of it.
    /// </summary>
    public class QueryStringBuilder
    {
        private readonly List<KeyValuePair<string, object>> _keyValuePairs
          = new List<KeyValuePair<string, object>>();

        /// <summary> Builds the query string from the given instance. </summary>
        public static string BuildQueryString(object queryData, string argSeperator = "&")
        {
            var encoder = new QueryStringBuilder();
            encoder.AddEntry(null, queryData, allowObjects: true);

            return encoder.GetUriString(argSeperator);
        }

        /// <summary>
        ///  Convert the key-value pairs that we've collected into an actual query string.
        /// </summary>
        private string GetUriString(string argSeperator)
        {
            return String.Join(argSeperator,
                               _keyValuePairs.Select(kvp =>
                               {
                                   var key = Uri.EscapeDataString(kvp.Key);
                                   var value = Uri.EscapeDataString(kvp.Value.ToString());
                                   return $"{key}={value}";
                               }));
        }

        /// <summary> Adds a single entry to the collection. </summary>
        /// <param name="prefix"> The prefix to use when generating the key of the entry. Can be null. </param>
        /// <param name="instance"> The instance to add.
        ///  
        ///  - If the instance is a dictionary, the entries determine the key and values.
        ///  - If the instance is a collection, the keys will be the index of the entries, and the value
        ///  will be each item in the collection.
        ///  - If allowObjects is true, then the object's properties' names will be the keys, and the
        ///  values of the properties will be the values.
        ///  - Otherwise the instance is added with the given prefix to the collection of items. </param>
        /// <param name="allowObjects"> true to add the properties of the given instance (if the object is
        ///  not a collection or dictionary), false to add the object as a key-value pair. </param>
        private void AddEntry(string prefix, object instance, bool allowObjects)
        {
            var dictionary = instance as IDictionary;
            var collection = instance as ICollection;

            if (dictionary != null)
            {
                Add(prefix, GetDictionaryAdapter(dictionary));
            }
            else if (collection != null)
            {
                Add(prefix, GetArrayAdapter(collection));
            }
            else if (allowObjects)
            {
                Add(prefix, GetObjectAdapter(instance));
            }
            else
            {
                _keyValuePairs.Add(new KeyValuePair<string, object>(prefix, instance));
            }
        }

        /// <summary> Adds the given collection of entries. </summary>
        private void Add(string prefix, IEnumerable<Entry> datas)
        {
            foreach (var item in datas)
            {
                var newPrefix = String.IsNullOrEmpty(prefix)
                  ? item.Key
                  : $"{prefix}[{item.Key}]";

                AddEntry(newPrefix, item.Value, allowObjects: false);
            }
        }

        private struct Entry
        {
            public string Key;
            public object Value;
        }

        /// <summary>
        ///  Returns a collection of entries that represent the properties on the object.
        /// </summary>
        private IEnumerable<Entry> GetObjectAdapter(object data)
        {
            var properties = data.GetType().GetProperties();

            foreach (var property in properties)
            {
                yield return new Entry()
                {
                    Key = property.Name,
                    Value = property.GetValue(data)
                };
            }
        }

        /// <summary>
        ///  Returns a collection of entries that represent items in the collection.
        /// </summary>
        private IEnumerable<Entry> GetArrayAdapter(ICollection collection)
        {
            int i = 0;
            foreach (var item in collection)
            {
                yield return new Entry()
                {
                    Key = i.ToString(),
                    Value = item,
                };
                i++;
            }
        }

        /// <summary>
        ///  Returns a collection of entries that represent items in the dictionary.
        /// </summary>
        private IEnumerable<Entry> GetDictionaryAdapter(IDictionary collection)
        {
            foreach (DictionaryEntry item in collection)
            {
                yield return new Entry()
                {
                    Key = item.Key.ToString(),
                    Value = item.Value,
                };
            }
        }
    }
}
