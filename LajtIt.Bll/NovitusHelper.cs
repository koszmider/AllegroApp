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
			[XmlElement(ElementName = "rabat")]
			public Rabat Rabat { get; set; }
			[XmlAttribute(AttributeName = "kasjer")]
			public string Kasjer { get; set; }
			[XmlAttribute(AttributeName = "nip")]
			public string Nip { get; set; }
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
			//sb.AppendLine(String.Format("<kasjer akcja='zalogowany' numer='1' nazwa='{0}'></kasjer>", receipt.InsertUser));
			sb.AppendLine(GetXml<Paragon>(GetParagon(true, receipt)));


			switch (receipt.ReceiptTypeId)
			{
				case 1:
				case 3:
					sb.AppendLine(GetPozycja(receipt, receiptProducts)); break;
				case 2:
					sb.AppendLine(GetXml<Zaliczka>(GetZaliczka(receipt, receiptProducts)));
					
					break;
			}
			
			sb.AppendLine(GetXmlL<List<Platnosc>>(GetPlatnosc(receipt)));
			sb.AppendLine(GetXml<Paragon>(GetParagon(false, receipt)));




			if(receipt.ReceiptTypeId==2)
				sb.AppendLine(GetOrderDetails(receipt));

			//sb.AppendLine(String.Format("<kasjer akcja='wylogowany' numer='1' nazwa='{0}'></kasjer>", receipt.InsertUser));
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

		private static Zaliczka GetZaliczka(OrderReceipt receipt, List<OrderReceiptProduct> receiptProducts)
		{

			Dal.Order order = Dal.DbHelper.Orders.GetOrder(receipt.OrderId);

			List<Dal.OrderReceipt> prePayments = Dal.DbHelper.Orders.GetReceipts(receipt.OrderId)
				.Where(x => x.ReceiptTypeId == 2)
				.ToList();


			decimal prePaymentsSum = prePayments.Sum(x => x.Amount);


			Zaliczka zaliczka = new Zaliczka()
			{
				Akcja = "zaliczka",
				Wartosc = receipt.Amount.ToString(new System.Globalization.CultureInfo("en-US")),
				Opis = String.Format("Zamówienie: {0}", receipt.OrderId),
				Stawka = "A",
				Doplata = Math.Round(order.AmountToPay- prePaymentsSum, 2).ToString(new System.Globalization.CultureInfo("en-US"))
			};


			return zaliczka;
		}
		 

		private static List<Platnosc> GetPlatnosc(OrderReceipt receipt)
        {
			List<Platnosc> platnosci = new List<Platnosc>();

			List<Dal.OrderPayment> orderPayments = Dal.DbHelper.Orders.GetOrderPayments(receipt.OrderId)
			.Where(x => x.Amount > 0 && x.OrderPaymentType.CashRegisterActive == true)
			.ToList();

			decimal total = 0;
			if (receipt.ReceiptTypeId == 3)
			{
				// rozliczenie paragonu
		
				total = orderPayments.Where(x => x.Amount > 0).Sum(x => x.Amount);
				foreach (Dal.OrderPayment payment in orderPayments)
				{
					Platnosc p = new Platnosc()
					{
						Akcja = "dodaj",
						Typ = payment.OrderPaymentType.CashRegisterType,
						//Wartosc = payment.Amount.ToString(new System.Globalization.CultureInfo("en-US")),
						Wartosc = payment.Amount.ToString(new System.Globalization.CultureInfo("en-US")),
					};

					platnosci.Add(p);
				}
			}
			else
			{
				total = receipt.Amount;

				string typ = "";
				Dal.OrderPayment op = orderPayments.OrderByDescending(x => x.PaymentId).FirstOrDefault();

				if (op != null && receipt.Amount == op.Amount)
					typ = op.OrderPaymentType.CashRegisterType;

				Platnosc platnosc = new Platnosc()
				{
					Akcja = "dodaj",
					Typ = typ,
					Wartosc = total.ToString(new System.Globalization.CultureInfo("en-US")),
				};
				platnosci.Add(platnosc);
			}
			//}
			return platnosci;
		}

        private static string GetPozycja(OrderReceipt receipt, List<OrderReceiptProduct> receiptProducts)
        {
			StringBuilder pozycja = new StringBuilder();

			Dal.Order order = Dal.DbHelper.Orders.GetOrder(receipt.OrderId);

			List<Dal.OrderReceipt> prePayments = Dal.DbHelper.Orders.GetReceipts(receipt.OrderId).Where(x => x.ReceiptTypeId == 2).ToList();

 
			decimal rate = prePayments.Sum(x => x.Amount) / order.AmountToPay;

			decimal i = 0;
			foreach (Dal.OrderReceiptProduct product in receiptProducts)
            {
				string stawka = "A";// product.OrderProduct.VAT == 0 ? "D" : "A";

				i++;
				Pozycja p = new Pozycja()
				{
					Akcja = "sprzedaz",
					Cena = product.Price.ToString(new System.Globalization.CultureInfo("en-US")),
					Ilosc = product.Quantity.ToString(),
					Jednostka = "szt",
					Kod_towaru = GetProductCode( product),
					Nazwa = product.Name,
					Opis = "",
					Stawka = stawka
				};


				if (product.Rebate > 0)
				{
					p.Rabat = new Rabat()
					{
						Akcja = "rabat",
						//Identyfikator_rabatu = "1",
						Nazwa="Rabat",
						Wartosc = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:0.00}%", product.Rebate), 

					};
				}
				pozycja.AppendLine(GetXml<Pozycja>(p));
			}
			return pozycja.ToString();
		}

		private static string GetOrderDetails(OrderReceipt receipt)
		{
			StringBuilder pozycja = new StringBuilder();

			Dal.Order order = Dal.DbHelper.Orders.GetOrder(receipt.OrderId);
			List<Dal.OrderReceiptProduct> orderProducts = Dal.DbHelper.Orders.GetReceiptProducts(receipt.ReceiptId);
			 
			pozycja.AppendLine(String.Format(@"<wydruk_niefiskalny numer_systemowy ='Zam:{0}, Par: {1}' naglowek_wydruku_niefiskalnego='nie' >", receipt.OrderId,receipt.ReceiptId));
			pozycja.AppendLine("<linia typ = 'linia' pogrubienie = 'tak' negatyw = 'nie' wysrodkowany='tak' numer_czcionki='1'" +
				"  wydruk_na='kopia' atrybut_czcionki='duza'>Zamówienie</linia>");
			   //< linia typ = "podkreslenie" ></ linia >
				//< linia > linia 3 </ linia >
				//   < linia ></ linia >
				//   < linia > linia 4 </ linia >
					  

					  



								  decimal i = 0;
			foreach (Dal.OrderReceiptProduct product in orderProducts)
			{
				i++;
				string name = String.Format("{0}. {2}*{3:0.00} {1}", i, product.Name, product.Quantity, product.Price);

				name = Bll.Helper.ReplacePolishCharacters(name);


				if (name.Length > 40)
					name = name.Substring(0, 40);


				name = String.Format("<linia>{0}</linia>", name);
				pozycja.AppendLine(name);

				if (product.Rebate > 0)
				{
					pozycja.AppendLine(String.Format("<linia>Rabat {0:0.00}%</linia>", product.Rebate));


				}
			}
			pozycja.AppendLine("<linia></linia>");
			pozycja.AppendLine(String.Format("<linia>Łączna wartość zamówienia: {0:0.00}</linia>", order.AmountToPay));
			pozycja.AppendLine("<linia></linia>");
			pozycja.AppendLine("<linia></linia>");

			List<Dal.OrderReceipt> prePayments = Dal.DbHelper.Orders.GetReceipts(receipt.OrderId)
				.Where(x => x.ReceiptTypeId == 2)
				.ToList();


			decimal prePaymentsSum = prePayments.Sum(x => x.Amount);

			pozycja.AppendLine(String.Format("<linia>Wpłacone zaliczki: {0:0.00}</linia>", prePaymentsSum));

			pozycja.AppendLine("</wydruk_niefiskalny>");
			return pozycja.ToString();
		}


		private static string GetProductCode(OrderReceiptProduct product)
        {
			if (product.OrderProduct != null)
			{ 
			 string code = Bll.Helper.ReplacePolishCharacters( product.OrderProduct.ProductCatalog.Code);
				if (code.Length > 20)
					code = code.Substring(0, 20);
				return code;
			}
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

		public static Dal.CashRegister IsConnected(int cashRegisterId)
        {
			Dal.CashRegister cs = Dal.DbHelper.Accounting.GetCashRegister(cashRegisterId);

			return cs;

			 

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
			{

				List<Dal.OrderReceipt> prePayments = Dal.DbHelper.Orders.GetReceipts(receipt.OrderId);

				decimal prePaymentsTotal = prePayments.Sum(x => x.Amount);
				decimal prePaymentsTotal2 = prePayments.Where(x=>x.ReceiptTypeId==2).Sum(x => x.Amount);
				decimal total = receipt.Amount;

				if (receipt.ReceiptTypeId == 3)
					total = prePaymentsTotal;

				Paragon paragon = new Paragon()
				{
					Akcja = "zamknij",
					Kasjer = receipt.InsertUser,
					Nip = receipt.Nip,
					Numer_systemowy = String.Format("Zam:{0}, Par: {1}", receipt.OrderId, receipt.ReceiptId),
					Kwota = total.ToString(new System.Globalization.CultureInfo("en-US"))
				};

				if (receipt.ReceiptTypeId == 3)
					paragon.Rabat = new Rabat()
					{
						Wartosc = prePaymentsTotal2.ToString(new System.Globalization.CultureInfo("en-US")),
						Akcja = "rabat",
						Nazwa = "Rozliczenie zaliczek"
					};

				return paragon;

			}
		}
    }
}
