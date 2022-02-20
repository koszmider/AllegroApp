using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace LajtIt.Web
{
    [Developer("75025a58-b4b7-434e-bf70-0d9823da680f")]
    public partial class ProductNew : LajtitPage
    { 
         
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack) BindSuppliers();
             

        }
 
     
        protected void txbCode_OnTextChanged(object sender, EventArgs e)
        {

            string code = txbCode.Text.Trim();
            int supplierId = Convert.ToInt32(ddlSuppliers.SelectedValue);


            Dal.OrderHelper oh = new Dal.OrderHelper();
            bool isUnique = oh.GetProductCatalogCode(null, code, supplierId);

            if (!isUnique)
            {
                txbCode.Text = "";
                DisplayMessage(String.Format("Kod: {0} jest już w użyciu", code));
            }

        }
        private void BindSuppliers()
        { 
            ddlSuppliers.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderBy(x => x.Name).ToList();
            ddlSuppliers.DataBind();

            ucProductCatalogGroup .BindProductCatalogGroups(Convert.ToInt32(ddlSuppliers.SelectedValue));
        }

         
        protected void btnProductNewSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            { 
                Dal.ProductCatalog pc1 = Dal.ProductCatalogHelper.GetProductCatalog(Convert.ToInt32(ddlSuppliers.SelectedValue), txbCode.Text.Trim(), true);
                int productCatalogGroupId = ucProductCatalogGroup.ProductCatalogGroupId;

                pc1.PriceBruttoFixed = Convert.ToDecimal(txbPriceBrutto.Text.Trim());

                pc1.ProductTypeId = rbProductTypeId1.Checked ? 1 : 3;

                Dal.OrderHelper oh = new Dal.OrderHelper();
                int productCatalogId = oh.SetProductCatalogNew(pc1, UserName, productCatalogGroupId);
                DisplayMessage(String.Format("Zapisano. <a href='/Product.aspx?id={0}'>Kliknij tutaj by zakończyć edycję produktu</a>", productCatalogId));
            }
            catch (Exception ex)
            {
                DisplayMessage(String.Format("Błąd podczas dodawania produktu. Sprawdź wszystkie pola i spróbuj ponownie.<br><br><Br>Opis błędu:{0}",
                    ex.Message));
            }
        }

        protected void ddlSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucProductCatalogGroup.BindProductCatalogGroups(Convert.ToInt32(ddlSuppliers.SelectedValue));
        }
    }
}