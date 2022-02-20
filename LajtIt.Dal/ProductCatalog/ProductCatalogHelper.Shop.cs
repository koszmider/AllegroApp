using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
    partial class ProductCatalog
    {
        //public static List<ShopFnResult> GetProductCatalogForShop(int shopId, int[] productCatalogIds)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ShopFn(shopId).Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();
        //    }
        //}
        public static List<ShopFnResult> GetProductCatalogForShop(int shopExportId, int shopImportId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopFn(shopExportId, shopImportId).ToList();
            }
        }
        public static ProductCatalogShopProduct SetProductCatalogShopProductAvailable(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogShopProduct pspToUpdate = ctx.ProductCatalogShopProduct.Where(x => x.Id == id).FirstOrDefault();

                pspToUpdate.IsPSAvailable = !pspToUpdate.IsPSAvailable;

                ctx.SubmitChanges();

                return pspToUpdate;
            }
        }
        public static List<ProductCatalogShopProduct> GetProductCatalogShopProductByProductCatalogId(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Dal.ProductCatalogShopProduct>(x => x.Shop);
                dlo.LoadWith<Dal.ProductCatalogShopProduct>(x => x.ProductCatalog);
                dlo.LoadWith<Dal.Shop>(x => x.ShopType);
                //dlo.LoadWith<Dal.Shop>(x => x.SupplierShop);
                //dlo.LoadWith<ProductCatalog>(x => x.ProductCatalogGroup);

                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogShopProduct.Where(x=>x.ProductCatalogId==productCatalogId).ToList();
            }
        }

        public static List<ProductCatalogFnResult> GetProductCatalogShopProduct(int shopId, int[] productCatalogIds)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                return ctx.ProductCatalogFn(shopId).Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();
            }
        }
        public static void SetProductCatalogShopProductDescription(ProductCatalogShopProduct psp)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogShopProduct pspToUpdate = ctx.ProductCatalogShopProduct.Where(x => x.ShopId == psp.ShopId && x.ProductCatalogId == psp.ProductCatalogId).FirstOrDefault();

                if (pspToUpdate != null)
                {
                    pspToUpdate.LongDescription = psp.LongDescription;
                    //pspToUpdate.ShortDescription = psp.ShortDescription;
                }
                else
                {
                    psp.IsNameLocked = false;
                    ctx.ProductCatalogShopProduct.InsertOnSubmit(psp);
                }
                ctx.SubmitChanges();
            }
        }
        public static void SetProductCatalogShopProductDescription(List<ProductCatalogShopProduct> psps)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach (Dal.ProductCatalogShopProduct psp in psps)
                {
                    ProductCatalogShopProduct pspToUpdate = ctx.ProductCatalogShopProduct
                        .Where(x => x.ShopId == psp.ShopId && x.ProductCatalogId == psp.ProductCatalogId).FirstOrDefault();

                    if (pspToUpdate != null)
                    {
                        pspToUpdate.LongDescription = psp.LongDescription;
                       
                    }
                    else
                    {
                        psp.IsNameLocked = false;
                        ctx.ProductCatalogShopProduct.InsertOnSubmit(psp);
                    }
                    ctx.SubmitChanges();
                }
            }
        }
        public static ProductCatalogShopProductFnResult GetProductCatalogShopProduct(Helper.Shop shop, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopProductFn((int)shop).Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();
            }
        }
        public static ProductCatalogShopProductFnResult GetProductCatalogShopProduct(int shopId, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopProductFn(shopId).Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();
            }
        }
        public static List<ProductCatalogShopProductFnResult> GetProductCatalogShopProduct(Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopProductFn((int)shop)
                    .ToList();
            }
        }
        public static List<ProductCatalogShopProductFnResult> GetProductCatalogShopProduct(Helper.Shop shop, int[] supplierIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopProductFn((int)shop)
                    .Where(x=> supplierIds.Contains(x.SupplierId))
                    .ToList();
            }
        }
        public static List<ProductCatalogShopProduct> GetProductCatalogShopProductByShopIds(Dal.Helper.Shop shop, string[] shopProductIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopProduct.Where(x =>
                x.ShopId == (int)shop
                && x.ShopProductId != null
                && shopProductIds.Contains(x.ShopProductId)
                ).ToList();
                //return ctx.ProductCatalog.Where(x => x.ShopProductId.HasValue && shopProductIds.Contains(x.ShopProductId.Value)).ToList();
            }
        }
        public static ProductCatalogShopProduct GetProductCatalogShopProductByShopId(Dal.Helper.Shop shop, 
            string shopProductId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopProduct.Where(x =>
                x.ShopId == (int)shop
                && x.ShopProductId == shopProductId).FirstOrDefault();
             }
        }
        public static void SetShopProductToProductCatalogById(Dal.Helper.Shop shop, int productCatalogId, string shopProductId, bool refreshImages, bool overwriteShopProductId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogShopProduct sp = ctx.ProductCatalogShopProduct.Where(x => x.ShopId == (int)shop && x.ProductCatalogId == productCatalogId).FirstOrDefault();

                if (sp == null)
                {
                    ctx.ProductCatalogShopProduct.InsertOnSubmit(
                        new ProductCatalogShopProduct()
                        {
                            IsNameLocked = false,
                            ProductCatalogId = productCatalogId,
                            ShopId = (int)shop,
                            ShopProductId = shopProductId
                        }
                        );
                }
                else
                {
                    if (sp.ShopProductId == null|| overwriteShopProductId)
                    {
                        sp.ShopProductId = shopProductId;
                    }
                }

                if (refreshImages)
                {
                    // po utworzeniu produktu w sklepie dograjmy zdjęcia
                    ctx.ProductCatalogShopUpdateSchedule.InsertOnSubmit(new ProductCatalogShopUpdateSchedule()
                    {
                        InsertDate = DateTime.Now,
                        InsertUser = "system",
                        ProductCatalogId = productCatalogId,
                        ShopId = (int)shop,
                        ShopColumnTypeId = (int)Dal.Helper.ShopColumnType.Images,
                        UpdateTypeId = (int)Dal.Helper.UpdateScheduleType.OnlineShopSingle,
                        UpdateComment = "Auto wpis",
                        UpdateStatusId = (int)Dal.Helper.ShopUpdateStatus.New
                    });
                }
                ctx.SubmitChanges();

            }
        }
    }
}
