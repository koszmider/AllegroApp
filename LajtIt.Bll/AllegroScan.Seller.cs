using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Bll
{
    public partial class AllegroScan
    {
        /// <summary>
        /// Metoda pozwala na pobranie pełnych danych kontaktowych kontrahentów z danej oferty.
        /// </summary>
        /// <param name="itemIDs"></param>
        //public void ProcessBuyerData(long[] itemIDs)
        //{
             

        //    Dal.AllegroScan allegroScan = new Dal.AllegroScan();
        //    AllegroHelper ah = new AllegroHelper();

        //    foreach (AllegroNewWCF.ItemPostBuyDataStruct buy in ah.GetBuyersData(itemIDs))
        //    {
        //        List<Dal.AllegroItemOrder> list = ProcessItemBuyers(buy);
        //        allegroScan.SetItemBuyers(list);
        //    }



        //}

        private Dal.AllegroItemOrder GetAllegroItemOrderFromBuyersData(string userName, long itemId, long buyerId)
        {
            AllegroHelper ah = new AllegroHelper();
            List<Dal.AllegroItemOrder> list = new List<Dal.AllegroItemOrder>();

            foreach (AllegroNewWCF.ItemPostBuyDataStruct buy in ah.GetBuyersData(userName, new long[]{itemId}))
            {
                list.AddRange(ProcessItemBuyers(buy));
            }

            return list.Where(x => x.UserId == buyerId).FirstOrDefault();

        }
        private List<Dal.AllegroItemOrder> ProcessItemBuyers(AllegroNewWCF.ItemPostBuyDataStruct buy)
        {
            List<Dal.AllegroItemOrder> list = new List<Dal.AllegroItemOrder>();
            if (buy == null || buy.usersPostBuyData== null) return list;

            foreach (AllegroNewWCF.UserPostBuyDataStruct b in buy.usersPostBuyData)
            {
                Dal.AllegroItemOrder aio = new Dal.AllegroItemOrder();
                aio.ItemId = buy.itemId;

 
                aio.UserId = b.userData.userId;
                aio.UserName = b.userData.userLogin;
                aio.UserPointCount = b.userData.userRating;
                aio.FirstName = b.userData.userFirstName;
                aio.LastName = b.userData.userLastName;
                aio.CompanyName = b.userData.userCompany;
                aio.CountryId = b.userData.userCountryId;
                aio.StateId = b.userData.userStateId;
                aio.Postcode = b.userData.userPostcode;
                aio.City = b.userData.userCity;
                aio.Address = b.userData.userAddress;
                aio.Email = b.userData.userEmail;
                aio.Phone = b.userData.userPhone;
                aio.Phone2 = b.userData.userPhone2;
                aio.RegistrationCountryId = b.userData.siteCountryId;
                aio.Junior = b.userData.userJuniorStatus == 1 ? true : false;
                aio.HasShop = b.userData.userHasShop == 1 ? true : false;
                aio.CompanyIcon = b.userData.userCompanyIcon == 1 ? true : false;
                aio.ShipmentFirstName = b.userSentToData.userFirstName;
                aio.ShipmentLastName = b.userSentToData.userLastName;
                aio.ShipmentCompanyName = b.userSentToData.userCompany;
                aio.ShipmentCountryId = b.userSentToData.userCountryId;
                aio.ShipmentPostcode = b.userSentToData.userPostcode;
                aio.ShipmentCity = b.userSentToData.userCity;
                aio.ShipmentAddress = b.userSentToData.userAddress;
                aio.AllegroStandard = b.userData.userIsAllegroStandard == 1 ? true : false;
                aio.OrderStatusId = 1;

                list.Add(aio);
            }

            return list;
        }

        /// <summary>
        /// Metoda pozwala sprzedającym na pobranie wszystkich danych z wypełnionych przez kupujących formularzy pozakupowych oraz dopłat do nich. 
        /// </summary>
        /// <param name="itemIDs"></param>
        //public void ProcessItemTransactions(long[] itemIDs)
        //{
        //    try
        //    {
        //        AllegroHelper ah = new AllegroHelper();
        //        AllegroNewWCF.PostBuyFormDataStruct[] buys = ah.GetItemTransactions(itemIDs);

        //        if (buys == null) return;

        //        foreach (AllegroNewWCF.PostBuyFormDataStruct buy in buys)
        //        {
        //            ProcessBuy(buy);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private void ProcessBuy(AllegroNewWCF.PostBuyFormDataStruct buy)
        {

            try
            {
                Dal.AllegroItemTransaction transaction = new Dal.AllegroItemTransaction()
                {
                    BuyAmount = (decimal)buy.postBuyFormAmount,
                    UserBuyerId = buy.postBuyFormBuyerId,
                    PaymentCancelDate = buy.postBuyFormDateCancel,
                    PaymentStartDate = buy.postBuyFormDateInit,
                    PaymentEndDate = buy.postBuyFormDateRecv,
                    //buy.postBuyFormgdadditionalinfo,
                    //buy.postBuyFormgdaddress,
                    BuyFormId = buy.postBuyFormId,
                    InvoiceCity = buy.postBuyFormInvoiceData.postBuyFormAdrCity,
                    InvoiceCompany = buy.postBuyFormInvoiceData.postBuyFormAdrCompany,
                    InvoiceName = buy.postBuyFormInvoiceData.postBuyFormAdrFullName,
                    InvoiceNIP = buy.postBuyFormInvoiceData.postBuyFormAdrNip,
                    InvoicePhone = buy.postBuyFormInvoiceData.postBuyFormAdrPhone,
                    InvoicePostcode = buy.postBuyFormInvoiceData.postBuyFormAdrPostcode,
                    InvoiceStreet = buy.postBuyFormInvoiceData.postBuyFormAdrStreet,
                    Invoice = buy.postBuyFormInvoiceOption == 1 ? true : false,
                    /// buy.postBuyFormitems,
                    UserMessage = buy.postBuyFormMsgToSeller,
                    PaymentId = buy.postBuyFormPayId,
                    PaymentAmount = (decimal)buy.postBuyFormPaymentAmount,
                    PaymentStatus = buy.postBuyFormPayStatus,
                    PaymentType = buy.postBuyFormPayType,
                    PostageAmount = (decimal)buy.postBuyFormPostageAmount,
                    // buy.postBuyFormsentbyseller,
                    ShipmentCity = buy.postBuyFormShipmentAddress.postBuyFormAdrCity,
                    ShipmentStreet = buy.postBuyFormShipmentAddress.postBuyFormAdrStreet,
                    ShipmentCompany = buy.postBuyFormShipmentAddress.postBuyFormAdrCompany,
                    ShipmentName = buy.postBuyFormShipmentAddress.postBuyFormAdrFullName,
                    ShipmentPhone = buy.postBuyFormShipmentAddress.postBuyFormAdrPhone,
                    ShipmentPostcode = buy.postBuyFormShipmentAddress.postBuyFormAdrPostcode,
                    ShipmentId = buy.postBuyFormShipmentId,
                    InsertDate = DateTime.Now,
                    ShipmentDeliveryPointInfo = buy.postBuyFormGdAddress.postBuyFormAdrFullName
                };
                List<Dal.AllegroItemTransactionItem> items = new List<Dal.AllegroItemTransactionItem>();
                foreach (var i in buy.postBuyFormItems)
                {
                    Dal.AllegroItemTransactionItem item = new Dal.AllegroItemTransactionItem()
                    {
                        Amount = (decimal)i.postBuyFormItAmount,
                        ItemId = i.postBuyFormItId,
                        Name = i.postBuyFormItTitle,
                        Price = (decimal)i.postBuyFormItPrice,
                        Quantity = i.postBuyFormItQuantity,
                        AllegroItemTransaction = transaction,
                        InsertDate = DateTime.Now
                    };

                    items.Add(item);
                }
                Dal.AllegroScan allegroScan = new Dal.AllegroScan();
                allegroScan.SetTransaction(transaction, items);
            }catch(Exception ex)
            {

                throw ex;
            }
        }
    }
}
