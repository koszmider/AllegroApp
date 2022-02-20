using System;
using System.Web.UI;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;


namespace LajtIt.Web
{
    public partial class AllegroItemPage : LajtitPage
    {
        private long ItemId { get { return Convert.ToInt64(Request.QueryString["id"].ToString()); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindItem();
        }

        protected void lbtnShowOrder_Click(object sender, EventArgs e)
        {
            long id = Convert.ToInt64(((LinkButton)sender).CommandArgument);

            Dal.AllegroItemHelper aih = new Dal.AllegroItemHelper();

            int? orderId = aih.GetOrderIdByItemOrder(id);

            if (orderId == null)
                DisplayMessage("Nie można odszukać zamówienia. Użyj wyszukiwarki zamówienień");
            else
                Response.Redirect(String.Format("/Order.aspx?id={0}", orderId));

        }

        protected void gvAllegroItemOrders_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.AllegroItemOrdersView aio = (Dal.AllegroItemOrdersView)e.Row.DataItem;

                LinkButton lbtnShowOrder = e.Row.FindControl("lbtnShowOrder") as LinkButton;

                lbtnShowOrder.CommandArgument = aio.Id.ToString();
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dal.AllegroItem ai = new Dal.AllegroItem();
            ai.ItemId = ItemId;
            ai.AutoBidEnabled = chbAutoBidEnabled.Checked;

            Dal.AllegroItemHelper aih = new Dal.AllegroItemHelper();

            aih.SetItemUpdate(ai, UserName);

            DisplayMessage("Zapisano");
        }
        private void BindItem()
        {
            Dal.AllegroItemHelper aih = new Dal.AllegroItemHelper();
            Dal.AllegroItem ai = aih.GetItem(ItemId);


            if (ai == null)
            {
                DisplayMessage("Przedmiot nie został znaleziony");
                return;
            }

            lbName.Text = ai.Name;

            btnSave.Enabled = Enum.GetValues(typeof(Dal.Helper.MyUsers)).Cast<Dal.Helper.MyUsers>().
                Select(x => (long)x).Contains(ai.UserId);


            List<Dal.AllegroItemOrdersView> orders = aih.GetItemOrders(ItemId);
            gvAllegroItemOrders.DataSource = orders;
            gvAllegroItemOrders.DataBind();
        }
    }
}