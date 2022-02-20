using System;
using System.Web.UI;
 
using LajtIt.Dal;

namespace LajtIt.Web
{
    [Developer("ac736682-3e5b-4e61-a96e-ad14ef1b0bb0")]
    public partial class ProductCatalogNotReady : LajtitPage
    { 
         
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!Page.IsPostBack)
            {
                BindSuppliers();
            }
        }

        private void BindSuppliers()
        {
            gvSuppliers.DataSource = LajtIt.Dal.DbHelper.ProductCatalog.GetSupplierNotReadyProducts();
            gvSuppliers.DataBind();
        }
    }
}