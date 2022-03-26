using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class PromoHelper
    {
        public int[] GetPromotionProductsIdsFromCatalog()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalog.Where(x => x.IsActivePricePromo == false && (x.PriceBruttoPromoDate != null || x.PriceBruttoPromo != null)).Select(x => x.ProductCatalogId).ToArray();
            }
        }

        public int[] GetPromotionProductsIds(int promoId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.PromoProduct.Where(x => x.PromotionId == promoId).Select(x => x.ProductCatalogId).ToArray();
            }
        }

        public List<PromoProduct> GetPromotionProductsIdsList(int promoId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.PromoProduct.Where(x => x.PromotionId == promoId).ToList();
            }
        }

        public List<Promo> GetActiveNotStartedPromotions()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Promo.Where(x => x.IsActive == true && x.IsGoingOn == false).ToList();
            }
        }

        public List<Promo> GetPromotions()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Promo.ToList();
            }
        }

        public List<Promo> GetActivePromotions()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Promo.Where(x => x.IsActive == true).ToList();
            }
        }

        public int UpdatePromotion(Promo promotion)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Promo promo = ctx.Promo.Where(x => x.PromotionId == promotion.PromotionId).FirstOrDefault();

                promo.IsActive = promotion.IsActive;
                promo.IsGoingOn = promotion.IsGoingOn;
                promo.StartDate = promotion.StartDate;
                promo.EndDate = promotion.EndDate;
                promo.PercentValue = promotion.PercentValue;
                promo.Description = promotion.Description;
                ctx.SubmitChanges();

                return promotion.PromotionId;
            }
        }

        public void DeletePromotion(Promo promotion)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<PromoProduct> list = GetPromotionProductsIdsList(promotion.PromotionId);
                foreach (PromoProduct pr in list)
                {
                    PromoProduct propro = ctx.PromoProduct.Where(x => x.ProductCatalogId == pr.ProductCatalogId && x.PromotionId == pr.PromotionId).FirstOrDefault();
                    ctx.PromoProduct.DeleteOnSubmit(propro);
                }

                Promo pro = ctx.Promo.Where(x => x.PromotionId == promotion.PromotionId).FirstOrDefault();
                ctx.Promo.DeleteOnSubmit(pro);

                ctx.SubmitChanges();
            }
        }

        public int AddPromotion(Promo promotion)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.Promo.InsertOnSubmit(promotion);
                ctx.SubmitChanges();

                return promotion.PromotionId;
            }
        }

        public void RemovePromotionProducts(int promoId, List<int> intList)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<PromoProduct> tmp = ctx.PromoProduct.Where(x => x.PromotionId == promoId && intList.Contains(x.ProductCatalogId)).ToList();
                foreach (PromoProduct t in tmp)
                {
                    ctx.PromoProduct.DeleteOnSubmit(t);
                }

                ctx.SubmitChanges();
            }
        }

        public void AddPromotionProducts(List<PromoProduct> list)
        {
            if (list.Count() == 0) return;

            using (LajtitDB ctx = new LajtitDB())
            {
                List<Promo> active = GetActivePromotions();
                active = active.Where(x => x.PromotionId != list.First().PromotionId).ToList();
                foreach (Promo promo in active)
                {
                    List<int> intList = list.Select(x => x.ProductCatalogId).ToList();
                    RemovePromotionProducts(promo.PromotionId, intList);
                }

                foreach (PromoProduct pr in list)
                {

                    ctx.PromoProduct.InsertOnSubmit(pr);
                }
                
                ctx.SubmitChanges();
            }
        }

        public Promo GetPromotion(int promotionId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Promo.Where(x => x.PromotionId == promotionId).FirstOrDefault();
            }
        }



        public int AddUpdate(Update update)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.Update.InsertOnSubmit(update);
                ctx.SubmitChanges();

                return update.UpdateId;
            }
        }

        public int UpdateUpdate(Update update)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Update upd = ctx.Update.Where(x => x.UpdateId == update.UpdateId).FirstOrDefault();

                upd.IsActive = update.IsActive;
                ctx.SubmitChanges();

                return update.UpdateId;
            }
        }

        public void DeleteUpdate(Update update)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Update upd = ctx.Update.Where(x => x.FileId == update.FileId).FirstOrDefault();
                ctx.Update.DeleteOnSubmit(upd);

                ctx.SubmitChanges();
            }
        }

        public List<Update> GetAllUpdates()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Update.ToList();
            }
        }

        public List<Update> GetNotActiveUpdates()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Update.Where(x => x.IsActive == false).ToList();
            }
        }

        public List<Update> GetActiveUpdates()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Update.Where(x => x.IsActive == true).ToList();
            }
        }

        public Update GetUpdate(int updateId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Update.Where(x => x.UpdateId == updateId).FirstOrDefault();
            }
        }
    }
}
