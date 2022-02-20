using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using System.IO;
using System.Drawing;

namespace LajtIt.Web
{
    [Developer("30ad37ee-8b6f-4a69-a7bf-7d8a0dd8c80b")]
    public partial class AdminUserStats : LajtitPage
    {
        private DateTime GetMonth
        {
            get { return Convert.ToDateTime(ddlMonths.SelectedValue); }
        }

        double minutes = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                BindUsers();
                BindMonths();
                BindReport();
            }
        }
        private void BindUsers()
        {
            Dal.AdminUserHelper auh = new Dal.AdminUserHelper();
            ddlUserName.DataSource = auh.GetUsers();
            ddlUserName.DataBind();
        }
        private void BindReport()
        {
            Dal.AdminUserHelper ah = new Dal.AdminUserHelper();

            List<Dal.AdminUserStatByMonthResult> results = ah.GetAdminUserStats(ddlUserName.SelectedValue,Convert.ToDateTime( ddlMonths.SelectedValue));

            gvResults.DataSource = results;
            gvResults.DataBind();

            minutes = (int)(minutes * 1.05);

            TimeSpan ts = TimeSpan.FromMinutes(minutes);

            lblMinutes.Text = String.Format("Liczba minut +5%: {0}, godziny: {1}:{2}", (int)minutes, ts.Hours, ts.Minutes);
        }
     
        protected void gvResults_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal LitId = e.Row.FindControl("LitId") as Literal;
                LitId.Text = (e.Row.RowIndex + 1).ToString();

                Label lblDay = e.Row.FindControl("lblDay") as Label;
                Label lblMin = e.Row.FindControl("lblMin") as Label;
                Dal.AdminUserStatByMonthResult r = e.Row.DataItem as Dal.AdminUserStatByMonthResult;
                lblDay.Text = r.Min.Value.DayOfWeek.ToString();


                TimeSpan diff = r.Max.Value.Subtract(r.Min.Value);

                lblMin.Text = String.Format("{0}", (int) diff.TotalMinutes);

                minutes += diff.TotalMinutes;
            } 
        } 
        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindReport();
        }
     
        private void BindMonths()
        {
            ddlMonths.Items.Clear();


            List<ListItem> items = new List<ListItem>();
            for (int year = DateTime.Now.Year; year >= 2017; year--)
                for (int month = DateTime.Now.Year == year ? DateTime.Now.Month : 12; month >= 1; month--)
                {
                    ListItem item = new ListItem()
                    {
                        Text = String.Format("{0}/{1}", year, month),
                        Value = String.Format("{0}/{1}/{2}", year, month, 1)
                    };
                    items.Add(item);
                }

            ddlMonths.Items.AddRange(items.ToArray());

        }
    }
}