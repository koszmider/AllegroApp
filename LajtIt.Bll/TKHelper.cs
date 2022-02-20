using CsvHelper.Configuration;
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
    public class TKHelper
    {
        int supplierId = 38;
        public class TKImport
        {
            public string Code { get;  set; }
            public string EAN { get;  set; }
            public string Title { get;  set; }
            public string Quantity { get;  set; }  
            public bool IstniejeWBazie { get;  set; }  

        }

        public void LoadData()
        {

            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.SettingsHelper sh = new Dal.SettingsHelper();


            Dal.Supplier supplier = oh.GetSupplier(supplierId);
            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format(path, String.Format("{0}_{1:yyyyMMMddHHmmss}.csv", supplier.Name, DateTime.Now));
            try
            {
                WebClient client = new WebClient(); 
                client.DownloadFile(supplier.ImportUrl, fileName);
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd wczytywania pliku: {0} ", supplier.Name));

            }
            ProcessData(fileName, supplier);
        }

     

        /*
         * 
         * 
           ID,EAN,Nazwa,Pełny symbol artykułu,Promocja,Stan magazynowy,Cena detaliczna netto,VAT,Cena detaliczna brutto,Koniec serii,Data aktualizacji
         * 
         * */

 
        public TKHelper()
        { 
        }

        private void ProcessData(string fileName, Dal.Supplier supplier)
        {


            using (StreamReader reader = new StreamReader(fileName, Encoding.Unicode ))
            //using (TextReader reader = File.OpenText(@"C:\intpub\wwwlajtit\Files\ImportFiles\produkty_csv_2_04-10-2018_11_56_35_pl.csv"))
            {
                

                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.Delimiter = "|";
                csv.Configuration.RegisterClassMap<FooMap>();

                List<TKImport> records = csv.GetRecords<TKImport>().ToList();

                ProcessData(records);

            }
        }
        public sealed class FooMap : ClassMap<TKImport>
        {
            public FooMap()
            {
                Map(m => m.Code).Index(1);
                Map(m => m.EAN).Index(3);
                Map(m => m.Quantity).Index(7);
                Map(m => m.Title).Index(2);


            }
        }
        private void ProcessData(List<TKImport> ii)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { supplierId });

            foreach(Dal.ProductCatalog productFromDb in products)
            {
                TKImport productToCheck = ii.Where(x => x.EAN == productFromDb.Ean).FirstOrDefault();

                int? quantity = null;

                if (productToCheck != null)
                {

                    if (!String.IsNullOrEmpty(productToCheck.Quantity))
                        quantity = Int32.Parse(productToCheck.Quantity);

                    productFromDb.SupplierQuantity = quantity;
                    productFromDb.IsAvailable = quantity.HasValue && quantity.Value > 0;
                    productToCheck.IstniejeWBazie = true;

                    //decimal price = 0;
                    //if (Decimal.TryParse(productToCheck.CenaBrutto, out price))
                    //    productFromDb.PriceBruttoFixed = Dal.Helper.RoundValue((Dal.Helper.SupplierRoundPriceType)supplier.RoundPriceTypeId, price);
                }
                else
                {
                    productFromDb.IsAvailable = false;
                    productFromDb.SupplierQuantity = null;
                }
                productFromDb.UpdateUser = "System";
                productFromDb.UpdateReason = "Aktualizacja automatyczna";
            }

            pch.SetProductCatalogUpdateItalux(products, supplierId);

            List<TKImport> productsItaluxToAdd = ii.Where(x => x.EAN != null && x.Code != null && !x.IstniejeWBazie).ToList();

            List<Dal.ProductCatalog> productsToAdd = new List<Dal.ProductCatalog>();
            foreach(TKImport italuxToAdd in productsItaluxToAdd.Where(x=>x.Code!=""))
            {
                try
                {
                    int? quantity = null;

                    if (!String.IsNullOrEmpty(italuxToAdd.Quantity))
                        quantity = Int32.Parse(italuxToAdd.Quantity);


                    Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(supplierId, italuxToAdd.Code, quantity.HasValue && quantity.Value > 0);
                    pc.Ean = italuxToAdd.EAN.Substring(0, Math.Min(italuxToAdd.EAN.Length, 13)); ;
                    pc.Name = italuxToAdd.Title.Substring(0, Math.Min(italuxToAdd.Title.Length, 100));

                    //decimal price = 0;
                    //if (Decimal.TryParse(italuxToAdd.CenaBrutto, out price))
                    //    pc.PriceBruttoFixed = Dal.Helper.RoundValue((Dal.Helper.SupplierRoundPriceType)supplier.RoundPriceTypeId, price);



                    pc.SupplierQuantity = quantity;


                    if (pc.Ean == "")
                        pc.Ean = null;
                    productsToAdd.Add(pc);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, "");
                }
            }

            pch.SetProductCatalogsByEan(productsToAdd, supplierId);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(supplierId, DateTime.Now);
        }
    }
}
