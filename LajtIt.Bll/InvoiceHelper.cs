using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace LajtIt.Bll
{
    public class InvoiceHelper
    {
        public string ImagesDirectory { get; set; }
        public string FilesDirectory { get; set; }
        public string InvoicesDirectory { get; set; }

        public void InsertUpdateInvoiceHeaders(int OrderId, Dal.Invoice invoice, string actingUser)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetInvoiceUpdate(OrderId, invoice, actingUser);
        }

        public void LockInvoiceCorrection(int OrderId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.Invoice invoice = CreateInvoiceCorrectionNumber(OrderId);


            string fileName =  GetPdfInvoiceCorrection(OrderId);



            string newFileName = String.Format("{0}\\Invoice-{1}_K_{2}.pdf", InvoicesDirectory, OrderId, invoice.InvoiceNumber.Replace("/", "-"));
            File.Copy(fileName, newFileName);

            FileInfo fi = new FileInfo(newFileName);
            fi.IsReadOnly = true;

            oh.SetInvoiceLock(invoice.InvoiceId, fi.Name);
        }
        public bool CanLockInvoice(int orderId, ref string msg)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

            Dal.Invoice invoice = order.Invoice;

            if (invoice == null)
                return false;



            msg = "";
            string vatRateIsOK = Bll.OrderHelper.ValidateVATRate(orderId, oh, order);

            if (vatRateIsOK != "")
            {
                msg += vatRateIsOK;
                return false;

            }

            //if (!order.AmountBalance.HasValue || (order.AmountBalance.Value < 0 && !order.ShippingType.PayOnDelivery))
            //{
            //    msg += String.Format("Zamówienie nie jest opłacone. Balans: {0:C}<br>", order.AmountBalance);
            //    return false;

            //}
            return true;
        }

        public void LockInvoice(int OrderId, bool checkPayments)
        {

            ImagesDirectory = System.Web.HttpContext.Current.Server.MapPath("/Images");
            FilesDirectory = System.Web.HttpContext.Current.Server.MapPath("/Files");
            InvoicesDirectory = System.Web.HttpContext.Current.Server.MapPath("/Files/Invoices");


            CreateInvoiceNumber(OrderId,checkPayments);


            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Invoice invoice = Dal.DbHelper.Orders.GetOrder(OrderId).Invoice;

            PDF pdf = new PDF(ImagesDirectory, FilesDirectory);
            string fileName = pdf.CreateInvoice(OrderId);
            string newFileName = String.Format("{0}\\Invoice-{1}_{2}.pdf", InvoicesDirectory, OrderId, invoice.InvoiceNumber.Replace("/", "-"));
            File.Copy(fileName, newFileName);

            FileInfo fi = new FileInfo(newFileName);
            fi.IsReadOnly = true;

            oh.SetInvoiceLock(invoice.InvoiceId, fi.Name);
        }



        public bool CreateInvoice(int OrderId, bool checkPayments)
        {

            return CreateInvoiceInternal(OrderId, checkPayments);
        }
        public Dal.Invoice CreateInvoiceCorrectionNumber(int orderId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Invoice i = Dal.DbHelper.Orders.GetOrder(orderId).Invoice;
            Dal.Invoice invoice = oh.GetInvoice(i.InvoiceCorrectionId.Value);

   
 

            if (invoice.InvoiceNumber != null)
            {
                invoice.InvoiceSeqNo = invoice.InvoiceSeqNo;
                invoice.InvoiceDate = invoice.InvoiceDate;
                invoice.InvoiceNumber = invoice.InvoiceNumber;
            }
            else
            {
                Dal.Invoice lastInvoice = oh.GetLastInvoice(invoice.CompanyId, 3);

                if (lastInvoice == null)
                {
                    invoice.InvoiceSeqNo = 1;
                }
                else
                {
                    invoice.InvoiceSeqNo = lastInvoice.InvoiceSeqNo.Value + 1;
                }
                invoice.InvoiceDate = DateTime.Now;
                invoice.InvoiceNumber = String.Format("{0}/K/{1:yyyy}", invoice.InvoiceSeqNo, DateTime.Now);// String.Format("{0}/{1}", invoice.InvoiceSeqNo, invoice.InvoiceDate.Year);

            } 
            oh.SetInvoiceNumber(invoice);
            invoice = oh.GetInvoice(i.InvoiceCorrectionId.Value);

            return invoice;
        }
        public bool CreateInvoiceNumber(int orderId,bool checkPayments)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);
            Dal.Invoice invoice = order.Invoice;
            
            if (invoice == null  )
                return false;


            //throw new NotImplementedException();
            if (checkPayments)
                if (((order.AmountPaid == 0 || order.AmountBalance < 0) /*&& order.ShippingType.PayOnDelivery == false*/))
                    return false;


            DateTime firstDateOfPayment = DateTime.Now;

         
            //Dal.OrderPayment orderPayment = Dal.DbHelper.Orders
            //    .GetOrderPayments(orderId)
            //    .Where(x => x.Amount > 0)
            //    .OrderByDescending(x => x.InsertDate)
            //    .Select(x => x)
            //    .FirstOrDefault();
         
            //if (orderPayment != null)
            //    firstDateOfPayment = orderPayment.InsertDate;

            if (invoice.InvoiceNumber != null)
            {
                invoice.InvoiceSeqNo = invoice.InvoiceSeqNo;
                invoice.InvoiceDate = invoice.InvoiceDate;
                invoice.InvoiceNumber = invoice.InvoiceNumber;
            }
            else
            {
                Dal.Invoice lastInvoice = oh.GetLastInvoice(invoice.CompanyId,2);

                if (lastInvoice == null)
                {
                    invoice.InvoiceSeqNo = 1;
                }
                else
                {
                    invoice.InvoiceSeqNo = lastInvoice.InvoiceSeqNo.Value + 1;
                }
                invoice.InvoiceDate = DateTime.Now;
                invoice.InvoiceNumber = String.Format("{0}/{1}", invoice.InvoiceSeqNo, invoice.InvoiceDate.Year);

            }
            //invoice.InvoiceAmount = order.AmountToPay;

            if (firstDateOfPayment > invoice.InvoiceDate)
                invoice.InvoiceSellDate = invoice.InvoiceDate;
            else
                invoice.InvoiceSellDate = firstDateOfPayment;
            oh.SetInvoiceNumber(invoice);
            return true;
        }

        private bool CreateInvoiceInternal(int orderId, bool checkPayments)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Invoice invoice = Dal.DbHelper.Orders.GetOrder(orderId) .Invoice;
            List<Dal.OrderProductsView> orderProducts = oh.GetOrderProducts(orderId);
            bool productsNotAssigned = orderProducts
                .Where(x => x.IsOrderProduct == 1 && x.Quantity > 0 && x.ProductCatalogId == null).Count() > 0;

            if (invoice == null || productsNotAssigned)
                return false;

            return true;// CreateInvoiceNumber(orderId, checkPayments); //koszmid
        }

        public bool InvoiceCorrectionLocked(int orderId, out string fileName)
        {
            fileName = "";
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Invoice invoice = Dal.DbHelper.Orders.GetOrder(orderId).Invoice;


            if (invoice == null || !invoice.InvoiceCorrectionId.HasValue)
                return false;

            Dal.Invoice invoiceCorrection = oh.GetInvoice(invoice.InvoiceCorrectionId.Value);
             

            if (invoiceCorrection.IsLocked.HasValue && invoiceCorrection.IsLocked.Value)
            {
                fileName = invoiceCorrection.InvoiceFileName;
                return true;

            }

            return false;
        }

        public bool CreateInvoiceCorrection(int orderId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Invoice invoice = Dal.DbHelper.Orders.GetOrder(orderId).Invoice;
        


            if (invoice == null)
                return false;

            int? invoiceCorrectionId = null;

            if (invoice.InvoiceCorrectionId.HasValue)
                invoiceCorrectionId = invoice.InvoiceCorrectionId.Value;
            else
                invoiceCorrectionId = oh.SetInvoiceCorrection(invoice);

            return true;

        }

        public string GetPdfInvoice(int OrderId)
        {
            PDF pdf = new PDF(ImagesDirectory, FilesDirectory); 
            return pdf.CreateInvoice(OrderId);
        }
        public string GetPdfInvoiceCorrection(int orderId)
        {
            PDF pdf = new PDF(ImagesDirectory, FilesDirectory);
            return pdf.CreateCorrectionInvoice(orderId);
        }
        public void SendFileToOutput(string fileName)
        {
            string contentType = contentType = "Application/pdf";

            HttpContext.Current. Response.ContentType = contentType;
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            HttpContext.Current.Response.WriteFile(fileName);
            HttpContext.Current.Response.End();
        }
        internal bool IsLocked(int orderId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);
            Dal.Invoice invoice = order.Invoice;
            bool result = true;

            if (invoice == null)
                return true;

            if (!invoice.IsLocked.HasValue || !invoice.IsLocked.Value)
            {
                result = false; 
            }


            return result;
        }

        //public string[] GetInvoiceFiles(int[] batchIds)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    int[] orderIds = oh.GetOrderIDsBasedOnBatchIDs(batchIds);
        //    return oh.GetInvoiceFiles(orderIds);
        //}

        //public void InsertUpdateInvoiceExcludeFromInvoiceReport(int orderId, bool exclude, string userName)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    oh.InsertUpdateInvoiceExcludeFromInvoiceReport(orderId, exclude, userName);
        //}

       
    }
}
