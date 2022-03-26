using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("32ea64e9-e5d3-43f1-99f7-ef3201f7187e")]
    public partial class ProductFileImportProcessing : LajtitPage
    {
        public int FileId { get { return Convert.ToInt32(Request.QueryString["id"]); } }
        private List<Dal.ProductCatalogFileValidation> validationResults;
        private string updateFields = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }

        } 
        protected void lbtnStatusChange_Click(object sender, EventArgs e)
        {
            int statusId = Convert.ToInt32(ddlStatus.SelectedValue);

            Dal.ProductFileImportHelper pf = new Dal.ProductFileImportHelper();

            Dal.ProductCatalogFile file = pf.GetFile(FileId);

            if (statusId == (int)Dal.Helper.FileImportStatus.ReadyToImport)
            {
                if (file.FileImportStatusId != (int)Dal.Helper.FileImportStatus.Ok)
                {
                    DisplayMessage("Aby zatwierdzić plik do importu musi mieć status OK");
                    return;
                }

                if (!txbUpdateDate.Text.Equals(""))
                {
                    Dal.Update update = new Dal.Update()
                    {
                        InsertDate = DateTime.Now,
                        InsertUser = "Administrator",
                        StartDate = DateTime.Parse(txbUpdateDate.Text),
                        FileId = FileId,
                        IsActive = false,
                        Description = Dal.DbHelper.ProductCatalog.GetSupplierName(file.SupplierId) + " " + file.Comment
                    };

                    Dal.PromoHelper ph = new Dal.PromoHelper();

                    ph.AddUpdate(update);
                    DisplayMessage("Zmiana statusu została pomyślnie zaplanowana");
                    return;
                }
            }
            else
            {
                if (!txbUpdateDate.Text.Equals(""))
                {
                    DisplayMessage("Opóźnienie zmiany statusu możliwe jest jedynie dla statusu \"Gotowy do importu\"");
                    return;
                }
            }

            Dal.ProductCatalogFile pcf = new Dal.ProductCatalogFile()
            {
                FileImportStatusId = statusId,
                ProductCatalogFileId = FileId

            };
            pf.SetFileUpdateStatus(pcf);
            DisplayMessage("Status został zmieniony");
            BindData();

        } 
        private void BindData()
        {
            Dal.ProductFileImportHelper pf = new Dal.ProductFileImportHelper();
            List<Dal.ProductCatalogFileDataFnResult> products = pf.GetProductFileData(FileId);
            Dal.ProductCatalogFile pcf = pf.GetFile(FileId);



            lblCount.Text = String.Format("{0}", products.Count());
            lblCountFound.Text = String.Format("{0}", products.Where(x=>x.ProductCatalogId.HasValue).Count());

            chbDuplicates.Checked = pcf.CheckDuplicates;
            updateFields = pcf.ImportUpdateFields??rblJoinColumn.SelectedValue;

            if (pcf.ImportActionTypeId.HasValue)
            {
                chbAddNew.Checked = pcf.ImportActionTypeId.Value == 1 || pcf.ImportActionTypeId.Value == 2;
                chbUpdateExisting.Checked = pcf.ImportActionTypeId.Value == 1 || pcf.ImportActionTypeId.Value == 3;
            }
            switch (ddlProductExists.SelectedIndex)
            {
                case 1: products = products.Where(x => x.ProductCatalogId.HasValue).ToList(); break;
                case 2: products = products.Where(x => !x.ProductCatalogId.HasValue).ToList(); break;
            }
            if (pcf.JoinByColumn != null)
                rblJoinColumn.SelectedValue = pcf.JoinByColumn;

            string name = txbName.Text.Trim();
            if (name != "")
                products = products.Where(x => (x.Linia!=null && x.Linia.Contains(name)) || x.Kod.Contains(name)).ToList();
            switch (ddlValidationErrors.SelectedIndex)
            {
                case 1:
                    products = products.Where(x => x.FileImportStatusId == (int)Dal.Helper.FileImportStatus.Ok).ToList();
                    break;
                case 2:
                    products = products.Where(x => x.FileImportStatusId == (int)Dal.Helper.FileImportStatus.Error).ToList();
                    break;
            }
            hlProductCatalog.NavigateUrl = String.Format(hlProductCatalog.NavigateUrl, FileId, pcf.SupplierId);

            validationResults = pf.GetFileValidationResults(FileId);

            gvFiles.AllowPaging = chbPaging.Checked;
            gvFiles.DataSource = products;
            gvFiles.DataBind();
        }

        protected void gvFiles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header && updateFields !=null)
            {

                string[] fields = updateFields.Split(new char[] { ',' });
                foreach (TableCell cell in e.Row.Cells)//.Controls.Cast<Control>().ToList())
                {
                    foreach (Control control in cell.Controls)
                        if (control.ID != null && control.ID.StartsWith("chbf"))
                        {
                            string field = control.ID.Replace("chbf", "");


                            bool exists = fields.Contains(field);
                            if (exists)
                            {
                                (control as CheckBox).Checked = true;
                                if (field == rblJoinColumn.SelectedValue)
                                {

                                    (control as CheckBox).Enabled = false;

                                }
                            }
                        }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogFileDataFnResult data = e.Row.DataItem as Dal.ProductCatalogFileDataFnResult;
                 
                HyperLink hlProductCatalog = e.Row.FindControl("hlProductCatalog") as HyperLink;


                if (data.ProductCatalogId.HasValue)
                {
                    hlProductCatalog.NavigateUrl = String.Format("/Product.aspx?id={0}", data.ProductCatalogId);
                    hlProductCatalog.Text = String.Format("{0}",data.ProductCatalogId);
                }



                foreach (TableCell cell in e.Row.Cells)//.Controls.Cast<Control>().ToList())
                {
                    foreach (Control control in cell.Controls)
                        if (control.ID != null && control.ID.StartsWith("lbl"))
                        {
                            string field = control.ID.Replace("lbl", "");

                            object o = data.GetType().GetProperties()
          .Single(pi => pi.Name == field).GetValue(data, null);
                            if (o != null)
                            {
                                (control as Label).Text = o.ToString();
                            }
                                Dal.ProductCatalogFileValidation val = validationResults.Where(x => x.FileDataId == data.FileDataId && x.FieldName == field).FirstOrDefault();
                                if(val!=null)
                                {
                                    cell.BackColor = System.Drawing.Color.Red;
                                    cell.ForeColor = System.Drawing.Color.White;
                                    cell.ToolTip = val.ErrorMsg;

                                }
                            
                        }
                }
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnJoinColumnSave_Click(object sender, EventArgs e)
        {
            SaveData(false);
        }

        private void SaveData(bool changeStatus)
        {
            Dal.ProductFileImportHelper ph = new Dal.ProductFileImportHelper();
            Dal.ProductCatalogFile file = ph.GetFile(FileId);

            if (changeStatus)
            {
                file.FileImportStatusId = (int)Dal.Helper.FileImportStatus.Processing;
                if (chbAddNew.Checked && chbUpdateExisting.Checked)
                    file.ImportActionTypeId = 1;
                else
                {
                    if (chbAddNew.Checked)
                        file.ImportActionTypeId = 2;
                    else
                        file.ImportActionTypeId = 3;
                }
                file.CheckDuplicates = chbDuplicates.Checked;
                file.ImportUpdateFields = GetUpdateFields();
            }
            else
            {
                file.ImportUpdateFields = rblJoinColumn.SelectedValue;
                file.FileImportStatusId = (int)Dal.Helper.FileImportStatus.New;

            }
            file.JoinByColumn = rblJoinColumn.SelectedValue;
            ph.SetFileUpdate(file);
            BindData();
        }

        protected void btnAddUpdate_Click(object sender, EventArgs e)
        {
            SaveData(true);

            DisplayMessage("Import został zapisany i uruchiomiony");
        }

        private string GetUpdateFields()
        {
            List<string> fieldsToUpdate = new List<string>();
            foreach (TableCell cell in gvFiles.HeaderRow.Cells)//.Controls.Cast<Control>().ToList())
            {
                foreach (Control control in cell.Controls)
                    if (control.ID != null && control.ID.StartsWith("chbf"))
                    {
                        string field = control.ID.Replace("chbf", "");

                        CheckBox chb = cell.FindControl(String.Format("chbf{0}", field)) as CheckBox;

                        if (chb != null && chb.Checked)
                            fieldsToUpdate.Add(field);
                    }
            }
            return string.Join(",", fieldsToUpdate.ToArray());
        }
         
        protected void gvFiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFiles.PageIndex = e.NewPageIndex;
            BindData();

        }
    }
}