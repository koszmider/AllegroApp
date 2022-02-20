using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("30627906-a31b-4c89-8fff-345336807ae2", false)]
    public partial class NoAccess : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Request.QueryString["err"])
            {
                case "NO_ACCESS":
                    lblInfo.Text = String.Format("Brak dostępu do zasobu {0}", Request.QueryString["g"]);
                    break;
                case "NO_REG":
                    lblInfo.Text = String.Format("Zasób niezarejestrowany");
                    break;
                case "ERR_ORDER":
                    lblInfo.Text = String.Format("Brak dostępu do zamówienia {0}", Request.QueryString["id"]);
                    break;


            }
        }
    }
}