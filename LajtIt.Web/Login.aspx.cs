using System;
using System.Web;

namespace LajtIt.Web.Account
{
    [Developer("36762b31-1655-4a93-bfcb-d72733a326d6", false)]
    public partial class Login : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txbLoginName1.ClientIDMode = System.Web.UI.ClientIDMode.Predictable;
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Bll.AdminUserHelper auh = new Bll.AdminUserHelper();
                Dal.AdminUser au = auh.IsAuthenticated(txbLoginName1.Text.Trim(), txbPassword1.Text.Trim());

                if (au != null)
                {
                    if (au.IsActive)
                        System.Web.Security.FormsAuthentication.RedirectFromLoginPage(txbLoginName1.Text.Trim(), false);
                    else
                    {
                        DisplayMessage("Twoje konto zostało zablokowane. Skontaktuj się administratorem systemu w celu wyjaśnienia sytuacji.");
                    }
                }
                else
                    DisplayMessage("Niepoprawny login/hasło");
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
            }
        }
    }
}
