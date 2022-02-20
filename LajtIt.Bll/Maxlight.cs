using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll.Maxlight
{

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class offers
    {

        private offersO[] oField;

        private string versionField;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("o", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public offersO[] o
        {
            get
            {
                return this.oField;
            }
            set
            {
                this.oField = value;
            }
        }

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class offersO
    {

        private string catField;

        private string nameField;

        private string descField;

        private offersOImgsMain[][] imgsField;

        private offersOAttrsA[][] attrsField;

        private string idField;

        private string urlField;

        private string priceField;

        private string availField;

        private string setField;

        private string stockField;

        private string weightField;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cat
        {
            get
            {
                return this.catField;
            }
            set
            {
                this.catField = value;
            }
        }

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string desc
        {
            get
            {
                return this.descField;
            }
            set
            {
                this.descField = value;
            }
        }

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("main", typeof(offersOImgsMain), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public offersOImgsMain[][] imgs
        {
            get
            {
                return this.imgsField;
            }
            set
            {
                this.imgsField = value;
            }
        }

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("a", typeof(offersOAttrsA), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public offersOAttrsA[][] attrs
        {
            get
            {
                return this.attrsField;
            }
            set
            {
                this.attrsField = value;
            }
        }

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
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

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string avail
        {
            get
            {
                return this.availField;
            }
            set
            {
                this.availField = value;
            }
        }

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string set
        {
            get
            {
                return this.setField;
            }
            set
            {
                this.setField = value;
            }
        }

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string stock
        {
            get
            {
                return this.stockField;
            }
            set
            {
                this.stockField = value;
            }
        }

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class offersOImgsMain
    {

        private string urlField;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class offersOAttrsA
    {

        private string nameField;

        private string valueField;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <uwagi/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class NewDataSet
    {

        private offers[] itemsField;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("offers")]
        public offers[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

}

