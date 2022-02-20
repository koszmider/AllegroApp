using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class ShopUpdateHelper
    {
        public List<Dal.ProductCatalogShopUpdateSchedule> GetSchedule(Dal.Helper.ShopType shopType, Guid processId)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogShopUpdateSchedule>(x => x.Shop);
                ctx.LoadOptions = dlo;

                SettingsHelper sh = new SettingsHelper();
                
                List<ProductCatalogShopUpdateSchedule> schedules = ctx.ProductCatalogShopUpdateSchedule.Where(x =>
               x.UpdateStatusId == (int)Dal.Helper.ShopUpdateStatus.New
               && x.ProcessId.HasValue == false
               && x.Shop.ShopTypeId == (int)shopType)
                    .Take(sh.GetSetting("SHOP_UPD").IntValue.Value)
                 .ToList();

                foreach (Dal.ProductCatalogShopUpdateSchedule schedule in schedules)
                    schedule.ProcessId = processId;

                ctx.SubmitChanges();

                return schedules;
            };
        }
         
    }
}
