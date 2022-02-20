using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("9232ece3-47d0-4f8c-90bc-670aaf1f26f5")]
    public partial class ShopCategoryManager : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShops();                
            }          
        }

        private void BindShops()
        { 

            ddlShop.DataSource = Dal.DbHelper.Shop.GetShops().Where(x =>
            x.ShopType.ShopEngineTypeId == (int)Dal.Helper.ShopEngineType.Shoper
            || x.ShopType.ShopEngineTypeId == (int)Dal.Helper.ShopEngineType.Allegro).ToList();
            ddlShop.DataBind();

        }

        protected void ShopCategoryAdded(object sender, EventArgs e)
        {
            BindShopCategories();

        }

        private void BindShopCategories()
        {
            Dal.ShopHelper ph = new Dal.ShopHelper();

            gvShopCategorys.DataSource = ph.GetShopCategoryManagers(Int32.Parse(ddlShop.SelectedValue));
            gvShopCategorys.DataBind();
        }

        protected void lbtnShopCategoryAdd_Click(object sender, EventArgs e)
        {
            if (ddlShop.SelectedValue == "0")
            {
                DisplayMessage("Wybierz sklep");
                return;
            }


            Dal.ShopCategoryManager scm = new Dal.ShopCategoryManager()
            { 
                InsertDate = DateTime.Now,
                InsertUser = UserName,
                IsActive = false,
                Name = String.Format("Nowa konfiguracja {0:yyyyMMdd HH:mm}", DateTime.Now) ,
                ShopId= Int32.Parse(ddlShop.SelectedValue)
            };

            Dal.ShopHelper ph = new Dal.ShopHelper();
            Response.Redirect(String.Format("ShopCategoryManagerPage.aspx?id={0}", ph.SetShopCategoryManager(scm)));


        }

        protected void lbtnRefreshCategory_Click(object sender, EventArgs e)
        {
            if (ddlShop.SelectedValue == "0")
            {
                DisplayMessage("Wybierz sklep");
                return;
            }

            Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), Int32.Parse(ddlShop.SelectedValue));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Bll.ShopRestHelper.Categories.GetCategories(shop);

            DisplayMessage("Odświeżono drzewo kategorii");
        }

        protected void ddlShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindShopCategories();
            if (ddlShop.Items[0].Value == "0")
                ddlShop.Items.RemoveAt(0);
        }
    }
}