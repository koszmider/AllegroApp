using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("0fa3f84b-8154-4fa1-b349-d5b1d92c5e4a")]
    public partial class CeneoManager : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if(!Page.IsPostBack)
            {
                BindShops();
            }

        }

        private void BindShops()
        { 
            ddlShop.DataSource = Dal.DbHelper.Shop.GetShops().Where(x => x.ShopTypeId == (int)Dal.Helper.ShopType.Ceneo).ToList();
            ddlShop.DataBind();

            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.Supplier> suppliers = oh.GetSuppliersForShop(Dal.Helper.Shop.Lajtitpl).OrderBy(x => x.Name).ToList() ;

            lbxSuppliers.DataSource = suppliers;
            lbxSuppliers.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<Dal.CeneoShopResult> products = GetProducts();

            lblResults.Text = String.Format("Liczba produktów {0}", products.Count());

        }

        private List<Dal.CeneoShopResult> GetProducts()
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();

            bool? isAuction = null;
            bool? IsActivePricePromo = null;
            if (ddlProductAuction.SelectedIndex > 0)
                isAuction = ddlProductAuction.SelectedIndex == 1;

            if (ddlShopPromo.SelectedIndex > 0)
                IsActivePricePromo = ddlShopPromo.SelectedIndex == 1;

            int[] supplierId = lbxSuppliers.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();

            if(supplierId.Length==0)
                supplierId= lbxSuppliers.Items.Cast<ListItem>().Select(x => Int32.Parse(x.Value)).ToArray();

            decimal? priceFrom = null, priceTo = null, bidFrom = null, bidTo = null;
            if (txbPriceFrom.Text.Trim() != "") priceFrom = Convert.ToDecimal(txbPriceFrom.Text);
            if (txbPriceTo.Text.Trim() != "") priceTo = Convert.ToDecimal(txbPriceTo.Text);
            if (txbBidFrom.Text.Trim() != "") bidFrom = Convert.ToDecimal(txbBidFrom.Text);
            if (txbBidTo.Text.Trim() != "") bidTo = Convert.ToDecimal(txbBidTo.Text);


            List<Dal.CeneoShopResult> products = sh.GetCeneoProducts(isAuction, IsActivePricePromo, supplierId, priceFrom, priceTo, bidFrom, bidTo);
            return products;
        }

        protected void btnClearPromotions_Click(object sender, EventArgs e)
        {
            List<Dal.CeneoShopResult> products = GetProducts();

            Bll.CeneoApiHelper ch = new Bll.CeneoApiHelper();

            try
            {
                ch.SetProductsBids(Dal.Helper.Shop.Ceneo, products, 0);


                Dal.ShopHelper sh = new Dal.ShopHelper();
                sh.SetProductCatalogShop(products, 0);
                DisplayMessage("Zrobione");
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
            }
        }

        protected void btnMaxBid_Click(object sender, EventArgs e)
        {

            List<Dal.CeneoShopResult> products = GetProducts();

            Bll.CeneoApiHelper ch = new Bll.CeneoApiHelper();

            try
            {
                decimal maxBid = Decimal.Parse(txbMaxBid.Text.Trim());
                ch.SetProductsBids(Dal.Helper.Shop.Ceneo, products, maxBid);
                Dal.ShopHelper sh = new Dal.ShopHelper();
                sh.SetProductCatalogShop(products, maxBid);
                DisplayMessage("Zrobione");
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
            }

        }
    }
}