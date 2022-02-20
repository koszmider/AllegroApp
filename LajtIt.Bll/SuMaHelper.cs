using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
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
    public class SuMaHelper : ImportData, IImportData
    { 
        public class SuMaImport
        { 
            public string Kod { get; set; }
            public string Ean { get; set; }
            public string Nazwa { get; set; }
            public int Stany { get; set; } 

        }
        internal void GetFile()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier supplier = oh.GetSupplier(SupplierId);

            string remoteUri = supplier.ImportUrl;


            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format("{1}_{0:yyyyMMddHHss}.csv", DateTime.Now, supplier.Name);

            string saveLocation = String.Format(path, fileName);

            try
            {

                // Create a new WebClient instance.
                using (WebClient myWebClient = new WebClient())
                { 
                    myWebClient.DownloadFile(remoteUri, saveLocation);
                }
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", supplier.Name));
                return;

            }

            int cnt = 0;
            var lines = File.ReadAllLines(saveLocation);
            foreach (String line in lines)
            {
                if (line.Equals(";;;"))
                    lines[cnt] = "";
                cnt++;
            }
            File.WriteAllLines(saveLocation, lines);

            using (TextReader reader = File.OpenText(saveLocation)) 
            {

                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);                
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<FooMap>();
                // List<CandelluxImport> records = csv.GetRecords<CandelluxImport>().ToList();
                List<SuMaImport> records = csv.GetRecords<SuMaImport>().ToList();
                ProcessData(records); 
            }

            oh.SetSupplierImportDate(SupplierId, DateTime.Now);

        }


     

        public sealed class FooMap : ClassMap<SuMaImport>
        {
            public FooMap()
            {
                Map(m => m.Ean).Index(0);
                Map(m => m.Kod).Index(1);
                Map(m => m.Nazwa).Index(2);
                Map(m => m.Stany).Index(3);
            }
        }
        public new void  LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }

        public void ProcessData(List<SuMaImport> obj)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
           
                .ToList();

             

            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = obj.Where(x => x.Kod == pc.Code).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = 0;

                    }
                    else
                    {
                        pc.IsAvailable = r.Stany  > 0;
                        pc.SupplierQuantity = r.Stany;
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu SU-MA ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, ex.Message));

                }
            }

            string[] EanInDb = products.Select(x => x.Ean).Distinct().ToArray();

            string[] EanNotInDb = obj.Where(x => !EanInDb.Contains(x.Ean) && x.Ean!="").Select(x => x.Ean).Distinct().ToArray();

            var oToAdd = obj.Where(x => EanNotInDb.Contains(x.Ean)).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach (SuMaImport ci in oToAdd)
            {
                try
                {
                    Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.Kod.Trim(), ci.Stany > 0);

                    pcToAdd.Ean = ci.Ean;
                    pcToAdd.SupplierQuantity = ci.Stany;

                    if (pcsToAdd.Where(x => x.Ean == pcToAdd.Ean).Count() > 0)
                        continue;
                    pcsToAdd.Add(pcToAdd);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu SU-MA ProductCatalogId: {0}, Code {1}",0, ex.Message));

                }
            }


            pch.SetProductCatalogUpdateLucide(products, SupplierId);
            pch.SetProductCatalogsByEan(pcsToAdd, SupplierId);

        }

        public void ProcessData<T>(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
