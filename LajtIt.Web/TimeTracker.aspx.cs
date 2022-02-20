using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("02ad763a-d4a4-4f18-874c-dffa967cb60a")]
    public partial class TimeTracker : LajtitPage
    {
        decimal hoursTotal = 0;
        bool isManager = false;
        private string SelectedUserName
        {
            get { return ddlUserName.SelectedValue; }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            isManager = HasAccess(new Dal.Helper.SystemRole[] { Dal.Helper.SystemRole.Admin, Dal.Helper.SystemRole.Manager });

            Chart2.Visible = isManager;

            if (!Page.IsPostBack)
            {
                BindUsers();


                ddlUserName.Enabled = isManager;
                txbDate.Enabled = isManager;
                ddlUserName.SelectedValue = UserName;

                calDate.SelectedDate = DateTime.Now;
                txbDate.Text = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                BindLocations(ddlLocation);
                BindTimeTacker();
                BindTaskTracker();
            }
            else
            {
                if (txbDate.Text != "")
                    calDate.SelectedDate = DateTime.Parse(txbDate.Text);

            }
            pnNew.Visible = calDate.SelectedDate.Value.Date == DateTime.Now.Date;
        }

        private void BindTaskTracker()
        {

            Dal.TaskTrackerHelper tth = new Dal.TaskTrackerHelper();
            List<Dal.TaskTrackerView> tasks = tth.GetTasks(UserName);
            int[] statuses = new int[] { 1, 2 };//nowy,gotowy
            tasks = tasks.Where(x => statuses.Contains(x.TaskTrackerStatusId)).ToList();


            ddlTaskTracker.DataSource = tasks;
            ddlTaskTracker.DataBind();
        }

        private void BindUsers()
        {
            Dal.AdminUserHelper auh = new Dal.AdminUserHelper();
            ddlUserName.DataSource = auh.GetActiveUsers();
            ddlUserName.DataBind();
        }
        protected void gvTimeTracker_OnRowEditing(object sender, GridViewEditEventArgs e)
        {

            gvTimeTracker.EditIndex = e.NewEditIndex;
            BindTimeTacker();

        }
        protected void gvTimeTracker_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            gvTimeTracker.EditIndex = -1;
            BindTimeTacker();

        }
        protected void gvTimeTracker_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvTimeTracker.Rows[e.RowIndex];
            TextBox txbHours = row.FindControl("txbHours") as TextBox;
            TextBox txbComment = row.FindControl("txbComment") as TextBox;
            DropDownList ddlLocation = row.FindControl("ddlLocation") as DropDownList;

            int id = Convert.ToInt32(gvTimeTracker.DataKeys[e.RowIndex][0]);

            Dal.TimeTracker tt = new Dal.TimeTracker()
            {
                Id = id,
                Comment = txbComment.Text.Trim(),
                WorkingHours = Convert.ToDecimal(txbHours.Text.Trim()),
                LocationTypeId = Convert.ToInt32(ddlLocation.SelectedValue)
            };

            Dal.TimeTrackerHelper tth = new Dal.TimeTrackerHelper();


            bool resultMonth = tth.CheckMonthHours(UserName, tt.WorkingHours, id);
            bool resultDay = tth.CheckDayHours(UserName, tt.WorkingHours, id);

            if (!resultMonth)
            {
                DisplayMessage("Przekroczono dopuszczalną liczbę godzin pracy w miesiącu");
            }
            if (!resultDay)
            {
                DisplayMessage("Przekroczono dopuszczalną liczbę godzin pracy w tym dniu");
            }
            if (resultMonth && resultDay)
            {
                tth.SetTimeTrackerUpdate(tt);

                DisplayMessage("Dane zostały zapisane");
                BindTimeTacker();






                gvTimeTracker.EditIndex = -1;
                hoursTotal = 0;
                BindTimeTacker();
            }

        }
        protected void gvTimeTracker_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Dal.TimeTrackerView tt = e.Row.DataItem as Dal.TimeTrackerView;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (tt.InsertDate.Date != DateTime.Now.Date || SelectedUserName != UserName)
                {
                    e.Row.Enabled = false;
                    e.Row.Cells[0].Controls.Clear();
                }

                hoursTotal += tt.WorkingHours;
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {

                    TextBox txbComment = e.Row.FindControl("txbComment") as TextBox;
                    TextBox txbHours = e.Row.FindControl("txbHours") as TextBox;
                    DropDownList ddlLocation = e.Row.FindControl("ddlLocation") as DropDownList;

                    txbComment.Text = tt.Comment;
                    txbHours.Text = String.Format("{0:0.00}", tt.WorkingHours);
                    BindLocations(ddlLocation);
                    ddlLocation.SelectedValue = tt.LocationTypeId.ToString();
                }
                else
                {
                    Label lblComment = e.Row.FindControl("lblComment") as Label;
                    Label lblHours = e.Row.FindControl("lblHours") as Label;
                    Label lblLocation = e.Row.FindControl("lblLocation") as Label;
                    HyperLink hlTask = e.Row.FindControl("hlTask") as HyperLink;

                    lblComment.Text = tt.Comment;
                    lblHours.Text = String.Format("{0:0.00}", tt.WorkingHours);
                    lblLocation.Text = tt.LocationName;
                    hlTask.Text = tt.TaskTitle;
                    hlTask.NavigateUrl = String.Format(hlTask.NavigateUrl, tt.TaskId);
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblHours = e.Row.FindControl("lblHours") as Label;
                lblHours.Text = String.Format("{0:0.00}", hoursTotal);
            }
        }
        private void BindTimeTacker()
        {
            Dal.TimeTrackerHelper tth = new Dal.TimeTrackerHelper();

            gvTimeTracker.DataSource = tth.GetTimeTracker(calDate.SelectedDate.Value, SelectedUserName);
            gvTimeTracker.DataBind();

            gvReport.DataSource = tth.GetTimeTrackerReport(calDate.SelectedDate.Value, SelectedUserName);
            gvReport.DataBind();

            Dal.AdminUserHelper auh = new Dal.AdminUserHelper();
            Dal.AdminUser au = auh.GetUser(SelectedUserName);

            if (au.MaxHoursPerDay.HasValue)
                lblHourLimits.Text = String.Format("Max. dziennie: {0}, max. miesięcznie: {1}", au.MaxHoursPerDay, au.MaxHoursPerMonth);

            BindChart();
        }

        private void BindLocations(DropDownList ddl)
        {
            Dal.TimeTrackerHelper tth = new Dal.TimeTrackerHelper();
            ddl.DataSource = tth.GetLocations();
            ddl.DataBind();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindTimeTacker();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int? taskId = null;
            if (ddlTaskTracker.SelectedIndex != 0)
                taskId = Convert.ToInt32(ddlTaskTracker.SelectedValue);

            Dal.TimeTracker tt = new Dal.TimeTracker()
            {
                InsertDate = DateTime.Now,
                InsertUser = UserName,
                Comment = txbComment.Text.Trim(),
                WorkingHours = Convert.ToDecimal(txbHours.Text.Trim()),
                LocationTypeId = Convert.ToInt32(ddlLocation.SelectedValue),
                TaskId=taskId
            };

            Dal.TimeTrackerHelper tth = new Dal.TimeTrackerHelper();

            bool resultMonth = tth.CheckMonthHours(UserName, tt.WorkingHours, null);
            bool resultDay = tth.CheckDayHours(UserName, tt.WorkingHours, null);

            if (!resultMonth)
            {
                DisplayMessage("Przekroczono dopuszczalną liczbę godzin pracy w miesiącu");
            }
            if (!resultDay)
            {
                DisplayMessage("Przekroczono dopuszczalną liczbę godzin pracy w tym dniu");
            }
            if (resultMonth && resultDay)
            {
                tth.SetTimeTracker(tt);

                DisplayMessage("Dane zostały zapisane");
                BindTimeTacker();
            }
        }

        protected void BindChart()
        {
            //if (UserName != "jacek")
            //{
            //    Chart2.Visible = false;
            //    return;
            //}
            if (!isManager)
                return;

            Chart2.Visible = isManager;
            Dal.TimeTrackerHelper tth = new Dal.TimeTrackerHelper();
            List<Dal.UserLog> logs = tth.GetTimeTrackerStats(calDate.SelectedDate.Value.Date, ddlUserName.SelectedValue);

            Chart2.Series.Add(new System.Web.UI.DataVisualization.Charting.Series()
            {
                Name = String.Format("{0:HH:mm}", "Czas"),
                ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line,
                XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Time,
                BorderWidth = 1,
                LabelFormat = "{0}",
                

            });

            var m = logs.Select(x => new
            {
                Minute = Math.Round((x.InsertDate - calDate.SelectedDate.Value.Date).TotalMinutes, 0)
            }).ToList();

            if (m.Count > 0)
            {
                double min = m.Min(x => x.Minute);
                double max = m.Max(x => x.Minute);
                for (double i = min; i <= max; i++)
                {
                    //int minute = m.Select(x=>x.Minute) 
                    DateTime dt = calDate.SelectedDate.Value.Date.AddMinutes(i);

                    int count = m.Where(x => x.Minute == i).Count();

                    Chart2.Series[0].Points.AddXY(dt, count);
                }
            }
            else
                Chart2.Visible = false;

        }
    }
}