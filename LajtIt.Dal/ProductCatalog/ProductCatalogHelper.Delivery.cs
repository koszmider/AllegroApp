using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
    partial class ProductCatalog
    {
        public class ProductsCostsSearch
        {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public decimal? MarzaFrom { get; set; }
            public decimal? MarzaTo { get; set; }
            public decimal? NarzutFrom { get; set; }
            public decimal? NarzutTo { get; set; }
            public int[] SupplierIds { get; set; }
            public int[] ShopIds { get; set; }

        }

        public static List<ProductCatalogDelivery> GetProductCatalogDeliveries(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogDelivery.Where(x => x.ProductCatalogId == productCatalogId && x.OrderId == null && x.Quantity > 0).ToList();
            }
        }

        public static List<ProductsCostsView> GetProductsCostsView(ProductsCostsSearch pcs)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                var p = ctx.ProductsCostsView.Where(x => x.InsertDate >= pcs.DateFrom && x.InsertDate <= pcs.DateTo.AddDays(1));

                if (pcs.SupplierIds.Length > 0)
                    p = p.Where(x => pcs.SupplierIds.Contains(x.SupplierId));
                if (pcs.ShopIds.Length > 0)
                    p = p.Where(x => pcs.ShopIds.Contains(x.ShopId));
                if (pcs.MarzaFrom.HasValue)
                    p = p.Where(x => x.Marza >= pcs.MarzaFrom.Value);
                if (pcs.MarzaTo.HasValue)
                    p = p.Where(x => x.Marza <= pcs.MarzaTo.Value);
                if (pcs.NarzutFrom.HasValue)
                    p = p.Where(x => x.Narzut >= pcs.NarzutFrom.Value);
                if (pcs.NarzutTo.HasValue)
                    p = p.Where(x => x.Narzut <= pcs.NarzutTo.Value);

                return p.ToList();
            }
        }


        public static List<ProductCatalogDeliveryManagerView> GetDeliveries(DateTime dateFrom, 
            DateTime dateTo, int supplierOwnerId, int price,
            string documentName, bool? hasInvoiceAssigned, string invoiceNumber)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                var q = ctx.ProductCatalogDeliveryManagerView
                    .Where(x => x.InsertDate <= dateTo && x.InsertDate >= dateFrom && x.Quantity>0);

                if (supplierOwnerId > 0)
                    q = q.Where(x => x.SupplierOwnerId == supplierOwnerId);
                if (!String.IsNullOrEmpty(documentName))
                    q = q.Where(x => x.DocumentName.Contains(documentName));
                if (!String.IsNullOrEmpty(invoiceNumber))
                    q = q.Where(x => x.InvoiceNumber.Contains(invoiceNumber));

                if (price>-1)
                    if (price==1)
                        q = q.Where(x => x.Price>0);
                    else
                        q = q.Where(x => x.Price==0);
                if (hasInvoiceAssigned.HasValue)
                    if (hasInvoiceAssigned.Value)
                        q = q.Where(x => x.CostId.HasValue);
                    else
                        q = q.Where(x => x.CostId.HasValue == false);

                return q.OrderByDescending(x=>x.InsertDate).ToList();
            }
        }


        public static void SetProductCatalogAttributeCopy(int productCatalogId2, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogAttributesToProductCopy(productCatalogId2, productCatalogId);
            }
        }

        public static List<Dal.SupplierProductCatalogNotReadyView> GetSupplierNotReadyProducts()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.SupplierProductCatalogNotReadyView.OrderBy(x => x.Name).ToList();
            }
        }

        public static ProductCatalogFileDataFnResult GetItaluxReturn(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogFileDataFn(2697).Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();
            }
        }

        public static void SetDeliveriesInvoice(int[] deliveryIds, int costId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ProductCatalogDelivery> deliveries = ctx.ProductCatalogDelivery.Where(x => deliveryIds.Contains(x.DeliveryId)).ToList();

                foreach (Dal.ProductCatalogDelivery delivery in deliveries)
                    delivery.CostId = costId;

                ctx.SubmitChanges();
            }
        }

 

        public static void SetDeliveryUpdate(ProductCatalogDelivery delivery)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogDelivery deliveryToUpdate = ctx.ProductCatalogDelivery.Where(x => x.DeliveryId==delivery.DeliveryId).FirstOrDefault();

                deliveryToUpdate.Quantity = delivery.Quantity;
                deliveryToUpdate.Price = delivery.Price;

                ctx.SubmitChanges();
            }
        }



        public static bool GetProductCatalogAttributeCanDelete(int attributeId, out List<string> msg)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                msg = new List<string>();

                int c = ctx.ProductCatalogAttributeToProduct.Where(x => x.AttributeId == attributeId).Count();
                if (c > 0)
                    msg.Add(String.Format("[KRYTYCZNY] Atrybut jest przypisany do {0} produktów ", c));


                if (ctx.ShopCategoryManagerAttribute.Where(x => x.AttributeId == attributeId).Count() > 0)
                    msg.Add("[NIEKRYTYCZNY] Atrybut tworzy konfigurację kategorii w sklepie");
                if (ctx.ProductCatalogAttributeAllegroExternalSource.Where(x => x.AttributeId == attributeId).Count() > 0)
                    msg.Add("[NIEKRYTYCZNY] Atrybut tworzy konfigurację na Allegro");


                if (ctx.ProductCatalogMixerAttributeGroup.Where(x => x.AttributeId == attributeId).Count() > 0)
                    msg.Add("[NIEKRYTYCZNY] Atrybut posiada wersję miksera treści");

                if (ctx.ProductCatalogAttributeGroupingAttribute.Where(x => x.AttributeId == attributeId).Count() > 0)
                    msg.Add("[NIEKRYTYCZNY] Atrybut bierze udział w grupowaniu atrybutów");
                if (ctx.SearchTableAttributes.Where(x => x.AttributeId == attributeId).Count() > 0)
                    msg.Add("[NIEKRYTYCZNY] Atrybut bierze udział w zapisanych szablonach wyszukiwań w kat. prod.");


                return msg.Count() == 0;
            }
        }

        public static List<Dal.ProductCatalog> GetProductCatalogByAttribute(int attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeToProduct.Where(x => x.AttributeId == attributeId && x.IsDefault.HasValue && x.IsDefault.Value)
                    .Select(x => x.ProductCatalog)
                    .Distinct()
                    .ToList();
            }
        }

        public static bool SetProductCatalogAttributeDelete(int attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.Connection.Open();
                using (System.Data.Common.DbTransaction trans = ctx.Connection.BeginTransaction())
                {
                    try
                    {
                        ctx.Transaction = trans;

                        List<Dal.SearchTableAttributes> sta = ctx.SearchTableAttributes.Where(x => x.AttributeId == attributeId).ToList();
                        List<Dal.ProductCatalogAttributeToProduct> atp = ctx.ProductCatalogAttributeToProduct.Where(x => x.AttributeId == attributeId).ToList();
                        List<Dal.ShopCategoryManagerAttribute> cm = ctx.ShopCategoryManagerAttribute.Where(x => x.AttributeId == attributeId).ToList();
                        List<Dal.ProductCatalogMixerAttributeGroup> mag = ctx.ProductCatalogMixerAttributeGroup.Where(x => x.AttributeId == attributeId).ToList();
                        List<Dal.ProductCatalogAttributeGroupingAttribute> aga = ctx.ProductCatalogAttributeGroupingAttribute.Where(x => x.AttributeId == attributeId).ToList();
                        List<Dal.ProductCatalogAttribute> a = ctx.ProductCatalogAttribute.Where(x => x.AttributeId == attributeId).ToList();
                        List<Dal.ProductCatalogAttributeAllegroExternalSource> ex = ctx.ProductCatalogAttributeAllegroExternalSource.Where(x => x.AttributeId == attributeId).ToList();

                        ctx.ProductCatalogAttributeAllegroExternalSource.DeleteAllOnSubmit(ex);
                        ctx.SearchTableAttributes.DeleteAllOnSubmit(sta);
                        ctx.ShopCategoryManagerAttribute.DeleteAllOnSubmit(cm);
                        ctx.ProductCatalogMixerAttributeGroup.DeleteAllOnSubmit(mag);
                        ctx.ProductCatalogAttributeGroupingAttribute.DeleteAllOnSubmit(aga);
                        ctx.ProductCatalogAttributeToProduct.DeleteAllOnSubmit(atp);
                        ctx.ProductCatalogAttribute.DeleteAllOnSubmit(a);
                         
                        ctx.SubmitChanges();
                        ctx.Transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction
                        if (trans != null)
                            trans.Rollback();
                        return false;
                    }
                }

            }
        }

        public static void SetProductCatalogReady(int productCatalogId, bool isReady, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalog pc = ctx.ProductCatalog.Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();

                pc.IsReady = isReady;
                pc.UpdateUser = userName;

                ctx.SubmitChanges();
            }
        }

        public static void SetProductCatalogAttributeGroupsUpdated(int[] attributeGroupsId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ProductCatalogAttributeGroup> groups = ctx.ProductCatalogAttributeGroup.Where(x => attributeGroupsId.Contains(x.AttributeGroupId)).ToList();
                foreach (Dal.ProductCatalogAttributeGroup group in groups)
                    group.UpdateShopConfiguration = false;

                ctx.SubmitChanges();
            }
        }


        public static void SetProductCatalogAttributesUpdated(int[] attributeIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ProductCatalogAttribute> attributes = ctx.ProductCatalogAttribute.Where(x => attributeIds.Contains(x.AttributeId)).ToList();
                foreach (Dal.ProductCatalogAttribute attribute in attributes)
                    attribute.UpdateShopConfiguration = false;

                ctx.SubmitChanges();
            }
        }


        //public static int[] GetProductCatalogDeliveries()
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogDelivery.Where(x => x.InsertDate > DateTime.Parse("2020-10-11")).Select(x => x.ProductCatalogId).Distinct().ToArray();
        //    }
        //}

        public static List<ProductCatalogShopProduct> GetProductCatalogShopProductsWithoutPromotions(Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopProduct
                    .Where(x => x.ShopId == (int)shop && x.ShopProductId !=null && x.IsPSActive && x.ProductCatalog.IsActivePricePromo==false)
                    .ToList();

            }
        }

        public static List<ProductCatalogMissingDescriptions> GetProductsWithoutDescription()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogMissingDescriptions.ToList();
            }
        }
    }
}
