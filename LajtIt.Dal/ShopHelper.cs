using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace LajtIt.Dal
{
    public class ShopHelper
    {
     
        public string CheckProductCode(Dal.Helper.Shop shop, int productCatalogId , bool createProduct)
        {
                Dal.ShopHelper sh = new ShopHelper();
                Dal.ProductCatalogShopProduct psp= sh.GetProductCatalogShopProduct((int)shop, productCatalogId);
            if (createProduct)
            {

                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                List<Dal.ProductCatalog> products = pch.GetProductCatalogByCode(psp.ProductCatalog.Code);

                if (products.Where(x => x.ProductCatalogId != productCatalogId /*&& x.ShopProductId.HasValue*/).Count() > 0)
                {
                    int addNumber = sh.SetProductCatalogCodeAddNumber(shop, productCatalogId, psp.ProductCatalog.Code);
                    return String.Format("{0} {1}", psp.ProductCatalog.Code, addNumber);
                }
                else
                    return psp.ProductCatalog.Code;

            }
            else
                if (psp.CodeAddNumber.HasValue)
                return String.Format("{0} {1}", psp.ProductCatalog.Code, psp.CodeAddNumber.Value);
            else
                return psp.ProductCatalog.Code;
        }
        public int GetShopIdByAllegroUserId(long userId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Shop.Where(x => x.ExternalId == userId).Select(x => x.ShopId).FirstOrDefault();
            }
        }
        public static void LogApiCall(string method, Object[] methodParams)
        {
            return;
            //using (LajtitDB ctx = new LajtitDB())
            //{

            //    ShopApiHelper api = new ShopApiHelper()
            //    {
            //        InsertDate = DateTime.Now,
            //        Method = method
            //    };
            //    if (method == "call")
            //    {
            //        api.Session = methodParams[0].ToString();
            //        api.Method = methodParams[1].ToString();
            //    }
            //    ctx.ShopApiHelper.InsertOnSubmit(api);
            //    ctx.SubmitChanges();

            //}

        }

        public List<ShopProducer> GetShopProducers(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopProducer.Where(x=>x.ShopId==shopId).ToList();
            }
        }
 

        public void SetShopToken(Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Shop shopToUpdate = ctx.Shop.Where(x => x.ShopId == shop.ShopId).FirstOrDefault();

                shopToUpdate.Token = shop.Token;
                shopToUpdate.TokenCreateDate = shop.TokenCreateDate;
                shopToUpdate.TokenEndDate = shop.TokenEndDate;
                shopToUpdate.TokenRefresh = shop.TokenRefresh;

                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogShopImageFnResult> GetProductCatalogImages(Helper.Shop shop, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopImageFn((int)shop).Where(x=>x.ProductCatalogId == productCatalogId).OrderBy(x=>x.Priority).ToList();
            }
        }

        public void SetProductCatalogImageShopImageId(Helper.Shop shop, int imageId, int shopImageId)
        {

            using (LajtitDB ctx = new LajtitDB())
            {

                Dal.ProductCatalogShopImage image = ctx.ProductCatalogShopImage.Where(x => x.ShopId == (int)shop && x.ImageId == imageId).FirstOrDefault();
                if (image == null)
                {
                    ctx.ProductCatalogShopImage.InsertOnSubmit(
                        new ProductCatalogShopImage()
                        {
                            ImageId = imageId,
                            ShopId = (int)shop,
                            ShopImageId = shopImageId
                        });
                    ctx.SubmitChanges();
                }

            }
          
        }

        public List<ShopAttributeGroup> ShopAttributeGroups(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopAttributeGroup.Where(x => x.ShopId == shopId).ToList();
            }
        }


        public ShopCategoryManager GetShopCategoryManager(int shopCategoryManagerId)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ShopCategoryManager>(x => x.Shop);
                ctx.LoadOptions = dlo;
                return ctx.ShopCategoryManager.Where(x => x.ShopCategoryManagerId == shopCategoryManagerId).FirstOrDefault();
            }
        }

        public List<ShopCategoryManagerCondition> GetShopCategoryManagerConditions(int shopCategoryManagerId)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                return ctx.ShopCategoryManagerCondition.Where(x => x.ShopCategoryManagerId == shopCategoryManagerId).ToList();
            }
        }

        public void SetSupplierShopProducer(SupplierShop ss, int producerId)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<SupplierShop>(x => x.Supplier);
                ctx.LoadOptions = dlo;


                Dal.SupplierShop ssToUpdate = ctx.SupplierShop.Where(x => x.Id == ss.Id).FirstOrDefault();

                Dal.ShopProducer shopProducer = new ShopProducer()
                {
                    InsertDate = DateTime.Now,
                    IsActive = true,
                    Name = ssToUpdate.Supplier.Name,
                    ShopId = ssToUpdate.ShopId,
                    ShopProducerId = producerId
                };
                ssToUpdate.ShopProducer = shopProducer;

                ctx.ShopProducer.InsertOnSubmit(shopProducer);

                ctx.SubmitChanges();
            }
        }



        public ProductCatalogShopProduct GetProductCatalogShopProduct(int shopId, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogShopProduct>(x => x.ProductCatalog);
                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogShopProduct.Where(x => x.ShopId == shopId && x.ProductCatalogId == productCatalogId).FirstOrDefault();
            }
        }
        public int SetProductCatalogCodeAddNumber(Dal.Helper.Shop shop, int productCatalogId, string code)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int? maxNumber = ctx.ProductCatalogShopProduct.Where(x => x.ShopId == (int)shop
                && x.ProductCatalog.Code == code
                && x.ShopProductId != null)
                    .Max(x => x.CodeAddNumber);

                ProductCatalogShopProduct pcToUpdate = ctx.ProductCatalogShopProduct
                    .Where(x => x.ShopId == (int)shop && x.ProductCatalogId == productCatalogId)
                    .FirstOrDefault();


                if (maxNumber.HasValue)
                    maxNumber++;
                else
                    maxNumber = 1;

                pcToUpdate.CodeAddNumber = maxNumber;
                ctx.SubmitChanges();

                return maxNumber.Value;
            }
        }

        public List<Shop> GetShopsForAttributeChanges()
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                return ctx.Shop.Where(x => x.IsActive && x.ShopType.ShopEngineType.RefreshAttributes).ToList();
            }
        }

        public void SetToken(Helper.Shop shop, string accessToken, int expiresIn)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                Dal.Shop shopToUpdate = ctx.Shop.Where(x => x.ShopId == (int)shop).FirstOrDefault();

                shopToUpdate.Token = accessToken;
                shopToUpdate.TokenEndDate = DateTime.Now.AddSeconds(expiresIn);

                ctx.SubmitChanges();
            }
        }

        //public void SetShopProducerUpdate(SupplierShop sp)
        //{
        //    using (LajtitDB ctx = new LajtitDB()) 
        //    {
        //        Dal.SupplierShop spToUpdate = ctx.SupplierShop.Where(x => x.Id == sp.Id).FirstOrDefault();

        //        spToUpdate.ProducerId = sp.ProducerId;

        //        ctx.SubmitChanges();
        //    }
        //}

        public List<ProductCatalogAttribute> GetShopCategoryManagerAttributes(int shopCategoryManagerId)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                return ctx.ShopCategoryManagerAttribute.Where(x => x.ShopCategoryManagerId == shopCategoryManagerId)
                    .Select(x=>x.ProductCatalogAttribute)
                    .ToList();
            }
        }
        public List<Supplier> GetShopCategoryManagerSuppliers(int shopCategoryManagerId)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                return ctx.ShopCategoryManagerSupplier.Where(x => x.ShopCategoryManagerId == shopCategoryManagerId)
                    .Select(x => x.Supplier)
                    .ToList();
            }
        }

        public void SetProductCatalogImageShopImageDelete(int imageId)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                Dal.ProductCatalogShopImage image = ctx.ProductCatalogShopImage.Where(x => x.ImageId == imageId).FirstOrDefault();

                ctx.ProductCatalogShopImage.DeleteOnSubmit(image);
                ctx.SubmitChanges();
            }
        }
        public void SetProductCatalogImageShopImageDelete(Dal.Helper.Shop shop, int shopImageId)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                Dal.ProductCatalogShopImage image = ctx.ProductCatalogShopImage.Where(x => x.ShopId == (int)shop && x.ShopImageId == shopImageId).FirstOrDefault();
                if (image != null)
                {
                    ctx.ProductCatalogShopImage.DeleteOnSubmit(image);
                    ctx.SubmitChanges();
                }
            }
        }

        public List<ShopCategoryManagerShopView> GetShopCategoryManagerShops(int shopCategoryManagerId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())

            {
                return ctx.ShopCategoryManagerShopView.Where(x => x.ShopCategoryManagerId == shopCategoryManagerId).ToList();
            }
        }

        public List<ShopCategoryManager> GetShopCategoryManagers(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopCategoryManager.Where(x=>x.ShopId==shopId).ToList();
            }
        }

        public static int? GetProductCatalogIdByShopProductId(Helper.Shop shop, string shopProductId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogShopProduct psp = ctx.ProductCatalogShopProduct.Where(x => x.ShopId == (int)shop && x.ShopProductId == shopProductId).FirstOrDefault();

                if (psp == null)
                    return null;
                else
                    return psp.ProductCatalogId;
            }
        }

        public void SetShopCategoryFields(Helper.ShopType shopType, int categoryId, List<ShopCategoryField> fields)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                string[] existingFieldIds = ctx.ShopCategoryField
                    .Where(x => x.ShopTypeId == (int)shopType && x.CategoryId == categoryId)
                    .Select(x => x.CategoryFieldId)
                    .ToArray();

                ctx.ShopCategoryField.InsertAllOnSubmit(fields.Where(x => !existingFieldIds.Contains(x.CategoryFieldId)));
                ctx.SubmitChanges();
            }
        }

        public List<ShopExportFileType> GetShopExportFileFormatTypes()
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                return ctx.ShopExportFileType.ToList();
            }
        }



        public int SetShopCategoryManager(ShopCategoryManager scm)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ShopCategoryManager.InsertOnSubmit(scm);
                ctx.SubmitChanges();

                return scm.ShopCategoryManagerId;
            }
        }

        public List<ShopAttributeFnResult> GetShopAttributes(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                return ctx.ShopAttributeFn(shopId).ToList();
            }
        }

        public List<ShopFnResult> GetProductCatalogForShopExportFile(int shopExportId, int shopImportId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {//1234567890
                return ctx.ShopFn(shopExportId, shopImportId)
                    .Where(x => x.MinPrice == null || (x.PriceBruttoMinimum.Value >= x.MinPrice.Value))
                    .ToList();
            }
        }

        public void SetShopProducers(Dal.Helper.Shop shop, List<ShopProducer> shopProducers)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] producerIds = shopProducers.Select(x => x.ShopProducerId).ToArray();

                List<Dal.ShopProducer> producersToUpdate = ctx.ShopProducer.Where(x => x.ShopId == (int)shop).ToList();

                int[] existingProducerIds = producersToUpdate.Select(x => x.ShopProducerId).ToArray();

                // edycja

                foreach (Dal.ShopProducer producerToUpdate in producersToUpdate.Where(x => producerIds.Contains(x.ShopProducerId)).ToList())
                    producerToUpdate.Name = shopProducers.Where(x => x.ShopProducerId == producerToUpdate.ShopProducerId).FirstOrDefault().Name;

                // nowi

                ctx.ShopProducer.InsertAllOnSubmit(shopProducers.Where(x => !existingProducerIds.Contains(x.ShopProducerId)));

                ctx.SubmitChanges();
            }
        }

        public void SetProductCatalogShopProductJson(Helper.Shop shop, int productCatalogId, string json)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogShopProduct psp = ctx.ProductCatalogShopProduct.Where(x => x.ShopId == (int)shop && x.ProductCatalogId == productCatalogId).FirstOrDefault();
                psp.JsonProduct = json;

                ctx.SubmitChanges();
            }
        }

        public ProductCatalogShopProduct GetShopProduct(Helper.Shop shop, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopProduct.Where(x => x.ShopId == (int)shop && x.ProductCatalogId == productCatalogId).FirstOrDefault();
            }
        }

        public int SetShopProducer(Helper.Shop shop, ShopProducer producer)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ShopProducer.InsertOnSubmit(producer);

                ctx.SubmitChanges();

                return producer.Id;
            }
        }


        public List<Dal.ShopColumnType> GetShopColumnTypes()
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                return ctx.ShopColumnType.ToList();
            }
        }

        public List<ShopExportFileAttributeFnResult> GetShopExportFileAttribute(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())

            {
                return ctx.ShopExportFileAttributeFn(shopId).ToList();
            }
        }

        public List<Dal.ShopUpdateType> GetShopUpdateTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopUpdateType.ToList();
            }
        }

        public void SetShopProductsTruncate()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ShopProductTruncate();
            }
        }

        public List<ShopUpdateColumnsView> GetShopUpdateColumns()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ShopUpdateColumnsView.OrderBy(x=>x.ColumnName).ToList();
            }
        }

        public List<ShopColumnTypeShopTypeView> GetShopColumnTypeShopType(int shopEngineTypeId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ShopColumnTypeShopTypeView.Where(x => x.ShopEngineTypeId == shopEngineTypeId).OrderBy(x => x.ShopColumnTypeId)
                    .ThenBy(x => x.TableName)
                    .ThenBy(x => x.ColumnName)
                    .ToList();
            }
        }

        public void SetShopColumnTypeShopType(ShopColumnTypeShopType st)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ShopColumnTypeShopType.InsertOnSubmit(st);
                ctx.SubmitChanges();
            };
        }

  
        public void SetShopExportFile(Shop shop, List<Dal.SupplierShop> ss, List<Dal.ShopExportFileAttribute> se)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Shop shopToUpdate = ctx.Shop.Where(x => x.ShopId == shop.ShopId).FirstOrDefault();

                shopToUpdate.ExportFileFormatTypeId = shop.ExportFileFormatTypeId;
                shopToUpdate.ExportFileEanRequired = shop.ExportFileEanRequired;
                shopToUpdate.ExportFileName = shop.ExportFileName;
                shopToUpdate.ExportFilePriceFrom = shop.ExportFilePriceFrom;
                shopToUpdate.ExportFilePriceTo = shop.ExportFilePriceTo;
                shopToUpdate.ExportFilePriceTypeId = shop.ExportFilePriceTypeId;
                shopToUpdate.ExportFileUrlParameters = shop.ExportFileUrlParameters;
                shopToUpdate.ExportFilePriceTypeId = shop.ExportFilePriceTypeId; 
                shopToUpdate.ExportFileFilterByProductType = shop.ExportFileFilterByProductType;
                shopToUpdate.ExportFileExportPriceTypeId= shop.ExportFileExportPriceTypeId;


                List<SupplierShop> ssToUpdate = ctx.SupplierShop.Where(x => x.ShopId == shop.ShopId).ToList();

                foreach (SupplierShop ssToU in ssToUpdate)
                {
                    SupplierShop ssh= ss.Where(x => x.SupplierId == ssToU.SupplierId).FirstOrDefault();
                    if (ssh != null)
                        ssToU.ExportFileEnabled = ssh.ExportFileEnabled;

                }


                ctx.SubmitChanges();

                List<Dal.ShopExportFileAttribute> attributesToDelete = ctx.ShopExportFileAttribute.Where(x => x.ShopId == shop.ShopId).ToList();

                ctx.ShopExportFileAttribute.DeleteAllOnSubmit(attributesToDelete);
                ctx.ShopExportFileAttribute.InsertAllOnSubmit(se);

                ctx.SubmitChanges();

            };
        }

        public List<ShopShipping> GetShopShipping(Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ShopShipping>(x => x.ShippingServiceMode);
                dlo.LoadWith<ShippingServiceMode>(x => x.ShippingCompany);
                ctx.LoadOptions = dlo;
                return ctx.ShopShipping.Where(x => x.ShopId == (int)shop).ToList();
            }
        }
        public List<ShopShipping> GetShopShipping(Helper.ShopType shopType)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ShopShipping>(x => x.ShippingServiceMode);
                dlo.LoadWith<ShopShipping>(x => x.Shop);
                dlo.LoadWith<ShippingServiceMode>(x => x.ShippingCompany);
                ctx.LoadOptions = dlo;
                return ctx.ShopShipping.Where(x => x.Shop.ShopTypeId == (int)shopType).ToList();
            }
        }
        public void SetShopOrderStatus(int orderId, string orderStatus)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ShopOrder so = ctx.ShopOrder.Where(x => x.OrderId == orderId).FirstOrDefault();
                so.ShopOrderStatus = orderStatus;
                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogShopCategoryFnResult> GetProductCatalogShopCategories(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopCategoryFn(shopId).ToList();
            }
        }

        public List<Dal.SupplierShopFnResult> GetSuppliersShop(int? shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierShopFn(shopId).ToList();
            }
        }


        public List<ShopCategoryField> GetShopCategoryFieldsByCategoryId(Dal.Helper.ShopType shopType, string shopCategoryId, bool? useDefaultValue)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopCategoryField.Where(x => x.ShopTypeId==(int)shopType &&  x.ShopCategory.ShopCategoryId == shopCategoryId && x.UseDefaultValue == (useDefaultValue ?? x.UseDefaultValue)).ToList();
            }
        }


        public void SetShopCategoryManager(ShopCategoryManager scm, int[] categoryIds, int[] attributes, int[] suppliers, List<ShopCategoryManagerCondition> conditions)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ShopCategoryManager scmToUpdate = ctx.ShopCategoryManager.Where(x => x.ShopCategoryManagerId == scm.ShopCategoryManagerId).FirstOrDefault();

                scmToUpdate.IsActive = scm.IsActive; 
                scmToUpdate.Name = scm.Name;
                scmToUpdate.Description = scm.Description;
                scmToUpdate.MainCategoryPriority = scm.MainCategoryPriority;

                var sh = ctx.ShopCategoryManagerShop.Where(x => x.ShopCategoryManagerId == scm.ShopCategoryManagerId).ToList();


                // dodaj nieistneijące
                int[] existingCategoryIds = sh.Select(x => x.CategoryId).ToArray();

                int[] categoryIdsToAdd = categoryIds.Where(x => !existingCategoryIds.Contains(x)).ToArray();//  sh.Where(x => !categoryIds.Contains(x.CategoryId)).Select(x => x.CategoryId).ToArray();
                ctx.ShopCategoryManagerShop.InsertAllOnSubmit(categoryIdsToAdd.Select(x => new ShopCategoryManagerShop() { ShopCategoryManagerId = scm.ShopCategoryManagerId, CategoryId = x }));
                ctx.SubmitChanges();


                // usuń niesitniejące
                List<ShopCategoryManagerShop> categoriesToDelete = ctx.ShopCategoryManagerShop
                    .Where(x => x.ShopCategoryManagerId == scm.ShopCategoryManagerId && !categoryIds.Contains(x.CategoryId))
                    .ToList();

                ctx.ShopCategoryManagerShop.DeleteAllOnSubmit(categoriesToDelete);




                var att = ctx.ShopCategoryManagerAttribute.Where(x => x.ShopCategoryManagerId == scm.ShopCategoryManagerId).ToList();
                ctx.ShopCategoryManagerAttribute.DeleteAllOnSubmit(att);
                var sup = ctx.ShopCategoryManagerSupplier.Where(x => x.ShopCategoryManagerId == scm.ShopCategoryManagerId).ToList();
                ctx.ShopCategoryManagerSupplier.DeleteAllOnSubmit(sup);


                List<Dal.ShopCategoryManagerCondition> conditionsToUpdate = ctx.ShopCategoryManagerCondition.Where(x => x.ShopCategoryManagerId == scm.ShopCategoryManagerId).ToList();

                foreach (Dal.ShopCategoryManagerCondition conditionToUpdate in conditionsToUpdate)
                {
                    Dal.ShopCategoryManagerCondition condition = conditions.Where(x => x.ConditionTypeId == conditionToUpdate.ConditionTypeId).FirstOrDefault();

                    conditionToUpdate.BitValue = condition.BitValue;
                    conditionToUpdate.DecimalValue = condition.DecimalValue;
                    conditionToUpdate.StringValue = condition.StringValue;
                    conditionToUpdate.IsActive = condition.IsActive;
                    conditionToUpdate.Name = condition.Name;
                }

                int[] existingConditionTypeIds = conditionsToUpdate.Select(x => x.ConditionTypeId).ToArray();
                List<Dal.ShopCategoryManagerCondition> conditionsToInsert = conditions.Where(x => !existingConditionTypeIds.Contains(x.ConditionTypeId)).ToList();

                ctx.ShopCategoryManagerCondition.InsertAllOnSubmit(conditionsToInsert);

                ctx.SubmitChanges();

                ctx.ShopCategoryManagerAttribute.InsertAllOnSubmit(attributes.Select(x => new ShopCategoryManagerAttribute() { ShopCategoryManagerId = scm.ShopCategoryManagerId, AttributeId = x }));

                ctx.SubmitChanges();

                ctx.ShopCategoryManagerSupplier.InsertAllOnSubmit(suppliers.Select(x => new ShopCategoryManagerSupplier() { ShopCategoryManagerId = scm.ShopCategoryManagerId, SupplierId= x }));

                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogForShopFnResult> ProductCatalogForShop(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogForShopFn(shopId).ToList();
            }
        }
        public List<ProductCatalogForShopFnResult> ProductCatalogForShopForEmpik(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogForShopFn(shopId)
                    .Where(x =>// x.ShopProductId != null && 
                    x.Ean != null)
                    .ToList();
            }
        }

        public Dal.ShopCategoryView GetShopCategoryView(int categoryId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ShopCategoryView.Where(x => x.CategoryId == categoryId).FirstOrDefault();
            }
        }

        public List<CeneoShopResult> GetCeneoProducts(bool? isAuction, bool? isActivePricePromo, int[] supplierId, decimal? priceFrom, decimal? priceTo, decimal? bidFrom, decimal? bidTo)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var results = ctx.CeneoShop((int)Dal.Helper.Shop.Ceneo, (int)Dal.Helper.Shop.Lajtitpl).AsQueryable();

                if (isAuction.HasValue && isAuction.Value)
                    results = results.Where(x => x.CeneoMaxBid > 0);
                if (isAuction.HasValue && !isAuction.Value)
                    results = results.Where(x => x.CeneoMaxBid == 0);

                if (isActivePricePromo.HasValue )
                    results = results.Where(x => x.IsActivePricePromo==isActivePricePromo.Value);

                if (priceFrom.HasValue) results = results.Where(x => supplierId.Contains(x.SupplierId) && x.PriceBruttoMinimum >= priceFrom.Value);
                if (priceTo.HasValue) results = results.Where(x => supplierId.Contains(x.SupplierId) && x.PriceBruttoMinimum <= priceTo.Value);
                if (bidFrom.HasValue) results = results.Where(x => supplierId.Contains(x.SupplierId) && x.CeneoMaxBid >= bidFrom.Value);
                if (bidTo.HasValue) results = results.Where(x => supplierId.Contains(x.SupplierId) && x.CeneoMaxBid <= bidTo.Value);

                return results.ToList();

            }
        }
        public List<CeneoShopResult> GetCeneoProducts(Dal.Helper.Shop ceneoShop, Dal.Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {


                return ctx.CeneoShop((int)ceneoShop, (int)shop).Where(x => /*x.SupplierId == 22 && */x.ShopProductId != null && x.CeneoShopProductId != null).ToList();

            }
        }
        public List<ShopProductsNotInSystemResult> GetShopProductsNotInSystem(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopProductsNotInSystem(shopId).ToList();
            }
        }

        public void SetProductCatalogShop(List<CeneoShopResult> products, decimal maxBid)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach(CeneoShopResult product in products)
                {
                    ProductCatalogShopProduct pcs = ctx.ProductCatalogShopProduct.Where(x => x.Id == product.Id).FirstOrDefault();

                    pcs.CeneoMaxBid = maxBid;
                    ctx.SubmitChanges();
                }
            }

        }

        public List<ShopRebate> GetRebates()
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopRebate.Where(x => x.RebateTypeId == 1).OrderBy(x => x.AmountFrom).ToList();
            }

        }
        public string[] InsertOrders(string[] shopOrderNumbers, string shopOrderStatus, Dal.Helper.Shop shop, string actingUser)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                shopOrderNumbers = shopOrderNumbers.OrderByDescending(x => x).Take(2000).ToArray();

                string[] shopOrdersNumbersSaved =
                    ctx.ShopOrder.Where(x => shopOrderNumbers.Contains(x.ShopOrderNumber) && x.ShopId==(int)shop).Select(x => x.ShopOrderNumber).ToArray();

                string[] shopOrdersNumbersNotSaved = shopOrderNumbers.Where(x => !shopOrdersNumbersSaved.Contains(x)).ToArray();


                List<ShopOrder> orders =
                    shopOrdersNumbersNotSaved.Select(x =>

                    new ShopOrder()
                    {
                        InsertDate = DateTime.Now,
                        InsertUser = actingUser,
                        IsProcessed = false,
                        ShopOrderNumber = x,
                        UpdateStatus = false,
                        ShopId=(int)shop,
                        CheckForPayment = true,
                        ShopOrderStatus= shopOrderStatus

                    }).ToList();
                ctx.ShopOrder.InsertAllOnSubmit(orders);
                ctx.SubmitChanges();
                return ctx.ShopOrder.Where(x => x.ShopId == (int)shop && x.IsProcessed == false).Select(x => x.ShopOrderNumber).ToArray() ;

            }

        }

        //public void SetSupplierShops(List<SupplierShop> ss)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        foreach (Dal.SupplierShop s in ss)
        //        {
        //            SupplierShop supplierToUpdate = ctx.SupplierShop.Where(x => x.SupplierId == s.SupplierId && x.ShopId == s.ShopId).FirstOrDefault();


        //            if (supplierToUpdate != null)
        //                supplierToUpdate.Template = s.Template;

        //        }
        //        ctx.SubmitChanges();
        //    }
        //}

        public void SetSupplierShopShort(List<SupplierShop> ss)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach (Dal.SupplierShop s in ss)
                {

                    SupplierShop supplierToUpdate = ctx.SupplierShop.Where(x => x.ShopId == s.ShopId && x.SupplierId == s.SupplierId).FirstOrDefault();
                    supplierToUpdate.Template = s.Template; 

                }
                ctx.SubmitChanges();
            }
        }
        public void SetSupplierShop(List<SupplierShop> ss)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach (Dal.SupplierShop s in ss)
                {

                    SupplierShop supplierToUpdate = ctx.SupplierShop.Where(x => x.Id == s.Id).FirstOrDefault();
                    supplierToUpdate.Template = s.Template;
                    supplierToUpdate.CategoryId = s.CategoryId;
                    supplierToUpdate.ExportFileEnabled = s.ExportFileEnabled;
                    supplierToUpdate.IsActive = s.IsActive;
                    supplierToUpdate.MaxNumberOfProductsInOffer = s.MaxNumberOfProductsInOffer;
                    supplierToUpdate.LockRebates = s.LockRebates;
                    //supplierToUpdate.ProducerId = s.ProducerId;
                    supplierToUpdate.SellCommision = s.SellCommision;
                    supplierToUpdate.SellDiscount = s.SellDiscount;
                    supplierToUpdate.SellDiscountValue = s.SellDiscountValue;
                    supplierToUpdate.ShowSupplierNameIdDescription = s.ShowSupplierNameIdDescription;
                    supplierToUpdate.Template = s.Template;
                    supplierToUpdate.MinPrice = s.MinPrice;
                    supplierToUpdate.MaxPrice = s.MaxPrice;

                }
                ctx.SubmitChanges();
            }
        }

        //public int SetShopMainPageGroupActive(int groupId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        Dal.ShopMainPageGroup groupOld = ctx.ShopMainPageGroup.Where(x => x.IsActive).FirstOrDefault();
        //        if (groupOld != null)
        //            groupOld.IsActive = false;

        //        Dal.ShopMainPageGroup groupNew = ctx.ShopMainPageGroup.Where(x => x.ShopMainPageGroupId == groupId).FirstOrDefault();
        //        if (groupNew != null)
        //            groupNew.IsActive = true;

        //        ctx.SubmitChanges();

        //        return groupId;
        //    }
        //}

        //public void SetCategoryDelete(int categoryId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        Dal.ShopCategory sc = ctx.ShopCategory.Where(x => x.CategoryId == categoryId).FirstOrDefault();

        //        if (sc != null)
        //        {

        //            foreach (ShopCategory subc in ctx.ShopCategory.Where(x => x.CategoryParentId == categoryId).ToList())
        //            {
        //                SetCategoryDelete(subc.CategoryId);

        //            }

        //            ctx.ShopCategory.DeleteOnSubmit(sc);

        //            ctx.SubmitChanges();
        //        }
        //    }
        //}

    
        //public ShopCategory GetCategory(int categoryId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {

        //        return ctx.ShopCategory.Where(x => x.CategoryId == categoryId).FirstOrDefault();



        //    }
        //}

      

        //public List<ShopMainPageView> GetShopMainPageProducts(int groupId)
        //{
        //    using (LajtitViewsDB ctx = new LajtitViewsDB())
        //    {
        //        return ctx.ShopMainPageView.Where(x => x.ShopMainPageGroupId == groupId).OrderBy(x => x.Priority).ToList();
        //    }
        //}

        public List<ShopMainPageGroup> GetShopMainPageGroups()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopMainPageGroup.OrderByDescending(x => x.IsActive).ThenByDescending(x => x.InsertDate).ToList();
            }
        }

        //public bool SetCategory(ShopCategory sc, int shopId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ShopCategory scToUpdate = ctx.ShopCategory.Where(x => x.ShopId == shopId && x.CategoryId == sc.CategoryId).FirstOrDefault();

        //        scToUpdate.Description = sc.Description;
        //        scToUpdate.IsActive = sc.IsActive;
        //        scToUpdate.IsPublished = sc.IsPublished;
        //        scToUpdate.Name = sc.Name;
        //        scToUpdate.Permalink = sc.Permalink;
        //        scToUpdate.SeoDescription = sc.SeoDescription;
        //        scToUpdate.SeoKeywords = sc.SeoKeywords;
        //        scToUpdate.SeoTitle = sc.SeoTitle;
        //        scToUpdate.ShopId = sc.ShopId;

        //        ctx.SubmitChanges();

        //        return true;
        //    }
        //}

        public List<ShopOrder> GetOrdersToProcess(Dal.Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopOrder.Where(x => x.ShopId == (int)shop && x.IsProcessed == false).ToList();

            }
        }

        public List<ShopCategoryFnResult> GetCategories(Dal.Helper.ShopType shopType)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopCategoryFn((int)shopType)
                    .ToList();

            }
        }

        public ShopCategory GetCategory(Dal.Helper.ShopType shopType, int categoryId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopCategory
                    .Where(x => x.ShopTypeId == (int)shopType && x.CategoryId== categoryId)
                    .FirstOrDefault();

            }
        }

        public List<ShopCategoryFnResult> GetShopCategories(Dal.Helper.ShopType shopType)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopCategoryFn((int)shopType).ToList();
            }
        }
        public ShopCategory GetShopCategory(Dal.Helper.ShopType shopType, string shopCategoryId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopCategory
                    .Where(x => x.ShopTypeId == (int)shopType && x.ShopCategoryId == shopCategoryId)
                    .FirstOrDefault();

            }
        }

        public List<ShopOrder> GetOrderTransactionNumber(Helper.Shop shop)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ShopOrder>(x => x.Order);
                dlo.LoadWith<Order>(x => x.OrderShipping);
                ctx.LoadOptions = dlo;
                return ctx.ShopOrder
                    .Where(x => x.ShopId == (int)shop
                    && x.OrderId != null
                    && x.Order.OrderShipping != null
                    && x.Order.OrderShipping.ShipmentTrackingNumber != null
                    && x.Order.OrderShipping.TrackingNumberSent == false)
                    .ToList();
            }
        }


        public int SetNewOrderClipperon(ShopOrder shopOrder,
            Invoice invoice,
            List<OrderProduct> products,
            Dal.Helper.Shop shop,
            OrderStatusHistory orderStatusHistory,
            Dal.OrderPayment orderPayment,
            Dal.OrderShipping os)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                ShopOrder sh = ctx.ShopOrder.Where(x => x.ShopOrderNumber == shopOrder.ShopOrderNumber && x.ShopId == (int)shop).FirstOrDefault();

                
                bool isPaid = false;

                if (ctx.OrderPayment.Where(x => x.OrderId == shopOrder.OrderId).Count() > 0)
                    isPaid = true;


                if (shopOrder.OrderId == 0) // zamowienie juz istnieje
                {
                    ctx.OrderStatusHistory.InsertOnSubmit(orderStatusHistory);
                    ctx.OrderProduct.InsertAllOnSubmit(products);
                }
                if (orderPayment != null && !isPaid)
                    ctx.OrderPayment.InsertOnSubmit(orderPayment);

                if (invoice != null && shopOrder.OrderId == 0)
                {
                    if (invoice.InvoiceTypeId == 0)
                        invoice.InvoiceTypeId = 2;
                    ctx.Invoice.InsertOnSubmit(invoice);
                }

                ctx.OrderShipping.InsertOnSubmit(os);
                ctx.SubmitChanges();

                sh.ShopExtraInfo = shopOrder.ShopExtraInfo;
                sh.IsProcessed = shopOrder.IsProcessed;
                sh.Order = orderStatusHistory.Order;
                sh.CheckForPayment = shopOrder.CheckForPayment;

                ctx.SubmitChanges();

                return sh.OrderId.Value;

            }
        }
        public int SetNewOrder(ShopOrder shopOrder,
    Invoice invoice,
    List<OrderProduct> products,
    Dal.Helper.Shop shop,
    OrderStatusHistory orderStatusHistory,
    Dal.OrderPayment orderPayment)
        {
            throw new NotImplementedException();
            return 1;
        }
        public int SetNewOrder(ShopOrder shopOrder, 
            Invoice invoice, 
            List<OrderProduct> products,
            Dal.Helper.Shop shop,
            OrderStatusHistory orderStatusHistory,
            Dal.OrderPayment orderPayment,
            Dal.OrderShipping orderShipping)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                ShopOrder sh = ctx.ShopOrder.Where(x => x.ShopOrderNumber == shopOrder.ShopOrderNumber && x.ShopId==(int)shop).FirstOrDefault();

                ctx.OrderStatusHistory.InsertOnSubmit(orderStatusHistory);
                ctx.OrderProduct.InsertAllOnSubmit(products);


                if (orderShipping != null)
                    if (orderShipping.ShippingServiceModeId == (int)Dal.Helper.ShippingServiceMode.Courier && products[0].Order.ShipmentCountryCode == "PL")
                    {
                        orderShipping.OrderShippingStatusId = (int)Dal.Helper.OrderShippingStatus.ReadyToCreate;

                        Dal.OrderShippingParcel parcel = new OrderShippingParcel()
                        {
                            OrderShipping = orderShipping,
                            Weight = Dal.Helper.DefaultParcelWeight
                        };
                        ctx.OrderShippingParcel.InsertOnSubmit(parcel);
                    }
                    else
                        ctx.OrderShipping.InsertOnSubmit(orderShipping);


                if (orderPayment != null)
                    ctx.OrderPayment.InsertOnSubmit(orderPayment);
                if (invoice != null)
                {
                    if (invoice.CompanyName.Length > 100)
                        invoice.CompanyName = invoice.CompanyName.Remove(99);
                    if (invoice.InvoiceTypeId == 0)
                        invoice.InvoiceTypeId = 2; 
                    ctx.Invoice.InsertOnSubmit(invoice);
                }

                ctx.SubmitChanges();

                sh.ShopExtraInfo = shopOrder.ShopExtraInfo;
                sh.IsProcessed = shopOrder.IsProcessed;
                sh.Order = orderStatusHistory.Order;
                sh.CheckForPayment = orderPayment == null;

                ctx.SubmitChanges();

                return sh.OrderId.Value;

            }
        }

        public void SetOrderCheckForPayment(ShopOrder shopOrder, Dal.Helper.Shop shop, bool checkForPayment)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ShopOrder sh = ctx.ShopOrder.Where(x => x.ShopOrderNumber == shopOrder.ShopOrderNumber && x.ShopId == (int)shop).FirstOrDefault();

                sh.CheckForPayment = checkForPayment;

                ctx.SubmitChanges();
            }
        }

                //public List<ShippingType> GetShippingTypes()
                //{
                //    using (LajtitDB ctx = new LajtitDB())
                //    {

                //        return ctx.ShippingType.Where(x => x.ShopExternalShippingId != null).ToList();
                //    }
                //}

                public void SetCategories(Dal.Helper.ShopType shop , List<Dal.ShopCategory> categories)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                string parentId = null;
                NewMethod(ctx, shop, categories, parentId);
            }
        }

        public List<ShopEngineType> GetShopEngineTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopEngineType.ToList();
            }
        }

        //public List<ProductCatalogView> GetProductCatalogByShopIds(int[] shopProductsId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogView.Where(x => x.ShopProductId.HasValue && shopProductsId.Contains(x.ShopProductId.Value)).ToList();
        //    }
        //}

        private static void NewMethod(LajtitDB ctx, Dal.Helper.ShopType shop, List<Dal.ShopCategory> categories, string parentId)
        {
            List<ShopCategory> c1 = categories.Where(x => x.CategoryParentId == parentId).ToList();

            foreach (ShopCategory sc in c1)
            {
                ShopCategory c = ctx.ShopCategory.Where(x => x.ShopTypeId == (int)shop &&  x.ShopCategoryId == sc.ShopCategoryId).FirstOrDefault();

                if (c == null)
                    ctx.ShopCategory.InsertOnSubmit(sc);
                else
                {
                    c.CategoryOrder = sc.CategoryOrder;
                    c.CategoryParentId = sc.CategoryParentId;
                    c.IsActive = sc.IsActive;
                    c.Name = sc.Name;
                    c.Url = sc.Url;
                    c.Description = sc.Description;
                    c.Permalink = sc.Permalink;
                    c.SeoTitle = sc.SeoTitle;
                    c.SeoKeywords = sc.SeoKeywords;
                    c.SeoDescription = sc.SeoDescription;
                    c.IsAllowed = sc.IsAllowed;
                    c.ShopCategoryId = sc.ShopCategoryId;
                }

                NewMethod(ctx, shop, categories, sc.ShopCategoryId);
            }

            ctx.SubmitChanges();
        }

        public void SetShopMainPageDeleteItem(int shopMainPageGroupId, int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ShopMainPage smp = ctx.ShopMainPage.Where(x => x.Id == id).FirstOrDefault();
                List<ShopMainPage> smps = ctx.ShopMainPage.Where(x => x.ShopMainPageGroupId == shopMainPageGroupId && x.Priority > smp.Priority).ToList();
                foreach (ShopMainPage p in smps)
                    p.Priority--;

                ctx.ShopMainPage.DeleteOnSubmit(smp);
                ctx.SubmitChanges();
            }
        }

        public SupplierShop GetSuppliersShopByAllegroUserId(int supplierId,long allegroUserId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierShop.Where(x =>
                     x.SupplierId == supplierId
                     && x.Shop.ShopTypeId == (int)Dal.Helper.ShopType.Allegro
                     && x.Shop.ExternalId == allegroUserId)
                     .FirstOrDefault();
            }
        }

        //public bool VerifyProductCode(int shopMainPageGroupId, string code, out List<string> message, string userName)
        //{

        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        Dal.OrderHelper oh = new Dal.OrderHelper();
        //        Dal.ProductCatalogView pc = new Dal.ProductCatalogView();

        //        pc = oh.GetProductCatalogByCode(code);
        //        message = new List<string>();

        //        if (pc == null)
        //        {
        //            message.Add("Produkt o podanym kodzie nie istnieje");
        //            return false;
        //        }

        //        //if (pc.ControlQuantities && pc.LeftQuantity.Value < 1)
        //        //    message.Add("Brak dostępnych produktów");

        //        if (!pc.IsActive)
        //            message.Add("Produkt oznaczony jako niemożliwy do przypisania");

        //        if (pc.ShopProductId==null)
        //            message.Add("Produkt nie istnieje w sklepie");

        //        if (message.Count ==0)
        //        {
        //            bool exists = ctx.ShopMainPage.Where(x => x.ShopMainPageGroupId == shopMainPageGroupId && x.ProductCatalogId == pc.ProductCatalogId).Count() > 0;

        //            if (exists)
        //                message.Add("Produkt już jest dodany do listy");

        //        }


        //        if (message.Count > 0)
        //        {
        //            return false;
        //        }

        //        int? priority = -1;
        //        Dal.ShopMainPage s = ctx.ShopMainPage.Where(x => x.ShopMainPageGroupId == shopMainPageGroupId).OrderByDescending(x => x.Priority).FirstOrDefault();
        //        if (s != null)
        //            priority = s.Priority + 1;

        //        if (!priority.HasValue)
        //            priority = 0;

        //        Dal.ShopMainPage smp = new Dal.ShopMainPage()
        //        {
        //            Priority = priority.Value,
        //            ProductCatalogId = pc.ProductCatalogId,
        //            ShopMainPageGroupId = shopMainPageGroupId
        //        };

        //        ctx.ShopMainPage.InsertOnSubmit(smp);
        //        ctx.SubmitChanges();
        //        return true;
        //    }
        //}

        public void SetShopProduct(ShopProduct sp)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ShopProduct.InsertOnSubmit(sp);
                ctx.SubmitChanges();

            }
        }
        public void SetShopProducts(List<ShopProduct> sp)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ShopProduct.InsertAllOnSubmit(sp);
                ctx.SubmitChanges();

            }
        }

        public List<ProductCatalogShopMainPageFnResult> GetProductCatalogForShopMainPage(Dal.Helper.Shop shop, int count)
        {

            using (LajtitDB ctx = new LajtitDB())
            {

                return ctx.ProductCatalogShopMainPageFn((int)shop) 
                    .OrderByDescending(x => x.ShopProductPriority)
                    .Take(50)
                    .ToList()
                    .OrderBy(x=> Guid.NewGuid())
                    .Take(count)
                    .ToList();
            }
        }

        public void SetShopMainPageChangeOrder(int shopMainPageGroupId, int newPriority, int oldPriority)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ShopMainPage smp = ctx.ShopMainPage.Where(x => x.ShopMainPageGroupId == shopMainPageGroupId && x.Priority == oldPriority).FirstOrDefault();
                if (newPriority > oldPriority)
                {
                    List<ShopMainPage> pages = ctx.ShopMainPage.Where(x => x.ShopMainPageGroupId == shopMainPageGroupId && x.Priority <= newPriority && x.Priority>oldPriority).ToList();
                    foreach (ShopMainPage p in pages)
                        p.Priority -= 1;


                }
                else
                {
                    List<ShopMainPage> pages = ctx.ShopMainPage.Where(x => x.ShopMainPageGroupId == shopMainPageGroupId && x.Priority >= newPriority && x.Priority< oldPriority).ToList();
                    foreach (ShopMainPage p in pages)
                        p.Priority += 1;


                }
                smp.Priority = newPriority;
                ctx.SubmitChanges();

            }
        }

        public List<SupplierShop> GetSupplierShop(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<SupplierShop>(x => x.Shop);
                dlo.LoadWith<SupplierShop>(x => x.Supplier);
                dlo.LoadWith<SupplierShop>(x => x.ShopProducer);
                ctx.LoadOptions = dlo;

                return ctx.SupplierShop.Where(x => x.ShopId == shopId).ToList();
            }
        }
        public SupplierShop GetSupplierShop(int shopId, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<SupplierShop>(x => x.Shop);
                dlo.LoadWith<SupplierShop>(x => x.Supplier);
                dlo.LoadWith<Shop>(x => x.ShopCategory);
                ctx.LoadOptions = dlo;

                return ctx.SupplierShop.Where(x => x.ShopId == shopId && x.SupplierId == supplierId).FirstOrDefault();
            }
        }

        public void SetProductCatalogShop(List<Dal.Helper.ShopCeneo> products)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach ( Dal.Helper.ShopCeneo p in products)
                {

                    var ceneoProduct = ctx.ProductCatalogShopProduct.Where(x => x.ShopId == (int)p.CeneoShop && x.ShopProductId == p.CeneoProductId).FirstOrDefault();

                    if(ceneoProduct!=null)
                    {
                        try
                        {
                            ceneoProduct.CeneoClickRate = p.CeneoClickRate;
                            ceneoProduct.CeneoClickRateWithMaxBid = p.CeneoClickRateWithMaxBid;
                            ceneoProduct.CeneoMaxBid = p.CeneoMaxBid;
                            ctx.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            Dal.ErrorHandler.LogError(ex, "");
                        }

                        continue;
                    }

                    var product = ctx.ProductCatalogShopProduct.Where(x => x.ShopId == (int)p.Shop && x.ShopProductId == p.ShopProductId).FirstOrDefault();

                    if (product == null)
                        continue;

                    if (product != null)
                    {
                        int productCatalogId = product.ProductCatalogId;

                        Dal.ProductCatalogShopProduct sp = new ProductCatalogShopProduct()
                        {
                            ProductCatalogId = productCatalogId,
                            ShopId = (int)p.CeneoShop,
                            ShopProductId = p.CeneoProductId,
                            CeneoClickRate=p.CeneoClickRate,
                            CeneoMaxBid=p.CeneoMaxBid,
                            CeneoClickRateWithMaxBid=p.CeneoClickRateWithMaxBid
                        };

                        Dal.ProductCatalogShopProduct spToUpdate = ctx.ProductCatalogShopProduct
                            .Where(x => x.ShopId == (int)p.CeneoShop && x.ProductCatalogId == productCatalogId)
                            .FirstOrDefault();

                        try
                        {


                            if (spToUpdate == null)
                                ctx.ProductCatalogShopProduct.InsertOnSubmit(sp);
                            else
                            {
                                spToUpdate.ShopProductId = p.CeneoProductId;
                            }
                            ctx.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            Dal.ErrorHandler.LogError(ex, String.Format("Sklep {0}, ShopProductId {1}", product.Shop, product.ShopProductId));
                        }
                    }

                }

            }
        }

        public List<ProductCatalogAttributeGroupsForShopResult> GetProductCatalogAttributeGroupsForShop(Helper.Shop shop, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeGroupsForShop((int)shop, productCatalogId).ToList();
            }
        }

        public ShopOrder GetShopOrder(Helper.Shop shop, string shopOrderNumber)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ShopOrder>(x => x.Order);
              
                ctx.LoadOptions = dlo;
                ShopOrder so = ctx.ShopOrder.Where(x => x.ShopOrderNumber == shopOrderNumber && x.ShopId == (int)shop).FirstOrDefault();
                return so;
            }
        }

        public void SetOrderUpdateStatusCompleted(string shopOrderNumber, Dal.Helper.Shop shop)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                ShopOrder so = ctx.ShopOrder.Where(x => x.ShopOrderNumber == shopOrderNumber && x.ShopId == (int)shop).FirstOrDefault();
                so.UpdateStatus = false;
                ctx.SubmitChanges();
            }
        }

        public List<ShopOrder> GetShopOrdersToUpdateStatus(Dal.Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ShopOrder>(x => x.Order);
                dlo.LoadWith<Order>(x => x.OrderShipping);
                dlo.LoadWith<OrderShipping>(x => x.ShippingCompany);
                dlo.LoadWith<Order>(x => x.OrderStatus);
                ctx.LoadOptions = dlo;

                return ctx.ShopOrder.Where(x=>x.UpdateStatus==true && x.OrderId.HasValue && x.ShopId == (int)shop)
                        .ToList();
            }
        }
         

        public List<ProductCatalogForShopResult> GetProductCatalogShopUpdate(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogForShop(shopId).OrderByDescending(x => x.ShopProductPriority).ToList();


            }
        }

        //public int[] GetShopProductIdByProductCatalog(int[] productCatalogIds)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalog
        //            .Where(x => x.ShopProductId.HasValue && productCatalogIds.Contains(x.ProductCatalogId))
        //            .Select(x => x.ShopProductId.Value)
        //            .ToArray();


        //    }
        //}



        //public ProductCatalogAttributeCategory GetProductCatalogShopCategory(int productCatalogId, Dal.Helper.ShopType shopType)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogAttributeCategory
        //            .Where(x => x.ProductCatalogId == productCatalogId && x.ShopTypeId == (int)shopType)
        //            .FirstOrDefault();
        //    }
        //}
        //public List<ShopCategory> GetProductCatalogShopCategory(int[] productCatalogIds)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogAttributeToProduct
        //            .Where(x => productCatalogIds.Contains(x.ProductCatalogId)&& x.ProductCatalogAttribute.CategoryId.HasValue)
        //            .Select(x => x.ProductCatalogAttribute.ShopCategory)
        //            .ToList();
        //    }
        //}

        public void SetCategoryPublished(int categoryId, bool isPublished)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var sc = ctx.ShopCategory.Where(x => x.CategoryId == categoryId).FirstOrDefault();
                sc.IsPublished = isPublished;
                ctx.SubmitChanges();
            }
        }

        public List<ShopCategoryManager> GetShopCategoryManagers()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopCategoryManager.Where(x => x.IsActive).ToList();
            }
        }

        public List<int> GetShopCategoryManagerInitialProducts(int shopCategoryManagerId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int shopId = ctx.ShopCategoryManager.Where(x => x.ShopCategoryManagerId == shopCategoryManagerId)
                    .Select(x => x.ShopId).FirstOrDefault();

                IQueryable<ProductCatalogShopProductFnResult> products = ctx.ProductCatalogShopProductFn(shopId)
                    .Where(x => x.IsPSActive.Value).AsQueryable();

                List<Dal.ShopCategoryManagerCondition> conditions = ctx.ShopCategoryManagerCondition.Where(x =>
                x.ShopCategoryManagerId == shopCategoryManagerId
                && x.IsActive)
                    .ToList();
                 
                foreach(Dal.ShopCategoryManagerCondition condition in conditions)
                {
                    switch(condition.ConditionTypeId)
                    {
                        case 1:
                            products = products.Where(x => condition.DecimalValue.HasValue && x.PriceBruttoMinimum >= condition.DecimalValue.Value);
                            break;
                        case 2:
                            products = products.Where(x => condition.DecimalValue.HasValue && x.PriceBruttoMinimum <= condition.DecimalValue.Value);
                            break;
                        case 3:
                            products = products.Where(x => condition.BitValue.HasValue && x.IsActivePricePromo);
                            break;
                        case 4:
                            products = products.Where(x => condition.DecimalValue.HasValue && x.IsActivePricePromo && x.PriceBruttoPromo/x.PriceBrutto > (1-condition.DecimalValue.Value/100));
                            break;

                    }
                }

                List<ProductCatalogAttribute> attributes = GetShopCategoryAttributes(shopCategoryManagerId);

                int[] groupIds = attributes.Select(x => x.AttributeGroupId).Distinct().ToArray();

                int[] productCatalogIds = products.Select(x => x.ProductCatalogId).ToArray();


                Dal.ProductCatalogHelper pch = new ProductCatalogHelper();

                List<int> productIds = new List<int>();
                foreach (int productCatalogId in productCatalogIds)
                {
                    var pa = pch.GetProductCatalogAttributesForProduct(productCatalogId);
                    bool exists = true;

                    foreach (int groupId in groupIds) // dla każdej grupy musi być choć jeden atrybut z wybranych w promocji
                    {
                        int[] attributeIdsSelectedForGroup = attributes.Where(x => x.AttributeGroupId == groupId).Select(x => x.AttributeId).ToArray(); // pobieram id
                        bool result = pa.Where(x => x.AttributeGroupId == groupId && attributeIdsSelectedForGroup.Contains(x.AttributeId)).Count() > 0; // sprawdzam czy przynajmniej jeden jest przypisany do produktu

                        if (!result)
                        {
                            exists = false;
                            break;
                        }
                    }
                    if (exists)
                        productIds.Add(productCatalogId);
                }

                return productIds.ToList();
            }
        }

        public List<ProductCatalogAttribute> GetShopCategoryAttributes(int shopCategoryManagerId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<PromotionAttribute>(x => x.ProductCatalogAttribute);
                dlo.LoadWith<ProductCatalogAttribute>(x => x.ProductCatalogAttributeGroup);

                ctx.LoadOptions = dlo;
                return ctx.ShopCategoryManagerAttribute.Where(x => x.ShopCategoryManagerId == shopCategoryManagerId)
                    .Select(x => x.ProductCatalogAttribute).ToList();
            }
        }

        public List<ProductCatalogAttributeGroup> GetProductCatalogAttributeGroupsNotInShop(Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                // pobierz grupy które już mają utworzone w sklepie
                int[] attributeGroupExistingIds = ctx.ProductCatalogShopAttribute
                    .Where(x => x.ShopAttribute.ShopAttributeGroup.ShopId == (int)shop && x.AttributeGroupId.HasValue)
                    .Select(x => x.AttributeGroupId.Value)
                    .ToArray();

                int[] attributeGroupTypeId = new int[] { 1, 2 };

                int[] attributeGroupIds = ctx.ProductCatalogAttributeGroup.Where(x => x.ExportToShop && attributeGroupTypeId.Contains(x.AttributeGroupTypeId))
                    .Select(x => x.AttributeGroupId)
                    .ToArray();

                // wybierz te grupy, których nie ma w sklepie

                int[] attributeGroupToCreateIds = attributeGroupIds.Except(attributeGroupExistingIds).ToArray();


                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAttributeGroup>(x => x.ProductCatalogAttribute);
                ctx.LoadOptions = dlo;


                List<Dal.ProductCatalogAttributeGroup> groupsToCreate = ctx.ProductCatalogAttributeGroup.Where(x => attributeGroupToCreateIds.Contains(x.AttributeGroupId) && x.ExportToShop).ToList();

                return groupsToCreate;


            }
        }
        public List<ProductCatalogAttribute> GetProductCatalogAttributesNotInShop(Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAttribute>(x => x.ProductCatalogAttributeGroup);
                ctx.LoadOptions = dlo;
                // pobierz atrybuty które już mają utworzone w sklepie
                int[] attributeExistingIds = ctx.ProductCatalogShopAttribute
                    .Where(x => x.ShopAttribute.ShopAttributeGroup.ShopId == (int)shop && x.AttributeId.HasValue)
                    .Select(x => x.AttributeId.Value)
                    .ToArray();


                int[] attributeIds = ctx.ProductCatalogAttribute.Where(x => x.ProductCatalogAttributeGroup.ExportToShop && x.ProductCatalogAttributeGroup.AttributeGroupTypeId==3)
                    .Select(x => x.AttributeId)
                    .ToArray();

                // wybierz te grupy, których nie ma w sklepie

                int[] attributeToCreateIds = attributeIds.Except(attributeExistingIds).ToArray();


                //DataLoadOptions dlo = new DataLoadOptions();
                //dlo.LoadWith<ProductCatalogAttributeGroup>(x => x.ProductCatalogAttribute);
                //ctx.LoadOptions = dlo;


                List<Dal.ProductCatalogAttribute> attributesToCreate = ctx.ProductCatalogAttribute.Where(x => attributeToCreateIds.Contains(x.AttributeId)).ToList();

                return attributesToCreate;


            }
        }

        public int SetShopAttributeGroup(Helper.Shop shop, bool exportToShop, string name, int externalGroupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ShopAttributeGroup sag = ctx.ShopAttributeGroup.Where(x => x.ShopId == (int)shop && x.Name.ToLower() == name.ToLower()).FirstOrDefault();

                if(sag!=null)
                {
                    
                }
                else
                {
                    sag = new ShopAttributeGroup()
                    {
                        IsActive = false,
                        IsSearchable = false,
                        Name = name,
                        ShopId = (int)shop,
                        ExternalShopAttributeGroupId = externalGroupId
                    };

                    ctx.ShopAttributeGroup.InsertOnSubmit(sag);

                }

                ctx.SubmitChanges();

                return sag.ShopAttributeGroupId;
            }
        }

        public int SetShopAttribute(Helper.Shop shop, bool isActive, string name, int externalId, int shopAttributeGroupId, int externalAttributeTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ShopAttribute sa = ctx.ShopAttribute.Where(x => x.ShopAttributeGroup.ShopId == (int)shop && x.ExternalShopAttributeId == externalId).FirstOrDefault();

                if(sa!=null)
                {

                }
                else
                {
                    sa = new ShopAttribute()
                    {
                        Description = null,
                        ExternalAttributeTypeId = externalAttributeTypeId,
                        ExternalShopAttributeId = externalId,
                        IsActive = isActive,
                        Name = name,
                        ShopAttributeGroupId = shopAttributeGroupId,
                        SortOrder = 0
                    };

                    ctx.ShopAttribute.InsertOnSubmit(sa);
                }
                ctx.SubmitChanges();

                return sa.ShopAttributeId;
            }
        }

        public void SetProductCatalogShopAttribute(int shopAttributeId, int? attributeId, int? attributeGroupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                    var a = ctx.ProductCatalogShopAttribute.Where(x => x.ShopAttributeId == shopAttributeId).AsQueryable();
                if (attributeGroupId.HasValue)
                    a = a.Where(x => x.AttributeGroupId.HasValue && x.AttributeGroupId == attributeGroupId);
                if (attributeId.HasValue)
                    a = a.Where(x => x.AttributeId.HasValue && x.AttributeId == attributeId);


                Dal.ProductCatalogShopAttribute psa = a.FirstOrDefault();

                if (psa == null)
                {
                    psa = new ProductCatalogShopAttribute()
                    {
                        ShopAttributeId = shopAttributeId,
                        AttributeGroupId = attributeGroupId,
                        AttributeId = attributeId
                    };
                    ctx.ProductCatalogShopAttribute.InsertOnSubmit(psa);
                }
                ctx.SubmitChanges();
            }
        }



        public ProductCatalogShopAttribute GetProductCatalogShopAttributeByAttributeId(Helper.Shop shop, int attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogShopAttribute>(x => x.ShopAttribute);
                dlo.LoadWith<ShopAttribute>(x => x.ShopAttributeGroup);

                ctx.LoadOptions = dlo;

                int attributeGroupId = ctx.ProductCatalogAttribute.Where(x => x.AttributeId == attributeId).FirstOrDefault().AttributeGroupId;
                int[] attributeIdsFromTheSameGroup = ctx.ProductCatalogAttribute.Where(x => x.AttributeGroupId == attributeGroupId).Select(x => x.AttributeId).ToArray();

                Dal.ProductCatalogShopAttribute psa = ctx.ProductCatalogShopAttribute
                    .Where(x => x.ShopAttribute.ShopAttributeGroup.ShopId == (int)shop && x.AttributeId.HasValue && attributeIdsFromTheSameGroup.Contains(x.AttributeId.Value))
                    .FirstOrDefault();

                return psa;

            }
        }

        public List<ProductCatalogAttributesForShopResult> GetProductCatalogAttributesForShop(Helper.Shop shop, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributesForShop((int)shop, productCatalogId).ToList();
            }
        }
      

        public void CreateAttributeGroupAndAssign(List<ProductCatalogShopAttribute> pcsas)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogShopAttribute.InsertAllOnSubmit(pcsas);
                ctx.SubmitChanges();
            }
        }

        public List<ShopAttributeGroup> GetShopAttributeGroups(Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopAttributeGroup.Where(x => x.ShopId == (int)shop).ToList();
            }
        }

        public List<ShopOrder> GetShopOrdersWithoutPayment(Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] status = new int[] { (int)Dal.Helper.OrderStatus.New, (int)Dal.Helper.OrderStatus.WaitingForPayment };
                return ctx.ShopOrder
                    .Where(x => x.ShopId == (int)shop
                    && x.CheckForPayment
                    && status.Contains(x.Order.OrderStatusId)
                    )
                    .ToList();
            }
        }

        public void SetShopOrderPayment(Helper.Shop shop, int orderId, OrderPayment orderPayment, DateTime? deliveryDate, int? deliveryDays)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ShopOrder so = ctx.ShopOrder.Where(x => x.ShopId == (int)shop && x.OrderId.Value == orderId).FirstOrDefault();

                try
                {
                    if (so != null)
                        so.CheckForPayment = false;

                    ctx.OrderPayment.InsertOnSubmit(orderPayment);

                    ctx.SubmitChanges();

                    Dal.Order o = ctx.Order.Where(x => x.ShopId == (int)shop && x.OrderId == orderId).FirstOrDefault();

                    if (o != null && deliveryDate != null)
                        o.DeliveryDate = deliveryDate;

                    if (o != null && deliveryDays != null)
                        o.DeliveryDays = deliveryDays;
                    ctx.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Dal.ErrorHandler.LogError(ex, String.Format("OrderId: {0}", orderId));
                }
            }
        }

        public ShopOrder GetShopOrder(Helper.Shop shop, int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopOrder.Where(x => x.ShopId == (int)shop && x.OrderId.Value == orderId).FirstOrDefault();

            }
        }
       

        public List<ShopOrder> GetOrdersSent(Helper.Shop shop)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                //int[] sentOrderIds = ctx.OrderStatusHistory
                //    .Where(x => x.Order.ShopId == (int)shop && x.OrderStatusId == (int)Dal.Helper.OrderStatus.Sent && x.SendNotification == null)
                //    .Select(x => x.OrderId)
                //    .ToArray();

                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ShopOrder>(x => x.Order);
                dlo.LoadWith<Order>(x => x.OrderShipping);
                dlo.LoadWith<OrderShipping>(x => x.ShippingCompany);
                ctx.LoadOptions = dlo;


                return ctx.ShopOrder
                    .Where(x => x.ShopId == (int)shop
                    && x.Order.OrderStatusId == (int)Dal.Helper.OrderStatus.Sent
                    && x.OrderId.HasValue
                    && x.Order.OrderShipping.ShipmentTrackingNumber != ""
                    && x.Order.OrderShipping.ShipmentTrackingNumber != null
                    && x.Order.OrderShipping.TrackingNumberSent == false)
                    .ToList();
            }
        }

    }
}
