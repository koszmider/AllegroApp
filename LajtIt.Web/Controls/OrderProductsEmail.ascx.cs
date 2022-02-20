using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class OrderProductsEmail : System.Web.UI.UserControl
    {
        public delegate void SavedEventHandler(object sender, EventArgs e);
        public event SavedEventHandler Send;
        public delegate void CancelEventHandler(object sender, EventArgs e);
        public event CancelEventHandler Cancel;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void LoadForm(int supplierId, int[] orderProductIds)
        {
            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            Dal.Supplier supplier = oh.GetSupplier(supplierId);


            txbEmail.Text = supplier.SupplierOwner.EmailOrder;
            txbBody.Text = sh.GetSetting("ORD_EM_BOD").StringValue;
            txbTitle.Text = sh.GetSetting("ORD_EM_TIT").StringValue;

            txbTitle.Text = txbTitle.Text.Replace("[SUPPLIER]", supplier.SupplierOwner.Name);

            rblWarehouse.DataSource = pch.GetWarehouse();
            rblWarehouse.DataBind();

            rblWarehouse.SelectedValue = "1";

            //txbBody.Text=txbBody.Text.Replace("[DELIVERY_ADDRESS]", rb)

            string s = "";

            List<Dal.OrderProductsWaitingForDelivery> products = oh.GetOrdersProductsWaitingForDelivery();

            products = products.Where(x => orderProductIds.Contains(x.OrderProductId)).ToList();
            foreach (Dal.OrderProductsWaitingForDelivery op in products)
            {
                s += String.Format("{0} {1} {2}szt.<br>\n", op.SupplierName, op.Code, op.Quantity);
            }
            txbBody.Text = txbBody.Text.Replace("[PRODUCTS]", s);
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

             
            List<Dal.ProductCatalogWarehouse> w = pch.GetWarehouse();

            string body = txbBody.Text;

            body = body.Replace("[DELIVERY_ADDRESS]", w.Where(x=>x.WarehouseId == Convert.ToInt32(rblWarehouse.SelectedValue))
                .FirstOrDefault().WarehouseAddress);

            Bll.EmailSender es = new Bll.EmailSender();
            es.SendEmail(txbTitle.Text, body, txbEmail.Text);


            if (Send != null)
                Send(null, null);

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Cancel != null)
                Cancel(null, null);

        }
    }
}