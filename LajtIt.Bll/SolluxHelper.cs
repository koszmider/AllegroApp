using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class SolluxHelper : ImportData, IImportData
    { 
        #region xml
        [XmlRoot(ElementName = "Linki_do_zdjec")]
        public class Linki_do_zdjec
        {
            [XmlElement(ElementName = "Link_do_zdjecia")]
            public List<string> Link_do_zdjecia { get; set; }
        }

        [XmlRoot(ElementName = "Produkt")]
        public class Produkt
        {
            [XmlElement(ElementName = "Marka")]
            public string Marka { get; set; }
            [XmlElement(ElementName = "Indeks")]
            public string Indeks { get; set; }
            [XmlElement(ElementName = "Nazwa")]
            public string Nazwa { get; set; }
            [XmlElement(ElementName = "Ean")]
            public string Ean { get; set; }
            [XmlElement(ElementName = "Kategoria")]
            public string Kategoria { get; set; }
            [XmlElement(ElementName = "Stan_mag")]
            public string Stan_mag { get; set; }
            [XmlElement(ElementName = "Cena_zakupu_netto")]
            public string Cena_zakupu_netto { get; set; }
            [XmlElement(ElementName = "Cena_zakupu_brutto")]
            public string Cena_zakupu_brutto { get; set; }
            [XmlElement(ElementName = "Vat")]
            public string Vat { get; set; }
            [XmlElement(ElementName = "Szt_dlugosc")]
            public string Szt_dlugosc { get; set; }
            [XmlElement(ElementName = "Szt_szerokosc")]
            public string Szt_szerokosc { get; set; }
            [XmlElement(ElementName = "Szt_wysokosc")]
            public string Szt_wysokosc { get; set; }
            [XmlElement(ElementName = "Szt_waga_netto")]
            public string Szt_waga_netto { get; set; }
            [XmlElement(ElementName = "Szt_waga_brutto")]
            public string Szt_waga_brutto { get; set; }
            [XmlElement(ElementName = "Linki_do_zdjec")]
            public Linki_do_zdjec Linki_do_zdjec { get; set; }
            [XmlElement(ElementName = "Opis")]
            public string Opis { get; set; }
        }

        [XmlRoot(ElementName = "Document")]
        public class Document
        {
            [XmlElement(ElementName = "Produkt")]
            public List<Produkt> Produkt { get; set; }
        }
        #endregion


        public new void LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }



        public void ProcessData<T>(T obj)
        {
            Document pm = obj as Document;

            List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();
            foreach(Produkt produkt in pm.Produkt)
            {
                try

                {
                    products.Add(new Dal.ProductCatalog()
                    {
                        Ean = produkt.Ean,
                        PriceBruttoFixed = Decimal.Parse(produkt.Cena_zakupu_brutto.Replace(".", ",")),
                        IsAvailable = Decimal.Parse(produkt.Stan_mag.Replace(".", ",")) > 0,
                        ExternalId = produkt.Indeks
                    });
                }
                catch (Exception ex)
                {

                    Bll.ErrorHandler.SendEmail(String.Format("Błąd w aktualizacji danych pliku Sollux, Nazwa: {0}, Ean: {1}", produkt.Nazwa, produkt.Ean));
                }

            }
            Dal.SolluxHelper sh = new Dal.SolluxHelper();
            string[]  eanToAdd = sh.SetUpdateProducts(products);


            List<Dal.ProductCatalog> productsToAdd = new List<Dal.ProductCatalog>();

            foreach(string ean in eanToAdd)
            {
                Produkt p = pm.Produkt.Where(x => x.Ean == ean).FirstOrDefault();
                if(p!=null)
                {
                    Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, p.Indeks, Decimal.Parse(p.Stan_mag.Replace(".", ",")) > 0);
                    pc.Ean = p.Ean;
                    pc.Name = p.Nazwa.Substring(0, Math.Min(p.Nazwa.Length, 100));
                    pc.PriceBruttoFixed = Decimal.Parse(p.Cena_zakupu_brutto.Replace(".", ","));
                    pc.Specification = p.Opis;
                    productsToAdd.Add(pc);
                }

            }

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetProductCatalogs(productsToAdd, SupplierId);

            List<Dal.ProductCatalog> addedProducts = pch.GetProductCatalogBySupplier(new int[] { SupplierId }).Where(x => x.ImageId == null).ToList();


            foreach (string ean in eanToAdd)
            {
                Produkt p = pm.Produkt.Where(x => x.Ean == ean).FirstOrDefault();
                Dal.ProductCatalog existingProduct = addedProducts.Where(x => x.Ean == ean).FirstOrDefault();

                if (p == null || existingProduct == null)
                    continue;

                if (p.Linki_do_zdjec != null && p.Linki_do_zdjec.Link_do_zdjecia != null && p.Linki_do_zdjec.Link_do_zdjecia.Count > 0)
                {

                    string[] photos = p.Linki_do_zdjec.Link_do_zdjecia.ToArray();

                    foreach (string photo in photos)
                    {
                        if (String.IsNullOrEmpty(photo))
                            continue;

                        AltavolaHelper.DownloadImage(photo, existingProduct.ProductCatalogId);
                    }
                }
            }


            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(SupplierId, DateTime.Now);


        }
    }
}
