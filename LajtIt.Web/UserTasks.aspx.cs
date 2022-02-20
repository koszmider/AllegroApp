using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("1ce7d1ba-792e-431b-b613-493ef6164c5e")]
    public partial class UserTasks : LajtitPage
    {
        bool isManager = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            isManager = this.HasAccess(new Dal.Helper.SystemRole[] { Dal.Helper.SystemRole.Admin, Dal.Helper.SystemRole.Manager });
            if (!Page.IsPostBack)
            {
                BindUsers();
                BindTaskStatues();
                BindTasks();
                BindTypes();
                //ddlUserName.Enabled = isManager;
            }
            btnSaveTasks.Visible = isManager;
        }
        private void BindTypes()
        {
            Dal.TaskTrackerHelper tth = new Dal.TaskTrackerHelper();
            lbxTaskTypes.DataSource = tth.GetTaskTypes();
            lbxTaskTypes.DataBind();
        }
        private void BindTasks()
        {
            if (this.HasAccess(new Dal.Helper.SystemRole[] { Dal.Helper.SystemRole.Admin }))
                BindTasks(null);
            else
            {
                ddlUserName.SelectedItem.Text = UserName;
                BindTasks(UserName);
            }

        }
        private void BindUsers()
        {
            Dal.AdminUserHelper auh = new Dal.AdminUserHelper();
            ddlUserName.DataSource = auh.GetUsers();
            ddlUserName.DataBind();
        }

        protected void gvTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.TaskTrackerView ttv = e.Row.DataItem as Dal.TaskTrackerView;
                TextBox txbPriority = e.Row.FindControl("txbPriority") as TextBox;
                Label lblPriority = e.Row.FindControl("lblPriority") as Label;
                HyperLink hlUserTask = e.Row.FindControl("hlUserTask") as HyperLink;
                Image imgReccuringTask = e.Row.FindControl("imgReccuringTask") as Image;

                hlUserTask.NavigateUrl = String.Format(hlUserTask.NavigateUrl, ttv.TaskId);
                hlUserTask.Text = ttv.Title;

                imgReccuringTask.Visible = ttv.IsRecurringTask;

                if (ttv.Priority.HasValue)
                {
                    txbPriority.Text = ttv.Priority.ToString();
                    lblPriority.Text = ttv.Priority.ToString();

                }
                txbPriority.Visible = isManager;
                lblPriority.Visible = !isManager;
                if (ttv.FinishDate.HasValue)
                {
                    double days = (ttv.FinishDate.Value - DateTime.Now).TotalDays;
                    if (days < 0)
                        e.Row.BackColor = System.Drawing.Color.LightPink;
                }
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ddlUserName.SelectedIndex == 0)
                BindTasks(null);
            else
                BindTasks(ddlUserName.SelectedItem.Text);
        }

        private void BindTaskStatues()
        {
            Dal.TaskTrackerHelper tth = new Dal.TaskTrackerHelper();
            lbxUserTaskStatus.DataSource = tth.GetStatuses();
            lbxUserTaskStatus.DataBind();  

            lbxUserTaskStatus.Items[1].Selected = true;
            lbxUserTaskStatus.Items[2].Selected = true;
        }
        private void BindTasks(string userName)
        {
            Dal.TaskTrackerHelper tth = new Dal.TaskTrackerHelper();
            List<Dal.TaskTrackerView> tasks = tth.GetTasks(userName);
            int[] statuses = lbxUserTaskStatus.Items.Cast<ListItem>().Where(x => x.Selected == true).Select(x => Convert.ToInt32(x.Value)).ToArray();

            if (statuses.Length > 0)
                tasks = tasks.Where(x => statuses.Contains(x.TaskTrackerStatusId)).ToList();
            int[] types = lbxTaskTypes.Items.Cast<ListItem>().Where(x => x.Selected == true).Select(x => Convert.ToInt32(x.Value)).ToArray();

            if (types.Length > 0)
                tasks = tasks.Where(x => types.Contains(x.TaskTypeId)).ToList();

            gvTasks.DataSource = tasks.OrderByDescending(x => x.IsRecurringTask).ToList() ;
            gvTasks.DataBind();
        }

        protected void btnSaveTasks_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> priorities = new Dictionary<int, int>();
            foreach(GridViewRow row in gvTasks.Rows)
            {

                TextBox txbPriority = row.FindControl("txbPriority") as TextBox;

                int taskId = Convert.ToInt32(gvTasks.DataKeys[row.RowIndex][0]);

                priorities.Add(taskId, Convert.ToInt32(txbPriority.Text));


            }

            Dal.TaskTrackerHelper tth = new Dal.TaskTrackerHelper();
            tth.SetTaskPriority(priorities);

            btnShow_Click(null, null);

        }

        protected void tmInterval_Tick(object sender, EventArgs e)
        {
            BindTasks();
        }
    }
}