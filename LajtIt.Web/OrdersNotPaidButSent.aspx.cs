using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;

namespace LajtIt.Web
{
    [Developer("14a8bd6e-11a0-4bff-9e6a-83cdf20784e1")]
    public partial class OrdersNotPaidButSent : LajtitPage
    {
        private decimal amount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                calDate.SelectedDate =  DateTime.Now;
                txbDate.Text = calDate.SelectedDate.Value.ToString("yyyy/MM/dd");
                BindShippingCompanies();
                BindOrders(null);

                //OrderHelper oh = new OrderHelper();
                //ddlOrderPaymentTypes.DataSource = oh.GetOrderPaymentTypes().Where(x => x.IsActive).OrderBy(x => x.PaymentOrderId).ToList();
                //ddlOrderPaymentTypes.DataBind();
                //ddlOrderPaymentTypes.Items.Insert(0, new ListItem());
            }
            else
            {
                calDate.SelectedDate = DateTime.Parse(txbDate.Text);
            }


        }
        protected void ddlShippingCompany_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindOrders(null);
        }
        private void BindShippingCompanies()
        {
            OrderHelper oh = new OrderHelper();
            ddlShippingCompany.DataSource = oh.GetShipppingCompanies();
            ddlShippingCompany.DataBind();
        }
        private void BindOrders(string[] trackingNumbers)
        {
            OrderHelper oh = new OrderHelper();
            List<Dal.OrdersNotPaidButSentView> q = oh.GetOrdersNotPaidButSent(Convert.ToInt32(ddlShippingCompany.SelectedValue));
            if (trackingNumbers != null)
                q = q.Where(x => trackingNumbers.Contains(x.ShipmentTrackingNumber)).ToList();

            gvOrders.DataSource = q.OrderBy(x => x.SentDate); ; ;
            gvOrders.DataBind();

            if (trackingNumbers != null)
            {
                string[] ordersFound = q.Select(x => x.ShipmentTrackingNumber).Distinct().ToArray();
                string[] ordersNotFound = trackingNumbers.Where(x => !ordersFound.Contains(x)).ToArray();

                if (ordersNotFound.Length > 0)
                    lblOrdersNotFound.Text = String.Format("Zamówienia nieodnalezione: {0}", String.Join(", ", ordersNotFound));
            }

            if (trackingNumbers != null)
            {
                litCount.Text = String.Format("{0}", q.Count);
                litAmount.Text = String.Format("{0:C}", q.Sum(x => x.COD.Value));
                pNumbers.Visible = true;
            }
        }
        protected void lnbCancel_Click(object sender, EventArgs e)
        {
            pNumbers.Visible = false;
            ddlOrderPaymentTypes.SelectedIndex = 0;
        }
        protected void btnAddPayments_Click(object sender, EventArgs e)
        {


            Bll.OrderHelper oh = new Bll.OrderHelper();
            decimal sum = oh.SetOrderPayments(GetTrackingNumbers(), 
                calDate.SelectedDate.Value, 
                Convert.ToInt32(ddlOrderPaymentTypes.SelectedValue), 
                Convert.ToInt32(ddlShippingCompany.SelectedValue),
                UserName);

            if (sum == 0)
            {
                DisplayMessage("Nie dodano żadnych płatności. Sprawdź wprowadzone dane i spróbuj ponownie");
                return;
            }
            DisplayMessage(String.Format("Dodana płatności na kwotę {0:C}", sum));
            pNumbers.Visible = false;
            ddlOrderPaymentTypes.SelectedIndex = 0;
            txbNumbers.Text = "";

            BindOrders(null);
        }

        protected void txbCheck_Click(object sender, EventArgs e)
        {
            string[] trackingNumbers = GetTrackingNumbers(); 
            BindOrders(trackingNumbers);
        }

        private string[] GetTrackingNumbers()
        {
            return System.Text.RegularExpressions.Regex.Split(txbNumbers.Text.Trim(), @"[^\dU.]+"); 
        }
        protected void gvOrders_OnRowDataBound(object sender,  GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            { 
                e.Row.Cells[5].Text = String.Format("-{0:C}", amount);
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.OrdersNotPaidButSentView order = e.Row.DataItem as Dal.OrdersNotPaidButSentView;
                amount += order.AmountToPay;

                int days = DateTime.Now.Subtract(order.SentDate.Value).Days;
                Literal litDays = e.Row.FindControl("litDays") as Literal;
                HyperLink hlUrl = e.Row.FindControl("hlUrl") as HyperLink;
                litDays.Text= days.ToString();

                hlUrl.Text = order.ShipmentTrackingNumber;
                hlUrl.NavigateUrl = String.Format(order.TrackingUrl, order.ShipmentTrackingNumber);


                if ( days> 10)
                {
                    e.Row.Style.Add("background-color", "red");
                    e.Row.Style.Add("color", "white");
                }
            }
        
        }
    }
}