using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
    public class AllegroHelper
    {
        public class Payments
        {
            public static bool SetPayments(long userId, List<AllegroPayments> payments)
            {
                using(LajtitAllegroDB ctx = new LajtitAllegroDB())
                {

                    foreach(Dal.AllegroPayments p in payments)
                    {
                        Dal.AllegroPayments existingPayment = ctx.AllegroPayments.Where(x => x.UserId == userId
                        && (
                         (x.PaymentId.HasValue && x.PaymentId == p.PaymentId && x.BuyerUserId == p.BuyerUserId)
                         ||

                         (!x.PaymentId.HasValue && x.OccuredAt == p.OccuredAt && x.Amount == p.Amount)
                         )
                         ).FirstOrDefault();

                        if (existingPayment == null)
                        {
                            ctx.AllegroPayments.InsertOnSubmit(p);
                            ctx.SubmitChanges();
                        }
                      //  else
                      //      return true;

                    }
                    return false;
                }
            }
        }

        public static List<AllegroItem> GetAllegroItems(long userId, string status)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroItem.Where(x => x.UserId == userId && x.ItemStatus == status).ToList();
            }
        }

        public static List<ProductCatalogAllegroItemsWithoutProducsView> GetAllegroItemsWithoutProducs()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogAllegroItemsWithoutProducsView.ToList();
            }
        }
        public static void SetAllegroProduct(string ean, Guid id, string json, bool? isValid)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                ctx.AllegroProductSet(ean, id, json, isValid);

            }
        }
        public static void SetAllegroProductToProductCatalog(long itemId, Guid id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.AllegroProductToProductCatalogSet(itemId, null, id, null);
            }
        }

        public static List<AllegroProduct> GetAllegroProductsToUpdate(int limit)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroProduct
                    .Where(x => x.ProductId.HasValue == true)
                    .OrderBy(x => x.LastUpdateDate)
                    .Take(limit).ToList();
            }
        }
        public static List<AllegroProduct> GetAllegroProductsToCheck(int limit)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroProduct
                    .Where(x => x.IsFound.HasValue==false)
                    .OrderBy(x => x.LastUpdateDate)
                    .Take(limit).ToList();
            }
        }

        public static List<AllegroItem> GetAllegroItemsWithHasProductFlag()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroItem
                    .Where(x => x.AllegroUser.MyUserId == true && x.HasProductId.HasValue && x.ProductId.HasValue==false)
                    .ToList();
            }
        }

        public static void SetAllegroItemProduct( long itemId, Guid id)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                var ai = ctx.AllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();

                ai.ProductId = id;
                ctx.SubmitChanges();
            }
        }

        public static List<AllegroItemsRestoreView> GetAllegroItemsRestore(long userId)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                var items = ctx.AllegroItemsRestoreView.Where(x=>x.UserId== userId).Take(300).ToList();

                long[] itemIds = items.Select(x => x.ItemId).ToArray();
                List<Dal.AllegroItem> itemsToUpdate = ctx.AllegroItem.Where(x => itemIds.Contains(x.ItemId)).ToList();

                foreach (Dal.AllegroItem item in itemsToUpdate)
                {
                    item.ItemStatus = "ACTIVATE";
                }
                ctx.SubmitChanges();

                return items;
            }
        }

        public static IEnumerable<AllegroUser> GetAllegroMyUsers()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroUser.Where(x => x.MyUserId == true).ToList();
            }
            }

        public static List<ProductCatalogAllegroItemActivatingView> GetProductCatalogAllegroItemActivatingView()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogAllegroItemActivatingView.ToList();
            }
        }

        public static Order GetAllegroOrder(string allegroOrderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Order.Where(x => x.ExternalOrderNumber == allegroOrderId && x.Shop.ShopTypeId == (int)Dal.Helper.ShopType.Allegro).FirstOrDefault();
            }
        }
    }
}
