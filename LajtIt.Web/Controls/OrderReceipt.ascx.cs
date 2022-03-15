using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using LajtIt.Dal;

namespace LajtIt.Web.Controls
{
    public partial class OrderReceiptControl : LajtitControl 
    {

        public delegate void ReloadedEventHandler();
        public event ReloadedEventHandler Reloaded;
        public delegate void ClosedEventHandler();
        public event ClosedEventHandler Closed;
        public delegate void DisabledEventHandler();
        public event DisabledEventHandler Disabled;
        public delegate void EnabledEventHandler();
        public event EnabledEventHandler Enabled;


        public string ValidationGroup
        {
            set
            {
                txbNip.ValidationGroup = txbPrePayment.ValidationGroup = value;
            }
        }
        public int OrderId
        {
            set
            {
                ViewState["OrderId"] = value.ToString();
            }
            get
            {
                return Int32.Parse(ViewState["OrderId"].ToString());
            }
        }
        private int ReceiptTypeId
        {
            get { return Convert.ToInt32(rbReceiptType.SelectedValue); }
        }
 
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
 

       

        private   decimal GetAmount()
        {
            Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
            decimal amount = order.AmountToPay;
            switch (Int32.Parse(rbReceiptType.SelectedValue))
            {
                case 2:
                    amount = Decimal.Parse(txbPrePayment.Text.Trim()); break;
                case 3:

                    List<Dal.OrderReceipt> receipts = Dal.DbHelper.Orders.GetReceipts(OrderId);
                    amount = order.AmountToPay - receipts.Sum(x => x.Amount);
                    break;
            }

            return amount;
        }


        internal void BindReceipt()
        {
            ddlCashRegister.DataSource = Dal.DbHelper.Accounting.GetCashRegisters();
            ddlCashRegister.DataBind();
            if (this.UserSourceTypeId != 0)
                ddlCashRegister.SelectedValue = "1";
            else
                ddlCashRegister.SelectedValue = "2";

            RefreshStatus();


            tmTimer.Enabled = true;
            rbReceiptType.Visible = true;
            Enabled?.Invoke();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

            if(order.Invoice!=null && order.Invoice.Nip!="")
            {
                Disabled?.Invoke();
                Closed?.Invoke();

                DisplayMessage("W przypadku faktury z NIP nie ma możliwości wydrukowania paragonu.");
            }

            ucReceiptOrderGrid.BindReceipts(OrderId);

            List<Dal.OrderReceipt> receipts = Dal.DbHelper.Orders.GetReceipts(OrderId);

            Dal.OrderReceipt receiptWithNip = receipts.Where(x => x.Nip != null).FirstOrDefault();
            if (receiptWithNip != null)
            {
                txbNip.Text = receiptWithNip.Nip;
                txbNip.Enabled = false;
            }

            if (receipts.Count > 0 && receiptWithNip == null)
                txbNip.Enabled = false;

            decimal receiptSum = receipts.Sum(x => x.Amount);


            lblTotal.Text = String.Format("{0:C}", order.AmountToPay);
            lblPayments.Text = String.Format("{0:C}", order.AmountPaid);
            lblReceiptTotal.Text = String.Format("{0:C}", receiptSum);

            //cvPrePayment.ValueToCompare = String.Format("{0}", order.AmountPaid - receiptSum);

           // if (order.AmountPaid < order.AmountToPay)
            //    cvPrePayment.Operator = ValidationCompareOperator.LessThanEqual;


            if (order.AmountPaid == 0  // brak wpłat
                || order.AmountToPay == receiptSum // wartość wpłat równa wartości paragonów
                || (order.AmountBalance.Value < 0 && receiptSum == order.AmountPaid) // jest niedopłata i  zaliczka wystawiona
                )
            {

                rbReceiptType.Visible = false;
                Disabled?.Invoke();
                return;
            }

            if (order.AmountPaid == order.AmountToPay && receiptSum==0)
            {
                rbReceiptType.Items[0].Enabled = true;
                rbReceiptType.Items[0].Selected = true;
                rbReceiptType.Items[1].Enabled = false;
                rbReceiptType.Items[2].Enabled = false;
                return;
            }

            if (order.AmountPaid == order.AmountToPay && receiptSum < order.AmountPaid) // wartość zaliczek rowna wartosci zamowienia
            {
                rbReceiptType.Items[0].Enabled = false;
                rbReceiptType.Items[1].Enabled = false;
                rbReceiptType.Items[2].Enabled = true;
                rbReceiptType.Items[2].Selected = true;
                txbPrePayment.Text = (order.AmountToPay - receiptSum).ToString();
                return;
            }


            rbReceiptType.Visible = true;
            if (order.AmountPaid > 0 && receiptSum > 0)
            {
                rbReceiptType.Items[0].Enabled = false;
                rbReceiptType.Items[1].Enabled = true;
            }



            // jest niedopłata i brak zaliczek
            if (order.AmountBalance.Value < 0 && receiptSum < order.AmountPaid)
            {
                rbReceiptType.Items[0].Enabled = false;
                rbReceiptType.Items[1].Selected = true;
                rbReceiptType.Items[2].Enabled = false;
                txbPrePayment.Text = (order.AmountPaid - receiptSum).ToString();
                //ddlReceiptType_SelectedIndexChanged(null, null);
                txbPrePayment.Enabled = false;
            }

            

            //if (receipts.Where(x=>x.ReceiptTypeId==1).Count()>0 || order.AmountBalance.Value<0)
            //{
            //    rbReceiptType.Items[0].Enabled = false;
            //    rbReceiptType.Items[1].Selected = true;

            //    if (order.AmountBalance == 0)
            //        rbReceiptType.Items[2].Enabled = true;
            //}
            //else
            //{
            //    rbReceiptType.Items[0].Enabled = true;
            //    rbReceiptType.Items[2].Enabled = false;

            //}
        }

