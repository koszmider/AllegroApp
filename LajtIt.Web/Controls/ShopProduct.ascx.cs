using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using static LajtIt.Bll.ShopHelper;

namespace LajtIt.Web.Controls
{
    public partial class ShopProduct : LajtitControl
    {
        string productUrl;
        long externalId;
        int shopEngineTypeId;
        bool hideExtraActions = false;
        int shopId;
        List<Dal.SupplierShop> supplierShops;
        Dal.ProductCatalog pc;
        bool hasAccessToUpdate = false;
        class Product
        {
            public string ShopProductId { get; set; }
        }

        public int ProductCatalogId
        {
            get
            {

                if (ViewState["ProductCatalogId"] != null)
                    return Convert.ToInt32(ViewState["ProductCatalogId"]);
                else
                    return 0;

            }
            set { ViewState["ProductCatalogId"] = value; }
        }
        public bool? OnlyActiveShops { get
            {
                if (ViewState["OnlyActiveShops"] == null)
                    return null;
                else
                    return Boolean.Parse(ViewState["OnlyActiveShops"].ToString()) ;
;            }
            set
            {
                ViewState["OnlyActiveShops"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public void BindProducts(int productCatalogId, bool _hideExtraActions)
        {
            hasAccessToUpdate = this.HasActionAccess(Guid.Parse("dca537cb-694f-4ba3-86b6-420880a6739c"));
            List<Dal.ProductCatalogShopProduct> shops =
                Dal.DbHelper.ProductCatalog.GetProductCatalogShopProductByProductCatalogId(productCatalogId);
            // List<Dal.Shop> shops = Dal.DbHelper.Shop.GetShops().Where(x=> x.IsActive&& x.CanExportProducts).ToList();
            pc = Dal.DbHelper.ProductCatalog.GetProductCatalog(productCatalogId);
            supplierShops = Dal.DbHelper.ProductCatalog.GetSupplierShops(pc.SupplierId);
            hideExtraActions = _hideExtraActions;

            switch (OnlyActiveShops)
            {
                case true:
                    shops = shops.Where(x => x.IsPSAvailableByConditions).ToList(); break;
                case false:
                    shops = shops.Where(x => !x.IsPSAvailableByConditions).ToList(); break;
            }

                    gvShops.DataSource = shops.OrderBy(x => x.Shop.Name);
            gvShops.DataBind();



        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            btnUpdate_Click(sender, e);
            //btnUpdateImages_Click(sender, e);
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            //ErliRESTHelper.Products.GetProducts(Dal.Helper.Shop.Erli);
            //return;


            int shopId = Convert.ToInt32(Int32.Parse(((ImageButton)sender).CommandArgument));

            Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopId);




            Bll.ShopHelper sh = new Bll.ShopHelper();
            try
            {
                Bll.ShopUpdateHelper.ClickShop cs = new ShopUpdateHelper.ClickShop();
                List<Dal.ProductCatalogShopUpdateSchedule> schedules = new List<Dal.ProductCatalogShopUpdateSchedule>();
                schedules.Add(
                    new Dal.ProductCatalogShopUpdateSchedule()
                    {
                        ProductCatalogId = ProductCatalogId,
                        ShopId = shopId,
                        ShopColumnTypeId = (int)Dal.Helper.ShopColumnType.All,
                        UpdateTypeId = (int)Dal.Helper.UpdateScheduleType.OnlineShopSingle,
                        UpdateStatusId=(int)Dal.Helper.ShopUpdateStatus.New
                    });
                //List<Bll.ShopUpdateHelper.ClickShop.UpdateResult> results = cs.Process(schedules, Guid.NewGuid());

                Bll.ShopRestHelper.Bulk.BulkResult result = new ShopRestHelper.Bulk.BulkResult();
                if (shop == Dal.Helper.Shop.Lajtitpl)
                    result = Bll.ShopRestHelper.Products.SetProductsUpdateBatch(shop, schedules, new int[] { ProductCatalogId });
                if (shop == Dal.Helper.Shop.Erli)
                     result = Bll.ErliRESTHelper.Products.Process(schedules, Guid.NewGuid());

                //bool result = sh.SetProductUpdateByShopId(shopProductId);

                if (result.errors == false)
                {
                    DisplayMessage("Produkt został zaktualizowany");
                }
                else
                {
                
                    DisplayMessage(String.Format("Błąd aktualizacji produktu w sklepie. Szczegóły błędu:<br><Br><Br> {0}", Bll.ShopRestHelper.Bulk.BulkResultToString(result)));
                }
            }
            catch (Exception ex)
            {
    

                DisplayMessage(String.Format("Błąd: {0} {1}", ex.Message, ex.StackTrace));
            }
        }

        protected void btnUpdateImages_Click(object sender, EventArgs e)
        {
            int shopId = Convert.ToInt32(Int32.Parse(((ImageButton)sender).CommandArgument));

            Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopId);

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                bool result = ShopRestHelper.ProductsImages.SetImages(shop, ProductCatalogId);


                if (result)
                    DisplayMessage("Zdjęcia zostały zaktualizowane");
                else
                    DisplayMessage("Błąd aktualizacji zdjęć");

            }
            catch (Exception ex)
            {

                DisplayMessage(String.Format("Błąd: {0}", ex.Message));
            }
        } 
        protected void gvProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Product shopProduct = e.Row.DataItem as Product;

