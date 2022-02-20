using LajtIt.Dal;
using LinqToExcel.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using static LajtIt.Bll.ShopUpdateHelper.ClickShop;

namespace LajtIt.Bll
{
    public partial class ShopRestHelper
    {
        public class AttributeGroups
        {
            #region Classes
            public class AttributeGroup
            {
                public string name { get; set; }
                public int lang_id { get; set; }
                public int active { get; set; }
                public int filters { get; set; }
                public List<int> categories { get; set; }
                public int attribute_group_id { get; set; }
            }
            public class Attribute
            {
                public string name { get; set; }
                public string description { get; set; }
                public int attribute_group_id { get; set; }
                public int order { get; set; }
                public int type { get; set; }
                public int active { get; set; }
                //public string default { get; set; }
                public List<string> options { get; set; }
            }

            public class RootAttributeGroup
            {
                public string count { get; set; }
                public int pages { get; set; }
                public int page { get; set; }
                public List<AttributeGroup> list { get; set; }
            }
            #endregion

            #region REST

            public static List<AttributeGroup> GetAttributeGroups(Dal.Helper.Shop shop)
            {

                List<AttributeGroup> attributeGroups = new List<AttributeGroup>();

                GetAttributeGroups(shop, 1, attributeGroups);

                return attributeGroups;

            }
            private static void GetAttributeGroups(Dal.Helper.Shop shop, int page, List<AttributeGroup> attributeGroups)
            {

                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format(@"/webapi/rest/attribute-groups?limit=50&page={0}", page), "GET");

                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    RootAttributeGroup rootAttributeGroups = json_serializer.Deserialize<RootAttributeGroup>(text);

                    attributeGroups.AddRange(rootAttributeGroups.list);


