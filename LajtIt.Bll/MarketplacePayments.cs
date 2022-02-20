using LinqToExcel;
using LinqToExcel.Attributes;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class MarketplacePayments
	{
		public class Przelewy24
		{
			[XmlRoot(ElementName = "tr")]
			public class Tr
			{
				[XmlElement(ElementName = "dp")]
				public string Dp { get; set; }
				[XmlElement(ElementName = "dw")]
				public string Dw { get; set; }
				[XmlElement(ElementName = "oi")]
				public string Oi { get; set; }
				[XmlElement(ElementName = "fo")]
				public string Fo { get; set; }
				[XmlElement(ElementName = "kw")]
				public string Kw { get; set; }
				[XmlElement(ElementName = "kl")]
				public string Kl { get; set; }
				[XmlElement(ElementName = "kr")]
				public string Kr { get; set; }
				[XmlElement(ElementName = "mi")]
				public string Mi { get; set; }
				[XmlElement(ElementName = "ko")]
				public string Ko { get; set; }
				[XmlElement(ElementName = "ad")]
				public string Ad { get; set; }
				[XmlElement(ElementName = "em")]
				public string Em { get; set; }
				[XmlElement(ElementName = "wy")]
				public string Wy { get; set; }
				[XmlElement(ElementName = "pr")]
				public string Pr { get; set; }
				[XmlElement(ElementName = "pf")]
				public string Pf { get; set; }
				[XmlElement(ElementName = "ty")]
				public string Ty { get; set; }
				[XmlAttribute(AttributeName = "id")]
				public string Id { get; set; }
			}

			[XmlRoot(ElementName = "transactions_list")]
			public class Transactions_list
			{
				[XmlElement(ElementName = "tr")]
				public List<Tr> Tr { get; set; }
			}
			public static void Process(int shopId, string userName, string fileName)
			{
				try
				{


					System.IO.StreamReader str = new System.IO.StreamReader(fileName);
					System.Xml.Serialization.XmlSerializer xSerializer =
						new System.Xml.Serialization.XmlSerializer(typeof(Transactions_list));
					Transactions_list data = (Transactions_list)xSerializer.Deserialize(str);
					str.Close();

					Process(shopId, userName, data);
				}
				catch (Exception ex)
				{
					//Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", name));

				}
			}

			private static void Process(int shopId, string userName, Transactions_list data)
			{
				List<Dal.ShopPayment> shopPayments = new List<Dal.ShopPayment>();

				foreach (Tr t in data.Tr.Skip(1).ToList())
				{
					try
					{ 
					shopPayments.Add(
						new Dal.ShopPayment()
						{
							Amount = decimal.Parse(t.Kw,  System.Globalization.CultureInfo.InvariantCulture)/100  ,
							BatchNumber = "",
							ClientName = t.Em,
							InsertDate = DateTime.Now,
							InsertUser = userName,
							PaymentDate = DateTime.Parse(t.Dw),
							PaymentOperator = "Przelewy24",
							PaymentNumber = t.Fo,
							PaymentTypeId = 1,
							ShopId = shopId,
							Title = t.Id.Substring(0, t.Id.IndexOf("_")==-1? t.Id.Length : t.Id.IndexOf("_")),
							TotalAmount = null
						});
					}catch(Exception ex)
                    {
						throw ex;
                    }
				}
				Dal.DbHelper.Accounting.SetShopPayments(shopId, shopPayments);
			}
		}

		public class Dpd
		{
			public class DpdXlsFile
			{

				[ExcelColumn("Nr_klienta")]
				public string NrKlienta { get; set; }

				[ExcelColumn("Odbiorca pobrania")]
				public string OdbiorcaPobrania { get; set; }

				[ExcelColumn("Odbiorca przesyłki")]
				public string OdbiorcaPrzesylki { get; set; }

				[ExcelColumn("Nr listu przewozowego (przesyłki)")]
				public string NumerPrzesylki { get; set; }

				[ExcelColumn("Data listu przewozowego (nadania)")]
				public string DataNadania { get; set; }

				[ExcelColumn("Kwota pobrania (COD)")]
				public string KwotaPobrania { get; set; }

				[ExcelColumn("Data skutecznego doręczenia przesyłki")]
				public string DataDoreczenia { get; set; }

				[ExcelColumn("Pole 'zawartość' w programie Unisoft-Klient")]
				public string Opis { get; set; }

				[ExcelColumn("Data zbiorczego przelewu")]
				public string DataPrzelewu { get; set; }

				[ExcelColumn("Nr rozliczenia (WY)")]
				public string NumerRozliczenia { get; set; }

				[ExcelColumn("Zbiorczy przelew")]
				public string ZbiorczyPrzelew { get; set; }

			}
			public static void Process(int shopId, string userName, string fileName)
			{
				try
				{


					ExcelQueryFactory eqf = new ExcelQueryFactory(fileName);// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\Maytoni_201810021244.xlsx");// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\LajtitImport.xls");
																			//eqf.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;

					//var c= eqf.GetColumnNames("Sheet1");



					var r = from p in eqf.Worksheet<DpdXlsFile>(0) select p;



					var rr = r.ToList();



					Process(shopId, userName, rr);
				}
				catch (Exception ex)
				{
					//Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", name));

				}
			}

			private static void Process(int shopId, string userName, List<DpdXlsFile> data)
			{
				List<Dal.ShopPayment> shopPayments = new List<Dal.ShopPayment>();

				foreach (DpdXlsFile t in data)
				{
					shopPayments.Add(
						new Dal.ShopPayment()
						{
							Amount = decimal.Parse(t.KwotaPobrania),
							BatchNumber = t.NumerRozliczenia,
							ClientName = t.OdbiorcaPrzesylki,
							InsertDate = DateTime.Now,
							InsertUser = userName,
							PaymentDate = DateTime.Parse(t.DataPrzelewu),
							PaymentOperator = "ING",
							PaymentNumber = t.Opis,
							PaymentTypeId = 1,
							ShopId = shopId,
							Title = GetOpis(t.Opis),
							TotalAmount = Decimal.Parse(t.ZbiorczyPrzelew)
						});
				}
				Dal.DbHelper.Accounting.SetShopPayments(shopId, shopPayments);
			}

            private static string GetOpis(string opis)
			{
				if (opis.IndexOf("-") > -1)
					return opis.Substring(0, opis.IndexOf("-"));

				else
					return "";

			}
        }
		public class Morele
		{
			public class MoreleXlsFile
			{

				[ExcelColumn("Data nadania statusu")]
				public string DataOperacji { get; set; }


				[ExcelColumn("Cash flow amount")]
				public string KwotaZamowienia { get; set; } 

				[ExcelColumn("Numer transakcji")]
				public string NumerTransakcji { get; set; }

				[ExcelColumn("Typ transakcji")]
				public string TypTransakcji { get; set; }
				[ExcelColumn("Status transakcji")]
				public string StatusTransakcji { get; set; }
				[ExcelColumn("Adres e-mail kupującego")]
				public string Email { get; set; }

			}
			public static void Process(int shopId, string userName, string fileName)
			{
				try
				{
			 

					ExcelQueryFactory eqf = new ExcelQueryFactory(fileName);// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\Maytoni_201810021244.xlsx");// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\LajtitImport.xls");
																			//eqf.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;

					//var c= eqf.GetColumnNames("Sheet1");



					var r = from p in eqf.Worksheet<MoreleXlsFile>(0) select p;



					var rr = r.ToList();



					Process(shopId, userName, rr);
				}
				catch (Exception ex)
				{
					throw ex;
					//Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", name));

				}
			}

			private static void Process(int shopId, string userName, List<MoreleXlsFile> data)
			{
				List<Dal.ShopPayment> shopPayments = new List<Dal.ShopPayment>();

				foreach (MoreleXlsFile t in data.Where(x => x.StatusTransakcji == "wykonana").ToList())
				{
					try
					{
						shopPayments.Add(
							new Dal.ShopPayment()
							{
								Amount = Decimal.Parse(t.KwotaZamowienia),
								BatchNumber = "",
								ClientName = t.Email??"",
								InsertDate = DateTime.Now,
								InsertUser = userName,
								PaymentDate = DateTime.Parse(t.DataOperacji),
								PaymentOperator = "DotPay",
								PaymentNumber = t.NumerTransakcji,
								PaymentTypeId = Decimal.Parse(t.KwotaZamowienia) > 0 ? 1 : 3,
								ShopId = shopId,
								Title = "",
								TotalAmount = null
							});
					}
					catch (Exception ex)
					{

					}

				}
				Dal.DbHelper.Accounting.SetShopPayments(shopId, shopPayments);
			}
 
		}
		public class Ceneo
		{
			public class CeneoXlsFile
			{

				[ExcelColumn("Data operacji")]
				public string DataOperacji { get; set; }

				[ExcelColumn("Nr zamowienia w Ceneo")]
				public string NumerZamowienia { get; set; }

				[ExcelColumn("Kwota zamowienia")]
				public string KwotaZamowienia { get; set; }

				[ExcelColumn("Saldo po operacji")]
				public string Saldo { get; set; }

				[ExcelColumn("Typ wyplaty")]
				public string TypWyplaty { get; set; }

			}
			public static void Process(int shopId, string userName, string fileName)
			{
				try
				{
					//Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
					//if (excelApp != null)
					//{
					//	Microsoft.Office.Interop.Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
					//	Microsoft.Office.Interop.Excel.Worksheet excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelWorkbook.Sheets[1];

					//	//                  Microsoft.Office.Interop.Excel.Range TempRange = excelWorksheet.get_Range("A1", "C3");

					//	//// 1. To Delete Entire Row - below rows will shift up
					//	//TempRange.EntireRow.Delete(Type.Missing);

					//	((Range)excelWorksheet.Rows[1, Missing.Value]).Delete(XlDeleteShiftDirection.xlShiftUp);
					//	((Range)excelWorksheet.Rows[1, Missing.Value]).Delete(XlDeleteShiftDirection.xlShiftUp);
					//	((Range)excelWorksheet.Rows[1, Missing.Value]).Delete(XlDeleteShiftDirection.xlShiftUp);
					//	//excelWorksheet.Rows[1].Delete();
					//	//excelWorksheet.Rows[2].Delete();
					//	//excelWorksheet.Rows[3].Delete();
					//	//excelWorksheet.Rows[4].Delete();
					//	fileName = fileName.Replace("-", "--");
					//	excelWorkbook.SaveCopyAs(fileName);
					//	//excelWorkbook.Close();
					//	excelApp.Quit();

					//}

					ExcelQueryFactory eqf = new ExcelQueryFactory(fileName);// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\Maytoni_201810021244.xlsx");// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\LajtitImport.xls");
																			//eqf.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;

					//var c= eqf.GetColumnNames("Sheet1");



					//var r = from p in eqf.Worksheet<CeneoXlsFile>(0) select p;

					var r = from p in eqf.WorksheetRange<CeneoXlsFile>("A4", "K60000", 0) select p;


					var rr = r.ToList();



					Process(shopId, userName, rr);
				}
				catch (Exception ex)
				{
					Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku Ceneo {0}", ex.Message));

				}
			}

			private static void Process(int shopId, string userName, List<CeneoXlsFile> data)
			{
				List<Dal.ShopPayment> shopPayments = new List<Dal.ShopPayment>();

				foreach (CeneoXlsFile t in data)
				{
					try
					{
						shopPayments.Add(
							new Dal.ShopPayment()
							{
								Amount = GetAmount(t),
								BatchNumber = "",
								ClientName = "",
								InsertDate = DateTime.Now,
								InsertUser = userName,
								PaymentDate = DateTime.Parse(t.DataOperacji),
								PaymentOperator = "PayU",
								PaymentNumber = String.Format("{0}{1}", GetPaymentType(t), t.NumerZamowienia),
								PaymentTypeId = GetPaymentType(t),
								ShopId = shopId,
								Title = t.NumerZamowienia,
								TotalAmount = Decimal.Parse(t.Saldo)
							});
					}
					catch (Exception ex)
					{

						Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku Ceneo {0}", ex.Message));
					}

				}

				Bll.ErrorHandler.SendEmail(String.Format("Liczba rekordów {0}", shopPayments.Count()));
				Dal.DbHelper.Accounting.SetShopPayments(shopId, shopPayments);
			}

			private static decimal GetAmount(CeneoXlsFile t)
			{
				if (!String.IsNullOrEmpty(t.TypWyplaty))
					return -Decimal.Parse(t.KwotaZamowienia);
				else
					return Decimal.Parse(t.KwotaZamowienia);
			}

			private static int GetPaymentType(CeneoXlsFile t)
			{
				if (!String.IsNullOrEmpty(t.TypWyplaty))
					return 3;
				else
					return Decimal.Parse(t.KwotaZamowienia) > 0 ? 1 : 2;

			}
		}

		public class Polcard
		{
			public class PolcardXlsFile
			{

				[ExcelColumn("Data transakcji")]
				public string DataTransakcji { get; set; }

				[ExcelColumn("Nr Transakcji")]
				public string NumerTransakcji { get; set; }

				[ExcelColumn("Kwota transakcji")]
				public string KwotaTransakcji { get; set; }

				[ExcelColumn("Numer płatności")]
				public string NumerPlatnosci { get; set; }

				[ExcelColumn("Kwota płatności")]
				public string KwotaPlatnosci { get; set; }

				[ExcelColumn("Data płatności")]
				public string DataPlatnosci { get; set; }

			}
			public static void Process(int shopId, string userName, string fileName)
			{
				try
				{


					ExcelQueryFactory eqf = new ExcelQueryFactory(fileName);// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\Maytoni_201810021244.xlsx");// @"C:\Users\Jacek\Documents\Visual Studio 2015\Projects\AllegroApp\LajtIt.Web\Files\ImportFiles\LajtitImport.xls");
																			//eqf.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;

					//var c= eqf.GetColumnNames("Sheet1");



					var r = from p in eqf.Worksheet<PolcardXlsFile>(0) select p;



					var rr = r.ToList();



					Process(shopId, userName, rr);
				}
				catch (Exception ex)
				{
					Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku Polcard{0}", ""));

				}
			}

			private static void Process(int shopId, string userName, List<PolcardXlsFile> data)
			{
				List<Dal.ShopPayment> shopPayments = new List<Dal.ShopPayment>();

				foreach (PolcardXlsFile t in data)
				{
					try
					{
					var p =		new Dal.ShopPayment()
							{
								
								BatchNumber = "",
								ClientName = "",
								InsertDate = DateTime.Now,
								InsertUser = userName,
								PaymentOperator = "First Data",
								ShopId = shopId,
								Title = "",
								TotalAmount = null,
								PaymentNumber=String.Format("{0}{1}", t.NumerTransakcji, t.DataTransakcji)
							};

						if (!String.IsNullOrEmpty(t.DataTransakcji))
						{
							string[] d = t.DataTransakcji.Substring(0, 8).Split(new char[] { '.' });
							p.PaymentDate = new DateTime(Int32.Parse("20" + d[2]), Int32.Parse(d[1]), Int32.Parse(d[0]));
							p.Amount = Decimal.Parse(t.KwotaTransakcji); 
							p.PaymentTypeId = Decimal.Parse(t.KwotaTransakcji) > 0 ? 1 : 2;
						}
						if (!String.IsNullOrEmpty(t.DataPlatnosci))
						{
							string[] d = t.DataPlatnosci.Substring(0, 8).Split(new char[] { '.' });
							p.PaymentDate = new DateTime(Int32.Parse("20" + d[2]), Int32.Parse(d[1]), Int32.Parse(d[0]));

							p.Amount = Decimal.Parse(t.KwotaPlatnosci);  
							p.PaymentTypeId = 3;
						}

						shopPayments.Add(p);
					}
					catch (Exception ex)
					{

					}

				}
				Dal.DbHelper.Accounting.SetShopPayments(shopId, shopPayments);
			}

		}
	}
}
