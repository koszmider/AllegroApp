using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("bfd8cee5-b946-45f8-8971-75c21989062b")]
    public partial class Maintanance : LajtitPage
    {
        protected void btnUpdateProductCatalogImages_Click(object sender, EventArgs e)
        {
            Bll.Helper.UpdateProductCatalogImages(HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ProductCatalogImages"]));
            DisplayMessage("Zrobione");
        }
    }
}