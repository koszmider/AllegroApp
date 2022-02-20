using LajtIt.Dal;
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
        public class Badges
        {
            #region Classess
            public class Eligibility
            {
                public bool eligible { get; set; }
                public List<object> refusalReasons { get; set; }
            }

            public class Application
            {
                public string type { get; set; }
                public DateTime? from { get; set; }
                public DateTime? to { get; set; }
            }

            public class Publication
            {
                public string type { get; set; }
                public DateTime? from { get; set; }
                public DateTime? to { get; set; }
            }

            public class Visibility
            {
                public string type { get; set; }
                public DateTime? from { get; set; }
                public DateTime? to { get; set; }
            }

            public class BadgeCampaign
            {
                public string id { get; set; }
                public string name { get; set; }
                public string type { get; set; }
                public Eligibility eligibility { get; set; }
                public Application application { get; set; }
                public Publication publication { get; set; }
                public Visibility visibility { get; set; }
                public string regulationsLink { get; set; }
            }

            public class RootBadge
            {
                public List<BadgeCampaign> badgeCampaigns { get; set; }
            }


            public class Campaign
            {
                public string id { get; set; }
            }

            public class Offer
            {
                public long id { get; set; }
            }

            public class Price
            {
                public decimal amount { get; set; }
                public string currency { get; set; }
            }



            public class Prices
            {
                public Price market { get; set; }
                public Price bargain { get; set; }
            }

            public class RootBadgeApply
            {
                public Campaign campaign { get; set; }
                public Offer offer { get; set; }
                public Prices prices { get; set; }
            }

            public class RootBadgeApplyResponse
            {
                public Guid id { get; set; }
                public DateTime createdAt { get; set; }
                public DateTime updatedAt { get; set; }
                public Campaign campaign { get; set; }
                public Offer offer { get; set; }
                public Prices prices { get; set; }
                public Process process { get; set; }
            }

            public class Badge
            {
                public Offer offer { get; set; }
                public Campaign campaign { get; set; }
                public Publication publication { get; set; }
                public Prices prices { get; set; }
                public Process process { get; set; }
            }

            public class RootBadges
            {
                public List<Badge> badges { get; set; }
            }



            public class Message
            {
                public string text { get; set; }
                public object link { get; set; }
            }

            public class RejectionReason
            {
                public string code { get; set; }
                public List<Message> messages { get; set; }
            }

            public class Process
            {
                public string status { get; set; }
                public List<RejectionReason> rejectionReasons { get; set; }
            }

            public class BadgeApplication
            {
                public Guid id { get; set; }
                public DateTime createdAt { get; set; }
                public DateTime updatedAt { get; set; }
                public Campaign campaign { get; set; }
                public Offer offer { get; set; }
                public Prices prices { get; set; }
                public Process process { get; set; }
            }

            public class RootBadgeApplication
            {
                public List<BadgeApplication> badgeApplications { get; set; }
            }


            #endregion

            #region Rest
            public static List<BadgeCampaign> GetBadges()
            {
                try
                {
                    HttpWebRequest request = GetHttpWebRequest("/sale/badge-campaigns", "GET", null, null);

                    request.ContentType = "application/vnd.allegro.beta.v1+json";
                    request.Accept = "application/vnd.allegro.beta.v1+json";
                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    RootBadge item = json_serializer.Deserialize<RootBadge>(text);

                    return item.badgeCampaigns;
                    //  return item.deliveryMethods;
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, "Błąd pobierania dostępnych metod dostaw");
                    return null;
                }
            }

            private static RootBadgeApplyResponse SetBadge(Dal.AllegroItemBadgeView item)
            {
                try
                {
                    HttpWebRequest request = GetHttpWebRequest("/sale/badges", "POST", item.ItemId, null);

                    request.ContentType = "application/vnd.allegro.beta.v1+json";
                    request.Accept = "application/vnd.allegro.beta.v1+json";
                    string text = null;


                    RootBadgeApply badge = new RootBadgeApply()
                    {
                        campaign = new Campaign()
                        {
                            id = item.BadgeId
                        },
                        offer = new Offer()
                        {
                            id = item.ItemId
                        },
                        prices = new Prices()
                        {
                            bargain = new Price() { amount = item.BadgePrice, currency = "PLN" },
                            market = new Price() { amount = item.PriceBruttoFixed, currency = "PLN" },

                        }
                    };
                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(badge);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();


                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    RootBadgeApplyResponse badgeResponse = json_serializer.Deserialize<RootBadgeApplyResponse>(text);

                    return badgeResponse;
                }
                catch (WebException ex)
                {
                    Bll.ShopRestHelper.ProcessException(ex, Dal.Helper.Shop.Oswietlenie_Lodz,
                        String.Format("Błąd tworzenia Badge. ItemId: {0}", item.ItemId));
                    // Bll.ErrorHandler.SendError(ex, "Błąd pobierania dostępnych metod dostaw");
                    return null;
                }
            }

            private static List<Badge> GetBadgeApplication(long itemId)
            {
                try
                {
                    HttpWebRequest request =
                        GetHttpWebRequest(String.Format("/sale/badges?offer.id={0}", itemId), "GET", itemId, null);

                    request.ContentType = "application/vnd.allegro.beta.v1+json";
                    request.Accept = "application/vnd.allegro.beta.v1+json";
                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    RootBadges item = json_serializer.Deserialize<RootBadges>(text);

                    return item.badges;
                    //  return item.deliveryMethods;
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, "Błąd pobierania dostępnych metod dostaw");
                    return null;
                }
            }
            #endregion

            #region help functions
            public static void SetBadges()
            {
                List<Dal.AllegroItemBadgeView> items = Dal.AllegroRestHelper.GetAllegroItemBadgesToCreate();

                foreach (Dal.AllegroItemBadgeView item in items)
                {
                    int priceRebateFrom = (int)(item.RebateFrom * 100);
                    int priceRebateTo = (int)(item.RebateTo * 100);


                    Random random = new Random();
                    decimal randomRebate = (decimal)(random.Next(priceRebateFrom, priceRebateTo) / 10000.00);

                    decimal badgePrice = item.PriceBruttoFixed * (1 - randomRebate);

                    item.BadgePrice = Math.Round(badgePrice, 2);

                    RootBadgeApplyResponse badgeResponse = SetBadge(item);

                    if (badgeResponse != null)
                        SetBadgeInternal(item, badgeResponse);
                }

            }

            private static void SetBadgeInternal(AllegroItemBadgeView item, RootBadgeApplyResponse badgeResponse)
            {
                item.RequestStatus = badgeResponse.process.status;
                item.RequestRejectReasons = GetRejectReasons(badgeResponse.process.rejectionReasons);
                item.ApplicationId = badgeResponse.id;

                Dal.AllegroRestHelper.SetAllegroBadgeUpdate(item);
            }

            public static void GetBadgeCampaignApplications()
            {
                Dal.AllegroScan asc = new Dal.AllegroScan();
                List<AllegroUser> users = asc.GetAllegroMyUsers();
                int limit = 1000;
                int offset = 0;

                List<Dal.AllegroBadge> badges = Dal.AllegroRestHelper.GetAllegroBadges();

                foreach (Dal.AllegroUser user in users)
                {
                    foreach (Dal.AllegroBadge badge in badges)
                        GetBadgeCampaignApplications(user.UserId, limit, offset, badge.BadgeId);

                }

            }

            public static void GetBadgeCampaignApplications(long userId, int limit, int offset, string badgeId)
            {

                try
                {
                    HttpWebRequest request =
                        GetHttpWebRequest(String.Format("/sale/badges?limit={0}&offset={1}&campaign.id={2}", 
                        limit, offset, badgeId), "GET", null, userId);

                    request.ContentType = "application/vnd.allegro.beta.v1+json";
                    request.Accept = "application/vnd.allegro.beta.v1+json";
                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    RootBadges bc = json_serializer.Deserialize<RootBadges>(text);

                    ProcessBadgeApplications(bc);

                    if (bc.badges.Count <= limit && bc.badges.Count>0)
                        GetBadgeCampaignApplications(userId, limit, offset + limit, badgeId);
                }
                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, Dal.Helper.Shop.Oswietlenie_Lodz, 
                        String.Format("Błąd pobierania listy kampanii. UserId: {0}, offset: {1}", userId, offset));
                  
                }
            }

            private static void ProcessBadgeApplications(RootBadges bc)
            {
                List<Dal.AllegroItemBadge> badges =
                    bc.badges.Select(x => new AllegroItemBadge()
                    {
                        ItemId = x.offer.id,
                        BadgeId = x.campaign.id,
                        //ApplicationId = x.i,
                        RequestRejectReasons = GetRejectReasons(x.process.rejectionReasons),
                        RequestStatus = x.process.status,
                       // LastUpdateDate = x.
                    }).ToList();

                Dal.AllegroRestHelper.SetAllegroBadgeUpdate(badges);
            }

            //public static void GetBadgeApplications()
            //{

            //    List<Dal.AllegroItemBadgeView> items = Dal.AllegroRestHelper.GetAllegroItemBadgesToCheck();

            //    long[] userIds = items.Select(x => x.UserId).Distinct().ToArray();


            //    foreach (Dal.AllegroItemBadgeView item in items)
            //    {
            //        List<Badge> badges = GetBadgeApplication(item.ItemId);

            //        foreach (Badge badge in badges)
            //        {
            //            Dal.AllegroItemBadgeView aib = new AllegroItemBadgeView()
            //            {
            //                ItemId = item.ItemId,
            //                BadgeId = badge.campaign.id,
            //                RequestStatus = badge.process.status,
            //                RequestRejectReasons = GetRejectReasons(badge.process.rejectionReasons)
            //            };

            //            Dal.AllegroRestHelper.SetAllegroBadgeUpdate2(aib);
            //        }

            //    }

            //}

            private static string GetRejectReasons(List<RejectionReason> rejectionReasons)
            {
                if (rejectionReasons.Count == 0)
                    return null;


                return Bll.RESTHelper.ToJson(rejectionReasons);
            }
            #endregion


        }
    }
}