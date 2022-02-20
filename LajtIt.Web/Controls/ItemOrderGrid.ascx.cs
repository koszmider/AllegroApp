using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using LajtIt.Bll;
using System.Linq;
using System.Web.UI;
using System.Drawing;

namespace LajtIt.Web.Controls
{
    public partial class ItemOrderGrid : LajtitControl
    {
        private decimal total = 0;
        private int count = 0;
        private List<Dal.ProductCatalogSubProductsView> subProducts;
        private List<Dal.OrderProduct> subOrderProducts;
 
        private int OrderId
        {
            set { ViewState["OrderId"] = value; }
            get { return Convert.ToInt32(ViewState["OrderId"]); }
        }
        public int[] Suppliers
        { 
            set { ViewState["Suppliers"] = value; }
        }
        private int[] GetSuppliers()
        {
            if (ViewState["Suppliers"] != null)
                return (int[])ViewState["Suppliers"];
            return null;
        }
        private bool AllowEditProducts
        {
            get
            {
                return Convert.ToBoolean(ViewState["AllowEditProducts"]);
            }
            set
            {
                ViewState["AllowEditProducts"] = value;
            }
        }


        public delegate void SavedEventHandler(object sender, bool amoutChanged);
        public event SavedEventHandler Saved;

        Dal.Order order;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //protected void ddlSupplier_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int supplierId = Convert.ToInt32(((DropDownList)sender).SelectedValue);

        //    DropDownList ddlProductCatalog = gvUserOrders.Rows[gvUserOrders.EditIndex].FindControl("ddlProductCatalog") as DropDownList;

        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    BindProductCatalogList(oh, supplierId, ddlProductCatalog, null, true);

        //}
        //protected void lbtnShowAllProducts_Click(object sender, EventArgs e)
        //{

        //    int supplierId = Convert.ToInt32(
        //        ((DropDownList)gvUserOrders.Rows[gvUserOrders.EditIndex].FindControl("ddlSupplier")).SelectedValue
        //        );

        //    DropDownList ddlProductCatalog = 
        //        gvUserOrders.Rows[gvUserOrders.EditIndex].FindControl("ddlProductCatalog") as DropDownList;

        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    string selectedProduct = ddlProductCatalog.SelectedValue;
        //    BindProductCatalogList(oh, supplierId, ddlProductCatalog, null, false);
        //    ddlProductCatalog.SelectedIndex = ddlProductCatalog.Items.IndexOf(ddlProductCatalog.Items.FindByValue(selectedProduct));

        //}
        //private void BindProductCatalogList(Dal.OrderHelper oh, int supplierId, 
        //    DropDownList ddlProductCatalog, int? doNotExcludeProductId,
        //    bool prefilterProducts)
        //{
        //    List<Dal.ProductCatalogView> products = oh.GetProductCatalogsForOrder(OrderId, supplierId, doNotExcludeProductId);

        //    ddlProductCatalog.Items.Clear();

        //    if (prefilterProducts && ViewState["ItemCodeFromAuction"]!=null)
        //    {

        //        string code = ViewState["ItemCodeFromAuction"].ToString();

               
        //            products = products.Where(x => x.Code != null && x.Code.StartsWith(code, StringComparison.InvariantCultureIgnoreCase))
        //                .ToList();

                
        //    }
        //    ddlProductCatalog.DataSource =
        //          products
        //            .OrderBy(x => x.Code).Select(x => new
        //          {
        //              ProductCatalogId = x.ProductCatalogId,
        //              Name = String.Format("{0}|{1}|{2}|{3}",
        //                   LajtIt.Web.Controls.Products.SetColumn(x.Code, 20, 1),
        //              LajtIt.Web.Controls.Products.SetColumn(x.Name, 50, 1),
        //               LajtIt.Web.Controls.Products.SetColumn(x.PriceBruttoFixed.ToString("C"), 12, 2),
        //                   LajtIt.Web.Controls.Products.SetColumn(String.Format("{0} szt. w mag.", x.LeftQuantity), 20, 1))
        //          }).ToList();
        //    ddlProductCatalog.DataBind();
        //    ddlProductCatalog.Items.Insert(0, new ListItem());

