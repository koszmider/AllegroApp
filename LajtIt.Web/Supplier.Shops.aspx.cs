using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("45d33ef6-6fc7-48a8-a1bd-c07e7202d786")]
    public partial class SupplierShop : LajtitPage
    {
        private int SupplierId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindForm();
        }

        protected void chbOnlyActive_CheckedChanged(object sender, EventArgs e)
        {
            BindForm();
        }
        private void BindForm()
        { 

            List<Dal.SupplierShop> shops = Dal.DbHelper.ProductCatalog.GetSupplierShops(SupplierId).Where(x => x.Shop.IsActive && x.Shop.CanExportProducts).ToList() ;
            switch (rblOnlyActive.SelectedIndex)
            {
                case 0:
                    //suppliers = suppliers.Where(x => x.IsActive).ToList(); 
                    break;
                case 1:
                    shops = shops.Where(x => x.IsActive).ToList(); break;
                case 2:
                    shops = shops.Where(x => !x.IsActive).ToList(); break;
            }


            gvShop.DataSource = shops;
            gvShop.DataBind();

        }

        protected void gvShop_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.SupplierShop ss = e.Row.DataItem as Dal.SupplierShop;

                CheckBox chbIsActive = e.Row.FindControl("chbIsActive") as CheckBox;
                CheckBox chbShowSupplierNameIdDescription = e.Row.FindControl("chbShowSupplierNameIdDescription") as CheckBox;
                CheckBox chbLockRebates = e.Row.FindControl("chbLockRebates") as CheckBox;
                TextBox txbMaxNumberOfProductsInOffer = e.Row.FindControl("txbMaxNumberOfProductsInOffer") as TextBox;
              
                TextBox txbTemplate = e.Row.FindControl("txbTemplate") as TextBox;
                TextBox txbSellDiscount = e.Row.FindControl("txbSellDiscount") as TextBox;
                TextBox txbSellDiscountValue = e.Row.FindControl("txbSellDiscountValue") as TextBox;
                TextBox txbSellCommision = e.Row.FindControl("txbSellCommision") as TextBox;
                TextBox txbMinPrice = e.Row.FindControl("txbMinPrice") as TextBox;
                TextBox txbMaxPrice = e.Row.FindControl("txbMaxPrice") as TextBox;
                Label lblShopMinPrice = e.Row.FindControl("lblShopMinPrice") as Label;
                Label lblShopMaxPrice = e.Row.FindControl("lblShopMaxPrice") as Label;
                Button btnCategory = e.Row.FindControl("btnCategory") as Button;
              //  Button btnProducer = e.Row.FindControl("btnProducer") as Button;

                //LajtIt.Web.Controls.ShopCategoryControlJson uc = e.Row.FindControl("ucShopCategoryControlJson") as LajtIt.Web.Controls.ShopCategoryControlJson;

                if (ss.Shop.HasDefaultCategory)
                    ucShopCategoryControlJson.BindForm();
                //if (ss.Shop.HasProducers)
                //    ucShopProducer.BindForm();

                if (ss.CategoryId.HasValue)
                    btnCategory.Text = ss.CategoryId.ToString();

                btnCategory.Visible = ss.Shop.HasDefaultCategory;
                // btnProducer.Visible = ss.Shop.HasProducers;

                if (ss.MinPrice.HasValue)
                    txbMinPrice.Text = String.Format("{0}", ss.MinPrice);
                if (ss.MaxPrice.HasValue)
                    txbMaxPrice.Text = String.Format("{0}", ss.MaxPrice);
                if (ss.Shop.MinPrice.HasValue)
                    lblShopMinPrice.Text = String.Format("{0}", ss.Shop.MinPrice);
                if (ss.Shop.MaxPrice.HasValue)
                    lblShopMaxPrice.Text = String.Format("{0}", ss.Shop.MaxPrice);


                chbIsActive.Checked = ss.IsActive;
                chbShowSupplierNameIdDescription.Checked = ss.ShowSupplierNameIdDescription;
                chbLockRebates.Checked = ss.LockRebates;
                if (ss.MaxNumberOfProductsInOffer.HasValue)
                    txbMaxNumberOfProductsInOffer.Text = ss.MaxNumberOfProductsInOffer.ToString();
                //if (ss.ShopProducer != null)
                //{
                //    btnProducer.Text = ss.ShopProducer.ShopProducerId.ToString();
                //    btnProducer.CommandArgument = String.Format("{0}|{1}|{2}", ss.ShopId, ss.ShopProducer.ShopProducerId, ss.Id);
                //}
                //else
                //    btnProducer.CommandArgument = String.Format("{0}|{1}|{2}", ss.ShopId, "", ss.Id);

                txbTemplate.Text = ss.Template;
                txbSellDiscount.Text = String.Format("{0:#.##}", ss.SellDiscount*100.00M);
                txbSellDiscountValue.Text = String.Format("{0:#.##}", ss.SellDiscountValue);
                txbSellCommision.Text = String.Format("{0:#.##}", ss.SellCommision*100.00M);


                btnCategory.CommandArgument = String.Format("{0}|{1}|{2}", ss.ShopId, ss.SupplierId, ss.Id);

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<Dal.SupplierShop> shops = new List<Dal.SupplierShop>();

            foreach(GridViewRow row in gvShop.Rows)
            {
                CheckBox chbIsActive = row.FindControl("chbIsActive") as CheckBox;
                CheckBox chbShowSupplierNameIdDescription = row.FindControl("chbShowSupplierNameIdDescription") as CheckBox;
                CheckBox chbLockRebates = row.FindControl("chbLockRebates") as CheckBox;
                TextBox txbMaxNumberOfProductsInOffer = row.FindControl("txbMaxNumberOfProductsInOffer") as TextBox; 
                TextBox txbTemplate = row.FindControl("txbTemplate") as TextBox;
                TextBox txbSellDiscount = row.FindControl("txbSellDiscount") as TextBox;
                TextBox txbSellDiscountValue = row.FindControl("txbSellDiscountValue") as TextBox;
                TextBox txbSellCommision = row.FindControl("txbSellCommision") as TextBox;
                TextBox txbMinPrice = row.FindControl("txbMinPrice") as TextBox;
                TextBox txbMaxPrice = row.FindControl("txbMaxPrice") as TextBox;
                Button btnCategory = row.FindControl("btnCategory") as Button;
               // Button btnProducer = row.FindControl("btnProducer") as Button;

                int id = Convert.ToInt32(gvShop.DataKeys[row.RowIndex][0]);

                Dal.SupplierShop ss = new Dal.SupplierShop()
                {
                    CategoryId = null,
                    ExportFileEnabled = null,
                    Id = id,
                    IsActive = chbIsActive.Checked,
                    LockRebates = chbLockRebates.Checked,
                    ShowSupplierNameIdDescription = chbShowSupplierNameIdDescription.Checked,
                    Template = txbTemplate.Text.Trim()
                };

                if (txbMaxNumberOfProductsInOffer.Text != "")
                    ss.MaxNumberOfProductsInOffer = Int32.Parse(txbMaxNumberOfProductsInOffer.Text);

                if (txbMinPrice.Text != "")
                    ss.MinPrice = decimal.Parse(txbMinPrice.Text);
                if (txbMaxPrice.Text != "")
                    ss.MaxPrice = decimal.Parse(txbMaxPrice.Text);
                //if (btnProducer.Visible && btnProducer.Text != "")
                //ss.ProducerId = Int32.Parse(btnProducer.Text);
                if (txbSellCommision.Text != "")
                    ss.SellCommision = Decimal.Parse(txbSellCommision.Text) / 100.0M;
                else
                    ss.SellCommision = 0;
                if (txbSellDiscount.Text != "")
                    ss.SellDiscount = Decimal.Parse(txbSellDiscount.Text) / 100.0M;
                else
                    ss.SellDiscount = 0;
                if (txbSellDiscountValue.Text != "")
                    ss.SellDiscountValue = Decimal.Parse(txbSellDiscountValue.Text);
                else
                    ss.SellDiscountValue = 0;
                if (btnCategory.Visible && btnCategory.Text != "")
                    ss.CategoryId = Int32.Parse(btnCategory.Text);
               // if (btnProducer.Visible && btnProducer.Text != "")
                //    ss.ProducerId = Int32.Parse(btnProducer.Text);

                shops.Add(ss);
            }

            Dal.ShopHelper sh = new Dal.ShopHelper();
            sh.SetSupplierShop(shops);

            DisplayMessage("Zapisano");
        }

        protected void btnCategory_Click(object sender, EventArgs e)
        {
            string cmd = ((Button)sender).CommandArgument;
            ViewState["ShopSupplier"] = cmd;

            int shopId = Int32.Parse(cmd.Split(new char[] { '|' })[0]);
            int supplierId = Int32.Parse(cmd.Split(new char[] { '|' })[1]);

            Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopId);

            ucShopCategoryControlJson.SetCategoryId(shop, supplierId);

            mpeCategory.Show();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            mpeCategory.Hide();

            int? categoryId = ucShopCategoryControlJson.GetCategoryId();

            if (categoryId.HasValue)
            {
                int id = Int32.Parse(ViewState["ShopSupplier"].ToString().Split(new char[] { '|' })[2]);

                int rowIndex = -1;
                foreach (GridViewRow gvRow in gvShop.Rows)
                {
                    var dataKey = gvShop.DataKeys[gvRow.DataItemIndex];
                    if (dataKey == null || (int)dataKey.Value != id) continue;
                    rowIndex = gvRow.DataItemIndex;
                    Button btnCategory = gvRow.FindControl("btnCategory") as Button;


                    btnCategory.Text = categoryId.ToString();

                    break;
                }
            }


            ViewState["ShopSupplier"] = null;

        }

        //protected void btnProducer_Click(object sender, EventArgs e)
        //{
        //    mpeCategory.Hide();
        //    int id = Int32.Parse(ViewState["ShopProducer"].ToString().Split(new char[] { '|' })[2]);

        //    int shopId = Int32.Parse(ViewState["ShopProducer"].ToString().Split(new char[] { '|' })[0]);
        //    ucShopProducer.SetShopId(shopId);

        //    int? producerId = ucShopProducer.GetProducerId();

        //    if (producerId.HasValue)
        //    {

        //        int rowIndex = -1;

        //        foreach (GridViewRow gvRow in gvShop.Rows)
        //        {
        //            var dataKey = gvShop.DataKeys[gvRow.DataItemIndex];
        //            if (dataKey == null || (int)dataKey.Value != id) continue;
        //            rowIndex = gvRow.DataItemIndex;
        //            Button btnProducer = gvRow.FindControl("btnProducer") as Button;


        //            btnProducer.Text = producerId.ToString();

        //            break;
        //        }
        //    }


        //    ViewState["ShopProducer"] = null;
        //}

        //protected void btnProducer_Click1(object sender, EventArgs e)
        //{

        //    string cmd = ((Button)sender).CommandArgument;
        //    ViewState["ShopProducer"] = cmd;

        //    int shopId = Int32.Parse(cmd.Split(new char[] { '|' })[0]);
        //    int? producerId = null;
        //    if (cmd.Split(new char[] { '|' })[1] != "")
        //        producerId = Int32.Parse(cmd.Split(new char[] { '|' })[1]);

        //    Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shopId);

        //    ucShopProducer.SetProducerId( SupplierId, shop, producerId);

        //    mpeProducers.Show();
        //}
    }
}