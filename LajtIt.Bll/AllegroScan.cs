using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LajtIt.Bll.Dto;
using System.Threading.Tasks;

namespace LajtIt.Bll
{
    public partial class AllegroScan
    {
        private LajtIt.Dal.AllegroScan allegroScan;

        public AllegroScan()
        {
            allegroScan = new LajtIt.Dal.AllegroScan();
        }

      

        //internal void SetAuction(string userName, long itemId)
        //{
        //    AllegroHelper ah = new AllegroHelper();
        //    AllegroNewWCF.ItemInfoStruct[] iis = ah.GetAuctions(userName, new long[] { itemId });

        //    List<Dal.AllegroItem> allegroItems = GetAllegroItemFromItmInfoStruct(iis);

        //    Dal.AllegroScan allegroScan = new Dal.AllegroScan();
        //    allegroScan.SetUserItems(allegroItems);
        //}

        //private List<Dal.AllegroItem> GetAllegroItemFromItmInfoStruct(AllegroNewWCF.ItemInfoStruct[] itemInfoStruct)
        //{
        //    List<Dal.AllegroItem> allegroItems = new List<Dal.AllegroItem>();

        //    if (itemInfoStruct == null)
        //        return allegroItems;

        //    foreach (AllegroNewWCF.ItemInfoStruct item in itemInfoStruct)
        //    {
        //        try
        //        {
        //            string imageUrl = null;
        //            AllegroNewWCF.ItemImageList image = item.itemImages.Where(x => x.imageType == 1).FirstOrDefault();
        //            if (image != null)
        //                imageUrl = image.imageUrl;

        //            int? categoryId = null;
        //            if (item.itemCats.Count() != 0)
        //                categoryId = (int)item.itemCats.Skip(item.itemCats.Count() - 1).FirstOrDefault().catId;

        //            allegroItems.Add(
        //                   new LajtIt.Dal.AllegroItem()
        //                   {
        //                       BidCount = item.itemInfo.itBidCount,
        //                       BuyNowPrice = (decimal)item.itemInfo.itBuyNowPrice,
        //                       CurrentPrice = (decimal)item.itemInfo.itPrice,
        //                       EndingInfo = item.itemInfo.itEndingInfo,
        //                       EndingTime = item.itemInfo.itEndingTime,
        //                       AllegroStandard = item.itemInfo.itIsAllegroStandard,
        //                       Name = item.itemInfo.itName,
        //                       UserId = item.itemInfo.itSellerId,
        //                       ItemId = item.itemInfo.itId,
        //                       FotoCount = item.itemInfo.itFotoCount,
        //                       CategoryId = categoryId,
        //                       HitCount = item.itemInfo.itHitCount,
        //                       LastUpdateDate = DateTime.Now,
        //                       Quantity = item.itemInfo.itQuantity,
        //                       ImageUrl = imageUrl,
        //                       Options = item.itemInfo.itOptions,
        //                       StartingQuantity = item.itemInfo.itStartingQuantity,
        //                       InsertDate = DateTime.Now
        //                   }
        //                   );
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorHandler.LogError(ex, String.Format("GetAllegroItemFromItmInfoStruct, IteimId: {0}", item.itemInfo.itId));
        //            throw ex;
        //        }
        //    }
        //    return allegroItems;
        //}

        //public long[] RefreshAuctions(string userName, long[] itemsId)
        //{

        //    AllegroHelper ah = new AllegroHelper();
        //    int partsCount = (itemsId.Length / 25);

        //    for (int i = 0; i < partsCount + 1; i++)
        //    {
        //        long[] itemsIdPart = itemsId.Skip(i * 25).Take(25).ToArray();

        //        if (itemsIdPart.Length == 0)
        //            continue;
        //        List<AllegroNewWCF.ItemInfoStruct> items = new List<AllegroNewWCF.ItemInfoStruct>();

        //        AllegroNewWCF.ItemInfoStruct[] iis = ah.GetAuctions(userName, itemsIdPart);

        //        if (iis != null && iis.Length > 0)
        //            ProcessAuctions(iis);


        //    }

        //    return itemsId;
        //}

        /// <summary>
        /// Odświeża informacje o przedmiotach
        /// </summary>
        /// <returns></returns>
        //public long[] RefreshAuctions()
        //{
        //    Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.JacekStawicki.ToString());
        //    long[] itemsId = allegroScan.GetItemIdsToRefresh(10000);//.Take(10000).ToArray();

        //    return RefreshAuctions(Dal.Helper.MyUsers.JacekStawicki.ToString(), itemsId);
        //}

        //private void ProcessAuctions(AllegroNewWCF.ItemInfoStruct[] itemInfoStruct)
        //{
        //    if (itemInfoStruct == null)
        //        return;


