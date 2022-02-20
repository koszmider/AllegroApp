using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;

namespace LajtIt.Web
{
    [Developer("a4d21e0e-e3b5-43d3-b6c3-cfd842769967")]
    public partial class Payments : LajtitPage
    {
        decimal total = 0;
        bool hasFullView = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            hasFullView = this.HasActionAccess(Guid.Parse("354d3e46-bc9a-46bf-8a82-74c8d6fcf8fc"));

            if (!Page.IsPostBack)
            {
                BindMonths();
                BindCompanies();
                BindAccoutingTypes();
                BindPaymentTypes();
            } 
            else
            {
                if (txbDateFrom.Text != "")
                    calDateFrom.SelectedDate = DateTime.Parse(txbDateFrom.Text);
                if (txbDateTo.Text != "")
                    calDateTo.SelectedDate = DateTime.Parse(txbDateTo.Text);
            }
        }

        private void BindPaymentTypes()
        {

            List<Dal.OrderPaymentType> payments = Dal.DbHelper.Orders.GetOrderPaymentTypes(DateTime.Parse(ddlMonth.SelectedValue), Int32.Parse(ddlCompany.SelectedValue));
            chbOrderPaymentType.DataSource = payments;
            chbOrderPaymentType.DataBind();
        }

        private void BindAccoutingTypes()
        {
            var a= Dal.DbHelper.Accounting.GetAccoutingTypes();
            chbAccountingType.DataSource = a;
            chbAccountingType.DataBind();

            ddlAccountingType.DataSource = a;
            ddlAccountingType.DataBind();
        }

