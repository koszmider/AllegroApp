using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public class LampexHelper : ImportData, IImportData
    {

        public new void LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }

        public void ProcessData<T>(T obj)
        {
            try
            {
                Bll.LampexHelper.Offers pm = obj as Bll.LampexHelper.Offers;


                List<Dal.ProductCatalogGroup> groups = GetProductCatalogGroups(pm, SupplierId);

                Dal.ProductCatalogGroupHelper pchg = new Dal.ProductCatalogGroupHelper();
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();


                pchg.SetProductCatalogGroups(groups);

                groups = pchg.GetProductCatalogGroups(SupplierId);

                List<Dal.ProductCatalog> products = GetProductCatalog(pm, groups, SupplierId);


                pch.SetProductCatalogUpdateLampex(products);

                List<ProductCatalog> productForSupplier = pch.GetProductCatalogBySupplier(new int[] { SupplierId });

                GetImages(pm, productForSupplier);
            }
            catch (Exception ex)
            {

                Bll.ErrorHandler.SendError(ex, "Lampex przetwarzanie pliku");
            }
        }

        private void GetImages(Offers pm, List<ProductCatalog> productForSupplier)
        { 
            foreach (LampexHelper.Product offer in pm.Product)
            {
                string ean = offer.Ean;
                if (String.IsNullOrEmpty(ean))
                    continue;

                Dal.ProductCatalog productFromCatalog = productForSupplier.Where(x => x.Ean == offer.Ean)
                    .Where(x => x.ImageId.HasValue == false)
                    .FirstOrDefault();

                if (productFromCatalog == null)
                    continue;

                 GetImage(productFromCatalog.ProductCatalogId, offer.Img);


            } 
        }

        private void GetImage(int productCatalogId, string img)
        {
            AltavolaHelper.DownloadImage(img, productCatalogId);
        }

        private List<ProductCatalogGroup> GetProductCatalogGroups(Offers pm, int supplierId)
        {
            List<Dal.ProductCatalogGroup> groups = new List<Dal.ProductCatalogGroup>();
            Dal.ProductCatalogGroupHelper pch = new ProductCatalogGroupHelper();
            int familyId = pch.GetProductCatalogFamilyDefault(supplierId);
            foreach (LampexHelper.Product offer in pm.Product)
            {
                string name = offer.Name;
                name = name.Replace("Kinkiet","");
                name = name.Replace("Żyradol", "");
                name = name.Replace("Żyrandol", "");
                name = name.Replace("Żarówka dekoracyjna", "");
                name = name.Replace("Lampa wiszaca","");
                name = name.Replace("Lampa Wiszaca","");
                name = name.Replace("Lampa Wisząca", "");
                name = name.Replace("Lampa wisząca", "");
                name = name.Replace("Lampa podłogowa", "");
                name = name.Replace("Lampa Podłogowa", "");
                name = name.Replace("Lampa stojąca", "");
                name = name.Replace("Lampa stołowa", "");
                name = name.Replace("Lampa sufitowa", "");
                name = name.Replace("Lampa średnia","");
                name = name.Replace("Lampa mała","");
                name = name.Replace("Lampka mala", "");
                name = name.Replace("Lampka mała", "");
                name = name.Replace("Listwa","");
                name = name.Replace("Plafon","");
                name = name.Replace("Zyrandol", "");

                name = name.Trim();

                if (groups.Where(x => x.GroupName == name).FirstOrDefault() == null)
                    groups.Add(new Dal.ProductCatalogGroup()
                    {
                        GroupName = name.Trim(),
                        SupplierId = supplierId,
                        FamilyId= familyId
                    });
            }
            return groups.Distinct().ToList();
        }

        public static List<Dal.ProductCatalog> GetProductCatalog(LampexHelper.Offers pm, List<Dal.ProductCatalogGroup> groups, int supplierId)
        {
            List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();




            foreach (LampexHelper.Product offer in pm.Product)
            {
                int groupId = 1;

                string name = offer.Name;
                name = name.Replace("Kinkiet", "");
                name = name.Replace("Żyradol", "");
                name = name.Replace("Żyrandol", "");
                name = name.Replace("Żarówka dekoracyjna", "");
                name = name.Replace("Lampa wiszaca", "");
                name = name.Replace("Lampa Wiszaca", "");
                name = name.Replace("Lampa Wisząca", "");
                name = name.Replace("Lampa wisząca", "");
                name = name.Replace("Lampa podłogowa", "");
                name = name.Replace("Lampa Podłogowa", "");
                name = name.Replace("Lampa stojąca", "");
                name = name.Replace("Lampa stołowa", "");
                name = name.Replace("Lampa sufitowa", "");
                name = name.Replace("Lampa średnia", "");
                name = name.Replace("Lampa mała", "");
                name = name.Replace("Lampka mala", "");
                name = name.Replace("Lampka mała", "");
                name = name.Replace("Listwa", "");
                name = name.Replace("Plafon", "");
                name = name.Replace("Zyrandol", "");
                name = name.Trim();
                Dal.ProductCatalogGroup group = groups.Where(x => x.GroupName == name).FirstOrDefault();
                if (group != null)
                    groupId = group.ProductCatalogGroupId;



                Dal.ProductCatalog product = Dal.ProductCatalogHelper.GetProductCatalog(supplierId, offer.Kod_producenta.Trim(), offer.Stock != "0");


                product.Name = offer.Name;
                //product.ProductCatalogGroupId = groupId;

                //throw new NotImplementedException("PG");
                product.SupplierId = supplierId;
                
                product.ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct;
                product.AutoAssignProduct = true;
                product.Ean = offer.Ean;
                product.PriceBruttoFixed = Decimal.Parse(offer.Price.Replace(".", ",")) * (1 + Dal.Helper.VAT);
                product.ImageId = null;
                product.IsDiscontinued = false; 
                product.PriceBruttoPromo = null;
                product.PriceBruttoPromoDate = null; 
                product.ExternalId = offer.Id;
                product.IsHidden = false;
                product.IsOnStock = false;

                int quantity = 0;
                if (Int32.TryParse(offer.Stock, out quantity))
                    product.SupplierQuantity = quantity;
                else
                    product.SupplierQuantity = null;




                products.Add(product);

            }

            return products;
        }

        [XmlRoot(ElementName = "product")]
        public class Product
        {
            [XmlElement(ElementName = "id")]
            public string Id { get; set; }
            [XmlElement(ElementName = "url")]
            public string Url { get; set; }
            [XmlElement(ElementName = "brand")]
            public string Brand { get; set; }
            [XmlElement(ElementName = "price")]
            public string Price { get; set; }
            [XmlElement(ElementName = "stock")]
            public string Stock { get; set; }
            [XmlElement(ElementName = "desc")]
            public string Desc { get; set; }
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "category")]
            public string Category { get; set; }
            [XmlElement(ElementName = "ean")]
            public string Ean { get; set; }
            [XmlElement(ElementName = "kod_producenta")]
            public string Kod_producenta { get; set; }
            [XmlElement(ElementName = "img")]
            public string Img { get; set; }
            [XmlElement(ElementName = "images_url_2")]
            public string Images_url_2 { get; set; }
            [XmlElement(ElementName = "images_url_3")]
            public string Images_url_3 { get; set; }
            [XmlElement(ElementName = "images_url_4")]
            public string Images_url_4 { get; set; }
            [XmlElement(ElementName = "images_url_5")]
            public string Images_url_5 { get; set; }
        }

        [XmlRoot(ElementName = "offers")]
        public class Offers
        {
            [XmlElement(ElementName = "product")]
            public List<Product> Product { get; set; }
            [XmlAttribute(AttributeName = "version")]
            public string Version { get; set; }
        }
    }


}



