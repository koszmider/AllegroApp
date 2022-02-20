using System;
using System.Collections.Generic;
using System.Linq;
using LajtIt.Bll;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class Products : LajtitControl
    {
        public delegate void CanceledEventHandler(object sender, EventArgs e);
        public event CanceledEventHandler Canceled;
        public delegate void AddedEventHandler(object sender, int productCatalog, int quantity);
        public event AddedEventHandler Added;

        protected void ddlSupplier_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindProducts();
        }
        protected void ddlProducts_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindValidator();

        }

        private void BindValidator()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            LajtIt.Dal.ProductCatalogView products =
                oh.GetProductCatalogsForOrder(Convert.ToInt32(ViewState["OrderId"]), Convert.ToInt32(ddlSupplier.SelectedValue), Convert.ToInt32(ddlProducts.SelectedValue))
               .Where(x => x.ProductCatalogId == Convert.ToInt32(ddlProducts.SelectedValue)).FirstOrDefault();

            
        }
        protected void lnbProductCancel_Click(object sender, EventArgs e)
        {

            if (Canceled != null)
                Canceled(this, e);
        }
        protected void btnProductAdd_Click(object sender, EventArgs e)
        {
            if (Added != null)
                Added(this, Convert.ToInt32(ddlProducts.SelectedValue), Convert.ToInt32(txbQuantity.Text));
        }
        public void BindProducts(int orderId)
        {
             

            ddlSupplier.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderBy(x=>x.Name);
            ddlSupplier.DataBind();
            ViewState["OrderId"] = orderId;

            BindProducts();
        }

        private void BindProducts()
        {
            try
            {
                Dal.OrderHelper oh = new Dal.OrderHelper();
                List<LajtIt.Dal.ProductCatalogView> products =
                    oh.GetProductCatalogsForOrder(Convert.ToInt32(ViewState["OrderId"]), Convert.ToInt32(ddlSupplier.SelectedValue), null).ToList();

                var o = products
                    .OrderBy(x => x.Code)
                    .Select(x => new
                    {
                        ProductCatalogId = x.ProductCatalogId,
                        Name = String.Format("{0}|{1}|{2}|{3}",
                        SetColumn(x.Code, 20, 1),
                        SetColumn(x.Name, 50, 1),
                        SetColumn(x.PriceBruttoFixed.ToString("C"), 12, 2),
                            SetColumn(String.Format("{0} szt. w mag.", x.LeftQuantity), 20, 1))
                    }).ToList();

                ddlProducts.DataSource = o;
                ddlProducts.DataBind();

                DecodeDropDownList(ddlProducts);
                BindValidator();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError(ex, "BindPRoducts");

            }
        }

        public static void DecodeDropDownList(DropDownList ddlProducts)
        {
            string spaceDecode = System.Web.HttpContext.Current. Server.HtmlDecode("&nbsp;");
            for (int i = 0; i < ddlProducts.Items.Count; i++)
            {
                System.Web.UI.WebControls.ListItem item = ddlProducts.Items[i];
                item.Text = item.Text.Replace(" ", spaceDecode);
            }
        }

        public static string SetColumn(string value, int length, int type)
        {
            string tmp = new String(' ', length);
            if (type == 1)
                return String.Format("{0}{1}", value, tmp).Substring(0, length);//.Replace(" ", "&nbsp;");
            else
                return String.Format("{1}{0}", value, tmp).Substring(tmp.Length+value.Length - length, length);//.Replace(" ", "&nbsp;");

        }
    }
}