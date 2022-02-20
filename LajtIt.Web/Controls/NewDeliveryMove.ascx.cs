using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class NewDeliveryMove : LajtitControl
    {
        public delegate void SavedEventHandler(object sender, EventArgs e);
        public event SavedEventHandler Saved;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void BindWarehouse(int? warehouseIdToExclude)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Dal.ProductCatalogWarehouse> warehouses = pch.GetWarehouse().Where(x => x.CanAssignDelivery).ToList();

            ddlWarehouseFrom.DataSource = warehouses;
            ddlWarehouseFrom.DataBind();

            if (warehouseIdToExclude.HasValue)
            {
                ddlWarehouseFrom.SelectedValue = warehouseIdToExclude.ToString();

            }
            else
                warehouseIdToExclude = Convert.ToInt32(ddlWarehouseFrom.SelectedValue);
            ddlWarehouseTo.DataSource = warehouses.Where(x => x.WarehouseId != warehouseIdToExclude.Value).ToList(); ;
            ddlWarehouseTo.DataBind();

        }

        protected void ddlWarehouseFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindWarehouse(Convert.ToInt32(ddlWarehouseFrom.SelectedValue));
            BindWarehouse();
        }
        protected void btnMove_Click(object sender, EventArgs e)
        {
            Bll.ProductCatalogHelper pch = new Bll.ProductCatalogHelper();
            bool result = pch.DeliveryProuctsMove(Convert.ToInt32(ViewState["ProductCatalogId"]),
                Convert.ToInt32(ddlWarehouseFrom.SelectedValue),
                Convert.ToInt32(ddlWarehouseTo.SelectedValue),
                Convert.ToInt32(txbQuantity.Text.Trim()));

            if (result)
            {
                DisplayMessage("Przeniesiono produkty");

                if (Saved != null)
                    Saved(this, e);
            }
            else
                DisplayMessage("Błąd przenoszenia produktów");

        }
        public void BindWarehouseByProduct(int productCatalogId)
        {
            BindWarehouse(null);
            ViewState["ProductCatalogId"] = productCatalogId;
            BindWarehouse();

        }

        private void BindWarehouse()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.ProductCatalogViewFnResult pc1 = oh.GetProductCatalogFn(Convert.ToInt32(ViewState["ProductCatalogId"]), Convert.ToInt32(ddlWarehouseFrom.SelectedValue));
            btnMove.Enabled =    pc1.LeftQuantity >0;
            lbInfo.Visible = pc1.LeftQuantity <= 0;

            rvQuantity.MaximumValue = pc1.LeftQuantity <0?"0": pc1.LeftQuantity.ToString();
            rvQuantity.Text = String.Format("Możesz przenieść maksymalnie {0} sztuk", pc1.LeftQuantity);
        }
    }
}