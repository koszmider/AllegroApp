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
    public class ItaluxHelper
    {
        int supplierId = 22;
        public class ItaluxImport
        {
            public string ID { get;  set; }
            public string EAN { get;  set; }
            public string Nazwa { get;  set; }
            public string Symbol { get;  set; }
            public string Promocja { get;  set; }
            public int? Stan { get;  set; }
            public string CenaNetto { get;  set; }
            public string CenaBrutto { get;  set; }
            public string KoniecSerii { get;  set; }
          //  public string DataAktualizacji { get;  set; }
            public bool Dostepny { get; set; }
            public bool IstniejeWBazie { get; set; }

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


        public ItaluxHelper(string fileName)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper(); 

            Dal.Supplier supplier = oh.GetSupplier(supplierId);
            ProcessData(fileName, supplier);
        }
        public ItaluxHelper()
        { 
        }

        private void ProcessData(string fileName, Dal.Supplier supplier)
        {
            using (TextReader reader = File.OpenText(fileName))
            {


                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                List<ItaluxImport> ii = new List<ItaluxImport>();
                //csv.BadDataFound = false;
                csv.Configuration.BadDataFound = null;
                csv.Configuration.Delimiter = ",";
                csv.Configuration.HasHeaderRecord = true;
                csv.Read();
                while (csv.Read())
                {
                    try
                    {
                        int? stan = Convert.ToInt32(csv[5].Replace(",00", ""));
                        int? s = !String.IsNullOrEmpty(csv[5]) ? stan : null;
                        if (ii.Where(x => x.Symbol.Trim() == csv[3].Trim()).Count() == 0)
                            ii.Add(
                                new ItaluxImport()
                                {
                                    ID = csv[0],
                                    EAN = csv[1],
                                    Stan = s,
                                    Dostepny = stan > 0 ,
                                    Symbol = csv[3].Trim(),
                                    Nazwa = csv[2],
                                    CenaNetto = csv[6],
                                    KoniecSerii = csv[9],
                                    CenaBrutto = csv[8],
                                    IstniejeWBazie = false,
                                    Promocja = csv[4]

                                });
                    }
                    catch (Exception ex)
                    {

                        //throw new Exception(String.Format("Błąd czytania pliku Italux: {0}", csv.ToString()));
                    }
                }

                ii = ii.Distinct().ToList();
                ProcessData(ii, supplier);

            }
        }

        private void ProcessData(List<ItaluxImport> ii, Dal.Supplier supplier)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { supplierId });

            foreach(Dal.ProductCatalog productFromDb in products)
            {
                ItaluxImport productToCheck = ii.Where(x => x.EAN == productFromDb.Ean).FirstOrDefault();

                if (productToCheck != null)
                {
                    productFromDb.SupplierQuantity = productToCheck.Stan;
                    productFromDb.IsAvailable = productToCheck.Dostepny;
                    productFromDb.UpdateUser = "System";
                    productFromDb.UpdateReason = "Aktualizacja automatyczna";
                    productToCheck.IstniejeWBazie = true;

                    decimal price = 0;
                    if (Decimal.TryParse(productToCheck.CenaBrutto, out price))
                        productFromDb.PriceBruttoFixed = Dal.Helper.RoundValue((Dal.Helper.SupplierRoundPriceType)supplier.RoundPriceTypeId, price);
                }
                //else
                    //productFromDb.IsAvailable = false;
            }

            pch.SetProductCatalogUpdateItalux(products, supplierId);

            List<ItaluxImport> productsItaluxToAdd = ii.Where(x => x.EAN != null && x.Symbol != null && !x.IstniejeWBazie).ToList();

            List<Dal.ProductCatalog> productsToAdd = new List<Dal.ProductCatalog>();
            foreach(ItaluxImport italuxToAdd in productsItaluxToAdd)
            {
                Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(supplierId, italuxToAdd.Symbol, italuxToAdd.Dostepny);
                pc.Ean = italuxToAdd.EAN.Substring(0, Math.Min(italuxToAdd.EAN.Length, 13)); ;
                pc.Name = italuxToAdd.Nazwa.Substring(0, Math.Min(italuxToAdd.Nazwa.Length, 100));

                decimal price = 0;
                if (Decimal.TryParse(italuxToAdd.CenaBrutto, out price))
                    pc.PriceBruttoFixed = Dal.Helper.RoundValue((Dal.Helper.SupplierRoundPriceType)supplier.RoundPriceTypeId, price);
                 
                pc.SupplierQuantity = italuxToAdd.Stan;


                if (pc.Ean == "")
                    pc.Ean = null;
                productsToAdd.Add(pc);
            }

            pch.SetProductCatalogs(productsToAdd, supplierId);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(supplierId, DateTime.Now);
        }
    }
}
