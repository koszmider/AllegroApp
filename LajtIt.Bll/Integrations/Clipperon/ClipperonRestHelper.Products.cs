using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using LajtIt.Dal;
using System.Linq;
using Newtonsoft.Json;

namespace LajtIt.Bll
{
    public partial class ClipperonRestHelper
    {
        public class Products
        {
            public class TaxRateByCountry
            {
                public string Country_Code { get; set; }
                public double? Tax_Rate { get; set; }
            }

            public class Product
            {
                public string Ean { get; set; }
                public string Name { get; set; }
                public string Type { get; set; }
                public string Category { get; set; }
                public int Quantity { get; set; }
                public decimal Net_Purchase_Price { get; set; }
                public int Leadtime_To_Ship { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public decimal? Tax_Rate { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string Product_Exts_Id { get; set; }
                public List<TaxRateByCountry> Tax_Rate_By_Country { get; set; }
                public int Product_Ext_Id { get; set; }
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string Sku { get; set; }
            }

            public class RootProduct
            {
                public List<Product> Products { get; set; }
            }

            public class Search
            {
                public class Asins
                {
                    public string Market_Name { get; set; }
                    public string Asin { get; set; }
                }

                public class Datum
                {
                    public List<Asins> Asins { get; set; }
                    public DateTime Date_Modified { get; set; }
                    public string Ean { get; set; }
                    public string Name { get; set; }
                    public string Type { get; set; }
                    public string Category { get; set; }
                    public double Quantity { get; set; }
                    public double Net_Purchase_Price { get; set; }
                    public int Leadtime_To_Ship { get; set; }
                    public double Tax_Rate { get; set; }
                    public List<object> Tax_Rate_By_Country { get; set; }
                    public int Product_Ext_Id { get; set; }
                    public string Sku { get; set; }
                    public string Product_Exts_Id { get; set; }
                }

                public class Paging
                {
                    public int AllRecordsCount { get; set; }
                    public int PageNo { get; set; }
                    public int PageSize { get; set; }
                }

                public class Root
                {
                    public List<Datum> Data { get; set; }
                    public Paging Paging { get; set; }
                }

            }

            public static ShopRestHelper.Bulk.BulkResult Process(List<ProductCatalogShopUpdateSchedule> schedules, 
                Guid processId)
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
                                SetProducts(shop, sch, productCatalogIds);
                                break;

                            case Dal.Helper.UpdateScheduleType.OnlineShopSingle:
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


            public static void SetProducts(Dal.Helper.Shop shop, List<ProductCatalogShopUpdateSchedule> schedules, 
                int[] productCatalogIds)
            {
                string pids = "";
                try
                {
                    List<Dal.ProductCatalogFnResult> products = Dal.DbHelper.ProductCatalog
                        .GetProductCatalogShopProduct((int)shop, productCatalogIds)
                        .Where(x=>x.Ean!=null && x.PurchasePrice.HasValue && x.PurchasePrice > 0).ToList();


                    if (products.Count == 0)
                        return;


                    RootProduct clipperonProducts = new RootProduct();
                    clipperonProducts.Products = new List<Product>();


                    foreach (Dal.ProductCatalogFnResult product in products)
                    {
                        List<TaxRateByCountry> tax = null;// new List<TaxRateByCountry>();
                        //tax.Add(new TaxRateByCountry()
                        //{
                        //    Country_Code = "DE",
                        //    Tax_Rate = null
                        //});
                        Random random = new Random();
                        
                        Product clipperonProduct = new Product()
                        {
                            Type = (product.SupplierId==5 || product.SupplierId ==6)? "grupa 4": null,
                            Category = "TBD",
                            Ean = product.Ean,
                            Leadtime_To_Ship = product.IsOnStock.Value? 1: product.DeliveryId ,
                            Name = product.Name,
                            Net_Purchase_Price = product.PurchasePrice.Value,
                            Product_Ext_Id = product.ProductCatalogId,
                            Quantity =   GetQuantity( product),
                            Product_Exts_Id = String.Format("{0}/{1}", product.ProductCatalogId, product.Code),
                            Tax_Rate = 23,
                            Tax_Rate_By_Country = tax
                        };
                        clipperonProducts.Products.Add(clipperonProduct);
                    }
                      pids = String.Join(",", products.Select(x=>x.ProductCatalogId).ToArray());
                    HttpWebRequest request = ClipperonRestHelper.GetHttpWebRequest(shop, "/api/Products", "POST");

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(clipperonProducts.Products.ToArray());
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    WebResponse webResponse = request.GetResponse();

                    Stream responseStream = webResponse.GetResponseStream();

                    StreamReader reader = new StreamReader(responseStream);

                    string text = reader.ReadToEnd();

                }

                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, shop , String.Format("ProductCatalogIds: {0}", pids));

                }
            }

