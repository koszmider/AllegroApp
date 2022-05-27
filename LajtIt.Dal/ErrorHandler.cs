using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Dal
{
    public static class ErrorHandler
    {
        public static List<ErrorLog> GetErrors()
        {
            using (LajtitHelperDB ctx = new LajtitHelperDB())
            {
                return ctx.ErrorLog.Where(x => x.IsSent == false).OrderBy(x => x.InsertDate).ToList() ;

            }
        }
        public static void LogError(Exception ex, string comment)
        {
            return;//koszmid
            using (LajtitHelperDB ctx = new LajtitHelperDB())
            {
                ErrorLog e = new ErrorLog();
                e.Comment = comment;
                e.InsertDate = DateTime.Now;
                e.Message = ex.Message;
                e.StackTrace = ex.StackTrace ?? "";
                e.IsSent = false;

                ctx.ErrorLog.InsertOnSubmit(e);
                ctx.SubmitChanges();

            }
        }
        public static void LogError(string comment)
        {

            using (LajtitHelperDB ctx = new LajtitHelperDB())
            {
                ErrorLog e = new ErrorLog();
                e.Comment = comment;
                e.InsertDate = DateTime.Now;
                e.Message = "";
                e.StackTrace = "";

                ctx.ErrorLog.InsertOnSubmit(e);
                ctx.SubmitChanges();

            }
        }
        public static void UserLog(string userName, Guid pageGuid, string pageName, bool hasAccess)
        {

            using (LajtitHelperDB ctx = new LajtitHelperDB())
            {
                UserLog ul = new Dal.UserLog()
                {
                    InsertDate = DateTime.Now,
                    InsertUser = userName,
                    PageGuid = pageGuid,
                    PageName = pageName.Substring(0, Math.Min(100, pageName.Length)),
                    HasAccess = hasAccess

                };
                ctx.UserLogs.InsertOnSubmit(ul);
                ctx.SubmitChanges();

            }
        }

        public static void SetErrorSent(int id)
        {
            using (LajtitHelperDB ctx = new LajtitHelperDB())
            {
                var e = ctx.ErrorLog.Where(x => x.Id == id).FirstOrDefault();
                e.IsSent = true;
                ctx.SubmitChanges();

            }
        }
    }
}
