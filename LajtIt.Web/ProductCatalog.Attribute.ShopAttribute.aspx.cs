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
    [Developer("8ae4cbc2-b5b6-44aa-864e-0d7851aaf51f")]
    public partial class ProductAttributeShopAttribute : LajtitPage
    {

        public int AttributeId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShops();

                BindShopAttributes();
            }
        }

        private void BindShops()
        {
            ddlShop.DataSource = Dal.DbHelper.Shop.GetShops(Dal.Helper.ShopEngineType.Shoper);
            ddlShop.DataBind();
        }

        protected void ddlShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlShop.Items.Clear();
            ddlShop.Items.Insert(0, new ListItem("", "0"));
            BindShopAttributes();
        }

        private void BindShopAttributes()
        {
            List<Dal.ProductCatalogAttributeShopGroupingType> shopGroupingType = Dal.DbHelper.Shop.GetProductCatalogAttributeShopGroupingTypes(Int32.Parse(ddlShop.SelectedValue));

            ddlProductCatalogAttributeShopGroupingType.DataSource = shopGroupingType;
            ddlProductCatalogAttributeShopGroupingType.DataBind();

            Dal.ProductCatalogAttributeShopGrouping selectedGrouping = Dal.DbHelper.Shop.GetProductCatalogAttributeShopGrouping(Int32.Parse(ddlShop.SelectedValue), AttributeId);
            if (selectedGrouping != null)
                ddlProductCatalogAttributeShopGroupingType.SelectedIndex = ddlProductCatalogAttributeShopGroupingType.Items.IndexOf(ddlProductCatalogAttributeShopGroupingType.Items.FindByValue(selectedGrouping.ShopGroupingTypeId.ToString()));


        }

        protected void btnAttributeSave_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogAttributeShopGrouping g = new ProductCatalogAttributeShopGrouping()
            {
                AttributeId = AttributeId,
                ShopGroupingTypeId = Int32.Parse(ddlProductCatalogAttributeShopGroupingType.SelectedValue)
            };

            Dal.DbHelper.Shop.SetProductCatalogAttributeShopGrouping(Int32.Parse(ddlShop.SelectedValue), g);

            DisplayMessage("Zapisano");

        }
    }
}