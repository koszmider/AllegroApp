using LajtIt.Dal;
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

namespace LajtIt.Bll
{
    public partial  class EmpikRESTHelper
    {
        public class Orders
        { 
        #region Orders
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class BillingAddress
        {
            public string city { get; set; }
            public string company { get; set; }
            public string country { get; set; }
            public object country_iso_code { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string phone { get; set; }
            public object state { get; set; }
            public string street_1 { get; set; }
            public object street_2 { get; set; }
            public string zip_code { get; set; }

        }

        public class ShippingAddress
        {
            public object additional_info { get; set; }
            public string city { get; set; }
            public string company { get; set; }
            public string country { get; set; }
            public string country_iso_code { get; set; }
            public string lastname { get; set; }
            public object state { get; set; }
            public string street_1 { get; set; }
            public object street_2 { get; set; }
            public string zip_code { get; set; }
            public string firstname { get; set; }
            public string phone { get; set; }

        }

        public class Customer
        {
            public BillingAddress billing_address { get; set; }
            public object civility { get; set; }
            public string customer_id { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public object locale { get; set; }
            public ShippingAddress shipping_address { get; set; }

        }

        public class Center
        {
            public string code { get; set; }

        }

        public class Fulfillment
        {
            public Center center { get; set; }

        }

        public class OrderAdditionalField
        {
            public string code { get; set; }
            public string type { get; set; }
            public string value { get; set; }

        }

        public class CommissionTax
        {
            public double amount { get; set; }
            public string code { get; set; }

        }

        public class Cancelation
        {
            public double amount { get; set; }
            public double commission_amount { get; set; }
            public List<CommissionTax> commission_taxes { get; set; }
            public double commission_total_amount { get; set; }
            public DateTime created_date { get; set; }
            public string id { get; set; }
            public int quantity { get; set; }
            public double shipping_amount { get; set; }
            public List<object> shipping_taxes { get; set; }
            public List<object> taxes { get; set; }

        }

        public class CommissionTax2
        {
            public double amount { get; set; }
            public string code { get; set; }
            public double rate { get; set; }

        }

        public class OrderLine
        {
            public bool can_refund { get; set; }
            public List<Cancelation> cancelations { get; set; }
            public string category_code { get; set; }
            public string category_label { get; set; }
            public double commission_fee { get; set; }
            public double commission_rate_vat { get; set; }
            public List<CommissionTax2> commission_taxes { get; set; }
            public double commission_vat { get; set; }
            public DateTime created_date { get; set; }
            public DateTime? debited_date { get; set; }
            public object description { get; set; }
            public DateTime last_updated_date { get; set; }
            public int offer_id { get; set; }
            public string offer_sku { get; set; }
            public string offer_state_code { get; set; }
            public List<object> order_line_additional_fields { get; set; }
            public string order_line_id { get; set; }
            public int order_line_index { get; set; }
            public string order_line_state { get; set; }
            public string order_line_state_reason_code { get; set; }
            public string order_line_state_reason_label { get; set; }
            public double price { get; set; }
            public object price_additional_info { get; set; }
            public double price_unit { get; set; }
            public List<object> product_medias { get; set; }
            public string product_sku { get; set; }
            public string product_title { get; set; }
            public List<object> promotions { get; set; }
            public int quantity { get; set; }
            public object received_date { get; set; }
            public List<object> refunds { get; set; }
            public DateTime? shipped_date { get; set; }
            public double shipping_price { get; set; }
            public object shipping_price_additional_unit { get; set; }
            public object shipping_price_unit { get; set; }
            public List<object> shipping_taxes { get; set; }
            public List<object> taxes { get; set; }
            public double total_commission { get; set; }
            public double total_price { get; set; }

        }

        public class Promotions
        {
            public List<object> applied_promotions { get; set; }
            public int total_deduced_amount { get; set; }

        }

        public class Order
        {
            public DateTime? acceptance_decision_date { get; set; }
            public bool can_cancel { get; set; }
            public bool can_shop_ship { get; set; }
            public object channel { get; set; }
            public string commercial_id { get; set; }
            public DateTime created_date { get; set; }
            public string currency_iso_code { get; set; }
            public Customer customer { get; set; }
            public DateTime? customer_debited_date { get; set; }
            public object delivery_date { get; set; }
            public Fulfillment fulfillment { get; set; }
            public bool has_customer_message { get; set; }
            public bool has_incident { get; set; }
            public bool has_invoice { get; set; }
            public DateTime last_updated_date { get; set; }
            public int leadtime_to_ship { get; set; }
            public List<OrderAdditionalField> order_additional_fields { get; set; }
            public string order_id { get; set; }
            public List<OrderLine> order_lines { get; set; }
            public string order_state { get; set; }
            public string order_state_reason_code { get; set; }
            public string order_state_reason_label { get; set; }
            public string paymentType { get; set; }
            public string payment_type { get; set; }
            public string payment_workflow { get; set; }
            public double price { get; set; }
            public Promotions promotions { get; set; }
            public object quote_id { get; set; }
            public string shipping_carrier_code { get; set; }
            public string shipping_company { get; set; }
            public DateTime shipping_deadline { get; set; }
            public double shipping_price { get; set; }
            public string shipping_tracking { get; set; }
            public string shipping_tracking_url { get; set; }
            public string shipping_type_code { get; set; }
            public string shipping_type_label { get; set; }
            public string shipping_zone_code { get; set; }
            public string shipping_zone_label { get; set; }
            public double total_commission { get; set; }
            public double total_price { get; set; }
            public DateTime? transaction_date { get; set; }
            public string transaction_number { get; set; }

        }

  

        public class OrderRoot
        {
            public List<Order> orders { get; set; }
            public int total_count { get; set; }

        }


        #endregion

        #region OrderAccept

        public class OrderLines
        {

            public List<OrderLine> order_lines { get; set; }
            public class OrderLine
            {
                public string id { get; set; }
                public bool accepted { get; set; }
            }
        }
        #endregion

        #region Shipping
        public class Shipping
        {
            public string carrier_code { get; set; }
            public string carrier_name { get; set; }
            public string carrier_url { get; set; }
            public string tracking_number { get; set; }
        }
        #endregion

        #region REST

        public static void SetShipment()
        {


                if (Dal.Helper.Env != Dal.Helper.EnvirotmentEnum.Prod)
                    return;



                Dal.ShopHelper shh = new Dal.ShopHelper();
                List<Dal.ShopOrder> orders = shh.GetOrderTransactionNumber(Dal.Helper.Shop.Empik);

                foreach (Dal.ShopOrder order in orders)
                {
                    try
                    {


                        Shipping sh = new Shipping();
                        switch (order.Order.OrderShipping.ShippingCompanyId)
                        {
                            case (int)Dal.Helper.ShippingCompany.Dpd:
                                sh.carrier_code = "dpd"; break;
                            case (int)Dal.Helper.ShippingCompany.InPost:
                                sh.carrier_code = "paczkomatyinpost"; break;
                            case (int)Dal.Helper.ShippingCompany.DHL:
                                sh.carrier_code = "dpd"; break;
                            case (int)Dal.Helper.ShippingCompany.UPS:
                                sh.carrier_code = "ups"; break;
                            case (int)Dal.Helper.ShippingCompany.GLS:
                                sh.carrier_code = "gls"; break;
                            case (int)Dal.Helper.ShippingCompany.FedEX:
                                sh.carrier_code = "fedex"; break;
                            case (int)Dal.Helper.ShippingCompany.InPostKurier:
                                sh.carrier_code = "inpostpaczkakurierska"; break;
                        }

                        sh.tracking_number = order.Order.OrderShipping.ShipmentTrackingNumber;

                        HttpWebRequest request = GetHttpWebRequest(String.Format("/api/orders/{0}/tracking", order.ShopOrderNumber), "PUT");
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

                        LajtIt.Dal.DbHelper.Orders.SetOrderTrackingNumberSent(order.OrderId.Value);
                    }
                    catch (WebException ex)
                    {
                        using (WebResponse response = ex.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            if (httpResponse == null)
                            {
                                Bll.ErrorHandler.SendError(ex, String.Format("Błąd wysyłania numeru przesyłki do Empik orderId: {0} msg {1}", order.OrderId, ex.Message));


                            }
                            //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string text = reader.ReadToEnd();


                                Bll.ErrorHandler.SendError(ex, String.Format("Błąd wysyłania numeru przesyłki do Empik orderId: {0} msg: {1}", order.OrderId, text));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd wysyłania numeru przesyłki do Empik orderId: {0} msg {1}", order.OrderId, ex.Message));
                    }
                }
            }
        public static bool SetOrderLineAccept(int orderId, int[] orderProductIds)
        {

            Dal.ShopHelper sh = new Dal.ShopHelper();
            Dal.ShopOrder so = sh.GetShopOrder(Dal.Helper.Shop.Empik, orderId);

            string orderNumber = so.ShopOrderNumber;

            HttpWebRequest request = GetHttpWebRequest(String.Format("/api/orders/{0}/accept", orderNumber), "PUT");


            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<Dal.OrderProductsView> orderProducts = oh.GetOrderProducts(orderId).Where(x=>x.IsOrderProduct==1).ToList();
                try
                {
                    OrderLines ols = new OrderLines();
                    ols.order_lines = orderProducts.Select(x => new OrderLines.OrderLine()
                    {
                        accepted = orderProductIds.Contains(x.OrderProductId),
                        id = String.Format("{0}-{1}", orderNumber, x.ExternalProductId)
                    }).ToList();

                    Stream dataStream = request.GetRequestStream();
                    string jsonEncodedParams = Bll.RESTHelper.ToJson(ols);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    WebResponse webResponse = request.GetResponse();

                    Stream responseStream = webResponse.GetResponseStream();

                    StreamReader reader = new StreamReader(responseStream);

                    string text = reader.ReadToEnd();

                    return true;
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
                        else
                        {
                            Console.WriteLine("Error code: {0}", httpResponse.StatusCode);

                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string text = reader.ReadToEnd(); 

                                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania zamówienia {0}, {1}", orderId, text));
                            }
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania zamówienia {0}, {1}", orderId, ex.Message));
                    return false;
                }
        }
        public static bool SetSentStatus(string shopOrderNumber)
        {
            if (Dal.Helper.Env != Dal.Helper.EnvirotmentEnum.Prod)
                return false;

            try
            {

                HttpWebRequest request = GetHttpWebRequest(String.Format("/api/orders/{0}/ship", shopOrderNumber), "PUT");

                WebResponse webResponse = request.GetResponse();

                Stream responseStream = webResponse.GetResponseStream();

                StreamReader reader = new StreamReader(responseStream);

                string text = reader.ReadToEnd();
                    return true;
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse == null)
                    {
                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd oznaczania przesyłki jako wysłana do Empik {0} msg {1}", shopOrderNumber, ex.Message));


                    }
                    //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();


                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd oznaczania przesyłki jako wysłana do Empik {0} msg {1}", shopOrderNumber, text));
                    }
                }
                    return false;
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd oznaczania przesyłki jako wysłana do Empik {0} msg {1}", shopOrderNumber, ex.Message));

                    return false;
                }
        }
        private static void GetOrdersInternal()
        {
            HttpWebRequest request = GetHttpWebRequest("/api/orders?limit=50&sort=dateCreated&order=desc", "GET");

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
                OrderRoot ordersRoot = json_serializer.Deserialize<OrderRoot>(text);

                SetOrders(ordersRoot);
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


                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania zamówienia {0}, {1}", 1, text));
                    }
                }
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania zamówienia {0}, {1}", 1, ex.Message));
            }
        }

        public static OrderRoot GetOrder(string orderNumber)
        {
            HttpWebRequest request = GetHttpWebRequest(String.Format("/api/orders?order_ids={0}", orderNumber), "GET");

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
                OrderRoot ordersRoot = json_serializer.Deserialize<OrderRoot>(text);

                return ordersRoot;
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


                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania zamówienia {0}, {1}", 1, text));
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania zamówienia {0}, {1}", 1, ex.Message));
                return null;
            }
        }
        #endregion

        public static void GetOrders()
        {
            GetOrdersInternal();
            GetOrdersProcessing();
            SetShipment();
            Bll.ShopHelper sh = new ShopHelper();
                if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Prod)
                    sh.SetOrderStatuses(Dal.Helper.Shop.Empik);
        }

        private static void GetOrdersProcessing()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<Dal.Order> orders = oh.GetOrdersByStatus(Dal.Helper.OrderStatus.WaitingForAcceptance).Where(x=>x.ShopId==(int)Dal.Helper.Shop.Empik).ToList();


            foreach (Dal.Order order in orders)
            {
                CheckOrder(order);
            }


            orders = oh.GetOrdersDeletedForEmpik();


            Dal.ShopHelper so = new Dal.ShopHelper();

            foreach (Dal.Order order in orders)
            {
                SetOrderLineAccept(order.OrderId, new int[] { });

                so.SetShopOrderStatus(order.OrderId, "REFUSED");
            }
        }

        private static void CheckOrder(Dal.Order order)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            OrderRoot or = GetOrder(order.ExternalOrderNumber);


            Order empikOrder = or.orders[0];

            int count = empikOrder.order_lines.Count();
            int countRefused = empikOrder.order_lines.Where(x => x.order_line_state == "REFUSED" ).Count();

            if(count==countRefused)
            {
                // ustaw status usuniety

                Dal.OrderStatusHistory osh = new OrderStatusHistory()
                {
                    Comment = "Automatyczne usunięcie zamówienia",
                    InsertDate = DateTime.Now,
                    InsertUser = "system",
                    OrderId = order.OrderId,
                    OrderStatusId = (int)Dal.Helper.OrderStatus.Deleted,
                    SendNotification = null
                };
                oh.SetOrderStatus(osh, null);
                return;
            }


            int countShipping = empikOrder.order_lines.Where(x => x.order_line_state == "SHIPPING").Count();


            if (countShipping>0)
            {
                order.DeliveryDate = DateTime.Parse(empikOrder.shipping_deadline.ToString());

                SetShippingAddress(order, empikOrder.customer.shipping_address);
                SetInvoice(order, empikOrder.customer.billing_address, empikOrder);

                oh.SetOrderUpdateFromEmpik(order);

                Dal.OrderStatusHistory osh = new OrderStatusHistory()
                {
                    Comment = "Automatyczne potwierdzenie zamówienia",
                    InsertDate = DateTime.Now,
                    InsertUser = "system",
                    OrderId = order.OrderId,
                    OrderStatusId = (int)Dal.Helper.OrderStatus.WaitingForPayment,
                    SendNotification = null
                };
                oh.SetOrderStatus(osh, null);
                 


                if (empikOrder.payment_workflow == "PAY_ON_ACCEPTANCE")
                {
                    Dal.OrderPayment payment = new OrderPayment()
                    {
                        Amount = (decimal)empikOrder.total_price,
                        Comment = empikOrder.payment_type,
                        InsertDate = empikOrder.transaction_date.Value,
                        InsertUser = "System",
                        ExternalPaymentId = empikOrder.transaction_number,
                        OrderId = order.OrderId,
                        PaymentTypeId = (int)Dal.Helper.OrderPaymentType.PayU23,
                        CurrencyCode="PLN",
                        CurrencyRate=1, 
                        AmountCurrency= (decimal)empikOrder.total_price
                    };
                    oh.SetOrderPayment(payment, "system");
                }
            } 

        }

       
        private static void SetOrders(OrderRoot ordersRoot)
        { 

            Dal.ShopHelper sh = new Dal.ShopHelper();

            string[] shopOrderNumbers = ordersRoot.orders.Select(x => x.order_id).ToArray();

            string[] orderToProcess = sh.InsertOrders(shopOrderNumbers, "WAITING_ACCEPTANCE", Dal.Helper.Shop.Empik, "system");


            ProcessOrders(orderToProcess);
        }

        private static void ProcessOrders(string[] orderToProcess)
        {
            foreach (string orderNumber in orderToProcess)
            {
                bool result = CreateOrder(orderNumber);



            }
        }

            private static bool CreateOrder(string orderNumber)
            {
                OrderRoot orderRoot = GetOrder(orderNumber);
                if (orderRoot.orders == null || orderRoot.orders.Count != 1)
                    return false;

                Dal.ShopHelper sh = new Dal.ShopHelper();
                Dal.ShopOrder so = sh.GetShopOrder(Dal.Helper.Shop.Empik, orderNumber);


                Order orderEmpik = orderRoot.orders[0];

                Dal.Order order = new Dal.Order()
                {
                    ExternalOrderNumber = orderNumber,
                    ShipmentFirstName = orderEmpik.customer.firstname.Trim(),
                    ShipmentLastName = orderEmpik.customer.lastname.Trim(),
                    Email = orderEmpik.order_additional_fields.Where(x => x.code == "customer-email").Select(x => x.value).FirstOrDefault(),
                    ExternalUserId = 0,//orderEmpik.customer.customer_id,
                    InsertDate = orderEmpik.created_date,
                    OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                    //ShippintTypeId=0,
                    ShopId = (int)Dal.Helper.Shop.Empik,

                    Phone = "",
                    ShipmentAddress = "",
                    ShipmentCity = "",
                    ShipmentCompanyName = "",
                    ShipmentPostcode = "",
                    CompanyId = Dal.Helper.DefaultCompanyId,
                    DoNotAutoEdit = false,
                    ShippingCostVAT = 0.23M,
                    ParActive = true,
                    ShippingCost = Decimal.Parse(orderEmpik.shipping_price.ToString()),
                    ShippingAmountCurrency = Decimal.Parse(orderEmpik.shipping_price.ToString()),
                    ShippingCurrencyCode = orderEmpik.currency_iso_code,
                    ShipmentCountryCode = "PL",
                    ShippingCurrencyRate = 1

                };
                if (orderEmpik.delivery_date != null)
                    order.DeliveryDate = DateTime.Parse(orderEmpik.delivery_date.ToString());

                if (orderEmpik.customer.shipping_address != null)
                    SetShippingAddress(order, orderEmpik.customer.shipping_address);
                Dal.Invoice invoice = null;
                if (orderEmpik.customer.billing_address != null)
                {
                    invoice = SetInvoice(order, orderEmpik.customer.billing_address, orderEmpik);
                    //order.Invoice = invoice;
                }
                Dal.OrderStatusHistory osh = new Dal.OrderStatusHistory()
                {
                    InsertDate = DateTime.Now,
                    InsertUser = "System",
                    Order = order,
                    OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                    Comment = "Sprawdź dostępność i zdecyduj o przyjęciu zamówienia"
                };

                List<Dal.OrderProduct> products = new List<Dal.OrderProduct>();

                foreach (OrderLine ol in orderEmpik.order_lines)
                {
                    Dal.OrderProduct op = new Dal.OrderProduct()
                    {
                        ExternalProductId = ol.order_line_index,
                        Order = order,
                        OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New,
                        Price = Decimal.Parse(ol.price_unit.ToString()),
                        Quantity = ol.quantity,
                        ProductName = ol.product_title + " (" + ol.offer_sku + ")",
                        ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
                        Rebate = 0,
                        VAT = 0.23M,
                        CurrencyRate = 1,
                        PriceCurrency = Decimal.Parse(ol.price_unit.ToString()),
                        ProductCatalogId = Dal.ShopHelper.GetProductCatalogIdByShopProductId(Dal.Helper.Shop.Empik, ol.product_sku)
                    };
                    products.Add(op);

                }
                so.IsProcessed = true;


                Dal.OrderShipping orderShipping = SetShipping(orderEmpik, ref order);
                sh.SetNewOrder(so, invoice, products, Dal.Helper.Shop.Empik, osh, null, orderShipping);


                return true;
            }
            private static Dal.OrderShipping SetShipping(Order orderEmpik, ref Dal.Order order)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                List<Dal.ShopShipping> ss = sh.GetShopShipping(Dal.Helper.Shop.Empik);


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
                string packstation = orderEmpik.order_additional_fields.Where(x => x.code == "delivery-point-name").Select(x => x.value).FirstOrDefault();

                if (packstation != null)
                    orderShipping.ServicePoint = packstation;

                var q = ss.Where(x => x.ShopId == (int)Dal.Helper.Shop.Empik && x.ShopShippingId == orderEmpik.shipping_type_code).FirstOrDefault();
                orderShipping.ShippingCompanyId = q.ShippingServiceMode.ShippingCompanyId;
                orderShipping.ShippingServiceModeId = q.ShippingServiceModeId;


                if (q.PayOnDelivery)
                    orderShipping.COD = (decimal)orderEmpik.total_price;



                return orderShipping;
            }
            private static Invoice SetInvoice(Dal.Order order, BillingAddress billing_address, Order orderEmpik)
        {
            Dal.Invoice invoice = new Invoice()
            {
                Address = billing_address.street_1 + " " + billing_address.street_2,
                City = billing_address.city,
                Postcode = billing_address.zip_code,
                CompanyId = Dal.Helper.DefaultCompanyId,
                Email = order.Email,
                InvoiceTypeId = 2,
                ExcludeFromInvoiceReport = false,
                InvoiceDate = order.InsertDate,
                IsLocked = false,
                IsDeleted = false 
            };
            order.Phone = billing_address.phone;

            if (billing_address.company != null)
                invoice.CompanyName = billing_address.company;
            else
                invoice.CompanyName = String.Format("{0} {1}", billing_address.firstname, billing_address.lastname);

            string nip = orderEmpik.order_additional_fields.Where(x => x.code == "nip").Select(x => x.value).FirstOrDefault();
            if (nip != null)
                invoice.Nip = nip;
            else
                invoice.Nip = "";

            return invoice;
            
        }

            private static void SetShippingAddress(Dal.Order order, ShippingAddress shipping_address)
            {
                order.ShipmentAddress = String.Format("{0} {1}", shipping_address.street_1, shipping_address.street_2);
                order.ShipmentCity = shipping_address.city ?? "";
                order.ShipmentCompanyName = shipping_address.company ?? "";
                order.ShipmentFirstName = shipping_address.firstname ?? "";
                order.ShipmentLastName = shipping_address.lastname ?? "";
                order.ShipmentPostcode = shipping_address.zip_code ?? "";
                order.ShipmentCountryCode = shipping_address.country_iso_code ?? "PL";

            }

       
        

    }
}
}