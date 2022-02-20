

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using LajtIt.Dal;
using static LajtIt.Bll.AllegroRESTHelper.DraftError;

namespace LajtIt.Bll
{
    public partial class AllegroRESTHelper
    {
        public class DraftError
        {
            public class Error
            {
                public string code { get; set; }
                public string message { get; set; }
                public string details { get; set; }
                public string path { get; set; }
                public string userMessage { get; set; }
            }

            public class RootObject
            {
                public List<Error> errors { get; set; }
                public DateTime validatedAt { get; set; }
            }

        }
    public class Draft
        {
            public int Id { get; set; }
            public int AllegroCategoryId { get; set; }
            public int ProductCatalogId { get; set; }
            public long? ItemId { get; set; }
            public string ValidationErrors { get; set; }
            public bool IsValid { get; set; }
            public DateTime ValidatedAt { get; set; } 
            public Guid? ProductId { get; set; }
        } 

        public class Validation
        {

            public class Category
            {
                public string id { get; set; }
            }

            public class CompatibilityList
            {
                public object items { get; set; }
            }

            public class Publication
            {
                public object duration { get; set; }
                public string status { get; set; }
                public object startingAt { get; set; }
                public object endingAt { get; set; }
                public object endedBy { get; set; }
            }

            public class Payments
            {
                public string invoice { get; set; }
                public Tax tax { get; set; }
            }
            public class Tax
            {
                public string percentage { get; set; }
            }
            public class Error
            {
                public string code { get; set; }
                public string message { get; set; }
                public string details { get; set; }
                public string path { get; set; }
                public string userMessage { get; set; }
            }

            public class ValidationResult
            {
                public List<Error> errors { get; set; }
                public DateTime validatedAt { get; set; }
            }

            public class RootObject
            {
                public string id { get; set; }
                public string name { get; set; }
                public Category category { get; set; }
                public object product { get; set; }
                public List<object> parameters { get; set; }
                public object ean { get; set; }
                public object description { get; set; }
                public CompatibilityList compatibilityList { get; set; }
                //public List<object> images { get; set; }
                public object sellingMode { get; set; }
                public object stock { get; set; }
                public Publication publication { get; set; }
                public object delivery { get; set; }
                public Payments payments { get; set; }
                public object afterSalesServices { get; set; }
                public object additionalServices { get; set; }
                public object sizeTable { get; set; }
                public object promotion { get; set; }
                public object location { get; set; }
                public object external { get; set; }
                public List<object> attachments { get; set; }
                public object contact { get; set; }
                public ValidationResult validation { get; set; }
                public DateTime createdAt { get; set; }
                public DateTime updatedAt { get; set; }
            }
        }


        public static void UpdateDraft()
        { 
            Bll.AllegroHelper ah = new Bll.AllegroHelper();

            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            Dal.Settings s = sh.GetSetting("ALL_IMGS");

            List<Dal.Shop> allegroShops = Dal.DbHelper.Shop.GetShops(Dal.Helper.ShopType.Allegro);

            List<Dal.ProductCatalogAllegroItemsView> items =
                Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogAllegroItemsToUpdate("INACTIVE", s.IntValue.Value);
                //oh.GetProductCatalogAllegroItemsToUpdate("INACTIVE", s.IntValue.Value).ToList();

            if (items.Count() == 0)
                return;

            Guid processId = Guid.NewGuid();

            long[] itemIds = items.Select(x => x.ItemId).ToArray();

            Dal.DbHelper.ProductCatalog.Allegro.SetProductCatalogAllegroItemProcess(processId, itemIds);

            items =
                Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogAllegroItemsToUpdate("INACTIVE", s.IntValue.Value, processId);

   



            foreach (Dal.ProductCatalogAllegroItemsView item in items)
            {
                try
                {

                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine(String.Format("ItemNo: {1}, ItemID: {0}", item.ItemId, items.IndexOf(item)));

                    DateTime dtStart = DateTime.Now;

                    HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offers/{0}", item.ItemId), "PUT", item.ItemId, null);

                    CreateImages(new List<Dal.ProductCatalogAllegroItemsView> { item });

                    Offer.RootObject offer = GetOfferDraft(item, allegroShops);

                    Draft draft = SetOffer(request, item, offer);

                    ah.SetProductCatalogAllegroDraft(item, draft, "INACTIVE");

                    TimeSpan ts = DateTime.Now - dtStart;
                    Console.WriteLine("Czas aktualizacji szkicu {0}: {1}sek.", item.ItemId, ts.TotalSeconds);
                }
                catch (Exception ex)
                {
                    ErrorHandler.SendError(ex, String.Format("ItemId {0}, ProductCatalogId: <a href='http://192.168.0.107/ProductCatalog.Specification.aspx?id={1}'>{1}</a>",
                        item.ItemId,
                        item.ProductCatalogId
                        ));

                }
            }

            Dal.DbHelper.ProductCatalog.Allegro.SetProductCatalogAllegroItemProcessClear(processId, itemIds);
        }

        public static void CreateDraft()
        { 
            Bll.AllegroHelper ah = new Bll.AllegroHelper();

            List<Dal.ProductCatalogAllegroItemsDraftView> items = Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogAllegroItemsDraft();

            if (items.Count() == 0)
                return;

            List<Dal.Shop> allegroShops = Dal.DbHelper.Shop.GetShops(Dal.Helper.ShopType.Allegro);

            foreach (Dal.ProductCatalogAllegroItemsDraftView d in items)
            {

                ProductCatalogAllegroItemsView item = GetItemFromDraft(d);

                HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offers", ""), "POST", null, item.UserId);

                Offer.RootObject offer = GetOfferDraft(item, allegroShops);

                Draft draft = SetOffer(request, item, offer);

                ah.SetProductCatalogAllegroDraftItem(item, draft);
            }   
        }
        public static void ReActivate()
        {
            Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();
            Bll.AllegroHelper ah = new Bll.AllegroHelper();

            List<Dal.ProductCatalogAllegroItemsToReActivateView> items =
                Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogAllegroItemsToReActivate().ToList();
            if (items.Count() == 0)
                return;

            long[] userIds = items.Select(x => x.AllegroUserId).Distinct().ToArray();

            List<Dal.Shop> allegroShops = Dal.DbHelper.Shop.GetShops(Dal.Helper.ShopType.Allegro);
            foreach (long userId in userIds)
            {
                long[] itemIds = items.Where(x => x.AllegroUserId == userId).Select(x => x.ItemId).ToArray();

                //PublishResult.RootObject publish = PublishOffers(userId, itemIds, "INACTIVE");

                //if (publish != null)
                {

                    Dal.DbHelper.ProductCatalog.Allegro.SetProductCatalogAllegroItemReActive(itemIds, "10000000000000");

                }
            }
        }

        private static ProductCatalogAllegroItemsView GetItemFromDraft(ProductCatalogAllegroItemsDraftView d)
        {
            return new ProductCatalogAllegroItemsView()
            { 
                
                CommandId = null,
                Comment = null,
                HasOrders = false,
                Id = 0,
                //ImageFullName = d.ImageFullName,  
                IsValid = false,
                ItemId = 0,
                NotificationSent = false,
                PriceBruttoMinimum = d.PriceBruttoMinimum, 
                ProductCatalogId = d.ProductCatalogId,
                ProductName = d.ProductName, 
                ShopId = d.ShopId,
                UpdateCommand = "10000000000000", 
                UserId = d.UserId,
                UserName = d.UserName ,
                //IsImageReady = false,
                ProductId = d.ProductId
            };
        }
        /// <summary>
        /// Aktualizacja aktywnych ofert
        /// </summary>
        public static void UpdateOffer()
        {
            UpdateOffer(Guid.NewGuid());
        }
        public static void UpdateOffer(Guid processId)
        {
            Console.WriteLine(processId);
            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            Dal.Settings s = sh.GetSetting("ALL_IMGS");

            try
            {
                List<Dal.Shop> allegroShops = Dal.DbHelper.Shop.GetShops(Dal.Helper.ShopType.Allegro);
                List<Dal.ProductCatalogAllegroItemsView> items = 
                    Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogAllegroItemsToUpdate("ACTIVE", s.IntValue.Value);

                if (items.Count() > 0)
                {
                    long[] itemIds = items.Select(x => x.ItemId).ToArray();
              
                    Dal.DbHelper.ProductCatalog.Allegro.SetProductCatalogAllegroItemProcess(processId, itemIds);

                    //items = GetAllegroItems(s.IntValue.Value, processId, true);

                    items = Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogAllegroItemsToUpdate("ACTIVE", s.IntValue.Value, processId);
               

                    DateTime dtStart = DateTime.Now;
                    UpdateOffer(items, false, allegroShops);

                    TimeSpan ts = DateTime.Now - dtStart;
                    Console.WriteLine("Czas aktualizacji ofert {0}: {1}sek.", items.Count(), ts.TotalSeconds);

                    Dal.DbHelper.ProductCatalog.Allegro.SetProductCatalogAllegroItemProcessClear(processId, itemIds);

                }
                Console.WriteLine("Items {0}", items.Count());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Bll.ErrorHandler.SendError(ex, "ALLEGRO_UPDATE_OFFER");
            }
        }

 

