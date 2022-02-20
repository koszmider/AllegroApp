using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper.Configuration;
using System.Net;

namespace LajtIt.Bll
{
    public class ArgonHelper : ImportData, IImportData
    {
        public class ArgonImport
        {
            public string Nazwa { get; set; }
            public int Stany { get; set; }
            public string Kod { get; set; }
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
                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.RegisterClassMap<FooMap>();
                List<ArgonImport> records = csv.GetRecords<ArgonImport>().ToList();
                ProcessData(records);
            }

            oh.SetSupplierImportDate(SupplierId, DateTime.Now);

        }




        public sealed class FooMap : ClassMap<ArgonImport>
        {
            public FooMap()
            {
                Map(m => m.Nazwa).Index(0);
                Map(m => m.Stany).Index(1);
                Map(m => m.Kod).Index(2);
            }
        }
        public new void LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }

        public void ProcessData(List<ArgonImport> obj)
        {
            var rtr = obj.Where(x => x.Stany >= 0).ToList();

            if (rtr.Count == 0)
            {
                Bll.ErrorHandler.SendEmail("Plik Argon zwraca zero produktów, prawdp. zmieniła się struktura", Dal.Helper.DevEmail);
                return;
            }


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
                        if (r.Stany == 0)
                        {
                            pc.IsAvailable = false;
                            pc.SupplierQuantity = 0;
                        }
                        else
                        {
                            pc.IsAvailable = true;// (int)Convert.ToDecimal(r.Stany) > 0;
                            pc.SupplierQuantity = r.Stany;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Argon ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, ex.Message));

                }
            }

            string[] codeInDb = products.Select(x => x.Code).Distinct().ToArray();

            string[] codeNotInDb = obj.Where(x => !codeInDb.Contains(x.Kod) && x.Kod != "").Select(x => x.Kod).Distinct().ToArray();

            var oToAdd = obj.Where(x => codeNotInDb.Contains(x.Kod)).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach (ArgonImport ci in oToAdd)
            {
                try
                {
                    Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.Kod.Trim(), (int)Convert.ToDecimal(ci.Stany) >= 0);


                    pcToAdd.SupplierQuantity = ci.Stany;

                    if (pcsToAdd.Where(x => x.Code == pcToAdd.Code).Count() > 0)
                        continue;
                    pcsToAdd.Add(pcToAdd);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Argon ProductCatalogId: {0}, Code {1}", 0, ex.Message));

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
