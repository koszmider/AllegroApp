using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
    public partial class ProductCatalog
    {
        public class Allegro
        {
            public static List<ProductCatalogAllegroItemsView> GetProductCatalogAllegroItemsToUpdate(string itemStatus, int limit)
            {
                return GetProductCatalogAllegroItemsToUpdate(new string[] { itemStatus }, limit);

            }
            public static List<ProductCatalogAllegroItemsView> GetProductCatalogAllegroItemsToUpdate(string[] itemStatus, int limit)
            {
                using (LajtitViewsDB ctx = new LajtitViewsDB())
                {
                    List<Dal.ProductCatalogAllegroItemsView> items = ctx.ProductCatalogAllegroItemsView
                        .Where(x => itemStatus.Contains(x.ItemStatus) && x.ProcessId == null && x.IsValid == false //&& x.IsImageReady == true
                         //&& x.SupplierId == 87  
                         )
                        .OrderBy(x => x.ValidatedAt)
                        .Take(limit)  
                        .ToList();


                    Console.WriteLine(String.Format("Items: {0}", items.Count()));


                    //(limit, itemStatus, null, true, null)
                    //.Where(x => x.IsValid == null || x.IsValid.Value == false)
                    ////.Take(limit)
                    //.ToList();

                    return items;
                }
            }
            public static List<ProductCatalogAllegroItemsView> GetProductCatalogAllegroItemsToUpdate(string[] itemStatus, int limit, int productCatalogId)
            {
                using (LajtitViewsDB ctx = new LajtitViewsDB())
                {
                    List<Dal.ProductCatalogAllegroItemsView> items = ctx.ProductCatalogAllegroItemsView
                        .Where(x => itemStatus.Contains(x.ItemStatus) && x.ProcessId == null
                        //&& x.IsValid == false && x.IsImageReady == true 
                        && x.ProductCatalogId == productCatalogId)
                        .OrderBy(x => x.ValidatedAt)
                        .Take(limit)
                        .ToList();
                    //(limit, itemStatus, null, true, null)
                    //.Where(x => x.IsValid == null || x.IsValid.Value == false)
                    ////.Take(limit)
                    //.ToList();

                    return items;
                }
            }
            public static List<ProductCatalogAllegroItemsView> GetProductCatalogAllegroItemsToUpdate(string itemStatus, int limit, Guid processId)
            {
                using (LajtitViewsDB ctx = new LajtitViewsDB())
                {
                    List<Dal.ProductCatalogAllegroItemsView> items = ctx.ProductCatalogAllegroItemsView
                        .Where(x => x.ItemStatus == itemStatus && x.ProcessId == processId && x.IsValid == false// && x.IsImageReady == true
                        )
                        .OrderBy(x => x.ValidatedAt)
                        .Take(limit)
                        .ToList();
                    //(limit, itemStatus, null, true, null)
                    //.Where(x => x.IsValid == null || x.IsValid.Value == false)
                    ////.Take(limit)
                    //.ToList();

                    return items;
                }
            }
            //public static List<ProductCatalogAllegroItemsView> GetProductCatalogItemsForImageUpdate(int limit)
            //{
            //    using (LajtitViewsDB ctx = new LajtitViewsDB())
            //    {

            //        string[] itemStatus = new string[] { "INACTIVE", "ACTIVE" };

            //        List<Dal.ProductCatalogAllegroItemsView> items = ctx.ProductCatalogAllegroItemsView
            //            .Where(x => itemStatus.Contains(x.ItemStatus) && x.ProcessId == null && x.IsImageReady == false
            //           // && x.SupplierId== 87  
            //            )
            //            .OrderBy(x => x.ValidatedAt)
            //            .Take(limit) 
            //            .ToList();

            //        Console.WriteLine(String.Format("Images: {0}", items.Count()));
            //      // return null;
            //        //List<Dal.ProductCatalogAllegroItemsFnResult> items = ctx.ProductCatalogAllegroItemsFn(limit, "INACTIVE", null, false, null)
            //        //    .OrderBy(x => x.ValidatedAt)
            //        //    .ToList();

            //        //if (items.Count() == limit)
            //        //    return items;

            //        //items.AddRange(ctx.ProductCatalogAllegroItemsFn(limit, "ACTIVE", null, false, null)
            //        //    .OrderBy(x => x.ValidatedAt));//.Take(limit - items.Count())) ;// ;// ;


            //        return items;
            //    }
            //}
            //public static List<ProductCatalogAllegroItemsView> GetProductCatalogItemsForImageUpdate(Guid processId)
            //{
            //    using (LajtitViewsDB ctx = new LajtitViewsDB())
            //    {

            //        string[] itemStatus = new string[] { "INACTIVE", "ACTIVE" };

            //        List<Dal.ProductCatalogAllegroItemsView> items = ctx.ProductCatalogAllegroItemsView
            //            .Where(x => itemStatus.Contains(x.ItemStatus) && x.ProcessId == processId && x.IsImageReady == false)
            //            .OrderBy(x => x.ValidatedAt)
            //            .ToList();


            //        return items;
            //    }
            //}
            public static List<ProductCatalogAllegroItemsActive> GetProductCatalogAllegroItemsByPcId(int productCatalogId)
            {
                using (LajtitViewsDB ctx = new LajtitViewsDB())
                {
                    return ctx.ProductCatalogAllegroItemsActive.Where(x => x.ProductCatalogId == productCatalogId).ToList();
                }
            }
            public static List<ProductCatalogAllegroItemsView> GetProductCatalogAllegroItemsForPublishing()
            {
                using (LajtitViewsDB ctx = new LajtitViewsDB())
                {
                    List<Dal.ProductCatalogAllegroItemsView> items = ctx.ProductCatalogAllegroItemsView
                        .Where(x => x.ItemStatus == "INACTIVE" && x.IsValid == true /*&& x.IsImageReady == true*/ && x.CommandId.HasValue == false
                       // && x.SupplierId == 87  
                        )
                        .Take(300)
                        .ToList();

                    Console.WriteLine(String.Format("Images: {0}", items.Count()));
                    return items;

                    //return ctx.ProductCatalogAllegroItemsFn(300, "INACTIVE", null, true, null)
                    //.Where(x =>
                    //x.IsValid.HasValue
                    //&& !x.CommandId.HasValue
                    //&& x.IsImageReady
                    //&& x.IsValid.Value
                    //)
                    //    .ToList();
                }
            }

            public static void SetProductCatalogAllegroItemsWithErrorsSent(List<ProductCatalogAllegroItemErrorsView> errors)
            {
                using (LajtitDB ctx = new LajtitDB())
                {
                    foreach(ProductCatalogAllegroItemErrorsView item in errors)
                    {
                        Dal.ProductCatalogAllegroItem itemToUpdate = ctx.ProductCatalogAllegroItem.Where(x => x.ItemId == item.ItemId).FirstOrDefault();

                        itemToUpdate.NotificationSent = true;
                    }
                    ctx.SubmitChanges();
                }
            }

            public static List<ProductCatalogAllegroItemErrorsView> GetProductCatalogAllegroItemsWithErrors()
            {
                using (LajtitViewsDB ctx = new LajtitViewsDB())
                {
                    return ctx.ProductCatalogAllegroItemErrorsView
                        .OrderBy(x => x.ValidatedAt)
                        .ToList();
                }
            }

            public static void SetProductCatalogAllegroItemReActive(long[] itemIds,  string updateCommand)
            {
                using (LajtitAllegroDB ctxAllegro = new LajtitAllegroDB())
                {
                    using (LajtitDB ctx = new LajtitDB())
                    {

                        List<ProductCatalogAllegroItem> pcaiToUpdate =
                            ctx.ProductCatalogAllegroItem.Where(x => itemIds.Contains(x.ItemId)).ToList();

                        foreach (Dal.ProductCatalogAllegroItem item in pcaiToUpdate)
                        {
                            item.CommandId = null;
                            item.ProcessId = null;
                            //item.IsImageReady = false;
                            item.IsValid = false;
                            item.Comment = "Reaktywacja";
                            item.UpdateCommand = updateCommand;

                        } 
                        List<AllegroItem> itemsToUpdate =
                            ctxAllegro.AllegroItem.Where(x => itemIds.Contains(x.ItemId)).ToList();

                        foreach (Dal.AllegroItem item in itemsToUpdate)
                        {
                            item.ItemStatus = "INACTIVE";
                            item.LastUpdateDate = DateTime.Now;
                        }

                        ctx.SubmitChanges();
                        ctxAllegro.SubmitChanges();
                    }
                }
            }
            public static void SetProductCatalogAllegroItemPublish(long[] itemIds, string status, Guid? commandId, string comment, string updateCommand)
            {
                using (LajtitAllegroDB ctxAllegro = new LajtitAllegroDB())
                {
                    using (LajtitDB ctx = new LajtitDB())
                    {

                        List<ProductCatalogAllegroItem> pcaiToUpdate =
                            ctx.ProductCatalogAllegroItem.Where(x => itemIds.Contains(x.ItemId)).ToList();

                        foreach (Dal.ProductCatalogAllegroItem item in pcaiToUpdate)
                        {
                            item.CommandId = commandId;
                            item.Comment = comment;
                            item.ProcessId = null;

                            //if (status == "INACTIVE")
                            //{
                            //    item.UpdateCommand = updateCommand;
                            //    item.IsImageReady = false;
                            //    //item.Comment = String.Format("SetProductCatalogAllegroItemPublish IsImageReady = false" );
                            //    item.IsValid = false;
                            //}
                        }



                        List<AllegroItem> itemsToUpdate =
                            ctxAllegro.AllegroItem.Where(x => itemIds.Contains(x.ItemId)).ToList();

                        foreach (Dal.AllegroItem item in itemsToUpdate)
                        {
                            item.ItemStatus = status;
                            item.LastUpdateDate = DateTime.Now;
                        }

                        ctx.SubmitChanges();
                        ctxAllegro.SubmitChanges();
                    }
                }
            }

            public static List<ProductCatalogAllegroItemsToReActivateView> GetProductCatalogAllegroItemsToReActivate()
            {
                using (LajtitViewsDB ctx = new LajtitViewsDB())
                {
                    return ctx.ProductCatalogAllegroItemsToReActivateView.ToList();
                }
            }

            public static List<ProductCatalogAllegroItemsView> GetProductCatalogAllegroItems(string itemStatus, string searchText, bool? isValid, bool? isImageReady, string errorMsg, long[] userIds)
            {
                using (LajtitViewsDB ctx = new LajtitViewsDB())
                {

                    IQueryable<Dal.ProductCatalogAllegroItemsView> q;

                    //if (itemStatus == "NOTCREATED")
                    //    q = GetItems(ctx.ProductCatalogAllegroDraftItemsFn());
                    //else
                    q = ctx.ProductCatalogAllegroItemsView.Where(x=>x.ItemStatus == itemStatus);

                    if (isValid.HasValue)
                        q = q.Where(x => x.IsValid == isValid);

                    //if (isImageReady.HasValue)
                    //    q = q.Where(x => x.IsImageReady == isImageReady);

                    // (1000, itemStatus, null, isValid, null);

                    if (itemStatus == "ACTIVE")
                        q = q.Where(x => x.UpdateCommand != null);

                    if (searchText != "")
                        q = q.Where(x => x.ItemId.ToString() == searchText
                        || x.ProductName.Contains(searchText.Trim())
                        || x.Ean == searchText.Trim());

                    if (!String.IsNullOrEmpty(errorMsg))
                        q = q.Where(x => x.Comment != null && x.Comment.ToLower().Contains(errorMsg.ToLower().Trim()));

                    if (userIds.Length > 0)
                        q = q.Where(x => userIds.Contains(x.UserId));

                    return q.Take(1000).ToList();
                }
            }
            public static List<ProductCatalogAllegroItemsDraftView> GetProductCatalogAllegroItemsDraft()
            {
                using (LajtitViewsDB ctx = new LajtitViewsDB())
                {
                    return ctx.ProductCatalogAllegroItemsDraftView.ToList();
                }
            }
            public static void SetProductCatalogAllegroItemProcess(Guid processId, long[] itemIds)
            {
                using (LajtitDB ctx = new LajtitDB())
                {
                    ctx.ProductCatalogAllegroItemProcessSet(String.Join(",", itemIds.Select(x => x.ToString()).ToArray()), processId);
                    //List<Dal.ProductCatalogAllegroItem> items = ctx.ProductCatalogAllegroItem
                    //    .Where(x => itemIds.Contains(x.ItemId))
                    //    .ToList();
                    //foreach (Dal.ProductCatalogAllegroItem item in items)
                    //    item.ProcessId = processId;

                    //ctx.SubmitChanges();
                }
            }
            public static void SetProductCatalogAllegroItemProcessClear(Guid processId, long[] itemIds)
            {
                using (LajtitDB ctx = new LajtitDB())
                {
                    List<Dal.ProductCatalogAllegroItem> items = ctx.ProductCatalogAllegroItem
                        .Where(x => x.ProcessId == processId && itemIds.Contains(x.ItemId))
                        .ToList();

                    foreach (Dal.ProductCatalogAllegroItem item in items)
                        item.ProcessId = null;

                    ctx.SubmitChanges();
                }
            }
            public static List<ProductCatalogAllegroItemsView> GetProductCatalogAllegroItemsToUpdate(int productCatalogId)
            {
                using (LajtitViewsDB ctx = new LajtitViewsDB())
                {
                    List<Dal.ProductCatalogAllegroItemsView> items = ctx.ProductCatalogAllegroItemsView
                        .Where(x => x.ItemStatus == "ACTIVE" && x.ProcessId == null /*&& x.IsImageReady == true */ && x.ProductCatalogId == productCatalogId)
                        .Take(100)
                        .ToList();

                    return items;
                    //return ctx.ProductCatalogAllegroItemsFn(100, "ACTIVE", productCatalogId, true, null)                    .ToList();
                }
            }

            public static void SetProductCatalogAllegroItemRefreshImages(long itemId)
            {
                using (LajtitDB ctx = new LajtitDB())
                {

                    ProductCatalogAllegroItem item =
                        ctx.ProductCatalogAllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();

                    if (item != null)
                    {
                        item.CommandId = null;
                        item.Comment = "odświeżenie zdjęć";
                        item.ProcessId = null;
                        item.UpdateCommand = "10000000000000";
                        //item.IsImageReady = false;
                        item.IsValid = false;
                    }
                }

            }
        }
    }
}