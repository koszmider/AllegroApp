using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("73ead6b4-44dc-476e-a94f-a2a7df3da08c")]
    public partial class ProductImports : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Bll.ProductCatalogImportHelper h = new Bll.ProductCatalogImportHelper();
            gvImports.DataSource = h.BindImports();
            gvImports.DataBind();
        }
    }
}