using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("21095a51-5c55-4d0f-80b8-c558994dbdfd")]
    public partial class ProductHistory : LajtitPage
    {
        public int ProductCatalogId { get { return Convert.ToInt32(Request.QueryString["id"]); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindColumns();
                BindProductCatalog();
            }
        }

        private void BindColumns()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            ddlColumnName.DataSource = pch.GetTableLogHistory(ProductCatalogId, new string[] { "ProductCatalog" })
                .Select(x => new { ColumnName = x.ColumnName }).Distinct().ToList();
            ddlColumnName.DataBind();
            ddlColumnName.Items.Insert(0, "");
        }

        private void BindProductCatalog()
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            gvHistory.DataSource = pch.GetTableLogHistory(ProductCatalogId, new string[] { "ProductCatalog" }, ddlColumnName.SelectedValue);
            gvHistory.DataBind();

        }

        protected void ddlColumnName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProductCatalog();
        }
        protected void gvHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.TableLogView tl = e.Row.DataItem as Dal.TableLogView;

                Label lblValue = e.Row.FindControl("lblValue") as Label;


                if (tl.IntValue.HasValue)
                    lblValue.Text = tl.IntValue.ToString();

                if (tl.DecimalValue.HasValue)
                    lblValue.Text = tl.DecimalValue.ToString();


                if (tl.StringValue != null)
                    lblValue.Text = tl.StringValue;


            }
        }
    }
}