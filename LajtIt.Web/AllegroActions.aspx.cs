using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("c945d553-26a7-4b8a-8a36-80359fb7ce8e")]
    public partial class AllegroActions: LajtitPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlStatus.SelectedIndex = 1;
                BindItems(); 
            }
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
            BindItems();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            //int[] ids = WebHelper.GetSelectedIds(gvAllegroItems, "chbOrder");
            //string updateCommand = GetUpdateCommand();

            //Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
            //pc.SetProductCatalogItemsToUpdate(ids, Dal.Helper.AllegroItemUpdateStatus.Pause, null, updateCommand);

            //DisplayMessage(String.Format("Dodano aukcje do aktualizowania: {0}", ids.Count()));

            //BindItems();

        }

  
        protected void btnStop_Click(object sender, EventArgs e)
        {
           throw new   NotImplementedException();
           // Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();

           // pc.SetProductCatalogItemsToUpdate(Dal.Helper.AllegroItemUpdateStatus.Verifying,
           //     Dal.Helper.AllegroItemUpdateStatus.Pause, "");

           // DisplayMessage("Zatrzymano");
           // BindItems();

           // UpdateTimer(false);

        }
        protected void btnRun_Click(object sender, EventArgs e)
        {
            //Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
            //string updateCommand = GetUpdateCommand();
            //pc.SetProductCatalogItemsToUpdate(Dal.Helper.AllegroItemUpdateStatus.Pause,
            //    Dal.Helper.AllegroItemUpdateStatus.Verifying,
            //    updateCommand);

            //DisplayMessage("Uruchomiono");
            //UpdateTimer(true);

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
            pc.SetAllegroActionDelete(WebHelper.GetSelectedIds<int>(gvAllegroItems, "chbOrder"));

            DisplayMessage("Usunięto");
            UpdateTimer(false);
            BindItems();

        } 
        private void BindItems()
        {
            Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
            List<Dal.AllegroActionView> items = pc.GetAllegroActions();

            switch (ddlStatus.SelectedIndex)
            {
                case 1:
                    items = items.Where(x => !x.IsProcessed).ToList();break;
                case 2:
                    items = items.Where(x => x.IsProcessed).ToList();break;
            }
            gvAllegroItems.DataSource = items;
            gvAllegroItems.DataBind();


            //List<Dal.ProductCatalogAllegroItemsActive> items = pc.GetProductCatalogAllegroItemsActive();

            //if (ddlSell.SelectedIndex == 1) // tak
            //    items = items.Where(x => x.BidCount > 0).ToList();
            //if (ddlSell.SelectedIndex == 2) // nie
            //    items = items.Where(x => x.BidCount == 0).ToList();


            //if (ddlIsPromoted.SelectedIndex != 0)
            //    items = items.Where(x => x.IsPromoted == Convert.ToBoolean(ddlIsPromoted.SelectedValue)).ToList();

            //if (ddlQuantities.SelectedIndex == 2)
            //    items = items.Where(x => x.LeftQuantityAllegro.HasValue
            //        && x.LeftQuantity.HasValue
            //        && x.LeftQuantity.Value != x.LeftQuantityAllegro.Value).ToList();

            //if (ddlQuantities.SelectedIndex == 1)
            //    items = items.Where(x => x.LeftQuantityAllegro.HasValue
            //        && x.LeftQuantity.HasValue
            //        && x.LeftQuantity.Value == x.LeftQuantityAllegro.Value).ToList();

            //string q = txbSearch.Text.Trim().ToLower();
            //if (!String.IsNullOrEmpty(q))
            //{
            //    items = items.Where(x => x.Name.ToLower().Contains(q) || x.AllegroName.ToLower().Contains(q) || x.Code.ToLower().Contains(q)).ToList();
            //}

            //if (ddlUserName.SelectedIndex != 0)
            //    items = items.Where(x => x.UserName == ddlUserName.SelectedItem.Text).ToList();
            //if (ddlSuppliers.SelectedIndex != 0)
            //    items = items.Where(x => x.SupplierId == Convert.ToInt32(ddlSuppliers.SelectedValue)).ToList();

            //if (ddlImport.SelectedIndex != 0)
            //{

            //    Dal.OrderHelper oh = new Dal.OrderHelper();
            //    int[] productIds = oh.GetProductCatalogFromImport(Convert.ToInt32(ddlImport.SelectedValue));

            //    items = items.Where(x => productIds.Contains(x.ProductCatalogId)).ToList();
            //}
            //gvAllegroItems.DataSource = items.OrderByDescending(x=>x.UpdateStatus);
            //gvAllegroItems.DataBind();


            //btnStop.Visible = items.Where(x => x.UpdateStatus == (int)Dal.Helper.AllegroItemUpdateStatus.Verifying).Count() > 0; 
            //btnRun.Visible = items.Where(x => x.UpdateStatus == (int)Dal.Helper.AllegroItemUpdateStatus.Pause).Count() > 0; 
            //btnDelete.Visible = items.Where(x => x.UpdateStatus == (int)Dal.Helper.AllegroItemUpdateStatus.Verifying
            //    || x.UpdateStatus == (int)Dal.Helper.AllegroItemUpdateStatus.Pause)
            //    .Count() > 0; 
        }
        protected void gvAllegroItems_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
         
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.AllegroActionView pc = e.Row.DataItem as Dal.AllegroActionView;


                //if (pc.IsPromoted.Value)
                //    e.Row.Style.Add("background-color", "silver");


                //HyperLink hlCalcuator = e.Row.FindControl("hlCalcuator") as HyperLink;
                // hlCalcuator.NavigateUrl = String.Format("/ProductCalculator.aspx?id={0}", pc.ProductCatalogId);
                //Image imgImage = e.Row.FindControl("imgImage") as Image;
                Literal liId = e.Row.FindControl("LitId") as Literal;
                HyperLink hlProduct = e.Row.FindControl("hlProduct") as HyperLink;
                HyperLink hlProductAllegro = e.Row.FindControl("hlProductAllegro") as HyperLink;
                HyperLink hlAllegroItem = e.Row.FindControl("hlAllegroItem") as HyperLink;
                HyperLink hlPreview = e.Row.FindControl("hlPreview") as HyperLink;
                //Label lblSellPrice = e.Row.FindControl("lblSellPrice") as Label;
                //Label lblCode = e.Row.FindControl("lblCode") as Label;
                //Label lblCodeSupplier = e.Row.FindControl("lblCodeSupplier") as Label;
                //Label lblQuantity = e.Row.FindControl("lblQuantity") as Label;
                //Label lblLeftQuantity = e.Row.FindControl("lblLeftQuantity") as Label;
                //Label lblAllegroPrice = e.Row.FindControl("lblAllegroPrice") as Label;
                //Label lblBidCount = e.Row.FindControl("lblBidCount") as Label;

               // lblBidCount.Text = String.Format("Sprzedano: {0}", pc.QuantityOrdered);
               // if (pc.BidCount > 0)
               // {
               //     lblBidCount.Font.Bold = true;
               //     lblBidCount.ForeColor = System.Drawing.Color.Green;
               //     lblBidCount.Style.Add("font-weight", "bold");
               //} 
               // lblCode.Text = pc.Code;
               // lblCodeSupplier.Text = pc.CodeSupplier;
               // lblQuantity.Text = pc.StartingQuantity.ToString();
               // lblLeftQuantity.Text = pc.LeftQuantity.HasValue ? pc.LeftQuantity.ToString() : "nielimit.";
                //liId.Text = String.Format("{0}.", gvAllegroItems.PageIndex * gvAllegroItems.PageSize + e.Row.RowIndex + 1);
                hlAllegroItem.NavigateUrl = String.Format(hlAllegroItem.NavigateUrl, pc.ItemId);
                hlAllegroItem.Text = pc.ItemId.ToString();
                hlPreview.NavigateUrl = String.Format(hlPreview.NavigateUrl, pc.ProductCatalogId, pc.SupplierId);
                hlPreview.Text = "--";
                //hlProduct.NavigateUrl = String.Format(hlProduct.NavigateUrl, pc.ProductCatalogId);
                //hlProduct.Text = pc.Name;
                //hlProductAllegro.NavigateUrl = String.Format(hlProductAllegro.NavigateUrl, pc.ProductCatalogId);
                //hlProductAllegro.Text = pc.AllegroName;

               
            }
        }
    }
}