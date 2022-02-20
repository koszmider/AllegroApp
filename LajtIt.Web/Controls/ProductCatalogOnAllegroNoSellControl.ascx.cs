using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class ProductCatalogOnAllegroNoSellControl : LajtitControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        internal void BindNotSold()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            gvProducts.DataSource = pch.GetProductCatalogOnAllegroNoSell();
                gvProducts.DataBind();
        }
    }
}