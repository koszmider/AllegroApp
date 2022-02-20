using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Bll
{
    class GoogleProducts
    {

        // UWAGA: Wygenerowany kod może wymagać co najmniej programu .NET Framework 4.5 lub .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
        public partial class feed
        {

            private string titleField;

            private feedLink linkField;

            private System.DateTime updatedField;

            private feedEntry[] entryField;

            /// <remarks/>
            public string title
            {
                get
                {
                    return this.titleField;
                }
                set
                {
                    this.titleField = value;
                }
            }

            /// <remarks/>
            public feedLink link
            {
                get
                {
                    return this.linkField;
                }
                set
                {
                    this.linkField = value;
                }
            }

            /// <remarks/>
            public System.DateTime updated
            {
                get
                {
                    return this.updatedField;
                }
                set
                {
                    this.updatedField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("entry")]
            public feedEntry[] entry
            {
                get
                {
                    return this.entryField;
                }
                set
                {
                    this.entryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public partial class feedLink
        {

            private string relField;

            private string typeField;

            private string hrefField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string rel
            {
                get
                {
                    return this.relField;
                }
                set
                {
                    this.relField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string href
            {
                get
                {
                    return this.hrefField;
                }
                set
                {
                    this.hrefField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public partial class feedEntry
        {

            private ushort idField;

            private string titleField;

            private string descriptionField;

            private string product_typeField;

            private string linkField;

            private string image_linkField;

            private string conditionField;

            private string availabilityField;

            private string priceField;

            private string sale_priceField;

            private string sale_price_effective_dateField;

            private string brandField;

            private string gtinField;

            private string mpnField;

            private string shipping_weightField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public ushort id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            public string title
            {
                get
                {
                    return this.titleField;
                }
                set
                {
                    this.titleField = value;
                }
            }

            /// <remarks/>
            public string description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public string product_type
            {
                get
                {
                    return this.product_typeField;
                }
                set
                {
                    this.product_typeField = value;
                }
            }

            /// <remarks/>
            public string link
            {
                get
                {
                    return this.linkField;
                }
                set
                {
                    this.linkField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public string image_link
            {
                get
                {
                    return this.image_linkField;
                }
                set
                {
                    this.image_linkField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public string condition
            {
                get
                {
                    return this.conditionField;
                }
                set
                {
                    this.conditionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public string availability
            {
                get
                {
                    return this.availabilityField;
                }
                set
                {
                    this.availabilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public string price
            {
                get
                {
                    return this.priceField;
                }
                set
                {
                    this.priceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public string sale_price
            {
                get
                {
                    return this.sale_priceField;
                }
                set
                {
                    this.sale_priceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public string sale_price_effective_date
            {
                get
                {
                    return this.sale_price_effective_dateField;
                }
                set
                {
                    this.sale_price_effective_dateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public string brand
            {
                get
                {
                    return this.brandField;
                }
                set
                {
                    this.brandField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public string gtin
            {
                get
                {
                    return this.gtinField;
                }
                set
                {
                    this.gtinField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public string mpn
            {
                get
                {
                    return this.mpnField;
                }
                set
                {
                    this.mpnField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://base.google.com/ns/1.0")]
            public string shipping_weight
            {
                get
                {
                    return this.shipping_weightField;
                }
                set
                {
                    this.shipping_weightField = value;
                }
            }
        }


    }
}
