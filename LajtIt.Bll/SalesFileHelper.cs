using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using LajtIt.Dal;
using LajtIt.Bll;


namespace LajtIt.Bll
{
    public class SalesFileHelper
    {

        public static void WriteToXlsxFile1(string sFrom, string sTo, string fName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("Worksheet1");

                    string[] header = new string[] {

                        "NIP",
                        "Numer dowodu",
                        "Kontrahent",
                        "Kraj",
                        "Data wystawienia",
                        "Data sprzedaży",
                        "Marketplace",
                        "Wartość zamówienia",
                        "Wpłaty",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "Paragony",
                        "",
                        "",
                        "",
                        "Paragon/Faktura",
                        "Numer referencyjny transakcji naszego systemu",
                        "Numer referencyjny transakcji systemu marketplace",
                        "Kurier",
                        "Rodzaj wysyłki"
                    };

                    var headerRow = new List<string[]>();
                    headerRow.Add(header);

                    string headerRange = "B1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    var worksheet = excel.Workbook.Worksheets["Worksheet1"];

                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    worksheet.Cells["J2"].Value = "Data wpłaty";
                    worksheet.Cells["K2"].Value = "Kwota wpłaty";
                    worksheet.Cells["L2"].Value = "Kwota wyksięgowana";
                    worksheet.Cells["M2"].Value = "Uwagi";
                    worksheet.Cells["N2"].Value = "Rodzaj płatności";
                    worksheet.Cells["O2"].Value = "Zaksięgowanie";

                    worksheet.Cells["P2"].Value = "Data pokwitowania";
                    worksheet.Cells["Q2"].Value = "Kwota pokwitowania";
                    worksheet.Cells["R2"].Value = "Typ pokwitowania";
                    worksheet.Cells["S2"].Value = "Kasa";

                    int cnt = 3;
                    int mergeStart = cnt;
                    int prevOrderId = -1;
                    bool colorFlag = false;
                    bool ifInvoice;
                    bool ifPar;

                    DateTime dtFrom = DateTime.Parse(sFrom);
                    DateTime dtTo = DateTime.Parse(sTo);

                    IQueryable<SalesFileFnResult> results = ctx.SalesFileFn(dtFrom, dtTo);

                    foreach (SalesFileFnResult result in results)
                    {
                        String str;
                        int cntr1 = 0;
                        int cntr2 = 0;
                        int cntrMax = 0;
                        IQueryable<SalesFilePaymentsFnResult> res1 = ctx.SalesFilePaymentsFn(result.OrderId);
                        IQueryable<SalesFileReceiptsFnResult> res2 = ctx.SalesFileReceiptsFn(result.OrderId);
                        cntrMax = res1.Count();
                        if (res2.Count() > cntrMax)
                            cntrMax = res2.Count();

                        foreach (SalesFilePaymentsFnResult r1 in res1)
                        {
                            str = String.Format("J{0}", cnt + cntr1);
                            worksheet.Cells[str].Value = r1.InsertDate.ToString();

                            str = String.Format("K{0}", cnt + cntr1);
                            worksheet.Cells[str].Value = r1.PaymentAmount;

                            str = String.Format("L{0}", cnt + cntr1);
                            worksheet.Cells[str].Value = r1.AmountMovedOut;

                            str = String.Format("M{0}", cnt + cntr1);
                            worksheet.Cells[str].Value = r1.Comment;

                            str = String.Format("N{0}", cnt + cntr1);
                            worksheet.Cells[str].Value = r1.PaymentTypeName;

                            str = String.Format("O{0}", cnt + cntr1);
                            worksheet.Cells[str].Value = r1.AccountingTypeName;

                            cntr1++;
                        }

                        foreach (SalesFileReceiptsFnResult r2 in res2)
                        {
                            str = String.Format("P{0}", cnt + cntr2);
                            worksheet.Cells[str].Value = r2.InsertDate.ToString();

                            str = String.Format("Q{0}", cnt + cntr2);
                            worksheet.Cells[str].Value = r2.ReceiptAmount;

                            str = String.Format("R{0}", cnt + cntr2);
                            worksheet.Cells[str].Value = r2.OrderReceiptTypeName;

                            str = String.Format("S{0}", cnt + cntr2);
                            worksheet.Cells[str].Value = r2.CashRegisterName;

                            cntr2++;
                        }

                        if (result.InvoiceNumber != null)
                            ifInvoice = true;
                        else
                            ifInvoice = false;

                        if (result.ParNumber != null)
                            ifPar = true;
                        else
                            ifPar = false;

                        str = String.Format("B{0}", cnt);
                        worksheet.Cells[str].Value = result.Nip;

                        str = String.Format("C{0}", cnt);
                        if (ifInvoice)
                            worksheet.Cells[str].Value = result.InvoiceNumber;
                        if (ifPar)
                            worksheet.Cells[str].Value = result.ParNumber;

                        str = String.Format("D{0}", cnt);
                        worksheet.Cells[str].Value = result.CompanyName;

                        str = String.Format("E{0}", cnt);
                        worksheet.Cells[str].Value = result.CountryName;

                        str = String.Format("F{0}", cnt);
                        if (ifInvoice)
                            worksheet.Cells[str].Value = result.InvoiceDate.ToString();
                        if (ifPar)
                            worksheet.Cells[str].Value = result.ParDate.ToString();

                        str = String.Format("G{0}", cnt);
                        worksheet.Cells[str].Value = result.SellDate.ToString();

                        str = String.Format("H{0}", cnt);
                        worksheet.Cells[str].Value = result.ShopName;

                        str = String.Format("I{0}", cnt);
                        worksheet.Cells[str].Value = result.AmountToPayCurrency;

                        str = String.Format("T{0}", cnt);
                        if (ifInvoice)
                            worksheet.Cells[str].Value = "Faktura";
                        if (ifPar)
                            worksheet.Cells[str].Value = "Paragon";

                        str = String.Format("U{0}", cnt);
                        worksheet.Cells[str].Value = result.OrderId;

                        str = String.Format("V{0}", cnt);
                        worksheet.Cells[str].Value = result.ExternalOrderNumber;

                        str = String.Format("W{0}", cnt);
                        worksheet.Cells[str].Value = result.ShippingCompanyName;

                        str = String.Format("X{0}", cnt);
                        worksheet.Cells[str].Value = result.ShippingServiceModeName;

                        for (int i = 0; i < header.Length; i++)
                        {
                            if (i < 8 || i > 17)
                            {
                                str = String.Format("{0}{1}:{0}{2}", (char)('A' + i + 1), cnt, cnt + cntrMax - 1);
                                worksheet.Cells[str].Merge = true;
                                worksheet.Cells[str].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                            }
                        }

                        cnt += cntrMax;
                    }

                    for (int i = 0; i < header.Length; i++)
                        worksheet.Column(i + 2).AutoFit();

                    String s = String.Format("J1:O1");
                    worksheet.Cells[s].Merge = true;
                    worksheet.Cells[s].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    s = String.Format("P1:S1");
                    worksheet.Cells[s].Merge = true;
                    worksheet.Cells[s].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                    for (int i = 0; i < header.Length; i++)
                    {
                        if (i < 8 || i > 17)
                        {
                            s = String.Format("{0}1:{0}2", (char)('A' + i + 1));
                            worksheet.Cells[s].Merge = true;
                            worksheet.Cells[s].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                        }
                    }

                    FileInfo excelFile = new FileInfo(@fName);
                    excel.SaveAs(excelFile);
                }
            }
        }

        public static void WriteToXlsxFile2(string sFrom, string sTo, string fName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("Worksheet1");

                    string[] header = new string[] {

                        "NIP",
                        "Numer dowodu",
                        "Kontrahent",
                        "Kraj",
                        "Data wystawienia",
                        "Data sprzedaży",
                        "Marketplace",
                        "Wartość zamówienia",
                        "Wpłaty",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "Paragony",
                        "",
                        "",
                        "",
                        "Paragon/Faktura",
                        "Numer referencyjny transakcji naszego systemu",
                        "Numer referencyjny transakcji systemu marketplace",
                        "Kurier",
                        "Rodzaj wysyłki"
                    };

                    var headerRow = new List<string[]>();
                    headerRow.Add(header);

                    string headerRange = "B1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    var worksheet = excel.Workbook.Worksheets["Worksheet1"];

                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    worksheet.Cells["J2"].Value = "Data wpłaty";
                    worksheet.Cells["K2"].Value = "Kwota wpłaty";
                    worksheet.Cells["L2"].Value = "Kwota wyksięgowana";
                    worksheet.Cells["M2"].Value = "Wpłacono razem";
                    worksheet.Cells["N2"].Value = "Uwagi";
                    worksheet.Cells["O2"].Value = "Rodzaj płatności";
                    worksheet.Cells["P2"].Value = "Zaksięgowanie";

                    worksheet.Cells["Q2"].Value = "Data pokwitowania";
                    worksheet.Cells["R2"].Value = "Kwota pokwitowania";
                    worksheet.Cells["S2"].Value = "Typ pokwitowania";
                    worksheet.Cells["T2"].Value = "Kasa";

                    int cnt = 3;
                    bool ifInvoice;
                    bool ifPar;

                    DateTime dtFrom = DateTime.Parse(sFrom);
                    DateTime dtTo = DateTime.Parse(sTo);

                    IQueryable<SalesFileOrderPaymentsFnResult> payments = ctx.SalesFileOrderPaymentsFn(dtFrom, dtTo);

                    foreach (SalesFileOrderPaymentsFnResult payment in payments)
                    {
                        String str;
                        SalesFileOrderDataForPaymentFnResult res1 = ctx.SalesFileOrderDataForPaymentFn(payment.OrderId).First();
                        SalesFileOrderPaidSoFarFnResult res2 = ctx.SalesFileOrderPaidSoFarFn(payment.OrderId, payment.InsertDate).First();
                        SalesFileReceiptForOrderPaymentFnResult res3 = ctx.SalesFileReceiptForOrderPaymentFn(payment.OrderId, payment.InsertDate).FirstOrDefault();

                        str = String.Format("J{0}", cnt);
                        worksheet.Cells[str].Value = payment.InsertDate.ToString();

                        str = String.Format("K{0}", cnt);
                        worksheet.Cells[str].Value = payment.PaymentAmount;

                        str = String.Format("L{0}", cnt);
                        worksheet.Cells[str].Value = payment.AmountMovedOut;

                        str = String.Format("M{0}", cnt);
                        worksheet.Cells[str].Value = res2.PaidSoFar;

                        str = String.Format("N{0}", cnt);
                        worksheet.Cells[str].Value = payment.Comment;

                        str = String.Format("O{0}", cnt);
                        worksheet.Cells[str].Value = payment.PaymentTypeName;

                        str = String.Format("P{0}", cnt);
                        worksheet.Cells[str].Value = payment.AccountingTypeName;

                        if (res3 != null)
                        {
                            str = String.Format("Q{0}", cnt);
                            worksheet.Cells[str].Value = res3.InsertDate.ToString();

                            str = String.Format("R{0}", cnt);
                            worksheet.Cells[str].Value = res3.ReceiptAmount;

                            str = String.Format("S{0}", cnt);
                            worksheet.Cells[str].Value = res3.OrderReceiptTypeName;

                            str = String.Format("T{0}", cnt);
                            worksheet.Cells[str].Value = res3.CashRegisterName;
                        }

                        if (res1.InvoiceNumber != null)
                            ifInvoice = true;
                        else
                            ifInvoice = false;

                        if (res1.ParNumber != null)
                            ifPar = true;
                        else
                            ifPar = false;

                        str = String.Format("B{0}", cnt);
                        worksheet.Cells[str].Value = res1.Nip;

                        str = String.Format("C{0}", cnt);
                        if (ifInvoice)
                            worksheet.Cells[str].Value = res1.InvoiceNumber;
                        if (ifPar)
                            worksheet.Cells[str].Value = res1.ParNumber;

                        str = String.Format("D{0}", cnt);
                        worksheet.Cells[str].Value = res1.CompanyName;

                        str = String.Format("E{0}", cnt);
                        worksheet.Cells[str].Value = res1.CountryName;

                        str = String.Format("F{0}", cnt);
                        if (ifInvoice)
                            worksheet.Cells[str].Value = res1.InvoiceDate.ToString();
                        if (ifPar)
                            worksheet.Cells[str].Value = res1.ParDate.ToString();

                        str = String.Format("G{0}", cnt);
                        worksheet.Cells[str].Value = res1.SellDate.ToString();

                        str = String.Format("H{0}", cnt);
                        worksheet.Cells[str].Value = res1.ShopName;

                        str = String.Format("I{0}", cnt);
                        worksheet.Cells[str].Value = res1.AmountToPay;

                        str = String.Format("U{0}", cnt);
                        if (ifInvoice)
                            worksheet.Cells[str].Value = "Faktura";
                        if (ifPar)
                            worksheet.Cells[str].Value = "Paragon";

                        str = String.Format("V{0}", cnt);
                        worksheet.Cells[str].Value = payment.OrderId;

                        str = String.Format("W{0}", cnt);
                        worksheet.Cells[str].Value = res1.ExternalOrderNumber;

                        str = String.Format("X{0}", cnt);
                        worksheet.Cells[str].Value = res1.ShippingCompanyName;

                        str = String.Format("Y{0}", cnt);
                        worksheet.Cells[str].Value = res1.ShippingServiceModeName;

                        cnt++;
                    }

                    for (int i = 0; i < header.Length; i++)
                        worksheet.Column(i + 2).AutoFit();

                    String s = String.Format("J1:P1");
                    worksheet.Cells[s].Merge = true;
                    worksheet.Cells[s].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    s = String.Format("Q1:T1");
                    worksheet.Cells[s].Merge = true;
                    worksheet.Cells[s].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                    for (int i = 0; i < header.Length; i++)
                    {
                        if (i < 8 || i > 18)
                        {
                            s = String.Format("{0}1:{0}2", (char)('A' + i + 1));
                            worksheet.Cells[s].Merge = true;
                            worksheet.Cells[s].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                        }
                    }

                    FileInfo excelFile = new FileInfo(@fName);
                    excel.SaveAs(excelFile);
                }
            }
        }

        public static void Generate()
        {
            string sFrom = "2022-01-01";
            string sTo = "2022-01-31";
            string fName = "styczen1.xlsx";

            //WriteToXlsxFile1(sFrom, sTo, fName);
            WriteToXlsxFile2(sFrom, sTo, fName);
        }

    }
}
