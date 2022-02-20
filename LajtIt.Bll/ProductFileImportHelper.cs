using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LajtIt.Dal;
using System.IO;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Net;

namespace LajtIt.Bll
{
    public class ProductFileImportHelper
    {
        public void ReadAndSaveExcel(Dal.ProductCatalogFile file, string saveLocation)
        {
            //throw new Exception(saveLocation);
            ExcelQueryFactory eqf = new ExcelQueryFactory(saveLocation);// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\LajtitImport.xls");
            //eqf.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Jet;
            
            //eqf.AddMapping<LajtitProductImport>(x => x.Code, "Code"); 

            var r = from p in eqf.Worksheet<Dal.ProductCatalogFileData>(0) select p;

            List<Dal.ProductCatalogFileData> products = r.ToList();

            foreach (Dal.ProductCatalogFileData data in products)
            {
                data.ProductCatalogFile = file;
                data.FileImportStatusId = (int)Dal.Helper.FileImportStatus.New;
            }

            Dal.ProductFileImportHelper ph = new Dal.ProductFileImportHelper();

            ph.SetFile(products);

        }

        public string GetFileTemplate()
        {

            Dal.ProductFileImportHelper pf = new Dal.ProductFileImportHelper();
            List<Dal.ProductCatalogAttributeGroupDict> dict = pf.GetProductCatalogAttributeGroupDict();
     


            ////*** Preparing excel Application

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            ///*** Opening Excel application

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
            string fileName = String.Format("LajtitImport_Template_{0}.xls", Guid.NewGuid().ToString().Substring(0, 6));
            string saveLocation = String.Format(path, fileName);
            xlWorkBook = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet); ;// xlApp.Workbooks.Open(saveLocation);
            xlWorkSheet = (Excel.Worksheet)(xlWorkBook.ActiveSheet as Excel.Worksheet);


            ////*** It will always remove the prvious result from the CSV file so that we can get always the updated data
            //xlWorkSheet.UsedRange.Select();
            //xlWorkSheet.UsedRange.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
            xlApp.DisplayAlerts = false;
            //xlWorkBook.Save();

            var colNames = dict.Select(x => new { FieldName = x.FieldName, FieldId = x.FieldId }).Distinct().OrderBy(x => x.FieldId).ToArray();

            int j = 0;
            foreach (var column in colNames)
            {
                j++;
                xlWorkSheet.Cells[1, j] = column.FieldName;

            }

            Excel.Worksheet xlWorkSheet2 = xlWorkBook.Worksheets.Add();// System.Reflection.Missing.Value, 2, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            xlWorkSheet2.Move(Missing.Value, xlWorkBook.Sheets[xlWorkBook.Sheets.Count]);

            xlWorkSheet2.Cells[1, 1] = "Grupa atrybutów";
            xlWorkSheet2.Cells[1, 2] = "Atrybut";
            xlWorkSheet2.Cells[1, 3] = "Kod";

            j = 1;
            foreach (Dal.ProductCatalogAttributeGroupDict d in dict.Where(x=>x.Name!=null).OrderBy(x=>x.FieldName).ThenBy(x=>x.Name).ToList())
            {
                j++;
                xlWorkSheet2.Cells[j, 1] = d.FieldName;
                xlWorkSheet2.Cells[j, 2] = d.Name;
                xlWorkSheet2.Cells[j, 3] = d.Code;
            }

            ///**Saving the csv file without notification.
            xlApp.DisplayAlerts = false; 
            xlWorkBook.SaveAs(saveLocation, Excel.XlFileFormat.xlExcel8, System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlShared, false, false, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);

