using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using OpenPop.Pop3;
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
    public class ZumalineHelper : ImportData, IImportData
    {
        #region class
        [XmlRoot(ElementName = "product")]
        public class Product
        {
            [XmlElement(ElementName = "id")]
            public string Id { get; set; }
            [XmlElement(ElementName = "softlab")]
            public string Softlab { get; set; }
            [XmlElement(ElementName = "nazwa")]
            public string Nazwa { get; set; }
            [XmlElement(ElementName = "stan")]
            public string Stan { get; set; }
            [XmlElement(ElementName = "twrKod")]
            public string TwrKod { get; set; }
        }

        [XmlRoot(ElementName = "products")]
        public class Products
        {
            [XmlElement(ElementName = "product")]
            public List<Product> Product { get; set; }
        }
        #endregion

        public class ZumalineImport
        {
            public string ExternalId { get; set; }
            public string Code { get; set; }
            public string Ean { get; set; }
            public string Qty { get; set; }
            public int Quantity { get { return (int)Convert.ToDecimal(Qty, new System.Globalization.CultureInfo("en-us")); } }
            //public bool IsAvailable { get { return Quantity > 0; } }
            //public bool IstniejeWBazie { get; set; }

        }
        public class PN_LD_Import
        { 
            public string Code { get; set; }
            public string Ean { get; set; }
            public string Lieferstatus { get; set; } 
            public bool IsAvailable
            {
                get
                {

                    if (Lieferstatus != null)
                        return Lieferstatus.Trim() == "Y";
                    else
                        return false;
                }
            }

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



            System.Threading.Thread.Sleep(30000);

            using (TextReader reader = File.OpenText(saveLocation)) 
            {


                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.HasHeaderRecord = false;
                // List<ZumalineImport> records = csv.GetRecords<ZumalineImport>().ToList();
                List<ZumalineImport> records = csv.GetRecords<ZumalineImport>().ToList();
                ProcessData(records);

            } 
            oh.SetSupplierImportDate(SupplierId, DateTime.Now);
        }

        public sealed class FooMap : ClassMap<ZumalineImport>
        {
            public FooMap()
            {
                Map(m => m.ExternalId).Index(0);
                Map(m => m.Code).Index(1);
                Map(m => m.Ean).Index(2);
                Map(m => m.Qty).Index(3);//.TypeConverter<Int32Converter<Quantity>>();
            }
        }
        public sealed class FooMap1 : ClassMap<PN_LD_Import>
        {
            public FooMap1()
            {
                Map(m => m.Code).Index(0);
                Map(m => m.Ean).Index(1);
                Map(m => m.Lieferstatus).Index(2);//.TypeConverter<Int32Converter<Quantity>>();
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


        public void ProcessData(List<ZumalineImport> obj)
        { 

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                .Where(x => x.IsDiscontinued == false)
                .ToList();
   


 
            foreach (Dal.ProductCatalog pc in products)
            {
                try
                { 
                var r = obj.Where(x => x.Ean == pc.Ean).FirstOrDefault();

                if (r == null)
                {
                    pc.IsAvailable = false;
                    pc.SupplierQuantity = null;
                }
                else
                {
                    pc.SupplierQuantity = r.Quantity;
                    pc.IsAvailable = pc.SupplierQuantity > 0;
                }
                }catch(Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Zumaline ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, pc.Code));

                }
            }

            pch.SetProductCatalogUpdateZumaline(products, SupplierId);
        }
        public void ProcessData<T>(T obj)
        {
            LajtIt.Bll.ZumalineHelper.Products data = obj as LajtIt.Bll.ZumalineHelper.Products;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                .Where(x => x.IsDiscontinued == false).ToList();

            foreach (Dal.ProductCatalog pc in products)
            {
                var r = data.Product.Where(x => x.Softlab == pc.ExternalId).FirstOrDefault();

                if (r == null)
                {
                    pc.IsAvailable = false;
                    pc.SupplierQuantity = null;
                }
                else
                {
                    pc.SupplierQuantity = Convert.ToInt32(r.Stan.Replace(",0000", ""));
                    pc.IsAvailable = pc.SupplierQuantity > 0;
                }
            }

            pch.SetProductCatalogUpdateZumaline(products, SupplierId);
        }


        public  void ReadMailbox()
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


                    Dal.AllegroScan asc = new Dal.AllegroScan();
                    Dal.AllegroEmail lastEmail = asc.GetAllegroEmailLast();


                    // Messages are numbered in the interval: [1, messageCount]
                    // Ergo: message numbers are 1-based.
                    // Most servers give the latest message the highest number
                    for (int i = messageCount; i > 0; i--)
                    {
                        OpenPop.Mime.Message oMail = client.GetMessage(i);

                        //if (lastEmail != null && lastEmail.SentDate >= oMail.Headers.DateSent.ToLocalTime())
                        //    break;



                        if (oMail.Headers.Subject.Contains("STOCK NORMAL") && oMail.Headers.From.Address.Contains("wojciech.wesoly@zumaline.pl"))
                        {

                            
                            foreach (var attachment in oMail.FindAllAttachments())
                            {
                                string filePath = Path.Combine(@"C:\Attachment", attachment.FileName);
                                if (attachment.FileName.EndsWith(".csv"))
                                {
                                    string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

                                    string fileName = String.Format("PN_LD_{0:yyyyMMddHHmm}.csv", DateTime.Now);
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
            using (TextReader reader = File.OpenText(saveLocation))
            {


                CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.HasHeaderRecord = false;
                // List<ZumalineImport> records = csv.GetRecords<ZumalineImport>().ToList();
                List<PN_LD_Import> records = csv.GetRecords<PN_LD_Import>().ToList();
                ProcessData1(records, 34);
                ProcessData1(records, 82);

            }
        }
        public void ProcessData1(List<PN_LD_Import> obj, int supplierId)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { supplierId })                
                .ToList();

            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = obj.Where(x => x.Ean == pc.Ean).FirstOrDefault();

                    if (r == null)
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = null;
                    }
                    else
                    {
                        pc.SupplierQuantity = null;
                        pc.IsAvailable = r.IsAvailable;
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu PN_LD ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, pc.Code));
                }
            }

            pch.SetProductCatalogUpdateZumaline(products, supplierId);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(supplierId, DateTime.Now);
        }
    }
}
