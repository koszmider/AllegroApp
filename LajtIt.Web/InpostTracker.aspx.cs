using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("60fd7c94-fb83-41a2-8447-2f1352f4b95a")]
    public partial class InpostTracker : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            Bll.InpostHelper ih = new Bll.InpostHelper();
            

            txbResult.Text = ih.Search(String.Format("tracking_number={0}", txbTrackingNumber.Text));
        }
    }
}