using LajtIt.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class ShopRestHelper
    {
        public class OrdersProducts
        {
            public class OrderProduct
            {
                public int id { get; set; }
                public int order_id { get; set; }
                public int product_id { get; set; }
                public int stock_id { get; set; }
                public decimal price { get; set; }
                public decimal discount_perc { get; set; }
                public int quantity { get; set; }
                public string delivery_time { get; set; }
                public string name { get; set; }
                public string code { get; set; }
                public string pkwiu { get; set; }
                public string tax { get; set; }
                public decimal tax_value { get; set; }
                public string unit { get; set; }
                public string option { get; set; }
                public string unit_fp { get; set; }
                public string weight { get; set; }
                public string type { get; set; }
                public object loyalty { get; set; }
                public string delivery_time_hours { get; set; }
                public List<object> text_options { get; set; }
                public List<object> file_options { get; set; }
            }

            public class RootOrderProduct
            {
                public string count { get; set; }
                public int pages { get; set; }
                public int page { get; set; }
                public List<OrderProduct> list { get; set; }
            }
            public static RootOrderProduct GetOrderProducts(Dal.Helper.Shop shop, int shopOrderId)
            {
                try
                {
                    HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, @"/webapi/rest/order-products?limit=50&filters={""order_id"":{""="":""" + shopOrderId + @"""}}", "GET");

                    string text = GetTextFromWebResponse(request);

                    var json_serializer = new JavaScriptSerializer();

                    RootOrderProduct orderProducts = json_serializer.Deserialize<RootOrderProduct>(text);


                    return orderProducts;
                }

                catch (WebException ex)
                {
                    ProcessException(ex, shop);
                    return null;

                }
            }
        }
    }
}