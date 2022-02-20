using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;

namespace LajtIt.Web
{
    [Developer("1c324b2c-6de1-4f9f-ad6c-4df5d1ac955c")]
    public partial class OrdersStatistics : LajtitPage
    {
        bool hasAdminAccess = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            hasAdminAccess= HasActionAccess(Guid.Parse("0fab172b-8797-4e91-9698-35b6c92988fd"));
            chbShowLabels.Visible = hasAdminAccess ;
            if (!Page.IsPostBack)
            {
               // (this.Master as LajtitMasterPage1).SignOutTimeout = 90000;
                calDateFrom.SelectedDate = DateTime.Now.AddMonths(-12);
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
                    BindReport();
                }
            }


        }

        private class Source
        {
            public int Id { get; set; }
            public string SourceName { get; set; }
        }
        private void BindReport()
        {
            bool completed = rbFinished.Checked;
            bool groupSources = rbGroupSources1.Checked;

            OrderHelper oh = new OrderHelper();
            var q = oh.GetOrdersBySourceStats(calDateFrom.SelectedDate.Value,
                calDateTo.SelectedDate.Value,
                completed,
                groupSources);
            ShopHelper sh = new ShopHelper();

            var shops = Dal.DbHelper.Shop.GetShops().Where(x=>x.ShopType.CanCreateOrders).OrderBy(x=>x.Name).ToList();
            var shopGroups = Dal.DbHelper.Shop.GetShopTypes().Where(x => x.CanCreateOrders).OrderBy(x => x.Name).ToList();

            if (!Page.IsPostBack)
            {
                lbxGroupSources0.DataSource = shops;
                lbxGroupSources0.DataBind();
                lbxGroupSources1.DataSource = shopGroups;
                lbxGroupSources1.DataBind();
                foreach (ListItem item in lbxGroupSources0.Items)
                    item.Selected = true;
                foreach (ListItem item in lbxGroupSources1.Items)
                    item.Selected = true;
            }

            int[] selectedSourceTypes = lbxGroupSources0.Items.Cast<ListItem>().Where(x => x.Selected == true).Select(x => Convert.ToInt32(x.Value)).ToArray();
            if (groupSources)
                selectedSourceTypes = lbxGroupSources1.Items.Cast<ListItem>().Where(x => x.Selected == true).Select(x => Convert.ToInt32(x.Value)).ToArray();



            var sourceTypesFiltered = shops.Where(x => selectedSourceTypes.Contains(x.ShopId))
                .Select(x => new Source()
                {
                    Id = x.ShopId,
                    SourceName = x.Name
                })
                    .ToList();
            if (groupSources)
                sourceTypesFiltered = shopGroups.Where(x => selectedSourceTypes.Contains(x.ShopTypeId))
                .Select(x => new Source()
                {
                    Id = x.ShopTypeId,
                    SourceName = x.Name
                })
                    .ToList();

            Chart1.Legends.Add(new System.Web.UI.DataVisualization.Charting.Legend()
            {
                Docking = System.Web.UI.DataVisualization.Charting.Docking.Bottom

            });
            Chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;

            Chart1.ChartAreas[0].AxisX.Interval = 1;
            Chart1.ChartAreas[0].AxisY.LabelStyle.Enabled = hasAdminAccess;
            //Chart1.ChartAreas[0].AxisY.Interval = 25;

            var dates = q.OrderBy(x => x.Year).ThenBy(x => x.Month).Select(x => x.YearMonth).Distinct();

            foreach (var type in sourceTypesFiltered)
            {
                Chart1.Series.Add(type.SourceName);
                Chart1.Series[type.SourceName].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                Chart1.Series[type.SourceName].LegendText = type.SourceName;
                Chart1.Series[0].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.String;
                Chart1.Series[type.SourceName].IsValueShownAsLabel = hasAdminAccess && chbShowLabels.Checked;
                Chart1.Series[type.SourceName].IsVisibleInLegend = true;
            }

            foreach (string date in dates)
            {
                foreach (var type in sourceTypesFiltered)
                {
                    Dal.OrdersSourceTypeStatsResult p = q.Where(x => x.ShopId == type.Id && x.YearMonth == date).FirstOrDefault();

                    if (p == null)
                        Chart1.Series[type.SourceName].Points.AddXY(date, 0);
                    else
                        if (rbByCount.Checked)
                        Chart1.Series[type.SourceName].Points.AddXY(p.YearMonth, p.OrdersCount);
                    else
                        Chart1.Series[type.SourceName].Points.AddXY(p.YearMonth, p.Amount.Value);


                    // var q = oh.GetCostsStats().Where(x => x.CostTypeId == Convert.ToInt32(ddlCostTypeSearch.SelectedValue)).ToList();

                    //foreach (var p in serie.OrderBy(x => x.Year).ThenBy(x => x.Month))
                    //{
                    //    Chart1.Series[type.SourceType].Points.AddXY(p.YearMonth, p.OrdersCount);
                    //}
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindReport();
        }
    }
}