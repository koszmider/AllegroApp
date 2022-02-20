using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("67ecd38a-41be-4bfc-9de9-ee38da45337b")]
    public partial class SystemAccessControlUser : LajtitPage
    {
        private List<Dal.SystemUserRoleFnResult> roles;

        private int UserId
        {
            get
            {
                return Convert.ToInt32(ddlAdminUser.SelectedValue);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                BindUsers();
                BindRoles();

               chbIsActive.Enabled= ! HasAccess( ddlAdminUser.SelectedItem.Text, new Dal.Helper.SystemRole[] { Dal.Helper.SystemRole.Admin });
            }
            
        }

        private void BindRoles()
        {

            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();
            gvRoles.DataSource = sac.GetRoles();
            gvRoles.DataBind();
        }

        private void BindUsers()
        {
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            ddlAdminUser.DataSource = sac.GetUsers();
            ddlAdminUser.DataBind();
        }

        protected void btnUserSave_Click(object sender, EventArgs e)
        {
            Dal.AdminUser user = new Dal.AdminUser()
            {
                Commision = Convert.ToDecimal(txbCommision.Text.Trim()),
                CompanyId = null,
                Email = txbEmail.Text.Trim(),
                UserName = txbUserName.Text.Trim(),
                UserId=UserId,
                IsActive= chbIsActive.Checked
            };

            if(!String.IsNullOrEmpty(txbPassword.Text.Trim()))
            {
                user.Pwd = Bll.AdminUserHelper.GetMD5HashData(txbPassword.Text.Trim());
            }


            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            int[] rolesIds = chblRoles.Items.Cast<ListItem>().Where(x => x.Selected).Select(x =>  Int32.Parse(x.Value)).ToArray();


            int userId = sac.SetUser(user, rolesIds);

            switch(userId)
            {
                case -1:
                    DisplayMessage("Użytkownik o podanym adresie email istnieje");
                    return;

                case 0:
                    DisplayMessage("Użytkownik o podanej nazwie istnieje");
                    return;


            }

            BindUsers();
            BindRoles();
            ddlAdminUser.SelectedValue = userId.ToString();

            DisplayMessage("Dane zostały zapisane");

            pnUser.Visible = false;
        }

        protected void btnUserAddEdit_Click(object sender, EventArgs e)
        {
            pnUser.Visible = true;

            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

            if(UserId==0)
            {
                

            }
            else
            {
                Dal.AdminUser user= sac.GetUser(UserId);

                txbCommision.Text = String.Format("{0:0.00}", user.Commision);
                txbEmail.Text = user.Email;
                txbUserName.Text = user.UserName;
                chbIsActive.Checked = user.IsActive;


            }
            roles = sac.GetUserRoles(UserId);
            chblRoles.DataSource = roles;
            chblRoles.DataBind();

            chbIsActive.Enabled = !HasAccess(ddlAdminUser.SelectedItem.Text, new Dal.Helper.SystemRole[] { Dal.Helper.SystemRole.Admin });
        }

        protected void lbtnUserSaveCancel_Click(object sender, EventArgs e)
        {
            pnUser.Visible = false;
        }

        protected void chblRoles_DataBound(object sender, EventArgs e)
        {
            foreach(Dal.SystemUserRoleFnResult role in roles)
            {
                chblRoles.Items.FindByValue(role.RoleId.ToString()).Selected = role.IsRoleAssigned == 1;
            }
        }

        protected void gvRoles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.SystemRole role = e.Row.DataItem as Dal.SystemRole;


                Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

                List<Dal.AdminUser> users = sac.GetUsersForRole(role.RoleId);

                GridView gvUsers = e.Row.FindControl("gvUsers") as GridView;
                Label lblRole = e.Row.FindControl("lblRole") as Label;

                lblRole.Text = role.RoleName;

                gvUsers.DataSource = users;
                gvUsers.DataBind();

            }
        }
    }
}