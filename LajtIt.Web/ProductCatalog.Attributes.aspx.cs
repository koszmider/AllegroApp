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
    [Developer("6b3149a6-7aac-452f-aed3-bf4ec1202ad8")]
    public partial class ProductAttributes : LajtitPage
    { 
        private int AttributeGroupId
        {
            get
            {
                return Convert.ToInt32(ddlAttributes.SelectedValue);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!Page.IsPostBack)
            {
                BindAttributeGroups();
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            Dal.ProductCatalogAttributeGroup group = new Dal.ProductCatalogAttributeGroup();

            if (ViewState["mode"].ToString() == "new" && pch.GetProductCatalogAttributeGroup(txbGroupCode.Text.Trim()) !=null)

            {
                DisplayMessage(String.Format("Istnieje grupa posiadająca kod {0}. Wprowadź inną nazwę.", txbGroupCode.Text));
                return;
            }





                if (ViewState["mode"].ToString() == "edit")
                group.AttributeGroupId = AttributeGroupId;
            group.ExportToShop = chbExportToShop.Checked;
            group.AttributeGroupTypeId = Convert.ToInt32(ddlAttributeGroupType.SelectedValue);
            group.Name = txbName.Text;
            group.GroupCode = txbGroupCode.Text.Trim();
            group.AllegroOrder = Convert.ToInt32(txbAllegroOrder.Text.Trim());



            if (ViewState["mode"].ToString() == "edit")
                pch.SetProductCatalogAttributeUpdate(group);
            else
            { 
                int attributeGroupId = pch.SetProductCatalogAttributeGroup(group);
                BindAttributeGroups();
                ddlAttributes.SelectedValue = attributeGroupId.ToString();
            }
            BindAttributeGroup();
            DisplayMessage("Zapisano");
        }



        protected void gvAttributes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogAttribute attribute = e.Row.DataItem as Dal.ProductCatalogAttribute;

                HyperLink hlName = e.Row.FindControl("hlName") as HyperLink;

                hlName.Text = attribute.Name;
                hlName.NavigateUrl = String.Format(hlName.NavigateUrl, attribute.AttributeId);

            }
        }



        private void BindAttributeGroups()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            ddlAttributes.DataSource = pch.GetProductCatalogAttributeGroups()
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    Name = String.Format("{0} ({1})", x.Name, x.AttributeGroupId),
                    AttributeGroupId = x.AttributeGroupId
                }
                )
                .ToList();
            ddlAttributes.DataBind();
            //ScriptManager.RegisterStartupScript(upAttributes, upAttributes.GetType(), "k", "categories();", true);
            ddlAttributeGroupType.DataSource = pch.GetProductCatalogAttributeGroupTypes();
            ddlAttributeGroupType.DataBind();

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["idg"]))
                {
                    ddlAttributes.SelectedValue = Request.QueryString["idg"];
                    ddlAttributes_SelectedIndexChanged(null, null);
                }

            }
        }
        private void BindAttributeGroup()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ProductCatalogAttributeGroup group = pch.GetProductCatalogAttributeGroup(AttributeGroupId);

            pnAttributeGroup.Visible = true;
            ViewState["mode"] = "edit";
            txbName.Text = group.Name;
            ddlAttributeGroupType.Enabled = false;
        

            chbExportToShop.Checked = group.ExportToShop;
            ddlAttributeGroupType.SelectedValue = group.AttributeGroupTypeId.ToString();
            
            txbAllegroOrder.Text = group.AllegroOrder.ToString();
            txbGroupCode.Text = group.GroupCode;
            lbtnAttributeNew.Visible = true;

            if (!String.IsNullOrEmpty(txbGroupCode.Text))
                txbGroupCode.Enabled = false;
            else
                txbGroupCode.Enabled = true;

            gvAttributes.DataSource = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributes(AttributeGroupId).OrderByDescending(x => x.SortOrder).ToList();
            gvAttributes.DataBind();
        }
    

        protected void ddlAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlAttributes.SelectedIndex==0)
            {
                pnAttributeGroup.Visible = false;
                gvAttributes.DataSource = null;
                gvAttributes.DataBind();

                return;
            }

            BindAttributeGroup();
        }

        protected void lbtnAttributeGroupNew_Click(object sender, EventArgs e)
        {
            ViewState["mode"] = "new";
            lbtnAttributeNew.Visible = false;
            txbAllegroOrder.Text = "";
            txbGroupCode.Text = "";
            txbName.Text = "";
            chbExportToShop.Checked = false;
            ddlAttributeGroupType.SelectedIndex = 0;
            ddlAttributeGroupType.Enabled = true;
            txbGroupCode.Enabled = true;
            pnAttributeGroup.Visible = true;
            ddlAttributes.SelectedIndex = 0;
            gvAttributes.DataSource = null;
            gvAttributes.DataBind();
        }

        protected void btnAttributeAdd_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogAttribute att = new ProductCatalogAttribute()
            {
                AttributeGroupId = AttributeGroupId,
                Name = txbAttributeName.Text.Trim(),
                SortOrder = 10,
                UpdateShopConfiguration = false
            };


            int attributeId = 0;
            bool result = Dal.DbHelper.ProductCatalog.SetProductCatalogAttribute(att, ref attributeId);


            if(!result)
            {
                DisplayMessage("Atrybut o takiej nazwie już istnieje");
                return;
            }
            Response.Redirect(String.Format("/ProductCatalog.Attribute.aspx?id={0}", attributeId));

        }

        protected void lbtnAttributeNew_Click(object sender, EventArgs e)
        {
            if (ddlAttributes.SelectedIndex != 0)
                mpeAttribute.Show();
        }
    }
}