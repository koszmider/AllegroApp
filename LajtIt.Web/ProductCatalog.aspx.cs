using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using LajtIt.Web.Controls;
using System.IO;

namespace LajtIt.Web
{
    [Developer("52a8955b-ca2a-45d1-8c06-2147ee115a0e")]
    public partial class ProductCatalog : LajtitPage
    { 


        private int total = 0;
        private int totalQuantity = 0;
        private bool canChangeImages = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            ucUploadImage.ImageIds += ImageIds;
            canChangeImages = HasActionAccess(Guid.Parse("42940950-da83-4eed-841e-a7f817a37ef9"));

            if (!Page.IsPostBack)
            {
                //ddlIsReady.SelectedIndex = 1;
                BindSuppliers();
                BindWarehouse();
                //BindAttributeGroups();
                BindCatalog();
                BindActions();

               // ucProductAttributes.IsRequired = false;
            }
        }
        protected int[] ImageIds(object sender, EventArgs e)
        {
            int[] productIds = WebHelper.GetSelectedIds<int>(gvProductCatalog, "chbOrder");

            if (rbtnAddToBatchAll.Checked)
                productIds = GetProducts().Select(x => x.ProductCatalogId).ToArray();


            return productIds;

        }

        private void BindWarehouse()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            chblWarehouse.DataSource = pch.GetWarehouse().Where(x => x.CanAssignDelivery).Where(x=>x.WarehouseId!=4).ToList();
            chblWarehouse.DataBind();
        }

        private void BindActions()
        {
            //if (UserName == "ania")
            //{
            //    for (int i = 1; i < ddlAction.Items.Count; i++)
            //        ddlAction.Items[i].Enabled = false;

            //    int[] availableActions = new int[] { 12, 14, 15, 1 };
            //    foreach (int i in availableActions)
            //        ddlAction.Items[ddlAction.Items.IndexOf(ddlAction.Items.FindByValue(i.ToString()))].Enabled = true;
            //}
        }

        //private void BindAttributeGroups()
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    ddlAttributes.DataSource = pch.GetProductCatalogAttributeGroups();
        //    ddlAttributes.DataBind();

        //    BinAttributes();
        //}

        private void BindSuppliers()
        { 

            lbxSearchSupplier.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderBy(x=>x.Name);
            lbxSearchSupplier.DataBind();

            //lbxSearchSupplier.SelectedValue = "3";
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindCatalog();
        }
        private void SetBatches(int[] productIds)
        {
            throw new NotImplementedException();

            //int[] supplierIds = lbxSearchSupplier.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();
            //if (supplierIds.Length != 1)
            //{
            //    DisplayMessage("Wybierz wpierw dostawcę, następnie wyszukaj prdukty by móc je dodać do batcha");
            //    return;
            //}
            //if (rbBatchNew.Checked && txbBatchName.Text.Trim() == "")
            //{
            //    DisplayMessage("Podaj nazwę batcha");
            //    return;
            //}

            //if (!rbBatchNew.Checked && ddlBatch.SelectedIndex == -1)
            //{
            //    DisplayMessage("Brak aktywnego batcha do którego możnaby dodać zaznaczone produkty");
            //    return;
            //}

            //Dal.ProductCatalogAllegroItemBatch batch = new Dal.ProductCatalogAllegroItemBatch()
            //{
            //    InsertDate = DateTime.Now,
            //    BatchStatusId = (int)Dal.Helper.ProductAllegroItemBatchStatus.New,
            //    Name = txbBatchName.Text.Trim(),
            //    SupplierId = supplierIds[0]
            //};

            //List<Dal.ProductCatalogAllegroItem> items = new List<Dal.ProductCatalogAllegroItem>();
            //foreach (int productId in productIds)
            //{
            //    Dal.ProductCatalogAllegroItem item = new Dal.ProductCatalogAllegroItem()
            //    {
            //        AllegroItemGuid = Guid.NewGuid(),
            //        InsertDate = DateTime.Now,
            //        AllegroItemStatusId = (int)Dal.Helper.ProductAllegroItemStatus.New,
            //        ProductCatalogId = productId

            //    };
            //    if (rbBatchNew.Checked)
            //        item.ProductCatalogAllegroItemBatch = batch;
            //    else
            //        item.BatchId = Convert.ToInt32(ddlBatch.SelectedValue);

            //    items.Add(item);
            //}


            //Dal.OrderHelper oh = new Dal.OrderHelper();
            //oh.SetProductCatalogAllegroItem(items);

            //if (rbBatchNew.Checked)
            //    DisplayMessage(String.Format("Dodano {0} produktów do kolejki: {1}. Przejdź do strony <a href='/ProductCatalogBatches.aspx'>wystawiania aukcji</a>", productIds.Count(), txbBatchName.Text));
            //else
            //    DisplayMessage(String.Format("Dodano {0} produktów do kolejki: {1}. Przejdź do strony <a href='/ProductCatalogBatches.aspx'>wystawiania aukcji</a>", productIds.Count(), ddlBatch.SelectedItem.Text));

            //BindCatalog();
        }

        protected void gvProductCatalog_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gvProductCatalog.PageIndex = e.NewPageIndex;
            BindCatalog();
        }
        protected void gvProductCatalog_DataBound(object sender, EventArgs e)
        {

            //int c = gvProductCatalog.FooterRow.Cells.Count - 1;
            //for (int i = c - 1; i >= 1; i += -1)
            //{
            //    gvProductCatalog.FooterRow.Cells.RemoveAt(i);
            //}
            //gvProductCatalog.FooterRow.Cells[0].ColumnSpan = c;
        }
        protected void gvProductCatalog_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            //int c = gvProductCatalog.FooterRow.Cells.Count - 1;
            //for (int i = c - 1; i >= 1; i += -1)
            //{
            //    gvProductCatalog.FooterRow.Cells.RemoveAt(i);
            //}
            //gvProductCatalog.FooterRow.Cells[0].ColumnSpan = c;
        }
        protected void gvProductCatalog_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //int c = e.Row.Cells.Count-1;
                //for (int i = c - 1; i >= 1; i += -1)
                //{
                //    e.Row.Cells.RemoveAt(i);
                //}

                //e.Row.Cells[0].ColumnSpan = c;
                Literal litTotal = e.Row.FindControl("litTotal") as Literal;
                litTotal.Text = String.Format("Produktów: {0}, przedmiotów: {1}", total, totalQuantity);
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogViewFnResult pc = e.Row.DataItem as Dal.ProductCatalogViewFnResult;

                //HyperLink hlCalcuator = e.Row.FindControl("hlCalcuator") as HyperLink;
                // hlCalcuator.NavigateUrl = String.Format("/ProductCalculator.aspx?id={0}", pc.ProductCatalogId);
                Image imgImage = e.Row.FindControl("imgImage") as Image; 
                Literal liId = e.Row.FindControl("liId") as Literal;
                HyperLink hlProduct = e.Row.FindControl("hlProduct") as HyperLink;
               // HyperLink hlProductAllegro = e.Row.FindControl("hlProductAllegro") as HyperLink;
                HyperLink hlPreview = e.Row.FindControl("hlPreview") as HyperLink;
                 
                Label lblSellPrice = e.Row.FindControl("lblSellPrice") as Label; 
                Label lblQuantity = e.Row.FindControl("lblQuantity") as Label;
                Label lblSupplierQuantity = e.Row.FindControl("lblSupplierQuantity") as Label;
                Label lblCode = e.Row.FindControl("lblCode") as Label;
                Label lblCode2 = e.Row.FindControl("lblCode2") as Label;
                Label lblCodeSupplier = e.Row.FindControl("lblCodeSupplier") as Label;
                Label lblExternalId = e.Row.FindControl("lblExternalId") as Label; 

                ImageButton imgCat = e.Row.FindControl("imgCat") as ImageButton;

                imgCat.CommandArgument =  pc.ProductCatalogId.ToString();


                //lblNewName.Text = Mixer.GetProductName(4, pc.ProductCatalogId);

                hlPreview.NavigateUrl = String.Format(hlPreview.NavigateUrl, pc.ProductCatalogId, pc.SupplierId);
                hlProduct.NavigateUrl = String.Format(hlProduct.NavigateUrl, pc.ProductCatalogId);
               // hlProductAllegro.NavigateUrl = String.Format(hlProductAllegro.NavigateUrl, pc.ProductCatalogId);
                hlProduct.Text = pc.Name;
               // hlProductAllegro.Text = pc.AllegroName;

                liId.Text = String.Format("{0}.", gvProductCatalog.PageIndex * gvProductCatalog.PageSize + e.Row.RowIndex + 1);

                if (!String.IsNullOrEmpty(pc.ImageFullName))
                    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}_m{1}", System.IO.Path.GetFileNameWithoutExtension(pc.ImageFullName), System.IO.Path.GetExtension(pc.ImageFullName));
                else
                    imgImage.Visible = false;
                //if (!pc.IsReady)
                //    e.Row.Style.Add("background-color", "silver");

                //hlAllegro.Visible = pc.IsActiveAllegro;
                //hlShop.Visible = pc.ShopProductId.HasValue;
                //hlShop.NavigateUrl = String.Format(hlShop.NavigateUrl, pc.ShopProductId);
                //hlAllegro.NavigateUrl = String.Format(hlAllegro.NavigateUrl, pc.Code, pc.AllegroUserIdAccount);

                lblCodeSupplier.Text = pc.Ean;
                lblCode.Text = pc.Code;
                lblCode2.Text = pc.Code2;
                lblExternalId.Text = pc.ExternalId;
                //Bll.ProductCatalogCalculator calc = new ProductCatalogCalculator(pc.CurrentPrice, pc.AllegroPrice, pc.Rebate.Value, pc.Margin.Value);

                lblSellPrice.Text = String.Format("{0:C}", pc.PriceBruttoFixed);
             
                lblQuantity.Text = pc.LeftQuantity > 0 ? pc.LeftQuantity.ToString() : "-";

                if (pc.SupplierQuantity.HasValue)
                {
                    if (pc.SupplierQuantity.Value == -1)
                        lblSupplierQuantity.Text = String.Format(lblSupplierQuantity.Text, "dost.");
                    else
                        lblSupplierQuantity.Text = String.Format(lblSupplierQuantity.Text, pc.SupplierQuantity);
                    lblSupplierQuantity.Visible = true;
                }
           

            }
        }
       
        private void BindCatalog()
        {
            bool autoSearch = false;

            if (!Page.IsPostBack && !String.IsNullOrEmpty(Request.QueryString["PromotionId"]))
            {
                lbxSearchSupplier.SelectedIndex = -1;
                lsbxStatus.SelectedIndex = -1;
                autoSearch = true;
            }
            if (!Page.IsPostBack && !String.IsNullOrEmpty(Request.QueryString["GroupId"]))
            {
                lbxSearchSupplier.SelectedIndex = -1;
                lsbxStatus.SelectedIndex = -1;
                autoSearch = true;
            }
            if (!Page.IsPostBack && !String.IsNullOrEmpty(Request.QueryString["FileImportId"]))
            {
                //chbNotReady.Checked = true;
                lbxSearchSupplier.SelectedValue = Request.QueryString["SupplierId"];
                lsbxStatus.SelectedIndex = -1;
                autoSearch = true;
            }

            if (Page.IsPostBack || autoSearch)
            {
                List<Dal.ProductCatalogViewFnResult> products = GetProducts().OrderBy(x => x.Code).ToList();//.Where(x=>x.ProductCatalogId==592).ToList();

                 
                gvProductCatalog.PageSize = Int32.Parse(txbPageSize.Text);
                gvProductCatalog.DataSource = products;
                gvProductCatalog.DataBind();
            }


        }
     
 

        private List<Dal.ProductCatalogViewFnResult> GetProducts()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            string name = txbSearchName.Text.Trim();
            int[] supplierIds = lbxSearchSupplier.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();

            int? warehouseId = null;
            int[] w = chblWarehouse.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();

            if (w.Length == 0 || w.Length == chblWarehouse.Items.Count)
                warehouseId = null;
            else
                warehouseId = w[0];

            List<Dal.ProductCatalogViewFnResult> products = oh.GetProductCatalog(name, supplierIds, warehouseId);

            if (lsbxStatus.Items.FindByValue("-1").Selected) products = products.Where(x => x.IsDiscontinued).ToList();
            if (lsbxStatus.Items.FindByValue("-2").Selected) products = products.Where(x => !x.IsDiscontinued).ToList();
            if (lsbxStatus.Items.FindByValue("0").Selected) products = products.Where(x => x.IsActive == false).ToList();
            if (lsbxStatus.Items.FindByValue("1").Selected) products = products.Where(x => x.IsActive == true).ToList();
            // if (lsbxStatus.Items.FindByValue("2").Selected) products = products.Where(x => x.IsActiveAllegro == true).ToList();
            //if (lsbxStatus.Items.FindByValue("3").Selected) products = products.Where(x => x.IsAvailableAllegro == false).ToList();

            // if (lsbxStatus.Items.FindByValue("6").Selected) products = products.Where(x => x.IsActiveOnline == true).ToList();
            //if (lsbxStatus.Items.FindByValue("7").Selected) products = products.Where(x => x.IsAvailableOnline == false).ToList();
            if (lsbxStatus.Items.FindByValue("8").Selected) products = products.Where(x => x.IsAvailable == true && x.IsActive == false && x.IsDiscontinued == false && x.IsHidden == false).ToList();

            if (lsbxStatus.Items.FindByValue("9").Selected) products = products.Where(x => x.IsAvailable == true).ToList();
            if (lsbxStatus.Items.FindByValue("10").Selected) products = products.Where(x => x.IsAvailable == false).ToList();

            if (lsbxStatus.Items.FindByValue("-3").Selected)
                products = products.Where(x => x.IsHidden == true).ToList();
            if (lsbxStatus.Items.FindByValue("-4").Selected)
                products = products.Where(x => x.IsHidden == false).ToList();
            if (lsbxStatus.Items.FindByValue("11").Selected)
                products = products.Where(x => x.IsReady == true).ToList();
            if (lsbxStatus.Items.FindByValue("12").Selected)
                products = products.Where(x => x.IsReady == false).ToList();


            if (chbIsFollowed.Checked)
                products = products.Where(x => x.IsFollowed.HasValue && x.IsFollowed.Value == true).ToList();
            if (chbIsOutlet.Checked)
                products = products.Where(x => x.IsOutlet.HasValue && x.IsOutlet.Value == true).ToList();
            
            decimal priceFrom = 0;
            decimal priceTo = 0;

            if (Decimal.TryParse(txbPriceFrom.Text.Trim(), out priceFrom))
                products = products.Where(x => x.PriceBruttoFixed >=  priceFrom).ToList();
            if (Decimal.TryParse(txbPriceTo.Text.Trim(), out priceTo))
                products = products.Where(x => x.PriceBruttoFixed <=  priceTo).ToList();

            if (chbPromo0.Checked)
                products = products.Where(x => x.IsActivePricePromo == false).ToList();
            if (chbPromo1.Checked)
                products = products.Where(x => x.IsActivePricePromo == true).ToList();
            
            // na końcu to wywołać
             


            if (!String.IsNullOrEmpty(Request.QueryString["FileImportId"]))
            {
                Dal.ProductFileImportHelper pfih = new Dal.ProductFileImportHelper();

                int[] productCatalogIds = pfih.GetProductFileData(Convert.ToInt32(Request.QueryString["FileImportId"])).Where(x => x.ProductCatalogId.HasValue)
                    .Select(x => x.ProductCatalogId.Value).Distinct().ToArray();
                products = products.Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();
            }
            if (!String.IsNullOrEmpty(Request.QueryString["GroupId"]))
            {
                Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();

                int[] productCatalogIds = pch.GetProductCatalogGroupProducts(Convert.ToInt32(Request.QueryString["GroupId"]));
                products = products.Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();
            }

            if (!String.IsNullOrEmpty(Request.QueryString["PromotionId"]))
            {
                Dal.PromotionHelper ph = new Dal.PromotionHelper();
                int[] productCatalogId = ph.GetProductCatalogs(Convert.ToInt32(Request.QueryString["PromotionId"]));

                products = products.Where(x => productCatalogId.Contains(x.ProductCatalogId)).ToList();
            }

 
            total = products.Count;
            totalQuantity = products.Sum(x => x.LeftQuantity.Value);
            return products;
        }


        private void BindShops()
        { 
            chblShops.DataSource = Dal.DbHelper.Shop.GetShops().Where(x=>x.IsActive && x.CanExportProducts).ToList();
            chblShops.DataBind();


        }

        protected void btnAction_Click(object sender, EventArgs e)
        {
            int[] productIds = WebHelper.GetSelectedIds<int>(gvProductCatalog, "chbOrder");

            if (rbtnAddToBatchAll.Checked)
                productIds = GetProducts().Select(x => x.ProductCatalogId).ToArray();

            if (productIds.Count() == 0)
            {
                DisplayMessage("Nie wybrano żadnego przedmiotu");
                return;
            }

            switch (Int32.Parse(ddlAction.SelectedValue))
            {
                case 1: SetStatus(productIds); break;

                case 2: SetProductUpdate(productIds); break;
           
                case 5: SetSchedule(productIds); break;

                case 7: SetSupplierUpdate(productIds); break;
                case 8: SetDeliveryType(productIds); break;
                case 9: SetExportAleo(productIds); break;
                case 11: SetAllegroActions(productIds); break; 
                case 13: SetShopImages(productIds); break;
                case 14: SetProductCatalogGroup(productIds); break;
                case 16: SetShopCreateUpdateProducts(productIds); break;
                case 17: SetProductCatalogNames(productIds); break;
                case 19: SetUploadImages(productIds); break;
                case 20: SetSettings(productIds); break;
                case 21: SetExportToFile(productIds); break;
                //case 22: SetDescriptionForShops(productIds); break;
                case 23: SetDelivery(productIds); break;
            }
        }

        private void SetDelivery(int[] productIds)
        {
            if (ddlShopDelivery.SelectedIndex == 0 )
            {
                DisplayMessage("Należy wybrać nowy czas dostawy");
                return;
            }
            


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogDelivery(productIds, Int32.Parse(ddlShopDelivery.SelectedValue), UserName);
            DisplayMessage(String.Format("Zaktualizowano {0} produktów", productIds.Length));

        }



        private void SetExportToFile(int[] productIds)
        {
            Bll.ProductCatalogHelper pch = new ProductCatalogHelper();
            string fileName = pch.ExportToFile(productIds, new int[] { 139 });

           


            string contentType = contentType = "Application/xml";

            Response.ContentType = contentType;
            Response.AppendHeader("content-disposition", "attachment; filename=" + (new FileInfo(fileName)).Name);

            Response.WriteFile(fileName);
            Response.End();
        }

        private void SetUploadImages(int[] productIds)
        {
            ucUploadImage.SetImages(productIds);
        }
 
        private void SetProductCatalogNames(int[] productIds)
        {
            // Start the long running task on one thread
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(BindNames);

            Thread thread = new Thread(parameterizedThreadStart);

            thread.Start(productIds);

            // Show Modal Progress Window
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "OpenProgressWindow('" + requestId.ToString() + "');", true);


            DisplayMessage("Zmiana nazw została zainicjowana. Wykonuje się w tle i może potrwać kilka minut.");

        }
        private void BindNames(object data)
        {
            int[] productIds = (int[])data;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogByIds(productIds);
            Bll.ProductCatalogHelper pchB = new ProductCatalogHelper();
            pchB.UpdateProductNames(products);
            //Bll.ThreadResult.Add(requestId.ToString(), "Item Processed Successfully.");

        }
        private void SetProductUpdate(int[] productIds)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            int[] selectedShopIds = lsbxShops.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();

            int[] selectedShopColumnTypeIds = chblShopColumnType.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();

            if (selectedShopIds.Length == 0)
            {
                DisplayMessage("Nie wybrano żadnego sklepu do aktualizacji produktów");
                return;
            }
            pch.ProductCatalogShopUpdateScheduleSet(productIds, selectedShopIds, selectedShopColumnTypeIds, UserName);

            DisplayMessage("Produkty zostały zapisane do aktualizacji");
        }

      
        private void SetSettings(int[] productIds)
        {
            //if (ddlSettingsStatus.SelectedIndex == 0 || lsbxSettings.SelectedIndex == -1)
            //{
            //    DisplayMessage("Należy wybrać źródło zmiany ustawień oraz status");
            //    return;
            //} 

            Dal.ProductCatalog pc = new Dal.ProductCatalog();

            
            if (lsbxSettings.Items.FindByValue("0").Selected)
                pc.IsOutlet = ddlSettingsStatus.SelectedValue == "1";
            if (lsbxSettings.Items.FindByValue("1").Selected)
                pc.IsPaczkomatAvailable = ddlSettingsStatus.SelectedValue == "1";

            bool changeLockRebates = true;
            switch (ddlLockRebates.SelectedValue)
            {
                case "1":
                    pc.LockRebates = true; break;
                case "0":
                    pc.LockRebates = false; break;
                case "-1":
                    pc.LockRebates = null; break;
                default:
                    changeLockRebates = false;break;
            }

            if (txbPricePromo.Text != "" && txbPricePromoDate.Text!="")
            {
                if (Convert.ToDecimal(txbPricePromo.Text.Trim()) > 0)
                {
                    pc.PriceBruttoPromo = Convert.ToDecimal(txbPricePromo.Text.Trim()) / 100;

                    calDate.SelectedDate = DateTime.Parse(txbPricePromoDate.Text);

                    pc.PriceBruttoPromoDate = calDate.SelectedDate.Value.AddHours(23).AddMinutes(59);
                }
                else
                {
                    pc.PriceBruttoPromo = 0;
                    pc.PriceBruttoPromoDate = null;
                }
            }


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogSettings(productIds, pc, changeLockRebates, UserName);
            DisplayMessage(String.Format("Zaktualizowano {0} produktów", productIds.Length));
        }
        private void SetStatus(int[] productIds)
        {
            if (ddlStatus.SelectedIndex == 0 || lsbxSource.SelectedIndex == -1)
            {
                DisplayMessage("Należy wybrać źródło zmiany statusu oraz status");
                return;
            }
            bool? isAvailable = null,  isDiscontinued = null, isHidden = null, isReady=null;

            if (lsbxSource.Items.FindByValue("-1").Selected)
                isDiscontinued = ddlStatus.SelectedValue == "1";
            if (lsbxSource.Items.FindByValue("1").Selected)
                isAvailable = ddlStatus.SelectedValue == "1";
            //if (lsbxSource.Items.FindByValue("2").Selected)
            //    isAvailableOnline = ddlStatus.SelectedValue == "1";
            //if (lsbxSource.Items.FindByValue("3").Selected)
            //    isAvailableAllegro = ddlStatus.SelectedValue == "1";
            if (lsbxSource.Items.FindByValue("4").Selected)
                isHidden = ddlStatus.SelectedValue == "1";
            if (lsbxSource.Items.FindByValue("5").Selected)
                isReady = ddlStatus.SelectedValue == "1";


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogStatus(productIds, isAvailable,  isDiscontinued, isHidden, isReady, UserName);
            DisplayMessage(String.Format("Zaktualizowano {0} produktów", productIds.Length));
        }

        private void SetShopCreateUpdateProducts(int[] productIds)
        {

            Bll.ShopHelper sh = new Bll.ShopHelper();
            Dictionary<int, string> results = new Dictionary<int, string>();

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.SupplierDeliveryTypeSource> sources = pch.GetDeliverySources(Dal.Helper.ShopType.ShoperLajtitPl);

            foreach (int productCatalogId in productIds)
            {
                bool result = false;
                try
                {
                    DisplayMessage("BRAK implementacji");
                    return;
                    //result = sh.SetProductUpdateByProductCatalogId(productCatalogId, null, Dal.Helper.UpdateScheduleType.OnlineShopBatch, sources)
                    //&& sh.SetProductUpdateByProductCatalogId(productCatalogId, null, Dal.Helper.UpdateScheduleType.OnlineShopSingle, sources);
                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, "Tworzenie produktu w sklepie");
                }
                if (result)
                    results.Add(productCatalogId, "OK");
                else
                    results.Add(productCatalogId, "Błąd");
            }

            int errorCount = results.Select(x => x.Value != "OK").Count();
            int count = productIds.Length;

            if (errorCount > 0)
                DisplayMessage(String.Format("Utworzono/zaktualizowano {0} z {1} produktów. Liczba błędów {2}", count - errorCount, count, errorCount));
            else
                DisplayMessage(String.Format("Utworzono/zaktualizowano {0} produktów", count));
        }


        private void SetProductCatalogGroup(int[] productIds)
        {

            int?[] supplierIds = lbxSearchSupplier.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => GetIntOrNull(x.Value)).ToArray();
            if (supplierIds.Length != 1)
            {
                DisplayMessage("Wybierz wpierw dostawcę, następnie wyszukaj produkty by móc je dodać do batcha");
                return;
            }
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();
            int productCatalogGroupId = -1;

            if (!String.IsNullOrEmpty(ddlProductCatalogGroup.SelectedValue))
                productCatalogGroupId = Convert.ToInt32(ddlProductCatalogGroup.SelectedValue);

            int familyId = Convert.ToInt32(ddlProductCatalogGroupFamily.SelectedValue);
            Dal.ProductCatalogGroup group;

            if (txbFamilyName.Text.Trim() != "")
            {
                Dal.ProductCatalogGroupFamily pcgf = new Dal.ProductCatalogGroupFamily()
                {
                    FamilyId=0,
                    FamilyName = txbFamilyName.Text.Trim(),
                    SupplierId = Convert.ToInt32(ddlProductCatalogGroupSupplier.SelectedValue),
                    FamilyTypeId = (int)Dal.Helper.ProductCatalogGroupFamilyType.Family
                };
                var family = pch.SetProductCatalogFamilyByName(pcgf);
                familyId = family.FamilyId;
                productCatalogGroupId = -1;
            }


            if (txbGroupName.Text.Trim() != "")
            {
                Dal.ProductCatalogGroup pcg = new Dal.ProductCatalogGroup()
                {
                    FamilyId = familyId,
                    GroupName = txbGroupName.Text.Trim(),
                    SupplierId = Convert.ToInt32(ddlProductCatalogGroupSupplier.SelectedValue)
                };
                group = pch.SetProductCatalogGroup(pcg);
            }
            else
                group = pch.GetProductCatalogGroup(productCatalogGroupId);


            if(group==null)
            {
                DisplayMessage("Grupa produktów jest nieokreślona. Utwórz nową lub wybierz z listy");
                return;
            }

            if (supplierIds[0] != Convert.ToInt32(ddlProductCatalogGroupSupplier.SelectedValue)&&
                group.ProductCatalogGroupFamily.ProductCatalogGroupFamilyType.FamilyTypeId == (int)Dal.Helper.ProductCatalogGroupFamilyType.Family)
            {
                DisplayMessage("Nie możesz zmienić dostawcy przypisując nową grupę");
                return;
            }



            pch.SetProductCatalogGroupsProducts(productIds, group.ProductCatalogGroupId, UserName);
            DisplayMessage(String.Format("Przypisano nową grupę  {0} dla wybranych produktów {1}", ddlProductCatalogGroup.SelectedItem.Text, productIds.Length));

            txbGroupName.Text = "";
            txbFamilyName.Text = "";
            BindCatalog();
        }

        private int? GetIntOrNull(string value)
        {
            int v = Convert.ToInt32(value);

            if (v == 0)
                return null;

            return v;
        }

        private void SetShopImages(int[] productCatalogIds)
        {
            DisplayMessage("Brak implementacji");
            return;
            //Dal.ShopHelper sh = new Dal.ShopHelper();
            //int[] shopProductIds = sh.GetShopProductIdByProductCatalog(productCatalogIds);
             

            //Bll.ShopUpdateHelper.ClickShop.SetImagesUpdate(shopProductIds);
            //DisplayMessage("Zdjęcia zostały zaktualizowane");
        }


        private void SetAllegroActions(int[] productIds)
        {

            int actionTypeId = Convert.ToInt32(ddlAllegroAuction.SelectedValue);
            
            Dal.OrderHelper pch = new Dal.OrderHelper();
            pch.SetAllegroActions(productIds, null, actionTypeId, false, "Usunięte ze strony ProductCatalog.aspx");
            DisplayMessage(String.Format("Zakolejkowana wykonanie akcji {0} na {1} produktach",
                ddlAllegroAuction.SelectedItem.Text,
                productIds.Length));
        }

        private void SetDeliveryType(int[] productIds)
        {

            DisplayMessage("Nie obsługiwane");
        }

        private void SetSupplierUpdate(int[] productIds)
        {
            int supplierId = Convert.ToInt32(ddlSupplierUpdate.SelectedValue);

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogSupplier(productIds, supplierId, UserName);
            DisplayMessage(String.Format("Zmieniono dostawcę dla {0} produktów na {1}", productIds.Length, ddlSupplierUpdate.SelectedItem.Text));

            BindCatalog();
        }

        private void SetSchedule(int[] productIds)
        {
            DisplayMessage("Nic - brak implementacji");
        }

        private void SetExportAleo(int[] productIds)
        {

            //    var products = GetProducts().Where(x => productIds.Contains(x.ProductCatalogId));

            //    List<LajtitShop.Products> ls = new List<LajtitShop.Products>();

            //    foreach (var pc in products)
            //    {

            //        //Bll.ProductCatalogCalculator calc = new ProductCatalogCalculator(pc.CurrentPrice, pc.AllegroPrice, pc.Rebate.Value, pc.Margin.Value);
            //        decimal? price = pc.PriceBrutto;
            //        ls.Add(new LajtitShop.Products()
            //        {
            //            active = "0",
            //            availability = "auto",
            //            //category = "Lajtit Exclusive > " +
            //            //GetShopCategoryName(pc.LampCategory),
            //            delivery = "48 godzin",
            //            description = Bll.OrderHelper.SearchAndReplace(pc.Specification, "").Replace("\n", "<br>") + "<br>" + pc.CommonSpecificationText.Replace("\n", "<br>") + "<br><br>Kod produktu: " + pc.Code,
            //            images1 = String.Format(@"http://{0}/ProductCatalog/{1}", Dal.Helper.StaticLajtitUrl, pc.ImageFullName),
            //            mainpage = "0",
            //            mainpage_priority = "1",
            //            name = pc.AllegroName.Replace('\n', ' ').Replace('\r', ' '),
            //            other_price = "",
            //            pkwiu = "",
            //            price = price.ToString(), //calc.SellPriceBrutto.ToString(),
            //            priority = "1",
            //            producer = "Lajtit",
            //            product_code = pc.Code,
            //            rank = "0",
            //            rank_votes = "0",
            //            short_description = "Kod produktu: " + pc.Code,
            //            stock = pc.LeftQuantity.ToString(),
            //            stock_warnlevel = "1",
            //            unit = "szt.",
            //            vat = "23%",
            //            views = "234",
            //            weight = "0.75"


            //        });
            //    }

            //    Bll.LajtitShop shop = new LajtitShop();
            //    shop.ExportProducts(ls);

            //    DisplayMessage("Plik został wyeksportowany");
        }

        protected void ddlAction_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            pnStatus.Visible =
               pnDeliveryCostType.Visible = pnCreateAuctions.Visible = pAction.Visible = pnScheduleCreate.Visible =
               pnSupplier.Visible = pnAllegroAuction.Visible =  pnUpdateShopImages.Visible =
               pnShopCreateUpdateProducts.Visible = pnProductUpdate.Visible = pnSettings.Visible = pnDelivery.Visible=
               pnCreateNames.Visible = pnImages.Visible = false;

            switch (Int32.Parse(ddlAction.SelectedValue))
            {
                case 0: return;
                case 1: BindStatuses(); break;
                case 2: BindProductUpdate();break;
                case 7: BindSuppliersForUpdate(); break;
                case 8: BindDeliveryCostTypes(); break;
                case 11: BindAllegroActions(); break; 
                case 13: BindShopImages(); break;
                case 14: BindProductCatalogGroups(); break;
                case 16: SetShopCreateUpdateProducts(); break;
                case 17: SetProductCatalogNames(); break;
                case 19: SetUploadImages(); break;
                case 20: BindSettings(); break;
                case 21: SetExportToFile(); break;
                case 22: SetDescriptionForShops(); break;
                case 23: Delivery(); break;
            }
            pAction.Visible = ddlAction.SelectedIndex != 0; ;
        }

        private void Delivery()
        {
            pnDelivery.Visible = true;
         
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                ddlShopDelivery.DataSource = pch.GetDeliveries().Select(x => new
                {
                    DeliveryId = x.DeliveryId,
                    Name = String.Format("{0} godz. ({1} dni)", x.DeliveryHours, x.DeliveryHours / 24)
                }).ToList();
                ddlShopDelivery.DataBind();
          
        }

        private void SetDescriptionForShops()
        {
            pnDescriptions.Visible = true;
            BindShops();
        }

        private void BindProductUpdate()
        {
            pnProductUpdate.Visible = true;
             
            lsbxShops.DataSource = Dal.DbHelper.Shop.GetShops().Where(x => x.IsActive && x.CanExportProducts).ToList();
            lsbxShops.DataBind();
            Dal.ShopHelper sh = new Dal.ShopHelper();
            chblShopColumnType.DataSource = sh.GetShopColumnTypes();
            chblShopColumnType.DataBind();
        }

        private void BindProductCatalogGroups()
        {
            pnProductCatalogGroup.Visible = true;
            ccdSuppliers.SelectedValue = lbxSearchSupplier.SelectedValue;
        }

        private void SetUploadImages()
        {
            pnImages.Visible = true;
        }

        private void SetExportToFile()
        {

        }

        private void SetProductCatalogNames()
        {
            pnCreateNames.Visible = true;
        }

        private void BindStatuses()
        {
            pnStatus.Visible = true;
            if (UserName == "ania")
            {
                foreach (ListItem i in lsbxSource.Items)
                {
                    if (i.Value == "4")
                        i.Enabled = true;
                    else
                        i.Enabled = false;
                }
                //lsbxSource.Enabled = false;
                //lsbxSource.Items.FindByValue("4").Selected = true;

            }
        }
        private void BindSettings()
        {
            pnSettings.Visible = true;
            if (UserName == "ania")
            {
                foreach (ListItem i in lsbxSource.Items)
                {
                    if (i.Value == "4")
                        i.Enabled = true;
                    else
                        i.Enabled = false;
                }
                //lsbxSource.Enabled = false;
                //lsbxSource.Items.FindByValue("4").Selected = true;

            }
        }

        private void SetShopCreateUpdateProducts()
        {
            pnShopCreateUpdateProducts.Visible = true;
        }

        private void BindShopImages()
        {
            pnUpdateShopImages.Visible = true;
        }

     
 
        private void BindDeliveryCostTypes()
        {
            pnDeliveryCostType.Visible = true;

            Dal.OrderHelper ph = new Dal.OrderHelper();

            ddlDeliveryCostType.DataSource = ph.GetAllegroDeliveryCostTypes().Where(x => x.DeliveryCostTypeId != 0).ToList();
            ddlDeliveryCostType.DataBind();
        }

        private void BindSuppliersForUpdate()
        {
            pnSupplier.Visible = true;
             

            ddlSupplierUpdate.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers();
            ddlSupplierUpdate.DataBind();
        }
        private void BindAllegroActions()
        {
            pnAllegroAuction.Visible = true;

            Dal.OrderHelper oh = new Dal.OrderHelper();

            ddlAllegroAuction.DataSource = oh.GetAllegroActions();
            ddlAllegroAuction.DataBind();
        }
    
 

        protected void imgCat_Click(object sender, ImageClickEventArgs e)
        {

            int productCatalogId = Int32.Parse(((ImageButton)sender).CommandArgument);
            ucShopProduct.ProductCatalogId = productCatalogId;
            ucShopProduct.BindProducts(productCatalogId, true);

            mpeShop.Show();           
        }
    }
}