        public static bool UpdateOffer(int productCatalogId)
        {

            List<Dal.Shop> allegroShops = Dal.DbHelper.Shop.GetShops(Dal.Helper.ShopType.Allegro);
            List<Dal.ProductCatalogAllegroItemsView> items =
                Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogAllegroItemsToUpdate(productCatalogId);
               
          
            return UpdateOffer(items, true, allegroShops);
        }
 
        //private static List<Dal.ProductCatalogAllegroItemsFnResult> GetAllegroItems(int limit, Guid? processId, bool? isImageReady)
        //{
        //    Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();
        //    List<Dal.ProductCatalogAllegroItemsFnResult> items = oh.GetAllegroItemsToUpdate(limit, processId, isImageReady);
            

        //    return items;
        //}
        //private static List<Dal.ProductCatalogAllegroItemsFnResult> GetAllegroItems(int count)
        //{
        //    Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper(); 
        //    List<Dal.ProductCatalogAllegroItemsFnResult> items = oh.GetAllegroItemsToUpdate(count);


        //    return items;
        //}

        public static bool UpdateOffer(List<Dal.ProductCatalogAllegroItemsView> items, bool refreshJson, List<Dal.Shop> allegroShops)
        {
            Bll.AllegroHelper ah = new Bll.AllegroHelper();
       
            bool result = true;


            foreach (Dal.ProductCatalogAllegroItemsView item in items)
            {
                try
                {
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine(String.Format("ItemNo: {1}, ItemID: {0}", item.ItemId, items.IndexOf(item)));

                    DateTime dtStart = DateTime.Now;

                    CreateImages(new List<Dal.ProductCatalogAllegroItemsView> { item });

                    Offer.RootObject itemFromAllegro = GetOffer(item.ItemId, refreshJson);
                    Offer.RootObject offerDraft = GetOfferDraft(item, allegroShops);

                    UpdateOffer(offerDraft, itemFromAllegro, item);

                    itemFromAllegro.createdAt = null;
                    itemFromAllegro.promotion = null;
                    itemFromAllegro.updatedAt = null;
                    itemFromAllegro.validation = null;
                    
                    HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offers/{0}", item.ItemId), "PUT", null, item.UserId);

                    Draft draft = SetOffer(request, item, itemFromAllegro);
                    TimeSpan ts = DateTime.Now - dtStart;
                    Console.WriteLine("Czas aktualizacji oferty {0}: {1}sek.", item.ItemId, ts.TotalSeconds);


                    ah.SetProductCatalogAllegroDraft(item, draft, null);

                    if (result && !draft.IsValid)
                        result = false;
                }
                catch (Exception ex)
                {
                    ErrorHandler.SendError(ex, String.Format("ItemId {0} ProductCatalogId {1}", item.ItemId, item.ProductCatalogId));
                }
            }

            return result;
            //if (items.Count() > 0)
            //{
            //    TimeSpan ts = DateTime.Now - dtStart;
            //    Bll.ErrorHandler.SendEmail(String.Format("Czas aktualizacji {0} ofert Allegro: {1}", items.Count(), ts.TotalSeconds));
            //}
        }

        private static void UpdateOffer(Offer.RootObject offerDraft, Offer.RootObject itemFromAllegro, ProductCatalogAllegroItemsView item)
        {
            try

            {
                Console.WriteLine("     Update offer fields");
                if (itemFromAllegro.createdAt > DateTime.Now.AddHours(12)) // nie zmieniaj kategorii jesli minelo 12 godzin
                    itemFromAllegro.category.id = offerDraft.category.id;
                if (!item.HasOrders.HasValue || item.HasOrders.Value == false) // nie zmieniaj tytułu jeśli byly zakupy
                {
                    itemFromAllegro.name = offerDraft.name;
                }

                List<Dal.ProductCatalogShopUpdateSchedule> schedules = UpdateCommandToSchedules(item.UpdateCommand);
                //if (Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Images) ||
                //Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Description) ||
                //Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Attributes) ||
                //Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.All))
                //{
                    itemFromAllegro.description = offerDraft.description;
                    itemFromAllegro.images = offerDraft.images;
                //}
                itemFromAllegro.delivery = offerDraft.delivery;
                itemFromAllegro.ean = offerDraft.ean;
                itemFromAllegro.external = offerDraft.external;
                itemFromAllegro.location = offerDraft.location;
                itemFromAllegro.parameters = offerDraft.parameters;
                itemFromAllegro.payments = offerDraft.payments;
                //itemFromAllegro.product = offerDraft.product;
                itemFromAllegro.tax = offerDraft.tax;
                if (Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Price) ||
                Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.All))
                {
                    AllegroRESTHelper.ChangePrice(item.ItemId, float.Parse(offerDraft.sellingMode.price.amount, CultureInfo.InvariantCulture.NumberFormat));
                }
                //itemFromAllegro.sellingMode.price = offerDraft.sellingMode.price;
                itemFromAllegro.sellingMode = offerDraft.sellingMode;
                itemFromAllegro.stock = offerDraft.stock;
            }
            catch (Exception ex)
            {
                ErrorHandler.SendError(ex, String.Format("UpdateOffer błąd. ItemId {0}", item.ItemId));
            }
        }

        public static List<ProductCatalogShopUpdateSchedule> UpdateCommandToSchedules(string updateCommand)
        {
            if (updateCommand == null)
                updateCommand = "10000000000000";


            List<ProductCatalogShopUpdateSchedule> schedules = new List<ProductCatalogShopUpdateSchedule>();
            foreach (int i in Enum.GetValues(typeof(Dal.Helper.ShopColumnType)))
            {
                if(updateCommand.Length>=i+1)
                {
                    if (updateCommand.Substring(i,1)=="1")
                    {
                        schedules.Add(
                            new ProductCatalogShopUpdateSchedule()
                            {
                                ShopColumnTypeId = i

                            }) ;
                    }
                }
            }
            return schedules;
        }

        private static Draft SetOffer(HttpWebRequest request, Dal.ProductCatalogAllegroItemsView item, Offer.RootObject offer)
        {
            Draft draft = new Draft();
            Validation.RootObject validationResult = null;
            try
            {

                draft.Id = item.Id;
                draft.ProductCatalogId = item.ProductCatalogId;
                draft.ItemId = item.ItemId;
                if (offer.product != null)
                    draft.ProductId = offer.product.id ;

                Stream dataStream = request.GetRequestStream();



                string jsonEncodedParams = Bll.RESTHelper.ToJson(offer);
                 

                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                 
                byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close(); 

                WebResponse webResponse = request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();

                var json_serializer = new JavaScriptSerializer();
                validationResult = json_serializer.Deserialize<Validation.RootObject>(text);


                //Bll.ErrorHandler.SendEmail(text);
                draft.ItemId = Int64.Parse(validationResult.id);
                draft.ValidationErrors = Bll.RESTHelper.ToJson(validationResult.validation);
                draft.IsValid = validationResult.validation.errors.Count == 0; 



                if (offer.category.id!=null)
                draft.AllegroCategoryId = Int32.Parse(offer.category.id);
                draft.ValidatedAt = validationResult.validation.validatedAt;

                //SendValidationErrors(validationResult, item);
                Console.WriteLine(String.Format("{0} {1}", item.ItemId, draft.ValidationErrors));

                return draft;

            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if(httpResponse==null)
                    {
                        draft.ValidationErrors = ex.Message;
                        draft.IsValid = false;
                        draft.ValidatedAt = DateTime.Now;
                        Bll.ErrorHandler.SendError(ex, ex.Message);
                        return draft;

                    }
                    Console.WriteLine("Error code: {0}, ItemID: {1}", httpResponse.StatusCode, item.ItemId);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();

                        draft.ValidationErrors = text;
                        draft.IsValid = false;
                        draft.ValidatedAt = DateTime.Now;

                        var json_serializer = new JavaScriptSerializer();
                        Exception1.RootObject exErr = json_serializer.Deserialize<Exception1.RootObject>(text);

                        if (exErr.errors.Where(x => x.userMessage.Contains("Podany adres obrazka jest nieprawidłowy.")).Count() > 0)
                            Dal.DbHelper.ProductCatalog.Allegro.SetProductCatalogAllegroItemRefreshImages(item.ItemId);



                        //if (item.NotificationSent.HasValue == false || !item.NotificationSent.Value)
                        //{

                        //    Console.WriteLine("Error {0}: ", String.Join("",  exErr.errors.Select(x => String.Format("<li>{0}</li>", x.userMessage)).ToArray()));

                        //    if (exErr != null)
                        //        SendValidationErrors(exErr, item);
                        //    else
                        //        Bll.ErrorHandler.SendError(ex, text);
                        //}
                        Console.WriteLine("Error {0}: ", String.Join("", exErr.errors.Select(x => String.Format("<li>{0}</li>", x.userMessage)).ToArray()));
                        return draft;
                    }
                }
            }
            catch (Exception ex)
            {
                draft.ValidationErrors = ex.Message;
                draft.IsValid = false;
                draft.ValidatedAt = DateTime.Now;
                ErrorHandler.SendError(ex, String.Format("UpdateOffer błąd. ItemId {0}", item.ItemId));
                return draft;
            }

        }
        public class Exception1
        {
            public class Error
            {
                public string code { get; set; }
                public string message { get; set; }
                public string details { get; set; }
                public string path { get; set; }
                public string userMessage { get; set; }
            }

            public class RootObject
            {
                public List<Error> errors { get; set; }
            }

        }
