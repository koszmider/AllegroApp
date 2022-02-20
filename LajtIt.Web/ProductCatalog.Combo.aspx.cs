using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("48537901-fefe-48a2-8d81-8cf778702e0b")]
    public partial class ProductCombo : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindProduct();
            }
        }

        private void BindProduct()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ProductCatalogView product = pch.GetProductCatalogView(ProductCatalogId);

            pnCombo.Visible = product.ProductTypeId == (int)Dal.Helper.ProductType.ComboProduct;
            if (!pnCombo.Visible)
            {
                lblInfo.Visible = true;
                return;
            }


            List<Dal.ProductCatalogSubProductsView> subProducts = pch.GetProductCatalogSubProducts(ProductCatalogId).Where(x=>x.ProductTypeId==(int)Dal.Helper.ProductType.ComboProduct).ToList();

            gvProducts.DataSource = subProducts;
            gvProducts.DataBind();


        }
        protected void gvProducts_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogSubProductsView pc = e.Row.DataItem as Dal.ProductCatalogSubProductsView;

                HyperLink hlPreview = e.Row.FindControl("hlPreview") as HyperLink;
                HyperLink hlProduct = e.Row.FindControl("hlProduct") as HyperLink;
                TextBox txbQuantity = e.Row.FindControl("txbQuantity") as TextBox;
                TextBox txbRebate = e.Row.FindControl("txbRebate") as TextBox;
                Image imgImage = e.Row.FindControl("imgImage") as Image;
                Label lblSupplierName = e.Row.FindControl("lblSupplierName") as Label;
                Label lblCode = e.Row.FindControl("lblCode") as Label;


                hlPreview.NavigateUrl = String.Format(hlPreview.NavigateUrl, pc.ProductCatalogRefId);
                //hlProduct.NavigateUrl = String.Format(hlProduct.NavigateUrl, pc.ProductCatalogId);
                if (pc.ImageFullName == null)
                    imgImage.Visible = false;
                else
                    imgImage.ImageUrl = String.Format("/images/ProductCatalog/{0}", pc.ImageFullName.Replace(".", "_m."));

                txbQuantity.Text = pc.Quantity.ToString();
                txbRebate.Text = String.Format("{0:0.00}", pc.Rebate * 100);
                lblSupplierName.Text = pc.SupplierName;
                lblCode.Text = pc.Code;
            }

        }

        protected void btnProductAdd_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            int subProductCatalogId = Int32.Parse(Request.Form[hfProductCatalogId.UniqueID]);
            pch.SetProductCatalogSubProduct(ProductCatalogId, subProductCatalogId);

            DisplayMessage("Produkt został dodany");

            txbProductCode.Text = "";
            hfProductCatalogId.Value= "";
            BindProduct();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<Dal.ProductCatalogSubProduct> products = new List<Dal.ProductCatalogSubProduct>();


            foreach(GridViewRow row in gvProducts.Rows)
            {
                TextBox txbQuantity = row.FindControl("txbQuantity") as TextBox;
                TextBox txbRebate = row.FindControl("txbRebate") as TextBox;
                CheckBox chbDelete = row.FindControl("chbDelete") as CheckBox;

                if(!chbDelete.Checked)
                {
                    Dal.ProductCatalogSubProduct sub = new Dal.ProductCatalogSubProduct()
                    {
                        Id = Int32.Parse(gvProducts.DataKeys[row.RowIndex][0].ToString()),
                        Quantity = Int32.Parse(txbQuantity.Text.Trim()),
                        Rebate = Decimal.Parse(txbRebate.Text.Trim())/100

                    };
                    products.Add(sub);

                }

            }
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogSubProductsUpdate(ProductCatalogId, products);

            BindProduct();
        }
    }
}