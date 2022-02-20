using LinqToExcel;
using LinqToExcel.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class SompexHelper
    {
         
        public class SompexXlsxFile
        { 
            [ExcelColumn("Nazwa produktu")]            
            public string Nazwa { get; set; }//New producer article number
            [ExcelColumn("Nr produktu")]
            public string Kod { get; set; }//Quantity in stock	
            [ExcelColumn("Kod EAN")]
            public string Ean { get; set; }//Expected delivery date	
            [ExcelColumn("Producent")]
            public string Producent { get; set; }//Expected delivery date	
            [ExcelColumn("StanHandlowy Morax")]
            public string StanMorax { get; set; }//Expected delivery date	
            [ExcelColumn("Stan Producent")]
            public string StanProducent { get; set; }//Expected delivery date	
            [ExcelColumn("Cena detal brutto")]
            public string Cena { get; set; } 

        }

        public class SompexImport
        {
            public string Code { get; set; }
            public string Ean { get; set; }
            public string Supplier { get; set; }
            public int SupplierQuantity { get; set; }
            public decimal Price { get; set; } 
        }
        public   void  LoadData()
        {

            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            

            Dal.Supplier supplier = oh.GetSupplier(20);
            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format(path, String.Format("{0}_{1:yyyyMMMddHHmmss}.xlsx", supplier.Name, DateTime.Now));
            try
            {
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential(sh.GetSetting("SOMPEX_L").StringValue, sh.GetSetting("SOMPEX_H").StringValue);
                client.DownloadFile(supplier.ImportUrl, fileName);
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd wczytywania pliku: {0} ", supplier.Name));
          
            }


            ProcessData(fileName);
        }

        

        public void ProcessData(string fileName)
        {
            //Dictionary<string, int> markslojd = new Dictionary<string, int>();


            //foreach (string line in File.ReadAllLines(fileName).Skip(1))
            //{
            //    string[] fields = line.Split('|');
            //    markslojd.Add(fields[1], Convert.ToInt32(fields[2]) );
            //}

            ExcelQueryFactory eqf = new ExcelQueryFactory(fileName);// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\Maytoni_201810021244.xlsx");// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\LajtitImport.xls");
       

            var r = from p in eqf.WorksheetRange<SompexXlsxFile>("A3", "I1500", 0) select p;



            var rr = r.ToList();

            rr = rr.Where(x => x.Kod != null).ToList();

            if(rr.Count==0)
            {
                Bll.ErrorHandler.SendEmail("Plik Sompex nie zwraca danych. Sprawdź jego strukturę");
                return;
            }


            //List<SompexImport> products = rr 
            //    .Select(x => new SompexImport()
            //{
            //       Code=x.Kod,
            //       Ean=x.Ean,
            //       Price=Decimal.Parse(x.Cena),
            //       Supplier=x.Producent,
            //       SupplierQuantity=Int32.Parse(x.StanMorax)+Int32.Parse(x.StanProducent)

            //})
            
            //.ToList();


            List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();

            foreach( SompexXlsxFile sompex in rr)
            {
                int supplierId = 0; // 20-Sompex

                switch(sompex.Producent)
                {
                    case "SOMPEX":
                        supplierId = 20; break;
                    case "VILLEROY&BOCH":
                        supplierId = 21; break;

                }

                if (supplierId == 0)
                    continue;

                Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(supplierId, sompex.Kod, true);

                pc.SupplierQuantity = Int32.Parse(sompex.StanMorax);// + Int32.Parse(sompex.StanProducent);
                
                pc.IsAvailable = pc.SupplierQuantity > 0;
                pc.PriceBruttoFixed = Decimal.Parse(sompex.Cena);

                string ean = null;
                if (sompex.Ean != null)
                    sompex.Ean = sompex.Ean.Trim();

                if (String.IsNullOrEmpty(sompex.Ean))
                    ean = null;
                else
                    ean = sompex.Ean;

                pc.Ean = ean;
                pc.Name = sompex.Nazwa;

                products.Add(pc);
            }
            
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();



            int[] supplierIds = products.Select(x => x.SupplierId).Distinct().ToArray();

            Dal.OrderHelper oh = new Dal.OrderHelper();

            foreach (int supplierId in supplierIds)
            {


                pch.SetProductCatalogUpdateSompex(products.Where(x => x.SupplierId == supplierId).ToList(), supplierId);

                oh.SetSupplierImportDate(supplierId, DateTime.Now);
            }

        }
    }
}
