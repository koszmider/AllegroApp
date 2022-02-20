using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Configuration;
using LajtIt.Dal;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Office.Interop.Excel;

namespace LajtIt.Bll
{
    #region classes

    public class AttributeGroup
    {
        public int pres_id { get; set; }//'] (int) - identyfikator zdjęcia
        public int lang_id { get; set; }//'] (int) - identyfikator produktu
        public int active { get; set; }//'] (int[0/1]) - czy zdjęcie jest zdjęciem głównym
        public int filters { get; set; }//'] (int) - kolejność zdjęcia
        public string name { get; set; } 
        public int[] categories { get; set; }//'] (string) - opis zdjęcia
        public ClickShopAttribute[] attributes { get; set; }//'] (string) - unikalna nazwa zdjęcia, określająca nazwę pliku w systemie
    }


    public class ClickShopAttribute
    {
        public int attribute_id { get; set; }//'] (int) - identyfikator zdjęcia 
        public string name { get; set; }
    }
  
    public class ShopCategoryTree
    {
        public int id { get; set; }
        public ShopCategoryTree[] children { get; set; }
    }
  
    public class ShopShippingType
    {
        public int shipping_id { get; set; }
        public decimal cost { get; set; }
        public string name { get; set; }
    }
    public class ShopPaymentType
    {
        public int payment_id { get; set; }
        public string name { get; set; }
    }
    public class ShopOrder
    {
        public int order_id { get; set; }
        public DateTime date { get; set; }
        public decimal sum { get; set; }
        public int payment_id { get; set; }
        public int shipping_id { get; set; }
        public decimal shipping_cost { get; set; }
        public decimal shipping_vat_value { get; set; }
        public decimal discount_code { get; set; }
        public string promo_code { get; set; }
        public bool is_overpayment { get; set; }
        public bool is_underpayment { get; set; }
        public bool is_paid { get; set; }
        public decimal paid { get; set; }
        public string email { get; set; }
        public string notes { get; set; }
        public ShopProduct[] products { get; set; }
        public ShopAddress billingAddress { get; set; }
        public ShopAddress deliveryAddress { get; set; }
        public ShippingAdditionalFields shipping_additional_fields { get; set; }
    }
    public class ShopProduct
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public int stock_id { get; set; }
        public decimal price { get; set; }
        public decimal discount_perc { get; set; }
        public int quantity { get; set; }
        public int delivery_time { get; set; }
        public string name { get; set; }
        public string pkwiu { get; set; }
        public string tax { get; set; }
        public decimal tax_value { get; set; }
        public string unit { get; set; }
        public string option { get; set; }
        public string code { get; set; }
    }
    public class ShippingAdditionalFields
    {
        public string machine { get; set; }
    }
    public class ShopAddress
    {
        public int address_id { get; set; }
        public int order_id { get; set; }
        public int type { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string company { get; set; }
        public string tax_id { get; set; }
        public string pesel { get; set; }
        public string city { get; set; }
        public string postcode { get; set; }
        public string street1 { get; set; }
        public string street2 { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string country_code { get; set; }

    }
 
    public class ShopDelivery
    {
        public int delivery_id { get; set; }
        public int days { get; set; }
    }

    #endregion

    #region ShopProduct
    public class ShopProductStock
    {
        public string stock_id { get; set; }
        public string extended { get; set; }
        public string price { get; set; }
        public string price_buying { get; set; }
        public string price_type { get; set; }
        public string stock { get; set; }
        public string package { get; set; }
        public string warn_level { get; set; }
        public string sold { get; set; }
        public string weight { get; set; }
        public string weight_type { get; set; }
        public string active { get; set; }
        public string @default { get; set; }
        public string product_id { get; set; }
        public object availability_id { get; set; }
        public string delivery_id { get; set; }
        public object gfx_id { get; set; }
        public string code { get; set; }
        public string ean { get; set; }
        public string comp_weight { get; set; }
        public string comp_price { get; set; }
        public string comp_promo_price { get; set; }
        public string price_wholesale { get; set; }
        public string comp_price_wholesale { get; set; }
        public string comp_promo_price_wholesale { get; set; }
        public string price_special { get; set; }
        public string comp_price_special { get; set; }
        public string comp_promo_price_special { get; set; }
        public string price_type_wholesale { get; set; }
        public string price_type_special { get; set; }
        public object calculation_unit_id { get; set; }
        public string calculation_unit_ratio { get; set; }
    }

    public class ShopProductPlPL
    {
        public string translation_id { get; set; }
        public string product_id { get; set; }
        public string name { get; set; }
        public string short_description { get; set; }
        public string description { get; set; }
        public string active { get; set; }
        public string lang_id { get; set; }
        public string isdefault { get; set; }
        public string seo_title { get; set; }
        public string seo_description { get; set; }
        public string seo_keywords { get; set; }
        public string order { get; set; }
        public string main_page { get; set; }
        public string main_page_order { get; set; }
        public string seo_url { get; set; }
        public string permalink { get; set; }
    }

    public class ShopProductTranslations
    {
        public ShopProductPlPL pl_PL { get; set; }
    }

    public class ShopProductRootObject
    {
        public string product_id { get; set; }
        public string producer_id { get; set; }
        public string group_id { get; set; }
        public string tax_id { get; set; }
        public string add_date { get; set; }
        public string edit_date { get; set; }
        public string other_price { get; set; }
        public string pkwiu { get; set; }
        public string unit_id { get; set; }
        public string in_loyalty { get; set; }
        public object loyalty_score { get; set; }
        public object loyalty_price { get; set; }
        public string bestseller { get; set; }
        public string newproduct { get; set; }
        public string dimension_w { get; set; }
        public string dimension_h { get; set; }
        public string dimension_l { get; set; }
        public string vol_weight { get; set; }
        public object currency_id { get; set; }
        public object gauge_id { get; set; }
        public string unit_price_calculation { get; set; }
        public string type { get; set; }
        public string category_id { get; set; }
        public List<int> categories { get; set; }
        public object promo_price { get; set; }
        public string code { get; set; }
        public string ean { get; set; }
        public ShopProductStock stock { get; set; }
        public ShopProductTranslations translations { get; set; }
        public List<object> options { get; set; }
        public bool is_product_of_day { get; set; }
    }

    #endregion
    public class ShopHelper  
    {
        string sessionId = null;

        public string SessionId
        {
            get { return sessionId; }
        }
        private static LajtIt.Bll.Helper.Cache<string, string> cache = new LajtIt.Bll.Helper.Cache<string, string>();
        #region Json
       
        public ShopHelper()
        {

            Login();
        }
        //public ShopHelper(bool loginToShop)
        //{
        //    if(loginToShop)
        //    Login();
        //}
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        // Dispose managed resources
        //    }

        //    // Free native resources
        //    Logout();
        //}

        //public void Dispose()
        //{

        //    Dispose(true);

        //    GC.SuppressFinalize(this);

        //}

        // Disposable types implement a finalizer.
        //~ShopHelper()
        //{
        //    Logout();
        //}

        private void Login()
        {

            //string session = cache.Get("ShopApiSession");
            //if (session != null)
            //{
            //    return session;

            //}
            //else
            //{
            Object[] methodParams = { System.Configuration.ConfigurationManager.AppSettings["ShopWebApiLogin"],
                System.Configuration.ConfigurationManager.AppSettings[String.Format("ShopWebApiPwd{0}", Dal.Helper.Shop.Lajtitpl.ToString())]
                                    };
            Object response = SendApiRequest("login", methodParams);



            if (response is Dictionary<String, Object>)
            {
                Dictionary<String, Object> d = (Dictionary<String, Object>)response;
                if (d.ContainsKey("error"))
                {
                    Console.WriteLine("Wystąpił błąd: {0}, kod: {1}", d["error"], d["code"]);
                }
            }
            else if (response is String)
            {
                sessionId = (String)response;
                //cache.Set("ShopApiSession", session);
            }
            // }

            Console.WriteLine(String.Format("Session START: {0}", sessionId));
            //return session;
        }

        private void Logout()
        {
            //Console.WriteLine("Logout");
            //string session = cache.Get("ShopApiSession");
            if (SessionId != null)
            {
                Object[] methodParams = { SessionId };
                Object response = SendApiRequest("logout", methodParams);
                Console.WriteLine(String.Format("Session END: {0}", SessionId));
                //cache.Set("ShopApiSession", null);

            }
        }
        //public static void Logout(string session)
        //{ 
        //    if (session != null)
        //    {
        //        Object[] methodParams = { session };
        //        Object response = SendApiRequest("logout", methodParams);
        //        Console.WriteLine(String.Format("Session END: {0}", session));
        //        cache.Set("ShopApiSession", null);


        //    }
        //}


        public static Object SendApiRequest(String method, Object[] methodParams)
        {
            try
            {
                Dal.ShopHelper.LogApiCall(method, methodParams);
            }
            catch (Exception ex)
            {

                Bll.ErrorHandler.SendError(ex, "SendApiRequest");
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebRequest request = WebRequest.Create("https://lajtit.pl/webapi/json/");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            Stream dataStream = request.GetRequestStream();

            Dictionary<String, Object> postParams = new Dictionary<String, Object>();
            postParams.Add("method", method);
            postParams.Add("params", methodParams);

            string jsonEncodedParams = Bll.RESTHelper.ToJson(postParams);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] byteArray = encoding.GetBytes("json=" + jsonEncodedParams);

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse webResponse = request.GetResponse();
            Stream responseStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string text = reader.ReadToEnd();

            Object response = Bll.RESTHelper.FromJson(text);
            return response;
        }
        #endregion


        #region process Products

        public int[] GetShopMainPage()
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("translations.pl_PL.main_page", "1");


            Object[] att = { conditions, "product_id", null };
            Object[] methodParams = { SessionId, "product.list.filter", att };

            var r = SendApiRequest("call", methodParams);
            var i = (SendApiRequest("call", methodParams) as object[]).Cast<int>().ToArray();



            return i;
        }


        public object GetShopProductsNotInSystem()
        {
            /*
             *array call(string $session_id, "product.list", array($extended = false, $translations = false, $options = false, $gfx = false, $attributes = false, $products = ""))
             * */

            int start = 20631;
            Dal.ShopHelper sh = new Dal.ShopHelper();

            while (true)
            {
                int productId = start++;
                Bll.ShopProductRootObject product = GetShopProduct(productId);

                if(product.translations!=null)
                {

                    Dal.ShopProduct sp = new Dal.ShopProduct()
                    {
                        Name = product.translations.pl_PL.name,
                        ShopId = 1,
                        ShopProductId = Convert.ToInt32(product.product_id),
                        Url = product.translations.pl_PL.permalink,
                        Code = product.code
                    };
                    sh.SetShopProduct(sp);
                    Console.WriteLine("{0} - 1", productId);
                }
                else
                    Console.WriteLine("{0} - 0", productId);
                if (start == 25000)
                    break;

            }


            return null;
           }


           public void SetShopMainPage(Dal.Helper.Shop shop)
           {

               Dal.ShopHelper sh = new Dal.ShopHelper();
               Dal.SettingsHelper sett = new SettingsHelper();
               int count = sett.GetSetting("SHOP_MP").IntValue.Value;

               List<Dal.ProductCatalogShopMainPageFnResult> products = sh.GetProductCatalogForShopMainPage(shop, count);

               Dictionary<string, object> conditions = new Dictionary<string, object>();
               conditions.Add("translations.pl_PL.main_page", "1");


               Object[] att = { conditions, "product_id", null };
               Object[] methodParams = { SessionId, "product.list.filter", att };

               var result = SendApiRequest("call", methodParams);
               if (result != null)
               {
                   int[] productIds = (SendApiRequest("call", methodParams) as object[]).Cast<int>().ToArray();

                   foreach (int productId in productIds)
                   {

                       Dictionary<string, object> d = new Dictionary<string, object>();
                       Dictionary<string, object> dt = new Dictionary<string, object>();
                       Dictionary<string, object> t = new Dictionary<string, object>();
                       t.Add("main_page", 0);//']  
                       dt.Add("pl_PL", t);
                       d.Add("translations", dt);

                       Object[] mp = { SessionId, "product.save", new Object[] { productId, d, true } };

                       var r = SendApiRequest("call", mp);
                   }
               }

               foreach (var s in products)
               {

                   Dictionary<string, object> d = new Dictionary<string, object>();
                   Dictionary<string, object> dt = new Dictionary<string, object>();
                   Dictionary<string, object> t = new Dictionary<string, object>();
                   t.Add("main_page", 1);
                   t.Add("main_page_order", s.ShopProductPriority);
                   dt.Add("pl_PL", t);
                   d.Add("translations", dt);

                   Object[] mp = { SessionId, "product.save", new Object[] { s.ShopProductId, d, true } };

                   var r = SendApiRequest("call", mp);
               }


           }

        //   public  void SetAllegroUpdateByCatalog(string actingUser)
        //   {
        //       Bll.ProductCatalogHelper pchB = new Bll.ProductCatalogHelper();
        //       pchB.SetProductCatalogScheduleDeleteDuplicates();
        //       //var watch = System.Diagnostics.Stopwatch.StartNew();
        //       //// the code that you want to measure comes here

        //       //int[] i = new[] { 1, 2, 3, 4, 5, 6 };
        //       //Parallel.ForEach(i, x =>
        //       //{
        //           SetAllegroUpdateByCatalogThread();
        //       //});
        //       //watch.Stop();
        //       //var elapsedMs = watch.ElapsedMilliseconds;

        //       //Console.WriteLine("mms {0} liczba {1} - {2}", elapsedMs, i.Count(), elapsedMs / i.Count());
        //   }
        //   private void SetAllegroUpdateByCatalogThread()
        //   { 
        //       UpdateByCatalog(Dal.Helper.ShopType.Allegro, Dal.Helper.UpdateScheduleType.OnlineShopSingle, "System");

        //   }

        //   internal void SetShopUpdateByCatalogSingle(string actingUser)
        //   {
        //       Bll.ProductCatalogHelper pchB = new Bll.ProductCatalogHelper();
        //       pchB.SetProductCatalogScheduleDeleteDuplicates();

        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    // the code that you want to measure comes here

        //    int[] i = new[] { 1, 2, 3, 4, 5, 6 };
        //    Parallel.ForEach(i, x =>
        //    {
        //        SetShopUpdateByCatalogSingleThread();
        //    });
        //    watch.Stop();
        //    var elapsedMs = watch.ElapsedMilliseconds;

        //Console.WriteLine("mms {0} liczba {1} - {2}", elapsedMs, i.Count(), elapsedMs / i.Count());
        //}
        //   private void SetShopUpdateByCatalogSingleThread()
        //   {

        //       UpdateByCatalog(Dal.Helper.ShopType.ClickShop, Dal.Helper.UpdateScheduleType.OnlineShopSingle, "System");

        //   }






        internal void SetOrderStatuses(Dal.Helper.Shop shop)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<Dal.ShopOrder> shopOrders = sh.GetShopOrdersToUpdateStatus(shop);

            foreach (Dal.ShopOrder so in shopOrders)
            {
                bool result = false;
                ShopStatus ss;
                switch (shop)
                {
                    case Dal.Helper.Shop.Lajtitpl:
                        ss = Dal.DbHelper.Shop.GetShopOrderStatus(Dal.Helper.ShopType.ShoperLajtitPl, so.Order.OrderStatusId);
                        if (ss != null)
                            result = SetOrderStatus(Int32.Parse(so.ShopOrderNumber), ss.ShopStatusId);
                        break;

                    case Dal.Helper.Shop.Empik:
                        if (so.Order.OrderStatusId == (int)Dal.Helper.OrderStatus.Sent)
                            result = EmpikRESTHelper.Orders.SetSentStatus(so.ShopOrderNumber);
                        break;
                    case Dal.Helper.Shop.Erli:
                        ss = Dal.DbHelper.Shop.GetShopOrderStatus(Dal.Helper.ShopType.Erli, so.Order.OrderStatusId);
                        if (ss != null)
                            result = ErliRESTHelper.Orders.SetSentStatus(shop, so, ss.ShopStatusId);
                        break;

                }

                if (result)
                    sh.SetOrderUpdateStatusCompleted(so.ShopOrderNumber, shop);
            }
        }
            
           //public void AssignShopProductToProductCatalog()
           //{

           //    //string session = LoginJason();
           //    //Dictionary<string, string> code = new Dictionary<string, string>();
           //    //code.Add("category_id", "18");
           //    //code.Add("product_id", "141");
           //    //Object[] att = { code, "product_id", 1};
           //    //Object[] methodParams = { session, "product.list.filter", att };

           //    //var r = SendApiRequest("call", methodParams);
           //    //var json_serializer = new JavaScriptSerializer();
           //    //json_serializer.MaxJsonLength = Int32.MaxValue;
           //    //ShopProductShort[] shopProduct = json_serializer.Deserialize<ShopProductShort[]>(ToJson(r));

           //    //   string s = "";



           //    Object[] att = { true, false, false, false, false, null };
           //    Object[] methodParams = { SessionId, "product.list", att };

           //    var r = SendApiRequest("call", methodParams);
           //    var json_serializer = new JavaScriptSerializer();
           //    json_serializer.MaxJsonLength = Int32.MaxValue;
           //    ShopProductShort[] shopProducts = json_serializer.Deserialize<ShopProductShort[]>(ToJson(r));

           //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

           //    var q = shopProducts.Where(x => x.promo_price != null).ToList();

           //    foreach (ShopProductShort p in shopProducts)
           //    {
           //        try
           //        {


           //            //string ss = "";
           //            //if (p.code.Contains("9960102"))
           //            //    ss = "ok";
           //            pch.SetShopProductToProductCatalogByCode(p.code, p.product_id);
           //        }
           //        catch (Exception ex)
           //        {
           //            LajtIt.Bll.ErrorHandler.SendError(ex, "AssignShopProductToProductCatalog");
           //        }

           //    }


           //    string s = "";
           //}


    

           internal void SetShopProductsUpdate(Dal.Helper.Shop shop)
           {
            //throw new NotImplementedException("TO DO");
            Dal.ShopHelper sh = new Dal.ShopHelper();

            List<Dal.ProductCatalogForShopResult> products = sh.GetProductCatalogShopUpdate((int)shop);

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources(Dal.Helper.ShopType.ShoperLajtitPl);

            SettingsHelper s = new SettingsHelper();
            int deliveryTime = s.GetSetting("SHOPMINDEL").IntValue.Value;

            //foreach (Dal.ProductCatalogForShop product in products)
            //    SetShopPriorityOrder(product, session, deliveryTime);


            int count = products.Count;
            int size = 300;
            List<int> t = new List<int>();

            int partsCount = (count / size);

            List<Dal.ProductCatalogForShopResult> productsPart = null;
            for (int i = 0; i < partsCount + 1; i++)
            {
                Dictionary<string, object> productsToUpdate = new Dictionary<string, object>();
                productsPart = products.Skip(i * size).Take(size).ToList();
                foreach (Dal.ProductCatalogForShopResult product in productsPart)
                    productsToUpdate.Add(product.ShopProductId.ToString(), SetShopPriorityOrder(product, deliveryTime, sources));

                SetShopPriorityOrderSendToShop(productsToUpdate);
            }



        }

        public void SetShopDescriptions(int[] productIds, Dal.Helper.Shop shop)
        {
            List<Dal.ProductCatalogShopProduct> psp = new List<ProductCatalogShopProduct>();

            foreach (int pId in productIds)
            {
                psp.Add(new ProductCatalogShopProduct()
                {
                    ShopId= (int)shop,
                    ProductCatalogId=pId,
                    LongDescription= Bll.Mixer.GetDescription(shop, pId)
                    
                }); 
                }



            Dal.DbHelper.ProductCatalog.SetProductCatalogShopProductDescription(psp);  


        }


        /*
           public UpdateResult SetProduct(Dal.ProductCatalogView pc,
               List<Dal.ProductCatalogAttributeCategory> categories,
               List<Dal.SupplierDeliveryTypeSource> sources,
               bool createProduct,
               string updateCommand)
        {
            //Dal.ShopHelper sh = new Dal.ShopHelper();
            //Dal.ProductCatalogAttributeCategory category = sh.GetProductCatalogShopCategory(pc.ProductCatalogId, Dal.Helper.ShopType.ClickShop);

            //if (category == null)
            if(categories.Count==0)
                return null;
            if (createProduct && pc.IsActiveOnline == false)
                return null;

            Dictionary<string, object> d = new Dictionary<string, object>();

            int result = 0;
            int product_id = 0;
            try
            {

                d = GetProductForShop(pc, categories, sources, updateCommand, createProduct);


                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

                 
                if (createProduct)
                {
                    Object[] methodParams = { SessionId, "product.create", new Object[] { d } };

                    var r = SendApiRequest("call", methodParams);

                    Dictionary<string, object> dic = r as Dictionary<string, object>;

                    product_id = (int)r;

                    if (product_id > 0)
                    {
                        result = 1;
                        pch.SetShopProductToProductCatalogById(pc.ProductCatalogId, product_id);
                        pc.ShopProductId = product_id;
                    }
                    else
                    {
                        result = product_id; 
                    }
                }
                else
                {
                    product_id = pc.ShopProductId.Value;
                    if (d.Count > 0)
                    {
                        Object[] methodParams = { SessionId, "product.save", new Object[] { product_id, d, true } };
                        var r = SendApiRequest("call", methodParams);
                        result = (int)r;
                    }
                    else
                        result = 1;

                }
                if (result == 1)
                {

                    if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Images))
                        SetImagesUpdate(product_id);
                    if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.PricePromo))
                        SetPricePromo(pc, product_id);
                    if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Category))
                    {
                        if (!pc.IsActiveAllegro)
                            DettachCategories(true, product_id, "563");
                        else
                            SetProductCategoriesFromAttributes(pc.ProductCatalogId);
                    }
                    if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Related))
                        SetRecommendedProducts(pc);


                    if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Attributes))
                    {


                        SetShopProductAttributes(pc.ProductCatalogId, pc.ShopProductId);

                        if (!pc.IsDiscontinued)
                            SetProductCategoriesFromAttributes(pc.ProductCatalogId);
                    }

                }
            }
            catch (Exception ex)
               {
                   Bll.ErrorHandler.SendError(ex, String.Format("SetProduct. ProductCatalogId: {0}<br><Br>{1}", pc.ProductCatalogId, ex.Message));

                   throw ex;

               }
               return new UpdateResult() { Result = result, ShopProductId = product_id }; ;
           }
           */


        //public static int? GetDeliveryIdForShop(int deliveryId, List<Dal.SupplierDeliveryTypeSource> sources)
        //{

        //    return sources.Where(x => x.DeliveryId == deliveryId).Select(x => x.ExternalValue).FirstOrDefault();
        //}

        private Dictionary<string, object> SetShopPriorityOrder(ProductCatalogForShopResult product, int deliveryTime, List<Dal.SupplierDeliveryTypeSource> sources)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            Dictionary<string, object> dt = new Dictionary<string, object>();
            Dictionary<string, object> t = new Dictionary<string, object>();
            t.Add("order", product.ShopProductPriority ?? product.ProductCatalogId);
            //t.Add("active", product.IsActiveOnline);//'] (int [0/1]) - aktywność produktu



            Dictionary<string, object> s = new Dictionary<string, object>();


            int shopDeliveryTimeId = ShopUpdateHelper.ClickShop. GetDeliveryTime(product.LeftQuantity.Value, ShopUpdateHelper.ClickShop.GetDeliveryIdForShop(product.DeliveryId, sources), deliveryTime);

            s.Add("delivery_id", shopDeliveryTimeId);
            s.Add("stock", ShopUpdateHelper.ClickShop.GetStock(product.SupplierQuantity, 
                product.LeftQuantity.Value, product.IsPSActive.Value, product.IsDiscontinued));


            d.Add("stock", s);//'] (array) - wymagana tablica asocjacyjna z informacjami o magazynie wariantu 



            dt.Add("pl_PL", t);
            d.Add("translations", dt);

            return d;
        }

        private void SetShopPriorityOrderSendToShop(Dictionary<string, object> products)
           {
               Object[] mp = { SessionId, "product.list.save", new Object[] { products, true } };

               var r = SendApiRequest("call", mp);
           }
           //public static int GetDeliveryTime(int leftQuantity, int? shopDeliveryDays, int deliveryTime)
           //{
           //    if (leftQuantity > 0)
           //        return deliveryTime;//'] (int]) - identyfikator czasu dostawy wariantu
           //    else
           //    {
           //        if (shopDeliveryDays.HasValue)
           //            return shopDeliveryDays.Value;//'] (int]) - identyfikator czasu dostawy wariantu
           //        else
           //            return 5;//'] (int]) - identyfikator czasu dostawy wariantu
           //    }


           //}
           //private int GetStock(int? supplierQuantity, int leftQuantity, bool isAvailable, bool isDiscountinued)
           //{
           //    int defaultQuantity = 50;

           // if (isDiscountinued)
           //     if (leftQuantity > 0)
           //     {
           //         return leftQuantity;
           //     }
           //     //else
           //     //{ return 0; }

           // if (supplierQuantity.HasValue && supplierQuantity.Value > 0)
           //     return supplierQuantity.Value + leftQuantity;



           //    // jeśli produkt jest na magazynie ale nie jest dostępny u dostawcy, to wystaw tylko ilość 
           //    // z naszego magazynu. W przeciwnym razie wystaw więcej produktów.
           //    if (leftQuantity > 0 && !isAvailable)
           //    {
           //        return leftQuantity;
           //    }
           //    else
           //    {
           //        return defaultQuantity;
           //    }

           //}
           //internal void CreateUpdateProducts()
           //{
           //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
           //    int[] productCatalogIds = pch.GetProductCatalogForShopCreateUpdate();

           //    foreach (int pId in productCatalogIds)
           //        try
           //        {
           //            SetProductUpdateByProductCatalogId(pId, null);
           //        }
           //        catch (Exception ex)
           //        {
           //            ErrorHandler.SendError(ex, String.Format("Błąd aktualizacji/tworzenia produktu w sklepie. ProductCatalogId {0}", pId));

           //        }

           //}

        //   public bool SetProductUpdateByShopId(int shopProductId)
        //   {
        //       Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //       Dal.ProductCatalogView pc =
        //       pch.GetProductCatalogOnShopProductId(shopProductId);

        //       List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources(Dal.Helper.ShopType.ClickShop);



        //       Dal.ShopHelper sh = new Dal.ShopHelper();
        //       List<Dal.ProductCatalogAttributeCategory> categories = pch.GetShopProductAndCategoriesFromAttributes(new int[] { pc.ProductCatalogId });
        //       UpdateResult result = SetProduct(pc, categories, sources, false, null);

        //       return result != null && result.Result==1;

        //}
        //public bool SetProductUpdateByProductCatalogId(int productCatalogId)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    Dal.ProductCatalogView pc =
        //    pch.GetProductCatalogView(productCatalogId);

        //    List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources(Dal.Helper.ShopType.ClickShop);



        //    Dal.ShopHelper sh = new Dal.ShopHelper();
        //    List<Dal.ProductCatalogAttributeCategory> categories = pch.GetShopProductAndCategoriesFromAttributes(new int[] { pc.ProductCatalogId });
        //    UpdateResult result = SetProduct(pc, categories, sources, true, null);

        //    return result != null && result.Result == 1;

        //}
        //public List<UpdateResult> SetProductUpdateByProductCatalogId(List<Dal.ProductCatalogUpdateScheduleView> schedules,
        //       Dal.Helper.UpdateScheduleType scheduleType)
        //   {
        //       if (schedules.Count == 0)
        //           return new List<UpdateResult>();

        //       Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //       Dal.ShopHelper sh = new Dal.ShopHelper();
        //       Dictionary<string, object> products = new Dictionary<string, object>();

        //       int[] productCatalogIds = schedules.Select(x => x.ProductCatalogId).Distinct().ToArray();
        //       List<Dal.ProductCatalogView> pcViews = pch.GetProductCatalogView(productCatalogIds);
        //    List<Dal.ProductCatalogAttributeCategory> categories = pch.GetShopProductAndCategoriesFromAttributes()
        //     .Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();

        //       List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources(Dal.Helper.ShopType.ClickShop);

        //       foreach (Dal.ProductCatalogUpdateScheduleView schedule in schedules)
        //       {
        //           Dal.ProductCatalogView pc = pcViews.Where(x => x.ProductCatalogId == schedule.ProductCatalogId).FirstOrDefault();

        //           Dictionary<string, object> product = GetProductForShop(pc, categories, sources, schedule.UpdateCommand, false);
        //           if (product != null && product.Count>0)
        //               products.Add(pc.ShopProductId.Value.ToString(), product);

        //       }

        //    if (products.Count > 0)
        //    {
        //        Object[] methodParams = { SessionId, "product.list.save", new Object[] { products, true } };

        //        var r = SendApiRequest("call", methodParams);
        //        //Dal.ErrorHandler.LogError(String.Format("SetProduct product.save: result {0}", r));
        //        //  if ((int)r == 1)
        //        //      result = true;  

        //        Dictionary<string, object> result = r as Dictionary<string, object>;

        //        return result.Select(x => new UpdateResult() { ShopProductId = Convert.ToInt32(x.Key), Result = Convert.ToInt32(x.Value) })
        //            .ToList();
        //    }
        //    else

        //        return new List<UpdateResult>() ;
        //}

        public void DeleteProducts(string[] ids)
        {

            Object[] att = { ids, true };
            Object[] methodParams = { SessionId, "product.list.delete", att };

            var r = SendApiRequest("call", methodParams);


        }
        public class UpdateResult
           {
               public int ShopProductId { get; set; }
               public int ProductCatalogId { get; set; }
               public int Result { get; set; }
           }

        //   private Dictionary<string, object> GetProductForShop(Dal.ProductCatalogView pc,
        //       List<Dal.ProductCatalogAttributeCategory> categories,
        //       List<Dal.SupplierDeliveryTypeSource> sources,
        //       string updateCommand,
        //       bool createProduct)
        //   {
        //       Dal.ProductCatalogAttributeCategory category = categories.Where(x => x.ProductCatalogId == pc.ProductCatalogId 
        //       && x.IsMainCategory==1
        //       && x.CategoryId!=null).FirstOrDefault();

        //       if (category == null)
        //           return null;
        //       //if (createProduct && pc.IsActiveOnline == false)
        //       //    return false;

        //       Dictionary<string, object> d = new Dictionary<string, object>();

        //       try
        //       {
        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.All))
        //            d.Add("producer_id", pc.ShopProducerId);//'] (int) - identyfikator producenta
        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.All))
        //            d.Add("tax_id", 1);//'] (int) - identyfikator stawki podatkowej
        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Category)
        //            || CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Status))
        //            d.Add("category_id", GetCategory(pc, category.CategoryId));// category.CategoryId.Value);//'] (int) - identyfikator głównej kategorii
        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Attributes))
        //            d.Add("attributes", GetShopAttributes(pc.ProductCatalogId));//'] (int) - identyfikator głównej kategorii

        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.All))
        //            d.Add("unit_id", 1);//'] (int) - identyfikator jednostki miary
        //                                //d.Add("other_price", );//'] (double) - cena produktu w innych sklepach

        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.All))
        //            d.Add("code", CheckProductCode(pc, createProduct));//'] (string) - kod produktu
        //                                                               //d.Add("pkwiu", );//'] (string) - pkwiu produktu

        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Ean))
        //            d.Add("ean", pc.Ean);//'] (string) - kod produktu


        //        Dictionary<string, object> s = new Dictionary<string, object>();

        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Price))
        //        {
        //            decimal p = 0.01M;

        //            if (!pc.OnlineShopLockRebates || pc.IsActivePricePromo)
        //                s.Add("price", pc.PriceBruttoShop);//'] (double) - cena wariantu podstawowego
        //            else
        //                s.Add("price", pc.PriceBruttoShop + p);//'] (double) - cena wariantu podstawowego

        //            Dictionary<string, object> spo = new Dictionary<string, object>();

        //            if (pc.IsActivePricePromo || pc.OnlineShopLockRebates)
        //            {
        //                if (pc.OnlineShopLockRebates && !pc.IsActivePricePromo)
        //                {
        //                    spo.Add("datefrom", String.Format("{0:yyyy-MM-dd}", DateTime.Now)); //(string) - data(w formacie yyyy - mm - dd) rozpoczęcia promocji
        //                    spo.Add("dateto", String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddYears(1))); //(string) - data(w formacie yyyy - mm - dd) zakończenia promocji
        //                    spo.Add("promoprice", pc.PriceBruttoShop);
        //                }
        //                else // jeśli blokjemy rabaty to trzeba ustawic cenę promocyjną jako cenę regularną. Hack
        //                {
        //                    spo.Add("datefrom", String.Format("{0:yyyy-MM-dd}", DateTime.Now)); //(string) - data(w formacie yyyy - mm - dd) rozpoczęcia promocji
        //                    spo.Add("dateto", String.Format("{0:yyyy-MM-dd}", pc.PriceBruttoPromoDate.Value)); //(string) - data(w formacie yyyy - mm - dd) zakończenia promocji
        //                    spo.Add("promoprice", pc.PriceBruttoPromo);
        //                }


        //                d.Add("specialOffer", spo);//'] (array) - wymagana tablica asocjacyjna z informacjami o magazynie wariantu 
        //            }
        //            else
        //            {
        //                //if (createProduct == false)
        //                //{
        //                //    Object[] methodParams = { session, "product.promo.delete", new Object[] { pc.ShopProductId.Value, true } };
        //                //    var r = SendApiRequest("call", methodParams);

        //                //}

        //            }


        //        }


        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Quantity))
        //        {
        //            int deliveryTime = Bll.Helper.GetSetting("SHOPMINDEL").IntValue.Value;
        //            int shopDeliveryTimeId = GetDeliveryTime(pc.LeftQuantity, GetDeliveryIdForShop(pc.DeliveryId, sources), deliveryTime);
        //            s.Add("delivery_id", shopDeliveryTimeId);
                
        //        }

        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Quantity))
        //           {
        //               s.Add("stock", GetStock(pc.SupplierQuantity, pc.LeftQuantity, pc.IsAvailable, pc.IsDiscontinued));//'] (float) - stan magazynowy
        //           }

        //           //s.Add("stock_relative", );//'] (float) - różnica stanu magazynowego o jaką ma zostać zaktualizowa wartość w sklepie. Jeśli ten parametr występuje, parametr 'stock' nie jest brany pod uwagę podczas aktualizacji danych.
        //           if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.All))
        //               s.Add("warn_level", 5);//'] (float) - alarm magazynowy
        //                                      //s.Add("sold", );//'] (float) - ilość sprzedanego towaru
        //                                      //s.Add("sold_relative", );//'] (float) - różnica ilości sprzedanego towaru o jaką ma zostać zaktualizowa wartość w sklepie. Jeśli ten parametr występuje, parametr 'sold' nie jest brany pod uwagę podczas aktualizacji danych.
        //                                      //s.Add("weight", );//'] (float) - waga towaru
        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Status))
        //            s.Add("availability_id", GetAvailability(pc) );//'] (int) - identyfikator dostępności wariantu


        //            /*  if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.All))
        //                  if (pc.Supplier.ShopDeliveryDays.HasValue)
        //                      s.Add("delivery_id", pc.Supplier.ShopDeliveryDays.Value);//'] (int]) - identyfikator czasu dostawy wariantu
        //                  else
        //                      s.Add("delivery_id", 5);//'] (int]) - identyfikator czasu dostawy wariantu
        //              *///s.Add("gfx_id", );//'] (int|null) - identyfikator zdjęcia produktu, które przedstawia dany warian
        //            if (s.Count > 0)
        //            d.Add("stock", s);//'] (array) - wymagana tablica asocjacyjna z informacjami o magazynie wariantu 


        //        Dictionary<string, object> dt = new Dictionary<string, object>();
        //        Dictionary<string, object> t = new Dictionary<string, object>();


        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Name))
        //            t.Add("name", pc.Name);//'] (string) - nazwa produktu
        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Description))
        //        {
        //            Dal.ProductCatalogDescriptionFnResult desc = GetDescription(pc.ProductCatalogId, Dal.Helper.Shop.Lajtitpl);
        //            if (desc != null)
        //            {
        //                t.Add("short_description", desc.ShortDescription);//'] (string) - krótki opis produktu
        //                t.Add("description", desc.LongDescription);//'] (string) - opis produktu
        //            }
        //            else
        //            {
        //                t.Add("short_description", null);//'] (string) - krótki opis produktu
        //                t.Add("description", null);//'] (string) - opis 

        //            }
        //        }
        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.Status))
        //            //t.Add("active", pc.IsActiveOnline);//'] (int [0/1]) - aktywność produktu
        //            t.Add("active", true);//'] (int [0/1]) - aktywność produktu ZAWSZE AKTYWNE, ZMIENIAMY JEDYNIE WIDOCZNOŚĆ OFERTY
        //                                               //t.Add("seo_title", );//'] (string) - tytuł wyświetlany w tagu <title>
        //                                               //t.Add("seo_description", );//'] (string) - opis wyświetlany w tagu meta description
        //                                               //t.Add("seo_keywords", );//'] (string) - opis wyświetlany w tagu meta keywords
        //                                               //if(!String.IsNullOrEmpty(pc.Ean))
        //                                               //    t.Add("seo_url", GetFriendlyUrl(pc));//'] (string) - opis wyświetlany w tagu meta keywords
        //        if (CanUpdateField(createProduct, updateCommand, Dal.Helper.ProductUpdateFlag.All))
        //            t.Add("order", pc.ShopProductPriority ?? pc.ProductCatalogId);//'] (int) - priorytet brany pod uwagę podczas sortowania listy produktów
        //                                                                          //t.Add("main_page", );//'] (int [0/1]) - czy produkt został wyróżniony na stronie głównej
        //                                                                          //t.Add("main_page_order", );//'] (int) - priorytet brany pod uwagę podczas sortowania listy produktów na stronie głównej

        //        if (t.Count > 0)
        //        {
        //            dt.Add("pl_PL", t);
        //            d.Add("translations", dt);
        //        }

        //        return d;

        //    }
        //    catch (Exception ex)
        //    {
        //        Bll.ErrorHandler.LogError(ex, String.Format("SetProduct. ProductCatalogId: {0}", pc.ProductCatalogId));
        //        //result = false;
        //        throw ex;

        //    }
        //}

        //private Dal.ProductCatalogDescriptionFnResult GetDescription(int productCatalogId, Dal.Helper.Shop shop)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    Dal.ProductCatalogDescriptionFnResult d = pch.GetProductCatalogDescription(productCatalogId, shop);
        //    return d;
        //}

        //private int? GetAvailability(ProductCatalogView pc)
        //{
        //    if (pc.IsActiveOnline || (pc.IsAvailableOnline && pc.LeftQuantity > 0) || (pc.IsDiscontinued && pc.LeftQuantity > 0))
        //        return null; // dostępny

        //    if (pc.IsDiscontinued)
        //        return 7; //wycofany z oferty
             
        //    else
        //        return 3; // spodziewana dostawa
        //}

        //private string GetCategory(ProductCatalogView pc, string categoryId)
        //{
        //    if (pc.IsActiveOnline)
        //        return categoryId; 
        //    else
        //        return "563"; // Archiwum
        //}

        //public UpdateResult SetProductUpdateByProductCatalogId(

        //    int productCatalogId,
        //    string updateCommand,
        //    Dal.Helper.UpdateScheduleType scheduleType,
        //    List<Dal.SupplierDeliveryTypeSource> sources)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    Dal.ProductCatalogView pc = pch.GetProductCatalogView(productCatalogId);
        //    bool createProduct = pc.ShopProductId.HasValue == false;
        //    ShopHelper sh = new ShopHelper();
        //    List<Dal.ProductCatalogAttributeCategory> categories = pch.GetShopProductAndCategoriesFromAttributes(new int[] { pc.ProductCatalogId });

        //    Console.WriteLine(String.Format("Shop SetProductUpdateByProductCatalogId: {0}", pc.ShopProductId));
        //    return SetProduct(pc, categories, sources, createProduct, updateCommand);
             
        //}
          

        //public void SetShopMainPage(int shopMainPageGroupId)
        //{
        //    Dictionary<string, object> conditions = new Dictionary<string, object>();
        //    conditions.Add("translations.pl_PL.main_page", "1");


        //    Object[] att = { conditions, "product_id", null };
        //    Object[] methodParams = { SessionId, "product.list.filter", att };

        //    var result = SendApiRequest("call", methodParams);
        //    int[] productIds = (SendApiRequest("call", methodParams) as object[]).Cast<int>().ToArray();

        //    foreach (int productId in productIds)
        //    {

        //        Dictionary<string, object> d = new Dictionary<string, object>();
        //        Dictionary<string, object> dt = new Dictionary<string, object>();
        //        Dictionary<string, object> t = new Dictionary<string, object>();
        //        t.Add("main_page", 0);//']  
        //        dt.Add("pl_PL", t);
        //        d.Add("translations", dt);

        //        Object[] mp = { SessionId, "product.save", new Object[] { productId, d, true } };

        //        var r = SendApiRequest("call", mp);
        //        //Dal.ErrorHandler.LogError(String.Format("Usuwanie znacznika main_page dla {0} wynik {1}", productId, r));
        //    }

        //    Dal.ShopHelper sh = new Dal.ShopHelper();
        //    List<Dal.ShopMainPageView> newProducts = sh.GetShopMainPageProducts(shopMainPageGroupId);

        //    foreach (Dal.ShopMainPageView s in newProducts)
        //    {

        //        Dictionary<string, object> d = new Dictionary<string, object>();
        //        Dictionary<string, object> dt = new Dictionary<string, object>();
        //        Dictionary<string, object> t = new Dictionary<string, object>();
        //        t.Add("main_page", 1);
        //        t.Add("main_page_order", 100 - s.Priority);
        //        dt.Add("pl_PL", t);
        //        d.Add("translations", dt);

        //        Object[] mp = { SessionId, "product.save", new Object[] { s.ShopProductId.Value, d, true } };

        //        var r = SendApiRequest("call", mp);
        //        //Dal.ErrorHandler.LogError(String.Format("Ustawianie znacznika main_page dla {0} wynik {1}", s.ShopProductId, r));
        //    }


        //}


        //private string CheckProductCode(ProductCatalogView pc, bool createProduct)
        //{
        //    if (createProduct)
        //    {
        //        Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //        List<Dal.ProductCatalog> products = pch.GetProductCatalogByCode(pc.Code);

        //        if (products.Where(x => x.ProductCatalogId != pc.ProductCatalogId && x.ShopProductId.HasValue).Count() > 0)
        //        {
        //            int addNumber = pch.SetProductCatalogCodeAddNumber(pc);
        //            return String.Format("{0} {1}", pc.Code, addNumber);
        //        }
        //        else
        //            return pc.Code;

        //    }
        //    else
        //        if (pc.CodeAddNumber.HasValue)
        //        return String.Format("{0} {1}", pc.Code, pc.CodeAddNumber.Value);
        //    else
        //        return pc.Code;
        //}

        private string GetFriendlyUrl(ProductCatalog pc)
        {
            throw new NotImplementedException();
        }

        //public static bool CanUpdateField(bool createProduct, string updateCommand, Dal.Helper.ProductUpdateFlag updateFlag)
        //{
        //    if (createProduct)
        //        return true;
        //    if (updateCommand == null)
        //        return true;
        //    updateCommand = String.Format("{0}00000000000", updateCommand);// na wypadek dodania nowych pól do aktualizacji

        //    string flag = updateCommand.Substring((int)updateFlag, 1);
        //    string flagAll = updateCommand.Substring((int)Dal.Helper.ProductUpdateFlag.All, 1);

        //    return flag == "1" || flagAll == "1";
        //}

        //private string GetDescription(ProductCatalogView pc)
        //{
        //    string spec = "";
        //    if (pc.Specification != null)
        //        spec = pc.Specification.Replace("\n", "<br>");

        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImages(pc.ProductCatalogId).Where(x => x.IsActive).ToList();

        //    string youtube = @"<br><br><iframe width=""560"" height=""315"" src=""{0}"" frameborder=""0"" allowfullscreen></iframe>";
        //    //https://www.youtube.com/watch?v=-5JZjzwonng
        //    //https://www.youtube.com/embed/-5JZjzwonng
        //    foreach (Dal.ProductCatalogImage image in images.Where(x => x.ImageTypeId == 2 && x.LinkUrl.Contains("youtube")).ToList())
        //    {
        //        spec += String.Format(youtube, image.LinkUrl.Replace("www.youtube.com/watch?v=", "www.youtube.com/embed/"));
        //    }

        //    return spec;
        //}
 

        //private void DeleteImages2(int productId, List<Dal.ProductCatalogImage> images)
        //{
        //    try
        //    {

        //        int[] imagesToDelete = images.Where(x => x.IsActive == false && x.ShopImageId.HasValue)
        //            .Select(x => x.ShopImageId.Value)
        //            .ToArray();


        //        if (images.Where(x => x.ShopImageId.HasValue == false).Count() == images.Count())
        //        {
        //            int[] imageIds = GetImages(productId);
        //            imagesToDelete = imageIds;
        //            if (imagesToDelete.Length > 0)
        //            {

        //                Object[] att = { productId, imagesToDelete, true };
        //                Object[] methodParams = { SessionId, "product.image.list.delete", att };

        //                var r = SendApiRequest("call", methodParams);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Dal.ErrorHandler.LogError(ex, "DeleteImages");
        //    }
        //}

        //private void DeleteImages(int productId)
        //{
        //    try
        //    {
        //        int[] imageIds = GetImages(productId);

        //        if (imageIds.Length > 0)
        //        {
        //            string session = LoginJason();
        //            Object[] att = { productId, imageIds, true };
        //            Object[] methodParams = { session, "product.image.list.delete", att };

        //            var r = SendApiRequest("call", methodParams);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Dal.ErrorHandler.LogError(ex, "DeleteImages");
        //    }
        //}
       
        #endregion

        #region process Attributes

        //public Bll.ShopCategory GetCategory(int categoryId)
        //{
        //    Object[] att = { categoryId, true};
        //    Object[] methodParams = { SessionId, "category.info", att };

        //    var r = SendApiRequest("call", methodParams);

        //    var json_serializer = new JavaScriptSerializer();
        //    json_serializer.MaxJsonLength = Int32.MaxValue;
        //    ShopCategory c = json_serializer.Deserialize<ShopCategory>(ToJson(r));

        //    if (c.category_id == 0)
        //        return null;
            
        //    return c;
        //}

        //public bool SetCategory(int categoryId)
        //{

        //    Dal.ShopHelper sh = new Dal.ShopHelper();

        //    Dal.ShopCategory sc = sh.GetCategory(categoryId);


        //    Object[] att = { categoryId, true };
        //    Object[] methodParams = { SessionId, "category.save", att };

        //    Dictionary<string, object> d = new Dictionary<string, object>();
        //    Dictionary<string, object> dt = new Dictionary<string, object>();
        //    Dictionary<string, object> t = new Dictionary<string, object>();

        //    t.Add("active", sc.IsActive ? "1" : "0");
        //    t.Add("name", sc.Name);
        //    t.Add("description", sc.Description);
        //    t.Add("seo_title", sc.SeoTitle);
        //    t.Add("seo_description", sc.SeoDescription);
        //    t.Add("seo_keywords", sc.SeoKeywords);
        //    t.Add("permalink", sc.Permalink);

        //    dt.Add("pl_PL", t);
        //    d.Add("translations", dt);

        //    Object[] mp = { SessionId, "category.save", new Object[] { categoryId, d, true } };

        //    var r = SendApiRequest("call", mp);


        //    sh.SetCategoryPublished(categoryId, true);


        //    return true;
        //}
        //public bool SetCategoryDelete(int categoryId)
        //{
        //    Object[] att = { categoryId, true };
        //    Object[] methodParams = { SessionId, "category.delete", att };

        //    var r = SendApiRequest("call", methodParams);

            

        //    return true;
        //}
        //public void GetCategories()
        //{

        //    Object[] att = { true, true, null };
        //    Object[] methodParams = { SessionId, "category.list", att };

        //    var r = SendApiRequest("call", methodParams);

        //    var json_serializer = new JavaScriptSerializer();
        //    json_serializer.MaxJsonLength = Int32.MaxValue;
        //    ShopCategory[] shopCategories = json_serializer.Deserialize<ShopCategory[]>(ToJson(r));

        //    List<Dal.ShopCategory> categories = new List<Dal.ShopCategory>();

        //    foreach (ShopCategory c in shopCategories)
        //    {
        //        Dal.ShopCategory category = new Dal.ShopCategory()
        //        {
        //            ShopCategoryId = c.category_id.ToString(),
        //            CategoryOrder = c.order,
        //            CategoryParentId = null,
        //            Name = c.translations["pl_PL"].name,
        //            Url = c.translations["pl_PL"].permalink,
        //            IsActive = c.translations["pl_PL"].active == 1,
        //            ShopTypeId = (int)Dal.Helper.ShopType.ShoperLajtitPl,
        //            IsAllowed=true
        //        };

        //        if (String.IsNullOrEmpty(category.Permalink))
        //            category.Permalink = c.translations["pl_PL"].permalink;
        //        if (String.IsNullOrEmpty(category.SeoDescription))
        //            category.SeoDescription = c.translations["pl_PL"].seo_description;
        //        if (String.IsNullOrEmpty(category.SeoKeywords))
        //            category.SeoKeywords= c.translations["pl_PL"].seo_keywords;
        //        if (String.IsNullOrEmpty(category.SeoTitle))
        //            category.SeoTitle = c.translations["pl_PL"].seo_title;
        //        if (String.IsNullOrEmpty(category.Description))
        //            category.Description = c.translations["pl_PL"].description;

        //        categories.Add(category);
        //    }


        //    GetCategoryTree(categories);

        //    Dal.ShopHelper sh = new Dal.ShopHelper();
        //    sh.SetCategories(Dal.Helper.ShopType.ShoperLajtitPl, categories);

        //} 


        //private void SetShopProductAttributes( 
        //    int productCatalogId,
        //    int? shopProductId)
        //{
           
        //    try
        //    {
        //        Dictionary<string, object> d = GetShopAttributes(productCatalogId);

        //        if (d.Count > 0)
        //        {
        //            if (shopProductId.HasValue)
        //                SetShopProductAttributesDelete(SessionId, shopProductId.Value);

        //            Object[] att = { shopProductId, d, true };
        //            Object[] methodParams = { SessionId, "product.attributes.save", att };
        //            var r = SendApiRequest("call", methodParams);

        //            if (r.ToString() != "1")
        //                throw new Exception(String.Format("Błąd aktualizacji parametrów SetShopProductAttributes. Zwrócony kod: {0}, ProductCatalogId: {1}",
        //                    r, productCatalogId));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LajtIt.Bll.ErrorHandler.SendError(ex, String.Format("SetAttributesToShopProduct, id produkt: {0}"
        //            , productCatalogId));

        //    }
        //}

        //private Dictionary<string, object> GetShopAttributes(int productCatalogId)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

        //    List<Dal.ProductCatalogAttributeToProduct> attributes
        //    = pch.GetProductCatalogAttributesForShopProduct(new int[] { productCatalogId });

        //    List<Dal.ProductCatalogAttributeToProduct> productCatalogAttributes =
        //       attributes.Where(x => x.ProductCatalogId == productCatalogId)
        //       .ToList();

        //    Dictionary<string, object> d = new Dictionary<string, object>();

        //    foreach (Dal.ProductCatalogAttributeToProduct attribute in productCatalogAttributes)
        //        switch (attribute.ProductCatalogAttribute.ProductCatalogAttributeGroup.AttributeGroupTypeId)
        //        {
        //            case 1:
        //            case 2:
        //                d.Add(attribute.ProductCatalogAttribute.ProductCatalogAttributeGroup.ShopAttributeId.ToString(), attribute.ProductCatalogAttribute.Name); break;
        //            case 3:
        //                d.Add(attribute.ProductCatalogAttribute.ShopAttributeId.ToString(), GetAttributeValue(attribute));
        //                break;
        //        }

        //    return d;

        //}
        //private static void SetShopProductAttributesDelete(string session, int shopProductId)
        //{
        //    try
        //    {
        //        Object[] att = { shopProductId };
        //        Object[] methodParams = { session, "product.attributes", att };
        //        var r = SendApiRequest("call", methodParams);

        //        Dictionary<string, object> atts = r as Dictionary<string, object>;


        //        Dictionary<string, object> d = new Dictionary<string, object>();
        //        if (atts != null)
        //        {
        //            foreach (KeyValuePair<string, object> a in atts)
        //            {
        //                string k = a.Key;
        //                Dictionary<string, object> o = a.Value as Dictionary<string, object>;
        //                if (o != null)
        //                    foreach (KeyValuePair<string, object> b in o)
        //                    {
        //                        d.Add(b.Key, null);
        //                    }

        //            }

        //            Object[] att2 = { shopProductId, d, true };
        //            Object[] methodParams2 = { session, "product.attributes.save", att2 };
        //            var r2 = SendApiRequest("call", methodParams2);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LajtIt.Bll.ErrorHandler.SendError(ex, String.Format("SetShopProductAttributesDelete, id produkt: {0}"
        //            , shopProductId));

        //    }

        //}
        //private static string GetAttributeValue(ProductCatalogAttributeToProduct attribute)
        //{
        //    switch (attribute.ProductCatalogAttribute.AttributeTypeId)
        //    {
        //        case 1:
        //            return String.Format("{0:0.##}", attribute.DecimalValue);
        //        case 2:
        //            return String.Format("{0}", attribute.StringValue);

        //    }
        //    return null;
        //}

       

    

        //private int CreateShopCategory(int parentId, string name)
        //{



        //    Dictionary<string, object> d = new Dictionary<string, object>();
        //    d.Add("order", 10);
        //    d.Add("parent_id", parentId);
        //    Dictionary<string, object> dt = new Dictionary<string, object>();
        //    Dictionary<string, object> t = new Dictionary<string, object>();
        //    t.Add("name", name);
        //    t.Add("description", "");
        //    t.Add("active", "1");
        //    t.Add("seo_title", "");
        //    t.Add("seo_description", "");
        //    t.Add("seo_keywords", "");
        //    dt.Add("pl_PL", t);
        //    d.Add("translations", dt);

        //    Object[] att = { d };
        //    Object[] methodParams = { SessionId, "category.create", att };

        //    var r = SendApiRequest("call", methodParams);

        //    return Convert.ToInt32(r);

        //}
        //private void GetCategoryTree(List<Dal.ShopCategory> categories)
        //{
        //    Object[] methodParams = { SessionId, "category.tree", null };

        //    var r = SendApiRequest("call", methodParams);

        //    var json_serializer = new JavaScriptSerializer();
        //    json_serializer.MaxJsonLength = Int32.MaxValue;
        //    ShopCategoryTree[] shopTree = json_serializer.Deserialize<ShopCategoryTree[]>(ToJson(r));
        //    GetCategoryTreeSub(categories, shopTree);

        //    var s = shopTree.Length;
        //}
        //private static void GetCategoryTreeSub(List<Dal.ShopCategory> categories, ShopCategoryTree[] shopTree)
        //{

        //    foreach (ShopCategoryTree t in shopTree)
        //    {
        //        string[] children = t.children.Select(x => x.id.ToString()).ToArray();
        //        List<Dal.ShopCategory> categoryList = categories.Where(x => children.Contains(x.ShopCategoryId)).ToList();

        //        foreach (Dal.ShopCategory c in categoryList)
        //            c.CategoryParentId = t.id.ToString();

        //        GetCategoryTreeSub(categories, t.children);
        //    }
        //}
        //public void SetProductCategoriesFromAttributes(Dal.Helper.Shop shop)
        //{

        //    ShopUpdateHelper.ClickShop cs = new ShopUpdateHelper.ClickShop();
        //    cs.SetProductCategoriesFromAttributes((int)shop, null);
        //}


        ////public void SetAttributeDefinitions()
        //{

        //   string session = LoginJason();
        //    Object[] w = { 
        //                    "12", "b" 
        //                   };

        //    Dictionary<string, object> d = new Dictionary<string, object>();
        //    d.Add("name", "teerwerw ewerwerwest");
        //    d.Add("description", "");
        //    //d.Add("pres_id", 8);
        //    d.Add("order", 0);
        //    //d.Add("type", 2);
        //    d.Add("active", 1);
        //    d.Add("default", 0);
        //    d.Add("options", w);



        //    //Object[] att1 = 
        //    //{              
        //    //    "Kolor oprawy",  //['name'] (string) - nazwa atrybutu
        //    //    "",              //['description'] (string) - opis atrybutu
        //    //    8,               //['pres_id'] (int) - identyfikator grupy atrybutów
        //    //    0,               //['order'] (int) - priorytet wyświetlania atrybutu
        //    //    1,               //['type'] (int[0/1/2]) - typ atrybutu: 0 - pole tekstowe, 1 - pole checkbox, 2 - pole select
        //    //    0,               //['active'] (int[0/1]) - aktywność atrybutu
        //    //    0,               //['default'] (string) - wartość domyślna atrybutu (w przypadku atrybutu typu checkbox, wartość 0 lub 1)
        //    //    w                //['options'] (array) - tablica dostępnych opcji dla atrybutu typu select, której wartościami są nazwy dostępnych opcji
        //    //};

        //    Object[] att = {17, d, true };
        //    Object[] methodParams1 = { session, "attribute.save", att };
        //    var r1 = SendApiRequest("call", methodParams1);

        //    string s = r1.ToString();
        //    //var json_serializer = new JavaScriptSerializer();
        //    //ShopAttribute shopAttribute = json_serializer.Deserialize<ShopAttribute>(ToJson(r));

        //    //string s = shopAttribute.name;


        //}
        //public void GetAttribute( )
        //{ 
        //    string session = LoginJason();
        //    Object[] att = { 21 };
        //    Object[] methodParams = { session, "attribute.info", att };
        //    var r = SendApiRequest("call", methodParams);


        //    var json_serializer = new JavaScriptSerializer();
        //    ShopAttribute shopAttribute = json_serializer.Deserialize<ShopAttribute>(ToJson(r));

        //    string s = shopAttribute.name;
        //    GetAttributes(141);
        //    SetAttribute();

        //}
        //public void SetAttribute()
        //{
        //    string session = LoginJason();

        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

        //    List<Dal.ProductCatalog> products = pch.GetProductCatalogForShop();

        //    foreach (Dal.ProductCatalog pc in products)
        //    {
        //        Dictionary<string, string> d = new Dictionary<string, string>();
        //        d.Add("21", pc.ProductCatalogLampCategory.Name);
        //        Object[] a = { d };
        //        Object[] att = { pc.ShopProductId.Value, d, true };
        //        Object[] methodParams = { session, "product.attributes.save", att };

        //        var r = SendApiRequest("call", methodParams);

        //    }
        //    string s = "";
        //}
        #endregion

        #region process Orders
        //public void GetOrders(Dal.Helper.Shop shop, string actingUser)
        //{

        //    Object[] att = { false, false, 15500 };
        //    Object[] methodParams = { SessionId, "order.new.list", att };

        //    object[] o = (object[])SendApiRequest("call", methodParams);

        //    string[] shopOrderNumbers = o.Cast<int>().Select(x=>x.ToString()).OrderByDescending(x => x).ToArray();

        //    Dal.ShopHelper sh = new Dal.ShopHelper();

        //    string[] shopOrdersNumbersNotSaved = sh.InsertOrders(shopOrderNumbers, null, shop, actingUser);


        //    ProcessOrders(shop, actingUser);

        //}

        //internal void GetOrderPayments(Dal.Helper.Shop shop, string actingUser)
        //{
        //    Dal.ShopHelper sh = new Dal.ShopHelper();

        //    List<Dal.ShopOrder> orders = sh.GetShopOrdersWithoutPayment(shop);

        //    string[] orderIds = orders.Select(x => x.ShopOrderNumber).ToArray();


        //    Object[] att = { true, false, orderIds };
        //    Object[] methodParams = { SessionId, "order.list", att };

        //    object o = SendApiRequest("call", methodParams);

        //    var json_serializer = new JavaScriptSerializer();
        //    ShopOrder[] shopOrders = json_serializer.Deserialize<ShopOrder[]>(Bll.RESTHelper.ToJson(o));


             

        //    ProcessOrders(shop, shopOrders, actingUser);


        //}

        //private void ProcessOrders(Dal.Helper.Shop shop, ShopOrder[] shopOrders, string actingUser)
        //{
        //    Dal.ShopHelper sh = new Dal.ShopHelper();


        //    foreach(ShopOrder so in shopOrders)
        //    {
        //        if(so.is_paid)
        //        {
        //            Dal.ShopOrder sho = sh.GetShopOrder(shop, so.order_id.ToString());
        //            OrderPayment orderPayment = new OrderPayment()
        //            {
        //                OrderId = sho.OrderId.Value,
        //                Amount = so.paid,
        //                InsertDate = DateTime.Now,
        //                InsertUser = "system",
        //                PaymentTypeId = (int)Dal.Helper.OrderPaymentType.eCard23
        //            };

        //            sh.SetShopOrderPayment(shop, sho.OrderId.Value, orderPayment);
        //        }

        //    }
        //}

        //private void ProcessOrders(Dal.Helper.Shop shop, string actingUser)
        //{
        //    Dal.ShopHelper sh = new Dal.ShopHelper();
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

        //    List<Dal.ShippingType> shippingTypes = sh.GetShippingTypes();

        //    List<Dal.ShopOrder> orders = sh.GetOrdersToProcess(shop);




        //    foreach (Dal.ShopOrder so in orders)
        //    {

        //        #region Order 

        //        Object[] att = { so.ShopOrderNumber, true, true, true, true, true, true, true };
        //        Object[] methodParams = {
        //                                 SessionId,
        //                                 "order.info",
        //                         att
        //                            };

        //        var r = SendApiRequest("call", methodParams);


        //        var json_serializer = new JavaScriptSerializer();
        //        ShopOrder shopOrder = json_serializer.Deserialize<ShopOrder>(Bll.RESTHelper.ToJson(r));


        //        Dal.Order order = new Dal.Order();

        //        //Dal.ShippingType st = shippingTypes
        //        //    .Where(y => y.ShopExternalShippingId.Value == shopOrder.shipping_id)
        //        //        .FirstOrDefault();

        //        //if (st != null)
        //        //    order.ShippintTypeId = st.ShippingTypeId;

        //        #region order
        //        if (!String.IsNullOrEmpty(shopOrder.promo_code))
        //            order.PromoCode = shopOrder.promo_code;

        //        order.PromoRebate = shopOrder.discount_code;
        //        order.Email = shopOrder.email;
        //        order.OrderStatusId = (int)Dal.Helper.OrderStatus.New;
        //        order.ExternalUserId = 0;// shopOrder.order_id;
        //        order.InsertDate = shopOrder.date;
        //        order.ShippingCost = shopOrder.shipping_cost;
        //        order.ShippingCostVAT = shopOrder.shipping_vat_value / 100;

        //        order.City = order.ShipmentCity = shopOrder.deliveryAddress.city;
        //        order.ShipmentCompanyName = shopOrder.deliveryAddress.company;
        //        order.FirstName = order.ShipmentFirstName = shopOrder.deliveryAddress.firstname;
        //        order.LastName = order.ShipmentLastName = shopOrder.deliveryAddress.lastname;
        //        order.Postcode = order.ShipmentPostcode = shopOrder.deliveryAddress.postcode;
        //        order.Address = order.ShipmentAddress = shopOrder.deliveryAddress.street1;
        //        if (shopOrder.deliveryAddress.street2 != null && shopOrder.deliveryAddress.street2 != "")
        //            order.ShipmentAddress = order.ShipmentAddress + " " + shopOrder.deliveryAddress.street2;
        //        order.Phone = shopOrder.deliveryAddress.phone;
        //        order.ParActive = true;
        //        order.ShopId=(int)shop; //sklep,
        //        order.CompanyId = Dal.Helper.DefaultCompanyId;
        //        order.ExternalOrderNumber = shopOrder.order_id.ToString();
        //        SetShipping(shopOrder, shippingTypes, ref order);
        //        #endregion
        //        #region Invoice
        //        Dal.Invoice invoice = null;
        //        if (shopOrder.billingAddress.tax_id != null && shopOrder.billingAddress.tax_id != "")
        //        {
        //            invoice = new Dal.Invoice();
        //            invoice.Address = shopOrder.billingAddress.street1;
        //            if (shopOrder.billingAddress.street2 != null && shopOrder.billingAddress.street2 != "")
        //                invoice.Address = invoice.Address + " " + shopOrder.billingAddress.street2;
        //            invoice.City = shopOrder.billingAddress.city;
        //            invoice.CompanyName = shopOrder.billingAddress.company;
        //            invoice.CompanyId = Dal.Helper.DefaultCompanyId;
        //            invoice.Email = order.Email;
        //            invoice.InvoiceDate = order.InsertDate;
        //            invoice.InvoiceTypeId = 2;
        //            invoice.Nip = shopOrder.billingAddress.tax_id;
        //            invoice.Postcode = shopOrder.billingAddress.postcode;

        //            order.Invoice = invoice;

        //        }
        //        #endregion
        //        #region products
        //        List<Dal.OrderProduct> products = new List<Dal.OrderProduct>();
        //        List<Dal.ProductCatalogShopProduct> productCatalogs = pch.GetProductCatalogShopProductByShopIds(shop, 
        //            shopOrder.products.Select(x => x.product_id.ToString()).ToArray()
        //            ) 
        //            .ToList();
        //        foreach (ShopProduct product in shopOrder.products)
        //        {
        //            Dal.OrderProduct p = new Dal.OrderProduct()
        //            {
        //                Comment = String.Format("{0}", product.option),
        //                ExternalProductId = product.product_id,
        //                Order = order,
        //                Price = product.price,
        //                ProductName = product.name,
        //                Quantity = product.quantity,
        //                VAT = Convert.ToDecimal(product.tax_value / 100), 
        //                ProductTypeId = (int)Dal.Helper.ProductType.RegularProduct,
        //                Rebate = product.discount_perc,
        //                OrderProductStatusId = (int)Dal.Helper.OrderProductStatus.New

        //            };
        //            Dal.ProductCatalogShopProduct pc = productCatalogs.Where(x => Int32.Parse(x.ShopProductId) == product.product_id).FirstOrDefault();
        //            if (pc != null)
        //            { 
        //                p.ProductCatalogId = pc.ProductCatalogId; 
        //            }
        //            products.Add(p);


        //        }

        //        #endregion

        //        #region payment
        //        Dal.OrderPayment orderPayment = null;
        //        //if (shopOrder.payment_id == 7 && shopOrder.paid>0)
        //        //{
        //        //     orderPayment = new OrderPayment()
        //        //    {
        //        //        Order = order,
        //        //        Amount = shopOrder.paid,
        //        //        InsertDate = DateTime.Now,
        //        //        InsertUser = "system",
        //        //        PaymentTypeId = (int)Dal.Helper.OrderPaymentType.eCard23

        //        //    };
        //        //}
                
        //        #endregion

        //        Dal.OrderStatusHistory orderStatusHistory = new Dal.OrderStatusHistory()
        //        {
        //            //Comment = String.Format("Zamówienie: {0}, \n\nDostawa: {1}, \n\nPłatność: {2},\n\nUwagi do zamówienia: \n\n{3}",
        //            Comment = String.Format("Zamówienie: {0}, \nPłatność: {2}\nUwagi do zamówienia: \n{1}",
        //       so.ShopOrderNumber,
        //       shopOrder.notes,
        //       GetPaymentName(shopOrder.payment_id)),
        //            InsertDate = DateTime.Now,
        //            InsertUser = actingUser,
        //            Order = order,
        //            OrderStatusId = (int)Dal.Helper.OrderStatus.New

        //        };



        //        #endregion

        //        sh.SetNewOrder(so, invoice, products, shop, orderStatusHistory, orderPayment);

        //    }


        //}

        //private void SetShipping(ShopOrder shopOrder, List<Dal.ShippingType> shippingTypes, ref Dal.Order order)
        //{
        //    Dal.ShippingType st = shippingTypes.Where(x => x.ShopExternalShippingId == shopOrder.shipping_id
        //    && x.ShopExternalPaymentIdIds.Contains(String.Format("_{0}_", shopOrder.payment_id))).FirstOrDefault();


        //    if (st != null)
        //    {
        //        order.ShippintTypeId = st.ShippingTypeId;

        //        if (st.ShippingCompanyId == 4) //Paczkomat
        //            order.ShippingData = String.Format("{0}|", shopOrder.shipping_additional_fields.machine);
        //    }


        //}

        //private string GetShippingName(int p)
        //{
        //    Object[] att = { p, false, false };
        //    Object[] methodParams = { SessionId, "shipping.info", att };

        //    var r = SendApiRequest("call", methodParams);


        //    var json_serializer = new JavaScriptSerializer();
        //    ShopShippingType shopShipping = json_serializer.Deserialize<ShopShippingType>(Bll.RESTHelper.ToJson(r));


        //    return shopShipping.name;
        //}

        //private object GetPaymentName(int p)
        //{
        //    Object[] att = { p, false, false };
        //    Object[] methodParams = { SessionId, "payment.info", att };

        //    var r = SendApiRequest("call", methodParams);

        //    var json_serializer = new JavaScriptSerializer();
        //    ShopPaymentType shopPayment = json_serializer.Deserialize<ShopPaymentType>(Bll.RESTHelper.ToJson(r));


        //    return shopPayment.name;
        //}

        private bool SetOrderStatus(int orderId, string orderStatusId)
        {


            Dictionary<string, object> d = new Dictionary<string, object>();
            Dictionary<string, object> dt = new Dictionary<string, object>();
            Dictionary<string, object> t = new Dictionary<string, object>();
            t.Add("status_id", orderStatusId);
            dt.Add("pl_PL", t);
            d.Add("translations", dt);


            Object[] att = { orderId, t, true };
            Object[] methodParams = { SessionId, "order.save", att };

            var r = SendApiRequest("call", methodParams);



            return true;

        }
        #endregion

        #region Deliveries

        public ShopDelivery[] GetDeliveries()
        {


            Object[] att = { true, true, null };
            Object[] methodParams = { SessionId, "delivery.list", att };

            var r = SendApiRequest("call", methodParams);


            var json_serializer = new JavaScriptSerializer();
            ShopDelivery[] delivery = json_serializer.Deserialize<ShopDelivery[]>(Bll.RESTHelper.ToJson(r));

            return delivery;

        }

        #endregion
        #region Options

        internal void SetShopoptions()
        {



            //object r = CreateGroup(session, CreateGroup());

            int groupId = 46;// Convert.ToInt32(r);
            GetOptionGroup(30);

            object c = CreateOptions(groupId, 8271);

        }

        private object CreateOptions(int groupId, int pp)
        {

            Dictionary<string, object> o = new Dictionary<string, object>();
            Dictionary<string, object> v = new Dictionary<string, object>();
            Dictionary<string, object> dt = new Dictionary<string, object>();
            Dictionary<string, string> t = new Dictionary<string, string>();


            v.Add("111", 1);


            t.Add("name", "wertterterte");
            t.Add("type", "select");
            t.Add("required", "1");
            t.Add("filters", "0");
            dt.Add("pl_PL", t);
            o.Add("group_id", 47);
            o.Add("translations", new Object[] { dt });



            Object[] att = { o };
            Object[] methodParams = { SessionId, "option.create", att };
            var r = SendApiRequest("call", methodParams);
            string s = r.ToString();
            return r;
        }

        private object GetOptionGroup(int groupId)
        {



            Object[] att = { groupId, true, true, true };
            Object[] methodParams = { SessionId, "option.group.info", att };
            var r = SendApiRequest("call", methodParams);
            string s = r.ToString();
            return r;
        }
        public void CreateGroup(string name)
        {
            Dictionary<string, object> t = new Dictionary<string, object>();
            Dictionary<string, object> dt = new Dictionary<string, object>();
            Dictionary<string, object> group = new Dictionary<string, object>();
            t.Add("name", name);
            dt.Add("pl_PL", t);
            group.Add("translations", dt);
            CreateGroup(SessionId, group);
        }
        private static object CreateGroup(string session, Dictionary<string, object> group)
        {
            Object[] att = { group };
            Object[] methodParams = { session, "option.group.create", att };
            var r = SendApiRequest("call", methodParams);
            string s = r.ToString();
            return r;
        }
        public object CreateOption(int groupId, string name)
        {
            Dictionary<string, object> t = new Dictionary<string, object>();
            Dictionary<string, object> dt = new Dictionary<string, object>();
            Dictionary<string, object> group = new Dictionary<string, object>();
            t.Add("name", name);
            dt.Add("pl_PL", t);
            group.Add("group_id", groupId);
            group.Add("translations", dt);


            Object[] att = { group };
            Object[] methodParams = { SessionId, "option.create", att };
            var r = SendApiRequest("call", methodParams);
            string s = r.ToString();
            return r;
        }
        private static Dictionary<string, object> CreateGroup()
        {
            Dictionary<string, object> group = new Dictionary<string, object>();
            Dictionary<string, object> dt = new Dictionary<string, object>();
            Dictionary<string, object> t = new Dictionary<string, object>();
            t.Add("name", "gruppa");
            dt.Add("pl_PL", t);
            group.Add("translations", dt);

            return group;
        }

        public ShopDelivery[] GetDeliveries1()
        {

            Object[] att = { true, true, null };
            Object[] methodParams = { SessionId, "option.create", att };

            var r = SendApiRequest("call", methodParams);


            var json_serializer = new JavaScriptSerializer();
            ShopDelivery[] delivery = json_serializer.Deserialize<ShopDelivery[]>(Bll.RESTHelper.ToJson(r));

            return delivery;

        }
 
        public ShopProductRootObject GetShopProduct(int id)
        {


            Object[] att = { id, true, true, false, false, false, false };
            Object[] methodParams = { SessionId, "product.info", att };
            var r = SendApiRequest("call", methodParams);


            var json_serializer = new JavaScriptSerializer();
            ShopProductRootObject p = json_serializer.Deserialize<ShopProductRootObject>(Bll.RESTHelper.ToJson(r));



            return p;
        }
        public class Prod
        {
            public int product_id { get; set; }
            public string code { get; set; }
            public string ean{ get; set; }
        }
 
        #endregion

        public static decimal GetShopRebate(decimal amount)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<Dal.ShopRebate> rebates = sh.GetRebates();

            decimal? rebate = rebates.Where(x => x.AmountFrom < amount && x.AmountTo >= amount).Select(x => x.Rebate).FirstOrDefault();

            if (rebate.HasValue)
                return rebate.Value * amount;
            else
                return 0;

        }
        //internal void UpdateByCatalogBatch(Dal.Helper.ShopType shopType, Dal.Helper.UpdateScheduleType scheduleType, string actingUser)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

        //    Bll.ProductCatalogHelper pchB = new Bll.ProductCatalogHelper();
        //    pchB.SetProductCatalogScheduleDeleteDuplicates();

        //    List<ProductCatalogUpdateScheduleView> schedules =
        //        pch.GetProductCatalogUpdateSchedule(shopType, scheduleType, 200);

        //    Guid processId = Guid.NewGuid();



        //    int[] ids = schedules.Select(x => x.Id).ToArray();

        //    pch.SetProductCatalogUpdateScheduleProcessId(ids, processId);


        //    try
        //    {

        //        List<UpdateResult> updateResult = SetProductUpdateByProductCatalogId(schedules, scheduleType);

        //        int[] schedulesErrors = updateResult.Where(x => x.Result != 1)
        //            .Select(x => x.ShopProductId).ToArray();


        //        List<ProductCatalogUpdateScheduleView> schedulesWithErrors =
        //            schedules
        //            .Where(x => x.ShopProductId.HasValue && schedulesErrors.Contains(x.ShopProductId.Value))
        //            .Select(x=>
        //            new ProductCatalogUpdateScheduleView()
        //            {
        //                Id=x.Id,
        //                ShopProductId= x.ShopProductId,
        //                UpdateComment = String.Format("Błąd aktualizacji. ShopProductId: {0}, kod błędu: {1}", x.ShopProductId,
        //                updateResult.Where(y=>y.ShopProductId==x.ShopProductId).Select(y=>y.Result).FirstOrDefault()
        //                )

        //            })
        //            .ToList();



        //        int[] schedulesCompleted = updateResult.Where(x => x.Result == 1).Select(x => x.ShopProductId).ToArray();
        //        schedules = schedules.Where(x => schedulesCompleted.Contains(x.ShopProductId.Value)).ToList();

        //        // aktualizacje które osiągnęly max prób
        //        List<ProductCatalogUpdateSchedule> schedulesWithErrorsAndStop = 
        //            pch.ProductCatalogUpdateSchedule(schedules, schedulesWithErrors,processId);

        //        NotifyAboutUpdateErrors(schedulesWithErrorsAndStop);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.SendError(ex, String.Format("UpdateByCatalogBatch Id: {0}, UpdateScheduleType: {1}", 
        //            shopType.ToString(), scheduleType.ToString()));
        //        //pch.ProductCatalogUpdateSchedule(schedule, Dal.Helper.ProductCatalogUpdateStatus.Error);
        //    }
        //}

        //private void NotifyAboutUpdateErrors(List<ProductCatalogUpdateSchedule> schedulesWithErrorsAndStop)
        //{
        //    if(schedulesWithErrorsAndStop.Count>0)
        //    {
        //        ErrorHandler.SendError(new Exception("Błąd aktualizacji danych w sklepie"),
        //            String.Format("Przekroczono liczbę prób aktualizacji <br>{0}",
        //            String.Join("",
        //            schedulesWithErrorsAndStop.Select(x => String.Format("<a href='http://192.168.0.107/Product.aspx?id={0}'>{0}</a> - {1}<br>",
        //            x.ProductCatalogId, x.UpdateComment)).ToArray())));

        //    }
           
        //}

        private static readonly object SyncObject = new object();
        //internal void UpdateByCatalog(Dal.Helper.ShopType shopType, Dal.Helper.UpdateScheduleType scheduleType, string actingUser)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();


        //    Guid processId = Guid.NewGuid();

        //    List<ProductCatalogUpdateScheduleView> schedules = null;

        //    lock (SyncObject)
        //    {
        //        schedules =
        //        pch.GetProductCatalogUpdateSchedule(shopType, scheduleType, 100);
        //        //.Take(200).ToList();
        //        //.Take(Bll.Helper.GetSetting("SHOP_UPD").IntValue.Value).ToList();

        //        int[] ids = schedules.Select(x => x.Id).ToArray();

        //        Console.WriteLine(String.Format("ProcessId: {0} START", processId));
        //        //Console.WriteLine(String.Format("{0}", String.Join(",", ids)));

        //        pch.SetProductCatalogUpdateScheduleProcessId(ids, processId);
        //    }
        //    //System.Threading.Thread.Sleep(100000000);

        //    List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources();

        //    List<ProductCatalogUpdateScheduleView> schedulesCompleted = new List<ProductCatalogUpdateScheduleView>();
        //    List<ProductCatalogUpdateScheduleView> schedulesWithErrors = new List<ProductCatalogUpdateScheduleView>();

        //    List<ProductCatalogAllegroItemsFnResult> itemsCreating = null;

        //    if(shopType==  Dal.Helper.ShopType.Allegro)
        //        itemsCreating = pch.GetProductCatalogAllegroItems("NOTCREATED", "INACTIVE", "ACTIVATING");

        //    foreach (ProductCatalogUpdateScheduleView schedule in schedules)
        //    {
        //        try
        //        {

        //            Int64 i = Convert.ToInt64(schedule.UpdateCommand);
        //            if (i != 0)
        //            {
        //                UpdateResult result = null;
        //                switch (shopType)
        //                {
        //                    case Dal.Helper.ShopType.Allegro:
        //                        result = ProductFileImportHelper. UpdateAllegro(schedule, itemsCreating); break;
        //                    case Dal.Helper.ShopType.ClickShop:
        //                        result = UpdateShop(schedule, scheduleType, sources.Where(x => x.ShopTypeId == (int)shopType).ToList()); break;

        //                }
        //                if (result != null)
        //                    if (result.Result == 1) { 
        //                        schedulesCompleted.Add(schedule);
        //                    }
        //                    else
        //                    {
        //                        schedule.UpdateCommand = String.Format("Błąd aktualizacji. Kod: {0}", result.Result);
        //                        schedulesWithErrors.Add(schedule);
        //                    }
        //                else
        //                {
        //                    schedulesCompleted.Add(schedule);

        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorHandler.SendError(ex, String.Format("UpdateByCatalog Id: {0}, ProductCatalogId: {1}", schedule.Id, schedule.ProductCatalogId));
        //            //pch.ProductCatalogUpdateSchedule(schedules, processId, Dal.Helper.ProductCatalogUpdateStatus.Error);
        //        }
        //    }
             
        //    // aktualizacje które osiągnęly max prób
        //    List<ProductCatalogUpdateSchedule> schedulesWithErrorsAndStop =
        //        pch.ProductCatalogUpdateSchedule(schedulesCompleted, schedulesWithErrors, processId);

        //    NotifyAboutUpdateErrors(schedulesWithErrorsAndStop);

        //    Console.WriteLine(String.Format("ProcessId: {0} END", processId));
        //}
        //public UpdateResult UpdateShop(
        //    ProductCatalogUpdateScheduleView schedule,
        //    Dal.Helper.UpdateScheduleType scheduleType,
        //    List<Dal.SupplierDeliveryTypeSource> sources)
        //{
        //     return SetProductUpdateByProductCatalogId(schedule.ProductCatalogId, schedule.UpdateCommand, scheduleType, sources);
        //}

        public bool SetProductDelete(string[] productIdsToDelete)
        {
             

            foreach(string id in productIdsToDelete)
            { 
            Object[] att = { id, true };
            Object[] methodParams = { SessionId, "product.delete", att };

            var r = SendApiRequest("call", methodParams);
            }
            return true;
        }

        public static void SetShopCategories()
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<Dal.ShopCategoryManager> scm = sh.GetShopCategoryManagers();

            foreach(Dal.ShopCategoryManager m in scm)
            {
                List<int> productCatalogIds = sh.GetShopCategoryManagerInitialProducts(m.ShopCategoryManagerId);
            }
        }

        public static void SetOrderAsSent(Dal.Helper.Shop shop)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();

            List<Dal.ShopOrder> orders = sh.GetOrdersSent(shop);

            switch (shop)
            {
                case Dal.Helper.Shop.Morele:

                    Bll.MoreleRESTHelper.Orders.SetWayBill(orders); break;
            }
        }
    }
}
