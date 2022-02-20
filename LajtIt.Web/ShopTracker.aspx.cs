using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("ac878794-eab3-4fa7-a706-d573eb58bb5d")]
    public partial class ShopTracker : LajtitPage
    {
        decimal total = 0;
        decimal totalCard = 0;
        bool hasAccessToReport = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            hasAccessToReport = this.HasActionAccess(Guid.Parse("869d4b00-8fc2-45ae-b8c7-2d6412938438"));

            lbtnReport.Visible = hasAccessToReport;

            if (!Page.IsPostBack)
            {
                calDate.SelectedDate = DateTime.Now;
                txbDate.Text = calDate.SelectedDate.Value.ToString("yyyy/MM/dd");
                BindShopPaymentTypes();
                BindReport();
            }
            else
            {
                calDate.SelectedDate = DateTime.Parse(txbDate.Text);
            }
            lblUserName.Text = UserName;
            pnAddPayment.Visible = DateTime.Today == calDate.SelectedDate.Value.Date;
        }
        protected void lbtnChangeDate_Click(object sender, EventArgs e)
        {
            BindReport();
        }
        protected void btnPaymentAdd_Click(object sender, EventArgs e)
        {
            Dal.ShopPaymentTracker t = new Dal.ShopPaymentTracker()
            {
                Amount = Convert.ToDecimal(txbAmount.Text.Trim()),
                InsertDate = DateTime.Now,
                ShopPaymentTypeId = Convert.ToInt32(ddlPaymentType.SelectedValue),
                UserName = UserName,
                Comment = txbComment.Text.Trim()

            };

            Dal.OrderHelper oh = new Dal.OrderHelper();

            try
            {
                oh.SetShopTrackerPayment(t);
                DisplayMessage("Wpis został zarejestrowany"); 

                Bll.EmailSender em = new Bll.EmailSender();
                em.SendEmail("Wypłata z kasy sklepu", String.Format("Użytkownik <b>{0}</b> wypłacił {1:C} z kasy sklepowej. Komentarz: {2}", UserName, t.Amount, txbComment.Text), Dal.Helper.MyManagerEmail);

            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
            }
            BindReport();
        }

        protected void gvShopTrackerReport_OnDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ShopPaymentTrackerView t = e.Row.DataItem as Dal.ShopPaymentTrackerView;

                total += t.Calculate * t.Amount.Value;
                if (t.ShopPaymentTypeId == 6)
                    totalCard += t.Amount.Value;

                switch (t.Calculate)
                {
                    case -1: e.Row.Style.Add("color", "red"); break;
                    case 1: e.Row.Style.Add("color", "green"); break;
                }

                if(t.CanCreateReceipt.Value==false)
                {
                    e.Row.Cells[6].Controls.Clear();
                    e.Row.Cells[7].Controls.Clear();

                }
            }
        }

        protected void lbtnChecked_Click(object sender, EventArgs e)
        {
            int orderPaymentId = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetOrderPaymentReceipt(orderPaymentId);
            BindReport();

            DisplayMessage("Paragon utworzony. Upewnij się, że masz go fizycznie.");
        }
        private void BindReport()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            var o = oh.ShopPaymentTrackerReport(calDate.SelectedDate.Value.Year
                , calDate.SelectedDate.Value.Month
                , calDate.SelectedDate.Value.Day);

            gvShopTrackerReport.DataSource = o;
            gvShopTrackerReport.DataBind();

            decimal sum = o.Where(x => x.CanCreateReceipt.Value).Sum(x => x.Amount.Value);
            lblReceiptSum.Text = String.Format("{0:C}", sum);
            cvReceiptSum.ValueToCompare = sum.ToString();

            lblTotal.Text = String.Format("{0:C}", total);
            lblCardTotal.Text = String.Format("{0:C}", totalCard);
        }

        private void BindShopPaymentTypes()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            ddlPaymentType.DataSource = oh.GetShopPaymentTypes().Where(x=>x.IsActive).ToList();
            ddlPaymentType.DataBind();
        }

        protected void btnDailyReport_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<Dal.ShopPaymentTracker> spt = oh.GetShopPayments(DateTime.Now);

            if(spt.Where(x=>x.ShopPaymentTypeId==8).FirstOrDefault()!=null)
            {

                DisplayMessage("Raport dobowy został już dodany");
                BindReport();
                return;
            }

            Dal.ShopPaymentTracker t = new Dal.ShopPaymentTracker()
            {
                Amount = Convert.ToDecimal(txbDailyReport.Text.Trim()),
                InsertDate = DateTime.Now,
                ShopPaymentTypeId = 8,
                UserName = UserName,
                Comment = ""

            };


            try
            {
                oh.SetShopTrackerPayment(t);
                DisplayMessage("Wpis został zarejestrowany"); 

                Bll.EmailSender em = new Bll.EmailSender();
                em.SendEmail("Raport dobowy", String.Format("Użytkownik <b>{0}</b> uzupełnił raport dobowy {1:C} ", UserName, t.Amount), Dal.Helper.MyManagerEmail);

            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
            }
            BindReport();
        }

        protected void lbtnReport_Click(object sender, EventArgs e)
        {
            string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            Dal.OrderHelper oh = new Dal.OrderHelper();
            var spt = oh.GetOrderPaymentsCash(calDate.SelectedDate.Value)
                .Where(x=>x.Amount>0)
                .OrderBy(x=>x.InsertDate)
                .Select(x => new
                {
                    Data = x.InsertDate,
                    Kwota = x.Amount
                }
                ).ToList();
                ;

            string saveLocation = String.Format(path, "RaportKasowy.csv");


            using (StreamWriter writer = new StreamWriter(saveLocation))
            {


                using (CsvHelper.CsvWriter csv = new CsvHelper.CsvWriter(writer))
                {
                    csv.Configuration.Delimiter = ";";
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.CultureInfo = CultureInfo.GetCultureInfo("pl-PL");

                    csv.WriteRecords(spt);
                }

                string contentType = contentType = "Application/xml";

                HttpContext.Current.Response.ContentType = contentType;
                HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(saveLocation)).Name);

                //Write the file directly to the HTTP content output stream.
                HttpContext.Current.Response.WriteFile(saveLocation);
                HttpContext.Current.Response.End();

            }
        }
    }
}