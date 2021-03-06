using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;

namespace LajtIt.Web.Controls
{
    public partial class OrderShipping : LajtitControl
    {
        
        
        public delegate void SavedEventHandler(object sender, EventArgs e);
        public event SavedEventHandler Saved;

        public int OrderId
        {
            set { ViewState["OrderId"] = value;
                BindShippingInfo();
            }
            get { return Convert.ToInt32(ViewState["OrderId"]); }
        }
        private int OrderShippingId
        {
            set { ViewState["OrderShippingId"] = value; }
            get { return Convert.ToInt32(ViewState["OrderShippingId"]); }
        }
        private bool AllowEdit
        {
            get
            {
                return Convert.ToBoolean(ViewState["AllowEdit"]);
            }
            set
            {
                ViewState["AllowEdit"] = value;
            }
        }
        int? orderShippingId = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            ucOrderShippingSimple.OrderId = OrderId;
            ucOrderShippingSimple.Refresh += Refresh;

            if (!Page.IsPostBack)
            {
                Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

                ucOrderShippingSimple.ValidateParcels = order.ShipmentCountryCode == "PL";
                OrderShippingId = 0;
                ddlCourier.DataSource = Dal.DbHelper.Orders.GetShippingCompanies().ToList();
                ddlCourier.DataBind();
                ddlOrderShippingStatus.DataSource = Dal.DbHelper.Orders.GetOrderShippingStatuses();
                ddlOrderShippingStatus.DataBind();
                ddlOrderShippingStatus.SelectedValue = "1";
                BindShippings();
                BindParcelsControl();
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString("N"), "MainFunction();", true);
        }

        private void Refresh()
        {
            mpeAttributesEdit.Show();
        }
        private void BindParcelsControl()
        {
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

            if (rbServiceModel.SelectedIndex != -1)
                ucOrderShippingSimple.BindControl(Int32.Parse(rbServiceModel.SelectedValue));
        }

        private void BindShippings()
        {
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
            orderShippingId = order.OrderShippingId;

            gvOrderShippings.DataSource = Dal.DbHelper.Orders.GetOrderShippings(OrderId).OrderByDescending(x=>x.Id).ToList();
            gvOrderShippings.DataBind();
        }

      

        private void BindShippingInfo()
        {
            Dal.OrdersView ov = Dal.DbHelper.Orders.GetOrderView(OrderId);
            lblShippingCompany.Text = ov.ShippingCompany;
            lblShippingMode.Text = ov.ShippingType;
            if (ov.PayOnDelivery.HasValue)
                lblPayOnDelivery.Visible = ov.PayOnDelivery.Value;
            if (ov.SendFromExternalWerehouse.HasValue && ov.SendFromExternalWerehouse.Value)
                lblSendFromExternalWerehouse.Visible = true;

            if (ov.ShippingServiceModeId == (int)Dal.Helper.ShippingServiceMode.Showroom)
                lblShippingCompany.Visible = false;
            if (!String.IsNullOrEmpty(ov.ServicePoint))
                lblServicePoint.Text = ov.ServicePoint;

            if(!String.IsNullOrEmpty(ov.ShipmentTrackingNumber))
            {
                imgbPrint.CommandArgument = ov.OrderShippingId.ToString();
                imgbPrint.Visible = (ov.ShipmentTrackingNumber != null);
                hlTracking.Text = ov.ShipmentTrackingNumber;
                hlTracking.NavigateUrl = String.Format(ov.TrackingUrl, ov.ShipmentTrackingNumber);

            }
        }

        //internal void BindShipping(Dal.Order order, Dal.ShippingType shippingType, decimal? cost)
        //{
        //    OrderId = order.OrderId; 
        //}

        protected void btnSaveShipping_Click(object sender, EventArgs e)
        {
            SetEditShipping();
        }

