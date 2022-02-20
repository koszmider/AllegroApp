using System;
using System.Linq;
using System.Web.UI;
using LajtIt.Bll;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LajtIt.Dal;

namespace LajtIt.Web.Controls
{
    public partial class OrderComment : LajtitControl
    {
        private int OrderId
        {
            set { ViewState["OrderId"] = value; }
            get { return Convert.ToInt32(ViewState["OrderId"]); }
        }
        public delegate void SavedEventHandler(object sender, EventArgs e);
        public event SavedEventHandler Saved;
        public delegate void SaveOptionsEventHandler();
        public event SaveOptionsEventHandler SaveOptions;
        public delegate int[] SelectedProductsIdsEventHandler();
        public event SelectedProductsIdsEventHandler GetSelectedProducts;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region Complaints
        public void btnComplaintAllegroSend_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();

            //long itemId = Convert.ToInt64(txbCompaintAllegroId.Text.Trim());
            //int reasonId = Convert.ToInt32(ddlCompaintAllegroReason.SelectedValue);
            //int quantity = Convert.ToInt32(txbComplaintQuantity.Text);

            //Bll.AllegroHelper ah = new AllegroHelper();
            //try
            //{
            //    bool result = ah.SetRefund(itemId, OrderId, reasonId, quantity);

            //    if (result)
            //        DisplayMessage("Wniosek został pomyślnie wysłany");
            //}
            //catch (Exception ex)
            //{
            //    DisplayMessage(String.Format("Błąd podczas wysyłania wniosku:<br><br>{0}", ex.Message));
            //}
        }

        protected void txbCompaintAllegroId_OnTextChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //try
            //{
            //    long itemId = Convert.ToInt64(txbCompaintAllegroId.Text.Trim());

            //    Bll.AllegroHelper ah = new AllegroHelper();

            //    Bll.AllegroNewWCF.ReasonInfoType[] reasons = ah.GetRefundsReasons(itemId, OrderId);

            //    ddlCompaintAllegroReason.DataSource = reasons.Select(x => new
            //    {
            //        ReasonId = x.reasonId,
            //        Reason = String.Format("{0} ({1})", x.reasonName, x.maxQuantity)
            //    });
            //    ddlCompaintAllegroReason.DataBind();
            //}
            //catch (Exception ex)
            //{
            //    DisplayMessage(String.Format("Wystąpił błąd:<br><br>{0}<br>{1}", ex.Message, ex.StackTrace));
            //}
        }
        #endregion
        protected void lnbShowEmail_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.OrderStatus orderStatus = oh.GetOrderStatus(Convert.ToInt32(ddlStatus.SelectedValue));

            if (orderStatus.SendEmailTemplateId.HasValue)
            {

                Bll.EmailEditor ee = new Bll.EmailEditor();
                string body =  ee.GetStatusNotificationEmailBody(OrderId, orderStatus.SendEmailTemplateId.Value);
                DisplayMessage(body.Replace("'", "\"").Replace('\n', ' ').Replace('\r', ' '));
            }
        }
        public void BindStatuses(Dal.OrderStatus orderStatus)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            var statuses= oh.GetStatuses(OrderId);

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
            int[] acceptableStatuses = new int[] { (int)Dal.Helper.OrderStatus.WaitingForAcceptance,
                 (int)Dal.Helper.OrderStatus.Comment,
                  (int)Dal.Helper.OrderStatus.Deleted
            };

