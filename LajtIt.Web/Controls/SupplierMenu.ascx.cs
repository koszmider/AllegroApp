using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;

namespace LajtIt.Web.Controls
{
    public partial class SupplierMenu : LajtitControl 
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
        private int SupplierId
        {
            get { return Convert.ToInt32(Request.QueryString["id"]); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindSelection();
            BindProduct(); 
        }

        private void BindProduct()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Supplier pc = oh.GetSupplier(SupplierId);
            litProductCatalog.Text = pc.Name;
        }

        private void BindSelection()
        {
            switch (SetTab)
            {

                case "td1": td1.Attributes.Add("class", "tabSelected"); break;
                case "td2": td2.Attributes.Add("class", "tabSelected"); break;
                case "td3": td3.Attributes.Add("class", "tabSelected"); break;
            }
            hl1.NavigateUrl = String.Format(hl1.NavigateUrl, SupplierId);
            hl2.NavigateUrl = String.Format(hl2.NavigateUrl, SupplierId);
            hl3.NavigateUrl = String.Format(hl3.NavigateUrl, SupplierId);
        }

        
    }
}