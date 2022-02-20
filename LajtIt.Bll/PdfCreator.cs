using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace LajtIt.Bll
{
    public class PdfCreator
    { 
        public PdfCreator()
        { 
        }
        public struct SalesPlanResources1
        {
            public string timePara { get; set; }
            public string pagePara { get; set; }
            public string timePrintMask { get; set; }
            public string datePara { get; set; }

            public string datePrintMask { get; set; }
        }
        public struct DefineFont
        {
            public string fontFamily { get; set; }
            public int fontSize { get; set; }
            public bool isBold { get; set; }
            public bool isItalic { get; set; }
            public bool isUnderlined { get; set; }
            public BaseColor foreColor { get; set; }
        }

        public string CreateOffer(string path, int offId, int verId, bool showHeader, bool includeFooter)
        {
            string pdfFile = String.Format(path, String.Format(@"Offer_{2:yyyyMMddHHmmss}_{0}_{1}.pdf", offId, verId, DateTime.Now));

            Dal.OfferHelper oh = new Dal.OfferHelper();
            List<Dal.OfferProductsView> products = oh.GetOfferProducts(verId);

            bool isRebate = products.Sum(x => x.Rebate) > 0;

            PdfPTable table;
            if (isRebate)
            {
                table = new PdfPTable(6);
                table.SetWidthPercentage(new float[] { 5f, 60f, 5f, 10f, 10f, 10f }, PageSize.A4);
            }
            else
            {
                table = new PdfPTable(5);
                table.SetWidthPercentage(new float[] { 5f, 70f, 5f, 10f, 10f }, PageSize.A4);
            }
            table.WidthPercentage = 97;

            table.AddCell(CreateHeaderCell("Lp"));
            table.AddCell(CreateHeaderCell("Nazwa towaru"));
            table.AddCell(CreateHeaderCell("Ilość"));
            if (isRebate)
                table.AddCell(CreateHeaderCell("Rabat"));
            table.AddCell(CreateHeaderCell(@"Cena
brutto"));
            table.AddCell(CreateHeaderCell("Razem"));
            table.Rows[0].MaxHeights = 30;
            int i = 1;
            decimal total = 0;
            decimal totalRebate = 0;

            foreach (Dal.OfferProductsView product in products)
            {
                decimal amount = product.OfferPrice.Value * product.Quantity * (1 - product.Rebate / 100);

                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, i.ToString()));
                table.AddCell(CreateRowImageCell(product));
                table.AddCell(CreateRowCell(Element.ALIGN_CENTER, String.Format("{0} ", product.Quantity.ToString())));
                if (isRebate)
                    table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", product.Rebate)));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", product.OfferPrice)));
                table.AddCell(CreateRowCell(Element.ALIGN_RIGHT, String.Format("{0:C}", amount)));
                total += amount;
                totalRebate += product.OfferPrice.Value * product.Quantity - amount;
                i++;
            }

            if (isRebate)
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "Razem:", true, 5));
            else
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "Razem:", true, 4));
            table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", total), true));

            if (totalRebate > 0)
            {
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "Razem przed rabatem:", true, 5));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:C}", total + totalRebate), true));


                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, "Łączny rabat:", true, 5));
                table.AddCell(CreateFooterCell(Element.ALIGN_RIGHT, String.Format("{0:0.00} %", (1 - (total / (total + totalRebate))) * 100), true));
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("Data utworzenia: {0:yyyy/MM/dd}", DateTime.Now));
            sb.AppendLine(String.Format("Oferta numer: 1908{0}", verId));

            if (includeFooter)
            {
                Dal.SettingsHelper sh = new Dal.SettingsHelper();
                table.AddCell(CreateFooterCell(Element.ALIGN_LEFT, sh.GetSetting("OFFER_I").StringValue, true, 6));
            }
            CreatePdf(iTextSharp.text.PageSize.LEGAL, table, sb.ToString(), pdfFile, showHeader);

            return pdfFile;
        }

        public class HeaderFooter : PdfPageEventHelper
        {
            #region Startup_Stuff

            private string[] _headerLines;
            private string _footerLine;
            private string _headerLogo;
            private string _footerLogo;
            private string _headerText;

            private DefineFont _boldFont;
            private DefineFont _normalFont;

            private iTextSharp.text.Font fontTxtBold;
            private iTextSharp.text.Font fontTxtRegular;

            private int _fontPointSize = 0;

            private bool hasFooter = false;
            private bool hasHeader = false;
            private bool hasLogo = false;

            private int _headerWidth = 0;
            private int _headerHeight = 0;
            private int _imageHeaderHeight = 0;
            private int _imageFooterHeight = 0;

            private int _scale = 0;
            private int _footerHeight = 0;
            private int _footerWidth = 0;

            private int _leftMargin = 0;
            private int _rightMargin = 0;
            private int _topMargin = 0;
            private int _bottomMargin = 0;

            private PageNumbers NumberSettings;

            private DateTime runTime = DateTime.Now;

            public enum PageNumbers
            {
                None,
                HeaderPlacement,
                FooterPlacement
            }

            // This is the contentbyte object of the writer
            PdfContentByte cb;

            PdfTemplate headerTemplate;
            PdfTemplate footerTemplate;

            public string[] headerLines
            {
                get
                {
                    return _headerLines;
                }
                set
                {
                    _headerLines = value;
                    hasHeader = true;
                }
            }

            public string headerLogo
            {
                get
                {
                    return _headerLogo;
                }
                set
                {
                    _headerLogo = value;
                    hasLogo = true;
                    hasHeader = true;
                }
            }
            public string footerLogo
            {
                get
                {
                    return _footerLogo;
                }
                set
                {
                    _footerLogo = value;
                    hasLogo = true;
                    hasFooter = true;
                }
            }
            public string footerLine
            {
                get
                {
                    return _footerLine;
                }
                set
                {
                    _footerLine = value;
                    hasFooter = true;
                }
            }
            public string headerText
            {
                get
                {
                    return _headerText;
                }
                set
                {
                    _headerText = value;
                    hasFooter = true;
                }
            }

            public DefineFont boldFont
            {
                get
                {
                    return _boldFont;
                }
                set
                {
                    _boldFont = value;
                }
            }

            public DefineFont normalFont
            {
                get
                {
                    return _normalFont;
                }
                set
                {
                    _normalFont = value;
                }
            }

            public int scale
            {
                get
                {
                    return _scale;
                }
                set
                {
                    _scale = value;
                }
            }
            public int fontPointSize
            {
                get
                {
                    return _fontPointSize;
                }
                set
                {
                    _fontPointSize = value;
                }
            }

            public int leftMargin
            {
                get
                {
                    return _leftMargin;
                }
                set
                {
                    _leftMargin = value;
                }
            }

            public int rightMargin
            {
                get
                {
                    return _rightMargin;
                }
                set
                {
                    _rightMargin = value;
                }
            }

            public int topMargin
            {
                get
                {
                    return _topMargin;
                }
                set
                {
                    _topMargin = value;
                }
            }

            public int bottomMargin
            {
                get
                {
                    return _bottomMargin;
                }
                set
                {
                    _bottomMargin = value;
                }
            }

            public int headerheight
            {
                get
                {
                    return _headerHeight;
                }
            }

            public int footerHeight
            {
                get
                {
                    return _footerHeight;
                }
            }

            public PageNumbers PageNumberSettings
            {
                get
                {
                    return NumberSettings;
                }

                set
                {
                    NumberSettings = value;
                }
            }

            public SalesPlanResources1 SalesPlanResources { get
                {
                    return new SalesPlanResources1()
                    {
                        datePara = "{0}",
                        datePrintMask = "{yyyyMMdd HH:mm}",
                        pagePara = "{0}",
                        timePara = "{0}",
                        timePrintMask = "{yyyyMMdd HH:mm}"
                    };
                }  }

            #endregion

            #region Write_Headers_Footers

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                if (hasHeader)
                {
                    // left side is the string array passed in
                    // right side is a built in string array (0 = date, 1 = time, 2(optional) = page)
                    float[] widths = new float[2] { 70f, 30f };

                    PdfPTable hdrTable = new PdfPTable(2);
                    hdrTable.TotalWidth = document.PageSize.Width - (_leftMargin + _rightMargin);
                    //hdrTable.WidthPercentage = 90;
                    // hdrTable.SetWidths(widths);
                    hdrTable.SetWidthPercentage(widths, PageSize.A4);
                    //hdrTable.LockedWidth = true;


                    if (hasLogo)
                    {
                        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Image.GetInstance(_headerLogo));
                        jpg.ScalePercent(_scale); 
                        _imageHeaderHeight = (int)jpg.Height;
                        jpg.Alignment = iTextSharp.text.Image.LEFT_ALIGN;

                        PdfPCell leftCell = new PdfPCell();
                        leftCell.AddElement(jpg);
                        leftCell.Border = 0;
                        hdrTable.AddCell(leftCell);
                        PdfPCell rightCell = new PdfPCell(new Phrase(headerText));
                        rightCell.HorizontalAlignment = Element.ALIGN_RIGHT; 
                        rightCell.Border = 0;
                        hdrTable.AddCell(rightCell);
                    }
                    #region
                    //for (int hdrIdx = 0; hdrIdx < (_headerLines.Length < 2 ? 2 : _headerLines.Length); hdrIdx++)
                    //{
                    //    string leftLine = (hdrIdx < _headerLines.Length ? _headerLines[hdrIdx] : string.Empty);

                    //    Paragraph leftPara = new Paragraph(5, leftLine, (hdrIdx == 0 ? fontTxtBold : fontTxtRegular));

                    //    switch (hdrIdx)
                    //    {
                    //        case 0:
                    //            {
                    //                leftPara.Font.Size = _fontPointSize;

                    //                PdfPCell leftCell = new PdfPCell(leftPara);
                    //                leftCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                leftCell.Border = 0;

                    //                string rightLine = string.Format(SalesPlanResources.datePara, runTime.ToString(SalesPlanResources.datePrintMask));
                    //                Paragraph rightPara = new Paragraph(5, rightLine, (hdrIdx == 0 ? fontTxtBold : fontTxtRegular));
                    //                rightPara.Font.Size = _fontPointSize;

                    //                PdfPCell rightCell = new PdfPCell(rightPara);
                    //                rightCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                rightCell.Border = 0;

                    //                hdrTable.AddCell(leftCell);
                    //                hdrTable.AddCell(rightCell);

                    //                break;
                    //            }

                    //        case 1:
                    //            {
                    //                leftPara.Font.Size = _fontPointSize;

                    //                PdfPCell leftCell = new PdfPCell(leftPara);
                    //                leftCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                leftCell.Border = 0;

                    //                string rightLine = string.Format(SalesPlanResources.timePara, runTime.ToString(SalesPlanResources.timePrintMask));
                    //                Paragraph rightPara = new Paragraph(5, rightLine, (hdrIdx == 0 ? fontTxtBold : fontTxtRegular));
                    //                rightPara.Font.Size = _fontPointSize;

                    //                PdfPCell rightCell = new PdfPCell(rightPara);
                    //                rightCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                rightCell.Border = 0;

                    //                hdrTable.AddCell(leftCell);
                    //                hdrTable.AddCell(rightCell);

                    //                break;
                    //            }

                    //        case 2:
                    //            {
                    //                leftPara.Font.Size = _fontPointSize;

                    //                PdfPCell leftCell = new PdfPCell(leftPara);
                    //                leftCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                leftCell.Border = 0;

                    //                string rightLine;
                    //                if (NumberSettings == PageNumbers.HeaderPlacement)
                    //                {
                    //                    rightLine = string.Concat(SalesPlanResources.pagePara, writer.PageNumber.ToString());
                    //                }
                    //                else
                    //                {
                    //                    rightLine = string.Empty;
                    //                }
                    //                Paragraph rightPara = new Paragraph(5, rightLine, fontTxtRegular);
                    //                rightPara.Font.Size = _fontPointSize;

                    //                PdfPCell rightCell = new PdfPCell(rightPara);
                    //                rightCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                rightCell.Border = 0;

                    //                hdrTable.AddCell(leftCell);
                    //                hdrTable.AddCell(rightCell);

                    //                break;
                    //            }

                    //        default:
                    //            {
                    //                leftPara.Font.Size = _fontPointSize;

                    //                PdfPCell leftCell = new PdfPCell(leftPara);
                    //                leftCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                leftCell.Border = 0;
                    //                leftCell.Colspan = 2;

                    //                hdrTable.AddCell(leftCell);

                    //                break;
                    //            }
                    //    }
                    //}
                    #endregion
                    hdrTable.WriteSelectedRows(0, -1, _leftMargin, document.PageSize.Height - _topMargin, writer.DirectContent);

                    //Move the pointer and draw line to separate header section from rest of page
                    //cb.MoveTo(_leftMargin, document.Top + 10);
                    //cb.LineTo(document.PageSize.Width - _leftMargin, document.Top + 10);
                    //cb.Stroke();
                }

                if (hasFooter && _footerLogo!=null)
                {
                    //// footer line is the width of the page so it is centered horizontally 
                    PdfPTable ftrTable = new PdfPTable(1);
                    float[] widths = new float[1] { 100 };

                    ftrTable.TotalWidth = document.PageSize.Width - 10;
                    ftrTable.WidthPercentage = 95;
                    ftrTable.SetWidths(widths);

                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Image.GetInstance(_footerLogo));
                    jpg.ScalePercent(_scale);
                    _imageFooterHeight = (int)jpg.Height;
                    jpg.Alignment = iTextSharp.text.Image.LEFT_ALIGN;

                    PdfPCell leftCell = new PdfPCell();
                    //jpg.ScaleAbsolute(PageSize.A4.Width, PageSize.A4.Height);
                    //jpg.SetAbsolutePosition(0, 0);
                    leftCell.AddElement(jpg);
                    leftCell.Border = 0;
                    ftrTable.AddCell(leftCell);


                    //string OneLine;

                    //if (NumberSettings == PageNumbers.FooterPlacement)
                    //{
                    //    OneLine = string.Concat(_footerLine, writer.PageNumber.ToString());
                    //}
                    //else
                    //{
                    //    OneLine = _footerLine;
                    //}

                    //Paragraph onePara = new Paragraph(0, OneLine, fontTxtRegular);
                    //onePara.Font.Size = _fontPointSize;

                    //PdfPCell oneCell = new PdfPCell(onePara);
                    //oneCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //oneCell.Border = 0;
                    //ftrTable.AddCell(oneCell);

                    ftrTable.WriteSelectedRows(0, -1, _leftMargin, (_footerHeight), writer.DirectContent);

                    //Move the pointer and draw line to separate footer section from rest of page
                    //cb.MoveTo(_leftMargin, document.PageSize.GetBottom(_footerHeight + 2));
                    //cb.LineTo(document.PageSize.Width - _leftMargin, document.PageSize.GetBottom(_footerHeight + 2));
                    //cb.Stroke();
                }
            }

            #endregion


            #region Setup_Headers_Footers_Happens_here

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                // create the fonts that are to be used
                // first the hightlight or Bold font
                fontTxtBold = FontFactory.GetFont(_boldFont.fontFamily, _boldFont.fontSize, _boldFont.foreColor);
                if (_boldFont.isBold)
                {
                    fontTxtBold.SetStyle(Font.BOLD);
                }
                if (_boldFont.isItalic)
                {
                    fontTxtBold.SetStyle(Font.ITALIC);
                }
                if (_boldFont.isUnderlined)
                {
                    fontTxtBold.SetStyle(Font.UNDERLINE);
                }

                // next the normal font
                fontTxtRegular = FontFactory.GetFont(_normalFont.fontFamily, _normalFont.fontSize, _normalFont.foreColor);
                if (_normalFont.isBold)
                {
                    fontTxtRegular.SetStyle(Font.BOLD);
                }
                if (_normalFont.isItalic)
                {
                    fontTxtRegular.SetStyle(Font.ITALIC);
                }
                if (_normalFont.isUnderlined)
                {
                    fontTxtRegular.SetStyle(Font.UNDERLINE);
                }

                // now build the header and footer templates
                try
                {
                    float pageHeight = document.PageSize.Height;
                    float pageWidth = document.PageSize.Width;

                    _headerWidth = (int)pageWidth - ((int)_rightMargin + (int)_leftMargin);
                    _footerWidth = _headerWidth;

                    if (hasHeader)
                    {
                        // i basically dummy build the headers so i can trial fit them and see how much space they take.
                        float[] widths = new float[1] { 90f };

                        PdfPTable hdrTable = new PdfPTable(1);
                        hdrTable.TotalWidth = document.PageSize.Width - (_leftMargin + _rightMargin);
                        hdrTable.WidthPercentage = 95;
                        hdrTable.SetWidths(widths);
                        hdrTable.LockedWidth = true;

                        _headerHeight = 0;

                        if (hasLogo)
                        {
                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Image.GetInstance(_headerLogo));
                            jpg.ScalePercent(_scale);

                            _imageHeaderHeight = (int)jpg.Height;

                        }
                        for (int hdrIdx = 0; hdrIdx < (_headerLines.Length < 2 ? 2 : _headerLines.Length); hdrIdx++)
                        {
                            Paragraph hdrPara = new Paragraph(5, hdrIdx > _headerLines.Length - 1 ? string.Empty : _headerLines[hdrIdx], (hdrIdx > 0 ? fontTxtRegular : fontTxtBold));
                            PdfPCell hdrCell = new PdfPCell(hdrPara);
                            hdrCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            hdrCell.Border = 0;
                            hdrTable.AddCell(hdrCell);
                            _headerHeight = _headerHeight + (int)hdrTable.GetRowHeight(hdrIdx);
                        }

                        // iTextSharp underestimates the size of each line so fudge it a little 
                        // this gives me 3 extra lines to play with on the spacing
                        _headerHeight = _headerHeight + (_fontPointSize * 3)+ _imageHeaderHeight*_scale/100;

                    }

                    if (hasFooter)
                    {
                        if (hasLogo)
                        {
                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Image.GetInstance(_footerLogo));
                            jpg.ScalePercent(_scale);

                            _imageFooterHeight = (int)jpg.Height;

                        }
                        _footerHeight = (_fontPointSize * 2) + _imageFooterHeight * _scale / 100;
                    }

                    document.SetMargins(_leftMargin, _rightMargin, (_topMargin + _headerHeight), _footerHeight);

                    cb = writer.DirectContent;

                    if (hasHeader)
                    {
                        headerTemplate = cb.CreateTemplate(_headerWidth, _headerHeight);
                    }

                    if (hasFooter)
                    {
                        footerTemplate = cb.CreateTemplate(_footerWidth, _footerHeight);
                    }
                }
                catch (DocumentException de)
                {

                }
                catch (System.IO.IOException ioe)
                {

                }
            }

            #endregion

            #region Cleanup_Doc_Processing

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);

                if (hasHeader)
                {
                    headerTemplate.BeginText();
                    headerTemplate.SetTextMatrix(0, 0);

                    if (NumberSettings == PageNumbers.HeaderPlacement)
                    {
                    }

                    headerTemplate.EndText();
                }

                if (hasFooter)
                {
                    footerTemplate.BeginText();
                    footerTemplate.SetTextMatrix(0, 0);

                    if (NumberSettings == PageNumbers.FooterPlacement)
                    {
                    }

                    footerTemplate.EndText();
                }
            }

            #endregion

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
            var f = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED, 10);
            PdfPCell cell = new PdfPCell();
            cell.HorizontalAlignment = Element.ALIGN_CENTER;


            PdfPTable table = new PdfPTable(1);



            table.SetWidthPercentage(new float[] { 600f }, PageSize.A4);


            PdfPCell p = new PdfPCell(new Phrase(op.OfferName, f));
            p.BorderWidth = 0;
            p.HorizontalAlignment = Element.ALIGN_CENTER;
            p.PaddingBottom = 10;


            table.AddCell(p);
            if (op.ImageFullName != null)
            {
                string imageFile = String.Format(@"{0}\{1}",
                   HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ProductCatalogImages"]), op.ImageFullName.Replace(".", "_m."));

                if (File.Exists(imageFile))
                {
                    Image jpg = Image.GetInstance(imageFile);
                    //var x = jpg.XYRatio;
                    // jpg.ScalePercent(8f);
                    // jpg.ScaleAbsoluteWidth(40);


                    int w = 150;
                    float jpgRate = jpg.Width / jpg.Height;
                    int newH = (int)(w * jpgRate);

                    jpg.ScaleToFit(w, newH);

                    PdfPCell i = new PdfPCell(jpg);
                    i.BorderWidth = 0;
                    i.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(i);
                }
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

        private static PdfPCell CreateFooterCell(int aligment, string text, bool noBorder, int colSpan)
        {
            PdfPCell cell = CreateFooterCell(aligment, text, noBorder);
            if (colSpan > 1)
                cell.Colspan = colSpan;

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
        public static void CreatePdf(iTextSharp.text.Rectangle pageSize, IElement element, string headerText,  string pdfFile, bool showHeader)
        {
            Document pdfDoc = new Document(pageSize, 10, 10, 25, 25);
      

            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(pdfFile, FileMode.Create));
            HeaderFooter headers = new HeaderFooter();
            //headers.bottomMargin = 100;
            //headers.topMargin = 100;
            //headers.footerLine = "footerline";
            string[] parmLines = new string[] { };// { "headerlines", "Werwe", "WerwerW" };
            //headers.PageNumberSettings = HeaderFooter.PageNumbers.FooterPlacement;
            
            writer.PageEvent = headers;  
            DefineFont passFont = new DefineFont();

            // always set this to the largest font used
            headers.fontPointSize = 8;

            // set up the highlight or bold font
            passFont.fontFamily = "Helvetica";
            passFont.fontSize = 8;
            passFont.isBold = true;
            passFont.isItalic = false;
            passFont.isUnderlined = false;
            passFont.foreColor = BaseColor.BLACK;

            headers.boldFont = passFont;

            // now setup the normal text font
            passFont.fontSize = 7;
            passFont.isBold = false;

            headers.normalFont = passFont;

            headers.leftMargin = 10;
            headers.bottomMargin = 25;
            headers.rightMargin = 10;
            headers.topMargin = 25;

            headers.PageNumberSettings = HeaderFooter.PageNumbers.FooterPlacement;

            headers.headerText = headerText;
            headers.footerLine = "Strona";
            headers.headerLines = parmLines.ToArray();
            int headerHeight = 140;
            if (showHeader)
            {
                headers.headerLogo = HttpContext.Current.Server.MapPath(@"/Images/lajtit_header.png");
                headers.footerLogo = HttpContext.Current.Server.MapPath(@"/Images/lajtit_footer.png");
            }
            else
            {
                headers.headerLogo = HttpContext.Current.Server.MapPath(@"/Images/lajtit_header1.png");
                headers.footerLogo = HttpContext.Current.Server.MapPath(@"/Images/lajtit_footer1.png");

            }
            headers.scale = 30;
            pdfDoc.SetMargins(headers.leftMargin, headers.rightMargin, headers.topMargin + headerHeight, headers.bottomMargin + headers.footerHeight);
            pdfDoc.Open();

            // the new page is necessary due to a bug in in the current version of itextsharp
            pdfDoc.NewPage();
            //for (int i = 0; i < 100; i++)
            //    pdfDoc.Add(CreateParagraph(Guid.NewGuid().ToString(), Element.ALIGN_RIGHT));

            pdfDoc.Add(element);
            pdfDoc.Close();
        } 
    }
}

