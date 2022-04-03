using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using LajtIt.Dal;
using Newtonsoft.Json;

namespace LajtIt.Bll
{
    public partial class OrderHelper
    {
        private Dal.OrderHelper oh;

        public OrderHelper()
        {
            oh = new Dal.OrderHelper();
        }

        public int GetOrderProductOriginalQuantity(int orderId, string code)
        {
            string comment = Dal.DbHelper.Orders.GetOrdersStatusNoteComment(orderId);
            comment = Regex.Replace(comment, "<br?>", String.Empty);
            List<string> strl = Regex.Split(comment, "<.*?>").Where(l => l != string.Empty).ToList();
            int cnt = 0;
            foreach (string s in strl)
            {
                if (s.Contains(code))
                {
                    cnt++;
                    break;
                }
                cnt++;
            }
            return Int32.Parse(strl[cnt]);
        }

        public static int SetReceipt(int orderId, int cashRegisterId, int receiptTypeId, decimal amount, string n, string userName)
        {

            string nip = null;
            if (n.Trim() != "")
                nip = n.Trim();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);
            List<Dal.OrderProduct> orderProducts = Dal.DbHelper.Orders.GetOrderProducts(orderId)
                .Where(x => x.Quantity > 0).ToList();


            List<Dal.OrderReceipt> receipts = Dal.DbHelper.Orders.GetReceipts(orderId);


            if (receipts.Sum(x => x.Amount) >= order.AmountPaid)
                return -3;


            Dal.OrderReceipt receipt = new Dal.OrderReceipt()
            {
                Amount = amount, //GetAmount(order),
                Description = "",
                InsertDate = DateTime.Now,
                InsertUser = userName,
                Nip = nip,
                OrderId = orderId,
                OrderReceiptStatusId = (int)Dal.Helper.OrderReceiptStatus.New,
                ReceiptTypeId = receiptTypeId,
                CashRegisterId= cashRegisterId

            };

            if (receipt.Amount <= 0)
                return -1;

            if (orderProducts.Count() == 0)
                return -2;

            List<Dal.OrderReceiptProduct> receiptProducts = new List<Dal.OrderReceiptProduct>();
            receiptProducts.AddRange(
                orderProducts.Select(x => new Dal.OrderReceiptProduct()
                {
                    Description = "",
                    Name = GetName(x),
                    OrderProductId = x.OrderProductId,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Rebate = x.Rebate,
                    OrderReceipt = receipt,
                    Unit = "szt"
                }
                ).ToList());

            if (order.ShippingCost > 0)
                receiptProducts.Add(new Dal.OrderReceiptProduct()
                {
                    Description = "",
                    Name = "Przesylka",
                    OrderProductId = null,
                    Price = order.ShippingCost,
                    Quantity = 1,
                    Rebate = 0,
                    OrderReceipt = receipt,
                    Unit = "szt"
                });


            int result = Dal.DbHelper.Orders.SetReceipt(receiptProducts);

            return result;
        }
        private static string GetName(OrderProduct orderProduct)
        {
            string name = String.Format("{0:000000} {1} {2}", orderProduct.ProductCatalogId,
                orderProduct.ProductCatalog.Supplier.Name,
                orderProduct.ProductCatalog.Code);

            name = Bll.Helper.ReplacePolishCharacters(name);

            if (name.Length > 40)
                name = name.Substring(0, 40);

            return name;
        }
        public static void SetOrdersReceipt(int[] orderPaymentIds, string userName)
        {
            List<Dal.OrderPayment> orderPayments = Dal.DbHelper.Orders.GetOrderPayments(orderPaymentIds)
                .Where(x=>x.Amount>0 
                && (x.Order.LockOrder.HasValue==false || x.Order.LockOrder.Value==false)
                ).ToList();

            List<Dal.Order> orders = orderPayments.Select(x => x.Order).Distinct().ToList();

            foreach(Dal.Order order in orders)
            {

                List<Dal.OrderPayment> payments = orderPayments.Where(x => x.OrderId == order.OrderId).ToList();

                if (payments.Count != 1)
                    continue;


                int result = Bll.OrderHelper.SetReceipt(order.OrderId, 2, 1, order.AmountPaid, "", userName);

                string msg = "";
                switch (result)
                {
                    case -3:
                        //DisplayMessage("<h2>Błąd</h2>Brak produktów na paragonie");
                        break;
                    case -2:
                        //DisplayMessage("<h2>Błąd</h2>Brak produktów na paragonie");
                        break;
                    case -1:
                        //DisplayMessage("<h2>Błąd</h2>Wyliczona kwota paragonów jest mniejsza bądź równa zero.");
                        break;
                    default:
                        string xml = Bll.NovitusHelper.GetReceiptXml(result);

                        Dal.DbHelper.Orders.SetReceiptCommand(result, xml);

                        Dal.DbHelper.Accounting.SetOrderPaymentsAccounting(payments.Select(x => x.PaymentId).ToArray(), (int)Dal.Helper.OrderPaymentAccoutingType.CashRegister);

                        //object r = Bll.NovitusHelper.SetCommand(xml, ref msg);

                        //DisplayMessage("Wysłano paragon do drukarki");
                        break;
                }


            }

        }

        //public void InsertAllegroProduct(List<Dal.AllegroItemOrdersForOrderCreation> allegroItemOrderCreations, List<Dal.AllegroItemTransactionItem> buyerForms)
        //{
        //    int orderId = 0;

        //    Dal.AllegroScan allegroScan = new Dal.AllegroScan();
        //    int[] ids = allegroItemOrderCreations.Select(x => x.Id).ToArray();

        //    List<Dal.AllegroItemOrder> allegroItemOrders = allegroScan
        //        .GetMyItemOrdersForOrderCreation(ids);

        //    foreach (int id in ids)
        //    {

        //        Dal.AllegroItemOrder allegroOrder = allegroItemOrders.Where(x => x.Id == id).FirstOrDefault();

        //        List<Dal.AllegroItemTransactionItem> forms = buyerForms
        //            .Where(x => x.ItemId == allegroOrder.ItemId && x.AllegroItemTransaction.UserBuyerId == allegroOrder.UserId)
        //            .ToList();

        //        string comment = "";
        //        Dal.Order order = CreateOrderFromAllegroItemOrder(allegroOrder, ref comment, forms);

        //        orderId = InsertUpdateOrder(order, comment, allegroOrder, forms);
        //        if (orderId == 0)
        //            continue;
        //        Dal.OrderProduct product = CreateProductFromAllegroItemOrder(allegroOrder, orderId);

        //        InsertOrUpdateOrderProduct(product);

        //    }
        //}

        //internal void UpdateOrderBasedOnBuyerForm(List<Dal.AllegroItemTransactionItem> buyerForms)
        //{
        //    var items = buyerForms.Select(x =>
        //        new
        //        {
        //            BuyerUserId = x.AllegroItemTransaction.UserBuyerId,
        //            ItemId = x.ItemId
        //        }).Distinct().ToArray();

        //    foreach (var i in items)
        //    {
        //        Dal.Order order = oh.GetOrderForAutoUpdate(i.BuyerUserId, i.ItemId);

        //        if (order == null)
        //            continue;
        //        Dal.AllegroItemTransaction buyerForm = buyerForms
        //            .Where(x => x.AllegroItemTransaction.UserBuyerId == i.BuyerUserId && x.ItemId == i.ItemId)
        //            .Select(x => x.AllegroItemTransaction)
        //            .OrderByDescending(x=>x.TransactionId)
        //            .FirstOrDefault();

        //        if (buyerForm == null)
        //            continue;
        //        //Dal.OrderComment comment = null;
        //        UpdateOrderBasedOnBuyerForm(order, buyerForm, order.Email);

        //        oh.UpdateOrderBasedOnBuyerForm(order, buyerForm);
        //    }

        //}
        private int InsertOrUpdateOrderProduct(Dal.OrderProduct product)
        {
            int orderproductId = oh.InsertUpdateOrderProduct(product);
            return orderproductId;
        }

        private Dal.OrderProduct CreateProductFromAllegroItemOrder(Dal.AllegroItemOrder aio, int orderId)
        {
            Dal.OrderProduct product = new Dal.OrderProduct()
            {
                OrderId = orderId,
                ExternalProductId = aio.ItemId, 
                Price = aio.ItemPrice,
                Quantity = aio.ItemsOrdered,
                ProductName = aio.AllegroItem.Name,
                ProductCatalogId = GetProductCatalogId(aio.ItemId),
                VAT = Dal.Helper.VAT,
                ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
                OrderProductStatusId = 1
            };

            return product;
        }

        private int? GetProductCatalogId(long itemId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();


            Dal.ProductCatalogAllegroItem pcai = oh.GetProductCatalogIdFromBatch(itemId);

            if (pcai == null)
                return null;

            Dal.ProductCatalogView pc = oh.GetProductCatalog(pcai.ProductCatalogId);

            if (!pc.AutoAssignProduct)
                return null;
            else
                return pcai.ProductCatalogId;

            //if (productCatalogIdFromBatch.HasValue)
            //    return productCatalogIdFromBatch.Value;

            //Dal.ProductCatalogSynonim synonim = oh.GetProductCatalogSynonim(item.Name);

            //if (synonim != null)
            //    return synonim.ProductCatalogId;
            //else
            //    return null;
        }

        private int InsertUpdateOrder(Dal.Order order, string comment, Dal.AllegroItemOrder allegroOrder, List<Dal.AllegroItemTransactionItem> forms)
        {
            int orderId = oh.InsertUpdateOrder(order, comment, allegroOrder, forms);
            return orderId;
        }

       
 
