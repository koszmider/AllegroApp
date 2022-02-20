using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class KingHomeHelper
    {
        public void SetUpdateProducts(List<ProductCatalog> products)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
 



                List<int> productIds = new List<int>();


                 StringBuilder sb = new StringBuilder();

                foreach (Dal.ProductCatalog pc in products.Where(x=>x.Ean!=null).ToList())
                {
                    int pId = 0;
                    Dal.ProductCatalog pcToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == pc.SupplierId && x.Ean == pc.Ean).FirstOrDefault();

                    if(pc.Ean!=null)
                    sb.AppendLine(String.Format("{0} {1}", (pc.Ean==null?"             ": pc.Ean), pc.Code));


                    try
                    {
                        if (pcToUpdate == null)
                        {   //  ctx.ProductCatalog.InsertOnSubmit(pc);
                            continue;
                        }
                        else
                        {
                            pcToUpdate.IsAvailable = pc.IsAvailable;
                            pcToUpdate.IsDiscontinued = pc.IsDiscontinued;
                            pcToUpdate.PriceBruttoFixed = pc.PriceBruttoFixed;
                            pcToUpdate.PriceBruttoPromo = pc.PriceBruttoPromo;
                            pcToUpdate.PriceBruttoPromoDate = pc.PriceBruttoPromoDate;
                            pcToUpdate.IsActivePricePromo = pc.IsActivePricePromo;
                            pcToUpdate.SupplierQuantity = pc.SupplierQuantity;
                            pcToUpdate.DeliveryId = pc.DeliveryId;
                            pcToUpdate.Code = pc.Code;

                            //if (pcToUpdate.Ean == null && !String.IsNullOrEmpty(pc.Ean))
                            //    pcToUpdate.Ean = pc.Ean;

                            pId = pcToUpdate.ProductCatalogId;
                            ctx.SubmitChanges();
                            //if (pId == 0)
                            //    pId = pc.ProductCatalogId;

                            productIds.Add(pId);

                        }
                    }
                    catch (Exception ex)
                    {
                       
                        Dal.ErrorHandler.LogError(ex, String.Format("Błąd aktualizacji produktu {0}, kod {1}, ean {2}", pc.ProductCatalogId, pc.Code, pc.Ean));

                    }
 
                }

                foreach (Dal.ProductCatalog pc in products.Where(x => x.Ean != null).ToList())
                {
                    int pId = 0;
                    Dal.ProductCatalog pcToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == pc.SupplierId && x.Ean == pc.Ean).FirstOrDefault();

              
                    try
                    {
                        if (pcToUpdate == null)
                        {
                            ctx.ProductCatalog.InsertOnSubmit(pc);
                            ctx.SubmitChanges();

                            pId = pc.ProductCatalogId;

                            productIds.Add(pId);

                        }
                    }
                    catch (Exception ex)
                    {

                        Dal.ErrorHandler.LogError(ex, String.Format("Błąd aktualizacji produktu {0}, kod {1}, ean {2}", pc.ProductCatalogId, pc.Code, pc.Ean));

                    }
                }


                string t = sb.ToString();
                Dal.ProductCatalogHelper pch = new ProductCatalogHelper();
                Dal.SupplierOwner so = pch.GetSupplierOwner("King Home");

                if(so==null)
                {


                }
                { 
                List<Dal.ProductCatalog> productsToDisable = ctx.ProductCatalog.Where(x => x.Supplier.SupplierOwnerId==so.SupplierOwnerId ).ToList();

                foreach(Dal.ProductCatalog pc in productsToDisable)
                {
                    if(!productIds.Contains(pc.ProductCatalogId))
                    {
                        pc.IsAvailable = false;
                            pc.SupplierQuantity = 0;
                            pc.UpdateReason = "Brak informacji w pliku aktualizującym";
                        pc.UpdateUser = "system";
                    }
                }
                ctx.SubmitChanges();
                }
            }
        }
    }
}
