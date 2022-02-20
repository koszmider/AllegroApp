using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.AllegroWeb
{
    public partial class LampaWiszacaSpiralTrioPage : System.Web.UI.Page, IPageColor
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            LampyOfertaKolory1.Model = "spiral";
        }

        public void SetPageColor(string imageUrl)
        {
            pPhotoColor.Visible = true;
            imgColor.ImageUrl = imageUrl;
        }


        public void SetPageColor(string imageUrl, string imageUrl2)
        {
            pPhotoColor.Visible = true;
            imgColor.ImageUrl = imageUrl;
            divPhoto2.Visible = true;
            imgColor2.ImageUrl = imageUrl2;
        }
    }
}
