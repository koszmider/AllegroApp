using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public partial class ShopUpdateHelper
    {
        public  class ClickShop
        {
            Bll.ShopHelper sh=null;

            public string SessionId
            {
                get
                {
                    if (sh == null)
                        sh = new ShopHelper();

                    return sh.SessionId;
                }
            }

            public class UpdateResult
            {
                public int ShopProductId { get; set; }
                public int ProductCatalogId { get; set; }
                public int Result { get; set; }
                public int ShopId { get; set; }
            }

            //public List<UpdateResult> Process(List<Dal.ProductCatalogShopUpdateSchedule> schedules, Guid processId)
            //{
            //    int[] shopIds = schedules.Select(x => x.ShopId).Distinct().ToArray();

            //    List<UpdateResult> results = new List<UpdateResult>();

            //    foreach (int shopId in shopIds)
            //    {
            //        int[] updateTypeIds = schedules.Select(x => x.UpdateTypeId.Value).Distinct().ToArray();

            //        Dal.ShopUpdateHelper suh = new Dal.ShopUpdateHelper();
            //        Dal.ShopHelper pch = new Dal.ShopHelper();

            //        foreach (int updateTypeId in updateTypeIds)
            //        {
            //            List<Dal.ProductCatalogShopUpdateSchedule> sch = schedules
            //                .Where(x => x.ShopId == shopId && x.UpdateTypeId.Value == updateTypeId)
            //                .ToList();
            //            int[] productCatalogIds = sch.Select(x => x.ProductCatalogId).Distinct().ToArray();
            //            List<Dal.ProductCatalogFnResult> pcViews = pch.GetProductCatalogShopProduct(shopId, productCatalogIds);

            //            switch ((Dal.Helper.UpdateScheduleType)Enum.Parse(typeof(Dal.Helper.UpdateScheduleType), updateTypeId.ToString()))
            //            {
            //                case Dal.Helper.UpdateScheduleType.OnlineShopBatch:

            //                    if (pcViews.Where(x => x.ShopProductId == null).Count() > 0)
            //                        results.AddRange(ProcessSchedulesSingle(shopId, sch, pcViews.Where(x => x.ShopProductId == null).ToList()));
            //                    else
            //                        results.AddRange(ProcessSchedulesBatch(shopId, sch, pcViews.Where(x => x.ShopProductId != null).ToList()));
            //                    break;
            //                case Dal.Helper.UpdateScheduleType.OnlineShopSingle:
            //                    results.AddRange(ProcessSchedulesSingle(shopId, sch, pcViews));
            //                    break;
            //            };
            //        }
            //    }
            //    return results;
            //}


            //private List<UpdateResult> ProcessSchedulesSingle(int shopId, List<ProductCatalogShopUpdateSchedule> sch,
            //    List<Dal.ProductCatalogFnResult> pcViews)
            //{
            //    #region deklaracje
            //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            //    int[] productCatalogIds = pcViews.Select(x => x.ProductCatalogId).ToArray();

            //    Dictionary<string, object> products = new Dictionary<string, object>();
            //    List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources(Dal.Helper.ShopType.ClickShop);
            //    List<Dal.ProductCatalogAttributeCategoryFnResult> categories = pch.GetShopProductAndCategoriesFromAttributes(shopId)
            //     .Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();
            //    #endregion

            //    Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopId);

            //    List<UpdateResult> results = new List<UpdateResult>();
            //    if (categories.Count == 0)
            //    {
            //        results.Add(
            //            new UpdateResult()
            //            {
            //                ProductCatalogId = 0,
            //                Result = -1,
            //                ShopId=shopId,
            //                ShopProductId = 0 
            //            }
            //            );
            //        return results;
            //    }
            //    //if (createProduct && pc.IsActiveOnline == false)
            //    //    return;


            //    foreach (int productCatalogId in productCatalogIds)
            //    {
            //        Dal.ProductCatalogFnResult pc = pcViews.Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();

            //        List<ProductCatalogShopUpdateSchedule> s = sch.Where(x => x.ProductCatalogId == productCatalogId).ToList();
            //        // bool createProduct = pc.ShopProductId == null;

            //        // Dictionary<string, object> product = GetProductForShop(pc, categories, sources, sch, createProduct);
            //        // if (product != null && product.Count > 0)
            //        //     products.Add(pc.ShopProductId, product);

            //        try
            //        {

            //            results.Add(UpdateProduct(shop, pc, categories, sources, s));
            //        }
            //        catch (Exception ex)
            //        {
            //            Bll.ErrorHandler.SendError(ex,
            //                String.Format("ProcessSchedulesSingle. ProductCatalogId: <a href='http://192.168.0.107/ProductCatalog.Specification.aspx?id={0}'>{0}</a>", productCatalogId));
            //        }
            //    }


            //    return results;


            //}

            public class Producer
            {
                public string name { get; set; }
                public int producer_id { get; set; }
            }
            public void GetProducers(Dal.Helper.Shop shop) {

                Object[] methodParams = { SessionId, "producer.list", new Object[] {  true } };
                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);



                var json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = Int32.MaxValue;
                Producer[] producers = json_serializer.Deserialize<Producer[]>(Bll.RESTHelper.ToJson(r));

                List<Dal.ShopProducer> shopProducers = producers.Select(x => new ShopProducer()
                {
                    Name = x.name,
                    ShopProducerId = x.producer_id,
                    InsertDate = DateTime.Now,
                    IsActive = true,
                    ShopId = (int)shop
                }).ToList();

                Dal.ShopHelper sh = new Dal.ShopHelper();
                sh.SetShopProducers(shop, shopProducers);
            }
            public int SetProducer(Dal.Helper.Shop shop, string name)
            {
                Dictionary<string, object> d = new Dictionary<string, object>();

                d.Add("name", name);

                Object[] methodParams = { SessionId, "producer.create", new Object[] { d } };
                int producer_id = (int)Bll.ShopHelper.SendApiRequest("call", methodParams);


                Dal.ShopProducer producer = new ShopProducer()
                {
                    Name = name,
                    ShopProducerId = producer_id,
                    InsertDate = DateTime.Now,
                    IsActive = true,
                    ShopId = (int)shop
                };

                Dal.ShopHelper sh = new Dal.ShopHelper();
                return sh.SetShopProducer(shop, producer);
            }

            //private  UpdateResult UpdateProduct(
            //    Dal.Helper.Shop shop,
            //    Dal.ProductCatalogFnResult pc,
            //    List<Dal.ProductCatalogAttributeCategoryFnResult> categories,
            //    List<Dal.SupplierDeliveryTypeSource> sources,
            //    List<ProductCatalogShopUpdateSchedule> schedules)
            //{

            //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            //    bool createProduct = pc.ShopProductId == null;

            //    Dictionary<string, object> d = new Dictionary<string, object>();
            //    Dal.ShopHelper sh = new Dal.ShopHelper();
            //    List<Dal.SupplierShop> producers = sh.GetSupplierShop((int)shop);

            //    int result = 0;
            //    int product_id = 0;
            //    try
            //    {

            //        d = GetProductForShop(shop, pc, categories, sources, schedules, producers, createProduct);

            //        if(d==null)
            //        {
            //            return new UpdateResult()
            //            {
            //                ProductCatalogId = pc.ProductCatalogId,
            //                Result = 0,
            //                ShopProductId = 0,
            //                ShopId = 0
            //            };
            //        }
                    

            //        if (createProduct)
            //        {
            //            if (pc.PriceBruttoFixed <= 0 || pc.ImageFullName == null || pc.HasProductType == false)
            //            {
            //                result = -1;
            //            }
            //            else
            //            {
            //                Object[] methodParams = { SessionId, "product.create", new Object[] { d } };

            //                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);

            //                Dictionary<string, object> dic = r as Dictionary<string, object>;

            //                product_id = (int)r;

            //                if (product_id > 0)
            //                {
            //                    result = 1;
            //                    pch.SetShopProductToProductCatalogById(Dal.Helper.Shop.Lajtitpl, pc.ProductCatalogId, product_id.ToString());
            //                    pc.ShopProductId = product_id.ToString();
            //                }
            //                else
            //                {
            //                    result = product_id;
            //                    Bll.ErrorHandler.SendEmail(String.Format("Bład tworzenia produktu w sklepie. ProductCatalogId: {0}, return code: {1}", pc.ProductCatalogId, result));
            //                }
            //            }
            //        }
            //        else
            //        {
            //            product_id = Int32.Parse(pc.ShopProductId);
            //            if (d.Count > 0)
            //            {
            //                Object[] methodParams = { SessionId, "product.save", new Object[] { product_id, d, true } };
            //                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);
            //                result = (int)r;
            //            }
            //            else
            //                result = 1;

            //        }
            //        if (result == 1)
            //        {

            //            if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Images) || CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.All))
            //                SetImagesUpdate(shop, product_id);
            //            if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.PricePromo) || CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.All))
            //                SetPricePromo(pc);
            //            if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Category) || CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.All))
            //            {
            //                if (!pc.IsActiveAllegro)
            //                    DettachCategories(true, product_id, "563");
            //                else
            //                    SetProductCategoriesFromAttributes((int)shop ,pc.ProductCatalogId);
            //            }
            //            if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Related) || CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.All))
            //                SetRecommendedProducts( pc);


            //            if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Attributes) || CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.All))
            //            {


            //                SetShopProductAttributes(SessionId, shop, pc.ProductCatalogId, Int32.Parse(pc.ShopProductId));

            //                if (!pc.IsDiscontinued)
            //                    SetProductCategoriesFromAttributes((int)shop, pc.ProductCatalogId);
            //            }

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Bll.ErrorHandler.SendError(ex, String.Format("SetProduct. ProductCatalogId: {0}<br><Br>{1}", pc.ProductCatalogId, ex.Message));

            //        throw ex;

            //    }
            //    return new UpdateResult() { Result = result, ShopProductId = product_id }; ;
            //}
            //private  void SetShopProductAttributes(
            //    string sessionId,
            //    Dal.Helper.Shop shop,
            //    int productCatalogId,
            //    int? shopProductId)
            //{

            //    try
            //    {
            //        Dictionary<string, object> d = GetShopAttributes(shop, productCatalogId);

            //        if (d.Count > 0)
            //        {
            //            if (shopProductId.HasValue)
            //                SetShopProductAttributesDelete(shopProductId.Value);

            //            Object[] att = { shopProductId, d, true };
            //            Object[] methodParams = { sessionId, "product.attributes.save", att };
            //            var r = Bll.ShopHelper.SendApiRequest("call", methodParams);

            //            if (r.ToString() != "1")
            //                throw new Exception(String.Format("Błąd aktualizacji parametrów SetShopProductAttributes. Zwrócony kod: {0}, ProductCatalogId: {1}",
            //                    r, productCatalogId));
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        LajtIt.Bll.ErrorHandler.SendError(ex, String.Format("SetAttributesToShopProduct, id produkt: {0}"
            //            , productCatalogId));

            //    }
            //}
            //private  void SetShopProductAttributesDelete(int shopProductId)
            //{
            //    try
            //    {
            //        Object[] att = { shopProductId };
            //        Object[] methodParams = { SessionId, "product.attributes", att };
            //        var r = Bll.ShopHelper.SendApiRequest("call", methodParams);

            //        Dictionary<string, object> atts = r as Dictionary<string, object>;


            //        Dictionary<string, object> d = new Dictionary<string, object>();
            //        if (atts != null)
            //        {
            //            foreach (KeyValuePair<string, object> a in atts)
            //            {
            //                string k = a.Key;
            //                Dictionary<string, object> o = a.Value as Dictionary<string, object>;
            //                if (o != null)
            //                    foreach (KeyValuePair<string, object> b in o)
            //                    {
            //                        d.Add(b.Key, null);
            //                    }

            //            }

            //            Object[] att2 = { shopProductId, d, true };
            //            Object[] methodParams2 = { SessionId, "product.attributes.save", att2 };
            //            var r2 = Bll.ShopHelper.SendApiRequest("call", methodParams2);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        LajtIt.Bll.ErrorHandler.SendError(ex, String.Format("SetShopProductAttributesDelete, id produkt: {0}"
            //            , shopProductId));

            //    }

            //}
            //public  void SetRecommendedProducts(ProductCatalogFnResult pc)
            //{

            //    Dal.ProductCatalogGroupHelper pch = new ProductCatalogGroupHelper();
            //    //List<Dal.ProductCatalog> products = pch.GetProductCatalogsByProductFamily(pc.ProductCatalogId, Dal.Helper.ProductCatalogGroupFamilyType.Family)
            //    //    .Where(x => x.ShopProductId.HasValue)
            //    //    .ToList();
            //    //products.AddRange(pch.GetProductCatalogsByProductFamily(pc.ProductCatalogId, Dal.Helper.ProductCatalogGroupFamilyType.Aletnative)
            //    //    .Where(x => x.ShopProductId.HasValue)
            //    //    .ToList());
            //    List<Dal.ProductCatalogRelatedByFamilyFnResult> products = pch.GetProductCatalogsByProductFamily(Dal.Helper.Shop.Lajtitpl, pc.ProductCatalogId);

            //    int[] recommededProductIds = products
            //            .Where(x => x.ShopProductId!=null)
            //            .Take(20)
            //            .Select(x => Int32.Parse(x.ShopProductId))
            //            .Distinct()
            //            .ToArray();
            //    SetRecommendedProducts(recommededProductIds, Int32.Parse(pc.ShopProductId));

            //}
            //private  void SetRecommendedProducts( int[] recommededProductIds, int productId)
            //{

            //    // get

            //    Object[] att = { productId };
            //    Object[] methodParams = { SessionId, "product.related.list", att };

            //    var r = Bll.ShopHelper.SendApiRequest("call", methodParams);

            //    if (r != null)
            //    {
            //        int[] toDelete = (r as object[]).Cast<int>().ToArray();
            //        //delete

            //        if (toDelete.Length > 0)
            //        {
            //            Object[] att1 = { productId, toDelete };
            //            Object[] methodParams1 = { SessionId, "product.related.list.delete", att1 };

            //            var r1 = Bll.ShopHelper.SendApiRequest("call", methodParams1);

            //        }
            //    }
            //    //set

            //    Object[] att2 = { productId, recommededProductIds };
            //    Object[] methodParams2 = { SessionId, "product.related.list.create", att2 };

            //    var r2 = Bll.ShopHelper.SendApiRequest("call", methodParams2);
            //}

            //public  void SetProductCategoriesFromAttributes(int shopId, int? productCatalogId)
            //{
            //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            //    List<Dal.ProductCatalogAttributeCategoryFnResult> products = null;

            //    if (productCatalogId.HasValue)
            //        products = pch.GetShopProductAndCategoriesFromAttributes(shopId, new int[] { productCatalogId.Value });
            //    else
            //        products = pch.GetShopProductAndCategoriesFromAttributes(shopId).Where(x=>x.ShopProductId != null).ToList();


            //    // produkty dostepne
            //    var shopProductAvailableIds = products
            //        .Where(x=>x.IsActiveOnline.Value==true)
            //        .Select(x => new
            //    {
            //        ProductCatalogId = x.ProductCatalogId, 
            //        IsShopMainCategory = (x.IsMainCategory == 1),
            //            ShopProductId = x.ShopProductId
            //        })
            //    .Where(x => x.IsShopMainCategory )
            //    .Distinct().ToArray();

            //    // produkty niedostepne, dodac do hurotwej zmiany kategorii
            //    int[] shopProductNotAvailableIds = products
            //        .Where(x => x.IsActiveOnline.Value == false)
            //        .Select(x =>   x.ProductCatalogId )
            //    .Distinct().ToArray();


            //    List<int> productCatalogIdsToBatchUpdate = new List<int>();
            //    productCatalogIdsToBatchUpdate.AddRange( shopProductAvailableIds.Select(x => x.ProductCatalogId).ToArray());
            //    productCatalogIdsToBatchUpdate.AddRange(shopProductNotAvailableIds);


            //    List<Dal.ProductCatalogShopUpdateSchedule> schedules = new List<ProductCatalogShopUpdateSchedule>();
            //    schedules.AddRange(
            //        productCatalogIdsToBatchUpdate.Select(x=>
            //        new ProductCatalogShopUpdateSchedule()
            //        {
            //            ProductCatalogId=x,
            //            ShopId= (int)Dal.Helper.Shop.Lajtitpl,
            //            ShopColumnTypeId = (int)Dal.Helper.ShopColumnType.Category,
            //            UpdateTypeId=(int)Dal.Helper.UpdateScheduleType.OnlineShopBatch
                        
            //        })
            //        .Distinct()
            //        .ToList()
            //        );

            //    int[] productCatalogIds = schedules.Select(x => x.ProductCatalogId).ToArray();

            //    Dal.ShopHelper sh = new Dal.ShopHelper();

            //    List <Dal.ProductCatalogFnResult> pcViews = sh.GetProductCatalogShopProduct(shopId, productCatalogIds);


            //    ProcessSchedulesBatch((int)Dal.Helper.Shop.Lajtitpl, schedules, pcViews);

            //    foreach (var p in shopProductAvailableIds)
            //    {
            //        try
            //        {
            //            string shopCategoryId = products.Where(x => x.ShopProductId == p.ShopProductId && x.IsMainCategory == 1)
            //                .Select(x => x.CategoryId).FirstOrDefault();

            //            //SetShopMainCategory(p.IsShopMainCategory, Int32.Parse(p.ShopProductId), shopCategoryId);

            //            DettachCategories(p.IsShopMainCategory, Int32.Parse(p.ShopProductId), shopCategoryId);


            //            string[] categoryIds = products.Where(x => x.ShopProductId == p.ShopProductId && x.IsMainCategory == 0)
            //             .Select(x => x.CategoryId).Distinct().ToArray();
            //            Object[] att = { p.ShopProductId, categoryIds, true };
            //            Object[] methodParams = { SessionId, "product.categories.attach", att };
            //            var r = Bll.ShopHelper.SendApiRequest("call", methodParams);

            //            Console.WriteLine(String.Format("SetProductCategoriesFromAttributes ShopProductId: {0}", p.ShopProductId));
            //        }
            //        catch (Exception ex)
            //        {
            //            LajtIt.Bll.ErrorHandler.SendError(ex, String.Format("SetProductCategoriesFromAttributes, ShopProductId: {0}, sprawdz czy istnieje przypisanie kategorii dla producenta"
            //                , p.ShopProductId));

            //        }
            //    }
            //}
            //private  void SetShopMainCategory( bool isShopMainCategory, int shopProductId, string categoryId)
            //{
            //    if (isShopMainCategory == false)
            //        return;
            //    Dictionary<string, object> product = new Dictionary<string, object>();
            //    product["category_id"] = categoryId;

            //    Object[] att = { shopProductId, product, true };
            //    Object[] methodParams = { SessionId, "product.save", att };
            //    var r = Bll.ShopHelper.SendApiRequest("call", methodParams);
            //    string s = r.ToString();
            //}
            //private  void SetPricePromo(ProductCatalogFnResult pc)
            //{
            //    if (pc.IsActivePricePromo || pc.OnlineShopLockRebates)
            //    {

            //    }
            //    else
            //    {
                    
            //        Object[] methodParams = { SessionId, "product.promo.delete", new Object[] { pc.ShopProductId, true } };
            //        var r = Bll.ShopHelper.SendApiRequest("call", methodParams);
            //    }
            //}
            //private  void DettachCategories( bool isShopMainCategory, int shopProductId, string categoryId)
            //{

            //    Object[] att = { shopProductId };
            //    Object[] methodParams = { SessionId, "product.categories", att };
            //    var i = (Bll.ShopHelper.SendApiRequest("call", methodParams) as object[]).Cast<int>().ToArray();

            //    if (isShopMainCategory)
            //        i = i.Where(x => x.ToString() != categoryId).ToArray();

            //    //int[] categoryIds = new int[];
            //    Object[] att1 = { shopProductId, i, true };
            //    Object[] methodParams1 = { SessionId, "product.categories.detach", att1 };
            //    var r = Bll.ShopHelper.SendApiRequest("call", methodParams1);
            //    string s = r.ToString();
            //}
            //public   void SetImagesUpdate(Dal.Helper.Shop shop, int[] productIds)
            //{
            //    foreach (int i in productIds)
            //        SetImagesUpdate(shop, i);
            //}
            //internal void ShopRefreshImages(Dal.Helper.Shop shop)
            //{
            //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            //    List<Dal.ProductCatalogShopProduct> products = pch.GetProductCatalogInOnlineShop(Dal.Helper.Shop.Lajtitpl);

            //    foreach (Dal.ProductCatalogShopProduct pc in products)
            //        SetImagesUpdate(shop, Int32.Parse(pc.ShopProductId));
            //}
            //private  void DeleteImages(int productId, List<Dal.ProductCatalogImage> images)
            //{
            //    //try
            //    //{
            //    //    int[] imagesIdsDb = images.Where(x => x.ShopImageId.HasValue).Select(x => x.ShopImageId.Value).ToArray();

            //    //    int[] imageIdsShop = GetImages(productId);


            //    //    int[] imagesToDelete = imageIdsShop.Where(x => !imagesIdsDb.Contains(x)).ToArray();



            //    //    if (imagesToDelete.Length > 0)
            //    //    {
                        
            //    //        Object[] att = { productId, imagesToDelete, true };
            //    //        Object[] methodParams = { SessionId, "product.image.list.delete", att };

            //    //        var r = Bll.ShopHelper.SendApiRequest("call", methodParams);
            //    //    }
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    Dal.ErrorHandler.LogError(ex, "DeleteImages");
            //    //}
            //}
            //public  int[] GetImages(int productId)
            //{

                
            //    Object[] att = { productId };
            //    Object[] methodParams = { SessionId, "product.images", att };

            //    var r = Bll.ShopHelper.SendApiRequest("call", methodParams);

            //    if (r == null)
            //        return new int[] { };

            //    var json_serializer = new JavaScriptSerializer();
            //    json_serializer.MaxJsonLength = Int32.MaxValue;
            //    ProductImage[] productImages = json_serializer.Deserialize<ProductImage[]>(Bll.ShopHelper.ToJson(r));

            //    return productImages.Select(x => x.gfx_id).ToArray();
            //}
            //public  bool SetImagesUpdate(Dal.Helper.Shop shop, int shopProductId)
            //{

            //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            //    Dal.ProductCatalog pc = pch.GetProductCatalogOnShopProductId(shop, shopProductId.ToString());
            //    List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImages(pc.ProductCatalogId).Where(x => x.IsActive).ToList() ;

            //    DeleteImages(shopProductId, images);
            //    try
            //    {

            //        foreach (Dal.ProductCatalogImage image in images)
            //        {
            //            int shopImageId = SetImage(shopProductId, pc, image, false);

            //            //if (!image.ShopImageId.HasValue)
            //            pch.SetProductCatalogImageShopImageId(image.ImageId, shopImageId);


            //        }

            //        SetImagesOrder(pc.ProductCatalogId, shopProductId);
            //        return true;
            //    }
            //    catch (Exception ex)
            //    {
            //        Dal.ErrorHandler.LogError(ex, "Aktualizacja zdjęć");
            //        return false;
            //    }
            //}

            //private  void SetImagesOrder(int productCatalogId, int shopProductId)
            //{
            //    //Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            //    //int[] images = pch.GetProductCatalogImages(productCatalogId)
            //    //    .Where(x => x.ShopImageId.HasValue)
            //    //    .Select(x => x.ShopImageId.Value)
            //    //    .ToArray();

                
            //    //Object[] att = { shopProductId, images, true };
            //    //Object[] methodParams = { SessionId, "product.images.order", att };

            //    //var r = Bll.ShopHelper.SendApiRequest("call", methodParams);

            //}

            //private  int SetImage(int shopProductId, Dal.ProductCatalog pc, Dal.ProductCatalogImage image, bool overwriteImageId)
            //{

            //    //// Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            //    // Dictionary<string, object> d = new Dictionary<string, object>();

            //    // if (image.ShopImageId.HasValue && overwriteImageId == false)
            //    //     d.Add("gfx_id", image.ShopImageId);//'] (int) - identyfikator zdjęcia do aktualizacji. Jeśli nie jest podany, utworzone zostanie nowe zdjęcie
            //    // d.Add("file", image.FileName);//'] (string) - nazwa pliku graficznego
            //    // string path = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];

            //    // if (!image.ShopImageId.HasValue || overwriteImageId == true)
            //    //     d.Add("content", Convert.ToBase64String(GetImage(path, image)));//'] (string) - zawartość pliku graficznego zakodowana w base64
            //    //                                                                     //d.Add("url, );//'] (string) - jeśli istnieje, zawartość pliku graficznego pobierana jest z podanego adresu url
            //    // d.Add("name", pc.Name);//'] (string) - opis zdjęcia
            //    // d.Add("hidden", image.IsActive ? "0" : "1");//'] (string) - opis zdjęcia



            //    // Object[] att = { shopProductId, d, true };
            //    // Object[] methodParams = { SessionId, "product.image.save", att };

            //    // var o = Bll.ShopHelper.SendApiRequest("call", methodParams);

            //    // int shopImageId = 0;

            //    // if (Int32.TryParse(o.ToString(), out shopImageId))
            //    // {
            //    //     if (shopImageId == 1)
            //    //         return image.ShopImageId.Value;
            //    //     //else
            //    //     //    return
            //    // }
            //    // else
            //    // {

            //    //     Dictionary<string, object> o1 = (Dictionary<string, object>)o;


            //    //     if (o1["code"].ToString() == "31" && overwriteImageId == false)
            //    //         return SetImage(shopProductId, pc, image, true);
            //    // }
            //    // return (int)o;
            //    return 0;
            //}

            //private  byte[] GetImage(string path, ProductCatalogImage image)
            //{
            //    if (image.Size <= 1000000)
            //    {
            //        return System.IO.File.ReadAllBytes(String.Format(path, image.FileName));
            //    }
            //    else
            //    {
            //        using (Bitmap bmp = new Bitmap(String.Format(path, image.FileName)))
            //        {
            //            int height = bmp.Height;
            //            int width = bmp.Width;
            //            decimal rate = bmp.Width / 1024M;
            //            int newH = (int)(height / rate);
            //            int newW = (int)(width / rate);

            //            return Bll.Helper.ResizeImage(String.Format(path, ""),
            //               image.FileName,
            //               newW,
            //               newH);

            //        }

            //    }

            //}

            //private  List<UpdateResult> ProcessSchedulesBatch(int shopId, 
            //    List<ProductCatalogShopUpdateSchedule> schedules, 
            //    List<Dal.ProductCatalogFnResult> pcViews)
            //{
            //    List<UpdateResult> results = new List<UpdateResult>();

            //    if (schedules.Count == 0)
            //        return results;

            //    #region deklaracje
            //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            //    int[] productCatalogIds = pcViews.Select(x => x.ProductCatalogId).ToArray();

            //    Dictionary<string, object> products = new Dictionary<string, object>();
 
            //    List<Dal.ProductCatalogAttributeCategoryFnResult> categories = pch.GetShopProductAndCategoriesFromAttributes(shopId)
            //     .Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();
            //    List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources(Dal.Helper.ShopType.ClickShop);

            //    Dal.ShopHelper sh = new Dal.ShopHelper();
            //    List<Dal.SupplierShop> producers = sh.GetSupplierShop(shopId);
            //    #endregion

            //    Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopId);
            //    foreach (int productCatalogId in productCatalogIds)
            //    {
            //        Dal.ProductCatalogFnResult pc = pcViews.Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();
            //        var sch = schedules.Where(x => x.ProductCatalogId == productCatalogId).ToList();

            //        Dictionary<string, object> product = GetProductForShop(shop, pc, categories, sources, sch, producers, false);
            //        if (product != null && product.Count > 0)
            //            products.Add(pc.ShopProductId, product);

            //    }

            //    int batchSize = 300;

            //    if (products.Count > 0)
            //    {
            //        int partsCount = (products.Count / batchSize);
            //        for (int i = 0; i < partsCount + 1; i++)
            //        {
            //            Dictionary<string, object> pp = new Dictionary<string, object>();
            //            foreach(KeyValuePair<string, object> k in products.Skip(i * batchSize).Take(batchSize).ToList())
            //            {
            //                pp.Add(k.Key, k.Value);

            //            }

            //            if (pp.Count == 0)
            //                continue;


            //            SendUpdateToShop(shopId, results, pp);

            //        }

            //    }

            //    return results;
            //}

            //private void SendUpdateToShop(int shopId, List<UpdateResult> results, Dictionary<string, object> products)
            //{
            //    Object[] methodParams = { SessionId, "product.list.save", new Object[] { products, true } };
            //    var r = Bll.ShopHelper.SendApiRequest("call", methodParams);
            //    Dictionary<string, object> result = r as Dictionary<string, object>;

            //    results.AddRange(
            //        result.Select(x => new UpdateResult()
            //        {
            //            ShopProductId = Convert.ToInt32(x.Key),
            //            Result = Convert.ToInt32(x.Value),
            //            ShopId = shopId
            //        })
            //        .ToList());
            //}

            #region Funkcje pomocnicze
     

            //private  string GetCategory(ProductCatalogFnResult pc, string categoryId)
            //{
            //    if (pc.IsActiveOnline)
            //        return categoryId;
            //    else
            //        return "563"; // Archiwum
            //}

            private  Dictionary<string, object> GetShopAttributes(Dal.Helper.Shop shop, int productCatalogId)
            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                Dal.ShopHelper sh= new Dal.ShopHelper();

               

                List<Dal.ProductCatalogAttributeGroupsForShopResult> accessibleAttributes = sh.GetProductCatalogAttributeGroupsForShop(shop, productCatalogId)
                    .Where(x=>(x.IsDefault.HasValue && x.IsDefault.Value) || !x.IsDefault.HasValue) // specycika clickshop gdzie podajemy tylko jedną wartość
                    .ToList();

                Dictionary<string, object> d = new Dictionary<string, object>();

                foreach (Dal.ProductCatalogAttributeGroupsForShopResult attribute in accessibleAttributes)
                {
                    if(attribute.IsDefault.HasValue==false)
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

            private  string GetAttributeValue(ProductCatalogAttributeGroupsForShopResult attribute)
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

       
            public static  int GetDeliveryTime(int leftQuantity, int? shopDeliveryDays, int deliveryTime)
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
            public static  int GetStock(int? supplierQuantity, int leftQuantity, bool isAvailable, bool isDiscountinued)
            {
                int defaultQuantity = 50;

                if (isDiscountinued)
                    if (leftQuantity > 0)
                    {
                        return leftQuantity;
                    }
                //else
                //{ return 0; }

                if (supplierQuantity.HasValue && supplierQuantity.Value > 0)
                    return supplierQuantity.Value + leftQuantity;



                // jeśli produkt jest na magazynie ale nie jest dostępny u dostawcy, to wystaw tylko ilość 
                // z naszego magazynu. W przeciwnym razie wystaw więcej produktów.
                if (leftQuantity > 0 && !isAvailable)
                {
                    return leftQuantity;
                }
                else
                {
                    return defaultQuantity;
                }

            }
            public static  int? GetDeliveryIdForShop(int deliveryId, List<Dal.SupplierDeliveryTypeSource> sources)
            {

                return sources.Where(x => x.DeliveryId == deliveryId).Select(x => x.ExternalValue).FirstOrDefault();
            }

            //private  int? GetAvailability(ProductCatalogFnResult pc)
            //{
            //    if (pc.IsActiveOnline || (pc.IsAvailableOnline && pc.LeftQuantity > 0) || (pc.IsDiscontinued && pc.LeftQuantity > 0))
            //        return null; // dostępny

            //    if (pc.IsDiscontinued)
            //        return 7; //wycofany z oferty

            //    else
            //        return 3; // spodziewana dostawa
            //}
    

            //private  Dictionary<string, object> GetProductForShop(
            // Dal.Helper.Shop shop,
            // Dal.ProductCatalogFnResult pc,
            // List<Dal.ProductCatalogAttributeCategoryFnResult> categories,
            // List<Dal.SupplierDeliveryTypeSource> sources,
            // List<Dal.ProductCatalogShopUpdateSchedule> schedules,
            // List<Dal.SupplierShop> producers,
            // bool createProduct)
            //{
            //    Dal.ProductCatalogAttributeCategoryFnResult category = 
            //        categories.Where(x => x.ProductCatalogId == pc.ProductCatalogId
            //    && x.IsMainCategory == 1
            //    && x.CategoryId != null).FirstOrDefault();

            //    if (category == null)
            //        return null;
            //    //if (createProduct && pc.IsActiveOnline == false)
            //    //    return false;

            //    Dictionary<string, object> d = new Dictionary<string, object>();

            //    try
            //    {
            //        Dal.SupplierShop supplier = producers.Where(x => x.SupplierId == pc.SupplierId).FirstOrDefault();

            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.All))
            //            d.Add("producer_id", supplier.ProducerId);//'] (int) - identyfikator producenta
            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.All))
            //            d.Add("tax_id", 1);//'] (int) - identyfikator stawki podatkowej
            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Category)
            //            || CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Status))
            //            d.Add("category_id", GetCategory(pc, category.CategoryId));// category.CategoryId.Value);//'] (int) - identyfikator głównej kategorii
            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Attributes))
            //            d.Add("attributes", GetShopAttributes(shop, pc.ProductCatalogId));//'] (int) - identyfikator głównej kategorii

            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.All))
            //            d.Add("unit_id", 1);//'] (int) - identyfikator jednostki miary
            //                                //d.Add("other_price", );//'] (double) - cena produktu w innych sklepach

            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.All))
            //            d.Add("code", CheckProductCode(pc, createProduct));//'] (string) - kod produktu
            //                                                               //d.Add("pkwiu", );//'] (string) - pkwiu produktu

            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Ean))
            //            d.Add("ean", pc.Ean);//'] (string) - kod produktu


            //        Dictionary<string, object> s = new Dictionary<string, object>();

            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Price))
            //        {
            //            decimal p = 0.01M;

            //            if (!supplier.LockRebates || pc.IsActivePricePromo)
            //                s.Add("price", pc.PriceBruttoShop);//'] (double) - cena wariantu podstawowego
            //            else
            //                s.Add("price", pc.PriceBruttoShop + p);//'] (double) - cena wariantu podstawowego

            //            Dictionary<string, object> spo = new Dictionary<string, object>();

            //            if (pc.IsActivePricePromo || supplier.LockRebates)
            //            {
            //                if (supplier.LockRebates && !pc.IsActivePricePromo)
            //                {
            //                    spo.Add("datefrom", String.Format("{0:yyyy-MM-dd}", DateTime.Now)); //(string) - data(w formacie yyyy - mm - dd) rozpoczęcia promocji
            //                    spo.Add("dateto", String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddYears(1))); //(string) - data(w formacie yyyy - mm - dd) zakończenia promocji
            //                    spo.Add("promoprice", pc.PriceBruttoShop);
            //                }
            //                else // jeśli blokjemy rabaty to trzeba ustawic cenę promocyjną jako cenę regularną. Hack
            //                {
            //                    spo.Add("datefrom", String.Format("{0:yyyy-MM-dd}", DateTime.Now)); //(string) - data(w formacie yyyy - mm - dd) rozpoczęcia promocji
            //                    spo.Add("dateto", String.Format("{0:yyyy-MM-dd}", pc.PriceBruttoPromoDate.Value)); //(string) - data(w formacie yyyy - mm - dd) zakończenia promocji
            //                    spo.Add("promoprice", pc.PriceBruttoPromo);
            //                }


            //                d.Add("specialOffer", spo);//'] (array) - wymagana tablica asocjacyjna z informacjami o magazynie wariantu 
            //            }
            //            else
            //            {
            //                //if (createProduct == false)
            //                //{
            //                //    Object[] methodParams = { session, "product.promo.delete", new Object[] { pc.ShopProductId.Value, true } };
            //                //    var r = SendApiRequest("call", methodParams);

            //                //}

            //            }


            //        }


            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Quantity)
            //            || CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Delivery)
            //            )
            //        {
            //            int deliveryTime = Bll.Helper.GetSetting("SHOPMINDEL").IntValue.Value;
            //            int shopDeliveryTimeId = GetDeliveryTime(pc.LeftQuantity, GetDeliveryIdForShop(pc.DeliveryId, sources), deliveryTime);
            //            s.Add("delivery_id", shopDeliveryTimeId);

            //        }

            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Quantity))
            //        {
            //            s.Add("stock", GetStock(pc.SupplierQuantity, pc.LeftQuantity, pc.IsAvailable, pc.IsDiscontinued));//'] (float) - stan magazynowy
            //        }

            //        //s.Add("stock_relative", );//'] (float) - różnica stanu magazynowego o jaką ma zostać zaktualizowa wartość w sklepie. Jeśli ten parametr występuje, parametr 'stock' nie jest brany pod uwagę podczas aktualizacji danych.
            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.All))
            //            s.Add("warn_level", 5);//'] (float) - alarm magazynowy
            //                                   //s.Add("sold", );//'] (float) - ilość sprzedanego towaru
            //                                   //s.Add("sold_relative", );//'] (float) - różnica ilości sprzedanego towaru o jaką ma zostać zaktualizowa wartość w sklepie. Jeśli ten parametr występuje, parametr 'sold' nie jest brany pod uwagę podczas aktualizacji danych.
            //                                   //s.Add("weight", );//'] (float) - waga towaru
            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Status))
            //            s.Add("availability_id", GetAvailability(pc));//'] (int) - identyfikator dostępności wariantu


            //        /*  if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopUpdateType.All))
            //              if (pc.Supplier.ShopDeliveryDays.HasValue)
            //                  s.Add("delivery_id", pc.Supplier.ShopDeliveryDays.Value);//'] (int]) - identyfikator czasu dostawy wariantu
            //              else
            //                  s.Add("delivery_id", 5);//'] (int]) - identyfikator czasu dostawy wariantu
            //          *///s.Add("gfx_id", );//'] (int|null) - identyfikator zdjęcia produktu, które przedstawia dany warian
            //        if (s.Count > 0)
            //            d.Add("stock", s);//'] (array) - wymagana tablica asocjacyjna z informacjami o magazynie wariantu 


            //        Dictionary<string, object> dt = new Dictionary<string, object>();
            //        Dictionary<string, object> t = new Dictionary<string, object>();


            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Name))
            //            t.Add("name", GetName(shop, pc));//'] (string) - nazwa produktu
            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Description))
            //        {
            //            Dal.ProductCatalogDescriptionFnResult desc = GetDescription(pc.ProductCatalogId, Dal.Helper.Shop.Lajtitpl);
            //            if (desc != null)
            //            {
            //                t.Add("short_description", desc.ShortDescription);//'] (string) - krótki opis produktu
            //                t.Add("description", desc.LongDescription);//'] (string) - opis produktu
            //            }
            //            else
            //            {
            //                t.Add("short_description", null);//'] (string) - krótki opis produktu
            //                t.Add("description", null);//'] (string) - opis 

            //            }
            //        }
            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.Status))
            //            //t.Add("active", pc.IsActiveOnline);//'] (int [0/1]) - aktywność produktu
            //            t.Add("active", true);//'] (int [0/1]) - aktywność produktu ZAWSZE AKTYWNE, ZMIENIAMY JEDYNIE WIDOCZNOŚĆ OFERTY
            //                                  //t.Add("seo_title", );//'] (string) - tytuł wyświetlany w tagu <title>
            //                                  //t.Add("seo_description", );//'] (string) - opis wyświetlany w tagu meta description
            //                                  //t.Add("seo_keywords", );//'] (string) - opis wyświetlany w tagu meta keywords
            //                                  //if(!String.IsNullOrEmpty(pc.Ean))
            //                                  //    t.Add("seo_url", GetFriendlyUrl(pc));//'] (string) - opis wyświetlany w tagu meta keywords
            //        if (CanUpdateField(createProduct, schedules, Dal.Helper.ShopColumnType.All))
            //            t.Add("order", pc.ShopProductPriority ?? pc.ProductCatalogId);//'] (int) - priorytet brany pod uwagę podczas sortowania listy produktów
            //                                                                          //t.Add("main_page", );//'] (int [0/1]) - czy produkt został wyróżniony na stronie głównej
            //                                                                          //t.Add("main_page_order", );//'] (int) - priorytet brany pod uwagę podczas sortowania listy produktów na stronie głównej

            //        if (t.Count > 0)
            //        {
            //            dt.Add("pl_PL", t);
            //            d.Add("translations", dt);
            //        }

            //        return d;

            //    }
            //    catch (Exception ex)
            //    {
            //        Bll.ErrorHandler.LogError(ex, String.Format("SetProduct. ProductCatalogId: {0}", pc.ProductCatalogId));
            //        //result = false;
            //        throw ex;

            //    }
            //}
            //private static string GetName(Dal.Helper.Shop shop, Dal.ProductCatalogFnResult product)
            //{
            //    if (!String.IsNullOrEmpty(product.Name))
            //    {
            //        return product.Name;

            //    }
            //    return Mixer.GetProductName((int)shop, product.ProductCatalogId);
                 

            //}
            #endregion

            #region Attrybuty


            public void CreateAttributeGroups(Dal.Helper.Shop shop, int numberOfGroups)
            {
                for(int i = 1; i<=numberOfGroups; i++)
                {
                   //reateAttributeGroup("tmp", false);
                    Bll.ShopRestHelper.AttributeGroups.CreateAttributeGroup(shop, "tmp", false);

                }
            }


            //public int CreateAttributeGroup(string groupName, bool isActive)
            //{
            //    /*
            //     data (array) - tablica asocjacyjna z danymi obiektu o strukturze:
            //    ['name'] (string) - nazwa grupy atrybutów
            //    ['lang_id'] (int) - identyfikator języka
            //    ['active'] (int[0/1]) - aktywność grupy atrybutów
            //    ['categories'] (array) - tablica identyfikatorów kategorii, przypisanych do tej grupy atrybutów
            //    */

            //    Dictionary<string, object> d = new Dictionary<string, object>();

            //    d.Add("name", groupName);
            //    d.Add("lang_id", "1");
            //    d.Add("active", isActive);
            //    d.Add("categories", null);


            //    Object[] methodParams = { SessionId, "attribute.group.create", new Object[] { d } };
            //    var r = Bll.ShopHelper.SendApiRequest("call", methodParams);


            //    return (int)r;
            //}
            private void UpdateAttributeGroup(int externalGroupId, ProductCatalogAttributeGroup group )
            {
                Dictionary<string, object> d = new Dictionary<string, object>();

                d.Add("name", group.Name);
                d.Add("lang_id", "1");
                d.Add("active", group.ExportToShop);
                


                Object[] methodParams = { SessionId, "attribute.group.save", new Object[] { externalGroupId, d, true } };
                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);


            }
            //public bool UpdateAttributeGroup(Dal.Helper.Shop shop, int externalGroupId, bool isActive, bool isSearchable)
            //{
            //    Dal.ShopHelper sh = new Dal.ShopHelper();
            //    List<Dal.ShopAttributeGroup> shopAttributeGroups = sh.ShopAttributeGroups((int)shop);

            //    Dal.ShopAttributeGroup group = shopAttributeGroups.Where(x => x.ExternalShopAttributeGroupId == externalGroupId).FirstOrDefault();
            //    Dal.ProductCatalogShopAttribute pcsa = null;


            //    if (group != null)
            //    {

            //        pcsa = sh.GetProductCatalogShopAttributeByExternalAttributeGroupId(shop, group.ExternalShopAttributeGroupId);


            //        Dictionary<string, object> d = new Dictionary<string, object>();

            //        d.Add("name", group.Name);
            //        d.Add("lang_id", "1");
            //        d.Add("active", isActive?1:0);
            //        d.Add("filters", isSearchable?1:0);



            //        Object[] methodParams = { SessionId, "attribute.group.save", new Object[] { externalGroupId, d, true } };
            //        var r = (int)Bll.ShopHelper.SendApiRequest("call", methodParams);

            //        return r == 1;
            //    }

            //    return false ;


            //}


            private void UpdateAttributeGroup(int externalGroupId, ProductCatalogAttributeGroup group, int[] categories)
            {
                Dictionary<string, object> d = new Dictionary<string, object>();

                d.Add("name", group.Name);
                d.Add("lang_id", "1");
                d.Add("active", group.ExportToShop);
                if (categories != null)
                    d.Add("categories", categories);


                Object[] methodParams = { SessionId, "attribute.group.save", new Object[] { externalGroupId, d, true } };
                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);


            }
            public int CreateAttribute(int groupId, Dal.ProductCatalogAttributeGroup group, string[] attributes)
            {
                /*
                 data (array) - tablica asocjacyjna z danymi obiektu o strukturze:
                ['name'] (string) - nazwa grupy atrybutów
                ['lang_id'] (int) - identyfikator języka
                ['active'] (int[0/1]) - aktywność grupy atrybutów
                ['categories'] (array) - tablica identyfikatorów kategorii, przypisanych do tej grupy atrybutów
                */

                Dictionary<string, object> d = new Dictionary<string, object>();

                d.Add("pres_id", groupId);
                d.Add("name", group.Name);
                d.Add("type", group.AttributeGroupTypeId == 3 ? "0" : "2");
                d.Add("active", group.ExportToShop);

                if (group.AttributeGroupTypeId != 3)
                    d.Add("options", attributes);


                Object[] methodParams = { SessionId, "attribute.create", new Object[] { d } };
                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);


                return (int)r;
            }
          

            public int CreateAttribute(int groupId, Dal.ProductCatalogAttribute attribute)
            {
                /*
                 data (array) - tablica asocjacyjna z danymi obiektu o strukturze:
                ['name'] (string) - nazwa grupy atrybutów
                ['lang_id'] (int) - identyfikator języka
                ['active'] (int[0/1]) - aktywność grupy atrybutów
                ['categories'] (array) - tablica identyfikatorów kategorii, przypisanych do tej grupy atrybutów
                */

                Dictionary<string, object> d = new Dictionary<string, object>();

                d.Add("pres_id", groupId);
                d.Add("name", attribute.Name);
                d.Add("type", 0);
                d.Add("active", attribute.ProductCatalogAttributeGroup.ExportToShop);
               


                Object[] methodParams = { SessionId, "attribute.create", new Object[] { d } };
                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);


                return (int)r;
            }
            private void UpdateAttribute(int externalGroupId, int externalAttributeId, ProductCatalogAttributeGroup group, string[] attributes)
            {
                Dictionary<string, object> d = new Dictionary<string, object>();

                //d.Add("pres_id", externalGroupId);
                d.Add("name", group.Name);
                d.Add("type", "2");
                d.Add("active", group.ExportToShop);
                d.Add("options", attributes);


                Object[] methodParams = { SessionId, "attribute.save", new Object[] { externalAttributeId, d, true } };
                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);

            }

            public AttributeGroup[] GetAttributeGroups(Dal.Helper.Shop shop)
            {

                Object[] methodParams = { SessionId, "attribute.group.list", new Object[] { true, true, null } };
                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);


                var json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = Int32.MaxValue;
                AttributeGroup[] groups = json_serializer.Deserialize<AttributeGroup[]>(Bll.RESTHelper.ToJson(r));

                return groups;
            }
                internal void SetAttributeGroupsClear(Dal.Helper.Shop shop, int startingId)
            { 

                Object[] methodParams = { SessionId, "attribute.group.list", new Object[] { true, true, null } };
                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);
                var json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = Int32.MaxValue;
                AttributeGroup[] groups = json_serializer.Deserialize<AttributeGroup[]>(Bll.RESTHelper.ToJson(r));


                // int[] i = (Bll.ShopHelper.SendApiRequest("call", methodParams) as object[]).Cast<int>().ToArray();

                //int[] toDelete = i.Where(x => x >= startingId).ToArray();

                AttributeGroup[] toDelete = groups.Where(x => x.pres_id >= startingId).ToArray();


                //int externalGroupId = 48;

                //UpdateAttributeGroup(externalGroupId, new ProductCatalogAttributeGroup { Name = "tmp", ExportToShop = true }, new int[] { });


                //Object[] methodParams2 = { SessionId, "attribute.group.delete", new Object[] { externalGroupId, true } };
                //var r = Bll.ShopHelper.SendApiRequest("call", methodParams2);

                foreach (AttributeGroup g in toDelete)
                {

                    UpdateAttributeGroup(g.pres_id, new ProductCatalogAttributeGroup { Name = "tmp", ExportToShop = false }, new int[] { });

                    foreach (int id in g.attributes.Select(x => x.attribute_id).ToArray())
                        UpdateAttributeDelete(id);


                    //Object[] methodParams2 = { SessionId, "attribute.group.list.delete", new Object[] { i, true } };
                    //var r = Bll.ShopHelper.SendApiRequest("call", methodParams2);

                }


                //Object[] methodParams2 = { SessionId, "attribute.group.list.delete", new Object[] { i ,true} };
                //var r = Bll.ShopHelper.SendApiRequest("call", methodParams2);


            }
            internal void SetAttributeGroupsDelete(Dal.Helper.Shop shop, int startingId, int endingId)
            {

                Object[] methodParams = { SessionId, "attribute.group.list", new Object[] { true, true, null } };
                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);
                var json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = Int32.MaxValue;
                AttributeGroup[] groups = json_serializer.Deserialize<AttributeGroup[]>(Bll.RESTHelper.ToJson(r));
 
                AttributeGroup[] toDelete = groups.Where(x => x.pres_id >= startingId && x.pres_id<endingId).ToArray();

                 
                foreach (AttributeGroup g in toDelete)
                {

                    UpdateAttributeGroup(g.pres_id, new ProductCatalogAttributeGroup { Name = "tmp", ExportToShop = false }, new int[] { });

                    foreach (int id in g.attributes.Select(x => x.attribute_id).ToArray())
                        UpdateAttributeDelete(id);


                    Object[] methodParams2 = { SessionId, "attribute.group.delete", new Object[] { g.pres_id, true } };
                    var re = Bll.ShopHelper.SendApiRequest("call", methodParams2);

                }

                 


            }
            internal void SetAttributeGroupsDeleteCategories(Dal.Helper.Shop shop, params int[] groupIds)
            {
                foreach (int groupId in groupIds)
                {
                    Dictionary<string, object> d = new Dictionary<string, object>();

                    // d.Add("name", group.Name);
                    // d.Add("lang_id", "1");
                    // d.Add("active", group.ExportToShop); 
                    d.Add("categories", new int[] { });


                    Object[] methodParams = { SessionId, "attribute.group.save", new Object[] { groupId, d, true } };
                    var r = Bll.ShopHelper.SendApiRequest("call", methodParams);

                }


            }
            private void UpdateAttributeDelete(int id)
            {
                Object[] methodParams = { SessionId, "attribute.delete", new Object[] { id, true} };
                var r = Bll.ShopHelper.SendApiRequest("call", methodParams);
            }

            //public void CreateAttributeGroups(Dal.Helper.Shop shop)
            //{
            //    Dal.ShopHelper sh = new Dal.ShopHelper();
            //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            //    List<Dal.ProductCatalogAttributeGroup> groups = sh.GetProductCatalogAttributeGroupsNotInShop(shop).ToList();


            //    foreach (Dal.ProductCatalogAttributeGroup group in groups)
            //    {

            //        int externalGroupId = Bll.ShopRestHelper.AttributeGroups.CreateAttributeGroup(shop, group.Name, group.ExportToShop);

            //        int shopAttributeGroupId = sh.SetShopAttributeGroup(shop, group.ExportToShop, group.Name, externalGroupId);

            //        string[] attributeOptions = pch.GetProductCatalogAttributes(group.AttributeGroupId).Select(x => x.Name).ToArray();


            //        int externalId = CreateAttribute(externalGroupId, group, attributeOptions);

            //        if (group.AttributeGroupTypeId != 3)
            //        {
            //            int shopAttributeId = sh.SetShopAttribute(shop, group.ExportToShop, group.Name, externalId, shopAttributeGroupId, 2);


            //            sh.SetProductCatalogShopAttribute(shopAttributeId, null, group.AttributeGroupId);
            //        }

            //    }

            //    List<Dal.ProductCatalogAttribute> attributes = sh.GetProductCatalogAttributesNotInShop(shop).ToList();

            //    pętla tworzy i uzupełnia atrybuty jeśli jakieś nowe doszly
            //    foreach (Dal.ProductCatalogAttribute attribute in attributes)
            //    {
            //        sprawdz czy już utworzono grupę w sklepie
            //        Dal.ProductCatalogShopAttribute psa = sh.GetProductCatalogShopAttributeByAttributeId(shop, attribute.AttributeId);
            //        int externalGroupId = 0;
            //        int shopAttributeGroupId = 0;
            //        if (psa != null)
            //        {
            //            externalGroupId = psa.ShopAttribute.ShopAttributeGroup.ExternalShopAttributeGroupId;
            //            shopAttributeGroupId = psa.ShopAttribute.ShopAttributeGroupId;
            //        }
            //        else
            //        {    // utwórz ShopAttributeGroup
            //            externalGroupId = Bll.ShopRestHelper.AttributeGroups.CreateAttributeGroup(shop, attribute.ProductCatalogAttributeGroup.Name, attribute.ProductCatalogAttributeGroup.ExportToShop);
            //            shopAttributeGroupId = sh.SetShopAttributeGroup(shop, attribute.ProductCatalogAttributeGroup.ExportToShop, attribute.ProductCatalogAttributeGroup.Name, externalGroupId);
            //        }
            //        utwórz ShopAttribute

            //        int externalId = CreateAttribute(externalGroupId, attribute);
            //        int shopAttributeId = sh.SetShopAttribute(shop, attribute.ProductCatalogAttributeGroup.ExportToShop, attribute.Name, externalId, shopAttributeGroupId, 2);

            //        utwórz ProductCatalogShopAttribute

            //        sh.SetProductCatalogShopAttribute(shopAttributeId, attribute.AttributeId, null);


            //        int externalGroupId = GetS CreateAttributeGroup(group.Name, group.ExportToShop);

            //        int shopAttributeGroupId = sh.SetShopAttributeGroup(shop, group.ExportToShop, group.Name, externalGroupId);

            //        List<Dal.ProductCatalogAttribute> attributes = pch.GetProductCatalogAttributes(group.AttributeGroupId).ToList();

            //        foreach (Dal.ProductCatalogAttribute attribute in attributes)
            //        {
            //            int externalId = CreateAttribute(externalGroupId, attribute);

            //            int shopAttributeId = sh.SetShopAttribute(shop, group, externalGroupId, externalId, shopAttributeGroupId, 0);


            //            sh.SetProductCatalogShopAttribute(shopAttributeId, attribute.AttributeId, null);
            //        }

            //    }

            //}

            //public void UpdateAttribute(Dal.Helper.Shop shop, int attributeGroupId)
            //{
            //    Dal.ShopHelper sh = new Dal.ShopHelper();
            //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            //    Dal.ProductCatalogAttributeGroup group = pch.GetProductCatalogAttributeGroup(attributeGroupId);
            //    Dal.ProductCatalogShopAttribute sss = sh.GetProductCatalogShopAttributeByAttributeGroupId(shop, group.AttributeGroupId);

            //    if (sss == null)
            //        return;


            //    int externalGroupId = sss.ShopAttribute.ShopAttributeGroup.ExternalShopAttributeGroupId;
            //    int externalAttributeId = sss.ShopAttribute.ExternalShopAttributeId;



            //    UpdateAttributeGroup(externalGroupId, group, null);

            //    int shopAttributeGroupId = sh.SetShopAttributeGroup(shop, group.ExportToShop, group.Name, externalGroupId);

            //    string[] attributes = pch.GetProductCatalogAttributes(group.AttributeGroupId).Select(x => x.Name).ToArray();


            //    UpdateAttribute(externalGroupId, externalAttributeId, group, attributes);



            //}


            //public void CreateAttributeGroupAndAssign(Dal.Helper.Shop shop, int pres_id, int attributeGroupId)
            //{
            //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            //    Dal.ProductCatalogAttributeGroup group = pch.GetProductCatalogAttributeGroup(attributeGroupId);
            //    List<Dal.ProductCatalogAttribute> attributes = pch.GetProductCatalogAttributes(attributeGroupId);

            //    int externalAttributeGroupId = pres_id;
            //    UpdateAttributeGroup(externalAttributeGroupId, group);

            //    Dal.ShopAttributeGroup sag = new ShopAttributeGroup()
            //    {
            //        IsActive = false,
            //        IsSearchable = false,
            //        Name = group.Name,
            //        ShopId = (int)shop,
            //        SortOrder = pres_id,
            //        ExternalShopAttributeGroupId = externalAttributeGroupId
            //    };
            //    List<Dal.ShopAttribute> sas = new List<ShopAttribute>();
            //    List<Dal.ProductCatalogShopAttribute> pcsas = new List<ProductCatalogShopAttribute>();
            //    if (group.AttributeGroupTypeId == 3)
            //    {
            //        foreach (Dal.ProductCatalogAttribute attribute in attributes)
            //        {
            //            int extAttId = CreateAttribute(externalAttributeGroupId, attribute);

            //            Dal.ShopAttribute sa = new ShopAttribute()
            //            {
            //                ShopAttributeGroup = sag,
            //                ExternalShopAttributeId = extAttId,
            //                SortOrder = attribute.SortOrder,
            //                Name = attribute.Name,
            //                ExternalAttributeTypeId = 0,
            //                IsActive = false
            //            };
            //            sas.Add(sa);


            //            Dal.ProductCatalogShopAttribute pcsa = new ProductCatalogShopAttribute()
            //            {
            //                AttributeGroupId = attributeGroupId,
            //                AttributeId = attribute.AttributeId,
            //                ShopAttribute = sa
            //            };

            //            pcsas.Add(pcsa);
            //        }
            //    }
            //    else
            //    {
            //        int extAttId = CreateAttribute(externalAttributeGroupId, group, attributes.Select(x => x.Name).ToArray());

            //        Dal.ShopAttribute sa = new ShopAttribute()
            //        {
            //            ShopAttributeGroup = sag,
            //            ExternalShopAttributeId = extAttId,
            //            SortOrder = 0,
            //            Name = group.Name,
            //            ExternalAttributeTypeId = 2,
            //            IsActive = false
            //        };
            //        sas.Add(sa);
            //        Dal.ProductCatalogShopAttribute pcsa = new ProductCatalogShopAttribute()
            //        {
            //            AttributeGroupId = attributeGroupId,
            //            AttributeId = null,
            //            ShopAttribute = sa
            //        };

            //        pcsas.Add(pcsa);
            //    }

            //    Dal.ShopHelper sh = new Dal.ShopHelper();
            //    sh.CreateAttributeGroupAndAssign(pcsas);



            //}


            #endregion
        }


    }
}
