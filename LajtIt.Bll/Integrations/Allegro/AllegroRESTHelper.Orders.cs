

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public partial class AllegroRESTHelper
    {
        public class Orders
        {
            #region
            public class Events
            {
                public class Seller
                {
                    public string id { get; set; }
                }

                public class Buyer
                {
                    public string id { get; set; }
                    public string email { get; set; }
                    public bool guest { get; set; }
                    public string login { get; set; }
                }

                public class External
                {
                    public string id { get; set; }
                }

                public class Offer
                {
                    public string id { get; set; }
                    public string name { get; set; }
                    public External external { get; set; }
                }

                public class Price
                {
                    public string amount { get; set; }
                    public string currency { get; set; }
                }

                public class OriginalPrice
                {
                    public string amount { get; set; }
                    public string currency { get; set; }
                }

                public class LineItem
                {
                    public Offer offer { get; set; }
                    public Price price { get; set; }
                    public string id { get; set; }
                    public int quantity { get; set; }
                    public OriginalPrice originalPrice { get; set; }
                    public DateTime boughtAt { get; set; }
                }

                public class CheckoutForm
                {
                    public string id { get; set; }
                }

                public class Order
                {
                    public Seller seller { get; set; }
                    public Buyer buyer { get; set; }
                    public List<LineItem> lineItems { get; set; }
                    public CheckoutForm checkoutForm { get; set; }
                }

                public class Event
                {
                    public string id { get; set; }
                    public Order order { get; set; }
                    public string type { get; set; }
                    public DateTime occurredAt { get; set; }
                }

                public class RootObject
                {
                    public List<Event> events { get; set; }
                }
            }
            public class EventLast
            {
                public class LatestEvent
                {
                    public string id { get; set; }
                    public DateTime occurredAt { get; set; }
                }

                public class RootObject
                {
                    public LatestEvent latestEvent { get; set; }
                }

            }
             
            public class CheckoutForm
            {
                public class Address
                {
                    public string street { get; set; }
                    public string city { get; set; }
                    public string postCode { get; set; }
                    public string countryCode { get; set; }
                }

                public class Buyer
                {
                    public string id { get; set; }
                    public string email { get; set; }
                    public string login { get; set; }
                    public string firstName { get; set; }
                    public string lastName { get; set; }
                    public string companyName { get; set; }
                    public bool guest { get; set; }
                    public string personalIdentity { get; set; }
                    public string phoneNumber { get; set; }
                    public Address address { get; set; }
                }

                public class PaidAmount
                {
                    public string amount { get; set; }
                    public string currency { get; set; }
                }

                public class Payment
                {
                    public string id { get; set; }
                    public string type { get; set; }
                    public string provider { get; set; }
                    public DateTime? finishedAt { get; set; }
                    public PaidAmount paidAmount { get; set; }
                }

                public class Address2
                {
                    public string firstName { get; set; }
                    public string lastName { get; set; }
                    public string street { get; set; }
                    public string city { get; set; }
                    public string zipCode { get; set; }
                    public string countryCode { get; set; }
                    public string companyName { get; set; }
                    public string phoneNumber { get; set; }
                }

                public class Method
                {
                    public string id { get; set; }
                    public string name { get; set; }
                }

                public class Address3
                {
                    public string street { get; set; }
                    public string zipCode { get; set; }
                    public string city { get; set; }
                }

                public class PickupPoint
                {
                    public string id { get; set; }
                    public string name { get; set; }
                    public string description { get; set; }
                    public Address3 address { get; set; }
                }

                public class Cost
                {
                    public string amount { get; set; }
                    public string currency { get; set; }
                }

                public class Guaranteed
                {
                    public DateTime? from { get; set; }
                    public DateTime? to { get; set; }
                }

                public class Time
                {
                    public Guaranteed guaranteed { get; set; }
                }

                public class Delivery
                {
                    public Address2 address { get; set; }
                    public Method method { get; set; }
                    public PickupPoint pickupPoint { get; set; }
                    public Cost cost { get; set; }
                    public Time time { get; set; }
                    public bool smart { get; set; }
                }

                public class Company
                {
                    public string name { get; set; }
                    public string taxId { get; set; }
                }

                public class NaturalPerson
                {
                    public string firstName { get; set; }
                    public string lastName { get; set; }
                }

                public class Address4
                {
                    public string street { get; set; }
                    public string city { get; set; }
                    public string zipCode { get; set; }
                    public string countryCode { get; set; }
                    public Company company { get; set; }
                    public NaturalPerson naturalPerson { get; set; }
                }

                public class Invoice
                {
                    public bool required { get; set; }
                    public Address4 address { get; set; }
                }

                public class External
                {
                    public string id { get; set; }
                }

                public class Offer
                {
                    public string id { get; set; }
                    public string name { get; set; }
                    public External external { get; set; }
                }

                public class OriginalPrice
                {
                    public string amount { get; set; }
                    public string currency { get; set; }
                }

                public class Price
                {
                    public string amount { get; set; }
                    public string currency { get; set; }
                }

                public class Price2
                {
                    public string amount { get; set; }
                    public string currency { get; set; }
                }

                public class SelectedAdditionalService
                {
                    public string definitionId { get; set; }
                    public string name { get; set; }
                    public Price2 price { get; set; }
                    public int quantity { get; set; }
                }

                public class LineItem
                {
                    public string id { get; set; }
                    public Offer offer { get; set; }
                    public int quantity { get; set; }
                    public OriginalPrice originalPrice { get; set; }
                    public Price price { get; set; }
                    public List<SelectedAdditionalService> selectedAdditionalServices { get; set; }
                    public DateTime boughtAt { get; set; }
                }

                public class PaidAmount2
                {
                    public string amount { get; set; }
                    public string currency { get; set; }
                }

                public class Surcharge
                {
                    public string id { get; set; }
                    public string type { get; set; }
                    public string provider { get; set; }
                    public DateTime? finishedAt { get; set; }
                    public PaidAmount2 paidAmount { get; set; }
                }

                public class Discount
                {
                    public string type { get; set; }
                }

                public class TotalToPay
                {
                    public string amount { get; set; }
                    public string currency { get; set; }
                }

                public class Summary
                {
                    public TotalToPay totalToPay { get; set; }
                }

                public class RootObject
                {
                    public string id { get; set; }
                    public string messageToSeller { get; set; }
                    public Buyer buyer { get; set; }
                    public Payment payment { get; set; }
                    public string status { get; set; }
                    public Delivery delivery { get; set; }
                    public Invoice invoice { get; set; }
                    public List<LineItem> lineItems { get; set; }
                    public List<Surcharge> surcharges { get; set; }
                    public List<Discount> discounts { get; set; }
                    public Summary summary { get; set; }
                }
                 
            }
            #endregion
            public static void GetOrders()
            {
                Dal.AllegroScan asc = new Dal.AllegroScan();
                List<Dal.AllegroUser> users = asc.GetAllegroMyUsers();//.Where(x => x.UserId == 44282528).ToList();


                foreach (Dal.AllegroUser au in users)
                    //GetEventLast(au);
                    GetEvents(au);
            }

            private static void GetEvents(AllegroUser au)
            {
                try
                {
                    if (au == null)
                        return;

                    long? lastId = GetLastId(au);
                    string from = "";
                    if (lastId.HasValue)
                        from = String.Format("&from={0}", lastId);

                    HttpWebRequest request = GetHttpWebRequest(
                        String.Format("/order/events?limit=1000{0}", from)//, limit, offset)
                        , "GET", null, au.UserId);



                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    Events.RootObject orders = json_serializer.Deserialize<Events.RootObject>(text);


                    SaveEvents(orders.events);



                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd aktualizacji ofert. Allegro.UserId: {0}", au.UserId));

                }
            }

            private static void GetCheckoutForm(int orderId, long userId, Guid checkoutFormId)
            {
                try
                {

                    HttpWebRequest request = GetHttpWebRequest(
                        String.Format("/order/checkout-forms/{0}", checkoutFormId)
                        , "GET", null, userId);


                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    CheckoutForm.RootObject checkoutForm = json_serializer.Deserialize<CheckoutForm.RootObject>(text);


                    SetCheckoutForm(orderId, checkoutForm);


                }
                catch (WebException ex)
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (httpResponse == null)
                        {
                            Bll.ErrorHandler.SendError(ex, ex.Message);


                        }
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string text = reader.ReadToEnd();


                            Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania zamówienia {0}, {1}", userId, text));
                        }
                    }
                }
                catch (Exception ex)
                { 
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania zamówienia {0}, {1}", userId, ex.Message));
                }
            }

            private static void SetCheckoutForm(int orderId, CheckoutForm.RootObject checkoutForm)
            {
                try
                {
                    Dal.AllegroRestHelper arh = new AllegroRestHelper();
                     
                    Dal.Order order = arh.GetOrderByCheckoutFormId(checkoutForm.id);
                    
                    //order.FirstName = checkoutForm.buyer.firstName;
                    //order.LastName = checkoutForm.buyer.lastName;
                    //if (checkoutForm.buyer.address != null)
                    //{
                    //    order.City = checkoutForm.buyer.address.city;
                    //    order.Postcode = checkoutForm.buyer.address.postCode.Trim();
                    //    order.Address = checkoutForm.buyer.address.street;
                    //} 
                    order.Phone = checkoutForm.buyer.phoneNumber;
                    order.ShipmentCountryCode = "PL";
                    order.ShippingCurrencyCode = "PLN";
                    order.ShippingCurrencyRate = 1;

                    if (checkoutForm.delivery != null)
                    {
                        if (checkoutForm.delivery.address != null)
                        {
                            order.ShipmentFirstName = checkoutForm.delivery.address.firstName;
                            order.ShipmentLastName = checkoutForm.delivery.address.lastName;
                            order.ShipmentCity = checkoutForm.delivery.address.city;
                            order.ShipmentPostcode = checkoutForm.delivery.address.zipCode;
                            order.ShipmentAddress = checkoutForm.delivery.address.street;
                            order.ShipmentCompanyName = checkoutForm.delivery.address.companyName;
                            order.Phone = checkoutForm.delivery.address.phoneNumber;
                            order.ShipmentCountryCode = checkoutForm.delivery.address.countryCode;
                        } 
                        order.ShippingCost = Decimal.Parse(checkoutForm.delivery.cost.amount, System.Globalization.CultureInfo.InvariantCulture);
                        order.ShippingAmountCurrency = order.ShippingCost;
                        order.ShippingCurrencyRate = 1;
                   
                    }
                    if (checkoutForm.delivery == null || checkoutForm.delivery.address == null)
                    {
                        order.ShipmentFirstName = checkoutForm.buyer.firstName;
                        order.ShipmentLastName = checkoutForm.buyer.lastName;
                        if (checkoutForm.buyer.address != null)
                        {
                            order.ShipmentCity = checkoutForm.buyer.address.city;
                            order.ShipmentPostcode = checkoutForm.buyer.address.postCode.Trim();
                            order.ShipmentAddress = checkoutForm.buyer.address.street;
                            order.ShipmentCountryCode = checkoutForm.buyer.address.countryCode;
                        }
                        order.Phone = checkoutForm.buyer.phoneNumber;
                    }
                    Dal.Invoice invoice = null;
                    if (checkoutForm.invoice != null)
                    {
                        if (checkoutForm.invoice.required && checkoutForm.invoice.address !=null)
                        {
                            invoice = new Invoice()
                            {
                                Address = checkoutForm.invoice.address.street,
                                City = checkoutForm.invoice.address.city,
                                CompanyId = (int)Dal.Helper.DefaultCompanyId,
                                Email = checkoutForm.buyer.email,
                                InvoiceTypeId = 2,
                                Postcode = checkoutForm.invoice.address.zipCode,
                                InvoiceDate = DateTime.Now
                            };
                            if (checkoutForm.invoice.address.company != null)
                            {
                                
                                invoice.CompanyName = checkoutForm.invoice.address.company.name;
                                invoice.Nip = checkoutForm.invoice.address.company.taxId;
                            }
                            else
                            {
                                if (checkoutForm.invoice.address.naturalPerson != null)
                                    invoice.CompanyName = String.Format("{0} {1}", checkoutForm.invoice.address.naturalPerson.firstName, checkoutForm.invoice.address.naturalPerson.lastName);
                                else
                                    invoice.CompanyName = "";
                                invoice.Nip = "";
                            }
                        }
                    }
                    List<Dal.OrderPayment> ops = new List<OrderPayment>();
                    if (checkoutForm.payment != null && checkoutForm.payment.type == "ONLINE" && checkoutForm.payment.paidAmount!=null)
                    {
                        Dal.OrderPayment op = new OrderPayment()
                        {
                            Comment = String.Format("Id płatności: {0}, typ: {1}"
                            , checkoutForm.payment.id
                            , checkoutForm.payment.provider),
                            Amount = Decimal.Parse(checkoutForm.payment.paidAmount.amount, System.Globalization.CultureInfo.InvariantCulture),
                            InsertDate = checkoutForm.payment.finishedAt.Value,
                            InsertUser = "system",
                            OrderId = order.OrderId,
                            PaymentTypeId = (int)Dal.Helper.OrderPaymentType.PayU23,
                            ExternalPaymentId = checkoutForm.payment.id,
                            CurrencyCode = "PLN",
                            CurrencyRate = 1,
                            AmountCurrency= Decimal.Parse(checkoutForm.payment.paidAmount.amount, System.Globalization.CultureInfo.InvariantCulture)
                        };
                        ops.Add(op);
                    }
                    if (checkoutForm.surcharges != null && checkoutForm.surcharges.Count>0)
                    {
                        foreach (CheckoutForm.Surcharge s in checkoutForm.surcharges)
                        {
                            Dal.OrderPayment op = new OrderPayment()
                            {
                                Comment = String.Format("Id dopłaty : {0}, typ: {1}"
                                , s.id
                                , s.provider),
                                Amount = Decimal.Parse(s.paidAmount.amount, System.Globalization.CultureInfo.InvariantCulture),
                                InsertDate = s.finishedAt.Value,
                                InsertUser = "system",
                                OrderId = order.OrderId,
                                PaymentTypeId = (int)Dal.Helper.OrderPaymentType.PayU23,
                                ExternalPaymentId = s.id,
                                CurrencyCode = "PLN",
                                CurrencyRate = 1,
                                AmountCurrency = Decimal.Parse(s.paidAmount.amount, System.Globalization.CultureInfo.InvariantCulture)
                            };

                            ops.Add(op);
                        }
                    }

                    List<Dal.OrderProduct> products = new List<OrderProduct>();

                    Dal.OrderShipping orderShipping = SetShipping(checkoutForm, ref order);

                    foreach (CheckoutForm.LineItem item in checkoutForm.lineItems)
                    {
                        Dal.OrderProduct op = new OrderProduct()
                        {
                            OrderId = orderId, 
                            Quantity = item.quantity,
                            Price = Decimal.Parse(item.price.amount, System.Globalization.CultureInfo.InvariantCulture),// * (decimal)1.23,
                            ExternalProductId = Int64.Parse(item.offer.id),
                            ProductName = item.offer.name,
                            VAT = Dal.Helper.VAT,
                            Rebate = 0,
                            ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
                            OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New,
                            ExternalAllegroLinieItemId = Guid.Parse(item.id),
                            CurrencyRate=1,
                            PriceCurrency = Decimal.Parse(item.price.amount, System.Globalization.CultureInfo.InvariantCulture)
                        };

                        if (item.offer.external != null && item.offer.external.id != null)
                        {
                            op.ProductCatalogId = Int32.Parse(item.offer.external.id);


                           // products.AddRange( GetComputedProducts(orderId, Guid.Parse(item.id), op));

                        }
                        //else
                        products.Add(op);
                    }
                    Dal.OrderStatusHistory osh = null;

                    if (!String.IsNullOrEmpty(checkoutForm.messageToSeller))
                    {
                        osh = new OrderStatusHistory()
                        {
                            Comment = checkoutForm.messageToSeller,
                            InsertDate = DateTime.Now,
                            InsertUser = checkoutForm.buyer.login,
                            OrderId = orderId,
                            OrderStatusId = (int)Dal.Helper.OrderStatus.Comment

                        };
                    }


                    arh.SetOrder(order, invoice, ops, products, orderShipping, osh);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania zamówienia checkoutFormId: {0}, msg; {1}", checkoutForm.id, ex.Message));

                }
            }
            private static Dal.OrderShipping SetShipping(CheckoutForm.RootObject checkoutForm, ref Dal.Order order)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                List<Dal.ShopShipping> ss = sh.GetShopShipping(Dal.Helper.ShopType.Allegro);
 

                Dal.OrderHelper oh = new Dal.OrderHelper();


               // Dal.ShippingCompany sc = oh.GetShipppingCompanies().Where(x => x.IsDefault).FirstOrDefault();
                Dal.OrderShipping orderShipping = new OrderShipping()
                {
                    OrderId = order.OrderId,
                    COD = null,
                    InsertDate = DateTime.Now,
                    InsertUser = "System",
                    OrderShippingStatusId = (int)Dal.Helper.OrderShippingStatus.Temporary,
                   // ShippingCompanyId = sc.ShippingCompanyId,
                    ShippingServiceTypeId = (int)Dal.Helper.ShippingServiceType.ForOrder,
                    TrackingNumberSent = false,
                    IsParcelReady = false

                };

                if (checkoutForm.delivery!=null && checkoutForm.delivery.pickupPoint != null)
                    orderShipping.ServicePoint = checkoutForm.delivery.pickupPoint.id;

                if (checkoutForm.delivery != null && checkoutForm.delivery.method != null)
                {
                    int shopId = order.ShopId;
                    var q = ss.Where(x => x.ShopId == shopId && x.ShopShippingId == checkoutForm.delivery.method.id).FirstOrDefault();
                    orderShipping.ShippingCompanyId = q.ShippingCompanyId.Value;
                    orderShipping.ShippingServiceModeId = q.ShippingServiceModeId;
                

                }

                if (checkoutForm.payment.type=="CASH_ON_DELIVERY")
                    orderShipping.COD = Decimal.Parse(checkoutForm.summary.totalToPay.amount, System.Globalization.CultureInfo.InvariantCulture);
            


                return orderShipping;
            }

            //public static List<OrderProduct> GetComputedProducts(Dal.Order order, Guid externalAllegroLinieItemId, OrderProduct op)

            //{
            //    GetComputedProducts(order)
            //}
            public static List<OrderProduct> GetComputedProducts(int orderId, Guid externalAllegroLinieItemId, OrderProduct op)

            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                List<OrderProduct> products = new List<OrderProduct>();
                Dal.ProductCatalog pc = Dal.DbHelper.ProductCatalog.GetProductCatalog(op.ProductCatalogId.Value);



                if (pc != null && pc.ProductTypeId == 3)
                {

                    List<Dal.ProductCatalogSubProductsView> subProducts =
                    pch.GetProductCatalogSubProducts(op.ProductCatalogId.Value).Where(x => x.ProductTypeId == (int)Dal.Helper.ProductType.ComboProduct).ToList();

                    decimal sum = subProducts.Sum(x => x.PriceBruttoFixed * x.Quantity);

                    foreach (Dal.ProductCatalogSubProductsView sp in subProducts)
                    {

                        decimal rate = op.Price / sum;

                        Dal.OrderProduct opsub = new OrderProduct()
                        {
                            OrderId = orderId,
                            Quantity = op.Quantity * sp.Quantity,
                            Price = sp.PriceBruttoFixed * rate,
                            ExternalProductId = 0,// Int64.Parse(item.offer.id),
                            ProductName = sp.Name,
                            VAT = Dal.Helper.VAT,
                            Rebate = 0,
                            ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
                            OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New,
                            ExternalAllegroLinieItemId = externalAllegroLinieItemId,
                            ProductCatalogId = sp.ProductCatalogRefId,
                            CurrencyRate = 1,
                            PriceCurrency = sp.PriceBruttoFixed * rate

                        };

                        products.Add(opsub);
                    }


                    op.Quantity = 0;

                }
                products.Add(op);

                return products;
            }

            private static long? GetLastId(AllegroUser au)
            {
                Dal.AllegroRestHelper arh = new AllegroRestHelper();

                return arh.GetAllegroOrderLastId(au.UserId);
            }

            private static void SaveEvents(List<Events.Event> events)
            {
                List<Dal.AllegroOrderLineItem> lineItems = new List<AllegroOrderLineItem>();



                List<Dal.AllegroUser> users = new List<AllegroUser>();

                users.AddRange(events.Select(x => new Dal.AllegroUser()
                {
                    UserId = Int64.Parse(x.order.buyer.id),
                    Follow = false,
                    UserName = x.order.buyer.login
                }).ToList());

                Dal.AllegroRestHelper arh = new AllegroRestHelper();

                arh.SetAllegroUsers(users);

                foreach (Events.Event o in events)
                {
                    Dal.AllegroOrder ao = new AllegroOrder()
                    {
                        CheckoutFormId = Guid.Parse(o.order.checkoutForm.id),
                        Email = o.order.buyer.email,
                        Guest = o.order.buyer.guest,
                        Login = o.order.buyer.login,
                        Id = Int64.Parse(o.id),
                        OccuredAt = o.occurredAt,
                        SellerId = Int64.Parse(o.order.seller.id),
                        UserId = Int64.Parse(o.order.buyer.id),
                        Type = o.type,
                        IsProcessed = false
                    };
                    
                    foreach (Events.LineItem item in o.order.lineItems)
                    {
                        Dal.AllegroOrderLineItem i = new AllegroOrderLineItem()
                        {
                            BoughtAt = item.boughtAt,
                            CheckoutFormId = Guid.Parse(item.id),
                            ExternalId = item.offer.external == null ? null : item.offer.external.id,
                            ItemId = Int64.Parse(item.offer.id),
                            Name = item.offer.name,
                            OriginalPrice = Decimal.Parse(item.originalPrice.amount, System.Globalization.CultureInfo.InvariantCulture),
                            Price = Decimal.Parse(item.price.amount, System.Globalization.CultureInfo.InvariantCulture),
                            Quantity = item.quantity,
                            AllegroOrder = ao,

                        };
                        lineItems.Add(i);
                    }
                }

                arh.SetAllegroOrders(lineItems);

            }

            //private static void GetEventLast(AllegroUser au)
            //{
            //    try
            //    {
            //        if (au == null)
            //            return;

            //        HttpWebRequest request = GetHttpWebRequest(
            //            String.Format("/order/event-stats", au.UserId)//, limit, offset)
            //            , "GET", null, au.UserId);



            //        string text = null;
            //        using (WebResponse webResponse = request.GetResponse())
            //        {
            //            Stream responseStream = webResponse.GetResponseStream();
            //            StreamReader reader = new StreamReader(responseStream);
            //            text = reader.ReadToEnd();
            //        }

            //        var json_serializer = new JavaScriptSerializer();


            //        EventLast.RootObject offers = json_serializer.Deserialize<EventLast.RootObject>(text);
            //        //Console.WriteLine(String.Format("Liczba ofert: {0}/{1}:", offers.count, offers.offers.Count));


            //        //if (limit <= offers.count)

            //        //    GetSellerOffers(au, limit, offset + limit);


            //    }
            //    catch (Exception ex)
            //    {
            //        Bll.ErrorHandler.SendError(ex, String.Format("Błąd aktualizacji ofert. Allegro.UserId: {0}", au.UserId));

            //    }
            //}

            public static void Process()
            {
                Dal.AllegroRestHelper arh = new AllegroRestHelper();

                List<Dal.AllegroOrder> allegroOrders = arh.GetAllegroOrdersToProcess();

                string[] checkoutFormIds = allegroOrders.Select(x => x.CheckoutFormId.ToString()).Distinct().ToArray();

                foreach (string checkoutFormId in checkoutFormIds)
                {

                    Dal.Order order = arh.GetOrderByCheckoutFormId(checkoutFormId);

                    var aos = allegroOrders.Where(x => x.CheckoutFormId == Guid.Parse(checkoutFormId))
                        .OrderBy(x => x.OccuredAt)
                        .ToList();

                    int orderId = 0;

                    if (order != null)
                        orderId = order.OrderId;

                        foreach (Dal.AllegroOrder ao in aos)
                    {
                        if (orderId == 0)
                            orderId = SetOrder(ao); 

                        //switch (ao.Type)
                        //{
                        //    case "BOUGHT":
                        //        SetOrderProduct(orderId, ao);
                        //        break;
                        //    case "FILLED_IN":  
                        //    case "READY_FOR_PROCESSING":
                        //        GetCheckoutForm(ao.SellerId, ao.CheckoutFormId);
                        //        break;

                        //}

                        GetCheckoutForm(orderId, ao.SellerId, ao.CheckoutFormId);
                        arh.SetAllegroOrderProcessed(ao.Id);
                    }

                }
            }

            //private static void SetOrderProduct(int orderId, AllegroOrder ao)
            //{
            //    Dal.AllegroRestHelper arh = new AllegroRestHelper();

            //    Dal.AllegroOrderLineItem item = arh.GetAllegroOrderLineItem(ao.Id);

            //    Dal.OrderProduct op = new OrderProduct()
            //    {
            //        OrderId = orderId, 
            //        Quantity = item.Quantity,
            //        Price = item.Price,
            //        ExternalProductId = item.ItemId,
            //        ProductName = item.Name,
            //        VAT = Dal.Helper.VAT,
            //        Rebate = 0,
            //        ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
            //        OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New
            //    };

            //    if (item.ExternalId != null)
            //        op.ProductCatalogId = Int32.Parse(item.ExternalId);

            //    arh.SetOrderProduct(op);

            //}

            private static int SetOrder(AllegroOrder ao)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                int shopId = sh.GetShopIdByAllegroUserId(ao.SellerId);

                Dal.Order order = new Dal.Order()
                { 
                    OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                    ShopId = shopId,
                    Email = ao.Email,
                    ExternalUserId = ao.UserId,
                    InsertDate = ao.OccuredAt,
                    
                    Phone = "",
                    DoNotAutoEdit = false,
                    AmountPaid = 0,
                    AmountToPay = 0,
                    ExternalOrderNumber = ao.CheckoutFormId.ToString(),
                    ShippingCostVAT = Dal.Helper.VAT,
                    ParActive = true,
                    CompanyId=Dal.Helper.DefaultCompanyId,
                    ShippingCurrencyRate=1,
                    ShippingCost=0,
                    ShippingAmountCurrency=0,
                    ShipmentCountryCode="PL",
                    ShippingCurrencyCode="PLN"

                };
                Dal.OrderStatusHistory osh = new OrderStatusHistory()
                {
                    InsertDate = DateTime.Now,
                    InsertUser = "system",
                    Order = order,
                    OrderStatusId = (int)Dal.Helper.OrderStatus.New
                };

                Dal.AllegroRestHelper arh = new AllegroRestHelper();

                arh.SetOrder(osh);

                return order.OrderId;
            }

            public class Shipment
            {

                public class ShipmentRootObject
                {
                    public string carrierId { get; set; }
                    public string waybill { get; set; }
                    public string carrierName { get; set; }
                    public List<LineItem> lineItems { get; set; }

                    public class LineItem
                    { 
                        public string id { get; set; } 
                    }
                }
            }

            public static void SetShipment()
            {
                if (Dal.Helper.Env != Dal.Helper.EnvirotmentEnum.Prod)
                    return;


                Dal.AllegroRestHelper arh = new AllegroRestHelper();
                List<Dal.OrderTransactiongNumber> orders = arh.GetOrderTransactionNumber(Dal.Helper.ShopType.Allegro);

                foreach (Dal.OrderTransactiongNumber order in orders)
                {
                    try
                    {
                        long? userId = arh.GetSellerIdByCheckouFormId(Guid.Parse(order.CheckoutFormId));

                        if (userId.HasValue == false)
                            continue;

                        HttpWebRequest request = GetHttpWebRequest(String.Format("/order/checkout-forms/{0}/shipments", order.CheckoutFormId), "POST", null, userId.Value);

                        Shipment.ShipmentRootObject sh = new Shipment.ShipmentRootObject();
                        sh.carrierId = order.AllegroOperatorCode;
                        sh.carrierName = order.Name;
                        sh.waybill = order.ShipmentTrackingNumber;
                        sh.lineItems = new List<Shipment.ShipmentRootObject.LineItem>();
                        sh.lineItems.AddRange(GetLineItemIds(order.CheckoutFormId));


                        Stream dataStream = request.GetRequestStream();
                        string jsonEncodedParams = Bll.RESTHelper.ToJson(sh);
                        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                        byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();


                        WebResponse webResponse = request.GetResponse();

                        Stream responseStream = webResponse.GetResponseStream();

                        StreamReader reader = new StreamReader(responseStream);

                        string text = reader.ReadToEnd();


                        LajtIt.Dal.DbHelper.Orders.SetOrderTrackingNumberSent(order.OrderId);
                    }
                    catch (WebException ex)
                    {
                        using (WebResponse response = ex.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            if (httpResponse == null)
                            {
                                Bll.ErrorHandler.SendError(ex, String.Format("{0} order.CheckoutFormId {1}", ex.Message, order.CheckoutFormId));


                            }
                            //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string text = reader.ReadToEnd();


                                Bll.ErrorHandler.SendError(ex, String.Format("Błąd wysyłania numeru przesyłki {0}", String.Format("{0} order.CheckoutFormId {1}, OrderId: <a href='http://192.168.0.107/Order.aspx?id={2}'>{2}</a>", text, order.CheckoutFormId, order.OrderId)));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd wysyłania numeru przesyłki  {0}", String.Format("{0} order.CheckoutFormId {1}, OrderId: <a href='http://192.168.0.107/Order.aspx?id={2}'>{2}</a>", ex.Message, order.CheckoutFormId, order.OrderId)));
                    }
                }
            }

            private static IEnumerable<Shipment.ShipmentRootObject.LineItem> GetLineItemIds(string checkoutFormId)
            {
                Dal.AllegroRestHelper arh = new AllegroRestHelper();
                return arh.GetAllegroOrderLineItems(checkoutFormId)
                    .Select(x => new Shipment.ShipmentRootObject.LineItem()
                    {
                        id = x.ToString()
                    }).ToList();
            }
        }
    }
}
