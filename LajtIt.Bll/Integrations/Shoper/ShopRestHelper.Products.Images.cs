using Lajtit.Bll.Altavola;
using LinqToExcel.Extensions;
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
    public partial class ShopRestHelper
    {
        public class ProductsImages
        {

            public class Image
            {
                public string gfx_id { get; set; }
                public string product_id { get; set; }
                public string name { get; set; }
                public string file { get; set; }
                public string url { get; set; }
                public string content { get; set; }
                public string hidden { get; set; }
                public int main { get; set; }
                public Translations translations { get; set; }
            }
            public class Translations
            {
                public PlPL pl_PL { get; set; }
            }

            public class RootImages
            {
                public string count { get; set; }
                public int pages { get; set; }
                public int page { get; set; }
                public List<Image> list { get; set; }
            }

            public class PlPL
            {
                public string translation_id { get; set; }
                public string gfx_id { get; set; }
                public string name { get; set; }
                public string lang_id { get; set; }
            }

            public static bool SetImages(Dal.Helper.Shop shop, int[] productCatalogIds)
            {
                foreach(int productCatalogId in productCatalogIds)
                {
                    SetImages(shop, productCatalogId);
                }

                return true;
            }
            public static bool SetImages(Dal.Helper.Shop shop, int productCatalogId)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();

                Dal.ProductCatalogShopProduct psp = sh.GetProductCatalogShopProduct((int)shop, productCatalogId);

                List<Dal.ProductCatalogShopImageFnResult> images = sh.GetProductCatalogImages(shop, productCatalogId);

                DeleteImages(shop, psp.ShopProductId, images);
                images = sh.GetProductCatalogImages(shop, productCatalogId);
                try
                {
                    int main = 1;

                    foreach (Dal.ProductCatalogShopImageFnResult image in images.Where(x=>x.IsActive).ToList())
                    {
                        int shopImageId = SetImage(shop, psp, image, main, true);

                        if (shopImageId > 1)
                            sh.SetProductCatalogImageShopImageId(shop, image.ImageId, shopImageId);

                        main++;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Dal.ErrorHandler.LogError(ex, "Aktualizacja zdjęć");
                    return false;
                }
            }
            private static void DeleteImages(Dal.Helper.Shop shop, string shopProductId, List<Dal.ProductCatalogShopImageFnResult> images)
            {
                try
                {
                    int[] imagesIdsDb = images.Where(x => x.ShopImageId.HasValue && x.IsActive == true).Select(x => x.ShopImageId.Value).ToArray();

                    int[] imageIdsShop = GetImages(shop, shopProductId);


                    int[] imagesToDelete = imageIdsShop;
                    // hack do momentu az naprawią pole sortowania zdjec w sklepie
                    // imageIdsShop.Where(x => !imagesIdsDb.Contains(x)).ToArray();





                    List<Bulk.BulkObject> bulkObjects = new List<Bulk.BulkObject>();

                    Dal.ShopHelper sh = new Dal.ShopHelper();

                    foreach (int imageId in imagesToDelete)
                    {
                        string id = String.Format("product-image-delete_PId-{0}", imageId);

                        bulkObjects.Add(
                            new Bulk.BulkObject()
                            {
                                id = id,
                                body = null,
                                method = "DELETE",
                                path = String.Format("/webapi/rest/product-images/{0}" , imageId)
                            });
                        sh.SetProductCatalogImageShopImageDelete(shop, imageId);
                    }


                    Bulk.BulkResult result = Bulk.Sent(shop, bulkObjects);

                    //if (imagesToDelete.Length > 0)
                    //{

                    //    Object[] att = { productId, imagesToDelete, true };
                    //    Object[] methodParams = { SessionId, "product.image.list.delete", att };

                    //    var r = Bll.ShopHelper.SendApiRequest("call", methodParams);
                    //}
                }
                catch (Exception ex)
                {
                    Dal.ErrorHandler.LogError(ex, "DeleteImages");
                }
            }

            private static void DeleteImage(Dal.Helper.Shop shop, string shopProductId, int shopImageId)
            {
                try
                {
                    


                    List<Bulk.BulkObject> bulkObjects = new List<Bulk.BulkObject>();

 
                        string id = String.Format("product-image-delete_PId-{0}", shopImageId);

                        bulkObjects.Add(
                            new Bulk.BulkObject()
                            {
                                id = id,
                                body = null,
                                method = "DELETE",
                                path = String.Format("/webapi/rest/product-images/{0}", shopImageId)
                            });

          

                    Bulk.BulkResult result = Bulk.Sent(shop, bulkObjects);

          
                }
                catch (Exception ex)
                {
                    Dal.ErrorHandler.LogError(ex, "DeleteImages");
                }
            }
            private static int[] GetImages(Dal.Helper.Shop shop, string shopProductId)
            {
                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, 
                        @"/webapi/rest/product-images?filters={""product_id"":{""="":"""+shopProductId+@"""}}", "GET");

                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    RootImages images = json_serializer.Deserialize<RootImages>(text);

                    if (images == null)
                        return null;
                    else
                        return images.list.Select(x => Int32.Parse(x.gfx_id)).ToArray();

                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop, String.Format("ShopProductId: {0}", shopProductId));
                    return null;
                }

            }

            private static int SetImage(Dal.Helper.Shop shop, Dal.ProductCatalogShopProduct psp, Dal.ProductCatalogShopImageFnResult image, int main, bool overwriteImageId)
            {
                Image img = new Image()
                {
                    hidden = image.IsActive ? "0" : "1",
                    file= image.FileName,
                    translations=new Translations()
                    {
                        pl_PL = new PlPL()
                        {
                            name = psp.Name
                        }
                    }, 
                    product_id = psp.ShopProductId,
                    main = main==1?1:0
                };

                bool createNew = !(image.ShopImageId.HasValue && overwriteImageId == false);


                if (createNew)
                {
                    string path = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];
                    img.content = Convert.ToBase64String(GetImage(path, image));
                }

                HttpWebRequest request;


                try
                {
                    if (createNew)
                        request = ShopRestHelper.GetHttpWebRequest(shop, "/webapi/rest/product-images", "POST");
                    else
                    {
                        request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/product-images/{0}", image.ShopImageId), "PUT");
                        img.gfx_id = image.ShopImageId.ToString();
                        img.translations.pl_PL.gfx_id = image.ShopImageId.ToString();
                    }
                     
                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(img);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();



                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();
                     

                    if (createNew)
                        return Int32.Parse(text);
                    else
                        return image.ShopImageId.Value;

                }

                catch (WebException ex)
                {
                 
                    if (image.ShopImageId.HasValue && !createNew && ex.Message.Contains("404"))
                    {
                        //DeleteImage(shop, psp.ShopProductId, image.ShopImageId.Value);
                        Dal.ShopHelper sh = new Dal.ShopHelper();
                        sh.SetProductCatalogImageShopImageDelete(image.ImageId);
                    }
                    if (ex.Message.Contains("404"))
                        return SetImage(shop, psp, image, main, true);
                    else  
                        ProcessException(ex, shop, String.Format("ShopProductId: {0}, ProductCatalogId: {1}, overwriteImageId: {2}", psp.ShopProductId, psp.ProductCatalogId, overwriteImageId));



                }

                return 0;


                //Object[] att = { shopProductId, d, true };
                //Object[] methodParams = { SessionId, "product.image.save", att };

                //var o = Bll.ShopHelper.SendApiRequest("call", methodParams);

                //int shopImageId = 0;

                //if (Int32.TryParse(o.ToString(), out shopImageId))
                //{
                //    if (shopImageId == 1)
                //        return image.ShopImageId.Value;
                //    //else
                //    //    return
                //}
                //else
                //{

                //    Dictionary<string, object> o1 = (Dictionary<string, object>)o;


                //    if (o1["code"].ToString() == "31" && overwriteImageId == false)
                //        return SetImage(shopProductId, pc, image, true);
                //}
                //return (int)o;

            }

            private static byte[] GetImage(string path, Dal.ProductCatalogShopImageFnResult image)
            {
                if (image.Size <= 1000000)
                {
                    return System.IO.File.ReadAllBytes(String.Format(path, image.FileName));
                }
                else
                {
                    using (Bitmap bmp = new Bitmap(String.Format(path, image.FileName)))
                    {
                        int height = bmp.Height;
                        int width = bmp.Width;
                        decimal rate = bmp.Width / 1024M;
                        int newH = (int)(height / rate);
                        int newW = (int)(width / rate);

                        return Bll.Helper.ResizeImage(String.Format(path, ""),
                           image.FileName,
                           newW,
                           newH);

                    }

                }

            }

            public static void GetImages(Dal.Helper.Shop shop, int productCatalogId)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                //195240
                Dal.ProductCatalogShopProduct psp = sh.GetProductCatalogShopProduct((int)shop, productCatalogId);
                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/product-images/{0}", psp.ShopProductId), "GET");


                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    //Product product = json_serializer.Deserialize<Product>(text);
                    //product.attributes = ProcessAttributes(product.attributes);

                    //return product;

                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop, String.Format("ShopProductId: {0}", psp.ShopProductId));
                   // return null;
                }


            }


        }
    }
}