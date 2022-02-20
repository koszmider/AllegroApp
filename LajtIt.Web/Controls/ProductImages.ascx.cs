using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using LajtIt.Dal;

namespace LajtIt.Web.Controls
{
    public partial class ProductImages : LajtitControl
    {
        bool canChangeImages = false;
        int count = 0;
        public void BindImages(int productCatalogId, bool canChangeImg)
        {
            canChangeImages = canChangeImg;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImages(productCatalogId);

            rpImages.DataSource = images;
            rpImages.DataBind();

            ListView1.DataSource = images;
            ListView1.DataBind();
             
        }

        protected void rpImages_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType== System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                Image imgImage = e.Item.FindControl("imgImage") as Image;
                ImageButton imgbImage = e.Item.FindControl("imgbImage") as ImageButton;
                ImageButton imgbDelete = e.Item.FindControl("imgbDelete") as ImageButton;

                Dal.ProductCatalogImage image = e.Item.DataItem as Dal.ProductCatalogImage;

                imgImage.ImageUrl = String.Format("/images/productcatalog/{0}_m{1}", System.IO.Path.GetFileNameWithoutExtension(image.FileName),
                        System.IO.Path.GetExtension(image.FileName));
                imgbImage.ImageUrl = String.Format("/images/productcatalog/{0}_m{1}", System.IO.Path.GetFileNameWithoutExtension(image.FileName),
                        System.IO.Path.GetExtension(image.FileName));
                imgbImage.CommandArgument = image.ImageId.ToString();
                imgbDelete.CommandArgument = image.ImageId.ToString();

                //imgbImage.Visible = canChangeImages;
                //imgImage.Visible = !canChangeImages;

                if (count == 0)
                    imgbDelete.Visible = false;
                count++;

            }
        }

        protected void imgbImage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            int imageId = Int32.Parse((sender as ImageButton).CommandArgument);

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            int productCatalogId = pch.SetProductCatalogImageMain(imageId);

            Repeater rpImages = (((sender as ImageButton).Parent.Parent) as Repeater);


            List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImages(productCatalogId);

            rpImages.DataSource = images;
            rpImages.DataBind();

        }

        protected void imgbDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            int imageId = Int32.Parse((sender as ImageButton).CommandArgument);

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<int> imageIds = new List<int>();
            imageIds.Add(imageId);

            int productCatalogId = pch.GetProductCatalogImage(imageId).ProductCatalogId ;

             pch.SetProductCatalogImages(new List<ProductCatalogImage>(), imageIds);

            Repeater rpImages = (((sender as ImageButton).Parent.Parent) as Repeater);


            List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImages(productCatalogId);

            rpImages.DataSource = images;
            rpImages.DataBind();

        }
    }
}