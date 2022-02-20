using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace LajtIt.Web
{
    [Developer("d841d554-8cae-4327-8b1b-781d735daec5")]
    public partial class Product : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        //private List<Dal.ProductCatalogImage> images;
        bool fullAccess = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            ucProductCatalogGroup.ProductCatalogId = ProductCatalogId;

            fullAccess = HasActionAccess(Guid.Parse("89e5ac3e-3b88-432c-a955-7a0a9aabe3f2"));

            if (!Page.IsPostBack)
            {
                BindProductTypes();
                BindProductCatalog();
                GetShopDeliveries();
                //calDate.SelectedDate = DateTime.Now;
                //txbPricePromoDate.Text = calDate.SelectedDate.Value.ToString("yyyy/MM/dd");
            }
            else
            {
                if (txbPricePromoDate.Text != "")
                    calDate.SelectedDate = DateTime.Parse(txbPricePromoDate.Text);

            }

        }
        private void GetShopDeliveries()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            ddlShopDelivery.DataSource = pch.GetDeliveries().Select(x => new
            {
                DeliveryId = x.DeliveryId,
                Name = String.Format("{0} godz. ({1} dni)", x.DeliveryHours, x.DeliveryHours / 24)
            }).ToList();
            ddlShopDelivery.DataBind();
        }
        protected void lbtnCreateNames_Click(object sender, EventArgs e)
        {
            Bll.ProductCatalogHelper pch = new Bll.ProductCatalogHelper(); 
            Dal.ProductCatalog pc = Dal.DbHelper.ProductCatalog.GetProductCatalog(ProductCatalogId);
            //Bll.ProductFileImportHelper pci = new Bll.ProductFileImportHelper();

            //Dal.OrderHelper oh = new Dal.OrderHelper();
            //Dal.Supplier supplier = oh.GetSupplier(pc.SupplierId);
            //if(chbAllegroName.Checked)
            //pc.AllegroName =  pci.GetProductName(pc, supplier,  Dal.Helper.Source.Allegro);

            //if (chbShopName.Checked)
            //    pc.Name = pci.GetProductName(pc, supplier,  Dal.Helper.Source.OnlineShop);

            pch.UpdateProductNames(new List<Dal.ProductCatalog>() { pc });

            DisplayMessage("Zaktualizowano nazwy produktów");

            BindProductCatalog();

        }
        protected void txbExternalId_OnTextChanged(object sender, EventArgs e)
        {

            string code = txbExternalId.Text.Trim();
            int supplierId = Convert.ToInt32(ddlSuppliers.SelectedValue);


            Dal.OrderHelper oh = new Dal.OrderHelper();
            bool isUnique = oh.GetProductCatalogExternalId(ProductCatalogId, code, supplierId);

            if (!isUnique)
            {
                Dal.ProductCatalogView product = oh.GetProductCatalog(ProductCatalogId);
                txbExternalId.Text = product.ExternalId;
                DisplayMessage(String.Format("Id zewnętrzne: {0} jest już w użyciu", code));
            }

        }
        protected void txbAllegroCode_OnTextChanged(object sender, EventArgs e)
        {

            string code = txbAllegroCode.Text.Trim();
            int supplierId = Convert.ToInt32(ddlSuppliers.SelectedValue);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            bool isUnique = oh.GetProductCatalogCode(ProductCatalogId, code, supplierId);

            if (!isUnique)
            {
                Dal.ProductCatalogView product = oh.GetProductCatalog(ProductCatalogId);
                txbAllegroCode.Text = product.Code;
                DisplayMessage(String.Format("Kod: {0} jest już w użyciu", code));
            }

        }
        protected void txbEan_OnTextChanged(object sender, EventArgs e)
        {

            string ean = txbEan.Text.Trim();


            Dal.OrderHelper oh = new Dal.OrderHelper();
            bool isUnique = oh.GetProductCatalogEan(ProductCatalogId, ean);
            int supplierId = Convert.ToInt32(ddlSuppliers.SelectedValue);

            if (!isUnique)
            {
                Dal.ProductCatalogView product = oh.GetProductCatalog(ProductCatalogId);
                txbEan.Text = product.Ean;
                DisplayMessage(String.Format("Kod EAN: {0} jest już w użyciu", ean));
            }

        }
        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            int productCatalogId = 0;
            productCatalogId=oh.SetProductCatalogDuplicate(ProductCatalogId);
            if (productCatalogId == 0)
                DisplayMessage("Nie można zduplikować produktu.");
            else
                DisplayMessage(String.Format("Produkt został zduplikowany. Kliknij <a href='Product.aspx?id={0}'>tutaj</a> aby wyświetlić", productCatalogId));
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            if (!oh.SetProductCatalogDelete(ProductCatalogId))
                DisplayMessage("Nie można usunąć produktu. Możliwe przyczyny to: przypisanie do istniejących zamówień, dostaw, synonimów");
            else
                DisplayMessage("Produkt został usunięty. Kliknij <a href='ProductCatalog.aspx'>tutaj</a> aby odświeżyć");
        }

      
        protected void ddlSuppliers_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            
            ucProductCatalogGroup.BindProductCatalogGroups(ProductCatalogId, Convert.ToInt32(ddlSuppliers.SelectedValue));
        } 
        private void BindProductCatalog()
        {
            pAdmin.Visible = fullAccess;
            BindSuppliers();
            int productCatalogId = ProductCatalogId;
            Dal.OrderHelper oh = new Dal.OrderHelper();
           // Dal.ProductCatalogItemStatsResult stat = oh.GetProductCatalogItemStats(productCatalogId);
            Dal.ProductCatalogView pc = oh.GetProductCatalog(productCatalogId);
           

            txbName.Text = pc.Name;
            txbPrice.Text = pc.PriceBruttoFixed.ToString();
            txbAllegroCode.Text = pc.Code;
            txbCode2.Text = pc.Code2;

            if (pc.PurchasePrice.HasValue)
                txbPurchasePrice.Text = pc.PurchasePrice.Value.ToString();

            if (pc.IsPaczkomatAvailable.HasValue)
            {
                if (pc.IsPaczkomatAvailable.Value)
                    rbPaczkomat1.Checked = true;
                else
                    rbPaczkomat0.Checked = true;
            }
            else
                rbPaczkomat.Checked = true;
            if (pc.IsOutlet.HasValue && pc.IsOutlet.Value)
                chbIsOutlet.Checked = true;
            txbEan.Text = pc.Ean;
            txbExternalId.Text = pc.ExternalId;
            ddlSuppliers.SelectedValue = pc.SupplierId.ToString();
            ddlProductType.SelectedValue = pc.ProductTypeId.ToString();
            ucProductCatalogGroup. BindProductCatalogGroups(pc.ProductCatalogId, pc.SupplierId);
            if (pc.PriceBruttoPromo.HasValue && pc.PriceBruttoPromoDate.HasValue)
            {
                txbPricePromo.Text = pc.PriceBruttoPromo.Value.ToString();
                calDate.SelectedDate = pc.PriceBruttoPromoDate.Value;
            }
            else
            {
                calDate.SelectedDate = null;
                txbPricePromoDate.Text = "";
            }

            //if (pc.AllegroPriceBruttoPromo.HasValue)
            //{
            //    lblAllegroPricePromoBrutto.Text = pc.AllegroPriceBruttoPromo.Value.ToString(); 
            //}
            //else
            //{
            //    lblAllegroPricePromoBrutto.Text = "";
            //}
  
            chbIsAvailable.Checked = pc.IsAvailable; 
            chbIsOnStock.Checked = pc.IsOnStock;
            chbIsHidden.Checked = pc.IsHidden   ;
            chbIsDiscontinued.Checked = pc.IsDiscontinued;
            chbImage.Checked = pc.ImageFullName != null;
            chbPrice.Checked = pc.PriceBruttoFixed > 0;
            chbCode.Checked = pc.Code != null;
            chbIsReady.Checked = pc.IsReady;
            chbHasProductType.Checked = pc.HasProductType;
            chbIsFollowed.Checked = pc.IsFollowed ?? false;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            int deliveryHours = pch.GetDeliveries().Where(x => x.DeliveryId == pc.SupplierDeliveryId).FirstOrDefault().DeliveryHours;
            lbSupplierDeliveryTime.Text = String.Format("{0} godz. ({1} dni)", deliveryHours, deliveryHours / 24);

            if (pc.ProductDeliveryId.HasValue)
            {
                ddlShopDelivery.SelectedValue = pc.ProductDeliveryId.ToString();
            }
            switch(pc.LockRebates)
            {
                case true: ddlLockRebates.SelectedValue = "1"; break;
                case false: ddlLockRebates.SelectedValue = "0"; break;
            }
        }
        private void BindSuppliers()
        { 
            ddlSuppliers.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderBy(x => x.Name).ToList();
            ddlSuppliers.DataBind();
        }
        private void BindProductTypes()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            ddlProductType.DataSource = pch.GetProductTypes();
            ddlProductType.DataBind();
        }




        protected void btnClientDataSave_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalog pc = new Dal.ProductCatalog()
            {
                ProductCatalogId = ProductCatalogId,
                Name = txbName.Text.Trim(),
                Ean = txbEan.Text.Trim(),
                Code = txbAllegroCode.Text.Trim(),
                Code2 = txbCode2.Text.Trim() == "" ? null : txbCode2.Text.Trim(),
                SupplierId = Convert.ToInt32(ddlSuppliers.SelectedValue),
                //ProductCatalogGroupId = Convert.ToInt32(ddlProductCatalogGroup.SelectedValue),
                ExternalId = txbExternalId.Text.Trim() == "" ? null : txbExternalId.Text.Trim(),
                UpdateUser = UserName,
                UpdateReason = "Zmiana na karcie produktu"

            };
            if (ddlShopDelivery.SelectedIndex != 0)
            {
                pc.DeliveryId = Int32.Parse(ddlShopDelivery.SelectedValue);
            }
            

            pc.IsOutlet = chbIsOutlet.Checked;
            pc.ProductTypeId = Convert.ToInt32(ddlProductType.SelectedValue);
            if (rbPaczkomat.Checked) pc.IsPaczkomatAvailable = null;
            if (rbPaczkomat1.Checked) pc.IsPaczkomatAvailable = true;
            if (rbPaczkomat0.Checked) pc.IsPaczkomatAvailable = false;
            pc.PriceBruttoFixed = Convert.ToDecimal(txbPrice.Text.Trim());
            pc.IsDiscontinued = chbIsDiscontinued.Checked;
            pc.IsAvailable = chbIsAvailable.Checked;
            pc.IsHidden = chbIsHidden.Checked;



            if (pAdmin.Visible)
            {
                switch (ddlLockRebates.SelectedValue)
                {
                    case "1":
                        pc.LockRebates = true; break;
                    case "0":
                        pc.LockRebates = false; break;
                    default:
                        pc.LockRebates = null; break;
                }
                if (!String.IsNullOrEmpty(txbPricePromo.Text) && String.IsNullOrEmpty(txbPricePromoDate.Text))
                {
                    DisplayMessage("Ustawiając cenę promocyjną należy określić datę końcową");
                    return;
                }

           
                decimal? purchasePrice = null;
                decimal pp = 0;
                if(Decimal.TryParse(txbPurchasePrice.Text.Trim(), out pp))
                {
                    purchasePrice = pp;
                }

                pc.PurchasePrice = purchasePrice;

                if (!String.IsNullOrEmpty(txbPricePromo.Text))
                {
                    pc.PriceBruttoPromo = Convert.ToDecimal(txbPricePromo.Text.Trim());
                    pc.PriceBruttoPromoDate = calDate.SelectedDate.Value.AddHours(23).AddMinutes(59);
                }
                else
                {
                    calDate.SelectedDate = null;
                    txbPricePromoDate.Text = "";
                }

            

                if (pc.PriceBruttoPromo.HasValue && pc.PriceBruttoPromo.Value >= pc.PriceBruttoFixed)
                {
                    DisplayMessage("Cena promocyjna musi być mniejsza niż cena podstawowa");
                    return;


                }

            }

            int productCatalogGroupId = ucProductCatalogGroup.ProductCatalogGroupId;

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetProductCatalogClientData(pc, productCatalogGroupId, pAdmin.Visible);
            DisplayMessage("Zapisano");
            BindProductCatalog();
        }

        protected void chbIsFollowed_CheckedChanged(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogIsFollowed(ProductCatalogId, chbIsFollowed.Checked);
            DisplayMessage("Zmiany zostały zapisane");
        }

    }
}