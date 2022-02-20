using LinqToExcel;
using LinqToExcel.Attributes;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
namespace LajtIt.Bll
{
    public class MaytoniHelper
    {
        const int supplierId = 32;
        public class MaytoniImport
        {
            public string Code { get; set; }
            public string Ean { get; set; }
            public string Description { get; set; }
            public bool IsAvailable { get; set; }
            public int? Quantity { get; set; }
            public bool IstniejeWBazie { get; set; }
        }
        public class MaytoniXlsxFile
        {
        //    [ExcelColumn("Old producer article number")]
        //    public string OldCode { get; set; }// Old producer article number	
        //    [ExcelColumn("New producer article number")]
        //    public string NewCode { get; set; }//New producer article number
        //    [ExcelColumn("Quantity in stock")]
        //    public string Quantity { get; set; }//Quantity in stock	
        //    [ExcelColumn("Expected delivery date")]
        //    public string DeliveryDate { get; set; }//Expected delivery date	
        //    [ExcelColumn("In production. ETA")]
        //    public string ProductionDate { get; set; }//In production. ETA 

        [ExcelColumn("Article Old No")]
        public string OldCode { get; set; }// Old producer article number	
        [ExcelColumn("Article No")]
        public string NewCode { get; set; }//New producer article number
        [ExcelColumn("Description")]
        public string Description { get; set; }//New producer article number
        [ExcelColumn("Avail Inventory")]
        public string Quantity { get; set; }//Quantity in stock	
        [ExcelColumn("Expected Receipt Date")]
        public string DeliveryDate { get; set; }//Expected delivery date	
        [ExcelColumn("In Production Date")]
        public string ProductionDate { get; set; }//In production. ETA 
        [ExcelColumn("EANList")]
        public string Ean { get; set; }//In production. ETA 

    }
        public void GetFile()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier supplier = oh.GetSupplier(supplierId);

            string remoteUri = supplier.ImportUrl;


            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format("{1}_{0:yyyyMMddHHss}.xlsx", DateTime.Now, "Maytoni");

            string saveLocation = String.Format(path, fileName);

            try
            {

                Dal.SettingsHelper sh = new Dal.SettingsHelper();
                Dal.Settings sl = sh.GetSetting("MAYTONI_L");
                Dal.Settings sp = sh.GetSetting("MAYTONI_P");



                // Create a new WebClient instance.
                using (WebClient myWebClient = new WebClient())
                {
                    //myWebClient.Credentials = new NetworkCredential(sl.StringValue, sp.StringValue);
                    myWebClient.DownloadFile(remoteUri, saveLocation);
                }
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", "w"));
                return;

            }
            //using (ExcelPackage excelEngine = new ExcelPackage())
            //{
            //    excelEngine.Workbook.Worksheets.Add("sheet1");
            //    excelEngine.Workbook.Worksheets.Add("sheet2");
            //    excelEngine.Workbook.Worksheets.Add("sheet3");

            //    String myFile = @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\M.xlsx";
            //    excelEngine.SaveAs(new FileInfo(myFile));

            //}


            //string Filepath = @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\M.xlsx";// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\Maytoni_201904112034.xlsx";
            //FileInfo importFileInfo = new FileInfo(Filepath);
            //using (var excelPackage = new ExcelPackage(importFileInfo))
            //{
            //    ExcelWorksheet w =excelPackage.Workbook.Worksheets.Add("dd");

            //    excelPackage.Save();
            //    //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];
            //    //int rowCount = worksheet.Dimension.Start.Row;
            //    //int colCount = worksheet.Dimension.End.Row;
            //}


            //ReadSample(saveLocation);

            ReadNpoiSample(saveLocation);
            return;


            /// ten sposób nie działa. Coś  jest nie tak z plikiem excel
            /// 

            System.Threading.Thread.Sleep(60000);
            ExcelQueryFactory eqf = new ExcelQueryFactory(saveLocation);// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\Maytoni_201810021244.xlsx");// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\LajtitImport.xls");
                                                                        //eqf.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;

             //var c= eqf.GetColumnNames("Sheet1");

          

            var r = from p in eqf.Worksheet<MaytoniXlsxFile>(0) select p;



            var rr = r.ToList();


            var maytoni = rr
                .Where(x => x.NewCode!=null )
                .Select(x => new
            {
                CodeOld = x.OldCode,
                Code = x.NewCode ,
                //Avail = x[2],
                //Delivery = x[3],
                // Production = x[4],
                IsAvailable = GetStatus(x.Quantity),
                    Quantity = GetQuantity(x.Quantity),
                    Ean = x.Ean,
                    Description= x.Description
                }) 
                   .Select(x => new MaytoniImport()
                   {
                       Code = x.Code,
                       IsAvailable = x.IsAvailable,
                       Quantity = x.Quantity,
                       Ean = x.Ean,
                       Description=x.Description
                   })
                   .ToList(); 

