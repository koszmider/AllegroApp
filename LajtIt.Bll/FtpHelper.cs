using LajtIt.Dal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Bll
{
    public class FtpHelper
    {
        public void UploadProductCatalogImages()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImagesToUpload();

            foreach (Dal.ProductCatalogImage image in images)
            {
                bool result = UploadImageToftp(image);

                if (result)
                    pch.SetProductCatalogImageUploaded(image);
            }
        }
        public void UploadProductCatalogImagesBySupplierId(int id)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImagesToUploadBySupplierId(id);

            foreach (Dal.ProductCatalogImage image in images)
            {
                bool result = UploadImageToftp(image);

                if (result)
                    pch.SetProductCatalogImageUploaded(image);
            }
        }
        private bool UploadImageToftp(Dal.ProductCatalogImage image)

        {

            try
            {
                string server = String.Format("ftp://{0}/ProductCatalog/", ConfigurationManager.AppSettings["Ftp"]); //server path
                string name = String.Format(ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())], image.FileName); ; //image path
                string Imagename = Path.GetFileName(name);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}{1}", server, Imagename)));
                request.Timeout = 86400000;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FtpU"], ConfigurationManager.AppSettings["FtpP"]);
                Stream ftpStream = request.GetRequestStream();
                FileStream fs = File.OpenRead(name);
                byte[] buffer = new byte[1024];
                int byteRead = 0;
                do
                {
                    byteRead = fs.Read(buffer, 0, 1024);
                    ftpStream.Write(buffer, 0, byteRead);
                }
                while (byteRead != 0);
                fs.Close();
                ftpStream.Flush();
                ftpStream.Close();

                return true;
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("ProductCatalogId: {0}, ImageId: {1}", image.ProductCatalogId, image.ImageId));
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="toDirectory">pusty lub poprzedzony i zakonczony / np /ProductCatalog/</param>
        /// <returns></returns>
        public bool UploadFileToftp(string file, string toDirectory)

        {

            try
            {
                string server = String.Format("ftp://{0}{1}", ConfigurationManager.AppSettings["Ftp"], toDirectory); //server path
                //string name = String.Format(ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())], image.FileName); ; //image path
                string fileName = Path.GetFileName(file);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}{1}", server, fileName)));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FtpU"], ConfigurationManager.AppSettings["FtpP"]);
                Stream ftpStream = request.GetRequestStream();
                FileStream fs = File.OpenRead(file);
                byte[] buffer = new byte[1024];
                int byteRead = 0;
                do
                {
                    byteRead = fs.Read(buffer, 0, 1024);
                    ftpStream.Write(buffer, 0, byteRead);
                }
                while (byteRead != 0);
                fs.Close();
                ftpStream.Flush();
                ftpStream.Close();

                return true;
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Błąd wysłania pliku: {0} ", file));
                return false;
            }
        }
        public void ClearFtpFiles()
        {
        List<string> files = GetFtpDirectoryContents(new Uri(String.Format("ftp://{0}/ProductCatalog/", ConfigurationManager.AppSettings["Ftp"])));


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetFtpFiles(files);


            List<Dal.FtpFilesOnServerNotInDB> filesToDelete = pch.GetFtpFiles();

            foreach (Dal.FtpFilesOnServerNotInDB file in filesToDelete)
                DeleteFileFromFtp(file);



        }

        private void DeleteFileFromFtp(FtpFilesOnServerNotInDB file)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(String.Format("ftp://{0}/ProductCatalog/{1}", ConfigurationManager.AppSettings["Ftp"], file.FileName)));
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FtpU"], ConfigurationManager.AppSettings["FtpP"]); ; //Set the Credentials of current FtpWebRequest.

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Delete status: {0}", response.StatusDescription);
            response.Close();
        }

        private  List<string> GetFtpDirectoryContents(Uri requestUri)
        {
            var directoryContents = new List<string>(); //Create empty list to fill it later.
                                                        //Create ftpWebRequest object with given options to get the Directory Contents. 
            var ftpWebRequest = GetFtpWebRequest(requestUri,  WebRequestMethods.Ftp.ListDirectory);
            try
            {
                using (var ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse()) //Excute the ftpWebRequest and Get It's Response.
                using (var streamReader = new StreamReader(ftpWebResponse.GetResponseStream())) //Get list of the Directory Contentss as Stream.
                {
                    var line = string.Empty; //Initial default value for line.
                    do
                    {
                        line = streamReader.ReadLine(); //Read current line of Stream.
                        if(line!=null)
                        directoryContents.Add(line); //Add current line to Directory Contentss List.
                    } while (!string.IsNullOrEmpty(line)); //Keep reading while the line has value.
                }
            }
            catch (Exception) { } //Do nothing incase of Exception occurred.
            return directoryContents; //Return all list of Directory Contentss: Files/Sub Directories.
        }

        private  FtpWebRequest GetFtpWebRequest(Uri requestUri,   string method = null)
        {
            var ftpWebRequest = (FtpWebRequest)WebRequest.Create(requestUri); //Create FtpWebRequest with given Request Uri.
            ftpWebRequest.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FtpU"], ConfigurationManager.AppSettings["FtpP"]); ; //Set the Credentials of current FtpWebRequest.

            if (!string.IsNullOrEmpty(method))
                ftpWebRequest.Method = method; //Set the Method of FtpWebRequest incase it has a value.
            return ftpWebRequest; //Return the configured FtpWebRequest.
        }

    }
}
