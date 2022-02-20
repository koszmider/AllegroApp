using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;
using LajtIt.Web.Controls;

namespace LajtIt.Web
{
    [Developer("6DB56B6B-5B08-4CAA-905E-FD96E810BB54")]
    public partial class ProductAttributeTitles : LajtitPage
    {

        public int AttributeId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindAttribute();
        }

        private void BindAttribute()
        {
            Dal.ProductCatalogAttribute attribute = Dal.DbHelper.ProductCatalog.GetProductCatalogAttribute(AttributeId);

            if(attribute.AttributeGroupId!=6)
            {
                ucAttributeMenu.NotAvailable();
                pnContent.Visible = false;
                return;
            }



            gvShop.DataSource = Dal.DbHelper.ProductCatalog.GetProductCatalogShopAttributeTemplate(AttributeId).OrderBy(x=>x.ShopName).ToList();
            gvShop.DataBind();
        }

        protected void gvShop_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogShopAttributeTemplateFnResult attribute = e.Row.DataItem as Dal.ProductCatalogShopAttributeTemplateFnResult;

                TextBox txbTemplate = e.Row.FindControl("txbTemplate") as TextBox;
                Label lblShopName = e.Row.FindControl("lblShopName") as Label;
                AjaxControlToolkit.TextBoxWatermarkExtender tbwWater = e.Row.FindControl("tbwWater") as AjaxControlToolkit.TextBoxWatermarkExtender;


                txbTemplate.Text = attribute.Template;
                lblShopName.Text = attribute.ShopName;

                if (attribute != null && !String.IsNullOrEmpty(attribute.ShopTemplate))
                {
                    tbwWater.WatermarkText = attribute.ShopTemplate;
                }
            }
        }
        protected void btnAttributeSave_Click(object sender, EventArgs e)
        {
            List<Dal.ProductCatalogShopAttributeTemplate> templates = new List<Dal.ProductCatalogShopAttributeTemplate>();

            foreach (GridViewRow rowSex in gvShop.Rows)
            {
                TextBox txbTemplate = rowSex.FindControl("txbTemplate") as TextBox;
                int shopId = Convert.ToInt32(gvShop.DataKeys[rowSex.RowIndex][0]);


                templates.Add(
                    new Dal.ProductCatalogShopAttributeTemplate()
                    {
                        AttributeId = AttributeId,
                        InsertDate = DateTime.Now,
                        InsertUser = UserName,
                        ShopId = shopId,
                        Template = txbTemplate.Text.Trim(),
                        UpdateDate = DateTime.Now,
                        UpdateUser = UserName
                    }
                    );

            }

            Dal.DbHelper.ProductCatalog.SetProductCatalogAttributeShopTemplates(AttributeId, templates);


            DisplayMessage("Zapisano zmiany");
        }

        protected void lbtnCreateNames_Click(object sender, EventArgs e)
        {
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(BindNames);

            Thread thread = new Thread(parameterizedThreadStart);

            object[] par = new object[] { AttributeId };
            thread.Start(par);

            // Show Modal Progress Window
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "OpenProgressWindow('" + requestId.ToString() + "');", true);


            DisplayMessage("Zmiana nazw została zainicjowana. Wykonuje się w tle i może potrwać kilka minut.");
        }
        private void BindNames(object data)
        {
            object[] par = (object[])data;
            int attributeId = (int)par[0]; 
             
            Bll.ProductCatalogHelper pchB = new Bll.ProductCatalogHelper();


            pchB.UpdateProductNames(attributeId, Helper.ShopType.Allegro);
            //Bll.ThreadResult.Add(requestId.ToString(), "Item Processed Successfully.");

        }
    }
}