        /// <summary>
        /// Pobiera zamówienia, które mają zbliżająca się datę deklarowanej wysyłki
        /// </summary>
        public static void GetOrdersToComplete()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<Dal.OrderProductsWaitingForDelivery> products = oh.GetOrdersProductsWaitingForDelivery();

            int[] orderProductStatusIds = new int[]
            {
                (int)Dal.Helper.OrderProductStatus.New,
                (int)Dal.Helper.OrderProductStatus.Ordered
            };

            if (orderProductStatusIds.Length > 0)
                products = products.Where(x => orderProductStatusIds.Contains(x.OrderProductStatusId)).ToList();

            products = products.Where(x => x.DeliveryDate.HasValue && x.DeliveryDate.Value.AddDays(-3) <= DateTime.Now)
                .OrderBy(x => x.DeliveryDate)
                .ToList();


            StringBuilder sb = new StringBuilder();

            string row = "<tr class='{0}'><td>{1}</td><td>{2}</td><td title='Na stanie | u dostawcy'>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>";
 

            sb.AppendLine("<table class='orders' border='1'>");
            sb.AppendLine(String.Format(row,
                "header",
                "Produkt",
                "Kod",
                "Dostępność",
                "Dostawca",
                "Status produktu",
                "Dekl. data dostawy"
                ));

            foreach (Dal.OrderProductsWaitingForDelivery product in products)
            {
                string cssClass = "";

                if (product.DeliveryDate.HasValue && product.DeliveryDate.Value <= DateTime.Now)
                    cssClass = "past";
                if (product.DeliveryDate.HasValue && product.DeliveryDate.Value > DateTime.Now && product.DeliveryDate.Value <= DateTime.Now.AddDays(2))
                    cssClass = "now";
                sb.AppendLine(
                    String.Format(row,
                    cssClass,
                    String.Format("<a href='http://192.168.0.107/Order.aspx?id={0}'>{1}</a>", product.OrderId, product.Name),
                    product.Code,
                    GetQuantity(product),
                    product.SupplierName,
                    product.StatusName,
                    String.Format("{0:yyyy/MM/dd}",product.DeliveryDate)
                ));

            }
            sb.AppendLine("</table>");


            Dal.DalHelper dal = new Dal.DalHelper();
            Dal.EmailTemplates emailTemplate = dal.GetEmailTemplate(13);


            Bll.EmailSender emailSender = new EmailSender();

            string subject = String.Format(emailTemplate.Subject, DateTime.Now);

            string body = emailTemplate.Body.Replace("[ORDER_PRODUCTS_TABLE]", sb.ToString());

            Dto.Email email = new Dto.Email()
            {
                Body = body,
                FromEmail = emailTemplate.FromEmail,
                FromName = emailTemplate.FromName,
                Subject = subject,          
                ToName = emailTemplate.FromName
            };

            email.ToRoles = new List<Dal.Helper.SystemRole>
            {
                Dal.Helper.SystemRole.Manager,
                Dal.Helper.SystemRole.Customer
            };

