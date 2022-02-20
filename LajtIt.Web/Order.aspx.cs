using System;
using System.Web.UI;
using LajtIt.Bll;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LajtIt.Dal;
using System.Text;

namespace LajtIt.Web
{
    [Developer("1b0b8280-f455-466e-83c2-229b46bdab6b")]
    public partial class AllegroOrder : LajtitPage
    {
        private int OrderId { get { return Convert.ToInt32(Request.QueryString["id"].ToString()); } }

  
     
        protected void Page_Load(object sender, EventArgs e)
        {
            chbInvoice.Attributes.Add("onclick", "if(! info(this)) return false;");
            //ucProducts.Added += ProductAdded;
            //ucProducts.Canceled += ProductCancelled; 
            ucOrderPayment.Saved += PaymentAdded;
            ucItemOrderGrid.Saved += Saved;
            ucOrderStatusHistory.SaveOptions += SaveOptions;
            ucOrderStatusHistory.Saved += StatusSaved;
            ucOrderShipping.Saved += ShippingSaved;
            ucOrderStatusHistory.GetSelectedProducts += SelectedProductIds;
            ucOrderReceipt.Reloaded += OrderReceiptReloaded;
            ucOrderReceipt.Closed += OrderReceiptClosed;
            ucOrderReceipt.Disabled += OrderReceiptDisabled;
            ucOrderReceipt.Enabled += OrderReceiptEnabled;
            ucOrderShipping.OrderId = OrderId;

            if (!Page.IsPostBack)
            {
                BindCountries();
                BindOrder();
                BindOtherOrders();
                hlOrder.NavigateUrl = String.Format(hlOrder.NavigateUrl, OrderId);
                hlOrder.Visible = HasActionAccess(Guid.Parse("ee6b7ae7-9f2f-4be8-bbf6-a4f6e085d3c8"));
            }
        }

        private void BindCountries()
        {
            List<Dal.Country> countries = Dal.DbHelper.Orders.GetCountries();

            ddlShipmentCountry.DataSource = countries;
            ddlShipmentCountry.DataBind();
        }

        public void OrderReceiptReloaded()
        {
            mpeReceipt.Show();
        }
        public void OrderReceiptClosed()
        {
            mpeReceipt.Hide();
        }
        public void OrderReceiptDisabled()
        {
            btnSaveReceipt.Visible = false;
        }
        public void OrderReceiptEnabled()
        {
            btnSaveReceipt.Visible = true;
        }
        private void BindOtherOrders()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            int ordersCount = oh.GetOrders(litEmail.Text.Trim());

