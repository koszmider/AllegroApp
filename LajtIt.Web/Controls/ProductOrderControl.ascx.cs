using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class ProductOrderControl : LajtitControl
    {
        public int ProductCatalogId
        {
            get
            {

                return Convert.ToInt32(ViewState["ProductCatalogId"]);


            }
            set { ViewState["ProductCatalogId"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindProducts();
        }

        private void BindProducts()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            gvProductOrder.DataSource = pch.GetProductOrders(ProductCatalogId);
            gvProductOrder.DataBind();
        }

        protected void btnProductOrder_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ProductOrder po = new Dal.ProductOrder()
            {
                InsertDate = DateTime.Now,
                InsertUser = UserName,
                OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New,
                ProductCatalogId = ProductCatalogId,
                Quantity = Convert.ToInt32(txbQuantity.Text.Trim()),
                Comment = txbComment.Text.Trim()
            };



            pch.SetProductOrder(po);

            DisplayMessage("Produkt został zgłoszony do zamówienia");
            BindProducts();
        }

        protected void gvProductOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType== DataControlRowType.DataRow)
            {
                Dal.ProductOrder po = e.Row.DataItem as Dal.ProductOrder;

                if (po.OrderProductStatusId != (int)Dal.Helper.OrderProductStatus.New)
                    e.Row.Cells[0].Controls.Clear();

            }
        }

        protected void gvProductOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvProductOrder.DataKeys[e.RowIndex][0]);
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetProductOrderDelete(id);

            BindProducts();
        }
    }
}