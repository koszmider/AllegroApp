using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class RabaluxHelper
    {

        int supplierId = 79;

        public class RabaluxImport
        {
            public string ean { get; set; }
            public string code { get; set; }
            public string availability { get; set; }
            public string next { get; set; }

        }
        public void LoadData()
        {

            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.SettingsHelper sh = new Dal.SettingsHelper();


            Dal.Supplier supplier = oh.GetSupplier(supplierId);
            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format(path, String.Format("{0}_{1:yyyyMMMddHHmmss}.csv", supplier.Name, DateTime.Now));
            try
            {
                WebClient client = new WebClient();
                //client.Credentials = new NetworkCredential(sh.GetSetting("MARKS_L").StringValue, sh.GetSetting("MARKS_P").StringValue);
                client.DownloadFile(supplier.ImportUrl, fileName);
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd wczytywania pliku: {0} ", supplier.Name));

            }
            ProcessData(fileName);
        }

        private void ProcessData(List<RabaluxImport> records)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { supplierId })
                //.Where(x => x.IsDiscontinued == false)
                .ToList();



            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = records.Where(x => x.ean == pc.Ean/* && x.code != "6258" && x.code != "6260" && x.code != "6259" && x.code != "1743"*/).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = null;

                    }
                    else
                    {
                        if (r.availability.Equals("nem elérhető") || r.availability.Equals("not available"))
                            r.availability = "0";
                        pc.IsAvailable = r.availability != "0";
                        pc.SupplierQuantity = GetQuantity( r.availability);
                        //pc.PriceBruttoFixed = Decimal.Parse(r.price, System.Globalization.CultureInfo.InvariantCulture);
                        //pc.Ean = r.barcode;
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Auhilon ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, ex.Message));

                }
            }

            string[] codeInDb = products.Where(x=>x.Ean!=null).Select(x => x.Ean.ToLower()).Distinct().ToArray();

            string[] codeNotInDb = records.Where(x => !codeInDb.Contains(x.ean.ToLower()) && x.ean != "").Select(x => x.ean.ToLower()).Distinct().ToArray();

            var oToAdd = records.Where(x => codeNotInDb.Contains(x.ean.ToLower())).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach (RabaluxImport ci in oToAdd)
            {
                try
                {
                    Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(supplierId, 
                        ci.code.Trim(), ci.availability!="0");


                    pcToAdd.SupplierQuantity = GetQuantity( ci.availability);
                   // pcToAdd.PriceBruttoFixed = Decimal.Parse(ci.price, System.Globalization.CultureInfo.InvariantCulture);
                    pcToAdd.Ean = ci.ean;


                    if (pcsToAdd.Where(x => x.Ean.ToLower() == pcToAdd.Ean.ToLower()).Count() > 0)
                        continue;
                    pcsToAdd.Add(pcToAdd);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Rabalux ProductCatalogId: {0}, Code {1}", 0, ex.Message));

                }
            }


            pch.SetProductCatalogUpdateItalux(products, supplierId);
            try
            {
                pch.SetProductCatalogs(pcsToAdd, supplierId);
            }
            catch (Exception ex)
            {

                Bll.ErrorHandler.SendEmail(String.Format("Błąd dodawania nowych produktów Rabalux<br><br>{0}", ex.Message));
            }
    
        }

        private int? GetQuantity(string availability)
        {
            int quantity = 0;
            if (Int32.TryParse(availability, out quantity))
                return quantity;

            else
                return null;
        }

        public void ProcessData(string fileName)
         {
        //    Dictionary<string, int> markslojd = new Dictionary<string, int>();


        //    foreach (string line in File.ReadAllLines(fileName).Skip(1))
        //    {
        //        string[] fields = line.Split('|');
        //        markslojd.Add(fields[1], Convert.ToInt32(fields[2]));
        //    }



        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

        //    pch.SetProductCatalogUpdateMarkslojd(supplierId, markslojd);

            using (TextReader reader = File.OpenText(fileName))
            {


                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<FooMap>();
                // List<CandelluxImport> records = csv.GetRecords<CandelluxImport>().ToList();
                List<RabaluxImport> records = csv.GetRecords<RabaluxImport>().ToList();

                //// odkąd candellux podaje równiez Apeti trzeba te kody wywalic
                //if (SupplierId == 50)
                //    records = records.Where(x => !x.Indeks.StartsWith("A")).ToList();

                ProcessData(records);
                //GetImages(records);

            }



            Dal.OrderHelper oh = new Dal.OrderHelper();



            oh.SetSupplierImportDate(supplierId, DateTime.Now);
        }

        public sealed class FooMap : ClassMap<RabaluxImport>
        {
            public FooMap()
            {
                Map(m => m.ean).Name("EAN code");
                Map(m => m.code).Name("article number");
                Map(m => m.availability).Name("availability");
                Map(m => m.next).Name("next expected arrival");

            }
        }
    }
}
