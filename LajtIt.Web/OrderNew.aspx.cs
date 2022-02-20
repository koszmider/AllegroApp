using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("60da33ef-94c1-430c-9fda-a7daf811a01c")]
    public partial class OrderNew : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnOrderNew_Click(object sender, EventArgs e)
        {
            Bll.OrderHelper oh = new Bll.OrderHelper();
            int orderId = oh.SetOrderNew(this.UserShopId);

            Response.Redirect(String.Format("/Order.aspx?id={0}", orderId));
        }
        protected void lbtnCreateOrder_Click(object sender, EventArgs e)
        {
            int orderId = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            int newOrderId = CreateNewOrder(orderId, this.UserShopId, false, false);
            Response.Redirect(String.Format("/Order.aspx?id={0}", newOrderId));

        }

        public static int CreateNewOrder(int orderId, int shopId, bool keepOrignalOrderInsertDate, bool checkForPayments)
        {
            Bll.OrderHelper oh = new Bll.OrderHelper();
            int newOrderId = oh.SetOrderNew(orderId, shopId, keepOrignalOrderInsertDate, checkForPayments);
          
            return newOrderId;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            gvClients.DataSource = oh.GetClients(txbName.Text.Trim());
            gvClients.DataBind();

            pOrderNew.Visible = true;
        }
    }
}