using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("20b59728-c6c7-431a-9928-82034c76f010")]
    public partial class ProductCatalogPreview : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["idProduct"]); } }
        public int Id { get { return Convert.ToInt32(Request.QueryString["id"]); } }
        private bool IsPreviewMode
        {
            get
            {
                if (Request.QueryString["preview"] == null || Request.QueryString["preview"] == "1")
                    return true;
                else
                    return false;

            }
        }
        private bool IsFromProductCatalog
        {
            get
            {
                if (Request.QueryString["catalog"] == null || Request.QueryString["catalog"] == "1")
                    return true;
                else
                    return false;

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Bll.OrderHelper oh = new Bll.OrderHelper();

            int id = IsFromProductCatalog ? ProductCatalogId : Id;

            int? idSupplier = null;
            if (Request.QueryString["idSupplier"] != null)
                idSupplier = Convert.ToInt32(Request.QueryString["idSupplier"]);

            if (!idSupplier.HasValue)
            {
                litPreview.Text = "Brak zdefiniowanego dostawcy";
                return;
            }

            Dal.Supplier supplier = oh.GetSupplier(idSupplier.Value);




            litPreview.Text = oh.GetProductCatalogPreview(supplier.AllegroUserIdAccount.Value, IsFromProductCatalog, IsPreviewMode, true, id);

            if (!IsPreviewMode)
                DisplayMessage("Widok produkcyjny. Jeśli jakieś zdjęcia/grafiki się nie wyświetlają, zgłoś to do mnie :)");
        }

    }
}