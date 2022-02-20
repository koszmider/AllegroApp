using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
    public partial class Orders
    {
        public static List<OrderReceiptProduct> GetReceiptProducts(int receiptId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderReceiptProduct>(x => x.OrderProduct);
                dlo.LoadWith<OrderProduct>(x => x.ProductCatalog);
                dlo.LoadWith<Dal.ProductCatalog>(x => x.Supplier); 

                ctx.LoadOptions = dlo;
                return ctx.OrderReceiptProduct.Where(x => x.ReceiptId == receiptId).ToList();
            }
        }

   

        public static List<OrdersWithoutReceipt> GetOrdersWithoutReceipt()
        {
            {
                using (LajtitViewsDB ctx = new LajtitViewsDB())
                {
                    return ctx.OrdersWithoutReceipt.ToList();
                }
            }
        }

        public static int SetReceipt(List<Dal.OrderReceiptProduct> receiptProducts)
        {
            {
                using (LajtitDB ctx = new LajtitDB())
                {
                    int orderId = receiptProducts[0].OrderReceipt.OrderId;

               

                    Dal.Invoice invoice = ctx.Order.Where(x => x.OrderId == orderId).Select(x=>x.Invoice).FirstOrDefault();

                    if (invoice!=null)
                    {
                        if(invoice.Nip=="")
                        {
                            invoice.AccountingTypeId =(int) Dal.Helper.OrderPaymentAccoutingType.CashRegister;
                        }
                    }


                    ctx.OrderReceiptProduct.InsertAllOnSubmit(receiptProducts);
                    ctx.SubmitChanges();
                    return receiptProducts[0].ReceiptId;
                }
            }
        }


        public static void SetReceiptCommandResult(int id, int statusId, string errorMsg)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.OrderReceiptCommand cmd = ctx.OrderReceiptCommand.Where(x => x.Id == id).FirstOrDefault();

                if (cmd != null)
                {
                    cmd.OrderReceiptStatusId = statusId;
                    cmd.ErrorMsg = errorMsg;
                    ctx.SubmitChanges();
                }
            }
        }

        public static OrdersView GetOrderView(int orderId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrdersView.Where(x => x.OrderId == orderId).FirstOrDefault();
            }
        }

        public static List<Country> GetCountries()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Country.OrderBy(x => x.CountryCode).OrderBy(x=>x.CountryCode).ToList();
            }
            }

        public static List<OrderReceiptCommand> GetReceiptCommands()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderReceiptCommand>(x => x.OrderReceipt);

                ctx.LoadOptions = dlo;
                return ctx.OrderReceiptCommand.Where(x => x.OrderReceiptStatusId == 1).ToList();
            }
        }



        public static List<OrderReceipt> GetReceipts(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderReceipt.Where(x => x.OrderId == orderId).ToList();
            }
        }

 
        public static List<OrderPayment> GetOrderPayments(int[] orderPaymentIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderPayment>(x => x.Order);

                ctx.LoadOptions = dlo;
                return ctx.OrderPayment.Where(x => orderPaymentIds.Contains(x.PaymentId)).ToList();
            }
        }

        public static void SetCashRegister(int id, string ip, bool isError, string msg)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.CashRegister cs = ctx.CashRegister.Where(x => x.Id == id).FirstOrDefault();

                if(cs!=null)
                {
                    cs.Ip = ip;
                    cs.LastUpdateDate = DateTime.Now;
                    cs.ErrorMsg = msg;
                    cs.IsError = isError;
                    ctx.SubmitChanges();

                }
            }
        }

      

        public static void SetReceiptCommand(int receiptId, string xml)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.OrderReceiptCommand cmd = new OrderReceiptCommand()
                {
                    OrderReceiptStatusId = 1,
                    ReceiptId = receiptId,
                    XmlCmd = xml
                };
                ctx.OrderReceiptCommand.InsertOnSubmit(cmd);
                ctx.SubmitChanges();
            }
        }

    }
}