//        private static void SendValidationErrors(Exception1.RootObject validationResult, 
//            Dal.ProductCatalogAllegroItemsView item)
//        {
//            Bll.EmailSender emailSender = new EmailSender();
//            string msg = @"<table>
//<tr><td>Produkt</td><td><a href='http://192.168.0.107/ProductCatalog.Specification.aspx?id={1}'>{2}</a> {4}</td></tr>
//<tr><td>Data walidacji</td><td>{0:yyyy-MM-dd HH:mm}</td></tr><tr><td colspan='2'>Znalezione błędy<ul>{3}</ul></td></tr></table>";

//            if (validationResult != null && validationResult.errors != null && validationResult.errors.Count > 0 &&
//                (item.NotificationSent.HasValue == false || !item.NotificationSent.Value))
//            {



//                msg = String.Format(msg,
//                    DateTime.Now,
//                    item.ProductCatalogId,
//                    item.ProductName,
//String.Join("", validationResult.errors.Select(x => String.Format("<li>{0}</li>", x.userMessage)).ToArray()),
//item.ItemId);


//                emailSender.SendEmail(new Dto.Email()
//                {
//                    Body = msg,
//                    FromEmail = Dal.Helper.MyEmail,
//                    FromName = "System",
//                    Subject = String.Format("Błąd w procedurze weryfikacji aukcji Allegro {0}", item.ProductName),
//                    ToEmail = Dal.Helper.BackendEmail,
//                    ToName = Dal.Helper.BackendEmail

//                });
//                emailSender.SendEmail(new Dto.Email()
//                {
//                    Body = msg,
//                    FromEmail = Dal.Helper.MyEmail,
//                    FromName = "System",
//                    Subject = String.Format("Błąd w procedurze weryfikacji aukcji Allegro {0}", item.ProductName),
//                    ToEmail = Dal.Helper.ErrorEmail,
//                    ToName = Dal.Helper.BackendEmail

//                });


//            }
//            else
//            {
//                msg = String.Format(msg,
//                    DateTime.Now,
//                    item.ProductCatalogId,
//                    item.ProductName,
//                    String.Format("<li>Nie można rzutować json na obiekt błędu</li>", ""),
//                    item.ItemId);


//                emailSender.SendEmail(new Dto.Email()
//                {
//                    Body = msg,
//                    FromEmail = Dal.Helper.MyEmail,
//                    FromName = "System",
//                    Subject = String.Format("Błąd w procedurze weryfikacji aukcji Allegro {0}", item.ProductName),
//                    ToEmail = Dal.Helper.ErrorEmail,
//                    ToName = Dal.Helper.BackendEmail

//                });

//            }
//        }
//        private static void SendValidationErrors(Validation.RootObject validationResult, Dal.ProductCatalogAllegroItemsView item)
//        {
//            if(validationResult.validation.errors.Count>0 && !item.NotificationSent.HasValue)
//            {

//                string msg = @"<table>
//<tr><td>Produkt</td><td><a href='http://192.168.0.107/ProductCatalog.Specification.aspx?id={1}'>{2}</a> {4}</td></tr>
//<tr><td>Data walidacji</td><td>{0:yyyy-MM-dd HH:mm}</td></tr><tr><td colspan='2'>Znalezione błędy<ul>{3}</ul></td></tr></table>";



//                msg = String.Format(msg,
//                    validationResult.validation.validatedAt,
//                    item.ProductCatalogId,
//                    item.ProductName,
//String.Join("", validationResult.validation.errors.Select(x => String.Format("<li>{0}</li>", x.userMessage)).ToArray()),
//item.ItemId);

//                    Bll.EmailSender emailSender = new EmailSender();

//                    emailSender.SendEmail(new Dto.Email()
//                    {
//                        Body = msg,
//                        FromEmail = Dal.Helper.MyEmail,
//                        FromName = "System",
//                        Subject = String.Format("Błąd w procedurze weryfikacji aukcji Allegro {0}", item.ProductName),
//                        ToEmail = Dal.Helper.BackendEmail,
//                        ToName = Dal.Helper.BackendEmail

//                    });
//                //emailSender.SendEmail(new Dto.Email()
//                //{
//                //    Body = msg,
//                //    FromEmail = Dal.Helper.MyEmail,
//                //    FromName = "System",
//                //    Subject = String.Format("Błąd w procedurze weryfikacji aukcji Allegro {0}", item.ProductName),
//                //    ToEmail = Dal.Helper.DevEmail,
//                //    ToName = Dal.Helper.BackendEmail

//                //});


