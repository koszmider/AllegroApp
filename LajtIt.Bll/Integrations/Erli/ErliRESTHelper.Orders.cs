using LajtIt.Dal;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static LajtIt.Bll.ShopHelper;

namespace LajtIt.Bll
{
    public partial class ErliRESTHelper
    {
        public class Orders
        {
            public class DeliveryAddress
            {
                public string firstName { get; set; }
                public string lastName { get; set; }
                public string companyName { get; set; }
                public string address { get; set; }
                public string zip { get; set; }
                public string city { get; set; }
                public string country { get; set; }
                public string phone { get; set; }
            }

            public class InvoiceAddress
            {
                public string address { get; set; }
                public string zip { get; set; }
                public string city { get; set; }
                public string country { get; set; }
                public string type { get; set; }
                public string companyName { get; set; }
                public string nip { get; set; }
            }

            public class User
            {
                public string email { get; set; }
                public DeliveryAddress deliveryAddress { get; set; }
                public InvoiceAddress invoiceAddress { get; set; }
            }

            public class Item
            {
                public int id { get; set; }
                public string externalId { get; set; }
                public int quantity { get; set; }
                public int unitPrice { get; set; }
                public string name { get; set; }
                public string slug { get; set; }
                public string ean { get; set; }
                public string sku { get; set; }
            }
            public class PickupPlace
            {
                public string externalId { get; set; }
                public string provider { get; set; }
                public string description { get; set; }
                public string address { get; set; }
                public string city { get; set; }
                public string zip { get; set; }
            }
            public class Delivery
            {
                public string name { get; set; }
                public string typeId { get; set; }
                public int price { get; set; }
                public bool cod { get; set; }
                public PickupPlace pickupPlace { get; set; }
            }

            public class Order
            {
                public string id { get; set; }
                public string status { get; set; }
                public User user { get; set; }
                public List<Item> items { get; set; }
                public Delivery delivery { get; set; }
                public int totalPrice { get; set; }
                public DateTime created { get; set; }
                public string comment { get; set; }
            }

            public class RootOrder
            {
                public Order MyArray { get; set; }
            }
            public class ProductSearch
            {
                public ErliRESTHelper.Products.Pagination pagination { get; set; }
            }

            public static void GetOrders(Dal.Helper.Shop shop)
            {

                // GetInbox(shop);

                ProductSearch ps = new ProductSearch()
                {
                    pagination = new ErliRESTHelper.Products.Pagination()
                    {
                        limit = 200,
                        after = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd"),
                        order = "ASC",
                        sortField = "created"
                    }
                };
                try
                {
                    HttpWebRequest request = ErliRESTHelper.GetHttpWebRequest(shop, "/orders/_search", "POST");

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(ps);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();




                    string text = ShopRestHelper.GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    List<Order> orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(text);

                    ProcessOrders(shop, orders);

                    //if (orders.Count() == rp.pagination.limit)
                    //    GetProducts(shop, after + 200);


                }

                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, shop);

                }
            }

