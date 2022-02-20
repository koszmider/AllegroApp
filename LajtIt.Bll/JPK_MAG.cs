using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll.JPK
{
    public class JPK_MAG
    {
		[XmlRoot(ElementName = "KodFormularza", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class KodFormularza
		{
			[XmlAttribute(AttributeName = "kodSystemowy")]
			public string KodSystemowy { get; set; }
			[XmlAttribute(AttributeName = "wersjaSchemy")]
			public string WersjaSchemy { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "Naglowek", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class Naglowek
		{
			[XmlElement(ElementName = "KodFormularza", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public KodFormularza KodFormularza { get; set; }
			[XmlElement(ElementName = "WariantFormularza", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string WariantFormularza { get; set; }
			[XmlElement(ElementName = "CelZlozenia", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string CelZlozenia { get; set; }
			[XmlElement(ElementName = "DataWytworzeniaJPK", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataWytworzeniaJPK { get; set; }
			[XmlElement(ElementName = "DataOd", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataOd { get; set; }
			[XmlElement(ElementName = "DataDo", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataDo { get; set; }
			[XmlElement(ElementName = "DomyslnyKodWaluty", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DomyslnyKodWaluty { get; set; }
			[XmlElement(ElementName = "KodUrzedu", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string KodUrzedu { get; set; }
		}

		[XmlRoot(ElementName = "IdentyfikatorPodmiotu", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class IdentyfikatorPodmiotu
		{
			[XmlElement(ElementName = "NIP", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
			public string NIP { get; set; }
			[XmlElement(ElementName = "PelnaNazwa", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
			public string PelnaNazwa { get; set; }
		}

		[XmlRoot(ElementName = "AdresPodmiotu", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class AdresPodmiotu
		{
			[XmlElement(ElementName = "KodKraju", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
			public string KodKraju { get; set; }
			[XmlElement(ElementName = "Wojewodztwo", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
			public string Wojewodztwo { get; set; }
			[XmlElement(ElementName = "Powiat", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
			public string Powiat { get; set; }
			[XmlElement(ElementName = "Gmina", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
			public string Gmina { get; set; }
			[XmlElement(ElementName = "Ulica", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
			public string Ulica { get; set; }
			[XmlElement(ElementName = "NrDomu", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
			public string NrDomu { get; set; }
			[XmlElement(ElementName = "Miejscowosc", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
			public string Miejscowosc { get; set; }
			[XmlElement(ElementName = "KodPocztowy", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
			public string KodPocztowy { get; set; }
			[XmlElement(ElementName = "Poczta", Namespace = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/")]
			public string Poczta { get; set; }
		}

		[XmlRoot(ElementName = "Podmiot1", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class Podmiot1
		{
			[XmlElement(ElementName = "IdentyfikatorPodmiotu", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public IdentyfikatorPodmiotu IdentyfikatorPodmiotu { get; set; }
			[XmlElement(ElementName = "AdresPodmiotu", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public AdresPodmiotu AdresPodmiotu { get; set; }
		}

		[XmlRoot(ElementName = "PZWartosc", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class PZWartosc
		{
			[XmlElement(ElementName = "NumerPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string NumerPZ { get; set; }
			[XmlElement(ElementName = "DataPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataPZ { get; set; }
			[XmlElement(ElementName = "WartoscPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string WartoscPZ { get; set; }
			[XmlElement(ElementName = "DataOtrzymaniaPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataOtrzymaniaPZ { get; set; }
			[XmlElement(ElementName = "Dostawca", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string Dostawca { get; set; }
			[XmlElement(ElementName = "DataFaPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataFaPZ { get; set; }
		}

		[XmlRoot(ElementName = "PZWiersz", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class PZWiersz
		{
			[XmlElement(ElementName = "Numer2PZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string Numer2PZ { get; set; }
			[XmlElement(ElementName = "KodTowaruPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string KodTowaruPZ { get; set; }
			[XmlElement(ElementName = "NazwaTowaruPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string NazwaTowaruPZ { get; set; }
			[XmlElement(ElementName = "IloscPrzyjetaPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string IloscPrzyjetaPZ { get; set; }
			[XmlElement(ElementName = "JednostkaMiaryPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string JednostkaMiaryPZ { get; set; }
			[XmlElement(ElementName = "CenaJednPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string CenaJednPZ { get; set; }
			[XmlElement(ElementName = "WartoscPozycjiPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string WartoscPozycjiPZ { get; set; }
		}

		[XmlRoot(ElementName = "PZCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class PZCtrl
		{
			[XmlElement(ElementName = "LiczbaPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string LiczbaPZ { get; set; }
			[XmlElement(ElementName = "SumaPZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string SumaPZ { get; set; }
		}

		[XmlRoot(ElementName = "PZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class PZ
		{
			[XmlElement(ElementName = "PZWartosc", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public List<PZWartosc> PZWartosc { get; set; }
			[XmlElement(ElementName = "PZWiersz", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public List<PZWiersz> PZWiersz { get; set; }
			[XmlElement(ElementName = "PZCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public PZCtrl PZCtrl { get; set; }
		}

		[XmlRoot(ElementName = "WZWartosc", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class WZWartosc
		{
			[XmlElement(ElementName = "NumerWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string NumerWZ { get; set; }
			[XmlElement(ElementName = "DataWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataWZ { get; set; }
			[XmlElement(ElementName = "WartoscWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string WartoscWZ { get; set; }
			[XmlElement(ElementName = "DataWydaniaWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataWydaniaWZ { get; set; }
			[XmlElement(ElementName = "OdbiorcaWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string OdbiorcaWZ { get; set; }
			[XmlElement(ElementName = "NumerFaWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string NumerFaWZ { get; set; }
			[XmlElement(ElementName = "DataFaWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataFaWZ { get; set; }
		}

		[XmlRoot(ElementName = "WZWiersz", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class WZWiersz
		{
			[XmlElement(ElementName = "Numer2WZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string Numer2WZ { get; set; }
			[XmlElement(ElementName = "NazwaTowaruWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string NazwaTowaruWZ { get; set; }
			[XmlElement(ElementName = "IloscWydanaWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string IloscWydanaWZ { get; set; }
			[XmlElement(ElementName = "JednostkaMiaryWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string JednostkaMiaryWZ { get; set; }
			[XmlElement(ElementName = "CenaJednWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string CenaJednWZ { get; set; }
			[XmlElement(ElementName = "WartoscPozycjiWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string WartoscPozycjiWZ { get; set; }
		}

		[XmlRoot(ElementName = "WZCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class WZCtrl
		{
			[XmlElement(ElementName = "LiczbaWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string LiczbaWZ { get; set; }
			[XmlElement(ElementName = "SumaWZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string SumaWZ { get; set; }
		}

		[XmlRoot(ElementName = "WZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class WZ
		{
			[XmlElement(ElementName = "WZWartosc", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public List<WZWartosc> WZWartosc { get; set; }
			[XmlElement(ElementName = "WZWiersz", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public WZWiersz WZWiersz { get; set; }
			[XmlElement(ElementName = "WZCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public WZCtrl WZCtrl { get; set; }
		}

		[XmlRoot(ElementName = "RWWartosc", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class RWWartosc
		{
			[XmlElement(ElementName = "NumerRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string NumerRW { get; set; }
			[XmlElement(ElementName = "DataRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataRW { get; set; }
			[XmlElement(ElementName = "WartoscRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string WartoscRW { get; set; }
			[XmlElement(ElementName = "DataWydaniaRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataWydaniaRW { get; set; }
			[XmlElement(ElementName = "SkadRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string SkadRW { get; set; }
			[XmlElement(ElementName = "DokadRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DokadRW { get; set; }
		}

		[XmlRoot(ElementName = "RWWiersz", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class RWWiersz
		{
			[XmlElement(ElementName = "Numer2RW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string Numer2RW { get; set; }
			[XmlElement(ElementName = "NazwaTowaruRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string NazwaTowaruRW { get; set; }
			[XmlElement(ElementName = "IloscWydanaRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string IloscWydanaRW { get; set; }
			[XmlElement(ElementName = "JednostkaMiaryRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string JednostkaMiaryRW { get; set; }
			[XmlElement(ElementName = "CenaJednRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string CenaJednRW { get; set; }
			[XmlElement(ElementName = "WartoscPozycjiRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string WartoscPozycjiRW { get; set; }
			[XmlElement(ElementName = "KodTowaruRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string KodTowaruRW { get; set; }
		}

		[XmlRoot(ElementName = "RWCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class RWCtrl
		{
			[XmlElement(ElementName = "LiczbaRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string LiczbaRW { get; set; }
			[XmlElement(ElementName = "SumaRW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string SumaRW { get; set; }
		}

		[XmlRoot(ElementName = "RW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class RW
		{
			[XmlElement(ElementName = "RWWartosc", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public List<RWWartosc> RWWartosc { get; set; }
			[XmlElement(ElementName = "RWWiersz", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public List<RWWiersz> RWWiersz { get; set; }
			[XmlElement(ElementName = "RWCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public RWCtrl RWCtrl { get; set; }
		}

		[XmlRoot(ElementName = "MMWartosc", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class MMWartosc
		{
			[XmlElement(ElementName = "NumerMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string NumerMM { get; set; }
			[XmlElement(ElementName = "DataMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataMM { get; set; }
			[XmlElement(ElementName = "WartoscMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string WartoscMM { get; set; }
			[XmlElement(ElementName = "DataWydaniaMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string DataWydaniaMM { get; set; }
		}

		[XmlRoot(ElementName = "MMWiersz", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class MMWiersz
		{
			[XmlElement(ElementName = "Numer2MM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string Numer2MM { get; set; }
			[XmlElement(ElementName = "NazwaTowaruMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string NazwaTowaruMM { get; set; }
			[XmlElement(ElementName = "IloscWydanaMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string IloscWydanaMM { get; set; }
			[XmlElement(ElementName = "JednostkaMiaryMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string JednostkaMiaryMM { get; set; }
			[XmlElement(ElementName = "CenaJednMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string CenaJednMM { get; set; }
			[XmlElement(ElementName = "WartoscPozycjiMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string WartoscPozycjiMM { get; set; }
			[XmlElement(ElementName = "KodTowaruMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string KodTowaruMM { get; set; }
		}

		[XmlRoot(ElementName = "MMCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class MMCtrl
		{
			[XmlElement(ElementName = "LiczbaMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string LiczbaMM { get; set; }
			[XmlElement(ElementName = "SumaMM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string SumaMM { get; set; }
		}

		[XmlRoot(ElementName = "MM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class MM
		{
			[XmlElement(ElementName = "MMWartosc", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public MMWartosc MMWartosc { get; set; }
			[XmlElement(ElementName = "MMWiersz", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public List<MMWiersz> MMWiersz { get; set; }
			[XmlElement(ElementName = "MMCtrl", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public MMCtrl MMCtrl { get; set; }
		}

		[XmlRoot(ElementName = "JPK", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
		public class JPK
		{
			[XmlElement(ElementName = "Naglowek", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public Naglowek Naglowek { get; set; }
			[XmlElement(ElementName = "Podmiot1", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public Podmiot1 Podmiot1 { get; set; }
			[XmlElement(ElementName = "Magazyn", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public string Magazyn { get; set; }
			[XmlElement(ElementName = "PZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public PZ PZ { get; set; }
			[XmlElement(ElementName = "WZ", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public WZ WZ { get; set; }
			[XmlElement(ElementName = "RW", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public RW RW { get; set; }
			[XmlElement(ElementName = "MM", Namespace = "http://jpk.mf.gov.pl/wzor/2016/03/09/03093/")]
			public MM MM { get; set; }
			[XmlAttribute(AttributeName = "xmlns")]
			public string Xmlns { get; set; }
			[XmlAttribute(AttributeName = "tns", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Tns { get; set; }
			[XmlAttribute(AttributeName = "kck", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Kck { get; set; }
			[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
			public string SchemaLocation { get; set; }
			[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Xsi { get; set; }
		}

	}
}
