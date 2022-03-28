using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public class PDF
    {
        private Dal.OrderHelper oh;
        private iTextSharp.text.pdf.BaseFont STF_Helvetica_Polish;

        Document doc1;
        decimal totalBrutto = 0;
        string path;
        string imgPath;
        decimal totalCorrection = 0;
        public static string CreatePdf(byte[] bytes, DirectoryInfo di, string trackingNumber)
        {

            string f = String.Format("{0}.pdf", trackingNumber);
            string fileName = String.Format(@"{0}\{1}", di.FullName, f);
            File.WriteAllBytes(fileName, bytes);

            return f;
        }



        public PDF(string imageLocation, string outputLocation)
        {
            imgPath = imageLocation;
            path = outputLocation;

            oh = new Dal.OrderHelper();


            STF_Helvetica_Polish =
                iTextSharp.text.pdf.BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, iTextSharp.text.pdf.BaseFont.EMBEDDED);

        }
        #region Common
        private string GetPaymentType(Dal.Order order)
        {
            if (order.ShippingCurrencyCode != "PLN")
                return "";
            if (order.OrderShipping!= null && order.OrderShipping.COD.HasValue)
                return "Pobranie";
            else
                return "Przelew";
        }
        private string TotalToText(string languageCode, decimal totalBrutto)
        {
            return Dal.Helper.Formatowanie.LiczbaSlownie(languageCode, totalBrutto);
        }
        private static Paragraph CreateParagraph(string text, iTextSharp.text.Font fontText, int aligment)
        {
            Paragraph p = new Paragraph(text, fontText);
            p.Alignment = aligment;

            return p;
        }
        private static PdfPCell CreateRowCell(int aligment, string text, Font rowCell)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, rowCell));
            cell.HorizontalAlignment = aligment;

            return cell;
        }
        private static PdfPCell CreateRowCellNoBorder(int aligment, string text, Font rowCell)
        {
            PdfPCell cell = CreateRowCell(aligment, text, rowCell);
            cell.Border = Rectangle.NO_BORDER;
            return cell;
        }

        private static PdfPCell CreateFooterCell(int aligment, string text, Font rowCell, bool noBorder)
        {
            Font footerCell = new Font(rowCell);
            footerCell.SetStyle(Font.BOLD);
            PdfPCell cell = CreateRowCell(aligment, text, footerCell);
            if (noBorder)
                cell.BorderWidth = 0;

            return cell;
        }

        private static PdfPCell CreateHeaderCell(string text, Font headerFont)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, headerFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;

            return cell;
        }

        #endregion

        #region Orders

        //public string CreateOrders(int[] batchIds)
        //{
        //    doc1 = new Document(PageSize.A4);
        //    string invoiceFile = path + @"\" + "Orders_" + "_" + Guid.NewGuid().ToString() + ".pdf";
        //    PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
        //    doc1.Open();

        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    int[] orderIds = oh.GetOrderIDsBasedOnBatchIDs(batchIds);


        //    PdfPTable tableOrder = new PdfPTable(1);
        //    Font headerCell = new Font(STF_Helvetica_Polish, 12, Font.BOLD);
        //    tableOrder.SetWidthPercentage(new float[] { 600f }, PageSize.A4);


        //    foreach (int orderId in orderIds)
        //    {
        //        Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

        //        tableOrder.AddCell(CreateOrderDocument(order));



        //    }

        //    doc1.Add(tableOrder);

        //    doc1.Close();

        //    return invoiceFile;
        //}
        //private PdfPCell CreateOrderDocument(Dal.Order order)
        //{
        //    PdfPCell cellOrder = new PdfPCell();

        //    iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 16, iTextSharp.text.Font.NORMAL);


        //    cellOrder.AddElement(CreateParagraph(String.Format("Zamówienie nr: {1} dnia {0:yyyy/MM/dd}", order.InsertDate, order.OrderId), fontText, Element.ALIGN_CENTER));

        //    cellOrder.AddElement(CreateParagraph(String.Format("{0}\n{1} {2}\n{3}\n{4} {5}",
        //            order.ShipmentCompanyName, order.ShipmentFirstName, order.ShipmentLastName, order.ShipmentAddress, order.ShipmentPostcode, order.ShipmentCity),
        //            fontText, Element.ALIGN_LEFT));
        //    string sellDoc = "";
        //    if (order.InvoiceId.HasValue)
        //        sellDoc = String.Format("Faktura: {0} {1}", order.Invoice.Company.Name, order.Invoice.InvoiceNumber);
        //    else
        //        if (order.ParNumber != null)
        //        sellDoc = String.Format("Paragon {0}", order.ParNumber);
        //    else
        //        sellDoc = "brak";

        //    cellOrder.AddElement(CreateParagraph(String.Format("Dokument sprzedażowy: {0}", sellDoc), new iTextSharp.text.Font(STF_Helvetica_Polish, 14, iTextSharp.text.Font.NORMAL), Element.ALIGN_LEFT));



        //    List<Dal.OrderProductsView> orderProducts =
        //        oh.GetOrderProducts(order.OrderId);


        //    List<Dal.OrderProduct> subProducts = oh.GetOrderSubProducts(order.OrderId);



        //    PdfPTable table = new PdfPTable(4);
        //    InitializeOrderTable(STF_Helvetica_Polish, table);

        //    int i = 1;

        //    foreach (Dal.OrderProductsView product in orderProducts.Where(x => x.Quantity > 0).ToList())
        //    {
        //        Font rowCell = new Font(STF_Helvetica_Polish, 12, Font.NORMAL);

        //        List<Dal.OrderProductsView> products = oh.GetOrderProducts(order.OrderId);


        //        string productName = "";
        //        string components = "";
        //        List<Dal.OrderProduct> sub = subProducts.Where(x => x.SubOrderProductId == product.OrderProductId).ToList();
        //        if (sub.Count > 0)
        //        {
        //            components = "Komponenty:\n";
        //            foreach (Dal.OrderProduct s in sub)
        //                components += String.Format("- {0}\n", s.ProductCatalog.Name);
        //        }

        //        productName =
        //         String.Format("{0}\nKod: ({1})", product.CatalogName, product.Code);

        //        if (components != "")
        //            productName += "\n\n" + components;




        //        table.AddCell(CreateRowCell(Element.ALIGN_CENTER, i.ToString(), rowCell));
        //        table.AddCell(CreateRowCell(Element.ALIGN_LEFT, productName, rowCell));
        //        table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0}", product.Quantity.ToString()), rowCell));
        //        table.AddCell(CreateRowCell(Element.ALIGN_LEFT, product.Comment, rowCell));

        //        i++;
        //    }


        //    cellOrder.AddElement(table);

        //    return cellOrder;
        //}


        //private static void InitializeOrderTable(iTextSharp.text.pdf.BaseFont STF_Helvetica_Polish, PdfPTable table)
        //{


        //    Font headerCell = new Font(STF_Helvetica_Polish, 12, Font.BOLD);


        //    table.SetWidthPercentage(new float[] { 20f, 290f, 40f, 200f }, PageSize.A4);
        //    table.AddCell(new PdfPCell(new Phrase("Lp", new Font(STF_Helvetica_Polish, 12, Font.BOLD))));
        //    table.AddCell(new PdfPCell(new Phrase("Produkt", new Font(STF_Helvetica_Polish, 12, Font.BOLD))));
        //    table.AddCell(new PdfPCell(new Phrase("Szt", new Font(STF_Helvetica_Polish, 12, Font.BOLD))));
        //    table.AddCell(new PdfPCell(new Phrase("Komentarz", new Font(STF_Helvetica_Polish, 12, Font.BOLD))));

        //    // table.SetWidths
        //    table.SpacingBefore = 20f;
        //    table.SpacingAfter = 30f;
        //}

     

        #endregion
        #region Paragon
        //public string CreateParagons(int[] batchIds)
        //{
        //    doc1 = new Document(PageSize.A4);
        //    string invoiceFile = path + @"\" + "Paragons_" + "_" + Guid.NewGuid().ToString() + ".pdf";
        //    PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
        //    doc1.Open();

        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    int[] orderIds = oh.GetOrderIDsBasedOnBatchIDs(batchIds);

        //    Dal.Company company = oh.GetCompanies().Where(x => x.CompanyId == 1).FirstOrDefault();



        //    foreach (int orderId in orderIds)
        //    {
        //        Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);
        //        if (order.ParSeqNo == null || !order.ParActive || order.InvoiceId != null)
        //            continue;
        //        totalBrutto = 0;
        //        doc1.NewPage();
        //        CreateParagonDocument(order, company);



        //    }
        //    doc1.Close();

        //    return invoiceFile;
        //}

        public string CreateParagon(int orderId)
        {
            doc1 = new Document(PageSize.A4);
            string invoiceFile = path + @"\" + "Paragon_" + orderId.ToString() + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);
            Dal.Company company = oh.GetCompanies().Where(x => x.CompanyId == order.CompanyId).FirstOrDefault();

            totalBrutto = 0;
            CreateParagonDocument(order, company);
            doc1.Close();

            return invoiceFile;
        }

      

        public string CreateOrderDocument(int orderId, List<Dal.OrderProductsView> orderProducts)
        {

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);
            doc1 = new Document(PageSize.A4);

            PdfPTable table = new PdfPTable(6);


            string file = path + @"\" + "Zamowienie_" + orderId.ToString() + "_" + Guid.NewGuid().ToString() + ".pdf";
            //PdfWriter.GetInstance(doc1, new FileStream(file, FileMode.Create));

            // doc1.Open();
            // doc1.Add(CreateParagraph(String.Format("Zamówienie nr: {0} dnia {1:yyyy/MM/dd}", orderId, DateTime.Now), fontText, Element.ALIGN_CENTER));

            table = CreateOrderTable(table, orderProducts);
            // doc1.Add(table);

            // doc1.Close();

            PdfCreator.CreatePdf(iTextSharp.text.PageSize.LEGAL, table, String.Format("Zamówienie nr: {0}", orderId), file, true);
            return file;

        }
        public string CreatePrePayment(int orderId, Dal.Company company)
        {
            doc1 = new Document(PageSize.A4);
            string invoiceFile = path + @"\" + "PrePayment_" + orderId.ToString() + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            totalBrutto = 0;
            CreatePrePaymentDocument(order, company);
            doc1.Close();

            return invoiceFile;
        }
        private void CreatePrePaymentDocument(Dal.Order order, Dal.Company company)
        {


            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);

            doc1.Add(CreateParagraph(String.Format("Dokument zaliczkowy nr: {1} dnia {0:yyyy/MM/dd}", order.ParDate, order.ParNumber), fontText, Element.ALIGN_CENTER));

            PdfPTable tableInvoice = new PdfPTable(2);


            string me = String.Format(@"Sprzedawca:

{0}
{1} {5}
{2} {3}
NIP: {4}
tel: 600 732 000",
                company.Name,
                company.Address,
                company.PostalCode,
                company.City,
                company.TaxId,
                company.AddressNo);
            string client = String.Format(@"Nabywca:

{0}
{1}
{2} {3}",
  String.Format("{0} {1}", order.ShipmentFirstName, order.ShipmentLastName),
    order.ShipmentAddress,
    order.ShipmentPostcode,
    order.ShipmentCity);

            tableInvoice.SpacingBefore = 20f;
            tableInvoice.SetWidthPercentage(new float[] { 300f, 300f }, PageSize.A4);
            fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 14, iTextSharp.text.Font.NORMAL);
            tableInvoice.AddCell(CreateRowCellNoBorder(PdfPCell.ALIGN_LEFT, me, fontText));
            tableInvoice.AddCell(CreateRowCellNoBorder(PdfPCell.ALIGN_LEFT, client, fontText));

            doc1.Add(tableInvoice);

            //            PdfPTable tableInvoice = new PdfPTable(1);
            //            string me = String.Format(
            //                @"{0}
            //{1}
            //{2} {3}
            //NIP: {4}
            //tel: 604 688 227 ", company.Name, company.Address, company.PostalCode, company.City, company.TaxId

            //                );


            //            tableInvoice.SpacingBefore = 20f;
            //            tableInvoice.SetWidthPercentage(new float[] { 600f }, PageSize.A4);
            //            fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 14, iTextSharp.text.Font.NORMAL);

            //            PdfPCell cell = CreateRowCell(PdfPCell.ALIGN_LEFT, me, fontText);
            //            cell.Border = 0;
            //            tableInvoice.AddCell(cell);

            //            doc1.Add(tableInvoice);

            PdfPTable table = new PdfPTable(6);

            CreateParagonOrderTable(order, table);

            doc1.Add(table);

            string payment = GetPayments(order);

            if (payment != "")
                doc1.Add(CreateParagraph(String.Format("Zapłacono: \r\n{0}", payment), fontText, Element.ALIGN_LEFT));
            else
                doc1.Add(CreateParagraph(String.Format("Forma płatności: {0}", GetPaymentType(order)), fontText, Element.ALIGN_LEFT));



            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));
            doc1.Add(CreateParagraph("Sklep online", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("www.lajtit.pl", new iTextSharp.text.Font(STF_Helvetica_Polish, 22, iTextSharp.text.Font.NORMAL), Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("Salon firmowy", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("Pabianicka 163/165, 93-490 Łódź", new iTextSharp.text.Font(STF_Helvetica_Polish, 22, iTextSharp.text.Font.NORMAL), Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("Pomoc techniczna", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("www.lajtit.pl/pomoc_techniczna", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("Śledź nasz profil na facebook", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("www.facebook.com/Lajtit.Oswietlenie", fontText, Element.ALIGN_CENTER));
        }

        private void CreateParagonDocument(Dal.Order order, Dal.Company company)
        {


            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);

            doc1.Add(CreateParagraph(String.Format("Dowód sprzedaży / paragon nr: {1} dnia {0:yyyy/MM/dd}", order.ParDate, order.ParNumber), fontText, Element.ALIGN_CENTER));


            PdfPTable tableInvoice = new PdfPTable(1);
            string me = String.Format(
                @"{0}
{1} {5}
{2} {3}
NIP: {4}
tel: 600 732 000 ", company.Name, company.Address, company.PostalCode, company.City, company.TaxId, company.AddressNo);


            tableInvoice.SpacingBefore = 20f;
            tableInvoice.SetWidthPercentage(new float[] { 600f }, PageSize.A4);
            fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 14, iTextSharp.text.Font.NORMAL);

            PdfPCell cell = CreateRowCell(PdfPCell.ALIGN_LEFT, me, fontText);
            cell.Border = 0;
            tableInvoice.AddCell(cell);

            doc1.Add(tableInvoice);

            PdfPTable table = new PdfPTable(6);

            CreateParagonOrderTable(order, table);

            doc1.Add(table);
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));
            doc1.Add(CreateParagraph("Sklep online", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("www.lajtit.pl", new iTextSharp.text.Font(STF_Helvetica_Polish, 22, iTextSharp.text.Font.NORMAL), Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("Salon firmowy", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("Pabianicka 163/165, 93-490 Łódź", new iTextSharp.text.Font(STF_Helvetica_Polish, 22, iTextSharp.text.Font.NORMAL), Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_CENTER));
           // doc1.Add(CreateParagraph("Pomoc techniczna", fontText, Element.ALIGN_CENTER));
           // doc1.Add(CreateParagraph("www.lajtit.pl/pomoc_techniczna", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("Śledź nasz profil na facebook", fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph("www.facebook.com/Lajtit.Oswietlenie", fontText, Element.ALIGN_CENTER));
        }

        private PdfPTable CreateOrderTable(PdfPTable table, List<Dal.OrderProductsView> products)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 12, Font.NORMAL);



            Font headerCell = new Font(STF_Helvetica_Polish, 12, Font.BOLD);

            //PdfPCell cell = new PdfPCell(new Phrase("Header spanning 3 columns"));
            //cell.Colspan = 3;
            //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //table.AddCell(cell);

            table.SetWidthPercentage(new float[] { 20f, 80f, 270f, 70f, 80f, 80f }, PageSize.A4);
            table.AddCell(CreateHeaderCell("Lp", headerCell));
            table.AddCell(CreateHeaderCell("Kod produktu", headerCell));
            table.AddCell(CreateHeaderCell("Nazwa towaru", headerCell));
            table.AddCell(CreateHeaderCell("Ilość", headerCell));
            table.AddCell(CreateHeaderCell("Status", headerCell));
            table.AddCell(CreateHeaderCell("Uwagi", headerCell));
            //table.AddCell(CreateHeaderCell("Rabat", headerCell));
            //table.AddCell(CreateHeaderCell("Cena brutto", headerCell));
            //table.AddCell(CreateHeaderCell("Wartość brutto", headerCell));

            // table.SetWidths
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            int quantity = 0;
            int i = 1;

            foreach (Dal.OrderProductsView product in products.Where(x => x.Quantity > 0).ToList())
            {
                quantity += product.Quantity;

                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, i.ToString(), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, product.Code, rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, product.CatalogName, rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0}", product.Quantity.ToString()), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, product.StatusName, rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, product.Comment, rowCell));
                //table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00}%", product.Rebate), rowCell));
                //table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", product.Price), rowCell));
                //table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", brutto), rowCell));

                i++;
            }

            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, String.Format("{0}", quantity), rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            //table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            //table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "Razem:", rowCell, true));
            //table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totalBrutto), rowCell, false));

            return table;
        }

        private void CreateParagonOrderTable(Dal.Order order, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 12, Font.NORMAL);

            List<Dal.OrderProductsView> products = oh.GetOrderProducts(order.OrderId);
            InitializeParagonTable(STF_Helvetica_Polish, table);

            int quantity = 0;
            int i = 1;

            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            Dal.Settings s = sh.GetSetting("INV_TEMPL");


            foreach (Dal.OrderProductsView product in products.Where(x => x.Quantity > 0 && x.Price>0).ToList())
            {
                decimal rebate = 1 - product.Rebate / 100M;
                decimal brutto = product.Price * product.Quantity * rebate;

                quantity += product.Quantity;

                totalBrutto += brutto;

                string productName = product.CatalogName;
                if (product.IsOrderProduct == 1)
                    productName = Mixer.GetProductName(s.StringValue, product.ProductCatalogId.Value);


                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, i.ToString(), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, productName, rowCell));
                //    table.AddCell(CreateRowCell(Element.ALIGN_LEFT, product.ProductCatalog.Name, rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0} szt.", product.Quantity.ToString()), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00}%", product.Rebate), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", product.Price), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", brutto), rowCell));

                i++;
            }

            //if (order.ShippingCost  > 0)
            //{
            //    decimal brutto = order.ShippingCost ;



            //    quantity += 1;
            //    totalBrutto += brutto;

            //    table.AddCell(CreateRowCell(Element.ALIGN_CENTER, i.ToString(), rowCell));
            //    table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "Przesyłka", rowCell));
            //    table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0} szt.", "1"), rowCell));
            //    table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("0,00%", ""), rowCell));
            //    table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", order.ShippingCost), rowCell));
            //    table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", brutto), rowCell));


            //}



            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "Razem:", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totalBrutto), rowCell, false));


        }


        private static void InitializeParagonTable(iTextSharp.text.pdf.BaseFont STF_Helvetica_Polish, PdfPTable table)
        {


            Font headerCell = new Font(STF_Helvetica_Polish, 12, Font.BOLD);

            //PdfPCell cell = new PdfPCell(new Phrase("Header spanning 3 columns"));
            //cell.Colspan = 3;
            //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //table.AddCell(cell);

            table.SetWidthPercentage(new float[] { 20f, 270f, 80f, 70f, 80f, 80f }, PageSize.A4);
            table.AddCell(CreateHeaderCell("Lp", headerCell));
            table.AddCell(CreateHeaderCell("Nazwa towaru", headerCell));
            table.AddCell(CreateHeaderCell("Ilość", headerCell));
            table.AddCell(CreateHeaderCell("Rabat", headerCell));
            table.AddCell(CreateHeaderCell("Cena brutto", headerCell));
            table.AddCell(CreateHeaderCell("Wartość brutto", headerCell));

            // table.SetWidths
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
        }

        #endregion
        #region Invoice
        /*
        public string CreateInvoices(int[] batchIds)
        {
            doc1 = new Document(PageSize.A4);
            string invoiceFile = path + @"\" + "Invoices_" +  "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();

            Dal.OrderHelper oh = new Dal.OrderHelper();
            int[] orderIds = oh.GetOrderIDsBasedOnBatchIDs(batchIds);

            foreach (int orderId in orderIds)
            {
                Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);
                if (order.Invoice == null)
                    continue;
                totalBrutto = 0;
                doc1.NewPage();
                CreateInvoiceDocument(order, "ORYGINAŁ");
                doc1.NewPage();
                totalBrutto = 0;
                CreateInvoiceDocument(order, "KOPIA");

            }
            doc1.Close();

            return invoiceFile;
        }
        */
        public string CreateCorrectionInvoice(int orderId)
        {
            //doc1 = new Document(PageSize.A4);
            string invoiceFile = path + @"\" + "Invoice_K_" + orderId.ToString() + "_" + Guid.NewGuid().ToString() + ".pdf";
            //PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
           // doc1.Open();
            Dal.OrderHelper bllOH = new Dal.OrderHelper();

            Dal.Invoice invoice = bllOH.GetInvoiceByOrderId(orderId);
            Dal.Invoice invoiceCorrection = bllOH.GetInvoice(invoice.InvoiceCorrectionId.Value);
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            totalBrutto = 0;
        


            PdfCreator.CreatePdf(iTextSharp.text.PageSize.LEGAL, CreateInvoiceCorrectionDocument(order, invoice, invoiceCorrection, ""), "", invoiceFile, true);
            //doc1.Close();

            //doc1.NewPage();
            //totalBrutto = 0;
            //CreateInvoiceDocument(order, invoice, "KOPIA");

            return invoiceFile;
        }
        public string CreateInvoice(int orderId)
        {
            //  doc1 = new Document(PageSize.A4);
            string invoiceFile = path + @"\" + "Invoice_" + orderId.ToString() + "_" + Guid.NewGuid().ToString() + ".pdf";
            //  PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            //doc1.Open();
            Bll.OrderHelper bllOH = new OrderHelper();

            Dal.Invoice invoice = bllOH.GetInvoice(orderId);
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            string languageCode = order.ShippingCurrencyCode == "PLN" ? "pl" : "en";

            totalBrutto = 0;
            PdfCreator.CreatePdf(iTextSharp.text.PageSize.LEGAL, CreateInvoiceDocument(languageCode, order, invoice, ""), "", invoiceFile, true);
            //doc1.NewPage();
            //totalBrutto = 0;
            //CreateInvoiceDocument(order, invoice, "KOPIA");
            // doc1.Close();

            return invoiceFile;
        }

        private PdfPTable CreateInvoiceCorrectionDocument(Dal.Order order, Dal.Invoice invoice, Dal.Invoice invoiceCorrection, string headerText)
        {


            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 10, iTextSharp.text.Font.NORMAL);

            PdfPTable mainTable = new PdfPTable(1);
            mainTable.DefaultCell.Border = Rectangle.NO_BORDER;

            mainTable.SetWidthPercentage(new float[] { 580f }, PageSize.A4);


            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_RIGHT, "Miejsce wystawienia: PL, Łódź", fontText  ));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_RIGHT, String.Format("Data wystawienia: {0:yyyy/MM/dd}", invoiceCorrection.InvoiceDate), fontText));

            iTextSharp.text.Font fontHeader = new iTextSharp.text.Font(STF_Helvetica_Polish, 12, iTextSharp.text.Font.BOLD);

            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_CENTER, String.Format("Faktura korygująca VAT: {0}", invoiceCorrection.InvoiceNumber), fontHeader));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_RIGHT,  String.Format("Dotyczy faktury: {0}", invoice.InvoiceNumber), fontText));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_RIGHT,  String.Format("Data wystawienia: {0:yyyy/MM/dd}", invoice.InvoiceDate), fontText));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_CENTER, headerText, fontHeader));


            PdfPTable tableInvoice = new PdfPTable(2);


            string me = String.Format(@"Sprzedawca:

{0}
{1} {5}
{2} {3}
NIP: {4}
tel: 600 732 000",
                invoice.Company.Name,
                invoice.Company.Address,
                invoice.Company.PostalCode,
                invoice.Company.City,
                invoice.Company.TaxId,
                invoice.Company.AddressNo);
            string client = String.Format(@"Nabywca:

{0}
{1}
{2} {3}
{4}",
    invoice.CompanyName,
    invoice.Address,
    invoice.Postcode,
    invoice.City,
    String.IsNullOrEmpty(invoice.Nip) ? "" : String.Format("NIP: {0} {1}", invoice.CountryCode, invoice.Nip));

            tableInvoice.SpacingBefore = 20f;
            tableInvoice.SetWidthPercentage(new float[] { 300f, 300f }, PageSize.A4);
            tableInvoice.AddCell(CreateRowCellNoBorder(PdfPCell.ALIGN_LEFT, me, fontText));
            tableInvoice.AddCell(CreateRowCellNoBorder(PdfPCell.ALIGN_LEFT, client, fontText));
            var c = new PdfPCell(tableInvoice);
            c.Border = Rectangle.NO_BORDER;
            mainTable.AddCell(c);


            //doc1.Add(tableInvoice);
            Font fontBigger = new Font(STF_Helvetica_Polish, 12f, Font.BOLD);




            PdfPTable table1 = new PdfPTable(10)
            {
                TotalWidth = 600
            };
            // table1.SetWidthPercentage(new float[] { 60f, 100f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f }, PageSize.A4);
            CreateInvoiceOrderTable(invoice, invoiceCorrection, table1);

            // doc1.Add(table1);

            var ct = new PdfPCell(table1);
            ct.Border = Rectangle.NO_BORDER;

            mainTable.AddCell(ct);

 


            if (totalCorrection < 0)
                mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, String.Format("Ogółem do zwrotu: {0:C}", -totalCorrection), fontBigger));
            else
                mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, String.Format("Ogółem do zapłaty: {0:C}", totalCorrection), fontBigger));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, String.Format("Słownie: {0}", TotalToText("pl", Math.Abs(totalCorrection))), fontText));



            PdfPTable table = new PdfPTable(2);
            //Font rowCell = new Font(STF_Helvetica_Polish, 8, Font.NORMAL);
            //Font headerCell = new Font(STF_Helvetica_Polish, 8, Font.BOLD);
            table.SetWidthPercentage(new float[] { 300f, 300f }, PageSize.A4);
            table.SpacingBefore = 10f;
            table.SpacingAfter = 10f;
        
            var cc = new PdfPCell(table);
            cc.Border = Rectangle.NO_BORDER;

            mainTable.AddCell(cc);

            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, "Osoba uprawniona do wystawienia faktury", fontText));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, invoice.Company.CompanyOwner, fontText));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_RIGHT, "Data odbioru/Osoba uprawniona do odbioru faktury:", fontText ));

  
            if (!String.IsNullOrEmpty(invoiceCorrection.Comment))
            {
                List<Dal.SystemDictionary> dictionary = Dal.DbHelper.Shop.GetSystemDictionary("PL");

                mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, String.Format(GetDictionary("FakturaUwagi", dictionary), invoiceCorrection.Comment), fontText));
                mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, "", fontText));
                mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, "", fontText));

            }



            return mainTable;
 

        }

        private static string GetDictionary(string keyName, List<Dal.SystemDictionary> dictionary)
        {
            Dal.SystemDictionary d = dictionary.Where(x => x.KeyName == keyName).FirstOrDefault();

            if (d == null)
                return null;
            else
                return d.Description;
        }
        private PdfPTable CreateInvoiceDocument(string countryCode,  Dal.Order order, Dal.Invoice invoice, string headerText)
        {
            List<Dal.SystemDictionary> dictionary = Dal.DbHelper.Shop.GetSystemDictionary(countryCode);

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 10, iTextSharp.text.Font.NORMAL);

            PdfPTable mainTable = new PdfPTable(1);
            mainTable.DefaultCell.Border = Rectangle.NO_BORDER;

            mainTable.SetWidthPercentage(new float[] { 580f }, PageSize.A4);


            //mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_RIGHT, "Miejsce wystawienia: PL, Łódź", fontText));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_RIGHT, GetDictionary("FakturaMiejsceWystawienia", dictionary), fontText));


            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_RIGHT, String.Format(GetDictionary("FakturaDataWystawienia", dictionary), invoice.InvoiceDate), fontText));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_RIGHT, String.Format(GetDictionary("FakturaDataSprzedazy", dictionary), invoice.InvoiceSellDate), fontText));

            iTextSharp.text.Font fontHeader = new iTextSharp.text.Font(STF_Helvetica_Polish, 12, iTextSharp.text.Font.BOLD);

            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_CENTER, String.Format(GetDictionary("FakturaNumer", dictionary), invoice.InvoiceNumber), fontHeader));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_CENTER, headerText, fontHeader));



            string me = String.Format(@"{8}:

{0}
{1} {5}
{2} {3}
{9}: {4}{6}{7}",
                invoice.Company.Name,
                invoice.Company.Address,
                invoice.Company.PostalCode,
                invoice.Company.City,
                invoice.Company.TaxId,
                invoice.Company.AddressNo,
                invoice.Company.Regon == null ? "" : String.Format(@"
Regon: {0}", invoice.Company.Regon),
                String.IsNullOrEmpty(invoice.Company.BDO) == true ? "" : String.Format(@"
BDO: {0}", invoice.Company.BDO),
                GetDictionary("FakturaSprzedawca", dictionary),
                GetDictionary("FakturaNIP", dictionary)
                );
            string client = String.Format(@"{5}:

{0}
{1}
{2} {3}
{4}",
    invoice.CompanyName,
    invoice.Address,
    invoice.Postcode,
    invoice.City,
    String.IsNullOrEmpty(invoice.Nip) ? "" : String.Format("{1}:  {0}",   invoice.Nip, GetDictionary("FakturaNIP", dictionary)),
                GetDictionary("FakturaNabywca", dictionary));

            PdfPTable tableInvoice = new PdfPTable(2);
            tableInvoice.SpacingBefore = 20f;
            tableInvoice.SetWidthPercentage(new float[] { 300f, 300f }, PageSize.A4);
            tableInvoice.AddCell(CreateRowCellNoBorder(PdfPCell.ALIGN_LEFT, me, fontText));
            tableInvoice.AddCell(CreateRowCellNoBorder(PdfPCell.ALIGN_LEFT, client, fontText));
            var c = new PdfPCell(tableInvoice);
            c.Border = Rectangle.NO_BORDER;
            mainTable.AddCell(c);
            Font fontBigger = new Font(STF_Helvetica_Polish, 14f, Font.BOLD);

            string currencyCode = order.ShippingCurrencyCode;
            decimal rate = order.ShippingCurrencyRate;


            PdfPTable table1 = new PdfPTable(9);

            CreateInvoiceOrderTable(order,  invoice, table1, dictionary);

            var ct = new PdfPCell(table1)
            {
                Border = Rectangle.NO_BORDER
            };

            mainTable.AddCell(ct);
            // doc1.Add(table1);


            mainTable.AddCell(CreateParagraph(String.Format(GetDictionary("FakturaOgolemDoZaplaty", dictionary), GetValue2(totalBrutto, currencyCode, rate)), fontBigger, Element.ALIGN_LEFT));
            mainTable.AddCell(CreateParagraph(String.Format(GetDictionary("FakturaSlownie", dictionary), TotalToText(countryCode, totalBrutto)), fontText, Element.ALIGN_LEFT));



            PdfPTable table = new PdfPTable(2);
            Font rowCell = new Font(STF_Helvetica_Polish, 8, Font.NORMAL);
            Font headerCell = new Font(STF_Helvetica_Polish, 8, Font.BOLD);
            table.SetWidthPercentage(new float[] { 300f, 300f }, PageSize.A4);
            table.SpacingBefore = 10f;
            table.SpacingAfter = 10f;
            StringBuilder sb = new StringBuilder();

            #region płatności
            if (order.AmountBalance.HasValue && order.AmountBalance.Value < 0)
            {
                Dal.SettingsHelper sh = new Dal.SettingsHelper();
                Dal.Settings s = sh.GetSetting("INV_DAYS");
                sb.AppendLine(String.Format(GetDictionary("FakturaPozostalo", dictionary), GetValue(-order.AmountBalance.Value, currencyCode,rate), s.IntValue));
            }


            string payment = GetPayments(order);

            if (payment != "")
                sb.AppendLine(String.Format(GetDictionary("FakturaZaplacono", dictionary), payment));
            else
                sb.AppendLine(String.Format(GetDictionary("FakturaFormaPlatnosci", dictionary), GetPaymentType(order)));

            sb.AppendLine(String.Format(GetDictionary("FakturaKontoBankowe", dictionary), invoice.Company.BankAccountNumber));
            sb.AppendLine(String.Format(GetDictionary("FakturaKontoBankoweSWIFT", dictionary), "INGBPLPW"));
            sb.AppendLine(String.Format(GetDictionary("FakturaNumerZamowienia", dictionary), order.OrderId, order.ExternalOrderNumber));
            if (currencyCode != "PLN")
            {
                sb.AppendLine(" ");
                sb.AppendLine(" ");
                sb.AppendLine(" ");
                sb.AppendLine(String.Format(GetDictionary("FakturaKurs", dictionary), currencyCode, 1 / rate));
            }
            sb.AppendLine(" ");

            string txt = sb.ToString();

            table.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, txt, rowCell));
            #endregion
            #region komentarz

            if (!String.IsNullOrEmpty(invoice.Comment))
                table.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, String.Format(GetDictionary("FakturaUwagi", dictionary), invoice.Comment), rowCell));
            else
                table.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, "", rowCell));


            #endregion
            var cc = new PdfPCell(table);
            cc.Border = Rectangle.NO_BORDER;

            mainTable.AddCell(cc);

            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, GetDictionary("FakturaOsobaUprawnionaWystawienie", dictionary), fontText));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_LEFT, invoice.Company.CompanyOwner, fontText));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_RIGHT, GetDictionary("FakturaOsobaUprawnionaOdbior", dictionary), fontText));



            return mainTable;

        }
        private string GetPayments(Dal.Order order)
        { 
            List<Dal.OrderPayment> payments = Dal.DbHelper.Orders.GetOrderPayments(order.OrderId).Where(x => x.Amount > 0).ToList();

            string sb = "";
            foreach (Dal.OrderPayment payment in payments)
            {
                sb += String.Format("  * {0:0.00} {1} / {2:yyyy/MM/dd}  \r\n", 
                    payment.Amount * order.ShippingCurrencyRate, order.ShippingCurrencyCode, payment.InsertDate );

            }

            return sb;
        }
         
        private void CreateInvoiceSummaryTable(Dal.Invoice invoice, PdfPTable table)
        {



        }

        private void CreateInvoiceOrderTable(Dal.Order order, Dal.Invoice invoice, PdfPTable table, List<Dal.SystemDictionary> dictionary)
        {
            string currencyCode = order.ShippingCurrencyCode;
            decimal rate = order.ShippingCurrencyRate;

            Font rowCell = new Font(STF_Helvetica_Polish, 8, Font.NORMAL);

            List<Dal.InvoiceProduct> products = oh.GetInvoiceProducts(invoice.InvoiceId);
            InittializeInvoiceTable(STF_Helvetica_Polish, table, dictionary);

            List<Dal.Helper.Amount> totals = new List<Dal.Helper.Amount>();

            int quantity = 0;
            int i = 1;

            #region loop
            foreach (Dal.InvoiceProduct product in products)
            {

                quantity += product.Quantity;

                Dal.Helper.Amount total = totals.Where(x => x.VATRate == product.VatRate).FirstOrDefault();
                if (total != null)
                {
                    total.VAT += product.CalculatedTotalVat.Value;
                    total.Netto += product.CalculatedTotalNetto.Value;
                    total.Brutto += product.CalculatedTotalBrutto.Value;
                }
                else
                {
                    total = new Dal.Helper.Amount()
                    {
                        VAT = product.CalculatedTotalVat.Value,
                        Netto = product.CalculatedTotalNetto.Value,
                        Brutto = product.CalculatedTotalBrutto.Value,
                        VATRate = product.VatRate
                    };
                    totals.Add(total);
                }
                totalBrutto += product.CalculatedTotalBrutto.Value;


                string productName = product.Name;
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, i.ToString(), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, productName, rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0} {1}", product.Quantity.ToString(), GetDictionary("FakturaTabelaJednostka", dictionary)), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", product.Rebate), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, GetValue(product.PriceBrutto          , currencyCode, rate             ), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %",            product.VatRate * 100M                           ), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, GetValue(product.CalculatedTotalVat.Value    ,currencyCode, rate), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, GetValue(product.CalculatedTotalNetto.Value, currencyCode, rate), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, GetValue(product.CalculatedTotalBrutto.Value, currencyCode, rate), rowCell));

                i++;
            }

            #endregion


            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, GetDictionary("FakturaRazem", dictionary), rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "--", rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, GetValue(totals.Sum(x => x.VAT), currencyCode   ,rate), rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, GetValue(totals.Sum(x => x.Netto), currencyCode ,rate), rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, GetValue(totals.Sum(x => x.Brutto), currencyCode,rate), rowCell, false));

            foreach (Dal.Helper.Amount total in totals.OrderBy(x => x.VATRate))
            {
                table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", total.VATRate * 100M), rowCell, false));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, GetValue(total.VAT, currencyCode   ,rate), rowCell, false));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, GetValue(total.Netto, currencyCode ,rate), rowCell, false));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, GetValue(total.Brutto, currencyCode,rate), rowCell, false));
            }
        }

        private string GetValue(decimal priceBrutto, string currencyCode, decimal rate)
        {
            if (currencyCode != "PLN")
                return String.Format("{0:0.00} {1}\r\n{2:0.00} PLN", priceBrutto * rate, currencyCode, priceBrutto);
            else
                return String.Format("{0:0.00} {1}", priceBrutto, currencyCode);

        }
        private string GetValue2(decimal priceBrutto, string currencyCode, decimal rate)
        {
            if (currencyCode != "PLN")
                return String.Format("{0:0.00} {1}", priceBrutto * rate, currencyCode);
            else
                return String.Format("{0:0.00} {1}", priceBrutto, currencyCode);

        }

        private void CreateInvoiceOrderTable(Dal.Invoice invoice, Dal.Invoice invoiceCorrection, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 8, Font.NORMAL);

            List<Dal.InvoiceProduct> products = oh.GetInvoiceProducts(invoice.InvoiceId);
            List<Dal.InvoiceProduct> productsCorrection = oh.GetInvoiceCorrectionProducts(invoice.InvoiceCorrectionId.Value);
            InittializeInvoiceCorrectionTable(STF_Helvetica_Polish, table);

            List<Dal.Helper.Amount> totalsBefore = new List<Dal.Helper.Amount>();
            List<Dal.Helper.Amount> totalsAfter = new List<Dal.Helper.Amount>();

            int quantity = 0;
            int i = 1;

            #region loop
            foreach (Dal.InvoiceProduct product in products)
            {
                Dal.InvoiceProduct invoiceCorrectionProduct = productsCorrection.Where(x => x.InvoiceProductId == product.InvoiceProductCorrectionId.Value).FirstOrDefault();

                if (
                    product.Name == invoiceCorrectionProduct.Name &&
                    product.Quantity == invoiceCorrectionProduct.Quantity &&
                    product.CalculatedTotalBrutto == invoiceCorrectionProduct.CalculatedTotalBrutto &&
                    product.VatRate == invoiceCorrectionProduct.VatRate &&
                    product.Rebate == invoiceCorrectionProduct.Rebate
                    )

                    continue;

                quantity += product.Quantity;

                Dal.Helper.Amount totalBefore = totalsBefore.Where(x => x.VATRate == product.VatRate).FirstOrDefault();
                if (totalBefore != null)
                {
                    totalBefore.VAT += product.CalculatedTotalVat.Value;
                    totalBefore.Netto += product.CalculatedTotalNetto.Value;
                    totalBefore.Brutto += product.CalculatedTotalBrutto.Value;
                }
                else
                {
                    totalBefore = new Dal.Helper.Amount()
                    {
                        VAT = product.CalculatedTotalVat.Value,
                        Netto = product.CalculatedTotalNetto.Value,
                        Brutto = product.CalculatedTotalBrutto.Value,
                        VATRate = product.VatRate
                    };
                    totalsBefore.Add(totalBefore);
                }

                Dal.Helper.Amount totalAfter = totalsAfter.Where(x => x.VATRate == invoiceCorrectionProduct.VatRate).FirstOrDefault();
                if (totalAfter != null)
                {
                    totalAfter.VAT += invoiceCorrectionProduct.CalculatedTotalVat.Value;
                    totalAfter.Netto += invoiceCorrectionProduct.CalculatedTotalNetto.Value;
                    totalAfter.Brutto += invoiceCorrectionProduct.CalculatedTotalBrutto.Value;
                }
                else
                {
                    totalAfter = new Dal.Helper.Amount()
                    {
                        VAT = invoiceCorrectionProduct.CalculatedTotalVat.Value,
                        Netto = invoiceCorrectionProduct.CalculatedTotalNetto.Value,
                        Brutto = invoiceCorrectionProduct.CalculatedTotalBrutto.Value,
                        VATRate = invoiceCorrectionProduct.VatRate
                    };
                    totalsAfter.Add(totalAfter);
                }


                totalBrutto += product.CalculatedTotalBrutto.Value;


                string productName = product.Name;
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, i.ToString(), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, productName, rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "Przed korektą", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0} {1}", product.Quantity.ToString(), product.MeasureName), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", product.Rebate), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", product.PriceBrutto), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", product.VatRate * 100M), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", product.CalculatedTotalVat), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", product.CalculatedTotalNetto), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", product.CalculatedTotalBrutto), rowCell));




                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, invoiceCorrectionProduct.Name, rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "Po korekcie", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0} {1}", invoiceCorrectionProduct.Quantity.ToString(), invoiceCorrectionProduct.MeasureName), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", invoiceCorrectionProduct.Rebate), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoiceCorrectionProduct.PriceBrutto), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", invoiceCorrectionProduct.VatRate * 100M), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoiceCorrectionProduct.CalculatedTotalVat), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoiceCorrectionProduct.CalculatedTotalNetto), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoiceCorrectionProduct.CalculatedTotalBrutto), rowCell));

                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "Korekta", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0} {1}", invoiceCorrectionProduct.Quantity - product.Quantity, invoiceCorrectionProduct.MeasureName), rowCell));
                //table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", invoiceCorrectionProduct.Rebate), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
                //table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoiceCorrectionProduct.PriceBrutto - product.PriceBrutto), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", invoiceCorrectionProduct.VatRate * 100M), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoiceCorrectionProduct.CalculatedTotalVat - product.CalculatedTotalVat), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoiceCorrectionProduct.CalculatedTotalNetto - product.CalculatedTotalNetto), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoiceCorrectionProduct.CalculatedTotalBrutto - product.CalculatedTotalBrutto), rowCell));

                i++;
            }

            #endregion

            totalCorrection = totalsAfter.Sum(x => x.Brutto) - totalsBefore.Sum(x => x.Brutto);

            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "Razem:", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "--", rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totalsAfter.Sum(x => x.VAT) - totalsBefore.Sum(x => x.VAT)), rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totalsAfter.Sum(x => x.Netto) - totalsBefore.Sum(x => x.Netto)), rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totalCorrection), rowCell, false));

            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "Razem przed korektą:", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "--", rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totalsBefore.Sum(x => x.VAT)), rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totalsBefore.Sum(x => x.Netto)), rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totalsBefore.Sum(x => x.Brutto)), rowCell, false));



            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "Razem  po korekcie:", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "--", rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totalsAfter.Sum(x => x.VAT)), rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totalsAfter.Sum(x => x.Netto)), rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totalsAfter.Sum(x => x.Brutto)), rowCell, false));

            List<decimal> vatRates = totalsBefore.Select(x => x.VATRate).Distinct().ToList();
            vatRates.AddRange(totalsAfter.Select(x => x.VATRate).Distinct().ToList());

            foreach (decimal rate in vatRates.Select(x => x).Distinct().OrderBy(x => x))
            {
                table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", rate * 100M), rowCell, false));
                decimal vat = totalsAfter.Where(x => x.VATRate == rate).Sum(x => x.VAT) - totalsBefore.Where(x => x.VATRate == rate).Sum(x => x.VAT);
                decimal netto = totalsAfter.Where(x => x.VATRate == rate).Sum(x => x.Netto) - totalsBefore.Where(x => x.VATRate == rate).Sum(x => x.Netto);
                decimal brutto = totalsAfter.Where(x => x.VATRate == rate).Sum(x => x.Brutto) - totalsBefore.Where(x => x.VATRate == rate).Sum(x => x.Brutto);
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", vat), rowCell, false));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", netto), rowCell, false));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", brutto), rowCell, false));
            }
        }

        public string GetInvoices(string[] files, string filesLocation)
        {
            return MergeFiles(files, filesLocation, path + @"\" + "Invoices_" + "_" + Guid.NewGuid().ToString() + ".pdf",
           new RectangleReadOnly(615, 1000));
        }

        /*
        private void CreateInvoiceOrderTable(Dal.Order order, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 8, Font.NORMAL);

            List<Dal.OrderProductsView> products = oh.GetOrderProducts(order.OrderId);
            InittializeInvoiceTable(STF_Helvetica_Polish, table);

            List<Dal.Helper.Amount> totals = new List<Dal.Helper.Amount>();

            int quantity = 0;
            //decimal totalNetto = 0;
            //decimal totalVat = 0;
            //decimal vat = 23.00M;
            //decimal vatRate = vat / 100M;
            int i = 1;

            foreach (Dal.OrderProductsView product in products.Where(x => x.Quantity > 0).ToList())
            {
                decimal rebate = 1 - product.Rebate / 100M;
                decimal brutto = product.Price.Value * product.Quantity * rebate;
                decimal netto = brutto / (1 + product.VAT);
                decimal vatValue = brutto - netto;

                quantity += product.Quantity;

                Dal.Helper.Amount total = totals.Where(x => x.VATRate == product.VAT).FirstOrDefault();
                if (total != null)
                {
                    total.VAT += vatValue;
                    total.Netto += netto;
                }
                else
                {
                    total = new Dal.Helper.Amount()
                    {
                        Netto = netto,
                        VAT = vatValue,
                        VATRate = product.VAT
                    };
                    totals.Add(total);
                }
                //totalNetto += netto;
                //totalVat += vatValue;
                totalBrutto += brutto;


                string productName = "";
                if (product.CodeSupplier == product.Code)
                    productName =
                     String.Format("{0} ({1})", product.CatalogName, product.Code);
                else
                    productName =
                     String.Format("{0} ({1} / {2})", product.CatalogName, product.Code, product.CodeSupplier);

                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, i.ToString(), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, productName, rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0} szt.", product.Quantity.ToString()), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", product.Rebate), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", product.Price), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", product.VAT * 100M), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", vatValue), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", netto), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", brutto), rowCell));

                i++;
            }

            if (order.ShippingCost.HasValue && order.ShippingCost.Value > 0)
            {
                decimal brutto = order.ShippingCost.Value;
                decimal netto = brutto / (1 + order.ShippingCostVAT);
                decimal vatValue = brutto - netto;


                quantity += 1;
                //totalNetto += netto;
                //totalVat += vatValue;

                Dal.Helper.Amount total = totals.Where(x => x.VATRate == order.ShippingCostVAT).FirstOrDefault();

                if (total != null)
                {
                    total.VAT += vatValue;
                    total.Netto += netto;
                }
                else
                {
                    total = new Dal.Helper.Amount()
                    {
                        Netto = netto,
                        VAT = vatValue,
                        VATRate = order.ShippingCostVAT
                    };
                    totals.Add(total);
                }


                totalBrutto += brutto;

                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, i.ToString(), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "Przesyłka", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0} szt.","1"), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", 0), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", order.ShippingCost), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", order.ShippingCostVAT * 100M), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", vatValue), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", netto), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", brutto), rowCell));


            }



            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "Razem:", rowCell, true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "--", rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totals.Sum(x=>x.VAT)), rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totals.Sum(x => x.Netto)), rowCell, false));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", totals.Sum(x => x.VAT + x.Netto)), rowCell, false));

            foreach (Dal.Helper.Amount total in totals.OrderBy(x=>x.VATRate))
            {
                table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", rowCell, true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", total.VATRate*100M), rowCell, false));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", total.VAT), rowCell, false));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", total.Netto), rowCell, false));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", total.VAT + total.Netto), rowCell, false));
            }
        }
         */

        private static void InittializeInvoiceCorrectionTable(iTextSharp.text.pdf.BaseFont STF_Helvetica_Polish, PdfPTable table)
        {


            Font headerCell = new Font(STF_Helvetica_Polish, 8, Font.BOLD);

            //PdfPCell cell = new PdfPCell(new Phrase("Header spanning 3 columns"));
            //cell.Colspan = 3;
            //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //table.AddCell(cell);

            table.SetWidthPercentage(new float[] { 20f, 220f, 40f, 30f, 70f, 70f, 70f, 70f, 70f, 70f }, PageSize.A4);
            table.AddCell(CreateHeaderCell("Lp", headerCell));
            table.AddCell(CreateHeaderCell("Nazwa towaru", headerCell));
            table.AddCell(CreateHeaderCell("Korekta", headerCell));
            table.AddCell(CreateHeaderCell("Ilość", headerCell));
            table.AddCell(CreateHeaderCell("Rabat", headerCell));
            table.AddCell(CreateHeaderCell("Cena brutto", headerCell));
            table.AddCell(CreateHeaderCell("VAT [%]", headerCell));
            table.AddCell(CreateHeaderCell("Wartość Vat", headerCell));
            table.AddCell(CreateHeaderCell("Wartość netto", headerCell));
            table.AddCell(CreateHeaderCell("Wartość brutto", headerCell));

            // table.SetWidths
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
        }

        private static void InittializeInvoiceTable(iTextSharp.text.pdf.BaseFont STF_Helvetica_Polish, PdfPTable table, List<Dal.SystemDictionary> dictionary)
        {


            Font headerCell = new Font(STF_Helvetica_Polish, 8, Font.BOLD);

            //PdfPCell cell = new PdfPCell(new Phrase("Header spanning 3 columns"));
            //cell.Colspan = 3;
            //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //table.AddCell(cell);

            table.SetWidthPercentage(new float[] { 20f, 270f, 30f, 70f, 70f, 70f, 70f, 70f, 70f }, PageSize.A4);
            table.AddCell(CreateHeaderCell(GetDictionary("FakturaTabelaLp", dictionary),                   headerCell));
            table.AddCell(CreateHeaderCell(GetDictionary("FakturaTabelaNazwa", dictionary),                           headerCell));
            table.AddCell(CreateHeaderCell(GetDictionary("FakturaTabelaIlosc", dictionary),                    headerCell));
            table.AddCell(CreateHeaderCell(GetDictionary("FakturaTabelaRabat", dictionary),                    headerCell));
            table.AddCell(CreateHeaderCell(GetDictionary("FakturaTabelaBrutto", dictionary),                          headerCell));
            table.AddCell(CreateHeaderCell(GetDictionary("FakturaTabelaVAT", dictionary),                     headerCell));
            table.AddCell(CreateHeaderCell(GetDictionary("FakturaTabelaVATWartosc", dictionary),                         headerCell));
            table.AddCell(CreateHeaderCell(GetDictionary("FakturaTabelaWartoscNetto", dictionary),                         headerCell));
            table.AddCell(CreateHeaderCell(GetDictionary("FakturaTabelaWartoscBrutto", dictionary),                          headerCell));

            // table.SetWidths
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
        }
        #endregion
         
        public string ProductCatalogWarehouseStock(List<Dal.ProductCatalogStockMonthEndResult> stock, DateTime month)
        {
            doc1 = new Document(PageSize.A4_LANDSCAPE);
            string invoiceFile = path + @"\" + "Magazyn_" + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();

            Dal.Company company = Dal.DbHelper.Accounting.GetCompany(78);

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);


            //  doc1.Add(CreateParagraph("Miejsce wystawienia: PL, Łódź", fontText, Element.ALIGN_RIGHT));
            doc1.Add(CreateParagraph(String.Format("{0}", company.Name), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(String.Format("Magazyn na koniec {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));

            Font headerCell = new Font(STF_Helvetica_Polish, 8, Font.BOLD);
            PdfPTable table = new PdfPTable(5);

            table.SetWidthPercentage(new float[] {  100f, /*100f,*/ 100f, 60f, 70f, 70f }, PageSize.A4_LANDSCAPE);
             

            // table.AddCell(CreateHeaderCell("Nazwa", headerCell));
            table.AddCell(CreateHeaderCell("Kod", headerCell));
            table.AddCell(CreateHeaderCell("Dostawca", headerCell));
            table.AddCell(CreateHeaderCell("Cena", headerCell));
            table.AddCell(CreateHeaderCell("Ilość", headerCell));
            table.AddCell(CreateHeaderCell("Wartość", headerCell));

            int qunatity = 0;
            decimal total = 0;
            foreach (Dal.ProductCatalogStockMonthEndResult s in stock)
            {
                CreateWarehouseTable(s, table);
                total += s.TotalValue.Value;
                qunatity += s.LeftQuantity.Value;
            }

             
             
            table.AddCell(CreateHeaderCell("", headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));
            Font rowCell = new Font(STF_Helvetica_Polish, 7, Font.NORMAL);
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, qunatity.ToString(), headerCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, total.ToString("C"), headerCell));

            doc1.Add(table);
            doc1.Close();

            return invoiceFile;
        }
        private void CreateWarehouseTable(Dal.ProductCatalogStockMonthEndResult stock, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 7, Font.NORMAL);
             

            //table.AddCell(CreateRowCell(Element.ALIGN_CENTER, delivery.Name, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, stock.Code, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, stock.Name, rowCell));

            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, stock.Price.Value.ToString("C"), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, stock.LeftQuantity.ToString(), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, stock.TotalValue.Value.ToString("C"), rowCell));

        }
        public string OrderProductsSent(List<Dal.OrderProductsSentView> ops, DateTime month)
        {
            doc1 = new Document(PageSize.A4_LANDSCAPE);
            string invoiceFile = path + @"\" + "Wz" + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();

            Dal.Company company = Dal.DbHelper.Accounting.GetCompany(78);

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);


            //  doc1.Add(CreateParagraph("Miejsce wystawienia: PL, Łódź", fontText, Element.ALIGN_RIGHT));
            doc1.Add(CreateParagraph(String.Format("{0}", company.Name), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(String.Format("Magazyn - wydania {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));

            Font headerCell = new Font(STF_Helvetica_Polish, 8, Font.BOLD);
            PdfPTable table = new PdfPTable(8);

            table.SetWidthPercentage(new float[] { 60f, 110f, 80f, 60f, 30f, 70f, 70f, 80f }, PageSize.A4_LANDSCAPE);

            table.AddCell(CreateHeaderCell("Data przyjęcia", headerCell));
            table.AddCell(CreateHeaderCell("Kod", headerCell));
            table.AddCell(CreateHeaderCell("Dostawca", headerCell));
            table.AddCell(CreateHeaderCell("Cena", headerCell));
            table.AddCell(CreateHeaderCell("Ilość", headerCell));
            table.AddCell(CreateHeaderCell("Wartość netto", headerCell));
            table.AddCell(CreateHeaderCell("Wartość brutto", headerCell));
            table.AddCell(CreateHeaderCell("Nr zamówienia", headerCell));

            int qunatity = 0;
            decimal total = 0;
            decimal totalBrutto = 0;
            foreach (Dal.OrderProductsSentView op in ops)
            {
                if (op.Price != null && op.Quantity != null)
                {
                    CreateOrderProductsSentTable(op, table);
                    total += (decimal)op.Netto;
                    totalBrutto += (decimal)op.Brutto;
                    qunatity += (int)op.Quantity;
                }
            }


            table.AddCell(CreateHeaderCell("", headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));
            Font rowCell = new Font(STF_Helvetica_Polish, 7, Font.NORMAL);
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, qunatity.ToString(), headerCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, total.ToString("C"), headerCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, totalBrutto.ToString("C"), headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));

            doc1.Add(table);
            doc1.Close();

            return invoiceFile;
        }
        private void CreateOrderProductsSentTable(Dal.OrderProductsSentView ops, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 7, Font.NORMAL);

            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ops.InsertDate.ToString("yyyy/MM/dd"), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ops.Code, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ops.Name, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, ops.Price.Value.ToString("C"), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ops.Quantity.ToString(), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, ops.Netto.Value.ToString("C"), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, ops.Brutto.Value.ToString("C"), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ops.OrderId.ToString(), rowCell));
        }
        public string ProductCatalogDelivery(List<Dal.ProductCatalogDeliveryWarehouseViewWithPrice> deliveries, DateTime month)
        {
            doc1 = new Document(PageSize.A4_LANDSCAPE);
            string invoiceFile = path + @"\" + "Pz" + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();

            Dal.Company company = Dal.DbHelper.Accounting.GetCompany(78);

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);


            //  doc1.Add(CreateParagraph("Miejsce wystawienia: PL, Łódź", fontText, Element.ALIGN_RIGHT));
            doc1.Add(CreateParagraph(String.Format("{0}", company.Name), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(String.Format("Magazyn - przyjęcia {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));

            Font headerCell = new Font(STF_Helvetica_Polish, 8, Font.BOLD);
            PdfPTable table = new PdfPTable(6);

            table.SetWidthPercentage(new float[] { 60f, 130f, 100f, 30f, 70f, 70f }, PageSize.A4_LANDSCAPE);

            table.AddCell(CreateHeaderCell("Data przyjęcia", headerCell));

            // table.AddCell(CreateHeaderCell("Nazwa", headerCell));
            table.AddCell(CreateHeaderCell("Kod", headerCell));
            table.AddCell(CreateHeaderCell("Dostawca", headerCell));
            table.AddCell(CreateHeaderCell("Cena", headerCell));
            table.AddCell(CreateHeaderCell("Ilość", headerCell));
            table.AddCell(CreateHeaderCell("Wartość", headerCell));

            int qunatity = 0;
            decimal total = 0;
            foreach (Dal.ProductCatalogDeliveryWarehouseViewWithPrice delivery in deliveries)
            {
                CreateProductCatalogDeliveryTable(delivery, table);
                total += delivery.Quantity * delivery.Price;
                qunatity += delivery.Quantity;
            }


            table.AddCell(CreateHeaderCell("", headerCell));

            // table.AddCell(CreateHeaderCell("Nazwa", headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));
            Font rowCell = new Font(STF_Helvetica_Polish, 7, Font.NORMAL);
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, qunatity.ToString(), headerCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, total.ToString("C"), headerCell));

            doc1.Add(table);
            doc1.Close();

            return invoiceFile;
        }
        private void CreateProductCatalogDeliveryTable(Dal.ProductCatalogDeliveryWarehouseViewWithPrice delivery, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 7, Font.NORMAL);

            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, delivery.InsertDate.Value.ToString("yyyy/MM/dd"), rowCell));

            //table.AddCell(CreateRowCell(Element.ALIGN_CENTER, delivery.Name, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, delivery.Code, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, delivery.SupplierName, rowCell));

            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, delivery.Price.ToString("C"), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, delivery.Quantity.ToString(), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, (delivery.Quantity * delivery.Price).ToString("C"), rowCell));

        }
        public string ProductCatalogWarehouse(List<Dal.ProductCatalogDeliveryInvoiceView> deliveries, DateTime month)
        {
            doc1 = new Document(PageSize.A4_LANDSCAPE);
            string invoiceFile = path + @"\" + "Magazyn_" + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();

            Dal.Company company = Dal.DbHelper.Accounting.GetCompany(78);

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);


            //  doc1.Add(CreateParagraph("Miejsce wystawienia: PL, Łódź", fontText, Element.ALIGN_RIGHT));
            doc1.Add(CreateParagraph(String.Format("{0}", company.Name), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(String.Format("Magazyn {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));

            Font headerCell = new Font(STF_Helvetica_Polish, 8, Font.BOLD);
            PdfPTable table = new PdfPTable(6);

            table.SetWidthPercentage(new float[] { 60f, 100f, /*100f,*/100f, 60f, 70f, 70f }, PageSize.A4_LANDSCAPE);

            table.AddCell(CreateHeaderCell("Data przyjęcia", headerCell));

            // table.AddCell(CreateHeaderCell("Nazwa", headerCell));
            table.AddCell(CreateHeaderCell("Kod", headerCell));
            table.AddCell(CreateHeaderCell("Dostawca", headerCell));
            table.AddCell(CreateHeaderCell("Cena", headerCell));
            table.AddCell(CreateHeaderCell("Ilość", headerCell));
            table.AddCell(CreateHeaderCell("Wartość", headerCell));

            int qunatity = 0;
            decimal total = 0;
            foreach (Dal.ProductCatalogDeliveryInvoiceView delivery in deliveries)
            {
                CreateWarehouseTable(delivery, table);
                total += delivery.Quantity * delivery.FinalPurchasePrice.Value;
                qunatity += delivery.Quantity;
            }


            table.AddCell(CreateHeaderCell("", headerCell));

            // table.AddCell(CreateHeaderCell("Nazwa", headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));
            table.AddCell(CreateHeaderCell("", headerCell));
            Font rowCell = new Font(STF_Helvetica_Polish, 7, Font.NORMAL);
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, qunatity.ToString(), headerCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, total.ToString("C"), headerCell));

            doc1.Add(table);
            doc1.Close();

            return invoiceFile;
        }
        private void CreateWarehouseTable(Dal.ProductCatalogDeliveryInvoiceView delivery, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 7, Font.NORMAL);

            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, delivery.insertDate.Value.ToString("yyyy/MM/dd"), rowCell));

            //table.AddCell(CreateRowCell(Element.ALIGN_CENTER, delivery.Name, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, delivery.Code, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, delivery.SupplierName, rowCell));

            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, delivery.FinalPurchasePrice.Value.ToString("C"), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, delivery.Quantity.ToString(), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, (delivery.Quantity * delivery.FinalPurchasePrice.Value).ToString("C"), rowCell));

        }
        public string ShopPaymentsStatement(int shopId, List<Dal.ShopPayment> payments , DateTime month, bool perShop)
        {
            doc1 = new Document(PageSize.A4_LANDSCAPE);
            string invoiceFile = path + @"\" + "ShopPayments_" + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();

            Dal.Company company = Dal.DbHelper.Accounting.GetCompany(78);

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);


            //  doc1.Add(CreateParagraph("Miejsce wystawienia: PL, Łódź", fontText, Element.ALIGN_RIGHT));
            doc1.Add(CreateParagraph(String.Format("{0}", company.Name), fontText, Element.ALIGN_CENTER));
            switch (shopId)
            {
                case 0:
                    doc1.Add(CreateParagraph(String.Format("Wyciąg z wpłat DPD {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER)); break;
                case 1:
                    doc1.Add(CreateParagraph(String.Format("Wyciąg z rachunku Przelewy24 {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER)); break;
                case 6:
                    doc1.Add(CreateParagraph(String.Format("Wyciąg z rachunku Ceneo {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER)); break;
                case 13:
                    doc1.Add(CreateParagraph(String.Format("Wyciąg z rachunku terminala kart {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER)); break;
                case 22:
                    doc1.Add(CreateParagraph(String.Format("Wyciąg z rachunku Empik {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER)); break;
                default:
                    doc1.Add(CreateParagraph(String.Format("Wyciąg z rachunku Allegro {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER)); break;
            }
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));

            Font headerCell = new Font(STF_Helvetica_Polish, 8, Font.BOLD);
            PdfPTable table = new PdfPTable(perShop?7:8);
            if (!perShop)
                table.SetWidthPercentage(new float[] { 60f, 100f, 100f, 100f, 60f, 70f, 70f, 70f }, PageSize.A4_LANDSCAPE);
            else
                table.SetWidthPercentage(new float[] { 60f, 100f, 100f, 60f, 70f, 70f, 70f }, PageSize.A4_LANDSCAPE);

            table.AddCell(CreateHeaderCell("Data księgowania", headerCell));
            if(!perShop)
                table.AddCell(CreateHeaderCell("Konto", headerCell));
            table.AddCell(CreateHeaderCell("Nazwa konta", headerCell));
            table.AddCell(CreateHeaderCell("Typ płatności", headerCell));
            table.AddCell(CreateHeaderCell("Operator płatności", headerCell));
            table.AddCell(CreateHeaderCell("Kwota", headerCell));
            table.AddCell(CreateHeaderCell("Saldo konta", headerCell));
            table.AddCell(CreateHeaderCell("Rozliczono", headerCell));


            foreach (Dal.ShopPayment ba in payments)
            {
                CreateBanStatementTable(ba, table, perShop);

            }


            //CreateInvoicesSummaryTable(list, table);

            doc1.Add(table);
            doc1.Close();

            return invoiceFile;
        }
        private void CreateBanStatementTable(Dal.ShopPayment ba, PdfPTable table, bool perShop)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 6, Font.NORMAL);

            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ba.PaymentDate.ToString("yyyy/MM/dd"), rowCell));
            if(!perShop)
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ba.Shop.Name, rowCell));

            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ba.ClientName, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ba.ShopPaymentType.Name, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ba.PaymentOperator, rowCell));

            if (ba.PaymentTypeId == 3)
            {
                PdfPCell cell = CreateRowCell(Element.ALIGN_RIGHT, ba.Amount.ToString("C"), rowCell);
                cell.BackgroundColor = BaseColor.PINK;
                table.AddCell(cell);
            }
            else
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, ba.Amount.ToString("C"), rowCell));
            if (ba.TotalAmount.HasValue)
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, ba.TotalAmount.Value.ToString("C"), rowCell));
            else
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
            if (ba.PaymentTypeId == 1 && ba.OrderPayment != null && ba.OrderPayment.OrderPaymentAccountingType!=null)
            {
                if (ba.OrderPayment != null && ba.OrderPayment.Order.Invoice!=null && ba.OrderPayment.Order.Invoice.InvoiceNumber != null)
                    table.AddCell(CreateRowCell(Element.ALIGN_LEFT, String.Format("{0} {1}", ba.OrderPayment.OrderPaymentAccountingType.Name, ba.OrderPayment.Order.Invoice.InvoiceNumber), rowCell));
                else
                    table.AddCell(CreateRowCell(Element.ALIGN_LEFT, ba.OrderPayment.OrderPaymentAccountingType.Name, rowCell));
            }
            else
            {
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "", rowCell));

            }

        }
 
        public string DpdPayments(List<DpdPaymentsView> dpdPayments, int companyId, DateTime month)
        {
            doc1 = new Document(PageSize.A4_LANDSCAPE);
            string invoiceFile = path + @"\" + "DpdPayments_" + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();

            Dal.Company company = Dal.DbHelper.Accounting.GetCompany(companyId);

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);


            //  doc1.Add(CreateParagraph("Miejsce wystawienia: PL, Łódź", fontText, Element.ALIGN_RIGHT));
            doc1.Add(CreateParagraph(String.Format("{0}", company.Name), fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph(String.Format("Rozliczenie przelewów DPD {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));

            Font headerCell = new Font(STF_Helvetica_Polish, 6, Font.BOLD);
            PdfPTable table = new PdfPTable(4);
            table.SetWidthPercentage(new float[] { 60f, 60f, 60f, 170f }, PageSize.A4_LANDSCAPE);
            
            table.AddCell(CreateHeaderCell("Data przelewu", headerCell));
            table.AddCell(CreateHeaderCell("Kwota", headerCell));
            table.AddCell(CreateHeaderCell("Kwota całkowita", headerCell));
            table.AddCell(CreateHeaderCell("Rozliczono", headerCell));



            foreach (Dal.DpdPaymentsView ba in dpdPayments)
            {
                CreateDpdStatementTable(ba, table);

            }


            //CreateInvoicesSummaryTable(list, table);

            doc1.Add(table);
            doc1.Close();

            return invoiceFile;
        }
        private void CreateDpdStatementTable(Dal.DpdPaymentsView ba, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 6, Font.NORMAL);

            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ba.PaymentDate.ToString("yyyy/MM/dd"), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, ba.DpdAmount.ToString("C"), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, ba.DpdTotalAmount.ToString("C"), rowCell));


            if (ba.InvoiceNumber != null)
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, String.Format("{0} {1}", ba.AccountingName, ba.InvoiceNumber), rowCell));
            else
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, ba.AccountingName, rowCell));


        }
        public string BankStatement(List<Dal.BankAccountView> bankAccounts, int companyId, int accountId, DateTime month)
        {
            doc1 = new Document(PageSize.A4_LANDSCAPE);
            string invoiceFile = path + @"\" + "BankStatement_" + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();

            Dal.Company company = Dal.DbHelper.Accounting.GetCompany(companyId);

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);


            //  doc1.Add(CreateParagraph("Miejsce wystawienia: PL, Łódź", fontText, Element.ALIGN_RIGHT));
            doc1.Add(CreateParagraph(String.Format("{0}", company.Name), fontText, Element.ALIGN_CENTER));

            switch(accountId)
            {
                case 1:
                    doc1.Add(CreateParagraph(String.Format("Wyciąg bankowy {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER));
                    break;
                case 2:
                    doc1.Add(CreateParagraph(String.Format("Wyciąg Przelewy24 {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER));
                    break;
            }


            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));

            Font headerCell = new Font(STF_Helvetica_Polish, 6, Font.BOLD);
            PdfPTable table = new PdfPTable(6);
            table.SetWidthPercentage(new float[] {40f, 60f, 170f, 170f, 70f, 70f}, PageSize.A4_LANDSCAPE);
            table.AddCell(CreateHeaderCell("Typ płatności", headerCell));
            table.AddCell(CreateHeaderCell("Data księgowania", headerCell));
            table.AddCell(CreateHeaderCell("Kontrahent", headerCell));
            table.AddCell(CreateHeaderCell("Tytuł przelewu", headerCell));
            table.AddCell(CreateHeaderCell("Kwota", headerCell));
            table.AddCell(CreateHeaderCell("Rozliczono", headerCell)); 



            foreach (Dal.BankAccountView ba in bankAccounts)
            {
                CreateBanStatementTable(ba, table);

            }


            //CreateInvoicesSummaryTable(list, table);

            doc1.Add(table);
            doc1.Close();

            return invoiceFile;
        }
        private void CreateBanStatementTable(Dal.BankAccountView ba, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 7, Font.NORMAL);

            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ba.TransferType, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, ba.PaymentDate.ToString("yyyy/MM/dd"), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_LEFT, ba.ClientName, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_LEFT, ba.Comment, rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, ba.Amount.ToString("C"), rowCell));
            if (ba.InvoiceNumber !=null)
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT,String.Format("{0} {1}", ba.AccoutingTypeName, ba.InvoiceNumber), rowCell));
            else
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, ba.AccoutingTypeName, rowCell));


        }
        public string OrderPaymentsReport(List<Dal.OrderPaymentsView> list, DateTime month, string title)
        {
            doc1 = new Document(PageSize.A4_LANDSCAPE);
            string invoiceFile = path + @"\" + "Payments_" + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);


            //  doc1.Add(CreateParagraph("Miejsce wystawienia: PL, Łódź", fontText, Element.ALIGN_RIGHT));
            //doc1.Add(CreateParagraph(String.Format("{0}", list.FirstOrDefault().), fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph(String.Format("{1} {0:yyyy/MM}", month, title), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));

            Font headerCell = new Font(STF_Helvetica_Polish, 6, Font.BOLD);
            PdfPTable table = new PdfPTable(6);
            table.SetWidthPercentage(new float[] { 70f, 70f, 70f, 70f, 70f, 50f }, PageSize.A4_LANDSCAPE);
            
            table.AddCell(CreateHeaderCell("Data zwrotu", headerCell));
            table.AddCell(CreateHeaderCell("Kwota brutto", headerCell));
            table.AddCell(CreateHeaderCell("Kwota netto", headerCell));
            table.AddCell(CreateHeaderCell("Wartość VAT", headerCell));
            table.AddCell(CreateHeaderCell("Rozliczono", headerCell));
            table.AddCell(CreateHeaderCell("Nr zamówienia", headerCell));



            foreach (Dal.OrderPaymentsView payment in list)
            {
                CreatePaymentTable(payment, table);

            }


            Font rowCell = new Font(STF_Helvetica_Polish, 6, Font.BOLD);

            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, "", rowCell));

            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", list.Sum(x => x.Amount)), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", list.Sum(x => x.Amount / (1 + x.VAT))), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", list.Sum(x => x.Amount - x.Amount / (1 + x.VAT))), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "", rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "", rowCell));

            doc1.Add(table);
            doc1.Close();

            return invoiceFile;
        }

        private void CreatePaymentTable(Dal.OrderPaymentsView payment, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 6, Font.NORMAL);

            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:yyyy/MM/dd} ", payment.InsertDate), rowCell));

            if (payment.AccountingTypeId != (int)Dal.Helper.OrderPaymentAccoutingType.Evidence)
            {
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
            }
            else
            {
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", payment.Amount), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", payment.Amount / (1 + payment.VAT)), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", payment.Amount - payment.Amount / (1 + payment.VAT)), rowCell));
            }
            table.AddCell(CreateRowCell(Element.ALIGN_LEFT, payment.AccountingName, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0}", payment.OrderId), rowCell));

        }
        public string InvoicesReport(List<Dal.InvoicesView> list, DateTime month)
        {
            PdfPTable mainTable = new PdfPTable(1);
            mainTable.DefaultCell.Border = Rectangle.NO_BORDER;

            mainTable.SetWidthPercentage(new float[] { 580f }, PageSize.A4_LANDSCAPE);


          //  doc1 = new Document(PageSize.A4_LANDSCAPE);
            string invoiceFile = path + @"\" + "InvoicesReport_" + "_" + Guid.NewGuid().ToString() + ".pdf";
            //PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            //doc1.Open();

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);



            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_CENTER, String.Format("{0}", list.FirstOrDefault().SellectCompanyName), fontText));
            mainTable.AddCell(CreateRowCellNoBorder(Element.ALIGN_CENTER, String.Format("Faktury {0:yyyy/MM}", month), fontText));
        

 

            Font headerCell = new Font(STF_Helvetica_Polish, 6, Font.BOLD);
            PdfPTable table = new PdfPTable(12);
            table.SetWidthPercentage(new float[] { 40f, 40f, 110f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 60f }, PageSize.A4_LANDSCAPE);
            table.AddCell(CreateHeaderCell("Nr. faktury", headerCell));
            table.AddCell(CreateHeaderCell("Data wyst.\nData sprz.", headerCell));
            table.AddCell(CreateHeaderCell("Dane firmy", headerCell));
            table.AddCell(CreateHeaderCell("NIP", headerCell));
            table.AddCell(CreateHeaderCell("Wartość brutto", headerCell));
            table.AddCell(CreateHeaderCell("Wartość netto", headerCell));
            table.AddCell(CreateHeaderCell("Wartość VAT", headerCell));
            table.AddCell(CreateHeaderCell("Netto dla VAT [0%]", headerCell));
            table.AddCell(CreateHeaderCell("VAT [0%]", headerCell));
            table.AddCell(CreateHeaderCell("Netto dla VAT [23%]", headerCell));
            table.AddCell(CreateHeaderCell("VAT [23%]", headerCell));
            table.AddCell(CreateHeaderCell("Uwagi", headerCell));



            foreach (Dal.InvoicesView invoice in list)
            {
                CreateInvoicesTable(invoice, table);

            }


            CreateInvoicesSummaryTable(list, table);
            mainTable.AddCell(table);


            PdfCreator.CreatePdf(iTextSharp.text.PageSize.A4_LANDSCAPE, mainTable, "", invoiceFile, true);


            return invoiceFile;
        }

        private void CreateInvoicesSummaryTable(List<Dal.InvoicesView> invoices, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 6, Font.BOLD);

            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, "", rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "", rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "", rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoices.Where(x => x.AccountingTypeId.HasValue == false || x.AccountingTypeId.Value == 2).Sum(x => x.CalculatedTotalBrutto)), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoices.Where(x => x.AccountingTypeId.HasValue == false || x.AccountingTypeId.Value == 2).Sum(x => x.CalculatedTotalNetto)), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoices.Where(x => x.AccountingTypeId.HasValue == false || x.AccountingTypeId.Value == 2).Sum(x => x.CalculatedTotalVat)), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoices.Where(x => x.AccountingTypeId.HasValue == false || x.AccountingTypeId.Value == 2).Sum(x => x.CalculatedTotalNetto000)), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoices.Where(x => x.AccountingTypeId.HasValue == false || x.AccountingTypeId.Value == 2).Sum(x => x.CalculatedTotalVat000)), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoices.Where(x => x.AccountingTypeId.HasValue == false || x.AccountingTypeId.Value == 2).Sum(x => x.CalculatedTotalNetto023)), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoices.Where(x => x.AccountingTypeId.HasValue == false || x.AccountingTypeId.Value == 2).Sum(x => x.CalculatedTotalVat023)), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "", rowCell));


        }
        private void CreateInvoicesTable(Dal.InvoicesView invoice, PdfPTable table)
        {

            Font rowCell = new Font(STF_Helvetica_Polish, 6, Font.NORMAL);

            table.AddCell(CreateRowCell(Element.ALIGN_CENTER, invoice.InvoiceNumber, rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:yyyy/MM/dd}\n{1:yyyy/MM/dd}", invoice.InvoiceDate, invoice.InvoiceSellDate), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_LEFT, String.Format("{0}\n{1}", invoice.CompanyName, invoice.CompanyAddress), rowCell));
            table.AddCell(CreateRowCell(Element.ALIGN_LEFT, invoice.Nip, rowCell));

            if (invoice.AccountingType!=null && invoice.AccountingTypeId.Value!=2)
            {
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, "", rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, invoice.AccountingType, rowCell));
            }
            else
            {
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoice.CalculatedTotalBrutto), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoice.CalculatedTotalNetto), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoice.CalculatedTotalVat), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoice.CalculatedTotalNetto000), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoice.CalculatedTotalVat000), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoice.CalculatedTotalNetto023), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", invoice.CalculatedTotalVat023), rowCell));
                table.AddCell(CreateRowCell(Element.ALIGN_LEFT, "", rowCell));
            }


        }

        public string SellSummaryReport(List<Dal.OrderPaymentsView> payments, DateTime month)
        {
            doc1 = new Document(PageSize.A4);
            string invoiceFile = path + @"\" + "SellSummaryReport_" + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);


            //  doc1.Add(CreateParagraph("Miejsce wystawienia: PL, Łódź", fontText, Element.ALIGN_RIGHT));
            doc1.Add(CreateParagraph(String.Format("Ewidencja sprzedaży {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));

            Font headerCell = new Font(STF_Helvetica_Polish, 12, Font.BOLD);
            PdfPTable table = new PdfPTable(3);
            table.SetWidthPercentage(new float[] { 170f, 170f, 170f }, PageSize.A4);
            table.AddCell(CreateHeaderCell("VAT", headerCell));
            table.AddCell(CreateHeaderCell("Netto", headerCell));
            table.AddCell(CreateHeaderCell("Brutto", headerCell));

            decimal brutto = payments.Sum(x => x.Amount);
            decimal netto = brutto / (1 + Dal.Helper.VAT);
            decimal vat = brutto - netto;


            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", vat), headerCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", netto), headerCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", brutto), headerCell));

            doc1.Add(table);

            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));

            //if (result.Compensation.HasValue && result.Compensation.Value > 0)
            //{
            //    table = new PdfPTable(1);
            //    table.SetWidthPercentage(new float[] { 370f }, PageSize.A4);

            //    table.AddCell(CreateHeaderCell("Wpływy z tytułu odszkodowań", headerCell));

            //    table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", result.Compensation), headerCell));

            //    doc1.Add(table);
            //}


            doc1.Close();

            return invoiceFile;
        }


        //public static string MergeFiles(string[] sourceFiles, string filesLocation, string destinationFile, Rectangle pageSize)
        //{


        //    if (System.IO.File.Exists(destinationFile))
        //        System.IO.File.Delete(destinationFile);

        //    if (sourceFiles.Length == 1)
        //    {
        //        File.Copy(String.Format("{0}\\{1}", filesLocation, sourceFiles[0].ToString()), destinationFile);
        //        return destinationFile;
        //    }

        //    //    sSrcFile = new string[2];
        //    sourceFiles = sourceFiles.Where(x => x != null).ToArray();
        //    for (int ic = 0; ic < sourceFiles.Length; ic++)
        //    {
        //        sourceFiles[ic] = String.Format("{0}\\{1}", filesLocation, sourceFiles[ic].ToString());
        //    }


        //    using (var destinationDocumentStream = new FileStream(destinationFile, FileMode.Create))
        //    {
        //        var pdfConcat = new PdfConcatenate(destinationDocumentStream);
        //        foreach (string file in sourceFiles)
        //        {
        //            using (var sourceDocumentStream1 = new FileStream(file, FileMode.Open, FileAccess.Read))
        //            {

        //                var pdfReader = new PdfReader(file);

        //                var pages = new List<int>();
        //                for (int i = 0; i < pdfReader.NumberOfPages; i++)
        //                {
        //                    pages.Add(i);
        //                }
                        
        //                pdfReader.SelectPages(pages);
                         
        //                pdfConcat.AddPages(pdfReader);



        //                pdfReader.Close();
        //            }
        //        }
        //        pdfConcat.Close();
        //    }

        //    return destinationFile;
        //}
        public static string MergeFiles(string[] sourceFiles, string filesLocation, string destinationFile, Rectangle pageSize)
        {
          

            if (System.IO.File.Exists(destinationFile))
                System.IO.File.Delete(destinationFile);

            if (sourceFiles.Length == 1)
            {
                File.Copy(String.Format("{0}\\{1}", filesLocation, sourceFiles[0].ToString()), destinationFile);
                return destinationFile;
            }

            //string[] sSrcFile;
            //sSrcFile = new string[2];

            //string[] arr = new string[2];
            //for (int i = 0; i <= sourceFiles.Length - 1; i++)
            //{
            //    if (sourceFiles[i] != null)
            //    {
            //        if (sourceFiles[i].Trim() != "")
            //            arr[i] = sourceFiles[i].ToString();
            //    }
            //}

            //if (arr != null)
            //{
            //    sSrcFile = new string[2];
            sourceFiles = sourceFiles.Where(x => x != null).ToArray();
            for (int ic = 0; ic < sourceFiles.Length; ic++)
            {
                sourceFiles[ic] = String.Format("{0}\\{1}", filesLocation, sourceFiles[ic].ToString());
            }
            //}
            try
            {
                int f = 0;

                PdfReader reader = new PdfReader(sourceFiles[f]);
                int n = reader.NumberOfPages;
                Document document = new Document(pageSize);

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
                   destinationFile,
                    FileMode.Create));

                document.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                int rotation;
                while (f < sourceFiles.Length)
                {
                    int i = 0;
                    while (i < n)
                    {
                        i++;

                        document.SetPageSize(pageSize); 
                        document.NewPage();
                        page = writer.GetImportedPage(reader, i);

                        rotation = reader.GetPageRotation(i);
                        if (rotation == 90 || rotation == 270)
                        {
                            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                        }
                        else
                        { 
                            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                        }
                    }

                    f++;
                    if (f < sourceFiles.Length)
                    {
                        reader = new PdfReader(sourceFiles[f]);
                        n = reader.NumberOfPages;
                    }
                }
                document.Close();
                return destinationFile;

            }
            catch (Exception ex)
            {
                ErrorHandler.SendError(ex, "Pdf merge file");
                return null;
            }


        }


        public string SellSummaryReport(List<Dal.Accounting2Result> results, DateTime month)
        {
            doc1 = new Document(PageSize.A4);
            string invoiceFile = path + @"\" + "SellSummaryReport_" + "_" + Guid.NewGuid().ToString() + ".pdf";
            PdfWriter.GetInstance(doc1, new FileStream(invoiceFile, FileMode.Create));
            doc1.Open();

            iTextSharp.text.Font fontText = new iTextSharp.text.Font(STF_Helvetica_Polish, 18, iTextSharp.text.Font.NORMAL);


            //  doc1.Add(CreateParagraph("Miejsce wystawienia: PL, Łódź", fontText, Element.ALIGN_RIGHT));
            doc1.Add(CreateParagraph(String.Format("{0}", results.FirstOrDefault().CompanyName), fontText, Element.ALIGN_CENTER));
            doc1.Add(CreateParagraph(String.Format("Ewidencja sprzedaży {0:yyyy/MM}", month), fontText, Element.ALIGN_CENTER));

            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));

            Font headerCell = new Font(STF_Helvetica_Polish, 12, Font.BOLD);
            PdfPTable table = new PdfPTable(3);
            table.SetWidthPercentage(new float[] { 170f, 170f, 170f }, PageSize.A4);
            table.AddCell(CreateHeaderCell("VAT", headerCell));
            table.AddCell(CreateHeaderCell("Netto", headerCell));
            table.AddCell(CreateHeaderCell("Brutto", headerCell));

            var res = results.Where(x => x.ForEvidence.Value);

            decimal brutto = res.Sum(x => x.ForEwidence).Value;
            decimal netto = res.Sum(x => x.ForEwidenceNetto).Value;
            decimal vat = res.Sum(x => x.ForEwidenceVat).Value;


            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", vat), headerCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", netto), headerCell));
            table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", brutto), headerCell));

            doc1.Add(table);

            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));
            doc1.Add(CreateParagraph(" ", fontText, Element.ALIGN_LEFT));


            var comp = results.Where(x => x.PaymentTypeId == 12 /*odszkodowanie */);

            if (comp.Count() > 0)
            {
                table = new PdfPTable(1);
                table.SetWidthPercentage(new float[] { 370f }, PageSize.A4);

                table.AddCell(CreateHeaderCell("Wpływy z tytułu odszkodowań", headerCell));

                decimal compensations = comp.Sum(x => x.ForEwidence).Value;

                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", compensations), headerCell));

                doc1.Add(table);
            }


            doc1.Close();

            return invoiceFile;
        }



    }
}
