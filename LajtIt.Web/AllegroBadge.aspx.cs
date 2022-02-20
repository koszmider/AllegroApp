using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using System.IO;
using System.Drawing;
using System.Net;

namespace LajtIt.Web
{
    [Developer("A53BA290-48CD-4A8A-ADAF-616777BB708B")]
    public partial class AllegroBadge : LajtitPage
    {
        private string BadgeId { get { return Request.QueryString["id"].ToString(); } }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                BindSuppliers();
                BindBadge();
            }
        }

        private void BindSuppliers()
        {
            List<int> suppliersForShopType = Dal.DbHelper.ProductCatalog.GetSupplierShop(Dal.Helper.ShopType.Allegro);


            lbxSuppliers.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers()
                .Where(x => suppliersForShopType.Contains(x.SupplierId))
                .OrderBy(x => x.Name); 
            lbxSuppliers.DataBind();
        }

        private void BindBadge()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var b = Bll.AllegroRESTHelper.Badges.GetBadges()
                .Where(x => x.id == BadgeId).FirstOrDefault();

            litName.Text = b.name;
            litBadgeId.Text = b.id;


            Dal.AllegroBadge ab = Dal.AllegroRestHelper.GetAllegroBadge(b.id);

            if (ab != null)
            {
                ddlIsAvtive.SelectedValue = ab.IsActive ? "1" : "0";
                txbRebateFrom.Text = String.Format("{0}", ab.RebateFrom);
                txbRebateTo.Text = String.Format("{0}", ab.RebateTo);

                int[] selectedSupplierIds;
                if (ab.SupplierIds != "")
                    selectedSupplierIds = ab.SupplierIds
                        .Split(new char[] { ',' }).Select(x => Int32.Parse(x)).ToArray();
                else
                    selectedSupplierIds = new int[] { };
                foreach (ListItem li in lbxSuppliers.Items)
                    if (selectedSupplierIds.Contains(Int32.Parse(li.Value)))
                        li.Selected = true;

                MoveItems(lbxSuppliers, lbxSuppliersSelected);
            }
        }
        private void MoveItems(ListBox listFrom, ListBox listTo)
        {
            ListItem[] items = listFrom.Items.Cast<ListItem>().Where(x => x.Selected).ToArray();
            int[] sel = listFrom.GetSelectedIndices();
            foreach (int si in sel.OrderByDescending(x => x).ToArray())
                listFrom.Items.RemoveAt(si);

            listTo.Items.AddRange(items);
            ListItem[] i = listTo.Items.Cast<ListItem>().OrderBy(x => x.Text).ToArray();

            listTo.Items.Clear();

            listTo.Items.AddRange(i);
        }

 

        protected void btnSupplierDel_Click(object sender, EventArgs e)
        {
            MoveItems(lbxSuppliersSelected, lbxSuppliers);

        }

        protected void btnSupplierAdd_Click(object sender, EventArgs e)
        {
            MoveItems(lbxSuppliers, lbxSuppliersSelected);

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int[] supplierIds = lbxSuppliersSelected.Items.Cast<ListItem>().Select(x=>Int32.Parse(x.Value)).ToArray();
            Dal.AllegroBadge ab = new Dal.AllegroBadge()
            {
                BadgeId = litBadgeId.Text,
                BadgeName = litName.Text,
                InsertDate = DateTime.Now,
                IsActive = ddlIsAvtive.SelectedValue == "1",
                RebateFrom = Decimal.Parse(txbRebateFrom.Text.Trim()),
                RebateTo = Decimal.Parse(txbRebateTo.Text.Trim()),
                SupplierIds = String.Join(",", supplierIds)


            };

            Dal.AllegroRestHelper arh = new Dal.AllegroRestHelper();
            arh.SetAllegroBadge(ab);

            DisplayMessage("Zapisano zmiany");
        }
    }
}