using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using static LajtIt.Bll.ShopHelper;

namespace LajtIt.Web.Controls
{
    public partial class SupplierProducer : LajtitControl
    {

        public delegate void ReloadedEventHandler();
        public event ReloadedEventHandler Reloaded;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void BindForm()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier supplier = oh.GetSupplier(SupplierId);

            List<Dal.ShopsFnResult> shopsView = oh.GetShops(SupplierId).Where(x => x.CanExportProducts).ToList();

            ddlOrderingType.DataSource = oh.GetSupplierOrderingTypes();
            ddlOrderingType.DataBind();

            lbxShops.DataSource = shopsView.Where(x => x.SupplierId.HasValue == false).ToList();
            lbxShops.DataBind();
            lbxShopsOn.DataSource = shopsView.Where(x => x.SupplierId.HasValue == true).ToList();
            lbxShopsOn.DataBind();

            List<Dal.SupplierOwner> so = oh.GetSupplierOwners().OrderBy(x => x.Name).ToList();
            ddlSupplierOwner.DataSource = so;
            ddlSupplierOwner.DataBind();

            ddlSupplierOwner.SelectedValue = supplier.SupplierOwnerId.ToString();

            ddlImportType.DataSource = oh.GetSupplierImportTypes();
            ddlImportType.DataBind();

            ddlImportType.SelectedIndex = ddlImportType.Items.IndexOf(ddlImportType.Items.FindByValue(supplier.ImportTypeId.ToString()));
            txbImportComment.Text = supplier.ImportComment;
            txbImportUrl.Text = supplier.ImportUrl;

            if (txbImportUrl.Text != "")
            {
                hlUrl.NavigateUrl = txbImportUrl.Text;
                hlUrl.Visible = true;
            }
            litSupplier.Text = supplier.Name;

            txbMargin.Text = String.Format("{0:0.00}", supplier.Margin * 100M);
            txbName.Text = supplier.Name;
            txbRebate.Text = String.Format("{0:0.00}", supplier.Rebate * 100M);
            //txbMax.Text = String.Format("{0}", supplier.MaxProductsForAllegro); 
            //ddlAllegroPriceTypeId.SelectedIndex = ddlAllegroPriceTypeId.Items.IndexOf(ddlAllegroPriceTypeId.Items.FindByValue(supplier.AllegroPriceTypeId.ToString()));
            chbIsActive.Checked = supplier.IsActive;
            chbShowSupplierInAllegro.Checked = supplier.ShowSupplierInAllegro.Value;
            ddlOrderingType.SelectedValue = supplier.OrderingTypeId.ToString();
            txbB2bEmail.Text = supplier.B2bEmail;
            txbB2bUrl.Text = supplier.B2bUrl;
            //ucShopCategoryControl.SetCategoryId(Dal.Helper.ShopType.ClickShop, supplier);
            //txbShopProducerId.Text = supplier.ShopProducerId.HasValue ?supplier.ShopProducerId.ToString():""; 
            //chbAutoCreateUpdateShop.Checked = supplier.AutoCreateUpdateShop;
            //txbProductNameTemplate.Text = supplier.ProductNameTemplate;
            //txbAllegroNameTemplate.Text = supplier.AllegroNameTemplate;
            //chbOnlineShopLockRebates.Checked = supplier.OnlineShopLockRebates;
            //txbShopSellDiscount.Text = String.Format("{0:0.00}", supplier.ShopSellDiscount * 100M);
            //txbAllegroSellDiscount.Text = String.Format("{0:0.00}", supplier.AllegroSellDiscount * 100M);
            //txbAllegroCommision.Text = String.Format("{0:0.00}", supplier.AllegroCommision * 100M); 
            chbIsDropShippingAvailable.Checked = supplier.IsDropShippingAvailable;
            if (supplier.IsQuantityTrackingAvailable.HasValue)
            {
                chbIsQuantityTrackingAvailable.Checked = supplier.IsQuantityTrackingAvailable.Value;
            }
            //if (supplier.AllegroDiscountValue.HasValue)
            //{
            //    txbAllegroDiscountValue.Text = supplier.AllegroDiscountValue.ToString();
            //    ddlAllegroDiscountQty.SelectedIndex = ddlAllegroDiscountQty.Items.IndexOf(ddlAllegroDiscountQty.Items.FindByValue(supplier.AllegroDiscountQty.ToString()));

            //}
            //else
            //    txbAllegroDiscountValue.Text = "";

            switch (supplier.RoundPriceTypeId)
            {
                case 0: ddlRoundPriceType.SelectedIndex = 0; break;
                case 1: ddlRoundPriceType.SelectedIndex = 1; break;

            }



            GetShopDeliveries();

            ddlShopDelivery.SelectedValue = supplier.DeliveryId.ToString();
            BindAllegroDeliveryTypes();

            ddlAllegroDeliveryType.SelectedIndex = ddlAllegroDeliveryType.Items.IndexOf(ddlAllegroDeliveryType.Items.FindByValue(
                supplier.DeliveryCostTypeId.ToString()));

            if (supplier.DeliveryCostTypeNoPaczkomatId.HasValue)
            {
                ddlAllegroAlternativeDeliveryType.SelectedIndex = ddlAllegroAlternativeDeliveryType.Items.IndexOf(ddlAllegroAlternativeDeliveryType.Items.FindByValue(
                    supplier.DeliveryCostTypeNoPaczkomatId.ToString()));

                BindShops();
            }
        }
        public void BindShops(int supplierId)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();


            gvShop.DataSource = sh.GetShopProducersBySupplierId(supplierId);
            gvShop.DataBind();
        }
        protected void gvShop_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Dal.SupplierShopFnResult ssv = e.Row.DataItem as Dal.SupplierShopFnResult;
            

                //if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                //{
                 
                //    TextBox txbShopProducerId = e.Row.FindControl("txbShopProducerId") as TextBox;

                //    txbShopProducerId.Text = ssv.ProducerId.ToString();
                //}
                //else
                //{ 
                //    Label lblShopProducerId = e.Row.FindControl("lblShopProducerId") as Label;
                //    lblShopProducerId.Text = ssv.ProducerId.ToString();

                //}

            }
        }

        protected void gvShop_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvShop.Rows[e.RowIndex];

            int id = Convert.ToInt32(gvShop.DataKeys[e.RowIndex][0]);

            TextBox txbShopProducerId = row.FindControl("txbtxbShopProducerIdDate") as TextBox;

            Dal.SupplierShop sp = new Dal.SupplierShop()
            {
                Id = id,
                ProducerId = Int32.Parse(txbShopProducerId.Text)
            };
 
            Dal.ShopHelper sh = new Dal.ShopHelper();
            sh.SetShopProducerUpdate(sp);

            gvShop.EditIndex = -1;
            if (Reloaded != null)
                Reloaded();
        }

        protected void gvShop_RowEditing(object sender, GridViewEditEventArgs e)
        {

            gvShop.EditIndex = e.NewEditIndex;
            if (Reloaded != null)
                Reloaded();
        }

        protected void gvShop_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            gvShop.EditIndex = -1;
            if (Reloaded != null)
                Reloaded();
        }
    }
}