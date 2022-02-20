using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class SystemAccessControl
    {
        public List<SystemPage> GetPages()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemPage.ToList();
            }
        }

        public SystemPage GetPages(int pageId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemPage.Where(x => x.PageId == pageId).FirstOrDefault();
            }
        }

        public List<SystemGroup> GetGroupsByAccess(bool filterByAccess, string userName, int? parentGroupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                var groups = ctx.SystemGroup
                    .Where(x => (!parentGroupId.HasValue && x.GroupParentId == null) || (parentGroupId.HasValue && x.GroupParentId == parentGroupId))

                    .Distinct();


                if (filterByAccess)
                {
                    int[] userRoleIds = GetUserRoles(userName).Select(x => x.RoleId).ToArray();

                    int[] userPageIds = GetPagesByRoles(userRoleIds);

                    int[] groupsWithPages = ctx.SystemPageGroup.Where(x => userPageIds.Contains(x.PageId)).Select(x => x.GroupId).Distinct().ToArray();

                    groups = groups.Where(x => groupsWithPages.Contains(x.GroupId));
                }



                return groups
                    .OrderBy(x => x.OrderId).ToList();
            }
        }

        public List<SystemMenuFnResult> GetSystemMenu(string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemMenuFn(userName).ToList();
            }
            }

        public List<SystemPage> GetPagesByAccess(bool filterByAccess, string userName, int groupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                var pages = ctx.SystemPageGroup
                    .Where(x => x.GroupId == groupId)
                    .OrderBy(x => x.OrderId)
                    .Select(x => x.SystemPage);


                if (filterByAccess)
                {
                    int[] userRoleIds = GetUserRoles(userName).Select(x => x.RoleId).ToArray() ;

                    int[] userPageIds = GetPagesByRoles(userRoleIds);

                    pages = pages.Where(x => userPageIds.Contains(x.PageId));
                }

                return pages.ToList();
            }
        }

        public List<AdminUser> GetUsersFromRoles(List<Helper.SystemRole> userRoles)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] roleIds = userRoles.Select(x => (int)x).Distinct().ToArray();

                return ctx.SystemUserRole.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.AdminUser).Distinct().ToList();
            }
        }

        private int[] GetPagesByRoles(int[] roleIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemPageRole.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.PageId).Distinct().ToArray();
            }
        }

        public int SetPage(SystemPage page, int[] rolesIds, List<Dal.SystemPageActionRole> pageActionRoles)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (page.PageId == 0)
                {
                    if (ctx.SystemPage.Where(x => x.GuidId == page.GuidId).Count() > 0)
                        return -1;
                    if (ctx.SystemPage.Where(x => x.Url == page.Url).Count() > 0)
                        return 0;

                    ctx.SystemPage.InsertOnSubmit(page);
                    ctx.SubmitChanges();
                }
                else
                {
                    if (ctx.SystemPage.Where(x => x.GuidId == page.GuidId && x.PageId != page.PageId).Count() > 0)
                        return -1;
                    if (ctx.SystemPage.Where(x => x.Url == page.Url && x.PageId != page.PageId).Count() > 0)
                        return 0;

                    SystemPage pageToUpdate = ctx.SystemPage.Where(x => x.PageId == page.PageId).FirstOrDefault();

                    pageToUpdate.GuidId = page.GuidId;
                    pageToUpdate.IsActive = page.IsActive;
                    pageToUpdate.PageName = page.PageName;
                    pageToUpdate.RequiresAuthentication = page.RequiresAuthentication;
                    pageToUpdate.Url = page.Url;
                    pageToUpdate.CanUseInMenu = page.CanUseInMenu;

                    var actionRolesToDelete = ctx.SystemPageActionRole.Where(x => x.SystemPageAction.PageId == page.PageId).ToList();
                    ctx.SystemPageActionRole.DeleteAllOnSubmit(actionRolesToDelete);
                    ctx.SystemPageActionRole.InsertAllOnSubmit(pageActionRoles);
                    ctx.SubmitChanges();

                }

                List<SystemPageRole> rolesToDelete = ctx.SystemPageRole.Where(x => x.PageId == page.PageId).ToList();
                ctx.SystemPageRole.DeleteAllOnSubmit(rolesToDelete);
                ctx.SystemPageRole.InsertAllOnSubmit(
                    rolesIds.Select(x => new SystemPageRole()
                    {
                        InsertDate = DateTime.Now,
                        PageId = page.PageId,
                        RoleId = x

                    }).ToList()
                    );
                ctx.SubmitChanges();
                return page.PageId;
            }
        }

        public SystemPage GetPage(int pageId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemPage.Where(x => x.PageId == pageId).FirstOrDefault();
            }
        }

        public List<SystemPageAction> GetPageActions(int pageId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemPageAction.Where(x => x.PageId == pageId).ToList();
            }
        }

        public List<SystemPageActionRoleFnResult> GetPageActionRoles(Guid pageGuid, Guid pageActionGuid)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemPageActionRoleFn(pageGuid, pageActionGuid).ToList() ;
            }
        }
        public List<SystemGroup> GetGroups()
        {
            List<Dal.SystemGroup> groups = new List<SystemGroup>();

            groups = GetGroups(null, 0);

            return groups;

        }

        public SystemPage GetPageByAccess(Guid guidId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] roleIds = GetUserRoles(userName).Where(x => x.IsRoleAssigned == 1).Select(x => x.RoleId).Distinct().ToArray();
                int[] pagesIds = ctx.SystemPageRole.Where(x => 
                roleIds.Contains(x.RoleId)
                || x.SystemPage.RequiresAuthentication == false)          
                .Select(x => x.PageId).Distinct().ToArray();
                return ctx.SystemPage
                    .Where(x => x.GuidId == guidId && pagesIds.Contains(x.PageId))
                    .FirstOrDefault();                

            }
        }

        public List<SystemGroup> GetGroups(int? parentGroupId, int level)
        {
            List<Dal.SystemGroup> groups = new List<SystemGroup>();
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.SystemGroup> parentGroups =
                    ctx.SystemGroup.Where(x => (x.GroupParentId.HasValue && x.GroupParentId == parentGroupId) || (!x.GroupParentId.HasValue && !parentGroupId.HasValue))
                    .OrderBy(x => x.OrderId)
                    .ToList();


                foreach (Dal.SystemGroup group in parentGroups)
                {
                    string pattern = "{0} {1}";
                    if (!group.IsActive)
                        pattern = "{0} ({1})";
                    groups.Add(new SystemGroup()
                    {
                        GroupId = group.GroupId,
                        GroupName = String.Format(pattern, GetSeparator(level), group.GroupName)
                    });
                    groups.AddRange(GetGroups(group.GroupId, level + 1));

                }
            }

            return groups;
        }

        private string GetSeparator(int level)
        {
            string s = "";
            for (int i = 0; i < level; i++)
                s += "-";

            return s;
        }

        public List<SystemRole> GetRoles()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemRole.ToList();
            }
        }

        public List<SystemUserRoleFnResult> GetUserRoles(string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int userId = ctx.AdminUser.Where(x => userName.ToLower() == x.UserName.ToLower()).Select(x => x.UserId).FirstOrDefault();

                return GetUserRoles(userId).Where(x => x.IsRoleAssigned == 1).ToList();
            }
        }
        public List<SystemUserRoleFnResult> GetUserRoles(int userId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemUserRoleFn(userId).ToList();
            }
        }

        public SystemGroup GetGroup(int groupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemGroup.Where(x => x.GroupId == groupId).FirstOrDefault();
            }
        }

        public AdminUser GetUser(int userId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.AdminUser.Where(x => x.UserId == userId).FirstOrDefault();
            }
        }

        public int SetUser(AdminUser user, int[] rolesIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (user.UserId == 0)
                {
                    if (ctx.AdminUser.Where(x => x.Email == user.Email).Count() > 0)
                        return -1;
                    if (ctx.AdminUser.Where(x => x.UserName == user.UserName).Count() > 0)
                        return 0;

                    ctx.AdminUser.InsertOnSubmit(user);
                    ctx.SubmitChanges();
                }
                else
                {
                    if (ctx.AdminUser.Where(x => x.Email == user.Email && x.UserId != user.UserId).Count() > 0)
                        return -1;
                    if (ctx.AdminUser.Where(x => x.UserName == user.UserName && x.UserId != user.UserId).Count() > 0)
                        return 0;

                    AdminUser userToUpdate = ctx.AdminUser.Where(x => x.UserId == user.UserId).FirstOrDefault();

                    userToUpdate.Commision = user.Commision;
                    userToUpdate.Email = user.Email;
                    userToUpdate.UserName = user.UserName;
                    userToUpdate.IsActive = user.IsActive;
                    if (user.Pwd != null)
                        userToUpdate.Pwd = user.Pwd;

                    ctx.SubmitChanges();

                }

                List<SystemUserRole> rolesToDelete = ctx.SystemUserRole.Where(x => x.UserId == user.UserId).ToList();
                ctx.SystemUserRole.DeleteAllOnSubmit(rolesToDelete);
                ctx.SystemUserRole.InsertAllOnSubmit(
                    rolesIds.Select(x => new SystemUserRole()
                    {
                        InsertDate = DateTime.Now,
                        UserId = user.UserId,
                        RoleId = x

                    }).ToList()
                    );
                ctx.SubmitChanges();
                return user.UserId;
            }
        }

        public void SetPageAction(SystemPageAction action)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.SystemPageAction.InsertOnSubmit(action);
                ctx.SubmitChanges();
            }
        }

        public List<SystemPage> GetPageGroup(int groupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemPageGroup.Where(x => x.GroupId == groupId).OrderBy(x => x.OrderId).Select(x => x.SystemPage).ToList();
            }
        }
 
        public List<SystemPage> GetPagesNotInGroup(int groupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] pageIdsInGroup = ctx.SystemPageGroup.Where(x => x.GroupId == groupId).Select(x => x.PageId).ToArray();
                return ctx.SystemPage.Where(x => !pageIdsInGroup.Contains(x.PageId)).ToList();
            }
        }
    
        public void SetPageGroup(SystemPageGroup spg)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                SystemPageGroup p = ctx.SystemPageGroup.Where(x => x.GroupId == spg.GroupId)
                    .OrderByDescending(x => x.OrderId).Take(1).FirstOrDefault();

                int orderId = 0;
                if (p != null)
                    orderId = p.OrderId + 1;
                spg.OrderId = orderId;
                ctx.SystemPageGroup.InsertOnSubmit(spg);
                ctx.SubmitChanges();
            }
        }
        public void SetGroup(SystemGroup group)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (group.GroupId == 0)
                {
                    ctx.SystemGroup.InsertOnSubmit(group);
                }
                else
                {
                    SystemGroup groupToUpdate = ctx.SystemGroup.Where(x => x.GroupId == group.GroupId).FirstOrDefault();

                    groupToUpdate.GroupName = group.GroupName;
                    groupToUpdate.GroupParentId = group.GroupParentId;
                    groupToUpdate.IsActive = group.IsActive;
                    groupToUpdate.OrderId = group.OrderId;


                }
                ctx.SubmitChanges();
            }
        }

        public void SetPageGroupMove(int pageId, int groupId, int newOrder, int oldOrder)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                SystemPageGroup smp = ctx.SystemPageGroup.Where(x => x.PageId == pageId && x.GroupId == groupId && x.OrderId== oldOrder).FirstOrDefault();
                if (newOrder > oldOrder)
                {
                    List<SystemPageGroup> pages = ctx.SystemPageGroup
                        .Where(x => x.GroupId == groupId && x.OrderId <= newOrder && x.OrderId> oldOrder).ToList();
                    foreach (SystemPageGroup p in pages)
                        p.OrderId -= 1;


                }
                else
                {
                    List<SystemPageGroup> pages = ctx.SystemPageGroup
                        .Where(x => x.GroupId == groupId && x.OrderId<= oldOrder && x.OrderId >= newOrder).ToList();
                    foreach (SystemPageGroup p in pages)
                        p.OrderId += 1;


                }
                smp.OrderId = newOrder;
                ctx.SubmitChanges();

            }
        }

        public void SetPageGroupDelete(int pageId, int groupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.SystemPageGroup.DeleteOnSubmit(ctx.SystemPageGroup.Where(x => x.PageId == pageId && x.GroupId == groupId).FirstOrDefault());
                ctx.SubmitChanges();

                int id = 0;
                foreach (SystemPageGroup spg in ctx.SystemPageGroup.Where(x => x.GroupId == groupId).OrderBy(x => x.OrderId).ToList())
                    spg.OrderId = id++;

                ctx.SubmitChanges();

            }
        }

        public void SetGroupDelete(int groupId)
        { 
                GetGroupChild(groupId);
 
          
        }

        public List<SystemGroup> GetGroupChild(int groupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<SystemGroup> groups = ctx.SystemGroup.Where(x => x.GroupParentId == groupId).ToList();

                if (groups.Count == 0)
                {
                    ctx.SystemGroup.DeleteOnSubmit(ctx.SystemGroup.Where(x => x.GroupId == groupId).FirstOrDefault());
                    ctx.SubmitChanges();
                }
                else
                {
                    foreach (SystemGroup group in groups)
                    {
                        GetGroupChild(group.GroupId);
                        ctx.SystemGroup.DeleteOnSubmit(ctx.SystemGroup.Where(x => x.GroupId == groupId).FirstOrDefault());
                        ctx.SubmitChanges();

                    } 
                }
                return groups;
            }
        }
        public List<AdminUser> GetUsersForRole(int roleId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<SystemUserRole>(x => x.AdminUser);

                ctx.LoadOptions = dlo;
                return ctx.SystemUserRole.Where(x => x.RoleId == roleId).Select(x=>x.AdminUser).ToList();
            }
        }

        public List<AdminUser> GetUsers()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.AdminUser.ToList();
            }
        }

        public List<SystemPageRoleFnResult> GetPageRoles(int pageId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SystemPageRoleFn(pageId).ToList();
            }
        }
    }
}
