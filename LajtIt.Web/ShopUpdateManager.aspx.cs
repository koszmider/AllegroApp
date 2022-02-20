using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("1141ec00-1b1f-45c7-af9b-ff953a8828d3")]
    public partial class ShopUpdateManager : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { 
                BindDropDowns();
                BindColumns();
            }
        }

        private void BindDropDowns()
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            ddlShopType.DataSource = sh.GetShopEngineTypes();
            ddlShopType.DataBind();

            ddlShopColumnType.DataSource = sh.GetShopColumnTypes();
            ddlShopColumnType.DataBind();

            ddlUpdateType.DataSource = sh.GetShopUpdateTypes();
            ddlUpdateType.DataBind();

            ddlColumnName.DataSource = sh.GetShopUpdateColumns();
            ddlColumnName.DataBind();
        }

        protected void ddlShopType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindColumns();
        }

        private void BindColumns()
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();
            int shopEngineTypeId = Int32.Parse(ddlShopType.SelectedValue);
            gvColumnType.DataSource = sh.GetShopColumnTypeShopType(shopEngineTypeId);
            gvColumnType.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dal.ShopHelper sh = new Dal.ShopHelper();

            string[] columnName = ddlColumnName.SelectedItem.Text.Split(new char[] { '.' });
            Dal.ShopColumnTypeShopType st = new Dal.ShopColumnTypeShopType()
            {
                ColumnName = columnName[1],
                ShopColumnTypeId = Int32.Parse(ddlShopColumnType.SelectedValue),
                ShopEngineTypeId = Int32.Parse(ddlShopType.SelectedValue),
                TableName = columnName[0],
                UpdateTypeId = Int32.Parse(ddlUpdateType.SelectedValue)
            };

            try
            {
                sh.SetShopColumnTypeShopType(st);

                BindColumns();

                DisplayMessage("Dodano");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if(ex.Number == 2601 || ex.Number == 2627)
                {

                    DisplayMessage("Wpis już istnieje");
                }
            }

        }
    }
}