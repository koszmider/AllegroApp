using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class AllegroRestHelper
    {
        public void SetAllegroOrders(List<AllegroOrderLineItem> items)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                  ctx.AllegroOrderLineItem.InsertAllOnSubmit(items);
                ctx.SubmitChanges();
            }
        }

        public void SetAllegroUsers(List<AllegroUser> users)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                long[] usersIds = users.Select(x => x.UserId).Distinct().ToArray();
                long[] existingUserIds = ctx.AllegroUser.Where(x => usersIds.Contains(x.UserId)).Select(x => x.UserId).ToArray();


                long[] userIdsToAdd = usersIds.Where(x => !existingUserIds.Contains(x)).ToArray();

                foreach(int userId in userIdsToAdd)
                {
                    if(ctx.AllegroUser.Where(x=>x.UserId==userId).FirstOrDefault()==null)
                    {
                        AllegroUser au = new AllegroUser()
                        {
                            UserId = userId,
                            UserName = users.Where(x=>x.UserId == userId).Select(x=> x.UserName).FirstOrDefault(),
                            Follow = false
                        };
                        ctx.AllegroUser.InsertOnSubmit(au);
                        ctx.SubmitChanges();
                    }
                }
                
            }
        }

        public static AllegroBadge GetAllegroBadge(string id)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroBadge.Where(x => x.BadgeId == id).FirstOrDefault();
            }
        }

        public void SetAllegroBillingTypes(List<AllegroBillingTypes> billingTypes)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                ctx.AllegroBillingTypes.DeleteAllOnSubmit(ctx.AllegroBillingTypes);
                ctx.AllegroBillingTypes.InsertAllOnSubmit(billingTypes);
                ctx.SubmitChanges();
            }
        }

        public long? GetAllegroOrderLastId(long userId)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroOrderLastId.Where(x => x.UserId == userId).Select(x => x.Id).FirstOrDefault();
            }
        }

        public List<AllegroOrder> GetAllegroOrdersToProcess()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroOrder.Where(x => x.IsProcessed == false).OrderBy(x => x.OccuredAt).ToList();
            }
        }

        public Order GetOrderByCheckoutFormId(string checkoutFormId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] availableStatusIds = new int[]
                {
                    (int)Dal.Helper.OrderStatus.New,
                    (int)Dal.Helper.OrderStatus.WaitingForDelivery,
                    (int)Dal.Helper.OrderStatus.WaitingForPayment,
                    (int)Dal.Helper.OrderStatus.WaitingForClient,

                };
                Dal.Order order= ctx.Order.Where(x => availableStatusIds.Contains(x.OrderStatusId) &&
                x.ExternalOrderNumber == checkoutFormId).FirstOrDefault();

                if (order == null)
                    return null;
                else
                    return order;
            }
        }
        public static List<AllegroItemBadgeView> GetAllegroItemBadgesToCheck()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                string[] statuses = new string[] { "IN_VERIFICATION", "REQUESTED", "WAITING_FOR_PUBLICATION", "ACTIVE" };
                return ctx.AllegroItemBadgeView.Where(x => statuses.Contains(x.RequestStatus )).ToList();
            }
        }
        public static List<AllegroItemBadgeView> GetAllegroItemBadgesToCreate()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroItemBadgeView.Where(x => x.RequestStatus == "").ToList();
            }
        }
        public void SetAllegroBadge(AllegroBadge ab)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                Dal.AllegroBadge abExisting = ctx.AllegroBadge.Where(x => x.BadgeId == ab.BadgeId).FirstOrDefault();

                if (abExisting == null)
                    ctx.AllegroBadge.InsertOnSubmit(ab);
                else
                {
                    abExisting.BadgeName = ab.BadgeName;
                    abExisting.IsActive = ab.IsActive;
                    abExisting.RebateFrom = ab.RebateFrom;
                    abExisting.RebateTo = ab.RebateTo;
                    abExisting.SupplierIds = ab.SupplierIds;
                }
                ctx.SubmitChanges();
            }
            }

        public void SetAllegroDisputeDone(Guid disputeId)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                var d = ctx.AllegroOrderDispute.Where(x => x.DisputeId == disputeId).FirstOrDefault();
                d.IsReplied = true;
                ctx.SubmitChanges();
            }
        }

        public List<AllegroOrderDisputeReply> GetAllegroDisputesToReply()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroOrderDisputeReply.ToList();
            }
        }

        public void SetAllegroBilling(List<AllegroBilling> billing)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                Guid[] ids = billing.Select(x => x.Id).ToArray();

                Guid[] existingIds = ctx.AllegroBilling.Where(x => ids.Contains(x.Id)).Select(x => x.Id).ToArray();

                ctx.AllegroBilling.InsertAllOnSubmit(billing.Where(x => !existingIds.Contains(x.Id)));
                ctx.SubmitChanges();
            }
        }

        public void SetTags(List<Dal.AllegroTags> allegroTags )
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                Guid[] tags = ctx.AllegroTags.Select(x => x.TagId).ToArray();

                ctx.AllegroTags.InsertAllOnSubmit(allegroTags.Where(x=>!tags.Contains(x.TagId)));
                ctx.SubmitChanges();
            }
        }

        public void SetOrder(OrderStatusHistory osh)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.OrderStatusHistory.InsertOnSubmit(osh);
                ctx.SubmitChanges();
            }
        }

        public void SetAllegroOrderProcessed(long id)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                var ao = ctx.AllegroOrder.Where(x => x.Id == id).FirstOrDefault();
                ao.IsProcessed = true;
                ctx.SubmitChanges();
            }
        }

        public AllegroOrderLineItem GetAllegroOrderLineItem(long id)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroOrderLineItem.Where(x => x.AllegroOrderId == id).FirstOrDefault();
            }
        }

        public void SetAllegroDisputeReply(Guid disputeId, List<AllegroOrderDisputeMessages> messages)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                Guid[] msgIds = messages.Select(x => x.MessageId).ToArray();

                Guid[] existingMsgIds = ctx.AllegroOrderDisputeMessages.Where(x => x.DisputeId == disputeId
                && msgIds.Contains(x.MessageId))
                    .Select(x => x.MessageId)
                    .ToArray();


                ctx.AllegroOrderDisputeMessages.InsertAllOnSubmit(messages.Where(x => !existingMsgIds.Contains(x.MessageId)).ToList());

                ctx.SubmitChanges();
            }
        }

        public void SetOrderProduct(OrderProduct op)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.OrderProduct.InsertOnSubmit(op);
                ctx.SubmitChanges();
            }
        }

        public void SetOrder(Order order, Invoice invoice, List<Dal.OrderPayment> ops, List<Dal.OrderProduct> products, Dal.OrderShipping orderShipping, Dal.OrderStatusHistory osh)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Order orderToUpdate = ctx.Order.Where(x => x.OrderId == order.OrderId).FirstOrDefault();

       
                orderToUpdate.Phone = order.Phone;
                orderToUpdate.ShipmentFirstName = order.ShipmentFirstName;
                orderToUpdate.ShipmentLastName = order.ShipmentLastName;
                orderToUpdate.ShipmentCity = order.ShipmentCity;
                orderToUpdate.ShipmentPostcode = order.ShipmentPostcode;
                orderToUpdate.ShipmentAddress = order.ShipmentAddress;
                orderToUpdate.ShipmentCompanyName = order.ShipmentCompanyName;
                orderToUpdate.Phone = order.Phone;
                orderToUpdate.ShippingCost = order.ShippingCost;
                //orderToUpdate.ShippingData = order.ShippingData;
                //orderToUpdate.ShippintTypeId = order.ShippintTypeId;
                orderToUpdate.ShippingCurrencyRate = order.ShippingCurrencyRate;
                orderToUpdate.ShippingCurrencyCode = order.ShippingCurrencyCode;
                orderToUpdate.ShipmentCountryCode = order.ShipmentCountryCode;

                if (invoice != null)
                {
                    if (orderToUpdate.InvoiceId.HasValue == false)
                    {
                        //ctx.SubmitChanges();

                        ctx.Invoice.InsertOnSubmit(invoice);
                        //ctx.SubmitChanges();
                        orderToUpdate.Invoice = invoice;
                    }
                }

                ctx.SubmitChanges();

                if (invoice != null)
                {
                    if (orderToUpdate.InvoiceId.HasValue != false)
                    { 
                        Dal.Invoice invoiceToUpdate = ctx.Invoice.Where(x => x.InvoiceId == orderToUpdate.InvoiceId).FirstOrDefault();

                        invoiceToUpdate.Address = invoice.Address;
                        invoiceToUpdate.City = invoice.City;
                        invoiceToUpdate.CompanyId = invoice.CompanyId;
                        invoiceToUpdate.CompanyName = invoice.CompanyName;
                        invoiceToUpdate.Email = invoice.Email;
                        invoiceToUpdate.Nip = invoice.Nip;
                        invoiceToUpdate.Postcode = invoice.Postcode;
                    }
                }

                if (ops.Count > 0)
                    foreach (OrderPayment op in ops)
                        if (ctx.OrderPayment.Where(x => x.ExternalPaymentId == op.ExternalPaymentId).Count() == 0)
                            ctx.OrderPayment.InsertOnSubmit(op);

                if (products.Count > 0)
                {
                    foreach (OrderProduct product in products)
                        if (ctx.OrderProduct.Where(x => x.ExternalAllegroLinieItemId == product.ExternalAllegroLinieItemId).Count() == 0)
                            ctx.OrderProduct.InsertOnSubmit(product); 
                }
                if (osh != null)
                    ctx.OrderStatusHistory.InsertOnSubmit(osh);

                if (orderShipping != null)
                    if (orderShipping.ShippingServiceModeId == (int)Dal.Helper.ShippingServiceMode.Courier)
                    {
                        orderShipping.OrderShippingStatusId = (int)Dal.Helper.OrderShippingStatus.ReadyToCreate;

                        Dal.OrderShippingParcel parcel = new OrderShippingParcel()
                        {
                            OrderShipping = orderShipping,
                            Weight = Dal.Helper.DefaultParcelWeight
                        };
                        ctx.OrderShippingParcel.InsertOnSubmit(parcel);
                    }
                    else
                        ctx.OrderShipping.InsertOnSubmit(orderShipping);

                ctx.SubmitChanges();
            }
        }

        public static void SetAllegroBadgeUpdate2(AllegroItemBadgeView item)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                Dal.AllegroItemBadge aib = ctx.AllegroItemBadge
                    .Where(x => x.ItemId == item.ItemId && x.BadgeId == item.BadgeId)
                    .FirstOrDefault();

                if (aib != null)
                {
                    aib.RequestStatus = item.RequestStatus;
                    aib.RequestRejectReasons = item.RequestRejectReasons;

                    ctx.SubmitChanges();
                }
            }
        }

        public static List<AllegroBadge> GetAllegroBadges()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroBadge.Where(x => x.IsActive).ToList();
            }
            }

        public static void SetAllegroBadgeUpdate(AllegroItemBadgeView item)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                Dal.AllegroItemBadge aib = ctx.AllegroItemBadge.Where(x => x.Id == item.Id).FirstOrDefault();

                aib.BadgePrice = item.BadgePrice;
                aib.RequestStatus = item.RequestStatus;
                aib.RequestRejectReasons = item.RequestRejectReasons;
                aib.ApplicationId = item.ApplicationId;

                ctx.SubmitChanges();

            }
        }
        public void SetAllegroDisputes(List<AllegroOrderDispute> disputes)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                foreach (Dal.AllegroOrderDispute d in disputes)
                {
                    Dal.AllegroOrderDispute disputeToUpdate = ctx.AllegroOrderDispute.Where(x => x.DisputeId == d.DisputeId).FirstOrDefault();

                    if (disputeToUpdate == null)
                        ctx.AllegroOrderDispute.InsertOnSubmit(d);
                    else
                    {
                        disputeToUpdate.DisputeStatus = d.DisputeStatus;
                    }
                }
                ctx.SubmitChanges();
            }
        }

        //public int GetShippingTypeId(string id)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        if(id!=null)
        //        {
        //            return ctx.ShippingType.Where(x => x.AllegroDeliveryMethodId == Guid.Parse(id)).Select(x => x.ShippingTypeId).FirstOrDefault();
        //        }
        //        return 0;
        //    }
        //}

        public List<OrderTransactiongNumber> GetOrderTransactionNumber(Dal.Helper.ShopType shopType)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                int[] excludedTrackingNumbers = new int[]
                  {
                    //(int)Dal.Helper.OrderStatus.Cancelled,
                    (int)Dal.Helper.OrderStatus.New,
                    (int)Dal.Helper.OrderStatus.Temporary,
                    (int)Dal.Helper.OrderStatus.Deleted,
                    (int)Dal.Helper.OrderStatus.Complaint

                  };


                return ctx.OrderTransactiongNumber.Where(x => 
                    x.ShopTypeId == (int)shopType
                    && x.ShipmentTrackingNumber != null
                    && (x.TrackingNumberSent == false)
                    && !excludedTrackingNumbers.Contains(x.OrderStatusId)
                    && x.CheckoutFormId !=null)
                .ToList();
            }
        }

        public static void SetAllegroBadgeUpdate(List<AllegroItemBadge> badges)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                foreach (AllegroItemBadge badge in badges)
                {

                    Dal.AllegroItemBadge aib = ctx.AllegroItemBadge.Where(x => x.ItemId == badge.ItemId && x.BadgeId == badge.BadgeId).FirstOrDefault();
                    if (aib != null)
                    { 
                   // aib.ApplicationId = badge.ApplicationId;
                    aib.RequestStatus = badge.RequestStatus;
                    aib.RequestRejectReasons = badge.RequestRejectReasons;
                    //aib.LastUpdateDate = badge.LastUpdateDate;

                    ctx.SubmitChanges();
                    }
                }
            }
        }

        public long? GetSellerIdByCheckouFormId(Guid checkoutFormId)
        {

            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                var allegroOrder  = ctx.AllegroOrder.Where(x => x.CheckoutFormId == checkoutFormId).FirstOrDefault();

                if (allegroOrder == null)
                    return null;
                else
                    return allegroOrder.SellerId;
            }
        }

        public Guid[] GetAllegroOrderLineItems(string checkoutFormId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderProduct.Where(x => x.Order.ExternalOrderNumber == checkoutFormId && x.Order.Shop.ShopTypeId == (int)Dal.Helper.ShopType.Allegro && x.ExternalProductId != 0
                && x.ExternalAllegroLinieItemId.HasValue)
                    .Select(x => x.ExternalAllegroLinieItemId.Value)
                    .Distinct()
                    .ToArray();
            } 
        }



        public static void SetLog(string url, string method, long? itemId, long? userId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
               
                try
                {
                    ctx.AllegroRestLog.InsertOnSubmit(new AllegroRestLog()
                    {
                        InsertDate = DateTime.Now,
                        ItemId = itemId,
                        UserId = userId,
                        Method = method,
                        Url = url
                    });
                    ctx.SubmitChanges();
                }
                catch(Exception ex)
                { }
            }
        }
    }
}
