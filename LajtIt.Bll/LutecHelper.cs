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
    public class LutecHelper : ImportData, IImportData
    { 
        public class LutecImport
        { 
            public string Kod { get; set; }
            public string NrKat { get; set; }
            public string Nazwa { get; set; }
            public string Dostepnosc { get; set; } 

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



            using (TextReader reader = File.OpenText(saveLocation)) 
            {


                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<FooMap>();
                // List<CandelluxImport> records = csv.GetRecords<CandelluxImport>().ToList();
                List<LutecImport> records = csv.GetRecords<LutecImport>().ToList();
                ProcessData(records); 
            }

            oh.SetSupplierImportDate(SupplierId, DateTime.Now);

        }


     

        public sealed class FooMap : ClassMap<LutecImport>
        {
            public FooMap()
            {
                Map(m => m.Kod).Index(0);
                Map(m => m.NrKat).Index(1);
                Map(m => m.Nazwa).Index(2);
                Map(m => m.Dostepnosc).Index(3);
            }
        }
        public new void  LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }

        public void ProcessData(List<LutecImport> obj)
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
                        pc.IsAvailable = (int)Convert.ToDecimal(r.Dostepnosc)  > 0;
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Lutec ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, ex.Message));

                }
            }

            string[] codeInDb = products.Select(x => x.Code).Distinct().ToArray();

            string[] codeNotInDb = obj.Where(x => !codeInDb.Contains(x.Kod) && x.Kod!="" && x.Dostepnosc!="").Select(x => x.Kod).Distinct().ToArray();

            var oToAdd = obj.Where(x => codeNotInDb.Contains(x.Kod)).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach (LutecImport ci in oToAdd)
            {
                try
                {
                    Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.Kod.Trim(), (int)Convert.ToDecimal(ci.Dostepnosc) > 0);


                    pcToAdd.SupplierQuantity = (int)Convert.ToDecimal(ci.Dostepnosc);

                    if (pcsToAdd.Where(x => x.Code == pcToAdd.Code).Count() > 0)
                        continue;
                    pcsToAdd.Add(pcToAdd);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Lutec ProductCatalogId: {0}, Code {1}",0, ex.Message));

                }
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
