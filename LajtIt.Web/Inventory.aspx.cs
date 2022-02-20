using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;

namespace LajtIt.Web
{
    [Developer("be0b3b57-7d22-496e-a3f6-c066a9f62276")]
    public partial class Inventory : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                ddlWarehouse.DataSource = pch.GetWarehouse();
                ddlWarehouse.DataBind();
                lbxSearchWarehouse.DataSource = pch.GetWarehouse();
                lbxSearchWarehouse.DataBind();
                    BindSuppliers();

                BindProducts();

            }
        }
        private void BindProducts()
        {
            int[] suppliers = lbxSearchSupplier.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();
            int[] warehouses = lbxSearchWarehouse.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();
            int year = 2019;

            Dal.InventoryHelper ih = new Dal.InventoryHelper();

            var r = ih.GetInventory();

            r = r.Where(x => x.Year == year).ToList();

            if (suppliers.Count() > 0)
                r = r.Where(x => suppliers.Contains(x.SupplierId)).ToList();

            if (warehouses.Count() > 0)
                r = r.Where(x => warehouses.Contains(x.WarehouseId)).ToList();


            gvProducts.DataSource = r.OrderByDescending(x=>x.InsertDate);
            gvProducts.DataBind();

            var i =  ih.GetInventorySummary(year);

            if (chbNotMatch.Checked)
                i = i.Where(x => x.Quantity != x.LeftQuantity).ToList();

            gvInventorySummary.DataSource = i;
            gvInventorySummary.DataBind();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string sproductCatalogId = Request.Form[hfProductCatalogId.UniqueID];
            int productCatalogId = 0;

            if (!Int32.TryParse(sproductCatalogId, out productCatalogId))
            {
                DisplayMessage("Produkt nie istnieje");
                return;
            }


            Dal.Inventory i = new Dal.Inventory()
            {
                Comment = txbComment.Text.Trim(),
                InsertDate = DateTime.Now,
                InventryProductStatusId = 1,
                ProductCatalogId = productCatalogId,
                Quantity = Int32.Parse(txbQuantity.Text.Trim()),
                WarehouseId = Int32.Parse(ddlWarehouse.SelectedValue),
                Year = 2019
            };

            Dal.InventoryHelper ih = new Dal.InventoryHelper();
            ih.SetInventory(i);

            DisplayMessage("Dodano");
            BindProducts();

        }

        private void BindSuppliers()
        { 
            lbxSearchSupplier.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers();
            lbxSearchSupplier.DataBind();

            lbxSearchSupplier.SelectedValue = "3";
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindProducts();
        }

        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int id = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex][0]);
            Dal.InventoryHelper ih = new Dal.InventoryHelper();
            ih.SetInventoryDelete(id);


            DisplayMessage("Usunięto");
            BindProducts();
        }

        protected void gvInventorySummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType== DataControlRowType.DataRow)
            {
                Dal.InventorySummaryResult i = e.Row.DataItem as Dal.InventorySummaryResult;

                if (i.Quantity.Value != i.LeftQuantity)
                    e.Row.BackColor = Color.Pink;
            }
        }
    }
}