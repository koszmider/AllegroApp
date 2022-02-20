using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using System.Collections.Generic;
using System.Linq;

namespace LajtIt.Web
{
    [Developer("d78f1def-15af-4c1c-b2b6-3a9819d1e769")]
    public partial class ProductCatalogOnAllegro : LajtitPage
    {
        List<Dal.ProductCatalogAllegroActiveItem> items;
        int startDaysColumn = 3;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindSuppliers();
                BindImports();
                BindCatalog();
            }
        }

        private void BindImports()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            ddlImport.DataSource = oh.GetImports();
            ddlImport.DataBind();
        }


        private void BindSuppliers()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            ddlSearchSupplier.DataSource = oh.GetSuppliers();
            ddlSearchSupplier.DataBind();

         //  ddlSearchSupplier.SelectedValue = "3";
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindCatalog();
        }


        protected void gvProductCatalog_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < 30; i++)
                {
                    int cell = startDaysColumn + i;
                    //TimeSpan ts = new TimeSpan(DateTime.Now.AddDays(i));
                    e.Row.Cells[cell].Text = String.Format("{0}<br>{1}.&nbsp;{2}", i, DateTime.Now.AddDays(i).Day, DateTime.Now.AddDays(i).DayOfWeek.ToString().Substring(0, 3) );
                    e.Row.Cells[cell].Width = 120;
                }


            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region
                for (int i = 0; i < 30; i++)
                {
                    int cell = startDaysColumn + i;
                    //TimeSpan ts = new TimeSpan(DateTime.Now.AddDays(i));
                    if(DateTime.Now.AddDays(i).DayOfWeek== DayOfWeek.Sunday)
                        e.Row.Cells[cell].BackColor = System.Drawing.Color.LightPink ;
                }


                Dal.ProductCatalogForAllegroGetResult pc = e.Row.DataItem as Dal.ProductCatalogForAllegroGetResult;

                //HyperLink hlCalcuator = e.Row.FindControl("hlCalcuator") as HyperLink;
                // hlCalcuator.NavigateUrl = String.Format("/ProductCalculator.aspx?id={0}", pc.ProductCatalogId);
                Image imgImage = e.Row.FindControl("imgImage") as Image;

                HyperLink hlProduct = e.Row.FindControl("hlProduct") as HyperLink;
                HyperLink hlProductAllegro = e.Row.FindControl("hlProductAllegro") as HyperLink;
                HyperLink hlPreview = e.Row.FindControl("hlPreview") as HyperLink;

                Label lblCode = e.Row.FindControl("lblCode") as Label; 

                hlPreview.NavigateUrl = String.Format(hlPreview.NavigateUrl, pc.ProductCatalogId, pc.SupplierId);
                hlProduct.NavigateUrl = String.Format(hlProduct.NavigateUrl, pc.ProductCatalogId);
                hlProductAllegro.NavigateUrl = String.Format(hlProductAllegro.NavigateUrl, pc.ProductCatalogId);
                hlProduct.Text = pc.Name;
                hlProductAllegro.Text = pc.AllegroName;


                if (!String.IsNullOrEmpty(pc.ImageFullName))
                    imgImage.ImageUrl = String.Format("/images/productcatalog/{0}", pc.ImageFullName);
                else
                    imgImage.Visible = false;
                if (!pc.IsReady.Value)
                    e.Row.Style.Add("background-color", "silver");


                 
                lblCode.Text = pc.Code;

                #endregion
                List<Dal.ProductCatalogAllegroActiveItem> activeItems = items
                    .Where(x => x.ProductCatalogId == pc.ProductCatalogId).ToList();


                foreach (Dal.ProductCatalogAllegroActiveItem i in activeItems)
                {
                    DateTime end = i.EndingDateTime.Value;
                    DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, end.Hour, end.Minute, end.Second);
                    TimeSpan ts = end - now;

                    int cell = startDaysColumn + (ts.Days < 0 ? 30 : ts.Days);
                     
                    if (end.Year == 1970)
                        cell = startDaysColumn;

                    if(e.Row.Cells.Count<=cell)
                        continue;

                    //if(end.Year == 1970)
                    //    cell = startDaysColumn;

                    string tmp = e.Row.Cells[cell].Text;

                    tmp += String.Format("<div style='background-color:{0}'>{2}<br><a href='http://allegro.pl/show_item.php?item={1}' target='_blank'>{1}&nbsp;({3})</a></div>",
                        i.IsPromoted.Value ? "silver" : "white", i.ItemId, i.UserName, i.BidCount);



                    e.Row.Cells[cell].Text = tmp;
                }




            }
        }


        private void BindCatalog()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            items = oh.GetProductCatalogAllegroActiveItems();
            bool filter = false;

            if (ddlSoldItems.SelectedIndex == 1)
                {items = items.Where(x => x.BidCount > 0).ToList(); filter = true;}
            if (ddlSoldItems.SelectedIndex == 2)
                {items = items.Where(x => x.BidCount == 0).ToList(); filter = true;}

            if (ddlPromotion.SelectedIndex == 1)
                {items = items.Where(x => x.IsPromoted.Value == true).ToList(); filter = true;}
            if (ddlPromotion.SelectedIndex == 2)
                {items = items.Where(x => x.IsPromoted.Value == false).ToList(); filter = true;}

            if(chbUnlimited.Checked )
            { items = items.Where(x => x.EndingDateTime.HasValue && x.EndingDateTime.Value.Year == 1970).ToList(); filter = true; }


            string name = txbSearchName.Text.Trim();
            int supplierId = Convert.ToInt32(ddlSearchSupplier.SelectedValue);
            bool? isReady = null;
            bool? notCreated = null;
            int importId = Convert.ToInt32(ddlImport.SelectedValue);
            if (chbReady.Checked)
                isReady = true;
            if (chbAllegroNotCreated.Checked)
                notCreated = true;
            List<Dal.ProductCatalogForAllegroGetResult> products = oh.GetProductCatalogForAllegro(name, supplierId, isReady, notCreated, importId);


            if (filter)
            {
                int [] productCatalogIds = items.Select(x => x.ProductCatalogId).Distinct().ToArray();
                products = products.Where(x => productCatalogIds.Contains(x.ProductCatalogId)).ToList();

            }
            gvProductCatalog.DataSource = products.OrderBy(x=>x.Code);
            gvProductCatalog.DataBind();


        }



    }
}