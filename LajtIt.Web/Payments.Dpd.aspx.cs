using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;

namespace LajtIt.Web
{
    [Developer("71253e50-5065-4820-9ef1-a5f044e90804")]
    public partial class PaymentsDpd : LajtitPage
    {
        decimal total = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindMonths();
                BindCompanies(); 
            }
        }
 

        private void BindCompanies()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            var c = oh.GetCompanies().Where(x => x.IsMyCompany && x.IsActive);
            ddlCompany.DataSource = c;
            ddlCompany.DataBind();
        }
        private void BindMonths()
        {
            List<ListItem> items = new List<ListItem>();
            for (int year = DateTime.Now.Year; year >= 2020; year--)
                for (int month = DateTime.Now.Year == year ? DateTime.Now.Month : 12; month >= 1; month--)
                {
                    ListItem item = new ListItem()
                    {
                        Text = String.Format("{0}/{1}", year, month),
                        Value = String.Format("{0}/{1}/{2}", year, month, 1)
                    };
                    items.Add(item);
                }

            ddlMonth.Items.AddRange(items.ToArray());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<Dal.DpdPaymentsView> payments = Dal.DbHelper.Orders
                .GetDpdPayments(DateTime.Parse(ddlMonth.SelectedValue), Int32.Parse(ddlCompany.SelectedValue));




            //int[] bankAccountTypeIds = chblBankAccountType.Items.Cast<ListItem>().Where(x => x.Selected)
            //    .Select(x => Int32.Parse(x.Value)).ToArray();

            //if (ddlPaymentType.SelectedIndex != 0)
            //    bankAccounts = bankAccounts.Where(x => x.TransferType == ddlPaymentType.SelectedValue).ToList();

            //if (bankAccountTypeIds.Count() > 0)
            //    bankAccounts = bankAccounts.Where(x => x.BankAccountTypeId.HasValue && bankAccountTypeIds.Contains(x.BankAccountTypeId.Value)).ToList();
            if (chbNotAssigned.Checked)
                payments = payments.Where(x => x.PaymentId.HasValue ==false).ToList();
            //  if(!String.IsNullOrEmpty(txbSearch.Text))
            //    bankAccounts = bankAccounts.Where(x => x.ClientName.ToLower().Contains(txbSearch.Text.Trim().ToLower()) 
            //     || x.Comment.ToLower().Contains(txbSearch.Text.Trim().ToLower())).ToList();

            //     if (paymentTypeIds.Count() > 0)
            //         payments = payments.Where(x => paymentTypeIds.Contains(x.PaymentTypeId)).ToList();

            //     int[] accoutingTypeIds = chbAccountingType.Items.Cast<ListItem>().Where(x => x.Selected)
            //.Select(x => Int32.Parse(x.Value)).ToArray();

            //     if (accoutingTypeIds.Count() > 0)
            //         payments = payments.Where(x => x.AccountingTypeId.HasValue &&
            //             accoutingTypeIds.Contains(x.AccountingTypeId.Value)).ToList();


            //     if (chbInvoice1.Checked)
            //         payments = payments.Where(x => x.InvoiceNumber != null).ToList();
            //     if (chbInvoice0.Checked)
            //         payments = payments.Where(x => x.InvoiceNumber == null).ToList();

            gvPayments.DataSource = payments;
            gvPayments.DataBind();


            lblCurrent.Text = String.Format("{0:C}", total);

            //if(chbOrderPaymentType.Items.Count==0)
            //{
            //    chbOrderPaymentType.DataSource = payments.Select(x => new { x.PaymentTypeId, x.PaymentName }).Distinct().ToList();
            //    chbOrderPaymentType.DataBind();
            //}
        }

        protected void gvPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal LitId = e.Row.FindControl("LitId") as Literal;
                LitId.Text = String.Format("{0}.", e.Row.RowIndex + 1);

                Dal.DpdPaymentsView o = e.Row.DataItem as Dal.DpdPaymentsView;
                total += o.DpdAmount;


                ImageButton imgOrderEdit = e.Row.FindControl("imgOrderEdit") as ImageButton;



                if (o.PaymentId.HasValue)
                {
                    HyperLink hlOrder = e.Row.FindControl("hlOrder") as HyperLink;
                    hlOrder.NavigateUrl = String.Format(hlOrder.NavigateUrl, o.OrderId);
                    hlOrder.Text = o.OrderId.ToString();



                    if (o.Amount.Value != o.DpdAmount)
                        e.Row.BackColor = Color.Pink;
                    imgOrderEdit.Visible = false;

                }


                else
                {
                    imgOrderEdit.CommandArgument = o.Id.ToString();
                    imgOrderEdit.Visible = true;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = String.Format("{0:C}", total);
            }
        }

        private void BindDpdPayment(int dpdPaymentId)
        {
            ViewState["dpdPaymentId"] = dpdPaymentId;
            List<Dal.OrderPayment> payments = Dal.DbHelper.Accounting.GetOrderPaymentsForDpd(dpdPaymentId);

            ddlOrderPayments.DataSource = payments
                .OrderBy(x => x.Amount)
                .Select(x => new
                {
                    x.PaymentId,
                    Text = String.Format("Zamówienie {0} - {1:C}", x.OrderId, x.Amount)
                }).ToList();
            ddlOrderPayments.DataBind();



            Dal.DpdPayment ba = Dal.DbHelper.Orders.GetDpdPayment(dpdPaymentId);
            lblBankAccount.Text = String.Format("{0}<br>{1}<br>Kwota: {2:C}", ba.TrackingNumber, ba.PaymentDate, ba.Amount);
        }

  
        protected void imgOrderEdit_Click(object sender, ImageClickEventArgs e)
        {

            BindDpdPayment(Convert.ToInt32(((ImageButton)sender).CommandArgument));

            mpeBankAccount.Show();
        }

        protected void btnOrderPaymentAssign_Click(object sender, EventArgs e)
        {
            int dpdPaymentId = Int32.Parse(ViewState["dpdPaymentId"].ToString());
            int orderPaymentId = Int32.Parse(ddlOrderPayments.SelectedValue);
            Dal.DbHelper.Orders.SetOrderPaymentsForDpdPayment(dpdPaymentId, orderPaymentId);

            mpeBankAccount.Hide();
            DisplayMessage("Przypisano");

            btnSearch_Click(null, null);
        }

        protected void lbtnAutoAssign_Click(object sender, EventArgs e)
        {
            Dal.DbHelper.Accounting.SetDpdPaymentToOrderPayment(DateTime.Parse(ddlMonth.SelectedValue));


            btnSearch_Click(null, null);

            
        }

        protected void lbtnBankStatement_Click(object sender, EventArgs e)
        { 
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            List<Dal.DpdPaymentsView> dpdPayments = Dal.DbHelper.Orders.GetDpdPayments(DateTime.Parse(ddlMonth.SelectedValue),
                Int32.Parse(ddlCompany.SelectedValue));

            string fileName = pdf.DpdPayments(dpdPayments,
                Int32.Parse(ddlCompany.SelectedValue),
                DateTime.Parse(ddlMonth.SelectedValue));


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }
    }
}