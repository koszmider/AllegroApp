using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;

namespace LajtIt.Web
{
    [Developer("4a8ae325-2f45-4922-8c5c-7eefb3737f94")]
    public partial class Promotion : LajtitPage
    {
        private int PromotionId { get { return Convert.ToInt32(Request.QueryString["id"].ToString()); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindPromotion();
                SetControls();
                BindFiles();
                BindProducts();
                hlProductCatalog.NavigateUrl = String.Format(hlProductCatalog.NavigateUrl, PromotionId);
            }
        }
        private void BindPromotion()
        {
            Dal.PromotionHelper ph = new Dal.PromotionHelper();

            Dal.Promotion promotion = ph.GetPromotion(PromotionId);
            List<Dal.PromotionCondition> conditions = ph.GetPromotionConditions(PromotionId);


            txbName.Text = promotion.Name;
            txbDateFrom.Text = promotion.StartDate.ToShortDateString();
            calDateFrom.SelectedDate = promotion.StartDate;
            //chbIsWatekmarkActive.Checked = promotion.IsWatekmarkActive;
            txbDescription.Text = promotion.Description;

            if (promotion.EndDate.HasValue)
            {
                txbDateTo.Text = promotion.EndDate.Value.ToShortDateString();
                calDateTo.SelectedDate = promotion.EndDate;
            }
            List<Dal.Supplier> suppliersSelected = ph.GetPromotionSuppliers(PromotionId);
            int[] suppliersSelectedIds = suppliersSelected.Select(x => x.SupplierId).ToArray();
            List<Dal.Shop> shopsSelected = ph.GetPromotionShops(PromotionId);
            int[] shopsSelectedIds = shopsSelected.Select(x => x.ShopId).ToArray();
            List<Dal.ProductCatalogAttribute> attributesSelected = ph.GetPromotionAttributes(PromotionId);
            int[] attributesSelectedIds = attributesSelected.Select(x => x.AttributeId).ToArray();


            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.Supplier> suppliers = Dal.DbHelper.ProductCatalog.GetSuppliers();


            lbxSuppliersSelected.DataSource = suppliersSelected;
            lbxSuppliersSelected.DataBind();

            lbxSuppliers.DataSource = suppliers.Where(x => !suppliersSelectedIds.Contains(x.SupplierId)).ToList();
            lbxSuppliers.DataBind();

            List<Dal.Shop> shops = oh.GetShops();


            lbxShopsSelected.Items.Clear();
            lbxShops.Items.Clear();
            lbxShopsSelected.DataSource = shopsSelected;
            lbxShopsSelected.DataBind();

            lbxShops.DataSource = shops.Where(x => !shopsSelectedIds.Contains(x.ShopId)).ToList();
            lbxShops.DataBind();

            SetPromotionConditions(conditions);

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();


            List<Dal.ProductCatalogAttribute> attributes = pch.GetProductCatalogAttributes().Where(x => x.ProductCatalogAttributeGroup.AttributeGroupTypeId==1 || x.ProductCatalogAttributeGroup.AttributeGroupTypeId == 2).ToList() ;

            var groups = attributes.Select(x => x.ProductCatalogAttributeGroup).Distinct().OrderBy(x => x.Name).ToList();

            lbxAttributes.Items.Clear();
            foreach(Dal.ProductCatalogAttributeGroup group in groups)
            {
                lbxAttributes.Items.AddRange(
                    attributes.Where(x => x.AttributeGroupId == group.AttributeGroupId)
                    .Select(x => new ListItem(String.Format("[{0}].({1})", group.Name, x.Name), x.AttributeId.ToString()))
                    .ToArray()
                    );
            }

            ListItem[] itemsSelected = lbxAttributes.Items.Cast<ListItem>().Where(x => attributesSelectedIds.Contains(Convert.ToInt32(x.Value))).ToArray();
            lbxAttributesSelected.Items.Clear();
            lbxAttributesSelected.Items.AddRange(itemsSelected);

            lblProductsCurrentCount.Text = String.Format("{0}", ph.GetProductCatalogs(PromotionId).Count());
            lblProductsCount.Text = String.Format("{0}", ph.GetProductCatalogsSelected(PromotionId).Where(x=>x.IsActive).Count());
        }