                    if (rootAttributeGroups.page < rootAttributeGroups.pages)
                        GetAttributeGroups(shop, rootAttributeGroups.page + 1, attributeGroups);
                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop);

                }
            }



            public static void CreateAttributeGroupAndAssign(Dal.Helper.Shop shop, int externalAttributeGroupId, int attributeGroupId)
            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                Dal.ProductCatalogAttributeGroup group = pch.GetProductCatalogAttributeGroup(attributeGroupId);
                List<Dal.ProductCatalogAttribute> attributes = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributes(attributeGroupId);

                UpdateAttributeGroup(shop, externalAttributeGroupId, group.ExportToShop, true);

                Dal.ShopAttributeGroup sag = new ShopAttributeGroup()
                {
                    IsActive = false,
                    IsSearchable = false,
                    Name = group.Name,
                    ShopId = (int)shop,
                    SortOrder = externalAttributeGroupId,
                    ExternalShopAttributeGroupId = externalAttributeGroupId
                };
                List<Dal.ShopAttribute> sas = new List<ShopAttribute>();
                List<Dal.ProductCatalogShopAttribute> pcsas = new List<ProductCatalogShopAttribute>();
                if (group.AttributeGroupTypeId == 3)
                {
                    foreach (Dal.ProductCatalogAttribute attribute in attributes)
                    {
                        int extAttId = CreateAttribute(shop, externalAttributeGroupId, attribute);

                        Dal.ShopAttribute sa = new ShopAttribute()
                        {
                            ShopAttributeGroup = sag,
                            ExternalShopAttributeId = extAttId,
                            SortOrder = attribute.SortOrder,
                            Name = attribute.Name,
                            ExternalAttributeTypeId = 0,
                            IsActive = false
                        };
                        sas.Add(sa);


                        Dal.ProductCatalogShopAttribute pcsa = new ProductCatalogShopAttribute()
                        {
                            AttributeGroupId = attributeGroupId,
                            AttributeId = attribute.AttributeId,
                            ShopAttribute = sa
                        };

                        pcsas.Add(pcsa);
                    }
                }
                else
                {
                    int extAttId = CreateAttribute(shop, externalAttributeGroupId, group, attributes.Select(x => x.Name).ToList());

                    Dal.ShopAttribute sa = new ShopAttribute()
                    {
                        ShopAttributeGroup = sag,
                        ExternalShopAttributeId = extAttId,
                        SortOrder = 0,
                        Name = group.Name,
                        ExternalAttributeTypeId = 2,
                        IsActive = false
                    };
                    sas.Add(sa);
                    Dal.ProductCatalogShopAttribute pcsa = new ProductCatalogShopAttribute()
                    {
                        AttributeGroupId = attributeGroupId,
                        AttributeId = null,
                        ShopAttribute = sa
                    };

                    pcsas.Add(pcsa);
                }

                Dal.ShopHelper sh = new Dal.ShopHelper();
                sh.CreateAttributeGroupAndAssign(pcsas);



            }

            /// <summary>
            /// Tworzy atrybut wartiościowy
            /// </summary>
            /// <param name="shop"></param>
            /// <param name="externalAttributeGroupId"></param>
            /// <param name="attribute"></param>
            /// <returns></returns>
            public static int CreateAttribute(Dal.Helper.Shop shop, int externalAttributeGroupId, Dal.ProductCatalogAttribute attribute)
            {


                Attribute attr = new Attribute()
                {
                    attribute_group_id = externalAttributeGroupId,
                    name = attribute.Name,
                    type = 0,
                    active = attribute.ProductCatalogAttributeGroup.ExportToShop ? 1 : 0,
                    order = externalAttributeGroupId
                };


                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/attributes", ""), "POST");

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(attr);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();



                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    return Int32.Parse(text);

                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop);
                    return 0;

                }
            }

            /// <summary>
            /// Tworzy atrybut słownikowy
            /// </summary>
            /// <param name="shop"></param>
            /// <param name="externalAttributeGroupId"></param>
            /// <param name="group"></param>
            /// <param name="attributes"></param>
            /// <returns></returns>
            public static int CreateAttribute(Dal.Helper.Shop shop, int externalAttributeGroupId, Dal.ProductCatalogAttributeGroup group, List<string> attributes)
            {
                Attribute attr = new Attribute()
                {
                    attribute_group_id = externalAttributeGroupId,
                    name = group.Name,
                    type = group.AttributeGroupTypeId == 3 ? 0 : 2,
                    active = group.ExportToShop ? 1 : 0,
                    order = externalAttributeGroupId
                };
                if (group.AttributeGroupTypeId != 3)
                    attr.options = attributes;


                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/attributes", ""), "POST");

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(attr);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();



                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    return Int32.Parse(text);

                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop);
                    return 0;

                }
            }
            public static int UpdateAttribute(Dal.Helper.Shop shop, Dal.ProductCatalogShopAttribute pcShopAttribute)
            {
                if (pcShopAttribute == null || pcShopAttribute.ShopAttribute ==null|| pcShopAttribute.ShopAttribute.ShopAttributeGroup == null)
                    return 0;

                int attribute_group_id = pcShopAttribute.ShopAttribute.ShopAttributeGroup.ExternalShopAttributeGroupId;
                string name = pcShopAttribute.ProductCatalogAttributeGroup.AttributeGroupTypeId == 3 ? pcShopAttribute.ShopAttribute.Name : pcShopAttribute.ShopAttribute.ShopAttributeGroup.Name;
                int type = pcShopAttribute.ProductCatalogAttributeGroup.AttributeGroupTypeId == 3 ? 0 : 2;
                int active = pcShopAttribute.ProductCatalogAttributeGroup.ExportToShop ? 1 : 0;
                int order = pcShopAttribute.ShopAttribute.ShopAttributeGroup.ExternalShopAttributeGroupId;

                Attribute attr = new Attribute()
                {
                    attribute_group_id = attribute_group_id,
                    name = name,
                    description="",
                    type = type,
                    active =active,
                    order = order
                };
                if (pcShopAttribute.ProductCatalogAttributeGroup.AttributeGroupTypeId != 3)
                    attr.options = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributes(pcShopAttribute.AttributeGroupId.Value).Select(x => x.Name).ToList();


                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/attributes/{0}", pcShopAttribute.ShopAttribute.ExternalShopAttributeId), "PUT");

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(attr);

                    Console.WriteLine(jsonEncodedParams);

                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();


                    string text = GetTextFromWebResponse(request);
                    Console.WriteLine(text);

                    var json_serializer = new JavaScriptSerializer();

                    return Int32.Parse(text);

                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop, String.Format("ProductCatalogShopAttribute.Id={0}", pcShopAttribute.Id));
                    return 0;

                }
            }



            //public static int CreateAttribute(Dal.Helper.Shop shop, int externalAttributeGroupId, Dal.ProductCatalogAttributeGroup group, List<string> attributes)
            //{
            //    Attribute attr = new Attribute()
            //    {
            //        attribute_group_id = externalAttributeGroupId,
            //        name = group.Name,
            //        type = group.AttributeGroupTypeId == 3 ? 0 : 2,
            //        active = group.ExportToShop ? 1 : 0,
            //        order = externalAttributeGroupId
            //    };
            //    if (group.AttributeGroupTypeId != 3)
            //        attr.options = attributes;


            //    try
            //    {
            //        HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/attributes", ""), "POST");

            //        Stream dataStream = request.GetRequestStream();
            //        string jsonEncodedParams = Bll.RESTHelper.ToJson(attr);
            //        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            //        byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

            //        dataStream.Write(byteArray, 0, byteArray.Length);
            //        dataStream.Close();



            //        string text = GetTextFromWebResponse(request);

            //        var json_serializer = new JavaScriptSerializer();

            //        return Int32.Parse(text);

            //    }

            //    catch (WebException ex)
            //    {
            //        ProcessException(ex, shop);
            //        return 0;

            //    }
            //}
            public static int UpdateAttribute2(Dal.Helper.Shop shop)
            {
                //List<Dal.ProductCatalogShopAttribute> attributes = Dal.DbHelper.ProductCatalog.GetProductCatalogShopAttributes((int)Dal.Helper.Shop.Lajtitpl)
                //    .Where(x => x.ProductCatalogAttributeGroup.AttributeGroupTypeId != 3).ToList() ;

                List<Dal.ShopAttribute> shopAttributes = Dal.DbHelper.Shop.GetShopAttributes((int)Dal.Helper.Shop.Lajtitpl);

                //if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Dev)
                //{
                //    int[] a = new int[] { 1122, 1123, 1124, 1125, 1126 };
                //    shopAttributes = shopAttributes.Where(x => a.Contains(x.ShopAttributeId)).ToList();
                //}

                foreach (Dal.ShopAttribute shopAttribute in shopAttributes)
                {
                    //if (shopAttribute == null || shopAttribute.ShopAttribute == null || shopAttribute.ShopAttribute.ShopAttributeGroup == null)
                    //    return 0;

                    //int attribute_group_id = shopAttribute.ShopAttribute.ShopAttributeGroup.ExternalShopAttributeGroupId;
                    ////string name = shopAttribute.ShopAttribute.ShopAttributeGroup.Name;
                    //int type = /*pcShopAttribute.ProductCatalogAttributeGroup.AttributeGroupTypeId == 3 ? 0 :*/ 2;
                    //int active = 1;// pcShopAttribute.ProductCatalogAttributeGroup.ExportToShop ? 1 : 0;
                    //int order = shopAttribute.ShopAttribute.ShopAttributeGroup.ExternalShopAttributeGroupId;

                    Attribute attr = new Attribute()
                    {
                        attribute_group_id = shopAttribute.ShopAttributeGroup.ExternalShopAttributeGroupId,
                        // name = name,
                        //description = "",
                        // type = type,
                        //active = active,
                        // order = order
                    };

                    // sprawdz czy atrybut bierze udział w grupowaniu
                    attr.options = new List<string>();


                    Dal.ProductCatalogAttributeShopGroupingType shopGroupingType = Dal.DbHelper.Shop.GetProductCatalogAttributeShopGroupingType((int)Dal.Helper.Shop.Lajtitpl,
                        shopAttribute.ShopAttributeId);

                    if (shopGroupingType != null)
                    {
                        List<Dal.ProductCatalogAttribute> shopGroupingAttributes = Dal.DbHelper.Shop.GetProductCatalogAttributeShopGrouping(shopGroupingType.ShopGroupingTypeId);
                        attr.options = shopGroupingAttributes.Select(x => x.Name).ToList();
                    }

                    else
                    {
                        List<Dal.ProductCatalogAttribute> shopGroupingAttributes = Dal.DbHelper.Shop.GetProductCatalogShopAttributes((int)Dal.Helper.Shop.Lajtitpl,
    shopAttribute.ShopAttributeId);
                        attr.options = shopGroupingAttributes.Select(x => x.Name).ToList();
                    }

                   // continue;





                    try
                    {
                        HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/attributes/{0}", shopAttribute.ExternalShopAttributeId), "PUT");

                        Stream dataStream = request.GetRequestStream();
                        string jsonEncodedParams = Bll.RESTHelper.ToJson(attr);
                        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                        byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();


                        string text =   GetTextFromWebResponse(request);

                        var json_serializer = new JavaScriptSerializer();

                      //  return Int32.Parse(text);

                    }

                    catch (WebException ex)
                    {
                        ProcessException(ex, shop, String.Format("ProductCatalogShopAttribute.ShopAttributeId={0}", shopAttribute.ShopAttributeId));
                      

                    }
                }
                return 1;
            }



            /// <summary>
            /// aktualizuje grupę atrybutów
            /// </summary>
            /// <param name="shop"></param>
            /// <param name="externalGroupId"></param>
            /// <param name="isActive"></param>
            /// <param name="isSearchable"></param>
            /// <returns></returns>
            public  static bool UpdateAttributeGroup(Dal.Helper.Shop shop, int externalGroupId, bool isActive, bool isSearchable)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                List<Dal.ShopAttributeGroup> shopAttributeGroups = sh.ShopAttributeGroups((int)shop);

                Dal.ShopAttributeGroup group = shopAttributeGroups.Where(x => x.ExternalShopAttributeGroupId == externalGroupId).FirstOrDefault();
          

                if (group != null)
                {
                    AttributeGroup ag = new AttributeGroup()
                    {
                        active = isActive ? 1 : 0,
                        filters = isSearchable ? 1 : 0,
                        lang_id = 1,
                        name = group.Name
                    };

                    try
                    {
                        HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/attribute-groups/{0}", externalGroupId), "PUT");


                        Stream dataStream = request.GetRequestStream();
                        string jsonEncodedParams = Bll.RESTHelper.ToJson(ag);
                        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                        byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();



                        string text = GetTextFromWebResponse(request);

                        var json_serializer = new JavaScriptSerializer();

                        return Int32.Parse(text)==1;

                    }

                    catch (WebException ex)
                    {
                        ProcessException(ex, shop, String.Format("Błąd aktualizacji grupy atrybutów Id: {0}, Nazwa: {1}", externalGroupId, group.Name));


                    }
                }

                return false;


            }

            public class RootAttributeGroup1
            {
                public string name { get; set; }
                public string lang_id { get; set; }
                public string active { get; set; }
                public string filters { get; set; }
                public List<int> categories { get; set; }
                public string attribute_group_id { get; set; }
            }

            public static bool UpdateAttributeGroupDeleteCategories(Dal.Helper.Shop shop, int externalGroupId)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                List<Dal.ShopAttributeGroup> shopAttributeGroups = sh.ShopAttributeGroups((int)shop);

                Dal.ShopAttributeGroup group = shopAttributeGroups.Where(x => x.ExternalShopAttributeGroupId == externalGroupId).FirstOrDefault();


                if (group != null)
                {
                  

                    try
                    {
                        HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/attribute-groups/{0}", externalGroupId), "GET");


                    



                        string text = GetTextFromWebResponse(request);

                        var json_serializer = new JavaScriptSerializer();

                        RootAttributeGroup1 rag = json_serializer.Deserialize<RootAttributeGroup1>(text);


                        AttributeGroup ag = new AttributeGroup()
                        {
                            //active = isActive ? 1 : 0,
                            //filters = isSearchable ? 1 : 0,
                            lang_id = 1,
                            name = rag.name,
                            categories = null,// rag.categories.Take(10).ToList(),
                            attribute_group_id=group.ExternalShopAttributeGroupId
                        };

                         request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/attribute-groups/{0}", externalGroupId), "PUT");

                        Stream dataStream = request.GetRequestStream();
                        string jsonEncodedParams = Bll.RESTHelper.ToJson(ag);
                        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                        byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();

                         text = GetTextFromWebResponse(request);

                        return Int32.Parse(text) == 1;

                    }

                    catch (WebException ex)
                    {
                        ProcessException(ex, shop, String.Format("Błąd aktualizacji grupy atrybutów Id: {0}, Nazwa: {1}", externalGroupId, group.Name));


                    }
                }

                return false;


            }
            public static bool UpdateAttributeGroupDropCategories(Dal.Helper.Shop shop, int externalGroupId)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
             
                    AttributeGroup ag = new AttributeGroup()
                    {
                        categories =  new List<int>() { 498 },
                        lang_id = 1
                    };

                    try
                    {
                        HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/attribute-groups/{0}", externalGroupId), "PUT");


                        Stream dataStream = request.GetRequestStream();
                        string jsonEncodedParams = Bll.RESTHelper.ToJson(ag);
                        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                        byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();
                     

                        string text = GetTextFromWebResponse(request);

                        var json_serializer = new JavaScriptSerializer();

                        return Int32.Parse(text) == 1;

                    }

                    catch (WebException ex)
                    {
                        ProcessException(ex, shop, String.Format("Błąd aktualizacji grupy atrybutów Id: {0}", externalGroupId));


                    }
              

                return false;


            }
            public static int CreateAttributeGroup(Dal.Helper.Shop shop, string groupName, bool isActive)
            {
                /*
                 data (array) - tablica asocjacyjna z danymi obiektu o strukturze:
                ['name'] (string) - nazwa grupy atrybutów
                ['lang_id'] (int) - identyfikator języka
                ['active'] (int[0/1]) - aktywność grupy atrybutów
                ['categories'] (array) - tablica identyfikatorów kategorii, przypisanych do tej grupy atrybutów
                */

                AttributeGroup ag = new AttributeGroup()
                {
                    active = isActive?1:0,
                    categories = null,
                    filters = 1,
                    lang_id = 1,
                    name = groupName
                };
                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, "/webapi/rest/attribute-groups", "POST");


                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(ag);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();



                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    return Int32.Parse(text);

                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop);

                    
                }


                return 0; // (int)r;
            }

            #endregion


        }
    }
}