using LajtIt.Dal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class AllegroRESTHelper
    {
   
        public class ImageObject
        {
            public DateTime expiresAt { get; set; }
            public string location { get; set; }
        }

        public class ImageUrl
        {
 
                public string url { get; set; } 
            
        }
        public static bool SendImage(ProductCatalogImage pci, string fileName, long itemId, int imageTypeId, out string msg)
        {

            DateTime dtStart = DateTime.Now;

            string filePath = GetImageFile(fileName);
            try
            {
                Dal.ProductCatalogImageAllegroItem aui = null;

                if (pci.UploadedToServer == false  || pci.Size >= 2000000 || pci.Width > 2500 || pci.Height > 2500)
                {


                    if (!File.Exists(filePath))
                    {
                        msg = String.Format("   Plik {0} nie istnieje", fileName);
                        return false;
                    }

                    Console.WriteLine(String.Format("   ItemId: {0} SendImageBinary", itemId));
                    aui = SendImageBinary(pci, filePath, itemId, imageTypeId);
                }
                else
                {
                    Console.WriteLine(String.Format("   ItemId: {0} SendImageLink", itemId));
                    aui = SendImageLink(pci, fileName, itemId, imageTypeId);


                    if (aui == null)
                    {
                        Console.WriteLine(String.Format("   ItemId: {0} SendImageBinary 2", itemId));
                        aui = SendImageBinary(pci, filePath, itemId, imageTypeId);
                    }

                }

                Dal.AllegroScan asc = new Dal.AllegroScan();
                asc.SetProductCatalogImageAllegroItem(aui);

                msg = "";

                TimeSpan ts = DateTime.Now - dtStart;
                Console.WriteLine(String.Format("   Czas: {0} sek", ts.TotalSeconds));
                return true;

            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse == null)
                    {
                        Bll.ErrorHandler.SendError(ex, ex.Message);
                        msg = ex.Message;
                        return false;

                    }
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();

                        Console.WriteLine("ItemId: {0}, error: {1}", itemId, text);

                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd przesyłania zdjęcia ItemId: {0}, fileName: {1}, {2}", itemId, fileName, text));
                        msg = text;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                Bll.ErrorHandler.SendError(ex, String.Format("Błąd przesyłania zdjęcia ItemId: {0}, fileName: {1}", itemId, fileName));

                msg = ex.Message;

                return false;

            }
        }

        private static Dal.ProductCatalogImageAllegroItem SendImageLink(ProductCatalogImage pci, string fileName, long itemId, int imageTypeId)
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            Dal.AllegroItemUserView ai = asc.GetAllegroItemUser(itemId);
            string token = ai.Token;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://upload.allegro.pl/sale/images");
            request.Accept = "application/vnd.allegro.public.v1+json";
            request.ContentType = "application/vnd.allegro.public.v1+json";
            request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");

            request.Method = "POST";

            ImageUrl img = new ImageUrl()
            {
                url = String.Format("http://static.lajtit.pl/ProductCatalog/{0}", fileName)
            };

            Stream dataStream = request.GetRequestStream();
            string jsonEncodedParams = Bll.RESTHelper.ToJson(img);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


            byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            { 
            WebResponse webResponse = request.GetResponse();
            Stream responseStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string text = reader.ReadToEnd();

            var json_serializer = new JavaScriptSerializer();
            ImageObject imageObj = json_serializer.Deserialize<ImageObject>(text);


            Dal.ProductCatalogImageAllegroItem iu = new Dal.ProductCatalogImageAllegroItem()
            {
                ExpireDate = imageObj.expiresAt,
                InsertDate = DateTime.Now,
                LocationUrl = imageObj.location,
                ItemId = itemId,
                ImageTypeId = imageTypeId
            };
            if (pci == null)
                iu.ImageId = null;
            else
                iu.ImageId = pci.ImageId;

            return iu;
            }
            catch(Exception ex)
            {
                //Bll.ErrorHandler.SendError(ex, String.Format("Błąd przesyłania zdjęcia z FTP. ItemId: {0}, Zdjęcie: {1}, ProductCatalogID: {2}",
                //    itemId, pci.FileName, pci.ProductCatalogId));
                return null;
            }
        }



        private static Dal.ProductCatalogImageAllegroItem SendImageBinary(ProductCatalogImage pci, string fileName, long itemId, int imageTypeId)
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            Dal.AllegroItemUserView ai = asc.GetAllegroItemUser(itemId);
            string token = ai.Token;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://upload.allegro.pl/sale/images");
            request.Accept = "application/vnd.allegro.public.v1+json";
            FileInfo fi = new FileInfo(fileName);
            switch (fi.Extension.ToLower())
            {
                case "png":
                    request.ContentType = "image/jpeg"; 
                    break;
                default:
                    request.ContentType = "image/jpeg";
                    break;
            }
            request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");

            request.Method = "POST";
            byte[] image = GetImage(fileName, pci);

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(image, 0, image.Length);
            dataStream.Close();

            WebResponse webResponse = request.GetResponse();
            Stream responseStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string text = reader.ReadToEnd();

            Console.WriteLine("ItemId: {0}", itemId);

            var json_serializer = new JavaScriptSerializer();
            ImageObject imageObj = json_serializer.Deserialize<ImageObject>(text);


            Dal.ProductCatalogImageAllegroItem iu = new Dal.ProductCatalogImageAllegroItem()
            {
                ExpireDate = imageObj.expiresAt,
                InsertDate = DateTime.Now,
                LocationUrl = imageObj.location,
                ItemId = itemId,
                ImageTypeId = imageTypeId
            };
            if (pci == null)
                iu.ImageId = null;
            else
                iu.ImageId = pci.ImageId;

            return iu;

        }

        private static byte[] GetImage(string path, ProductCatalogImage image)
        {
            if (image == null)
                return System.IO.File.ReadAllBytes(path);

            if (image.Size <= 2000000 && image.Width < 2500 && image.Height < 2500)
            {
                return System.IO.File.ReadAllBytes(path);
            }
            else
            {
                using (Bitmap bmp = new Bitmap(path))
                {
                    int height = bmp.Height;
                    int width = bmp.Width;
                    decimal rate = bmp.Width / 1024M;
                    int newH = (int)(height / rate);
                    int newW = (int)(width / rate);

                    return Bll.Helper.ResizeImage(path,
                       "",
                       newW,
                       newH);
                }
            }
        }
        private static string GetImageFile(string fileName)
        {
            string path = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];

            return String.Format(path, fileName);
        }

        public static bool CreateImages(int productCatalogId)
        {    

            List<Dal.ProductCatalogAllegroItemsView> items  = 
                Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogAllegroItemsByPcId(productCatalogId)
                .Select(x=>new ProductCatalogAllegroItemsView()
                {
                    ProductCatalogId = x.ProductCatalogId,
                    ItemId = x.ItemId,
                    Id = x.Id
                }).ToList();

            Dal.OrderHelper oh = new Dal.OrderHelper();
            foreach (Dal.ProductCatalogAllegroItemsView item in items)
                oh.SetProductCatalogAllegroItemUpdate(item.ItemId, "1000000000000", false, null);

            return CreateImages(items);
        }
        //public static void CreateImages()
        //{
        //    DateTime dtStart = DateTime.Now;
        //    Guid processId = Guid.NewGuid();

        //    Dal.SettingsHelper sh = new Dal.SettingsHelper();
        //    Dal.Settings s = sh.GetSetting("ALL_IMGS");


        //    List<Dal.ProductCatalogAllegroItemsView> items = 
        //        Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogItemsForImageUpdate(s.IntValue.Value);

        //    if (items.Count() > 0)
        //    {
        //        long[] itemIds = items.Select(x => x.ItemId).ToArray();

        //        Dal.DbHelper.ProductCatalog.Allegro.SetProductCatalogAllegroItemProcess(processId, itemIds);

        //        items = Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogItemsForImageUpdate(processId);

        //        CreateImages(items);


        //        Dal.DbHelper.ProductCatalog.Allegro.SetProductCatalogAllegroItemProcessClear(processId, itemIds);
        //    }
        //    //if (items.Count() > 0)
        //    //{
        //    //    TimeSpan ts = DateTime.Now - dtStart;
        //    //    Bll.ErrorHandler.SendEmail(String.Format("Czas tworzenia  {0} zdjęć dla ofert Allegro: {1}", items.Count(), ts.TotalSeconds));
        //    //}
        //}

        //private static List<ProductCatalogAllegroItemsFnResult> GetAllegroItemsForImageUpdate(Guid processId)
        //{
        //    Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();

        //    Dal.SettingsHelper sh = new Dal.SettingsHelper();
        //    Dal.Settings s = sh.GetSetting("ALL_IMGS");
        //    List<Dal.ProductCatalogAllegroItemsFnResult> items = oh.GetProductCatalogItemsForImageUpdate(s.IntValue.Value, processId);


        //    return items;
        //}

        //private static List<Dal.ProductCatalogAllegroItemsFnResult> GetProductCatalogItemsForImageUpdate(int limit)
        //{
        //    Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();

        //    List<Dal.ProductCatalogAllegroItemsFnResult> items = oh.GetProductCatalogItemsForImageUpdate(limit); 


        //    return items;
        //}


        public static bool CreateImages(List<Dal.ProductCatalogAllegroItemsView> items)
        {
            Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();

            bool result = true;

            foreach (Dal.ProductCatalogAllegroItemsView item in items)
            {
                result = true;

                List<Dal.ProductCatalogImage> images = oh.GetProductCatalogImages(item.ProductCatalogId)
                    .Where(x => x.IsActive)
                    .ToList();

                Console.WriteLine("     Delete images from DB");
                oh.SetProductCatalogAllegroItemImageDelete(item.ItemId);

                string comment = "";
                string msg = "";
                foreach (Dal.ProductCatalogImage image in images)
                {
                    msg = "";



                    if (!SendImage(image, image.FileName, item.ItemId, 1, out msg))
                    {
                        result = false;
                        comment = msg;
                        if (!result)
                            break;
                    }
                }

              
                oh.SetProductCatalogAllegroItemImageReady(item.Id, result, comment);
            }
            return result;
        }

    
    }
}