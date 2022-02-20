using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class ShopCategoryControl : System.Web.UI.UserControl
    {

        public int? SelectedCategoryId
        {
            get
            {
                if (lsbCategories.SelectedIndex == -1)
                    return null;
                else
                    return Convert.ToInt32(lsbCategories.SelectedValue);

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //public void BindCategories(int? categoryId, int? rootCategoryId, int shopTypeId)
        //{
        //    BindCategories(false, categoryId, rootCategoryId, shopTypeId);
        //}
        public void BindCategories(bool mainOnly, int? categoryId, Dal.Helper.ShopType shopType)
        {
            BindCategories(mainOnly, categoryId, null, shopType);
        }
        private void BindCategories(bool mainOnly, int? categoryId, int? rootCategoryId, Dal.Helper.ShopType shopType)
        {

            lsbCategories.Items.Clear();

            Dal.ShopHelper sh = new Dal.ShopHelper();

            List<Dal.ShopCategoryFnResult> categories = sh.GetShopCategories(shopType);

            if (mainOnly)
                categories = categories.Where(x => x.CategoryParentId == null).ToList();

            if (categoryId.HasValue)
            { 
                Dal.ShopCategory sc= sh.GetCategory(shopType, categoryId.Value);

                if (rootCategoryId.HasValue)
                    categories = categories.Where(x => x.RootCategoryId == sc.ShopCategoryId).ToList();
            }


            string parentId = null;

            BindSubCategories(parentId, categories, 0);

            if (categoryId.HasValue == true)
            {
                lsbCategories.SelectedValue = categoryId.ToString();
                hidSelected.Value = categoryId.ToString();
            }
        }

        private void BindSubCategories(string parentId, List<Dal.ShopCategoryFnResult> categories, int level)
        {
            List<Dal.ShopCategoryFnResult> subCategories = categories.Where(x => x.CategoryParentId == parentId).
                OrderBy(x => x.CategoryOrder).ToList();

            int nextLevel = level + 1;
            foreach (Dal.ShopCategoryFnResult c in subCategories)
            {
                StateBag b = new StateBag();
                string name = c.Name;

                if (!c.IsActive)
                    name = String.Format("[{0}]", name);
                ListItem l = new ListItem()
                {
                    Text = String.Format("{0} {1}", String.Join("", Enumerable.Repeat("-", level)), name),
                    Value = c.CategoryId.ToString()
                };
                lsbCategories.Items.Add(l);

                BindSubCategories(c.ShopCategoryId, categories, nextLevel);
            }
        }
    }
}