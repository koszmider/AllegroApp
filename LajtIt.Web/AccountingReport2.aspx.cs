using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using System.IO;
using System.Drawing;

namespace LajtIt.Web
{
    [Developer( "a84d93df-87ff-41de-ba3f-4c45d7f1a04f")]
    public partial class AccoutingReporting2 : LajtitPage
    {
        private DateTime GetMonth
        {
            get { return Convert.ToDateTime(ddlMonths.SelectedValue); }
        }
        private int CompanyId
        {
            get { return Convert.ToInt32(ddlCompany.SelectedValue); }
        }

        private decimal total = 0;
        private decimal totalInvoice = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                BindMonths();
                BindReport();
            }
        }

        private void BindReport()
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();

            List<Dal.Accounting2Result> results = ah.GetReport(GetMonth, CompanyId);

            gvResults.DataSource = results;
            gvResults.DataBind();

            ShowInvoices();

        }
        private void ShowInvoices()
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();

            var i=    ah.GetInvoicesFromMonth(GetMonth, CompanyId);

            if (chbNotLocked.Checked)
                i = i.Where(x => !x.IsLocked.HasValue || x.IsLocked.Value == false).ToList();

            gvInvoices.DataSource = i;
            gvInvoices.DataBind();
        }
        protected void gvResults_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.Accounting2Result r = e.Row.DataItem as Dal.Accounting2Result;
                if (r.ForEwidence.HasValue)
                    total += r.ForEwidence.Value;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = String.Format("{0:C}", total);
            }

        }
        protected void gvInvoices_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.InvoicesView r = e.Row.DataItem as Dal.InvoicesView;
                totalInvoice += r.CalculatedTotalNetto.Value;
                //totalInvoice0 += r.CalculatedTotalNetto.Value;


                if (!r.AmountPaid.HasValue || r.AmountPaid.Value != r.CalculatedTotalBrutto)
                    e.Row.BackColor = Color.Silver;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = String.Format("{0:C}", totalInvoice);
            }

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindReport();
        }
        protected void btnJPK_FA_Click(object sender, EventArgs e)
        {
            Bll.JPK.JPKHelper jpk = new Bll.JPK.JPKHelper();
             
            string jpkFileName = jpk.GetJPKFile(GetMonth, Server.MapPath("/Files/JPK"), CompanyId);
            string fileName = String.Format("{0}/{1}", "/Files/JPK", jpkFileName);


            string contentType = contentType = "Application/xml";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);
             
            Response.WriteFile(fileName);
            Response.End();
        }
        protected void btnPrintInvoices_Click(object sender, EventArgs e)
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            string[] invoices = ah.GetInvoicesFromMonth(GetMonth, CompanyId).Where(x=>x.ExcludeFromInvoiceReport==false).Select(x => x.InvoiceFileName).ToArray();
            string fileName = pdf.GetInvoices(invoices, Server.MapPath("/Files/Invoices"));


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);
             
            Response.WriteFile(fileName);
            Response.End();
        }
        protected void btnPrintInvoiceSummary_Click(object sender, EventArgs e)
        {

            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));
            string fileName = pdf.InvoicesReport(ah.GetInvoicesFromMonth(GetMonth, CompanyId), GetMonth);


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }
        protected void btnEwidence_Click(object sender, EventArgs e)
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            List<Dal.Accounting2Result> results = ah.GetReport(GetMonth, CompanyId);
            string fileName = pdf.SellSummaryReport(results, GetMonth);


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }
        private void BindMonths()
        {
            ddlMonths.Items.Clear();


            List<ListItem> items = new List<ListItem>();
            for (int year = DateTime.Now.Year; year >= 2012; year--)
                for (int month = DateTime.Now.Year == year ? DateTime.Now.Month : 12; month >= 1; month--)
                {
                    ListItem item = new ListItem()
                    {
                        Text = String.Format("{0}/{1}", year, month),
                        Value = String.Format("{0}/{1}/{2}", year, month, 1)
                    };
                    items.Add(item);
                }

            ddlMonths.Items.AddRange(items.ToArray());

        }
    }
}