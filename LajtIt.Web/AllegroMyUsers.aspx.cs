using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("a94c71c5-688b-483c-be90-4f6ccc94b69c")]
    public partial class AllegroMyUsers :  LajtitPage
    {
        public long UserId { get { return Convert.ToInt32(ddlUsers.SelectedValue); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Enctype = "multipart/form-data";

            ucUploadImage.Saved += ImageSaved;
            ucUploadImage.FileName += FileName;

            if (!Page.IsPostBack)
            {
                BindAllegroMyUsers(ddlUsers);
            }
        }
        protected string  FileName(object sender, EventArgs e)
        {
            return String.Format("{0}_{1}.jpg", UserId, "head" );
        }
        protected void ImageSaved(object sender, EventArgs e)
        {
            BindImages();
        }

        protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            pConfig.Visible = true;
            BindImages();
        }

        private void BindImages()
        {


            //string filepath = Bll.AllegroHelper.GetAllegroUserHeadImage(UserId, true);

            //if(filepath!=null)
            //{
            //    imgHader.ImageUrl = String.Format("{0}?{1}",filepath,DateTime.Now.Ticks);
            //    imgHader.Visible = true;
            //    lbtnDelete.Visible = true;
            //}
            //else
            //{
            //    imgHader.Visible = false;
            //    lbtnDelete.Visible = false;
            //}
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            string filepath = Bll.AllegroHelper.GetAllegroUserHeadImage(UserId, false);

            File.Delete(filepath);

            BindImages();
        }
    }
}