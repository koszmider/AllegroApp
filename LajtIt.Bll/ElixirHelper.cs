using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQtoCSV;


namespace LajtIt.Bll
{
    public class ElixirHelper
    {

        class Elixir
        {
            [CsvColumn(FieldIndex = 1)]
            public string TypTransakcji { get; set; }

            [CsvColumn(FieldIndex = 2)]
            public string DataPlatnosci { get; set; }

            [CsvColumn(FieldIndex = 3)]
            public int Kwota { get; set; }

            [CsvColumn(FieldIndex = 4)]
            public string NRBZlecajacy { get; set; }

            [CsvColumn(FieldIndex = 5)]
            public string Wartosc { get; set; }

            [CsvColumn(FieldIndex = 6)]
            public string NumerRachunkuZlecajacy { get; set; }

            [CsvColumn(FieldIndex = 7)]
            public string NumerRachunkuKontrahenta { get; set; }

            [CsvColumn(FieldIndex = 8)]
            public string NazwaZlecajacy { get; set; }

            [CsvColumn(FieldIndex = 9)]
            public string NazwaKontrahenta { get; set; }

            [CsvColumn(FieldIndex = 10)]
            public string NieUzywane { get; set; }

            [CsvColumn(FieldIndex = 11)]
            public string NumerRozliczeniowyKontrahenta { get; set; }

            [CsvColumn(FieldIndex = 12)]
            public string OpisPlatnosci { get; set; }

            [CsvColumn(FieldIndex = 13)]
            public string PustePole1 { get; set; }

            [CsvColumn(FieldIndex = 14)]
            public string PustePole2 { get; set; }

            [CsvColumn(FieldIndex = 15)]
            public string TypTransakcji2 { get; set; }

            [CsvColumn(FieldIndex = 16)]
            public string NumerReferencyjny { get; set; }
        }


        public bool GetFile(int[] costsIds, int companyOwnerId, out string file)
        {
            file = null;
            if (!CheckCosts(costsIds))
                return false;

            //List<Elixir> payments = new List<Elixir>();

            List<Dal.Cost> costs = Dal.DbHelper.Accounting.GetCosts(costsIds).Where(x => x.Company.CanSendToBank && x.ToPay.HasValue && x.ToPay.Value &&  x.Amount > 0).ToList();

            if(costs.Select(x=>x.CompanyOwnerId).Distinct().Count()>1) // można eksportować tylko dla jednej firmy na raz
            {
                return false;

            }

            costs=costs.Where(x => x.CompanyOwnerId == companyOwnerId).ToList();

            Dal.Company companyOwner = Dal.DbHelper.Accounting.GetCompany(companyOwnerId);

            StringBuilder sb = new StringBuilder();

            foreach(Dal.Cost cost in costs)
            {
                string tmp = @"{0},{1},{2},{3},{4},""{5}"",""{6}"",""{7}"",""{8}"",{9},{10},""{11}"",""{12}"",""{13}"",""{14}"",""{15}""";


                string line = String.Format(tmp,
                    "110",
                    cost.PaidDate.Value.ToString("yyyyMMdd"),
                    (int)(cost.Amount * 100),
                    "10901870",
                    0,
                    companyOwner.BankAccountNumber.Replace(" ", ""),
                    cost.Company.BankAccountNumber,
                    companyOwner.BankName,
                    cost.Company.BankName,
                    "0",
                    cost.Company.BankNumber,
                    String.Format("{0} ({1:MM.dd})", cost.InvoiceNumber, cost.PaidDate),
                    "",
                    "",
                    "51",
                    cost.CostId);

                sb.AppendLine(line);

                //Elixir payment = new Elixir()
                //{
                //    DataPlatnosci = cost.Date.ToString("yyyyMMdd"),
                //    Kwota = (int)(cost.Amount * 100),
                //    NazwaKontrahenta = cost.Comment,
                //    NazwaZlecajacy = "Lajtit.pl",
                //    NieUzywane = "0",
                //    NRBZlecajacy = "10501038",
                //    NumerRachunkuKontrahenta = "65116022020000000297396105",
                //    NumerRachunkuZlecajacy = "15105014611000009145859220",
                //    NumerReferencyjny = cost.CostId.ToString(),
                //    NumerRozliczeniowyKontrahenta = "10901870",
                //    OpisPlatnosci = cost.Comment,
                //    PustePole1 = "",
                //    PustePole2 = "",
                //    TypTransakcji = "110",
                //    TypTransakcji2 = "51",
                //    Wartosc = "0"


                //};

                //payments.Add(payment);
            }
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute=true, 
                TextEncoding = Encoding.GetEncoding("Windows-1250"),
                 
            };



            //CsvContext cc = new CsvContext();
            string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
             file = String.Format(path, "products2.txt");
            //cc.Write(payments, file, inputFileDescription);

            File.WriteAllText(file, sb.ToString());

           



            return true;
        }

        private bool CheckCosts(int[] costsIds)
        { 

            List<Dal.Cost> costs = Dal.DbHelper.Accounting.GetCosts(costsIds).Where(x => x.Company.IsReadyForPayment == false).ToList();

            return costs.Count() == 0;
        }
    }
}
