using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("3ade67f7-9401-4441-82a1-9b17ae4dd654")]
    public partial class AllegroPerformance : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (!Page.IsPostBack)
                BindAllegroMyUsers(gvAllegro);
        }

        protected void gvAllegro_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.AllegroUser r = e.Row.DataItem as Dal.AllegroUser;

                Dal.Shop shop = Dal.DbHelper.Shop.GetShopByExternalId(r.UserId);
                if (shop != null)
                {
                    List<Dal.SupplierShop> suppliers = Dal.DbHelper.ProductCatalog.GetSupplierShopsByShopId(shop.ShopId);

                    Label lblShopSuppliers = e.Row.FindControl("lblShopSuppliers") as Label;

                    lblShopSuppliers.Text = String.Format("{0}", suppliers.Where(x => x.IsActive).Count()) ;
                }


                Bll.AllegroRESTHelper.Users.RootUserRating rating = LajtIt.Bll.AllegroRESTHelper.Users.GetUserRatingSummary(r.UserId);

                if (rating == null)
                    return;

                Label lblRecommend          = e.Row.FindControl("lblRecommend") as Label;
                Label lblRecommendUnique    = e.Row.FindControl("lblRecommendUnique") as Label;
                Label lblNotRecommend       = e.Row.FindControl("lblNotRecommend") as Label;
                Label lblNotRecommendUniquel = e.Row.FindControl("lblNotRecommendUnique") as Label;
                Label lblAverage = e.Row.FindControl("lblAverage") as Label;
                Label lblRateDesc = e.Row.FindControl("lblRateDesc") as Label;
                Label lblRateService = e.Row.FindControl("lblRateService") as Label;
                Label lblRateDelivery = e.Row.FindControl("lblRateDelivery") as Label;
                lblRecommend.Text =             String.Format("{0}", rating.recommended.total);
                lblRecommendUnique.Text =       String.Format("{0}", rating.recommended.unique);
                lblNotRecommend.Text =          String.Format("{0}", rating.notRecommended.total);
                lblNotRecommendUniquel.Text = String.Format("{0}", rating.notRecommended.unique);
                lblNotRecommendUniquel.Text = String.Format("{0}", rating.notRecommended.unique);
                lblNotRecommendUniquel.Text = String.Format("{0}", rating.notRecommended.unique);
                lblNotRecommendUniquel.Text = String.Format("{0}", rating.notRecommended.unique);
                lblRateDesc    .Text = String.Format("{0}", rating.averageRates.description);
                lblRateService .Text = String.Format("{0}", rating.averageRates.service);
                lblRateDelivery.Text = String.Format("{0}", rating.averageRates.deliveryCost);

                if (rating.recommended.unique + rating.notRecommended.unique != 0)
                { 
                    lblAverage.Text = String.Format("{0:0.00}%", rating.recommended.unique * 100.00 / (rating.recommended.unique + rating.notRecommended.unique));

                    if (rating.recommended.unique * 100.00 / (rating.recommended.unique + rating.notRecommended.unique) < 98)
                        lblAverage.ForeColor = Color.Red;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //e.Row.Cells[3].Text = String.Format("{0:C}", total);
            }
        }
    }
}