        //    List<LajtIt.Dal.AllegroItem> allegroItems = GetAllegroItemFromItmInfoStruct(itemInfoStruct); // new List<LajtIt.Dal.AllegroItem>();


        //    allegroScan.UpdateItems(allegroItems);
        //}

        /// <summary>
        /// Pobiera aukcje użytkownika oraz tworzy jego jeśli nie istniał do tej pory
        /// </summary>
        /// <param name="user"></param>
        //private void GetUserAuctions(string userName, LajtIt.Dal.AllegroUserTrackList user)
        //{
        //    AllegroHelper ah = new AllegroHelper();
        //    List<AllegroNewWCF.ItemsListType> items = ah.GetUserAuctions(user.UserId,  userName);

        //    //AllegroNewWCF.UserItemList i = items.Where(x => x.itid == 2875425138).FirstOrDefault();

        //    List<LajtIt.Dal.AllegroItem> allegroItems = new List<LajtIt.Dal.AllegroItem>();

        //    foreach (AllegroNewWCF.ItemsListType item in items)
        //    {
        //        allegroItems.Add(
        //            new LajtIt.Dal.AllegroItem()
        //            {
        //                BidCount = item.bidsCount,
        //                BuyNowPrice = (decimal)item.priceInfo[0].priceValue,
        //                CurrentPrice = (decimal)item.priceInfo[0].priceValue,
        //                Name = item.itemTitle,
        //                UserId = user.UserId,
        //                ItemId = item.itemId,
        //                //CategoryId = (int)ah.GetItemCategory(item.itid),
        //                InsertDate = DateTime.Now
        //            }
        //            );

        //    }

        //    Dal.AllegroUser allegroUser = allegroScan.SetUser(new LajtIt.Dal.AllegroUser()
        //      {
        //          UserId = user.UserId,
        //          UserName = user.UserName
        //      });

        //    List<Dal.AllegroItem> insertedItems = allegroScan.SetUserItems(allegroItems);

        //    NotifyAboutNewItems(allegroUser, insertedItems);

        //    long[] itemIDs =
        //        allegroScan.GetItemsToRefresh(user.UserId);
                 
        //        //.Select(x => x.ItemId)
        //       // .ToArray();
        //    RefreshAuctions(userName, itemIDs);


        //}

        private void NotifyAboutNewItems(Dal.AllegroUser allegroUser, List<Dal.AllegroItem> insertedItems)
        {
            Bll.EmailEditor ee = new EmailEditor();
            ee.NotifyAboutNewItems(allegroUser, insertedItems);
        }


        //public void GetItemFields()
        //{
        //    Bll.AllegroHelper.GetVersionKeys();//(Dal.Helper.MyUsers.JacekStawicki.ToString());
        //    AllegroHelper ah = new AllegroHelper();
        //    ah.GetItemFields();

        //}
        /// <summary>
        /// Pobiera i aktualizuje listę kategorii
        /// </summary>
        public void GetCategories()
        {
            Bll.AllegroHelper.GetVersionKeys();//(Dal.Helper.MyUsers.JacekStawicki.ToString());

            AllegroHelper ah = new AllegroHelper();
            AllegroNewWCF.CatInfoType[] cats = ah.GetCategories();

            allegroScan.SetCategories(cats.Select(x =>
                new LajtIt.Dal.AllegroCategory()
                {
                    CategoryId = x.catId,
                    Name = x.catName,
                    CategoryParentId = x.catParent
                }
                ).ToList());


            //// pobierz kategorie w sklepie
            //cats = ah.GetShopCategories(Dal.Helper.MyUsers.JacekStawicki.ToString());

            //allegroScan.SetShopCategories(Dal.Helper.MyUsers.JacekStawicki, cats.Select(x =>
            //    new LajtIt.Dal.AllegroShopCategory()
            //    {
            //        CatId = x.catId,
            //        Name = x.catName,
            //        CatParentId = x.catParent,
            //        CatPosition = x.catParent,
            //        IsProductCatalogEnabled = x.catIsProductCatalogueEnabled,
            //        IsDeleted = false,
            //        UserId = (int)Dal.Helper.MyUsers.JacekStawicki
            //    }
            //    ).ToList());




            //// pobierz kategorie w sklepie


            //Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.CzerwoneJablko.ToString());
            //cats = ah.GetShopCategories(Dal.Helper.MyUsers.CzerwoneJablko.ToString());

            //allegroScan.SetShopCategories(Dal.Helper.MyUsers.CzerwoneJablko, cats.Select(x =>
            //    new LajtIt.Dal.AllegroShopCategory()
            //    {
            //        CatId = x.catId,
            //        Name = x.catName,
            //        CatParentId = x.catParent,
            //        CatPosition = x.catParent,
            //        IsProductCatalogEnabled = x.catIsProductCatalogueEnabled,
            //        IsDeleted=false,
            //        UserId = (int)Dal.Helper.MyUsers.CzerwoneJablko
            //    }
            //    ).ToList());

        }

