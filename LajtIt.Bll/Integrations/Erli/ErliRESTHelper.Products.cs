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
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static LajtIt.Bll.ShopHelper;

namespace LajtIt.Bll
{
    public partial class ErliRESTHelper
    {
        public class Products
        {
            #region Product
            public class Item
            {
                public string type { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string content { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string url { get; set; }
            }

            public class Section
            {
                public List<Item> items { get; set; }
            }

            public class Description
            {
                public List<Section> sections { get; set; }
            }

            public class ExternalAttribute
            {
                public string id { get; set; }
                public string type { get; set; }
                public List<object> values { get; set; }
                public string source { get; set; }
                public int index { get; set; }
            }

            public class Breadcrumb
            {
                public string name { get; set; }
                public string id { get; set; }
            }

            public class ExternalCategory
            {
                public string source { get; set; }
                public List<Breadcrumb> breadcrumb { get; set; }
                public int index { get; set; }
            }

            public class Image
            {
                public string url { get; set; }
            }

            public class DispatchTime
            {
                public int period { get; set; }
                public string unit { get; set; }
            }

            public class Packaging
            {
                public List<object> tags { get; set; }
            }

        
            public class Frozen
            {
                public bool name { get; set; }
                public bool description { get; set; }
                public bool ean { get; set; }
                public bool sku { get; set; }
                public bool externalAttributes { get; set; }
                public bool externalCategories { get; set; }
                public bool externalVariantGroup { get; set; }
                public bool images { get; set; }
                public bool files { get; set; }
                public bool price { get; set; }
                public bool stock { get; set; }
                public bool status { get; set; }
                public bool dispatchTime { get; set; }
                public bool packaging { get; set; }
                public bool obligatoryIdentifier { get; set; }
                public bool voluntaryIdentifier { get; set; }
                public bool returnIdentifier { get; set; }
            }
            public class Files
            {
                public string url { get; set; }
            }
            public class RootProduct
            {
                public string externalId { get; set; }
                public string name { get; set; }
                public Description description { get; set; }
                //public string externalDescription { get; set; }
                public string ean { get; set; }
                public string sku { get; set; }
                public List<ExternalAttribute> externalAttributes { get; set; }
                public List<ExternalCategory> externalCategories { get; set; }
                public List<Image> images { get; set; }
                public List<Files> files { get; set; }
                public int price { get; set; }
                public int stock { get; set; }
                public string status { get; set; }
                public DispatchTime dispatchTime { get; set; }
                public Packaging packaging { get; set; }
                // public int marketplaceId { get; set; }
                // public string slug { get; set; }
                // public List<string> buyableProblems { get; set; }
                //public Frozen frozen { get; set; }
                //public DateTime created { get; set; }
                //public DateTime updated { get; set; }
                public string obligatoryIdentifier { get; set; }
                public string voluntaryIdentifier { get; set; }
                public string returnIdentifier { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public bool? overrideFrozen { get; set; }
            }



            public class Pagination
            {
                public string sortField { get; set; }
                public string order { get; set; }
                public int limit { get; set; }
                public string after { get; set; }
            }

            public class RootProducts
            {
                public Pagination pagination { get; set; }
                public List<string> fields { get; set; }
            }
            public class Product
            {
                public string externalId { get; set; }
                public string name { get; set; }
                public string ean { get; set; }
                public string sku { get; set; }
                public string marketplaceId { get; set; }
            }

            public class RootProductsResult
            {
                public List<Product> MyArray { get; set; }
            }



            #endregion
            public static void GetProduct(Dal.Helper.Shop shop)
            {
                try
                {

                    HttpWebRequest request = ErliRESTHelper.GetHttpWebRequest(shop, "/products/1006", "GET");

                    string text = ShopRestHelper.GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    RootProduct product = json_serializer.Deserialize<RootProduct>(text);





                }

                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, shop);

                }
            }
            public static void GetProducts(Dal.Helper.Shop shop)
            {
                GetProducts(shop, 0);
            }

            private static void GetProducts(Dal.Helper.Shop shop, int after)
            {

                RootProducts rp = new RootProducts()
                {
                    pagination = new Pagination()
                    {
                        limit = 200,
                        after = after.ToString(),
                        order = "ASC",
                        sortField = "externalId"
                    },
                    fields = new List<string>() { "name", "externalId", "ean", "sku", "marketplaceId", "description" }
                };
                try
                {
                    HttpWebRequest request = ErliRESTHelper.GetHttpWebRequest(shop, "/products/_search", "POST");

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(rp);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();




                    string text = ShopRestHelper.GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    Product[] products = Newtonsoft.Json.JsonConvert.DeserializeObject<Product[]>(text);

                    //ProcessProducts(shop, products);

                    if (products.Count() == rp.pagination.limit)
                        GetProducts(shop, after + 200);


                }

                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, shop);

                }
            }

