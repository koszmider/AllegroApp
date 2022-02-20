using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using System.Collections.Generic;
using System.Drawing;

namespace LajtIt.Web
{
    [Developer("e63f9b88-d566-4b4c-8b78-cb43bd83c887")]
    public partial class AllegroOrders : LajtitPage
    {
        decimal totalPage = 0, totalAll = 0;
        decimal paidPage = 0, paidAll = 0;
        decimal leftPage = 0, leftAll = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShops();
                BindStatuses();
                BindShippingCompanies();

                BindProductCatalog();

                BindResults(); 

                //calDateFrom.SelectedDate = DateTime.Now.AddMonths(-12);
                //txbDateFrom.Text = calDateFrom.SelectedDate.Value.ToString("yyyy/MM/dd");
                //calDateTo.SelectedDate = DateTime.Now;
                //txbDateTo.Text = calDateTo.SelectedDate.Value.ToString("yyyy/MM/dd");


            }
            else
            {
                if (txbDateFrom.Text != "")
                    calDateFrom.SelectedDate = DateTime.Parse(txbDateFrom.Text);
                if (txbDateTo.Text != "")
                    calDateTo.SelectedDate = DateTime.Parse(txbDateTo.Text);
            }
        }

        private void BindShops()
        { 
            lbxShop.DataSource = Dal.DbHelper.Shop.GetShops().Where(x => x.IsActive&& x.CanPlaceOrders).OrderBy(x => x.Name).ToList();
            lbxShop.DataBind();
        }

        private void BindProductCatalog()
        {
            if (Request.QueryString["idProduct"] != null && ViewState["RemoveProductCatalog"] == null)
            {
                litProductCatalog.Visible = lbtnProductCatalog.Visible = true;

                Dal.OrderHelper oh = new Dal.OrderHelper();

                litProductCatalog.Text = oh.GetProductCatalog(Convert.ToInt32(Request.QueryString["idProduct"])).Name;
                lsbOrderStatus.SelectedValue = "3";
            }
            else
                lsbOrderStatus.SelectedValue = "1";

            if (Request.QueryString["email"] != null)
            {
                txbName.Text = Request.QueryString["email"];

            }
            if (Request.QueryString["tracking"] != null)
            {
                txbName.Text = Request.QueryString["tracking"];
                lsbOrderStatus.SelectedIndex = -1;

            }
        }

        protected void lbtnProductCatalog_Click(object sender, EventArgs e)
        {
            ViewState["RemoveProductCatalog"] = "1";
            litProductCatalog.Visible = lbtnProductCatalog.Visible = false;
            BindResults();
        } 

        private void BindShippingCompanies()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            chblShippingCompany.DataSource = oh.GetShipppingCompanies();
            chblShippingCompany.DataBind();
            //foreach (ListItem item in chblShippingCompany.Items.Cast<ListItem>().ToList())
            //    item.Selected = true;

            if (this.UserShopId != 0)
                chblShippingCompany.SelectedValue = "3";

        }

        private void BindStatuses()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            lsbOrderStatus.DataSource = oh.GetOrderStatuses(true);
            lsbOrderStatus.DataBind();
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {

        }
        protected void gvOrders_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrders.PageIndex = e.NewPageIndex;
            BindResults();
        }
        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            BindResults();
        }

        private void BindResults()
        {
            int[] orderStatusIds = lsbOrderStatus.Items.Cast<ListItem>()
                .Where(x => x.Selected == true)
                .Select(x => Convert.ToInt32(x.Value))
                .ToArray();
            int[] shopIds = lbxShop.Items.Cast<ListItem>()
                .Where(x => x.Selected == true)
                .Select(x => Convert.ToInt32(x.Value))
                .ToArray(); ;

            int[] shippingCompanyIds = chblShippingCompany.Items.Cast<ListItem>()
                .Where(x => x.Selected == true)
                .Select(x => Convert.ToInt32(x.Value))
                .ToArray();
            int? productCatalogId = null;
            if (Request.QueryString["idProduct"] != null && ViewState["RemoveProductCatalog"] == null)
                productCatalogId = Convert.ToInt32(Request.QueryString["idProduct"]);
          
            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            if (txbDateTo.Text != "")
                dateTo = calDateTo.SelectedDate.Value.AddDays(1).AddMilliseconds(-1);

            if (txbDateFrom.Text != "")
                dateFrom = calDateFrom.SelectedDate.Value;


            int? hasInvoiceParagon = null;
            if (ddlInvoice.SelectedIndex == 1) hasInvoiceParagon = 1;
            if (ddlInvoice.SelectedIndex == 2) hasInvoiceParagon = 2;
            if (ddlInvoice.SelectedIndex == 3) hasInvoiceParagon = 3;

            string orderId = null;
            if (!String.IsNullOrEmpty(txbOrderId.Text))
                orderId = txbOrderId.Text.Trim();


            Bll.OrderHelper oh = new Bll.OrderHelper();
            bool? isPaid = null;

            if (chbPaid1.Checked)
                isPaid = true;

            if (chbPaid0.Checked)
                isPaid = false;
            List<Dal.OrdersView> orders = oh.GetOrders(orderId,
                isPaid,
                chbPayOnDelivery.Checked,
                orderStatusIds,
                shippingCompanyIds,
                txbName.Text.Trim(),
                txbInvoiceNumber.Text.Trim(),
                productCatalogId,
                hasInvoiceParagon,
                dateFrom,
                dateTo,
                shopIds,
                this.UserShopId,
                UserName)
                .OrderBy(x => x.LastStatusChangeDate).ToList();
            totalAll = orders.Sum(x => x.AmountToPay);
            paidAll = orders.Sum(x => x.AmountPaid);
            leftAll = orders.Sum(x => x.AmountBalance??0);
            gvOrders.DataSource = orders;
            gvOrders.DataBind();

        }
        protected void gvOrders_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal LitId = e.Row.FindControl("LitId") as Literal;
                Literal litOrderId = e.Row.FindControl("litOrderId") as Literal;
                Label lblNick = e.Row.FindControl("lblNick") as Label;
                Label lblAmountToPay = e.Row.FindControl("lblAmountToPay") as Label;
                Label lblAmountPaid = e.Row.FindControl("lblAmountPaid") as Label;
                Label lblAmountBalance = e.Row.FindControl("lblAmountBalance") as Label;
                Label lblClient = e.Row.FindControl("lblClient") as Label;
                Label lblShippingMode = e.Row.FindControl("lblShippingMode") as Label;
                Label lblShippingCompany = e.Row.FindControl("lblShippingCompany") as Label;
                Label lblPayOnDelivery = e.Row.FindControl("lblPayOnDelivery") as Label;
                Label lblSendFromExternalWerehouse = e.Row.FindControl("lblSendFromExternalWerehouse") as Label;
                HyperLink hlOrderId = e.Row.FindControl("hlOrderId") as HyperLink;
                System.Web.UI.WebControls.Image imgFlag = e.Row.FindControl("imgFlag") as System.Web.UI.WebControls.Image;

                System.Web.UI.WebControls.Image imgSource = e.Row.FindControl("imgSource") as System.Web.UI.WebControls.Image;

                LitId.Text = (e.Row.RowIndex + 1 + gvOrders.PageSize * gvOrders.PageIndex).ToString();
                
                int[] statuses = new int[]{(int)Dal.Helper.OrderStatus.WaitingForClient,
                    (int)Dal.Helper.OrderStatus.WaitingForPayment,
                    (int)Dal.Helper.OrderStatus.WaitingForProduct};

                Dal.OrdersView ov = e.Row.DataItem as Dal.OrdersView;

                lblShippingCompany.Text = ov.ShippingCompany;
                lblShippingMode.Text = ov.ShippingType;
                if (ov.PayOnDelivery.HasValue)
                    lblPayOnDelivery.Visible = ov.PayOnDelivery.Value;
                if (ov.SendFromExternalWerehouse.HasValue && ov.SendFromExternalWerehouse.Value)
                    lblSendFromExternalWerehouse.Visible = true;

                if (ov.ShippingServiceModeId == (int)Dal.Helper.ShippingServiceMode.Showroom)
                    lblShippingCompany.Visible = false;

                litOrderId.Text = ov.OrderId.ToString();
                imgSource.ImageUrl = String.Format(@"/images/sources/{0}.png", ov.ShopId);
                
                lblAmountToPay.Text = String.Format("{0:C}", ov.AmountToPay);
                lblAmountPaid.Text = String.Format("{0:C}", ov.AmountPaid);
                lblAmountBalance.Text = String.Format("{0:C}", ov.AmountBalance);

                if (ov.AmountToPay == ov.AmountPaid)
                    lblAmountPaid.ForeColor = Color.Green;

                if (ov.AmountBalance < 0)
                    lblAmountBalance.ForeColor = Color.Red;
                lblClient.Text = ov.Client;
                imgFlag.ImageUrl = String.Format("/images/flags/{0}.png", ov.ShipmentCountryCode);
                lblNick.Text = ov.UserName;
                hlOrderId.NavigateUrl = String.Format(hlOrderId.NavigateUrl, ov.OrderId);
                //hlOrderId.Text = ov.OrderId.ToString();

                totalPage += Convert.ToDecimal(ov.AmountToPay);
                paidPage += Convert.ToDecimal(ov.AmountPaid);
                leftPage += Convert.ToDecimal(ov.AmountBalance);

                if (lsbOrderStatus.Items.Cast<ListItem>().Where(x => x.Selected == true).Count() == 1)
                {
                    Dal.OrdersView order = (Dal.OrdersView)e.Row.DataItem;
                    if (order.LastStatusChangeDate.HasValue && statuses.Contains(Convert.ToInt32(lsbOrderStatus.SelectedValue)))
                    {
                        TimeSpan ts = DateTime.Now - order.LastStatusChangeDate.Value;

                        if (ts.TotalHours > 96)
                            e.Row.CssClass = "red_row";


                    }
                }
            }
            if (UserName == "jacek" || UserName == "magda")
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[5].Text = String.Format("<b>Strona:<br>{0:C}<br>Razem:<br>{1:C}</b><br>", totalPage, totalAll);
                    e.Row.Cells[5].Text += String.Format("Strona:<br>{0:C}<br>Razem:<br>{1:C}<br>", paidPage, paidAll);
                    e.Row.Cells[5].Text += String.Format("<span style='color:red'>Strona:<br>{0:C}<br>Razem:<br>{1:C}</span>", leftPage, leftAll);
                }
            }
        }
    }
}