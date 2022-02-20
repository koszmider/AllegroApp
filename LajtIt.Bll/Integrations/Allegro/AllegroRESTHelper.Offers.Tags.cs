

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using LajtIt.Dal;
using LinqToExcel.Extensions;

namespace LajtIt.Bll
{
    public partial class AllegroRESTHelper
    {
        public class Tags
        {
            public class Tag
            {
                public Guid id { get; set; }
                public string name { get; set; }
                public bool hidden { get; set; }
            }

            public class RootTag
            {
                public List<Tag> tags { get; set; }
            }



            //public static void CreateTags()
            //{
            //    Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();
            //    Bll.AllegroHelper ah = new Bll.AllegroHelper();

            //    List<Dal.ProductCatalogAllegroDraftItemsFnResult> items = oh.GetProductCatalogAllegroDraftItems();

            //    foreach (Dal.ProductCatalogAllegroDraftItemsFnResult d in items)
            //    {

            //        ProductCatalogAllegroItemsFnResult item = GetItemFromDraft(d);

            //        HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offers", ""), "POST", null, item.UserId);

            //        Offer.RootObject offer = GetOfferDraft(item);

            //        Draft draft = SetOffer(request, item, offer);

            //        ah.SetProductCatalogAllegroDraftItem(item, draft);
            //    }
            //}
            public static void GetTags()
            {
                foreach (Dal.Helper.MyUsers e in Enum.GetValues(typeof(Dal.Helper.MyUsers)))
                {
                    try
                    {
                        long userId = (long)e;
                        HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offer-tags?user.id={0}", userId), "GET", null, userId);


                        string text = null;
                        using (WebResponse webResponse = request.GetResponse())
                        {
                            Stream responseStream = webResponse.GetResponseStream();
                            StreamReader reader = new StreamReader(responseStream);
                            text = reader.ReadToEnd();
                        }

                        var json_serializer = new JavaScriptSerializer();
                        RootTag tags = json_serializer.Deserialize<RootTag>(text);

                        SetTags(userId, tags);
                    }
                    catch (WebException ex)
                    {
                        using (WebResponse response = ex.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            if (httpResponse != null && httpResponse.StatusCode.ToString().Contains("403"))
                            {

                                Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                                using (Stream data = response.GetResponseStream())
                                using (var reader = new StreamReader(data))
                                {
                                    string text = reader.ReadToEnd();


                                    var json_serializer = new JavaScriptSerializer();
                                    Exception1.RootObject exErr = json_serializer.Deserialize<Exception1.RootObject>(text);

                                    Bll.ErrorHandler.SendError(ex, text);

                                }
                            }
                        }
                    }
                }
            }

            private static void SetTags(long userId, RootTag tags)
            {
                List<Dal.AllegroTags> allegroTags = tags.tags.Select(x =>
                new AllegroTags()
                {
                    IsHidden = x.hidden,
                    Name = x.name,
                    TagId = x.id,
                    UserId = userId
                }).ToList();


                Dal.AllegroRestHelper arh = new AllegroRestHelper();

                arh.SetTags(allegroTags);

            }
        }
    }
}