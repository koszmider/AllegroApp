using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using LajtIt.Bll;
using System.Linq;
using System.Web.UI;
using System.Drawing;

namespace LajtIt.Web.Controls
{
    public partial class ReceiptOrderGrid : LajtitControl
    {

 
        private int OrderId
        {
            set { ViewState["OrderId"] = value; }
            get { return Convert.ToInt32(ViewState["OrderId"]); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void BindReceipts(int orderId)
        {
            OrderId = orderId;

            List<Dal.ReceiptsForOrderFnResult> receipts = Dal.DbHelper.Orders.GetReceiptsFn(OrderId);

            gvUserOrders.DataSource = receipts;
            gvUserOrders.DataBind();
        }


        protected void gvOrderReceipts_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litParType = e.Row.FindControl("litParType") as Literal;
                Literal litParAmount = e.Row.FindControl("litParAmount") as Literal;
                Literal litParDate = e.Row.FindControl("litParDate") as Literal;

                Dal.ReceiptsForOrderFnResult orderReceipt = e.Row.DataItem as Dal.ReceiptsForOrderFnResult;

                litParType.Text = orderReceipt.OrderReceiptTypeName;
                litParAmount.Text = orderReceipt.ReceiptAmount.ToString();
                litParDate.Text = orderReceipt.InsertDate.ToShortDateString();
            }

        }

    }
}