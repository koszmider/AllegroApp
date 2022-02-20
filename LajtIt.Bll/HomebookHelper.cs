using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
   public class HomebookHelper
    {
         

            [XmlRoot(ElementName = "img")]
            public class Img
            {
                [XmlAttribute(AttributeName = "default")]
                public string Default { get; set; }
                [XmlText]
                public string Text { get; set; }
            }

            [XmlRoot(ElementName = "imgs")]
            public class Imgs
            {
                [XmlElement(ElementName = "img")]
                public List<Img> Img { get; set; }
            }

            [XmlRoot(ElementName = "attr")]
            public class Attr
            {
                [XmlAttribute(AttributeName = "name")]
                public string Name { get; set; }
                [XmlText]
                public string Text { get; set; }
            }

            [XmlRoot(ElementName = "attrs")]
            public class Attrs
            {
                [XmlElement(ElementName = "attr")]
                public List<Attr> Attr { get; set; }
            }

            [XmlRoot(ElementName = "offer")]
            public class Offer
            {
                [XmlElement(ElementName = "id")]
                public string Id { get; set; }
                [XmlElement(ElementName = "url")]
                public string Url { get; set; }
                [XmlElement(ElementName = "price")]
                public string Price { get; set; }
                [XmlElement(ElementName = "oldprice")]
                public string Oldprice { get; set; }
                [XmlElement(ElementName = "brand")]
                public string Brand { get; set; }
                [XmlElement(ElementName = "cat")]
                public string Cat { get; set; }
                [XmlElement(ElementName = "name")]
                public string Name { get; set; }
                [XmlElement(ElementName = "imgs")]
                public Imgs Imgs { get; set; }
                [XmlElement(ElementName = "desc")]
                public string Desc { get; set; }
                [XmlElement(ElementName = "attrs")]
                public Attrs Attrs { get; set; }
            }

            [XmlRoot(ElementName = "offers")]
            public class Offers
            {
                [XmlElement(ElementName = "offer")]
                public List<Offer> Offer { get; set; }
            }


        
    }
}
