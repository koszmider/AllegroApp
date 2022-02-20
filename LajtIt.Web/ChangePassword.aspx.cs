using System;
using System.Web;

namespace LajtIt.Web.Account
{
    [Developer("9baf10b0-57a8-45f4-9858-efa285a5fb4b")]
    public partial class ChangePassword :LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnPasswordChange_Click(object sender, EventArgs e)
        {
            Bll.AdminUserHelper auh = new Bll.AdminUserHelper();
            Dal.AdminUser au = auh.IsAuthenticated(HttpContext.Current.User.Identity.Name, txbPasswordOld.Text.Trim());

            if (au != null) 
                if (auh.ChangePassword(HttpContext.Current.User.Identity.Name, txbPasswordOld.Text.Trim(), txbPassword.Text.Trim()))
                    DisplayMessage("Hasło zostało zmienione");
                else
                    DisplayMessage("Bład zmiany hasła. Upewnij się że wprowadzono poprawne hasło ");
            else
                DisplayMessage("Bład zmiany hasła. Upewnij się że wprowadzono poprawne hasło ");
        }
    }
}
