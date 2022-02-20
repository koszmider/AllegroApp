using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("9cf50b70-764a-415a-aaf1-4e566ff38a8f")]
    public partial class AllegroDeliveries : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindDeliveries();
        }

        private void BindDeliveries()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            gvDeliveries.DataSource = oh.GetAllegroDeliveryCostTypesWithNumbers();
            gvDeliveries.DataBind();
        }
    }
}