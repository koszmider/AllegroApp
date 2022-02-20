using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    public partial class AllegroGoals : LajtitPage
    {
        List<Dal.AllegroGoalsScheduleView> schedules;

        private int GoalId
        {
            set { hidGoalId.Value = value.ToString(); }
            get { return Convert.ToInt32(hidGoalId.Value); }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GoalId = 0;
                BindProducts(true, 0);
                BindGoals();
                BindSchedules();
            }
        }
        private void BindGoals()
        {
            int productGroupId = Convert.ToInt32(ddlProductGroupsSearch.SelectedValue);
            Dal.AllegroGoalHelper agh = new Dal.AllegroGoalHelper();
            List<Dal.AllegroGoalsView> goals = agh.BindGoals();
            if (productGroupId != 0)
                goals = goals.Where(x => x.ProductCatalogGroupId == productGroupId).ToList();
            if (cbhIsActive.Checked)
                goals = goals.Where(x => x.IsActive == true).ToList();

            gvGoals.DataSource = goals;
            gvGoals.DataBind();

        }
        protected void ddlProductGroupsSearch_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindSchedules();
            BindGoals();
        }
        private void BindSchedules()
        {
            var values = Enum.GetValues(typeof(Dal.Helper.Weekdays)).Cast<Dal.Helper.Weekdays>()
                .Select(x=>new  {
                    WeekdayId = (int)x,
                    Weekday = x
                }).ToList();



            Dal.AllegroGoalHelper agh = new Dal.AllegroGoalHelper();
            schedules = agh.GetSchedules();


            int productGroupId = Convert.ToInt32(ddlProductGroupsSearch.SelectedValue);
            if (productGroupId != 0)
                schedules = schedules.Where(x => x.ProductCatalogGroupId == productGroupId).ToList();
            if (cbhIsActive.Checked)
                schedules = schedules.Where(x => x.IsActive==true).ToList();


            rpWeekdays.DataSource = values;
            rpWeekdays.DataBind();

             
        }
        protected void gvGoals_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.AllegroGoalsView goal = e.Row.DataItem as Dal.AllegroGoalsView;
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(String.Format("#{0}", goal.Color));
           
                e.Row.Cells[0].BackColor = color;

                HyperLink hlProductAllegroName = e.Row.FindControl("hlProductAllegroName") as HyperLink;
                hlProductAllegroName.NavigateUrl = String.Format("http://allegro.pl/show_item.php?item={0}", goal.ItemId);
                hlProductAllegroName.Text = goal.ItemName ?? goal.ItemId.ToString();

            }
        }


        protected void btnNewScheduleAdd_Click(object sender, EventArgs e)
        {
            Dal.AllegroGoalSchedule schedule = new Dal.AllegroGoalSchedule()
            {
                Day = ddlWeekday.SelectedIndex,
                GoalId = GoalId,
                Hour = new TimeSpan( Convert.ToInt32(txbHour.Text),  Convert.ToInt32(txbMinute.Text), 0)
            };
            Dal.AllegroGoalHelper agh = new Dal.AllegroGoalHelper();
            agh.SetGoalSchedule(schedule);

            BindGoal(GoalId); 
        }

        protected void gvGoalSchedule_OnRowDeleting(object sender,  GridViewDeleteEventArgs e)
        {
            int scheduleId = Convert.ToInt32((sender as GridView).DataKeys[e.RowIndex][0]);
        }
        protected void lnbDeleteSchedule_Click(object sender, EventArgs e)
        {
            int scheduleId = Convert.ToInt32((sender as LinkButton).CommandArgument);

            Dal.AllegroGoalHelper agh = new Dal.AllegroGoalHelper();
            agh.SetGoalScheduleDelete(scheduleId);
            BindGoal(Convert.ToInt32(hidGoalId.Value));
        }
        protected void gvGoalSchedule_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                LinkButton lnbDeleteSchedule = e.Row.FindControl("lnbDeleteSchedule") as LinkButton;
                string color = DataBinder.Eval(e.Row.DataItem, "Color").ToString();

                object scheduleId = DataBinder.Eval(e.Row.DataItem, "ScheduleId");


                object goalId = DataBinder.Eval(e.Row.DataItem, "GoalId");


                lnbDeleteSchedule.Visible = scheduleId != null && goalId != null && Convert.ToInt32(goalId) == GoalId;

                if (scheduleId != null)
                {
                    e.Row.Cells[2].ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[2].BackColor = System.Drawing.ColorTranslator.FromHtml(String.Format("{0}", color));
                }
            }
        }

        protected void rpWeekdays_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblWeekday = e.Item.FindControl("lblWeekday") as Label;
                int weekday = Convert.ToInt32(lblWeekday.ToolTip);
                GridView gvGoalSchedule = e.Item.FindControl("gvGoalSchedule") as GridView;
                var result = Enumerable.Range(0, 24)
                    .Select(i => new {
                        GoalId = GetGoalId(i, weekday),
                        Hour = i,
                        DateTimeValue = String.Format("{0:HH}", new DateTime(2011, 1, 1, i, 0, 0)),
                        Schedule = GetTimeSchedule(i, weekday),
                        ScheduleId = GetTimeScheduleId(i, weekday),
                        Color =   GetColorSchedule(i, weekday),
                        Weekday = weekday})
                    .ToArray();
                gvGoalSchedule.DataSource = result;
                gvGoalSchedule.DataBind();
            }
        }

        private string GetColorSchedule(int hour, int weekday)
        {
            Dal.AllegroGoalsScheduleView schedule = schedules.Where(x => x.Hour.Hours == hour && x.Day == weekday).FirstOrDefault();
            if (schedule == null)
                return "#000000";
            return String.Format("#{0}",  schedule.Color);
        }
        private string GetTimeSchedule(int hour, int weekday)
        {
            Dal.AllegroGoalsScheduleView schedule = schedules.Where(x => x.Hour.Hours == hour && x.Day == weekday).FirstOrDefault();
            if (schedule == null)
                return null;
            return String.Format("{0:mm}", new DateTime(2011, 1, 1, hour, schedule.Hour.Minutes, 0));
        }
        private int? GetTimeScheduleId(int hour, int weekday)
        {
            Dal.AllegroGoalsScheduleView schedule = schedules.Where(x => x.Hour.Hours == hour && x.Day == weekday).FirstOrDefault();
            if (schedule == null)
                return null;
            return schedule.ScheduleId;
        }
        private int? GetGoalId(int hour, int weekday)
        {
            Dal.AllegroGoalsScheduleView schedule = schedules.Where(x => x.Hour.Hours == hour && x.Day == weekday).FirstOrDefault();
            if (schedule == null)
                return null;
            return schedule.GoalId;
        }
        protected void gvGoals_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument );
            int goalId = Convert.ToInt32(gvGoals.DataKeys[index][0]);

            BindGoal(goalId);
        }

        private void BindGoal(int goalId)
        {
            pGoalNew.Visible = true;
            GoalId = goalId;
            Dal.AllegroGoalHelper agh = new Dal.AllegroGoalHelper();
            Dal.AllegroGoalsView goal = agh.GetGoal(goalId);

            txbGoalNewItemId.Text = goal.ItemId.ToString();
            txbGoalNewName.Text = goal.Name;
            chbGoalNewIsActive.Checked = goal.IsActive;
            txbColor.Text = goal.Color;
            BindProducts(true, goal.ProductCatalogGroupId.Value);
            ddlProducts.SelectedValue = goal.ProductCatalogId.ToString();

            pNewSchedule.Visible = true;
             
            BindSchedules( );
        }
        protected void ddlProductGroups_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindProducts(false, Convert.ToInt32(ddlProductGroups.SelectedValue));
        }
        protected void btnGoalNew_Click(object sender, EventArgs e)
        {
            //Dal.AllegroGoal goal = new Dal.AllegroGoal()
            //{ 
            //    IsActive = chbGoalNewIsActive.Checked,
            //    ItemId = Convert.ToInt64(txbGoalNewItemId.Text),
            //    LastUpdateDate = DateTime.Now,
            //    Name = txbGoalNewName.Text.Trim(),
            //    ProductCatalogId = Convert.ToInt32(ddlProducts.SelectedValue),
            //    Color = txbColor.Text
            //};
            //Bll.AllegroGoalHelper agh = new Bll.AllegroGoalHelper();
            //if (!agh.VerifyItemId(!!!!!!!!!!!!!!!!!!!!!! goal.ItemId))
            //{
            //    DisplayMessage("Nie udało się zweryfikować numeru aukcji.");
            //    return;
            //}

            //if (GoalId == 0)
            //    NewGoal(goal);
            //else
            //    EditGoal(goal);

            //BindGoals();

            //pGoalNew.Visible = false;
        }

        private void EditGoal(Dal.AllegroGoal goal)
        {
            Bll.AllegroGoalHelper agh = new Bll.AllegroGoalHelper();
            goal.GoalId = Convert.ToInt32(hidGoalId.Value);
            bool result = agh.GoalEdit(goal); 
        }

        private void NewGoal(Dal.AllegroGoal goal)
        {
            Bll.AllegroGoalHelper agh = new Bll.AllegroGoalHelper();
            bool result = agh.GoalAdd(goal); 
        }
        protected void lnbGoalNewCancel_Click(object sender, EventArgs e)
        {
            pGoalNew.Visible = false;
            GoalId = 0;
            BindSchedules();
        }
        protected void lnbGoalNew_Click(object sender, EventArgs e)
        {
            pGoalNew.Visible = true;
            BindProducts(true, 0);
            txbGoalNewItemId.Text = txbGoalNewName.Text = "";
            ddlProducts.SelectedIndex = 0;
            chbGoalNewIsActive.Checked = false;
            GoalId = 0; 
            pNewSchedule.Visible = false;
        }


        public void BindProducts(bool bindGroups, int productGroupId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<LajtIt.Dal.ProductCatalog> products = oh.GetProductsFromCatalog();

            var g = products.Select(x => new
            {
                ProductCatalogGroupId = x.ProductCatalogGroup.ProductCatalogGroupId,
                GroupName = x.ProductCatalogGroup.GroupName
            }).Distinct().OrderBy(x => x.GroupName).ToList();

            if (bindGroups)
            {
                ddlProductGroups.DataSource = g;
                ddlProductGroups.DataBind();
            }
            if (productGroupId != 0)
                ddlProductGroups.SelectedValue = productGroupId.ToString();

            if (!Page.IsPostBack)
            {
                ddlProductGroupsSearch.DataSource = g;
                ddlProductGroupsSearch.DataBind();
            }
            var o = products
                .Where(x => x.ProductCatalogGroupId == Convert.ToInt32(ddlProductGroups.SelectedValue))
                .Select(x => new
                {
                    ProductCatalogId = x.ProductCatalogId,
                    Name = String.Format("{0} ({1:C})", x.Name, x.Price)
                }).OrderBy(x => x.Name).ToList();

            ddlProducts.DataSource = o;
            ddlProducts.DataBind();
        }
    }
}