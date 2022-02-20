using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("f0497a0c-4e4c-411a-b147-62ad650438e1")]
    public partial class SystemAccessControl : LajtitPage
    {
        private List<Dal.SystemPageRoleFnResult> roles;
        private Dal.SystemPage page;

        private int PageId
        {
            get
            {
                return Convert.ToInt32(ddlSystemPage.SelectedValue);
            }
        }
        private int GroupId
        {
            get
            {
                return Convert.ToInt32(ddlSystemGroup.SelectedValue);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                BindPages(ddlSystemPage);
                BindGroups(ddlSystemGroup);
            }
        }

        private void BindGroups(DropDownList ddl)
        {
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            List<Dal.SystemGroup> groups = sac.GetGroups();
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("--- nowa grupa nadrzędna -- ", "0"));
            ddl.DataSource = groups;
            ddl.DataBind();
        }

        private void BindPages(DropDownList ddl)
        {
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            ddl.DataSource = sac.GetPages();
            ddl.DataBind();
        }

        private void BindPagesForGroup(DropDownList ddl)
        {
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            ddl.DataSource = sac.GetPagesNotInGroup(GroupId).Where(x => x.CanUseInMenu).ToList();
            ddl.DataBind();
        }
        protected void btnPageSave_Click(object sender, EventArgs e)
        {
            Dal.SystemPage page = new Dal.SystemPage()
            {
                GuidId = Guid.Parse(txbGuidId.Text.Trim()),
                IsActive = chbIsActive.Checked,
                PageId = PageId,
                PageName = txbName.Text.Trim(),
                RequiresAuthentication = chbRequiresAuthentication.Checked,
                Url = txbUrl.Text.Trim(),
                CanUseInMenu= chbCanUseInMenu.Checked
            };

            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            int[] rolesIds = chblRoles.Items.Cast<ListItem>().Where(x => x.Selected).Select(x =>  Int32.Parse(x.Value)).ToArray();


            List<Dal.SystemPageActionRole> pageActionRoles = new List<Dal.SystemPageActionRole>();
            if (PageId != 0)
            {

                foreach(GridViewRow row in gvPageActions.Rows)
                {
                    Guid actionGuid = Guid.Parse(row.Cells[1].Text);

                    CheckBoxList chbPageActionRole = row.FindControl("chbPageActionRole") as CheckBoxList;

                    foreach(ListItem item in chbPageActionRole.Items)
                    {
                        if (item.Selected)
                            pageActionRoles.Add(new Dal.SystemPageActionRole()
                            {
                                InsertDate = DateTime.Now,
                                RoleId = Int32.Parse(item.Value),
                                PageActionId = Int32.Parse(gvPageActions.DataKeys[row.RowIndex][0].ToString())
                            });

                    }

                }

            }


            int pageId = sac.SetPage(page, rolesIds, pageActionRoles);


            switch (pageId)
            {
                case -1:
                    DisplayMessage("Strona o podanym identyfikatorze Guid istnieje");
                    return;

                case 0:
                    DisplayMessage("Strona  podanym adresie Url istnieje");
                    return;


            }



            BindPages(ddlSystemPage);
            ddlSystemPage.SelectedValue = pageId.ToString();

            DisplayMessage("Dane zostały zapisane");

            pnPage.Visible = false;
            BindPage();
        }

        protected void btnPageAddEdit_Click(object sender, EventArgs e)
        {
            BindPage();

        }

        private void BindPage()
        {
            pnPage.Visible = true;

            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            if (PageId == 0)
            {
                txbGuidId.Text = "";
                txbName.Text = "";
                txbUrl.Text = "";
                lbtnGroupDelete.Visible = false;
                txbPageAction.Enabled = lbtnPageAction.Enabled = false;
            }
            else
            {
                Dal.SystemPage page = sac.GetPages(PageId);

                lbtnGroupDelete.Visible = true;
                txbUrl.Text = page.Url;
                txbName.Text = page.PageName;
                txbGuidId.Text = page.GuidId.ToString();
                chbIsActive.Checked = page.IsActive;
                chbRequiresAuthentication.Checked = page.RequiresAuthentication;
                chbCanUseInMenu.Checked = page.CanUseInMenu;
                txbPageAction.Enabled = lbtnPageAction.Enabled = true;

            }
            roles = sac.GetPageRoles(PageId);
            chblRoles.DataSource = roles;
            chblRoles.DataBind();

            page = sac.GetPage(PageId);

            gvPageActions.DataSource = sac.GetPageActions(PageId);
            gvPageActions.DataBind();
        }

        protected void lbtnPageSaveCancel_Click(object sender, EventArgs e)
        {
            pnPage.Visible = false;
        }

        protected void chblRoles_DataBound(object sender, EventArgs e)
        {
            foreach(Dal.SystemPageRoleFnResult role in roles)
            {
                chblRoles.Items.FindByValue(role.RoleId.ToString()).Selected = role.IsRoleAssigned == 1;
            }
        }

        protected void btnGroupAddEdit_Click(object sender, EventArgs e)
        {
            BindGroup();
        }

        private void BindGroup()
        {
            pnGroup.Visible = true;

            BindGroups(ddlSystemGroupParent);
            BindPagesForGroup(ddlPages);
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            if (GroupId == 0)
            {
                txbGroupName.Text = "";
                txbGroupOrder.Text = "1";
                chbIsActiveGroup.Checked = true;
                rlPages.Visible = false;
                ddlPages.Enabled = false;
                lbtnPageAdd.Enabled = false;
            }
            else
            {
                Dal.SystemGroup group = sac.GetGroup(GroupId);
                rlPages.Visible = true;

                txbGroupName.Text = group.GroupName;
                txbGroupOrder.Text = group.OrderId.ToString();
                chbIsActiveGroup.Checked = group.IsActive;
                if (group.GroupParentId.HasValue)
                    ddlSystemGroupParent.SelectedValue = group.GroupParentId.ToString();

                ddlPages.Enabled = true;
                lbtnPageAdd.Enabled = true;
                rlPages.DataSource = sac.GetPageGroup(GroupId);
                rlPages.DataBind();

            }
        }

        protected void btnGroupSave_Click(object sender, EventArgs e)
        {
            Dal.SystemGroup group = new Dal.SystemGroup()
            {
                GroupName = txbGroupName.Text.Trim(),
                IsActive = chbIsActiveGroup.Checked,
                OrderId = Convert.ToInt32(txbGroupOrder.Text.Trim())

            };
            if (ddlSystemGroupParent.SelectedIndex != 0)
                group.GroupParentId = Convert.ToInt32(ddlSystemGroupParent.SelectedValue);


            if (GroupId != 0)
                group.GroupId = GroupId;

            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            sac.SetGroup(group);

            DisplayMessage("Zapisano zmiany");
            pnGroup.Visible = false;
            BindGroups(ddlSystemGroup);

        }

        protected void lbtnGroupSaveCancel_Click(object sender, EventArgs e)
        {
            pnGroup.Visible = false;

        } 

        protected void lbtnGroupDelete_Click(object sender, EventArgs e)
        {
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            sac.SetGroupDelete(GroupId);

            BindGroups(ddlSystemGroup);
            pnGroup.Visible = false;
            DisplayMessage("Grupa stron została usunięta");
        }

        protected void lbtnPageAdd_Click(object sender, EventArgs e)
        {
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();
            Dal.SystemPageGroup spg = new Dal.SystemPageGroup()
            {
                GroupId = GroupId,
                PageId = Convert.ToInt32(ddlPages.SelectedValue)
            };
            sac.SetPageGroup(spg);

            BindGroup();
        }

        protected void rlPages_ItemDataBound(object sender, AjaxControlToolkit.ReorderListItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink hlPage = e.Item.FindControl("hlPage") as HyperLink;
                Label lblPage = e.Item.FindControl("lblPage") as Label;

                Dal.SystemPage page = e.Item.DataItem as Dal.SystemPage;

                hlPage.NavigateUrl = page.Url;
                lblPage.Text = page.PageName; 
            }
        }

        protected void rlPages_ItemReorder(object sender, AjaxControlToolkit.ReorderListItemReorderEventArgs e)
        {
            var newOrder = e.NewIndex;
            var oldOrder = e.OldIndex;

            int pageId =   Int32.Parse(rlPages.DataKeys[e.OldIndex].ToString());

            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            sac.SetPageGroupMove(pageId, GroupId, newOrder, oldOrder);

            BindGroup();

        }

        protected void rlPages_DeleteCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {

            int pageId = Int32.Parse(e.CommandArgument.ToString());

            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            sac.SetPageGroupDelete(pageId, GroupId);

            BindGroup();
        }

        protected void lbtnPageAction_Click(object sender, EventArgs e)
        {
            Dal.SystemPageAction action = new Dal.SystemPageAction()
            {
                GuidId = Guid.NewGuid(),
                Name = txbPageAction.Text.Trim(),
                PageId = PageId
            };

            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();
            sac.SetPageAction(action);

            BindPage();
        }

        protected void gvPageActions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.SystemPageAction action = e.Row.DataItem as Dal.SystemPageAction;

                Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

                CheckBoxList chbPageActionRole = e.Row.FindControl("chbPageActionRole") as CheckBoxList;


                var roles = sac.GetPageActionRoles(page.GuidId, action.GuidId)
                    .Select(x => new ListItem()
                    {
                        Selected = x.RoleAssigned.Value,
                        Text = x.RoleName,
                        Value = x.RoleId.ToString()

                    }).ToArray();

                chbPageActionRole.Items.AddRange(roles);



            }
        }
    }
}