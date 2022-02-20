using System;
using System.Web.UI;
using LajtIt.Bll;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LajtIt.Dal;
using System.Text;
using System.Drawing;

namespace LajtIt.Web
{
    [Developer("261c26b5-e1fc-4f27-8165-c4acf9a2a137")]
    public partial class OrderCost : LajtitPage
    {
        private int OrderId { get { return Convert.ToInt32(Request.QueryString["id"].ToString()); } }

        decimal gain = 0;
        decimal loss = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            hlOrder.NavigateUrl = String.Format(hlOrder.NavigateUrl, OrderId);

            gvOrderCost.DataSource = Dal.DbHelper.Orders.GetOrderCost(OrderId);
            gvOrderCost.DataBind();

            lblGain.Text = String.Format("{0:C}", gain);
            lblLoss.Text = String.Format("{0:C}", -loss);
            lblGainRate.Text = String.Format("{0:C}", gain + loss);

            //if (loss != 0)
            //    lblGainRatePerc.Text = String.Format("{0:0.00}%", (1 - gain  / -loss) * 100);
            //else
            //    lblGainRatePerc.Text = String.Format("{0:0.00}%", 0); 

            decimal marza = 0;
            if (gain != 0)
                marza = (gain + loss) / gain;
            decimal narzut = 0;
            if(loss != 0)
                narzut = -(gain + loss) / loss;


            lblMarza.Text = String.Format("{0:0.00}%", (double)marza * 100.00);
            lblNarzut.Text = String.Format("{0:0.00}%", (double)narzut * 100.00);
            if (gain + loss < 0)
                lblGainRate.ForeColor = Color.Red;
            else
                lblGainRate.ForeColor = Color.Green;
        }

        protected void gvOrderCost_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.OrderCostFnResult cost = e.Row.DataItem as Dal.OrderCostFnResult;

                if (cost.PriceTotal > 0)
                    gain += cost.PriceTotal.Value;
                if (cost.PriceTotal < 0)
                    loss += cost.PriceTotal.Value;

            }
        }
    }
}