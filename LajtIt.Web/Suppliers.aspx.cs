using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("d13455a4-bc47-417c-83b0-de6478b78e62")]
    public partial class Suppliers : LajtitPage
    {
        bool martketing = false;
        protected void Page_Load(object sender, EventArgs e)
        {
              martketing = HasActionAccess(Guid.Parse("60f1ceb1-0bba-49f8-bb34-34d174d8a9dc"));
            if (!Page.IsPostBack)
            { 
                ddlShop.DataSource = Dal.DbHelper.Shop.GetShops().Where(x=>x.IsActive && x.CanExportProducts).OrderBy(x=>x.Name).ToList();
                ddlShop.DataBind();

                BindSuppliers();

              
            }
           btnSave.Visible=  ddlShop.Visible = HasActionAccess(Guid.Parse("60f1ceb1-0bba-49f8-bb34-34d174d8a9dc"));
        }

        private void BindSuppliers()
        {
            int? shopId = null;

            if (ddlShop.SelectedIndex != 0)
                shopId = Convert.ToInt32(ddlShop.SelectedValue);

            gvSuppliers.Columns[8].Visible = gvSuppliers.Columns[9].Visible = HasActionAccess(Guid.Parse("fd24b3d7-0520-4507-b611-df8d1f80ee00"));

            gvSuppliers.Columns[10].Visible = martketing;

           


            Dal.ShopHelper oh = new Dal.ShopHelper();
            gvSuppliers.DataSource = oh.GetSuppliersShop(shopId);
            gvSuppliers.DataBind();
        }
 
        private void BindNames(object data)
        {
            int[] supplierIds = WebHelper.GetSelectedIds<int>(gvSuppliers, "chbOrder");

            Bll.ProductCatalogHelper pch = new Bll.ProductCatalogHelper();
            pch.UpdateProductNames(supplierIds);
            //Bll.ThreadResult.Add(requestId.ToString(), "Item Processed Successfully.");

        }
        private void LongRuningTask(object data)
        {
            //  simulate long running task – your main logic should   go here
            //Thread.Sleep(15000);

            // Add ThreadResult -- when this
            // line executes it  means task has been
            // completed
            //Bll.ThreadResult.Add(requestId.ToString(), "Item Processed Successfully."); // you  can add your result in second parameter.
            DisplayMessage("Zaktualizowano nazwy produktów");
        }

        protected void ddlShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSuppliers();
        }

        protected void lbtnCopy_Click(object sender, EventArgs e)
        {

            string template = ((TextBox)((LinkButton)sender).Parent.FindControl("txbTemplate")).Text;

            foreach (GridViewRow row in gvSuppliers.Rows)
            {
                TextBox txbTemplate = row.FindControl("txbTemplate") as TextBox;
                if (txbTemplate.Enabled)
                    txbTemplate.Text = template;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            int? shopId = null;

            if (ddlShop.SelectedIndex != 0)
                shopId = Convert.ToInt32(ddlShop.SelectedValue);

            if(!shopId.HasValue)
            {
                DisplayMessage("Wybierz dostawcę");
                return;
            }

            List<Dal.SupplierShop> ss = new List<Dal.SupplierShop>();

            foreach (GridViewRow row in gvSuppliers.Rows)
            {
                TextBox txbTemplate = row.FindControl("txbTemplate") as TextBox;

                if (txbTemplate == null || !txbTemplate.Enabled)
                    continue;

                string template = txbTemplate.Text.Trim() ;
                if (template == "")
                    template = null;
                int supplierId = Convert.ToInt32(gvSuppliers.DataKeys[row.RowIndex][0]);

                ss.Add(new Dal.SupplierShop()
                {
                    ShopId = shopId.Value,
                    Template = template,
                    SupplierId=supplierId
                });
            }

            Dal.ShopHelper sh = new Dal.ShopHelper();

            sh.SetSupplierShopShort(ss);


            DisplayMessage("Zapisano");
        }

        protected void gvSuppliers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.SupplierShopFnResult ss = e.Row.DataItem as Dal.SupplierShopFnResult;

                TextBox txbTemplate = e.Row.FindControl("txbTemplate") as TextBox;
                HyperLink hlSupplier = e.Row.FindControl("hlSupplier") as HyperLink;

                txbTemplate.Text = ss.Template;

                txbTemplate.Enabled = ss.ShopExportEnabled.Value;

                hlSupplier.Text = ss.Name;
                if(!martketing)
                    hlSupplier.NavigateUrl = String.Format(hlSupplier.NavigateUrl, ss.SupplierId);
                else
                    hlSupplier.NavigateUrl = String.Format("/Supplier.Descriptions.aspx?id={0}", ss.SupplierId);


                if (ss.ImportName.Equals("Automatycznie") && ss.IsActive && ss.LastImportDate.HasValue && ss.LastImportDate.Value >= DateTime.Now.AddHours(-4))
                    e.Row.Cells[7].BackColor = Color.GreenYellow;
                else if (ss.ImportName.Equals("Automatycznie") && ss.IsActive && ss.LastImportDate.HasValue && ss.LastImportDate.Value < DateTime.Now.AddDays(-1))
                    e.Row.Cells[7].BackColor = Color.Pink;
                else if (ss.ImportName.Equals("Automatycznie") && ss.IsActive && ss.LastImportDate.HasValue && ss.LastImportDate.Value < DateTime.Now.AddHours(-8))
                    e.Row.Cells[7].BackColor = Color.Orange;
                else if (ss.ImportName.Equals("Automatycznie") && ss.IsActive && ss.LastImportDate.HasValue && ss.LastImportDate.Value < DateTime.Now.AddHours(-4))
                    e.Row.Cells[7].BackColor = Color.Yellow;
            }
        }

        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnNames.Visible = pnDeliveries.Visible = false;

            switch (Int32.Parse(ddlAction.SelectedValue))
            {
                case 0: return;
                case 1: BindNames(); break;
                case 2: BindDeliveries(); break; 
            }
            pAction.Visible = ddlAction.SelectedIndex != 0; ;
        }

        private void BindDeliveries()
        {
            pnDeliveries.Visible = true;
            Dal.ProductCatalogHelper pc = new Dal.ProductCatalogHelper();
            var d = pc.GetAllegroDeliveryCostTypes().Where(x => x.DeliveryCostTypeId != 0 && x.IsActive).ToList();
            ddlAllegroDeliveryType.DataSource = d;
            ddlAllegroDeliveryType.DataBind();
            ddlAllegroAlternativeDeliveryType.DataSource = d;
            ddlAllegroAlternativeDeliveryType.DataBind();
        }

        private void BindNames()
        {
            pnNames.Visible = true;
        }

        protected void btnAction_Click(object sender, EventArgs e)
        {
            int[] supplierIds = WebHelper.GetSelectedIds<int>(gvSuppliers, "chbOrder");
 
            if (supplierIds.Count() == 0)
            {
                DisplayMessage("Nie wybrano żadnego dostawcy");
                return;
            }

            switch (Int32.Parse(ddlAction.SelectedValue))
            {
                case 1: SetNames(supplierIds); break;
                case 2: SetDeliveries(supplierIds); break;
            }
        }

        private void SetDeliveries(int[] supplierIds)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            if (ddlAllegroDeliveryType.SelectedIndex == 0 || ddlAllegroAlternativeDeliveryType.SelectedIndex == 0)
            {
                DisplayMessage("Musisz wybrać cenniki");
                return;
            }
            foreach (int supplierId in supplierIds)
            {
                int d1 = Int32.Parse(ddlAllegroDeliveryType.SelectedValue);
                int d2 = Int32.Parse(ddlAllegroAlternativeDeliveryType.SelectedValue);
                pch.SetSupplierDelivery(supplierId, d1, d2);

            }
            DisplayMessage("Zmiany zapisane");
        }

        private void SetNames(int[] supplierIds)
        {     // Start the long running task on one thread
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(BindNames);

            Thread thread = new Thread(parameterizedThreadStart);

            thread.Start();

            // Show Modal Progress Window
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "OpenProgressWindow('" + requestId.ToString() + "');", true);


            DisplayMessage("Zmiana nazw została zainicjowana. Wykonuje się w tle i może potrwać kilka minut.");
        }
    }
}