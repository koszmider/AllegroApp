using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using LajtIt.Dal;
using System.Linq;
using Newtonsoft.Json;

namespace LajtIt.Bll
{
    public partial class ClipperonRestHelper
    {
        public class Orders
        {
            public class AddressRaw
            {
                public string Line1 { get; set; }
                public string Line2 { get; set; }
                public string Line3 { get; set; }
            }

            public class Item
            {
                public int Product_Ext_Id { get; set; }
                public string Product_Exts_Id { get; set; }
                public string Ean { get; set; }
                public string Asin { get; set; }
                public double Quantity { get; set; }
                public double Item_Price { get; set; }
                public double Currency { get; set; }
                public string Currency_Code { get; set; }
                public string Sku { get; set; }
                public double Tax_Rate { get; set; }
                public string Name { get; set; }
                public string Name_From_Platform { get; set; }
            }

            public class TaxInfo
            {
                public string Id { get; set; }
                public string Type { get; set; }
                public string Country { get; set; }
            }

            public class AddressData
            {
                public string Name { get; set; }
                public string Address { get; set; }
                public AddressRaw Address_Raw { get; set; }
                public string City { get; set; }
                public string State { get; set; }
                public string Code { get; set; }
                public string Country_Code { get; set; }
                public bool Data_Complete { get; set; }
            }

            public class BillingData
            {
                public TaxInfo Tax_Info { get; set; }
                public AddressData Address_Data { get; set; }
            }

            public class Datum
            {
                public DateTime Purchase_Date { get; set; }
                public string Currency_Code { get; set; }
                public double Currency { get; set; }
                public string Currency_Source { get; set; }
                public DateTime Currency_Date { get; set; }
                public string Ship_Country_Code { get; set; }
                public DateTime Ship_Deadline { get; set; }
                public string Market_Name { get; set; }
                public string Ship_Name { get; set; }
                public string Ship_Address { get; set; }
                public AddressRaw Address_Raw { get; set; }
                public string Ship_City { get; set; }
                public double Amount { get; set; }
                public double Ship_Amount { get; set; }
                public string Customer_Name { get; set; }
                public string Customer_Email { get; set; }
                public string Ship_Code { get; set; }
                public string Phone { get; set; }
                public string Order_Status { get; set; }
                public string Shipment_Status { get; set; }
                public DateTime Updated_Date { get; set; }
                public string Shipment_Service_Level { get; set; }
                public bool Is_Bussines { get; set; }
                public string Fulfillment_Channel { get; set; }
                public List<Item> Items { get; set; }
                public BillingData Billing_Data { get; set; }
                public string Order_Id { get; set; }
                public bool Pcm_Send { get; set; }
                public string Order_Ext_Id { get; set; }
                public string Tracking_Nos { get; set; }
                public string Ship_Company_Name { get; set; }
                public DateTime Shipment_Sended { get; set; }
            }

            public class Paging
            {
                public int AllRecordsCount { get; set; }
                public int PageNo { get; set; }
                public int PageSize { get; set; }
            }

            public class RootOrders
            {
                public List<Datum> Data { get; set; }
                public Paging Paging { get; set; }
            }
            public class Filter
            {
                public string Field { get; set; }
                public string FilterType { get; set; }
                public string Value_1 { get; set; }
            }

            public class RootOrdersSearch
            {
                public List<Filter> Filters { get; set; }
                public List<Datum> Data { get; set; }
                public Paging Paging { get; set; }
            }

            public static void GetOrders(Dal.Helper.Shop shop)
            {
                try
                {

                    HttpWebRequest request = ClipperonRestHelper.GetHttpWebRequest(shop, "/api/Orders", "GET");

                    string text = ShopRestHelper.GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();


                    RootOrders shopOrders = json_serializer.Deserialize<RootOrders>(text);

                    if (shopOrders.Data.Count() == 0)
                        return;


                    Dal.ShopHelper sh = new Dal.ShopHelper();


                    string[] shopOrderNumbers = shopOrders.Data.Select(x => x.Order_Id).ToArray();   //..d.results.Select(x => x.DisplayedOrderId).ToArray();

                    string[] orderToProcess = sh.InsertOrders(shopOrderNumbers, null, shop, "system");

                    int count = shopOrders.Paging.AllRecordsCount;

                    ProcessOrders(shop, orderToProcess);
                }

                catch (WebException ex)
                {
                    ShopRestHelper.ProcessException(ex, shop);

                }
            }
            private static void ProcessOrders(Dal.Helper.Shop shop,string[] orderToProcess)
            {
                foreach (string orderNumber in orderToProcess)
                {
                    bool result = CreateOrder(shop, orderNumber);



                }
            }