            emailSender.SendEmail(email);
        }

        

        private static string GetQuantity(OrderProductsWaitingForDelivery product)
        {
            string str = "";
            if (product.IsAvailable)
                str = "dostępny";
            else
                str = "niedostępny";
            string sq = "";
            if (product.SupplierQuantity.HasValue == false || product.SupplierQuantity.Value == -1)
                sq= "-";
            else
                sq= product.SupplierQuantity.ToString();


            str += String.Format("<br>({0}|{1})", product.LeftQuantity - product.Quantity, sq);

            return str;

        }

        public void SetOrderOwnerFromOtherOrder(int newOrderId, int orderId)
        {
            string userName = oh.GetOrderStatusHistory(orderId).OrderBy(x=>x.InsertDate).Where(x=>x.OrderStatusId == (int)Dal.Helper.OrderStatus.New).Select(x=>x.InsertUser).FirstOrDefault();

            Dal.OrderStatusHistory osh = new Dal.OrderStatusHistory()
            {
                Comment = String.Format("Zamówienie utworzone na podstawie zamówienia {0} przez {1}", orderId, userName),
                InsertDate = DateTime.Now,
                InsertUser = userName,
                OrderId = newOrderId,
                OrderStatusId = (int)Dal.Helper.OrderStatus.New,
                SendNotification = null

            };
            oh.SetOrderStatus(osh, null);
           
        }

        public bool GetCanMoveProductsFromOrder(int orderId, int[] productIds)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            int[] orderStatusIds = new int[]
            {
                (int)Dal.Helper.OrderStatus.New,
                (int)Dal.Helper.OrderStatus.Cancelled,
                (int)Dal.Helper.OrderStatus.Complaint,
                (int)Dal.Helper.OrderStatus.Deleted,
                (int)Dal.Helper.OrderStatus.WaitingForClient,
                (int)Dal.Helper.OrderStatus.WaitingForPayment,
                (int)Dal.Helper.OrderStatus.WaitingForProduct,
                (int)Dal.Helper.OrderStatus.WaitingForDelivery
            };
            if (!orderStatusIds.Contains(order.OrderStatusId))
                return false;

            return oh.GetCanMoveProductsFromOrder( productIds);
        }

        
        public List<Dal.OrdersView> GetOrders(string orderId,
            bool? isPaid,
            bool isPayOnDelivery,
            int[] orderStatusIds,
            int[]shippingCompanyIds,
            string name,
            string invoiceNumber,
            int? productCatalogId,
            int? hasInvoiceParagon,
            DateTime? dateFrom,
            DateTime? dateTo,
            int[] shopIds,
            int userShopId,
            string userName)
        {
            var o = oh.GetOrders(orderId,
                isPaid,
                isPayOnDelivery,
                orderStatusIds, 
                shippingCompanyIds,
                name,
                invoiceNumber,
                productCatalogId,
                hasInvoiceParagon,
                dateFrom,
                dateTo,
                shopIds,
                userShopId,
                userName);


            return o;
        }

        public int VerifyProductCode(int orderId, int productCatalogId, string actingUser)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper(); 
            Dal.ProductCatalog pc = Dal.DbHelper.ProductCatalog.GetProductCatalog(productCatalogId);

            Dal.OrderProduct op = new Dal.OrderProduct()
            {
                Comment = "",
                ExternalProductId =0,// pc.ProductCatalogId,
                OrderId = orderId, 
                Price = pc.PriceBruttoFixed,
                ProductCatalogId = pc.ProductCatalogId,
                ProductName = pc.Name,
                Quantity = 1,
                VAT = Dal.Helper.VAT,
                ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
                OrderProductStatusId = 1,
                PriceCurrency =pc.PriceBruttoFixed,
                CurrencyRate=1
            };
           

            oh.InsertProductToOrder(op, actingUser);
             
            //if (pc.ProductTypeId == (int)Dal.Helper.ProductType.ComboProduct)
            //{
            //    List<Dal.ProductCatalogSubProductsView> subProducts = pch.GetProductCatalogSubProducts(productCatalogId);

            //    foreach(Dal.ProductCatalogSubProductsView sub in subProducts)
            //    {
            //        Dal.OrderProduct ops = new Dal.OrderProduct()
            //        {
            //            Comment = "",
            //            ExternalProductId = sub.ProductCatalogRefId,
            //            OrderId = orderId, 
            //            Price = 0,
            //            ProductCatalogId = sub.ProductCatalogRefId,
            //            ProductName = sub.Name,
            //            Quantity = sub.Quantity * op.Quantity,
            //            VAT = Dal.Helper.VAT,
            //            ProductTypeId = (int)Dal.Helper.ProductType.ComboProduct,
            //            OrderProductStatusId = 1,
            //            SubOrderProductId = op.OrderProductId
            //        };
            //        oh.InsertProductToOrder(ops, actingUser);
            //    }

            //}


            return op.OrderProductId;

        }
        //public void AddProductCatalogToOrder(int orderId, int productCatalogId, int quantity, string actingUser)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    Dal.ProductCatalogView pc = oh.GetProductCatalog(productCatalogId);

        //    Dal.OrderProduct op = new Dal.OrderProduct()
        //    {
        //        Comment = "",
        //        ExternalProductId = productCatalogId,
        //        OrderId = orderId, 
        //        Price = pc.PriceBruttoFixed,
        //        ProductCatalogId = productCatalogId,
        //        ProductName = pc.Name,
        //        Quantity = quantity,
        //        VAT = Dal.Helper.VAT,
        //        ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
        //        OrderProductStatusId = 1,
        //        CurrencyRate=1,
        //        PriceCurrency=pc.PriceBruttoFixed
              
        //    };

        //    oh.InsertProductToOrder(op, actingUser);

        //}


        public bool ValidateStatusSent(int orderId, ref string msg)
        {
            InvoiceHelper ih = new InvoiceHelper();
            bool result = true;

            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);
            if(!ih.IsLocked(orderId))
            {
                result = false;
                msg = msg + "- Faktura musi zostać ostatecznie zatwierdzona i zamknięta zanim status zostanie zmieniony<br>";
            }



            if (
                (order.AmountBalance.HasValue == false
                || (order.AmountBalance.Value < 0)//&& !order.ShippingType.PayOnDelivery)
                )
                && order.OrderShipping.ShippingCompanyId == 3 // odbiór osobisty

                )
            {
                result = false;
                msg = msg + String.Format("- Zamówienie nie jest opłacone. Balans: {0:C}<br>", order.AmountBalance);
            }

            return result;
        }
        public static bool IsZipCode(Dal.Order order)
        {

            if (order.ShipmentCountryCode != "PL")
                return true;

            if (String.IsNullOrEmpty(order.ShipmentPostcode))
                return true;

            string zipCode = order.ShipmentPostcode;
            string pattern = @"^\d{2}\-\d{3}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(zipCode);
        }
        public static bool ValidateStatusReadyToSend(int orderId, bool autoLockInvoice, ref string msg)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            msg = "";
            bool result = true;

            if (order.OrderShippingId == null)
            {
                result = false;
                msg = msg + "- Należy określić metodę dostawy<br>";

            }
            else
            {


                if (!order.OrderShipping.IsReady && order.ShipmentCountryCode == "PL")
                {
                    result = false;
                    msg = msg + "- Należy utworzyć metodę dostawy i ustawić jej status jako Gotowy<br>";
                }
                List<Dal.OrderProductsView> productsV = oh.GetOrderProducts(orderId).Where(x => x.IsOrderProduct == 1).ToList();
                foreach (Dal.OrderProductsView op in productsV)
                {

                    if (op.Quantity > 0 && !IsOnStock(op, order.OrderShipping))
                    {
                        result = false;
                        msg = msg + String.Format("- Brak produktu {0} w magazynie. Uzupełnij magazyn by kontynuować<br>", op.ProductName);
                    }
                }
            }


            if (
                (!order.AmountBalance.HasValue
                || (order.AmountBalance.Value < 0 && order.OrderShippingId != null && !order.OrderShipping.COD.HasValue)
                )
               // && order.ShippingType.ShippingCompanyId.Value != 3 // odbiór osobisty

                )
            {
                result = false;
                msg = msg + String.Format("- Zamówienie nie jest opłacone. Balans: {0:C}<br>", order.AmountBalance);
            }
            if (order.AmountBalance.HasValue && order.AmountBalance.Value > 0)
            {
                result = false;
                msg = msg + String.Format("- Wystąpiła nadpłata w kwocie {0:C}. Rozłóż tę kwotę na produkty lub dodaj do wartości przesyłki.<br>", order.AmountBalance);
            }
        
            if (order.ShipmentPostcode != "" && order.ShipmentCountryCode == "PL" && !IsZipCode(order))
            {
                result = false;
                msg = msg + "- Kod pocztowy niepoprawny<br>";
            }
            if (order.CompanyId == null)
            {
                result = false;
                msg = msg + "- Nie przyporządkowano firmy do realizacji zamówienia<br>";
            }



        
            List<Dal.OrderProduct> products = oh.GetOrderProductsWithoutCatalogAssigment(orderId);

            if (products.Count > 0)
            {
                result = false;
                msg = msg + "Przypisz następującym produktom produkt z katalogu:<br>";

                foreach (Dal.OrderProduct op in products)
                {
                    msg = msg + String.Format("- {0}<br>", op.ProductName);

                }
            }

            msg += ValidateVATRate(orderId, oh, order);





            //List<Dal.OrderProductSubProductsNotAssignedResult> orderProductsSubProducts =
            //    oh.GetOrderProductsSubProductsNotAssigned(orderId);

            //if (orderProductsSubProducts.Count > 0)
            //{
            //    result = false;
            //    msg = msg + "Produkty nie posiadające przypisanych podproduktów:<br>";

            //    foreach (Dal.OrderProductSubProductsNotAssignedResult op in orderProductsSubProducts)
            //        msg = msg + String.Format("- Produkt: {1}, podprodukt: {0}<br>", op.GroupName, op.ProductName);
            //}


            InvoiceHelper ih = new InvoiceHelper();


            if (!ih.IsLocked(orderId))// && order.ShippingType.ShippingCompanyId.Value != 3)
            {

                if (autoLockInvoice && ih.CanLockInvoice(orderId, ref msg))
                    ih.LockInvoice(orderId, false);
                else
                {
                    result = false;
                    msg = msg + "- Faktura musi zostać ostatecznie zatwierdzona i zamknięta zanim status zostanie zmieniony<br>";
                }
            }


            if (!result)
                return false;

            return result;
        }
        public static bool ValidateStatusWaitingForPayment(int orderId,   ref string msg)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            msg = "";
            bool result = true;

            if (!order.OrderShipping.IsReady)
            {
                result = false;
                msg = msg + String.Format("- Dostawa jest nieskonfigurowana<br>", order.AmountBalance);
            }
            return result;
        }
 
        public static bool ValidateStatusWaitingForAcceptance(int orderId, int[] orderProductIds, ref string msg)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            msg = "";
            bool result = true;
     
            if (order.ShopId == (int)Dal.Helper.Shop.Empik && orderProductIds.Length == 0)
            {

                result = false;
                msg += "- Musisz wybrać przynajmniej jeden produkt do realizacji<br>";
            }
            if (!order.OrderShipping.IsReady)
            {
                result = false;
                msg = msg + String.Format("- Dostawa jest nieskonfigurowana<br>", order.AmountBalance);
            }

            if (!result)
                return false;

            return result;
        }
        public static bool ValidateStatusWaitingForDelivery(int orderId, bool autoLockInvoice, ref string msg)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            msg = "";
            bool result = true;
           
            if (order.DeliveryDate.HasValue==false || order.DeliveryDate.Value.AddHours(23).AddMinutes(59) <= DateTime.Now)
            {
                result = false;
                msg = msg + "- Musisz określić maksymalną datę dostawy<br>";
            }

            List<Dal.OrderProduct> products = oh.GetOrderProductsWithoutCatalogAssigment(orderId);

            if (products.Count > 0)
            {
                result = false;
                msg = msg + "Przypisz następującym produktom produkt z katalogu:<br>";

                foreach (Dal.OrderProduct op in products)
                {
                    msg = msg + String.Format("- {0}<br>", op.ProductName);

                }
            }

            if (order.AmountBalance.HasValue && order.AmountBalance.Value > 0)
            {
                result = false;
                msg = msg + String.Format("- Wystąpiła nadpłata w kwocie {0:C}. Rozłóż tę kwotę na produkty lub dodaj do wartości przesyłki.<br>", order.AmountBalance);
            }
            if (!order.OrderShipping.IsReady && order.ShipmentCountryCode == "PL" )
            {
                result = false;
                msg = msg + String.Format("- Dostawa jest nieskonfigurowana<br>", order.AmountBalance);
            }

            if (!result)
                return false;

            return result;
        }

        private static bool IsOnStock(OrderProductsView op, Dal.OrderShipping st)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper(); 
            Dal.ProductCatalogViewFnResult pc = oh.GetProductCatalogFn(op.ProductCatalogId.Value, st.ShippingServiceMode.WarehouseId);

            Dal.ProductCatalogHelper ph = new Dal.ProductCatalogHelper();
            int quantityDeliveredForOrder = ph.GetProductCatalogDeliveryForOrder(op.ProductCatalogId.Value, op.OrderId);
            return (pc.LeftQuantity >= 0 || op.Quantity <= quantityDeliveredForOrder);

             
        }
 
        public static string ValidateVATRate(int orderId, Dal.OrderHelper oh, Dal.Order order)
        {
            List<Dal.OrderProductsView> p = oh.GetOrderProducts(orderId);
            decimal vatRate = order.ShippingCostVAT;
            string msg = "";


            // wyłączamy to narazie
            //if (p.Count > 0)
            //{
            //    foreach (Dal.OrderProductsView op in p)
            //        if (vatRate != op.VAT)
            //        {
            //            msg = "- Stawka VAT musi być taka sama dla wszystkich produktów oraz kosztu przesyłki<br>";
            //            break;
            //        }
            //}
            return msg;
        }

        public Dal.OrderStatus GetOrderStatus(int orderStatusId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            return oh.GetOrderStatus(orderStatusId);
        }
        //public void ExportForPocztaPolska(int[] orderIds, DirectoryInfo di, string actingUser)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    List<Dal.Order> orders = oh.GerOrders(orderIds);
        //    Bll.PocztaPolskaHelper pph = new PocztaPolskaHelper();
        //    string fileName = pph.ExportOrders(orders, di);

        //    oh.SetOrdersStatus(Dal.Helper.ShippingCompany.PocztaPolska, orderIds, Dal.Helper.OrderStatus.Exported, "Export automatyczny", actingUser, fileName);
        //}
        //public int ExportForInPostCsv(int[] orderIds, DirectoryInfo di, string actingUser, out int batchId)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    List<Dal.Order> orders = oh.GerOrders(orderIds);
        //    Bll.PaczkomatyHelper pph = new PaczkomatyHelper();
        //    List<Dal.Order> ordersToFile = new List<Dal.Order>();

        //    int[] shippingIdNotToExport = oh.GetShippingTypes().Where(x => !x.ExportToFile).Select(x => x.ShippingTypeId).ToArray();

        //    ordersToFile = orders.Where(x => !shippingIdNotToExport.Contains(x.ShippintTypeId)).ToList();

        //    string fileName = pph.ExportOrders(ordersToFile, di);

        //    batchId = oh.SetOrdersStatus(Dal.Helper.ShippingCompany.InPost, orderIds, Dal.Helper.OrderStatus.Exported, "Export automatyczny", actingUser, fileName);

        //    return orderIds.Count();
        //}
        //public bool ExportForInPost(string service_code, int orderId, string actingUser)
        //{
        //    try
        //    {
        //        Bll.InpostHelper ih = new InpostHelper();
        //        Dal.OrderHelper oh = new Dal.OrderHelper();
        //        Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);
        //        return ih.ExportInpostBatch(service_code, order, actingUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        Bll.ErrorHandler.SendError(ex, "Tworzenie etykiet Inpost"); 
        //        return false;
        //    }
        //}
        //public string GetExportDataForSiodemka(int[] orderIds, DirectoryInfo outputDir, string actingUser, Dal.Helper.ShippingCompany sc)
        //{
        //    string file = String.Format(@"{0}\{2}_{1}.xlsx", outputDir.FullName, Guid.NewGuid(), sc.ToString());
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    List<Dal.Order> orders = oh.GetOrders(orderIds).OrderBy(x=>x.OrderId).ToList();
        //    FileInfo newFile = new FileInfo(file);
        //    if (newFile.Exists)
        //    {
        //        newFile.Delete();  // ensures we create a new workbook
        //        newFile = new FileInfo(file);
        //    }
        //    #region
        //    string f = null;
        //    using (ExcelPackage package = new ExcelPackage(newFile))
        //    {

        //        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Inventory");

        //        worksheet.Cells[1, 1].Value = "Płatnik ";
        //        worksheet.Cells[1, 2].Value = "Nr Ext";
        //        worksheet.Cells[1, 3].Value = "NIP ";
        //        worksheet.Cells[1, 4].Value = "Nazwa klienta ";
        //        worksheet.Cells[1, 5].Value = "Imię ";
        //        worksheet.Cells[1, 6].Value = "Nazwisko ";
        //        worksheet.Cells[1, 7].Value = "Miasto ";
        //        worksheet.Cells[1, 8].Value = "Kod pocztowy ";
        //        worksheet.Cells[1, 9].Value = "Ulica ";
        //        worksheet.Cells[1, 10].Value = "Nr domu ";
        //        worksheet.Cells[1, 11].Value = "Nr lokalu ";
        //        worksheet.Cells[1, 12].Value = "Telefon ";
        //        worksheet.Cells[1, 13].Value = "e-mail ";
        //        worksheet.Cells[1, 14].Value = "Nazwisko nadawcy ";
        //        worksheet.Cells[1, 15].Value = "Kwota pobrania ";
        //        worksheet.Cells[1, 16].Value = "Wartość ubezpieczenia ";
        //        worksheet.Cells[1, 17].Value = "Opis zawartości ";
        //        worksheet.Cells[1, 18].Value = "Zwrot dokumentów ";
        //        worksheet.Cells[1, 19].Value = "Ksero doręczenia ";
        //        worksheet.Cells[1, 20].Value = "Kopert ";
        //        worksheet.Cells[1, 21].Value = "Paczek do 1 kg ";
        //        worksheet.Cells[1, 22].Value = "Paczek do 5 kg ";
        //        worksheet.Cells[1, 23].Value = "Paczek do 10 kg ";
        //        worksheet.Cells[1, 24].Value = "Paczek do 20 kg ";
        //        worksheet.Cells[1, 25].Value = "Paczek do 30 kg ";
        //        worksheet.Cells[1, 26].Value = "Paczek pow. 30 kg ";
        //        worksheet.Cells[1, 27].Value = "Waga pacz. pow. 30 kg ";
        //        worksheet.Cells[1, 28].Value = "Łączna waga paczek ";
        //        worksheet.Cells[1, 29].Value = "Palet do 100 kg ";
        //        worksheet.Cells[1, 30].Value = "Palet do 200 kg ";
        //        worksheet.Cells[1, 31].Value = "Palet do 300 kg ";
        //        worksheet.Cells[1, 32].Value = "Palet do 400 kg ";
        //        worksheet.Cells[1, 33].Value = "Palet do 500 kg ";
        //        worksheet.Cells[1, 34].Value = "Palet do 600 kg ";
        //        worksheet.Cells[1, 35].Value = "Palet do 700 kg ";
        //        worksheet.Cells[1, 36].Value = "Palet do 800 kg ";
        //        worksheet.Cells[1, 37].Value = "Łączna waga palet ";
        //        worksheet.Cells[1, 38].Value = "MPK ";
        //        worksheet.Cells[1, 39].Value = "Nr bezpiecznej koperty ";
        //        worksheet.Cells[1, 40].Value = "Numer zewnętrzny";

        //        int r = 2;

        //        #region  (Dal.Order order in orders)
        //        foreach (Dal.Order order in orders)
        //        {

        //            worksheet.Cells[r, 1].Value = "";
        //            worksheet.Cells[r, 2].Value = order.OrderId.ToString(); ;
        //            worksheet.Cells[r, 3].Value = null;
        //            worksheet.Cells[r, 4].Value = String.Format("{0} {1} {2}", order.ShipmentCompanyName, order.ShipmentFirstName, order.ShipmentLastName);
        //            worksheet.Cells[r, 5].Value = "";// "Imię ";
        //            worksheet.Cells[r, 6].Value = "";// "Nazwisko ";
        //            worksheet.Cells[r, 7].Value = order.ShipmentCity;
        //            worksheet.Cells[r, 8].Value = order.ShipmentPostcode;
        //            worksheet.Cells[r, 9].Value = order.ShipmentAddress;
        //            worksheet.Cells[r, 10].Value = "."; // "Nr domu ";
        //            worksheet.Cells[r, 11].Value = ""; // "Nr lokalu ";
        //            worksheet.Cells[r, 12].Value = order.Phone;
        //            worksheet.Cells[r, 13].Value = order.Email;
        //            worksheet.Cells[r, 14].Value = "Lajt it - oświetlenie";
        //            worksheet.Cells[r, 15].Value = order.ShippingType.PayOnDelivery == true ? Math.Abs(order.AmountBalance.Value).ToString() : null;
        //            worksheet.Cells[r, 16].Value = GetInsuranceAmount(sc, order);
        //            worksheet.Cells[r, 17].Value = "Materiały dekoracyjne. Ostrożnie szkło!!!";
        //            worksheet.Cells[r, 18].Value = null;
        //            worksheet.Cells[r, 19].Value = null;
        //            worksheet.Cells[r, 20].Value = "0";// "Kopert ";

        //            string[] w =new string[] { "0", "0", "0", "0", "0" };
        //            if (order.ShippingData != null)
        //            {
        //                w = order.ShippingData.Split(',');
        //                if (w.Length != 5)
        //                {
        //                    w = new string[] { "0", "0", "0", "0", "0" };
        //                }
        //            }
        //            worksheet.Cells[r, 21].Value = w[0]; //"Paczek do 1 kg ";
        //            worksheet.Cells[r, 22].Value = w[1];// "Paczek do 5 kg ";
        //            worksheet.Cells[r, 23].Value = w[2];// "Paczek do 10 kg ";
        //            worksheet.Cells[r, 24].Value = w[3];// "Paczek do 20 kg ";
        //            worksheet.Cells[r, 25].Value = w[4];// "Paczek do 30 kg ";
        //            worksheet.Cells[r, 26].Value = "0";// "Paczek pow. 30 kg ";
        //            worksheet.Cells[r, 27].Value = "0";// "Waga pacz. pow. 30 kg ";
        //            worksheet.Cells[r, 28].Value = "0";//"Łączna waga paczek ";
        //            worksheet.Cells[r, 29].Value = "0";//"Palet do 100 kg ";
        //            worksheet.Cells[r, 30].Value = "0";//"Palet do 200 kg ";
        //            worksheet.Cells[r, 31].Value = "0";//"Palet do 300 kg ";
        //            worksheet.Cells[r, 32].Value = "0";//"Palet do 400 kg ";
        //            worksheet.Cells[r, 33].Value = "0";//"Palet do 500 kg ";
        //            worksheet.Cells[r, 34].Value = "0";//"Palet do 600 kg ";
        //            worksheet.Cells[r, 35].Value = "0";//"Palet do 700 kg ";
        //            worksheet.Cells[r, 36].Value = "0";//"Palet do 800 kg ";
        //            worksheet.Cells[r, 37].Value = "0";// "Łączna waga palet ";
        //            worksheet.Cells[r, 38].Value = "";// "MPK ";
        //            worksheet.Cells[r, 39].Value = "";// "Nr bezpiecznej koperty ";
        //            worksheet.Cells[r, 40].Value = order.OrderId.ToString(); ;

        //            r++;
        //        }
        //        #endregion
        //        package.Save();

        //        GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

        //        var xlsxFile = new GemBox.Spreadsheet.ExcelFile();

        //        // Load data from XLSX file.
        //        xlsxFile.LoadXlsx(file, GemBox.Spreadsheet.XlsxOptions.PreserveMakeCopy);

        //        f= String.Format("{1}_{0:yyyyMMddHHmmss}.xls", DateTime.Now, sc.ToString());
        //        // Save XLSX file to XLS file.
        //        xlsxFile.SaveXls(String.Format(@"{0}\{1}", outputDir.FullName,  f));

        //    }
        //    #endregion
        //    oh.SetOrdersStatus(sc, 
        //        orderIds, Dal.Helper.OrderStatus.Exported, "Export automatyczny", actingUser, f); 

        //    return f;
        //}


        public int SetOrderNew(int shopId)
        {
            Dal.Order o = new Dal.Order()
            {
                OrderStatusId = -1,
                InsertDate = DateTime.Now,
         
                Email = "", 
                Phone = "",
                //ShippintTypeId = 0,
                ExternalUserId = 0,
                AmountPaid = 0, 
                ShippingCostVAT = 0.23M,
                ParActive=true,
                CompanyId = Dal.Helper.DefaultCompanyId,
                ShippingAmountCurrency=0,
                ShippingCurrencyRate=1,
                ShipmentCountryCode="PL",
                ShippingCurrencyCode="PLN"
            };

            if (shopId != 0)
                o.ShopId = shopId;


            Dal.OrderHelper oh = new Dal.OrderHelper();
            return oh.SetOrderNew(o);
        }

        public void SerOrderProductsToNewOrder(int orderId, int newOrderId, int[] orderProductToMove, string actingUser)
        {

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SerOrderProductsToNewOrder(orderId, newOrderId, orderProductToMove, actingUser);

        }
        public void SerOrderPaymentToNewOrder(int orderId, int newOrderId, string userName)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SerOrderPaymentToNewOrder(orderId, newOrderId, userName);
        }
        public int SetOrderNew(int orderId, int shopId, bool keepOrignalOrderInsertDate, bool checkForPayments)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Order o = Dal.DbHelper.Orders.GetOrder(orderId);

            Dal.Invoice i = null;

            if (o.Invoice != null)
            {
                i = new Dal.Invoice()
                {
                    Address = o.Invoice.Address,
                    City = o.Invoice.City,
                    CompanyName = o.Invoice.CompanyName,
                    Email = o.Invoice.Email,
                    InvoiceDate = DateTime.Now,
                    IsDeleted = false,
                    Nip = o.Invoice.Nip,
                    Postcode = o.Invoice.Postcode,
                    CompanyId = Dal.Helper.DefaultCompanyId,
                    InvoiceTypeId = 2,
                    ExcludeFromInvoiceReport = false

                };
            }
            Dal.Order newOrder = new Dal.Order()
            {
                OrderStatusId = -1,
                 
                Email = o.Email, 
                ShipmentFirstName = o.ShipmentFirstName,
                ShipmentLastName = o.ShipmentLastName,
                ShipmentAddress = o.ShipmentAddress,
                ShipmentPostcode = o.ShipmentPostcode,
                ShipmentCity = o.ShipmentCity,
                ShipmentCompanyName = o.ShipmentCompanyName,
                Phone = o.Phone,
                Phone2 = o.Phone2,
                //ShippintTypeId = o.ShippintTypeId,
                ExternalUserId = o.ExternalUserId,
                AmountPaid = 0,
                Invoice = i,
                ShopId = shopId==0?o.ShopId:shopId,
                ShippingCostVAT=0.23M,
                ParActive = o.ParActive,
                CompanyId = Dal.Helper.DefaultCompanyId,
                ShippingCurrencyCode = o.ShippingCurrencyCode,
                ShipmentCountryCode=o.ShipmentCountryCode,
                ShippingCurrencyRate=o.ShippingCurrencyRate,
                ShippingAmountCurrency=o.ShippingAmountCurrency,
                DoNotAutoEdit = o.DoNotAutoEdit,
                ExternalOrderNumber = o.ExternalOrderNumber,
                PromoCode = o.PromoCode,
                PromoRebate = o.PromoRebate,
                //ShippingData = o.ShippingData,
                
                
            };
            if (keepOrignalOrderInsertDate)
                newOrder.DeliveryDate = o.DeliveryDate;

            if (keepOrignalOrderInsertDate)
                newOrder.InsertDate = o.InsertDate;
            else
                newOrder.InsertDate = DateTime.Now;


            if (checkForPayments)
            {
                List<Dal.OrderReceipt> or = Dal.DbHelper.Orders.GetReceipts(orderId);
                decimal sum = 0;

                if (or.Count() > 0)
                    sum = or.Sum(x => x.Amount);


                if ((o.Invoice != null && o.Invoice.IsLocked.HasValue && o.Invoice.IsLocked.Value) || sum == o.AmountToPay)
                    newOrder.LockOrder = true;

            }



            return oh.SetOrderNew(newOrder);

        }
         

        public bool CreateParagon(int orderId)
        {
             
            bool result = false;
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            if (order == null || /*order.Invoice!=null ||*/ !order.ParActive)
                return false;

            int? parSeqNo = oh.GetLastParagon( orderId);

            if (parSeqNo == null)
            {
                parSeqNo = 1;
            }
            else
            {
                parSeqNo = parSeqNo.Value + 1;
            } 

            string parNumber = String.Format("{0}/{1}", parSeqNo, DateTime.Now.Year);
            oh.SetParagonNumber(parSeqNo.Value, parNumber,   orderId);
            result = true;

            return result;
        }
 
 

 
