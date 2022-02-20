using CsvHelper;
using CsvHelper.Configuration;
using LajtIt.Dal;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    public partial class EmpikRESTHelper
    {
        public partial class Offers
        {

            #region Classes
            public class CSV
            {
                public string sku { get; set; }
                [Display(Name = "product-id")]
                public string productid { get; set; }
                [Display(Name = "product-id-type")]
                public string productidtype { get; set; }
                public string description { get; set; }
                [Display(Name = "internal-description")]
                public string internaldescription { get; set; }
                public string price { get; set; }
                [Display(Name = "price-additional-info")]
                public string priceadditionalinfo { get; set; }
                public int quantity { get; set; }
                [Display(Name = "min-quantity-alert")]
                public string minquantityalert { get; set; }
                public string state { get; set; }
                [Display(Name = "available-start-date")]
                public string availablestartdate { get; set; }
                [Display(Name = "available-end-date")]
                public string availableenddate { get; set; }
                [Display(Name = "discount-price")]
                public string discountprice { get; set; }
                [Display(Name = "discount-start-date")]
                public string discountstartdate { get; set; }
                [Display(Name = "discount-end-date")]
                public string discountenddate { get; set; }
                [Display(Name = "discount-ranges")]
                public string discountranges { get; set; }
                [Display(Name = "allow-quote-requests")]
                public string allowquoterequests { get; set; }
                [Display(Name = "leadtime-to-ship")]
                public int leadtimetoship { get; set; }
                [Display(Name = "min-order-quantity")]
                public string minorderquantity { get; set; }
                [Display(Name = "max-order-quantity")]
                public string maxorderquantity { get; set; }
                [Display(Name = "package-quantity")]
                public string packagequantity { get; set; }
                [Display(Name = "update-delete")]
                public string updatedelete { get; set; }
                [Display(Name = "price-ranges")]
                public string priceranges { get; set; }
            }
            #endregion

            public class AutoClassMapWithApplyDisplayNameAttribute<T> : ClassMap<T>
            {
                public AutoClassMapWithApplyDisplayNameAttribute()
                {
                    AutoMap();

                    foreach (var memberMap in MemberMaps)
                    {
                        var displayAttribute = memberMap.Data.Member.GetCustomAttribute<DisplayAttribute>();
                        if (displayAttribute != null)
                        {
                            memberMap.Data.Names.Clear();
                            memberMap.Data.Names.Add(displayAttribute.Name);
                            memberMap.Data.IsNameSet = true;
                        }
                    }
                }
            }
            public static string GetFile(Dal.Helper.Shop shop)
            {
                string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

                Dal.ShopHelper sh = new Dal.ShopHelper();
                Dal.Shop s = Dal.DbHelper.Shop.GetShop((int)shop);

                string saveLocation = String.Format(path, s.ExportFileName);


                List<CSV> offers = new List<CSV>();



                List<Dal.ProductCatalogForShopFnResult> products = sh.ProductCatalogForShopForEmpik(s.ShopId); 
                   // .Where(x => x.Ean != null && x.ShopProductId != null).ToList();

                offers.AddRange(
                    products.Select(x => new CSV()
                    {
                        productidtype = "EAN",
                        productid = x.Ean,
                        sku = x.Code,
                        price = String.Format("{0:#.00}", x.PriceBruttoMinimum),
                        quantity = ShopUpdateHelper.ClickShop.GetStock(x.SupplierQuantity, x.LeftQuantity.Value, x.IsAvailable, x.IsDiscontinued),
                        leadtimetoship = GetAvail(x),
                        updatedelete = "",
                        state = "11",
                        description = x.ShopProductName
                    }).ToList()
                    );
                using (StreamWriter writer = new StreamWriter(saveLocation))
                {


                    using (CsvHelper.CsvWriter csv = new CsvHelper.CsvWriter(writer))
                    {


                        csv.Configuration.RegisterClassMap<AutoClassMapWithApplyDisplayNameAttribute<CSV>>();
                        csv.Configuration.Delimiter = ";";
                        csv.Configuration.HasHeaderRecord = true;
                        csv.Configuration.CultureInfo = CultureInfo.GetCultureInfo("en-US");
                        
                        csv.WriteRecords(offers);
                    } 

                }
                //return saveLocation;

               //return  File.ReadAllText(saveLocation);
                return saveLocation;


            }

            private static int GetAvail(ProductCatalogForShopFnResult x)
            {
                if (x.IsOnStock.Value)
                    return 1;
                else
                    return x.DeliveryId;
            }
        }
    }

}
