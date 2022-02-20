using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    public partial class AllegroGoal : LajtitPage
    {
        private int GoalId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGoal();
                BindItems(true);
            }
        }

        private void BindGoal()
        {
            Dal.AllegroGoalHelper agh = new Dal.AllegroGoalHelper();
            Dal.AllegroGoalsView goal = agh.GetGoal(GoalId);

            imgUrl.ImageUrl = goal.ImageUrl;
            litName.Text = goal.Name;
            litProductCatalog.Text = String.Format("{0}/{1}", goal.GroupName, goal.ProductCatalog);
            hlAllegroItem.NavigateUrl = String.Format("http://allegro.pl/show_item.php?item={0}", goal.ItemId);
            hlAllegroItem.Text = goal.Name ?? goal.ItemId.ToString();
            chbIsActive.Checked = goal.IsActive;
            litItemsCreated.Text = String.Format("{0}/{1}/{2}", goal.AllItemsCount, goal.AllAuctionsSold, goal.AllItemsOrdered);
            litItemsActive.Text = String.Format("{0}/{1}/{2}", goal.ActiveItemsCount, goal.ActiveAuctionsSold, goal.ActiveItemsOrdered);
        }
        private void BindItems(bool onlyActive)
        {
            Dal.AllegroGoalHelper agh = new Dal.AllegroGoalHelper();
            var q = agh.GetItems(GoalId).AsQueryable();

            if (onlyActive)
                q=q.Where(x=>x.EndingInfo==null|| x.EndingInfo.Value==1);

            gvItems.DataSource = q.OrderByDescending(x=>x.EndingDateTime).ToList();
            gvItems.DataBind();
        }
        protected void cbhIsActive_OnCheckedChanged(object sender, EventArgs e)
        {
            BindItems((sender as CheckBox).Checked);
        }
        protected void gvItems_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.AllegroItem item = e.Row.DataItem as Dal.AllegroItem; 

                HyperLink hlProductAllegroName = e.Row.FindControl("hlProductAllegroName") as HyperLink;
                hlProductAllegroName.NavigateUrl = String.Format("http://allegro.pl/show_item.php?item={0}", item.ItemId);
                hlProductAllegroName.Text = item.Name;

            }
        }
    }
}