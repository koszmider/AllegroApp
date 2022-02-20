using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class CostsControl : LajtitControl
    {

        private decimal sumNetto = 0;
        private decimal sumBrutto = 0;
        public delegate void ReloadedEventHandler(object sender, EventArgs e);
        public event ReloadedEventHandler Reloaded;

        public int? PreselectedCostId
        {
            get
            {

                if (ViewState["CostId"] != null)
                    return Convert.ToInt32(ViewState["CostId"]);
                else
                    return null;

            }
            set { ViewState["CostId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucCostControl.Saved += Saved;
            ucCostControl.Reloaded += Reloaded1;
        }

        private void Reloaded1(object sender, EventArgs e)
        {
            mpeCostReceipt.Show();
        }
        private void Saved(object sender, EventArgs e)
        {

            if (Reloaded != null)
                Reloaded(this, e);
        }

        protected void lbtnChecked_Click(object sender, EventArgs e)
        {
            int costId = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetCostChecked(costId);

            if (Reloaded != null)
                Reloaded(null, null);
        } 
        internal void BindCosts(List<Dal.Cost> costs, bool hasAdminAccess)
        {

            gvCosts.Columns[gvCosts.Columns.Count - 1].Visible = hasAdminAccess;

            int[] costTypesForRoles = Dal.DbHelper.Accounting.GetCostTypesForUser(UserName);

            bool fullView = HasActionAccess(Guid.Parse("7749528b-10e4-4688-bd31-15fb31bf78f5"));
       


            if (!fullView)
                costs = costs.Where(x => x.InsertDate > DateTime.Now.AddMonths(-2)).ToList();


            gvCosts.DataSource = costs.Where(x=> costTypesForRoles.Contains(x.CostTypeId)).ToList();
            gvCosts.DataBind();
        }
        protected void gvCosts_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCosts.EditIndex = e.NewEditIndex;
            if (Reloaded != null)
                Reloaded(null, null);

        }
        protected void gvCosts_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCosts.EditIndex = -1;
            if (Reloaded != null)
                Reloaded(null, null);

        }
        protected void gvCosts_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvCosts.Rows[e.RowIndex];

            int costId = Convert.ToInt32(gvCosts.DataKeys[e.RowIndex][0]);

            TextBox txbDate = row.FindControl("txbDate") as TextBox;
            TextBox txbPaidDate = row.FindControl("txbPaidDate") as TextBox;
            TextBox txbVAT = row.FindControl("txbVAT") as TextBox;
            TextBox txbAmount = row.FindControl("txbAmount") as TextBox;
            TextBox txbComment = row.FindControl("txbComment") as TextBox;
            TextBox txbInvoiceNumber = row.FindControl("txbInvoiceNumber") as TextBox;
            DropDownList ddlCostType = row.FindControl("ddlCostType") as DropDownList;
            DropDownList ddlCompany = row.FindControl("ddlCompany") as DropDownList;
            DropDownList ddlCompanyOwner = row.FindControl("ddlCompanyOwner") as DropDownList;
            RadioButton rbToPay1 = row.FindControl("rbToPay1") as RadioButton;
            RadioButton rbToPay0 = row.FindControl("rbToPay0") as RadioButton;

            Dal.Cost cost = new Dal.Cost()
            {
                CostId = costId,
                Amount = Decimal.Parse(txbAmount.Text.Trim()),
                Comment = txbComment.Text.Trim(),
                Date = DateTime.Parse(txbDate.Text.Trim()),
                InsertUser = UserName,
                VAT = Decimal.Parse(txbVAT.Text.Trim()) / 100M,
                CostTypeId = Int32.Parse(ddlCostType.SelectedValue),
                InvoiceNumber = txbInvoiceNumber.Text,
                CompanyId = Convert.ToInt32(ddlCompany.SelectedValue),
                CompanyOwnerId = Convert.ToInt32(ddlCompanyOwner.SelectedValue),
                ToPay = rbToPay1.Checked
            };

            if (!String.IsNullOrEmpty(txbPaidDate.Text))
                cost.PaidDate = DateTime.Parse(txbPaidDate.Text.Trim());


            
            int costUpdateId =             Dal.DbHelper.Accounting.SetCost(cost);
            switch(costUpdateId)
            {
                case -1: DisplayMessage("Podany numer faktury już istnieje dla tej firmy"); break;
                case 0: DisplayMessage(String.Format("Błąd zapisu. Sprawdź poprawność danych."));break;
                    default:
                    gvCosts.EditIndex = -1;
                    if (Reloaded != null)
                        Reloaded(null, null);

                    break;
            }

        }

        protected void gvCosts_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCosts.PageIndex = e.NewPageIndex;
            if (Reloaded != null)
                Reloaded(null, null);
        }
        protected void gvCost_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblVATValue = e.Row.FindControl("lblVATValue") as Label;
            Label lblAmountNetto = e.Row.FindControl("lblAmountNetto") as Label;
            Image imgBatch = e.Row.FindControl("imgBatch") as Image;

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblAmountBrutto = e.Row.FindControl("lblAmountBrutto") as Label;
                lblAmountNetto.Text = String.Format("{0:C}", sumNetto);
                lblAmountBrutto.Text = String.Format("{0:C}", sumBrutto);
                lblVATValue.Text = String.Format("{0:C}", sumBrutto - sumNetto);
                sumNetto = sumBrutto = 0;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Dal.Cost cost = e.Row.DataItem as Dal.Cost;
                sumNetto += cost.Amount / (1 + cost.VAT);
                sumBrutto += cost.Amount;


                if (cost.CostDocumentTypeId == 2)
                { 
                    if (cost.InvoiceCorrectionPaid.HasValue && cost.InvoiceCorrectionPaid.Value)
                        e.Row.ForeColor = System.Drawing.Color.Green;
                    else
                    {

                        e.Row.ForeColor = System.Drawing.Color.Red;
                    }

                    CheckBox chbOrder = e.Row.FindControl("chbOrder") as CheckBox;
                    chbOrder.Visible = false;
                }
                Button btnEdit = e.Row.FindControl("btnEdit") as Button;

                btnEdit.CommandArgument = cost.CostId.ToString();


                lblVATValue.Text = String.Format("{0:C}", cost.Amount - cost.Amount / (1 + cost.VAT));
                lblAmountNetto.Text = String.Format("{0:C}", cost.CostNetto);

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlCostType = e.Row.FindControl("ddlCostType") as DropDownList;
                    DropDownList ddlCompany = e.Row.FindControl("ddlCompany") as DropDownList;
                    DropDownList ddlCompanyOwner = e.Row.FindControl("ddlCompanyOwner") as DropDownList;
                    BindCostTypes(ddlCostType);
                    BindCompanies(ddlCompany, false);
                    BindCompanies(ddlCompanyOwner, true);
                    TextBox txbDate = e.Row.FindControl("txbDate") as TextBox;
                    TextBox txbVat = e.Row.FindControl("txbVat") as TextBox;
                    TextBox txbAmount = e.Row.FindControl("txbAmount") as TextBox;
                    TextBox txbComment = e.Row.FindControl("txbComment") as TextBox;
                    TextBox txbPaidDate = e.Row.FindControl("txbPaidDate") as TextBox;
                    TextBox txbInvoiceNumber = e.Row.FindControl("txbInvoiceNumber") as TextBox;
                    RadioButton rbToPay1 = e.Row.FindControl("rbToPay1") as RadioButton;
                    RadioButton rbToPay0 = e.Row.FindControl("rbToPay0") as RadioButton;

                    txbDate.Text = String.Format("{0:yyyy-MM-dd}", cost.Date);
                    txbPaidDate.Text = String.Format("{0:yyyy-MM-dd}", cost.PaidDate);
                    txbVat.Text = String.Format("{0:0.##}", cost.VAT * 100);
                    txbAmount.Text = String.Format("{0}", cost.Amount);
                    txbComment.Text = cost.Comment;
                    ddlCostType.SelectedValue = cost.CostTypeId.ToString();
                    ddlCompany.SelectedValue = cost.CompanyId.ToString();
                    ddlCompanyOwner.SelectedValue = cost.CompanyOwnerId.ToString();
                    txbInvoiceNumber.Text = cost.InvoiceNumber;
                    rbToPay1.Checked = cost.ToPay.HasValue && cost.ToPay.Value;
                    rbToPay0.Checked = !rbToPay1.Checked;
                }
                else
                {
                    HyperLink hlCompany = e.Row.FindControl("hlCompany") as HyperLink;
                    Label lblCostType = e.Row.FindControl("lblCostType") as Label;
                    Label lblCompanyOwner = e.Row.FindControl("lblCompanyOwner") as Label;
                    Label lblDate = e.Row.FindControl("lblDate") as Label;
                    Label lblVat = e.Row.FindControl("lblVat") as Label;
                    Label lblAmount = e.Row.FindControl("lblAmount") as Label;
                    Label lblComment = e.Row.FindControl("lblComment") as Label;
                    Label lblPaidDate = e.Row.FindControl("lblPaidDate") as Label;
                    Label lblInvoiceNumber = e.Row.FindControl("lblInvoiceNumber") as Label;
                    CheckBox chbIsChecked = e.Row.FindControl("chbIsChecked") as CheckBox;

                    lblCompanyOwner.Text = cost.Company1.Name;
                    lblDate.Text = String.Format("{0:yy-MM-dd}", cost.Date);
                    lblPaidDate.Text = String.Format("{0:yy-MM-dd}", cost.PaidDate);
                    lblVat.Text = String.Format("{0:0.##}%", cost.VAT * 100);
                    lblAmount.Text = String.Format("{0:C}", cost.Amount);
                    lblComment.Text = cost.Comment;
                    lblCostType.Text = cost.CostType.Name;
                    lblInvoiceNumber.Text = cost.InvoiceNumber;
                    hlCompany.Text = cost.Company.Name;
                    chbIsChecked.Checked = cost.IsChecked.HasValue && cost.IsChecked.Value;

                    if (cost.Company.CompanyId == 0)
                        hlCompany.NavigateUrl = "#";
                    else
                        hlCompany.NavigateUrl = String.Format(hlCompany.NavigateUrl, cost.CompanyId);


                    if (cost.Company.IsReadyForPayment == false)
                        hlCompany.ForeColor = System.Drawing.Color.Red;

                    imgBatch.Visible = cost.ToPay.HasValue && cost.ToPay.Value;

                }

            }
        }

        internal int[] GetSelectedIds()
        {
            return WebHelper.GetSelectedIds<int>(gvCosts, "chbOrder");
        }

        private void BindCompanies(DropDownList ddlCompany, bool isMyCompany)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            if (isMyCompany)
                ddlCompany.DataSource = oh.GetCompanies().Where(x => x.IsMyCompany).ToList();
            else
                ddlCompany.DataSource = oh.GetCompanies();
            ddlCompany.DataBind();
        }
        public void BindCostTypes(DropDownList ddlCostType)
        { 
            ddlCostType.DataSource = Dal.DbHelper.Accounting.GetCostTypes(UserName);
            ddlCostType.DataBind();

            if (PreselectedCostId.HasValue)
            {

                ddlCostType.SelectedIndex = ddlCostType.Items.IndexOf(ddlCostType.Items.FindByValue(PreselectedCostId.ToString()));
                ddlCostType.Enabled = false;
            }

        }

        internal void PreselectCostType(int costId)
        {
            PreselectedCostId = costId;
        }

        protected void btnSaveCost_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

     
            protected void btnEdit_Click(object sender, EventArgs e)
            {
                int costId = Int32.Parse(((Button)sender).CommandArgument);


            ucCostControl.SetCost(costId);


                
                mpeCostReceipt.Show();

         
        }

        protected void lbtnCostNew_Click(object sender, EventArgs e)
        {
            mpeCostReceipt.Show();
            ucCostControl.SetCost();
        }
    }
}