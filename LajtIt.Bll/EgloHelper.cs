using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class EgloHelper
    {

        int supplierId = 11;


        public   void  LoadData()
        {

            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            

            Dal.Supplier supplier = oh.GetSupplier(supplierId);
            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format(path, String.Format("{0}_{1:yyyyMMMddHHmmss}.csv", supplier.Name, DateTime.Now));
            try
            {
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential(sh.GetSetting("EGLO_L").StringValue, sh.GetSetting("EGLO_P").StringValue);
                client.DownloadFile(supplier.ImportUrl, fileName);
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd wczytywania pliku: {0} ", supplier.Name));
          
            }
            ProcessData(fileName);
        }

        

        public void ProcessData(string fileName)
        {
            Dictionary<string, int> eglo = new Dictionary<string, int>();


            foreach (string line in File.ReadAllLines(fileName).Skip(1))
            {
                string[] fields = line.Split(';');
                if (!String.IsNullOrEmpty(fields[1]))
                    eglo.Add(fields[1], Convert.ToInt32(fields[3]));
            }
             


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetProductCatalogUpdateEglo(supplierId, eglo);
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(supplierId, DateTime.Now);
        }
    }
}
