using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace LajtIt.Dal
{
    public class AccountingHelper2
    {
        public List<Accounting2Result> GetReport(DateTime date, int companyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Accounting2(date, companyId).ToList();
            }
        }
        public List<Invoice> GetInvoicesCorrections(DateTime month, int companyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Invoice.Where(x => 
                    x.CompanyId == companyId 
                    && x.InvoiceDate.Month== month.Month 
                    && x.InvoiceDate.Year == month.Year 
                    && x.InvoiceSeqNo.HasValue
                    && x.InvoiceTypeId== 3
                )
                    .OrderBy(x => x.InvoiceDate)
                .ToList();

            }
        }

        public List<InvoicesView> GetInvoicesFromMonth(DateTime month, int companyId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.InvoicesView.Where(x => x.SellerCompanyId == companyId && x.InvoiceMonth == month.Month 
                && x.InvoiceYear == month.Year 
                && x.InvoiceSeqNo.HasValue
                //&& x.InvoiceFileName != null
                )
                    .OrderBy(x => x.InvoiceDate)
                .ToList();

            }
        }
        public List<InvoiceProduct> GetInvoiceProductsFromMonth(DateTime month, int companyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<InvoiceProduct>(x => x.Invoice);

                ctx.LoadOptions = dlo;
                int[] invoiceIds = GetInvoicesFromMonth(month, companyId).Select(x => x.InvoiceId).ToArray();
                return ctx.InvoiceProduct.Where(x => invoiceIds.Contains(x.InvoiceId)).ToList();

            }
        }
    }
}