        private void SetPromotionConditions(List<PromotionCondition> conditions)
        {
            if (conditions.Count == 0)
                return;

            foreach(Dal.PromotionCondition condition in conditions)
            {
                switch(condition.ConditionTypeId)
                {
                    case 1:
                        chbPriceFrom.Checked = condition.IsActive;
                        txbPriceFrom.Text = String.Format("{0:0}", condition.DecimalValue);
                        break;
                    case 2:
                        chbPriceTo.Checked = condition.IsActive;
                        txbPriceTo.Text = String.Format("{0:0}", condition.DecimalValue);
                        break;
                    case 3:
                        chbPromotion.Checked = condition.IsActive; 
                        switch(condition.BitValue)
                        {
                            case null: ddlPromotion.SelectedIndex = 0; break;
                            case false: ddlPromotion.SelectedIndex = 1; break;
                            case true: ddlPromotion.SelectedIndex = 2; break;
                        };
                        break;
                }
            }
        }
        protected void btnProductAdd_Click(object sender, EventArgs e)
        {
            Dal.PromotionHelper oh = new Dal.PromotionHelper();
            List<string> message = new List<string>();


            string productName = Request.Form[txbProductCode.UniqueID];
            string sproductCatalogId = Request.Form[hfProductCatalogId.UniqueID];
            int productCatalogId = 0;

            if (!Int32.TryParse(sproductCatalogId, out productCatalogId))
            {
                DisplayMessage("Produkt nie istnieje");
                return;
            }

            oh.SetPromotionProduct(PromotionId, productCatalogId, UserName);

             
            BindProducts();
            txbProductCode.Text = "";
        }

        private void BindProducts()
        {

            Dal.PromotionHelper ph = new Dal.PromotionHelper();

            gvPromotionProducts.DataSource = ph.GetPromotionProducts(PromotionId);
            gvPromotionProducts.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Dal.Promotion promotion = new Dal.Promotion()
                {
                    PromotionId = PromotionId,
                    EndDate = calDateTo.SelectedDate,
                    IsActive = chbIsActive.Checked,
                    IsWatekmarkActive = false,// chbIsWatekmarkActive.Checked,
                    Name = txbName.Text.Trim(),
                    StartDate = calDateFrom.SelectedDate.Value,
                    Description = txbDescription.Text.Trim()
                };

                int[] suppliers = lbxSuppliersSelected.Items.Cast<ListItem>().Select(x => Convert.ToInt32(x.Value)).ToArray();
                int[] shops = lbxShopsSelected.Items.Cast<ListItem>().Select(x => Convert.ToInt32(x.Value)).ToArray();
                int[] attributes = lbxAttributesSelected.Items.Cast<ListItem>().Select(x => Convert.ToInt32(x.Value)).ToArray();

                List<Dal.PromotionCondition> conditions = new List<Dal.PromotionCondition>();

                Dal.PromotionCondition conPriceFrom = new Dal.PromotionCondition()
                {
                    BitValue = null,
                    ConditionTypeId = (int)Dal.Helper.PromotionConditionType.PriceFrom,
                    DecimalValue = Convert.ToDecimal(txbPriceFrom.Text.Trim()),
                    IsActive = chbPriceFrom.Checked,
                    Name = "Cena od",
                    PromotionId = PromotionId
                };
                Dal.PromotionCondition conPriceTo = new Dal.PromotionCondition()
                {
                    BitValue = null,
                    ConditionTypeId = (int)Dal.Helper.PromotionConditionType.PriceTo,
                    DecimalValue = Convert.ToDecimal(txbPriceTo.Text.Trim()),
                    IsActive = chbPriceTo.Checked,
                    Name = "Cena do",
                    PromotionId = PromotionId
                };
                Dal.PromotionCondition conPromo = new Dal.PromotionCondition()
                {
                    BitValue = null,
                    ConditionTypeId = (int)Dal.Helper.PromotionConditionType.Promotion,
                    DecimalValue = null,
                    IsActive = chbPromotion.Checked,
                    Name = "Cena do",
                    PromotionId = PromotionId
                };
                if (ddlPromotion.SelectedValue == "0") conPromo.BitValue = false;
                if (ddlPromotion.SelectedValue == "1") conPromo.BitValue = true;


                conditions.Add(conPriceFrom);
                conditions.Add(conPriceTo);
                conditions.Add(conPromo);

                Dal.PromotionHelper ph = new Dal.PromotionHelper();

                ph.SetPromotion(promotion, suppliers, shops, attributes, conditions);

                DisplayMessage("Zmiany zostały zapisane");
                BindPromotion();
            }
            catch (Exception ex)
            {
                DisplayMessage(String.Format("Błąd zapisu: {0}", ex.Message));

            }
        }

