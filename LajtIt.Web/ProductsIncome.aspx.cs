using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("4a3318e6-cc6c-403e-90b8-3f1062b18354")]
    public partial class ProductsIncome : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                calDateFrom.SelectedDate = DateTime.Now.AddDays(-3);
                calDateTo.SelectedDate = DateTime.Now;
                 
                lbxSuppliers.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderBy(x=>x.Name).ToList();
                lbxSuppliers.DataBind();
            }
            else
            {
                if (txbDateFrom.Text != "")
                    calDateFrom.SelectedDate = DateTime.Parse(txbDateFrom.Text);
                if (txbDateTo.Text != "")
                    calDateTo.SelectedDate = DateTime.Parse(txbDateTo.Text);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindProducts("Marza", "ASC");
        }

        private void BindProducts(string sortingExpression, string sortingDirection)
        { 
            int[] supplierIds = lbxSuppliers.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();


    

            Dal.DbHelper.Orders.OrdersIncomeSearch pcs = new Dal.DbHelper.Orders.OrdersIncomeSearch()
            { 
                SupplierIds = supplierIds,
                DateFrom = calDateFrom.SelectedDate.Value,
                DateTo = calDateTo.SelectedDate.Value,
                MarzaFrom = GetDecimalValue(txbMarzaFrom.Text),
                MarzaTo = GetDecimalValue(txbMarzaTo.Text),
                NarzutFrom = GetDecimalValue(txbNarzutFrom.Text),
                NarzutTo = GetDecimalValue(txbNarzutTo.Text),
       
            };


            List<Dal.ProductsIncomeFnResult> products = Dal.DbHelper.Orders.GetProductsIncomeSearch(pcs);

            if (products.Count() > 0)
            {
                lblMarza.Text =  String.Format("{0:0.00}%", products.Sum(x => x.Marza) / products.Count());
                lblNarzut.Text = String.Format("{0:0.00}%", products.Sum(x => x.Narzut) / products.Count());
                lblProfit.Text = String.Format("{0:C}", products.Sum(x => x.SellPrice - x.PriceFromDelivery ) );
                pnResults.Visible = true;
            }
            else
            {
                pnResults.Visible = false;
            }
            switch (sortingDirection)
            {
                case "ASC": sortingDirection = "DESC";break;
                default: sortingDirection = "ASC"; break;

            }
            ViewState["sortingDirection"] = sortingDirection;


            if (sortingDirection == "ASC")
                switch (sortingExpression)
                {
                    //case "Profit": products = products.OrderBy(x => x.PriceTotal).ToList(); break;
                    case "Marza": products = products.OrderBy(x => x.Marza).ToList(); break;
                    case "Narzut": products = products.OrderBy(x => x.Narzut).ToList(); break;

                }
            else
                switch (sortingExpression)
                {
                    //case "Profit": products = products.OrderByDescending(x => x.PriceTotal).ToList(); break;
                    case "Marza": products = products.OrderByDescending(x => x.Marza).ToList(); break;
                    case "Narzut": products = products.OrderByDescending(x => x.Narzut).ToList(); break;

                }



            gvProducts.DataSource = products;
            gvProducts.DataBind();
        }

        private decimal? GetDecimalValue(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;
            return Decimal.Parse(text);
        }

        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvProducts_Sorting(object sender, GridViewSortEventArgs e)
        {

            BindProducts(e.SortExpression, ViewState["sortingDirection"].ToString());
        }
    }
}