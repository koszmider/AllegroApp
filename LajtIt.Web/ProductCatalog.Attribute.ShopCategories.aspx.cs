using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;
using LajtIt.Web.Controls;

namespace LajtIt.Web
{
    [Developer("5FC87F15-7A32-4ADF-A0C6-F4A04178DA0F")]
    public partial class ProductAttributeShopCategories : LajtitPage
    {
       
        public int AttributeId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindAttribute();
        }

        private void BindAttribute()
        {
            Dal.ProductCatalogAttribute attribute = Dal.DbHelper.ProductCatalog.GetProductCatalogAttribute(AttributeId);
            if (attribute.AttributeGroupId != 6)
            
                {
                ucAttributeMenu.NotAvailable();
                pnContent.Visible = false;
                return;
            }

             
                ddlShopType.DataSource = Dal.DbHelper.Shop.GetShopTypes();
                ddlShopType.DataBind();
            }
 
        protected void btnAttributeSave_Click(object sender, EventArgs e)
        {

            Dal.ProductCatalogAttributeCategoryShop cat = GetShopCategory(AttributeId);

            Dal.DbHelper.ProductCatalog.SetProductCatalogAttributeCategoryShop(cat);

            DisplayMessage("Zapisano zmiany");
        }
        private ProductCatalogAttributeCategoryShop GetShopCategory(int attributeId)
        {
            string shopCategoryId = ucShopCategoryControl.GetShopCategoryId();
            Dal.Helper.ShopType? ShopType = ucShopCategoryControl.GetShopType();

            if (!ShopType.HasValue || shopCategoryId == null)
                return null;

            Dal.ShopHelper sh = new ShopHelper();

            var cat = sh.GetShopCategory(ShopType.Value, shopCategoryId);

            if (cat != null)
            {
                Dal.ProductCatalogAttributeCategoryShop acs = new Dal.ProductCatalogAttributeCategoryShop()
                {
                    AttributeId = attributeId,
                    CategoryId = cat.CategoryId,
                    ShopTypeId = (int)ShopType.Value
                };
                return acs;
            }
            else
                return null;


        }
        protected void ddlShopType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dal.Helper.ShopType st = (Dal.Helper.ShopType)Convert.ToInt32(ddlShopType.SelectedValue);
            ucShopCategoryControl.SetCategoryId(st, AttributeId);
        }
    }
}