            private static string GetProduct(Dal.Helper.Shop shop, int externalProductId)
            {



                

                try
                {
                    HttpWebRequest request = ErliRESTHelper.GetHttpWebRequest(shop, String.Format("/products/{0}", externalProductId), "GET");

 
                    string text = ShopRestHelper.GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();


                    Product product = json_serializer.Deserialize<Product>(text);

                    return product.marketplaceId;
                }

                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, shop);

                    return "";
                }
            }
            private static void UpdateProduct(Dal.Helper.Shop shop, Dal.ProductCatalogFnResult pc)
            {



                RootProduct product = GetProduct(shop, pc);
               // product.overrideFrozen = true;


                try
                {
                    HttpWebRequest request = ErliRESTHelper.GetHttpWebRequest(shop, String.Format("/products/{0}", pc.ProductCatalogId), "PATCH");

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(product);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    string text = ShopRestHelper.GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    string shopProductId = pc.ShopProductId;

                    // tworzymy produkt wzgl ProductCatalogId ale chcemy pobrać MarketplaceId do użytku wewnętrznego.
                    if (shopProductId == pc.ProductCatalogId.ToString())
                        shopProductId = GetProduct(shop, pc.ProductCatalogId);

                    Dal.DbHelper.ProductCatalog.SetShopProductToProductCatalogById(shop, pc.ProductCatalogId, shopProductId, false, true);

                }

                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, shop);

                }
            }
            private static void SetProduct(Dal.Helper.Shop shop, Dal.ProductCatalogFnResult pc)
            {
                RootProduct product = GetProduct(shop, pc);
                try
                {
                    HttpWebRequest request = ErliRESTHelper.GetHttpWebRequest(shop, String.Format("/products/{0}", pc.ProductCatalogId), "POST");

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(product);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                     
                    string text = ShopRestHelper.GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();


                    string shopProductId = GetProduct(shop, pc.ProductCatalogId);
                    Dal.DbHelper.ProductCatalog.SetShopProductToProductCatalogById(shop, pc.ProductCatalogId, shopProductId, false, true);

                }

                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, shop, String.Format("PId: {0}", pc.ProductCatalogId));

                }
            }
            private static Description GetDescription(Dal.Helper.Shop shop, Dal.ProductCatalogFnResult pc)
            {
                Dal.OrderHelper oh = new Dal.OrderHelper();

                Item itemImg = null;
                List<ProductCatalogImage> images = oh.GetProductCatalogImages(pc.ProductCatalogId).Where(x => x.IsActive).ToList();

       



                Description desc = new Description();
                desc.sections = new List<Section>();
 


    

                string content = OrderHelper.GetPreviewErli(shop, pc.ProductCatalogId, false,false).ToString();
                content = Bll.Helper.ReplaceInvalidAllegroCharactersFromDescription(content);
                Item itemText = new Item()
                {
                    type = "TEXT",
                    content = String.Format("{0}", content.Replace("\r","").Replace("\n",""))
                };

                List<Item> mainSection = new List<Item>();

                if (images.Count > 0)
                {
                    itemImg = new Item()
                    {
                        type = "IMAGE",
                        url = String.Format("http://static.lajtit.pl/ProductCatalog/{0}", images[0].FileName)
                    };
                }


                mainSection.Add(itemText);
                if (itemImg != null)
                    mainSection.Add(itemImg);

                Section section = new Section()
                {
                    items = mainSection
                };

                desc.sections.Add(section);


                if (images != null && images.Count > 1)
                {
                    for (int i = 1; i < images.Count; i++)
                    {
                        Item iImg = new Item()
                        {
                            type = "IMAGE",
                            url = String.Format("http://static.lajtit.pl/ProductCatalog/{0}", images[i].FileName)
                        };
                        List<Item> iItems = new List<Item>();
                        iItems.Add(iImg);
                        Section imgSec = new Section()
                        {
                            items = iItems
                        };
                        desc.sections.Add(imgSec);
                    }
                }

                return desc;
            }
            private static RootProduct GetProduct(Dal.Helper.Shop shop, ProductCatalogFnResult pc)
            {
                return new RootProduct()
                {
                    description = GetDescription(shop, pc),
                    ean = pc.Ean,
                    externalAttributes = GetExternalAttributes(pc),
                    externalCategories = GetCategories(shop, pc),
                    externalId = pc.ProductCatalogId.ToString(),
                    name = ShopRestHelper.Products.GetName(shop, pc),
                    sku = pc.Code,
                    price = (int)(pc.PriceBruttoMinimum.Value * 100),
                    stock = ShopRestHelper.Products.GetStock(pc),
                    status = GetStatus(pc),
                    dispatchTime = GetDispatchTime(pc),
                    images = GetImages(pc),
                    packaging = GetPacking(pc),
                    obligatoryIdentifier = null,
                    voluntaryIdentifier = null,
                    returnIdentifier = null
                };
            }

            private static Packaging GetPacking(ProductCatalogFnResult pc)
            {
                Packaging packing = new Packaging();
                packing.tags = new List<object>();
                packing.tags.Add("Podstawowy");

                return packing;
            }

            private static List<ExternalAttribute> GetExternalAttributes(ProductCatalogFnResult pc)
            {
                 

                List<ExternalAttribute> attributes = new List<ExternalAttribute>();

                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                List<Dal.ProductCatalogToAllegroParametersGetResult> productCatalogParameters = pch.GetProductCatalogToAllegroParameters(pc.ProductCatalogId, null);

                string[] categoryFieldIds = productCatalogParameters.Select(x => x.CategoryFieldId).Distinct().ToArray();

                foreach (string categoryFieldId in categoryFieldIds)
                {
                    ExternalAttribute ea = new ExternalAttribute()
                    {
                        id = categoryFieldId,
                        source="allegro"
                    };

                    var pcp = productCatalogParameters.Where(x => x.CategoryFieldId == categoryFieldId).FirstOrDefault();

                    switch (pcp.FieldType)
                    {
                        case 1:
                            List<object> valueIds = productCatalogParameters.Where(x => x.CategoryFieldId == categoryFieldId)
                                .Select(x => new { id = x.AllegroParameterId, name = x.Name })
                                .Select(x=>(object)x)
                                .Distinct()
                                .ToList();
                            ea.type = "dictionary";
                            ea.values = valueIds;
                            attributes.Add(ea);
                            break;
                        case 2:
                            if (!String.IsNullOrEmpty(pcp.StringValue))
                            {

                                ea.type = "string";
                                if (pcp.UseDefaultValue)
                                    ea.values = new List<object>() { pcp.StringValue };
                                else
                                {
                                    if (pcp.SystemFieldId.HasValue && pcp.SystemFieldId.Value == 3) // Nazwa kolekcji
                                    {
                                        string kolekcja = Bll.Helper.ReplaceInvalidAllegroCharacters(pcp.StringValue);
                                        if (!String.IsNullOrEmpty(kolekcja) && kolekcja.Length > 20)
                                            kolekcja = kolekcja.Substring(0, 20);
                                        ea.values = new List<object>() { kolekcja };
                                    }
                                    else
                                        ea.values = new List<object>() { pcp.StringValue };

                                }
                                attributes.Add(ea);
                            }
                            break;
                        case 3:
                            ea.type = "number";
                            if (pcp.DecimalValue.HasValue)
                                ea.values = new List<object>() {pcp.DecimalValue.Value};// .ToString().Replace(",", ".") };
                            else
                                ea.values = new List<object>() { null };
                            attributes.Add(ea);
                            break;
                    }


                }
                return attributes;
            }

            private static List<ExternalCategory> GetCategories(Dal.Helper.Shop shop, ProductCatalogFnResult pc)
            {
                List<ExternalCategory> categories = new List<ExternalCategory>();
                List<Breadcrumb> b = new List<Breadcrumb>();

                var cat = Bll.AllegroRESTHelper.GetAllegroCategory(pc.ProductCatalogId);


                if (cat == null)
                    return null;

                b.Add(new Breadcrumb()
                {
                    id = cat.AllegroCategoryId,
                    name=cat.AllegroCategoryId
                });
                categories.Add(new ExternalCategory()
                {
                    source = "allegro",
                    breadcrumb = b
                });

                return categories;
            }

            private static Frozen GetFrozen()
            {
                return new Frozen()
                {
                    name = false,
                    description = false,
                    ean = false,
                    sku = false,
                    externalAttributes = false,
                    externalCategories = false,
                    externalVariantGroup = false,
                    images = false,
                    files = false,
                    price = false,
                    stock = false,
                    status = false,
                    dispatchTime = false,
                    packaging = false,
                    obligatoryIdentifier = false,
                    voluntaryIdentifier = false,
                    returnIdentifier = false,
                };
            }

            private static DispatchTime GetDispatchTime(ProductCatalogFnResult pc)
            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources(Dal.Helper.ShopType.Erli);
                int deliveryTime = Bll.Helper.GetSetting("SHOPMINDEL").IntValue.Value;

                int shopDeliveryTimeId = ShopRestHelper.Products.
                    GetDeliveryTime(pc.LeftQuantity, ShopRestHelper.Products.GetDeliveryIdForShop(pc.DeliveryId, sources),
                    deliveryTime);

                DispatchTime dt = new DispatchTime()
                {
                    unit = "day",
                    period = shopDeliveryTimeId
                };

                return dt;
            }

            private static List<Image> GetImages(ProductCatalogFnResult pc)
            {
                List<Image> images = new List<Image>();

                Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();

                List<Dal.ProductCatalogImage> imgs = oh.GetProductCatalogImages(pc.ProductCatalogId)
                       .Where(x => x.IsActive)
                       .ToList();

                images.AddRange(
                    imgs.Select(x =>
                    new Image() { url = String.Format("http://static.lajtit.pl/ProductCatalog/{0}", x.FileName) })
                    .ToList()
                    );

                return images;
            }

 

            private static string GetStatus(ProductCatalogFnResult pc)
            {

                if (pc.IsPSActive)// || (pc.IsAvailableOnline && pc.LeftQuantity > 0) || (pc.IsDiscontinued && pc.LeftQuantity > 0))
                    return "active"; // dostępny

                return "inactive";
            }
            //private static void ProcessProducts(Dal.Helper.Shop shop, Product[] products)
            //{
            //    List<int> notExists = new List<int>();
            //    foreach (Product product in products)
            //    {

            //        try
            //        {
            //            Dal.ProductCatalogShopProduct psp =
            //                Dal.DbHelper.ProductCatalog
            //                .GetProductCatalogShopProductByShopId(Dal.Helper.Shop.Lajtitpl, product.externalId);

            //            Dal.DbHelper.ProductCatalog.SetShopProductToProductCatalogById(shop,
            //                psp.ProductCatalogId,
            //                product.marketplaceId);

            //        }
            //        catch (Exception ex)
            //        {
            //            notExists.Add(Int32.Parse(product.externalId));
            //        }
            //    }
            //    if (notExists.Count > 0)
            //        Bll.ErrorHandler.SendEmail(String.Join(",", notExists.ToArray()));
            //}
        
        public static ShopRestHelper.Bulk.BulkResult Process(List<ProductCatalogShopUpdateSchedule> schedules, Guid processId)
        {

            int[] shopIds = schedules.Select(x => x.ShopId).Distinct().ToArray();

                ShopRestHelper.Bulk.BulkResult results = new ShopRestHelper.Bulk.BulkResult();

            foreach (int shopId in shopIds)
            {
                int[] updateTypeIds = schedules.Select(x => x.UpdateTypeId.Value).Distinct().ToArray();

                Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopId);


                Dal.ShopUpdateHelper suh = new Dal.ShopUpdateHelper();
                Dal.ShopHelper pch = new Dal.ShopHelper();

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

                            //SetProductsUpdateBatch(shop, sch, productCatalogIds);
                            break;

                        case Dal.Helper.UpdateScheduleType.OnlineShopSingle:
                            //results.AddRange(ProcessSchedulesSingle(shopId, sch, pcViews));
                            SetProductsUpdateSingle(shop, sch, productCatalogIds);
                            break;
                    };
                }
            }
                List<ShopRestHelper.Bulk.Item> items = new List<ShopRestHelper.Bulk.Item>();
                items.Add(new ShopRestHelper.Bulk.Item()
                {
                    code = 200,
                    id = "ok"
                });
                results.Append(new ShopRestHelper.Bulk.BulkResult()
                {
                    errors = false,
                    items = items
                });
            return results;
        }
        public static void SetProductsUpdateSingle(Dal.Helper.Shop shop, 
            List<Dal.ProductCatalogShopUpdateSchedule> schedules,
            int[] productCatalogIds)
        {

            //int[] productCatalogIds = schedules.Select(x => x.ProductCatalogId).Distinct().ToArray();


            foreach (int pId in productCatalogIds)
            {
                var sch = schedules.Where(x => x.ProductCatalogId == pId).ToList();

                ProcessInternal(shop, pId, sch);
            }
        }

        private static void ProcessInternal(Dal.Helper.Shop shop, int productCatalogId, List<ProductCatalogShopUpdateSchedule> sch)
        {
                //Dal.ProductCatalogShopProductFnResult psp =
                //    Dal.DbHelper.ProductCatalog.GetProductCatalogShopProduct(shop, pId);
                Dal.ProductCatalogFnResult pc = Dal.DbHelper.ProductCatalog.GetProductCatalogShopProduct((int)shop, new int[] { productCatalogId }).FirstOrDefault();


                if (pc.ShopProductId == null)
                    SetProduct(shop, pc);
                else
                    UpdateProduct(shop, pc);

        }



     
        }
    }
}