//        private string GetProductCatalogRecommendedProducts(string serverUrl, int ProductCatalogId, long allegroUserId)
//        {
//            Dal.OrderHelper oh = new Dal.OrderHelper();
//            List<Dal.ProductCatalogRecommendedProductsResult> products = oh.GetProductCatalogAllegroRecommended(ProductCatalogId)
//                .OrderBy(x => x.SelectionType).ThenBy(x=>x.IsReady).Where(x => x.IsReady).ToList();   

//            StringBuilder sb = new StringBuilder();

//            int[] selectionTypes = products.Select(x => x.SelectionType).Distinct().OrderBy(x => x).ToArray();

//            foreach (int st in selectionTypes)
//            {                 
//                List<Dal.ProductCatalogRecommendedProductsResult> productsFromType = products.Where(x=>x.SelectionType == st).ToList();
//                switch (st)
//                {
//                    case 1:
//                        sb.AppendLine("<div style='clear:both;'></div>");
//                        BindMainSuggestions(productsFromType, sb, serverUrl,   allegroUserId);
//                        break;
//                    case 2: 
//                        sb.AppendLine("<div style='clear:both;'></div><div class='suggestions_subtitles'>Produkty z tej kategorii</div>");
//                        BindSuggestions(productsFromType, sb, serverUrl, allegroUserId);
//                        break;
//                    case 3: 
//                        sb.AppendLine("<div style='clear:both;'></div><div class='suggestions_subtitles'>Pozostałe polecane produkty</div>");
//                        BindSuggestions(productsFromType, sb, serverUrl,   allegroUserId);
//                        break;
//                }
                 
                 
//            }
//            return sb.ToString();
//        }

