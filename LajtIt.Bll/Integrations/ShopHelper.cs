using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Bll.Integrations
{
    public class ShopHelper
    {
        public static void SetShopConfiguration(int? shopAttributeId)
        { 

            List<Dal.Shop> shops = Dal.DbHelper.Shop.GetShops(Dal.Helper.ShopEngineType.Shoper).Where(x => x.IsActive).ToList();


            foreach (Dal.Shop shop in shops)
            {
                Dal.Helper.Shop s = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shop.ShopId);
                Bll.ShopRestHelper.Producers.SetProducers(s);
            }


            if (shopAttributeId.HasValue == false)
                return;

            Dal.ProductCatalogShopAttribute shopAttribute = Dal.DbHelper.ProductCatalog.GetProductCatalogShopAttribute(Dal.Helper.Shop.Lajtitpl, shopAttributeId.Value);

            Bll.ShopRestHelper.AttributeGroups.UpdateAttribute(Dal.Helper.Shop.Lajtitpl, shopAttribute);


            //List<Dal.ProductCatalogAttributeGroup> attributeGroups = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributeGroupsForShopUpdate();

            //if (attributeGroups.Count > 0)
            //{
            //    Dal.ShopHelper sh = new Dal.ShopHelper();
            //    List<Dal.Shop> shopsForAttributes = sh.GetShopsForAttributeChanges();

            //    foreach (Dal.Shop shop in shopsForAttributes)
            //    {
            //        Dal.Helper.Shop s = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shop.ShopId);

            //        Dal.ProductCatalogShopAttribute shopAttribute = null;

            //        foreach (Dal.ProductCatalogAttributeGroup attribute in attributeGroups)
            //        {
            //            switch (attribute.AttributeGroupTypeId)
            //            {
            //                case 1:
            //                case 2:
            //                    shopAttribute = Dal.DbHelper.ProductCatalog.GetProductCatalogShopAttributeByAttributeGroupId(s, attribute.AttributeGroupId);
            //                    Bll.ShopRestHelper.AttributeGroups.UpdateAttribute(s, shopAttribute);
            //                    break;
            //                case 3:
            //                    //shopAttribute = Dal.DbHelper.ProductCatalog.GetProductCatalogShopAttributeByAttributeGroupId(s, attribute.AttributeGroupId, attribute.AttributeId);
            //                    break;
            //            }
            //        }

            //        //Bll.ShopRestHelper.AttributeGroups.SetAttibuteGroup(s);

            //    }

            //    Dal.DbHelper.ProductCatalog.SetProductCatalogAttributeGroupsUpdated(attributeGroups.Select(x=>x.AttributeGroupId).ToArray());
            //}


            //List<Dal.ProductCatalogAttribute> attributes = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributesForShopUpdate();

            //if(attributes.Count>0)
            //{
            //    Dal.ShopHelper sh = new Dal.ShopHelper();
            //    List<Dal.Shop> shopsForAttributes = sh.GetShopsForAttributeChanges();

            //    foreach(Dal.Shop shop in shopsForAttributes)
            //    {
            //        Dal.Helper.Shop s = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shop.ShopId);

            //        Dal.ProductCatalogShopAttribute shopAttribute = null;

            //        foreach (Dal.ProductCatalogAttribute attribute in attributes)
            //        {
            //            switch (attribute.ProductCatalogAttributeGroup.AttributeGroupTypeId)
            //            {
            //                case 1:
            //                case 2:
            //                    shopAttribute = Dal.DbHelper.ProductCatalog.GetProductCatalogShopAttributeByAttributeGroupId(s, attribute.AttributeGroupId);
            //                    Bll.ShopRestHelper.AttributeGroups.UpdateAttribute(s, shopAttribute);
            //                    break;
            //                case 3:
            //                    shopAttribute = Dal.DbHelper.ProductCatalog.GetProductCatalogShopAttributeByAttributeGroupId(s, attribute.AttributeGroupId, attribute.AttributeId);

            //                    if(shopAttribute == null)
            //                    {
            //                        //
            //                    }
            //                    else
            //                    {

            //                        Bll.ShopRestHelper.AttributeGroups.UpdateAttribute(s, shopAttribute);
            //                    }


            //                    break;
            //            }
            //        }

            //    }
            //    Dal.DbHelper.ProductCatalog.SetProductCatalogAttributesUpdated(attributes.Select(x => x.AttributeId).ToArray());


            //}

        }
    }
}
