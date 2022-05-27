using LajtIt.Dal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LajtIt.Bll
{
    public class EmailEditor
    {

        public void SendEmails(List<LajtIt.Bll.Dto.Email> emails)
        {

        }

//        private string GetFooter()
//        {
//            return @"<div style='color: gray;'>
//            Pozdrawiam<br>
//Jacek Stawicki<br>
//tel. 604 688 227, GG: 3459943<br><br>
//
//Lajt it. Design you like.<br>
//www: <a href='http://www.lajtit.pl/'>www.lajtit.pl</a><br>
//FaceBook: <a href='http://www.facebook.com/lajtit/'>www.facebook.com/lajtit</a><br>
//Blog: <a href='http://lajtit.wordpress.com/'>lajtit.wordpress.com</a><br></div>";
//        }
        public void SendOrderStatusNotification(int orderId, Dal.OrderStatus orderStatus , string actingUser)
        {
            int emailTemplateId = orderStatus.SendEmailTemplateId.Value;

            Dal.DalHelper dal = new Dal.DalHelper();
            Dal.EmailTemplates emailTemplate = dal.GetEmailTemplate(emailTemplateId);
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);


            Bll.EmailSender emailSender = new EmailSender();

            string body = GetStatusNotificationEmailBody(orderId, emailTemplate, order);

            Dto.Email email = new Dto.Email()
            {
                Body = body,
                FromEmail = emailTemplate.FromEmail,
                FromName = emailTemplate.FromName,
                Subject = emailTemplate.Subject.Replace("[ORDER_ID]", String.Format("#1908{0}", orderId)),
                ToEmail = order.Email,
                ToName = String.Format("{0} {1}", order.ShipmentFirstName, order.ShipmentLastName)
            };
            string fileName = null;

            if (orderStatus.OrderStatusId==(int)Dal.Helper.OrderStatus.Sent )
            {
                if (order.Invoice != null)
                    email.AttachmentFile = String.Format(@"{0}\{1}", System.Web.HttpContext.Current.Server.MapPath("/Files/Invoices"), order.Invoice.InvoiceFileName);
                else
                {
                    Bll.OrderHelper boh = new Bll.OrderHelper();
                    if (boh.CreateParagon(orderId))
                    {
                        PDF pdf = new PDF(System.Web.HttpContext.Current.Server.MapPath("/Images"), System.Web.HttpContext.Current.Server.MapPath("/Files"));

                      
                         fileName = pdf.CreateParagon(orderId);
                        email.AttachmentFile = fileName;


                    }
                }
            }
            emailSender.SendEmail(email);


            Dal.OrderStatusHistory osh = new Dal.OrderStatusHistory()
            {
                Comment = String.Format("{0}",
                 body.Replace('\n', ' ')),
                InsertDate = DateTime.Now,
                InsertUser = actingUser,
                OrderId = orderId,
                OrderStatusId = (int)Dal.Helper.OrderStatus.Comment

            };
            oh.SetOrderStatus(osh, null);
             osh = new Dal.OrderStatusHistory()
            {
                Comment = "Wysłano email",
                InsertDate = DateTime.Now,
                InsertUser = actingUser,
                OrderId = orderId,
                OrderStatusId = (int)Dal.Helper.OrderStatus.Comment

            };
            oh.SetOrderStatus(osh, null);

            try
            {

                File.Delete(fileName);
            }
            catch(Exception ex)
            {
               // Dal.ErrorHandler.LogError(ex, fileName);
            }
        }
        public string GetStatusNotificationEmailBody(int orderId, int emailTemplateId)
        {
            Dal.DalHelper dal = new Dal.DalHelper();
            Dal.EmailTemplates emailTemplate = dal.GetEmailTemplate(emailTemplateId);
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            return GetStatusNotificationEmailBody( orderId,  emailTemplate,  order);
        }
        private string GetStatusNotificationEmailBody(int orderId, Dal.EmailTemplates emailTemplate, Dal.Order order)
        {


            string body = emailTemplate.Body;
            body = body.Replace("[USER_FIRSTNAME]", order.ShipmentFirstName);
            body = body.Replace("[USER_LASTNAME]", order.ShipmentLastName);
            body = body.Replace("[EMAIL]", order.Email);
            body = body.Replace("[SHIPMENT_TYPE]",

              String.Format("{0}<br>{1} {2:C}", GetDeliveryDecription(order),
              order.OrderShipping.COD == null ? "" : "płatność przy odbiorze",
              order.OrderShipping.COD == null ? null : order.OrderShipping.COD));
            //    GetShipmentDays(order.ShippingType, order.ShippingType.DeliveryNumberOfDays)); ;

            // = ov.ShippingCompany;
            //lblShippingMode.Text = ov.ShippingType;
            //if (ov.PayOnDelivery.HasValue)
            //    lblPayOnDelivery.Visible = ov.PayOnDelivery.Value;
            //if (ov.SendFromExternalWerehouse.HasValue && ov.SendFromExternalWerehouse.Value)
            //    lblSendFromExternalWerehouse.Visible = true;

            //if (ov.ShippingServiceModeId == (int)Dal.Helper.ShippingServiceMode.Showroom)
            //    lblShippingCompany.Visible = false;


            body = body.Replace("[ORDER_PRODUCTS_TABLE]", GetOrderProductsTable(order));
            body = body.Replace("[SUGGESTIONS]", GetOrderSuggestions(orderId));
            body = body.Replace("[LINKS]", GetLinks(orderId));
            body = body.Replace("[ORDER_TOTAL]", String.Format("{0:C}", order.AmountToPay));
            body = body.Replace("[SHIPMENT_COST]", String.Format("{0:C}", order.ShippingCost));
            body = body.Replace("[ORDER_ID]", String.Format("#1908{0}", orderId));


            if (order.OrderShipping.ShippingServiceModeId == (int)Dal.Helper.ShippingServiceMode.Point)
            {
                Dal.PaczkomatyHelper ph = new Dal.PaczkomatyHelper();
                //string[] shippingData = order.ShippingData.Split('|');
                Dal.PaczkomatyView p = ph.GetPaczkomaty().Where(x => x.Name.ToUpper() == order.OrderShipping.ServicePoint).FirstOrDefault();
                if (p != null)
                    body = body.Replace("[SHIPPING_ADDRESS]", String.Format("Paczkomat InPost:<br><br><b>{0}</b><br>Telefon: {1}",
                    p.Description,
                    order.Phone));
                else
                    body = body.Replace("[SHIPPING_ADDRESS]", "");
            }
            else
            {

                body = body.Replace("[SHIPPING_ADDRESS]", String.Format("{0} {1} {2}<br>{3}<br>{4} {5}<br>Telefon: {6}",
                    order.ShipmentCompanyName,
                    order.ShipmentFirstName,
                    order.ShipmentLastName,
                    order.ShipmentAddress,
                    order.ShipmentPostcode,
                    order.ShipmentCity,
                    order.Phone));
            }
            //            if (order.ShippintTypeId == 1 || order.ShippintTypeId == 2)
            //                body = body.Replace("[SHIPPING_INFO]", @" - <i class='small_text'>wybór listu poleconego oznacza, iż zamówione lampy zostaną przesłane do samodzielnego złożenia. 
            //Jeśli życzą sobie Państwo otrzymać je złożone i w krótszym czasie, prosimy o wybór przesyłki kurierskiej (dostawa w jeden dzień).</i>");
            //            else
            body = body.Replace("[SHIPPING_INFO]", @"");


            //if ((Dal.Helper.OrderStatus)order.OrderStatusId == Dal.Helper.OrderStatus.WaitingForPayment
            //|| (Dal.Helper.OrderStatus)order.OrderStatusId == Dal.Helper.OrderStatus.WaitingForClient)
            {
                body = body.Replace("[NOTIFICATION_LIST]", GetOrderNotifications(order));
            }

            if (order.OrderShipping!=null && !String.IsNullOrEmpty(order.OrderShipping.ShipmentTrackingNumber))
            {
                string url = "";
                if (order.OrderShipping != null && order.OrderShipping.ShippingCompany != null)
                {
                    url = String.Format(order.OrderShipping.ShippingCompany.TrackingUrl, order.OrderShipping.ShipmentTrackingNumber);
                }

                body = body.Replace("[SHIPPING_TRACKING_NUMBER]",
                    String.Format("<p>Sprawdź status przesyłki: <a href='{1}' target='_blank'>{0} ({2})</a>.</p>",
                    order.OrderShipping.ShipmentTrackingNumber, url, order.OrderShipping.ShippingCompany.Name));
            }
            else
                body = body.Replace("[SHIPPING_TRACKING_NUMBER]", "");


            if (order.Shop.ShopTypeId != (int)Dal.Helper.ShopType.Allegro)
            {
                decimal orderAmount = order.AmountToPay - order.ShippingCost;
                decimal rebate = GetShopRebates(orderAmount);
                if (rebate == 0)
                    body = body.Replace("[SHOP_REBATES]", "");
                else
                    body = body.Replace("[SHOP_REBATES]", String.Format(@"Składając zamówienie w sklepie <a href='https://lajtit.pl/rabaty_na_zakup_oswietlenia_lamp' target='_blank'>www.lajtit.pl</a> o wartości {0:C} można uzyskać rabat w wysokości maksymalnej {1:C}. 
Przejdź do sklepu <a href='https://lajtit.pl/rabaty_na_zakup_oswietlenia_lamp' target='_blank'>www.lajtit.pl</a> i sprawdź!", orderAmount, rebate));
            }
            else
                body = body.Replace("[SHOP_REBATES]", "");


            //body = body + GetFooter();
            return body;
        }

        private string GetDeliveryDecription(Order order)
        {
            string str = "";
            switch (order.OrderShipping.ShippingServiceModeId)
            {
                case (int)Dal.Helper.ShippingServiceMode.Showroom:
                    str = "Odbiór w salonie Lajtit<br>ul. Przewodnia 16, 93-419 Łódź";break;
                    default:
                            str = String.Format("{0} {1}", order.OrderShipping.ShippingServiceMode.Name, order.OrderShipping.ShippingCompany.Name); break;
            }
            return str;
        }

        private decimal GetShopRebates(decimal amount)
        {
           return  Bll.ShopHelper .GetShopRebate(amount);
        }

        private string GetLinks(int orderId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.ProductCatalogLink> productCatalogLinks = oh.GetProductCatalogLinks(orderId);
            var links = productCatalogLinks.Select(x => new { Url = x.Url, Name = x.Name }).Distinct();

            StringBuilder sb = new StringBuilder();
            foreach (var link in links)
            {
                sb.Append(String.Format("<li><a href='{0}' target='_blank'>{1}</a></li>", link.Url, link.Name));
            }

            if (links.Count() > 0)
                return String.Format("<p>Przydatne informacje: <ul>{0}</ul></p>", sb.ToString());
            else
                return "";
        }

        private string GetOrderNotifications(Dal.Order order)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.OrderNotificationsView> notifications = oh.GetOrderNotifications(order.OrderId);

            StringBuilder sb = new StringBuilder();

            foreach (Dal.OrderNotificationsView notification in notifications)
            {
                switch (notification.NotificationTypeId)
                {
                    case 0:
                        sb.AppendLine(String.Format("<li>{0}</li>", notification.Comment));
                        break;
                    case 6:
                    case 7:
                        sb.AppendLine(String.Format(notification.NameForClient, order.AmountToPay));
                        break;
                    case 15:
                        Bll.PaczkomatyHelper ph = new PaczkomatyHelper();
                        string paczkomaty = ph.GetPaczkomatyByPostcode(order.ShipmentPostcode);

                        sb.AppendLine(String.Format(notification.NameForClient, paczkomaty));
                        break;
                    default:
                        sb.AppendLine(String.Format("<li>{0}</li>", notification.NameForClient));
                        break;

                }

            }


            return sb.ToString();

        }

        //private string GetShipmentDays(Dal.ShippingType st, int days)
        //{
        //    switch (st.ShippingTypeId)
        //    {
        //        case 0:
        //            return "nie wybrano"; break;
        //        case 5:
        //        case 6: return "Odbiór osobisty w salonie Lajtit"; break;
        //        default:

        //            if (days == 1)
        //                return String.Format("{0} (dostawa w ciągu {1} dnia roboczego od momentu wysłania)", st.Name, days);
        //            else
        //                return String.Format("{0} (dostawa w ciągu {1} dni roboczych od momentu wysłania)", st.Name, days);
        //            break;
        //    }
        //}

        private string GetOrderSuggestions(int orderId)
        {
            StringBuilder text = new StringBuilder();

            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<Dal.OrderProductsSuggestionsResult> suggestions = oh.GerOrderProductSuggestions(Dal.Helper.Shop.Lajtitpl, orderId);


            if (suggestions.Count > 0)
            {
                text.Append("<p>Nasi klienci kupujący to co Ty, wybrali również:<table  cellspacing='0' cellpadding='2' rules='all' border='1' class='ship_table' >");

                text.Append("<tr>");
                foreach (Dal.OrderProductsSuggestionsResult suggestion in suggestions)
                {
                    text.Append(String.Format("<td class='sugg_name' style='width:{1}%'><b>{0}</b></td>", suggestion.Name, 100/suggestions.Count));
                }
                text.Append("</tr>");
                text.Append("<tr>");
                foreach (Dal.OrderProductsSuggestionsResult suggestion in suggestions)
                {
                    text.Append(String.Format("<td class='sugg_img'><a href='http://lajtit.pl/pl/p/p/{0}'><img src='http://{2}/ProductCatalog/{1}' style='width:100px; border:0px;'/></a></td>", suggestion.ShopProductId, suggestion.ImageUrl, Dal.Helper.StaticLajtitUrl));
                }
                text.Append("</tr>");
                text.Append("<tr>");
                foreach (Dal.OrderProductsSuggestionsResult suggestion in suggestions)
                {
                    text.Append(String.Format("<td class='sugg_price'>{0:C}</td>",  suggestion.PriceBruttoMinimum));
                }
                text.Append("</tr>");


                text.Append("</table>W przypadku zainteresowania, prosimy o kontakt mailowy lub telefoniczny.</p>");
            }
            return text.ToString();
        }

        private string GetOrderProductsTable(Dal.Order order)
        {
            StringBuilder sb = new StringBuilder();
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.OrderProductsView> products = oh.GetOrderProducts(order.OrderId)
                .Where(x => x.Quantity > 0).ToList();
            int i = 1;
            decimal total = 0;

            //if (order.ShippingCost > 0)
            //    products.Add(new Dal.OrderProductsView
            //    {
            //        Quantity = 1,
            //        Rebate = 0,
            //        Price = order.ShippingCost,
            //        ProductName = order.ShippingType.Name

            //    });

            sb.AppendLine(@"<table cellspacing='0' cellpadding='2' rules='all' border='1' class='ship_table' >
<tr class='ship_header' ><td style='width:30px;'>Lp.</td><td style=''>Nazwa produktu</td><td style='width:30px;'>L.szt</td>
<td style='width:50px;'>Rabat</td><td style='width:80px;'>Cena</td><td style='width:80px;'>Razem</td></tr>");
            foreach (Dal.OrderProductsView product in products.ToList())
            {
                decimal tmp = product.Quantity * product.Price * (100.00M - product.Rebate) / 100.00M;
                total += tmp;

                sb.AppendLine(String.Format(@"<tr><td style='text-align:center'>{0}</td><td>{1}</td><td style='text-align:center'>{2}</td>
                    <td style='text-align:right'>{3:0.00}%</td><td style='text-align:right'>{4:C}</td><td style='text-align:right'>{5:C}</td></tr>",
                    i++,
                    GetProductName(product),
                    product.Quantity,
                    product.Rebate,
                    product.Price,
                   tmp));
            }
            sb.AppendLine(String.Format("<tr><td></td><td></td><td></td><td></td><td></td><td style='text-align:right'>{0:C}</td></tr>", total));
            sb.AppendLine("</table>");
            return sb.ToString();
        }

        private string GetProductName(Dal.OrderProductsView product)
        {
            string name = "";
            switch (product.ShopTypeId)
            {
                case 0:
                    name =  String.Format(product.ProductName); break;
                case 1:
                    name = String.Format("<a href='http://allegro.pl/show_item.php?item={0}'>{1}</a>",
                        product.ExternalProductId, product.ProductName); break;
                default:
                    name = product.CatalogName; break;
            }

            if (!String.IsNullOrEmpty(product.Code))
                name = String.Format("{0}<br>({1})", name, product.Code);

            return name;

        }

        //public void SendAllegroComments()
        //{
        //    Dal.DalHelper dal = new Dal.DalHelper();
        //    Dal.EmailTemplates emailTemplate = dal.GetEmailTemplate((int)Dal.Helper.EmailTemplate.AllegroComment);

        //    List<Dal.AllegroCommentsToSendView> comments = dal.GetCommentsToSend();

          
        //    Bll.EmailSender emailSender = new EmailSender();

        //    foreach (Dal.AllegroCommentsToSendView comment in comments.ToList())
        //    {
        //        string body = emailTemplate.Body;
        //        body = body.Replace("[COMMENT]", comment.Comment);
        //        body = body.Replace("[USER_NAME]", comment.AuthorUserName);
        //        body = body.Replace("[USER_FIRSTNAME]", comment.FirstName);
        //        body = body.Replace("[USER_LASTNAME]", comment.LastName);
        //        body = body.Replace("[EMAIL]", comment.Email);
        //        body = body.Replace("[AUCTION_NAME]", comment.Name);

        //        if (body.Contains("[REBATE_CODE]"))
        //        {
                    
        //        }
        //        emailSender.SendEmail(new Dto.Email()
        //        {
        //            Body = body,
        //            FromEmail = emailTemplate.FromEmail,
        //            FromName = emailTemplate.FromName,
        //            Subject = emailTemplate.Subject,
        //            ToEmail = comment.Email,
        //            ToName = String.Format("{0} {1}", comment.FirstName, comment.LastName)

        //        });

        //        dal.SetEmailSentFlag(comment.CommentId);

        //    }
        //}

//        internal void NotifyAboutNewItems(Dal.AllegroUser allegroUser, List<Dal.AllegroItem> insertedItems)
//        {
//            if (!allegroUser.Follow || insertedItems.Count == 0)
//                return;


//            Dal.DalHelper dal = new Dal.DalHelper();
//            Dal.EmailTemplates emailTemplate = dal.GetEmailTemplate((int)Dal.Helper.EmailTemplate.AllegroNewItems);


//            string body = emailTemplate.Body;
//            body = body.Replace("[USER_NAME]", allegroUser.UserName);
//            emailTemplate.Subject = String.Format(emailTemplate.Subject, allegroUser.UserName);

//            int i = 1;
//            StringBuilder sb = new StringBuilder();
//            foreach (Dal.AllegroItem ai in insertedItems)
//            {
//                sb.Append(String.Format(@"<tr><td>{5}</td><td><a href='http://allegro.pl/show_item.php?item={4}'>{0}</a></td><td>{1}</td>
//<td style='text-align:right;'>{2:C}</td><td>{3:yyyy/MM/dd HH:mm}</td></tr>",
//                ai.Name, GetCategoryName(ai), ai.BuyNowPrice, ai.EndingDateTime, ai.ItemId, i++));

//            }


//            body = body.Replace("[PLACEHOLDER]", String.Format("<table><tr><td>Lp.</td><td>Nazwa</td><td>Kategoria</td><td>Cena</td><td>Koniec</td></tr>{0}</table>", sb.ToString()));


//            Bll.EmailSender emailSender = new EmailSender();
//            emailSender.SendEmail(new Dto.Email()
//            {
//                Body = body,
//                FromEmail = emailTemplate.FromEmail,
//                FromName = emailTemplate.FromName,
//                Subject = emailTemplate.Subject,
//                ToEmail = Dal.Helper.DevEmail,
//                ToName = Dal.Helper.MyEmail

//            });
//        }

        //private string GetCategoryName(Dal.AllegroItem ai)
        //{
        //    if (ai.AllegroCategory != null)
        //        return ai.AllegroCategory.Name;
        //    else
        //        return "";
        //}
    }
}
