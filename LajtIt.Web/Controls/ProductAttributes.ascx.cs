using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;
using System.Web.UI.HtmlControls;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace LajtIt.Web.Controls
{
    public partial class ProductAttributes : LajtitControl
    {
        Dictionary<int, bool> attributeGroups = new Dictionary<int, bool>();
        Dictionary<int, bool> attributes = new Dictionary<int, bool>();

        private int schemaId = 0;
        List<Dal.ProductCatalogAttributesForProductResult> groupsForGrouping;
        const bool enableAdding = true;
        // const bool isRequired = true;
        int groupingId;

        #region Public methods
        public int ProductCatalogId
        {
            get
            {

                if (ViewState["ProductCatalogId"] != null)
                    return Convert.ToInt32(ViewState["ProductCatalogId"]);

                return Convert.ToInt32(Request.QueryString["id"]);
            }
            set { ViewState["ProductCatalogId"] = value; }
        }
        public bool EnableAdding
        {
            get
            {

                if (ViewState["EnableAdding"] != null)
                    return Convert.ToBoolean(ViewState["EnableAdding"]);

                return enableAdding;
            }
            set { ViewState["EnableAdding"] = value; }
        }

        /// <summary>
        /// Pokazuj opcje grupowania
        /// </summary>
        public bool EnableBindGrouping
        {
            get
            {

                if (ViewState["BindGrouping"] != null)
                    return Convert.ToBoolean(ViewState["BindGrouping"]);

                return false;
            }
            set { ViewState["BindGrouping"] = value; }
        }
        /// <summary>
        /// wymusza wybór opcji grupowania
        /// </summary>
        public bool EnableBindEmptyGrouping
        {
            get
            {

                if (ViewState["EnableBindEmptyGrouping"] != null)
                    return Convert.ToBoolean(ViewState["EnableBindEmptyGrouping"]);

                return false;
            }
            set { ViewState["EnableBindEmptyGrouping"] = value; }
        }

        public void AttributesSave(bool allowRemove)
        {
            AttributesSave(ProductCatalogId, allowRemove); //, false, null);
            BindAttributeGroups(); 
        }
        public bool AttributesSave(int productCatalogId, bool allowRemove)
        {
            List<ProductCatalogAttributeToProduct> attributesToSave = GetSelectedAttributes(productCatalogId, allowRemove);


            int[] allGroupIds = attributeGroups.Select(x => x.Key).ToArray();

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogAttributes(productCatalogId, attributesToSave, allGroupIds);//, clearBeforeAdd, groupIdToClear);

            return true;// (attributeGroups.Where(x => x.Value).Count() > 0||attributes.Where(x => x.Value).Count() > 0);
        }
        public void BindAttributeGroups(int[] attributeGroupIds)
        { 
            BindAttributeGroupsInternal(attributeGroupIds);
        }

        private void BindAttributeGroupsInternal(int[] attributeGroupIds)
        {

            if (!CheckForGrouping() && EnableBindGrouping)
                return;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();


            if(!EnableBindGrouping && (attributeGroupIds ==null || attributeGroupIds.Length==0))
            {
                rpAttributeGroup.DataSource = pch.GetProductCatalogAttributeGroups(ProductCatalogId, null, schemaId);
                rpAttributeGroup.DataBind();

                rpAttributeGroup.Visible = true;
                return;
            }


            // if (EnableBindGrouping)
            // {
            groupingId = pch.GetProductCatalogAttributeDefaultGrouping(ProductCatalogId);

            ddlGrouping.SelectedIndex = ddlGrouping.Items.IndexOf(ddlGrouping.Items.FindByValue(groupingId.ToString()));
            //  }



            List<Dal.ProductCatalogAttributeGroupForProductResult> groups = pch.GetProductCatalogAttributeGroups(ProductCatalogId, groupingId, schemaId)
                //.OrderByDescending(x=>x.IsRequired)
                //.ThenBy(x => x.AttributeExists)
                //.ThenBy(x => x.AttributeGroupId)
                .ToList();



            if (attributeGroupIds != null)
                groups = groups.Where(x => attributeGroupIds.Contains(x.AttributeGroupId)).ToList();

            List<Dal.ProductCatalogAttributeGroupForProductResult> groupsResult = new List<ProductCatalogAttributeGroupForProductResult>();

            if (chbAllegro.Checked)
                groupsResult.AddRange(groups.Where(x => x.IsRequired));
            int[] existing = groupsResult.Select(x => x.AttributeGroupId).ToArray();
            if (chbRequried.Checked)
                groupsResult.AddRange(groups.Where(x => x.GroupRequired.Value && !existing.Contains(x.AttributeGroupId)));
            existing = groupsResult.Select(x => x.AttributeGroupId).ToArray();
            if (chbOption.Checked)
                groupsResult.AddRange(groups.Where(x => !x.GroupRequired.Value && !existing.Contains(x.AttributeGroupId)));



            if (groupsResult.Count() == 0)
            {
                groupsResult.AddRange(groups.Where(x => !x.GroupRequired.Value && !existing.Contains(x.AttributeGroupId)));
                chbAllegro.Checked = chbRequried.Checked = false;
                chbOption.Checked = true;
            }
            groupsResult = groupsResult
                .OrderBy(x => x.Order)
                .ThenBy(x => x.AllegroOrder)
                .ToList();

            rpAttributeGroup.DataSource = groupsResult;
            rpAttributeGroup.DataBind();

            rpAttributeGroup.Visible = true;


        }


   
        public void BindAttributeGroups()
        {

            if (!EnableBindGrouping)
            {
                pnAttributeTypes.Visible = false;
                pnEmpty.Visible = false;
                ddlGrouping.Visible = false;
            }
            BindAttributeGroupsInternal(null);
        }
        #endregion

        #region Events
        protected void ddlGroupingEmpty_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEmptyGrouping();
        }


        protected void Page_Load(object sender, EventArgs e)
        { 
        }

        
        protected void rpAttributeGroup_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Dal.ProductCatalogAttributeGroupForProductResult group = e.Item.DataItem as Dal.ProductCatalogAttributeGroupForProductResult;

                HyperLink lblAttributeGroup = e.Item.FindControl("lblAttributeGroup") as HyperLink;
                Label lblShopAttributeId = e.Item.FindControl("lblShopAttributeId") as Label;
                CheckBoxList chblAttributes = e.Item.FindControl("chblAttributes") as CheckBoxList;
                RadioButtonList rblAttributes = e.Item.FindControl("rblAttributes") as RadioButtonList;
                LinkButton lbtnAttributeNewAdd = e.Item.FindControl("lbtnAttributeNewAdd") as LinkButton;
                DropDownList ddlAttributeDefault = e.Item.FindControl("ddlAttributeDefault") as DropDownList; 
                TextBox txbAttributeNew = e.Item.FindControl("txbAttributeNew") as TextBox;
                Repeater rpDecimalValues = e.Item.FindControl("rpDecimalValues") as Repeater;
                HiddenField hidGroupTypeId = e.Item.FindControl("hidGroupTypeId") as HiddenField;
                HtmlControl tdAddNew = e.Item.FindControl("tdAddNew") as HtmlControl;
                HyperLink hlLink = e.Item.FindControl("hlLink") as HyperLink;
                HyperLink hlEmpty = e.Item.FindControl("hlEmpty") as HyperLink;
                ImageButton ibtnDelete = e.Item.FindControl("ibtnDelete") as ImageButton;
                HtmlControl tr = e.Item.FindControl("tr") as HtmlControl;
                //HiddenField hidIsRequired = e.Item.FindControl("hidIsRequired") as HiddenField;
                Image imgAllegro = e.Item.FindControl("imgAllegro") as Image;
                Label lblAttributeGroupId = e.Item.FindControl("lblAttributeGroupId") as Label;
                lblAttributeGroupId.Text = group.AttributeGroupId.ToString();

                imgAllegro.Visible = group.IsRequired;

                //hidIsRequired.Value = (group.IsRequired || group.AttributeExists==0).ToString();
                tdAddNew.Visible = EnableAdding;
                lblAttributeGroup.Text = group.Name;
                lblAttributeGroup.NavigateUrl = String.Format(lblAttributeGroup.NavigateUrl, group.AttributeGroupId);

                hidGroupTypeId.Value = group.AttributeGroupTypeId.ToString();

                 
                lbtnAttributeNewAdd.CommandArgument = group.AttributeGroupId.ToString();

                int cols = EnableAdding ? 9 : 6;
                bool isReady = false;
                switch (group.AttributeGroupTypeId)
                {
                    case 1:
                        rblAttributes.Visible = true;
                        BindAttributes(group.AttributeGroupId, rblAttributes.Items, ddlAttributeDefault, "");
                        rblAttributes.RepeatColumns = cols;
                        ibtnDelete.Visible = true;
                        isReady = rblAttributes.Items.Cast<ListItem>().Where(x => x.Selected).Count() > 0;

                        rblAttributes.AutoPostBack = EnableAdding;
                        hlEmpty.Visible = rblAttributes.Items.Count == 0;
                        break;

                    case 2:
                        chblAttributes.Visible = true;
                        BindAttributes(group.AttributeGroupId, chblAttributes.Items, ddlAttributeDefault, "");
                        chblAttributes.RepeatColumns = cols;
                        isReady = chblAttributes.Items.Cast<ListItem>().Where(x => x.Selected).Count() > 0;
                        chblAttributes.AutoPostBack = EnableAdding;
                        hlEmpty.Visible = chblAttributes.Items.Count == 0;
                        break;
                    case 3:
                        /*
                        txbAttributeValue.Visible = true;
                        bool hasAttribute =  BindAttributes(group.AttributeGroupId, txbAttributeValue, lblAttributeName, "");
                        if (hasAttribute)
                            lbtnAttributeNewAdd.Visible = txbAttributeNew.Visible = false;
                        */
                        BindAttributes(group.AttributeGroupId, rpDecimalValues, ddlAttributeDefault, "");
                        break;
                }
                if (group.GroupRequired.Value)
                    tr.Style.Add("background-color", "lightgray");

                //attributeGroups.Add(group.AttributeGroupId, group.IsRequired && !isReady);
            }
        }


        protected void btnEmpty_Click(object sender, EventArgs e)
        {

            Dal.ProductCatalogHelper pch = new ProductCatalogHelper();
            pch.SetProductCatalogAttribute(new ProductCatalogAttributeToProduct()
            {
                AttributeId = Int32.Parse(rblAttributeEmptyGroups.SelectedValue),
                IsDefault = true,
                ProductCatalogId = ProductCatalogId
            });
            EnableBindEmptyGrouping = false;

            BindAttributeGroups();


        }
        protected void lbtnAttributeNewAdd_Click(object sender, EventArgs e)
        {
            Dal.ProductCatalogAttribute attribute = new Dal.ProductCatalogAttribute();
            attribute.AttributeGroupId = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            attribute.Name = ((TextBox)((LinkButton)sender).Parent.FindControl("txbAttributeNew")).Text.Trim();
            attribute.UpdateShopConfiguration = true;

            int attributeId = 0;
            bool result = Dal.DbHelper.ProductCatalog.SetProductCatalogAttribute(attribute, ref attributeId);


            if (!result)

                DisplayMessage(String.Format("Wartość dla tej grupy już istnieje (może być ukryta dla danej konfiguracji). <a href='ProductCatalog.Attribute.aspx?id={0}'>kliknij tutaj i sprawdź jej konfigurację</a>", 
                    attributeId));
            else
                BindAttributeGroups();
        }
        protected void chblAttributes_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList chblAttributes = sender as CheckBoxList;
            RadioButtonList rblAttributes = sender as RadioButtonList;
            DropDownList ddlAttributeDefault = ((HtmlTableCell)((System.Web.UI.Control)sender).Parent.Parent.Parent)
                .FindControl("ddlAttributeDefault") as DropDownList; ;

            string defaulValue = ddlAttributeDefault.SelectedValue;

            BindDefaultAttributes((chblAttributes == null ? rblAttributes.Items : chblAttributes.Items),
                ddlAttributeDefault, defaulValue);


        }

        protected void rpDecimalValues_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Dal.ProductCatalogAttributesForProductResult p = e.Item.DataItem as Dal.ProductCatalogAttributesForProductResult;

                Label lblAttributeName = e.Item.FindControl("lblAttributeName") as Label;
                TextBox txbAttributeValue = e.Item.FindControl("txbAttributeValue") as TextBox;
                HiddenField hidAttributeId = e.Item.FindControl("hidAttributeId") as HiddenField;
                HiddenField hidAttributeTypeId = e.Item.FindControl("hidAttributeTypeId") as HiddenField;
                RegularExpressionValidator revDecimal = e.Item.FindControl("revDecimal") as RegularExpressionValidator;
                HtmlControl tr = e.Item.FindControl("tr") as HtmlControl;
                //HiddenField hidIsRequired = e.Item.FindControl("hidIsRequired") as HiddenField;

                hidAttributeId.Value = p.AttributeId.ToString();
                hidAttributeTypeId.Value = p.AttributeTypeId.HasValue ? p.AttributeTypeId.ToString() : "";
                //hidIsRequired.Value = p.IsRequired.ToString();
                lblAttributeName.Text = p.AttributeName;

                switch (p.AttributeTypeId)
                {
                    case 1:
                        if (p != null && p.DecimalValue != null)
                        {
                            txbAttributeValue.Text = String.Format("{0:0.00}", p.DecimalValue);
                        }
                        revDecimal.Enabled = true;
                        break;
                    case 2:
                        txbAttributeValue.Text = p.StringValue;
                        txbAttributeValue.TextMode = TextBoxMode.MultiLine;
                        txbAttributeValue.Rows = 3;

                        break;
                }

                if (/*IsRequired &&*/ p.IsRequired && txbAttributeValue.Text == "")// && !isReady)
                    tr.Style.Add("background-color", "lightpink");


            }
        }

        protected void ibtnDelete_Click(object sender, ImageClickEventArgs e)
        {

            CheckBoxList chblAttributes = sender as CheckBoxList;
            RadioButtonList rblAttributes = ((HtmlTableCell)((System.Web.UI.Control)sender).Parent.Parent.Parent)
                .FindControl("rblAttributes") as RadioButtonList; ;

            foreach (ListItem item in rblAttributes.Items)
                item.Selected = false;



        }

        protected void chbRequried_CheckedChanged(object sender, EventArgs e)
        {
            if (ProductCatalogId != 0)
                AttributesSave(true);

            BindAttributeGroups();
        }

        protected void ddlGrouping_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableBindEmptyGrouping = true;
            EnableBindGrouping = true;
            CheckForGrouping();

            rpAttributeGroup.Visible = false;



        }
        #endregion

        #region Private

        private bool CheckForGrouping()
        {
            if (ddlGrouping.Items.Count == 0)
                BindGrouping();

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
           
             
           groupsForGrouping =
                pch.GetProductCatalogAttributes(6, ProductCatalogId, null)
                .Where(x=>x.ProductCatalogId.HasValue)
                .OrderBy(x => x.AttributeName).ToList();


            // niewybrany rodzaj produktu lub zmiana celowa grupy
            if ((groupsForGrouping.Count()==0 || EnableBindEmptyGrouping) && EnableBindGrouping)
            {
                pnEmpty.Visible = true;
                pnAttributeTypes.Visible = false;

                BindEmptyGrouping();
            }
            else
            {
                pnAttributeTypes.Visible = EnableBindGrouping;
                pnEmpty.Visible = false;
            }


            return groupsForGrouping.Count() > 0;
        }
        private void BindEmptyGrouping()
        {
            Dal.ProductCatalogHelper pch = new ProductCatalogHelper();

            if(ddlGroupingEmpty.SelectedValue!="")
            { 
            List<Dal.ProductCatalogAttribute> attributes = pch.GetProductCatalogAttributeGroupingAttributes(Int32.Parse(ddlGroupingEmpty.SelectedValue))
                    .Where(x=>x.AttributeGroupId==6)
                    .ToList();

            rblAttributeEmptyGroups.DataSource = attributes;
            rblAttributeEmptyGroups.DataBind();
        }
        }
        private void BindGrouping()
        {
            Dal.ProductCatalogHelper pch = new ProductCatalogHelper();

            List<Dal.ProductCatalogAttributeGrouping> grouping = pch.GetProductCatalogAttributeGroupings(2);

            ddlGrouping.DataSource = grouping;
            ddlGrouping.DataBind();
            ddlGroupingEmpty.DataSource = grouping;
            ddlGroupingEmpty.DataBind();
        }

        private List<ProductCatalogAttributeToProduct> GetSelectedAttributes(int productCatalogId, bool allowRemove)
        {
            List<Dal.ProductCatalogAttributeToProduct> attributes = new List<Dal.ProductCatalogAttributeToProduct>();
            foreach (RepeaterItem item in rpAttributeGroup.Items)
            {
                CheckBoxList chblAttributes = item.FindControl("chblAttributes") as CheckBoxList;
                RadioButtonList rblAttributes = item.FindControl("rblAttributes") as RadioButtonList;
                DropDownList ddlAttributeDefault = item.FindControl("ddlAttributeDefault") as DropDownList;
                Label lblAttributeGroupId = item.FindControl("lblAttributeGroupId") as Label;
                HiddenField hidGroupTypeId = item.FindControl("hidGroupTypeId") as HiddenField;
                //HiddenField hidIsRequired = item.FindControl("hidIsRequired") as HiddenField; 
                Repeater rpDecimalValues = item.FindControl("rpDecimalValues") as Repeater;


                bool isReady = true;
                switch (hidGroupTypeId.Value)
                {
                    case "1":
                        attributes.AddRange(GetSelectedAttributes(productCatalogId, rblAttributes.Items, ddlAttributeDefault));
                        isReady = rblAttributes.Items.Cast<ListItem>().Where(x => x.Selected).Count() > 0;
                        break;
                    case "2":
                        attributes.AddRange(GetSelectedAttributes(productCatalogId, chblAttributes.Items, ddlAttributeDefault));
                        isReady = chblAttributes.Items.Cast<ListItem>().Where(x => x.Selected).Count() > 0;
                        break;
                    case "3":
                        var a = GetAttributesFromRepeater(productCatalogId, rpDecimalValues);
                        if (a.Count() > 0)
                        {
                            attributes.AddRange(a);
                        }
                        else
                            isReady = false ;
                        break;
                }
                //if(IsRequired)
                {
                    //  if (Boolean.Parse(hidIsRequired.Value) && !isReady)
                    if (!attributeGroups.ContainsKey(Int32.Parse(lblAttributeGroupId.Text)) && (isReady || allowRemove))
                        attributeGroups.Add(Int32.Parse(lblAttributeGroupId.Text), true);
                    // else
                    //      attributeGroups.Add(Int32.Parse(lblAttributeGroupId.Text), false);
                }
            }

            return attributes;
        }

        private static List<Dal.ProductCatalogAttributeToProduct> GetSelectedAttributes(int productCatalogId, ListItemCollection items, DropDownList ddlAttributeDefault)
        {
            string defaultValue = ddlAttributeDefault.SelectedValue;

            if (items.Cast<ListItem>().Where(x => x.Selected).Where(x => x.Value == defaultValue).Count() == 0)
                defaultValue = items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).FirstOrDefault();


            return items.Cast<ListItem>().Where(x => x.Selected).Select(x => new Dal.ProductCatalogAttributeToProduct()
            {
                AttributeId = Convert.ToInt32(x.Value),
                IsDefault = defaultValue == x.Value,
                ProductCatalogId = productCatalogId
            }).ToList();
        }

        private IEnumerable<ProductCatalogAttributeToProduct> GetAttributesFromRepeater(int productCatalogId, Repeater rpAttributeGroup)
        {
            List<int> attributesRequired = new List<int>();
            List<ProductCatalogAttributeToProduct> attributesToAdd = new List<ProductCatalogAttributeToProduct>();
            foreach (RepeaterItem item in rpAttributeGroup.Items)
            {
                TextBox txbAttributeValue = item.FindControl("txbAttributeValue") as TextBox;
                HiddenField hidAttributeId = item.FindControl("hidAttributeId") as HiddenField;
                HiddenField hidAttributeTypeId = item.FindControl("hidAttributeTypeId") as HiddenField;
                HiddenField hidIsRequired = item.FindControl("hidIsRequired") as HiddenField;

                Dal.ProductCatalogAttributeToProduct atp = new Dal.ProductCatalogAttributeToProduct()
                {
                    AttributeId = Convert.ToInt32(hidAttributeId.Value),
                    ProductCatalogId = productCatalogId
                };

                if (txbAttributeValue.Text.Trim() != "")
                {

                    switch (hidAttributeTypeId.Value)
                    {
                        case "1":
                            atp.DecimalValue = Convert.ToDecimal(txbAttributeValue.Text.Trim()); break;
                        case "2":
                            atp.StringValue = txbAttributeValue.Text.Trim(); break;

                    }
                }
                else
                {

                }
                attributesToAdd.Add(atp);
                // if (Convert.ToBoolean(hidIsRequired.Value ))
                // attributes.Add(Convert.ToInt32(hidAttributeId.Value), true);

            }

            return attributesToAdd.Where(x => x.DecimalValue.HasValue  || x.StringValue!=null).ToList();
        }

        internal void SetAttributes(int[] productIds, bool allowRemove)//, bool clearBeforeAdd, int? groupIdToClear)
        {
            foreach (int productCatalogId in productIds)
            {
                AttributesSave(productCatalogId, allowRemove);//, clearBeforeAdd, groupIdToClear);
            }
        }
        private  void BindDefaultAttributes(ListItemCollection list, DropDownList ddlAttributeDefault, string defaulValue)
        {

            ListItem[] items;

            if (EnableAdding) { 
                items = list.Cast<ListItem>()
                .Where(x => x.Selected)
                .Select(x => new ListItem()
                {
                    Selected = x.Value == defaulValue,
                    Value = x.Value,
                    Text = x.Text
                })
                .ToArray(); }
            else
            {  items = list.Cast<ListItem>()
                .Select(x => new ListItem()
                {
                    Selected = x.Value == defaulValue,
                    Value = x.Value,
                    Text = x.Text
                })
                .ToArray();
        }
            ddlAttributeDefault.Items.Clear();

            ddlAttributeDefault.Items.AddRange(items);
            if(!EnableAdding)
            ddlAttributeDefault.Items.Insert(0, new ListItem());
        }
        //private bool BindAttributes(int attributeGroupId, TextBox txbAttributeValue, Label lblAttributeName, string v)
        //{
        //    Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper(); 

        //    Dal.ProductCatalogAttributesForProductResult attribute =
        //        pch.GetProductCatalogAttributes(attributeGroupId, ProductCatalogId)
        //        .FirstOrDefault();

        //    if (attribute != null && attribute.DecimalValue!=null)
        //    {
        //        txbAttributeValue.Text = String.Format("{0:0.00}", attribute.DecimalValue);

        //    }
        //    if (attribute != null)
        //        lblAttributeName.Text = attribute.AttributeName;
        //    else
        //        txbAttributeValue.Visible = false;
        //    return attribute!=null;
        //    //defaulValue = attributes.Where(x => x.IsDefault.HasValue && x.IsDefault.Value).Select(x => x.AttributeId.ToString()).FirstOrDefault();
        //    //BindDefaultAttributes(list, ddlAttributeDefault, defaulValue);
        //}
        private void BindAttributes(int attributeGroupId, Repeater rpDecimalValues, DropDownList ddlAttributeDefault, string v)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogAttributesForProductResult> attributes =
                pch.GetProductCatalogAttributes(attributeGroupId, ProductCatalogId, groupingId)
                .OrderBy(x => x.AttributeName).ToList();

            rpDecimalValues.DataSource = attributes;
            rpDecimalValues.DataBind();
            ddlAttributeDefault.Visible = false;
            //defaulValue = attributes.Where(x => x.IsDefault.HasValue && x.IsDefault.Value).Select(x => x.AttributeId.ToString()).FirstOrDefault();
            //BindDefaultAttributes(list, ddlAttributeDefault, defaulValue);
        }
        private void BindAttributes(int attributeGroupId, ListItemCollection list, DropDownList ddlAttributeDefault, string defaulValue)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogAttributesForProductResult> attributes =
                pch.GetProductCatalogAttributes(attributeGroupId, ProductCatalogId, groupingId)
                .OrderBy(x => x.AttributeName).ToList();

            foreach (Dal.ProductCatalogAttributesForProductResult attribute in attributes)
            {
                ListItem i = new ListItem()
                {
                    Text = attribute.AttributeName,
                    Value = attribute.AttributeId.ToString(),
                    Selected = attribute.ProductCatalogId.HasValue

                };
                list.Add(i);
            }
            defaulValue = attributes.Where(x => x.IsDefault.HasValue && x.IsDefault.Value).Select(x => x.AttributeId.ToString()).FirstOrDefault();
            BindDefaultAttributes(list, ddlAttributeDefault, defaulValue);


        }



        internal void SetView(bool required, bool allegro, bool option)
        {
            chbRequried.Checked = required;
            chbAllegro.Checked = allegro;
            chbOption.Checked = option;

            chbRequried_CheckedChanged(null, null);
        }

        #endregion

        protected void lbtnChangeView_Click(object sender, EventArgs e)
        {
            BindAttributeGroups();
        }
    }
}