using LajtIt.Bll.JPK.VAT;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static LajtIt.Bll.CeneoHelper;

namespace LajtIt.Bll.JPK
{
    public class JPKHelper
    {
        public string GetJPKMAGFile(DateTime date, string saveLocation, int companyId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Company company = oh.GetCompanies().Where(x => x.CompanyId == companyId).FirstOrDefault();

      
            JPK_MAG.JPK jpk = new JPK_MAG.JPK();
          

            JPK_MAG.KodFormularza kodFormularza = new JPK_MAG.KodFormularza()
            {
                KodSystemowy = "JPK_MAG (1)",
                Text = "JPK_MAG",
                WersjaSchemy = "1-0"
            };
            JPK_MAG.Naglowek naglowek = new JPK_MAG.Naglowek()
            {
                KodFormularza = kodFormularza,
                WariantFormularza = "1",
                CelZlozenia = "1",
                DataWytworzeniaJPK = DateTime.Now.ToString("s"),
                DataOd = String.Format("{0:yyyy-MM-01}", date),
                DataDo = String.Format("{0:yyyy-MM-dd}", date.AddMonths(1).AddHours(-1)),
                DomyslnyKodWaluty = "PLN",
                KodUrzedu = "1010"
            };
            JPK_MAG.AdresPodmiotu adresPodmiotu = new JPK_MAG.AdresPodmiotu()
            {
                Gmina = "Łódź",
                KodKraju = "PL",
                KodPocztowy = company.PostalCode,
                Miejscowosc = company.City,
                NrDomu = company.AddressNo,
                Poczta = "Łódź",
                Powiat = "Łódź",
                Ulica = String.Format("{0} {1}", company.Address, company.AddressNo),
                Wojewodztwo = "łódzkie"
            };
            JPK_MAG.IdentyfikatorPodmiotu identyfikatorPodmiotu = new JPK_MAG.IdentyfikatorPodmiotu()
            {
                NIP = company.TaxId,
                PelnaNazwa = company.Name 
            };
            JPK_MAG.Podmiot1 podmiot = new JPK_MAG.Podmiot1()
            {
                AdresPodmiotu = adresPodmiotu,
                IdentyfikatorPodmiotu = identyfikatorPodmiotu

            };
            jpk.Naglowek = naglowek;
            jpk.Podmiot1 = podmiot;
            jpk.Magazyn = "Magazyn główny";

         

            jpk.PZ = new JPK_MAG.PZ();
            jpk.PZ.PZWartosc = new List<JPK_MAG.PZWartosc>();
            jpk.PZ.PZWiersz = new List<JPK_MAG.PZWiersz>();


            List<Dal.ProductCatalogDeliveryInvoiceView> deliveries = Dal.DbHelper.Accounting.GetProductCatalogDeliveryInvoice(date);

            int?[] costIds = deliveries.Select(x => x.CostId).Distinct().ToArray();

            foreach(int? costId in costIds)
            {

                string numerPz = "brak";
                DateTime dataPz = date;
                decimal wartoscPZ = deliveries.Where(x => x.CostId == costId).Sum(x => x.FinalPurchasePrice.Value);
                DateTime dataOtrzymaniaPz = date;
                string dostawca = "brak";

                Dal.ProductCatalogDeliveryInvoiceView delivery = deliveries.Where(x => x.CostId == costId).FirstOrDefault();

                    numerPz = delivery.InvoiceNumber ?? "brak";
                if (costId.HasValue)
                {
                    wartoscPZ = delivery.InvoiceAmount.Value;
                    dataPz = delivery.InvoiceDate.Value;
                    dataOtrzymaniaPz = delivery.insertDate.Value;
                    dostawca = delivery.SupplierName;
                }


                jpk.PZ.PZWartosc.Add(new  JPK_MAG.PZWartosc()
                {
                    NumerPZ = numerPz,
                    DataPZ = dataPz.ToString("yyyy-MM-dd"),
                    WartoscPZ = wartoscPZ.ToString(),
                    DataOtrzymaniaPZ = dataOtrzymaniaPz.ToString("yyyy-MM-dd"),
                    Dostawca= dostawca
                });


                List<Dal.ProductCatalogDeliveryInvoiceView> deliveriesInvoice = deliveries.Where(x => x.CostId == costId).ToList();

                foreach (Dal.ProductCatalogDeliveryInvoiceView d in deliveriesInvoice)
                {
                    jpk.PZ.PZWiersz.Add(new JPK_MAG.PZWiersz()
                    {
                        Numer2PZ = numerPz,
                        KodTowaruPZ = d.Code,
                        CenaJednPZ = d.FinalPurchasePrice.ToString(),
                        IloscPrzyjetaPZ = d.Quantity.ToString(),
                        JednostkaMiaryPZ = "szt",
                        NazwaTowaruPZ = d.Name,
                        WartoscPozycjiPZ = (d.FinalPurchasePrice * d.Quantity).ToString()
                    });
                }
            }

             

            string lines = Serialize(jpk);
            string fileName = String.Format("{0:yyyy-MM-01}_{1}.xml", date, Guid.NewGuid());
            File.WriteAllText(String.Format(@"{0}\{1}", saveLocation, fileName), lines);

            return fileName;

        }
        public string GetJPKFile(DateTime date, string saveLocation, int companyId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Company company = oh.GetCompanies().Where(x => x.CompanyId == companyId).FirstOrDefault();

            StawkiPodatku stawkiPodatku = new StawkiPodatku();
            stawkiPodatku.Stawka1 = "0.23";
            stawkiPodatku.Stawka2 = "0.08";
            stawkiPodatku.Stawka3 = "0.05";
            stawkiPodatku.Stawka4 = "0.00";
            stawkiPodatku.Stawka5 = "0.00";

            JPK jpk = new JPK();
            jpk.StawkiPodatku = stawkiPodatku;

            KodFormularza kodFormularza = new KodFormularza()
            {
                KodSystemowy = "JPK_FA (1)",
                Text = "JPK_FA",
                WersjaSchemy = "1-0"
            };
            Naglowek naglowek = new Naglowek()
            {
                KodFormularza = kodFormularza,
                WariantFormularza = "1",
                CelZlozenia = "1",
                DataWytworzeniaJPK = DateTime.Now.ToString("s"),
                DataOd = String.Format("{0:yyyy-MM-01}", date),
                DataDo = String.Format("{0:yyyy-MM-dd}", date.AddMonths(1).AddHours(-1)),
                DomyslnyKodWaluty = "PLN",
                KodUrzedu = "1010"
            };
            AdresPodmiotu adresPodmiotu = new AdresPodmiotu()
            {
                Gmina = "Łódź",
                KodKraju = "PL",
                KodPocztowy = company.PostalCode,
                Miejscowosc = company.City,
                NrDomu = company.AddressNo,
                Poczta = "Łódź",
                Powiat = "Łódź",
                Ulica = String.Format("{0} {1}", company.Address, company.AddressNo),
                Wojewodztwo = "łódzkie"
            };
            IdentyfikatorPodmiotu identyfikatorPodmiotu = new IdentyfikatorPodmiotu()
            {
                NIP = company.TaxId,
                PelnaNazwa = company.Name,
                REGON = company.Regon
            };
            Podmiot1 podmiot = new Podmiot1()
            {
                AdresPodmiotu = adresPodmiotu,
                IdentyfikatorPodmiotu = identyfikatorPodmiotu

            };
            jpk.Naglowek = naglowek;
            jpk.Podmiot1 = podmiot;
            jpk.Faktura = new List<Faktura>();

            #region Faktury
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();

            List<Dal.InvoicesView> invoices = ah.GetInvoicesFromMonth(date, companyId)
              .Where(x => x.AccountingTypeId == null || x.AccountingTypeId == 2)
              .ToList();


            foreach (Dal.InvoicesView invoice in invoices)
            {
                Faktura faktura = new Faktura()
                {
                    RodzajFaktury = "VAT",
                    Typ = "G",
                    P_1 = String.Format("{0:yyyy-MM-dd}", invoice.InvoiceDate),
                    P_106E_2 = "false",
                    P_106E_3 = "false",
                    P_15 = FormatValue(invoice.CalculatedTotalBrutto.Value),
                    P_16 = "false",
                    P_17 = "false",
                    P_18 = "false",
                    P_19 = "false",
                    P_20 = "false",
                    P_21 = "false",
                    P_23 = "false",
                    P_2A = invoice.InvoiceNumber,
                    P_3A = invoice.CompanyName,
                    P_3B = invoice.CompanyAddress,
                    P_3C = company.Name,
                    P_3D = String.Format("{0} {1}, {2} {3}", company.Address, company.AddressNo, company.PostalCode, company.City),
                    P_4A = "PL",
                    P_4B = company.TaxId,
                    P_6 = String.Format("{0:yyyy-MM-dd}", invoice.InvoiceSellDate)
                };
                if (invoice.CalculatedTotalNetto000.HasValue)
                {
                    faktura.P_13_5 = FormatValue(invoice.CalculatedTotalNetto000.Value); //invoice.CalculatedTotalNetto000.ToString();
                    faktura.P_14_5 = FormatValue(invoice.CalculatedTotalNetto000.Value); //invoice.CalculatedTotalNetto000.ToString();
                }
                else
                {
                    faktura.P_13_1 = FormatValue(invoice.CalculatedTotalNetto023.Value);//invoice.CalculatedTotalNetto023.ToString();
                    faktura.P_14_1 = FormatValue(invoice.CalculatedTotalVat023.Value);// invoice.CalculatedTotalVat023.ToString();
                }
                if (!String.IsNullOrEmpty(invoice.Nip))
                    faktura.P_5B = invoice.Nip;

                jpk.Faktura.Add(faktura);
            }


            List<Dal.InvoiceProduct> invoiceProducts = ah.GetInvoiceProductsFromMonth(date, companyId)
              .Where(x => x.Invoice.AccountingTypeId == null || x.Invoice.AccountingTypeId == 2)
              .ToList();

            jpk.FakturaWiersz = new List<FakturaWiersz>();

            foreach (Dal.InvoiceProduct product in invoiceProducts)
            {
                FakturaWiersz fakturaWiersz = new FakturaWiersz()
                {
                    Typ = "G",
                    P_2B = product.Invoice.InvoiceNumber,
                    P_7 = product.Name,
                    P_8A = product.MeasureName,
                    P_8B = FormatValue(product.Quantity),
                    P_9A = FormatValue(product.PriceBrutto * (1 - product.Rebate / 100)),
                    P_9B = FormatValue(product.CalculatedTotalNetto.Value / product.Quantity),
                    P_11 = FormatValue(product.CalculatedTotalNetto.Value),
                    P_11A = FormatValue(product.CalculatedTotalBrutto.Value),
                    P_12 = FormatValue(product.VatRate * 100)
                };
                jpk.FakturaWiersz.Add(fakturaWiersz);
            }

            FakturaWierszCtrl fakturaWierszCtrl = new FakturaWierszCtrl()
            {
                LiczbaWierszyFaktur = invoiceProducts.Count().ToString(),
                WartoscWierszyFaktur = FormatValue(invoiceProducts.Sum(x => x.CalculatedTotalBrutto).Value)
            };
            jpk.FakturaWierszCtrl = fakturaWierszCtrl;
            #endregion

            FakturaCtrl fakturaCtrl = new FakturaCtrl()
            {
                LiczbaFaktur = invoices.Count().ToString(),
                WartoscFaktur = FormatValue(invoices.Sum(x => x.CalculatedTotalBrutto).Value)
            };

            jpk.FakturaCtrl = fakturaCtrl;

            string lines = Serialize(jpk);
            string fileName = String.Format("{0:yyyy-MM-01}_{1}.xml", date, Guid.NewGuid());
            File.WriteAllText(String.Format(@"{0}\{1}", saveLocation, fileName), lines);

            return fileName;

        }

