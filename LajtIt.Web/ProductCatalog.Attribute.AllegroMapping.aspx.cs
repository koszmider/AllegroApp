using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;
using LajtIt.Web.Controls;

namespace LajtIt.Web
{
    [Developer("04E48448-0A18-4B5C-8BCE-82E122133B26")]
    public partial class ProductAttributeAllegroMapping : LajtitPage
    {
       
        public int AttributeId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindCategories();
        }

        private void BindCategories()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ShopHelper sh = new Dal.ShopHelper();
             

            Dal.ProductCatalogAttribute attribute = Dal.DbHelper.ProductCatalog.GetProductCatalogAttribute(AttributeId);

            Dal.Helper.ShopType shopType = Dal.Helper.ShopType.Allegro;

            int[] categoriesWithGroup = pch.GetAllegroCategoryWithAttributeGroup(shopType, attribute.AttributeGroupId);

            var c = sh.GetCategories(shopType).Where(x => categoriesWithGroup.Contains(x.CategoryId)).OrderBy(x => x.Name).ToList();
            if (c.Count > 0)
            {
                ddlShopCategory.DataSource = c;
                ddlShopCategory.DataBind();
                //if (ShopCategoryId != null)
                //    ddlShopCategory.SelectedValue = ShopCategoryId.ToString();
                //else
                //    ShopCategoryId = ddlShopCategory.SelectedValue;

                BindAllegroCategoryParameters();
            }
        }

        private void BindAttribute()
        { 
               
            }
 
        protected void btnAttributeSave_Click(object sender, EventArgs e)
        {

            string shopCategoryId = ddlShopCategory.SelectedValue;
            Dal.DbHelper.ProductCatalog.SetProductCatalogAttributeAllegroExternalSource(AttributeId, shopCategoryId, ddlAllegroCategoryParameters.SelectedValue);

            DisplayMessage("Zapisano zmiany");
        }
        protected void ddlExternalSourceAllegroCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AllegroCategoryId = Convert.ToInt32((gvAttributes.HeaderRow.FindControl("ddlExternalSourceAllegroCategory") as  DropDownList).SelectedValue);
            //ShopCategoryId = (gvAttributes.HeaderRow.FindControl("ddlShopCategory") as DropDownList).SelectedValue;

            //BindAttributes();// BindAttributesGrid();


            BindAllegroCategoryParameters();
        }

        private void BindAllegroCategoryParameters()
        {
            ddlAllegroCategoryParameters.Items.Clear();
            ddlAllegroCategoryParameters.Items.Add(new ListItem());

            Dictionary<string, string> categoryParameters = new Dictionary<string, string>();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string shopCategoryId = ddlShopCategory.SelectedValue;

            Dal.ShopHelper sh = new Dal.ShopHelper();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            Dal.ProductCatalogAttribute attribute = Dal.DbHelper.ProductCatalog.GetProductCatalogAttribute(AttributeId);

            Dal.ShopCategoryField acf = sh.GetShopCategoryFieldsByCategoryId(Dal.Helper.ShopType.Allegro, shopCategoryId, null)
                .Where(x => x.AttributeGroupId == attribute.AttributeGroupId).FirstOrDefault();

            if (acf != null)
            {
                string newFieldId = acf.CategoryFieldId;

                foreach (Bll.AllegroRESTHelper.Parameters.Dictionary dict in Bll.AllegroRESTHelper.Parameters.GetDictionary(shopCategoryId, newFieldId))
                {
                    categoryParameters.Add(dict.id, dict.value);
                }

                ddlAllegroCategoryParameters.DataSource = categoryParameters;
                ddlAllegroCategoryParameters.DataBind();

                Dal.ProductCatalogAttributeAllegroExternalSource attributeAllegro =
                    Dal.DbHelper.ProductCatalog.GetProductCatalogAttributeAllegroExternalSource(AttributeId, shopCategoryId);



                if (attributeAllegro!=null && attributeAllegro.AllegroParameterId != null)
                {
                    ddlAllegroCategoryParameters.SelectedIndex =
                        ddlAllegroCategoryParameters.Items.IndexOf(ddlAllegroCategoryParameters.Items.FindByValue(attributeAllegro.AllegroParameterId.ToString()));
                }

            }
        }
    }
}