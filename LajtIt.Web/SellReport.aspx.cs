using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("153e5f20-cb2c-4162-9855-74d9c6fa6393")]
    public partial class SellReport : LajtitPage
    {
        private decimal total = 0;
        private decimal commision = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //(this.Master as LajtitMasterPage1).SignOutTimeout = 30000;
                if (UserName == "agata" || UserName == "ania")
                {
                    ddlUserName.Enabled = false;
                }
                ddlUserName.SelectedValue = UserName;
                BindMonths();
                BindReport();

            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindReport();
        }
        protected void gvReport_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.SellReportResult r = e.Row.DataItem as Dal.SellReportResult;

                total += r.Amount;
                commision += r.CommisionValue.Value;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = String.Format("{0:C}", total);
                e.Row.Cells[4].Text = String.Format("{0:C}", commision);
                e.Row.Style.Add("text-align", "right");

            }
        }
        private void BindMonths()
        {
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

            ddlMonth.Items.AddRange(items.ToArray()); 
        }
        private void BindReport()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            gvReport.DataSource = oh.GetSellReport(ddlUserName.SelectedValue, Convert.ToDateTime(ddlMonth.SelectedValue));
            gvReport.DataBind();
            
            Dal.AdminUserHelper auh = new Dal.AdminUserHelper();

            Dal.AdminUser au = auh.GetUser(ddlUserName.SelectedValue);

            lblSellCommision.Text = String.Format("{0:0.00}%", au.Commision);
        }
    }
}