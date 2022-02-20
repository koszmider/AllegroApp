using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using iTextSharp.text;
using System.Configuration;

namespace LajtIt.Bll
{
    public class DpdHelper
    {
        //const int fid = 180315;

       //public void ExportToDpd(Dal.Order order, string actingUser,  DpdWCF.outputDocPageFormatDSPEnumV1 pdfFormat)
       //{
       //    Dal.OrderHelper oh = new Dal.OrderHelper();

       //     if (String.IsNullOrEmpty(order.ShipmentPostcode))
       //         return;


       //    string fileName = CreateDpdDocument(order,pdfFormat);
           
       //        string shipmentTrackingNumber = order.ShipmentTrackingNumber;
       //        oh.SetOrderTrackingNumber(order.OrderId, shipmentTrackingNumber);
       // }
        public void ExportToDpd2(Dal.OrderShipping order, string actingUser, DpdWCF.outputDocPageFormatDSPEnumV1 pdfFormat)
        {
            //Dal.OrderHelper oh = new Dal.OrderHelper();

            if (String.IsNullOrEmpty(order.Order1.ShipmentPostcode))
                return;


            string fileName = CreateDpdDocument2(order, pdfFormat);

            string shipmentTrackingNumber = order.ShipmentTrackingNumber;

            Dal.DbHelper.Orders.SetOrderShippingTrackingNumber(order.Id, shipmentTrackingNumber);
            //oh.SetOrderTrackingNumber(order.OrderId, shipmentTrackingNumber);
        }
        //public void ExportToDpd(int[] orderIds, string actingUser)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    List<Dal.Order> orders = oh.GetOrders(orderIds).OrderBy(x => x.OrderId).ToList();

        //    foreach (Dal.Order order in orders)
        //    {

        //        string shipmentTrackingNumber = CreateParcel(order);
        //        oh.SetOrderTrackingNumber(order.OrderId, shipmentTrackingNumber);
        //    }

        //    oh.SetOrdersStatus(Dal.Helper.ShippingCompany.TBA,
        //        orderIds, Dal.Helper.OrderStatus.Exported, "Export automatyczny", actingUser, "WebApi");
        //}
        //internal string ExportOrders(List<Dal.Order> orders, DirectoryInfo di)
        //{


        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("nazwa;adres;kod;miasto;pobranie;telefon;email;waga;liczba;ref");

        //    foreach (Dal.Order order in orders)
        //    {
        //        string[] shippingData = order.ShippingData.Split('|');

        //        string waga = "30";
        //        for (int i = 1; i < Int32.Parse(order.ShippingData); i++)
        //            waga += "|30";

        //        sb.AppendLine(
        //            String.Format("{0} {1} {2};{3};{4};{5};{6};{7};{8};{9};{10};{11}",
        //            order.ShipmentCompanyName,
        //            order.ShipmentFirstName,
        //            order.ShipmentLastName,
        //            order.ShipmentAddress,
        //            order.ShipmentPostcode,
        //            order.ShipmentCity,
        //            order.ShippingType.PayOnDelivery == true ? Math.Abs(order.AmountBalance.Value).ToString() : "",
        //            order.Phone,
        //            order.Email,
        //            waga,
        //            order.ShippingData,
        //            order.OrderId
        //            )

        //            );
        //    }

        //    string f = String.Format("Dpd_{0:yyyyMMddHHmmss}.csv", DateTime.Now);
        //    string fileName = String.Format(@"{0}\{1}", di.FullName, f);
        //    System.IO.StreamWriter file =
        //       new System.IO.StreamWriter(fileName);

        //    file.Write(sb.ToString());

        //    file.Close();

        //    return f;

        //}
        //private string CreateDpdDocument(Dal.Order order,  DpdWCF.outputDocPageFormatDSPEnumV1 pdfFormat)
        //{
        //    DpdWCF.DPDPackageObjServicesClient dpd = new DpdWCF.DPDPackageObjServicesClient();
        //    int numcat = order.Company.DpdNumcat.Value;

        //    List<DpdWCF.packageOpenUMLFeV1> packages = new List<DpdWCF.packageOpenUMLFeV1>();


        //    DpdWCF.packageOpenUMLFeV1 package = new DpdWCF.packageOpenUMLFeV1();
        //    GetPackage(order, package);
        //    packages.Add(package);