            public class Filters
            {
                public string Field { get; set; }
                public string FilterType { get; set; }
                public string Value_1 { get; set; }

            }
            private static bool CreateOrder(Dal.Helper.Shop shop,string orderNumber)
            {
                RootOrdersSearch shopOrder =  GetOrderFromClipperon(shop, orderNumber);
                if (shopOrder.Data.Count() == 0)
                    return false;


                Datum o = shopOrder.Data[0];
           
                Dal.Order order = new Order()
                {
                    ShopId = (int)shop,
                    
                    OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                    InsertDate = o.Purchase_Date,
                    Email = o.Customer_Email,
                    Phone = o.Phone,
                    ShipmentPostcode = o.Ship_Code.Trim(),
                    ShipmentAddress = o.Ship_Address.Trim(),
                    ShipmentCity = o.Ship_City.Trim(),
                    ShipmentCountryCode = o.Ship_Country_Code,
                    ShipmentCompanyName = o.Ship_Company_Name.Trim(),
                    ShipmentFirstName = o.Ship_Name.Trim(),
                    ShipmentLastName="",
                    ShippingCost = (decimal)( o.Ship_Amount / o.Items[0].Currency  ),
                    ShippingAmountCurrency = (decimal) o.Ship_Amount,
                    //ShippintTypeId = (int)Dal.Helper.ShippingType.International,
                    ShippingCurrencyRate = (decimal)o.Items[0].Currency,
                    ShippingCurrencyCode = o.Currency_Code,
                    CompanyId = Dal.Helper.DefaultCompanyId,
                    ExternalOrderNumber = o.Order_Id,
                    DeliveryDate = o.Ship_Deadline,
                    ShippingCostVAT = (decimal)(o.Items[0].Tax_Rate / 100.00),
                };

                List<Dal.OrderProduct> products = new List<OrderProduct>();
                decimal rate = 1;



                foreach(Item item in o.Items )
                {
                    Dal.OrderProduct op = new OrderProduct()
                    {
                        Order = order,
                        Comment = "",
                        CurrencyRate = (decimal)item.Currency,
                        OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New,
                        ProductCatalogId = item.Product_Ext_Id,
                        Price = (decimal)item.Item_Price / (decimal)item.Currency,
                        VAT = (decimal)(item.Tax_Rate/ 100.00),
                        Quantity = (int)item.Quantity,
                        Rebate = 0,
                        ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
                        PriceCurrency = (decimal)item.Item_Price,
                        ProductName = item.Name,
                        
                    };
                    rate = (decimal)item.Currency;
                    products.Add(op);
                }

                order.ShippingCurrencyRate = rate;


                Dal.OrderStatusHistory osh = new OrderStatusHistory()
                {
                    Comment = o.Market_Name,
                    InsertDate = DateTime.Now,
                    InsertUser = "system",
                    Order = order,
                    OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                    SendNotification = null
                };
                Dal.ShopHelper sh = new Dal.ShopHelper();
                Dal.Invoice invoice = null;

                if(o.Billing_Data!=null && o.Billing_Data.Tax_Info!=null)
                {
                    invoice = new Invoice()
                    {
                        Address = o.Billing_Data.Address_Data.Address.Trim(),
                        City =   o.Billing_Data.Address_Data.City,
                        CompanyId = Dal.Helper.DefaultCompanyId,
                        CompanyName =   o.Billing_Data.Address_Data.Name,
                        Email = o.Customer_Email,
                        InvoiceTypeId = 2,
                        Postcode = o.Billing_Data.Address_Data.Code,
                        Nip = o.Billing_Data.Tax_Info.Id,
                        CountryCode =   o.Billing_Data.Tax_Info.Country,
                        InvoiceDate=o.Purchase_Date
                    };
                    order.Invoice = invoice;
                }

                Dal.OrderPayment orderPayment = null;


                Dal.ShopOrder shopOrderSaved =   sh.GetShopOrder(Dal.Helper.Shop.Clipperon, orderNumber);

                Dal.ShopOrder so = new Dal.ShopOrder()// sh.GetShopOrder(Dal.Helper.Shop.Clipperon, orderNumber);
                {
                    ShopExtraInfo = o.Market_Name,
                    ShopId = (int)shop,
                    ShopOrderNumber = orderNumber,

                };

                switch(o.Order_Status.ToUpper())
                {
                    case "UNSHIPPED":
                        if(shopOrderSaved.CheckForPayment)
                        { 
                        orderPayment = new OrderPayment()
                        {
                            Amount = (decimal)o.Amount / rate,
                            Comment = "",
                            CurrencyCode = "PLN",
                            CurrencyRate = rate,
                            InsertDate = DateTime.Now,
                            InsertUser = "system",
                            Order = order,
                            PaymentTypeId = (int)Dal.Helper.OrderPaymentType.Przelewy24
                        };
                        so.CheckForPayment = false;
                        }
                        so.IsProcessed = true;
                        break;
                    case "PENDING":
                        so.CheckForPayment = true;
                        so.IsProcessed = false;
                        break;
                    case "CANCELED":
                        so.CheckForPayment = false;
                        so.IsProcessed = true; 
                        break;
                }

                if (/*o.Billing_Data.Address_Data.Data_Complete &&*/ so.CheckForPayment == false)
                {
                    so.IsProcessed = true;
                }
                else
                    so.IsProcessed = false;

                Dal.OrderShipping os = new OrderShipping()
                {
                    InsertDate = DateTime.Now,
                    InsertUser = "system",
                    IsReady = true,
                    Order1 = order,
                    OrderShippingStatusId = (int)Dal.Helper.OrderShippingStatus.Temporary,
                    SendFromExternalWerehouse = false,
                    ShippingCompanyId = 1,
                    ShippingServiceModeId = (int)Dal.Helper.ShippingServiceMode.Courier,
                    ShippingServiceTypeId = (int)Dal.Helper.ShippingServiceType.ForOrder,
                    TrackingNumberSent = false

                };


                int orderId = sh.SetNewOrderClipperon(so, invoice, products, shop, osh, orderPayment, os);
                so.OrderId = orderId;


                if (Dal.Helper.Env == Dal.Helper.EnvirotmentEnum.Prod)
                    SendDataToClipperon(shop, so);

                return true;
            }

