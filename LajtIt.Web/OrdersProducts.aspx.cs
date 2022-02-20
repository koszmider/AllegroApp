using LajtIt.Web.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("7d6f5e08-07b8-476f-8c98-07b7a232eb2b")]
    public partial class OrdersProducts : LajtitPage
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            ucOrderProductsEmail.Send += SendEmail;
            ucOrderProductsEmail.Cancel += CancelEmail;


            if (!Page.IsPostBack)
            {
                BindProducts("",SortDirection.Ascending);
                CheckForUnProcessedProducts();
            }
            //else
            //    if (tmRunOnce.Enabled)
            //{
            //    tmRunOnce.Enabled = false;
            //    chblSuppliers_OnSelectedIndexChanged(sender, e);
            //}
        }

        private void CheckForUnProcessedProducts()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<Dal.OrdersUprocessed> orders = oh.GetOrdersUnprocessed();

            if(orders.Count>0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Lista zamówień, które muszą być przeprocosowane (zmiana statusu zamówienia lub statusu produktu). Obecnie wszystkie <b>produkty</b> z tego zamówienia mają status, który pozwala na przestawienie statusu <b>zamówienia</b>.<br><br>");

                foreach(Dal.OrdersUprocessed order in orders)
                {
                    sb.AppendLine(String.Format(@"<a href='/Order.aspx?id={0}' target='_blank'>{0}</a> ", order.OrderId));
                }
                DisplayMessage(sb.ToString());
            }


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

        }

        protected void lbtnNewDelivery_Click(object sender, EventArgs e)
        {
            Mode = FormMode.Normal;
            BindLayout();
            BindProducts("",SortDirection.Ascending);

        }
       

       

        private void BindLayout()
        {
            ddlOrderProductStatus.Visible = btnChangeStatus.Visible = Mode == FormMode.Normal; 
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
            //string t = "";


            var sells = products
.GroupBy(a => a.ProductCatalogId)
.Select(a => new { Quantity = a.Sum(b => b.Quantity), ProductCatalogId = a.Key })
.OrderByDescending(a => a.Quantity)
.ToList();



            foreach (var op in sells)
            {
                string code = products.Where(x => x.ProductCatalogId == op.ProductCatalogId).Select(x => x.Code).FirstOrDefault();
                string supplierName = products.Where(x => x.ProductCatalogId == op.ProductCatalogId).Select(x => x.SupplierName).FirstOrDefault();
           

            

                s += String.Format("{0} {1} {2}szt.<br>", supplierName, code, op.Quantity);
               // t += String.Format("<tr><td>{0}</td><td>{1}</td></tr>", code, op.Quantity);
            }

            //string table = "<table><tr><td>{0}</td><td><table border=1>{1}</table></td></tr></table>";



            DisplayMessage(s);
            //DisplayMessage(String.Format(table, s, t));
        }
        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {

        


            //int[] orderProductIds = WebHelper.GetSelectedIds(gvProductCatalog, "chbOrder");

            List<int> products = new List<int>();
            List<int> orders = new List<int>();

            foreach (GridViewRow row in gvProductCatalog.Rows)
            {
                CheckBox chbOrder = row.FindControl("chbOrder") as CheckBox;
                if (chbOrder.Checked)
                {
                    int id = Convert.ToInt32(gvProductCatalog.DataKeys[row.RowIndex][0]);
                    int orderType = Convert.ToInt32(gvProductCatalog.DataKeys[row.RowIndex][1]);
                    switch(orderType)
                    {
                        case 1: orders.Add(id); break;
                        case 2: products.Add(id); break;

                    }
                }
            } 



            if (orders.Count+ products.Count==0)
            {
                DisplayMessage("Nie wybrano żadnych produktów");
                return;
            }
            int[] supplierIds = chbSupplierOwners.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();

            if (Int32.Parse(ddlOrderProductStatus.SelectedValue) == (int)Dal.Helper.OrderProductStatus.Ordered
            && supplierIds.Length != 1)
            {
                DisplayMessage("Zmiana statusu na <b>Zamówiony</b> jest możliwa tylko dla jednego dostawcy na raz. ");
                return;

            }


            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetOrderProductsStatus(orders.ToArray(), Convert.ToInt32(ddlOrderProductStatus.SelectedValue), UserName);
            oh.SetProductOrdersStatus(products.ToArray(), Convert.ToInt32(ddlOrderProductStatus.SelectedValue), UserName);


            if (Int32.Parse(ddlOrderProductStatus.SelectedValue) == (int)Dal.Helper.OrderProductStatus.Ordered
&& supplierIds .Length== 1)
            {
                oh.SetProductOrderBatch(orders.ToArray(), products.ToArray(), Int32.Parse(chbSupplierOwners.SelectedValue) , UserName);


            }


            DisplayMessage(String.Format("Ustawiono status {0} dla {1} produktów", ddlOrderProductStatus.SelectedItem.Text,
                orders.Count + products.Count));
            BindProducts("",SortDirection.Ascending);


        }
        protected void chblSuppliers_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindProducts("",SortDirection.Ascending);
        }
        protected void chbSupplierOwners_SelectedIndexChanged(object sender, EventArgs e)
        {
            chblSupplier.Items.Clear();
            BindProducts("",SortDirection.Ascending);
        }
        protected void gvProductCatalog_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.OrderProductsWaitingForDelivery o = e.Row.DataItem as Dal.OrderProductsWaitingForDelivery;
                CheckBox chbOrder = e.Row.FindControl("chbOrder") as CheckBox;
                CheckBox chbIsAvailable = e.Row.FindControl("chbIsAvailable") as CheckBox;
                HyperLink hlOrder = e.Row.FindControl("hlOrder") as HyperLink;
                Label lblWarehouse = e.Row.FindControl("lblWarehouse") as Label;
                Label lblQuantity = e.Row.FindControl("lblQuantity") as Label;
                Label lblStatus = e.Row.FindControl("lblStatus") as Label;
                LinkButton lbtnOrderBatch = e.Row.FindControl("lbtnOrderBatch") as LinkButton;

                lblStatus.Text = o.StatusName;

                if(o.ProductOrderBatchId.HasValue && o.OrderProductStatusId==(int)Dal.Helper.OrderProductStatus.Ordered)
                {
                    lbtnOrderBatch.Text = String.Format("{0:yyyy/MM/dd}-{1}",   o.BatchInsertDate, o.ProductOrderBatchId);
                    lbtnOrderBatch.CommandArgument = o.ProductOrderBatchId.ToString();
                }

                if (o.OrderId.HasValue)
                {
                    hlOrder.NavigateUrl = String.Format(hlOrder.NavigateUrl, o.OrderId);
                    hlOrder.Text = o.OrderId.ToString();
                    lblWarehouse.Text = o.WarehouseName;

                    chbIsAvailable.Checked = o.IsAvailable;
                    //if (o.SupplierQuantity.HasValue == false || o.SupplierQuantity.Value == -1)
                    //    lblQuantity.Visible = false;
                    //else
                        lblQuantity.Text = String.Format("({0}|{1})", o.LeftQuantity+o.Quantity, GetQuantity(o.SupplierQuantity));
                }

                if(o.SendFromExternalWerehouse.Value==1 )
               // if (o.WarehouseId.HasValue && o.WarehouseId.Value == 3)
                    e.Row.BackColor = Color.Pink;
                if (o.ShopId.HasValue  && o.ShopTypeId.Value==(int)Dal.Helper.ShopType.Allegro)
                    e.Row.BackColor = Color.Orange;


                if (o.OrderProductStatusId == (int)Dal.Helper.OrderProductStatus.Completed)
                    chbOrder.Visible = false;

                if(Mode == FormMode.NewDelivery && o.OrderProductStatusId != (int)Dal.Helper.OrderProductStatus.Ordered)
                    chbOrder.Visible = false;

 
            }

        }

        private string GetQuantity(int? supplierQuantity)
        {
            if (supplierQuantity.HasValue == false || supplierQuantity.Value == -1)
                return "-";
            else
                return supplierQuantity.ToString();
        }

        private void BindProducts(string sort, SortDirection direction)
        {
            if (sort != "")
                ViewState["sort"] = sort;
            else
                if (ViewState["sort"] != null)
                sort = ViewState["sort"].ToString();


            int supplierOwnerId = 0;

            int[] selectedSupplierIds = chblSupplier.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();

            if (chbSupplierOwners.SelectedIndex != -1)
            {
                supplierOwnerId = Int32.Parse(chbSupplierOwners.SelectedValue);

                chblSupplier.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliersByOwner(supplierOwnerId);
                chblSupplier.DataBind();
            }
            chblSupplier.Visible = chblSupplier.Items.Count > 1;




            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<Dal.OrderProductsWaitingForDelivery> products = oh.GetOrdersProductsWaitingForDelivery();
            if (!Page.IsPostBack)
            {
                var w = products.Select(x => new
                {
                    WarehouseId = x.WarehouseId,
                    Name = x.WarehouseName

                }).Distinct().ToList();

                var statuses = oh.GetOrderProductStauses();
                chbOps.DataSource = statuses;
                chbOps.DataBind();
                chbOps.SelectedIndex = chbOps.Items.IndexOf(chbOps.Items.FindByValue("1"));
                ddlOrderProductStatus.DataSource = statuses;
                ddlOrderProductStatus.DataBind();

                chbWarehouse.DataSource = w;
                chbWarehouse.DataBind();
            }



            int[] selectedSupplierOwnerIds = chbSupplierOwners.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();

            var so = products.Select(x => new
            {
                x.SupplierOwnerId,
                Name = x.SupplierOwnerName

            }).Distinct().OrderBy(x => x.Name).ToList();



            int[] orderProductStatusIds = chbOps.Items.Cast<ListItem>()
                .Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();

            int[] warehouseIds = chbWarehouse.Items.Cast<ListItem>()
                .Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();


            if (orderProductStatusIds.Length > 0)
                products = products.Where(x => orderProductStatusIds.Contains(x.OrderProductStatusId)).ToList();


            if (warehouseIds.Length > 0)
                products = products.Where(x => x.WarehouseId.HasValue && warehouseIds.Contains(x.WarehouseId.Value)).ToList();

            switch(rblSendFromOurWarehouse.SelectedIndex)
            {
                case 1:
                    products = products.Where(x => x.SendFromExternalWerehouse.Value == 0).ToList(); break;
                case 2:
                    products = products.Where(x => x.SendFromExternalWerehouse.Value == 1).ToList(); break;
            }


            if (chbNotReady.Checked)
            {
                products = products.Where(x => x.OrderStatusId != (int)Dal.Helper.OrderStatus.WaitingForDelivery).ToList();
                chbNotReady.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                products = products.Where(x => x.OrderStatusId == (int)Dal.Helper.OrderStatus.WaitingForDelivery).ToList();
                chbNotReady.ForeColor = System.Drawing.Color.Black;
            }

            chbSupplierOwners.Items.Clear();

            foreach (var supplierOwner in so)
            {
                int count = products.Where(x => x.SupplierOwnerId == supplierOwner.SupplierOwnerId).Count();
                bool hasAmazon = products.Where(x => x.SupplierOwnerId == supplierOwner.SupplierOwnerId && x.ShopTypeId == (int)Dal.Helper.ShopType.Allegro).Count() > 0;

                bool selected = selectedSupplierOwnerIds.Contains(supplierOwner.SupplierOwnerId);

                string tmp = "{0} ({1})";
                if (count > 0)
                    tmp = "<b>{0} ({1})</b>";

                if (hasAmazon)
                    tmp = String.Format("<span style='color:orange'>{0}</span>", tmp);

                tmp = String.Format(tmp, supplierOwner.Name, count);

                ListItem li = new ListItem(String.Format("{0}", tmp), supplierOwner.SupplierOwnerId.ToString());

                if (selected)
                    li.Selected = true;

                if (count == 0)
                    li.Enabled = false;
             
                chbSupplierOwners.Items.Add(li);
            }

             

            if (chblSupplier.Items.Count > 0)
                foreach (ListItem item in chblSupplier.Items)
                {
                    int count = products.Where(x => x.SupplierId == Int32.Parse(item.Value)).Count();

                    bool hasAmazon = products.Where(x => x.SupplierId == Int32.Parse(item.Value) && x.ShopTypeId == (int)Dal.Helper.ShopType.Allegro).Count() > 0;

                    string tmp = "{0} ({1})";
                    if (count > 0)
                        tmp = "<b>{0} ({1})</b>";

                    if (hasAmazon)
                        tmp = String.Format("<span style='color:orange'>{0}</span>", tmp);

                    tmp = String.Format(tmp, item.Text, count);

                    item.Text = String.Format("{0}", tmp);

                    if (selectedSupplierIds.Contains(Int32.Parse(item.Value)))
                        item.Selected = true;

                    if (count == 0)
                        item.Enabled = false;

                }



            int[] supplierOwnersIds = chbSupplierOwners.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();

            if (supplierOwnersIds.Length > 0)
                products = products.Where(x => supplierOwnersIds.Contains(x.SupplierOwnerId)).ToList();

            if (chblSupplier.Items.Count > 0 && selectedSupplierIds.Length > 0 && chblSupplier.Items.Count != selectedSupplierIds.Length)
                products = products.Where(x => selectedSupplierIds.Contains(x.SupplierId)).ToList();

            if (chbDeliveryDateAhead.Checked)
                    products = products.Where(x => x.DeliveryDate.HasValue && x.DeliveryDate.Value.AddDays(-2) <= DateTime.Now).ToList();


      
            if (Mode == FormMode.NewDelivery)
            {

                int[] orderProductIds = WebHelper.GetSelectedIds<int>(gvProductCatalog, "chbOrder");
                products = products.Where(x => orderProductIds.Contains(x.OrderProductId)).ToList();
            }
             


            if (Page.IsPostBack)
            {
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
                    default: gvProductCatalog.DataSource = products.OrderBy(x => x.Name).ToList(); break;

                }

                gvProductCatalog.DataBind();
            }


            bool canOrder = chbOps.SelectedValue == "1" && chbOps.Items.Cast<ListItem>().Where(x => x.Selected).Count() == 1;

            //if (chbSupplierOwners.Items.Cast<ListItem>().Where(x => x.Selected).Count() == 1 && canOrder)
            //    lbtnOrderProductsEmail.Visible = true;
            //else
            //{
            //    lbtnOrderProductsEmail.Visible = false;
            //    ucOrderProductsEmail.Visible = false;
            //}

            //if (UserName == "agata")
            //{
            //    lbtnOrderProductsEmail.Visible = false;
            //    ucOrderProductsEmail.Visible = false;
            //}
        }


        protected void gvProductCatalog_Sorting(object sender, GridViewSortEventArgs e)
        {
            BindProducts(e.SortExpression, e.SortDirection);
        }


        protected void lbtnOrderProductsEmail_Click(object sender, EventArgs e)
        {
            int[] orderProductIds = WebHelper.GetSelectedIds<int>(gvProductCatalog, "chbOrder");

            if (orderProductIds.Length == 0)
            {
                DisplayMessage("Nie wybrano żadnych produktów");
                return;
            }
            bool canOrder = chbOps.SelectedValue == "1" && chbOps.Items.Cast<ListItem>().Where(x => x.Selected).Count() == 1;
            if (!canOrder)
            {
                DisplayMessage("Produkty można zamawiać tylko w statusie Nowy");
                return;
            }


            ucOrderProductsEmail.Visible = true;
            int supplierId = Convert.ToInt32(chbSupplierOwners.Items.Cast<ListItem>().Where(x => x.Selected).FirstOrDefault().Value);
            ucOrderProductsEmail.LoadForm(supplierId, orderProductIds);
        }

        private void SendEmail(object sender, EventArgs e)
        {

            int[] orderProductIds = WebHelper.GetSelectedIds<int>(gvProductCatalog, "chbOrder");


            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetOrderProductsStatus(orderProductIds, (int)Dal.Helper.OrderProductStatus.Ordered, UserName);
            DisplayMessage(String.Format("Ustawiono status {0} dla {1} produktów oraz wysłano email z zamówieniem", ddlOrderProductStatus.SelectedItem.Text,
                orderProductIds.Count()));
            BindProducts("",SortDirection.Ascending);

            ucOrderProductsEmail.Visible = false;
        }
        private void CancelEmail(object sender, EventArgs e)
        {
            ucOrderProductsEmail.Visible = false;
        }

        protected void lbtn_Click(object sender, EventArgs e)
        {
            Bll.OrderHelper.GetOrdersToComplete();
            DisplayMessage("Raport wysłany na maila");
        }

        protected void lbtnOrderProductsToFile_Click(object sender, EventArgs e)
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


            //int[] productsOrdered = products.Select(x => x.ProductCatalogId.Value).Distinct().ToArray();

            // produkty zamawiane po raz pierwszy
            //int[] productCatalogOrderedId = Dal.DbHelper.ProductCatalog.GetProductCatalogDeliveries();

            // twórz plik z produktami zamawianymi pierwszy raz

            string f1 = CreateFile1(products);
            //string f2 = CreateFile2(products, productCatalogOrderedId);

 
            if (f1 != null)
            {
                hlFile1.NavigateUrl = @"/Files/ExportFiles/"+Path.GetFileName(f1);
                hlFile1.Visible = true;
            }
            else
                hlFile1.Visible = false;


            //if (f2 != null)
            //{
            //    hlFile2.NavigateUrl = @"/Files/ExportFiles/" + Path.GetFileName(f2);
            //    hlFile2.Visible = true;
            //}
            //else
            //    hlFile2.Visible = false;

            mpeFiles.Show();

            //foreach (Dal.OrderProductsWaitingForDelivery op in products)
            //{
            //    string code = op.Code; 

            //    sb.AppendLine(String.Format("{0},{1}", code, op.Quantity));
            //}

            //string contentType = contentType = "Application/csv";

            //Response.ContentType = contentType;
            //Response.ContentEncoding = Encoding.UTF8;
            //Response.AppendHeader("content-disposition", String.Format("attachment; filename=Zamowienie_{0:yyyyMMddHH:mm}.csv", DateTime.Now));
            //Response.Write(sb.ToString());
            //Response.End();

        }
    //    private string CreateFile2(List<Dal.OrderProductsWaitingForDelivery> products, int[] productCatalogOrderedId)
    //    {
    //        var sells = products
    //.GroupBy(a => a.ProductCatalogId)
    //.Select(a => new { Quantity = a.Sum(b => b.Quantity), ProductCatalogId = a.Key })
    //.OrderByDescending(a => a.Quantity)
    //.ToList();



    //        StringBuilder sb = new StringBuilder();
    //        sb.AppendLine("kod produktu,ilość");

    //        int[] productsOrderedFirstTimeId = 
    //            products.Where(x => !productCatalogOrderedId.Contains(x.ProductCatalogId.Value)).Select(x=>x.ProductCatalogId.Value).Distinct().ToArray();

    //        Dictionary<int, int> pr = new Dictionary<int, int>();
    //        foreach (var s in sells)
    //        {

    //            if (productsOrderedFirstTimeId.Where(x => x == s.ProductCatalogId.Value).Count() > 0)
    //                pr.Add(s.ProductCatalogId.Value, s.Quantity.Value-1);
    //            else
    //                pr.Add(s.ProductCatalogId.Value, s.Quantity.Value);
                
    //        }

    //        bool exists = false;

    //        foreach ( KeyValuePair<int,int> k in pr.Where(x=>x.Value>0))
    //        {
    //            exists = true;

    //            var s = products.Where(x => x.ProductCatalogId.Value == k.Key).FirstOrDefault();

    //            sb.AppendLine(String.Format("{0},{1}", s.Code, k.Value));
    //        }



    //        if (!exists)
    //            return null;

    //        string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];


    //        string saveLocation = String.Format(path, String.Format("AZzardo_{0:yyyyMMddHHmmss}_zamowienie.csv", DateTime.Now));


    //        System.IO.File.WriteAllText(saveLocation, sb.ToString());



    //        return saveLocation;
    //    }
        private string CreateFile1(List<Dal.OrderProductsWaitingForDelivery> products)
        {
            if (products.Count == 0)
                return null;

            StringBuilder sb = new StringBuilder();

            //List<Dal.OrderProductsWaitingForDelivery> p = products.Where(x => !productCatalogOrderedId.Contains(x.ProductCatalogId.Value)).ToList();

            bool exists = false;

            if(products[0].SupplierId == 14 || products[0].SupplierId == 13) // Nowodvorski
                sb.AppendLine("Code;Quantity;Unit");

            if (products[0].SupplierId == 8) // AZzardo
                sb.AppendLine("kod produktu,ilość");

            foreach (Dal.OrderProductsWaitingForDelivery product in products)
            {
                exists = true;

                if (products[0].SupplierId == 14  ) // Nowodvorski
                    sb.AppendLine(String.Format("{0};{1};", product.Ean, product.Quantity));
                if  ( products[0].SupplierId == 13) // Nowodvorski
                    sb.AppendLine(String.Format("{0};{1};", product.Code, product.Quantity));//Milagro

                if (products[0].SupplierId == 8) // AZzardo
                    sb.AppendLine(String.Format("{0},{1}", product.Code, product.Quantity));
            }


            if (!exists)
                return null;

            string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}",    Dal.Helper.Env.ToString())];


            string saveLocation = String.Format(path, String.Format("{1}_{0:yyyyMMddHHmmss}.csv", DateTime.Now, products[0].SupplierName));


            System.IO.File.WriteAllText(saveLocation, sb.ToString());



            return saveLocation;
        }

        protected void lbtnOrderBatch_Click(object sender, EventArgs e)
        {
            int batchId = Int32.Parse(((LinkButton)sender).CommandArgument);

            Dal.OrderHelper oh = new Dal.OrderHelper();

             List<Dal.ProductOrderDeliveryView> productOrders = oh.GetProductOrderDelivery(batchId);

            gvProductOrder.DataSource = productOrders;
            gvProductOrder.DataBind();

            mpeProductOrders.Show();
        }
    }
}