//        private void BindMainSuggestions(List<Dal.ProductCatalogRecommendedProductsResult> productsFromType, StringBuilder sb,
//            string serverUrl, long allegroUserId)
//        {

//            foreach (Dal.ProductCatalogRecommendedProductsResult product in productsFromType)
//                {
//                    string imageUrl = product.ImageFullName; 

//                    //Bll.ProductCatalogCalculator calc = new ProductCatalogCalculator(product.PurchasePrice.Value, 
//                    //    product.AllegroPrice, 
//                    //    product.Rebate,
//                    //    product.Margin);

//                    string shortDescription = product.ShortDescription??"";
                
//                    sb.AppendLine(
//                        String.Format(@"<div class='suggestion2 bold_frame'>
//    <div class='suggestion_title' title='{2}'>{2}</div>

//<div class='suggestion2_img'  title='Kliknij i zobacz: {2}'><a href='http://allegro.pl/listing/user.php?string={7}&us_id={6}' 
//        target='_blank'><img src='{0}/ProductCatalog/{1}' style='{5}'/></a>
//    </div>
//<div class='suggestion_desc'>{8}</div><div style='clear:both;'></div>
//<div class='suggestion2_price'>{3:C}</div></div>",
//                       // String.Format("<div class='suggestion {7}'><div class='suggestion_title' title='{2}'>{2}</div><div class='suggestion_img'  title='Kliknij i zobacz: {2}'><a href='http://allegro.pl/listing/listing.php?description=1&order=m&string=%28PRODUCT{4}%29&search_scope=category-5317&bmatch=seng-v0-o-1021' target='_blank'><img src='{0}/ProductCatalog/{1}' style='{5}'/></a></div><div class='suggestion_price'>{3:C}</div></div>",
//                        serverUrl,
//                        imageUrl,
//                        product.AllegroName,
//                        product.PriceBrutto,
//                        product.ProductCatalogId,
//                        GetImageSize(product),
//                        allegroUserId,
//                        product.Code  ,
//                        shortDescription.Replace("\n", "<br>")
//                        )

//                        );

//                }
//                sb.AppendLine("<div style='clear:both;'></div>");
//        }

//        private void BindSuggestions(List<Dal.ProductCatalogRecommendedProductsResult> productsFromType, StringBuilder sb,
//            string serverUrl, long allegroUserId)
//        {

//            foreach (Dal.ProductCatalogRecommendedProductsResult product in productsFromType)
//            {
//                string imageUrl = product.ImageFullName;

//               // Bll.ProductCatalogCalculator calc = new ProductCatalogCalculator(product.PurchasePrice.Value, product.AllegroPrice, product.Rebate, product.Margin);

//                sb.AppendLine(
//                    String.Format(@"<div class='suggestion'><div class='suggestion_title' title='{2}'>{2}</div>
//                    <div class='suggestion_img'  title='Kliknij i zobacz: {2}'>
//        <a href='http://allegro.pl/listing/user.php?string={6}&us_id={5}' target='_blank'><img src='{0}/ProductCatalog/{1}' style='{4}'/></a>
//</div><div style='clear:both;'></div><div class='suggestion_price'>{3:C}</div></div>",
//                    // String.Format("<div class='suggestion {7}'><div class='suggestion_title' title='{2}'>{2}</div><div class='suggestion_img'  title='Kliknij i zobacz: {2}'><a href='http://allegro.pl/listing/listing.php?description=1&order=m&string=%28PRODUCT{4}%29&search_scope=category-5317&bmatch=seng-v0-o-1021' target='_blank'><img src='{0}/ProductCatalog/{1}' style='{5}'/></a></div><div class='suggestion_price'>{3:C}</div></div>",
//                    serverUrl,
//                    imageUrl,
//                    product.AllegroName,
//                    product.PriceBrutto,
//                    GetImageSize(product),
//                    allegroUserId,
//                    product.Code
//                    )

