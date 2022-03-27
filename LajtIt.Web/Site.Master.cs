using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace LajtIt.Web
{
    public partial class LajtitMasterPage1 : LajtitMasterPage
    {
        public int SignOutTimeout { set { tmLogout.Enabled = true; tmLogout.Interval = value; } }
      
        protected void Page_Load(object sender, EventArgs e)
        {


            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                lblUserName.Text = String.Format("Zalogowany jako: {0}", HttpContext.Current.User.Identity.Name);
                lblStyle.Visible = HttpContext.Current.User.Identity.Name == "tadeusz";

                pnlUser.Visible = true;

                ucSystemMenu.Visible = true;
                ucSystemMenu.BindMenu();
                Dal.TaskTrackerHelper tth = new Dal.TaskTrackerHelper();
                int taskCount = tth.GetTaskWaitingCount(UserName);
                if (taskCount > 0)
                {
                    litTasksCount.Text = taskCount.ToString();

                    if (Session["taskinfo"] == null)
                    {
                        DisplayMessage(String.Format("Witaj<br><br>Oczekujących zadań: <b>{0}</b>. Otwórz stronę z <a href='UserTasks.aspx'>Zadaniami do wykonania</a> i sprawdź szczegóły.", taskCount));
                        Session["taskinfo"] = taskCount;
                    }
                }
                else
                {
                    litTasksCount.Text = taskCount.ToString();
                }
            }
            else
            {
                hlTasks.Visible = false;
                ucSystemMenu.Visible = true;
            }
                Dal.Helper.SetEnv();

            if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Prod)
            {
                divHeader.Attributes.Remove("class");
                divHeader.Attributes.Add("class", "header prod");
            }

            if (Request.QueryString.AllKeys.Contains("hideMenu"))
            {
                divHeader.Attributes.Remove("class");
                divHeader.Attributes.Add("class", "hidden");
            }

            // get a reference to ScriptManager and check if we have a partial postback

            // partial (asynchronous) postback occured
            // insert Ajax custom logic here
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "initMe11", "initMe();", true);

            if (HttpContext.Current.User.Identity.Name == "tomek")
            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                int cntr = pch.GetProductCatalogUpdateScheduleProcessIsNullCounter();
                lblInfo.Text = String.Format("Zadań: {0}", cntr);
            }

            if (!HttpContext.Current.User.Identity.Name.Equals(""))
            {
                ListBoxSuppliers.Attributes.Add("Style", "background-color:#feeebd");

                List <Dal.Supplier> ls = Dal.DbHelper.ProductCatalog.GetSuppliers();
                ls = ls.Where(x => x.LastImportDate != null && x.IsActive == true && x.SupplierImportType.Name.Equals("Automatycznie")).ToList();
                ls = ls.Where(x => x.LastImportDate.Value < DateTime.Now.AddDays(-1)).OrderBy(x => x.Name).ToList();

                if (ls.Count() > 0)
                {
                    ListBoxSuppliers.Visible = true;
                    ListBoxSuppliers.DataSource = ls.Select(x => x.Name);
                    ListBoxSuppliers.DataBind();
                }
                else
                {
                    ListBoxSuppliers.Visible = false;
                }
            }

        }
        protected void lbtnLogout_Click(object sender, EventArgs e)
        {
            SignOut();

        }

        private void SignOut()
        {
            Session["taskinfo"] = null;
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        public void DisplayMessage(string text)
        {
            //System.Threading.Thread.Sleep(2000);
            divDialog.InnerHtml = text;
            // lblInfo.Text = text;

           // if (Page.IsPostBack)
            {
                // get a reference to ScriptManager and check if we have a partial postback
                if (ScriptManager.GetCurrent(this.Page).IsInAsyncPostBack)
                {
                    // partial (asynchronous) postback occured
                    // insert Ajax custom logic here
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "msg", "showDialog();", true);
                }
                else
                {
                    // regular full page postback occured
                    // custom logic accordingly           
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "msg", "showDialog();", true);
                }
            }
        }
    }
}
