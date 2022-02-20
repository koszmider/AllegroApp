using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;

namespace LajtIt.Web
{
    [Developer("b036db1c-6e3d-476e-ae37-537f1b195997")]
    public partial class ShopCategory : LajtitPage
    {
        List<Dal.ShopCategoryView> categories;
        string selectedCategoryId = "0";
        TreeNode selectedTreeNode;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindTree();
        }

        List<int> catIds = new List<int>();
        private void BindTree()
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
           
            categories = sh.GetCategories((int)Dal.Helper.Shop.Lajtitpl);


            if (txbSearch.Text.Trim() != "")
            { 
                var catSelected = categories.Where(x => x.Name.StartsWith(txbSearch.Text.Trim())).ToList();

                List<int> c = catSelected.Where(x => x.CategoryParentId.HasValue).Select(x => x.CategoryParentId.Value).Distinct().ToList();
                catIds.AddRange(GetParents(c));

                catSelected.AddRange(categories.Where(x => catIds.Contains(x.CategoryId)).ToList());

                categories = catSelected.Distinct().ToList();
            }


            tvCategory.Nodes.Clear();

            PopulateTreeView(null, null);
            if(selectedTreeNode!=null)
            ExpandToRoot(selectedTreeNode);
        }

        private List<int> GetParents(List<int> cIds)
        {
            catIds.AddRange(cIds);

            var catSelected = categories.Where(x => cIds.Contains(x.CategoryId)).ToList();

            if (catSelected.Count == 0)
                return cIds;

            List<int> c = catSelected.Where(x => x.CategoryParentId.HasValue).Select(x => x.CategoryParentId.Value).Distinct().ToList();

            if (catSelected.Count == 0)
                return new List<int>();
            else
                return GetParents(c);

        }

        private void PopulateTreeView(int? parentId, TreeNode treeNode)
        {
            IEnumerable<Dal.ShopCategoryView> cat;
            if (parentId.HasValue)
                cat = categories.Where(x => x.CategoryParentId == parentId.Value);
            else
                cat = categories.Where(x => x.CategoryParentId is null);

            cat = cat.OrderBy(x => x.CategoryOrder).ToList();

            foreach (Dal.ShopCategoryView c in cat)
            {
                TreeNode child = new TreeNode
                {
                    Text = c.IsActive? c.Name: "<span style='color:grey'>"+c.Name+"</span>",
                    Value = c.CategoryId.ToString(),
                    ImageUrl= GetImageUrl(c),
                     
                };
                if (parentId == null)
                {
                    tvCategory.Nodes.Add(child);

                    PopulateTreeView(int.Parse(child.Value), child);
                }
                else
                {
                    treeNode.ChildNodes.Add(child);
                    PopulateTreeView(int.Parse(child.Value), child);
                }
                if (c.CategoryId == selectedCategoryId)
                {
                    child.Selected = true;
                    selectedTreeNode = child;
            }
            }
        }

        private string GetImageUrl(ShopCategoryView c)
        {
            if (c.IsPublished)
                return "/Images/ok.jpg";
            else
                return "/Images/false.jpg";

        }

        protected void btnDeleteNotExisting_Click(object sender, EventArgs e)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            List<Dal.ShopCategoryView> cats = sh.GetCategories((int)Dal.Helper.Shop.Lajtitpl);
            List<int> notex = new List<int>();
            Bll.ShopHelper shb = new Bll.ShopHelper();
            int count = 0;
            foreach (Dal.ShopCategoryView c in cats)
            {
                Bll.ShopCategory category = shb.GetCategory(c.CategoryId);

                if (category == null)
                {
                    notex.Add(c.CategoryId);
                    //sh.SetCategoryDelete(c.CategoryId);
                    count++;
                }
            }

            DisplayMessage(String.Format("Usunięto {0} kategrii {1}", count, string.Join(",", notex.ToArray())));
            BindTree();

        }
 

        protected void tvCategory_SelectedNodeChanged(object sender, EventArgs e)
        {
            int categoryId = Convert.ToInt32(tvCategory.SelectedValue);
            Bll.ShopHelper sh = new Bll.ShopHelper();
            Bll.ShopCategory category = sh.GetCategory(categoryId);

            if (category == null)
            {
                lblInfo.Visible = true;
                pnCategory.Visible = false;
            }
            else
            {
                BindCategory(categoryId, category);

            }
            var tree = (TreeView)sender;
            foreach (TreeNode node in tree.Nodes)
            {
                node.CollapseAll();
            }
            ExpandToRoot(tree.SelectedNode);
             
        }

        private void BindCategory(int categoryId, Bll.ShopCategory category)
        {
            Dal.ShopHelper shd = new Dal.ShopHelper();
            var c = category.translations["pl_PL"];
            lblInfo.Visible = false;
            pnCategory.Visible = true;
            lblName.Text = c.name;
            hlLink.NavigateUrl = hlLink.Text = c.permalink;
            chbIsActive.Checked = c.active == 1;


            Dal.ShopCategory sc = shd.GetCategory(categoryId);


            txbDesc.Text = sc.Description;
            txbName.Text = sc.Name;
            txbSeoDesc.Text = sc.SeoDescription;
            txbSeoKeywords.Text = sc.SeoKeywords;
            txbSeoTitle.Text = sc.SeoTitle;
            txbUrl.Text = sc.Permalink;

            imgFalse.Visible = !sc.IsPublished;
            imgOK.Visible = sc.IsPublished;
        }

        private void ExpandToRoot(TreeNode node)
        {
            node.Expand();
            if (node.Parent != null)
            {
                ExpandToRoot(node.Parent);
            }
        }

        protected void txbSearch_TextChanged(object sender, EventArgs e)
        {
            BindTree();
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            Bll.ShopHelper sh = new Bll.ShopHelper();
            bool result = sh.SetCategoryDelete(Convert.ToInt32(tvCategory.SelectedValue));

            if(result)
            {
                DisplayMessage("Kategoria została usunięta");
                BindTree();
            }
            else
                DisplayMessage("Kategoria nie została usunięta. Sprawdź czy produkty nie są pod nią podpięte.");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            selectedCategoryId = tvCategory.SelectedValue;
            Dal.ShopCategory sc = new Dal.ShopCategory()
            {
                ShopCategoryId = selectedCategoryId,
                Description = txbDesc.Text.Trim(),
                IsActive = chbIsActive.Checked,
                IsPublished = false,
                Name = txbName.Text.Trim(),
                Permalink = GetStringOrNull(txbUrl.Text),
                SeoDescription = GetStringOrNull(txbSeoDesc.Text),
                SeoKeywords = GetStringOrNull(txbSeoKeywords.Text),
                SeoTitle = GetStringOrNull(txbSeoTitle.Text),
                ShopId=(int)Dal.Helper.Shop.Lajtitpl
            };
            int shopId = (int)Dal.Helper.Shop.Lajtitpl;
            Dal.ShopHelper sh = new Dal.ShopHelper();
            bool result = sh.SetCategory(sc, shopId);

            if (result)
            {
                DisplayMessage("Kategoria została zapisana");
                BindTree();
            }
            else
                DisplayMessage("Błąd podczas zapisywania");

            //tvCategory.SelectedNode = categoryId.ToString();

            Bll.ShopHelper shb = new Bll.ShopHelper();
            Bll.ShopCategory category = shb.GetCategory(selectedCategoryId);
            BindCategory(selectedCategoryId, category);

        }

        private string GetStringOrNull(string text)
        {
            if (text.Trim() == "")
                return null;
            else
                return text.Trim();

        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            selectedCategoryId = Convert.ToInt32(tvCategory.SelectedValue);
            Bll.ShopHelper sh = new Bll.ShopHelper();
            bool result = sh.SetCategory(selectedCategoryId);

            if (result)
            {
                DisplayMessage("Kategoria została zapisana");
                BindTree();
            }
            else
                DisplayMessage("Błąd podczas zapisywania");

            Bll.ShopHelper shb = new Bll.ShopHelper();
            Bll.ShopCategory category = shb.GetCategory(selectedCategoryId);
            BindCategory(selectedCategoryId, category);
        }
    }
}