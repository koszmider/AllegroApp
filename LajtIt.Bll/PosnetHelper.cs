using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class PosnetHelper
    {
 
        [XmlRoot(ElementName = "item")]
        public class Item
        {
            [XmlElement(ElementName = "id_unique")]
            public string Id_unique { get; set; }
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "KodKreskowy")]
            public string KodKreskowy { get; set; }
            [XmlElement(ElementName = "minMagazyn")]
            public string MinMagazyn { get; set; }
            [XmlElement(ElementName = "typ")]
            public string Typ { get; set; }
            [XmlElement(ElementName = "ptu")]
            public string Ptu { get; set; }
            [XmlElement(ElementName = "price")]
            public string Price { get; set; }
            [XmlElement(ElementName = "packNr")]
            public string PackNr { get; set; }
            [XmlElement(ElementName = "jmNr")]
            public string JmNr { get; set; }
            [XmlElement(ElementName = "plu_formatIlosci")]
            public string Plu_formatIlosci { get; set; }
            [XmlElement(ElementName = "constPrice")]
            public string ConstPrice { get; set; }
            [XmlElement(ElementName = "rabat")]
            public string Rabat { get; set; }
            [XmlElement(ElementName = "groupNR")]
            public string GroupNR { get; set; }
            [XmlElement(ElementName = "rabat_flaga")]
            public string Rabat_flaga { get; set; }
            [XmlElement(ElementName = "wielopakIlosc")]
            public string WielopakIlosc { get; set; }
            [XmlElement(ElementName = "PLUNr")]
            public string PLUNr { get; set; }
            [XmlElement(ElementName = "notatnik")]
            public string Notatnik { get; set; }
        }

        [XmlRoot(ElementName = "PLUcodes")]
        public class PLUcodes
        {
            [XmlElement(ElementName = "item")]
            public List<Item> Item { get; set; }
        }

        [XmlRoot(ElementName = "POSClientData")]
        public class POSClientData
        {
            [XmlElement(ElementName = "PLUcodes")]
            public PLUcodes PLUcodes { get; set; }
            [XmlElement(ElementName = "PLUEAN")]
            public string PLUEAN { get; set; }
            [XmlElement(ElementName = "PLUZestaw")]
            public string PLUZestaw { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

    }

}
