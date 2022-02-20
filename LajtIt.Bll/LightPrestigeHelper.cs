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
    public class LightPrestigeHelper : ImportData, IImportData
    {
        #region xml
        [XmlRoot(ElementName = "linki_do_zdjec")]
        public class Linki_do_zdjec
        {
            [XmlElement(ElementName = "link_do_zdjecia")]
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
            [XmlElement(ElementName = "Typ")]
            public string Typ { get; set; }
            [XmlElement(ElementName = "Kategoria")]
            public string Kategoria { get; set; }
            [XmlElement(ElementName = "Stan_mag")]
            public string Stan_mag { get; set; }
            [XmlElement(ElementName = "Cena_z_cennika")]
            public string Cena_z_cennika { get; set; }
            [XmlElement(ElementName = "Cena_kat_brutto")]
            public string Cena_kat_brutto { get; set; }
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
            [XmlElement(ElementName = "Rozmiar")]
            public string Rozmiar { get; set; }
            [XmlElement(ElementName = "Material_przew")]
            public string Material_przew { get; set; }
            [XmlElement(ElementName = "Material_dod")]
            public string Material_dod { get; set; }
            [XmlElement(ElementName = "Typ_oprawy")]
            public string Typ_oprawy { get; set; }
            [XmlElement(ElementName = "Ilosc_zrodel")]
            public string Ilosc_zrodel { get; set; }
            [XmlElement(ElementName = "Moc_zarowki")]
            public string Moc_zarowki { get; set; }
            [XmlElement(ElementName = "Kolor")]
            public string Kolor { get; set; }
            [XmlElement(ElementName = "Uwagi")]
            public string Uwagi { get; set; }
            [XmlElement(ElementName = "Kolekcja")]
            public string Kolekcja { get; set; }
            [XmlElement(ElementName = "Przedzial_cen")]
            public string Przedzial_cen { get; set; }
            [XmlElement(ElementName = "linki_do_zdjec")]
            public Linki_do_zdjec Linki_do_zdjec { get; set; }
            [XmlElement(ElementName = "opis")]
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

            Dal.Supplier supplier = Dal.DbHelper.ProductCatalog.GetSuppliers(new int[] { SupplierId }).FirstOrDefault();

            List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();
            foreach(Produkt produkt in pm.Produkt)
            {
                try

                {
                    Dal.ProductCatalog p = new Dal.ProductCatalog()
                    {
                        Code = produkt.Indeks,
                        Ean = produkt.Ean,
                        PriceBruttoFixed = Decimal.Parse(produkt.Cena_kat_brutto.Replace(".", ",")),
                        //IsAvailable = Decimal.Parse(produkt.Stan_mag.Replace(".", ",")) > 0,
                        ExternalId = produkt.Indeks
                    };

                    if (UpdatePurchasePrice)
                        p.PurchasePrice = p.PriceBruttoFixed * (1 - supplier.Rebate) / (1 + Dal.Helper.VAT);

                    if (String.IsNullOrEmpty(produkt.Stan_mag))
                    {
                        p.IsAvailable = false;

                        p.SupplierQuantity = 0;
                    }
                    else
                    {
                        p.SupplierQuantity = Int32.Parse(produkt.Stan_mag);

                        p.IsAvailable = Int32.Parse(produkt.Stan_mag)>0;
                    }
                        products.Add(p);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd w aktualizacji danych pliku LightPrestige, Nazwa: {0}, Ean: {1}", produkt.Nazwa, produkt.Ean));
                }

            }
            Dal.LightPrestigeHelper sh = new Dal.LightPrestigeHelper();
            string[]  eanToAdd = sh.SetUpdateProducts(products);


            List<Dal.ProductCatalog> productsToAdd = new List<Dal.ProductCatalog>();

            foreach(string ean in eanToAdd)
            {
                Produkt p = pm.Produkt.Where(x => x.Ean == ean).FirstOrDefault();
                if(p!=null)
                {
                    Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, p.Indeks, Decimal.Parse(p.Stan_mag.Replace(".", ",")) > 0);
                    pc.Ean = String.IsNullOrEmpty(p.Ean)?null: p.Ean;
                    pc.Name = p.Nazwa.Substring(0, Math.Min(p.Nazwa.Length, 100));
                    pc.PriceBruttoFixed = Decimal.Parse(p.Cena_kat_brutto.Replace(".", ","));
                    pc.Specification = p.Opis;
                    productsToAdd.Add(pc);
                }

            }

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetProductCatalogs(productsToAdd, SupplierId);

            List<Dal.ProductCatalog> addedProducts = pch.GetProductCatalogBySupplier(new int[] { SupplierId }).Where(x => x.ImageId == null).ToList();

           
            foreach (Produkt produkt in pm.Produkt)
            {
                //Produkt p = pm.Produkt.Where(x => x.Indeks==pro).FirstOrDefault();
                Dal.ProductCatalog existingProduct = addedProducts.Where(x => x.Code == produkt.Indeks).FirstOrDefault();

                if (existingProduct == null)
                    continue;

                if (produkt.Linki_do_zdjec != null && produkt.Linki_do_zdjec.Link_do_zdjecia != null && produkt.Linki_do_zdjec.Link_do_zdjecia.Count > 0)
                {

                    string[] photos = produkt.Linki_do_zdjec.Link_do_zdjecia.ToArray();

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
