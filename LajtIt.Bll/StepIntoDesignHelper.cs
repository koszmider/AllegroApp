using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lajtit.Bll.Altavola;
using LajtIt.Dal;
using System.Net;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using LinqToExcel;
using System.Data;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class StepIntoDesignHelper : ImportData, IImportData
    {
        #region classes
        [XmlRoot(ElementName = "producer", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Producer
        {
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "category", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Category
        {
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "unit", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Unit
        {
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "card", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Card
        {
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
        }

        [XmlRoot(ElementName = "name", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Name
        {
            [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
            public string Lang { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "long_desc", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Long_desc
        {
            [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
            public string Lang { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "short_desc", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Short_desc
        {
            [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
            public string Lang { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "description", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Description
        {
            [XmlElement(ElementName = "name", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<Name> Name { get; set; }
            [XmlElement(ElementName = "long_desc", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<Long_desc> Long_desc { get; set; }
            [XmlElement(ElementName = "short_desc", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<Short_desc> Short_desc { get; set; }
            [XmlElement(ElementName = "version", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<Version> Version { get; set; }
        }

        [XmlRoot(ElementName = "price", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Price
        {
            [XmlAttribute(AttributeName = "gross")]
            public string Gross { get; set; }
            [XmlAttribute(AttributeName = "net")]
            public string Net { get; set; }
        }

        [XmlRoot(ElementName = "srp", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Srp
        {
            [XmlAttribute(AttributeName = "gross")]
            public string Gross { get; set; }
            [XmlAttribute(AttributeName = "net")]
            public string Net { get; set; }
        }

        [XmlRoot(ElementName = "price", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class PricePromo
        {
            [XmlAttribute(AttributeName = "normal_price")]
            public string Normal_price { get; set; }
            [XmlAttribute(AttributeName = "normal_price_net")]
            public string Normal_price_net { get; set; }
        }

        [XmlRoot(ElementName = "promotion", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Promotion
        {
            [XmlElement(ElementName = "price", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public PricePromo PricePromo { get; set; }
        }

        [XmlRoot(ElementName = "stock", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Stock
        {
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "quantity")]
            public string Quantity { get; set; }
        }
        
        [XmlRoot(ElementName = "size", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Size
        {
            [XmlElement(ElementName = "stock", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<Stock> Stock { get; set; }
            [XmlElement(ElementName = "price", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Price Price { get; set; }
            [XmlElement(ElementName = "srp", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Srp Srp { get; set; }
            [XmlAttribute(AttributeName = "code_producer")]
            public string Code_producer { get; set; }
            [XmlAttribute(AttributeName = "code")]
            public string Code { get; set; }
            [XmlAttribute(AttributeName = "weight")]
            public string Weight { get; set; }
            [XmlElement(ElementName = "strikethrough_retail_price", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Strikethrough_retail_price Strikethrough_retail_price { get; set; }
        }

        [XmlRoot(ElementName = "sizes", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Sizes
        {
            [XmlElement(ElementName = "size", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Size Size { get; set; }
        }

        [XmlRoot(ElementName = "image", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Image
        {
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
            [XmlAttribute(AttributeName = "date_changed")]
            public string Date_changed { get; set; }
            [XmlAttribute(AttributeName = "hash")]
            public string Hash { get; set; }
            [XmlAttribute(AttributeName = "width")]
            public string Width { get; set; }
            [XmlAttribute(AttributeName = "height")]
            public string Height { get; set; }
        }

        [XmlRoot(ElementName = "large", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Large
        {
            [XmlElement(ElementName = "image", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<Image> Image { get; set; }
        }

        [XmlRoot(ElementName = "icon", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Icon
        {
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
            [XmlAttribute(AttributeName = "date_changed")]
            public string Date_changed { get; set; }
            [XmlAttribute(AttributeName = "hash")]
            public string Hash { get; set; }
            [XmlAttribute(AttributeName = "width")]
            public string Width { get; set; }
            [XmlAttribute(AttributeName = "height")]
            public string Height { get; set; }
        }

        [XmlRoot(ElementName = "group_icon", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Group_icon
        {
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
            [XmlAttribute(AttributeName = "date_changed")]
            public string Date_changed { get; set; }
            [XmlAttribute(AttributeName = "hash")]
            public string Hash { get; set; }
            [XmlAttribute(AttributeName = "width")]
            public string Width { get; set; }
            [XmlAttribute(AttributeName = "height")]
            public string Height { get; set; }
        }

        [XmlRoot(ElementName = "icons", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Icons
        {
            [XmlElement(ElementName = "icon", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Icon Icon { get; set; }
            [XmlElement(ElementName = "group_icon", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Group_icon Group_icon { get; set; }
        }

        [XmlRoot(ElementName = "images", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Images
        {
            [XmlElement(ElementName = "large", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Large Large { get; set; }
            [XmlElement(ElementName = "icons", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Icons Icons { get; set; }
        }

        [XmlRoot(ElementName = "value", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Value
        {
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "priority")]
            public string Priority { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "parameter", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Parameter
        {
            [XmlElement(ElementName = "value", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<Value> Value { get; set; }
            [XmlAttribute(AttributeName = "type")]
            public string Type { get; set; }
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "priority")]
            public string Priority { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "parameters", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Parameters
        {
            [XmlElement(ElementName = "parameter", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<Parameter> Parameter { get; set; }
        }

        [XmlRoot(ElementName = "warranty", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Warranty
        {
            [XmlAttribute(AttributeName = "iaiext", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Iaiext { get; set; }
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "type", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
            public string Type { get; set; }
            [XmlAttribute(AttributeName = "period", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
            public string Period { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "product", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Product
        {
            [XmlElement(ElementName = "producer", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Producer Producer { get; set; }
            [XmlElement(ElementName = "category", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Category Category { get; set; }
            [XmlElement(ElementName = "unit", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Unit Unit { get; set; }
            [XmlElement(ElementName = "card", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Card Card { get; set; }
            [XmlElement(ElementName = "description", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Description Description { get; set; }
            [XmlElement(ElementName = "price", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Price Price { get; set; }
            [XmlElement(ElementName = "srp", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Srp Srp { get; set; }
            [XmlElement(ElementName = "promotion", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Promotion Promotion { get; set; }
            [XmlElement(ElementName = "sizes", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Sizes Sizes { get; set; }
            [XmlElement(ElementName = "images", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Images Images { get; set; }
            [XmlElement(ElementName = "parameters", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Parameters Parameters { get; set; }
            [XmlElement(ElementName = "warranty", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Warranty Warranty { get; set; }
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "vat")]
            public string Vat { get; set; }
            [XmlAttribute(AttributeName = "type")]
            public string Type { get; set; }
            [XmlAttribute(AttributeName = "producer_code_standard")]
            public string Producer_code_standard { get; set; }
            [XmlAttribute(AttributeName = "code_on_card")]
            public string Code_on_card { get; set; }
            [XmlElement(ElementName = "series", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Series Series { get; set; }
            [XmlElement(ElementName = "group", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Group Group { get; set; }
            [XmlElement(ElementName = "strikethrough_retail_price", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Strikethrough_retail_price Strikethrough_retail_price { get; set; }
            [XmlElement(ElementName = "iai_category", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
            public Iai_category Iai_category { get; set; }
            [XmlElement(ElementName = "attachments", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Attachments Attachments { get; set; }
        }

        [XmlRoot(ElementName = "series", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Series
        {
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "version", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Version
        {
            [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
            public string Lang { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "product_value", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Product_value
        {
            [XmlElement(ElementName = "name", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<Name> Name { get; set; }
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
        }

        [XmlRoot(ElementName = "group_by_parameter", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Group_by_parameter
        {
            [XmlElement(ElementName = "name", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<Name> Name { get; set; }
            [XmlElement(ElementName = "product_value", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Product_value Product_value { get; set; }
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
        }

        [XmlRoot(ElementName = "group", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Group
        {
            [XmlElement(ElementName = "group_by_parameter", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Group_by_parameter Group_by_parameter { get; set; }
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "first_product_id")]
            public string First_product_id { get; set; }
        }

        [XmlRoot(ElementName = "strikethrough_retail_price", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Strikethrough_retail_price
        {
            [XmlAttribute(AttributeName = "gross")]
            public string Gross { get; set; }
            [XmlAttribute(AttributeName = "net")]
            public string Net { get; set; }
        }

        [XmlRoot(ElementName = "iai_category", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public class Iai_category
        {
            [XmlAttribute(AttributeName = "iaiext", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Iaiext { get; set; }
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "path")]
            public string Path { get; set; }
        }

        [XmlRoot(ElementName = "file", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class File
        {
            [XmlElement(ElementName = "name", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Name Name { get; set; }
            [XmlAttribute(AttributeName = "iaiext", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Iaiext { get; set; }
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
            [XmlAttribute(AttributeName = "priority")]
            public string Priority { get; set; }
            [XmlAttribute(AttributeName = "attachment_file_extension")]
            public string Attachment_file_extension { get; set; }
            [XmlAttribute(AttributeName = "enable", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
            public string Enable { get; set; }
            [XmlAttribute(AttributeName = "attachment_file_type", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
            public string Attachment_file_type { get; set; }
            [XmlAttribute(AttributeName = "download_log", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
            public string Download_log { get; set; }
        }

        [XmlRoot(ElementName = "attachments", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Attachments
        {
            [XmlElement(ElementName = "file", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<File> File { get; set; }
        }

        [XmlRoot(ElementName = "products", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Products
        {
            [XmlElement(ElementName = "product", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public List<Product> Product { get; set; }
            [XmlAttribute(AttributeName = "iaiext", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Iaiext { get; set; }
            [XmlAttribute(AttributeName = "language")]
            public string Language { get; set; }
            [XmlAttribute(AttributeName = "currency")]
            public string Currency { get; set; }
        }

        [XmlRoot(ElementName = "offer", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
        public class Offer
        {
            [XmlElement(ElementName = "products", Namespace = "http://www.iai-shop.com/developers/iof.phtml")]
            public Products Products { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
            [XmlAttribute(AttributeName = "file_format")]
            public string File_format { get; set; }
            [XmlAttribute(AttributeName = "version")]
            public string Version { get; set; }
            [XmlAttribute(AttributeName = "generated")]
            public string Generated { get; set; }
            [XmlAttribute(AttributeName = "extensions")]
            public string Extensions { get; set; }
        }
        #endregion
        public new void LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }

        private string CleanInt(string str)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(str, "");
        }



        List<AltConf> conf = new List<AltConf>();
        List<string> columns = new List<string>();
        public void ProcessData<T>(T obj)
        {
            Offer pm = obj as Offer;
            
            List<Dal.ProductCatalog> products = GetProductCatalog(pm).Where(x => !String.IsNullOrEmpty(x.Ean)).ToList();
            List<Dal.ProductCatalog> productsFromAltavolaInDb = new List<Dal.ProductCatalog>();

            string[] c = pm.Products.Product.Select(x => x.Category.Name).Distinct().ToArray();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            try
            {

                List<Dal.ProductCatalog> productsFromCatalog = pch.GetProductCatalogBySupplier(new int[] { SupplierId });

                foreach (Dal.ProductCatalog productsExisting in productsFromCatalog)
                {
                    try {
                        // po kodzie
                        Dal.ProductCatalog pc = products.Where(x => productsExisting.Ean != null && x.SupplierQuantity > 0 && productsExisting.Ean.Trim().ToLower() == x.Ean.Trim().ToLower()).FirstOrDefault();
                        if (pc != null)
                        {
                            pch.SetProductCatalogAltavolaUpdate(productsExisting.ProductCatalogId, pc);
                            productsFromAltavolaInDb.Add(pc);
                        }
                        else
                        {
                            // produkt nie znaleziony w otrzymanych z Altavoli. deaktywuj go
                            productsExisting.IsAvailable = false;
                            productsExisting.SupplierQuantity = 0;
                            pch.SetProductCatalogAltavolaUpdate(productsExisting.ProductCatalogId, productsExisting);

                        }
                    } catch (Exception ex)
                    {
                        ErrorHandler.SendError(ex, String.Format("StepIntoDesign ProductCatalogId {0}", productsExisting.ProductCatalogId));
                        continue;
                    }
                }

                string[] eanInDb = productsFromAltavolaInDb.Select(x => x.Ean.ToLower()).ToArray();
                List<Dal.ProductCatalog> productsFromAltavolaNotInDb = products.Where(x => !eanInDb.Contains(x.Ean.ToLower()) && !String.IsNullOrEmpty(x.Code)).ToList();

                pch.SetProductCatalogAltavolaAddNew(productsFromAltavolaNotInDb);


                SetConfiguration();


                LoadImages(pm);
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, "StepIntoDesign import");
                throw ex;
            }
        }

        private void SetConfiguration()
        {
            DataTable table = new DataTable();
            //columns  

            foreach (string column in columns)

                table.Columns.Add(column, typeof(string));




            foreach (AltConf ac in conf.Where(x => x.Ean != "" && x.Ean != null).ToList())
            {
                DataRow dr = table.NewRow();

                dr["ean"] = ac.Ean;

                foreach (string key in ac.Dict.Keys)
                {
                    dr[key] = ac.Dict[key];
                }

                table.Rows.Add(dr);
            }



            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format("Altavola_Configuration.csv");

            string saveLocation = String.Format(path, fileName);


            ToCSV(table, saveLocation);

        }

        private void LoadImages(Offer pm)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> productsFromCatalog = pch.GetProductCatalogBySupplier(new int[] { SupplierId }).Where(x => x.Ean != null && x.ImageId == null).ToList();





            foreach (Dal.ProductCatalog pc in productsFromCatalog)
            {

                Product product = pm.Products.Product.Where(x => x.Sizes.Size.Code_producer == pc.Ean).FirstOrDefault();


                if (product == null)
                    continue;

                if (product.Images.Large != null)//.Image)
                    foreach (string url in product.Images.Large.Image.Select(x => x.Url).ToArray())
                        DownloadImage(url, pc.ProductCatalogId);
            }
        }

        public void ToCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
            //headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        //public void ProcessData<T>(T obj)
        //{
        //    CeneoHelper.Offers pm = obj as CeneoHelper.Offers;

        //    List<Dal.ProductCatalog> products = GetProductCatalog(pm);
        //    List<Dal.ProductCatalog> productsFromAltavolaInDb = new List<ProductCatalog>();

        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

        //    try
        //    {

        //        List<Dal.ProductCatalog> productsFromCatalog = pch.GetProductCatalogBySupplier(new int[] { SupplierId });

        //        foreach (Dal.ProductCatalog productsExisting in productsFromCatalog)
        //        {
        //            try
        //            {

        //                // po kodzie
        //                Dal.ProductCatalog pc = products.Where(x => productsExisting.Ean.Trim().ToLower() == x.Ean.Trim().ToLower()).FirstOrDefault();
        //                if (pc != null)
        //                {
        //                    pch.SetProductCatalogAltavolaUpdate(pc);
        //                    productsFromAltavolaInDb.Add(pc);
        //                }
        //                else
        //                {
        //                    // produkt nie znaleziony w otrzymanych z Altavoli. deaktywuj go
        //                    productsExisting.IsAvailable = false;
        //                    pch.SetProductCatalogAltavolaUpdate(productsExisting);

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania danych Altavola, Ean: {0}, Kod {1}",
        //                    productsExisting.Ean, productsExisting.Code));

        //            }


        //            string[] codeInDb = productsFromAltavolaInDb.Select(x => x.Code.ToLower()).ToArray();
        //            List<Dal.ProductCatalog> productsFromAltavolaNotInDb = products.Where(x => !codeInDb.Contains(x.Code.ToLower())).ToList();


        //            pch.SetProductCatalogAltavolaAddNew(productsFromAltavolaNotInDb);



        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Bll.ErrorHandler.SendError(ex, "Altavola import");
        //        throw ex;
        //    }

        //}

        public class AltConf
        {
            public string Ean { get; set; }

            public Dictionary<string, string> Dict { get; set; }
            public List<string> Imgs { get; set; }
        }
        public List<Dal.ProductCatalog> GetProductCatalog(Offer pm)
        {
            List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier supplier = oh.GetSupplier(SupplierId);


            foreach (Product offer in pm.Products.Product.Where(x => x.Category.Name.StartsWith("Lampy")))
            {
                //Dictionary<string, string> attributes = new Dictionary<string, string>();
                //foreach (Parameter att in offer.Parameters.Parameter)
                //{

                //    if (!columns.Contains(att.Name.ToLower()))
                //        columns.Add(att.Name.ToLower());

                //    if (!attributes.Keys.Contains(att.Name.ToLower()))
                //        attributes.Add(att.Name.ToLower(), att.Text);

                //    if (att.Name.ToLower() == "ean" && !String.IsNullOrEmpty(att.Text))
                //        ean = att.Text.Trim();
                //    if (att.Name.ToLower() == "kod-av" && !String.IsNullOrEmpty(att.Text))
                //        code = att.Text.Trim();
                //    if (att.Name.ToLower() == "kod_producenta" && !String.IsNullOrEmpty(att.Text))
                //        code = att.Text.Trim();
                //    if (att.Name.ToLower() == "seria" && !String.IsNullOrEmpty(att.Text))
                //        serie = att.Text.Trim();
                //    if (att.Name.ToLower() == "producent" && !String.IsNullOrEmpty(att.Text))
                //    { 
                //        producers.Add(att.Text.Trim());
                //        switch (att.Text.Trim())
                //        { 
                //            case "ALTAVOLA DESIGN":   
                //            case "Prestige by ALTAVOLA": break;
                //            default: notAltavola = true; break;
                //        }

                //    }

                //}





                try
                {


                    Dal.ProductCatalog product = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, offer.Code_on_card.Trim(), offer.Sizes.Size.Stock[0].Quantity != "0");
                    product.Name = offer.Description.Name.Where(x => x.Lang == "pol").Select(x => x.Text).FirstOrDefault();
                    //product.Specification = offer.Desc; 
                    // product.PurchasePrice = (decimal)(Decimal.Parse(offer.PriceWholesale, System.Globalization.CultureInfo.InvariantCulture) * 65 / 100);
                    product.Ean = offer.Sizes.Size.Code_producer;
                    product.PriceBruttoFixed = Decimal.Parse(offer.Price.Gross, System.Globalization.CultureInfo.InvariantCulture);
                    if (offer.Promotion != null)
                    {
                        product.PriceBruttoFixed = Decimal.Parse(offer.Promotion.PricePromo.Normal_price, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    product.ExternalId = offer.Id;
                    product.SupplierQuantity = (int)Decimal.Parse(offer.Sizes.Size.Stock[0].Quantity, System.Globalization.CultureInfo.InvariantCulture);

                    //int deliveryDays = 0;
                    //if(Int32.TryParse(offer.DeliveryDays, out deliveryDays))
                    //{
                    //    if(deliveryDays>0)
                    //    {
                    //        if (deliveryDays > supplier.DeliveryId)
                    //            product.DeliveryId = deliveries.Where(x => x.DeliveryId <= deliveryDays).OrderByDescending(x => x.DeliveryId).FirstOrDefault().DeliveryId;
                    //        else
                    //            product.DeliveryId = null;
                    //    }

                    //}

                    //if (serie != "")
                    //{
                    //    group = pch.GetProductCatalogGroup(supplierId, serie);
                    //    //product.ProductCatalogGroupId = group.ProductCatalogGroupId;
                    //    throw new NotImplementedException("PG");
                    //}

                    products.Add(product);

                }
                catch (Exception ex)
                {
                    ErrorHandler.SendError(ex, String.Format("StepIntoDesign Code {0}", offer.Code_on_card.Trim()));
                    continue;
                }
            }


            return products;
        }
     
       
        private int GetIntValue(string s)
        {
            int v = 0;
            if (!Int32.TryParse(s, out v))
                return 0;
            return v;
        }
 
        private List<string> GetValues(Product product, string key)
        {

            List<string> s = new List<string>();
            try
            { 
            List<Value> values = new List<Value>();
            s.AddRange(product.Parameters.Parameter.Where(x => x.Name.ToLower() == key.ToLower()).FirstOrDefault().Value.Select(x=>x.Name).ToList());
            }catch (Exception ex)
            {
                var e = ex.Message;
            }
            return s;
        }

        public static string DownloadImage(string imageUrl, int productCatalogId)
        {
            try
            {
                var uri = new Uri(imageUrl);
                var fileName = uri.Segments.Last();

                var fi = new FileInfo(uri.AbsolutePath);
                var ext = fi.Extension;

                if (ext == "" || ext != ".jpeg" || ext != ".png" || ext != ".gif")
                    ext = ".jpg";

                string newFileName = String.Format("{0}{1}", Guid.NewGuid(), ext);

                string saveLocation = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];
                string filePath = String.Format(saveLocation, newFileName);

                byte[] imageBytes;
                HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
                WebResponse imageResponse = imageRequest.GetResponse();

                Stream responseStream = imageResponse.GetResponseStream();

                using (BinaryReader br = new BinaryReader(responseStream))
                {
                    imageBytes = br.ReadBytes(8000000);
                    br.Close();
                }
                responseStream.Close();
                imageResponse.Close();

                FileStream fs = new FileStream(filePath, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(fs);
                try
                {
                    bw.Write(imageBytes);
                }
                catch (Exception ex)
                {
                    ErrorHandler.SendError(ex, String.Format("Download image from url DownoloadImage {0}", imageUrl));
                    return null;
                }
                finally
                {
                    fs.Close();
                    bw.Close();
                }
                ProductCatalogHelper.SaveFile(new int[] { productCatalogId }, filePath, newFileName, fileName);
                return filePath;
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("404"))
                    ErrorHandler.SendError(ex, imageUrl);
                return null;
            }
        }
    }
    
}
