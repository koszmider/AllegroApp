using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Dal
{
    public class AdminUserHelper
    {



        public Dal.AdminUser IsAuthenticated(string userName, string password)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.AdminUser au = ctx.AdminUser.Where(x => x.UserName == userName && x.Pwd == password).FirstOrDefault();

                
                return au;
            }
        }  
        public bool ChangePassword(string userName, string passwordOld, string passwordNew)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                AdminUser au = ctx.AdminUser.Where(x => x.UserName == userName && x.Pwd == passwordOld).FirstOrDefault();
                if (au == null)
                    return false;

                au.Pwd = passwordNew;
                ctx.SubmitChanges();
                return true;

            }
        }

        public List<AdminUserStatByMonthResult> GetAdminUserStats(string userName, DateTime month)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.AdminUserStatByMonth(month, userName).OrderBy(x=>x.Day).ToList();
            }
        }

        public AdminUser GetUser(string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.AdminUser.Where(x => x.UserName == userName).FirstOrDefault();
            }
        }

        public List<AdminUser> GetUsers()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.AdminUser.Where(x => x.IsActive).OrderBy(x => x.UserName).ToList();
            }
        }
        public List<AdminUser> GetActiveUsers()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.AdminUser.OrderBy(x => x.UserName).ToList();
            }
        }
    }

}
