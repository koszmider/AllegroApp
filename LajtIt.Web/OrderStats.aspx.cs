using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("a987d552-60df-44b2-a9b6-54b5956fc246")]
    public partial class OrderStats : LajtitPage
    {
        private int? PaymentType
        {
            get
            {
                if (ViewState["PaymentType"] == null)
                    return null;
                else
                    return Convert.ToInt32(ViewState["PaymentType"].ToString());
            }
            set { ViewState["PaymentType"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                (this.Master as LajtitMasterPage1).SignOutTimeout = 30000;
                BindResults();
            }
        }

        private void BindResults()
        {

            Dal.OrderHelper oh = new Dal.OrderHelper();
            gvOrderStats.DataSource = oh.GetOrderStats(PaymentType);
            gvOrderStats.DataBind();

            gvOrdersByDay.DataSource = oh.GetOrderStatsByDay();
            gvOrdersByDay.DataBind();
        }

        protected void ddlAccount_Changed(object sender, EventArgs e)
        {
            if (((DropDownList)sender).SelectedIndex == 0)
                PaymentType = null;
            else

            PaymentType = Convert.ToInt32(((DropDownList)sender).SelectedValue);
            BindResults();

        }
        protected void gvOrderStats_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

            Dal.OrderStatsFunResult os = e.Row.DataItem as Dal.OrderStatsFunResult;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPayment = e.Row.FindControl("lblPayment") as Label;

                lblPayment.Text = String.Format("{0:C}", os.PaymentNetto);


            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                DropDownList ddlAccount = e.Row.FindControl("ddlAccount") as DropDownList;

                if (PaymentType != null) 
                ddlAccount.SelectedValue = PaymentType.ToString();
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (os.Date == "Razem")
                {
                    e.Row.Style.Add("font-weight", "bold");
                    e.Row.Style.Add("background-color", "silver");
                }

                e.Row.Cells[8].ToolTip = String.Format("VAT zakupy za {1}: {0:C}", os.CostVAT, os.Date);
          
            }
        }
    }
}