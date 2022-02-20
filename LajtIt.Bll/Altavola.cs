
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Lajtit.Bll.Altavola
{
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

    [XmlRoot(ElementName = "series")]
    public class Series
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "card")]
    public class Card
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
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

    [XmlRoot(ElementName = "delivery_time", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
    public class Delivery_time
    {
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }
    }

    [XmlRoot(ElementName = "price")]
    public class Price
    {
        [XmlAttribute(AttributeName = "vat")]
        public string Vat { get; set; }
        [XmlAttribute(AttributeName = "net")]
        public string Net { get; set; }
        [XmlAttribute(AttributeName = "gross")]
        public string Gross { get; set; }
    }

    [XmlRoot(ElementName = "price", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
    public class Price2
    {
        [XmlAttribute(AttributeName = "vat")]
        public string Vat { get; set; }
        [XmlAttribute(AttributeName = "net")]
        public string Net { get; set; }
        [XmlAttribute(AttributeName = "gross")]
        public string Gross { get; set; }
    }

    [XmlRoot(ElementName = "stock", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
    public class Stock
    {
        [XmlAttribute(AttributeName = "quantity")]
        public string Quantity { get; set; }
    }

    [XmlRoot(ElementName = "size", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
    public class Size
    {
        [XmlElement(ElementName = "price", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public Price2 Price2 { get; set; }
        [XmlElement(ElementName = "stock", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public List<Stock> Stock { get; set; }
        [XmlAttribute(AttributeName = "weight")]
        public string Weight { get; set; }
        [XmlAttribute(AttributeName = "code_producer")]
        public string Code_producer { get; set; }
        [XmlAttribute(AttributeName = "available")]
        public string Available { get; set; }
    }

    [XmlRoot(ElementName = "sizes", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
    public class Sizes
    {
        [XmlElement(ElementName = "size", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public Size Size { get; set; }
    }

    [XmlRoot(ElementName = "image")]
    public class Image
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
    }

    [XmlRoot(ElementName = "large")]
    public class Large
    {
        [XmlElement(ElementName = "image")]
        public List<Image> Image { get; set; }
    }

    [XmlRoot(ElementName = "icon")]
    public class Icon
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
    }

    [XmlRoot(ElementName = "icons")]
    public class Icons
    {
        [XmlElement(ElementName = "icon")]
        public Icon Icon { get; set; }
    }

    [XmlRoot(ElementName = "images")]
    public class Images
    {
        [XmlElement(ElementName = "large")]
        public Large Large { get; set; }
        [XmlElement(ElementName = "icons")]
        public Icons Icons { get; set; }
    }

    [XmlRoot(ElementName = "value")]
    public class Value
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "parameter")]
    public class Parameter
    {
        [XmlElement(ElementName = "value")]
        public List<Value> Value { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
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
        [XmlElement(ElementName = "series")]
        public Series Series { get; set; }
        [XmlElement(ElementName = "card")]
        public Card Card { get; set; }
        [XmlElement(ElementName = "description")]
        public Description Description { get; set; }
        [XmlElement(ElementName = "delivery_time", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public Delivery_time Delivery_time { get; set; }
        [XmlElement(ElementName = "price")]
        public Price Price { get; set; }
        [XmlElement(ElementName = "sizes", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public Sizes Sizes { get; set; }
        [XmlElement(ElementName = "images")]
        public Images Images { get; set; }
        [XmlElement(ElementName = "parameters")]
        public Parameters Parameters { get; set; }
        [XmlAttribute(AttributeName = "producer_code_standard", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public string Producer_code_standard { get; set; }
        [XmlAttribute(AttributeName = "code_on_card", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public string Code_on_card { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
        [XmlAttribute(AttributeName = "code_producer")]
        public string Code_producer { get; set; }
    }

    [XmlRoot(ElementName = "product_bundle", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
    public class Product_bundle
    {
        [XmlElement(ElementName = "producer")]
        public Producer Producer { get; set; }
        [XmlElement(ElementName = "category")]
        public Category Category { get; set; }
        [XmlElement(ElementName = "series")]
        public Series Series { get; set; }
        [XmlElement(ElementName = "card")]
        public Card Card { get; set; }
        [XmlElement(ElementName = "description")]
        public Description Description { get; set; }
        [XmlElement(ElementName = "delivery_time", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public Delivery_time Delivery_time { get; set; }
        [XmlElement(ElementName = "price")]
        public Price Price { get; set; }
        [XmlElement(ElementName = "sizes", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public Sizes Sizes { get; set; }
        [XmlElement(ElementName = "images")]
        public Images Images { get; set; }
        [XmlElement(ElementName = "parameters")]
        public Parameters Parameters { get; set; }
        [XmlAttribute(AttributeName = "producer_code_standard", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public string Producer_code_standard { get; set; }
        [XmlAttribute(AttributeName = "code_on_card", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public string Code_on_card { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
        [XmlAttribute(AttributeName = "code_producer")]
        public string Code_producer { get; set; }
    }

    [XmlRoot(ElementName = "product_collection", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
    public class Product_collection
    {
        [XmlElement(ElementName = "producer")]
        public Producer Producer { get; set; }
        [XmlElement(ElementName = "category")]
        public Category Category { get; set; }
        [XmlElement(ElementName = "card")]
        public Card Card { get; set; }
        [XmlElement(ElementName = "description")]
        public Description Description { get; set; }
        [XmlElement(ElementName = "delivery_time", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public Delivery_time Delivery_time { get; set; }
        [XmlElement(ElementName = "price")]
        public Price Price { get; set; }
        [XmlElement(ElementName = "sizes", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public Sizes Sizes { get; set; }
        [XmlElement(ElementName = "images")]
        public Images Images { get; set; }
        [XmlElement(ElementName = "parameters")]
        public Parameters Parameters { get; set; }
        [XmlAttribute(AttributeName = "producer_code_standard", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public string Producer_code_standard { get; set; }
        [XmlAttribute(AttributeName = "code_on_card", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public string Code_on_card { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
        [XmlAttribute(AttributeName = "code_producer")]
        public string Code_producer { get; set; }
    }

    [XmlRoot(ElementName = "products")]
    public class Products
    {
        [XmlElement(ElementName = "product")]
        public List<Product> Product { get; set; }
        [XmlElement(ElementName = "product_bundle", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public List<Product_bundle> Product_bundle { get; set; }
        [XmlElement(ElementName = "product_collection", Namespace = "http://www.iai-shop.com/developers/iof/extensions.phtml")]
        public Product_collection Product_collection { get; set; }
    }

    [XmlRoot(ElementName = "offer")]
    public class Offer
    {
        [XmlElement(ElementName = "products")]
        public Products Products { get; set; }
        [XmlAttribute(AttributeName = "iaiext", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Iaiext { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string NoNamespaceSchemaLocation { get; set; }
    }

}
