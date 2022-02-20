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

namespace LajtIt.Bll
{ 
    public class AltavolaHelper : ImportData, IImportData
    {
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
        //public Dictionary<int, bool> GetAttributeId(List<ProductCatalogAttribute> attributes, string groupName, Product product)
        //{
        //    Dictionary<int, bool> l = new Dictionary<int, bool>();

        //    if (groupName == "LAMPTYPE")
        //    {
        //        List<string> values = GetValues(product, "Typ lampy");

        //        switch (product.Category.Name)
        //        {
        //            case "Lampy/Kinkiety":
        //            case "Kinkiety/Wall":
        //                l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "KINKIET").Select(x => x.AttributeId).FirstOrDefault(), true); break;
        //            case "Lampy/Wiszące":
        //            case "Lampy/Żyrandole":
        //            case "Wiszące/Pendant":
        //                l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "WISZACA").Select(x => x.AttributeId).FirstOrDefault(), true); break;
        //            case "Lampy/Żarówki":
        //                l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "ZAROWKA").Select(x => x.AttributeId).FirstOrDefault(), true); break;
        //            case "Lampy/Biurowe/Stołowe":
        //            case "Biurkowe/Table":
        //                l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "STOLOWA").Select(x => x.AttributeId).FirstOrDefault(), true); break;
        //            case "Lampy/Stojące":
        //            case "Podłogowe/Floor":
        //                l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "PODLOGOWA").Select(x => x.AttributeId).FirstOrDefault(), true); break;

        //            case "Plafony/Ceiling":
        //                l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "PLAFON").Select(x => x.AttributeId).FirstOrDefault(), true); break;

        //            case "Halogenowe/Spots":
        //                l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "SPOT").Select(x => x.AttributeId).FirstOrDefault(), true); break;

        //            case "Zewnętrzne/Outdoor":
        //                l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "ZEWNETRZNA").Select(x => x.AttributeId).FirstOrDefault(), true); break;

        //        }
        //    }
        //    if (groupName == "LAMPSTYLE")
        //    {
        //        List<string> values = GetValues(product, "Styl");


        //        if (values.Contains("Vintage") || values.Contains("Rustykalny") || values.Contains("loft") || values.Contains("industrial"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "LOFT").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("skandynawski") || values.Contains("universal") || values.Contains("eko"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "KLASYCZNY").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Modern") || values.Contains("Marine"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "NOWOCZESNY").Select(x => x.AttributeId).FirstOrDefault(), false);

        //    }
        //    if (groupName == "KOLOR")
        //    {
        //        List<string> values = GetValues(product, "Kolor");


        //        if (values.Contains("Brązowy") || values.Contains("Rdzawy"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "BRAZOWY").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Transparentny"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "TRANSPARENTNY").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Bursztynowy"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "BURSZTYNOWY").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Dymny") || values.Contains("Srebrny") || values.Contains("Szary"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "SREBRNY").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Mosiądz"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "MOSIADZ").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Chrom"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "CHROM").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Niebieski"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "NIEBIESKI").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Beżowy"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "BEZOWY").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Czarny"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "CZARNY").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Biały"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "BIALY").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Czerwony"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "CZERWONY").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        //if (values.Contains("Naturalny"))
        //        //    l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "BRAZOWY").Select(x => x.AttributeId).FirstOrDefault(), false);


        //    }
        //    if (groupName == "MATERIAL")
        //    {
        //        List<string> values = GetValues(product, "Materiał");

        //        if (values.Contains("miedź"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "MIEDZ").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("aluminium"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "ALUMINIUM").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("metal") || values.Contains("żelazo"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "METAL").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Tkanina"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "TKANINA").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("Szkło") || values.Contains("lustro"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "SZKLO").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        //if (values.Contains("Naturalny"))
        //        //    l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "BRAZOWY").Select(x => x.AttributeId).FirstOrDefault(), false);

        //    }
        //    if (groupName == "KLOCHR")
        //    {
        //        List<string> values = GetValues(product, "Stopień ochrony");

        //        if (values.Contains("IP 20"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "IP20").Select(x => x.AttributeId).FirstOrDefault(), false);

        //    }
        //    if (groupName == "ZAROWKA")
        //    {
        //        List<string> values = GetValues(product, "Żarówka w komplecie");

        //        if (values.Contains("tak"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "ZAR1").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("nie"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "ZAR0").Select(x => x.AttributeId).FirstOrDefault(), false);

