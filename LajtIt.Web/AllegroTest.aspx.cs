using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("83be80d0-a476-41d2-8609-55e64d0b8cb5")]
    public partial class AllegroTest : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {

                //Bll.AllegroHelper ah = new Bll.AllegroHelper();

                //Bll.AllegroHelper.GetVersionKeys();// (Dal.Helper.MyUsers.JacekStawicki.ToString());
                //string s = ah.GetSessionHandle("JacekStawicki");

//DisplayMessage(String.Format("<h1>Działa</h1>{0}", s));
            }
            catch (Exception ex)
            {
                DisplayMessage(String.Format("<h1>Błąd</h1>{0}<br>{1}", ex.Message, ex.StackTrace));
            }
        }
    }
}