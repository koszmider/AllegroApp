using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("7a22749e-403b-4292-a148-f4a2714b5f23")]
    public partial class ProductCatalog_Preview : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }
        public bool Preview
        {
            get
            {
                if (String.IsNullOrEmpty(Request.QueryString["preview"] ))
                    return true;
                else
                    return Convert.ToBoolean(Request.QueryString["preview"]);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindProductTypeSchemas();
                BindForm();
            } 
        }

        private void BindProductTypeSchemas()
        { 
            ddlShop.DataSource = Dal.DbHelper.Shop.GetShops().Where(x=>x.IsActive && x.CanExportProducts).ToList();
            ddlShop.DataBind();

        }
        protected void rpImages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Image imgImage = e.Item.FindControl("imgImage") as Image;
                Dal.ProductCatalogImage image = e.Item.DataItem as Dal.ProductCatalogImage;
                imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", image.FileName);

            }
        }

        protected void ddlShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindForm();
        }

        private void BindForm()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImages(ProductCatalogId).Where(x=>x.IsActive).ToList();

            Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), Int32.Parse(ddlShop.SelectedValue));
            Dal.ProductCatalogShopProductFnResult pc = Dal.DbHelper.ProductCatalog.GetProductCatalogShopProduct(shop, ProductCatalogId);

            string sb = Bll.OrderHelper.GetPreview(shop, ProductCatalogId, Preview);


          
            if (!String.IsNullOrEmpty(pc.LongDescription))
            {
                lblDescriptionInfo.Text = "Zapisany opis. Zmień sklep by zobaczyć inną wersję";
                lblDescription.Text = pc.LongDescription;
            }
            else
            {
                lblDescriptionInfo.Text = "Opis wygenerowany automatycznie";
                lblDescription.Text = Bll.Mixer.GetDescription(shop, ProductCatalogId);
            }
            lblSpecification.Text = sb;
            lblName.Text = pc.Name;
            hlProduct.NavigateUrl = String.Format(hlProduct.NavigateUrl, ProductCatalogId);

            rpImages.DataSource = images;
            rpImages.DataBind();
        }
    }
}