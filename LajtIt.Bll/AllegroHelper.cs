using System;
using System.Collections.Generic;
using System.Linq;

using System.Security.Cryptography;
using System.IO;
using System.Globalization;
using System.Configuration;
using System.Text;
using LajtIt.Bll.AllegroNewWCF;
using LajtIt.Dal;
using System.Net;

namespace LajtIt.Bll
{
    public partial class AllegroHelper
    {
        #region properties
        private AllegroNewWCF.servicePortClient allegroService = new AllegroNewWCF.servicePortClient();

        //private string password = "";
        //private static string webApiKey = "e3dd972ff5";
        //private static long versionKey = 53989091;
        private static int countryCode = 1;
        private static LajtIt.Bll.Helper.Cache<string, string> cache = new LajtIt.Bll.Helper.Cache<string, string>();

        private static long userId = 0;
        private static long serverTime = 0;
        private static int pageSize = 500;
        public static int PageSize { get { return pageSize; } }
        private static DateTime sessionStart = DateTime.Now;
        #endregion

        public static string GetCachedValue(string fieldName, string userName)
        {
            string key = String.Format("{0}{1}", fieldName, userName);

            string value = cache.Get(key);
            if (value == null)
                return null;
            else
                return value;

        }
        public static void SetCachedValue(string fieldName, string userName, string value)
        {
            string key = String.Format("{0}{1}", fieldName, userName);

            cache.Set(key, value);

        }
        public List<AllegroNewWCF.UserIncomingPaymentStruct> GetPayments(string userName, int buyerid, long itemid, int offset)
        {
            try
            {
                AllegroNewWCF.UserIncomingPaymentStruct[] payments =
                    allegroService.doGetMyIncomingPayments(GetSessionHandle(userName), buyerid, itemid, 0, 0, 25, offset, 0);

                return payments.ToList();

            }
            catch (Exception ex)
            {
                Exception e = ex;
                return new List<AllegroNewWCF.UserIncomingPaymentStruct>();

            }


        }

        //internal void SetAllegroParcelNumber()
        //{
        //    long transactionId=0;
        //    Bll.AllegroHelper.GetVersionKeys();//(Dal.Helper.MyUsers.JacekStawicki.ToString());
        //    try
        //    {
        //        Dal.OrderHelper oh = new Dal.OrderHelper();
        //        List<Dal.AllegroItamTransactionTrackingNumber> transactions
        //            = oh.GetAllegroParcelNumber();
        //        List<long> transactionsId = new List<long>();
        //        foreach (Dal.AllegroItamTransactionTrackingNumber transaction in transactions)
        //        {

        //            try
        //            {
        //                string userName = transaction.UserName;
        //                transactionId = transaction.BuyFormId.Value;
        //                string packageId = transaction.ShipmentTrackingNumber;
        //                int operatorId = transaction.AllegroOperatorId.Value;

        //                PackageInfoStruct[] pis = new PackageInfoStruct[]
        //                {
        //                new PackageInfoStruct() { operatorId = operatorId, packageId =  packageId}
        //                };
        //                AllegroNewWCF.PostBuyFormPackageInfoStruct pck =
        //                    allegroService.doAddPackageInfoToPostBuyForm(GetSessionHandle(userName), transactionId, pis);
        //                if (pck.packageIdsAdded.Contains(transaction.ShipmentTrackingNumber))
        //                    transactionsId.Add(transaction.TransactionId);
        //            }
        //            catch (Exception ex)
        //            {
        //                Bll.ErrorHandler.SendError(ex, String.Format("Przesyłanie danych paczki, transactionId {0}", transactionId));
        //                //return new List<AllegroNewWCF.UserIncomingPaymentStruct>();

        //            }
        //        }

        //        oh.SetAllegroParcelNumber(transactionsId);

        //    }
        //    catch (Exception ex)
        //    {
        //        Bll.ErrorHandler.SendError(ex, String.Format("Przesyłanie danych paczki, transactionId {0}", transactionId));
        //        //return new List<AllegroNewWCF.UserIncomingPaymentStruct>();

        //    }
        //}


        //public AllegroNewWCF.ItemInfoStruct[] GetAuctions(string userName, long[] itemsId)
        //{
        //    if (itemsId.Length == 0)
        //        return null;


        //    long[] notfound;
        //    long[] killed;

        //    try
        //    {
        //        return allegroService.doGetItemsInfo(GetSessionHandle(userName), itemsId,
        //             0, 1, 1, 0, 0, 0, out notfound, out killed);
        //    }
        //    catch (Exception ex)
        //    {

        //        ErrorHandler.LogError(ex, "public AllegroNewWCF.ItemInfoStruct[] GetAuctions(long[] itemsId)");


        //        return null;
        //    }

        //}

        //private string GetCategory(long productid)
        //{
        //    AllegroNewWCF.ItemCatList[] cat;
        //    AllegroNewWCF.ItemImageList[] image;
        //    AllegroNewWCF.AttribStruct[] att;
        //    AllegroNewWCF.PostageStruct[] post;
        //    AllegroNewWCF.ItemPaymentOptions pay;
        //    AllegroNewWCF.CompanyInfoStruct cmp;
        //    AllegroNewWCF.ProductStruct product;

        //    string category = " Allegro";
        //    AllegroNewWCF.ItemInfoExt cate =
        //    allegroService.doShowItemInfoExt(out cat,
        //    out image,
        //    out att,
        //    out post,
        //    out pay,
        //    out  cmp,
        //    out   product,
        //    GetSessionHandle(), productid, 0, 0, 0, 0, 0, 0);

        //    foreach (AllegroNewWCF.ItemCatList c in cat.ToList())
        //    {
        //        category += "> " + c.catname;
        //    } return category;
        //}

        public AllegroNewWCF.ItemInfoStruct[] GetItems(long[] itemIds, string userName, out long[] notFound, out long[] killed)
        {
            AllegroNewWCF.ItemInfoStruct[] itemsInfo =
            allegroService.doGetItemsInfo(
            GetSessionHandle(userName),
            itemIds,
            0,
            1,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            out notFound,
            out killed);



            return itemsInfo;
        }
        public AllegroNewWCF.ItemInfoExt GetItem(long itemId, string userName, out int? categoryId, out string imageUrl)
        {
            AllegroNewWCF.ItemCatList[] cat;
            AllegroNewWCF.ItemImageList[] image;
            AllegroNewWCF.AttribStruct[] att;
            AllegroNewWCF.PostageStruct[] post;
            AllegroNewWCF.ItemPaymentOptions pay;
            AllegroNewWCF.CompanyInfoStruct cmp;
            AllegroNewWCF.ProductStruct product;
            //AllegroNewWCF.ItemVariantStruct[] variant;
            string itemVariants;
            AllegroNewWCF.AfterSalesServiceConditionsStruct after;
            string s;
            AllegroNewWCF.ItemInfoExt itemInfo =
            allegroService.doShowItemInfoExt(
            GetSessionHandle(userName), 
            itemId, 
            0, 1, 0, 0, 0, 0,0,0,0, out cat,
            out image,
            out att,
            out post,
            out pay,
            out  cmp,
            out   product,
            out itemVariants,
            out after,out s);

            categoryId = null;
            if (cat.Count() != 0)
                categoryId = (int)cat.Skip(cat.Count() - 1).FirstOrDefault().catId;

            imageUrl = null;
            AllegroNewWCF.ItemImageList i = image.Where(x => x.imageType == 1).FirstOrDefault();
            if (i != null)
                imageUrl = i.imageUrl;


            return itemInfo;
        }

        public static void GetVersionKeys()
        {
            foreach (string name in Enum.GetNames(typeof(Dal.Helper.MyUsers)))
            {
                GetVersionKey(name);
            }

        }
        private static void GetVersionKey(string uName)
        {
            long verKey = 0;
            string verStr = null;

            try
            {
                AllegroNewWCF.servicePortClient ase = new AllegroNewWCF.servicePortClient();
                ase.doQuerySysStatus(1, countryCode, GetWebApiKey(uName), out verKey);

                SetCachedValue("VersionKey", uName, verKey.ToString());
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError(ex, String.Format("verKey: {0}, verStr: {1}", verKey, verStr));
            }
        }
        //public static void GetVersionKey()
        //{
        //      GetVersionKey(defaultUserName);
        //}
        public string GetSessionHandle(string uName)
        {

            TimeSpan ts = DateTime.Now.Subtract(sessionStart);
            if (ts.Hours >= 2)
            {
                SetCachedValue("SessionHandle", uName, null);
            }

            if (GetCachedValue("SessionHandle", uName) != null)
                return GetCachedValue("SessionHandle", uName);

            using (SHA256 sha256 = new SHA256Managed())
            {
                //byte[] passwordHash = sha256.ComputeHash(StrToByteArray(password));
                string encodedPassword = "fApU5PGQhmlNIZe7Ykcg7/vita7iW3FPapcHolYNO54=";// Convert.ToBase64String(passwordHash);

                try
                {
                    string sessionHandle = allegroService.doLoginEnc(uName, encodedPassword,
                        countryCode, GetWebApiKey(uName), Convert.ToInt64(GetCachedValue("VersionKey", uName)),
                        out userId, out serverTime
                        );
                    sessionStart = DateTime.Now;
                    SetCachedValue("SessionHandle", uName, sessionHandle);

                    return sessionHandle;
                }
                catch (Exception e)
                {
                    throw e;//obsługa wyjątku w przypadku niepowodzenia
                }
            }

        }

