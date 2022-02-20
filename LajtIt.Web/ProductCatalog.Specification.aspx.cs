using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("a8270203-cd51-472a-bc9b-11a9a21a3e7a")]
    public partial class ProductCatalogSpecification : LajtitPage
    {
        private int ProductCatalogId
        {
            get { return Convert.ToInt32(Request.QueryString["id"]); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ucProductCompare.Reloaded += ReloadProductCompare;
            if (!Page.IsPostBack)
                BindAttributeGroups();
        }

        private void ReloadProductCompare()
        {
            mpeCompare.Show();
        }
        protected void btnAttributesSave_Click(object sender, EventArgs e)
        {
            ucProductAttributes.AttributesSave(true);

            DisplayMessage("Zapisano");
        }
        protected void lbtnAttributeGroupNewAdd_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogAttributeGroup attribute = new Dal.ProductCatalogAttributeGroup();
            attribute.Name = txbAttributeGroupNew.Text.Trim();
            attribute.AttributeGroupTypeId = Convert.ToInt32( ddlAttributeGroupType.SelectedValue);
            attribute.UpdateShopConfiguration = true;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogAttributeGroup(attribute);

            ddlAttributeGroupType.DataSource = pch.GetProductCatalogAttributeGroupTypes();
            ddlAttributeGroupType.DataBind();
            BindAttributeGroups();
        }

        private void BindAttributeGroups()
        {
            Dal.ProductCatalog pc = Dal.DbHelper.ProductCatalog.GetProductCatalog(ProductCatalogId);

            chbIsReady.Checked = pc.IsReady;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper(); ;

            ddlAttributeGroupType.DataSource = pch.GetProductCatalogAttributeGroupTypes().OrderBy(x => x.AttributeGroupTypeId).ToList();
            ddlAttributeGroupType.DataBind();
            ucProductAttributes.EnableBindGrouping = true;
            ucProductAttributes.BindAttributeGroups();
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            Dal.DbHelper.ProductCatalog.SetProductCatalogReady(ProductCatalogId, chbIsReady.Checked, UserName);
            DisplayMessage("Zmieniono ustawienia flagi");
        }

        protected void imgbCompare_Click(object sender, EventArgs e)
        {
            ucProductCompare.ProductCatalogId = ProductCatalogId;
            ucProductCompare.ProductCatalogId2 = 0;
          
            mpeCompare.Show();
        }
    }
}