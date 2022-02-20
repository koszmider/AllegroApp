using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("ec93d992-5eeb-4089-adad-1de28dfd075d")]
    public partial class ProductsTracker : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                BindSuppliers(); 


            }
        }

        private void BindProducts()
        {
            int quantity = Convert.ToInt32(txbQuantity.Text);
            int supplierId = 0;

            if (ddlSuppliers.SelectedIndex != -1)
                supplierId= Convert.ToInt32(ddlSuppliers.SelectedValue);

            Dal.OrderHelper oh = new Dal.OrderHelper();


            List<Dal.ProductCatalogViewFnResult> products = oh.GetProductCatalog(txbSearchName.Text.Trim(), new int[] { supplierId }, null).Where(x=>x.IsAvailable && !x.IsDiscontinued).ToList();

            //if (lsbxStatus.Items.FindByValue("-1").Selected) products = products.Where(x => x.IsDiscontinued).ToList();


            products = products.Where(x => x.SupplierQuantity.HasValue && x.SupplierQuantity.Value > 0 && x.SupplierQuantity.Value <= quantity).ToList();
            switch(ddlPromotion.SelectedIndex)
            {
                case 1: products = products.Where(x => x.IsActivePricePromo).ToList(); break;
                case 2: products = products.Where(x => !x.IsActivePricePromo).ToList(); break;

            }
            gvProductCatalog.DataSource = products;
            gvProductCatalog.DataBind();
        }

        private void BindSuppliers()
        {
            ddlSuppliers.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliersWithQuantityTrackingEnabled();
            ddlSuppliers.DataBind();
        }
        protected void gvProductCatalog_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gvProductCatalog.PageIndex = e.NewPageIndex;
            BindProducts();
        }
        protected void gvProductCatalog_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
             
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogViewFnResult pc = e.Row.DataItem as Dal.ProductCatalogViewFnResult;

                //HyperLink hlCalcuator = e.Row.FindControl("hlCalcuator") as HyperLink;
                // hlCalcuator.NavigateUrl = String.Format("/ProductCalculator.aspx?id={0}", pc.ProductCatalogId);
                Image imgImage = e.Row.FindControl("imgImage") as Image;
                HyperLink hlShop = e.Row.FindControl("hlShop") as HyperLink;
                HyperLink hlAllegro = e.Row.FindControl("hlAllegro") as HyperLink;
                Literal liId = e.Row.FindControl("liId") as Literal;
                HyperLink hlProduct = e.Row.FindControl("hlProduct") as HyperLink; 
              
                Label lblRealPurchasePrice = e.Row.FindControl("lblRealPurchasePrice") as Label;
                Label lblSellPrice = e.Row.FindControl("lblSellPrice") as Label;  
                Label lblQuantity = e.Row.FindControl("lblQuantity") as Label;
                Label lblSupplierQuantity = e.Row.FindControl("lblSupplierQuantity") as Label;
                Label lblCode = e.Row.FindControl("lblCode") as Label;
                Label lblCode2 = e.Row.FindControl("lblCode2") as Label;
                Label lblCodeSupplier = e.Row.FindControl("lblCodeSupplier") as Label;
                Label lblExternalId = e.Row.FindControl("lblExternalId") as Label;
                 
                hlProduct.NavigateUrl = String.Format(hlProduct.NavigateUrl, pc.ProductCatalogId); 
                hlProduct.Text = pc.Name; 

                liId.Text = String.Format("{0}.", gvProductCatalog.PageIndex * gvProductCatalog.PageSize + e.Row.RowIndex + 1);


                if (!String.IsNullOrEmpty(pc.ImageFullName))
                    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}_m{1}", System.IO.Path.GetFileNameWithoutExtension(pc.ImageFullName), System.IO.Path.GetExtension(pc.ImageFullName));
                else
                    imgImage.Visible = false;
                //if (!pc.IsReady)
                //    e.Row.Style.Add("background-color", "silver");

                //hlAllegro.Visible = pc.IsActiveAllegro;
                //hlShop.Visible = pc.ShopProductId.HasValue;
                //hlShop.NavigateUrl = String.Format(hlShop.NavigateUrl, pc.ShopProductId);
                //hlAllegro.NavigateUrl = String.Format(hlAllegro.NavigateUrl, pc.Code, pc.AllegroUserIdAccount);

                lblCodeSupplier.Text = pc.Ean;
                lblCode.Text = pc.Code;
                lblCode2.Text = pc.Code2;
                lblExternalId.Text = pc.ExternalId;
                //Bll.ProductCatalogCalculator calc = new ProductCatalogCalculator(pc.CurrentPrice, pc.AllegroPrice, pc.Rebate.Value, pc.Margin.Value);

                lblSellPrice.Text = String.Format("{0:C}", pc.PriceBruttoFixed);
                
                lblQuantity.Text = pc.LeftQuantity > 0 ? pc.LeftQuantity.ToString() : "-";

                if (pc.SupplierQuantity.HasValue)
                {
                    if (pc.SupplierQuantity.Value == -1)
                        lblSupplierQuantity.Text = String.Format(lblSupplierQuantity.Text, "dost.");
                    else
                        lblSupplierQuantity.Text = String.Format(lblSupplierQuantity.Text, pc.SupplierQuantity);
                    lblSupplierQuantity.Visible = true;
                } 

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindProducts();
        }
    }
}