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
    public class ElsteadHelper : ImportData, IImportData
    { 
        public class ElsteadImport
        { 
            public string StockCode { get; set; }
            public string TestedStock { get; set; }
            public string EuStock { get; set; }
             

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
                csv.Configuration.Delimiter = ",";
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.BadDataFound = null;
                csv.Configuration.RegisterClassMap<FooMap>();
                List<ElsteadImport> records = csv.GetRecords<ElsteadImport>().ToList();

          
                ProcessData(records); 

            }


            oh.SetSupplierImportDate(SupplierId, DateTime.Now);

        }
 

        private void GetImage(int productCatalogId, string img)
        {
            AltavolaHelper.DownloadImage(img, productCatalogId);
        }

        public sealed class FooMap : ClassMap<ElsteadImport>
        {
            public FooMap()
            {
                Map(m => m.StockCode).Index(0);
                Map(m => m.TestedStock).Index(1);
                Map(m => m.EuStock).Index(2); 
            }
        }
        public new void  LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }

        public class Int32Converter<T> : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return Int32.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
            } 
        }


        public void ProcessData(List<ElsteadImport> obj)
        { 

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                .Where(x => x.IsDiscontinued == false)
                .ToList();


            //obj = obj.Where(x => !x.Indeks.StartsWith("A")).ToList();

            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = obj.Where(x => x.StockCode.Trim() == pc.Code.Trim()).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = null;

                    }
                    else
                    { 
                        pc.IsAvailable = Convert.ToInt32(r.TestedStock) > 0;
                        pc.SupplierQuantity = Convert.ToInt32(r.TestedStock);
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Elstead ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, pc.Code));

                }
            }
             
            string[] codeInDb = products.Select(x => x.Code).Distinct().ToArray();

            string[] eanNotInDb = obj.Where(x => !codeInDb.Contains(x.StockCode)).Select(x=>x.StockCode).Distinct().ToArray();

            var oToAdd = obj.Where(x => eanNotInDb.Contains(x.StockCode)).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach(ElsteadImport ci in oToAdd)
            {
                Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.StockCode.Trim(), Convert.ToInt32(ci.TestedStock) > 0);

                pcToAdd.Ean = null;
                pcToAdd.SupplierQuantity = Convert.ToInt32(ci.TestedStock);
                pcToAdd.PriceBruttoFixed = 0;

                if (pcsToAdd.Where(x => x.Code == pcToAdd.Code).Count()>0)
                    continue;
                pcsToAdd.Add(pcToAdd);
            }


            pch.SetProductCatalogUpdateCandellux(products, SupplierId); 
            pch.SetProductCatalogs(pcsToAdd, SupplierId);

        }

        public void ProcessData<T>(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