        public string GetJPKVATFile(DateTime date, string saveLocation, int companyId)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Company company = oh.GetCompanies().Where(x => x.CompanyId == companyId).FirstOrDefault();

            //StawkiPodatku stawkiPodatku = new StawkiPodatku();
            //stawkiPodatku.Stawka1 = "0.23";
            //stawkiPodatku.Stawka2 = "0.08";
            //stawkiPodatku.Stawka3 = "0.05";
            //stawkiPodatku.Stawka4 = "0.00";
            //stawkiPodatku.Stawka5 = "0.00";

            JPK_VAT.JPK jpk = new JPK_VAT.JPK();

            JPK_VAT.KodFormularza kodFormularza = new JPK_VAT.KodFormularza()
            {
                KodSystemowy = "JPK_V7M (1)",
                Text = "JPK_VAT",
                WersjaSchemy = "1-2E",
            };

            jpk.Naglowek = new JPK_VAT.Naglowek()
            {

                WariantFormularza = "1",
                CelZlozenia = new JPK_VAT.CelZlozenia() { Text = "1", Poz = "P_7" },
                DataWytworzeniaJPK = DateTime.Now.ToString("s"),

                Rok = date.Year.ToString(),
                Miesiac = date.Month.ToString(),
                KodUrzedu = "1010",
                KodFormularza = kodFormularza,
                NazwaSystemu = ""

            };


