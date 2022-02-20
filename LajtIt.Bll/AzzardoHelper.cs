using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class AzzardoHelper : ImportData, IImportData
    {
        #region class
        [XmlRoot(ElementName = "Towar")]
        public class Towar
        {
            [XmlElement(ElementName = "kod")]
            public string Kod { get; set; }
            //[XmlElement(ElementName = "new_index")]
            //public string New_index { get; set; }
            [XmlElement(ElementName = "nazwa")]
            public string Nazwa { get; set; }
            [XmlElement(ElementName = "kod_kreskowy")]
            public string Kod_kreskowy { get; set; }
            [XmlElement(ElementName = "stan_magazynowy")]
            public string Stan_magazynowy { get; set; }
        }

        [XmlRoot(ElementName = "Towary")]
        public class Towary
        {
            [XmlElement(ElementName = "Towar")]
            public List<Towar> Towar { get; set; }
        }
        #endregion




        public new void LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }



        public void ProcessData<T>(T obj)
        {

            LajtIt.Bll.AzzardoHelper.Towary data = obj as LajtIt.Bll.AzzardoHelper.Towary;

            if (data == null)
                throw new Exception("Plik z danymi AZzardo jest pusty lub nie może być pobrany");


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ProductCatalogGroupHelper pchg = new Dal.ProductCatalogGroupHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogBySupplier(new int[] { SupplierId })
                 
                .ToList();
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier supplier = oh.GetSupplier(SupplierId);


            //List<Dal.ProductCatalogGroup> groups = pchg.GetProductCatalogGroups(SupplierId);

            //string[] d = data.Towar.Select(x => x.Stan_magazynowy).Distinct().ToArray();
            foreach (Dal.ProductCatalog pc in products)
            {
                try
                {
                    var r = data.Towar.Where(x => x.Kod_kreskowy!=null && x.Kod_kreskowy.Length>=13 && x.Kod_kreskowy.Trim().Substring(0,13) == pc.Ean ).FirstOrDefault();


                    if (r == null)
                    { 
                        pc.IsAvailable = false;
                        pc.SupplierQuantity = 0;
                    }
                    else
                    {
                        string stan = r.Stan_magazynowy.Replace(">", "");

                        int s = 0;
                        bool ok = false;
                        if (Int32.TryParse(stan, out s))
                            ok = true;

                        if (ok && s < 10 && s > 0)
                            pc.SupplierQuantity = s;
                        else
                            pc.SupplierQuantity = null;

                        pc.IsAvailable = ok && s > 0; // Convert.ToInt32(r.Stan.Replace(",0000", "")) > 0;

                        //if (r.New_index != null && pc.Code2 == null)
                        //{
                        //    pc.Code2 = r.New_index.Trim();
                        //}
                    }

                }
                catch (Exception ex)
                {
                    LajtIt.Bll.ErrorHandler.LogError(ex, String.Format("AZzardo kod: {0}", pc.Code));
                }

              
            }
            string[] azzardoEan = data.Towar.Where(x => x.Kod_kreskowy != "" && x.Kod_kreskowy != null).Select(x => x.Kod_kreskowy.Trim()).Distinct().ToArray();

            string[] dbEan = products.Where(x=>x.Ean!=null  ).Select(x => x.Ean.Trim() ).Distinct().ToArray();

            string[] eanToAdd = azzardoEan.Where(x => !dbEan.Contains(x)).ToArray();

            pch.SetProductCatalogUpdateZumaline(products, SupplierId);


            foreach (string ean in eanToAdd)
            {
                Towar t = data.Towar.Where(x => x.Kod_kreskowy.Length>=13 && x.Kod_kreskowy.Substring(0, 13) == ean && x.Kod!="").FirstOrDefault();

                if (t == null)
                    continue;

                Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(SupplierId, t.Kod, true);

                pc.Ean = ean.Substring(0, 13);
                pc.Name = t.Nazwa;

                try
                {
                    pch.SetProductCatalog(pc);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd dodawania produktu AZzardo {0} {1}", ean, t.Kod));
                }
            }
        }
    }
}
