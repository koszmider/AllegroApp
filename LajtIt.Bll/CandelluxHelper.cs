using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class CandelluxHelper: ImportData, IImportData
    { 
        public class CandelluxImport
        { 
            public string Indeks { get; set; }
            public string EAN { get; set; }
            public string Nazwa_handlowa { get; set; }
            public string Material { get; set; }
            public string Kolor_podst { get; set; }
            public string Klasa_energetyczna { get; set; }
            public string Stan_magazynowy { get; set; } 
            public string Cena_detaliczna_brutto { get; set; }
            public string Wysokosc_prod { get; set; }
            public string Szerokosc_prod { get; set; }
            public string Glebokosc_prod { get; set; }
            public string Separator_wymiaru { get; set; }
            public string Srednica_prod { get; set; }
            public string Waga_netto { get; set; }
            public string Wymiar_opak { get; set; }
            public string Link_foto { get; set; }
            public string Aranzacja_1 { get; set; }
            public string Aranzacja_2 { get; set; }
            public string Aranzacja_3 { get; set; }
            public string Aranzacja_4 { get; set; }
            public string Aranzacja_5 { get; set; }
            public string Fotografie { get; set; } 
             

        }
        internal void GetFile()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier supplier = oh.GetSupplier(SupplierId);

            string remoteUri = supplier.ImportUrl;


            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format("{1}_{0:yyyyMMddHHss}.csv", DateTime.Now, supplier.Name);

            string saveLocation = String.Format(path, fileName);

            try
            {

                // Create a new WebClient instance.
                using (WebClient myWebClient = new WebClient())
                { 
                    myWebClient.DownloadFile(remoteUri, saveLocation);
                }
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", supplier.Name));
                return;

            }



            using (TextReader reader = File.OpenText(saveLocation)) 
            {


                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.HasHeaderRecord = true;


                if (SupplierId == 50)
                {
                    csv.Configuration.RegisterClassMap<FooMap>(); //hack bo zmieniaja konf pliku
                   
                }
                else
                {
                    csv.Configuration.RegisterClassMap<FooMap1>();

                }



                List<CandelluxImport> records = csv.GetRecords<CandelluxImport>().ToList();

                // odkąd candellux podaje równiez Apeti trzeba te kody wywalic
                if (SupplierId == 50)
                {
                    records = records.Where(x => !x.Indeks.StartsWith("A")).ToList();
                }
                ProcessData(records);
                GetImages(records);

            }


            oh.SetSupplierImportDate(SupplierId, DateTime.Now);

        }


        private void GetImages(List<CandelluxImport> records)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> productForSupplier = pch.GetProductCatalogBySupplier(new int[] { SupplierId });

            foreach (CandelluxImport offer in records)
            {
                string ean = offer.EAN;
                if (String.IsNullOrEmpty(ean))
                    continue;

                Dal.ProductCatalog productFromCatalog = productForSupplier.Where(x => x.Ean == offer.EAN)
                    .Where(x => x.ImageId.HasValue == false)
                    .FirstOrDefault();

                if (productFromCatalog == null)
                    continue;

                List<string> images = new List<string>();
                images.Add(offer.Link_foto);
                GetImage(productFromCatalog.ProductCatalogId, offer.Link_foto);

                images.Add(offer.Aranzacja_1);
                images.Add(offer.Aranzacja_2);
                images.Add(offer.Aranzacja_3);
                images.Add(offer.Aranzacja_4);
                images.Add(offer.Aranzacja_5);
                if(offer.Fotografie!=null)
                images.AddRange(offer.Fotografie.Split(new char[] { ',' }));
                //if (!String.IsNullOrEmpty(offer.Aranzacja_1)) GetImage(productFromCatalog.ProductCatalogId, offer.Aranzacja_1);
                //if (!String.IsNullOrEmpty(offer.Aranzacja_2)) GetImage(productFromCatalog.ProductCatalogId, offer.Aranzacja_2);
                //if (!String.IsNullOrEmpty(offer.Aranzacja_3)) GetImage(productFromCatalog.ProductCatalogId, offer.Aranzacja_3);
                //if (!String.IsNullOrEmpty(offer.Aranzacja_4)) GetImage(productFromCatalog.ProductCatalogId, offer.Aranzacja_4);
                //if (!String.IsNullOrEmpty(offer.Aranzacja_5)) GetImage(productFromCatalog.ProductCatalogId, offer.Aranzacja_5);

                foreach (string image in images.Distinct().Where(x => x != offer.Link_foto && !String.IsNullOrEmpty(x)).ToArray())
                {
                    GetImage(productFromCatalog.ProductCatalogId, image);
                }

            }
        }

        private void GetImage(int productCatalogId, string img)
        {
            AltavolaHelper.DownloadImage(img, productCatalogId);
        }

        public sealed class FooMap : ClassMap<CandelluxImport>
        {
            public FooMap()
            {

                Map(m => m.Indeks).Name("Indeks");
                Map(m => m.EAN).Name("EAN");
                Map(m => m.Nazwa_handlowa).Name("Nazwa_handlowa");
                Map(m => m.Material).Name("Material");
                Map(m => m.Kolor_podst).Name("Kolor_podst");
                Map(m => m.Klasa_energetyczna).Name("Klasa_energetyczna");
                Map(m => m.Stan_magazynowy).Name("Stan_magazynowy");
                Map(m => m.Cena_detaliczna_brutto).Name("Cena_detaliczna_brutto");
                Map(m => m.Wysokosc_prod).Name("Wysokosc_prod");
                Map(m => m.Szerokosc_prod).Name("Szerokosc_prod");
                Map(m => m.Glebokosc_prod).Name("Glebokosc_prod");
                Map(m => m.Separator_wymiaru).Name("Separator_wymiaru");
                Map(m => m.Srednica_prod).Name("Srednica_prod");
                Map(m => m.Waga_netto).Name("Waga_netto");
                Map(m => m.Wymiar_opak).Name("Wymiar_opak");
                Map(m => m.Link_foto).Name("Link_foto");
                Map(m => m.Aranzacja_1).Name("Aranzacja_1");
                Map(m => m.Aranzacja_2).Name("Aranzacja_2");
                Map(m => m.Aranzacja_3).Name("Aranzacja_3");
                Map(m => m.Aranzacja_4).Name("Aranzacja_4");
                Map(m => m.Aranzacja_5).Name("Aranzacja_5");
            }
        }
        public sealed class FooMap1 : ClassMap<CandelluxImport>
        {
            public FooMap1()
            {

                Map(m => m.Indeks).Name("Indeks");
                Map(m => m.EAN).Name("EAN");
                Map(m => m.Nazwa_handlowa).Name("Nazwa_handlowa");
                Map(m => m.Material).Name("Material");
                Map(m => m.Kolor_podst).Name("Kolor_podst");
                Map(m => m.Klasa_energetyczna).Name("Klasa_energetyczna");
                Map(m => m.Stan_magazynowy).Name("Stan magazynowy");
                Map(m => m.Cena_detaliczna_brutto).Name("Cena_detaliczna_brutto");
                Map(m => m.Wysokosc_prod).Name("Wysokosc_prod");
                Map(m => m.Szerokosc_prod).Name("Szerokosc_prod");
                Map(m => m.Glebokosc_prod).Name("Glebokosc_prod");
                Map(m => m.Separator_wymiaru).Name("Separator_wymiaru");
                Map(m => m.Srednica_prod).Name("Srednica_prod");
                Map(m => m.Waga_netto).Name("Waga_netto");
                Map(m => m.Wymiar_opak).Name("Wymiar_opak");
                Map(m => m.Link_foto).Name("Link_foto");
                Map(m => m.Aranzacja_1).Name("Aranzacja_1");
                Map(m => m.Aranzacja_2).Name("Aranzacja_2");
                Map(m => m.Aranzacja_3).Name("Aranzacja_3");
                Map(m => m.Aranzacja_4).Name("Aranzacja_4");
                Map(m => m.Aranzacja_5).Name("Aranzacja_5");
            }
        }
        public new void  LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }

        public class Int32Converter<T> : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return Int32.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
            } 
        }


        public void ProcessData(List<CandelluxImport> obj)
        { 

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                .Where(x => x.IsDiscontinued == false)
                .ToList();


            //obj = obj.Where(x => !x.Indeks.StartsWith("A")).ToList();

            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = obj.Where(x => x.EAN == pc.Ean).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = 0;

                    }
                    else
                    {
                        pc.PriceBruttoFixed = Convert.ToDecimal(r.Cena_detaliczna_brutto);
                        pc.IsAvailable = Convert.ToInt32(r.Stan_magazynowy) > 0;
                        pc.SupplierQuantity = Convert.ToInt32(r.Stan_magazynowy);
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Candellux ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, pc.Code));

                }
            }
             
            string[] eanInDb = products.Select(x => x.Ean).Distinct().ToArray();

            string[] eanNotInDb = obj.Where(x => !eanInDb.Contains(x.EAN)).Select(x=>x.EAN).Distinct().ToArray();

            var oToAdd = obj.Where(x => eanNotInDb.Contains(x.EAN)).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach(CandelluxImport ci in oToAdd)
            {
                Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.Indeks.Trim(), Convert.ToInt32(ci.Stan_magazynowy) > 0);

                pcToAdd.Ean = ci.EAN==""?null:ci.EAN;
                pcToAdd.SupplierQuantity = Convert.ToInt32(ci.Stan_magazynowy);
                pcToAdd.PriceBruttoFixed = Convert.ToDecimal(ci.Cena_detaliczna_brutto);

                if (pcsToAdd.Where(x => x.Code == pcToAdd.Code).Count()>0)
                    continue;
                pcsToAdd.Add(pcToAdd);
            }


            pch.SetProductCatalogUpdateCandellux(products, SupplierId); 
            pch.SetProductCatalogs(pcsToAdd, SupplierId);

        }

        public void ProcessData<T>(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
