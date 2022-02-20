using LajtIt.Dal;
using LinqToExcel.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using static LajtIt.Bll.ShopUpdateHelper.ClickShop;

namespace LajtIt.Bll
{
    public partial class ShopRestHelper
    {
        public class Products
        {

            public class List
            {
                public string product_id { get; set; }
                public string producer_id { get; set; }
                public string code { get; set; }
                public string ean { get; set; }
            }

            public class RootProductsList
            {
                public string count { get; set; }
                public int pages { get; set; }
                public int page { get; set; }
                public List<List> list { get; set; }
            }




            #region Product

            public static Dictionary<string, object> ProcessAttributes(Dictionary<string, object> _attributes)
            {
                Dictionary<string, object> att = new Dictionary<string, object>();
                foreach (string key in _attributes.Keys)
                {
                    Dictionary<string, object> values = _attributes[key] as Dictionary<string, object>;
                    if (values != null)
                        foreach (string key1 in values.Keys)
                        {

                            string value = values[key1].ToString();

                            att.Add(key1, value);
                        }

                }

                return att;
            }

            public class Stock
            {
                //public string stock_id { get; set; }
                //public string extended { get; set; }
                public decimal price { get; set; }
                //public string price_type { get; set; }
                //public string price_buying { get; set; }
                public decimal stock { get; set; }
                //public string package { get; set; }
                //public string warn_level { get; set; }
                //public string sold { get; set; }
                //public string weight { get; set; }
                //public string weight_type { get; set; }
                //public string active { get; set; }
                //public string default { get; set; }
                //public string product_id { get; set; }
                public object availability_id { get; set; }
                public string delivery_id { get; set; }
                //public object gfx_id { get; set; }
                //public string code { get; set; }
                //public string ean { get; set; }
                //public string comp_weight { get; set; }
                //public string comp_price { get; set; }
                //public string comp_promo_price { get; set; }
                //public string price_wholesale { get; set; }
                //public string comp_price_wholesale { get; set; }
                //public string comp_promo_price_wholesale { get; set; }
                //public string price_special { get; set; }
                //public string comp_price_special { get; set; }
                //public string comp_promo_price_special { get; set; }
                //public string price_type_wholesale { get; set; }
                //public string price_type_special { get; set; }
                //public object calculation_unit_id { get; set; }
                //public string calculation_unit_ratio { get; set; }
            }

            public class PlPL
            {
                //public string translation_id { get; set; }
                //public string product_id { get; set; }
                public string name { get; set; }
                public string short_description { get; set; }
                public string description { get; set; }
                public string active { get; set; }
                //public string lang_id { get; set; }
                //public string isdefault { get; set; }
                //public string seo_title { get; set; }
                //public string seo_description { get; set; }
                //public string seo_keywords { get; set; }
                public string order { get; set; }
                //public string main_page { get; set; }
                //public string main_page_order { get; set; }
                //public string booster { get; set; }
                //public string seo_url { get; set; }
                //public string permalink { get; set; }
            }

            public class Translations
            {
                public PlPL pl_PL { get; set; }
            }



            public class PlPL2
            {
                public string translation_id { get; set; }
                public string gfx_id { get; set; }
                public string name { get; set; }
                public string lang_id { get; set; }
            }

            public class Translations2
            {
                public PlPL2 pl_PL { get; set; }
            }

            public class MainImage
            {
                public string gfx_id { get; set; }
                public string name { get; set; }
                public string unic_name { get; set; }
                public string order { get; set; }
                public string hidden { get; set; }
                public Translations2 translations { get; set; }
            }

            public class Product
            {
                private Dictionary<string, object> _attributes = new Dictionary<string, object>();