        //    }
        //    if (groupName == "BSWIATLA")
        //    {
        //        List<string> values = GetValues(product, "Barwa światła");

        //        if (values.Contains("ciepła"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "BCIEPLA").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("zimna"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "BZIMNA").Select(x => x.AttributeId).FirstOrDefault(), false);
        //        if (values.Contains("neutralna"))
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == "BNEUTALNA").Select(x => x.AttributeId).FirstOrDefault(), false);

        //    }
        //    if (groupName == "GWINT")
        //    {
        //        List<string> values = GetValues(product, "Oprawa żarówki");

        //        foreach (string v in values)
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == v).Select(x => x.AttributeId).FirstOrDefault(), false);


        //    }
        //    if (groupName == "KLENER")
        //    {
        //        List<string> values = GetValues(product, "Klasa efektywności energetycznej");

        //        foreach (string v in values)
        //            l.Add(attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == v).Select(x => x.AttributeId).FirstOrDefault(), false);


        //    }
        //    if (groupName == "ILOSCPKT")
        //    {
        //        List<string> values = GetValues(product, "Ilość źródeł światła");

        //        foreach (string v in values)
        //        {
        //            var a1v = attributes.Where(x => x.ProductCatalogAttributeGroup.GroupCode == groupName && x.Code == v).Select(x => x.AttributeId).FirstOrDefault();
        //            if (a1v != 0)
        //                l.Add(a1v, false);
        //        }


        //    }
        //    return l;
        //}


        List<AltConf> conf = new List<AltConf>();

        List<string> columns = new List<string>();
        public void ProcessData<T>(T obj)
        {
            CeneoHelper.Offers pm = obj as CeneoHelper.Offers;

            List<Dal.ProductCatalog> products = GetProductCatalog(pm).Where(x=>!String.IsNullOrEmpty(x.Ean)).ToList();
            List<Dal.ProductCatalog> productsFromAltavolaInDb = new List<Dal.ProductCatalog>();


            //StringBuilder sb = new StringBuilder();

            //foreach (Dal.ProductCatalog pc in products)
            //{
            //    sb.AppendLine(String.Format("update ProductCatalog set Ean = '{0}' where SupplierId = 15 and Ean is null and Code='{1}'", pc.Ean, pc.Code));

            //}

            //string t = sb.ToString();

             Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper(); 

            try
            {
                
                List<Dal.ProductCatalog> productsFromCatalog = pch.GetProductCatalogBySupplier(new int[] { SupplierId });

                foreach (Dal.ProductCatalog productsExisting in productsFromCatalog)
                {
                    try
                    {
                        
                        Dal.ProductCatalog pc = products.Where(x => productsExisting.Ean != null && productsExisting.Ean.Trim().ToLower() == x.Ean.Trim().ToLower()).FirstOrDefault();
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
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.SendError(ex, String.Format("Altavola ProductCatalogId {0}", productsExisting.ProductCatalogId));
                        continue;
                    }
                }

                string[] eanInDb = productsFromAltavolaInDb.Select(x => x.Ean.ToLower()).ToArray();
                List<Dal.ProductCatalog> productsFromAltavolaNotInDb = products.Where(x => !eanInDb.Contains(x.Ean.ToLower()) && !String.IsNullOrEmpty(x.Code)).ToList();

                pch.SetProductCatalogAltavolaAddNew(productsFromAltavolaNotInDb);

                
                SetConfiguration();


                LoadImages();
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, "Altavola import");
                throw ex;
            }
        }

        private void SetConfiguration()
        {
            DataTable table = new DataTable();
            //columns  

            foreach(string column in columns)

            table.Columns.Add(column, typeof(string));




            foreach (AltConf ac in conf.Where(x=>x.Ean!=""&&x.Ean!=null).ToList())
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

        private void LoadImages()
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> productsFromCatalog = pch.GetProductCatalogBySupplier(new int[] { SupplierId }).Where(x=>x.Ean!=null && x.ImageId ==null).ToList();


            foreach(Dal.ProductCatalog pc in productsFromCatalog)
            {
                AltConf ac = conf.Where(x => x.Ean == pc.Ean).FirstOrDefault();

                if (ac == null)
                    continue;

                foreach (string url in ac.Imgs)
                    DownloadImage(url, pc.ProductCatalogId);
            }
        }

        public static void ToCSV(DataTable dtDataTable, string strFilePath)
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
        public   List<Dal.ProductCatalog> GetProductCatalog(CeneoHelper.Offers pm )
        {
            List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();

            string s = String.Join(",", pm.O.Select(x => x.Cat).Distinct().ToArray());
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier supplier = oh.GetSupplier(SupplierId);


            List<string> series = new List<string>();
             
            List<Dal.SupplierDeliveryType> deliveries = pch.GetSupplierDeliveryTypes();

            List<string> producers = new List<string>();
            foreach (CeneoHelper.O offer in pm.O)
            {
                string code = "";
                string ean = null;
                string serie = "";
                int supplierId = SupplierId;
                bool notAltavola = false;

                Dictionary<string, string> attributes = new Dictionary<string, string>();
                foreach (CeneoHelper.A att in offer.Attrs.A)
                {

                    if (!columns.Contains(att.Name.ToLower()))
                        columns.Add(att.Name.ToLower());

                    if (!attributes.Keys.Contains(att.Name.ToLower()))
                        attributes.Add(att.Name.ToLower(), att.Text);

                    if (att.Name.ToLower() == "ean" && !String.IsNullOrEmpty(att.Text))
                        ean = att.Text.Trim();
                    if (att.Name.ToLower() == "kod-av" && !String.IsNullOrEmpty(att.Text))
                        code = att.Text.Trim();
                    if (att.Name.ToLower() == "kod_producenta" && !String.IsNullOrEmpty(att.Text))
                        code = att.Text.Trim();
                    if (att.Name.ToLower() == "seria" && !String.IsNullOrEmpty(att.Text))
                        serie = att.Text.Trim();
                    if (att.Name.ToLower() == "producent" && !String.IsNullOrEmpty(att.Text))
                    { 
                        producers.Add(att.Text.Trim());
                        switch (att.Text.Trim())
                        { 
                            case "ALTAVOLA DESIGN":   
                            case "Prestige by ALTAVOLA": break;
                            default: notAltavola = true; break;
                        }

                    }

                }


                if (notAltavola)
                    continue;

                AltConf ac = new AltConf();

                ac.Imgs = new List<string>();

                if (offer.Imgs!=null)
                {
                    ac.Imgs.Add(offer.Imgs.Main.Url);

                    foreach (CeneoHelper.I img in offer.Imgs.I)
                    {

                        ac.Imgs.Add(img.Url);
                    }

                }


                ac.Ean = ean;
                ac.Dict = attributes;
                conf.Add(ac);
                

                Dal.ProductCatalog product = Dal.ProductCatalogHelper.GetProductCatalog(supplierId, code.Trim(), offer.Stock != "0");
                product.Name = offer.Name;
                product.Specification = offer.Desc; 
                product.PurchasePrice = 
                    (decimal)(Decimal.Parse(offer.PriceWholesale , System.Globalization.CultureInfo.InvariantCulture) * (1 - supplier.Rebate) / (1+Dal.Helper.VAT));
                product.Ean = ean;
                product.PriceBruttoFixed = Decimal.Parse(offer.OldPrice, System.Globalization.CultureInfo.InvariantCulture);
                product.ExternalId = offer.Id;
                product.SupplierQuantity = Convert.ToInt32(offer.Stock);

                //if(offer.IsInPromotion=="yes")
                //{
                //    product.PriceBruttoPromo = Decimal.Parse(offer.Price, System.Globalization.CultureInfo.InvariantCulture);
                //    product.PriceBruttoPromoDate = DateTime.Today.AddDays(2).AddMinutes(-1);
                //    product.IsActivePricePromo = true;
                //}
                //else
                //{
                //    product.PriceBruttoPromo = null;
                //    product.PriceBruttoPromoDate = null;
                //    product.IsActivePricePromo = false;
                //}

                if (product.Ean == null && ean != null)
                    product.Ean = ean;


                int deliveryDays = 0;
                if(Int32.TryParse(offer.DeliveryDays, out deliveryDays))
                {
                    if(deliveryDays>0)
                    {
                        if (deliveryDays > supplier.DeliveryId)
                            product.DeliveryId = deliveries.Where(x => x.DeliveryId <= deliveryDays).OrderByDescending(x => x.DeliveryId).FirstOrDefault().DeliveryId;
                        else
                            product.DeliveryId = null;
                    }

                }

             

                products.Add(product);

            }
            var p = products.Where(x => String.IsNullOrEmpty(x.Ean)).ToList();
            producers = producers.Distinct().ToList();

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
