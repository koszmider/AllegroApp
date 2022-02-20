using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("03b92454-7ba6-45b9-b910-56eee0f63679")]
    public partial class SupplierDescriptions : LajtitPage
    {
        private int SupplierId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindForm();
        }


        private void BindForm()
        { 

            List<Dal.SupplierShop> shops = Dal.DbHelper.ProductCatalog.GetSupplierShops(SupplierId).Where(x => x.Shop.IsActive && x.Shop.CanExportProducts).ToList() ;

            List<ListItem> items = new List<ListItem>();

            foreach (Dal.SupplierShop ss in shops.OrderByDescending(x => x.IsDescriptionActive))
                items.Add(new ListItem(String.Format("{0} {1}", ss.IsDescriptionActive ? "[X]" : "__", ss.Shop.Name), ss.ShopId.ToString()));


            ddlShops.Items.AddRange(items.ToArray());
            BindDescription(); 
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string desc = null;

            if (!String.IsNullOrEmpty(txtLongDescription.Text.Trim()))
                desc = txtLongDescription.Text.Trim();

            Dal.SupplierShop ss = new Dal.SupplierShop()
            {
                ShopId = Int32.Parse(ddlShops.SelectedValue),
                SupplierId = SupplierId,
                LongDescription = desc,
                IsDescriptionActive = chbIsActive.Checked
            };

            Dal.DbHelper.ProductCatalog.SetSupplierShop(ss);

            DisplayMessage("Zmiany zapisane");
        }

        protected void btnShopShow_Click(object sender, EventArgs e)
        {
            BindDescription();

        }

        private void BindDescription()
        {
            Dal.SupplierShop ss = Dal.DbHelper.ProductCatalog.GetSupplierShops(SupplierId).Where(x => x.ShopId == Int32.Parse(ddlShops.SelectedValue)).FirstOrDefault();

            if (ss.Shop.ShopTypeId == (int)Dal.Helper.ShopType.Allegro
                || ss.Shop.ShopTypeId == (int)Dal.Helper.ShopType.Erli)
            {
                pnTags.Visible = true;
                htmlLong.Enabled = false;
            }
            else
            {
                pnTags.Visible = false;
                htmlLong.Enabled = true;
            }

            chbIsActive.Checked = ss.IsDescriptionActive;
            txtLongDescription.Text = ss.LongDescription;
        }
    }
}