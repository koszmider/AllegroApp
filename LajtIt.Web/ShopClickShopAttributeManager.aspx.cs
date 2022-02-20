using LajtIt.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("be9da178-2a21-4df1-aef1-ebd7d94f95e2")] 
    public partial class ShopClickShopAttributeManager : LajtitPage
    {
        private Dal.Helper.Shop Shop
        {
            get
            {
                return (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), Int32.Parse(ddlShop.SelectedValue));
            }
        }

        List<Dal.ShopAttributeGroup> shopAttributeGroups;
        protected void Page_Load(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (!Page.IsPostBack)
                BindShops();
        }
        protected void ddlShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindAttributeGroups();
            if (ddlShop.Items[0].Value == "0")
                ddlShop.Items.RemoveAt(0);
        }
        protected void btnGetGroups_Click(object sender, EventArgs e)
        {
            BindAttributeGroups();

        }
        private void BindShops()
        { 

            ddlShop.DataSource = Dal.DbHelper.Shop.GetShops().Where(x => x.ShopType.ShopEngineTypeId == (int)Dal.Helper.ShopEngineType.Shoper).ToList();
            ddlShop.DataBind();
        }


        private void BindAttributeGroups()
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            shopAttributeGroups = sh.ShopAttributeGroups((int)Shop);

            List<Bll.ShopRestHelper.AttributeGroups.AttributeGroup> groups = Bll.ShopRestHelper.AttributeGroups.GetAttributeGroups(Shop);

           //Bll.ShopUpdateHelper.ClickShop cs = new Bll.ShopUpdateHelper.ClickShop();

            //AttributeGroup[] groups = cs.GetAttributeGroups(Shop);

            if(chbAssignedOnly.Checked)
            {
                int[] ids = sh.GetShopAttributeGroups(Shop).Select(x => x.ExternalShopAttributeGroupId).ToArray() ;

                groups = groups.Where(x => ids.Contains(x.attribute_group_id)).ToList();

            }

            gvAttributeGroups.DataSource = groups;
            gvAttributeGroups.DataBind();
        }

        protected void gvAttributeGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType== DataControlRowType.DataRow)
            {


                Bll.ShopRestHelper.AttributeGroups.AttributeGroup ag = e.Row.DataItem as Bll.ShopRestHelper.AttributeGroups.AttributeGroup;


                if (ag.attribute_group_id % 10 == 0)
                    e.Row.BackColor = System.Drawing.Color.Silver;


                Label lblExternalShopAttributeGroupId = e.Row.FindControl("lblExternalShopAttributeGroupId") as Label;
                Label lblExternalShopAttributeGroupName = e.Row.FindControl("lblExternalShopAttributeGroupName") as Label;
                CheckBox chbIsActive = e.Row.FindControl("chbIsActive") as CheckBox;
                CheckBox chbIsSearchable = e.Row.FindControl("chbIsSearchable") as CheckBox;

                lblExternalShopAttributeGroupId.Text = ag.attribute_group_id.ToString();
                chbIsActive.Checked = ag.active ==1;
                chbIsSearchable.Checked = ag.filters == 1;


                Dal.ShopAttributeGroup group = shopAttributeGroups.Where(x => x.ExternalShopAttributeGroupId == ag.attribute_group_id).FirstOrDefault();
                Dal.ProductCatalogShopAttribute pcsa = null;

              

                if (group != null)
                {

                    pcsa = Dal.DbHelper.ProductCatalog.GetProductCatalogShopAttributeByExternalAttributeGroupId(Shop, group.ExternalShopAttributeGroupId);

                }
                Label lblAttributeGroup = e.Row.FindControl("lblAttributeGroup") as Label;
                if (pcsa != null && pcsa.AttributeGroupId.HasValue)
                {
                    lblAttributeGroup.Text = pcsa.ProductCatalogAttributeGroup.Name;
                }

                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {

                    lblExternalShopAttributeGroupName.Text = ag.name;

                }
                else
                {
                    DropDownList ddlAttributeGroups = e.Row.FindControl("ddlAttributeGroups") as DropDownList;
                    TextBox txbExternalShopAttributeGroupName = e.Row.FindControl("txbExternalShopAttributeGroupName") as TextBox;
                     

                    ddlAttributeGroups.DataSource= Dal.DbHelper.ProductCatalog.GetProductCatalogAttributeGroupsForShop(Shop);
                    ddlAttributeGroups.DataBind();
                    ddlAttributeGroups.Items.Insert(0, new ListItem());

                    if (pcsa != null && pcsa.AttributeGroupId.HasValue)
                    {
                        ddlAttributeGroups.Visible = false;// SelectedIndex = ddlAttributeGroups.Items.IndexOf(ddlAttributeGroups.Items.FindByValue(pcsa.AttributeGroupId.ToString()));

                    }

                    txbExternalShopAttributeGroupName.Text = ag.name;
                }

            }
        }

        protected void gvAttributeGroups_RowEditing(object sender, GridViewEditEventArgs e)
        {

            gvAttributeGroups.EditIndex = e.NewEditIndex;
            BindAttributeGroups();
        }

        protected void gvAttributeGroups_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvAttributeGroups.Rows[e.RowIndex];

            int externalAttributeGroupId = Convert.ToInt32(gvAttributeGroups.DataKeys[e.RowIndex][0]);

            DropDownList ddlAttributeGroups = row.FindControl("ddlAttributeGroups") as DropDownList;
            CheckBox chbIsActive = row.FindControl("chbIsActive") as CheckBox;
            CheckBox chbIsSearchable = row.FindControl("chbIsSearchable") as CheckBox;

            //Bll.ShopUpdateHelper.ClickShop cs = new ShopUpdateHelper.ClickShop();

            if (ddlAttributeGroups.Visible)
            {
                if (ddlAttributeGroups.SelectedIndex != 0)
                {
                    int attributeGroupId = Int32.Parse(ddlAttributeGroups.SelectedValue);

                    Bll.ShopRestHelper.AttributeGroups.CreateAttributeGroupAndAssign(Shop, externalAttributeGroupId, attributeGroupId);

                    DisplayMessage("Utworzono i przypisano atrybut");
                }
            }
            else
            {
                bool isActive = chbIsActive.Checked;
                bool isSearchable = chbIsSearchable.Checked;
                ///TODO 
                ///
                //if (cs.UpdateAttributeGroup(Shop, pres_id, isActive, isSearchable))
                if (Bll.ShopRestHelper.AttributeGroups.UpdateAttributeGroup(Shop, externalAttributeGroupId, isActive, isSearchable))
                    DisplayMessage("Zaktualizowano");
                else
                    DisplayMessage("Błąd aktualizacji");

            }
            gvAttributeGroups.EditIndex = -1;
            BindAttributeGroups();
        }

        protected void gvAttributeGroups_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAttributeGroups.EditIndex = -1;
            BindAttributeGroups();

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Bll.ShopUpdateHelper.ClickShop cs = new ShopUpdateHelper.ClickShop();
            cs.CreateAttributeGroups(Shop, 100);

            DisplayMessage("Utworzono");

            BindAttributeGroups();
        }
    }
}