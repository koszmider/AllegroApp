using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("83acc15b-f107-46ab-a252-90b1167cd081")]
    public partial class DescriptionMixer : LajtitPage
    {
        private int AttributeGroupId
        {
            get
            {
                return Convert.ToInt32(ddlAttributeGroups.SelectedValue);
            }
        }
        private int ShopTypeId
        {
            get
            {
                return Convert.ToInt32(ddlShopType.SelectedValue);
            }
        }
        private int? AttributeGroupingId
        {
            get
            {
                int? attributeGroupingId = null;
                if (ddlProductAttributeGroupings.SelectedIndex != 0)
                    attributeGroupingId = Int32.Parse(ddlProductAttributeGroupings.SelectedValue);

                return attributeGroupingId;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShopTypes();
                BindAttributeGroups();
                BindAttributesGrouping();
            }
        }

        private void BindShopTypes()
        {
            ddlShopType.DataSource = Dal.DbHelper.Shop.GetShopTypes().Where(x => x.ShopTypeId == 1 || x.ShopTypeId == 2).ToList();
            ddlShopType.DataBind();
        }

        private void BindAttributesGrouping()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            ddlProductAttributeGroupings.DataSource = pch.GetProductCatalogAttributeGroupings(1);
            ddlProductAttributeGroupings.DataBind();
        }

        private void BindAttributes(int? attributeId)
        {
            if (attributeId.HasValue)
                ddlAttributes.SelectedIndex = ddlAttributes.Items.IndexOf(ddlAttributes.Items.FindByValue(attributeId.ToString()));
            else
                ddlAttributes.SelectedIndex = 0;

        }
        private void BindAttributeGroups()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            ddlAttributeGroups.DataSource = pch.GetProductCatalogAttributeGroups()
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    Name = String.Format("{0} ({1})", x.Name, x.GroupCode),
                    AttributeGroupId = x.AttributeGroupId
                }
                )
                .ToList();
            ddlAttributeGroups.DataBind(); 

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["idg"]))
                {
                    ddlAttributeGroups.SelectedValue = Request.QueryString["idg"];
                    ddlAttributeGroups_OnSelectedIndexChanged(null, null);
                }

            }
        }
        protected void ddlAttributeGroups_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAttributeGroups.Items[0].Text == "")
            {
          
                ddlAttributeGroups.Items.RemoveAt(0);
            }
             
            ddlAttributes.Items.Clear();
            ddlAttributes.DataSource = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributes(AttributeGroupId).OrderBy(x => x.Name).ToList();
            ddlAttributes.DataBind();
            ddlAttributes.Items.Insert(0, new ListItem("wszystkich atrybutów", "0"));
             
            pnAttributeGroup.Visible = true;

            BindAttributeGroupsMixer();
            BindAttributes();
            ClearForm();
        }

        private void BindAttributeGroupsMixer()
        {
            Dal.MixerHelper mixer = new Dal.MixerHelper();
             
            gvAttributeGroup.DataSource = mixer.GetAttributeGroupMixers(AttributeGroupId, ShopTypeId);
            gvAttributeGroup.DataBind();
        }

        protected void gvAttributeGroup_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            int id = Convert.ToInt32(gvAttributeGroup.DataKeys[e.NewSelectedIndex][0]);
             
            btnSaveEdit.Visible = true;
            btnSaveNew.Visible = false;

            BindAttributeGroupEdit(id);
        }

        private void BindAttributeGroupEdit(int id)
        {
            ViewState["id"] = id;

            Dal.MixerHelper mixer = new Dal.MixerHelper();

            Dal.ProductCatalogMixerAttributeGroup mag = mixer.GetAttributeGroupMixer(id);

            txbTemplateM.Text = mag.TemplateM;
            txbTemplateF.Text = mag.TemplateF;
            cbxIsActive.Checked = mag.IsActive;
            if (mag.AttributeGroupingId.HasValue)
                ddlProductAttributeGroupings.SelectedValue = mag.AttributeGroupingId.ToString();

            BindAttributes(mag.AttributeId);

        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            int? attributeId = null; 
            if (ddlAttributes.SelectedIndex != 0)
                attributeId = Int32.Parse(ddlAttributes.SelectedValue); 
            int id = Int32.Parse(ViewState["id"].ToString());

            Dal.ProductCatalogMixerAttributeGroup mag = new Dal.ProductCatalogMixerAttributeGroup()
            {
                IsActive = cbxIsActive.Checked,
                TemplateF = txbTemplateF.Text.Trim(),
                TemplateM = txbTemplateM.Text.Trim(),
                TemplateN = txbTemplateN.Text.Trim(),
                Id = id,
                AttributeGroupId = AttributeGroupId,
                AttributeId = attributeId,
                AttributeGroupingId = AttributeGroupingId,
                ShopTypeId = ShopTypeId
            };

            Dal.MixerHelper mixer = new Dal.MixerHelper();
            mixer.SetAttributeGroupMixer(id, mag);
            BindAttributeGroupsMixer();

            ClearForm();
            DisplayMessage("Zmiany zapisane");
        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
            int? attributeId = null;
            if (ddlAttributes.SelectedIndex != 0)
                attributeId = Int32.Parse(ddlAttributes.SelectedValue);

            Dal.ProductCatalogMixerAttributeGroup mag = new Dal.ProductCatalogMixerAttributeGroup()
            {
                IsActive = cbxIsActive.Checked,
                TemplateF = txbTemplateF.Text.Trim(),
                TemplateM = txbTemplateM.Text.Trim(),
                TemplateN = txbTemplateN.Text.Trim(),
                AttributeGroupId = AttributeGroupId,
                AttributeId = attributeId,
                AttributeGroupingId = AttributeGroupingId,
                ShopTypeId = ShopTypeId
            };

            Dal.MixerHelper mixer = new Dal.MixerHelper();
            mixer.SetAttributeGroupMixer(mag);

            BindAttributeGroupsMixer();

            ClearForm();
        }


        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txbTemplateF.Text = "";
            txbTemplateM.Text = "";
            txbTemplateN.Text = "";
            cbxIsActive.Checked = false;
            ViewState["id"] = null;

            btnSaveEdit.Visible = false;
            btnSaveNew.Visible = true;

            gvAttributeGroup.SelectedIndex = -1;

            ddlProductAttributeGroupings.SelectedIndex = 0;

            BindAttributes(null);
        }

        protected void gvAttributeGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTemplateM = e.Row.FindControl("lblTemplateM") as Label;
                Label lblTemplateF = e.Row.FindControl("lblTemplateF") as Label;
                Label lblTemplateN = e.Row.FindControl("lblTemplateN") as Label;

                Dal.ProductCatalogMixerAttributeGroup mag = e.Row.DataItem as Dal.ProductCatalogMixerAttributeGroup;

                lblTemplateF.Text = MakeBold(mag.TemplateF);
                lblTemplateM.Text = MakeBold(mag.TemplateM);
                lblTemplateN.Text = MakeBold(mag.TemplateN);
            }
        }

        private string MakeBold(string text)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            text = text.Replace("[", "<span class='tag'>[");
            text = text.Replace("]", "]</span>");

            text = text.Replace("{", "<span class='tagText'>{");
            text = text.Replace("}", "}</span>");

            return text;
        }

        protected void gvAttribute_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow )
            {
                Dal.ProductCatalogAttribute a = e.Row.DataItem as Dal.ProductCatalogAttribute;

                if (e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Normal)
                {
                    Label lblTemplateM = e.Row.FindControl("lblTemplateM") as Label;
                    Label lblTemplateF = e.Row.FindControl("lblTemplateF") as Label;
                    Label lblTemplateN = e.Row.FindControl("lblTemplateN") as Label;


                    lblTemplateF.Text = MakeBold(a.FriendlyDescriptionF);
                    lblTemplateM.Text = MakeBold(a.FriendlyDescriptionM);
                    lblTemplateN.Text = MakeBold(a.FriendlyDescriptionN);
                }

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                TextBox txbTemplateM = e.Row.FindControl("txbTemplateM") as TextBox;
                    TextBox txbTemplateF = e.Row.FindControl("txbTemplateF") as TextBox;
                    TextBox txbTemplateN = e.Row.FindControl("txbTemplateN") as TextBox;

                    txbTemplateF.Text = MakeBold(a.FriendlyDescriptionF);
                    txbTemplateM.Text = MakeBold(a.FriendlyDescriptionM);
                    txbTemplateN.Text = MakeBold(a.FriendlyDescriptionN);
                }
            }

        }

        protected void gvAttribute_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAttribute.EditIndex = -1;
            BindAttributes();
        }

        protected void gvAttribute_RowEditing(object sender, GridViewEditEventArgs e)
        {

            gvAttribute.EditIndex = e.NewEditIndex;
            BindAttributes();
        }

        private void BindAttributes()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogAttribute> attributes = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributes(AttributeGroupId);

            gvAttribute.DataSource = attributes;
            gvAttribute.DataBind();
        }

        protected void gvAttribute_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvAttribute.Rows[e.RowIndex];

            int attributeId = Convert.ToInt32(gvAttribute.DataKeys[e.RowIndex][0]);

            TextBox txbTemplateM = row.FindControl("txbTemplateM") as TextBox;
            TextBox txbTemplateF = row.FindControl("txbTemplateF") as TextBox;
            TextBox txbTemplateN = row.FindControl("txbTemplateN") as TextBox;

            Dal.ProductCatalogAttribute attribute = new Dal.ProductCatalogAttribute()
            {
                AttributeId = attributeId,
                FriendlyDescriptionF = txbTemplateF.Text.Trim(),
                FriendlyDescriptionM = txbTemplateM.Text.Trim(),
                FriendlyDescriptionN = txbTemplateN.Text.Trim(),
            };


            Dal.MixerHelper mh = new Dal.MixerHelper();

            mh.SetAttribute(attribute);

            DisplayMessage("Zapisano");

            gvAttribute.EditIndex = -1;
            BindAttributes();
        }
    }
}