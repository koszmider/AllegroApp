using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace LajtIt.Dal
{
    public class AllegroItemUpdater
    { 
       
        public bool UpdateAll { get; set; }
        public bool UpdateDelivery { get; set; }
        public bool UpdateQuantity { get; set; }
        public bool UpdatePrice { get; set; }
        public bool UpdateName { get; set; }
        public bool UpdateDescription { get; set; }
        public bool UpdatePictures { get; set; }
        public bool UpdateParameters { get; set; }
        public bool Status { get; set; }
        public bool Category { get; set; }
        public bool Ean { get; set; }

         


        public AllegroItemUpdater(string updateCommand)
        {
            UpdateAll = GetField(updateCommand, 0);
            UpdateDelivery = GetField(updateCommand, 1);
            UpdateQuantity = GetField(updateCommand, 2);
            UpdatePrice = GetField(updateCommand, 3);
            UpdateName = GetField(updateCommand, 4);
            UpdateDescription = GetField(updateCommand, 5);
            UpdatePictures = GetField(updateCommand, 6);
            UpdateParameters = GetField(updateCommand, 7);
            Status = GetField(updateCommand, 8);
            Category = GetField(updateCommand, 9);
            Ean = GetField(updateCommand, 10);
        }

        private bool GetField(string updateCommand, int idx)
        {
            return updateCommand[idx] == '1' ? true : false;
        }
    }
    public class OrderHelper
    {

        public int InsertUpdateOrder(Order order, string comment, Dal.AllegroItemOrder allegroOrder,
            List<Dal.AllegroItemTransactionItem> forms)
        {
            using (LajtitAllegroDB ctxa = new LajtitAllegroDB())
            {
                using (LajtitDB ctx = new LajtitDB())
                {
                    int orderId = 0;
                    try
                    {
                        int[] orderStatusIds = new int[] { 1, /* 4,*/ 9, 10, 11 };
                        Dal.Order exitingOrder = ctx.Order
                            .Where(x => orderStatusIds.Contains(x.OrderStatusId)
                                && x.Email.ToLower().Trim() == order.Email.ToLower().Trim())
                            .FirstOrDefault();

                        if (exitingOrder != null)
                        {
                            orderId = exitingOrder.OrderId;

                            //OrderStatusHistory osh = new OrderStatusHistory();
                            //osh.InsertUser = "Auto";
                            //osh.InsertDate = order.InsertDate;

                            //if (exitingOrder.OrderStatusId == (int)Dal.Helper.OrderStatus.ReadyToSend)
                            //{
                            //    exitingOrder.OrderStatusId = (int)Dal.Helper.OrderStatus.New;
                            //    exitingOrder.ShippingData = null;

                            //    osh.Order = exitingOrder;
                            //    osh.OrderStatusId = exitingOrder.OrderStatusId;
                            //    osh.Comment = String.Format("Wycofanie zamówienia do statusu Nowy gdyż dodano nowy produkt. Komentarz użytkownika: {0}",
                            //        comment) ;
                            //}
                            //else
                            //{
                            //    osh.Order = exitingOrder;
                            //    osh.OrderStatusId = (int)Dal.Helper.OrderStatus.Comment;
                            //    osh.Comment = String.Format("Dodano nowy produkt comment;
                            //}
                            //ctx.OrderStatusHistory.InsertOnSubmit(osh);
                            UpdateOrder(order, ctx, exitingOrder);
                        }
                        else
                        {
                            OrderStatusHistory osh = new OrderStatusHistory();
                            osh.InsertUser = "Auto";
                            osh.InsertDate = order.InsertDate;
                            osh.Order = order;
                            osh.OrderStatusId = order.OrderStatusId;
                            osh.Comment = comment;
                            ctx.OrderStatusHistory.InsertOnSubmit(osh);
                            ctx.SubmitChanges();
                            orderId = order.OrderId;
                        }
                        Dal.AllegroItemOrder existingAllegroItemOrder =
                                ctxa.AllegroItemOrder.Where(x => x.Id == allegroOrder.Id).FirstOrDefault();
                        existingAllegroItemOrder.OrderCreated = true;

                        List<Dal.AllegroItemTransaction> existingTransactions =
                            ctxa.AllegroItemTransaction
                            .Where(x => forms.Select(y => y.TransactionId).ToArray().Contains(x.TransactionId))
                            .ToList();
                        foreach (Dal.AllegroItemTransaction transaction in existingTransactions)
                        {
                            transaction.OrderCreated = true;
                            transaction.OrderId = orderId;
                        }

                        ctx.SubmitChanges();
                        ctxa.SubmitChanges();

                        return orderId;
                    }
                    catch (Exception ex)
                    {
                        throw ex; 
                    }
                }
            }

        }


        public void SetCostToPay(int[] costIds, bool? toPay)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Guid batchId = Guid.NewGuid();
                DateTime batchDate = DateTime.Now;
                var costs= ctx.Cost.Where(x => costIds.Contains(x.CostId)).ToList();

                foreach (Cost c in costs)
                {
                    c.ToPay = toPay;
                    c.BatchDate = batchDate;
                    c.BatchId = batchId;
                }


                ctx.SubmitChanges();
            }
        }

        public List<OrderPayment> GetOrderPaymentsCash(DateTime date)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderPayment.Where(x => x.InsertDate.Year == date.Year
                && x.InsertDate.Month == date.Month
                && x.PaymentTypeId == 25)
                    .ToList();
            }
        }

        public List<ProductCatalogDelivery> GetProductCatalogDeliveryByOrderId(int orderId, int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogDelivery
                    .Where(x => x.ProductCatalogId == productCatalogId && x.OrderId == orderId)
                    .ToList();
            }
        }

        public bool GetCompanyReadyForPayment(int companyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Company company = ctx.Company.Where(x => x.CompanyId == companyId
                && x.IsReadyForPayment==true

                    ).FirstOrDefault();


                return company != null;
            }
        }

        public void SetProductOrderBatch(int[] orderProductIds, int[] productOrderIds, int supplierId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductOrderBatch batch = new ProductOrderBatch()
                {
                    InsertDate = DateTime.Now,
                    InsertUser = userName,
                    SupplierOwnerId = supplierId
                };
                List<Dal.ProductOrderBatchProduct> batchProducts = new List<ProductOrderBatchProduct>();
                List<Dal.OrderProduct> orderProducts = ctx.OrderProduct.Where(x => orderProductIds.Contains(x.OrderProductId)).ToList();
                List<Dal.ProductOrder> productOrders = ctx.ProductOrder.Where(x => productOrderIds.Contains(x.Id)).ToList();

                foreach (Dal.OrderProduct orderProduct in orderProducts)
                {
                    Dal.ProductOrderBatchProduct bp = new ProductOrderBatchProduct()
                    {
                        ProductOrderBatch = batch,
                        ProductCatalogId = orderProduct.ProductCatalogId.Value,
                        Quantity = orderProduct.Quantity
                    };
                    orderProduct.ProductOrderBatchProduct = bp;
                    batchProducts.Add(bp);

                }
                foreach (Dal.ProductOrder productOrder in productOrders)
                {
                    Dal.ProductOrderBatchProduct bp = new ProductOrderBatchProduct()
                    {
                        ProductOrderBatch = batch,
                        ProductCatalogId = productOrder.ProductCatalogId,
                        Quantity = productOrder.Quantity
                    };
                    productOrder.ProductOrderBatchProduct = bp;
                    batchProducts.Add(bp);

                } 

                ctx.ProductOrderBatchProduct.InsertAllOnSubmit(batchProducts);
                ctx.SubmitChanges();
            }
        }

        public int SetCompany(Company company, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.Company.InsertOnSubmit(company);
                ctx.SubmitChanges();

                return company.CompanyId;
            }
        }

        public void SetCompany(int companyId, Company company, string actingUser)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Company companyToUpdate = ctx.Company.Where(x => x.CompanyId == companyId).FirstOrDefault();

                if (company.BankAccountNumber != companyToUpdate.BankAccountNumber)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana numeru konta z {0} na {1}", companyToUpdate.BankAccountNumber, company.BankAccountNumber)
                        , actingUser
                        , companyId
                        , "Company"));
                }
                if (company.BankName != companyToUpdate.BankName)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana nazwy właściciela rachunku z {0} na {1}", companyToUpdate.BankName, company.BankName)
                        , actingUser
                        , companyId
                        , "Company"));
                }
                if (company.BankName != companyToUpdate.BankName)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana nazwy właściciela rachunku z {0} na {1}", companyToUpdate.BankName, company.BankName)
                        , actingUser
                        , companyId
                        , "Company"));
                }
                if (company.BankAddress != companyToUpdate.BankAddress)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana adresu właściciela rachunku z {0} na {1}", companyToUpdate.BankAddress, company.BankAddress)
                        , actingUser
                        , companyId
                        , "Company"));
                }
                if (company.BankNumber != companyToUpdate.BankNumber)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana numeru rozliczeniowego rachunku z {0} na {1}", companyToUpdate.BankNumber, company.BankNumber)
                        , actingUser
                        , companyId
                        , "Company"));
                }
                if (company.KRS != companyToUpdate.KRS)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana numeru KRS z {0} na {1}", companyToUpdate.KRS, company.KRS)
                        , actingUser
                        , companyId
                        , "Company"));
                }
                if (company.BDO != companyToUpdate.BDO)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana numeru BDO z {0} na {1}", companyToUpdate.BDO, company.BDO)
                        , actingUser
                        , companyId
                        , "Company"));
                }
                if (company.CanSendToBank != companyToUpdate.CanSendToBank)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana CanSendToBank z {0} na {1}", companyToUpdate.CanSendToBank, company.CanSendToBank)
                        , actingUser
                        , companyId
                        , "Company"));
                }









                companyToUpdate.Name                        =company.Name              ;
                companyToUpdate.Address                     =company.Address           ;
                companyToUpdate.PostalCode                  =company.PostalCode        ;
                companyToUpdate.City                        =company.City              ;
                companyToUpdate.TaxId                       =company.TaxId             ;
                companyToUpdate.BankAccountNumber           =company.BankAccountNumber ;
                companyToUpdate.CompanyOwner                =company.CompanyOwner      ;
                companyToUpdate.DpdNumcat                   =company.DpdNumcat         ;
                companyToUpdate.Regon                       =company.Regon             ;
                companyToUpdate.AddressNo                   =company.AddressNo         ;
                companyToUpdate.IsActive                    =company.IsActive          ;
                companyToUpdate.IsMyCompany                 =company.IsMyCompany       ;
                companyToUpdate.BankName                    =company.BankName          ;
                companyToUpdate.BankAddress                 =company.BankAddress;
                companyToUpdate.BankNumber = company.BankNumber;
                companyToUpdate.PaymentDays = company.PaymentDays;
                companyToUpdate.KRS = company.KRS;
                companyToUpdate.BDO = company.BDO;
                companyToUpdate.CanSendToBank = company.CanSendToBank;



                ctx.SubmitChanges();
            }
        }

        public List<Order> GetOrdersDeletedForEmpik()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopOrder
                    .Where(x => x.Order.OrderStatusId == (int)Dal.Helper.OrderStatus.Deleted && x.ShopOrderStatus == "WAITING_ACCEPTANCE")
                    .Select(x=>x.Order)
                    .ToList();
            }
        }

        public void SetOrderUpdateFromEmpik(Order order)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Order orderToUpdate = ctx.Order.Where(x => x.OrderId == order.OrderId).FirstOrDefault();

                orderToUpdate.ShipmentAddress = order.ShipmentAddress;
                orderToUpdate.ShipmentCity = order.ShipmentCity;
                orderToUpdate.ShipmentCompanyName = order.ShipmentCompanyName;
                orderToUpdate.ShipmentFirstName = order.ShipmentFirstName;
                orderToUpdate.ShipmentLastName = order.ShipmentLastName;
                orderToUpdate.ShipmentPostcode = order.ShipmentPostcode;
                orderToUpdate.DeliveryDate = order.DeliveryDate;
                orderToUpdate.Phone = order.Phone;

                ShopOrder so = ctx.ShopOrder.Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                so.ShopOrderStatus = "SHIPPING";




                ctx.SubmitChanges();
            }
        }

        public Order GetOrderByShopAndExternalOrderNumber(int shopId, string externalOrderNumber)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Order.Where(x => x.ShopId == shopId && x.ExternalOrderNumber.Equals(externalOrderNumber)).FirstOrDefault();
            }
        }

        public List<Order> GetOrdersByStatus(Helper.OrderStatus orderStatus)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Order.Where(x => x.OrderStatusId == (int)orderStatus).ToList();
            }
        }
        public void SetOrderComplaint(ComplaintStatusHistory csh, Dal.OrderComplaint complaint)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ComplaintStatus cs = ctx.ComplaintStatus.Where(x => x.ComplaintStatusId == csh.ComplaintStatusId).FirstOrDefault();
                Dal.OrderComplaint oc = ctx.OrderComplaint.Where(x => x.Id == csh.OrderComplaintId).FirstOrDefault();

                if (cs.ChangesStatus)
                {
                   oc.ComplaintStatusId = csh.ComplaintStatusId;
                }

                oc.InvoiceCorrectionExpected = complaint.InvoiceCorrectionExpected;
                oc.LastUpdateDate = DateTime.Now;
                oc.OrderComplaintTypeId = complaint.OrderComplaintTypeId;

                ctx.ComplaintStatusHistory.InsertOnSubmit(csh);
                ctx.SubmitChanges();
            }
        }

        public void SetOrderPaymentReceipt(int orderPaymentId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                OrderPayment op = ctx.OrderPayment.Where(x => x.PaymentId == orderPaymentId).FirstOrDefault();

                if (op.ReceiptCreated == null)
                    op.ReceiptCreated = true;
                else
                    op.ReceiptCreated = !op.ReceiptCreated.Value;

                ctx.SubmitChanges();
            }
        }
    

        public List<ShopPaymentTracker> GetShopPayments(DateTime date)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopPaymentTracker.Where(x => x.InsertDate.Year == date.Year && x.InsertDate.Month == date.Month && x.InsertDate.Day == date.Day).ToList();
            }
            }

        public int SetSupplier(Supplier supplier)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int ids = ctx.Supplier.Max(x => x.SupplierId);

                ids++;

                supplier.SupplierId = ids;

                ctx.Supplier.InsertOnSubmit(supplier);
                ctx.SubmitChanges();



                int id = ctx.ProductCatalogGroup.Max(x => x.ProductCatalogGroupId);
                id++;
                ProductCatalogGroupFamily f = new ProductCatalogGroupFamily()
                {
                    FamilyName = "",
                    FamilyTypeId = (int)Dal.Helper.ProductCatalogGroupFamilyType.Family,
                    SupplierId = ids
                };
                ProductCatalogGroup pg = new ProductCatalogGroup()
                {
                    GroupName = "",
                    SupplierId = ids,
                    ProductCatalogGroupId = id,
                    ProductCatalogGroupFamily = f
                };

                ctx.ProductCatalogGroup.InsertOnSubmit(pg);

                Supplier s = ctx.Supplier.Where(x => x.SupplierId == ids).FirstOrDefault();
                s.ProductCatalogGroupId = id;
                ctx.SubmitChanges();

                return ids;
            }
        }

        public int SetSupplierOwner(string owner)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int id = ctx.SupplierOwner.Max(x => x.SupplierOwnerId);
                id++;
                SupplierOwner so = new SupplierOwner()
                {
                     Name = owner,
                      SupplierOwnerId = id
                };
                ctx.SupplierOwner.InsertOnSubmit(so);
                ctx.SubmitChanges();
                return id;
            }
        }

        public List<AllegoDelieryCostTypeUserFnResult> GetAllegroDeliveryCostTypeForAllegro(int deliveryCostTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                
                return ctx.AllegoDelieryCostTypeUserFn(deliveryCostTypeId).ToList();
            }
        }

        public void SetOrderProductsDelete(int orderId, int[] ids)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                List<Dal.OrderProduct> orderProducts = ctx.OrderProduct.Where(x => x.OrderId == orderId && !ids.Contains(x.OrderProductId)).ToList();

                foreach(Dal.OrderProduct op in orderProducts)
                {
                    op.Quantity = 0;
                    op.Comment = "Odrzucony przez operatora";
                }
                ctx.SubmitChanges();
            }
        }

        public AllegroDeliveryCostType GetAllegroDeliveryCostType(int deliveryCostTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                return ctx.AllegroDeliveryCostType.Where(x => x.DeliveryCostTypeId == deliveryCostTypeId).FirstOrDefault();
            }
        }

        public void SetAllegroDeliveryCostTypeUser(int deliveryTypeId, long userId, Guid? id, string comment)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.AllegroDeliveryCostTypeUser a = ctx.AllegroDeliveryCostTypeUser.Where(x => x.DeliveryCostTypeId == deliveryTypeId && x.UserId== userId).FirstOrDefault();

                if (a != null)
                {
                    a.LastUpdateDate = DateTime.Now;
                    a.UserId = userId;
                    a.AllegroShippingId = id;
                    a.Comment = comment;
                }
                else

                    ctx.AllegroDeliveryCostTypeUser.InsertOnSubmit(new AllegroDeliveryCostTypeUser()
                    {
                        AllegroShippingId = id,
                        Comment = comment,
                        DeliveryCostTypeId = deliveryTypeId,
                        LastUpdateDate = DateTime.Now,
                        UserId = userId

                    });
                ctx.SubmitChanges();
            }
        }
        public void SetAllegroDeliveryPublished(int deliveryTypeId )
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.AllegroDeliveryCostType a = ctx.AllegroDeliveryCostType.Where(x => x.DeliveryCostTypeId == deliveryTypeId).FirstOrDefault();

                a.IsPublished = true;
 
                ctx.SubmitChanges();
            }
        }

        public ProductCatalogGroup GetProductCatalogGroup(int newGroupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroup.Where(x => x.ProductCatalogGroupId == newGroupId).FirstOrDefault();
            }
            }

        public void SetProductOrderDelete(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductOrder po = ctx.ProductOrder.Where(x => x.Id == id).FirstOrDefault();
                ctx.ProductOrder.DeleteOnSubmit(po);

                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogDeliveryWarehouseView> GetProductCatalogDeliveryWarehouse(DateTime date)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogDeliveryWarehouseView.Where(x => x.InsertDate.Value.Year == date.Year && x.InsertDate.Value.Month == date.Month && x.InsertDate.Value.Day == date.Day).ToList();
            }
        }

        public List<ProductOrderDeliveryView> GetProductOrderDelivery(int batchId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductOrderDeliveryView.Where(x => x.ProductOrderBatchId == batchId).OrderBy(x=>x.ProductName).ToList();
            }
        }

        //public void SetAllegroParcelNumber(List<long> transactionsId)
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        List<Dal.AllegroItemTransaction> transactions = ctx.AllegroItemTransactions.Where(x => transactionsId.Contains(x.TransactionId)).ToList();
        //        foreach (AllegroItemTransaction t in transactions)
        //            t.TrackingNumberSent = true;

        //        ctx.SubmitChanges();
        //    }
        //}

        //public List<AllegroItamTransactionTrackingNumber> GetAllegroParcelNumber()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        return ctx.AllegroItamTransactionTrackingNumbers.ToList();
        //    }
        //}

        public void SetSupplierImportDate(int supplierId, DateTime now)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Supplier supplier = ctx.Supplier.Where(x => x.SupplierId == supplierId).FirstOrDefault();
                supplier.LastImportDate = now;

                if (supplier.IsQuantityTrackingAvailable && supplier.QuantityMinLevel.HasValue && supplier.QuantityMinLevel.Value > 0)
                {
                    List<Dal.ProductCatalog> products = ctx.ProductCatalog.Where(x => x.SupplierId == supplier.SupplierId && x.IsActive && x.SupplierQuantity.HasValue).ToList();
                    foreach (Dal.ProductCatalog product in products)
                    {
                        if(product.SupplierQuantity.Value - supplier.QuantityMinLevel.Value < 0 && product.IsAvailable)
                        {
                            product.IsAvailable = false ;
                            product.UpdateReason = 
                                String.Format("Wyłączony gdyż min. ilość produktów dla producenta została osiągnięta: {0}", supplier.QuantityMinLevel);
                        }
                    }
                }



                ctx.SubmitChanges();
            }
        }

        public List<Order> GetOrdersByOrderProductId(int[] orderProductId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                // dlo.LoadWith<Order>(x => x.ShippingType);
                dlo.LoadWith<Order>(x => x.OrderShipping);
                dlo.LoadWith<OrderShipping>(x => x.ShippingCompany);
                ctx.LoadOptions = dlo;
                return ctx.OrderProduct.Where(x => orderProductId.Contains(x.OrderProductId)).Select(x => x.Order).Distinct().ToList();
            }
        }

        //public void SetProductCatalogAllegroItemErrorFixed(int id, bool isFixed)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ProductCatalogAllegroItem ai = ctx.ProductCatalogAllegroItem.Where(x => x.Id == id).FirstOrDefault();
        //        ai.IsFixed = isFixed;
        //        ctx.SubmitChanges();
        //    }
        //    }

        public void SetInvoiceCorrectionProducts(List<InvoiceProduct> products, string comment)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int invoiceId = 0;
                int[] productIds = products.Select(x => x.InvoiceProductId).ToArray();

                List<InvoiceProduct> productsToUpdate = ctx.InvoiceProduct.Where(x => productIds.Contains(x.InvoiceProductId)).ToList();

                foreach (InvoiceProduct productToUpdate in productsToUpdate)
                {
                    InvoiceProduct product = products.Where(x => x.InvoiceProductId == productToUpdate.InvoiceProductId).FirstOrDefault();
                    productToUpdate.PriceBrutto = product.PriceBrutto;
                    productToUpdate.Quantity = product.Quantity;
                    productToUpdate.VatRate = product.VatRate;
                    productToUpdate.Rebate = product.Rebate;
                    productToUpdate.Name = product.Name;

                    invoiceId = productToUpdate.InvoiceId;

                }

                Dal.Invoice invoice = ctx.Invoice.Where(x => x.InvoiceId == invoiceId).FirstOrDefault();

                invoice.Comment = comment;


                ctx.SubmitChanges();
            }
        }

        public void SetShopSupplier(int supplierId, int[] shopIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.SupplierShop.InsertAllOnSubmit(
                    shopIds.Select(x =>
                    new SupplierShop()
                    {
                        ShopId = x,
                        SupplierId = supplierId

                    }

                    ).ToList());
                ctx.SubmitChanges();


            }
        }


        public void SetShopSupplierDelete(int supplierId, int[] shopIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<SupplierShop> shops = ctx.SupplierShop.Where(x => x.SupplierId == supplierId && shopIds.Contains(x.ShopId)).ToList();
                ctx.SupplierShop.DeleteAllOnSubmit(shops);
                ctx.SubmitChanges();


            }
        }
        public void SetProductCatalogImageFriendlyName(ProductCatalogImage image)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogImage imageToUpdate = ctx.ProductCatalogImage.Where(x => x.ImageId == image.ImageId).FirstOrDefault();
                imageToUpdate.FriendlyFileName = image.FriendlyFileName;
                ctx.SubmitChanges();
            }
        }

 

        public int SetInvoiceCorrection(Invoice invoice)
        {
            using(LajtitDB ctx = new LajtitDB())
            {

                Invoice invoiceToUpdate = ctx.Invoice.Where(x => x.InvoiceId == invoice.InvoiceId).FirstOrDefault();

                //int? lastId = null;


                Invoice invoiceToAdd = new Invoice()
                {
                    Address = invoiceToUpdate.Address,
                    City = invoiceToUpdate.City,
                    Comment = invoiceToUpdate.Comment,
                    CompanyId = invoiceToUpdate.CompanyId,
                    CompanyName = invoiceToUpdate.CompanyName,
                    Email = invoiceToUpdate.Email,
                    ExcludeFromInvoiceReport = invoiceToUpdate.ExcludeFromInvoiceReport,
                    InvoiceDate = DateTime.Now,
                    InvoiceFileName = invoiceToUpdate.InvoiceFileName,
                    //InvoiceNumber = String.Format("{0}/K/{1:yyyy}", lastId, DateTime.Now),
                    InvoiceSellDate = DateTime.Now,
                    //InvoiceSeqNo = lastId,
                    InvoiceTypeId = 3,
                    IsDeleted = false,
                    IsLocked = false,
                    Nip = invoiceToUpdate.Nip,
                    Postcode = invoiceToUpdate.Postcode
                };

                if (invoiceToAdd.InvoiceTypeId == 0)
                    invoiceToAdd.InvoiceTypeId = 2;

                ctx.Invoice.InsertOnSubmit(invoiceToAdd);

                //ctx.SubmitChanges();

                List<InvoiceProduct> invoiceProducts = ctx.InvoiceProduct.Where(x => x.InvoiceId == invoice.InvoiceId).ToList();
                //foreach(InvoiceProduct product in invoiceProducts)
                foreach(InvoiceProduct product in invoiceProducts)
                {

                    InvoiceProduct productToAdd = new InvoiceProduct()
                    {
                        Invoice = invoiceToAdd,//.InvoiceId,
                        ProductCatalogId = product.ProductCatalogId,
                        MeasureName = product.MeasureName,
                        Name = product.Name,
                        Quantity = product.Quantity,
                        Rebate = product.Rebate,
                        VatRate = product.VatRate,
                        PriceBrutto = product.PriceBrutto,
                    };
                    ctx.InvoiceProduct.InsertOnSubmit(productToAdd);
                    //  ctx.SubmitChanges();
                    //InvoiceProduct ip = ctx.InvoiceProduct.Where(x => x.InvoiceProductId == product.InvoiceProductId).FirstOrDefault();
                    //ip.InvoiceProductCorrectionId = productToAdd.InvoiceProductId;
                     product.InvoiceProduct1 = productToAdd;
                   // ctx.SubmitChanges();

                }
                invoiceToUpdate.Invoice1 = invoiceToAdd;//.InvoiceId;
                ctx.SubmitChanges();

                return invoiceToAdd.InvoiceId;
            }
        }

        internal void SetOrderProducts(List<OrderProduct> orderProdcuts)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.OrderProduct.InsertAllOnSubmit(orderProdcuts);
                ctx.SubmitChanges();
            }
        }

        public void InsertUpdateInvoiceExcludeFromInvoiceReport(int orderId, bool exclude, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Invoice invoice = ctx.Order.Where(x => x.OrderId == orderId).Select(x => x.Invoice).FirstOrDefault();

                if (invoice != null)
                    invoice.ExcludeFromInvoiceReport = exclude;

                ctx.SubmitChanges();
            }
        }

        //public void SetShopCategoryFieldDelete(int categoryId, int fieldId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ShopCategoryField field = ctx.ShopCategoryField
        //            .Where(x => x.FieldId == fieldId && x.CategoryId == categoryId)
        //            .FirstOrDefault();
        //        ctx.ShopCategoryField.DeleteOnSubmit(field);
        //        ctx.SubmitChanges();
        //    }
        //}

        private static void UpdateOrder(Order order, LajtitDB ctx, Dal.Order exitingOrder)
        {  
            exitingOrder.Phone = order.Phone;
            exitingOrder.Phone2 = order.Phone2; 
            exitingOrder.ShipmentAddress = order.ShipmentAddress;
            exitingOrder.ShipmentCity = order.ShipmentCity;
            exitingOrder.ShipmentCompanyName = order.ShipmentCompanyName;
            exitingOrder.ShipmentFirstName = order.ShipmentFirstName;
            exitingOrder.ShipmentLastName = order.ShipmentLastName;
            exitingOrder.ShipmentPostcode = order.ShipmentPostcode;
            //exitingOrder.ShippintTypeId = order.ShippintTypeId;
            exitingOrder.ShippingCost = order.ShippingCost;
            //exitingOrder.ShippingData = order.ShippingData;

            if (order.Invoice != null)
            {
                Invoice invoice = new Invoice()
                {
                    Address = order.Invoice.Address,
                    City = order.Invoice.City,
                    CompanyName = order.Invoice.CompanyName,
                    Email = order.Invoice.Email,
                    InvoiceDate = order.Invoice.InvoiceDate,
                    IsDeleted = false,
                    Nip = order.Invoice.Nip,
                    Postcode = order.Invoice.Postcode,
                    CompanyId = Dal.Helper.DefaultCompanyId,
                    InvoiceTypeId = order.Invoice.InvoiceTypeId,
                    Comment = order.Invoice.Comment

                };
                exitingOrder.Invoice = invoice;

            }
            else
                exitingOrder.Invoice = null;

            TableLog log = new TableLog()
            {
                Comment = "Aktualizacja danych zamówienia na podstawie formularza dostawy",
                InsertDate = DateTime.Now,
                InsertUser = "System",
                ObjectId = order.OrderId,
                TableName = "Order"
            };
            ctx.TableLog.InsertOnSubmit(log);
        }

        public AllegroUserByOrderResult GetAllegroUserByOrder(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                try
                {
                    return ctx.AllegroUserByOrder(orderId).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public void SetShopCategoryField(ShopCategoryField field)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ShopCategoryField.InsertOnSubmit(field);


                ctx.SubmitChanges();
            }
        }

        public void SetShopCategoryFieldUpdate(ShopCategoryField field)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ShopCategoryField fieldToUpdate = ctx.ShopCategoryField
                    .Where(x => x.Id == field.Id).FirstOrDefault();

                fieldToUpdate.Description = field.Description;
                fieldToUpdate.FieldType = field.FieldType;
                fieldToUpdate.FloatValue = field.FloatValue;
                fieldToUpdate.IntValue = field.IntValue;
                fieldToUpdate.PassToShop = field.PassToShop;
                fieldToUpdate.StringValue = field.StringValue;
                fieldToUpdate.UseDefaultValue = field.UseDefaultValue;
                fieldToUpdate.AttributeGroupId = field.AttributeGroupId;
                fieldToUpdate.AttributeId = field.AttributeId;
                fieldToUpdate.UpdateParameter = field.UpdateParameter;
                fieldToUpdate.IsRequired = field.IsRequired;
                fieldToUpdate.UseDefaultAttribute = field.UseDefaultAttribute;
                fieldToUpdate.SystemFieldId = field.SystemFieldId;
                fieldToUpdate.CategoryFieldId = field.CategoryFieldId == "" ? null : field.CategoryFieldId;


                ctx.SubmitChanges();
            }
        }

        public OrderDeliveryTimeFnResult GetOrderDeliveryTime(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderDeliveryTimeFn(orderId).FirstOrDefault();
            }
        }

        public object GetOrderProductStauses()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderProductStatus.OrderBy(x => x.StatusName).ToList();
            }
        }

        public int InsertUpdateOrderProduct(OrderProduct product)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                // zakomentowane gdyz brane są zawsze nowe AllegroItemOrder
                //Dal.OrderProduct exitingOrder = ctx.OrderProduct
                //    .Where(x => x.ExternalProductId == product.ExternalProductId
                //        && x.OrderId == product.OrderId)
                //    .FirstOrDefault();

                //if (exitingOrder != null)
                //{
                //    exitingOrder.Price = product.Price;
                //    exitingOrder.Quantity = product.Quantity;
                //    ctx.SubmitChanges();
                //    return exitingOrder.OrderProductId;
                //}
                //else
                {
                    ctx.OrderProduct.InsertOnSubmit(product);
                    ctx.SubmitChanges();
                    return product.OrderProductId;
                }
            }
        }

        public bool GetCanMoveProductsFromOrder(int[] productIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return 
                    ctx.OrderProduct.Where(x => productIds.Contains(x.OrderProductId) && x.OrderProductStatus.CanMove == true).Count() > 0 
                    && 
                    ctx.OrderProduct.Where(x => productIds.Contains(x.OrderProductId) && x.PriceTotal > 0).Count() > 0;

            }
        }

        public AllegroDeliveryFnResult GetAllegroDelivery(int productCatalogId, long userId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.AllegroDeliveryFn(productCatalogId, userId).FirstOrDefault();
            }
        }

        public List<OrdersView> GetOrders(string orderId,
            bool? isPaid,
            bool isPayOnDelivery,
            int[] orderStatusIds,
            int[] shippingCompanyIds,
            string name,
            string invoiceNumber,
            int? productCatalogId,
            int? hasInvoiceParagon,
            DateTime? dateFrom,
            DateTime? dateTo,
            int[] shopIds,
            int userShopId,
            string userName)
        {

            using (LajtitViewsDB ctxv = new LajtitViewsDB())
            {
                using (LajtitDB ctx = new LajtitDB())
                {
                    var q = ctxv.OrdersView
                        .AsQueryable();

                    if (userShopId != 0)
                    {
                        //int[] acceptedStatuses = new int[] { (int)Dal.Helper.OrderStatus.WaitingForDelivery, (int)Dal.Helper.OrderStatus.ReadyToSend, (int)Dal.Helper.OrderStatus.ClientContact };
                        //int[] acceptedShipping = new int[] { (int)Dal.Helper.ShippingType.SelfPaymentByDelivery, (int)Dal.Helper.ShippingType.SelfDelivery };
                        //q = q.Where(x => x.ShopId == userShopId || (acceptedStatuses.Contains(x.OrderStatusId) && acceptedShipping.Contains(x.ShippintTypeId)));
                        q = q.Where(x => x.CreateDate > DateTime.Now.AddMonths(-2) || x.ShopId == userShopId);
                        q = q.Where(x => x.ShopId!=26);
                    }

                    if (isPaid.HasValue)
                    {
                        if (isPaid.Value)
                            q = q.Where(x => x.AmountPaid > 0);
                        else
                            q = q.Where(x => x.AmountPaid == 0 && !x.PayOnDelivery.Value);
                    }
                    if (isPayOnDelivery)
                        q = q.Where(x => x.PayOnDelivery.Value);




                    if (orderId != null)
                    {
                        int oId = 0;
                        if(Int32.TryParse(orderId, out oId))
                        {
                            return q.Where(x => x.ExternalOrderNumber == orderId || x.OrderId == oId).ToList();
                        }
                        else
                            return q.Where(x => x.ExternalOrderNumber == orderId).ToList();
                    }

                    if (!String.IsNullOrEmpty(name))
                    { 
                        int[] orderIds = Dal.DbHelper.Orders.GetOrderShippingByTrackingNumber(name).Select(x => x.OrderId).ToArray() ;
                    
                        q = q.Where(x => (x.Email.StartsWith(name) || x.UserName.StartsWith(name) 
                        || x.Client.Contains(name)  || orderIds.Contains(x.OrderId)
                        || x.PhoneClear.Contains(name)  ));



                    }

                    if (orderStatusIds.Length > 0)
                        q = q.Where(x => orderStatusIds.Contains(x.OrderStatusId));

                    if (shippingCompanyIds.Length > 0)
                    {
                        q = q.Where(x => shippingCompanyIds.Contains(x.ShippingCompanyId.Value));
                    }
                    if (shopIds.Length > 0)
                    {
                        q = q.Where(x => shopIds.Contains(x.ShopId));
                    }
                    if (!String.IsNullOrEmpty(invoiceNumber))
                    {
                        if (hasInvoiceParagon == 3)
                            q = q.Where(x => (x.ParNumber == invoiceNumber));
                        if (hasInvoiceParagon == 1)
                            q = q.Where(x => (x.InvoiceNumber == invoiceNumber || x.InvoiceData.Contains(invoiceNumber)));
                    }

                    if (hasInvoiceParagon.HasValue)
                    {
                        switch (hasInvoiceParagon.Value)
                        {
                            case 1: q = q.Where(x => x.HasInvoice == true); break;
                            case 2: q = q.Where(x => x.HasInvoice == false); break;
                            case 3: q = q.Where(x => x.HasParagon == true); break;
                        }
                    }

                    if (productCatalogId.HasValue)
                    {
                        // int[] orderIds = q.Select(x => x.OrderId).ToArray();
                        int[] orderIdsWithProduct = ctx.OrderProduct.Where(x => x.ProductCatalogId == productCatalogId.Value)
                            .Select(x => x.OrderId).ToArray();
                        //var results = q.ToList();
                        q = q.Where(x => orderIdsWithProduct.Contains(x.OrderId));
                    }
                    if (dateFrom.HasValue)
                        q = q.Where(x => x.CreateDate >= dateFrom);

                    if (dateTo.HasValue)
                        q = q.Where(x => x.CreateDate <= dateTo);

                    bool fullView = false;

                    int[] roleIds =  ctx.SystemPageActionRole.Where(x => x.SystemPageAction.GuidId == Guid.Parse("da9b8d5f-d109-452e-83d5-bf82a3d63128"))
                        .Select(x => x.RoleId)
                        .ToArray();

                    if (ctx.SystemUserRole.Where(x => x.AdminUser.UserName == userName && roleIds.Contains(x.RoleId)).Count() > 0)
                        fullView = true;


                    if (!fullView)
                        q = q.Where(x => x.CreateDate > DateTime.Now.AddMonths(-25));

                    return q.ToList();
                }
            }
        }



        public Dal.Invoice SetOrderInvoiceUnlock(int orderId, string actingUser)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Invoice invoice = ctx.Order.Where(x => x.OrderId == orderId).Select(x => x.Invoice).FirstOrDefault();

                if(invoice!=null)
                {
                    invoice.IsLocked = false;

                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                      String.Format("Oblokowano fakturę id: {0}", invoice.InvoiceId)
                      , actingUser
                      , orderId
                      , "Order"));
                    ctx.SubmitChanges();


                    ctx.SubmitChanges();
                    return invoice;
                }
                return null;
            }
            }

        public List<OrderProduct> GetOrderProducts(int[] orderProductsId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                int[] orderIds = ctx.OrderProduct.Where(x => orderProductsId.Contains(x.OrderProductId))
                    .Select(x => x.OrderId).Distinct().ToArray();


                return ctx.OrderProduct.Where(x => orderIds.Contains(x.OrderId) && x.Quantity > 0).ToList();


            }
        }
        public List<OrderProductsView> GetOrderProducts(int orderId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {

                return ctx.OrderProductsView.Where(x => x.OrderId == orderId)
                    .OrderByDescending(x=>x.IsOrderProduct)
                    .ThenBy(x => x.SupplierName)
                    .ToList();
            }
        }
        public List<OrderProductsForWarehouseView> GetOrderProductsForWarehouse(int orderId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {

                return ctx.OrderProductsForWarehouseView.Where(x => x.OrderId == orderId)
                   
                    .ToList();
            }
        }

        public OrderProduct GetOrderProduct(int orderProductId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                return ctx.OrderProduct.Where(x => x.OrderProductId == orderProductId).FirstOrDefault();
            }
        }
        /// <summary>
        /// Aktualizuje ilosc, cene i komentarz oraz loguje zmiany
        /// </summary>
        /// <param name="orderProduct"></param>
        /// <param name="changeLog"></param>
        /// <param name="p"></param>
        public void UpdateOrderProduct(OrderProduct orderProduct,  string changeLog, string actingUser)
        {
            //using (LajtitViewsDB ctxv = new LajtitViewsDB())
            //{
                using (LajtitDB ctx = new LajtitDB())
                {
                    //List<OrderProduct> subProductsToDelete = ctx.OrderProduct
                    //    .Where(x => x.ProductTypeId == 2 && x.SubOrderProductId == orderProduct.OrderProductId)
                    //    .ToList();

                    //ctx.OrderProduct.DeleteAllOnSubmit(subProductsToDelete);

                   //List<OrderProduct> orderProductsToAdd = new List<OrderProduct>();

                    //var products = subProductCatalogIDs.Select(x =>
                    //    new
                    //    {
                    //        ProductCatalogId = Convert.ToInt32(x.Split('|')[0]),
                    //        Quantity = Convert.ToInt32(x.Split('|')[1]),

                    //    }).ToList();
                    //int[] productIDs = products.Select(x => x.ProductCatalogId).ToArray();

                    //List<ProductCatalogView> pcv = ctxv.ProductCatalogView.Where(x => productIDs.Contains(x.ProductCatalogId)).ToList();

                    //foreach (var q in products)
                    //{
                    //    ProductCatalogView p = pcv.Where(x => x.ProductCatalogId == q.ProductCatalogId).FirstOrDefault();
                    //    if (p != null)
                    //    {
                    //        OrderProduct op = new OrderProduct()
                    //        {
                    //            Comment = "Sub Produkt",
                    //            ExternalProductId = q.ProductCatalogId,
                    //            OrderId = orderProduct.OrderId,
                    //            Price = 0,//p.PurchasePrice,
                    //            ProductCatalogId = q.ProductCatalogId,
                    //            ProductName = p.Name,
                    //            ProductTypeId = (int)Dal.Helper.ProductType.ComponentProduct,
                    //            Quantity = q.Quantity,
                    //            // DeliveryId = p.CurrentDeliveryId,
                    //            Rebate = 0,
                    //            SubOrderProductId = orderProduct.OrderProductId,
                    //            VAT = 0,
                    //            OrderProductStatusId = 1,
                    //            CurrencyRate=orderProduct.CurrencyRate,
                    //            PriceCurrency = orderProduct.PriceCurrency
                                
                    //        };

                    //        orderProductsToAdd.Add(op);
                    //    }
                    //}

                    //if (orderProduct.ProductCatalogId.HasValue)
                    //{
                    //    Dal.ProductCatalogHelper pch = new ProductCatalogHelper();
                    //    Dal.ProductCatalog pc = Dal.DbHelper.ProductCatalog.GetProductCatalog(orderProduct.ProductCatalogId.Value);

                    //    if (pc.ProductTypeId == (int)Dal.Helper.ProductType.ComboProduct)
                    //    {

                    //        List<OrderProduct> subProducts = ctx.OrderProduct
                    //    .Where(x => x.ProductTypeId == (int)Dal.Helper.ProductType.ComboProduct
                    //    && x.SubOrderProductId == orderProduct.OrderProductId)
                    //    .ToList();

                    //        foreach (Dal.OrderProduct sub in subProducts)
                    //        {
                    //            Dal.ProductCatalogSubProduct sp = ctx.ProductCatalogSubProduct.Where(x => x.ProductCatalogId == orderProduct.ProductCatalogId.Value && x.ProductCatalogRefId == sub.ProductCatalogId).FirstOrDefault();

                    //            if (sp == null)
                    //                ctx.OrderProduct.DeleteOnSubmit(sub);
                    //            else
                    //            {
                    //                sub.Quantity = sp.Quantity * orderProduct.Quantity;
                    //            }


                    //        }

                    //    }

                    //}

                    Dal.OrderProduct existingProduct = ctx.OrderProduct.Where(x => x.OrderProductId == orderProduct.OrderProductId).FirstOrDefault();
                    existingProduct.Quantity = orderProduct.Quantity;
                    existingProduct.Price = orderProduct.Price;
                    existingProduct.Comment = orderProduct.Comment;
                    existingProduct.Rebate = orderProduct.Rebate;
                    existingProduct.VAT = orderProduct.VAT;
                    existingProduct.ProductCatalogId = orderProduct.ProductCatalogId;
                    existingProduct.LastUpdateReason = orderProduct.LastUpdateReason;
                    existingProduct.LastUpdateDate = orderProduct.LastUpdateDate;
                    existingProduct.OrderProductStatusId = orderProduct.OrderProductStatusId;

                    ctx.TableLog.InsertOnSubmit(GetLogEntry(changeLog, actingUser, orderProduct.OrderProductId, "OrderProduct"));

                    //if (orderProductsToAdd.Count > 0)
                    //    ctx.OrderProduct.InsertAllOnSubmit(orderProductsToAdd);

                    ctx.SubmitChanges();
                }
            //}
        }
        public void UpdateOrder(Order order,  string changeLog, string actingUser)
        {
            using (LajtitViewsDB ctxv = new LajtitViewsDB())
            {
                using (LajtitDB ctx = new LajtitDB())
                {
                   

                    Dal.Order existingOrder = ctx.Order.Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                    existingOrder.ShippingCost = order.ShippingCost;
                    existingOrder.ShippingCostVAT = order.ShippingCostVAT;
          
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(changeLog, actingUser, order.OrderId, "Order"));

                   

                    ctx.SubmitChanges();
                }
            }
        }

        public void SerOrderPaymentToNewOrder(int orderId, int newOrderId, string userName)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Order order = ctx.Order.Where(x => x.OrderId == newOrderId && x.OrderShipping.COD.HasValue == false).FirstOrDefault();

                if (order == null)
                    return;

                Dal.OrderPayment op = ctx.OrderPayment.Where(x => x.OrderId == orderId)
                    .Where(x => x.OrderPaymentType.AllowNegative == false && x.Amount > 0 && x.Amount >= order.AmountToPay).FirstOrDefault();

                if (op != null)
                {
                    Dal.OrderPayment opToAdd = new OrderPayment()
                    {
                        Amount = order.AmountToPay,
                        Comment = op.Comment,
                        ExternalPaymentId = op.ExternalPaymentId,
                        InsertDate = op.InsertDate,
                        InsertUser = String.Format("{0}/{1}", userName, op.InsertUser),
                        NotForEvidence = op.NotForEvidence,
                        OrderId = newOrderId,
                        PaymentTypeId = op.PaymentTypeId,
                        CurrencyCode = op.CurrencyCode,
                        CurrencyRate = op.CurrencyRate,
                        AmountCurrency = op.AmountCurrency,
                        AccountingTypeId = op.AccountingTypeId
                    };
                    op.Amount -= order.AmountToPay;

                    ctx.OrderPayment.InsertOnSubmit(opToAdd);
                    ctx.SubmitChanges();
                }
            }
        }

        public List<OrderInpostView> GetOrdersForInpost()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                int[] availableStatusIds = new int[] { (int)Dal.Helper.OrderStatus.ReadyToSend, (int)Dal.Helper.OrderStatus.Sent };
                return ctx.OrderInpostView.Where(x => availableStatusIds.Contains(x.OrderStatusId)).ToList();
            }
        }
        public List<OrderInpostView> GetOrdersForInpost(List<string> inpostShipmentIds)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            { 
                return ctx.OrderInpostView.Where(x => x.InpostShipmentId!=null && inpostShipmentIds.Contains(x.InpostShipmentId)).ToList();
            }
        }

        /// <summary>
        /// Sprawdza czy oferta jest w trakcie tworzenia w Allegro.
        /// </summary>
        /// <param name="productCatalogId"></param>
        /// <returns></returns>
        //public bool GetProductCatalogAllegroItemCreating(int productCatalogId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //      //  int[] statuses = new int[] {(int)Dal.Helper.ProductAllegroItemBatchStatus.}
        //        bool existsInCreating =  ctx.ProductCatalogAllegroItem.Where(x => 
        //        x.ProductCatalogId == productCatalogId
        //        && x.ItemId == null
        //        && x.AllegroItemStatusId != (int)Dal.Helper.ProductAllegroItemStatus.VerifiedError
        //       //&& x.ProductCatalogAllegroItemBatch.BatchStatusId != (int)Dal.Helper.ProductAllegroItemBatchStatus.Deleted
        //        ).Count() != 0;

        //        return existsInCreating;
        //    }
        //}
        private TableLog GetLogEntry(string changeLog, string actingUser, int objectId, string tableName)
        {
            Dal.TableLog log = new TableLog()
            {
                Comment = changeLog,
                InsertDate = DateTime.Now,
                InsertUser = actingUser,
                ObjectId = objectId,
                TableName = tableName
            };

            return log;
        }

        public List<Dal.OrderStatus> GetStatuses(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int orderStatusId = ctx.Order.Where(x => x.OrderId == orderId).Select(x => x.OrderStatusId).FirstOrDefault();
                List<Dal.OrderStatusWorkFlow> workflow = ctx.OrderStatusWorkFlow
                    .Where(x => x.CurrentOrderStatusId == orderStatusId) 
                    .ToList();

                var w = workflow.Where(x => x.CheckForShippingType.HasValue && x.CheckForShippingType.Value);

                //if (w.Count() > 0)
                //{
                //    List<Dal.OrderStatusWorkFlow> toDelete = new List<OrderStatusWorkFlow>();
                //    Dal.Order order = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();

                //    foreach (var i in w)
                //    { 
                //        var s = ctx.OrderStatusWorkFlowShippingType
                //            .Where(x => x.OrderStatusId == i.NextOrderStatusId && x.ShippingTypeId == order.ShippintTypeId);

                //        if (s.Count() == 0)
                //            toDelete.Add(i);

                //    }
                //    if(toDelete.Count>0)
                //        workflow.RemoveAll(x=>toDelete.Contains(x));

                //}

                int[] statusIds = workflow.Select(x => x.NextOrderStatusId).ToArray();
                List<Dal.OrderStatus> statuses = ctx.OrderStatus

                    .Where(x => statusIds.Contains(x.OrderStatusId))
                    .OrderBy(x=>x.SortOrderId)
                    .ToList();

                return statuses;
            }
        }

        public void SetOrderStatus(Dal.OrderStatusHistory osh, Dal.ComplaintStatusHistory oc)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Order order = ctx.Order.Where(x => x.OrderId == osh.OrderId).FirstOrDefault();
                string oldStatus = ctx.OrderStatus.Where(x => x.OrderStatusId == order.OrderStatusId).FirstOrDefault().StatusName;
                Dal.OrderStatus newStatus = ctx.OrderStatus.Where(x => x.OrderStatusId == osh.OrderStatusId).FirstOrDefault();

                if (newStatus.ChangesStatus == true)
                    order.OrderStatusId = osh.OrderStatusId;

                ctx.TableLog.InsertOnSubmit(GetLogEntry(
                    String.Format("Zmiana statusu z {0} na {1}", oldStatus, newStatus.StatusName)
                    , osh.InsertUser
                    , osh.OrderStatusId
                    , "Order"));
                ctx.OrderStatusHistory.InsertOnSubmit(osh);

                if (oc != null)
                    ctx.ComplaintStatusHistory.InsertOnSubmit(oc);

                ctx.SubmitChanges();
            }
        }

        public void SetOrderUpdate(Order order, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Order orderToUpdate = ctx.Order.Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                orderToUpdate.DeliveryDate = order.DeliveryDate;
                orderToUpdate.OrderPriority = order.OrderPriority;

                ctx.SubmitChanges();
            }
        }

        //public void SetOrderInpostShipmentId(int orderId, string inpostShipmentId, string serviceCode)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        Dal.Order order = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
        //        order.InpostShipmentId = inpostShipmentId;
        //        order.InpostServiceCode = serviceCode;
        //        order.ShipmentTrackingNumber = null;
        //        order.TrackingNumberSent = null; 
        //        ctx.SubmitChanges();
        //    }
        //}
 
        public void SetShipmentAddressUpdate(Order order, string actingUser)
        {
            using (LajtitDB ctx = new LajtitDB())
            {


                Dal.Order o = ctx.Order.Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                #region
                if (o.Email != order.Email)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana adresu email z {0} na {1}", o.Email, order.Email)
                        , actingUser
                        , order.OrderId
                        , "Order"));
                    o.Email = order.Email;
                }
                if (o.ShipmentAddress != order.ShipmentAddress)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana adresu wysyłki. ShipmentAddress z {0} na {1}", o.ShipmentAddress, order.ShipmentAddress)
                        , actingUser
                        , order.OrderId
                        , "Order"));
                    o.ShipmentAddress = order.ShipmentAddress;
                }
                if (o.ShipmentCity != order.ShipmentCity)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana adresu wysyłki. ShipmentCity z {0} na {1}", o.ShipmentCity, order.ShipmentCity)
                        , actingUser
                        , order.OrderId
                        , "Order"));
                    o.ShipmentCity = order.ShipmentCity;
                }
                if (o.ShipmentCompanyName != order.ShipmentCompanyName)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana adresu wysyłki. ShipmentCompanyName z {0} na {1}", o.ShipmentCompanyName, order.ShipmentCompanyName)
                        , actingUser
                        , order.OrderId
                        , "Order"));
                    o.ShipmentCompanyName = order.ShipmentCompanyName;
                }
                if (o.ShipmentFirstName != order.ShipmentFirstName)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana adresu wysyłki. ShipmentFirstName z {0} na {1}", o.ShipmentFirstName, order.ShipmentFirstName)
                        , actingUser
                        , order.OrderId
                        , "Order"));
                    o.ShipmentFirstName = order.ShipmentFirstName;
                }

                if (o.ShipmentLastName != order.ShipmentLastName)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana adresu wysyłki. ShipmentLastName z {0} na {1}", o.ShipmentLastName, order.ShipmentLastName)
                        , actingUser
                        , order.OrderId
                        , "Order"));
                    o.ShipmentLastName = order.ShipmentLastName;
                }
                if (o.ShipmentPostcode != order.ShipmentPostcode)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana adresu wysyłki. ShipmentPostcode z {0} na {1}", o.ShipmentPostcode, order.ShipmentPostcode)
                        , actingUser
                        , order.OrderId
                        , "Order"));
                    o.ShipmentPostcode = order.ShipmentPostcode;
                }
                if (o.Phone != order.Phone)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana adresu wysyłki. Phone z {0} na {1}", o.Phone, order.Phone)
                        , actingUser
                        , order.OrderId
                        , "Order"));
                    o.Phone = order.Phone;
                }
                if (o.Phone2 != order.Phone2)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana adresu wysyłki. Phone2 z {0} na {1}", o.Phone2, order.Phone2)
                        , actingUser
                        , order.OrderId
                        , "Order"));
                    o.Phone2 = order.Phone2;
                }
                if (o.ShipmentCountryCode != order.ShipmentCountryCode)
                {
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zmiana kraju wysyłki. ShipmentCountryCode z {0} na {1}", o.ShipmentCountryCode, order.ShipmentCountryCode)
                        , actingUser
                        , order.OrderId
                        , "Order"));
                    o.ShipmentCountryCode = order.ShipmentCountryCode;
                }
                #endregion
                ctx.SubmitChanges();
            }
        }

        //public void SetOrderInpostShipmentsCancel(List<string> shipments)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        List<Order> orders = ctx.Order.Where(x => x.InpostShipmentId != null && shipments.Contains(x.InpostShipmentId)).ToList();

        //        foreach (Dal.Order order in orders)
        //            order.InpostShipmentId = null;

        //        ctx.SubmitChanges();
        //    }
        //}

        public Invoice GetInvoiceByOrderId(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Invoice>(x => x.Company);
                ctx.LoadOptions = dlo;

                return ctx.Order.Where(x => x.OrderId == orderId).Select(x=>x.Invoice).FirstOrDefault();
            }
        }
        public Invoice GetInvoice(int invoiceId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Invoice>(x => x.Company);
                ctx.LoadOptions = dlo;

                return ctx.Invoice.Where(x => x.InvoiceId == invoiceId).FirstOrDefault();
            }
        }
         
        public void SetInvoiceUpdate(int orderId, Invoice invoice, string actingUser)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                Dal.Invoice i = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault().Invoice;
                Dal.Order o = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();

                if (i == null)
                {
                    invoice.CountryCode = invoice.CountryCode == "" ? null : invoice.CountryCode;
                    o.Invoice = invoice;
                    
                    ctx.SubmitChanges();


                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Dodano fakturę id: {0}", invoice.InvoiceId)
                        , actingUser
                        , orderId
                        , "Order"));
                    ctx.SubmitChanges();
                }
                else
                {
                    i.Address = invoice.Address;
                    i.City = invoice.City;
                    i.CompanyName = invoice.CompanyName;
                    i.Nip = invoice.Nip;
                    i.Postcode = invoice.Postcode;
                    i.Email = invoice.Email;
                    i.InvoiceTypeId = invoice.InvoiceTypeId;
                    i.IsLocked = invoice.IsLocked;
                    i.Comment = invoice.Comment;
                    i.CountryCode = invoice.CountryCode == "" ? null : invoice.CountryCode;
                    i.ExcludeFromInvoiceReport = invoice.ExcludeFromInvoiceReport;
                    if (i.InvoiceTypeId == 0)
                        i.InvoiceTypeId = 2;

                    if (i.InvoiceNumber == null)
                        i.CompanyId = invoice.CompanyId;

                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Zaktualizowano fakturę id: {0}", i.InvoiceId)
                        , actingUser
                        , orderId
                        , "Order"));

                    ctx.SubmitChanges();
                }

            }
        }

        public void SetInvoiceDelete(int orderId, string actingUser)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Order order = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
                if (order.Invoice != null)
                {

                    Dal.Invoice invoice = ctx.Invoice.Where(x => x.InvoiceId == order.InvoiceId).FirstOrDefault();

                    order.Invoice = null;
                    invoice.IsDeleted = true;

                    ctx.TableLog.InsertOnSubmit(GetLogEntry(
                        String.Format("Usunięto fakturę id: {0}", invoice.InvoiceId)
                        , actingUser
                        , orderId
                        , "Order"));
                    ctx.SubmitChanges();

                }
            }
        }
 

        //public int[] GetProductCatalogIDsForOrder(int orderId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.OrderProduct
        //            .Where(x => x.OrderId == orderId && x.ProductCatalogId.HasValue)
        //            .Select(x => x.ProductCatalogId.Value).ToArray();
        //    }
        //}
        public List<ProductCatalogView> GetProductCatalogsForOrder(int orderId, int supplierId, int? doNotExcludeProductId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                //int[] pcs = ctx.OrderProduct
                //    .Where(x => x.OrderId == orderId && x.ProductCatalogId.HasValue)
                //    .Select(x => x.ProductCatalogId.Value).ToArray();
                //if (doNotExcludeProductId.HasValue)
                //    pcs = pcs.Where(x => x != doNotExcludeProductId.Value).ToArray();

                return ctx.ProductCatalogView
                    .Where(x => 
                    //x.IsActive                        && 
                        x.SupplierId == supplierId
                        //&& !pcs.Contains(x.ProductCatalogId)
                        )
                    .OrderBy(x => x.Name)
                    .ToList();
            }
        }

        public ProductCatalogView GetProductCatalog(int productCatalogId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogView.Where(x => /*x.IsActive == true &&*/ x.ProductCatalogId == productCatalogId).FirstOrDefault();
            }
        }
        public ProductCatalogViewFnResult GetProductCatalogFn( int productCatalogId, int warehouseId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogViewFn(warehouseId).Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();
            }
        }

        public void InsertProductToOrder(OrderProduct op, string actingUser)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.OrderProduct.InsertOnSubmit(op);
                ProductCatalog pc = ctx.ProductCatalog.Where(x => x.ProductCatalogId == op.ProductCatalogId).FirstOrDefault();
                ctx.TableLog.InsertOnSubmit(GetLogEntry(
                    String.Format("Dodano produkt: {0}, ilość: {1}, cena: {2}", pc.Name, op.Quantity, op.Price)
                    , actingUser
                    , op.OrderId
                    , "Order"));

                ctx.SubmitChanges();
            }
        }

        public void SerOrderProductsToNewOrder(int orderId, int newOrderId, int[] orderProductToMove, string actingUser)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<OrderProduct> orderProducts = ctx.OrderProduct.Where(x => orderProductToMove.Contains(x.OrderProductId) && x.OrderId == orderId).ToList();




                foreach (OrderProduct op in orderProducts)
                {
                    op.OrderId = newOrderId;

                    ctx.TableLog.InsertOnSubmit(GetLogEntry(String.Format("Produkt (OrderProduct) {0} przeniesiony z zamówienia {1} do {2}",
                        op.OrderProductId, orderId, newOrderId), actingUser, orderId, "Order"));
                    ctx.TableLog.InsertOnSubmit(GetLogEntry(String.Format("Produkt (OrderProduct) {0} przeniesiony z zamówienia {1} do {2}",
                        op.OrderProductId, orderId, newOrderId), actingUser, newOrderId, "Order"));


                    List<OrderProduct> subProducts = ctx.OrderProduct.Where(x => x.SubOrderProductId == op.OrderProductId).ToList();

                    foreach (OrderProduct sp in subProducts)
                    {
                        sp.OrderId = newOrderId;
                    }


                    }


                Order order = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
                string tmp = order.ShipmentAddress;
                order.ShipmentAddress = "XXXXXX";
                ctx.SubmitChanges();
                // hack
                order.ShipmentAddress = tmp;
                ctx.SubmitChanges();
            }
        }
   


        public List<AllegroItemOrder> GetBuyersForPayments()
        {

            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroItemOrder
                    .Where(x => Helper.GetMyIds().Contains(x.AllegroItem.UserId)
                        && x.OrderDate != 0
                        && x.OrderDateTime >= DateTime.Now.AddDays(-30))
                    .OrderByDescending(x => x.OrderDateTime)
                    .ToList();
            }
        }
        /// <summary>
        /// Pobieranie zamówień, które są w statusie new oraz maja wyłączoną flagę DoNotAutoEdit
        /// </summary>



        ////public Order GetOrderForAutoUpdate(long buyerUserId, long itemId)
        ////{
        ////    using (LajtitDB ctx = new LajtitDB())
        ////    {
        ////        DataLoadOptions dlo = new DataLoadOptions();
        ////        dlo.LoadWith<OrderProduct>(x => x.Order);
        ////        dlo.LoadWith<Order>(x => x.Invoice);
        ////        ctx.LoadOptions = dlo;

        ////        int[] acceptableStatuses = new int[] { 1, 9, 10 };

        ////        return ctx.OrderProduct
        ////            .Where(x => acceptableStatuses.Contains(x.Order.OrderStatusId)
        ////                && x.Order.DoNotAutoEdit == false
        ////                && x.ExternalProductId == itemId
        ////                && x.Order.ExternalUserId == buyerUserId
        ////                && x.SourceTypeId == 1) // Allegro product
        ////            .Select(x => x.Order)
        ////                .FirstOrDefault();
        ////    }
        ////}


        /// <summary>
        /// Jeśli inpostBatchId jest podany to znaczy, że aktualizujemy lub dodajemy shipmenty do batcha
        /// </summary>
        /// <param name="ordersInpost"></param>
        /// <param name="oib"></param>
        /// <param name="inpostBatchId"></param>
        //public void SetOrderInpostBatch(List<OrderInpost> ordersInpost, OrderInpostBatch oib, int? inpostBatchId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        if(!inpostBatchId.HasValue)
        //        { 
        //        ctx.OrderInpost.InsertAllOnSubmit(ordersInpost);
        //        ctx.SubmitChanges();
        //        }
        //        else
        //        {
        //            List<Dal.OrderInpost> ordersInDb = ctx.OrderInpost.Where(x => x.InpostBatchId == inpostBatchId.Value).ToList();

        //            foreach (OrderInpost order in ordersInpost)
        //            {
        //                OrderInpost oiInDb = ordersInDb.Where(x => x.ShipmentId == order.ShipmentId).FirstOrDefault();

        //                if (oiInDb != null)
        //                {
        //                    oiInDb.TrackingNumber = order.TrackingNumber;

        //                }
        //                else
        //                {
        //                    order.InpostBatchId = inpostBatchId.Value;
        //                    ctx.OrderInpost.InsertOnSubmit(order);
        //                }
        //            }

        //            ctx.SubmitChanges();
        //            ordersInDb = ctx.OrderInpost.Where(x => x.InpostBatchId == inpostBatchId.Value).ToList();

        //            OrderInpostBatch batch = ctx.OrderInpostBatch.Where(x => x.InpostBatchId == inpostBatchId.Value).FirstOrDefault();

        //            string status = "generated";
        //            if (ordersInDb.Where(x => x.TrackingNumber == null || x.TrackingNumber == "").Count() > 0)
        //                status = "in_progress";

        //            batch.BatchStatus = status;
        //            ctx.SubmitChanges();

        //        }
        //    }
        //}

        public void UpdateOrderBasedOnBuyerForm(Order orderUpdating, AllegroItemTransaction transaction)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                using (LajtitAllegroDB ctxa = new LajtitAllegroDB())
                {

                    #region Order
                    Order existingOrder = ctx.Order.Where(x => x.OrderId == orderUpdating.OrderId &&
                    x.DoNotAutoEdit == false).FirstOrDefault();

                    if (existingOrder != null)
                    {
                        UpdateOrder(orderUpdating, ctx, existingOrder);
                        OrderStatusHistory osh = new OrderStatusHistory()
                        {
                            Comment = transaction.UserMessage,
                            InsertDate = transaction.InsertDate,
                            InsertUser = "",
                            Order = existingOrder,
                            OrderStatusId = 8 // adnotacja
                        };
                        ctx.OrderStatusHistory.InsertOnSubmit(osh);
                    }
                    #endregion

                    #region BuyerForm


                    //long[] itemIds = transaction.AllegroItemTransactionItems.Select(x => x.ItemId).ToArray();
                    long[] itemIds = ctxa.AllegroItemTransactionItem.Where(x => x.TransactionId == transaction.TransactionId)
                        .Select(x => x.ItemId)
                        .Distinct()
                        .ToArray();
                    // pobiez wszystkie formularze dostawy dla tego kupujacego, ktore sa jeszcze aktywne
                    int[] transactionIds = ctxa.AllegroItemTransactionItem.Where(x => x.AllegroItemTransaction.UserBuyerId == transaction.UserBuyerId
                        && itemIds.Contains(x.ItemId)
                        && x.AllegroItemTransaction.OrderCreated == false)
                        .Select(x => x.AllegroItemTransaction.TransactionId)
                        .Distinct()
                        .ToArray();

                    foreach (int transactionId in transactionIds)
                    {
                        AllegroItemTransaction existingTransaction = ctxa.AllegroItemTransaction
                            .Where(x => x.TransactionId == transactionId).FirstOrDefault();

                        if (existingTransaction != null)
                        {
                            existingTransaction.OrderId = existingOrder.OrderId;
                            existingTransaction.OrderCreated = true;
                    }
                    }
                    #endregion

                    ctx.SubmitChanges();
                    ctxa.SubmitChanges();

                }
            }
        }



        public List<OrderPaymentType> GetOrderPaymentTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderPaymentType.ToList();
            }
        }

        public void SetOrderPayment(OrderPayment payment, string actingUser)
        {

            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.OrderPayment.InsertOnSubmit(payment);
                ctx.SubmitChanges();
            }
        }

        public List<OrderLog> GetOrderLog(int orderId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrderLog.Where(x => x.OrderId == orderId).OrderBy(x => x.InsertDate).ToList();
            }
        }

        public List<OrderStatusHistory> GetOrderStatusHistory(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderStatusHistory>(x => x.OrderStatus);
                ctx.LoadOptions = dlo;
                return ctx.OrderStatusHistory.Where(x => x.OrderId == orderId).OrderBy(x => x.InsertDate).ToList();
            }
        }

        public OrderInpostView GetOrdersForInpost(int orderShippingId, int orderId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrderInpostView.Where(x => x.OrderShippingId == orderShippingId && x.OrderId == orderId).FirstOrDefault();
            }
        }
        //public OrderInpostView2 GetOrdersForInpost2(int orderId)
        //{
        //    using (LajtitViewsDB ctx = new LajtitViewsDB())
        //    {
        //        return ctx.OrderInpostView2.Where(x => x.OrderId == orderId).FirstOrDefault();
        //    }
        //}

        //public void SetOrderComment(OrderComment comment)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ctx.OrderComments.InsertOnSubmit(comment);
        //        ctx.SubmitChanges();
        //    }
        //}

        //public List<OrderCommentType> GetCommentTypes(bool showOnUI)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        var q = ctx.OrderCommentTypes.AsQueryable();
        //        if (showOnUI)
        //            q = q.Where(x => x.ShowOnUI == true);

        //        return q.ToList();
        //    }
        //}

        public List<OrderProduct> GetOrderProductsWithoutCatalogAssigment(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderProduct
                    .Where(x => x.OrderId == orderId
                        //&& x.SourceTypeId == 1
                        && x.ProductCatalogId == null
                        && x.Quantity > 0)
                    .ToList();
            }
        }

        public object GetOrdersForExport(int shippingCompanyId)
        {

            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ShippingExport.Where(x => x.ShippingCompanyId == shippingCompanyId).ToList();
            }
        }

        public OrderStatus GetOrderStatus(int orderStatusId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderStatus.Where(x => x.OrderStatusId == orderStatusId).FirstOrDefault();
            }
        }

        public bool GetProductCatalogCode(int? productCatalogId, string code, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (productCatalogId.HasValue)
                    return ctx.ProductCatalog.Where(x => x.ProductCatalogId != productCatalogId.Value && x.SupplierId == supplierId && x.Code == code)
                        .Count() == 0;
                else
                    return ctx.ProductCatalog.Where(x => x.Code == code && x.SupplierId == supplierId)
                        .Count() == 0;
            }


        }
        public bool GetProductCatalogExternalId(int? productCatalogId, string externalId, int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (String.IsNullOrEmpty(externalId))
                    return true;

                if (productCatalogId.HasValue)
                    return ctx.ProductCatalog.Where(x => x.ProductCatalogId != productCatalogId.Value && x.SupplierId == supplierId && x.ExternalId == externalId)
                        .Count() == 0;
                else
                    return ctx.ProductCatalog.Where(x => x.ExternalId == externalId && x.SupplierId == supplierId)
                        .Count() == 0;
            }


        }

        public bool GetProductCatalogEan(int? productCatalogId, string ean)
        {
            if (ean.Trim() == "")
                return true;

            using (LajtitDB ctx = new LajtitDB())
            {
                if (productCatalogId.HasValue)
                    return ctx.ProductCatalog.Where(x => x.ProductCatalogId != productCatalogId.Value && x.Ean == ean)
                        .Count() == 0;
                else
                    return ctx.ProductCatalog.Where(x => x.Ean == ean)
                        .Count() == 0;
            }


        }
     
        public List<ProductCatalogViewFnResult> GetProductCatalog(string name, int[] supplierIds, int? warehouseId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                bool negative = false;
                if (name.StartsWith("~"))
                    negative = true;

                string searchName = name.Replace("~", "");

                //var query = from pe in ctx.ProductCatalogViewFn(warehouseId)
                // where 
                // SqlMethods.Like(pe.Name, "%" + name + "%")
                // ||
                // SqlMethods.Like(pe.AllegroName, "%" + name + "%")
                // ||
                // SqlMethods.Like(pe.Code, "%" + name + "%")
                // ||
                // SqlMethods.Like(pe.Code2, "%" + name + "%")
                // ||
                // SqlMethods.Like(pe.Ean, "%" + name + "%")
                //            select pe;
                IQueryable<ProductCatalogViewFnResult> query;
                if (negative)
                    query = from pe in ctx.ProductCatalogViewFn(warehouseId)
                            where
                             !pe.Name.Contains(searchName)
                            &&
                             !pe.Code.Contains(searchName)
                            &&
                             !pe.CodeClear.Contains(searchName)
                            //&&
                            // (pe.Code==null || !pe.Code2.Contains(searchName))
                            //&&
                            // (pe.Ean == null || !pe.Ean.Contains(searchName))

                            select pe;
                else
                    query = from pe in ctx.ProductCatalogViewFn(warehouseId)
                            where

                             pe.Name.Contains(searchName)
                            ||
                             pe.Code.Contains(searchName)
                            ||
                             pe.CodeClear.Contains(searchName)
                            ||
                             pe.Code2.Contains(searchName)
                            ||
                             pe.Ean.Contains(searchName)

                            select pe;


                //var q = ctx.ProductCatalogView.AsQueryable();
                var q = query;

                //if (!String.IsNullOrEmpty(name))
                //{
                //    q = q.Where(x => x.Name.Contains(name) || x.AllegroName.Contains(name) || x.Code.StartsWith(name) || x.CodeSupplier.StartsWith(name));
                //}
                //if (!String.IsNullOrEmpty(desc))
                //q = q.Where(x => x.Specification.Contains(desc) || x.Description.Contains(desc));
                if (supplierIds.Length > 0)
                    q = q.Where(x => supplierIds.Contains(x.SupplierId));
                if (warehouseId.HasValue)
                    q = q.Where(x => x.LeftQuantity > 0);


                return q.OrderBy(x => x.Name).ToList();
            }
        }

        public IOrderedQueryable<ProductCatalogViewDbFnResult> GetProductCatalogForDb(LajtitDB ctx, int? shopId, string name,   int[] supplierIds, Guid? searchId)
        {
           
                bool negative = false;
                if (name.StartsWith("~"))
                    negative = true;

                 string searchName = name.Replace("~", "");

                IQueryable< ProductCatalogViewDbFnResult> query;
                if (negative)
                    query = from pe in ctx.ProductCatalogViewDbFn(shopId, searchId)
                            where
                             !pe.Name.Contains(searchName)
                            &&
                             !pe.Code.Contains(searchName)
                            &&
                             !pe.CodeClear.Contains(searchName)
                            //&&
                            // (pe.Code==null || !pe.Code2.Contains(searchName))
                            //&&
                            // (pe.Ean == null || !pe.Ean.Contains(searchName))

                            select pe;
                else
                    query = from pe in ctx.ProductCatalogViewDbFn(shopId, searchId)
                            where

                             SqlMethods.Like(pe.Name, "%" + searchName + "%")
                            ||
                            SqlMethods.Like(pe.Code, "%" + searchName + "%")
                            ||
                            SqlMethods.Like(pe.CodeClear, "%" + searchName + "%")
                            ||
                             SqlMethods.Like(pe.Code2, "%" + searchName + "%")
                            ||
                             SqlMethods.Like(pe.Ean, "%" + searchName + "%")
                            select pe;
            


                //var q = ctx.ProductCatalogView.AsQueryable();
                var q = query;
                 
                if (supplierIds.Length > 0)
                    q = q.Where(x => supplierIds.Contains(x.SupplierId)); 

                return q.OrderBy(x => x.Name);
             
        }

        public bool SetProductCatalogDelete(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                try
                {
                    ProductCatalog pc = ctx.ProductCatalog.Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();
                    ctx.ProductCatalog.DeleteOnSubmit(pc);

                    ctx.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public object GetOrderStatuses(bool changesStatus)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var q = ctx.OrderStatus.AsQueryable();
                if (changesStatus)
                    q = q.Where(x => x.ChangesStatus == true);
                q = q.Where(x => x.IsActive);
                return q.OrderBy(x=>x.SortOrderId).ToList();
            }
        }

        public List<ShippingCompany> GetShipppingCompanies()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShippingCompany.Where(x=>x.IsActive).ToList();
            }
        }

        //public List<Order> GetOrders(int[] orderIds)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<Order>(x => x.ShippingType);
        //        dlo.LoadWith<Order>(x => x.Company);
        //        ctx.LoadOptions = dlo;
        //        return ctx.Order.Where(x => orderIds.Contains(x.OrderId)).ToList();
        //    }
        //}
        //public int SetOrderExportBatch(Dal.Helper.ShippingCompany shippingCompany,
        //    int[] orderIds, 
        //    string comment,
        //    string actingUser,
        //    string fileName)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        List<Order> orders = ctx.Order.Where(x => orderIds.Contains(x.OrderId)).ToList(); 
        //        OrderExportBatch batch = new OrderExportBatch()
        //        {
        //            Comment = String.Format("Wyeksportowano: {0} zamówień", orderIds.Length),
        //            InsertDate = DateTime.Now,
        //            InsertUser = actingUser,
        //            ShippingCompanyId = (int)shippingCompany,
        //            FileName = fileName
        //        };
        //        List<OrderExportBatchOrder> batchOrders = new List<OrderExportBatchOrder>();

        //        foreach (Order order in orders)
        //        { 
        //            OrderExportBatchOrder oxbo = new OrderExportBatchOrder()
        //            {
        //                OrderExportBatch = batch,
        //                OrderId = order.OrderId
        //            };
        //            batchOrders.Add(oxbo);
        //        }
        //        ctx.OrderExportBatchOrder.InsertAllOnSubmit(batchOrders);
        //        ctx.SubmitChanges();
        //        return batch.OrderExportBatchId;
        //    }
        //}

        //public OrderInpostBatch GetOrderInpostBatch()
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.OrderInpostBatch.Where(x => x.BatchStatus == "in_progress").FirstOrDefault();
        //    }
        //}

        public void SetOrdersStatus(Dal.Helper.ShippingCompany shippingCompany, 
            int orderId, 
            Helper.OrderStatus orderStatus, 
            string comment, 
            string actingUser)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Order> orders = ctx.Order.Where(x => x.OrderId==orderId).ToList();
                OrderStatus status = ctx.OrderStatus.Where(x => x.OrderStatusId == (int)orderStatus).FirstOrDefault();
                //OrderExportBatch batch = new OrderExportBatch()
                //{
                //    Comment = String.Format("Wyeksportowano: {0} zamówień", orderIds.Length),
                //    InsertDate = DateTime.Now,
                //    InsertUser = actingUser,
                //    ShippingCompanyId = (int)shippingCompany,
                //    FileName = fileName
                //};
                //List<OrderExportBatchOrder> batchOrders = new List<OrderExportBatchOrder>();

                foreach (Order order in orders)
                {
                    if (status.ChangesStatus)
                        order.OrderStatusId = (int)orderStatus;

                    OrderStatusHistory osh = new OrderStatusHistory()
                    {
                        Comment = comment,
                        InsertDate = DateTime.Now,
                        InsertUser = actingUser,
                        OrderId = order.OrderId,
                        OrderStatusId = (int)orderStatus
                    };
                    ctx.OrderStatusHistory.InsertOnSubmit(osh);
                    //OrderExportBatchOrder oxbo = new OrderExportBatchOrder()
                    //{
                    //    OrderExportBatch = batch,
                    //    OrderId = order.OrderId
                    //};
                    //batchOrders.Add(oxbo);
                }
                //ctx.OrderExportBatchOrder.InsertAllOnSubmit(batchOrders);
                ctx.SubmitChanges(); 
            }
        }
        //public void SetOrdersStatus(int[] orderIds, Helper.OrderStatus orderStatus, string comment, string actingUser)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        List<Order> orders = ctx.Order.Where(x => orderIds.Contains(x.OrderId)).ToList();
        //        OrderStatus status = ctx.OrderStatus.Where(x => x.OrderStatusId == (int)orderStatus).FirstOrDefault();
        //        foreach (Order order in orders)
        //        {
        //            if (status.ChangesStatus)
        //                order.OrderStatusId = (int)orderStatus;

        //            OrderStatusHistory osh = new OrderStatusHistory()
        //            {
        //                Comment = comment,
        //                InsertDate = DateTime.Now,
        //                InsertUser = actingUser,
        //                OrderId = order.OrderId,
        //                OrderStatusId = (int)orderStatus
        //            };
        //            ctx.OrderStatusHistory.InsertOnSubmit(osh);
        //        }
        //        ctx.SubmitChanges();

        //    }
        //}
        public int SetOrderNew(Order o)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.Order.InsertOnSubmit(o);
                ctx.SubmitChanges();
                return o.OrderId;
            }
        }

        public List<OrderStatsFunResult> GetOrderStats(int? paymentAccountTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderStatsFun(paymentAccountTypeId)
                    .ToList()
                    .OrderByDescending(x => x.Year)
                    .ThenBy(x => x.DataType)
                    .ThenByDescending(x => x.Date)
                    .ToList();
            }
        }

        public List<OrderComplantType> GetOrderComplaintTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderComplantType.ToList();
            }
        }

        public List<OrderClientsForSearch> GetClients(string name)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrderClientsForSearch
                    .Where(x => x.Email.StartsWith(name)
                        || x.Client.Contains(name)
                        || (x.UserName != null && x.UserName.StartsWith(name)))
                        .OrderByDescending(x => x.CreateDate)
                        .ToList();


            }
        }



        //public List<AllegroPayment> GetPaymentsFromAllegro()
        //{
        //    using (LajtitAllegroDB ctx = new LajtitAllegroDB())
        //    {
        //        //DataLoadOptions dlo = new DataLoadOptions();
        //        //dlo.LoadWith<AllegroPaymentDetail>(x => x.AllegroPayment);
        //        //ctx.LoadOptions = dlo;
        //        return ctx.AllegroPayments.Where(x => x.OrderId == null && x.IsActive == true).ToList();
        //    }
        //}

        //public Order GetOrderForPayment(long buyerId, long[] itemIds)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        int[] acceptableStatuses = new int[] { 1, 9, 10, 11, 12 };
        //        return ctx.Order
        //            .Where(x => acceptableStatuses.Contains(x.OrderStatusId) // Nowy, Oczekujący na wpłatę/kontakt
        //                && x.SourceTypeId == 1 // Allegro
        //                && x.ExternalUserId == buyerId
        //                && x.OrderProduct.Any(y => itemIds.Contains(y.ExternalProductId))
        //                ).FirstOrDefault();

        //    }
        //}

        public void SetOrderPayments(List<OrderPayment> orderPayments, List<AllegroPayment> paymentsProcessed)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                using (LajtitAllegroDB ctxa = new LajtitAllegroDB())
                {
                    ctx.OrderPayment.InsertAllOnSubmit(orderPayments);

                    long[] paymentTransactiondIds = paymentsProcessed.Select(x => x.PaymentTransactiondId).ToArray();

                    List<AllegroPayment> allegroPaymentsToUpdate =
                        ctxa.AllegroPayment.Where(x => paymentTransactiondIds.Contains(x.PaymentTransactiondId)).ToList();
                    foreach (AllegroPayment allegroPaymentToUpdate in allegroPaymentsToUpdate)
                    {
                        AllegroPayment ap = paymentsProcessed.Where(x => x.PaymentTransactiondId == allegroPaymentToUpdate.PaymentTransactiondId)
                        .FirstOrDefault();
                        allegroPaymentToUpdate.OrderId = ap.OrderId;
                    }

                    ctx.SubmitChanges();
                    ctxa.SubmitChanges();
                }
            }
        }


        //public void SetOrdersStatusIfPaid(List<OrderPayment> orderPayments)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        int[] orderIds = orderPayments.Select(x => x.OrderId).Distinct().ToArray();
        //        List<Order> orders = ctx.Order.Where(x => orderIds.Contains(x.OrderId)
        //            && x.OrderStatusId == 1
        //            && x.ShippintTypeId != 0
        //            && x.AmountBalance == 0).ToList();
        //        List<OrderStatusHistory> statuses = new List<OrderStatusHistory>();

        //        foreach (Order order in orders)
        //        {
        //            OrderStatusHistory osh = new OrderStatusHistory()
        //            {
        //                Comment = "Automatyczna zmiana statusu po płatności z PayU",
        //                InsertDate = DateTime.Now,
        //                InsertUser = "System",
        //                Order = order,
        //                OrderStatusId = (int)Helper.OrderStatus.ReadyToSend

        //            };
        //            statuses.Add(osh);
        //            order.OrderStatusId = (int)Helper.OrderStatus.ReadyToSend;
        //        }

        //        ctx.OrderStatusHistory.InsertAllOnSubmit(statuses);
        //        ctx.SubmitChanges();
        //    }
        //}

        public List<OrderComplaint> GetComplaints(int[] complaintStatusIds, string orderId, string clientName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderComplaint>(x => x.Order);
                dlo.LoadWith<OrderComplaint>(x => x.ComplaintStatus);
                dlo.LoadWith<OrderComplaint>(x => x.OrderComplantType);
                ctx.LoadOptions = dlo;


                var r = ctx.OrderComplaint.AsQueryable();

                if (complaintStatusIds.Length > 0)
                    r = r.Where(x => complaintStatusIds.Contains(x.ComplaintStatusId.Value));

                if (!String.IsNullOrEmpty(orderId))
                    r = r.Where(x => x.OrderId == Int32.Parse(orderId));

                if (!String.IsNullOrEmpty(clientName))
                    r = r.Where(x => x.Order.Email.Contains(clientName) || x.Order.ShipmentCompanyName.Contains(clientName) || x.Order.ShipmentFirstName.Contains(clientName) || x.Order.ShipmentLastName.Contains(clientName));

                //if (!String.IsNullOrEmpty(orderId))
                //    r = r.Where(x => x.OrderId == Int32.Parse(orderId));




                return r.ToList();
            }
        }
 

        //public object GetProductCatalogSynonims(int productCatalogId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogynonims
        //            .Where(x => x.ProductCatalogId == productCatalogId).ToList();
        //    }

        //}

     

        //public void SetProductCatalogSynonim(ProductCatalogSynonim synonim)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {

        //        ctx.ProductCatalogynonims.InsertOnSubmit(synonim);
        //        ctx.SubmitChanges();
        //    }
        //}

        //public ProductCatalogSynonim GetProductCatalogSynonim(string name)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogynonims.Where(x => x.IsActive == true && x.Name == name).FirstOrDefault();
        //    }
        //}

        public List<ProductCatalogStatsResult> GetProductStats(DateTime startDate, DateTime endDate, int? supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogStats(startDate, endDate, supplierId).ToList();
            }
        }
        public List<ProductCatalogGroupStatsResult> GetProductGroupStats(DateTime startDate, DateTime endDate, int? supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroupStats(startDate, endDate, supplierId).ToList();
            }
        }

        public void SetOrdersFromMigration(List<OrderProduct> orderProducts, List<Dal.OrderPayment> payments, List<OrderStatusHistory> statuses)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                ctx.OrderProduct.InsertAllOnSubmit(orderProducts);
                ctx.OrderPayment.InsertAllOnSubmit(payments);
                ctx.OrderStatusHistory.InsertAllOnSubmit(statuses);
                ctx.SubmitChanges();
            }
        }

        public void SetOrderPaymentUpdate(OrderPayment op, string actingUser)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                OrderPayment orderPayment = ctx.OrderPayment.Where(x => x.PaymentId == op.PaymentId).FirstOrDefault();

                TableLog log = new TableLog()
                {
                    Comment = String.Format("Zmiana płatności: {0}, Amount z {1:C} na {2:C}, Comment z '{3}' na '{4}', AccountingTypeId z '{5}' na '{6}', Data z {7} na {8}",
                    op.PaymentId, orderPayment.Amount, op.Amount, orderPayment.Comment, op.Comment, orderPayment.AccountingTypeId, op.AccountingTypeId,
                    
                    op.InsertDate, orderPayment.InsertDate),
                    InsertDate = DateTime.Now,
                    InsertUser = actingUser,
                    ObjectId = orderPayment.PaymentId,
                    TableName = "OrderPayment"
                };
                ctx.TableLog.InsertOnSubmit(log);


                orderPayment.Comment = op.Comment;
                orderPayment.Amount = op.Amount;
                orderPayment.NotForEvidence = op.NotForEvidence;
                orderPayment.AccountingTypeId = op.AccountingTypeId;
                orderPayment.InsertDate = op.InsertDate;
                ctx.SubmitChanges();
            }
        }

        //public List<OrderProduct> GetOrderProductsReadyForSend(int shippingCompanyId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<OrderProduct>(x => x.ProductCatalog);
        //        ctx.LoadOptions = dlo;
        //        return ctx.OrderProduct
        //            .Where(x =>
        //                x.Order.ShippingType.ShippingCompanyId == shippingCompanyId &&
        //                (x.Order.OrderStatusId == (int)Helper.OrderStatus.ReadyToSend ||
        //        x.Order.OrderStatusId == (int)Helper.OrderStatus.Exported) && x.Quantity > 0)
        //            .ToList();
        //    }
        //}

        //public List<OrderProduct> GetOrderProductsReadyForSend(int[] exportBatchIds, 
        //    List<Dal.Helper.OrderStatus> statuses)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<OrderProduct>(x => x.ProductCatalog);
        //        dlo.LoadWith<OrderProduct>(x => x.Order);
        //        dlo.LoadWith<Order>(x => x.Invoice);
        //        dlo.LoadWith<Invoice>(x => x.Company);
        //        ctx.LoadOptions = dlo;

        //        int[] orderIds = GetOrderIDsBasedOnBatchIDs(exportBatchIds);

        //        return ctx.OrderProduct
        //            .Where(x =>
        //                orderIds.Contains(x.OrderId)
        //                && statuses.Select(y => (int)y).ToArray().Contains(x.Order.OrderStatusId)
        //                && x.Quantity > 0
        //                && x.ProductTypeId == (int)Dal.Helper.ProductType.RegularProduct)
        //            .ToList();
        //    }
        //}


        public List<OrdersReadyForSent> GetOrderProductsReadyForSend(IEnumerable<Dal.Helper.OrderStatus> statuses, 
            int[] shippingCompanyId, DateTime date, out DateTime? firstSent, out DateTime? lastSent)
        {
            using (LajtitViewsDB ctxv = new LajtitViewsDB())
            {
                using (LajtitDB ctx = new LajtitDB())
                { 

                    var q = ctxv.OrdersReadyForSent
                    .Where(x =>
                        statuses.Select(y => (int)y).ToArray().Contains(x.OrderStatusId)
                        && x.Quantity > 0
                        && x.ProductTypeId == (int)Dal.Helper.ProductType.RegularProduct);

                    if (shippingCompanyId.Length>0)
                        q = q.Where(x => shippingCompanyId.Contains(x.ShippingCompanyId));

                    firstSent = lastSent = null;

                    if (statuses.Contains(Dal.Helper.OrderStatus.Sent))
                    {
                        int[] ordersSentIds = ctx.OrderStatusHistory.Where(x => x.OrderStatusId == (int)Dal.Helper.OrderStatus.Sent
                        && x.InsertDate.Date == date.Date).Select(x => x.OrderId).Distinct().ToArray();
                        q = q.Where(x => ordersSentIds.Contains(x.OrderId));

                        if (q.Count() > 0)
                        {
                            firstSent = ctx.OrderStatusHistory.Where(x => x.OrderStatusId == (int)Dal.Helper.OrderStatus.Sent
                            && x.InsertDate.Date == date.Date).Min(x => x.InsertDate);
                            lastSent = ctx.OrderStatusHistory.Where(x => x.OrderStatusId == (int)Dal.Helper.OrderStatus.Sent
                            && x.InsertDate.Date == date.Date).Max(x => x.InsertDate);
                        }
                    }
                        return q.ToList();
                }
            }
        }
        //public int[] GetOrderIDsBasedOnBatchIDs(int[] exportBatchIds)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        int[] orderIds = ctx.OrderExportBatchOrder
        //            .Where(x => exportBatchIds.Contains(x.OrderExportBatchId))
        //            .Select(x => x.OrderId)
        //            .Distinct()
        //            .ToArray();
        //        return orderIds;
        //    }
        //}

        public List<OrderStatsByDay> GetOrderStatsByDay()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrderStatsByDay
                    .OrderByDescending(x => x.Date)
                    .Take(90)
                    .ToList();
            }
        }

        //public List<OrderExportBatch> GetOrderBatches()
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<OrderExportBatch>(x => x.ShippingCompany); 
        //        ctx.LoadOptions = dlo;
        //        return ctx.OrderExportBatch.ToList();
        //    }
        //}
         

        public void SetOrderDateAndSource(Order order, string actingUser)
        {


            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Order>(x => x.Shop);
                ctx.LoadOptions = dlo;

                Order exitstingOrder = ctx.Order.Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                DateTime existingDate = exitstingOrder.InsertDate;
                int existingShopId = exitstingOrder.Shop.ShopId;
                string existingShop = exitstingOrder.Shop.Name;

                exitstingOrder.InsertDate = order.InsertDate;
                exitstingOrder.Shop = ctx.Shop.Where(x => x.ShopId == order.ShopId).FirstOrDefault();
                ctx.SubmitChanges();

                if (order.ShopId != existingShopId)
                {
                    TableLog log = new TableLog()
                    {
                        Comment = String.Format("Zmiana źródła pochodzenia zamówienia z {0} na {1}", existingShop, exitstingOrder.Shop.Name),
                        InsertDate = DateTime.Now,
                        InsertUser = actingUser,
                        ObjectId = order.OrderId,
                        TableName = "Order"
                    };
                    ctx.TableLog.InsertOnSubmit(log);
                }
                if (order.InsertDate != exitstingOrder.InsertDate)
                {
                    TableLog log = new TableLog()
                    {
                        Comment = String.Format("Zmiana daty zamówienia z {0} na {1}", order.InsertDate, exitstingOrder.InsertDate),
                        InsertDate = DateTime.Now,
                        InsertUser = actingUser,
                        ObjectId = order.OrderId,
                        TableName = "Order"
                    };
                    ctx.TableLog.InsertOnSubmit(log);
                }
                ctx.SubmitChanges();
            }
        }

   

        //public List<Order> GerOrders(int[] orderIds)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {

        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<Order>(x => x.ShippingType);
        //        ctx.LoadOptions = dlo;


        //        return ctx.Order
        //            .Where(x => orderIds.Contains(x.OrderId))
        //            .ToList();
        //    }
        //}

        //public List<Order> GetOrdersForPocztaPolska(int orderBatchId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.OrderExportBatchOrder
        //            .Where(x => x.OrderExportBatchId == orderBatchId && x.Order.OrderStatusId == 3)
        //            .Select(x => x.Order)
        //            .ToList();
        //    }
        //}

        public Invoice GetLastInvoice(int companyId, int invoiceTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Invoice
                    .Where(x => x.IsDeleted == false
                        && x.InvoiceDate.Year == DateTime.Now.Year
                        && x.InvoiceSeqNo.HasValue
                        && x.CompanyId == companyId
                        && x.InvoiceTypeId == invoiceTypeId
                        )
                        .OrderByDescending(x => x.InvoiceSeqNo)
                        .FirstOrDefault();
            }
        }


        public int? GetLastParagon(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Order o = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();

                return ctx.Order
                    .Where(x =>
                      x.ParSeqNo.HasValue
                      && x.ParDate.Value.Year == DateTime.Now.Year
                      &&
                    x.CompanyId == o.CompanyId
                        )
                        .Select(x => x.ParSeqNo)
                        .OrderByDescending(x => x)
                        .FirstOrDefault();
            }
        }
        public void SetInvoiceNumber(Invoice invoice)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Invoice invToUpdate = ctx.Invoice.Where(x => x.InvoiceId == invoice.InvoiceId).FirstOrDefault();
                invToUpdate.InvoiceDate = invoice.InvoiceDate;
                invToUpdate.InvoiceNumber = invoice.InvoiceNumber;
                invToUpdate.InvoiceSeqNo = invoice.InvoiceSeqNo;
                //invToUpdate.InvoiceAmount = invoice.InvoiceAmount;
                invToUpdate.InvoiceSellDate = invoice.InvoiceSellDate;

                ctx.SubmitChanges();

            }
        }

     

        public void SetParagonNumber(int parSeqNo, string parNumber, int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Order order = ctx.Order.Where(x => x.OrderId == orderId && x.ParSeqNo == null).FirstOrDefault();

                if (order != null)
                {
                    order.ParSeqNo = parSeqNo;
                    order.ParNumber = parNumber;
                    order.ParDate = DateTime.Now;

                    TableLog log = new TableLog()
                    {
                        Comment = String.Format("Dodano paragon: {0} ", parNumber),
                        InsertDate = DateTime.Now,
                        InsertUser = "System",
                        ObjectId = order.OrderId,
                        TableName = "Order"
                    };
                    ctx.TableLog.InsertOnSubmit(log);
                    ctx.SubmitChanges();
                }
            }
        }
 
        //public void SetProductCatalogCosts(ProductCatalog pc)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ProductCatalog pcToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();
        //        pcToUpdate.CostAdditions = pc.CostAdditions;
        //        pcToUpdate.CostCreateAllegro = pc.CostCreateAllegro;
        //        pcToUpdate.CostCreateShop = pc.CostCreateShop;
        //        pcToUpdate.CostMaterial = pc.CostMaterial;
        //        pcToUpdate.CostSellAllegro = pc.CostSellAllegro;
        //        pcToUpdate.CostSellShop = pc.CostSellShop;
        //        pcToUpdate.CostWork = pc.CostWork;
                
        //        ctx.SubmitChanges();
        //    }
        //}

        public List<ProductCatalogGroup> GetProductCatalogGroups()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogGroup.ToList();
            }
        }

        public void SetProductCatalogClientData(ProductCatalog pc, int productCatalogGroupId, bool isAdmin)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalog productCatalogToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();

                productCatalogToUpdate.Name = pc.Name;
                //productCatalogToUpdate.IsActive = pc.IsActive; 
                productCatalogToUpdate.Code = pc.Code;
                productCatalogToUpdate.Code2 = pc.Code2;
                if (pc.Ean == "")
                    productCatalogToUpdate.Ean = null;
                else
                    productCatalogToUpdate.Ean = pc.Ean;

                productCatalogToUpdate.ExternalId = pc.ExternalId;
                productCatalogToUpdate.SupplierId = pc.SupplierId; 

               
                productCatalogToUpdate.UpdateReason = pc.UpdateReason;
                productCatalogToUpdate.UpdateUser = pc.UpdateUser;
                productCatalogToUpdate.DeliveryId = pc.DeliveryId;
                productCatalogToUpdate.IsPaczkomatAvailable = pc.IsPaczkomatAvailable;
                productCatalogToUpdate.IsOutlet = pc.IsOutlet;
                productCatalogToUpdate.IsDiscontinued = pc.IsDiscontinued;
                productCatalogToUpdate.IsAvailable = pc.IsAvailable;
                productCatalogToUpdate.IsHidden = pc.IsHidden;

                if (isAdmin)
                {
                    productCatalogToUpdate.PriceBruttoFixed = pc.PriceBruttoFixed;
                    productCatalogToUpdate.PurchasePrice = pc.PurchasePrice;
                    productCatalogToUpdate.PriceBruttoPromo = pc.PriceBruttoPromo;
                    productCatalogToUpdate.PriceBruttoPromoDate = pc.PriceBruttoPromoDate;
                    productCatalogToUpdate.ProductTypeId = pc.ProductTypeId;
                    productCatalogToUpdate.LockRebates = pc.LockRebates;
                }

                Dal.ProductCatalogGroupProduct pcgToUpdate = ctx.ProductCatalogGroupProduct
                    .Where(x => 
                        x.ProductCatalogId == pc.ProductCatalogId 
                        && x.ProductCatalogGroup.ProductCatalogGroupFamily.FamilyTypeId == (int)Dal.Helper.ProductCatalogGroupFamilyType.Family)
                    .FirstOrDefault();
                Dal.ProductCatalogGroupProduct pcpg = new ProductCatalogGroupProduct()
                {
                    InsertDate = DateTime.Now,
                    InsertUser = "",
                    ProductCatalogId = pc.ProductCatalogId,
                    ProductCatalogGroupId = productCatalogGroupId
                };

                if (pcgToUpdate ==null ||(pcgToUpdate != null && pcgToUpdate.ProductCatalogGroupId != productCatalogGroupId))
                {
                    if(pcgToUpdate!=null)
                    ctx.ProductCatalogGroupProduct.DeleteOnSubmit(pcgToUpdate);
               
                    ctx.ProductCatalogGroupProduct.InsertOnSubmit(pcpg);
                }
                ctx.SubmitChanges();
            }
        }

        public void SetProductCatalogAllegroData(ProductCatalog pc )
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalog productCatalogToUpdate = ctx.ProductCatalog.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();
   
                productCatalogToUpdate.AutoAssignProduct = pc.AutoAssignProduct; 

              


                ctx.SubmitChanges();
            }
        }
        public List<OrderProductsSuggestionsResult> GerOrderProductSuggestions(Dal.Helper.Shop shop, int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                return ctx.OrderProductsSuggestions((int)shop, orderId).ToList();
            }
        }

        public int SetProductCatalogImage(ProductCatalogImage image)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogImage pci = ctx.ProductCatalogImage.Where(x => x.ProductCatalogId == image.ProductCatalogId)
                    .OrderByDescending(x => x.Priority).FirstOrDefault();

                if (pci != null)
                    image.Priority = pci.Priority + 1;

                ctx.ProductCatalogImage.InsertOnSubmit(image);
                ctx.SubmitChanges();
                return image.ImageId;
            }
        }

        public List<ProductCatalogImage> GetProductCatalogImages(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogImage>(x => x.ProductCatalog);
                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogImage.Where(x => x.ProductCatalogId == productCatalogId).OrderBy(x => x.Priority).ToList();
            }
        }


        public List<ProductCatalogImage> GetProductCatalogImages()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogImage.OrderBy(x => x.Priority).ThenBy(x => x.InsertDate).ToList();

            }
        }

        public void SetProductCatalogAllegroItem(ProductCatalogAllegroItem item)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogAllegroItem.InsertOnSubmit(item);
                ctx.SubmitChanges();
            }
        }
        public void SetProductCatalogAllegroItemUpdate(long itemId, string updateCommand, bool? isImageReady, Guid? processId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var p = ctx.ProductCatalogAllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();
                p.UpdateCommand = updateCommand;
                //if (isImageReady.HasValue) // null oznacza że nie decydujemy tutaj o zmianie zdjęć
                //{
                //    p.IsImageReady = isImageReady.Value;
                //    p.Comment = String.Format("SetProductCatalogAllegroItemUpdate IsImageReady {0}", p.IsImageReady);
                //}
                p.IsValid = false;
                p.Comment = "";
                p.ProcessId = processId;
                ctx.SubmitChanges();
            }
        }
     

        public void SetProductCatalogAllegroItemRemoveRecommend(int ProductCatalogId, int[] productCatalogIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductCatalogAllegroRecommend> products = ctx.ProductCatalogAllegroRecommend.Where(x => x.ProductCatalogId == ProductCatalogId
                    && productCatalogIds.Contains(x.ProductCatalogRecommendId)).ToList();

                ctx.ProductCatalogAllegroRecommend.DeleteAllOnSubmit(products);
                ctx.SubmitChanges();

            }
        }

        public void SetProductCatalogAllegroItemRecommend(int ProductCatalogId, int[] productCatalogIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                 

                List<ProductCatalogAllegroRecommend> productToRecommends = new List<ProductCatalogAllegroRecommend>();

                foreach (int productId in productCatalogIds)
                {
                    productToRecommends.Add(new ProductCatalogAllegroRecommend()
                        {
                            ProductCatalogId = ProductCatalogId,
                            ProductCatalogRecommendId = productId
                        }); 
               

                }

                foreach (ProductCatalogAllegroRecommend productRec in productToRecommends)
                {
                    ProductCatalogAllegroRecommend existing =
                        ctx.ProductCatalogAllegroRecommend.Where(x => x.ProductCatalogId == productRec.ProductCatalogId
                            && x.ProductCatalogRecommendId == productRec.ProductCatalogRecommendId).FirstOrDefault();

                    if (existing == null)
                        ctx.ProductCatalogAllegroRecommend.InsertOnSubmit(productRec);

                }
                ctx.SubmitChanges();
            }

            #region stara procedura wiazaca wzajemnie

            //using (LajtitDB ctx = new LajtitDB())
            //{


            //    // wybierz wszystkie produkty powiazae z danym produktem
            //    List<int> productsIds = ctx.ProductCatalogAllegroRecommend.Where(x => x.ProductCatalogId == ProductCatalogId)
            //        .Select(x => x.ProductCatalogRecommendId).ToList();

            //    //dorzuc produkt
            //    productsIds.Add(ProductCatalogId);
            //    //dorzuc nowe produkty ktore dowiazemy do danego produktu
            //    productsIds.AddRange(productCatalogIds);

            //    // wez unikalne wartosci
            //    productsIds = productsIds.Select(x => x).Distinct().ToList();

            //    List<ProductCatalogAllegroRecommend> productToRecommends = new List<ProductCatalogAllegroRecommend>();

            //    foreach (int productId in productsIds)
            //    {
            //        List<int> recommended = productsIds.Where(x => x != productId).ToList();

            //        foreach (int rec in recommended)
            //        {
            //            productToRecommends.Add(new ProductCatalogAllegroRecommend()
            //            {
            //                ProductCatalogId = productId,
            //                ProductCatalogRecommendId = rec
            //            });
            //        }


            //    }

            //    foreach (ProductCatalogAllegroRecommend productRec in productToRecommends)
            //    {
            //        ProductCatalogAllegroRecommend existing =
            //            ctx.ProductCatalogAllegroRecommend.Where(x => x.ProductCatalogId == productRec.ProductCatalogId
            //                && x.ProductCatalogRecommendId == productRec.ProductCatalogRecommendId).FirstOrDefault();

            //        if (existing == null)
            //            ctx.ProductCatalogAllegroRecommend.InsertOnSubmit(productRec);

            //    }
            //    ctx.SubmitChanges();
            //}

            #endregion
        }

        public ProductCatalogAllegroItem GetProductCatalogAllegroItem(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAllegroItem>(x => x.ProductCatalog);
                dlo.LoadWith<ProductCatalog>(x => x.Supplier);
                ctx.LoadOptions = dlo;

                return ctx.ProductCatalogAllegroItem
                        .Where(x => x.Id == id)
                        .FirstOrDefault();

            }
        }



        public List<ProductCatalogAllegroHistoryView> GetProductCatalogAllegroItemHistory(int productCatalogId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {

                return ctx.ProductCatalogAllegroHistoryView.Where(x => x.ProductCatalogId == productCatalogId)
                    .OrderBy(x => x.StatusOrder)
                    .ToList();
            }
        }
        public List<ProductCatalogAllegroHistoryView> GetProductCatalogAllegroItemHistory(int productCatalogId, long allegroUserId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {

                return ctx.ProductCatalogAllegroHistoryView.Where(x => x.ProductCatalogId == productCatalogId && x.UserId== allegroUserId)
                    .OrderBy(x => x.StatusOrder)
                    .ToList();
            }
        }

        public void SetProductCatalogAllegroItem(List<ProductCatalogAllegroItem> items)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogAllegroItem.InsertAllOnSubmit(items);
                ctx.SubmitChanges();
            }
        }

        //public void SetProductCatalogAllegroItemStatus(int id, Helper.ProductAllegroItemStatus productAllegroItemStatus)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ctx.CommandTimeout = 500;

        //        ProductCatalogAllegroItem batch = ctx.ProductCatalogAllegroItem.Where(x => x.Id == id).FirstOrDefault();
        //        batch.AllegroItemStatusId = (int)productAllegroItemStatus;
        //        batch.LastUpdateDateTime = DateTime.Now;

        //        ctx.SubmitChanges();
        //    }
        //}
 

 

        //public void SetProductCatalogAllegroItemUpdate(ProductCatalogAllegroItem item)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ctx.CommandTimeout = 300;
        //        ProductCatalogAllegroItem itemToUpdate = ctx.ProductCatalogAllegroItem.Where(x => x.Id == item.Id).FirstOrDefault();
        //        //itemToUpdate.AllegroItemStatusId = item.AllegroItemStatusId;
        //        itemToUpdate.LastUpdateDateTime = item.LastUpdateDateTime;
        //        itemToUpdate.Comment = item.Comment;
        //        itemToUpdate.Cost = item.Cost;
        //        itemToUpdate.ItemId = item.ItemId;
        //        //itemToUpdate.AllegroItemCreateDate = item.AllegroItemCreateDate;
        //        itemToUpdate.CommentSimple = item.CommentSimple;

        //        itemToUpdate.IsFixed = null;

        //        ctx.SubmitChanges();
        //    }
        //}
 

        public void SetOrderProductsVAT(int orderId, int orderPaymentTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                OrderPaymentType opt = ctx.OrderPaymentType.Where(x => x.PaymentTypeId == orderPaymentTypeId).FirstOrDefault();

                List<OrderProduct> products = ctx.OrderProduct.Where(x => x.OrderId == orderId).ToList();


                foreach (OrderProduct product in products)
                {

                    ctx.TableLog.InsertOnSubmit(
                        new TableLog()
                            {
                                Comment = String.Format("Zmiana VAT z {0} na {1} podczas dodawania płatności", product.VAT, opt.VAT),
                                InsertDate = DateTime.Now,

                                InsertUser = "",
                                ObjectId = orderId,
                                TableName = "Order"
                            }

                        );
                    product.VAT = opt.VAT;
                }

                ctx.SubmitChanges();

            }
        }

        public List<OrdersNotPaidButSentView> GetOrdersNotPaidButSent(int shippingCompanyId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrdersNotPaidButSentView
                    .Where(x=>x.ShippingCompanyId == shippingCompanyId)
                    .OrderBy(x => x.LastStatusChangeDate).ToList();

            }
        }

        public List<OrderNotificationType> GetOrderNotificationTypes(int orderStatusId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderNotificationType.Where(x => x.OrderStatusId == orderStatusId
                    && x.IsActive).ToList();
            }
        }

        public void SetOrderNotifications(int orderId, List<OrderNotification> notifications)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.OrderNotification> toDelete = new List<OrderNotification>();
                toDelete = ctx.OrderNotification.Where(x => x.OrderId == orderId && x.IsSent == false).ToList();
                ctx.OrderNotification.DeleteAllOnSubmit(toDelete);
                ctx.OrderNotification.InsertAllOnSubmit(notifications);
                ctx.SubmitChanges();


            }
        }

        public List<OrderNotificationsView> GetOrderNotifications(int orderId)
        {
            using (LajtitViewsDB ctxv = new LajtitViewsDB())
            {
                using (LajtitDB ctx = new LajtitDB())
                {
                    var view = ctxv.OrderNotificationsView.Where(x => x.OrderId == orderId && x.IsSent == false).OrderBy(x => x.NotificationTypeId).ToList();
                    int[] id = view.Select(x => x.Id).ToArray();
                    List<OrderNotification> not = ctx.OrderNotification.Where(x => id.Contains(x.Id)).ToList();

                    foreach (OrderNotification n in not)
                        n.IsSent = true;

                    ctx.SubmitChanges();

                    return view;
                }   }
        }


        public List<Order> GetOrdersForNotifications()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderNotification>(x => x.Order);
                dlo.LoadWith<Order>(x => x.OrderStatus);
                ctx.LoadOptions = dlo;
                return ctx.OrderNotification.Where(x =>
                    x.IsSent == false
                    && (x.Order.OrderStatusId == (int)Helper.OrderStatus.WaitingForClient
                        || x.Order.OrderStatusId == (int)Helper.OrderStatus.WaitingForPayment)
                    && x.InsertDate.AddDays(2) <= DateTime.Now)
                    .Select(x => x.Order).Distinct().ToList();
            }
        }

        public void SetOrderNotificationsAsSent(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.OrderNotification> toUpdate = new List<OrderNotification>();
                toUpdate = ctx.OrderNotification.Where(x => x.OrderId == orderId && x.IsSent == false).ToList();

                foreach (OrderNotification n in toUpdate)
                    n.IsSent = true;

                ctx.SubmitChanges();


            }
        }
        public void SetOrderNotificationsClone(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<Dal.OrderNotification> toUpdate = new List<OrderNotification>();
                toUpdate = ctx.OrderNotification.Where(x => x.OrderId == orderId && x.IsSent == false).ToList();

                foreach (OrderNotification n in toUpdate)
                    n.IsSent = true;


                ctx.OrderNotification.InsertAllOnSubmit(
                    toUpdate.Select(x =>
                    new OrderNotification()
                    {
                        Comment = x.Comment,
                        InsertDate = DateTime.Now,
                        IsSent = false,
                        NotificationTypeId = x.NotificationTypeId,
                        OrderId = x.OrderId
                    }
                    )

                    );
                ctx.SubmitChanges();


            }
        }

        public List<Order> GetOrdersWithoutTrackingNumbers()
        {
            throw new NotImplementedException();
            //using (LajtitDB ctx = new LajtitDB())
            //{
            //    int[] excludedTrackingNumbers = new int[]
            //    {
            //        //(int)Dal.Helper.OrderStatus.Cancelled,
            //        (int)Dal.Helper.OrderStatus.New,
            //        (int)Dal.Helper.OrderStatus.Temporary,
            //        (int)Dal.Helper.OrderStatus.Deleted,
            //        (int)Dal.Helper.OrderStatus.Complaint

            //    };
            //    DataLoadOptions dlo = new DataLoadOptions();
            //    dlo.LoadWith<Order>(x => x.ShippingType);
            //    dlo.LoadWith<ShippingType>(x => x.ShippingCompany);
            //    dlo.LoadWith<Order>(x => x.Company);
            //    ctx.LoadOptions = dlo;

            //    return ctx.Order.Where(
            //        x =>
            //            (x.ShippingType.ShippingCompany.HasIntegration)
            //            && x.ShipmentTrackingNumber == null
            //            && !excludedTrackingNumbers.Contains(x.OrderStatusId)
            //           && x.Company.DpdNumcat.HasValue
            //           && (x.ShippingType.PayOnDelivery == true || (x.ShippingType.PayOnDelivery == false && x.AmountBalance >= 0))
            //           && x.OrderId > 80000

            //        )
            //        .ToList();
            //}
        }
        public List<OrderShipping> GetOrdersWithoutTrackingNumbers2()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] excludedTrackingNumbers = new int[]
                {
                    //(int)Dal.Helper.OrderStatus.Cancelled,
                    (int)Dal.Helper.OrderStatus.New,
                    (int)Dal.Helper.OrderStatus.Temporary,
                    (int)Dal.Helper.OrderStatus.Deleted,
                    (int)Dal.Helper.OrderStatus.Complaint

                };
                DataLoadOptions dlo = new DataLoadOptions();
                //dlo.LoadWith<OrderShipping>(x => x.ShippingType);
                dlo.LoadWith<OrderShipping>(x => x.ShippingCompany);
                dlo.LoadWith<OrderShipping>(x => x.Order1);
                dlo.LoadWith<Order>(x => x.Company);
                ctx.LoadOptions = dlo;

                var orders = ctx.OrderShipping.Where(
                    x =>
                    x.OrderShippingStatusId == (int)Dal.Helper.OrderShippingStatus.ReadyToCreate
                        && x.ShippingCompany.HasIntegration
                        && x.ShipmentTrackingNumber == null
                        &&  x.ShippingServiceTypeId != (int)Dal.Helper.ShippingServiceType.ForOrder                       
                    )
                    .ToList();

                orders.AddRange(
                    ctx.Order.Where(
                    x =>
                            x.OrderShipping.OrderShippingStatusId == (int)Dal.Helper.OrderShippingStatus.ReadyToCreate
                        && x.OrderShipping.ShippingCompany.HasIntegration
                        && x.OrderShipping.ShipmentTrackingNumber == null
                    )
                    .Select(x => x.OrderShipping)
                    .ToList()
                    );

                return orders;
            }
        }

        //public OrderExportBatch GetOrderBatchForTrackingNumberImport()
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.OrderExportBatche.Where(x => x.TrackingNumbersImported == null
        //            && x.ShippingCompanyId == (int)Helper.ShippingCompany.Siodemka)
        //            .OrderBy(x => x.OrderExportBatchId)
        //            .FirstOrDefault();
        //    }
        //}

        //public void SetOrderTrackingNumber(int orderId, string trackingNumber, string inpostShipmentId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        Order order = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();

        //        if (String.IsNullOrEmpty(trackingNumber))
        //        { 
        //            trackingNumber = null;
        //            order.InpostShipmentId = null;
        //        }
        //        order.ShipmentTrackingNumber = trackingNumber;
        //        if (inpostShipmentId != null)
        //            order.InpostShipmentId = inpostShipmentId;
        //        order.TrackingNumberSent = null;

        //        OrderStatusHistory ash = new OrderStatusHistory()
        //        {
        //            Comment = String.Format("Pobrano numer przesyłki: {0}", trackingNumber),
        //            InsertDate = DateTime.Now,
        //            InsertUser = "system",
        //            OrderId = orderId,
        //            OrderStatusId = (int)Helper.OrderStatus.Comment
        //        };
        //        ctx.OrderStatusHistory.InsertOnSubmit(ash);
        //        ctx.SubmitChanges();
        //    }
        //}
        //public void SetOrderTrackingNumber(int orderId, string trackingNumber)
        //{
        //    SetOrderTrackingNumber(orderId, trackingNumber, null);
        //}

        //public void SetOrderExportBatchImportTrackingNumbersDone(int orderExportBatchId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {

        //        OrderExportBatch oeb = ctx.OrderExportBatche.Where(x => x.OrderExportBatchId == orderExportBatchId).FirstOrDefault();
        //        oeb.TrackingNumbersImported = true;

        //        ctx.SubmitChanges();
        //    }
        //}

        public void SetOrderPayments(List<OrderPayment> payments)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                ctx.OrderPayment.InsertAllOnSubmit(payments);

                ctx.SubmitChanges();
            }
        }

        public Supplier GetSupplier(int idSupplier)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Supplier>(x => x.SupplierOwner); 
                ctx.LoadOptions = dlo;
                return ctx.Supplier.Where(x => x.SupplierId == idSupplier).FirstOrDefault();
            }
        }

        public int SetProductCatalogNew(ProductCatalog pc, string userName, int productCatalogGroupId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalogGroupProduct pcgp = new ProductCatalogGroupProduct()
                {
                    InsertUser = userName,
                    InsertDate = DateTime.Now,
                    ProductCatalogGroupId = productCatalogGroupId,
                    ProductCatalog = pc 

                };
                ctx.ProductCatalog.InsertOnSubmit(pc);
                ctx.ProductCatalogGroupProduct.InsertOnSubmit(pcgp);
                ctx.SubmitChanges();

                return pc.ProductCatalogId;
            }
        }



        //public List<ProductCatalogView> GetProductCatalogView(int supplierId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogView.Where(x => x.SupplierId == supplierId).OrderBy(x => x.Name).ToList();
        //    }
        //}

        public List<OrdersSourceTypeStatsResult> GetOrdersBySourceStats(DateTime dateFrom, DateTime dateTo,
                bool completed,
                bool groupSources)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrdersSourceTypeStats(dateFrom, dateTo.AddHours(23).AddMinutes(59).AddSeconds(59), completed, groupSources)
                   // .Where(x=>x.in
                    .ToList();
            }
        }
        public List<OrderProfitResult> GetOrdersProfits(DateTime dateFrom, DateTime dateTo, int groupingType)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderProfit(dateFrom, dateTo.AddHours(23).AddMinutes(59).AddSeconds(59), groupingType)
                    // .Where(x=>x.in
                    .ToList();
            }
        }


        //public void SetProductCatalogAllegroItemsBatchFields(int batchId, List<ProductCatalogAllegroItemBatchField> fields)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {

        //        List<ProductCatalogAllegroItemBatchField> fieldsToDelete = ctx.ProductCatalogAllegroItemBatchFields
        //                .Where(x => x.BatchId == batchId).ToList();
        //        ctx.ProductCatalogAllegroItemBatchFields.DeleteAllOnSubmit(fieldsToDelete);
        //        ctx.SubmitChanges();

        //        foreach (ProductCatalogAllegroItemBatchField field in fields)
        //        {
        //            ProductCatalogAllegroItemBatchField fieldToUpdate = ctx.ProductCatalogAllegroItemBatchFields.Where(x => x.BatchId == field.BatchId
        //                && x.FieldId == field.FieldId).FirstOrDefault();

        //            if (fieldToUpdate == null)
        //                ctx.ProductCatalogAllegroItemBatchFields.InsertAllOnSubmit(fields);
        //            else
        //            {
        //                fieldToUpdate.Description = field.Description;
        //                fieldToUpdate.FieldType = field.FieldType;
        //                fieldToUpdate.FloatValue = field.FloatValue;
        //                fieldToUpdate.IntValue = field.IntValue;
        //                fieldToUpdate.StringValue = field.StringValue;
        //            }

        //            ctx.SubmitChanges();
        //        }


        //    }
        //}

        //public List<ProductCatalogAllegroFieldView> GetProductCatalogAllegroItemBatchFieldsByProductCatalogId(int[] productCatalogIDs)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //        return ctx.ProductCatalogAllegroFieldView
        //            .Where(x => productCatalogIDs.Contains(x.ProductCatalogId))
        //            .ToList();

        //}

        public bool GetProductCatalogAllegroItemsToReCreate(int batchId)
        {
            throw new NotImplementedException();
        }

        //public ShippingType GetShippingTypeFromOrder(int OrderId)
        //{
        //    throw new NotImplementedException();
        //    //using (LajtitDB ctx = new LajtitDB())
        //    //{
        //    //    return ctx.Order.Where(x=>x.OrderId==OrderId).Select(x => x.ShippingType).FirstOrDefault();
        //    //}
        //}

        public ProductCatalogAllegroItem GetProductCatalogIdFromBatch(long itemId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogAllegroItem>(x => x.ProductCatalog); 
                ctx.LoadOptions = dlo;
                ProductCatalogAllegroItem pcai =  ctx.ProductCatalogAllegroItem.Where(x => x.ItemId == itemId).FirstOrDefault();

                return pcai; 
            }
        }

   

        public void SetParagonStatus(int orderId, bool paragonActive, string actingUser)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Order order = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
                order.ParActive = paragonActive;
                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogImport> GetImports()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogImport.ToList();
            }
        }

        public int[] GetProductCatalogFromImport(int importId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogDelivery.Where(x => x.ImportId == importId)
                    .Select(x => x.ProductCatalogId).ToArray();
            }
        }

        public List<ProductCatalogAllegroItemsActive> GetProductCatalogAllegroActiveItems()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogAllegroItemsActive.ToList();
            }
        }

        //public List<ProductCatalogForAllegroGetResult> GetProductCatalogForAllegro(string name, int supplierId, bool? isReady, bool? notCreated, int importId)
        //{

        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogForAllegroGet(name, supplierId, isReady, notCreated, importId).ToList();
        //    }
        //}

        public void SetInvoiceLock(int invoiceId, string fileName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Invoice invoice = ctx.Invoice.Where(x => x.InvoiceId == invoiceId).FirstOrDefault();
                invoice.IsLocked = true;
                invoice.InvoiceFileName = fileName;


                if (invoice.Nip != "")
                    invoice.AccountingTypeId = (int)Dal.Helper.OrderPaymentAccoutingType.Invoice;

                ctx.SubmitChanges();
            }
        }

        public void SetInvoiceProducts(int invoiceId, List<InvoiceProduct> invoiceProducts)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<InvoiceProduct> invoiceProductsToDelete = ctx.InvoiceProduct.Where(x => x.InvoiceId == invoiceId).ToList();

                ctx.InvoiceProduct.DeleteAllOnSubmit(invoiceProductsToDelete);


                ctx.InvoiceProduct.InsertAllOnSubmit(invoiceProducts);
                ctx.SubmitChanges();
            }
        }

        public List<InvoiceProduct> GetInvoiceProducts(int invoiceId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.InvoiceProduct.Where(x => x.InvoiceId == invoiceId && x.Quantity > 0 && x.PriceBrutto > 0).ToList();
            }
        }
        public List<InvoiceProduct> GetInvoiceCorrectionProducts(int invoiceId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.InvoiceProduct.Where(x => x.InvoiceId == invoiceId ).ToList();
            }
        }

        public string[] GetInvoiceFiles(int[] orderIds)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Order.Where(x => orderIds.Contains(x.OrderId) && x.Invoice.InvoiceFileName != null)
                    .OrderBy(x=>x.Invoice.InvoiceDate)
                    .Select(x => x.Invoice.InvoiceFileName).ToArray();
            }
        }

        //public void SetProductCatalogAllegroItemsConfiguration(int batchId, List<ProductCatalogAllegroItem> items)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        List<Dal.ProductCatalogAllegroItem> itemsToUpdate = ctx.ProductCatalogAllegroItem
        //            .Where(x => items.Select(y => y.Id).ToArray().Contains(x.Id))
        //            .ToList();

        //        foreach (ProductCatalogAllegroItem itemToUpdate in itemsToUpdate)
        //        {
        //            ProductCatalogAllegroItem item = items.Where(x=>x.Id == itemToUpdate.Id).FirstOrDefault();

        //            itemToUpdate.AllegroName = item.AllegroName;
        //            itemToUpdate.EnablePromotions = item.EnablePromotions;
        //            itemToUpdate.PriceValue = item.PriceValue;
        //            itemToUpdate.PriceAddType = item.PriceAddType;
        //            itemToUpdate.PriceAddValueType = item.PriceAddValueType;
        //            itemToUpdate.AllegroUserAccountId = item.AllegroUserAccountId;
        //        }

        //        ctx.SubmitChanges();
        //    }
        //}

        public List<ProductCatalogLink> GetProductCatalogLinks(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] productCatalogIds = ctx.OrderProduct.Where(x => x.OrderId == orderId && x.Quantity > 0 && x.ProductCatalogId.HasValue).
                    Select(x => x.ProductCatalogId.Value).Distinct().ToArray();


                return ctx.ProductCatalogLink.Where(x => productCatalogIds.Contains(x.ProductCatalogId))
                    .Distinct().ToList();

            }
        }


        public List<Company> GetCompanies()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Company.OrderByDescending(x => x.IsMyCompany).ThenBy(x => x.Name).ToList();

            }
        }
        public List<Company> GetCompanies(string searchString)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                var c = ctx.Company.AsQueryable();

                if (!String.IsNullOrEmpty(searchString))
                    c = c.Where(x => x.Name.Contains(searchString) || x.Address.Contains(searchString));


                return c.OrderByDescending(x => x.IsMyCompany).ThenBy(x => x.Name).ToList();

            }
        }




        public int[] GetProductCatalogIdsNotOnAllegro()
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                int[] productIds = GetProductCatalogAllegroActiveItems().Select(x => x.ProductCatalogId).Distinct().ToArray();

                return productIds;
            }

        }
        public int[] GetProductCatalogWithGroups()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {

                int[] productIds = ctx. ProductCatalogFamilyGroupView.Select(x => x.ProductCatalogId).Distinct().ToArray();

                return productIds;
            }

        }
 
        public List<OrderProduct> GetSubOrderProducts(int orderProductId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                

                return ctx.OrderProduct.Where(x => x.SubOrderProductId == orderProductId
                    && x.ProductTypeId == (int)Dal.Helper.ProductType.ComponentProduct).ToList();
              
            }
        }

        public List<OrderProductSubProductsNotAssignedResult> GetOrderProductsSubProductsNotAssigned(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {


                return ctx.OrderProductSubProductsNotAssigned(orderId).ToList();

            }
        }

        //public List<Order> GetInvoices(int[] orderBatchIds)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        DataLoadOptions dlo = new DataLoadOptions();
        //        dlo.LoadWith<Order>(x => x.Invoice);
        //        dlo.LoadWith<Invoice>(x => x.Company);
        //        ctx.LoadOptions = dlo;
        //        int[] orderIds = ctx.OrderExportBatchOrder.Where(x => orderBatchIds.Contains(x.OrderExportBatchId))
        //            .Select(x => x.OrderId).ToArray();
        //        return ctx.Order.Where(x => orderIds.Contains(x.OrderId) && x.InvoiceId != null).ToList();

        //    }
        //}

        public ProductCatalog GetProductCatalogByExternalProductId(long externalProductId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogAllegroItem.Where(x => x.ItemId == externalProductId)
                    .Select(x => x.ProductCatalog).FirstOrDefault();

            }
        }

        public List<OrderProduct> GetOrderSubProducts(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderProduct>(x => x.ProductCatalog); 
                ctx.LoadOptions = dlo;
                //int[] productCatalogIds = ctx.OrderProduct.Where(x => x.Quantity > 0 && x.ProductTypeId == (int)Dal.Helper.ProductType.ComponentProduct
                //&& x.OrderId == orderId && x.ProductCatalogId.HasValue).Select(x => x.ProductCatalogId.Value).ToArray();

                return ctx.OrderProduct.Where(x => x.OrderId == orderId && x.Quantity > 0 && x.ProductTypeId == (int)Dal.Helper.ProductType.ComponentProduct)
                    .ToList();// productCatalogIds.Contains(x.ProductCatalogId)).ToList();
            }
        }
         

 

        public List<AllegroDeliveryCostType> GetAllegroDeliveryCostTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.AllegroDeliveryCostType.ToList();
            }
        }

        public List<AllegroDeliveryCostsByIdResult> GetAllegroDeliveryCosts(int? deliveryCostTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.AllegroDeliveryCostsById(deliveryCostTypeId).ToList();
            }
        }

        public void SetAllegroDeliveries(int? deliveryCostTypeId, string name, bool isActive, bool isPaczkomatAvailable,  List<AllegroDeliveryCost> deliveries)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                AllegroDeliveryCostType adct;

                if (deliveryCostTypeId.HasValue)
                {
                    ctx.AllegroDeliveryCost
                        .DeleteAllOnSubmit(ctx.AllegroDeliveryCost.Where(x => x.DeliveryCostTypeId == deliveryCostTypeId.Value).ToList());

                     adct = ctx.AllegroDeliveryCostType.Where(x => x.DeliveryCostTypeId == deliveryCostTypeId.Value).FirstOrDefault();
                    adct.Name = name;
                    adct.IsActive = isActive;
                    adct.IsPaczkomatAvailable = isPaczkomatAvailable;
                }
                else
                {
                    int newId = ctx.AllegroDeliveryCostType.Max(x => x.DeliveryCostTypeId) + 1;
                    adct = new AllegroDeliveryCostType()
                    {
                        Name = name,
                        DeliveryCostTypeId = newId,
                        IsActive = isActive,
                        IsPaczkomatAvailable = isPaczkomatAvailable
                    };
                }

                foreach (AllegroDeliveryCost delivery in deliveries)
                    delivery.AllegroDeliveryCostType = adct;

                ctx.AllegroDeliveryCost.InsertAllOnSubmit(deliveries);

                ctx.SubmitChanges();
            }
        }

        public List<AllegroDeliveryCostTypesWithNumbers> GetAllegroDeliveryCostTypesWithNumbers()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {

                return ctx.AllegroDeliveryCostTypesWithNumbers.ToList();
            }
        }

   

        //public OrderExportBatch GetOrderExportBatch(int batchId)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.OrderExportBatch.Where(x => x.OrderExportBatchId == batchId).FirstOrDefault();
        //    }
        //}

        public List<OrderComplaint> GetOrderComplaints(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderComplaint>(x => x.OrderComplantType);
                ctx.LoadOptions = dlo;
                return ctx.OrderComplaint.Where(x => x.OrderId == orderId).OrderBy(x => x.InsertDate).ToList();
            }
        }


        public void SetOrderComplaintUpdate(OrderComplaint oc, string UserName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.OrderComplaint ocToUpdate = ctx.OrderComplaint.Where(x => x.Id == oc.Id).FirstOrDefault();
                ocToUpdate.Comment = oc.Comment;
                ocToUpdate.ComplaintPerson = oc.ComplaintPerson;
                ocToUpdate.Cost = oc.Cost;
                ocToUpdate.OrderComplaintTypeId = oc.OrderComplaintTypeId;

                ctx.SubmitChanges();
            }
        }

        public List<OrderComplaint> GetOrderComplaints()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OrderComplaint>(x => x.OrderComplantType);
                ctx.LoadOptions = dlo;
                return ctx.OrderComplaint.OrderByDescending(x => x.InsertDate).ToList();
            }
        }

        public List<ComplaintsStats> GetOrderComplaintsByMonth()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ComplaintsStats.OrderByDescending(x=>x.Month).ToList();
            }
        }

        //public List<Dal.OrderExportView> GetOrderExportView(int[] batchIds)
        //{
        //    using (LajtitViewsDB ctx = new LajtitViewsDB())
        //    {
        //        int[] orderIds = GetOrderIDsBasedOnBatchIDs(batchIds);

        //        return ctx.OrderExportView.Where(x => orderIds.Contains(x.OrderId)).ToList();
        //    }
        //}

        //public ProductCatalogView GetProductCatalogByCode(string code)
        //{
        //    using (LajtitViewsDB ctx = new LajtitViewsDB())
        //    {

        //        return ctx.ProductCatalogView.Where(x => x.Code == code).FirstOrDefault();

        //    }
        //}

        public int SetProductCatalogDuplicate(int productCatalogId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                return ctx.ProductCatalogDuplicate(productCatalogId);

            }
        }
 

        public void SetOrderCompanyId(int OrderId, int companyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Order order = ctx.Order.Where(x=>x.OrderId == OrderId).FirstOrDefault();
                order.CompanyId = companyId;


                TableLog log = new TableLog()
                {
                    Comment = String.Format("Aktualizacja firmy realizującej zamówienie: {0}", companyId),
                    InsertDate = DateTime.Now,
                    InsertUser = "System",
                    ObjectId = order.OrderId,
                    TableName = "Order"
                };
                ctx.TableLog.InsertOnSubmit(log);


                ctx.SubmitChanges();

            }
        }

        public List<Dal.AllegroActionType> GetAllegroActions()
        {
            using (LajtitAllegroDB ctx = new LajtitAllegroDB())
            {
                return ctx.AllegroActionType.ToList();
            }
        }

        public void SetAllegroActions(int[] productIds, int? shopId, int actionTypeId, bool excludeAuction,  string comment)
        {

            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                IQueryable<ProductCatalogAllegroItemsActive> items = ctx.ProductCatalogAllegroItemsActive
                    .Where(x => productIds.Contains(x.ProductCatalogId));

                if (shopId.HasValue)
                    items = items.Where(x => x.ShopId == shopId);



                SetAllegroActions(items.ToList(), excludeAuction, null, actionTypeId, comment);


            }
        }
        public void SetAllegroActions(List<ProductCatalogAllegroItemsActive> items, bool excludeAuction, bool? doNotReActive,
            int actionTypeId, string comment)
        {
            using (LajtitAllegroDB ctxa = new LajtitAllegroDB())
            {
                foreach (var item in items)
                {
                    if (excludeAuction && item.SellingMode != null && item.SellingMode == "AUCTION")
                        continue;
                    Dal.AllegroAction action = new AllegroAction()
                    {
                        InsertDate = DateTime.Now,
                        IsProcessed = false,
                        ItemId = item.ItemId,
                        TypeId = actionTypeId,
                        Comment= comment,
                        DoNotReActive= doNotReActive
                    };
                    ctxa.AllegroAction.InsertOnSubmit(action);
                }
                ctxa.SubmitChanges();
            }

        }
        public void SetAllegroActions(long[] itemIds, int actionTypeId, bool doNotReActive,  string comment)
        {
            using (LajtitAllegroDB ctxa = new LajtitAllegroDB())
            {
                foreach (var item in itemIds)
                { 
                    Dal.AllegroAction action = new AllegroAction()
                    {
                        InsertDate = DateTime.Now,
                        IsProcessed = false,
                        ItemId = item,
                        TypeId = actionTypeId,
                        Comment= comment,
                        DoNotReActive = doNotReActive
                    };
                    ctxa.AllegroAction.InsertOnSubmit(action);
                }
                ctxa.SubmitChanges();
            }

        }
        public List<Supplier> GetSuppliersForOrder(int orderId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderProduct.Where(x => x.OrderId == orderId && x.ProductCatalogId.HasValue && x.Quantity > 0)
                    .Select(x => x.ProductCatalog.Supplier)
                    .Distinct()
                    .ToList();
            }
        }

        public List<ShopPaymentTrackerType> GetShopPaymentTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopPaymentTrackerType.Where(x => x.PaymentTypeId == null).ToList();
            }
        }

        public void SetShopTrackerPayment(ShopPaymentTracker t)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                Dal.ShopPaymentTrackerType pt = ctx.ShopPaymentTrackerType
                    .Where(x => x.ShopPaymentTypeId == t.ShopPaymentTypeId)
                    .FirstOrDefault();

                Dal.ShopPaymentTracker shopPaymentTracker = ctx.ShopPaymentTracker.Where(x =>
                    x.InsertDate.Year == t.InsertDate.Year
                    && x.InsertDate.Month == t.InsertDate.Month
                    && x.InsertDate.Day == t.InsertDate.Day
                    && (x.ShopPaymentTypeId == 1 || x.ShopPaymentTypeId == 2)
                    && x.ShopPaymentTypeId == t.ShopPaymentTypeId).FirstOrDefault();

                if (shopPaymentTracker != null)
                    throw new Exception(String.Format("Wpis /{0}/ już istnieje dla dzisiejszego dnia", pt.Name));

                ctx.ShopPaymentTracker.InsertOnSubmit(t);
                ctx.SubmitChanges();
            }
        }

        public List<ShopPaymentTrackerView> ShopPaymentTrackerReport(int year, int month, int day)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ShopPaymentTrackerView
                    .Where(x => x.Year == year && x.Month == month && x.Day == day)
                    .OrderBy(x => x.DisplayOrder)
                    .ThenBy(x => x.InsertDate)
                    .ToList();
            }
        }

        public List<Dal.SellReportResult> GetSellReport(string userName, DateTime date)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SellReport(userName, date).ToList();
            }
        }

        public void SetOrderPaymentMoveOut(int paymentId, string UserName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.OrderPayment payment = ctx.OrderPayment.Where(x => x.PaymentId == paymentId).FirstOrDefault();

                if (payment.AmountMovedOut == null)
                {
                    payment.AmountMovedOut = payment.Amount;
                    payment.Amount = 0;
                    payment.AmountMovedOutDate = DateTime.Now;
                    if (payment.Comment == null)
                        payment.Comment = "";
                    payment.Comment += String.Format(" (wpłata {2:C} wyksięgowana przez: {0} w dniu {1:yyyy/MM/dd HH:mm})",
                    UserName, DateTime.Now, payment.AmountMovedOut);
                    payment.AmountMovedOutUserName = UserName;
                    ctx.SubmitChanges();

                }
            }
        }

        public void SetCostChecked(int costId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Cost cost = ctx.Cost.Where(x => x.CostId == costId).FirstOrDefault();
                cost.IsChecked = cost.IsChecked.HasValue && cost.IsChecked.Value? false:true;
                ctx.SubmitChanges();
            }
        }

        public int GetOrders(string email)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Order.Where(x => x.Email == email && x.OrderStatusId > 0).Count();
            }
        }

        public List<OrderProductsWaitingForDelivery> GetOrdersProductsWaitingForDelivery()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrderProductsWaitingForDelivery.OrderBy(x => x.SupplierName).ThenBy(x => x.StatusName).ThenBy(x=>x.OrderDate).ToList();
            }
        }

        public void SetOrderProductsStatus(int[] orderProductIds, int orderProductStatusId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<OrderProduct> orderProducts = ctx.OrderProduct.Where(x => orderProductIds.Contains(x.OrderProductId)).ToList();
                List<OrderProductStatus> statuses = ctx.OrderProductStatus.ToList();
                string newStatus = statuses.Where(x => x.OrderProductStatusId == orderProductStatusId)
                    .Select(x => x.StatusName).FirstOrDefault();
                foreach (OrderProduct op in orderProducts)
                {
                    string status = statuses.Where(x => x.OrderProductStatusId == op.OrderProductStatusId)
                        .Select(x => x.StatusName).FirstOrDefault();
                    op.OrderProductStatusId = orderProductStatusId;

                    TableLog log = new TableLog()
                    {
                        Comment = String.Format("Zmiana statusu produktu {2} z {0} na {1}", status, newStatus, op.ProductCatalog.Name),
                        InsertDate = DateTime.Now,
                        InsertUser = userName,
                        ObjectId = op.OrderProductId,
                        TableName = "OrderProduct"
                    };
                    ctx.TableLog.InsertOnSubmit(log);
                    ctx.SubmitChanges();

                }
            }
        }
        public void SetProductOrdersStatus(int[] productOrderIds, int orderProductStatusId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<ProductOrder> orderProducts = ctx.ProductOrder.Where(x => productOrderIds.Contains(x.Id)).ToList();
                List<OrderProductStatus> statuses = ctx.OrderProductStatus.ToList();
                string newStatus = statuses.Where(x => x.OrderProductStatusId == orderProductStatusId)
                    .Select(x => x.StatusName).FirstOrDefault();
                foreach (ProductOrder op in orderProducts)
                {
                    string status = statuses.Where(x => x.OrderProductStatusId == op.OrderProductStatusId)
                        .Select(x => x.StatusName).FirstOrDefault();
                    op.OrderProductStatusId = orderProductStatusId;

                    TableLog log = new TableLog()
                    {
                        Comment = String.Format("Zmiana statusu produktu {2} z {0} na {1}", status, newStatus, op.ProductCatalog.Name),
                        InsertDate = DateTime.Now,
                        InsertUser = userName,
                        ObjectId = op.Id,
                        TableName = "ProductOrder"
                    };
                    ctx.TableLog.InsertOnSubmit(log);
                    ctx.SubmitChanges();

                }
            }
        }

        public bool GetProductCatalogImageExists(ProductCatalogImage image)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogImage.Where(x => x.ProductCatalogId == image.ProductCatalogId && x.OriginalFileName == image.OriginalFileName && x.Size == image.Size).FirstOrDefault() != null;
            }
        }

        public List<OrderInvoiceCorrection> GetInvoiceCorrection(int orderId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrderInvoiceCorrection.Where(x => x.OrderId == orderId).ToList();
            }
        }

        //public List<ProductCatalogAllegroItemErrorView> GetProductCatalogAllegroItemsWithErrors()
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        return ctx.ProductCatalogAllegroItemErrorView.OrderBy(x => x.LastUpdateDateTime).ToList();            }
        //}

        public List<SupplierImportType> GetSupplierImportTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierImportType.ToList();
            }
        }

        public List<OrdersProcessingReport> GetOrdersProcessingReport()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrdersProcessingReport.ToList();
            }
        }



        public List<ShopsFnResult> GetShops(int supplierId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ShopsFn(supplierId).ToList();
            }
        }

        public List<Shop> GetShops()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Shop.ToList();
            }
        }

        //public void SetOrderTrackingNumberClear(int orderId, string serviceCode, string userName)
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        Order order = ctx.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
        //        order.TrackingNumberSent = null;
        //        order.ShipmentTrackingNumber = null;
        //        order.InpostServiceCode = serviceCode;
        //        order.InpostShipmentId = null;



        //        OrderStatusHistory ash = new OrderStatusHistory()
        //        {
        //            Comment = String.Format("Generowanie nowego numeru przesyłki. Miejsce nadania: {0}", serviceCode),
        //            InsertDate = DateTime.Now,
        //            InsertUser = userName,
        //            OrderId = orderId,
        //            OrderStatusId = (int)Helper.OrderStatus.Comment
        //        };
        //        ctx.OrderStatusHistory.InsertOnSubmit(ash);


        //        ctx.SubmitChanges();
        //    }
        //}

        public List<Supplier> GetSuppliersForShop(Helper.Shop shop)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierShop.Where(x => x.ShopId == (int)shop).Select(x => x.Supplier).ToList();
            }
        }

        public List<SupplierOrderingType> GetSupplierOrderingTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.SupplierOrderingType.ToList();
            }
        }

        public List<ComplaintStatus> GetComplaintStatuses()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ComplaintStatus.ToList();
            }
        }

        public List<ComplaintStatusHistory> GetComplaintHistory(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ComplaintStatusHistory>(x => x.ComplaintStatus);
                ctx.LoadOptions = dlo;

                return ctx.ComplaintStatusHistory.Where(x => x.OrderComplaintId == id).OrderByDescending(x => x.InsertDate).ToList();
            }
        }

        public OrderComplaint GetOrderComplaint(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OrderComplaint.Where(x => x.Id == id).FirstOrDefault();
            }
        }



        public List<Cost> GetCostsBatches()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Cost.Where(x => x.BatchId != null).OrderByDescending(x => x.BatchDate).ToList();
            }
        }

        public List<OrdersUprocessed> GetOrdersUnprocessed()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OrdersUprocessed.ToList();
            }
        }
    }
}