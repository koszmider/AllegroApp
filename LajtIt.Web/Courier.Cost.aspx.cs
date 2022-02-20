using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsvHelper.Configuration;
using LajtIt.Bll;
using LajtIt.Dal;
using LinqToExcel;
using LinqToExcel.Attributes;

namespace LajtIt.Web
{
    [Developer("cf945f7c-52f6-4707-bc74-9b1fbc3e066a")]
    public partial class CourierCosts : LajtitPage
    {

        decimal total = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
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

            ddlMonth.Items.AddRange(items.ToArray());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {



            var c1 = Dal.DbHelper.Accounting.GetShippingCompanyCosts(DateTime.Parse(ddlMonth.SelectedValue), Int32.Parse(ddlCourier.SelectedValue));
            if (txbFrom.Text != "")
                c1 = c1.Where(x => x.TotalCost >= Int32.Parse(txbFrom.Text)).ToList();
            if (txbTo.Text != "")
                c1 = c1.Where(x => x.TotalCost <= Int32.Parse(txbTo.Text)).ToList();
            switch (ddlOrder.SelectedIndex)
            {
                case 1: c1 = c1.Where(x => x.OrderId.HasValue).ToList(); break;
                case 2: c1 = c1.Where(x => !x.OrderId.HasValue).ToList(); break;
            }
            gvParcels.DataSource = c1;
            gvParcels.DataBind();

        }
        protected void gvDpd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal LitId = e.Row.FindControl("LitId") as Literal;
                LitId.Text = String.Format("{0}.", e.Row.RowIndex + 1);