        //    DpdWCF.packagesGenerationResponseV2 packGenResp = dpd.generatePackagesNumbersV2(
        //            packages.ToArray(),
        //            DpdWCF.pkgNumsGenerationPolicyV1.IGNORE_ERRORS,
        //            "PL",
        //            Auth(numcat));

        //    DpdWCF.dpdServicesParamsV1 servicesParams = new DpdWCF.dpdServicesParamsV1();


        //    CreateServicesParams(numcat, packGenResp, servicesParams, order);

        //    DpdWCF.documentGenerationResponseV1 docGenResp1 = dpd.generateSpedLabelsV2(servicesParams,
        //        DpdWCF.outputDocFormatDSPEnumV1.PDF,
        //        pdfFormat,
        //        "BIC3",
        //        Auth(numcat));

        //    if (docGenResp1.session.statusInfo.status == DpdWCF.statusDGREnumV1.OK)
        //    {
        //        DpdWCF.packagePGRV2 p =
        //            packGenResp.Packages.Where(x => x.Reference.Contains(order.OrderId.ToString())).FirstOrDefault();
        //        //  docGenResp1.session.packages.Where(x => x.reference == order.OrderId.ToString()).FirstOrDefault();
        //        if (p != null)
        //        {
        //            order.ShipmentTrackingNumber = p.Parcels[0].Waybill;

        //            string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
        //            path = String.Format(path, @"Shipping\Dpd\");
        //            DirectoryInfo di = new DirectoryInfo(path);
        //            return PDF.CreatePdf(docGenResp1.documentData, di, order.ShipmentTrackingNumber);
        //        }
        //        else
        //            return null;
        //    }
        //    else
        //    {
        //        //StringBuilder sb = new StringBuilder();

        //        //foreach (Dal.Order order1 in orders)
        //        //{
        //        //    DpdWCF.packagePGRV2 p =
        //        //        packGenResp.Packages.Where(x => x.Reference.Contains(order1.OrderId.ToString())).FirstOrDefault();
        //        //    //  docGenResp1.session.packages.Where(x => x.reference == order.OrderId.ToString()).FirstOrDefault();
        //        //    if (p != null)
        //        //    {
        //        //        sb.AppendLine(String.Format("{0} - {1}<br>", order1.OrderId, p.Parcels[0].Status));
        //        //        ordersRejcted.Add(order1.OrderId);
        //        //    }
        //        //}


        //        throw new Exception(
        //            String.Format("Błąd generowania etykiet<br><br>OrderId: <a href='http://192.168.0.107/Order.aspx?id={0}'>{0}</a><br><br>{1}",
        //            order.OrderId,
        //            docGenResp1.session.statusInfo.status ));
        //    }

        //}


