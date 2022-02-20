using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("e3c3338e-292e-46b8-848f-0c452035d3db")]
    public partial class ProductCatalogOnStock : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindProducts();
        }
        protected void chblSuppliers_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindProducts();
        }
        private void BindProducts()
        {
            Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();

            List<Dal.ProductCatalogOnStock> products = oh.GetProductCatalogOnStock();


            if (Page.IsPostBack)
            {
                int[] suppliersIds = chblSuppliers.Items.Cast<ListItem>()
                    .Where(x => x.Selected)
                    .Select(x => Convert.ToInt32(x.Value))
                    .ToArray();
                int[] lampTypeId = chbLampType.Items.Cast<ListItem>()
                    .Where(x => x.Selected)
                    .Select(x => Convert.ToInt32(x.Value))
                    .ToArray();
                if (suppliersIds.Length > 0)
                    products = products.Where(x => suppliersIds.Contains(x.SupplierId)).ToList();

                if(!String.IsNullOrEmpty(txbPriceFrom.Text))
                products = products.Where(x => x.PriceBruttoFixed >= Convert.ToDecimal(txbPriceFrom.Text)).ToList();

                if (!String.IsNullOrEmpty(txbPriceTo.Text))
                    products = products.Where(x => x.PriceBruttoFixed <= Convert.ToDecimal(txbPriceTo.Text)).ToList();


                if (lampTypeId.Length > 0)
                {
                   int[] filteredProductCatalogIds =   oh.GetProductCatalogForAttributesAndProducts(lampTypeId, products.Select(x => x.ProductCatalogId).ToArray());
                    products = products.Where(x => filteredProductCatalogIds.Contains(x.ProductCatalogId)).ToList();

                }
            }
            else
            {
                var s = products.Select(x => new
                {
                    SupplierId = x.SupplierId,
                    SupplierName = x.SupplierName

                }).Distinct().OrderBy(x=>x.SupplierName).ToList();


                chblSuppliers.DataSource = s;
                chblSuppliers.DataBind();

                chbLampType.DataSource = oh.GetProductCatalogAttributesForGroupAndProducts(6, products.Select(x=>x.ProductCatalogId).ToArray()).OrderBy(x=>x.Name);
                chbLampType.DataBind();
            }
         
            rpProducts.DataSource = products.OrderBy(x=>x.Name);
            rpProducts.DataBind();
        }

        protected void rpProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Dal.ProductCatalogOnStock product = e.Item.DataItem as Dal.ProductCatalogOnStock;


                Image imgImage = e.Item.FindControl("imgImage") as Image;
                HyperLink hlImage = e.Item.FindControl("hlImage") as HyperLink;
                HyperLink hlProduct = e.Item.FindControl("hlProduct") as HyperLink;
                Label lblQuantity = e.Item.FindControl("lblQuantity") as Label;
                Label lbPrice = e.Item.FindControl("lbPrice") as Label;

                if (!String.IsNullOrEmpty(product.ImageFullName))
                    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}_m{1}", 
                        System.IO.Path.GetFileNameWithoutExtension(product.ImageFullName), System.IO.Path.GetExtension(product.ImageFullName));
                else
                    imgImage.Visible = false;

                hlProduct.NavigateUrl = String.Format(hlProduct.NavigateUrl, product.ProductCatalogId, product.SupplierId);


                hlImage.NavigateUrl = hlProduct.NavigateUrl;
                
                if ( product.IsActivePricePromo)
                    lbPrice.Text = String.Format("{0:C} <span class='promo'>{1:C}</span>", product.PriceBruttoPromo, product.PriceBruttoFixed);
                else
                    lbPrice.Text = String.Format("{0:C}", product.PriceBruttoFixed);
                lblQuantity.Text = String.Format("{0} szt.", product.Quantity);
                hlProduct.Text = product.Name;


            }
        }
    }
}