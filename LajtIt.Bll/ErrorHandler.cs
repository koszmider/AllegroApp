using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Bll
{
    public class ErrorHandler
    {


        public static void LogError(Exception ex, string comment)
        {
            Dal.ErrorHandler.LogError(ex, comment);
        }
        public static void SendEmail( string comment)
        {
            EmailSender e = new EmailSender();
            e.SendEmail(new Dto.Email()
            {
                Body = String.Format(@"Env: {1}<br>Comment: {0}", comment, Dal.Helper.Env.ToString()),
                FromEmail = Dal.Helper.ErrorEmail,
                FromName = "Error log",
                Subject = "Email z aplikacji",
                ToEmail = Dal.Helper.ErrorEmail,
                ToName = ""
            }
            );
        }
        public static void SendEmail(string comment, string email)
        {
            EmailSender e = new EmailSender();
            e.SendEmail(new Dto.Email()
            {
                Body = String.Format(@"Env: {1}<br>Comment: {0}", comment, Dal.Helper.Env.ToString()),
                FromEmail = Dal.Helper.ErrorEmail,
                FromName = "Error log",
                Subject = "Email z aplikacji",
                ToEmail = email,
                ToName = ""
            }
            );
        }
        public static void SendError(Exception ex, string comment)
        {
            Dal.ErrorHandler.LogError(ex, comment);


            return;
            EmailSender e = new EmailSender();
            e.SendEmail(new Dto.Email()
            {
                Body = String.Format(@"Env: {3}<br>Comment: {0}<br>,
Exception: {1}<br>
StackTrace: {2}", comment, ex.Message, ex.StackTrace ?? "", Dal.Helper.Env.ToString()),
                FromEmail = Dal.Helper.ErrorEmail,
                FromName = "Error log",
                Subject = "Błąd w aplikacji",
                ToEmail = Dal.Helper.ErrorEmail,
                ToName = ""
            }
            );
        }

    }
}
