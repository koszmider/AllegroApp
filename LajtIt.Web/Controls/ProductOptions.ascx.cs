using System;

using System.Web.UI.WebControls;
namespace LajtIt.Web.Controls
{
    public partial class ProductOptions : LajtitControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                Image imgProduct = e.Row.FindControl("imgProduct") as Image;
                HyperLink hlProduct = e.Row.FindControl("hlProduct") as HyperLink;
                CheckBox chbOrder = e.Row.FindControl("chbOrder") as CheckBox;

                Dal.ProductCatalogOptionsFuncResult o = e.Row.DataItem as Dal.ProductCatalogOptionsFuncResult;

                imgProduct.ImageUrl = String.Format("/images/productcatalog/{0}", o.ImageFullName);
                hlProduct.NavigateUrl = String.Format("/Product.aspx?id={0}", o.ProductCatalogOptionId);
                chbOrder.Checked = o.ProductCatalogId != null;
            }
        }
        public void BindProducts(int productCatalogId)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            gvProducts.DataSource = pch.GetProductCatalogOptions(productCatalogId);
            gvProducts.DataBind();
        }
        public int[] GetProductCatalogIds()
        {
            return WebHelper.GetSelectedIds(gvProducts, "chbOrder");
        }
    }
}