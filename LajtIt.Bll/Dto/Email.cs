using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Bll.Dto
{
    public class Email
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string ToEmail { get; set; }
        public List<Dal.Helper.SystemRole> ToRoles { get; set; }
        public string ToName { get; set; }
        public string AttachmentFile { get; set; }
    }
}
