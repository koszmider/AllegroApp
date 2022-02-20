using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("2d4ea6be-a7e1-415c-8af9-8394f5e9df2f")]
    public partial class Shops : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindShops();
        }

        private void BindShops()
        {
            gvShops.DataSource = Dal.DbHelper.Shop.GetShops();
            gvShops.DataBind();
        }
    }
}