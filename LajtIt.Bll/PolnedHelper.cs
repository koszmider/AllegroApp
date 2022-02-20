using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class PolnedHelper
    {

        int supplierId = 83;

        public class PolnedImport
        {
            public string code { get; set; }
            public decimal quantity { get; set; } 

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
                client.Credentials = new NetworkCredential(sh.GetSetting("POLNED_L").StringValue, sh.GetSetting("POLNED_P").StringValue);
                client.DownloadFile(supplier.ImportUrl, fileName);
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd wczytywania pliku: {0} ", supplier.Name));

            }
            ProcessData(fileName);
        }

        private void ProcessData(List<PolnedImport> records)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { supplierId })
                //.Where(x => x.IsDiscontinued == false)
                .ToList();



            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = records.Where(x => x.code.Trim() == pc.Code).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = null;

                    }
                    else
                    { 
                        pc.IsAvailable = (int)r.quantity>0;
                        pc.SupplierQuantity = (int)r.quantity;
                   
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Garden Light ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, ex.Message));

                }
            }

        

            pch.SetProductCatalogUpdateItalux(products, supplierId);
         
    
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
        
            using (TextReader reader = File.OpenText(fileName))
            {


                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.Delimiter = ",";
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.BadDataFound = null;
                csv.Configuration.RegisterClassMap<FooMap>();
                csv.Configuration.CultureInfo = CultureInfo.InvariantCulture; 
                List<PolnedImport> records = csv.GetRecords<PolnedImport>().ToList(); 
           
                ProcessData(records); 

            }



            Dal.OrderHelper oh = new Dal.OrderHelper();



            oh.SetSupplierImportDate(supplierId, DateTime.Now);
        }

        public sealed class FooMap : ClassMap<PolnedImport>
        {
            public FooMap()
            { 
                Map(m => m.code).Name("symbol");
                Map(m => m.quantity).Name("ilosc");
            }
        }
    }
}