        public List<Bll.Dto.UserAuctions> GetAuctionsByDate(long userId, int year, int month, int day )
        {
            var o = allegroScan.GetAuctionsByDate(userId, year, month, day);

            return
                o.Select(x => new Bll.Dto.UserAuctions()
                {
                    ItemId = x.ItemId,
                    BidCount = x.BidCount,
                    BuyNowPrice = (decimal)x.BuyNowPrice,
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    CurrentPrice = (decimal)x.CurrentPrice,
                    EndingInfo = x.EndingInfo,
                    HitCount = x.HitCount,
                    ImageUrl = x.ImageUrl,
                    ItemsOrdered = x.ItemsOrdered,
                    ItemsValue = x.ItemsValue,
                    LastUpdateDate = x.LastUpdateDate,
                    Name = x.Name,
                    UserName = x.UserName,
                    EndingDateTime = x.EndingDateTime,
                    IsPromoted =( x.IsPromoted??false)
                })
                .OrderByDescending(x => x.ItemsValue)
                .ToList();
        }

        public List<Bll.Dto.UserAuctions> GetAuctions(long userId, long auctionType, int sell)
        {

            return allegroScan.GetAuctions(userId, auctionType, sell)
                .Select(x => new Bll.Dto.UserAuctions()
                {
                    ItemId = x.ItemId,
                    BidCount = x.BidCount,
                    BuyNowPrice = (decimal)x.BuyNowPrice,
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    CurrentPrice = (decimal)x.CurrentPrice,
                    EndingInfo = x.EndingInfo,
                    HitCount = x.HitCount,
                    ImageUrl = x.ImageUrl,
                    ItemsOrdered = x.ItemsOrdered,
                    ItemsValue = x.ItemsValue,
                    LastUpdateDate = x.LastUpdateDate,
                    Name = x.Name,
                    UserName = x.UserName,
                    EndingDateTime = x.EndingDateTime,
                    IsPromoted = (x.IsPromoted??false)
                })
                .OrderByDescending(x => x.EndingDateTime)
                .ToList();

            //return items.OrderBy(x => x.EndingTime).ToList();
        }

