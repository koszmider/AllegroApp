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
using OpenPop.Pop3;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace LajtIt.Bll
{
    public class NordluxHelper : ImportData, IImportData
    {
        public class NordluxImport
        {
            public string Code { get; set; }
            public string Ean { get; set; }
            public string Stock { get; set; }
            public bool IsAvailable
            {
                get
                {

                    if (!Stock.Equals("Not in stock"))
                        return Stock.Trim() != "0";
                    else
                        return false;
                }
            }
        }

        public sealed class FooMap1 : ClassMap<NordluxImport>
        {
            public FooMap1()
            {
                Map(m => m.Code).Index(0);
                Map(m => m.Ean).Index(1);
                Map(m => m.Stock).Index(6);
            }
        }


        public class Int32Converter<T> : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return Int32.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        public class NordluxXlsxFile1
        {
            [ExcelColumn("Item no.")]
            public string Code { get; set; }
            [ExcelColumn("EAN no.")]
            public string Ean { get; set; }
            [ExcelColumn("Description")]
            public string Name { get; set; }
            [ExcelColumn("Area")]
            public string Area { get; set; }
            [ExcelColumn("Type")]
            public string Type { get; set; }
            [ExcelColumn("Novelty")]
            public string Novelty { get; set; }
            [ExcelColumn("Stock status")]
            public string Stock { get; set; }

        }

        public class NordluxXlsxFile
        {
            [ExcelColumn("Kod produktu/Product Code")]
            public string KodProduktu { get; set; }
            [ExcelColumn("Lokalizacja")]
            public string Lokalizacja { get; set; }
            [ExcelColumn("Ilość / QTY in Stock")]
            public int Ilosc { get; set; }
            [ExcelColumn("uwagi")]
            public string uwagi { get; set; }


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

                client.DownloadFile(/*supplier.ImportUrl*/@"https://lightingbrands-my.sharepoint.com/personal/export_lighting-brands_com/_layouts/15/download.aspx?UniqueId=9b065570%2Dc6d4%2D4619%2D836a%2Dbb3cd2f28576", fileName);
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd wczytywania pliku: {0} ", supplier.Name));

            }


            ProcessData(fileName);
        }

        public void ProcessData(string fileName)
        {
            ExcelQueryFactory eqf = new ExcelQueryFactory(fileName);


            var r = from p in eqf.WorksheetRange<NordluxXlsxFile>("A1", "D1500", 0) select p;



            var rr = r.ToList();

            rr = rr.Where(x => x.KodProduktu != null).ToList();

            if (rr.Count == 0)
            {
                Bll.ErrorHandler.SendEmail("Plik Nordlux nie zwraca danych. Sprawdź jego strukturę");
                return;
            }




            List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();

            foreach (NordluxXlsxFile max in rr)
            {
                Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, max.KodProduktu, max.Ilosc > 0);

                pc.SupplierQuantity = max.Ilosc;

                pc.IsAvailable = pc.SupplierQuantity > 0;

                if (max.KodProduktu != null)
                    max.KodProduktu = max.KodProduktu.Trim();

                pc.Code = max.KodProduktu;

                products.Add(pc);
            }

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();



            Dal.OrderHelper oh = new Dal.OrderHelper();


            pch.SetProductCatalogUpdateNordlux(products);

            oh.SetSupplierImportDate(SupplierId, DateTime.Now);


        }

        public void ReadMailbox()
        {
            try
            {

                using (Pop3Client client = new Pop3Client())
                {
                    // Connect to the server
                    client.Connect(System.Configuration.ConfigurationManager.AppSettings["SMTP_Server"],
                        995, true);
                    Dal.SettingsHelper sh = new Dal.SettingsHelper();
                    Dal.Settings se = sh.GetSetting("EM_E_INT");
                    Dal.Settings sp = sh.GetSetting("EM_P_INT");
                    // Authenticate ourselves towards the server
                    client.Authenticate(se.StringValue, sp.StringValue);

                    // Get the number of messages in the inbox
                    int messageCount = client.GetMessageCount();

                    // We want to download all messages
                    List<OpenPop.Mime.Message> allMessages = new List<OpenPop.Mime.Message>(messageCount);




                    // Messages are numbered in the interval: [1, messageCount]
                    // Ergo: message numbers are 1-based.
                    // Most servers give the latest message the highest number
                    for (int i = messageCount; i > 0; i--)
                    {
                        OpenPop.Mime.Message oMail = client.GetMessage(i);

                        //if (lastEmail != null && lastEmail.SentDate >= oMail.Headers.DateSent.ToLocalTime())
                        //    break;



                        if (oMail.Headers.Subject.Contains("Nordlux"))
                        {


                            foreach (var attachment in oMail.FindAllAttachments())
                            {
                                string filePath = Path.Combine(@"C:\Attachment", attachment.FileName);
                                if (attachment.FileName.EndsWith(".xlsx"))
                                {
                                    string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

                                    string fileName = String.Format("Nordlux_{0:yyyyMMddHHmm}.xlsx", DateTime.Now);
                                    string saveLocation = String.Format(path, fileName);

                                    FileStream Stream = new FileStream(saveLocation, FileMode.Create);
                                    BinaryWriter BinaryStream = new BinaryWriter(Stream);
                                    BinaryStream.Write(attachment.Body);
                                    BinaryStream.Close();

                                    ProcessFile(saveLocation);
                                    break;
                                }
                            }

                        }


                    }
                }
                Console.WriteLine("Completed!");
            }
            catch (Exception ep)
            {
                Console.WriteLine(ep.Message);
            }

        }
        
        private void ProcessFile(string saveLocation)
        {
            ExcelQueryFactory eqf = new ExcelQueryFactory(saveLocation);

            var r = from p in eqf.Worksheet<NordluxXlsxFile1>(0) select p;

            var rr = r.Select(x => new NordluxImport()
            {
                Stock = x.Stock,
                Ean = x.Ean,
                Code = x.Code
            }
            ).ToList();

            ProcessData(rr);

        }

        public void ProcessData(List<NordluxImport> obj)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                .ToList();

            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = obj.Where(x => x.Ean == pc.Ean).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = true;
                        pc.SupplierQuantity = null;
                    }
                    else
                    {
                        pc.SupplierQuantity = null;
                        pc.IsAvailable = false;
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Nordlux ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, pc.Code));
                }
            }

            pch.SetProductCatalogUpdateNordlux(products);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(SupplierId, DateTime.Now);
        }

        public void ProcessData<T>(T obj)
        {
            throw new NotImplementedException();
        }

    }
}
