using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI.DataVisualization.Charting;

using System.Net;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Drawing;

namespace LajtIt.Web
{
    [Developer("d026436d-8131-4ae0-9076-444114f82867")]
    public partial class TestPage : LajtitPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
          
            //lbl.Text = (39393.4593000).ToString("0.000", new System.Globalization.CultureInfo("en-US"));

            //if(!Page.IsPostBack)
            //ucShopCategoryControl.SetCategoryId(7, null);
            //ucShopCategoryControl.BindCategories(null, null, (int)Dal.Helper.Shop.eMag);

            if(!Page.IsPostBack)
            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImages(97001);

                ListView1.DataSource = images;
                ListView1.DataBind();
                ListView2.DataSource = images;
                ListView2.DataBind();
            }
        }
 
    }
}