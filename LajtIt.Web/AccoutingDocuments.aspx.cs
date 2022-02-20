using LajtIt.Bll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("39f38933-00de-4502-a39c-60e944648ed6")]
    public partial class AccoutingDocuments : LajtitPage
    {
        bool hasFullView = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            hasFullView = this.HasActionAccess(Guid.Parse("8a6b8ca6-8d38-4eb2-a014-d9133cabc9d8"));


            if (!Page.IsPostBack)
            {
                BindMonths();
            }
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
             

            if (hasFullView)
                ddlMonth.Items.AddRange(items.ToArray());
            else
                ddlMonth.Items.AddRange(items.Take(3).ToArray());

            ddlMonth.SelectedIndex = 1;
        }

        protected void lbtnAllegro_Click(object sender, EventArgs e)
        {
            string fileName = PaymentsBankAccountMarketplace.GetShopStatement((int)Dal.Helper.Shop.JacekStawicki, 
                DateTime.Parse(ddlMonth.SelectedValue));
            string contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=Allegro_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }

        protected void lbtnDpd_Click(object sender, EventArgs e)
        {
            string fileName = PaymentsBankAccountMarketplace.GetShopStatement(0, DateTime.Parse(ddlMonth.SelectedValue));
            string contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=DPD_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();


            //List<Dal.DpdPaymentsView> dpdPayments = Dal.DbHelper.Orders.GetDpdPayments(DateTime.Parse(ddlMonth.SelectedValue), Dal.Helper.DefaultCompanyId);
            //PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files/ExportFiles"));

            //string fileName = pdf.DpdPayments(dpdPayments, Dal.Helper.DefaultCompanyId, DateTime.Parse(ddlMonth.SelectedValue));

            //string contentType = "Application/pdf";

            //Response.ContentType = contentType;
            //Response.AppendHeader("content-disposition", "attachment; filename=DPD_" + (new FileInfo(fileName)).Name);

            ////Write the file directly to the HTTP content output stream.
            //Response.WriteFile(fileName);
            //Response.End();
        }

        protected void lbtnCeneo_Click(object sender, EventArgs e)
        {
            string fileName = PaymentsBankAccountMarketplace.GetShopStatement((int)Dal.Helper.Shop.Ceneo, DateTime.Parse(ddlMonth.SelectedValue));
            string contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=Ceneo_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();

        }

        protected void lbtnLajtit_Click(object sender, EventArgs e)
        {
            string fileName = PaymentsBankAccountMarketplace.GetShopStatement((int)Dal.Helper.Shop.Lajtitpl, DateTime.Parse(ddlMonth.SelectedValue));
            string contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=Przelewy24_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();

        }

        protected void lbtnInvoices_Click(object sender, EventArgs e)
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            string[] invoices = ah.GetInvoicesFromMonth(DateTime.Parse(ddlMonth.SelectedValue), Dal.Helper.DefaultCompanyId)
                .Where(x =>  x.AccountingTypeId == null || x.AccountingTypeId == 2 ).Select(x => x.InvoiceFileName).ToArray();
            string fileName = pdf.GetInvoices(invoices, Server.MapPath("/Files/Invoices"));


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            Response.WriteFile(fileName);
            Response.End();
        }

        protected void lbtnInvoicesCorrections_Click(object sender, EventArgs e)
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            string[] invoices = ah.GetInvoicesCorrections(DateTime.Parse(ddlMonth.SelectedValue), Dal.Helper.DefaultCompanyId)
                .Where(x => x.AccountingTypeId == null || x.AccountingTypeId == 2).Select(x => x.InvoiceFileName).ToArray();
            string fileName = pdf.GetInvoices(invoices, Server.MapPath("/Files/Invoices"));


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            Response.WriteFile(fileName);
            Response.End();
        }

        protected void lbtnInvoicesSummary_Click(object sender, EventArgs e)
        {

            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            List<Dal.InvoicesView> invoices = ah.GetInvoicesFromMonth(DateTime.Parse(ddlMonth.SelectedValue), Dal.Helper.DefaultCompanyId)
                //.Where(x => x.AccountingTypeId == null || x.AccountingTypeId == 2)
                .ToList();


            string fileName = pdf.InvoicesReport(invoices,
                DateTime.Parse(ddlMonth.SelectedValue));


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=Faktury_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }

        protected void lbtnJPK_Click(object sender, EventArgs e)
        {
            Bll.JPK.JPKHelper jpk = new Bll.JPK.JPKHelper();

            string jpkFileName = jpk.GetJPKFile(DateTime.Parse(ddlMonth.SelectedValue), Server.MapPath("/Files/JPK"), 
                Dal.Helper.DefaultCompanyId);
            string fileName = String.Format("{0}/{1}", "/Files/JPK", jpkFileName);


            string contentType = "Application/xml";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            Response.WriteFile(fileName);
            Response.End();
        }
        protected void lbtnJPKMAG_Click(object sender, EventArgs e)
        {
            Bll.JPK.JPKHelper jpk = new Bll.JPK.JPKHelper();

            string jpkFileName = jpk.GetJPKMAGFile(DateTime.Parse(ddlMonth.SelectedValue), Server.MapPath("/Files/JPK"), 
                Dal.Helper.DefaultCompanyId);
            string fileName = String.Format("{0}/{1}", "/Files/JPK", jpkFileName);


            string contentType = "Application/xml";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=JPK_MAG_" + (new FileInfo(fileName)).Name);

            Response.WriteFile(fileName);
            Response.End();
        }

        protected void lbtnEvidence_Click(object sender, EventArgs e)
        {
            
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            

            List<Dal.OrderPaymentsView> payments = Dal.DbHelper.Orders
    .GetOrderPayments(DateTime.Parse(ddlMonth.SelectedValue), Dal.Helper.DefaultCompanyId)
    .Where(x => x.AccountingTypeId == 1).ToList();

            string fileName = pdf.SellSummaryReport(payments, DateTime.Parse(ddlMonth.SelectedValue));


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }

        protected void lbtnMorele_Click(object sender, EventArgs e)
        {
            string fileName = PaymentsBankAccountMarketplace.GetShopStatement((int)Dal.Helper.Shop.Morele, DateTime.Parse(ddlMonth.SelectedValue));
            string contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=Morele_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }

        protected void lbtnFirstData_Click(object sender, EventArgs e)
        {
            string fileName = PaymentsBankAccountMarketplace.GetShopStatement((int)Dal.Helper.Shop.Showroom, DateTime.Parse(ddlMonth.SelectedValue));
            string contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=FirstData_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();

        }

        protected void lbtnPayBack_Click(object sender, EventArgs e)
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            List<Dal.OrderPaymentsView> payments = Dal.DbHelper.Orders
                .GetOrderPayments(
                DateTime.Parse(ddlMonth.SelectedValue),
                (int)Dal.Helper.DefaultCompanyId,
                false,
                false,
                new int[] { (int)Dal.Helper.OrderPaymentAccoutingType.Evidence },
                new int[] { }
                );


            string fileName = pdf.OrderPaymentsReport(payments, DateTime.Parse(ddlMonth.SelectedValue), "Wpłaty i zwrotu ewidencja");


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=Wplaty_Zwroty_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }

        protected void lbtnWarehouse_Click(object sender, EventArgs e)
        {
            string fileName = PaymentsBankAccountMarketplace.GetProductCatalogWarehouse(DateTime.Parse(ddlMonth.SelectedValue));
            string contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=Magazyn_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();

        }
 

        protected void lbtnWarehouseEndMonthList_Click(object sender, EventArgs e)
        {
            string fileName = PaymentsBankAccountMarketplace.GetProductCatalogWarehouseStock(DateTime.Parse(ddlMonth.SelectedValue));
            string contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=Magazyn_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();

        }

        protected void lbtnPayBack2_Click(object sender, EventArgs e)
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            List<Dal.OrderPaymentsView> payments = Dal.DbHelper.Orders
                .GetOrderPayments(
                DateTime.Parse(ddlMonth.SelectedValue),
                (int)Dal.Helper.DefaultCompanyId,
                false,
                false,
                new int[] { (int)Dal.Helper.OrderPaymentAccoutingType.Evidence },
                new int[] { }
                ).Where(x => x.Amount < 0).ToList();


            string fileName = pdf.OrderPaymentsReport(payments, DateTime.Parse(ddlMonth.SelectedValue), "Zwroty ewidencja");


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=Zwroty_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }
        protected void lbtnPayBack3_Click(object sender, EventArgs e)
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            List<Dal.OrderPaymentsView> payments = Dal.DbHelper.Orders
                .GetOrderPayments(
                DateTime.Parse(ddlMonth.SelectedValue),
                (int)Dal.Helper.DefaultCompanyId,
                false,
                false,
                new int[] { (int)Dal.Helper.OrderPaymentAccoutingType.Evidence },
                new int[] { }
                ).Where(x => x.Amount > 0).ToList();


            string fileName = pdf.OrderPaymentsReport(payments, DateTime.Parse(ddlMonth.SelectedValue), "Wpłaty ewidencja");


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=Zwroty_" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }

    }
}