using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace LajtIt.Web
{
    [Developer("8a1e245b-ee32-4f62-a261-a4548ff09e1b")]
    public partial class ProductDelivery : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            ucNewDeliveryMove.Saved += BindProductCatalog;
            ucProductOrderControl.ProductCatalogId = ProductCatalogId;

            if (!Page.IsPostBack)
            {
                BindWarehouse(rblWarehouse1);
                BindProductCatalog();
            }


        }

        private void BindProductCatalog(object sender, EventArgs e)
        {
            BindProductCatalog();
        }
        private void BindWarehouse(RadioButtonList rbl)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            rbl.DataSource = pch.GetWarehouse().Where(x => x.CanAssignDelivery).ToList();
            rbl.DataBind();
            rbl.SelectedIndex = 0;
        }
        protected void gvDeliveries_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvDeliveries.Rows[e.RowIndex];

            RadioButtonList rblWarehouse = row.FindControl("rblWarehouse") as RadioButtonList;
            TextBox txbPrice = row.FindControl("txbPrice") as TextBox;
            TextBox txbOrderId = row.FindControl("txbOrderId") as TextBox;

            int deliveryId = Convert.ToInt32(gvDeliveries.DataKeys[e.RowIndex][0]);

            Dal.ProductCatalogDelivery delivery = new Dal.ProductCatalogDelivery()
            {
                Comment = (row.Cells[9].Controls[0] as TextBox).Text,
                DeliveryId = deliveryId,
                Price = Convert.ToDecimal(txbPrice.Text),
                Quantity = Convert.ToInt32((row.Cells[2].Controls[0] as TextBox).Text),
                QuantityBlocked = Convert.ToInt32((row.Cells[3].Controls[0] as TextBox).Text),
                WarehouseId = Convert.ToInt32(rblWarehouse.SelectedValue)
            };

            if (txbOrderId.Text.Trim() != "")
                delivery.OrderId = Int32.Parse(txbOrderId.Text.Trim());

            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();
            h.SetDeliveryUpdate(delivery);

            gvDeliveries.EditIndex = -1;

            BindProductCatalog();
        }
        protected void gvDeliveries_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDeliveries.EditIndex = e.NewEditIndex;
            BindDeliveries();
        }

        protected void gvDeliveries_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDeliveries.EditIndex = -1;
            BindDeliveries();
        }
        protected void CalculateBruto(object sender, EventArgs e)
        {
            litFixedPriceBrutto.Text = String.Format("{0:C}", Bll.ProductCatalogCalculator1.BruttoValue2(Convert.ToDecimal(txbFixedPrice.Text.Trim())));
        }
        protected void btnFixedPriceSave_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();

            h.SetPurchasePrice(ProductCatalogId, Convert.ToDecimal(txbFixedPrice.Text.Trim()));

            DisplayMessage("Zapisano");

        }

        protected void btnDeliveryAdd_Click(object sender, EventArgs e)
        {
            int? importId = null;
            //if (ddlProductImport.SelectedIndex != 0)
                //importId = Convert.ToInt32(ddlProductImport.SelectedValue);



            Dal.ProductCatalogDelivery delivery = new Dal.ProductCatalogDelivery()
            {
                Comment = txbComment.Text.Trim(),
                ImportId = importId,
                Price = Convert.ToDecimal(txbPrice.Text.Trim()),
                ProductCatalogId = ProductCatalogId,
                Quantity = Convert.ToInt32(txbQuantity.Text.Trim()),
                WarehouseId = Convert.ToInt32(rblWarehouse1.SelectedItem.Value),
                QuantityBlocked = Convert.ToInt32(txbQuantityBlocked.Text.Trim())
            };

            if (txbOrderId.Text != "")
                delivery.OrderId = Int32.Parse(txbOrderId.Text);

            if (ViewState["DeliveryId"] != null)
            {
                DisplayMessage("Nowa dostawa została dodana");
                delivery.DeliveryId = Int32.Parse(ViewState["DeliveryId"].ToString());
            }
            else
            {
                DisplayMessage("Zmiany zostały zapisane");
                delivery.InsertDate = DateTime.Now;
                delivery.InsertUser = UserName;
            }
            if (hfInvoiceNumber.Value != "" && txbInvoiceNumber.Text != "")
                delivery.CostId = Int32.Parse(hfInvoiceNumber.Value);

            mpeDelivery.Hide();



            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();

            h.SetDelivery(delivery);

            BindProductCatalog();

            ViewState["DeliveryId"] = null;
        }
        private void BindProductCatalog()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.ProductCatalogView pc = oh.GetProductCatalog(ProductCatalogId);
            Dal.ProductCatalog pcc = Dal.DbHelper.ProductCatalog.GetProductCatalog(ProductCatalogId);
            Dal.ProductCatalogViewFnResult pc1 = oh.GetProductCatalogFn(ProductCatalogId, 1);
            Dal.ProductCatalogViewFnResult pc2 = oh.GetProductCatalogFn(ProductCatalogId, 2);
            Dal.ProductCatalogViewFnResult pc3 = oh.GetProductCatalogFn(ProductCatalogId, 4);


            lblBlocked.Text = pcc.QuantityBlocked.ToString();
            lblTotalQuantity.Text = pcc.TotalQuantity.ToString();
            lblSold.Text = pcc.SoldQuantity.ToString();
            lblLeftQuantity.Text = pcc.LeftQuantity.ToString();

            if (pc.SupplierQuantity.HasValue)
            {
                litTotalWh4.Visible = true;
                lblTotalWh4.Text = pc.SupplierQuantity.ToString();
            }
            else
                litTotalWh4.Visible = false;

            pnlDoNotControlQuantities.Visible = !pnlControlQuantities.Visible;
            int[] importIds = BindDeliveries();

            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();
            //ddlProductImport.DataSource = h.GetImports().Where(x => !importIds.Contains(x.ImportId)).ToList();
            //ddlProductImport.DataBind();
            //ddlProductImport.Items.Insert(0, new ListItem("-- brak --", "0"));

     
            lblTotalWh1.Text = String.Format("{0}", pc1.LeftQuantity);
            lblTotalWh2.Text = String.Format("{0}", pc2.LeftQuantity);
            lblTotalWh3.Text = String.Format("{0}", pc3.LeftQuantity);

            txbFixedPrice.Text = String.Format("{0:0.00}", pc.PurchasePrice);
            //CalculateBruto(null, null);

            ucNewDeliveryMove.BindWarehouseByProduct(ProductCatalogId);
        }

        private int[] BindDeliveries()
        {
            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();

            var q = h.GetDeliveryView(ProductCatalogId);
            gvDeliveries.DataSource = q;
            gvDeliveries.DataBind();

            return q.Where(x => x.ImportId.HasValue).Select(x => x.ImportId.Value).ToArray();
        }


        protected void gvDeliveries_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            Dal.ProductCatalogDeliveryView row = e.Row.DataItem as Dal.ProductCatalogDeliveryView;


            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                Label lblWarehouse = e.Row.FindControl("lblWarehouse") as Label;
                Label lblPrice = e.Row.FindControl("lblPrice") as Label;
                HyperLink hlOrder = e.Row.FindControl("hlOrder") as HyperLink;
                HyperLink hlInvoice = e.Row.FindControl("hlInvoice") as HyperLink;
                Button btnEdit = e.Row.FindControl("btnEdit") as Button;

                btnEdit.CommandArgument = row.DeliveryId.ToString();

                if (row.OrderId.HasValue)
                {
                    hlOrder.Visible = true;
                    hlOrder.NavigateUrl = String.Format(hlOrder.NavigateUrl, row.OrderId);
                    hlOrder.Text = row.OrderId.ToString();
                }
                lblWarehouse.Text = row.WarehouseName;
                lblPrice.Text = String.Format("{0:C}", row.Price);

                if (row.CostId.HasValue)
                {
                    hlInvoice.NavigateUrl = String.Format(hlInvoice.NavigateUrl, row.CostId);
                    hlInvoice.Text = row.InvoiceNumber;
                }
            }
            //if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            //{ 

            //    RadioButtonList rblWarehouse = e.Row.FindControl("rblWarehouse") as RadioButtonList;
            //    TextBox txbPrice = e.Row.FindControl("txbPrice") as TextBox;
            //    TextBox txbOrderId = e.Row.FindControl("txbOrderId") as TextBox;
            //    TextBox txbInvoiceNumber = e.Row.FindControl("txbInvoiceNumber") as TextBox;
            //    HiddenField hlInvoice = e.Row.FindControl("hlInvoice") as HiddenField;
            //    BindWarehouse(rblWarehouse);
            //    rblWarehouse.SelectedValue = row.WarehouseId.ToString();
            //    txbPrice.Text = String.Format("{0}", row.Price);
            //    if (row.OrderId.HasValue)
            //        txbOrderId.Text = String.Format("{0}", row.OrderId);

            //    if(row.CostId.HasValue)
            //    {
            //        txbInvoiceNumber.Text = row.InvoiceNumber;
            //        hlInvoice.Value =  row.CostId.ToString();
            //    }

            //}
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int deliveryId = Int32.Parse(((Button)sender).CommandArgument);

            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();

            var d = h.GetDeliveryView(ProductCatalogId).Where(x => x.DeliveryId == deliveryId).FirstOrDefault();

            txbComment.Text = d.Comment;
            txbPrice.Text = String.Format("{0}", d.Price);
            txbInvoiceNumber.Text = d.InvoiceNumber;
            txbQuantity.Text = d.Quantity.ToString();
            txbQuantityBlocked.Text = d.QuantityBlocked.ToString();
            txbOrderId.Text = String.Format("{0}", d.OrderId);
            rblWarehouse1.SelectedIndex = rblWarehouse1.Items.IndexOf(rblWarehouse1.Items.FindByValue( d.WarehouseId.ToString()));
            
            if (d.CostId.HasValue)
            {
                hfInvoiceNumber.Value = String.Format("{0}", d.CostId);
            }
            ViewState["DeliveryId"] = deliveryId;
            mpeDelivery.Show();

        }

        protected void lbtnNewDelivery_Click(object sender, EventArgs e)
        {
            mpeDelivery.Show();

            txbComment.Text = "";
            txbPrice.Text = "0";
            txbInvoiceNumber.Text = "";
            txbQuantity.Text = "0";
            txbQuantityBlocked.Text = "0";
            txbOrderId.Text = "";
            rblWarehouse1.SelectedIndex=0;


        }
    }
}