        private void BindCompanies()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            var c = oh.GetCompanies().Where(x=>x.IsMyCompany && x.IsActive);
            ddlCompany.DataSource = c;
            ddlCompany.DataBind(); 
        }
        private void BindMonths()
        {
            List<ListItem> items = new List<ListItem>();
            for (int year = DateTime.Now.Year; year >= 2010; year--)
                for (int month = DateTime.Now.Year == year ? DateTime.Now.Month : 12; month >= 1; month--)
                {
                    ListItem item = new ListItem()
                    {
                        Text = String.Format("{0}/{1}", year, month),
                        Value = String.Format("{0}/{1}/{2}", year, month, 1)
                    };
                    items.Add(item);
                }

            

            if(hasFullView)
                ddlMonth.Items.AddRange(items.ToArray());
            else
                ddlMonth.Items.AddRange(items.Take(2).ToArray());
        }
        protected void lbtnInvoiceSummary_Click(object sender, EventArgs e)
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));
            string fileName = pdf.InvoicesReport(ah.GetInvoicesFromMonth(GetMonth, Int32.Parse(ddlCompany.SelectedValue)), GetMonth);


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }
        private DateTime GetMonth
        {
            get { return Convert.ToDateTime(ddlMonth.SelectedValue); }
        }

        protected void lbtnEvidence_Click(object sender, EventArgs e)
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

           // List<Dal.Accounting2Result> results = ah.GetReport(GetMonth, Int32.Parse(ddlCompany.SelectedValue));

            List<Dal.OrderPaymentsView> payments = Dal.DbHelper.Orders
    .GetOrderPayments(DateTime.Parse(ddlMonth.SelectedValue), Int32.Parse(ddlCompany.SelectedValue))
    .Where(x=>x.AccountingTypeId == 1).ToList();

            string fileName = pdf.SellSummaryReport(payments, GetMonth);


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
            int[] accoutingTypeIds = chbAccountingType.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();
            int[] paymentTypeIds = chbOrderPaymentType.Items.Cast<ListItem>().Where(x => x.Selected)
    .Select(x => Int32.Parse(x.Value)).ToArray();


            List<Dal.OrderPaymentsView> payments = Dal.DbHelper.Orders
                .GetOrderPayments(
                DateTime.Parse(ddlMonth.SelectedValue),
                Int32.Parse(ddlCompany.SelectedValue),
                chbNotAssigned.Checked,
                chbReceiptReady.Checked,
                accoutingTypeIds,
                paymentTypeIds
                );


            lblEwidencja.Text = String.Format("{0:C}",  payments.Where(x => x.AccountingTypeId == 1).Sum(x => x.Amount));
            lblFaktury.Text = String.Format("{0:C}",    payments.Where(x => x.AccountingTypeId == 2).Sum(x => x.Amount));
            lblKasa.Text = String.Format("{0:C}",       payments.Where(x => x.AccountingTypeId == 3).Sum(x => x.Amount));

            lblTotalAccount.Text = String.Format("{0:C}", payments.Sum(x => x.Amount));


            List<Dal.BankAccountView> bankAccounts = Dal.DbHelper.Accounting.GetBankAccount(DateTime.Parse(ddlMonth.SelectedValue), Int32.Parse(ddlCompany.SelectedValue), 1);

            lblCredit.Text = String.Format("{0:C}", bankAccounts.Where(x => x.TransferType== "CRDT").Sum(x => x.Amount));
            lblDebit.Text = String.Format("{0:C}", bankAccounts.Where(x => x.TransferType == "DBIT").Sum(x => x.Amount));

     


            if (chbInvoice1.Checked)
            {
                if(chbInvoiceNip1.Checked)
                    payments = payments.Where(x => x.InvoiceNumber != null && x.Nip!= "").ToList();
                if (chbInvoiceNip0.Checked)
                    payments = payments.Where(x => x.InvoiceNumber != null && x.Nip == "").ToList();

            }
            if (chbInvoice0.Checked)
                payments = payments.Where(x => x.InvoiceNumber == null).ToList();

            if (calDateFrom.SelectedDate.HasValue)
                payments = payments.Where(x => x.InsertDate >= calDateFrom.SelectedDate.Value).ToList();
            if (calDateTo.SelectedDate.HasValue)
                payments = payments.Where(x => x.InsertDate <= calDateTo.SelectedDate.Value.AddDays(1).AddSeconds(-1)).ToList();




            if (chbReceipt1.Checked)
                payments = payments.Where(x => x.HasReceipt == 1).ToList();
            if (chbReceipt0.Checked)
                payments = payments.Where(x => x.HasReceipt == 0).ToList();

            gvPayments.DataSource = payments;
            gvPayments.DataBind();

            lblCurrent.Text = String.Format("{0:C}", total);


            lbtnEvidence.Visible = lbtnInvoiceSummary.Visible = lbtnInvoices.Visible = lbtnJPK.Visible = hasFullView;
            pnSummary.Visible = hasFullView;

        }

        protected void gvPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal LitId = e.Row.FindControl("LitId") as Literal;
                LitId.Text = String.Format("{0}.", e.Row.RowIndex + 1);

                Dal.OrderPaymentsView o = e.Row.DataItem as Dal.OrderPaymentsView;
                total += o.Amount;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = String.Format("{0:C}", total);
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {

            int[] orderPaymentIds = WebHelper.GetSelectedIds<int>(gvPayments, "chbPOrder");


            Dal.DbHelper.Accounting
               .SetOrderPaymentsAccounting(orderPaymentIds, Int32.Parse(ddlAccountingType.SelectedValue));

            DisplayMessage("Zapisano zmiany");
            btnSearch_Click(null, null);
        }

        protected void lbtnInvoices_Click(object sender, EventArgs e)
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            string[] invoices = ah.GetInvoicesFromMonth(GetMonth, Int32.Parse(ddlCompany.SelectedValue))
                .Where(x => x.ExcludeFromInvoiceReport == false).Select(x => x.InvoiceFileName).ToArray();
            string fileName = pdf.GetInvoices(invoices, Server.MapPath("/Files/Invoices"));


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            Response.WriteFile(fileName);
            Response.End();
        }

        protected void lbtnJPK_Click(object sender, EventArgs e)
        {
            Bll.JPK.JPKHelper jpk = new Bll.JPK.JPKHelper();

            string jpkFileName = jpk.GetJPKFile(GetMonth, Server.MapPath("/Files/JPK"), Int32.Parse(ddlCompany.SelectedValue));
            string fileName = String.Format("{0}/{1}", "/Files/JPK", jpkFileName);


            string contentType =   "Application/xml";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            Response.WriteFile(fileName);
            Response.End();
        }

        protected void lbtnOrderReceipt_Click(object sender, EventArgs e)
        {
            int[] orderPaymentIds = WebHelper.GetSelectedIds<int>(gvPayments, "chbPOrder");

            Bll.OrderHelper.SetOrdersReceipt(orderPaymentIds, UserName);

            DisplayMessage("Wysłano do drukarki fiskalnej");
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPaymentTypes();
        }
    }
}