        private static string GetWebApiKey(string uName)
        {
            Dal.AllegroScan asc = new Dal.AllegroScan();
            return asc.GetWebApiKey(uName); 
        }
        //private string GetSessionHandle()
        //{
        //    return GetSessionHandle(defaultUserName);
        //}
        private static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }


        internal AllegroNewWCF.CatInfoType[] GetCategories()
        {
            long verkey = 0;
            string verstr;

            AllegroNewWCF.CatInfoType[] categories =
                allegroService.doGetCatsData(countryCode, 0, GetWebApiKey(Dal.Helper.MyUsers.JacekStawicki.ToString()), false, out verkey, out verstr);

            return categories;
        }

        internal AllegroNewWCF.CatInfoType[] GetShopCategories(string userName)
        {
            throw new Exception("TO DO GetShopCategories");
            //return allegroService.doGetShopCatsData(GetSessionHandle(userName));
        }
        internal AllegroNewWCF.ItemBilling[] GetAllegroCost(string userName, long itemId, out AllegroNewWCF.ItemBilling[] billing)
        {
            return allegroService.doMyBillingItem(GetSessionHandle(userName), itemId, "F", out billing);
        }

        public AllegroNewWCF.BidListStruct2[] GetItemOrders(string userName, long itemId)
        {
            try
            {


                return allegroService.doGetBidItem2(GetSessionHandle(userName), itemId);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError(ex, String.Format("itemId: {0}", itemId));
                AllegroScan allegroScan = new AllegroScan();
                if (ex.Message.Contains("przeniesiona do archiwum"))
                {
                    ErrorHandler.LogError(ex, String.Format("Przeniesiono do archwiwum: {0}", itemId));
                    allegroScan.SetMovedToArchive(itemId);
                }
                if (ex.Message.Contains("usunięta przez administratora"))
                {
                    ErrorHandler.LogError(ex, String.Format("Aukcja usunięta przez administratora: {0}", itemId));
                    allegroScan.SetEndingInfo(itemId, 4);
                }
            }
            return null;
        }


        //internal AllegroNewWCF.MyFeedbackListStruct2[] GetComments(string userName, long itemId)
        //{
        //    try
        //    {
        //        return allegroService.doMyFeedback2Limit(GetSessionHandle(userName), "fb_recvd", 0, 0, new long[] { itemId }, 1000);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.LogError(ex, "allegroService.doMyFeedback2");
        //        //AllegroScan allegroScan = new AllegroScan();
        //        //if (ex.Message.Contains("przeniesiona do archiwum"))
        //        //{
        //        //    ErrorHandler.LogError(ex, String.Format("Przeniesiono do archwiwum: {0}", itemId));
        //        //    allegroScan.SetMovedToArchive(itemId);
        //        //}
        //        //if (ex.Message.Contains("usunięta przez administratora"))
        //        //{
        //        //    ErrorHandler.LogError(ex, String.Format("Aukcja usunięta przez administratora: {0}", itemId));
        //        //    allegroScan.SetEndingInfo(itemId, 4);
        //        //}
        //    }
        //    return null;
        //}

        // stara metoda

        //public List<AllegroNewWCF.UserItemList> GetUserAuctions(int userId, string userName)
        //{
        //    int count;
        //    List<AllegroNewWCF.UserItemList> uil = new List<AllegroNewWCF.UserItemList>();

        //    string webApiKey = GetWebApiKey(userName);

        //    uil.AddRange(allegroService.doGetUserItems(userId, webApiKey, 1, 0, 100, out count).ToList());


        //    int partsCount = (count / 100);

        //    for (int i = 1; i < partsCount + 1; i++)
        //    {
        //        uil.AddRange(allegroService.doGetUserItems(userId, webApiKey, 1, i, 100, out count).ToList());
        //    }

        //    return uil;
        //}

        //public List<AllegroNewWCF.ItemsListType> GetUserAuctions(long userId, string userName)
        //{
        //    List<AllegroNewWCF.FilterOptionsType> filters = new List<AllegroNewWCF.FilterOptionsType>();
        //    filters.Add(
        //        new AllegroNewWCF.FilterOptionsType()
        //        {
        //            filterId = "userId",
        //            filterValueId = new string[] { userId.ToString() }
        //        }
        //        );

        //    int count;
        //    List<AllegroNewWCF.ItemsListType> uil = new List<AllegroNewWCF.ItemsListType>();

        //    uil.AddRange(GetItemsList(userName, filters.ToArray(), 0, out count));


        //    int partsCount = (count / pageSize);

        //    for (int i = 1; i < partsCount + 1; i++)
        //    {
        //        uil.AddRange(GetItemsList(userName, filters.ToArray(), i, out count));
        //    }

        //    return uil;
        //}
         

        //internal AllegroNewWCF.ItemsListType[] GetItemsList(string userName, AllegroNewWCF.FilterOptionsType[] filters, int offset, out int count)
        //{
        //    int itemsFeaturedCount;
        //    AllegroNewWCF.ItemsListType[] itemsList;
        //    AllegroNewWCF.CategoriesListType categoriesList;
        //    AllegroNewWCF.FiltersListType[] filtersList;
        //    string[] filtersRejected;

        //    count = allegroService.doGetItemsList(GetWebApiKey(userName),
        //        countryCode,
        //        filters,
        //        null,
        //        pageSize,
        //        offset * pageSize,
        //        0,
        //        out itemsFeaturedCount,
        //        out itemsList,
        //        out categoriesList,
        //        out filtersList,
        //        out filtersRejected
        //        );
        //    return itemsList;
        //}
         
        internal int SetShopItem(string userName, long itemIdToCopyFrom, int nextScheduleId, out long newItemId)
        {
            AllegroNewWCF.StructSellFailed[] failed = null;
            long[] itemsIdsNotFound = null;
            allegroService.doSellSomeAgainInShop(
                GetSessionHandle(userName),
                new long[] { itemIdToCopyFrom },
                0,
                30,
                2,
                0,
                0,
                new int[] { nextScheduleId },
                out failed,
                out itemsIdsNotFound);

            int status;
            long startTime;
            newItemId = allegroService.doVerifyItem(GetSessionHandle(userName), nextScheduleId, out status, out startTime);

            return status;
        }

        internal void SetShopItem(string userName, long itemIdToCopyFrom)
        {
            AllegroNewWCF.StructSellFailed[] failed = null;
            long[] itemsIdsNotFound = null;
            allegroService.doSellSomeAgainInShop(
                GetSessionHandle(userName),
                new long[] { itemIdToCopyFrom },
                0,
                30,
                2,
                0,
                0,
                null,
                out failed,
                out itemsIdsNotFound);

        }


        public List<Dal.AllegroStatsResult> GetAllegroStats(int year, int month, decimal value)
        {
            Dal.AllegroScan allegroScan = new Dal.AllegroScan();


            return allegroScan.GetAllegroStats(year, month, value);


        }

        //public string GetFormFields(string categoryId)
        //{
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    StringBuilder sb = new StringBuilder();
        //    GetVersionKeys();

        //    try
        //    { 
        //        AllegroNewWCF.SellFormFieldsForCategoryStruct s =
        //            allegroService.doGetSellFormFieldsForCategory(GetWebApiKey(Dal.Helper.MyUsers.JacekStawicki.ToString()),
        //            countryCode, Convert.ToInt32(categoryId));
               


        //        // AllegroNewWCF.SellFormType[] forms = allegroService.doGetSellFormFieldsExt(out verkey, out verstr, countryCode,                     Convert.ToInt64(GetCachedValue("VersionKey", "JacekStawicki")), webApiKey);
        //        AllegroNewWCF.SellFormType[] forms = s.sellFormFieldsList;
        //        sb.AppendLine(String.Format("Kategoria: {0}", categoryId));

        //        foreach (AllegroNewWCF.SellFormType form in forms)
        //        {

        //        sb.AppendLine(String.Format("sellformid: {0}, title: {5}, typ: {3}, sellformdesc: {1}, sellformfielddesc: {2}, sellformoptsvalues: {4} ", form.sellFormId,
        //                form.sellFormDesc,
        //                form.sellFormFieldDesc,
        //                form.sellFormType,
        //                form.sellFormOptsValues,
        //                form.sellFormTitle));
        //        }
        //    }catch(Exception ex)
        //    {
        //        return String.Format("Błąd: {0}", ex.Message);

        //    }
        //    return sb.ToString();
        //    }


        //        //outfile.WriteLine();
        //        //outfile.WriteLine();
        //        //outfile.WriteLine();
        //        //foreach (AllegroNewWCF.FieldsValue field in fieldsItem)
        //        //{
        //        //    outfile.WriteLine(String.Format("id: {0}, fvaluestring: {1}", field.fid, field.fvaluestring));

        //        //}
        //    }

