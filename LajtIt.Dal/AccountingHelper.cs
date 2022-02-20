using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Reflection;

namespace LajtIt.Dal
{


    public class AccountingHelper
    {
        public Dal.AccoutingReportResult GetReport(DateTime firstDayOfMonth, int companyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.AccoutingReport(firstDayOfMonth, companyId).FirstOrDefault();

            }
        }

        public List<InvoicesView> GetInvoicesFromMonth(DateTime month, int companyId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.InvoicesView.Where(x => x.SellerCompanyId == companyId && x.InvoiceMonth == month.Month && x.InvoiceYear == month.Year && x.InvoiceFileName!=null)
                    .OrderBy(x=>x.InvoiceDate)
                .ToList();

            }
        }

        //public List<AccountingInvoiceMonthBeforeResult> GetInvoicesFromMonthBefore(DateTime month)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.AccountingInvoiceMonthBefore(month).OrderBy(x => x.InvoiceNumber).ThenBy(x => x.PaymentDate).ToList();

        //    }
        //}

        //public List<AccountingPaymentMonthBeforeThanInvoiceResult> GetInvoicesFromThisMonthWithPaymentFromPrevious(DateTime month)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.AccountingPaymentMonthBeforeThanInvoice(month).OrderBy(x => x.InvoiceNumber).ThenBy(x => x.PaymentDate).ToList();

        //    }
        //}

        public void SetPaymentsAccountingPeriod(DateTime month)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.AccountingPeriodSet(month);

            }
        }
 
    }
}
