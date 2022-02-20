using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("9b21f22a-e92a-4ee3-bee4-b47db0eb43ec")]
    public partial class ProductCatalogAttributesGrouping : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                Dal.ProductCatalogHelper ph = new Dal.ProductCatalogHelper();

                var g = ph.GetProductCatalogAttributeGroupingTypes();
                ddlGroupingType.DataSource = g;
                ddlGroupingType.DataBind();
                rblGroupingTypes.DataSource = g;
                rblGroupingTypes.DataBind();
                rblGroupingTypes.SelectedIndex = 0;
                BindGroupings();

                
            }
          
        }

        protected void ShopCategoryAdded(object sender, EventArgs e)
        {
            BindGroupings();

        }

        private void BindGroupings()
        {
            Dal.ProductCatalogHelper ph = new Dal.ProductCatalogHelper();

           
            gvGroupings.DataSource = ph.GetProductCatalogAttributeGroupings(Int32.Parse(rblGroupingTypes.SelectedValue));
            gvGroupings.DataBind();
        }

        protected void lbtnGroupingAdd_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogAttributeGrouping scm = new Dal.ProductCatalogAttributeGrouping()
            { 
                Name = txbName.Text.Trim(),
                GroupingTypeId= Int32.Parse(ddlGroupingType.SelectedValue)
            };

            Dal.ProductCatalogHelper ph = new Dal.ProductCatalogHelper();
            Response.Redirect(String.Format("ProductCatalog.Attributes.Grouping.Page.aspx?id={0}", ph.SetProductCatalogAttributeGrouping(scm)));


        }

        protected void rblGroupingTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGroupings();
        }
    }
}