//                    );

//            }
//            sb.AppendLine("<div style='clear:both;'></div>");
//        }

//        private object GetImageSize(Dal.ProductCatalogRecommendedProductsResult product)
//        {
//            if (product.ImageWidth > product.ImageHeight)
//                return "width: 170px;";
//            else
//                return "height:160px";
//        }
//        private string GetProductCatalogPreviewDescription(Dal.ProductCatalogView pc)
//        {
////            StringBuilder sb = new StringBuilder();
////            string tableRow = "<div class='regular_text'>{0}</div>";
             

             

////            string description = String.Format(@"{0}
////{1}", pc.Description, pc.Supplier.CommonDescriptionText);
////            string[] lines = description.Split(new char[] { '\n' });

////            foreach (string line in lines)
////            {
////                if (String.IsNullOrEmpty(line))
////                    continue; 
                 
////                    sb.AppendLine(String.Format(tableRow,line));
                
////            }

////            return sb.ToString();


//            string line  = String.Format("<div class='regular_text'>{0}</div>", pc.Description);

//            return line;
//        }
//        private string GetProductCatalogPreviewSpecification(Dal.ProductCatalogView pc, Dal.Supplier supplier)
//        {
//            StringBuilder sb = new StringBuilder();
//            string tableRow = "<tr class='offer_spec_row'><td class='offer_spec_title'>{0}</td><td class='offer_spec_value'>{1}</td></tr>";

//            if (supplier.ShowSupplierInAllegro.HasValue && supplier.ShowSupplierInAllegro.Value)
//                sb.Append(String.Format(tableRow, "Producent", supplier.Name));



//            if (!String.IsNullOrEmpty(pc.Code))
//                sb.Append(String.Format(tableRow, "Kod produktu", pc.Code));

//            string specification = String.Format(@"{0}", pc.Specification);
           
//            string[] lines = specification.Split(new char[] { '\n' });

//            foreach (string line in lines)
//            {
//                if(String.IsNullOrEmpty(line))
//                    continue;

//                string l = line;

//                string[] parts = l.Split(new char[] { ':' }, 2);

//                if (parts.Length > 1)
//                    sb.AppendLine(String.Format(tableRow, parts[0], parts[1]));
//                else
//                    sb.AppendLine(String.Format(tableRow, "", parts[0]));

//            }

