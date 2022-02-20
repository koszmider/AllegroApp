using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("1a54ba8f-c57b-4deb-ba8b-269862fe3618")]
    public partial class AllegroMyAuctions : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.JacekStawicki.ToString());



            //Dal.AllegroItem ai = Bll.AllegroScan.AddOrUpdateAuction(3644061199, Dal.Helper.MyUsers.JacekStawicki.ToString());
            //Dal.AllegroScan ascan = new Dal.AllegroScan();
            //ascan.AddOrUpdateAuction(ai);

            //ai = Bll.AllegroScan.AddOrUpdateAuction(3640372535, Dal.Helper.MyUsers.JacekStawicki.ToString());
            //ascan.AddOrUpdateAuction(ai);

            //ai = Bll.AllegroScan.AddOrUpdateAuction(3648098712, Dal.Helper.MyUsers.JacekStawicki.ToString());
            //ascan.AddOrUpdateAuction(ai);

            //ai = Bll.AllegroScan.AddOrUpdateAuction(3650794800, Dal.Helper.MyUsers.JacekStawicki.ToString());
            //ascan.AddOrUpdateAuction(ai);

            //Response.Write(ai.Price.ToString());
            if (!Page.IsPostBack)
                BindAuctions();

        }

        private void BindAuctions()
        {
            Dal.AllegroScan allegroScan = new Dal.AllegroScan();
            gvAuctions.DataSource = allegroScan.GetAllegroMyAuctions();
            gvAuctions.DataBind();
           
        }
        protected void gvAuctions_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.AllegroMyAuctionsResult allegro = (Dal.AllegroMyAuctionsResult)e.Row.DataItem;

                if (allegro.IsPromoted.Value)
                    e.Row.Style.Add("background-color", "silver");


                Image imgImage = e.Row.FindControl("imgImage") as Image;
                HyperLink hlProduct = e.Row.FindControl("hlProduct") as HyperLink;
                HyperLink hlProductAllegro = e.Row.FindControl("hlProductAllegro") as HyperLink;
                HyperLink hlAllegroItem = e.Row.FindControl("hlAllegroItem") as HyperLink;
                HyperLink hlPreview = e.Row.FindControl("hlPreview") as HyperLink;
                Label lblSellPrice = e.Row.FindControl("lblSellPrice") as Label;
                Label lblCode = e.Row.FindControl("lblCode") as Label; 
                Label lblQuantity = e.Row.FindControl("lblQuantity") as Label;
                Label lblLeftQuantity = e.Row.FindControl("lblLeftQuantity") as Label;
                Label lblAllegroPrice = e.Row.FindControl("lblAllegroPrice") as Label;
                Label lblEndHours = e.Row.FindControl("lblEndHours") as Label;
                Label lblEndDateTime = e.Row.FindControl("lblEndDateTime") as Label;
                Label lblAllegroCost = e.Row.FindControl("lblAllegroCost") as Label;
                Label lblIncome = e.Row.FindControl("lblIncome") as Label;
                Label lblHighBidder = e.Row.FindControl("lblHighBidder") as Label;
                Label lblProductCost = e.Row.FindControl("lblProductCost") as Label;
                Label lblBids = e.Row.FindControl("lblBids") as Label;

                lblBids.Text = allegro.NumberOfBids.ToString() ;
                lblHighBidder.Text = allegro.HightBidderLogin;
                lblCode.Text = allegro.Code; 
                lblQuantity.Text = allegro.StartingQuantity.ToString();
                lblLeftQuantity.Text = allegro.LeftQuantity.ToString();
                //liId.Text = String.Format("{0}.", gvAllegroItems.PageIndex * gvAllegroItems.PageSize + e.Row.RowIndex + 1);
                hlAllegroItem.NavigateUrl = String.Format(hlAllegroItem.NavigateUrl, allegro.ItemId);
                hlAllegroItem.Text = allegro.ItemId.ToString();
                hlPreview.NavigateUrl = String.Format(hlPreview.NavigateUrl, allegro.ProductCatalogId, allegro.SupplierId);
                hlProduct.NavigateUrl = String.Format(hlProduct.NavigateUrl, allegro.ProductCatalogId);
                hlProduct.Text = allegro.ProductName;
                hlProductAllegro.NavigateUrl = String.Format(hlProductAllegro.NavigateUrl, allegro.ItemId);
                hlProductAllegro.Text = allegro.AllegroName;
                lblEndDateTime.Text = String.Format("{0:yyyy/MM/dd HH:mm}", allegro.EndingDateTime);
                TimeSpan ts = allegro.EndingDateTime.Value - DateTime.Now;

                if (ts.Days == 0)
                {
                    lblEndHours.Style.Add("color", "red");
                    lblEndHours.Style.Add("font-size", "14pt");
                    lblEndHours.Text = String.Format("{1}g, {2}m", ts.Days, ts.Hours, ts.Minutes);
                }
                else
                    lblEndHours.Text = String.Format("{0}d, {1}g, {2}m", ts.Days, ts.Hours, ts.Minutes);

                // Bll.ProductCatalogCalculator calc = new LajtIt.Bll.ProductCatalogCalculator(allegro.CurrentPrice, allegro.AllegroPrice, allegro.Rebate, allegro.Margin);

                lblSellPrice.Text = "0";// zmienic sprawdzic co to za cena String.Format("{0:C}", Bll.ProductCatalogCalculator1.BruttoValue(allegro.CurrentPrice));
                lblAllegroPrice.Text = String.Format("{0:C}", allegro.Price);

               // calc = new LajtIt.Bll.ProductCatalogCalculator(allegro.CurrentPrice, allegro.Price, allegro.Rebate, allegro.Margin);


                //decimal income = allegro.StartingQuantity.Value * 
                //    (allegro.Price  - Bll.ProductCatalogCalculator1.BruttoValue(allegro.CurrentPrice))
                //    + allegro.AllegroTotalCost;
                // lblIncome.Text = String.Format("{0:C}", income);
                // lblAllegroCost.Text = String.Format("{0:C}", allegro.AllegroTotalCost);

                // lblProductCost.Text = String.Format("{0:C}", allegro.StartingQuantity.Value * Bll.ProductCatalogCalculator1.BruttoValue(allegro.CurrentPrice));
                // if (income < 0)
                //{
                //    lblIncome.Style.Add("color", "red");
                //}
                //else

                //    lblIncome.Style.Add("color", "green");

                if (!String.IsNullOrEmpty(allegro.ImageFullName))
                    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", allegro.ImageFullName);
                else
                    imgImage.Visible = false;

            }
        }
    }
}