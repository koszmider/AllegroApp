using LajtIt.Dal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace LajtIt.Bll
{
    public class NovitusHelper
    {
		  static string ip = "192.168.001.111";
		  static int port = 6001;
		#region Classes
		[XmlRoot(ElementName = "rabat")]
		public class Rabat
		{
			[XmlAttribute(AttributeName = "wartosc")]
			public string Wartosc { get; set; }
			[XmlAttribute(AttributeName = "nazwa")]
			public string Nazwa { get; set; }
			[XmlAttribute(AttributeName = "identyfikator_rabatu")]
			public string Identyfikator_rabatu { get; set; }
			[XmlAttribute(AttributeName = "akcja")]
			public string Akcja { get; set; }
		}

		[XmlRoot(ElementName = "paragon")]
		public class Paragon
		{
			[XmlAttribute(AttributeName = "akcja")]
			public string Akcja { get; set; }
			[XmlAttribute(AttributeName = "tryb")]
			public string Tryb { get; set; }
			[XmlAttribute(AttributeName = "numer_systemowy")]
			public string Numer_systemowy { get; set; }
			[XmlAttribute(AttributeName = "kwota")]
			public string Kwota { get; set; }
		}

		[XmlRoot(ElementName = "zaliczka")]
		public class Zaliczka
		{
			[XmlAttribute(AttributeName = "akcja")]
			public string Akcja { get; set; }
			[XmlAttribute(AttributeName = "opis")]
			public string Opis { get; set; }
			[XmlAttribute(AttributeName = "stawka")]
			public string Stawka { get; set; }
			[XmlAttribute(AttributeName = "wartosc")]
			public string Wartosc { get; set; }
			[XmlAttribute(AttributeName = "doplata")]
			public string Doplata { get; set; }
		}
		[XmlRoot(ElementName = "pozycja")]
		public class Pozycja
		{
			[XmlAttribute(AttributeName = "nazwa")]
			public string Nazwa { get; set; }
			[XmlAttribute(AttributeName = "ilosc")]
			public string Ilosc { get; set; }
			[XmlAttribute(AttributeName = "jednostka")]
			public string Jednostka { get; set; }
			[XmlAttribute(AttributeName = "stawka")]
			public string Stawka { get; set; }
			[XmlAttribute(AttributeName = "cena")]
			public string Cena { get; set; }
			[XmlAttribute(AttributeName = "kod_towaru")]
			public string Kod_towaru { get; set; }
			[XmlAttribute(AttributeName = "recepta")]
			public string Recepta { get; set; }
			[XmlAttribute(AttributeName = "oplata")]
			public string Oplata { get; set; }
			[XmlAttribute(AttributeName = "plu")]
			public string Plu { get; set; }
			[XmlAttribute(AttributeName = "opis")]
			public string Opis { get; set; }
			[XmlAttribute(AttributeName = "akcja")]
			public string Akcja { get; set; }
			[XmlElement(ElementName = "rabat")]
			public Rabat Rabat { get; set; }
		}



        [XmlRoot(ElementName = "platnosc")]
		public class Platnosc
		{
			[XmlAttribute(AttributeName = "typ")]
			public string Typ { get; set; }
			[XmlAttribute(AttributeName = "akcja")]
			public string Akcja { get; set; }
			[XmlAttribute(AttributeName = "wartosc")]
			public string Wartosc { get; set; }
		}

		[XmlRoot(ElementName = "pakiet")]
		public class Pakiet
		{
			[XmlElement(ElementName = "paragon")]
			public Paragon Paragon { get; set; }
			[XmlElement(ElementName = "pozycja")]
			public List<Pozycja> Pozycja { get; set; }
			[XmlElement(ElementName = "zaliczka")]
			public List<Zaliczka> Zaliczka { get; set; }
			[XmlElement(ElementName = "platnosc")]
			public Platnosc Platnosc { get; set; }

			[XmlElement(ElementName = "paragon")]
			public Paragon ParagonKoniec { get; set; }
		}
		#endregion

		public static string GetReceiptXml(int receiptId)
		{
			Dal.OrderReceipt receipt = Dal.DbHelper.Orders.GetReceipt(receiptId);
			List<Dal.OrderReceiptProduct> receiptProducts = Dal.DbHelper.Orders.GetReceiptProducts(receiptId);

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("<pakiet>");
			sb.AppendLine(GetXml<Paragon>(GetParagon(true, receipt)));

		 

			if (receipt.ReceiptTypeId == 2)
				sb.AppendLine(GetXmlL<List<Zaliczka>>(GetZaliczka(receipt, receiptProducts)));
			else
				sb.AppendLine(GetPozycja(receipt, receiptProducts));



			sb.AppendLine(GetXmlL<List<Platnosc>>(GetPlatnosc(receipt)));

			sb.AppendLine(GetXml<Paragon>(GetParagon(false, receipt)));

			sb.AppendLine("</pakiet>");

			string s = sb.ToString();
			s = s.Replace("<ArrayOfZaliczka>", "");
			s = s.Replace("</ArrayOfZaliczka>", "");
			s = s.Replace("<ArrayOfPozycja>", "");
			s = s.Replace("</ArrayOfPozycja>", "");
			s = s.Replace("<ArrayOfPlatnosc>", "");
			s = s.Replace("</ArrayOfPlatnosc>", "");
			s = s.Replace("<Pozycja", "<pozycja");
			s = s.Replace("<Platnosc", "<platnosc");
			s = s.Replace("<Zaliczka", "<zaliczka");
			return s;
		}

		private static string GetXmlL<T>(T o)
		{
			XmlRootAttribute root = new XmlRootAttribute("");

			XmlSerializer xmlserializer = new XmlSerializer(typeof(T), root);
			CeneoHelper.Utf8StringWriter stringWriter = new CeneoHelper.Utf8StringWriter();

			var settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.OmitXmlDeclaration = true;

			XmlWriter writer = XmlWriter.Create(stringWriter, settings);
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", "");

			xmlserializer.Serialize(writer, o, ns);

			string serializeXml = stringWriter.ToString();

			writer.Close();

			return serializeXml;
		}
		private static string GetXml<T>(T o)
        {

			XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
			CeneoHelper.Utf8StringWriter stringWriter = new CeneoHelper.Utf8StringWriter();

			var settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.OmitXmlDeclaration = true;
			 
			XmlWriter writer = XmlWriter.Create(stringWriter, settings);
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", "");

			xmlserializer.Serialize(writer, o, ns);

			string serializeXml = stringWriter.ToString();

			writer.Close();

			return serializeXml;
		}

        private static List<Zaliczka> GetZaliczka(OrderReceipt receipt, List<OrderReceiptProduct> receiptProducts )
        {
			List<Zaliczka> zaliczka = new List<Zaliczka>();
	 
			Dal.Order order = Dal.DbHelper.Orders.GetOrder(receipt.OrderId);

			decimal rate = receipt.Amount / order.AmountToPay;
			int i = 0;
			decimal totalZaliczka = 0;

			foreach (Dal.OrderReceiptProduct product in receiptProducts)
			{
				i++;
				decimal total = product.Price * product.Quantity;
				decimal totalPrepayment = Math.Round(total * rate,2);
				decimal totalToPay = total - totalPrepayment;

				totalZaliczka += totalPrepayment;

				decimal diff = 0;
				if (i == receiptProducts.Count())
					diff = receipt.Amount - totalZaliczka;

				Zaliczka p = new Zaliczka()
				{
					Akcja = "zaliczka",
					Wartosc = (totalPrepayment+ diff).ToString(new System.Globalization.CultureInfo("en-US")),
					//Ilosc = product.Quantity.ToString(),
					//Jednostka = "szt",
					//Kod_towaru = GetProductName(product),
					//Nazwa = product.Name,
					Opis = product.Name,
					Stawka = "A",
					Doplata= Math.Round(totalToPay- diff, 2).ToString(new System.Globalization.CultureInfo("en-US"))
				};

	 
				zaliczka.Add(p);

			}
			return zaliczka;
		}

        private static List<Platnosc> GetPlatnosc(OrderReceipt receipt)
        {
			List<Platnosc> platnosci = new List<Platnosc>();
			List<Dal.OrderPayment> orderPayments = Dal.DbHelper.Orders.GetOrderPayments(receipt.OrderId)
				.Where(x => x.Amount > 0)
				.ToList() ;

			foreach(Dal.OrderPayment payment in orderPayments)
			{
				
				Platnosc platnosc = new Platnosc()
				{
					Akcja = "dodaj",
					Typ = "",
					Wartosc = payment.Amount.ToString(new System.Globalization.CultureInfo("en-US")),
				};

				platnosci.Add(platnosc);
			}
			return platnosci;
		}

        private static string GetPozycja(OrderReceipt receipt, List<OrderReceiptProduct> receiptProducts)
        {
			StringBuilder pozycja = new StringBuilder();

			Dal.Order order = Dal.DbHelper.Orders.GetOrder(receipt.OrderId);

			List<Dal.OrderReceipt> prePayments = Dal.DbHelper.Orders.GetReceipts(receipt.OrderId).Where(x => x.ReceiptTypeId == 2).ToList();


			decimal totalPrePayments = 0;
			decimal rate = prePayments.Sum(x => x.Amount) / order.AmountToPay;

			decimal i = 0;
			foreach (Dal.OrderReceiptProduct product in receiptProducts)
            {
				i++;
				Pozycja p = new Pozycja()
				{
					Akcja = "sprzedaz",
					Cena = product.Price.ToString(new System.Globalization.CultureInfo("en-US")),
					Ilosc = product.Quantity.ToString(),
					Jednostka = "szt",
					Kod_towaru = GetProductName( product),
					Nazwa = product.Name,
					Opis = "",
					Stawka = "A"
				};


				if (product.Rebate > 0)
				{
					p.Rabat = new Rabat()
					{
						Akcja = "rabat",
						Identyfikator_rabatu = "1",
						Wartosc = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:0.00}%", product.Rebate), 

					};

				}
				pozycja.AppendLine(GetXml<Pozycja>(p));


				if(receipt.ReceiptTypeId ==3)
                {
					decimal priceTotal = Math.Round(product.Quantity * product.Price * (1 - product.Rebate / 100),2);
					decimal prePayment = Math.Round(priceTotal* rate,2);
					totalPrePayments += prePayment;
					decimal diff = 0;
					if( i == receiptProducts.Count())
                    {
						diff = receipt.Amount - totalPrePayments;
                    }						
					Zaliczka zaliczka = new Zaliczka()
					{
						Akcja = "rozliczenie_zaliczki",
						Opis = product.Name,
						Stawka = "A",
						Wartosc = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:0.00}", prePayment),  //wartosc zaliczki
						Doplata = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:0.00}", priceTotal - prePayment)  // doplata
					};
					pozycja.AppendLine(GetXml<Zaliczka>(zaliczka));
				}

			}
			return pozycja.ToString();
		}

        private static string GetProductName(OrderReceiptProduct product)
        {
			if (product.OrderProduct != null)
				return product.OrderProduct.ProductCatalog.Code;
			else
				return "";
        }

		public static bool SetCommand(string xml, ref string msg)
		{
			xml = @"<pakiet><informacja akcja='wersja' wersja='' data=''></informacja></pakiet>";

			try
			{
				using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
				{
					Byte[] bytesSent = Encoding.ASCII.GetBytes(xml);
					s.SendTimeout = 2000;
					s.Connect(ip, port);
					// Send request to the server.
					s.Send(bytesSent, bytesSent.Length, 0);
					Byte[] bytesReceived = new Byte[256];
					// Receive the server home page content.
					int bytes = 0;
					string page = "";

					// The following will block until the page is transmitted.
					while(s.Available>0)
					{
						bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
						page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
					}
					 
					msg = page;

					return true;
				}
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				return false;
			}

		}

		public static bool IsConnected()
        {
			string xml = @"<pakiet><dle_pl/></pakiet>";
			string msg = "";
			bool result = SetCommand(xml, ref msg);

			return result;

		}
        private static Paragon GetParagon(bool start, Dal.OrderReceipt receipt)
        {
			if (start)
				return new Paragon()
				{
					Akcja = "poczatek",
					Tryb = "online"
				};
			else
				return new Paragon()
				{
					Akcja = "zamknij",
					Numer_systemowy = String.Format("Zam:{0}, Par: {1}", receipt.OrderId, receipt.ReceiptId),
					Kwota = receipt.Amount.ToString(new System.Globalization.CultureInfo("en-US"))
				};
		}
    }
}
