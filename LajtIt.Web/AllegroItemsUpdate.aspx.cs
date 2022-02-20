using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("43893252-1117-4c03-bb14-8b79087e5960")]
    public partial class AllegroItemsUpdate : LajtitPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindImports();
                BindSuppliers();
            }
        }
        private void BindSuppliers()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            ddlSuppliers.DataSource = oh.GetSuppliers().OrderByDescending(x => x.SupplierId).ToList();
            ddlSuppliers.DataBind();
             
        }
        private void BindImports()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            ddlImport.DataSource = oh.GetImports();
            ddlImport.DataBind();
        }    
        protected void tmTimer_OnTick(object sender, EventArgs e)
        {
            BindItems(); 
        }
        protected void btnTimer_Click(object sender, EventArgs e)
        { 
            tmTimer.Enabled = !tmTimer.Enabled;
            UpdateTimer(tmTimer.Enabled);
        }
        private void UpdateTimer(bool active)
        {
            tmTimer.Enabled = active;
            imgVerifying.Visible = tmTimer.Enabled;
            btnTimer.Text = tmTimer.Enabled ? "Wyłącz odświeżanie" : "Włącz odświeżanie";

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            int[] ids ;
            if (chbSearchAll.Checked)
                ids = GetProducts().Select(x => x.Id).ToArray();
            else
                ids = WebHelper.GetSelectedIds(gvAllegroItems, "chbOrder");


            string updateCommand = GetUpdateCommand();

            Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
            pc.SetProductCatalogItemsToUpdate(ids, Dal.Helper.AllegroItemUpdateStatus.Pause, "aktualizacja", updateCommand);

            DisplayMessage(String.Format("Dodano aukcje do aktualizowania: {0}", ids.Count()));

            BindItems();

        }

        protected void gvAllegroItems_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAllegroItems.PageIndex = e.NewPageIndex;
            BindItems();
        }
        private string GetUpdateCommand()
        {
            string tmp = "";
            foreach (ListItem chb in chblUpdate.Items)
                if (chb.Selected)
                    tmp += "1";
                else
                    tmp += "0";

            return tmp;
        }
        protected void btnStop_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();

            pc.SetProductCatalogItemsToUpdate(Dal.Helper.AllegroItemUpdateStatus.Verifying,
                Dal.Helper.AllegroItemUpdateStatus.Pause, "");

            DisplayMessage("Zatrzymano");
            BindItems();

            UpdateTimer(false);

        }
        protected void btnRun_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
            string updateCommand = GetUpdateCommand();
            pc.SetProductCatalogItemsToUpdate(Dal.Helper.AllegroItemUpdateStatus.Pause,
                Dal.Helper.AllegroItemUpdateStatus.Verifying,
                updateCommand);

            DisplayMessage("Uruchomiono");
            UpdateTimer(true);

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
            pc.SetProductCatalogItemsToUpdate(Dal.Helper.AllegroItemUpdateStatus.Pause,
                Dal.Helper.AllegroItemUpdateStatus.NotScheduled, "");
            pc.SetProductCatalogItemsToUpdate(Dal.Helper.AllegroItemUpdateStatus.Verifying,
                Dal.Helper.AllegroItemUpdateStatus.NotScheduled, "");

            DisplayMessage("Usunięto z kolejki do aktualizacji");
            UpdateTimer(false);
            BindItems();

        } 
        private void BindItems()
        {
            List<Dal.ProductCatalogAllegroItemsActive> items = GetProducts();


            btnStop.Visible = items.Where(x => x.UpdateStatus == (int)Dal.Helper.AllegroItemUpdateStatus.Verifying).Count() > 0; 
            btnRun.Visible = items.Where(x => x.UpdateStatus == (int)Dal.Helper.AllegroItemUpdateStatus.Pause).Count() > 0; 
            btnDelete.Visible = items.Where(x => x.UpdateStatus == (int)Dal.Helper.AllegroItemUpdateStatus.Verifying
                || x.UpdateStatus == (int)Dal.Helper.AllegroItemUpdateStatus.Pause)
                .Count() > 0; 
        }

        private List<Dal.ProductCatalogAllegroItemsActive> GetProducts()
        {
            Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogAllegroItemsActive> items = pc.GetProductCatalogAllegroItemsActive();

            if (ddlSell.SelectedIndex == 1) // tak
                items = items.Where(x => x.BidCount > 0).ToList();
            if (ddlSell.SelectedIndex == 2) // nie
                items = items.Where(x => x.BidCount == 0).ToList();


            if (ddlIsPromoted.SelectedIndex != 0)
                items = items.Where(x => x.IsPromoted == Convert.ToBoolean(ddlIsPromoted.SelectedValue)).ToList();

            //if (ddlQuantities.SelectedIndex == 2)
            //    items = items.Where(x => x.LeftQuantityAllegro.HasValue
            //        && x.LeftQuantity.HasValue
            //        && x.LeftQuantity.Value != x.LeftQuantityAllegro.Value).ToList();

            //if (ddlQuantities.SelectedIndex == 1)
            //    items = items.Where(x => x.LeftQuantityAllegro.HasValue
            //        && x.LeftQuantity.HasValue
            //        && x.LeftQuantity.Value == x.LeftQuantityAllegro.Value).ToList();

            string q = txbSearch.Text.Trim().ToLower();
            if (!String.IsNullOrEmpty(q))
            {
                items = items.Where(x => x.Name.ToLower().Contains(q) || x.AllegroName.ToLower().Contains(q) || x.Code.ToLower().Contains(q)).ToList();
            }

            if (ddlUserName.SelectedIndex != 0)
                items = items.Where(x => x.UserName == ddlUserName.SelectedItem.Text).ToList();
            if (ddlSuppliers.SelectedIndex != 0)
                items = items.Where(x => x.SupplierId == Convert.ToInt32(ddlSuppliers.SelectedValue)).ToList();

            if (ddlImport.SelectedIndex != 0)
            {

                Dal.OrderHelper oh = new Dal.OrderHelper();
                int[] productIds = oh.GetProductCatalogFromImport(Convert.ToInt32(ddlImport.SelectedValue));

                items = items.Where(x => productIds.Contains(x.ProductCatalogId)).ToList();
            }
            gvAllegroItems.DataSource = items.OrderByDescending(x => x.UpdateStatus).ToList();
            gvAllegroItems.DataBind();
            return items;
        }
        protected void gvAllegroItems_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
         
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogAllegroItemsActive pc = e.Row.DataItem as Dal.ProductCatalogAllegroItemsActive;


                if (pc.IsPromoted.Value)
                    e.Row.Style.Add("background-color", "silver");


                //HyperLink hlCalcuator = e.Row.FindControl("hlCalcuator") as HyperLink;
                // hlCalcuator.NavigateUrl = String.Format("/ProductCalculator.aspx?id={0}", pc.ProductCatalogId);
               // Image imgImage = e.Row.FindControl("imgImage") as Image;
                Literal liId = e.Row.FindControl("LitId") as Literal;
                HyperLink hlProduct = e.Row.FindControl("hlProduct") as HyperLink;
                HyperLink hlProductAllegro = e.Row.FindControl("hlProductAllegro") as HyperLink;
                HyperLink hlAllegroItem = e.Row.FindControl("hlAllegroItem") as HyperLink;
               // HyperLink hlPreview = e.Row.FindControl("hlPreview") as HyperLink;
                Label lblSellPrice = e.Row.FindControl("lblSellPrice") as Label;
                Label lblCode = e.Row.FindControl("lblCode") as Label; 
                Label lblQuantity = e.Row.FindControl("lblQuantity") as Label;
                Label lblLeftQuantity = e.Row.FindControl("lblLeftQuantity") as Label;
                Label lblAllegroPrice = e.Row.FindControl("lblAllegroPrice") as Label;
                Label lblBidCount = e.Row.FindControl("lblBidCount") as Label;

                lblBidCount.Text = String.Format("Sprzedano: {0}", pc.QuantityOrdered);
                if (pc.BidCount > 0)
                {
                    lblBidCount.Font.Bold = true;
                    lblBidCount.ForeColor = System.Drawing.Color.Green;
                    lblBidCount.Style.Add("font-weight", "bold");
               } 
                lblCode.Text = pc.Code; 
                lblQuantity.Text = pc.StartingQuantity.ToString();
                //lblLeftQuantity.Text = pc.LeftQuantity.HasValue ? pc.LeftQuantity.ToString() : "nielimit.";
                liId.Text = String.Format("{0}.", gvAllegroItems.PageIndex * gvAllegroItems.PageSize + e.Row.RowIndex + 1);
                hlAllegroItem.NavigateUrl = String.Format(hlAllegroItem.NavigateUrl, pc.ItemId);
                hlAllegroItem.Text = pc.ItemId.ToString();
               // hlPreview.NavigateUrl = String.Format(hlPreview.NavigateUrl, pc.ProductCatalogId, pc.SupplierId);
                hlProduct.NavigateUrl = String.Format(hlProduct.NavigateUrl, pc.ProductCatalogId); 
                hlProduct.Text = pc.Name;
                hlProductAllegro.NavigateUrl = String.Format(hlProductAllegro.NavigateUrl, pc.ProductCatalogId);
                hlProductAllegro.Text = pc.AllegroName;

                //Bll.ProductCatalogCalculator calc = new LajtIt.Bll.ProductCatalogCalculator(pc.CurrentPrice, pc.AllegroPrice, pc.Rebate.Value, pc.Margin.Value);
                 
                //lblSellPrice.Text = String.Format("{0:C}", calc.SellPriceBrutto);
                //lblAllegroPrice.Text = String.Format("{0:C}", pc.BuyNowPrice);

                //if (!String.IsNullOrEmpty(pc.ImageFullName))
                //    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", pc.ImageFullName);
                //else
                //    imgImage.Visible = false;


                switch (pc.UpdateStatus)
                {
                    case 1: e.Row.FindControl("imgOK").Visible = true; 
                        ((Literal)e.Row.FindControl("litMsg")).Text = String.Format("{0:yyyy/MM/dd HH:mm}<br>{1}", pc.UpdateDate, pc.UpdateComment);
                        break;
                    case 0: e.Row.FindControl("imgError").Visible = true;
                        ((Literal)e.Row.FindControl("litMsg")).Text = String.Format("{0:yyyy/MM/dd HH:mm}<br>{1}", pc.UpdateDate, pc.UpdateComment);
                        break;
                    case 2: e.Row.FindControl("imgVerifying").Visible = true; break;
                    case 3: e.Row.FindControl("imgPause").Visible = true; break;

                }

            }
        }
    }
}