using LajtIt.Dal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    

    public partial class MoreleRESTHelper
    {
        public class Orders
        {
            public class Customer
            {
                public string id { get; set; }
                public string name { get; set; }
                public string company { get; set; }
                public string type { get; set; }
                public string phone { get; set; }
                public string email { get; set; }
                public string billing_name { get; set; }
                public string billing_phone { get; set; }
                public string billing_country { get; set; }
                public string billing_city { get; set; }
                public string billing_street { get; set; }
                public string billing_street_name { get; set; }
                public string billing_street_number { get; set; }
                public string billing_postal_code { get; set; }
                public string billing_nip { get; set; }
                public string shipping_contact { get; set; }
                public string shipping_phone { get; set; }
                public string shipping_country { get; set; }
                public string shipping_city { get; set; }
                public string shipping_street { get; set; }
                public string shipping_street_name { get; set; }
                public string shipping_street_number { get; set; }
                public string shipping_postal_code { get; set; }
            }

            public class Product
            {
                public string id { get; set; }
                public string product_id { get; set; }
                public string status { get; set; }
                public string part_number { get; set; }
                public string date_created { get; set; }
                public string date_modified { get; set; }
                public string currency { get; set; }
                public string quantity { get; set; }
                public string sale_price_brutto { get; set; }
                public string vat { get; set; }
                public string vendor_product_name { get; set; }
                public string brand_code { get; set; }
                public object barcode { get; set; }
            }

            public class Order
            {
                public string order_id { get; set; }
                public string status { get; set; }
                public string is_complete { get; set; }
                public string payment_mode_id { get; set; }
                public string payment_status { get; set; }
                public string delivery_mode { get; set; }
                public string date_created { get; set; }
                public string date_modified { get; set; }
                public string pickup_point { get; set; }
                public string merchant_id { get; set; }
                public string customer_payment { get; set; }
                public string order_value { get; set; }
                public string note { get; set; }
                public string deadline_delay { get; set; }
                public string deadline_at { get; set; }
                public string shipping_cost { get; set; }
                public object waybill_number { get; set; }
                public object waybill_tracking_link { get; set; }
                public object invoice_number { get; set; }
                public object commission_return_status_id { get; set; }
                public object commission_return_request_created_at { get; set; }
                public object commission_return_request_reason_id { get; set; }
                public Customer customer { get; set; }
                public List<Product> products { get; set; }
            }

            public class RootOrders
            {
                public List<Order> data { get; set; }
                public int recordsFiltered { get; set; }
                public int recordsTotal { get; set; }
            }
            public static void GetOrders()
            {


                HttpWebRequest request = GetHttpWebRequest(String.Format("/orders?date_created_from={0:yyyy-MM-dd HH:mm:ss}", DateTime.Now.AddDays(-100)), "GET");

                try
                {
                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    RootOrders orders = json_serializer.Deserialize<RootOrders>(text);

                    Dal.ShopHelper sh = new Dal.ShopHelper();

                    string[] shopOrderNumbers = orders.data.Select(x => x.order_id).ToArray();

                    string[] orderToProcess = sh.InsertOrders(shopOrderNumbers, null, s, "system");

                    ProcessOrders(orderToProcess, orders);

                    List<Dal.ShopOrder> shopOrdersNoPayment = sh.GetShopOrdersWithoutPayment(Dal.Helper.Shop.Morele);

                    CheckOrdersPayments(shopOrdersNoPayment, shopOrderNumbers, orders);
                }
                catch (WebException ex)
                {

                    Bll.ShopRestHelper.ProcessException(ex, s, "Pusto");
                }

            }

            private static void CheckOrdersPayments(List<Dal.ShopOrder> shopOrdersNoPayment, string[] shopOrderNumbers, RootOrders orders)
            {
                foreach(string orderNumber in shopOrderNumbers)
                {
                    Dal.ShopOrder so = shopOrdersNoPayment.Where(x => x.ShopOrderNumber.Equals(orderNumber)).FirstOrDefault();
                    if (so != null)
                    {
                        Order shopOrder = orders.data.Where(x => x.order_id == orderNumber).FirstOrDefault();
                        if (shopOrder != null)
                        {
                            if (shopOrder.payment_mode_id == "3" && shopOrder.payment_status == "1")
                            {
                                Dal.OrderHelper oh = new Dal.OrderHelper();

                                int accountingType;
                                if (!String.IsNullOrEmpty(shopOrder.customer.billing_nip))
                                    accountingType = (int)Dal.Helper.OrderPaymentAccoutingType.Invoice;
                                else
                                    accountingType = (int)Dal.Helper.OrderPaymentAccoutingType.CashRegister;

                                OrderPayment p = new OrderPayment();
                                p.OrderId = (int)so.OrderId;
                                p.Amount = Decimal.Parse(shopOrder.customer_payment, System.Globalization.CultureInfo.InvariantCulture);
                                p.Comment = "";
                                p.InsertDate = DateTime.Parse(shopOrder.date_created);
                                p.InsertUser = "system";
                                p.PaymentTypeId = (int)Dal.Helper.OrderPaymentType.Przelewy24;
                                p.AccountingTypeId = accountingType;
                                p.CurrencyCode = "PLN";
                                p.CurrencyRate = 1;
                                p.AmountCurrency = Decimal.Parse(shopOrder.customer_payment, System.Globalization.CultureInfo.InvariantCulture);

                                if (p != null)
                                {
                                    oh.SetOrderPayment(p, "system");
                                    Dal.Order o = oh.GetOrderByShopAndExternalOrderNumber((int)Dal.Helper.Shop.Morele, orderNumber);
                                    if (o != null)
                                    {
                                        if (o.AmountToPay <= o.AmountPaid)
                                        {
                                            Dal.ShopHelper sh = new Dal.ShopHelper();
                                            sh.SetOrderCheckForPayment(so, Dal.Helper.Shop.Morele, false);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
 
            private static void ProcessOrders(string[] orderToProcess, RootOrders orders)
            {
                foreach (string orderNumber in orderToProcess)
                {
                    bool result = CreateOrder(orderNumber, orders);



                }
            }

            private static bool CreateOrder(string orderNumber, RootOrders orders)
            {
                Order shopOrder =  orders.data.Where(x => x.order_id == orderNumber).FirstOrDefault();

                

                Dal.ShopHelper sh = new Dal.ShopHelper();
                Dal.ShopOrder so = sh.GetShopOrder(s, orderNumber);

 
                Dal.Order order = new Dal.Order()
                {
                    ExternalOrderNumber = orderNumber, 
                    ShipmentFirstName = shopOrder.customer.shipping_contact.Trim(),
                     
                    ShipmentLastName = "",
                    Email = shopOrder.customer.email,
                    ExternalUserId = 0,//orderEmpik.customer.customer_id,
                    InsertDate = DateTime.Parse(shopOrder.date_created),
                    OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                    //ShippintTypeId = 0,
                    ShopId = (int)s, 
                    Phone2 = shopOrder.customer.phone,
                    Phone=shopOrder.customer.shipping_phone,
                    ShipmentAddress = shopOrder.customer.shipping_street,
                    ShipmentCity = shopOrder.customer.shipping_city,
                    ShipmentCompanyName = shopOrder.customer.company,
                    ShipmentPostcode = shopOrder.customer.shipping_postal_code,
                    CompanyId = Dal.Helper.DefaultCompanyId,
                    DoNotAutoEdit = false,
                    ShippingCostVAT = 0.23M,
                    ParActive = true,
                    ShippingCost = Decimal.Parse(shopOrder.shipping_cost, System.Globalization.CultureInfo.InvariantCulture),
                    ShippingCurrencyCode = "PLN",
                    ShipmentCountryCode = "PL", //shopOrder.customer.shipping_country,
                    ShippingCurrencyRate=1,
                    ShippingAmountCurrency= Decimal.Parse(shopOrder.shipping_cost, System.Globalization.CultureInfo.InvariantCulture)

                };
                if (shopOrder.deadline_at != null)
                    order.DeliveryDate = DateTime.Parse(shopOrder.deadline_at);

       

     


                Dal.Invoice invoice = null;
                if (!String.IsNullOrEmpty(shopOrder.customer.billing_nip ))
                {
                    invoice = SetInvoice(shopOrder);
                    order.Invoice = invoice;
                }
                Dal.OrderStatusHistory osh = new Dal.OrderStatusHistory()
                {
                    InsertDate = DateTime.Now,
                    InsertUser = "System",
                    Order = order,
                    OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                    Comment = ""
                };

                if (shopOrder.status == "5")
                    osh.Comment = "Uwaga! Klient anulował zakup w marketplace. Sprawdź szczegóły";

                List<Dal.OrderProduct> products = new List<Dal.OrderProduct>();

                foreach (Product ol in shopOrder.products)
                {
                    Dal.OrderProduct op = new Dal.OrderProduct()
                    {
                        ExternalProductId = Int32.Parse(ol.product_id),
                        Order = order,
                        OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New,
                        Price = Decimal.Parse(ol.sale_price_brutto, System.Globalization.CultureInfo.InvariantCulture),
                        Quantity = Int32.Parse(ol.quantity),
                        ProductName = ol.vendor_product_name + " (" + ol.brand_code+ ")",
                        ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
                        Rebate = 0,
                        VAT = 0.23M,
                        ProductCatalogId= Dal.ShopHelper.GetProductCatalogIdByShopProductId(Dal.Helper.Shop.Lajtitpl, ol.part_number),
                        PriceCurrency = Decimal.Parse(ol.sale_price_brutto, System.Globalization.CultureInfo.InvariantCulture),
                        CurrencyRate=1
                    };
                    products.Add(op);

                }

                OrderPayment p = null;

                //if(shopOrder.payment_mode_id == "2" && shopOrder.payment_mode_id=="1")
                //{
                //    p = new OrderPayment()
                //    {
                //        Order = order,
                //        Amount = Decimal.Parse(shopOrder.customer_payment, System.Globalization.CultureInfo.InvariantCulture),
                //        Comment = "",
                //        InsertDate = DateTime.Parse(shopOrder.date_created),
                //        InsertUser = "system",
                //        PaymentTypeId = (int)Dal.Helper.OrderPaymentType.PayU23,
                //        CurrencyCode = "PLN",
                //        CurrencyRate=1,
                //        AmountCurrency = Decimal.Parse(shopOrder.customer_payment, System.Globalization.CultureInfo.InvariantCulture)
                //    };
                //}
                if (shopOrder.payment_mode_id == "3" && shopOrder.payment_status == "1")
                {
                    int accountingType;
                    if (!String.IsNullOrEmpty(shopOrder.customer.billing_nip))
                        accountingType = (int)Dal.Helper.OrderPaymentAccoutingType.Invoice;
                    else
                        accountingType = (int)Dal.Helper.OrderPaymentAccoutingType.CashRegister;

                    p = new OrderPayment()
                    {
                        Order = order,
                        Amount = Decimal.Parse(shopOrder.customer_payment, System.Globalization.CultureInfo.InvariantCulture),
                        Comment = "",
                        InsertDate = DateTime.Parse(shopOrder.date_created),
                        InsertUser = "system",
                        PaymentTypeId = (int)Dal.Helper.OrderPaymentType.Przelewy24,
                        AccountingTypeId = accountingType,
                        CurrencyCode = "PLN",
                        CurrencyRate = 1,
                        AmountCurrency = Decimal.Parse(shopOrder.customer_payment, System.Globalization.CultureInfo.InvariantCulture)
                    };
                }

                if (shopOrder.delivery_mode=="2")
                {
                    order.ShipmentAddress = "";
                    order.ShipmentCity = "";
                    order.ShipmentCompanyName = "";
                    order.ShipmentPostcode = "";
                }

    
                so.IsProcessed = true;

                Dal.OrderShipping orderShipping = SetShipping(shopOrder, ref order);
                sh.SetNewOrder(so, invoice, products, s, osh, p, orderShipping);


                return true;
            }
            private static Dal.OrderShipping SetShipping(Order shopOrder, ref Dal.Order order)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                List<Dal.ShopShipping> ss = sh.GetShopShipping(Dal.Helper.Shop.Morele);


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
                string packstation = shopOrder.pickup_point;
                if (packstation != null)
                    orderShipping.ServicePoint = packstation;


                //switch(shopOrder.delivery_mode)
                //{
                //    case "1":
                //        if (shopOrder.payment_mode_id == "1")
                //            order.ShippintTypeId = (int)Dal.Helper.ShippingType.DpdCourierCOD;
                //        else
                //            order.ShippintTypeId = (int)Dal.Helper.ShippingType.DpdCourier;
                //        break;

                //    case "2":
                //        if (shopOrder.payment_mode_id == "1")
                //            order.ShippintTypeId = (int)Dal.Helper.ShippingType.InpostLockerCOD;
                //        else
                //            order.ShippintTypeId = (int)Dal.Helper.ShippingType.InpostLocker;
                //        break;

                //};


                var q = ss.Where(x => x.ShopId == (int)Dal.Helper.Shop.Morele && x.ShopShippingId == shopOrder.delivery_mode && x.ShopPaymentId == Int32.Parse(shopOrder.payment_mode_id)).FirstOrDefault();
                orderShipping.ShippingCompanyId = q.ShippingServiceMode.ShippingCompanyId;
                orderShipping.ShippingServiceModeId = q.ShippingServiceModeId;


                if (q.PayOnDelivery)
                    orderShipping.COD = Decimal.Parse(shopOrder.order_value, System.Globalization.CultureInfo.InvariantCulture);



                return orderShipping;
            }
            private static Invoice SetInvoice(Order shopOrder)
            {
                Dal.Invoice invoice = new Invoice()
                {
                    CompanyName=shopOrder.customer.billing_name,
                    Address = shopOrder.customer.billing_street,
                    City = shopOrder.customer.billing_city,
                    Postcode = shopOrder.customer.billing_postal_code,
                    CompanyId = Dal.Helper.DefaultCompanyId,
                    Email = shopOrder.customer.email,
                    InvoiceTypeId = 2,
                    ExcludeFromInvoiceReport = false,
                    InvoiceDate = DateTime.Parse(shopOrder.date_created),
                    IsLocked = false,
                    IsDeleted = false
                }; 
                  
                if (shopOrder.customer.billing_nip != null)
                    invoice.Nip = shopOrder.customer.billing_nip;
                else
                    invoice.Nip = "";

                return invoice;
            }


            public class WayBill
            {
                public string order_id { get; set; }
                public string waybill_number { get; set; }
                public string waybill_tracking_link { get; set; }
            }
            public static void SetWayBill(List<Dal.ShopOrder> orders)
            {


                foreach (Dal.ShopOrder so in orders)
                {


                    WayBill w = new WayBill()
                    {
                        
                        order_id = so.ShopOrderNumber,
                        waybill_number = so.Order.OrderShipping.ShipmentTrackingNumber,
                        waybill_tracking_link = String.Format(so.Order.OrderShipping.ShippingCompany.TrackingUrl, so.Order.OrderShipping.ShipmentTrackingNumber)
                    };
                    HttpWebRequest request = GetHttpWebRequest("/order/waybill", "POST");

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(w);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();



                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    LajtIt.Dal.DbHelper.Orders.SetOrderTrackingNumberSent(so.OrderId.Value);


                }
            }

        }
    }
}
