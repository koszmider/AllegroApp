using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class UploadImageAllegroUser : LajtitControl
    {
        public delegate void SavedEventHandler(object sender, EventArgs e);
        public event SavedEventHandler Saved;


        public delegate string FileName1(object sender, EventArgs e);
        public event FileName1 FileName;


        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void SetImages(string fn)
        {
            string filepath = Server.MapPath("/images/AllegroUsers");
            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);



            HttpFileCollection uploadedFiles = Request.Files;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbe = new StringBuilder();



            HttpPostedFile userPostedFile = uploadedFiles[0];

            try
            {
                if (userPostedFile.ContentLength > 0)
                {
                    string fileName = String.Format(@"{0}\{1}", filepath, fn);

                    if (fileName != null)
                        userPostedFile.SaveAs(fileName);




                }
            }
            catch (Exception Ex)
            {
                DisplayMessage(String.Format("Błąd zapisu pliku {0}", Ex.Message));
            }


            if (Saved != null)
                Saved(this, null);

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            string fileName = null;
            if (FileName != null)
                fileName = FileName(this, e);
            SetImages(fileName);

        }

    }
}