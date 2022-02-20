using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("82b8da09-9227-4edb-bf05-17b93298556f")]
    public partial class Costs : LajtitPage
    {

        bool hasFullView = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            hasFullView = this.HasActionAccess(Guid.Parse("62adcf07-4f5a-4adb-8cb8-60842f9afc1c"));

            ucCostsControl.Reloaded += Reloaded;
            //ucCostControl.Canceled += CostCanceled;
            ucCostsControl.Reloaded += CostsReloaded;

            if (!Page.IsPostBack)
            {
                BindMonths();
                BindCompanies();

                BindCostTypes();
                ddlCostTypeSearch.Items.Insert(0, new ListItem());
                BindBatches();
                BindCosts();
            }
            
        }
        public void BindCostTypes()
        {
            ddlCostTypeSearch.DataSource = Dal.DbHelper.Accounting.GetCostTypes(UserName);
            ddlCostTypeSearch.DataBind();
        }
        private void BindBatches()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            ddlBatch.DataSource = oh.GetCostsBatches()
                .Select(x=>new
                {
                    x.BatchId,
                    x.BatchDate
                }
                ).Distinct()
                .Take(10)
                .ToList();
            ddlBatch.DataBind();

            ddlBatch.Items.Insert(0, new ListItem("-- ostatnie paczki przelewów --"));
        }

        private void BindCompanies()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            var c = oh.GetCompanies();
            ddlCompany.DataSource = c;
            ddlCompany.DataBind();
            ddlCompanyOwner.DataSource = c.Where(x => x.IsMyCompany && x.IsActive).ToList();
            ddlCompanyOwner.DataBind();
        }
        protected void Reloaded(object sender, EventArgs e)
        {

          //  gvCosts.EditIndex = -1;
          //  gvCosts.PageIndex = 0;
            BindCosts();
            //pCostNew.Visible = false;
            //lbtnCostNew.Visible = true;

        }
        protected void CostCanceled(object sender, EventArgs e)
        {

          //  pCostNew.Visible = false;
           // lbtnCostNew.Visible = true;

        }
        private void CostsReloaded(object sender, EventArgs e)
        {
            BindCosts();
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

            items.Insert(0, new ListItem());

            if (hasFullView)
                ddlMonth.Items.AddRange(items.ToArray());
            else
                ddlMonth.Items.AddRange(items.Take(3).ToArray());
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindCosts();
        }
        protected void lbtnCostNew_Click(object sender, EventArgs e)
        {
          //  pCostNew.Visible = true;
            //lbtnCostNew.Visible = false;
        }
        private void BindCosts()
        {
            List<Dal.Cost> costs = Dal.DbHelper.Accounting.GetCosts();

            if (ddlBatch.SelectedIndex != 0)
            {
                costs = costs.Where(x => x.BatchId == Guid.Parse(ddlBatch.SelectedValue)).ToList();
                ucCostsControl.BindCosts(costs, HasActionAccess(Guid.Parse("7749528b-10e4-4688-bd31-15fb31bf78f5")));
                return;
            }

            if (ddlCompanyOwner.SelectedIndex != 0)
            {
                costs = costs.Where(x => x.CompanyOwnerId == Int32.Parse(ddlCompanyOwner.SelectedValue)).ToList();
            }
            if (ddlMonth.SelectedIndex != 0)
            {
                DateTime date = DateTime.Parse(ddlMonth.SelectedValue);
                costs = costs.Where(x => x.Date.Year == date.Year && x.Date.Month == date.Month).ToList();
            }
            if (chbIsForAccouting.Checked)
                costs = costs.Where(x => x.IsChecked.HasValue==false|| x.IsChecked == false).ToList();

            if (ddlCostTypeSearch.SelectedIndex != 0)
            {
                costs = costs.Where(x => x.CostTypeId == Convert.ToInt32(ddlCostTypeSearch.SelectedValue)).ToList();
            }
            if (ddlCompany.SelectedIndex != 0)
            {
                costs = costs.Where(x => x.CompanyId == Convert.ToInt32(ddlCompany.SelectedValue)).ToList();
            }
            if(txbComment.Text!="")
            {
                costs = costs.Where(x => x.Comment.ToLower().Contains(txbComment.Text.Trim().ToLower())).ToList();

            }
            if (chbToPay.Checked)
                costs = costs.Where(x => x.ToPay == true).ToList();



            switch(ddlCostDocumentType.SelectedIndex)
            {
                case 1:
                    costs = costs.Where(x => x.CostDocumentTypeId == 1).ToList(); break;
                case 2:
                    costs = costs.Where(x => x.CostDocumentTypeId == 2).ToList(); break;
                case 3:
                    costs = costs.Where(x => x.CostDocumentTypeId == 2 && x.InvoiceCorrectionPaid.HasValue && x.InvoiceCorrectionPaid.Value).ToList(); break;
                case 4:
                    costs = costs.Where(x => x.CostDocumentTypeId == 2 && (!x.InvoiceCorrectionPaid.HasValue || !x.InvoiceCorrectionPaid.Value)).ToList(); break;
            }

            ucCostsControl.BindCosts(costs, HasActionAccess(Guid.Parse("7749528b-10e4-4688-bd31-15fb31bf78f5")));


            if (ddlCostTypeSearch.SelectedIndex != 0 && ddlMonth.SelectedIndex == 0)
            {
                pChart.Visible = true;
                Chart1.Series["Series1"].XValueMember = "Date";
                Chart1.Series["Series1"].YValueMembers = "Total Amount";
                Chart1.Series["Series1"].IsValueShownAsLabel = true;

                List<Dal.CostsView> q = Dal.DbHelper.Accounting.GetCostsStats().Where(x => x.CostTypeId == Convert.ToInt32(ddlCostTypeSearch.SelectedValue)).OrderBy(x => x.Date).ToList();
                if (rbtnlMonths.SelectedIndex != 0)
                    q = q.OrderByDescending(x => x.Date).Take(Convert.ToInt32(rbtnlMonths.SelectedValue)).OrderBy(x => x.Date).ToList();



                foreach (Dal.CostsView c in q)
                {
                    Chart1.Series[0].Points.AddXY(c.Date, c.TotalAmount);
                }
            }
            else

                pChart.Visible = false;

        }

        protected void lbtnElixir_Click(object sender, EventArgs e)
        {
            int[] costIds = ucCostsControl.GetSelectedIds();

            if(costIds.Length==0)
            {
                DisplayMessage("Wybierz koszty do utworzenia pliku z przelewami");
                return;
            }
            if(ddlCompanyOwner.SelectedIndex==0)
            {
                DisplayMessage("Wybierz firmę, dla której chcesz wyeksportować paczkę przelewów");
                return;
            }
            Bll.ElixirHelper eh = new Bll.ElixirHelper();
            string fileName = null;

            bool isReady = eh.GetFile(costIds, Int32.Parse(ddlCompanyOwner.SelectedValue), out fileName);

            if(!isReady)
            {
                DisplayMessage("Błąd generowania. Możliwe przyczyny:<ul><li>Można generować plik tylko dla jednej firmy na raz</li><li>Niektóre koszty mają niezdefiniowaną konfigurację przelewów. Przejrzyj wynik wyszukiwania i sprawdź koszty bez konfiguracji (na czerwono)</li></ul>");
                return;

            }


            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetCostToPay(costIds, false);
            BindBatches();

            ddlBatch.SelectedIndex = 1;
            BindCosts();

            string contentType =   "Application/xml";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", String.Format("attachment; filename=przelewy_{0:yyyyMMddmm:HH:ss}.txt", DateTime.Now));

            Response.WriteFile(fileName);
            Response.End();
        }
    }
}