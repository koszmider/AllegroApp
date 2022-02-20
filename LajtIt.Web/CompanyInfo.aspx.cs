using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("cc0ed97a-0ca2-4f0c-acb7-6630842b3bf0")]
    public partial class CompanyInfo : LajtitPage
    {
        private bool hasAdminAccess = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            hasAdminAccess = HasActionAccess(Guid.Parse("4fd0c686-3ff3-4aba-a473-4acdb60ef624"));

            pnAdmin.Visible = hasAdminAccess;


            if (!Page.IsPostBack)
                BindCompanies();
        }

        private void BindCompanies()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.Company> companies;

            if (!hasAdminAccess)
                companies = oh.GetCompanies().Where(x => x.IsMyCompany && x.IsActive).ToList();
            else
            {
                companies = oh.GetCompanies(txbSearch.Text).Where(x => x.CompanyId > 0 && x.IsActive).ToList();
            }
            if (chbIsMyCompany.Checked)
                companies = companies.Where(x => x.IsMyCompany == true).ToList();

            gvCompany.DataSource = companies;
            gvCompany.DataBind();

            lbtnClear.Visible = txbSearch.Text.Trim() != "";
        }

        protected void gvCompany_RowDataBound(object sender, GridViewRowEventArgs g)
        {
            if (g.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.Company company = g.Row.DataItem as Dal.Company;

                TextBox txbCompanyInfo = g.Row.FindControl("txbCompanyInfo") as TextBox;

                if (company.IsMyCompany)

                    txbCompanyInfo.Text = String.Format("{0}\r\n{1} {2}\r\n{3} {4}\r\nNIP: {5}\r\nKRS: {6}\r\nBDO: {7}", company.Name, company.Address, company.AddressNo, company.PostalCode,
                        company.City, company.TaxId, company.KRS, company.BDO);
                else
                {
                    txbCompanyInfo.Rows = 1;
                    txbCompanyInfo.Text = String.Format("{0}", company.Name);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindCompanies();
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            txbSearch.Text = "";
            BindCompanies();
        }
    }
}