                HyperLink hlShopProduct = e.Row.FindControl("hlShopProduct") as HyperLink;
               
                    if (shopProduct != null)
                {
                    hlShopProduct.Text = shopProduct.ShopProductId;
                    hlShopProduct.NavigateUrl = String.Format(productUrl, shopProduct.ShopProductId);
                }


                ImageButton btnUpdate = e.Row.FindControl("btnUpdate") as ImageButton;
                ImageButton btnUpdateImages = e.Row.FindControl("btnUpdateImages") as ImageButton;
                ImageButton imbCreate = e.Row.FindControl("imbCreate") as ImageButton;

                if (shopEngineTypeId == (int)Dal.Helper.ShopEngineType.Shoper
                    || shopEngineTypeId == (int)Dal.Helper.ShopEngineType.Erli)
                    if (shopProduct.ShopProductId != null)
                    {
                        btnUpdate.Visible = true;
                        btnUpdateImages.Visible = true;
                        imbCreate.Visible = false;

                        btnUpdate.CommandArgument = btnUpdateImages.CommandArgument = shopId.ToString();
                    }
                    else
                    {
                        imbCreate.CommandArgument = shopId.ToString();
                        imbCreate.Visible = true;
                        btnUpdate.Visible = false;
                        btnUpdateImages.Visible = false;
                    }

            }
        }

        protected void gvShops_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            Dal.ProductCatalogShopProduct shop = e.Row.DataItem as Dal.ProductCatalogShopProduct;
            Label lblPriceBruttoPromo = e.Row.FindControl("lblPriceBruttoPromo") as Label;
            Label lblPriceBrutto = e.Row.FindControl("lblPriceBrutto") as Label;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (pc.IsActivePricePromo)
                {
                    lblPriceBruttoPromo.Visible = true;
                    lblPriceBruttoPromo.Text = String.Format("{0:C}", pc.PriceBruttoPromo); 
                }
                else
                {
                    lblPriceBruttoPromo.Visible = false;
                }
                lblPriceBrutto.Text = String.Format("{0:C}", pc.PriceBruttoFixed);
            }

                if (e.Row.RowType == DataControlRowType.DataRow)
            {
                productUrl = shop.Shop.ProductUrl;
                externalId = shop.Shop.ExternalId;
                shopEngineTypeId = shop.Shop.ShopType.ShopEngineTypeId;
                shopId = shop.ShopId;
                Label lblPriceBruttoDiff = e.Row.FindControl("lblPriceBruttoDiff") as Label;
                HyperLink hlUrl = e.Row.FindControl("hlUrl") as HyperLink;
                CheckBox chbSupplierActive = e.Row.FindControl("chbSupplierActive") as CheckBox;
                CheckBox chbIsPSAvailable = e.Row.FindControl("chbIsPSAvailable") as CheckBox;
                ImageButton imbAvailable = e.Row.FindControl("imbAvailable") as ImageButton;

                imbAvailable.CommandArgument = shop.Id.ToString();
                imbAvailable.Visible = hasAccessToUpdate;
                chbIsPSAvailable.Checked = shop.IsPSAvailable;
                chbSupplierActive.Checked = supplierShops.Where(x => x.ShopId == shop.ShopId).Select(x => x.IsActive).FirstOrDefault();
                hlUrl.NavigateUrl = shop.Shop.Url;
                hlUrl.Text = shop.Shop.Name;

                decimal r = 0;

                if (shop.IsPSActive)
                {
                    if (shop.ProductCatalog.IsActivePricePromo)
                    {
                        lblPriceBruttoPromo.Visible = true;
                        lblPriceBruttoPromo.Text = String.Format("{0:C}", shop.PriceBruttoMinimum);
                        if (pc.PriceBruttoPromo.HasValue && pc.PriceBruttoPromo.Value != 0)
                        {
                            r = 100 - (shop.PriceBruttoPromo.Value * 100 / pc.PriceBruttoPromo.Value);
                            lblPriceBruttoDiff.Text = String.Format("{0:0.00}%", -r);
                        }
                    }
                    else
                    {
                        lblPriceBruttoPromo.Visible = false;
                        if (pc.PriceBruttoFixed != 0 && shop.PriceBrutto.HasValue)
                        {
                            r = 100 - (shop.PriceBrutto.Value * 100 / pc.PriceBruttoFixed);
                            lblPriceBruttoDiff.Text = String.Format("{0:0.00}%", -r);
                        }
                    }
                    lblPriceBrutto.Text = String.Format("{0:C}", shop.PriceBrutto);

                    if (r < 0)
                        lblPriceBruttoDiff.ForeColor = lblPriceBrutto.ForeColor = lblPriceBruttoPromo.ForeColor = System.Drawing.Color.Green;
                    if (r > 0)
                        lblPriceBruttoDiff.ForeColor = lblPriceBrutto.ForeColor = lblPriceBruttoPromo.ForeColor = System.Drawing.Color.Red;
                }

                GridView gvProducts = e.Row.FindControl("gvProducts") as GridView;

                if (shop.Shop.ShopTypeId != (int)Dal.Helper.ShopType.Allegro)
                {
                    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                    List<Dal.ProductCatalogShopProduct> products = pch.GetProductCatalogShopProducts(ProductCatalogId, shop.ShopId);
                    gvProducts.DataSource = products.Select(x => new Product() { ShopProductId = x.ShopProductId }).ToList();
                }

                if (shop.Shop.ShopTypeId==(int)Dal.Helper.ShopType.Allegro)
                {
                    Dal.OrderHelper oh = new Dal.OrderHelper();
                    List<Dal.ProductCatalogAllegroHistoryView> allegro = oh.GetProductCatalogAllegroItemHistory(ProductCatalogId, externalId).Where(x => x.ItemStatus == "ACTIVE").ToList() ;
                    gvProducts.DataSource = allegro.Select(x => new Product() { ShopProductId = x.ItemId.ToString() }).ToList();
                }

                if (hideExtraActions)
                    gvProducts.Columns[1].Visible = false;

                gvProducts.DataBind();
            }

        }

        protected void imbAvailable_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            int id = Int32.Parse(((ImageButton)sender).CommandArgument);

            Dal.ProductCatalogShopProduct psp =  Dal.DbHelper.ProductCatalog.SetProductCatalogShopProductAvailable(id);

            BindProducts(psp.ProductCatalogId, false);
            DisplayMessage(String.Format("Zmieniono widoczność produktu w sklepie {0}", supplierShops.Where(x => x.ShopId == psp.ShopId).FirstOrDefault().Shop.Name));
        }
    }
}