        private void SetEditShipping()
        {

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
            Dal.OrderShipping os = new Dal.OrderShipping()
            {
                Id = OrderShippingId,
                COD = GetCODValue(order),
                OrderId = OrderId,
                ShippingCompanyId = Int32.Parse(ddlCourier.SelectedValue),
                ShippingServiceTypeId = Int32.Parse(ddlShippingServiceType.SelectedValue),
                InsertDate = DateTime.Now,
                SendFromExternalWerehouse = chbSendExternal.Checked,
                ShippingServiceModeId = Int32.Parse(rbServiceModel.SelectedValue),
                OrderShippingStatusId = Int32.Parse(ddlOrderShippingStatus.SelectedValue),
                InsertUser=UserName
            };

            if (txbServicePoint.Enabled && !String.IsNullOrEmpty(txbServicePoint.Text))
                os.ServicePoint = hfServicePoint.Value;

            if (rbServiceModel.SelectedIndex == 0)
                os.ServicePoint = null;

            List<Dal.OrderShippingParcel> parcels = ucOrderShippingSimple.GetOrderShippingParcels(null);
            foreach (Dal.OrderShippingParcel parcel in parcels)
            {
                //parcel.OrderShipping = null;
                parcel.OrderShippingId = os.Id;
            }

            if (Int32.Parse(rbServiceModel.SelectedValue) == (int)Dal.Helper.ShippingServiceMode.ExternalShipping)
                os.ShipmentTrackingNumber = parcels[0].ParcelTrackingNumber;

            int orderShippingId = Dal.DbHelper.Orders.SetOrderShipping(os, parcels);
            OrderShippingId =  orderShippingId;
                                // BindShipping(orderShippingId);


            pnOrderShipping.Visible = false;
            lbtnOrderShippingNew.Visible = true;
            BindShippings();
            BindShippingInfo();
        }

        private decimal? GetCODValue(Dal.Order order)
        {
            if (rblCOD.SelectedValue=="1")
                return Math.Abs(order.AmountBalance.Value);
            else
                return null;
        }

        private int? GetIntValue(TextBox textBox)
        {
            int v = 0;
            if (Int32.TryParse(textBox.Text, out v))
                return v;
            else
                return null;
        }
        private int? GetIntValue2(DropDownList ddl)
        { 
            if (ddl.SelectedValue=="0")
                return null;
            else
                return Int32.Parse(ddl.SelectedValue);
        }


 

        private void BindShipping(int orderShippingId)
        {
            ddlShippingServiceType.SelectedValue = ((int)Dal.Helper.OrderShippingStatus.Temporary).ToString();

            Dal.OrderShipping os = Dal.DbHelper.Orders.GetOrderShipping(orderShippingId);

            ddlCourier.SelectedValue = os.ShippingCompanyId.ToString();
            ddlShippingServiceType.SelectedValue = os.ShippingServiceTypeId.ToString();
            rblCOD.SelectedValue = os.COD.HasValue && os.COD > 0 ? "1" : "0";
            ddlOrderShippingStatus.SelectedValue = os.OrderShippingStatusId.ToString();
            rbServiceModel.SelectedValue = os.ShippingServiceModeId.ToString();
            chbSendExternal.Checked = os.SendFromExternalWerehouse;

            txbServicePoint.Text = os.ServicePoint;
            hfServicePoint.Value = os.ServicePoint;
            // BindShippingParcels(orderShippingId);

            ucOrderShippingSimple.BindControlFromShipping(os.Id);

            if (os.OrderShippingStatusId == (int)Dal.Helper.OrderShippingStatus.Generated)
            {
                lbtnOrderShippingNew.Visible = true;
                pnOrderShipping.Enabled = false;
            }
            else
            {
                lbtnOrderShippingNew.Visible = true;
                pnOrderShipping.Enabled = true;
            }
        }

    
        protected void imgbPrint_Click(object sender, ImageClickEventArgs e)
        {
            int orderShippingId = Int32.Parse(((ImageButton)sender).CommandArgument);

            Dal.OrderShipping os = Dal.DbHelper.Orders.GetOrderShipping(orderShippingId);  


            if (!Bll.OrderHelper.ExportFile(new string[] { String.Format(@"Shipping\{0}\{1}.pdf", 
                os.ShippingCompany.Name, 
                os.ShipmentTrackingNumber) }))
                DisplayMessage("Etykieta nie istnieje");


        }
        protected void gvOrderShippings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.OrderShippingView orderShipping = e.Row.DataItem as Dal.OrderShippingView;

