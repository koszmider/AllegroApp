using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class TrackingUrlControl : LajtitControl
    {
        public int OrderId
        {
            set { ViewState["OrderId"] = value; }
            get { return Convert.ToInt32(ViewState["OrderId"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindTrackingNumber();
        }

        protected void imgbPrint_Click(object sender, ImageClickEventArgs e)
        {
            throw new NotImplementedException();

            //Dal.OrderHelper oh = new Dal.OrderHelper();
            //Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

            //if (order.ShippingType.ShippingCompany == null)
            //    return;


            //if (!Bll.OrderHelper.ExportFile(new string[] { String.Format(@"Shipping\{0}\{1}.pdf", order.ShippingType.ShippingCompany.Name, order.ShipmentTrackingNumber) }))
            //    DisplayMessage("Etykieta nie istnieje");


        }
        private void BindTrackingNumber()
        {

            throw new NotImplementedException();
            //Dal.OrderHelper oh = new Dal.OrderHelper();

            //Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
            //imgbEdit.Visible = true;
            //imgbSave.Visible = false;
            //imgbCancel.Visible = false;

            //lblTrackingNumber.Visible = true;
            //hlTrackingNumber.Visible = false;

            //txbTrackingNumber.Visible = false;

            //if (String.IsNullOrEmpty(order.ShipmentTrackingNumber))
            //{
            //}
            //else
            //{
            //    if (order.ShippingType != null && order.ShippingType.ShippingCompany!=null)
            //    {
            //        lblTrackingNumber.Visible = false;
            //        hlTrackingNumber.Visible = true;
            //        hlTrackingNumber.Text = String.Format("{0} ({1})", order.ShipmentTrackingNumber, order.ShippingType.ShippingCompany.Name);
            //        hlTrackingNumber.NavigateUrl = String.Format(order.ShippingType.ShippingCompany.TrackingUrl, order.ShipmentTrackingNumber);
            //    }
            //}
        }

        protected void imgbSave_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetOrderTrackingNumber(OrderId, txbTrackingNumber.Text.Trim());

            BindTrackingNumber();
        }
        protected void imgbEdit_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

            lblTrackingNumber.Visible = hlTrackingNumber.Visible = false;
            txbTrackingNumber.Visible = true;
            imgbSave.Visible = true;
            imgbEdit.Visible = false;
            imgbCancel.Visible = true;
            txbTrackingNumber.Text = order.ShipmentTrackingNumber;

        }

        protected void imgbCancel_Click(object sender, EventArgs e)
        {

            BindTrackingNumber();
        }
    }
}