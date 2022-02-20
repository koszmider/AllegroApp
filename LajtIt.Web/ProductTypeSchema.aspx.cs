using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("afd6038f-c15c-4516-98c9-a99dd7ddcf11")]
    public partial class ProductTypeSchema : LajtitPage
    {
        private int SchemaId { get { return Int32.Parse(ddlProductTypeSchema.SelectedValue); } }
        private int AttributeId { get { return Int32.Parse(ddlProductCatalogProductTypeAttribute.SelectedValue); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindProductTypeSchemas();
                BindProductTypeAttributes();
                BindProductTypeSchemaAttributes();
                BindAttributeGroups();
                BindProductTypes();
            }
        }

        private void BindAttributeGroups()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            ddlAtributeGroups.DataSource = pch.GetProductCatalogAttributeGroups().OrderBy(x=>x.Name).ToList();
            ddlAtributeGroups.DataBind();
        }

        private void BindProductTypeAttributes()
        { 

            ddlProductCatalogProductTypeAttribute.DataSource = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributes(6).OrderBy(x => x.Name).ToList();
            ddlProductCatalogProductTypeAttribute.DataBind();
        }

        private void BindProductTypeSchemas()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            ddlProductTypeSchema.DataSource = pch.GetProductTypeSchemas();
            ddlProductTypeSchema.DataBind();

            BindShops();
        }

        private void BindShops()
        {
            lbxShops.Enabled = lbxShopsOn.Enabled = btnAdd1.Enabled = btnDel.Enabled = SchemaId != 0 && SchemaId != 3;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            Dal.ProductCatalogProductTypeSchema schema=  pch.GetProductCatalogProductTypeSchema(SchemaId);

            if (schema.CanBeSendToShop == false)
            {
                lbxShops.Items.Clear();
                lbxShopsOn.Items.Clear();

                pnShops.Visible = false;

                return;
            }
            pnShops.Visible = true;
            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<Dal.Shop> shops = pch.GetProductCatalogProductTypeShops(SchemaId);

            lbxShopsOn.DataSource = shops;
            lbxShopsOn.DataBind();

            List<Dal.Shop> allShops = Dal.DbHelper.Shop.GetShops().Where(x=>x.IsActive && x.CanExportProducts).ToList();

            List<Dal.Shop> assignedShops = pch.GetProductCatalogProductTypeShops();
            int[] assignedShopIds = assignedShops.Select(x => x.ShopId).ToArray();
            lbxShops.DataSource = allShops.Where(x => !assignedShopIds.Contains(x.ShopId));
            lbxShops.DataBind();
        }

        protected void ddlProductTypeSchema_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProductTypeSchemaAttributes();
            BindShops();
            BindProductTypes();
        }

        public class Attribute
        {
            public int AttributeGroupId { get; set; }
            public string Name { get; set; }
            public int Order { get; set; }

        }
        private void BindProductTypeSchemaAttributes()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Attribute> attributes = pch.GetProductTypeSchemaAttributes(SchemaId, AttributeId)
                .Select(x => new Attribute()
                {
                    AttributeGroupId = x.AttributeGroupId,
                    Name = x.ProductCatalogAttributeGroup.Name,
                    Order = x.Order
                })
                .OrderBy(x => x.Order)
                .ToList();


            gvAttributeGroups.DataSource = attributes;
            gvAttributeGroups.DataBind();

            int[] groupIds = attributes.Select(x => x.AttributeGroupId).ToArray();

            List<Dal.ProductCatalogAttributeGroup> availableGroups = pch.GetProductCatalogAttributeGroups()
                .Where(x => !groupIds.Contains(x.AttributeGroupId))
                .OrderBy(x=>x.Name)
                .ToList();

            lbxAttributeGroups.DataSource = availableGroups;
            lbxAttributeGroups.DataBind();

        }
        private void BindProductTypes()
        {
            int attributeGroupId = Int32.Parse(ddlAtributeGroups.SelectedValue);

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Dal.ProductCatalogAttribute> attributes = pch.GetProductTypesForSchema(SchemaId, attributeGroupId)
                .ToList();


            gvProductTypesSelected.DataSource = attributes;
            gvProductTypesSelected.DataBind();

            int[] attributeIds = attributes.Select(x => x.AttributeId).ToArray();

            List<Dal.ProductCatalogAttribute> availableAttributes = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributes(6)
                .Where(x => !attributeIds.Contains(x.AttributeId))
                .OrderBy(x => x.Name)
                .ToList();

            lbxProductTypes.DataSource = availableAttributes;
            lbxProductTypes.DataBind();

        }
        protected void btnAdd1_Click(object sender, EventArgs e)
        {
            int[] shopIds = lbxShops.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();
            Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();

            oh.SetProductCatalogProductTypeShops(SchemaId, shopIds);
            DisplayMessage("Zapisano");
            BindShops();
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            int[] shopIds = lbxShopsOn.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();

            Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();
            oh.SetProductCatalogProductTypeShopsDelete(SchemaId, shopIds);
           // DisplayMessage("Zapisano");
            BindShops();

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int[] attIds = lbxAttributeGroups.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetProductCatalogProductTypeMembers(SchemaId, AttributeId, attIds);

            //DisplayMessage("Zapisano");

            BindProductTypeSchemaAttributes();
        }

        protected void btnSaveAttributeGroups_Click(object sender, EventArgs e)
        {


            List<Dal.ProductCatalogProductTypeMembers> membersToUpdate = new List<Dal.ProductCatalogProductTypeMembers>();
         
            foreach (GridViewRow row in gvAttributeGroups.Rows)
            {

                int attributeGroupId = Convert.ToInt32(gvAttributeGroups.DataKeys[row.RowIndex][0]);


                TextBox txbOrder = row.FindControl("txbOrder") as TextBox;
                CheckBox chbAttributeGroup = row.FindControl("chbAttributeGroup") as CheckBox;


                Dal.ProductCatalogProductTypeMembers member = new Dal.ProductCatalogProductTypeMembers()
                {
                    AttributeGroupId = attributeGroupId,
                    Order = Int32.Parse(txbOrder.Text),
                    ProductTypeSchemaId = SchemaId,
                    ProductTypeAttributeId = AttributeId
                };
                if (chbAttributeGroup.Checked)
                    member.Order = -1;

                    membersToUpdate.Add(member);

            }

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetProductCatalogProductTypeMembersUpdate(SchemaId, AttributeId, membersToUpdate);

          //  DisplayMessage("Zapisano");

            BindProductTypeSchemaAttributes();
        }

        protected void ddlProductCatalogProductTypeAttribute_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProductTypeSchemaAttributes();
        }
         
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetProductCatalogProductTypeSchema(txbName.Text.Trim());
            DisplayMessage("Zapisano");
            BindProductTypeSchemas();
            ddlProductTypeSchema.SelectedIndex = ddlProductTypeSchema.Items.Count - 1;
        }

        protected void ddlAtributeGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProductTypes();
        }

        protected void btnProductsDelete_Click(object sender, EventArgs e)
        {
            int attributeGroupId = Int32.Parse(ddlAtributeGroups.SelectedValue);

            List<Dal.ProductCatalogProductTypeMembers> membersToDelete = new List<Dal.ProductCatalogProductTypeMembers>();

            foreach (GridViewRow row in gvProductTypesSelected.Rows)
            {

                int attributeId = Convert.ToInt32(gvProductTypesSelected.DataKeys[row.RowIndex][0]);

                CheckBox chbAttribute = row.FindControl("chbAttribute") as CheckBox;


                Dal.ProductCatalogProductTypeMembers member = new Dal.ProductCatalogProductTypeMembers()
                {
                    AttributeGroupId = attributeGroupId,
                    Order = 0,
                    ProductTypeSchemaId = SchemaId,
                    ProductTypeAttributeId = attributeId
                };
                if (chbAttribute.Checked)
                    membersToDelete.Add(member);

            }

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetProductCatalogProductTypeMembersDelete(SchemaId, attributeGroupId, membersToDelete);

          //  DisplayMessage("Zapisano");

            BindProductTypeSchemaAttributes();
            BindProductTypes();
        }

        protected void btnProductsAdd_Click(object sender, EventArgs e)
        {
            int attributeGroupId = Int32.Parse(ddlAtributeGroups.SelectedValue);

            int[] attIds = lbxProductTypes.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetProductCatalogProductTypesForGroup(SchemaId, attributeGroupId, attIds);

         //   DisplayMessage("Zapisano");

            BindProductTypeSchemaAttributes(); BindProductTypes();
        }
    }

}