                public int[] related { get; set; }
                public string product_id { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string producer_id { get; set; }
                //public string group_id { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string tax_id { get; set; }
                //public string add_date { get; set; }
                // public string edit_date { get; set; }
                //public string other_price { get; set; }
                //public string pkwiu { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string unit_id { get; set; }
                //public string in_loyalty { get; set; }
                //public object loyalty_score { get; set; }
                //public object loyalty_price { get; set; }
                //public string bestseller { get; set; }
                //public string newproduct { get; set; }
                public string dimension_w { get; set; }
                public string dimension_h { get; set; }
                public string dimension_l { get; set; }
                public string vol_weight { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public object currency_id { get; set; }
                public string gauge_id { get; set; }
                //public string unit_price_calculation { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string type { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int? category_id { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public List<int> categories { get; set; }
                //public object promo_price { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string code { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string ean { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public Stock stock { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public Translations translations { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public List<object> options { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public Dictionary<string, object> attributes
                {
                    get { return _attributes; }
                    set { _attributes = value; }
                }
                //public MainImage main_image { get; set; }
                //public bool is_product_of_day { get; set; }
                //public List<string> feeds_excludes { get; set; }
                //public List<object> tags { get; set; }

                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public SpecialOffer special_offer { get; set; }

            }
            public class SpecialOffer
            {

                //public string active { get; set; }
                //public int promo_id { get; set; }
                public decimal discount { get; set; }
                //public decimal discount_wholesale { get; set; }
                //public decimal discount_special { get; set; }
                public string date_from { get; set; }
                public string date_to { get; set; }
                //public int product_id { get; set; }
            }
            #endregion

            #region REST

            public static Product GetProductFromShop(Dal.Helper.Shop shop, string shopProductId, int productCatalogId)
            {

                Product productFromDb = GetProductFromDb(shop, productCatalogId);

                if (productFromDb != null)
                    return productFromDb;

                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/products/{0}", shopProductId), "GET");

                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    Product product = json_serializer.Deserialize<Product>(text);
                    product.attributes = ProcessAttributes(product.attributes);

                    //Dal.ShopHelper sh = new Dal.ShopHelper();

                    //sh.SetProductCatalogShopProductJson(shop, productCatalogId, Bll.RESTHelper.ToJson(product));

                    return product;

                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop, String.Format("ShopProductId: {0}", shopProductId));
                    return null;
                }
            }

            //public void SetProductUpdate(Dal.Helper.Shop shop, List<Dal.ProductCatalogShopUpdateSchedule> schedules, int productCatalogId)
            //{
            //    ProductCatalogShopProduct psp;
            //    bool createProduct;
            //    Product productToProcess =  GetProductToProcess(shop, schedules, productCatalogId, out psp, out createProduct);


            //    try
            //    {
            //        HttpWebRequest request;

            //        if (createProduct == false)
            //            request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/products/{0}", psp.ShopProductId), "PUT");
            //        else
            //            request = ShopRestHelper.GetHttpWebRequest(shop, "/webapi/rest/products", "POST");


            //        Stream dataStream = request.GetRequestStream();
            //        string jsonEncodedParams = Bll.RESTHelper.ToJson(productToProcess);
            //        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            //        byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

            //        dataStream.Write(byteArray, 0, byteArray.Length);
            //        dataStream.Close();

            //        WebResponse webResponse = request.GetResponse();

            //        Stream responseStream = webResponse.GetResponseStream();

            //        StreamReader reader = new StreamReader(responseStream);

            //        string text = reader.ReadToEnd();
            //    }

            //    catch (WebException ex)
            //    {
            //        ProcessException(ex, shop, String.Format("PcId: {0}", productCatalogId));
            //    }
            //}

            public static void GetAllProducts(Dal.Helper.Shop shop)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                sh.SetShopProductsTruncate();

