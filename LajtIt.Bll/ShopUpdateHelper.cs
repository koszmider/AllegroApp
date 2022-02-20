using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Bll
{
    public partial class ShopUpdateHelper
    {

        public static void ProcessClickShopSingle(Dal.Helper.ShopType shopType)
        {
            Process(shopType, Dal.Helper.UpdateScheduleType.OnlineShopSingle);
        }
        public static void ProcessClickShopBatch(Dal.Helper.ShopType shop)
        {
            Process(shop, Dal.Helper.UpdateScheduleType.OnlineShopBatch);
        }
    
 
        private static void Process(Dal.Helper.ShopType shopType, Dal.Helper.UpdateScheduleType updateScheduleType)
        {
            Guid processId = Guid.NewGuid();

            Dal.ShopUpdateHelper suh = new Dal.ShopUpdateHelper();
            List<Dal.ProductCatalogShopUpdateSchedule> schedule = suh.GetSchedule(shopType, processId);
            try
            {
                switch (shopType)
                {
                    case Dal.Helper.ShopType.ShoperLajtitPl:
                    case Dal.Helper.ShopType.ShoperOswietlenieTechnicznePl:

                        Bll.ShopRestHelper.Products.Process(schedule, processId);
                        break;
                    case
                        Dal.Helper.ShopType.Allegro:
                        Bll.ShopUpdateHelper.Allegro allegro = new Bll.ShopUpdateHelper.Allegro();

                        allegro.Process(schedule, processId);
                        break;
                    case
                        Dal.Helper.ShopType.Erli:
                        Bll.ErliRESTHelper.Products.Process(schedule, processId);
                        break;
                    case
                        Dal.Helper.ShopType.Clipperon:
                        Bll.ClipperonRestHelper.Products.Process(schedule, processId);
                        break;
                }

            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex,
                    String.Format("Process. shopType: {0}, updateScheduleType {1}", shopType, updateScheduleType));
            }
        }

        public static bool CanUpdateField(bool createProduct, List<Dal.ProductCatalogShopUpdateSchedule> schedules, Dal.Helper.ShopColumnType shopColumnType)
        {
            if (createProduct)
                return true;
            if (schedules.Count == 0)
                return false;

            bool updateAll = schedules.Where(x => x.ShopColumnTypeId == (int)Dal.Helper.ShopColumnType.All).FirstOrDefault() != null;
            bool updateType = schedules.Where(x => x.ShopColumnTypeId == (int)shopColumnType).FirstOrDefault() != null;

            return updateAll || updateType;
        }
        public static string GetUpdateCommandFromShopColumnTypes(List<Dal.ProductCatalogShopUpdateSchedule> schedules)
        {
            string updateCommand = "";

            for (int i = 0; i < 15; i++)
                if (schedules.Where(x => x.ShopColumnTypeId == i).Count() > 0)
                    updateCommand +="1";
                else
                    updateCommand +="0";

            return updateCommand;
        }
    }
}
