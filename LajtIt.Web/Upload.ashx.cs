using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using LajtIt.Bll;

namespace LajtIt.Web
{

    public class Upload : IHttpHandler
    {
        private static object Locker = new object();

        public void ProcessRequest(HttpContext context)
        {
            int productCatalogId = 0;
            string oryginalFileName = "";
            try
            {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

                HttpPostedFile postedFile = context.Request.Files["Filedata"];
                  productCatalogId = Convert.ToInt32(context.Request.QueryString["folder"].Replace("/",""));

                string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(postedFile.FileName));
                  oryginalFileName = System.IO.Path.GetFileName(postedFile.FileName);
                string saveLocation = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ProductCatalogImages"]) + "\\" + fileName;


                postedFile.SaveAs(saveLocation); 
                context.Response.StatusCode = 200;
                 
                Bitmap bmp = new Bitmap(saveLocation);
                int height = bmp.Height;
                int width = bmp.Width;

                Dal.ProductCatalogImage image = new Dal.ProductCatalogImage()
                {
                    FileName = fileName,
                    Height = height,
                    InsertDate = DateTime.Now,
                    IsActive = true,
                    OriginalFileName = oryginalFileName,
                    Priority = 0,
                    ProductCatalogId = productCatalogId,
                    Size = postedFile.ContentLength,
                    Width = width,
                    Description = "",
                    ImageTypeId = 1
                };

                lock (Locker)
                {
                    Dal.OrderHelper oh = new Dal.OrderHelper();
                    oh.SetProductCatalogImage(image);
                }

            }
            catch (Exception ex)
            {
                ErrorHandler.LogError(ex, String.Format("productCatalogId:{0}, oryginalFileName: {1}", productCatalogId, oryginalFileName));
                context.Response.Write("Error: " + ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}