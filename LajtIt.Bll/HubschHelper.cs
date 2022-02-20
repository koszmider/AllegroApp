using System;
using System.Configuration;
using System.Linq;
using System.Net;
using LinqToExcel;
using LinqToExcel.Attributes;
using System.Collections.Generic;
using System.IO;
using CsvHelper.Configuration;
using System.Text;

namespace LajtIt.Bll
{
    public class HubschHelper : ImportData //, IImportData
    { 

        public class HubschImport
        {
            public string item_number { get; set; }
            public string ean { get; set; }
            public int inventory { get; set; }
            public string i1 { get; set; }
            public string i2 { get; set; }
            public string i3 { get; set; }
            public string i4 { get; set; }
            public string i5 { get; set; }
            public string i6 { get; set; }
            public string i7 { get; set; }
            public string i8 { get; set; }
            public string i9 { get; set; }
            public string i10 { get; set; }
            public string i11 { get; set; }


        }
        public new void LoadData<T>()
        {
            T data = base.LoadData<T>();
            //ProcessData(data);
            base.PostLoadProcess();
        }
        public void GetFile()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier supplier = oh.GetSupplier(SupplierId);

            string remoteUri = supplier.ImportUrl;


            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format("{1}_{0:yyyyMMddHHss}.csv", DateTime.Now, "Hubsch");

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
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", "w"));
                return;

            }

            using (StreamReader reader = new StreamReader(saveLocation, Encoding.GetEncoding("Windows-1250")))
            {


                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.Delimiter = ",";
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.BadDataFound = null;
                csv.Configuration.RegisterClassMap<FooMap>(); 
                List<HubschImport> records = csv.GetRecords<HubschImport>().ToList();
                 

                ProcessData(records);
                //GetImages(records);

            }

      
        }

        private void ProcessData(List<HubschImport> records)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
               // .Where(x => x.IsDiscontinued == false)
                .ToList();



            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = records.Where(x => x.ean == pc.Ean).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = null;
                        
                    }
                    else
                    {
                        pc.IsAvailable = r.inventory>0;
                        pc.SupplierQuantity = r.inventory;
                        
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Hubsch ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, ex.Message));

                }
            }

            string[] codeInDb = products.Select(x => x.Ean.ToLower()).Distinct().ToArray();

            string[] codeNotInDb = records.Where(x => !codeInDb.Contains(x.ean.ToLower()) && x.ean != "")
                .Select(x => x.ean.ToLower()).Distinct().ToArray();

            var oToAdd = records.Where(x => codeNotInDb.Contains(x.ean.ToLower())).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach (HubschImport ci in oToAdd)
            {
                try
                {
                    Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.item_number.Trim(), ci.inventory>0);


                    pcToAdd.SupplierQuantity = ci.inventory;
                    pcToAdd.PriceBruttoFixed = 0;
                    pcToAdd.Ean = ci.ean;


                    if (pcsToAdd.Where(x => x.Code.ToLower() == pcToAdd.Code.ToLower()).Count() > 0)
                        continue;
                    pcsToAdd.Add(pcToAdd);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Hubsch ProductCatalogId: {0}, Code {1}", 0, ex.Message));

                }
            }


            pch.SetProductCatalogHubschUpdate(products, SupplierId);
            try
            {
                pch.SetProductCatalogs(pcsToAdd, SupplierId);
            }
            catch (Exception ex)
            {

                Bll.ErrorHandler.SendEmail(String.Format("Błąd dodawania nowych produktów Hubsch<br><br>{0}", ex.Message));
            }
            GetImages(records);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(SupplierId, DateTime.Now);
        }
        private void GetImages(List<HubschImport> records)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> productForSupplier = pch.GetProductCatalogBySupplier(new int[] { SupplierId });

            foreach (HubschImport offer in records)
            {
                string code = offer.ean;


                Dal.ProductCatalog productFromCatalog = productForSupplier.Where(x => x.Ean.ToLower() == offer.ean.ToLower())
                    .Where(x => x.ImageId.HasValue == false)
                    .FirstOrDefault();

                if (productFromCatalog == null)
                    continue;

                List<string> images = new List<string>();
                images.Add(offer.i1);
                AltavolaHelper.DownloadImage(offer.i1, productFromCatalog.ProductCatalogId);

                images.Add(offer.i2);
                images.Add(offer.i3);
                images.Add(offer.i4);
                images.Add(offer.i5);
                images.Add(offer.i6);
                images.Add(offer.i7);
                images.Add(offer.i8);
                images.Add(offer.i9);
                images.Add(offer.i10);
                images.Add(offer.i11);

                foreach (string image in images.Distinct().Where(x => x != offer.i1 && !String.IsNullOrEmpty(x)).ToArray())
                {
                    AltavolaHelper.DownloadImage(image, productFromCatalog.ProductCatalogId);
                }
            }
        }
        public sealed class FooMap : ClassMap<HubschImport>
        {
            public FooMap()
            {
                Map(m => m.inventory).Name("Inventory");
                Map(m => m.item_number).Name("Item number");
                Map(m => m.ean).Name("EAN");
                Map(m => m.i1).Name("Image URL");
                Map(m => m.i2).Name("Packshots01");
                Map(m => m.i3).Name("Close-ups01");
                Map(m => m.i4).Name("Styleshots01");
                Map(m => m.i5).Name("Styleshots02");
                Map(m => m.i6).Name("Styleshots03");
                Map(m => m.i7).Name("Styleshots04");
                Map(m => m.i8).Name("Styleshots05");
                Map(m => m.i9).Name("Styleshots06");
                Map(m => m.i10).Name("Styleshots07");
                Map(m => m.i11).Name("Styleshots08");


            }
        }
      
    }
}