        internal void SaveReceipt()
        {
            if (!Page.IsValid)
            {

                Reloaded?.Invoke();
                return;
            }
            if (rbReceiptType.Items[1].Selected && txbPrePayment.Text=="")
            {
                lblPrePayment.Visible = true;
                Reloaded?.Invoke();
                return;
            }
            lblPrePayment.Visible = false;

            decimal amount = GetAmount();

            int result = Bll.OrderHelper. SetReceipt(OrderId, Int32.Parse(ddlCashRegister.SelectedValue), ReceiptTypeId, amount, txbNip.Text, UserName);
           

            List<Dal.OrderPayment> orderPayments = Dal.DbHelper.Orders.GetOrderPayments(OrderId).Where(x => x.Amount > 0).ToList();


            switch (result)
            {
                case -3:
                    DisplayMessage("<h2>Błąd</h2>Paragon już wystawiony");
                    break;
                case -2:
                    DisplayMessage("<h2>Błąd</h2>Brak produktów na paragonie");
                    break;
                case -1:
                    DisplayMessage("<h2>Błąd</h2>Wyliczona kwota paragonów jest mniejsza bądź równa zero.");break;
                default:
                    string xml = Bll.NovitusHelper.GetReceiptXml(result);

                    Dal.DbHelper.Orders.SetReceiptCommand(result, xml);

                    var payments = orderPayments.Where(x => x.Amount == amount && x.AccountingTypeId == null).ToList();

                    Dal.DbHelper.Accounting.SetOrderPaymentsAccounting(payments.Select(x => x.PaymentId).ToArray(), (int)Dal.Helper.OrderPaymentAccoutingType.CashRegister);

                    //object r = Bll.NovitusHelper.SetCommand(xml, ref msg);

                    DisplayMessage("Wysłano paragon do drukarki");
                    break;
            }

            tmTimer.Enabled = false;
         
        }

        protected void ddlReceiptType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if(rbReceiptType.SelectedValue=="2")
            {
                txbPrePayment.Enabled = true;
            }
            else
            {
                txbPrePayment.Enabled = false;
            }

            Reloaded?.Invoke();
        }

        protected void tmTimer_Tick(object sender, EventArgs e)
        {
            RefreshStatus();

            Reloaded?.Invoke();
        }

        private void RefreshStatus()
        {
            Dal.CashRegister cs = Bll.NovitusHelper.IsConnected(Int32.Parse(ddlCashRegister.SelectedValue));
            bool isError = cs.IsError;


      

            if(!cs.IsError && DateTime.Now.AddMinutes(-1) > cs.LastUpdateDate)
            {
                isError = true;

            }
            string status = isError ? "BŁĄD" : "OK";

            string msg = String.Format("Ostatnia aktualizacja: {0:yyyy/MM/dd HH:mm:ss}, status: {1}", cs.LastUpdateDate, status);

         
            lblStatus.Text = msg;

            if (isError)
                lblStatus.ForeColor = Color.Red;
            else
                lblStatus.ForeColor = Color.Green;
        }

        internal void CloseReceipt()
        {
            tmTimer.Enabled = false;
        }

        protected void ddlCashRegister_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshStatus();
            Reloaded?.Invoke();

        }
    }
}