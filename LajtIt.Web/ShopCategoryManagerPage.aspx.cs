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
    [Developer("53ab2131-0850-41f6-a173-86467b3d8c00")]
    public partial class ShopCategoryManagerPage : LajtitPage
    {
        private int ShopCategoryManagerId { get { return Convert.ToInt32(Request.QueryString["id"].ToString()); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShopCategoryManagerPage();
                BindCountries();
                SetControls();

                //BindProducts();
                //hlProductCatalog.NavigateUrl = String.Format(hlProductCatalog.NavigateUrl, ShopCategoryManagerPageId);
            }
        }

        private void BindCountries()
        {
            ddlCountry.DataSource = Dal.DbHelper.ProductCatalog.GetCountries();
            ddlCountry.DataBind();
        }

        private void BindShopCategoryManagerPage()
        {
            Dal.ShopHelper ph = new Dal.ShopHelper();

            Dal.ShopCategoryManager scm = ph.GetShopCategoryManager(ShopCategoryManagerId);
            if(!Page.IsPostBack)
            {

                Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), scm.ShopId);
                ucShopCategoryControl.SetCategoryId(shop, 0);
            }
            lblShop.Text = scm.Shop.Name;

            List<Dal.ShopCategoryManagerCondition> conditions = ph.GetShopCategoryManagerConditions(ShopCategoryManagerId);

            chbIsActive.Checked = scm.IsActive;
            txbName.Text = scm.Name;
            //txbDescription.Text = promotion.Description;

            if (scm.MainCategoryPriority.HasValue)
                txbMainCategoryPriority.Text = scm.MainCategoryPriority.ToString();

            List<Dal.ShopCategoryManagerShopView> shopsSelected = ph.GetShopCategoryManagerShops(ShopCategoryManagerId);
            //int[] shopCategoryIds = shopsSelected.Select(x => x.CategoryId).ToArray();
            List<Dal.ProductCatalogAttribute> attributesSelected = ph.GetShopCategoryManagerAttributes(ShopCategoryManagerId);
            int[] attributesSelectedIds = attributesSelected.Select(x => x.AttributeId).ToArray();
            List<Dal.Supplier> suppliersSelected = ph.GetShopCategoryManagerSuppliers(ShopCategoryManagerId);
            int[] suppliersSelectedIds = suppliersSelected.Select(x => x.SupplierId).ToArray();



            lbxShopsSelected.Items.Clear();
           //lbxShops.Items.Clear();
            lbxShopsSelected.DataSource = shopsSelected;
            lbxShopsSelected.DataBind();

            //lbxShops.DataSource = shops.Where(x => !shopCategoryIds.Contains(x.ShopId)).ToList();
            //lbxShops.DataBind();

            SetShopCategoryManagerPageConditions(conditions);

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

            List<Dal.Supplier> suppliers = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderBy(x=>x.Name).ToList();

            lbxSuppliers.Items.Clear();
   
                lbxSuppliers.Items.AddRange(
                    suppliers.Where(x => !suppliersSelectedIds.Contains(x.SupplierId))
                    .Select(x => new ListItem(String.Format("{0}",  x.Name), x.SupplierId.ToString()))
                    .ToArray()
                    );
       
             
            lbxSuppliersSelected.Items.Clear();
            lbxSuppliersSelected.Items.AddRange(
                suppliers.Where(x => suppliersSelectedIds.Contains(x.SupplierId))
                .Select(x => new ListItem(String.Format("{0}", x.Name), x.SupplierId.ToString()))
                .ToArray()
                );

        }

        private void SetShopCategoryManagerPageConditions(List<ShopCategoryManagerCondition> conditions)
        {
            if (conditions.Count == 0)
                return;

            foreach(Dal.ShopCategoryManagerCondition condition in conditions)
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
                        chbConditonPromotion.Checked = condition.IsActive; 
                        switch(condition.BitValue)
                        {
                            case null: ddlConditonPromotion.SelectedIndex = 0; break;
                            case false: ddlConditonPromotion.SelectedIndex = 1; break;
                            case true: ddlConditonPromotion.SelectedIndex = 2; break;
                        };
                        break;
                    case 4:
                        chbConditonPromotionRate.Checked = condition.IsActive;
                        txbPromotionRate.Text = String.Format("{0:0}", condition.DecimalValue);
                        break;
                    case 5:
                        chbAvailability.Checked = condition.IsActive;
                        switch (condition.BitValue)
                        {
                            case null: ddlAvailability.SelectedIndex = 0; break;
                            case false: ddlAvailability.SelectedIndex = 1; break;
                            case true: ddlAvailability.SelectedIndex = 2; break;
                        };
                        break;
                    case 6:
                        chbOutlet.Checked = condition.IsActive;
                        switch (condition.BitValue)
                        {
                            case null: ddlOutlet.SelectedIndex = 0; break;
                            case false: ddlOutlet.SelectedIndex = 1; break;
                            case true: ddlOutlet.SelectedIndex = 2; break;
                        };
                        break;
                    case 7:
                        chbCountry.Checked = condition.IsActive;

                        if (condition.StringValue !=null)
                            ddlCountry.SelectedValue = condition.StringValue;
                        break;
                }
            }
        }
        //protected void btnProductAdd_Click(object sender, EventArgs e)
        //{
        //    Dal.ShopHelper oh = new Dal.ShopHelper();
        //    List<string> message = new List<string>();


        //    string productName = Request.Form[txbProductCode.UniqueID];
        //    string sproductCatalogId = Request.Form[hfProductCatalogId.UniqueID];
        //    int productCatalogId = 0;

        //    if (!Int32.TryParse(sproductCatalogId, out productCatalogId))
        //    {
        //        DisplayMessage("Produkt nie istnieje");
        //        return;
        //    }

        //    oh.SetShopCategoryManagerPageProduct(ShopCategoryManagerPageId, productCatalogId, UserName);

             
        //    BindProducts();
        //    txbProductCode.Text = "";
        //}

        //private void BindProducts()
        //{

        //    Dal.ShopCategoryManagerPageHelper ph = new Dal.ShopCategoryManagerPageHelper();

        //    gvShopCategoryManagerPageProducts.DataSource = ph.GetShopCategoryManagerPageProducts(ShopCategoryManagerPageId);
        //    gvShopCategoryManagerPageProducts.DataBind();
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Dal.ShopCategoryManager scm = new Dal.ShopCategoryManager()
                {
                    ShopCategoryManagerId = ShopCategoryManagerId,
                    IsActive = chbIsActive.Checked,
                    Name = txbName.Text.Trim(),
                    Description = null// txbDescription.Text.Trim()
                };

                if (!String.IsNullOrEmpty(txbMainCategoryPriority.Text))
                    scm.MainCategoryPriority = Int32.Parse(txbMainCategoryPriority.Text);


                int[] categoryIds = lbxShopsSelected.Items.Cast<ListItem>().Select(x => Convert.ToInt32(x.Value)).ToArray();
                int[] attributes = lbxAttributesSelected.Items.Cast<ListItem>().Select(x => Convert.ToInt32(x.Value)).ToArray();
                int[] suppliers = lbxSuppliersSelected.Items.Cast<ListItem>().Select(x => Convert.ToInt32(x.Value)).ToArray();

                List<Dal.ShopCategoryManagerCondition> conditions = new List<Dal.ShopCategoryManagerCondition>();

                Dal.ShopCategoryManagerCondition conPriceFrom = new Dal.ShopCategoryManagerCondition()
                {
                    BitValue = null,
                    ConditionTypeId = (int)Dal.Helper.PromotionConditionType.PriceFrom,
                    DecimalValue = GetValue(txbPriceFrom),
                    IsActive = chbPriceFrom.Checked,
                    Name = "Cena od",
                    ShopCategoryManagerId = ShopCategoryManagerId
                };
                Dal.ShopCategoryManagerCondition conPriceTo = new Dal.ShopCategoryManagerCondition()
                {
                    BitValue = null,
                    ConditionTypeId = (int)Dal.Helper.PromotionConditionType.PriceTo,
                    DecimalValue = GetValue(txbPriceTo),
                    IsActive = chbPriceTo.Checked,
                    Name = "Cena do",
                    ShopCategoryManagerId = ShopCategoryManagerId
                };
                Dal.ShopCategoryManagerCondition conPromo = new Dal.ShopCategoryManagerCondition()
                {
                    BitValue = null,
                    ConditionTypeId = (int)Dal.Helper.PromotionConditionType.Promotion,
                    DecimalValue = null,
                    IsActive = chbConditonPromotion.Checked && ddlConditonPromotion.SelectedIndex>0,
                    Name = "Promocja",
                    ShopCategoryManagerId = ShopCategoryManagerId
                };
                Dal.ShopCategoryManagerCondition conPromoRate = new Dal.ShopCategoryManagerCondition()
                {
                    BitValue = null,
                    ConditionTypeId = (int)Dal.Helper.PromotionConditionType.PromotionRate,
                    DecimalValue = GetValue(txbPromotionRate),
                    IsActive = chbConditonPromotionRate.Checked ,
                    Name = "Promocja x%",
                    ShopCategoryManagerId = ShopCategoryManagerId
                };
                Dal.ShopCategoryManagerCondition conAvail = new Dal.ShopCategoryManagerCondition()
                {
                    BitValue = null,
                    ConditionTypeId = (int)Dal.Helper.PromotionConditionType.Availability,
                    DecimalValue = null,
                    IsActive = chbAvailability.Checked && ddlAvailability.SelectedIndex>0,
                    Name = "Dostępność",
                    ShopCategoryManagerId = ShopCategoryManagerId
                };
                Dal.ShopCategoryManagerCondition conOutlet = new Dal.ShopCategoryManagerCondition()
                {
                    BitValue = null,
                    ConditionTypeId = (int)Dal.Helper.PromotionConditionType.Outlet,
                    DecimalValue = null,
                    IsActive = chbOutlet.Checked && ddlOutlet.SelectedIndex>0,
                    Name = "Outlet",
                    ShopCategoryManagerId = ShopCategoryManagerId
                };
                Dal.ShopCategoryManagerCondition conCountry = new Dal.ShopCategoryManagerCondition()
                {
                    BitValue = null,
                    ConditionTypeId = (int)Dal.Helper.PromotionConditionType.SupplierCountry,
                    StringValue = ddlCountry.SelectedValue,
                    IsActive = chbCountry.Checked && ddlCountry.SelectedIndex > 0,
                    Name = "Kraj dostawcy",
                    ShopCategoryManagerId = ShopCategoryManagerId
                };
                if (ddlConditonPromotion.SelectedValue == "0") conPromo.BitValue = false;
                if (ddlConditonPromotion.SelectedValue == "1") conPromo.BitValue = true;
                if (ddlAvailability.SelectedValue == "0") conAvail.BitValue = false;
                if (ddlAvailability.SelectedValue == "1") conAvail.BitValue = true;
                if (ddlOutlet.SelectedValue == "0") conOutlet.BitValue = false;
                if (ddlOutlet.SelectedValue == "1") conOutlet.BitValue = true;
                if (ddlCountry.SelectedValue == "0") conCountry.IsActive = false;
                //if (ddlCountry.SelectedValue == "1") conCountry.BitValue = true;


                conditions.Add(conPriceFrom);
                conditions.Add(conPriceTo);
                conditions.Add(conPromo);
                conditions.Add(conPromoRate);
                conditions.Add(conAvail);
                conditions.Add(conOutlet);
                conditions.Add(conCountry);

                if (conditions.Where(x=>x.IsActive).Count() == 0 && attributes.Length == 0 && scm.IsActive)
                {
                    scm.IsActive = false;
                    DisplayMessage("Zmiany zostały zapisane lecz konfiguracja wyłączona ponieważ nie określono żadnego z warunków.<br>Dodaj warunki i następnie włącz konfigurację.");
                }
                else
                    DisplayMessage("Zmiany zostały zapisane");


                Dal.ShopHelper ph = new Dal.ShopHelper();

                ph.SetShopCategoryManager(scm, categoryIds, attributes, suppliers, conditions);

                BindShopCategoryManagerPage();
            }
            catch (Exception ex)
            {
                DisplayMessage(String.Format("Błąd zapisu: {0}", ex.Message));

            }
        }

        private int GetValue(TextBox txb)
        {
            if (String.IsNullOrEmpty(txb.Text))
                return 0;
            else
                return Int32.Parse(txb.Text);
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
      

        protected void btnShopsDel_Click(object sender, EventArgs e)
        {
            //  MoveItems(lbxShopsSelected, lbxShops);
            ListItem[] items = lbxShopsSelected.Items.Cast<ListItem>().Where(x => x.Selected).ToArray();
            int[] sel = lbxShopsSelected.GetSelectedIndices();
            foreach (int si in sel.OrderByDescending(x => x).ToArray())
                lbxShopsSelected.Items.RemoveAt(si);
        }

        protected void btnShopsAdd_Click(object sender, EventArgs e)
        {

            List<int> shopCategoryIds = lbxShopsSelected.Items.Cast<ListItem>().Select(x => Convert.ToInt32(x.Value)).ToList();
            string c = ucShopCategoryControl.GetShopCategoryId();
            if (c == null)
                return;


            string shopCategoryId = ucShopCategoryControl.GetShopCategoryId();
            

            Dal.ShopHelper sh = new ShopHelper();

            Dal.ShopCategoryManager scm = sh.GetShopCategoryManager(ShopCategoryManagerId);

            Dal.Helper.ShopType shopType = (Dal.Helper.ShopType)Enum.ToObject(typeof(Dal.Helper.ShopType), scm.Shop.ShopTypeId);


            Dal.ShopCategory sc = sh.GetShopCategory(shopType, shopCategoryId);

            Dal.ShopCategoryView scv = sh.GetShopCategoryView(sc.CategoryId);


            if(lbxShopsSelected.Items.FindByValue(sc.CategoryId.ToString())!=null)
            {
                DisplayMessage("Kategoria została już dodana");
                return;
            }

            lbxShopsSelected.Items.Add(new ListItem() { Text = scv.CategoryPath, Value = scv.CategoryId.ToString() });

            //lbxShopsSelected.Items.Clear();
            //lbxShopsSelected.DataSource = shopsSelected;
            //lbxShopsSelected.DataBind();

        }

        protected void btnAttributeDel_Click(object sender, EventArgs e)
        {
            MoveItems(lbxAttributesSelected, lbxAttributes);

        }

        protected void btnAttributeAdd_Click(object sender, EventArgs e)
        {
            MoveItems(lbxAttributes, lbxAttributesSelected);
        }
        protected void btnSupplierDel_Click(object sender, EventArgs e)
        {
            MoveItems(lbxSuppliersSelected, lbxSuppliers);

        }

        protected void btnSupplierAdd_Click(object sender, EventArgs e)
        {
            MoveItems(lbxSuppliers, lbxSuppliersSelected);
        }
        protected void chbShopCategoryManagerPage_CheckedChanged(object sender, EventArgs e)
        {
            SetControls();
        }

        private void SetControls()
        {
            ddlConditonPromotion.Enabled = chbConditonPromotion.Checked;
            ddlAvailability.Enabled = chbAvailability.Checked;
            ddlOutlet.Enabled = chbOutlet.Checked;
            txbPriceFrom.Enabled = chbPriceFrom.Checked;
            txbPriceTo.Enabled = chbPriceTo.Checked;
            txbPromotionRate.Enabled = chbConditonPromotionRate .Checked;
            ddlCountry.Enabled = chbCountry.Checked;

        }

        

       

        //protected void ibtnDelete_Click(object sender, ImageClickEventArgs e)
        //{
        //    int id = Convert.ToInt32((sender as ImageButton).CommandArgument);
        //    Dal.ShopCategoryManagerPageHelper oh = new Dal.ShopCategoryManagerPageHelper();
        //    oh.SetShopCategoryManagerPageProductDelete(id);
        //    BindProducts();
        //}
       
    }
}