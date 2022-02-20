using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class PromotionControl : LajtitControl
    {
        public delegate void SavedEventHandler(object sender, EventArgs e);
        public event SavedEventHandler Saved;

        private int? PromotionGroupId { get {
                if (String.IsNullOrEmpty(Request.QueryString["id"]))
                    return null;
                else
                    return Convert.ToInt32(Request.QueryString["id"].ToString()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                txbStartDate.Text = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                txbEndDate.Text = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            }
            else
            {
                if (txbStartDate.Text != "")
                    calStartDate.SelectedDate = DateTime.Parse(txbStartDate.Text);
                if (txbEndDate.Text != "")
                    calEndDate.SelectedDate = DateTime.Parse(txbEndDate.Text);

            }
        }

        internal void BindPromotion(int promotionGroupId)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            Dal.ProductCatalogPromotionGroup pg = pch.GetPromotionGroup(promotionGroupId);
            calEndDate.SelectedDate = pg.EndDate;
            chbIsActive.Checked = pg.IsActive;
            txbName.Text = pg.Name;
            calStartDate.SelectedDate = pg.StartDate;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogPromotionGroup pg = new Dal.ProductCatalogPromotionGroup()
            {
                EndDate = calEndDate.SelectedDate.Value,
                IsActive = chbIsActive.Checked,
                Name = txbName.Text.Trim(),
                StartDate = calStartDate.SelectedDate.Value
            };
            if (PromotionGroupId.HasValue)
                pg.PromotionGroupId = PromotionGroupId.Value;
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            int id = pch.SetPromotion(pg);

            DisplayMessage(String.Format("Promocja została zapisana. Kliknij <a href='Promotion.aspx?id={0}'>tutaj</a> by dokończyć edycję", id));

            if (Saved != null)
                Saved(this, e);

        }
    }
}