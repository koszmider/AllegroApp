using LajtIt.Dal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public static class ShopExportFileHelper
    {
        public static void ExportFile(Dal.Helper.Shop shopExp, Dal.Helper.Shop shopI)
        { 
            List<Dal.Shop> shops = Dal.DbHelper.Shop.GetShops().Where(x => x.IsActive &&  x.ExportFileName != null)
             
                .ToList();


            //if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Dev)
            {
                shops = shops.Where(x => x.ShopId == (int)shopI).ToList();
            }



            Dal.Shop shopExport = Dal.DbHelper.Shop.GetShop((int)shopExp);

            foreach (Dal.Shop shopImport in shops)
            {
                //Dal.Shop shopImport = sh.GetShop((int)shopImp);

                switch (shopImport.ExportFileFormatTypeId)
                {
                    case 1:
                        if (shopImport.ShopId == 21)
                            GenerateCeneoXMLFileHTML(shopExport, shopImport);
                        else
                            GenerateCeneoXMLFile(shopExport, shopImport);
                        break;
                    case 2:
                        GeneratePgeXMLFile(shopImport);
                        break;
                    case 3:
                        GenerateHomebookXMLFile(shopExport, shopImport);
                        break;
                    case 4:
                        GenerateFBXMLFile(shopExport, shopImport);
                        break;
                    case 5:
                        GenerateEmpikFile(shopExport, shopImport);
                        break;
                }
            }

        }

        private static void GenerateEmpikFile(Shop shopExport, Shop shopImport)
        {

            string saveLocation = Bll.EmpikRESTHelper.Offers.GetFile(Dal.Helper.Shop.Empik);
            if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Prod)
            {

                FtpHelper fh = new FtpHelper();
                fh.UploadFileToftp(saveLocation, "/");
            }

            //return;

            //Dal.ProductCatalogHelper shh = new Dal.ProductCatalogHelper();

            //List<Dal.EmpikView> products = shh.GetProductCatalogEmpik();

            //SaveCSV<Dal.EmpikView>(shopImport, products.Select(x => new Dal.EmpikView
            //{
            //    id = x.id,
            //    title = x.title,
            //    description = Bll.OrderHelper.GetPreview(Dal.Helper.Shop.Empik, x.ProductCatalogId, false, true).ToString(),
            //    external_id = x.external_id,
            //    location_city = x.location_city,
            //    location_district = x.location_district,
            //    images = x.images,
            //    price_value = x.price_value,
            //    price_currency = x.price_currency,
            //    url = x.url,
            //    courier = x.courier,
            //    state = x.state

            //}
            //).AsEnumerable<Dal.EmpikView>());
        }
        private static void SaveCSV<T>(Shop shopImport, IEnumerable<T> offers)
        {
            string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];


            string saveLocation = String.Format(path, shopImport.ExportFileName);
 
             
            using (StreamWriter writer = new StreamWriter(saveLocation))
            {


                using (CsvHelper.CsvWriter csv = new CsvHelper.CsvWriter(writer))
                {
                    csv.Configuration.Delimiter = ",";
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.CultureInfo = CultureInfo.GetCultureInfo("en-US");

                    csv.WriteRecords(offers);
                }

            }



            if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Prod)
            {

                FtpHelper fh = new FtpHelper();
                fh.UploadFileToftp(saveLocation, "/");
            }
        }
        private static void GenerateFBXMLFile(Shop shopExport, Shop shopImport)
        {
            Dal.ShopHelper shh = new Dal.ShopHelper();
            List<Dal.SupplierShopFnResult> supplierShops = shh.GetSuppliersShop(shopImport.ShopId).Where(x=>x.IsActive).ToList();
            List<ShopFnResult> products = GetProducts(shopExport, shopImport);

            List<ProductCatalogShopCategoryFnResult> categories = shh.GetProductCatalogShopCategories(shopExport.ShopId);

            List<FbHelper.Entry> entries = new List<FbHelper.Entry>();
            FbHelper.Link link = new FbHelper.Link()
            {
                Href = "https://lajtit.pl",
                Rel = "alternate",
                Type = "text/html"
            };

            FbHelper.Feed feed = new FbHelper.Feed()
            {
                Title = "Lajtit.pl - Doradzamy. Oświetlamy.",
                Updated = DateTime.UtcNow.ToString(),
                Xmlns = "http://www.w3.org/2005/Atom",
                Entry = entries,
                Link = link,
                G = "http://base.google.com/ns/1.0"
            };

             
             

            foreach (ShopFnResult product in products)
            {
                int st = ShopUpdateHelper.ClickShop.GetStock(product.SupplierQuantity, product.LeftQuantity,
                product.IsAvailable, product.IsDiscontinued);
                if (st < 0)
                    st = 50;

                string stock = st.ToString();
                string cat = "";

                ProductCatalogShopCategoryFnResult categoryFn = categories.Where(x => x.ProductCatalogId == product.ProductCatalogId).FirstOrDefault();

                if (categoryFn != null)
                    cat = categoryFn.Name;
                List<string> images = new List<string>();
                images = GetImagesFb(product);

                FbHelper.Entry entry = new FbHelper.Entry()
                {
                    Brand = product.SupplierName,
                    Id = product.ShopProductId,
                    Price = String.Format(CultureInfo.InvariantCulture, "{0:0.00} PLN", product.PriceBruttoMinimum.Value),
                Link = String.Format("http://lajtit.pl/pl/p/p/{0}{1}", product.ShopProductId, GetUrlQueryString(shopImport)),
                    Inventory = stock,
                    Title = product.Name,
                    Product_type = cat,
                    Availability = "in stock",
                    Condition = "new",
                    Description = product.LongDescription,
                    Mpn = product.Code,
                    Gtin = product.Ean 
                };

                if (images.Count() > 0)
                    entry.Image_link = images[0];

                if (images.Count() > 1)
                    entry.Additional_image_link = images.Skip(1).ToList();

                if(product.IsActivePricePromo)
                {
                    entry.Sale_price = String.Format(CultureInfo.InvariantCulture, "{0:0.00} PLN", product.PriceBruttoPromo);
                    entry.Sale_price_effective_date = String.Format("{0:o}/{1:o}", DateTime.UtcNow, product.PriceBruttoPromoDate.Value.ToUniversalTime());
                }
                feed.Entry.Add(entry);
              
                //    string ceneoDesc = s.StringValue ?? "";


            
                //O offer = new O()
                //{

                //    Attrs = GetAttributes(product),
                //    Avail = GetAvail(product),
                //    Desc = ceneoDesc,
                //    Id = product.ShopProductId,
                //    Imgs = GetImages(product),
                //    Name = GetName(product, rebates, supplierShops, shopImport),
                //    Price = GetPrice(product, shopImport),
                //    Url = String.Format("http://lajtit.pl/pl/p/p/{0}{1}", product.ShopProductId, GetUrlQueryString(shopImport)),
                //    Set = "0",
                //    Stock = stock,
                //    Cat = cat,
                //    Basket = product.IsActivePricePromo ? "0" : "1"
                //};
                //offers.O.Add(offer);
                ////xml.Xsi = "http://www.w3.org/2001/XMLSchema-instance";

            }
            SaveXML<FbHelper.Feed>(shopImport, feed);
        }

        private static List<string> GetImagesFb(ShopFnResult product)
        {
            List<string> images = new List<string>();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogImage> imgs = pch.GetProductCatalogImages(product.ProductCatalogId).Where(x => x.IsActive).OrderBy(x => x.Priority).Skip(1).ToList();
            images.Add(String.Format(@"http://{0}/ProductCatalog/{1}", Dal.Helper.StaticLajtitUrl, product.FileName));
            images.AddRange(imgs.Select(x=> String.Format(@"http://{0}/ProductCatalog/{1}", Dal.Helper.StaticLajtitUrl, x.FileName)).ToList());

            return images;
        }
        private static void GenerateHomebookXMLFile(Shop shopExport, Shop shopImport)
        {
            Dal.ShopHelper shh = new Dal.ShopHelper();
            List<Dal.SupplierShopFnResult> supplierShops = shh.GetSuppliersShop(shopImport.ShopId).Where(x => x.IsActive).ToList();
            List<ShopFnResult> products = GetProducts(shopExport, shopImport);

            HomebookHelper.Offers offers = new HomebookHelper.Offers();

            offers.Offer = new List<HomebookHelper.Offer>();

           

            foreach (ShopFnResult product in products)
            {

                HomebookHelper.Imgs images = new HomebookHelper.Imgs();
                images.Img = new List<HomebookHelper.Img>();
                images.Img.Add(new HomebookHelper.Img()
                {
                    Default = "true",
                    Text = String.Format(@"http://{0}/ProductCatalog/{1}", Dal.Helper.StaticLajtitUrl, product.FileName)
                });
                HomebookHelper.Offer offer = new HomebookHelper.Offer()
                {
                    Attrs = GetHomebookAttributres(product),
                    Brand = product.SupplierName,
                    Cat = "",
                    Desc = GetDescription(Dal.Helper.Shop.Homebook, product),
                    Id = product.ShopProductId,
                    Imgs = images,
                    Name = GetName(product, supplierShops ),

                    Url = String.Format("http://lajtit.pl/pl/p/p/{0}{1}", product.ShopProductId, GetUrlQueryString(shopImport))
                };

                if(product.IsActivePricePromo)
                {
                    offer.Price = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", product.PriceBruttoPromo);
                    offer.Oldprice = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", product.PriceBrutto);
                }else
                {

                    offer.Price = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", product.PriceBruttoMinimum);
                }


                offers.Offer.Add(offer);

            }
            SaveXML<HomebookHelper.Offers>(shopImport, offers);
        }

        private static string GetDescription(Dal.Helper.Shop shop, ShopFnResult product)
        {
            return OrderHelper.GetPreview(shop, product.ProductCatalogId, false).ToString();
        }

        private static HomebookHelper.Attrs GetHomebookAttributres(ShopFnResult product)
        {
            HomebookHelper.Attrs attributes = new HomebookHelper.Attrs();
            attributes.Attr = new List<HomebookHelper.Attr>();
            attributes.Attr.Add(new HomebookHelper.Attr() { Name = "Producent", Text = product.SupplierName });
            attributes.Attr.Add(new HomebookHelper.Attr() { Name = "Kod_producenta", Text = product.Code });
            attributes.Attr.Add(new HomebookHelper.Attr() { Name = "Ean", Text = product.Ean });

            return attributes;
        }

        public static void GeneratePgeXMLFile(  Dal.Shop shopImport)
        {
            Dal.ShopHelper shh = new Dal.ShopHelper();
            List<Dal.SupplierShopFnResult> supplierShops = shh.GetSuppliersShop(shopImport.ShopId).Where(x => x.IsActive).ToList();
            List<ProductCatalogForShopFnResult> products = GetProductsForShop(shopImport);
            List<ProductCatalogShopCategoryFnResult> categories = shh.GetProductCatalogShopCategories(shopImport.ShopId);
            PGEHelper.Offer offers = new PGEHelper.Offer();
            offers.Products = new PGEHelper.Products();
            offers.Products.Product = new List<PGEHelper.Product>();
            offers.Version = "1";

            List<Dal.ShopRebate> rebates = shh.GetRebates();

            if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Dev)
                products = products.Where(x => x.SupplierId == 9).ToList();
            foreach (ProductCatalogForShopFnResult product in products)
            {
                string cat = "";

                ProductCatalogShopCategoryFnResult categoryFn = categories.Where(x => x.ProductCatalogId == product.ProductCatalogId).FirstOrDefault();

                if (categoryFn != null)
                    cat = /*"Produkty/Dom i ogród/Oświetlenie/Plafony";//*/ categoryFn.Name;

                PGEHelper.Category category = new PGEHelper.Category()
                {
                    Name =cat
                };


                Dal.Helper.Shop shopImportEnum = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopImport.ShopId);
                string name = GetName(product, supplierShops);
                PGEHelper.Description descritption = new PGEHelper.Description()
                {
                    Name = name,
                    Long_desc= Bll.OrderHelper.GetPreview(shopImportEnum, product.ProductCatalogId, false, true).ToString(),
                    Short_desc=name
                };
          

                PGEHelper.Large large = new PGEHelper.Large();
                large.Image = new List<PGEHelper.Image>();
                large.Image.Add(new PGEHelper.Image()
                {
                    Display_order = "1",
                    Url = String.Format(@"http://{0}/ProductCatalog/{1}", Dal.Helper.StaticLajtitUrl, product.ImageFullName)
                });

                PGEHelper.Images images = new PGEHelper.Images()
                {
                    Large = large
                };
                PGEHelper.Parameters parameters = new PGEHelper.Parameters();
                List<PGEHelper.Value> v = new List<PGEHelper.Value>();
                v.Add(new PGEHelper.Value() { Name = "60" });
                parameters.Parameter = new List<PGEHelper.Parameter>();
                //parameters.Parameter.Add(
                //    new PGEHelper.Parameter()
                //    {
                //        Name = "Szerokość",
                //        Value = v
                //    });
                //parameters.Parameter.Add(
                //    new PGEHelper.Parameter()
                //    {
                //        Name = "Szerokość",
                //        Value = v
                //    });
                PGEHelper.Producer producer = new PGEHelper.Producer();
                producer.Name = product.SupplierName;
                PGEHelper.Promotion promotion = new PGEHelper.Promotion();
                PGEHelper.Sizes sizes = new PGEHelper.Sizes();
                sizes.Size = new PGEHelper.Size()
                {
                    Stock = new PGEHelper.Stock()
                    {
                        Quantity = GetAvail(product.IsOnStock.Value, product.DeliveryId)
                    }
                };
                decimal p = product.PriceBruttoMinimum.Value;
                //GetPriceGross(product.PriceBrutto, 
                //    product.PriceBruttoPromo, 
                //    product.PriceBrutto, product.IsActivePricePromo, shopImport);

                PGEHelper.Price price = new PGEHelper.Price()
                {
                    Gross = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", p),
                    Net = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", p / (1 + Dal.Helper.VAT)),
                };
                PGEHelper.Product offer = new PGEHelper.Product()
                {
                         Vat= String.Format(CultureInfo.InvariantCulture, "{0:0.00}",Dal.Helper.VAT*100),
                         Code_on_card=product.Code,
                         Category = category,
                         Description = descritption,
                         
                         Display_order="0",
                         Images=images,
                         Parameters = parameters,
                         Producer = producer,
                         Producer_code_standard= product.Code,
                    //Promotion = promotion,
                    Sizes =sizes,
                         Price = price
                         
                    //Cat = "",

                };
                offers.Products.Product.Add(offer);
                offers.File_format = "IOF";
                offers.Version = "2.6";
                offers.Extensions = "no";
                offers.Generated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //xml.Xsi = "http://www.w3.org/2001/XMLSchema-instance";

            }
            SaveXML<PGEHelper.Offer>(shopImport, offers);
        }

        public static void GenerateCeneoXMLFileHTML(Dal.Shop shopExport, Dal.Shop shopImport)
        {
            Dal.ShopHelper shh = new Dal.ShopHelper();
            List<Dal.SupplierShopFnResult> supplierShops = shh.GetSuppliersShop(shopImport.ShopId).Where(x => x.IsActive).ToList();
            List<ShopFnResult> products = GetProducts(shopExport, shopImport).ToList();
            //.Where(x => x.SupplierId == 22 || x.SupplierId == 32 || x.LeftQuantity > 0)
            //.ToList();
            products = products.Where(x => x.SupplierId != 1 && x.SupplierId != 3).ToList();
            //;
            //.Take(100).OrderBy(x=>Guid.NewGuid()).ToList();

            List <ProductCatalogShopCategoryFnResult> categories = null;

            //if((Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopImport.ShopId) == Dal.Helper.Shop.Ceneo)
            //    categories = shh.GetProductCatalogShopCategories(shopExport.ShopId); // jak wgramy drzewo kategorii Ceneo to można uusunąć
            //else
            categories = shh.GetProductCatalogShopCategories(shopImport.ShopId);



            Offers offers = new Offers();
            offers.O = new List<O>();
            offers.Version = "1";

            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            Dal.Settings s = sh.GetSetting("CENEO_DESC");

            Dal.Helper.Shop shopImportEnum = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopImport.ShopId);


            //if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Dev)
            //products = products.Where(x => x.SupplierId == 2).ToList();

            //Dal.OrderHelper oh = new Dal.OrderHelper();

            //using (StreamWriter outputFile = new StreamWriter(Path.Combine("", "WriteLines.txt")))
            //{
            //    outputFile.WriteLine("vendorPartNumber;deliveryDays\n");
            //    foreach (ShopFnResult product in products)
            //    {
            //        Supplier supplier = oh.GetSupplier(product.SupplierId);
            //        String id = String.Format("{0}", product.Code);
            //        string avail = GetAvailHTML(product.IsOnStock, product.DeliveryId, product.pcDeliveryId);
            //        outputFile.WriteLine(id + ";" + avail + ";" + supplier.Name + "\n");
            //    }
            //    outputFile.Close();
            //}
            //return;

            foreach (ShopFnResult product in products)
            {
                System.Console.Write(String.Format("{0}", product.ProductCatalogId) + '\n');

                string ceneoDesc = s.StringValue ?? "";

                string cat = "";

                ProductCatalogShopCategoryFnResult categoryFn =
                    categories.Where(x => x.ProductCatalogId == product.ProductCatalogId)
                    .FirstOrDefault();

                if (categoryFn != null)
                    cat = "<![CDATA[" + categoryFn.Name + "]]>";

                int st = ShopUpdateHelper.ClickShop.GetStock(product.SupplierQuantity, product.LeftQuantity,
                    product.IsAvailable, product.IsDiscontinued);
                if (st < 0)
                    st = 50;
                
                string stock = st.ToString();
                O offer = new O()
                {

                    Attrs = GetAttributesHTML(product),
                    Avail = GetAvailHTML(product.IsOnStock, product.DeliveryId, product.pcDeliveryId),
                    Desc = Bll.OrderHelper.GetPreviewHTML(shopImportEnum, product.ProductCatalogId, false, false).ToString(),
                    Id = product.ShopProductId,
                    Imgs = GetImages(product),
                    Name = GetNameHTML(product, supplierShops),
                    Price = GetPriceFromPCSP(product, shopImport),
                    Url = String.Format("http://lajtit.pl/pl/p/p/{0}{1}", product.ShopProductId, GetUrlQueryString(shopImport)),
                    Set = "0",
                    Stock = stock,
                    Cat = cat,
                    Basket = product.IsActivePricePromo ? "0" : "1"
                };
                offers.O.Add(offer);
                //xml.Xsi = "http://www.w3.org/2001/XMLSchema-instance";

            }
            SaveXML<Offers>(shopImport, offers);
        }

        public static void GenerateCeneoXMLFile(Dal.Shop shopExport, Dal.Shop shopImport)
        {   
            Dal.ShopHelper shh = new Dal.ShopHelper();
            List<Dal.SupplierShopFnResult> supplierShops = shh.GetSuppliersShop(shopImport.ShopId).Where(x => x.IsActive).ToList();
            List<ShopFnResult> products = GetProducts(shopExport, shopImport)
                //.Where(x => x.SupplierId == 22 || x.SupplierId == 32 || x.LeftQuantity > 0)
                .ToList();
                
                ;
            //.Take(100).OrderBy(x=>Guid.NewGuid()).ToList();

            List<ProductCatalogShopCategoryFnResult> categories = null;
            
            //if((Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopImport.ShopId) == Dal.Helper.Shop.Ceneo)
            //    categories = shh.GetProductCatalogShopCategories(shopExport.ShopId); // jak wgramy drzewo kategorii Ceneo to można uusunąć
            //else
                categories = shh.GetProductCatalogShopCategories(shopImport.ShopId);



            Offers offers = new Offers();
            offers.O = new List<O>();
            offers.Version = "1"; 

            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            Dal.Settings s = sh.GetSetting("CENEO_DESC");
             
            Dal.Helper.Shop shopImportEnum = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopImport.ShopId);


            //if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Dev)
            //    products = products.Where(x => x.SupplierId == 2).ToList();

            foreach (ShopFnResult product in products)
            {
                string ceneoDesc = s.StringValue ?? "";


                string cat = "";

                ProductCatalogShopCategoryFnResult categoryFn = 
                    categories.Where(x => x.ProductCatalogId == product.ProductCatalogId)
                    .FirstOrDefault();

                if (categoryFn != null)
                    cat = categoryFn.Name;

                int st = ShopUpdateHelper.ClickShop.GetStock(product.SupplierQuantity, product.LeftQuantity,
                    product.IsAvailable, product.IsDiscontinued);
                if (st < 0)
                    st = 50;

                string stock = st.ToString();
                O offer = new O()
                {

                    Attrs = GetAttributes(product),
                    Avail = GetAvail(product.IsOnStock, product.DeliveryId),
                    Desc = Bll.OrderHelper.GetPreview(shopImportEnum, product.ProductCatalogId, false, true).ToString(),
                    Id = product.ShopProductId,
                    Imgs = GetImages(product),
                    Name = GetName(product, supplierShops ),
                    Price = GetPrice(product, shopImport),
                    Url = String.Format("http://lajtit.pl/pl/p/p/{0}{1}", product.ShopProductId, GetUrlQueryString(shopImport)),
                    Set = "0",
                    Stock = stock,
                    Cat = cat,
                    Basket = product.IsActivePricePromo ? "0" : "1"
                };
                offers.O.Add(offer);
                //xml.Xsi = "http://www.w3.org/2001/XMLSchema-instance";

            }
            SaveXML<Offers>(shopImport, offers);
        }

        private static void SaveXML<T>(Shop shopImport, object offers)
        {
            string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];


            string saveLocation = String.Format(path, shopImport.ExportFileName);

            XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
            Utf8StringWriter stringWriter = new Utf8StringWriter();
            XmlWriter writer = XmlWriter.Create(stringWriter);

            xmlserializer.Serialize(writer, offers);

            string serializeXml = stringWriter.ToString();

            writer.Close();

            serializeXml = serializeXml.Replace("&gt;", ">");
            serializeXml = serializeXml.Replace("&lt;", "<");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(serializeXml);


            //if (File.Exists(saveLocation))
            //    File.Copy(saveLocation.Replace(".xml", String.Format("_{0}.xml", Guid.NewGuid())), saveLocation);
            doc.Save(saveLocation);

            if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Prod)
            {

                FtpHelper fh = new FtpHelper();
                fh.UploadFileToftp(saveLocation, "/");
            }
        }


        public static List<ProductCatalogForShopFnResult> GetProductsForShop(Shop shopImport)
        {
            Dal.ShopHelper shh = new Dal.ShopHelper();
            List<Dal.ProductCatalogForShopFnResult> products = shh.ProductCatalogForShop(shopImport.ShopId);


            if (shopImport.ExportFileEanRequired.Value)
            {
                products = products.Where(x => x.Ean != null && x.Ean != "").ToList();
            }

 

            if (shopImport.ExportFileFilterByProductType)
            {
                int[] productCatalogIds = shh.GetShopExportFileAttribute(shopImport.ShopId)
                    .Select(x => x.ProductCatalogId)
                    .ToArray();

                products = products.Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();
            }

            if (shopImport.ExportFilePriceFrom.HasValue)
                products = products.Where(x => x.PriceBruttoMinimum.Value >= shopImport.ExportFilePriceFrom.Value).ToList();
            if (shopImport.ExportFilePriceTo.HasValue)
                products = products.Where(x => x.PriceBruttoMinimum.Value <= shopImport.ExportFilePriceTo.Value).ToList();

             
            return products;
        }


        public static List<ShopFnResult> GetProducts(Shop shopExport, Shop shopImport)
        {
            Dal.ShopHelper shh = new Dal.ShopHelper();
            List<Dal.ShopFnResult> products = shh.GetProductCatalogForShopExportFile(shopExport.ShopId, shopImport.ShopId);


            if (shopImport.ExportFileEanRequired.Value)
            {
                products = products.Where(x => x.Ean != null && x.Ean != "").ToList();
            }

      

            if (shopImport.ExportFileFilterByProductType)
            {
                int[] productCatalogIds = shh.GetShopExportFileAttribute(shopImport.ShopId)
                    .Select(x => x.ProductCatalogId)
                    .ToArray();

                products = products.Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();
            }

            if (shopImport.ExportFilePriceFrom.HasValue)
                products = products.Where(x => x.PriceBruttoMinimum.Value >= shopImport.ExportFilePriceFrom.Value).ToList();
            if (shopImport.ExportFilePriceTo.HasValue)
                products = products.Where(x => x.PriceBruttoMinimum.Value <= shopImport.ExportFilePriceTo.Value).ToList();


            if (shopImport.ShopId != (int)Dal.Helper.Shop.Morele)
                products = products.Where(x => x.ShopProductId != null).ToList();
            return products;
        }

        private static string GetUrlQueryString(Shop shop)
        {
            if (shop.ExportFileUrlParameters != null)
                return String.Format(shop.ExportFileUrlParameters);
            else
                return "";
        }
        private static string GetName(ProductCatalogForShopFnResult product, List<Dal.SupplierShopFnResult> supplierShops)
        {
            if (String.IsNullOrEmpty(product.ShopProductName))
                return product.Name;
            else
                return
                    product.ShopProductName;

        }
        private static string GetNameHTML(ShopFnResult product, List<Dal.SupplierShopFnResult> supplierShops)
        {


            /*
            Tworzenie nazw:
            1. Pobierz nazwe z ProductCatalogShopProduct gdzie nazwy są tworzone automatem raz dziennie
            2. Jeśli nazwy nie ma weź z nazwy produktu
            3. Pobierz szablon nazwy z poziomu dostawcy dla danego sklepu. Złącz z nazwą produktu



            */
            string productName = "";
            if (product.TemplateName != null)
                productName = product.TemplateName; // z ProductCatalogShopProduct
            else
                productName = product.Name; // z productCatalog             // Mixer.GetProductName(shopImport.ShopId, product.ProductCatalogId);



            string supplierTemplateName = "";


            Dal.SupplierShopFnResult ss = supplierShops.Where(x => x.SupplierId == product.SupplierId).FirstOrDefault();

            if (ss != null && !String.IsNullOrEmpty(ss.Template))
            {
                supplierTemplateName = ss.Template;
            }


            if (String.IsNullOrEmpty(supplierTemplateName))
            { }//   productName = productName;
            else
            {
                supplierTemplateName = supplierTemplateName.Replace("[SUPPLIER]", product.SupplierName);
                supplierTemplateName = supplierTemplateName.Replace("[PRODUCT]", productName);
                productName = supplierTemplateName;
            }

            return "<![CDATA[" + productName + "]]>";

        }
        private static string GetName(ShopFnResult product,  List<Dal.SupplierShopFnResult> supplierShops )
        {
 
     
            /*
            Tworzenie nazw:
            1. Pobierz nazwe z ProductCatalogShopProduct gdzie nazwy są tworzone automatem raz dziennie
            2. Jeśli nazwy nie ma weź z nazwy produktu
            3. Pobierz szablon nazwy z poziomu dostawcy dla danego sklepu. Złącz z nazwą produktu



            */
            string productName = "";
            if (product.TemplateName != null)
                productName = product.TemplateName; // z ProductCatalogShopProduct
            else
                productName = product.Name; // z productCatalog             // Mixer.GetProductName(shopImport.ShopId, product.ProductCatalogId);



            string supplierTemplateName = "";
       

            Dal.SupplierShopFnResult ss = supplierShops.Where(x => x.SupplierId == product.SupplierId).FirstOrDefault();

            if(ss!=null && !String.IsNullOrEmpty(ss.Template))
            {
                supplierTemplateName = ss.Template;
            }

         
            if (String.IsNullOrEmpty(supplierTemplateName))
            { }//   productName = productName;
            else
            {
                supplierTemplateName = supplierTemplateName.Replace("[SUPPLIER]", product.SupplierName);
                supplierTemplateName = supplierTemplateName.Replace("[PRODUCT]", productName);
                productName = supplierTemplateName;
            }
            
            return productName;

        }

        #region funkcje pomocnicze
        private static string GetAvailHTML(bool? isOnStock, int deliveryId, int? pcdeliveryId)
        {
            if (isOnStock.HasValue && isOnStock.Value)
                return "1";

            //return deliveryId.ToString();
            /*
             Ceneo
            1 – dostępny,
            3 – dostępny do 3 dni,
            7 – dostępny do tygodnia,
            14 – dostępny do 14 dni,
            90 – na zamówienie,
            99 – sprawdź dostępność,
            110 – w przedsprzedaży.
            */

            if (pcdeliveryId != null)
                deliveryId = (int)pcdeliveryId;

            switch (deliveryId)
            {
                case 1:
                    return "1";
                case 2:
                case 3:
                    return "3";
                case 4:
                case 5:
                case 6:
                case 7:
                    return "7";
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                    return "14";
                default: return "90";
            }
        }

        private static string GetAvail(bool? isOnStock, int deliveryId)
        {
            if (isOnStock.HasValue && isOnStock.Value)
                return "1";

            //return deliveryId.ToString();
            /*
             Ceneo
            1 – dostępny,
            3 – dostępny do 3 dni,
            7 – dostępny do tygodnia,
            14 – dostępny do 14 dni,
            90 – na zamówienie,
            99 – sprawdź dostępność,
            110 – w przedsprzedaży.
            */

            switch (deliveryId)
            {
                case 1:
                    return "1"; 
                case 2:
                case 3:
                    return "3";
                case 4:
                case 5:
                case 8:
                    return "7"; 
                default: return "14"; 
            }
        }

        /*

Podstawowe informacje o ofercie.
id - unikalne i niezmienne id produktu. Maksymalna ilość znaków 100. Wymagane
url - url produktu. Maksymalna ilość znaków 2048. Wymagane
price - cena produktu. Liczba zmiennoprzecinkowa, separator kropka. Wymagane

avail - dostępność produktu. Dostępne opcje [1, 3, 7, 14, 99] gdzie: 1 – dostępny, sklep wyśle
produkt w ciągu 24 godzin, 3 – sklep wyśle produkt do 3 dni, 7 – sklep wyśle produkt w ciągu
tygodnia, 14 – sklep wyśle produkt do 14 dni, 90 – towar na zamówienie, 99 – brak informacji
o dostępności – status „sprawdź w sklepie”, 110 – przedsprzedaż.
Podane wartości muszą być zgodnie ze stanem faktycznym, znacznik nie może pozostawać
pusty czy też posiadać wartość „0”. Opcjonalnie

set - zestaw. Czy oferta jest zestawem. Dostępne opcje [1, 0] gdzie; 1 – tak, oferta jest
zestawem, 0 – nie, oferta nie jest zestawem. Opcjonalnie
weight - waga. Waga oferty w kilogramach, separator kropka, nie może być podana wartość
„0” bądź puste pole . Opcjonalnie (podanie wag produktów z czasem będzie wymagane)
basket - dotyczy sklepów aktywnych w usłudze „Kup na Ceneo”. Czy oferta ma być dostępna
w Kup na Ceneo. Dostępne opcje [1, 0] gdzie; 1 – tak, oferta dostępna w Kup na Ceneo, 0 –
nie, oferta niedostępna w Kup na Ceneo. Opcjonalnie
stock - stan magazynowy. Liczna całkowita dodatnia. Pole nie może być puste. Opcjonalnie

*/
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }
        private static Attrs GetAttributesHTML(ShopFnResult product)
        {
            Attrs attributes = new Attrs();
            attributes.A = new List<A>();
            attributes.A.Add(new A() { Name = "Producent", Text = product.SupplierName });
            attributes.A.Add(new A() { Name = "Kod_producenta", Text = product.Code });
            attributes.A.Add(new A() { Name = "EAN", Text = product.Ean });
            return attributes;
        }
        private static Attrs GetAttributes(ShopFnResult product)
        {
            Attrs attributes = new Attrs();
            attributes.A = new List<A>();
            attributes.A.Add(new A() { Name = "Producent", Text = product.SupplierName });
            attributes.A.Add(new A() { Name = "Kod_producenta", Text = product.Code });
            attributes.A.Add(new A() { Name = "Ean", Text = product.Ean });
            return attributes;
        }

        private static Imgs GetImages(ShopFnResult product)
        {
            Imgs images = new Imgs();
            images.Main = new Main()
            {
                Url = String.Format(@"http://{0}/ProductCatalog/{1}", Dal.Helper.StaticLajtitUrl, product.FileName)

            };
            return images;
        }

        private static string GetPriceFromPCSP(ShopFnResult product, Shop shop)
        {
            decimal price = 0;

            Dal.ShopHelper shh = new Dal.ShopHelper();
            ProductCatalogShopProduct psp = shh.GetShopProduct(Dal.Helper.Shop.Morele, product.ProductCatalogId);
            if (psp != null)
            {
                if (psp.PriceBruttoMinimum != null)
                    price = psp.PriceBruttoMinimum.Value;
                else
                    price = product.PriceBruttoMinimum.Value;
            }
            else price = product.PriceBruttoMinimum.Value;// GetPriceGross(product.PriceBrutto, product.PriceBruttoPromo, product.PriceBruttoMinimum, product.IsActivePricePromo, shop);

            if (shop.ExportFileExportPriceTypeId == 2) //netto
                price = Decimal.Round(price / (1 + Dal.Helper.VAT), 2);

            return String.Format(CultureInfo.InvariantCulture, "{0:0.00}", price);
        }

        private static string GetPrice(ShopFnResult product, Shop shop)
        {
            decimal price = 0;

            price = product.PriceBruttoMinimum.Value;// GetPriceGross(product.PriceBrutto, product.PriceBruttoPromo, product.PriceBruttoMinimum, product.IsActivePricePromo, shop);

            if (shop.ExportFileExportPriceTypeId == 2) //netto
                price = Decimal.Round(price / (1 + Dal.Helper.VAT), 2);

            return String.Format(CultureInfo.InvariantCulture, "{0:0.00}", price);
        }

        private static decimal GetPriceGross(decimal? priceBruttoAllegro, decimal? priceBruttoPromo, decimal? priceBruttoShop, bool isActivePricePromo, Shop shop)
        {
            decimal price;
            if (shop.ExportFilePriceTypeId == 2) //cena allegrowa
            {
                price = priceBruttoAllegro.Value; // cena promocyjna jest już w tej cenie wyliczona
            }
            else // cena sklepowa
            {
                if (isActivePricePromo)
                    price =priceBruttoPromo.Value;
                else
                    price = priceBruttoShop.Value;
            }

            return price;
        }

        [XmlRoot(ElementName = "main")]
        public class Main
        {
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
        }

        [XmlRoot(ElementName = "i")]
        public class I
        {
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
        }

        [XmlRoot(ElementName = "imgs")]
        public class Imgs
        {
            [XmlElement(ElementName = "main")]
            public Main Main { get; set; }
            [XmlElement(ElementName = "i")]
            public List<I> I { get; set; }
        }

        [XmlRoot(ElementName = "a")]
        public class A
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "attrs")]
        public class Attrs
        {
            [XmlElement(ElementName = "a")]
            public List<A> A { get; set; }
        }

        [XmlRoot(ElementName = "o")]
        public class O
        {
            [XmlElement(ElementName = "cat")]
            public string Cat { get; set; }
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "imgs")]
            public Imgs Imgs { get; set; }
            [XmlElement(ElementName = "desc")]
            public string Desc { get; set; }
            [XmlElement(ElementName = "attrs")]
            public Attrs Attrs { get; set; }
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
            [XmlAttribute(AttributeName = "price_wholesale")]
            public string PriceWholesale { get; set; }
            [XmlAttribute(AttributeName = "price")]
            public string Price { get; set; }
            [XmlAttribute(AttributeName = "avail")]
            public string Avail { get; set; }
            [XmlAttribute(AttributeName = "set")]
            public string Set { get; set; }
            [XmlAttribute(AttributeName = "weight")]
            public string Weight { get; set; }
            [XmlAttribute(AttributeName = "basket")]
            public string Basket { get; set; }
            [XmlAttribute(AttributeName = "stock")]
            public string Stock { get; set; }
        }

        [XmlRoot(ElementName = "offers")]
        public class Offers
        {
            [XmlElement(ElementName = "o")]
            public List<O> O { get; set; }
            [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Xsi { get; set; }
            [XmlAttribute(AttributeName = "version")]
            public string Version { get; set; }
        }
        #endregion
    }
}