            if (maytoni.Count == 0)
            {

                throw new Exception("W otrzymanym pliku aktualizującym nie ma danych lub mapowanie zostało zmienione. Sprawdź ręcznie przyczynę problemu.");

            }
            ProcessData(maytoni);
            
        }

        public void ReadSample(string saveLocation)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Excel.Application();
            if (excelApp != null)
            {
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(saveLocation, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets[1];

                Excel.Range excelRange = excelWorksheet.UsedRange;
                int rowCount = excelRange.Rows.Count;
                int colCount = excelRange.Columns.Count;

                List<MaytoniImport> maytoni = new List<MaytoniImport>();


                for (int i = 2; i <= rowCount; i++)
                {
                    //for (int j = 1; j <= colCount; j++)
                    //{
                    //    try
                    //    {
                    //        Excel.Range range = (excelWorksheet.Cells[i, j] as Excel.Range);
                    //        string cellValue = null;

                    //        if (range == null)

                    //        {
                    //            cellValue = null;
                    //        }
                    //        else
                    //            cellValue = range.Value.ToString();

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        string e = ex.Message;
                    //    }

                    //}

                    MaytoniImport m = new MaytoniImport()
                    {
                        Code = GetValue(excelWorksheet.Cells[i, 1] as Excel.Range),
                        Ean = GetValue(excelWorksheet.Cells[i, 7] as Excel.Range),
                        Description = GetValue(excelWorksheet.Cells[i, 3] as Excel.Range),
                        IsAvailable = GetStatus(GetValue(excelWorksheet.Cells[i, 4] as Excel.Range)),
                        Quantity = GetQuantity(GetValue(excelWorksheet.Cells[i, 4] as Excel.Range))

                    };
                    maytoni.Add(m);

                }

                excelWorkbook.Close();
                excelApp.Quit();


                if (maytoni.Count == 0)
                {

                    throw new Exception("W otrzymanym pliku aktualizującym nie ma danych lub mapowanie zostało zmienione. Sprawdź ręcznie przyczynę problemu.");

                }
                ProcessData(maytoni.Where(x=>x.Code!=null).ToList());
            }
        }
        public void ReadNpoiSample(string saveLocation)
        {
            DataTable dt = ReadExcelFileToDataTable(saveLocation);


            List<MaytoniImport> maytoni = new List<MaytoniImport>();

            foreach (DataRow dr in dt.Rows)
            {

                try
                {
                    string code = GetValue(dr, 0);
                    if (String.IsNullOrEmpty(code))
                        continue;


                MaytoniImport m = new MaytoniImport()
                {
                    Code = code,
                    Ean = GetValue(dr,6),
                    Description = GetValue(dr,2),
                    IsAvailable = GetStatus(GetValue(dr,3)),
                    Quantity = GetQuantity(GetValue(dr,3))

                };
                    if (m.Code != null)
                        maytoni.Add(m);
                }catch (Exception ex)
                {
                    Bll.ErrorHandler.LogError(ex, String.Format("Maytoni {0}", dr[0]));
                }
            }
            ProcessData(maytoni.Where(x => x.Code != null).ToList());
        }
        public DataTable ReadExcelFileToDataTable(string filePath)
        {
            //string filename = @"H:\Demo\UserMaster.xlsx";
            //byte[] bytes= System.IO.File.ReadAllBytes();
            // FileStream excelStream = new FileStream(Server.MapPath(filename), FileMode.Open);
            // FileStream excelStream = new FileStream(filename, FileMode.Open);
            FileStream excelStream = new FileStream(filePath, FileMode.Open);
            var table = new DataTable();
            var book = new XSSFWorkbook(excelStream);
            excelStream.Close();

            var sheet = book.GetSheetAt(0);
            var headerRow = sheet.GetRow(0);//
            var cellCount = headerRow.LastCellNum; //LastCellNum = PhysicalNumberOfCells
            var rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

            //header
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            //body
            for (var i = sheet.FirstRowNum + 1; i <= rowCount; i++)
            {
                var row = sheet.GetRow(i);
                var dataRow = table.NewRow();
                if (row != null && row.FirstCellNum>=0)
                {
                    try
                    { 
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = GetCellValue(row.GetCell(j));
                    }
                    }catch(Exception ex)
                    {
                        throw ex;
                    }
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }

        private string GetCellValue(ICell cell)
        {

            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return cell.ToString(); //This is a trick to get the correct value of the cell.
                                            //NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        var e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

        private string GetValue(DataRow dr, int index)
        {
            try
            {
                if (dr[index] != null && dr[index].ToString() != null)
                    return dr[index].ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string GetValue(Excel.Range range)
        {
            try
            {
                if (range != null && range.Value.ToString() != null)
                    return range.Value.ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /// <summary>
        ///  jest wersja pliku csv ale zniknęła. Trzeba było dostosować do xlsx
        ///// </summary>
        //public void GetCsvFile()
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    Dal.Supplier supplier = oh.GetSupplier(supplierId);

        //    string remoteUri = supplier.ImportUrl;


        //    string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

        //    string fileName = String.Format("{1}_{0:yyyyMMddHHss}.csv", DateTime.Now, "Maytoni");

        //    string saveLocation = String.Format(path, fileName);

        //    try
        //    {

        //        Dal.SettingsHelper sh = new Dal.SettingsHelper();
        //        Dal.Settings sl = sh.GetSetting("MAYTONI_L");
        //        Dal.Settings sp = sh.GetSetting("MAYTONI_P");



        //        // Create a new WebClient instance.
        //        using (WebClient myWebClient = new WebClient())
        //        {
        //            myWebClient.Credentials = new NetworkCredential(sl.StringValue, sp.StringValue);
        //            myWebClient.DownloadFile(remoteUri, saveLocation);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", "w"));
        //        return;

        //    }


        //    List<MaytoniImport> maytoni = File.ReadAllLines(saveLocation)
        //           .Skip(1)
        //           .Select(x => x.Split(';'))
        //           .Select(x => new
        //           {
        //               CodeOld = x[0],
        //               Code = x[1],
        //               Avail = x[2],
        //               Delivery = x[3],
        //               Production = x[4],
        //               IsAvailable = GetStatus(x[2])
        //           })
        //           .Where(x => x.Code.Trim() != "")
        //           .Select(x => new MaytoniImport()
        //           {
        //               Code = x.Code,
        //               IsAvailable = x.IsAvailable
        //           })
        //           .ToList();

        //    if (maytoni.Count == 0)
        //    {

        //        throw new Exception("W otrzymanym pliku aktualizującym nie ma danych lub mapowanie zostało zmienione. Sprawdź ręcznie przyczynę problemu.");

        //    }


        //    ProcessData(maytoni, supplier);
        //}
        private void ProcessData(List<MaytoniImport> ii)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { supplierId });

          
            foreach (Dal.ProductCatalog productFromDb in products)
            {
                MaytoniImport productToCheck = ii.Where(x => x.Ean == productFromDb.Ean).FirstOrDefault();


                if (productToCheck != null)
                {
                    productFromDb.Code = productToCheck.Code;
                    productFromDb.IsAvailable = productToCheck.IsAvailable;
                    productToCheck.IstniejeWBazie = true;
                    //productFromDb.PriceBruttoFixed = Convert.ToDecimal(productToCheck.retailPriceGross);//.Replace(",", "."));
                    productFromDb.SupplierQuantity = productToCheck.Quantity;

                }
                else
                {
                    productFromDb.IsAvailable = false;
                    productFromDb.SupplierQuantity = null;

                }
                productFromDb.UpdateUser = "System";
                productFromDb.UpdateReason = "Aktualizacja automatyczna";

                pch.SetProductCatalogAltavolaUpdate(productFromDb.ProductCatalogId, productFromDb);

                //pch.SetProductCatalogBySupplierAndCode(supplierId, productFromDb.Code, productFromDb.IsAvailable,quantity);
            }

            //pch.SetProductCatalogUpdateStatusPrice(products, supplierId);

            //List<MaytoniImport> productsMaytoniToAdd = ii.Where(x => !String.IsNullOrEmpty(x.Ean) && !x.IstniejeWBazie).ToList();

            //List<Dal.ProductCatalog> productsToAdd = new List<Dal.ProductCatalog>();
            //foreach (MaytoniImport maytoniToAdd in productsMaytoniToAdd)
            //{
            //    Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(supplierId, maytoniToAdd.Code, maytoniToAdd.IsAvailable);
            //    pc.Ean = maytoniToAdd.Ean.Substring(0, Math.Min(maytoniToAdd.Ean.Length, 13)); ;
            //    pc.Name = maytoniToAdd.Description.Substring(0, Math.Min(maytoniToAdd.Description.Length, 100));
            //    pc.AllegroName = maytoniToAdd.Description.Substring(0, Math.Min(maytoniToAdd.Description.Length, 50));
                
            //    productsToAdd.Add(pc);
            //}

            //pch.SetProductCatalogs(productsToAdd, supplierId);
 

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(supplierId, DateTime.Now);

        }
        //private void ProcessData(List<MaytoniImport> maytoni, Dal.Supplier supplier)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

        //    foreach (MaytoniImport mi in maytoni)
        //    {
        //        pch.SetProductCatalogBySupplierAndCode(supplier.SupplierId, mi.Code, mi.IsAvailable, mi.Quantity);
        //    }

        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    oh.SetSupplierImportDate(supplier.SupplierId, DateTime.Now);
        //}

        private bool GetStatus(string desc)
        {
            if (desc == null || desc == "Not available")
                return false;
            else
                return true;
        }
        private int? GetQuantity(string desc)
        {
            if (desc == null || desc == "Not available")
                return null;
            else
            {
                if (desc == "More than 10 pieces")
                    return null;
                else
                    return Convert.ToInt32(desc);

            }
        }
    }
}
