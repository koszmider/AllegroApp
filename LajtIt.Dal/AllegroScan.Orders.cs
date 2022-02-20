using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace LajtIt.Dal
{
    public partial class AllegroScan
    {


        public List<AllegroItemTransactionItem> GetItemTransactions(string email)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<AllegroItemTransactionItem>(x => x.AllegroItemTransaction);


                ctx.LoadOptions = dlo;

                long? userId = ctx.AllegroItemOrder.Where(x => x.Email == email).Select(x => x.UserId).FirstOrDefault();
                if (!userId.HasValue)
                    return new List<AllegroItemTransactionItem>();

                long[] itemIDs = ctx.AllegroItemOrder.Where(x => x.Email == email).Select(x => x.ItemId).Distinct().ToArray();
                //dd
                return ctx.AllegroItemTransactionItem.Where(x => itemIDs.Contains(x.ItemId)).ToList();
            }
        }

        public List<AllegroUser> GetAllegroMyUsers()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroUser.Where(x => x.MyUserId == true).ToList();

            }
        }

        public List<AllegroItemOrder> GetItemOrders(string Email, int orderStatusId)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroItemOrder.Where(x => x.Email == Email).OrderByDescending(x => x.OrderDateTime).ToList();

            }
        }

        public void SetAllegroUserUpdateToken(AllegroUser allegroUser)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                AllegroUser au = ctx.AllegroUser.Where(x => x.UserId == allegroUser.UserId).FirstOrDefault();


                au.Token = allegroUser.Token;
                au.TokenCreateDate = allegroUser.TokenCreateDate;
                au.TokenEndDate = allegroUser.TokenEndDate;
                au.TokenRefresh = allegroUser.TokenRefresh;

                ctx.SubmitChanges();

            }
        }

        public AllegroEmail GetAllegroEmailLast()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroEmail.OrderByDescending(x => x.SentDate).FirstOrDefault();
            }
        }


        public void SetProductCatalogImageAllegroItem(ProductCatalogImageAllegroItem iu)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogImageAllegroItem imageToUpdate = ctx.ProductCatalogImageAllegroItem.Where(x => x.ItemId == iu.ItemId && x.ImageId == iu.ImageId).FirstOrDefault();

                if (imageToUpdate == null)
                    ctx.ProductCatalogImageAllegroItem.InsertOnSubmit(iu);
                else
                {
                    imageToUpdate.ExpireDate = iu.ExpireDate;
                    imageToUpdate.LocationUrl = iu.LocationUrl;
                }
                ctx.SubmitChanges();

            }
        }

        public void SetAllegroEmail(AllegroEmail ae)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                ctx.AllegroEmail.InsertOnSubmit(ae);
                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogImageAllegroItem> GetProductCatalogImageAllegroItem(long itemId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogImageAllegroItem.Where(x => x.ItemId == itemId).OrderBy(x=>x.ProductCatalogImage.Priority) .ToList();
            }
        }

        public AllegroItemUserView GetAllegroItemUser(long itemId)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroItemUserView.Where(x => x.ItemId == itemId).FirstOrDefault();
            }
        }

        public void CreateMissingOffers(long userId, List<AllegroItem> allegroItems, List<ProductCatalogAllegroItem> pcAllegroItems)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                long[] itemIds = allegroItems.Select(x => x.ItemId).ToArray();

                long[] itemIdsExisting = ctx.AllegroItem.Where(x => itemIds.Contains(x.ItemId)).Select(x => x.ItemId).ToArray();

                List<AllegroItem> itemsToAdd = allegroItems.Where(x => !itemIdsExisting.Contains(x.ItemId)).ToList();

                ctx.AllegroItem.InsertAllOnSubmit(itemsToAdd);

                ctx.SubmitChanges();
            }
            using (LajtitDB ctx = new LajtitDB())
            {
                long[] itemIds = pcAllegroItems.Select(x => x.ItemId).ToArray();

                long[] itemIdsExisting = ctx.ProductCatalogAllegroItem.Where(x =>itemIds.Contains(x.ItemId)).Select(x => x.ItemId).ToArray();

                List<ProductCatalogAllegroItem> itemsToAdd = pcAllegroItems.Where(x => !itemIdsExisting.Contains(x.ItemId)).ToList();

                ctx.ProductCatalogAllegroItem.InsertAllOnSubmit(itemsToAdd);

                ctx.SubmitChanges();
            }
        }

        public void SetAllegroEmailReplied(int id)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                Dal.AllegroEmail ae = ctx.AllegroEmail.Where(x => x.Id == id).FirstOrDefault();
                ae.IsReplied = true;
                ctx.SubmitChanges();
            }
        }
    

    public List<AllegroEmail> GetAllegroEmails()
    {
        using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        {
            return ctx.AllegroEmail
                .Where(x => x.IsReplied == false && x.FromEmail == "powiadomienia@allegro.pl" && x.Subject.StartsWith("Pytanie o przedmiot"))
                .ToList();
        }
    }

        public void SetAllegroItemRefresh(List<AllegroItem> allegroItems, ref List<long> itemIdsOnAllegroNotIsDb)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                long[] itemIds = allegroItems.Select(x => x.ItemId).ToArray();

                List<AllegroItem> itemsToUpdate= ctx.AllegroItem.Where(x => itemIds.Contains(x.ItemId)).ToList();

                foreach (AllegroItem ai in allegroItems)
                {

                    AllegroItem itemToUpdate = itemsToUpdate.Where(x => x.ItemId == ai.ItemId).FirstOrDefault();

                    if (itemToUpdate != null)
                    {
                        itemToUpdate.Name = ai.Name;
                        itemToUpdate.CategoryId = ai.CategoryId;
                        itemToUpdate.EndingDate = ai.EndingDate;
                       // itemToUpdate.BidCount = ai.BidCount;
                        itemToUpdate.CurrentPrice = ai.CurrentPrice;
                        itemToUpdate.ItemStatus = ai.ItemStatus;
                        itemToUpdate.LastUpdateDate = ai.LastUpdateDate;
                        if (ai.HasProductId.HasValue)
                            itemToUpdate.HasProductId = ai.HasProductId;
                    }
                    else
                        itemIdsOnAllegroNotIsDb.Add(ai.ItemId);
                }
                ctx.SubmitChanges();
            }
        }
        public void SetAllegroItemRefreshImages(long[] itemIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (itemIds.Length == 0)
                    return;

                foreach (long itemId in itemIds)
                {

                    Dal.ProductCatalogAllegroItem item = ctx.ProductCatalogAllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();

                    if (item == null)
                        continue;
                    item.IsValid = false;
                    item.UpdateCommand = "000000100000";
                    item.ProcessId = null;
                    item.Comment = "Dogrywanie zdjęć";
                }
                ctx.SubmitChanges();
            }
        }
        public List<AllegroShipment> GetAllegroShipping()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroShipment.ToList();
            }
            }

        public List<long> GetAllegroInactiveItems()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroItem.Where(x => x.ItemStatus == "INACTIVE").Select(x => x.ItemId).ToList();
            }
        }

        //public void SetAllegroItemNotFoud(long itemId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        AllegroItem ai = ctx.AllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();

        //        if (ai != null)
        //        {
        //            ai.ItemStatus = "ENDED";
        //            ctx.SubmitChanges();
        //        }

        //    }
        //}
    }
}
