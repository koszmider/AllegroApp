using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using System.Collections.Generic;
using System.IO;
using LajtIt.Dal;

namespace LajtIt.Web
{
    [Developer("043d98dd-4bff-4ad1-bfc6-44cdd3b0d816")]
    public partial class OrdersProcessing : LajtitPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //ucInpostExport.GetIds += SendOrderIds;

            if (!Page.IsPostBack)
            {
                txbSentDate.Text = DateTime.Now.Date.ToString("dd.MM.yyyy");
                //BindOrderBatch();
                BindShippingReport();
                BindProducts();
                BindInpostCourier();
            }
            else
                txbSentDate.Text = txbSentDate.Text;


        }
        //public void SendOrderIds()
        //{
        //    ucInpostExport.OrderIds= GetProducts().Where(x => x.ShippingCompanyId.Value == (int)Dal.Helper.ShippingCompany.InPost)
        //            .Select(x => x.OrderId).Distinct().ToArray();
        //}
        protected void tmInterval_Tick(object sender, EventArgs e)
        {
            //BindOrderBatch();
            BindShippingReport();
            BindProducts();
            BindInpostCourier();

        }

        private void BindInpostCourier()
        {
//            Bll.InpostHelper ih = new InpostHelper();
//            try
//            {
//                Bll.InpostHelper.DItem item = ih.DispatchOrdersGetByStatus("new,sent,accepted");

//                if (item != null)
//                {
//                    lblInpostInfo.Text = String.Format(@"Zlecenie Inpost utworzone.<br>
//Status: {0}<br>data utworzenia: {1:MM/dd HH:mm}<br>Liczba paczek: {2}", GetInpostStatus(item.status), item.created_at, item.shipments.Count);
//                    lbtCallCourier.Visible = false;
//                    lbtCancelCourier.Visible = true;
//                }
//                else
//                {
//                    lblInpostInfo.Text = "Kurier Inpost niezamówiony";
//                    lbtCallCourier.Visible = true;
//                    lbtCancelCourier.Visible = false;
//                }
//            }
//            catch (Exception ex)
//            {
//                lblInpostInfo.Text = "Błąd pobierania danych Inpost";
//                lbtCallCourier.Visible = false;
//                lbtCancelCourier.Visible = false;

//            }
        }

        private string GetInpostStatus(string status)
        {
            switch (status)
            {
                case "new": return "nowy";
                case "sent": return "wysłany";
                case "accepted": return "przyjęte";
                default: return "";
            }
        }

        private void BindShippingReport()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<Dal.OrdersProcessingReport> report = oh.GetOrdersProcessingReport();


            litExportedDpd.Text = GetNumber(1, (int)Dal.Helper.OrderStatus.Exported, report).ToString();
            litExportedInpost.Text = GetNumber(4, (int)Dal.Helper.OrderStatus.Exported, report).ToString();
            litReadyDpd.Text = GetNumber(1, (int)Dal.Helper.OrderStatus.ReadyToSend, report).ToString();
            litReadyInpost.Text = GetNumber(4, (int)Dal.Helper.OrderStatus.ReadyToSend, report).ToString();
            //litSentDpd.Text = GetNumber(1, (int)Dal.Helper.OrderStatus.Sent, report).ToString();
            //litSentInpost.Text = GetNumber(4, (int)Dal.Helper.OrderStatus.Sent, report).ToString();
        }

        private int GetNumber(int companyId, int orderStatusId, List<OrdersProcessingReport> report)
        {
            var r = report.Where(x => x.ShippingCompanyId == companyId && x.OrderStatusId == orderStatusId).Select(x => x.OrderCount).FirstOrDefault();

            return r ?? 0;
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            BindProducts();
        }

        private List<Dal.OrdersReadyForSent> GetProducts()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            List<Dal.Helper.OrderStatus> statuses = new List<Dal.Helper.OrderStatus>();

            // if (chbShowSent.Checked)
            //     statuses.Add(Dal.Helper.OrderStatus.Sent);

            if (rbExported.Checked)
                statuses.Add(Dal.Helper.OrderStatus.Exported);
            if (rbReady.Checked)
                statuses.Add(Dal.Helper.OrderStatus.ReadyToSend);
            if (rbSent.Checked)
                statuses.Add(Dal.Helper.OrderStatus.Sent);


            List<int> shippingCompanyId = new List<int>();
            if (chbDpd.Checked)
                shippingCompanyId.Add((int)Dal.Helper.ShippingCompany.Dpd); ;
            if (chbInpost.Checked)
                shippingCompanyId.Add((int)Dal.Helper.ShippingCompany.InPost);
            DateTime? firstSent, lastSent;
            return oh.GetOrderProductsReadyForSend(statuses, shippingCompanyId.ToArray(), DateTime.Now, out firstSent, out lastSent);// Parse(txbSentDate.Text));


        }
        private void BindProducts()
        {
            List<Dal.OrdersReadyForSent> products = GetProducts();

            gvProducts.DataSource = products.Select(x => new
            {
                x.Ean,
                x.GroupName,
                x.SupplierName,
                Name = x.CatalogName,
                x.Code,
                Quantity = x.Quantity,
                Comment = x.Comment,
                OrderId = x.OrderId,  
                OrderPriority = x.OrderPriority,
                x.OrderProductId,
                x.DeliveryDate

            }).OrderByDescending(x=>x.OrderPriority) .ThenBy(x => x.DeliveryDate).ToList();
            gvProducts.DataBind();

            lbtnExport.Visible = products.Count() > 0;

            Dal.OrderHelper oh = new Dal.OrderHelper();
            DateTime? firstSent = null;
            DateTime? lastSent = null;

            var o = oh.GetOrderProductsReadyForSend(new Dal.Helper.OrderStatus[] { Dal.Helper.OrderStatus.Sent }, new int[] { }, DateTime.Now, out firstSent, out lastSent);
          
            if(firstSent!=null)
            {
               double m = lastSent.Value.Subtract(firstSent.Value).TotalMinutes;

                var sent = o.GroupBy(a => a.OrderId).Count();

                lblOrdersSent.Text = sent.ToString();
                if (m != 0 && sent > 0)
                { 
                    lblOrderSentPerMinute.Text = String.Format("Wysyłasz jedno zamówienie co {0} min", (int)( m / sent));
                    lblOrderSentPerMinute.Visible=true;
                }
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

        //private void BindOrderBatch()
        //{
        //    string id = null;
        //    string idSel = null;
        //    if (lsbOrderBatch.Items.Count > 0)
        //    {
        //        idSel = lsbOrderBatch.SelectedValue;
        //        id = lsbOrderBatch.Items[0].Value;
        //    }
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    lsbOrderBatch.DataSource = oh.GetOrderBatches()
        //        .OrderByDescending(x => x.OrderExportBatchId)
        //        .Take(100)
        //        .Select(x => new
        //        {
        //            OrderExportBatchId = x.OrderExportBatchId,
        //            //BatchName = String.Format("{0} - {1:yyyy-MM-dd HH:mm} - {2}", x.OrderExportBatchId, x.InsertDate, x.ShippingCompany.Name)
        //            BatchName = String.Format("{0:MM-dd HH:mm} - {1}", x.InsertDate, x.ShippingCompany.Name)

        //        })
        //        .ToList();
        //    lsbOrderBatch.DataBind();

        //    if (id != lsbOrderBatch.Items[0].Value && Page.IsPostBack)
        //    { 
        //        lsbOrderBatch.SelectedIndex = 0; // lsbOrderBatch.Items.IndexOf(lsbOrderBatch.Items.FindByValue(id));
        //        DisplayMessage("Nowy plik utworzony. Można go pobrać.");
        //    }
        //    else
        //        if(idSel!=null)
        //        lsbOrderBatch.SelectedIndex =   lsbOrderBatch.Items.IndexOf(lsbOrderBatch.Items.FindByValue(idSel));

        //}
        //protected void lbtnExportPath_Click(object sender, EventArgs e)
        //{
        //    int batchId = Convert.ToInt32(lsbOrderBatch.SelectedValue);

        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    Dal.OrderExportBatch batch = oh.GetOrderExportBatch(batchId);

        //    if (batch.FileName == null)
        //    {
        //        DisplayMessage("Plik nie istnieje");
        //        return;
        //    }

        //    string fileName = String.Format(@"{0}\{1}", Server.MapPath("/Files/ExportFiles"), batch.FileName);


        //    string contentType = "Application/pdf";

        //    switch (batch.ShippingCompanyId)
        //    {
        //        case 1: contentType = "Application/xls"; break;
        //        case 2: contentType = "Application/xml"; break;
        //        case 3: contentType = "Application/xml"; break;
        //        case 4: contentType = "Application/xml"; break;
        //        //case 5: contentType = "Application/xls"; break;
        //        case 5: contentType = "Application/csv"; break;
        //    }



        //    Response.ContentType = contentType;
        //    Response.AppendHeader("content-disposition", "attachment; filename=" + batch.FileName);

        //    //Write the file directly to the HTTP content output stream.
        //    Response.WriteFile(fileName);
        //    Response.End();
        //}
        protected void lbtnExport_Click(object sender, EventArgs e)
        {
            SetOrdersAsExported(Dal.Helper.ShippingCompany.Dpd);
        }
        protected void lbtnExportInpost_Click(object sender, EventArgs e)
        {
            SetOrdersAsExported(Dal.Helper.ShippingCompany.InPost);
        }

        private void SetOrdersAsExported(Dal.Helper.ShippingCompany sc)
        { 
            
                Dal.OrderHelper oh = new Dal.OrderHelper();
                int[] orderIds = GetProducts().Where(x => x.ShippingCompanyId  == (int)sc )
                    .Select(x => x.OrderId).Distinct().ToArray();

                foreach (int orderId in orderIds)
                    oh.SetOrdersStatus(sc,
                    orderId,
                    Dal.Helper.OrderStatus.Exported,
                    "Export operatora",
                    UserName);
                rbExported.Checked = true;
                rbReady.Checked = false;
                BindShippingReport();
                //BindOrderBatch();
                BindProducts();
                DisplayMessage(String.Format("Wyeksportowano {0} zamówień", orderIds.Count()));

       
        }

        protected void lbtCallCourier_Click(object sender, EventArgs e)
        {
            //Bll.InpostHelper ih = new InpostHelper();
            //int? id = ih.DispatchCreate2();

            //switch (id)
            //{
            //    case -1: DisplayMessage("Brak aktywnych paczek do utworzenia zlecenia"); break;
            //    case 0:
            //        DisplayMessage("Obecnie istnieje aktywne zlecenia dla kuriera"); break;
            //    default:
            //    DisplayMessage(String.Format("Utworzono zlecenie odbioru dla kuriera")); break;
            //}
            //BindInpostCourier();
        }

        protected void lbtCancelCourier_Click(object sender, EventArgs e)
        {
            //Bll.InpostHelper ih = new InpostHelper();

            //try
            //{
            //bool result = ih.DispatchCancel();

            //    if (result)
            //        DisplayMessage(String.Format("Kurier Inpost został odwołany"));
            //    else
            //        DisplayMessage("Nie udało się odwołać kuriera");
            //}
            //catch (Exception ex)
            //{
            //    DisplayMessage("Nie udało się odwołać kuriera");

            //}
            //BindInpostCourier();
        }

        protected void lbtnPrint_Click(object sender, EventArgs e)
        {

            int[] orderProductId = WebHelper.GetSelectedIds<int>(gvProducts, "chbOrder");

            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.Order> orders = oh.GetOrdersByOrderProductId(orderProductId).Where(x => x.OrderShipping.ShippingCompany != null && x.OrderShipping.ShipmentTrackingNumber != null).ToList();

            string[] files = orders.Select(x => String.Format(@"Shipping\{0}\{1}.pdf", x.OrderShipping.ShippingCompany.Name, x.OrderShipping.ShipmentTrackingNumber)).Distinct().ToArray();


            if (!Bll.OrderHelper.ExportFile(files))
                DisplayMessage("Etykieta nie istnieje");

        }
    }
}
