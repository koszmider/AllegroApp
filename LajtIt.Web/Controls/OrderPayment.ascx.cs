using System;
using System.Linq;
using System.Web.UI;
using LajtIt.Bll;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace LajtIt.Web.Controls
{
    public partial class OrderPayment : LajtitControl
    {
        public delegate void SavedEventHandler(object sender, EventArgs e);
        public event SavedEventHandler Saved;

        private int OrderId
        {
            set { ViewState["OrderId"] = value; }
            get { return Convert.ToInt32(ViewState["OrderId"]); }
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

        public bool DisableAdding
        {
            set { lbtnPaymentAdd.Visible = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                BindCompanies();

                txbDateFrom.Text = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                calDateFrom.SelectedDate = DateTime.Now;
            }
            else
                if (txbDateFrom.Text != "")
                calDateFrom.SelectedDate = DateTime.Parse(txbDateFrom.Text);

            if (UserName == "agata" || UserName == "ania")
            {
                txbDateFrom.Enabled = false;
                chbUpdateVAT.Visible = false;
                ddlCompany.Enabled = false;
            }

        }

        private void BindCompanies()
        {

            Dal.OrderHelper oh = new Dal.OrderHelper();


            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

            var c = oh.GetCompanies().Where(x=>x.CompanyId==order.CompanyId).ToList();

            //Dal.Invoice invoice = order.Invoice;
            //if (invoice != null)
            //    c = c.Where(x => x.CompanyId == invoice.CompanyId).ToList();
            //else
            //{
            //    if (this.UserCompanyId != 0)
            //        c = c.Where(x => x.CompanyId == this.UserCompanyId).ToList();
            //}

            ddlCompany.DataSource = c;
            ddlCompany.DataBind();
            BindPaymentTypes();
        }
        protected void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindPaymentTypes();
        }
        protected void lbtnMoveOut_Click(object sender, EventArgs e)
        {
            int paymentId = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetOrderPaymentMoveOut(paymentId, UserName);

            gvOrderPayments.EditIndex = -1;
            BindPayments(OrderId, AllowEdit);
        }

        protected void gvOrderPayments_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var r = e.Row.DataItem as Dal.OrderPayment;

                LinkButton lbtnMoveOut = e.Row.FindControl("lbtnMoveOut") as LinkButton;
                Label lblAccoutingType = e.Row.FindControl("lblAccoutingType") as Label;
         

              // var amountMovedOut = r.GetType().GetProperty("AmountMovedOut").GetValue(r, null);
              //  bool canMoveOut =  Convert.ToBoolean(r.GetType().GetProperty("CanMoveOut").GetValue(r, null));

                lbtnMoveOut.Visible = r.OrderPaymentType.CanMoveOut && r.AmountMovedOut == null;
                lbtnMoveOut.CommandArgument = r.GetType().GetProperty("PaymentId").GetValue(r, null).ToString();
                DateTime paymentDate = Convert.ToDateTime(r.GetType().GetProperty("InsertDate").GetValue(r, null));

                if ((r.OrderPaymentType.CanMoveOut && r.AmountMovedOut != null) || (UserName=="agata" && DateTime.Now.Date != paymentDate.Date))
                    e.Row.Cells[0].Controls.Clear();



                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlAccoutingType = e.Row.FindControl("ddlAccoutingType") as DropDownList;

                    ddlAccoutingType.DataSource = LajtIt.Dal.DbHelper.Accounting.GetAccoutingTypes();
                    ddlAccoutingType.DataBind();

                    if (r.AccountingTypeId.HasValue)
                        ddlAccoutingType.SelectedValue = r.AccountingTypeId.ToString();
                }
                else
                {
                    if (r.OrderPaymentAccountingType != null)
                        lblAccoutingType.Text = r.OrderPaymentAccountingType.Name;
                    else
                    {
                        lblAccoutingType.Text = "brak";
                    }
                }
            }

        }
        private void BindPaymentTypes()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            var p = oh.GetOrderPaymentTypes().Where(x => x.CompanyId ==
                Convert.ToInt32(ddlCompany.SelectedValue)
                && x.IsActive)
                .OrderBy(x => x.PaymentOrderId)
                .Select(x =>
                new
                {
                    PaymentTypeId = x.PaymentTypeId,
                    Name = String.Format("{0} - {1}", x.FriendlyInvoiceName, x.Name)

                }

                );

            if (this.UserSourceTypeId == 8)
            {
                int[] paymentsId = new int[] { 18, 19, 4 };
                p = p.Where(x => paymentsId.Contains(x.PaymentTypeId)).ToList();

            }


            ddlOrderPaymentTypes.DataSource = p;
            ddlOrderPaymentTypes.DataBind();

            int idx = ddlOrderPaymentTypes.Items.IndexOf(ddlOrderPaymentTypes.Items.FindByValue("13"));
            if (idx != -1)
                ddlOrderPaymentTypes.SelectedIndex = idx;
        }
        protected void gvOrderPayments_OnRowEditing(object sender, GridViewEditEventArgs e)
        {

            gvOrderPayments.EditIndex = e.NewEditIndex;
            BindPayments(OrderId, AllowEdit);
        }
        protected void gvOrderPayments_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            gvOrderPayments.EditIndex = -1;
            BindPayments(OrderId, AllowEdit);
        }
        protected void gvOrderPayments_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow row = gvOrderPayments.Rows[e.RowIndex];
            int paymentId = Convert.ToInt32(row.Cells[7].Text);
            decimal amount = Convert.ToDecimal((row.Cells[4].Controls[0] as TextBox).Text);
            string comment = (row.Cells[5].Controls[0] as TextBox).Text;
            bool? notForEvidence = null;

            DateTime date = Convert.ToDateTime((row.Cells[2].Controls[0] as TextBox).Text);
            DropDownList ddlAccoutingType = row.FindControl("ddlAccoutingType") as DropDownList;

            int? accoutingTypeId = null;
            if (ddlAccoutingType.SelectedIndex != 0)
                accoutingTypeId = Int32.Parse(ddlAccoutingType.SelectedValue);

            //if ((row.Cells[6].Controls[0] as CheckBox).Checked)
            //    notForEvidence = true;
            Dal.OrderPayment op = new Dal.OrderPayment()
            {
                Amount = amount,
                Comment = comment,
                OrderId = OrderId,
                PaymentId = paymentId,
                InsertUser = UserName,
                NotForEvidence = notForEvidence,
                InsertDate = date,
                AccountingTypeId = accoutingTypeId
            };
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetOrderPaymentUpdate(op, UserName);
            gvOrderPayments.EditIndex = -1;
            BindPayments(OrderId, AllowEdit);
            Saved?.Invoke(this, e);
        }
        protected void lbtnPaymentAdd_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
            pPayments.Visible = true;
            if (order.AmountBalance.Value <= 0)
                txbAmount.Text = Math.Abs(order.AmountBalance.Value).ToString();
        }
        protected void btnProductSave_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            if (chbUpdateVAT.Checked)
            {
                List<Dal.OrderProductsView> products = oh.GetOrderProducts(OrderId);

                if (products.Select(x => x.VAT).Distinct().Count() > 1)
                {
                    DisplayMessage(@"Zaznaczono aktualizację VAT dla produktów. Ponieważ istnieją różne stawki VAT, nie można dokonać automatycznej aktualizacji. 
Odznacz tę opcję a następnie dokonaj ręcznej aktualizacji VAT dla każdego z produktów.<br><br>Płatność nie została zapisana.");
                    return;

                }

                oh.SetOrderProductsVAT(OrderId, Convert.ToInt32(ddlOrderPaymentTypes.SelectedValue));
            }

            Dal.OrderPaymentType op = oh.GetOrderPaymentTypes()
                .Where(x => x.PaymentTypeId == Convert.ToInt32(ddlOrderPaymentTypes.SelectedValue))
       
                .FirstOrDefault();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

            if(op.AllowNegative && order.OrderStatusId != (int)Dal.Helper.OrderStatus.Complaint )
            {

                DisplayMessage(@"Opcja zwortu środków jest możliwa tylko w statusie zamówienia 'Reklamacja'");
                return;

            }

            Dal.OrderPayment payment;
            if (Convert.ToInt32(ddlOrderPaymentTypes.SelectedValue) == 27 || Convert.ToInt32(ddlOrderPaymentTypes.SelectedValue) == 28)
            {
                payment = new Dal.OrderPayment()
                {
                    Amount = Convert.ToDecimal(txbAmount.Text),
                    ExternalPaymentId = txbExternalId.Text.Trim(),
                    InsertDate = calDateFrom.SelectedDate.Value.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second),// DateTime.Now,
                    OrderId = OrderId,
                    PaymentTypeId = Convert.ToInt32(ddlOrderPaymentTypes.SelectedValue),
                    InsertUser = UserName,
                    CurrencyRate = 1,
                    AmountCurrency = Convert.ToDecimal(txbAmount.Text),
                    CurrencyCode = "PLN",
                    AccountingTypeId = (int)Dal.Helper.OrderPaymentAccoutingType.Evidence

                };
            }
            else
            {
                payment = new Dal.OrderPayment()
                {
                    Amount = Convert.ToDecimal(txbAmount.Text),
                    ExternalPaymentId = txbExternalId.Text.Trim(),
                    InsertDate = calDateFrom.SelectedDate.Value.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second),// DateTime.Now,
                    OrderId = OrderId,
                    PaymentTypeId = Convert.ToInt32(ddlOrderPaymentTypes.SelectedValue),
                    InsertUser = UserName,
                    CurrencyRate = 1,
                    AmountCurrency = Convert.ToDecimal(txbAmount.Text),
                    CurrencyCode = "PLN"

                };
            }
            string actingUser = "";
            oh.SetOrderPayment(payment, actingUser);
            BindPayments(OrderId, AllowEdit);

            //InvoiceHelper ih = new InvoiceHelper();
            //if (ih.CreateInvoiceNumber(OrderId, true))
            //{
            //    DisplayMessage("Wpłata została zapisana. Faktura została utworzona");
            //}
            DisplayMessage("Wpłata została zapisana. Utwórz fakturę jeśli jest potrzebna");
            pPayments.Visible = false;

            if (Saved != null)
                Saved(this, e);
        }

        protected void lnbPaymentCancel_Click(object sender, EventArgs e)
        {

            pPayments.Visible = false;
        }
        public void BindPayments(int orderId, bool allowEdit)
        {
            OrderId = orderId;
            AllowEdit = allowEdit;



            var o = Dal.DbHelper.Orders.GetOrderPayments(orderId);
                //.Select(x => new
                //{
                //    PaymentType = x.OrderPaymentType.Name,
                //    InsertDate = x.InsertDate,
                //    Amount = x.Amount,
                //    Comment = x.Comment,
                //    PaymentId = x.PaymentId,
                //    InsertUser = x.InsertUser,
                //    NotForEvidence = (x.NotForEvidence.HasValue && x.NotForEvidence.Value == true) ? true : false,
                //    AmountMovedOut = x.AmountMovedOut,
                //    CanMoveOut = x.OrderPaymentType.CanMoveOut
                //}).ToList();

            gvOrderPayments.DataSource = o;
            gvOrderPayments.DataBind();

            // ddlOrderPaymentTypes.DataSource = oh.GetOrderPaymentTypes().Where(x => x.IsActive).OrderBy(x => x.PaymentOrderId).ToList();
            // ddlOrderPaymentTypes.DataBind();

            // ddlOrderPaymentTypes.Items.Insert(0, "");
        }
    }
}