using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Bll
{
    public partial class ShopUpdateHelper
    {
        public class Allegro
        {
            public void Process(List<Dal.ProductCatalogShopUpdateSchedule> schedules, Guid processId)
            {
                int[] productCatalogIds = schedules.Select(x => x.ProductCatalogId).Distinct().ToArray();


                foreach(int pId in productCatalogIds)
                {
                    var sch = schedules.Where(x => x.ProductCatalogId == pId).ToList();

                    ProcessAllegro(pId, sch, processId);
                }


            }
            public  ShopHelper.UpdateResult ProcessAllegro(int productCatalogId, List<Dal.ProductCatalogShopUpdateSchedule> schedulesBatch, Guid processId)
            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                Dal.SettingsHelper sh = new Dal.SettingsHelper();
                Dal.Settings s = sh.GetSetting("ALL_IMGS");


                List<Dal.ProductCatalogAllegroItemsView> items
                    = Dal.DbHelper.ProductCatalog.Allegro
                    .GetProductCatalogAllegroItemsToUpdate(new string[] { "INACTIVE", "ACTIVE" }, s.IntValue.Value, productCatalogId);
                //   = pch.GetProductCatalogAllegroItems(s.IntValue.Value, productCatalogId, /*"NOTCREATED",*/ "INACTIVE", "ACTIVATING");

                ShopHelper.UpdateResult result = new ShopHelper.UpdateResult();
                result.ProductCatalogId = productCatalogId;

                if (items.Count()==0)
                {

                    result.Result = 1;
                    return result;
                }

                long[] itemIds = items.Select(x => x.ItemId).ToArray();
                Dal.DbHelper.ProductCatalog.Allegro.SetProductCatalogAllegroItemProcess(processId, itemIds);


                //Dal.Shop[] shops = schedulesBatch.Select(x => x.Shop).Distinct().ToArray();

                int[] shopIds = items.Select(x => x.ShopId).ToArray();

                int actionTypeId = 1; //usun


                foreach (int shopId in shopIds)
                { 
                    List<Dal.ProductCatalogShopUpdateSchedule> schedules = schedulesBatch.Where(x => x.ShopId == shopId).ToList();

                    bool isStatusAction = ShopUpdateHelper.CanUpdateField(false, schedules, Dal.Helper.ShopColumnType.Status);
                     
                    Dal.ProductCatalogShopProductFnResult pc =
                        Dal.DbHelper.ProductCatalog.GetProductCatalogShopProduct(shopId, productCatalogId);
                   

                    if (isStatusAction || pc.IsPSActive == false 
                        || (pc.IsOnStock == false && pc.SellOnlyFromStock))
                    {
                        if (pc.IsPSActive.Value == false || (pc.IsOnStock == false && pc.SellOnlyFromStock))
                        {
                            Dal.OrderHelper oh = new Dal.OrderHelper();
                            oh.SetAllegroActions(new int[] { pc.ProductCatalogId }, shopId, actionTypeId, true, "Usunięte z modułu aktualizacji ofert ProcessAllegro" );
                            result.Result = 1;

                            return result;
                        }
                    }

                    AllegroHelper ah = new AllegroHelper();
                   
                    if (items.Count > 0)
                    {
                        foreach (Dal.ProductCatalogAllegroItemsView item in items.Where(x=>x.ShopId==shopId).ToList())
                        { 
                            ah.UpdateAllegroItem(item, schedules);
                        }
                    }
                    //else
                    //  ah.CreateNewAllegroAuction(productCatalogId, shop.ExternalId, itemsCreating);
                }
                result.Result = 1;

                return result;

            }

        }
    }
}
