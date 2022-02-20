using LajtIt.Dal;
using LinqToExcel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using static LajtIt.Bll.ShopUpdateHelper.ClickShop;

namespace LajtIt.Bll
{
    public partial class ShopRestHelper
    {
        public class Categories
        {

            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
            public class PlPL
            {
                public string trans_id { get; set; }
                public string category_id { get; set; }
                public string name { get; set; }
                public string description { get; set; }
                public string description_bottom { get; set; }
                public int active { get; set; }
                public object pres_id { get; set; }
                public string isdefault { get; set; }
                public string lang_id { get; set; }
                public string seo_title { get; set; }
                public string seo_description { get; set; }
                public string seo_keywords { get; set; }
                public string seo_url { get; set; }
                public string permalink { get; set; }
                public int items { get; set; }
                public List<int> attribute_groups { get; set; }
            }

            public class Translations
            {
                public PlPL pl_PL { get; set; }
            }

            public class Category
            {
                public string category_id { get; set; }
                public int order { get; set; }
                public string root { get; set; }
                public string in_loyalty { get; set; }
                public Translations translations { get; set; }
            }

            public class RootCategory
            {
                public string count { get; set; }
                public int pages { get; set; }
                public int page { get; set; }
                public List<Category> list { get; set; }
            }




            #region REST

            public static void GetCategories(Dal.Helper.Shop shop)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                Dal.Shop s = Dal.DbHelper.Shop.GetShop((int)shop);

                Dal.Helper.ShopType shopType = (Dal.Helper.ShopType)Enum.ToObject(typeof(Dal.Helper.ShopType), s.ShopTypeId);



                List<Dal.ShopCategory> categories = new List<Dal.ShopCategory>();
                GetCategories(shop, shopType, 1, categories);


                List<object> tree = GetCategoriesTree(shop);

                ProcessCategoiesTree(categories, tree, null);



                sh.SetCategories(shopType, categories);
            }

            private static void GetCategories(Dal.Helper.Shop shop, Dal.Helper.ShopType shopType, int page, List<Dal.ShopCategory> categories)
            {
                //Product productFromDb = GetProductFromDb(shop, productCatalogId);

                //if (productFromDb != null)
                //    return productFromDb;

                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/categories?limit=50&page={0}", page), "GET");

                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    RootCategory shopCategories = json_serializer.Deserialize<RootCategory>(text);


                    foreach (Category c in shopCategories.list)
                    {
                        Dal.ShopCategory category = new Dal.ShopCategory()
                        {
                            ShopCategoryId = c.category_id.ToString(),
                            CategoryOrder = c.order,
                            CategoryParentId = null,
                            Name = c.translations.pl_PL.name,
                            Url = c.translations.pl_PL.permalink,
                            IsActive = c.translations.pl_PL.active == 1,
                            ShopTypeId = (int)shopType,
                            IsAllowed = true
                        };

                        if (String.IsNullOrEmpty(category.Permalink))
                            category.Permalink = c.translations.pl_PL.permalink;
                        if (String.IsNullOrEmpty(category.SeoDescription))
                            category.SeoDescription = c.translations.pl_PL.seo_description;
                        if (String.IsNullOrEmpty(category.SeoKeywords))
                            category.SeoKeywords = c.translations.pl_PL.seo_keywords;
                        if (String.IsNullOrEmpty(category.SeoTitle))
                            category.SeoTitle = c.translations.pl_PL.seo_title;
                        if (String.IsNullOrEmpty(category.Description))
                            category.Description = c.translations.pl_PL.description;

                        categories.Add(category);

                    }

                    if(shopCategories.page<shopCategories.pages)
                        Bll.ShopRestHelper.Categories.GetCategories(shop, shopType, shopCategories.page+1, categories);
                    //Dal.ShopHelper sh = new Dal.ShopHelper();

                    //sh.SetProductCatalogShopProductJson(shop, productCatalogId, Bll.RESTHelper.ToJson(product));



                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop, String.Format("Błąd pobierania drzewa kategorii {0}", shop.ToString()));

                }
            }
            private static List<object> GetCategoriesTree(Dal.Helper.Shop shop)
            {

                //Product productFromDb = GetProductFromDb(shop, productCatalogId);

                //if (productFromDb != null)
                //    return productFromDb;

                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/categories-tree{0}", ""), "GET");
                     
                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();
                    List<object> tree = json_serializer.Deserialize<List<object>>(text);
                    return tree;

                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop, String.Format("Błąd pobierania drzewa kategorii {0}",shop.ToString()));
                    return null;
                }
            }

            private static void ProcessCategoiesTree(List<Dal.ShopCategory> categories, List<object> tree, string parentId)
            {
                //var ll = tree as Dictionary<string, object>;



                List<string> categoryIds = new List<string>();

                foreach (object leaf in tree)
                {
                    Dictionary<string, object> l = leaf as Dictionary<string, object>;
                    string category_id = l["id"].ToString();

                    categoryIds.Add(category_id);


                    ProcessCategoiesTree(categories, (l["children"] as object[]).ToList(), category_id);

                }


                //string[] children = (ll["children"] as object[]).ToList().Select(x => (x as Dictionary<string, object>)["id"].ToString()).ToArray();
                List<Dal.ShopCategory> categoryList = categories.Where(x => categoryIds.Contains(x.ShopCategoryId)).ToList();


                foreach (Dal.ShopCategory c in categoryList)
                    c.CategoryParentId = parentId;
            }

            private static void ProcessCategoiesTree(Dictionary<string, object> tree, int? parent_category_id)
            {
                foreach (var leaf in tree)
                {
                    //Dictionary<string, object> l = leaf;
                    int? category_id = Int32.Parse(leaf.Value.ToString());

                    //ProcessCategoiesTree(leaf.k as Dictionary<string, object>, category_id);

                }
            }
            #endregion



        }
    }
}