using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using System.Text.RegularExpressions;

namespace LajtIt.Web
{
    [Developer("303d72e6-2880-42d5-a891-f3fa3541f3fc")]
    public partial class AllegroItems : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                calFrom.SelectedDate = DateTime.Now.AddMonths(-1);
                calTo.SelectedDate = DateTime.Now.AddMonths(1);
                txbUserName.Text = "JacekStawicki";
                ddlIsFinished.SelectedIndex=1;
                ddlIsPromoted.SelectedIndex=1;
                BindSuppliers();
                ddlSuppliers.SelectedValue = "3";
                Search();
            }
            else
            {
                calFrom.SelectedDate = DateTime.Parse(txbDateFrom.Text);
                calTo.SelectedDate = DateTime.Parse(txbDateTo.Text);

            }
        }
        private void BindSuppliers()
        { 
            ddlSuppliers.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderByDescending(x => x.SupplierId).ToList();
            ddlSuppliers.DataBind();

        }
        protected void rbtnSearchTypeQuery_Click(object sender, EventArgs e)
        {
            SetControls(true);
        }
        protected void rbtnSearchTypeDates_Click(object sender, EventArgs e)
        {
            SetControls(false);
        }
        private void SetControls(bool selectionByQuery)
        {
            txbAllegroItem.Enabled = txbSearchText.Enabled = selectionByQuery;
            txbDateFrom.Enabled = txbDateTo.Enabled = !selectionByQuery;
        }
        protected void gvAllegroItems_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.AllegroItemsView aiv = e.Row.DataItem as Dal.AllegroItemsView;

                if (aiv.IsPromoted.Value)
                    e.Row.Style.Add("background-color", "silver");

                Image imgIsAuction = e.Row.FindControl("imgIsAuction") as Image;
                Label lblPrice = e.Row.FindControl("lblPrice") as Label;

               // imgIsAuction.Visible = aiv.IsAuction.Value; 
               // lblPrice.Text =  String.Format("{0:C}", aiv.IsAuction.Value? aiv.CurrentPrice: aiv.BuyNowPrice);

            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {

            bool? isFinished = null;
            bool? isSold = null;
            bool? isPromoted = null;
            long? itemId = null;
            string searchQuery = txbSearchText.Text.Trim();
            string allegroItemId = txbAllegroItem.Text.Trim();
            string userName = txbUserName.Text.Trim();
            bool searchByQuery = rbtnSearchTypeQuery.Checked;
            bool? isAuction = null;
            DateTime dateFrom = calFrom.SelectedDate.Value;
            DateTime dateTo = calTo.SelectedDate.Value;
            int categoryType = Convert.ToInt32(ddlCategory.SelectedValue);


            if (!String.IsNullOrEmpty(allegroItemId))
                itemId = Convert.ToInt64(allegroItemId);


            switch ( ddlIsAuction .SelectedIndex)
            {
                case 1: isAuction = true; break;
                case 2: isAuction = false; break;
            }
            switch (ddlIsFinished.SelectedIndex)
            {
                case 1: isFinished = false; break;
                case 2: isFinished = true; break;
            }
            switch (ddlSoldItems.SelectedIndex)
            {
                case 1: isSold = true; break;
                case 2: isSold = false; break;
            }
            switch (ddlIsPromoted.SelectedIndex)
            {
                case 1: isPromoted = true; break;
                case 2: isPromoted = false; break;
            }


            if (rbtnSearchTypeQuery.Checked && String.IsNullOrEmpty(txbUserName.Text.Trim()) && !itemId.HasValue && String.IsNullOrEmpty(searchQuery))
            {
                DisplayMessage("Musisz podać numer przedmiotu lub szukany tekst");
                return;
            }

            try
            {
                Dal.AllegroScan asc = new Dal.AllegroScan();
                var q = asc.GetAllegroItems(
                    searchByQuery,
                    searchQuery,
                    itemId,
                    dateFrom,
                    dateTo,
                    isFinished,
                    isSold,
                    isPromoted,
                    isAuction,
                    userName,
                    categoryType);

                if (ddlSuppliers.SelectedIndex != 0)
                {
                    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

                    long[] itemsFromSupplier = pch.GetProductCatalogAllegroItemsForSupplier(Convert.ToInt32(ddlSuppliers.SelectedValue));
                    q = q.Where(x => itemsFromSupplier.Contains(x.ItemId)).ToList();

                }


                gvAllegroItems.DataSource = q;
                gvAllegroItems.DataBind();

                if (chbRegExp.Checked)
                    BindSummary(q);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError(ex, "Search Allegro Items");
                DisplayMessage(String.Format("Nastąpił błąd wyszukiwania {0}", ex.Message));
            }
        }

        private void BindSummary(List<Dal.AllegroItemsView> q)
        {

            var a = q.Select(x => new
            {
                BuyNowPrice = 0,//x.BuyNowPrice,
                BidCount = x.BidCount,
                Name = GetName(x.Name),
                A=x.Name
            });
          //  .GroupBy(x => x.Name)
           // .Select(g => new {Name =x. g.Sum(x => x.BidCount * x.BuyNowPrice));

            gvSummary.DataSource = a;
                gvSummary.DataBind();
        }

        private string GetName(string p)
        {
            Regex reg = new Regex(txbRegExp.Text.Trim());

            if (reg.IsMatch(p))
                return reg.Match(p).Value;

            return "brak";
        }
    }
}