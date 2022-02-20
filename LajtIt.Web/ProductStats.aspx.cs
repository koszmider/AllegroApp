using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("0807dd0a-65d9-4e39-9dab-f3eef45b8b6d")]
    public partial class ProductStats : LajtitPage
    {
        private int orderCount = 0, itemsCount = 0;
        private decimal itemsValue = 0;
        private int groupOrderCount = 0, groupItemsCount = 0;
        private decimal groupItemsValue = 0;

        DateTime startDate;
        DateTime endDate;

        protected void Page_Load(object sender, EventArgs e)
        {
            


            if (!Page.IsPostBack)
            {
                (this.Master as LajtitMasterPage1).SignOutTimeout = 30000;
                calFrom.SelectedDate = DateTime.Now.AddMonths(-1);
                calTo.SelectedDate = DateTime.Now;
                BindSuppliers();
            }
            else
            {
                calFrom.SelectedDate = DateTime.Parse(txbDateFrom.Text);
                calTo.SelectedDate = DateTime.Parse(txbDateTo.Text);

                startDate = calFrom.SelectedDate.Value;
                endDate = calTo.SelectedDate.Value.AddDays(1).AddSeconds(-1);
               // txbDateFrom.Text = startDate.ToString(System.Globalization.CultureInfo.CurrentCulture);
               // txbDateTo.Text = endDate.ToString(System.Globalization.CultureInfo.CurrentCulture);
            }
        }

        private void BindSuppliers()
        { 
            ddlSuppliers.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderByDescending(x => x.SupplierId).ToList();
            ddlSuppliers.DataBind();

        }
        protected void gvProductGroup_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogGroupStatsResult product = e.Row.DataItem as Dal.ProductCatalogGroupStatsResult;
                groupOrderCount += product.OrdersCount.Value;
                groupItemsCount += product.Quantity.Value;
                groupItemsValue += product.Amount.Value;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = groupOrderCount.ToString();
                e.Row.Cells[3].Text = groupItemsCount.ToString();
                e.Row.Cells[5].Text = String.Format("{0:C}", groupItemsValue);
            }
        }
        protected void gvStats_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogStatsResult product = e.Row.DataItem as Dal.ProductCatalogStatsResult;
                orderCount += product.OrdersCount.Value;
                itemsCount += product.Quantity.Value;
                itemsValue += product.Amount.Value;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = orderCount.ToString();
                e.Row.Cells[2].Text = itemsCount.ToString();
                e.Row.Cells[3].Text = String.Format("{0:C}", itemsValue);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindStats();
        }
        protected void gvStats_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sd = SortDirection.Ascending;
            if (ViewState["sort"] != null)

                sd = (SortDirection)ViewState["sort"] == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            ViewState["sort"] = sd;


            List<Dal.ProductCatalogStatsResult> items = GetData();
            switch (e.SortExpression)
            {
                case "Name": items = (sd == SortDirection.Ascending) ? items.OrderBy(x => x.Name).ToList() : items.OrderByDescending(x => x.Name).ToList(); break;
                case "OrdersCount": items = (sd == SortDirection.Ascending) ? items.OrderBy(x => x.OrdersCount).ToList() : items.OrderByDescending(x => x.OrdersCount).ToList(); break;
                case "Quantity": items = (sd == SortDirection.Ascending) ? items.OrderBy(x => x.Quantity).ToList() : items.OrderByDescending(x => x.Quantity).ToList(); break;
                case "Amount": items = (sd == SortDirection.Ascending) ? items.OrderBy(x => x.Amount).ToList() : items.OrderByDescending(x => x.Amount).ToList(); break;
            }


            gvStats.DataSource = items;
            gvStats.DataBind();

        }
        protected void gvProductGroup_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sd = SortDirection.Ascending;
            if (ViewState["sortg"] != null)

                sd = (SortDirection)ViewState["sortg"] == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            ViewState["sortg"] = sd;


            List<Dal.ProductCatalogGroupStatsResult> items = GetGroupData();
            switch (e.SortExpression)
            {
            
                case "OrdersCount": items = (sd == SortDirection.Ascending) ? items.OrderBy(x => x.OrdersCount).ToList() : items.OrderByDescending(x => x.OrdersCount).ToList(); break;
                case "Quantity": items = (sd == SortDirection.Ascending) ? items.OrderBy(x => x.Quantity).ToList() : items.OrderByDescending(x => x.Quantity).ToList(); break;
                case "Amount": items = (sd == SortDirection.Ascending) ? items.OrderBy(x => x.Amount).ToList() : items.OrderByDescending(x => x.Amount).ToList(); break;
                case "TotalOrdersCount": items = (sd == SortDirection.Ascending) ? items.OrderBy(x => x.TotalOrdersCount).ToList() : items.OrderByDescending(x => x.TotalOrdersCount).ToList(); break;
                case "TotalQuantity": items = (sd == SortDirection.Ascending) ? items.OrderBy(x => x.TotalQuantity).ToList() : items.OrderByDescending(x => x.TotalQuantity).ToList(); break;
                case "TotalAmount": items = (sd == SortDirection.Ascending) ? items.OrderBy(x => x.TotalAmount).ToList() : items.OrderByDescending(x => x.TotalAmount).ToList(); break;
            }


            gvProductGroup.DataSource = items;
            gvProductGroup.DataBind();

        }

        private List<Dal.ProductCatalogStatsResult> GetData()
        {
            int? supplierId = null;
            if (ddlSuppliers.SelectedIndex != 0)
                supplierId = Convert.ToInt32(ddlSuppliers.SelectedValue);

            SetDates();

            Dal.OrderHelper oh = new Dal.OrderHelper();
            return oh.GetProductStats(startDate, endDate, supplierId);
        }
        private List<Dal.ProductCatalogGroupStatsResult> GetGroupData()
        {

            SetDates();

            int? supplierId = null;
            if (ddlSuppliers.SelectedIndex != 0)
                supplierId = Convert.ToInt32(ddlSuppliers.SelectedValue);
            Dal.OrderHelper oh = new Dal.OrderHelper();
            return oh.GetProductGroupStats(startDate, endDate, supplierId);
        }

        private void SetDates()
        {
            switch (rbtnDateOption.SelectedIndex)
            {
                case 1: startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    endDate = DateTime.Now; break;
                case 2: startDate = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
                    endDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddSeconds(-1); break;
                case 3: startDate = DateTime.Now.AddDays(-30);
                    endDate = DateTime.Now; break;
            }
            lblDates.Text = String.Format("Statystyki za okres: {0:yyyy-MM-dd} - {1:yyyy-MM-dd}", startDate, endDate);
        }
        private void BindStats()
        {
            gvStats.DataSource = GetData();
            gvStats.DataBind();
            gvProductGroup.DataSource = GetGroupData();
            gvProductGroup.DataBind();
        }
    }
}