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
        public class Payments
        {

            #region Classes

            public class Balance
            {
                public decimal amount { get; set; }
                public string currency { get; set; }
            }

            public class Wallet
            {
                public string paymentOperator { get; set; }
                public string type { get; set; }
                public Balance balance { get; set; }
            }

            public class Value
            {
                public decimal amount { get; set; }
                public string currency { get; set; }
            }

            public class Payment
            {
                public Guid id { get; set; }
            }

            public class Address
            {
                public string street { get; set; }
                public string city { get; set; }
                public string postCode { get; set; }
            }

            public class Participant
            {
                public long id { get; set; }
                public string companyName { get; set; }
                public string login { get; set; }
                public string firstName { get; set; }
                public string lastName { get; set; }
                public Address address { get; set; }
            }

            public class PaymentOperation
            {
                public string type { get; set; }
                public string group { get; set; }
                public Wallet wallet { get; set; }
                public DateTime occurredAt { get; set; }
                public Value value { get; set; }
                public Payment payment { get; set; }
                public Participant participant { get; set; }
            }

            public class RootPayments
            {
                public List<PaymentOperation> paymentOperations { get; set; }
                public int count { get; set; }
                public int totalCount { get; set; }
            }
            #endregion
            public static void GetPayments()
            {
                Dal.AllegroScan asc = new Dal.AllegroScan();
                List<Dal.AllegroUser> users = asc.GetAllegroMyUsers();
                foreach (Dal.AllegroUser user in users)
                {
                    GetPayments(user.UserId, 0);
                }

              
            }

            private static void GetPayments(long userId, int offset)
            {
                try
                {
                    HttpWebRequest request = GetHttpWebRequest(String.Format("/payments/payment-operations?offset={0}&occurredAt.gte={1}", offset,
                        DateTime.UtcNow.AddMonths(-2).ToString("o")), "GET", null, userId);
 
                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    RootPayments payments = json_serializer.Deserialize<RootPayments>(text);

                    bool takeNext = ProcessPayments(userId, payments);

                    if (takeNext && payments.totalCount > offset)
                        GetPayments(userId, offset + 50);

                }
                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, Dal.Helper.Shop.Oswietlenie_Lodz, null); 
                }
            }

            private static bool ProcessPayments(long userId, RootPayments payments)
            {
                List<Dal.AllegroPayments> p = payments.paymentOperations.Select(x => new AllegroPayments()
                {
                    UserId = userId,
                    Amount = x.value.amount,
                    BuyerLogin = GetLogin(x),//.participant.login,
                    BuyerUserId = GetId(x), //.participant.id,
                    CurrencyCode = x.value.currency,
                    GroupType = x.group,
                    OccuredAt = x.occurredAt,
                    PaymentId = GetPaymentId(x),//.payment.id,
                    PaymentType = x.type,
                    WalletBalance = x.wallet.balance.amount,
                    WalletType = x.wallet.type,
                    PaymentOperator = x.wallet.paymentOperator
                }).ToList();

                Dal.DbHelper.AllegroHelper.Payments.SetPayments(userId, p);


                return true;
            }

            private static Guid? GetPaymentId(PaymentOperation x)
            {
                if (x.payment != null)
                    return x.payment.id;
                else
                    return null;
            }

            private static string GetLogin(PaymentOperation x)
            {
                if (x.participant != null)
                    return x.participant.login;
                else
                    return null;
            }
            private static long? GetId(PaymentOperation x)
            {
                if (x.participant != null)
                    return x.participant.id;
                else
                    return null;
            }
        }
    }
}