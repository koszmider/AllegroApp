using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace LajtIt.Web
{
    [Developer("97f49f34-5a28-4822-87ca-c8a64e3e497c")]
    public partial class ProductSynonims : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        //private List<Dal.ProductCatalogImage> images;
        private static decimal VAT = 23;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindProductCatalog();



        }
        protected void btnSynonimAdd_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogSynonim synonim = new Dal.ProductCatalogSynonim()
            {
                IsActive = true,
                Name = txbSynonim.Text.Trim(),
                ProductCatalogId = ProductCatalogId
            };
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetProductCatalogSynonim(synonim);
            BindProductCatalog();
        }
        protected void gvProductCatalogSynonims_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogSynonim syn = e.Row.DataItem as Dal.ProductCatalogSynonim;

                CheckBox chbAssign = e.Row.FindControl("chbAssign") as CheckBox;
                CheckBox chbCreate = e.Row.FindControl("chbCreate") as CheckBox;

                chbAssign.Checked = syn.IsActive;
                chbCreate.Checked = syn.UseForCreatingAuctions;

            }

        }
        //protected void gvProductCatalogSynonims_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    int productCatalogSynonimId = Convert.ToInt32(gvProductCatalogSynonims.DataKeys[e.RowIndex][0]);
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    oh.SetProductCatalogSynonimDelete(productCatalogSynonimId);

        //    BindProductCatalog();


        //}

        protected void chbAssign_OnCheckedChanged(object sender, EventArgs e)
        {
            int productCatalogSynonimId = GetId(sender);

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetProductCatalogSynonimActiveFlag(productCatalogSynonimId, ((CheckBox)sender).Checked);

            DisplayMessage("Zapisano");
        }
        protected void chbCreate_OnCheckedChanged(object sender, EventArgs e)
        {

            int productCatalogSynonimId = GetId(sender);

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetProductCatalogSynonimCreateFlag(productCatalogSynonimId, ((CheckBox)sender).Checked);

            DisplayMessage("Zapisano");

        }
        private int GetId(object sender)
        {
            GridViewRow gvr = (((CheckBox)sender).Parent.Parent.Parent.Parent) as GridViewRow;
            int productCatalogSynonimId = Convert.ToInt32(gvProductCatalogSynonims.DataKeys[gvr.RowIndex][0]);
            return productCatalogSynonimId;
        }
        private void BindProductCatalog()
        {
            int productCatalogId = ProductCatalogId;
            Dal.OrderHelper oh = new Dal.OrderHelper();
            // Dal.ProductCatalogItemStatsResult stat = oh.GetProductCatalogItemStats(productCatalogId);
            Dal.ProductCatalog pc = oh.GetProductCatalog(productCatalogId);
             
            gvProductCatalogSynonims.DataSource = oh.GetProductCatalogSynonims(productCatalogId);
            gvProductCatalogSynonims.DataBind();
        }




    }
}