                GetAllProducts(shop, 1);
            }
            private static void GetAllProducts(Dal.Helper.Shop shop, int pageId)
            {
                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/products?page={0}&limit=50", pageId), "GET");

                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    RootProductsList product = json_serializer.Deserialize<RootProductsList>(text);

                    SaveProducts(shop, product);

                    if (product.page < product.pages)
                        GetAllProducts(shop, pageId + 1);


                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop);

                }
            }

            private static void SaveProducts(Dal.Helper.Shop shop, RootProductsList product)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();


                sh.SetShopProducts(product.list.Select(x => new Dal.ShopProduct()
                {
                    Code = x.code,
                    Ean = x.ean,
                    ShopProductId = Int32.Parse(x.product_id),
                    ShopId = (int)shop,
                    Name = "",
                    Url = ""
                }).ToList());
            }
            #endregion


            #region funkcje pomocnicze
            public static List<UpdateResult> Process(List<Dal.ProductCatalogShopUpdateSchedule> schedules, Guid processId)
            {
                int[] shopIds = schedules.Select(x => x.ShopId).Distinct().ToArray();

                List<UpdateResult> results = new List<UpdateResult>();

                Console.WriteLine(String.Format("Liczba sklepw {0}", shopIds.Length));
                foreach (int shopId in shopIds)
                {
                    int[] updateTypeIds = schedules.Select(x => x.UpdateTypeId.Value).Distinct().ToArray();

                    Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopId);


                    Dal.ShopUpdateHelper suh = new Dal.ShopUpdateHelper();
                    Dal.ShopHelper pch = new Dal.ShopHelper();

                    Console.WriteLine(String.Format("Liczba updatow {0}", updateTypeIds.Length));
                    foreach (int updateTypeId in updateTypeIds)
                    {
                        List<Dal.ProductCatalogShopUpdateSchedule> sch = schedules
                            .Where(x => x.ShopId == shopId && x.UpdateTypeId.Value == updateTypeId)
                            .ToList();
                        int[] productCatalogIds = sch.Select(x => x.ProductCatalogId).Distinct().ToArray();
                        //List<Dal.ProductCatalogFnResult> pcViews = pch.GetProductCatalogShopProduct(shopId, productCatalogIds);

                        switch ((Dal.Helper.UpdateScheduleType)Enum.Parse(typeof(Dal.Helper.UpdateScheduleType), updateTypeId.ToString()))
                        {
                            case Dal.Helper.UpdateScheduleType.OnlineShopBatch:
                                Console.WriteLine(String.Format("Liczba produktow batch {0}", productCatalogIds.Length));

                                SetProductsUpdateBatch(shop, sch, productCatalogIds);
                                break;

                            case Dal.Helper.UpdateScheduleType.OnlineShopSingle:
                                Console.WriteLine(String.Format("Liczba produktow single {0}", productCatalogIds.Length));
                                SetProductsUpdateSingle(shop, sch, productCatalogIds);
                                break;
                        };
                    }
                }
                return results;
            }
            private static Product GetProductFromDb(Dal.Helper.Shop shop, int productCatalogId)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();

                Dal.ProductCatalogShopProduct psp = sh.GetShopProduct(shop, productCatalogId);

                if (String.IsNullOrEmpty(psp.JsonProduct))
                    return null;

                var json_serializer = new JavaScriptSerializer();

                try
                {
                    Product product = json_serializer.Deserialize<Product>(psp.JsonProduct);

                    return product;
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex,
                        String.Format("Nie można utworzyć obiektu Product z zapisanego json w DB. Sklep: {0}, ProductCatalogId: {1}", shop, productCatalogId));


                    return null;
                }


            }

            public static Bulk.BulkResult SetProductsUpdateSingle(Dal.Helper.Shop shop, List<Dal.ProductCatalogShopUpdateSchedule> schedules, int[] productCatalogIds)
            {
                Bulk.BulkResult bulkResult = new Bulk.BulkResult();
                int[] shopColumnTypeIds = schedules.Select(x => x.ShopColumnTypeId).Distinct().ToArray();

                foreach (int shopColumnTypeId in shopColumnTypeIds)
                {
                    Dal.Helper.ShopColumnType shopColumnType = (Dal.Helper.ShopColumnType)Enum.ToObject(typeof(Dal.Helper.ShopColumnType), shopColumnTypeId);

                    switch (shopColumnType)
                    {
                        case Dal.Helper.ShopColumnType.Images:
                            ShopRestHelper.ProductsImages.SetImages(shop, productCatalogIds);
                            break;
                        case Dal.Helper.ShopColumnType.Price:
                        case Dal.Helper.ShopColumnType.PricePromo:
                            //foreach (int productCatalogId in productCatalogIds) // hack bo trzeba wywołac bulk tylko z jednym produktem.
                            //    ShopRestHelper.Products.SetProductsUpdateInternal(shop, schedules, new int[] { productCatalogId });

                            ShopRestHelper.Products.SetProductsUpdateInternal(shop, schedules, productCatalogIds);
                            break;


                    }

                }

                return bulkResult;
            }
            public static Bulk.BulkResult SetProductsUpdateBatch(Dal.Helper.Shop shop, List<Dal.ProductCatalogShopUpdateSchedule> schedules, int[] productCatalogIds)
            {
                Bulk.BulkResult bulkResult = new Bulk.BulkResult();

                int count = 25;

                // if (schedules.Where(x => x.ShopColumnTypeId == 11).Count() != 0)
                //     count = 1; // hack bo sie wywala przy bulk dla promocji


                int partsCount = (productCatalogIds.Length / count);

                Console.WriteLine("Liczba czesci {0}", partsCount);
                for (int i = 0; i < partsCount + 1; i++)
                {
                    int[] productCatalogIdsPart = productCatalogIds.Skip(i * count).Take(count).ToArray();

                    Console.WriteLine("Produkty {0}", String.Join(",", productCatalogIdsPart));

                    if (productCatalogIdsPart.Length == 0)
                        continue;

                    bulkResult.Append(SetProductsUpdateInternal(shop, schedules, productCatalogIdsPart));
                }

                return bulkResult;
            }
            private static Bulk.BulkResult SetProductsUpdateInternal(Dal.Helper.Shop shop, List<Dal.ProductCatalogShopUpdateSchedule> schedules, int[] productCatalogIds)
            {
                List<Bulk.BulkObject> bulkObjects = new List<Bulk.BulkObject>();

                List<string> shopProductIds = new List<string>();

                foreach (int productCatalogId in productCatalogIds)
                {
                    try
                    { 
                    ProductCatalogShopProduct psp;
                    bool createProduct;

                    List<Dal.ProductCatalogShopUpdateSchedule> s = schedules.Where(x => x.ProductCatalogId == productCatalogId).ToList();

                    Product productToProcess = GetProductToProcess(shop, s, productCatalogId, out psp, out createProduct);

                    if (productToProcess == null)
                        continue;

                    string id;
                    if (createProduct)
                        id = String.Format("product-insert_PcId-{0}_PId-{1}", productCatalogId, "");
                    else
                    {
                        id = String.Format("product-update_PcId-{0}_PId-{1}", productCatalogId, psp.ShopProductId);

                    }


                    Console.WriteLine(String.Format("Produkt: {0}, createProduct {1}", productCatalogId, createProduct));


                    bulkObjects.Add(
                        new Bulk.BulkObject()
                        {
                            id = id,
                            body = productToProcess,
                            method = createProduct ? "POST" : "PUT",
                            path = createProduct ? "/webapi/rest/products" : String.Format("/webapi/rest/products/{0}", psp.ShopProductId)
                        });

                    if (Bll.ShopUpdateHelper.CanUpdateField(false, s, Dal.Helper.ShopColumnType.Price)
                        || Bll.ShopUpdateHelper.CanUpdateField(false, s, Dal.Helper.ShopColumnType.PricePromo))
                    {
                        Console.WriteLine(String.Format("Dal.Helper.ShopColumnType.Price"));
                        shopProductIds.Add(psp.ShopProductId);
                    }
                    }
                    catch(Exception ex)
                    {

                        throw ex;
                    }

                }

                Console.WriteLine(String.Format("out"));
                if (shopProductIds.Count() > 0)
                    Bll.ShopRestHelper.SpecialOffers.SetPromoFindAndDelete(shop, shopProductIds.ToArray());

                Console.WriteLine(String.Format("ShIds: {0}", String.Join(",", shopProductIds.ToArray())));
                Bulk.BulkResult result = Bulk.Sent(shop, bulkObjects);


                Console.WriteLine(result.errors);
                

                foreach (Bulk.Item item in result.items)
                {
                    string[] p = item.id.Split(new char[] { '_' });

                    switch (p[0])
                    {
                        case "product-insert":

                            if (item.code == 200)
                            {
                                int productCatalogId = Int32.Parse(p[1].Split(new char[] { '-' })[1]);
                                string shopProductId = item.body.ToString();// p[2].Split(new char[] { '-' })[1];
                                Console.WriteLine(String.Format("Insert PcId:{0}, ShId: {1}", productCatalogId, shopProductId));

                                Dal.DbHelper.ProductCatalog.SetShopProductToProductCatalogById(shop, productCatalogId, shopProductId, true, false);
                            }
                            break;
                    }


                }


                return result;

            }
            private static Product GetProductToProcess(Dal.Helper.Shop shop,
             List<ProductCatalogShopUpdateSchedule> schedules,
             int productCatalogId,
             out ProductCatalogShopProduct psp,
             out bool createProduct)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                psp = sh.GetProductCatalogShopProduct((int)shop, productCatalogId);
                createProduct = psp.ShopProductId == null;
                //Product productFromShop = null;

               // if (createProduct == false)
               //     productFromShop = GetProductFromShop(shop, psp.ShopProductId, productCatalogId);




                Product productFromCatalog = GetProductFromCatalog(shop, productCatalogId);

                if (productFromCatalog == null)
                {
                    Console.WriteLine("ProductCatalogFromCatalog {0} nie istnieje", productCatalogId);
                    return null;
                }


                //if (productFromShop == null && productFromCatalog == null)
                //{
                //    ProcessException(new WebException(String.Format("Nie udało się utworzyć produktu: PcId: {0}", productCatalogId)), shop);
                //    return null;
                //}

                Product productToProcess = null;
                if (createProduct == false) // jeśli produkt istnieje to go aktulizujemy
                    productToProcess = UpdateProduct(shop, schedules, productFromCatalog, psp.ShopProductId);
                else
                    productToProcess = productFromCatalog;

                if (createProduct)
                {
                    CheckProductCode(shop, productCatalogId, productToProcess);
                }

                if (createProduct == false)
                    sh.SetProductCatalogShopProductJson(shop, productCatalogId, Bll.RESTHelper.ToJson(productToProcess));

                Console.WriteLine("productToProcess {0}", productToProcess.ToString());
                return productToProcess;
            }

            private static void CheckProductCode(Dal.Helper.Shop shop, int productCatalogId, Product productToProcess)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();

                string code = sh.CheckProductCode(shop, productCatalogId, true);

                productToProcess.code = code;
            }

            private static Product UpdateProduct(Dal.Helper.Shop shop, List<Dal.ProductCatalogShopUpdateSchedule> schedules, Product productFromCatalog, string shopProductId)
            {
                //if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Dev)
                //{
                //    schedules.Clear();
                //    schedules.Add(
                //        new ProductCatalogShopUpdateSchedule()
                //        {
                //            ShopColumnTypeId = 12
                //        });
                //    schedules.Add(
                //        new ProductCatalogShopUpdateSchedule()
                //        {
                //            ShopColumnTypeId = 3
                //        });
                //    schedules.Add(
                //        new ProductCatalogShopUpdateSchedule()
                //        {
                //            ShopColumnTypeId = 5
                //        });
                //}


                Product productFromShop = new Product();
                if (Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.All))
                {
                    productFromShop.producer_id = productFromCatalog.producer_id;
                    productFromShop.code = productFromCatalog.code;
                    productFromShop.gauge_id = productFromCatalog.gauge_id;

                    productFromShop.vol_weight = productFromCatalog.gauge_id;
                }
                else
                {
                    productFromShop.producer_id = null;
                    productFromShop.code = null;
                }
                if (Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Category)
                    || Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Status))
                {
                    productFromShop.categories = productFromCatalog.categories;
                    productFromShop.category_id = productFromCatalog.category_id;
                }
                else
                {
                    productFromShop.categories = null;
                    productFromShop.category_id = null;
                }
                if (Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Attributes))
                    productFromShop.attributes = productFromCatalog.attributes;
                else
                    productFromShop.attributes = null;


                if (Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Ean))
                    productFromShop.ean = productFromCatalog.ean;
                else
                    productFromShop.ean = null;

                if (Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Related))
                    productFromShop.related = productFromCatalog.related;
                else
                    productFromShop.related = null;

                if (Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Price)
                     || Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Quantity)
                    || Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Delivery)
                    || Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Status)
                    || Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.PricePromo))
                {
                    productFromShop.stock = productFromCatalog.stock;
                }
                else
                    productFromShop.stock = null;

                if (Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Price)
                    || Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.PricePromo))
                {
                    //    if (
                    //    (productFromShop.special_offer != null && productFromCatalog.special_offer == null)
                    //    ||
                    //    (
                    //    productFromShop.special_offer != null && productFromCatalog.special_offer != null
                    //    &&
                    //        (
                    //        productFromShop.special_offer.date_to != productFromCatalog.special_offer.date_to
                    //        ||
                    //        productFromShop.special_offer.discount != productFromCatalog.special_offer.discount
                    //        )
                    //    )
                    //    )
                    //{
                    //Bll.ShopRestHelper.SpecialOffers.SetPromoFindAndDelete(shop, shopProductId);

                    productFromShop.special_offer = productFromCatalog.special_offer;
                }
                else
                    productFromShop.special_offer = null;
                



                //if (schedules.Where(x => x.ShopColumnTypeId == 11).Count() == 0)
                //    productFromShop.special_offer = null; // hack bo sie wywala przy bulk dla promocji


                if (Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Name)
                    || Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Description))
                    productFromShop.translations = productFromCatalog.translations;
                else
                    productFromShop.translations = null;

                return productFromShop;
            }

            private static Product GetProductFromCatalog(Dal.Helper.Shop shop, int productCatalogId)
            {

                Dal.ShopHelper sh = new Dal.ShopHelper();
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                Dal.ProductCatalogGroupHelper pgch = new ProductCatalogGroupHelper();
                #region Listy

                Dal.ProductCatalogFnResult pc = Dal.DbHelper.ProductCatalog.GetProductCatalogShopProduct((int)shop, new int[] { productCatalogId }).FirstOrDefault();

                Dal.SupplierShop supplierShop = sh.GetSupplierShop((int)shop, pc.SupplierId);


                if (supplierShop.IsActive == false && pc.LeftQuantity <=0 )
                {
                    Bll.ErrorHandler.SendError(
                        new Exception(String.Format("Nie można eksportować <a href='http://192.168.0.107/ProductCatalog.Preview.aspx?id={1}'>produktu</a> do sklepu {0}", supplierShop.Shop.Name, productCatalogId))
                        , "");
                    return null;
                }
                if (supplierShop.Shop.ShopCategory == null)
                    throw new Exception(String.Format("Dla sklepu {0} nie została zdefiniowana kategoria Archiwum", supplierShop.Shop.Name));

                int archiveCategoryId = Int32.Parse(supplierShop.Shop.ShopCategory.ShopCategoryId);

                Dal.Helper.ShopType shopType = (Dal.Helper.ShopType)Enum.ToObject(typeof(Dal.Helper.ShopType), supplierShop.Shop.ShopTypeId);


                List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources(shopType);
                List<Dal.SupplierShop> producers = sh.GetSupplierShop((int)shop);
                List<Dal.ProductCatalogRelatedByFamilyFnResult> products = pgch.GetProductCatalogsByProductFamily(shop, pc.ProductCatalogId);

                #endregion
                #region Wartości



                int? producerId = producers.Where(x => x.SupplierId == pc.SupplierId).Select(x => x.ShopProducer.ShopProducerId).FirstOrDefault();

                if (producerId == null)
                    throw new Exception(String.Format("Dla sklepu {0} nie został zdefiniowany producent {1}", supplierShop.Shop.Name, supplierShop.Supplier.Name));



                string code = String.Format("{0} {1}", pc.Code, pc.CodeAddNumber).Trim();

                int deliveryTime = Bll.Helper.GetSetting("SHOPMINDEL").IntValue.Value;
                int shopDeliveryTimeId = GetDeliveryTime(pc.LeftQuantity, GetDeliveryIdForShop(pc.DeliveryId, sources), deliveryTime);
                decimal p = 0.01M;
                decimal price = 0;
                if (!pc.OnlineShopLockRebates || pc.IsActivePricePromo)
                    price = pc.PriceBrutto.Value;//'] (double) - cena wariantu podstawowego
                else
                    price = pc.PriceBrutto.Value + p;//'] (double) - cena wariantu podstawowego


                #endregion

                Stock stock = new Stock()
                {
                    //active = "1", // zawsze aktywny, zmieniamy jedynie dostępność availability_id
                    availability_id = GetAvailability(pc),
                    //calculation_unit_id = null,
                    //calculation_unit_ratio = "0",
                    //code = code,
                    delivery_id = shopDeliveryTimeId.ToString(),
                    price = price,
                    stock = GetStock(pc),
                    //ean = pc.Ean,
                    //comp_price = price.ToString(),
                    //comp_price_special = price.ToString(),
                    //comp_price_wholesale = price.ToString(),
                    //comp_promo_price = price.ToString(),
                    //comp_promo_price_special = price.ToString(),
                    //comp_promo_price_wholesale = price.ToString(),
                    //comp_weight = "0",
                    //extended = "0",
                    //package = "0",
                    //price_buying = "0.00",
                    //price_special = price.ToString(),
                    //price_type = "1",
                    //price_type_special = "0",
                    //price_type_wholesale = "0",
                    //price_wholesale = price.ToString(),
                    //gfx_id = null,
                    //sold=null



                };


                Translations translations = new Translations()
                {
                    pl_PL = new PlPL()
                    {
                        name = GetName(shop, pc),
                        order = (pc.ShopProductPriority ?? pc.ProductCatalogId).ToString(),
                        active = "1",
                        description = pc.LongDescription,
                        short_description = pc.ShortDescription,

                    }
                };
                List<object> options = new List<object>();
                //Attributes attributes = null;// new Attributes();

                int[] related = products
                        .Where(x => x.ShopProductId != null)
                        .Take(20)
                        .Select(x => Int32.Parse(x.ShopProductId))
                        .Distinct()
                        .ToArray();


                MainImage main_image = new MainImage();

                List<object> feeds_excludes = new List<object>();
                List<object> tags = new List<object>();
                SpecialOffer special_offer = GetSpecialOffer(pc);



                List<Dal.ProductCatalogAttributeCategoryFnResult> productShopCategories = pch.GetShopProductAndCategoriesFromAttributes((int)shop, productCatalogId);

                int mainCategoryId = GetMainCategory(productShopCategories, pc, archiveCategoryId);

                List<int> otherCategoryIds = new List<int>();

                if (pc.IsPSActive)
                    otherCategoryIds = productShopCategories.Where(x => x.CategoryId != mainCategoryId.ToString())
                        .Select(x => Int32.Parse(x.CategoryId)).ToList();



                Product product = new Product()
                {
                    attributes = GetShopAttributes(shop, pc.ProductCatalogId),
                    categories = otherCategoryIds,
                    stock = stock,
                    translations = translations,
                    options = options,
                    related = related,
                    //main_image = main_image,
                    //feeds_excludes = feeds_excludes,
                    // tags = tags,
                    special_offer = special_offer,
                    //bestseller = "0",
                    //add_date = null,
                    category_id = mainCategoryId,
                    code = code,
                    currency_id = 1,
                    //dimension_h = "0",
                    //dimension_l = "0",
                    //dimension_w = "0",
                    ean = pc.Ean,
                    gauge_id = pc.ShopPackingPricingTypeId,
                    //group_id = null,
                    //in_loyalty = null,
                    //loyalty_price = null,
                    //loyalty_score = null,
                    //is_product_of_day = false,
                    //other_price = null,
                    //pkwiu = null,
                    producer_id = producerId.ToString(),
                    // product_id=0,
                    tax_id = "1",
                    type = "1",
                    unit_id = "1",
                    //unit_price_calculation = null,
                    //vol_weight = null

                };

                return product;
            }
            private static int GetMainCategory(List<Dal.ProductCatalogAttributeCategoryFnResult> productShopCategories, ProductCatalogFnResult pc, int archiveCategoryId)
            {
                if (pc.IsPSActive)
                {
                    if (productShopCategories.Where(x => x.MainCategoryPriority > 0).Count() > 0)
                        return productShopCategories.OrderByDescending(x => x.MainCategoryPriority)
                            .Select(x => Int32.Parse(x.CategoryId)).FirstOrDefault();
                    else
                        return productShopCategories.Where(x => x.IsMainCategory == 1).Select(x => Int32.Parse(x.CategoryId)).FirstOrDefault();
                }
                else
                {

                    return archiveCategoryId; // Archiwum
                }
            }

            private static Dictionary<string, object> GetShopAttributes(Dal.Helper.Shop shop, int productCatalogId)
            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                Dal.ShopHelper sh = new Dal.ShopHelper();



                List<Dal.ProductCatalogAttributeGroupsForShopResult> accessibleAttributes = sh.GetProductCatalogAttributeGroupsForShop(shop, productCatalogId)
                    .Where(x => (x.IsDefault.HasValue && x.IsDefault.Value) || !x.IsDefault.HasValue) // specycika clickshop gdzie podajemy tylko jedną wartość
                    .ToList();

                Dictionary<string, object> d = new Dictionary<string, object>();

                foreach (Dal.ProductCatalogAttributeGroupsForShopResult attribute in accessibleAttributes)
                {
                    if (attribute.IsDefault.HasValue == false)
                    {
                        d.Add(attribute.ExternalShopAttributeId.ToString(), GetAttributeValue(attribute));
                    }
                    else
                    {
                        d.Add(attribute.ExternalShopAttributeId.ToString(), attribute.AttName);
                    }
                }
                //switch (attribute.ProductCatalogAttribute.ProductCatalogAttributeGroup.AttributeGroupTypeId)
                //{
                //    case 1:
                //    case 2:
                //        d.Add(attribute.ProductCatalogAttribute.ProductCatalogAttributeGroup.ShopAttributeId.ToString(), attribute.ProductCatalogAttribute.Name); break;
                //    case 3:
                //        d.Add(attribute.ProductCatalogAttribute.ShopAttributeId.ToString(), GetAttributeValue(attribute));
                //        break;
                //}

                return d;

            }
            private static string GetAttributeValue(ProductCatalogAttributeGroupsForShopResult attribute)
            {
                switch (attribute.AttributeTypeId)
                {
                    case 1:
                        return String.Format("{0:0.##}", attribute.DecimalValue);
                    case 2:
                        return String.Format("{0}", attribute.StringValue);

                }
                return null;
            }
            private static SpecialOffer GetSpecialOffer(ProductCatalogFnResult pc)
            {
                SpecialOffer so;
                if (pc.IsActivePricePromo || pc.OnlineShopLockRebates)
                {
                    so = new SpecialOffer();
                    if (pc.OnlineShopLockRebates && !pc.IsActivePricePromo)
                    {
                        so.date_from = String.Format("{0:yyyy-MM-dd}", DateTime.Now); //(string) - data(w formacie yyyy - mm - dd) rozpoczęcia promocji
                        so.date_to = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddYears(5)); //(string) - data(w formacie yyyy - mm - dd) zakończenia promocji
                        so.discount = 0.01M;
                        //so.discount_special = 0.01M;
                        //so.discount_wholesale = 0.01M;
                    }
                    else // jeśli blokjemy rabaty to trzeba ustawic cenę promocyjną jako cenę regularną. Hack
                    {
                        so.date_from = String.Format("{0:yyyy-MM-dd}", DateTime.Now); //(string) - data(w formacie yyyy - mm - dd) rozpoczęcia promocji
                        so.date_to = String.Format("{0:yyyy-MM-dd}", pc.PriceBruttoPromoDate.Value); //(string) - data(w formacie yyyy - mm - dd) zakończenia promocji
                        so.discount = pc.PriceBrutto.Value - pc.PriceBruttoPromo.Value;
                        // so.discount_special = pc.PriceBruttoShop.Value - pc.PriceBruttoPromo.Value;
                        //so.discount_wholesale = pc.PriceBruttoShop.Value - pc.PriceBruttoPromo.Value;
                    }



                }
                else
                {
                    return null;
                }
                return so;
            }

            public static string GetName(Dal.Helper.Shop shop, Dal.ProductCatalogFnResult product)
            {
                string name = product.Name;
                if (!String.IsNullOrEmpty(name))
                {
                    name = product.Name;

                }
                else
                    name = Mixer.GetProductName((int)shop, product.ProductCatalogId);


                return name.Substring(0, Math.Min(75, name.Length));


            }
            public static int GetStock(ProductCatalogFnResult pc)
            {
        //        pc.SupplierQuantity, pc.LeftQuantity, pc.IsPsAvailable, pc.IsDiscontinued
                int defaultQuantity = 50;

                if (pc.IsDiscontinued)
                    if (pc.LeftQuantity > 0)
                    {
                        return pc.LeftQuantity;
                    }
                //else
                //{ return 0; }

                if (pc.SupplierQuantity.HasValue && pc.SupplierQuantity.Value > 0)
                    return pc.SupplierQuantity.Value + pc.LeftQuantity;



                // jeśli produkt jest na magazynie nie jest dostępny u dostawcy, to wystaw tylko ilość 
                // z naszego magazynu. W przeciwnym razie wystaw więcej produktów.
                if (pc.LeftQuantity > 0 && !pc.IsAvailable)
                {
                    return pc.LeftQuantity;
                }
                else
                {
                    return defaultQuantity;
                }

            }
            public static int? GetDeliveryIdForShop(int deliveryId, List<Dal.SupplierDeliveryTypeSource> sources)
            {

                return sources.Where(x => x.DeliveryId == deliveryId).Select(x => x.ExternalValue).FirstOrDefault();
            }
            public static int GetDeliveryTime(int leftQuantity, int? shopDeliveryDays, int deliveryTime)
            {
                if (leftQuantity > 0)
                    return deliveryTime;//'] (int]) - identyfikator czasu dostawy wariantu
                else
                {
                    if (shopDeliveryDays.HasValue)
                        return shopDeliveryDays.Value;//'] (int]) - identyfikator czasu dostawy wariantu
                    else
                        return 5;//'] (int]) - identyfikator czasu dostawy wariantu
                }


            }
            private static int GetAvailability(ProductCatalogFnResult pc)
            {
                if (pc.IsPSActive)// || (pc.IsAvailableOnline && pc.LeftQuantity > 0) || (pc.IsDiscontinued && pc.LeftQuantity > 0))
                    return 4; // dostępny

                if (pc.IsDiscontinued)
                    return 7; //wycofany z oferty

                else
                    return 3; // spodziewana dostawa
            }
            #endregion
        }
    }
}