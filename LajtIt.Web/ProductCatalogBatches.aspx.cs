using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;
namespace LajtIt.Web
{
    [Developer("37a9d570-7eec-4e4e-b1a5-60852850dd02")]
    public partial class ProductCatalogBatches : LajtitPage
    {
        List<LajtIt.Bll.Helper.ProductCategoryMessage> errors;
        protected void Page_Load(object sender, EventArgs e)
        {
            //DateTime n = DateTime.Now;
            //long d = Bll.ProductCatalogForAllegroHelper.DateTimeToUtc(n);
            //Response.Write(String.Format("Now: {0}, Unix: {1}, From Unix: {2}, 2014-10-01 16:10:24.000: {3}",
            //    n,
            //    Bll.ProductCatalogForAllegroHelper.DateTimeToUtc(n),
            //    Bll.ProductCatalogForAllegroHelper.UtcToDateTime(d),
            //    Bll.ProductCatalogForAllegroHelper.UtcToDateTime(1412172624)));

            if (!Page.IsPostBack)
            {
                BindBatchStatuses();
                BindBatches();
                BindPlaning();
            }
        }
        protected void chbPlanning_OnCheckedChanged(object sender, EventArgs e)
        {
            ddlDay.Enabled = ddlHour.Enabled = chbPlanning.Checked;
        }
        private void BindPlaning()
        {
            ddlDay.Items.Clear();

            for (int i = 0; i < 30; i++)
            {
                DateTime day = DateTime.Now.AddDays(i);

                String d = String.Format("{1} {2} - {0}", System.Globalization.DateTimeFormatInfo.CurrentInfo.GetDayName(day.DayOfWeek), 
                    day.Day,
                     System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName( day.Month) );

                ddlDay.Items.Add(new ListItem(d, i.ToString()));

            }

            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            while (now.Day == DateTime.Now.Day)
            {
                ddlHour.Items.Add(new ListItem(String.Format("{0:HH:mm}", now), String.Format("{0:HH:mm}",now)));
                now = now.AddMinutes(15);
            }


            //for (int h = 0; h < 24; h++)
            //{for (int m = 0; m < 24; m++)
            //{
            //    DateTime day = DateTime.Now.AddDays(i);

            //    String d = String.Format("{0} - {1} {2}", System.Globalization.DateTimeFormatInfo.CurrentInfo.GetDayName(day.DayOfWeek),
            //        day.Day,
            //         System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(day.Month));

            //    ddlDay.Items.Add(new ListItem(d, i.ToString()));

            //}
            //}
        }
        protected void ddlBatch_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindBatch(); 
            BindItems();
        }
        protected void btnBatchSearch_Click(object sender, EventArgs e)
        {
                BindBatches();  
        }
        protected void btnBatchDelete_Click(object sender, EventArgs e)
        {
            ChangeBatchStatus("Batch został usunięty", Dal.Helper.ProductAllegroItemBatchStatus.Deleted);
        }

