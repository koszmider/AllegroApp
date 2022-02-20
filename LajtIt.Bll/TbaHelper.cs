using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Bll
{
    public class TbaHelper
    {
        //static string kod = "90442039559EFAC612542BC55E6A9297";

        //public bool HasBeenSent(string trackingNuymber)
        //{

        //    TbaWCF.IklServiceClient tba = new TbaWCF.IklServiceClient();
        //    TbaWCF.paczkaStatus [] statusy = tba.pobierzStatusyPrzesylki(kod, trackingNuymber, 0);

        //    if (statusy == null)
        //        return false;
        //    else
        //        return statusy.Where(x => x.skrot == "PK").Count() > 0;


        //}
        //private string CreateParcel(Dal.Order order)
        //{
        //    TbaWCF.IklServiceClient tba = new TbaWCF.IklServiceClient();
             
        //    List<TbaWCF.paczka> paczka = new List<TbaWCF.paczka>();
        //    string[] gabarytyPrzesylek = order.ShippingData.Split(',');
        //    string[] wagi = new string[] { "2,0", "5,0", "10,0", "20,0", "31,5" };
        //    for (int i = 0; i < 5; i++)
        //        for (int n = 0; n < Int32.Parse(gabarytyPrzesylek[i]); n++)
        //            paczka.Add(new TbaWCF.paczka()
        //            {
        //                typ = "PC",
        //                waga = wagi[i],
        //                gab1 = "1",
        //                gab2 = "1",
        //                gab3 = "1",
        //                ksztalt = "0",

        //            });
        //    TbaWCF.list przesylka = new TbaWCF.list()
        //    {
        //        paczki = paczka.ToArray(),
        //        formaPlatnosci = "P",
        //        nadawca = new TbaWCF.kontrahentNadawca()
        //        {
        //            numer = "155803"
        //        },
        //        nrExt = order.OrderId.ToString(),
        //        odbiorca = new TbaWCF.kontrahentOdbiorca()
        //        {
        //            emailKontakt = order.Email,
        //            kod = order.ShipmentPostcode,
        //            miasto = order.ShipmentCity,
        //            nazwisko = String.Format("{0} {1} {2}", order.ShipmentCompanyName, order.ShipmentFirstName, order.ShipmentLastName).TrimStart().TrimEnd(),
        //            nrDom = ".",
        //            nrExt = order.OrderId.ToString(),
        //            telKontakt = order.Phone,
        //            ulica = order.ShipmentAddress,
        //            czyFirma = "0",
        //            kodKraju = "PL"
        //        },

        //        uslugi = new TbaWCF.uslugi()
        //        {
                
        //            ubezpieczenie = new TbaWCF.ubezpieczenie()
        //                {
        //                    kwotaUbezpieczenia = (order.AmountToPay+100).ToString(),
        //                    opisZawartosci = "Materiały dekoracyjne"
        //                }

        //        },
        //        rodzajPrzesylki = "K",
        //        placi = "1",
        //        uwagi = "",
        //        potwierdzenieNadania = new TbaWCF.potwierdzenieNadania()
        //        {
        //             dataNadania=DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
        //              numerKuriera = "103",
        //               podpisNadawcy="Stawicki"
        //        }

        //    };
        //    if (order.ShippingType.PayOnDelivery)
        //        przesylka.uslugi.pobranie = new TbaWCF.pobranie()
        //            {
        //                formaPobrania = "B",
        //                kwotaPobrania = order.AmountToPay.ToString(),
        //                nrKonta = order.BankAccountNumber
        //            };
        //    string v = tba.pobierzWersje(kod);
        //    var q = tba.zapiszList(kod, przesylka);

        //    return q.nrPrzesylki;
        //}

        //public void ExportToTba(int[] orderIds, string actingUser)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    List<Dal.Order> orders = oh.GetOrders(orderIds).OrderBy(x => x.OrderId).ToList();

        //    foreach (Dal.Order order in orders)
        //    {
                
        //        string shipmentTrackingNumber =  CreateParcel(order);
        //        oh.SetOrderTrackingNumber(order.OrderId, shipmentTrackingNumber);
        //    }

        //    oh.SetOrdersStatus(Dal.Helper.ShippingCompany.TBA,
        //        orderIds, Dal.Helper.OrderStatus.Exported, "Export automatyczny", actingUser, "WebApi"); 
        //}
    }
}
