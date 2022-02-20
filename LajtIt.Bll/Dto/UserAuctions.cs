using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Bll.Dto
{
    public class UserAuctions
    { 

        public long ItemId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal BuyNowPrice { get; set; }
        public int BidCount { get; set; }
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
        public int ItemsOrdered { get; set; }
        public decimal ItemsValue { get; set; }
        public long? HitCount { get; set; }
        public long? EndingInfo { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime? EndingDateTime { get; set; }
        public bool IsPromoted { get; set; } 
    }
}
