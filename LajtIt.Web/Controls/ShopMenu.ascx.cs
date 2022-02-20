using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;

namespace LajtIt.Web.Controls
{
    public partial class ShopMenu : LajtitControl 
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
        private int ShopId
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
            Dal.Shop pc = Dal.DbHelper.Shop.GetShop(ShopId);
            litProductCatalog.Text = pc.Name;
        }

        private void BindSelection()
        {
            switch (SetTab)
            {

                case "td1": td1.Attributes.Add("class", "tabSelected"); break;
                case "td2": td2.Attributes.Add("class", "tabSelected"); break; 
            }
            hl1.NavigateUrl = String.Format(hl1.NavigateUrl, ShopId);
            hl2.NavigateUrl = String.Format(hl2.NavigateUrl, ShopId); 
        }

        
    }
}