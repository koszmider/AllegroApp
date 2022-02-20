using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Security.Cryptography.X509Certificates;

namespace LajtIt.Dal
{
 
    public partial class ProductCatalogHelper
    {


        public void SetProductCatalogImages(List<ProductCatalogImage> images, List<int> imagesToDelete)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] imageIds = images.Select(x => x.ImageId).ToArray();
                List<ProductCatalogImage> imagesToUpdate = ctx.ProductCatalogImage.Where(x => imageIds.Contains(x.ImageId)).ToList();

                foreach (ProductCatalogImage imageToUpdate in imagesToUpdate)
                {
                    ProductCatalogImage image = images.Where(x => x.ImageId == imageToUpdate.ImageId).FirstOrDefault();
                    if (image != null)
                    {
                        imageToUpdate.Priority = image.Priority;
                        imageToUpdate.IsActive = image.IsActive;
                        //imageToUpdate.IsThumbnail = image.IsThumbnail;
                        //imageToUpdate.Description = image.Description;
                        //imageToUpdate.Title = image.Title;
                        //imageToUpdate.ImageTypeId = image.ImageTypeId;
                        //imageToUpdate.LinkUrl = image.LinkUrl;

                    }

                }

                ctx.SubmitChanges();
                List<ProductCatalogImage> toDelete = ctx.ProductCatalogImage.Where(x => imagesToDelete.Contains(x.ImageId)).ToList();
                List<ProductCatalogShopImage> toDelete2 = ctx.ProductCatalogShopImage.Where(x => imagesToDelete.Contains(x.ImageId)).ToList();
                List<Dal.ProductCatalogImageAllegroItem> toDelete1 = ctx.ProductCatalogImageAllegroItem.Where(x => x.ImageId.HasValue && imagesToDelete.Contains(x.ImageId.Value)).ToList();
                ctx.ProductCatalogShopImage.DeleteAllOnSubmit(toDelete2);
                ctx.ProductCatalogImage.DeleteAllOnSubmit(toDelete);
                ctx.ProductCatalogImageAllegroItem.DeleteAllOnSubmit(toDelete1);
                ctx.SubmitChanges();
            }

        }

        public List<ProductCatalogAttributeToProduct> GetProductCatalogAttributeProducts(int[] productCatalogIds, int[] attributeIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAttributeToProduct>(x => x.ProductCatalogAttribute);
                ctx.LoadOptions = dlo;

                return ctx.ProductCatalogAttributeToProduct
                    .Where(x => productCatalogIds.Contains(x.ProductCatalogId) && attributeIds.Contains(x.AttributeId))
                    .ToList();
            }
        }

        public void SetProductCatalogImagesOrder(int[] imageIdInOrder)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ProductCatalogImage> images = ctx.ProductCatalogImage.Where(x => imageIdInOrder.Contains(x.ImageId)).ToList();

                int pcId = images[0].ProductCatalogId;

               


                int order = 1;
                images = ctx.ProductCatalogImage.Where(x => x.ProductCatalogId == pcId).ToList();
                int[] imagesToDelete = images.Where(x => !imageIdInOrder.Contains(x.ImageId)).Select(x => x.ImageId).ToArray();

                foreach (Dal.ProductCatalogImage image in images)
                {
                    //Dal.ProductCatalogImage image = images.Where(x => x.ImageId == id).FirstOrDefault();
                    if (image!=null)
                    {
                        image.Priority = imageIdInOrder.FindIndex<int>(image.ImageId);

                        if (!imageIdInOrder.Contains(image.ImageId))
                            image.IsActive = false;
                        order += 1;
                    }
                }

                ctx.SubmitChanges();
                List<ProductCatalogImage> toDelete = ctx.ProductCatalogImage.Where(x => imagesToDelete.Contains(x.ImageId)).ToList();
                List<ProductCatalogShopImage> toDelete2 = ctx.ProductCatalogShopImage.Where(x => imagesToDelete.Contains(x.ImageId)).ToList();
                List<Dal.ProductCatalogImageAllegroItem> toDelete1 = ctx.ProductCatalogImageAllegroItem.Where(x => x.ImageId.HasValue && imagesToDelete.Contains(x.ImageId.Value)).ToList();
                ctx.ProductCatalogShopImage.DeleteAllOnSubmit(toDelete2);
                ctx.ProductCatalogImage.DeleteAllOnSubmit(toDelete);
                ctx.ProductCatalogImageAllegroItem.DeleteAllOnSubmit(toDelete1);
                ctx.SubmitChanges();



            }
        }

        public List<Dal.ProductCatalogAttributeGroupingType> GetProductCatalogAttributeGroupingTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeGroupingType.ToList();
            }
        }

        //public List<EmpikView> GetProductCatalogEmpik()
        //{
        //    using (LajtitViewsDB ctx = new LajtitViewsDB())
        //    {
        //        return ctx.EmpikView.ToList();
        //    }
        //}

        public SupplierOwner GetSupplierOwner(string v)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierOwner.Where(x => x.Name == "King Home").FirstOrDefault();
            }
        }

       



        public int GetProductCatalogAttributeDefaultGrouping(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                int attributeId = ctx.ProductCatalogAttributeToProduct.Where(x => x.ProductCatalogId == productCatalogId 
                && x.ProductCatalogAttribute.AttributeGroupId == 6 
                && x.IsDefault.HasValue == true
                && x.IsDefault.Value)
                    .Select(x => x.AttributeId)
                    .FirstOrDefault();
                
                return ctx.ProductCatalogAttributeGroupingAttribute
                    .Where(x => x.ProductCatalogAttributeGrouping.GroupingTypeId == 2
                    && x.ProductCatalogAttribute.AttributeGroupId == 6
                    && x.AttributeId == attributeId)
                    .Select(x => x.AttributeGroupingId)
                    .FirstOrDefault();
            }
        }

        public void SetProductCatalogUpdateNordlux(List<ProductCatalog> products)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int supplierId = products[0].SupplierId;
                int[] productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId).Select(x => x.ProductCatalogId).ToArray();

                foreach (int pId in productsToUpdate)
                {

                    Dal.ProductCatalog productToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId == pId).FirstOrDefault();

                    Dal.ProductCatalog pc;

                    pc = products.Where(x => x.Code == productToUpdate.Code).FirstOrDefault();

                    productToUpdate.UpdateReason = "Aktualizacja automatyczna";
                    productToUpdate.UpdateUser = "system";



                    if (pc != null)
                    {
                        productToUpdate.IsAvailable = pc.IsAvailable;
                        productToUpdate.SupplierQuantity = pc.SupplierQuantity;

                        //   if (pc.Ean != null && productToUpdate.Ean == null)
                        //      productToUpdate.Ean = pc.Ean;
                        productToUpdate.Code = pc.Code;
                    }

                    try
                    {
                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        Dal.ErrorHandler.LogError(ex, String.Format("ProductCatalogId: {0}", productToUpdate.ProductCatalogId));
                    }

                }

            }
        }

        public void SetProductCatalogUpdateMaxlight(List<ProductCatalog> products)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int supplierId = products[0].SupplierId;
                int[] productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId).Select(x=>x.ProductCatalogId).ToArray();

                foreach (int pId in productsToUpdate)
                {
                    
                    Dal.ProductCatalog productToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId == pId).FirstOrDefault();

                    Dal.ProductCatalog pc;
                    
                    if(productToUpdate.Ean!=null)
                        pc = products.Where(x => x.Ean == productToUpdate.Ean).FirstOrDefault();
                    else
                        pc = products.Where(x => x.Code.Trim().ToLower() == productToUpdate.Code.Trim().ToLower() && productToUpdate.Ean == null).FirstOrDefault();

                    productToUpdate.UpdateReason = "Aktualizacja automatyczna";
                    productToUpdate.UpdateUser = "system";
                    
               

                    if (pc == null)
                    {
                        productToUpdate.IsAvailable = false;
                        productToUpdate.SupplierQuantity = null;
                    }
                    else
                    {
                        productToUpdate.IsAvailable = pc.IsAvailable;
                        productToUpdate.SupplierQuantity = pc.SupplierQuantity;

                        //   if (pc.Ean != null && productToUpdate.Ean == null)
                        //      productToUpdate.Ean = pc.Ean;
                        productToUpdate.Code = pc.Code;
                    }

                    try
                    {
                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        Dal.ErrorHandler.LogError(ex, String.Format("ProductCatalogId: {0}", productToUpdate.ProductCatalogId));
                    }

                }



                //foreach (Dal.ProductCatalog productToAdd in products)
                //{
                //    Dal.ProductCatalog productInCatalog = ctx.ProductCatalog
                //        .Where(x => x.SupplierId== supplierId &&  x.Ean == productToAdd.Ean)
                //        .FirstOrDefault();

                //    if (productInCatalog != null)
                //        continue;

                //    string description = productToAdd.Specification;
                //    string line =        productToAdd.UpdateReason;

                //    productToAdd.Specification=null;
                //    productToAdd.UpdateReason =null;

                //    Dal.ProductCatalogGroup group =  ctx.ProductCatalogGroup.Where(x => x.SupplierId == supplierId && x.GroupName == line).FirstOrDefault();

                //    if(group!=null)
                //    {
                //        ctx.ProductCatalogGroupProduct.InsertOnSubmit(new ProductCatalogGroupProduct()
                //        {
                //            InsertDate = DateTime.Now,
                //            InsertUser = "system",
                //            ProductCatalog = productToAdd,
                //            ProductCatalogGroup = group

                //        });
                //    }
                //    else
                //    {
                //        Dal.ProductCatalogGroupFamily family = ctx.ProductCatalogGroupFamily
                //            .Where(x => x.SupplierId == supplierId && x.FamilyName == line).FirstOrDefault();

                //        if(family==null)
                //        {
                //            family = new ProductCatalogGroupFamily()
                //            {
                //                FamilyTypeId = 1,
                //                FamilyName = line,
                //                SupplierId = supplierId
                //            };
                //            //ctx.ProductCatalogGroupFamily.InsertOnSubmit(family);

                //        }
                //        //family = ctx.ProductCatalogGroupFamily
                //        //.Where(x => x.SupplierId == supplierId && x.FamilyName == "").FirstOrDefault();


                //        int maxId = ctx.ProductCatalogGroup.Max(x => x.ProductCatalogGroupId) + 1;
                //        Dal.ProductCatalogGroup groupToAdd = new ProductCatalogGroup()
                //        {
                //            ProductCatalogGroupId= maxId,
                //            GroupName = line,
                //            SupplierId = supplierId,
                //            ProductCatalogGroupFamily = family
                //        };

                //        ctx.ProductCatalogGroupProduct.InsertOnSubmit(new ProductCatalogGroupProduct()
                //        {
                //            InsertDate = DateTime.Now,
                //            InsertUser = "system",
                //            ProductCatalog = productToAdd,
                //            ProductCatalogGroup = groupToAdd

                //        });

                //    }

                //    //ctx.ProductCatalog.InsertOnSubmit(productToAdd);




                //    try
                //    {
                //        ctx.SubmitChanges();
                //    }
                //    catch (Exception ex)
                //    {
                //        Dal.ErrorHandler.LogError(ex, String.Format("ProductCatalog.Code: {0}", productToAdd.Code));
                //    }
         //   }
            }
        }

        public void SetProductCatalogHubschUpdate(List<ProductCatalog> products, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId).ToList();

                foreach(Dal.ProductCatalog pToUpdate in productsToUpdate)
                {
                    Dal.ProductCatalog product = products.Where(x => x.Ean == pToUpdate.Ean).FirstOrDefault();

                    if(product!=null)
                    {
                        pToUpdate.IsAvailable = product.IsAvailable;
                        pToUpdate.SupplierQuantity = product.SupplierQuantity;
                    }

                }

                ctx.SubmitChanges();
            }
        }
    

        public void ProductCatalogShopProductName(int productCatalogId, int shopId, string name)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogShopProduct psp = ctx.ProductCatalogShopProduct.Where(x => x.ProductCatalogId == productCatalogId && x.ShopId == shopId).FirstOrDefault();

                if (psp != null && psp.IsNameLocked)
                    return;

                if (psp == null)
                {
                    psp = new ProductCatalogShopProduct()
                    {
                        ProductCatalogId = productCatalogId,
                        ShopId = shopId,
                        Name = name
                    };
                    ctx.ProductCatalogShopProduct.InsertOnSubmit(psp);
                }
                else
                {
                    psp.Name = name;
                }

                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogAttributeGroup> GetProductCatalogAttributeGroupingAttributeGroups(int attributeGroupingId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAttributeGroupingAttributeGroup>(x => x.ProductCatalogAttributeGroup);
                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogAttributeGroupingAttributeGroup.Where(x => x.AttributeGroupingId == attributeGroupingId).Select(x=>x.ProductCatalogAttributeGroup).ToList();
            }
        }
        public List<ProductCatalogAttribute> GetProductCatalogAttributeGroupingAttributes(int attributeGroupingId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeGroupingAttribute.Where(x => x.AttributeGroupingId == attributeGroupingId).Select(x=>x.ProductCatalogAttribute).ToList();
            }
        }

        public ProductCatalogProductTypeSchema GetProductCatalogProductTypeSchema(int schemaId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogProductTypeSchema.Where(x => x.ProductTypeSchemaId == schemaId).FirstOrDefault();
            }
        }

        public ProductCatalogAttributeGrouping GetProductCatalogAttributeGrouping(int attributeGroupingId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeGrouping.Where(x => x.AttributeGroupingId == attributeGroupingId).FirstOrDefault();
            }
        }

        public List<Dal.ProductCatalogAttributeGrouping> GetProductCatalogAttributeGroupings(int groupingTypeId)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeGrouping.Where(x=>x.GroupingTypeId== groupingTypeId).OrderBy(x=>x.Name).ToList();
            }
        }

        //public void SetProductCatalogAttributeGroupingAttribute(ProductCatalogAttributeGroupingAttribute att)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        if (ctx.ProductCatalogAttributeGroupingAttribute.Where(x => x.AttributeId == att.AttributeId && x.AttributeGroupingId == att.AttributeGroupingId).Count() == 0)
        //        {
        //            ctx.ProductCatalogAttributeGroupingAttribute.InsertOnSubmit(att);
        //            ctx.SubmitChanges();
        //        }
        //    }
        //}

        public int SetProductCatalogAttributeGrouping(ProductCatalogAttributeGrouping scm)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogAttributeGrouping.InsertOnSubmit(scm);
                ctx.SubmitChanges();

                return scm.AttributeGroupingId;
            }
        }

        //public void SetProductCatalogAttributeGroupingAttributeGroup(ProductCatalogAttributeGroupingAttributeGroup att)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        if (ctx.ProductCatalogAttributeGroupingAttributeGroup.Where(x => x.AttributeGroupId == att.AttributeGroupId && x.AttributeGroupingId == att.AttributeGroupingId).Count() == 0)
        //        {
        //            ctx.ProductCatalogAttributeGroupingAttributeGroup.InsertOnSubmit(att);
        //            ctx.SubmitChanges();
        //        }
        //    }
        //}

        public void SetProductCatalogAttributeGrouping(ProductCatalogAttributeGrouping scm, int[] attributes, int[] groups)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogAttributeGrouping g = ctx.ProductCatalogAttributeGrouping.Where(x => x.AttributeGroupingId == scm.AttributeGroupingId).FirstOrDefault();

                g.Name = scm.Name;

                var attToDelete = ctx.ProductCatalogAttributeGroupingAttribute.Where(x => x.AttributeGroupingId == scm.AttributeGroupingId).ToList();
                var attGrDelete = ctx.ProductCatalogAttributeGroupingAttributeGroup.Where(x => x.AttributeGroupingId == scm.AttributeGroupingId).ToList();

                ctx.ProductCatalogAttributeGroupingAttribute.DeleteAllOnSubmit(attToDelete);
                ctx.ProductCatalogAttributeGroupingAttributeGroup.DeleteAllOnSubmit(attGrDelete);

                ctx.ProductCatalogAttributeGroupingAttribute.InsertAllOnSubmit(attributes.Select(x => new ProductCatalogAttributeGroupingAttribute()
                {
                    AttributeGroupingId = scm.AttributeGroupingId,
                    AttributeId = x
                }));
                ctx.ProductCatalogAttributeGroupingAttributeGroup.InsertAllOnSubmit(groups.Select(x => new ProductCatalogAttributeGroupingAttributeGroup()
                {
                    AttributeGroupingId = scm.AttributeGroupingId,
                    AttributeGroupId = x
                }));

                ctx.SubmitChanges();
            }
        }

        public void SetProductCatalogAttribute(ProductCatalogAttributeToProduct atp)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogAttributeToProduct.DeleteAllOnSubmit(
                    ctx.ProductCatalogAttributeToProduct.Where(x => x.ProductCatalogId == atp.ProductCatalogId && x.ProductCatalogAttribute.AttributeGroupId == 6));
                ctx.ProductCatalogAttributeToProduct.InsertOnSubmit(atp);
                ctx.SubmitChanges();
            }
        }

        public List<Supplier> GetSuppliersByOwner(int supplierOwnerId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Supplier.Where(x => x.SupplierOwnerId == supplierOwnerId).ToList();
            }
        }

        public List<FtpFilesOnServerNotInDB> GetFtpFiles()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.FtpFilesOnServerNotInDB.ToList();
            }
        }
        public void SetFtpFiles(List<string> files)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ExecuteCommand("truncate table FtpFiles");
                ctx.SubmitChanges();
                ctx.FtpFiles.InsertAllOnSubmit(files.Select(x => new FtpFiles() { FileName = x }));
                ctx.SubmitChanges();

            }
        }

        public List<Shop> GetProductCatalogProductTypeShops()
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                return ctx.ProductCatalogProductTypeShop
                    .Select(x => x.Shop)
                    .ToList();

            }
        }

        public List<Shop> GetProductCatalogProductTypeShops(int schemaId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                 
                return ctx.ProductCatalogProductTypeShop.Where(x => x.ProductTypeSchemaId == schemaId)
                    .Select(x=>x.Shop)
                    .ToList();

            }
        }

        public List<ProductCatalogProductTypeMembers> GetProductTypeSchemaAttributes(int schemaId, int attributeId)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogProductTypeMembers>(x => x.ProductCatalogAttributeGroup);
                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogProductTypeMembers.Where(x => x.ProductTypeSchemaId == schemaId && x.ProductTypeAttributeId == attributeId).ToList();

            }
        }

        public List<ProductCatalog> GetProductCatalogBySupplierOwner(int supplierOwnerId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalog.Where(x => x.Supplier.SupplierOwnerId == supplierOwnerId).ToList();
            }
        }

        public void SetProductCatalogStatusPolux(int supplierId, Dictionary<string, string> polux)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ProductCatalog> products = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId).ToList();

                foreach (Dal.ProductCatalog product in products)
                {
                    if (polux.ContainsKey(product.Ean))
                    {
                        product.IsAvailable = polux[product.Ean] == "Dostępny";
                    }
                    // na podstawie sitemap ktora jest nieaktualna mozemy zbyt wiele produktow usunac.
                    // else
                    //product.IsAvailable = false;0
                }
                ctx.SubmitChanges();

            }
        }

        public List<ProductCatalogProductTypeSchema> GetProductTypeSchemas()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogProductTypeSchema.ToList();

            }
        }



        public List<ProductCatalogImage> GetProductCatalogImagesToUpload()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogImage.Where(x => x.UploadedToServer == false).ToList();
            }
        }

        public List<ProductCatalogImage> GetProductCatalogImagesToUploadBySupplierId(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ProductCatalog> products = ctx.ProductCatalog.Where(x => x.SupplierId == id).ToList();
                List<ProductCatalogImage> pciList = new List<ProductCatalogImage>();
                foreach (Dal.ProductCatalog pc in products)
                {
                    List < ProductCatalogImage > tmpList = ctx.ProductCatalogImage.Where(x => x.UploadedToServer == false && x.ProductCatalogId == pc.ProductCatalogId).ToList();
                    tmpList.ForEach(item => pciList.Add(item));
                }
                
                return pciList;
            }
        }



        public void SetSupplierDelivery(int supplierId, int d1, int d2)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Supplier supplier = ctx.Supplier.Where(x => x.SupplierId == supplierId).FirstOrDefault();
                supplier.DeliveryCostTypeId = d1;
                supplier.DeliveryCostTypeNoPaczkomatId = d2;

                ctx.SubmitChanges();
            }
        }

 

        public void SetProductCatalogProductTypeShops(int schemaId, int[] shopIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogProductTypeShop.InsertAllOnSubmit(shopIds.Select(x => new ProductCatalogProductTypeShop()
                {
                    ProductTypeSchemaId = schemaId,
                    ShopId = x
                }));
                ctx.SubmitChanges();
            }
        }


        public ProductCatalogMixerAttributeGroup GetProductCatalogMixerAttributeGroup(int attributeGroupId, int attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Random rand = new Random();

                var attr = ctx.ProductCatalogMixerAttributeGroup
                    .Where(x => x.AttributeGroupId == attributeGroupId && x.AttributeId == attributeId && x.IsActive == true)
                    .ToList();

                if (attr.Count() == 0)
                    return null;

                return attr[rand.Next(attr.Count)];
            }
        }

        public void SetProductCatalogProductTypeMembers(int schemaId, int attributeId, int[] attIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int order = 1;
                Dal.ProductCatalogProductTypeMembers memeberLast = ctx.ProductCatalogProductTypeMembers.Where(x => x.ProductTypeSchemaId == schemaId && x.ProductTypeAttributeId == attributeId)
                    .OrderByDescending(x => x.Order)
                    .FirstOrDefault();

                if (memeberLast != null)
                    order = memeberLast.Order + 1;

                foreach(int attributeGroupId in attIds)
                {
                    Dal.ProductCatalogProductTypeMembers m = new ProductCatalogProductTypeMembers()
                    {
                        Order = order,
                        ProductTypeSchemaId = schemaId,
                        ProductTypeAttributeId = attributeId,
                        AttributeGroupId = attributeGroupId
                    };
                    ctx.ProductCatalogProductTypeMembers.InsertOnSubmit(m);
                    order++;
                }
                ctx.SubmitChanges();
            }
        }

        public void SetProductCatalogProductTypesForGroup(int schemaId, int attributeGroupId, int[] attIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                foreach (int attributeId in attIds)
                {

                    int order = 1;
                    Dal.ProductCatalogProductTypeMembers memeberLast = ctx.ProductCatalogProductTypeMembers
                        .Where(x => x.ProductTypeSchemaId == schemaId && x.ProductTypeAttributeId == attributeId)
                        .OrderByDescending(x => x.Order)
                        .FirstOrDefault();

                    if (memeberLast != null)
                        order = memeberLast.Order + 1;


                    Dal.ProductCatalogProductTypeMembers m = new ProductCatalogProductTypeMembers()
                    {
                        Order = order,
                        ProductTypeSchemaId = schemaId,
                        ProductTypeAttributeId = attributeId,
                        AttributeGroupId = attributeGroupId
                    };
                    ctx.ProductCatalogProductTypeMembers.InsertOnSubmit(m);
                
                }
                ctx.SubmitChanges();
            }
        }
        public List<ProductCatalogAttribute> GetProductTypesForSchema(int schemaId, int attributeGroupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogProductTypeMembers
                    .Where(x => x.ProductTypeSchemaId == schemaId && x.AttributeGroupId == attributeGroupId)
                    .Select(x => x.ProductCatalogAttribute)
                    .ToList();
            }
        }

        public void SetProductCatalogProductTypeShopsDelete(int schemaId, int[] shopIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var toDelete = ctx.ProductCatalogProductTypeShop
                     .Where(x => x.ProductTypeSchemaId == schemaId && shopIds.Contains(x.ShopId))
                     .ToList();

                ctx.ProductCatalogProductTypeShop.DeleteAllOnSubmit(toDelete);
                ctx.SubmitChanges();
            }
        }

      
        public ProductCatalogAttributeCategoryShop GetProductCatalogAttributeCategory(Dal.Helper.ShopType shopType, int attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeCategoryShop
                    .Where(x => x.ShopTypeId == (int)shopType && x.AttributeId == attributeId)
                    .FirstOrDefault();
            }
        }
        public void SetProductCatalogProductTypeMembersUpdate(int schemaId, int attributeId, List<ProductCatalogProductTypeMembers> membersToUpdate)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalogProductTypeMembers> members = ctx.ProductCatalogProductTypeMembers
                    .Where(x => x.ProductTypeSchemaId == schemaId && x.ProductTypeAttributeId == attributeId).ToList();

                foreach (ProductCatalogProductTypeMembers m in members)
                {
                    ProductCatalogProductTypeMembers memberToUpdate = membersToUpdate.Where(x => x.AttributeGroupId == m.AttributeGroupId).FirstOrDefault();
                    if (memberToUpdate != null)
                        m.Order = memberToUpdate.Order;
                    if(memberToUpdate.Order==-1)
                        ctx.ProductCatalogProductTypeMembers.DeleteOnSubmit(m);
                    

                }
                ctx.SubmitChanges();
            }
        }

        public void SetProductCatalogProductTypeMembersDelete(int schemaId, int attributeGroupId, List<ProductCatalogProductTypeMembers> membersToDelete)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalogProductTypeMembers> members = ctx.ProductCatalogProductTypeMembers
                    .Where(x => x.ProductTypeSchemaId == schemaId && x.AttributeGroupId == attributeGroupId).ToList();

                foreach (ProductCatalogProductTypeMembers m in members)
                {
                    ProductCatalogProductTypeMembers memberToUpdate = membersToDelete.Where(x => x.ProductTypeAttributeId == m.ProductTypeAttributeId).FirstOrDefault();
                    if (memberToUpdate != null)
                        ctx.ProductCatalogProductTypeMembers.DeleteOnSubmit(m);
                }
                ctx.SubmitChanges();
            }
        }
        public void SetProductCatalogProductTypeSchema(string name)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogProductTypeSchema schema = new ProductCatalogProductTypeSchema()
                {
                    CanBeSendToShop = true,
                    SchemaName = name
                };
                ctx.ProductCatalogProductTypeSchema.InsertOnSubmit(schema);
                ctx.SubmitChanges();
            }
        }


        public List<ProductCatalogShopFnResult> GetProductCatalogShops(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopFn(productCatalogId).ToList();
            }
        }
 


        public List<ProductCatalogDuplicates> GetProductCatalogDuplicates()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogDuplicates.OrderByDescending(x=>x.ProductCount).ToList();
            }
        }

        public List<ProductCatalogAttributesView> GetProductCatalogAttributesView()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogAttributesView.OrderBy(x => x.GroupName).ThenBy(x => x.AttributeName).ToList();
            }
        }

        public List<ProductCatalogDuplicatesFnResult> GetProductCatalogDuplicates(string ean)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogDuplicatesFn(ean).ToList();
            }
        }

        public void SetProductCatalogShop(List<ProductCatalogShopProduct> shops, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalogShopProduct> shopsToUpdate = ctx.ProductCatalogShopProduct.Where(x => x.ProductCatalogId == productCatalogId).ToList();

                foreach(Dal.ProductCatalogShopProduct shop in shops)
                {
                    Dal.ProductCatalogShopProduct shopToUpdate = shopsToUpdate.Where(x => x.ShopId == shop.ShopId).FirstOrDefault();

                    if (shopToUpdate == null)
                        ctx.ProductCatalogShopProduct.InsertOnSubmit(shop);
                    else
                    {
                        shopToUpdate.IsNameLocked = shop.IsNameLocked;
                        shopToUpdate.Name = shop.Name;
                    }
                }
                ctx.SubmitChanges();
            }
        }

       
 

        public List<ShopType> GetShopTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopType.ToList();
            }
        }
        public int SetProductCatalogImageMain(int imageId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogImage image = ctx.ProductCatalogImage.Where(x => x.ImageId == imageId).FirstOrDefault();

                image.Priority = 1;//koszmid
                int priority = 1;

                foreach(Dal.ProductCatalogImage img in ctx.ProductCatalogImage.Where(x=>x.ProductCatalogId==image.ProductCatalogId
                && x.ImageId!=image.ImageId).OrderBy(x=>x.Priority).ToList())
                {
                    img.Priority = priority++;

                }
                ctx.SubmitChanges();

                return image.ProductCatalogId;
            }
        }
         

        public void SetProductCatalogSubProduct(int productCatalogId, int subProductCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var p = ctx.ProductCatalogSubProduct.Where(x => x.ProductCatalogId == productCatalogId && x.ProductCatalogRefId == subProductCatalogId).FirstOrDefault();

                if(p==null)
                {
                    Dal.ProductCatalogSubProduct sp = new ProductCatalogSubProduct()
                    {
                        IsRequired = true,
                        ProductCatalogId = productCatalogId,
                        ProductCatalogRefId = subProductCatalogId,
                        Quantity = 1,
                        Rebate = 0,
                        SubProductGroupId = 0
                    };

                    ctx.ProductCatalogSubProduct.InsertOnSubmit(sp);
                    ctx.SubmitChanges();
                }
            }
        }

        public List<SupplierDeliveryType> GetSupplierDeliveryTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierDeliveryType.ToList();

            }
        }

        public void SetProductCatalogUpdateSompex(List<ProductCatalog> products)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                int[] supplierId = products.Select(x => x.SupplierId).Distinct().ToArray();


            }

            }

        public void SetProductCatalogSubProductsUpdate(int productCatalogId, List<ProductCatalogSubProduct> products)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalogSubProduct> existingProducts = ctx.ProductCatalogSubProduct
                    .Where(x => x.ProductCatalogId == productCatalogId)
                    .ToList();
                int[] updatedIds = products.Select(x => x.Id).ToArray();
                var toDelete = existingProducts.Where(x => !updatedIds.Contains(x.Id)).ToList();

                foreach(var p in products)
                {
                    var productToUpdate = existingProducts.Where(x => x.Id == p.Id).FirstOrDefault();

                    productToUpdate.Quantity = p.Quantity;
                    productToUpdate.Rebate = p.Rebate;
                }

                ctx.ProductCatalogSubProduct.DeleteAllOnSubmit(toDelete);
                ctx.SubmitChanges();
            }
        }

        public ProductCatalogAllegroCategory GetProductCatalogAllegroCategory(int productCatalogId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogAllegroCategory.Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();
            }
        }
 
        //public List<ProductCatalogAllegroItemsFnResult> GetProductCatalogAllegroItems(string itemStatus, 
        //    string searchText,
        //    bool? isValid, 
        //    string errorMsg,
        //    long[] userIds )
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {

        //        IQueryable<Dal.ProductCatalogAllegroItemsFnResult> q;

        //        if (itemStatus == "NOTCREATED")
        //            q = GetItems(ctx.ProductCatalogAllegroDraftItemsFn());
        //        else
        //            q = ctx.ProductCatalogAllegroItemsFn(1000, itemStatus, null, isValid, null);

        //        if (itemStatus == "ACTIVE")
        //            q = q.Where(x => x.UpdateCommand != null);

        //        if (searchText != "")
        //            q = q.Where(x => x.ItemId.ToString() == searchText);

        //        if (!String.IsNullOrEmpty(errorMsg) )
        //            q = q.Where(x => x.Comment!=null && x.Comment.ToLower().Contains(errorMsg.ToLower().Trim()));

        //        if(userIds.Length>0)
        //            q=q.Where(x => userIds.Contains(x.UserId));

        //        return q.ToList();
        //    }
        //}

        public void SetProductCatalogDelivery(int[] productIds, int deliveryId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach(int pId in productIds)
                {
                    var p = ctx.ProductCatalog.Where(x => x.ProductCatalogId == pId).FirstOrDefault();
                    p.DeliveryId = deliveryId;
                    p.UpdateReason = "Zmiana ręczna";
                    p.UpdateUser = userName;
                    ctx.SubmitChanges();
                }
            }
        }

        //private IQueryable<ProductCatalogAllegroItemsFnResult> GetItems(IQueryable<ProductCatalogAllegroDraftItemsFnResult> queryable)
        //{
        //    return queryable.Select(x =>
        //    new ProductCatalogAllegroItemsFnResult()
        //    {
        //        CommandId = null,
        //        Comment = null,
        //        HasOrders = false,
        //        Id = 0,
        //        //ImageFullName = x.ImageFullName,
        //        InsertDate = DateTime.Now,
        //        IsImageReady = false,
        //        IsValid = false,
        //        ItemId = 0,
        //        NotificationSent = false,
        //        PriceBruttoMinimum = x.PriceBruttoMinimum, 
        //        ProductCatalogId = x.ProductCatalogId,
        //        ProductName = x.ProductName,
        //        ShopId = x.ShopId,
        //        UpdateCommand = "",
        //        UserId = x.UserId,
        //        UserName = x.UserName,
        //        ValidatedAt = DateTime.Now
        //    });
        //}

        public void SetSearchTableAttributeDelete(Guid searchId, int attributeGroupId, int? attributeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.SearchTableAttributes sta;
                if (attributeId.HasValue)
                    sta = ctx.SearchTableAttributes.Where(x => x.SearchId == searchId && x.AttributeGroupId == attributeGroupId && x.AttributeId == attributeId.Value).FirstOrDefault();
                else
                    sta = ctx.SearchTableAttributes.Where(x => x.SearchId == searchId && x.AttributeGroupId == attributeGroupId).FirstOrDefault();

                ctx.SearchTableAttributes.DeleteOnSubmit(sta);
                ctx.SubmitChanges();
            }
        }

        public int[] GetProductCatalogAttributeGroups(int[] attributeIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttribute.Where(x => attributeIds.Contains(x.AttributeId))
                    .Select(x => x.AttributeGroupId)
                    .Distinct()
                    .ToArray();
            }
        }

        //public List<ProductCatalogAllegroItemsFnResult> GetProductCatalogAllegroItems(int limit, string itemStatus)
        //{
        //    return GetProductCatalogAllegroItems(limit, null, itemStatus);
        //}
        //public List<ProductCatalogAllegroItemsFnResult> GetProductCatalogAllegroItems(int limit, int? productCatalogId, params string[] itemStatuses)
        //{
        //    List<ProductCatalogAllegroItemsFnResult> items = new List<ProductCatalogAllegroItemsFnResult>();

        //    foreach (string itemStatus in itemStatuses)
        //        items.AddRange(GetAllegroItemsToUpdate(limit, itemStatus, productCatalogId));

        //    return items;
        //}
        public List<TableLogView> GetTableLogHistory(int objectId, string[] tables)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.TableLogView.Where(x => tables.Contains(x.TableName) && x.ObjectId == objectId)
                    .OrderByDescending(x => x.InsertDate)
                    .ToList();
            }
        }

        public void SetProductOrder(Dal.ProductOrder po)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
             
                ctx.ProductOrder.InsertOnSubmit(po);
                ctx.SubmitChanges();
            }
        }

        public List<TableLogView> GetTableLogHistory(int objectId, string[] tables, string columnName)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                var r = ctx.TableLogView.Where(x => tables.Contains(x.TableName) && x.ObjectId == objectId);
                if (columnName != "")
                    r = r.Where(x => x.ColumnName == columnName);

                return r.OrderByDescending(x=>x.InsertDate).ToList();
            }
        }

        public void SetProductCatalogDeleteDuplicates(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogDuplicatesDelete(productCatalogId);

            }
        }

        public List<ProductCatalogOnStock> GetProductCatalogOnStock()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogOnStock.OrderBy(x => x.SupplierId).ToList();
            }
        }


        //public void SetPromotionRun(int promotionGroupId, bool checked1, bool checked2)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
                
        //    }
        //}

        //public List<Dal.ProductCatalogView> GetProductCatalogForPosnet()
        //{
        //    using (LajtitViewsDB ctx = new LajtitViewsDB())
        //    { 
        //        ctx.CommandTimeout = 30000;
        //        return ctx.ProductCatalogView.Where(x=>x.PosnetUniqueId !=null ).ToList();
        //    }
        //}

        public void SetProductCatalogAllegroItemImageDelete(long itemId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalogImageAllegroItem> images = ctx.ProductCatalogImageAllegroItem.Where(x => x.ItemId == itemId).ToList();

                ctx.ProductCatalogImageAllegroItem.DeleteAllOnSubmit(images);
                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalog> GetProductCatalog(bool onlyActive)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (onlyActive)
                    return ctx.ProductCatalog.Where(x => x.IsActive == true).ToList();
                else
                    return ctx.ProductCatalog.Where(x => x.IsHidden == false && x.IsDiscontinued == false).ToList();
            }
        }

        public void SetProductCatalogName(int shopId, int productCatalogId, string name)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogShopProduct pcs = ctx.ProductCatalogShopProduct.Where(x => x.ShopId == shopId && x.ProductCatalogId == productCatalogId).FirstOrDefault();

                if (pcs == null)
                    ctx.ProductCatalogShopProduct.InsertOnSubmit(new ProductCatalogShopProduct()
                    {
                        IsNameLocked = false,
                        ProductCatalogId = productCatalogId,
                        ShopId = shopId,
                        Name = name
                    });
                else
                {
                    if (pcs.IsNameLocked == false)
                    {
                        pcs.Name = name;
                    }
                }
                ctx.SubmitChanges();
            }
        }
        public List<Dal.ProductCatalogView> GetProductCatalogForSearch(string query, int count)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogView.Where(x =>
                x.Code.Contains(query)
                || x.Name.Contains(query) 
                || x.Ean.Contains(query)
                )
                .OrderBy(x=>x.SupplierName)
                .Take(count)
                .ToList();
            }
        }
        public List<Dal.ProductCatalogView> GetProductCatalogForSearch(int supplierId, string query, int count)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogView.Where(x =>
                    x.SupplierId == supplierId
                    &&
                    (
                        x.Code.Contains(query)
                        || x.Name.Contains(query)
                        || x.Ean.Contains(query)
                    )
                )
                .OrderBy(x => x.SupplierName)
                .Take(count)
                .ToList();
            }
        }

        public void SetProductCatalogAllegroItemImageReady(int id, bool isImageRady, string comment)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogAllegroItem pcai = ctx.ProductCatalogAllegroItem.Where(x => x.Id == id).FirstOrDefault();
               // pcai.IsImageReady = isImageRady;
                pcai.ProcessId = null;
                pcai.IsValid = false;
                pcai.ValidatedAt = DateTime.Now;
                pcai.Comment = comment;
                ctx.SubmitChanges();
            }
        }

        public void SetSearchTableAttribute(SearchTableAttributes searchAttribute)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.SearchTableAttributes toUpdate = ctx.SearchTableAttributes.Where(x => x.Id == searchAttribute.Id).FirstOrDefault();
                toUpdate.ValueFrom = searchAttribute.ValueFrom;
                toUpdate.ValueString = searchAttribute.ValueString;
                toUpdate.ValueTo = searchAttribute.ValueTo;
                toUpdate.AttributeExists = searchAttribute.AttributeExists;
                ctx.SubmitChanges();
            }
        }
        public void SetSearchTableAttributeAdd(SearchTableAttributes searchAttribute)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.SearchTableAttributes.InsertOnSubmit(searchAttribute);
                ctx.SubmitChanges();
            }
        }

        public void SetSearchTableAttributes(Guid searchId, List<SearchTableAttributes> searchAttributes)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.SearchTable.DeleteAllOnSubmit(ctx.SearchTable.Where(x => x.SearchId == searchId).ToList());
                ctx.SearchTableAttributes.DeleteAllOnSubmit(ctx.SearchTableAttributes.Where(x => x.SearchId == searchId).ToList());

                ctx.SearchTableAttributes.InsertAllOnSubmit(searchAttributes);


                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogFieldsToAllegroParametersResult> GetProductCatalogFieldsToAllegroParameters(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogFieldsToAllegroParameters(productCatalogId).ToList();
            }
        }

        public ProductOrder GetProductOrder(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductOrder.Where(x => x.Id == id).FirstOrDefault();
            }
        }
        public List<ProductOrder> GetProductOrders(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] allowedStatuses = new int[] { (int)Helper.OrderProductStatus.New, (int)Helper.OrderProductStatus.Ordered };
                return ctx.ProductOrder.Where(x => x.ProductCatalogId == productCatalogId && allowedStatuses.Contains(x.OrderProductStatusId)).ToList();
            }
        }

        public List<ProductCatalogType> GetProductTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogType.ToList();
            }
        }

        //Tuple zrobic wersje dla nowej ścieżki
        public void SetProductCatalogAllegroItem(ProductCatalogAllegroItem pcai, int allegroCategoryId, string itemStatus)
        {
            using (LajtitAllegroDB ctxAllegro = new LajtitAllegroDB())
            {
                using (LajtitDB ctx = new LajtitDB())
                {
                    Dal.ProductCatalogAllegroItem pcaiToUpdate = ctx.ProductCatalogAllegroItem.Where(x => x.Id == pcai.Id).FirstOrDefault();

                    Dal.AllegroItem ai = ctxAllegro.AllegroItem.Where(x => x.ItemId == pcai.ItemId).FirstOrDefault();
                        if (ai ==null && allegroCategoryId != 0)
                        {
                             ai = new AllegroItem()
                            {
                                ItemId = pcai.ItemId,
                                InsertDate = DateTime.Now,
                                UserId = pcai.AllegroUserId,
                                ItemStatus = itemStatus,
                                BuyNowPrice = 0,
                                BidCount = 0,
                                CurrentPrice = 0,
                                Name = "",
                                CategoryId = allegroCategoryId,
                                //MovedToArchive = false,
                                SellingMode = "BUY_NOW"
                            };

                            ctxAllegro.AllegroItem.InsertOnSubmit(ai);
                            ctxAllegro.SubmitChanges();
                        }
                        //else
                        //{
                        //    Dal.AllegroItem itemToUpdate = ctxAllegro.AllegroItems.Where(x => x.ItemId == pcai.ItemId.Value).FirstOrDefault();

                        //    itemToUpdate.ItemStatus = itemStatus;
                        //}
                 
                    pcaiToUpdate.ItemId = pcai.ItemId;
                    pcaiToUpdate.Comment = pcai.Comment;
                    pcaiToUpdate.IsValid = pcai.IsValid;
                    pcaiToUpdate.IsValidationError = pcai.IsValidationError;
                    pcaiToUpdate.ValidatedAt = pcai.ValidatedAt;
                    pcaiToUpdate.NotificationSent = pcai.NotificationSent;
                    pcaiToUpdate.ProcessId = pcai.ProcessId; 

                    if (pcaiToUpdate.IsValid.HasValue && pcaiToUpdate.IsValid.Value )
                        pcaiToUpdate.UpdateCommand = null;

                    pcaiToUpdate.UpdateDate = DateTime.Now;

                    ctx.SubmitChanges();

                }
            }
        }

        public SearchTable GetSearchTable(Guid searchId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SearchTable.Where(x => x.SearchId == searchId).FirstOrDefault();
            }
        }

        public List<SearchTableAttributes> GetSearchTableAttributes(Guid searchId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<SearchTableAttributes>(x => x.ProductCatalogAttribute);
                dlo.LoadWith<SearchTableAttributes>(x => x.ProductCatalogAttributeGroup);

                ctx.LoadOptions = dlo;
                return ctx.SearchTableAttributes.Where(x => x.SearchId == searchId).ToList();
            }
        }

        public List<SearchTable> GetSearchTables(string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SearchTable.Where(x => x.InsertUser == userName || x.IsPublic).OrderByDescending(x => x.InsertDate).ToList();
            }
        }

        public void SetSearchTable(SearchTable st)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.SearchTable stToUpdate = ctx.SearchTable.Where(x => x.SearchId == st.SearchId).FirstOrDefault();

                if (stToUpdate == null)
                    ctx.SearchTable.InsertOnSubmit(st);
                else
                {
                    stToUpdate.Title = st.Title;
                    stToUpdate.IsPublic = st.IsPublic;

                }
                ctx.SubmitChanges();

            }
        }

        public void SetProductCatalogAllegroDraftItem(ProductCatalogAllegroItem pcai, int allegroCategoryId)
        {
            using (LajtitAllegroDB ctxAllegro = new LajtitAllegroDB())
            {
                using (LajtitDB ctx = new LajtitDB())
                {
                    if (  allegroCategoryId != 0)
                    {
                        Dal.AllegroItem ai = new AllegroItem()
                        {
                            ItemId = pcai.ItemId,
                            InsertDate = DateTime.Now,
                            UserId = pcai.AllegroUserId,
                            ItemStatus = "INACTIVE",
                            BuyNowPrice = 0,
                            BidCount = 0,
                            CurrentPrice = 0,
                            Name = "",
                            CategoryId = allegroCategoryId,
                            //MovedToArchive = false,
                            SellingMode = "BUY_NOW"
                        };

                        ctxAllegro.AllegroItem.InsertOnSubmit(ai);
                        ctxAllegro.SubmitChanges();

                        ctx.ProductCatalogAllegroItem.InsertOnSubmit(pcai);
                        ctx.SubmitChanges();
                    }


                }
            }
        }

        public List<ProductCatalogShopProduct> GetProductCatalogShopProducts(Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopProduct.Where(x => x.ShopId == (int)shop).ToList();
            }
        }
        public List<ProductCatalogShopProduct> GetProductCatalogShopProducts(int productCatalogId, int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopProduct.Where(x => x.ProductCatalogId == productCatalogId && x.ShopId == shopId).ToList();
            }
        }

            public int[] GetAllegroCategoryWithAttributeGroup(Dal.Helper.ShopType shopType, int attributeGroupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopCategoryField.Where(x => x.ShopTypeId==(int)shopType && x.AttributeGroupId == attributeGroupId).Select(x => x.CategoryId).Distinct().ToArray();
            }
        }

                public void SetProductOrderStatus(int id, int orderProductStatusId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var po= ctx.ProductOrder.Where(x => x.Id == id).FirstOrDefault();

                po.OrderProductStatusId = orderProductStatusId;

                ctx.SubmitChanges();
            }
        }
        public void SetProductCatalogUpdateMarkslojd(int supplierId, Dictionary<string, int> markslojd)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ProductCatalog> products = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId).ToList();

                foreach (Dal.ProductCatalog pc in products)
                {
                    try
                    {
                        if (markslojd.ContainsKey(pc.Code))
                        {
                            pc.IsAvailable = markslojd[pc.Code] > 0 ? true : false;
                            pc.SupplierQuantity = markslojd[pc.Code];
                        }
                        else
                        {
                            pc.IsAvailable = false;
                            pc.SupplierQuantity = null;
                        }
                        pc.UpdateUser = "System";
                        pc.UpdateReason = "Aktualizacja automatyczna";

                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        Dal.ErrorHandler.LogError(ex, String.Format("pcId {0}", pc.ProductCatalogId));
                    }
                }

            }
        }
        public void SetProductCatalogUpdateEglo(int supplierId, Dictionary<string, int> eglo)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ProductCatalog> products = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId).ToList();

                foreach (Dal.ProductCatalog pc in products)
                {
                    try
                    {
                        if (pc.Ean != null && eglo.ContainsKey(pc.Ean))
                        {
                            pc.IsAvailable = eglo[pc.Ean] > 0 ? true : false;
                            pc.SupplierQuantity = eglo[pc.Ean];
                        }
                        else
                        {
                            pc.IsAvailable = false;
                            pc.SupplierQuantity = null;
                        }
                        pc.UpdateUser = "System";
                        pc.UpdateReason = "Aktualizacja automatyczna";

                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        Dal.ErrorHandler.LogError(ex, String.Format("pcId {0}", pc.ProductCatalogId));
                    }
                }

            }
        }

        public void SetProductCatalogUpdateNowodvorski(int supplierId, List<Dal.ProductCatalog> productsFromFile)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ProductCatalog> products = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId && x.Ean!=null).ToList();

                int cnt = 0;
                foreach (Dal.ProductCatalog pc in products)
                {
                    cnt++;
                    Dal.ProductCatalog productFromFile = productsFromFile.Where(x => x.Ean == pc.Ean).FirstOrDefault();


                    if (productFromFile!=null)
                    {
                        pc.IsAvailable = productFromFile.IsAvailable;
                        pc.SupplierQuantity = productFromFile.SupplierQuantity;

                        productFromFile.IsFollowed = true; /// oznaczam sobie że produkt znaleziony, nie bedzie potrzeby go dodawania ponizej
                    }
                    else
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = null;

                    }
                    pc.UpdateUser = "System";
                    pc.UpdateReason = "Aktualizacja automatyczna";
                }

                var productsToAdd = productsFromFile.Where(x => x.IsFollowed.HasValue == false).ToList();
                ctx.ProductCatalog.InsertAllOnSubmit(productsToAdd);

                ctx.SubmitChanges();
            }
        }

        public ProductCatalogGroup GetProductCatalogGroup(int supplierId, string serie)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var g = ctx.ProductCatalogGroup.Where(x => x.SupplierId == supplierId && x.GroupName.ToLower() == serie.ToLower()).FirstOrDefault();

                if (g == null)
                {

                    int lastGroupId = ctx.ProductCatalogGroup.Max(x => x.ProductCatalogGroupId) + 1;

                    ProductCatalogGroup newGroup = new ProductCatalogGroup()
                    {
                        GroupName = serie,
                        SupplierId = supplierId,
                        ProductCatalogGroupId = lastGroupId
                    };
                    ctx.ProductCatalogGroup.InsertOnSubmit(newGroup);
                    ctx.SubmitChanges();
                    return newGroup;
                }
                return g;
            }
        }

        public List<ProductCatalogAttribute> GetProductCatalogAttributesForGroupAndProducts(int attributeGroupId, int[] productCatalogIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeToProduct
                .Where(x => x.ProductCatalogAttribute.AttributeGroupId == attributeGroupId && productCatalogIds.Contains(x.ProductCatalogId))
                .Select(x => x.ProductCatalogAttribute)
                .Distinct()
                .ToList();
            }
        }

        public void SetProductCatalogBySupplierAndCode(int supplierId, string code, bool isAvailable, int? supplierQuantity)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalog pc = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId && x.Code == code).FirstOrDefault();

                if (pc != null)
                {
                    pc.IsAvailable = isAvailable;
                    pc.UpdateUser = "System";
                    pc.UpdateReason = "Aktualizacja automatyczna";
                    pc.SupplierQuantity = supplierQuantity;
                }
                else
                {
                    if (isAvailable)
                    {
                        var pcToAdd = GetProductCatalog(supplierId, code, isAvailable);
                        pcToAdd.SupplierQuantity = supplierQuantity;
                        ctx.ProductCatalog.InsertOnSubmit(pcToAdd);
                    }
                }

                ctx.SubmitChanges();

            }
        }

        public static ProductCatalog GetProductCatalog(int supplierId, string code, bool isAvailable)
        {
            return new ProductCatalog()
            { 
                Name = code, 
                IsAvailable = isAvailable,
                Code = code,
                PriceBruttoFixed = 0, 
                ProductTypeId = 1,
                AutoAssignProduct = true,
                IsDiscontinued = false,
                IsHidden = false,
                IsOnStock = false,              
                SupplierId = supplierId,
                IsActivePricePromo = false 
            };  
    }

        private static int GetDefaultSupplierProductGroupId(int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Supplier.Where(x => x.SupplierId == supplierId).Select(x => x.ProductCatalogGroupId).FirstOrDefault();
            }
            }



        public List<TableLogFollowResult> GetTableLogHistory(Helper.TableName tableName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.TableLogFollow(tableName.ToString())
                    .OrderBy(x=>x.ObjectId)
                    .ThenBy(x=>x.InsertDate)
                    .ToList();
            }
        }
 
        public int[] GetProductCatalogForAttributesAndProducts(int[] attributeIds, int[] productCatalogIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeToProduct
                .Where(x => attributeIds.Contains(x.AttributeId) && productCatalogIds.Contains(x.ProductCatalogId))
                .Select(x => x.ProductCatalog.ProductCatalogId)
                .Distinct()
                .ToArray();
            }
        }

        public void SetProductCatalogImagesConvert(List<ProductCatalogImage> imagesToAdd, int[] imagesToDelete)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                foreach(int imageId in imagesToDelete)
                {
                    ProductCatalog pc = ctx.ProductCatalog.Where(x => x.ImageId == imageId).FirstOrDefault();

                    if (pc != null)
                        pc.ImageId = null;

                }


                ctx.ProductCatalogImage.DeleteAllOnSubmit(ctx.ProductCatalogImage.Where(x=> imagesToDelete.Contains(x.ImageId)));
                ctx.SubmitChanges();
                ctx.ProductCatalogImage.InsertAllOnSubmit(imagesToAdd);
                ctx.SubmitChanges();
            }
        }

        public void SetTableLogHistorySent(Helper.TableName tableName)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.TableLogSent(tableName.ToString());
            }
        }

        //public List<ProductCatalogPromotionResult> GetPromotion(int promotionGroupId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogPromotion(promotionGroupId)  
        //            .ToList();
        //    }
        //}

        public List<ProductCatalog> GetRandomProducts(int v)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalog>(x => x.Supplier);

                ctx.LoadOptions = dlo;
                return ctx.ProductCatalog.Where(x=>x.Ean!=null && x.Ean!="" &&x.SupplierId==9).OrderBy(x=> Guid.NewGuid()).Take(v).ToList();
            }
        }

        //public ProductCatalogPromotionGroup GetPromotionGroup(int promotionGroupId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogPromotionGroups.Where(x => x.PromotionGroupId == promotionGroupId).FirstOrDefault();
        //    }
        //}

        public void SetProductCatalogImageUploaded(ProductCatalogImage image)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogImage imgToUpdate = ctx.ProductCatalogImage.Where(x=>x.ImageId== image.ImageId).FirstOrDefault();
                imgToUpdate.UploadedToServer = true;
                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogToAllegroParametersGetResult> GetProductCatalogToAllegroParameters(int? productCategoryId, long? itemId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogToAllegroParametersGet(productCategoryId.Value, itemId).ToList();
            }
        }

        public void SetProductCatalogUpdateLampex(List<ProductCatalog> products)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == 33 ).ToList();

                foreach (ProductCatalog pcToPdate in productsToUpdate)
                {
                    //ProductCatalog pcToPdate = productsToUpdate.Where(x => x.Ean == pc.Ean).FirstOrDefault();
                    ProductCatalog pc = products.Where(x => x.Ean.Trim() == pcToPdate.Ean.Trim()).FirstOrDefault();

                    
                    if (pc != null)
                    { 
                        pcToPdate.IsAvailable = pc.IsAvailable;
                        pcToPdate.PriceBruttoFixed = Decimal.Round(pc.PriceBruttoFixed,2);
                        pcToPdate.UpdateUser = "System";
                        pcToPdate.UpdateReason = "Aktualizacja automatyczna";
                        pcToPdate.SupplierQuantity = pc.SupplierQuantity;
                       // pcToPdate.Code = pc.Code;
                    }
                    else
                    {
                        pcToPdate.IsAvailable = false;
                        pcToPdate.SupplierQuantity = 0;
                    }
                    try
                    {
                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {

                        Dal.ErrorHandler.LogError(ex, String.Format("ProductCatalogId {0}", pc.ProductCatalogId));
                    }
                }

                string[] allProducts = ctx.ProductCatalog.Where(x => x.SupplierId == 33).Select(x => x.Ean).ToArray();
                var p = products.Where(x => !allProducts.Contains(x.Ean)).ToList();

                string[] codes = p.Select(x => x.Code).ToArray();

                string[] productsExistsing = ctx.ProductCatalog.Where(x =>codes.Contains(x.Code)).Select(x => x.Code).ToArray();

            
                p = p.Where(x => !productsExistsing.Contains(x.Code)).ToList();
                ctx.ProductCatalog.InsertAllOnSubmit(p);


                ctx.SubmitChanges();
            }
        }
        public void SetProductCatalogUpdateZumaline(List<ProductCatalog> products, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId)
                    .Where(x => x.IsDiscontinued == false && x.ProductTypeId != (int)Dal.Helper.ProductType.ComboProduct)
                    .ToList();


                foreach (ProductCatalog pc in products)
                { 
                    ProductCatalog pcToPdate = productsToUpdate.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();

                    if (pcToPdate != null)
                    {
                        pcToPdate.IsAvailable = pc.IsAvailable;
                        pcToPdate.Code2 = pc.Code2;
                        pcToPdate.UpdateUser = "System";
                        pcToPdate.UpdateReason = "Aktualizacja automatyczna";
                        pcToPdate.SupplierQuantity = pc.SupplierQuantity; 
                    }
                }
                ctx.SubmitChanges();
            }
        }

        public void SetProductCatalogUpdateSompex(List<ProductCatalog> products, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                // aktualizuj ean

                List<ProductCatalog> productsWithoutEan = ctx.ProductCatalog.Where(x => x.SupplierId==supplierId&& x.Ean == null).ToList();

                foreach(Dal.ProductCatalog pn in productsWithoutEan)
                {
                    Dal.ProductCatalog pcWithEan = products.Where(x => x.Code == pn.Code && !String.IsNullOrEmpty(x.Ean)).FirstOrDefault();
                    if(pcWithEan!=null)
                    {
                        if(ctx.ProductCatalog.Where(x=>x.Ean==pcWithEan.Ean).FirstOrDefault()==null)
                        { 
                        pn.Ean = pcWithEan.Ean;
                        ctx.SubmitChanges();
                        }
                    }

                }


                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId)
                    //.Where(x => x.IsDiscontinued == false && x.ProductTypeId != (int)Dal.Helper.ProductType.ComboProduct)
                    .ToList();


                foreach (ProductCatalog pc in productsToUpdate)
                {
                    ProductCatalog pcToPdate = products.Where(x => x.Ean == pc.Ean).FirstOrDefault();

                    if (pcToPdate != null)
                    {
                        pc.IsAvailable = pcToPdate.IsAvailable; 
                        pc.UpdateUser = "System";
                        pc.UpdateReason = "Aktualizacja automatyczna";
                        pc.SupplierQuantity = pcToPdate.SupplierQuantity;
                    }
                    else
                    {
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = 0;

                    }
                }
                string[] eanExists = productsToUpdate.Select(x => x.Ean).ToArray();// .Where(x=>!ean.Contains(x.Ean)).Select

                string[] codeExists = productsToUpdate.Select(x => x.Code).ToArray();

                List<ProductCatalog> productsToAdd = products.Where(x => !eanExists.Contains(x.Ean) && !codeExists.Contains(x.Code)).ToList();



                ctx.ProductCatalog.InsertAllOnSubmit(productsToAdd);



                ctx.SubmitChanges();
            }
        }

   

        public void SetProductCatalogUpdateGlasberg(List<ProductCatalog> products, int supplierOwnerId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.Supplier.SupplierOwnerId== supplierOwnerId)
                  
                    .ToList();


                foreach (ProductCatalog pc in products)
                {
                    ProductCatalog pcToPdate = productsToUpdate.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();

                    if (pcToPdate != null)
                    {
                        pcToPdate.IsAvailable = pc.IsAvailable;
                        pcToPdate.UpdateUser = "System";
                        pcToPdate.UpdateReason = "Aktualizacja automatyczna";
                        pcToPdate.SupplierQuantity = pc.SupplierQuantity;
                        if (pc.PriceBruttoFixed != 0)
                            pcToPdate.PriceBruttoFixed = pc.PriceBruttoFixed;
                    }
                }
                ctx.SubmitChanges();
            }
        }
        public void SetProductCatalogUpdateLucide(List<ProductCatalog> products, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId)
                    .Where(x => x.IsDiscontinued == false)
                    .ToList();


                foreach (ProductCatalog pc in products)
                {
                    ProductCatalog pcToPdate = productsToUpdate.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();

                    if (pcToPdate != null)
                    {
                        pcToPdate.IsAvailable = pc.IsAvailable;
                        pcToPdate.UpdateUser = "System";
                        pcToPdate.UpdateReason = "Aktualizacja automatyczna";
                        pcToPdate.SupplierQuantity = pc.SupplierQuantity;
                        if (pc.PriceBruttoFixed != 0)
                            pcToPdate.PriceBruttoFixed = pc.PriceBruttoFixed;
                    }
                }
                ctx.SubmitChanges();
            }
        }
        public void SetProductCatalogUpdateTrioRL(List<ProductCatalog> products, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId)
                    .Where(x => x.IsDiscontinued == false)
                    .ToList();


                foreach (ProductCatalog pc in products)
                {
                    ProductCatalog pcToPdate = productsToUpdate.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();

                    if (pcToPdate != null)
                    {
                        pcToPdate.IsAvailable = pc.IsAvailable;
                        pcToPdate.UpdateUser = "System";
                        pcToPdate.UpdateReason = "Aktualizacja automatyczna";
                        pcToPdate.SupplierQuantity = pc.SupplierQuantity;
                        if(!String.IsNullOrEmpty(pc.Ean))
                        pcToPdate.Ean = pc.Ean;

                    }
                }
                ctx.SubmitChanges();
            }
        }
        public void SetProductCatalogUpdateCandellux(List<ProductCatalog> products, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId)
                    .Where(x => x.IsDiscontinued == false)
                    .ToList();


                foreach (ProductCatalog pc in products)
                {
                    ProductCatalog pcToPdate = productsToUpdate.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();

                    if (pcToPdate != null)
                    {
                        pcToPdate.IsAvailable = pc.IsAvailable;
                        pcToPdate.UpdateUser = "System";
                        pcToPdate.UpdateReason = "Aktualizacja automatyczna";
                        pcToPdate.SupplierQuantity = pc.SupplierQuantity;
                        if (pc.PriceBruttoFixed != 0)
                            pcToPdate.PriceBruttoFixed = pc.PriceBruttoFixed;
                    }
                }
                ctx.SubmitChanges();
            }
        }
        public void SetProductCatalogUpdateItalux(List<ProductCatalog> products, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId)
                  //  .Where(x => x.IsDiscontinued == false)
                    .ToList();


                foreach (ProductCatalog pc in products)
                {
                    ProductCatalog pcToPdate = productsToUpdate.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();

                    if (pcToPdate != null)
                    {
                        pcToPdate.IsAvailable = pc.IsAvailable;
                        pcToPdate.Code2 = pc.Code2;
                        pcToPdate.UpdateUser = "System";
                        pcToPdate.UpdateReason = "Aktualizacja automatyczna";
                        pcToPdate.SupplierQuantity = pc.SupplierQuantity;
                        if (pc.PriceBruttoFixed != 0)
                            pcToPdate.PriceBruttoFixed = pc.PriceBruttoFixed;
                    }

                }
                ctx.SubmitChanges();
            }
        }
        public void SetProductCatalogIsFollowed(int productCatalogId, bool isFollowed)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalog pc = ctx.ProductCatalog.Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();
                pc.IsFollowed = isFollowed;

                ctx.SubmitChanges();
            }
        }

        public void SetProductCatalogUpdateStatusPrice(List<ProductCatalog> products, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId).Where(x => x.IsDiscontinued == false).ToList();


                foreach (ProductCatalog pc in products)
                {
                    ProductCatalog pcToPdate = productsToUpdate.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();

                    if (pcToPdate != null)
                    {
                        pcToPdate.IsAvailable = pc.IsAvailable;
                        pcToPdate.PriceBruttoFixed = pc.PriceBruttoFixed;
                        pcToPdate.PurchasePrice = pc.PurchasePrice;
                        pcToPdate.SupplierQuantity = pc.SupplierQuantity;
                        pcToPdate.UpdateUser = "System";
                        pcToPdate.UpdateReason = "Aktualizacja automatyczna";
                    }

                }
                ctx.SubmitChanges();
            }
        }

        public ProductCatalogSourceDeliveryFnResult GetProductCatalogSourceDelivery(int productCatalogId, int sourceTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogSourceDeliveryFn(productCatalogId, sourceTypeId).FirstOrDefault();
            }
        }

        //public void SetProductCatalogAllegroItemPromotionDelete(int supplierId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ctx.ProductCatalogItemDiscountsDelete(supplierId);
        //    }
        //}

        public List<Dal.ProductCatalog> SetProductCatalogs(List<ProductCatalog> productsToAdd, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                string[] codesToAdd = productsToAdd.Select(x => x.Code.ToLower()).Distinct().ToArray();
                string[] existingCodes = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId && codesToAdd.Contains(x.Code.ToLower())).Select(x => x.Code.ToLower()).ToArray();
                List<Dal.ProductCatalog> toAdd = productsToAdd.Where(x => !existingCodes.Contains(x.Code.ToLower())).Distinct().ToList();
                ctx.ProductCatalog.InsertAllOnSubmit(toAdd);
                ctx.SubmitChanges();

                return toAdd;
            }
        }
        public List<Dal.ProductCatalog> SetProductCatalogsBySupplierOwner(List<ProductCatalog> productsToAdd, int supplierOwnerId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                string[] codesToAdd = productsToAdd.Select(x => x.Ean.ToLower()).Distinct().ToArray();
                string[] existingCodes = ctx.ProductCatalog.Where(x => x.Supplier.SupplierOwnerId == supplierOwnerId && codesToAdd.Contains(x.Ean.ToLower())).Select(x => x.Ean.ToLower()).ToArray();
                List<Dal.ProductCatalog> toAdd = productsToAdd.Where(x => !existingCodes.Contains(x.Ean.ToLower())).Distinct().ToList();
                ctx.ProductCatalog.InsertAllOnSubmit(toAdd);
                ctx.SubmitChanges();

                return toAdd;
            }
        }
        public List<Dal.ProductCatalog> SetProductCatalogsByEan(List<ProductCatalog> productsToAdd, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                string[] codesToAdd = productsToAdd.Select(x => x.Ean).Distinct().ToArray();
                string[] existingCodes = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId && codesToAdd.Contains(x.Ean)).Select(x => x.Ean).ToArray();
                List<Dal.ProductCatalog> toAdd = productsToAdd.Where(x => !existingCodes.Contains(x.Ean)).Distinct().ToList();

                foreach (Dal.ProductCatalog pc in toAdd)
                {
                    try
                    {
                        ctx.ProductCatalog.InsertOnSubmit(pc);
                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        Dal.ErrorHandler.LogError(ex, String.Format("Ean {0}", pc.Ean));
                    }
                }
                return toAdd;
            }
        }
        public void SetProductCatalogUpdateMaxlight2(List<ProductCatalog> products)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == 2).ToList();

                foreach(ProductCatalog pc in products)
                {
                    try
                    {
                        ProductCatalog pcToPdate = productsToUpdate.Where(x => x.Code == pc.Code).FirstOrDefault();

                        if (pcToPdate != null)
                        {
                            pcToPdate.ExternalId = pc.ExternalId;
                            pcToPdate.IsAvailable = pc.IsAvailable;
                            pcToPdate.Specification = pc.Specification;
                            if (!String.IsNullOrEmpty(pc.Ean))
                                pcToPdate.Ean = pc.Ean;

                        }
                        else
                        {
                            ctx.ProductCatalog.InsertOnSubmit(pc);

                        }
                        ctx.SubmitChanges();

                    }
                    catch (Exception ex)
                    {
                        Dal.ErrorHandler.LogError(ex, String.Format("Code: {0}", pc.Code));
                    }
                }
            }
        }

       

        public void SetProductCatalogUpdateByCeneoFile(List<ProductCatalog> products, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId).ToList();



                // czasami zmieniaja kody wiec prostujemy kody na podstawie ean.


                foreach(Dal.ProductCatalog pc in productsToUpdate.Where(x=>x.Ean !=null).ToList())
                {
                    var p = products.Where(x => x.Ean == pc.Ean).FirstOrDefault();
                    if (p != null && p.Code != pc.Code)
                        pc.Code = p.Code;

                }
                ctx.SubmitChanges();

                productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId).ToList();

                foreach (ProductCatalog pcToPdate  in productsToUpdate)
                {
                    try
                    {
                        ProductCatalog pc = products.Where(x => x.Code == pcToPdate.Code).FirstOrDefault();

                        if (pc != null)
                        {
                            pcToPdate.ExternalId = pc.ExternalId;
                            pcToPdate.IsAvailable = pc.IsAvailable;
                            pcToPdate.Specification = pc.Specification;
                            pcToPdate.UpdateUser = "System";
                            pcToPdate.UpdateReason = "Aktualizacja automatyczna";
                            if (!String.IsNullOrEmpty(pc.Ean))
                                pcToPdate.Ean = pc.Ean;

                            pcToPdate.SupplierQuantity = pc.SupplierQuantity;
                            //if (pcToPdate.PriceBruttoFixed - 1 > pc.PriceBruttoFixed)
                            //{
                            //    pcToPdate.PriceBruttoPromo = (decimal)Math.Round((double)pc.PriceBruttoFixed * 0.98,0);

                            //    if (!pcToPdate.PriceBruttoPromoDate.HasValue)
                            //        pcToPdate.PriceBruttoPromoDate = DateTime.Now.AddMonths(12);
                            //    else
                            //    {
                            //        if (pcToPdate.PriceBruttoPromoDate.HasValue && pcToPdate.PriceBruttoPromoDate.Value < DateTime.Now.AddMonths(6))
                            //            pcToPdate.PriceBruttoPromoDate = DateTime.Now.AddMonths(12);
                            //    }
                            //}
                            //else
                            //{
                            //    pcToPdate.PriceBruttoPromo = null;
                            //    pcToPdate.PriceBruttoPromoDate = null;
                            //}
                            if(pc.IsAvailable)
                            {
                                pcToPdate.IsDiscontinued = false;
                            }
                        }
                        else
                        {
                            //ctx.ProductCatalog.InsertOnSubmit(pc);
                            pcToPdate.IsAvailable = false;
                            pcToPdate.SupplierQuantity = 0;

                        }
                        ctx.SubmitChanges();

                    }
                    catch (Exception ex)
                    {
                        Dal.ErrorHandler.LogError(ex, String.Format("Code: {0}", pcToPdate.Code));
                    }
                }
                productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId).ToList();
                string[] codes = productsToUpdate.Select(x => x.Code.ToLower()).ToArray();
                List<ProductCatalog> productsToAdd = products.Where(x => !codes.Contains(x.Code.ToLower())).ToList();

                ctx.ProductCatalog.InsertAllOnSubmit(productsToAdd);
                ctx.SubmitChanges();


            }
        }

      

        public ProductCatalogAllegroItem GetProductCatalogAllegroItem(long itemId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();
            }
        }

        public void SetProductCatalogAuhilonUpdateByCeneoFile(List<ProductCatalog> products, int supplierId)
        {
            List<string> existingIds = new List<string>();

            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalog> productsToUpdate = ctx.ProductCatalog.Where(x => x.SupplierId == supplierId).ToList();
 
                foreach (ProductCatalog pcToPdate in productsToUpdate.Where(x=>x.Ean!=null).ToList())
                {
                    try
                    {
                        ProductCatalog pc = products.Where(x => x.Ean  == pcToPdate.Ean ).FirstOrDefault();

                        if (pc != null)
                        {
                            pcToPdate.IsAvailable = pc.IsAvailable;
                            pcToPdate.UpdateUser = "System";
                            pcToPdate.UpdateReason = "Aktualizacja automatyczna";
                            //if (!String.IsNullOrEmpty(pc.Ean))
                            //    pcToPdate.Ean = pc.Ean;
                            pcToPdate.PriceBruttoFixed = pc.PriceBruttoFixed;
                            pcToPdate.SupplierQuantity = pc.SupplierQuantity;

                            existingIds.Add(pc.Code.ToLower());
                        }
                        else
                        { 
                            pcToPdate.IsAvailable = false;
                            pcToPdate.SupplierQuantity = 0;
                        }

                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        Dal.ErrorHandler.LogError(ex, String.Format("Code: {0}", pcToPdate.Code));
                    }
                }
                foreach (ProductCatalog pcToPdate in productsToUpdate.Where(x => x.Ean == null).ToList())
                {
                    try
                    {
                        ProductCatalog pc = products.Where(x => x.Code.ToLower() == pcToPdate.Code.ToLower()).FirstOrDefault();

                        if (pc != null)
                        {
                            pcToPdate.IsAvailable = pc.IsAvailable;
                            pcToPdate.UpdateUser = "System";
                            pcToPdate.UpdateReason = "Aktualizacja automatyczna";
                            if (!String.IsNullOrEmpty(pc.Ean))
                                pcToPdate.Ean = pc.Ean;
                            pcToPdate.PriceBruttoFixed = pc.PriceBruttoFixed;
                            pcToPdate.SupplierQuantity = pc.SupplierQuantity;

                            existingIds.Add(pc.Code.ToLower());
                        }
                        else
                        {
                            pcToPdate.IsAvailable = false;
                            pcToPdate.SupplierQuantity = 0;
                        }

                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        Dal.ErrorHandler.LogError(ex, String.Format("Code: {0}", pcToPdate.Code));
                    }
                }
                List<ProductCatalog> productsToAdd = products.Where(x => !existingIds.Contains(x.Code.ToLower())).ToList();

                foreach(Dal.ProductCatalog pcToAdd in productsToAdd)
                {
                    if(ctx.ProductCatalog.Where(x=>
                    (x.Ean == null && x.Code.ToLower() == pcToAdd.Code.ToLower())
                    ||
                    (x.Ean == pcToAdd.Ean)
                        ).FirstOrDefault() == null)
                    {

                        try
                        {
                            ctx.ProductCatalog.InsertOnSubmit(pcToAdd);
                            ctx.SubmitChanges();
                        }
                        catch (Exception ex)
                        {

                            Dal.ErrorHandler.LogError(ex, String.Format("Code: {0}", pcToAdd.Code));

                        }
                    }


                }



            }
        }

  
 

        public List<SupplierDeliveryType> GetDeliveries()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierDeliveryType.ToList();
            }
            }

        public void SetProductCatalogImageMissing(int imageId, bool exists)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogImage imageToUpdate = ctx.ProductCatalogImage.Where(x => x.ImageId == imageId).FirstOrDefault();
                imageToUpdate.FileExists = exists;
                ctx.SubmitChanges();
            }
        }

        public void SetProductCatalogUpdateScheduleProcessId(int[] ids, Guid processId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
            
                    List<ProductCatalogShopUpdateSchedule> schedules = ctx.ProductCatalogShopUpdateSchedule
                    .Where(x => ids.Contains(x.Id)).ToList();

                    foreach (ProductCatalogShopUpdateSchedule schedule in schedules)
                        schedule.ProcessId = processId;

                    ctx.SubmitChanges();
                 
            }
        }

        public int GetProductCatalogUpdateScheduleProcessIsNullCounter()
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                List<ProductCatalogShopUpdateSchedule> schedules = ctx.ProductCatalogShopUpdateSchedule
                .Where(x => x.ProcessId == null).ToList();

                return schedules.Count();
            }
        }

        //public void SetPromotion(int promotionGroupId, List<ProductCatalogPromotionSource> sources)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        List<Dal.ProductCatalogPromotionSource> promotionSourcesToDelete =
        //            ctx.ProductCatalogPromotionSources.Where(x => x.ProductCatalogPromotionGroup.PromotionGroupId == promotionGroupId)
        //            .ToList();
        //        ctx.ProductCatalogPromotionSources.DeleteAllOnSubmit(promotionSourcesToDelete);
        //        ctx.SubmitChanges();
        //        ctx.ProductCatalogPromotionSources.InsertAllOnSubmit(sources);
        //        ctx.SubmitChanges();
        //    }
        //}

        public List<ProductCatalogImage> GetProductCatalogImage(string image)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogImage.Where(x => x.FileName.ToLower() == image.ToLower()).ToList();
            }
        }

        public void SetProductCatalogAllegroItemDelete(long itemId)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                AllegroItem ai= ctx.AllegroItem.Where(x => x.ItemId == itemId && x.ItemStatus == "INACTIVE").FirstOrDefault();
                if (ai != null)
                {
                    ai.ItemStatus = "ENDED";
                    ctx.SubmitChanges();
                }

            }
        }

        public ProductCatalogImage GetProductCatalogImage(int imageId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogImage.Where(x => x.ImageId == imageId).FirstOrDefault();
            }
        }
        //public int SetPromotion(ProductCatalogPromotionGroup pg)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        if(pg.PromotionGroupId==0)
        //        ctx.ProductCatalogPromotionGroups.InsertOnSubmit(pg);
        //        else
        //        {
        //            Dal.ProductCatalogPromotionGroup pgToUpdate = ctx.ProductCatalogPromotionGroups.Where(x => x.PromotionGroupId == pg.PromotionGroupId).FirstOrDefault();

        //            pgToUpdate.EndDate = pg.EndDate;
        //            pgToUpdate.IsActive = pg.IsActive;
        //            pgToUpdate.Name = pg.Name;
        //            pgToUpdate.StartDate = pg.StartDate;
        //        }
        //        ctx.SubmitChanges();
        //        return pg.PromotionGroupId;
        //    }
        //}

        //public List<ProductCatalogPromotionGroup> GetPromotions()
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogPromotionGroups.ToList();
        //    }
        //}

        public List<ProductCatalog> GetProductCatalogBySupplier(int[] supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                //dlo.LoadWith<ProductCatalog>(x => x.ProductCatalogGroup);
                dlo.LoadWith<ProductCatalog>(x => x.Supplier);

                ctx.LoadOptions = dlo;
                return ctx.ProductCatalog.Where(x => supplierId.Contains( x.SupplierId )).ToList();
            }
        }
         

        public List<ProductCatalogWarehouse> GetWarehouse()
        {
            using (LajtitDB ctx = new LajtitDB())
            {  
                return ctx.ProductCatalogWarehouse.Where(x => x.IsActive).ToList();
            }
        }

        public void SetProductCatalogSettings(int[] productIds, Dal.ProductCatalog pc, bool changeLockRebates, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach (int productCatalogId in productIds)
                {
                    ProductCatalog pcToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();
                    if (pc.IsOutlet.HasValue)
                        pcToUpdate.IsOutlet = pc.IsOutlet.Value;
                    if (pc.IsPaczkomatAvailable.HasValue)
                        pcToUpdate.IsPaczkomatAvailable = pc.IsPaczkomatAvailable.Value;
                    if (changeLockRebates)
                        pcToUpdate.LockRebates = pc.LockRebates;

                    if (pc.PriceBruttoPromo.HasValue)
                    {
                        if (pc.PriceBruttoPromo.Value == 0)
                        {
                            pcToUpdate.PriceBruttoPromo = null;
                            pcToUpdate.PriceBruttoPromoDate = null;
                        }
                        else
                        {
                            if (pc.PriceBruttoPromoDate.HasValue)
                            {
                                pcToUpdate.PriceBruttoPromo = (1 - pc.PriceBruttoPromo.Value /* tu kryje się zmiana procentowa */) * pcToUpdate.PriceBruttoFixed;
                                pcToUpdate.PriceBruttoPromoDate = pc.PriceBruttoPromoDate;
                            }
                        }
                    } 
                    pcToUpdate.UpdateUser = userName;
                    pcToUpdate.UpdateReason = "Zmiana ustawień";
                    ctx.SubmitChanges();
                }
            }
        }

        public int GetProductCatalogDeliveryForOrder(int productCatalogId, int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogDelivery.Where(x => x.ProductCatalogId == productCatalogId && x.OrderId == orderId)
         .AsEnumerable()
         .Sum(x => x.Quantity);
            }
        }
  
 

        public void SetProductCatalogAltavolaAddNew(List<ProductCatalog> productsFromAltavolaNotInDb)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach(ProductCatalog pcToAdd in productsFromAltavolaNotInDb)
                {
                    if (ctx.ProductCatalog
                        .Where(x => x.SupplierId == pcToAdd.SupplierId && x.Code == pcToAdd.Code.ToLower().TrimEnd().TrimStart())
                        .FirstOrDefault() == null)
                    {
                        try
                        {
                            pcToAdd.Name = pcToAdd.Name.Substring(0, Math.Min(100, pcToAdd.Name.Length));
                            ctx.ProductCatalog.InsertOnSubmit(pcToAdd);
                            ctx.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.LogError(ex, String.Format("Błąd dodawania produktu {0}", pcToAdd.Code));

                        }
                    }
                }
            }
        }

        public void SetProductCatalogAllegroItemPublishCompleted(Guid cmdId)
        {
            using(LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogAllegroItemPublishCompleted(cmdId);
                ctx.SubmitChanges();
            }
        }

        public void SetProductCatalogAltavolaUpdate(int productCatalogId, ProductCatalog pc)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalog pcToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId== productCatalogId).FirstOrDefault();

                try

                {
                    //if (pcToUpdate == null)
                    //{
                    //    ctx.ProductCatalog.InsertOnSubmit(pc);
                    //    ctx.SubmitChanges(); 
                    //}
                    //else
                    {

                        //pcToUpdate.AllegroName = pc.AllegroName;
                        //pcToUpdate.Name = pc.Name;
                        //pcToUpdate.Description = pc.Description;
                        //pcToUpdate.AutoAssignProduct = pc.AutoAssignProduct;
                        pcToUpdate.Code = pc.Code;
                        pcToUpdate.ExternalId = pc.ExternalId;
                        pcToUpdate.Ean = pc.Ean;
                        pcToUpdate.IsAvailable = pc.IsAvailable;
                        //pcToUpdate.IsHidden = pc.IsHidden;
                        //pcToUpdate.IsAvailableAllegro = pc.IsAvailableAllegro;
                        //pcToUpdate.IsAvailableOnline = pc.IsAvailableOnline;
                        //pcToUpdate.IsDiscontinued = pc.IsDiscontinued;
                        pcToUpdate.PriceBruttoFixed = pc.PriceBruttoFixed;
                        //pcToUpdate.ProductTypeId = pc.ProductTypeId;
                        //pcToUpdate.ShortDescription = pc.ShortDescription;
                        //pcToUpdate.SupplierId = pc.SupplierId;
                        //pcToUpdate.ProductCatalogGroupId = pc.ProductCatalogGroupId == 1 ? pcToUpdate.ProductCatalogGroupId : pc.ProductCatalogGroupId;


                        if (pc.PriceBruttoPromo != null)
                        {
                            pcToUpdate.IsActivePricePromo = pc.IsActivePricePromo;
                            pcToUpdate.PriceBruttoPromo = pc.PriceBruttoPromo;
                            pcToUpdate.PriceBruttoPromoDate = pc.PriceBruttoPromoDate;
                        }

                        pcToUpdate.DeliveryId = pc.DeliveryId;
                        //pcToUpdate.PurchasePrice = pc.PurchasePrice;
                        pcToUpdate.UpdateUser = "System";
                        pcToUpdate.UpdateReason = "Aktualizacja automatyczna";
                        pcToUpdate.SupplierQuantity = pc.SupplierQuantity;
                        ctx.SubmitChanges();
                    }
                }
                catch (Exception ex)
                {
                    Dal.ErrorHandler.LogError(ex, String.Format("Błąd aktualizacji PcId: {0}, Code: {1}, Ean: {2}", pcToUpdate.ProductCatalogId, pc.Code, pc.Ean));
                }
            }
        }

        public void SetProductCatalogRedluxUpdate(int productCatalogId, ProductCatalog pc)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalog pcToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();

                try 
                { 
                       // pcToUpdate.Code = pc.Code;
                        pcToUpdate.ExternalId = pc.ExternalId;
                        pcToUpdate.Ean = pc.Ean;
                        pcToUpdate.IsAvailable = pc.IsAvailable; 
                        pcToUpdate.UpdateUser = "System";
                        pcToUpdate.UpdateReason = "Aktualizacja automatyczna";
                        pcToUpdate.SupplierQuantity = pc.SupplierQuantity;
                        ctx.SubmitChanges();
             
                }
                catch (Exception ex)
                {
                    Dal.ErrorHandler.LogError(ex, String.Format("Błąd aktualizacji PcId: {0}, Code: {1}, Ean: {2}", pcToUpdate.ProductCatalogId, pc.Code, pc.Ean));
                }
            }
        }
        public List<Dal.ProductCatalogShopProduct> GetProductCatalogInOnlineShop(Dal.Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogShopProduct.Where(x => x.ShopId == (int)shop && x.ShopProductId != null).ToList(); 
            }
        }
 
         
        public List<Dal.ProductCatalogAllegroItem> GetProductCatalogAllegroItems(long[] itemIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAllegroItem.Where(x => itemIds.Contains(x.ItemId)).ToList();
            }
        }

        public List<ProductCatalogAttribute> GetProductCatalogAttributes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAttribute>(x => x.ProductCatalogAttributeGroup);

                ctx.LoadOptions = dlo;

                return ctx.ProductCatalogAttribute.ToList();
           
            }
        }
         
        public List<ProductCatalogImage> GetProductCatalogImages()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogImage.ToList();
            }
        }

        public ProductCatalogAttributeGroup GetProductCatalogAttributeGroup(string groupCode)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeGroup.Where(x => x.GroupCode == groupCode).FirstOrDefault();
            }
        }

        public void SetProductCatalogUpdateStock()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogStockUpdate();
            }
        }
  

        //public void SetProductCatalogAllegroItemPromotionsDelete(ProductCatalogAllegroItemDiscountsDeleteView item)
        //{
        //    throw new NotImplementedException();
        //    //using (LajtitDB ctx = new LajtitDB())
        //    //{
        //    //    ProductCatalogAllegroItem itemToUpdate = ctx.ProductCatalogAllegroItem
        //    //        .Where(x => x.Id == item.Id)
        //    //        .FirstOrDefault();

        //    //    itemToUpdate.AllegroDiscountDelete = null;
        //    //    itemToUpdate.AllegroDiscountId = null;
        //    //    ctx.SubmitChanges();

        //    //}
        //}

        //public List<ProductCatalogAllegroItemDiscountsDeleteView> GetProductCatalogAllegroItemPromotionsToDelete()
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogAllegroItemDiscountsDeleteView.ToList();

        //    }
        //}

        public int SetProductCatalogImport(ProductCatalog pc)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                ctx.ProductCatalog.InsertOnSubmit(pc);
                ctx.SubmitChanges();
                return pc.ProductCatalogId;
            }
        }

        
        public void UpdateProductCatalogImage(ProductCatalogImage image)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogImage imageToUpdate = ctx.ProductCatalogImage.Where(x => x.ImageId == image.ImageId).FirstOrDefault();

                imageToUpdate.Width = image.Width;
                imageToUpdate.Height = image.Height;
                imageToUpdate.Size = image.Size;
                ctx.SubmitChanges();

            }
        }

        public List<ProductCatalogAllegroItemsActive> GetProductCatalogAllegroItemsActive()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogAllegroItemsActive.ToList();//.OrderByDescending(x => x.EndingDateTime).ToList();
            }
        }
        public List<ProductCatalogAllegroItemsActive> GetProductCatalogAllegroItemsActiveForDiscounts()
        {
            //using (LajtitDB ctx = new LajtitDB())
            //{
            //    return ctx.ProductCatalogAllegroItemsActive
            //        .Where(x =>
            //        x.AllegroDiscountId == null
            //        && x.AllegroDiscountValue != null
            //        ).ToList();
            //}
            throw new NotImplementedException();
        }

        //public void SetProductCatalogItemsToUpdate(int[] ids, Dal.Helper.AllegroItemUpdateStatus updateStatus, 
        //    string comment,
        //    string updateCommand)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {

        //        int count = ids.Length;
        //        List<ProductCatalogAllegroItem> t = new List<ProductCatalogAllegroItem>();

        //        int partsCount = (count / 2000);

        //        int[] itemsPart;
        //        for (int i = 0; i < partsCount + 1; i++)
        //        {
        //            itemsPart = ids.Skip(i * 2000).Take(2000).ToArray();
        //            t.AddRange(ctx.ProductCatalogAllegroItem.Where(x => itemsPart.Contains(x.Id)).ToList()

        //                );

        //        }



        //        try
        //        {
        //            foreach (Dal.ProductCatalogAllegroItem item in t)
        //            {
        //                item.UpdateStatus = (int)updateStatus;
        //                item.UpdateComment = comment;
        //                item.UpdateCommand = updateCommand;
        //                ctx.SubmitChanges();
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            string s = ex.Message;

        //        }
        //    }
        //}

        public void SetProductCatalogAllegroItemPromotionId(int id, string promotionId)
        {
            throw new NotImplementedException();
            //using (LajtitDB ctx = new LajtitDB())
            //{
            //    ProductCatalogAllegroItem item = ctx.ProductCatalogAllegroItem.Where(x => x.Id == id).FirstOrDefault();
            //    item.AllegroDiscountId = promotionId;

            //    ctx.SubmitChanges();
            //}
        }

        //public string[] ProductCatalogUpdateSchedules(int shopId, int productCatalogId, Dal.Helper.UpdateScheduleType scheduleType)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogShopUpdateSchedule
        //            .Where(x => x.ProductCatalogId == productCatalogId
        //            && x.ShopId == shopId
        //            && x.UpdateStatusId == (int)Dal.Helper.ProductCatalogUpdateStatus.New
        //            && x.ScheduleTypeId == (int)scheduleType)
        //            .Select(x => x.UpdateCommand)
        //            .Distinct()
        //            .ToArray();
        //    }
        //}

        //public List<ProductCatalogUpdateScheduleDuplcates> GetProductCatalogUpdateScheduleDuplicates()
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogUpdateScheduleDuplcates.ToList(); ;
        //    }
        //}

        //public void SetProductCatalogScheduleAndDeleteDuplicates(List<ProductCatalogUpdateScheduleDuplcates> duplicates, 
        //    ProductCatalogUpdateSchedule schedule,
        //    int shopId, 
        //    int productCatalogId, 
        //    int scheduleTypeId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        List<Dal.ProductCatalogUpdateSchedule> schedules = ctx.ProductCatalogUpdateSchedule
        //            .Where(x => x.ProductCatalogId == productCatalogId
        //            && x.ShopId == shopId
        //            && x.UpdateStatusId == (int)Dal.Helper.ProductCatalogUpdateStatus.New
        //            && x.ScheduleTypeId == scheduleTypeId)
        //            .ToList();

        //        foreach (ProductCatalogUpdateSchedule s in schedules)
        //        {
        //            s.UpdateStatusId = (int)Dal.Helper.ProductCatalogUpdateStatus.Deleted;
        //            s.UpdateComment = "usunięte w ramach merge";
        //            s.UpdateDate = DateTime.Now;
        //        }
        //        if (schedule != null)
        //            ctx.ProductCatalogUpdateSchedule.InsertOnSubmit(schedule);
        //        ctx.SubmitChanges();
        //    }
        //}
 

    
        
        public void SetProductCatalogUpdate(ProductCatalogFileDataFnResult product, ProductCatalog productValues, string[] fields, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalog productToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId == product.ProductCatalogId.Value).FirstOrDefault();


                productToUpdate.UpdateUser = "System";
                productToUpdate.UpdateReason = String.Format("Plik aktualizujący nr: {0}, autor: {1}, komentarz: {2} ", product.ProductCatalogFileId, product.FileInsertUser, product.FileComment);

                if (fields.Contains("CenaSprzedazyBrutto") && product.CenaSprzedazyBrutto != null)
                    productToUpdate.PriceBruttoFixed = productValues.PriceBruttoFixed;

                if (fields.Contains("CenaPromocyjnaSprzedazyBrutto") && product.CenaPromocyjnaSprzedazyBrutto != null)
                    productToUpdate.PriceBruttoPromo = productValues.PriceBruttoPromo;

                if (fields.Contains("PromocjaKoniecData") && product.PromocjaKoniecData != null)
                    productToUpdate.PriceBruttoPromoDate = productValues.PriceBruttoPromoDate;


                if (fields.Contains("CenaZakupuNetto") && product.CenaZakupuNetto != null)
                    productToUpdate.PurchasePrice = productValues.PurchasePrice;

                if (fields.Contains("Linia") && product.Linia != null)
                {
                    Dal.ProductCatalogGroupHelper pc = new ProductCatalogGroupHelper();
                    pc.SetProductCatalogGroupProduct(product.Linia, product.ProductCatalogId.Value, supplierId);
                }
                if (fields.Contains("Kod") && product.Kod != null)
                    productToUpdate.Code = productValues.Code;
                if (fields.Contains("Ean") && product.Ean != null)
                    productToUpdate.Ean = productValues.Ean;
                if (fields.Contains("IdZewnetrzne") && product.IdZewnetrzne != null)
                    productToUpdate.ExternalId = productValues.ExternalId;
                if (fields.Contains("Opis") && product.Opis != null)
                    productToUpdate.Specification = productValues.Specification;
                string status = product.Status ?? "";
                if (fields.Contains("Status") && !String.IsNullOrEmpty(status.Trim()))
                {
                    productToUpdate.IsAvailable = productValues.IsAvailable;

                    if (product.Status == "-1")
                        productToUpdate.IsDiscontinued = productValues.IsDiscontinued;
                    //else
                    //if (product.Status == "0" || product.Status == "1")
                    //    productToUpdate.IsDiscontinued = false;
                }

                if (fields.Contains("Nazwa") && productValues.Name !=null)
                {
                    productToUpdate.Name = productValues.Name; 
                }

                if (    fields.Contains("PaczkaDlugosc") && !String.IsNullOrEmpty(product.PaczkaDlugosc)
                    &&  fields.Contains("PaczkaWysokosc") && !String.IsNullOrEmpty(product.PaczkaWysokosc)
                    &&  fields.Contains("PaczkaSzerokosc") && !String.IsNullOrEmpty(product.PaczkaSzerokosc))
                {
                    try
                    {
                        string[] width = product.PaczkaSzerokosc.Split(new char[] { '/' });
                        string[] length = product.PaczkaDlugosc.Split(new char[] { '/' });
                        string[] height = product.PaczkaWysokosc.Split(new char[] { '/' });


                        if (width.Length == length.Length && length.Length == height.Length)
                        {
                            List<Dal.ProductCatalogPacking> packings = new List<ProductCatalogPacking>();
                            for (int i = 0; i < width.Length; i++)
                            {
                                Dal.ProductCatalogPacking p = new ProductCatalogPacking()
                                {
                                    Height = Decimal.Parse(height[i]),
                                    Width = Decimal.Parse(width[i]),
                                    Length = Decimal.Parse(length[i])
                                };
                                if (i == 0 && !String.IsNullOrEmpty(product.Waga))
                                {
                                    p.Weight = Decimal.Parse(product.Waga);
                                }
                                if (product.ProductCatalogId.HasValue)
                                    p.ProductCatalogId = product.ProductCatalogId.Value;

                                packings.Add(p);
                            }

                            if (packings.Count() > 0 && product.ProductCatalogId.HasValue)
                            {
                                List<Dal.ProductCatalogPacking> toDelete = ctx.ProductCatalogPacking.Where(x => x.ProductCatalogId == product.ProductCatalogId.Value).ToList();
                                ctx.ProductCatalogPacking.DeleteAllOnSubmit(toDelete);
                                ctx.ProductCatalogPacking.InsertAllOnSubmit(packings);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }


                ctx.SubmitChanges();
            }
        }

        public ProductCatalogAttribute GetProductCatalogAttributeGroup(int productCatalogId, string groupCode)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                //DataLoadOptions dlo = new DataLoadOptions();
                //dlo.LoadWith<product>(x => x.ProductCatalogGroup);
                //dlo.LoadWith<ProductCatalog>(x => x.Supplier);

                //ctx.LoadOptions = dlo;

                return ctx.ProductCatalogAttributeToProduct.Where(x => x.ProductCatalogId == productCatalogId && x.ProductCatalogAttribute.ProductCatalogAttributeGroup.GroupCode == groupCode && x.IsDefault == true)
                    .Select(x => x.ProductCatalogAttribute)
                    .FirstOrDefault();

            }
        }
        public ProductCatalogAttribute GetProductCatalogAttribute(int productCatalogId, string code)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                //int? groupDefaultAttributeId = ctx.ProductCatalogAttributeToProduct.Where(x => x.GroupCode == groupCode).Select(x=>x.DefaultAttributeId).FirstOrDefault();
                //if (!groupDefaultAttributeId.HasValue)
                //    return null;

                return ctx.ProductCatalogAttributeToProduct.Where(x => x.ProductCatalogId == productCatalogId && x.ProductCatalogAttribute.Code == code)
                    .Select(x => x.ProductCatalogAttribute)
                    .FirstOrDefault();

            }
        }
        public ProductCatalogAttributeToProduct GetProductCatalogAttributeValue(int productCatalogId, string code)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAttributeToProduct>(x => x.ProductCatalogAttribute); 

                ctx.LoadOptions = dlo;

                return ctx.ProductCatalogAttributeToProduct.Where(x => x.ProductCatalogId == productCatalogId && x.ProductCatalogAttribute.Code == code)
                    .FirstOrDefault();

            }
        }

        public List<ProductCatalogAttributeGroup> GetAttibuteGroupsFromAttributes(int[] attributeIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttribute
                    .Where(x => attributeIds.Contains(x.AttributeId))
                    .Select(x=>x.ProductCatalogAttributeGroup)
                    .Distinct()
                    .ToList();
            }
        }
   
        //public List<ProductCatalogAllegroItemsFnResult> GetAllegroItemsToUpdate(int count )
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogAllegroItemsFn(count, "ACTIVE", null, true, null)
        //            //.Where(x => x.IsValid == false)
        //           // .Where(x=>x.ItemId== 8962699045)
        //           // .Take(count)
        //           .OrderBy(x => x.ValidatedAt)
        //            .ToList();
        //    }
        //}

        //public List<ProductCatalogAllegroItemsFnResult> GetAllegroItemsToUpdate(int limit, Guid? processId, bool? isImageReady)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogAllegroItemsFn(limit, "ACTIVE", null, isImageReady, processId)
        //            //.Where(x => x.UpdateCommand != null && x.IsImageReady && x.ProcessId == processId)
        //            .ToList();
        //    }
        //}
       

        //public List<ProductCatalogAllegroItemsFnResult> GetAllegroItemsToUpdate(int limit, string itemStatus, int? productCatalogId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        //    return ctx.ProductCatalogAllegroItemsActive.Where(x => x.UpdateStatus == (int)Helper.AllegroItemUpdateStatus.Verifying).ToList();
        //        return ctx.ProductCatalogAllegroItemsFn(limit, itemStatus, productCatalogId, null, null).ToList();
        //    }
        //}
        //public List<ProductCatalogAllegroItemsFnResult> GetProductCatalogItemsForImageUpdate(int limit, Guid processId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    { 
        //        List<Dal.ProductCatalogAllegroItemsFnResult> items = ctx.ProductCatalogAllegroItemsFn(limit, "INACTIVE", null, false, processId).ToList();

        //        items.AddRange(ctx.ProductCatalogAllegroItemsFn(limit, "ACTIVE", null, false, processId));
        //        return items;
        //    }
        //}
        //public List<ProductCatalogAllegroItemsFnResult> GetProductCatalogItemsForImageUpdate(int limit)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
                
                    
        //        //    return ctx.ProductCatalogAllegroItemsActive.Where(x => x.UpdateStatus == (int)Helper.AllegroItemUpdateStatus.Verifying).ToList();
        //        List<Dal.ProductCatalogAllegroItemsFnResult> items = ctx.ProductCatalogAllegroItemsFn(limit, "INACTIVE", null, false, null)
        //            .OrderBy(x=>x.ValidatedAt) 
        //            .ToList();

        //        if (items.Count() == limit)
        //            return items;

        //        items.AddRange(ctx.ProductCatalogAllegroItemsFn(limit, "ACTIVE", null, false, null)
        //            .OrderBy(x => x.ValidatedAt)) ;//.Take(limit - items.Count())) ;// ;// ;
        //        return items;
        //    }
        //}

        //public List<ProductCatalogAllegroItemsFnResult> GetProductCatalogAllegroItemsToUpdate(int limit, string itemStatus, Guid processId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        List<Dal.ProductCatalogAllegroItemsFnResult> items = ctx.ProductCatalogAllegroItemsFn(limit, itemStatus, null, true, processId).ToList();

        //        return items;
        //    }
        //}

        //public List<ProductCatalogAllegroItemsFnResult> GetAllegroItemsToUpdateForImages()
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogAllegroItemsFn("ACTIVE", null, false, null)
        //            .ToList();
        //    }
        //}
        
        public List<ProductCatalogAllegroItemsActive> GetAllegroItemsActiveToUpdate(int productCatalogId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogAllegroItemsActive.Where(x => x.ProductCatalogId == productCatalogId).ToList();

            }
        }

        public List<SupplierDeliveryTypeSource> GetDeliverySources(Helper.ShopType shopType)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierDeliveryTypeSource.Where(x => x.ShopTypeId == (int)shopType).ToList();
            }
        }
        public List<SupplierDeliveryTypeSource> GetDeliverySources()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierDeliveryTypeSource.ToList();
            }
        }

        public void SetProductCatalogUpdateAltavola(ProductCatalog pc, ProductCatalog p)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalog pToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();
                pToUpdate.ExternalId = p.ExternalId;
                ctx.SubmitChanges();
            }
        }
        public void SetProductCatalogUpdateAltavola1(ProductCatalog pc, ProductCatalog p)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalog pToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();
                pToUpdate.Ean = p.Ean;
                ;
                ctx.SubmitChanges();
            }
        }
 

        public ProductCatalogAttribute GetProductCatalogAttribute(string code)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttribute.Where(x => x.Code == code.ToUpper()).FirstOrDefault();
            }
        }

        public List<ProductCatalogAttributeGroupType> GetProductCatalogAttributeGroupTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeGroupType.ToList();
            }
        }

        public List<ProductCatalog> GetProductCatalogByIds(int[] productCatalogIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                //dlo.LoadWith<ProductCatalog>(x => x.ProductCatalogGroup);
                dlo.LoadWith<ProductCatalog>(x => x.Supplier);

                ctx.LoadOptions = dlo;
 

                int count = productCatalogIds.Length;
                List<ProductCatalog> t = new List<ProductCatalog>();

                int partsCount = (count / 2000);

                int[] itemsPart;
                for (int i = 0; i < partsCount + 1; i++)
                {
                    itemsPart = productCatalogIds.Skip(i * 2000).Take(2000).ToArray();
                    t.AddRange(
                        ctx.ProductCatalog.Where(x => //x.ShopProductId.HasValue &&
                        itemsPart.Contains(x.ProductCatalogId))
                    .ToList()

                        );

                }
                return t.ToList();

            }
        }

        public int[] GetProductAttributeGroupId(List<ProductCatalogAttributeToProduct> attributesToAdd)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] attributeIds = attributesToAdd.Select(x => x.AttributeId).Distinct().ToArray();
                return ctx.ProductCatalogAttribute.Where(x => attributeIds.Contains(x.AttributeId)).Select(x => x.AttributeGroupId).Distinct().ToArray();

            }
        }

        public List<ProductCatalogAttributeType> GetProductCatalogAttributeTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeType.ToList();

            }
        }

        //public ProductCatalogAllegroItemsView GetProductCatalogAllegroItemView(int id)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogAllegroItemsView.Where(x => x.Id == id).FirstOrDefault();

        //    }
        //}

        //public ProductCatalogAllegroItemsView GetProductCatalogAllegroItemView(long itemId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogAllegroItemsView.Where(x => x.ItemId == itemId).FirstOrDefault();

        //    }
        //}
        //public void SetProductCatalogAllegroItemUpdate(ProductCatalogAllegroItem item)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ProductCatalogAllegroItem itemToUpdate = ctx.ProductCatalogAllegroItem.Where(x => x.Id == item.Id).FirstOrDefault();

        //        itemToUpdate.Cost = item.Cost;
        //        itemToUpdate.LastUpdateDateTime = item.LastUpdateDateTime;
        //        itemToUpdate.UpdateComment = item.UpdateComment;
        //        itemToUpdate.UpdateDate = item.UpdateDate;
        //        itemToUpdate.UpdateStatus = item.UpdateStatus;

        //        ctx.SubmitChanges();
        //    }
        //}

        //public List<ProductCatalogUpdateScheduleView> GetProductCatalogUpdateSchedule(Dal.Helper.ShopType shopType, 
        //    Dal.Helper.UpdateScheduleType scheduleType,
        //    int limit)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        int[] shopIds = ctx.Shop.Where(x => x.ShopTypeId == (int)shopType).Select(x => x.ShopId).ToArray();

        //        return ctx.ProductCatalogUpdateScheduleView
        //            .Where(x => shopIds.Contains(x.ShopId)
        //                && x.UpdateStatusId == (int)Dal.Helper.ProductCatalogUpdateStatus.New
        //                && x.ScheduleTypeId == (int)scheduleType
        //                && x.ProcessId.HasValue==false
        //                )
        //            .OrderBy(x=>x.InsertDate)
        //            .Take(limit)
        //            .ToList();
        //    }
        //}

        public long[] GetProductCatalogAllegroItemsForSupplier(int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAllegroItem.Where(x => x.ProductCatalog.SupplierId == supplierId)
              
                    .Select(x => x.ItemId).ToArray();

            }
        }

        //public List<ProductCatalogUpdateSchedule> ProductCatalogUpdateSchedule(List<ProductCatalogUpdateScheduleView> schedulesCompleted,
        //    List<ProductCatalogUpdateScheduleView> schedulesErrors,
        //    Guid processId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        int[] schedulesToUpdateIds = schedulesCompleted.Select(x => x.Id).ToArray();

        //        List<ProductCatalogUpdateSchedule> schedulesToUpdate = ctx.ProductCatalogUpdateSchedule
        //            .Where(x => schedulesToUpdateIds.Contains(x.Id)
        //            && x.ProcessId == processId
        //            ).ToList();

        //        foreach (ProductCatalogUpdateSchedule schedule in schedulesToUpdate)
        //        { 
        //            schedule.UpdateStatusId = (int)Dal.Helper.ProductCatalogUpdateStatus.Completed;
        //            schedule.UpdateDate = DateTime.Now;
        //            schedule.UpdateComment = String.Format("Aktualizacja OK, batch {0}", processId);
        //        }
        //        ctx.SubmitChanges();


        //        schedulesToUpdateIds = schedulesErrors.Select(x => x.Id).ToArray();

        //        schedulesToUpdate = ctx.ProductCatalogUpdateSchedule
        //            .Where(x => schedulesToUpdateIds.Contains(x.Id)
        //            && x.ProcessId == processId
        //            ).ToList();

        //        int maxAttemptsCount = 3;

        //        foreach (ProductCatalogUpdateSchedule schedule in schedulesToUpdate)
        //        {
        //            int attempts = (schedule.RetryAttempts ?? 0);

        //            schedule.UpdateStatusId = attempts>= maxAttemptsCount?
        //                (int)Dal.Helper.ProductCatalogUpdateStatus.Error :
        //                (int)Dal.Helper.ProductCatalogUpdateStatus.New;
        //            schedule.ProcessId = null;
        //            schedule.UpdateDate = DateTime.Now;
        //            schedule.UpdateComment = schedulesErrors.Where(x=>x.Id == schedule.Id).Select(x=>x.UpdateComment).FirstOrDefault();
        //            schedule.RetryAttempts = attempts + 1;
        //        }

        //        ctx.SubmitChanges();

        //        return schedulesToUpdate.Where(x => x.RetryAttempts.HasValue && x.RetryAttempts.Value >= maxAttemptsCount).ToList();
        //    }
        //}

        //public void ProductCatalogUpdateSchedule(ProductCatalogUpdateScheduleView schedule, Helper.ProductCatalogUpdateStatus status)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ProductCatalogUpdateSchedule scheduleToUpdate = ctx.ProductCatalogUpdateSchedules.Where(x => x.Id == schedule.Id).FirstOrDefault();
        //        scheduleToUpdate.UpdateStatusId = (int)status;
        //        scheduleToUpdate.UpdateDate = DateTime.Now;
        //        scheduleToUpdate.UpdateComment = "";
        //        ctx.SubmitChanges();

        //    }
        //}
        //public void SetProductToCreateOnAllegro()
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ctx.AllegroAutoCreateAuctionSet();

        //    }
        //}

        public List<AllegroDeliveryCostType> GetAllegroDeliveryCostTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.AllegroDeliveryCostType.ToList();

            }
        } 

        //public void SetProductCatalogUpdateAvailability(List<ProductCatalog> products)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        foreach (Dal.ProductCatalog product in products)
        //        {
        //            Dal.ProductCatalog productToUpdate = ctx.ProductCatalog.
        //                Where(x => x.ProductCatalogId == product.ProductCatalogId).FirstOrDefault();

        //            productToUpdate.IsAvailable = product.IsAvailable;
        //            productToUpdate.PriceBruttoFixed = product.PriceBruttoFixed;

        //        }

        //        ctx.SubmitChanges();
        //    }
        //} 

        public void SetProductCatalogImageShopImageId(int imageId, int shopImageId)
        {

            Dal.ShopHelper sh = new ShopHelper();
            sh.SetProductCatalogImageShopImageId(Helper.Shop.Lajtitpl, imageId, shopImageId);

            //using (LajtitDB ctx = new LajtitDB())
            //{

            //    Dal.ProductCatalogImage image = ctx.ProductCatalogImage.Where(x => x.ImageId == imageId).FirstOrDefault();
            //    if(image!=null)
            //    {
            //        image.ShopImageId = shopImageId;
            //        ctx.SubmitChanges();
            //    }

            //}
        }

        //public int[] GetProductCatalogForShopCreateUpdate()
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {

        //        return ctx.ProductCatalog.Where(x => x.Supplier.AutoCreateUpdateShop && x.IsActiveOnline ).Select(x => x.ProductCatalogId).OrderByDescending(x=>x).ToArray();

        //    }
        //} 
         

        public List<ProductCatalog> GetProductCatalogByCode(string code)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalog.Where(x => x.Code == code).ToList();
            }
        }


        public void ProductCatalogShopUpdateScheduleSet(int[] productIds, int[] selectedShopIds, int[] selectedShopColumnTypeIds, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                string shopIds = String.Join(",", selectedShopIds);
                string columnIds = String.Join(",", selectedShopColumnTypeIds);
                foreach (int pId in productIds)
                {
                    ctx.ProductCatalogShopUpdateScheduleByProductSet(pId, shopIds, columnIds, userName);
                }
            }
        }
   

        public void SetProductCatalogStatus(int[] productIds, bool? isAvailable,   
            bool? isDiscontinued, bool? isHidden, bool? isReady, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach (int productCatalogId in productIds)
                {
                    ProductCatalog pc = ctx.ProductCatalog.Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();
                    if (isAvailable.HasValue)
                        pc.IsAvailable = isAvailable.Value;
 
                    if (isDiscontinued.HasValue)
                        pc.IsDiscontinued = isDiscontinued.Value;
                    if (isHidden.HasValue)
                        pc.IsHidden = isHidden.Value;
                    if (isReady.HasValue)
                        pc.IsReady = isReady.Value;
                    pc.UpdateUser = userName;
                    pc.UpdateReason = "Zmiana statusów";

                    ctx.SubmitChanges();
                }
            }
        } 
 
 
        public List<ProductCatalogSubProductsView> GetProductCatalogSubProducts(int productCatalogId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            { 

                return ctx.ProductCatalogSubProductsView.Where(x => x.ProductCatalogId == productCatalogId).ToList();
            }
        }
 

        public void SetProductCatalogSupplier(int[] productIds, int supplierId, string UserName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                List<Dal.ProductCatalog> products = ctx.ProductCatalog
                    .Where(x => productIds.Contains(x.ProductCatalogId)).ToList();

                foreach (Dal.ProductCatalog product in products)
                {
                    product.SupplierId = supplierId;

                }
                ctx.SubmitChanges();

            }
        }
         
        public List<AllegroActionView> GetAllegroActions()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {

                return ctx.AllegroActionView.ToList();

            }
        }

        public List<AllegroActionView> GetAllegroActionsActive()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {

                return ctx.AllegroActionView.Where(x=>!x.IsProcessed).ToList();

            }
        }

        public void SetAllegroAction(List<AllegroActionView> itemsProcessed)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                int[] ids = itemsProcessed.Select(x => x.ActionId).ToArray();
                long[] itemIds = itemsProcessed.Select(x => x.ItemId).ToArray();

                List<AllegroAction> allegroActions = ctx.AllegroAction.Where(x => ids.Contains(x.ActionId)).ToList();
                List<AllegroItem> allegroItems = ctx.AllegroItem.Where(x => itemIds.Contains(x.ItemId)).ToList();

                foreach (AllegroAction actionToUpdate in allegroActions)
                {
                    //AllegroActionView a = itemsProcessed.Where(x=>x.ItemId==actionToUpdate.ItemId).FirstOrDefault();
                    actionToUpdate.IsProcessed = true;
                    actionToUpdate.ProcessedDate = DateTime.Now;

                    AllegroItem itemToUpdate = allegroItems.Where(x => x.ItemId == actionToUpdate.ItemId).FirstOrDefault();

                    itemToUpdate.EndingDate = DateTime.Now;
                    itemToUpdate.ItemStatus = "ENDED";
                    itemToUpdate.DoNotReActive = actionToUpdate.DoNotReActive;

                }
                ctx.SubmitChanges();
            }
        }

        public void SetAllegroActionDelete(int[] actionIds)
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                List<AllegroAction> allegroActions = ctx.AllegroAction.Where(x => actionIds.Contains(x.ActionId)).ToList();

                foreach (AllegroAction actionToUpdate in allegroActions)
                {
                    actionToUpdate.IsProcessed = true;
                    actionToUpdate.ProcessedDate = DateTime.Now;
                    actionToUpdate.Comment = "Usunięto z kolejki";
                }

                ctx.SubmitChanges();
            }
        }

        //public void SetShopProductToProductCatalogByCode(string code, int shopProductId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {

        //        ctx.ShopProductToProductCatalogByCode(code, shopProductId);

        //    }
        //}
      

        public bool SetShopProductToProductCatalogByEan(Dal.Helper.Shop shop, string ean, string shopProductId)
        { 
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalog pc = ctx.ProductCatalog.Where(x => x.Ean == ean).FirstOrDefault();

                if (pc == null)
                {
                    return false;
                }

                Dal.ProductCatalogShopProduct sp = ctx.ProductCatalogShopProduct.Where(x => x.ShopId == (int)shop 
                && x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();

                if (sp == null)
                {
                    ctx.ProductCatalogShopProduct.InsertOnSubmit(
                        new ProductCatalogShopProduct()
                        {
                            IsNameLocked = false,
                            ProductCatalogId = pc.ProductCatalogId,
                            ShopId = (int)shop,
                            ShopProductId = shopProductId
                        }
                        );
                }
                else
                {
                    if (sp.ShopProductId == null)
                    {
                        sp.ShopProductId = shopProductId;
                    }
                }
              
                ctx.SubmitChanges();

            }
            return true;
        }
        public List<ProductCatalogAttributesForProductResult> GetProductCatalogAttributes(int attributeGroupId, int? productCatalogId, int? attributeGroupingId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (attributeGroupingId.HasValue && attributeGroupingId.Value == 0)
                    attributeGroupingId = null;

                    return ctx.ProductCatalogAttributesForProduct(attributeGroupId, productCatalogId, attributeGroupingId).ToList(); 

            }
        }

        public List<ProductCatalogAttributeGroup> GetProductCatalogAttributeGroups()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeGroup
                    .ToList();

            }
        }

        public List<ProductCatalogAttributeGroupForProductResult> GetProductCatalogAttributeGroups(int productCatalogId, int? attributeGroupingId, int schemaId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeGroupForProduct(productCatalogId, attributeGroupingId, schemaId)
                    .ToList();

            }
        }


        public void SetProductCatalogAttributes(int ProductCatalogId, 
            List<Dal.ProductCatalogAttributeToProduct> attributesToAdd,
            int[] allGroupIds
            //, bool clearBeforeAdd,
            //int? grupIdToClear
            )
        {
           // if (attributesToAdd.Count == 0)// && clearBeforeAdd == false)
            //    return; // nothing to do

            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalogAttributeToProduct> attributesToDelete;

                //if (clearBeforeAdd)
                //    attributesToDelete = ctx.ProductCatalogAttributeToProduct.Where(x => x.ProductCatalogId == ProductCatalogId).ToList();
                //else
                //{
                    #region
                    int[] selectedAttributeIds = attributesToAdd.Select(x => x.AttributeId).Distinct().ToArray();
                    // atrybuty słownikowe usuwamy per grupa
                    int[] attributesGroups = ctx.ProductCatalogAttribute
                        .Where(x =>
                            selectedAttributeIds.Contains(x.AttributeId)
                            && (x.ProductCatalogAttributeGroup.AttributeGroupTypeId == 1
                                || x.ProductCatalogAttributeGroup.AttributeGroupTypeId == 2))
                        .Select(x => x.AttributeGroupId)
                        .Distinct()
                        .ToArray();

                    attributesToDelete =
                        ctx.ProductCatalogAttributeToProduct
                        .Where(x =>
                            x.ProductCatalogId == ProductCatalogId
                            && allGroupIds.Contains(x.ProductCatalogAttribute.AttributeGroupId))
                        .ToList();

                    //List<ProductCatalogAttributeToProduct> attributes = new List<ProductCatalogAttributeToProduct>();
                    //attributes.AddRange(attributesToAdd);

                    // atrybuty liczbowe usuwamy per atrybut
                    int[] attributeIds = ctx.ProductCatalogAttribute
                        .Where(x =>
                            selectedAttributeIds.Contains(x.AttributeId)
                            && x.ProductCatalogAttributeGroup.AttributeGroupTypeId == 3)
                        .Select(x => x.AttributeId)
                        .Distinct()
                        .ToArray();

                    attributesToDelete.AddRange(
                        ctx.ProductCatalogAttributeToProduct
                        .Where(x => x.ProductCatalogId == ProductCatalogId
                        && attributeIds.Contains(x.AttributeId))
                        .ToList());


                // z parametrów wartościowych/liczbowych usun te ktore nie mają wartości
                int[] attributeValueIds = ctx.ProductCatalogAttribute
                     .Where(x =>
                         selectedAttributeIds.Contains(x.AttributeId)
                         &&  x.ProductCatalogAttributeGroup.AttributeGroupTypeId == 3 )
                     .Select(x => x.AttributeId)
                     .Distinct()
                     .ToArray();


                attributesToAdd.RemoveAll(x=>
                x.DecimalValue.HasValue==false && x.StringValue == null && attributeValueIds.Contains(x.AttributeId));
                //.ToList();



                    //if (grupIdToClear.HasValue)
                    //{

                //    attributesToDelete.AddRange(
                //        ctx.ProductCatalogAttributeToProduct
                //        .Where(x => x.ProductCatalogId == ProductCatalogId
                //        && x.ProductCatalogAttribute.ProductCatalogAttributeGroup.AttributeGroupId == grupIdToClear.Value)
                //        .ToList());
                //}
                #endregion
                //}


                //ctx.SubmitChanges();

                ctx.ProductCatalogAttributeToProduct.DeleteAllOnSubmit(attributesToDelete);
                    ctx.ProductCatalogAttributeToProduct.InsertAllOnSubmit(attributesToAdd);
                    ctx.SubmitChanges();
                }
            
        }
 

        public int SetProductCatalogAttributeGroup(ProductCatalogAttributeGroup attribute)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int lastId = ctx.ProductCatalogAttributeGroup.Max(x => x.AttributeGroupId);
                attribute.AllegroOrder = lastId+1;
                ctx.ProductCatalogAttributeGroup.InsertOnSubmit(attribute);
                ctx.SubmitChanges();

                return attribute.AttributeGroupId;
            }
        }

        public ProductCatalogAttributeGroup GetProductCatalogAttributeGroup(int attributeGroupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAttributeGroup>(x => x.ShopCategory);

                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogAttributeGroup
                    .Where(x => x.AttributeGroupId == attributeGroupId)
                    .FirstOrDefault();
            }
        }
 

        public void SetProductCatalogAttributeUpdate(

            ProductCatalogAttributeGroup group)
        {
            using (LajtitDB ctx = new LajtitDB())
            {


                Dal.ProductCatalogAttributeGroup groupToUpdate = ctx.ProductCatalogAttributeGroup
                    .Where(x => x.AttributeGroupId == group.AttributeGroupId)
                    .FirstOrDefault();
                groupToUpdate.ExportToShop = group.ExportToShop;
                groupToUpdate.Name = group.Name;
                groupToUpdate.AttributeGroupTypeId = group.AttributeGroupTypeId;

                groupToUpdate.GroupCode = group.GroupCode == "" ? null : group.GroupCode;
                groupToUpdate.AllegroOrder = group.AllegroOrder;

                ctx.SubmitChanges();


            }
        }

        //public List<ProductCatalogAttributeToProduct> GetProductCatalogAttributesForShopProduct(Dal.Helper.Shop shop, int[] productCatalogIds)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<ProductCatalogAttributeToProduct>(x => x.ProductCatalogAttribute);
        //        dlo.LoadWith<ProductCatalogAttribute>(x => x.ProductCatalogAttributeGroup);
        //        int[] ids = new int[] { 1, 2 };
        //        ctx.LoadOptions = dlo;

        //        List<ProductCatalogAttributeToProduct> attributes = new List<ProductCatalogAttributeToProduct>();


        //        int count = productCatalogIds.Length;
        //        ///List<int> t = new List<int>();

        //        int partsCount = (count / 2000);

        //        int[] itemsPart;
        //        for (int i = 0; i < partsCount + 1; i++)
        //        {
        //            itemsPart = productCatalogIds.Skip(i * 2000).Take(2000).ToArray();

        //            attributes.AddRange(ctx.ProductCatalogAttributeToProduct
        //                .Where(x => itemsPart.Contains(x.ProductCatalogId)
        //                    && x.ProductCatalogAttribute.ProductCatalogAttributeGroup.ExportToShop
        //                    && x.ProductCatalogAttribute.ProductCatalogAttributeGroup.ShopAttributeId.HasValue
        //                    && ids.Contains(x.ProductCatalogAttribute.ProductCatalogAttributeGroup.AttributeGroupTypeId)
        //                    && x.IsDefault.HasValue
        //                    && x.IsDefault.Value)
        //                .ToList());

        //            attributes.AddRange(ctx.ProductCatalogAttributeToProduct
        //                .Where(x => itemsPart.Contains(x.ProductCatalogId)
        //                    && x.ProductCatalogAttribute.ProductCatalogAttributeGroup.ExportToShop
        //                    && x.ProductCatalogAttribute.ShopAttributeId.HasValue
        //                    && x.ProductCatalogAttribute.ProductCatalogAttributeGroup.AttributeGroupTypeId == 3)
        //                .ToList());

        //        } 



        //        return attributes;
        //    }
        //}

        public List<ProductCatalogAttributesForProductPrevResult> GetProductCatalogAttributesForProduct(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributesForProductPrev(productCatalogId).ToList();
            }
            }

        public void SetProductCatalog(ProductCatalog pc )
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalog.InsertOnSubmit(pc);
                ctx.SubmitChanges();
            }
        }
        //public void SetProductCatalog(ProductCatalog pc, int[] selectedProductIds)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ProductCatalog productCatalogToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();

        //        productCatalogToUpdate.ShopProductId = pc.ShopProductId;

        //        ctx.SubmitChanges();

        //        //List<Dal.ProductCatalogOption> productOptions = ctx.ProductCatalogOptions.Where(x => x.ProductCatalogId == pc.ProductCatalogId).ToList();

        //        //if (productOptions.Count() > 0)
        //        //    ctx.ProductCatalogOptions.DeleteAllOnSubmit(productOptions);
        //        //ctx.SubmitChanges();

        //        //List<ProductCatalogOption> productOptionsToInsert = selectedProductIds.Select(x =>
        //        //new ProductCatalogOption()
        //        //{
        //        //    ProductCatalogId = pc.ProductCatalogId,
        //        //    ProductCatalogOptionId = x,
        //        //    ShopInclude = true

        //        //}).ToList();

        //        //ctx.ProductCatalogOptions.InsertAllOnSubmit(productOptionsToInsert);

        //        //ctx.SubmitChanges();
        //    }
        //}

        public int[] GetProductCatalogAttributes(int[] filteredProductCatalogIds, int attributeGroupId, bool? exists, int? attributeId, bool? existsValue)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                if (exists.HasValue && exists.Value)
                {
                    int count = filteredProductCatalogIds.Length;
                    List<int> t = new List<int>();

                    int partsCount = (count / 2000);

                    int[] itemsPart;
                    for (int i = 0; i < partsCount + 1; i++)
                    {
                        itemsPart = filteredProductCatalogIds.Skip(i * 2000).Take(2000).ToArray();
                        t.AddRange(ctx.ProductCatalogAttributeToProduct
                        .Where(x => x.ProductCatalogAttribute.AttributeGroupId == attributeGroupId
                            && itemsPart.Contains(x.ProductCatalogId) == true)
                            .Select(x => x.ProductCatalogId)
                            .Distinct()
                            .ToArray());

                    }
                    filteredProductCatalogIds = t.ToArray();
                }
                if (exists.HasValue && !exists.Value)
                {
                    int count = filteredProductCatalogIds.Length;
                    List<int> t = new List<int>();

                    int partsCount = (count / 2000);

                    int[] itemsPart;
                    for (int i = 0; i < partsCount + 1; i++)
                    {
                        itemsPart = filteredProductCatalogIds.Skip(i * 2000).Take(2000).ToArray();
                        //   uil.AddRange(allegroService.doGetUserItems(userId, webApiKey, 1, i, 100, out count).ToList());
                        t.AddRange(ctx.ProductCatalogAttributeToProduct
                           .Where(x => x.ProductCatalogAttribute.AttributeGroupId == attributeGroupId
                               && itemsPart.Contains(x.ProductCatalogId) == true)
                               .Select(x => x.ProductCatalogId)
                               .Distinct()
                               .ToArray());

                    }


                    filteredProductCatalogIds =  filteredProductCatalogIds.Where(x => t.Contains(x) == false).ToArray();

                }
                if(existsValue.HasValue)
                {
                    int count = filteredProductCatalogIds.Length;
                    List<int> t = new List<int>();

                    int partsCount = (count / 2000);

                    int[] itemsPart;
                    for (int i = 0; i < partsCount + 1; i++)
                    {
                        itemsPart = filteredProductCatalogIds.Skip(i * 2000).Take(2000).ToArray();
                        //   uil.AddRange(allegroServicePr.doGetUserItems(userId, webApiKey, 1, i, 100, out count).ToList());
                        t.AddRange(ctx.ProductCatalogAttributeToProduct
                           .Where(x => x.AttributeId == attributeId.Value && itemsPart.Contains(x.ProductCatalogId))
                               .Select(x => x.ProductCatalogId)
                               .Distinct()
                               .ToArray());
                    }
                    if (existsValue.Value)
                    {
                        filteredProductCatalogIds = filteredProductCatalogIds.Where(x => t.Contains(x)).ToArray();
                    }
                    else
                    {
                        filteredProductCatalogIds = filteredProductCatalogIds.Where(x => !t.Contains(x)).ToArray();

                    }






                }
                return filteredProductCatalogIds;
                //}
                //else
                //{
                //    int e = exists?1:0; int[] pg;
                //    if(exists)
                //   pg = ctx.ProductCatalogAttributeCompleteds
                //        .Where(x=>
                //            filteredProductCatalogIds.Contains(x.ProductCatalogId)
                //            &&
                //            x.GroupCount == x.ProductGroupCount)
                //            .Select(x=>x.ProductCatalogId)
                //            .ToArray();
                //    else
                //        pg = ctx.ProductCatalogAttributeCompleteds
                //             .Where(x =>
                //                 filteredProductCatalogIds.Contains(x.ProductCatalogId)
                //                 &&
                //                 x.ProductGroupCount==0)
                //                 .Select(x => x.ProductCatalogId)
                //                 .ToArray();


                //        return ctx.ProductCatalogAttributeToProduct
                //            .Where(x => pg.Contains(x.ProductCatalogId) == true)
                //                .Select(x => x.ProductCatalogId)
                //                .Distinct()
                //                .ToArray();


                //    }
            }
        }

        //public List<ProductCatalog> GetProductCatalogByShopId(int[] shopProductIds)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalog.Where(x => x.ShopProductId.HasValue && shopProductIds.Contains(x.ShopProductId.Value)).ToList();
        //    }
        //}
    


        public List<ShopCategoryFnResult> GetShopCategories(int shopTypeId, string categoryParentId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var scv = ctx.ShopCategoryFn(shopTypeId);

                if (categoryParentId !=null && categoryParentId!="0")
                    scv = scv.Where(x => x.CategoryParentId == categoryParentId);
                else
                    scv = scv.Where(x => x.CategoryParentId == null);
                return scv.ToList();
            }
        }
        public List<ProductCatalogAttributeCategoryFnResult> GetShopProductAndCategoriesFromAttributes(int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeCategoryFn(shopId).ToList();
            }
        }
        public List<ProductCatalogAttributeCategoryFnResult> GetShopProductAndCategoriesFromAttributes(int shopId, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeCategoryFn(shopId)
                    .Where(x=>x.ProductCatalogId== productCatalogId)
                    .ToList();
            }
        }

        public List<ProductCatalogAttributeCategoryFnResult> GetShopProductAndCategoriesFromAttributes(int shopId, int[] productCatalogIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAttributeCategoryFn(shopId)
                    .Where(x=> productCatalogIds.Contains(x.ProductCatalogId))
                    .ToList();
            }
        }


        public List<ProductCatalogView> GetProductCatalogView(int[] productCatalogIds)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                int batchSize = 2000;
                List<ProductCatalogView> products = new List<ProductCatalogView>();

                if (productCatalogIds.Length > 0)
                {
                    int partsCount = (productCatalogIds.Length / batchSize);
                    for (int i = 0; i < partsCount + 1; i++)
                    {
                        int[] transactionIDsPart = productCatalogIds.Skip(i * batchSize).Take(batchSize).ToArray();

                        if (transactionIDsPart.Length == 0)
                            continue;

                        products.AddRange(
                            ctx.ProductCatalogView
                                .Where(x => transactionIDsPart.Contains(x.ProductCatalogId)).ToList()
                            );
                    }

                }


                return products;
            }
        }
        public ProductCatalogView GetProductCatalogView(int productCatalogId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            { 
                return ctx.ProductCatalogView
                    .Where(x => x.ProductCatalogId == productCatalogId)
                    .FirstOrDefault();
            }
        }

        public ProductCatalog GetProductCatalogOnShopProductId(Dal.Helper.Shop shop, string productId)
        {
            using (LajtitDB ctx = new LajtitDB())
            { 

                return ctx.ProductCatalogShopProduct
                    .Where(x =>x.ShopId==(int)shop && x.ShopProductId==productId )
                    .Select(x=>x.ProductCatalog)
                    .FirstOrDefault();
            }
        }

        public List<ProductCatalogImage> GetProductCatalogImages(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogImage
                    .Where(x => x.ProductCatalogId == productCatalogId )
                    .OrderBy(x => x.Priority)
                    .ToList();
            }
        }
         

    }
}
