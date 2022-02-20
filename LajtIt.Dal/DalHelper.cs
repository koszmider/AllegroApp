using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Dal
{
    public class DalHelper
    {


        public List<Dal.EmailTemplates> GetEmailTemplates()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.EmailTemplates.ToList();
            }
        }

        public EmailTemplates GetEmailTemplate(int emailTemplateId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                SettingsHelper sh = new SettingsHelper();
                string emailTemplate = sh.GetSetting("EMAILTEMPL").StringValue;
                EmailTemplates et = ctx.EmailTemplates.Where(x => x.EmailTemplateId == emailTemplateId).FirstOrDefault();

                et.Body = emailTemplate.Replace("[CONTENT]", et.Body);
                return et;
            }
        }
        public EmailTemplates GetEmailTemplateInner(int emailTemplateId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                SettingsHelper sh = new SettingsHelper();
                string emailTemplate = sh.GetSetting("EMAILTEMP").StringValue;
                EmailTemplates et = ctx.EmailTemplates.Where(x => x.EmailTemplateId == emailTemplateId).FirstOrDefault();

                et.Body = emailTemplate.Replace("[CONTENT]", et.Body);
                return et;
            }
        }

        public EmailTemplates GetEmailTemplateSource(int emailTemplateId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                EmailTemplates et = ctx.EmailTemplates.Where(x => x.EmailTemplateId == emailTemplateId).FirstOrDefault();
                 
                return et;
            }
        }
        //public List<AllegroCommentsToSendView> GetCommentsToSend()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return ctx.AllegroCommentsToSendViews.ToList();
        //    }
        //}

        public void SetEmailTemplate(EmailTemplates emailTemplate)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                EmailTemplates tmp = ctx.EmailTemplates.Where(x => x.EmailTemplateId == emailTemplate.EmailTemplateId).FirstOrDefault();

                tmp.Body = emailTemplate.Body;
                tmp.FromEmail = emailTemplate.FromEmail;
                tmp.FromName = emailTemplate.FromName;
                tmp.Subject = emailTemplate.Subject;
                tmp.TemplateName = emailTemplate.TemplateName;
                tmp.UpdateDate = emailTemplate.UpdateDate;

                ctx.SubmitChanges();
            }
        }

        //public void SetEmailSentFlag(long commentId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        AllegroComment comment = ctx.AllegroComments.Where(x => x.CommentId == commentId).FirstOrDefault();

        //        if (comment != null)
        //        {
        //            comment.EmailSent = true;
        //            comment.EmailSentDate = DateTime.Now;
        //            ctx.SubmitChanges();
        //        } 
        //    }
        //}
        
       
    }
}
