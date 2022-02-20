using AjaxControlToolkit;
using LajtIt.Bll;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("336b11a9-7039-4ee0-9fbd-85a6690230d5")]
    public partial class ProductCatalog_Images : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }

        //private static object Locker = new object();

        protected void Page_Load(object sender, EventArgs e)
        {
            ucUploadImage.Saved += ImageSaved;
            ucUploadImage.ImageIds += ImageIds;

            if (!Page.IsPostBack)
                BindImages(); 
        }
        protected int[] ImageIds(object sender, EventArgs e)
        {
            return new int[] { ProductCatalogId };
        }
        protected void ImageSaved(object sender, EventArgs e)
        {
            BindImages();
        } 

 
        protected void gvImages_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogImage img = e.Row.DataItem as Dal.ProductCatalogImage;
                HyperLink hlImage = e.Row.FindControl("hlImage") as HyperLink;
                System.Web.UI.WebControls.Image imgImage = e.Row.FindControl("imgImage") as System.Web.UI.WebControls.Image;
                Literal litFileName = e.Row.FindControl("litFileName") as Literal;
                TextBox txbDescription = e.Row.FindControl("txbDescription") as TextBox;
                TextBox txbTitle = e.Row.FindControl("txbTitle") as TextBox;
                Literal litSize = e.Row.FindControl("litSize") as Literal;
                CheckBox chbIsActive = e.Row.FindControl("chbIsActive") as CheckBox;
                CheckBox chbIsThumbnail = e.Row.FindControl("chbIsThumbnail") as CheckBox;
                Literal litDate = e.Row.FindControl("litDate") as Literal;
                TextBox txbPriority = e.Row.FindControl("txbPriority") as TextBox;
                HiddenField hidImageId = e.Row.FindControl("hidImageId") as HiddenField;
                DropDownList ddlImageTypeId = e.Row.FindControl("ddlImageTypeId") as DropDownList;
                TextBox txbLinkUrl = e.Row.FindControl("txbLinkUrl") as TextBox;

                hlImage.NavigateUrl = imgImage.ImageUrl = String.Format("/Images/ProductCatalog/{0}", img.FileName);
                litFileName.Text = img.OriginalFileName;
               // txbDescription.Text = img.Description;
               // txbTitle.Text = img.Title;
                litSize.Text = String.Format("{0}x{1} - {2}kB", img.Width, img.Height, img.Size / 1024);
                chbIsActive.Checked = img.IsActive;
              //  chbIsThumbnail.Checked = img.IsThumbnail;
                litDate.Text = String.Format("{0:yyyy/MM/dd HH:mm}", img.InsertDate);
                txbPriority.Text = img.Priority.ToString();
                hidImageId.Value = img.ImageId.ToString();
             //   ddlImageTypeId.SelectedValue = img.ImageTypeId.ToString();
              //  txbLinkUrl.Text = img.LinkUrl;


                //if (img.IsThumbnail)
                //    e.Row.BackColor = System.Drawing.Color.Silver;// .Add("background-color", "silver");

            }
        }

        protected void btnImages_Click(object sender, EventArgs e)
        {
            List<Dal.ProductCatalogImage> images = new List<Dal.ProductCatalogImage>();
            List<int> imagesToDelete = new List<int>();
            foreach (GridViewRow row in gvImages.Rows)
            {
                TextBox txbDescription = row.FindControl("txbDescription") as TextBox;
                TextBox txbTitle = row.FindControl("txbTitle") as TextBox;
                CheckBox chbIsActive = row.FindControl("chbIsActive") as CheckBox;
                CheckBox chbIsThumbnail = row.FindControl("chbIsThumbnail") as CheckBox;
                CheckBox chbDelete = row.FindControl("chbDelete") as CheckBox;
                TextBox txbPriority = row.FindControl("txbPriority") as TextBox;
                HiddenField hidImageId = row.FindControl("hidImageId") as HiddenField;
                DropDownList ddlImageTypeId = row.FindControl("ddlImageTypeId") as DropDownList;
                TextBox txbLinkUrl = row.FindControl("txbLinkUrl") as TextBox;


                Dal.ProductCatalogImage image = new Dal.ProductCatalogImage()
                {
                    ImageId = Convert.ToInt32(hidImageId.Value),
                 //   Description = txbDescription.Text.Trim(),
                 //   Title = txbTitle.Text.Trim(),
                    IsActive = chbIsActive.Checked,
                 //   IsThumbnail = chbIsThumbnail.Checked,
                    Priority =  chbDelete.Checked ? 1000:  Convert.ToInt32(txbPriority.Text.Trim()),
                 //   ImageTypeId = Convert.ToInt32(ddlImageTypeId.SelectedValue),
                 //   LinkUrl = txbLinkUrl.Text.Trim()
                };
                if (chbDelete.Checked)
                    imagesToDelete.Add(Convert.ToInt32(hidImageId.Value));

                images.Add(image);
            }
            Dal.ProductCatalogHelper oh = new Dal.ProductCatalogHelper();
            oh.SetProductCatalogImages(images, imagesToDelete);
            BindImages();
            DisplayMessage("Zmiany w zdjęciach zostały zapisane");
        }
        protected void btnImage_Click(object sender, EventArgs e)
        {
            //if ((fuImage.PostedFile != null) && (fuImage.PostedFile.ContentLength > 0))
            //{
            //    string fileName = String.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(fuImage.PostedFile.FileName));
            //    string oryginalFileName = System.IO.Path.GetFileName(fuImage.PostedFile.FileName);
            //    string saveLocation = Server.MapPath("/Images/ProductCatalog") + "\\" + fileName;
            //    try
            //    {
            //        fuImage.PostedFile.SaveAs(saveLocation);
            //        Bitmap bmp = new Bitmap(saveLocation);
            //        int height = bmp.Height;
            //        int width = bmp.Width;

            //        Dal.ProductCatalogImage image = new Dal.ProductCatalogImage()
            //        {
            //            FileName = fileName,
            //            Height = height,
            //            InsertDate = DateTime.Now,
            //            IsActive = true,
            //            OriginalFileName = oryginalFileName,
            //            Priority = 0,
            //            ProductCatalogId = ProductCatalogId,
            //            Size = fuImage.PostedFile.ContentLength,
            //            Width = width,
            //            Description = ""
            //        };

            //        Dal.OrderHelper oh = new Dal.OrderHelper();
            //        oh.SetProductCatalogImage(image);
            //        BindImages();
            //       // Page.ClientScript.RegisterStartupScript(typeof(Page), "wwew", "window.location.href='#zdjecia';", true);   
            //    }
            //    catch (Exception ex)
            //    {
            //        DisplayMessage(String.Format("Błąd wczytywania pliku: {0}", ex.Message));
            //    }
            //}
            //else
            //{
            //    DisplayMessage("Podaj plik do wczytania");
            //}

            BindImages();
        }

        private void BindImages()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            gvImages.DataSource = oh.GetProductCatalogImages(ProductCatalogId);
            gvImages.DataBind();
        }
    }
}