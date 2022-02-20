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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using LajtIt.Dal;

namespace LajtIt.Web
{
    [Developer("52a8955b-ca2a-45d1-8c06-2147ee115a0e")]
    public partial class ProductCatalogForDb : LajtitPage
    {


        private bool canChangeImages = false;

        private Guid? SearchId
        {
            get
            {
                if (ViewState["SearchId"] != null)
                    return Guid.Parse(ViewState["SearchId"].ToString());
                else
                    return null;

            }
            set { ViewState["SearchId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucUploadImage.ImageIds += ImageIds;
            canChangeImages = HasActionAccess(Guid.Parse("ebaac25f-1dc7-4d1e-b1e6-12a75a9f903a"));

            if (!Page.IsPostBack)
            {
                BindShopsSearch();
                BindSuppliers();
                BindCatalog();
                BindSearchTable();
                BindAttributeGroups();
            }
        }
        private void SetDelivery(int[] productIds)
        {
            if (ddlShopDelivery.SelectedIndex == 0)
            {
                DisplayMessage("Należy wybrać nowy czas dostawy");
                return;
            }



            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogDelivery(productIds, Int32.Parse(ddlShopDelivery.SelectedValue), UserName);
            DisplayMessage(String.Format("Zaktualizowano {0} produktów", productIds.Length));

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
        private void BindShopsSearch()
        {
            ddlShopsSearch.DataSource = Dal.DbHelper.Shop.GetShops().Where(x => x.IsActive && x.CanExportProducts).OrderBy(x => x.Name).ToList();
            ddlShopsSearch.DataBind();
            // ddlShopsSearch.SelectedValue = "1";
        }

        private void BindAttributeGroups()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            chblAttributeGroups.DataSource = pch.GetProductCatalogAttributeGroups().OrderBy(x => x.Name).ToList();
            chblAttributeGroups.DataBind();
        }

        protected int[] ImageIds(object sender, EventArgs e)
        {
            int[] productIds = WebHelper.GetSelectedIds<int>(gvProductCatalog, "chbOrder");

            if (rbtnAddToBatchAll.Checked)
                productIds = GetProducts().Select(x => x.ProductCatalogId).ToArray();


            return productIds;

        }



        private void BindSuppliers()
        {
            lbxSearchSupplier.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderBy(x => x.Name);
            lbxSearchSupplier.DataBind();

            //lbxSearchSupplier.SelectedValue = "3";
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindCatalog();
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

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogViewDbFnResult pc = e.Row.DataItem as Dal.ProductCatalogViewDbFnResult;

                //HyperLink hlCalcuator = e.Row.FindControl("hlCalcuator") as HyperLink;
                // hlCalcuator.NavigateUrl = String.Format("/ProductCalculator.aspx?id={0}", pc.ProductCatalogId);
                Image imgImage = e.Row.FindControl("imgImage") as Image;

                Literal liId = e.Row.FindControl("liId") as Literal;
                HyperLink hlProduct = e.Row.FindControl("hlProduct") as HyperLink;
                // HyperLink hlProductAllegro = e.Row.FindControl("hlProductAllegro") as HyperLink;
                HyperLink hlPreview = e.Row.FindControl("hlPreview") as HyperLink;

                Label lblCode = e.Row.FindControl("lblCode") as Label;
                Label lblCode2 = e.Row.FindControl("lblCode2") as Label;
                Label lblCodeSupplier = e.Row.FindControl("lblCodeSupplier") as Label;
                Label lblExternalId = e.Row.FindControl("lblExternalId") as Label;
                // Label lblNewName = e.Row.FindControl("lblNewName") as Label;


                if (chbImages.Checked)
                {
                    ProductImages ucProductImages = e.Row.FindControl("ucProductImages") as ProductImages;
                    ucProductImages.BindImages(pc.ProductCatalogId, canChangeImages);
                    ucProductImages.Visible = true;
                }

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


                lblCodeSupplier.Text = pc.Ean;
                lblCode.Text = pc.Code;
                lblCode2.Text = pc.Code2;
                lblExternalId.Text = pc.ExternalId;


                if (chbAttributesSearch.Checked)
                {
                    LajtIt.Web.Controls.ProductAttributes ucProductAttributesProduct = e.Row.FindControl("ucProductAttributesProduct") as LajtIt.Web.Controls.ProductAttributes;
                    ucProductAttributesProduct.ProductCatalogId = pc.ProductCatalogId;
                    ucProductAttributesProduct.EnableAdding = false;
                    ucProductAttributesProduct.EnableBindGrouping = false;
                    // if (chbAttributeOnly.Checked)
                    ucProductAttributesProduct.BindAttributeGroups(GetAttributeGroupIds());
                    //else
                    //    ucProductAttributesProduct.BindAttributeGroups();
                }

            }
        }

        private int[] GetAttributeGroupIds()
        {
            int count = chblAttributeGroups.Items.Count;

            int[] result = chblAttributeGroups.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();

            if (count == result.Length || result.Length == 0)
                return chblAttributeGroups.Items.Cast<ListItem>().Select(x => Int32.Parse(x.Value)).ToArray();
            else
                return result;


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
            if (!Page.IsPostBack && !String.IsNullOrEmpty(Request.QueryString["action"]))
            {

                lbxSearchSupplier.SelectedValue = Request.QueryString["SupplierId"];
                lsbxStatus.SelectedIndex = -1;
                lsbxStatus.Items.FindByValue("9").Selected = true;
                lsbxStatus.Items.FindByValue("12").Selected = true;
                lsbxStatus.Items.FindByValue("-2").Selected = true;

                autoSearch = true;
            }
            if (Page.IsPostBack || autoSearch)
            {
                List<Dal.ProductCatalogViewDbFnResult> products = GetProducts().OrderBy(x => x.Code).ToList();//.Where(x=>x.ProductCatalogId==592).ToList();


                //gvProductCatalog.Columns[4].Visible = !chbAttributesSearch.Checked;
                gvProductCatalog.Columns[4].Visible = /*gvProductCatalog.Columns[6].Visible = */  chbAttributesSearch.Checked;

                gvProductCatalog.PageSize = Int32.Parse(txbPageSize.Text);
                gvProductCatalog.DataSource = products;
                gvProductCatalog.DataBind();

                txbPageNo.Text = (gvProductCatalog.PageIndex + 1).ToString();
                lblPageNo.Text = String.Format("/{0}", gvProductCatalog.PageCount);
                lblCount.Text = String.Format("{0}", products.Count());
            }


        }
        protected void chbAttributesSearch_Click(object sender, EventArgs e)
        {
            //ddlAttributes.Enabled = ddlAttributesExists.Enabled = ddlAttributesValue.Enabled = ddlAttributesValueExists.Enabled = chbAttributesSearch.Checked;
        }
        protected void ddlAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BinAttributes();

        }