            jpk.Podmiot1 = new JPK_VAT.Podmiot1()
            {
                Rola = "Podatnik",
                OsobaNiefizyczna = new JPK_VAT.OsobaNiefizyczna()
                {
                    Email = "jacek@lajtit.pl",
                    NIP = company.TaxId,
                    PelnaNazwa = company.Name,
                    Telefon = ""
                }
            };

            jpk.Deklaracja = GetDeklaracja();


            //#region Faktury
            //Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();

            //List<Dal.InvoicesView> invoices = ah.GetInvoicesFromMonth(date, companyId);

            //foreach (Dal.InvoicesView invoice in invoices)
            //{
            //    Faktura faktura = new Faktura()
            //    {
            //        RodzajFaktury = "VAT",
            //        Typ = "G",
            //        P_1 = String.Format("{0:yyyy-MM-dd}", invoice.InvoiceDate),
            //        P_106E_2 = "false",
            //        P_106E_3 = "false",
            //        P_15 = FormatValue(invoice.CalculatedTotalBrutto.Value),
            //        P_16 = "false",
            //        P_17 = "false",
            //        P_18 = "false",
            //        P_19 = "false",
            //        P_20 = "false",
            //        P_21 = "false",
            //        P_23 = "false",
            //        P_2A = invoice.InvoiceNumber,
            //        P_3A = invoice.CompanyName,
            //        P_3B = invoice.CompanyAddress,
            //        P_3C = company.Name,
            //        P_3D = String.Format("{0} {1}, {2} {3}", company.Address, company.AddressNo, company.PostalCode, company.City),
            //        P_4A = "PL",
            //        P_4B = company.TaxId,
            //        P_6 = String.Format("{0:yyyy-MM-dd}", invoice.InvoiceSellDate)
            //    };
            //    if (invoice.CalculatedTotalNetto000.HasValue)
            //    {
            //        faktura.P_13_5 = FormatValue(invoice.CalculatedTotalNetto000.Value); //invoice.CalculatedTotalNetto000.ToString();
            //        faktura.P_14_5 = FormatValue(invoice.CalculatedTotalNetto000.Value); //invoice.CalculatedTotalNetto000.ToString();
            //    }
            //    else
            //    {
            //        faktura.P_13_1 = FormatValue(invoice.CalculatedTotalNetto023.Value);//invoice.CalculatedTotalNetto023.ToString();
            //        faktura.P_14_1 = FormatValue(invoice.CalculatedTotalVat023.Value);// invoice.CalculatedTotalVat023.ToString();
            //    }
            //    if (!String.IsNullOrEmpty(invoice.Nip))
            //        faktura.P_5B = invoice.Nip;

