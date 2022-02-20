using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace LajtIt.Bll.JPK.VAT
{
    public class JPK_VAT
    {



		[XmlRoot(ElementName = "KodFormularza", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class KodFormularza
		{
			[XmlAttribute(AttributeName = "kodSystemowy")]
			public string KodSystemowy { get; set; }
			[XmlAttribute(AttributeName = "wersjaSchemy")]
			public string WersjaSchemy { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "CelZlozenia", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class CelZlozenia
		{
			[XmlAttribute(AttributeName = "poz")]
			public string Poz { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "Naglowek", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class Naglowek
		{
			[XmlElement(ElementName = "KodFormularza", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public KodFormularza KodFormularza { get; set; }
			[XmlElement(ElementName = "WariantFormularza", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string WariantFormularza { get; set; }
			[XmlElement(ElementName = "DataWytworzeniaJPK", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string DataWytworzeniaJPK { get; set; }
			[XmlElement(ElementName = "NazwaSystemu", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string NazwaSystemu { get; set; }
			[XmlElement(ElementName = "CelZlozenia", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public CelZlozenia CelZlozenia { get; set; }
			[XmlElement(ElementName = "KodUrzedu", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string KodUrzedu { get; set; }
			[XmlElement(ElementName = "Rok", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string Rok { get; set; }
			[XmlElement(ElementName = "Miesiac", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string Miesiac { get; set; }
			[XmlElement(ElementName = "KodFormularzaDekl", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public KodFormularzaDekl KodFormularzaDekl { get; set; }
			[XmlElement(ElementName = "WariantFormularzaDekl", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string WariantFormularzaDekl { get; set; }
		}

		[XmlRoot(ElementName = "OsobaNiefizyczna", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class OsobaNiefizyczna
		{
			[XmlElement(ElementName = "NIP", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string NIP { get; set; }
			[XmlElement(ElementName = "PelnaNazwa", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string PelnaNazwa { get; set; }
			[XmlElement(ElementName = "Email", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string Email { get; set; }
			[XmlElement(ElementName = "Telefon", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string Telefon { get; set; }
		}

		[XmlRoot(ElementName = "Podmiot1", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class Podmiot1
		{
			[XmlElement(ElementName = "OsobaNiefizyczna", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public OsobaNiefizyczna OsobaNiefizyczna { get; set; }
			[XmlAttribute(AttributeName = "rola")]
			public string Rola { get; set; }
		}

		[XmlRoot(ElementName = "KodFormularzaDekl", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class KodFormularzaDekl
		{
			[XmlAttribute(AttributeName = "kodSystemowy")]
			public string KodSystemowy { get; set; }
			[XmlAttribute(AttributeName = "kodPodatku")]
			public string KodPodatku { get; set; }
			[XmlAttribute(AttributeName = "rodzajZobowiazania")]
			public string RodzajZobowiazania { get; set; }
			[XmlAttribute(AttributeName = "wersjaSchemy")]
			public string WersjaSchemy { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "PozycjeSzczegolowe", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class PozycjeSzczegolowe
		{
			[XmlElement(ElementName = "P_10", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_10 { get; set; }
			[XmlElement(ElementName = "P_11", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_11 { get; set; }
			[XmlElement(ElementName = "P_12", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_12 { get; set; }
			[XmlElement(ElementName = "P_13", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_13 { get; set; }
			[XmlElement(ElementName = "P_14", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_14 { get; set; }
			[XmlElement(ElementName = "P_15", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_15 { get; set; }
			[XmlElement(ElementName = "P_16", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_16 { get; set; }
			[XmlElement(ElementName = "P_17", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_17 { get; set; }
			[XmlElement(ElementName = "P_18", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_18 { get; set; }
			[XmlElement(ElementName = "P_19", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_19 { get; set; }
			[XmlElement(ElementName = "P_20", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_20 { get; set; }
			[XmlElement(ElementName = "P_21", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_21 { get; set; }
			[XmlElement(ElementName = "P_22", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_22 { get; set; }
			[XmlElement(ElementName = "P_23", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_23 { get; set; }
			[XmlElement(ElementName = "P_24", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_24 { get; set; }
			[XmlElement(ElementName = "P_25", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_25 { get; set; }
			[XmlElement(ElementName = "P_26", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_26 { get; set; }
			[XmlElement(ElementName = "P_27", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_27 { get; set; }
			[XmlElement(ElementName = "P_28", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_28 { get; set; }
			[XmlElement(ElementName = "P_29", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_29 { get; set; }
			[XmlElement(ElementName = "P_30", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_30 { get; set; }
			[XmlElement(ElementName = "P_31", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_31 { get; set; }
			[XmlElement(ElementName = "P_32", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_32 { get; set; }
			[XmlElement(ElementName = "P_33", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_33 { get; set; }
			[XmlElement(ElementName = "P_34", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_34 { get; set; }
			[XmlElement(ElementName = "P_35", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_35 { get; set; }
			[XmlElement(ElementName = "P_36", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_36 { get; set; }
			[XmlElement(ElementName = "P_37", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_37 { get; set; }
			[XmlElement(ElementName = "P_38", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_38 { get; set; }
			[XmlElement(ElementName = "P_39", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_39 { get; set; }
			[XmlElement(ElementName = "P_40", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_40 { get; set; }
			[XmlElement(ElementName = "P_41", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_41 { get; set; }
			[XmlElement(ElementName = "P_42", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_42 { get; set; }
			[XmlElement(ElementName = "P_43", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_43 { get; set; }
			[XmlElement(ElementName = "P_44", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_44 { get; set; }
			[XmlElement(ElementName = "P_45", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_45 { get; set; }
			[XmlElement(ElementName = "P_46", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_46 { get; set; }
			[XmlElement(ElementName = "P_47", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_47 { get; set; }
			[XmlElement(ElementName = "P_48", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_48 { get; set; }
			[XmlElement(ElementName = "P_49", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_49 { get; set; }
			[XmlElement(ElementName = "P_50", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_50 { get; set; }
			[XmlElement(ElementName = "P_51", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_51 { get; set; }
			[XmlElement(ElementName = "P_52", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_52 { get; set; }
			[XmlElement(ElementName = "P_53", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_53 { get; set; }
			[XmlElement(ElementName = "P_54", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_54 { get; set; }
			[XmlElement(ElementName = "P_55", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_55 { get; set; }
			[XmlElement(ElementName = "P_56", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_56 { get; set; }
			[XmlElement(ElementName = "P_57", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_57 { get; set; }
			[XmlElement(ElementName = "P_58", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_58 { get; set; }
			[XmlElement(ElementName = "P_59", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_59 { get; set; }
			[XmlElement(ElementName = "P_60", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_60 { get; set; }
			[XmlElement(ElementName = "P_61", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_61 { get; set; }
			[XmlElement(ElementName = "P_62", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_62 { get; set; }
			[XmlElement(ElementName = "P_63", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_63 { get; set; }
			[XmlElement(ElementName = "P_64", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_64 { get; set; }
			[XmlElement(ElementName = "P_65", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_65 { get; set; }
			[XmlElement(ElementName = "P_66", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_66 { get; set; }
			[XmlElement(ElementName = "P_67", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_67 { get; set; }
			[XmlElement(ElementName = "P_68", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_68 { get; set; }
			[XmlElement(ElementName = "P_69", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_69 { get; set; }
			[XmlElement(ElementName = "P_ORDZU", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string P_ORDZU { get; set; }
		}

		[XmlRoot(ElementName = "Deklaracja", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class Deklaracja
		{
			[XmlElement(ElementName = "Naglowek", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public Naglowek Naglowek { get; set; }
			[XmlElement(ElementName = "PozycjeSzczegolowe", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public PozycjeSzczegolowe PozycjeSzczegolowe { get; set; }
			[XmlElement(ElementName = "Pouczenia", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string Pouczenia { get; set; }
		}

		[XmlRoot(ElementName = "SprzedazWiersz", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class SprzedazWiersz
		{
			[XmlElement(ElementName = "LpSprzedazy", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string LpSprzedazy { get; set; }
			[XmlElement(ElementName = "KodKrajuNadaniaTIN", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string KodKrajuNadaniaTIN { get; set; }
			[XmlElement(ElementName = "NrKontrahenta", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string NrKontrahenta { get; set; }
			[XmlElement(ElementName = "NazwaKontrahenta", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string NazwaKontrahenta { get; set; }
			[XmlElement(ElementName = "DowodSprzedazy", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string DowodSprzedazy { get; set; }
			[XmlElement(ElementName = "DataWystawienia", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string DataWystawienia { get; set; }
			[XmlElement(ElementName = "DataSprzedazy", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string DataSprzedazy { get; set; }
			[XmlElement(ElementName = "TypDokumentu", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string TypDokumentu { get; set; }
			[XmlElement(ElementName = "GTU_01", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_01 { get; set; }
			[XmlElement(ElementName = "GTU_02", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_02 { get; set; }
			[XmlElement(ElementName = "GTU_03", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_03 { get; set; }
			[XmlElement(ElementName = "GTU_04", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_04 { get; set; }
			[XmlElement(ElementName = "GTU_05", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_05 { get; set; }
			[XmlElement(ElementName = "GTU_06", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_06 { get; set; }
			[XmlElement(ElementName = "GTU_07", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_07 { get; set; }
			[XmlElement(ElementName = "GTU_08", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_08 { get; set; }
			[XmlElement(ElementName = "GTU_09", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_09 { get; set; }
			[XmlElement(ElementName = "GTU_10", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_10 { get; set; }
			[XmlElement(ElementName = "GTU_11", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_11 { get; set; }
			[XmlElement(ElementName = "GTU_12", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_12 { get; set; }
			[XmlElement(ElementName = "GTU_13", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string GTU_13 { get; set; }
			[XmlElement(ElementName = "SW", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string SW { get; set; }
			[XmlElement(ElementName = "EE", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string EE { get; set; }
			[XmlElement(ElementName = "TP", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string TP { get; set; }
			[XmlElement(ElementName = "TT_WNT", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string TT_WNT { get; set; }
			[XmlElement(ElementName = "TT_D", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string TT_D { get; set; }
			[XmlElement(ElementName = "MR_T", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string MR_T { get; set; }
			[XmlElement(ElementName = "MR_UZ", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string MR_UZ { get; set; }
			[XmlElement(ElementName = "I_42", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string I_42 { get; set; }
			[XmlElement(ElementName = "I_63", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string I_63 { get; set; }
			[XmlElement(ElementName = "B_SPV", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string B_SPV { get; set; }
			[XmlElement(ElementName = "B_SPV_DOSTAWA", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string B_SPV_DOSTAWA { get; set; }
			[XmlElement(ElementName = "B_MPV_PROWIZJA", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string B_MPV_PROWIZJA { get; set; }
			[XmlElement(ElementName = "MPP", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string MPP { get; set; }
			[XmlElement(ElementName = "KorektaPodstawyOpodt", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string KorektaPodstawyOpodt { get; set; }
			[XmlElement(ElementName = "K_10", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_10 { get; set; }
			[XmlElement(ElementName = "K_11", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_11 { get; set; }
			[XmlElement(ElementName = "K_12", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_12 { get; set; }
			[XmlElement(ElementName = "K_13", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_13 { get; set; }
			[XmlElement(ElementName = "K_14", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_14 { get; set; }
			[XmlElement(ElementName = "K_15", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_15 { get; set; }
			[XmlElement(ElementName = "K_16", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_16 { get; set; }
			[XmlElement(ElementName = "K_17", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_17 { get; set; }
			[XmlElement(ElementName = "K_18", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_18 { get; set; }
			[XmlElement(ElementName = "K_19", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_19 { get; set; }
			[XmlElement(ElementName = "K_20", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_20 { get; set; }
			[XmlElement(ElementName = "K_21", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_21 { get; set; }
			[XmlElement(ElementName = "K_22", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_22 { get; set; }
			[XmlElement(ElementName = "K_23", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_23 { get; set; }
			[XmlElement(ElementName = "K_24", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_24 { get; set; }
			[XmlElement(ElementName = "K_25", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_25 { get; set; }
			[XmlElement(ElementName = "K_26", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_26 { get; set; }
			[XmlElement(ElementName = "K_27", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_27 { get; set; }
			[XmlElement(ElementName = "K_28", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_28 { get; set; }
			[XmlElement(ElementName = "K_29", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_29 { get; set; }
			[XmlElement(ElementName = "K_30", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_30 { get; set; }
			[XmlElement(ElementName = "K_31", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_31 { get; set; }
			[XmlElement(ElementName = "K_32", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_32 { get; set; }
			[XmlElement(ElementName = "K_33", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_33 { get; set; }
			[XmlElement(ElementName = "K_34", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_34 { get; set; }
			[XmlElement(ElementName = "K_35", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_35 { get; set; }
			[XmlElement(ElementName = "K_36", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_36 { get; set; }
			[XmlElement(ElementName = "SprzedazVAT_Marza", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string SprzedazVAT_Marza { get; set; }
		}

		[XmlRoot(ElementName = "SprzedazCtrl", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class SprzedazCtrl
		{
			[XmlElement(ElementName = "LiczbaWierszySprzedazy", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string LiczbaWierszySprzedazy { get; set; }
			[XmlElement(ElementName = "PodatekNalezny", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string PodatekNalezny { get; set; }
		}

		[XmlRoot(ElementName = "ZakupWiersz", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class ZakupWiersz
		{
			[XmlElement(ElementName = "LpZakupu", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string LpZakupu { get; set; }
			[XmlElement(ElementName = "KodKrajuNadaniaTIN", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string KodKrajuNadaniaTIN { get; set; }
			[XmlElement(ElementName = "NrDostawcy", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string NrDostawcy { get; set; }
			[XmlElement(ElementName = "NazwaDostawcy", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string NazwaDostawcy { get; set; }
			[XmlElement(ElementName = "DowodZakupu", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string DowodZakupu { get; set; }
			[XmlElement(ElementName = "DataZakupu", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string DataZakupu { get; set; }
			[XmlElement(ElementName = "DataWplywu", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string DataWplywu { get; set; }
			[XmlElement(ElementName = "DokumentZakupu", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string DokumentZakupu { get; set; }
			[XmlElement(ElementName = "MPP", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string MPP { get; set; }
			[XmlElement(ElementName = "IMP", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string IMP { get; set; }
			[XmlElement(ElementName = "K_40", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_40 { get; set; }
			[XmlElement(ElementName = "K_41", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_41 { get; set; }
			[XmlElement(ElementName = "K_42", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_42 { get; set; }
			[XmlElement(ElementName = "K_43", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_43 { get; set; }
			[XmlElement(ElementName = "K_44", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_44 { get; set; }
			[XmlElement(ElementName = "K_45", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_45 { get; set; }
			[XmlElement(ElementName = "K_46", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_46 { get; set; }
			[XmlElement(ElementName = "K_47", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string K_47 { get; set; }
			[XmlElement(ElementName = "ZakupVAT_Marza", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string ZakupVAT_Marza { get; set; }
		}

		[XmlRoot(ElementName = "ZakupCtrl", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class ZakupCtrl
		{
			[XmlElement(ElementName = "LiczbaWierszyZakupow", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string LiczbaWierszyZakupow { get; set; }
			[XmlElement(ElementName = "PodatekNaliczony", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public string PodatekNaliczony { get; set; }
		}

		[XmlRoot(ElementName = "Ewidencja", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class Ewidencja
		{
			[XmlElement(ElementName = "SprzedazWiersz", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public SprzedazWiersz SprzedazWiersz { get; set; }
			[XmlElement(ElementName = "SprzedazCtrl", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public SprzedazCtrl SprzedazCtrl { get; set; }
			[XmlElement(ElementName = "ZakupWiersz", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public ZakupWiersz ZakupWiersz { get; set; }
			[XmlElement(ElementName = "ZakupCtrl", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public ZakupCtrl ZakupCtrl { get; set; }
		}

		[XmlRoot(ElementName = "JPK", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
		public class JPK
		{
			[XmlElement(ElementName = "Naglowek", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public Naglowek Naglowek { get; set; }
			[XmlElement(ElementName = "Podmiot1", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public Podmiot1 Podmiot1 { get; set; }
			[XmlElement(ElementName = "Deklaracja", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public Deklaracja Deklaracja { get; set; }
			[XmlElement(ElementName = "Ewidencja", Namespace = "http://crd.gov.pl/wzor/2020/05/08/9393/")]
			public Ewidencja Ewidencja { get; set; }
			[XmlAttribute(AttributeName = "etd", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Etd { get; set; }
			[XmlAttribute(AttributeName = "tns", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Tns { get; set; }
			[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Xsi { get; set; }
		}



	}

}
