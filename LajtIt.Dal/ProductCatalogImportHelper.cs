using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace LajtIt.Dal
{
    public class ProductCatalogImportHelper
    {
        public List<ProductCatalogImport> GetImports()
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogImport.ToList();
            }
        }

        public ProductCatalogImport GetImport(int ImportId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogImport.Where(x => x.ImportId == ImportId).FirstOrDefault();
            }
        }

        public List<Cost> GetImportCosts(int ImportId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Cost>(x => x.CostType);
                dlo.LoadWith<Cost>(x => x.Company);
                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogImportCost.Where(x => x.ImportId == ImportId).Select(x => x.Cost).ToList();
            }
        }

        public void SetImportCost(int costId, int importId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogImportCost.InsertOnSubmit(new ProductCatalogImportCost()
                {
                     CostId = costId,
                     ImportId = importId
                });
                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogDeliveryView> GetDeliveryView(int productCatalogId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            { 

                return ctx.ProductCatalogDeliveryView.Where(x => x.ProductCatalogId == productCatalogId).OrderByDescending(x => x.DeliveryId).ToList();
            }
        }

        public List<ProductCatalogDelivery> GetDeliveries(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                return ctx.ProductCatalogDelivery.Where(x => x.ProductCatalogId == productCatalogId).OrderByDescending(x => x.DeliveryId).ToList();
            }
        }

        public string SetDelivery(ProductCatalogDelivery delivery)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (delivery.DeliveryId == 0)
                {
                    if (delivery.OrderId.HasValue)
                    {
                        ProductCatalogDelivery d = ctx.ProductCatalogDelivery.Where(x => x.ProductCatalogId == delivery.ProductCatalogId
                        &&
                        (
                        (delivery.OrderId.HasValue && x.OrderId == delivery.OrderId.Value)
                        ||
                        (!delivery.OrderId.HasValue)
                        )
                        ).FirstOrDefault();

                        int quantity = 0;

                        var q = ctx.ProductCatalogDelivery.Where(x => x.ProductCatalogId == delivery.ProductCatalogId && x.OrderId == delivery.OrderId.Value).ToList();
                        if (q.Count() > 0)
                        {
                            quantity = q.Sum(x => x.Quantity);
                        }
                   

                        int orderQuantity = ctx.OrderProduct.Where(x => x.ProductCatalogId == delivery.ProductCatalogId
                        && x.OrderId == delivery.OrderId).Sum(x => x.Quantity);

                        //if (d != null)
                        //if (orderQuantity < quantity + delivery.Quantity)
                        //    return d.ProductCatalog.Name;
                    }
                    ctx.ProductCatalogDelivery.InsertOnSubmit(delivery);
                }
                else
                {
                    Dal.ProductCatalogDelivery deliveryToUpdate = ctx.ProductCatalogDelivery.Where(x => x.DeliveryId == delivery.DeliveryId).FirstOrDefault();

                    deliveryToUpdate.Comment = delivery.Comment;
                    deliveryToUpdate.CostId = delivery.CostId;
                    deliveryToUpdate.OrderId = delivery.OrderId;
                    deliveryToUpdate.Price = delivery.Price;
                    deliveryToUpdate.Quantity = delivery.Quantity;
                    deliveryToUpdate.QuantityBlocked = delivery.QuantityBlocked;
                    deliveryToUpdate.WarehouseId = delivery.WarehouseId;

                }
            

                ctx.SubmitChanges();

                return "";
            }
        }

        //public ProductCatalogDeliveryStatsResult GetProductDeliveryStats(int ProductCatalogId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {

        //        return ctx.ProductCatalogDeliveryStats(ProductCatalogId, null).FirstOrDefault();
        //    }
        //}

        public int SetProductCatalogDelivery(ProductCatalogDeliveryDocument doc)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                ctx.ProductCatalogDeliveryDocument.InsertOnSubmit(doc);
                
                ctx.SubmitChanges();

                return doc.DeliveryDocumentId;
            }
        }

        public void SetPurchasePrice(int ProductCatalogId, decimal price)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                ProductCatalog pc = ctx.ProductCatalog.Where(x => x.ProductCatalogId == ProductCatalogId).FirstOrDefault();
                pc.PurchasePrice = price;
                ctx.SubmitChanges();
            }
        }

        public void SetDeliveryUpdate(List<ProductCatalogDelivery> deliveryToAdd, List<ProductCatalogDelivery> deliveryToUpdate)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] deliveryIds = deliveryToUpdate.Select(x => x.DeliveryId).ToArray();
                List<ProductCatalogDelivery> dToUpdates = ctx.ProductCatalogDelivery.Where(x => deliveryIds.Contains(x.DeliveryId)).ToList();

                foreach(ProductCatalogDelivery d in dToUpdates)
                {
                    ProductCatalogDelivery dToUp = deliveryToUpdate.Where(x => x.DeliveryId == d.DeliveryId).FirstOrDefault();
                    d.Quantity = dToUp.Quantity;
                    d.WarehouseId = dToUp.WarehouseId;
                }
                ctx.ProductCatalogDelivery.InsertAllOnSubmit(deliveryToAdd);
                ctx.SubmitChanges();
            }
        }

        public ProductImportStatResult GetImportStat(int importId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                return ctx.ProductImportStat(importId).FirstOrDefault() ;
            }
        }

        public void SetDeliveryUpdate(ProductCatalogDelivery delivery)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogDelivery pcdToUpdate = ctx.ProductCatalogDelivery.Where(x => x.DeliveryId == delivery.DeliveryId).FirstOrDefault();

                pcdToUpdate.Comment = delivery.Comment;
                pcdToUpdate.Price = delivery.Price;
                pcdToUpdate.Quantity = delivery.Quantity;
                pcdToUpdate.QuantityBlocked = delivery.QuantityBlocked;
                pcdToUpdate.WarehouseId = delivery.WarehouseId;
                pcdToUpdate.OrderId = delivery.OrderId;
                ctx.SubmitChanges();
            }
        }
    }
}