            //    jpk.Faktura.Add(faktura);
            //}


            //List<Dal.InvoiceProduct> invoiceProducts = ah.GetInvoiceProductsFromMonth(date, companyId);
            //jpk.FakturaWiersz = new List<FakturaWiersz>();

            //foreach (Dal.InvoiceProduct product in invoiceProducts)
            //{
            //    FakturaWiersz fakturaWiersz = new FakturaWiersz()
            //    {
            //        Typ = "G",
            //        P_2B = product.Invoice.InvoiceNumber,
            //        P_7 = product.Name,
            //        P_8A = product.MeasureName,
            //        P_8B = FormatValue(product.Quantity),
            //        P_9A = FormatValue(product.PriceBrutto * (1 - product.Rebate / 100)),
            //        P_9B = FormatValue(product.CalculatedTotalNetto.Value / product.Quantity),
            //        P_11 = FormatValue(product.CalculatedTotalNetto.Value),
            //        P_11A = FormatValue(product.CalculatedTotalBrutto.Value),
            //        P_12 = FormatValue(product.VatRate * 100)
            //    };
            //    jpk.FakturaWiersz.Add(fakturaWiersz);
            //}

            //FakturaWierszCtrl fakturaWierszCtrl = new FakturaWierszCtrl()
            //{
            //    LiczbaWierszyFaktur = invoiceProducts.Count().ToString(),
            //    WartoscWierszyFaktur = FormatValue(invoiceProducts.Sum(x => x.CalculatedTotalBrutto).Value)
            //};
            //jpk.FakturaWierszCtrl = fakturaWierszCtrl;
            //#endregion

