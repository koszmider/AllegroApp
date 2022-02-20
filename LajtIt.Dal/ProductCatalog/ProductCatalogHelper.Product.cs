using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
    partial class ProductCatalog
    {
        public static void SetProductCatalogPacking(ProductCatalogPacking packing)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (packing.Id == 0)
                    ctx.ProductCatalogPacking.InsertOnSubmit(packing);
                else
                {
                    Dal.ProductCatalogPacking packingToUpdate = ctx.ProductCatalogPacking.Where(x => x.Id == packing.Id).FirstOrDefault();

                    packingToUpdate.Height = packing.Height;
                    packingToUpdate.Length = packing.Length;
                    packingToUpdate.Size = packing.Size;
                    packingToUpdate.Weight = packing.Weight;
                    packingToUpdate.Width = packing.Width;
                    packingToUpdate.IsOversize = packing.IsOversize;

                }
                ctx.SubmitChanges();
            }
        }
        public static ProductCatalogPacking GetProductCatalogPacking(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogPacking.Where(x => x.Id == id).FirstOrDefault();
            }
        }
        public static void SetProductCatalogPackingDelete(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var p = ctx.ProductCatalogPacking.Where(x => x.Id == id).FirstOrDefault();

                ctx.ProductCatalogPacking.DeleteOnSubmit(p);

                ctx.SubmitChanges();
            }
        }
        public static List<ProductCatalogPacking> GetProductCatalogPackings(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogPacking.Where(x => x.ProductCatalogId == productCatalogId).ToList();
            }
        }
        public static Dal.ProductCatalogView GetProductCatalogByEan(string ean)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogView.Where(x => x.Ean != null && x.Ean == ean).FirstOrDefault();
            }
        }
        public static Dal.ProductCatalogView GetProductCatalogView(int productCatalogId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogView.Where(x => x.ProductCatalogId== productCatalogId).FirstOrDefault();
            }
        }
        public static Dal.ProductCatalog GetProductCatalog(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Dal.ProductCatalog>(x => x.Supplier);
                //dlo.LoadWith<ProductCatalog>(x => x.ProductCatalogGroup);

                ctx.LoadOptions = dlo;
                return ctx.ProductCatalog
                    .Where(x => x.ProductCatalogId == productCatalogId)
                    .FirstOrDefault();
            }
        }
    }
}
