using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class BrilliantHelper : ImportData, IImportData
    {
        public class BrilliantImport
        {
            private int ilosc;
            public string Kod { get; set; }
            public string Ean { get; set; } 
            public string Ilosc1
            {
                get { return ilosc.ToString(); }
                set
                {

                    try
                    {
                        if (String.IsNullOrEmpty(value.Trim()))
                            ilosc = 0;
                        else
                            ilosc = Int32.Parse(value.Replace(" ", ""));
                    }
                    catch (Exception ex)
                    {
                        ilosc = 0;
                    }
                }
            }

            public int Ilosc { get { return ilosc; } }

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
                csv.Configuration.BadDataFound = null;
                // List<CandelluxImport> records = csv.GetRecords<CandelluxImport>().ToList();
                List<BrilliantImport> records = csv.GetRecords<BrilliantImport>().ToList();
   
                ProcessData(records); 
            }

            oh.SetSupplierImportDate(SupplierId, DateTime.Now);

        }


     

        public sealed class FooMap : ClassMap<BrilliantImport>
        {
            public FooMap()
            {
                Map(m => m.Kod).Name("ArtNo");
                Map(m => m.Ean).Name("EAN");
                Map(m => m.Ilosc1).Name("Quantity");
                //Map(m => m.Kod).Index(0);
                //Map(m => m.Ean).Index(2);
               // Map(m => m.Ilosc1).Index(3);
            }
        }
        public new void  LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }

        public void ProcessData(List<BrilliantImport> obj)
        {
            var rtr = obj.Where(x => x.Ilosc > 0).ToList();

            if(rtr.Count==0)
            {
                Bll.ErrorHandler.SendEmail("Plik Brilliant zwraca zero produktów, prawdp. zmieniła się struktura", Dal.Helper.DevEmail);
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
                        pc.IsAvailable = (int)Convert.ToDecimal(r.Ilosc) > 0;
                        pc.SupplierQuantity = r.Ilosc;
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Brilliant ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, ex.Message));

                }
            }

            string[] codeInDb = products.Select(x => x.Code).Distinct().ToArray();

            string[] codeNotInDb = obj.Where(x => !codeInDb.Contains(x.Kod) && x.Kod!="" ).Select(x => x.Kod).Distinct().ToArray();

            var oToAdd = obj.Where(x => codeNotInDb.Contains(x.Kod)).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach (BrilliantImport ci in oToAdd)
            {
                try
                {
                    Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.Kod.Trim(), (int)Convert.ToDecimal(ci.Ilosc) > 0);


                    pcToAdd.SupplierQuantity = ci.Ilosc;

                    if (pcsToAdd.Where(x => x.Code == pcToAdd.Code).Count() > 0)
                        continue;
                    pcsToAdd.Add(pcToAdd);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Brilliant ProductCatalogId: {0}, Code {1}",0, ex.Message));

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
