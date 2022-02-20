using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("5b7ebf77-905c-4a11-8615-db7da018df64")]
    public partial class Duplicates : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindDuplicates();
            }
        }

        private void BindDuplicates()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogDuplicates> duplicates = pch.GetProductCatalogDuplicates();

            ddlDuplicates.Items.AddRange(
                duplicates.Select(x => new ListItem() { Text = String.Format("{0} {2} ({1})  ", x.Ean, x.ProductCount, x.SupplierName), Value = x.Ean }).ToArray()
                );
        }

        protected void ddlDuplicates_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProducts();
        }

        private void BindProducts()
        {
            string ean = ddlDuplicates.SelectedValue;


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogDuplicatesFnResult> duplicates = pch.GetProductCatalogDuplicates(ean);


            gvDuplicates.DataSource = duplicates;
            gvDuplicates.DataBind();

            btnDelete.Visible = duplicates.Count > 1;
        }

        protected void gvDuplicates_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogDuplicatesFnResult pc = e.Row.DataItem as Dal.ProductCatalogDuplicatesFnResult;

                Image imgImage = e.Row.FindControl("imgImage") as Image;
                HyperLink hlPreview = e.Row.FindControl("hlPreview") as HyperLink;
                RadioButton rbRowSelector = e.Row.FindControl("rbRowSelector") as RadioButton;

                rbRowSelector.Attributes.Add("onclick", String.Format("checkRadioBtn(this,'{0}');", gvDuplicates.ClientID));
                hlPreview.NavigateUrl = String.Format(hlPreview.NavigateUrl, pc.ProductCatalogId, "");

                if (!String.IsNullOrEmpty(pc.ImageFileName))
                    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", pc.ImageFileName);
                else
                    imgImage.Visible = false;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int? productCatalogId = null;

            foreach (GridViewRow row in gvDuplicates.Rows)
            {
                RadioButton rbRowSelector = row.FindControl("rbRowSelector") as RadioButton;

                if (rbRowSelector.Checked)
                    productCatalogId = Int32.Parse(gvDuplicates.DataKeys[row.RowIndex][0].ToString());

            }

            if (!productCatalogId.HasValue)
                DisplayMessage("Wybierz produkt, który nie będzie usunięty");


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            string[] ids = pch.GetProductCatalogDuplicates(ddlDuplicates.SelectedValue)
                .Where(x => x.ShopProductId!=null && x.ProductCatalogId != productCatalogId)
                .Select(x => x.ShopProductId)
                .ToArray();

            try

            {

                Bll.ShopHelper sh = new Bll.ShopHelper();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
                sh.DeleteProducts(ids);

                pch.SetProductCatalogDeleteDuplicates(productCatalogId.Value);
                BindProducts();
            }
            catch (Exception ex)

            {
                DisplayMessage(ex.Message);
            }
        }
    }
}