        //    //// pobierz ilość
        //    //AllegroNewWCF.FieldsValue quantityField = fieldsItem.Where(x => x.fid == 5).FirstOrDefault();
        //    //quantityField.fvalueint = 1;

        //    //AllegroNewWCF.FieldsValue[] fields = null;// new AllegroNewWCF.FieldsValue[] { quantityField };
        //    //List<int> fieldsToRemove = new List<int>();

        //    //for (int id = 17; id < 24; id++)
        //    //{
        //    //    AllegroNewWCF.FieldsValue imageField = fieldsItem.Where(x => x.fid == id).FirstOrDefault();
        //    //    if (imageField != null)
        //    //        fieldsToRemove.Add(id);
        //    //}


        //    //AllegroNewWCF.ChangedItemStruct change = allegroService.doChangeItemFields(
        //    //    GetSessionHandle(),
        //    //itemId,
        //    //fields,
        //    //fieldsToRemove.ToArray(),
        //    //0);

        //}

        public List<Helper.ProductCategoryMessage> SendOffers(string userName,
            int batchId,
            string titlePattern,
            string templateFile,
            string path)
        {
            /*  Bll.AllegroHelper.GetVersionKey(userName);

              Dal.OrderHelper oh = new Dal.OrderHelper();
              List<Dal.ProductCatalogAllegroItemsView> items =
                  oh.GetProductCatalogAllegroItems(batchId).Where(x => x.IsAllegroItemCreated == false).ToList();
              List<Dal.ShopCategoryField> categoryFields = oh.GetShopCategoryFieldsForBatch(batchId);

              List<LajtIt.Bll.Helper.ProductCategoryMessage> errors = new List<Helper.ProductCategoryMessage>();

              foreach (Dal.ProductCatalogAllegroItemsView item in items)
              {
                  try
                  {
                      string itemInfo = "";
                      int isAllegroStandard;
                      AllegroNewWCF.FieldsValue[] fields = GetFieldsForItem(item, categoryFields, titlePattern, templateFile, path);
                      AllegroNewWCF.ItemTemplateCreateStruct template = new AllegroNewWCF.ItemTemplateCreateStruct()
                      {
                           itemtemplatename = "",
                            itemtemplateoption = 0
                      };
                      long itemId = allegroService.doNewAuctionExt(out itemInfo, out isAllegroStandard, GetSessionHandle(userName), fields, 0, 0, template);
                      oh.SetProductCatalogAllegroItemSent(item.Id, itemId);

                      errors.Add(
                          new Helper.ProductCategoryMessage() { Id = item.Id, ErrorMessage = itemInfo, IsError = false }
                          );
                  }
                  catch (Exception ex)
                  {
                      errors.Add(
                          new Helper.ProductCategoryMessage() { Id = item.Id, ErrorMessage = ex.Message, IsError = true }
                          );

                  }
              }

            
              return errors;
             * */
            return null;
        }
        /// <summary>
        /// Tworzy nowe aukcje na Allegro
        /// </summary>
        /// <param name="productCatalogId"></param>
        internal void CreateNewAllegroAuction(int productCatalogId, long allegroUserId, List<ProductCatalogAllegroItemsFnResult> itemsCreating)
        { 

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
             


            if (itemsCreating.Where(x => x.ProductCatalogId == productCatalogId).Count() == 0)
            {

                Dal.ProductCatalogAllegroItem pcai = new ProductCatalogAllegroItem()
                {
                    ProductCatalogId = productCatalogId,
                    InsertDate = DateTime.Now,
                    AllegroUserId = allegroUserId,
                    IsImageReady = false
                };

                Dal.OrderHelper oh = new Dal.OrderHelper();

                oh.SetProductCatalogAllegroItem(pcai);
            }
        }

        internal void UpdateAllegroItem(ProductCatalogAllegroItemsActive item, string updateCommand)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            bool isImageReady = false;

            if (Bll.ShopHelper.CanUpdateField(false, updateCommand, Dal.Helper.ProductUpdateFlag.Images) ||
                Bll.ShopHelper.CanUpdateField(false, updateCommand, Dal.Helper.ProductUpdateFlag.Description) ||
                Bll.ShopHelper.CanUpdateField(false, updateCommand, Dal.Helper.ProductUpdateFlag.Attributes) ||
                Bll.ShopHelper.CanUpdateField(false, updateCommand, Dal.Helper.ProductUpdateFlag.All))
                isImageReady = false;
            else
                isImageReady = true;