//            }
//        }

        private static Offer.RootObject GetOfferDraft(  
            Dal.ProductCatalogAllegroItemsView item,
            List<Dal.Shop> allegroShops)
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            Dal.AllegroUser au = asc.GetAllegroUser(item.UserId);
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                Dal.ProductCatalogView pc = pch.GetProductCatalogView(item.ProductCatalogId);
            var cat = GetAllegroCategory(pc.ProductCatalogId);


            Offer.RootObject offer = new Offer.RootObject()
            {
                additionalServices = null,
                afterSalesServices = GetAfterSalesServices(au),
                category = new Offer.Category() { id = (cat == null) ? null : cat.AllegroCategoryId },
                ////compatibilityList = null,
                ////contact = null,
                ////createdAt = null,
                delivery = GetDelivery(item),
                description = GetDescription(item),
                //ean = GetEan(pc),
                external = new Offer.External() { id = item.ProductCatalogId.ToString() },

                images = GetImages(item),
                location = GetLocation(),
                name = GetName(item),

                payments = new Offer.Payments() { invoice = "VAT" },
                sellingMode = new Offer.SellingMode()
                {
                    format = "BUY_NOW",
                    price = new Offer.Price() { amount = item.PriceBruttoMinimum.Value.ToString().Replace(",", "."), currency = "PLN" }
                },
                tax = new Offer.Tax() { percentage = Dal.Helper.VAT * 100 },
                stock = GetStock(pc, item.UserId, allegroShops),
                publication = GetPublication(pc),
                product = GetProduct(item)
            };

            //if(offer.product==null)
            //{
                offer.parameters = GetParameters(pc.ProductCatalogId, item.ItemId);
            //}
            //else
            //{
            //    offer.parameters = AllegroRESTHelper.Products. GetParametersFromProduct(offer.product.id);
            //}

            

            if (item.ItemId > 0)
                offer.id = item.ItemId.ToString();

            return offer;
        }

     

        private static Offer.Product GetProduct(ProductCatalogAllegroItemsView item)
        {
            if (item.ProductId != null)
                return new Offer.Product() { id = item.ProductId.Value };

            return null;
        }

        private static string GetName(ProductCatalogAllegroItemsView item)
        {
            if (!String.IsNullOrEmpty(item.ProductName) )
            { 
                string an = item.ProductName;

                if (an.Length > 50)
                {
                    an = an.Substring(0, 50);
                }
                return Bll.Helper.ReplaceInvalidAllegroCharacters(an);

            }

            string allegroName = "";

            List<string> names = new List<string>();

            for(int i=0;i<5;i++)
            {
                string name = Mixer.GetProductName(item.ShopId, item.ProductCatalogId);

                if (name.Length <= 50)
                {
                    allegroName = name;
                    break;
                }
                else
                    names.Add(name);
            }

            if(allegroName=="")
            {
                allegroName = names.OrderBy(x => x.Length).FirstOrDefault();
                allegroName = allegroName.Substring(0, Math.Min(50, allegroName.Length));
            }



            return Bll.Helper.ReplaceInvalidAllegroCharacters(allegroName);

        }

        //private static string GetEan(ProductCatalogView pc)
        //{
        //    if (String.IsNullOrEmpty(pc.Ean))
        //        return null;
        //    else
        //        return pc.Ean;
        //}

        public static Dal.ProductCatalogAllegroCategory GetAllegroCategory(int productCatalogId)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ProductCatalogAllegroCategory ac = pch.GetProductCatalogAllegroCategory(productCatalogId);

             
                return ac; 
        }


        #region metody pomocnicze go GetOfferDraft
        private static Offer.Publication GetPublication(ProductCatalogView item)
        {
            return new Offer.Publication()
            {
                duration = null,
                /*
                 -- wymagane, czas trwania oferty, dostępne
                                               wartości: null (do wyczerpania zapasów), 
                                               P3D (3 dni); P5D (5 dni); P7D (7 dni); 
                                               P10D (10 dni); P20D (20 dni); P30D (30 dni), czas 
                                               możesz podać też w godzinach - np.: P72H (3 dni). 
                                               Dla oferty typu AUCTION (licytacja) nie możesz 
                                               podać wartości null i dostępne są wartości:
                                               P3D (3 dni); P5D (5 dni); P7D (7 dni); P10D (10 dni);
                                               Natomiast dla ofert typu ADVERTISMENT (ogłoszenia) 
                                               dostępne są wartości: null (na czas nieokreślony - 
                                               opłaty naliczane co 10 dni); P10D (10 dni); 
                                               P20D (20 dni); P30D (30 dni)
                                               */
                status = "INACTIVE",
                //endedBy = null,
                endingAt = null,
                startingAt = null
            };
        }
        private static Offer.Stock GetStock(ProductCatalogView item, long allegroUserId, List<Dal.Shop> allegroShops)
        {
            int quantity = 50;


            Dal.Shop shop = allegroShops.Where(x => x.ExternalId == allegroUserId).FirstOrDefault();

            if (shop.SellOnlyFromStock)
                quantity = item.LeftQuantity;
            else
            {
                if (item.SupplierQuantity.HasValue && item.SupplierQuantity.Value > 0)
                    quantity = item.SupplierQuantity.Value + item.LeftQuantity;
                else
                {
                    if (item.IsOnStock  && (!item.IsAvailable || item.IsDiscontinued))
                    {
                        quantity = item.LeftQuantity;// + 10;
                    }
                    else
                    {
                        //Dal.ShopHelper shh = new Dal.ShopHelper();
                        //Dal.SupplierShop supplierShops = shh.GetSuppliersShopByAllegroUserId(item.SupplierId, allegroUserId);
                        //if (supplierShops != null && supplierShops.MaxNumberOfProductsInOffer.HasValue)
                        quantity = 50;// supplierShops.MaxNumberOfProductsInOffer.Value;
                    }
                }
            }
            return new Offer.Stock()
            {
                available = quantity,
                unit = "UNIT"
            };
        }

        public static List<Offer.Parameter> GetParameters(int productCatalogId, long itemId)
        {
            List<Offer.Parameter> parameters = new List<Offer.Parameter>();

            //if (offer.product != null)
            //    parameters.AddRange(AllegroRESTHelper.Products.GetParametersFromProduct(offer.product.id));


            string[] id = parameters.Select(x => x.id).ToArray();

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogToAllegroParametersGetResult> productCatalogParameters = pch.GetProductCatalogToAllegroParameters(productCatalogId, itemId);

            string[] categoryFieldIds = productCatalogParameters
                .Where(x=>!id.Contains(x.CategoryFieldId))
                .Select(x => x.CategoryFieldId).Distinct().ToArray();

            foreach(string categoryFieldId in categoryFieldIds)
            {
                Offer.Parameter parameter = new Offer.Parameter
                {
                    id = categoryFieldId
                };

                var pcp = productCatalogParameters.Where(x => x.CategoryFieldId == categoryFieldId).FirstOrDefault();

                switch (pcp.FieldType)
                {
                    case 1:
                        List<string> valueIds = productCatalogParameters.Where(x => x.CategoryFieldId == categoryFieldId)
                            .Select(x => x.AllegroParameterId)
                            .Distinct()
                            .ToList();
                        parameter.valuesIds = valueIds;
                        parameters.Add(parameter);
                        break;
                    case 2:
                        if(!String.IsNullOrEmpty(pcp.StringValue)){
                            if (pcp.UseDefaultValue)
                                parameter.valuesIds = new List<string>() { pcp.StringValue };
                            else
                            {
                                if (pcp.SystemFieldId.HasValue && pcp.SystemFieldId.Value == 3) // Nazwa kolekcji
                                {
                                    string kolekcja = Bll.Helper.ReplaceInvalidAllegroCharacters(pcp.StringValue);
                                    if (!String.IsNullOrEmpty(kolekcja) && kolekcja.Length > 20)
                                        kolekcja = kolekcja.Substring(0, 20);
                                    parameter.values = new List<string>() { kolekcja };
                                }
                                else
                                    parameter.values = new List<string>() { pcp.StringValue };
                                 
                            }
                            parameters.Add(parameter);
                        }
                        break;
                    case 3:
                        if (pcp.DecimalValue.HasValue)
                            parameter.values = new List<string>() { String.Format(CultureInfo.InvariantCulture, "{0:0.##}", pcp.DecimalValue.Value)};// .ToString().Replace(",", ".") };
                        else
                            parameter.values = new List<string>() { null };
                        parameters.Add(parameter);
                        break;
                }
                
                 
            }
            return parameters;
        }
         
        private static Offer.Location GetLocation()
        {
            return new Offer.Location()
            {
                city = "Łódź",
                countryCode = "PL",
                postCode = "93-490",
               province = "LODZKIE"
            };
        }

        private static List<Offer.Image> GetImages(ProductCatalogAllegroItemsView item)
        {
            //if (!item.IsImageReady.Value )
            //    return null;

            Dal.AllegroScan asc = new Dal.AllegroScan();

            return  asc.GetProductCatalogImageAllegroItem(item.ItemId)
                .OrderBy(x => x.ImageTypeId).ThenBy(x=>x.Id)
                .Select(x => new Offer.Image() { url = x.LocationUrl })                
                .ToList();


        }
 
        private static Offer.Description GetDescription(ProductCatalogAllegroItemsView item)
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            Offer.Item itemImg = null;
            List<ProductCatalogImageAllegroItem> images = new List<ProductCatalogImageAllegroItem>();
            List<ProductCatalogImageAllegroItem> imagesProducts = new List<ProductCatalogImageAllegroItem>();

            Offer.Description desc = new Offer.Description();
            desc.sections = new List<Offer.Section>();

         
                images = asc.GetProductCatalogImageAllegroItem(item.ItemId);
                imagesProducts = images.Where(x => x.ImageTypeId == 1).ToList();

              
                #region header image
                var headerImage = images.Where(x => x.ImageTypeId == 2).FirstOrDefault();

                if (headerImage != null)
                {
                    Offer.Item iImg = new Offer.Item()
                    {
                        type = "IMAGE",
                        url = headerImage.LocationUrl
                    };
                    List<Offer.Item> iItems = new List<Offer.Item>();
                    iItems.Add(iImg);
                    Offer.Section imgSec = new Offer.Section()
                    {
                        items = iItems
                    };
                    desc.sections.Add(imgSec);
                }
                #endregion

           
            Dal.ShopHelper sh = new Dal.ShopHelper();
            int shopId = sh.GetShopIdByAllegroUserId(item.UserId);

            Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopId);


            string content = OrderHelper.GetPreview(shop, item.ProductCatalogId, false).ToString();
            content = Bll.Helper.ReplaceInvalidAllegroCharactersFromDescription(content);
            Offer.Item itemText = new Offer.Item()
            {
                type = "TEXT",
                content = content
            };

            List<Offer.Item> mainSection = new List<Offer.Item>();

            if (imagesProducts.Count > 0)
            {
                itemImg = new Offer.Item()
                {
                    type = "IMAGE",
                    url = imagesProducts[0].LocationUrl
                };
            }


            if (itemImg != null)
                mainSection.Add(itemImg);
            mainSection.Add(itemText);

            Offer.Section section = new Offer.Section()
            {
                items = mainSection
            };

            desc.sections.Add(section);


            #region opis produktu
             
            if (!String.IsNullOrEmpty(item.LongDescription ))
            {
                Offer.Item itemDesc = new Offer.Item()
                {
                    type = "TEXT",
                    content = String.Format("{0}", Bll.Helper.ReplaceInvalidAllegroCharactersFromDescription(item.LongDescription))
                };

                Offer.Section secDesc = new Offer.Section();
                secDesc.items = new List<Offer.Item>();
                secDesc.items.Add(itemDesc);

                desc.sections.Add(secDesc);

            }


            #endregion

            #region opis producenta

            Dal.SupplierShop supplierShop = Dal.DbHelper.ProductCatalog.GetSupplierShops(item.SupplierId)
                .Where(x => x.ShopId == item.ShopId && x.IsDescriptionActive == true).FirstOrDefault();
            if (supplierShop != null && supplierShop.LongDescription != null)
            {
                Offer.Item itemDesc = new Offer.Item()
                {
                    type = "TEXT",
                    content = String.Format("{0}", Bll.Helper.ReplaceInvalidAllegroCharactersFromDescription(supplierShop.LongDescription))
                };

                Offer.Section secDesc = new Offer.Section();
                secDesc.items = new List<Offer.Item>();
                secDesc.items.Add(itemDesc);

                desc.sections.Add(secDesc);

            }
            

            #endregion
            if (imagesProducts != null && imagesProducts.Count > 1)
            {
                for (int i = 1; i < imagesProducts.Count; i++)
                {
                    Offer.Item iImg = new Offer.Item()
                    {
                        type = "IMAGE",
                        url = imagesProducts[i].LocationUrl
                    };
                    List<Offer.Item> iItems = new List<Offer.Item>();
                    iItems.Add(iImg);
                    Offer.Section imgSec = new Offer.Section()
                    {
                        items = iItems
                    };
                    desc.sections.Add(imgSec);
                }
            }

            return desc;
        }

        private static Offer.Delivery GetDelivery(ProductCatalogAllegroItemsView item )
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
          
            Dal.ProductCatalogSourceDeliveryFnResult sourceDelivery =
                pch.GetProductCatalogSourceDelivery(item.ProductCatalogId, (int)Dal.Helper.ShopType.Allegro);

            return new Offer.Delivery()
            {
                shippingRates = GetShippingRates(item.ProductCatalogId, item.UserId),
                handlingTime = String.Format("PT{0}H", sourceDelivery.ExternalValue),
                /*
                 -- czas wysyłki, obecnie dostępne są
                                                wartości: PT0S (natychmiast), PT24H (24 godziny), 
                                                P2D (2 dni), P3D (3 dni), P4D (4 dni), P5D (5 dni), 
                                                P7D (7 dni), P10D (10 dni), P14D (14 dni), 
                                                P21D (21 dni), P30D (30 dni), P60D (60 dni).   
                                                Można również podać te wartości 
                                                w godzinach, np. PT72H (3 dni). 
                                                Niewymagane dla formatu sprzedaży 
                                                typu ADVERTISEMENT (ogłoszenie).
                */
                shipmentDate = DateTime.UtcNow.AddDays(0).ToString("o"),
                additionalInfo = ""

            };
        }

        private static Offer.ShippingRates GetShippingRates(int productCatalogId, long userId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.AllegroDeliveryFnResult delivery = oh.GetAllegroDelivery(productCatalogId, userId);

            if (delivery==null || !delivery.AllegroShippingId.HasValue)
                return null;

            return new Offer.ShippingRates()
            {
                id = delivery.AllegroShippingId.Value.ToString()

            };
        }

        private static Offer.AfterSalesServices GetAfterSalesServices(AllegroUser au)
        {
            return new Offer.AfterSalesServices()
            {
                impliedWarranty = new Offer.ImpliedWarranty() { id = au.ImpliedWarranty },
                returnPolicy = new Offer.ReturnPolicy() { id = au.ReturnPolicy },
                warranty = new Offer.Warranty() { id = au.Warranty }
            };
        }
        #endregion


        public static void PublishOffers()
        { 
            List<Dal.ProductCatalogAllegroItemsView> items =
                Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogAllegroItemsForPublishing();

            long[] userIds = items.Select(x => x.UserId).Distinct().ToArray();

            foreach (long userId in userIds)
            {
                List<Dal.ProductCatalogAllegroItemsView> itemsToPublish =
                    items.Where(x => x.UserId == userId).ToList();

                PublishResult.RootObject publish = PublishOffers(userId, itemsToPublish);

                if(publish!=null)
                {
                    long[] itemIds = itemsToPublish.Select(x => x.ItemId).ToArray();
                    Dal.DbHelper.ProductCatalog.Allegro
                        .SetProductCatalogAllegroItemPublish(itemIds, "ACTIVATE", Guid.Parse(publish.id), "Publikacja", null);
                }
            }
        }
        public class Publish
        {
            public class Publication
            {
                public string action { get; set; }
                public string scheduledFor { get; set; }
            }

            public class Offer
            {
                public string id { get; set; }
            }

            public class OfferCriteria
            {
                public List<Offer> offers { get; set; }
                public string type { get; set; }
            }

            public class RootObject
            {
                public Publication publication { get; set; }
                public List<OfferCriteria> offerCriteria { get; set; }
            }
        }
        public class PublishResult
        {
            public class TaskCount
            {
                public int total { get; set; }
                public int success { get; set; }
                public int failed { get; set; }
            }

            public class RootObject
            {
                public string id { get; set; }
                public TaskCount taskCount { get; set; }
            }
        }

        //public static void SetOffersRestore()
        //{
        //    Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();

            
         
        //    foreach (long userId in Dal.DbHelper.AllegroHelper.GetAllegroMyUsers().Select(x=>x.UserId).ToArray())
        //    {
        //        List<Dal.AllegroItemsRestoreView> itemsToRestore = 
        //            Dal.DbHelper.AllegroHelper.GetAllegroItemsRestore(userId);


        //        long[] itemIds = itemsToRestore.Select(x => x.ItemId).ToArray();

        //        if (itemIds.Length > 0)
        //        {
        //            PublishResult.RootObject publish = PublishOffers(userId, itemIds, "INACTIVE");

        //            if (publish != null)
        //            {

        //                Dal.DbHelper.ProductCatalog.Allegro.SetProductCatalogAllegroItemPublish(itemIds, "INACTIVE", null, "Wznawianie oferty", "10000000000000");

        //            }
        //        }
        //    }

        //}
        private static PublishResult.RootObject PublishOffers(long userId, List<Dal.ProductCatalogAllegroItemsView> itemsToPublish)
        {
            return PublishOffers(userId, itemsToPublish.Select(x=>x.ItemId).ToArray(), "ACTIVATE");
        }

        private static PublishResult.RootObject PublishOffers(long userId, long[] itemIds, string status)
        {
            Guid commandId = Guid.NewGuid();
            try
            {

                HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offer-publication-commands/{0} ", commandId), "PUT", null, userId);


                Stream dataStream = request.GetRequestStream();

                Publish.RootObject publish = new Publish.RootObject()
                {
                    offerCriteria = GetOffersCriteria(itemIds),
                    publication = new Publish.Publication() { action = status }
                };


                string jsonEncodedParams = Bll.RESTHelper.ToJson(publish);
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();



                WebResponse webResponse = request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();

                var json_serializer = new JavaScriptSerializer();
                PublishResult.RootObject validationResult = json_serializer.Deserialize<PublishResult.RootObject>(text);



                return validationResult;

            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse == null)
                    {
                        Bll.ErrorHandler.SendError(ex, ex.Message);
                        return null;

                    }
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();

                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd publikowania ofert userId {0}, status: {3}, commandId {1}, {2}", userId, commandId, text, status));

                        return null;
                    }
                }
            }
        }
        private static PublishResult.RootObject PublishOffers(long userId, Guid commandId)
        { 
            try
            {

                HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offer-publication-commands/{0}/tasks", commandId), "GET", null, userId);


                //Stream dataStream = request.GetRequestStream();

                //Publish.RootObject publish = new Publish.RootObject()
                //{
                //    offerCriteria = GetOffersCriteria(itemIds),
                //    publication = new Publish.Publication() { action = status }
                //};


                //string jsonEncodedParams = Bll.RESTHelper.ToJson(publish);
                //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                //byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                //dataStream.Write(byteArray, 0, byteArray.Length);
                //dataStream.Close();



                WebResponse webResponse = request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string text = reader.ReadToEnd();

                var json_serializer = new JavaScriptSerializer();
                PublishResult.RootObject validationResult = json_serializer.Deserialize<PublishResult.RootObject>(text);



                return validationResult;

            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse == null)
                    {
                        Bll.ErrorHandler.SendError(ex, ex.Message);
                        return null;

                    }
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();

                        Bll.ErrorHandler.SendError(ex, String.Format("Błąd publikowania ofert userId {0},  commandId {1}, {2}", userId, commandId, text));

                        return null;
                    }
                }
            }
        }
        private static List<Publish.OfferCriteria> GetOffersCriteria(long[] itemsToPublish)
        {
            Publish.OfferCriteria criteria = new Publish.OfferCriteria()
            {
                offers = itemsToPublish.Select(x => new Publish.Offer() { id = x.ToString() }).ToList(),
                type = "CONTAINS_OFFERS"
            };

            List<Publish.OfferCriteria> criterias = new List<Publish.OfferCriteria>();
            criterias.Add(criteria);
            return criterias;
        }

        public class Delete
        {
            public class Offer
            {
                public string id { get; set; }
            }

            public class OfferCriteria
            {
                public List<Offer> offers { get; set; }
                public string type { get; set; }
            }

            public class Publication
            {
                public string action { get; set; }
                public string scheduledFor { get; set; }
            }

            public class RootObject
            {
                public List<OfferCriteria> offerCriteria { get; set; }
                public Publication publication { get; set; }
            }
        }
        public  static void DeleteAllegroItems(List<Dal.AllegroActionView> list)
        {

            Dal.ProductCatalogHelper ph = new Dal.ProductCatalogHelper();
            List<Dal.AllegroActionView> itemsProcessed = new List<Dal.AllegroActionView>();

            long[] userIds = list.Select(x => x.UserId).Distinct().ToArray();

            foreach (long userId in userIds)
            {
                try
                {

                    HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offer-publication-commands/{0}", Guid.NewGuid()), "PUT", null, userId);


                    Stream dataStream = request.GetRequestStream();

                    List<Delete.Offer> offers = new List<Delete.Offer>();
                    List<Delete.OfferCriteria> critetias = new List<Delete.OfferCriteria>();
                    offers.AddRange(list.Where(x => x.UserId == userId).Select(x => new Delete.Offer() { id = x.ItemId.ToString() }).ToList());
                    critetias.Add(new Delete.OfferCriteria() { offers = offers, type = "CONTAINS_OFFERS" });

                    Delete.RootObject delete = new Delete.RootObject()
                    {
                        publication = new Delete.Publication() { action = "END", scheduledFor = DateTime.UtcNow.ToString("o") },
                        offerCriteria = critetias

                    };


                    string jsonEncodedParams = Bll.RESTHelper.ToJson(delete);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();



                    WebResponse webResponse = request.GetResponse();
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string text = reader.ReadToEnd();

                    //var json_serializer = new JavaScriptSerializer();
                    //Validation.RootObject validationResult = json_serializer.Deserialize<Validation.RootObject>(text);
 

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


                            Bll.ErrorHandler.SendError(ex, String.Format("Błąd usuwania ofert oferty {0}, {1}", userId, text));
                        }
                    } 
                }

            }

        }

        public class PublishCommand
        {
            public class TaskCount
            {
                public int total { get; set; }
                public int success { get; set; }
                public int failed { get; set; }
            }

            public class RootObject
            {
                public string id { get; set; }
                public TaskCount taskCount { get; set; }
            }
        }  
        public static void PublishOffersDetails()
        {
            PublishOffers(44282528, Guid.Parse("ae84fe3c-b845-40fa-88a6-35e0be2aaaab"));
        }
        public static void PublishOffersCheck()
        {
            Guid? commandID = null ;
            try
            {
                Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();
                Bll.AllegroHelper ah = new Bll.AllegroHelper();

                List<Dal.ProductCatalogAllegroItemActivatingView> items = Dal.DbHelper.AllegroHelper.GetProductCatalogAllegroItemActivatingView();

                var commands = items.Where(x=>x.CommandId.HasValue)
                    .Select(x => new { UserId = x.UserId, CmdId = x.CommandId }).Distinct().ToList();

                foreach (var command in commands)
                {
                    commandID = command.CmdId;
                          HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offer-publication-commands/{0}", command.CmdId), "GET",null, command.UserId);


                        string text = null;
                        using (WebResponse webResponse = request.GetResponse())
                        {
                            Stream responseStream = webResponse.GetResponseStream();
                            StreamReader reader = new StreamReader(responseStream);
                            text = reader.ReadToEnd();
                        }

                        var json_serializer = new JavaScriptSerializer();
                        PublishCommand.RootObject publish = json_serializer.Deserialize<PublishCommand.RootObject>(text);

                    if (publish.taskCount.success == publish.taskCount.total)
                        oh.SetProductCatalogAllegroItemPublishCompleted(command.CmdId.Value);
                    else
                    {
                        if (publish.taskCount.failed >0)
                        {
                            //Bll.ErrorHandler.SendEmail(String.Format("Nieudana próba publikacji ofert. Wszystich: {0}, sukces: {1}, błąd: {2}, CommandId: {3}, Powód:{4}",
                            //    publish.taskCount.total, publish.taskCount.success, publish.taskCount.failed, publish.id, publish.taskCount.total
                            //    ));

                        }
                    } 
                }


            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Nieudana próba publikacji ofert. CommandId: {0}<br><br>{1}", commandID, ex.Message));
            } 
        }
        internal static void GetAllegroProductsFromOffers()
        {
            List<Dal.AllegroItem> allegroItems = Dal.DbHelper.AllegroHelper.GetAllegroItemsWithHasProductFlag();

            foreach(Dal.AllegroItem item in allegroItems)
            {
                Offer.RootObject offer = GetOffer(item.ItemId, true);
                string ean = offer.parameters.Where(x => x.id == "225693").Select(x => x.values[0]).FirstOrDefault();
                if (offer != null && ean != null)
                {
                    Dal.DbHelper.AllegroHelper.SetAllegroProduct(ean, offer.product.id, null, true);
                    Dal.DbHelper.AllegroHelper.SetAllegroItemProduct(item.ItemId, offer.product.id);//, offer.publication.status);
                }
            }
        }
        internal static void GetSellerOffers(int limit, int offset, bool? hasProduct)
        {

            Dal.AllegroScan allegroScan = new Dal.AllegroScan();
            foreach (Dal.AllegroUser au in allegroScan.GetMyUsers())
            {
                GetSellerOffers(au, limit, offset, hasProduct);
            }

        }
        //public static void TEST()
        //{
        //    var json_serializer = new JavaScriptSerializer();
        //    string text = File.ReadAllText(@"C:\Users\jacek\source\repos\AllegroApp\json.txt");

        //    SaleOfferRootObject offers = json_serializer.Deserialize<SaleOfferRootObject>(text);
        //    //Console.WriteLine(String.Format("Liczba ofert: {0}/{1}:", offers.count, offers.offers.Count));

        //    List<Dal.AllegroItem> allegroItems = new List<Dal.AllegroItem>();

        //    foreach (SaleOfferOffer item in offers.offers)
        //    {
        //        Console.WriteLine(String.Format("{0} {1} {2}", item.id, item.publication.status, item.name));
        //        Dal.AllegroItem ai = new Dal.AllegroItem()
        //        {
        //            ItemId = Int64.Parse(item.id),
        //            Name = item.name,
        //            EndingInfo = item.publication.status == "ACTIVE" ? 1 : 2,
        //            BidCount = item.saleInfo.biddersCount,

        //            //CurrentPrice = Convert.ToDecimal(item.sellingMode.price.amount, new System.Globalization.CultureInfo("en-us")),
        //            ItemStatus = item.publication.status,
        //            LastUpdateDate = DateTime.Now
        //        };
        //        if (item.sellingMode != null)
        //            ai.CurrentPrice = Convert.ToDecimal(item.sellingMode.price.amount, new System.Globalization.CultureInfo("en-us"));
        //        allegroItems.Add(ai);
        //    }
        //}
        internal static void GetSellerOffers(Dal.AllegroUser au, int limit, int offset, bool? hasProduct)
        {
            string text = null;
            try
            {
                if (au == null)
                    return;

                string hasProductStr = "";
                if (hasProduct.HasValue)
                    hasProductStr = "&product.id.empty=false";



                HttpWebRequest request = GetHttpWebRequest(
                    String.Format("/sale/offers?seller.id={0}&limit={1}&offset={2}{3}", au.UserId, limit, offset, hasProductStr)
                    , "GET", null, au.UserId);

    

                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }

                var json_serializer = new JavaScriptSerializer();
                SaleOfferRootObject offers = json_serializer.Deserialize<SaleOfferRootObject>(text);
                //Console.WriteLine(String.Format("Liczba ofert: {0}/{1}:", offers.count, offers.offers.Count));

                List<Dal.AllegroItem> allegroItems = new List<Dal.AllegroItem>();

                foreach (SaleOfferOffer item in offers.offers)
                {
                    Dal.AllegroItem ai = new Dal.AllegroItem()
                    {
                        ItemId = Int64.Parse(item.id),
                        Name = item.name,
                        // EndingInfo = item.publication.status == "ACTIVE" ? 1 : 2,
                        BidCount = item.saleInfo.biddersCount,
                        //ProductId = item.sa
                        CurrentPrice = Convert.ToDecimal(item.sellingMode.price.amount, new System.Globalization.CultureInfo("en-us")),
                        ItemStatus = item.publication.status,
                        LastUpdateDate = DateTime.Now,
                        EndingDate = item.publication.endedAt,

                    };

                    if (item.category != null && !String.IsNullOrEmpty(item.category.id))
                        ai.CategoryId = Int32.Parse(item.category.id);

                    if (hasProduct.HasValue)
                        ai.HasProductId = true;
               
                    allegroItems.Add(ai);
                    if (item.sellingMode != null)
                        ai.CurrentPrice = Convert.ToDecimal(item.sellingMode.price.amount, new System.Globalization.CultureInfo("en-us"));
                    else
                        ai.CurrentPrice = 0;
                    Console.WriteLine(String.Format("{0} {1} {2}", item.id, item.publication.status, item.name));
                }


                long[] itemsToRefreshImages = offers.offers.Where(x => String.IsNullOrEmpty(x.primaryImage.url)
                && x.publication.status == "ACTIVE").Select(x => Int64.Parse(x.id)).ToArray();


                Dal.AllegroScan allegroScan = new Dal.AllegroScan();
                allegroScan.SetAllegroItemRefreshImages(itemsToRefreshImages);

                List<long> itemIdsOnAllegroNotIsDb = new List<long>();
                allegroScan.SetAllegroItemRefresh(allegroItems, ref itemIdsOnAllegroNotIsDb);
                if (itemIdsOnAllegroNotIsDb.Count > 0)
                {
                    Console.WriteLine(String.Format("Liczba ofert na Allegro ale nie w systemie: {0}:", itemIdsOnAllegroNotIsDb.Count));

                    Bll.ErrorHandler.SendError(new Exception(), String.Format("Oferty na Allegro ale nie w systemie, UserId {0}, itemIds: {1}", au.UserName, 
                        String.Join(",", itemIdsOnAllegroNotIsDb)));


                    CreateMissingOffers(au.UserId, offers.offers, itemIdsOnAllegroNotIsDb);
                }
                if (limit <= offers.count)

                    GetSellerOffers(au, limit, offset + limit, hasProduct);


            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd aktualizacji ofert. Allegro.UserId: {0}, zmienna text: {1}", au.UserId, text));

            }
        }
        public static void GetProductsFromOffers()
        {
            //List<Dal.AllegroProduct> allegroItems = Dal.DbHelper.ProductCatalog.Allegro.GetAllegroProducts();

            //foreach (var item in allegroItems)
            //{
            //    var i = GetOffer(item.ItemId, true);

            //    if (i != null && i.product != null)
            //        Dal.DbHelper.AllegroHelper.SetAllegroProductToProductCatalog(item.ItemId, i.product.id);

            //}
        }
   

    
        public static void DeleteDraft()
        {

            Dal.AllegroScan allegroScan = new Dal.AllegroScan();
            foreach (Dal.AllegroUser au in allegroScan.GetMyUsers())

            {
                GetSellerDrafts(au, 500, 0);

            }


        }
        public static void SetDraftDelete(string id, long userId)
        {
            try
            {

                HttpWebRequest request = 
                    GetHttpWebRequest(String.Format("/sale/offers/{0}", id), "DELETE", null, userId);

                using (WebResponse webResponse = request.GetResponse())
                { }

                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                pch.SetProductCatalogAllegroItemDelete(Int64.Parse(id));
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd usuwania oferty {1}. Allegro.UserId: {0}", userId, id));
            }
        }
        internal static void GetSellerDrafts(Dal.AllegroUser au, int limit, int offset)
        {
            string text = null;
            try
            {
                if (au == null)
                    return;

                HttpWebRequest request = GetHttpWebRequest(
                    String.Format("/sale/offers?seller.id={0}&publication.status=INACTIVE&limit={1}&offset={2}", au.UserId, limit, offset)
                    , "GET", null, au.UserId);



                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }

                var json_serializer = new JavaScriptSerializer();
                SaleOfferRootObject offers = json_serializer.Deserialize<SaleOfferRootObject>(text);
                //Console.WriteLine(String.Format("Liczba ofert: {0}/{1}:", offers.count, offers.offers.Count));

                List<Dal.AllegroItem> allegroItems = new List<Dal.AllegroItem>();

                foreach (SaleOfferOffer item in offers.offers)
                {
                    //Dal.AllegroItem ai = new Dal.AllegroItem()
                    //{
                    //    ItemId = Int64.Parse(item.id),
                    //    Name = item.name,
                    //    EndingInfo = item.publication.status == "ACTIVE" ? 1 : 2,
                    //    BidCount = item.saleInfo.biddersCount,

                    //    //CurrentPrice = Convert.ToDecimal(item.sellingMode.price.amount, new System.Globalization.CultureInfo("en-us")),
                    //    ItemStatus = item.publication.status,
                    //    LastUpdateDate = DateTime.Now
                    //};
                    //allegroItems.Add(ai);
                    //if (item.sellingMode != null)
                    //    ai.CurrentPrice = Convert.ToDecimal(item.sellingMode.price.amount, new System.Globalization.CultureInfo("en-us"));
                    //else
                    //    ai.CurrentPrice = 0;
                    SetDraftDelete(item.id, au.UserId);
                    Console.WriteLine(String.Format("{0} {1} {2}", item.id, item.publication.status, item.name));
                }

                //Dal.AllegroScan allegroScan = new Dal.AllegroScan();
                //List<long> itemIdsOnAllegroNotIsDb = new List<long>();
                //allegroScan.SetAllegroItemRefresh(allegroItems, ref itemIdsOnAllegroNotIsDb);
                //if (itemIdsOnAllegroNotIsDb.Count > 0)
                //{
                //    Console.WriteLine(String.Format("Liczba ofert na Allegro ale nie w systemie: {0}:", itemIdsOnAllegroNotIsDb.Count));

                //    Bll.ErrorHandler.SendError(new Exception(), String.Format("Oferty na Allegro ale nie w systemie, UserId {0}, itemIds: {1}", au.UserName,
                //        String.Join(",", itemIdsOnAllegroNotIsDb)));


                //    CreateMissingOffers(au.UserId, offers.offers, itemIdsOnAllegroNotIsDb);
                //}
                if (limit <= offers.count)

                    GetSellerDrafts(au, limit, offset + limit);


            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd aktualizacji ofert. Allegro.UserId: {0}, zmienna text: {1}", au.UserId, text));

            }
        }

        private static void CreateMissingOffers(long userId, List<SaleOfferOffer> offers, List<long> itemIdsOnAllegroNotIsDb)
        {
            List<Dal.AllegroItem> allegroItems = new List<AllegroItem>();
            List<Dal.ProductCatalogAllegroItem> pcAllegroItems = new List<ProductCatalogAllegroItem>();
            foreach (SaleOfferOffer offer in offers.Where(x => itemIdsOnAllegroNotIsDb.Contains(Int64.Parse(x.id))).ToList())
            {
                Dal.AllegroItem ai = new AllegroItem()
                {
                    ItemId = Int64.Parse(offer.id),
                    Name = offer.name,
                   // EndingInfo = offer.publication.status == "ACTIVE" ? 1 : 2,
                    BidCount = offer.saleInfo.biddersCount,
                    //CurrentPrice = Convert.ToDecimal(offer.sellingMode.price.amount, new System.Globalization.CultureInfo("en-us")),
                    ItemStatus = offer.publication.status,
                    LastUpdateDate = DateTime.Now,
                    BuyNowPrice = Convert.ToDecimal(offer.sellingMode.price.amount, new System.Globalization.CultureInfo("en-us")),
                    //MovedToArchive = false,
                    InsertDate = DateTime.Now,
                    //SellingMode = offer.sellingMode.format,
                    UserId = userId
                };
                if(offer.sellingMode!=null)
                {
                    ai.CurrentPrice = Convert.ToDecimal(offer.sellingMode.price.amount, new System.Globalization.CultureInfo("en-us"));
                    ai.BuyNowPrice = Convert.ToDecimal(offer.sellingMode.price.amount, new System.Globalization.CultureInfo("en-us"));
                    ai.SellingMode = offer.sellingMode.format;
                }
                else
                {

                    ai.CurrentPrice = 0;
                    ai.BuyNowPrice = 0;
                }

                allegroItems.Add(ai);
                Dal.ProductCatalogAllegroItem pcai = new ProductCatalogAllegroItem()
                {
                    AllegroUserId = userId,
                    InsertDate = DateTime.Now,
                    IsValid = false, 
                    //Comment = String.Format("CreateMissingOffers"),
                    ItemId = Int64.Parse(offer.id)
                };

                if (offer.external != null && offer.external.id != null)
                {
                    pcai.ProductCatalogId = Int32.Parse(offer.external.id);

                    pcAllegroItems.Add(pcai);
                }


            }
            Dal.AllegroScan allegroScan = new Dal.AllegroScan();
            allegroScan.CreateMissingOffers(userId, allegroItems, pcAllegroItems);
        }

        public static Offer.RootObject GetOffer(long itemId, bool refreshJson)
        {
            string text = null;
            try
            {
                Offer.RootObject offer = null;
                var json_serializer = new JavaScriptSerializer();


                //Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

                //Dal.ProductCatalogAllegroItem ai= pch.GetProductCatalogAllegroItem(itemId);

                //if (refreshJson == false && ai != null && ai.JsonAllegroItem != null)
                //{

                //    try
                //    {
                //        offer = json_serializer.Deserialize<Offer.RootObject>(ai.JsonAllegroItem);

                //        if (offer != null)
                //            return offer;
                //    }
                //    catch(Exception ex)
                //    {
                //        pch.SetProductCatalogAllegroItemClearJson(itemId);
                //        Bll.ErrorHandler.SendError(ex, String.Format("Błąd serializacji obiektu JSON. ItemId {0}<br><br>Json:{1}", itemId, ai.JsonAllegroItem));

                //    }
                //}

                HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offers/{0}", itemId), "GET", itemId, null);


                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }



                offer = json_serializer.Deserialize<Offer.RootObject>(text);

                return offer;
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania informacji o ofercie: {0}<br>{1}", itemId,text));
                return null;
            }
        }
        public static string GetOfferJson(long itemId)
        {
            string text = null;
            try
            { 
                var json_serializer = new JavaScriptSerializer();


                HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offers/{0}", itemId), "GET", itemId, null);


                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }

                 

                return text;
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania informacji o ofercie: {0}<br>{1}", itemId, text));
                return null;
            }
        }

        //public static void GetOffersStatus()
        //{ 

        //    Dal.AllegroScan asc = new Dal.AllegroScan();

        //    List<long> itemIds = asc.GetAllegroInactiveItems();


        //    foreach(long itemId in itemIds)
        //    {
        //        string text = null;
        //        try
        //        {
        //            var json_serializer = new JavaScriptSerializer();


        //            HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offers/{0}", itemId), "GET", itemId, null);


        //            using (WebResponse webResponse = request.GetResponse())
        //            {
        //                Stream responseStream = webResponse.GetResponseStream();
        //                StreamReader reader = new StreamReader(responseStream);
        //                text = reader.ReadToEnd();
        //            }


                     
        //        }
        //        catch (WebException ex)
        //        {
        //            using (WebResponse response = ex.Response)
        //            {
        //                HttpWebResponse httpResponse = (HttpWebResponse)response;
        //                if (httpResponse == null)
        //                {
                            

        //                } 
        //                using (Stream data = response.GetResponseStream())
        //                using (var reader = new StreamReader(data))
        //                {
        //                    string t = reader.ReadToEnd();

        //                    var json_serializer = new JavaScriptSerializer();
        //                    RootObject listing = json_serializer.Deserialize<RootObject>(t);

        //                    if (listing.errors.Where(x => x.code == "NOT_FOUND").Count() > 0)
        //                        asc.SetAllegroItemNotFoud(itemId);

        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string msg = ex.Message;
        //        }
        //    }


        //}
            public static void GetMissingOffers()
        {
            foreach (Dal.Helper.MyUsers e in Enum.GetValues(typeof(Dal.Helper.MyUsers)))
            {
                List<long> itemsNotIdSystem = new List<long>();
                int userId = (int)e;
               // int userId = 44282528;
                SetAllegroMissingOffers(userId, 100, 0, itemsNotIdSystem);
                Bll.ErrorHandler.SendEmail(String.Format("Brak w tablicy ProductCatalogAllegroItem {0}", String.Join(",", itemsNotIdSystem.ToArray())));
            }
        }
        //public static void GetOffersJson()
        //{
        //    foreach (Dal.Helper.MyUsers e in Enum.GetValues(typeof(Dal.Helper.MyUsers)))
        //    {
        //        int userId = (int)e;

        //        GetOffersJson(userId, 100, 0);

        //    }
        //}
        //internal static bool GetOffersJson(int userId, int limit, int offset)
        //{ 
        //    try
        //    {
        //        Dal.AllegroScan asc = new Dal.AllegroScan();
        //        Dal.AllegroUser au = asc.GetAllegroUser(userId);
        //        Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();


        //        if (au == null)
        //            return false;



        //        HttpWebRequest request = GetHttpWebRequest(String.Format("/offers/listing?seller.id={0}&limit={1}&offset={2}"
        //            , au.UserId, limit, offset), "GET", null, au.UserId);

        //        string text = null;
        //        using (WebResponse webResponse = request.GetResponse())
        //        {
        //            Stream responseStream = webResponse.GetResponseStream();
        //            StreamReader reader = new StreamReader(responseStream);
        //            text = reader.ReadToEnd();
        //        }

        //        var json_serializer = new JavaScriptSerializer();
        //        ListingRootObject listing = json_serializer.Deserialize<ListingRootObject>(text);
                
                 


        //        if (offset < listing.searchMeta.totalCount)

        //            GetOffersJson(userId, limit, offset + limit);


        //        return false;// promotions.totalCount > 50;
        //    }
        //    catch (Exception ex)
        //    {
        //        Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania obiektu JSON oferty. Allegro.UserId: {0}, ItemId :{1}", userId, itemIdtmp));
        //        return false;
        //    }
        //}
        internal static bool SetAllegroMissingOffers(int userId, int limit, int offset, List<long> itemsNotIdSystem)
        { 
            try
            {
                Dal.AllegroScan asc = new Dal.AllegroScan();
                Dal.AllegroUser au = asc.GetAllegroUser(userId);

                if (au == null)
                    return false;

                HttpWebRequest request = GetHttpWebRequest(String.Format("/offers/listing?seller.id={0}&limit={1}&offset={2}"
                    , au.UserId, limit, offset), "GET", null, au.UserId);

                string text = null;
                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }

                var json_serializer = new JavaScriptSerializer();
                ListingRootObject listing = json_serializer.Deserialize<ListingRootObject>(text);
                //Console.WriteLine(String.Format("Liczba ofert: {0}:", listing.items.regular.Count));

                //foreach (ListingRegular item in listing.items.regular)
                //    Console.WriteLine(String.Format("{0} {1}", item.id, item.name));


                long[] itemIds = listing.items.regular.Select(x => Int64.Parse(x.id)).ToArray();

                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

                long[] itemIdsNotExisting = itemIds.Except(pch.GetProductCatalogAllegroItems(itemIds).Select(x => x.ItemId).ToArray()).ToArray();

                if (itemIdsNotExisting.Count() > 0)
                    itemsNotIdSystem.AddRange(itemIdsNotExisting);
                if (offset < listing.searchMeta.totalCount)

                    SetAllegroMissingOffers(userId, limit, offset + limit, itemsNotIdSystem);


                return false;// promotions.totalCount > 50;
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd aktualizacji ceny. Allegro.UserId: {0}", userId));
                return false;
            }
        }

        /// <summary>
        /// Pobiera pojedynczo informaje o ofertach, które nie mają w systemie informacji o statusie
        /// </summary>
        

    }
}