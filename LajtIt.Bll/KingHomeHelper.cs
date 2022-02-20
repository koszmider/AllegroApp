using LajtIt.Dal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
	public class KingHomeHelper : ImportData, IImportData
	{
		#region xml
		[XmlRoot(ElementName = "lista_zdjec")]
		public class Lista_zdjec
		{
			[XmlElement(ElementName = "zdjecie")]
			public List<string> Zdjecie { get; set; }
		}

		[XmlRoot(ElementName = "produkt")]
		public class Produkt
		{
			[XmlElement(ElementName = "numer")]
			public string Numer { get; set; }
			[XmlElement(ElementName = "indeks_handlowy")]
			public string Indeks_handlowy { get; set; }
			[XmlElement(ElementName = "nazwa")]
			public string Nazwa { get; set; }
			[XmlElement(ElementName = "opis")]
			public string Opis { get; set; }
			[XmlElement(ElementName = "zdjecie")]
			public string Zdjecie { get; set; }
			[XmlElement(ElementName = "stan")]
			public string Stan { get; set; }
			[XmlElement(ElementName = "kategoria_glowna")]
			public string Kategoria_glowna { get; set; }
			[XmlElement(ElementName = "kod_kreskowy")]
			public string Kod_kreskowy { get; set; }
			[XmlElement(ElementName = "kategoria_wielopoziomowa")]
			public string Kategoria_wielopoziomowa { get; set; }
			[XmlElement(ElementName = "wysokosc_calkowita")]
			public string Wysokosc_calkowita { get; set; }
			[XmlElement(ElementName = "szerokosc")]
			public string Szerokosc { get; set; }
			[XmlElement(ElementName = "wysokosc_krawedzi_siedziska")]
			public string Wysokosc_krawedzi_siedziska { get; set; }
			[XmlElement(ElementName = "glebokosc")]
			public string Glebokosc { get; set; }
			[XmlElement(ElementName = "wysokosc_podlokietnikow")]
			public string Wysokosc_podlokietnikow { get; set; }
			[XmlElement(ElementName = "produkt_na_zamowienie")]
			public string Produkt_na_zamowienie { get; set; }
			[XmlElement(ElementName = "Ilosc_w_paczce")]
			public string Ilosc_w_paczce { get; set; }
			[XmlElement(ElementName = "Koszt_dostawy")]
			public string Koszt_dostawy { get; set; }
			[XmlElement(ElementName = "vat")]
			public string Vat { get; set; }
			[XmlElement(ElementName = "cena")]
			public string Cena { get; set; }
			[XmlElement(ElementName = "cena_zakupu_netto")]
			public string Cena_zakupu_netto { get; set; }
			[XmlElement(ElementName = "cena_zakupu_brutto")]
			public string Cena_zakupu_brutto { get; set; }
			[XmlElement(ElementName = "cena_zakupu_netto_bez_waluty")]
			public string Cena_zakupu_netto_bez_waluty { get; set; }
			[XmlElement(ElementName = "cena_zakupu_brutto_bez_waluty")]
			public string Cena_zakupu_brutto_bez_waluty { get; set; }
			[XmlElement(ElementName = "stan_liczbowy")]
			public string Stan_liczbowy { get; set; }
			[XmlElement(ElementName = "producent")]
			public string Producent { get; set; }
			[XmlElement(ElementName = "opis_tekstowy")]
			public string Opis_tekstowy { get; set; }
			[XmlElement(ElementName = "lista_zdjec")]
			public Lista_zdjec Lista_zdjec { get; set; }
			[XmlElement(ElementName = "promocja_do_dnia")]
			public string Promocja_do_dnia { get; set; }
			[XmlElement(ElementName = "cena_detaliczna_netto")]
			public string Cena_detaliczna_netto { get; set; }
			[XmlElement(ElementName = "cena_detaliczna_brutto")]
			public string Cena_detaliczna_brutto { get; set; }
			[XmlElement(ElementName = "cena_detaliczna_netto_bez_waluty")]
			public string Cena_detaliczna_netto_bez_waluty { get; set; }
			[XmlElement(ElementName = "cena_detaliczna_brutto_bez_waluty")]
			public string Cena_detaliczna_brutto_bez_waluty { get; set; }
			[XmlElement(ElementName = "cena_data")]
			public string Cena_data { get; set; }
			[XmlElement(ElementName = "jednostka_wymiaru")]
			public string Jednostka_wymiaru { get; set; }
			[XmlElement(ElementName = "waga")]
			public string Waga { get; set; }
			[XmlElement(ElementName = "czy_w_promocji")]
			public string Czy_w_promocji { get; set; }
			[XmlElement(ElementName = "promocja_od_dnia")]
			public string Promocja_od_dnia { get; set; }
			[XmlElement(ElementName = "czy_nowosc")]
			public string Czy_nowosc { get; set; }
			[XmlElement(ElementName = "cena_promocyjna_netto")]
			public string Cena_promocyjna_netto { get; set; }
			[XmlElement(ElementName = "cena_promocyjna_brutto")]
			public string Cena_promocyjna_brutto { get; set; }
			[XmlElement(ElementName = "produkt_wycofany")]
			public string Produkt_wycofany { get; set; }
		}

		[XmlRoot(ElementName = "produkty")]
		public class Produkty
		{
			[XmlElement(ElementName = "produkt")]
			public List<Produkt> Produkt { get; set; }
			[XmlAttribute(AttributeName = "data")]
			public string Data { get; set; }
		}


		#endregion


		public new void LoadData<T>()
		{
			T data = base.LoadData<T>();
			ProcessData(data);
			base.PostLoadProcess();
		}



		public void ProcessData<T>(T obj)
		{
			Produkty pm = obj as Produkty;

			List<Produkt> produkty = pm.Produkt.Where(x => x.Kategoria_glowna == "Oświetlenie" || x.Kategoria_wielopoziomowa == "Lampy ogrodowe").ToList();

			Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
			Dal.SupplierOwner so = pch.GetSupplierOwner("King Home");

			List<Dal.Supplier> suppliers = pch.GetSuppliersByOwner(so.SupplierOwnerId);




			List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();
			foreach (Produkt produkt in produkty)
			{
				try

				{

					int? supplierId = GetSupplier(suppliers, produkt.Producent);

					if (!supplierId.HasValue)
						continue;


					Dal.Supplier supplier = suppliers.Where(x => x.SupplierId == supplierId.Value).FirstOrDefault();

					if (!supplierId.HasValue)
						continue;

					Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(supplierId.Value, produkt.Indeks_handlowy, false);

					pc.PriceBruttoFixed = Decimal.Parse(produkt.Cena_detaliczna_brutto_bez_waluty);
					pc.ExternalId = produkt.Numer;
					pc.Code = produkt.Indeks_handlowy;
					pc.Ean = GetStringOrNull(produkt.Kod_kreskowy);
					pc.IsAvailable = Int32.Parse(produkt.Stan_liczbowy) > 0; // IsAvailable(supplier, );
					pc.SupplierQuantity = Int32.Parse(produkt.Stan_liczbowy);
					pc.IsDiscontinued = produkt.Produkt_wycofany == "TAK";
					pc.SupplierId = supplierId.Value;
					pc.Name = produkt.Nazwa;
					pc.DeliveryId = GetDeliveryId(produkt.Produkt_na_zamowienie);

					if (produkt.Czy_w_promocji == "TAK" && Decimal.Parse(produkt.Cena_promocyjna_brutto.Replace("PLN", "")) < Decimal.Parse(produkt.Cena_detaliczna_brutto_bez_waluty))
					{
						pc.PriceBruttoPromo = Decimal.Parse(produkt.Cena_promocyjna_brutto.Replace("PLN", ""));
						pc.PriceBruttoPromoDate = DateTime.Parse(produkt.Promocja_do_dnia);
						pc.IsActivePricePromo = true;
					}
					else
					{
						pc.PriceBruttoPromo = null;
						pc.PriceBruttoPromoDate = null;
						pc.IsActivePricePromo = false;
					}
					products.Add(pc);
				}
				catch (Exception ex)
				{
					Bll.ErrorHandler.SendEmail(String.Format("Błąd w aktualizacji danych pliku KingHome, Nazwa: {0}, Kod: {1}, <br><br>{2}", produkt.Nazwa, produkt.Indeks_handlowy, ex.Message));
				}

			}
			Dal.KingHomeHelper sh = new Dal.KingHomeHelper();
			sh.SetUpdateProducts(products);




			List<Dal.ProductCatalog> productsForImages = pch.GetProductCatalogBySupplierOwner(so.SupplierOwnerId).Where(x => x.ImageId == null).ToList();



			foreach (Dal.ProductCatalog productImage in productsForImages)
			{
				Produkt p = produkty.Where(x => x.Numer == productImage.ExternalId).FirstOrDefault();


				if (p == null)
					continue;

				if (p.Lista_zdjec != null && p.Lista_zdjec.Zdjecie != null && p.Lista_zdjec.Zdjecie.Count > 0)
				{
					foreach (string photo in p.Lista_zdjec.Zdjecie)
					{
						if (String.IsNullOrEmpty(photo))
							continue;

						AltavolaHelper.DownloadImage(photo, productImage.ProductCatalogId);
					}
				}
			}

			Dal.OrderHelper oh = new Dal.OrderHelper();

			foreach (Dal.Supplier supplier in suppliers)
				oh.SetSupplierImportDate(supplier.SupplierId, DateTime.Now);


		}

		private bool IsAvailable(Dal.Supplier supplier, int quantity)
		{
			if (supplier.IsQuantityTrackingAvailable && supplier.QuantityMinLevel.HasValue)
				return quantity - supplier.QuantityMinLevel.Value > 0;
			else
				return quantity > 0;
		}
		private int? GetDeliveryId(string value)
		{
			if (value == null)
				return null;

			if (value.Contains("14 dni"))
				return 14;
			if (value.Contains("21 dni"))
				return 14;
			if (value.Contains("45 dni"))
				return 60;
			if (value.Contains("3 mies"))
				return 60;

			return null;
		}

		private int? GetSupplier(List<Supplier> suppliers, string producent)
		{
			Dal.Supplier supplier = suppliers.Where(x => x.Name == producent).FirstOrDefault();

			if (supplier != null)
				return supplier.SupplierId;
			else
				return null;
		}

		private string GetStringOrNull(string v)
		{
			if (String.IsNullOrEmpty(v))
				return null;
			else
				return v.Trim();
		}
	}
}
