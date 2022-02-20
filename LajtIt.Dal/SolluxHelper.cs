using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class SolluxHelper
    {
        public string[] SetUpdateProducts(List<ProductCatalog> products)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                string[] ean = products.Select(x => x.Ean).Distinct().ToArray();
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog
                    .Where(x => x.SupplierId == 31)
                    .ToList();
                List<string> eanIdDb = new List<string>();
                foreach(ProductCatalog pcToUpdate in productsToUpdate)
                {
                    ProductCatalog p = products.Where(x => x.Ean == pcToUpdate.Ean).FirstOrDefault();

                    if (p != null)
                    {
                        pcToUpdate.IsAvailable = p.IsAvailable;
                        pcToUpdate.ExternalId = p.ExternalId;
                        pcToUpdate.PriceBruttoFixed = p.PriceBruttoFixed;
                        eanIdDb.Add(p.Ean);
                    }
                    else
                    {
                        pcToUpdate.IsAvailable = false;
                    }
                    pcToUpdate.UpdateReason = "Aktualizacja statusów";
                    pcToUpdate.UpdateUser = "System";

                    ctx.SubmitChanges();

                }

                string[] eanToAdd = ean.Where(x => !eanIdDb.Contains(x)).ToArray();

                return eanToAdd;
            }

        }
    }
}
