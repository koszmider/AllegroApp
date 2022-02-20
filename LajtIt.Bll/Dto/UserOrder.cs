using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Bll.Dto
{
    public class UserOrder
    {
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemsOrdered { get; set; }
        public decimal ItemPrice { get; set; }
        public string OrderStatusName { get; set; }
        public DateTime? OrderDateTime { get; set; }
    }
}
