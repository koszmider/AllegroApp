using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
    public class Shop
    {
        public static Dal.Shop GetShop(int shopId)
        {

            using (LajtitDB ctx = new LajtitDB())

            {
                return ctx.Shop.Where(x => x.ShopId == shopId).FirstOrDefault();
            }
        }

        public static void SetShop(Dal.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Shop shopToUpdate = ctx.Shop.Where(x => x.ShopId == shop.ShopId).FirstOrDefault();

                shopToUpdate.Name = shop.Name;
                shopToUpdate.ClientSecret = shop.ClientSecret;
                shopToUpdate.Template = shop.Template;
                shopToUpdate.MinPrice = shop.MinPrice;
                shopToUpdate.MaxPrice = shop.MaxPrice;


                ctx.SubmitChanges();
            }
        }

        public static Dal.Shop GetShopByExternalId(long externalShopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Shop.Where(x => x.ExternalId == (int)externalShopId).FirstOrDefault();
            }
        }

        public static List<Dal.Shop> GetShops(Helper.ShopEngineType shopEngineType)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Shop.Where(x => x.ShopType.ShopEngineTypeId == (int)shopEngineType).ToList();
            }
        }

        public static List<ProductCatalogAttributeShopGroupingType> GetProductCatalogAttributeShopGroupingTypes(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeShopGroupingType.Where(x => x.ShopId == (int)shopId).ToList();
            }
        }

        public static List<Dal.Shop> GetShops(Helper.ShopType shopType)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Shop.Where(x => x.ShopTypeId == (int)shopType).ToList();
            }
        }

        public static ProductCatalogAttributeShopGrouping GetProductCatalogAttributeShopGrouping(int shopId, int attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeShopGrouping
                    .Where(x => x.AttributeId == attributeId && x.ProductCatalogAttributeShopGroupingType.Shop.ShopId == shopId)
                    .FirstOrDefault();

            }
        }

        public static void SetProductCatalogAttributeShopGrouping(int shopId, ProductCatalogAttributeShopGrouping g)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (g.ShopGroupingTypeId == 0)
                {
                    var d = ctx.ProductCatalogAttributeShopGrouping.Where(x => x.AttributeId == g.AttributeId && x.ProductCatalogAttributeShopGroupingType.Shop.ShopId == shopId).FirstOrDefault();

                    if (d != null)
                    {
                        ctx.ProductCatalogAttributeShopGrouping.DeleteOnSubmit(d);
                        ctx.SubmitChanges();
                    }

                }
                else
                if (ctx.ProductCatalogAttributeShopGrouping.Where(x => x.AttributeId == g.AttributeId && x.ShopGroupingTypeId == g.ShopGroupingTypeId).FirstOrDefault() == null)
                {
                    ctx.ProductCatalogAttributeShopGrouping.InsertOnSubmit(g);
                    ctx.SubmitChanges();
                }
            }
        }

        public static List<Dal.Shop> GetShops()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Dal.Shop>(x => x.ShopType);
                ctx.LoadOptions = dlo;
                return ctx.Shop.ToList();
            }
        }
        public static List<ShopType> GetShopTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopType.ToList();
            }
        }

        public static ShopStatus GetShopOrderStatus(Helper.ShopType shopType, int orderStatusId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopStatus.Where(x => x.ShopTypeId == (int)shopType && x.OrderStatusId == orderStatusId).FirstOrDefault();
            }
        }

        public static List<ShopCurrency> GetShopCurrency(Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopCurrency.Where(x => x.ShopId == (int)shop).ToList();
            }
        }

        public static List<ShopOrder> GetShopOrdersForStatusUpdate(Helper.Shop shop)
        {
        
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Dal.ShopOrder>(x => x.Order);
                dlo.LoadWith<Dal.Order>(x => x.OrderShipping);
                dlo.LoadWith<Dal.OrderShipping>(x => x.ShippingCompany);
                ctx.LoadOptions = dlo;

                return ctx.ShopOrder
                    .Where(x => x.ShopId == (int)shop && x.UpdateStatus == true && x.Order.OrderStatusId == (int)Dal.Helper.OrderStatus.Sent)
                    .ToList();
            }
        }

        public static List<CeneoClicksSupplierFnResult> GetCeneoClicksBySupplier(DateTime? dateFrom, DateTime? dateTo)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.CeneoClicksSupplierFn(dateFrom.Value, dateTo.Value)
                    .ToList();
            }
            }

        public static void SetCeneoClick(List<CeneoClicks> clicks)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                foreach (CeneoClicks click in clicks)
                    if (ctx.CeneoClicks.Where(x => x.ShopId == click.ShopId && x.ShopProductId == click.ShopProductId && x.Date == click.Date && x.IP == click.IP).Count() == 0)
                    {
                        try
                        {
                            ctx.CeneoClicks.InsertOnSubmit(click); 
                            ctx.SubmitChanges();
                        }
                        catch (Exception ex)
                        {

                        }
                    }

            }
        }

        public static void SetProductCatalogShopProductId(Helper.Shop shop, List<ProductCatalogShopProduct> products)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach (Dal.ProductCatalogShopProduct psp in products.Where(x => x.ShopProductId != null).ToList())
                {
                    Dal.ProductCatalogShopProduct pspToUpdate = ctx.ProductCatalogShopProduct
                        .Where(x => x.ShopId == (int)shop && x.ProductCatalogId == psp.ProductCatalogId)
                        .FirstOrDefault();
                    if (pspToUpdate != null)
                        pspToUpdate.ShopProductId = psp.ShopProductId;


                }
                ctx.SubmitChanges();
            }
        }

        public static DateTime? GetCeneoClicksLast()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var c = ctx.CeneoClicks.OrderByDescending(x => x.Date).FirstOrDefault();
                if (c == null)
                    return null;
                else
                    return c.Date;
            }
        }

        public static List<SystemDictionary> GetSystemDictionary(string countryCode)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemDictionary.Where(x => x.CountryCode == countryCode).ToList();
            }
        }

        public static List<ShopAttribute> GetShopAttributes(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Dal.ShopAttribute>(x => x.ShopAttributeGroup); 
                ctx.LoadOptions = dlo;

                return ctx.ShopAttribute.Where(x => x.ShopAttributeGroup.ShopId == shopId && x.ExternalAttributeTypeId == 2).ToList();
            }
        }
        public static ProductCatalogAttributeShopGroupingType GetProductCatalogAttributeShopGroupingType(int shopId, int shopAttributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeShopGroupingType.Where(x => x.ShopId == shopId && x.ShopAttributeId == shopAttributeId).FirstOrDefault();
            }
        }
        public static List<ProductCatalogAttribute> GetProductCatalogAttributeShopGrouping(int shopGroupingTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeShopGrouping.Where(x => x.ShopGroupingTypeId == shopGroupingTypeId)
                    .Select(x => x.ProductCatalogAttribute)
                    .ToList();

            }
        }
        public static List<ProductCatalogAttribute> GetProductCatalogShopAttributes(int shopId, int shopAttributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var g = ctx.ProductCatalogShopAttribute.Where(x => x.ShopAttribute.ShopAttributeGroup.ShopId == shopId && x.ShopAttributeId == shopAttributeId)
                    .Select(x => x.ProductCatalogAttributeGroup).FirstOrDefault();

                if (g != null)
                    return ctx.ProductCatalogAttribute.Where(x => x.AttributeGroupId == g.AttributeGroupId).ToList();


                return new List<ProductCatalogAttribute>();

            }
        }
    }
}