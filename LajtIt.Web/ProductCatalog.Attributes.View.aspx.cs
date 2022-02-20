using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("3f06378c-e823-4196-8268-9aa16b17c10c")]
    public partial class ProductCatalog_Attributes_View : LajtitPage
    {
        List<Dal.ProductCatalogAttributesView> attributes;
        protected void Page_Load(object sender, EventArgs e)
        {
            BindAttributes();  
        }

        private void BindAttributes()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ProductFileImportHelper pfih = new Dal.ProductFileImportHelper();
            gvFileSpec.DataSource = pfih.GetProductCatalogFileSpecification();
            gvFileSpec.DataBind();

            attributes = pch.GetProductCatalogAttributesView();

            var g = attributes.Select(x =>
                new  
                {
                    AttributeGroupId = x.AttributeGroupId,
                    Name = x.GroupName,
                    ExcelColumnName = x.GroupFieldName
                }

                )
                .Distinct()
                .Select(x =>
                new Dal.ProductCatalogAttributeGroup()
                {
                    AttributeGroupId = x.AttributeGroupId,
                    Name = x.Name
                }

                )
                .Distinct()
                .ToList();
            rpGroups.DataSource = g;
            rpGroups.DataBind();

        }

        protected void rpGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblGroup = e.Item.FindControl("lblGroup") as Label; 
                Repeater rpAttributes = e.Item.FindControl("rpAttributes") as Repeater;

                Dal.ProductCatalogAttributeGroup a = e.Item.DataItem as Dal.ProductCatalogAttributeGroup;

                lblGroup.Text = a.Name; 
                var att = attributes.Where(x => x.AttributeGroupId == a.AttributeGroupId).ToList();
                rpAttributes.DataSource = att;
                rpAttributes.DataBind();
            }
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.HtmlControls.HtmlControl trRow = e.Item.FindControl("trRow") as System.Web.UI.HtmlControls.HtmlControl;
                trRow.Style.Add("background-color", "silver");

            }
        }

        protected void rpAttributes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink hlAttribute = e.Item.FindControl("hlAttribute") as HyperLink;
                Label lblCode = e.Item.FindControl("lblCode") as Label;
                Label lblExcelColumnName = e.Item.FindControl("lblExcelColumnName") as Label;



                Dal.ProductCatalogAttributesView a = e.Item.DataItem as Dal.ProductCatalogAttributesView;

                hlAttribute.Text = a.AttributeName;
                hlAttribute.NavigateUrl = String.Format(hlAttribute.NavigateUrl, a.AttributeId);

                if (a.AttributeFieldName!=null)
                    hlAttribute.Text = String.Format("{0}/<b>{1}</b>", a.AttributeName, a.AttributeFieldName);
                lblCode.Text = a.Code;

            }

        }
    }
}