using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("24C9AFE1-F7BA-46F4-B632-80520445F12C")]
    public partial class Deliveries : LajtitPage
    {
            int count = 0;
            decimal totalCost = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                calDateFrom.SelectedDate = DateTime.Now.AddDays(-1);
                txbDateFrom.Text = calDateFrom.SelectedDate.Value.ToString("yyyy/MM/dd");
                calDateTo.SelectedDate = DateTime.Now;
                txbDateTo.Text = calDateTo.SelectedDate.Value.ToString("yyyy/MM/dd");

                BindSuppplierOwners();
                BindInvoice();
            }
            else
            {
                if (txbDateFrom.Text != "")
                    calDateFrom.SelectedDate = DateTime.Parse(txbDateFrom.Text);
                if (txbDateTo.Text != "")
                    calDateTo.SelectedDate = DateTime.Parse(txbDateTo.Text);

                ddlAction.Visible = true;
            }
        }

        private void BindInvoice()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["costId"]))
            {
                Dal.Cost invoice = Dal.DbHelper.Accounting.GetCost(Int32.Parse(Request.QueryString["costId"].ToString()));
                if (invoice != null)
                    txbInvoiceSearch.Text = invoice.InvoiceNumber;
            }
        }

        private void BindSuppplierOwners()
        {
            ddlSupplierOwner.DataSource = Dal.DbHelper.ProductCatalog.GetSupplierOwners().OrderBy(x=>x.Name).ToList();
            ddlSupplierOwner.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDeliveries();
        }

        private void BindDeliveries()
        {
            bool? hasInvoiceAssigned = null;

            if (ddlInvoice.SelectedIndex > 0)
                hasInvoiceAssigned = ddlInvoice.SelectedValue == "TAK";

              var p=  Dal.DbHelper.ProductCatalog.GetDeliveries(calDateFrom.SelectedDate.Value,
                calDateTo.SelectedDate.Value.AddMinutes(59).AddHours(24),
                Int32.Parse(ddlSupplierOwner.SelectedValue),
                Int32.Parse(ddlPrice.SelectedValue),
                txbDocumentName.Text.Trim(),
                hasInvoiceAssigned,
                txbInvoiceSearch.Text.Trim());


            if (chbPriceDiff.Checked)
            {
                p = p.Where(x => x.Price > 0 && x.PurchasePrice.HasValue && x.PurchasePrice.Value > 0
                  && Math.Abs(x.Price - x.PurchasePrice.Value) > Decimal.Parse(txbPriceDiff.Text)).ToList();

            }

            gvDeliveries.DataSource = p.ToList();
            gvDeliveries.DataBind();
        }

        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            pAction.Visible = true;

            switch (ddlAction.SelectedValue)
            {
                case "0":
                    pnIvoice.Visible = pAction.Visible = false;
                    break;
                case "1":
                    pnIvoice.Visible = true;
                    break;

            }
        }

        protected void btnAction_Click(object sender, EventArgs e)
        {

            switch (ddlAction.SelectedValue)
            {
                case "0":
                    
                    break;
                case "1":
                    SetInvoice();
                    break;

            }
            ddlAction.SelectedIndex = 0;
            ddlAction_SelectedIndexChanged(null, null);
        }

        private void SetInvoice()
        {
            int[] deliveryIds = WebHelper.GetSelectedIds<int>(gvDeliveries, "chbOrder");
            if (deliveryIds.Length==0)
            {
                DisplayMessage("Zaznacz dostawy by przypisac fakturę");
                return;
            }
            if (String.IsNullOrEmpty(hfInvoiceNumber.Value))
            {
                DisplayMessage("Wyszukaj wpierw fakturę");
                return;
            }
            int costId = Int32.Parse(hfInvoiceNumber.Value);
            LajtIt.Dal.DbHelper.ProductCatalog.SetDeliveriesInvoice(deliveryIds, costId);

            BindDeliveries();
        }

        protected void gvDeliveries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblQuantity = e.Row.FindControl("lblQuantity") as Label;
            Label lblPriceTotal = e.Row.FindControl("lblPriceTotal") as Label;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvDeliveries, "Edit$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["style"] = "cursor:pointer";
                e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvDeliveries, "Edit$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["style"] = "cursor:pointer";


                Label lblInvoice = e.Row.FindControl("lblInvoice") as Label;
                Label lblPriceCatalog = e.Row.FindControl("lblPriceCatalog") as Label;
                Literal litCounter = e.Row.FindControl("litCounter") as Literal;
                Label lblPrice = e.Row.FindControl("lblPrice") as Label;
                Label lblDocumentName = e.Row.FindControl("lblDocumentName") as Label;
                Label lblInsertUser = e.Row.FindControl("lblInsertUser") as Label;
                Label lblInsertDate = e.Row.FindControl("lblInsertDate") as Label;
                HyperLink hlProductName = e.Row.FindControl("hlProductName") as HyperLink;
                Label lblCode = e.Row.FindControl("lblCode") as Label;
                Label lblEan = e.Row.FindControl("lblEan") as Label;

                Dal.ProductCatalogDeliveryManagerView delivery = e.Row.DataItem as Dal.ProductCatalogDeliveryManagerView;

                lblInsertDate.Text = delivery.InsertDate.Value.ToString("dd/MM HH:mm");
                lblInsertUser.Text = delivery.InsertUser;
                lblDocumentName.Text = delivery.DocumentName;
                hlProductName.Text = delivery.Name;
                hlProductName.NavigateUrl = String.Format(hlProductName.NavigateUrl, delivery.ProductCatalogId);
                lblCode.Text = delivery.Code;
                lblEan.Text = delivery.Ean;
                litCounter.Text = String.Format("{0}", e.Row.RowIndex + 1);

                if (delivery.PurchasePrice.HasValue)
                    lblPriceCatalog.Text = String.Format("{0:C}", delivery.PurchasePrice);

                lblPriceTotal.Text = String.Format("{0:C}", delivery.Quantity * delivery.Price);
                if (delivery.CostId.HasValue)
                    lblInvoice.Text = String.Format("{0}<br>{1}", delivery.InvoiceNumber, delivery.CompanyName);

                count += delivery.Quantity;
                totalCost += delivery.Quantity * delivery.Price;


                if (lblPrice != null && delivery.Price > 0 && delivery.PurchasePrice > 0 && delivery.Price - delivery.PurchasePrice.Value != 0)
                {

                    if (delivery.PurchasePrice.Value - delivery.Price < 0
                        && Math.Abs(delivery.PurchasePrice.Value - delivery.Price) > Decimal.Parse(txbPriceDiff.Text))
                    {
                        lblPrice.BackColor = Color.Red;
                        lblPrice.ForeColor = Color.White;
                    }

                    if (  delivery.Price - delivery.PurchasePrice.Value < 0)
                    {
                        lblPrice.BackColor = Color.Green;
                        lblPrice.ForeColor = Color.White;
                    }
                }

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox txbPrice = e.Row.FindControl("txbPrice") as TextBox;
                    TextBox txbQuantity = e.Row.FindControl("txbQuantity") as TextBox;

                    txbPrice.Text = delivery.Price.ToString();
                    txbQuantity.Text = delivery.Quantity.ToString();

                    ((TextBox)e.Row.Cells[2].FindControl("txbPrice")).Focus();
                    ((TextBox)e.Row.Cells[2].FindControl("txbPrice")).Attributes.Add("onfocus", "this.select()");
                }
                else
                {
                    lblQuantity.Text = delivery.Quantity.ToString();
                    lblPrice.Text = String.Format("{0:C}", delivery.Price);

                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                lblQuantity.Text = count.ToString(); 
                lblPriceTotal.Text = String.Format("{0:C}", totalCost);

            }
        }

        protected void gvDeliveries_RowEditing(object sender, GridViewEditEventArgs e)
        {

            gvDeliveries.EditIndex = e.NewEditIndex;
            BindDeliveries();
        }

        protected void gvDeliveries_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            gvDeliveries.EditIndex = -1;
            BindDeliveries();
        }

        protected void gvDeliveries_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int deliveryId = Convert.ToInt32(gvDeliveries.DataKeys[e.RowIndex][0]);


            GridViewRow row = gvDeliveries.Rows[e.RowIndex];
            TextBox txbPrice =    row.FindControl("txbPrice") as TextBox;
            TextBox txbQuantity = row.FindControl("txbQuantity") as TextBox;

            Dal.ProductCatalogDelivery delivery = new Dal.ProductCatalogDelivery()
            {
                DeliveryId = deliveryId,
                Quantity = Int32.Parse(txbQuantity.Text.Trim()),
                Price = Decimal.Parse(txbPrice.Text.Trim())
            };

            Dal.DbHelper.ProductCatalog.SetDeliveryUpdate(delivery);


            gvDeliveries.EditIndex = -1;
            BindDeliveries();
        }
    }
}