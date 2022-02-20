using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("35c33c57-4a86-4b07-84f1-b3243e3120b1")]
    public partial class SupplierNew : LajtitPage
    { 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindForm();
        }
         

        private void BindForm()
        { 
  

            List<Dal.SupplierOwner> so = LajtIt.Dal.DbHelper.ProductCatalog.GetSupplierOwners().OrderBy(x=>x.Name).ToList();
            ddlSupplierOwner.DataSource = so;
            ddlSupplierOwner.DataBind();
             

            GetShopDeliveries();

            
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dal.Supplier supplier = new Dal.Supplier()
            {
                IsActive = true,
                Margin = 0,
                Name = txbName.Text.Trim(),
                Rebate = 0,
                //MaxProductsForAllegro = 50,
                //ShopCategoryId = null,
                DeliveryId = Convert.ToInt32(ddlShopDelivery.SelectedValue),
                DeliveryFixedId = Convert.ToInt32(ddlShopDelivery.SelectedValue),
                //AllegroPriceTypeId = 1,
                //AutoCreateUpdateShop = true,
                //ProductNameTemplate = "[SUPPLIER] [LAMP_TYPE] [LINE] [COLOR] [CODE]",
                //AllegroNameTemplate = "[SUPPLIER] [LAMP_TYPE] [LINE] [COLOR] [CODE]",
                DeliveryCostTypeId = Convert.ToInt32(ddlAllegroDeliveryType.SelectedValue),
                //OnlineShopLockRebates = false,
                //AllegroSellDiscount = 0,
                //ShopSellDiscount = 0,
                ImportUrl = null,
                ImportComment = null,
                ImportTypeId = 0,
                SupplierOwnerId = Convert.ToInt32(ddlSupplierOwner.SelectedValue),
                UpdateReason = "Nowy dostawca",
                UpdateUser = UserName,
                //AllegroCommision = 0.11M,
                IsQuantityTrackingAvailable = false,
                RoundPriceTypeId = 0,
                //AllegroDiscountQty = null,
                //AllegroUserIdAccount = (int)Dal.Helper.MyUsers.Oswietlenie_Lodz,
                DeliveryCostTypeNoPaczkomatId = null,
                DisplayName = txbName.Text.Trim(),
                InsertDate = DateTime.Now,
                
                IsDropShippingAvailable = false,
                ProductCatalogGroupId = 1,
                ShowSupplierInAllegro = true,
                OrderingTypeId=1,
                CountryCode="PL"
                 
                
            };
            
            Dal.OrderHelper oh = new Dal.OrderHelper();
            int ids = oh.SetSupplier(supplier);

            DisplayMessage(String.Format("Nowy dostawca został dodany. Kliknij <a href='/supplier.aspx?id={0}'>tutaj</a> aby dokończyć edycję",ids));
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


            var d = pch.GetAllegroDeliveryCostTypes().Where(x => x.DeliveryCostTypeId != 0).ToList();
            ddlAllegroDeliveryType.DataSource = d;
            ddlAllegroDeliveryType.DataBind();
        }

        protected void lbtnSupplierOwnerAdd_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper pch = new Dal.OrderHelper();

            int supplierOwner = pch.SetSupplierOwner(txbSupplierOwner.Text.Trim());

            BindForm();

            ddlSupplierOwner.SelectedValue = supplierOwner.ToString();
        }
    }
}