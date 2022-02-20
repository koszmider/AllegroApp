using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Linq;

namespace LajtIt.Dal
{
    public partial class AllegroScan
    {
        public AllegroScan()
        {
            //    LajtitAllegroDB ctx = new LajtitAllegroDB();
            //    ctx.Connection
        }

        public string GetWebApiKey(string userName)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroUser.Where(x => x.UserName.ToLower() == userName.ToLower()).Select(x => x.WebApiKey).FirstOrDefault();
            }
        }
        public void SetCategories(List<AllegroCategory> categories)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                int[] existingCategories = ctx.AllegroCategory.Select(x => x.CategoryId).ToArray();
                List<AllegroCategory> toInsert = categories.Where(x => !existingCategories.Contains(x.CategoryId)).ToList();
                ctx.AllegroCategory.InsertAllOnSubmit(toInsert);
                ctx.SubmitChanges();


                foreach(Dal.AllegroCategory categoryToUpdate in ctx.AllegroCategory)
                {
                    Dal.AllegroCategory category = 
                        categories.Where(x => x.CategoryId == categoryToUpdate.CategoryId).FirstOrDefault();

                    if (category != null)
                    {
                        categoryToUpdate.CategoryParentId = category.CategoryParentId;
                        categoryToUpdate.Name = category.Name;
                    }

                }
                ctx.SubmitChanges();
            }
        }

        public AllegroUser SetUser(AllegroUser user)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                AllegroUser existingUser = ctx.AllegroUser.Where(x => x.UserId == user.UserId).FirstOrDefault();

                if (existingUser == null)
                {
                    ctx.AllegroUser.InsertOnSubmit(user);
                    ctx.SubmitChanges();
                    return user;
                }

                return existingUser;
            }
        }


        /// <summary>
        /// Gets active AllegroItems 
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public AllegroItem[] GetItems(LajtitAllegroDB ctx, long[] itemIds)
        {
            return ctx.AllegroItem
                .Where(x => itemIds.Contains(x.ItemId))
                .ToArray();
        }

        //public List<AllegroItem> GetItems(long userId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<AllegroItem>(x => x.AllegroCategory);
        //        ctx.LoadOptions = dlo;
        //        return ctx.AllegroItem
        //            .Where(x => x.UserId == userId && x.EndingInfo.Value == 1)
        //            .ToList();
        //    }
        //}
        public AllegroUser GetAllegroUser(long userId)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroUser
                    .Where(x => x.UserId == userId)
                    .FirstOrDefault();

            }
        }
        public List<AllegroUser> GetAllegroUsers(long[] userIds)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroUser
                    .Where(x => userIds.Contains(x.UserId))
                    .ToList();

            }
        }
        //public long[] GetItemsToRefresh(long userId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return GetItemsToRefresh(ctx, userId);
        //    }
        //}

        /// <summary>
        /// Pobiera listę przedmiotów do odświeżenia
        /// Jeśli parametr userId jest podany to pobiera dla danego użytkownika
        /// jeśli nie jest podany, to wyklucza użytkowników z listy do śledzenia oraz własne loginy.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        //public long[] GetItemsToRefresh(LajtitAllegroDB ctx, long userId)
        //{

        //    var q = ctx.AllegroItem.AsQueryable();
        //    if (userId != 0)
        //        q = q.Where(x => x.UserId == userId);
        //    else
        //    { 
        //        long[] usersToExclude = ctx.AllegroUser.Where(x => x.Follow).Select(x => x.UserId).ToArray();
        //        q = q.Where(x => !Helper.GetMyIds().Contains(x.UserId)
        //                  && !usersToExclude.Contains(x.UserId)
        //                  && x.BidCount > 0) ;
        //    }

        //    q = q.Where(x => x.EndingInfo == 1 || x.EndingInfo == null);

        //    //
        //    q = q.OrderBy(x => x.LastUpdateDate);
        //    return q.Select(x=>x.ItemId).ToArray();
        //}

        //public long[] GetItemIdsToRefresh(int limit)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        // użytkownicy których śledzę
        //        List<long> usersToExclude = ctx.AllegroUser.Where(x => x.Follow).Select(x => x.UserId).ToList();
        //        // moje loginy
        //        usersToExclude.AddRange(Helper.GetMyIds());

        //        return ctx.AllegroItem
        //            .Where(x =>
        //                !usersToExclude.Contains(x.UserId)
        //                //&& !usersToExclude.Contains(x.UserId)
        //                && (x.EndingInfo == 1 || x.EndingInfo == null))
        //            .OrderBy(x => x.LastUpdateDate)
        //            .Select(x => x.ItemId)
        //            .Take(limit)
        //            .ToArray();
        //    }
        //}

        public List<AllegroUser> GetMyUsers()
        {

            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            { 

                return ctx.AllegroUser
                    .Where(x => x.MyUserId == true)
                    .ToList();
            }
        }
        //public List<AllegroItem> GetMyItemIDs()
        //{

        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<AllegroItem>(x => x.AllegroUser);
        //        ctx.LoadOptions = dlo;

        //        return ctx.AllegroItem
        //            .Where(x => Helper.GetMyIds().Contains(x.UserId) && x.MovedToArchive == false)
        //            .ToList();
        //    }
        //}
        //public void UpdateItems(List<AllegroItem> allegroItems)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        long[] itemIds = allegroItems.Select(x => x.ItemId).Distinct().ToArray();
        //        AllegroItem[] itemsToRefresh = GetItems(ctx, itemIds);

        //        foreach (AllegroItem itemToRefresh in itemsToRefresh)
        //        {
        //            AllegroItem ai = allegroItems.FirstOrDefault(x => x.ItemId == itemToRefresh.ItemId);
        //            if (ai == null) continue;

        //            UpdateItem(itemToRefresh, ai);

        //        }

        //        ctx.SubmitChanges();
        //    }
        //}
        

        //private void UpdateItemOrder(AllegroItemOrder existingOrder, AllegroItemOrder aio)
        //{
        //    existingOrder.UserName = aio.UserName;
        //    existingOrder.UserPointCount = aio.UserPointCount;
        //    existingOrder.ItemsOrdered = aio.ItemsOrdered;
        //    existingOrder.ItemPrice = aio.ItemPrice;
        //    existingOrder.OrderDate = aio.OrderDate;
        //    existingOrder.OrderStatus = aio.OrderStatus;
        //    existingOrder.LastUpdateDate = DateTime.Now;
        //}
        //private static void UpdateItem(AllegroItem itemToRefresh, AllegroItem ai)
        //{
        //    itemToRefresh.BidCount = ai.BidCount;
        //    itemToRefresh.BuyNowPrice = ai.BuyNowPrice;
        //    itemToRefresh.CurrentPrice = ai.CurrentPrice;
        //    itemToRefresh.EndingInfo = ai.EndingInfo;
        //    itemToRefresh.EndingTime = ai.EndingTime;
        //    itemToRefresh.AllegroStandard = ai.AllegroStandard;
        //    itemToRefresh.Name = ai.Name;
        //    itemToRefresh.UserId = ai.UserId;
        //    itemToRefresh.ItemId = ai.ItemId;
        //    itemToRefresh.FotoCount = ai.FotoCount;
        //    itemToRefresh.CategoryId = ai.CategoryId;
        //    itemToRefresh.HitCount = ai.HitCount;
        //    itemToRefresh.LastUpdateDate = ai.LastUpdateDate;
        //    itemToRefresh.Quantity = ai.Quantity;
        //    itemToRefresh.ImageUrl = ai.ImageUrl;
        //    itemToRefresh.Options = ai.Options;
        //    itemToRefresh.StartingQuantity = ai.StartingQuantity;
        //    itemToRefresh.ReservePrice = ai.ReservePrice;
        //    itemToRefresh.Price = ai.Price;
        //    itemToRefresh.BuyNowActive = ai.BuyNowActive;
        //    itemToRefresh.HightBidderId = ai.HightBidderId;
        //    itemToRefresh.HightBidderLogin = ai.HightBidderLogin;
        //    itemToRefresh.SellingMode = ai.SellingMode;
        //}

        //public List<AllegroItemsView> GetAuctions(long userId, long auctionType, int sell)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {

        //        int[] endingInfo = GetEndingInfo(auctionType);

        //        var q = ctx.AllegroItemsViews.Where(x =>

        //            endingInfo.Contains(Convert.ToInt32(x.EndingInfo.Value))
        //            );
        //        //if (!String.IsNullOrEmpty(userName))
        //          //  q = q.Where(x => x.UserName.ToLower().StartsWith(userName.ToLower()));
        //        q = q.Where(x => x.UserId == userId);
        //        if (sell == 1)
        //            q = q.Where(x => x.BidCount > 0);
        //        if (sell == 2)
        //            q = q.Where(x => x.BidCount == 0);
        //        //if (categoryType != 0)
        //        //{
        //        //    int[] categoryTypes;
        //        //    if (categoryType == 1)
        //        //        categoryTypes = new int[] { 2, 3, 4 };
        //        //    else
        //        //        categoryTypes = new int[] { categoryType };

        //        //    q = q.Where(x => categoryTypes.Contains(x.CategoryTypeId.Value));
        //        //}
        //        return q.ToList();

        //    }
        //}

        //private int[] GetEndingInfo(long auctionType)
        //{
        //    if (auctionType == 0)
        //        return new int[] { 1, 2, 3 };

        //    if (auctionType == 1)
        //        return new int[] { 1 };

        //    return new int[] { 2, 3 };



        //}

        //public void SetFinalScan(long itemId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        AllegroItem item = ctx.AllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();
        //        if (item != null)
        //        {
        //            item.FinalScan = true;
        //            item.LastUpdateDate = DateTime.Now;
        //            ctx.SubmitChanges();
        //        }

        //    }
        //}
        //public void SetMovedToArchive(long itemId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        AllegroItem item = ctx.AllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();
        //        if (item != null)
        //        {
        //            item.MovedToArchive = true;
        //            item.EndingInfo = item.EndingInfo ?? 2;
        //            item.LastUpdateDate = DateTime.Now;
        //            ctx.SubmitChanges();
        //        }

        //    }
        //}
        ///// <summary>
        ///// Informacja na temat stanu oferty (1 - trwa, 2 - zakończyła się w sposób "naturalny" 
        ///// (koniec czasu trwania, albo wykupienie wszystkich dostępnych przedmiotów w przypadku Kup Teraz), 
        ///// 3 - została zakończona przez sprzedającego przed czasem).
        ///// 4. - usunieta przez Allegro
        ///// </summary>
        ///// <param name="itemId"></param>
        ///// <param name="endingInfo"></param>
        //public void SetEndingInfo(long itemId, int endingInfo)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        AllegroItem item = ctx.AllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();
        //        if (item != null)
        //        {
        //            item.EndingInfo = endingInfo;
        //            item.LastUpdateDate = DateTime.Now;
        //            ctx.SubmitChanges();
        //        }
        //        List<Dal.AllegroSiteJournalDeal> deals = ctx.AllegroSiteJournalDeals.Where(x => x.DealItemId == itemId
        //            && x.DealEventType == 1).ToList();
        //        foreach (AllegroSiteJournalDeal deal in deals)
        //        {
        //            deal.IsProcessed = true;
        //            deal.ProcessedDate = DateTime.Now;
        //        }
        //        ctx.SubmitChanges();
        //    }
        //}

        //public List<AllegroStatsResult> GetAllegroStats(int year, int month, decimal value)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return ctx.AllegroStats(year, month, value).ToList();
        //    }
        //}

        //public void SetTransaction(AllegroItemTransaction transaction, List<AllegroItemTransactionItem> items)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        if (ctx.AllegroItemTransactions.Where(x => x.BuyFormId == transaction.BuyFormId).FirstOrDefault() == null)
        //        {
        //            ctx.AllegroItemTransactionItems.InsertAllOnSubmit(items);
        //            ctx.SubmitChanges();
        //        }
        //    }
        //}



        //public void SetItemBuyers(List<AllegroItemOrder> list)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        long[] itemIDs = list.Select(x => x.ItemId).Distinct().ToArray();
        //        List<AllegroItemOrder> existingItemOrders = ctx.AllegroItemOrder
        //            .Where(x => itemIDs.Contains(x.ItemId))
        //            .ToList();

        //        foreach (AllegroItemOrder aio in list)
        //        {
        //            try
        //            {
        //                AllegroItemOrder exitingItem = existingItemOrders.Where(x => x.ItemId == aio.ItemId
        //                    && x.UserId == aio.UserId).FirstOrDefault();

        //                if (exitingItem == null)
        //                    ctx.AllegroItemOrder.InsertOnSubmit(aio);
        //                else
        //                    UpdateItemOrders(exitingItem, aio);

        //                ctx.SubmitChanges();
        //            }
        //            catch (Exception ex)
        //            {
        //                ErrorHandler.LogError(ex, String.Format("ItemId: {0}, UserId: {1}", aio.ItemId, aio.UserId));

        //            }
        //        }
        //    }

        //}

        //private void UpdateItemOrders(AllegroItemOrder existingItem, AllegroItemOrder aio)
        //{
        //    //Helper.CopyProperties(aio, exitingItem);

        //    //aio.UserId                     = aio.UserId                 ;    
        //    existingItem.UserName = aio.UserName;
        //    existingItem.UserPointCount = aio.UserPointCount;
        //    existingItem.FirstName = aio.FirstName;
        //    existingItem.LastName = aio.LastName;
        //    existingItem.CompanyName = aio.CompanyName;
        //    existingItem.CountryId = aio.CountryId;
        //    existingItem.StateId = aio.StateId;
        //    existingItem.Postcode = aio.Postcode;
        //    existingItem.City = aio.City;
        //    existingItem.Address = aio.Address;
        //    existingItem.Email = aio.Email;
        //    existingItem.Phone = aio.Phone;
        //    existingItem.Phone2 = aio.Phone2;
        //    existingItem.RegistrationCountryId = aio.RegistrationCountryId;
        //    existingItem.Junior = aio.Junior;
        //    existingItem.HasShop = aio.HasShop;
        //    existingItem.CompanyIcon = aio.CompanyIcon;
        //    existingItem.ShipmentFirstName = aio.ShipmentFirstName;
        //    existingItem.ShipmentLastName = aio.ShipmentLastName;
        //    existingItem.ShipmentCompanyName = aio.ShipmentCompanyName;
        //    existingItem.ShipmentCountryId = aio.ShipmentCountryId;
        //    existingItem.ShipmentPostcode = aio.ShipmentPostcode;
        //    existingItem.ShipmentCity = aio.ShipmentCity;
        //    existingItem.ShipmentAddress = aio.ShipmentAddress;
        //    existingItem.AllegroStandard = aio.AllegroStandard;


        //    existingItem.LastUpdateDate = DateTime.Now;
        //}




        //public void SetScanCategory(List<AllegroUser> users, List<AllegroItem> items)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        long[] existingUsersIDs = users.Select(x => x.UserId).Distinct().ToArray();
        //        List<AllegroUser> existingUsers = ctx.AllegroUser.Where(x => existingUsersIDs.Contains(x.UserId)).ToList();

        //        List<AllegroUser> usersToAdd = users.Where(x => !existingUsers.Select(y => y.UserId).Distinct().ToArray().Contains(x.UserId)).ToList();

        //        ctx.AllegroUser.InsertAllOnSubmit(usersToAdd);

        //        ctx.SubmitChanges();

        //        SetUserItems(items);

        //    }
        //}

        //public List<AllegroUser> GetUsers()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return ctx.AllegroUser.OrderBy(x => x.UserName).ToList();
        //    }
        //}

        //public List<AllegroItemsSoldByUserResult> GetAuctionsByDate(long userId, int year, int month, int day)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return ctx.AllegroItemsSoldByUser(userId, year, month, day).ToList();
        //    }
        //}

        //public object GetItemsSoldByUserByDay(long userId, int year, int month)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return ctx.AllegroItemsSoldByUserByDay(userId, year, month).ToList();
        //    }
        //}

        //public long[] GetItemsToFinalRefresh()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        var q = ctx.AllegroItem.Where(x =>
        //                x.EndingInfo == 2
        //                && (!x.FinalScan.HasValue || x.FinalScan.Value == false)
        //                && x.MovedToArchive == false)
        //            .AsQueryable();

        //        q = q.Where(x => !Helper.GetMyIds().Contains(x.UserId)); ;

        //        return q.Select(x => x.ItemId).ToArray();
        //    }
        //}
        ////public long[] GetMyItemsToFinalRefresh()
        ////{
        ////    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        ////    {
        ////        return ctx.AllegroItem.Where(x => x.UserId==Helper.MyID 
        ////            //&& x.EndingInfo == 2 
        ////            && (!x.FinalScan.HasValue || x.FinalScan.Value == false)
        ////            && x.MovedToArchive == false)
        ////            .Select(x => x.ItemId).ToArray();
        ////    }
        ////}

        //public void SetComments(List<AllegroComment> comments)
        //{
        //    long[] commentIDs = comments.Select(x => x.CommentId).ToArray();


        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        long[] existingComments = ctx.AllegroComments.Where(x => commentIDs.Contains(x.CommentId)).Select(x => x.CommentId).ToArray();

        //        ctx.AllegroComments.InsertAllOnSubmit(comments.Where(x => !existingComments.Contains(x.CommentId)).ToList());

        //        ctx.SubmitChanges();

        //    }
        //}

        //public List<AllegroComment> GetComments()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<AllegroComment>(x => x.AllegroItem);
        //        dlo.LoadWith<AllegroItem>(x => x.AllegroUser);

        //        ctx.LoadOptions = dlo;

        //        return ctx.AllegroComments
        //            .Where(x => x.ReplyCommentId == null && x.Side == "SELLER" && x.CommentTypeId == 1)
        //            .OrderByDescending(x => x.CommentDateTime)
        //            //.Take(10)
        //            .ToList();

        //    }
        //}

        //public void SetCommentReply(long commentId, int replyCommentId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        AllegroComment comment = ctx.AllegroComments.Where(x => x.CommentId == commentId).FirstOrDefault();
        //        if (comment != null)
        //        {
        //            comment.ReplyCommentId = replyCommentId;

        //            ctx.SubmitChanges();

        //        }

        //    }
        //}

        //public List<AllegroCommentText> GetCommentTexts()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return ctx.AllegroCommentTexts.ToList();

        //    }
        //}

        //public List<AllegroItemOrder> GetItemOrders(long itemId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<AllegroItemOrder>(x => x.AllegroItem);

        //        ctx.LoadOptions = dlo;

        //        return ctx.AllegroItemOrder.Where(x => //itemId.Contains(x.ItemId) 
        //            x.ItemId == itemId
        //            && Helper.GetMyIds().Contains(x.AllegroItem.UserId)
        //            && x.OrderStatus == 1).ToList();

        //    }
        //}

        //public void RefreshUserLastUpdate(int userId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        AllegroUser au = ctx.AllegroUser.Where(x => x.UserId == userId).FirstOrDefault();

        //        if (au != null)
        //        {
        //            au.LastUpdate = DateTime.Now;
        //            ctx.SubmitChanges();
        //        }
        //    }
        //}

        //public void SetPayments(List<AllegroPayment> payments, List<AllegroPaymentDetail> paymentDetails)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        long[] incomingPayments = payments.Select(x => x.PaymentTransactiondId).Distinct().ToArray();
        //        long[] existingPayments = ctx.AllegroPayments.Where(x => incomingPayments.Contains(x.PaymentTransactiondId))
        //            .Select(x => x.PaymentTransactiondId).ToArray();
        //        ctx.AllegroPaymentDetails.InsertAllOnSubmit(paymentDetails.Where(x => !existingPayments.Contains(x.PaymentTransactiondId)));
        //        ctx.AllegroPayments.InsertAllOnSubmit(payments.Where(x => !existingPayments.Contains(x.PaymentTransactiondId)));

        //        ctx.SubmitChanges();

        //    }
        //}

        //public List<AllegroItemOrdersForOrderCreation> GetMyItemOrdersForOrderCreation()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        ctx.CommandTimeout = 360;
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<AllegroItemOrder>(x => x.AllegroItem);
        //        ctx.LoadOptions = dlo;
        //        //return ctx.AllegroItemOrder
        //        //    .Where(x => Helper.GetMyIds().Contains(x.AllegroItem.UserId))
        //        //    .Where(x => x.OrderStatus == 1) //Zakończone sprzedażą
        //        //    .Where(x => x.OrderCreated == false)
        //        //    .Where(x => x.Email != null)
        //        //    .ToList();

        //        return ctx.AllegroItemOrderForOrderCreations.ToList();
        //    }
        //}

        //public List<AllegroItemOrder> GetMyItemOrdersForOrderCreation(int[] allegroItemOrderIDs)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        ctx.CommandTimeout = 360;
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<AllegroItemOrder>(x => x.AllegroItem);
        //        ctx.LoadOptions = dlo;
        //        return ctx.AllegroItemOrder
        //            .Where(x => allegroItemOrderIDs.Contains(x.Id))
        //            .ToList();

             
        //    }
        //}
        //public List<AllegroItemTransactionItem> GetMyBuyerFormsForOrderCreation()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<AllegroItemTransactionItem>(x => x.AllegroItemTransaction);
        //        ctx.LoadOptions = dlo;


        //        return ctx.AllegroItemTransactionItems
        //            .Where(x => Helper.GetMyIds().Contains(x.AllegroItem.UserId))
        //            .Where(x => x.AllegroItemTransaction.OrderCreated == false)
        //            .ToList();
        //    }
        //}

        


        //public void SetAllegroItemResponse(AllegroItemResponse air)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        ctx.AllegroItemResponses.InsertOnSubmit(air);
        //        ctx.SubmitChanges();
        //    }
        //}

        //public void SetAllegroCosts(long itemId, List<AllegroCost> costs)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        List<AllegroCost> toDelete = ctx.AllegroCosts.Where(x => x.ItemId == itemId).ToList();
        //        ctx.AllegroCosts.DeleteAllOnSubmit(toDelete);
        //        ctx.AllegroCosts.InsertAllOnSubmit(costs);
        //        ctx.SubmitChanges();
        //    }
        //}

        //public List<AllegroItem> GetItems(long[] itemIds)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return ctx.AllegroItem.Where(x => itemIds.Contains(x.ItemId)).ToList();
        //    }
        //}

        //public AllegroItem GetAllegroItem(long itemId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<AllegroItem>(x => x.AllegroUser);
        //        ctx.LoadOptions = dlo;
        //        return ctx.AllegroItem.Where(x => x.ItemId == itemId && Helper.GetMyIds().Contains(x.UserId)).FirstOrDefault();
        //    }
        //}

    


    
        ////public List<AllegroCategoryView> GetCategories(int shopTypeId)
        ////{
        ////    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        ////    {
        ////        return ctx.AllegroCategoryViews.OrderBy(x => x.FullName).ToList();
        ////    }
        ////}

        //public void SetAllegroSiteJournalDeal(List<AllegroSiteJournalDeal> journalDeals)
        //{

        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        ctx.AllegroSiteJournalDeals.InsertAllOnSubmit(journalDeals);
        //        ctx.SubmitChanges();
        //    }
        //}

        //public void SetAllegroSiteJournal(List<AllegroSiteJournal> journals)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        ctx.AllegroSiteJournals.InsertAllOnSubmit(journals);
        //        ctx.SubmitChanges();
        //    }
        //}

        //public long GetJournalDealLastEventId(long userId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        var q = ctx.AllegroSiteJournalDeals
        //            .Where(x => x.DealSellerId == userId)
        //            .OrderByDescending(x => x.DealEventId).FirstOrDefault();
        //        if (q == null)
        //            return 0;
        //        else
        //            return q.DealEventId;
        //    }
        //}

        //public long GetJournalLastRowId(string userName)
        //{
        //    //using (LajtitHelperDB ctx = new LajtitHelperDB())
        //    //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {


        //        long q = 0;

        //        if (userName != null)
        //        {
        //            long userId = ctx.AllegroUser.Where(x => x.UserName == userName).Select(x => x.UserId).FirstOrDefault();

        //            q = ctx.AllegroSiteJournals
        //                .Where(x => x.UserSellerId == userId)
        //                .Select(x => x.RowId)
        //                .OrderByDescending(x => x)
        //                .FirstOrDefault();
        //        }
        //        else
        //        {
        //            long[] usersIds = ctx.AllegroUser.Where(x => x.MyUserId == true).Select(x => x.UserId).ToArray();

        //            q = ctx.AllegroSiteJournals
        //                .Where(x => !usersIds.Contains(x.UserSellerId))
        //                .Select(x => x.RowId)
        //                .OrderByDescending(x => x)
        //                .FirstOrDefault();
        //        }
        //        return q == null ? 0 : q;
        //        //  }
        //    }
        //}


        //public List<AllegroSiteJournal> GetMyStartJournal()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {

        //        long[] myUserId = ctx.AllegroUser.Where(x => x.MyUserId == true).Select(x => x.UserId).ToArray();
        //        string[] changeTypes = new string[] { "start", "change",  "bid" };

        //        return ctx.AllegroSiteJournals
        //            .Where(x =>
        //                    myUserId.Contains(x.UserSellerId)
        //                    && x.IsProcessed == false
        //                    && changeTypes.Contains(x.ChangeType)
        //                )
        //                .OrderBy(x => x.RowId)
        //                .ToList();

        //    }
        //}
        //public List<AllegroSiteJournal> GetOtherStartJournal()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {

        //        ctx.CommandTimeout = 6000;
        //        long[] myUserId = ctx.AllegroUser.Where(x => x.MyUserId == true).Select(x => x.UserId).ToArray();
        //       // string[] changeTypes = new string[] { "start"/*, "change", "end"*/ };

        //        return ctx.AllegroSiteJournals
        //            .Where(x =>
        //                myUserId.Contains(x.UserSellerId)    
        //                && x.ChangeType == "start"
        //                )
        //                .OrderBy(x => x.RowId)
        //                .ToList();

        //    }
        //}

        //public List<AllegroSiteJournal> GetMyNowJournal()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        ctx.CommandTimeout = 6000;
        //        long[] myUserId = ctx.AllegroUser.Where(x => x.MyUserId == true).Select(x => x.UserId).ToArray();
        //        return ctx.AllegroSiteJournals
        //            .Where(x =>
        //                myUserId.Contains(x.UserSellerId)    
        //                && x.IsProcessed ==true
        //                && x.ChangeType == "now"
        //                )
        //                .OrderBy(x => x.RowId)
        //                .ToList();

        //    }
            
        //}
    
        //public List<AllegroSiteJournal> GetMyEndJournal()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        ctx.CommandTimeout = 6000;
        //        long[] myUserId = ctx.AllegroUser.Where(x => x.MyUserId == true).Select(x => x.UserId).ToArray();

        //        return ctx.AllegroSiteJournals
        //            .Where(x =>
        //                myUserId.Contains(x.UserSellerId)
        //                && x.ChangeType == "end"
        //                && x.IsProcessed == false
        //                )
        //                .OrderBy(x => x.RowId)
        //                .ToList();
        //    }
        //}
        //public List<AllegroSiteJournal> GetOtherEndJournal()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        ctx.CommandTimeout = 6000;
        //        long[] myUserId = ctx.AllegroUser.Where(x => x.MyUserId == true).Select(x => x.UserId).ToArray();

        //        return ctx.AllegroSiteJournals
        //            .Where(x =>
        //                !myUserId.Contains(x.UserSellerId)
        //                && x.ChangeType == "end"
        //                )
        //                .OrderBy(x => x.RowId)
        //                .ToList();
        //    }
        //}


        //public List<AllegroSiteJournalDeal> GetJournalNewSoldItems(long itemId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<AllegroSiteJournalDeal>(x => x.AllegroUser);
        //        ctx.LoadOptions = dlo;
        //        return ctx.AllegroSiteJournalDeals
        //            .Where(x => x.DealEventType == 1
        //                && x.IsProcessed == false
        //                && x.DealItemId == itemId
        //                )
        //                .ToList();
        //    }

        //}

        //public long[] GetJournalDealNewItems()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return ctx.AllegroSiteJournalDeals
        //            .Where(x => x.DealEventType == 1
        //                && x.IsProcessed == false
        //                )
        //                .Select(x=>x.DealItemId)
        //                .ToArray();
        //    }

        //}
        //public void SetAllegroItemOrders(List<AllegroItemOrder> itemOrders, List<AllegroSiteJournalDeal> dealsProcessed)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        ctx.AllegroItemOrder.InsertAllOnSubmit(itemOrders);
        //        ctx.SubmitChanges();

        //        SetAllegroSiteJournalDealsAsProcessed(String.Join(",", dealsProcessed.Select(x=>x.DealEventId.ToString()).ToArray()));
        //    }
        //}

        //public void AddOrUpdateAuction(AllegroItem ai)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        AllegroItem existingItem = ctx.AllegroItem.Where(x => x.ItemId == ai.ItemId).FirstOrDefault();

        //        if (existingItem == null)
        //        {
        //            ctx.AllegroItem.InsertOnSubmit(ai);
        //        }
        //        else
        //        {
        //            UpdateItem(existingItem, ai);
        //        }
                 

        //        ctx.SubmitChanges();
        //    }
        //}

        //public void AddOrUpdateAuctions(List<AllegroSiteJournal> journals, List<AllegroItem> allegroItems)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        long[] itemIds = allegroItems.Select(x=>x.ItemId).ToArray();

        //        List<AllegroItem> existingItems = ctx.AllegroItem.Where(x => itemIds.Contains(x.ItemId)).ToList();

        //        foreach (AllegroItem ai in allegroItems)
        //        {
        //            AllegroItem existingItem = existingItems.Where(x => x.ItemId == ai.ItemId).FirstOrDefault();

        //            if (existingItem == null)
        //            {
        //                ctx.AllegroItem.InsertOnSubmit(ai);
        //            }
        //            else
        //            {
        //                UpdateItem(existingItem, ai);
        //            }
        //        }

        //       // long[] existingItems = ctx.AllegroItem.Where(x => itemIds.Contains(x.ItemId)).Select(x => x.ItemId).ToArray();
        //       // ctx.AllegroItem.InsertAllOnSubmit(allegroItems.Where(x=>!existingItems.Contains(x.ItemId)));
        //        ctx.SubmitChanges();
        //        if (journals != null)
        //            SetAllegroSiteJournalAsProcessed(String.Join(",", journals.Select(x => x.RowId.ToString()).ToArray()));
        //    }
        //}
        //public void AddOrUpdateAuctions(List<AllegroSiteJournal> journals, List<AllegroItem> allegroItems)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        long[] itemIds = allegroItems.Select(x => x.ItemId).ToArray();

        //        List<AllegroItem> existingItems = ctx.AllegroItem.Where(x => itemIds.Contains(x.ItemId)).ToList();

        //        foreach (AllegroItem ai in allegroItems)
        //        {
        //            AllegroItem existingItem = existingItems.Where(x => x.ItemId == ai.ItemId).FirstOrDefault();

        //            if (existingItem == null)
        //            {
        //                ctx.AllegroItem.InsertOnSubmit(ai);
        //            }
        //            else
        //            {
        //                UpdateItem(existingItem, ai);
        //            }
        //        }

        //        //long[] existingItems = ctx.AllegroItem.Where(x => itemIds.Contains(x.ItemId)).Select(x => x.ItemId).ToArray();
        //        //ctx.AllegroItem.InsertAllOnSubmit(allegroItems.Where(x => !existingItems.Contains(x.ItemId)));
        //        ctx.SubmitChanges();

        //    }
        //} 
        //public void AddAuctions(List<AllegroItem> allegroItems)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {

        //        ctx.AllegroItem.InsertAllOnSubmit(allegroItems); 

        //        ctx.SubmitChanges();
        //    }
        //}
        //public void AddOrUpdateAuctionWhenDeleted(List<AllegroSiteJournal> journals)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        foreach (long itemId in journals.Select(x => x.ItemId).Distinct().ToArray())
        //        {
        //            AllegroItem existingItem = ctx.AllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();

        //            if (existingItem == null)
        //            {
        //            }
        //            else
        //            {
        //                existingItem.EndingInfo = 2; //koniec
        //            }
        //        }
        //        ctx.SubmitChanges();

        //        SetAllegroSiteJournalAsProcessed(String.Join(",", journals.Select(x => x.RowId.ToString()).ToArray()));

        //    }
        //}
        ////public void AddOrUpdateAuctionWhenDeleted(List<AllegroSiteJournalsEnd> journals)
        ////{
        ////    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        ////    {
        ////        foreach (long itemId in journals.Select(x => x.ItemId).Distinct().ToArray())
        ////        {
        ////            AllegroItem existingItem = ctx.AllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();

        ////            if (existingItem == null)
        ////            {
        ////            }
        ////            else
        ////            {
        ////                existingItem.EndingInfo = 2; //koniec
        ////            }
        ////        }
        ////        ctx.SubmitChanges();

        ////        SetAllegroSiteJournalAsProcessed(String.Join(",", journals.Select(x => x.RowId.ToString()).ToArray()));

        ////    }
        ////}

        //public void SetAllegroSiteJournalDealsAsProcessed(string dealEventsId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        ctx.AllegroSiteJournalDealsSetAsProcessed(dealEventsId);
        //    }

        //}
        //public void SetAllegroSiteJournalAsProcessed(string rowIds)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        ctx.AllegroSiteJournalSetAsProcessed(rowIds);
        //    }
        //}
         
  
 

        public List<AllegroItemsView> GetAllegroItems(
            bool searchByQuery,
            string searchQuery, 
            long? itemId, 
            DateTime dateFrom,
            DateTime dateTo,            
            bool? isFinished, 
            bool? isSold, 
            bool? isPromoted,
            bool? isAuction,
            string userName,
            int categoryType)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                ctx.CommandTimeout = 360;

                var q = ctx.AllegroItemsView.AsQueryable();

                if (searchByQuery)
                {
                    if (searchQuery != null)
                        q = q.Where(x => x.Name.Contains(searchQuery));
                    if (itemId.HasValue)
                        q = q.Where(x => x.ItemId == itemId);
                }
                else
                {
                    q = q.Where(x => x.EndingDateTime.Value >= dateFrom && x.EndingDateTime.Value <= dateTo.AddDays(1));
                }
                if (!String.IsNullOrEmpty(userName))
                    q = q.Where(x => x.UserName == userName);

                // trwajaca 
                //if (isFinished.HasValue && isFinished.Value == false)
                //    q = q.Where(x => x.EndingInfo.HasValue == null || (x.EndingInfo.HasValue && x.EndingInfo.Value == 1));
                //if (isFinished.HasValue && isFinished.Value == true)
                //    q = q.Where(x => x.EndingInfo.HasValue != null && x.EndingInfo.Value != 1);
                //if (isSold.HasValue && isSold.Value)
                //    q = q.Where(x => x.BuyNowPrice > 0 && x.BidCount > 0);
                if (isSold.HasValue && !isSold.Value)
                    q = q.Where(x => x.BidCount == 0);
                if (isPromoted.HasValue && isPromoted.Value)
                    q = q.Where(x => x.IsPromoted.Value);
                if (isPromoted.HasValue && !isPromoted.Value)
                    q = q.Where(x => !x.IsPromoted.Value);
                //if (isAuction.HasValue)
                //    q = q.Where(x => x.IsAuction.Value == isAuction.Value);
                if (categoryType != 0)
                {
                    int[] categoryTypes;
                    if (categoryType == 1)
                        categoryTypes = new int[] { 2, 3, 4 };
                    else
                        categoryTypes = new int[] { categoryType };

                    q = q.Where(x => categoryTypes.Contains(x.CategoryTypeId.Value));
                }
                return q.OrderByDescending(x=>x.EndingDateTime).Take(1000).ToList();
            }
        }

        //public List<AllegroMyAuctionsResult> GetAllegroMyAuctions()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return ctx.AllegroMyAuctions().ToList();
        //    }
        //}
 
        //public List<AllegroSiteJournalDeal> GetJournalDealsToProcess(int[] eventTypes)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {

        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<AllegroSiteJournalDeal>(x => x.AllegroUser);
        //        ctx.LoadOptions = dlo;
        //        return ctx.AllegroSiteJournalDeals.Where(x =>
        //            x.IsProcessed == false
        //            && eventTypes.Contains(x.DealEventType)).ToList();

        //    }
        //}
        //public List<AllegroPaymentDetail> GetPaymentDetailsFromAllegro(long p)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return ctx.AllegroPaymentDetails.Where(x => x.PaymentTransactiondId == p).ToList();
        //    }
        //}

    }
}
