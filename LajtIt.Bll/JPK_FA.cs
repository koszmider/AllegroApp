using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace LajtIt.Bll.JPK.FA
{

    [XmlRoot(ElementName = "import", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Import
    {
        [XmlAttribute(AttributeName = "namespace")]
        public string Namespace { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation")]
        public string SchemaLocation { get; set; }
    }

    [XmlRoot(ElementName = "annotation", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Annotation
    {
        [XmlElement(ElementName = "documentation", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public string Documentation { get; set; }
    }

    [XmlRoot(ElementName = "enumeration", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Enumeration
    {
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
        [XmlElement(ElementName = "annotation", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Annotation Annotation { get; set; }
    }

    [XmlRoot(ElementName = "restriction", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Restriction
    {
        [XmlElement(ElementName = "enumeration", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public List<Enumeration> Enumeration { get; set; }
        [XmlAttribute(AttributeName = "base")]
        public string Base { get; set; }
        [XmlElement(ElementName = "totalDigits", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public TotalDigits TotalDigits { get; set; }
        [XmlElement(ElementName = "fractionDigits", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public FractionDigits FractionDigits { get; set; }
        [XmlElement(ElementName = "minExclusive", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public MinExclusive MinExclusive { get; set; }
        [XmlElement(ElementName = "minLength", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public MinLength MinLength { get; set; }
        [XmlElement(ElementName = "maxLength", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public MaxLength MaxLength { get; set; }
    }

    [XmlRoot(ElementName = "simpleType", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class SimpleType
    {
        [XmlElement(ElementName = "annotation", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Annotation Annotation { get; set; }
        [XmlElement(ElementName = "restriction", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Restriction Restriction { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "attribute", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Attribute
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "use")]
        public string Use { get; set; }
        [XmlAttribute(AttributeName = "fixed")]
        public string Fixed { get; set; }
    }

    [XmlRoot(ElementName = "extension", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Extension
    {
        [XmlElement(ElementName = "attribute", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public List<Attribute> Attribute { get; set; }
        [XmlAttribute(AttributeName = "base")]
        public string Base { get; set; }
    }

    [XmlRoot(ElementName = "simpleContent", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class SimpleContent
    {
        [XmlElement(ElementName = "extension", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Extension Extension { get; set; }
    }

    [XmlRoot(ElementName = "complexType", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class ComplexType
    {
        [XmlElement(ElementName = "simpleContent", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public SimpleContent SimpleContent { get; set; }
        [XmlElement(ElementName = "sequence", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Sequence Sequence { get; set; }
    }

    [XmlRoot(ElementName = "element", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Element
    {
        [XmlElement(ElementName = "complexType", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public ComplexType ComplexType { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "simpleType", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public SimpleType SimpleType { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "annotation", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Annotation Annotation { get; set; }
    }

    [XmlRoot(ElementName = "sequence", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Sequence
    {
        [XmlElement(ElementName = "element", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public List<Element> Element { get; set; }
    }

    [XmlRoot(ElementName = "totalDigits", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class TotalDigits
    {
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "fractionDigits", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class FractionDigits
    {
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "minExclusive", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class MinExclusive
    {
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "minLength", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class MinLength
    {
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "maxLength", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class MaxLength
    {
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "complexContent", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class ComplexContent
    {
        [XmlElement(ElementName = "extension", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Extension Extension { get; set; }
    }

    [XmlRoot(ElementName = "schema", Namespace = "http://www.w3.org/2001/XMLSchema")]
    public class Schema
    {
        [XmlElement(ElementName = "import", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public List<Import> Import { get; set; }
        [XmlElement(ElementName = "simpleType", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public List<SimpleType> SimpleType { get; set; }
        [XmlElement(ElementName = "complexType", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public ComplexType ComplexType { get; set; }
        [XmlElement(ElementName = "element", Namespace = "http://www.w3.org/2001/XMLSchema")]
        public Element Element { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "etd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Etd { get; set; }
        [XmlAttribute(AttributeName = "kck", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Kck { get; set; }
        [XmlAttribute(AttributeName = "tns", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Tns { get; set; }
        [XmlAttribute(AttributeName = "targetNamespace")]
        public string TargetNamespace { get; set; }
        [XmlAttribute(AttributeName = "elementFormDefault")]
        public string ElementFormDefault { get; set; }
        [XmlAttribute(AttributeName = "attributeFormDefault")]
        public string AttributeFormDefault { get; set; }
        [XmlAttribute(AttributeName = "lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string Lang { get; set; }
    }

}

