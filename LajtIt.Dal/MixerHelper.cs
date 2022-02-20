using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class MixerHelper
    {
        public ProductCatalogMixerAttributeGroup GetAttributeGroupMixer(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogMixerAttributeGroup.Where(x => x.Id == id).FirstOrDefault();

            }
        }

        public void SetAttributeGroupMixer(int id, ProductCatalogMixerAttributeGroup mag)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogMixerAttributeGroup magToUpdate = ctx.ProductCatalogMixerAttributeGroup.Where(x => x.Id == id).FirstOrDefault();

                magToUpdate.IsActive = mag.IsActive;
                magToUpdate.TemplateF = mag.TemplateF;
                magToUpdate.TemplateM = mag.TemplateM;
                magToUpdate.TemplateN = mag.TemplateN;
                magToUpdate.AttributeId = mag.AttributeId;
                magToUpdate.AttributeGroupingId = mag.AttributeGroupingId;
                magToUpdate.ShopTypeId = mag.ShopTypeId;

                ctx.SubmitChanges();

            }
        }

        public void SetAttributeGroupMixer(ProductCatalogMixerAttributeGroup mag)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogMixerAttributeGroup.InsertOnSubmit(mag);
                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogMixerAttributeGroup> GetAttributeGroupMixers(int attributeGroupId, int shopTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogMixerAttributeGroup>(x => x.ProductCatalogAttribute);
                dlo.LoadWith<ProductCatalogMixerAttributeGroup>(x => x.ProductCatalogAttributeGrouping);
                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogMixerAttributeGroup.Where(x => x.AttributeGroupId == attributeGroupId && x.ShopTypeId == shopTypeId).OrderBy(x => x.ProductCatalogAttribute.Name).ToList();
            }
        }

        public List<ProductCatalogMixerAttributeGroupRandomFnResult> GetProductCatalogMixerAttributeGroupRandom(int productCatalogId, Dal.Helper.Shop shop )
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogMixerAttributeGroupRandomFn(productCatalogId, (int)shop)
                    .OrderBy(x=>x.Order)
                    .ToList();
            }
        }

        public void SetAttribute(ProductCatalogAttribute attribute)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogAttribute attToUpdate = ctx.ProductCatalogAttribute.Where(x => x.AttributeId == attribute.AttributeId).FirstOrDefault();

                attToUpdate.FriendlyDescriptionF = attribute.FriendlyDescriptionF;
                attToUpdate.FriendlyDescriptionM = attribute.FriendlyDescriptionM;
                attToUpdate.FriendlyDescriptionN = attribute.FriendlyDescriptionN;

                ctx.SubmitChanges();

            }
        }
    }
}
