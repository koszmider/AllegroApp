using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class INGHelper
    {
		[XmlRoot(ElementName = "GrpHdr")]
		public class GrpHdr
		{
			[XmlElement(ElementName = "MsgId")]
			public string MsgId { get; set; }
			[XmlElement(ElementName = "CreDtTm")]
			public string CreDtTm { get; set; }
		}

		[XmlRoot(ElementName = "FrToDt")]
		public class FrToDt
		{
			[XmlElement(ElementName = "FrDtTm")]
			public string FrDtTm { get; set; }
			[XmlElement(ElementName = "ToDtTm")]
			public string ToDtTm { get; set; }
		}

		[XmlRoot(ElementName = "Id")]
		public class Id
		{
			[XmlElement(ElementName = "IBAN")]
			public string IBAN { get; set; }
			[XmlElement(ElementName = "Other")]
			public Other Other { get; set; }
		}

		[XmlRoot(ElementName = "PstlAdr")]
		public class PstlAdr
		{
			[XmlElement(ElementName = "Ctry")]
			public string Ctry { get; set; }
			[XmlElement(ElementName = "AdrLine")]
			public List<string> AdrLine { get; set; }
		}

		[XmlRoot(ElementName = "Ownr")]
		public class Ownr
		{
			[XmlElement(ElementName = "Nm")]
			public string Nm { get; set; }
			[XmlElement(ElementName = "PstlAdr")]
			public PstlAdr PstlAdr { get; set; }
		}

		[XmlRoot(ElementName = "Acct")]
		public class Acct
		{
			[XmlElement(ElementName = "Id")]
			public Id Id { get; set; }
			[XmlElement(ElementName = "Ownr")]
			public Ownr Ownr { get; set; }
		}

		[XmlRoot(ElementName = "Amt")]
		public class Amt
		{
			[XmlAttribute(AttributeName = "Ccy")]
			public string Ccy { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "BookgDt")]
		public class BookgDt
		{
			[XmlElement(ElementName = "DtTm")]
			public string DtTm { get; set; }
		}

		[XmlRoot(ElementName = "ValDt")]
		public class ValDt
		{
			[XmlElement(ElementName = "DtTm")]
			public string DtTm { get; set; }
		}

		[XmlRoot(ElementName = "Fmly")]
		public class Fmly
		{
			[XmlElement(ElementName = "Cd")]
			public string Cd { get; set; }
			[XmlElement(ElementName = "SubFmlyCd")]
			public string SubFmlyCd { get; set; }
		}

		[XmlRoot(ElementName = "Domn")]
		public class Domn
		{
			[XmlElement(ElementName = "Cd")]
			public string Cd { get; set; }
			[XmlElement(ElementName = "Fmly")]
			public Fmly Fmly { get; set; }
		}

		[XmlRoot(ElementName = "BkTxCd")]
		public class BkTxCd
		{
			[XmlElement(ElementName = "Domn")]
			public Domn Domn { get; set; }
		}

		[XmlRoot(ElementName = "Refs")]
		public class Refs
		{
			[XmlElement(ElementName = "MsgId")]
			public string MsgId { get; set; }
			[XmlElement(ElementName = "InstrId")]
			public string InstrId { get; set; }
			[XmlElement(ElementName = "EndToEndId")]
			public string EndToEndId { get; set; }
		}

		[XmlRoot(ElementName = "Cdtr")]
		public class Cdtr
		{
			[XmlElement(ElementName = "Nm")]
			public string Nm { get; set; }
		}

		[XmlRoot(ElementName = "Other")]
		public class Other
		{
			[XmlElement(ElementName = "Id")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "CdtrAcct")]
		public class CdtrAcct
		{
			[XmlElement(ElementName = "Id")]
			public Id Id { get; set; }
		}

		[XmlRoot(ElementName = "RltdPties")]
		public class RltdPties
		{
			[XmlElement(ElementName = "Cdtr")]
			public Cdtr Cdtr { get; set; }
			[XmlElement(ElementName = "CdtrAcct")]
			public CdtrAcct CdtrAcct { get; set; }
			[XmlElement(ElementName = "Dbtr")]
			public Dbtr Dbtr { get; set; }
			[XmlElement(ElementName = "DbtrAcct")]
			public DbtrAcct DbtrAcct { get; set; }
		}

		[XmlRoot(ElementName = "RmtInf")]
		public class RmtInf
		{
			[XmlElement(ElementName = "Ustrd")]
			public string Ustrd { get; set; }
		}

		[XmlRoot(ElementName = "TxDtls")]
		public class TxDtls
		{
			[XmlElement(ElementName = "Refs")]
			public Refs Refs { get; set; }
			[XmlElement(ElementName = "RltdPties")]
			public RltdPties RltdPties { get; set; }
			[XmlElement(ElementName = "RmtInf")]
			public RmtInf RmtInf { get; set; }
		}

		[XmlRoot(ElementName = "NtryDtls")]
		public class NtryDtls
		{
			[XmlElement(ElementName = "TxDtls")]
			public TxDtls TxDtls { get; set; }
		}

		[XmlRoot(ElementName = "Ntry")]
		public class Ntry
		{
			[XmlElement(ElementName = "Amt")]
			public Amt Amt { get; set; }
			[XmlElement(ElementName = "CdtDbtInd")]
			public string CdtDbtInd { get; set; }
			[XmlElement(ElementName = "Sts")]
			public string Sts { get; set; }
			[XmlElement(ElementName = "BookgDt")]
			public BookgDt BookgDt { get; set; }
			[XmlElement(ElementName = "ValDt")]
			public ValDt ValDt { get; set; }
			[XmlElement(ElementName = "BkTxCd")]
			public BkTxCd BkTxCd { get; set; }
			[XmlElement(ElementName = "NtryDtls")]
			public NtryDtls NtryDtls { get; set; }
		}

		[XmlRoot(ElementName = "Dbtr")]
		public class Dbtr
		{
			[XmlElement(ElementName = "Nm")]
			public string Nm { get; set; }
		}

		[XmlRoot(ElementName = "DbtrAcct")]
		public class DbtrAcct
		{
			[XmlElement(ElementName = "Id")]
			public Id Id { get; set; }
		}

		[XmlRoot(ElementName = "Rpt")]
		public class Rpt
		{
			[XmlElement(ElementName = "Id")]
			public string Id { get; set; }
			[XmlElement(ElementName = "CreDtTm")]
			public string CreDtTm { get; set; }
			[XmlElement(ElementName = "FrToDt")]
			public FrToDt FrToDt { get; set; }
			[XmlElement(ElementName = "Acct")]
			public Acct Acct { get; set; }
			[XmlElement(ElementName = "Ntry")]
			public List<Ntry> Ntry { get; set; }
		}

		[XmlRoot(ElementName = "BkToCstmrAcctRpt")]
		public class BkToCstmrAcctRpt
		{
			[XmlElement(ElementName = "GrpHdr")]
			public GrpHdr GrpHdr { get; set; }
			[XmlElement(ElementName = "Rpt")]
			public Rpt Rpt { get; set; }
		}

		[XmlRoot(ElementName = "Document")]
		public class Document
		{
			[XmlElement(ElementName = "BkToCstmrAcctRpt")]
			public BkToCstmrAcctRpt BkToCstmrAcctRpt { get; set; }
		}


		public static void LoadData(string saveLocation)
        {
			try
			{


				System.IO.StreamReader str = new System.IO.StreamReader(saveLocation, Encoding.GetEncoding("Windows-1250"));
				System.Xml.Serialization.XmlSerializer xSerializer =
					new System.Xml.Serialization.XmlSerializer(typeof(Document));
				Document data = (Document)xSerializer.Deserialize(str);
				str.Close();

				PricessData(data);
				 
			}
			catch (Exception ex)
			{
				Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku {0}", saveLocation));
				
			}


		}

        private static void PricessData(Document data)
        {
			List<Dal.BankAccount> bank = new List<Dal.BankAccount>();


			foreach(Ntry b in data.BkToCstmrAcctRpt.Rpt.Ntry)
            {
				string clientName = "";

				if (b.CdtDbtInd == "CRDT")
					clientName = b.NtryDtls.TxDtls.RltdPties.Dbtr.Nm;

				if (b.CdtDbtInd == "DBIT")
					clientName = b.NtryDtls.TxDtls.RltdPties.Cdtr.Nm;

				bank.Add(new Dal.BankAccount()
				{
					AccountId = 1,
					CompanyId = 78,
					TransferType = b.CdtDbtInd,
					AccountNumber = data.BkToCstmrAcctRpt.Rpt.Acct.Id.IBAN,
					Amount = Decimal.Parse(b.Amt.Text, System.Globalization.CultureInfo.InvariantCulture),
					ClientName = clientName,
					Comment = b.NtryDtls.TxDtls.RmtInf.Ustrd,
					InsertDate = DateTime.Now,
					InstrId = b.NtryDtls.TxDtls.Refs.InstrId,
					PaymentDate = DateTime.Parse(b.BookgDt.DtTm)

				});

            }


			Dal.DbHelper.Accounting.SetBankData(bank);
        }
    }
}
