using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    public static class WebHelper
    {
        public static long[] GetIDs(ListControl list)
        {
            return   list.Items.Cast<ListItem>().Where(x=>x.Selected).Select(x=>Convert.ToInt64(x.Value)).ToArray();
        }

        public static T[] GetSelectedIds<T>(GridView gv, string checkboxName)
        {
            return GetSelectedIds<T>(gv, checkboxName, 0);
        }
        public static T[] GetSelectedIds<T>(GridView gv, string checkboxName, int keyIndex)
        {
            List<T> list = new List<T>();
            foreach (GridViewRow row in gv.Rows)
            {
                CheckBox chbOrder = row.FindControl(checkboxName) as CheckBox;
                if (chbOrder.Checked)
                {
                    T id =  (T)gv.DataKeys[row.RowIndex][keyIndex];
                    list.Add(id);
                }
            }
            return list.ToArray();
        }
    }
}