            public static void RestoreOrder(Dal.Helper.Shop clipperon)
            {
                SendDataToClipperon(Dal.Helper.Shop.Clipperon, new Dal.ShopOrder()
                {
                    OrderId = 0,
                    ShopOrderNumber = "028-9944608-1229910",
                    IsProcessed=false
                });
            }

            public class RootOrderUpdate
            {
                public string Order_Id { get; set; }
                public bool Pcm_Send { get; set; }
                public string Order_Ext_Id { get; set; }
                public string Tracking_Nos { get; set; }
                public string Ship_Company_Name { get; set; }
                public DateTime? Shipment_Sended { get; set; }
            }


            private static void SendDataToClipperon(Dal.Helper.Shop shop, Dal.ShopOrder order)
            {
                if (GetEnv() == Dal.Helper.EnvirotmentEnum.Dev)
                    return; 


                RootOrderUpdate orderUpdate = new RootOrderUpdate
                {
                    Order_Id = order.ShopOrderNumber,
                    Order_Ext_Id = order.OrderId.ToString(),
                    Shipment_Sended = null,
                    Ship_Company_Name = null,
                    Tracking_Nos = null
                };
                if (order.IsProcessed)
                    orderUpdate.Pcm_Send = true;
                else
                    orderUpdate.Pcm_Send = false;

                if (order.Order != null && order.Order.OrderStatusId == (int)Dal.Helper.OrderStatus.Sent)
                {
                    orderUpdate.Shipment_Sended = DateTime.Now;
                    orderUpdate.Ship_Company_Name = order.Order.OrderShipping.ShippingCompany.AllegroOperatorCode;
                    orderUpdate.Tracking_Nos = order.Order.OrderShipping.ShipmentTrackingNumber;
                }


                HttpWebRequest request = ClipperonRestHelper.GetHttpWebRequest(shop, "/api/Orders", "PUT");

                //string text = ShopRestHelper.GetTextFromWebResponse(request);

                Stream dataStream = request.GetRequestStream();
                string jsonEncodedParams = Bll.RESTHelper.ToJson(new object[] { orderUpdate });
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                string text = "";

                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }


                Dal.ShopHelper sh = new Dal.ShopHelper();
                sh.SetOrderUpdateStatusCompleted(order.ShopOrderNumber, shop);
            }

            public static void SetOrderStatus(Dal.Helper.Shop shop)
            {
                List<Dal.ShopOrder> orders = Dal.DbHelper.Shop.GetShopOrdersForStatusUpdate(shop);

                foreach(Dal.ShopOrder order in orders)
                {
                    SendDataToClipperon(shop, order);
                }

            }

            public static RootOrdersSearch GetOrderFromClipperon(Dal.Helper.Shop shop, string orderNumber)
            {
                Filters f = new Filters
                {
                    Field = "Order_Id",
                    FilterType = "Equal",
                    Value_1 = orderNumber
                };
                HttpWebRequest request = ClipperonRestHelper.GetHttpWebRequest(shop, "/api/Orders/Search", "POST");

                //string text = ShopRestHelper.GetTextFromWebResponse(request);

                Stream dataStream = request.GetRequestStream();
                string jsonEncodedParams = Bll.RESTHelper.ToJson(new object[] { f });
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                string text = "";

                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }

                var json_serializer = new JavaScriptSerializer();


                RootOrdersSearch shopOrder = json_serializer.Deserialize<RootOrdersSearch>(text);

                return shopOrder;
            }
        }


    }
}
