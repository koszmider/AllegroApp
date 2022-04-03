using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
      public partial class Accounting
    {
        public static List<CostTypeFnResult> GetCostTypes(string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.CostTypeFn(userName).OrderBy(x => x.Name).ToList();
            }
        }

        public static List<Cost> GetCosts(int supplierOwnerId, string invoiceNumber)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Cost>(x => x.Company);
                ctx.LoadOptions = dlo;


                var q = ctx.Cost.Where(x => x.InvoiceNumber.Contains(invoiceNumber));

                //if(supplierOwnerId>0)
                //    q=q.Where(x=>x.Company)

                return q.ToList();
            }
        }

        public static List<InvoicesManagerView> GetInvoices(DateTime date, int companyId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.InvoicesManagerView
                    .Where(x=>x.InvoiceDate.Year== date.Year && x.InvoiceDate.Month==date.Month && x.CompanyId==companyId)
                    .OrderBy(x=>x.InvoiceDate)
                    .ToList();
            }
        }

        public static List<ShippingCompanyCost> GetShippingCompanyCosts(DateTime date, int shippingCompanyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShippingCompanyCost
                    .Where(x => x.ShippingCompanyId == shippingCompanyId && x.DeliveryDate.Value.Year == date.Year && x.DeliveryDate.Value.Month == date.Month)
                    .OrderBy(x => x.DeliveryDate)
                    .ThenBy(x => x.ParcelNumber)
                    .ToList();
            }
        }

        //public static List<InpostCost> GetInpostCosts(DateTime date)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.InpostCost
        //            .Where(x => x.DataNadania.Value.Year == date.Year && x.DataNadania.Value.Month == date.Month )
        //            .OrderBy(x => x.DataNadania)
        //            .ToList();
        //    }
        //}
        public static object GetBankAccountTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.BankAccountType.ToList();
            }
        }

        public static List<Dal.OrderPaymentAccountingType> GetAccoutingTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderPaymentAccountingType.ToList();
            }
        }

        public static List<OrderProductsSentView> GetOrderProductsSent(DateTime date)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrderProductsSentView
                    .Where(x => x.InsertDate.Value.Year == date.Year && x.InsertDate.Value.Month == date.Month)
                    .OrderBy(x => x.InsertDate)
                    .ToList();
            }
        }

        public static List<ProductCatalogDeliveryWarehouseViewWithPrice> GetProductCatalogDeliveryBlocked(DateTime date)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogDeliveryWarehouseViewWithPrice
                    .Where(x => x.InsertDate.Value.Year == date.Year && x.InsertDate.Value.Month == date.Month && x.QuantityBlocked > 0)
                    .OrderBy(x => x.InsertDate)
                    .ToList();
            }
        }

        public static List<ProductCatalogDeliveryWarehouseViewWithPrice> GetProductCatalogDelivery(DateTime date)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogDeliveryWarehouseViewWithPrice
                    .Where(x => x.InsertDate.Value.Year == date.Year && x.InsertDate.Value.Month == date.Month)
                    .OrderBy(x => x.InsertDate)
                    .ToList();
            }
        }

        public static List<ProductCatalogDeliveryInvoiceView> GetProductCatalogDeliveryInvoice(DateTime date)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogDeliveryInvoiceView
                    .Where(x => x.insertDate.Value.Year == date.Year && x.insertDate.Value.Month == date.Month && x.Quantity>0)
                    .OrderBy(x=>x.insertDate)
                    .ToList();
            }
        }

        public static List<ProductCatalogStockMonthEndResult> GetProductCatalogStockMonthEnd(DateTime date)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogStockMonthEnd(date).ToList();
            }
        }
        public static int SetCost(Cost cost)
        {
            try
            {
                if (String.IsNullOrEmpty(cost.InvoiceNumber))
                    cost.InvoiceNumber = null;

                using (LajtitDB ctx = new LajtitDB())
                {

                    if(cost.OrderId.HasValue)
                    {
                        if (ctx.Order.Where(x => x.OrderId == cost.OrderId).Count() != 1)
                            return -3;
                    }


                    Dal.CostDocumentType cdt = ctx.CostDocumentType
                        .Where(x => x.CostDocumentTypeId == cost.CostDocumentTypeId).FirstOrDefault();

                    if (cdt.AllowNegatives && cost.Amount >= 0 || !cdt.AllowNegatives && cost.Amount < 0)
                        return -2;

                    if (cost.CostId == 0)
                    {
                        if (cost.PaidDate.HasValue == false && cost.CompanyId > 0)
                        {
                            Company company = ctx.Company.Where(x => x.CompanyId == cost.CompanyId).FirstOrDefault();

                            if (company != null && company.PaymentDays.HasValue)
                            {
                                cost.PaidDate = cost.Date.AddDays(company.PaymentDays.Value);
                            }
                        }

                        ctx.Cost.InsertOnSubmit(cost);
                    }
                    else
                    {
                        Dal.Cost costToUpdate = ctx.Cost.Where(x => x.CostId == cost.CostId).FirstOrDefault();
                        costToUpdate.Amount = cost.Amount;
                        costToUpdate.Comment = cost.Comment;
                        costToUpdate.Date = cost.Date;
                        costToUpdate.VAT = cost.VAT;
                        costToUpdate.CostTypeId = cost.CostTypeId;
                        costToUpdate.InvoiceNumber = cost.InvoiceNumber;
                        costToUpdate.PaidDate = cost.PaidDate;
                        costToUpdate.CompanyId = cost.CompanyId;
                        costToUpdate.ToPay = cost.ToPay;
                        costToUpdate.CompanyOwnerId = cost.CompanyOwnerId;
                        costToUpdate.CostDocumentTypeId = cost.CostDocumentTypeId;
                        costToUpdate.CostRefId = cost.CostRefId;
                        costToUpdate.InvoiceCorrectionPaid = cost.InvoiceCorrectionPaid;
                        costToUpdate.OrderId = cost.OrderId;

                    }
                    ctx.SubmitChanges();
                }
                return cost.CostId;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627) // unique violation
                {
                    return -1;
                }
                return 0;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }



        public static int[] GetCostTypesForUser(string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] roleIds = ctx.SystemUserRole.Where(x => x.AdminUser.UserName == userName).Select(x => x.RoleId).ToArray();

                return ctx.CostTypeRole.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.CostTypeId).Distinct().ToArray();
            }
            }

        public static void SetShopPayments(int shopId, List<Dal.ShopPayment> payments)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach (Dal.ShopPayment payment in payments)
                {
                    if(ctx.ShopPayment.Where(x=>x.ShopId == shopId  
                    && x.PaymentNumber == payment.PaymentNumber).Count()==0)
                    {
                        ctx.ShopPayment.InsertOnSubmit(payment);
                    }

                }
                ctx.SubmitChanges();
            }
        }

        public static List<ShopPayment> GetShopPayments(int shopId, DateTime date, bool perShop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ShopPayment>(x => x.OrderPayment);
                dlo.LoadWith<ShopPayment>(x => x.ShopPaymentType);
                dlo.LoadWith<ShopPayment>(x => x.Shop);
                dlo.LoadWith<OrderPayment>(x => x.Order);
                dlo.LoadWith<OrderPayment>(x => x.OrderPaymentAccountingType);
                dlo.LoadWith<Order>(x => x.Invoice);
                ctx.LoadOptions = dlo;


                List<Dal.ShopPayment> payments;
                Dal.Shop shop = ctx.Shop.Where(x => x.ShopId == shopId).FirstOrDefault();

                if (shop.ShopTypeId == (int)Dal.Helper.ShopType.Allegro && !perShop)
                {
                    payments = ctx.ShopPayment
                    .Where(x => x.Shop.ShopTypeId == (int)Dal.Helper.ShopType.Allegro && x.PaymentDate.Year == date.Year && x.PaymentDate.Month == date.Month)
                    .OrderBy(x => x.ShopId).ThenBy(x => x.PaymentOperator).ThenBy(x => x.PaymentDate).ToList();
                }
                else
                {
                    payments = ctx.ShopPayment
                 .Where(x => x.ShopId == shopId && x.PaymentDate.Year == date.Year && x.PaymentDate.Month == date.Month)
                .OrderBy(x => x.PaymentOperator)
                .ThenBy(x => x.PaymentDate)
                .ThenByDescending(x => x.TotalAmount)
                 .ToList();

                }




                return payments;
            }
        }

        //public static void SetInpostCosts(List<InpostCost> costs)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {


        //        foreach (Dal.InpostCost cost in costs)
        //        {
        //            InpostCost c = ctx.InpostCost.Where(x => x.NumerPrzesylki == cost.NumerPrzesylki && x.Serwis == cost.Serwis).FirstOrDefault();

        //            if (c == null)
        //            {
        //                ctx.InpostCost.InsertOnSubmit(cost);
        //                ctx.SubmitChanges();
        //            }
        //        }
        //    }
        //}

        public static void SetOrderPaymentsAccounting(int[] orderPaymentIds, int accountingTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.OrderPayment> payments = ctx.OrderPayment.Where(x => orderPaymentIds.Contains(x.PaymentId)).ToList();

                foreach (Dal.OrderPayment p in payments)
                    p.AccountingTypeId = accountingTypeId;


                ctx.SubmitChanges();
            }
        }

        public static void SetBankAccountTypes(int[] ids, int bankAccountTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.BankAccount> payments = ctx.BankAccount.Where(x => ids.Contains(x.Id)).ToList();

                foreach (Dal.BankAccount p in payments)
                    p.BankAccountTypeId = bankAccountTypeId;


                ctx.SubmitChanges();
            }
        }

        public static void SetShippingCompanyCosts(List<ShippingCompanyCost> costs)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
               

                foreach (Dal.ShippingCompanyCost cost in costs)
                {
                    ShippingCompanyCost c = ctx.ShippingCompanyCost.Where(x => x.CostExternalId == cost.CostExternalId 
                    && x.InvoiceNumber == cost.InvoiceNumber).FirstOrDefault();

                    if(c==null)
                    {
                        ctx.ShippingCompanyCost.InsertOnSubmit(cost);
                        ctx.SubmitChanges();
                    }
                }
            }
        }

        public static void SetDpdPaymentToOrderPayment(DateTime dateTime)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                throw new NotImplementedException();

            //    List<Dal.DpdPayment> shopPayments = ctx.DpdPayment.Where(x =>
            //         x.PaymentDate.Year == dateTime.Year
            //        && x.PaymentDate.Month == dateTime.Month).ToList();
            //foreach (Dal.DpdPayment ba in shopPayments)
            //{
            //    List<OrderPayment> orderPayments = ctx.OrderPayment.Where(x =>
            //        x.OrderPaymentType.Name.Contains("ING")
            //        && x.Amount == ba.Amount
            //        && x.Order.ShipmentTrackingNumber == ba.TrackingNumber
            //        && x.InsertDate.Year == ba.PaymentDate.Year
            //        && x.InsertDate.Month == ba.PaymentDate.Month
            //        && x.InsertDate.Day == ba.PaymentDate.Day
            //        )
            //     .ToList();


            //    if (orderPayments.Count() == 1)
            //    {
            //        ba.OrderPaymentId = orderPayments[0].PaymentId;
            //        ctx.SubmitChanges();

            //    }
            //    }
            }
        }
        public static List<BankAccountMarketplaceResult> GetBankAccountMarketplace(int marketplaceId, DateTime date, int companyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.BankAccountMarketplace(companyId,date.Year, date.Month)
                    .Where(x=>x.OccuredAt.Year==date.Year && x.OccuredAt.Month == date.Month)
                    .OrderBy(x => x.UserName)
                    .ThenBy(x => x.PaymentOperator)
                    .ThenBy(x => x.OccuredAt)
                    .ToList();
            }
        }

        public static List<BankAccountView> GetBankAccount(DateTime date, int companyId, int accountId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.BankAccountView.Where(x => x.AccountId==accountId && x.CompanyId == companyId && x.PaymentDate.Year == date.Year && x.PaymentDate.Month == date.Month)
                    .OrderBy(x => x.PaymentDate)
                    .ToList();
            }
            }

        public static List<Cost> GetCostsByCompanyId(int companyId, string invoiceNumber)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions(); 
                dlo.LoadWith<Cost>(x => x.Company); 
                ctx.LoadOptions = dlo;
                return ctx.Cost.Where(x => x.CompanyId == companyId && x.CostDocumentTypeId == 1 && x.InvoiceNumber.Contains(invoiceNumber))
                    
                    .OrderByDescending(x => x.InsertDate)
                    .Take(100)
                    .ToList();
            }
            }

        public static Cost GetCost(int costId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Cost>(x => x.CostType);
                dlo.LoadWith<Cost>(x => x.Company);
                dlo.LoadWith<Cost>(x => x.Company1);
                 
                ctx.LoadOptions = dlo;
                return ctx.Cost.Where(x => x.CostId == costId).FirstOrDefault();
            }
        }

        public static List<Cost> GetCosts()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Cost>(x => x.CostType);
                dlo.LoadWith<Cost>(x => x.Company);
                dlo.LoadWith<Cost>(x => x.Company1);
                ctx.LoadOptions = dlo;
                return ctx.Cost.OrderByDescending(x => x.Date).ToList();
            }
        }
        public static List<Cost> GetCosts(int[] costsIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Cost>(x => x.Company);
                ctx.LoadOptions = dlo;

                return ctx.Cost.Where(x => costsIds.Contains(x.CostId)).ToList();
            }
        }
        public static void SetOrderPaymentsForBankAccount(int bankAccountId, int orderPaymentId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                BankAccount ba = ctx.BankAccount.Where(x => x.Id == bankAccountId).FirstOrDefault();

                ba.OrderPaymentId = orderPaymentId;
                ctx.SubmitChanges();

            }
        }

        public static void SetOrderPaymentsForShopPayment(int shopPaymentId, int orderPaymentId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ShopPayment ba = ctx.ShopPayment.Where(x => x.Id == shopPaymentId).FirstOrDefault();

                ba.OrderPaymentId = orderPaymentId;
                ctx.SubmitChanges();
            }
        }

        public static List<OrderPayment> GetOrderPaymentsForShopPayment(int shopPaymentId, int shopId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ShopPayment ba = ctx.ShopPayment.Where(x => x.Id == shopPaymentId).FirstOrDefault();



                List<OrderPayment> orderPayments = ctx.OrderPayment.Where(x => x.Order.ShopId == (shopId ==0 ? x.Order.ShopId: shopId)
               //&& x.PaymentTypeId == paymentTypeId// x.OrderPaymentType.Name.Contains("ING")
               && x.Amount > 0
               && x.InsertDate.Year == ba.PaymentDate.Year
               && x.InsertDate.Month == ba.PaymentDate.Month
               && x.InsertDate.Day == ba.PaymentDate.Day)
                    .ToList();

                int[] orderPaymentIds = orderPayments.Select(x => x.PaymentId).ToArray();
                int[] orderPaymentIdsAssigned = ctx.ShopPayment
                    .Where(x => x.OrderPaymentId.HasValue && orderPaymentIds.Contains(x.OrderPaymentId.Value))
                    .Select(x => x.OrderPaymentId.Value)
                    .ToArray();


                return orderPayments.Where(x => !orderPaymentIdsAssigned.Contains(x.PaymentId)).ToList();
            }
        }
        public static List<OrderPayment> GetOrderPaymentsForBankAccount(int bankAccountId, int accountId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                BankAccount ba = ctx.BankAccount.Where(x => x.Id == bankAccountId).FirstOrDefault();
                int paymentTypeId = 0;
                switch (accountId)
                {
                    case 1: paymentTypeId = 21; break;
                    case 2: paymentTypeId = 22; break;
                }


                List< OrderPayment> orderPayments = ctx.OrderPayment.Where(x => x.OrderPaymentType.CompanyId == ba.CompanyId 
                && x.PaymentTypeId== paymentTypeId// x.OrderPaymentType.Name.Contains("ING")
                && x.Amount > 0 
                && x.InsertDate.Year == ba.PaymentDate.Year
                && x.InsertDate.Month == ba.PaymentDate.Month
                && x.InsertDate.Day == ba.PaymentDate.Day)
                    .ToList();

                int[] orderPaymentIds = orderPayments.Select(x => x.PaymentId).ToArray();
                int[] orderPaymentIdsAssigned = ctx.BankAccount
                    .Where(x => x.OrderPaymentId.HasValue && orderPaymentIds.Contains(x.OrderPaymentId.Value))
                    .Select(x=>x.OrderPaymentId.Value)
                    .ToArray();


                return orderPayments.Where(x => !orderPaymentIdsAssigned.Contains(x.PaymentId)).ToList();
            }
        }

        public static ShopPayment GetShopPayment(int shopPaymentId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopPayment.Where(x => x.Id == shopPaymentId).FirstOrDefault();
            }
            }

        //public static List<OrderPayment> GetOrderPaymentsForDpd(int dpdPaymentId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        DpdPayment ba = ctx.DpdPayment.Where(x => x.Id == dpdPaymentId).FirstOrDefault();

        //        List<OrderPayment> orderPayments = ctx.OrderPayment.Where(x => //x.OrderPaymentType.CompanyId == ba.CompanyId
        //          x.OrderPaymentType.Name.Contains("ING")
        //       && x.Amount > 0
        //       && x.InsertDate.Year == ba.PaymentDate.Year
        //       && x.InsertDate.Month == ba.PaymentDate.Month
        //       && x.InsertDate.Day == ba.PaymentDate.Day)
        //            .ToList();

        //        int[] orderPaymentIds = orderPayments.Select(x => x.PaymentId).ToArray();
        //        int[] orderPaymentIdsAssigned = ctx.DpdPayment
        //            .Where(x => x.OrderPaymentId.HasValue && orderPaymentIds.Contains(x.OrderPaymentId.Value))
        //            .Select(x => x.OrderPaymentId.Value)
        //            .ToArray();


        //        return orderPayments.Where(x => !orderPaymentIdsAssigned.Contains(x.PaymentId)).ToList();
        //    }
        //}
        public static BankAccount GetBankAccount(int bankAccountId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                BankAccount ba = ctx.BankAccount.Where(x => x.Id == bankAccountId).FirstOrDefault();
                return ba; 
            }
        }


        public static List<CostsView> GetCostsStats()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.CostsView.ToList();
            }
        }

        public static void SetBankData(List<BankAccount> bank)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                foreach(BankAccount b in bank)
                {
                    Dal.BankAccount existing = ctx.BankAccount.Where(x => x.CompanyId == b.CompanyId && x.AccountNumber == b.AccountNumber && x.InstrId == b.InstrId).FirstOrDefault();

                    if (existing == null)
                        ctx.BankAccount.InsertOnSubmit(b);

                }
                ctx.SubmitChanges();
            }
        }

        public static void SetOrderInvoiceAccoutingType(int orderId, int? accountingTypeId)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Invoice invoice = ctx.Order.Where(x => x.OrderId == orderId).Select(x=>x.Invoice).FirstOrDefault();

                if (invoice != null)
                {
                    invoice.AccountingTypeId = accountingTypeId;

                    ctx.SubmitChanges();
                }
            }
        }

        public static void SetShopPaymentToOrderPayment(int shopId,  DateTime date)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.ShopPayment> shopPayments = ctx.ShopPayment.Where(x => x.ShopId == shopId
                 && x.PaymentDate.Year == date.Year
                 && x.PaymentDate.Month == date.Month
                 && x.PaymentTypeId == 1                 
                 && x.OrderPaymentId == null
                 )
                  .ToList();

                Dal.Shop shop = ctx.Shop.Where(x => x.ShopId == shopId).FirstOrDefault();

                if (shop.ShopTypeId == (int)Dal.Helper.ShopType.Allegro)
                {
                    AssignAllegroPayments(ctx, shopPayments);
                    return;

                }
                switch (shop.ShopId)
                {
                    case 0:
                        AssignDpd(ctx,  shopPayments); break;
                    case 1:
                        AssignPrzelewy24(ctx, shop.ShopId, shopPayments); break;
                    case 21:
                        AssignMorele(ctx, shop.ShopId, shopPayments); break;
                    case 6:
                        AssignPayUCeneo(ctx, shop.ShopId, shopPayments); break;
                    case 13:
                        AssignPolcard(ctx, shop.ShopId, shopPayments); break;
                }


                //switch(shopId)

                //int paymentTypeId = 0;
                //switch (accountId)
                //{
                //    case 1: paymentTypeId = 21; break;
                //    case 2: paymentTypeId = 22; break;
                //}

                //foreach (Dal.BankAccount ba in bankAccount)
                //{
                //    List<OrderPayment> orderPayments = ctx.OrderPayment.Where(x => x.OrderPaymentType.CompanyId == ba.CompanyId
                //        && x.PaymentTypeId == paymentTypeId// x.OrderPaymentType.Name.Contains("ING")
                //        && x.Amount > 0
                //        && x.InsertDate.Year == ba.PaymentDate.Year
                //        && x.InsertDate.Month == ba.PaymentDate.Month
                //        && x.InsertDate.Day == ba.PaymentDate.Day
                //        )
                //     .ToList();

                //    int[] orderPaymentIds = orderPayments.Select(x => x.PaymentId).ToArray();
                //    int[] orderPaymentIdsAssigned = ctx.BankAccount
                //        .Where(x => x.OrderPaymentId.HasValue && orderPaymentIds.Contains(x.OrderPaymentId.Value))
                //        .Select(x => x.OrderPaymentId.Value)
                //        .ToArray();

                //    orderPayments = orderPayments.Where(x => !orderPaymentIdsAssigned.Contains(x.PaymentId)).ToList();


                //    Dal.OrderPayment op = orderPayments.Where(x => x.Amount == ba.Amount).FirstOrDefault();

                //    if (op != null)
                //    {
                //        ba.OrderPaymentId = op.PaymentId;
                //        ctx.SubmitChanges();

                //    }
            }


            }

        public static void SetCourierCostsAssign()
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                ctx.CourierCostAssign();
            }
        }

        private static void AssignDpd(LajtitDB ctx,  List<ShopPayment> shopPayments)
        {
         
       

                foreach (Dal.ShopPayment ba in shopPayments)
                {
                    List<OrderPayment> orderPayments = ctx.OrderPayment.Where(x => 
                        x.OrderPaymentType.Name.Contains("ING")
                      //  && x.Amount == ba.Amount
                        && x.OrderId.ToString() == ba.Title.Trim()
                        && x.InsertDate.Year == ba.PaymentDate.Year
                        && x.InsertDate.Month == ba.PaymentDate.Month
                        && x.InsertDate.Day == ba.PaymentDate.Day
                        )
                     .ToList();


                    if (orderPayments.Count() == 1)
                    {
                        ba.OrderPaymentId = orderPayments[0].PaymentId;
                        ctx.SubmitChanges();

                    }
                }

 
        }

        private static void AssignMorele(LajtitDB ctx, int shopId, List<ShopPayment> shopPayments)
        {

            foreach (Dal.ShopPayment ba in shopPayments)
            {
                List<OrderPayment> orderPayments = ctx.OrderPayment.Where(x => x.Order.ShopId == shopId
                    && x.Order.Email == ba.ClientName
                    //&& x.OrderPaymentType.Name.Contains("Przelewy24")
                    && x.Amount > 0
                    && x.InsertDate.Year == ba.PaymentDate.Year
                    && x.InsertDate.Month == ba.PaymentDate.Month
                    && x.InsertDate.Day == ba.PaymentDate.Day
                    )
                 .ToList();

                int[] orderPaymentIds = orderPayments.Select(x => x.PaymentId).ToArray();
                int[] orderPaymentIdsAssigned = ctx.ShopPayment
                    .Where(x => x.OrderPaymentId.HasValue && orderPaymentIds.Contains(x.OrderPaymentId.Value))
                    .Select(x => x.OrderPaymentId.Value)
                    .ToArray();

                orderPayments = orderPayments.Where(x => !orderPaymentIdsAssigned.Contains(x.PaymentId)).ToList();


                Dal.OrderPayment op = orderPayments.Where(x => x.Amount == ba.Amount).FirstOrDefault();

                if (op != null)
                {
                    ba.OrderPaymentId = op.PaymentId;
                    ctx.SubmitChanges();

                }
            }
        } 
        private static void AssignPrzelewy24(LajtitDB ctx, int shopId, List<ShopPayment> shopPayments)
        {

            foreach (Dal.ShopPayment ba in shopPayments)
            {
                List<OrderPayment> orderPayments = ctx.OrderPayment.Where(x => x.Order.ShopId == shopId
                    && x.Order.ExternalOrderNumber == ba.Title
                    && x.OrderPaymentType.Name.Contains("Przelewy24")
                    && x.Amount > 0
                    && x.InsertDate.Year == ba.PaymentDate.Year
                    && x.InsertDate.Month == ba.PaymentDate.Month
                    && x.InsertDate.Day == ba.PaymentDate.Day
                    )
                 .ToList();

                int[] orderPaymentIds = orderPayments.Select(x => x.PaymentId).ToArray();
                int[] orderPaymentIdsAssigned = ctx.ShopPayment
                    .Where(x => x.OrderPaymentId.HasValue && orderPaymentIds.Contains(x.OrderPaymentId.Value))
                    .Select(x => x.OrderPaymentId.Value)
                    .ToArray();

                orderPayments = orderPayments.Where(x => !orderPaymentIdsAssigned.Contains(x.PaymentId)).ToList();


                Dal.OrderPayment op = orderPayments.Where(x => x.Amount == ba.Amount).FirstOrDefault();

                if (op != null)
                {
                    ba.OrderPaymentId = op.PaymentId;
                    ctx.SubmitChanges();

                }
            }
        } 
        private static void AssignPolcard(LajtitDB ctx, int shopId, List<ShopPayment> shopPayments)
        {

            foreach (Dal.ShopPayment ba in shopPayments)
            {
                List<OrderPayment> orderPayments = ctx.OrderPayment.Where(x => //x.Order.ShopId == shopId
                    x.OrderPaymentType .Name.Contains("Karta")
                    && x.Amount > 0
                    && x.InsertDate.Year == ba.PaymentDate.Year
                    && x.InsertDate.Month == ba.PaymentDate.Month
                    && x.InsertDate.Day == ba.PaymentDate.Day
                    )
                 .ToList();

                int[] orderPaymentIds = orderPayments.Select(x => x.PaymentId).ToArray();
                int[] orderPaymentIdsAssigned = ctx.ShopPayment
                    .Where(x => x.OrderPaymentId.HasValue && orderPaymentIds.Contains(x.OrderPaymentId.Value))
                    .Select(x => x.OrderPaymentId.Value)
                    .ToArray();

                orderPayments = orderPayments.Where(x => !orderPaymentIdsAssigned.Contains(x.PaymentId)).ToList();


                Dal.OrderPayment op = orderPayments.Where(x => x.Amount == ba.Amount).FirstOrDefault();

                if (op != null)
                {
                    ba.OrderPaymentId = op.PaymentId;
                    ctx.SubmitChanges();

                }
            }
        }
        private static void AssignPayUCeneo(LajtitDB ctx, int shopId, List<ShopPayment> shopPayments)
        {

            foreach (Dal.ShopPayment ba in shopPayments)
            {
                List<OrderPayment> orderPayments = ctx.OrderPayment.Where(x => x.Order.ShopId == shopId
                    && x.Order.ExternalOrderNumber==ba.Title
                    && x.Amount > 0
                    && x.InsertDate.Year == ba.PaymentDate.Year
                    && x.InsertDate.Month == ba.PaymentDate.Month
                    && x.InsertDate.Day == ba.PaymentDate.Day
                    )
                 .ToList();

                int[] orderPaymentIds = orderPayments.Select(x => x.PaymentId).ToArray();
                int[] orderPaymentIdsAssigned = ctx.ShopPayment
                    .Where(x => x.OrderPaymentId.HasValue && orderPaymentIds.Contains(x.OrderPaymentId.Value))
                    .Select(x => x.OrderPaymentId.Value)
                    .ToArray();

                orderPayments = orderPayments.Where(x => !orderPaymentIdsAssigned.Contains(x.PaymentId)).ToList();


                Dal.OrderPayment op = orderPayments.Where(x => x.Amount == ba.Amount).FirstOrDefault();

                if (op != null)
                {
                    ba.OrderPaymentId = op.PaymentId;
                    ctx.SubmitChanges();

                }
            }
        }
        private static void AssignAllegroPayments(LajtitDB ctx, List<ShopPayment> shopPayments)
        {
            
            foreach(Dal.ShopPayment sp in shopPayments)
            {
              
                    Dal.OrderPayment op = ctx.OrderPayment.Where(x => x.ExternalPaymentId == sp.PaymentNumber).FirstOrDefault();

                if(op!=null)
                {
                    if(ctx.ShopPayment.Where(x=>x.OrderPaymentId.Value == op.PaymentId).Count()==0)
                    {
                        sp.OrderPaymentId = op.PaymentId;
                        ctx.SubmitChanges();
                    }

                }
            }
        }

        public static void SetBankAccountToOrderPayment(DateTime date, int companyId, int accountId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.BankAccount> bankAccount = ctx.BankAccount.Where(x => x.CompanyId == companyId
                 && x.PaymentDate.Year == date.Year
                 && x.PaymentDate.Month == date.Month
                 && x.TransferType == "CRDT"
                 && x.BankAccountTypeId == 2
                 && x.OrderPaymentId == null
                 )
                  .ToList();

                int paymentTypeId = 0;
                switch(accountId)
                {
                    case 1: paymentTypeId = 21; break;
                    case 2: paymentTypeId = 22; break;
                }

                foreach (Dal.BankAccount ba in bankAccount)
                {
                    List<OrderPayment> orderPayments = ctx.OrderPayment.Where(x => x.OrderPaymentType.CompanyId == ba.CompanyId
                        && x.PaymentTypeId== paymentTypeId// x.OrderPaymentType.Name.Contains("ING")
                        && x.Amount > 0
                        && x.InsertDate.Year == ba.PaymentDate.Year
                        && x.InsertDate.Month == ba.PaymentDate.Month
                        && x.InsertDate.Day == ba.PaymentDate.Day
                        )
                     .ToList();

                    int[] orderPaymentIds = orderPayments.Select(x => x.PaymentId).ToArray();
                    int[] orderPaymentIdsAssigned = ctx.BankAccount
                        .Where(x => x.OrderPaymentId.HasValue && orderPaymentIds.Contains(x.OrderPaymentId.Value))
                        .Select(x => x.OrderPaymentId.Value)
                        .ToArray();

                    orderPayments = orderPayments.Where(x => !orderPaymentIdsAssigned.Contains(x.PaymentId)).ToList();


                    Dal.OrderPayment op = orderPayments.Where(x => x.Amount == ba.Amount).FirstOrDefault();

                    if (op != null)
                    { 
                        ba.OrderPaymentId = op.PaymentId;
                        ctx.SubmitChanges();

                    }
                }


            }
        }
    }
}
