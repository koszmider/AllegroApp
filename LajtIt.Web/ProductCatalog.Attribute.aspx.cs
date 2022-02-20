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
    [Developer("8ae4cbc2-b5b6-44aa-864e-0d7851aaf51f")]
    public partial class ProductAttribute : LajtitPage
    {

        public int AttributeId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindAttribute();
        }

        private void BindAttribute()
        {

            Dal.ProductCatalogAttribute attribute = Dal.DbHelper.ProductCatalog.GetProductCatalogAttribute(AttributeId);

            if (attribute == null)
            {
                ucAttributeMenu.NotExists();
                pnContent.Visible = false;
                return;
            }

            switch (attribute.SexTypeId)
            {
                case 1: rbSex1.Checked = true; break;
                case 0: rbSex0.Checked = true; break;
                case 2: rbSex2.Checked = true; break;
                default: rbSex.Checked = true; break;
            }

            txbName.Text = attribute.Name;
            txbCode.Text = attribute.Code;
            txbFieldTemplate.Text = attribute.FieldTemplate;
            txbFriendlyDescriptionF.Text = attribute.FriendlyDescriptionF;
            txbFriendlyDescriptionM.Text = attribute.FriendlyDescriptionM;
            txbFriendlyDescriptionN.Text = attribute.FriendlyDescriptionN;
            txbFriendlyNameF.Text = attribute.FriendlyNameF;
            txbFriendlyNameM.Text = attribute.FriendlyNameM;
            txbFriendlyNameN.Text = attribute.FriendlyNameN;

            if (attribute.AttributeTypeId.HasValue)
                ddlAttributeType.SelectedValue = attribute.AttributeTypeId.ToString();

            if (attribute.ProductCatalogAttributeGroup.AttributeGroupTypeId == 3)
            {
                if (attribute.AttributeTypeId.HasValue)
                {
                    ddlAttributeType.SelectedValue = attribute.AttributeTypeId.ToString();
                }

                txbFriendlyDescriptionF.Enabled = txbFriendlyDescriptionM.Enabled = txbFriendlyDescriptionN.Enabled =
                    txbFriendlyNameF.Enabled = txbFriendlyNameM.Enabled = txbFriendlyNameN.Enabled = false;
            }
            else
            {
                ddlAttributeType.Enabled = false;
                txbFieldTemplate.Enabled = false;
            }
        }

        protected void btnAttributeSave_Click(object sender, EventArgs e)
        {
            int? sex = null;
            int? attributeTypeId = null;


            Dal.ProductCatalogAttribute attribute = Dal.DbHelper.ProductCatalog.GetProductCatalogAttribute(AttributeId);

            if (ddlAttributeType.SelectedIndex != 0)
            {
                attributeTypeId = Int32.Parse(ddlAttributeType.SelectedValue);
            }
            else
                attributeTypeId = attribute.AttributeTypeId;

            if (rbSex1.Checked) sex = 1;
            if (rbSex0.Checked) sex = 0;
            if (rbSex2.Checked) sex = 2;

            string code = null;
            if (!String.IsNullOrEmpty(txbCode.Text.Trim()))
                code = txbCode.Text.Trim();

            Dal.ProductCatalogAttribute att = new ProductCatalogAttribute()
            {
                AttributeId = AttributeId,
                SexTypeId = sex,
                AttributeTypeId = attributeTypeId,
                AttributeGroupId = attribute.AttributeGroupId,
                Code = code,
                FieldTemplate = String.IsNullOrEmpty(txbFieldTemplate.Text.Trim()) ? null : txbFieldTemplate.Text.Trim(),
                FriendlyDescriptionF = String.IsNullOrEmpty(txbFriendlyDescriptionF.Text.Trim()) ? null : txbFriendlyDescriptionF.Text.Trim(),
                FriendlyDescriptionM = String.IsNullOrEmpty(txbFriendlyDescriptionM.Text.Trim()) ? null : txbFriendlyDescriptionM.Text.Trim(),
                FriendlyDescriptionN = String.IsNullOrEmpty(txbFriendlyDescriptionN.Text.Trim()) ? null : txbFriendlyDescriptionN.Text.Trim(),
                FriendlyNameF = String.IsNullOrEmpty(txbFriendlyNameF.Text.Trim()) ? null : txbFriendlyNameF.Text.Trim(),
                FriendlyNameM = String.IsNullOrEmpty(txbFriendlyNameM.Text.Trim()) ? null : txbFriendlyNameM.Text.Trim(),
                FriendlyNameN = String.IsNullOrEmpty(txbFriendlyNameN.Text.Trim()) ? null : txbFriendlyNameN.Text.Trim(),
                Name = txbName.Text.Trim(),
                SortOrder = attribute.SortOrder
            };

            int result = Dal.DbHelper.ProductCatalog.SetProductCatalogAttributeUpdate(att);

            switch (result)
            {
                case 1:
                    DisplayMessage("Zapisano zmiany"); break;
                case 0:
                    DisplayMessage("Błąd"); break;
                case -1:
                    DisplayMessage("Podano istniejącą wartość dla pola KOD. Wstaw unikalną w obrębie grupy"); break;
            }
        }
    }
}