using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using LinqToExcel;
using LinqToExcel.Attributes;
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
    public class PaulmannHelper : ImportData, IImportData
    {
       
        public class PaulmannImport
        { 
            public string Code { get; set; }
            public string Ean { get; set; }
            public string Status { get; set; } 
            public bool IsAvailable
            {
                get
                {

                    if (Status != null)
                        return Status.Trim() != "0";
                    else
                        return false;
                }
            }

        }
         
        public sealed class FooMap1 : ClassMap<PaulmannImport>
        {
            public FooMap1()
            {
                Map(m => m.Code).Index(0);
                Map(m => m.Ean).Index(1);
                Map(m => m.Status).Index(3);//.TypeConverter<Int32Converter<Quantity>>();
            }
        }
  

        public class Int32Converter<T> : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return Int32.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
            } 
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


              

                    // Messages are numbered in the interval: [1, messageCount]
                    // Ergo: message numbers are 1-based.
                    // Most servers give the latest message the highest number
                    for (int i = messageCount; i > 0; i--)
                    {
                        OpenPop.Mime.Message oMail = client.GetMessage(i);

                        //if (lastEmail != null && lastEmail.SentDate >= oMail.Headers.DateSent.ToLocalTime())
                        //    break;



                        if (oMail.Headers.Subject.Contains("Paulmann") && oMail.Headers.From.Address.Contains("edifakt2@paulmann.de"))
                        {

                            
                            foreach (var attachment in oMail.FindAllAttachments())
                            {
                                string filePath = Path.Combine(@"C:\Attachment", attachment.FileName);
                                if (attachment.FileName.EndsWith(".xls"))
                                {
                                    string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

                                    string fileName = String.Format("Paulmann_{0:yyyyMMddHHmm}.xls", DateTime.Now);
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
        public class PaulmannXlsxFile
        {
            [ExcelColumn("Material")]
            public string Code { get; set; }// Old producer article number	
            [ExcelColumn("EAN")]
            public string Ean { get; set; }//New producer article number
            [ExcelColumn("Artikelbezeichnung")]
            public string Name { get; set; }//Quantity in stock	
            [ExcelColumn("Status")]
            public string Status { get; set; }//Expected delivery date	
        }
            private void ProcessFile(string saveLocation)
        {
            //using (TextReader reader = File.OpenText(saveLocation))
            //{


            //    CsvHelper.CsvReader csv = new CsvHelper.CsvReader(reader);
            //    csv.Configuration.MissingFieldFound = null;
            //    csv.Configuration.HeaderValidated = null;
            //    csv.Configuration.Delimiter = ";";
            //    csv.Configuration.HasHeaderRecord = false;
            //    // List<ZumalineImport> records = csv.GetRecords<ZumalineImport>().ToList();
            //    List<PaulmannImport> records = csv.GetRecords<PaulmannImport>().ToList();
            //    ProcessData(records); 

            //}

            ExcelQueryFactory eqf = new ExcelQueryFactory(saveLocation);// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\Maytoni_201810021244.xlsx");// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\LajtitImport.xls");

            var r = from p in eqf.Worksheet<PaulmannXlsxFile>(0) select p;

            var rr = r.Select(x => new PaulmannImport()
            {
                Status = x.Status,
                Ean = x.Ean,
                Code = x.Code
            }
            ).ToList();

            ProcessData(rr);

        }
        public void ProcessData(List<PaulmannImport> obj)
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
                    Bll.ErrorHandler.SendEmail(String.Format("Błąd przetwarzania rekordu Paulmann ProductCatalogId: {0}, Code {1}", pc.ProductCatalogId, pc.Code));
                }
            }

            pch.SetProductCatalogUpdateZumaline(products, SupplierId);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(SupplierId, DateTime.Now);
        }

        public void ProcessData<T>(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
