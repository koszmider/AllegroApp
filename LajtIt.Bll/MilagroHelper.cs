using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LajtIt.Bll
{
    public class MilagroHelper
    {
        private int SupplierId = 13;

        public class MilagroImport
        {
                public string ean{get;set;}
                public string id{get;set;}
                public string sku{get;set;}
                public string name{get;set;}
                public string model{get;set;}
                public string desc{get;set;}
                public string url{get;set;}
                public string photo{get;set;}
                public string photo1{get;set;}
                public string photo2{get;set;}
                public string photo3{get;set;}
                public string Kategoria{get;set;}
                public string unit{get;set;}
                public string weight{get;set;}
                public string PKWiU{get;set;}
                public string inStock{get;set;}
                public string qty{get;set;}
                public string availability{get;set;}
                public string requiredBox{get;set;}
                public string quantityPerBox{get;set;}
                public string priceAfterDiscountNet{get;set;}
                public string vat{get;set;}
                public string retailPriceGross { get; set; }
                public bool IsAvailable { get { return inStock == "True"; } }
                public bool IstniejeWBazie { get; set; }

        }

        // ean;id;sku;name;model;desc;url;photo;photo1;photo2;photo3;Kategoria;unit;weight;PKWiU;inStock;qty;availability;requiredBox;quantityPerBox;priceAfterDiscountNet;vat;retailPriceGross
        public MilagroHelper()
        {
           
        }

        private void ProcessData(List<MilagroImport> milagro)
        {
            //string[] names = new string[] {""}
            // odfiltrować produkty inne niż oświetlenie
            List<MilagroImport> ii = new List<MilagroImport>();

            ii.AddRange(milagro.Where(x => x.Kategoria.ToUpper().Contains("OŚWIETLENIE")).ToList());
            ii.AddRange(milagro.Where(x => x.Kategoria.ToUpper().Contains("ŹRÓDŁA ŚWIATŁA")).ToList());
            ii.AddRange(milagro.Where(x => x.Kategoria.ToUpper().Contains("PROMOCJA")).ToList());
            ii.AddRange(milagro.Where(x => x.Kategoria.ToUpper().Contains("OUTLET")).ToList());
            ii.AddRange(milagro.Where(x => x.Kategoria.ToUpper().Contains("NOWOŚCI")).ToList());
            ii.AddRange(milagro.Where(x => x.Kategoria.ToUpper().Contains("WENTYLATORY")).ToList());


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId });

            int available = products.Where(x => x.IsAvailable).Count();

            List<MilagroImport> images = ii.Where(x => x.photo != null && x.photo != "").ToList();

            foreach (Dal.ProductCatalog productFromDb in products)
            {
                MilagroImport productToCheck = ii.Where(x => x.ean == productFromDb.Ean).FirstOrDefault();

                if (productToCheck != null)
                {
                    productFromDb.IsAvailable = productToCheck.IsAvailable;
                    productToCheck.IstniejeWBazie = true;
                    productFromDb.PriceBruttoFixed = Convert.ToDecimal(productToCheck.retailPriceGross);//.Replace(",", "."));
                    productFromDb.PurchasePrice = Convert.ToDecimal(productToCheck.priceAfterDiscountNet);//.Replace(",", "."));
                    productFromDb.UpdateUser = "System";
                    productFromDb.UpdateReason = "Aktualizacja automatyczna";
                    int qty = -1;
                    if(Int32.TryParse(productToCheck.qty, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out qty))
                        productFromDb.SupplierQuantity = qty;
                    else

                        productFromDb.SupplierQuantity = null;
                }
                else
                { 
                    productFromDb.IsAvailable = false;
                    productFromDb.SupplierQuantity = null;

                }
            }

            int available2 = products.Where(x => x.IsAvailable).Count();


            if (available == 0 || available2 / available < 0.05) //zabezpieczenie jakby cala oferta miala byc wykasowana
                return;


            pch.SetProductCatalogUpdateStatusPrice(products, SupplierId);

            List<MilagroImport> productsMilagroToAdd = ii.Where(x => x.ean != null && x.sku != null && !x.IstniejeWBazie).Distinct().ToList();

            List<Dal.ProductCatalog> productsToAdd = new List<Dal.ProductCatalog>();
            foreach (MilagroImport MilagroToAdd in productsMilagroToAdd)
            {
                Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, MilagroToAdd.sku, MilagroToAdd.IsAvailable);
                pc.Ean = MilagroToAdd.ean.Substring(0, Math.Min(MilagroToAdd.ean.Length, 13)); ;
                pc.Name = MilagroToAdd.name.Substring(0, Math.Min(MilagroToAdd.name.Length, 100));
                pc.PriceBruttoFixed = Convert.ToDecimal(MilagroToAdd.retailPriceGross);//.Replace(",", "."));
                pc.PurchasePrice = Convert.ToDecimal(MilagroToAdd.priceAfterDiscountNet);//.Replace(",", "."));
                int qty = -1;
                if (Int32.TryParse(MilagroToAdd.qty, out qty))
                    pc.SupplierQuantity = qty;
                else

                    pc.SupplierQuantity = qty;
                if (String.IsNullOrEmpty(pc.Ean))
                    pc.Ean = null;
                productsToAdd.Add(pc);
            }

            pch.SetProductCatalogs(productsToAdd, SupplierId);

            List<Dal.ProductCatalog> addedProducts = pch.GetProductCatalogBySupplier(new int[] { SupplierId }).Where(x=>x.ImageId==null).ToList();


            foreach (MilagroImport milagroProduct in ii)
            {
                Dal.ProductCatalog existingProduct = addedProducts.Where(x => x.Ean == milagroProduct.ean).FirstOrDefault();

                if (existingProduct == null)
                    continue;


                string[] photos = new string[]
                {
                    milagroProduct.photo,
                    milagroProduct.photo1,
                    milagroProduct.photo2,
                    milagroProduct.photo3
                };

                foreach (string photo in photos)
                {
                    if (String.IsNullOrEmpty(photo))
                        continue;

                    AltavolaHelper.DownloadImage(photo, existingProduct.ProductCatalogId);
                }

            }


            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(SupplierId, DateTime.Now);

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
                    myWebClient.Credentials = new NetworkCredential("sprzedaz@lajtit.pl", "mi852ARA&");
                    myWebClient.DownloadFile(remoteUri, saveLocation);
                }
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", supplier.Name));
                return;

            }



            using (TextReader reader = File.OpenText(saveLocation))
             //using (TextReader reader = File.OpenText(@"C:\inetpub\wwwlajtit\Files\ImportFiles\produkty_csv_2_04-10-2018_11_56_35_pl.csv"))
            {


                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.Delimiter = ";";

                List<MilagroImport> records = csv.GetRecords<MilagroImport>().ToList();

                ProcessData(records);

            }
        }
    }
}
