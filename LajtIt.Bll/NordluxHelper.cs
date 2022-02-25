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
            public bool IsDiscontinued { get; set; }
            public bool IsAvailable { get; set; }
            public int? SupplierQuantity { get; set; }
        }

        public class NxdiscontinuedXlsxFile
        {
            [ExcelColumn("No#")]
            public string Code { get; set; }
            [ExcelColumn("Description - English")]
            public string Name { get; set; }
            [ExcelColumn("Fase")]
            public string Fase { get; set; }
            [ExcelColumn("EAN")]
            public string Ean { get; set; }
            [ExcelColumn("DB no#")]
            public string DbCode { get; set; }

        }

        public class NordluxNAPXlsxFile
        {
            [ExcelColumn("Item no#")]
            public string Code { get; set; }
            [ExcelColumn("EAN no#")]
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

        public class NordluxPriceXlsxFile
        {
            [ExcelColumn("Model")]
            public string Model { get; set; }
            [ExcelColumn("Type")]
            public string Type { get; set; }
            [ExcelColumn("Material")]
            public string Material { get; set; }
            [ExcelColumn("Color")]
            public string Color { get; set; }
            [ExcelColumn("Number")]
            public string Number { get; set; }
            [ExcelColumn("EAN")]
            public string EAN { get; set; }
            [ExcelColumn("Package")]
            public string Package { get; set; }
            [ExcelColumn("RRP €")]
            public string RRP { get; set; }
            [ExcelColumn("DETAL RRP PLN 10/2021")]
            public Decimal Detal { get; set; }
            [ExcelColumn("RRP PLN NETTO ")]
            public Decimal RRPPLNNETTO { get; set; }
            [ExcelColumn("CENA ZAKUPU PLN NETTO")]
            public Decimal CENAZAKUPUPLNNETTO { get; set; }

        }

        private string[] files = new string[3];
        private List<NordluxImport>[] objects = new List<NordluxImport>[3];

        public NordluxHelper()
        {
            for (int i = 0; i < files.Count(); i++)
            {
                files[i] = "";
                objects[i] = null;
            }
        }

        //private void ProcessPriceListData(string fileName)
        //{
        //    ExcelQueryFactory eqf = new ExcelQueryFactory(fileName);


        //    var r = from p in eqf.WorksheetRange<NordluxXlsxFile>("A6", "K60000", 0) select p;



        //    var rr = r.ToList();

        //    rr = rr.Where(x => x.Number != null).ToList();

        //    if (rr.Count == 0)
        //    {
        //        Bll.ErrorHandler.SendEmail("Plik Nordlux nie zwraca danych. Sprawdź jego strukturę");
        //        return;
        //    }




        //    List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();

        //    foreach (NordluxXlsxFile max in rr)
        //    {
        //        Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, max.Number, true);

        //        pc.SupplierQuantity = null;

        //        if (max.Number != null)
        //            max.Number = max.Number.Trim();

        //        pc.Code = max.Number;

        //        products.Add(pc);
        //    }

        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();



        //    Dal.OrderHelper oh = new Dal.OrderHelper();


        //    pch.SetProductCatalogUpdateNordlux(products);

        //    oh.SetSupplierImportDate(SupplierId, DateTime.Now);


        //}

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
                                    string fileName = "";
                                    int fileKind = -1;
                                    if (files[0].Equals("") && (attachment.FileName.Contains("Nordlux Price")))// || attachment.FileName.Contains("DFTP Price")))
                                    {
                                        fileKind = 0;
                                        fileName = String.Format("Nordlux_Price_{0:yyyyMMddHHmm}.xlsx", DateTime.Now);
                                        files[fileKind] = String.Format(path, fileName);
                                    }
                                    else if (files[1].Equals("") && attachment.FileName.Contains("Nx discontinued"))
                                    {
                                        fileKind = 1;
                                        fileName = String.Format("Nordlux_Discontinued_{0:yyyyMMddHHmm}.xlsx", DateTime.Now);
                                        files[fileKind] = String.Format(path, fileName);
                                    }
                                    else if (files[2].Equals("") && attachment.FileName.Contains("Nordlux NAP"))
                                    {
                                        fileKind = 2;
                                        fileName = String.Format("Nordlux_NAP_{0:yyyyMMddHHmm}.xlsx", DateTime.Now);
                                        files[fileKind] = String.Format(path, fileName);
                                    }

                                    if (fileKind >= 0)
                                    {
                                        FileStream Stream = new FileStream(files[fileKind], FileMode.Create);
                                        BinaryWriter BinaryStream = new BinaryWriter(Stream);
                                        BinaryStream.Write(attachment.Body);
                                        BinaryStream.Close();

                                        if (files[fileKind].Equals("")) break;

                                        switch (fileKind)
                                        {
                                            case 0:
                                                {
                                                    ExcelQueryFactory eqf = new ExcelQueryFactory(files[fileKind]);
                                                    var r = from p in eqf.WorksheetRange<NordluxPriceXlsxFile>("A6", "K60000", 0) select p;
                                                    var rr = r.Select(x => new NordluxImport()
                                                    {
                                                        Ean = x.EAN,
                                                        Code = x.Number,
                                                        IsAvailable = true,
                                                        IsDiscontinued = false,
                                                        SupplierQuantity = null
                                                    }
                                                    ).ToList();
                                                    objects[fileKind] = rr;
                                                }
                                                break;

                                            case 1:
                                                {
                                                    ExcelQueryFactory eqf = new ExcelQueryFactory(files[fileKind]);
                                                    var r = from p in eqf.WorksheetRange<NxdiscontinuedXlsxFile>("A2", "E60000", 0) select p;
                                                    var rr = r.Select(x => new NordluxImport()
                                                    {
                                                        Ean = x.Ean,
                                                        Code = x.Code,
                                                        IsAvailable = false,
                                                        IsDiscontinued = true,
                                                        SupplierQuantity = null
                                                    }
                                                    ).ToList();
                                                    objects[fileKind] = rr;
                                                }
                                                break;

                                            case 2:
                                                {
                                                    ExcelQueryFactory eqf = new ExcelQueryFactory(files[fileKind]);
                                                    var r = from p in eqf.WorksheetRange<NordluxNAPXlsxFile>("A1", "H60000", 0) select p;
                                                    var rr = r.Select(x => new NordluxImport()
                                                    {
                                                        Ean = x.Ean,
                                                        Code = x.Code,
                                                        IsAvailable = false,
                                                        IsDiscontinued = false,
                                                        SupplierQuantity = null
                                                    }
                                                    ).ToList();
                                                    objects[fileKind] = rr;
                                                }
                                                break;
                                        }

                                    }

                                }
                            }

                            if (!files[0].Equals("") && !files[1].Equals("") && !files[2].Equals(""))
                                break;
                        }
                    }

                    ProcessDatas();
                }
                Console.WriteLine("Completed!");
            }
            catch (Exception ep)
            {
                Console.WriteLine(ep.Message);
            }

        }

        private void ProcessDatas()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                .ToList();

            NordluxImport o0 = null;
            NordluxImport o1 = null;
            NordluxImport o2 = null;

            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    pc.SupplierQuantity = null;

                    if (objects[0] != null)
                    {
                        o0 = objects[0].Where(x => x.Ean == pc.Ean).FirstOrDefault();
                        if (o0 == null)
                        {
                            pc.IsDiscontinued = true;
                            if (pc.LeftQuantity > 0)
                                pc.IsAvailable = true;
                            else
                                pc.IsAvailable = false;
                        }
                        else
                        {
                            pc.IsDiscontinued = false;
                            pc.IsAvailable = true;
                        }
                    }

                    if (objects[1] != null)
                    {
                        o1 = objects[1].Where(x => x.Ean == pc.Ean).FirstOrDefault();
                        if (o1 != null)
                        {
                            pc.IsDiscontinued = true;
                            if (pc.LeftQuantity > 0)
                                pc.IsAvailable = true;
                            else
                                pc.IsAvailable = false;
                        }
                    }

                    if (objects[2] != null)
                    {
                        o2 = objects[2].Where(x => x.Ean == pc.Ean).FirstOrDefault();
                        if (o2 != null)
                        {
                            pc.IsAvailable = false;
                        }
                        else
                        {
                            if (pc.IsDiscontinued == false)
                                pc.IsAvailable = true;
                        }
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
