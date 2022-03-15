using LajtIt.Dal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Bll
{
    public class PromoHelper
    {
        public void ManagePromos()
        {
            Dal.PromoHelper pch = new Dal.PromoHelper();

            List<Promo> activeNotStarted = pch.GetActiveNotStartedPromotions();
            List<Promo> active = pch.GetActivePromotions();

            foreach (Promo p in activeNotStarted)
            {
                if (p.StartDate < DateTime.Now)
                {
                    int[] productIds = pch.GetPromotionProducts(p.PromotionId);
                    StartPromotion(p, productIds);
                    p.IsGoingOn = true;
                    pch.UpdatePromotion(p);
                }
            }

            foreach (Promo p in active)
            {
                if (p.EndDate < DateTime.Now)
                {
                    int[] productIds = pch.GetPromotionProducts(p.PromotionId);
                    StopPromotion(p, productIds);
                    pch.DeletePromotion(p);
                }
            }

        }
 
        private void StartPromotion(Promo promo, int[] productIds)
        {
            Dal.ProductCatalog pc = new Dal.ProductCatalog();

            pc.IsOutlet = null;
            pc.LockRebates = null;
            pc.IsPaczkomatAvailable = null;

            pc.PriceBruttoPromo = Convert.ToDecimal(promo.PercentValue) / 100;
            pc.PriceBruttoPromoDate = promo.EndDate.Value.AddHours(23).AddMinutes(59);

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogSettings(productIds, pc, false, "Administrator");
        }


        private void StopPromotion(Promo promo, int[] productIds)
        {
            Dal.ProductCatalog pc = new Dal.ProductCatalog();

            pc.IsOutlet = null;
            pc.LockRebates = null;
            pc.IsPaczkomatAvailable = null;

            pc.PriceBruttoPromo = 0;
            pc.PriceBruttoPromoDate = null;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogSettings(productIds, pc, false, "Administrator");
        }

    }
}
