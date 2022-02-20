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
    public class GlasbergHelper : ImportData, IImportData
    { 
        public class SupplierImport
        { 
            public string Code { get; set; }
            //public string Ean { get; set; }
            public int Quantity { get; set; } 

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
                csv.Configuration.BadDataFound = null;
                csv.Configuration.RegisterClassMap<FooMap>();
                // List<CandelluxImport> records = csv.GetRecords<CandelluxImport>().ToList();
                List<SupplierImport> records = csv.GetRecords<SupplierImport>().ToList();
                ProcessData(records); 
            }

            oh.SetSupplierImportDate(SupplierId, DateTime.Now);
            oh.SetSupplierImportDate(85, DateTime.Now);
            oh.SetSupplierImportDate(87, DateTime.Now);
            oh.SetSupplierImportDate(88, DateTime.Now);
            oh.SetSupplierImportDate(89, DateTime.Now);

        }


     

        public sealed class FooMap : ClassMap<SupplierImport>
        {
            //public FooMap()
            //{
            //    Map(m => m.Code).Name("Sku");
            //    Map(m => m.Ean).Name("ean");
            //    Map(m => m.Quantity).Name("Qty");
            //}
            public FooMap()
            {
                Map(m => m.Code).Name("sku");
                //Map(m => m.Ean).Name("ean");
                Map(m => m.Quantity).Name("qty");
            }
        }
        public new void  LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }

        public void ProcessData(List<SupplierImport> obj)
        {
            int supplierOwnerId = 74;
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplierOwner(supplierOwnerId)
           
                .ToList();

             

            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    //var r = obj.Where(x => x.Ean == pc.Ean).FirstOrDefault();
                    var r = obj.Where(x => x.Code == pc.Code).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = null;

                    }
                    else
                    {
                        pc.IsAvailable = r.Quantity  > 0;
                        pc.SupplierQuantity = r.Quantity;
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Glasberg ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, ex.Message));

                }
            }

            //string[] eanIdDb = products.Select(x => x.Ean).Distinct().ToArray();

            //string[] eanNotIdDb = obj.Where(x => !eanIdDb.Contains(x.Ean) && x.Ean != "" ).Select(x => x.Ean).Distinct().ToArray();

            //var oToAdd = obj.Where(x => eanNotIdDb.Contains(x.Ean)).ToList();


            string[] eanIdDb = products.Select(x => x.Code).Distinct().ToArray();

            string[] eanNotIdDb = obj.Where(x => !eanIdDb.Contains(x.Code) && x.Code != "").Select(x => x.Code).Distinct().ToArray();

            var oToAdd = obj.Where(x => eanNotIdDb.Contains(x.Code)).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach (SupplierImport ci in oToAdd)
            {
                try
                {
                    Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.Code.Trim(),ci.Quantity > 0);


                    pcToAdd.SupplierQuantity = ci.Quantity;

                    //if (pcsToAdd.Where(x => x.Ean == pcToAdd.Ean).Count() > 0)
                    if (pcsToAdd.Where(x => x.Code == pcToAdd.Code).Count() > 0)
                        continue;
                    pcsToAdd.Add(pcToAdd);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Glasberg ProductCatalogId: {0}, Code {1}",0, ex.Message));

                }
            }


            pch.SetProductCatalogUpdateGlasberg(products, supplierOwnerId);
           // pch.SetProductCatalogsBySupplierOwner(pcsToAdd, supplierOwnerId);

        }

        public void ProcessData<T>(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
