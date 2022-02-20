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
        public class Disputes
        {
            public class Author
            {
                public string login { get; set; }
                public string role { get; set; }
            }


            public class Message
            {
                public string id { get; set; }
                public string text { get; set; }
                public Author author { get; set; }
                public DateTime createdAt { get; set; }
                public Attachment attachment { get; set; }
            }

            public class RootMessage
            {
                public List<Message> messages { get; set; }
            }




            public class Subject
            {
                public string name { get; set; }

            }
            public class Buyer
            {
                public string id { get; set; }
                public string login { get; set; }

            }
            public class CheckoutForm
            {
                public string id { get; set; }
                public DateTime createdAt { get; set; }

            }
            public class Attachment
            {
                public string fileName { get; set; }
                public string url { get; set; }

            }
       
            public class Dispute
            {
                public string id { get; set; }
                public Subject subject { get; set; }
                public string status { get; set; }
                public Buyer buyer { get; set; }
                public CheckoutForm checkoutForm { get; set; }
                public Message message { get; set; }
                public int messagesCount { get; set; }

            }
            public class Application
            {
                public IList<Dispute> disputes { get; set; }

            }

            public class DisputeMessage
            {
                public string text { get; set; }
                public string type { get; set; }

            }
            public static void GetDisputes()
            {
                Dal.AllegroScan asc = new Dal.AllegroScan();
                List<Dal.AllegroUser> users = asc.GetAllegroMyUsers();
                foreach (Dal.AllegroUser user in users)
                {
                    GetDisputes(user.UserId);
                }

                 SetDisputesReplies(); 

            }
            public static void SetDisputesReplies()
            {
                Dal.AllegroRestHelper arh = new Dal.AllegroRestHelper();
                List<Dal.AllegroOrderDisputeReply> disputes = arh.GetAllegroDisputesToReply();

                Dal.SettingsHelper sh = new Dal.SettingsHelper();
                Dal.Settings s = sh.GetSetting("ALL_DISP");


                foreach (Dal.AllegroOrderDisputeReply dispute in disputes)
                {
                    SetDispute(dispute, s.StringValue);
                    arh.SetAllegroDisputeDone(dispute.DisputeId);
                }


            }


            private static void SetDispute(AllegroOrderDisputeReply dispute, string msg)
            {
                string jsonEncodedParams = "";
                try
                {


                    HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/disputes/{0}/messages", dispute.DisputeId),
                        "POST", null, dispute.SellerId);

                    DisputeMessage d = new DisputeMessage()
                    {
                        text = msg,
                        type = "REGULAR"
                    };



                    Stream dataStream = request.GetRequestStream();
                    jsonEncodedParams = Bll.RESTHelper.ToJson(d);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();


                    WebResponse webResponse = request.GetResponse();

                    Stream responseStream = webResponse.GetResponseStream();

                    StreamReader reader = new StreamReader(responseStream);

                    string text = reader.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd wysyłania dyskusji dla konta {1} {0} <br><br>{2}",
                        dispute.DisputeId, dispute.SellerId, jsonEncodedParams));

                }
            }
            private static void GetDisputeReply(long userId, Guid disputeId)
            {
                GetDisputeReply(0, userId, disputeId);

            }

            private static void GetDisputeReply(int page, long userId, Guid disputeId)
            {
                int limit = 100;
                try
                {

                    HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/disputes/{2}/messages?limit={0}&offset={1}",
                        limit, limit * page, disputeId), "GET", null, userId);


                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    RootMessage item = json_serializer.Deserialize<RootMessage>(text);

                    SetDisputeMessages(item, disputeId);
                    if (item.messages.Count() == limit)
                        GetDisputeReply(page + 1, userId, disputeId);

                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania dyskusji dla konta {0}", userId));

                }
            }

            private static void SetDisputeMessages(RootMessage item, Guid disputeId)
            {
                List<Dal.AllegroOrderDisputeMessages> messages = item.messages.Select(x => new AllegroOrderDisputeMessages()
                {
                    DisputeId = disputeId,
                    MessageId = Guid.Parse(x.id),
                    Attachement = x.attachment==null?null: x.attachment.url,
                    InsertDate = x.createdAt,
                    Msg = x.text??"",
                    UserRole = x.author.role

                }).ToList();
                Dal.AllegroRestHelper oh = new Dal.AllegroRestHelper();

                oh.SetAllegroDisputeReply(disputeId, messages);
            }

            private static void GetDisputes(long userId)
            {
                GetDisputes(0, userId);

            }

            private static void GetDisputes(int page, long userId)
            {
                int limit = 10;
                try
                {

                    HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/disputes?limit={0}&offset={1}", limit, limit * page), "GET", null, userId);


                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    Application item = json_serializer.Deserialize<Application>(text);

                    SetDisputes(userId, item);
                    if (item.disputes.Count() == limit)
                        GetDisputes(page + 1, userId);

                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania dyskusji dla konta {0}", userId));

                }
            }

            private static void SetDisputes(long userId, Application item)
            {
                List<Dal.AllegroOrderDispute> disputes = new List<Dal.AllegroOrderDispute>();

                disputes.AddRange(
                    item.disputes.Select(x => new Dal.AllegroOrderDispute()
                    {
                        CheckoutFormId = Guid.Parse(x.checkoutForm.id),
                        CreatedAt = x.message.createdAt,
                        DisputeId = Guid.Parse(x.id),
                        IsReplied = false,
                        DisputeStatus = x.status
                    }
                    ).ToList());

                Dal.AllegroRestHelper oh = new Dal.AllegroRestHelper();

                oh.SetAllegroDisputes(disputes);


                foreach (Dispute dispute in item.disputes)
                {
                    GetDisputeReply(0, userId, Guid.Parse(dispute.id));

                }
            }
        }
    }
}