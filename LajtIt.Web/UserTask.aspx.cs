using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("5642cc5e-0b39-4c9d-8e09-2ac9e9ba1c94")]
    public partial class UserTask : LajtitPage
    {
        public int? TaskId
        {
            get
            {
                if (Request.QueryString["id"] != null)
                    return Convert.ToInt32(Request.QueryString["id"]);
                else
                    return null;

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //calDate.SelectedDate = DateTime.Now;
                //txbDate.Text = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                BindUsers();
                BindTaskStatues();
                BindTypes();
                if (TaskId.HasValue)
                    BindTask();
                else
                    ViewState["Status"] = "";

                //txbDate.Enabled =txbPriority.Enabled = txbComment.Enabled = txbTitle.Enabled = txbWorkingHours.Enabled = (this.HasAccess(new Dal.Helper.SystemRole[] { Dal.Helper.SystemRole.Admin, Dal.Helper.SystemRole.Manager }) || !TaskId.HasValue);
            }
            else
            {
                if (txbDate.Text != "")
                    calDate.SelectedDate = DateTime.Parse(txbDate.Text);

            }
            lblAtt.Visible = !TaskId.HasValue;
            pAtt.Visible = TaskId.HasValue;
        }

        private void BindUsers()
        {
            Dal.AdminUserHelper auh = new Dal.AdminUserHelper();
            chblAssignedUser.DataSource = auh.GetUsers();
            chblAssignedUser.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string assignedUser = String.Join(",", chblAssignedUser.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Text).ToArray());

            DateTime? finishDate = null;
            if (txbDate.Text != "")
                finishDate = calDate.SelectedDate;

            Dal.TaskTracker tt = new Dal.TaskTracker()
            {
                Comment = txbComment.Text.Trim(),
                FinishDate = finishDate,
                TaskId = TaskId ?? 0,
                TaskTrackerStatusId = Convert.ToInt32(ddlTaskTrackerStatus.SelectedValue),
                Title = txbTitle.Text.Trim(),
                UserAssigned = assignedUser,
                WorkingHours = Convert.ToDecimal(txbWorkingHours.Text),
                Priority = Convert.ToInt32(txbPriority.Text),
                TaskTypeId = Convert.ToInt32(ddlTaskType.SelectedValue),
                IsRecurringTask = chbIsRecurringTask.Checked
            };
            Dal.TaskTrackerStatusHistory ttsh = new Dal.TaskTrackerStatusHistory()
            {
                InsertDate = DateTime.Now,
                InsertUser = UserName,
                TaskId = TaskId ?? 0,
                TaskTrackerStatusId = Convert.ToInt32(ddlTaskTrackerStatus.SelectedValue),
                Comment = txbStatusComment.Text.Trim()
            };
            Dal.TaskTrackerHelper tth = new Dal.TaskTrackerHelper();

            int result = tth.SetTaskUpdate(tt, ttsh);

            if (result != 0)
            {
                SendEmail(tt);
                if (TaskId.HasValue)
                {
                    DisplayMessage("Zmiany zostały zapisane");
                    BindTask();
                }
                else
                    Response.Redirect(String.Format("UserTask.aspx?id={0}", result));
            }
            else
                DisplayMessage("Błąd zapisu danych");
        }

        private void SendEmail(Dal.TaskTracker tt)
        {     
            Dal.DalHelper dh = new Dal.DalHelper();

            Dal.EmailTemplates emailTemplate = dh.GetEmailTemplateInner(14);


            string body = String.Format(emailTemplate.Body, 
                UserName, 
                tt.TaskId,
                Dal.TaskTrackerHelper.TextToHtml(txbStatusComment.Text),
                Dal.TaskTrackerHelper.TextToHtml(txbComment.Text));



            Bll.Dto.Email email = new Bll.Dto.Email()
            {
                Body = body,
                FromEmail = emailTemplate.FromEmail,
                FromName = emailTemplate.FromName,
                Subject = String.Format( emailTemplate.Subject, ddlTaskType.SelectedItem.Text, tt.Title),
             //   ToEmail = "",
            //    ToName = String.Format("{0} {1}", order.ShipmentFirstName, order.ShipmentLastName)
            };

            Bll.EmailSender e = new Bll.EmailSender();

            //e.SendEmail(subject, body, Dal.Helper.DevEmail);
            Dal.AdminUserHelper auh = new Dal.AdminUserHelper();


            string[] assignedUser = chblAssignedUser.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Text).ToArray();

            foreach (string user in assignedUser)
            {
                Dal.AdminUser au = auh.GetUser(user);
                email.ToEmail = au.Email;
                email.ToName = au.UserName;
                e.SendEmail(email);
            }
        }

        private void BindTaskStatues()
        {
            Dal.TaskTrackerHelper tth = new Dal.TaskTrackerHelper();
            ddlTaskTrackerStatus.DataSource = tth.GetStatuses();
            ddlTaskTrackerStatus.DataBind();
            ddlTaskTrackerStatus.SelectedValue = "1";


        }
        private void BindTypes()
        {
            Dal.TaskTrackerHelper tth = new Dal.TaskTrackerHelper();
            ddlTaskType.DataSource = tth.GetTaskTypes();
            ddlTaskType.DataBind();
        }

        private Dal.TaskTracker BindTask()
        {
            Dal.TaskTrackerHelper tth = new Dal.TaskTrackerHelper();

            Dal.TaskTracker tt = tth.GetTask(TaskId.Value);
            ViewState["Status"] = tt.TaskTrackerStatusId;

            txbComment.Text = tt.Comment;
            txbTitle.Text = tt.Title;
            txbWorkingHours.Text = tt.WorkingHours.ToString();
            ddlTaskTrackerStatus.SelectedValue = tt.TaskTrackerStatusId.ToString();
            if (tt.UserAssigned != null)

                foreach (string user in tt.UserAssigned.Split(new char[] { ',' }).Where(x=>!String.IsNullOrEmpty(x)).ToArray())
                    chblAssignedUser.Items.FindByText(user).Selected = true;

            if (tt.Priority.HasValue)
                txbPriority.Text = tt.Priority.ToString();

            ddlTaskType.SelectedValue = tt.TaskTypeId.ToString();

            chbIsRecurringTask.Checked = tt.IsRecurringTask;
            txbDate.Enabled = !tt.IsRecurringTask;
            if (!tt.IsRecurringTask)
            {

                if (tt.FinishDate.HasValue)
                    txbDate.Text = "";
                calDate.SelectedDate = tt.FinishDate;

            }
            else
                rfvDate.Enabled = false;
            gvTasks.DataSource = tth.GetTaskStatuseHistory(TaskId.Value);
            gvTasks.DataBind();


            BindFiles();

            return tt;
        }

        private void BindFiles()
        {
            string upl = String.Format(@"/Files/UsersTasks/{0}", TaskId);
            if (Directory.Exists(Server.MapPath(upl)))
            {
                gvFiles.DataSource = Directory.GetFiles(Server.MapPath(upl)).OrderByDescending(d => new FileInfo(d).CreationTime);
                gvFiles.DataBind();
            }

            btnDelete.Visible = gvFiles.Rows.Count > 0;
        }

        protected void btnAtt_Click(object sender, EventArgs e)
        { 
            HttpFileCollection uploadedFiles = Request.Files;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbe = new StringBuilder();

            string uploadDir = @"/Files/UsersTasks";
            if(!Directory.Exists(Server.MapPath(uploadDir)))
                Directory.CreateDirectory(Server.MapPath(uploadDir));


            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];

                try
                {
                    if (userPostedFile.ContentLength > 0)
                    {
                        string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(userPostedFile.FileName));
                        string orginalFileName = System.IO.Path.GetFileName(userPostedFile.FileName);

                        string upl = String.Format(@"/Files/UsersTasks/{0}", TaskId);
                        if (!Directory.Exists(Server.MapPath(upl)))
                            Directory.CreateDirectory(Server.MapPath(upl));


                        string saveLocation = String.Format(@"{0}\{1}", Server.MapPath(upl), orginalFileName);

                        if (saveLocation != null)
                            userPostedFile.SaveAs(saveLocation);

                       
                        sb.AppendLine(String.Format("{0}. plik: {1}<br>", i + 1, userPostedFile.FileName));


                    }
                }
                catch (Exception Ex)
                {
                    sbe.AppendLine(String.Format("{0}. plik: {1}<br>", i + 1, userPostedFile.FileName));
                }
            }
            if (sbe.Length == 0)
                DisplayMessage(String.Format("Zapisano poprawnie<br><br>{0}", sb.ToString()));
            else
                DisplayMessage(String.Format("Błędy<br><br>{0}", sbe.ToString()));

            BindFiles();
        }

        protected void gvFiles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                string fileName = e.Row.DataItem as string;
                FileInfo fi = new FileInfo(fileName);
                fileName = Path.GetFileName(fileName);

                HyperLink hlFile = e.Row.FindControl("hlFile") as HyperLink;
                HiddenField hidFileName = e.Row.FindControl("hidFileName") as HiddenField;
                Label  lblData = e.Row.FindControl("lblData") as Label;
                hlFile.NavigateUrl = String.Format(hlFile.NavigateUrl, TaskId, fileName);
                hlFile.Text = fileName;
                lblData.Text = fi.LastAccessTime.ToString();
                hidFileName.Value = fileName;


                Literal liId = e.Row.FindControl("liId") as Literal;


                liId.Text = String.Format("{0}.", gvFiles.PageIndex * gvFiles.PageSize + e.Row.RowIndex + 1);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

            foreach (GridViewRow row in gvFiles.Rows)
            {
                CheckBox chbOrder = row.FindControl("chbOrder") as CheckBox;


                if (chbOrder.Checked)
                {
                    var fileName = (row.FindControl("hidFileName") as HiddenField).Value;

                    File.Delete(Server.MapPath(String.Format("/Files/UsersTasks/{0}/{1}", TaskId, fileName)));
                }



                BindFiles();

                //DisplayMessage("Plik usunięty");


            }
        }

        protected void chbIsRecurringTask_CheckedChanged(object sender, EventArgs e)
        {
            txbDate.Enabled = !chbIsRecurringTask.Checked;
            txbDate.Text = "";
            calDate.SelectedDate = null;
            rfvDate.Enabled = !chbIsRecurringTask.Checked;
        }
    }
}