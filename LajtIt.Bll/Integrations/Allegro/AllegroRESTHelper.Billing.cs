using LajtIt.Dal;
using Remotion.Collections;
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
        public class Billing
        {

            public class BillingType
            {
                public string id { get; set; }
                public string description { get; set; }

            }

            public class BillingTypes
            {
                public List<BillingType> Billings { get; set; }

            }

            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
            public class Type
            {
                public string id { get; set; }
                public string name { get; set; }

            }

            public class Offer
            {
                public string id { get; set; }
                public string name { get; set; }

            }

            public class Value
            {
                public string amount { get; set; }
                public string currency { get; set; }

            }

            public class Tax
            {
                public string percentage { get; set; }

            }

            public class Balance
            {
                public string amount { get; set; }
                public string currency { get; set; }

            }

            public class BillingEntry
            {
                public string id { get; set; }
                public DateTime occurredAt { get; set; }
                public Type type { get; set; }
                public Offer offer { get; set; }
                public Value value { get; set; }
                public Tax tax { get; set; }
                public Balance balance { get; set; }

            }

            public class BillingEntries
            {
                public List<BillingEntry> billingEntries { get; set; }

            }

            public static void GetBilling()
            {
                Dal.AllegroScan asc = new Dal.AllegroScan();
                List<Dal.AllegroUser> users = asc.GetAllegroMyUsers().Where(x=>x.UserId== 44282528).ToList();
                foreach (Dal.AllegroUser user in users)
                {
                    GetBilling(user.UserId, 0);
                }
                 
            }

            private static void GetBilling(long userId, int offset)
            {
                try
                {

                    HttpWebRequest request = GetHttpWebRequest(String.Format("/billing/billing-entries?offset={0}&limit=100", offset), "GET", null, userId);


                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    BillingEntries be = json_serializer.Deserialize<BillingEntries>(text);

                    Dal.AllegroRestHelper arh = new AllegroRestHelper();

                    List<Dal.AllegroBilling> billing = new List<AllegroBilling>();

                    foreach (BillingEntry b in be.billingEntries)
                    {
                        Dal.AllegroBilling abe = new AllegroBilling()
                        {
                            Amount = Decimal.Parse(b.value.amount, System.Globalization.CultureInfo.InvariantCulture),
                            BillingTypeId = b.type.id,
                            InsertDate = b.occurredAt,
                            Tax = Decimal.Parse(b.tax.percentage, System.Globalization.CultureInfo.InvariantCulture),
                            Id = Guid.Parse(b.id)
                        };
                        if (b.offer != null)
                            abe.ItemId = Int64.Parse(b.offer.id);

                        billing.Add(abe);
                    }
                    arh.SetAllegroBilling(billing);


                    if (be.billingEntries.Count >= 100)
                        GetBilling(userId, offset + 1);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania dyskusji dla konta {0}", userId));

                }
            }
 

            public static void GetBillingTypes()
            {
                try
                {

                    HttpWebRequest request = GetHttpWebRequest("/billing/billing-types", "GET", null, (long)Dal.Helper.MyUsers.JacekStawicki);


                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    List<BillingType> bt = json_serializer.Deserialize<List<BillingType>>(text);

                    Dal.AllegroRestHelper arh = new AllegroRestHelper();
                    arh.SetAllegroBillingTypes(
                        bt.Select(x => new Dal.AllegroBillingTypes()
                        {
                            Description = x.description,
                            Id = x.id
                        }
                        ).ToList());

                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania dyskusji dla konta "));

                }
            }
        }
    }
}