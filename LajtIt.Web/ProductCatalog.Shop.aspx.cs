using System;
using System.Collections.Generic;
using System.Web.UI;

namespace LajtIt.Web
{
    [Developer("007ec6a7-139c-42e4-a289-2e204a862d22")]
    public partial class ProductShop : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ucShopProduct.ProductCatalogId = ProductCatalogId;
                ucShopProduct.BindProducts(ProductCatalogId, false); 
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            throw new NotImplementedException("TO DO");
            //int shopProductId = 0; // Convert.ToInt32(txbShopProductId.Text.Trim());

            //Bll.ShopHelper sh = new Bll.ShopHelper();
            //try
            //{
            //    bool result = sh.SetProductUpdateByShopId(shopProductId);

            //    if (result)
            //        DisplayMessage("Produkt został zaktualizowane");
            //    else
            //        DisplayMessage("Błąd aktualizacji");

            //}
            //catch (Exception ex)
            //{

            //    DisplayMessage(String.Format("Błąd: {0}", ex.Message));
            //}
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("TO DO");
            //Bll.ShopHelper sh = new Bll.ShopHelper();
            //try
            //{
            //    bool result = sh.SetProductUpdateByProductCatalogId(ProductCatalogId);

            //    if (result)
            //        DisplayMessage("Produkt został utworzony");
            //    else
            //        DisplayMessage("Błąd tworzenia");
            //}
            //catch (Exception ex)
            //{

            //    DisplayMessage(String.Format("Błąd: {0}", ex.Message));
            //} 

        }
        protected void btnUpdateImages_Click(object sender, EventArgs e)
        {
            //int shopProductId = 0;// Convert.ToInt32(txbShopProductId.Text.Trim());
             
            try
            {
                //Bll.ShopUpdateHelper.ClickShop cs = new Bll.ShopUpdateHelper.ClickShop();
                //bool result = cs.SetImagesUpdate(Dal.Helper.Shop.Lajtitpl, shopProductId);

                bool result = Bll.ShopRestHelper.ProductsImages.SetImages(Dal.Helper.Shop.Lajtitpl, ProductCatalogId);

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

        protected void chbOnlyActiveShops_CheckedChanged(object sender, EventArgs e)
        {
            bool? onlyActive = null;
            if (rblOnlyActive.SelectedIndex == 1)
                onlyActive = true;
            if (rblOnlyActive.SelectedIndex == 2)
                onlyActive = false;
            ucShopProduct.OnlyActiveShops = onlyActive;
            ucShopProduct.BindProducts(ProductCatalogId, false);
        }
    }
}