using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("3a69c76a-5d85-4ac3-9b35-85f0a7ed3865")]
    public partial class ShopMainPageCreator : LajtitPage
    {

        public class Product
        {

            public int Id { get; set; }
            public int Priority { get; set; }
            public int ShopMainPageGroupId { get; set; }
            public string GroupName { get; set; }
            public bool GroupIsActive { get; set; }
            public int ProductCatalogId { get; set; }
            public string Name { get; set; }
            public decimal PriceBrutto { get; set; }
            public string ImageFullName { get; set; }
            public int? ShopProductId { get; set; }
            public bool IsActiveOnlineShop { get; set; }

        }
        protected int NewListOrderNumber;
        protected List<AjaxControlToolkit.ReorderListItem> ListDataItems;

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!Page.IsPostBack)
            {
                BindMainPageGroups();
                BindProducts();
            }
        }
        protected void btnActive_Click(object sender, EventArgs e)
        {

            Dal.ShopHelper sh = new Dal.ShopHelper();
            int groupId = sh.SetShopMainPageGroupActive(Convert.ToInt32(ddlMainPageGroup.SelectedValue));

            Bll.ShopHelper wcfSh = new Bll.ShopHelper();
            wcfSh.SetShopMainPage(groupId);


            DisplayMessage("Grupa ustawiona jako aktywna");
            BindMainPageGroups();
            ddlMainPageGroup.SelectedValue = groupId.ToString();
            BindProducts();
        }

        protected void lbtnSaveNew_Click(object sender, EventArgs e)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();

            int groupId = Convert.ToInt32(ddlMainPageGroup.SelectedValue);
            int newGroupId = 0;

            if (groupId == 0)
            {

                Bll.ShopHelper wcfSh = new Bll.ShopHelper();

                int[] shopProductsId = wcfSh.GetShopMainPage();

                newGroupId = sh.SetShopMainPageNewGroupByShopIds(shopProductsId, txbShopMainPageGroupName.Text.Trim());


            }
            else
            {

                newGroupId = sh.SetShopMainPageGroupNew(groupId, txbShopMainPageGroupName.Text.Trim());
            }
            DisplayMessage("Zapisano");
            BindMainPageGroups();
            ddlMainPageGroup.SelectedValue = newGroupId.ToString();
            BindProducts();
        }
        private void BindProducts()
        {
            int groupId = Convert.ToInt32(ddlMainPageGroup.SelectedValue);
            rlProducts.Enabled = groupId > 0;

            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<Dal.ShopMainPageView> smpv = sh.GetShopMainPageProducts(groupId);
            List<Product> products = smpv.Select(x => new Product()
            {
                GroupIsActive = x.GroupIsActive,
                GroupName = x.GroupName,
                Id = x.Id,
                ImageFullName = x.ImageFullName,
                IsActiveOnlineShop = x.IsActiveOnline.Value,
                Name = x.Name,
                PriceBrutto = x.PriceBruttoShop.Value,
                Priority = x.Priority,
                ProductCatalogId = x.ProductCatalogId,
                ShopMainPageGroupId = x.ShopMainPageGroupId,
                ShopProductId = x.ShopProductId
            }).ToList();
            rlProducts.DataSource = products;
            rlProducts.DataBind();
        }
        protected void rlProducts_OnItemDataBound(object sender, AjaxControlToolkit.ReorderListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Label lblProductName = e.Item.FindControl("lblProductName") as Label;
                Label lblPriceBrutto = e.Item.FindControl("lblPriceBrutto") as Label;
                HyperLink hlProduct = e.Item.FindControl("hlProduct") as HyperLink;
                HyperLink hlShop = e.Item.FindControl("hlShop") as HyperLink;
                Image imgImage = e.Item.FindControl("imgImage") as Image;

                Product smp = e.Item.DataItem as Product;

                lblProductName.Text = smp.Name;
                lblPriceBrutto.Text = String.Format("{0:C}", smp.PriceBrutto);
                imgImage.ImageUrl = String.Format("/Images/ProductCatalog/{0}", smp.ImageFullName);
                hlProduct.NavigateUrl = String.Format("/Product.aspx?id={0}", smp.ProductCatalogId);

                if (smp.ShopProductId.HasValue)
                {
                    hlShop.NavigateUrl = String.Format("http://lajtit.pl/pl/p/p/{0}", smp.ShopProductId);
                }
                else
                {
                    hlShop.Visible = false;
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int>();
            //foreach (AjaxControlToolkit.ReorderListItem item in rlProducts.Items)
            //{
            //    int id =0;// rlProducts.DataKeys[0][1];// 90 item.ItemIndex;
            //    list.Add(id);
            //}

            list.AddRange(rlProducts.DataKeys.Cast<int>().ToList());
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindProducts();

        }
        private void BindMainPageGroups()
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<Dal.ShopMainPageGroup> groups = new List<Dal.ShopMainPageGroup>();
            groups = sh.GetShopMainPageGroups();

            var g = groups.Select(x => new
            {
                ShopMainPageGroupId = x.ShopMainPageGroupId,
                Name = String.Format("{0}|{1}|{2}",
                       LajtIt.Web.Controls.Products.SetColumn(x.Name, 30, 1),
                        LajtIt.Web.Controls.Products.SetColumn(x.IsActive ? "(aktywny)" : "", 12, 1),
                          LajtIt.Web.Controls.Products.SetColumn(String.Format("{0:yyyy/MM/dd HH:mm}", x.InsertDate), 20, 1))

            });
            ddlMainPageGroup.DataSource = g;
            ddlMainPageGroup.DataBind();

            LajtIt.Web.Controls.Products.DecodeDropDownList(ddlMainPageGroup);
        }

        protected void lbtnReadShop_Click(object sender, EventArgs e)
        {
            rlProducts.Enabled = false;

            Bll.ShopHelper wcfSh = new Bll.ShopHelper();

            int[] shopProductsId = wcfSh.GetShopMainPage();

            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<Dal.ProductCatalogView> p = sh.GetProductCatalogByShopIds(shopProductsId);


            List<Product> products = p.Select(x => new Product()
            {
                GroupIsActive = true,
                GroupName = "Shop",
                Id = 0,
                ImageFullName = x.ImageFullName,
                IsActiveOnlineShop = x.IsActiveOnline,
                Name = x.Name,
                PriceBrutto = x.PriceBruttoShop.Value,
                Priority = x.ProductCatalogId,
                ProductCatalogId = x.ProductCatalogId,
                ShopMainPageGroupId = 0,
                ShopProductId = x.ShopProductId
            }).ToList();

            rlProducts.DataSource = products;
            rlProducts.DataBind();

            int[] foundProducts = p.Select(x => x.ShopProductId.Value).ToArray();
            int[] missingProducts = shopProductsId.Where(x => !foundProducts.Contains(x)).ToArray();

            pnMissingProducts.Visible = missingProducts.Count() > 0;
            gvMissingProducts.DataSource = missingProducts.Select(x => new { Id = x }).ToArray(); ;
            gvMissingProducts.DataBind();

            ddlMainPageGroup.Items.Insert(0, new ListItem("-- aktywny układ sklepowy --", "0"));
            ddlMainPageGroup.SelectedIndex = 0;

        }
        protected void btnProductAdd_Click(object sender, EventArgs e)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<string> message = new List<string>();

            bool result = sh.VerifyProductCode(Convert.ToInt32(ddlMainPageGroup.SelectedValue), txbProductCode.Text.Trim(), out message, UserName);
            if (message.Count > 0)
            {
                DisplayMessage(String.Format("Wykryto błędy podczas dodawania produktu: <ul>{0}</ul>",
                    String.Join("<li>- ", message)));
                return;
            }
            BindProducts();
        }
        protected void rlProducts_DeleteCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {

            int id = Int32.Parse(e.CommandArgument.ToString());

            Dal.ShopHelper sh = new Dal.ShopHelper();
            sh.SetShopMainPageDeleteItem(Convert.ToInt32(ddlMainPageGroup.SelectedValue), id);

            BindProducts();
        }
        protected void rlProducts_ItemReorder(object sender, AjaxControlToolkit.ReorderListItemReorderEventArgs e)
        {
            var newOrder = e.NewIndex;
            var oldOrder = e.OldIndex;


            Dal.ShopHelper sh = new Dal.ShopHelper();
            sh.SetShopMainPageChangeOrder(Convert.ToInt32(ddlMainPageGroup.SelectedValue), newOrder, oldOrder);

            BindProducts();
        }
    }
}