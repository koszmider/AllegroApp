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

        public int[] GetPromotionProducts(int promoId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.PromoProduct.Where(x => x.PromotionId == promoId).Select(x => x.ProductCatalogId).ToArray();
            }
        }

        public List<PromoProduct> GetPromotionProductsList(int promoId)
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
                List<PromoProduct> list = GetPromotionProductsList(promotion.PromotionId);
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



    }
}
