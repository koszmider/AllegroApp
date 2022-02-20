using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class CostControl : LajtitControl
    {
        public delegate void SavedEventHandler(object sender, EventArgs e);
        public event SavedEventHandler Saved;
        public delegate void CanceledEventHandler(object sender, EventArgs e);
        public event CanceledEventHandler Canceled;
        public delegate void ReloadedEventHandler(object sender, EventArgs e);
        public event ReloadedEventHandler Reloaded;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                txbDate.Text = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                
                BindCostTypes(ddlCostType);
           
            }
            else
            {
                if (txbDate.Text != "")
                    calDate.SelectedDate = DateTime.Parse(txbDate.Text);
                if (txbPaidDate.Text != "")
                    calPaidDate.SelectedDate = DateTime.Parse(txbPaidDate.Text);

            }
        }

        public void PreselectCostType(int costId)
        {
            ddlCostType.SelectedIndex = ddlCostType.Items.IndexOf(ddlCostType.Items.FindByValue(costId.ToString()));
            ddlCostType.Enabled = false;
        }

       

        protected void lbtnCostCancel_Click(object sender, EventArgs e)
        {
            if (Canceled != null)
                Canceled(this, e);
        }
        public void BindCostTypes(DropDownList ddlCostType)
        {
            ddlCostType.DataSource = Dal.DbHelper.Accounting.GetCostTypes(UserName);
            ddlCostType.DataBind();
        }

        //public void BindCompanies()
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    ddlCompany.DataSource = oh.GetCompanies();
        //    ddlCompany.DataBind();
        //}

        protected void btnCostAdd_Click(object sender, EventArgs e)
        {

            if(!Page.IsValid)
            {
                DisplayMessage("Uzupełnij wybrane pola");
                return;
            }    


            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Cost cost = new Dal.Cost()
            {
                Amount = Decimal.Parse(txbAmount.Text.Trim()),
                Comment = txbComment.Text.Trim(),
                Date = calDate.SelectedDate.Value,// DateTime.Parse(txbDate.Text.Trim()),
                InsertUser = UserName,
                VAT = Decimal.Parse(ddlVAT.SelectedValue) / 100M,
                CostTypeId = Int32.Parse(ddlCostType.SelectedValue),
                InvoiceNumber = txbInvoiceNumber.Text.Trim(),
                IsForAccounting = true,//cbhIsForAccountingAdd.Checked,
                CompanyId = Convert.ToInt32(hfCompanyId.Value),
                CompanyOwnerId = Int32.Parse(rblCompanyFor.SelectedValue),
                InsertDate = DateTime.Now,
                CostDocumentTypeId = Int32.Parse(rblCostDocumentType.SelectedValue),
                InvoiceCorrectionPaid = chbInvoiceCorrectionPaid.Checked
            };

            if (txbOrder.Text != "")
                cost.OrderId = Int32.Parse(txbOrder.Text);

            if (rblCostDocumentType.SelectedIndex==1 && hfInvoiceCorrection.Value!="")
            {
                cost.CostRefId = Int32.Parse(hfInvoiceCorrection.Value);
            }
            if (ViewState["CostId"] != null)
                cost.CostId = Int32.Parse(ViewState["CostId"].ToString());

            if (cost.CompanyId!=0 && cost.PaidDate.HasValue == false)
            {
                Dal.Company company = Dal.DbHelper.Accounting.GetCompany(cost.CompanyId);
                if(company.PaymentDays.HasValue == false)
                {
                    DisplayMessage(String.Format("Uzupełnij datę płatności lub skonfiguruj domyślny termin płatności dla <a href='/Company.aspx?id={0}' target='_blank'>{1}</a>", company.CompanyId, company.Name));
                    return;
                }

            }


            if (!String.IsNullOrEmpty(txbPaidDate.Text))
                cost.PaidDate = calPaidDate.SelectedDate;// DateTime.Parse(txbPaidDate.Text.Trim());

            if (chbToPay.Enabled && chbToPay.Checked)
            {
                cost.ToPay = true;

                if (cost.CompanyId == 0 || !oh.GetCompanyReadyForPayment(cost.CompanyId))
                    DisplayMessage("Koszt został dodany.<br><br><br>Uwaga! Firma nie została wybrana lub nie posiada aktywnej konfiguracji konta.");
            }

            int costId = Dal.DbHelper.Accounting.SetCost(cost);
            switch (costId)
            {
                case -3: DisplayMessage(String.Format("Zamówienie o numerze {0} nie istnieje", txbOrder.Text)); break;
                case -2: DisplayMessage("Typ dokumentu wymaga odpowiedniej wartości (ujemna dla korekty, dodatnia dla faktury)"); break;
                case -1: DisplayMessage("Podany numer faktury już istnieje dla tej firmy"); break;
                case 0: DisplayMessage(String.Format("Błąd zapisu. Sprawdź poprawność danych.")); break;
                default:
                    if (ImportId != null)
                        SetImportCost(costId);

                    if (Saved != null)
                        Saved(this, e);
                    rblCompanyFor.SelectedIndex = 0;
                    break;
            }



        }

        private void SetImportCost(int costId)
        {
            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();
            h.SetImportCost(costId, ImportId.Value);
        }

        public int? ImportId
        {
            get
            {

                if (ViewState["ImportId"] != null)
                    return Convert.ToInt32(ViewState["ImportId"]);
                else
                    return null;

            }
            set { ViewState["ImportId"] = value; }
        }

        protected void rblCostDocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetControls();

            if (Reloaded != null)
                Reloaded(this, e);
        }

        private void SetControls()
        {
            switch (rblCostDocumentType.SelectedValue)
            {
                case "1":
                    chbToPay.Enabled = true;
                    rfvAmout.ValidationExpression = @"^[0-9]+(\,[0-9]{1,2})?$";
                    rfvAmout.Text = "oczekiwana wartość dodatnia";
                    lblInvoiceCorrection.Enabled = false;
                    txbInvoiceCorrectionNumber.Enabled = false;
                    rfvInvoiceCorrectionNumber.Enabled = false;
                    rfvInvoiceNumber.Enabled = false;
                    chbInvoiceCorrectionPaid.Enabled = false;
                    break;
                case "2":
                    chbToPay.Enabled = false;
                    rfvAmout.ValidationExpression = @"^\-[0-9]+(\,[0-9]{1,2})?$";
                    rfvAmout.Text = "oczekiwana wartość ujemna";
                    lblInvoiceCorrection.Enabled = true;
                    txbInvoiceCorrectionNumber.Enabled = true;
                    rfvInvoiceCorrectionNumber.Enabled = true;
                    rfvInvoiceNumber.Enabled = true;
                    chbInvoiceCorrectionPaid.Enabled = true;
                    break;
            }
        }

        internal void SetCost(int costId)
        {
            Dal.Cost cost = Dal.DbHelper.Accounting.GetCost(costId);

            rblCompanyFor.SelectedValue = cost.CompanyOwnerId.ToString();
            rblCostDocumentType.SelectedValue = cost.CostDocumentTypeId.ToString();
            hfCompanyId.Value = cost.CompanyId.ToString();
            txbCompany.Text = cost.Company.Name;
            if (cost.OrderId.HasValue)
                txbOrder.Text = cost.OrderId.ToString();

            ddlCostType.SelectedValue = cost.CostTypeId.ToString();
            calDate.SelectedDate = cost.Date;
            if (cost.PaidDate.HasValue)
                calPaidDate.SelectedDate = cost.PaidDate.Value;

            txbComment.Text = cost.Comment;
            txbInvoiceNumber.Text = cost.InvoiceNumber;
            ddlVAT.SelectedValue = cost.VAT.ToString();
            chbInvoiceCorrectionPaid.Checked = cost.InvoiceCorrectionPaid.HasValue && cost.InvoiceCorrectionPaid.Value;

            if (cost.ToPay.HasValue)
                chbToPay.Checked = cost.ToPay.Value;

            txbAmount.Text = cost.Amount.ToString();

            if(cost.CostRefId.HasValue)
            {
                hfInvoiceCorrection.Value = cost.CostRefId.ToString() ;
                Dal.Cost c = Dal.DbHelper.Accounting.GetCost(cost.CostRefId.Value);
                txbInvoiceCorrectionNumber.Text = c.InvoiceNumber;
            }
            else
            {
                hfInvoiceCorrection.Value = null;
                txbInvoiceCorrectionNumber.Text = null;
            }
            ViewState["CostId"] = costId.ToString();
            SetControls();
            rblCostDocumentType.Enabled = false;
        }

        internal void SetCost()
        {
            rblCompanyFor.SelectedIndex = 0;
            rblCostDocumentType.SelectedIndex = 0;
            hfCompanyId.Value = null;
            txbCompany.Text = null;
            //ddlCostType.SelectedIndex = 0;
            calDate.SelectedDate = DateTime.Now;
            calPaidDate.SelectedDate = null;

            txbComment.Text = null;
            txbInvoiceNumber.Text = null;
            ddlVAT.SelectedIndex = 0;
            txbAmount.Text = "0";
            chbToPay.Checked = false;
            chbToPay.Enabled = true;
            ViewState["CostId"] = null;
            hfInvoiceCorrection.Value = null;
            txbInvoiceCorrectionNumber.Text = null;
            rblCostDocumentType.Enabled = true;
            chbInvoiceCorrectionPaid.Checked = false;
            txbPaidDate.Text = "";
            calPaidDate.SelectedDate = null;
            SetControls();
        }
    }
}