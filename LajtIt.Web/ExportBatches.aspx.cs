using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using System.Collections.Generic;
using System.IO;

namespace LajtIt.Web
{
    [Developer("3e9bcb96-4b06-4491-905f-0b1b072c0522")]
    public partial class ExportBatches : LajtitPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { 
                BindShippingCompanies(); 
                BindResults();
                BindOrderBatch();
            }
        }
        protected void btnShowOrderDetails_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
             

            List<Dal.OrderExportView> orders = oh.GetOrderExportView(GetSelectedIds());

            gvOrderExportView.DataSource = orders.OrderBy(x=>x.ShippingType);
            gvOrderExportView.DataBind();
        }
        protected void ddlSupplier_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindProducts();
        }
        private int[] GetSelectedIds()
        {
            List<int> list = new List<int>();
            foreach (GridViewRow row in gvBatches.Rows)
            {
                CheckBox chbOrder = row.FindControl("chbOrder") as CheckBox;
                if (chbOrder.Checked)
                {
                    int orderId = Convert.ToInt32(gvBatches.DataKeys[row.RowIndex][0]);
                    list.Add(orderId);
                }
            }
            return list.ToArray();
        }
        protected void btnShowProducts_Click(object sender, EventArgs e)
        {
            BindProducts();
        }

        private void BindProducts()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<Dal.Helper.OrderStatus> statuses = new List<Dal.Helper.OrderStatus>();
            statuses.Add(Dal.Helper.OrderStatus.ReadyToSend);
            statuses.Add(Dal.Helper.OrderStatus.Exported);

            if (chbShowSent.Checked)
                statuses.Add(Dal.Helper.OrderStatus.Sent);

            List<Dal.OrderProduct> products = oh.GetOrderProductsReadyForSend(GetSelectedIds(), statuses);

            if(ddlSupplier.SelectedIndex!=0)
                products = products.Where(x=>x.ProductCatalog.SupplierId == Convert.ToInt32(ddlSupplier.SelectedValue)).ToList();

            gvProducts.DataSource = products.Select(x => new
            {
                Name = x.ProductCatalog.Name,
                Quantity = x.Quantity,
                Comment = x.Comment,
                OrderId = x.OrderId, 
                CompanyName = x.Order.Invoice == null ? "" : x.Order.Invoice.Company.Name,
                UserName = String.Format("{0}<br>{1} {2}<br>{3}",
                x.Order.Email, x.Order.ShipmentFirstName,x.Order.ShipmentLastName, x.Order.ShippingData),
                OrderPriority = x.Order.OrderPriority

            }).OrderBy(x => x.Name).ToList();
            gvProducts.DataBind();

            pProducts.Visible = products.Count > 0;
        }

        protected void btnShowInvoices_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            gvInvoices.DataSource = oh.GetInvoices(GetSelectedIds())
                .Select(x => new
                {
                    OrderId = x.OrderId,
                    
                    Name = String.Format("{0} {1}", x.ShipmentFirstName , x.ShipmentLastName),
                    CompanyName = x.Invoice.Company.Name

                }).OrderBy(x => x.CompanyName). ToList();
            gvInvoices.DataBind();
        }


        protected void btnPrintOrders_Click(object sender, EventArgs e)
        {

            Bll.OrderHelper oh = new OrderHelper();

            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));
            string fileName = pdf.CreateOrders(GetSelectedIds());


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }

        protected void btnPrintParagons_Click(object sender, EventArgs e)
        {

            Bll.OrderHelper oh = new OrderHelper();
            oh.CreateParagons(GetSelectedIds());

            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));
            string fileName = pdf.CreateParagons(GetSelectedIds());


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }
        protected void btnPrintInvoices_Click(object sender, EventArgs e)
        {
          //  DisplayMessage("Do zrobienia");

            InvoiceHelper ih = new InvoiceHelper();
            string[] files = ih.GetInvoiceFiles(GetSelectedIds());

            //Bll.OrderHelper oh = new OrderHelper();
            //oh.CreateInvoices(GetSelectedIds());

            PDF pdf = new PDF(Server.MapPath("/Images"), Server.MapPath("/Files"));
            string fileName = pdf.GetInvoices(files,  Server.MapPath("/Files/Invoices"));


            string contentType = contentType = "Application/pdf";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            //Write the file directly to the HTTP content output stream.
            Response.WriteFile(fileName);
            Response.End();
        }
        private void BindOrderBatch()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            lsbOrderBatch.DataSource = oh.GetOrderBatches()
                .Select(x=> new 
                {
                    OrderExportBatchId = x.OrderExportBatchId,
                    BatchName = String.Format("{0} - {1:yyyy-MM-dd HH:mm}",
                    x.OrderExportBatchId, x.InsertDate)

                })
                .OrderByDescending(x=>x.OrderExportBatchId)
                .ToList() ;
            lsbOrderBatch.DataBind();
        }

        private void BindShippingCompanies()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            chblShippingCompany.DataSource = oh.GetShipppingCompanies().Where(x => x.IsActive).ToList() ;
            chblShippingCompany.DataBind();
            foreach (ListItem item in chblShippingCompany.Items.Cast<ListItem>().ToList())
                item.Selected = true;
        }
         
        protected void btnShow_Click(object sender, EventArgs e)
        {

        }
        protected void gvOrders_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBatches.PageIndex = e.NewPageIndex;
            BindResults();
        }
        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            BindResults();
        }

        private void BindResults()
        {
           
            int[] orderBatchIds = lsbOrderBatch.Items.Cast<ListItem>()
                .Where(x => x.Selected == true)
                .Select(x => Convert.ToInt32(x.Value))
                .ToArray();

            int[] shippingCompanyIds = chblShippingCompany.Items.Cast<ListItem>()
                .Where(x => x.Selected == true)
                .Select(x => Convert.ToInt32(x.Value))
                .ToArray();

            Dal.OrderHelper oh = new Dal.OrderHelper();


            gvBatches.DataSource = oh.GetOrderBatches(
                orderBatchIds,
                shippingCompanyIds)
                .OrderByDescending(x => x.InsertDate)
                .ToList();
            gvBatches.DataBind();
            pActions.Visible = gvBatches.Rows.Count > 0;

        } 
        protected void gvOrders_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {  
                Literal LitId = e.Row.FindControl("LitId") as Literal;
                LitId.Text = (e.Row.RowIndex + 1 + gvBatches.PageSize * gvBatches.PageIndex).ToString();
                 



            }
        }

        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var test = DataBinder.Eval(e.Row.DataItem, "OrderPriority");

                if (test != null && Convert.ToBoolean(test))
                {
                    e.Row.BackColor = System.Drawing.Color.Red;
                    e.Row.ForeColor = System.Drawing.Color.White;
                }
            }

        }
    }
}