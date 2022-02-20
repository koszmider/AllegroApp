using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LajtIt.Bll
{
    public class OfferHelper
    {
        Document doc1;


        public void SendFileToOutput(string fileName)
        {
            string contentType = contentType = "Application/pdf";

            HttpContext.Current.Response.ContentType = contentType;
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            HttpContext.Current.Response.WriteFile(fileName);
            HttpContext.Current.Response.End();
        }

        public string GetOfferVersion(int offerVersionId)
        {
            Dal.OfferHelper oh = new Dal.OfferHelper();
            Dal.OfferVersion ov = oh.GetOfferVersion(offerVersionId);

            doc1 = new Document(PageSize.A4);
            string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string pdfFile = String.Format(path, String.Format(@"Offer_{2:yyyyMMddHHmmss}_{0}_{1}.pdf", ov.OfferId, ov.OfferVersionId, DateTime.Now));
            PdfWriter writer = PdfWriter.GetInstance(doc1, new FileStream(pdfFile, FileMode.Create));

            doc1.Open();

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Image.GetInstance(HttpContext.Current.Server.MapPath(@"/Images/pdf_bgr.jpg")));


            jpg.ScalePercent(52);
            //If you want to choose image as background then,

            jpg.Alignment = iTextSharp.text.Image.UNDERLYING;

            doc1.Add(jpg);
            jpg.ScaleAbsolute(PageSize.A4.Width, PageSize.A4.Height);
            jpg.SetAbsolutePosition(0, 0);
            CreateOfferDocument(offerVersionId);

            doc1.Close();

            return pdfFile;
        }

        private void CreateOfferDocument(int offerVersionId)
        {
            Dal.OfferHelper oh = new Dal.OfferHelper();
            Dal.OfferVersion ov = oh.GetOfferVersion(offerVersionId);

            Dal.SettingsHelper sh = new Dal.SettingsHelper();




            doc1.Add(CreateParagraph(sh.GetSetting("OFFER_H").StringValue, Element.ALIGN_RIGHT));
            doc1.Add(CreateParagraph(String.Format("Data utworzenia: {0:yyyy/MM/dd}", DateTime.Now), Element.ALIGN_RIGHT));


            doc1.Add(CreateParagraph(String.Format("Oferta numer: 1908{0}", offerVersionId), Element.ALIGN_CENTER));



            string client = String.Format(@"Dane klienta:

{0}
tel: {1}
email: {2}",
                ov.Offer.ContactName, ov.Offer.Phone, ov.Offer.Email);


            doc1.Add(CreateParagraph(client, PdfPCell.ALIGN_LEFT));




            PdfPTable table1 = new PdfPTable(6);

            CreateInvoiceOrderTable(offerVersionId, table1);

            doc1.Add(table1);

            doc1.Add(CreateParagraph(sh.GetSetting("OFFER_I").StringValue, Element.ALIGN_LEFT));


        }
        private static void InittializeInvoiceTable(PdfPTable table)
        {

            table.SetWidthPercentage(new float[] { 5f, 50f, 5f, 10f, 15f, 15f }, PageSize.A4);
            table.WidthPercentage = 100;
            table.AddCell(CreateHeaderCell("Lp"));
            table.AddCell(CreateHeaderCell("Nazwa towaru"));
            table.AddCell(CreateHeaderCell("Ilość"));
            table.AddCell(CreateHeaderCell("Rabat"));
            table.AddCell(CreateHeaderCell("Cena brutto"));
            table.AddCell(CreateHeaderCell("Razem"));

            // table.SetWidths
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
        }
        private void CreateInvoiceOrderTable(int offerVersionId, PdfPTable table)
        {
            Dal.OfferHelper oh = new Dal.OfferHelper();


            List<Dal.OfferProductsView> products = oh.GetOfferProducts(offerVersionId);

            InittializeInvoiceTable(table);

            List<Dal.Helper.Amount> totals = new List<Dal.Helper.Amount>();

            int i = 1;
            decimal total = 0;
            decimal totalRebate = 0;

            #region loop
            foreach (Dal.OfferProductsView product in products)
            {

                decimal amount = product.OfferPrice.Value * product.Quantity * (1 - product.Rebate / 100);


                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, i.ToString()));
                table.AddCell(CreateRowImageCell(product));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0} ", product.Quantity.ToString())));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", product.Rebate)));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", product.OfferPrice)));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", amount)));
                total += amount;
                totalRebate += product.OfferPrice.Value * product.Quantity - amount;
                i++;
            }

            #endregion


            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", true));
            table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "Razem:", true));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", total), true));

            if (totalRebate > 0)
            {
                table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", true));
                table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "Razem przed rabatem:", true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", total + totalRebate), true));

                table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", true));
                table.AddCell(CreateFooterCell(Element.ALIGN_CENTER, "", true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "", true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "Łączny rabat:", true));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", (1 - (total / (total + totalRebate))) * 100), true));
            }
        }

        private static Paragraph CreateParagraph(string text, int aligment)
        {
            var f = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED, 10);
            Paragraph p = new Paragraph(text, f);
            p.Alignment = aligment;

            return p;
        }
        private static PdfPCell CreateRowCell(int aligment, string text)
        {
            var f = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED, 10);
            PdfPCell cell = new PdfPCell(new Phrase(text, f));
            cell.HorizontalAlignment = aligment;

            return cell;
        }
        private static PdfPCell CreateRowImageCell(Dal.OfferProductsView op)
        {
            var f = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED, 8);
            PdfPCell cell = new PdfPCell();
            cell.HorizontalAlignment = Element.ALIGN_CENTER;


            PdfPTable table = new PdfPTable(1);



            table.SetWidthPercentage(new float[] { 600f }, PageSize.A4);


            PdfPCell p = new PdfPCell(new Phrase(op.OfferName, f));
            p.BorderWidth = 0;
            p.HorizontalAlignment = Element.ALIGN_CENTER;
            p.PaddingBottom = 10;


            table.AddCell(p);
            string imageFile = String.Format(@"{0}\{1}",
               HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ProductCatalogImages"]), op.ImageFullName);

            if (File.Exists(imageFile))
            {
                Image jpg = Image.GetInstance(imageFile);
                //var x = jpg.XYRatio;
                // jpg.ScalePercent(8f);
                // jpg.ScaleAbsoluteWidth(40);


                int w = 290;
                float jpgRate = jpg.Width / jpg.Height;
                int newH = (int)(w * jpgRate);

                jpg.ScaleToFit(w, newH);

                PdfPCell i = new PdfPCell(jpg);
                i.BorderWidth = 0;
                i.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(i);
            }

            StringBuilder sb = new StringBuilder();

            if (!String.IsNullOrEmpty(op.Comment))
                sb.AppendLine(String.Format("Uwagi: {0}", op.Comment));
            if (op.Bulb != null)
                sb.AppendLine(String.Format("Żarówka w zestawie: {0}", op.Bulb));
            if (op.ShowSupplier)
                sb.AppendLine(String.Format("Producent: {0}", op.SupplierName));
            if (op.ShowCode)
                sb.AppendLine(String.Format("Kod produktu: {0}", op.Code));

            sb.AppendLine(String.Format("Id produktu: {0}", op.ProductCatalogId));

            PdfPCell m = new PdfPCell(new Phrase(sb.ToString(), f));
            m.BorderWidth = 0;
            m.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(m);

            //table.SpacingBefore = 20f;
            //table.SpacingAfter = 30f;

            cell.AddElement(table);


            return cell;
        }
        private static PdfPCell CreateFooterCell(int aligment, string text, bool noBorder)
        {
            Font footerCell = new Font();
            footerCell.SetStyle(Font.BOLD);
            PdfPCell cell = CreateRowCell(aligment, text);
            if (noBorder)
                cell.BorderWidth = 0;

            return cell;
        }
        private static PdfPCell CreateHeaderCell(string text)
        {
            var f = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED, 10);
            PdfPCell cell = new PdfPCell(new Phrase(text, f));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;

            return cell;
        }
    }
}
