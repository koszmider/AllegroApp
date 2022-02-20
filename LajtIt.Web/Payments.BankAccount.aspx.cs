using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;

namespace LajtIt.Web
{
    [Developer("c164338f-0b52-434f-a4bb-bd2ca8b566ea")]
    public partial class PaymentsBankAccount : LajtitPage
    {
        decimal total = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindMonths();
                BindCompanies();
                BindAccoutingTypes();
            }
        }

        private void BindAccoutingTypes()
        {
        


            var t = Dal.DbHelper.Accounting.GetBankAccountTypes();
            chblBankAccountType.DataSource = t;
            chblBankAccountType.DataBind();
            ddlBankAccountType.DataSource = t;
            ddlBankAccountType.DataBind();
        }

        private void BindCompanies()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            var c = oh.GetCompanies().Where(x=>x.IsMyCompany && x.IsActive);
            ddlCompany.DataSource = c;
            ddlCompany.DataBind(); 
        }
        private void BindMonths()
        {
            List<ListItem> items = new List<ListItem>();
            for (int year = DateTime.Now.Year; year >= 2010; year--)
                for (int month = DateTime.Now.Year == year ? DateTime.Now.Month : 12; month >= 1; month--)
                {
                    ListItem item = new ListItem()
                    {
                        Text = String.Format("{0}/{1}", year, month),
                        Value = String.Format("{0}/{1}/{2}", year, month, 1)
                    };
                    items.Add(item);
                }

            ddlMonth.Items.AddRange(items.ToArray()); 
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<Dal.OrderPaymentsView> payments = Dal.DbHelper.Orders
                .GetOrderPayments(DateTime.Parse(ddlMonth.SelectedValue), Int32.Parse(ddlCompany.SelectedValue));


            lblEwidencja.Text = String.Format("{0:C}", payments.Where(x => x.AccountingTypeId == 1).Sum(x => x.Amount));
            lblFaktury.Text = String.Format("{0:C}", payments.Where(x => x.AccountingTypeId == 2).Sum(x => x.Amount));
            lblKasa.Text = String.Format("{0:C}", payments.Where(x => x.AccountingTypeId == 3).Sum(x => x.Amount));

            lblTotalAccount.Text = String.Format("{0:C}", payments.Sum(x => x.Amount));


            List<Dal.BankAccountView> bankAccounts = Dal.DbHelper.Accounting.GetBankAccount(DateTime.Parse(ddlMonth.SelectedValue), Int32.Parse(ddlCompany.SelectedValue), Int32.Parse(ddlAccountId.SelectedValue));
            bankAccounts = bankAccounts.Where(x => x.AccountId == Int32.Parse(ddlAccountId.SelectedValue)).ToList();

            lblCredit.Text = String.Format("{0:C}", bankAccounts.Where(x => x.TransferType== "CRDT").Sum(x => x.Amount));
            lblDebit.Text = String.Format("{0:C}", bankAccounts.Where(x => x.TransferType == "DBIT").Sum(x => x.Amount));

            int[] bankAccountTypeIds = chblBankAccountType.Items.Cast<ListItem>().Where(x => x.Selected)
                .Select(x => Int32.Parse(x.Value)).ToArray();

            if (ddlPaymentType.SelectedIndex != 0)
                bankAccounts = bankAccounts.Where(x => x.TransferType == ddlPaymentType.SelectedValue).ToList();

            if (bankAccountTypeIds.Count() > 0)
                bankAccounts = bankAccounts.Where(x => x.BankAccountTypeId.HasValue && bankAccountTypeIds.Contains(x.BankAccountTypeId.Value)).ToList();
            if (chbNotAssigned.Checked)
                bankAccounts = bankAccounts.Where(x => x.BankAccountTypeId.HasValue ==false).ToList();        
            if (chbOrderNotAssigned.Checked)
                bankAccounts = bankAccounts.Where(x => x.OrderId.HasValue ==false).ToList();
            if(!String.IsNullOrEmpty(txbSearch.Text))
                bankAccounts = bankAccounts.Where(x => x.ClientName.ToLower().Contains(txbSearch.Text.Trim().ToLower()) 
                || x.Comment.ToLower().Contains(txbSearch.Text.Trim().ToLower())).ToList();

            //     if (paymentTypeIds.Count() > 0)
            //         payments = payments.Where(x => paymentTypeIds.Contains(x.PaymentTypeId)).ToList();

            //     int[] accoutingTypeIds = chbAccountingType.Items.Cast<ListItem>().Where(x => x.Selected)
            //.Select(x => Int32.Parse(x.Value)).ToArray();

            //     if (accoutingTypeIds.Count() > 0)
            //         payments = payments.Where(x => x.AccountingTypeId.HasValue &&
            //             accoutingTypeIds.Contains(x.AccountingTypeId.Value)).ToList();


            //     if (chbInvoice1.Checked)
            //         payments = payments.Where(x => x.InvoiceNumber != null).ToList();
            //     if (chbInvoice0.Checked)
            //         payments = payments.Where(x => x.InvoiceNumber == null).ToList();

            gvPayments.DataSource = bankAccounts;
            gvPayments.DataBind();


            lblCurrent.Text =String.Format("{0:C}", total);

            //if(chbOrderPaymentType.Items.Count==0)
            //{
            //    chbOrderPaymentType.DataSource = payments.Select(x => new { x.PaymentTypeId, x.PaymentName }).Distinct().ToList();
            //    chbOrderPaymentType.DataBind();
            //}
        }

        protected void gvPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal LitId = e.Row.FindControl("LitId") as Literal;
                LitId.Text = String.Format("{0}.", e.Row.RowIndex + 1);

                Dal.BankAccountView o = e.Row.DataItem as Dal.BankAccountView;
                total += o.Amount;


                ImageButton imgOrderEdit = e.Row.FindControl("imgOrderEdit") as ImageButton;

                if (o.BankAccountTypeId == 2) // wpłata klienta
                {

                    if (o.OrderPaymentId.HasValue)
                    {
                        HyperLink hlOrder = e.Row.FindControl("hlOrder") as HyperLink;
                        hlOrder.NavigateUrl = String.Format(hlOrder.NavigateUrl, o.OrderId);
                        hlOrder.Text = o.OrderId.ToString();

                        if (o.Amount != o.OrderPaymentAmount.Value)
                            e.Row.BackColor = Color.Pink;

                    }


                    imgOrderEdit.CommandArgument = o.Id.ToString();
                }
                else

                    imgOrderEdit.Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = String.Format("{0:C}", total);
            }
        }

        private void BindBankPayment(int bankAccountId)
        {
            ViewState["bankAccountId"] = bankAccountId;
            List<Dal.OrderPayment> payments = Dal.DbHelper.Accounting.GetOrderPaymentsForBankAccount(bankAccountId, Int32.Parse(ddlAccountId.SelectedValue));

            ddlOrderPayments.DataSource = payments
                .OrderBy(x=>x.Amount)
                .Select(x=>new {
            x.PaymentId,
            Text = String.Format("Zamówienie {0} - {1:C}", x.OrderId, x.Amount)
            }).ToList();
            ddlOrderPayments.DataBind();



            Dal.BankAccount ba = Dal.DbHelper.Accounting.GetBankAccount(bankAccountId);
            lblBankAccount.Text = String.Format("{0}<br>{1}<br>Kwota: {2:C}", ba.ClientName, ba.Comment, ba.Amount);
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {

            int[] ids = WebHelper.GetSelectedIds<int>(gvPayments, "chbPOrder");


            Dal.DbHelper.Accounting
               .SetBankAccountTypes(ids, Int32.Parse(ddlBankAccountType.SelectedValue));

            DisplayMessage("Zapisano zmiany");
            btnSearch_Click(null, null);
        }
        private string SaveFile(HttpPostedFile postedFile)
        {
            string oryginalFileName = "";
            //try
            //{ 

            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];


            string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(postedFile.FileName));
            oryginalFileName = System.IO.Path.GetFileName(postedFile.FileName);
            string saveLocation = String.Format(path, fileName);


            return saveLocation;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            HttpFileCollection uploadedFiles = Request.Files;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbe = new StringBuilder();
            string saveLocation = "";




            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];

                try
                {
                    if (userPostedFile.ContentLength > 0)
                    {
                        saveLocation = SaveFile(userPostedFile);

                        userPostedFile.SaveAs(saveLocation);
                        Bll.INGHelper.LoadData(saveLocation);
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



            //if (rblActionType.SelectedIndex == 1)
            //    ReadAndExecute(saveLocation);
        }

        protected void imgOrderEdit_Click(object sender, ImageClickEventArgs e)
        {

            BindBankPayment(Convert.ToInt32(((ImageButton)sender).CommandArgument));

            mpeBankAccount.Show();
        }

        protected void btnOrderPaymentAssign_Click(object sender, EventArgs e)
        {
            int bankAccountId = Int32.Parse(ViewState["bankAccountId"].ToString());
            int orderPaymentId = Int32.Parse(ddlOrderPayments.SelectedValue);
            Dal.DbHelper.Accounting.SetOrderPaymentsForBankAccount(bankAccountId, orderPaymentId);

            mpeBankAccount.Hide();
            DisplayMessage("Przypisano");

            btnSearch_Click(null, null);
        }

        protected void lbtnAutoAssign_Click(object sender, EventArgs e)
        {
            Dal.DbHelper.Accounting.SetBankAccountToOrderPayment(DateTime.Parse(ddlMonth.SelectedValue)
                , Int32.Parse(ddlCompany.SelectedValue)
                , Int32.Parse(ddlAccountId.SelectedValue));


            btnSearch_Click(null, null);
        }

        protected void lbtnBankStatement_Click(object sender, EventArgs e)
        {
            Dal.AccountingHelper2 ah = new Dal.AccountingHelper2();
            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));

            List<Dal.BankAccountView> bankAccounts = Dal.DbHelper.Accounting.GetBankAccount(DateTime.Parse(ddlMonth.SelectedValue),
                Int32.Parse(ddlCompany.SelectedValue),
                Int32.Parse(ddlAccountId.SelectedValue));

            string fileName = pdf.BankStatement(bankAccounts,
                Int32.Parse(ddlCompany.SelectedValue),
                Int32.Parse(ddlAccountId.SelectedValue),
                DateTime.Parse(ddlMonth.SelectedValue));


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }
    }
}