            private static void GetInbox(Dal.Helper.Shop shop)
            {
                try
                {
                    HttpWebRequest request = ErliRESTHelper.GetHttpWebRequest(shop, "/inbox", "GET");





                    string text = ShopRestHelper.GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    List<Order> orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(text);

                    ProcessOrders(shop, orders);

                    //if (orders.Count() == rp.pagination.limit)
                    //    GetProducts(shop, after + 200);


                }

                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, shop);

                }
            }

            public static void GetOrderPayments(Dal.Helper.Shop shop)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();

                List<Dal.ShopOrder> orders = sh.GetShopOrdersWithoutPayment(shop);


                foreach (Dal.ShopOrder order in orders)
                {

                    try
                    {
                        HttpWebRequest request = ErliRESTHelper.GetHttpWebRequest(shop, String.Format(@"/orders/{0}", order.ShopOrderNumber), "GET");

                        string text = ShopRestHelper.GetTextFromWebResponse(request);

                        var json_serializer = new JavaScriptSerializer();

                        Order shopOrder = json_serializer.Deserialize<Order>(text);



                        if (shopOrder.status == "purchased" && shopOrder.delivery.cod == false)
                        {
                            OrderPayment p = new OrderPayment()
                            {
                                OrderId = order.OrderId.Value,
                                Amount = (decimal)(shopOrder.totalPrice / 100.00),
                                Comment = "",
                                InsertDate = shopOrder.created,
                                InsertUser = "system",
                                PaymentTypeId = (int)Dal.Helper.OrderPaymentType.PayU23,
                                CurrencyCode = "PLN",
                                CurrencyRate = 1,
                                AmountCurrency = (decimal)(shopOrder.totalPrice / 100.00)
                            };
                            Dal.ShopHelper oh = new Dal.ShopHelper();
                            oh.SetShopOrderPayment(shop, order.OrderId.Value, p, null, null);

                        }



                    }

                    catch (WebException ex)
                    {
                        ShopRestHelper.ProcessException(ex, shop);

                    }

                }



            }


            private static void ProcessOrders(Dal.Helper.Shop shop, List<Order> orders)
            {

                Dal.ShopHelper sh = new Dal.ShopHelper();

                string[] shopOrderNumbers = orders.Select(x => x.id).ToArray();

                string[] orderToProcess = sh.InsertOrders(shopOrderNumbers, null, shop, "system");

                ProcessOrders(shop, orderToProcess, orders);

            }

            private static void ProcessOrders(Dal.Helper.Shop shop, string[] orderToProcess, List<Order> orders)
            {
                foreach (string orderNumber in orderToProcess)
                {
                    bool result = CreateOrder(shop, orderNumber, orders);



                }
            }
            private static bool CreateOrder(Dal.Helper.Shop shop, string orderNumber, List<Order> orders)
            {
                Order shopOrder = orders.Where(x => x.id == orderNumber).FirstOrDefault();



                Dal.ShopHelper sh = new Dal.ShopHelper();
                Dal.ShopOrder so = sh.GetShopOrder(shop, orderNumber);


                Dal.Order order = new Dal.Order()
                {
                    ExternalOrderNumber = orderNumber,

                    ShipmentFirstName = shopOrder.user.deliveryAddress.firstName.Trim(),
                    ShipmentLastName = shopOrder.user.deliveryAddress.lastName.Trim(),
                    Email = shopOrder.user.email,
                    ExternalUserId = 0,//orderEmpik.customer.customer_id,
                    InsertDate = shopOrder.created,
                    OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                    //ShippintTypeId = 0,
                    ShopId = (int)shop,
                    Phone = shopOrder.user.deliveryAddress.phone,
                    ShipmentAddress = shopOrder.user.deliveryAddress.address,
                    ShipmentCity = shopOrder.user.deliveryAddress.city,
                    ShipmentCompanyName = shopOrder.user.deliveryAddress.companyName,
                    ShipmentPostcode = shopOrder.user.deliveryAddress.zip,
                    CompanyId = Dal.Helper.DefaultCompanyId,
                    DoNotAutoEdit = false,
                    ShippingCostVAT = 0.23M,
                    ParActive = true,
                    ShippingCost = (decimal)(shopOrder.delivery.price / 100.00),
                    ShippingAmountCurrency = (decimal)(shopOrder.delivery.price / 100.00),
                    ShippingCurrencyCode = "PLN",
                    ShippingCurrencyRate = 1,
                    ShipmentCountryCode = shopOrder.user.deliveryAddress.country.ToUpper()

                };



                Dal.Invoice invoice = null;
                if (shopOrder.user.invoiceAddress != null)
                {
                    invoice = new Invoice()
                    {
                        Address = shopOrder.user.invoiceAddress.address,
                        City = shopOrder.user.invoiceAddress.city,
                        CompanyId = Dal.Helper.DefaultCompanyId,
                        Email = shopOrder.user.email,
                        InvoiceTypeId = 2,
                        IsDeleted = false,
                        IsLocked = false,
                        Postcode = shopOrder.user.invoiceAddress.zip,

                        InvoiceDate = shopOrder.created

                    };

                    if (shopOrder.user.invoiceAddress.type == "person")
                    {
                        invoice.CompanyName = String.Format("{0} {1}", shopOrder.user.deliveryAddress.firstName, shopOrder.user.deliveryAddress.lastName);
                        invoice.Nip = "";
                    }
                    else
                    {
                        invoice.CompanyName = shopOrder.user.invoiceAddress.companyName;
                        invoice.Nip = shopOrder.user.invoiceAddress.nip;
                    }

                order.Invoice = invoice;
                }
                Dal.OrderStatusHistory osh = new Dal.OrderStatusHistory()
                {
                    InsertDate = DateTime.Now,
                    InsertUser = "System",
                    Order = order,
                    OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                    Comment = shopOrder.comment
                };


                List<Dal.OrderProduct> products = new List<Dal.OrderProduct>();

                foreach (Item ol in shopOrder.items)
                {
                    Dal.OrderProduct op = new Dal.OrderProduct()
                    {
                        ExternalProductId = ol.id,
                        Order = order,
                        OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New,
                        Price = (decimal)(ol.unitPrice / 100.00),
                        Quantity = ol.quantity,
                        ProductName = ol.name,
                        ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
                        Rebate = 0,
                        VAT = 0.23M,
                        ProductCatalogId = Int32.Parse(ol.externalId),
                        CurrencyRate = 1,
                        PriceCurrency = (decimal)(ol.unitPrice / 100.00)
                    };
                    products.Add(op);

                }

                OrderPayment p = null;

                if (shopOrder.status == "purchased" && shopOrder.delivery.cod == false)
                {
                    p = new OrderPayment()
                    {
                        Order = order,
                        Amount = (decimal)(shopOrder.totalPrice / 100.00),
                        Comment = "",
                        InsertDate = shopOrder.created,
                        InsertUser = "system",
                        PaymentTypeId = (int)Dal.Helper.OrderPaymentType.PayU23,
                        CurrencyCode = "PLN",
                        CurrencyRate = 1,
                        AmountCurrency = (decimal)(shopOrder.totalPrice / 100.00)
                    };
                }


                Dal.OrderShipping orderShipping = SetShipping(shop, shopOrder, ref order);
                so.IsProcessed = true;

                sh.SetNewOrder(so, invoice, products, shop, osh, p, orderShipping);


                return true;
            }
            private static Dal.OrderShipping SetShipping(Dal.Helper.Shop shop, Order orderErli, ref Dal.Order order)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                List<Dal.ShopShipping> ss = sh.GetShopShipping(shop);


                Dal.OrderHelper oh = new Dal.OrderHelper();


                // Dal.ShippingCompany sc = oh.GetShipppingCompanies().Where(x => x.IsDefault).FirstOrDefault();
                Dal.OrderShipping orderShipping = new OrderShipping()
                {
                    Order1 = order,
                    COD = null,
                    InsertDate = DateTime.Now,
                    InsertUser = "System",
                    OrderShippingStatusId = (int)Dal.Helper.OrderShippingStatus.Temporary,
                    // ShippingCompanyId = sc.ShippingCompanyId,
                    ShippingServiceTypeId = (int)Dal.Helper.ShippingServiceType.ForOrder,
                    TrackingNumberSent = false,
                    IsParcelReady = false

                };

                if (orderErli.delivery.pickupPlace != null)
                {
                    if (orderErli.delivery.pickupPlace.externalId != null)
                        orderShipping.ServicePoint = orderErli.delivery.pickupPlace.externalId;
                }

                var q = ss.Where(x => x.ShopId == (int)shop && x.ShopShippingId == orderErli.delivery.typeId).FirstOrDefault();
                orderShipping.ShippingCompanyId = q.ShippingServiceMode.ShippingCompanyId;
                orderShipping.ShippingServiceModeId = q.ShippingServiceModeId;


                if (q.PayOnDelivery)
                    orderShipping.COD = (decimal)(orderErli.totalPrice / 100.00);



                return orderShipping;
            }


            public class OrderDeliveryUpdate
            {
                public DeliveryTracking deliveryTracking { get; set; }
                public bool? canceled { get; set; }
                public string externalOrderId { get; set; }
            }


            public class DeliveryTracking
            {
                public string status { get; set; }
                public string vendor { get; set; }
                public string trackingNumber { get; set; }
            }
            public static bool SetSentStatus(Dal.Helper.Shop shop, Dal.ShopOrder so, string shopOrderStatusId)
            {
                DeliveryTracking d = new DeliveryTracking()
                {
                    status = shopOrderStatusId
                };
                if (so.Order.OrderShipping != null && !String.IsNullOrEmpty(so.Order.OrderShipping.ShipmentTrackingNumber))
                {
                    d.trackingNumber = so.Order.OrderShipping.ShipmentTrackingNumber;
                    if (so.Order.OrderShipping.ShippingCompany.AllegroOperatorCode != null)
                        d.vendor = so.Order.OrderShipping.ShippingCompany.AllegroOperatorCode.ToLower();
                }
                OrderDeliveryUpdate odu = new OrderDeliveryUpdate()
                {
                    deliveryTracking = d,
                    externalOrderId = so.OrderId.ToString()
                };
                try
                {
                    HttpWebRequest request = ErliRESTHelper.GetHttpWebRequest(shop, String.Format(@"/orders/{0}", so.ShopOrderNumber), "PATCH");

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(odu);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    Console.WriteLine(jsonEncodedParams);

                    string text = ShopRestHelper.GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();
                    return true;
                }

                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, shop);
                    return false;
                }
            }

            public static void SetOrderStatus(Dal.Helper.Shop shop)
            {
                Bll.ShopHelper sh = new ShopHelper();
                sh.SetOrderStatuses(shop);
            }
        }
    }
}