        public List<LajtIt.Dal.AllegroItemOrder> GetItemOrders(string userName, long itemId, bool isMyItem)
        {
            AllegroHelper ah = new AllegroHelper();

            if (isMyItem)
            {
                Dal.AllegroItemHelper aih = new Dal.AllegroItemHelper();
                Dal.AllegroItem ai = aih.GetItem(itemId);

                userName = ai.AllegroUser.UserName;
            }
            AllegroNewWCF.BidListStruct2[] bids = ah.GetItemOrders(userName, itemId);

            if (bids == null || bids.Length == 0)
                return null;

            List<LajtIt.Dal.AllegroItemOrder> aio = new List<LajtIt.Dal.AllegroItemOrder>();

            foreach (AllegroNewWCF.BidListStruct2 bid in bids)
            {
                aio.Add(GetAllegroItemOrder(bid.bidsArray));
            }

            return aio;
        }
        public void SetAllegroCosts()
        {
            List<Dal.AllegroItem> items = allegroScan.GetMyItemIDs();

            Bll.AllegroHelper.GetVersionKeys();//(Dal.Helper.MyUsers.JacekStawicki.ToString());
            //Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.CzerwoneJablko.ToString());
            foreach (Dal.AllegroItem item in items)
            {
                try
                {
                    List<Dal.AllegroCost> costs = GetAllegroCosts(item.AllegroUser.UserName, item.ItemId);

                    allegroScan.SetAllegroCosts( item.ItemId, costs);
                }
                catch (Exception ex)
                {

                    if (ex.Message.Contains("przeniesiona do archiwum"))
                    {
                        ErrorHandler.LogError(ex, String.Format("Przeniesiono do archwiwum: {0}", item.ItemId));
                        allegroScan.SetMovedToArchive(item.ItemId);
                    }
                    else
                        ErrorHandler.LogError(ex, String.Format("SetAllegroCosts - {0}", item.ItemId));
                }
            }
        }
        internal List<Dal.AllegroCost> GetAllegroCosts(string userName, long itemId)
        {
            AllegroHelper ah = new AllegroHelper();
            AllegroNewWCF.ItemBilling[] billing = null;
            AllegroNewWCF.ItemBilling[] costs = ah.GetAllegroCost(userName, itemId, out billing);

            List<Dal.AllegroCost> ac = new List<Dal.AllegroCost>();

            ac.AddRange(
                costs.Select(x => new Dal.AllegroCost()
                {
                    ItemId = itemId,
                    Description = x.biName,
                    Amount = Convert.ToDecimal(x.biValue.Replace(".", ","))
                }).ToList()
                );
            if (billing != null)

                ac.AddRange(
                    billing.Select(x => new Dal.AllegroCost()
                    {
                        ItemId = itemId,
                        Description = x.biName,
                        Amount = Convert.ToDecimal(x.biValue.Replace(".", ","))
                    }).ToList()
                    );
            return ac;
        }
        private LajtIt.Dal.AllegroItemOrder GetAllegroItemOrder(string[] bid)
        {
            #region Allegro Specification
            /*
                 *
      identyfikator oferty,
    *
      identyfikator użytkownika; pełna wartość tego pola widoczna jest tylko dla sprzedającego w danej ofercie, dla pozostałych w polu tym zwracane jest 0,
    *
      nazwa użytkownika; pełna wartość tego pola widoczna jest tylko dla sprzedającego w danej ofercie, dla pozostałych w polu tym zwracana jest nazwa użytkownika w formie zanonimizowanej (X...Y),
    *
      liczba punktów użytkownika,
    *
      status konta użytkownika (0 - konto aktywne, 1 - konto zablokowane),
    *
      liczba zakupionych przedmiotów w ofercie,
    *
      cena pojedynczego przedmiotu,
    *
      data zakupu (w formacie Unix time),
    *
      status zakupu (-1 - oferta odwołana, 0 - oferta nie zakończona sprzedażą, 1 - oferta zakończona sprzedażą),
    *
      data odwołania oferty (w formacie Unix time),
    *
      powód odwołania oferty,
    *
      status odwołania oferty (0 - oferta nieodwołana, 1 - oferta odwołana przez sprzedającego, 2 - oferta odwołana przez administratora serwisu).

             */
            #endregion
            LajtIt.Dal.AllegroItemOrder aio = new LajtIt.Dal.AllegroItemOrder();


            aio.ItemId = Convert.ToInt64(bid[0]);
            aio.ItemPrice = Convert.ToDecimal(bid[6].ToString(), new System.Globalization.CultureInfo("en-us"));
            aio.ItemsOrdered = Convert.ToInt32(bid[5]);
            aio.OrderDate = Convert.ToInt64(bid[7]);
            aio.OrderStatus = Convert.ToInt32(bid[8]);
            aio.UserId = Convert.ToInt32(bid[1]);
            aio.UserName = bid[2];
            aio.UserPointCount = Convert.ToInt32(bid[3]);
            aio.OrderStatusId = 1;
            aio.LastUpdateDate = DateTime.Now;


            return aio;
        }


        /// <summary>
        /// Odświeża informacje o liczbie kupujacych w dane aukcji, cenie i liczbie sztuk.
        /// Dotyczy aukcji dla wszystkich sprzedajacych
        /// </summary>  
        /// <param name="itemIDs"></param>
        //public void RefreshItemOrders()
        //{
        //    Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.JacekStawicki.ToString());
        //    long[] itemIDs = allegroScan.GetItemsToRefresh(0);
        //        //.Where(x => x.BidCount > 0)
        //       // .Select(x => x.ItemId)
        //        //.ToArray();
        //    RefreshItemOrders(Dal.Helper.MyUsers.JacekStawicki.ToString(), itemIDs, false);
        //}
        //public void FinalRefreshItemOrders()
        //{
        //    Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.JacekStawicki.ToString());
        //    long[] itemIDs = allegroScan.GetItemsToFinalRefresh();
        //    RefreshItemOrders(Dal.Helper.MyUsers.JacekStawicki.ToString(), itemIDs, true);
             

        //}
        public void RefreshItemOrders(string userName, long[] itemIDs, string journals, bool isMyItem)
        {

            Console.WriteLine(String.Format("Nast batch"));
            Parallel.ForEach(itemIDs, i =>
            {
                Console.WriteLine(String.Format("Start Id: {0}", i));
                GetItemOrders2(userName, i, isMyItem);
                Console.WriteLine(String.Format("Stop Id: {0}", i));
            });

            //foreach (long i in itemIDs)
            //{
            //    GetItemOrders2(userName, i, isMyItem);
            //}

            if (journals != null)
                allegroScan.SetAllegroSiteJournalAsProcessed(journals);

        }

