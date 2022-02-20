using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace LajtIt.Bll
{
    public class EmailSender
    {

        public void SendEmail(Bll.Dto.Email email)
        {
            SendEmail(email,
                System.Configuration.ConfigurationManager.AppSettings["SMTP_Server"],
                Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTP_Port"]));
        }
        public void SendEmail(string subject, string body, string toEmail)
        {
            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            //Dal.Settings s = sh.GetSetting("SMTP_FROM");

            Bll.Dto.Email email = new Bll.Dto.Email()
            {
                Body = body,
                FromName = sh.GetSetting("SMTP_FROM").StringValue,
                FromEmail = System.Configuration.ConfigurationManager.AppSettings["SMTP_User"],
                Subject = subject,
                ToEmail = toEmail,
                ToName = "Lajtit"

            };
            SendEmail(email,
                System.Configuration.ConfigurationManager.AppSettings["SMTP_Server"],
                Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTP_Port"]));
        }

        public void SendEmail(Bll.Dto.Email email, string server, int port)
        {
            try
            {
                string supportEmail =   Dal.Helper.MyEmail;
           
                Dal.SettingsHelper sh = new Dal.SettingsHelper();
                Dal.Settings s = sh.GetSetting("SMTP_FROM");

                using (MailMessage message = new MailMessage())
                {
                    message.Subject = email.Subject;
                    message.Body = email.Body;

                    if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Dev)// || email.ToEmail == null)
                    {
                        message.To.Add(new MailAddress(Dal.Helper.DevEmail, email.ToName));
                    }
                    else
                        if(email.ToEmail!=null)
                        message.To.Add(new MailAddress(email.ToEmail, email.ToName));

                    if (email.ToEmail==null && email.ToRoles!=null)
                    {
                        Dal.SystemAccessControl sac = new Dal.SystemAccessControl();
                        List<Dal.AdminUser> users = sac.GetUsersFromRoles(email.ToRoles);

                        foreach(Dal.AdminUser au in users)
                        {
                            message.To.Add(new MailAddress(au.Email, au.UserName));
                        }

                    }
                    

                        if (email.ToEmail!=null && !email.ToEmail.EndsWith("lajtit.pl"))
                        message.Bcc.Add(new MailAddress(supportEmail));


                    message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["SMTP_User"],
                        s.StringValue);
                    message.ReplyToList.Add(new MailAddress("kontakt@lajtit.pl", email.FromName));
                    message.IsBodyHtml = true;


                    if (email.AttachmentFile != null)
                    {
                        Attachment att = new Attachment(email.AttachmentFile);
                        message.Attachments.Add(att);
                    }

                    using (SmtpClient client = new SmtpClient(server, port))
                    {
                        client.UseDefaultCredentials = false;

                        //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                      
                        client.EnableSsl =  Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SMTP_Ssl"]);
                        client.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SMTP_User"],
                            System.Configuration.ConfigurationManager.AppSettings["SMTP_Pwd"]);
                        client.ServicePoint.MaxIdleTime = 1; /* without this the connection is idle too long and not terminated, times out at the server and gives sequencing errors */
                        client.Timeout = 10000; //10 sek
                        client.Send(message);
                    }
                }
            }
            catch (SmtpException ex)
            {
                ErrorHandler.LogError(ex, String.Format("{0} {1}", email.ToEmail, email.Body));
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError(ex, String.Format("{0} {1}", email.ToEmail, email.Body));
            }

        }
        public static void SendErrors()
        {
            List<Dal.ErrorLog> errors = Dal.ErrorHandler.GetErrors();

            if (errors.Count == 0)
                return;



            StringBuilder sb = new StringBuilder();


            sb.Append("<style> table tr td {padding:2px;}</style><table>");

            foreach(Dal.ErrorLog error in errors)
            {
                sb.AppendLine(String.Format("<tr style='background-color:black; color:white;'><td>{0:yyyy/MM/dd hh:mm:ss}</td><td>{1}</td></tr>", error.InsertDate, error.Comment));
                sb.AppendLine(String.Format("<tr style='background-color:silver;'><td colspan='2'>{0}</td></tr>", error.Message));
                sb.AppendLine(String.Format("<tr><td colspan='2'>{0}</td></tr>", error.StackTrace));

                Dal.ErrorHandler.SetErrorSent(error.Id);
            }


            sb.Append("</table>");

            EmailSender e = new EmailSender();
            e.SendEmail(new Dto.Email()
            {
                Body = sb.ToString(),
                FromEmail = Dal.Helper.ErrorEmail,
                FromName = "Error log",
                Subject = String.Format( "Błędy w aplikacji ({0})", errors.Count),
                ToEmail = Dal.Helper.ErrorEmail,
                ToName = ""
            }
            );
        }
        public static void AllegroSendErrors()
        {
            List<Dal.ProductCatalogAllegroItemErrorsView> errors = Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogAllegroItemsWithErrors();

            if (errors.Count == 0)
                return;



            StringBuilder sb = new StringBuilder();


            sb.Append("<style> table tr td {padding:2px;}</style><table>");

            foreach (Dal.ProductCatalogAllegroItemErrorsView error in errors)
            {
                sb.AppendLine(String.Format(@"<tr style='background-color:black; color:white;'>
<td>{0:yyyy/MM/dd hh:mm:ss}</td>
<td>{2}</td><td>Allegro: <a href='http://allegro.pl/show_item.php?item={1}' target='_blank'>{1}</a></td>
<td>Katalog: <a href='http://192.168.0.107/ProductCatalog.Specification.aspx?id={2}' target='_blank'>{2}</a></td>
<td>{3}</td>
</tr>", error.ValidatedAt, error.ItemId, error.ProductCatalogId, error.ShopName));

                sb.AppendLine(String.Format("<tr><td>{0}</td></tr>",  error.Comment));
                 
                 
            }
            Dal.DbHelper.ProductCatalog.Allegro.SetProductCatalogAllegroItemsWithErrorsSent(errors);

            sb.Append("</table>");

            EmailSender e = new EmailSender();

            List<Dal.Helper.SystemRole> roles = new List<Dal.Helper.SystemRole>
            {
                Dal.Helper.SystemRole.Admin,
                Dal.Helper.SystemRole.Backend
            };
            e.SendEmail(new Dto.Email()
            {
                Body = sb.ToString(),
                FromEmail = Dal.Helper.ErrorEmail,
                FromName = "System",
                Subject = String.Format("Błąd w procedurze weryfikacji aukcji Allegro ({0})", errors.Count),
                ToEmail = Dal.Helper.ErrorEmail,
                ToRoles = roles,
                ToName = ""
            }
            );
        }
    }
}
