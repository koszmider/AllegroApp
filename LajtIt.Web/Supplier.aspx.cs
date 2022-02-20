using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("45d33ef6-6fc7-48a8-a1bd-c07e7202d786")]
    public partial class Supplier : LajtitPage
    {
        private int SupplierId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {

             

            if (!Page.IsPostBack)
                BindForm();
        }

 

        protected void lbtnCreateNames_Click(object sender, EventArgs e)
        {
            Bll.ProductCatalogHelper pch = new Bll.ProductCatalogHelper();
            pch.UpdateProductNames(new int[] { SupplierId });

            DisplayMessage("Zaktualizowano nazwy produktów");

        }

        private void BindForm()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier supplier = oh.GetSupplier(SupplierId);

            ddlCountry.DataSource = Dal.DbHelper.ProductCatalog.GetCountries();
            ddlCountry.DataBind();

            ddlOrderingType.DataSource = oh.GetSupplierOrderingTypes();
            ddlOrderingType.DataBind();



            List<Dal.SupplierOwner> so = LajtIt.Dal.DbHelper.ProductCatalog.GetSupplierOwners().OrderBy(x => x.Name).ToList();
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
            chbIsQuantityTrackingAvailable.Checked = supplier.IsQuantityTrackingAvailable;
           
            if (supplier.QuantityMinLevel.HasValue)
            {
                txbQuantityMin.Text = supplier.QuantityMinLevel.ToString();
            }
            if (supplier.CountryCode !=null)
            {
                ddlCountry.SelectedValue = supplier.CountryCode;
            }
            if (supplier.OrderWeekDays != null)
            {
                string[] days = supplier.OrderWeekDays.Split(new char[] { ',' });

                foreach (ListItem item in chblOrderWeekDays.Items)
                    if (days.Contains(item.Value))
                        item.Selected = true;
            }

            switch (supplier.RoundPriceTypeId)
            {
                case 0: ddlRoundPriceType.SelectedIndex = 0; break;
                case 1: ddlRoundPriceType.SelectedIndex = 1; break;

            }



            GetShopDeliveries();

            ddlShopDelivery.SelectedValue = supplier.DeliveryFixedId.ToString();
            BindAllegroDeliveryTypes();

            ddlAllegroDeliveryType.SelectedIndex = ddlAllegroDeliveryType.Items.IndexOf(ddlAllegroDeliveryType.Items.FindByValue(
                supplier.DeliveryCostTypeId.ToString()));

            if (supplier.DeliveryCostTypeNoPaczkomatId.HasValue)
            {
                ddlAllegroAlternativeDeliveryType.SelectedIndex = ddlAllegroAlternativeDeliveryType.Items.IndexOf(ddlAllegroAlternativeDeliveryType.Items.FindByValue(
                    supplier.DeliveryCostTypeNoPaczkomatId.ToString()));


            }
        }
       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dal.Supplier supplier = new Dal.Supplier()
            {
                SupplierId = SupplierId,
                IsActive = chbIsActive.Checked,
                Margin = Convert.ToDecimal(txbMargin.Text.Trim()) / 100M,
                Name = txbName.Text.Trim(),
                Rebate = Convert.ToDecimal(txbRebate.Text.Trim()) / 100M,
               // ShowSupplierInAllegro = chbShowSupplierInAllegro.Checked,
                //MaxProductsForAllegro = Convert.ToInt32(txbMax.Text.Trim()),
                //ShopCategoryId = ucShopCategoryControl.GetCategoryId(), 
                DeliveryFixedId = Convert.ToInt32(ddlShopDelivery.SelectedValue),
                //AllegroPriceTypeId = Convert.ToInt32(ddlAllegroPriceTypeId.SelectedValue),
                //AutoCreateUpdateShop = chbAutoCreateUpdateShop.Checked,
                //ProductNameTemplate = txbProductNameTemplate.Text.Trim(), 
                //AllegroNameTemplate = txbAllegroNameTemplate.Text.Trim(),
                DeliveryCostTypeId = Convert.ToInt32(ddlAllegroDeliveryType.SelectedValue),
                //OnlineShopLockRebates = chbOnlineShopLockRebates.Checked,
                //AllegroSellDiscount = Convert.ToDecimal(txbAllegroSellDiscount.Text.Trim()) / 100M,
                //ShopSellDiscount = Convert.ToDecimal(txbShopSellDiscount.Text.Trim()) / 100M,
                ImportUrl = txbImportUrl.Text.Trim(),
                ImportComment = txbImportComment.Text.Trim(),
                ImportTypeId = Convert.ToInt32(ddlImportType.SelectedValue),
                SupplierOwnerId = Convert.ToInt32(ddlSupplierOwner.SelectedValue),
                UpdateReason = "Zmiana parametrów dostawcy",
                UpdateUser = UserName, 
              //  AllegroCommision = Convert.ToDecimal(txbAllegroCommision.Text.Trim()) / 100M,
                IsQuantityTrackingAvailable = chbIsQuantityTrackingAvailable.Checked,
                RoundPriceTypeId = Convert.ToInt32(ddlRoundPriceType.SelectedValue),
                OrderingTypeId = Convert.ToInt32(ddlOrderingType.SelectedValue),
                IsDropShippingAvailable = chbIsDropShippingAvailable.Checked,
                DeliveryCostTypeNoPaczkomatId = Int32.Parse(ddlAllegroAlternativeDeliveryType.SelectedValue),
                OrderWeekDays = GetOrderWeekDays()
            };
 

             if(txbQuantityMin.Text!="")
                supplier.QuantityMinLevel = Int32.Parse(txbQuantityMin.Text);
            if (txbB2bUrl.Text.Trim() != "")
                supplier.B2bUrl= txbB2bUrl.Text.Trim();
            if (txbB2bEmail.Text.Trim() != "")
                supplier.B2bEmail = txbB2bEmail.Text.Trim();
            if (ddlCountry.SelectedIndex != 0)
                supplier.CountryCode = ddlCountry.SelectedValue;
            else
                supplier.CountryCode = null;
      
            Dal.DbHelper.ProductCatalog.SetSupplierUpdate(supplier);

            DisplayMessage("Zaktualizowano");
        }

        private string GetOrderWeekDays()
        {
            string days = String.Join(",", chblOrderWeekDays.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray());

            if (days == "")
                return null;

            return days;
        }

        private void GetShopDeliveries()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            ddlShopDelivery.DataSource = pch.GetDeliveries().Select(x=>new
            {
                DeliveryId = x.DeliveryId,
                Name = String.Format("{0} godz. ({1} dni)", x.DeliveryHours, x.DeliveryHours / 24)
            }).ToList();
            ddlShopDelivery.DataBind();
        }
        private void BindAllegroDeliveryTypes()
        {
            Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
            var d = pc.GetAllegroDeliveryCostTypes().Where(x => x.DeliveryCostTypeId != 0).ToList();
            ddlAllegroDeliveryType.DataSource = d;
            ddlAllegroDeliveryType.DataBind();
            ddlAllegroAlternativeDeliveryType.DataSource = d;
            ddlAllegroAlternativeDeliveryType.DataBind();
        }

        //protected void lbtnAllegroDiscountDelete_Click(object sender, EventArgs e)
        //{
        //    Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
        //    pc.SetProductCatalogAllegroItemPromotionDelete(SupplierId);

        //    DisplayMessage("Promocje zostaną usunięte");
        //    BindForm();

        //}


    }
}