        //    LajtIt.Web.Controls.Products.DecodeDropDownList(ddlProductCatalog);
        
        //}

        public decimal BindProducts(int orderId, bool allowEditProducts, int[] suppliersIds)
        {
            AllowEditProducts = allowEditProducts;
            OrderId = orderId;
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.OrderProductsView> orderProducts =
                oh.GetOrderProducts(OrderId);
            if (suppliersIds.Length > 0)
                orderProducts = orderProducts.Where(x =>
                    x.SupplierId.HasValue
                  && suppliersIds.Contains(x.SupplierId.Value)).ToList();

            if (ViewState["OrderProductStatusId"]  !=null && Convert.ToInt32(ViewState["OrderProductStatusId"] )> -1)
                orderProducts = orderProducts.Where(x => x.OrderProductStatusId == Convert.ToInt32(ViewState["OrderProductStatusId"])).ToList();
            order = Dal.DbHelper.Orders.GetOrder(OrderId);
            gvUserOrders.DataSource = orderProducts.OrderByDescending(x=>x.IsOrderProduct).ThenBy(x=>x.OrderSorting).ThenBy(x=>x.ProductTypeId).ThenBy(x=>x.StatusName);
            gvUserOrders.DataBind();

            Suppliers = suppliersIds;

            return total;
        }

        int counter = 1;
        protected void gvUserOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                counter = 1;

                if (!AllowEditProducts)
                    gvUserOrders.Columns[0].Visible = false;
                total = count = 0;