        private void MoveItems(ListBox listFrom, ListBox listTo)
        {
            ListItem[] items = listFrom.Items.Cast<ListItem>().Where(x => x.Selected).ToArray();
            int[] sel = listFrom.GetSelectedIndices();
            foreach (int si in sel.OrderByDescending(x => x).ToArray())
                listFrom.Items.RemoveAt(si);

            listTo.Items.AddRange(items);
            ListItem[] i = listTo.Items.Cast<ListItem>().OrderBy(x => x.Text).ToArray();

            listTo.Items.Clear();

            listTo.Items.AddRange(i);
        }
        protected void btnSuppliersAdd_Click(object s, EventArgs e)
        {
            MoveItems(lbxSuppliers, lbxSuppliersSelected);
        }

        protected void btnSuppliersDel_Click(object sender, EventArgs e)
        {
            MoveItems(lbxSuppliersSelected, lbxSuppliers);
        }

        protected void btnShopsDel_Click(object sender, EventArgs e)
        {
            MoveItems(lbxShopsSelected, lbxShops);
        }

        protected void btnShopsAdd_Click(object sender, EventArgs e)
        {
            MoveItems(lbxShops, lbxShopsSelected);
        }

        protected void btnAttributeDel_Click(object sender, EventArgs e)
        {
            MoveItems(lbxAttributesSelected, lbxAttributes);

        }

        protected void btnAttributeAdd_Click(object sender, EventArgs e)
        {
            MoveItems(lbxAttributes, lbxAttributesSelected);
        }

        protected void chbPromotion_CheckedChanged(object sender, EventArgs e)
        {
            SetControls();
        }

        private void SetControls()
        {
            if (chbPromotion.Checked)
            { 
                ddlPromotion.Enabled = true;
            }
            else
            { 
                ddlPromotion.Enabled = false;
            }

            txbPriceFrom.Enabled = chbPriceFrom.Checked;
            txbPriceTo.Enabled = chbPriceTo.Checked;
        }

        protected void btnImgAdd_Click(object sender, EventArgs e)
        {
 
            HttpFileCollection uploadedFiles = Request.Files;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbe = new StringBuilder();

            string uploadDir = @"/Files/Promotions";
            if (!Directory.Exists(Server.MapPath(uploadDir)))
                Directory.CreateDirectory(Server.MapPath(uploadDir));


            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];

