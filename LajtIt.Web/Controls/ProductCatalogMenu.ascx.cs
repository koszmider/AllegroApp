using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;

namespace LajtIt.Web.Controls
{
    public partial class ProductCatalogMenu : LajtitControl 
    {

        public string SetTab
        {
            set
            {
                ViewState["tab"] = value;
            }
            get
            {
                return ViewState["tab"].ToString();
            }
        }
        private int ProductCatalogId
        {
            get { return Convert.ToInt32(Request.QueryString["id"]); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindSelection();
            BindProduct();
            hlPreview.NavigateUrl = String.Format(hlPreview.NavigateUrl, ProductCatalogId);
        }

        private void BindProduct()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.ProductCatalogView pc = oh.GetProductCatalog(ProductCatalogId);
            litProductCatalog.Text = pc.Name;
        }

        private void BindSelection()
        {
            switch (SetTab)
            {

                case "td1": td1.Attributes.Add("class", "tabSelected"); break;
                case "td2": td2.Attributes.Add("class", "tabSelected"); break;
                case "td3": td3.Attributes.Add("class", "tabSelected"); break;
                case "td4": td4.Attributes.Add("class", "tabSelected"); break;
              //  case "td5": td5.Attributes.Add("class", "tabSelected"); break;
                case "td6": td6.Attributes.Add("class", "tabSelected"); break;
                //case "td7": td7.Attributes.Add("class", "tabSelected"); break;
                case "td8": td8.Attributes.Add("class", "tabSelected"); break;
                case "td9": td9.Attributes.Add("class", "tabSelected"); break;
                case "td10": td10.Attributes.Add("class", "tabSelected"); break;
                case "td11": td11.Attributes.Add("class", "tabSelected"); break;
                case "td12": td12.Attributes.Add("class", "tabSelected"); break;
                case "td13": td13.Attributes.Add("class", "tabSelected"); break;
                case "td14": td14.Attributes.Add("class", "tabSelected"); break;
                case "td15": td15.Attributes.Add("class", "tabSelected"); break;
            }
            hl1.NavigateUrl = String.Format(hl1.NavigateUrl, ProductCatalogId);
            hl2.NavigateUrl = String.Format(hl2.NavigateUrl, ProductCatalogId);
            hl3.NavigateUrl = String.Format(hl3.NavigateUrl, ProductCatalogId);
            hl4.NavigateUrl = String.Format(hl4.NavigateUrl, ProductCatalogId);
           // hl5.NavigateUrl = String.Format(hl5.NavigateUrl, ProductCatalogId);
            hl6.NavigateUrl = String.Format(hl6.NavigateUrl, ProductCatalogId);
            //hl7.NavigateUrl = String.Format(hl7.NavigateUrl, ProductCatalogId);
            hl8.NavigateUrl = String.Format(hl8.NavigateUrl, ProductCatalogId);
            hl9.NavigateUrl = String.Format(hl9.NavigateUrl, ProductCatalogId);
            hl10.NavigateUrl = String.Format(hl10.NavigateUrl, ProductCatalogId);
            hl11.NavigateUrl = String.Format(hl11.NavigateUrl, ProductCatalogId);
            hl12.NavigateUrl = String.Format(hl12.NavigateUrl, ProductCatalogId);
            hl13.NavigateUrl = String.Format(hl13.NavigateUrl, ProductCatalogId);
            hl14.NavigateUrl = String.Format(hl14.NavigateUrl, ProductCatalogId);
            hl15.NavigateUrl = String.Format(hl15.NavigateUrl, ProductCatalogId);
        }

        
    }
}