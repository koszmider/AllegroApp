using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("0ad8fe98-801d-4202-bda5-2f630cd8f3af")]
    public partial class ProductGrouping : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGroups();
            }
        }

        private void BindGroups()
        {
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();

            List<Dal.ProductCatalogFamilyGroupView> groups = pch.GetProductCatalogFamilyGroup(ProductCatalogId);
            gvGrouping.DataSource = groups;
            gvGrouping.DataBind();
        }

        protected void gvGrouping_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if(e.Row.RowType== System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                Dal.ProductCatalogFamilyGroupView group = e.Row.DataItem as Dal.ProductCatalogFamilyGroupView;

                if (group.FamilyTypeId == (int)Dal.Helper.ProductCatalogGroupFamilyType.Family)
                    e.Row.Cells[0].Controls.Clear();
            }
        }

        protected void gvGrouping_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int groupId = Convert.ToInt32(gvGrouping.DataKeys[e.RowIndex][0]);


            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();
            pch.SetProductCatalogGroupDelete(ProductCatalogId, groupId);

            BindGroups();


            DisplayMessage("Usunieto z grupy");
        }
    }
}