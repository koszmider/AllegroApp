using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper.Configuration;
using System.Net;
using CsvHelper.TypeConversion;
using CsvHelper;

namespace LajtIt.Bll
{
    public class CosmoLightHelper : ImportData, IImportData
    {
        public class CosmoLightImport
        {
            public string ean { get; set; }
            public string name { get; set; }
            public string sku { get; set; }
            public string availability { get; set; }
            public string price { get; set; }
            public string max_power { get; set; }
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
                csv.Configuration.Delimiter = ",";
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<FooMap>();
                List<CosmoLightImport> records = csv.GetRecords<CosmoLightImport>().ToList();
                ProcessData(records);
            }

            oh.SetSupplierImportDate(SupplierId, DateTime.Now);

        }




        public sealed class FooMap : ClassMap<CosmoLightImport>
        {
            public FooMap()
            {
                Map(m => m.ean).Name("ean");
                Map(m => m.name).Name("name");
                Map(m => m.sku).Name("sku");
                Map(m => m.availability).Name("availability");
                Map(m => m.price).Name("price");
                Map(m => m.max_power).Name("moc-maksymalna");
            }
        }
        public new void LoadData<T>()
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

        public void ProcessData(List<CosmoLightImport> obj)
        {
            List<CosmoLightImport> rtr = new List<CosmoLightImport>();
            foreach (CosmoLightImport o in obj)
            {
                o.price = o.price.Replace('.', ',');
                if (!o.max_power.Trim().Equals(""))
                    rtr.Add(o);
            }

            if (rtr.Count == 0)
            {
                Bll.ErrorHandler.SendEmail("Plik CosmoLight zwraca zero produktów, prawdp. zmieniła się struktura", Dal.Helper.DevEmail);
                return;
            }


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                .Where(x => x.IsDiscontinued == false)
                .ToList();



            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = rtr.Where(x => x.ean == pc.Ean).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = 0;

                    }
                    else
                    {
                        int stanyVal = 0;
                        if (r.availability.Equals("Dostępny")) stanyVal = 1;
                        pc.IsAvailable = Convert.ToInt32(stanyVal) > 0;
                        pc.SupplierQuantity = null;
                        pc.PriceBruttoFixed = Convert.ToDecimal(r.price);
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu CosmoLight ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, pc.Code));

                }
            }

            string[] eanInDb = products.Select(x => x.Ean).Distinct().ToArray();

            string[] eanNotInDb = rtr.Where(x => !eanInDb.Contains(x.ean)).Select(x => x.ean).Distinct().ToArray();

            var oToAdd = rtr.Where(x => eanNotInDb.Contains(x.ean)).ToList();

            List<Dal.ProductCatalog> pcsToAdd = new List<Dal.ProductCatalog>();


            foreach (CosmoLightImport ci in oToAdd)
            {
                int stanyVal = 0;
                if (ci.availability.Equals("Dostępny")) stanyVal = 1;
                Dal.ProductCatalog pcToAdd = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, ci.sku.Trim(), Convert.ToInt32(stanyVal) > 0);

                pcToAdd.Ean = ci.ean == "" ? null : ci.ean;
                pcToAdd.SupplierQuantity = null;
                pcToAdd.PriceBruttoFixed = Convert.ToDecimal(ci.price);

                if (pcsToAdd.Where(x => x.Code == pcToAdd.Code).Count() > 0)
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
