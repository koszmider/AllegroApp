using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public class ProductCatalogForAllegroHelper
    {
        private const int dayOffset = 0;

        #region static
        private static int GetAllegroDaysFromNumberOfDays(int days)
        {
            switch (days)
            {
                case 3: return 0;
                case 5: return 1;
                case 7: return 2;
                case 10: return 3;
                case 14: return 4;
                case 30: return 5;
                case 99: return 99;
                default: return 5;
            }
        }
        public static int GetAllegroDaysFromNumberOfDaysRevers(int allegroFieldValue)
        {
            switch (allegroFieldValue)
            {
                case 0: return  3 ;
                case 1: return  5 ;
                case 2: return  7 ;
                case 3: return  10;
                case 4: return  14;
                case 5: return  30;
                case 99: return 99;
                default: return 30;
            }
        }
        /// <summary>
        /// Format sprzedaży (Allegro/sklep)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        //public static ProductCatalogAllegroItemBatchField GetAllegroSellFormBaseOnNumberOfDays(int days)
        //{
        //    ProductCatalogAllegroItemBatchField field = new ProductCatalogAllegroItemBatchField()
        //    {
        //        FieldId = 29,
        //        FieldType = 1,
        //        IntValue = GetAllegroDaysFromNumberOfDays(days) >= 5 ? 1 : 0,
        //        Description = "Format sprzedaży (Allegro/sklep)"
        //    };
        //    return field;
        //}
        /// <summary>
        /// Czas trwania
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        //public static ProductCatalogAllegroItemBatchField GetAllegroSellDays(int days)
        //{
        //    ProductCatalogAllegroItemBatchField field = new ProductCatalogAllegroItemBatchField()
        //    {
        //        FieldId = 4,
        //        FieldType = 1,
        //        IntValue = GetAllegroDaysFromNumberOfDays(days),
        //        Description = "Czas trwania"
        //    };
        //    return field;
        //}
        /// <summary>
        /// Data rozpoczęcia
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        //public static ProductCatalogAllegroItemBatchField GetAllegroStartDate(DateTime date)
        //{
        //    double d = DateTimeToUtc(date);

        //    ProductCatalogAllegroItemBatchField field = new ProductCatalogAllegroItemBatchField()
        //    { 
        //        FieldId = 3,
        //        FieldType = 9,
        //        IntValue = (int)d,
        //        Description = "Data rozpoczęcia"
        //    };
        //    return field;
        //}

        public static long DateTimeToUtc(DateTime date)
        {
            return (long)(date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime UtcToDateTime(long date)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(date).ToLocalTime());
        }

        #endregion
    
    


    }
}
