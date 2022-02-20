using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("5dd34613-452f-4a99-a0fa-1abfbaae2103")]
    public partial class ProductImport : LajtitPage
    {
        private int ImportId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucCostsControl.Reloaded += CostsReloaded;

            ucCostControl.Saved += CostAdded;
            ucCostControl.Canceled += CostCanceled;
            if (!Page.IsPostBack)
            {
                BindImport();
            }
        }
        protected void CostAdded(object sender, EventArgs e)
        {

            //  gvCosts.EditIndex = -1;
            //  gvCosts.PageIndex = 0;
            BindCosts();
            pCostNew.Visible = false;
            lbtnCostNew.Visible = true;

        }
        protected void CostCanceled(object sender, EventArgs e)
        {

            pCostNew.Visible = false;
            lbtnCostNew.Visible = true;

        }
        protected void lbtnCostNew_Click(object sender, EventArgs e)
        {
            pCostNew.Visible = true;
            lbtnCostNew.Visible = false;
            ucCostControl.PreselectCostType((int)Dal.Helper.CostType.ImportCost);
        }
        protected void CostsReloaded(object sender, EventArgs e)
        {
            BindCosts();
        }

        private void BindCosts()
        {
            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();
            List<Dal.Cost> costs = h.GetImportCosts(ImportId);
            ucCostsControl.BindCosts(costs, true);
            ucCostsControl.PreselectCostType((int)Dal.Helper.CostType.ImportCost);
            ucCostControl.ImportId = ImportId;
        }

        private void BindImport()
        {
            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();

            Dal.ProductCatalogImport import = h.GetImport(ImportId);
            Dal.ProductImportStatResult importStat = h.GetImportStat(ImportId);
            litComment.Text = import.Comment;
            litDate.Text = import.ImportDate.ToString("yyyy/MM/dd");
            litName.Text = import.Name;

            litTotalQuantityOrdered.Text = String.Format("{0}", importStat.TotalQuantityOrdered);
            litTotalQuantitySell.Text = String.Format("{0}", importStat.TotalQuantitySell);
            litTotalQuantitySellPerc.Text = String.Format("{0:0.00}", importStat.SellQuantityPerc);
            litTotalSell.Text = String.Format("{0:C}", importStat.TotalSellNetto);
            litTotalSellPerc.Text = String.Format("{0:0.00}", importStat.SellValuePerc);
            litCost.Text = String.Format("{0:C}", importStat.TotalCostNetto);

            litAllegroCost.Text = String.Format("{0:C}", importStat.AllegroCostNetto);
            BindCosts();
        }
    }
}