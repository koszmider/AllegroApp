using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class PromotionHelper
    {

        public void SetProductCatalogs(int promotionId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<int> productCatalogIds = GetProductCatalogs(promotionId).ToList();

                var productsNotInList = ctx.PromotionProductsSelected.Where(x => !productCatalogIds.Contains(x.ProductCatalogId)).ToList();

                foreach (var p in productsNotInList)
                    p.IsActive = false;

                int[] productIdsNotInList = productsNotInList.Select(x => x.ProductCatalogId).ToArray();
                int[] productIdsToAdd = productCatalogIds.Except(productIdsNotInList).ToArray().Except(ctx.PromotionProductsSelected.Select(x => x.ProductCatalogId).ToArray()).ToArray();

                ctx.PromotionProductsSelected.InsertAllOnSubmit(
                    productIdsToAdd.Select(x => new PromotionProductsSelected()
                    {
                        IsActive = true,
                        ProductCatalogId = x,
                        PromotionId = promotionId
                    }
                    ));

                ctx.SubmitChanges();
            }
        }

        public List<PromotionProductsSelected> GetProductCatalogsSelected(int promotionId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.PromotionProductsSelected.Where(x => x.PromotionId == promotionId).ToList();
            }
        }
        public int[] GetProductCatalogs(int promotionId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] supplierIds = GetPromotionSuppliers(promotionId).Select(x => x.SupplierId).ToArray();
                int[] shopIds = GetPromotionShops(promotionId).Select(x => x.ShopId).ToArray();

                int[] shopSupplierIds = ctx.SupplierShop.Where(x => shopIds.Contains(x.ShopId)).Select(x => x.SupplierId).ToArray();
                var products = ctx.ProductCatalog.Where(x => supplierIds.Contains(x.SupplierId) && shopSupplierIds.Contains(x.SupplierId));

                List<PromotionCondition> conditions = GetPromotionConditions(promotionId).Where(x => x.IsActive).ToList();

                foreach (Dal.PromotionCondition condition in conditions)
                {
                    switch (condition.ConditionTypeId)
                    {

                        case 1:
                            products = products.Where(x => x.PriceBruttoFixed >= condition.DecimalValue.Value); break;
                        case 2:
                            products = products.Where(x => x.PriceBruttoFixed <= condition.DecimalValue.Value); break;
                        case 3:
                            if (condition.BitValue.HasValue)
                                products = products.Where(x => x.IsActivePricePromo == condition.BitValue.Value); break;
                    }
                }


                List<ProductCatalogAttribute> attributes = GetPromotionAttributes(promotionId);

                int[] groupIds = attributes.Select(x => x.AttributeGroupId).Distinct().ToArray();

                int[] productCatalogIds = products.Select(x => x.ProductCatalogId).ToArray();


                Dal.ProductCatalogHelper pch = new ProductCatalogHelper();

                List<int> productIds = new List<int>();
                foreach (int productCatalogId in productCatalogIds)
                {
                    var pa = pch.GetProductCatalogAttributesForProduct(productCatalogId);
                    bool exists = true;

                    foreach (int groupId in groupIds) // dla każdej grupy musi być choć jeden atrybut z wybranych w promocji
                    {
                        int[] attributeIdsSelectedForGroup = attributes.Where(x => x.AttributeGroupId == groupId).Select(x => x.AttributeId).ToArray(); // pobieram id
                        bool result = pa.Where(x => x.AttributeGroupId == groupId && attributeIdsSelectedForGroup.Contains(x.AttributeId)).Count() > 0; // sprawdzam czy przynajmniej jeden jest przypisany do produktu

                        if (!result)
                        {
                            exists = false;
                            break;
                        }
                    }
                    if (exists)
                        productIds.Add(productCatalogId);
                }

                return productIds.ToArray();
            }
        }

        public int SetPromotion(Promotion promotion)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.Promotion.InsertOnSubmit(promotion);
                ctx.SubmitChanges();
                return promotion.PromotionId;
            }
        }

        public Promotion GetPromotion(int promotionId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Promotion.Where(x => x.PromotionId == promotionId).FirstOrDefault();
            }
        }

        public List<Supplier> GetPromotionSuppliers(int promotionId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<PromotionSupplier>(x => x.Supplier);

                ctx.LoadOptions = dlo;
                return ctx.PromotionSupplier.Where(x => x.PromotionId == promotionId).Select(x => x.Supplier).ToList();
            }
        }

        public List<PromotionCondition> GetPromotionConditions(int promotionId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.PromotionCondition.Where(x => x.PromotionId == promotionId).ToList();
            }
        }

        public List<Promotion> GetPromotions()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Promotion.OrderByDescending(x => x.StartDate).ToList();
            }
        }

        public List<Shop> GetPromotionShops(int promotionId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<PromotionShop>(x => x.Shop);

                ctx.LoadOptions = dlo;
                return ctx.PromotionShop.Where(x => x.PromotionId == promotionId).Select(x => x.Shop).ToList();
            }
        }

        public List<ProductCatalogAttribute> GetPromotionAttributes(int promotionId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<PromotionAttribute>(x => x.ProductCatalogAttribute);
                dlo.LoadWith<ProductCatalogAttribute>(x => x.ProductCatalogAttributeGroup);

                ctx.LoadOptions = dlo;
                return ctx.PromotionAttribute.Where(x => x.PromotionId == promotionId).Select(x => x.ProductCatalogAttribute).ToList();
            }
        }

        public void SetPromotion(Promotion promotion, int[] suppliers, int[] shops, int[] attributes, List<Dal.PromotionCondition> conditions)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Promotion promotionToUpdate = ctx.Promotion.Where(x => x.PromotionId == promotion.PromotionId).FirstOrDefault();

                promotionToUpdate.EndDate = promotion.EndDate;
                promotionToUpdate.IsActive = promotion.IsActive;
                promotionToUpdate.IsWatekmarkActive = promotion.IsWatekmarkActive;
                promotionToUpdate.Name = promotion.Name;
                promotionToUpdate.StartDate = promotion.StartDate;
                promotionToUpdate.Description = promotion.Description;

                var sup = ctx.PromotionSupplier.Where(x => x.PromotionId == promotion.PromotionId).ToList();
                var sh = ctx.PromotionShop.Where(x => x.PromotionId == promotion.PromotionId).ToList();
                var att = ctx.PromotionAttribute.Where(x => x.PromotionId == promotion.PromotionId).ToList();

                ctx.PromotionSupplier.DeleteAllOnSubmit(sup);
                ctx.PromotionShop.DeleteAllOnSubmit(sh);
                ctx.PromotionAttribute.DeleteAllOnSubmit(att);


                List<Dal.PromotionCondition> conditionsToUpdate = ctx.PromotionCondition.Where(x => x.PromotionId == promotion.PromotionId).ToList();

                foreach (Dal.PromotionCondition conditionToUpdate in conditionsToUpdate)
                {
                    Dal.PromotionCondition condition = conditions.Where(x => x.ConditionTypeId == conditionToUpdate.ConditionTypeId).FirstOrDefault();

                    conditionToUpdate.BitValue = condition.BitValue;
                    conditionToUpdate.DecimalValue = condition.DecimalValue;
                    conditionToUpdate.IsActive = condition.IsActive;
                    conditionToUpdate.Name = condition.Name;
                }

                int[] existingConditionTypeIds = conditionsToUpdate.Select(x => x.ConditionTypeId).ToArray();
                List<Dal.PromotionCondition> conditionsToInsert = conditions.Where(x => !existingConditionTypeIds.Contains(x.ConditionTypeId)).ToList();

                ctx.PromotionCondition.InsertAllOnSubmit(conditionsToInsert);

                ctx.SubmitChanges();

                ctx.PromotionSupplier.InsertAllOnSubmit(suppliers.Select(x => new PromotionSupplier() { PromotionId = promotion.PromotionId, SupplierId = x }));
                ctx.PromotionShop.InsertAllOnSubmit(shops.Select(x => new PromotionShop() { PromotionId = promotion.PromotionId, ShopId = x }));
                ctx.PromotionAttribute.InsertAllOnSubmit(attributes.Select(x => new PromotionAttribute() { PromotionId = promotion.PromotionId, AttributeId = x }));

                ctx.SubmitChanges();
            }
        }

        public List<Dal.PromotionProductView> GetPromotionProducts(int promotionId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.PromotionProductView.Where(x => x.PromotionId == promotionId).ToList();
            }
        }

        public void SetPromotionProduct(int promotionId, int productCatalogId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.PromotionProduct pp = new PromotionProduct()
                {
                    ProductCatalogId = productCatalogId,
                    PromotionId = promotionId,
                    Quantity = 1
                };

                ctx.PromotionProduct.InsertOnSubmit(pp);
                ctx.SubmitChanges();

            }
        }

        public void SetPromotionProductDelete(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.PromotionProduct pp = ctx.PromotionProduct.Where(x => x.Id == id).FirstOrDefault();

                ctx.PromotionProduct.DeleteOnSubmit(pp);
                ctx.SubmitChanges();

            }
        }

    }
}
