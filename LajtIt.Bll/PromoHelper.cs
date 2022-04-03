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
            GarbagePromosCleaner();

            Dal.PromoHelper ph = new Dal.PromoHelper();

            List<Promo> activeNotStarted = ph.GetActiveNotStartedPromotions();
            List<Promo> active = ph.GetActivePromotions();

            foreach (Promo p in activeNotStarted)
            {
                if (p.StartDate < DateTime.Now)
                {
                    int[] productIds = ph.GetPromotionProductsIds(p.PromotionId);
                    StartPromotion(p, productIds);
                    p.IsGoingOn = true;
                    ph.UpdatePromotion(p);
                }
            }

            foreach (Promo p in active)
            {
                if (p.EndDate < DateTime.Now)
                {
                    int[] productIds = ph.GetPromotionProductsIds(p.PromotionId);
                    StopPromotion(p, productIds);
                    ph.DeletePromotion(p);
                }
            }

        }

        private void GarbagePromosCleaner()
        {
            Dal.PromoHelper ph = new Dal.PromoHelper();

            int[] pIds = ph.GetPromotionProductsIdsFromCatalog();

            Dal.ProductCatalog pc = new Dal.ProductCatalog();
            pc.LockRebates = null;
            pc.IsPaczkomatAvailable = null;
            pc.IsOutlet = null;
            pc.PriceBruttoPromo = null;
            pc.PriceBruttoPromoDate = null;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogSettings(pIds, pc, false, "System");
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
            pch.SetProductCatalogSettings(productIds, pc, false, "System");
        }

        public void StopPromotion(Promo promo, int[] productIds)
        {
            Dal.ProductCatalog pc = new Dal.ProductCatalog();

            pc.IsOutlet = null;
            pc.LockRebates = null;
            pc.IsPaczkomatAvailable = null;

            pc.PriceBruttoPromo = 0;
            pc.PriceBruttoPromoDate = null;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogSettings(productIds, pc, false, "System");
        }

        public void AddPromotion(Dal.Promo p, int[] ids)
        {

        }



        public void ManageUpdates()
        {
            Dal.PromoHelper ph = new Dal.PromoHelper();
            Dal.ProductFileImportHelper pf = new Dal.ProductFileImportHelper();

            List<Update> activeUpdates = ph.GetActiveUpdates();
            List<Update> notActiveUpdates = ph.GetNotActiveUpdates();

            foreach (Update update in activeUpdates)
            {
                Dal.ProductCatalogFile file = pf.GetFile(update.FileId);
                if (file.FileImportStatusId == (int)Dal.Helper.FileImportStatus.Imported)
                    ph.DeleteUpdate(update);
            }

            foreach (Update update in notActiveUpdates)
            {
                if (update.StartDate < DateTime.Now)
                {
                    SetReadyToImportFileStatus(update);
                }
            }
        }

        private void SetReadyToImportFileStatus(Update update)
        {
            Dal.PromoHelper ph = new Dal.PromoHelper();

            update.IsActive = true;
            ph.UpdateUpdate(update);

            Dal.ProductCatalogFile pcf = new Dal.ProductCatalogFile()
            {
                FileImportStatusId = (int)Dal.Helper.FileImportStatus.ReadyToImport,
                ProductCatalogFileId = update.FileId

            };

            Dal.ProductFileImportHelper pf = new Dal.ProductFileImportHelper();
            pf.SetFileUpdateStatus(pcf);
        }
    }
}
