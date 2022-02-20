using LajtIt.Bll;
using LajtIt.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class OrderShippingSimple : LajtitControl
    {
        public int OrderId
        {
            set
            {
                ViewState["OrderId"] = value;

            }
            get { return Convert.ToInt32(ViewState["OrderId"]); }
        }
        public bool ValidateParcels
        {
            set
            {
                ViewState["ValidateParcels"] = value;

            }
            get {
                if (ViewState["ValidateParcels"] == null)
                    return true;

                return Convert.ToBoolean(ViewState["ValidateParcels"]); }
        }
        private Dal.Order order;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public delegate void SavedEventHandler();
        public event SavedEventHandler Saved;
        public delegate void RefreshedEventHandler();
        public event RefreshedEventHandler Refresh;

        private class Parcel
        {
            public decimal? Weight { get; set; }
            public decimal? Width { get; set; }
            public decimal? Length { get; set; }
            public decimal? Height { get; set; }
            public int? Size { get; set; }
            public string TrackingNumber { get; set; }
        }

        protected void btnInpostSave_Click(object sender, EventArgs e)
        {

            Dal.OrderShipping os = GetOrderShipping();

            Dal.OrderShippingParcel parcel = new Dal.OrderShippingParcel()
            {
                OrderShipping = os,
                Size = Int32.Parse(ddlSize.SelectedValue)
            };

            int parcelId = Dal.DbHelper.Orders.SetOrderShippingParcel(parcel);

            Dal.OrderShipping osNew = Dal.DbHelper.Orders.GetOrderShippingParcel(parcelId).OrderShipping;

            Bll.InpostHelper ih = new Bll.InpostHelper();

            ih.ExportInpostBatch(null, osNew, UserName);

            DisplayMessage("Nowa etykieta zostanie utworzona i wyświetlona wkrótce");

            if (Saved != null)
                Saved();
            pnInpost.Visible = false;
        }

        public Dal.OrderShipping GetOrderShipping()
        {
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

            decimal? cod = null;
            if (order.AmountBalance.Value < 0)
                cod = Math.Abs(order.AmountBalance.Value);
            return new Dal.OrderShipping()
            {
                COD = cod,
                InsertDate = DateTime.Now,
                InsertUser = UserName,
                OrderId = OrderId,
                OrderShippingStatusId = (int)Dal.Helper.OrderShippingStatus.ReadyToCreate,
                SendFromExternalWerehouse = false,
                ServicePoint =          order.OrderShipping.ServicePoint,
                ServicePointSender =    order.OrderShipping.ServicePointSender,
                ShippingCompanyId = order.OrderShipping.ShippingCompanyId,
                ShippingServiceModeId = order.OrderShipping.ShippingServiceModeId,
                ShippingServiceTypeId = order.OrderShipping.ShippingServiceTypeId
            };
        }

        internal void BindControl(int shippingServiceModeId)
        {
            pnInpost.Visible = pnCourier.Visible = pnExternal.Visible = false;
 

            switch(shippingServiceModeId)
            {
                case (int)Dal.Helper.ShippingServiceMode.Courier:
                    BindParcels();
                    pnCourier.Visible = true;
                    break;
                case (int)Dal.Helper.ShippingServiceMode.Point:
                    BindInpostParcel();
                    pnInpost.Visible = true;
                    break;
                case (int)Dal.Helper.ShippingServiceMode.Showroom:

                    break;
                case (int)Dal.Helper.ShippingServiceMode.ExternalShipping:
                    BindExternalParcel();
                    pnExternal.Visible = true;
                    break;

                case (int)Dal.Helper.ShippingServiceMode.CourierInPost:
                    BindParcels();
                    pnCourier.Visible = true;
                    break;

            }
        }
        internal void BindControlFromOrder()
        { 
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
            if (order.OrderShipping == null)
                return;

            BindControl(order.OrderShipping.ShippingServiceModeId);
        }
        internal void BindControlFromOrder(Dal.Helper.ShippingServiceMode mode)
        {
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
            if (order.OrderShipping == null)
                return;

            BindControl((int)mode);
        }
        private void BindExternalParcel()
        {
            string text = null;

            if (ViewState["parcels"] != null)
                text = ViewState["parcels"].ToString();


            var json_serializer = new JavaScriptSerializer();
            List<Parcel> parcels = new List<Parcel>();

            if (text != null)

                parcels = json_serializer.Deserialize<List<Parcel>>(text);


            if (parcels.Count() == 1)
                txbTrackingNumber.Text = parcels[0].TrackingNumber;


            ViewState["parcels"] = Bll.RESTHelper.ToJson(parcels);
        }
        private void BindInpostParcel()
        {
            string text = null;

            if (ViewState["parcels"] != null)
                text = ViewState["parcels"].ToString();


            var json_serializer = new JavaScriptSerializer();
            List<Parcel> parcels =new List<Parcel>();

            if (text != null)
            
                parcels = json_serializer.Deserialize<List<Parcel>>(text);


            if (parcels.Count() == 1 && parcels[0].Size.HasValue)
                ddlSize.SelectedValue = parcels[0].Size.ToString();

            
            ViewState["parcels"] = Bll.RESTHelper.ToJson(parcels);
        }
        private void BindParcels()
        {
            string text = null;

            if (ViewState["parcels"] != null)
                 text = ViewState["parcels"].ToString(); 


            var json_serializer = new JavaScriptSerializer();
            List<Parcel> parcels;

            if (text == null)
            {
                parcels = new List<Parcel>();
                parcels.Add(new Parcel() { Weight = 5 });
            }
            else
                parcels = json_serializer.Deserialize<List<Parcel>>(text);


           
        
            order = Dal.DbHelper.Orders.GetOrder(OrderId);

            ViewState["parcels"] = Bll.RESTHelper.ToJson(parcels);


            rpParcels.DataSource = parcels;
            rpParcels.DataBind();
        }

        public List<Dal.OrderShippingParcel> GetOrderShippingParcels(Dal.OrderShipping os)
        {

 
            List<Dal.OrderShippingParcel> parcels = new List<OrderShippingParcel>();

            if (pnExternal.Visible)
            {
                var p = new Dal.OrderShippingParcel()
                {
                    ParcelTrackingNumber = txbTrackingNumber.Text.Trim()
                };

                if (os != null)
                    p.OrderShipping = os;
                parcels.Add(p);
            }

            if (pnInpost.Visible)
            {
                var p = new Dal.OrderShippingParcel()
                {
                    Size = Int32.Parse(ddlSize.SelectedValue)
                };

                if (os != null)
                    p.OrderShipping = os;
                parcels.Add(p);
            }
            if(pnCourier.Visible)
            foreach (Parcel x in GetParcels())
            {

                    var p = new Dal.OrderShippingParcel()
                    {
                        DimnesionHeight = x.Height,
                        DimnesionLength = x.Length,
                        DimnesionWidth = x.Width,
                        Weight = x.Weight

                    };

                if (os != null)
                    p.OrderShipping = os;
                parcels.Add(p);
            }


            pnInpost.Visible = pnCourier.Visible = pnExternal.Visible = false;

            return parcels;


        }

        protected void rpParcels_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType== ListItemType.Item || e.Item.ItemType== ListItemType.AlternatingItem)
            {
                TextBox txbWeight = e.Item.FindControl("txbWeight") as TextBox;
                TextBox txbHeight = e.Item.FindControl("txbHeight") as TextBox;
                TextBox txbWidth = e.Item.FindControl("txbWidth") as TextBox;
                TextBox txbLength = e.Item.FindControl("txbLength") as TextBox;
                RequiredFieldValidator rfvHeight = e.Item.FindControl("rfvHeight") as RequiredFieldValidator;
                RequiredFieldValidator rfvWidth = e.Item.FindControl("rfvWidth") as RequiredFieldValidator;
                RequiredFieldValidator rfvLength = e.Item.FindControl("rfvLength") as RequiredFieldValidator;
                RequiredFieldValidator rfWeight = e.Item.FindControl("rfWeight") as RequiredFieldValidator;


                Parcel parcel = e.Item.DataItem as Parcel;

                if (parcel.Weight.HasValue)
                    txbWeight.Text = parcel.Weight.ToString();
                if (parcel.Height.HasValue)
                    txbHeight.Text = parcel.Height.ToString();
                if (parcel.Length.HasValue)
                    txbLength.Text = parcel.Length.ToString();
                if (parcel.Width.HasValue)
                    txbWidth.Text = parcel.Width.ToString();


                rfWeight.Visible = ValidateParcels;

                if(order.ShipmentCountryCode!="PL"  &&  ValidateParcels)
                {
                    rfvHeight.Visible = true;
                    rfvWidth.Visible = true;
                    rfvLength.Visible = true;
                }
            }   
        }

        protected void lbtnParcelAdd_Click(object sender, EventArgs e)
        {
            List<Parcel> parcels = GetParcels();

            parcels.Add(new Parcel());

            order = Dal.DbHelper.Orders.GetOrder(OrderId);
            rpParcels.DataSource = parcels;
            rpParcels.DataBind();
            ViewState["parcels"] = Bll.RESTHelper.ToJson(parcels);

            Refresh?.Invoke();
        }

        private List<Parcel> GetParcels()
        {
            List<Parcel> parcels = new List<Parcel>();

            foreach (RepeaterItem item in rpParcels.Items)
            {
                TextBox txbWeight = item.FindControl("txbWeight") as TextBox;
                TextBox txbHeight = item.FindControl("txbHeight") as TextBox;
                TextBox txbWidth = item.FindControl("txbWidth") as TextBox;
                TextBox txbLength = item.FindControl("txbLength") as TextBox;

                parcels.Add(new Parcel()
                {
                    Weight = GetIntOrNull(txbWeight.Text),
                    Height = GetIntOrNull(txbHeight.Text),
                    Width = GetIntOrNull(txbWidth.Text),
                    Length = GetIntOrNull(txbLength.Text)

                });

            }

            return parcels;
        }

        private decimal? GetIntOrNull(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;
            else
                return Decimal.Parse(text);
        }

        internal void BindControlFromShipping(int orderShippingId)
        {
            List<Dal.OrderShippingParcel> parcels = Dal.DbHelper.Orders.GetOrderShippingParcels(orderShippingId);

            var p = parcels.Select(x => new Parcel()
            {
                Height = x.DimnesionHeight,
                Weight = x.Weight,
                Width = x.DimnesionWidth,
                Length = x.DimnesionLength,
                Size = x.Size,
                TrackingNumber = x.ParcelTrackingNumber
            }).ToList();
            if (p.Count() == 0)
                p.Add(new Parcel() { });

            Dal.OrderShipping os = Dal.DbHelper.Orders.GetOrderShipping(orderShippingId);

            ViewState["parcels"] = Bll.RESTHelper.ToJson(p);
            BindControl(os.ShippingServiceModeId); 
            
        }
    }
}