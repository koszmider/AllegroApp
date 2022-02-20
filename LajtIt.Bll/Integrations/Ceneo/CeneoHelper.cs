using LajtIt.Dal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class CeneoApiHelper
    {
        public class OrderItemsObject
        {
            public class Metadata
            {
                public string id { get; set; }
                public string uri { get; set; }
                public string type { get; set; }
            }

            public class Deferred
            {
                public string uri { get; set; }
            }

            public class Order
            {
                public Deferred __deferred { get; set; }
            }

            public class Result
            {
                public Metadata __metadata { get; set; }
                public Order Order { get; set; }
                public string OrderId { get; set; }
                public string ShopProductId { get; set; }
                public string Name { get; set; }
                public string ShopGuid { get; set; }
                public string Price { get; set; }
                public int Count { get; set; }
                public string TotalItemPrice { get; set; }
                public object TotalItemPriceDiscount { get; set; }
                public object Variant { get; set; }
                public int Returned { get; set; }
            }

            public class D
            {
                public List<Result> results { get; set; }
            }

            public class RootObject
            {
                public D d { get; set; }
            }

        }
        public class PaymentTypeObject {
            public class Metadata
            {
                public string id { get; set; }
                public string uri { get; set; }
                public string type { get; set; }
            }

            public class Deferred
            {
                public string uri { get; set; }
            }

            public class Orders
            {
                public Deferred __deferred { get; set; }
            }

            public class D
            {
                public Metadata __metadata { get; set; }
                public Orders Orders { get; set; }
                public int Id { get; set; }
                public string Value { get; set; }
                public string Description { get; set; }
            }

            public class RootObject
            {
                public D d { get; set; }
            }
        }
        public class PaymentDataObject{
            public class Metadata
        {
            public string id { get; set; }
            public string uri { get; set; }
            public string type { get; set; }
        }

        public class Deferred
        {
            public string uri { get; set; }
        }

        public class Orders
        {
            public Deferred __deferred { get; set; }
        }

        public class Result
        {
            public Metadata __metadata { get; set; }
            public Orders Orders { get; set; }
            public string OrderId { get; set; }
            public string ShopGuid { get; set; }
            public string DisplayedOrderId { get; set; }
            public object InvoiceFirstName { get; set; }
            public object InvoiceLastName { get; set; }
            public object InvoiceCompanyName { get; set; }
            public object InvoiceNIP { get; set; }
            public object InvoiceAddress { get; set; }
            public object InvoicePostCode { get; set; }
            public object InvoiceCity { get; set; }
            public object InvoiceRegion { get; set; }
            public object InvoiceCountry { get; set; }
            public bool InvoiceProformaRequested { get; set; }
        }

        public class D
        {
            public List<Result> results { get; set; }
        }

        public class RootObject
        {
            public D d { get; set; }
        }
    }
        public class InvoiceDataObject
        {
            public class Metadata
            {
                public string id { get; set; }
                public string uri { get; set; }
                public string type { get; set; }
            }

            public class Deferred
            {
                public string uri { get; set; }
            }

            public class Orders
            {
                public Deferred __deferred { get; set; }
            }

            public class Result
            {
                public Metadata __metadata { get; set; }
                public Orders Orders { get; set; }
                public string OrderId { get; set; }
                public string ShopGuid { get; set; }
                public string DisplayedOrderId { get; set; }
                public object InvoiceFirstName { get; set; }
                public object InvoiceLastName { get; set; }
                public object InvoiceCompanyName { get; set; }
                public object InvoiceNIP { get; set; }
                public object InvoiceAddress { get; set; }
                public object InvoicePostCode { get; set; }
                public object InvoiceCity { get; set; }
                public object InvoiceRegion { get; set; }
                public object InvoiceCountry { get; set; }
                public bool InvoiceProformaRequested { get; set; }
            }

            public class D
            {
                public List<Result> results { get; set; }
            }

            public class RootObject
            {
                public D d { get; set; }
            }
        }
        public class ShippingDataObject
        {
            public class Metadata
            {
                public string id { get; set; }
                public string uri { get; set; }
                public string type { get; set; }
            }

            public class Deferred
            {
                public string uri { get; set; }
            }

            public class Orders
            {
                public Deferred __deferred { get; set; }
            }

            public class Result
            {
                public Metadata __metadata { get; set; }
                public Orders Orders { get; set; }
                public string OrderId { get; set; }
                public string ShopGuid { get; set; }
                public string Email { get; set; }
                public string DisplayedOrderId { get; set; }
                public string PhoneNumber { get; set; }
                public string ShippingFirstName { get; set; }
                public string ShippingLastName { get; set; }
                public object ShippingCompanyName { get; set; }
                public string ShippingAddress { get; set; }
                public string ShippingPostCode { get; set; }
                public string ShippingCity { get; set; }
                public object ShippingRegion { get; set; }
                public string ShippingCountry { get; set; }
            }

            public class D
            {
                public List<Result> results { get; set; }
            }

            public class RootObject
            {
                public D d { get; set; }
            }
        }

     
        #region Obiekty

        public class Metadata
        {
            public string id { get; set; }
            public string uri { get; set; }
            public string type { get; set; }
        }

        public class Deferred
        {
            public string uri { get; set; }
        }

        public class PaymentTypes
        {
            public Deferred __deferred { get; set; }
        }

        public class Deferred2
        {
            public string uri { get; set; }
        }

        public class OrderItems
        {
            public Deferred2 __deferred { get; set; }
        }

        public class Deferred3
        {
            public string uri { get; set; }
        }

        public class ShippingData
        {
            public Deferred3 __deferred { get; set; }
        }

        public class Deferred4
        {
            public string uri { get; set; }
        }

        public class InvoiceData
        {
            public Deferred4 __deferred { get; set; }
        }

        public class Deferred5
        {
            public string uri { get; set; }
        }

        public class OrderStates
        {
            public Deferred5 __deferred { get; set; }
        }

        public class Deferred6
        {
            public string uri { get; set; }
        }

        public class PaymentMethods
        {
            public Deferred6 __deferred { get; set; }
        }

        public class Result
        {
            public Metadata __metadata { get; set; }
            public PaymentTypes PaymentTypes { get; set; }
            public OrderItems OrderItems { get; set; }
            public ShippingData ShippingData { get; set; }
            public InvoiceData InvoiceData { get; set; }
            public OrderStates OrderStates { get; set; }
            public PaymentMethods PaymentMethods { get; set; }
            public string DisplayedOrderId { get; set; }
            public string ShopGuid { get; set; }
            public object ShopOrderId { get; set; }
            public DateTime CreatedDate { get; set; }
            public int PaymentTypeId { get; set; }
            public string ShopDeliveryFormName { get; set; }
            public string Id { get; set; }
            public string DeliveryCost { get; set; }
            public string CustomerRemark { get; set; }
            public object ShopRemark { get; set; }
            public string OrderValue { get; set; }
            public string ProductsValue { get; set; }
            public int OrderStateId { get; set; }
            public object PaymentMethodId { get; set; }
            public object DeliveryCostDiscount { get; set; }
            public object ProductsValueDiscount { get; set; }
            public object InternalRefund { get; set; }
            public object CouponNames { get; set; }
        }

        public class D
        {
            public List<Result> results { get; set; }
            public string __next { get; set; }
        }

        public class RootObject
        {
            public D d { get; set; }
        }
        #endregion

        #region ceneoBid
        public class CeneoBid
        {
            public class Bid
            {
                public string ProductId { get; set; }
                public string ProductName { get; set; }
                public string ClickRate { get; set; }
                public string MaxBid { get; set; }
                public string ClickRateWithMaxBid { get; set; }
                public string ShopProductId { get; set; }
            }

            public class Bids
            {
                public List<Bid> bid { get; set; }
            }

            public class Result
            {
                public Bids bids { get; set; }
            }

            public class Response
            {
                public string Status { get; set; }
                public Result Result { get; set; }
            }

            public class RootObject
            {
                public Response Response { get; set; }
            }
        }
        #endregion

        public static bool GetToken()
        {
            WebRequest request = WebRequest.Create("https://developers.ceneo.pl//AuthorizationService.svc/GetToken?grantType='client_credentials'");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            string clcs = "42775212-8fae-4227-ad04-bf641010480f";
            request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", clcs));

            try
            {
                WebResponse webResponse = request.GetResponse();


                Helper.SetCachedValue("CeneoAccessToken", "Ceneo", webResponse.Headers["access_token"]);

                var s = Helper.GetCachedValue("CeneoAccessToken", "Ceneo");
            }catch(Exception ex)
            {
                ErrorHandler.SendError(ex, "Błąd autoryzacji do Ceneo");
                return false;
            }
            return true;
        }

        internal static void GetOrders()
        {
            GetOrders("https://developers.ceneo.pl/BasketService.svc/Orders?$format=json&$orderby=CreatedDate desc");
        }

        public class CeneoProduct
        {
            public string CeneoProductId { get; set; }
            public string  MaxBid { get; set; }
        }
        public class ShopProduct
        {
            public int ShopProductId { get; set; }
        }
     
        List<Dal.Helper.ShopCeneo> productsAll = new List<Dal.Helper.ShopCeneo>();


        public class Categories : ImportData, IImportData
        {
            #region kategorie
            [XmlRoot(ElementName = "Category")]
            public class Category
            {
                [XmlElement(ElementName = "Id")]
                public string Id { get; set; }
                [XmlElement(ElementName = "Name")]
                public string Name { get; set; }
                [XmlElement(ElementName = "Subcategories")]
                public Subcategories Subcategories { get; set; }
            }

            [XmlRoot(ElementName = "Subcategories")]
            public class Subcategories
            {
                [XmlElement(ElementName = "Category")]
                public List<Category> Category { get; set; }
            }

            [XmlRoot(ElementName = "ArrayOfCategory")]
            public class ArrayOfCategory
            {
                [XmlElement(ElementName = "Category")]
                public List<Category> Category { get; set; }
            }
            #endregion
            Bll.CeneoApiHelper.Categories.ArrayOfCategory ceneoCategories = null;
            public new void LoadData<T>()
            {
                T data = base.LoadData<T>("https://developers.ceneo.pl/api/v3/kategorie", "CeneoKategorie");
                ProcessData(data);
                base.PostLoadProcess();
            }
            public void ProcessData<T>(T obj)
            {
                

                try
                {
                    ceneoCategories = obj as Bll.CeneoApiHelper.Categories.ArrayOfCategory;

                    List<Dal.ShopCategory> categories = new List<Dal.ShopCategory>();
                    GetCategories(categories, null, ceneoCategories.Category);


                    Dal.ShopHelper sh = new Dal.ShopHelper();
                    sh.SetCategories(Dal.Helper.ShopType.Ceneo, categories);

                }
                catch (Exception ex)
                {

                    Bll.ErrorHandler.SendError(ex, "Błąd przetawarznia drzewa kategorii Ceneo");
                }
           



            }
            private static void GetCategories(List<Dal.ShopCategory> categories, string categoryParentId, List<Category> ceneoCategories)
            {
                foreach (Category c in ceneoCategories)
                {
                    Dal.ShopCategory sc = new Dal.ShopCategory()
                    {

                        CategoryOrder = 0,
                        IsActive = true,
                        IsAllowed = c.Subcategories.Category.Count() == 0,
                        IsPublished = true,
                        Name = c.Name,
                        ShopCategoryId = c.Id,
                        Url = "",
                        ShopTypeId = (int)Dal.Helper.ShopType.Ceneo,
                        CategoryParentId = categoryParentId

                    };

                    categories.Add(sc);

                    if (c.Subcategories.Category.Count() > 0)
                        GetCategories(categories, c.Id, c.Subcategories.Category);
                }
            }
        }

        public void SetCeneoBids(Dal.Helper.Shop ceneoShop, Dal.Helper.Shop shop)
        {
            if (!GetToken())
                return;


            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<Dal.CeneoShopResult> products = sh.GetCeneoProducts(ceneoShop, shop);

            SetProductsBids(ceneoShop, products);
        }
        public void SetProductsBids(Dal.Helper.Shop ceneoShop, List<Dal.CeneoShopResult> products)
        {
            int size = 100;
            int cntr = 0;
            int cntrLimit = 50;

            int partsCount = (products.Count / size);

            for (int i = 0; i < partsCount + 1; i++)
            {
                List<Dal.CeneoShopResult> productsToTake = products.Skip(i * size).Take(size).ToList();

                if (productsToTake.Count == 0)
                    continue;


                SetProductsBidsInternal(ceneoShop, productsToTake);
                cntr++;
                if (cntr == cntrLimit)
                {
                    cntr = 0;
                    System.Threading.Thread.Sleep(60000*61);
                }
            }
        }
        internal void SetProductsBidsInternal(Dal.Helper.Shop ceneoShop, List<Dal.CeneoShopResult> products)
        {

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                // products = products.Where(x => x.Code == "1110104").ToList();

                List<CeneoProduct> ceneoProducts = new List<CeneoProduct>();

                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                nfi.CurrencyDecimalSeparator = ".";
                nfi.CurrencyGroupSeparator = "";
                nfi.CurrencySymbol = "";


                foreach (Dal.CeneoShopResult p in products)
                {
                    ceneoProducts.Add(new CeneoProduct()
                    {
                        CeneoProductId = p.CeneoShopProductId,
                        MaxBid = String.Format(nfi, "{0:00.00}", p.CeneoMaxBid)
                    });
                }

                string jsonEncodedParams = Bll.RESTHelper.ToJson(ceneoProducts);

                string url = String.Format("https://developers.ceneo.pl/api/v2/function/bidding.SetProductsBids/Call?bids={0}&apiKey={1}&resultFormatter=json&resultPlainText=true",
               HttpUtility.UrlEncode(jsonEncodedParams),
                "42775212-8fae-4227-ad04-bf641010480f"
                );


                WebRequest methodRequest = WebRequest.Create(String.Format(url));
                methodRequest.Method = "POST";
                methodRequest.ContentType = "application/json";
                methodRequest.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", Helper.GetCachedValue("CeneoAccessToken", "Ceneo")));
                methodRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");


                Stream dataStream = methodRequest.GetRequestStream();
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(jsonEncodedParams);
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                var webResponse = methodRequest.GetResponse();

                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();


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
        }
        public void SetProductsBids(Dal.Helper.Shop ceneoShop, List<Dal.CeneoShopResult> products, decimal maxBid)
        {
            int size = 100;

            int partsCount = (products.Count / size);

            for (int i = 0; i < partsCount + 1; i++)
            {
                List<Dal.CeneoShopResult> productsToTake = products.Skip(i * size).Take(size).ToList();

                if (productsToTake.Count == 0)
                    continue;


                SetProductsBidsInternal(ceneoShop, productsToTake, maxBid);
            }
        }

        public  void GetCeneoProducts(Dal.Helper.Shop ceneoShop, Dal.Helper.Shop shop)
        {
            if (!GetToken())
                return;
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Dal.ProductCatalogShopProduct> products = pch.GetProductCatalogShopProducts(shop).Where(x=>x.ShopProductId!=null).ToList();

            int size = 100;

            int partsCount = (products.Count / size);

            for (int i = 0; i < partsCount + 1; i++)
            {
                List<Dal.ProductCatalogShopProduct> productsToTake = products.Skip(i * size).Take(size).ToList();

                if (productsToTake.Count == 0)
                    continue;


                GetProductsBids(ceneoShop, shop, productsToTake);
            }
            //var p = productsAll.Where(x => x.ShopProductId.Contains(",")).ToList();
        }

        internal void SetProductsBidsInternal(Dal.Helper.Shop ceneoShop, List<Dal.CeneoShopResult> products, decimal maxBid)
        {

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


               // products = products.Where(x => x.Code == "1110104").ToList();

                List<CeneoProduct> ceneoProducts = new List<CeneoProduct>();

                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                nfi.CurrencyDecimalSeparator = ".";
                nfi.CurrencyGroupSeparator = "";
                nfi.CurrencySymbol = ""; 


                foreach (Dal.CeneoShopResult p in products)
                {
                    ceneoProducts.Add(new CeneoProduct()
                    {
                        CeneoProductId = p.CeneoShopProductId,
                        MaxBid = String.Format(nfi,   "{0:00.00}", maxBid)
                    });
                }

                string jsonEncodedParams = Bll.RESTHelper.ToJson(ceneoProducts);

                string url = String.Format("https://developers.ceneo.pl/api/v2/function/bidding.SetProductsBids/Call?bids={0}&apiKey={1}&resultFormatter=json&resultPlainText=true",
               HttpUtility. UrlEncode(jsonEncodedParams),
                "42775212-8fae-4227-ad04-bf641010480f"
                );


                WebRequest methodRequest = WebRequest.Create(String.Format(url));
                methodRequest.Method = "POST";
                methodRequest.ContentType = "application/json";
                methodRequest.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", Helper.GetCachedValue("CeneoAccessToken", "Ceneo")));
                methodRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");


                Stream dataStream = methodRequest.GetRequestStream();
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(jsonEncodedParams);
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                var webResponse = methodRequest.GetResponse();

                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();

//Object response = Bll.RESTHelper.FromJson(text);

                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //serializer.MaxJsonLength = Int32.MaxValue;
                //CeneoBid.RootObject root = serializer.Deserialize<CeneoBid.RootObject>(text);
                //ProcessBids(root, ceneoShop, shop);
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
        }
        internal  void GetProductsBids(Dal.Helper.Shop ceneoShop, Dal.Helper.Shop shop, List<Dal.ProductCatalogShopProduct> products)
        {
            try
            {


                List<ShopProduct> ceneoProducts = new List<ShopProduct>();

                foreach (Dal.ProductCatalogShopProduct p in products)
                {
                    ceneoProducts.Add(new ShopProduct()
                    {
                        ShopProductId = Int32.Parse(p.ShopProductId)
                    });
                }

                string jsonEncodedParams = Bll.RESTHelper.ToJson(ceneoProducts);

                string url = String.Format("https://developers.ceneo.pl/api/v2/function/bidding.GetProductsBidsByShopProductId/Call?products={0}&apiKey={1}&resultFormatter=json&resultPlainText=true",
                HttpUtility.UrlEncode(jsonEncodedParams),
                "42775212-8fae-4227-ad04-bf641010480f"
                );


                WebRequest methodRequest = WebRequest.Create(String.Format(url));
                methodRequest.Method = "POST";
                methodRequest.ContentType = "application/json";
                methodRequest.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", Helper.GetCachedValue("CeneoAccessToken", "Ceneo")));
                methodRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");


                Stream dataStream = methodRequest.GetRequestStream();
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(jsonEncodedParams);
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                var webResponse = methodRequest.GetResponse();

                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();

                Object response = Bll.RESTHelper.FromJson(text);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                CeneoBid.RootObject root = serializer.Deserialize<CeneoBid.RootObject>(text);
                ProcessBids(root, ceneoShop, shop);
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
        }

        
        private  void ProcessBids(CeneoBid.RootObject root, Dal.Helper.Shop ceneoShop, Dal.Helper.Shop shop)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();

            if (root.Response == null || root.Response.Result == null || root.Response.Result.bids == null || root.Response.Result.bids.bid == null)
                return;

            List<Dal.Helper.ShopCeneo> products = root.Response.Result.bids.bid.Select(x => new Dal.Helper.ShopCeneo()
            {
                Shop = shop,
                CeneoShop = ceneoShop,
                CeneoProductId = x.ProductId,
                ProductCatalogId = 0,
                ShopProductId = x.ShopProductId,
                CeneoClickRate = Decimal.Parse(x.ClickRate, CultureInfo.InvariantCulture.NumberFormat),
                CeneoClickRateWithMaxBid = Decimal.Parse(x.ClickRateWithMaxBid, CultureInfo.InvariantCulture.NumberFormat),
                CeneoMaxBid = Decimal.Parse(x.MaxBid, CultureInfo.InvariantCulture.NumberFormat)
            }).ToList();

            //productsAll.AddRange(products);
            sh.SetProductCatalogShop(products);

        }

        internal static void GetOrders(string url)
        {
            if (!GetToken())
                return;

            var methodRequest = WebRequest.CreateHttp(url);
            methodRequest.Headers["Authorization"] = "Bearer " + Helper.GetCachedValue("CeneoAccessToken", "Ceneo");

            try
            {
                var webResponse = methodRequest.GetResponse();

                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();

                Object response = Bll.RESTHelper.FromJson(text);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                RootObject root = serializer.Deserialize<RootObject>(text);
                ProcessOrders(root, Dal.Helper.Shop.Ceneo);
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
                        ErrorHandler.SendError(new Exception("Ceneo GetOrders"), text);
                    }
                }
            }
        }
        internal static string GetObjectData(string url)
        {
            var methodRequest = WebRequest.CreateHttp(url+ "?$format=json");
            methodRequest.Headers["Authorization"] = "Bearer " + Helper.GetCachedValue("CeneoAccessToken", "Ceneo");
            var webResponse = methodRequest.GetResponse();

            Stream responseStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string text = reader.ReadToEnd();
            return text;
        }
        private static void ProcessOrders(RootObject root, Dal.Helper.Shop shop)
        { 
            Dal.ShopHelper sh = new Dal.ShopHelper();


            string[] shopOrderNumbers = root.d.results.Select(x => x.DisplayedOrderId).ToArray();

            string[] orderToProcess = sh.InsertOrders(shopOrderNumbers, null, shop, "system");



            #region
            foreach (Result r in root.d.results.Where(x=> orderToProcess.Contains(x.DisplayedOrderId)).ToList())
            {
                Dal.ShopOrder so = sh.GetShopOrder(shop, r.DisplayedOrderId);

                if (so == null || (so != null && so.IsProcessed))
                    continue;

                List<Dal.OrderProduct> products = new List<OrderProduct>();
          
                Dal.Order order = null;
                 

                try
                {
                    order = new Order()
                    {
                        InsertDate = r.CreatedDate,
                        ShippingCost = Convert.ToDecimal(r.DeliveryCost,  System.Globalization.CultureInfo.InvariantCulture),
                        ShippingCostVAT = Dal.Helper.VAT,
                        ShopId = (int)Dal.Helper.Shop.Ceneo,
                        OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                        ParDate = null,
                        CompanyId = Dal.Helper.DefaultCompanyId,
                        ExternalOrderNumber = r.DisplayedOrderId.ToString(),
                        ShippingAmountCurrency = Convert.ToDecimal(r.DeliveryCost, System.Globalization.CultureInfo.InvariantCulture),
                        ShippingCurrencyRate = 1,
                        ShippingCurrencyCode = "PLN",
                       // ShipmentAddress = r.ShippingData.__deferred
                    };


                    Dal.OrderShipping orderShipping = SetShipping(shop, r, ref order);




                    GetShippingData(order, r);
                    Dal.Invoice invoice = GetInvoiceData(order, r);
                    Dal.OrderPayment op = GetPaymentDataData(order, r);
                    products.AddRange(GetOrderItems(order, r));


                   //Dal.OrderPayment op = GetOrderPayment(order, r);

                    OrderStatusHistory osh = new OrderStatusHistory()
                    {
                        Comment = String.Format("Dostawa: {0}\n\nUwagi: {1}", r.ShopDeliveryFormName, r.CustomerRemark),
                        InsertDate = DateTime.Now,
                        Order = order,
                        InsertUser = "system",
                        OrderStatusId = (int)Dal.Helper.OrderStatus.New,


                    };
                    so.IsProcessed = true;
                    sh.SetNewOrder(so, invoice, products, shop, osh, op, orderShipping);
                }
                catch(Exception ex)
                {
                    ErrorHandler.SendError(ex, String.Format("Błąd wczytywania zamówienia Ceneo {0}", r.DisplayedOrderId));

                }
            }
            #endregion
           // if (root.d.__next != null)
           //     GetOrders(root.d.__next); // tu sie wywalal limit czasu. nie wiadomo dlaczego.
        }
        private static Dal.OrderShipping SetShipping(Dal.Helper.Shop shop, Result orderCeneo, ref Dal.Order order)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<Dal.ShopShipping> ss = sh.GetShopShipping(shop);


            Dal.OrderHelper oh = new Dal.OrderHelper();


              Dal.ShippingCompany sc = oh.GetShipppingCompanies().Where(x => x.IsDefault).FirstOrDefault();
            Dal.OrderShipping orderShipping = new OrderShipping()
            {
                Order1 = order,
                COD = null,
                InsertDate = DateTime.Now,
                InsertUser = "System",
                OrderShippingStatusId = (int)Dal.Helper.OrderShippingStatus.Temporary,
                ShippingCompanyId = sc.ShippingCompanyId,
                ShippingServiceTypeId = (int)Dal.Helper.ShippingServiceType.ForOrder,
                TrackingNumberSent = false,
                IsParcelReady = false

            };

            if (orderCeneo.CustomerRemark != null && orderCeneo.CustomerRemark.Contains("Nr punktu odbioru: "))
            {
                string[] a = orderCeneo.CustomerRemark.Split('\n');

                foreach (string l in a)
                {
                    if (l.Contains("Nr punktu odbioru: "))
                        orderShipping.ServicePoint = l.Replace("Nr punktu odbioru: ", "").Trim();
                }
            }


            //int shippingTypeId = 3; /// kurier
            //// Paczkomaty InPost, Płatność z góry,Przesyłka
            //if (orderCeneo.ShopDeliveryFormName.Contains("Paczkomaty InPost, Płatność z góry"))
            //    shippingTypeId = 9;

            //order.ShippintTypeId = shippingTypeId;



            var q = ss.Where(x => x.ShopId == (int)shop && x.ShopShippingId == orderCeneo.ShopDeliveryFormName).FirstOrDefault();
            orderShipping.ShippingCompanyId = q.ShippingServiceMode.ShippingCompanyId;
            orderShipping.ShippingServiceModeId = q.ShippingServiceModeId;

            if (q.PayOnDelivery)
                orderShipping.COD = Decimal.Parse(orderCeneo.ProductsValue, System.Globalization.CultureInfo.InvariantCulture) + Decimal.Parse(orderCeneo.DeliveryCost, System.Globalization.CultureInfo.InvariantCulture);

            return orderShipping;
        }
        private static OrderPayment GetOrderPayment(Order order, Result r)
        {
            Dal.OrderPayment op = new OrderPayment();
            op.Order = order;


            string o = GetObjectData(r.PaymentMethods.__deferred.uri);


            return op;
        }

        private static List<OrderProduct> GetOrderItems(Order order, Result r)
        {
            List<OrderProduct> products = new List<OrderProduct>();

            string o = GetObjectData(r.OrderItems.__deferred.uri);
             

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            OrderItemsObject.RootObject root = serializer.Deserialize<OrderItemsObject.RootObject>(o);


            List<Dal.ProductCatalogShopProduct> productCatalog = 
                Dal.DbHelper.ProductCatalog.GetProductCatalogShopProductByShopIds(Dal.Helper.Shop.Lajtitpl, root.d.results.Select(x => x.ShopProductId).Distinct().ToArray());



            foreach (OrderItemsObject.Result p in  root.d.results)
            {
                Dal.OrderProduct product = new OrderProduct()
                {
                    ExternalProductId = Convert.ToInt32(p.ShopProductId),
                    Order = order,
                    OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New,
                    Price = Convert.ToDecimal(p.Price, System.Globalization.CultureInfo.InvariantCulture),
                    ProductCatalogId = productCatalog.Where(x => x.ShopProductId == p.ShopProductId).Select(x => x.ProductCatalogId).FirstOrDefault(),
                    ProductName = p.Name,
                    Quantity = p.Count, 
                    VAT = Dal.Helper.VAT,
                    Rebate = 0,
                    ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
                    LastUpdateDate = null,
                    CurrencyRate = 1,
                    PriceCurrency = Convert.ToDecimal(p.Price, System.Globalization.CultureInfo.InvariantCulture)
                };
                products.Add(product);
            }

            return products;
        }

        private static Dal.OrderPayment GetPaymentDataData(Order order, Result r)
        {
            string o = GetObjectData(r.PaymentTypes.__deferred.uri);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            PaymentTypeObject.RootObject root = serializer.Deserialize<PaymentTypeObject.RootObject>(o);

            //string p = GetObjectData(root.d.__metadata.uri);
            switch (root.d.Id)
            {
                case 1:
                    Dal.OrderPayment op = new OrderPayment()
                    {
                        Amount = Decimal.Parse(r.OrderValue, System.Globalization.CultureInfo.InvariantCulture),
                        InsertDate = r.CreatedDate,
                        CurrencyCode = "PLN",
                        CurrencyRate = 1,
                        InsertUser = "system",
                        PaymentTypeId = (int)Dal.Helper.OrderPaymentType.PayU23,
                        Order = order,

                    };
                    return op;
                    break;

                case 5: //Płatność bezpośrednia na konto sklepu
                    break;
                default:
                    var s = "";
                    break;

            }
            return null;
        }

        private static Dal.Invoice  GetInvoiceData(Order order, Result r)
        {
            string o = GetObjectData(r.InvoiceData.__deferred.uri);

            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            InvoiceDataObject.RootObject root = serializer.Deserialize<InvoiceDataObject.RootObject>(o);

            InvoiceDataObject.Result result = root.d.results[0];

            if (result.InvoiceCity != null)
            {
                Dal.Invoice invoice = new Invoice()
                {

                    Address = result.InvoiceAddress != null ? result.InvoiceAddress.ToString() : "",
                    City = result.InvoiceCity != null ? result.InvoiceCity.ToString() : "",
                    CompanyId = Dal.Helper.DefaultCompanyId,
                    CompanyName = result.InvoiceCompanyName != null ? result.InvoiceCompanyName.ToString() : "",
                    IsDeleted = false,
                    Nip = result.InvoiceNIP != null ? result.InvoiceNIP.ToString() : "",
                    Postcode = result.InvoicePostCode != null ? result.InvoicePostCode.ToString() : "",
                    InvoiceDate = DateTime.Now,
                    InvoiceSellDate = null,
                    Email = order.Email
                };
                order.Invoice = invoice;
                return invoice;
            }
            else
                return null;
        }

        private static void GetShippingData(Order order, Result r)
        {
            string o = GetObjectData(r.ShippingData.__deferred.uri);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            ShippingDataObject.RootObject root = serializer.Deserialize<ShippingDataObject.RootObject>(o);

            ShippingDataObject.Result result = root.d.results[0];

             
              order.ShipmentCompanyName = result.ShippingCompanyName == null ? "" : result.ShippingCompanyName.ToString(); 
            order.Email = result.Email;
            order.Phone = result.PhoneNumber;
            order.ShipmentCountryCode = "PL";// result.ShippingCountry;
            order.ShipmentAddress = result.ShippingAddress;
            order.ShipmentPostcode = result.ShippingPostCode;
            order.ShipmentCity = result.ShippingCity;
            order.ShipmentFirstName = result.ShippingFirstName;
            order.ShipmentLastName = result.ShippingLastName;
        }
    }



    public class CeneoHelper
    {

        public void GetCeneoFile()
        {

            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
            string remoteUri = "http://lajtit.pl/console/integration/execute/name/CeneoV2";

            string fileName = String.Format("Ceneo_lajtit.pl_clickweb_{0:yyyyMMddHHss}.xml", DateTime.Now);

            string saveLocation = String.Format(path, fileName);
            string saveFixedLocation = String.Format(path, "ceneo.xml");

            try
            {

                // Create a new WebClient instance.
                using (WebClient myWebClient = new WebClient())
                {
                    myWebClient.DownloadFile(remoteUri, saveLocation);

                    File.Copy(saveLocation, saveFixedLocation, true);

                    FtpHelper fh = new FtpHelper();
                    fh.UploadFileToftp(saveFixedLocation, "/");

                }
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku Ceneo z lokalizacji {0}", remoteUri));
          

            }
        }


        //public void GenerateXMLFile()
        //{  
        //    List<Dal.ShopFnResult> products = Dal.DbHelper.ProductCatalog.GetProductCatalogForShop((int)Dal.Helper.Shop.Lajtitpl);


        //    Offers offers = new Offers();
        //    offers.O = new List<O>();
        //    offers.Version = "1";
        //    products = products.Where(x => x.ShopProductId!=null ).ToList();

        //    Dal.SettingsHelper sh = new Dal.SettingsHelper();
        //    Dal.Settings s = sh.GetSetting("CENEO_DESC");

        //    Dal.ShopHelper shh = new Dal.ShopHelper();
        //    List<Dal.ShopRebate> rebates = shh.GetRebates();

        //    //products = products.Where(x => x.ProductCatalogId == 8259).ToList();
        //    foreach (ShopFnResult product in products)
        //    {
        //        string ceneoDesc = s.StringValue ?? "";

        //        O offer = new O()
        //        {

        //            Attrs = GetAttributes(product),
        //            Avail = GetAvail(product),
        //            Desc = ceneoDesc,
        //            Id = product.ShopProductId,
        //            Imgs = GetImages(product),
        //            Name = GetName(product, rebates),
        //            Price = GetPrice(product),
        //            Url = String.Format("http://lajtit.pl/pl/p/p/{0}?utm_source=ceneo&utm_medium=cpc&utm_campaign=regular", product.ShopProductId),
        //            Set = "0",
        //            Stock ="50" 
        //            //Cat = "",
                    
        //        };
        //        offers.O.Add(offer);
        //        //xml.Xsi = "http://www.w3.org/2001/XMLSchema-instance";

        //    }
        //    string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", 
        //        Dal.Helper.Env.ToString())];


        //    string saveLocation = String.Format(path, "ceneo.xml");
        //    //string saveLocation = String.Format(path, "ceneo_sys.xml");

        //    XmlSerializer xmlserializer = new XmlSerializer(typeof(Offers));
        //    Utf8StringWriter stringWriter = new Utf8StringWriter();
        //    XmlWriter writer = XmlWriter.Create(stringWriter);
            
        //    xmlserializer.Serialize(writer, offers);

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

        //private string GetName(ShopFnResult product, List<Dal.ShopRebate> rebates)
        //{

        //    //decimal rebate = 0; 

        //    //rebate = rebates.Where(x => x.AmountFrom <= product.pric &&  product.PriceBruttoShop < x.AmountTo).Select(x => x.Rebate).FirstOrDefault();

        //    //string tmpRebateLock = "= INDYWIDUALNE RABATY – ZADZWOŃ = {0} = DARMOWA WYSYŁKA OD 300 ZŁ! =";
        //    //string tmpRebate1 = "= RABAT {1:0}% W KOSZYKU = {0} = WYSYŁKA 0 ZŁ! =";
        //    //string tmpRebate0 = "= KUPUJ WIĘCEJ - RABATY DO {1:0}% W KOSZYKU = {0} = DARMOWA WYSYŁKA OD 300 ZŁ! =";

        //    string productName = "";
        //    if (product.TemplateName != null)
        //        productName = product.TemplateName;
        //    else
        //        productName = Mixer.GetProductName(5, product.ProductCatalogId);

        //    string ceneoName = "";

        //    string supplierTemplateName = product.SupplierTemplateName;

        //    //if (product.LeftQuantity > 0)
        //    //{
        //    //    ceneoName = "20% rabatu - kod MAGAZYN - wyprzedaż do 2.02";
        //    //}
        //    //else
        //    //{
        //        if (supplierTemplateName == null)
        //            ceneoName = productName;
        //        else
        //        {
        //            supplierTemplateName = supplierTemplateName.Replace("[SUPPLIER]", product.SupplierName);
        //            supplierTemplateName = supplierTemplateName.Replace("[PRODUCT]", productName);
        //            ceneoName = supplierTemplateName;
        //        }
        //    //}


        //    //if (product.OnlineShopLockRebates)
        //    //{
        //    //    if (product.TemplateName != null)
        //    //        ceneoName = productName;
        //    //    else
        //    //    {
        //    //        ceneoName = String.Format(tmpRebateLock, productName);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    if (rebate > 0)
        //    //        ceneoName = String.Format(tmpRebate1, productName, rebate * 100);
        //    //    else
        //    //        ceneoName = String.Format(tmpRebate0, productName, rebates.Max(x => x.Rebate * 100));
        //    //}

        //    return ceneoName;

        //}

        #region funkcje pomocnicze
        //private string GetAvail(ShopFnResult product)
        //{
        //    if (product.IsOnStock)
        //        return "1";

        //    switch (product.ShopDeliveryDays)
        //    {
        //        case 1:
        //            return "1"; 
        //        case 2:         
        //        case 3:         
        //            return "3"; 
        //        case 4:         
        //        case 5:         
        //        case 8:         
        //            return "7"; 
        //        default: return "14";  
        //    }
        //}

        /*

Podstawowe informacje o ofercie.
id - unikalne i niezmienne id produktu. Maksymalna ilość znaków 100. Wymagane
url - url produktu. Maksymalna ilość znaków 2048. Wymagane
price - cena produktu. Liczba zmiennoprzecinkowa, separator kropka. Wymagane

avail - dostępność produktu. Dostępne opcje [1, 3, 7, 14, 99] gdzie: 1 – dostępny, sklep wyśle
produkt w ciągu 24 godzin, 3 – sklep wyśle produkt do 3 dni, 7 – sklep wyśle produkt w ciągu
tygodnia, 14 – sklep wyśle produkt do 14 dni, 90 – towar na zamówienie, 99 – brak informacji
o dostępności – status „sprawdź w sklepie”, 110 – przedsprzedaż.
Podane wartości muszą być zgodnie ze stanem faktycznym, znacznik nie może pozostawać
pusty czy też posiadać wartość „0”. Opcjonalnie

set - zestaw. Czy oferta jest zestawem. Dostępne opcje [1, 0] gdzie; 1 – tak, oferta jest
zestawem, 0 – nie, oferta nie jest zestawem. Opcjonalnie
weight - waga. Waga oferty w kilogramach, separator kropka, nie może być podana wartość
„0” bądź puste pole . Opcjonalnie (podanie wag produktów z czasem będzie wymagane)
basket - dotyczy sklepów aktywnych w usłudze „Kup na Ceneo”. Czy oferta ma być dostępna
w Kup na Ceneo. Dostępne opcje [1, 0] gdzie; 1 – tak, oferta dostępna w Kup na Ceneo, 0 –
nie, oferta niedostępna w Kup na Ceneo. Opcjonalnie
stock - stan magazynowy. Liczna całkowita dodatnia. Pole nie może być puste. Opcjonalnie

*/
        public class Utf8StringWriter : StringWriter
        { 
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }
        private Attrs GetAttributes(ShopFnResult product)
        {
            Attrs attributes = new Attrs();
            attributes.A = new List<A>();
            attributes.A.Add(new A() { Name = "Producent", Text = product.SupplierName });
            attributes.A.Add(new A() { Name = "Kod_producenta", Text = product.Code });
            attributes.A.Add(new A() { Name = "Ean", Text = product.Ean });
            return attributes;
        }

        private Imgs GetImages(ShopFnResult product)
        {
            Imgs images = new Imgs();
            images.Main = new Main()
            {
                Url = String.Format(@"http://{0}/ProductCatalog/{1}", Dal.Helper.StaticLajtitUrl, product.FileName)

            };
            return images;
        }

        private string GetPrice(ShopFnResult product)
        {
            decimal price =product.IsActivePricePromo ? product.PriceBruttoPromo.Value : product.PriceBruttoMinimum.Value;

            return String.Format(CultureInfo.InvariantCulture, "{0:0.00}", price);
        }


        [XmlRoot(ElementName = "main")]
        public class Main
        {
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
        }

        [XmlRoot(ElementName = "i")]
        public class I
        {
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
        }

        [XmlRoot(ElementName = "imgs")]
        public class Imgs
        {
            [XmlElement(ElementName = "main")]
            public Main Main { get; set; }
            [XmlElement(ElementName = "i")]
            public List<I> I { get; set; }
        }

        [XmlRoot(ElementName = "a")]
        public class A
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "attrs")]
        public class Attrs
        {
            [XmlElement(ElementName = "a")]
            public List<A> A { get; set; }
        }

        [XmlRoot(ElementName = "o")]
        public class O
        {
            [XmlElement(ElementName = "cat")]
            public string Cat { get; set; }
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "imgs")]
            public Imgs Imgs { get; set; }
            [XmlElement(ElementName = "desc")]
            public string Desc { get; set; }
            [XmlElement(ElementName = "attrs")]
            public Attrs Attrs { get; set; }
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
            [XmlAttribute(AttributeName = "price_wholesale")]
            public string PriceWholesale { get; set; }
            [XmlAttribute(AttributeName = "is_in_promotion")]
            public string IsInPromotion { get; set; }
            [XmlAttribute(AttributeName = "price")]
            public string Price { get; set; }
            [XmlAttribute(AttributeName = "oldprice")]
            public string OldPrice { get; set; }
            [XmlAttribute(AttributeName = "avail")]
            public string Avail { get; set; }
            [XmlAttribute(AttributeName = "set")]
            public string Set { get; set; }
            [XmlAttribute(AttributeName = "weight")]
            public string Weight { get; set; }
            [XmlAttribute(AttributeName = "basket")]
            public string Basket { get; set; }
            [XmlAttribute(AttributeName = "stock")]
            public string Stock { get; set; }
            [XmlAttribute(AttributeName = "delivery_time_days")]
            public string DeliveryDays { get; set; }
        }

        [XmlRoot(ElementName = "offers")]
        public class Offers
        {
            [XmlElement(ElementName = "o")]
            public List<O> O { get; set; }
            [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Xsi { get; set; }
            [XmlAttribute(AttributeName = "version")]
            public string Version { get; set; }
        }
        #endregion
    }

}