            //xlWorkBook.SaveAs("Book1.csv", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);



            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            return saveLocation;
            ////MessageBox.Show("Excel file created , you can find the file C:\\Users\\MM18100\\Documents\\informations.xls");

        }


        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }

        }
        internal void SetImportProductImages(int supplierId, string dir)
        {
            //ImagesByCodeForSuppliers(new int[] { 5, 6 });
            ImagesByCode(supplierId, dir);


            //ImagesByLine();
        }
        internal void SetImportProductImagesAdHoc(int supplierId, string dir)
        {
            ImagesByCodeAdHoc(supplierId, dir);
        }

        //private void ImagesByLine()
        //{
        //    string dir = @"C:\LajtitData\LajtitPictures\common\italux\import";
        //    string[] extensions = new[] { ".jpg", ".tiff", ".bmp", ".png", ".jpeg" };

        //    FileInfo[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
        //        .Select(x => new FileInfo(x))
        //        .Where(f => extensions.Contains(f.Extension.ToLower()))
        //        //.Where(f=>f.FullName.Contains("E9371-37-LED-BL"))
        //        .ToArray();


        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { 22 });
        //    string s = "";
        //    foreach (ProductCatalog pc in products)
        //    {
        //        if (pc.ProductCatalogGroup == null)
        //            continue;

        //        string group = pc.ProductCatalogGroup.GroupName;

        //        FileInfo[] filesForProduct = files.Where(x => Path.GetFileNameWithoutExtension(x.FullName).Contains(group)).ToArray();

        //        foreach (FileInfo fi in filesForProduct)
        //        {
        //            string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(fi.FullName));
        //            string orginalFileName = System.IO.Path.GetFileName(fi.FullName);
        //            string saveLocation = String.Format(ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())], fileName);

        //            File.Copy(fi.FullName, saveLocation);
        //            Bll.ProductCatalogHelper.SaveFile(pc.ProductCatalogId, saveLocation, fileName, orginalFileName);

        //        }


        //    }
        //}
        private void ImagesByCode(int supplierId, string dir)
        {
            //string dir = @"C:\Users\Jacek\Desktop\MATERIAŁY ON_LINE 26.04.2018\ZDJĘCIA";
            string[] extensions = new[] { ".jpg",   ".bmp", ".png", ".jpeg" };

            FileInfo[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
                .Select(x => new FileInfo(x))
                .Where(f => extensions.Contains(f.Extension.ToLower()))
                //.Where(f=>f.FullName.Contains("E9371-37-LED-BL"))
                .ToArray();


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] {supplierId })
                .Where(x => x.ImageId == null)
                .ToList() ;
            string s = "";
            foreach (ProductCatalog pc in products)
            {
                try

                {
                    string code = pc.Code;
                    string codes = pc.Code.Replace("/", "_");

                    FileInfo[] filesForProduct = files.Where(x => Path.GetFileNameWithoutExtension(x.FullName).Contains(code)
                    || Path.GetFileNameWithoutExtension(x.FullName).Contains(codes)
                    ).ToArray();


                    foreach (FileInfo fi in filesForProduct)
                    {
                        string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(fi.FullName));
                        string orginalFileName = System.IO.Path.GetFileName(fi.FullName);
                        string saveLocation = String.Format(ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())], fileName);

                        File.Copy(fi.FullName, saveLocation);
                        Bll.ProductCatalogHelper.SaveFile(new int[] { pc.ProductCatalogId }, saveLocation, fileName, orginalFileName);
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.LogError(ex, String.Format("PId: {0}", pc.ProductCatalogId));

                }
            }
        }

        private void ImagesByCodeAdHoc(int supplierId, string dir)
        {
            //string dir = @"C:\Users\Jacek\Desktop\MATERIAŁY ON_LINE 26.04.2018\ZDJĘCIA";
            string[] extensions = new[] { ".jpg", ".bmp", ".png", ".jpeg" };

            FileInfo[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
                .Select(x => new FileInfo(x))
                .Where(f => extensions.Contains(f.Extension.ToLower()))
                //.Where(f=>f.FullName.Contains("E9371-37-LED-BL"))
                .ToArray();


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { supplierId })
                //.Where(x => x.ImageId == null)
                .ToList();
     
            foreach (ProductCatalog pc in products)
            {
                try

                {
                    string code = pc.Code;
                    string codes = pc.Code.Replace("/", "_");
                    //codes = codes.Replace("_", "/");

                    FileInfo[] filesForProduct = files.Where(x => Path.GetFileNameWithoutExtension(x.FullName).Contains(code)
                    || Path.GetFileNameWithoutExtension(x.FullName).Contains(codes)
                    ).ToArray();

                    foreach (FileInfo fi in filesForProduct)
                    {
                        string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(fi.FullName));
                        string orginalFileName = System.IO.Path.GetFileName(fi.FullName);
                        string saveLocation = String.Format(ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())], fileName);

                        File.Copy(fi.FullName, saveLocation);
                        Bll.ProductCatalogHelper.SaveFile(new int[] { pc.ProductCatalogId }, saveLocation, fileName, orginalFileName);
                    }
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.LogError(ex, String.Format("PId: {0}", pc.ProductCatalogId));

                }
            }
        }

        public void ImagesByCodeByCatalog(int supplierId, string dir)
        {
            //string dir = @"C:\Users\Jacek\Desktop\MATERIAŁY ON_LINE 26.04.2018\ZDJĘCIA";
            string[] extensions = new[] { ".jpg",".jpeg" };

       
            string[] dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories);


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { supplierId })
                .Where(x => x.ImageId == null)
                .ToList();
          
            foreach (ProductCatalog pc in products)
            {
                string code = pc.Code;
                string codes = pc.Code.Replace("/", "_");
                string dirFound = dirs.Where(x => x.Contains(pc.Code)).FirstOrDefault();

                if (dirFound == null)
                    continue;

                FileInfo[] files = Directory.GetFiles(dirFound, "*.*", SearchOption.AllDirectories)
                   .Select(x => new FileInfo(x))
                   .Where(f => extensions.Contains(f.Extension.ToLower()))
                   .ToArray();

                //FileInfo[] filesForProduct = files.Where(x => Path.GetFileNameWithoutExtension(x.FullName).Contains(code)
                //|| Path.GetFileNameWithoutExtension(x.FullName).Contains(codes)
                //).ToArray();


                foreach (FileInfo fi in files)
                {
                    try
                    { 
                    string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(fi.FullName));
                    string orginalFileName = System.IO.Path.GetFileName(fi.FullName);
                    string saveLocation = String.Format(ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())], fileName);

                    File.Copy(fi.FullName, saveLocation);
                    Bll.ProductCatalogHelper.SaveFile(new int[] { pc.ProductCatalogId }, saveLocation, fileName, orginalFileName);
                    }
                    catch (Exception ex)
                    {

                        ErrorHandler.LogError(ex, String.Format("productCatalogId:{0}, oryginalFileName: {1}", pc.ProductCatalogId, fi.FullName));
                    }
                }
            }
        }

        public void ImagesByCodeByCatalogAdd(int supplierId, string dir)
        {
            //string dir = @"C:\Users\Jacek\Desktop\MATERIAŁY ON_LINE 26.04.2018\ZDJĘCIA";
            string[] extensions = new[] { ".jpg", ".jpeg" };


            string[] dirs = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories);


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { supplierId })
                //.Where(x => x.ImageId == null)
                .ToList();

            foreach (ProductCatalog pc in products)
            {
                string code = pc.Code;
                if (code.Equals(""))
                    continue;

                string dirFound = null;
                foreach (string s in dirs)
                {
                    string[] ls = s.Split('\\');
                    string[] rs = ls.Last().Split(' ');
                    if (rs.Where(x => x.Equals(pc.Code)).FirstOrDefault() != null)
                        dirFound = s;
                }

                if (dirFound == null)
                    continue;

                FileInfo[] files = Directory.GetFiles(dirFound, "*.*", SearchOption.AllDirectories)
                   .Select(x => new FileInfo(x))
                   .Where(f => extensions.Contains(f.Extension.ToLower()))
                   .ToArray();

                //FileInfo[] filesForProduct = files.Where(x => Path.GetFileNameWithoutExtension(x.FullName).Contains(code)
                //|| Path.GetFileNameWithoutExtension(x.FullName).Contains(codes)
                //).ToArray();


                foreach (FileInfo fi in files)
                {
                    try
                    {
                        string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(fi.FullName));
                        string orginalFileName = System.IO.Path.GetFileName(fi.FullName);
                        string saveLocation = String.Format(ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())], fileName);

                        File.Copy(fi.FullName, saveLocation);
                        Bll.ProductCatalogHelper.SaveFile(new int[] { pc.ProductCatalogId }, saveLocation, fileName, orginalFileName);
                    }
                    catch (Exception ex)
                    {

                        ErrorHandler.LogError(ex, String.Format("productCatalogId:{0}, oryginalFileName: {1}", pc.ProductCatalogId, fi.FullName));
                    }
                }
            }
        }

        private void ImagesByCodeForSuppliers(int[] supplierId)
        {
            //string dir = @"C:\LajtitData\LajtitBatch\Zumaline";
            string dir = @"\\serwer\common\Britop\zdjecia";
            string[] extensions = new[] { ".jpg", ".tiff", ".bmp", ".png", ".jpeg" };

            FileInfo[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
                .Select(x => new FileInfo(x))
                .Where(f => extensions.Contains(f.Extension.ToLower()))
                //.Where(f=>f.FullName.Contains("E9371-37-LED-BL"))
                .ToArray();


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(supplierId).Where(x => x.ImageId == null).ToList();
            string s = "";
            foreach (ProductCatalog pc in products)
            {
                string code = pc.Code;

                FileInfo[] filesForProduct = files.Where(x => CleanIt(Path.GetFileNameWithoutExtension(x.FullName)) == code).ToArray();

                foreach (FileInfo fi in filesForProduct)
                {
                    string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(fi.FullName));
                    string orginalFileName = System.IO.Path.GetFileName(fi.FullName);
                    string saveLocation = String.Format(ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())], fileName);

                    File.Copy(fi.FullName, saveLocation);
                    Bll.ProductCatalogHelper.SaveFile(new int[] { pc.ProductCatalogId }, saveLocation, fileName, orginalFileName);

                }


            }
        }
        string CleanIt(string code)
        {
            System.Text.RegularExpressions.Regex digitsOnly = new System.Text.RegularExpressions.Regex(@"[^\d]");
            return digitsOnly.Replace(code, "");
        }
        internal void SetImportProducts()
        {
            Dal.ProductFileImportHelper ph = new Dal.ProductFileImportHelper();

            Dal.ProductCatalogFile file = ph.GetFileToImport(Dal.Helper.FileImportStatus.ReadyToImport);

            if (file == null)
                return;

            ProcessFile(file);

        }

        internal void SetImportProductsCheck()
        {
            Dal.ProductFileImportHelper ph = new Dal.ProductFileImportHelper();

            Dal.ProductCatalogFile file = ph.GetFileToImport(Dal.Helper.FileImportStatus.Processing);

            if (file == null)
                return;

            ProcessFileCheck(file);
        }
        private void ProcessFileCheck(ProductCatalogFile file)
        {
            Dal.ProductFileImportHelper ph = new Dal.ProductFileImportHelper();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.ProductCatalogFileDataFnResult> products = ph.GetProductFileData(file.ProductCatalogFileId);
            Supplier supplier = oh.GetSupplier(file.SupplierId);
            List<Dal.ProductCatalogFileSpecification> specification = ph.GetFileSpecification(1);
            List<Dal.ProductCatalogAttribute> attributes = pch.GetProductCatalogAttributes();
            bool hasErrors = false;


            List<Dal.ProductCatalogFileDataFnResult> duplicates = new List<ProductCatalogFileDataFnResult>();
            if (file.CheckDuplicates)
            {
                switch (file.JoinByColumn)
                {
                    case "Ean":
                        duplicates = products.Duplicates(x => x.Ean).ToList(); break;
                    case "IdZewnetrzne":
                        duplicates = products.Duplicates(x => x.IdZewnetrzne).ToList(); break;
                    default:
                        duplicates = products.Duplicates(x => x.Kod).ToList(); break;
                }
            }


            foreach (Dal.ProductCatalogFileDataFnResult product in products)
            {
                List<FileImportValidator> errors = CheckImportProduct(specification, attributes, product, duplicates, file);

                ph.SetFileValidationResult(product.FileDataId, errors);


                if (hasErrors == false && errors.Where(x=>x.ValidationResult ==false ).Count() > 0)
                    hasErrors = true;
            }

            file.FileImportStatusId = hasErrors == true ? (int)Dal.Helper.FileImportStatus.Error : (int)Dal.Helper.FileImportStatus.Ok;

            ph.SetFileUpdateStatus(file);


        }

        private List<FileImportValidator> CheckImportProduct(List<Dal.ProductCatalogFileSpecification> specification, List<Dal.ProductCatalogAttribute> attributes,
            ProductCatalogFileDataFnResult product, List<Dal.ProductCatalogFileDataFnResult> duplicates,
            ProductCatalogFile file)
        {
            List<FileImportValidator> errors = new List<FileImportValidator>();

            foreach (ProductCatalogFileDataFnResult dup in duplicates.Where(x=>x.FileDataId == product.FileDataId).ToList())
            {
                FileImportValidator error = new FileImportValidator();

                error.FileDataId = product.FileDataId;
                error.FieldName = file.JoinByColumn;

                error.AddError(String.Format("Pole {0} nie jest unikalne w przesłanym zbiorze", file.JoinByColumn));
                errors.Add(error);
            }

            foreach (Dal.ProductCatalogFileSpecification spec in specification)
            {
                FileImportValidator error = new FileImportValidator();
                try
                {
                    error.FileDataId = product.FileDataId;
                    error.FieldName = spec.FieldName;

                    object o = product.GetType().GetProperties().Single(pi => pi.Name == spec.FieldName).GetValue(product, null);

                    if (spec.IsRequired && (o == null || o.ToString() == ""))
                    {
                        error.AddError("Pole wymagane");
                        errors.Add(error);
                        continue;
                    }
                    if ((o == null || o.ToString() == ""))
                    {
                        errors.Add(error);
                        continue;
                    }
                    switch (spec.FieldTypeId)
                    {
                        case 1: // string
                            break;
                        case 2: // decimal
                            decimal decimalValue = 0;
                            if (!Decimal.TryParse(o.ToString(), out decimalValue))
                                error.AddError("Wymagana wartość liczbowa");

                            break;
                        case 3: // atrybuty słownik
                            Dal.ProductCatalogAttribute attribute = attributes.Where(x => x.Code == o.ToString().ToUpper() && x.AttributeGroupId == spec.AttributeGroupId).FirstOrDefault();
                            if(attribute==null)
                                error.AddError("Wartość nie jest zdefiniowana w słowniku");
                            break;
                    }
                    errors.Add(error);
                }
                catch (Exception ex)
                {
                    error.AddError(ex.Message);


            }
            }
            return errors;

        }
        private void ImportProductUpdate(ProductCatalogFile file, ProductCatalogFileDataFnResult product, List<Dal.ProductCatalogFileSpecification> specs, ProductCatalog pc)
        {
            string[] fields = file.ImportUpdateFields.Split(new char[] { ',' });

            Dal.ProductFileImportHelper ph = new Dal.ProductFileImportHelper();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogAttribute> attributes = pch.GetProductCatalogAttributes();

            List<Dal.ProductCatalogAttributeToProduct> attributesToAdd = new List<ProductCatalogAttributeToProduct>();

            try
            {
                attributesToAdd.AddRange(CreateAttributes(product, file, product.ProductCatalogId.Value, fields, specs));

                int[] attributeGroupIds = pch.GetProductCatalogAttributeGroups(attributesToAdd.Select(x => x.AttributeId).Distinct().ToArray());

                pch.SetProductCatalogAttributes(product.ProductCatalogId.Value, attributesToAdd, attributeGroupIds);//, false, null);
                pch.SetProductCatalogUpdate(product, pc, fields, file.SupplierId);
            }
            catch (Exception ex)
            {
                throw ex;
            }  
        }
          
    

        public static bool UpdateAllegro(int productCatalogId)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
 
            /*
             *    All = 0,//Wszystko	
            Delivery = 1,//Cennik dostawy	
            Quantity = 2,//Ilość	
            Price = 3,//Cena	
            Name = 4,//Tytuł 
            Description = 5,//Opis	
            Images = 6,//Zdjęcia
            Attributes = 7,//Parametry	
            Status = 8,//Status
            Category = 9,//Kategoria
            Ean = 10,//Ean
            PricePromo = 11,//Cena	
            Related = 12,//Produkty powiązane
            */
            
            //if(!Bll.AllegroRESTHelper.CreateImages(productCatalogId))
            //{
            //    return false;
            //}
            if (!Bll.AllegroRESTHelper.UpdateOffer(productCatalogId))
                return false;

            return true;
           // UpdateAllegro(schedule, itemsCreating);

        }
        //public static ShopHelper.UpdateResult UpdateAllegro(ProductCatalogUpdateScheduleView schedule, 
        //    List<ProductCatalogAllegroItemsFnResult> itemsCreating)
        //{

        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

        //    ShopHelper.UpdateResult result = new ShopHelper.UpdateResult();
        //    result.ProductCatalogId = schedule.ProductCatalogId;

        //    int actionTypeId = 1; //usun

        //    bool isStatusAction = ShopHelper.CanUpdateField(false, schedule.UpdateCommand, Dal.Helper.ProductUpdateFlag.Status);

        //    Dal.ProductCatalogView pc = pch.GetProductCatalogView(schedule.ProductCatalogId);

        //    if (isStatusAction || pc.IsActiveAllegro == false)
        //    {
        //        if (pc.IsActiveAllegro == false)
        //        {

        //            Dal.OrderHelper oh = new Dal.OrderHelper();
        //            oh.SetAllegroActions(new int[] { pc.ProductCatalogId }, actionTypeId, true);
        //            result.Result = 1;

        //            return result;
        //        }
        //    }

        //    AllegroHelper ah = new AllegroHelper();
        //    List<Dal.ProductCatalogAllegroItemsActive> items = pch.GetAllegroItemsToUpdate(schedule.ProductCatalogId);

 

        //    if (items.Count > 0)
        //    {
        //        foreach (Dal.ProductCatalogAllegroItemsActive item in items)
        //        {
                     
        //             ah.UpdateAllegroItem(item, schedule.UpdateCommand);
        //        }
        //    }
        //    else
        //        ah.CreateNewAllegroAuction(schedule.ProductCatalogId, pc.AllegroUserIdAccount.Value, itemsCreating);

        //    result.Result = 1;

        //    return result;

        //}

        private void ProcessFile(ProductCatalogFile file)
        {
            Dal.ProductFileImportHelper ph = new Dal.ProductFileImportHelper();
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.ProductCatalogFileDataFnResult> products = ph.GetProductFileData(file.ProductCatalogFileId)
                .Where(x => x.FileImportStatusId == (int)Dal.Helper.FileImportStatus.Ok).ToList();

            Supplier supplier = oh.GetSupplier(file.SupplierId);

            foreach (Dal.ProductCatalogFileDataFnResult product in products)
                ImportProduct(file, product, supplier);

            #region Images
            products = ph.GetProductFileData(file.ProductCatalogFileId)
                .Where(x => x.ProductCatalogId.HasValue).ToList();

            

            foreach (Dal.ProductCatalogFileDataFnResult product in products)
                ImportImages(file, product, supplier);
            #endregion

            file.FileImportStatusId = (int)Dal.Helper.FileImportStatus.Imported;
            ph.SetFileUpdateStatus(file);

        }
        private bool ImportImages(ProductCatalogFile file, ProductCatalogFileDataFnResult product, Supplier supplier)
        {
            

           
            try
            {
                if (!String.IsNullOrEmpty(product.Zdjecie1) && (file.ImportUpdateFields.Contains("Zdjecie1") ))
                    AltavolaHelper.DownloadImage(product.Zdjecie1, product.ProductCatalogId.Value);          
                if (!String.IsNullOrEmpty(product.Zdjecie2) && (file.ImportUpdateFields.Contains("Zdjecie2") ))
                    AltavolaHelper.DownloadImage(product.Zdjecie2, product.ProductCatalogId.Value);          
                if (!String.IsNullOrEmpty(product.Zdjecie3) && (file.ImportUpdateFields.Contains("Zdjecie3") ))
                    AltavolaHelper.DownloadImage(product.Zdjecie3, product.ProductCatalogId.Value);          
                if (!String.IsNullOrEmpty(product.Zdjecie4) && (file.ImportUpdateFields.Contains("Zdjecie4") ))
                    AltavolaHelper.DownloadImage(product.Zdjecie4, product.ProductCatalogId.Value);          
                if (!String.IsNullOrEmpty(product.Zdjecie5) && (file.ImportUpdateFields.Contains("Zdjecie5") ))
                    AltavolaHelper.DownloadImage(product.Zdjecie5, product.ProductCatalogId.Value);
                return true;
            }
            catch (Exception ex)
            {
                

                ErrorHandler.SendError(ex, String.Format("Kod: {0}, Ean: {1}, IdZew: {2}", product.Kod, product.Ean, product.IdZewnetrzne));
                return false;
            }
        }

        private bool ImportProduct(ProductCatalogFile file, ProductCatalogFileDataFnResult product, Supplier supplier)
        {
            if (!file.ImportActionTypeId.HasValue)
                return true;
             

            Dal.ProductFileImportHelper ph = new Dal.ProductFileImportHelper();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ProductCatalog pc = null;

            List<Dal.ProductCatalogAttributeToProduct> attributesToAdd = new List<ProductCatalogAttributeToProduct>();


            try
            {


                bool addNew = (file.ImportActionTypeId == 1 || file.ImportActionTypeId == 2) && !product.ProductCatalogId.HasValue;
                bool update = (file.ImportActionTypeId == 1 || file.ImportActionTypeId == 3) && product.ProductCatalogId.HasValue;

                if (!addNew && !update)
                    return true;

                List<Dal.ProductCatalogFileSpecification> specs = ph.GetFileSpecification(1);
                pc = ImportProductNew(file, product, supplier);
                string comment = "";
                Dal.Helper.FileImportStatus status = Dal.Helper.FileImportStatus.Error;
                if (addNew)
                {

                    int productCatalogId = pch.SetProductCatalogImport(pc);

                    SetProductCatalogGroupProduct(product.Linia, productCatalogId, supplier.SupplierId); 

                    if (productCatalogId > 0)
                    {
                        comment = String.Format("Produkt został zaimportowany {0}", productCatalogId);
                        status = Dal.Helper.FileImportStatus.Ok;
                       
                        attributesToAdd.AddRange(CreateAttributes(product, file, productCatalogId, null, specs));
                        int[] attributeGroupIds = pch.GetProductCatalogAttributeGroups(attributesToAdd.Select(x => x.AttributeId).Distinct().ToArray());
                        pch.SetProductCatalogAttributes(productCatalogId, attributesToAdd, attributeGroupIds);//, false, null);
                    }
                    else

                    {
                        comment = String.Format("Produkt nie został zaimportowany {0}", product.Kod);
                        status = Dal.Helper.FileImportStatus.Error;
                    }
                }
                if (update)
                {
                    comment = String.Format("Produkt został zaktualizowany {0}", product.ProductCatalogId.Value);
                    status = Dal.Helper.FileImportStatus.Imported;
                    ImportProductUpdate(file, product, specs, pc);
                }


                ph.SetFileDataUpdate(product.FileDataId, status, comment);
                return true;
            }
            catch (Exception ex)
            {
                List<FileImportValidator> errors = new List<FileImportValidator>();
                string fieldName = "";
                if (ex.Message.Contains("UIDX_ProductCatalog_Ean"))
                    fieldName = "Ean";

                FileImportValidator val = new FileImportValidator()
                {
                    FileDataId = product.FileDataId,
                    FieldName= fieldName
                };
                if(ex.Message!=null)
                val.AddError(ex.Message.Substring(0,Math.Min(100, ex.Message.Length)));

                errors.Add(
                    val
                    );
                ph.SetFileValidationResult(product.FileDataId, errors);


                ErrorHandler.SendError(ex, String.Format("Kod: {0}, Ean: {1}, IdZew: {2}", product.Kod, product.Ean, product.IdZewnetrzne));
                return false;
            }            
        }

        private IEnumerable<ProductCatalogAttributeToProduct> CreateAttributes(ProductCatalogFileDataFnResult product, 
            ProductCatalogFile file, 
            int productCatalogId,
            string[] fieldsToProcess,
            List<Dal.ProductCatalogFileSpecification> specs)
        {
            List<Dal.ProductCatalogAttributeToProduct> attributesToAdd = new List<ProductCatalogAttributeToProduct>();

            Dal.ProductFileImportHelper ph = new Dal.ProductFileImportHelper();
           // List<Dal.ProductCatalogFileSpecification> specs = ph.GetFileSpecification(file.ProductCatalogFileId);

            if (fieldsToProcess != null)
                specs = specs.Where(x => fieldsToProcess.Contains(x.FieldName)).ToList();

            foreach(Dal.ProductCatalogFileSpecification spec in specs.Where(x=>x.AttributeGroupId.HasValue).ToList())
            {
                attributesToAdd.Add(GetAttribute(spec.ProductCatalogAttributeGroup.GroupCode, spec.FieldName, product, productCatalogId));
            }
            foreach (Dal.ProductCatalogFileSpecification spec in specs.Where(x => x.AttributeId.HasValue).ToList())
            {
                try
                { 
                switch(spec.FieldTypeId)
                {
                    case 1: //string

                        attributesToAdd.Add(GetAttributeStringValue(spec.ProductCatalogAttribute.Code, spec.FieldName, product, productCatalogId));
                        break;
                    case 2: //decimal

                        attributesToAdd.Add(GetAttributeDecimalValue(spec.ProductCatalogAttribute.Code, spec.FieldName, product, productCatalogId));
                        break;
                }
                }catch(Exception ex)
                {

                    throw ex;
                }
            }
            return attributesToAdd.Where(x=>x!=null).ToList();
        }
        private ProductCatalogAttributeToProduct GetAttributeStringValue(string code, string field, ProductCatalogFileDataFnResult product, int productCatalogId)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();


            Dal.ProductCatalogAttribute att = pch.GetProductCatalogAttribute(code);

            if (att == null)
                return null;

            object o = product.GetType().GetProperties().Single(pi => pi.Name == field).GetValue(product, null);
            if (o == null)
                return null;
             
                return
                    new ProductCatalogAttributeToProduct()
                    {
                        AttributeId = att.AttributeId,
                        //IsDefault = true,
                        ProductCatalogId = productCatalogId,
                        StringValue = o.ToString()
                    };
             
        }
        private ProductCatalogAttributeToProduct GetAttributeDecimalValue(string code, string field, ProductCatalogFileDataFnResult product, int productCatalogId)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            

            Dal.ProductCatalogAttribute att = pch.GetProductCatalogAttribute(code);

            if (att == null)
                return null;

            object o = product.GetType().GetProperties().Single(pi => pi.Name == field).GetValue(product, null);
            if (o == null)
                return null;

            decimal decimalValue = 0;
            if (att != null && Decimal.TryParse(o.ToString(), out decimalValue))
                return
                    new ProductCatalogAttributeToProduct()
                    {
                        AttributeId = att.AttributeId,
                        //IsDefault = true,
                        ProductCatalogId = productCatalogId,
                        DecimalValue = decimalValue
                    };

            return null;
        }
        private ProductCatalogAttributeToProduct GetAttribute(string groupCode, string field, ProductCatalogFileDataFnResult product, int productCatalogId)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ProductCatalogAttributeGroup group = pch.GetProductCatalogAttributeGroup(groupCode);
            if (group == null)
                return null;

            List<Dal.ProductCatalogAttribute> attributes = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributes(group.AttributeGroupId);

             
            object o = product.GetType().GetProperties().Single(pi => pi.Name == field).GetValue(product, null);
            if (o == null)
                return null;

            ProductCatalogAttribute att = attributes.Where(x => x.Code == o.ToString().ToUpper()).FirstOrDefault();

            if (att != null)
                return
                    new ProductCatalogAttributeToProduct()
                    {
                        AttributeId = att.AttributeId,
                        IsDefault = true,
                        ProductCatalogId = productCatalogId
                    };

            return null;
        }


       
        private Dal.ProductCatalog ImportProductNew(ProductCatalogFile file, ProductCatalogFileDataFnResult product, Supplier supplier)
        {
            string allegroName = String.Format("{0} {1} {2}", supplier.Name, product.Kod, product.Linia);

             

            Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(supplier.SupplierId, product.Kod, product.Status == "1");

            pc.Name = allegroName;
            pc.SupplierId = file.SupplierId;
            pc.Code = product.Kod;
            pc.Specification = product.Opis;
            pc.PurchasePrice = GetPurchasePrice(product, supplier);
           
            pc.ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct;
            pc.AutoAssignProduct = true;
            pc.Ean = String.IsNullOrEmpty(product.Ean) ? null : product.Ean; 
            pc.PriceBruttoFixed = GetPriceBrutto(product.CenaSprzedazyBrutto, supplier) ?? 0;
            pc.PriceBruttoPromo = GetPriceBrutto(product.CenaPromocyjnaSprzedazyBrutto, supplier);
            pc.PriceBruttoPromoDate = GetPromoFinishDate(product.PromocjaKoniecData);
            pc.IsDiscontinued = product.Status == "-1";
            pc.ExternalId = product.IdZewnetrzne;
          
            if (!String.IsNullOrEmpty(product.Status))
                pc.IsAvailable = product.Status.Trim() == "1";
            return pc;
        }

        private DateTime? GetPromoFinishDate(string s)
        {
            DateTime date;
            if (DateTime.TryParse(s, out date))
                return date;
            else
                return null;
        }

        private decimal? GetPriceBrutto(string price, Supplier supplier)
        {
            decimal p;
            if (Decimal.TryParse(price, out p))
            {
                p = Dal.Helper.RoundValue((Dal.Helper.SupplierRoundPriceType)supplier.RoundPriceTypeId, p);
                return p;
            }
            else
                return null;
        }

        private string GetShortDescription(ProductCatalogFileDataFnResult product)
        {
            return "";// throw new NotImplementedException();
        }
         

        private decimal? GetPurchasePrice(ProductCatalogFileDataFnResult product, Supplier supplier)
        {
            decimal? purchasePrice = null; // ProductCatalogCalculator1.NettoValue(Convert.ToDecimal(product.CenaSprzedazyBrutto));
            if(product.CenaZakupuNetto!=null)
                 purchasePrice = Convert.ToDecimal(product.CenaZakupuNetto);
          
            return purchasePrice;
        }
 

        public static int SetProductCatalogGroupProduct(string lineName, int productCatalogId, int supplierId)
        {
            Dal.ProductCatalogGroupHelper ph = new Dal.ProductCatalogGroupHelper();
            return ph.SetProductCatalogGroupProduct(lineName, productCatalogId, supplierId);
        }
    }
}
