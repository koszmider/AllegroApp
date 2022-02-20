using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("0650d623-57b0-4dc5-90ad-98772433b51a")]
    public partial class Company : LajtitPage
    {
        public int CompanyId { get { return Convert.ToInt32(Request.QueryString["id"]); } }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
                BindCompany();
        }

        private void BindCompany()
        {
            if (CompanyId == 0)
                return;

            Dal.Company company = Dal.DbHelper.Accounting.GetCompany(CompanyId);



            txbAddress.Text = company.Address;
            txbAddressNo.Text = company.AddressNo;
            txbBandCompanyName.Text = company.BankName;
            if (company.BankAccountNumber != null)
                txbBankAccountNumber.Text = company.BankAccountNumber.Replace(" ", "");
            txbBankAccountNumber2.Text = company.BankNumber;
            txbCity.Text = company.City;
            txbCompanyAddress.Text = company.BankAddress;
            if (company.DpdNumcat.HasValue)
                txbDPDNumcat.Text = company.DpdNumcat.ToString();
            txbName.Text = company.Name;
            txbOwner.Text = company.CompanyOwner;
            txbPostalCode.Text = company.PostalCode;
            txbRegon.Text = company.Regon;
            txbTaxId.Text = company.TaxId;
            chbIsActive.Checked = company.IsActive;
            chbIsMyCompany.Checked = company.IsMyCompany;
            txbKRS.Text = company.KRS;
            txbBDO.Text = company.BDO;

            if (company.PaymentDays.HasValue)
                txbPaymentDays.Text = company.PaymentDays.ToString();
            chbCanSendToBank.Checked = company.CanSendToBank;

        }
        public static bool ValidateBankAccount(string szNR)
        {
            bool bResult = false;

            szNR = szNR.Trim()
                        .Replace(" ", "")
                        .Replace("-", "");

            if (szNR.Length == 26 || szNR.Length == 28)
            {
                string nr = szNR;
                if (nr.Length == 26)
                {
                    nr = (nr + "PL" + nr.Substring(0, 2)).Remove(0, 2);
                }
                else
                {
                    nr = (nr + nr.Substring(0, 4)).Remove(0, 4);
                }

                nr = nr.Replace("P", "25").Replace("L", "21");

                String nr6 = nr.Substring(0, 6);
                String nr12 = nr.Substring(6, 6);
                String nr18 = nr.Substring(12, 6);
                String nr24 = nr.Substring(18, 6);
                String nr30 = nr.Substring(24);

                int r = Convert.ToInt32(nr6) % 97;
                nr12 = (r > 0 ? r.ToString() : "") + nr12;
                r = Convert.ToInt32(nr12) % 97;
                nr18 = (r > 0 ? r.ToString() : "") + nr18;
                r = Convert.ToInt32(nr18) % 97;
                nr24 = (r > 0 ? r.ToString() : "") + nr24;
                r = Convert.ToInt32(nr24) % 97;
                nr30 = (r > 0 ? r.ToString() : "") + nr30;

                bResult = (Convert.ToInt32(nr30) % 97 == 1);
            }
            else return false;

            //if (bResult && retFormated)
            //{
            //    if (szNR.Length == 26)
            //    {
            //        szNR = szNR.Insert(22, " ").Insert(18, " ").Insert(14, " ").Insert(10, " ").Insert(6, " ").Insert(2, " ");
            //    }
            //    else
            //    {
            //        szNR = szNR.Insert(24, " ").Insert(20, " ").Insert(16, " ").Insert(12, " ").Insert(8, " ").Insert(4, " ");
            //    }
            //}

            return bResult;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {

            if(txbBankAccountNumber.Text=="" || !ValidateBankAccount(txbBankAccountNumber.Text))
            {
                DisplayMessage("Numer konta niepoprawny");
                return;
            }

            Dal.Company company = new Dal.Company()
            {
                CompanyId = CompanyId,
                Address = txbAddress.Text.Trim(),
                AddressNo = txbAddressNo.Text.Trim(),
                BankName = txbBandCompanyName.Text.Trim(),
                BankAccountNumber = txbBankAccountNumber.Text.Trim(),
                BankNumber = txbBankAccountNumber2.Text.Trim(),
                City = txbCity.Text.Trim(),
                BankAddress = txbCompanyAddress.Text.Trim(),
                Name = txbName.Text.Trim(),
                CompanyOwner = txbOwner.Text.Trim(),
                PostalCode = txbPostalCode.Text.Trim(),
                Regon = txbRegon.Text.Trim(),
                TaxId = txbTaxId.Text.Trim(),
                IsActive = chbIsActive.Checked,
                IsMyCompany = chbIsMyCompany.Checked,
                BDO = txbBDO.Text,
                KRS = txbKRS.Text,
                CanSendToBank=chbCanSendToBank.Checked
            };
            if(!String.IsNullOrEmpty(txbDPDNumcat.Text.Trim()))
                company.DpdNumcat = Int32.Parse(txbDPDNumcat.Text.Trim());
            if(!String.IsNullOrEmpty(txbPaymentDays.Text.Trim()))
                company.PaymentDays = Int32.Parse(txbPaymentDays.Text.Trim());

            Dal.OrderHelper oh = new Dal.OrderHelper();
            if (CompanyId != 0)
            {
                oh.SetCompany(CompanyId, company, UserName);
                DisplayMessage("Zapisano zmiany");
            }
            else
            {
                int cId=oh.SetCompany(company, UserName);

                DisplayMessage(String.Format("Dodano nową firmę. <a href='Company.aspx?id={0}'>Kliknij tutaj by przejść do jej edycji</a> lub by wrócić <a href='CompanyInfo.aspx'>do wszystkich firm</a>", cId));
            }

        }
 
    }
}