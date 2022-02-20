using System;
using System.Collections.Generic;
using System.Linq;
using LajtIt.Bll;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class Products : LajtitControl
    {
        public delegate void ReloadedEventHandler();
        public event ReloadedEventHandler Reloaded;
        public int ProductCatalogId
        {
            set
            {
                ViewState["ProductCatalogId"] = value;
            }
            get
            {
                return Int32.Parse(ViewState["ProductCatalogId"].ToString());
            }
        }
        public int ProductCatalogId2
        {
            set
            {
                ViewState["ProductCatalogId2"] = value;
            }
            get
            {
                return Int32.Parse(ViewState["ProductCatalogId2"].ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
              
                

            }
        }
        public void Build()
        {
            lblPreview.Text = Bll.OrderHelper.GetPreviewCompare(Dal.Helper.Shop.Lajtitpl, ProductCatalogId, ProductCatalogId2);

        }

        protected void lbtnCopy_Click(object sender, EventArgs e)
        {
            Dal.DbHelper.ProductCatalog.SetProductCatalogAttributeCopy(ProductCatalogId2, ProductCatalogId);
            DisplayMessage("Atrybuty zostały skopiowane");
            Build();
        }

        protected void btnProductCompare_Click(object sender, EventArgs e)
        {
            lbtnCopy.Visible = true;
            ProductCatalogId2 = Int32.Parse(hfProductCatalogId.Value);
            Build();
            Reloaded?.Invoke();

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            Dal.ProductCatalogView pc = pch.GetProductCatalogView(ProductCatalogId);
            Dal.ProductCatalogView pc2 = pch.GetProductCatalogView(ProductCatalogId2);


            lbtnCopy.OnClientClick = String.Format("return confirm ('Czy skopiować atrybuty z {0} do {1}?')", pc2.Code, pc.Code);
        }
    }
}