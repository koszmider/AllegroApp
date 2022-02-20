using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("70aa268b-9b22-4c4f-ac83-1d82ab8bd751")]
    public partial class EmailTestPage : LajtitPage
    {
        protected void btnSend_Click(object sender, EventArgs e)
        {
            Bll.EmailSender s = new Bll.EmailSender();
            Bll.Dto.Email email = new Bll.Dto.Email()
            {
                Body = "test",
                FromEmail = Dal.Helper.MyEmail,
                FromName = "Jaaa",
                Subject = "Test maila z systemu",
                ToEmail = txbTo.Text.Trim(),
                ToName = "www"

            };
            s.SendEmail(email, txbSMTP.Text.Trim(), Convert.ToInt32(txbPort.Text.Trim()));

            DisplayMessage("Wysłano");
        }
    }
}