            private static int GetQuantity(ProductCatalogFnResult product)
            {

                int quantity = 50;

                if (product.SupplierQuantity.HasValue && product.SupplierQuantity.Value > 0)
                    quantity = product.SupplierQuantity.Value + product.LeftQuantity;
                else
                {
                    if (product.IsOnStock.Value && !product.IsAvailable)
                    {
                        quantity = product.LeftQuantity;// + 10;
                    }
                    else
                    {
                        //Dal.ShopHelper shh = new Dal.ShopHelper();
                        //Dal.SupplierShop supplierShops = shh.GetSuppliersShopByAllegroUserId(item.SupplierId, allegroUserId);
                        //if (supplierShops != null && supplierShops.MaxNumberOfProductsInOffer.HasValue)
                        //    quantity = supplierShops.MaxNumberOfProductsInOffer.Value;
                    }
                }

                // na końcu
                // clipperon deaktywuje produkt gdy ilość == 0

                // deaktyuj produkt gdy jest intencjonalnie wyłączony: IsPsAvailable = 0
                // nie ma na stanie


                if (
                    (!product.IsOnStock.Value && !product.IsAvailable) 
                    || quantity < 0 
                    || !product.IsPsAvailable 
                    || product.IsDiscontinued 
                    //|| !product.IsPSActive
                    )
                    quantity = 0;
               

                return quantity;
            }

            public static void GetProducts(Dal.Helper.Shop shop)
            {
                GetProducts(shop, 1);
            }
            private static void GetProducts(Dal.Helper.Shop shop, int pageId)
            {
                try
                {

                    HttpWebRequest request = ClipperonRestHelper.GetHttpWebRequest(shop, String.Format("/api/Products?pageNo={0}", pageId), "GET");

                    string text = ShopRestHelper.GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    Search.Root rootProducts = json_serializer.Deserialize<Search.Root>(text);

                    ProcessProducts(shop, rootProducts);

                    if(rootProducts.Paging.PageNo * 50 < rootProducts.Paging.AllRecordsCount)
                        GetProducts(shop, pageId + 1);
                }

                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, shop);

                }
            }

            private static void ProcessProducts(Dal.Helper.Shop shop, Search.Root rootProducts)
            {
                List<Dal.ProductCatalogShopProduct> products = new List<ProductCatalogShopProduct>();

                foreach(Search.Datum product in rootProducts.Data)
                {
                    products.Add(new ProductCatalogShopProduct()
                    {
                        ShopId = (int)shop,
                        ProductCatalogId = product.Product_Ext_Id,
                        ShopProductId = product.Sku// GetAsin(product)
                    });
                }

                Dal.DbHelper.Shop.SetProductCatalogShopProductId(shop, products);

            }

            private static string GetAsin(Search.Datum product)
            {
                var de = product.Asins.Where(x => x.Market_Name == "amazon-de" && x.Asin != "").FirstOrDefault();

                if (de != null)
                    return de.Asin;

                var asin = product.Asins.Where(x =>  x.Asin != "").FirstOrDefault();

                if (asin != null)
                    return asin.Asin;

                return null;
            }
        }


    }
}
