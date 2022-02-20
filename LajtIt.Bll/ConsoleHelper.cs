using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Bll
{
    public class ConsoleHelper
    {
        public static void CheckImagesSize()
        {
            string path = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];
            Bll.Helper.UpdateProductCatalogImages(path);
        }

        public static void ImageThumbsCreate()
        {
            string path = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];
            Bll.Helper.ProductCatalogThumbsImage(path);

        }

        public static void LajtitplSetImages(int id)
        {
            Bll.ShopRestHelper.ProductsImages.SetImages(Dal.Helper.Shop.Lajtitpl, id);
        }

        public static void ShopSync()
        {
            //Bll.ShopHelper sh = new ShopHelper();
            //sh.GetShopProductsNotInSystem();

            Bll.ShopRestHelper.Products.GetAllProducts(Dal.Helper.Shop.Lajtitpl);
        }
        public static void ShopTest()
        {
            // Bll.ShopRestHelper.Categories.GetCategories(Dal.Helper.Shop.OswietlenieTechniczne);
            Bll.ShopRestHelper.AttributeGroups.UpdateAttributeGroupDeleteCategories(Dal.Helper.Shop.Lajtitpl, 100);
        }
    }
}