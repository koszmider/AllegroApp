

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public partial class AllegroRESTHelper
    {
        public class Categories
        {

            public class Options
            {
                public bool variantsByColorPatternAllowed { get; set; }
                public bool advertisement { get; set; }
                public bool advertisementPriceOptional { get; set; }
                public bool offersWithProductPublicationEnabled { get; set; }
                public bool productCreationEnabled { get; set; }
                public bool productEANRequired { get; set; }
            }

            public class Category
            {
                public string id { get; set; }
                public string name { get; set; }
                public Parent parent { get; set; }
                public bool leaf { get; set; }
                public Options options { get; set; }
            }
            public class Parent
            {
                public string id { get; set; }
            }
            public class RootObject
            {
                public List<Category> categories { get; set; }
            }
            public static void GetCategories()
            {
                string url = "/sale/categories";

                List<Dal.ShopCategory> categories = new List<Dal.ShopCategory>();

                GetCategories(url, categories);

                Dal.ShopHelper sh = new Dal.ShopHelper();
                sh.SetCategories(Dal.Helper.ShopType.Allegro, categories);
            }
            private  static void GetCategories(string url, List<Dal.ShopCategory> categories)
            { 
                try
                {
                    HttpWebRequest request = GetHttpWebRequest(url, "GET", null, null);

                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    RootObject root = json_serializer.Deserialize<RootObject>(text);

                    SaveCategories(root, categories);
              
                }

                catch (WebException ex)
                {

                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (httpResponse == null)
                        {
                            Bll.ErrorHandler.SendError(ex, ex.Message);
                          

                        }
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string text = reader.ReadToEnd();


                            Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania kategorii {0} ", text));
                        }
                    } 
                }
            }

            private static void SaveCategories(RootObject root,List<Dal.ShopCategory> categories)
            {
                foreach(Category category in root.categories)
                {
                    Dal.ShopCategory sc = new Dal.ShopCategory()
                    {
                        CategoryOrder = 0,
                        IsActive = true,
                        IsAllowed = category.leaf,
                        IsPublished = true,
                        Name = category.name,
                        ShopCategoryId = category.id,
                        Url="",
                        ShopTypeId=(int)Dal.Helper.ShopType.Allegro
                    };
                    if (category.parent != null)
                    {
                        sc.CategoryParentId = category.parent.id;
                    }
                    if(category.leaf==false)
                    { 
                        string url = String.Format("/sale/categories?parent.id={0}", category.id);
                        GetCategories(url, categories);
                    }
                        categories.Add(sc);
                }
            }
        }
    }
}