        private string CreateDpdDocument2(Dal.OrderShipping order, DpdWCF.outputDocPageFormatDSPEnumV1 pdfFormat)
        {
            DpdWCF.DPDPackageObjServicesClient dpd = new DpdWCF.DPDPackageObjServicesClient();
            int numcat = order.Order1.Company.DpdNumcat.Value;

            List<DpdWCF.packageOpenUMLFeV1> packages = new List<DpdWCF.packageOpenUMLFeV1>();


            DpdWCF.packageOpenUMLFeV1 package = new DpdWCF.packageOpenUMLFeV1();
            GetPackage2(order, package);
            packages.Add(package);



            DpdWCF.packagesGenerationResponseV2 packGenResp = dpd.generatePackagesNumbersV2(
                    packages.ToArray(),
                    DpdWCF.pkgNumsGenerationPolicyV1.IGNORE_ERRORS,
                    "PL",
                    Auth(numcat));

            DpdWCF.dpdServicesParamsV1 servicesParams = new DpdWCF.dpdServicesParamsV1();


            CreateServicesParams(numcat, packGenResp, servicesParams, order.Order1);

            DpdWCF.documentGenerationResponseV1 docGenResp1 = dpd.generateSpedLabelsV2(servicesParams,
                DpdWCF.outputDocFormatDSPEnumV1.PDF,
                pdfFormat,
                "BIC3",
                Auth(numcat));

            if (docGenResp1.session.statusInfo.status == DpdWCF.statusDGREnumV1.OK)
            {
                DpdWCF.packagePGRV2 p =
                    packGenResp.Packages.Where(x => x.Reference.Contains(order.OrderId.ToString())).FirstOrDefault();
                //  docGenResp1.session.packages.Where(x => x.reference == order.OrderId.ToString()).FirstOrDefault();
                if (p != null)
                {
                    order.ShipmentTrackingNumber = p.Parcels[0].Waybill;

                    string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
                    path = String.Format(path, @"Shipping\Dpd\");
                    DirectoryInfo di = new DirectoryInfo(path);
                    return PDF.CreatePdf(docGenResp1.documentData, di, order.ShipmentTrackingNumber);
                }
                else
                    return null;
            }
            else
            {
                //StringBuilder sb = new StringBuilder();

                //foreach (Dal.Order order1 in orders)
                //{
                //    DpdWCF.packagePGRV2 p =
                //        packGenResp.Packages.Where(x => x.Reference.Contains(order1.OrderId.ToString())).FirstOrDefault();
                //    //  docGenResp1.session.packages.Where(x => x.reference == order.OrderId.ToString()).FirstOrDefault();
                //    if (p != null)
                //    {
                //        sb.AppendLine(String.Format("{0} - {1}<br>", order1.OrderId, p.Parcels[0].Status));
                //        ordersRejcted.Add(order1.OrderId);
                //    }
                //}


                throw new Exception(
                    String.Format("Błąd generowania etykiet<br><br>OrderId: <a href='http://192.168.0.107/Order.aspx?id={0}'>{0}</a><br><br>{1}",
                    order.OrderId,
                    docGenResp1.session.statusInfo.status));
            }

        }
        private static void GetPackage2(Dal.OrderShipping order, DpdWCF.packageOpenUMLFeV1 package)
        {

            package.receiver = GetReceiver(order.Order1);
            package.reference = package.ref1 = String.Format("{0}-{1}-{2}", order.OrderId, order.Order1.Company.DpdNumcat, Guid.NewGuid().ToString().Substring(0, 6));
            package.ref2 = package.ref3 = "";
            package.payerType = DpdWCF.payerTypeEnumOpenUMLFeV1.SENDER;
            package.payerTypeSpecified = true;
            package.thirdPartyFIDSpecified = false;

            package.sender = GetSender(order.Order1);

            if (order.COD.HasValue && order.COD> 0)
            {
                LajtIt.Bll.DpdWCF.servicesOpenUMLFeV2 services = new DpdWCF.servicesOpenUMLFeV2();
                services.cod = new DpdWCF.serviceCODOpenUMLFeV1()
                {
                    amount = Math.Abs(order.COD.Value).ToString().Replace(",", "."),
                    currency = DpdWCF.serviceCurrencyEnum.PLN,
                    currencySpecified = true

                };

                package.services = services;
            }


            AddParcels2(order, package);
        }
        //private static void GetPackage(Dal.Order order, DpdWCF.packageOpenUMLFeV1 package)
        //{

        //    package.receiver = GetReceiver(order);
        //    package.reference = package.ref1 = String.Format("{0}-{1}-{2}", order.OrderId, order.Company.DpdNumcat, Guid.NewGuid().ToString().Substring(0, 6));
        //    package.ref2 = package.ref3 = "";
        //    package.payerType = DpdWCF.payerTypeEnumOpenUMLFeV1.SENDER;
        //    package.payerTypeSpecified = true;
        //    package.thirdPartyFIDSpecified = false;

        //    package.sender = GetSender(order);

        //    if (order.ShippingType.PayOnDelivery)
        //    {
        //        LajtIt.Bll.DpdWCF.servicesOpenUMLFeV2 services = new DpdWCF.servicesOpenUMLFeV2();
        //        services.cod = new DpdWCF.serviceCODOpenUMLFeV1()
        //        {
        //            amount = Math.Abs(order.AmountBalance.Value).ToString().Replace(",", "."),
        //            currency = DpdWCF.serviceCurrencyEnum.PLN,
        //            currencySpecified = true

        //        };

        //        package.services = services;
        //    }


        //    AddParcels(order, package);
        //}

        private static void CreateServicesParams(int numcat, DpdWCF.packagesGenerationResponseV2 packGenResp, DpdWCF.dpdServicesParamsV1 servicesParams, Dal.Order order)
        {
            servicesParams.policy = DpdWCF.policyDSPEnumV1.STOP_ON_FIRST_ERROR;
            servicesParams.policySpecified = true;
            servicesParams.session = new DpdWCF.sessionDSPV1()
            {
                sessionId = packGenResp.SessionId,
                sessionIdSpecified = true,
                sessionType = DpdWCF.sessionTypeDSPEnumV1.DOMESTIC,
                sessionTypeSpecified = true
            };
            if (order.ShipmentCountryCode != "PL")
                servicesParams.session.sessionType = DpdWCF.sessionTypeDSPEnumV1.INTERNATIONAL;

            servicesParams.pickupAddress = new DpdWCF.pickupAddressDSPV1()
            {
                address = String.Format("{0} {1}", order.Company.Address, order.Company.AddressNo),
                city = order.Company.City,
                countryCode = "PL",
                email = Dal.Helper.MyEmail,
                name = "Lajtit.pl - Oświetlamy. Doradzamy",
                phone = "0048 600 732 000",
                postalCode = order.Company.PostalCode,
                fid = numcat,
                fidSpecified = true
            };
        }

        private static void AddParcels2(Dal.OrderShipping order, DpdWCF.packageOpenUMLFeV1 package)
        {
            List<DpdWCF.parcelOpenUMLFeV1> parcels = new List<DpdWCF.parcelOpenUMLFeV1>();

            //string[] shippingData = order.ShippingData.Split(new char[] { '|' });

            List<Dal.OrderShippingParcel> orderShippingParcels = Dal.DbHelper.Orders.GetOrderShippingParcels(order.Id);

            if (order.Order1.ShipmentCountryCode != "PL")
            {
                foreach (Dal.OrderShippingParcel p in orderShippingParcels)
                    if (!p.Weight.HasValue || !p.DimnesionLength.HasValue || !p.DimnesionHeight.HasValue || !p.DimnesionWidth.HasValue)
                        throw new Exception("Dla przesyłek zagranicznych należy podać wagę i wymiary");
            }


            int numberOfParcels = orderShippingParcels.Count();

            double weight = 3;

            foreach (Dal.OrderShippingParcel p in orderShippingParcels)
            {
                DpdWCF.parcelOpenUMLFeV1 parcel = new DpdWCF.parcelOpenUMLFeV1()
                { 
                    reference = Guid.NewGuid().ToString(),
                    sizeXSpecified = false,
                    sizeYSpecified = false,
                    sizeZSpecified = false,
                    weightSpecified=false,
                    content = "Ostrożnie szkło!",
                    customerData1 = ""

                };
                if (p.Weight.HasValue)
                {
                    parcel.weight =weight;
                    parcel.weightSpecified = true;

                }
        
                if ( p.DimnesionLength.HasValue && p.DimnesionHeight.HasValue && p.DimnesionWidth.HasValue)
                {
                    parcel.sizeXSpecified = true;
                    parcel.sizeX = Decimal.ToInt32(p.DimnesionWidth.Value);
                    parcel.sizeYSpecified = true;
                    parcel.sizeY = Decimal.ToInt32(p.DimnesionHeight.Value);
                    parcel.sizeZSpecified = true;
                    parcel.sizeZ = Decimal.ToInt32(p.DimnesionLength.Value);
                }
                parcels.Add(parcel);
            }
            
            package.parcels = parcels.ToArray();
        }
        //private static void AddParcels(Dal.Order order, DpdWCF.packageOpenUMLFeV1 package)
        //{
        //    List<DpdWCF.parcelOpenUMLFeV1> parcels = new List<DpdWCF.parcelOpenUMLFeV1>();

        //    string[] shippingData = order.ShippingData.Split(new char[] { '|' });

        //    if (order.ShipmentCountryCode != "PL" && shippingData.Length != 2)
        //        throw new Exception("Dla przesyłek zagranicznych należy podać wagę");



        //    int numberOfParcels = 1;
        //    if (order.ShipmentCountryCode != "PL" && shippingData.Length == 2)
        //        if (!Int32.TryParse(shippingData[0], out numberOfParcels))
        //        {
        //            numberOfParcels = 1;
        //        }
        //    if (order.ShipmentCountryCode == "PL" )
        //        if (!Int32.TryParse(order.ShippingData, out numberOfParcels))
        //        {
        //            numberOfParcels = 1;
        //        }



        //    double weight = 20;
        //    if (order.ShipmentCountryCode != "PL" && shippingData.Length == 2)
        //        weight = Double.Parse(shippingData[1]);


        //    for (int i = 0; i < numberOfParcels; i++)
        //    {
        //        parcels.Add(new DpdWCF.parcelOpenUMLFeV1()
        //        {
        //            weight = weight,
        //            weightSpecified = true,
        //            reference = Guid.NewGuid().ToString(),
        //            sizeXSpecified = false,
        //            sizeYSpecified = false,
        //            sizeZSpecified = false,
        //            content = "Ostrożnie szkło!",
        //            customerData1 = ""

        //        });
        //    }
        //    package.parcels = parcels.ToArray();
        //}

        private static   DpdWCF.packageAddressOpenUMLFeV1 GetReceiver(Dal.Order order)
        {
            return new DpdWCF.packageAddressOpenUMLFeV1()
            {
                address = order.ShipmentAddress,
                city = order.ShipmentCity,
                countryCode = order.ShipmentCountryCode,
                email = order.Email,
                name = String.Format("{0} {1} {2}", order.ShipmentCompanyName, 
                    order.ShipmentFirstName, order.ShipmentLastName).Trim(),
                phone = order.Phone,
                postalCode = order.ShipmentPostcode.Replace("-", "")
            };
        }

        private static DpdWCF.packageAddressOpenUMLFeV1  GetSender(Dal.Order order )
        {
            return new DpdWCF.packageAddressOpenUMLFeV1()
            {
                address = String.Format("{0} {1}", order.Company.Address, order.Company.AddressNo),
                city = order.Company.City,
                countryCode = "PL",
                email = Dal.Helper.MyEmail,
                name = "Lajtit.pl - Oświetlamy. Doradzamy",
                phone = "0048 600 730 000",
                postalCode = order.Company.PostalCode.Replace("-",""),
                fid = order.Company.DpdNumcat.Value,
                fidSpecified = true
            };
        }

        public string GetStatus(Dal.Order order)
        {
            DpdStatusWCF.DPDInfoServicesObjEventsClient dpd = new DpdStatusWCF.DPDInfoServicesObjEventsClient();
            string trackingNumber = "0000034780590U";
            DpdStatusWCF.customerEventsResponseV3 resp 
                = dpd.getEventsForWaybillV1(trackingNumber, DpdStatusWCF.eventsSelectTypeEnum.ALL, "PL", AuthLogin());

            
            return "";
        }
        public string Test()
        {
            DpdWCF.DPDPackageObjServicesClient dpd = new DpdWCF.DPDPackageObjServicesClient();
            DpdWCF.postalCodeV1 code = new DpdWCF.postalCodeV1();
            code.countryCode = "PL";
            code.zipCode = "93428";
            DpdWCF.authDataV1 auth = Auth(180315);

            DpdWCF.findPostalCodeResponseV1 resp = dpd.findPostalCodeV1(code, auth);
            return resp.status;

        }

        private static DpdWCF.authDataV1 Auth(int masterFid)
        {
            DpdWCF.authDataV1 auth = new DpdWCF.authDataV1();
            //auth.login = "test";
            //auth.masterFid = fid;
            //auth.password = "KqvsoFLT2M";
            //auth.login = "18031501";
            //auth.masterFid = 180315;
            //auth.password = "7Mlc6koLBngAJpD2";
            auth.login = "29943401";
            auth.masterFid = masterFid;
            auth.password = "VWSURuXGZcd1Libj";
            return auth;
        }
        private static DpdStatusWCF.authDataV1 AuthLogin()
        {
            DpdStatusWCF.authDataV1 auth = new DpdStatusWCF.authDataV1();
            //auth.login = "test";
            //auth.masterFid = fid;
            //auth.password = "KqvsoFLT2M";
            //auth.login = "18031501";
            //auth.masterFid = 180315;
            //auth.password = "7Mlc6koLBngAJpD2";
            auth.login = "180315";
            auth.channel = "clientChannel";
            auth.password = "7Mlc6koLBngAJpD2";
            return auth;
        }

    }
}
