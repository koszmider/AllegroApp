using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace LajtIt.Bll
{
    public partial class AllegroScan
    {
        int itemsCount = 10;
        public void GetMyJournal()
        {
            foreach (Dal.Helper.MyUsers e in Enum.GetValues(typeof(Dal.Helper.MyUsers)))
            {
                GetJournal(e.ToString());
            }
        }
        public void GetMyJournalDeals()
        {
            AllegroHelper ah = new AllegroHelper();

            foreach (Dal.Helper.MyUsers e in Enum.GetValues(typeof(Dal.Helper.MyUsers)))
            { 

                    long journalDealEventId = 0;
                    try
                    {
                        long userId = Dal.Helper.GetUserId(e.ToString());
                        journalDealEventId = allegroScan.GetJournalDealLastEventId(userId);

                        List<Dal.AllegroSiteJournalDeal> journalDeals = ah.GetSiteJournalDeals(journalDealEventId, e.ToString());
                        allegroScan.SetAllegroSiteJournalDeal(journalDeals);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError(ex, "GetJournal");
                        ErrorHandler.SendError(ex, String.Format("GetJournal, journalDealEventId: {0}", journalDealEventId));
                    }
                
            }
        }
        public void GetOtherJournal()
        {
            GetJournal(null);
        }
        internal void GetJournal(string userNameIn)
        {
            int getLoggedInUserJournal = 1;
            string userName;
            if (userNameIn != null)
            {
                getLoggedInUserJournal = 0;
                userName = userNameIn;
            }
            else
                userName = Dal.Helper.MyUsers.JacekStawicki.ToString();

            Bll.AllegroHelper.GetVersionKeys();

            long journalRowId = 0;
            int i = 0;
            int loops = 0;
            List<Dal.AllegroSiteJournal> journals = new List<Dal.AllegroSiteJournal>();
            try
            {
                journalRowId = allegroScan.GetJournalLastRowId(userNameIn);
                bool hasMoreJournals = false;
                journals = GetJournalInternal(userName, getLoggedInUserJournal, journalRowId, out hasMoreJournals);

                allegroScan.SetAllegroSiteJournal(journals);

                while (hasMoreJournals && loops < 5000)
                {
                    i = journals.Count();
                    long lastId = journals[i - 1].RowId;

                    journals = GetJournalInternal(userName, getLoggedInUserJournal, lastId, out hasMoreJournals);

                    allegroScan.SetAllegroSiteJournal(journals);
                    loops++;
                }

            }
            catch (Exception ex)
            {
                ErrorHandler.LogError(ex, "GetJournal");
               // ErrorHandler.SendError(ex, String.Format("GetJournal, journalRowId: {0}", journalRowId));

            }
        }

        private static List<Dal.AllegroSiteJournal> GetJournalInternal(string userName, int getLoggedInUserJournal, long journalRowId, out bool hasMoreJournals)
        {
            AllegroHelper ah = new AllegroHelper();
            var j = ah.doGetSiteJournal(journalRowId, userName, getLoggedInUserJournal);
            hasMoreJournals = j.Count() == 100;
            string[] changeTypeToExclude = new string[] { "change", "bid" };

            if (j.Where(x => !changeTypeToExclude.Contains(x.changeType)).Count() == 0)
                j = j.OrderByDescending(x => x.rowId).Take(1).ToArray();
            else
                j = j.Where(x => !changeTypeToExclude.Contains(x.changeType)).ToArray();

            return j.Select(x =>
                new Dal.AllegroSiteJournal()
                {
                    ChangeDate = x.changeDate,
                    ChangeType = x.changeType,
                    CurrentPrice = (decimal)x.currentPrice,
                    ItemId = x.itemId,
                    RowId = x.rowId,
                    UserSellerId = x.itemSellerId
                }
                )
                
                .ToList(); 
        }

    
        //public void ProcessEndJournal(string actingUser)
        //{
        //    //Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.JacekStawicki.ToString());
        //    Bll.AllegroHelper.GetVersionKeys();// (Dal.Helper.MyUsers.CzerwoneJablko.ToString());

        //    ProcessAllegroItems(allegroScan.GetMyEndJournal(), true);
             
        //}
        //public void ProcessOtherStartJournal(string actingUser)
        //{
        //    Bll.AllegroHelper.GetVersionKeys();// (Dal.Helper.MyUsers.JacekStawicki.ToString());

        //    ProcessAllegroItems(allegroScan.GetOtherStartJournal());
        //}

   
        //public void ProcessMyJournal(string actingUser)
        //{

        //    Bll.AllegroHelper.GetVersionKeys();//(Dal.Helper.MyUsers.JacekStawicki.ToString());

        //   // Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.CzerwoneJablko.ToString());
        //   // GetJournal();

        //    // wstaw/zaktualizuj aukcje
        //    ProcessMyAuctions();
        //    // pobierz formularze dostawy i płatności PayU
        //    ProcessDeals();

        //    // utwórz AllegroItemOrders
        //    ProcessAllegroItemOrders();

        //    // Tworzenie zamówień w systemie
        //    // tworzymy juz nowym sposobem
        //   /// CreateOrders();
             
        //    /// Przypisywanie płatności
         
        //    //Bll.OrderHelper ohb = new Bll.OrderHelper();
        //    //ohb.SetPaymentsForOrders(actingUser);

        //  //  RefreshUserLastUpdate((int)Dal.Helper.MyID);

        //}

        public void ShopSync()
        {
            Bll.ShopHelper sh = new ShopHelper();
            sh.GetShopProductsNotInSystem();

        }

        private void ProcessAllegroItemOrders()
        {
            //List<Dal.AllegroSiteJournal> journals =
            //    allegroScan.GetJournalToProcess(new string[] { "now" });
            long[] itemIds =
               allegroScan.GetJournalDealsToProcess(new int[] {
                    1 // zakup
                }).Select(x => x.DealItemId).Distinct().ToArray();

            // nowe akty zakupowe dla danej oferty
            //long[] itemIds = allegroScan.GetJournalDealsNewItems();
            foreach (long itemId in itemIds)
            {
                AddNewOrder(itemId);
            }
        }


        public void ShopTest()
        { 
            Bll.ShopHelper sh = new Bll.ShopHelper(); 
            sh.SetRecommendedProducts(new Dal.ProductCatalogView()
            {
                ProductCatalogId= 16726,
                ShopProductId = 9576
            });
        }

        //private void ProcessDeals()
        //{
        //    List<Dal.AllegroSiteJournalDeal> journalDeals =
        //        allegroScan.GetJournalDealsToProcess(new int[] {
        //            2, // formularz
        //            4 // PayU
        //        });

        //    foreach (Dal.AllegroSiteJournalDeal journalDeal in journalDeals)
        //    {
        //        try
        //        {
        //            bool updateDeal = false;
        //            switch (journalDeal.DealEventType)
        //            {
        //                case 2:
        //                    updateDeal = GetTransactionForms(journalDeal);
        //                    break;
        //                case 4:
        //                    updateDeal = GetPayment(journalDeal);
        //                    break;
        //            }
        //            if (updateDeal)
        //                allegroScan.SetAllegroSiteJournalDealsAsProcessed(journalDeal.DealEventId.ToString());
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorHandler.LogError(ex, String.Format("ItemId: {0}, TransactionId: {1}",
        //                journalDeal.DealItemId, journalDeal.DealTransactionId));
        //            //ErrorHandler.SendError(ex, String.Format("Błąd w metodzie ProcessDeals, ItemId: {0}, TransactionId: {1}",
        //            //    journalDeal.DealItemId, journalDeal.DealTransactionId));
        //        }
        //    }
        //}

        //private void ProcessMyAuctions()
        //{
        //    ProcessAllegroItems(allegroScan.GetMyStartJournal());
        //   // ProcessAllegroItemOrders(allegroScan.GetMyNowJournal(), true);
        //}
        //private void ProcessOther()
        //{
        //    ProcessAuctions2(allegroScan.GetOtherJournalToProcess());
        //}
        //internal void ProcessAuctions2(List<Dal.AllegroSiteJournal> journals)
        //{
        //}

        //private void ProcessAllegroItemOrders(List<Dal.AllegroSiteJournal> journals, bool isMyItem)
        //{

        //    long[] bidItemIds = journals.Select(x => x.ItemId)
        //        .Distinct()
        //        .ToArray();


        //    //foreach (long itemId in bidItemIds)
        //    //{
        //    //    SetAllegroItemOrder(journals.Where(x => x.ItemId == itemId).ToList(), itemId);

        //    //}

        //    long[] partItemIds = bidItemIds.Take(itemsCount).ToArray();

        //    while (partItemIds.Length > 0)
        //    {

        //        SetAllegroItemOrder(journals.Where(x => partItemIds.Contains(x.ItemId)).ToList(), partItemIds, isMyItem);
        //        //AddOrUpdateAuctions(bidItemIds.Where(x => partItemIds.Contains(x.ItemId)).ToList(), partItemIds);

        //        bidItemIds = bidItemIds.Skip(itemsCount).ToArray();
        //        partItemIds = bidItemIds.Take(itemsCount).ToArray();
        //    }



        //}


        //private void ProcessAllegroItems(List<Dal.AllegroSiteJournal> journals, bool isMyItem)
        //{

        //    long[] updateItemIds = journals.Select(x => x.ItemId)
        //        .Distinct()
        //        .ToArray();

        //    long[] partItemIds = updateItemIds.Take(itemsCount).ToArray();

        //    while (partItemIds.Length > 0)
        //    {

        //        AddOrUpdateAuctions(journals.Where(x => partItemIds.Contains(x.ItemId)).ToList(), partItemIds, isMyItem);

        //        updateItemIds = updateItemIds.Skip(itemsCount).ToArray();
        //        partItemIds = updateItemIds.Take(itemsCount).ToArray();
        //    }
        //}

        public void CheckImagesSize()
        {
            string path = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];
            Bll.Helper.UpdateProductCatalogImages(path);
        }

        public void ImageThumbsCreate()
        {
            string path = ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())];
            Bll.Helper.ProductCatalogThumbsImage(path);
        }
        //private void ProcessAllegroItems(List<Dal.AllegroSiteJournal> journals)
        //{

        //    List<Dal.AllegroSiteJournal> updateJournals = journals.Where(x =>
        //        x.ChangeType == "start"
        //        || x.ChangeType == "change"
        //         //|| x.ChangeType == "end"
        //         || x.ChangeType == "bid"
        //        ).ToList();


        //    long[] updateItemIds = journals.Select(x => x.ItemId)
        //        .Distinct()
        //        .ToArray();

       
        //    long[] partItemIds = updateItemIds.Take(itemsCount).ToArray();

        //    while (partItemIds.Length > 0)
        //    {

        //        AddOrUpdateAuctions(journals.Where(x => partItemIds.Contains(x.ItemId)).ToList(), partItemIds);

        //        updateItemIds = updateItemIds.Skip(itemsCount).ToArray();
        //        partItemIds = updateItemIds.Take(itemsCount).ToArray();
        //    }
        //}
        /*
        internal void ProcessAuctions(List<Dal.AllegroSiteJournal> journals)
        {


            List<Dal.AllegroSiteJournal> updateJournals = journals.Where(x =>
                x.ChangeType == "start"
                || x.ChangeType == "change"
                || x.ChangeType == "end"
                ).ToList();


            long[] updateItemIds = updateJournals.Select(x => x.ItemId)
                .Distinct()
                .ToArray(); 
            foreach (long itemId in updateItemIds)
            {
                AddOrUpdateAuction(updateJournals.Where(x => x.ItemId == itemId).ToList(), itemId);

            }


            List<Dal.AllegroSiteJournal> bidJournals = journals.Where(x =>
                x.ChangeType == "bid"
                || x.ChangeType == "now"
                ).ToList();


            long[] bidItemIds = bidJournals.Select(x => x.ItemId)
                .Distinct()
                .ToArray();


            foreach (long itemId in bidItemIds)
            {
                SetAllegroItemOrder(bidJournals.Where(x => x.ItemId == itemId).ToList(), itemId);

            } 
        }
        */
        //private void SetAllegroItemOrder(List<Dal.AllegroSiteJournal> journals, long[] itemIds, bool isMyItem)
        //{
        //    List<Dal.AllegroItem> allegroItemsInSystem = allegroScan.GetItems(itemIds);
        //    List<Dal.AllegroItem> allegroItemsToAdd =null;
        //    long[] notFound;
        //    long[] killed;

        //    long[] itemsNotInSystem = itemIds.Where(x => !allegroItemsInSystem.Select(y => y.ItemId).ToArray().Contains(x)).ToArray();

        //    if (itemsNotInSystem.Length>0)  // jeśli nie ma aukcji to ją dodaj
        //    {
        //        allegroItemsToAdd = AddOrUpdateAuctions(itemsNotInSystem, Dal.Helper.MyUsers.JacekStawicki.ToString(), out notFound, out killed);
        //        if (allegroItemsToAdd != null)
        //            allegroScan.AddAuctions(allegroItemsToAdd);
        //    }
        //    if (allegroItemsToAdd == null)
        //        allegroItemsToAdd = new List<Dal.AllegroItem>();

        //    long[] itemsToRefresh =
        //        allegroItemsInSystem.Select(x=>x.ItemId).ToArray()
        //        .Union(allegroItemsToAdd.Select(y=>y.ItemId).ToArray())
        //        .ToArray();

        //        RefreshItemOrders(
        //            Dal.Helper.MyUsers.JacekStawicki.ToString(),
        //            itemsToRefresh,
        //            String.Join(",", journals.Select(x => x.RowId.ToString()).ToArray()),
        //            isMyItem);
        //   // else
        //   //     allegroScan.SetAllegroSiteJournalAsProcessed(journals);
        //}

        private bool GetPayment(Dal.AllegroSiteJournalDeal journalDeal)
        {
            return GetPayments(journalDeal.AllegroUser.UserName,  journalDeal.DealItemId, journalDeal.DealBuyerId);
        }

        private bool GetTransactionForms(Dal.AllegroSiteJournalDeal journalDeal)
        {
            AllegroHelper ah = new AllegroHelper();
            List<AllegroNewWCF.PostBuyFormDataStruct> buys = ah.GetItemTransactionsForTransactionsIDs(journalDeal.AllegroUser.UserName,
                new long[] { journalDeal.DealTransactionId });


            foreach (AllegroNewWCF.PostBuyFormDataStruct buy in buys)
            {
                ProcessBuy(buy);
            }

            return buys.Count > 0;
        }

        private void AddNewOrder(long itemId)
        {

            Dal.AllegroScan ah = new Dal.AllegroScan();

            List<Dal.AllegroSiteJournalDeal> journalDeals = ah.GetJournalNewSoldItems(itemId);

            if (journalDeals.Count == 0)
                return;

            string userName = journalDeals[0].AllegroUser.UserName;

            // utwórz nowe obiekty AllegroItemOrder dla każdego aktu zakupowego.
            List<Dal.AllegroItemOrder> allegroItemOrders = GetItemOrders(userName, itemId, false);
            List<Dal.AllegroItemOrder> itemOrdersToAdd = new List<Dal.AllegroItemOrder>();



            List<Dal.AllegroItemOrder> allegroItemOrdersToBeAdded = new List<Dal.AllegroItemOrder>();
            List<Dal.AllegroSiteJournalDeal> dealsProcessed = new List<Dal.AllegroSiteJournalDeal>();

            if (allegroItemOrders != null)
            {
                #region loop
                foreach (Dal.AllegroSiteJournalDeal journalDeal in journalDeals)
                {

                    // wybierz obiekt dla kupującego
                    Dal.AllegroItemOrder aio = new Dal.AllegroItemOrder(); 
                    
                    
                    Dal.AllegroItemOrder aioOriginal = allegroItemOrders
                        .Where(x => x.ItemId == journalDeal.DealItemId
                           && x.UserId == journalDeal.DealBuyerId)
                        .FirstOrDefault();

                    if (aio == null)
                    {
                        ErrorHandler.SendError(new Exception("Błąd w metodzie AddOrder(Dal.AllegroSiteJournal journal)"),
                            String.Format(@"Obiekt 'Dal.AllegroItemOrder aio' jest pusty. DealEventId: {0}
                        , ItemId: {1}, DealBuyerId: {2}", journalDeal.DealEventId, journalDeal.DealItemId, journalDeal.DealBuyerId));
                        continue;

                    }
                    // uzupełnij obiekt aio
                    Dal.AllegroItemOrder itemOrderFromBuyerData = GetAllegroItemOrderFromBuyersData(userName, journalDeal.DealItemId, journalDeal.DealBuyerId);

                    if (itemOrderFromBuyerData == null)
                    {
                        ErrorHandler.SendError(new Exception("Błąd w metodzie AddOrder(Dal.AllegroSiteJournal journal)"),
                            String.Format(@"Obiekt itemOrderFromBuyerData jest pusty. DealEventId: {0}
 , ItemId: {1}, DealBuyerId: {2}", journalDeal.DealEventId, journalDeal.DealItemId, journalDeal.DealBuyerId));
                        continue;
                    }

                    #region



                    aio.ItemId = aioOriginal.ItemId;
                    aio.ItemPrice = aioOriginal.ItemPrice;
                    aio.ItemsOrdered = aioOriginal.ItemsOrdered;
                    aio.OrderDate = aioOriginal.OrderDate;
                    aio.OrderStatus = aioOriginal.OrderStatus;
                    aio.UserId = aioOriginal.UserId;
                    aio.UserName = aioOriginal.UserName;
                    aio.UserPointCount = aioOriginal.UserPointCount;
                    aio.OrderStatusId = aioOriginal.OrderStatusId;
                    aio.LastUpdateDate = aioOriginal.LastUpdateDate;



                    // take quantity fromn deal
                    aio.ItemsOrdered = journalDeal.DealQuantity;


                    aio.Address = itemOrderFromBuyerData.Address;
                    aio.AllegroStandard = itemOrderFromBuyerData.AllegroStandard;
                    aio.City = itemOrderFromBuyerData.City;
                    aio.CompanyIcon = itemOrderFromBuyerData.CompanyIcon;
                    aio.CompanyName = itemOrderFromBuyerData.CompanyName;
                    aio.CountryId = itemOrderFromBuyerData.CountryId;
                    aio.Email = itemOrderFromBuyerData.Email;
                    aio.FirstName = itemOrderFromBuyerData.FirstName;
                    aio.HasShop = itemOrderFromBuyerData.HasShop;
                    aio.Junior = itemOrderFromBuyerData.Junior;
                    aio.LastName = itemOrderFromBuyerData.LastName;
                    aio.Phone = itemOrderFromBuyerData.Phone;
                    aio.Phone2 = itemOrderFromBuyerData.Phone2;
                    aio.Postcode = itemOrderFromBuyerData.Postcode;
                    aio.RegistrationCountryId = itemOrderFromBuyerData.RegistrationCountryId;
                    aio.ShipmentAddress = itemOrderFromBuyerData.ShipmentAddress;
                    aio.ShipmentCity = itemOrderFromBuyerData.ShipmentCity;
                    aio.ShipmentCompanyName = itemOrderFromBuyerData.ShipmentCompanyName;
                    aio.ShipmentCountryId = itemOrderFromBuyerData.ShipmentCountryId;
                    aio.ShipmentFirstName = itemOrderFromBuyerData.ShipmentFirstName;
                    aio.ShipmentLastName = itemOrderFromBuyerData.ShipmentLastName;
                    aio.ShipmentPostcode = itemOrderFromBuyerData.ShipmentPostcode;
                    aio.StateId = itemOrderFromBuyerData.StateId;
                     

                    aio.LastUpdateDate = DateTime.Now;
                    aio.OrderCreated = false;
                    aio.OrderStatusId = 1;

                    #endregion

                    allegroItemOrdersToBeAdded.Add(aio);
                    dealsProcessed.Add(journalDeal);
                }
                #endregion
            }
            allegroScan.SetAllegroItemOrders(allegroItemOrdersToBeAdded, dealsProcessed);
            //AddOrUpdateAuction(journal);

            // po co to?

            //Dal.AllegroItem ai = AddOrUpdateAuction(itemId, userName);
            //allegroScan.AddOrUpdateAuction( ai);
    

        }

        //        private Dal.AllegroItemOrder NewAllegroItemOrderFromJournalDeal(Dal.AllegroSiteJournalDeal journalDeal)
        //        { 

        //                LajtIt.Dal.AllegroItemOrder order = 
        //                    .Where(x => x.UserId == item.DealBuyerId).FirstOrDefault();

        //                            if (order == null)
        //                            {
        //                                ErrorHandler.SendError(new Exception("Błąd w metodzie ProcessMyJournal"), String.Format(@"Obiekt order jest pusty. DealEventId: {0}
        //            , ItemId: {1}, DealBuyerId: {2}", item.DealEventId, item.DealItemId, item.DealBuyerId));
        //                                continue;
        //                            }



        //                            dealsProcessed.Add(item);
        //                            itemOrders.Add(order);

        //                        }
        //                        allegroScan.SetAllegroItemOrders(itemOrders, dealsProcessed);
        //        }
        /*
        private void AddOrUpdateAuction(List<Dal.AllegroSiteJournal> journals, long itemId)
        {
            string userName = Dal.Helper.MyUsers.JacekStawicki.ToString(); // journal.AllegroUser.UserName;

            Dal.AllegroItem ai = null;
            try
            {
                ai = AddOrUpdateAuction(itemId, userName);
                allegroScan.AddOrUpdateAuction(journals, ai);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Wskazana oferta została usunięta przez administratora serwisu.")
                    allegroScan.AddOrUpdateAuctionWhenDeleted(journals);

                ErrorHandler.LogError(ex, String.Format("AddOrUpdateAuction {0} {1}", ex.Message, itemId));
                ErrorHandler.SendError(ex, String.Format("AddOrUpdateAuction {0} {1}", ex.Message, itemId)); 
            }
        }
        */

        //public Dal.AllegroItem AddOrUpdateAuction(long itemId, string userName)
        //{
        //    int? categoryId;
        //    string imageUrl;
        //    AllegroHelper ah = new AllegroHelper();

        //    AllegroNewWCF.ItemInfoExt item = ah.GetItem(itemId, userName, out categoryId, out imageUrl);

        //    Dal.AllegroUser au = allegroScan.GetAllegroUser(item.itSellerId);

        //    Dal.AllegroItem ai = new Dal.AllegroItem()
        //    {
        //        AllegroStandard = item.itIsAllegroStandard,
        //        BidCount = item.itBidCount,
        //        BuyNowPrice = (decimal)item.itBuyNowPrice,
        //        CategoryId = categoryId,
        //        CurrentPrice = (decimal)item.itPrice,
        //        EndingInfo = item.itEndingInfo,
        //        EndingTime = item.itEndingTime,
        //        FotoCount = item.itFotoCount,
        //        HitCount = item.itHitCount,
        //        ImageUrl = imageUrl,
        //        InsertDate = DateTime.Now,
        //        ItemId = item.itId,
        //        LastUpdateDate = DateTime.Now,
        //        Name = item.itName,
        //        Options = item.itOptions,
        //        Quantity = item.itQuantity,
        //        UserId = item.itSellerId,
        //        StartingQuantity = item.itStartingQuantity,
        //        ReservePrice = (decimal)item.itReservePrice,
        //        Price = (decimal)item.itPrice,
        //        BuyNowActive = item.itBuyNowActive,
        //        HightBidderId = item.itHighBidder,
        //        HightBidderLogin = item.itHighBidderLogin

        //    };

        //    if (au == null)
        //    {
        //        au = new Dal.AllegroUser()
        //        {
        //            Follow = false,
        //            UserId = item.itSellerId,
        //            UserName = item.itSellerLogin
        //        };
        //        ai.AllegroUser = au;
        //    }

        //    if (ai.EndingInfo == 2)
        //        ai.FinalScan = true;
        //    return ai;
        //}

        //private void AddOrUpdateAuctions(List<Dal.AllegroSiteJournal> journals, long[] itemIds)
        //{
        //    string userName = Dal.Helper.MyUsers.JacekStawicki.ToString(); // journal.AllegroUser.UserName;
        //    long[] notFound;
        //    long[] killed;

        //    List<Dal.AllegroItem> allegroItems = null;
        //    try
        //    {
        //        allegroItems = AddOrUpdateAuctions(itemIds, userName, out notFound, out killed);
        //        allegroScan.AddOrUpdateAuctions(journals, allegroItems);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Wskazana oferta została usunięta przez administratora serwisu.")
        //            allegroScan.AddOrUpdateAuctionWhenDeleted(journals);


        //        if (ex.Message == "Błędny identyfikator sesji. Proszę spróbować zalogować się jeszcze raz!")
        //        {
        //        }

        //        ErrorHandler.LogError(ex, String.Format("AddOrUpdateAuctions {0} {1}", ex.Message, String.Join(",", itemIds)));
        //        System.Environment.Exit(1);
        //    }
        //}
        //private void AddOrUpdateAuctions(List<Dal.AllegroSiteJournal> journals, long[] itemIds, bool isMyItem)
        //{
        //    string userName = Dal.Helper.MyUsers.JacekStawicki.ToString(); // journal.AllegroUser.UserName;
        //    long[] notFound;
        //    long[] killed;

        //    List<Dal.AllegroItem> allegroItems = null;
        //    try
        //    {
        //        allegroItems = AddOrUpdateAuctions(itemIds, userName, out notFound, out killed);
        //        allegroScan.AddOrUpdateAuctions(journals, allegroItems);

        //        RefreshItemOrders(
        //            Dal.Helper.MyUsers.JacekStawicki.ToString(),
        //            itemIds,
        //            null,
        //            isMyItem);

        //        allegroScan.SetAllegroSiteJournalAsProcessed(String.Join(",", journals.Select(x => x.RowId.ToString()).ToArray()));

        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Wskazana oferta została usunięta przez administratora serwisu.")
        //            allegroScan.AddOrUpdateAuctionWhenDeleted(journals);


        //        if (ex.Message == "Błędny identyfikator sesji. Proszę spróbować zalogować się jeszcze raz!")
        //        {
        //        }

        //        ErrorHandler.LogError(ex, String.Format("AddOrUpdateAuctions {0} {1}", ex.Message, String.Join(",", itemIds)));
        //        System.Environment.Exit(1);
        //    }
        //}

        //public List<Dal.AllegroItem> AddOrUpdateAuctions(long[] itemIds, string userName, out long[] notFound, out long[] killed)
        //{
        //    AllegroHelper ah = new AllegroHelper();

        //    List<Dal.AllegroItem> allegroItems = new List<Dal.AllegroItem>();

        //    AllegroNewWCF.ItemInfoStruct[] items = ah.GetItems(itemIds, userName, out notFound, out killed);

        //    long[] userIDs = items.Select(x => x.itemInfo.itSellerId).ToArray();

        //    List<Dal.AllegroUser> users = allegroScan.GetAllegroUsers(userIDs);

        //    #region foreach (AllegroNewWCF.ItemInfoStruct item in items)
        //    foreach (AllegroNewWCF.ItemInfoStruct item in items)
        //    {

        //        Dal.AllegroItem ai = new Dal.AllegroItem()
        //        {
        //            AllegroStandard = item.itemInfo.itIsAllegroStandard,
        //            BidCount = item.itemInfo.itBidCount,
        //            BuyNowPrice = (decimal)item.itemInfo.itBuyNowPrice,
        //            CategoryId = GetCategory(item.itemCats),
        //            CurrentPrice = (decimal)item.itemInfo.itPrice,
        //            EndingInfo = item.itemInfo.itEndingInfo,
        //            EndingTime = item.itemInfo.itEndingTime,
        //            FotoCount = item.itemInfo.itFotoCount,
        //            HitCount = item.itemInfo.itHitCount,
        //            ImageUrl = GetImage(item.itemImages),
        //            InsertDate = DateTime.Now,
        //            ItemId = item.itemInfo.itId,
        //            LastUpdateDate = DateTime.Now,
        //            Name = item.itemInfo.itName,
        //            Options = item.itemInfo.itOptions,
        //            Quantity = item.itemInfo.itQuantity,
        //            StartingQuantity = item.itemInfo.itStartingQuantity,
        //            ReservePrice = (decimal)item.itemInfo.itReservePrice,
        //            Price = (decimal)item.itemInfo.itPrice,
        //            BuyNowActive = item.itemInfo.itBuyNowActive,
        //            HightBidderId = item.itemInfo.itHighBidder,
        //            HightBidderLogin = item.itemInfo.itHighBidderLogin,
        //            SellingMode = item.

        //        };

        //        Dal.AllegroUser au = users.Where(x => x.UserId == item.itemInfo.itSellerId).FirstOrDefault();

        //        if (au == null)
        //        {
        //            ai.AllegroUser = new Dal.AllegroUser()
        //            {
        //                Follow = false,
        //                UserId = item.itemInfo.itSellerId,
        //                UserName = item.itemInfo.itSellerLogin
        //            };
        //            users.Add(ai.AllegroUser);
        //        }
        //        else
        //            ai.UserId = item.itemInfo.itSellerId;

        //        if (ai.EndingInfo == 2)
        //            ai.FinalScan = true;


        //        //foreach(long killedItemId in killed)
        //        //{
        //        //    ai = new Dal.AllegroItem()
        //        //    { 
        //        //        EndingInfo = 3,
        //        //        EndingTime = DateTime.Now.Ticks,
        //        //        FinalScan = false

        //        //    };

        //        //}

        //        allegroItems.Add(ai);
        //    }
        //    #endregion

        //    return allegroItems;
        //}

        private string GetImage(AllegroNewWCF.ItemImageList[] image)
        {
            string imageUrl = null;
            AllegroNewWCF.ItemImageList i = image.Where(x => x.imageType == 1).FirstOrDefault();
            if (i != null)
                imageUrl = i.imageUrl;

            return imageUrl;
        }

        private int? GetCategory(AllegroNewWCF.ItemCatList[] cat)
        {
            int? categoryId = null;
            if (cat.Count() != 0)
                categoryId = (int)cat.Skip(cat.Count() - 1).FirstOrDefault().catId;

            return categoryId;

        }

        /*   private void Tmp()
           {


               /////  --------- 1 --------- /////
               /// Pobiera aukcje użytkownika
               GetUserAuctions(new Dal.AllegroUserTrackList() { UserId = (int)Dal.Helper.MyID });
               GetMyAuctions(Dal.Helper.MyID);



               List<Dal.AllegroSiteJournalDeal> newSoldItems =
                   allegroScan.GetJournalNewSoldItems();
               List<Dal.AllegroItemOrder> itemOrders = new List<Dal.AllegroItemOrder>();
               List<Dal.AllegroSiteJournalDeal> dealsProcessed = new List<Dal.AllegroSiteJournalDeal>();


               foreach (Dal.AllegroSiteJournalDeal item in newSoldItems)
               {

                   LajtIt.Dal.AllegroItemOrder order = GetItemOrders(item.DealItemId)
                       .Where(x => x.UserId == item.DealBuyerId).FirstOrDefault();

                   if (order == null)
                   {
                       ErrorHandler.SendError(new Exception("Błąd w metodzie ProcessMyJournal"), String.Format(@"Obiekt order jest pusty. DealEventId: {0}
   , ItemId: {1}, DealBuyerId: {2}", item.DealEventId, item.DealItemId, item.DealBuyerId));
                       continue;
                   }

                   Dal.AllegroItemOrder itemOrderFromBuyerData = GetAllegroItemOrderFromBuyersData(item.DealItemId, item.DealBuyerId);

                   if (itemOrderFromBuyerData == null)
                   {
                       ErrorHandler.SendError(new Exception("Błąd w metodzie ProcessMyJournal"), String.Format(@"Obiekt itemOrderFromBuyerData jest pusty. DealEventId: {0}
   , ItemId: {1}, DealBuyerId: {2}", item.DealEventId, item.DealItemId, item.DealBuyerId));
                       continue;
                   }

                   #region
                   order.Address = itemOrderFromBuyerData.Address;
                   order.AllegroStandard = itemOrderFromBuyerData.AllegroStandard;
                   order.City = itemOrderFromBuyerData.City;
                   order.CompanyIcon = itemOrderFromBuyerData.CompanyIcon;
                   order.CompanyName = itemOrderFromBuyerData.CompanyName;
                   order.CountryId = itemOrderFromBuyerData.CountryId;
                   order.Email = itemOrderFromBuyerData.Email;
                   order.FirstName = itemOrderFromBuyerData.FirstName;
                   order.HasShop = itemOrderFromBuyerData.HasShop;
                   order.Junior = itemOrderFromBuyerData.Junior;
                   order.LastName = itemOrderFromBuyerData.LastName;
                   order.Phone = itemOrderFromBuyerData.Phone;
                   order.Phone2 = itemOrderFromBuyerData.Phone2;
                   order.Postcode = itemOrderFromBuyerData.Postcode;
                   order.RegistrationCountryId = itemOrderFromBuyerData.RegistrationCountryId;
                   order.ShipmentAddress = itemOrderFromBuyerData.ShipmentAddress;
                   order.ShipmentCity = itemOrderFromBuyerData.ShipmentCity;
                   order.ShipmentCompanyName = itemOrderFromBuyerData.ShipmentCompanyName;
                   order.ShipmentCountryId = itemOrderFromBuyerData.ShipmentCountryId;
                   order.ShipmentFirstName = itemOrderFromBuyerData.ShipmentFirstName;
                   order.ShipmentLastName = itemOrderFromBuyerData.ShipmentLastName;
                   order.ShipmentPostcode = itemOrderFromBuyerData.ShipmentPostcode;
                   order.StateId = itemOrderFromBuyerData.StateId;


                   order.LastUpdateDate = DateTime.Now;
                   order.OrderCreated = false;
                   order.OrderStatusId = 1;
                   #endregion

                   dealsProcessed.Add(item);
                   itemOrders.Add(order);

               }
               allegroScan.SetAllegroItemOrders(itemOrders, dealsProcessed);




               /////  --------- 2 --------- /////
               /// Odśwież oferty w aukcjach

               //long[] itemIDs = //new long[] { 2721565957 };
               //    /// TODO ! uncomment this when done!
               //allegroScan.GetItemsToRefresh(Dal.Helper.MyID).Select(x => x.ItemId).ToArray();

               //RefreshItemOrders(itemIDs, false, true);

               /////  --------- 3 --------- /////
               /// Pobieranie formularzy dostawy

               /// TODO ! uncomment this when done!
               long[] itemIDs = allegroScan.GetMyItemIDs();

               ProcessItemTransactions(itemIDs);


               /////  --------- 4 --------- /////
               /// Tworzenie zamówień w systemie
               CreateOrders();

               /////  --------- 5 --------- /////
               /// Pobierania płatności
               GetPayments();

               RefreshUserLastUpdate((int)Dal.Helper.MyID);
           }
         * 
         * */
    }
}
