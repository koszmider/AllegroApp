using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Bll
{
    public class PGEHelper
    {

        #region XML
        [XmlRoot(ElementName = "producer")]
        public class Producer
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "category")]
        public class Category
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "description")]
        public class Description
        {
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "short_desc")]
            public string Short_desc { get; set; }
            [XmlElement(ElementName = "long_desc")]
            public string Long_desc { get; set; }
        }

        [XmlRoot(ElementName = "price")]
        public class Price
        {
            [XmlAttribute(AttributeName = "gross")]
            public string Gross { get; set; }
            [XmlAttribute(AttributeName = "net")]
            public string Net { get; set; }
        }

        [XmlRoot(ElementName = "promotion")]
        public class Promotion
        {
            [XmlAttribute(AttributeName = "enable")]
            public string Enable { get; set; }
            [XmlAttribute(AttributeName = "ending_date")]
            public string Ending_date { get; set; }
            [XmlAttribute(AttributeName = "normal_price")]
            public string Normal_price { get; set; }
            [XmlAttribute(AttributeName = "normal_price_net")]
            public string Normal_price_net { get; set; }
        }

        [XmlRoot(ElementName = "stock")]
        public class Stock
        {
            [XmlAttribute(AttributeName = "quantity")]
            public string Quantity { get; set; }
        }

        [XmlRoot(ElementName = "size")]
        public class Size
        {
            [XmlElement(ElementName = "stock")]
            public Stock Stock { get; set; }
        }

        [XmlRoot(ElementName = "sizes")]
        public class Sizes
        {
            [XmlElement(ElementName = "size")]
            public Size Size { get; set; }
        }

        [XmlRoot(ElementName = "image")]
        public class Image
        {
            [XmlAttribute(AttributeName = "url")]
            public string Url { get; set; }
            [XmlAttribute(AttributeName = "display_order")]
            public string Display_order { get; set; }
        }

        [XmlRoot(ElementName = "large")]
        public class Large
        {
            [XmlElement(ElementName = "image")]
            public List<Image> Image { get; set; }
        }

        [XmlRoot(ElementName = "images")]
        public class Images
        {
            [XmlElement(ElementName = "large")]
            public Large Large { get; set; }
        }

        [XmlRoot(ElementName = "value")]
        public class Value
        {
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "parameter")]
        public class Parameter
        {
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "value")]
            public List<Value> Value { get; set; }
        }

        [XmlRoot(ElementName = "parameters")]
        public class Parameters
        {
            [XmlElement(ElementName = "parameter")]
            public List<Parameter> Parameter { get; set; }
        }

        [XmlRoot(ElementName = "product")]
        public class Product
        {
            [XmlElement(ElementName = "producer")]
            public Producer Producer { get; set; }
            [XmlElement(ElementName = "category")]
            public Category Category { get; set; }
            [XmlElement(ElementName = "description")]
            public Description Description { get; set; }
            [XmlElement(ElementName = "price")]
            public Price Price { get; set; }
            [XmlElement(ElementName = "promotion")]
            public Promotion Promotion { get; set; }
            [XmlElement(ElementName = "sizes")]
            public Sizes Sizes { get; set; }
            [XmlElement(ElementName = "images")]
            public Images Images { get; set; }
            [XmlElement(ElementName = "parameters")]
            public Parameters Parameters { get; set; }
            [XmlAttribute(AttributeName = "vat")]
            public string Vat { get; set; }
            [XmlAttribute(AttributeName = "producer_code_standard")]
            public string Producer_code_standard { get; set; }
            [XmlAttribute(AttributeName = "code_on_card")]
            public string Code_on_card { get; set; }
            [XmlAttribute(AttributeName = "display_order")]
            public string Display_order { get; set; }
        }

        [XmlRoot(ElementName = "products")]
        public class Products
        {
            [XmlElement(ElementName = "product")]
            public List<Product> Product { get; set; }
        }

        [XmlRoot(ElementName = "offer")]
        public class Offer
        {
            [XmlElement(ElementName = "products")]
            public Products Products { get; set; }
            [XmlAttribute(AttributeName = "file_format")]
            public string File_format { get; set; }
            [XmlAttribute(AttributeName = "version")]
            public string Version { get; set; }
            [XmlAttribute(AttributeName = "extensions")]
            public string Extensions { get; set; }
            [XmlAttribute(AttributeName = "generated")]
            public string Generated { get; set; }
        }
        #endregion


        //public static void GetOffer()
        //{
        //    Offer offer = new Offer();
        //    offer.Products = GetProducts();
        //}

        //private static Products GetProducts()
        //{ 
        //    List<Dal.ShopFnResult> products = Dal.DbHelper.ProductCatalog.GetProductCatalogForShop((int)Dal.Helper.Shop.PGE);


        //    Products pgeProducts = new Products();




        //    return null;
        //}
    }

}