        //private void BinAttributes()
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
        //    ddlAttributesValue.DataSource = pch.GetProductCatalogAttributes(Convert.ToInt32(ddlAttributes.SelectedValue));
        //    ddlAttributesValue.DataBind();
        //}

        private List<Dal.ProductCatalogViewDbFnResult> GetProducts()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            string name = txbSearchName.Text.Trim();
            int[] supplierIds = lbxSearchSupplier.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();
            int? shopId = null;
            if (ddlShopsSearch.SelectedIndex > 0)
                shopId = Int32.Parse(ddlShopsSearch.SelectedValue);
            LajtitDB ctx = new LajtitDB();
            IQueryable<Dal.ProductCatalogViewDbFnResult> products = oh.GetProductCatalogForDb(ctx, shopId, name, supplierIds, SearchId);

            if (lsbxStatus.Items.FindByValue("-1").Selected) products = products.Where(x => x.IsDiscontinued);
            if (lsbxStatus.Items.FindByValue("-2").Selected) products = products.Where(x => !x.IsDiscontinued);
            if (lsbxStatus.Items.FindByValue("0").Selected) products = products.Where(x => x.IsActive == false);
            if (lsbxStatus.Items.FindByValue("1").Selected) products = products.Where(x => x.IsActive == true);
            // if (lsbxStatus.Items.FindByValue("2").Selected) products = products.Where(x => x.IsActiveAllegro == true);
            // if (lsbxStatus.Items.FindByValue("3").Selected) products = products.Where(x => x.IsAvailableAllegro == false);

