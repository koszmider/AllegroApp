using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("3f7a464b-287f-4be9-8a4e-36dab2186095")]
    public partial class AllegroTokens : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindUsers();
        }

        private void BindUsers()
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            ddlAllegroUsers.DataSource = asc.GetAllegroMyUsers();
            ddlAllegroUsers.DataBind();

            gvUsers.DataSource = asc.GetAllegroMyUsers();
            gvUsers.DataBind();
        }

        protected void btnGetToken_Click(object sender, EventArgs e)
        {

            string token = Bll.AllegroRESTHelper.GetToken( txbCode.Text.Trim(), Int32.Parse(ddlAllegroUsers.SelectedValue));

            DisplayMessage("Pobrano token");
            BindUsers();
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[Convert.ToInt32(e.CommandArgument)][0]);

            Bll.AllegroRESTHelper.GetRefreshToken(userId);
            DisplayMessage("Odświeżono token");
            BindUsers();
        }
    }
}