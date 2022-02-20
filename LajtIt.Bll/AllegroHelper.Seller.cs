using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public partial class AllegroHelper
    {
        /// <summary>
        /// Metoda pozwala sprzedającym na pobranie wszystkich danych z wypełnionych przez kupujących formularzy pozakupowych oraz dopłat do nich. 
        /// </summary>
        /// <param name="itemIDs"></param>
        /// <returns></returns>
        //public AllegroNewWCF.PostBuyFormDataStruct[] GetItemTransactions(string userName, long[] itemIDs)
        //{
        //    long[] transactionsidsarray = GetItemTransactionsIDs(userName, itemIDs);
        //    if (transactionsidsarray.Length == 0)
        //        return null;

        //    List<AllegroNewWCF.PostBuyFormDataStruct> buys = GetItemTransactionsForTransactionsIDs(userName, transactionsidsarray);
        //    return buys.ToArray();

        //}

        //public List<AllegroNewWCF.PostBuyFormDataStruct> GetItemTransactionsForTransactionsIDs(string userName, long[] transactionsidsarray)
        //{
        //    List<AllegroNewWCF.PostBuyFormDataStruct> buys = new List<AllegroNewWCF.PostBuyFormDataStruct>();

        //    int partsCount = (transactionsidsarray.Length / 25);

        //    for (int i = 0; i < partsCount + 1; i++)
        //    {
        //        long[] transactionIDsPart = transactionsidsarray.Skip(i * 25).Take(25).ToArray();

        //        if (transactionIDsPart.Length == 0)
        //            continue;


        //        buys.AddRange(allegroService.doGetPostBuyFormsDataForSellers(GetSessionHandle(userName), transactionIDsPart));

        //    }
        //    return buys;
        //}

        //private long[] GetItemTransactionsIDs(string userName, long[] itemIDs)
        //{
        //    //long localVersion;
        //    List<long> list = new List<long>();

        //    //long[] shipment = allegroService.doGetShipmentData(out localVersion, countryCode, webApiKey)
        //    //    .Select(x => (long)x.shipmentid)
        //    //    .ToArray();


        //    int partsCount = (itemIDs.Length / 25);
        //    long[] itemIDsPart;
        //    for (int i = 0; i < partsCount + 1; i++)
        //    {
        //        try
        //        {
        //              itemIDsPart = itemIDs.Skip(i * 25).Take(25).ToArray();

        //            if (itemIDsPart.Length == 0)
        //                continue;

        //            string session = GetSessionHandle(userName);
        //            list.AddRange(allegroService.doGetTransactionsIDs(session, itemIDsPart, "seller", null));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    return list.ToArray();
        //}


        //private long[] GetItemTransactionsIDsForItemId(long itemId)
        //{
        //    return allegroService.doGetTransactionsIDs(GetSessionHandle(), new long[] { itemId }, "seller", null);                
        //}
        /// <summary>
        /// Metoda pozwala na pobranie pełnych danych kontaktowych kontrahentów z danej oferty.
        /// </summary>
        /// <param name="itemIDs"></param>
        /// <returns></returns>
        //public AllegroNewWCF.ItemPostBuyDataStruct[] GetBuyersData(string userName, long[] itemIDs)
        //{
        //    List<AllegroNewWCF.ItemPostBuyDataStruct> list = new List<AllegroNewWCF.ItemPostBuyDataStruct>();

        //    int partsCount = (itemIDs.Length / 25);
        //    for (int i = 0; i < partsCount + 1; i++)
        //    {
        //        long[] itemIDsPart = itemIDs.Skip(i * 25).Take(25).ToArray();

        //        if (itemIDsPart.Length == 0)
        //            continue;
        //        long[] buyerFilterArray = new long[] { };

        //        list.AddRange(allegroService.doGetPostBuyData(GetSessionHandle(userName), itemIDsPart, buyerFilterArray));
        //    }

        //    return list.ToArray();
        //}

        /// <summary>
        /// Tworzy nowe aukcje na Allegro
        /// </summary>
        /// <param name="productCatalogId"></param>
        //internal void CreateNewAllegroAuction(int productCatalogId, long allegroUserId, List<ProductCatalogAllegroItemsFnResult> itemsCreating)
        //{

        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();



        //    if (itemsCreating.Where(x => x.ProductCatalogId == productCatalogId).Count() == 0)
        //    {

        //        Dal.ProductCatalogAllegroItem pcai = new ProductCatalogAllegroItem()
        //        {
        //            ProductCatalogId = productCatalogId,
        //            InsertDate = DateTime.Now,
        //            AllegroUserId = allegroUserId,
        //            IsImageReady = false
        //        };

        //        Dal.OrderHelper oh = new Dal.OrderHelper();

        //        oh.SetProductCatalogAllegroItem(pcai);
        //    }
        //}
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
        internal void SetAllegroAction()
        {
            //GetVersionKeys();

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

        internal void UpdateAllegroItem(ProductCatalogAllegroItemsView item, List<Dal.ProductCatalogShopUpdateSchedule>  schedules)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            bool? isImageReady = null;

            if (Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Images) //||
                //Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Description) ||
                //Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Attributes) ||
                //Bll.ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.All)
                )
                isImageReady = false; 

            string updateCommand = Bll.ShopUpdateHelper.GetUpdateCommandFromShopColumnTypes(schedules);

            oh.SetProductCatalogAllegroItemUpdate(item.ItemId, updateCommand, isImageReady, null);
        }


        internal void SetProductCatalogAllegroDraftItem(Dal.ProductCatalogAllegroItemsView item, 
            AllegroRESTHelper.Draft draft)
        {
            Dal.ProductCatalogAllegroItem pcai = new Dal.ProductCatalogAllegroItem()
            {
                ItemId = draft.ItemId.Value,
                ProductCatalogId=item.ProductCatalogId,
                AllegroUserId = item.UserId,
                IsValid = draft.IsValid,
                ValidatedAt = draft.ValidatedAt,
                 
                UpdateDate = DateTime.Now,
                InsertDate=DateTime.Now
            };

            if (draft.IsValid)
            {
                pcai.Comment = "Szkic oferty utworzony";
                pcai.NotificationSent = null;
            }
            else
            {
                pcai.Comment = draft.ValidationErrors;
                pcai.NotificationSent = true;
            }

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            try
            {
                pch.SetProductCatalogAllegroDraftItem(pcai, draft.AllegroCategoryId);
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, 
                    String.Format("pcai.ProductCatalogId {0}, pcai.ItemId {1}, draft.Id {2}", 
                    pcai.ProductCatalogId, pcai.ItemId, draft.Id));
                throw ex;
            }
        }
        internal void SetProductCatalogAllegroDraft(Dal.ProductCatalogAllegroItemsView item, AllegroRESTHelper.Draft draft, string itemStatus)
        {
            Dal.ProductCatalogAllegroItem pcai = new Dal.ProductCatalogAllegroItem()
            {
                Id = item.Id,
                ItemId = draft.ItemId.Value,
                AllegroUserId = item.UserId,
                IsValid = draft.IsValid,
                IsValidationError = !draft.IsValid,
                ValidatedAt = draft.ValidatedAt,
                //IsImageReady= item.IsImageReady.Value,
                Comment = draft.ValidationErrors,
                ProcessId = null 
            };

            if (draft.IsValid)
            {
               // pcai.Comment = "Szkic oferty utworzony";
                pcai.NotificationSent = null;
                pcai.UpdateCommand = null;
            }
            else
            {
               // pcai.Comment = draft.ValidationErrors;
                pcai.NotificationSent = false;
            }

            pcai.Comment =  pcai.Comment;
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            try
            {
                pch.SetProductCatalogAllegroItem(pcai, draft.AllegroCategoryId, itemStatus);
                if(draft.ProductId.HasValue && item.Ean !=null)
                    Dal.DbHelper.AllegroHelper.SetAllegroProduct(item.Ean, draft.ProductId.Value, null, true);

            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("item.ProductCatalogId {0}, item.ItemId {1}, draft.Id {2}, itemStatus: {3}", item.ProductCatalogId, pcai.ItemId, draft.Id, itemStatus));
                throw ex;
            }
        }

        //public void GetItemsUpdate(long[] itemIds, string u)
        //{
        //    Bll.AllegroScan asc = new AllegroScan();
        //    long[] k;  long[] n;  long[] b;

        //    List<Dal.AllegroItem> items = new List<AllegroItem>();

        //    int count = itemIds.Length;

        //    int partsCount = (count / 10);

        //    for (int i = 0; i < partsCount + 1; i++)
        //    {
        //        var it = itemIds.Skip(i * 10).Take(10).ToArray();
        //        Console.WriteLine(String.Join(",", it));
        //        items.AddRange(asc.AddOrUpdateAuctions(it, u, out n, out b));
        //    }

        //    Dal.AllegroScan a = new Dal.AllegroScan();
        //    a.AddOrUpdateAuctions(null, items);


        //}
    }
}
