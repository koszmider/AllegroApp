using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LajtIt.Bll
{
    public class PocztaPolskaHelper
    {

        public PocztaPolskaHelper()
        {

            ////string idPakiet = Guid.NewGuid().ToString();
            //tEN = new PocztaPolskaWCF.ElektronicznyNadawca();
            //System.Net.NetworkCredential c = new System.Net.NetworkCredential();
            //c.UserName = "jstawik@gazeta.pl";
            //c.Password = "Ekn19081908";

            //System.Net.CredentialCache cc = new System.Net.CredentialCache();
            //cc.Add(new Uri("https://e-nadawca.poczta-polska.pl/websrv/en.wsdl"), "Basic", c);
            //tEN.Credentials = cc;

            //Dal.OrderHelper oh = new Dal.OrderHelper();
            //int orderBatchId = 19;
            //List<Dal.Order> orders = oh.GetOrdersForPocztaPolska( orderBatchId);

            //SendOrders(orders.Take(1).ToList());
        }

        internal string ExportOrders(List<Dal.Order> orders, DirectoryInfo di)
        {
            //List<PocztaPolskaWCF.przesylkaType> przesylki = new List<PocztaPolskaWCF.przesylkaType>();

            LajtIt.Bll.PocztaPolska.Nadawca nadawca = new PocztaPolska.Nadawca()
            {
                Struktura = "1.6",
                Nazwa = "RTP Development Jacek Stawicki",
                NazwaSkrocona = "RTP",
                Ulica = "Treflowa",
                Dom = "20",
                Lokal = "",
                Miejscowosc = "Łódź",
                Kod = "93428",
                NIP = "7272491257",
                Zrodlo = "NADAWCA",
                Guid = "{" + Guid.NewGuid().ToString() + "}"
            };
            LajtIt.Bll.PocztaPolska.NadawcaZbior zbior = new PocztaPolska.NadawcaZbior()
            {
                DataUtworzenia = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                Guid = "{" + Guid.NewGuid().ToString() + "}",
                IloscPrzesylek = orders.Count.ToString(),
                Nazwa = "Przesylki",
                Opis = DateTime.Now.ToString("{yyyy-MM-ddTHH:mm:sss}"),
                Przesylka = null
            };

            List<LajtIt.Bll.PocztaPolska.NadawcaZbiorPrzesylka> przesylki = new List<PocztaPolska.NadawcaZbiorPrzesylka>();
            foreach (Dal.Order order in orders)
            {
                LajtIt.Bll.PocztaPolska.NadawcaZbiorPrzesylka przesylka = new PocztaPolska.NadawcaZbiorPrzesylka();
                List<LajtIt.Bll.PocztaPolska.NadawcaZbiorPrzesylkaAtrybut> atrybuty = new List<PocztaPolska.NadawcaZbiorPrzesylkaAtrybut>();
                atrybuty.Add(CreateAttribute(order, "", "Symbol", "845"));

                throw new NotImplementedException();
                //string[] dane = order.ShippingData.Split(new char[] { '|' });
                string masa = "";// dane[1];
               // string kategoria = order.ShippintTypeId == 1 ? "E" : "P";
                //string gabaryt = dane[0];
                atrybuty.Add(CreateAttribute(order, "", "Masa", masa));
               // atrybuty.Add(CreateAttribute(order, "", "Kategoria", kategoria));
                atrybuty.Add(CreateAttribute(order, "", "PosteRestante", "N"));
                atrybuty.Add(CreateAttribute(order, "", "Ilosc", "1"));
                atrybuty.Add(CreateAttribute(order, "", "DlaOciemn", "N"));
                atrybuty.Add(CreateAttribute(order, "", "EgzBibl", "N"));
                atrybuty.Add(CreateAttribute(order, "", "Uslugi", "R"));
                //atrybuty.Add(CreateAttribute(order, "", "Strefa", gabaryt));
                atrybuty.Add(CreateAttribute(order, "", "Wersja", "1"));
                atrybuty.Add(CreateAttribute(order, "Adresat", "Nazwa", String.Format("{0} {1}",
                    order.ShipmentFirstName, order.ShipmentLastName)));
                atrybuty.Add(CreateAttribute(order, "Adresat", "NazwaII", order.ShipmentCompanyName));
                atrybuty.Add(CreateAttribute(order, "Adresat", "Ulica", order.ShipmentAddress));
                atrybuty.Add(CreateAttribute(order, "Adresat", "Dom", "."));
                //atrybuty.Add(CreateAttribute(order, "Adresat", "Local", ""));
                atrybuty.Add(CreateAttribute(order, "Adresat", "Miejscowosc", order.ShipmentCity));
                atrybuty.Add(CreateAttribute(order, "Adresat", "Kod", order.ShipmentPostcode.Replace("-", "")));
                atrybuty.Add(CreateAttribute(order, "Adresat", "Kraj", "POLSKA"));
                przesylka.Atrybut = atrybuty.ToArray();
                przesylka.Guid = "{" + Guid.NewGuid().ToString() + "}";
                przesylki.Add(przesylka);
            }
            zbior.Przesylka = przesylki.ToArray();

            nadawca.Zbior = new PocztaPolska.NadawcaZbior[] { zbior };

            System.Xml.Serialization.XmlSerializer writer =
               new System.Xml.Serialization.XmlSerializer(nadawca.GetType());
            string file = String.Format("PocztaPolska_{0:yyyyMMddHHmmss}.xml", DateTime.Now);
            string fileName = String.Format(@"{0}\{1}", di.FullName, file);
            System.IO.StreamWriter stream =
               new System.IO.StreamWriter(fileName);

            writer.Serialize(stream, nadawca);
            stream.Close();

            return file;
        }



        private static PocztaPolska.NadawcaZbiorPrzesylkaAtrybut CreateAttribute(Dal.Order order,
            string typ,
            string nazwa,
            string value)
        {
            return new LajtIt.Bll.PocztaPolska.NadawcaZbiorPrzesylkaAtrybut()
            {
                Nazwa = nazwa,
                Typ = typ,
                Value = value
            };
        }

        private string ProcessString(string p)
        {
            if (p == null) return null;
            return p.Substring(0, Math.Max(p.Length, 3)) + Guid.NewGuid().ToString().Substring(0, 3);
        }
    }
}