            // if (lsbxStatus.Items.FindByValue("6").Selected) products = products.Where(x => x.IsActiveOnline == true);
            //  if (lsbxStatus.Items.FindByValue("7").Selected) products = products.Where(x => x.IsAvailableOnline == false);
            if (lsbxStatus.Items.FindByValue("8").Selected) products = products.Where(x => x.IsAvailable == true && x.IsActive == false && x.IsDiscontinued == false && x.IsHidden == false);

            if (lsbxStatus.Items.FindByValue("9").Selected) products = products.Where(x => x.IsAvailable == true);
            if (lsbxStatus.Items.FindByValue("10").Selected) products = products.Where(x => x.IsAvailable == false);

            if (lsbxStatus.Items.FindByValue("-3").Selected)
                products = products.Where(x => x.IsHidden == true);
            if (lsbxStatus.Items.FindByValue("-4").Selected)
                products = products.Where(x => x.IsHidden == false);
            if (lsbxStatus.Items.FindByValue("11").Selected)
                products = products.Where(x => x.IsReady == true);
            if (lsbxStatus.Items.FindByValue("12").Selected)
                products = products.Where(x => x.IsReady == false);


            if (chbWithoutShopAssigment.Checked)
                products = products.Where(x => x.ShopProductId == null);
            if (chbWithShopAssigment.Checked)
                products = products.Where(x => x.ShopProductId != null);


            if (chbImage0.Checked)
                products = products.Where(x => x.ImageFullName == null);
            if (chbImage1.Checked)
                products = products.Where(x => x.ImageFullName != null);
            if (chbExtId0.Checked)
                products = products.Where(x => String.IsNullOrEmpty(x.ExternalId) == true);
            if (chbExtId1.Checked)
                products = products.Where(x => String.IsNullOrEmpty(x.ExternalId) == false);
            if (chbPrice0.Checked)
                products = products.Where(x => x.PriceBruttoFixed == 0);
            if (chbPrice1.Checked)
                products = products.Where(x => x.PriceBruttoFixed > 0);

            if (chbDescription0.Checked)
                products = products.Where(x => x.HasDescription == 0);
            if (chbDescription1.Checked)
                products = products.Where(x => x.HasDescription == 1);


            // na końcu to wywołać
            //if (chbAttributesSearch.Checked)
            //{

            //    List<AttributesSearch> attributes = GetAttributes();



            //    //    bool? exists = null;
            //    //    bool? existsValue = null;

            //    //    if (ddlAttributesExists.SelectedIndex != 0)
            //    //        exists = ddlAttributesExists.SelectedIndex == 1;

            //    //    int attributeGroupId = Convert.ToInt32(ddlAttributes.SelectedValue);
            //    //    int? attributeId = null;
            //    //    if (ddlAttributesValueExists.SelectedIndex != 0)
            //    //    {
            //    //        existsValue = ddlAttributesValueExists.SelectedIndex == 1; ;
            //    //        attributeId = Convert.ToInt32(ddlAttributesValue.SelectedValue);
            //    //    }
            //    //    int[] filteredProductCatalogIds = products.Select(x => x.ProductCatalogId).ToArray();
            //    //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            //    //    int[] foundProductCatalogIds = pch.GetProductCatalogAttributes(filteredProductCatalogIds, attributeGroupId, exists, attributeId, existsValue);

            //    //    products = products.Where(x => foundProductCatalogIds.Contains(x.ProductCatalogId)).ToList();
            //}



            List<ProductCatalogViewDbFnResult> productsResult = products.ToList();

            if (!String.IsNullOrEmpty(Request.QueryString["FileImportId"]))
            {
                Dal.ProductFileImportHelper pfih = new Dal.ProductFileImportHelper();

                int[] productCatalogIds = pfih.GetProductFileData(Convert.ToInt32(Request.QueryString["FileImportId"])).Where(x => x.ProductCatalogId.HasValue)
                    .Select(x => x.ProductCatalogId.Value).Distinct().ToArray();
                productsResult = productsResult.Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();
            }
            if (!String.IsNullOrEmpty(Request.QueryString["GroupId"]))
            {
                Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();

                int[] productCatalogIds = pch.GetProductCatalogGroupProducts(Convert.ToInt32(Request.QueryString["GroupId"]));
                productsResult = productsResult.Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();
            }

