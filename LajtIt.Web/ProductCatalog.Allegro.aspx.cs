using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;

namespace LajtIt.Web
{
    [Developer("2fcf2dc9-0965-4880-86ff-ef8e661f7252")]
    public partial class ProductAllegro : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { 
                BindProductCatalog();

            }

            //string js = "$('#" + fuImage.ClientID + "').fileUpload({'uploader': '/Scripts/uploader.swf','cancelImg': '/images/cancel.jpg','buttonText': 'Wybierz','script': '/Upload.ashx', 'folder':'" +
            //    ProductCatalogId.ToString() + "','fileDesc': 'Image Files','fileExt': '*.jpg;*.jpeg;*.gif;*.png','multi': true,'auto': true});";


            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "loadProduct", js, true);

        }


        protected void btnAllegroUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                if (Bll.ProductFileImportHelper.UpdateAllegro(ProductCatalogId))
                    DisplayMessage("Oferty zostały zaktualizowane");
                else
                    DisplayMessage("Błąd aktualizacji ofert. Sprawdź błędy w tabeli ofert");

                BindProductCatalog();
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
            }
        }
    
 
        
        //protected void btnAddRecommended_Click(object sender, EventArgs e)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    oh.SetProductCatalogAllegroItemRecommend(ProductCatalogId, WebHelper.GetSelectedIds(gvProductSearchResult, "chbProductCatalog0"));
        //    btnProductSearch_Click(null, null);
        //}

        //protected void btnDeleteRecommended_Click(object sender, EventArgs e)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    oh.SetProductCatalogAllegroItemRemoveRecommend(ProductCatalogId, WebHelper.GetSelectedIds(gvProductReleated, "chbProductCatalog1"));
        //    btnProductSearch_Click(null, null);
        //}

   
    //    private void BindRecommendedProducts()
    //    {
    //        Dal.OrderHelper oh = new Dal.OrderHelper();

    //        //images = oh.GetProductCatalogImages();

    //        gvProductReleated.DataSource = oh.GetProductCatalogAllegroRecommended(ProductCatalogId)
    //            .Where(x => x.SelectionType == 1).ToList() ;
    //        gvProductReleated.DataBind();


    //}
        //protected void gvProductReleated_OnRowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        Dal.ProductCatalogRecommendedProductsResult pc = e.Row.DataItem as Dal.ProductCatalogRecommendedProductsResult;

        //        System.Web.UI.WebControls.Image imgImage = e.Row.FindControl("imgImage") as System.Web.UI.WebControls.Image;
        //        HyperLink hlName = e.Row.FindControl("hlName") as HyperLink;
        //        Literal litQuantity = e.Row.FindControl("litQuantity") as Literal;
        //        Literal litPrice = e.Row.FindControl("litPrice") as Literal;
        //        Literal litCategory = e.Row.FindControl("litCategory") as Literal;
        //        //Literal litLampType = e.Row.FindControl("litLampType") as Literal;
        //        Literal litSelectionType = e.Row.FindControl("litSelectionType") as Literal;
        //        LinkButton lbtnProduct = e.Row.FindControl("lbtnProduct") as LinkButton;

        //        //litLampType.Text = pc.LampCategoryName;
        //        litCategory.Text = pc.AllegroCategoryFullName;
        //        switch (pc.SelectionType)
        //        {
        //            case 1: litSelectionType.Text = "Ręczny"; break;
        //            case 2: litSelectionType.Text = "Wg typu lampy"; break;
        //            case 3: litSelectionType.Text = "Losowy"; break;

        //        }
        //        imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", pc.ImageFullName);

        //        //Dal.ProductCatalogImage img = images.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();
        //        //if (img != null)
        //        //    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", img.FileName);
        //        //else
        //        //    imgImage.Visible = false;


        //        hlName.Text = pc.AllegroName;
        //        hlName.NavigateUrl = String.Format(hlName.NavigateUrl, pc.ProductCatalogId);

               

        //        //litQuantity.Text = calc
        //        litPrice.Text = "";// String.Format("{0:C}", calc.SellPriceBrutto);


        //        lbtnProduct.CommandArgument = pc.ProductCatalogId.ToString();
        //        lbtnProduct.Visible = pc.SelectionType == 1;
        //    }
        //}
        protected void gvProductSearchResult_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogView pc = e.Row.DataItem as Dal.ProductCatalogView;

            

                System.Web.UI.WebControls.Image imgImage = e.Row.FindControl("imgImage") as System.Web.UI.WebControls.Image;
                HyperLink hlName = e.Row.FindControl("hlName") as HyperLink;
                Literal litQuantity = e.Row.FindControl("litQuantity") as Literal;
                Literal litPrice = e.Row.FindControl("litPrice") as Literal;
                //Literal litCategory = e.Row.FindControl("litCategory") as Literal;
               // Literal litLampType = e.Row.FindControl("litLampType") as Literal;
                LinkButton lbtnProduct = e.Row.FindControl("lbtnProduct") as LinkButton;

                //litLampType.Text = pc.LampCategoryName ?? "";
                //litCategory.Text = pc.AllegroCategoryFullName ?? "";

                imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", pc.ImageFullName);

                //Dal.ProductCatalogImage img = images.Where(x => x.ProductCatalogId == pc.ProductCatalogId).FirstOrDefault();
                //if (img != null)
                //    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", img.FileName);
                //else
                //    imgImage.Visible = false;


                //Bll.ProductCatalogCalculator calc = new Bll.ProductCatalogCalculator(pc.CurrentPrice, pc.AllegroPrice, pc.Rebate.Value, pc.Margin.Value);
                hlName.Text = pc.Name;
                hlName.NavigateUrl = String.Format(hlName.NavigateUrl, pc.ProductCatalogId);
                //litPrice.Text = String.Format("{0:C}", calc.SellPriceBrutto);


                lbtnProduct.CommandArgument = pc.ProductCatalogId.ToString(); 
            }
        }
      
     
        private void BindProductCatalog()
        {
            int productCatalogId = ProductCatalogId;
            Dal.OrderHelper oh = new Dal.OrderHelper();
           // Dal.ProductCatalogItemStatsResult stat = oh.GetProductCatalogItemStats(productCatalogId);
            Dal.ProductCatalogView pc = oh.GetProductCatalog(productCatalogId);
            //Dal.ProductCatalogAllegroItem productCatalogAllegroItem = oh.GetProductCatalogAllegroItemActive(productCatalogId);
            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();
            //Dal.ProductCatalogDeliveryStatsResult productDelivery = h.GetProductDeliveryStats(productCatalogId);
            Dal.Supplier supplier = oh.GetSupplier(pc.SupplierId);
        
            //if (pc.LeftQuantity == null)
            //    litAllegroQuantity.Text = "Ilość nielimitowana";
            //else
                litAllegroQuantity.Text = pc.LeftQuantity.ToString();
             

          

            litPriceBrutto.Text = String.Format("{0:C}", pc.PriceBruttoFixed);

        
            chbAutoAssignProduct.Checked = pc.AutoAssignProduct;
             
            hlPreview.NavigateUrl = String.Format("ProductCatalogPreview.aspx?idProduct={0}&idSupplier={1}", ProductCatalogId, pc.SupplierId);
 

            //if (productCatalogAllegroItem != null)
            //{
            //    btnDeleteFromAuction.Visible  = true;
            //    btnDeleteFromAuction.CommandArgument = productCatalogAllegroItem.Id.ToString();
            //    btnCreateAuction.Enabled = pnBatch.Enabled= false;
            //    //litScheduled.Text = String.Format("Produkt zakolejkowany w dniu {0:yyyy/MM/dd HH:mm} do wystawienia, batch: <b>{1}</b> ",
            //    //    productCatalogAllegroItem.InsertDate, productCatalogAllegroItem.ProductCatalogAllegroItemBatch.Name);
            //    //ddlBatch.SelectedIndex = ddlBatch.Items.IndexOf(ddlBatch.Items.FindByValue(productCatalogAllegroItem.BatchId.ToString()));
            //}
            //else
            //{
            //    btnDeleteFromAuction.Visible = false;
            //    btnCreateAuction.Enabled = pnBatch.Enabled = true;
            //    litScheduled.Text = "";
            //} 
            //BindRecommendedProducts();
            BindAllegroHistory();          
        }

  
        private void BindAllegroHistory()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.ProductCatalogAllegroHistoryView> allegro = oh.GetProductCatalogAllegroItemHistory(ProductCatalogId);
            gvAllegroHistory.DataSource = allegro; 
            gvAllegroHistory.DataBind(); 
        }
        
        
      

        protected void btnAllegro_Click(object sender, EventArgs e)
        {   

            Dal.ProductCatalog pc = new Dal.ProductCatalog()
            {
                ProductCatalogId = ProductCatalogId,
                AutoAssignProduct = chbAutoAssignProduct.Checked
           };
          

        
            Dal.OrderHelper oh = new Dal.OrderHelper(); 
            oh.SetProductCatalogAllegroData(pc);
            DisplayMessage("Zapisano");
            BindProductCatalog();
        }
 
    
        private int getIntFromBitArray(bool[] a)
        {
            int value = 0;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i])
                    value += Convert.ToInt32(Math.Pow(2, i));
            }

            return value;
        }

 
        
       
        protected void btnCreateAuction_Click(object sender, EventArgs e)
        {

            throw new NotImplementedException();
        }

        protected void gvAllegroHistory_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogAllegroHistoryView h = e.Row.DataItem as Dal.ProductCatalogAllegroHistoryView;

                Label lbUserName = e.Row.FindControl("lbUserName") as Label;
                Label lblComment = e.Row.FindControl("lblComment") as Label;
                HyperLink hlItem = e.Row.FindControl("hlItem") as HyperLink;

                hlItem.Text = h.ItemId.ToString();

                hlItem.NavigateUrl = String.Format(hlItem.NavigateUrl, h.ItemId);
                lbUserName.Text = h.UserName;
                lblComment.Text = h.Comment;
                //if (h.IsPromoted.HasValue && h.IsPromoted.Value)
                //    e.Row.Style.Add("background-color", "silver");


                System.Web.UI.WebControls.Image imgIsAuction = e.Row.FindControl("imgIsAuction") as System.Web.UI.WebControls.Image;
                Label lblPrice = e.Row.FindControl("lblPrice") as Label;

                //imgIsAuction.Visible = h.SellingMode!=null && h.SellingMode=="AUCTION";
                lblPrice.Text = String.Format("{0:C}",  h.CurrentPrice );

            }

        }
    }
}