//            if (order.ShopId == (int)Dal.Helper.Shop.Empik && orderStatus.OrderStatusId == (int)Dal.Helper.OrderStatus.New)
//            {
//                statuses = statuses.Where(x => acceptableStatuses.Contains(x.OrderStatusId)).ToList();
//            }


            ddlStatus.DataSource = statuses;
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, "");
           // ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(orderStatus.OrderStatusId.ToString()));
            lblOrderStatus.Text = orderStatus.StatusName;
            ddlStatus_OnSelectedIndexChanged(null, null);


         

        }
        protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStatus.SelectedValue == "") return;


            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.OrderStatus orderStatus = oh.GetOrderStatus(Convert.ToInt32(ddlStatus.SelectedValue));

            pnlEmail.Visible = orderStatus.SendEmailTemplateId.HasValue;

            if (Dal.Helper.OrderStatus.Complaint == (Dal.Helper.OrderStatus)Convert.ToInt32(ddlStatus.SelectedValue))
            {
            }
            else
                pOrderComplaint.Visible = false;

            pOrderNotification.Visible = false;
            pEmpik.Visible = false;
            pEmpikDelete.Visible = false;

            switch ((Dal.Helper.OrderStatus)Convert.ToInt32(ddlStatus.SelectedValue))
            {
                case Dal.Helper.OrderStatus.WaitingForAcceptance:
                    BindEmpik();
                    break;
                case Dal.Helper.OrderStatus.Deleted:
                    BindEmpikDelete();
                    break;
                case Dal.Helper.OrderStatus.WaitingForPayment:
                    BindOrderNotifications();
                    break;
                case Dal.Helper.OrderStatus.ReadyToSend:  
                case Dal.Helper.OrderStatus.WaitingForClient:
                case Dal.Helper.OrderStatus.ReturnRecieved:
                case Dal.Helper.OrderStatus.ReturnPayment:
                case Dal.Helper.OrderStatus.CustomerNotification:
                case Dal.Helper.OrderStatus.WaitingForDelivery:
                     
                    BindOrderNotifications();


                    break;
                case Dal.Helper.OrderStatus.Complaint:
                    ddlOrderComplaintType.DataSource = oh.GetOrderComplaintTypes();
                    ddlOrderComplaintType.DataBind();
                    pOrderComplaint.Visible = true;
                    break;
                default:
                    pOrderComplaint.Visible = false;
                    pOrderNotification.Visible = false;
                    pEmpik.Visible = false;
                    pEmpikDelete.Visible = false;
                    //pnAccountNumber.Visible = false;
                    break;
            }

        }

        private void BindEmpik()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();


            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

            if (order.ShopId == (int)Dal.Helper.Shop.Empik)
            {
                pEmpik.Visible = true;
            }
        }
        private  void BindEmpikDelete()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();


            Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);

            if (order.ShopId == (int)Dal.Helper.Shop.Empik)
            {
                pEmpikDelete.Visible = true;
            }
        }



        private void BindOrderNotifications()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            pOrderNotification.Visible = true;
            chblOrderNotificationList.DataSource = oh.GetOrderNotificationTypes(Convert.ToInt32(ddlStatus.SelectedValue));
            chblOrderNotificationList.DataBind();
        }
        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {

            if (SaveOptions != null)
                SaveOptions();

            Dal.OrderHelper oh = new Dal.OrderHelper();
            if (Dal.Helper.OrderStatus.ReadyToSend == (Dal.Helper.OrderStatus)Convert.ToInt32(ddlStatus.SelectedValue))
            {
                int companyId = (int)Dal.Helper.DefaultCompanyId;
                oh.SetOrderCompanyId(OrderId, companyId);
            }
            if (!ValidateStatus())
                return;        

            Dal.OrderStatusHistory osh = new Dal.OrderStatusHistory()
            {
                Comment = txbComment.Text.Trim(),
                InsertDate = DateTime.Now,
                InsertUser = UserName,
                OrderStatusId = Convert.ToInt32(ddlStatus.SelectedValue),
                OrderId = OrderId,
                SendNotification = chbSendEmail.Checked
            };
          

            #region 
            if (pOrderNotification.Visible)
            {

                List<Dal.OrderNotification> notifications = new List<Dal.OrderNotification>();

                if (txbOrderNotificationComment.Text.Trim() != "")
                {
                    notifications.Add(
                                        new Dal.OrderNotification
                                        {
                                            Comment = txbOrderNotificationComment.Text.Trim(),
                                            InsertDate = DateTime.Now,
                                            OrderId = OrderId,
                                            IsSent = false,
                                            NotificationTypeId = 0
                                        }
                                     );
                }


                notifications.AddRange(
                chblOrderNotificationList.Items.Cast<ListItem>().Where(x=>x.Selected).Select(x =>
                    new Dal.OrderNotification
                    {
                        Comment = "",
                        InsertDate = DateTime.Now,
                        OrderId = OrderId,
                        IsSent = false,
                        NotificationTypeId = Convert.ToInt32(x.Value),


                    }

                    ).ToList()
                 );

                //if (notifications.Count > 0)
                    oh.SetOrderNotifications(OrderId, notifications);
                //else
                //    chbSendEmail.Checked = false;


            }
            #endregion

            SendEmailNotification(pnlEmail.Visible && chbSendEmail.Checked, 
                Convert.ToInt32(ddlStatus.SelectedValue),
                OrderId,
                UserName);
              
            Dal.ComplaintStatusHistory csh = null;
            if (Dal.Helper.OrderStatus.Complaint == (Dal.Helper.OrderStatus)Convert.ToInt32(ddlStatus.SelectedValue))
            {

                Dal.OrderComplaint oc = oc = new Dal.OrderComplaint()
                {
                    Comment = txbComment.Text.Trim(),
                    InsertDate = DateTime.Now,
                    InsertUser = UserName,
                    OrderComplaintTypeId = Convert.ToInt32(ddlOrderComplaintType.SelectedValue),
                    OrderId = OrderId, 
                    ComplaintPerson = null,
                    Cost = 0,
                    LastUpdateDate=DateTime.Now
                };
                if (chbCloseComplaint.Checked)
                    oc.ComplaintStatusId = (int)Dal.Helper.ComplaintStatus.Closed;
                else
                    oc.ComplaintStatusId = (int)Dal.Helper.ComplaintStatus.New;
                csh = new Dal.ComplaintStatusHistory()
                {
                    Comment = txbComment.Text.Trim(),
                    ComplaintStatusId = 1,
                    InsertDate = DateTime.Now,
                    InsertUser = UserName,
                    OrderComplaint = oc
                };

                if (chbClearOrder.Checked)
                    Dal.DbHelper.Orders.SetOrderClear(OrderId, UserName);
            }
            oh.SetOrderStatus(osh, csh);

            if (Dal.Helper.OrderStatus.Sent == (Dal.Helper.OrderStatus)Convert.ToInt32(ddlStatus.SelectedValue))
            {
                Bll.InvoiceHelper ih = new InvoiceHelper();

                Dal.Order order = Dal.DbHelper.Orders.GetOrder(OrderId);
                Dal.Invoice invoice = order.Invoice;

                if (invoice != null)
                {
                    if (invoice.InvoiceNumber == null)
                        ih.CreateInvoiceNumber(OrderId, false);
                }
            }

            pOrderNotification.Visible = false;
            pnlEmail.Visible = false;
            //pnAccountNumber.Visible = false;
            if (Saved != null)
                Saved(null, null);
        }

        public static void SendEmailNotification(bool sendEmail, int orderStatusId, int orderId, string actingUser)
        {
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            if (sendEmail && order.ShipmentCountryCode == "PL")
            {
                Dal.OrderHelper oh = new Dal.OrderHelper();
                Dal.OrderStatus orderStatus = oh.GetOrderStatus(orderStatusId);
 

                if (orderStatus.SendEmailTemplateId.HasValue)
                {
                    Bll.EmailEditor ee = new Bll.EmailEditor();
                    ee.SendOrderStatusNotification(orderId, orderStatus, actingUser);
                }

            }
        }

      

        private bool ValidateStatus()
        {
            Dal.Helper.OrderStatus orderStatus = (Dal.Helper.OrderStatus)Convert.ToInt32(ddlStatus.SelectedValue);
            bool result = false;
            Bll.OrderHelper oh = new Bll.OrderHelper();
            string msg = "";

            switch (orderStatus)
            {
                case Dal.Helper.OrderStatus.WaitingForAcceptance:
                    int[] ids = GetSelectedProducts();
                    if (Bll.OrderHelper.ValidateStatusWaitingForAcceptance(OrderId, ids, ref msg))
                    {
                        result = SetExtraActions(OrderId, ids, ref msg);
                    }
                    break;
                case Dal.Helper.OrderStatus.ReadyToSend:
                    result = Bll.OrderHelper.ValidateStatusReadyToSend(OrderId, false, ref msg); break;
                case Dal.Helper.OrderStatus.WaitingForDelivery:
                    result = Bll.OrderHelper.ValidateStatusWaitingForDelivery(OrderId, false, ref msg); break;
                case Dal.Helper.OrderStatus.WaitingForPayment:
                    result = Bll.OrderHelper.ValidateStatusWaitingForPayment(OrderId,   ref msg); 

                    //if (result)
                    //    SetExtraActions(OrderId, ids, ref msg);
                    break;
                case Dal.Helper.OrderStatus.Sent:
                    result = oh.ValidateStatusSent(OrderId, ref msg); break;
                default: result = true; break;
            }

            Dal.OrderStatus newOrderStatus = oh.GetOrderStatus((int)orderStatus);

            if (newOrderStatus.CommentRequired && txbComment.Text.Trim() == "")
            {
                msg += "Komentarz do zmiany statusu jest wymagany<br>";
                result = false;
            }
            if (!result)
            {
                msg = String.Format("Przy próbie zmiany statusu, wykryto następujące błędy walidacji<br><br>{0}", msg);
                DisplayMessage(msg);
    
            }
            return result;

        }

        private bool SetExtraActions(int orderId, int[] ids, ref string msg)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);
             
            bool result = true; 
            if (order.ShopId == (int)Dal.Helper.Shop.Empik )
            {
                result =  Bll.EmpikRESTHelper.Orders.SetOrderLineAccept(orderId, ids);
                if (!result)
                    msg += "- Akceptacja zamówienia w Empik zakończona błędem<br>";
                else
                {
                    oh.SetOrderProductsDelete(orderId, ids);
                }
            }


            return result;
        }

        public void BindStatusHistory(int orderId)
        {
            OrderId = orderId;
            Dal.OrderHelper oh = new Dal.OrderHelper();
            gvOrderStatusHistory.DataSource = oh.GetOrderStatusHistory(orderId)
                .Select(x => new
                {
                    InsertDate = x.InsertDate,
                    InsertUser = x.InsertUser,
                    Status = x.OrderStatus.StatusName,
                    Comment = GetComment( x.Comment)

                })
                .OrderByDescending(x => x.InsertDate)
                .ToList();
            gvOrderStatusHistory.DataBind();
        }

        private string GetComment(string comment)
        {
            if (comment == null)
                return "";
            else
                return comment.Replace("\n", "<br>");
        }
    }
}