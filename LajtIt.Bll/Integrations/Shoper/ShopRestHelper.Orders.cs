using LajtIt.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class ShopRestHelper
    {
        public class Orders
        {
            public class AdditionalField
            {
                public string field_id { get; set; }
                public string type { get; set; }
                public string locate { get; set; }
                public string req { get; set; }
                public string active { get; set; }
                public string order { get; set; }
                public string field_value { get; set; }
                public string value { get; set; }
            }

            public class DeliveryAddress
            {
                public string address_id { get; set; }
                public string order_id { get; set; }
                public string type { get; set; }
                public string firstname { get; set; }
                public string lastname { get; set; }
                public string company { get; set; }
                public string pesel { get; set; }
                public string city { get; set; }
                public string postcode { get; set; }
                public string street1 { get; set; }
                public string street2 { get; set; }
                public string state { get; set; }
                public string country { get; set; }
                public string phone { get; set; }
                public string country_code { get; set; }
                public string tax_identification_number { get; set; }
            }

            public class BillingAddress
            {
                public string address_id { get; set; }
                public string order_id { get; set; }
                public string type { get; set; }
                public string firstname { get; set; }
                public string lastname { get; set; }
                public string company { get; set; }
                public string pesel { get; set; }
                public string city { get; set; }
                public string postcode { get; set; }
                public string street1 { get; set; }
                public string street2 { get; set; }
                public string state { get; set; }
                public string country { get; set; }
                public string phone { get; set; }
                public string country_code { get; set; }
                public string tax_identification_number { get; set; }
            }

            public class ShippingAdditionalFields
            {
                public string machine { get; set; }
            }

            public class List
            {
                public int order_id { get; set; }
                public int? user_id { get; set; }
                public DateTime date { get; set; }
                public string status_date { get; set; }
                public string confirm_date { get; set; }
                public DateTime delivery_date { get; set; }
                public string status_id { get; set; }
                public decimal sum { get; set; }
                public int payment_id { get; set; }
                public string user_order { get; set; }
                public int shipping_id { get; set; }
                public decimal shipping_cost { get; set; }
                public string email { get; set; }
                public string delivery_code { get; set; }
                public string code { get; set; }
                public string confirm { get; set; }
                public string notes { get; set; }
                public string notes_priv { get; set; }
                public string notes_pub { get; set; }
                public int currency_id { get; set; }
                public decimal currency_rate { get; set; }
                public decimal paid { get; set; }
                public string ip_address { get; set; }
                public string discount_client { get; set; }
                public string discount_group { get; set; }
                public string discount_levels { get; set; }
                public decimal discount_code { get; set; }
                public string code_id { get; set; }
                public string lang_id { get; set; }
                public string origin { get; set; }
                public object parent_order_id { get; set; }
                public string registered { get; set; }
                public string promo_code { get; set; }
                public List<AdditionalField> additional_fields { get; set; }
                public List<object> tags { get; set; }
                public bool is_paid { get; set; }
                public bool is_overpayment { get; set; }
                public bool is_underpayment { get; set; }
                public int total_parcels { get; set; }
                public int total_products { get; set; }
                public List<object> children { get; set; }
                public int loyalty_cost { get; set; }
                public string shipping_tax_name { get; set; }
                public decimal shipping_tax_value { get; set; }
                public string shipping_tax_id { get; set; }
                public DeliveryAddress delivery_address { get; set; }
                public BillingAddress billing_address { get; set; }
                public string pickup_point { get; set; }
                public ShippingAdditionalFields shipping_additional_fields { get; set; }
            }

            public class RootOrder
            {
                public string count { get; set; }
                public int pages { get; set; }
                public int page { get; set; }
                public List<List> list { get; set; }
            }

            public static void GetOrders(Dal.Helper.Shop shop)
            {
                GetOrders(shop, 1);
            }
                private static void GetOrders(Dal.Helper.Shop shop, int page)
                {
                    try
                {
                    string url = @"/webapi/rest/orders?filters={""status_id"":{""="":""1""}}&page=" + page.ToString();

                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, url ,  "GET");

                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    RootOrder orders = json_serializer.Deserialize<RootOrder>(text);
                  


                    string[] shopOrderNumbers = orders.list.Select(x => x.order_id.ToString()).OrderByDescending(x => x).ToArray();

                    Dal.ShopHelper sh = new Dal.ShopHelper();

                    string[] shopOrdersNumbersNotSaved = sh.InsertOrders(shopOrderNumbers, null, shop, "system");


                    ProcessOrders(shop, orders, "system");

                    if (page < orders.pages)
                        GetOrders(shop, page + 1);

                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop);
               
                }
            }
            private static void ProcessOrders(Dal.Helper.Shop shop, RootOrder rootOrders, string actingUser)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

                //List<Dal.ShippingType> shippingTypes = sh.GetShippingTypes();

                List<Dal.ShopOrder> orders = sh.GetOrdersToProcess(shop);
                Payments.RootPayment rootPayments=null;

                if (orders.Count > 0)
                    rootPayments = Payments.GetPayments(shop);

                foreach (Dal.ShopOrder so in orders)
                {
                    Dal.Order order = new Dal.Order();

                    List shopOrder = rootOrders.list.Where(x => x.order_id.ToString() == so.ShopOrderNumber).FirstOrDefault();

                    if (shopOrder == null)
                        continue;

                    #region order
                    if (!String.IsNullOrEmpty(shopOrder.promo_code))
                        order.PromoCode = shopOrder.promo_code;


                    order.DeliveryDate = shopOrder.delivery_date;
                    order.PromoRebate = shopOrder.discount_code;
                    order.Email = shopOrder.email;
                    order.OrderStatusId = (int)Dal.Helper.OrderStatus.New;
                    order.ExternalUserId = shopOrder.user_id == null ? 0 : shopOrder.user_id.Value;
                    order.InsertDate = shopOrder.date;
                    order.ShippingCost = shopOrder.shipping_cost;
                    order.ShippingCostVAT = shopOrder.shipping_tax_value / 100;

                    order.ShipmentCity = shopOrder.delivery_address.city;
                    order.ShipmentCompanyName = shopOrder.delivery_address.company;
                    order.ShipmentFirstName = shopOrder.delivery_address.firstname;
                    order.ShipmentLastName = shopOrder.delivery_address.lastname;
                    order.ShipmentPostcode = shopOrder.delivery_address.postcode;
                    order.ShipmentAddress = shopOrder.delivery_address.street1;
                    if (shopOrder.delivery_address.street2 != null && shopOrder.delivery_address.street2 != "")
                        order.ShipmentAddress = order.ShipmentAddress + " " + shopOrder.delivery_address.street2;
                    order.Phone = shopOrder.delivery_address.phone;
                    order.ParActive = true;
                    order.ShopId = (int)shop; //sklep,
                    order.CompanyId = Dal.Helper.DefaultCompanyId;
                    order.ExternalOrderNumber = shopOrder.order_id.ToString();
                    order.ShipmentCountryCode = shopOrder.delivery_address.country_code;
 


                    Dal.OrderShipping orderShipping =  SetShipping(shop, shopOrder, ref order);

                    #endregion

                    #region Invoice
                    Dal.Invoice invoice = null;
                    if (!String.IsNullOrEmpty(shopOrder.billing_address.tax_identification_number))
                    {
                        invoice = new Dal.Invoice();
                        invoice.Address = shopOrder.billing_address.street1;
                        if (shopOrder.billing_address.street2 != null && shopOrder.billing_address.street2 != "")
                            invoice.Address = invoice.Address + " " + shopOrder.billing_address.street2;
                        invoice.City = shopOrder.billing_address.city;
                        invoice.CompanyName = shopOrder.billing_address.company;
                        invoice.CompanyId = Dal.Helper.DefaultCompanyId;
                        invoice.Email = order.Email;
                        invoice.InvoiceDate = order.InsertDate;
                        invoice.InvoiceTypeId = 2;
                        invoice.Nip = shopOrder.billing_address.tax_identification_number;
                        invoice.Postcode = shopOrder.billing_address.postcode;

                        order.Invoice = invoice;

                    }
                    #endregion

                    #region products


                    OrdersProducts.RootOrderProduct orderProducts = OrdersProducts.GetOrderProducts(shop, shopOrder.order_id);

                    List<Dal.OrderProduct> products = new List<Dal.OrderProduct>();
                    List<Dal.ProductCatalogShopProduct> productCatalogs = Dal.DbHelper.ProductCatalog.GetProductCatalogShopProductByShopIds(shop,
                        orderProducts.list.Select(x => x.product_id.ToString()).ToArray()
                        )
                        .ToList();
                    foreach (OrdersProducts.OrderProduct product in orderProducts.list)
                    {
                        Dal.OrderProduct p = new Dal.OrderProduct()
                        {
                            Comment = String.Format("{0}", product.option),
                            ExternalProductId = product.product_id,
                            Order = order,
                            Price = product.price,
                            ProductName = product.name,
                            Quantity = product.quantity,
                            VAT = Convert.ToDecimal(product.tax_value / 100),
                            ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
                            Rebate = product.discount_perc,
                            OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New,
                            CurrencyRate = shopOrder.currency_rate,
                            PriceCurrency = product.price
                        };
                        Dal.ProductCatalogShopProduct pc = productCatalogs.Where(x => Int32.Parse(x.ShopProductId) == product.product_id).FirstOrDefault();
                        if (pc != null)
                        {
                            p.ProductCatalogId = pc.ProductCatalogId;
                        }
                        products.Add(p);


                    }

                    #endregion

                    #region payment
                    Dal.OrderPayment orderPayment = null;
                    
                    if (orderShipping.COD.HasValue == true && orderShipping.COD.Value > 0 && shopOrder.paid > 0)
                    {
                        orderPayment = new OrderPayment()
                        {
                            Order = order,
                            Amount = shopOrder.paid,
                            InsertDate = DateTime.Now,
                            InsertUser = "system",
                            PaymentTypeId = (int)Dal.Helper.OrderPaymentType.Przelewy24,
                            CurrencyCode = GetShopCurrencyCode(shop, shopOrder.currency_id).CurrencyCode,
                            CurrencyRate = shopOrder.currency_rate,
                            AmountCurrency = shopOrder.paid

                        };
                    }

                    if (orderShipping.COD.HasValue && shopOrder.paid == 0)
                    {
                        order.DeliveryDate = null;
                        TimeSpan span = shopOrder.delivery_date.Subtract(shopOrder.date);
                        order.DeliveryDays = (int)span.TotalDays;

                    }
                        #endregion


                        string comment = String.Format("Zamówienie: {0}, \nPłatność: {2}\nUwagi do zamówienia: \n{1}",
                   so.ShopOrderNumber,
                   shopOrder.notes,
                   rootPayments.list.Where(x=>x.payment_id == shopOrder.payment_id).Select(x=>x.name).FirstOrDefault()
                   );


                    Dal.OrderStatusHistory orderStatusHistory = new Dal.OrderStatusHistory()
                    {
                        Comment = comment,
                        InsertDate = DateTime.Now,
                        InsertUser = actingUser,
                        Order = order,
                        OrderStatusId = (int)Dal.Helper.OrderStatus.New
                    };
                    so.IsProcessed = true;
                    sh.SetNewOrder(so, invoice, products, shop, orderStatusHistory, orderPayment, orderShipping);

                }


            }
            public static void GetOrderPayments(Dal.Helper.Shop shop)
            {
                GetOrderPayments(shop, 1);
            }
            private static void GetOrderPayments(Dal.Helper.Shop shop, int pageId)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();

                List<Dal.ShopOrder> orders = sh.GetShopOrdersWithoutPayment(shop);

                string[] orderIds = orders.Select(x => x.ShopOrderNumber).ToArray();

                string orderIdss =   String.Join(",", orderIds);

                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, @"/webapi/rest/orders?limit=50&page="+pageId.ToString()+@"&filters={""order_id"":{""IN"":[" + orderIdss + @"]}}", "GET");

                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    RootOrder rootOrder = json_serializer.Deserialize<RootOrder>(text);


                    ProcessOrdersForPayment(shop, rootOrder);

                    if (rootOrder.page < rootOrder.pages)
                        GetOrderPayments(shop, pageId + 1);

                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop);

                }





            }

            private static void ProcessOrdersForPayment(Dal.Helper.Shop shop, RootOrder rootOrder)
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();


                foreach (List so in rootOrder.list)
                {
                    if (so.is_paid)
                    {
                        Dal.ShopOrder sho = sh.GetShopOrder(shop, so.order_id.ToString());
                        OrderPayment orderPayment = new OrderPayment()
                        {
                            OrderId = sho.OrderId.Value,
                            Amount = so.paid,
                            InsertDate = DateTime.Now,
                            InsertUser = "system",
                            PaymentTypeId = (int)Dal.Helper.OrderPaymentType.Przelewy24,
                            CurrencyCode = GetShopCurrencyCode(shop, so.currency_id).CurrencyCode,
                            CurrencyRate = so.currency_rate,
                            AmountCurrency = so.paid
                        };

                    
                        TimeSpan span = so.delivery_date.Subtract(so.date);
                        int deliveryDays = (int)span.TotalDays;


                        sh.SetShopOrderPayment(shop, sho.OrderId.Value, orderPayment, so.delivery_date, deliveryDays);
                    }

                }
            }
            private static Dal.OrderShipping   SetShipping(Dal.Helper.Shop shop, List shopOrder, ref Dal.Order order )
            {
                Dal.ShopHelper sh = new Dal.ShopHelper();
                List<Dal.ShopShipping> ss = sh.GetShopShipping(shop);

                order.ShippingCurrencyCode = GetShopCurrencyCode(shop, shopOrder.currency_id).CurrencyCode;
                order.ShippingCurrencyRate = shopOrder.currency_rate;
                order.ShippingAmountCurrency = shopOrder.shipping_cost;

                Dal.OrderHelper oh = new Dal.OrderHelper();
                

                Dal.ShippingCompany sc = oh.GetShipppingCompanies().Where(x => x.IsDefault ).FirstOrDefault();
                Dal.OrderShipping orderShipping = new OrderShipping()
                {
                    Order1 = order,
                    COD = null,
                    InsertDate = DateTime.Now,
                    InsertUser = "System",
                    OrderShippingStatusId = (int)Dal.Helper.OrderShippingStatus.Temporary,                   
                    ShippingCompanyId = sc.ShippingCompanyId,
                    ShippingServiceTypeId = (int)Dal.Helper.ShippingServiceType.ForOrder,
                    TrackingNumberSent=false,
                    IsParcelReady=false

                };
                 
                if (shopOrder.shipping_additional_fields != null)
                    orderShipping.ServicePoint = shopOrder.shipping_additional_fields.machine;
                Dal.ShopShipping shopShipping = 
                    ss.Where(x => x.ShopPaymentId == shopOrder.payment_id && x.ShopShippingId == shopOrder.shipping_id.ToString())                     
                    .FirstOrDefault();

                orderShipping.ShippingServiceModeId = shopShipping.ShippingServiceModeId;
                orderShipping.ShippingCompanyId = shopShipping.ShippingServiceMode.ShippingCompany.ShippingCompanyId;

                if (shopShipping.PayOnDelivery)
                    orderShipping.COD = shopOrder.sum;

                return orderShipping;
            }

            private static ShopCurrency GetShopCurrencyCode(Dal.Helper.Shop shop, int currency_id)
            {
                Dal.ShopCurrency sc = Dal.DbHelper.Shop.GetShopCurrency(shop).Where(x => x.ShopCurrencyId == currency_id).FirstOrDefault() ;

                return sc;
            }
        }
    }
}