            if (ordersCount - 1 > 0)
            {
                hlOrders.Text = String.Format("Liczba zamówień: {0}", ordersCount);
                hlOrders.NavigateUrl = String.Format("Orders.aspx?email={0}", litEmail.Text.Trim());

                hlOrders.Visible = true;
            }
        }
        protected void chblSuppliers_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindOrder();
        }
        protected void btnOrderPrint_Click(object sender, EventArgs e)
        {
            int[] productIds = ucItemOrderGrid.GetSelectedProductIds();

            if (productIds.Length == 0)
            {
                DisplayMessage("Zaznacz produkty do wydruku");
                return;
            }


            Bll.OrderHelper oh = new Bll.OrderHelper();
        

            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));
            Dal.OrderHelper o = new Dal.OrderHelper();



            List<Dal.OrderProductsView> orderProducts = o.GetOrderProducts(OrderId)
                .Where(x => productIds.Contains(x.OrderProductId) && x.IsOrderProduct==1).ToList();

             string fileName =pdf.CreateOrderDocument(OrderId, orderProducts);


             string contentType = contentType = "Application/pdf";

             Response.ContentType = contentType;
             Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

             //Write the file directly to the HTTP content output stream.
             Response.WriteFile(fileName);
             Response.End();


        }
        protected void btnPrePayment_Click(object sender, EventArgs e)
        {
            Bll.OrderHelper oh = new Bll.OrderHelper();
            if (!oh.CreateParagon(OrderId))
            {
                DisplayMessage("Nie można wydrukować paragonu, spróbuj wydrukować fakturę");
                return;
            }


            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));
            Dal.OrderHelper o = new Dal.OrderHelper();
            Dal.Company company = o.GetCompanies().Where(x => x.CompanyId == 1).FirstOrDefault();

            string fileName = pdf.CreatePrePayment(OrderId, company);


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();

        }
        protected void btnInvoiceCreate_Click(object sender, EventArgs e)
        {
            txbInvoiceCompanyName.Text = txbShipmentFirstName.Text.Trim() + " " + txbShipmentLastName.Text.Trim();
            txbInvoiceAddress.Text = txbShipmentAddress.Text.Trim();
            txbInvoiceCity.Text = txbShipmentCity.Text.Trim();
            txbInvoicePostCode.Text = txbShipmentPostCode.Text.Trim();
            
            chbInvoice.Checked = true;
            SetEditInvoiceButtons(true, true);
            chbInvoice_Changed(null, null);
        }
        protected void btnInvoiceGet_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Invoice invoice = Dal.DbHelper.Orders.GetOrder(OrderId).Invoice;
            Bll.InvoiceHelper ih = new InvoiceHelper();

            if (invoice != null && invoice.IsLocked.HasValue && invoice.IsLocked.Value)
                ih.SendFileToOutput(Server.MapPath(String.Format("/Files/Invoices/{0}", invoice.InvoiceFileName)));
            else
                CreateInvoice();


        }

        protected void btnInvoiceCorrection_Click(object sender, EventArgs e)
        {


            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Invoice invoice = Dal.DbHelper.Orders.GetOrder(OrderId).Invoice;
            CreateInvoiceCorrection();

            Response.Redirect(String.Format("InvoiceCorrection.aspx?id={0}", OrderId));
        }

        private void CreateInvoiceCorrection()
        {
            Bll.InvoiceHelper ih = new InvoiceHelper();

            string fileName = "";
            if (ih.InvoiceCorrectionLocked(OrderId, out fileName))
            {
                ih.SendFileToOutput(Server.MapPath(String.Format("/Files/Invoices/{0}", fileName)));
                return;
            }
            if (!ih.CreateInvoiceCorrection(OrderId))
            {
                DisplayMessage("Nie można wydrukować faktury. Sprawdź czy wszystkie dane są wypełnione oraz czy produkty z katalogu zostały przypisane.");
                return;
            }
            //ih.ImagesDirectory = Server.MapPath("/Images");
            //ih.FilesDirectory = Server.MapPath("/Files");

            //string fileName = ih.GetPdfInvoiceCorrection(OrderId);


            //ih.SendFileToOutput(fileName);
            //DisplayMessage("OK");

        }
        private void CreateInvoice()
        {
            Bll.InvoiceHelper ih = new InvoiceHelper();

            if (!ih.CreateInvoice(OrderId, false))
            {
                DisplayMessage("Nie można wydrukować faktury. Sprawdź czy wszystkie dane są wypełnione oraz czy produkty z katalogu zostały przypisane.");
                return;
            }
            ih.ImagesDirectory = Server.MapPath("/Images");
            ih.FilesDirectory = Server.MapPath("/Files");

            string fileName = ih.GetPdfInvoice(OrderId);


            ih.SendFileToOutput(fileName);
        }

       
        protected void btnParagonGet_Click(object sender, EventArgs e)
        {
            Bll.OrderHelper oh = new Bll.OrderHelper();
            if (!oh.CreateParagon(OrderId))
            {
                DisplayMessage("Nie można wydrukować paragonu, spróbuj wydrukować fakturę");
                return;
            }
             
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));
            Dal.OrderHelper o = new Dal.OrderHelper();

            string fileName = pdf.CreateParagon(OrderId);


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();


        }
        protected void lbtnChangeDateCancel_Click(object sender, EventArgs e)
        {
            litDate.Visible = true;
            pChangeDate.Visible = false;
            lbtnChangeDate.Visible = true;
        }
        protected void lbtnChangeDateSave_Click(object sender, EventArgs e)
        {
            Dal.Order order = new Dal.Order();
            order.OrderId = OrderId;
            order.InsertDate = DateTime.Parse(txbChangeDate.Text);
            order.ShopId = Int32.Parse(ddlShop.SelectedValue);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetOrderDateAndSource(order, UserName);
            litDate.Visible = true;
            lbtnChangeDate.Visible = true;
            pChangeDate.Visible = false;

            BindOrder();
        }
        protected void lbtnChangeDate_Click(object sender, EventArgs e)
        {
            litDate.Visible = false;
            pChangeDate.Visible = true;
            lbtnChangeDate.Visible = false;
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<Dal.Shop> shops = Dal.DbHelper.Shop.GetShops();

            if (this.UserShopId!=0)
                shops = shops.Where(x => x.ShopId == this.UserShopId).ToList();

            ddlShop.DataSource = shops;
            ddlShop.DataBind();
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
            ddlShop.SelectedIndex = ddlShop.Items.IndexOf(ddlShop.Items.FindByValue( order.ShopId.ToString()));

        }
        protected void lbtnOrderNew_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh1 = new Dal.OrderHelper();

            List<Dal.OrderProductsView> orderProducts = oh1.GetOrderProducts(OrderId).Where(x=>x.IsOrderProduct==1).ToList();

            int[] productIds = ucItemOrderGrid.GetSelectedProductIds();

            if(productIds.Length==0 || orderProducts.Count()<=1)
            {
                DisplayMessage("Zaznacz produkty do przeniesienia. Nie można przenieść jedynego produktu w zamówieniu.");
                return;
            }
            Bll.OrderHelper oh = new Bll.OrderHelper();
         


            bool canBeMoved = oh.GetCanMoveProductsFromOrder(OrderId, productIds);

            if(!canBeMoved)
            {
                DisplayMessage("Produkty oznaczone jako Wydane nie mogą być przenoszone do nowego zamówienia i/lub status zamówienia uniemożliwia przeniesienie");
                return;
            }

            int newOrderId = OrderNew.CreateNewOrder(OrderId, this.UserShopId, true, true);
            oh.SetOrderOwnerFromOtherOrder(newOrderId, OrderId);
            oh.SerOrderProductsToNewOrder(OrderId, newOrderId, productIds, UserName);
            oh.SerOrderPaymentToNewOrder(OrderId, newOrderId,  UserName);
            Response.Redirect(String.Format("Order.aspx?id={0}", newOrderId));


        }
        protected void btnOrderNew_Click(object sender, EventArgs e)
        {
            int newOrderId = OrderNew.CreateNewOrder(OrderId, this.UserShopId, false, false);
            Response.Redirect(String.Format("Order.aspx?id={0}", newOrderId));
        }
        //protected void btnChangeStatus_Click(object sender, EventArgs e)
        //{


        //}


        #region Shipment Address

        protected void btnEditShipmentAddress_Click(object sender, EventArgs e)
        {
            SetEditShipmentButtons(true);
        }
        private void SetEditShipmentButtons(bool edit)
        {
            btnEditShipmentAddress.Visible = !edit;
            btnEditShipmentCancel.Visible = edit;
            btnEditShipmentSave.Visible = edit;
            txbShipmentAddress.Enabled = edit;
            txbShipmentCity.Enabled = edit;
            txbShipmentPostCode.Enabled = edit;
            ddlShipmentCountry.Enabled = edit;
            txbShipmentCompanyName.Enabled = edit;
            txbShipmentFirstName.Enabled = edit;
            txbShipmentLastName.Enabled = edit;
            txbShipmentPhone.Enabled = edit;
            txbShipmentPhone2.Enabled = edit;
            txbEmail.Enabled = edit; 
        }
        protected void btnEditShipmentCancel_Click(object sender, EventArgs e)
        {
            SetEditShipmentButtons(false);
            BindOrder();
        }
        protected void btnEditShipmentSave_Click(object sender, EventArgs e)
        {

            SetEditShipmentButtons(false);
            Dal.Order order = new Dal.Order()
            {
                OrderId = OrderId,
                ShipmentAddress = txbShipmentAddress.Text.Trim(),
                ShipmentCity = txbShipmentCity.Text.Trim(),
                ShipmentCompanyName = txbShipmentCompanyName.Text.Trim(),
                ShipmentFirstName = txbShipmentFirstName.Text.Trim(),
                ShipmentLastName = txbShipmentLastName.Text.Trim(),
                ShipmentPostcode = txbShipmentPostCode.Text.Trim(),
                ShipmentCountryCode= ddlShipmentCountry.SelectedValue,
                Phone = txbShipmentPhone.Text.Trim(),
                Phone2 = txbShipmentPhone2.Text.Trim(),
                Email = txbEmail.Text.Trim() 

            };
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetShipmentAddressUpdate(order, UserName);
            BindOrder();

            order = Dal.DbHelper.Orders.GetOrder(OrderId);

            //if (isChanged && order.ShipmentTrackingNumber != null)
                DisplayMessage("UWAGA! Adres został zmieniony. Czy potrzebujesz wygenerować nową etykietę?");

        }
        #endregion
        #region Invoice

        protected void chbInvoice_Changed(object sender, EventArgs e)
        {
            btnEditInvoiceAddress.Visible = chbInvoice.Checked;
            if (!chbInvoice.Checked)
            {
                Dal.OrderHelper oh = new Dal.OrderHelper();
                oh.SetInvoiceDelete(OrderId, UserName);
                BindOrder();
            }
        }
        protected void chbParagon_Changed(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetParagonStatus(OrderId, chbParagon.Checked, UserName);
            DisplayMessage("Zmieniono opcję generowania paragonu");
        }
        protected void btnEditInvoiceAddress_Click(object sender, EventArgs e)
        {
            SetEditInvoiceButtons(true, true);
        }
        private void SetEditInvoiceButtons(bool edit, bool changeButtons)
        {
            if (changeButtons)
            {
                btnEditInvoiceAddress.Visible = !edit;
                btnEditInvoiceCancel.Visible = edit;
                btnEditInvoiceSave.Visible = edit;
            }
            txbInvoiceNIP.Enabled = edit;
            txbInvoiceCompanyName.Enabled = edit;
            txbInvoiceAddress.Enabled = edit;
            txbInvoicePostCode.Enabled = edit;
            txbInvoiceCity.Enabled = edit;
            txbInvoiceComment.Enabled = edit;
            chbInvoice.Enabled = edit;
            //chbInvoiceExcludeFromReport.Visible = edit;
            //ddlCompany.Enabled = edit;
            btnInvoiceCreate.Visible = !chbInvoice.Checked;
            btnInvoiceCorrection.Visible = !edit;
           // if (this.UserCompanyId != 0)
            //    ddlCompany.Items[1].Enabled = false;
        }
        protected void btnEditInvoiceCancel_Click(object sender, EventArgs e)
        {
            SetEditInvoiceButtons(false, true);
            BindOrder();
        }
        protected void btnEditInvoiceSave_Click(object sender, EventArgs e)
        {
            if (txbInvoiceCompanyName.Text.Length > 100)
            {
                DisplayMessage(String.Format("Nazwa firmy może zawierać maksymalnie 100 znaków. Wpisana nazwa ma długość {0} znaków.", txbInvoiceCompanyName.Text.Length));
                return;

            }

            SetEditInvoiceButtons(false, true);

            if (txbInvoiceAddress.Text.Trim() == "" ||
                txbInvoiceCity.Text.Trim() == "" ||
                txbInvoiceCompanyName.Text.Trim() == "" ||
                txbInvoicePostCode.Text.Trim() == "")
            {
                DisplayMessage("Należy wypełnić wszystkie pola dotyczące faktury");
                BindOrder();  
                return;

            }


            Bll.InvoiceHelper ih = new InvoiceHelper();
            Dal.Invoice invoice = new Dal.Invoice()
                        {
                            Address = txbInvoiceAddress.Text.Trim(),
                            City = txbInvoiceCity.Text.Trim(),
                            CompanyName = txbInvoiceCompanyName.Text.Trim(),
                            Nip = txbInvoiceNIP.Text.Trim(),
                            Postcode = txbInvoicePostCode.Text.Trim(),
                            InvoiceDate = DateTime.Now,
                            Email = litEmail.Text.Trim(),
                            IsDeleted = false,
                            IsLocked = false,
                            InvoiceTypeId = 2, // nowy typ faktury
                            CompanyId = Convert.ToInt32(ddlCompany.SelectedValue),
                            Comment = txbInvoiceComment.Text.Trim(),
                            CountryCode=txbInvoiceCountryCode.Text
                            //ExcludeFromInvoiceReport = chbInvoiceExcludeFromReport.Checked
            };


            ih.InsertUpdateInvoiceHeaders(OrderId, invoice, UserName);

            
            BindOrder();        
        }
        #endregion

        #region Product Manager
        private void Saved(object sender, bool amountChanged)
        {
            BindOrder();

            if (amountChanged)
                DisplayMessage("UWAGA! Kwota pobrania się zmieniła. Wykasuj numer przesyłki by wygenerować nową etykietę.");
        }
        private int[] SelectedProductIds()
        {
            return ucItemOrderGrid.GetSelectedProductIds();
        }
        private void StatusSaved(object sender, EventArgs e)
        {
            BindOrder(); 
        }
        private void ShippingSaved(object sender, EventArgs e)
        {
            BindOrder(); 
        }
        private void PaymentAdded(object sender, EventArgs e)
        {
            BindOrder();
        }
        protected void btnInvoiceLock_Click(object sender, EventArgs e)
        {
            try
            {

                Bll.InvoiceHelper ih = new InvoiceHelper();

                string msg = "";

                if (ih.CanLockInvoice(OrderId, ref msg))
                {

                    ih.LockInvoice(OrderId, true);
                    DisplayMessage("Faktura została zablokowana do edycji");
                }
                else
                    DisplayMessage(msg);

                BindOrder();
            }
            catch (Exception ex)
            {
                DisplayMessage(String.Format("Błąd podczas blokowania faktury<br><br>{0}", ex.Message));
            }

        }
 
      
         
        protected void btnProductAdd_Click(object sender, EventArgs e)
        {
            Bll.OrderHelper oh = new Bll.OrderHelper();
        

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);



            List<string> message = new List<string>();


            string productName = Request.Form[txbProductCode.UniqueID];
            string sproductCatalogId = Request.Form[hfProductCatalogId.UniqueID];
            int productCatalogId = 0;

            if (!Int32.TryParse(sproductCatalogId, out productCatalogId))
            {
                DisplayMessage("Produkt nie istnieje");
                return;
            }

            int orderProductId = oh.VerifyProductCode(OrderId, productCatalogId, UserName);
            if (message.Count > 0)
            {
                DisplayMessage(String.Format("Wykryto błędy podczas dodawania produktu: <ul>{0}</ul>",
                    String.Join("<li>- ", message)));
                return;
            }
            //oh.AddProductCatalogToOrder(OrderId, productCatalogId, quantity, UserName);
            BindOrder();
            txbProductCode.Text = "";
       
            bool isChanged = Dal.DbHelper.Orders.GetOrder(OrderId).AmountToPay != order.AmountToPay;


            if (isChanged && order.OrderShipping!=null && order.OrderShipping.COD.HasValue)
                DisplayMessage("UWAGA! Zmiana wartości pobrania. Czy potrzebujesz wygenerować nową etykietę?");
        }

        #endregion
        private Dal.Order BindOrder()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

            if (order.LockOrder.HasValue && order.LockOrder.Value)
            {
                txbProductCode.Enabled = btnProductAdd.Enabled = btnInvoiceCreate.Enabled = imgbCash.Visible = false;
                lblLockInfo.Visible = true;
            }
            else
            {
                txbProductCode.Enabled = btnProductAdd.Enabled = btnInvoiceCreate.Enabled = imgbCash.Visible = true;
                lblLockInfo.Visible = false;

            }


            //if (this.UserShopId != 0)
            //{
            //    int[] acceptedStatuses = new int[] { (int)Dal.Helper.OrderStatus.WaitingForDelivery, (int)Dal.Helper.OrderStatus.ReadyToSend, (int)Dal.Helper.OrderStatus.ClientContact };
            //    int[] acceptedShipping = new int[] { (int)Dal.Helper.ShippingType.SelfPaymentByDelivery, (int)Dal.Helper.ShippingType.SelfDelivery };


           
            //    if (order.ShopId == this.UserShopId || (acceptedStatuses.Contains(order.OrderStatusId) && acceptedShipping.Contains(order.ShippintTypeId)))
            //    {

            //    }
            //    else

            //        Response.Redirect(String.Format("/NoAccess.aspx?err=ERR_ORDER&id={0}", order.OrderId));
            //}

            int[] suppliersIds = chblSuppliers.Items.Cast<ListItem>()
                .Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();

            decimal totalProducts = ucItemOrderGrid.BindProducts(OrderId, order.OrderStatus.AllowEditProducts, suppliersIds);
            chblSuppliers.DataSource = oh.GetSuppliersForOrder(OrderId);
            chblSuppliers.DataBind();

            foreach (ListItem item in chblSuppliers.Items)
                if (suppliersIds.Contains(Convert.ToInt32(item.Value)))
                    item.Selected = true;

            ucOrderPayment.DisableAdding = HasActionAccess(Guid.Parse("f715d6b9-6c63-4a4c-aa3f-2bcd0d4f25be"));
            ucOrderPayment.BindPayments(OrderId, true);
            ucOrderComplaint.BindOrderComplaints(OrderId, true);

            if (order != null)
            {
                revShipmentPostCode.Enabled = order.ShipmentCountryCode == "PL";

                if (!String.IsNullOrEmpty(order.PromoCode) || order.PromoRebate != 0)
                {
                    if (order.PromoCode != null)
                        lblShopInfo.Text = String.Format("Promocja: {0}, rabat: {1:#.##}%", order.PromoCode, order.PromoRebate);
                    else
                        if (order.PromoRebate.HasValue && order.PromoRebate.Value > 0)
                        lblShopInfo.Text = String.Format("Rabat: {0:#.##}%", order.PromoCode, order.PromoRebate);
                }

                //if (order.SourceTypeId == 1)
                //    lbtnChangeDate.Visible = false;
                chbPriority.Checked = order.OrderPriority ?? false;
                chbParagon.Checked = order.ParActive;
                litDate.Text = String.Format("<b>{0}</b>/Nr zam: {1}/{2}", order.Shop.Name, order.ExternalOrderNumber, Dal.Helper.DateTimeToStringWithDays(order.InsertDate));
                txbChangeDate.Text = String.Format("{0:yyyy-MM-dd HH:mm}", order.InsertDate);
                Dal.AllegroUserByOrderResult auo= oh.GetAllegroUserByOrder(OrderId);
                if (auo != null)
                    litEmail.Text = String.Format("{0} / {1}", auo.UserName, auo.Email);
                else
                    litEmail.Text = order.Email;
              
                if(order.ParNumber!=null)
                    btnParagonGet.Text = String.Format("{0}", order.ParNumber);
                 
                ucOrderStatusHistory.BindStatusHistory(OrderId);

                lblAmountToPay.Text = String.Format("{0:0.00} PLN", order.AmountToPay);
                if (order.ShippingCurrencyCode != "PLN")
                    lblAmountToPayCurrency.Text = String.Format("{0:0.00} {1}", order.AmountToPayCurrency, order.ShippingCurrencyCode);
                lblAmountPaid.Text = String.Format("{0:0.00} PLN", order.AmountPaid);
                lblAmountBalance.Visible = order.AmountBalance.HasValue;
                if (order.AmountBalance.HasValue)
                {
                    if (order.AmountBalance.Value < 0)
                    {
                        lblAmountBalance.Text = String.Format("Niedopłata: {0:0.00}  PLN", order.AmountBalance);
                        lblAmountBalance.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblAmountBalance.Text = String.Format("Nadpłata: {0:0.00} PLN", order.AmountBalance);
                        lblAmountBalance.ForeColor = System.Drawing.Color.Green;
                    }
                } 

                txbShipmentAddress.Text = order.ShipmentAddress;
                txbShipmentCompanyName.Text = order.ShipmentCompanyName;
                txbShipmentFirstName.Text = order.ShipmentFirstName;
                txbShipmentLastName.Text = order.ShipmentLastName;
                txbShipmentPostCode.Text = order.ShipmentPostcode;
                ddlShipmentCountry.SelectedValue = order.ShipmentCountryCode;
                imgCountryCode.ImageUrl = String.Format("/images/flags/{0}.png", order.ShipmentCountryCode);
                txbShipmentCity.Text = order.ShipmentCity;
                txbShipmentAddressFull.Text = String.Format("{0}\n{1} {2}\n{3}\n{4} {5}, {7}\ntel: {6}",
                    order.ShipmentCompanyName, order.ShipmentFirstName, order.ShipmentLastName, order.ShipmentAddress, order.ShipmentPostcode, order.ShipmentCity, order.Phone,order.ShipmentCountryCode).Trim();
                txbShipmentPhone.Text = order.Phone;
                txbShipmentPhone2.Text = order.Phone2;
                txbEmail.Text = order.Email;
                ucOrderStatusHistory.BindStatuses(order.OrderStatus);
                if (order.DeliveryDate.HasValue)
                    txbDeliveryDate.Text = order.DeliveryDate.Value.ToString("yyyy-MM-dd");

            

                ArrangeInvoice(order);
            }

            lbtnChangeDate.Visible = HasActionAccess(Guid.Parse("b2ece56a-e78d-4ff5-afd6-44b4382a48e7"));
            BindDeliveryInfo(order);
            BindOrderStatusDependentControls(order.OrderStatus);

            return order;
        }

        private void BindDeliveryInfo(Dal.Order order)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.OrderDeliveryTimeFnResult dt = oh.GetOrderDeliveryTime(OrderId);
            if(dt!=null)
            { 
            if (dt.StartingDate.HasValue)
                lblDeliveryInfo.Text = String.Format("{0} dni od {1:dd.MM.yyyy HH:mm}", (order.DeliveryDays??dt.NumberOfDays), dt.StartingDate);
            else
                lblDeliveryInfo.Text = String.Format("{0} dni", (order.DeliveryDays ?? dt.NumberOfDays));
        }
        }

        private void ArrangeInvoice(Dal.Order order)
        {
            if (order.Invoice == null)
            {
                txbInvoiceCountryCode.Text = "";
                txbInvoiceAddress.Text = "";
                txbInvoiceAddressFull.Text = "";
                txbInvoiceCity.Text = "";
                txbInvoiceCompanyName.Text = "";
                txbInvoiceNIP.Text = "";
                txbInvoicePostCode.Text = "";
                chbInvoice.Checked = false;
                btnEditInvoiceAddress.Visible = false;
                btnInvoiceLock.Visible = false;
                btnInvoiceGet.Visible = false;
                txbInvoiceComment.Text = "";
                //chbInvoiceExcludeFromReport.Checked = false;
                lbtnUnlock.Visible = false;
                ddlCompany.SelectedValue = order.CompanyId.ToString();
                lbtnAccoutingType.Enabled = false;
                return;
            }

            if (order.Invoice.InvoiceTypeId == 1)
            {
                chbInvoice.Enabled = false;
                btnInvoiceGet.Enabled = false;
                btnInvoiceLock.Enabled = false;
                btnEditInvoiceAddress.Visible = false;
                lbtnUnlock.Visible = false;
            }


            if (order.Invoice.AccountingTypeId.HasValue)
                rblAccountingType.SelectedValue = order.Invoice.AccountingTypeId.ToString();

            txbInvoiceCountryCode.Text = order.Invoice.CountryCode;
            txbInvoiceAddress.Text = order.Invoice.Address;
            txbInvoiceAddressFull.Text = String.Format("{0}\n{1}\n{2} {3}\nNIP: {4}",
            order.Invoice.CompanyName, order.Invoice.Address, order.Invoice.Postcode, order.Invoice.City, order.Invoice.Nip);
            txbInvoiceCity.Text = order.Invoice.City;
            txbInvoiceComment.Text = order.Invoice.Comment;
            txbInvoiceCompanyName.Text = order.Invoice.CompanyName;
            txbInvoiceNIP.Text = order.Invoice.Nip;
            txbInvoicePostCode.Text = order.Invoice.Postcode;
            chbInvoice.Checked = true;
            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(order.Invoice.CompanyId.ToString()));
            //chbInvoiceExcludeFromReport.Checked = order.Invoice.ExcludeFromInvoiceReport;
            btnEditInvoiceAddress.Visible = chbInvoice.Checked && order.Invoice.InvoiceTypeId != 1 && (order.Invoice.IsLocked??false) == false;
            btnInvoiceGet.Text = String.Format("Faktura {0}", order.Invoice.InvoiceNumber);
            btnInvoiceCorrection.Visible = order.Invoice.IsLocked.HasValue && order.Invoice.IsLocked.Value;



            Dal.Invoice invoiceCorrection = GetInvoice(order.Invoice.InvoiceCorrectionId);
            if (invoiceCorrection != null)
                btnInvoiceCorrection.Text = String.Format("Korekta {0}", invoiceCorrection.InvoiceNumber);

            if (order.Invoice.IsLocked.HasValue && order.Invoice.IsLocked.Value)
            {
                btnInvoiceLock.Visible = false;
                btnInvoiceGet.ForeColor = System.Drawing.Color.Red;
                lbtnUnlock.Visible = true;
            }
            else
            {
                btnInvoiceLock.Visible = true;
                btnInvoiceGet.Visible = true;
                btnInvoiceGet.ForeColor = System.Drawing.Color.Green;
                lbtnUnlock.Visible = false;
            }
            btnInvoiceCreate.Visible = !chbInvoice.Checked;
            
        }

        private Invoice GetInvoice(int? invoiceCorrectionId)
        {
            if (invoiceCorrectionId.HasValue == false)
                return null;

            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Invoice invoice = oh.GetInvoice(invoiceCorrectionId.Value);

            return invoice;
        }

        private void BindOrderStatusDependentControls(Dal.OrderStatus orderStatus)
        {
            btnEditShipmentAddress.Enabled = orderStatus.AllowEditShipmentAddress;
                 
        }

        //protected void chbInvoiceExcludeFromReport_CheckedChanged(object sender, EventArgs e)
        //{
        //    Bll.InvoiceHelper ih = new InvoiceHelper();
        //    ih.InsertUpdateInvoiceExcludeFromInvoiceReport(OrderId, chbInvoiceExcludeFromReport.Checked, UserName);

        //    DisplayMessage("Zmiany zostały zapisane");
        //}

        protected void imgbCash_Click(object sender, ImageClickEventArgs e)
        {
            ucOrderReceipt.OrderId = OrderId;
            ucOrderReceipt.BindReceipt();
            mpeReceipt.Show();

            //string str = "<table class='cash' ><tr class='header'><td>Kod kasy</td><td>Nazwa produktu</td><td>L.szt.</td><td>Rabat</td><td class='am'>Cena brutto</td><td class='am'>Wartość</td></tr>{0}</table>";

            //Dal.OrderHelper oh = new Dal.OrderHelper();
            //List<Dal.OrderProductsView> products = oh.GetOrderProducts(OrderId).Where(x=>x.Quantity>0).ToList();
            //StringBuilder sb = new StringBuilder();

            //int row= 0;
            //foreach(OrderProductsView p in products)
            //{
            //    string cl = row % 2 == 0 ? "" : " class='alt'";
            //    string s = "<tr {6}><td>{0}</td><td>{1}</td><td>{2}</td><td>{5}</td><td style='text-align:right;'>{3:C}</td><td style='text-align:right;'>{4:C}</td></tr>";

            //    sb.Append(String.Format(s, p.PosnetUniqueId, p.CatalogName, p.Quantity, p.Price, p.Quantity * p.Price.Value * (100.00M - p.Rebate) / 100.00M , p.Rebate, cl));
            //    row++;
            //}
            //sb.Append("Nabijaj na kasie: <ul><li>Kod kasy</li><li>Liczba sztuk</li><li>Jak jest rabat: SHIFT+menu, Rabat procentowy</li><li>Cena jednostkowa (przed rabatem)</li></ul>");

            //DisplayMessage(String.Format(str, sb.ToString()));
        }
        
        private void SaveOptions()
        {

            Dal.Order order = new Order()
            {
                OrderId = OrderId,
                OrderPriority = chbPriority.Checked


            };
            if (txbDeliveryDate.Text != "")
                order.DeliveryDate = Convert.ToDateTime(txbDeliveryDate.Text);

            Dal.OrderHelper oh = new Dal.OrderHelper();

            oh.SetOrderUpdate(order, UserName);
        }
        protected void btnSaveOptions_Click(object sender, EventArgs e)
        {
            SaveOptions();
            DisplayMessage("Zapisano zmiany");

        }

        protected void lbtnOrderLog_Click(object sender, EventArgs e)
        {
            OrderLog.BindLog(OrderId);
        }

       

        protected void lbtnUnlock_Click(object sender, EventArgs e)
        {
            if( this.HasActionAccess(Guid.Parse("d8fa25ef-3c04-43ed-a3fd-eda701f6a1b1")))
            {
                Dal.OrderHelper oh = new Dal.OrderHelper();

                Dal.Invoice invoice = oh.SetOrderInvoiceUnlock(OrderId, UserName);

                if (invoice != null)
                    try
                    {
                        File.SetAttributes(Server.MapPath(String.Format("/Files/Invoices/{0}", invoice.InvoiceFileName)), ~FileAttributes.ReadOnly);

                        File.Delete(Server.MapPath(String.Format("/Files/Invoices/{0}", invoice.InvoiceFileName)));
                        DisplayMessage("Faktura została odblokowana a plik usunięty");
                        ArrangeInvoice(Dal.DbHelper.Orders.GetOrder(OrderId));
                    }
                    catch(Exception ex)
                    {
                        DisplayMessage(ex.Message);
                    }
                else
                    DisplayMessage("Błąd odblokowania. Faktura nie istnieje.");



            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool result = LajtIt.Dal.DbHelper.Orders.SetOrderCompany(OrderId, Int32.Parse(ddlCompany.SelectedValue));


            if (!result)
                DisplayMessage("Nie można zmienić firmy przypisanej do zamówienia. Usuń fakturę i spróbuj ponownie.");
        }

        protected void btnSaveReceipt_Click(object sender, EventArgs e)
        {
            ucOrderReceipt.SaveReceipt();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ucOrderReceipt.CloseReceipt();
            mpeReceipt.Hide();
            mpeAccoutingType.Hide();
        }

        protected void btnAccountingType_Click(object sender, EventArgs e)
        {
            int? accountingTypeId = null;

            if (rblAccountingType.SelectedIndex != 0)
                accountingTypeId = Int32.Parse(rblAccountingType.SelectedValue);

            Dal.DbHelper.Accounting.SetOrderInvoiceAccoutingType(OrderId, accountingTypeId);

            DisplayMessage("Zmieniono sposób rozliczenia faktury");
            mpeAccoutingType.Hide();
        }
    }
}