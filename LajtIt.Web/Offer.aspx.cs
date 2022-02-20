using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;
using System.IO;
using System.Configuration;

namespace LajtIt.Web
{
    [Developer("fc45896c-4bfb-498e-8afc-ed892955aba6")]
    public partial class Offer : LajtitPage
    {
        private int OfferId { get { return Convert.ToInt32(Request.QueryString["id"].ToString()); } }
        private int? OfferVersionId
        {
            get
            {
                if (ddlOfferVersion.SelectedValue== "0")
                    return null;
                else
                    return Convert.ToInt32(ddlOfferVersion.SelectedValue);

            }
        }

        private decimal total = 0;
        private bool isLocked = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int? versionId = null;
                if (Request.QueryString["v"] != null)
                    versionId = Convert.ToInt32(Request.QueryString["v"].ToString().Replace("1908",""));
                BindOffer(versionId);
            }
        }

        private void BindOffer(int? offerVersionId)
        {

            Dal.OfferHelper oh = new Dal.OfferHelper();
            Dal.Offer offer = oh.GetOffer(OfferId);

            List<Dal.OfferVersion> offerVersions = oh.GetOfferVersions(OfferId);

            Dal.OfferVersion offerVersion = null;

            if (offerVersionId == null && offerVersions.Count() > 0)
            {
                offerVersion = offerVersions.FirstOrDefault();
            }

            if (offerVersionId != null)
            {
                offerVersion = offerVersions.Where(x => x.OfferVersionId == offerVersionId.Value).FirstOrDefault();
            }

            if (offerVersions.Count>0)
            { 
            ddlOfferVersion.DataSource = offerVersions.Select(x => new
            {
                OfferVersionId = x.OfferVersionId,
                Name = String.Format("{0:yyyy/MM/dd HH:mm} - {1} {2}", x.LastUpdateDate, x.UpdateUser, x.IsLocked?"":"(robocza)")
            });
            ddlOfferVersion.DataBind();
            }
            if (offerVersion != null)
            {
                isLocked = offerVersion.IsLocked;
                offerVersionId = offerVersion.OfferVersionId;
                ddlOfferVersion.SelectedIndex = ddlOfferVersion.Items.IndexOf(ddlOfferVersion.Items.FindByValue(offerVersionId.ToString()));
            }

            //if (offerVersions.Count() > 0)
            // ddlOfferVersion.Items.Insert(0, new ListItem("--- nowa wersja ---", "0"));


            ddlOfferStatus.DataSource = oh.GetOfferStatuses();
            ddlOfferStatus.DataBind();

            txbContactName.Text = offer.ContactName;
            txbEmail.Text = offer.Email;
            txbName.Text = offer.Name;
            txbPhone.Text = offer.Phone;
            chbShowCode.Checked = offer.ShowCode;
            chbShowSupplier.Checked = offer.ShowSupplier;
            btnOrder.Visible = OfferVersionId.HasValue;
            ddlOfferStatus.SelectedIndex = ddlOfferStatus.Items.IndexOf(ddlOfferStatus.Items.FindByValue(offer.OfferStatusId.ToString()));


 

            if (offerVersionId != null)
            {
                gvOfferProducts.DataSource = oh.GetOfferProducts(offerVersionId.Value);
                gvOfferProducts.DataBind();

            }

            btnSaveOfferProducts.Visible = btnLock.Visible = !isLocked;
            upNewProduct.Visible = (!isLocked && offerVersionId!=null) || offerVersionId==null;
            btnDownload.Visible = offerVersionId != null;
            btnNewOfferVersion.Visible = offerVersions.Where(x => x.IsLocked == false).Count() == 0;
            btnDuplicate.Visible = offerVersions.Where(x => x.IsLocked == false).Count() == 0 && offerVersionId!=null;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dal.OfferHelper oh = new Dal.OfferHelper();
            Dal.Offer offer = new Dal.Offer()
            {
                ContactName = txbContactName.Text.Trim(),
                Email = txbEmail.Text.Trim(),
                Phone = txbPhone.Text.Trim(),
                Name = txbName.Text.Trim(),
                OfferId = OfferId,
                OfferStatusId = Convert.ToInt32(ddlOfferStatus.SelectedValue),
                ShowCode = chbShowCode.Checked,
                ShowSupplier = chbShowSupplier.Checked
            };

            oh.SetOffer(offer, UserName);

            DisplayMessage("Zapisano zmiany");
        }
        protected void btnProductAdd_Click(object sender, EventArgs e)
        {
            Dal.OfferHelper oh = new Dal.OfferHelper();
            List<string> message = new List<string>();


            string productName = Request.Form[txbProductCode.UniqueID];
            string sproductCatalogId = Request.Form[hfProductCatalogId.UniqueID];
            int productCatalogId = 0;

            if (!Int32.TryParse(sproductCatalogId, out productCatalogId))
            {
                DisplayMessage("Produkt nie istnieje");
                return;
            }

            int offerVersionId = oh.SetOfferProduct(OfferId, OfferVersionId, productCatalogId, UserName);
            if (message.Count > 0)
            {
                DisplayMessage(String.Format("Wykryto błędy podczas dodawania produktu: <ul>{0}</ul>",
                    String.Join("<li>- ", message)));
                return;
            }
            //oh.AddProductCatalogToOrder(OrderId, productCatalogId, quantity, UserName);
            BindOffer(OfferVersionId);
            txbProductCode.Text = "";
        }


        protected void gvOfferProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblProductCatalogSupplier = e.Row.FindControl("lblProductCatalogSupplier") as Label;
                Label lblProductCatalogName = e.Row.FindControl("lblProductCatalogName") as Label;
                Label lblProductCatalogCode = e.Row.FindControl("lblProductCatalogCode") as Label;
                Label lblProductCatalogPrice = e.Row.FindControl("lblProductCatalogPrice") as Label;
                Label lblAdditionlInfo = e.Row.FindControl("lblAdditionlInfo") as Label;
                TextBox txbOfferPrice = e.Row.FindControl("txbOfferPrice") as TextBox;
                TextBox txbOfferQuantity = e.Row.FindControl("txbOfferQuantity") as TextBox;
                TextBox txbOfferRebate = e.Row.FindControl("txbOfferRebate") as TextBox;
                TextBox txbOfferName = e.Row.FindControl("txbOfferName") as TextBox;
                TextBox txbComment = e.Row.FindControl("txbComment") as TextBox;
                ImageButton ibtnDelete = e.Row.FindControl("ibtnDelete") as ImageButton;
                HyperLink hlImage = e.Row.FindControl("hlImage") as HyperLink;
                Image imgImage = e.Row.FindControl("imgImage") as Image;
                HiddenField hidOfferProductId = e.Row.FindControl("hidOfferProductId") as HiddenField;
 
                Dal.OfferProductsView offerProduct = e.Row.DataItem as Dal.OfferProductsView;

                if (offerProduct.Bulb != null)
                    lblAdditionlInfo.Text = String.Format("Żarówka w zestawie: {0}", offerProduct.Bulb);

                hlImage.NavigateUrl = String.Format(hlImage.NavigateUrl, offerProduct.ProductCatalogId);
                hidOfferProductId.Value = offerProduct.Id.ToString();
                ibtnDelete.CommandArgument = offerProduct.Id.ToString();
                lblProductCatalogName.Text = offerProduct.ProductName;
                lblProductCatalogCode.Text = offerProduct.Code;
                lblProductCatalogSupplier.Text = offerProduct.SupplierName;

                lblProductCatalogPrice.Text = String.Format("{0:C}", offerProduct.PriceBruttoFixed);

                txbOfferName.Text = offerProduct.Name  ;
               
                txbOfferPrice.Text = String.Format("{0:0.00}", offerProduct.Price);
                txbOfferQuantity.Text = String.Format("{0}", offerProduct.Quantity);
                txbOfferRebate.Text = String.Format("{0:0.00}", offerProduct.Rebate);

                if (offerProduct.ImageFullName != null)
                    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", offerProduct.ImageFullName.Replace(".", "_m."));
                else
                    imgImage.Visible = false;

                ibtnDelete.Visible = !isLocked;
                txbOfferName.Enabled = txbOfferPrice.Enabled = txbOfferQuantity.Enabled = txbOfferRebate.Enabled = txbComment.Enabled = !isLocked;
                txbComment.Text = offerProduct.Comment;
                decimal price = offerProduct.Price ?? offerProduct.PriceBruttoFixed;
                total += price * offerProduct.Quantity * (1 - offerProduct.Rebate / 100);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label litTotalFooter = e.Row.FindControl("litTotalFooter") as Label;

                litTotalFooter.Text = String.Format("Razem: {0:C}", total);

            }

        }
        protected void ibtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            int offerProductId = Convert.ToInt32((sender as ImageButton).CommandArgument);
            Dal.OfferHelper oh = new Dal.OfferHelper();
            oh.SetOfferProductDelete(offerProductId);
            BindOffer(OfferVersionId);
        }

        protected void btnSaveOfferProducts_Click(object sender, EventArgs e)
        {
            SaveOfferProduct(false);
        }
        private void SaveOfferProduct(bool lockOffer)
        {

            List<Dal.OfferProduct> offerProducts = new List<Dal.OfferProduct>();

            foreach (GridViewRow item in gvOfferProducts.Rows)
            {
                TextBox txbOfferPrice = item.FindControl("txbOfferPrice") as TextBox;
                TextBox txbOfferQuantity = item.FindControl("txbOfferQuantity") as TextBox;
                TextBox txbOfferRebate = item.FindControl("txbOfferRebate") as TextBox;
                TextBox txbOfferName = item.FindControl("txbOfferName") as TextBox;
                TextBox txbComment = item.FindControl("txbComment") as TextBox;
                HiddenField hidOfferProductId = item.FindControl("hidOfferProductId") as HiddenField;

                decimal? price = null;
                if (txbOfferPrice.Text.Trim() != "")
                    price = Convert.ToDecimal(txbOfferPrice.Text.Trim());

                offerProducts.Add(new Dal.OfferProduct()
                {
                    Id = Convert.ToInt32(hidOfferProductId.Value),
                    Name = txbOfferName.Text.Trim() == "" ? null : txbOfferName.Text.Trim(),
                    OfferVersionId = OfferVersionId.Value,
                    Price = price,
                    Quantity = Convert.ToInt32(txbOfferQuantity.Text.Trim()),
                    Rebate = Convert.ToDecimal(txbOfferRebate.Text.Trim()),
                    Comment = txbComment.Text.Trim()
                });
            }

            Dal.OfferHelper oh = new Dal.OfferHelper();
            string fileName = String.Format("Oferta-1908{0}.pdf", OfferVersionId);
          

            oh.SetOfferProducts(OfferVersionId.Value, offerProducts, lockOffer, fileName);

            Bll.OfferHelper ohb = new Bll.OfferHelper();
            Bll.PdfCreator pdf = new Bll.PdfCreator();

            string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];

            
            if (lockOffer)
            {
                //SaveOfferPdf(ohb.GetOfferVersion(OfferVersionId.Value), fileName);
                SaveOfferPdf(pdf.CreateOffer(path, OfferId, OfferVersionId.Value, chbOfferHeader.Checked, chbOfferFooter.Checked), fileName);
                DisplayMessage("Zapisano zmiany w produktach. Oferta została zablokowana do edycji.");
            }
            else
                DisplayMessage("Zapisano zmiany w produktach");
            BindOffer(OfferVersionId);

        }

        private void SaveOfferPdf(string fileName, string newFileName)
        {
            string newPathFileName = String.Format(@"{0}\{1}",
              System.Web.HttpContext.Current.Server.MapPath("/Files/Offers"),
              newFileName);
            File.Copy(fileName, newPathFileName);
        }

        protected void btnLock_Click(object sender, EventArgs e)
        {
            SaveOfferProduct(true);
        }

        protected void ddlOfferVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
             
            BindOffer(OfferVersionId);
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Dal.OfferHelper oh = new OfferHelper();
            Dal.OfferVersion ov = oh.GetOfferVersion(OfferVersionId.Value);

            Bll.OfferHelper ohb = new Bll.OfferHelper();
            if (ov.IsLocked)
            {
                string newFileName = String.Format(@"{0}\{1}",
                  System.Web.HttpContext.Current.Server.MapPath("/Files/Offers"),
                  ov.FileName);
                ohb.SendFileToOutput(newFileName);
            }
            else
            {
                string path = ConfigurationManager.AppSettings[String.Format("ProductExportFilesDirectory_{0}", Dal.Helper.Env.ToString())];
                Bll.PdfCreator pdf = new Bll.PdfCreator();
                ohb.SendFileToOutput(pdf.CreateOffer(path, OfferId, OfferVersionId.Value, chbOfferHeader.Checked, chbOfferFooter.Checked));
            }
        }

        protected void btnNewOfferVersion_Click(object sender, EventArgs e)
        {
            Dal.OfferHelper oh = new Dal.OfferHelper();
            oh.SetOfferVersion(OfferId, null, UserName);
            BindOffer(null);
            DisplayMessage("Utworzono nową wersję oferty");
        }

        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            Dal.OfferHelper oh = new Dal.OfferHelper();
            oh.SetOfferVersion(OfferId, OfferVersionId, UserName);
            BindOffer(null);
            DisplayMessage("Utworzono nową wersję oferty z bieżącej");

        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {

            Dal.OfferHelper oh = new OfferHelper();
            Dal.OfferVersion ov = oh.GetOfferVersion(OfferVersionId.Value);

            Bll.OfferHelper ohb = new Bll.OfferHelper();

            int orderId = oh.SetOrderFromOffer(OfferId, OfferVersionId, this.UserShopId, UserName);

            DisplayMessage(String.Format("Nowe zamówienie zostało utworzone. <a href='order.aspx?id={0}' target='_blank'>Kliknij tutaj i sprawdź</a>", orderId));
        }
    }
}