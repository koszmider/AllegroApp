using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class NewDelivery : LajtitControl
    {
        public int WarehouseId { get { return Convert.ToInt32(ddlWarehouse.SelectedValue); } }
        public int Quantity { get { return Convert.ToInt32(txbCount.Text.Trim()); } }
        public int QuantityWarehouse { get { return Convert.ToInt32(txbCountWarehouse.Text.Trim()); } }

        public bool HasDeliveryForWarehouse
        {
            get
            {
                return (!String.IsNullOrEmpty(txbCountWarehouse.Text));


            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindProductCatalog(List<Dal.ProductCatalogWarehouse> warehouse, int count, int? warehouseId)
        {
            txbCount.Text = String.Format("{0}", count);


            ddlWarehouse.DataSource = warehouse;
            ddlWarehouse.DataBind();

            if (warehouseId.HasValue)
                ddlWarehouse.SelectedValue = warehouseId.Value.ToString();
        }
    }
}