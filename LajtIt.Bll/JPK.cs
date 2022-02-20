using System;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace LajtIt.Bll.JPK
{

    [XmlRoot(ElementName = "KodFormularza", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
    public class KodFormularza
    {
        [XmlAttribute(AttributeName = "kodSystemowy")]
        public string KodSystemowy { get; set; }
        [XmlAttribute(AttributeName = "wersjaSchemy")]
        public string WersjaSchemy { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Naglowek", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
    public class Naglowek
    {
        [XmlElement("tns", ElementName = "KodFormularza", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public KodFormularza KodFormularza { get; set; }
        [XmlElement("tns", ElementName = "WariantFormularza", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string WariantFormularza { get; set; }
        [XmlElement("tns", ElementName = "CelZlozenia", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string CelZlozenia { get; set; }
        [XmlElement("tns", ElementName = "DataWytworzeniaJPK", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string DataWytworzeniaJPK { get; set; }
        [XmlElement("tns", ElementName = "DataOd", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string DataOd { get; set; }
        [XmlElement("tns", ElementName = "DataDo", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string DataDo { get; set; }
        [XmlElement("tns", ElementName = "DomyslnyKodWaluty", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string DomyslnyKodWaluty { get; set; }
        [XmlElement("tns", ElementName = "KodUrzedu", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string KodUrzedu { get; set; }
    }

    [XmlRoot(ElementName = "IdentyfikatorPodmiotu", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
    public class IdentyfikatorPodmiotu
    {
        [XmlElement("tns", ElementName = "NIP", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string NIP { get; set; }
        [XmlElement("tns", ElementName = "PelnaNazwa", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string PelnaNazwa { get; set; }
        [XmlElement("tns", ElementName = "REGON", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string REGON { get; set; }
    }

    [XmlRoot(ElementName = "AdresPodmiotu", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
    public class AdresPodmiotu
    {
        [XmlElement("tns", ElementName = "KodKraju", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string KodKraju { get; set; }
        [XmlElement("tns", ElementName = "Wojewodztwo", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string Wojewodztwo { get; set; }
        [XmlElement("tns", ElementName = "Powiat", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string Powiat { get; set; }
        [XmlElement("tns", ElementName = "Gmina", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string Gmina { get; set; }
        [XmlElement("tns", ElementName = "Ulica", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string Ulica { get; set; }
        [XmlElement("tns", ElementName = "NrDomu", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string NrDomu { get; set; }
        [XmlElement("tns", ElementName = "Miejscowosc", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string Miejscowosc { get; set; }
        [XmlElement("tns", ElementName = "KodPocztowy", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string KodPocztowy { get; set; }
        [XmlElement("tns", ElementName = "Poczta", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
        public string Poczta { get; set; }
    }

    [XmlRoot(ElementName = "Podmiot1", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
    public class Podmiot1
    {
        [XmlElement("tns", ElementName = "IdentyfikatorPodmiotu", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public IdentyfikatorPodmiotu IdentyfikatorPodmiotu { get; set; }
        [XmlElement("tns", ElementName = "AdresPodmiotu", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public AdresPodmiotu AdresPodmiotu { get; set; }
    }

    [XmlRoot(ElementName = "Faktura", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
    public class Faktura
    {
        [XmlElement("tns", ElementName = "P_1", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_1 { get; set; }
        [XmlElement("tns", ElementName = "P_2A", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_2A { get; set; }
        [XmlElement("tns", ElementName = "P_3A", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_3A { get; set; }
        [XmlElement("tns", ElementName = "P_3B", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_3B { get; set; }
        [XmlElement("tns", ElementName = "P_3C", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_3C { get; set; }
        [XmlElement("tns", ElementName = "P_3D", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_3D { get; set; }
        [XmlElement("tns", ElementName = "P_4A", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_4A { get; set; }
        [XmlElement("tns", ElementName = "P_4B", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_4B { get; set; }
        [XmlElement("tns", ElementName = "P_5A", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_5A { get; set; }
        [XmlElement("tns", ElementName = "P_6", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_6 { get; set; }
        [XmlElement("tns", ElementName = "P_13_5", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_13_5 { get; set; }
        [XmlElement("tns", ElementName = "P_14_5", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_14_5 { get; set; }
        [XmlElement("tns", ElementName = "P_15", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_15 { get; set; }
        [XmlElement("tns", ElementName = "P_16", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_16 { get; set; }
        [XmlElement("tns", ElementName = "P_17", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_17 { get; set; }
        [XmlElement("tns", ElementName = "P_18", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_18 { get; set; }
        [XmlElement("tns", ElementName = "P_19", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_19 { get; set; }
        [XmlElement("tns", ElementName = "P_20", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_20 { get; set; }
        [XmlElement("tns", ElementName = "P_21", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_21 { get; set; }
        [XmlElement("tns", ElementName = "P_23", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_23 { get; set; }
        [XmlElement("tns", ElementName = "P_106E_2", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_106E_2 { get; set; }
        [XmlElement("tns", ElementName = "P_106E_3", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_106E_3 { get; set; }
        [XmlElement("tns", ElementName = "RodzajFaktury", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string RodzajFaktury { get; set; }
        [XmlAttribute(AttributeName = "typ")]
        public string Typ { get; set; }
        [XmlElement("tns", ElementName = "P_5B", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_5B { get; set; }
        [XmlElement("tns", ElementName = "P_13_1", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_13_1 { get; set; }
        [XmlElement("tns", ElementName = "P_14_1", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_14_1 { get; set; }
    }

    [XmlRoot(ElementName = "FakturaCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
    public class FakturaCtrl
    {
        [XmlElement("tns", ElementName = "LiczbaFaktur", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string LiczbaFaktur { get; set; }
        [XmlElement("tns", ElementName = "WartoscFaktur", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string WartoscFaktur { get; set; }
    }

    [XmlRoot(ElementName = "StawkiPodatku", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
    public class StawkiPodatku
    {
        [XmlElement("tns", ElementName = "Stawka1", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string Stawka1 { get; set; }
        [XmlElement("tns", ElementName = "Stawka2", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string Stawka2 { get; set; }
        [XmlElement("tns", ElementName = "Stawka3", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string Stawka3 { get; set; }
        [XmlElement("tns", ElementName = "Stawka4", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string Stawka4 { get; set; }
        [XmlElement("tns", ElementName = "Stawka5", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string Stawka5 { get; set; }
    }

    [XmlRoot(ElementName = "FakturaWiersz", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
    public class FakturaWiersz
    {
        [XmlElement("tns", ElementName = "P_2B", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_2B { get; set; }
        [XmlElement("tns", ElementName = "P_7", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_7 { get; set; }
        [XmlElement("tns", ElementName = "P_8A", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_8A { get; set; }
        [XmlElement("tns", ElementName = "P_8B", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_8B { get; set; }
        [XmlElement("tns", ElementName = "P_9A", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_9A { get; set; }
        [XmlElement("tns", ElementName = "P_9B", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_9B { get; set; }
        [XmlElement("tns", ElementName = "P_11", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_11 { get; set; }
        [XmlElement("tns", ElementName = "P_11A", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_11A { get; set; }
        [XmlAttribute(AttributeName = "typ")]
        public string Typ { get; set; }
        [XmlElement("tns", ElementName = "P_12", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string P_12 { get; set; }
    }

    [XmlRoot(ElementName = "FakturaWierszCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
    public class FakturaWierszCtrl
    {
        [XmlElement("tns", ElementName = "LiczbaWierszyFaktur", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string LiczbaWierszyFaktur { get; set; }
        [XmlElement("tns", ElementName = "WartoscWierszyFaktur", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public string WartoscWierszyFaktur { get; set; }
    }

    [XmlRoot(ElementName = "JPK", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
    public class JPK
    {
        [XmlElement("tns", ElementName = "Naglowek", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public Naglowek Naglowek { get; set; }
        [XmlElement("tns", ElementName = "Podmiot1", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public Podmiot1 Podmiot1 { get; set; }
        [XmlElement("tns", ElementName = "Faktura", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public List<Faktura> Faktura { get; set; }
        [XmlElement("tns", ElementName = "FakturaCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public FakturaCtrl FakturaCtrl { get; set; }
        [XmlElement("tns", ElementName = "StawkiPodatku", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public StawkiPodatku StawkiPodatku { get; set; }
        [XmlElement("tns", ElementName = "FakturaWiersz", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public List<FakturaWiersz> FakturaWiersz { get; set; }
        [XmlElement("tns", ElementName = "FakturaWierszCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/")]
        public FakturaWierszCtrl FakturaWierszCtrl { get; set; }
        [XmlAttribute(AttributeName = "etd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Etd { get; set; }
        [XmlAttribute(AttributeName = "kck", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Kck { get; set; }
        [XmlAttribute(AttributeName = "tns", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Tns { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

}
