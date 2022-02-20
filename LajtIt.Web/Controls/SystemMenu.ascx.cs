using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class SystemMenu : LajtitControl
    {
        bool filterByAccess = true;
        List<Dal.SystemMenuFnResult> menu;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void BindMenu()
        {

            if (Page.IsPostBack)
                return;

            Dal.AllegroScan asc = new Dal.AllegroScan();
            rpLinks.DataSource = asc.GetMyUsers();
            rpLinks.DataBind();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<ul id='menu' class='menu'>");
            sb.AppendLine(BindMenus());
            sb.AppendLine("</ul>");
            lblMenu.Text = sb.ToString();

        }

        private string BindMenus()
        {
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();

             menu = sac.GetSystemMenu(UserName);

            StringBuilder sb = new StringBuilder();
            sb.Append(BindMenu(null));

            return sb.ToString();

        }
        private string GetGroupPages(int groupId)
        {
            Dal.SystemAccessControl sac = new Dal.SystemAccessControl();
            var pages = menu.Where(x => x.GroupId == groupId).OrderBy(x=>x.OrderId).Select(x=> new { x.Url, x.PageName }) .Distinct().ToList();

            if (pages.Count == 0)
                return "<a href='#'>_</a>";

            StringBuilder sb = new StringBuilder();
             
            foreach (var page in pages)
            {
                sb.AppendLine(String.Format("<li><a href='{1}'>{0}</a></li>", page.PageName, page.Url));
            } 
            return sb.ToString();
        }
        private string BindMenu(int? parentGroupId)
        {
            StringBuilder sb = new StringBuilder();

            var groups = menu.Where(x => x.GroupParentId == parentGroupId)
                .Select(x=>new { x.GroupId, x.GroupName, x.GroupOrderId })
                .Distinct()
                .OrderBy(x => x.GroupOrderId)
                .ToList();

            if (groups.Count() == 0)
                return "";

            foreach (var m in groups)
            {
                string groupPages = GetGroupPages(m.GroupId);
                sb.AppendLine(String.Format("<li><a href='#'>{0}</a><ul class='menu'>{1}{2}</ul></li>", m.GroupName, groupPages, BindMenu( m.GroupId)));

            }
            return sb.ToString();
        }
        //    private string BindMenu2(int? parentGroupId)
        //{

        //    Dal.SystemAccessControl sac = new Dal.SystemAccessControl();


        //    List<Dal.SystemGroup> groups = sac.GetGroupsByAccess(filterByAccess, UserName, parentGroupId);

        //    if (groups.Count == 0)
        //        return "";

        //    StringBuilder sb = new StringBuilder();

        //    //sb.AppendLine("<li><ul>");
        //    foreach (Dal.SystemGroup group in groups)
        //    {
        //        string groupPages = GetGroupPages2(filterByAccess, UserName, group.GroupId);
        //        sb.AppendLine(String.Format("<li><a href='#'>{0}</a><ul class='menu'>{1}{2}</ul></li>", group.GroupName, groupPages, BindMenu(group.GroupId)));
        //    }
        //    //sb.AppendLine("</ul></li>");

        //    return sb.ToString();
        //}

        //private string GetGroupPages2(bool filterByAccess, string userName, int groupId)
        //{
        //    Dal.SystemAccessControl sac = new Dal.SystemAccessControl();
        //    List<Dal.SystemPage> pages = sac.GetPagesByAccess(filterByAccess, UserName, groupId);

        //    if (pages.Count == 0)
        //        return "<a href='#'>_</a>";

        //    StringBuilder sb = new StringBuilder();

        //    //sb.AppendLine("");
        //    foreach(Dal.SystemPage page in pages)
        //    {
        //        sb.AppendLine(String.Format("<li><a href='{1}'>{0}</a></li>", page.PageName, page.Url));
        //    }
        //    //sb.AppendLine("</ul>");
        //    return sb.ToString();
        //}

        protected void rpLinks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Dal.AllegroUser au = e.Item.DataItem as Dal.AllegroUser;

                HyperLink hlAllegro = e.Item.FindControl("hlAllegro") as HyperLink;
                hlAllegro.Text = au.UserName;
                hlAllegro.NavigateUrl = String.Format(hlAllegro.NavigateUrl, au.UserId);

            }
        }
    }
}