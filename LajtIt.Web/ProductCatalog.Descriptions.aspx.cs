using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("4003f1bc-3169-4dd6-afa0-01b85ac7ec4d")]
    public partial class ProductDescriptions : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShops(); 
                BindProductCatalog();
                
            }
        }

  

        private void BindShops()
        {
            ddlShops.DataSource = Dal.DbHelper.Shop.GetShops().Where(x=>x.CanExportProducts).ToList();
            ddlShops.DataBind();
            ddlShops.SelectedIndex = 0;
        }

        private void BindProductCatalog()
        {
            Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), Int32.Parse(ddlShops.SelectedValue));

            Dal.ProductCatalogShopProductFnResult psp = Dal.DbHelper.ProductCatalog.GetProductCatalogShopProduct(shop, ProductCatalogId);

            if (psp != null)
            {
                txtLongDescription.Text = psp.LongDescription;
                txtShortDescription.Text = psp.ShortDescription;
            }


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogShopProduct psp = new Dal.ProductCatalogShopProduct()
            {
                ShopId = Int32.Parse(ddlShops.SelectedValue),
                LongDescription = txtLongDescription.Text.Trim(),
                ShortDescription = txtShortDescription.Text.Trim(),
                ProductCatalogId = ProductCatalogId
            };

            Dal.DbHelper.ProductCatalog.SetProductCatalogShopProductDescription(psp);
 
            DisplayMessage("Dane zostały zapisane");

        }
 

 

        protected void lbtnPublish_Click(object sender, EventArgs e)
        {


            //Bll.ShopHelper sh = new Bll.ShopHelper();

            //Dal.ProductCatalogUpdateScheduleView schedule = new Dal.ProductCatalogUpdateScheduleView()
            //{
            //    ProductCatalogId = ProductCatalogId,
            //    UpdateCommand = "00000100000"

            //};

            //sh.UpdateShop(schedule, Dal.Helper.UpdateScheduleType.OnlineShopSingle, null);


            Dal.ProductCatalogShopUpdateSchedule sus = new Dal.ProductCatalogShopUpdateSchedule()
            {
                ProcessId = Guid.NewGuid(),
                ProductCatalogId = ProductCatalogId,
                ShopId = (int)Dal.Helper.Shop.Lajtitpl,
                ShopColumnTypeId = (int)Dal.Helper.ShopColumnType.Description,
                UpdateStatusId = (int)Dal.Helper.ShopUpdateStatus.New,
                UpdateTypeId = (int)Dal.Helper.UpdateScheduleType.OnlineShopSingle
            };
            List<Dal.ProductCatalogShopUpdateSchedule> schedules = new List<Dal.ProductCatalogShopUpdateSchedule>();
            schedules.Add(sus);
            //Bll.ShopUpdateHelper.ClickShop cs = new Bll.ShopUpdateHelper.ClickShop();
            //cs.Process(schedules, Guid.NewGuid());
            //DisplayMessage("Opis przekazany do publikacji");

            Bll.ShopRestHelper.Bulk.BulkResult result = Bll.ShopRestHelper.Products.SetProductsUpdateBatch(Dal.Helper.Shop.Lajtitpl, 
                schedules, new int[] { ProductCatalogId });
             

            if (result.errors == false)
                DisplayMessage("Produkt został zaktualizowany");
            else
                DisplayMessage(String.Format("Błąd aktualizacji {0}", Bll.ShopRestHelper.Bulk.BulkResultToString(result)));


        }

        protected void lbnGenerateLong_Click(object sender, EventArgs e)
        {
            Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), Int32.Parse(ddlShops.SelectedValue));

            txtLongDescription.Text = Bll.Mixer.GetDescription(shop, ProductCatalogId);

            DisplayMessage("Opis wygenerowany ale nie zapisany. Jeśli chcesz przywrócić poprzedni opis odśwież stronę");
        }

        protected void rblShops_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProductCatalog();
        }
    }
}