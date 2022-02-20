using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("ef0ca67d-0f5d-4a2b-b43e-396f6602ef38")]
    public partial class ShopExportFile : LajtitPage
    {
        private int ShopId { get { return Int32.Parse(ddlShop.SelectedValue); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShops();
                BindShop();
            }
            }

        private void BindShops()
        { 
            ddlShop.DataSource = Dal.DbHelper.Shop.GetShops().Where(x => x.CanExportProducts && x.ExportFileFormatTypeId.HasValue).ToList();
            ddlShop.DataBind();

            Dal.ShopHelper sh = new Dal.ShopHelper();
            ddlExportFileFormatType.DataSource = sh.GetShopExportFileFormatTypes().OrderBy(x => x.Name).ToList() ;
            ddlExportFileFormatType.DataBind();


        }

        protected void ddlShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindShop();
        }

        private void BindShop()
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            Dal.Shop shop = Dal.DbHelper.Shop.GetShop(ShopId);

            if (shop.ExportFilePriceTypeId.HasValue)
                ddlPriceTypeId.SelectedValue = shop.ExportFilePriceTypeId.ToString();

            txbExportFileName.Text = shop.ExportFileName;
            if (String.IsNullOrEmpty(shop.ExportFileUrlParameters))
                txbUrlParameters.Text = "";
            else
                txbUrlParameters.Text = shop.ExportFileUrlParameters;

            chbFilterByLampType.Checked = shop.ExportFileFilterByProductType;
            



            if (shop.ExportFilePriceFrom.HasValue)
                txbPriceFrom.Text = String.Format("{0:0.00}", shop.ExportFilePriceFrom);
            else
                txbPriceFrom.Text = "";
            if (shop.ExportFilePriceTo.HasValue)
                txbPriceTo.Text = String.Format("{0:0.00}", shop.ExportFilePriceTo);
            else
                txbPriceTo.Text = "";

            if (shop.ExportFileEanRequired.HasValue)
                chbExportFileEanRequired.Checked = shop.ExportFileEanRequired.Value;

            if (shop.ExportFileFormatTypeId.HasValue)
                ddlExportFileFormatType.SelectedValue = shop.ExportFileFormatTypeId.ToString();

            //if (shop.ExportFileExportPriceTypeId == 2)
            rblPrice.SelectedValue = shop.ExportFileExportPriceTypeId.ToString() ;
 


            gvLampTypes.DataSource = sh.GetShopAttributes(ShopId).OrderBy(x => x.Name).ToList();
            gvLampTypes.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            decimal pFrom = 0;
            decimal pTo = 0;
            decimal? priceFrom = null;
            decimal? priceTo = null;

            if (Decimal.TryParse(txbPriceFrom.Text, out pFrom))
                priceFrom = pFrom;
            if (Decimal.TryParse(txbPriceTo.Text, out pTo))
                priceTo = pTo;

            Dal.Shop shop = new Dal.Shop()
            {
                ShopId = ShopId,
                ExportFileEanRequired = chbExportFileEanRequired.Checked,
                ExportFileFormatTypeId = Convert.ToInt32(ddlExportFileFormatType.SelectedValue),
                ExportFilePriceFrom = priceFrom,
                ExportFilePriceTo = priceTo,
                ExportFilePriceTypeId = Convert.ToInt32(ddlPriceTypeId.SelectedValue),
                ExportFileName = txbExportFileName.Text.Trim(),
                
                ExportFileFilterByProductType = chbFilterByLampType.Checked,
                ExportFileExportPriceTypeId = Int32.Parse(rblPrice.SelectedValue)
            };
            if (txbUrlParameters.Text.Trim() == "")
                shop.ExportFileUrlParameters = null;
            else
                shop.ExportFileUrlParameters = txbUrlParameters.Text.Trim();

            List<Dal.SupplierShop> ss = new List<Dal.SupplierShop>();
 
            List<Dal.ShopExportFileAttribute> se = new List<Dal.ShopExportFileAttribute>();

            foreach (GridViewRow row in gvLampTypes.Rows)
            {

                CheckBox chbExportEnabled = row.FindControl("chbExportEnabled") as CheckBox;

                if(chbExportEnabled.Checked)
                {
                    Dal.ShopExportFileAttribute a = new Dal.ShopExportFileAttribute()
                    {
                        ShopId = ShopId,
                        AttributeId = Int32.Parse(gvLampTypes.DataKeys[row.RowIndex][0].ToString())
                    };

                    se.Add(a);
                }
            }

            Dal.ShopHelper sh = new Dal.ShopHelper();
            sh.SetShopExportFile(shop, ss, se);

            DisplayMessage("Zapisano");
             
            Dal.Shop shopImport = Dal.DbHelper.Shop.GetShop(ShopId);
            Dal.Shop shopExport = Dal.DbHelper.Shop.GetShop((int)Dal.Helper.Shop.Lajtitpl);


            lbInfo.Text = String.Format("Liczba pasujących produktów: {0}", Bll.ShopExportFileHelper.GetProducts(shopExport, shopImport).Count());
        }

        protected void gvSuppliers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType== DataControlRowType.DataRow)
            {
                Dal.SupplierShopFnResult ss = e.Row.DataItem as Dal.SupplierShopFnResult;

                CheckBox chbExportEnabled = e.Row.FindControl("chbExportEnabled") as CheckBox;

                chbExportEnabled.Checked = ss.ExportFileEnabled.HasValue && ss.ExportFileEnabled.Value;
            }
        }

        protected void gvLampTypes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            { 
                Dal.ShopAttributeFnResult ss = e.Row.DataItem as Dal.ShopAttributeFnResult;

                CheckBox chbExportEnabled = e.Row.FindControl("chbExportEnabled") as CheckBox;

                chbExportEnabled.Checked = ss.AttributeExists.HasValue && ss.AttributeExists.Value;
            }
        }
    }
}