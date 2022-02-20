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
    public class RedluxHelper : ImportData, IImportData
    {
        [XmlRoot(ElementName = "IMAGES")]
        public class IMAGES
        {
            [XmlElement(ElementName = "IMGURL")]
            public string IMGURL { get; set; }
            [XmlElement(ElementName = "IMGURL_ALTERNATIVE")]
            public List<string> IMGURL_ALTERNATIVE { get; set; }
        }

        [XmlRoot(ElementName = "PARAMETER")]
        public class PARAMETER
        {
            [XmlElement(ElementName = "NAME")]
            public string NAME { get; set; }
            [XmlElement(ElementName = "VALUE")]
            public string VALUE { get; set; }
        }

        [XmlRoot(ElementName = "PARAMETERS")]
        public class PARAMETERS
        {
            [XmlElement(ElementName = "PARAMETER")]
            public List<PARAMETER> PARAMETER { get; set; }
        }

        [XmlRoot(ElementName = "MATERIALS")]
        public class MATERIALS
        {
            [XmlElement(ElementName = "PARAMETER")]
            public List<MATERIAL> MATERIAL { get; set; }
        }
        [XmlRoot(ElementName = "PARAMETER")]
        public class MATERIAL
        {
            [XmlElement(ElementName = "NAME")]
            public string NAME { get; set; } 
        }
        [XmlRoot(ElementName = "SHOPITEM")]
        public class SHOPITEM
        {
            [XmlElement(ElementName = "ITEM_ID")]
            public string ITEM_ID { get; set; }
            [XmlElement(ElementName = "WEB_ITEM_ID")]
            public string WEB_ITEM_ID { get; set; }
            [XmlElement(ElementName = "PRODUCT")]
            public string PRODUCT { get; set; }
            [XmlElement(ElementName = "DESCRIPTION")]
            public string DESCRIPTION { get; set; }
            [XmlElement(ElementName = "URL")]
            public string URL { get; set; }
            [XmlElement(ElementName = "EAN")]
            public string EAN { get; set; }
            [XmlElement(ElementName = "IMAGES")]
            public IMAGES IMAGES { get; set; }
            [XmlElement(ElementName = "PARAMETERS")]
            public PARAMETERS PARAMETERS { get; set; }
            [XmlElement(ElementName = "MATERIALS")]
            public MATERIALS MATERIALS { get; set; }
            [XmlElement(ElementName = "PRICE_VAT")]
            public string PRICE_VAT { get; set; }
            [XmlElement(ElementName = "VAT")]
            public string VAT { get; set; }
            [XmlElement(ElementName = "DELIVERY_DATE")]
            public string DELIVERY_DATE { get; set; }
            [XmlElement(ElementName = "AVAILABILITY")]
            public string AVAILABILITY { get; set; }
            [XmlElement(ElementName = "IN_STOCK")]
            public string IN_STOCK { get; set; }
            [XmlElement(ElementName = "CATEGORYTEXT")]
            public string CATEGORYTEXT { get; set; }
            [XmlElement(ElementName = "ITEMGROUP_ID")]
            public string ITEMGROUP_ID { get; set; }
        }

        [XmlRoot(ElementName = "SHOP")]
        public class SHOP
        {
            [XmlElement(ElementName = "SHOPITEM")]
            public List<SHOPITEM> SHOPITEM { get; set; }
        }


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
       
        public void ProcessData<T>(T obj)
        {
            RedluxHelper.SHOP pm = obj as RedluxHelper.SHOP;
             
             Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<string> productsInDb = new List<string>();
            try
            {
                
                List<Dal.ProductCatalog> productsFromCatalog = pch.GetProductCatalogBySupplier(new int[] { SupplierId });

                foreach (Dal.ProductCatalog productsExisting in productsFromCatalog)
                {
                    try
                    {

                        RedluxHelper.SHOPITEM pc = pm.SHOPITEM.Where(x => x.EAN == productsExisting.Ean).FirstOrDefault();
                        if (pc != null)
                        {
                            productsExisting.IsAvailable = pc.IN_STOCK.ToLower() == "yes";
                            productsExisting.SupplierQuantity = null;
                            productsExisting.ExternalId = pc.WEB_ITEM_ID;
                            productsInDb.Add(pc.EAN);
                        }
                        else
                        {
                            productsExisting.IsAvailable = false;
                            productsExisting.SupplierQuantity = null;
                        }
                        pch.SetProductCatalogRedluxUpdate(productsExisting.ProductCatalogId, productsExisting);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.SendError(ex, String.Format("Redlux ProductCatalogId {0}", productsExisting.ProductCatalogId));
                        continue;
                    }
                }

                string[] eanInDb = productsInDb.Select(x => x).ToArray();
                List<SHOPITEM> productsNotInDb = pm.SHOPITEM.Where(x => !eanInDb.Contains(x.EAN)).ToList();

                List<Dal.ProductCatalog> toAdd = new List<ProductCatalog>();

                foreach(SHOPITEM item in productsNotInDb)
                {
                    Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, item.ITEM_ID, item.IN_STOCK.ToLower()=="yes");

                    pc.Ean = item.EAN;
                    pc.Name = item.PRODUCT;

                    toAdd.Add(pc);
                }

                pch.SetProductCatalogsByEan(toAdd, SupplierId);

              


                LoadImages(pm);
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, "Altavola import");
                throw ex;
            }
        }

        public class AltConf
        {
            public string Ean { get; set; }
            public string Code { get; set; }
            public string Nazwa { get; set; }
            public string Opis { get; set; }
            public string Category { get; set; }
            public string Material { get; set; }

            public Dictionary<string, string> Dict { get; set; }
            public List<string> Imgs { get; set; }
        }


        List<string> columns = new List<string>();
        List<AltConf> conf = new List<AltConf>();
        private void LoadImages(RedluxHelper.SHOP pm)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> productsFromCatalog = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                //.Where(x=>x.ImageId == null)
                .ToList();


            foreach (Dal.ProductCatalog pc in productsFromCatalog)
            {
                SHOPITEM item = pm.SHOPITEM.Where(x => x.EAN == pc.Ean).FirstOrDefault();

                if (item == null)
                    continue;

                List<string> images = new List<string>();

                if (item.IMAGES != null && !String.IsNullOrEmpty(item.IMAGES.IMGURL))
                    images.Add(item.IMAGES.IMGURL);

                if (item.IMAGES != null && item.IMAGES.IMGURL_ALTERNATIVE != null)
                    foreach (string url in item.IMAGES.IMGURL_ALTERNATIVE)
                        if (!String.IsNullOrEmpty(url))
                            images.Add(url);

                if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Dev && !pc.ImageId.HasValue)
                    foreach (string img in images)
                        DownloadImage(img, pc.ProductCatalogId);


                Dictionary<string, string> attributes = new Dictionary<string, string>();

                foreach (PARAMETER p in item.PARAMETERS.PARAMETER) { 

                    if (!columns.Contains(p.NAME.ToLower()))
                        columns.Add(p.NAME.ToLower());

                    if (!attributes.Keys.Contains(p.NAME.ToLower()))
                        attributes.Add(p.NAME.ToLower(), p.VALUE);


                }
                conf.Add(new AltConf()
                {
                    Ean = pc.Ean,
                    Dict = attributes,
                    Imgs = images,
                    Code = pc.Code,
                    Category = item.CATEGORYTEXT,
                    Nazwa = item.PRODUCT,
                    Opis = item.DESCRIPTION,
                    Material = GetMaterial( item)
                });

            }

                SetConfiguration();

        }

        private string GetMaterial(SHOPITEM item)
        {
            if (item.MATERIALS!=null && item.MATERIALS.MATERIAL !=null && item.MATERIALS.MATERIAL.FirstOrDefault() != null)
                return item.MATERIALS.MATERIAL.FirstOrDefault().NAME;
            else
                return "";
        }

        private void SetConfiguration()
        {
            DataTable table = new DataTable();
            //columns  

            table.Columns.Add("ean", typeof(string));
            table.Columns.Add("material", typeof(string));
            table.Columns.Add("nazwa", typeof(string));
            table.Columns.Add("opis", typeof(string));
            table.Columns.Add("category", typeof(string));

            foreach (string column in columns)

                table.Columns.Add(column, typeof(string));




            foreach (AltConf ac in conf.Where(x => x.Ean != "" && x.Ean != null).ToList())
            {
                DataRow dr = table.NewRow();

                dr["ean"] = ac.Ean;
                dr["material"] = ac.Material;
                dr["nazwa"] = ac.Nazwa;
                dr["opis"] = ac.Opis;
                dr["category"] = ac.Category;

                foreach (string key in ac.Dict.Keys)
                {
                    dr[key] = ac.Dict[key];
                }

                table.Rows.Add(dr);
            }



            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format("Redlux_Configuration.csv");

            string saveLocation = String.Format(path, fileName);


           AltavolaHelper.ToCSV(table, saveLocation);

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