                if(ViewState["OrderProductStatusId"] != null && Convert.ToInt32(ViewState["OrderProductStatusId"]) > -1)
                {
                    DropDownList ddlStatusName = e.Row.FindControl("ddlStatusName") as DropDownList;
                    ddlStatusName.SelectedValue = ViewState["OrderProductStatusId"].ToString();
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litCounter = e.Row.FindControl("litCounter") as Literal;
                Literal litStatusName = e.Row.FindControl("litStatusName") as Literal;
                Label lbPriceAmount = e.Row.FindControl("lbPriceAmount") as Label;
                Label lbPriceAmountTotal = e.Row.FindControl("lbPriceAmountTotal") as Label;
                CheckBox chbOrder = e.Row.FindControl("chbOrder") as CheckBox;

                


                #region
                if (!AllowEditProducts)
                    gvUserOrders.Columns[0].Visible = false;

                HyperLink hlProductCatalogName = e.Row.FindControl("hlProductCatalogName") as HyperLink;
                Literal litVAT = e.Row.FindControl("litVAT") as Literal;
                Dal.OrderProductsView orderProduct = e.Row.DataItem as Dal.OrderProductsView;


                    if (orderProduct.ShippingCurrencyCode != "PLN")
                {
                    lbPriceAmount.Visible = true;
                    lbPriceAmount.Text = String.Format("{0:0.00} {1}", orderProduct.PriceCurrency, orderProduct.ShippingCurrencyCode);
                    lbPriceAmountTotal.Text = String.Format("{0:0.00} {1}", (orderProduct.PriceCurrency - orderProduct.PriceCurrency * orderProduct.Rebate / 100.00M) * orderProduct.Quantity, orderProduct.ShippingCurrencyCode);

                }


                    litStatusName.Text = orderProduct.StatusName;


                if (orderProduct.ProductTypeId == (int)Dal.Helper.ProductType.ComboProduct)
                {
                    int[] excludedCells = new int[] { 2,4 };
                    for (int i = 0; i < e.Row.Cells.Count - 1; i++)
                    {
                        if (excludedCells.Contains(i))
                            continue;
                        foreach (Control control in e.Row.Cells[i].Controls)
                            control.Visible = false;
                    }
                }
                else
                {
                    litCounter.Text = counter.ToString();
                    counter++;
                }
                if (orderProduct.Quantity == 0)
                    e.Row.CssClass = "grayedout";
                else
                if (orderProduct.LeftQuantity < 0 )
                {
                    if (orderProduct.ProductCatalogId.HasValue)
                    {
                        Dal.ProductCatalogHelper ph = new Dal.ProductCatalogHelper();
                        int quantityDeliveredForOrder = ph.GetProductCatalogDeliveryForOrder(orderProduct.ProductCatalogId.Value, orderProduct.OrderId);
                        if (orderProduct.Quantity > quantityDeliveredForOrder)
                        {
                            e.Row.Cells[4].BackColor = Color.LightPink;
                            e.Row.Cells[4].ToolTip = "Brak w magazynie";
                        }
                    }
                }
                if (litVAT != null)
                    litVAT.Text = String.Format("{0:0.00}%", orderProduct.VAT * 100M);

                if (hlProductCatalogName != null && orderProduct.ProductCatalogId.HasValue && orderProduct.IsOrderProduct==1)
                {

                    hlProductCatalogName.Text = String.Format("{0} ({1})",  orderProduct.CatalogName, orderProduct.Code);
                    hlProductCatalogName.NavigateUrl = String.Format(hlProductCatalogName.NavigateUrl, orderProduct.ProductCatalogId);

                }
                HyperLink hlExternalProduct = e.Row.FindControl("hlExternalProduct") as HyperLink;
                if (hlExternalProduct != null &&  orderProduct.ShopTypeId==(int)Dal.Helper.ShopType.Allegro && orderProduct.ExternalProductId!=0)
                {
                    hlExternalProduct.Text = String.Format("{0}<br>{1}", orderProduct.ExternalProductId, orderProduct.CategoryName);
                    hlExternalProduct.NavigateUrl = String.Format("http://allegro.pl/show_item.php?item={0}", orderProduct.ExternalProductId);

                }
                if (hlExternalProduct != null && orderProduct.ShopTypeId == (int)Dal.Helper.ShopType.Undefined)
                {
                    hlExternalProduct.Text = orderProduct.ExternalProductId.ToString();
                    hlExternalProduct.NavigateUrl = null;
                }
                if (orderProduct.IsOrderProduct == 0)
                {
                    chbOrder.Visible = false;
                }
                    #endregion
                    if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {



                    Panel pnStatus = e.Row.FindControl("pnStatus") as Panel;
                    if (orderProduct.IsOrderProduct==0)
                    {
                        chbOrder.Visible = false;
                        pnStatus.Visible = false;
                        e.Row.Cells[4].Enabled = false;
                        e.Row.Cells[5].Enabled = false;
                    }
                    else
                        pnStatus.Visible = true;

                    DropDownList ddlOrderProductStatus = e.Row.FindControl("ddlOrderProductStatus") as DropDownList;
                    DropDownList ddlSupplier = e.Row.FindControl("ddlSupplier") as DropDownList;
                    TextBox txbProductCode = e.Row.FindControl("txbProductCode") as TextBox;
                    HiddenField hfProductCatalogId = e.Row.FindControl("hfProductCatalogId") as HiddenField;

                    System.Web.UI.ScriptManager.
                  RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString("N"),
                  String.Format("goAutoComplProduct('{0}','{1}','{2}');",
                  txbProductCode.ClientID,
                  ddlSupplier.ClientID,
                  hfProductCatalogId.ClientID)
                  , true);


                    RequiredFieldValidator rfvPc = e.Row.FindControl("rfvPc") as RequiredFieldValidator;

                     
                    Dal.OrderHelper oh = new Dal.OrderHelper();
                    ddlOrderProductStatus.DataSource = oh.GetOrderProductStauses();
                    ddlOrderProductStatus.DataBind();
                    ddlOrderProductStatus.SelectedValue = orderProduct.OrderProductStatusId.ToString();

                    ddlSupplier.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderBy(x=>x.Name);
                    ddlSupplier.DataBind();
                    ddlSupplier.SelectedValue = "3";

                    if (orderProduct.ExternalProductId != null && orderProduct.ExternalProductId != 0 && orderProduct.ShopTypeId == (int)Dal.Helper.ShopType.Allegro)
                    {
                        Dal.ProductCatalog pcExtItem = oh.GetProductCatalogByExternalProductId(orderProduct.ExternalProductId.Value);

                        if (pcExtItem != null)
                        {
                            Label lblItemCodeFromAuction = e.Row.FindControl("lblItemCodeFromAuction") as Label;
                            lblItemCodeFromAuction.Text = String.Format("Kod produktu z aukcji: <b>{0}</b>", pcExtItem.Code);
                           
                            ViewState["ItemCodeFromAuction"] = pcExtItem.Code;
                        }
                    }
                    


                    if (orderProduct.ProductCatalogId != null)
                    {
                        ddlSupplier.SelectedValue = orderProduct.SupplierId.ToString();
                        ViewState["ItemCodeFromAuction"] = orderProduct.Code;

                        txbProductCode.Text = String.Format("{0} - {1}", orderProduct.Code, orderProduct.CatalogName);
                        hfProductCatalogId.Value = orderProduct.ProductCatalogId.ToString();

                    }

                    DropDownList ddlVAT = e.Row.FindControl("ddlVAT") as DropDownList;
                    if (orderProduct.VAT == 0.22M)
                        ddlVAT.Items[2].Enabled = true;

                    foreach (ListItem item in ddlVAT.Items)
                    {
                        if (orderProduct.VAT == Convert.ToDecimal(item.Value))
                            item.Selected = true;

                    }
                    //  ddlVAT.SelectedIndex = ddlVAT.Items.IndexOf(ddlVAT.Items.FindByValue(orderProduct.VAT.ToString()));


                    if (order.LockOrder.HasValue && order.LockOrder.Value )
                    {
                        TextBox txbQuantity = e.Row.FindControl("txbQuantity") as TextBox;
                        TextBox txbRebate = e.Row.FindControl("txbRebate") as TextBox;
                        //DropDownList ddlVAT = e.Row.FindControl("ddlVAT") as DropDownList;
                        TextBox txbPrice = e.Row.FindControl("txbPrice") as TextBox;

                        txbQuantity.Enabled = txbRebate.Enabled = ddlVAT.Enabled = txbPrice.Enabled = false;
                    }

                    if (orderProduct.ProductCatalogId != null)
                    {
                        ViewState["OrderProuctId"] = orderProduct.OrderProductId;
                        ViewState["ProductCatalogId"] = orderProduct.ProductCatalogId.Value;
                        ViewState["OrderProductQuantity"] = orderProduct.Quantity;
                        //BindSubProducts(e.Row);
                    }
                }

