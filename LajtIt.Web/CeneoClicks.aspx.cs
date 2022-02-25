using LinqToExcel;
using LinqToExcel.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("2b21d450-4fc7-452f-9c27-d5c1d2505fb5")]
    public partial class CeneoClicks : LajtitPage
    {
        private decimal totalCost = 0;
        private decimal totalSell = 0;
        public class CeneoXlsFile
        {
            [ExcelColumn("Kategoria glowna")]
            public string KategoriaGlowna { get; set; }
            [ExcelColumn("Kategoria najnizsza")]
            public string KategoriaNajnizsza { get; set; }
            [ExcelColumn("Produkt")]
            public string Produkt { get; set; }
            [ExcelColumn("Id produktu w sklepie")]
            public string IdProduktu { get; set; }
            [ExcelColumn("Data")]
            public DateTime Data { get; set; }
            [ExcelColumn("Stawka przekliku")]
            public decimal StawkaPrzekliku { get; set; }
            [ExcelColumn("Stawka podbicia")]
            public decimal? StawkaPodbicia { get; set; }
            [ExcelColumn("Koszt przekliku razem")]
            public decimal? KosztPrzeklikuRazem { get; set; }
            [ExcelColumn("Zajmowana pozycja")]
            public int? ZajmowanaPozycja { get; set; }
            [ExcelColumn("IP")]
            public string IP { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime? lastCeneo = Dal.DbHelper.Shop.GetCeneoClicksLast();

            if (lastCeneo == null)
            {
                lblCeneo.Text = "brak danych";
                lastCeneo = DateTime.Now;
            }
            else
                lblCeneo.Text = String.Format("{0:yyyy/MM/dd HH:mm}", lastCeneo);


            if (!Page.IsPostBack)
            {
                calFrom.SelectedDate = DateTime.Now.AddDays(-8);
                calTo.SelectedDate = lastCeneo;
              
            }
            else
            {
                calFrom.SelectedDate = DateTime.Parse(txbDateFrom.Text);
                calTo.SelectedDate = DateTime.Parse(txbDateTo.Text);

                if (calTo.SelectedDate.Value.AddDays(1) > lastCeneo)
                    calTo.SelectedDate = lastCeneo;
            }

        }

        protected void btnReportUpload_Click(object sender, EventArgs e)
        {
            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            HttpFileCollection uploadedFiles = Request.Files;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbe = new StringBuilder();


            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];

                try
                {
                    if (userPostedFile.ContentLength > 0)
                    {


                        string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(userPostedFile.FileName));
                        string orginalFileName = System.IO.Path.GetFileName(userPostedFile.FileName);
                        string saveLocation = String.Format(path, fileName);

                        if (saveLocation != null)
                            userPostedFile.SaveAs(saveLocation);

                        // Bll.ProductCatalogHelper.SaveFile(productCatalogIds, saveLocation, fileName, orginalFileName);

                        sb.AppendLine(String.Format("{0}. plik: {1}<br>", i + 1, userPostedFile.FileName));


                        ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(ProcessFile);

                        Thread thread = new Thread(parameterizedThreadStart);

                        thread.Start(saveLocation);


                         
                    }
                }
                catch (Exception Ex)
                {
                    sbe.AppendLine(String.Format("{0}. plik: {1}<br>", i + 1, userPostedFile.FileName));
                }
            }
            if (sbe.Length == 0)
                DisplayMessage(String.Format("Zapisano poprawnie<br><br>{0}", sb.ToString()));
            else
                DisplayMessage(String.Format("Błędy<br><br>{0}", sbe.ToString()));

        }
        private void ProcessFile(object saveLocation)
        {
            ExcelQueryFactory eqf = new ExcelQueryFactory(saveLocation.ToString());// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\Maytoni_201810021244.xlsx");// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\LajtitImport.xls");
            //eqf.WorksheetNoHeader();
            var r = from p in eqf.WorksheetRange<CeneoXlsFile>("A5", "K60000", 0) select p;

            var rr = r.ToList();

            ProcessFile(rr.Take(rr.Count()-1).ToList());


        }

        private void ProcessFile(List<CeneoXlsFile> rr)
        {
            List<Dal.CeneoClicks> clicks = rr.Select(x => new Dal.CeneoClicks()
            {
                ClickCost = x.StawkaPrzekliku,
                Date=x.Data,
                ExtraCost=x.StawkaPodbicia,
                IP=x.IP,
                LeafCategory=x.KategoriaNajnizsza,
                MainCategory=x.KategoriaGlowna,
                Position=x.ZajmowanaPozycja,
                ProductName=x.Produkt,
                ShopId=(int)Dal.Helper.Shop.Ceneo,
                ShopProductId=x.IdProduktu,
                

            }).ToList();

            Dal.DbHelper.Shop.SetCeneoClick(clicks);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData("Rate");
        }


    
            protected void gvCeneo_Sorting(object sender, GridViewSortEventArgs e)
        {
            BindData(e.SortExpression);
        }

        private void BindData(string sortExpression)
        {
            var d= Dal.DbHelper.Shop.GetCeneoClicksBySupplier(calFrom.SelectedDate, calTo.SelectedDate.Value.AddDays(1));

            switch(sortExpression)
            {
                case "Rate": d = d.OrderByDescending(x => x.Rate).ToList(); break;
                case "TotalCost": d = d.OrderByDescending(x => x.TotalCost).ToList(); break;
                case "TotalSell": d = d.OrderByDescending(x => x.TotalSell).ToList(); break;
                case "TotalCostRate": d = d.OrderByDescending(x => x.TotalCostRate).ToList(); break;
                case "TotalSellRate": d = d.OrderByDescending(x => x.TotalSellRate).ToList(); break;
                case "SupplierName": d = d.OrderBy(x => x.SupplierName).ToList(); break;
            }
            totalCost = d.Sum(x => x.TotalCost);
            totalSell = d.Sum(x => x.TotalSell);

            gvCeneo.DataSource = d;
            gvCeneo.DataBind();

        }

        protected void gvCeneo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = String.Format("{0:C}", totalCost);
                e.Row.Cells[2].Text = String.Format("{0:C}", totalSell);
                if (totalSell != 0)
                    e.Row.Cells[3].Text = String.Format("{0:0.00}%", totalCost * 100 / totalSell);
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.CeneoClicksSupplierFnResult ceneo = e.Row.DataItem as Dal.CeneoClicksSupplierFnResult;

                if (totalSell != 0)
                    if (ceneo.Rate > totalCost * 100 / totalSell)
                        e.Row.Cells[3].BackColor = Color.LightPink;
                    else
                        e.Row.Cells[3].BackColor = Color.LightGreen;
                 
            }
        }
    }
}