using iTextSharp.text;
using iTextSharp.text.pdf;
using LajtIt.Dal;
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
    public partial class OrderHelper
    {
        //public static void SetShippingNumbers()
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    List<Order> orders = oh.GetOrdersWithoutTrackingNumbers().ToList(); 


        //    foreach (Dal.Order order in orders)
        //    {
        //        try
        //        {
        //            switch (order.ShippingType.ShippingCompanyId)
        //            {
        //                case 1:
        //                    if (order.ShipmentCountryCode == "PL" || (order.ShippingData!=null 
        //                        && order.ShippingData.Split(new char[] {'|'}).Length==2 
        //                        &&  order.ShippingData.Split(new char[] { '|' })[1]!=""))
        //                    {
        //                        DpdHelper dpd = new DpdHelper();
        //                        dpd.ExportToDpd(order, "System", DpdWCF.outputDocPageFormatDSPEnumV1.LBL_PRINTER);
        //                    }
        //                    break;
        //                case 4:
        //                case 20:

        //                    Bll.InpostHelper ih = new InpostHelper();

        //                    ih.ExportInpostBatch(null, order, "System");
        //                    break;

        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Bll.ErrorHandler.SendEmail(String.Format("SetShippingNumbers  ShippingCompany: {0}, OrderId: {1}, <bR><br>{2}", order.ShippingType.ShippingCompanyId,order.OrderId, ex.Message));
        //        }
        //    }
        //}
        public static void SetShippingNumbers()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<OrderShipping> orders = oh.GetOrdersWithoutTrackingNumbers2()
            
                .ToList();


            foreach (Dal.OrderShipping order in orders)
            {
                BindTracking(order);
            }
        }

        public static void BindTracking(OrderShipping order)
        {
            try
            {
                switch (order.ShippingCompanyId)
                {
                    case 1:

                        DpdHelper dpd = new DpdHelper();
                        dpd.ExportToDpd2(order, "System", DpdWCF.outputDocPageFormatDSPEnumV1.LBL_PRINTER);

                        break;
                    case 4:
                    case 20:

                        Bll.InpostHelper ih = new InpostHelper();

                        ih.ExportInpostBatch(null, order, "System");
                        break;

                }

            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendEmail(String.Format("SetShippingNumbers  ShippingCompany: {0}, OrderId: {1}, <bR><br>{2}{3}", order.ShippingCompanyId, order.OrderId, ex.Message,ex.StackTrace));
            }
        }

        public static bool ExportFile(string[] ff)
        {
            if (ff.Length == 0)
                return false;


            string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            List<string> files = new List<string>();

            foreach(string fileToCheck in ff)
            {
                if (File.Exists(String.Format("{0}\\{1}", String.Format(path, ""), fileToCheck)))
                    files.Add(fileToCheck);
            }

            if (files.Count() == 0)
                return false;

            Dal.OrderHelper oh = new Dal.OrderHelper();

            //Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            //if (String.IsNullOrEmpty(order.ShipmentTrackingNumber))
            //{

            //    return false;
            //}


            string file = String.Format(path, String.Format(@"Shipping\{0}.pdf", Guid.NewGuid()));
            //file = String.Format(file, order.ShippingType.ShippingCompany.Name, order.ShipmentTrackingNumber);

            //if (!System.IO.File.Exists(file))
            //  return false;

            string f = Bll.PDF.MergeFiles(files.ToArray(), String.Format(path,""), file, iTextSharp.text.PageSize.A4);

            string newFile = MakeA4PdfFile(f);

            string contentType = "Application/pdf";

            contentType = "Application/pdf";

            HttpContext.Current.Response.ContentType = contentType;
            HttpContext.Current.Response.AppendHeader("content-disposition", String.Format("attachment; filename={0}.pdf", "shipping"));

            HttpContext.Current.Response.WriteFile(newFile);
            HttpContext.Current.Response.End();
            return true;

        }

        private static string MakeA4PdfFile(string file)
        {
            string dest = file.Replace(".pdf", "_A4.pdf");

            PdfReader reader = new PdfReader(file);
            // step 1
            Rectangle pagesize = new Rectangle(
                PageSize.A4.Width,
                PageSize.A4.Height);
            Document document = new Document(pagesize);
            // step 2
            PdfWriter writer
                = PdfWriter.GetInstance(document, new FileStream(dest, FileMode.Create));
            // step 3
            document.Open();
            // step 4
            PdfContentByte canvas = writer.DirectContent;
            float a4_width = PageSize.A4.Width;
            int n = reader.NumberOfPages;
            int p = 1; 
            while ((p - 1) / 4 <= n / 4)
            {
                addPage(canvas, reader, p, 10, -10);
                addPage(canvas, reader, p + 1, a4_width/2, -10);
                //document.NewPage();
                addPage(canvas, reader, p + 2, 10, PageSize.A4.Height / 2 -20);
                addPage(canvas, reader, p + 3, a4_width/2, PageSize.A4.Height / 2 - 20);
                document.NewPage();
                p += 4;
            }
            // step 5
            document.Close();
            reader.Close();
            return dest;
        }

        public static void addPage(PdfContentByte canvas,
         PdfReader reader, int p, float x, float y)
        {
            if (p > reader.NumberOfPages) return;
            PdfImportedPage page = canvas.PdfWriter.GetImportedPage(reader, p);
            canvas.AddTemplate(page, x , y);
        }
    }
}
