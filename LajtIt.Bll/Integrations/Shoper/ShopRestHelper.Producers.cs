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
        public  class Producers
        {

            #region clasess
            public class PlPL
            {
                public string translation_id { get; set; }
                public string producer_id { get; set; }
                public string description { get; set; }
                public string lang_id { get; set; }
                public string seo_title { get; set; }
                public string seo_description { get; set; }
                public string seo_keywords { get; set; }
                public string seo_url { get; set; }
            }

            public class Translations
            {
                public PlPL pl_PL { get; set; }
            }
            public class Gfx
            {
                public string file    {get;set;}
                public string content { get; set; }
            }



            public class Producer
            {
                public int producer_id { get; set; }
                public string name { get; set; }
                public string web { get; set; }
                public string gfx { get; set; }
                public string isdefault { get; set; }
                public string in_loyalty { get; set; }
                public Translations translations { get; set; }
            }

            public class RootProducer
            {
                public string count { get; set; }
                public int pages { get; set; }
                public int page { get; set; }
                public List<Producer> list { get; set; }
            }

            #endregion


            #region REST
            public static List<Producer> GetProducers(Dal.Helper.Shop shop)
            {
                List<Producer> producers = new List<Producer>();
                GetProducers(shop, 1, producers);

                return producers;
            }
            private static void GetProducers(Dal.Helper.Shop shop, int pageId, List<Producer> producers)
            {

                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, String.Format("/webapi/rest/producers?limit=50&page={0}", pageId), "GET");



                    string text = GetTextFromWebResponse(request);



                    var json_serializer = new JavaScriptSerializer();

                    RootProducer rootProducer = json_serializer.Deserialize<RootProducer>(text);

                    producers.AddRange(rootProducer.list);

                    if (rootProducer.page < rootProducer.pages)
                        GetProducers(shop, rootProducer.page + 1, producers);
                }

                catch (WebException ex)
                {
                    Bll.ShopRestHelper.ProcessException(ex, shop, "Pusto");

                }
            }
            public static int SetProducer(Dal.Helper.Shop shop, string name)
            {

                try
                { 
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, "/webapi/rest/producers", "POST");

                    Producer producer = new Producer()
                    {
                        name = name,
                        translations = new Translations()
                        {
                            pl_PL = new PlPL()
                            {
                                
                            }
                        },
                        gfx = null,
                        web = ""
                    };


                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(producer);
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
                    ProcessException(ex, shop, "Pusto");
                    return 0;
                }
            }

            internal static void SetProducers(Dal.Helper.Shop s)
            {

                Dal.ShopHelper sh = new Dal.ShopHelper();

                List<Dal.SupplierShop> shopSuppliers = sh.GetSupplierShop((int)s);

                List<Dal.SupplierShop> ssToAdd = shopSuppliers.Where(x => x.IsActive && x.ShopProducerId.HasValue == false).ToList();// && !existingSuppliers.Contains(x.ProducerId))

                

                //List<Producer> producers = GetProducers(s);
                //int[] existingSuppliers = producers.Select(x => x.producer_id).ToArray();

                foreach (Dal.SupplierShop ss in ssToAdd)
                {
                    int producerId = SetProducer(s, ss.Supplier.Name);

                    if (producerId != 0)

                        sh.SetSupplierShopProducer(ss, producerId);

                }

            }

            #endregion

        }

    }
}