//            string s = sb.ToString();
//            s = SearchAndReplace(s, 
//                String.Format(@"<a href='http://allegro.pl/listing/user/listing.php?us_id={0}&string=$1&search_scope=userItems-{0}' target='_blank'>$2</a>", supplier.AllegroUserIdAccount));
//            return s;
//        }

        public class Item
        {
            public string type { get; set; }
            public string url { get; set; }
            public string content { get; set; }
        }

        public class Section
        {
            public List<Item> items { get; set; }
        }

        public class ProductSpec
        {
            public List<Section> sections { get; set; }
        }

        public static string GetPreview(Dal.Helper.Shop shop, int productCatalogId, bool preview)
        {
            return GetPreview(shop, productCatalogId, preview, false);
        }

        public static string GetPreviewHTML(Dal.Helper.Shop shop, int productCatalogId, bool preview, bool removeHtmlTags)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ShopHelper sh = new Dal.ShopHelper();

            List<Dal.ProductCatalogAttributesForShopResult> attributesForShop = sh.GetProductCatalogAttributesForShop(shop, productCatalogId);

            //List<ProductCatalogAttributesForProductPrevResult> attributes = pch.GetProductCatalogAttributesForProduct(productCatalogId);

            Dal.ProductCatalogView pc = pch.GetProductCatalogView(productCatalogId);

            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("<div><p><strong>Producent: </strong>{0}</p>" + Environment.NewLine, pc.SupplierName));
            if (!String.IsNullOrEmpty(pc.Code))
                sb.Append(String.Format("<p><strong>Kod produktu: </strong>{0}</p>" + Environment.NewLine, pc.Code));


            var groups = attributesForShop.OrderBy(x => x.Order).Select(x => new
            {
                x.AttributeGroupId,
                AttributeGroupName = x.GroupName,
                x.AttributeGroupTypeId

            }).Distinct().ToList();

            //if (!preview)
            //    groups = groups.Where(x => x.ExportToAllegro).ToList();

            foreach (var g in groups)
            {

                var products = attributesForShop.Where(x => x.AttributeGroupId == g.AttributeGroupId).ToList();

                List<string> rowsAtt = new List<string>();
                //string cssClass = "";// !g.ExportToAllegro ? "class='grayedout'" : "";
                switch (g.AttributeGroupTypeId)

                {
                    case 1:
                        sb.Append(String.Format("<p><strong>{0}: </strong>{1}</p>" + Environment.NewLine, g.AttributeGroupName, products[0].AttName));
                        break;
                    case 2:
                        string productsCommaSep = String.Join(", ", products.OrderByDescending(x => x.IsDefault).Select(x => x.AttName).Distinct().ToArray());
                        sb.Append(String.Format("<p><strong>{0}: </strong>{1}</p>" + Environment.NewLine, g.AttributeGroupName, productsCommaSep));
                        break;
                    case 3:

                        foreach (var p in products)
                        {
                            switch (p.AttributeTypeId.Value)
                            {
                                case 1:
                                    rowsAtt.Add(String.Format("<li>{0}: {1}</li>" + Environment.NewLine, p.AttName, GetFieldValue2(p, p.DecimalValue)));
                                    break;
                                case 2:
                                    rowsAtt.Add(String.Format("<li>{0}: {1}</li>" + Environment.NewLine, p.AttName, GetFieldValue2(p, p.StringValue)));
                                    break;
                            }
                        }
                        sb.Append(String.Format("<p><strong>{0}: </strong></p><ul>{1}</ul>" + Environment.NewLine, g.AttributeGroupName, String.Join("", rowsAtt.ToArray())));

                        break;
                }

                sb.Append(String.Format("</div>"));

            }


            if (removeHtmlTags)
            {
                return Regex.Replace(sb.ToString(), "<.*?>", String.Empty);
            }

            return "<![CDATA[" + sb.ToString() + "]]>";
        }
        public static string GetPreview(Dal.Helper.Shop shop, int productCatalogId, bool preview, bool removeHtmlTags)
            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ShopHelper sh = new Dal.ShopHelper();

            List<Dal.ProductCatalogAttributesForShopResult> attributesForShop  = sh.GetProductCatalogAttributesForShop(shop, productCatalogId);

            //List<ProductCatalogAttributesForProductPrevResult> attributes = pch.GetProductCatalogAttributesForProduct(productCatalogId);

            Dal.ProductCatalogView pc = pch.GetProductCatalogView(productCatalogId);

            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("<p><b>Producent: </b>{0}</p>" + Environment.NewLine, pc.SupplierName));
            if (!String.IsNullOrEmpty(pc.Code))
                sb.Append(String.Format("<p><b>Kod produktu: </b>{0}</p>" + Environment.NewLine, pc.Code)); 


            var groups = attributesForShop.OrderBy(x => x.Order).Select(x => new
            {
                x.AttributeGroupId,
                AttributeGroupName = x.GroupName,
                x.AttributeGroupTypeId

            }).Distinct().ToList();

            //if (!preview)
            //    groups = groups.Where(x => x.ExportToAllegro).ToList();

            foreach (var g in groups)
            {

                var products = attributesForShop.Where(x => x.AttributeGroupId == g.AttributeGroupId).ToList();

                List<string> rowsAtt = new List<string>();
                //string cssClass = "";// !g.ExportToAllegro ? "class='grayedout'" : "";
                switch (g.AttributeGroupTypeId)

                {
                    case 1:
                        sb.Append(String.Format("<p><b>{0}: </b>{1}</p>" + Environment.NewLine, g.AttributeGroupName, products[0].AttName));
                        break;
                    case 2:
                        string productsCommaSep = String.Join(", ", products.OrderByDescending(x => x.IsDefault).Select(x => x.AttName).Distinct().ToArray());
                        sb.Append(String.Format("<p><b>{0}: </b>{1}</p>" + Environment.NewLine, g.AttributeGroupName, productsCommaSep));
                        break;
                    case 3:

                        foreach (var p in products)
                        {
                            switch (p.AttributeTypeId.Value)
                            {
                                case 1:
                                    rowsAtt.Add(String.Format("<li>{0}: {1}</li>" + Environment.NewLine, p.AttName, GetFieldValue2(p, p.DecimalValue)));
                                    break;
                                case 2:
                                    rowsAtt.Add(String.Format("<li>{0}: {1}</li>" + Environment.NewLine, p.AttName, GetFieldValue2(p, p.StringValue)));
                                    break;
                            }
                        }
                        sb.Append(String.Format("<p><b>{0}: </b></p><ul>{1}</ul>"+ Environment.NewLine, g.AttributeGroupName, String.Join("", rowsAtt.ToArray())));

                        break;
                }



            }


            if(removeHtmlTags)
            {
               return Regex.Replace(sb.ToString(), "<.*?>", String.Empty);
            }

            return sb.ToString();
        }
        public static string GetPreviewCompare(Dal.Helper.Shop shop, int productCatalogId, int productCatalogId2)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ShopHelper sh = new Dal.ShopHelper();

            List<Dal.ProductCatalogAttributesForShopResult> attributesForShop = sh.GetProductCatalogAttributesForShop(shop, productCatalogId);
            List<Dal.ProductCatalogAttributesForShopResult> attributesForShop2 = sh.GetProductCatalogAttributesForShop(shop, productCatalogId2);

            //List<ProductCatalogAttributesForProductPrevResult> attributes = pch.GetProductCatalogAttributesForProduct(productCatalogId);

            Dal.ProductCatalogView pc = pch.GetProductCatalogView(productCatalogId);
            Dal.ProductCatalogView pc2 = pch.GetProductCatalogView(productCatalogId2);

            StringBuilder sb = new StringBuilder();
            sb.Append("<table><th width='200'/><th width='300'/><th width='300'/>");

            sb.Append(String.Format(@"<tr><td></td><td><a href='/ProductCatalog.Preview.aspx?id={0}' target='_blank'><img src='/images/ProductCatalog/{1}' width='150'></td>" +
                "<td><a href='/ProductCatalog.Preview.aspx?id={2}' target='_blank'><img src='/images/ProductCatalog/{3}' width='150'></td></tr>", 
                pc.ProductCatalogId, pc.ImageFullName, pc2.ProductCatalogId, pc2.ImageFullName));
            sb.Append(String.Format("<tr><td><b>Producent: </b></td><td>{0}</td><td>{1}</td></tr>", pc.SupplierName, pc2.SupplierName));
            sb.Append(String.Format("<tr><td><b>Kod produktu: </b></td><td>{0}</td><td>{1}</td></tr>", pc.Code, pc2.Code));


            List<Dal.ProductCatalogAttributesForShopResult> attributes = new List<ProductCatalogAttributesForShopResult>() ;
            attributes.AddRange(attributesForShop);
            attributes.AddRange(attributesForShop2);

            var groups = attributes.OrderBy(x => x.Order).Select(x => new
            {
                x.AttributeGroupId,
                AttributeGroupName = x.GroupName,
                x.AttributeGroupTypeId
            }).Distinct().ToList();

            //if (!preview)
            //    groups = groups.Where(x => x.ExportToAllegro).ToList();

            foreach (var g in groups)
            {

                var products   = attributesForShop.Where(x => x.AttributeGroupId == g.AttributeGroupId).ToList();
                var products2  = attributesForShop2.Where(x => x.AttributeGroupId == g.AttributeGroupId).ToList();

                List<string> rowsAtt = new List<string>(); 
                
                switch (g.AttributeGroupTypeId)

                {
                    case 1:
                        sb.Append(String.Format("<tr><td><b>{0}</b></td><td>{1}</td><td>{2}</td></tr>", 
                            g.AttributeGroupName, GetValue(products), GetValue(products2)));                         
                        break;
                    case 2:
                        string productsCommaSep = String.Join(", ", products.OrderByDescending(x => x.IsDefault).Select(x => x.AttName).Distinct().ToArray());
                        string productsCommaSep2 = String.Join(", ", products2.OrderByDescending(x => x.IsDefault).Select(x => x.AttName).Distinct().ToArray());
                        sb.Append(String.Format("<tr><td><b>{0}</b></td><td>{1}</td><td>{2}</td></tr>",
                            g.AttributeGroupName, GetStringValue(productsCommaSep), GetStringValue(productsCommaSep2)));
                        break;
                    case 3:

                        List<Dal.ProductCatalogAttributesForShopResult> prod = new List<ProductCatalogAttributesForShopResult>();
                        prod.AddRange(products);
                        prod.AddRange(products2);

                        string str = "";

                        /*
                            String.Join("", rowsAtt.ToArray()),
                            String.Join("", rowsAtt2.ToArray())
                        */
                        foreach (var p in prod.Select(x=>new { x.AttName, x.AttributeId, x.AttributeTypeId }).Distinct())
                        {
                            str = "<tr><td>" + p.AttName + "</td><td>{0}</td><td>{1}</td></tr>";

                            switch (p.AttributeTypeId.Value)
                            {
                                case 1:
                                    str = String.Format(str, GetFieldValue(products, p.AttributeId), GetFieldValue(products2, p.AttributeId));
                                    //rowsAtt.Add(String.Format( "<li>{0}: {1}</li>" + Environment.NewLine, p.AttName,  GetFieldValue(products,  p)));
                                    //rowsAtt2.Add(String.Format("<li>{0}: {1}</li>" + Environment.NewLine, p.AttName, GetFieldValue(products2, p)));
                                    break;
                                case 2:
                                    str = String.Format(str, GetFieldValue(products, p.AttributeId), GetFieldValue(products2, p.AttributeId));
                                    //rowsAtt.Add(String.Format("<li>{0}: {1}</li>" + Environment.NewLine, p.AttName,  GetFieldValue(products, p )));
                                    //rowsAtt2.Add(String.Format("<li>{0}: {1}</li>" + Environment.NewLine, p.AttName, GetFieldValue(products2, p)));
                                    break;
                            }
                            rowsAtt.Add(str);
                        }
                        sb.Append(String.Format(@"<tr><td><b>{0}: </b></td><td></td><td></td></tr>"
                            , g.AttributeGroupName));
                        sb.Append(String.Format(@"<tr><td colspan='3'>
                            <table><th width='200'/><th width='300'/><th width='300'/>{0}</table>
                        </td></tr>" 
                            , String.Join("", rowsAtt)));

                        break;
                }
            }
            sb.Append("</table>");

            return sb.ToString();
        }

        private static string GetStringValue(string str)
        {
            if (String.IsNullOrEmpty(str))
                return "-";
            return str;

        }

        private static string GetValue(List<ProductCatalogAttributesForShopResult> products)
        {
            if (products != null && products.Count()>0)
                return products[0].AttName;
            return "-";
        }

        public static string GetPreviewErli(Dal.Helper.Shop shop, int productCatalogId, bool preview, bool removeHtmlTags)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ShopHelper sh = new Dal.ShopHelper();

            List<Dal.ProductCatalogAttributesForShopResult> attributesForShop = sh.GetProductCatalogAttributesForShop(shop, productCatalogId);

            //List<ProductCatalogAttributesForProductPrevResult> attributes = pch.GetProductCatalogAttributesForProduct(productCatalogId);

            Dal.ProductCatalogView pc = pch.GetProductCatalogView(productCatalogId);

            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("<p><b>Producent: </b>{0}</p>" , pc.SupplierName));
            if (!String.IsNullOrEmpty(pc.Code))
                sb.Append(String.Format("<p><b>Kod produktu: </b>{0}</p>" , pc.Code));


            var groups = attributesForShop.OrderBy(x => x.Order).Select(x => new
            {
                x.AttributeGroupId,
                AttributeGroupName = x.GroupName,
                x.AttributeGroupTypeId

            }).Distinct().ToList();

            //if (!preview)
            //    groups = groups.Where(x => x.ExportToAllegro).ToList();

            foreach (var g in groups)
            {

                var products = attributesForShop.Where(x => x.AttributeGroupId == g.AttributeGroupId).ToList();

                List<string> rowsAtt = new List<string>();
                //string cssClass = "";// !g.ExportToAllegro ? "class='grayedout'" : "";
                switch (g.AttributeGroupTypeId)

                {
                    case 1:
                        sb.Append(String.Format("<p><b>{0}: </b>{1}</p>" , g.AttributeGroupName, products[0].AttName));
                        break;
                    case 2:
                        string productsCommaSep = String.Join(", ", products.OrderByDescending(x => x.IsDefault).Select(x => x.AttName).Distinct().ToArray());
                        sb.Append(String.Format("<p><b>{0}</b>: {1}</p>" , g.AttributeGroupName, productsCommaSep));
                        break;
                    case 3:

                        foreach (var p in products)
                        {
                            switch (p.AttributeTypeId.Value)
                            {
                                case 1:
                                    rowsAtt.Add(String.Format("<li>{0}: {1}</li>" , p.AttName, GetFieldValue2(p, p.DecimalValue)));
                                    break;
                                case 2:
                                    rowsAtt.Add(String.Format("<li>{0}: {1}</li>" , p.AttName, GetFieldValue2(p, p.StringValue)));
                                    break;
                            }
                        }
                        sb.Append(String.Format("<p><b>{0}: </b></p><ul>{1}</ul>" , g.AttributeGroupName, String.Join("", rowsAtt.ToArray())));

                        break;
                }



            }


            if (removeHtmlTags)
            {
                return Regex.Replace(sb.ToString(), "<.*?>", String.Empty);
            }

            return sb.ToString();
        }
        private static object GetFieldValue(List<Dal.ProductCatalogAttributesForShopResult> products, int attributeId)
        {
            var p = products.Where(x => x.AttributeId == attributeId).FirstOrDefault();
            if (p == null)
                return "-";

            if (p.FieldTemplate == null)
                return p.StringValue;
            else
                return String.Format(p.FieldTemplate, p.DecimalValue);
        }
        private static object GetFieldValue2( ProductCatalogAttributesForShopResult p, object v)
        {
        

            if (p.FieldTemplate == null)
                return v;
            else
                return String.Format(p.FieldTemplate, v);
        }


        //private string GetProductCatalogPreviewSpecificationFromAtt(Dal.ProductCatalogView pc, Dal.Supplier supplier)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    List<ProductCatalogAttributesForProductPrevResult> attributes = pch.GetProductCatalogAttributesForProduct(pc.ProductCatalogId);

        //    StringBuilder sb = new StringBuilder();
        //    string tableRow1 = "<tr class='offer_spec_row'><td class='offer_spec_title' rowspan='{2}'>{0}</td>{1}</tr>";
        //    string tableRow2 = "<td class='offer_spec_value'>{0}</td>";
        //    string tableRow3 = "<tr class='offer_spec_row'>{0}</tr>";

        //    if (supplier.ShowSupplierInAllegro.HasValue && supplier.ShowSupplierInAllegro.Value)
        //        sb.Append(String.Format(tableRow1, "Producent", String.Format(tableRow2, supplier.Name), 1));
        //    if (!String.IsNullOrEmpty(pc.Code))
        //        sb.Append(String.Format(tableRow1, "Kod produktu", String.Format(tableRow2, pc.Code), 1));


        //    var groups = attributes.OrderBy(x=>x.AllegroOrder).Select(x => new 
        //    {
        //        AttributeGroupId = x.AttributeGroupId,
        //        AttributeGroupName = x.AttributeGroupName,
        //        AttributeGroupTypeId = x.AttributeGroupTypeId

        //    }).Distinct().ToList();


        //    foreach(var g in groups)
        //    {
        //        int rowSpan = 1;

        //        var products = attributes.Where(x => x.AttributeGroupId == g.AttributeGroupId).ToList();
        //        string rows = "";
        //        List<string> rowsAtt = new List<string>();

        //        switch (g.AttributeGroupTypeId)

        //        {
        //            case 1:
        //                rows += String.Format(tableRow2, String.Format("{0}", products[0].AttributeName ));
        //                rowSpan = 1;
        //                sb.Append(String.Format(tableRow1, g.AttributeGroupName, rows, rowSpan));
        //                break;
        //            case 2:
        //                rowSpan = 1;
        //                rows += String.Format(tableRow2, String.Format("{0}", String.Join(", ", products.OrderByDescending(x=>x.IsDefault).Select(x=>x.AttributeName).Distinct().ToArray())));
        //                sb.Append(String.Format(tableRow1, g.AttributeGroupName, rows, rowSpan));
        //                break;
        //            case 3:
        //                rowSpan = products.Count;

        //                foreach (var p in products)
        //                {
        //                    switch(p.AttributeTypeId.Value)
        //                    {
        //                        case 1:
        //                            rowsAtt.Add(String.Format(tableRow2, String.Format("{0}: {1:0.##}", p.AttributeName, p.DecimalValue)));
        //                            break;
        //                        case 2:
        //                            rowsAtt.Add(String.Format(tableRow2, String.Format("{0}: {1}", p.AttributeName, p.StringValue)));
        //                            break;

        //                    }



        //                }
        //                sb.Append(String.Format(tableRow1, g.AttributeGroupName, rowsAtt[0], rowSpan));
        //                foreach (var r in rowsAtt.Skip(1).ToList())
        //                    sb.Append(String.Format(tableRow3, r));

        //                break;
        //        }



        //    }






        //    return sb.ToString(); 
        //}
        public static string SearchAndReplace(string input, string output)
        {
            string pattern = String.Format(@"\[{0} {1}='(.*)'\](.*)\[/{0}\]", "SEARCH", "QUERY");             

            string href = Regex.Replace(input, pattern, output);


            return href;

 
        }
        //private string GetProductCatalogPreviewImages(bool isPreviewMode, int productCatalogId, int imageTypeId)
        //{
        //    StringBuilder sb = new StringBuilder();

        //  string imageTemplate =   @"<div class=""offer_main_photo_frame"">
        //        {3}<img src=""/images/productcatalog/{0}""
        //            alt=""{1}"" title=""{6}"" />{4}
        //            <div class=""offer_main_photo_comment"">{2}</div>
        //    </div>";
        //    if(!isPreviewMode)
        //         imageTemplate = @"<div class=""offer_main_photo_frame"">
        //        {3}<img src=""http://{5}/ProductCatalog/{0}""
        //            alt=""{1}"" />{4}
        //            <div class=""offer_main_photo_comment"">{2}</div>
        //    </div>";
             


        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    List<Dal.ProductCatalogImage> images = oh.GetProductCatalogImages(productCatalogId).Where(x => x.IsActive /*&& x.ProductCatalog.IsReady */&& x.ImageTypeId == imageTypeId).ToList();

        //    foreach (Dal.ProductCatalogImage image in images)
        //    {
        //        string imageUrl = imageTemplate;
                 
        //        if (String.IsNullOrEmpty(image.LinkUrl))
        //            imageUrl =
        //                 String.Format(imageTemplate, 
        //                    image.FileName,
        //                    image.Description.Replace(@"""", @""""""),
        //                    image.Description,
        //                    "",
        //                    "", 
        //                    Dal.Helper.StaticLajtitUrl,
        //                    image.Title
        //                 );
        //        else
        //            imageUrl =
        //                 String.Format(imageTemplate, 
        //                    image.FileName,
        //                    image.Description.Replace(@"""", @""""""),
        //                    image.Description,
        //                    String.Format(@"<a href=""{0}"" target=""_blank"">", image.LinkUrl),
        //                    "</a>",
        //                    Dal.Helper.StaticLajtitUrl,
        //                    image.Title
        //                 );
        //        sb.AppendLine(imageUrl);

        //    }

        //  return sb.ToString();
        //}

        public decimal SetOrderPayments(string[] trackingNumbers, DateTime paymentDate, int paymentTypeId, int shippingCompanyId, string actingUser)
        {

            if (trackingNumbers.Count() == 0)
                return 0 ;

            List<Dal.OrdersNotPaidButSentView> orders = oh.GetOrdersNotPaidButSent(shippingCompanyId).Where(x => trackingNumbers.Contains(x.ShipmentTrackingNumber)).ToList();

            List<Dal.OrderPayment> payments = new List<Dal.OrderPayment>();
            decimal sum = 0;

            foreach (Dal.OrdersNotPaidButSentView o in orders)
            {
                payments.Add(new Dal.OrderPayment()
                {
                    Amount = o.COD.Value,// -o.AmountBalance.Value,
                    Comment = "Płatność dodana w batchu",
                    ExternalPaymentId = "",
                    InsertDate = paymentDate,
                    OrderId = o.OrderId,
                    PaymentTypeId = paymentTypeId,
                    InsertUser = actingUser,
                    CurrencyCode="PLN",
                    CurrencyRate=1,
                    AmountCurrency = -o.AmountBalance.Value

                });
                sum += -o.AmountBalance.Value;
            };

            oh.SetOrderPayments(payments);

            return sum ;
        }

        public Dal.Supplier GetSupplier(int idSupplier)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            return oh.GetSupplier(idSupplier);
        }


        internal Dal.Invoice GetInvoice(int orderId)
        {

            List<Dal.OrderProductsView> products = oh.GetOrderProducts(orderId);
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);
            Dal.Invoice invoice = order.Invoice;
            List<Dal.InvoiceProduct> invoiceProducts = new List<Dal.InvoiceProduct>();


            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            Dal.Settings s = sh.GetSetting("INV_TEMPL");

            #region Create InvoiceProducts
            foreach (Dal.OrderProductsView product in products.Where(x => x.Quantity > 0).ToList())
            {

                string productName = "";

                if (product.IsOrderProduct == 1)
                    productName = Mixer.GetProductName(s.StringValue, product.ProductCatalogId.Value);
                else
                    productName = "Przesyłka";
                
                decimal rebate = 1 - product.Rebate / 100M;
                // decimal brutto = product.Price.Value * rebate;
                //  decimal netto = brutto / (1 + product.VAT);
                //  decimal vatValue = brutto - netto;


                Dal.InvoiceProduct invoiceProduct = new Dal.InvoiceProduct()
                {
                    InvoiceId = invoice.InvoiceId,
                    MeasureName = "szt.",
                    Name = productName,
                    PriceBrutto = product.Price,
                    ProductCatalogId = GetNullableInt(product.ProductCatalogId),
                    Quantity = product.Quantity,
                    VatRate = product.VAT,
                    Rebate = product.Rebate,
                };

                invoiceProducts.Add(invoiceProduct);

            }
            #endregion
            #region Shipping
            //if (order.ShippingCost > 0)
            //{
            //    Dal.InvoiceProduct invoiceProduct = new Dal.InvoiceProduct()
            //    {
            //        InvoiceId = invoice.InvoiceId,
            //        MeasureName = "szt.",
            //        Name = "Przesyłka",
            //        PriceBrutto = order.ShippingCost ,
            //        ProductCatalogId = null,
            //        Quantity = 1,
            //        VatRate = order.ShippingCostVAT,
            //        Rebate = 0
            //    };

            //    invoiceProducts.Add(invoiceProduct);

            //}
            #endregion

            oh.SetInvoiceProducts(invoice.InvoiceId, invoiceProducts);


            return Dal.DbHelper.Orders.GetOrder(orderId).Invoice;
        }

        private int? GetNullableInt(int? productCatalogId)
        {
            if (productCatalogId.HasValue)
                return productCatalogId.Value;
            else
                return null;
        }



        //public void ExportForDpd(int[] orderIds, DirectoryInfo di, string actingUser)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    List<Dal.Order> orders = oh.GerOrders(orderIds);
        //    Bll.DpdHelper pph = new DpdHelper();
        //    List<Dal.Order> ordersToFile = new List<Dal.Order>();

        //    int[] shippingIdNotToExport = oh.GetShippingTypes().Where(x => !x.ExportToFile).Select(x => x.ShippingTypeId).ToArray();

        //    ordersToFile = orders.Where(x => !shippingIdNotToExport.Contains(x.ShippintTypeId)).ToList();

        //    string fileName = pph.ExportOrders(ordersToFile, di);

        //    oh.SetOrdersStatus(Dal.Helper.ShippingCompany.InPost, orderIds, Dal.Helper.OrderStatus.Exported, "Export automatyczny", actingUser, fileName); 
        //}
    }
}
