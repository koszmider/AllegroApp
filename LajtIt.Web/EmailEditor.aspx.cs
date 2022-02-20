using System;
using System.Web.UI;
using LajtIt.Bll;

namespace LajtIt.Web
{
    [Developer("6bab41fb-e123-413b-9b00-688877a22b0f")]
    public partial class EmailEditor : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                LoadEmailTemplatess();
        }

        private void LoadEmailTemplatess()
        {
            Dal.DalHelper dal = new Dal.DalHelper();
            ddlEmailTemplates.DataSource = dal.GetEmailTemplates();
            ddlEmailTemplates.DataBind();
        }
        protected void btnOpen_Click(object sender, EventArgs e)
        {
            if (ddlEmailTemplates.SelectedIndex == 0)
                return;


            Dal.DalHelper dal = new Dal.DalHelper();
            Dal.EmailTemplates EmailTemplates = dal.GetEmailTemplateSource(Convert.ToInt32(ddlEmailTemplates.SelectedValue));

            txbBody.Text = EmailTemplates.Body;
            txbSubject.Text = EmailTemplates.Subject;
            txbFromEmail.Text = EmailTemplates.FromEmail;
            txbFromName.Text = EmailTemplates.FromName;
            pnlEditor.Visible = true;
            btnCancel.Visible = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dal.EmailTemplates EmailTemplates = new Dal.EmailTemplates()
            {
                Body = txbBody.Text.Trim(),
                EmailTemplateId = Convert.ToInt32(ddlEmailTemplates.SelectedValue),
                FromEmail = txbFromEmail.Text.Trim(),
                FromName = txbFromName.Text.Trim(),
                Subject = txbSubject.Text.Trim(),
                UpdateDate = DateTime.Now, 
                TemplateName= ddlEmailTemplates.SelectedItem.Text                
            };


            Dal.DalHelper dal = new Dal.DalHelper();
            dal.SetEmailTemplate(EmailTemplates);
            btnCancel_Click(null, null);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlEditor.Visible = false;
            btnCancel.Visible = false;
            
        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            Dal.Settings s = sh.GetSetting("EMAILTEMPL");
            pnlPreview.Visible = true;
            litPreview.Text = s.StringValue.Replace("[CONTENT]", txbBody.Text);

        }
    }
}