            if (!String.IsNullOrEmpty(Request.QueryString["PromotionId"]))
            {
                Dal.PromotionHelper ph = new Dal.PromotionHelper();
                int[] productCatalogId = ph.GetProductCatalogs(Convert.ToInt32(Request.QueryString["PromotionId"]));

                productsResult = productsResult.Where(x => productCatalogId.Contains(x.ProductCatalogId)).ToList();
            }



            if (chbGroup0.Checked || chbGroup1.Checked)
            {
                int[] productIdsWithGroups = oh.GetProductCatalogWithGroups();
                if (chbGroup0.Checked)
                    productsResult = productsResult.Where(x => !productIdsWithGroups.Contains(x.ProductCatalogId)).ToList();
                if (chbGroup1.Checked)
                    productsResult = productsResult.Where(x => productIdsWithGroups.Contains(x.ProductCatalogId)).ToList();
            }


            return productsResult;
        }


        private void BindShops()
        {
            rblShops.DataSource = Dal.DbHelper.Shop.GetShops().Where(x => x.IsActive && x.CanExportProducts).ToList();
            rblShops.DataBind();


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
                case 12: SetAttributes(productIds); break;
                case 14: SetProductCatalogGroup(productIds); break;
                case 17: SetProductCatalogNames(productIds); break;
                case 19: SetUploadImages(productIds); break;
                case 21: SetExportToFile(productIds); break;
                case 22: SetDescriptionForShops(productIds); break;
                case 23: SetDelivery(productIds); break;
            }
        }


        private void SetDescriptionForShops(int[] productIds)
        {
            if (rblShops.SelectedIndex == -1)
            {
                DisplayMessage("Wybierz sklep dla którego chcesz generować opisy");
                return;
            }

            // Start the long running task on one thread
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(SetDescriptions);

            Thread thread = new Thread(parameterizedThreadStart);

            thread.Start(productIds);



            DisplayMessage("Zmiana nazw została zainicjowana. Wykonuje się w tle i może potrwać kilka minut.");
        }
        private void SetDescriptions(object data)
        {
            int[] productIds = (int[])data;

            int shopId = Int32.Parse(rblShops.SelectedValue);


            Dal.Helper.Shop s = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopId);


            Bll.ShopHelper pchB = new Bll.ShopHelper();
            pchB.SetShopDescriptions(productIds, s);

        }

        private void SetExportToFile(int[] productIds)
        {
            Bll.ProductCatalogHelper pch = new Bll.ProductCatalogHelper();
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
            int[] shopIds = lbxShops.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();

            if (shopIds.Length == 0)
            {
                DisplayMessage("Nie wybrano żadnego sklepu");
                return;
            }


            // Start the long running task on one thread
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(BindNames);

            Thread thread = new Thread(parameterizedThreadStart);

            object[] par = new object[] { productIds, shopIds };
            thread.Start(par);

            // Show Modal Progress Window
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "OpenProgressWindow('" + requestId.ToString() + "');", true);


            DisplayMessage("Zmiana nazw została zainicjowana. Wykonuje się w tle i może potrwać kilka minut.");

        }
        private void BindNames(object data)
        {
            object[] par = (object[])data;
            int[] productIds = (int[])par[0];
            int[] shopIds = (int[])par[1];

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalogByIds(productIds);
            Bll.ProductCatalogHelper pchB = new Bll.ProductCatalogHelper();


            pchB.UpdateProductNames(products, shopIds);
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


        private void SetStatus(int[] productIds)
        {
            if (ddlStatus.SelectedIndex == 0 || lsbxSource.SelectedIndex == -1)
            {
                DisplayMessage("Należy wybrać źródło zmiany statusu oraz status");
                return;
            }
            bool? isAvailable = null, isDiscontinued = null, isHidden = null, isReady = null;

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
            pch.SetProductCatalogStatus(productIds, isAvailable, isDiscontinued, isHidden, isReady, UserName);
            DisplayMessage(String.Format("Zaktualizowano {0} produktów", productIds.Length));
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
                    FamilyId = 0,
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


            if (group == null)
            {
                DisplayMessage("Grupa produktów jest nieokreślona. Utwórz nową lub wybierz z listy");
                return;
            }

            if (supplierIds[0] != Convert.ToInt32(ddlProductCatalogGroupSupplier.SelectedValue) &&
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



        private void SetSchedule(int[] productIds)
        {
            DisplayMessage("Nic - brak implementacji");
        }


        protected void ddlAction_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            pnStatus.Visible = pnDescriptions.Visible =
                 pAction.Visible =
               pnSupplier.Visible = pnAttributes.Visible = pnUpdateShopImages.Visible =
               pnShopCreateUpdateProducts.Visible = pnProductUpdate.Visible =
               pnCreateNames.Visible = pnImages.Visible = false;

            switch (Int32.Parse(ddlAction.SelectedValue))
            {
                case 0: return;
                case 1: BindStatuses(); break;
                case 2: BindProductUpdate(); break;
                case 7: BindSuppliersForUpdate(); break;
                case 12: BindAttributes(); break;
                case 14: BindProductCatalogGroups(); break;
                case 16: SetShopCreateUpdateProducts(); break;
                case 17: SetProductCatalogNames(); break;
                case 19: SetUploadImages(); break;
                case 21: SetExportToFile(); break;
                case 22: SetDescriptionForShops(); break;
                case 23: Delivery(); break;
            }
            pAction.Visible = ddlAction.SelectedIndex != 0; ;
        }


        private void SetDescriptionForShops()
        {
            pnDescriptions.Visible = true;
            BindShops();
        }

        private void BindProductUpdate()
        {
            pnProductUpdate.Visible = true;

            Dal.ShopHelper sh = new Dal.ShopHelper();
            lsbxShops.DataSource = Dal.DbHelper.Shop.GetShops().Where(x => x.IsActive && x.CanExportProducts).ToList();
            lsbxShops.DataBind();

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
            lbxShops.DataSource = Dal.DbHelper.Shop.GetShops().Where(x => x.IsActive && x.CanExportProducts).ToList();
            lbxShops.DataBind();
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


        private void SetShopCreateUpdateProducts()
        {
            pnShopCreateUpdateProducts.Visible = true;
        }



        private void BindAttributes()
        {
            pnAttributes.Visible = true;
            ucProductAttributes.EnableBindGrouping = false;
            ucProductAttributes.EnableAdding = false;
            ucProductAttributes.BindAttributeGroups();
        }

        private void SetAttributes(int[] productIds)
        {
            ucProductAttributes.SetAttributes(productIds, false);//, false, null);
            DisplayMessage(String.Format("Ustawiono atrybuty dla {0} produktów", productIds.Length));

        }


        private void BindSuppliersForUpdate()
        {
            pnSupplier.Visible = true;


            ddlSupplierUpdate.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers();
            ddlSupplierUpdate.DataBind();
        }

        protected void btnAction_Click1(object sender, EventArgs e)
        {
            bool isReady = true;
            foreach (GridViewRow row in gvProductCatalog.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int productCatalogId = Convert.ToInt32(gvProductCatalog.DataKeys[row.RowIndex][0]);
                    if (chbAttributesSearch.Checked)
                    {
                        //bool clearBeforeAdd = !chbAttributeOnly.Checked;
                        //int? groupIdToClear = null;
                        //if (chbAttributeOnly.Checked)
                        //    groupIdToClear = Convert.ToInt32(ddlAttributes.SelectedValue);

                        LajtIt.Web.Controls.ProductAttributes ucProductAttributesProduct = row.FindControl("ucProductAttributesProduct") as LajtIt.Web.Controls.ProductAttributes;
                        ucProductAttributesProduct.ProductCatalogId = productCatalogId;

                        if (ucProductAttributesProduct.AttributesSave(productCatalogId, true))//, clearBeforeAdd, groupIdToClear))
                            isReady = false;
                    }
                }
            }
            if (isReady)
                DisplayMessage("Zapisano");
            else

                DisplayMessage("Zmiany zostały zapisane. Niektóre produkty wymagają uzupełnienia obowiązkowych atrybutów");
            BindCatalog();

        }



        protected void txbPageNo_TextChanged(object sender, EventArgs e)
        {
            gvProductCatalog.PageIndex = Int32.Parse(txbPageNo.Text) - 1;
            BindCatalog();
        }

        public class AttributesSearch
        {
            public string Id { get; set; }
            public bool Exists { get; set; }
            public string Name { get; set; }
            public decimal? From { get; set; }
            public decimal? To { get; set; }
            public string StringValue { get; set; }
            public int? Type  { get; set; }
        }
        protected void btnAttributeAdd_Click(object sender, ImageClickEventArgs e)
        {
            List<AttributesSearch> attributes = GetAttributes();

            //if (attributes.Where(x => x.Id == hfAttribute.Value).Count() > 0)
            //{
            //    DisplayMessage("Atrybut jest już wybrany");
            //    return;
            //}
            //else
            //    attributes.Add(new AttributesSearch()
            //    {
            //        Id = hfAttribute.Value,
            //        Exists = ddlAttributeExists.SelectedIndex == 0,
            //        Name = txbAttribute.Text
            //    });

            string[] ids = hfAttribute.Value.Split(new char[] { '-' });


            Dal.SearchTableAttributes searchAttribute = new Dal.SearchTableAttributes()
            {
                AttributeExists = ddlAttributeExists.SelectedIndex == 0,
                AttributeGroupId = Int32.Parse(ids[2]),
                
            };

            if (!SearchId.HasValue)
            {

                SearchId = Guid.NewGuid();

                Dal.SearchTable st = new SearchTable()
                {
                    InsertDate = DateTime.Now,
                    InsertUser = UserName,
                    IsPublic = false,
                    SearchId = SearchId.Value,
                    Title = "Nowy "
                };
                searchAttribute.SearchTable = st;
            }
            else
                searchAttribute.SearchId = SearchId.Value;

            string groupOrAttribute = ids[1];
            if (groupOrAttribute == "A")
                searchAttribute.AttributeId = Int32.Parse(ids[3]);



            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetSearchTableAttributeAdd(searchAttribute);


            BindAttributesFromDb();

            txbAttribute.Text = "";


        }

        private void SaveAttributes(List<AttributesSearch> attributes)
        {
            List<Dal.SearchTableAttributes> searchAttributes = new List<Dal.SearchTableAttributes>();
            if (SearchId == null)
            {
                SearchId = Guid.NewGuid();
                if (txbSearchAttributeName.Text == "")
                    txbSearchAttributeName.Text = String.Format("Nowy {0:yyyy/MM/dd HH:mm}", DateTime.Now);
            }
            Dal.SearchTable st = new Dal.SearchTable()
            {
                InsertDate = DateTime.Now,
                InsertUser = UserName,
                Title = txbSearchAttributeName.Text.Trim(),
                IsPublic = chbSearchIsPublic.Checked,
                SearchId = SearchId.Value
            };

            foreach (AttributesSearch attribute in attributes)
            {
                string[] ids = attribute.Id.Split(new char[] { '-' });

                string groupOrAttribute = ids[1];

                Dal.SearchTableAttributes searchAttribute = new Dal.SearchTableAttributes()
                {
                    Id=Int32.Parse(ids[0]),
                    AttributeExists = attribute.Exists,
                    AttributeGroupId = Int32.Parse(ids[2]),
                    ValueFrom=attribute.From,
                    ValueTo=attribute.To,
                    ValueString= attribute.StringValue,
                    SearchTable = st
                };

                if (groupOrAttribute == "A")
                    searchAttribute.AttributeId = Int32.Parse(ids[3]);

                searchAttributes.Add(searchAttribute);

            }

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetSearchTableAttributes(SearchId.Value, searchAttributes);

            BindSearchTable();
            lbxSearchTable.SelectedValue = SearchId.ToString();

        }



        private void BindAttributes(bool save, List<AttributesSearch> attributes)
        {
            gvAttributes.DataSource = attributes.OrderBy(x => x.Name).ToList();
            gvAttributes.DataBind();
            mpeAttributes.Show();

            if (attributes.Count() > 0)
                lbtnAttributes.Text = String.Format("wybrano {0}", attributes.Count());
            else
                lbtnAttributes.Text = "wybierz";

            if (save)
                SaveAttributes(attributes);
        }

        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            gvAttributes.DataSource = null;
            gvAttributes.DataBind();
            lbtnAttributes.Text = "wybierz";
            ViewState["SearchId"] = null;
            txbSearchAttributeName.Text = "";
            chbSearchIsPublic.Checked = false;
            lbxSearchTable.SelectedIndex = -1;
        }


        protected void gvAttributes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string[] idToDelete = gvAttributes.DataKeys[e.RowIndex][0].ToString().Split(new char[] { '-' });

            //List<AttributesSearch> attributes = new List<AttributesSearch>();

            //GetAttributes(
            //foreach (GridViewRow row in gvAttributes.Rows)
            //{

            //    string id = gvAttributes.DataKeys[row.RowIndex][0].ToString();
            //    bool exists = ((CheckBox)row.Cells[2].Controls[0]).Checked;

            //    if (id != idToDelete)
            //        attributes.Add(new AttributesSearch()
            //        {
            //            Id = id,
            //            Exists = exists,
            //            Name = row.Cells[1].Text
            //        });
            //}
            int attributeGroupId = Int32.Parse(idToDelete[2]);

            string groupOrAttribute = idToDelete[1];
            int? attributeId = null;

          
                if (groupOrAttribute == "A")
                    attributeId = Int32.Parse(idToDelete[3]);

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetSearchTableAttributeDelete(SearchId.Value, attributeGroupId, attributeId);

            BindAttributesFromDb();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SearchId.HasValue == false)
            {
                SearchId = Guid.NewGuid();


            }
            Dal.SearchTable st = new Dal.SearchTable()
            {
                InsertDate = DateTime.Now,
                InsertUser = UserName,
                IsPublic = chbSearchIsPublic.Checked,
                SearchId = SearchId.Value,
                Title = txbSearchAttributeName.Text
            };

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetSearchTable(st);
            BindSearchTable();
            lbxSearchTable.SelectedValue = SearchId.ToString();
        }

        private void BindSearchTable()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Dal.SearchTable> st = pch.GetSearchTables(UserName).Take(10).ToList();

            lbxSearchTable.DataSource = st;
            lbxSearchTable.DataBind();

        }

        protected void lbxSearchTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchId = Guid.Parse(lbxSearchTable.SelectedValue);
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.SearchTable st = pch.GetSearchTable(SearchId.Value);

            txbSearchAttributeName.Text = st.Title;
            chbSearchIsPublic.Checked = st.IsPublic;

            BindAttributesFromDb();

        }

        private void BindAttributesFromDb()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.SearchTableAttributes> attributes = pch.GetSearchTableAttributes(SearchId.Value);


            BindAttributes(false, attributes.Select(x => new AttributesSearch()
            {
                Exists = x.AttributeExists,
                Name = GetName(x),
                Id = GetId(x),
                From = x.ValueFrom,
                To = x.ValueTo,
                StringValue = x.ValueString,
                Type = x.ProductCatalogAttribute?.AttributeTypeId
            }
               ).ToList());
        }

        private string GetName(SearchTableAttributes x)
        {
            if (x.AttributeId.HasValue == false)
                return String.Format("[{0}]", x.ProductCatalogAttributeGroup.Name);
            else
                return String.Format("[{0}].[{1}]", x.ProductCatalogAttributeGroup.Name, x.ProductCatalogAttribute.Name);
        }

        private string GetId(SearchTableAttributes x)
        {
            if (x.AttributeId.HasValue == false)
                return String.Format("{1}-G-{0}", x.AttributeGroupId,x.Id);
            else
                return String.Format("{2}-A-{0}-{1}", x.AttributeGroupId, x.AttributeId, x.Id);
        }
        private List<AttributesSearch> GetAttributes()
        {
            if (!SearchId.HasValue)
                return null;
                //SearchId = Guid.NewGuid();


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

           List<SearchTableAttributes> atts =  pch.GetSearchTableAttributes(SearchId.Value);

            return atts.Select(x => new AttributesSearch()
            {
                Id = GetId(x),
                Exists=x.AttributeExists,
                From=x.ValueFrom,
                Name=GetName(x),
                StringValue=x.ValueString,
                To=x.ValueTo,
                Type=x.ProductCatalogAttribute?.AttributeTypeId

            }).ToList();

            //List<AttributesSearch> attributes = new List<AttributesSearch>();
            //foreach (GridViewRow row in gvAttributes.Rows)
            //{

            //    string id = gvAttributes.DataKeys[row.RowIndex][0].ToString();
            //    bool exists = ((CheckBox)row.Cells[2].Controls[0]).Checked;
            //    TextBox txbTo = row.FindControl("txbTo") as TextBox;
            //    TextBox txbFrom = row.FindControl("txbFrom") as TextBox;
            //    TextBox txbText = row.FindControl("txbText") as TextBox;
            //    Panel pnRange = row.FindControl("pnRange") as Panel;
            //    Panel pnText = row.FindControl("pnText") as Panel;


            //    var a = new AttributesSearch()
            //    {
            //        Id = id,
            //        Exists = exists,
            //        Name = row.Cells[1].Text
            //    };
            //    //if(pnRange.Visible)
            //    //{
            //    //    if (txbFrom.Text != "")
            //    //        a.From = Decimal.Parse(txbFrom.Text);
            //    //    if (txbTo.Text != "")
            //    //        a.To = Decimal.Parse(txbTo.Text);
            //    //}

            //    //if (pnText.Visible && txbText.Text != "")
            //    //    a.StringValue = txbText.Text;

            //    attributes.Add(a);
            //}

            //return attributes;
        }
        protected void gvAttributes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AttributesSearch a = e.Row.DataItem as AttributesSearch;

                Label lblName = e.Row.FindControl("lblName") as Label;
                TextBox txbTo = e.Row.FindControl("txbTo") as TextBox;
                TextBox txbFrom = e.Row.FindControl("txbFrom") as TextBox;
                TextBox txbText = e.Row.FindControl("txbText") as TextBox;
                Panel pnRange = e.Row.FindControl("pnRange") as Panel;
                Panel pnText = e.Row.FindControl("pnText") as Panel;

                switch (a.Type)
                {
                    case 1:
                        lblName.Text = String.Format("{0} od <b>{1}</b> do <b>{2}</b>", a.Name, a.From, a.To) ; break;
                    case 2:
                        lblName.Text = String.Format("{0} zawiera %<b>{1}</b>%", a.Name, a.StringValue); break;
                    default:
                        lblName.Text = a.Name; break;

                }




                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    lblName.Text = a.Name;
                    switch (a.Type)
                    {
                        case 1: pnRange.Visible = true;
                            if (a.From.HasValue)
                                txbFrom.Text = String.Format("{0}", a.From);
                            if (a.To.HasValue)
                                txbTo.Text = String.Format("{0}", a.To);

                            break;
                        case 2: pnText.Visible = true;
                            txbText.Text = a.StringValue;                            
                            break;
                    }
                }
            }
        }

        protected void gvAttributes_RowEditing(object sender, GridViewEditEventArgs e)
        {

            gvAttributes.EditIndex = e.NewEditIndex;
            BindAttributes(true, GetAttributes());
       
        }

        protected void gvAttributes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            gvAttributes.EditIndex = -1;
            BindAttributes(true, GetAttributes());
        }

        protected void gvAttributes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvAttributes.Rows[e.RowIndex];

            string id = gvAttributes.DataKeys[e.RowIndex][0].ToString();

            string[] ids = id.Split(new char[] { '-' });
             
             
            TextBox txbTo = row.FindControl("txbTo") as TextBox;
            TextBox txbFrom = row.FindControl("txbFrom") as TextBox;
            TextBox txbText = row.FindControl("txbText") as TextBox;
            Panel pnRange = row.FindControl("pnRange") as Panel;
            Panel pnText = row.FindControl("pnText") as Panel;

            string groupOrAttribute = ids[1];

            Dal.SearchTableAttributes searchAttribute = new Dal.SearchTableAttributes()
            {
                Id=Int32.Parse(ids[0]),
                AttributeExists = ((CheckBox)row.Cells[2].Controls[0]).Checked,
                AttributeGroupId = Int32.Parse(ids[2]), 
                SearchId = SearchId.Value
            };
            if (pnRange.Visible && txbFrom.Text != "")
                searchAttribute.ValueFrom = Decimal.Parse(txbFrom.Text);
            if (pnRange.Visible && txbTo.Text != "")
                searchAttribute.ValueTo = Decimal.Parse(txbTo.Text);
            if (pnText.Visible && txbText.Text != "")
                searchAttribute.ValueString = txbText.Text;


            if (groupOrAttribute == "A")
                searchAttribute.AttributeId = Int32.Parse(ids[2]);


            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            pch.SetSearchTableAttribute(searchAttribute);


            gvAttributes.EditIndex = -1;
            BindAttributes(true, GetAttributes());
        }
    }
}