                Dal.ShippingCompanyCost c = e.Row.DataItem as Dal.ShippingCompanyCost;
                total += c.TotalCost.Value;


                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[2].Text = String.Format("{0:C}", total);
                }
            }
        }
      


        protected void btnRaportUpload_Click(object sender, EventArgs e)
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

                        switch (ddlFileSource.SelectedValue)
                        {
                            case "1":
                                ProcessDpdFile(saveLocation); break;
                            case "2":
                                ProcessInpostFile(saveLocation); break;
                            case "3":
                                ProcessAllegroFile(saveLocation); break;
                        }
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

        #region Allegro
        public class AllegroImport
        {
            public string Data { get; set; }
            public string NazwaOferty { get; set; }
            public string IdOferty { get; set; }
            public string TypOperacji { get; set; }
            public string Uznania { get; set; }
            public string Obciazenia { get; set; }
            public string Saldo { get; set; }
            public string Szczegoly { get; set; }

        }
        public sealed class FooMap2 : ClassMap<AllegroImport>
        {
            public FooMap2()
            {
                Map(m => m.Data).Index(0);
                Map(m => m.NazwaOferty).Index(1);
                Map(m => m.IdOferty).Index(2);
                Map(m => m.TypOperacji).Index(3);
                Map(m => m.Uznania).Index(4);//.TypeConverterOption.CultureInfo(new CultureInfo("pl-PL")).TypeConverterOption.NumberStyles(NumberStyles.Any);
                Map(m => m.Obciazenia).Index(5);//.TypeConverterOption.CultureInfo(new CultureInfo("pl-PL")).TypeConverterOption.NumberStyles(NumberStyles.Any); 
                Map(m => m.Saldo).Index(6);//.TypeConverterOption.CultureInfo(new CultureInfo("pl-PL")).TypeConverterOption.NumberStyles(NumberStyles.Any);
                Map(m => m.Szczegoly).Index(7);
            }
        }
        private void ProcessAllegroFile(string saveLocation)
        {

            try
            {


                using (TextReader reader = File.OpenText(saveLocation))
                {


                    CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.Delimiter = ";";
                    csv.Configuration.CultureInfo = CultureInfo.InvariantCulture;
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.RegisterClassMap<FooMap2>();
                    // List<CandelluxImport> records = csv.GetRecords<CandelluxImport>().ToList();
                    List<AllegroImport> records = csv.GetRecords<AllegroImport>().ToList();

                    //// odkąd candellux podaje równiez Apeti trzeba te kody wywalic
                    //if (SupplierId == 50)
                    //    records = records.Where(x => !x.Indeks.StartsWith("A")).ToList();
                    
                    ProcessAllegroFile(records);
                    //GetImages(records);

                }

            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);

            }

        }
        private static void ProcessAllegroFile(List<AllegroImport> data)
        {
            List<Dal.ShippingCompanyCost> costs = new List<Dal.ShippingCompanyCost>();

            try
            {

                foreach (AllegroImport t in data)
                {
                    IsDpdCost(t, costs);
                    //IsInpostCost(t, costs);

                    //costs.Add(
                    //    new Dal.InpostCost()
                    //    {
                    //        DataNadania = DateTime.Parse(t.DataNadania),
                    //        KwotaNetto = t.KwotaNetto,
                    //        Nadawca = t.Nadawca,
                    //        NumerPrzesylki = t.NumerPrzesylki,
                    //        Odbiorca = t.Odbiorca,
                    //        Paczki = t.Paczki,
                    //        Pozycja = t.Pozycja,
                    //        Serwis = t.Serwis,
                    //        Uslugi = t.Uslugi,
                    //        WagaLaczna = t.WagaLaczna,
                    //        Uwagi = t.Uwagi,
                    //        Waga = t.Waga,
                    //        InsertDate = DateTime.Now


                    //    });
                }
                Dal.DbHelper.Accounting.SetShippingCompanyCosts(costs);
             
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private static void IsInpostCost(AllegroImport import, List<ShippingCompanyCost> inpostCosts)
        {
            if (import.Szczegoly.ToUpper().Contains("INPOST"))
            {
                string allegroOrderId = RetrieveGUID2(import.Szczegoly);
                string parcelNumber = RetrieveInpostParcelNumber(import.Szczegoly);

                if (allegroOrderId != null)
                {
                    Dal.Order order = Dal.DbHelper.AllegroHelper.GetAllegroOrder(allegroOrderId);

                    ShippingCompanyCost dpdCost = new ShippingCompanyCost()
                    {
                        CostExternalId = import.Szczegoly,
                        ServiceName = "allegro",
                        Cost = -Decimal.Parse(import.Obciazenia, NumberStyles.Number),
                        TotalCost = -Decimal.Parse(import.Obciazenia, NumberStyles.Number),
                        Comments = import.Szczegoly,
                        SendDate = DateTime.Parse(import.Data.Replace(".", "/")),// CultureInfo.InvariantCulture),
                        DeliveryDate = DateTime.Parse(import.Data.Replace(".", "/")), //CultureInfo.InvariantCulture),
                        InsertDate = DateTime.Now,
                        ParcelNumber = parcelNumber,
                        ShippingCompanyId = (int)Dal.Helper.ShippingCompany.InPost
                    };

                    if (order != null)
                        dpdCost.OrderId = order.OrderId;

                    inpostCosts.Add(dpdCost);
                }

            }
        }

        private static void IsDpdCost(AllegroImport import, List<ShippingCompanyCost> dpdCosts)
        {
            if (import.Szczegoly.Contains("DPD"))
            {
                string allegroOrderId = RetrieveGUID2(import.Szczegoly);
                string parcelNumber = RetrieveDpdParcelNumber(import.Szczegoly);

                if (allegroOrderId != null)
                {
                    Dal.Order order = Dal.DbHelper.AllegroHelper.GetAllegroOrder(allegroOrderId);

                    string[] d = import.Data.Substring(0, 10).Split(new char[] { '.' });

                    ShippingCompanyCost dpdCost = new ShippingCompanyCost()
                    {
                        CostExternalId = import.Szczegoly,
                        InvoiceNumber = "allegro",
                        Cost = -Decimal.Parse(import.Obciazenia, NumberStyles.Number),
                        TotalCost = -Decimal.Parse(import.Obciazenia, NumberStyles.Number),
                        Comments = import.Szczegoly,
                        SendDate = new DateTime(Int32.Parse(d[2]), Int32.Parse(d[1]), Int32.Parse(d[0])),
                        DeliveryDate = new DateTime(Int32.Parse(d[2]), Int32.Parse(d[1]), Int32.Parse(d[0])), 
                        InsertDate = DateTime.Now,
                        ParcelNumber = parcelNumber,
                        ParcelNumber2 = parcelNumber,
                        ShippingCompanyId = (int)Dal.Helper.ShippingCompany.Dpd
                    };

                    if (order != null)
                        dpdCost.OrderId = order.OrderId;

                    dpdCosts.Add(dpdCost);
                }

            }
        }
        private static string RetrieveDpdParcelNumber(string line)
        {

            var match = Regex.Match(line, @"[0-9]{13}[A-Za-z]{1}");
            if (match.Success)
                return match.Value;


            return null;
        }
        private static string RetrieveInpostParcelNumber(string line)
        {

            var match = Regex.Match(line, @"[0-9]{24}");
            if (match.Success)
                return match.Value;


            return null;
        }
        private static string RetrieveGUID2(string line)
        {

            var match = Regex.Match(line, @"[{(]?[0-9A-Fa-f]{8}[-]?([0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}[)}]?");
            if (match.Success)
                return match.Value;


            return null;
        }
        #endregion
        #region Inpost
        public class InpostImport
        {
            public string NumerPrzesylki { get; set; }
            public string DataNadania { get; set; }
            public string Odbiorca { get; set; }
            public string Nadawca { get; set; }
            public decimal KwotaNetto { get; set; }
            public string Serwis { get; set; }
            public decimal Waga { get; set; }
            public decimal WagaLaczna { get; set; }
            public string Uwagi { get; set; }
            public int Paczki { get; set; }
            public string Pozycja { get; set; }
            public string Uslugi { get; set; }

        }
        public sealed class FooMap : ClassMap<InpostImport>
        {
            public FooMap()
            {
                Map(m => m.NumerPrzesylki).Name("nr");
                Map(m => m.DataNadania).Name("datanadania");
                Map(m => m.Nadawca).Name("nadawca");
                Map(m => m.Odbiorca).Name("odbiorca");
                Map(m => m.KwotaNetto).Name("netto").TypeConverterOption.CultureInfo(new CultureInfo("nl-NL")).TypeConverterOption.NumberStyles(NumberStyles.Any);
                Map(m => m.Serwis).Name("serwis");
                Map(m => m.Waga).Name("waga").TypeConverterOption.CultureInfo(new CultureInfo("nl-NL")).TypeConverterOption.NumberStyles(NumberStyles.Any);
                Map(m => m.WagaLaczna).Name("wagalaczna").TypeConverterOption.CultureInfo(new CultureInfo("nl-NL")).TypeConverterOption.NumberStyles(NumberStyles.Any);
                Map(m => m.Uwagi).Name("uwagi");
                Map(m => m.Paczki).Name("paczki").TypeConverterOption.CultureInfo(new CultureInfo("nl-NL")).TypeConverterOption.NumberStyles(NumberStyles.Any);
                Map(m => m.Pozycja).Name("pozycja");
                Map(m => m.Uslugi).Name("uslugi");
            }
        }
        private void ProcessInpostFile(string saveLocation)
        {

            try
            {


                using (TextReader reader = File.OpenText(saveLocation))
                {


                    CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.Delimiter = ";";
                    csv.Configuration.CultureInfo = CultureInfo.InvariantCulture;
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.RegisterClassMap<FooMap>();
                    // List<CandelluxImport> records = csv.GetRecords<CandelluxImport>().ToList();
                    List<InpostImport> records = csv.GetRecords<InpostImport>().ToList();

                    //// odkąd candellux podaje równiez Apeti trzeba te kody wywalic
                    //if (SupplierId == 50)
                    //    records = records.Where(x => !x.Indeks.StartsWith("A")).ToList();

                    ProcessInpostData(records);
                    //GetImages(records);

                }

            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);

            }

        }
        private static void ProcessInpostData(List<InpostImport> data)
        {
            List<Dal.ShippingCompanyCost> costs = new List<Dal.ShippingCompanyCost>();

            foreach (InpostImport t in data)
            {
                costs.Add(
                    new Dal.ShippingCompanyCost()
                    {
                        SendDate = DateTime.Parse(t.DataNadania),
                        DeliveryDate = DateTime.Parse(t.DataNadania),
                        Cost = t.KwotaNetto,
                        TotalCost = t.KwotaNetto,
                        SenderName = t.Nadawca,
                        ParcelNumber = t.NumerPrzesylki,
                        Recipent = t.Odbiorca,
                        ParcelCount = t.Paczki,
                        // = t.Pozycja,
                        ServiceName = t.Serwis,
                        Comments = String.Format("{0} {1}", t.Uwagi, t.Uslugi),
                        Weight = t.Waga,
                        InsertDate = DateTime.Now,
                        CostExternalId=t.NumerPrzesylki,
                        DeliveredTo=t.Odbiorca,
                        SentFrom=t.Nadawca,
                        ShippingCompanyId = (int)Dal.Helper.ShippingCompany.InPost

                    });
            }
            Dal.DbHelper.Accounting.SetShippingCompanyCosts(costs);
        }
        #endregion
        #region Dpd
        public class DpdXlsFile
        {

            //  id Nr_Faktury  Firma/Dział zlecający   Płatnik Nazwa nadawcy Ulica nadawcy Miasto nadawcy PlatnikNazwisko Data nadania    DataWykonaniaUslugi
            //Numer Listu List pierwotny Uwagi   Cena netto  Kwota Nadanie Doręczenie Rodzaj usługi waga    ilość paczek   
            //Nazwa odbiorcy  Miasto odbiorcy Ulica odbiorcy  Numer domu  platnik Nr klienta

            [ExcelColumn("id")]
            public int DpdCostId { get; set; }

            [ExcelColumn("Nr_Faktury")]
            public string NrFaktury { get; set; }

            [ExcelColumn("Nazwa nadawcy")]
            public string NazwaNadawcy { get; set; }

            [ExcelColumn("Data nadania")]
            public DateTime DataNadania { get; set; }

            [ExcelColumn("DataWykonaniaUslugi")]
            public DateTime DataWykonaniaUslugi { get; set; }

            [ExcelColumn("Numer Listu")]
            public string NumerListu { get; set; }

            [ExcelColumn("List pierwotny")]
            public string ListPierwotny { get; set; }

            [ExcelColumn("Uwagi")]
            public string Uwagi { get; set; }

            [ExcelColumn("Cena netto")]
            public decimal CenaNetto { get; set; }

            [ExcelColumn("Kwota")]
            public decimal Kwota { get; set; }

            [ExcelColumn("Nadanie")]
            public string Nadanie { get; set; }

            [ExcelColumn("Doręczenie")]
            public string Doreczenie { get; set; }

            [ExcelColumn("Rodzaj usługi")]
            public string RodzajUslugi { get; set; }

            [ExcelColumn("waga")]
            public decimal Waga { get; set; }

            [ExcelColumn("ilość paczek")]
            public int IloscPaczek { get; set; }

            [ExcelColumn("Nazwa odbiorcy")]
            public string NazwaOdbiorcy { get; set; }

            [ExcelColumn("platnik")]
            public int Platnik { get; set; }

            [ExcelColumn("Nr klienta")]
            public int NrKlienta { get; set; }

        }
        private bool ProcessDpdFile(string saveLocation)
        {

            try
            {


                ExcelQueryFactory eqf = new ExcelQueryFactory(saveLocation);


                var r = from p in eqf.Worksheet<DpdXlsFile>(0) select p;



                var rr = r.ToList();

                ProcessDpd(rr);
                return true;
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);

                throw ex;
                return false;
            }

        }
        private static void ProcessDpd(List<DpdXlsFile> data)
        {
            List<Dal.ShippingCompanyCost> costs = new List<Dal.ShippingCompanyCost>();

            foreach (DpdXlsFile t in data)
            {
                costs.Add(
                    new Dal.ShippingCompanyCost()
                    {
                        Cost = t.CenaNetto,
                        CostExternalId = t.DpdCostId.ToString(),
                        SendDate= t.DataNadania,
                        DeliveryDate = t.DataWykonaniaUslugi,
                        DeliveredTo = t.Doreczenie,
                        ParcelCount = t.IloscPaczek,
                        TotalCost = t.Kwota,
                        ParcelNumber2 = t.ListPierwotny,
                        SentFrom = t.Nadanie,
                        SenderName = t.NazwaNadawcy,
                        Recipent = t.NazwaOdbiorcy,
                        InvoiceNumber = t.NrFaktury,
                        ClientId = t.NrKlienta,
                        ParcelNumber = t.NumerListu,
                        Payer = t.Platnik,
                        ServiceName = t.RodzajUslugi,
                        Comments = t.Uwagi,
                        Weight = t.Waga,
                        InsertDate = DateTime.Now,
                        ShippingCompanyId = (int)Dal.Helper.ShippingCompany.Dpd

                        //Amount = decimal.Parse(t.KwotaPobrania),
                        //BatchNumber = t.NumerRozliczenia,
                        //ClientName = t.OdbiorcaPrzesylki,
                        //InsertDate = DateTime.Now,
                        //InsertUser = userName,
                        //PaymentDate = DateTime.Parse(t.DataPrzelewu),
                        //PaymentOperator = "ING",
                        //PaymentNumber = t.Opis,
                        //PaymentTypeId = 1,
                        //ShopId = shopId,
                        //Title = t.Opis.Substring(0, t.Opis.IndexOf("-")),
                        //TotalAmount = Decimal.Parse(t.ZbiorczyPrzelew)
                    });
            }
            Dal.DbHelper.Accounting.SetShippingCompanyCosts(costs);
        }
        #endregion

        protected void lbtnAssign_Click(object sender, EventArgs e)
        {

            Dal.DbHelper.Accounting.SetCourierCostsAssign();
            btnSearch_Click(null, null);
            DisplayMessage("Gotowe");
        }
    }
}
 