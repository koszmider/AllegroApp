using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    public partial class JQueryTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //dp.ContextKey = rbl.ClientID;

            List<ListItem> items = new List<ListItem>(); 
            for (int i = 10; i < 20; i++)
            {
                var it = new ListItem()
                {
                    Text = Guid.NewGuid().ToString().Substring(0, 4) + "=" + i.ToString(),
                    Value = i.ToString(),
                    Selected = i % 2 == 0
                }; 
                items.Add(it);
            }


            rbl.Items.AddRange(items.ToArray());
        }
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static CascadingDropDownNameValue[] DynamicPopulateMethod(string contextKey) {
            List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();
             
            for (int i = 10; i < 20; i++)
            {
                var it = new CascadingDropDownNameValue()
                {
                    name = Guid.NewGuid().ToString().Substring(0, 4) + "=" + i.ToString(),
                    value = i.ToString(),
                    isDefaultValue  = i % 2 == 0
                };
                l.Add(it);
            }


            return l.ToArray();
        }
    }
}