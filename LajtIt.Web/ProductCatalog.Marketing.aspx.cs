using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("3e7a845d-9955-4cf6-a6f6-5b944433bbf8")]
    public partial class ProductMarketing : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShops();
            }
        }

        private void BindShops()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            gvShops.DataSource = pch.GetProductCatalogShops(ProductCatalogId).Where(x => x.CanExportProducts).ToList() ;
            gvShops.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<Dal.ProductCatalogShopProduct> shops = new List<Dal.ProductCatalogShopProduct>();

            foreach(GridViewRow row in gvShops.Rows)
            {
                string name = ((TextBox)row.FindControl("txbShopProductName")).Text.Trim();
                Dal.ProductCatalogShopProduct shop = new Dal.ProductCatalogShopProduct()
                {
                    ShopId = Convert.ToInt32(gvShops.DataKeys[row.RowIndex][0]),
                    ProductCatalogId = ProductCatalogId,
                    IsNameLocked = ((CheckBox)row.FindControl("chbIsNameLocked")).Checked,
                    Name = String.IsNullOrEmpty(name) ? null : name
                };
                shops.Add(shop);
            }
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogShop(shops, ProductCatalogId);

            DisplayMessage("Zapisano zmiany");
        }

        protected void gvShops_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if(e.Row.RowType== System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                Dal.ProductCatalogShopFnResult shop = e.Row.DataItem as Dal.ProductCatalogShopFnResult;

                TextBox txbShopProductName = e.Row.FindControl("txbShopProductName") as TextBox;
                CheckBox chbIsNameLocked = e.Row.FindControl("chbIsNameLocked") as CheckBox;
                Label lblTemplate = e.Row.FindControl("lblTemplate") as Label;
                Label lblShopTemplate = e.Row.FindControl("lblShopTemplate") as Label;
                Label lblName = e.Row.FindControl("lblName") as Label;
                HyperLink hlProductType = e.Row.FindControl("hlProductType") as HyperLink;

                hlProductType.Text = shop.ProductTypeName;
                hlProductType.NavigateUrl = String.Format("/ProductCatalog.Attributes.aspx?idg={0}#{1}", shop.AttributeGroupId, shop.AttributeId);
                lblShopTemplate.Text = shop.ShopTemplate;
                if (shop.Template != null)
                    lblTemplate.Text = shop.Template;
                txbShopProductName.Text = shop.ShopProductName;
                chbIsNameLocked.Checked = shop.IsNameLocked ?? false;

                 
                lblName.Text = Bll.Mixer.GetProductName(shop.ShopId, ProductCatalogId, true);
            }
        }
    }
}