using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.AllegroWeb.Controls
{
    public partial class Kronos : System.Web.UI.UserControl
    {
        public string Header
        {
            set { litHeader1.Text = litHeader2.Text = litHeader3.Text = value; }
        }
        public string KronosType
        {
            set { ViewState["KronosType"] = value; }
            get { return ViewState["KronosType"].ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}