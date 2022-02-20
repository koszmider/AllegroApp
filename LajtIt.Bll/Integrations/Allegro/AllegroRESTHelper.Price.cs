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
        public class Fees
        {
            public class Fee
            {
                public string amount { get; set; }
                public string currency { get; set; }
            }

            public class Offer
            {
                public string id { get; set; }
            }

            public class Quote
            {
                public string type { get; set; }
                public string name { get; set; }
                public DateTime nextDate { get; set; }
                public Fee fee { get; set; }
                public Offer offer { get; set; }
                public bool enabled { get; set; }
            }

            public class RootObject
            {
                public int count { get; set; }
                public List<Quote> quotes { get; set; }
            }
        }

        public class BatchPriceChange
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
            public class Modification
            {
                public string type { get; set; }
            }

            public class Offer
            {
                public string id { get; set; }
            }

            public class OfferCriteria
            {
                public List<Offer> offers { get; set; }
                public string type { get; set; }
            }

            public class Root
            {
                public Modification modification { get; set; }
                public List<OfferCriteria> offerCriteria { get; set; }
            }


        }
        public static void GetFees()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();


            List<Dal.ProductCatalogAllegroItemsActive> items =    pch.GetProductCatalogAllegroItemsActive();
            foreach (Dal.Helper.MyUsers e in Enum.GetValues(typeof(Dal.Helper.MyUsers)))
            {
                int userId = (int)e;
                int total = items.Where(x => x.UserId == userId).Count();
                int skip = 0;
                int no = 20;
                //Dal.Helper.MyUsers e = Dal.Helper.MyUsers.Oswietlenie_Lodz;

                List<Fees.Quote> fees = new List<Fees.Quote>();

                Console.WriteLine(String.Format("User {0}", e));
                while (true)
                {
                    long[] itemIds = items.Where(x => x.UserId == userId).Skip(skip).Take(no).Select(x => x.ItemId).ToArray();
                    Fees.RootObject fee = GetFees(userId, itemIds);

                    if (fee != null)
                        fees.AddRange(fee.quotes);

                    skip += no;
                    Console.WriteLine(String.Format("Skip {0}", skip));
                    if (total < skip)
                        break;
                }

                fees = fees
                    .Where(x => x.nextDate.Date < DateTime.Now.AddDays(3).Date && x.type == "INEFFECTIVE_LISTING_FEE")
                    .ToList();

                long[] itemIdsToDelete = fees.Select(x => Int64.Parse(x.offer.id)).ToArray();
                Console.WriteLine(String.Join(",", itemIdsToDelete));
                var i = items.Where(x => itemIdsToDelete.Contains(x.ItemId)).ToList();

                Dal.OrderHelper oh = new Dal.OrderHelper();
                oh.SetAllegroActions(i, true, true, 1,  "Usunięte z modułu kosztów GetFees");

            }
        }
        public static Fees.RootObject GetFees(int userId, long[] itemIds)
        { 
            try
            {
                string ids = String.Join("&offer.id=", itemIds);

                HttpWebRequest request = GetHttpWebRequest(String.Format("/pricing/offer-quotes?offer.id={0}", ids), "GET", null, userId);
                 
                string text = null;
                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }

                var json_serializer = new JavaScriptSerializer();
                Fees.RootObject item = json_serializer.Deserialize<Fees.RootObject>(text);
                return item;
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Nie można pobrać opłat dla produktu. Allegro.ItemId: {0}", 7402098480));
                return null;
            } 
        }

      

        public static void ChangePrice(long itemId, float price)
        {
            float? currentPrice = GetPrice(itemId);

            if (currentPrice == null)
                return;


            if (price == currentPrice.Value)
                return;
            #region logika allegro
            /*
            Jeśli cena wynosi:
            do 50 zł - zwiększysz ją maksymalnie o 50 zł
            ponad 50 zł - zwiększysz ją maksymalnie o 100%
            */

            #endregion

            price = (float)Math.Round(price, 2);

            if (price < currentPrice.Value || price - currentPrice.Value <= 50)
            {
                //zmień na niższą lub wyższą do 50zł
                ChangePriceInternal(itemId, price);
                return;
            }
            else
            {
                ChangePriceInternal(itemId, currentPrice.Value + 50);
                ChangePrice(itemId, price);
            }
        }

      

        private static void ChangePriceInternal(long itemId, float price)
        { 
            try
            { 

                HttpWebRequest request = GetHttpWebRequest(String.Format("/offers/{0}/change-price-commands/{1}", itemId, Guid.NewGuid()), "PUT", itemId, null);


                Stream dataStream = request.GetRequestStream();

                BuyNowPriceRootObject b = new BuyNowPriceRootObject();
                Input input = new Input();
                BuyNowPrice buy = new BuyNowPrice()
                {
                    amount = price.ToString("0.00", new System.Globalization.CultureInfo("en-US")),
                    currency = "PLN"

                };
                input.buyNowPrice = buy;
                b.input = input;

                string jsonEncodedParams = Bll.RESTHelper.ToJson(b);
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();



                WebResponse webResponse = request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();

                Object response = Bll.RESTHelper.FromJson(text);
            }
            catch (WebException ex)
            {
                Bll.ShopRestHelper.ProcessException(ex, Dal.Helper.Shop.Oswietlenie_Lodz, 
                    String.Format("Błąd aktualizacji ceny. Allegro.ItemId: <a href='http://allegro.pl/show_item.php?item={0}'>{0}</a>", itemId));
            }
            catch (Exception ex)
            { 
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd aktualizacji ceny. Allegro.ItemId: {0}", itemId));
            }
        }



        public static float? GetPrice(long itemId)
        {
            float? price = null;
            try
            {

                HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offers/{0}", itemId), "GET", itemId, null);


                string text = null;
                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }

                var json_serializer = new JavaScriptSerializer();
                ItemRootObject item = json_serializer.Deserialize<ItemRootObject>(text);
                // Console.WriteLine(String.Format("Liczba promocji: {0}:", promotions.totalCount));

                //price = Convert.tod(item.sellingMode.price.amount.Replace(".", ","));
                price = float.Parse(item.sellingMode.price.amount, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Nie można pobrać ceny produktu. Allegro.ItemId: {0}", itemId));
            }
            return price;
        }

    }
}
