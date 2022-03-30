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
    public class LucideHelper : ImportData, IImportData
    { 
        public class LucideImport
        { 
            public string Kod { get; set; }
            public string Ean { get; set; }
            public string Description { get; set; }
            public string Stock { get; set; } 

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
                int result = line.ToCharArray().Count(c => c == ';');
                if (line.Length == result)
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
                List<LucideImport> records = csv.GetRecords<LucideImport>().ToList();
                ProcessData(records); 
            }

            oh.SetSupplierImportDate(SupplierId, DateTime.Now);

        }


     

        public sealed class FooMap : ClassMap<LucideImport>
        {
            public FooMap()
            {
                Map(m => m.Kod).Index(0);
                Map(m => m.Ean).Index(1);
                Map(m => m.Description).Index(2);
                Map(m => m.Stock).Index(3);
            }
        }
        public new void  LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }

        public void ProcessData(List<LucideImport> obj)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                .Where(x => x.IsDiscontinued == false)
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
                        pc.IsAvailable = Convert.ToInt32(r.Stock) > 0;
                        pc.SupplierQuantity = Convert.ToInt32(r.Stock);
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Lucide ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, pc.Code));

                }
            }

            string[] codeInDb = products.Select(x => x.Code).Distinct().ToArray();

            string[] codeNotInDb = obj.Where(x => !codeInDb.Contains(x.Kod)).Select(x => x.Kod).Distinct().ToArray();

            var oToAdd = obj.Where(x => codeNotInDb.Contains(x.Kod)).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach (LucideImport ci in oToAdd)
            {
                Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.Kod.Trim(), Convert.ToInt32(ci.Stock) > 0);


                pcToAdd.SupplierQuantity = Convert.ToInt32(ci.Stock); 

                if (pcsToAdd.Where(x => x.Code == pcToAdd.Code).Count() > 0)
                    continue;
                pcsToAdd.Add(pcToAdd);
            }


            pch.SetProductCatalogUpdateLucide(products, SupplierId);
            pch.SetProductCatalogs(pcsToAdd, SupplierId);

        }

        public void ProcessData<T>(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
