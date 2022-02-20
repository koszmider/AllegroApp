using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class FbHelper
    {
 
	[XmlRoot(ElementName="link", Namespace="http://www.w3.org/2005/Atom")]
	public class Link {
		[XmlAttribute(AttributeName="rel")]
		public string Rel { get; set; }
		[XmlAttribute(AttributeName="type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName="href")]
		public string Href { get; set; }
	}

	[XmlRoot(ElementName="entry", Namespace="http://www.w3.org/2005/Atom")]
	public class Entry {
		[XmlElement(ElementName="id", Namespace="http://www.w3.org/2005/Atom")]
		public string Id { get; set; }
		[XmlElement(ElementName="title", Namespace="http://www.w3.org/2005/Atom")]
		public string Title { get; set; }
		[XmlElement(ElementName="description", Namespace="http://www.w3.org/2005/Atom")]
		public string Description { get; set; }
		[XmlElement(ElementName="product_type", Namespace="http://www.w3.org/2005/Atom")]
		public string Product_type { get; set; }
		[XmlElement(ElementName="link", Namespace="http://www.w3.org/2005/Atom")]
		public string Link { get; set; }
		[XmlElement(ElementName="condition", Namespace="http://www.w3.org/2005/Atom")]
		public string Condition { get; set; }
		[XmlElement(ElementName="availability", Namespace="http://www.w3.org/2005/Atom")]
		public string Availability { get; set; }
		[XmlElement(ElementName="inventory", Namespace="http://www.w3.org/2005/Atom")]
		public string Inventory { get; set; }
		[XmlElement(ElementName="price", Namespace="http://www.w3.org/2005/Atom")]
		public string Price { get; set; }
		[XmlElement(ElementName="sale_price", Namespace="http://www.w3.org/2005/Atom")]
		public string Sale_price { get; set; }
		[XmlElement(ElementName="sale_price_effective_date", Namespace="http://www.w3.org/2005/Atom")]
		public string Sale_price_effective_date { get; set; }
		[XmlElement(ElementName="brand", Namespace="http://www.w3.org/2005/Atom")]
		public string Brand { get; set; }
		[XmlElement(ElementName="gtin", Namespace="http://www.w3.org/2005/Atom")]
		public string Gtin { get; set; }
		[XmlElement(ElementName="mpn", Namespace="http://www.w3.org/2005/Atom")]
		public string Mpn { get; set; }
		[XmlElement(ElementName="shipping_weight", Namespace="http://www.w3.org/2005/Atom")]
		public string Shipping_weight { get; set; }
		[XmlElement(ElementName="image_link", Namespace="http://www.w3.org/2005/Atom")]
		public string Image_link { get; set; }
		[XmlElement(ElementName="additional_image_link", Namespace="http://www.w3.org/2005/Atom")]
		public List<string> Additional_image_link { get; set; }
	}

	[XmlRoot(ElementName="feed", Namespace="http://www.w3.org/2005/Atom")]
	public class Feed {
		[XmlElement(ElementName="title", Namespace="http://www.w3.org/2005/Atom")]
		public string Title { get; set; }
		[XmlElement(ElementName="link", Namespace="http://www.w3.org/2005/Atom")]
		public Link Link { get; set; }
		[XmlElement(ElementName="updated", Namespace="http://www.w3.org/2005/Atom")]
		public string Updated { get; set; }
		[XmlElement(ElementName="entry", Namespace="http://www.w3.org/2005/Atom")]
		public List<Entry> Entry { get; set; }
		[XmlAttribute(AttributeName="xmlns")]
		public string Xmlns { get; set; }
		[XmlAttribute(AttributeName="g", Namespace="http://www.w3.org/2000/xmlns/")]
		public string G { get; set; }
	}

	}

}
