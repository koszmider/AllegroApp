using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;
using LajtIt.Web.Controls;

namespace LajtIt.Web
{
    [Developer("31B97915-C81E-497C-A686-0ABD9C7FFAFB")]
    public partial class ProductAttributeShopNames : LajtitPage
    {
       
        public int AttributeId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

         
    }
}