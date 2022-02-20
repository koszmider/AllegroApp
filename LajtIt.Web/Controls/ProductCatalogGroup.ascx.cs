using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class ProductCatalogGroupControl : LajtitControl
    {
        public int ProductCatalogId
        {
            set { ViewState["ProductCatalogId"] = value; }
            get { return Convert.ToInt32(ViewState["ProductCatalogId"]); }
        }
        public int SupplierId
        {
            set { ViewState["SupplierId"] = value; }
            get { return Convert.ToInt32(ViewState["SupplierId"]); }
        }

        public int ProductCatalogGroupId { get { return Convert.ToInt32(ddlProductCatalogGroup.SelectedValue); } }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void SetProductCatalogGroup(Dal.ProductCatalogGroup g)
        {
            ddlProductCatalogGroup.SelectedIndex
                    = ddlProductCatalogGroup.Items.IndexOf(ddlProductCatalogGroup.Items.FindByValue(g.ProductCatalogGroupId.ToString()));

            ddlProductCatalogFamily.SelectedIndex
                                    = ddlProductCatalogFamily.Items.IndexOf(ddlProductCatalogFamily.Items.FindByValue(g.FamilyId.ToString()));
        }

        internal void BindProductCatalogGroups(int supplierId)
        {
            BindProductCatalogGroups(0, supplierId);
        }
        public void BindProductCatalogGroups(int productCatalogId, int supplierId)
        {
            SupplierId = supplierId;
            ProductCatalogId = productCatalogId;

            Dal.ProductCatalogGroupHelper ph = new Dal.ProductCatalogGroupHelper();

            List<Dal.ProductCatalogGroupFamily> groupFamilies = ph.GetProductCatalogFamilies(supplierId).OrderBy(x => x.FamilyName).ToList();

            ddlProductCatalogFamily.DataSource = groupFamilies;
            ddlProductCatalogFamily.DataBind();

            if (productCatalogId != 0)
            {
                var g = ph.GetProductCatalogLine(productCatalogId);
                if (g != null)
                {
                    List<Dal.ProductCatalogGroup> groups = ph.GetProductCatalogGroups(supplierId,
                        Dal.Helper.ProductCatalogGroupFamilyType.Family)
                        .Where(x => x.FamilyId == g.FamilyId)
                        .OrderBy(x => x.GroupName)
                        .ToList();

                    ddlProductCatalogGroup.DataSource = groups;
                    ddlProductCatalogGroup.DataBind();
                    if (groups.Where(x => x.GroupName == "").Count() ==0)
                        ddlProductCatalogGroup.Items.Insert(0, new ListItem());


                    SetProductCatalogGroup(g);

                }
            }
        }

        protected void ddlProductCatalogFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dal.ProductCatalogGroupHelper ph = new Dal.ProductCatalogGroupHelper();
            Dal.ProductCatalogGroupFamily f = ph.GetProductCatalogFamily(Convert.ToInt32(ddlProductCatalogFamily.SelectedValue));

            BindProductCatalogGroups(f);
        }

        protected void lbtnProductCatalogGroupSave_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogGroup group = new Dal.ProductCatalogGroup()
            {
                GroupName = txbGroupName.Text.Trim(),
                SupplierId = SupplierId,
                FamilyId = Convert.ToInt32(ddlProductCatalogFamily.SelectedValue)

            };

            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();
            Dal.ProductCatalogGroup g = pch.SetProductCatalogGroup(group);

            var f = pch.GetProductCatalogFamily(group.FamilyId);
             
            SetProductCatalogGroupFamily(f);
            SetProductCatalogGroup(g);

            txbGroupName.Text = "";
            DisplayMessage("Nowa grupa została utworzona");

        }
        protected void lbtnProductCatalogFamilyGroupSave_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogGroupFamily family = new Dal.ProductCatalogGroupFamily()
            {
                FamilyName = txbFamilyName.Text.Trim(),
                FamilyTypeId = (int)Dal.Helper.ProductCatalogGroupFamilyType.Family,
                SupplierId = SupplierId

            };


            Dal.ProductCatalogGroupHelper ph = new Dal.ProductCatalogGroupHelper();

            var f = ph.SetProductCatalogFamily(family);


            BindProductCatalogGroups(ProductCatalogId, SupplierId);
            SetProductCatalogGroupFamily(f);


        }

        private void SetProductCatalogGroupFamily(Dal.ProductCatalogGroupFamily f)
        {

            ddlProductCatalogFamily.SelectedIndex
                                    = ddlProductCatalogFamily.Items.IndexOf(ddlProductCatalogFamily.Items.FindByValue(f.FamilyId.ToString()));

            BindProductCatalogGroups(f);
        }


        private void BindProductCatalogGroups(Dal.ProductCatalogGroupFamily f)
        {

            Dal.ProductCatalogGroupHelper ph = new Dal.ProductCatalogGroupHelper();

            List<Dal.ProductCatalogGroup> groups = ph.GetProductCatalogGroups(f)
                .OrderBy(x => x.GroupName)
                .ToList();

            if (groups.Count > 0)
            {
                ddlProductCatalogGroup.DataSource = groups;
                ddlProductCatalogGroup.DataBind();
                ddlProductCatalogGroup.Items.Insert(0, new ListItem());
            }
            else
                ddlProductCatalogGroup.Items.Clear();
        }
    }
}