        private void BindBatch()
        {
            litBatchInfo.Text = "";
            imgVerifying.Visible = false;
            btnPreview.Enabled = true;
            btnSave.Enabled = false;
            btnSent.Enabled = false;
            tmTimer.Enabled = false;
            btnBatchPause.Visible = false;
            pnBatch.Visible = false;
            btnBatchDelete.Visible = false;
            pCreateItems.Enabled = true;
            if (ddlBatch.SelectedIndex == -1)
                return;

            pnBatch.Visible = true ;

            Dal.OrderHelper oh = new Dal.OrderHelper();
            int batchId = Convert.ToInt32(ddlBatch.SelectedValue);

            Dal.ProductCatalogAllegroItemBatch batch = oh.GetProductCatalogAllegroItemBatch(batchId);
            pnBatch.GroupingText = batch.Name;



            Dal.Helper.ProductAllegroItemBatchStatus batchStatus = (Dal.Helper.ProductAllegroItemBatchStatus)batch.BatchStatusId;

            tmTimer.Enabled = batchStatus == Dal.Helper.ProductAllegroItemBatchStatus.Creating ||
                batchStatus == Dal.Helper.ProductAllegroItemBatchStatus.Verifying;

            litBatchInfo.Text = String.Format("Bieżący status: {0}", batchStatus.Description());

            BindDisplay(batchStatus);
            if (batchStatus == Helper.ProductAllegroItemBatchStatus.Created)
            {
                bool hasItemsToReCreate = oh.GetProductCatalogAllegroItemsToReCreate(batchId);

                btnSent.Enabled = hasItemsToReCreate;
            }

             
            if (batch.AllegroUserAccountId.HasValue)
                ddlAllegroUser.SelectedIndex = ddlAllegroUser.Items.IndexOf(ddlAllegroUser.Items.FindByValue(batch.AllegroUserAccountId.ToString()));

            if (batch.IsAuction.HasValue)
                rbtnAuctionNo.Checked = !batch.IsAuction.Value;
            rbtnAuctionYes.Checked = !rbtnAuctionNo.Checked;

            switch (batch.EnablePromotions)
            {
                case true:
                    ddlPromotion.SelectedValue = "true"; break;
                default:
                    ddlPromotion.SelectedValue = "false"; break;
            }

            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(batch.ProductStatus.ToString()));


            //List<Dal.ProductCatalogAllegroItemBatchFieldView> fields = oh.GetProductCatalogAllegroItemBatchFields(batchId);

