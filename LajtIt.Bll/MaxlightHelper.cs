using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinqToExcel;
using LinqToExcel.Attributes;

namespace LajtIt.Bll
{
    public class MaxlightHelper : ImportData, IImportData
    {
        public class MaxlightXlsxFile
        {
            [ExcelColumn("name")]
            public string Nazwa { get; set; }//New producer article number
            [ExcelColumn("catalog_number")]
            public string Kod { get; set; }//Quantity in stock	
            [ExcelColumn("ean")]
            public string Ean { get; set; }//Expected delivery date	
            [ExcelColumn("code_manufacturer")]
            public string Linia { get; set; }//Expected delivery date	
            [ExcelColumn("description")]
            public string Opis { get; set; }//Expected delivery date	
            [ExcelColumn("quantity")]
            public int Ilosc { get; set; }//Expected delivery date	
  

        }

       
        public new void LoadData<T>()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.SettingsHelper sh = new Dal.SettingsHelper();


            Dal.Supplier supplier = oh.GetSupplier(SupplierId);
            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format(path, String.Format("{0}_{1:yyyyMMMddHHmmss}.xlsx", supplier.Name, DateTime.Now));
            try
            {
                WebClient client = new WebClient();
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
            ExcelQueryFactory eqf = new ExcelQueryFactory(fileName);// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\Maytoni_201810021244.xlsx");// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\LajtitImport.xls");


            var r = from p in eqf.WorksheetRange<MaxlightXlsxFile>("A1", "F1500", 0) select p;



            var rr = r.ToList();

            rr = rr.Where(x => x.Ean != null).ToList();

            if (rr.Count == 0)
            {
                Bll.ErrorHandler.SendEmail("Plik Maxlight nie zwraca danych. Sprawdź jego strukturę");
                return;
            }




            List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();

            foreach (MaxlightXlsxFile max in rr)
            {
                Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, max.Kod, max.Ilosc > 0);

                pc.SupplierQuantity = max.Ilosc;

                pc.IsAvailable = pc.SupplierQuantity > 0;

                if (max.Ean != null)
                    max.Ean = max.Ean.Trim();

                pc.Ean = max.Ean;
                pc.Name = max.Nazwa;

                pc.Specification = max.Opis;

                pc.UpdateReason = max.Linia.Replace(max.Kod, "").Trim();

                products.Add(pc);
            }

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();



            Dal.OrderHelper oh = new Dal.OrderHelper();


            pch.SetProductCatalogUpdateMaxlight(products);

            oh.SetSupplierImportDate(SupplierId, DateTime.Now);


        }

        public void ProcessData<T>(T obj)
        {
            throw new NotImplementedException();
        }

        //public void ProcessData<T>(T obj)
        //{
        //    Bll.CeneoHelper.Offers pm = obj as Bll.CeneoHelper.Offers;

        //    List<Dal.ProductCatalog> products = GetProductCatalog(pm, SupplierId);

        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    pch.SetProductCatalogUpdateByCeneoFile(products, SupplierId);
        //}

        //public static   List<Dal.ProductCatalog> GetProductCatalog(CeneoHelper.Offers pm, int supplierId )
        //{
        //    List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();

        //    string s = String.Join(",", pm.O.Select(x => x.Cat).Distinct().ToArray());
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    Dal.ProductCatalogGroup group;
        //    List<string> series = new List<string>();
        //    foreach (CeneoHelper.O offer in pm.O)
        //    {
        //        string code = "";
        //        string ean = null;
        //        string serie = "";
        //        foreach (CeneoHelper.A att in offer.Attrs.A)
        //        {
        //            if (att.Name.ToLower() == "ean" && !String.IsNullOrEmpty(att.Text))
        //                ean = att.Text.Trim();
        //            if (att.Name.ToLower() == "kod-av" && !String.IsNullOrEmpty(att.Text))
        //                code = att.Text.Trim();
        //            if (att.Name.ToLower() == "kod_producenta" && !String.IsNullOrEmpty(att.Text))
        //                code = att.Text.Trim();
        //            if (att.Name.ToLower() == "seria" && !String.IsNullOrEmpty(att.Text))
        //                serie = att.Text.Trim();
        //        }


        //        Dal.ProductCatalog product = Dal.ProductCatalogHelper.GetProductCatalog(supplierId, code.Trim(), offer.Stock != "0");



        //        // ProductCatalogId         = ,
        //        product.Name = offer.Name;
        //        //throw new NotImplementedException("PG");
        //        //product.ProductCatalogGroupId = 1;
        //           product.SupplierId = supplierId;
        //           product.Code = code.Trim();
        //           product.Specification = offer.Desc;
        //           product.PurchasePrice = (decimal)(Decimal.Parse(offer.Price.Replace(".", ",")) * 65 / 100);

        //           product.ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct;
        //           product.AutoAssignProduct = true;
        //           product.Ean = ean;
        //           product.PriceBruttoFixed = Decimal.Parse(offer.Price.Replace(".",","));
        //           product.ImageId = null;
        //           product.IsDiscontinued = false;
        //           product.IsAvailable = offer.Stock != "0";

        //           product.PriceBruttoPromo = null;
        //           product.PriceBruttoPromoDate = null;

        //           product.ExternalId = offer.Id;
        //           product.IsHidden = false;
        //           product.IsOnStock = false;

        //        int stock = 0;
        //        if (!Int32.TryParse(offer.Stock, out stock))
        //            product.SupplierQuantity = null;
        //        else
        //            product.SupplierQuantity = stock;

        //        //IsActive                 = ,
        //        //IsActiveAllegro          = ,
        //        //IsActiveOnline           = 




        //        //if (serie != "")
        //        //{
        //        //    group = pch.GetProductCatalogGroup(supplierId, serie);
        //        //    //product.ProductCatalogGroupId = group.ProductCatalogGroupId;

        //        //    throw new NotImplementedException("PG");
        //        //}





        //        products.Add(product);

        //    }



        //    return products;
        //}
    }
}
