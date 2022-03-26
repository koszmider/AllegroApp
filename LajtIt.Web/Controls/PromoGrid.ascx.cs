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
    public partial class PromoGrid : LajtitControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void BindPromos()
        {
            Dal.PromoHelper ph = new Dal.PromoHelper();
            List<Dal.Promo> promos = ph.GetPromotions();

            gvPromos.DataSource = promos;
            gvPromos.DataBind();
        }


        protected void gvPromos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litDesc = e.Row.FindControl("litDesc") as Literal;
                Literal litPercentValue = e.Row.FindControl("litPercentValue") as Literal;
                Literal litStartDate = e.Row.FindControl("litStartDate") as Literal;
                Literal litEndDate = e.Row.FindControl("litEndDate") as Literal;
                Literal litIsActive = e.Row.FindControl("litIsActive") as Literal;
                Literal litIsGoingOn = e.Row.FindControl("litIsGoingOn") as Literal;

                Dal.Promo promos = e.Row.DataItem as Dal.Promo;

                litDesc.Text = promos.Description;
                litPercentValue.Text = promos.PercentValue.ToString();
                litStartDate.Text = promos.StartDate.ToString();
                if (promos.EndDate != null)
                    litEndDate.Text = promos.EndDate.ToString();
                if (promos.IsActive)
                    litIsActive.Text = "Tak";
                else
                    litIsActive.Text = "Nie";
                if (promos.IsGoingOn)
                    litIsGoingOn.Text = "Tak";
                else
                    litIsGoingOn.Text = "Nie";
            }

        }

        protected void gvPromos_OnRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("IdDelete"))
            {
                int promoId = Convert.ToInt32(e.CommandArgument);

                Bll.PromoHelper phbll = new Bll.PromoHelper();
                Dal.PromoHelper ph = new Dal.PromoHelper();
                
                Dal.Promo promo = ph.GetPromotion(promoId);
                int[] productIds = ph.GetPromotionProductsIds(promo.PromotionId);
                phbll.StopPromotion(promo, productIds);
                ph.DeletePromotion(promo);
                Response.Redirect("Promotions.aspx");
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {

        }
    }
}