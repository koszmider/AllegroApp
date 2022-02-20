using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("6de76aa3-09fb-4188-8122-c85f1a019dee")]
    public partial class ProductCatalogVerification : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindProducts();
        }

        private void BindProducts()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            gvProducts.DataSource = oh.GetProductCatalogAllegroItemsWithErrors();
            gvProducts.DataBind();
        }


        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogAllegroItemErrorView pc = e.Row.DataItem as Dal.ProductCatalogAllegroItemErrorView;

                //HyperLink hlCalcuator = e.Row.FindControl("hlCalcuator") as HyperLink;
                // hlCalcuator.NavigateUrl = String.Format("/ProductCalculator.aspx?id={0}", pc.ProductCatalogId);
                Image imgImage = e.Row.FindControl("imgImage") as Image;
                Literal liId = e.Row.FindControl("liId") as Literal;
                HyperLink hlProduct = e.Row.FindControl("hlProduct") as HyperLink;
                HyperLink hlPreview = e.Row.FindControl("hlPreview") as HyperLink;


                Label lblCode = e.Row.FindControl("lblCode") as Label;

                hlProduct.Text = pc.AllegroName;
                hlProduct.NavigateUrl = String.Format(hlProduct.NavigateUrl, pc.ProductCatalogId);

                //liId.Text = String.Format("{0}.", gvProducts.PageIndex * gvProducts.PageSize + e.Row.RowIndex + 1);

                if (!String.IsNullOrEmpty(pc.ImageFullName))
                    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", pc.ImageFullName);
                else
                    imgImage.Visible = false;
                //if (!pc.IsReady)
                //    e.Row.Style.Add("background-color", "silver");

                lblCode.Text = String.Format("{0}<br>{1}", pc.Code, pc.Ean);



            }
        }
        protected void lbtnChecked_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetProductCatalogAllegroItemErrorFixed(id, true);
            DisplayMessage("Naprawiono, sprawdzono");
        }
    }
}
