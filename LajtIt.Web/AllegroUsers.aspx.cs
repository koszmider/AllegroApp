using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;

namespace LajtIt.Web
{
    [Developer("ade86098-9770-475a-bd1d-42e1df6a8863")]
    public partial class AllegroUsers : LajtitPage
    {
        private int itemsCount = 0;
        private int bidsCount = 0;
        private decimal itemsValue = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { 
                ProcessQueryParameters();
            }
        }

        private void ProcessQueryParameters()
        {
            if (Request.QueryString["u"] != null)
                txbUserName.Text = Request.QueryString["u"];

            if (Request.QueryString["y"] != null && Request.QueryString["m"] != null)
            {
                chbDateRangeFilter.Text = String.Format("Pokazuj sprzedane przedmioty z miesiąca {0}/{1}",
                    Request.QueryString["y"], Request.QueryString["m"]);
                chbDateRangeFilter.Visible = true;
                chbDateRangeFilter.Checked = true;
                ViewState["m"] = Request.QueryString["m"];
                ViewState["y"] = Request.QueryString["y"];
                ViewState["d"] = Request.QueryString["d"] ?? "0";

                if (ViewState["d"].ToString() == "0")
                    chbDateRangeFilter.Text = String.Format("Pokazuj sprzedane przedmioty z miesiąca {0}/{1}",
                        Request.QueryString["y"], Request.QueryString["m"]);
                else
                    chbDateRangeFilter.Text = String.Format("Pokazuj sprzedane przedmioty z dnia {0}/{1}/{2}",
                        Request.QueryString["y"], Request.QueryString["m"], Request.QueryString["d"]);

                ddlSell.SelectedIndex = 1;
                ddlAuctionType.SelectedIndex = 0;

                BindAuctions();
            }
        }
         
        protected void chbDateRangeFilter_OnCheckedChanged(object sender, EventArgs e)
        {
            chbDateRangeFilter.Visible = false;
            chbDateRangeFilter.Checked = false;
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindAuctions();
        }

        private void BindAuctions()
        {

            throw new NotImplementedException();
            //Bll.AllegroScan allegroScan = new Bll.AllegroScan();

            //List<Bll.Dto.UserAuctions> auctions = new List<Bll.Dto.UserAuctions>();

            //long userId =  Convert.ToInt64(txbUserName.Text.Trim());
            //if (chbDateRangeFilter.Checked)
            //{
            //    int year = Convert.ToInt32(ViewState["y"]);
            //    int month = Convert.ToInt32(ViewState["m"]);
            //    int day = Convert.ToInt32(ViewState["d"]);

            //    auctions.AddRange(allegroScan.GetAuctionsByDate(userId, year, month, day));
            //}
            //else
            //{
            //    auctions.AddRange(allegroScan.GetAuctions(
            //        userId, 
            //        Convert.ToInt64(ddlAuctionType.SelectedValue),
            //    Convert.ToInt32(ddlSell.SelectedValue)                 ));
            //}
            //gvUserAuctions.DataSource = auctions;
            //gvUserAuctions.DataBind();
        }

        protected void gvUserAuctions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Bll.Dto.UserAuctions aiv = e.Row.DataItem as Bll.Dto.UserAuctions;
                itemsCount += aiv.ItemsOrdered;
                bidsCount += aiv.BidCount;
                itemsValue += aiv.ItemsValue;


                Literal litStatus = e.Row.FindControl("litStatus") as Literal;
                litStatus.Text = String.Format("{0}<br/>{1}", aiv.EndingInfo.Value == 1 ? "Aktywny" : "Nieaktywny",
                    aiv.EndingDateTime);

                Literal litCategory = e.Row.FindControl("litCategory") as Literal;
                litCategory.Text = aiv.CategoryName;
                if (aiv.IsPromoted)
                    e.Row.Style.Add("background-color", "silver");

                //if (aiv.Options.HasValue)
                //{
                //    Bll.Enums.AllegroOptions options = (Bll.Enums.AllegroOptions)Enum.Parse(typeof(Bll.Enums.AllegroOptions), aiv.Options.Value.ToString());


                //    if (LajtIt.Bll.Helper.HasAllegroOption(aiv.Options.Value, Bll.Enums.AllegroOptions.ItemCategoryPromoted))
                //    {
                //        Label litPromoted = e.Row.FindControl("litPromoted") as Label;
                //        litPromoted.Text = "Promowana";
                //        litPromoted.ForeColor = System.Drawing.Color.Red;
                //    }
                //}
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = String.Format("{0}/{1}",bidsCount, itemsCount);
                e.Row.Cells[6].Text = itemsValue.ToString();
            }
        }

    }
}