            //FakturaCtrl fakturaCtrl = new FakturaCtrl()
            //{
            //    LiczbaFaktur = invoices.Count().ToString(),
            //    WartoscFaktur = FormatValue(invoices.Sum(x => x.CalculatedTotalBrutto).Value)
            //};

            //jpk.FakturaCtrl = fakturaCtrl;

            string lines = Serialize(jpk);
            string fileName = String.Format("{0:yyyy-MM-01}_{1}.xml", date, Guid.NewGuid());
            File.WriteAllText(String.Format(@"{0}\{1}", saveLocation, fileName), lines);

            return fileName;

        }

        private JPK_VAT.Deklaracja GetDeklaracja()
        {
            JPK_VAT.Deklaracja deklaracja = new JPK_VAT.Deklaracja();

            JPK_VAT.KodFormularzaDekl kodFormularzaDekl = new JPK_VAT.KodFormularzaDekl()
            {
                KodPodatku = "VAT",
                KodSystemowy = "VAT-7 (21)",
                RodzajZobowiazania = "Z",
                Text = "VAT-7",
                WersjaSchemy = "1-2E"
            };
             
            deklaracja.Naglowek = new JPK_VAT.Naglowek()
            {
                KodFormularzaDekl = kodFormularzaDekl,
                WariantFormularzaDekl = "21"
            };

            deklaracja.PozycjeSzczegolowe = GetPozycjeSzczegolowe();
            deklaracja.Pouczenia = "1";

            return deklaracja;
        }

        private JPK_VAT.PozycjeSzczegolowe GetPozycjeSzczegolowe()
        {
            JPK_VAT.PozycjeSzczegolowe ps = new JPK_VAT.PozycjeSzczegolowe()
            {
                P_10 = "0",
                P_11 = "0",
                P_12 = "0",
                P_13 = "0",
                P_14 = "0",
                P_15 = "0",
                P_16 = "0",
                P_17 = "0",
                P_18 = "0",
                P_19 = "0",
                P_20 = "0",
                P_21 = "0",
                P_22 = "0",
                P_23 = "0",
                P_24 = "0",
                P_25 = "0",
                P_26 = "0",
                P_27 = "0",
                P_28 = "0",
                P_29 = "0",
                P_30 = "0",
                P_31 = "0",
                P_32 = "0",
                P_33 = "0",
                P_34 = "0",
                P_35 = "0",
                P_36 = "0",
                P_37 = "0",
                P_38 = "0",
                P_39 = "0",
                P_40 = "0",
                P_41 = "0",
                P_42 = "0",
                P_43 = "0",
                P_44 = "0",
                P_45 = "0",
                P_46 = "0",
                P_47 = "0",
                P_48 = "0",
                P_49 = "0",
                P_50 = "0",
                P_51 = "0",
                P_52 = "0",
                P_53 = "0",
                P_54 = "0",
                P_55 = "0",
                P_56 = "0",
                P_57 = "0",
                P_58 = "0",
                P_59 = "0",
                P_60 = "0",
                P_61 = "0",
                P_62 = "0",
                P_63 = "0",
                P_64 = "0",
                P_65 = "0",
                P_66 = "0",
                P_67 = "0",
                P_68 = "0",
                P_69 = "0",
                P_ORDZU = ""
            };

            return ps;
        }

        private string FormatValue(decimal v)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0:0.00}", v);
        }

        public static string Serialize(object dataToSerialize)
        {
            if (dataToSerialize == null) return null;

            using (StringWriter stringwriter = new  Utf8StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("etd", "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2016/01/25/eD/DefinicjeTypy/");
                ns.Add("kck", "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2013/05/23/eD/KodyCECHKRAJOW/");
                ns.Add("tns", "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/");
                ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                var serializer = new XmlSerializer(dataToSerialize.GetType());
                serializer.Serialize(stringwriter, dataToSerialize, ns);
                return stringwriter.ToString();
            }
             


        }
    }
}
