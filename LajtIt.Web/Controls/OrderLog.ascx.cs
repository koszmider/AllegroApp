using System;
using LajtIt.Bll;

namespace LajtIt.Web.Controls
{
    public partial class OrderLog : LajtitControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void BindLog(int orderId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            gvOrderLog.DataSource = oh.GetOrderLog(orderId);
            gvOrderLog.DataBind();
        }
    }
}