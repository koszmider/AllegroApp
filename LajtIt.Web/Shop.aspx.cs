using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("411c18b3-f20a-42c8-9c88-4dbaca9a4fc7")]
    public partial class Shop : LajtitPage
    {
        private int ShopId { get { return Int32.Parse(Request.QueryString["id"].ToString()); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindShop();
        }

        private void BindShop()
        {
            Dal.Shop shop = Dal.DbHelper.Shop.GetShop(ShopId);
            
            txbClientSecret.Text = shop.ClientSecret;
            txbShop.Text = shop.Name;
            txbTemplate.Text = shop.Template;

            txbMaxPrice.Text = String.Format("{0:0.00}", shop.MaxPrice);
            txbMinPrice.Text = String.Format("{0:0.00}", shop.MinPrice);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dal.Shop shop = new Dal.Shop()
            {
                ShopId = ShopId,
                Name = txbShop.Text,
                ClientSecret = GetString(txbClientSecret.Text),
                Template = txbTemplate.Text.Trim()
            };
            if (txbMinPrice.Text != "")
                shop.MinPrice = decimal.Parse(txbMinPrice.Text);
            if (txbMaxPrice.Text != "")
                shop.MaxPrice = decimal.Parse(txbMaxPrice.Text);
            Dal.DbHelper.Shop.SetShop(shop);

            DisplayMessage("Zapisano zmiany");
        }

        private string GetString(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;
            else
                return text;
        }

        protected void btnName_Click(object sender, EventArgs e)
        {


            // Start the long running task on one thread
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(BindNames);

            Thread thread = new Thread(parameterizedThreadStart);

            thread.Start(ShopId);
 
            DisplayMessage("Zmiana nazw została zainicjowana. Wykonuje się w tle i może potrwać kilka minut.");



        }
        private void BindNames(object data)
        {
            int productIds = (int)data;
            Bll.ProductCatalogHelper pch = new Bll.ProductCatalogHelper();

            pch.UpdateProductNamesForShop(ShopId, chbCreateNew.Checked);

        }
    }
}