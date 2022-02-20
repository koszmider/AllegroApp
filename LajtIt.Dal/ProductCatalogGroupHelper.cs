using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class ProductCatalogGroupHelper
    {
        public ProductCatalogGroup GetProductCatalogLine(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var g = ctx.ProductCatalogGroupProduct
                    .Where(x => x.ProductCatalogId == productCatalogId 
                        && x.ProductCatalogGroup.ProductCatalogGroupFamily.FamilyTypeId == (int)Dal.Helper.ProductCatalogGroupFamilyType.Family)
                    .Select(x => x.ProductCatalogGroup)
                    .FirstOrDefault();

                return g;
            }
        }

        public List<ProductCatalogFamilyGroupView> GetProductCatalogFamilyGroup(int productCatalogId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogFamilyGroupView.Where(x => x.ProductCatalogId == productCatalogId).ToList();
            }
        }
        public List<ProductCatalogGroupFamilyType> GetProductCatalogFamilyTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroupFamilyType.ToList();
            }
        }

        public List<ProductCatalogGroup> GetProductCatalogGroups(int supplierId, Dal.Helper.ProductCatalogGroupFamilyType ft)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroup
                    .Where(x => x.SupplierId == supplierId && x.ProductCatalogGroupFamily.FamilyTypeId == (int)ft)
                    .ToList();

            }
        }

        public void SetProductCatalogsByProductFamily(int id, bool createNew, Guid setId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var f = ctx.ProductCatalogGroupFamilyAllegro
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                f.CanUpdate = false;

                if (createNew && !f.SetId.HasValue)
                    f.SetId = setId;

                ctx.SubmitChanges();

            }
        }

        public void SetProductCatalogsByProductFamilyDelete(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var f = ctx.ProductCatalogGroupFamilyAllegro
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                ctx.ProductCatalogGroupFamilyAllegro.DeleteOnSubmit(f);

                ctx.SubmitChanges();

            }
        }

        public void SetProductCatalogsByProductFamilyClear(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var f = ctx.ProductCatalogGroupFamilyAllegro
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                f.SetId = null;
                f.CanUpdate = true;

                ctx.SubmitChanges();

            }
        }
        public void SetProductCatalogGroupDelete(int productCatalogId, int groupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var p = ctx.ProductCatalogGroupProduct.Where(x => x.ProductCatalogId == productCatalogId && x.ProductCatalogGroupId == groupId).FirstOrDefault();

                ctx.ProductCatalogGroupProduct.DeleteOnSubmit(p);

                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogGroup> GetProductCatalogGroups(ProductCatalogGroupFamily f)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroup
                    .Where(x =>  x.FamilyId == f.FamilyId)
                    .ToList();

            }
        }
        public List<ProductCatalogGroup> GetProductCatalogGroups()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroup
                    .ToList();

            }
        }
        public void SetProductCatalogGroups(List<ProductCatalogGroup> groups)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int lastGroupId = ctx.ProductCatalogGroup.Max(x => x.ProductCatalogGroupId);
                foreach (ProductCatalogGroup group in groups)
                {
                    if (ctx.ProductCatalogGroup.Where(x => x.SupplierId == group.SupplierId && x.GroupName == group.GroupName).FirstOrDefault() == null)
                    {
                        lastGroupId++;
                        group.ProductCatalogGroupId = lastGroupId;
                        ctx.ProductCatalogGroup.InsertOnSubmit(group);
                    }
                }
                ctx.SubmitChanges();
            }
        }
        public ProductCatalogGroup SetProductCatalogGroup(ProductCatalogGroup group)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogGroup>(x => x.ProductCatalogGroupFamily);
                dlo.LoadWith<ProductCatalogGroupFamily>(x => x.ProductCatalogGroupFamilyType);

                ctx.LoadOptions = dlo;

                ProductCatalogGroup g;

                if (group.ProductCatalogGroupId == 0)
                    g = ctx.ProductCatalogGroup.Where(x => x.SupplierId == group.SupplierId && x.FamilyId == group.FamilyId && x.GroupName == group.GroupName ).FirstOrDefault();
                else
                    g = ctx.ProductCatalogGroup.Where(x => x.ProductCatalogGroupId == group.ProductCatalogGroupId).FirstOrDefault();

                if (g != null)
                {
                    g.GroupName = group.GroupName;
                    g.SupplierId = group.SupplierId;
                    g.FamilyId = group.FamilyId;
                    ctx.SubmitChanges();
                    return g;
                }
                else
                {
                    int max = ctx.ProductCatalogGroup.Max(x => x.ProductCatalogGroupId);
                    group.ProductCatalogGroupId = max + 1;

                    if(group.FamilyId==0)
                    {

                        ProductCatalogGroupFamily family = ctx.ProductCatalogGroupFamily.Where(x => x.SupplierId == group.SupplierId
                        && x.FamilyName == group.GroupName).FirstOrDefault();

                        if (family == null)
                        {
                            family = new ProductCatalogGroupFamily()
                            {
                                FamilyName = group.GroupName,
                                FamilyTypeId = (int)Dal.Helper.ProductCatalogGroupFamilyType.Family,
                                SupplierId = group.SupplierId
                            };

                            group.ProductCatalogGroupFamily = family;
                        }
                        else
                            group.FamilyId = family.FamilyId;

                    }

                    ctx.ProductCatalogGroup.InsertOnSubmit(group);
                    ctx.SubmitChanges();
                    return group;
                }

            }
        }
        public List<ProductCatalogGroup> GetProductCatalogGroups(int supplierId)
        {
            return GetProductCatalogGroups(supplierId, Helper.ProductCatalogGroupFamilyType.Family);
        }

        public List<ProductCatalogGroupFamily> GetProductCatalogFamilies(int? supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if(supplierId.HasValue)
                    return ctx.ProductCatalogGroupFamily
                        .Where(x => x.SupplierId == supplierId.Value)
                        .ToList();
                else
                    return ctx.ProductCatalogGroupFamily
                        .Where(x => x.SupplierId == null)
                        .ToList();

            }
        }

        public List<ProductCatalogGroupFamily> GetProductCatalogFamilies()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroupFamily 
                    .ToList();

            }
        }
        public void SetProductCatalogGroupsMove(int familyId, int[] groupIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalogGroup> groupsToUpdate = ctx.ProductCatalogGroup.Where(x => groupIds.Contains(x.ProductCatalogGroupId)).ToList();

                foreach (ProductCatalogGroup groupToUpdate in groupsToUpdate)
                    groupToUpdate.FamilyId = familyId;
                ctx.SubmitChanges();
            }
        }

        public Dal.ProductCatalogGroupFamily GetProductCatalogFamily(int familyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroupFamily
                    .Where(x => x.FamilyId== familyId)
                    .FirstOrDefault();

            }
        }

        public ProductCatalogGroupFamily SetProductCatalogFamily(ProductCatalogGroupFamily family)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var f = ctx.ProductCatalogGroupFamily.Where(x => x.FamilyId == family.FamilyId /*&& x.FamilyName == family.FamilyName*/).FirstOrDefault();

                if (f != null)
                {
                    f.SupplierId = family.SupplierId;
                    f.FamilyName = family.FamilyName;
                    f.FamilyTypeId = family.FamilyTypeId;
                    ctx.SubmitChanges();

                    return f;
                }
                else
                {
                    ctx.ProductCatalogGroupFamily.InsertOnSubmit(family);
                    ctx.SubmitChanges();

                    return family;

                }

            }
        }
        public ProductCatalogGroupFamily SetProductCatalogFamilyByName(ProductCatalogGroupFamily family)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var f = ctx.ProductCatalogGroupFamily.Where(x => x.FamilyName == family.FamilyName && x.SupplierId == family.SupplierId/*&& x.FamilyName == family.FamilyName*/).FirstOrDefault();

                if (f != null)
                { 

                    return f;
                }
                else
                {
                    ctx.ProductCatalogGroupFamily.InsertOnSubmit(family);
                    ctx.SubmitChanges();

                    return family;

                }

            }
        }

        public ProductCatalogGroup GetProductCatalogGroup(int groupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogGroup>(x => x.ProductCatalogGroupFamily);
                dlo.LoadWith<ProductCatalogGroupFamily>(x => x.ProductCatalogGroupFamilyType);

                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogGroup.Where(x => x.ProductCatalogGroupId == groupId).FirstOrDefault();
            }
        }

        public List<Dal.ProductCatalogRelatedByFamilyFnResult> GetProductCatalogsByProductFamily(Dal.Helper.Shop shop, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                //List<Dal.ProductCatalogGroup> groups = ctx.ProductCatalogGroupProduct
                //    .Where(x => x.ProductCatalogId == productCatalogId
                //        && x.ProductCatalogGroup.ProductCatalogGroupFamily.FamilyTypeId == (int)familyType)
                //    .Select(x => x.ProductCatalogGroup)
                //    .ToList();

                //if (groups.Count==0)
                //    return new List<ProductCatalog>();

                //int[] familyIds = groups.Select(x => x.FamilyId).Distinct().ToArray();


                //return ctx.ProductCatalogGroupProduct
                //    .Where(x => x.ProductCatalogId != productCatalogId && familyIds.Contains(x.ProductCatalogGroup.FamilyId))
                //    .Select(x => x.ProductCatalog)
                //    .Distinct()
                //    .ToList();

                return ctx.ProductCatalogRelatedByFamilyFn((int)shop, productCatalogId).ToList();
            }
        }

        public void SetProductCatalogGroupMerge(ProductCatalogGroup newGroup, int[] groupIdsToMerge)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ProductCatalogGroupProduct> gp = ctx.ProductCatalogGroupProduct.Where(x => groupIdsToMerge.Contains(x.ProductCatalogGroupId)).ToList();

                List<Dal.ProductCatalogGroupProduct> gpToAdd = new List<ProductCatalogGroupProduct>();

                gpToAdd.AddRange(gp.Select(x => new ProductCatalogGroupProduct()
                {
                    InsertDate = x.InsertDate,
                    InsertUser = x.InsertUser,
                    ProductCatalogId = x.ProductCatalogId,
                    ProductCatalogGroupId = newGroup.ProductCatalogGroupId

                }).ToList());

                ctx.ProductCatalogGroupProduct.DeleteAllOnSubmit(gp);
                ctx.ProductCatalogGroupProduct.InsertAllOnSubmit(gpToAdd);

                ctx.SubmitChanges();

                List<Dal.ProductCatalogGroup> groupsToDelete = ctx.ProductCatalogGroup.Where(x => groupIdsToMerge.Contains(x.ProductCatalogGroupId)).ToList();

                foreach (Dal.ProductCatalogGroup g in groupsToDelete)
                {
                    try
                    {
                        ctx.ProductCatalogGroup.DeleteOnSubmit(g);
                        ctx.SubmitChanges();
                    }
                    catch (Exception ex)
                    {

                    }
                }


            }
        }

        public void SetProductCatalogFamilyFromGroup(int[] groups)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach(int groupId in groups)
                {
                    Dal.ProductCatalogGroup group = ctx.ProductCatalogGroup.Where(x => x.ProductCatalogGroupId == groupId).FirstOrDefault();

                    Dal.ProductCatalogGroupFamily family = ctx.ProductCatalogGroupFamily.Where(x => x.SupplierId == group.SupplierId &&
                    x.FamilyName.ToLower() == group.GroupName.ToLower()).FirstOrDefault();


                    if(family==null)
                    {
                        Dal.ProductCatalogGroupFamily familyToAdd = new ProductCatalogGroupFamily()
                        {
                            FamilyName = group.GroupName,
                            FamilyTypeId = (int)Dal.Helper.ProductCatalogGroupFamilyType.Family,
                            SupplierId = group.SupplierId
                        };

                        ctx.ProductCatalogGroupFamily.InsertOnSubmit(familyToAdd);
                        ctx.SubmitChanges();

                        group.FamilyId = familyToAdd.FamilyId;

                        ctx.SubmitChanges();
                    }

                }
            }
        }

        public void SetProductCatalogGroupsProducts(int[] productIds, int productCatalogGroupId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogGroupFamily groupFamily = ctx.ProductCatalogGroup
                    .Where(x => x.ProductCatalogGroupId == productCatalogGroupId)
                    .Select(x=>x.ProductCatalogGroupFamily)
                    .FirstOrDefault();

                if (groupFamily.FamilyTypeId == (int)Dal.Helper.ProductCatalogGroupFamilyType.Family)
                {

                    List<Dal.ProductCatalogGroupProduct> groupsToDelete = ctx.ProductCatalogGroupProduct.Where(x => productIds.Contains(x.ProductCatalogId)
                    && x.ProductCatalogGroup.ProductCatalogGroupFamily.FamilyTypeId == (int)Dal.Helper.ProductCatalogGroupFamilyType.Family)
                    .ToList();
                    List<Dal.ProductCatalogGroupProduct> groupsToAdd = productIds.Select(x => new ProductCatalogGroupProduct()
                    {
                        InsertDate = DateTime.Now,
                        InsertUser = userName,
                        ProductCatalogGroupId = productCatalogGroupId,
                        ProductCatalogId = x
                    }).ToList();

                    ctx.ProductCatalogGroupProduct.DeleteAllOnSubmit(groupsToDelete);
                    ctx.ProductCatalogGroupProduct.InsertAllOnSubmit(groupsToAdd);
                }
                else
                {
                    int[] pIdg = ctx.ProductCatalogGroupProduct
                        .Where(x => productIds.Contains(x.ProductCatalogId) && x.ProductCatalogGroupId == productCatalogGroupId)
                        .Select(x => x.ProductCatalogId)
                        .ToArray();

                    int[] productsNotInGroup = productIds.Except(pIdg).ToArray();

                    ctx.ProductCatalogGroupProduct.InsertAllOnSubmit(
                        productsNotInGroup.Select(x => new ProductCatalogGroupProduct()
                        {
                            InsertDate=DateTime.Now,
                            InsertUser=userName,
                            ProductCatalogId=x,
                            ProductCatalogGroupId=productCatalogGroupId
                        }).ToList());

                }

                ctx.SubmitChanges();
            }
        }
        public  int SetProductCatalogGroupProduct(string groupName, int productCatalogId, int supplierId)
        {
            if (groupName == null)
                groupName = "";
             

            List<ProductCatalogGroup> groups = GetProductCatalogGroups(supplierId);

            ProductCatalogGroup group = groups.Where(x => x.GroupName.ToLower() == groupName.Trim().ToLower()).FirstOrDefault();

            if (group == null)
            {
                group = new ProductCatalogGroup()
                {
                    GroupName = groupName.Trim(),
                    SupplierId = supplierId,
                    FamilyId = GetProductCatalogFamilyDefault(supplierId)
                };
                group = SetProductCatalogGroup(group);
            }

            SetProductCatalogGroupsProducts(new int[] { productCatalogId }, group.ProductCatalogGroupId, "system");

            return group.ProductCatalogGroupId;
        }
        public int SetProductCatalogFamiliesJoin(int[] familyIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int familyIdToKeep = familyIds[0];

                List<Dal.ProductCatalogGroup> groups = ctx.ProductCatalogGroup.Where(x => x.FamilyId != familyIdToKeep && familyIds.Contains(x.FamilyId)).ToList();

                foreach (Dal.ProductCatalogGroup group in groups)
                    group.FamilyId = familyIdToKeep;

                List<Dal.ProductCatalogGroupFamilyAllegro> familiesAllegro = ctx.ProductCatalogGroupFamilyAllegro.Where(x => x.FamilyId != familyIdToKeep && familyIds.Contains(x.FamilyId)).ToList();

                foreach (Dal.ProductCatalogGroupFamilyAllegro fa in familiesAllegro)
                    fa.FamilyId = familyIdToKeep;

                List<Dal.ProductCatalogGroupFamily> families = ctx.ProductCatalogGroupFamily.Where(x => x.FamilyId != familyIdToKeep && familyIds.Contains(x.FamilyId)).ToList();

                ctx.ProductCatalogGroupFamily.DeleteAllOnSubmit(families);

                ctx.SubmitChanges();

                return familyIdToKeep;
            }
        }

        public int GetProductCatalogFamilyDefault(int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            { 
                Dal.ProductCatalogGroupFamily family = ctx.ProductCatalogGroupFamily.Where(x => x.SupplierId == supplierId
                && x.FamilyTypeId == (int)Dal.Helper.ProductCatalogGroupFamilyType.Family
                && x.FamilyName == "").FirstOrDefault();

                if(family==null)
                {
                    family = new ProductCatalogGroupFamily()
                    {
                        FamilyName = "",
                        FamilyTypeId = (int)Dal.Helper.ProductCatalogGroupFamilyType.Family,
                        SupplierId = supplierId
                    };
                    ctx.ProductCatalogGroupFamily.InsertOnSubmit(family);
                }

                 
                return family.FamilyId;
            }
        }

        public long[] GetProductCatalogsByProductFamilyForAllegro(int familyId, long allegroUserId, int allegroCategoryId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroupFamilyAllegroFn(familyId, allegroUserId, allegroCategoryId).Select(x => x.ItemId).ToArray();
            }
        }

        public List<Dal.ProductCatalogGroupFamilyAllegro> GetGroupFamilyForAllegro()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroupFamilyAllegro.Where(x => x.CanUpdate == true && x.AllegroCategoryId.HasValue).ToList();
            }

        }
        public List<Dal.ProductCatalogGroupFamilyAllegro> GetGroupFamilyForAllegroCreated()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroupFamilyAllegro.Where(x => x.SetId.HasValue).ToList();
            }

        }

        public int[] GetProductCatalogGroupProducts(int groupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroupProduct.Where(x => //x.ProductCatalogGroup.ProductCatalogGroupFamily.FamilyTypeId == (int)Dal.Helper.ProductCatalogGroupFamilyType.Family
                //&& 
                x.ProductCatalogGroupId == groupId)
                .Select(x => x.ProductCatalogId)
                .Distinct()
                .ToArray();
            }
        }
    }
}
