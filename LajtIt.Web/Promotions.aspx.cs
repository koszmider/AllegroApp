using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("9962d63a-8e05-4db3-ba92-9d888972ed50")]
    public partial class Promotions : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
 
                BindPromotions();

                
            }
          
        }

        protected void PromotionAdded(object sender, EventArgs e)
        {
            BindPromotions();

        }

        private void BindPromotions()
        {
            Dal.PromotionHelper ph = new Dal.PromotionHelper();

            gvPromotions.DataSource = ph.GetPromotions();
            gvPromotions.DataBind();
        }

        protected void lbtnPromotionAdd_Click(object sender, EventArgs e)
        {
            Dal.Promotion promotion = new Dal.Promotion()
            {
                EndDate = null,
                InsertDate = DateTime.Now,
                InsertUser = UserName,
                IsActive = false,
                IsWatekmarkActive = false,
                Name = String.Format("Nowa promocja {0:yyyyMMdd HH:mm}", DateTime.Now),
                StartDate = DateTime.Now.AddDays(7)
            };

            Dal.PromotionHelper ph = new Dal.PromotionHelper();
            Response.Redirect(String.Format("Promotion.aspx?id={0}", ph.SetPromotion(promotion)));


        }
    }
}