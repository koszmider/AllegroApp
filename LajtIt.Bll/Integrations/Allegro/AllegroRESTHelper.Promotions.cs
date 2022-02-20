using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class AllegroRESTHelper
    {

        public static string SetPromotion(Dal.ProductCatalogAllegroItemsActive item)
        {
            string text = "";
            string jsonEncodedParams = "";
            try
            {
 


                HttpWebRequest request = GetHttpWebRequest("/sale/loyalty/promotions", "POST", item.ItemId, null);

                PromotionRootObject prom = new PromotionRootObject();
                PromotionBenefit ben = new PromotionBenefit();
                PromotionSpecification spec = new PromotionSpecification();
                PromotionConfiguration conf = new PromotionConfiguration();
                PromotionTrigger trg = new PromotionTrigger();
                trg.discountedNumber = "1";
                trg.forEachQuantity = item.AllegroDiscountQty.ToString(); // na którą sztukę
                conf.percentage = item.AllegroDiscountValue.ToString();
                /*
                 * Minimalne wartości rabatów wynoszą:
                    15% na drugą sztukę,
                    30% na trzecią,
                    40% na czwartą,
                    50% na piątą.

                    100% oznacza, że dany przedmiot jest gratis.
                */
                spec.configuration = conf;
                spec.type = "UNIT_PERCENTAGE_DISCOUNT";
                spec.trigger = trg;
                ben.specification = spec;
                prom.benefits = new List<PromotionBenefit>();
                prom.benefits.Add(ben);


                PromotionOfferCriteria crt = new PromotionOfferCriteria();
                crt.type = "CONTAINS_OFFERS";

                crt.offers = new List<PromotionOffer>();
                crt.offers.Add(new PromotionOffer()
                {
                    id = item.ItemId.ToString()
                });

                prom.offerCriteria = new List<PromotionOfferCriteria>();
                prom.offerCriteria.Add(crt);



                Stream dataStream = request.GetRequestStream();
                jsonEncodedParams = ToJson(prom);
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();


                WebResponse webResponse = request.GetResponse();

                Stream responseStream = webResponse.GetResponseStream();

                StreamReader reader = new StreamReader(responseStream);

                text = reader.ReadToEnd();


                var json_serializer = new JavaScriptSerializer();
                PromotionCreatedRootObject promotion = json_serializer.Deserialize<PromotionCreatedRootObject>(text);

                Console.WriteLine(String.Format("ItemId: {0} OK", item.ItemId));

                return promotion.id;

            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("ItemId: {0} FAIL {1} ", item.ItemId, jsonEncodedParams));
                Bll.ErrorHandler.LogError(ex, String.Format("Błąd tworzenia promocji. Allegro.ItemId: {0}. Text {1}", item.ItemId, text));
                return null;
            }
        }


        public static void SetPromotionDelete(string id, long userId)
        {
            try
            {

                HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/loyalty/promotions/{0}", id), "DELETE", null, userId);

                using (WebResponse webResponse = request.GetResponse())
                { }


            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd aktualizacji ceny. Allegro.UserId: {0}, Promotion.Id", userId, id));
            }
        }


        //public static void SetPromotionsDelete()
        //{
        //    try
        //    {
        //        Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

        //        List<Dal.ProductCatalogAllegroItemDiscountsDeleteView> items = pch.GetProductCatalogAllegroItemPromotionsToDelete();

        //        foreach (Dal.ProductCatalogAllegroItemDiscountsDeleteView item in items)
        //        {
        //            SetPromotionDelete(item.AllegroDiscountId, item.UserId);
        //            pch.SetProductCatalogAllegroItemPromotionsDelete(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Bll.ErrorHandler.SendError(ex, String.Format("Błąd SetPromotionsDelete"));
        //    }
        //}


        internal static void SetPromotionsDeleteAll()
        {
            while (GetPromotions(44282528)) { }
        }


        public static bool GetPromotions(int userId)
        {
            try
            {

                HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/loyalty/promotions?user.id={0}", userId), "GET", null, userId);


                string text = null;
                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }

                var json_serializer = new JavaScriptSerializer();
                PromotionListRootObject promotions = json_serializer.Deserialize<PromotionListRootObject>(text);
                Console.WriteLine(String.Format("Liczba promocji: {0}:", promotions.totalCount));


                foreach (PromotionListPromotion promotion in promotions.promotions)
                    SetPromotionDelete(promotion.id, userId);

                return promotions.totalCount > 50;
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd aktualizacji ceny. Allegro.UserId: {0}", userId));
                return false;
            }
        }
        internal static void SetPromotions()
        {

            Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogAllegroItemsActive> items = pc.GetProductCatalogAllegroItemsActiveForDiscounts().ToList();

            int[] supplierIds = items.Select(x => x.SupplierId).Distinct().ToArray();


            foreach (int supplierId in supplierIds)
            {
                List<Dal.ProductCatalogAllegroItemsActive> itemsForSupplier = items.Where(x => x.SupplierId == supplierId).ToList();

                foreach (Dal.ProductCatalogAllegroItemsActive item in items)
                {
                    string id = SetPromotion(item);

                    if (id != null)
                        pc.SetProductCatalogAllegroItemPromotionId(item.Id, id);
                }

            }


        }


    }
}
