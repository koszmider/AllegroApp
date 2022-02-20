using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace LajtIt.Web
{
    [Developer("a933c0a4-7768-4234-90c9-c9b81f126cab")]
    public partial class ProductStatus : LajtitPage
    {


        protected void Page_Load(object sender, EventArgs e)
        { 
            if(!Page.IsPostBack && !String.IsNullOrEmpty(Request.QueryString["ean"]) )
            {
                Dal.ProductCatalogView pc = Dal.DbHelper.ProductCatalog.GetProductCatalogByEan(Request.QueryString["ean"].ToString());

                if (pc != null)
                {
                    BindProduct(pc.ProductCatalogId);

                }
                else
                    BindProduct(0);
            }
        }

        private void BindProduct(int productCatalogId)
        {

            Dal.ProductCatalogView pc;

            if (Request.QueryString["ean"] != null)
                pc = Dal.DbHelper.ProductCatalog.GetProductCatalogByEan(Request.QueryString["ean"].ToString());
            else
                pc = Dal.DbHelper.ProductCatalog.GetProductCatalogView(productCatalogId);

            if(pc==null)
            {
                pnProduct.Visible = false;
                lblProductNotExists.Visible = true;
                return;
            }
            else
            {
                pnProduct.Visible = true;
                lblProductNotExists.Visible = false;

            }
            hfProductCatalogId.Value = pc.ProductCatalogId.ToString();
            txbProductCode.Text = pc.Name;
            imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", pc.ImageFullName);
            imgImage.Visible = pc.ImageFullName != null;

            hlProduct.NavigateUrl = String.Format("/ProductCatalog.Preview.aspx?id={0}", productCatalogId);
            imgImage.Visible = productCatalogId != 0;


            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.ProductCatalogViewFnResult pc1 = oh.GetProductCatalogFn(productCatalogId, 1);
            Dal.ProductCatalogViewFnResult pc2 = oh.GetProductCatalogFn(productCatalogId, 2);
            Dal.ProductCatalogViewFnResult pc3 = oh.GetProductCatalogFn(productCatalogId, 4);

            if (pc.SupplierQuantity.HasValue)
            {
                litTotalWh4.Visible = true;
                lblTotalWh4.Text = pc.SupplierQuantity.ToString();
            }
            else
                litTotalWh4.Visible = false;



            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();


          
            lblTotal.Text = String.Format("{0}", pc.TotalQuantity - pc.SoldQuantity - pc.QuantityBlocked);
            lblTotalWh1.Text = String.Format("{0}", pc1.LeftQuantity);
            lblTotalWh2.Text = String.Format("{0}", pc2.LeftQuantity);
            lblTotalWh3.Text = String.Format("{0}", pc3.LeftQuantity);



            List<Dal.ProductCatalogStatusView> products = Dal.DbHelper.Orders.GetProductCatalogStatus(productCatalogId);

            int[] readyToSend = new int[] { (int)Dal.Helper.OrderStatus.ReadyToPickupByClient, (int)Dal.Helper.OrderStatus.ReadyToSend, (int)Dal.Helper.OrderStatus.Exported };

            gvReadyToSend.DataSource = products.Where(x => readyToSend.Contains(x.OrderStatusId)).ToList();
            gvReadyToSend.DataBind();


            gvWaiting.DataSource = products.Where(x => !readyToSend.Contains(x.OrderStatusId)).ToList();
            gvWaiting.DataBind();


            var i= Dal.DbHelper.ProductCatalog.GetItaluxReturn(pc.ProductCatalogId);

            if (i == null)
                lblItalux.Text = "brak";
            else
                lblItalux.Text = "ok";


        }

        protected void btnProductAdd_Click(object sender, EventArgs e)
        {
            int productCatalogId = Int32.Parse(hfProductCatalogId.Value);


            BindProduct(productCatalogId);
        }

        protected void gvReadyToSend_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType== DataControlRowType.DataRow)
            {
                Dal.ProductCatalogStatusView product = e.Row.DataItem as Dal.ProductCatalogStatusView;

                if(product.WarehouseId==2)
                {
                    e.Row.BackColor = Color.Silver;

                }
            }
        }
    }
}