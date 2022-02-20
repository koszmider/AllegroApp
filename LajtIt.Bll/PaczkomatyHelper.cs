using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace LajtIt.Bll
{
    public class PaczkomatyHelper
    {
        internal void SetPaczkomaty(string actingUser)
        {
            XmlReader xml = GetXml("do=listmachines_xml");

            List<Dal.Paczkomaty> paczkomaty = GetPaczkomaty(xml);

            Dal.PaczkomatyHelper ph = new Dal.PaczkomatyHelper();
            ph.SetPaczkomaty(paczkomaty, actingUser);

        }

        private List<Dal.Paczkomaty> GetPaczkomaty(XmlReader xml)
        {
            List<Dal.Paczkomaty> paczkomaty = new List<Dal.Paczkomaty>();

            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            XmlNode PaczkomatyNode =
               doc.SelectSingleNode("/paczkomaty");
            XmlNodeList MachineNodes =
                PaczkomatyNode.SelectNodes("machine");
            Dal.Paczkomaty p = new Dal.Paczkomaty();
            try
            {
                foreach (XmlNode node in MachineNodes)
                {
                    p = new Dal.Paczkomaty()
                    {
                        Buildingnumber = node["buildingnumber"].InnerText,
                        Latitude = node["latitude"].InnerText,
                        Locationdescription = node["locationdescription"].InnerText,
                        LocationDescription2 = node["locationDescription2"].InnerText,
                        Longitude = node["longitude"].InnerText,
                        Name = node["name"].InnerText,
                        Operatinghours = node["operatinghours"].InnerText,
                        Partnerid = Convert.ToInt32(node["partnerid"].InnerText),
                        Paymentavailable = node["paymentavailable"].InnerText,
                        Paymentpointdescr = node["paymentpointdescr"].InnerText,
                        Paymenttype = Convert.ToInt32(node["paymenttype"].InnerText),
                        Postcode = node["postcode"].InnerText,
                        Province = node["province"].InnerText,
                        Status = node["status"].InnerText,
                        Street = node["street"].InnerText,
                        Town = node["town"].InnerText,
                        Type = node["type"].InnerText



                    };


                    paczkomaty.Add(p);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Błąd Paczkomaty: {0} {1}", ex.Message, p.Name));
            }

            return paczkomaty;
        }
        private List<Dal.Paczkomaty> GetPaczkomatyByName(XmlReader xml)
        {
            List<Dal.Paczkomaty> paczkomaty = new List<Dal.Paczkomaty>();

            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            XmlNode PaczkomatyNode =
               doc.SelectSingleNode("/paczkomaty");
            XmlNodeList MachineNodes =
                PaczkomatyNode.SelectNodes("machine");
            try
            {
                foreach (XmlNode node in MachineNodes)
                {
                    Dal.Paczkomaty p = new Dal.Paczkomaty()
                    {
                        Buildingnumber = node["buildingnumber"].InnerText,
                        Name = node["name"].InnerText,
                        Operatinghours = node["operatinghours"].InnerText,
                        Partnerid = Convert.ToInt32(node["partnerid"].InnerText),
                        Paymentavailable = node["paymentavailable"].InnerText,
                        Paymentpointdescr = node["paymentpointdescr"].InnerText,
                        Paymenttype = Convert.ToInt32(node["paymenttype"].InnerText),
                        Postcode = node["postcode"].InnerText,
                        Province = node["province"].InnerText,
                        Status = node["status"].InnerText,
                        Street = node["street"].InnerText,
                        Town = node["town"].InnerText,
                        Type = node["type"].InnerText



                    };


                    paczkomaty.Add(p);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return paczkomaty;
        }

        private XmlReader GetXml(string parameters)
        {
            XmlReader xmlReader = XmlReader.Create(String.Format("{0}?{1}", System.Configuration.ConfigurationManager.AppSettings["PaczkomatyURL"],
                parameters));

            return xmlReader;
         
        }

//        internal string ExportOrders(List<Dal.Order> orders, DirectoryInfo di)
//        {

//            /*
//             * e-mail;
//             * telefon;
//             * rozmiar;
//             * paczkomat;
//             * numer_referencyjny;
//             * ubezpieczenie;
//             * za_pobraniem
//jan.kowalski@inpost.pl;500300100;A;KRA128;MOJ_SKLEP#1234;10;50
//anna.nowak@inpost.pl;500700900;B;WAW350;MOJ_SKLEP#2345;20;100    
//             * */

//            StringBuilder sb = new StringBuilder();
//            sb.AppendLine("e-mail;telefon;rozmiar;paczkomat;numer_referencyjny;ubezpieczenie;za_pobraniem");

//            foreach (Dal.Order order in orders)
//            {
//                string[] shippingData = order.ShippingData.Split('|');

//                sb.AppendLine(
//                    String.Format("{0};{1};{2};{3};{4};{5};{6}",
//                    order.Email,
//                    order.Phone,
//                    shippingData[1],
//                    shippingData[0],
//                    order.OrderId,
//                    0,
//                    order.ShippingType.PayOnDelivery == true ? Math.Abs(order.AmountBalance.Value).ToString() : null)

//                    );
//            }

//            string f = String.Format("InPost_{0:yyyyMMddHHmmss}.csv", DateTime.Now);
//            string fileName = String.Format(@"{0}\{1}", di.FullName, f);
//            System.IO.StreamWriter file =
//               new System.IO.StreamWriter(fileName);

//            file.Write(sb.ToString());

//            file.Close();

//            return f;

//        }

        internal string GetPaczkomatyByPostcode(string postCode)
        {
            XmlReader xml = GetXml(String.Format("do=findnearestmachines&postcode={0}&limit=3", postCode));

            List<Dal.Paczkomaty> paczkomaty = GetPaczkomaty(xml);

            return "";
        }
    }
}
