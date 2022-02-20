using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
    partial class ProductCatalog
    {
        public static bool SetProductCatalogAttribute(ProductCatalogAttribute attribute, ref int attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogAttribute att = ctx.ProductCatalogAttribute
                    .Where(x => x.AttributeGroupId == attribute.AttributeGroupId && x.Name.ToLower() == attribute.Name.ToLower())
                    .FirstOrDefault();


                if (att != null)
                {
                    attributeId = att.AttributeId;
                    return false;
                }
                ctx.ProductCatalogAttribute.InsertOnSubmit(attribute);
                ctx.SubmitChanges();
                attributeId = attribute.AttributeId;

                return true;
            }
        }
        public static void SetProductCatalogAttributeCategoryShop(ProductCatalogAttributeCategoryShop cat)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                Dal.ProductCatalogAttributeCategoryShop catToUpdate = ctx.ProductCatalogAttributeCategoryShop
                    .Where(x => x.AttributeId == cat.AttributeId && x.ShopTypeId == cat.ShopTypeId)
                    .FirstOrDefault();

                if (catToUpdate != null)
                    catToUpdate.CategoryId = cat.CategoryId;
                else
                    ctx.ProductCatalogAttributeCategoryShop.InsertOnSubmit(cat);

                ctx.SubmitChanges();
            }
        }

        public static void SetProductCatalogAttributeAllegroExternalSource(int attributeId, string shopCategoryId, string allegroParameterId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogAttributeAllegroExternalSource e = ctx.ProductCatalogAttributeAllegroExternalSource
                    .Where(x => x.AttributeId == attributeId && x.AllegroCategoryId.ToString() == shopCategoryId)
                    .FirstOrDefault();

                if (e == null)
                    ctx.ProductCatalogAttributeAllegroExternalSource.InsertOnSubmit(new ProductCatalogAttributeAllegroExternalSource()
                    {
                        AllegroCategoryId = Int32.Parse(shopCategoryId),
                        AllegroParameterId = allegroParameterId,
                        AttributeId = attributeId
                    });
                else
                    e.AllegroParameterId = allegroParameterId;

                ctx.SubmitChanges();
            }
        }
        public static void SetProductCatalogAttributeShopTemplates(int attributeId, List<ProductCatalogShopAttributeTemplate> templates)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] shopIds = templates.Select(x => x.ShopId).ToArray();

                foreach (int shopId in shopIds)
                {
                    Dal.ProductCatalogShopAttributeTemplate template = templates
                        .Where(x => x.ShopId == shopId && x.AttributeId == attributeId)
                        .FirstOrDefault();

                    Dal.ProductCatalogShopAttributeTemplate templToUpdate = ctx.ProductCatalogShopAttributeTemplate
                        .Where(x => x.ShopId == shopId && x.AttributeId == attributeId)
                        .FirstOrDefault();

                    if (templToUpdate == null)
                    {
                        if (!String.IsNullOrEmpty(template.Template))
                        {
                            ctx.ProductCatalogShopAttributeTemplate.InsertOnSubmit(template);
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(template.Template))
                        {
                            templToUpdate.Template = template.Template;
                            templToUpdate.UpdateDate = template.UpdateDate;
                            templToUpdate.UpdateUser = template.UpdateUser;
                        }
                        else
                        {
                            ctx.ProductCatalogShopAttributeTemplate.DeleteOnSubmit(templToUpdate);
                        }
                    }

                }
                ctx.SubmitChanges();
            }
        }

        public static ProductCatalogAttributeAllegroExternalSource GetProductCatalogAttributeAllegroExternalSource(int attributeId, string shopCategoryId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeAllegroExternalSource
                    .Where(x => x.AttributeId == attributeId && x.AllegroCategoryId.ToString() == shopCategoryId)
                    .FirstOrDefault();
            }
        }
        public static List<ProductCatalogShopAttributeTemplateFnResult> GetProductCatalogShopAttributeTemplate(int attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopAttributeTemplateFn(attributeId).ToList();
            }
        }
        public static int SetProductCatalogAttributeUpdate(ProductCatalogAttribute att)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                try
                {

                Dal.ProductCatalogAttribute attributeToUpdate = ctx.ProductCatalogAttribute.Where(x => x.AttributeId == att.AttributeId).FirstOrDefault();

                attributeToUpdate.AttributeGroupId = att.AttributeGroupId;
                attributeToUpdate.Name = att.Name;
                attributeToUpdate.Code = att.Code;
                attributeToUpdate.AttributeTypeId = att.AttributeTypeId;
                attributeToUpdate.SexTypeId = att.SexTypeId;
                attributeToUpdate.FriendlyNameM = att.FriendlyNameM;
                attributeToUpdate.FriendlyNameF = att.FriendlyNameF;
                attributeToUpdate.FriendlyNameN = att.FriendlyNameN;
                attributeToUpdate.SortOrder = att.SortOrder;
                attributeToUpdate.FriendlyDescriptionM = att.FriendlyDescriptionM;
                attributeToUpdate.FriendlyDescriptionF = att.FriendlyDescriptionF;
                attributeToUpdate.FriendlyDescriptionN = att.FriendlyDescriptionN;
                attributeToUpdate.FieldTemplate = att.FieldTemplate;

                ctx.SubmitChanges();
                    return 1;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2601 || ex.Number == 2627)
                    {
                        return -1;
                    }
                    return 0;
                }


            }
        }

        public static ProductCatalogAttribute GetProductCatalogAttribute(int attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAttribute>(x => x.ProductCatalogAttributeGroup);

                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogAttribute.Where(x => x.AttributeId == attributeId).FirstOrDefault();
            }
        }
        public static List<ProductCatalogAttribute> GetProductCatalogAttributesForShopUpdate()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAttribute>(x => x.ProductCatalogAttributeGroup);

                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogAttribute.Where(x => x.UpdateShopConfiguration).ToList();
            }
        }
        public static List<ProductCatalogAttributeGroup> GetProductCatalogAttributeGroupsForShopUpdate()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
           
                return ctx.ProductCatalogAttributeGroup.Where(x => x.UpdateShopConfiguration).ToList();
            }
        }
        public static void SetProductCatalogAttributesForShopUpdate(int attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogAttribute attribute = ctx.ProductCatalogAttribute.Where(x => x.AttributeId == attributeId).FirstOrDefault();

                attribute.UpdateShopConfiguration = false;
                ctx.SubmitChanges();
            }
        }
        public static ProductCatalogShopAttribute GetProductCatalogShopAttribute(Dal.Helper.Shop shop, int shopAttributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ShopAttribute);
                dlo.LoadWith<ShopAttribute>(x => x.ShopAttributeGroup);
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ProductCatalogAttributeGroup);

                ctx.LoadOptions = dlo;

                Dal.ProductCatalogShopAttribute psa = ctx.ProductCatalogShopAttribute
                    .Where(x => x.ShopAttribute.ShopAttributeGroup.ShopId == (int)shop && x.ShopAttributeId == shopAttributeId)
                    .FirstOrDefault();

                return psa;

            }
        }
        public static ProductCatalogShopAttribute GetProductCatalogShopAttributeByAttributeGroupId(Helper.Shop shop, int attributeGroupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ShopAttribute);
                dlo.LoadWith<ShopAttribute>(x => x.ShopAttributeGroup); 
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ProductCatalogAttributeGroup);

                ctx.LoadOptions = dlo;

                Dal.ProductCatalogShopAttribute psa = ctx.ProductCatalogShopAttribute
                    .Where(x => x.ShopAttribute.ShopAttributeGroup.ShopId == (int)shop && x.AttributeGroupId == attributeGroupId)
                    .FirstOrDefault();

                return psa;

            }
        }
        public static ProductCatalogShopAttribute GetProductCatalogShopAttributeByAttributeGroupId(Helper.Shop shop, int attributeGroupId, int attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ShopAttribute);
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ProductCatalogAttributeGroup);
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ProductCatalogAttribute);
                dlo.LoadWith<ShopAttribute>(x => x.ShopAttributeGroup);

                ctx.LoadOptions = dlo;

                Dal.ProductCatalogShopAttribute psa = ctx.ProductCatalogShopAttribute
                    .Where(x => x.ShopAttribute.ShopAttributeGroup.ShopId == (int)shop && x.AttributeGroupId == attributeGroupId && x.AttributeId == attributeId)
                    .FirstOrDefault();

                return psa;

            }
        }
        public static List<ProductCatalogAttributeGroup> GetProductCatalogAttributeGroupsForShop(Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] assignedAttributeGroupIds = ctx.ProductCatalogShopAttribute
                    .Where(x => x.ShopAttribute.ShopAttributeGroup.ShopId == (int)shop && x.AttributeGroupId.HasValue)
                    .Select(x => x.AttributeGroupId.Value)
                    .ToArray();

                return ctx.ProductCatalogAttributeGroup.Where(x => !assignedAttributeGroupIds.Contains(x.AttributeGroupId)).ToList();
            }
        }
        public static ProductCatalogShopAttribute GetProductCatalogShopAttributeByExternalAttributeGroupId(Helper.Shop shop, int externalAttributeGroupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ShopAttribute);
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ProductCatalogAttributeGroup);
                dlo.LoadWith<ShopAttribute>(x => x.ShopAttributeGroup);

                ctx.LoadOptions = dlo;

                Dal.ProductCatalogShopAttribute psa = ctx.ProductCatalogShopAttribute
                    .Where(x => x.ShopAttribute.ShopAttributeGroup.ShopId == (int)shop && x.ShopAttribute.ShopAttributeGroup.ExternalShopAttributeGroupId == externalAttributeGroupId)
                    .FirstOrDefault();

                return psa;

            }
        }
        public static List<ProductCatalogAttribute> GetProductCatalogAttributes(int attributeGroupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAttribute>(x => x.ProductCatalogAttributeGroup);

                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogAttribute
                    .Where(x => x.AttributeGroupId == attributeGroupId)
                    .ToList();
            }
        }
        public static List<ProductCatalogShopAttribute> GetProductCatalogShopAttributes(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ProductCatalogAttributeGroup);
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ShopAttribute);
                dlo.LoadWith<ShopAttribute>(x => x.ShopAttributeGroup);

                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogShopAttribute
                    .Where(x => x.ShopAttribute.ShopAttributeGroup.ShopId == shopId)
                    .ToList();
            }
        }
    }
}
