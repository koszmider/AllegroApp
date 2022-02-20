using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("9aa61fd5-1a3b-4486-9abd-5dcc2bcb92cd")]
    public partial class ProductFileImport : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindSuppliers();
                BindFiles();
            }

        }
        protected void lbtnTemplate_Click(object sender, EventArgs e)
        {
            Bll.ProductFileImportHelper pf = new Bll.ProductFileImportHelper();
            string file = pf.GetFileTemplate();

            if(file!=null)
            {
                string contentType = contentType = "application/vnd.ms-excel";

                Response.ContentType = contentType;
                Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(file)).Name);

                //Write the file directly to the HTTP content output stream.
                Response.WriteFile(file);
                Response.End();

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindFiles();
        }
        //private void ReadExcel()
        //{
        //    Bll.ProductFileImportHelper ph = new Bll.ProductFileImportHelper();
        //    ph.ReadExcel();
        //}

        private void BindFiles()
        {
            Dal.ProductFileImportHelper pf = new Dal.ProductFileImportHelper();

            int[] statuses;

            if (ddlStatus.SelectedIndex == 0)
                statuses = new int[] { 1, 2, 3, 4, 6 };
            else
                statuses = new int[] { 0, 5 };

            gvFiles.DataSource = pf.GetFiles().Where(x => statuses.Contains(x.FileImportStatusId)).ToList();
            gvFiles.DataBind();
        }

        private void BindSuppliers()
        { 
            ddlSuppliers.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderBy(x => x.Name).ToList();
            ddlSuppliers.DataBind();


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            HttpFileCollection uploadedFiles = Request.Files;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbe = new StringBuilder();
            string saveLocation = "";

            if(txbComment.Text.Trim()=="")
            {
                DisplayMessage("Komentarz do aktualizacji jest wymagany");
                return;
            }


            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];

                try
                {
                    if (userPostedFile.ContentLength > 0)
                    {
                        saveLocation = SaveFile(userPostedFile);
                        sb.AppendLine(String.Format("{0}. plik: {1}<br>", i + 1, userPostedFile.FileName));

                    }
                }
                catch (Exception Ex)
                {
                    sbe.AppendLine(String.Format("{0}. plik: {1} - {2}<br>", i + 1, userPostedFile.FileName, Ex.Message));
                    // Span1.Text += "Error: <br>" + Ex.Message;
                }
            }
            if (sbe.Length == 0)
                DisplayMessage(String.Format("Zapisano poprawnie<br><br>{0}", sb.ToString()));
            else
                DisplayMessage(String.Format("Błędy<br><br>{0}", sbe.ToString()));
            BindFiles();


            //if (rblActionType.SelectedIndex == 1)
            //    ReadAndExecute(saveLocation);
        }

        //private void ReadAndExecute(string saveLocation)
        //{
        //    switch(ddlSuppliers.SelectedValue)
        //    {
        //        case "22":
        //            Bll.ItaluxHelper ih = new Bll.ItaluxHelper(saveLocation);
        //            DisplayMessage(String.Format("Zaimportowano dane Italux"));
        //            break;
        //        case "37":
        //            Bll.AuhilonHelper ah = new Bll.AuhilonHelper(saveLocation);
        //            DisplayMessage(String.Format("Zaimportowano dane Auhilon"));
        //            break;

        //    }
        //}

        private string SaveFile(HttpPostedFile postedFile)
        {
            string oryginalFileName = "";
            //try
            //{ 

            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];


            string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(postedFile.FileName));
            oryginalFileName = System.IO.Path.GetFileName(postedFile.FileName);
            string saveLocation = String.Format(path, fileName);


            postedFile.SaveAs(saveLocation);

            Dal.ProductCatalogFile file = new Dal.ProductCatalogFile()
            {
                InsertUser = UserName,
                FileImportStatusId = 1,
                InsertDate = DateTime.Now,
                SupplierId = Convert.ToInt32(ddlSuppliers.SelectedValue),
                FileName = fileName,
                Comment = txbComment.Text.Trim()
            };

            if (rblActionType.SelectedIndex == 0)
            {
                Bll.ProductFileImportHelper ph = new Bll.ProductFileImportHelper();
                ph.ReadAndSaveExcel(file, saveLocation);
            }

            return saveLocation;
        }
    }
}