                oh.SetProductCatalogAllegroItemUpdate(item.ItemId.Value, updateCommand, isImageReady);
        }
        /// <summary>
        /// Tworzy nowe aukcje na Allegro
        /// </summary>
        /// <param name="productCatalogId"></param>
        //internal void CreateNewAllegroAuction(Dal.ProductCatalogAllegroItemBatch batch, int productCatalogId)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    if (oh.GetProductCatalogAllegroItemCreating(productCatalogId))
        //        return;

        //    Dal.ProductCatalog pc = pch.GetProductCatalog(productCatalogId); 

        //    List<Dal.ProductCatalogAllegroItem> items = new List<Dal.ProductCatalogAllegroItem>();
        //    items.Add(new Dal.ProductCatalogAllegroItem()
        //    {
        //        AllegroItemGuid = Guid.NewGuid(),
        //        InsertDate = DateTime.Now,
        //        AllegroItemStatusId = (int)Dal.Helper.ProductAllegroItemStatus.Verifying,
        //        ProductCatalogId = pc.ProductCatalogId

        //    });
        //    items[0].ProductCatalogAllegroItemBatch = batch;

        //    oh.SetProductCatalogAllegroItem(items);
        //}
        //public void VerifyOffers(Dal.ProductCatalogAllegroItemBatch batch )
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    int[] availableStatuses = new int[]
        //   {
        //         (int)Dal.Helper.ProductAllegroItemStatus.New,
        //          (int)Dal.Helper.ProductAllegroItemStatus.VerifiedError,
        //           (int)Dal.Helper.ProductAllegroItemStatus.VerifiedOK,
        //            (int)Dal.Helper.ProductAllegroItemStatus.Verifying

        //   };
        //    // nowe uruchomienie resetuje status wszystkich produktów
        //    oh.SetProductCatalogAllegroItemsResetStatus(batch.BatchId, Dal.Helper.ProductAllegroItemStatus.New);

        //    VerifyInternal(batch, availableStatuses);



        //    if (batch.BatchStatusId == (int)Dal.Helper.ProductAllegroItemBatchStatus.VerifyingAndCreating)
        //        oh.SetProductCatalogAllegroItemBatchStatus(batch.BatchId, Dal.Helper.ProductAllegroItemBatchStatus.Creating);
        //    else
        //        oh.SetProductCatalogAllegroItemBatchStatus(batch.BatchId, Dal.Helper.ProductAllegroItemBatchStatus.Verified);

        //}

        //private bool VerifyInternal(Dal.ProductCatalogAllegroItemBatch batch, int[] availableStatuses)
        //{
        //    string userName = "";

        //    Dal.OrderHelper oh = new Dal.OrderHelper();


        //    List<Dal.ProductCatalogAllegroItemsView> items = oh.GetProductCatalogAllegroItems(batch.BatchId)
        //    .Where(x => availableStatuses.Contains(x.AllegroItemStatusId)).ToList();


        //    List<Dal.ProductCatalogAllegroBatchFieldView> batchFields = oh.GetProductCatalogAllegroBatchFieldView(batch.BatchId);



        //    foreach (Dal.ProductCatalogAllegroItemsView item in items)
        //    {
        //        userName = Dal.Helper.GetUserName(item.AllegroUserIdAccount.Value);

        //        if (userName == null)
        //            return false;
        //        Bll.AllegroHelper.GetVersionKey(userName);

        //        Dal.ProductCatalogAllegroItemBatch batchToCheck = oh.GetProductCatalogAllegroItemBatch(batch.BatchId);
        //        int[] batchStatusId = new int[] { (int)Dal.Helper.ProductAllegroItemBatchStatus.Verifying, (int)Dal.Helper.ProductAllegroItemBatchStatus.VerifyingAndCreating };
        //        if (!batchStatusId.Contains(batchToCheck.BatchStatusId))
        //            return false;

        //        oh.SetProductCatalogAllegroItemStatus(item.Id, Dal.Helper.ProductAllegroItemStatus.Verifying);

        //        List<Dal.ProductCatalogAllegroBatchFieldView> batchFieldsToVerify =
        //            batchFields.Where(x => (x.BatchId.HasValue && x.BatchId == batch.BatchId
        //                ||
        //                (x.ProductCatalogId == item.ProductCatalogId))).ToList();

        //        Dal.ProductCatalogAllegroItem itemToUpdate = VerifyInAllegro(userName, item, batchFieldsToVerify, batchToCheck);

        //        oh.SetProductCatalogAllegroItemUpdate(itemToUpdate);
        //    }

        //    // jeśli doszły transakcje w międzyczasie to je też sprawdź
        //    int[] newStatuses = new int[] { (int)Dal.Helper.ProductAllegroItemStatus.New, (int)Dal.Helper.ProductAllegroItemStatus.Verifying };
        //    items = oh.GetProductCatalogAllegroItems(batch.BatchId)
        //      //  .Where(x => availableStatuses.Contains(x.AllegroItemStatusId)).ToList();
        //      .Where(x => newStatuses.Contains(x.AllegroItemStatusId)).ToList();
        //    if (items.Count > 0)
        //        return VerifyInternal(batch, newStatuses);
        //    else
        //        return false;

        //}

        //private Dal.ProductCatalogAllegroItem VerifyInAllegro(string userName, 
        //    Dal.ProductCatalogAllegroItemsView item,
        //    List<Dal.ProductCatalogAllegroBatchFieldView> batchFields,
        //    ProductCatalogAllegroItemBatch batch)
        //{
        //    Dal.ProductCatalogAllegroItem itemToUpdate;
        //    try
        //    {
        //        string priceDesc = "";
        //        int isAllegroStandard;

        //        //List<Dal.ProductCatalogAllegroFieldView> fieldsToVerify =
        //        //    GetFields(batchFields);

        //        AllegroNewWCF.FieldsValue[] fields = GetAllegroItemFields(
        //            item,                    
        //            Dal.Helper.GetUserId(userName), 
        //            batchFields).OrderBy(x=>x.fid).ToArray() ; 

        //        string s = allegroService.doCheckNewAuctionExt(GetSessionHandle(userName), fields, null, null, null, out priceDesc, out isAllegroStandard);

        //        decimal cost = 0;
        //        if (Decimal.TryParse(s, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, System.Threading.Thread.CurrentThread.CurrentCulture, out cost))
        //        {
        //        }



        //        itemToUpdate = new Dal.ProductCatalogAllegroItem()
        //        {
        //            Id = item.Id,
        //            AllegroItemStatusId = (int)Dal.Helper.ProductAllegroItemStatus.VerifiedOK,
        //            Comment = priceDesc,
        //            LastUpdateDateTime = DateTime.Now,
        //            Cost = cost,
        //            AllegroItemCreateDate = null,
        //            ItemId = null
        //        };

        //    }
        //    catch (Exception ex)
        //    {

        //        itemToUpdate = new Dal.ProductCatalogAllegroItem()
        //        {
        //            Id = item.Id,
        //            AllegroItemStatusId = (int)Dal.Helper.ProductAllegroItemStatus.VerifiedError,
        //            Comment = String.Format("Błąd weryfikacji: {0} {1}", ex.Message, ex.StackTrace),
        //            LastUpdateDateTime = DateTime.Now,
        //            Cost = 0,
        //            AllegroItemCreateDate = null,
        //            ItemId = null,
        //            CommentSimple = ex.Message 
        //        };
        //        Bll.EmailSender emailSender = new EmailSender();
        //        emailSender.SendEmail(new Dto.Email()
        //        {
        //            Body = String.Format("Batch: {0}<br>Produkt: <a href='http://192.168.0.107/ProductCatalog.Specification.aspx?id={2}' target=_blank>{3}</a><br><Br>{1}", 
        //                batch.Name,
        //                ex.Message, 
        //                item.ProductCatalogId, 
        //                item.AllegroName),
        //            FromEmail = Dal.Helper.MyEmail,
        //            FromName = "System",
        //            Subject = String.Format("Błąd w procedurze weryfikacji aukcji Allegro {0}", item.AllegroName),
        //            ToEmail = Dal.Helper.BackendEmail,
        //            ToName = Dal.Helper.BackendEmail

        //        });

        //    }
        //    return itemToUpdate;
        //}


        //public void CreateOffers(int batchId)
        //{
        //    string userName;
        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    int[] availableStatuses = new int[]
        //    {
        //           (int)Dal.Helper.ProductAllegroItemStatus.VerifiedOK,
        //           (int)Dal.Helper.ProductAllegroItemStatus.CreatingError

        //    };

        //    List<Dal.ProductCatalogAllegroItemsView> items = oh.GetProductCatalogAllegroItems(batchId) 
        //        .Where(x => availableStatuses.Contains(x.AllegroItemStatusId))
        //        .ToList();


        //    List<Dal.ProductCatalogAllegroBatchFieldView> batchFields = oh.GetProductCatalogAllegroBatchFieldView(batchId);

        //    List<LajtIt.Bll.Helper.ProductCategoryMessage> errors = new List<Helper.ProductCategoryMessage>();


        //    foreach (Dal.ProductCatalogAllegroItemsView item in items)
        //    {
        //        userName = Dal.Helper.GetUserName(item.AllegroUserIdAccount.Value);

        //        if (userName == null)
        //            return;
        //        Bll.AllegroHelper.GetVersionKey(userName);


        //        Dal.ProductCatalogAllegroItemBatch batchToCheck = oh.GetProductCatalogAllegroItemBatch(batchId);



        //        if (batchToCheck.BatchStatusId != (int)Dal.Helper.ProductAllegroItemBatchStatus.Creating)
        //            return;



        //        oh.SetProductCatalogAllegroItemStatus(item.Id, Dal.Helper.ProductAllegroItemStatus.Creating);

        //        Dal.ProductCatalogAllegroItem itemToUpdate = null;

        //        List<Dal.ProductCatalogAllegroBatchFieldView> batchFieldsFiltered =
        //         batchFields.Where(x => (x.BatchId.HasValue && x.BatchId == batchId
        //             ||
        //             ( x.ProductCatalogId == item.ProductCatalogId))).ToList();




        //        try
        //        {
        //            string priceDesc = "";
        //            int isAllegroStandard;

        //            AllegroNewWCF.FieldsValue[] fields = GetAllegroItemFields(item,
        //                          Dal.Helper.GetUserId(userName),
        //                          batchFieldsFiltered); ;

        //            //AllegroNewWCF.AfterSalesServiceConditionsStruct afterStruct = new AllegroNewWCF.AfterSalesServiceConditionsStruct();
        //            LajtIt.Bll.AllegroNewWCF.AfterSalesServiceConditionsStruct afterSalesServiceConditions = new AllegroNewWCF.AfterSalesServiceConditionsStruct();
        //            afterSalesServiceConditions.impliedWarranty = item.ImpliedWarranty;// "fcc7807f-6c3e-4a6b-ac4e-0172b35656d3";
        //            afterSalesServiceConditions.returnPolicy = item.ReturnPolicy;// "46c940c9-73c1-47a3-af68-ab5675b505ec";
        //            afterSalesServiceConditions.warranty = item.Warranty;// "89dbf6c6-7944-418e-9e25-b046de8dab9b";

        //            long itemId = allegroService.doNewAuctionExt(
        //                GetSessionHandle(userName), // string sessionHandle,
        //                fields,                     // LajtIt.Bll.AllegroNewWCF.FieldsValue[] fields,
        //                0,                          // int itemTemplateId,
        //                item.Id,                    // int localId,
        //                null,                       // LajtIt.Bll.AllegroNewWCF.ItemTemplateCreateStruct itemTemplateCreate,
        //                null,                       // LajtIt.Bll.AllegroNewWCF.VariantStruct[] variants,
        //                GetAllegroTags(item.SupplierName, item.AllegroTags),                       // LajtIt.Bll.AllegroNewWCF.TagNameStruct[] tags,
        //                afterSalesServiceConditions,// LajtIt.Bll.AllegroNewWCF.AfterSalesServiceConditionsStruct afterSalesServiceConditions,
        //                null,
        //                out priceDesc,              // out string itemInfo,
        //                out isAllegroStandard);     // out int itemIsAllegroStandard



        //           // System.Threading.Thread.Sleep(3000);
        //            decimal cost = 0;
        //            if (Decimal.TryParse(priceDesc, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, System.Threading.Thread.CurrentThread.CurrentCulture, out cost))
        //            {
        //            }
        //            errors.Add(new Helper.ProductCategoryMessage() { Id = item.Id, ErrorMessage = priceDesc, IsError = false, Cost = cost });

        //            itemToUpdate = new Dal.ProductCatalogAllegroItem()
        //            {
        //                Id = item.Id,
        //                AllegroItemStatusId = (int)Dal.Helper.ProductAllegroItemStatus.Created,
        //                Comment = priceDesc,
        //                LastUpdateDateTime = DateTime.Now,
        //                Cost = cost,
        //                AllegroItemCreateDate = DateTime.Now,
        //                ItemId = itemId
        //            };

        //        }
        //        catch (Exception ex)
        //        {
        //            #region handle error
        //            itemToUpdate = new Dal.ProductCatalogAllegroItem()
        //            {
        //                Id = item.Id,
        //                AllegroItemStatusId = (int)Dal.Helper.ProductAllegroItemStatus.CreatingError,
        //                Comment = String.Format("Błąd wystawiania aukcji: {0}", ex.Message),
        //                LastUpdateDateTime = DateTime.Now,
        //                Cost = 0,
        //                AllegroItemCreateDate = null,
        //                ItemId = null,
        //                IsFixed=false,
        //                CommentSimple = ex.Message
        //            };


        //            errors.Add(new Helper.ProductCategoryMessage() { Id = item.Id, ErrorMessage = ex.Message, IsError = true });


        //            Bll.EmailSender emailSender = new EmailSender();
        //            emailSender.SendEmail(new Dto.Email()
        //            {
        //                Body = String.Format("Batch: {0}, <a href='http://192.168.0.107/ProductCatalog.Preview.aspx?id={2}'>{3}</a>,  błąd: {1}", 
        //                batchToCheck.Name, 
        //                itemToUpdate.Comment,
        //                item.ProductCatalogId, 
        //                item.Name),
        //                FromEmail = Dal.Helper.MyEmail,
        //                FromName = "Lajtit backend",
        //                Subject = "Błąd w procedurze wystawiania aukcji",
        //                ToEmail = Dal.Helper.DevEmail,
        //                ToName = Dal.Helper.MyEmail

        //            });
        //            #endregion
        //        }
        //        oh.SetProductCatalogAllegroItemUpdate(itemToUpdate);
        //    }
        //    items = oh.GetProductCatalogAllegroItems(batchId)
        //        .Where(x => availableStatuses.Contains(x.AllegroItemStatusId)).ToList();

        //    if (items.Count == 0)
        //        oh.SetProductCatalogAllegroItemBatchStatus(batchId, Dal.Helper.ProductAllegroItemBatchStatus.Created);
        //    else
        //        oh.SetProductCatalogAllegroItemBatchStatus(batchId, Dal.Helper.ProductAllegroItemBatchStatus.CreatedWithErrors);

        //}

        /// <summary>
        /// Pobiera parametry na podstawie Atrybutów produktu
        /// </summary>
        /// <param name="productCatalogId"></param>
        /// <param name="onlyAllegroParameters">Przy akautlizacji aukcji wybiera się zakres danych do aktualizacji. Wybór Parametrów musi ograniczać ilość pól. To jest konfigurowalne</param>
        /// <returns></returns>
        //private IEnumerable<AllegroNewWCF.FieldsValue> GetFields(int productCatalogId, bool onlyAllegroParameters)
        //{
        //    Dal.ProductCatalogHelper pch = new
        //        Dal.ProductCatalogHelper();

        //    List<Dal.ProductCatalogAttributesToAllegroParametersResult> parameters =
        //        pch.GetProductCatalogAttributesToAllegroParameters(productCatalogId);

        //    if (onlyAllegroParameters)
        //        parameters = parameters.Where(x => x.UpdateParameter).ToList();

        //    List<AllegroNewWCF.FieldsValue> fields = new List<AllegroNewWCF.FieldsValue>();

        //    foreach (Dal.ProductCatalogAttributesToAllegroParametersResult parameter in parameters)
        //    {
        //        switch (parameter.FieldType)
        //        {
        //            case 1:
        //                if (parameter.UseDefaultValue)
        //                    fields.Add(NewIntField(parameter.FieldId, parameter.IntValue.Value));
        //                else
        //                    if (parameter.ParameterIntValue.HasValue)
        //                    fields.Add(NewIntField(parameter.FieldId, (int)parameter.ParameterIntValue.Value));
        //                break;

        //            case 2:
        //                if (parameter.StringValue != null)
        //                    fields.Add(NewStringField(parameter.FieldId, parameter.StringValue));
        //                break;
        //            case 3:
        //                if (parameter.UseDefaultValue)
        //                    fields.Add(NewFloatField(parameter.FieldId, parameter.IntValue.Value));
        //                else
        //                    if (parameter.ParameterIntValue.HasValue)
        //                    fields.Add(NewFloatField(parameter.FieldId, parameter.ParameterIntValue.Value));
        //                break;
        //        }
        //    }
        //    return fields;
        //}
        //private List<int> GetEmptyFields(int productCatalogId)
        //{
        //    Dal.ProductCatalogHelper pch = new
        //        Dal.ProductCatalogHelper();

        //    List<Dal.ProductCatalogAttributesToAllegroParametersResult> parameters =
        //        pch.GetProductCatalogAttributesToAllegroParameters(productCatalogId) .Where(x => x.UpdateParameter).ToList();

        //    return parameters.Where(x => x.ParameterIntValue.HasValue == false && x.UseDefaultValue==false).Select(x => x.FieldId).ToList(); 
        //}

        /// <summary>
        /// Pola uzwane do nowych aukcji oraz aktualizowania istniejacych
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        //private AllegroNewWCF.FieldsValue[] GetAllegroItemFields(Dal.ProductCatalogAllegroItemsView item,
        //     long allegroUserId, List<Dal.ProductCatalogAllegroBatchFieldView> batchFields)
        //{
        //    List<AllegroNewWCF.FieldsValue> fields = new List<AllegroNewWCF.FieldsValue>();

        //    Bll.OrderHelper oh = new Bll.OrderHelper();
        //    //string templateFile = 
        //    //    String.Format(@"{0}\{1}",
        //    //    ConfigurationManager.AppSettings[String.Format("WWWDir_{0}", Dal.Helper.Env.ToString())], 
        //    //    item.AllegroTemplate);
             
        //    fields.AddRange(GetFields(item.ProductCatalogId, false));
        //    if (batchFields != null) fields.AddRange(GetFieldsFromBatch(batchFields));


        //    fields.Add(NewIntField(2, item.ProductCategoryId.Value)); // kategoria
        //    fields.Add(NewIntField(9, 1)); // kraj PL
        //    fields.Add(NewIntField(10, 5)); // województwo
        //    fields.Add(NewStringField(11, "Łódź")); //  Miejscowość
        //    fields.Add(NewIntField(12, 1)); //  Transport [Kupujący pokrywa koszty transportu]
        //    fields.Add(NewIntField(13, 32)); // Opcje dot. transportu
        //    fields.Add(NewIntField(14, 32)); // Formy płatności [Wystawiam faktury VAT]
        //    fields.Add(NewIntField(28, 0)); //  Sztuki/Komplety/Pary [Sztuk]
        //    fields.Add(NewStringField(32, "93-428")); //  Kod pocztowy
        //    fields.AddRange(GetImagesFields(item.ProductCatalogId));
        //    //fields.Add(NewStringField(24, oh.GetProductCatalogPreview(allegroUserId, false, false, false, templateFile, item.Id))); // opis
        //    fields.Add(NewStringField(341, Bll.OrderHelper.GetProductCatalogPreviewSpecificationFromAttJson(item, fields.Where(x => x.fid >= 16 && x.fid <= 22).Count()))); // opis
        //    fields.Add(NewFloatField(8, GetPrice(item))); // cena kup teraz    
        //    fields.Add(NewIntField(35, 5)); // sellformid: 35, title: Darmowe opcje przesyłki, typ: 6, sellformdesc: Odbiór osobisty|Przesyłka elektroniczna (e-mail)|Odbiór osobisty po przedpłacie, sellformfielddesc: , sellformoptsvalues: 1|2|4 
        //    fields.Add(NewStringField(1,  GetAllegroName(item))); // tytuł aukcji 


        //    //fields.Add(NewStringField(43547, item.Code)); //  Kod produktu
        //    //if (!String.IsNullOrEmpty(item.Ean)) fields.Add(NewStringField(337, item.Ean)); // tytuł aukcji
        //    fields.Add(NewIntField(340, item.DeliveryDays)); //  Wysyłka w ciągu
        //    if (item.EnablePromotions) fields.Add(NewIntField(15, 8)); // wyróżnienie

        //    int quantity = item.MaxProductsForAllegro.Value;
        //    if (item.IsOnStock && !item.IsAvailable)
        //    {
        //        quantity = item.LeftQuantity.Value;// + 10;
        //    }
        //    if (item.IsAuction.HasValue && item.IsAuction.Value)
        //    {
        //        fields.Add(NewIntField(5, 1)); // ilość
        //        fields.Add(NewFloatField(6, 1)); // Cena wywoławcza, typ: 3, sellformdesc: , sellformfielddesc: Cena, od której rozpocznie się licytacja. Najniższa cena wywoławcza to <b>1 zł</b>., sellformoptsvalues:  
        //    }
        //    else
        //        fields.Add(NewIntField(5, quantity)); // ilość


        //    //fields.Add(NewIntField(4, 99)); //  Sztuki/Komplety/Pary [Sztuk]


        //    return fields.OrderBy(x=>x.fid).ToArray();
        //}

        //private decimal GetPrice(ProductCatalogAllegroItemsView item)
        //{
        //    if (item.AllegroPriceTypeId == 2 && item.IsActivePricePromo)
        //        return item.PriceBruttoPromo.Value;
        //    else
        //    { 
        //            return item.PriceBruttoAllegro.Value;
        //    }
        //}

        //private List<FieldsValue> GetFieldsFromBatch(List<ProductCatalogAllegroBatchFieldView> batchFields)
        //{
        //    List<FieldsValue> allegroFields = new List<FieldsValue>();

        //    foreach (ProductCatalogAllegroBatchFieldView field in batchFields)
        //    {
        //        FieldsValue fv = new FieldsValue(); 

        //        switch (field.FieldType)
        //        {
        //            case 1:
        //                fv = NewIntField(field.FieldId.Value, field.IntValue.Value);
        //                break;
        //            //case 2:
        //            //    fv.fvalueString = field.IntValue.Value;
        //            //    break;
        //            case 3:
        //                fv = NewFloatField(field.FieldId.Value, field.FloatValue.Value);
        //                break;



        //        }
        //        allegroFields.Add(fv);
        //    }
        //    return allegroFields;
        //}

    //    private List<FieldsValue> GetImagesFields(int productCatalogId)
    //    {
    //        List<FieldsValue> imageFields = new List<FieldsValue>();
    //        // zdjęcia
    //        Dal.OrderHelper dalOh = new Dal.OrderHelper();
    //        int imageId = 16;
    //        // string path = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];

    //        string path =
    //String.Format(@"{0}\Images\ProductCatalog",
    //ConfigurationManager.AppSettings[String.Format("WWWDir_{0}", Dal.Helper.Env.ToString())]);
    //        path = path + @"\{0}";


    //        List<Dal.ProductCatalogImage> images = dalOh
    //            .GetProductCatalogImages(productCatalogId)
    //            .Where(x => x.IsActive && String.IsNullOrEmpty(x.LinkUrl))
    //            .OrderBy(x => x.Priority)
    //            .ToList();


    //        foreach (Dal.ProductCatalogImage image in images)
    //        {

    //            imageFields.Add(NewImageField(imageId, path, image.FileName));

    //            imageId++;
    //            if (imageId > 23) break;
    //        }
    //        return imageFields;
    //    }

    //    private string GetAllegroName(Dal.ProductCatalogAllegroItemsView item)
    //    {
    //        //Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
    //        //List<Dal.ProductCatalogSynonim> synonims = pch.GetProductCatalogSynonimsForCreating(item.ProductCatalogId);

    //        //if (synonims.Count == 0)
    //        return Bll.Helper.ReplaceInvalidAllegroCharacters(item.AllegroName);
    //        //else
    //        //{
    //        //    return synonims.OrderBy(qu => Guid.NewGuid()).FirstOrDefault().Name;
    //        //}
    //    }

        //private AllegroNewWCF.FieldsValue NewImageField(int id, string path, string fileName)
        //{
        //    return new AllegroNewWCF.FieldsValue()
        //    {
        //        fid = id,
        //        fvalueImage = System.IO.File.ReadAllBytes(String.Format(path, fileName)),
        //    };
        //}

        //private AllegroNewWCF.FieldsValue NewFloatField(int id, decimal value)
        //{
        //    return new AllegroNewWCF.FieldsValue()
        //    {
        //        fid = id,
        //        fvalueFloat = (float)Math.Round(value, 2),
        //        fvalueFloatSpecified = true
        //    };
        //}


        //private AllegroNewWCF.FieldsValue NewIntField(int id, int value)
        //{
        //    return new AllegroNewWCF.FieldsValue()
        //    {
        //        fid = id,
        //        fvalueInt = value,
        //        fvalueIntSpecified = true
        //    };
        //}

        //private AllegroNewWCF.FieldsValue NewStringField(int id, string value)
        //{
        //    return new AllegroNewWCF.FieldsValue()
        //    {
        //        fid = id,
        //        fvalueString = value

        //    };
        //}


        //public List<Dal.AllegroSiteJournal> GetSiteJournal(long startpoint, string userName)
        //{
        //    List<Dal.AllegroSiteJournal> journals = new List<Dal.AllegroSiteJournal>();
        //    var j = doGetSiteJournal(startpoint, userName);
        //    journals.AddRange(j.Select(x =>
        //        new Dal.AllegroSiteJournal()
        //        {
        //            ChangeDate = x.changeDate,
        //            ChangeType = x.changeType,
        //            CurrentPrice = (decimal)x.currentPrice,
        //            ItemId = x.itemId,
        //            RowId = x.rowId,
        //            UserSellerId = x.itemSellerId
        //        }
        //        ));
        //    int i = j.Count();
        //    if (i == 100)
        //    {
        //        long s = journals[i - 1].RowId;
        //        journals.AddRange(GetSiteJournal(s, userName));
        //    }
        //    return journals;
        //}

        public AllegroNewWCF.SiteJournal[] doGetSiteJournal(long startpoint, string userName, int getLoggedInUserJournal)
        {
            var j = allegroService.doGetSiteJournal(GetSessionHandle(userName), startpoint, getLoggedInUserJournal);
            return j;
        }

        public List<Dal.AllegroSiteJournalDeal> GetSiteJournalDeals(long startpoint, string userName)
        {
            List<Dal.AllegroSiteJournalDeal> journals = new List<Dal.AllegroSiteJournalDeal>();
            var j = doGetSiteJournalDeals(startpoint, userName);
            journals.AddRange(j.Select(x =>
                new Dal.AllegroSiteJournalDeal()

                {
                    DealBuyerId = x.dealBuyerId,
                    DealEventId = x.dealEventId,
                    DealEventTime = x.dealEventTime,
                    DealEventType = x.dealEventType,
                    DealId = x.dealId,
                    DealItemId = x.dealItemId,
                    DealQuantity = x.dealQuantity,
                    DealSellerId = x.dealSellerId,
                    DealTransactionId = x.dealTransactionId,


                }


                ));
            int i = j.Count();
            if (i == 100)
            {
                long s = journals[i - 1].DealEventId;
                journals.AddRange(GetSiteJournalDeals(s, userName));
            }
            return journals;
        }

        public AllegroNewWCF.SiteJournalDealsStruct[] doGetSiteJournalDeals(long startpoint, string userName)
        {
            var j = allegroService.doGetSiteJournalDeals(GetSessionHandle(userName), startpoint);
            return j;
        }

        //internal void UpdateAllegroItems()
        //{
             

        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    List<Dal.ProductCatalogAllegroItemsActive> items = pch.GetAllegroItemsToUpdate();


        //    foreach (Dal.ProductCatalogAllegroItemsActive item in items)
        //    {
        //        UpdateAllegroItemInternal(item, item.UpdateCommand);
        //    }

        //}

        //public  void UpdateAllegroItemInternal(ProductCatalogAllegroItemsActive item, string updateCommand)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    string userName = "";
        //    //Dal.ProductCatalogAllegroItemsActive itemOriginal = pch.GetProductCatalogAllegroItem(item.Id);

        //    //if (itemOriginal == null || itemOriginal.UpdateStatus != (int)Dal.Helper.AllegroItemUpdateStatus.Verifying)
        //    //    return;

        //    #region wybierz konto
        //    Dal.ProductCatalogAllegroItemsView firstItem = pch.GetProductCatalogAllegroItemView(item.Id);


        //    if (firstItem == null)
        //        return;
        //    else
        //        userName = Dal.Helper.GetUserName(firstItem.AllegroUserIdAccount.Value);

        //    if (userName == null)
        //        return;

        //    Bll.AllegroHelper.GetVersionKey(userName);
        //    #endregion


        //    Dal.ProductCatalogAllegroItem itemToUpdate = null;

        //    try
        //    {
        //        AllegroNewWCF.FieldsValue[] fields =
        //            GetAllegroItemFields(firstItem,
        //            Dal.Helper.GetUserId(item.UserName),
        //            null) // nie przekazujemy batchFields więc nie ma np czasu trwania, kosztów dostawy
        //            .OrderBy(x => x.fid)
        //            .ToArray();

        //        List<AllegroNewWCF.FieldsValue> fieldsToUpdate = new List<AllegroNewWCF.FieldsValue>();


        //        Dal.AllegroItemUpdater aiu = new Dal.AllegroItemUpdater(updateCommand);

        //        List<int> fieldsToRemove = new List<int>();// GetFieldsToRemove();

        //        #region updates

        //        if (aiu.UpdateDelivery || aiu.UpdateAll)
        //        {
        //            var f = GetAllegroItemDeliveryFields(item);

        //            fieldsToUpdate.AddRange(f);
        //            fieldsToUpdate.Add(fields.Where(x => x.fid == 340).FirstOrDefault()); // wysyłka w ciągu

        //            int[] included = f.Select(x => x.fid).Distinct().ToArray();
        //            int[] excluded = pch.AllegroDeliveryCostUniqueFields();

        //            excluded = excluded.Where(x => !included.Contains(x)).ToArray();

        //            fieldsToRemove.AddRange(excluded);
        //        }


        //        if ((aiu.UpdatePictures || aiu.UpdateAll))//&& item.BidCount == 0)
        //        {
        //            fieldsToRemove.AddRange(GetImagesFieldsToRemove());
        //            var f = GetAllegroItemImagesFields(fields);
        //            fieldsToRemove.RemoveAll(x => f.Select(y => y.fid).Contains(x));

        //            fieldsToUpdate.AddRange(f);
        //        }

        //        if (aiu.UpdateQuantity || aiu.UpdateAll)
        //        {
        //            var f = fields.Where(x => x.fid == 5).FirstOrDefault();
        //            f.fvalueInt = item.QuantityOrdered.Value + f.fvalueInt;
        //            fieldsToUpdate.Add(fields.Where(x => x.fid == 5).FirstOrDefault());

        //        }
        //        if (aiu.UpdateParameters || aiu.UpdateAll)
        //        {
        //            fieldsToUpdate.AddRange(GetFields(item.ProductCatalogId, true));
        //            fieldsToRemove.AddRange(GetEmptyFields(item.ProductCatalogId)); 
        //            pch.SetProductCatalogAttributesUpdate(item.ProductCatalogId);
        //        }

        //        if (aiu.UpdatePrice || aiu.UpdateAll)
        //        {
        //           // if (item.BidCount == 0)
        //            //{
        //            //    fieldsToUpdate.Add(fields.Where(x => x.fid == 8).FirstOrDefault());
        //            //}
        //           // else
        //           // {
        //                long id = item.ItemId.Value; 
        //                UpdateAllegroPrice(id, fields.Where(x => x.fid == 8).FirstOrDefault().fvalueFloat);
        //            //}
        //        }
        //        if ((aiu.UpdateName || aiu.UpdateAll))// && item.BidCount == 0)
        //            fieldsToUpdate.Add(fields.Where(x => x.fid == 1).FirstOrDefault());

        //        if (aiu.UpdateDescription || aiu.UpdateAll)
        //        {
        //            fieldsToUpdate.Add(fields.Where(x => x.fid == 24).FirstOrDefault());
        //            fieldsToUpdate.Add(fields.Where(x => x.fid == 341).FirstOrDefault());
        //        }
        //        #endregion
        //        if (fieldsToUpdate.Count == 0)
        //        {

        //            itemToUpdate = new Dal.ProductCatalogAllegroItem()
        //            {
        //                Id = item.Id,
        //                //AllegroItemStatusId = (int)Dal.Helper.ProductAllegroItemStatus.Created,
        //                //Comment = priceDesc,
        //                LastUpdateDateTime = DateTime.Now,
        //                Cost = 0,
        //                UpdateStatus = (int)Dal.Helper.AllegroItemUpdateStatus.OK,
        //                UpdateComment = "Brak pól do aktualizacji",
        //                UpdateDate = DateTime.Now
        //            };

        //        }
        //        else
        //        {

        //            LajtIt.Bll.AllegroNewWCF.AfterSalesServiceConditionsStruct afterSalesServiceConditions = new AllegroNewWCF.AfterSalesServiceConditionsStruct();
        //            afterSalesServiceConditions.impliedWarranty = item.ImpliedWarranty;// "fcc7807f-6c3e-4a6b-ac4e-0172b35656d3";
        //            afterSalesServiceConditions.returnPolicy = item.ReturnPolicy;// "46c940c9-73c1-47a3-af68-ab5675b505ec";
        //            afterSalesServiceConditions.warranty = item.Warranty;// "89dbf6c6-7944-418e-9e25-b046de8dab9b";

                   
        //            fieldsToUpdate = fieldsToUpdate.Where(x => x != null).ToList();

        //            AllegroNewWCF.ChangedItemStruct itemStruct =
        //                allegroService.doChangeItemFields(GetSessionHandle(userName),
        //                item.ItemId.Value,
        //                fieldsToUpdate.ToArray(),
        //                fieldsToRemove.ToArray(), 0, null, GetAllegroTags(item.SupplierName, item.AllegroTags), afterSalesServiceConditions, null);

        //            #region handle return
        //            decimal cost = (decimal)itemStruct.itemSurcharge.Where(x => x.surchargeDescription == "Suma")
        //                .Select(x => x.surchargeAmount.amountValue).FirstOrDefault();
        //            decimal costAdded = (decimal)itemStruct.itemSurcharge.Where(x => x.surchargeDescription == "Dopłata za nowe opcje")
        //                .Select(x => x.surchargeAmount.amountValue).FirstOrDefault();
        //            //System.Threading.Thread.Sleep(3000); 


        //            itemToUpdate = new Dal.ProductCatalogAllegroItem()
        //            {
        //                Id = item.Id,
        //                //AllegroItemStatusId = (int)Dal.Helper.ProductAllegroItemStatus.Created,
        //                //Comment = priceDesc,
        //                LastUpdateDateTime = DateTime.Now,
        //                Cost = item.Cost == null ? cost : item.Cost.Value + cost,
        //                UpdateStatus = (int)Dal.Helper.AllegroItemUpdateStatus.OK,
        //                UpdateComment = String.Format("Zaktualizowano. Dopłata za nowe opcje {0:C}, łącznie pobrano: {1:C}", costAdded, cost),
        //                UpdateDate = DateTime.Now
        //            };
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        itemToUpdate = new Dal.ProductCatalogAllegroItem()
        //        {
        //            Id = item.Id,
        //            LastUpdateDateTime = DateTime.Now,
        //            Cost = item.Cost,
        //            UpdateStatus = (int)Dal.Helper.AllegroItemUpdateStatus.Error,
        //            UpdateComment = String.Format("Błąd: {0}", ex.Message),
        //            UpdateDate = DateTime.Now
        //        };

        //        ErrorHandler.SendError(ex, String.Format("Nie można zaktualizować produktu <a href='http://192.168.0.107/Product.aspx?id={0}'>{0}</a> na Allegro <a href='http://allegro.pl/show_item.php?item={1}'>{1}</a>", item.ProductCatalogId, item.ItemId));
               
        //    }
        //    pch.SetProductCatalogAllegroItemUpdate(itemToUpdate);

        //}

        private TagNameStruct[] GetAllegroTags(string supplierName, bool allegroTags)
        {
            List<LajtIt.Bll.AllegroNewWCF.TagNameStruct> tags = null;
            LajtIt.Bll.AllegroNewWCF.TagNameStruct[] tag = null;
            if (allegroTags)
            {
                tags = new List<TagNameStruct>();
                tags.Add(new TagNameStruct() { tagName = supplierName });
                tag = tags.ToArray();
            }
            return tag;
        }

        private void UpdateAllegroPrice(long itemId, float price)
        { 
            AllegroRESTHelper.ChangePrice(itemId, price);
        }

        //private List<AllegroNewWCF.FieldsValue> GetAllegroItemDeliveryFields(ProductCatalogAllegroItemsActive productCatalog)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    List< AllegroDeliveryCostView> costs=  pch.GetAllegroDeliveryCost(productCatalog.ProductCatalogId);
             
        //    List<AllegroNewWCF.FieldsValue> list = new List<AllegroNewWCF.FieldsValue>();

        //    for (int i = 36; i < 63; i++)
        //    {
        //        if (costs.Where(x => x.FieldId == i).Count() > 0) {
        //            var c1 = costs.Where(x => x.FieldId == i).FirstOrDefault();
        //            list.Add(NewFloatField(c1.FieldId.Value, c1.FloatValue.Value) );

        //        }
        //        if (costs.Where(x => x.FieldId == i + 100).Count() > 0)
        //        {
        //            var c2 = costs.Where(x => x.FieldId == i + 100).FirstOrDefault();
        //            list.Add( NewFloatField(c2.FieldId.Value, c2.FloatValue.Value));

        //        }
        //        if (costs.Where(x => x.FieldId == i + 200).Count() > 0)
        //        {
        //            var c3 = costs.Where(x => x.FieldId == i + 200).FirstOrDefault();
        //            list.Add( NewIntField(c3.FieldId.Value, c3.IntValue.Value));
        //        }
        //    } 
        //    list.Add( NewIntField(35, 1)); // Darmowe opcje przesyłki, typ: 6, sellformdesc: Odbiór osobisty|Przesyłka elektroniczna (e-mail)|Odbiór osobisty po przedpłacie, sellformfielddesc: , sellformoptsvalues: 1|2|4 

        //    /// dodatkowa logika:
        //    /// 1. Jeśli produkt jest na magazynie i możliwa wysyłka paczkomatem dodaj tę opcję
        //    /// 2. Jeśli nie jest możliwy dropshipping to również dodaj opcję paczkomatu jeśli jest możliwość umieszczenia lampy w paczkomacie


        //    if ((productCatalog.IsOnStock || !productCatalog.IsDropShippingAvailable) && productCatalog.IsPaczkomatAvailable.HasValue && productCatalog.IsPaczkomatAvailable.Value)
        //    {
        //        if (list.Where(x => x.fid == 59).Count() == 0) // nie ma Allegro Paczkomaty InPost
        //        {
        //            list.Add(new FieldsValue()
        //            {
        //                fid = 59,
        //                fvalueFloat = 8.99F,
        //                fvalueFloatSpecified = true
        //            }
        //                );
        //            list.Add(new FieldsValue()
        //            {
        //                fid = 159,
        //                fvalueFloat = 0,
        //                fvalueFloatSpecified = true
        //            }

        //                );
        //            list.Add(new FieldsValue()
        //            {
        //                fid = 259,
        //                fvalueInt = 50,
        //                fvalueIntSpecified = true
        //            }
        //                );
        //        }
        //        if (list.Where(x => x.fid == 60).Count() == 0) // nie ma Allegro Paczkomaty InPost pobranie
        //        {
        //            list.Add(new FieldsValue()
        //            {
        //                fid = 60,
        //                fvalueFloat = 12.49F,
        //                fvalueFloatSpecified = true
        //            }
        //                );
        //            list.Add(new FieldsValue()
        //            {
        //                fid = 160,
        //                fvalueFloat = 0,
        //                fvalueFloatSpecified = true
        //            }

        //                );
        //            list.Add(new FieldsValue()
        //            {
        //                fid = 260,
        //                fvalueInt = 50,
        //                fvalueIntSpecified = true
        //            }
        //                );
        //        }
        //        if (list.Where(x => x.fid == 45).Count() == 0) // nie ma kurier pobranie
        //        {
        //            list.Add(new FieldsValue()
        //            {
        //                fid = 45,
        //                fvalueFloat = 18F,
        //                fvalueFloatSpecified = true
        //            }
        //                );
        //            list.Add(new FieldsValue()
        //            {
        //                fid = 145,
        //                fvalueFloat = 0,
        //                fvalueFloatSpecified = true
        //            }

        //                );
        //            list.Add(new FieldsValue()
        //            {
        //                fid = 245,
        //                fvalueInt = 50,
        //                fvalueIntSpecified = true
        //            }
        //                );
        //        }
        //    }


        //    return list;
        //}

        private List<AllegroNewWCF.FieldsValue> GetAllegroItemImagesFields(AllegroNewWCF.FieldsValue[] fields)
        {
            List<AllegroNewWCF.FieldsValue> list = new List<AllegroNewWCF.FieldsValue>();

            for (int i = 16; i < 24; i++)
            {
                if (fields.Where(x => x.fid == i).Count() > 0) list.Add(fields.Where(x => x.fid == i).FirstOrDefault());
            }

            return list;
        }
         
        private List<int> GetImagesFieldsToRemove()
        {
            List<int> list = new List<int>();

            for (int i = 16; i < 24; i++)
            {
                list.Add(i);
            }


            return list;
        }

        //private AllegroNewWCF.FieldsValue[] GetFieldsForUpdateItem(Dal.ProductCatalogAllegroItemsView item,
        //      List<Dal.ProductCatalogAllegroBatchFieldView> batchFields)
        //{

        //    // hack
        //    //item.AllegroName = item.AllegroNameOriginal;

        //    return GetAllegroItemFields(item, item.AllegroUserIdAccount.Value, batchFields);
        //}

        internal void Test()
        {
            long l = 0;
            AllegroNewWCF.ShipmentDataStruct[] itemStruct = allegroService.doGetShipmentData(
                countryCode,
                GetWebApiKey("JacekStawicki"),
                out l);

            string sb = "";
            foreach (AllegroNewWCF.ShipmentDataStruct s in itemStruct)
            {
                sb += String.Format("{0} - {1} ({2})\n", s.shipmentId, s.shipmentName, s.shipmentType);
            }

            int a;
        }

        public bool SetRefund(long itemId, int orderId, int reasonId, int quantity)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Dal.AllegroScan ah = new Dal.AllegroScan();
            string userName = ah.GetAllegroItem(itemId).AllegroUser.UserName;

            Bll.AllegroHelper.GetVersionKey(userName);

            string session = GetSessionHandle(userName);

            AllegroNewWCF.ReasonInfoType[] reasons = new AllegroNewWCF.ReasonInfoType[] { };
            long[] dealIds = GetDealId(itemId, orderId, session);

            return  SetRefund(dealIds, session, reasonId, quantity);
             
        }
        private bool SetRefund(long[] dealIds, string session, int reasonId, int quantity)
        {
            AllegroNewWCF.ReasonInfoType[] reasonsList = new AllegroNewWCF.ReasonInfoType[] { };

            foreach (int dealId in dealIds)
                allegroService.doSendRefundForm(session, dealId, reasonId, quantity);

            return true;
        }
        public AllegroNewWCF.ReasonInfoType[] GetRefundsReasons(long itemId, int orderId)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Dal.AllegroScan ah = new Dal.AllegroScan();
            string userName = ah.GetAllegroItem(itemId).AllegroUser.UserName;

            Bll.AllegroHelper.GetVersionKey(userName);

            string session = GetSessionHandle(userName);

            AllegroNewWCF.ReasonInfoType[] reasons = new AllegroNewWCF.ReasonInfoType[] { };
            long[] dealIds = GetDealId(itemId, orderId, session);

            reasons = GetRefundsReasons(dealIds, session);

            return reasons;
        }

        private AllegroNewWCF.ReasonInfoType[] GetRefundsReasons(long[] dealIds, string session)
        {
            AllegroNewWCF.ReasonInfoType[] reasonsList = new AllegroNewWCF.ReasonInfoType[] { };

            foreach (int dealId in dealIds)
                allegroService.doGetRefundsReasons(session, dealId, out reasonsList);

            return reasonsList;
        }

        private long[] GetDealId(long itemId, int orderId, string session)
        {
            List<AllegroNewWCF.FilterOptionsType> filters = new List<AllegroNewWCF.FilterOptionsType>();
            AllegroNewWCF.RefundsDealsListType[] rlt;
            AllegroNewWCF.FiltersListType[] flt;

            filters.Add(
                new AllegroNewWCF.FilterOptionsType()
                {
                    filterId = "itemId",
                    filterValueId = new string[] { itemId.ToString() }
                }
                );

            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Order order = oh.GetOrder(orderId);

            long buyerId = order.ExternalUserId;
            //filters.Add(
            //    new AllegroNewWCF.FilterOptionsType()
            //    {
            //        filterId = "userId",
            //        filterValueId = new string[] { buyerId.ToString() }
            //    }
            //    );

            allegroService.doGetRefundsDeals(session, filters.ToArray(), "asc", 100, 0, out rlt, out flt);


            return rlt
                .Where(x => x.buyerId == buyerId)
                .Select(x => x.dealId).ToArray();
        }

        internal void SetAllegroAction()
        {
            GetVersionKeys();

            Dal.ProductCatalogHelper ph = new Dal.ProductCatalogHelper();
            List<Dal.AllegroActionView> actions =
                ph.GetAllegroActionsActive().Take(1000).ToList();
            int[] actionTypes = actions.Select(x => x.TypeId).Distinct().ToArray();

            foreach (int actionType in actionTypes)
            {
                switch (actionType)
                {
                    case 1:
                        var items = actions.Where(x => x.TypeId == 1).ToList();
                        AllegroRESTHelper.DeleteAllegroItems(items); 
                        ph.SetAllegroAction(items);
                        break; 
                }

            }
        }

        //private void DeleteAllegroItems(Dal.AllegroActionView[] list)
        //{

        //    Dal.ProductCatalogHelper ph = new Dal.ProductCatalogHelper();
        //    List<Dal.AllegroActionView> itemsProcessed = new List<Dal.AllegroActionView>();

        //    long[] userIds = list.Select(x => x.UserId).Distinct().ToArray();

        //    foreach (long userId in userIds)
        //    {
        //        Dal.AllegroActionView[] items = list.Where(x => x.UserId == userId).ToArray();
        //        int count = items.Length;

        //        int partsCount = (count / 25);
        //        Dal.AllegroActionView[] itemsPart;
        //        for (int i = 0; i < partsCount + 1; i++)
        //        {
        //            itemsPart = items.Skip(i * 25).Take(25).ToArray();


        //            AllegroNewWCF.FinishItemsStruct[] finishItems = itemsPart.Select(x => new AllegroNewWCF.FinishItemsStruct()
        //            {
        //                finishItemId = x.ItemId
        //            }).ToArray();
        //            AllegroNewWCF.FinishFailureStruct[] finishItemsFailed;
        //            long[] itemsOk =
        //            allegroService.doFinishItems(
        //                GetSessionHandle(itemsPart[0].UserName),
        //                finishItems,
        //                out finishItemsFailed);

        //            itemsProcessed.AddRange(itemsOk.Select(x => new Dal.AllegroActionView()
        //            {
        //                ItemId = x,
        //                Comment = "OK",
        //                ProcessedDate = DateTime.Now,
        //                IsProcessed = true
        //            }).ToList());


        //            itemsProcessed.AddRange(finishItemsFailed.Select(x => new Dal.AllegroActionView()
        //            {
        //                ItemId = x.finishItemId,
        //                Comment = String.Format("{0} {1}", x.finishErrorCode, x.finishErrorMessage),
        //                ProcessedDate = DateTime.Now,
        //                IsProcessed = true
        //            }).ToList());

        //            ph.SetAllegroAction(itemsProcessed);
        //            itemsProcessed.Clear();

        //        }

        //    }

        //}

 
            /// <summary>
            /// 
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="getUrl">true - pobiera url do pliku, false - pobiera fizyczną lokalizację</param>
            /// <returns></returns>
        public static string GetAllegroUserHeadImage(long userId, bool getUrl)
        {
            string path = String.Format(
                ConfigurationManager.AppSettings[String.Format("ImagesDirectory_{0}", Dal.Helper.Env.ToString())],
                String.Format(@"AllegroUsers\{0}_head.jpg", userId));
             

            if (File.Exists(path))
            {
                if (getUrl)
                    return String.Format("/images/AllegroUsers/{0}_head.jpg", userId);
                else
                    return path;
            }

            return null;
        }
        internal void SetRest()
        {
        //    List<Dal.AllegroActionView> list = new List<AllegroActionView>();

        //    list.Add(new AllegroActionView()
        //    {
        //          ItemId = 7883991319,
        //          UserId = 44282528
        //    });
        //    AllegroRESTHelper.DeleteAllegroItems(list.ToArray());
        }
    }
}
