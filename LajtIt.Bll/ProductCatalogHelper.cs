using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public class ProductCatalogHelper
    {
        public string ExportToFile(int[] productCatalogIds, int[] attributeIds)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogByIds(productCatalogIds);

            List<Dal.ProductCatalogAttributeToProduct> attributes = pch.GetProductCatalogAttributeProducts(productCatalogIds, attributeIds);

            StringBuilder sb = new StringBuilder();

            string attributeColumns = String.Join(";", attributes.Select(x => x.ProductCatalogAttribute.Name).Distinct());

            sb.AppendLine(String.Format("id;code;ean;name;externalId;supplier;{0}", attributeColumns));

            string columnTemplate = "0";

            for (int i = 1; i <= 5 + attributes.Select(x => x.ProductCatalogAttribute.Name).Distinct().Count(); i++)
            {

                columnTemplate += ";{" + String.Format("{0}", i) + "}";
            }

            foreach (Dal.ProductCatalog pc in products)
            {

                sb.AppendLine(
                    String.Format(columnTemplate,
                    pc.ProductCatalogId,
                    pc.Code,
                    pc.Ean,
                    Regex.Replace(pc.Name, @"\t|\n|\r", ""),// pc.Name.Replace(Environment.NewLine, ""),
                    pc.ExternalId,
                    pc.Supplier.Name,
                    attributes.Where(x=>x.ProductCatalogId==pc.ProductCatalogId && x.ProductCatalogAttribute.Code == "INFORMACJE").Select(x=>x.StringValue).FirstOrDefault()
                    ));
            }
            Bll.OrderHelper oh = new Bll.OrderHelper();
            string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
            path = String.Format(path, "");

            

            DirectoryInfo di = new DirectoryInfo(path);

            string f = String.Format("ProductCatalog_{0:yyyyMMddHHmmss}.csv", DateTime.Now);
            string fileName = String.Format(@"{0}\{1}", di.FullName, f);
            System.IO.StreamWriter file =
               new System.IO.StreamWriter(fileName);

            file.Write(sb.ToString());

            file.Close();

            return fileName;

        }
        public   void UpdateProductNames(int[] supplierIds)
        { 
            List<Dal.Supplier> suppliers = Dal.DbHelper.ProductCatalog.GetSuppliers(supplierIds);



            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(supplierIds);
            UpdateProductNames(products);
        }
        public void UpdateProductNamesForShop(int shopId, bool createNew)
        {
            
            Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopId);

            List<Dal.SupplierShop> suppliers = Dal.DbHelper.ProductCatalog.GetSuppliersShop(shopId);

            int[] supplierIds = suppliers.Where(x => x.IsActive).Select(x => x.SupplierId).ToArray();


            List<Dal.ProductCatalogShopProductFnResult> products =
                Dal.DbHelper.ProductCatalog.GetProductCatalogShopProduct(shop, supplierIds);

            List<int> shopIds = new List<int>();
            shopIds.Add(shopId);

            foreach (Dal.ProductCatalogShopProductFnResult pc in products)
            {
                Mixer.SetProductNames(pc.ProductCatalogId, shopIds, createNew);
            }

        }
        public void UpdateProductNames(List<Dal.ProductCatalog> products, int[] shopIds)
        {
            Bll.ProductFileImportHelper pci = new Bll.ProductFileImportHelper();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();


       
            foreach (Dal.ProductCatalog pc in products)
            {
                Mixer.SetProductNames(pc.ProductCatalogId, shopIds.ToList());
            }

        }
        public void UpdateProductNames(List<Dal.ProductCatalog> products)
        {
            Bll.ProductFileImportHelper pci = new Bll.ProductFileImportHelper();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();


            List<Dal.Shop> shops = Dal.DbHelper.Shop.GetShops().Where(x => x.IsActive && x.CanExportProducts).ToList();
            List<int> shopIds = new List<int>();
            foreach (Dal.ProductCatalog pc in products)
            {
                Mixer.SetProductNames(pc.ProductCatalogId, shops.Select(x => x.ShopId).ToList());
            }

        }
        public void UpdateProductNames(int attributeId, Dal.Helper.ShopType shopType)
        {
            Bll.ProductFileImportHelper pci = new Bll.ProductFileImportHelper();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Dal.ProductCatalog> products = Dal.DbHelper.ProductCatalog.GetProductCatalogByAttribute(attributeId);

            List<Dal.Shop> shops = Dal.DbHelper.Shop.GetShops().Where(x => x.IsActive && x.CanExportProducts && x.ShopTypeId == (int)shopType).ToList();

            List<int> shopIds = new List<int>();
            foreach (Dal.ProductCatalog pc in products)
            {
                Mixer.SetProductNames(pc.ProductCatalogId, shops.Select(x => x.ShopId).ToList());
            }

        }

        public bool DeliveryProuctsMove(int productCatalogId, int fromWarehouseId, int toWarehouseId, int quantity)
        {
            Dal.ProductCatalogImportHelper pch = new Dal.ProductCatalogImportHelper();
            Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();

            List<Dal.ProductCatalogDelivery> deliveries = pch.GetDeliveries(productCatalogId);
            List<Dal.ProductCatalogDelivery> deliveryToUpdate = new List<Dal.ProductCatalogDelivery>();
            List<Dal.ProductCatalogDelivery> deliveryToAdd = new List<Dal.ProductCatalogDelivery>();
            List<Dal.ProductCatalogWarehouse> warehouses = pc.GetWarehouse();
            int total = quantity;

            foreach(Dal.ProductCatalogDelivery  deliveryFrom in deliveries.Where(x=>x.WarehouseId == fromWarehouseId).ToList())
            {
                if(deliveryFrom.Quantity >= total)
                {

                    if (deliveryFrom.Quantity - total == 0)
                        deliveryFrom.ProductCatalogWarehouse = warehouses.Where(x => x.WarehouseId == toWarehouseId).FirstOrDefault();
                    else
                    {
                        deliveryToAdd.Add(
                            new Dal.ProductCatalogDelivery()
                            {
                                Comment = deliveryFrom.Comment,
                                ImportId = deliveryFrom.ImportId,
                                Price = deliveryFrom.Price,
                                ProductCatalogId = deliveryFrom.ProductCatalogId,
                                Quantity = total,
                                QuantityBlocked = 0,
                                WarehouseId = toWarehouseId,
                                OrderId = deliveryFrom.OrderId
                            }
                            );
                        deliveryFrom.Quantity += -total;
                    }
                    total = 0;
                }
                else //deliveryFrom.Quantity < total
                {
                    deliveryFrom.ProductCatalogWarehouse = warehouses.Where(x => x.WarehouseId == toWarehouseId).FirstOrDefault();
                    total -= deliveryFrom.Quantity;

                }
                deliveryToUpdate.Add(deliveryFrom);

                if (total == 0)
                    break;
            }

            pch.SetDeliveryUpdate(deliveryToAdd, deliveryToUpdate);
            return true;
        }

        public static string SaveFile(int[] productCatalogIds, string saveLocation, string fileName, string orginalFileName)
        {

            System.Drawing.Image img = System.Drawing.Image.FromFile(saveLocation);
            int height = img.Height;
            int width = img.Width;
            FileInfo fi = new FileInfo(saveLocation);
            Bll.ProductCatalogHelper pch = new ProductCatalogHelper();
            string path = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];

            foreach (int productCatalogId in productCatalogIds)
            {
                try
                {
                    Dal.ProductCatalogImage image = new Dal.ProductCatalogImage()
                    {
                        FileName = fileName,
                        Height = height,
                        InsertDate = DateTime.Now,
                        IsActive = true,
                        OriginalFileName = orginalFileName,
                        Priority = 0,
                        ProductCatalogId = productCatalogId,
                        Size = (int)fi.Length, //postedFile.ContentLength,
                        Width = width,
                        Description = "",
                        ImageTypeId = 1
                    };

                    Dal.OrderHelper oh = new Dal.OrderHelper();
                    bool fileExists = oh.GetProductCatalogImageExists(image);
                    if (!fileExists)
                        oh.SetProductCatalogImage(image);

                    Bll.Helper.CreateThumbImage(path, image.FileName);
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError(ex, String.Format("productCatalogId:{0}, oryginalFileName: {1}", productCatalogId, orginalFileName));
                   // return null;
                }
            }
            return saveLocation;

        }

        internal void FollowObjects()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.TableLogFollowResult> log = pch.GetTableLogHistory(Dal.Helper.TableName.ProductCatalog);

            if (log.Count == 0)
                return;


            StringBuilder sb = new StringBuilder();
            sb.Append(@"<style>table, th, td: {border: 1px solid gray; padding: 15px;} td{text-align:center; border: solid 1px black} tr th {background-color: gray; color: white}</style>
<table>
    <tr>
        <th>Nazwa produktu</th>
        <th>Wartość zmiany</th>
        <th>Pole</th>
        <th>Data</th>
    </tr>");
            foreach(Dal.TableLogFollowResult l in log)
            {
                sb.AppendLine(String.Format(@"<tr>
    <td><a href='http://192.168.0.107/Product.aspx?id={0}'>{1}</a></td>
    <td>{2}</td>
    <td>{3}</td>
    <td>{4:yyyy-MM-dd HH:mm}</td>
    </tr>", l.ObjectId, l.ProductName, GetValue(l), l.ColumnName, l.InsertDate));

            }
            sb.Append("</table>");



            Dal.DalHelper dal = new Dal.DalHelper();
            Dal.EmailTemplates emailTemplate = dal.GetEmailTemplate(11);


            Bll.EmailSender emailSender = new EmailSender();

            string body = emailTemplate.Body.Replace("[COMMENT]", sb.ToString());

            Dto.Email email = new Dto.Email()
            {
                Body = body,
                FromEmail = emailTemplate.FromEmail,
                FromName = emailTemplate.FromName,
                Subject = emailTemplate.Subject,
                ToEmail = Dal.Helper.MyEmail,
                ToName = emailTemplate.FromName
            };

            emailSender.SendEmail(email);

            pch.SetTableLogHistorySent(Dal.Helper.TableName.ProductCatalog);
        }

        private object GetValue(TableLogFollowResult l)
        {
            if (l.IntValue.HasValue)
                return l.IntValue.ToString();
            if (l.DecimalValue.HasValue)
                return l.DecimalValue.ToString();
            return l.StringValue;
        }

        public string GetFriendlyFileName(int imageId)
        {

   //         Dal.SettingsHelper sh = new Dal.SettingsHelper();
   //         Dal.Settings s = sh.GetSetting("IMG_TMPL");
   //         Bll.ProductFileImportHelper pfih = new ProductFileImportHelper();
   //         Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
   //         Dal.ProductCatalogImage image = pch.GetProductCatalogImage(imageId);
   //         Dal.ProductCatalog pc = pch.GetProductCatalog(image.ProductCatalogId);
   //         Dal.ProductCatalogAttribute attLampType = pch.GetProductCatalogAttributeGroup(pc.ProductCatalogId, "LAMPTYPE");

   //         string fileNameTemplate = pfih.GetProductName(s.StringValue, pc, attLampType);


   //         string file = String.Format("{0}_{1}", fileNameTemplate, imageId, Path.GetExtension(image.FileName));


   //         file=RemoveAccent(file);
   //         file = file.Replace(" ", "_");
   //         file = file.Replace(@"\", "_");
   //         file = file.Replace("/", "_");
   //         file = Regex.Replace(
   //file,
   //@"[^A-Za-z0-9_.]",  /*Matches any nonword character. Equivalent to '[^A-Za-z0-9_]'*/
   //"",
   //RegexOptions.IgnoreCase);



   //         //foreach (char lDisallowed in System.IO.Path.GetInvalidFileNameChars())
   //         //{
   //         //    file = file.Replace(lDisallowed.ToString(), "");
   //         //}
   //         //foreach (char lDisallowed in System.IO.Path.GetInvalidPathChars())
   //         //{
   //         //    file = file.Replace(lDisallowed.ToString(), "");
   //         //}



   //         //Array.ForEach(Path.GetInvalidFileNameChars(),
   //         //      c => file = file.Replace(c.ToString(), String.Empty));

   //          file = String.Format("{0}{1}", file, Path.GetExtension(image.FileName));

            return "";
        }
        public static string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        internal void CheckMisssingImages()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImages();

            string filepath = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];

            foreach (Dal.ProductCatalogImage image in images)
            {
                string fileName = String.Format(filepath, image.FileName);

                bool exists = File.Exists(fileName);

                pch.SetProductCatalogImageMissing(image.ImageId, exists);

            }
            int notExistsCount= pch.GetProductCatalogImages().Where(x => x.FileExists.HasValue && !x.FileExists.Value).Count();

            if(notExistsCount>0)
            {
                EmailSender es = new EmailSender();
                Dto.Email email = new Dto.Email()
                {
                    Body = String.Format("Liczba zdjęć w systemie bez plików: {0}", notExistsCount),
                    FromEmail = Dal.Helper.MyEmail,
                    FromName = "Lajtit backend",
                    Subject = "Zdjęcia produktów - brak plików zdjęć",
                    ToEmail = Dal.Helper.DevEmail,
                    ToName = Dal.Helper.MyEmail

                };
                es.SendEmail(email);
            }
        }

        internal void CheckFilesNotIdDb()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImages();

            string filepath = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];
            filepath = String.Format(filepath, "");
            string[] filesInDb = images.Select(x => x.FileName).Distinct().ToArray();

            string[] filesOnDisc = Directory.GetFiles(filepath).Select(x => Path.GetFileName(x)).ToArray();


            string[] filesNotInDb = filesOnDisc.Except(filesInDb).ToArray();

            foreach(string fileName in filesNotInDb)
            {
                string fn = String.Format(ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())], fileName);

                File.Delete(fn);

            }

            Bll.ErrorHandler.SendEmail(String.Format("Liczba zdjęć na dysku bez DB {0}, wszystkich zdjęć {1}", filesNotInDb.Count(), filesOnDisc.Count()));
            //foreach (Dal.ProductCatalogImage image in images)
            //{
            //    string fileName = String.Format(filepath, image.FileName);

            //    bool exists = File.Exists(fileName);

            //    pch.SetProductCatalogImageMissing(image.ImageId, exists);

            //}
            //int notExistsCount = pch.GetProductCatalogImages().Where(x => x.FileExists.HasValue && !x.FileExists.Value).Count();

            //if (notExistsCount > 0)
            //{
            //    EmailSender es = new EmailSender();
            //    Dto.Email email = new Dto.Email()
            //    {
            //        Body = String.Format("Liczba zdjęć w systemie bez plików: {0}", notExistsCount),
            //        FromEmail = Dal.Helper.MyEmail,
            //        FromName = "Lajtit backend",
            //        Subject = "Zdjęcia produktów - brak plików zdjęć",
            //        ToEmail = Dal.Helper.DevEmail,
            //        ToName = Dal.Helper.MyEmail

            //    };
            //    es.SendEmail(email);
            //}
        }
        //public void SetProductCatalogScheduleDeleteDuplicates()
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    List<Dal.ProductCatalogUpdateScheduleDuplcates> duplicates = pch.GetProductCatalogUpdateScheduleDuplicates();

        //    foreach (Dal.Helper.UpdateScheduleType scheduleType in Enum.GetValues(typeof(Dal.Helper.UpdateScheduleType)))
        //    { 
        //        foreach (Dal.ProductCatalogUpdateScheduleDuplcates d in duplicates)
        //    {
        //        string[] updateCommands = pch.ProductCatalogUpdateSchedules(d.ShopId, d.ProductCatalogId, scheduleType);
        //        string updateCommand = GetUpdateCommand(updateCommands);

        //            Int64 i = Convert.ToInt64(updateCommand);
        //            Dal.ProductCatalogUpdateSchedule schedule = null;
        //            if (i != 0)
        //                schedule =
        //            new Dal.ProductCatalogUpdateSchedule()
        //            {
        //                InsertDate = DateTime.Now,
        //                InsertUser = "auto merge",
        //                ProductCatalogId = d.ProductCatalogId,
        //                ShopId = d.ShopId,
        //                UpdateCommand = updateCommand,
        //                UpdateStatusId = (int)Dal.Helper.ProductCatalogUpdateStatus.New,
        //                UpdateComment = "połączenie",
        //                ScheduleTypeId = (int)scheduleType
        //            };

        //            pch.SetProductCatalogScheduleAndDeleteDuplicates(duplicates, schedule, d.ShopId, d.ProductCatalogId, (int)scheduleType);
        //    }
        //    }
        //}

    
        private string GetUpdateCommand(string[] updateCommands)
        {
            if (updateCommands.Length == 1)
                return updateCommands[0];

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 15; i++)
            {
                string t = "0";
                foreach (string str in updateCommands)
                {
                    if (i < str.Length)
                        if (str[i] == '1')
                            t = "1";

                }
                sb.Append(t);
            }
            return sb.ToString();
        }
 
    }
}
