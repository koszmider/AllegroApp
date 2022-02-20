using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("04dce416-8561-4a2f-83e4-1ef17913405d")]
    public partial class ProductCatalogGroupFamily : LajtitPage
    {
        private int FamilyId
        {
            get
            {

                int familyId = 0;
                if (lbxFamilies.SelectedIndex != -1)
                    familyId = Convert.ToInt32(lbxFamilies.SelectedValue);

                return familyId;
            }
        }
        private int? SupplierId
        {
            get
            {

                int? supplierId = null;
                if (ddlSuppliers.SelectedIndex != 0)
                    supplierId = Convert.ToInt32(ddlSuppliers.SelectedValue);

                return supplierId;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindSuppliers();
                BindFamilies();
                BindFamilies();
                BindFamilyTypes();
                ddlSuppliers_SelectedIndexChanged(null, null);
            }
        }
        private void SetVisibility()
        {
            if(ddlSuppliers.SelectedIndex==0)
            {
                ddlFamilyTypeEdit.SelectedValue = "2";
            }
            else
            { 
                ddlFamilyTypeEdit.SelectedValue = "1";
            }

            pFamilyJoin.Visible = lbxFamilies.GetSelectedIndices().Count() > 1;
            pnGroupEdit.Visible = lbxGroups.GetSelectedIndices().Count() == 1;
            pnGroupAddMerge.Visible = lbxGroups.GetSelectedIndices().Count() >1;
            pFamilyAssign.Visible = lbxGroups.GetSelectedIndices().Count() > 0;
            ddlFamilyTypeEdit.Enabled = false;
            pnGroupAdd.Visible = lbxFamilies.GetSelectedIndices().Count() ==1;
        }

        private void BindFamilyTypes()
        {
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper(); 
            ddlFamilyTypeEdit.DataSource = pch.GetProductCatalogFamilyTypes();
            ddlFamilyTypeEdit.DataBind();
        }

        private void BindSuppliers()
        { 
            ddlSuppliers.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderBy(x => x.Name).ToList();
            ddlSuppliers.DataBind();
        }

        private void BindFamilies()
        {
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();
            lbxFamilies.DataSource = pch.GetProductCatalogFamilies(SupplierId).OrderBy(x => x.FamilyName).ToList();
            lbxFamilies.DataBind();
        }
        private void BindGroups()
        {
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();

            if (lbxFamilies.SelectedIndex != -1)
            {
                int familyId = Convert.ToInt32(lbxFamilies.SelectedValue);
                Dal.ProductCatalogGroupFamily f = new Dal.ProductCatalogGroupFamily() { FamilyId = familyId };
                lbxGroups.DataSource = pch.GetProductCatalogGroups(f).OrderBy(x => x.GroupName).ToList();
                lbxGroups.DataBind();
 
            ddlFamilyAssign.DataSource = pch.GetProductCatalogFamilies(SupplierId).Where(x=>x.FamilyId!=familyId).OrderBy(x=>x.FamilyName).ToList();
                ddlFamilyAssign.DataBind();

            }
        }

        protected void ddlSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbxFamilies.Items.Clear();
            lbxGroups.Items.Clear();
            BindFamilies();
            SetVisibility();
        }

        protected void lbxFamilies_SelectedIndexChanged(object sender, EventArgs e)
        { 
            lbxGroups.Items.Clear();
            BindGroups();


            if (lbxFamilies.GetSelectedIndices().Count() == 1)
            { 

                BindFamily();

            }
            btnFamily.Visible = true;
            btnFamilyAdd.Visible = false;


            SetVisibility();
        }

        private void BindFamily()
        {
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();

            Dal.ProductCatalogGroupFamily family = pch.GetProductCatalogFamily(FamilyId);

            if (family != null)
            {
                txbFamilyEdit.Text = family.FamilyName;

                ddlFamilyTypeEdit.SelectedValue = family.FamilyTypeId.ToString();
            }
            SetVisibility();
        }

        protected void btnGroupsMove_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();
            int[] groupIds = lbxGroups.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();
            int? familyId = null;

            if (txbFamily.Text.Trim() == "")
                familyId = Convert.ToInt32(ddlFamilyAssign.SelectedValue);
            else
            {
                Dal.ProductCatalogGroupFamily family = new Dal.ProductCatalogGroupFamily()
                {
                    FamilyName = txbFamily.Text.Trim(),
                    FamilyTypeId = Convert.ToInt32(ddlFamilyTypeEdit.SelectedValue),
                    SupplierId = SupplierId
                };
                var f = pch.SetProductCatalogFamily(family);

                familyId = f.FamilyId;
            }

            txbFamily.Text = "";

            pch.SetProductCatalogGroupsMove(familyId.Value, groupIds);

            DisplayMessage("Wybrane grupy zostały przeniesione");

            int fidx = lbxFamilies.SelectedIndex;
            BindFamilies();
            lbxFamilies.SelectedIndex = fidx;

            if (chbJump.Checked)
            {
                lbxFamilies.SelectedValue = familyId.ToString();
            }
            BindGroups();


            if (chbJump.Checked)
            {
                foreach (ListItem item in lbxGroups.Items)
                {
                    int gid = Convert.ToInt32(item.Value);
                    if (groupIds.Contains(gid))
                        item.Selected = true;

                }

            }
        }

        protected void lbxGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxGroups.GetSelectedIndices().Count() == 1)
            {
                Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper(); 

                int groupId = lbxGroups.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).FirstOrDefault();

                Dal.ProductCatalogGroup g = pch.GetProductCatalogGroup(groupId);

                txbGroupName.Text = g.GroupName;
                hlProductCatalog.NavigateUrl = String.Format("ProductCatalog.aspx?GroupId={0}", g.ProductCatalogGroupId);
            }

            if (lbxGroups.GetSelectedIndices().Count() > 1)
            {
                txbGroupMerge.Text = lbxGroups.Items.Cast<ListItem>().FirstOrDefault().Text;

            }

                SetVisibility();
        }

        protected void btnGroupsEdit_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();
            Dal.ProductCatalogGroup group = new Dal.ProductCatalogGroup()
            {
                FamilyId = FamilyId,
                GroupName = txbGroupName.Text.Trim(),
                SupplierId = SupplierId,
                ProductCatalogGroupId = lbxGroups.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).FirstOrDefault()
        };

            try
            {
                pch.SetProductCatalogGroup(group);
                BindGroups();
                lbxGroups.SelectedValue = group.ProductCatalogGroupId.ToString();
                DisplayMessage("Zmiany zapisane");
            }
            catch(Exception ex)
            {
                if(ex.Message.Contains("Cannot insert duplicate key row in object"))
                    DisplayMessage(String.Format("Błąd zapisu. Istnieje grupa o tej nazwie <b>{0}</b>", txbGroupName.Text));

                else
                DisplayMessage(String.Format("Błąd zapisu {0}", ex.Message));
            }

        }

        protected void btnFamily_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogGroupFamily family = new Dal.ProductCatalogGroupFamily()
            {
                FamilyTypeId = Convert.ToInt32(ddlFamilyTypeEdit.SelectedValue),
                FamilyId = FamilyId,
                FamilyName = txbFamilyEdit.Text.Trim(),
                SupplierId = SupplierId
            };

            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();

            pch.SetProductCatalogFamily(family);

            ddlSuppliers_SelectedIndexChanged(null, null);

            lbxFamilies.SelectedValue = family.FamilyId.ToString();

            lbxFamilies_SelectedIndexChanged(null, null);
        }
        protected void btnFamilyAdd_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogGroupFamily family = new Dal.ProductCatalogGroupFamily()
            {
                FamilyTypeId = Convert.ToInt32(ddlFamilyTypeEdit.SelectedValue),
                FamilyId = 0,
                FamilyName = txbFamilyEdit.Text.Trim(),
                SupplierId = SupplierId
            };

            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();

            pch.SetProductCatalogFamily(family);

            ddlSuppliers_SelectedIndexChanged(null, null);

            lbxFamilies.SelectedValue = family.FamilyId.ToString();

            lbxFamilies_SelectedIndexChanged(null, null);

            SetVisibility();
        }

        protected void lbtnFamilyNew_Click(object sender, EventArgs e)
        {
            ddlFamilyTypeEdit.Enabled = true;
            btnFamily.Visible = false;
            btnFamilyAdd.Visible = true;
        }

        protected void btnFamilyJoin_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();


            int[] familyIds = lbxFamilies.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();

            int familyId = pch.SetProductCatalogFamiliesJoin(familyIds);

            BindFamilies();
            lbxFamilies_SelectedIndexChanged(null, null);
            lbxFamilies.SelectedValue = familyId.ToString();

            BindGroups();
        }
 
        protected void btnGroupNew_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();
            Dal.ProductCatalogGroup group = new Dal.ProductCatalogGroup()
            {
                FamilyId = FamilyId,
                GroupName = txbGroupNew.Text.Trim(),
                SupplierId = SupplierId,
                ProductCatalogGroupId = 0
            };

            try
            {
                pch.SetProductCatalogGroup(group);
                BindGroups();
                lbxGroups.SelectedValue = group.ProductCatalogGroupId.ToString();
                DisplayMessage("Dodano nową grupę");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key row in object"))
                    DisplayMessage(String.Format("Błąd zapisu. Istnieje grupa o tej nazwie <b>{0}</b>", txbGroupNew.Text));

                else
                    DisplayMessage(String.Format("Błąd zapisu {0}", ex.Message));
            }


        }

        protected void btnGroupMerge_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();
            Dal.ProductCatalogGroup group = new Dal.ProductCatalogGroup()
            {
                FamilyId = FamilyId,
                GroupName = txbGroupMerge.Text.Trim(),
                SupplierId = SupplierId,
                ProductCatalogGroupId = 0
            };

            try
            {
                Dal.ProductCatalogGroup newGroup = pch.SetProductCatalogGroup(group);

                int[] groupIdsToMerge = lbxGroups.Items.Cast<ListItem>().Where(x => x.Selected && x.Text != "").Select(x => Int32.Parse(x.Value)).ToArray();

                newGroup = pch.SetProductCatalogGroup(group);

                pch.SetProductCatalogGroupMerge(newGroup, groupIdsToMerge);

                BindGroups();
                lbxGroups.SelectedValue = newGroup.ProductCatalogGroupId.ToString();
                DisplayMessage("Dodano nową grupę i połączone wybrane");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key row in object"))
                    DisplayMessage(String.Format("Błąd zapisu. Istnieje grupa o tej nazwie <b>{0}</b>", txbGroupNew.Text));

                else
                    DisplayMessage(String.Format("Błąd zapisu {0}", ex.Message));
            }
        }

        protected void btnFamilyFromGroups_Click(object sender, EventArgs e)
        {
            int[] groups = lbxGroups.Items.Cast<ListItem>().Where(x => x.Selected && x.Text != "").Select(x => Int32.Parse(x.Value)).ToArray();

            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();
            pch.SetProductCatalogFamilyFromGroup(groups);

            BindFamilies();
            lbxFamilies.SelectedIndex = 0;
            BindGroups();
        }
    }
}