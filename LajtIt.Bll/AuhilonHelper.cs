using System;
using System.Configuration;
using System.Linq;
using System.Net;
using LinqToExcel;
using LinqToExcel.Attributes;
using System.Collections.Generic;
using System.IO;
using CsvHelper.Configuration;

namespace LajtIt.Bll
{
    public class AuhilonHelper : ImportData//, IImportData
    {
        int supplierId = 37;

        public class AuhilonImport
        {
            public string product_code { get; set; }
            public bool active { get; set; }
            public string name { get; set; }
            public string price { get; set; }
            public string vat { get; set; }
            public string unit { get; set; }
            public string category { get; set; }
            public string producer { get; set; }
            public int stock { get; set; }
            public string stock_warnlevel { get; set; }
            public string availability { get; set; }
            public string images_1 { get; set; }
            public string images_2 { get; set; }
            public string images_3 { get; set; }
            public string images_4 { get; set; }
            public string images_5 { get; set; }
            public string images_6 { get; set; }
            public string images_7 { get; set; }
            public string images_8 { get; set; }
            public string images_9 { get; set; }
            public string images_10 { get; set; }
            public string barcode { get; set; }

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
            Dal.Supplier supplier = oh.GetSupplier(supplierId);

            string remoteUri = supplier.ImportUrl;


            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format("{1}_{0:yyyyMMddHHss}.csv", DateTime.Now, "Auhilon");

            string saveLocation = String.Format(path, fileName);

            try
            {

                Dal.SettingsHelper sh = new Dal.SettingsHelper();
                Dal.Settings sl = sh.GetSetting("AUHILON_L");
                Dal.Settings sp = sh.GetSetting("AUHILON_P");



                // Create a new WebClient instance.
                using (WebClient myWebClient = new WebClient())
                {
                    myWebClient.Credentials = new NetworkCredential(sl.StringValue, sp.StringValue);
                    myWebClient.DownloadFile(remoteUri, saveLocation);
                }
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", "w"));
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
                List<AuhilonImport> records = csv.GetRecords<AuhilonImport>().ToList();

                //// odkąd candellux podaje równiez Apeti trzeba te kody wywalic
                //if (SupplierId == 50)
                //    records = records.Where(x => !x.Indeks.StartsWith("A")).ToList();

                ProcessData(records);
                //GetImages(records);

            }

      
        }

        private void ProcessData(List<AuhilonImport> records)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                //.Where(x => x.IsDiscontinued == false)
                .ToList();



            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = records.Where(x => x.product_code == pc.Code).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = 0;
                        
                    }
                    else
                    {
                        pc.IsAvailable = r.stock>0;
                        pc.SupplierQuantity = r.stock;
                        pc.PriceBruttoFixed = Decimal.Parse(r.price, System.Globalization.CultureInfo.InvariantCulture);
                        if (!String.IsNullOrEmpty(r.barcode))
                            pc.Ean = r.barcode;
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Auhilon ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, ex.Message));

                }
            }

            string[] codeInDb = products.Select(x => x.Code.ToLower()).Distinct().ToArray();

            string[] codeNotInDb = records.Where(x => !codeInDb.Contains(x.product_code.ToLower()) && x.product_code != "").Select(x => x.product_code.ToLower()).Distinct().ToArray();

            var oToAdd = records.Where(x => codeNotInDb.Contains(x.product_code.ToLower())).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach (AuhilonImport ci in oToAdd)
            {
                try
                {
                    Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.product_code.Trim(), ci.stock>0);


                    pcToAdd.SupplierQuantity = ci.stock;
                    pcToAdd.PriceBruttoFixed = Decimal.Parse(ci.price, System.Globalization.CultureInfo.InvariantCulture);
                    pcToAdd.Ean = ci.barcode;


                    if (pcsToAdd.Where(x => x.Code.ToLower() == pcToAdd.Code.ToLower()).Count() > 0)
                        continue;
                    pcsToAdd.Add(pcToAdd);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Auhilon ProductCatalogId: {0}, Code {1}", 0, ex.Message));

                }
            }


            pch.SetProductCatalogAuhilonUpdateByCeneoFile(products, SupplierId);
      
            GetImages(records);
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(SupplierId, DateTime.Now);
        }
        private void GetImages(List<AuhilonImport> records)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> productForSupplier = pch.GetProductCatalogBySupplier(new int[] { SupplierId });

            foreach (AuhilonImport offer in records)
            {
                string code = offer.product_code;


                Dal.ProductCatalog productFromCatalog = productForSupplier.Where(x => x.Code.ToLower() == offer.product_code.ToLower())
                    .Where(x => x.ImageId.HasValue == false)
                    .FirstOrDefault();

                if (productFromCatalog == null)
                    continue;

                List<string> images = new List<string>();
                images.Add(offer.images_1);
                AltavolaHelper.DownloadImage(offer.images_1, productFromCatalog.ProductCatalogId);

                images.Add(offer.images_2);
                images.Add(offer.images_3);
                images.Add(offer.images_4);
                images.Add(offer.images_5);
                images.Add(offer.images_6);
                images.Add(offer.images_7);
                images.Add(offer.images_8);
                images.Add(offer.images_9);
                images.Add(offer.images_10); 

                foreach (string image in images.Distinct().Where(x => x != offer.images_1 && !String.IsNullOrEmpty(x)).ToArray())
                {
                    AltavolaHelper.DownloadImage(image, productFromCatalog.ProductCatalogId);
                }
            }
        }
        public sealed class FooMap : ClassMap<AuhilonImport>
        {
            public FooMap()
            {
                Map(m => m.images_1).Name("images 1");
                Map(m => m.images_2).Name("images 2");
                Map(m => m.images_3).Name("images 3");
                Map(m => m.images_4).Name("images 4");
                Map(m => m.images_5).Name("images 5");
                Map(m => m.images_6).Name("images 6");
                Map(m => m.images_7).Name("images 7");
                Map(m => m.images_8).Name("images 8");
                Map(m => m.images_9).Name("images 9");
                Map(m => m.images_10).Name("images 10");
                Map(m => m.product_code).Name("product_code");
                Map(m => m.active).Name("active");
                Map(m => m.name).Name("name");
                Map(m => m.price).Name("price");
                Map(m => m.vat).Name("vat");
                Map(m => m.unit).Name("unit");
                Map(m => m.category).Name("category");
                Map(m => m.producer).Name("producer");
                Map(m => m.stock).Name("stock");
                Map(m => m.stock_warnlevel).Name("stock_warnlevel");
                Map(m => m.availability).Name("availability");
                Map(m => m.barcode).Name("barcode");


            }
        }
    

        public AuhilonHelper()
        {
        }

        /*
         LP	K	Tech01	FOTO	RODZAJ OPRAWY	NAZWA	MODEL	EAN	INFORMACJE DODATKOWE	DOSTĘPNOŚĆ	OSTATNIE ZMIANY STANÓW MAGAZYNOWYCH	UWAGI

            */

      
    }
}
