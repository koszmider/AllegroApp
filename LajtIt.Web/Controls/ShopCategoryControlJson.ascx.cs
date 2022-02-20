using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;

namespace LajtIt.Web.Controls
{
    public partial class ShopCategoryControlJson : System.Web.UI.UserControl
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!Page.IsPostBack)
        //        BindForm();
        //}


        public void BindForm()
        {
            ScriptManager.RegisterStartupScript(upCategory, upCategory.GetType(), "k",  "categories();" 
                , true);
        }

        internal void SetCategoryId(Helper.ShopType shopType, Dal.Supplier supplier)
        {
            string shopParentCategoryId = "0";
            string shopCategoryId = "0";
            throw new NotImplementedException("TO DO");


            //if (supplier.ShopCategoryId.HasValue)
            //{
            //    Dal.ShopHelper sh = new Dal.ShopHelper();
            //    Dal.ShopCategory sc = sh.GetCategory(shopType, supplier.ShopCategoryId.Value);
            //    if (sc != null)
            //    {
            //        shopParentCategoryId = (sc.CategoryParentId == null) ? "0" : sc.CategoryParentId;
            //        shopCategoryId = sc.ShopCategoryId.ToString();
            //    }
            //}
            hidShopTypeId.Value = ((int)shopType).ToString();
            hidSelected.Value = shopCategoryId.ToString();
            hidStartParent.Value = shopParentCategoryId.ToString();
        }
        public void SetCategoryId(Dal.Helper.ShopType shopType,  int attributeId)
        {
            string shopParentCategoryId = "0";
            string shopCategoryId = "0";

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ProductCatalogAttributeCategoryShop acs = pch.GetProductCatalogAttributeCategory(shopType, attributeId);


            if (acs!=null)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                Dal.ShopCategory sc = sh.GetCategory(shopType, acs.CategoryId);
                if (sc != null)
                { 
                    shopParentCategoryId = (sc.CategoryParentId == null)? "0": sc.CategoryParentId;
                    shopCategoryId = sc.ShopCategoryId.ToString();
                }
            } 

            hidShopTypeId.Value = ((int)shopType).ToString();
            hidSelected.Value = shopCategoryId.ToString();
            hidStartParent.Value= shopParentCategoryId.ToString();
        }
        public void SetCategoryId(Dal.Helper.Shop shop, int supplierId)
        {
            BindForm();
            string shopParentCategoryId = "0";
            string shopCategoryId = "0";

            Dal.ShopHelper sh = new ShopHelper();
            Dal.SupplierShop ss;
            if (supplierId == 0)
                ss = sh.GetSupplierShop((int)shop).FirstOrDefault();

            else
                ss = sh.GetSupplierShop((int)shop).Where(x => x.SupplierId == supplierId).FirstOrDefault();

            Dal.Helper.ShopType shopType = (Dal.Helper.ShopType)Enum.ToObject(typeof(Dal.Helper.ShopType), ss.Shop.ShopTypeId);

            if (ss.CategoryId.HasValue)
            { 
                Dal.ShopCategory sc = sh.GetCategory(shopType, ss.CategoryId.Value);
                if (sc != null)
                {
                    shopParentCategoryId = (sc.CategoryParentId == null) ? "0" : sc.CategoryParentId;
                    shopCategoryId = sc.ShopCategoryId.ToString();
                }
            }


            hidShopTypeId.Value = ((int)shopType).ToString();
            hidSelected.Value = shopCategoryId.ToString();
            hidStartParent.Value = shopParentCategoryId.ToString();
        }

        public string GetShopCategoryId()
        {
            if (hidSelected.Value == "0")
                return null;
            return hidSelected.Value;
        }
        public int? GetCategoryId()
        {
            if (hidSelected.Value == "0")
                return null;
            else
            {
                Dal.ShopHelper sh = new ShopHelper();

                var cat = sh.GetShopCategory((Dal.Helper.ShopType) Int32.Parse(hidShopTypeId.Value), GetShopCategoryId());

                if (cat != null)
                    return cat.CategoryId;
                else
                    return null;
            }

        }
        public Dal.Helper.ShopType? GetShopType()
        {
            if (Int32.Parse(hidShopTypeId.Value) == 0)
                return null;
            return (Dal.Helper.ShopType) Int32.Parse(hidShopTypeId.Value);
        }

    }
}