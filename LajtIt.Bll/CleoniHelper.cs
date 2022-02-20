using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CsvHelper.Configuration;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public class CleoniHelper
    {
        public new void LoadData()
        {
            GetFile();
        }

        internal void GetFile()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier supplier = oh.GetSupplier(23);

            string remoteUri = supplier.ImportUrl;


            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format("{1}_{0:yyyyMMddHHss}.csv", DateTime.Now, supplier.Name);

            string saveLocation = String.Format(path, fileName);

            try
            {

                // Create a new WebClient instance.
                using (WebClient myWebClient = new WebClient())
                {
                    myWebClient.DownloadFile("https://static.lajtit.pl/CLEONI11.csv", saveLocation);
                }
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", supplier.Name));
                return;

            }

             

            using (TextReader reader = File.OpenText(saveLocation))
            {


                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.HasHeaderRecord = false;
                // List<ZumalineImport> records = csv.GetRecords<ZumalineImport>().ToList();
                List<CleoniImport> records = csv.GetRecords<CleoniImport>().ToList();
               
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                List<Dal.ProductCatalog> pc = pch.GetProductCatalogBySupplier(new int[] { 23 });
                GetImages(records, pc);

            }
            oh.SetSupplierImportDate(23, DateTime.Now);
        }
        public sealed class FooMap : ClassMap<CleoniImport>
        {
            public FooMap()
            {
                Map(m => m.Code).Index(0);
                Map(m => m.Url).Index(1);
            }
        }
        public class CleoniImport
        { 
            public string Code { get; set; }
            public string Url { get; set; } 

        }
        private void GetImages(List<CleoniImport> products, List<Dal.ProductCatalog> pc)
        {
            foreach (CleoniImport product in products)
            {
                string code = product.Code;
                if (String.IsNullOrEmpty(code))
                    continue;

                Dal.ProductCatalog productFromCatalog = pc.Where(x => x.Code == code)
                    .Where(x => x.ImageId.HasValue == false)
                    .FirstOrDefault();

                if (productFromCatalog == null)
                    continue;

                GetImage(productFromCatalog.ProductCatalogId, product.Url);


            }
        }

        private void GetImage(int productCatalogId, string img)
        {
            AltavolaHelper.DownloadImage(img, productCatalogId);
        }

      


    }


}