                Dal.OrderProductsView op = e.Row.DataItem as Dal.OrderProductsView;
                Literal litTotal = e.Row.FindControl("litTotal") as Literal;
                decimal tmp = op.Quantity * op.Price * (100.00M - op.Rebate) / 100.00M;
                total += tmp;
                count += op.Quantity;
                litTotal.Text = String.Format("{0:0.00} PLN", tmp);



            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (!AllowEditProducts)
                    gvUserOrders.Columns[0].Visible = false;
                Literal litTotalFooter = e.Row.FindControl("litTotalFooter") as Literal;
                Literal litQuantityFooter = e.Row.FindControl("litQuantityFooter") as Literal;

                litTotalFooter.Text = String.Format("{0:0.00} PLN", total);
                litQuantityFooter.Text = String.Format("{0}", count);

            }
        }

        //private void BindSubProducts(GridViewRow row)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    int orderProuctId = Convert.ToInt32(ViewState["OrderProuctId"]);
        //    int productCatalogId = Convert.ToInt32(ViewState["ProductCatalogId"]);
            
        //    subProducts = pch.GetProductCatalogSubProducts(productCatalogId).Where(x => x.ProductTypeId != (int)Dal.Helper.ProductType.ComboProduct).ToList();
        //    subOrderProducts = oh.GetSubOrderProducts(orderProuctId);

        //    Panel pnSubProducts = row.FindControl("pnSubProducts") as Panel;

        //    if (subProducts.Count > 0)
        //    {
        //        pnSubProducts.Visible = true;



        //        var groups = subProducts.Select(x => new
        //        {
        //            SubProductGroupId = x.SubProductGroupId,
        //            Name = x.GroupName,
        //            IsRequired = x.IsRequired,
        //            OrderProductId = orderProuctId
        //            //ProductCatalogId = x.ProductCatalogRefId
        //        }).Distinct().ToList();

        //        Repeater rpSubProducts = row.FindControl("rpSubProducts") as Repeater;

        //        rpSubProducts.DataSource = groups;
        //        rpSubProducts.DataBind();
        //    }
        //    else

        //        pnSubProducts.Visible = false ;
        //}
        //protected void rpSubProducts_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        Label lblName = e.Item.FindControl("lblName") as Label;
        //        DropDownList ddlSubProduct = e.Item.FindControl("ddlSubProduct") as DropDownList;
        //        RequiredFieldValidator rfvPc = e.Item.FindControl("rfvPc") as RequiredFieldValidator;

        //        TextBox txbSubQuantity = e.Item.FindControl("txbSubQuantity") as TextBox;

        //        lblName.Text = DataBinder.Eval(e.Item.DataItem, "Name").ToString();

        //        int subProductGroupId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "SubProductGroupId"));
        //      //  int productCatalogId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ProductCatalogId"));
        //        int orderProductId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "OrderProductId"));

        //        bool isRequired = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsRequired"));

        //        ddlSubProduct.DataSource = subProducts.Where(x => x.SubProductGroupId == subProductGroupId)
        //            .Select(x => new
        //            {
        //                ProductCatalogId = x.ProductCatalogRefId,
        //                Name = x.Name
        //            })
        //            .ToList();
        //        ddlSubProduct.DataBind();

        //        int [] pIds = subProducts.Where(x=>x.SubProductGroupId == subProductGroupId).Select(x=>x.ProductCatalogRefId).ToArray();


        //        Dal.OrderProduct subOrderProduct = subOrderProducts.Where(x => x.SubOrderProductId == orderProductId
        //            && pIds.Contains(x.ProductCatalogId.Value)).FirstOrDefault();

        //        int idx = -1;
        //        int quantity = 1;
        //        ddlSubProduct.Items.Insert(0, new ListItem("", ""));

        //        if (subOrderProduct != null)
        //        {
        //            idx = ddlSubProduct.Items.IndexOf(ddlSubProduct.Items.FindByValue(subOrderProduct.ProductCatalogId.ToString()));
        //            quantity = subOrderProduct.Quantity;
        //        }

        //        if (idx != -1)
        //        {
        //            ddlSubProduct.SelectedIndex = idx;
        //            if(ViewState["OrderProductQuantity"]!=null && ViewState["OrderProductQuantity"]!=""
        //                && Convert.ToInt32(ViewState["OrderProductQuantity"])!=0)
        //            txbSubQuantity.Text = (quantity / Convert.ToInt32(ViewState["OrderProductQuantity"]) ).ToString();
        //        }


        //        rfvPc.Visible = isRequired;

        //    }
        //}
        //public static IDictionary GetValues(GridViewRow row)
        //{
        //    IOrderedDictionary values = new OrderedDictionary();
        //    foreach (DataControlFieldCell cell in row.Cells)
        //    {
        //        if (cell.Visible)
        //        {
        //            // Extract values from the cell
        //            cell.ContainingField.ExtractValuesFromCell(values, cell, row.RowState, true);
        //        }
        //    }

        //    return values;
        //}

        protected void ddlProductCatalogGrid_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = (sender as DropDownList).Parent.Parent as GridViewRow;


            ViewState["ProductCatalogId"] = Convert.ToInt32((sender as DropDownList).SelectedValue);
            //BindSubProducts(row);
        }
        protected void gvUserOrders_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            order = Dal.DbHelper.Orders.GetOrder(OrderId);

            decimal total = order.AmountToPay;


            GridViewRow row = gvUserOrders.Rows[e.RowIndex];
            TextBox txbQuantity = row.FindControl("txbQuantity") as TextBox;
            Panel pnStatus = row.FindControl("pnStatus") as Panel;
            TextBox txbPrice = row.FindControl("txbPrice") as TextBox;
            TextBox txbComment = row.FindControl("txbComment") as TextBox;
            TextBox txbRebate = row.FindControl("txbRebate") as TextBox;
            // TextBox txbVAT = row.FindControl("txbVAT") as TextBox;
            DropDownList ddlVAT = row.FindControl("ddlVAT") as DropDownList;
            DropDownList ddlOrderProductStatus = row.FindControl("ddlOrderProductStatus") as DropDownList;
            HiddenField hfProductCatalogId = row.FindControl("hfProductCatalogId") as HiddenField;
            TextBox txbProductCode = row.FindControl("txbProductCode") as TextBox;
 

            string changeLog = "";

            int orderProductId = Convert.ToInt32(gvUserOrders.DataKeys[e.RowIndex][0]);


            if (orderProductId != 0)
            {

                Dal.OrderProduct orderProduct = oh.GetOrderProduct(orderProductId);

                if (orderProduct.Quantity != Convert.ToInt32(txbQuantity.Text))
                {
                    changeLog += String.Format("Zmiana Ilość z {0} na {1}\n", orderProduct.Quantity, txbQuantity.Text);
                }
                if (orderProduct.Price != Convert.ToDecimal(txbPrice.Text))
                {
                    changeLog += String.Format("Zmiana Cena z {0} na {1:C}\n", orderProduct.Price, txbPrice.Text);
                }
                if (orderProduct.VAT != Convert.ToDecimal(ddlVAT.SelectedValue))
                {
                    changeLog += String.Format("Zmiana VAT z {0} na {1}\n", orderProduct.VAT, ddlVAT.SelectedValue);
                }
                if (orderProduct.Rebate != Convert.ToDecimal(txbRebate.Text))
                {
                    changeLog += String.Format("Zmiana Rabat z {0} na {1:00}\n", orderProduct.Rebate, txbRebate.Text);
                }

                if (orderProduct.Comment != txbComment.Text)
                {
                    changeLog += String.Format("Zmiana Komentarz z '{0}' na '{1}'\n", orderProduct.Comment, txbComment.Text);
                }
                if (hfProductCatalogId.Value != orderProduct.ProductCatalogId.ToString())
                {
                    changeLog += String.Format("Przypisano produkt z katalogu: {0} - {1}\n", hfProductCatalogId.Value, txbProductCode.Text);

                }
                if (ddlOrderProductStatus.SelectedValue != orderProduct.OrderProductStatusId.ToString())
                {
                    changeLog += String.Format("Zmiana statusu produktu z {0} na {1}\n", ddlOrderProductStatus.Items.FindByValue(orderProduct.OrderProductStatusId.ToString()).Text,
                        ddlOrderProductStatus.SelectedItem.Text);

                }

                #region Subproducts
 


                    if (changeLog != "")
                    {
                        orderProduct.Quantity = Convert.ToInt32(txbQuantity.Text);
                        orderProduct.Price = Convert.ToDecimal(txbPrice.Text);
                        orderProduct.Rebate = Convert.ToDecimal(txbRebate.Text);
                        orderProduct.VAT = Convert.ToDecimal(ddlVAT.SelectedValue);
                        orderProduct.Comment = txbComment.Text;
                        orderProduct.LastUpdateDate = DateTime.Now;
                        orderProduct.LastUpdateReason = "Aktualizacja ręczna na stronie";
                        orderProduct.OrderProductStatusId = Convert.ToInt32(ddlOrderProductStatus.SelectedValue);

                        if (pnStatus.Visible)
                        {
                            orderProduct.ProductCatalogId = Convert.ToInt32(hfProductCatalogId.Value);
                        }

                        oh.UpdateOrderProduct(orderProduct, changeLog, UserName);
                    }
 


                #endregion
            }
            else
            {
                if (order.ShippingCost != Convert.ToDecimal(txbPrice.Text))
                {
                    changeLog += String.Format("Zmiana kosztu wysyłki z {0} na {1:C}\n", order.ShippingCost, txbPrice.Text);
                }
                if (order.ShippingCostVAT != Convert.ToDecimal(ddlVAT.SelectedValue))
                {
                    changeLog += String.Format("Zmiana stawki VAT dla przesyłki z {0} na {1}\n", order.ShippingCostVAT, ddlVAT.SelectedValue);
                }

                order.ShippingCost = Convert.ToDecimal(txbPrice.Text);
                order.ShippingCostVAT = Convert.ToDecimal(ddlVAT.SelectedValue);
                oh.UpdateOrder(order, changeLog, UserName);
            }


            gvUserOrders.EditIndex = -1;
            BindProducts(OrderId, AllowEditProducts, GetSuppliers());

            if (Saved != null)
            {
                 
                order = Dal.DbHelper.Orders.GetOrder(OrderId);

                decimal totalNew = order.AmountToPay;

                order = Dal.DbHelper.Orders.GetOrder(OrderId);
                bool amountChanged = true;// order.OrderShipping.cod && total != totalNew && order.ShipmentTrackingNumber != null;
                Saved(this, amountChanged);
            }
        }
        protected void gvUserOrders_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUserOrders.EditIndex = -1;
            BindProducts(OrderId, AllowEditProducts, GetSuppliers());
        }
        protected void gvUserOrders_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUserOrders.EditIndex= e.NewEditIndex;
            BindProducts(OrderId, AllowEditProducts, GetSuppliers());
             
        }

        public int[] GetSelectedProductIds()
        {
            return WebHelper.GetSelectedIds<int>(gvUserOrders, "chbOrder");
        }

        protected void lbtnQuantity_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.OrderHelper oh = new Dal.OrderHelper();
            order = Dal.DbHelper.Orders.GetOrder(OrderId);
            List<Dal.ProductCatalogWarehouse> warehouses = pch.GetWarehouse();
            string s = "";

            
            int productCatalogId = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            Dal.ProductCatalog pcc = Dal.DbHelper.ProductCatalog.GetProductCatalog(productCatalogId);
       

            string total = String.Format("Aktualny łączny stan: {0}", pcc.TotalQuantity - pcc.SoldQuantity - pcc.QuantityBlocked);
            string totalSupplier = "";
            if (pcc.SupplierQuantity.HasValue)
                totalSupplier = String.Format("U producenta: {0}", pcc.SupplierQuantity);

            foreach (Dal.ProductCatalogWarehouse w in warehouses)
            {

                Dal.ProductCatalogViewFnResult pc = oh.GetProductCatalogFn(productCatalogId, w.WarehouseId);

               
                if (order.OrderShipping.ShippingServiceMode.WarehouseId == w.WarehouseId)
                {
                    List<Dal.OrderProductsView> orderProducts = oh.GetOrderProducts(OrderId);
                    int quantity = orderProducts.Where(x => x.ProductCatalogId == productCatalogId).Sum(x => x.Quantity);
                    s += String.Format("<tr><td>{0}</td><td>{1}szt.</td></tr>", w.Name, pc.LeftQuantity + quantity);

                }
                else
                    s += String.Format("<tr><td>{0}</td><td>{1}szt.</td></tr>", w.Name, pc.LeftQuantity);


            }
            DisplayMessage(String.Format("{1}<br>{2}<br><Br>Stan magazynowy: <br><br><table>{0}</table><br><br>Stany magazynowe bez produktów z tego zamówienia", s, total, totalSupplier));
        }

        protected void ddlStatusName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["OrderProductStatusId"]  = Int32.Parse((sender as DropDownList).SelectedValue);
            

            BindProducts(OrderId, AllowEditProducts, GetSuppliers());
        }
    }
}