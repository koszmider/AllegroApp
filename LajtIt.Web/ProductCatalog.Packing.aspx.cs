using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("b5ead7ca-2260-4b95-8526-6895d9f6edde")]
    public partial class ProductPacking : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindPackings();
            }
        }

        private void BindPackings()
        {
            List<Dal.ProductCatalogPacking> packings = Dal.DbHelper.ProductCatalog.GetProductCatalogPackings(ProductCatalogId);

            gvPacking.DataSource = packings;
            gvPacking.DataBind();
        }

        protected void gvPacking_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Dal.ProductCatalogPacking packing = e.Row.DataItem as Dal.ProductCatalogPacking;

                Button btnEdit = e.Row.FindControl("btnEdit") as Button;
                Label lblSize = e.Row.FindControl("lblSize") as Label;

                lblSize.Text = Dal.DbHelper.Orders.GetParcelSizeInpostCode(packing.Size);
                btnEdit.CommandArgument = packing.Id.ToString();

            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {

            int id = Int32.Parse(((Button)sender).CommandArgument);

            Dal.ProductCatalogPacking packing = Dal.DbHelper.ProductCatalog.GetProductCatalogPacking(id);
            mpePacking.Show();
            ViewState["Id"] = id;
            txbWeight.Text = String.Format("{0}", packing.Weight);
            txbWidth.Text = String.Format("{0}", packing.Width);
            txbHeight.Text = String.Format("{0}", packing.Height);
            txbLength.Text = String.Format("{0}", packing.Length);
            switch (packing.IsOversize)
            {
                case true: ddlIsOversize.SelectedIndex = 1; break;
                case false: ddlIsOversize.SelectedIndex = 2; break;
            }
            if (packing.Size.HasValue)
                ddlSize.SelectedValue = packing.Size.ToString();

        }

        protected void lbtnPackingNew_Click(object sender, EventArgs e)
        {
            mpePacking.Show();
            ViewState["Id"] = null;
            txbWeight.Text = "";
            txbWidth.Text = "";
            txbHeight.Text = "";
            txbLength.Text = "";
            ddlIsOversize.SelectedIndex = 0;
            ddlSize.SelectedIndex = 0;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (ViewState["Id"] != null)
                id = Int32.Parse(ViewState["Id"].ToString());

            Dal.ProductCatalogPacking packing = new Dal.ProductCatalogPacking()
            {
                Height = GetIntOrNull(txbHeight),
                Width = GetIntOrNull(txbWidth),
                Length = GetIntOrNull(txbLength),
                Weight = GetIntOrNull(txbWeight),
                Size = GetIntOrNull(ddlSize),
                Id = id,
                ProductCatalogId = ProductCatalogId
            };

            switch(ddlIsOversize.SelectedIndex)
            {
                case 1: packing.IsOversize = true; break;
                case 2: packing.IsOversize = false; break;
            }

            Dal.DbHelper.ProductCatalog.SetProductCatalogPacking(packing);
            BindPackings();
            ViewState["Id"] = null;

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (ViewState["Id"] != null)
                id = Int32.Parse(ViewState["Id"].ToString());
            Dal.DbHelper.ProductCatalog.SetProductCatalogPackingDelete(id);

            BindPackings();
        }
    }
}