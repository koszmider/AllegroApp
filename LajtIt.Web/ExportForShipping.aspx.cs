using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using LajtIt.Bll;
using System.Linq;

namespace LajtIt.Web
{
    [Developer("6539d8cc-323e-41f3-ad9f-6059588c549d")]
    public partial class ExportForShipping : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShippingCompanies();
                BindOrderBatch();
            }
        }

        private void BindOrderBatch()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            lsbOrderBatch.DataSource = oh.GetOrderBatches()
                .OrderByDescending(x => x.OrderExportBatchId)
                .Take(100)
                .Select(x => new
                {
                    OrderExportBatchId = x.OrderExportBatchId,
                    BatchName = String.Format("{0} - {1:yyyy-MM-dd HH:mm} - {2}",
                    x.OrderExportBatchId, x.InsertDate, x.ShippingCompany.Name)

                })
                .ToList();
            lsbOrderBatch.DataBind();
        }

        protected void lbtnExportPath_Click(object sender, EventArgs e)
        {
            int batchId = Convert.ToInt32(lsbOrderBatch.SelectedValue);

            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.OrderExportBatch batch = oh.GetOrderExportBatch(batchId);

            if (batch.FileName == null)
            {
                DisplayMessage("Plik nie istnieje");
                return;
            }
            
            string fileName = String.Format(@"{0}\{1}", Server.MapPath("/Files/ExportFiles"), batch.FileName);


            string  contentType = "Application/pdf";

            switch (batch.ShippingCompanyId)
            {
                case 1: contentType = "Application/xls"; break;
                case 2: contentType = "Application/xml"; break;
                case 3: contentType = "Application/xml"; break;
                case 4: contentType = "Application/xml"; break;
                //case 5: contentType = "Application/xls"; break;
                case 5: contentType = "Application/csv"; break;
            }



            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + batch.FileName);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }
        private void BindShippingCompanies()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            ddlShippingCompany.DataSource = oh.GetShipppingCompanies();
            ddlShippingCompany.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                int shippingCompany = Convert.ToInt32(ddlShippingCompany.SelectedValue);
                switch (shippingCompany)
                {
                    //case 1: ExportForSiodemka(Dal.Helper.ShippingCompany.Siodemka); break;
                    case 1: ExportForDpd(); break;
                    case 2: ExportForPocztaPolska(); break;
                    case 3:
                    case 6:
                        ExportForOdbiorOsobisty(); break;
                    case 4: ExportForInPost(); break;
                    case 5: ExportForTba(); break;
                }
                BindOrderBatch();
            }
            catch (Exception ex)
            {
                DisplayMessage(String.Format("Błąd eksportu<br><br>{0}", ex.Message));
                ErrorHandler.LogError(ex, "Export for Shipping");
            }
        }

        //private void ExportForDpd()
        //{
        //    Bll.OrderHelper oh = new Bll.OrderHelper();

        //    DirectoryInfo di = new DirectoryInfo(GetFileWriteDirectory());
        //    oh.ExportForDpd(WebHelper.GetSelectedIds(gvExport, "chbOrder"), di, UserName);
        //    BindForm();
        //}
        private void ExportForDpd()
        {
            DpdHelper dpd = new DpdHelper();
            int batchId = 0;
            dpd.ExportToDpd(WebHelper.GetSelectedIds<int>(gvExport, "chbOrder"), GetFileWriteDirectory(), UserName, out batchId, Bll.DpdWCF.outputDocPageFormatDSPEnumV1.A4);
            BindForm();
        }
        private string GetFileWriteDirectory()
        {
            return Server.MapPath("/Files/ExportFiles"); 
                //System.Configuration.ConfigurationManager.AppSettings["ExportDirectory"];
        }
        private void ExportForInPost()
        {
            Bll.OrderHelper oh = new Bll.OrderHelper();
            oh.ExportForInPost("",WebHelper.GetSelectedIds<int>(gvExport, "chbOrder"),   UserName);
            BindForm();
            DisplayMessage("Tworzenie etykiet zostało zainicjowane");
        }

        private void ExportForOdbiorOsobisty()
        {
            Bll.OrderHelper oh = new Bll.OrderHelper();

            DirectoryInfo di = new DirectoryInfo(GetFileWriteDirectory());
            oh.ExportForOdbiorOsobisty(WebHelper.GetSelectedIds<int>(gvExport, "chbOrder"),  UserName);
            BindForm();
        }

        private void ExportForPocztaPolska()
        {
            Bll.OrderHelper oh = new Bll.OrderHelper();

            DirectoryInfo di = new DirectoryInfo(GetFileWriteDirectory());
            oh.ExportForPocztaPolska(WebHelper.GetSelectedIds<int>(gvExport, "chbOrder"), di, UserName);
            BindForm();
        }

        //private void ExportForSiodemka(Dal.Helper.ShippingCompany sc)
        //{
        //    Bll.OrderHelper oh = new Bll.OrderHelper();
        //    DirectoryInfo di = new DirectoryInfo(GetFileWriteDirectory());

        //    string path = oh.GetExportDataForSiodemka(WebHelper.GetSelectedIds(gvExport,"chbOrder") , di, UserName, sc);
        //    BindForm();
        //}
        private void ExportForTba()
        {
            //Bll.OrderHelper oh = new Bll.OrderHelper();

            //Bll.TbaHelper tba = new TbaHelper();
            //tba.ExportToTba(WebHelper.GetSelectedIds(gvExport, "chbOrder"), UserName);

            //BindForm();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindForm();
        }
        protected void gvExport_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAmountToPay = e.Row.FindControl("litAmountToPay") as Literal;
                Literal LitId = e.Row.FindControl("LitId") as Literal;
                LitId.Text = (e.Row.RowIndex + 1 + gvExport.PageSize * gvExport.PageIndex).ToString();
                Dal.ShippingExport ov = e.Row.DataItem as Dal.ShippingExport;

                if (ov.ShippintTypeId == 4)
                    litAmountToPay.Text = String.Format("{0:C}", -ov.AmountBalance);

            }
        }
        private void BindForm()
        {
            int shippingCompany = Convert.ToInt32(ddlShippingCompany.SelectedValue);
            Dal.OrderHelper oh = new Dal.OrderHelper();
            gvExport.DataSource = oh.GetOrdersForExport(shippingCompany);
            gvExport.DataBind();
            btnExport.Visible = gvExport.Rows.Count > 0;


        }
    }
}