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
    [Developer("27e1af10-c4a5-4973-bb83-9d45dec7701b")]
    public partial class PaymentsBankAccountMarketplace : LajtitPage
    {
        decimal total = 0;
        decimal totalIncome = 0;
        decimal totalOutcome = 0;
        decimal totalRefund = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShops();
                BindMonths();
                BindCompanies(); 
            }
        }

        private void BindShops()
        {
            ddlShops.DataSource = Dal.DbHelper.Shop.GetShops().Where(x => x.CanPlaceOrders || x.ShopId ==0).ToList();
            ddlShops.DataBind();
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
            List<Dal.ShopPayment> payments = Dal.DbHelper.Accounting
                .GetShopPayments(Int32.Parse(ddlShops.SelectedValue),
                DateTime.Parse(ddlMonth.SelectedValue), true)
                .OrderBy(x => x.ShopId)
                .ThenBy(x=>x.PaymentOperator)
                .ThenBy(x => x.PaymentDate)
                .ThenByDescending(x=>x.TotalAmount)
                .ToList();
                
            /*
             * CONTRIBUTION"
             * REFUND_CHARGE
             * PAYOUT">Wypła
             * 
             */
            if (ddlPaymentType.SelectedIndex != 0)
                payments = payments.Where(x => x.PaymentTypeId == Int32.Parse(ddlPaymentType.SelectedValue)).ToList();

            //lblEwidencja.Text = String.Format("{0:C}", payments.Where(x => x.AccountingTypeId == 1).Sum(x => x.Amount));
            //lblFaktury.Text = String.Format("{0:C}", payments.Where(x => x.AccountingTypeId == 2).Sum(x => x.Amount));
            //lblKasa.Text = String.Format("{0:C}", payments.Where(x => x.AccountingTypeId == 3).Sum(x => x.Amount));

            //lblTotalAccount.Text = String.Format("{0:C}", payments.Sum(x => x.Amount));


            //List<Dal.BankAccountView> bankAccounts = Dal.DbHelper.Accounting.GetBankAccount(DateTime.Parse(ddlMonth.SelectedValue), Int32.Parse(ddlCompany.SelectedValue));

            //lblCredit.Text = String.Format("{0:C}", bankAccounts.Where(x => x.TransferType== "CRDT").Sum(x => x.Amount));
            //lblDebit.Text = String.Format("{0:C}", bankAccounts.Where(x => x.TransferType == "DBIT").Sum(x => x.Amount));

            //int[] bankAccountTypeIds = chblBankAccountType.Items.Cast<ListItem>().Where(x => x.Selected)
            //    .Select(x => Int32.Parse(x.Value)).ToArray();

            //if (ddlPaymentType.SelectedIndex != 0)
            //    bankAccounts = bankAccounts.Where(x => x.TransferType == ddlPaymentType.SelectedValue).ToList();

            //if (bankAccountTypeIds.Count() > 0)
            //    bankAccounts = bankAccounts.Where(x => x.BankAccountTypeId.HasValue && bankAccountTypeIds.Contains(x.BankAccountTypeId.Value)).ToList();
            if (chbNotAssigned.Checked)
                payments = payments.Where(x => x.OrderPaymentId.HasValue == false).ToList();
            //if(!String.IsNullOrEmpty(txbSearch.Text))
            //    bankAccounts = bankAccounts.Where(x => x.ClientName.ToLower().Contains(txbSearch.Text.Trim().ToLower()) 
            //    || x.Comment.ToLower().Contains(txbSearch.Text.Trim().ToLower())).ToList();

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

            gvPayments.DataSource = payments;
            gvPayments.DataBind();


            lblCurrent.Text = String.Format("{0:C}", total);
            lblOutcome.Text = String.Format("{0:C}", totalOutcome);
            lblRefund.Text = String.Format("{0:C}", totalRefund);
            lblIncome.Text = String.Format("{0:C}", totalIncome);

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

                Dal.ShopPayment o = e.Row.DataItem as Dal.ShopPayment;
                total += o.Amount;
                // if (o.GroupType == "INCOME" && o.PaymentType == "CONTRIBUTION")
                if (o.PaymentTypeId == 1)
                    totalIncome += o.Amount;
                // if (o.GroupType == "OUTCOME" && o.PaymentType == "PAYOUT")
                if (o.PaymentTypeId == 3)
                    totalOutcome += o.Amount;
                //if (o.GroupType == "REFUND" && o.PaymentType == "REFUND_CHARGE")
                if (o.PaymentTypeId == 2)
                    totalRefund += o.Amount;

                ImageButton imgOrderEdit = e.Row.FindControl("imgOrderEdit") as ImageButton;


                if (o.PaymentTypeId == 3)
                    e.Row.Cells[2].BackColor = Color.Pink;

                imgOrderEdit.CommandArgument = o.Id.ToString();

                if (o.OrderPayment != null)
                {
                    HyperLink hlOrder = e.Row.FindControl("hlOrder") as HyperLink;
                    hlOrder.NavigateUrl = String.Format(hlOrder.NavigateUrl, o.OrderPayment.OrderId);
                    hlOrder.Text = o.OrderPayment.OrderId.ToString();


                    imgOrderEdit.Visible = false;
                    //if (o.Amount != o.OrderPaymentAmount.Value)
                    //    e.Row.BackColor = Color.Pink;
                }

                else

                    imgOrderEdit.Visible = true;

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[2].Text = String.Format("{0:C}", total);
                }
            }
        }
         

 

        protected void btnOrderPaymentAssign_Click(object sender, EventArgs e)
        {
            int shopPaymentId = Int32.Parse(ViewState["paymentId"].ToString());
            int orderPaymentId = Int32.Parse(ddlOrderPayments.SelectedValue);
            Dal.DbHelper.Accounting.SetOrderPaymentsForShopPayment(shopPaymentId, orderPaymentId);

            mpeBankAccount.Hide();
            DisplayMessage("Przypisano");

            btnSearch_Click(null, null);
        }

  

        protected void lbtnBankStatement_Click(object sender, EventArgs e)
        {
            string fileName = GetShopStatement(Int32.Parse(ddlShops.SelectedValue), DateTime.Parse(ddlMonth.SelectedValue));
            string contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }

        public static string GetShopStatement(int shopId, DateTime date)
        {
    
            PDF pdf = new PDF(
                String.Format(ConfigurationManager.AppSettings[String.Format("ImagesDirectory_{0}", Dal.Helper.Env.ToString())], ""),
                String.Format(ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())], ""));

            List<Dal.ShopPayment> payments =
                Dal.DbHelper.Accounting.GetShopPayments(shopId, date, false);

            string fileName = pdf.ShopPaymentsStatement(shopId, payments, date, false);

            return fileName;


        }
        public static string GetOrderProductsSent(DateTime date)
        {

            PDF pdf = new PDF(
                String.Format(ConfigurationManager.AppSettings[String.Format("ImagesDirectory_{0}", Dal.Helper.Env.ToString())], ""),
                String.Format(ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())], ""));

            List<Dal.OrderProductsSentView> ops = Dal.DbHelper.Accounting.GetOrderProductsSent(date);
            //ops = ops.Where(x => x.OrderId == 107518).ToList();
            List<Dal.OrderProductsSentView> opsPrice = ops.Where(x => x.Price != null).ToList();
            List<Dal.OrderProductsSentView> opsNullPrice = ops.Where(x => x.Price == null).ToList();
            foreach (Dal.OrderProductsSentView op in opsNullPrice)
            {
                Dal.ProductCatalogDelivery pcd = Dal.DbHelper.ProductCatalog.GetProductCatalogDeliveries((int)op.ProductCatalogId).FirstOrDefault();
                if (pcd != null)
                {
                    op.Price = pcd.Price;
                    op.Netto = pcd.Price * op.Quantity;
                    op.Brutto = pcd.Price * op.Quantity * ((Decimal)1.00 + op.VAT);
                    opsPrice.Add(op);
                }
            }

            string fileName = pdf.OrderProductsSent(opsPrice, date);

            return fileName;


        }
        public static string GetProductCatalogDelivery(DateTime date)
        {

            PDF pdf = new PDF(
                String.Format(ConfigurationManager.AppSettings[String.Format("ImagesDirectory_{0}", Dal.Helper.Env.ToString())], ""),
                String.Format(ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())], ""));

            List<Dal.ProductCatalogDeliveryWarehouseViewWithPrice> deliveries = Dal.DbHelper.Accounting.GetProductCatalogDelivery(date);


            string fileName = pdf.ProductCatalogDelivery(deliveries, date);

            return fileName;


        }
        public static string GetProductCatalogWarehouse(DateTime date)
        {
    
            PDF pdf = new PDF(
                String.Format(ConfigurationManager.AppSettings[String.Format("ImagesDirectory_{0}", Dal.Helper.Env.ToString())], ""),
                String.Format(ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())], ""));

            List<Dal.ProductCatalogDeliveryInvoiceView> deliveries = Dal.DbHelper.Accounting.GetProductCatalogDeliveryInvoice(date);


            string fileName = pdf.ProductCatalogWarehouse(deliveries, date);

            return fileName;


        }
    public static string GetProductCatalogWarehouseStock(DateTime date)
        {
    
            PDF pdf = new PDF(
                String.Format(ConfigurationManager.AppSettings[String.Format("ImagesDirectory_{0}", Dal.Helper.Env.ToString())], ""),
                String.Format(ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())], ""));

            List<Dal.ProductCatalogStockMonthEndResult> stock = Dal.DbHelper.Accounting.GetProductCatalogStockMonthEnd(date);


            string fileName = pdf.ProductCatalogWarehouseStock(stock, date);

            return fileName;


        }
        protected void lbtnAutoAssign_Click(object sender, EventArgs e)
        {
            Dal.DbHelper.Accounting.SetShopPaymentToOrderPayment(Int32.Parse(ddlShops.SelectedValue), DateTime.Parse(ddlMonth.SelectedValue));

            DisplayMessage("Przypisano");

            btnSearch_Click(null, null);
        }

        protected void imgOrderEdit_Click(object sender, ImageClickEventArgs e)
        {
            BindPayment(Convert.ToInt32(((ImageButton)sender).CommandArgument));

            mpeBankAccount.Show();

        }
        private void BindPayment(int shopPaymentId)
        {
            ViewState["paymentId"] = shopPaymentId;
            List<Dal.OrderPayment> payments = Dal.DbHelper.Accounting.GetOrderPaymentsForShopPayment(shopPaymentId, Int32.Parse(ddlShops.SelectedValue));

            ddlOrderPayments.DataSource = payments
                .OrderBy(x => x.Amount)
                .Select(x => new {
                    x.PaymentId,
                    Text = String.Format("Zamówienie {0} - {1:C}", x.OrderId, x.Amount)
                }).ToList();
            ddlOrderPayments.DataBind();



            Dal.ShopPayment ba = Dal.DbHelper.Accounting.GetShopPayment(shopPaymentId);
            lblBankAccount.Text = String.Format("{0}<br>{1}<br>Kwota: {2:C}", ba.ClientName, ba.Title, ba.Amount);
        }

        protected void btnRaportUpload_Click(object sender, EventArgs e)
        {
            string path = ConfigurationManager.AppSettings[String.Format("ProductImportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
             
            HttpFileCollection uploadedFiles = Request.Files;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbe = new StringBuilder();

 
            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];

                try
                {
                    if (userPostedFile.ContentLength > 0)
                    {
                          

                        string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(userPostedFile.FileName));
                        string orginalFileName = System.IO.Path.GetFileName(userPostedFile.FileName);
                        string saveLocation = String.Format(path, fileName);

                        if (saveLocation != null)
                            userPostedFile.SaveAs(saveLocation);

                       // Bll.ProductCatalogHelper.SaveFile(productCatalogIds, saveLocation, fileName, orginalFileName);

                        sb.AppendLine(String.Format("{0}. plik: {1}<br>", i + 1, userPostedFile.FileName));

                        ProcessFile(saveLocation);
                    }
                }
                catch (Exception Ex)
                {
                    sbe.AppendLine(String.Format("{0}. plik: {1}<br>", i + 1, userPostedFile.FileName));
                }
            }
            if (sbe.Length == 0)
                DisplayMessage(String.Format("Zapisano poprawnie<br><br>{0}", sb.ToString()));
            else
                DisplayMessage(String.Format("Błędy<br><br>{0}", sbe.ToString()));

        }

        private void ProcessFile(string saveLocation)
        {
            try
            { 
            switch(Int32.Parse(ddlFile.SelectedValue))
            {
                case 1:
                    LajtIt.Bll.MarketplacePayments.Przelewy24.Process((int)Dal.Helper.Shop.Lajtitpl, UserName, saveLocation); break;
                case 0:
                    LajtIt.Bll.MarketplacePayments.Dpd.Process(0, UserName, saveLocation); break;
                case 6:
                    LajtIt.Bll.MarketplacePayments.Ceneo.Process((int)Dal.Helper.Shop.Ceneo, UserName, saveLocation); break;
                case 21:
                    LajtIt.Bll.MarketplacePayments.Morele.Process((int)Dal.Helper.Shop.Morele, UserName, saveLocation); break;
                case 13:
                    LajtIt.Bll.MarketplacePayments.Polcard.Process((int)Dal.Helper.Shop.Showroom, UserName, saveLocation); break;
            }
            }catch(Exception ex)
            {
                DisplayMessage(ex.Message);
            }
            btnSearch_Click(null, null);
        }
    }
}
 