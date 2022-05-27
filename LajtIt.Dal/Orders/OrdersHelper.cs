using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
    public partial class Orders
    {
        public class OrdersIncomeSearch
        {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public decimal? MarzaFrom { get; set; }
            public decimal? MarzaTo { get; set; }
            public decimal? NarzutFrom { get; set; }
            public decimal? NarzutTo { get; set; }
            public int[] ShopIds { get; set; }
            public int[] SupplierIds { get; set; }
            public bool? IsReady { get; set; }

        }

        public static string GetOrdersStatusNoteComment(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                OrderStatusHistory p = ctx.OrderStatusHistory.Where(x => x.OrderId == orderId && x.OrderStatusId == 8 && x.Comment.Contains("Zamówienie zostało przyjęte do realizacji i jest kompletowane")).FirstOrDefault();

                return p.Comment;
            }
        }

        public static List<OrdersIncomeFnResult> GetOrdersIncomeSearch(OrdersIncomeSearch pcs)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var p = ctx.OrdersIncomeFn(pcs.DateFrom, pcs.DateTo.AddDays(1));//.Where(x => x.InsertDate >= pcs.DateFrom && x.InsertDate <= pcs.DateTo.AddDays(1));
                 
                if (pcs.ShopIds.Length > 0)
                    p = p.Where(x => pcs.ShopIds.Contains(x.ShopId));
                if (pcs.MarzaFrom.HasValue)
                    p = p.Where(x => x.Marza >= pcs.MarzaFrom.Value);
                if (pcs.MarzaTo.HasValue)
                    p = p.Where(x => x.Marza <= pcs.MarzaTo.Value);
                if (pcs.NarzutFrom.HasValue)
                    p = p.Where(x => x.Narzut >= pcs.NarzutFrom.Value);
                if (pcs.NarzutTo.HasValue)
                    p = p.Where(x => x.Narzut <= pcs.NarzutTo.Value);
                if (pcs.IsReady.HasValue)
                    p = p.Where(x => x.IsReady.Value== pcs.IsReady.Value);

                return p.ToList();
            }
        }
        public static List<ProductsIncomeFnResult> GetProductsIncomeSearch(OrdersIncomeSearch pcs)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var p = ctx.ProductsIncomeFn(pcs.DateFrom, pcs.DateTo.AddDays(1));//.Where(x => x.InsertDate >= pcs.DateFrom && x.InsertDate <= pcs.DateTo.AddDays(1));
                 
                if (pcs.SupplierIds.Length > 0)
                    p = p.Where(x => pcs.SupplierIds.Contains(x.SupplierId));
                if (pcs.MarzaFrom.HasValue)
                    p = p.Where(x => x.Marza >= pcs.MarzaFrom.Value);
                if (pcs.MarzaTo.HasValue)
                    p = p.Where(x => x.Marza <= pcs.MarzaTo.Value);
                if (pcs.NarzutFrom.HasValue)
                    p = p.Where(x => x.Narzut >= pcs.NarzutFrom.Value);
                if (pcs.NarzutTo.HasValue)
                    p = p.Where(x => x.Narzut <= pcs.NarzutTo.Value);
             

                return p.ToList();
            }
        }
        public static List<OrderShipping> GetOrderShippingByTrackingNumber(string trackingNumber)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderShipping.Where(x => x.ShipmentTrackingNumber == trackingNumber).ToList();
            }
        }
        public static void SetOrderShippingInternalId(int id, string internalId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                OrderShipping osToUpdate = ctx.OrderShipping.Where(x => x.Id == id ).FirstOrDefault();

                osToUpdate.InternalId = internalId;

                ctx.SubmitChanges();
            }
        }
        public static void SetOrderShippingTrackingNumber(int id, string shipmentTrackingNumber)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                OrderShipping os = ctx.OrderShipping.Where(x => x.Id == id && x.OrderShippingStatusId == (int)Dal.Helper.OrderShippingStatus.ReadyToCreate && x.ShipmentTrackingNumber == null).FirstOrDefault();

                os.OrderShippingStatusId = (int)Dal.Helper.OrderShippingStatus.Generated;
                os.ShipmentTrackingNumber = shipmentTrackingNumber;
                OrderStatusHistory ash = new OrderStatusHistory()
                {
                    Comment = String.Format("Pobrano numer przesyłki: {0}", shipmentTrackingNumber),
                    InsertDate = DateTime.Now,
                    InsertUser = "system",
                    OrderId = os.OrderId,
                    OrderStatusId = (int)Helper.OrderStatus.Comment
                };
                ctx.OrderStatusHistory.InsertOnSubmit(ash);

                ctx.SubmitChanges();
            }
        }
        public static void SetOrderShippingParcelDelete(int orderShippingParcelId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                OrderShippingParcel p = ctx.OrderShippingParcel.Where(x => x.Id == orderShippingParcelId).FirstOrDefault();

                ctx.OrderShippingParcel.DeleteOnSubmit(p);

                ctx.SubmitChanges();
            }
        }
        public static List<Dal.OrderShippingParcel> GetOrderShippingParcels(int orderShippingId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderShippingParcel.Where(x => x.OrderShippingId == orderShippingId).ToList();
            }
        }
        public static Dal.OrderShippingParcel GetOrderShippingParcel(int parcelId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderShippingParcel>(x => x.OrderShipping);

                ctx.LoadOptions = dlo;
                return ctx.OrderShippingParcel.Where(x => x.Id == parcelId).FirstOrDefault();
            }
        }
        public static string  GetParcelSizeInpostCode(int? size)
        {
            switch(size)
            {
                case 0: return "niemożliwy";
                case 1: return "A";
                case 2: return "B";
                case 3: return "C";
            }
            return "";
        }
        public static List<OrderShippingView> GetOrderShippings(int orderId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrderShippingView.Where(x=>x.OrderId == orderId) .ToList();
            }
        }       
        public static int SetOrderShippingParcel(Dal.OrderShippingParcel parcel)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.OrderShippingParcel.InsertOnSubmit(parcel);
                ctx.SubmitChanges();

                return parcel.Id;
            }
        }
        public static void SetOrderShippingParcels(List<Dal.OrderShippingParcel> parcels)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderShipping>(x => x.Order1);

                ctx.LoadOptions = dlo;

                ctx.OrderShippingParcel.InsertAllOnSubmit(parcels);
                ctx.SubmitChanges();
 
            }
        }
        public static OrderShipping GetOrderShipping(int orderShippingId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
           
                    DataLoadOptions dlo = new DataLoadOptions();
                    dlo.LoadWith<OrderShipping>(x => x.ShippingCompany);

                    ctx.LoadOptions = dlo;
                    return ctx.OrderShipping.Where(x => x.Id == orderShippingId).FirstOrDefault();
            }
        }
        public static List<ShippingCompany> GetShippingCompanies()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShippingCompany.ToList();
            }
        }
        public static List<Dal.OrderShippingStatus> GetOrderShippingStatuses()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderShippingStatus.ToList();
            }
        }

        public static int SetOrderShipping(OrderShipping os, List<OrderShippingParcel> parcels)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                if (os.Id == 0)
                {
                    ctx.OrderShipping.InsertOnSubmit(os);

                    ctx.SubmitChanges();

                    foreach (Dal.OrderShippingParcel parcel in parcels)
                        parcel.OrderShipping = os;

                    ctx.OrderShippingParcel.InsertAllOnSubmit(parcels);
                    ctx.SubmitChanges();

                }
                else
                {
                    Dal.OrderShipping osToUpdate = ctx.OrderShipping.Where(x => x.Id == os.Id).FirstOrDefault();

                    osToUpdate.OrderShippingStatusId = os.OrderShippingStatusId;
                    osToUpdate.SendFromExternalWerehouse = os.SendFromExternalWerehouse;
                    osToUpdate.ServicePoint = os.ServicePoint;
                    osToUpdate.ShipmentTrackingNumber = os.ShipmentTrackingNumber;
                    osToUpdate.ShippingCompanyId = os.ShippingCompanyId;
                    osToUpdate.ShippingServiceModeId = os.ShippingServiceModeId;
                    osToUpdate.ShippingServiceTypeId = os.ShippingServiceTypeId;
                    osToUpdate.COD = os.COD;

                    ctx.OrderShippingParcel.DeleteAllOnSubmit(ctx.OrderShippingParcel.Where(x => x.OrderShippingId == os.Id).ToList());
                    ctx.SubmitChanges();
                    ctx.OrderShippingParcel.InsertAllOnSubmit(parcels);
           
                    ctx.SubmitChanges();
                }
                return os.Id;
            }
        }

        public static void SetOrderTrackingNumberSent(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var o = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
                Dal.OrderShipping os = ctx.OrderShipping.Where(x => x.Id == o.OrderShippingId).FirstOrDefault();
                if (os != null)
                {
                    os.TrackingNumberSent = true;
                    ctx.SubmitChanges();
                }
            }
        }
        public static List<ProductCatalogStatusView> GetProductCatalogStatus(int productCatalogId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogStatusView.Where(x=>x.ProductCatalogId == productCatalogId).ToList();
            }
        }
        public static List<Dal.OrderCostFnResult> GetOrderCost(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderCostFn(orderId).ToList();
            }
        }
        //public static void SetOrderPaymentsForDpdPayment(int dpdPaymentId, int orderPaymentId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        Dal.DpdPayment dpd = ctx.DpdPayment.Where(x => x.Id == dpdPaymentId).FirstOrDefault();
        //        dpd.OrderPaymentId = orderPaymentId;
        //        ctx.SubmitChanges();
        //    }
        //}
        public static int[] GetOrderComplaintsWithProduct(int[] selectedOrderIds, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] orderIds = ctx.OrderProduct.Where(x => x.ProductCatalogId == productCatalogId && selectedOrderIds.Contains(x.OrderId)).Select(x => x.OrderId).ToArray();

                return ctx.OrderComplaint.Where(x => x.OrderId.HasValue && orderIds.Contains(x.OrderId.Value)).Select(x => x.Id).ToArray();

            }
        }
        public static void SetOrderClear(int orderId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                Dal.Order order = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
                List<Dal.OrderProduct> products = ctx.OrderProduct.Where(x => x.OrderId == orderId).ToList();
                List<Dal.TableLog> log = new List<TableLog>();
                foreach (Dal.OrderProduct product in products)
                { 
                    product.Quantity = 0;
                    log.Add(new TableLog()
                    {
                        Comment = String.Format("Zeruję produkt {0} z powodu reklamacji", product.ProductName),
                        ObjectId = product.OrderProductId,
                        InsertDate = DateTime.Now,
                        InsertUser = userName,
                        TableName = "OrderProduct",
                        ColumnName="Quantity",
                        IntValue=0
                    });
                }
                order.ShippingCost = 0;
                log.Add(new TableLog()
                {
                    Comment = String.Format("Zeruję wartość przesyłki z powodu reklamacji"),
                    ObjectId = orderId,
                    InsertDate = DateTime.Now,
                    InsertUser = userName,
                    TableName = "Order",
                    ColumnName = "ShippingCost",
                    IntValue = 0
                });
                ctx.TableLog.InsertAllOnSubmit(log);
                ctx.SubmitChanges();

            }
        }
        public static int[] GetOrderComplaintsWithSupplier(int[] selectedOrderIds, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] orderIds = ctx.OrderProduct.Where(x => x.ProductCatalog.SupplierId == supplierId && selectedOrderIds.Contains(x.OrderId)).Select(x => x.OrderId).ToArray();

                return ctx.OrderComplaint.Where(x => x.OrderId.HasValue && orderIds.Contains(x.OrderId.Value)).Select(x => x.Id).ToArray();

            }
        }
        //public static DpdPayment GetDpdPayment(int dpdPaymentId)
        //{

        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.DpdPayment.Where(x => x.Id == dpdPaymentId).FirstOrDefault();
        //    }
        //}
     
        public static Order GetOrder(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Order>(x => x.OrderStatus);
                dlo.LoadWith<Order>(x => x.OrderShipping);
                dlo.LoadWith<OrderShipping>(x => x.ShippingServiceMode);
                dlo.LoadWith<OrderShipping>(x => x.ShippingCompany);
                dlo.LoadWith<Order>(x => x.Invoice);
                dlo.LoadWith<Order>(x => x.Shop);
                dlo.LoadWith<Order>(x => x.Company);
                dlo.LoadWith<Invoice>(x => x.Company); 

                ctx.LoadOptions = dlo;

                return ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
            }
        }
        public static bool SetOrderCompany(int orderId, int companyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Order order = ctx.Order.Where(x => x.OrderId == orderId && x.InvoiceId == null).FirstOrDefault();

                if (order == null)
                    return false;

                order.CompanyId = companyId;

                ctx.SubmitChanges();
                return true;
            }
        }

        public static List<OrderProduct> GetOrderProducts(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderProduct>(x => x.ProductCatalog);
                dlo.LoadWith<Dal.ProductCatalog>(x => x.Supplier); 

                ctx.LoadOptions = dlo;
                return ctx.OrderProduct.Where(x => x.OrderId == orderId).ToList();
            }
        }

        public static OrderReceipt GetReceipt(int receiptId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderReceipt.Where(x => x.ReceiptId == receiptId).FirstOrDefault();
            }
        }
        //public static List<DpdPaymentsView> GetDpdPayments(DateTime date, int v)
        //{
        //    using (LajtitViewsDB ctx = new LajtitViewsDB())
        //    {

        //        return ctx.DpdPaymentsView.Where(x =>  
        //         x.PaymentDate.Year == date.Year
        //        && x.PaymentDate.Month == date.Month)
        //            .OrderBy(x => x.PaymentDate)
        //            .ToList();

        //    }
        //}
        public static List<OrderPayment> GetOrderPayments(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderPayment>(x => x.OrderPaymentType);
                dlo.LoadWith<OrderPayment>(x => x.OrderPaymentAccountingType);
                ctx.LoadOptions = dlo;

                return ctx.OrderPayment.Where(x => x.OrderId == orderId).ToList();

            }
        }
        public static List<OrderPaymentsView> GetOrderPayments(DateTime date, int companyId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {

                return ctx.OrderPaymentsView.Where(x => x.CompanyId == companyId
                && x.PYear == date.Year
                && x.PMonth == date.Month)
                    .OrderBy(x => x.InsertDate)
                    .ToList();

            }
        }
        public static List<OrderPaymentType> GetOrderPaymentTypes(DateTime date, int companyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                return ctx.OrderPayment.Where(x => x.OrderPaymentType.CompanyId == companyId
                && x.PYear == date.Year
                && x.PMonth == date.Month)
                    .Select(x=>x.OrderPaymentType)
                    .Distinct()
                    .ToList();

            }
        }
        public static List<OrderPaymentsView> GetOrderPayments(DateTime date, int companyId, 
            bool notAssigned, bool receiptReady, int[] accoutingTypeIds, int[] paymentTypeIds)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {

                var r = ctx.OrderPaymentsView.Where(x => x.CompanyId == companyId
                && x.PYear == date.Year
                && x.PMonth == date.Month);


                if (notAssigned)
                    r = r.Where(x => x.AccountingTypeId == null);

                if (receiptReady)
                {
                    r = r.Where(x => x.ReceiptReadyToCreate == 1);
                }
                if (accoutingTypeIds.Count() > 0)
                    r = r.Where(x => x.AccountingTypeId.HasValue &&
                        accoutingTypeIds.Contains(x.AccountingTypeId.Value));

                if (paymentTypeIds.Count() > 0)
                    r = r.Where(x => paymentTypeIds.Contains(x.PaymentTypeId));



                return r.OrderBy(x => x.InsertDate)
                    .ToList();

            }
        }

    }
}
 