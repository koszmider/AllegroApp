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
    public class NowodvorskiHelper
    {

        int supplierId = 14;


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
                client.Credentials = new NetworkCredential(sh.GetSetting("NOWO_L").StringValue, sh.GetSetting("NOWO_P").StringValue);
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
            
            List<Dal.ProductCatalog> products = new List<Dal.ProductCatalog>();


            foreach (string line in File.ReadAllLines(fileName))
            {
                try
                {
                    string[] fields = line.Split(',');

                    Dal.ProductCatalog pc = Dal.ProductCatalogHelper.GetProductCatalog(supplierId, fields[1], Convert.ToInt32(fields[2]) > 0);
                    pc.Ean = fields[0];
                    pc.SupplierQuantity = Convert.ToInt32(fields[2]);
                    products.Add(pc);

                }
                catch (Exception ex)
                {
                    //Bll.ErrorHandler.SendError(ex, String.Format("NowodvorskiImport. Błąd czytania csv: {0}", line));
                }
            }
             


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetProductCatalogUpdateNowodvorski(supplierId, products);
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(supplierId, DateTime.Now);
        }
    }
}
