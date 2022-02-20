using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("4eabe64c-8698-4c6b-951d-c9d6c57b5237")]
    public partial class AllegroSellMatrix : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ucAllegroSellMatrixControl1.EndDate = DateTime.Now.AddYears(-1).AddDays(14);
                ucAllegroSellMatrixControl2.EndDate = DateTime.Now.AddDays(-7);
            }
           
        }
        protected void btnAnime_Click(object sender, EventArgs e)
        {
            tmAnime.Enabled = !tmAnime.Enabled;
        }
        protected void Anime_Tick(object sender, EventArgs e)
        {
            ucAllegroSellMatrixControl1.EndDate = ucAllegroSellMatrixControl1.EndDate.AddDays(-7);
            ucAllegroSellMatrixControl2.EndDate = ucAllegroSellMatrixControl2.EndDate.AddDays(-7);
            ucAllegroSellMatrixControl1.BindMatrix();
            ucAllegroSellMatrixControl2.BindMatrix();
        }
    }
}