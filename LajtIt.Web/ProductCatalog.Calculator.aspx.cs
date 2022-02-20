using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("8204ab93-34c2-4d80-9edf-7a65f9f0f0d9")]
    public partial class ProductCalculator : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindProductCatalog();
        }
        private void BindProductCatalog()
        { 
            int productCatalogId = Convert.ToInt32(Request.QueryString["id"]);
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.ProductCatalogItemStatsResult stat = oh.GetProductCatalogItemStats(productCatalogId);
            Dal.ProductCatalogView pc = oh.GetProductCatalog(productCatalogId);
     

            if (stat != null)
            {
                txbItemsSoldAllegro.Text = txbItemsSoldShop.Text = stat.AvgQuantityOrdered.Value.ToString("0.00");
            }
            else
                txbItemsSoldAllegro.Text = "0";

           


            txbBrutto.Text = pc.PriceBrutto.ToString();
            txbAdditions.Text = pc.CostAdditions.ToString();
            txbCreateCostAllegro.Text = pc.CostCreateAllegro.ToString();
            txbCreateCostShop.Text = pc.CostCreateShop.ToString();
            txbMaterial.Text = pc.CostMaterial.ToString();
            txbItemsSoldAllegro.Text = pc.CostSellAllegro.ToString();
            txbItemsSoldShop.Text = pc.CostSellShop.ToString();
            txbWork.Text = pc.CostWork.ToString();
  
            Calulcate(null, null);
        } 
        protected void Calulcate(object sender, EventArgs e)
        {
            decimal brutto = Convert.ToDecimal(txbBrutto.Text.Trim());
            decimal netto = brutto / 1.23M;
            decimal material = Convert.ToDecimal(txbMaterial.Text.Trim());
            decimal additions = Convert.ToDecimal(txbAdditions.Text.Trim());
            decimal work = Convert.ToDecimal(txbWork.Text.Trim());
            decimal createAllegro = Convert.ToDecimal(txbCreateCostAllegro.Text.Trim());
            decimal createShop = Convert.ToDecimal(txbCreateCostShop.Text.Trim());
            decimal costAllegro = CalculateAllegroCost(brutto);
            decimal costShop = Bll.ProductCatalogCalculator1.CalculateShopCostNetto(brutto);
            decimal itemsSoldAllegro = Convert.ToDecimal(txbItemsSoldAllegro.Text);
            decimal itemsSoldShop = Convert.ToDecimal(txbItemsSoldShop.Text);

            decimal totalAllegro = GetTotal(material, additions, work, createAllegro, costAllegro, itemsSoldAllegro);
            decimal totalShop = GetTotal(material, additions, work, createShop, costShop, itemsSoldShop);

            lblNetto.Text = String.Format("{0:C}", netto);
            lblCommissionAllegro.Text = String.Format("{0:C}", costAllegro);
            lblCommissionShop.Text = String.Format("{0:C}", costShop);

            lblTotalAllegro.Text = String.Format("{0:C}", totalAllegro);
            lblTotalShop.Text = String.Format("{0:C}", totalShop);

            lblTotalIncomeAllegro.Text = String.Format("{0:C}", netto - totalAllegro);
            lblTotalIncomeShop.Text = String.Format("{0:C}", netto - totalShop);
            if(netto!=0)
            { 
            lblTotalIncomePercAllegro.Text = String.Format("{0:00.00}%", (netto - totalAllegro) * 100.0M / netto);
            lblTotalIncomePercShop.Text = String.Format("{0:00.00}%", (netto - totalShop) * 100.0M / netto);
        }
        }

        private decimal GetTotal(decimal material, decimal additions, decimal work, decimal create, decimal cost, decimal itemsSold)
        {
            itemsSold = itemsSold == 0 ? 1 : itemsSold;
            return material + additions + work + create / itemsSold + cost;
        }

        private decimal CalculateAllegroCost(decimal brutto)
        {
            decimal total = 0;
             
            if (brutto <= 5000)
                total = (brutto - 1000) * 0.015M + 33;
            else
                total = (brutto - 5000) * 0.005M + 93;
            if (brutto <= 1000)
                total = (brutto - 100) * 0.03M + 6;
            if (brutto <= 100)
                total = brutto * 0.06M;


            return total / 1.23M;
        }

      
        //public static decimal CalculateShopCostBrutto(decimal netto)
        //{
        //    return CalculateShopCostNetto(netto * 1.23M);// *1.23M;
        //}
        protected void btnSaveCalculation_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalog pc = new Dal.ProductCatalog()
            {
                ProductCatalogId = Convert.ToInt32(Request.QueryString["id"]), 
                CostAdditions = Convert.ToDecimal(txbAdditions.Text.Trim()),
                CostCreateAllegro = Convert.ToDecimal(txbCreateCostAllegro.Text.Trim()),
                CostCreateShop = Convert.ToDecimal(txbCreateCostShop.Text.Trim()),
                CostMaterial = Convert.ToDecimal(txbMaterial.Text.Trim()),
                CostSellAllegro = Convert.ToDecimal(txbItemsSoldAllegro.Text.Trim()),
                CostSellShop = Convert.ToDecimal(txbItemsSoldShop.Text.Trim()),
                CostWork = Convert.ToDecimal(txbWork.Text.Trim())
            };

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetProductCatalogCosts(pc);
            DisplayMessage("Zapisano");
        }
    }
}