            //Dal.ProductCatalogAllegroItemBatchFieldView fieldDays = fields.Where(x => x.FieldId.Value == 4).FirstOrDefault();
            //if (fieldDays != null)
            //    ddlDays.SelectedValue = LajtIt.Bll.ProductCatalogForAllegroHelper.GetAllegroDaysFromNumberOfDaysRevers(fieldDays.IntValue.Value).ToString(); // ddlDays.Items.IndexOf(ddlDays.Items.FindByValue(fieldDays.IntValue.ToString()));
 


        }

        private void BindDisplay(Dal.Helper.ProductAllegroItemBatchStatus batchStatus)
        {
            switch (batchStatus)
            {
                case Dal.Helper.ProductAllegroItemBatchStatus.Deleted:
                    btnPreview.Enabled = false;
                    pCreateItems.Enabled = false;
                    break;
                case Dal.Helper.ProductAllegroItemBatchStatus.New:
                    btnPreview.Enabled = true;
                    btnBatchDelete.Visible = true;
                    btnSave.Enabled = true;
                    break;
                case Dal.Helper.ProductAllegroItemBatchStatus.CreatedWithErrors:
                case Dal.Helper.ProductAllegroItemBatchStatus.Created:
                    imgVerifying.Visible = false;
                    btnPreview.Enabled = false;
                    btnSent.Enabled = false;
                    tmTimer.Enabled = false;
                    btnBatchPause.Visible = false;
                    pCreateItems.Enabled = false;
                    btnBatchDelete.Visible = true;
                    break;
                case Dal.Helper.ProductAllegroItemBatchStatus.Creating:
                    imgVerifying.Visible = true;
                    btnBatchPause.Visible = true;
                    tmTimer.Enabled = true;
                    btnPreview.Enabled = false;
                    break;
                case Dal.Helper.ProductAllegroItemBatchStatus.Verified:
                    imgVerifying.Visible = false;
                    tmTimer.Enabled = false;
                    btnBatchDelete.Visible = true;
                    btnSent.Enabled = true;
                    btnSave.Enabled = true;
                    break;
                case Dal.Helper.ProductAllegroItemBatchStatus.Verifying:
                case Dal.Helper.ProductAllegroItemBatchStatus.VerifyingAndCreating:
                    imgVerifying.Visible = true;
                    btnBatchPause.Visible = true;
                    tmTimer.Enabled = true;
                    btnPreview.Enabled = false;
                    btnBatchDelete.Visible = true;
                    break;

            }
        }
        protected void btnBatchPause_Click(object sender, EventArgs e)
        {
            ChangeBatchStatus("Zatrzymano przetwarzanie batcha", Dal.Helper.ProductAllegroItemBatchStatus.New);
            BindBatch();
            BindItems();
        }
        private void BindBatchStatuses()
        {

            lbxBatchStatus.Items.AddRange(Enum.GetValues(typeof(Dal.Helper.ProductAllegroItemBatchStatus)).OfType<Dal.Helper.ProductAllegroItemBatchStatus>()
                .Select(x => new ListItem()
                {
                    Selected = (x != Helper.ProductAllegroItemBatchStatus.Deleted) && (x != Helper.ProductAllegroItemBatchStatus.Created),
                    Text = x.Description(),
                    Value = ((int)x).ToString()
                }).ToArray());        
        
        }
        private void BindBatches()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            int[] batchStatusIds = WebHelper.GetIDs(lbxBatchStatus).Select(x=>(int)x).ToArray();
            ddlBatch.DataSource = oh.GetProductCatalogBatches(batchStatusIds);//chbIsSent.Checked);
            ddlBatch.DataBind();
            BindBatch();
            BindItems();
        }

        private void BindItems()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            if (ddlBatch.SelectedIndex != -1)
            {
                List<Dal.ProductCatalogAlegroItemsView> items = oh.GetProductCatalogAllegroItems(Convert.ToInt32(ddlBatch.SelectedValue));

             
                ProductCatalogAlegroItemsView firstItem = items.FirstOrDefault();

                if (firstItem != null)
                {
                    string summary = String.Format("Produktów: {0}<br>Zweryfikowanych poprawnie: {1}<br>Zweryfikowanych błędnie: {2}<br>Wystawionych: {3}<br>Koszt: {4:C}",
                        items.Count(),
                        items.Where(x => x.AllegroItemStatusId == (int)Dal.Helper.ProductAllegroItemStatus.VerifiedOK).Count(),
                        items.Where(x => x.AllegroItemStatusId == (int)Dal.Helper.ProductAllegroItemStatus.VerifiedError).Count(),
                        items.Where(x => x.AllegroItemStatusId == (int)Dal.Helper.ProductAllegroItemStatus.Created).Count(),
                        items.Sum(x => x.ItemCost ?? 0));
                     
                    litSummary.Text = summary;

                    gvItems.DataSource = items;
                    gvItems.DataBind();
                }
            }
            else
            {
                gvItems.DataSource = null;
                gvItems.DataBind();
            }
        }
        protected void tmTimer_OnTick(object sender, EventArgs e)
        {
            BindBatch();
            BindItems();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveBatch();
            BindBatch();
            BindItems();
            DisplayMessage("Dane zostały zapisane");
        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {

            Dal.OrderHelper oh = new Dal.OrderHelper();
            int batchId = Convert.ToInt32(ddlBatch.SelectedValue);
            SaveBatch();
            oh.SetProductCatalogAllegroItemsResetStatus(batchId, Dal.Helper.ProductAllegroItemStatus.New);
            ChangeBatchStatus("Batch jest w trakcie weryfikacji", Dal.Helper.ProductAllegroItemBatchStatus.Verifying);
          
        }

        private void SaveBatch()
        {
            //Dal.OrderHelper oh = new Dal.OrderHelper();
            //int batchId = Convert.ToInt32(ddlBatch.SelectedValue);

            ////List<Dal.ProductCatalogAllegroItemBatchField> fields = new List<ProductCatalogAllegroItemBatchField>();


            //ProductCatalogAllegroItemBatchField sellFormField = Bll.ProductCatalogForAllegroHelper.GetAllegroSellFormBaseOnNumberOfDays(Convert.ToInt32(ddlDays.SelectedValue));
            //sellFormField.BatchId = batchId;
            //fields.Add(sellFormField);
            //ProductCatalogAllegroItemBatchField daysFormField = 
            //    Bll.ProductCatalogForAllegroHelper.GetAllegroSellDays(Convert.ToInt32(ddlDays.SelectedValue));
            //daysFormField.BatchId = batchId;
            //fields.Add(daysFormField);

            //if (chbPlanning.Checked)
            //{
            //    DateTime date = DateTime.Parse(String.Format("{0} {1}", DateTime.Now.ToShortDateString(), ddlHour.SelectedValue));
            //    date = date.AddDays(Convert.ToInt32(ddlDay.SelectedValue));

            //    ProductCatalogAllegroItemBatchField startFormField =
            //        Bll.ProductCatalogForAllegroHelper.GetAllegroStartDate(date);

            //    startFormField.BatchId = batchId;
            //    fields.Add(startFormField);

            //} 
            //oh.SetProductCatalogAllegroItemsBatchFields(batchId, fields);
            SetItemsConfiguration();

        }
        private void ChangeBatchStatus(string msg, Dal.Helper.ProductAllegroItemBatchStatus batchStatus)
        {

            Dal.OrderHelper oh = new Dal.OrderHelper();
            int batchId = Convert.ToInt32(ddlBatch.SelectedValue);
            oh.SetProductCatalogAllegroItemBatchStatus(batchId, batchStatus);

          //  if(batchStatus == Helper.ProductAllegroItemBatchStatus.Creating)
          //      oh.SetProductCatalogAllegroItemsResetStatus(batchId, Dal.Helper.ProductAllegroItemStatus.Creating);

            DisplayMessage(msg);

            BindBatch();
            BindItems();
        }
        protected void gvItems_OnDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogAlegroItemsView item = e.Row.DataItem as Dal.ProductCatalogAlegroItemsView;
                //Bll.ProductCatalogCalculator calc = new Bll.ProductCatalogCalculator(item.PurchasePrice, item.AllegroPrice.Value, item.Rebate.Value, item.Margin.Value);

                Literal litMsg = e.Row.FindControl("litMsg") as Literal;
                HyperLink hlName = e.Row.FindControl("hlName") as HyperLink;
                Literal litSupplier = e.Row.FindControl("litSupplier") as Literal;
                Literal litQuantity = e.Row.FindControl("litQuantity") as Literal;
                Literal litPrice = e.Row.FindControl("litPrice") as Literal;
                Literal litInsertDate = e.Row.FindControl("litInsertDate") as Literal;
                Literal litCategory = e.Row.FindControl("litCategory") as Literal;
                Literal litAllegroUserName = e.Row.FindControl("litAllegroUserName") as Literal;
                Image imgImage = e.Row.FindControl("imgImage") as Image;
                HyperLink hlPreview = e.Row.FindControl("hlPreview") as HyperLink;
                HyperLink hlAllegroItemId = e.Row.FindControl("hlAllegroItemId") as HyperLink;
                LinkButton lbtnDeleteItemFromQueue = e.Row.FindControl("lbtnDeleteItemFromQueue") as LinkButton;
                Label lblAllegroCreateDate = e.Row.FindControl("lblAllegroCreateDate") as Label;
                Panel pnlCreated = e.Row.FindControl("pnlCreated") as Panel;
                Panel pnlCreate = e.Row.FindControl("pnlCreate") as Panel;
                  
                Label lblFinalPrice = e.Row.FindControl("lblFinalPrice") as Label;
                Label lblAllegroName = e.Row.FindControl("lblAllegroName") as Label;
                Label lblQuantity = e.Row.FindControl("lblQuantity") as Label;
                Label lblAllegroCreatePrice = e.Row.FindControl("lblAllegroCreatePrice") as Label;
                Label lblAllegroUserCreated = e.Row.FindControl("lblAllegroUserCreated") as Label;




                litAllegroUserName.Text = Dal.Helper.GetUserName(item.AllegroUserIdAccount.Value);
                // litCategory.Text = item.AllegroCategoryFullName;


                hlPreview.NavigateUrl = String.Format("ProductCatalogPreview.aspx?id={0}&preview=0&catalog=0&idSupplier={1}", item.Id, item.SupplierId);
                hlName.Text = item.AllegroName;
                hlName.NavigateUrl = String.Format("ProductCatalog.Allegro.aspx?id={0}", item.ProductCatalogId);
                 
                litSupplier.Text = item.SupplierName;
                litQuantity.Text = item.LeftQuantity>0  ? item.LeftQuantity.ToString() : "Ilość nielimitowana";

       
                litPrice.Text = String.Format("{0:C}", item.PriceBruttoAllegro);
                litInsertDate.Text = String.Format("{0:yyyy/MM/dd HH:mm}", item.InsertDate);
                //imgImage.ImageUrl = String.Format("/images/ProductCatalog/{0}", item.ImageFullName);

                if (errors != null && errors.Count > 0)
                {
                    string error = String.Join("<br>", errors.Where(x => x.Id == item.Id)
                        .Select(x => x.ErrorMessage)
                        .ToArray());
                    litMsg.Text = error;
                    if (errors.Where(x => x.Id == item.Id && x.IsError).Count() > 0)
                    {
                        e.Row.BackColor = System.Drawing.Color.Red;
                    }
                }

                  Dal.Helper.ProductAllegroItemStatus status = (Dal.Helper.ProductAllegroItemStatus)item.AllegroItemStatusId;

                lbtnDeleteItemFromQueue.Visible =
                    ( status == Helper.ProductAllegroItemStatus.New
                    || status == Helper.ProductAllegroItemStatus.VerifiedError
                    || status == Helper.ProductAllegroItemStatus.VerifiedOK
                    );

                Image imgError = e.Row.FindControl("imgError") as Image;
                pnlCreate.Visible = true;
                pnlCreated.Visible = false;
                switch (status)
                {
                    case Dal.Helper.ProductAllegroItemStatus.VerifiedOK:
                        Image imgOK = e.Row.FindControl("imgOK") as Image;
                        imgOK.Visible = true;
                        litMsg.Text = item.Comment;
                        break;

                    case Dal.Helper.ProductAllegroItemStatus.VerifiedError:
                        imgError.Visible = true;
                        litMsg.Text = item.Comment; 
                        break;

                    case Dal.Helper.ProductAllegroItemStatus.Verifying:
                    case Dal.Helper.ProductAllegroItemStatus.Creating:
                        Image imgVerifying = e.Row.FindControl("imgVerifying") as Image;
                        if(imgVerifying !=null)
                        imgVerifying.Visible = true;
                        break;
                    case Dal.Helper.ProductAllegroItemStatus.Created:
                        litMsg.Text = item.Comment; 
                        lblAllegroCreateDate.Text = String.Format("{0:yyyy/MM/dd HH:mm:ss}", item.AllegroItemCreateDate);
          
                        hlAllegroItemId.NavigateUrl = String.Format("http://allegro.pl/show_item.php?item={0}", item.ItemId);
                        hlAllegroItemId.Text = item.ItemId.ToString();
                        lblAllegroName.Text = item.AllegroName;
                        lblQuantity.Text = String.Format("{0}", item.StartingQuantity);
                        lblAllegroCreatePrice.Text = String.Format("{0:C}", item.BuyNowPrice);
                        lblAllegroUserCreated.Text = Dal.Helper.GetUserName(item.AllegroUserIdAccount.Value); 
                        pnlCreated.Visible = true;
                        pnlCreate.Visible = false;
                        break;
                    case Dal.Helper.ProductAllegroItemStatus.CreatingError:
                        imgError.Visible = true;
                        litMsg.Text = item.Comment;
                        break;

                }


            }
        }
        protected void lbtnDeleteItemFromQueue_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((sender as LinkButton).CommandArgument));
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetProductCatalogAllegroItemDelete(id);
            BindItems();
        }
        protected void chbIsSent_Click(object sender, EventArgs e)
        {
            BindBatches();
        }
        protected void btnSent_Click(object sender, EventArgs e)
        {
            SetItemsConfiguration();
            ChangeBatchStatus("Batch został zakolejkowany do wystawienia", Helper.ProductAllegroItemBatchStatus.Creating);

            BindItems();
        }

        private void SetItemsConfiguration()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            int batchId = Convert.ToInt32(ddlBatch.SelectedValue);
            DateTime? date = null;

            if (chbPlanning.Checked)
            {
                date = DateTime.Parse(String.Format("{0} {1}", DateTime.Now.ToShortDateString(), ddlHour.SelectedValue));
                date = date.Value.AddDays(Convert.ToInt32(ddlDay.SelectedValue));
                long d = Bll.ProductCatalogForAllegroHelper.DateTimeToUtc(date.Value);
            }



            oh.SetProductCatalogAllegroItemBatchConfiguration(
                batchId,
                GetBooleanNull(ddlPromotion), 
                GetInt64Null(ddlAllegroUser), 
                rbtnAuctionYes.Checked,
                Convert.ToInt32(ddlDays.SelectedValue),
                date,
                Int32.Parse(ddlStatus.SelectedValue)
                );

            //List<Dal.ProductCatalogAllegroItem> items = new List<ProductCatalogAllegroItem>();

            //for (int i = 0; i < gvItems.Rows.Count; i++)
            //{
            //    Dal.ProductCatalogAllegroItem item = new ProductCatalogAllegroItem();
            //    item.Id = Convert.ToInt32(gvItems.DataKeys[i][0]);
            //    item.AllegroName = null;
            //    if (((TextBox)gvItems.Rows[i].FindControl("txbAllegroName")).Text.Trim() != "")
            //        item.AllegroName = ((TextBox)gvItems.Rows[i].FindControl("txbAllegroName")).Text.Trim();
            //    item.EnablePromotions = null;
            //    if (((DropDownList)gvItems.Rows[i].FindControl("ddlPromotion")).SelectedIndex != 0)
            //        item.EnablePromotions = Convert.ToBoolean(((DropDownList)gvItems.Rows[i].FindControl("ddlPromotion")).SelectedValue);
            //    item.AllegroUserAccountId = null;
            //    if (((DropDownList)gvItems.Rows[i].FindControl("ddlUserId")).SelectedIndex != 0)
            //        item.AllegroUserAccountId = Convert.ToInt64(((DropDownList)gvItems.Rows[i].FindControl("ddlUserId")).SelectedValue);
            //    item.PriceAddType = null;
            //    if (((DropDownList)gvItems.Rows[i].FindControl("ddlPriceAddType")).SelectedIndex != 0)
            //        item.PriceAddType = Convert.ToInt32(((DropDownList)gvItems.Rows[i].FindControl("ddlPriceAddType")).SelectedValue);
            //    item.PriceAddValueType = null;
            //    if (((DropDownList)gvItems.Rows[i].FindControl("ddlPriceAddValueType")).SelectedIndex != 0)
            //        item.PriceAddValueType = Convert.ToInt32(((DropDownList)gvItems.Rows[i].FindControl("ddlPriceAddValueType")).SelectedValue);
            //    item.PriceValue = null;
            //    if (((TextBox)gvItems.Rows[i].FindControl("txbPriceValue")).Text.Trim() != "")
            //        item.PriceValue = Convert.ToDecimal(((TextBox)gvItems.Rows[i].FindControl("txbPriceValue")).Text.Trim());

            //    items.Add(item);
            //}

            //oh.SetProductCatalogAllegroItemsConfiguration(batchId,
            //   items);


        }

        private decimal? GetDecimalNull(TextBox txb)
        {
            if (txb.Text.Trim() == "")
                return null;
            else
                return Convert.ToDecimal(txb.Text.Trim());

        }
        private int? GetInt32Null(DropDownList ddl)
        {
            if (ddl.SelectedValue == "0")
                return null;
            else
                return Convert.ToInt32(ddl.SelectedValue);

        }
        private long? GetInt64Null(DropDownList ddl)
        {
            if (ddl.SelectedValue == "0")
                return null;
            else
                return Convert.ToInt64(ddl.SelectedValue);

        }
        private bool? GetBooleanNull(DropDownList ddl)
        {
            if (ddl.SelectedValue == "0")
                return null;
            else
                return Convert.ToBoolean(ddl.SelectedValue);

        }
    }
}