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
    public class TrioRLHelper : ImportData, IImportData
    {
        public class TrioRLImport
        {
            public string Code { get; set; }
            public string Ean { get; set; }
            public int Lifecycle { get; set; }
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
                csv.Configuration.RegisterClassMap<FooMap>();
                // List<TrioRLImport> records = csv.GetRecords<TrioRLImport>().ToList();
                List<TrioRLImport> records = csv.GetRecords<TrioRLImport>().ToList();

                if (SupplierId == 70)
                    records = records.Where(x => !x.Code.StartsWith("R")).ToList();
                if (SupplierId == 74)
                    records = records.Where(x => x.Code.StartsWith("R")).ToList();

                ProcessData(records);

            }


            oh.SetSupplierImportDate(SupplierId, DateTime.Now);

        }



        public sealed class FooMap : ClassMap<TrioRLImport>
        {
            public FooMap()
            {
                Map(m => m.Code).Index(0);
                Map(m => m.Ean).Index(1);
                Map(m => m.Lifecycle).Index(2);
                Map(m => m.Quantity).Index(3);

            }
        }
        public new void LoadData<T>()
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


        public void ProcessData(List<TrioRLImport> obj)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                .Where(x => x.IsDiscontinued == false)
                .ToList();

            //foreach (Dal.ProductCatalog pc in products)
            //{
            //    try
            //    {
            //        var r = obj.Where(x => x.Code == pc.Code).FirstOrDefault();

            //        if (r == null)
            //        {
            //            pc.IsAvailable = false;
            //            pc.SupplierQuantity = 0;

            //        }
            //        else
            //        {
            //            pc.IsAvailable = r.Quantity > 0;
            //            pc.SupplierQuantity = r.Quantity;
            //            //pc.Ean = r.Ean;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu TrioRL ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, pc.Code));

            //    }
            //}

            int available = products.Where(x => x.IsAvailable).Count();

            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = obj.Where(x => x.Ean == pc.Ean).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = 0;

                    }
                    else
                    {
                        pc.IsAvailable = r.Quantity > 0;
                        pc.SupplierQuantity = r.Quantity;
                        pc.Ean = r.Ean;
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu TrioRL ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, pc.Code));

                }
            }


            int available2 = products.Where(x => x.IsAvailable).Count();

            if (available == 0 || available2 / available < 0.05)
                return;

            string[] eanInDb = products.Select(x => x.Ean).Distinct().ToArray();

            if (eanInDb.Length == 0)
                return;

            string[] eanNotInDb = obj.Where(x => !eanInDb.Contains(x.Ean)).Select(x=>x.Ean).Distinct().ToArray();

            var oToAdd = obj.Where(x => eanNotInDb.Contains(x.Ean)).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach(TrioRLImport ci in oToAdd)
            {
                Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.Code.Trim(), Convert.ToInt32(ci.Quantity) > 0);

                pcToAdd.Ean = ci.Ean;
                pcToAdd.SupplierQuantity = Convert.ToInt32(ci.Quantity);

                if (pcsToAdd.Where(x => x.Code == pcToAdd.Code).Count()>0)
                    continue;
                pcsToAdd.Add(pcToAdd);
            }


            pch.SetProductCatalogUpdateTrioRL(products, SupplierId); 
            pch.SetProductCatalogs(pcsToAdd, SupplierId);

        }

        public void ProcessData<T>(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