                try
                {
                    if (userPostedFile.ContentLength > 0)
                    {
                        string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(userPostedFile.FileName));
                        string orginalFileName = System.IO.Path.GetFileName(userPostedFile.FileName);

                        string upl = String.Format(@"/Files/Promotions/{0}", PromotionId);
                        if (!Directory.Exists(Server.MapPath(upl)))
                            Directory.CreateDirectory(Server.MapPath(upl));


                        string saveLocation = String.Format(@"{0}\{1}", Server.MapPath(upl), "watermark.png");

                        if (saveLocation != null)
                            userPostedFile.SaveAs(saveLocation);


                        sb.AppendLine(String.Format("{0}. plik: {1}<br>", i + 1, userPostedFile.FileName));


                    }
                }
                catch (Exception Ex)
                {
                    sbe.AppendLine(String.Format("{0}. plik: {1}<br>", i + 1, userPostedFile.FileName));
                }
            }
            if (sbe.Length == 0)
                DisplayMessage(String.Format("Zapisano poprawnie<br><br>{0}", sb.ToString()));
            else
                DisplayMessage(String.Format("Błędy<br><br>{0}", sbe.ToString()));

            BindFiles();
        }

        private void BindFiles()
        {
            //string file = String.Format(@"/Files/Promotions/{0}/watermark.png", PromotionId);
            //hlWatermark.Visible = File.Exists(Server.MapPath(file));
            //hlWatermark.NavigateUrl = file;
            //imgWatermark.ImageUrl = file;
        }

        protected void ibtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32((sender as ImageButton).CommandArgument);
            Dal.PromotionHelper oh = new Dal.PromotionHelper();
            oh.SetPromotionProductDelete(id);
            BindProducts();
        }
        protected void gvPromotionProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblProductCatalogSupplier = e.Row.FindControl("lblProductCatalogSupplier") as Label;
                Label lblProductCatalogName = e.Row.FindControl("lblProductCatalogName") as Label;
                Label lblProductCatalogCode = e.Row.FindControl("lblProductCatalogCode") as Label;
                Label lblProductCatalogPrice = e.Row.FindControl("lblProductCatalogPrice") as Label;
                TextBox txbOfferQuantity = e.Row.FindControl("txbOfferQuantity") as TextBox;
                ImageButton ibtnDelete = e.Row.FindControl("ibtnDelete") as ImageButton;
                HyperLink hlImage = e.Row.FindControl("hlImage") as HyperLink;
                System.Web.UI.WebControls.Image imgImage = e.Row.FindControl("imgImage") as System.Web.UI.WebControls.Image;
                
                Dal.PromotionProductView offerProduct = e.Row.DataItem as Dal.PromotionProductView;
                 
                hlImage.NavigateUrl = String.Format(hlImage.NavigateUrl, offerProduct.ProductCatalogId);
           
                ibtnDelete.CommandArgument = offerProduct.Id.ToString();
                lblProductCatalogName.Text = offerProduct.ProductName;
                lblProductCatalogCode.Text = offerProduct.Code;
                lblProductCatalogSupplier.Text = offerProduct.SupplierName;

                lblProductCatalogPrice.Text = String.Format("{0:C}", offerProduct.PriceBruttoFixed);


                 txbOfferQuantity.Text = String.Format("{0}", offerProduct.Quantity);
                 
                if (offerProduct.ImageFullName != null)
                    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", offerProduct.ImageFullName.Replace(".", "_m."));
                else
                    imgImage.Visible = false;

                
            }
             
        }

        protected void btnPromotionProductsSelect_Click(object sender, EventArgs e)
        {

            Dal.PromotionHelper ph = new Dal.PromotionHelper();
            ph.SetProductCatalogs(PromotionId);

            BindPromotion();
            DisplayMessage("Nowa lista została utworzona");

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string upl = String.Format(@"/Files/Promotions/{0}", PromotionId);

            using (System.Drawing.Image backImg = System.Drawing.Image.FromFile(@"C:\Users\jacek\source\repos\AllegroApp\LajtIt.Web\Images\ProductCatalog\c906f8fe-5cd4-4a69-a077-15f8a41978aa.jpg"))
            {

                using (System.Drawing.Image mrkImg = System.Drawing.Image.FromFile(String.Format(@"{0}\{1}", Server.MapPath(upl), "watermark.png")))
                {
                    using (Graphics g = Graphics.FromImage(backImg))
                    {
                        //mrkImg.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        float widthOfBgr = 0.25F;
                        int widthOgWatemark = (int)(backImg.Width * widthOfBgr);
                        double rate = widthOgWatemark * 1.0 / mrkImg.Width;
                        int heightOfWatermark = (int)(rate * 1.0 * mrkImg.Height);

                        g.DrawImage(mrkImg, 0, 0, widthOgWatemark, heightOfWatermark);
                        backImg.Save(@"C:\Users\jacek\source\repos\AllegroApp\LajtIt.Web\Images\ProductCatalog\result.jpg");
                    }
                }
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            Bll.ShopHelper sh = new Bll.ShopHelper();

//            sh.CreateGroup(String.Format("Promocja {0}", PromotionId));

            sh.CreateOption(48, "testttt");//
        }
    }
}