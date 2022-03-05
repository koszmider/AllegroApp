using LajtIt.Bll;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("36ece789-13d4-4f69-a456-92e7530cffa0")]
    public partial class OrderProcessing : LajtitPage
    {
        private int OrderId { get { return Convert.ToInt32(Request.QueryString["id"].ToString()); } }
        private int count = 0;
        private List<Dal.OrderProduct> subProducts;
        private static int scId;

        protected void Page_Load(object sender, EventArgs e)
        {

            //ucInpostExport.RefreshOrder += RefreshOrder;
            hlOrder.NavigateUrl = String.Format(hlOrder.NavigateUrl, OrderId);
            ucOrderShippingSimple.Saved += ReBindOrder;
            if (!Page.IsPostBack)
            {
                BindOrder();
            }
        }
        private void ReBindOrder()
        {
            lbtnShippingChange.Visible = lbtnShippingModeChange.Visible = lbtnShippingModeChange1.Visible = true;
            BindOrder();
        }
        protected void tmInterval_Tick(object sender, EventArgs e)
        {

            BindOrder();
        }
        public void RefreshOrder(bool enabled)
        {
            tmInterval.Enabled = enabled;
            BindOrder();
        }
        protected void btnSent_Click(object sender, EventArgs e)
        {
             
            Dal.OrderStatusHistory osh = new Dal.OrderStatusHistory()
            {
                Comment = "Wysłano przez stronę obsługi zamówień",
                InsertDate = DateTime.Now,
                InsertUser = UserName,
                OrderStatusId = (int)Dal.Helper.OrderStatus.Sent,
                OrderId = OrderId,
                SendNotification = true
            };
           
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetOrderStatus(osh, null);
            LajtIt.Web.Controls.OrderComment.SendEmailNotification(true, (int)Dal.Helper.OrderStatus.Sent, OrderId, UserName);

            Bll.InvoiceHelper ih = new InvoiceHelper();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
            Dal.Invoice invoice = order.Invoice;

            if (invoice != null)
            {
                if (invoice.InvoiceNumber == null)
                    ih.CreateInvoiceNumber(OrderId, false);
            }

            DisplayMessage("Zamówienie zostało oznaczone jako 'Wysłane'");
            BindOrder();
        }
        //protected void lbtnShippingCancel_Click(object sender, EventArgs e)
        //{
        //    pShipping.Visible = false;
        //}
        //protected void lbtnShippingSave_Click(object sender, EventArgs e)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    string shippingData = null;

        //    shippingData = ucShippingTypesControl.GetShippingData();
        //    oh.SetOrderShippingData(
        //        OrderId,
        //        shippingData,
        //        UserName);
        //    pShipping.Visible = false;

        //    DisplayMessage("Parametry paczki zostały zmienione. Wyegeneruj nową etykietę");
        //}
        protected void lbtnShippingChange_Click(object sender, EventArgs e)
        {
            pShipping.Visible = true;
            lbtnShippingChange.Visible = lbtnShippingModeChange.Visible = lbtnShippingModeChange1.Visible = false;
            
            ucOrderShippingSimple.OrderId = OrderId;
            ucOrderShippingSimple.BindControlFromOrder();
 
        }

        protected void lbtnShippingModeChange1_Click(object sender, EventArgs e)
        {
            scId = 4;
            pShipping.Visible = true;
            lbtnShippingChange.Visible = lbtnShippingModeChange.Visible = lbtnShippingModeChange1.Visible = false;

            ucOrderShippingSimple.OrderId = OrderId;
            ucOrderShippingSimple.BindControlFromOrder(Dal.Helper.ShippingServiceMode.Courier);
            ViewState["ChangeMode"] = Dal.Helper.ShippingServiceMode.Courier;
        }

        protected void lbtnShippingModeChange_Click(object sender, EventArgs e)
        {
            scId = 1;
            pShipping.Visible = true;
            lbtnShippingChange.Visible = lbtnShippingModeChange.Visible = lbtnShippingModeChange1.Visible = false;

            ucOrderShippingSimple.OrderId = OrderId;
            ucOrderShippingSimple.BindControlFromOrder(Dal.Helper.ShippingServiceMode.Courier);
            ViewState["ChangeMode"] = Dal.Helper.ShippingServiceMode.Courier;
        }
        private void BindOrder()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);


            if (order != null)
            {
                Dal.Shop pc = Dal.DbHelper.Shop.GetShop(order.ShopId);
                if (pc != null)
                {
                    lblShopName.Text = pc.Name;
                }

                imgCourier.ImageUrl = String.Format("/images/courier/{0}.png", order.OrderShipping.ShippingCompanyId);

                pShippngChanges.Visible = order.OrderStatusId == (int)Dal.Helper.OrderStatus.Exported;
                //ucInpostExport.Visible = order.ShippingType.ShippingCompanyId == (int)Dal.Helper.ShippingCompany.InPost;
                //lbtnExport.Visible = order.ShippingType.ShippingCompanyId == (int)Dal.Helper.ShippingCompany.Dpd;
                imgCountryCode.ImageUrl = String.Format("/images/flags/{0}.png", order.ShipmentCountryCode);
                if (order.OrderShipping!=null && order.OrderShipping.ShipmentTrackingNumber != null)
                {
                    tmInterval.Enabled = false;
                    pnGenerating.Visible = false;
                    imgbPrint.Visible = true;
                }
                else
                {
                    tmInterval.Enabled = true;
                    pnGenerating.Visible = true;
                    imgbPrint.Visible = false;

                }

                List<Dal.OrderShippingParcel> parcels = Dal.DbHelper.Orders.GetOrderShippingParcels(order.OrderShippingId.Value);

                if (order.OrderShipping.ShippingServiceModeId == (int)Dal.Helper.ShippingServiceMode.Point)
                {
                    lblClientName.Text = String.Format("{0}<br>{1}", order.Email, order.Phone);
                    lbtnShippingModeChange.Visible = true;
                    lbtnShippingModeChange1.Visible = true;

                    if (parcels.Count()>0&&parcels[0] != null)
                    {
                        lblInpost.Text = String.Format("<b>Gabaryt: {0}</b>", Dal.DbHelper.Orders.GetParcelSizeInpostCode(parcels[0].Size.Value));
                    }
                }
                else
                { 
                    lbtnShippingModeChange.Visible = false;
                    lbtnShippingModeChange1.Visible = false;
                    lblClientName.Text = String.Format("{0}\n{1} {2}\n{3}\n{4} {5}\n{6}",
                    order.ShipmentCompanyName, order.ShipmentFirstName, order.ShipmentLastName,
                    order.ShipmentAddress, order.ShipmentPostcode, order.ShipmentCity, order.Phone);
                    lblInpost.Text = String.Format("Liczba paczek: <b>{0}</b>", parcels.Count());


                }
                lblShipment.Text = String.Format("{0} {1}", order.OrderShipping.ShippingCompany.Name, order.OrderShipping.ShippingServiceMode.Name);
                hlTracking.NavigateUrl = String.Format(order.OrderShipping.ShippingCompany.TrackingUrl, order.OrderShipping.ShipmentTrackingNumber);// String.Format("{0:yyyy/MM/dd HH:mm}", order.InsertDate);
                hlTracking.Text = order.OrderShipping.ShipmentTrackingNumber;
                


                imgbPrint.Visible =   !String.IsNullOrEmpty(order.OrderShipping.ShipmentTrackingNumber);


            }
             
            List<Dal.OrderProductsForWarehouseView> orderProducts =
                oh.GetOrderProductsForWarehouse(OrderId);

            subProducts = oh.GetOrderSubProducts(OrderId);

            gvUserOrders.DataSource = orderProducts.Where(x=>x.Quantity>0 && 
            (x.OrderProductStatusId == (int)Dal.Helper.OrderProductStatus.New||
            x.OrderProductStatusId == (int)Dal.Helper.OrderProductStatus.Ready||
            x.OrderProductStatusId == (int)Dal.Helper.OrderProductStatus.Ordered));
            gvUserOrders.DataBind();

            lblOrderStatus.Text = order.OrderStatus.StatusName;
            if (order.OrderStatusId == (int)Dal.Helper.OrderStatus.Exported)
            {
                btnSent.Visible = true;
                lblOrderStatus.Visible = false;
            }
            else
            {
                btnSent.Visible = false;
                lblOrderStatus.Visible = true;
            }

        }
        protected void gvUserOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblProductCatalogName = e.Row.FindControl("lblProductCatalogName") as Label;
                Label lblProductCatalogCode = e.Row.FindControl("lblProductCatalogCode") as Label;
                Label lblSubProducts = e.Row.FindControl("lblSubProducts") as Label;
                Panel pnlComponents = e.Row.FindControl("pnlComponents") as Panel;
                HyperLink hlImage = e.Row.FindControl("hlImage") as HyperLink;
                Image imgImage = e.Row.FindControl("imgImage") as Image;
                DropDownList ddlQuantity = e.Row.FindControl("ddlQuantity") as DropDownList;

                Dal.OrderProductsForWarehouseView orderProduct = e.Row.DataItem as Dal.OrderProductsForWarehouseView;

                CompareValidator cvQuantity = e.Row.FindControl("cvQuantity") as CompareValidator;
                RequiredFieldValidator rvQuantity = e.Row.FindControl("rvQuantity") as RequiredFieldValidator;

                cvQuantity.ErrorMessage = String.Format("Niepoprawna liczba przedmiotów dla: {0}", orderProduct.ProductName);
                rvQuantity.ErrorMessage = String.Format("Niepodano liczby przedmiotów dla: {0}", orderProduct.ProductName);

                ddlQuantity.Items.Add(new ListItem());
                for (int i = 1; i <= orderProduct.Quantity; i++)
                    ddlQuantity.Items.Add(new ListItem(i.ToString()));

                if (lblProductCatalogName != null && orderProduct.ProductCatalogId.HasValue)
                {
                    lblProductCatalogName.Text = orderProduct.CatalogName; 
                        lblProductCatalogCode.Text = orderProduct.Code;


                    var producatalogSubProducts = subProducts.Where(x => x.SubOrderProductId == orderProduct.OrderProductId)
                        .Select(x =>
                            new
                            {
                                ProductCatalogId = x.ProductCatalog,
                                Name = x.ProductName,
                                Quantity = x.Quantity
                            }).ToList();

                    lblSubProducts.Text = "Komponenty:";
                    if (producatalogSubProducts.Count == 0)
                        lblSubProducts.Visible = false;
                    foreach (var pc in producatalogSubProducts)
                    {
                        lblSubProducts.Text = lblSubProducts.Text + "<br>- " + pc.Name + " x" + pc.Quantity.ToString();

                    }


                }
                if ( orderProduct.ImageFullName!=null)
                {
                    e.Row.Style.Add("cursor", "pointer");
                    lblProductCatalogName.Attributes.Add("onclick",
                   String.Format(@"$( '#{1}' ).show( 'slow' );", lblProductCatalogName.ClientID, pnlComponents.ClientID));
                    pnlComponents.Attributes.Add("onclick",
                   String.Format(@"$( '#{1}' ).hide( 'slow' );", lblProductCatalogName.ClientID, pnlComponents.ClientID));
 
                    if (orderProduct.ImageFullName != null)
                    {
                        imgImage.ImageUrl = String.Format("/Images/ProductCatalog/{0}", orderProduct.ImageFullName);
                        hlImage.Visible = true;
                        hlImage.NavigateUrl = String.Format("/ProductCatalog.Preview.aspx?id={0}", orderProduct.ProductCatalogId);

                    }

                    e.Row.Style.Add("color", "red");
                } 
                count += orderProduct.Quantity; 
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            { 
                Literal litQuantityFooter = e.Row.FindControl("litQuantityFooter") as Literal;
                 
                litQuantityFooter.Text = String.Format("{0}", count);

            }
        }
 


        protected void imgbPrint_Click(object sender, ImageClickEventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

 
            if (order.OrderShipping.ShippingCompany == null)
                return;


            if (!Bll.OrderHelper.ExportFile(new string[] { String.Format(@"Shipping\{0}\{1}.pdf", order.OrderShipping.ShippingCompany.Name, order.OrderShipping.ShipmentTrackingNumber) }))
                DisplayMessage("Etykieta nie istnieje");


        }


        protected void btnCourierSave_Click(object sender, EventArgs e)
        {
            Dal.OrderShipping os = ucOrderShippingSimple.GetOrderShipping();

        
            if (ViewState["ChangeMode"] != null)
            {
                //Dal.ShippingCompany sc = Dal.DbHelper.Orders.GetShippingCompanies().Where(x => x.IsDefault).FirstOrDefault();
                Dal.Helper.ShippingServiceMode newMode = (Dal.Helper.ShippingServiceMode)ViewState["ChangeMode"];
                os.ShippingCompanyId = scId;//sc.ShippingCompanyId;
                os.ServicePoint = null;
                os.ServicePointSender = null;
                os.ShippingServiceModeId = (int)newMode;
            }
            List<Dal.OrderShippingParcel> orderShippingParcels = ucOrderShippingSimple.GetOrderShippingParcels(os);


            Dal.DbHelper.Orders.SetOrderShippingParcels(orderShippingParcels);

            Dal.OrderShipping osNew = Dal.DbHelper.Orders.GetOrder(OrderId).OrderShipping;
             
            try
            {

                Bll.OrderHelper.BindTracking(osNew);

                DisplayMessage("Nowa etykieta zostanie utworzona i wyświetlona wkrótce");
 

                ReBindOrder();

            }
            catch (Exception ex)
            {
                DisplayMessage(String.Format("Błąd tworzenia etykiety<br><br>{0}", ex.Message));

            }


        }

    }
}