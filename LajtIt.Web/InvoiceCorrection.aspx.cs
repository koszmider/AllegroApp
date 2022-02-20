using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("da9e0a25-0285-4bc2-a3f4-5457a051cb8a")]
    public partial class InvoiceCorrection : LajtitPage
    {
        private int OrderId { get { return Convert.ToInt32(Request.QueryString["id"].ToString()); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindInvoice();

            hlOrder.NavigateUrl = String.Format("Order.aspx?id={0}", OrderId);
        }
        protected void btnInvoiceCorrection_Click(object sender, EventArgs e)
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            Dal.Invoice invoice = Dal.DbHelper.Orders.GetOrder(OrderId).Invoice;
            string fileName = CreateInvoiceCorrection();
            Bll.InvoiceHelper ih = new Bll.InvoiceHelper();

            ih.SendFileToOutput(fileName);
            BindInvoice();
        }
        protected void btnLock_Click(object sender, EventArgs e)
        {

            Bll.InvoiceHelper ih = new Bll.InvoiceHelper();
            ih.ImagesDirectory = System.Web.HttpContext.Current.Server.MapPath("/Images");
            ih.FilesDirectory = System.Web.HttpContext.Current.Server.MapPath("/Files");
            ih.InvoicesDirectory = System.Web.HttpContext.Current.Server.MapPath("/Files/Invoices");

            ih.LockInvoiceCorrection(OrderId);
            DisplayMessage("Faktura została zablokowana do edycji");
            BindInvoice();
        }
        private string CreateInvoiceCorrection()
        {
            Bll.InvoiceHelper ih = new Bll.InvoiceHelper();

            Dal.OrderHelper oh = new Dal.OrderHelper();


            Dal.Invoice invoice = Dal.DbHelper.Orders.GetOrder(OrderId).Invoice;

            Dal.Invoice invoiceCorrection = oh.GetInvoice(invoice.InvoiceCorrectionId.Value);

            if (invoiceCorrection != null && invoiceCorrection.IsLocked.HasValue && invoiceCorrection.IsLocked.Value)
                return Server.MapPath(String.Format("/Files/Invoices/{0}", invoiceCorrection.InvoiceFileName));


            if (!ih.CreateInvoiceCorrection(OrderId))
            {
                DisplayMessage("Nie można wydrukować faktury. Sprawdź czy wszystkie dane są wypełnione oraz czy produkty z katalogu zostały przypisane.");
                return "";
            }
            ih.ImagesDirectory = Server.MapPath("/Images");
            ih.FilesDirectory = Server.MapPath("/Files");

            string fileName = ih.GetPdfInvoiceCorrection(OrderId);

            return fileName;
            //DisplayMessage("OK");

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<Dal.InvoiceProduct> products = new List<Dal.InvoiceProduct>();
            foreach(GridViewRow row in gvUserOrders.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    int invoiceProductId = Convert.ToInt32(gvUserOrders.DataKeys[row.RowIndex][0]);


                    TextBox txbProductName = row.FindControl("txbProductName") as TextBox;
                    TextBox txbQuantity = row.FindControl("txbQuantity") as TextBox;
                    TextBox txbRebate = row.FindControl("txbRebate") as TextBox;
                    TextBox txbVAT = row.FindControl("txbVAT") as TextBox;
                    TextBox txbPrice = row.FindControl("txbPrice") as TextBox;

                    Dal.InvoiceProduct product = new Dal.InvoiceProduct()
                    {
                        InvoiceProductId = invoiceProductId,
                        Name = txbProductName.Text.Trim(),
                        PriceBrutto = Decimal.Parse(txbPrice.Text.Trim()),
                        Quantity = Int32.Parse(txbQuantity.Text.Trim()),
                        Rebate = Decimal.Parse(txbRebate.Text.Trim()),
                        VatRate = Decimal.Parse(txbVAT.Text.Trim()),
                    };
                    products.Add(product);
                }

            }

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetInvoiceCorrectionProducts(products, txbComment.Text);
            DisplayMessage("Zapisano");
            BindInvoice();
        }
        private void BindInvoice()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

             
            Dal.Invoice invoice = Dal.DbHelper.Orders.GetOrder(OrderId).Invoice;

            Dal.Invoice invoiceCorrection = oh.GetInvoice(invoice.InvoiceCorrectionId.Value);

            if(invoiceCorrection.IsLocked.HasValue && invoiceCorrection.IsLocked.Value)
            {
                btnLock.Enabled = false;
                btnSave.Enabled = false;
                gvUserOrders.Enabled = false;
                btnInvoiceCorrection.Text = String.Format("Korekta {0}", invoiceCorrection.InvoiceNumber);
                    
            }



            gvUserOrders.DataSource = oh.GetInvoiceCorrection(OrderId);
            gvUserOrders.DataBind();
        }
        protected void gvUserOrders_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.OrderInvoiceCorrection c = e.Row.DataItem as Dal.OrderInvoiceCorrection;




            }

        }
    }
}