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
        public  class SpecialOffers
        {

            public class SpecialOffer
            {
                public string promo_id { get; set; }
                public string discount { get; set; }
                public string discount_wholesale { get; set; }
                public string discount_special { get; set; }
                public string date_from { get; set; }
                public string date_to { get; set; }
                public string product_id { get; set; }
            }

            public class SpecialOffersRoot
            {
                public string count { get; set; }
                public int pages { get; set; }
                public int page { get; set; }
                public List<SpecialOffer> list { get; set; }
            }





            #region REST

            //public static void GetSpecialOffers(Dal.Helper.Shop shop)
            //{
            //    GetSpecialOffers(shop, 0);
            //}
            //private static void GetSpecialOffers(Dal.Helper.Shop shop, int page)
            //{

            //    try
            //    {
            //        //&filters ={""date_to"":{""="":""" + DateTime.Now.ToString("yyyy-MM-dd") + @"""},""date_from"":{""<"":""" + DateTime.Now.ToString("yyyy-MM-dd") + @"""}}
            //        string url = @"/webapi/rest/specialoffers?limit=25&page=" + page.ToString();
            //        HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, url, "GET");

            //        string text = GetTextFromWebResponse(request);

            //        var json_serializer = new JavaScriptSerializer();

            //        SpecialOffersRoot specialOffers = json_serializer.Deserialize<SpecialOffersRoot>(text);


            //        if (page == 0 && Int32.Parse(specialOffers.count)>0)
            //            GetSpecialOffers(shop, specialOffers.pages);



            //        List<Bulk.BulkObject> bulkObjects = new List<Bulk.BulkObject>();

            //        foreach (SpecialOffer specialOffer in specialOffers.list)
            //        {
            //            if (DateTime.Parse(specialOffer.date_to) < DateTime.Now)
            //            {
            //                bulkObjects.Add(
            //                    new Bulk.BulkObject()
            //                    {
            //                        id = String.Format("promo-delete_{0}", specialOffer.promo_id),
            //                        body = null,
            //                        method = "DELETE",
            //                        path = String.Format("/webapi/rest/specialoffers/{0}", specialOffer.promo_id)
            //                    });
            //            }
            //        }
            //        if (bulkObjects.Count > 0)
            //        { 
            //            Bulk.BulkResult result = Bulk.Sent(shop, bulkObjects);
            //        }

            //        if (page >0)
            //            GetSpecialOffers(shop, page -1);




            //    }

            //    catch (WebException ex)
            //    {
            //        ProcessException(ex, shop);

            //    }
            //}

            //internal static void SetPromoDelete(Dal.Helper.Shop shop, string promo_id)
            //{
            //    try
            //    {
            //        HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/specialoffers/{0}", promo_id), "DELETE");

            //        string text = GetTextFromWebResponse(request);
            //    }

            //    catch (WebException ex)
            //    {
            //        ProcessException(ex, shop, String.Format("Delete Promo_Id: {0}", promo_id));

            //    }
            //}
            internal static void SetPromoFindAndDelete(Dal.Helper.Shop shop, string[] product_id)
            {
                try
                {
                    //&filters ={""date_to"":{""="":""" + DateTime.Now.ToString("yyyy-MM-dd") + @"""},""date_from"":{""<"":""" + DateTime.Now.ToString("yyyy-MM-dd") + @"""}}
                    //string url = @"/webapi/rest/specialoffers?filters={""product_id"":{"""":""" + product_id+ @"""}""";

                    string url = @"/webapi/rest/specialoffers?filters={""product_id"":{""IN"":[" + String.Join(",", product_id) + @"]}}";


                    //       string url = @"/webapi/rest/specialoffers?filters={""date_to"":{""<"":""" 
                    //+ DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + @"""},""date_from"":{""<"":""" 
                    //   + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + @"""}}";
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, url, "GET");

                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    SpecialOffersRoot specialOffers = json_serializer.Deserialize<SpecialOffersRoot>(text);
                    List<Bulk.BulkObject> objects = new List<Bulk.BulkObject>();


                    foreach (SpecialOffer so in specialOffers.list)
                    {
                        objects.Add(new Bulk.BulkObject()
                        {
                            body = null,
                            method = "DELETE",
                            id = String.Format("d{0}", so.promo_id),
                            path = String.Format("/webapi/rest/specialoffers/{0}", so.promo_id)
                        });
                        //SetPromoDelete(shop, so.promo_id);
                    }

                    ShopRestHelper.Bulk.Sent(shop, objects);

                    if (Int32.Parse(specialOffers.count) > 0)
                        SetPromoFindAndDelete(shop, product_id);


                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop);

                }
            }
            internal static void SetPromoFindAndDelete(Dal.Helper.Shop shop, string product_id)
            {
                try
                {
                    //&filters ={""date_to"":{""="":""" + DateTime.Now.ToString("yyyy-MM-dd") + @"""},""date_from"":{""<"":""" + DateTime.Now.ToString("yyyy-MM-dd") + @"""}}
                    //string url = @"/webapi/rest/specialoffers?filters={""product_id"":{""="":""" + product_id+ @"""}""";

                    string url = @"/webapi/rest/specialoffers?filters={""product_id"":{""="":""" + product_id + @"""}}";


             //       string url = @"/webapi/rest/specialoffers?filters={""date_to"":{""<"":""" 
             //+ DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + @"""},""date_from"":{""<"":""" 
             //   + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + @"""}}";
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, url, "GET");

                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    SpecialOffersRoot specialOffers = json_serializer.Deserialize<SpecialOffersRoot>(text);
                    List<Bulk.BulkObject> objects = new List<Bulk.BulkObject>();


                    foreach (SpecialOffer so in specialOffers.list)
                    {
                        objects.Add(new Bulk.BulkObject()
                        {
                            body = null,
                            method = "DELETE",
                            id = String.Format("d{0}", so.promo_id),
                            path= String.Format("/webapi/rest/specialoffers/{0}",so.promo_id)
                        });
                        //SetPromoDelete(shop, so.promo_id);
                    }

                    ShopRestHelper.Bulk.Sent(shop, objects);

                    if (Int32.Parse(specialOffers.count) > 0)
                        SetPromoFindAndDelete(shop, product_id);


                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop);

                }
            }

            public static void SetPromotionsDelete(Dal.Helper.Shop shop)
            {
                List<Dal.ProductCatalogShopProduct> products = Dal.DbHelper.ProductCatalog.GetProductCatalogShopProductsWithoutPromotions(shop);



                int partsCount = (products.Count / 25);

                for (int i = 0; i < partsCount + 1; i++)
                {
                    List<Dal.ProductCatalogShopProduct> productsPArt = products.Skip(i * 25).Take(25).ToList();

                    if (products.Count == 0)
                        continue;


                    SetPromoFindAndDelete(shop, productsPArt.Select(x => x.ShopProductId).ToArray());

                }
            }
            #endregion

        }

    }
}