                LinkButton lbtnOrderShipping = e.Row.FindControl("lbtnOrderShipping") as LinkButton;
                HyperLink hlTracking = e.Row.FindControl("hlTracking1") as HyperLink;
                ImageButton imgbPrint = e.Row.FindControl("imgbPrint1") as ImageButton;

                imgbPrint.CommandArgument = orderShipping.Id.ToString();
                imgbPrint.Visible = (orderShipping.ShipmentTrackingNumber != null);
                hlTracking.Text = orderShipping.ShipmentTrackingNumber;
                hlTracking.NavigateUrl = String.Format(orderShipping.TrackingUrl, orderShipping.ShipmentTrackingNumber);
                lbtnOrderShipping.Text = String.Format(lbtnOrderShipping.Text, orderShipping.InsertDate);
                lbtnOrderShipping.CommandArgument = orderShipping.Id.ToString();

                //  if(orderShipping.ShippingServiceModeId == (int)Dal.Helper.ShippingServiceMode.Showroom)

                if (orderShippingId.HasValue && orderShipping.Id == orderShippingId.Value)
                    e.Row.BackColor = Color.Silver;
            }
        }

        protected void lbtnOrderShipping_Click(object sender, EventArgs e)
        {
            int orderShippingId = Int32.Parse(((LinkButton)sender).CommandArgument);

            OrderShippingId = orderShippingId;

            BindShipping(orderShippingId);

             
            pnOrderShipping.Visible = true;
            mpeAttributesEdit.Show();

            //ddlSize.SelectedIndex = 0;
            //txbHeight.Text = txbLength.Text = txbWeight.Text = txbWidth.Text = "";
        }

        protected void gvOrderShippingParcel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.OrderShippingParcel parcel = e.Row.DataItem as Dal.OrderShippingParcel;

                DropDownList ddlSize = e.Row.FindControl("ddlSize") as DropDownList;
                TextBox txbWeight = e.Row.FindControl("txbWeight") as TextBox;
                TextBox txbHeight = e.Row.FindControl("txbHeight") as TextBox;
                TextBox txbWidth = e.Row.FindControl("txbWidth") as TextBox;
                TextBox txbLength = e.Row.FindControl("txbLength") as TextBox;
                if (parcel.Size.HasValue)
                    ddlSize.SelectedValue = parcel.Size.Value.ToString();
                txbWeight.Text = GetString(parcel.Weight);
                txbHeight.Text = GetString(parcel.DimnesionHeight);
                txbWidth.Text = GetString(parcel.DimnesionWidth);
                txbLength.Text = GetString(parcel.DimnesionLength);




                ImageButton ibtnDelete = e.Row.FindControl("ibtnDelete") as ImageButton;
                 
                ibtnDelete.CommandArgument = parcel.Id.ToString();
            }
        }

        private string GetString(decimal? value)
        {
            if (value.HasValue)
                return value.ToString();
            else
                return "";
        }

        protected void lbtnOrderShippingNew_Click(object sender, EventArgs e)
        {

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

            //if (order.AmountBalance < 0)
            //    chbCOD.Checked=true;

            if (order.AmountBalance.Value < 0)
                lblCOD.Text = String.Format("{0:C}", -order.AmountBalance.Value);
            else
                lblCOD.Text = "";
            pnOrderShipping.Enabled = true;
            OrderShippingId = 0;
            //BindShippingParcels(0);
            pnOrderShipping.Visible = true;
           lbtnOrderShippingNew.Visible = false;
            mpeAttributesEdit.Show();
        }

        protected void ibtnAdd_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

        }

        protected void ibtnDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            int parcelId = Int32.Parse(((ImageButton)sender).CommandArgument);
            Dal.DbHelper.Orders.SetOrderShippingParcelDelete(parcelId);

            //BindShippingParcels(OrderShippingId);
            mpeAttributesEdit.Show();

        }

        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            pnAttributes2.Visible = false;
            lbtnOrderShippingNew.Visible = true;
            mpeAttributesEdit.Hide();
        }

        protected void rbServiceModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucOrderShippingSimple.BindControl(Int32.Parse(rbServiceModel.SelectedValue));
            mpeAttributesEdit.Show();
        }
    }
}