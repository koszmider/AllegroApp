using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
namespace LajtIt.Web
{
    [Developer("edfe26dd-1d2a-44b5-9016-5eec07260387")]
    public partial class AllegroStats : LajtitPage
    {
        private decimal itemsValue = 0;
        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";

        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }
        }




        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                (this.Master as LajtitMasterPage1).SignOutTimeout = 30000;
                BindMonths();
                lsbxMonths.SelectedIndex = 0;
                btnShow_Click(null, null);
            }
        }

        private void BindMonths()
        {
            List<ListItem> items = new List<ListItem>();
            for (int year = DateTime.Now.Year; year >= 2012; year--)
                for (int month = DateTime.Now.Year == year ? DateTime.Now.Month : 12; month >= 1; month--)
                {
                    ListItem item = new ListItem()
                    {
                        Text = String.Format("{0}/{1}", year, month),
                        Value = String.Format("{0}/{1}/{2}", year, month, 1)
                    };
                    items.Add(item);
                }

            lsbxMonths.Items.AddRange(items.ToArray());
        }
       

        protected void btnShow_Click(object sender, EventArgs e)
        {
            SortGridView("Year", "");
        }
        protected void gvStats_OnSorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;

            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, DESCENDING);
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, ASCENDING);
            }

        }

        private void SortGridView(string sortExpression, string DESCENDING)
        {
            List<Dal.AllegroStatsResult> results = BindStats();

            switch(sortExpression)
            {
                case "Year": results = results.OrderByDescending(x => x.Year).ToList(); break;
                case "Month": results = results.OrderByDescending(x => x.Month).ToList(); break;
                case "UserName": results = results.OrderByDescending(x => x.UserName).ToList(); break;
                case "ItemsOrdered": results = results.OrderByDescending(x => x.ItemsOrdered).ToList(); break;
                case "ItemsValue": results = results.OrderByDescending(x => x.ItemsValue).ToList(); break;
                }

            gvStats.DataSource = results;
            gvStats.DataBind();
            
        }
        protected void imbByDay_Click(object sender, EventArgs e)
        {
            
            long userId = Convert.ToInt64(((ImageButton)sender).CommandArgument);
            ViewState["userId"] = userId;

            Dal.AllegroScan allegroScan = new Dal.AllegroScan();
            gvStatsByDay.DataSource = allegroScan.GetItemsSoldByUserByDay(
                userId,
                Convert.ToInt32(lsbxMonths.SelectedItem.Value.Split(new char[] { '/' })[0]),
                Convert.ToInt32(lsbxMonths.SelectedItem.Value.Split(new char[] { '/' })[1]));
            gvStatsByDay.DataBind();
            gvStatsByDay.Visible = true;
        }


        protected void gvStatsByDay_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LajtIt.Dal.AllegroItemsSoldByUserByDayResult aiv = e.Row.DataItem as LajtIt.Dal.AllegroItemsSoldByUserByDayResult;

                HyperLink hlViewAuctions = e.Row.FindControl("hlViewAuctions") as HyperLink;
                hlViewAuctions.NavigateUrl = String.Format("AllegroUsers.aspx?y={0}&m={1}&u={2}&c={3}&d={4}",
                    aiv.Year, aiv.Month, Convert.ToInt32(ViewState["userId"]), "", aiv.Day);
                hlViewAuctions.Text = String.Format("{0:C}", aiv.ItemsValue);
            }
        }
        protected void gvStats_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LajtIt.Dal.AllegroStatsResult aiv = e.Row.DataItem as LajtIt.Dal.AllegroStatsResult;
             

                if (Dal.Helper.GetMyIds().Contains(aiv.UserId ))
                    e.Row.BackColor = System.Drawing.Color.Silver;

                Literal litId = e.Row.Cells[0].FindControl("litId") as Literal;
                litId .Text = (e.Row.RowIndex + 1).ToString();

                HyperLink hlViewAuctions = e.Row.FindControl("hlViewAuctions") as HyperLink;
                hlViewAuctions.NavigateUrl = String.Format("AllegroUsers.aspx?y={0}&m={1}&u={2}",
                    aiv.Year, aiv.Month, aiv.UserId);


                HyperLink hlUserName = e.Row.FindControl("hlUserName") as HyperLink;
                hlUserName.NavigateUrl = String.Format("http://allegro.pl/listing/user.php?us_id={0}",
                    aiv.UserId);
                hlUserName.Text =  aiv.UserName;
            }
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    e.Row.Cells[3].Text = bidsCount.ToString();
            //    e.Row.Cells[4].Text = itemsCount.ToString();
            //    e.Row.Cells[7].Text = String.Format("{0:c}", itemsValue);
            //}
        }
        #region private
         
        private List<Dal.AllegroStatsResult> BindStats()
        {
            
            /*
            int year = Convert.ToInt32(lsbxMonths.SelectedValue.Split(new char[] { '/' })[0]);
            int month = Convert.ToInt32(lsbxMonths.SelectedValue.Split(new char[] { '/' })[1]);
            */

            //List<int> years = new List<int>();
            //List<int> months = new List<int>();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month ;

            if (lsbxMonths.SelectedIndex != -1)
            {
                year = Convert.ToInt32(lsbxMonths.SelectedItem.Value.Split(new char[] { '/' })[0]);
                month = Convert.ToInt32(lsbxMonths.SelectedItem.Value.Split(new char[] { '/' })[1]);
            }
            //foreach (ListItem item in lsbxMonths.Items.Cast<ListItem>().Where(x=>x.Selected))
            //{
            //    years.Add(Convert.ToInt32(item.Value.Split(new char[] { '/' })[0]));
            //    months.Add(Convert.ToInt32(item.Value.Split(new char[] { '/' })[1]));
            //}
            AllegroHelper ah = new AllegroHelper();
            return ah.GetAllegroStats(year, month, Convert.ToDecimal(txbItemsValue.Text));
        }


        #endregion
    }
}