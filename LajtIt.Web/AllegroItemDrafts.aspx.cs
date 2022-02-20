using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("c87e7c28-b07c-45be-9466-8fd8149581d7")]
    public partial class AllegroItemDrafts : LajtitPage
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                BindAllegroMyUsers(lsbxUserId);
                BindAttributeGroups();
            }
        }

       

        private void BindAttributeGroups()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            ddlAttributes.DataSource = pch.GetProductCatalogAttributeGroups();
            ddlAttributes.DataBind();
             
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindItems();
        }

        protected void gvAllegroItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvAllegroItems.PageIndex = e.NewPageIndex;
            BindItems();
        }

        protected void ddlAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            BinAttributes();

        }

        private void BinAttributes()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            ddlAttributesValue.DataSource = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributes(Convert.ToInt32(ddlAttributes.SelectedValue));
            ddlAttributesValue.DataBind();
        }

        private void BindItems()
        {
            try
            {
                gvAllegroItems.PageSize = Int32.Parse(txbPageSize.Text);
                bool? isValid = null, isImageReady = null;
                if (ddlIsValid.SelectedIndex != 0)
                    isValid = Int32.Parse(ddlIsValid.SelectedValue) == 1;
                if (ddlIsImageReady.SelectedIndex != 0)
                    isImageReady = Int32.Parse(ddlIsImageReady.SelectedValue) == 1;

                string itemStatus = ddlAllegroOfferStatus.SelectedValue;

                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

                long[] userIds = lsbxUserId.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int64.Parse(x.Value)).ToArray();

                List<Dal.ProductCatalogAllegroItemsView> allegroItems =
                        Dal.DbHelper.ProductCatalog.Allegro.GetProductCatalogAllegroItems(itemStatus
                    , txbSearchName.Text
                    , isValid
                    , isImageReady
                    , txbComment.Text
                    , userIds);


                // na końcu to wywołać
                if (chbAttributesSearch.Checked)
                {

                    bool? exists = null;

                    if (ddlAttributesExists.SelectedIndex != 0)
                        exists = ddlAttributesExists.SelectedIndex == 1;
                    bool? existsValue = null;
                    int? attributeId = null;
                    if (ddlAttributesValueExists.SelectedIndex != 0)
                    {
                        existsValue = ddlAttributesValueExists.SelectedIndex == 1; ;
                        attributeId = Convert.ToInt32(ddlAttributesValue.SelectedValue);
                    }
                    int attributeGroupId = Convert.ToInt32(ddlAttributes.SelectedValue);
                    int[] filteredProductCatalogIds = allegroItems.Select(x => x.ProductCatalogId).ToArray();
                    int[] foundProductCatalogIds = pch.GetProductCatalogAttributes(filteredProductCatalogIds, attributeGroupId, exists, attributeId, existsValue);

                    allegroItems = allegroItems.Where(x => foundProductCatalogIds.Contains(x.ProductCatalogId)).ToList();

                }
                gvAllegroItems.Columns[6].Visible = chbAttributesSearch.Checked;

                gvAllegroItems.DataSource = allegroItems;
                gvAllegroItems.DataBind();



                txbPageNo.Text = (gvAllegroItems.PageIndex + 1).ToString();
                lblPageNo.Text = String.Format("/{0}", gvAllegroItems.PageCount);
                lblCount.Text = String.Format("{0}", allegroItems.Count());

                if (allegroItems.Count > 0)
                    pnDelete.Visible = HasActionAccess(Guid.Parse("aa2a56aa-b423-40eb-8485-21e2b8a34a0a"));
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
            }

        }
        protected void txbPageNo_TextChanged(object sender, EventArgs e)
        {
            gvAllegroItems.PageIndex = Int32.Parse(txbPageNo.Text) - 1;
            BindItems();
        }
        protected void btnAction_Click1(object sender, EventArgs e)
        {
            bool isReady = true;
            foreach (GridViewRow row in gvAllegroItems.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int productCatalogId = Convert.ToInt32(gvAllegroItems.DataKeys[row.RowIndex][0]);
                    if (chbAttributesSearch.Checked)
                    {  
                        LajtIt.Web.Controls.ProductAttributes ucProductAttributesProduct = row.FindControl("ucProductAttributesProduct") as LajtIt.Web.Controls.ProductAttributes;
                        ucProductAttributesProduct.ProductCatalogId = productCatalogId;

                        if (ucProductAttributesProduct.AttributesSave(productCatalogId, true))//, clearBeforeAdd, groupIdToClear))
                            isReady = false;
                    }
                }
            }
            if (isReady)
                DisplayMessage("Zapisano");
            else

                DisplayMessage("Zmiany zostały zapisane. Niektóre produkty wymagają uzupełnienia obowiązkowych atrybutów");
            BindItems();

        }

        protected void gvAllegroItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.ProductCatalogAllegroItemsView item = e.Row.DataItem as Dal.ProductCatalogAllegroItemsView;

                Image imgImage = e.Row.FindControl("imgImage") as Image;
                HyperLink hlProductImage = e.Row.FindControl("hlProductImage") as HyperLink;
                Image imgOK = e.Row.FindControl("imgOK") as Image;
                Image imgFalse = e.Row.FindControl("imgFalse") as Image;
                Image imgVOK = e.Row.FindControl("imgVOK") as Image;
                Image imgVFalse = e.Row.FindControl("imgVFalse") as Image;
                HyperLink hlItem = e.Row.FindControl("hlItem") as HyperLink;
                HyperLink hlProduct = e.Row.FindControl("hlProduct") as HyperLink;
                Literal liId = e.Row.FindControl("liId") as Literal;


                //if (!String.IsNullOrEmpty(item.ImageFullName))
                //    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}_m{1}", System.IO.Path.GetFileNameWithoutExtension(item.ImageFullName), 
                //        System.IO.Path.GetExtension(item.ImageFullName));
                //else
                    imgImage.Visible = false;


                liId.Text = String.Format("{0}.", gvAllegroItems.PageIndex * gvAllegroItems.PageSize + e.Row.RowIndex + 1);
                hlProduct.Text = String.Format("{0}", item.ProductName);
                hlProductImage.NavigateUrl = String.Format(hlProduct.NavigateUrl, item.ProductCatalogId);
                hlProduct.NavigateUrl = String.Format(hlProduct.NavigateUrl, item.ProductCatalogId);
                if(item.ItemId==0)
                { 
                hlItem.Text = String.Format("{0}", item.UserName);
                hlItem.NavigateUrl = String.Format(hlItem.NavigateUrl, item.ItemId);
                }
                else
                {
                    hlItem.Text = String.Format("{0} {1}", item.ItemId, item.UserName);
                    hlItem.NavigateUrl = String.Format(hlItem.NavigateUrl, item.ItemId);

                }
                //imgOK.Visible = item.IsImageReady.Value;
                //imgFalse.Visible = !item.IsImageReady.Value;

                imgVOK.Visible = item.IsValid.HasValue && item.IsValid.Value;
                imgVFalse.Visible = !item.IsValid.HasValue || !item.IsValid.Value;



                Bll.AllegroRESTHelper.DraftError.RootObject validationResult = GetErrorObject(item.Comment);


                Label lblComment = e.Row.FindControl("lblComment") as Label;
                GridView gvErrors = e.Row.FindControl("gvErrors") as GridView;

                if (validationResult == null)
                {
                    lblComment.Text = item.Comment;

                }
                else
                {
                    gvErrors.DataSource = validationResult.errors;
                    gvErrors.DataBind();
                }

                if (chbAttributesSearch.Checked)
                {
                 
                    LajtIt.Web.Controls.ProductAttributes ucProductAttributesProduct = e.Row.FindControl("ucProductAttributesProduct") as LajtIt.Web.Controls.ProductAttributes;
                    ucProductAttributesProduct.ProductCatalogId = item.ProductCatalogId;
                    ucProductAttributesProduct.EnableAdding = false;
                    //ucProductAttributesProduct.IsRequired = true;

                    ucProductAttributesProduct.BindAttributeGroups(new int[] { Convert.ToInt32(ddlAttributes.SelectedValue) });
                }


            }
        }

        private Bll.AllegroRESTHelper.DraftError.RootObject GetErrorObject(string comment)
        {
            var json_serializer = new JavaScriptSerializer();

            try
            {
                return json_serializer.Deserialize<Bll.AllegroRESTHelper.DraftError.RootObject>(comment);

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            long[] itemIds = WebHelper.GetSelectedIds<long>(gvAllegroItems, "chbOrder",1);

            Dal.OrderHelper pch = new Dal.OrderHelper();
            pch.SetAllegroActions(itemIds, 1, chbDoNotReActive.Checked, "Usunięte z AllegroItemDrafts.aspx");
            DisplayMessage(String.Format("Zakolejkowana wykonanie akcji w {0} oferatch",
                itemIds.Length));
        }
    }
}