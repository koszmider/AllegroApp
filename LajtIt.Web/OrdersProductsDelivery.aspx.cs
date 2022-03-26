using LajtIt.Web.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace LajtIt.Web
{
    [Developer("ec5a5680-9628-4df8-b0a0-874b6e0dddcb")]
    public partial class OrdersProductsDelivery : LajtitPage
    {
        List<Dal.ProductCatalogWarehouse> warehouse;
        private enum FormMode
        {
            Normal = 1,
            NewDelivery = 2

        }
        private FormMode Mode
        {
            set { ViewState["Mode"] = value; }
            get
            {
                if (ViewState["Mode"] == null)
                    return FormMode.Normal;
                else
                    return (FormMode)Enum.Parse(typeof(FormMode), ViewState["Mode"].ToString());
            }
        }
        public static void SetOrderStatusAfterDelivery(int[] orderProductIds, string userName)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.OrderProduct> orderProducts = oh.GetOrderProducts(orderProductIds);

            int[] orderIds = orderProducts.Select(x => x.OrderId).Distinct().ToArray();

            foreach (int orderId in orderIds)
            {
                int count = orderProducts.Where(x => x.OrderId == orderId && x.SubOrderProductId == null).Count();
                int[] statusesToGo =
                    new int[] { (int)Dal.Helper.OrderProductStatus.Ready, (int)Dal.Helper.OrderProductStatus.Completed, (int)Dal.Helper.OrderProductStatus.Deleted };

                int countReady = orderProducts.Where(x => x.OrderId == orderId
                && statusesToGo.Contains(x.OrderProductStatusId)
                && x.SubOrderProductId == null).Count();
                string msg = "";
                Dal.OrderStatusHistory osh = new Dal.OrderStatusHistory()
                {
                    Comment = "Przyjęcie towaru, automatyczna zmiana statusu zamówienia",
                    InsertDate = DateTime.Now,
                    InsertUser = userName,
                    OrderId = orderId

                };

                Dal.Order order = Dal.DbHelper.Orders.GetOrder(orderId);

                if (count == countReady)
                {

                //    throw new NotImplementedException();
                   // hack
                    oh.SetOrderCompanyId(orderId, Dal.Helper.DefaultCompanyId);
                    int statusId = (int)Dal.Helper.OrderStatus.New;
                    switch (order.OrderShipping.ShippingServiceMode.WarehouseId)
                    {
                        case 1:

                            if (Bll.OrderHelper.ValidateStatusReadyToSend(orderId, true, ref msg))
                                statusId = (int)Dal.Helper.OrderStatus.ReadyToSend;
                            else
                                statusId = (int)Dal.Helper.OrderStatus.New;
                            break;
                        case 2:
                            statusId = (int)Dal.Helper.OrderStatus.ClientContact; break;
                    }
                    osh.OrderStatusId = statusId;
                    oh.SetOrderStatus(osh, null);
                }
            }

        }

        bool hasFullView = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            hasFullView = this.HasActionAccess(Guid.Parse("8a6b8ca6-8d38-4eb2-a014-d9133cabc9d8"));

            if (!Page.IsPostBack)
            {
                txbDeliveryDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                BindProducts("",SortDirection.Ascending);
                BindMonths();
            }
            BindProductsLog();
        }

        private void BindMonths()
        {
            List<ListItem> items = new List<ListItem>();
            for (int year = DateTime.Now.Year; year >= 2010; year--)
                for (int month = DateTime.Now.Year == year ? DateTime.Now.Month : 12; month >= 1; month--)
                {
                    ListItem item = new ListItem()
                    {
                        Text = String.Format("{0}/{1}", year, month),
                        Value = String.Format("{0}/{1}/{2}", year, month, 1)
                    };
                    items.Add(item);
                }


            if (hasFullView)
            {
                ddlMonth1.Items.AddRange(items.ToArray());
                ddlMonth2.Items.AddRange(items.ToArray());
            }
            else
            {
                ddlMonth1.Items.AddRange(items.Take(3).ToArray());
                ddlMonth2.Items.AddRange(items.Take(3).ToArray());
            }

            ddlMonth1.SelectedIndex = 1;
            ddlMonth2.SelectedIndex = 1;
        }

        protected void btnNewDelivery_Click(object sender, EventArgs e)
        {

            int[] orderProductIds = WebHelper.GetSelectedIds<int>(gvProductCatalog, "chbOrder");

            if (orderProductIds.Length == 0)
            {
                DisplayMessage("Nie wybrano żadnych produktów");
                return;
            }

            Mode = FormMode.NewDelivery;
            BindLayout();
            BindProducts("",SortDirection.Ascending);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScrollTop", "setTimeout('window.scrollTo(0,0)', 2);", true);


            Dal.OrderHelper pch = new Dal.OrderHelper();

            List<Dal.Company> companies = pch.GetCompanies().Where(x => x.IsActive && x.IsMyCompany).ToList() ;

            ddlCompany.DataSource = companies;
            ddlCompany.DataBind();

        }

        protected void lbtnNewDelivery_Click(object sender, EventArgs e)
        {
            Mode = FormMode.Normal;
            BindLayout();
            BindProducts("",SortDirection.Ascending);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScrollTop", "setTimeout('window.scrollTo(0,0)', 2);", true);

        }
        protected void btnAddDelivery_Click(object sender, EventArgs e)
        {
            int[] ids = WebHelper.GetSelectedIds<int>(gvProductCatalog, "chbOrder");

            if(ids.Length==0)
            {
                DisplayMessage("Zaznacz produkty do dostawy.");
                return;

            }

            List<int> orderProductIds = new List<int>();

         

            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<string> errors = new List<string>();

            Dal.ProductCatalogDeliveryDocument doc = new Dal.ProductCatalogDeliveryDocument()
            {
                CompanyId = Int32.Parse(ddlCompany.SelectedValue),
                DocumentName = txbDeliveryDocument.Text,
                InsertDate = DateTime.Now,
                InsertUser = UserName
            };

            int deliveryDocumentId = h.SetProductCatalogDelivery(doc);


            foreach (GridViewRow row in gvProductCatalog.Rows)
            {
                CheckBox chbOrder = row.FindControl("chbOrder") as CheckBox;
                NewDelivery ucNewDelivery = row.FindControl("ucNewDelivery") as NewDelivery;

                if (chbOrder.Checked)
                {
                    int id = Convert.ToInt32(gvProductCatalog.DataKeys[row.RowIndex][0]);
                    int orderType = Convert.ToInt32(gvProductCatalog.DataKeys[row.RowIndex][1]);
                    int? orderId = null;
                    int productCatalogId=0;
                    int productOrderBatchProductId = 0;
                    switch (orderType)
                    {
                        case 1:
                            Dal.OrderProduct op = oh.GetOrderProduct(id);
                            orderId = op.OrderId;
                            productCatalogId = op.ProductCatalogId.Value;
                            orderProductIds.Add(id);
                            if (op.ProductOrderBatchProductId.HasValue)
                                productOrderBatchProductId = op.ProductOrderBatchProductId.Value;
                            break;
                        case 2:
                            Dal.ProductOrder po= pch.GetProductOrder(id);
                            productCatalogId = po.ProductCatalogId;
                            if (po.ProductOrderBatchProductId.HasValue)
                                productOrderBatchProductId = po.ProductOrderBatchProductId.Value;

                            break;
                    }

                     

                    Dal.ProductCatalogDelivery delivery = new Dal.ProductCatalogDelivery()
                    {
                        Comment = String.Format("Do zamówienia: {0}", orderId),
                        ImportId = null,
                        Price = 0,
                        ProductCatalogId = productCatalogId,
                        Quantity = Convert.ToInt32(ucNewDelivery.Quantity),
                        WarehouseId = Convert.ToInt32(ucNewDelivery.WarehouseId),
                        OrderId = orderId,
                        InsertDate = DateTime.Now,
                        InsertUser = UserName,
                        DeliveryDocumentId= deliveryDocumentId
                    };

                    if (productOrderBatchProductId != 0)
                        delivery.ProductOrderBatchProductId = productOrderBatchProductId;

                    string msg = h.SetDelivery(delivery);
                    if (msg != "")
                        errors.Add(msg);

                    if(ucNewDelivery.HasDeliveryForWarehouse)
                    {
                        Dal.ProductCatalogDelivery deliveryWarehouse = new Dal.ProductCatalogDelivery()
                        {
                            Comment = "",
                            ImportId = null,
                            Price = 0,
                            ProductCatalogId = productCatalogId,
                            Quantity = Convert.ToInt32(ucNewDelivery.QuantityWarehouse),
                            WarehouseId = Convert.ToInt32(ucNewDelivery.WarehouseId),
                            OrderId = null,
                            InsertDate = DateTime.Now,
                            InsertUser = UserName,
                            DeliveryDocumentId = deliveryDocumentId
                        };

                        if (productOrderBatchProductId != 0)
                            deliveryWarehouse.ProductOrderBatchProductId = productOrderBatchProductId;
                        h.SetDelivery(deliveryWarehouse);
                    }


                    switch (orderType)
                    {
                        case 1:
                            Dal.OrderProduct op = oh.GetOrderProduct(id);
                            List<Dal.ProductCatalogDelivery> deliveries = oh.GetProductCatalogDeliveryByOrderId(delivery.OrderId.Value, op.ProductCatalogId.Value);
                            if (op.Quantity <= deliveries.Sum(x=>x.Quantity) && op.OrderProductStatusId == (int)Dal.Helper.OrderProductStatus.Ordered)
                                oh.SetOrderProductsStatus(new int[] { op.OrderProductId }, (int)Dal.Helper.OrderProductStatus.Ready, UserName);
                            break;
                        case 2:
                            Dal.ProductOrder po = pch.GetProductOrder(id);
                            pch.SetProductOrderStatus(id, (int)Dal.Helper.OrderProductStatus.Ready, UserName);

                            break;
                    }



                }
            }
             SetOrderStatusAfterDelivery(orderProductIds.ToArray(), UserName);
            if (errors.Count > 0)
                DisplayMessage(String.Format("Przypisano dodstawy oprócz produktów: <br><br><ul>{0}</ul>", String.Join("<li>", errors.ToArray())));
            else
                DisplayMessage("Przypisano dodstawy");
            lbtnNewDelivery_Click(null, null);
            BindProductsLog();
        }

         

        private void BindLayout()
        { 
            btnNewDelivery.Visible = Mode == FormMode.Normal;
            pnDelivery.Visible=lbtnNewDelivery.Visible = btnAddDelivery.Visible = Mode == FormMode.NewDelivery;

        }

        protected void lbtnOrderProducts_Click(object sender, EventArgs e)
        {
            int[] orderProductIds = WebHelper.GetSelectedIds<int>(gvProductCatalog, "chbOrder");

            if (orderProductIds.Length == 0)
            {
                DisplayMessage("Nie wybrano żadnych produktów");
                return;
            }
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.OrderProductsWaitingForDelivery> products = oh.GetOrdersProductsWaitingForDelivery();

            products = products.Where(x => orderProductIds.Contains(x.OrderProductId)).ToList();

            string s = "";

            foreach (Dal.OrderProductsWaitingForDelivery op in products)
            {
                string code = op.Code;
                string code2 = op.Code2;

                if (!String.IsNullOrEmpty(code2) && code!=code2)
                    code = code + " " + code2;

                s += String.Format("{0} {1} {2}szt.<br>", op.SupplierName, code, op.Quantity);
            }
            DisplayMessage(s);
        }
       
        protected void chblSuppliers_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Mode = FormMode.Normal;
            BindLayout();
            BindProducts("",SortDirection.Ascending);
        }
        protected void chbSupplierOwners_SelectedIndexChanged(object sender, EventArgs e)
        {

            Mode = FormMode.Normal;
            BindLayout();
            BindProducts("",SortDirection.Ascending);
        }
        protected void gvProductCatalog_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.OrderProductsWaitingForDelivery o = e.Row.DataItem as Dal.OrderProductsWaitingForDelivery;
                CheckBox chbOrder = e.Row.FindControl("chbOrder") as CheckBox; 
                HyperLink hlOrder = e.Row.FindControl("hlOrder") as HyperLink;
                Label lblWarehouse = e.Row.FindControl("lblWarehouse") as Label;
                Literal LitId = e.Row.FindControl("LitId") as Literal;

                LitId.Text = String.Format("{0}", e.Row.RowIndex + 1);

                if (o.OrderId.HasValue)
                {
                    hlOrder.NavigateUrl = String.Format(hlOrder.NavigateUrl, o.OrderId);
                    hlOrder.Text = o.OrderId.ToString();
                    lblWarehouse.Text = o.WarehouseName; 
                }

                if (o.WarehouseId == 2)
                    e.Row.BackColor = Color.LightGray;

               if(o.ShippingServiceModeId==(int)Dal.Helper.ShippingServiceMode.ExternalShipping)
                    e.Row.BackColor = Color.Orange;

                if (o.OrderProductStatusId == (int)Dal.Helper.OrderProductStatus.Completed)
                    chbOrder.Visible = false;

                if(Mode == FormMode.NewDelivery && o.OrderProductStatusId != (int)Dal.Helper.OrderProductStatus.Ordered)
                    chbOrder.Visible = false;


                NewDelivery ucNewDelivery = e.Row.FindControl("ucNewDelivery") as NewDelivery;

                ucNewDelivery.BindProductCatalog(warehouse, o.Quantity.Value,  o.WarehouseId);
            }

        }

        private string GetQuantity(int? supplierQuantity)
        {
            if (supplierQuantity.HasValue == false || supplierQuantity.Value == -1)
                return "-";
            else
                return supplierQuantity.ToString();
        }
        private void BindProductsLog()
        {

            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.ProductCatalogDeliveryWarehouseView> products = oh.GetProductCatalogDeliveryWarehouse(DateTime.Parse(txbDeliveryDate.Text));

            var toSend = products.Where(x => x.WarehouseId == (int)Dal.Helper.Warehouse.Przewodnia && x.OrderId.HasValue).ToList();
            var toWarehouse = products.Where(x => x.WarehouseId == (int)Dal.Helper.Warehouse.Przewodnia && !x.OrderId.HasValue).ToList();
            var toPabianicka = products.Where(x => x.WarehouseId == (int)Dal.Helper.Warehouse.Pabianicka).ToList();

            if(rblSupplierOwners.SelectedIndex!=-1)
            {
                int supplierOwnerId = Int32.Parse(rblSupplierOwners.SelectedValue);
                toSend      = toSend     .Where(x=>x.SupplierOwnerId == supplierOwnerId).ToList();
                toWarehouse =toWarehouse .Where(x=>x.SupplierOwnerId == supplierOwnerId).ToList();
                toPabianicka = toPabianicka.Where(x => x.SupplierOwnerId == supplierOwnerId).ToList();


            }

            gvOrdersToSend.DataSource = toSend.OrderBy(x => x.StatusName);
            gvOrdersToSend.DataBind();
            gvOrdersToPabianicka.DataSource = toPabianicka.OrderBy(x => x.StatusName);
            gvOrdersToPabianicka.DataBind();
            gvWarehouse.DataSource = toWarehouse;
            gvWarehouse.DataBind();
        }

        private void BindProducts(string sort, SortDirection direction)
        {
            if (sort != "")
                ViewState["sort"] = sort;
            else
                if (ViewState["sort"] != null)
                sort = ViewState["sort"].ToString();

            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<Dal.OrderProductsWaitingForDelivery> products = oh.GetOrdersProductsWaitingForDelivery();

            products = products.Where(x => x.OrderProductStatusId == (int)Dal.Helper.OrderProductStatus.Ordered).ToList();
       
            int[] supplierOwnersIds = rblSupplierOwners.Items.Cast<ListItem>()
                .Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();
           
            int[] warehouseIds = chbWarehouse.Items.Cast<ListItem>()
                .Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();


            if (Page.IsPostBack)
            { 
                if (supplierOwnersIds.Length > 0)
                    products = products.Where(x => supplierOwnersIds.Contains(x.SupplierOwnerId)).ToList();
                if (warehouseIds.Length > 0)
                    products = products.Where(x => x.WarehouseId.HasValue && warehouseIds.Contains(x.WarehouseId.Value)).ToList();
 
            }
            else
            {
                var s = products.Select(x => new
                {
                    SupplierId = x.SupplierId,
                    SupplierName = x.SupplierName

                }).Distinct().ToList();

                var so = products.Select(x => new
                {
                    SupplierOwnerId = x.SupplierOwnerId,
                    Name = x.SupplierOwnerName

                }).Distinct().ToList();

                var w = products.Select(x => new
                {
                    WarehouseId = x.WarehouseId,
                    Name = x.WarehouseName

                }).Distinct().ToList();
                rblSupplierOwners.DataSource = so.OrderBy(x => x.Name).ToList();
                rblSupplierOwners.DataBind();
           
               

                chbWarehouse.DataSource = w;
                chbWarehouse.DataBind();
            }
            if (Mode == FormMode.NewDelivery)
            {

                int[] orderProductIds = WebHelper.GetSelectedIds<int>(gvProductCatalog, "chbOrder");
                products = products.Where(x => orderProductIds.Contains(x.OrderProductId)).ToList();
            }
            gvProductCatalog.Columns[9].Visible = Mode == FormMode.NewDelivery;


            switch (sort)
            {
                case "product": gvProductCatalog.DataSource = products.OrderBy(x => x.Name).ToList(); break;
                case "code": gvProductCatalog.DataSource = products.OrderBy(x => x.Code).ToList(); break;
                case "order": gvProductCatalog.DataSource = products.OrderBy(x => x.OrderId).ToList(); break;
                case "supplier": gvProductCatalog.DataSource = products.OrderBy(x => x.SupplierName).ToList(); break;
                case "status": gvProductCatalog.DataSource = products.OrderBy(x => x.StatusName).ToList(); break;
                case "date": gvProductCatalog.DataSource = products.OrderBy(x => x.DeliveryDate).ToList(); break;
                case "dateOrder":
                    gvProductCatalog.DataSource = products.OrderBy(x => x.OrderDate).ToList(); break;
                default: gvProductCatalog.DataSource = products.OrderBy(x => x.Code).ToList(); break;

            }
            if (rblSupplierOwners.SelectedIndex == -1)
                gvProductCatalog.DataSource = null;



            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            warehouse = pch.GetWarehouse().Where(x => x.CanAssignDelivery).ToList();


            gvProductCatalog.DataBind(); 

        }


        protected void gvProductCatalog_Sorting(object sender, GridViewSortEventArgs e)
        {
            BindProducts(e.SortExpression, e.SortDirection);
        }

        protected void btnChangeDate_Click(object sender, EventArgs e)
        {
            BindProductsLog();
        }

        protected void btn13ChangeDate_Click(object sender, EventArgs e)
        {
            if (!ddlMonth1.SelectedValue.Equals("") && !ddlMonth2.SelectedValue.Equals(""))
            {
                Bll.SalesFileHelper.GenerateWarehouseDeliveryReport(DateTime.Parse(ddlMonth1.SelectedValue), DateTime.Parse(ddlMonth2.SelectedValue));

                DisplayMessage("Raporty zostały wysłane na maila");
            }
            else
                DisplayMessage("Obydwa pola z datami muszą być wypełnione!");
        }
    }
}