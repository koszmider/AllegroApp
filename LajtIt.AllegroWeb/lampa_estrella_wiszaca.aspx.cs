using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.AllegroWeb
{
    public partial class EstrellaPage : System.Web.UI.Page, IPageColor
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LampyOfertaKolory1.Model = "estrella";
        }

        public void SetPageColor(string imageUrl)
        { 
        }


        public void SetPageColor(string imageUrl, string imageUrl2)
        {
            pPhotoColor.Visible = true;
            pPhotoColor2.Visible = true;
            imgColor_1_1.ImageUrl = imageUrl.Replace("II", "I");
            imgColor_1_2.ImageUrl = imageUrl2.Replace("II", "I");
            imgColor_2_1.ImageUrl = imageUrl;
            imgColor_2_2.ImageUrl = imageUrl2;
        }
    } 
}
