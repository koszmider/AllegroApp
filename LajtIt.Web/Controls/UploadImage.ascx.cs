using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class UploadImage : LajtitControl
    {
        public delegate void SavedEventHandler(object sender, EventArgs e);
        public event SavedEventHandler Saved;


        public delegate int[] IdsEventHandler(object sender, EventArgs e);
        public event IdsEventHandler ImageIds;


        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void SetImages(int[] productCatalogIds)
        {
            string filepath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ProductCatalogImages"]);
            HttpFileCollection uploadedFiles = Request.Files;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbe = new StringBuilder();


            string path = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];

            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];

                try
                {
                    if (userPostedFile.ContentLength > 0)
                    {
                        string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(userPostedFile.FileName));
                        string orginalFileName = System.IO.Path.GetFileName(userPostedFile.FileName);
                        string saveLocation = String.Format(ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())], fileName);

                        if (saveLocation != null)
                            userPostedFile.SaveAs(saveLocation);

                        Bll.ProductCatalogHelper.SaveFile(productCatalogIds, saveLocation, fileName, orginalFileName);

                        sb.AppendLine(String.Format("{0}. plik: {1}<br>", i + 1, userPostedFile.FileName));


                        Bll.Helper.CreateThumbImage(path, fileName);
                    }
                }
                catch (Exception Ex)
                {
                    sbe.AppendLine(String.Format("{0}. plik: {1}<br>", i + 1, userPostedFile.FileName));
                }
            }
            if (sbe.Length == 0)
                DisplayMessage(String.Format("Zapisano poprawnie<br><br>{0}", sb.ToString()));
            else
                DisplayMessage(String.Format("Błędy<br><br>{0}", sbe.ToString()));


            if (Saved != null)
                Saved(this, null);

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            int[] productCatalogIds = null;
            if (ImageIds != null)
                productCatalogIds = ImageIds(this, e);
            SetImages(productCatalogIds);

        }

    }
}