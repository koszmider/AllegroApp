using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static LajtIt.Bll.Helper;

namespace LajtIt.Bll
{
    interface IImportData
    {
        void ProcessData<T>(T obj);

    }
    public class ImportData
    { 
        public int SupplierId
        {
            get;set;
        }
        public bool UpdatePurchasePrice
        {
            get;set;
        }
        public T LoadData<T>()
        {
            if (SupplierId == 0)
                throw new Exception("Nie określono dostawcy");

            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier supplier = oh.GetSupplier(SupplierId);

            return LoadData<T>(supplier.ImportUrl, supplier.Name);


        }
 
        public T LoadData<T>(string remoteUri, string name)
        {

            //FtpWebRequest request =
            //(FtpWebRequest)WebRequest.Create("ftp://maytoni.com/stock-retail.csv");
            //request.Credentials = new NetworkCredential("stockretail", "EszMDYSaojhxDAt");
            //request.EnableSsl = true;
            //request.Method = WebRequestMethods.Ftp.DownloadFile;

            //FtpWebResponse response = (FtpWebResponse)request.GetResponse();


            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            string fileName = String.Format("{1}_{0:yyyyMMddHHss}.xml", DateTime.Now, name);

            string saveLocation = String.Format(path, fileName);

            try
            {

                // Create a new WebClient instance.
                using (WebClientDownload myWebClient = new WebClientDownload(180000))
                {
                    myWebClient.DownloadFile(remoteUri, saveLocation);
                }
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", name));
                return default(T);

            }


            try
            {
                //saveLocation = @"C:\Users\jacek\source\repos\AllegroApp\LajtIt.Web\Files\ImportFiles\Lampex_201908010945.xml";
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(saveLocation);

                //Read the first line of text
                string line = sr.ReadToEnd();
                //close the file
                sr.Close();

                line = line.Replace("<xmp>", "");
                line = line.Replace("</xmp>", "");

                StreamWriter sw = new StreamWriter(saveLocation);


                sw.Write(line);
                sw.Close();


            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }


            try
            {


                System.IO.StreamReader str = new System.IO.StreamReader(saveLocation);
                System.Xml.Serialization.XmlSerializer xSerializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(T));
                T data = (T)xSerializer.Deserialize(str);
                str.Close();
                return data;
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania pliku ze statusami {0}", name));
                return default(T);
            }
        }
        internal void PostLoadProcess()
        {
            UpdateImportDetails();
            SendNotification();
        }

        private void UpdateImportDetails()
        {
            if (SupplierId==0)
                return;

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetSupplierImportDate(SupplierId, DateTime.Now);
        }
        private void SendNotification()
        {


        }
    }
}
