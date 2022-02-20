using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class LightPrestigeHelper
    {
        public string[] SetUpdateProducts(List<ProductCatalog> products)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                string[] ean = products.Select(x => x.Ean).Distinct().ToArray();
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog
                    .Where(x => x.SupplierId == 55)
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
                        pcToUpdate.SupplierQuantity = p.SupplierQuantity;

                        if(pcToUpdate.PurchasePrice.HasValue == false && p.PurchasePrice.HasValue && p.PurchasePrice.Value > 0)
                            pcToUpdate.PurchasePrice = p.PurchasePrice.Value;
                        else
                        {
                            if(pcToUpdate.PurchasePrice.HasValue  && p.PurchasePrice.HasValue && p.PurchasePrice.Value > 0 
                                && Math.Abs(p.PurchasePrice.Value - pcToUpdate.PurchasePrice.Value) > (decimal)0.01)
                                    pcToUpdate.PurchasePrice = p.PurchasePrice.Value;
                        }

                        
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
