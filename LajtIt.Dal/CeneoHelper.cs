using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class CeneoHelper
    {
        public void SetNewOrder(CeneoOrder co, Invoice invoice, List<OrderProduct> products,
            OrderStatusHistory orderStatusHistory)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                CeneoOrder sh = ctx.CeneoOrder.Where(x => x.CeneoOrderId == co.CeneoOrderId).FirstOrDefault();

                //  ctx.Order.InsertOnSubmit(order);
                ctx.OrderStatusHistory.InsertOnSubmit(orderStatusHistory);
                ctx.OrderProduct.InsertAllOnSubmit(products);
                if (invoice != null)
                {
                    if (invoice.InvoiceTypeId == 0)
                        invoice.InvoiceTypeId = 2;
                    ctx.Invoice.InsertOnSubmit(invoice);
                }
                ctx.SubmitChanges();

                sh.IsProcessed = true;
                sh.Order = orderStatusHistory.Order;
                ctx.SubmitChanges();
            }
        }

        public bool SetNewOrder(CeneoOrder co)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                CeneoOrder c = ctx.CeneoOrder.Where(x => x.CeneoOrderId == co.CeneoOrderId).FirstOrDefault();

                if (c == null)
                {
                    ctx.CeneoOrder.InsertOnSubmit(co);
                    ctx.SubmitChanges();

                    return false;
                }
                else
                {
                    if (ctx.CeneoOrder.Where(x => x.CeneoOrderId == co.CeneoOrderId && x.IsProcessed == false).FirstOrDefault() != null)
                    {
                        return false;
                    }
                    else
                        return true;
                }
            }
        }
    }
}
