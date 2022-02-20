using EAGetMail;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LajtIt.Bll
{
    public class AllegroEmailResponder
    {
        public static void ReadMailbox()
        {
            try
            {

                using (Pop3Client client = new Pop3Client())
                {
                    // Connect to the server
                    client.Connect(System.Configuration.ConfigurationManager.AppSettings["SMTP_Server"], 
                        995, true);

                    // Authenticate ourselves towards the server
                    client.Authenticate("kontakt@lajtit.pl", System.Configuration.ConfigurationManager.AppSettings["SMTP_Kontakt_Pwd"]);

                    // Get the number of messages in the inbox
                    int messageCount = client.GetMessageCount();

                    // We want to download all messages
                    List<OpenPop.Mime.Message> allMessages = new List<OpenPop.Mime.Message>(messageCount);


                    Dal.AllegroScan asc = new Dal.AllegroScan();
                    Dal.AllegroEmail lastEmail = asc.GetAllegroEmailLast();


                    // Messages are numbered in the interval: [1, messageCount]
                    // Ergo: message numbers are 1-based.
                    // Most servers give the latest message the highest number
                    for (int i = messageCount; i > 0; i--)
                    {
                       OpenPop.Mime.Message oMail=  client.GetMessage(i);

                        if (lastEmail != null && lastEmail.SentDate >= oMail.Headers.DateSent.ToLocalTime())
                            break;



                        if (oMail.Headers.Subject.StartsWith("Pytanie o przedmiot") && oMail.Headers.From.Address.Contains("powiadomienia@allegro.pl"))
                        {
                            Dal.AllegroEmail ae = new Dal.AllegroEmail()
                            {
                                EmailId = "",//oMail.MessagePart. info.UIDL,
                                HtmlBody = "",//oMail.MessagePart.GetBodyAsText(),
                                Subject = oMail.Headers.Subject,
                                IsReplied = false,
                                SentDate = oMail.Headers.DateSent.ToLocalTime(),
                                FromEmail = oMail.Headers.From.Address
                            };

                            if (oMail.Headers.ReplyTo != null)
                                ae.UserEmail = oMail.Headers.ReplyTo.Address;
                            else
                                ae.UserEmail = oMail.Headers.From.Address;


                            asc.SetAllegroEmail(ae);

                            Console.WriteLine("From: {0}", ae.FromEmail);
                            Console.WriteLine("Subject: {0}\r\n", ae.Subject);
                        }


                    }
                }
                    Console.WriteLine("Completed!");
            }
            catch (Exception ep)
            {
                Console.WriteLine(ep.Message);
            }

            ProcessResponses();

        }

        //public static void ReadMailbox1 ()
        //{
        //    try
        //    {
            
        //        // Create a folder named "inbox" under current directory
        //        // to save the email retrieved.
        //        //string localInbox = string.Format("{0}\\inbox", Directory.GetCurrentDirectory());
        //        //// If the folder is not existed, create it.
        //        //if (!Directory.Exists(localInbox))
        //        //{
        //        //    Directory.CreateDirectory(localInbox);
        //        //}

        //        MailServer oServer = new MailServer(System.Configuration.ConfigurationManager.AppSettings["SMTP_Server"],
        //                "kontakt@lajtit.pl",
        //                System.Configuration.ConfigurationManager.AppSettings["SMTP_Kontakt_Pwd"],
        //                ServerProtocol.Pop3);

        //        // Enable SSL/TLS connection, most modern email server require SSL/TLS by default
        //        oServer.SSLConnection = true;
        //        oServer.Port = 995;

        //        // if your server doesn't support SSL/TLS, please use the following codes
        //        // oServer.SSLConnection = false;
        //        // oServer.Port = 110;

        //        MailClient oClient = new MailClient("TryIt");
        //        oClient.Connect(oServer);

        //        MailInfo[] infos = oClient.GetMailInfos();
        //        Console.WriteLine("Total {0} email(s)\r\n", infos.Length);

        //        Dal.AllegroScan asc = new Dal.AllegroScan();
        //        Dal.AllegroEmail lastEmail = asc.GetAllegroEmailLast();




        //        int exitAfter = 200;


        //        for (int i =  infos.Length-1; i > 0;  --i)
        //        {
        //            MailInfo info = infos[i];
        //            // Receive email from POP3 server
        //            Mail oMail = oClient.GetMail(info);
        //            Console.WriteLine("Index: {0}; Size: {1}; UIDL: {2}",
        //                info.Index, info.Size, info.UIDL);

        //            if (lastEmail!=null && lastEmail.SentDate >= oMail.ReceivedDate)
        //                break;



        //            //if (oMail.Subject.StartsWith("Pytanie o przedmiot") && oMail.From.Address.Contains("powiadomienia@allegro.pl"))
        //            {
        //                Dal.AllegroEmail ae = new Dal.AllegroEmail()
        //                {
        //                    EmailId = info.UIDL,
        //                    HtmlBody = oMail.HtmlBody,
        //                    Subject = oMail.Subject,
        //                    IsReplied = false,
        //                    SentDate = oMail.ReceivedDate,
        //                    FromEmail = oMail.From.Address,
        //                    UserEmail = oMail.ReplyTo.Address
        //                };

        //                asc.SetAllegroEmail(ae);

        //                Console.WriteLine("From: {0}", oMail.From.ToString());
        //                Console.WriteLine("Subject: {0}\r\n", oMail.Subject);
        //            }
        //            // Generate an unqiue email file name based on date time.
        //            //  string fileName = _generateFileName(i + 1);
        //            //  string fullPath = string.Format("{0}\\{1}", localInbox, fileName);

        //            // Save email to local disk
        //            // oMail.SaveAs(fullPath, true);

        //            // Mark email as deleted from POP3 server.
        //            //oClient.Delete(info);

        //            exitAfter--;
        //            if (lastEmail == null &&  exitAfter < 0)
        //                break;

        //        }

        //        // Quit and expunge emails marked as deleted from POP3 server.
        //        oClient.Quit();
        //        Console.WriteLine("Completed!");
        //    }
        //    catch (Exception ep)
        //    {
        //        Console.WriteLine(ep.Message);
        //    }

        //   /// ProcessResponses();

        //}

        private static void ProcessResponses()
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            List<Dal.AllegroEmail> allegroEmails = asc.GetAllegroEmails();


            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            Dal.Settings s = sh.GetSetting("ALL_EMAIL");


            foreach (Dal.AllegroEmail email in allegroEmails)
            {
                Bll.EmailSender emailSender = new EmailSender();
                string msg = String.Format("{0}<br><br><br>{1}",
                    s.StringValue,
                    email.HtmlBody);



               

                    emailSender.SendEmail(new Dto.Email()
                    {
                        Body = msg,
                        FromEmail = Dal.Helper.MyEmail,
                        FromName = "Lajtit - Doradzamy. Oświetlamy",
                        Subject = String.Format("Odp: {0}", email.Subject.Replace("(Trial Version)","")),
                        ToEmail =  email.UserEmail,
                        ToName = email.UserEmail
                        

                    });

                    asc.SetAllegroEmailReplied(email.Id);
            }
        }
    }
}
