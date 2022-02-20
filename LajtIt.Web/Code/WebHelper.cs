using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Web.Security;
using LajtIt.Bll;
using System.Threading.Tasks;

namespace LajtIt.Web 
{
    [AttributeUsage(AttributeTargets.All)]
    public class DeveloperAttribute : Attribute
    {
        // Private fields.
        private Guid guid;
        private bool required = true;

        // This constructor defines two required parameters: name and level.

        public DeveloperAttribute(string guid)
        {
            this.guid = new Guid(guid);
        }
        public DeveloperAttribute(string guid, bool required)
        {
            this.guid = new Guid(guid);
            this.required = required;
        }
         

        public virtual Guid GuidId
        {
            get { return guid; }
        }
        public virtual bool Required
        {
            get { return required; }
        }

    }

    public class CommonPage  
    {
        private Dal.AdminUser user;

      

        public   CommonPage()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                AdminUserHelper auh = new AdminUserHelper();
                Dal.AdminUser au = auh.GetUser(HttpContext.Current.User.Identity.Name);

                user = au;
            }
        }
        private static LajtIt.Bll.Helper.Cache<string, List<Dal.SystemUserRoleFnResult>> cache = 
            new LajtIt.Bll.Helper.Cache<string, List<Dal.SystemUserRoleFnResult>>(new TimeSpan(0,30,0));

        public bool HasAccess(string userName, params Dal.Helper.SystemRole[] acceptableRoles)
        {
            string key = String.Format("UserRoles{0}", userName);
            List<Dal.SystemUserRoleFnResult> value = cache.Get(key);
            if (value == null)
            {
                Dal.SettingsHelper sh = new Dal.SettingsHelper();
                Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

                List<Dal.SystemUserRoleFnResult> roles = sac.GetUserRoles(userName);

                cache.Set(key, roles);
                value = roles;
            }
            int[] roleIds = acceptableRoles.Select(x => (int)x).ToArray();
            value = value.Where(x => x.IsRoleAssigned == 1 && roleIds.Contains(x.RoleId)).ToList();


            return value.Count > 0;
        }
        public bool HasAccess(params Dal.Helper.SystemRole[] acceptableRoles)
        {
            return HasAccess(UserName, acceptableRoles);
        }
        public string UserName
        { 
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        } 

        public int UserCompanyId
        {
            get
            {

                if (user != null)
                {
                    if (user.CompanyId.HasValue)
                        return user.CompanyId.Value;
                    else
                        return 0;
                }
                return 0;
            }
        }
        public int UserShopId
        {
            get
            { 
                if (user != null)
                {
                    if (user.ShopId.HasValue)
                        return user.ShopId.Value;
                    else
                        return 0;
                }
                return 0;
            }
        }

    }

    public class LajtitMasterPage : MasterPage
    {
  

        public string UserName
        { 
            get
            {
                return (this.Page as LajtitPage).cm.UserName.ToLower();
            }
        }
        public int UserCompanyId
        {
            get
            {
                return (this.Page as LajtitPage).cm.UserCompanyId;
            }
        }
 
    }
    public class LajtitPage : Page
    {
        CommonPage _cm;
        public CommonPage cm
        {
            get
            {
                if (_cm == null)
                {
                    _cm = new CommonPage();
                }
                return _cm;

            }
        }


        public LajtitPage()
        {
            _cm = new CommonPage();
    
        }
        public Guid PageGuidId
        {
            get
            {
                MemberInfo m = this.GetType();
                DeveloperAttribute MyAttribute =
                    (DeveloperAttribute)Attribute.GetCustomAttribute(m, typeof(DeveloperAttribute));

                return MyAttribute.GuidId;
            }

        }

        public Decimal? GetIntOrNull(TextBox txb)
        {
            if (String.IsNullOrEmpty(txb.Text.Trim()))
                return null;

            return Decimal.Parse(txb.Text.Trim());
        }
        public int? GetIntOrNull(DropDownList ddl)
        {
            if (ddl.SelectedIndex == 0)
                return null;

            return Int32.Parse(ddl.SelectedValue);
        }
        public bool HasAccess(params Dal.Helper.SystemRole[] acceptableRoles)
        {
            return cm.HasAccess(acceptableRoles);

        }
        public bool HasAccess(string userName, params Dal.Helper.SystemRole[] acceptableRoles)
        {
            return cm.HasAccess(userName, acceptableRoles);

        }
        public string UserName
        {
            get
            {
                return cm.UserName.ToLower();
            }
        }
        public int UserCompanyId
        {
            get
            {
                return cm.UserCompanyId;
            }
        }
        public int UserShopId
        {

            get
            {
                return cm.UserShopId;
            }
        }
         
        public void Page_PreLoad(object sender, EventArgs e)
        {
        //    if (HasSession == false && Request.Url.ToString().Contains("Login.aspx") == false)
        //    {
  
        //        FormsAuthentication.SignOut();
        //        FormsAuthentication.RedirectToLoginPage();
        //    }
            MemberInfo m = this.GetType();
            GetAttribute(m);
        }
        public void GetAttribute(MemberInfo t)
        {
            // Get instance of the attribute.
            DeveloperAttribute MyAttribute =
                (DeveloperAttribute)Attribute.GetCustomAttribute(t, typeof(DeveloperAttribute));

            if (MyAttribute == null)
            {
                    Response.Redirect(String.Format("/NoAccess.aspx?err=NO_REG&g={1}", MyAttribute.GuidId));
            }
            else
            {
                if (MyAttribute.Required && !CheckAccess(MyAttribute.GuidId))
                    Response.Redirect(String.Format("/NoAccess.aspx?err=NO_ACCESS&g={0}", MyAttribute.GuidId));
                else
                {
                    MessageData messageData = new MessageData()
                    {
                        Id = MyAttribute.GuidId,
                        UserName = UserName,
                        Query = Request.Url.PathAndQuery
                    };
                    //Task.Factory.StartNew(LogUser, messageData);
                    Task.Factory.StartNew(() => LogUser(messageData));
                }

            }
        }
        public class MessageData
        {
            public string UserName { get; set; }
            public Guid Id { get; set; }
            public string Query { get; set; }
        }
        public static void LogUser(MessageData messageData)
        {
            Dal.ErrorHandler.UserLog(messageData.UserName, messageData.Id, messageData.Query, true);

        }
        private bool CheckAccess(Guid guidId)
        {
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();
            Dal.SystemPage page = sac.GetPageByAccess(guidId, UserName);

            return page != null;
        }
        public bool HasActionAccess( Guid actionGuidId)
        {
            Guid pageGuidId = this.PageGuidId;
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();
            List<int> rolesForAction = sac.GetPageActionRoles(pageGuidId, actionGuidId).Where(x => x.RoleAssigned.Value).Select(x=>x.RoleId).ToList();
            List<int> rolesForUser = sac.GetUserRoles(UserName).Select(x => x.RoleId).ToList();


            return rolesForAction.Any(x => rolesForUser.Contains(x));


        }

        public void DisplayMessage(string msg)
        {
            LajtitMasterPage1 mp = this.Master as LajtitMasterPage1;

            if (mp != null)
                mp.DisplayMessage(msg);

            //string prompt = "<script>$(document).ready(function(){{$.prompt('{0}');}});</script>";
            //string message = string.Format(prompt, msg.Replace('\r',' ').Replace('\n',' '));
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "message", message);

        }

        public void BindAllegroMyUsers(System.Web.UI.WebControls.ListControl list)
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            list.DataSource = asc.GetAllegroMyUsers();
            list.DataBind();
        }
        public void BindAllegroMyUsers(System.Web.UI.WebControls.GridView list)
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            list.DataSource = asc.GetAllegroMyUsers();
            list.DataBind();
        }
    }
    public class LajtitControl : UserControl
    {
      
   
   

        public string UserName
        { 
            get
            {
                return (this.Page as LajtitPage).cm.UserName.ToLower();
            }
        }
        public int UserCompanyId
        {
            get
            {
                return (this.Page as LajtitPage).cm.UserCompanyId;
            }
        }
        public int UserSourceTypeId
        {

            get
            {
                return (this.Page as LajtitPage).cm.UserShopId;
            }
        }

        public void DisplayMessage(string msg)
        {
       
            LajtitMasterPage1 mp = this.Page.Master as LajtitMasterPage1;

            mp.DisplayMessage(msg);

            //string prompt = "<script>$(document).ready(function(){{$.prompt('{0}');}});</script>";
            //string message = string.Format(prompt, msg);
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "message", message);

        }
        public bool HasActionAccess(Guid actionGuidId)
        {
            Guid pageGuidId = ((LajtitPage)this.Page).PageGuidId;
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();
            List<int> rolesForAction = sac.GetPageActionRoles(pageGuidId, actionGuidId).Where(x => x.RoleAssigned.Value).Select(x => x.RoleId).ToList();
            List<int> rolesForUser = sac.GetUserRoles(UserName).Select(x => x.RoleId).ToList();


            return rolesForAction.Any(x => rolesForUser.Contains(x));


        }
    } 
}