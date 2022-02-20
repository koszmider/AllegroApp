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
        public static List<Supplier> GetSuppliersByOwner(int supplierOwnerId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Supplier.Where(x => x.SupplierOwnerId == supplierOwnerId).OrderBy(x => x.Name).ToList();
            }
        }
        public static List<SupplierShop> GetSuppliersShop(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierShop.Where(x => x.ShopId == shopId).ToList();
            }
        }
        public static void SetSupplierShop(SupplierShop ss)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.SupplierShop ssToUpdate = ctx.SupplierShop.Where(x => x.SupplierId == ss.SupplierId && x.ShopId == ss.ShopId).FirstOrDefault();

                ssToUpdate.IsDescriptionActive = ss.IsDescriptionActive;
                ssToUpdate.LongDescription = ss.LongDescription;

                ctx.SubmitChanges();
            }
        }
        public static List<SupplierShop> GetSupplierShopsByShopId(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<SupplierShop>(x => x.Shop);
                dlo.LoadWith<SupplierShop>(x => x.ShopProducer);
                dlo.LoadWith<SupplierShop>(x => x.Supplier);
                ctx.LoadOptions = dlo;
                return ctx.SupplierShop.Where(x => x.ShopId == shopId). OrderBy(x=>x.Supplier.Name).ToList();
            }
        }
        public static List<SupplierShop> GetSupplierShops(int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<SupplierShop>(x => x.Shop);
                dlo.LoadWith<SupplierShop>(x => x.ShopProducer);
                ctx.LoadOptions = dlo;
                return ctx.SupplierShop.Where(x => x.SupplierId == supplierId).ToList();
            }
        }
        public static List<int> GetSupplierShop(Helper.ShopType shopType)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierShop.Where(x => x.IsActive && x.Shop.ShopTypeId == (int) shopType)
                    .Select(x => x.SupplierId).ToList();
            }
        }
        public static List<Supplier> GetSuppliers()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Supplier>(x => x.SupplierImportType);
                dlo.LoadWith<Supplier>(x => x.AllegroDeliveryCostType);
                ctx.LoadOptions = dlo;

                return ctx.Supplier.Where(x => x.IsActive).ToList();
            }

        }
        public static List<Supplier> GetSuppliers(int[] supplierIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                //dlo.LoadWith<Supplier>(x => x.AllegroUser);
                ctx.LoadOptions = dlo;

                return ctx.Supplier.Where(x => supplierIds.Contains(x.SupplierId)).ToList();
            }

        }
        public static   List<Supplier> GetSuppliersWithQuantityTrackingEnabled()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Supplier
                    .Where(x => x.IsQuantityTrackingAvailable)
                    .ToList();
            }
        }
        public static void SetSupplierUpdate(Supplier supplier)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Supplier supplierToUpdate = ctx.Supplier.Where(x => x.SupplierId == supplier.SupplierId).FirstOrDefault();

                supplierToUpdate.IsActive = supplier.IsActive;
                supplierToUpdate.Margin = supplier.Margin;
                supplierToUpdate.Name = supplier.Name;
                supplierToUpdate.Rebate = supplier.Rebate;
                supplierToUpdate.ShowSupplierInAllegro = supplier.ShowSupplierInAllegro;
                supplierToUpdate.DeliveryFixedId = supplier.DeliveryFixedId;
                supplierToUpdate.DeliveryCostTypeId = supplier.DeliveryCostTypeId;
                supplierToUpdate.ImportUrl = supplier.ImportUrl;
                supplierToUpdate.ImportComment = supplier.ImportComment;
                supplierToUpdate.ImportTypeId = supplier.ImportTypeId;
                supplierToUpdate.UpdateUser = supplier.UpdateUser;
                supplierToUpdate.UpdateReason = supplier.UpdateReason;
                supplierToUpdate.SupplierOwnerId = supplier.SupplierOwnerId;
                supplierToUpdate.IsQuantityTrackingAvailable = supplier.IsQuantityTrackingAvailable;
                supplierToUpdate.RoundPriceTypeId = supplier.RoundPriceTypeId;
                supplierToUpdate.OrderingTypeId = supplier.OrderingTypeId;
                supplierToUpdate.B2bEmail = supplier.B2bEmail;
                supplierToUpdate.B2bUrl = supplier.B2bUrl;
                supplierToUpdate.IsDropShippingAvailable = supplier.IsDropShippingAvailable;
                supplierToUpdate.DeliveryCostTypeNoPaczkomatId = supplier.DeliveryCostTypeNoPaczkomatId;
                supplierToUpdate.QuantityMinLevel = supplier.QuantityMinLevel;
                supplierToUpdate.CountryCode = supplier.CountryCode;
                supplierToUpdate.OrderWeekDays = supplier.OrderWeekDays;

                ctx.SubmitChanges();
            }
        }
        public static List<SupplierOwner> GetSupplierOwners()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierOwner.ToList();
            }
        }
    }
}