        private void GetItemOrders2(string userName, long itemId, bool isMyItem)
        {
            List<LajtIt.Dal.AllegroItemOrder> orders = GetItemOrders(userName, itemId, isMyItem);
            try
            {
                if (orders != null)
                {
                    // naprawic bo wywala blad
                    //allegroScan.UpdateItemOrders(orders, itemId, isMyItem);

                }
                // if (isFinal)
                //     allegroScan.SetFinalScan(itemId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ///// <summary>
        ///// Pobiera moje aukcje i sprawdza oferty
        ///// </summary>
        //public void RefreshItemOrders(long[] itemIDs)
        //{            
        //    RefreshItemOrders(itemIDs,false);
        //}
        internal void SetMovedToArchive(long itemId)
        {
            allegroScan.SetMovedToArchive(itemId);
        }
        internal void SetEndingInfo(long itemId, int endingInfo)
        {
            allegroScan.SetEndingInfo(itemId, endingInfo);
        }
   

        //public void GetBuyerData(long[] itemIDs)
        //{
        //    Dal.AllegroScan allegroScan = new Dal.AllegroScan();

        //    ProcessBuyerData(itemIDs);
        //    ProcessItemTransactions(itemIDs);
        //}
 


        //public void ScanCategory(int[] categories)
        //{
        //    Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.JacekStawicki.ToString());
        //    AllegroHelper ah = new AllegroHelper();
        //    ScanCategory(ah, categories);

        //    //for (int offset = 0; offset < offsetCount; offset++)
        //    //{
        //    //    ScanCategory(ah, categoryId, offset);
        //    //}

        //}

        //private void ScanCategory(AllegroHelper ah, int[] categories)
        //{
        //    List<AllegroNewWCF.ItemsListType> srt = new List<AllegroNewWCF.ItemsListType>();
        //    int count;

        //    srt.AddRange(ah.ScanCategory(Dal.Helper.MyUsers.JacekStawicki.ToString(), categories, 0, out count));
        //    SetAllegroItems(srt);

        //    int partsCount = (count / Bll.AllegroHelper.PageSize);

        //    for (int i = 1; i < partsCount + 1; i++)
        //    {
        //        srt.AddRange(ah.ScanCategory(Dal.Helper.MyUsers.JacekStawicki.ToString(), categories, i, out count));
        //        SetAllegroItems(srt);

        //        srt.Clear();
        //    }


            
        //}

        //private void SetAllegroItems(List<AllegroNewWCF.ItemsListType> srt)
        //{
        //    #region Save Items
        //    List<Dal.AllegroItem> items = new List<Dal.AllegroItem>();
        //    List<Dal.AllegroUser> users = new List<Dal.AllegroUser>();
        //    foreach (AllegroNewWCF.ItemsListType s in srt)
        //    {
        //        if (users.Where(x => x.UserId == s.sellerInfo.userId).Count() == 0)
        //        {
        //            Dal.AllegroUser user = new Dal.AllegroUser()
        //            {
        //                UserId = s.sellerInfo.userId,
        //                UserName = s.sellerInfo.userLogin
        //            };
        //            users.Add(user);
        //        }

        //        Dal.AllegroItem item = new Dal.AllegroItem()
        //        {
        //            ItemId = s.itemId,
        //            UserId = s.sellerInfo.userId,
        //            BuyNowPrice = (decimal)s.priceInfo[0].priceValue,
        //            BidCount = s.bidsCount,
        //            CurrentPrice = (decimal)s.priceInfo[0].priceValue,
        //            Name = s.itemTitle,
        //            CategoryId = s.categoryId,
        //            EndingTime = Dal.Helper.DateTimeToUnixTime(s.endingTime),
        //            FotoCount = s.photosInfo.Length,
        //            EndingInfo = 1,
        //            AllegroStandard = (s.additionalInfo == 1 || s.additionalInfo == 3 ? 1 : 0),
        //            ImageUrl = s.photosInfo[0].photoUrl,
        //            InsertDate = DateTime.Now
        //        };
        //        items.Add(item);
        //    }
        //    allegroScan.SetScanCategory(users, items);
        //    #endregion
        //}

        public List<Dal.AllegroUser> GetUsers()
        {
            return allegroScan.GetUsers();
        }
    
            //public void RefreshMe()
            //{
            //    // przeniesione do procedury ProcessMe
            //    //return;



            //    Bll.AllegroHelper.GetVersionKeys();//(Dal.Helper.MyUsers.JacekStawicki.ToString());
            //    Bll.AllegroHelper asc = new Bll.AllegroHelper();
            //    ///  --------- 1 --------- /////
            //    // Pobiera aukcje użytkownika
            //    foreach (Dal.Helper.MyUsers e in Enum.GetValues(typeof(Dal.Helper.MyUsers)))
            //    {

            //        List<Dal.AllegroItem> items = new List<Dal.AllegroItem>();
            //        List<AllegroNewWCF.ItemsListType> srt = asc.GetUserAuctions( Dal.Helper.GetUserId(e.ToString()), e.ToString());

            //        long[] itemIds = srt.Select(x => x.itemId).ToArray();
            //        string userName = e.ToString();

            //        Bll.AllegroScan scan = new Bll.AllegroScan();
            //        long[] notFound;
            //        long[] killed;


            //        long[] partItemIds = itemIds.Take(25).ToArray();

            //        while (partItemIds.Length > 0)
            //        {


            //            List<Dal.AllegroItem> allegroItems = scan.AddOrUpdateAuctions(partItemIds, userName, out notFound, out killed);

            //            items.AddRange(allegroItems);
            //            Dal.AllegroScan allegroScan = new Dal.AllegroScan();
            //            allegroScan.AddOrUpdateAuctions(null, allegroItems);

            //            itemIds = itemIds.Skip(25).ToArray();
            //            partItemIds = itemIds.Take(25).ToArray();
            //        }


            //        Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            //        List<Dal.AllegroItem> notin = pch.GetTmp(items);
            //        List<Dal.ProductCatalogAllegroItem> itemsToAdd = new List<Dal.ProductCatalogAllegroItem>();
            //        foreach (Dal.AllegroItem item in notin)
            //            itemsToAdd.Add(
            //                new Dal.ProductCatalogAllegroItem()
            //                {
            //                    AllegroItemStatusId = 66,
            //                    BatchId = 1,
            //                    InsertDate = DateTime.Now,
            //                    ItemId = item.ItemId,
            //                    ProductCatalogId = 1,
            //                    Comment = item.Name


            //                }
            //                );
            //        pch.SetTmp(itemsToAdd);
 
            //    }
            //    /*
            //             /////  --------- 2 --------- /////
            //             /// Odśwież oferty w aukcjach

            //             long[] itemIDs = //new long[] { 2721565957 };
            //                 /// TODO ! uncomment this when done!
            //             allegroScan.GetItemsToRefresh(Dal.Helper.MyID).Select(x => x.ItemId).ToArray();

            //             RefreshItemOrders(itemIDs, false, true);

            //             /////  --------- 3 --------- /////
            //             /// Pobieranie formularzy dostawy

            //             /// TODO ! uncomment this when done!
            //             itemIDs = allegroScan.GetMyItemIDs();
            //             GetBuyerData(itemIDs);


            //             /////  --------- 4 --------- /////
            //             /// Tworzenie zamówień w systemie
            //             CreateOrders();

            //             /////  --------- 5 --------- /////
            //             /// Pobierania płatności
            //             GetPayments();

            //             RefreshUserLastUpdate((int)Dal.Helper.MyID);
            //              * */
            //}
        /// <summary>
        /// Tworzenie zamówień w systemie
        /// </summary>
        public void CreateOrders()
        {
            OrderHelper oh = new OrderHelper();

            List<Dal.AllegroItemOrdersForOrderCreation> aios = allegroScan.GetMyItemOrdersForOrderCreation();
            //.Where(x=>itemIDs.Contains(x.ItemId)).ToList();
            List<Dal.AllegroItemTransactionItem> buyerForms = allegroScan.GetMyBuyerFormsForOrderCreation();
            //.Where(x => itemIDs.Contains(x.ItemId)).ToList();
            //foreach (Dal.AllegroItemOrder aio in aios)
            oh.InsertAllegroProduct(aios, buyerForms);


            // może się zdarzyć, iż istnieją formularze dostawy wypelnione pozniej
            // ktore nie "poszly" razem z AllegroItemOrder. Zaczytajmy wiec ich dane osobno
            // Pobieramy je wiec jeszcze raz jako ze te ktore zostaly wczesniej wczytane maja juz flage OrderCreated.
            buyerForms = allegroScan.GetMyBuyerFormsForOrderCreation();
            //  .Where(x => itemIDs.Contains(x.ItemId)).ToList();
            //buyerForms = buyerForms.Where(
            //     x => x.ItemId == 2807396316 && x.AllegroItemTransaction.UserBuyerId == 3714890).ToList();
            oh.UpdateOrderBasedOnBuyerForm(buyerForms);
        }

        //public void RefreshUser(int userId)
        //{
        //    long[] itemIDs = allegroScan.GetItemsToRefresh(userId).Where(x => x.BidCount > 0 && x.UserId == userId).Select(x => x.ItemId).ToArray();
        //    RefreshItemOrders(itemIDs,false);
        //    RefreshUserLastUpdate(userId);
        //}

        private void RefreshUserLastUpdate(int userId)
        {
            allegroScan.RefreshUserLastUpdate(userId);
        }
        //public void GetComments()
        //{
        //    Bll.AllegroHelper.GetVersionKeys();// (Dal.Helper.MyUsers.JacekStawicki.ToString());
        //    //Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.CzerwoneJablko.ToString());
        //    //Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.Oswietlenie_Lodz.ToString());
        //    List<Dal.AllegroItem> items = allegroScan.GetMyItemIDs();
        //    AllegroHelper ah = new AllegroHelper();
        //    try
        //    {
        //        foreach (Dal.AllegroItem item in items)
        //        {

        //            AllegroNewWCF.MyFeedbackListStruct2[] iis = ah.GetComments(item.AllegroUser.UserName, item.ItemId);

        //            if (iis != null)
        //                ProcessComments(iis);


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.LogError(ex, "GetComments");
        //        throw ex;
        //    }

        //    try
        //    {
        //        EmailEditor ee = new EmailEditor();
        //        ee.SendAllegroComments();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.LogError(ex, "SendAllegroComments");
        //        throw ex;
        //    }

        //    try
        //    {
        //        SetComments();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.LogError(ex, "SetComments");
        //        throw ex;
        //    }

        //}

        private void SetComments()
        {
            //long commentId = 0;

            //List<Dal.AllegroComment> comments = allegroScan.GetComments();
            //List<Dal.AllegroCommentText> commentTexts = allegroScan.GetCommentTexts();
            //Random rnd = new Random();
            //AllegroHelper ah = new AllegroHelper();

            //foreach (Dal.AllegroComment comment in comments)
            //{
            //    try
            //    {
            //        commentId = comment.CommentId;
            //        string commentText = commentTexts[rnd.Next(commentTexts.Count)].Comment;

            //        int replyCommentId = ah.SetComment(comment.AllegroItem.AllegroUser.UserName, comment.ItemId, (int)comment.AuthorUserId, commentText);

            //        allegroScan.SetCommentReply(commentId, replyCommentId);
            //    }
            //    catch (Exception ex)
            //    {
            //        if (ex.Message != "Użytkownikowi we wskazanej aukcji został już wystawiony komentarz.")
            //        {
            //            throw ex;
            //        }
            //        else
            //            allegroScan.SetCommentReply(commentId, 0);
            //    }
            //}

        }

        //private void ProcessComments(AllegroNewWCF.MyFeedbackListStruct2[] feed)
        //{
        //    List<Dal.AllegroComment> comments = new List<Dal.AllegroComment>();

        //    comments.AddRange(

        //        feed.Select(x => ProcessComments(x)).ToList());

        //    Dal.AllegroScan allegroScan = new Dal.AllegroScan();
        //    allegroScan.SetComments(comments);
        //}

        //private Dal.AllegroComment ProcessComments(AllegroNewWCF.MyFeedbackListStruct2 x)
        //{
        //    try
        //    {
        //        Dal.AllegroComment comment = new Dal.AllegroComment();
        //        comment.AuthorUserId = Convert.ToInt64(x.feedbackArray[0]);        //identyfikator użytkownika, który wystawił komentarz,
        //        if (x.feedbackArray[1] != null && x.feedbackArray[1] != "NULL")
        //            comment.RecipentUserId = Convert.ToInt64(x.feedbackArray[1]);       //identyfikator użytkownika, któremu komentarz został wystawiony,
        //        comment.CommentDateTime = Convert.ToDateTime(x.feedbackArray[2]);        //data wystawienia komentarza,
        //        comment.CommentTypeId = Convert.ToInt32(x.feedbackArray[3]);        //typ komentarza (1 - pozytywny, 2 - negatywny, 3 - neutralny),
        //        comment.Comment = x.feedbackArray[4];         //treść komentarza,
        //        comment.ItemId = Convert.ToInt64(x.feedbackArray[5]);        //identyfikator oferty,
        //        comment.CommentId = Convert.ToInt64(x.feedbackArray[6]);         //identyfikator komentarza,
        //        if (x.feedbackArray[7] != null && x.feedbackArray[7] != "NULL" && x.feedbackArray[7] != "0")
        //            comment.ReplyTime = Convert.ToInt64(x.feedbackArray[7]);      //data odpowiedzi na komentarz,
        //        comment.ReplyComment = x.feedbackArray[8];         //treść odpowiedzi na komentarz,
        //        comment.Side = x.feedbackArray[9];        //strona transakcji, której komentarz został wystawiony (BUYER - komentarz wystawiony kupującemu, SELLER - komentarz wystawiony sprzedającemu),
        //        comment.AuthorUserName = x.feedbackArray[10];     //nazwa użytkownika wystawiającego komentarz,
        //        //liczba punktów użytkownika wystawiającego komentarz,
        //        //identyfikator kraju użytkownika wystawiającego komentarz.

        //        comment.EmailSent = false;
        //        return comment;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void GetPayments()
        //{

        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    List<Dal.AllegroItemOrder> orders = oh.GetBuyersForPayments();


        //    foreach (Dal.AllegroItemOrder aio in orders)
        //    {
        //        GetPayments(aio.ItemId, aio.UserId);
        //    }


        //}

        private bool GetPayments(string userName, long itemId, long userId)
        {
            AllegroHelper ah = new AllegroHelper();
            Dal.AllegroScan allegroScan = new Dal.AllegroScan();
             
            int offset = 0;

            List<Dal.AllegroPayment> payments = new List<Dal.AllegroPayment>();
            List<Dal.AllegroPaymentDetail> paymentDetails = new List<Dal.AllegroPaymentDetail>();

            payments.AddRange(GetPayments(ah.GetPayments(userName, (int)userId, itemId, offset), ref paymentDetails));
            allegroScan.SetPayments(payments, paymentDetails);

            return payments.Count > 0;
        }

        private List<Dal.AllegroPayment> GetPayments(List<AllegroNewWCF.UserIncomingPaymentStruct> payments,
            ref List<Dal.AllegroPaymentDetail> paymentDetails)
        {
            List<Dal.AllegroPayment> l = new List<Dal.AllegroPayment>();
            foreach (AllegroNewWCF.UserIncomingPaymentStruct payment in payments)
            {
                Dal.AllegroPayment p = new Dal.AllegroPayment()
                {
                    BuyerId = payment.payTransBuyerId,
                    ItemId = payment.payTransItId,
                    PaymentTransactiondId = payment.payTransId,
                    PostageAmount = (decimal)payment.payTransPostageAmount,
                    TransactionAmount = (decimal)payment.payTransAmount,
                    TransactionCount = payment.payTransCount,
                    TransactionEndDate = payment.payTransRecvDate,
                    TransactionIncomplate = payment.payTransIncomplete,
                    TransactionPrice = (decimal)payment.payTransPrice,
                    TransactionStartDate = payment.payTransCreateDate,
                    TransactionStatus = payment.payTransStatus,
                    TransactionType = payment.payTransType,
                    IsActive = true
                };
                l.Add(p);
                if (payment.payTransDetails.Length > 0)
                {
                    foreach (AllegroNewWCF.PaymentDetailsStruct pds in payment.payTransDetails)
                    {
                        paymentDetails.Add(new Dal.AllegroPaymentDetail()
                        {
                            AllegroPayment = p,
                            Count = pds.payTransDetailsCount,
                            ItemId = pds.payTransDetailsItId,
                            PaymentTransactiondId = p.PaymentTransactiondId,
                            Price = (decimal)pds.payTransDetailsPrice
                        });

                    }
                }
            }


            return l;
        }

        //public void AutoCreateAuction()
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    pch.SetProductToCreateOnAllegro();


        //    /* stare metody
        //    Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.JacekStawicki.ToString());
        //    Dal.AllegroScan ac = new Dal.AllegroScan();
        //    long itemId = ac.GetAllegrItemForAutoCreation();
        //    AllegroHelper ah = new AllegroHelper();
        //    ah.SetShopItem(Dal.Helper.MyUsers.JacekStawicki.ToString(), itemId);
        //     */
        //}

        //public void FollowUsers()
        //{
            //Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.JacekStawicki.ToString());
            //Dal.OrderHelper oh = new Dal.OrderHelper();
            //List<Dal.AllegroUser> users = oh.GetUsersToFollow();

            //foreach (Dal.AllegroUser user in users)
            //{

            //    /////  --------- 1 --------- /////
            //    /// Pobiera aukcje użytkownika
            //    GetUserAuctions(Dal.Helper.MyUsers.JacekStawicki.ToString(), new Dal.AllegroUserTrackList() { UserId = (int)user.UserId });

            //    /////  --------- 2 --------- /////
            //    /// Odśwież oferty w aukcjach

            //    long[] itemIDs = allegroScan.GetItemsToRefresh(user.UserId);//.Select(x => x.ItemId).ToArray();

            //    RefreshItemOrders(Dal.Helper.MyUsers.JacekStawicki.ToString(), itemIDs, false);
            //    RefreshUserLastUpdate((int)user.UserId);
            //}
        //}
    }
}
