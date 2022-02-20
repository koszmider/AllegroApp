using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;

namespace LajtIt.Web
{
    [Developer("9c00ef62-0d3c-4442-b113-f478973efc4b")]
    public partial class OrdersStatisticsIncome : LajtitPage
    {
        bool hasAdminAccess = false;

        protected void Page_Load(object sender, EventArgs e)
        {
           // hasAdminAccess= HasActionAccess(Guid.Parse("0fab172b-8797-4e91-9698-35b6c92988fd"));
 
            if (!Page.IsPostBack)
            {
               // (this.Master as LajtitMasterPage1).SignOutTimeout = 90000;
                calDateFrom.SelectedDate = DateTime.Now.AddMonths(-1);
                txbDateFrom.Text = calDateFrom.SelectedDate.Value.ToString("yyyy/MM/dd");
                calDateTo.SelectedDate = DateTime.Now;
                txbDateTo.Text = calDateTo.SelectedDate.Value.ToString("yyyy/MM/dd");


            }
            else
            {
                calDateFrom.SelectedDate = DateTime.Parse(txbDateFrom.Text);
                calDateTo.SelectedDate = DateTime.Parse(txbDateTo.Text);
            }
            if (!Page.IsPostBack)
            { 
                if (!Page.IsPostBack)
                {
                    BindOrders("Name", "ASC");
                }
            }


        }

        private class Source
        {
            public int Id { get; set; }
            public string SourceName { get; set; }
        }
        private void BindOrders(string sortExpression, string sortOrder)
        {
            
            ViewState["SortExpression"] = sortExpression;
            ViewState["SortDirection"] = sortOrder;

          

            OrderHelper oh = new OrderHelper();
            var q = oh.GetOrdersProfits(calDateFrom.SelectedDate.Value,
                calDateTo.SelectedDate.Value, 1);
            

            switch(sortExpression)
            {
                case "Name": q = q.OrderBy(x => x.Name).ToList(); break;
                case "Total": q = q.OrderByDescending(x => x.Total).ToList(); break;
                case "TotalIncome": q = q.OrderByDescending(x => x.TotalIncome).ToList(); break;
                case "TotalOutcome": q = q.OrderBy(x => x.TotalOutcome).ToList(); break;
                case "Marza": q = q.OrderByDescending(x => x.Marza).ToList(); break;
                case "Narzut": q = q.OrderByDescending(x => x.Narzut).ToList(); break; 

            }
            gvOrders.DataSource = q;
            gvOrders.DataBind();
          
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindOrders(ViewState["SortExpression"].ToString(), ViewState["SortDirection"].ToString());
        }

        protected void gvOrders_Sorting(object sender, GridViewSortEventArgs e)
        {

            BindOrders(e.SortExpression, e.SortDirection.ToString());
        }
    }
}