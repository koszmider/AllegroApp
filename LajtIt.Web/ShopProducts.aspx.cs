using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("1b5cffda-4644-45b9-95b2-fda6669bb9ef")]
    public partial class ShopProducts : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnShopProducts_Click(object sender, EventArgs e)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();

            gvProductsInShop.DataSource = sh.GetShopProductsNotInSystem(1);
            gvProductsInShop.DataBind();
        }

        protected void btnShopProductsDeleteNotExisingInSystem_Click(object sender, EventArgs e)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
      
            string[] productIdsToDelete = sh.GetShopProductsNotInSystem(1).Select(x => x.ShopProductId).Distinct().ToArray();


            Bll.ShopHelper shb = new Bll.ShopHelper();
            bool result = shb.SetProductDelete(productIdsToDelete);

            if (result)
                DisplayMessage("Produkty zostały usunięte. Znikną z listy po ponownym pobraniu z serwera.");
            else
                DisplayMessage("Problem z usunięciem produktów");
        }
    }
}