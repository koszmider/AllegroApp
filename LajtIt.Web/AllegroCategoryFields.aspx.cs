using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("4ed93ea3-531c-43a6-9795-755c4ccb35dd")]
    public partial class ShopCategoryFields : LajtitPage
    {
        private List<Dal.ProductCatalogAttribute> attributes;
        private List<Dal.ProductCatalogAttributeGroup> attributeGroups;
        private List<Dal.ProductCatalogFieldsToAllegroParametersResult> catalogFields;



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShopTypes();
                ddlShopType_SelectedIndexChanged(null, null);
                //BindCategories();

                //BindFields();
            }
        }

        private void BindShopTypes()
        { 
            ddlShopType.DataSource = Dal.DbHelper.Shop.GetShopTypes();
            ddlShopType.DataBind();
        }

        Bll.AllegroRESTHelper.Parameters.RootObject parameters;
        Bll.EmagRESTHelper.Result parametersEmag;
        
          protected void lbtnFieldsSpecImport_Click(object sender, EventArgs e)
        {
            List<Dal.ShopCategoryField> fields = new List<Dal.ShopCategoryField>();

            int categoryId = ucShopCategoryControl.GetCategoryId().Value;
            switch ((Dal.Helper.ShopType)Int32.Parse(ddlShopType.SelectedValue))
            {
                case Dal.Helper.ShopType.Allegro:

                    Bll.AllegroHelper ah = new Bll.AllegroHelper();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    parameters = Bll.AllegroRESTHelper.GetCategoryParameters(ucShopCategoryControl.GetShopCategoryId());
                    foreach (Bll.AllegroRESTHelper.Parameters.Parameter par in parameters.parameters)
                    {
                        fields.Add(new Dal.ShopCategoryField()
                        {
                            CategoryFieldId = par.id,
                            CategoryId = categoryId,
                            Description = par.name,
                            FieldType = GetFieldType (par.type),
                            IsRequired = par.required,
                            PassToShop = true,
                            ShopTypeId= (int)Dal.Helper.ShopType.Allegro,
                            UseDefaultValue = false,
                            UpdateParameter = true,
                            UseDefaultAttribute = false

                        }
                        );

                    }


                    Dal.ShopHelper oh = new Dal.ShopHelper();

                    oh.SetShopCategoryFields(Dal.Helper.ShopType.Allegro, categoryId, fields);

                    break;
                case Dal.Helper.ShopType.eMag:
                    parametersEmag = Bll.EmagRESTHelper.GetCategory(ucShopCategoryControl.GetShopCategoryId());

                    gvParametersEmag.DataSource = parametersEmag.characteristics.OrderByDescending(x => x.is_mandatory).ToList();
                    gvParametersEmag.DataBind();
                    break;
            }
            BindFields();
        }

        private int GetFieldType(string type)
        {
            switch(type)
            {
                case "float":
                    return 3;
                case "string":
                    return 2;
                default:
                    return 1;
            }
        }
 
        public class AllegroParameters : Bll.AllegroRESTHelper.Parameters.Parameter
        { 
        public string source { get; set; }
        }

        List<AllegroParameters> ap ;
        protected void lbtnFieldsSpec_Click(object sender, EventArgs e)
        {

            switch((Dal.Helper.ShopType)Int32.Parse(ddlShopType.SelectedValue))
            {
                case Dal.Helper.ShopType.Allegro:

                    Bll.AllegroHelper ah = new Bll.AllegroHelper();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                 

                    ap = new List<AllegroParameters>(); 

                    ap.AddRange(Bll.AllegroRESTHelper.GetCategoryProductParameters(ucShopCategoryControl.GetShopCategoryId()).parameters.Select(x => new AllegroParameters()
                    {
                        source = "product",
                        dictionary = x.dictionary,
                        id = x.id,
                        name = x.name,
                        options = x.options,
                        required = x.required,
                        restrictions = x.restrictions,
                        type = x.type,
                        unit = x.unit
                    }));
                    string[] ids = ap.Select(x => x.id).ToArray();

                    ap.AddRange(Bll.AllegroRESTHelper.GetCategoryParameters(ucShopCategoryControl.GetShopCategoryId()).parameters.Select(x => new AllegroParameters()
                    {
                        source = "offer",
                        dictionary = x.dictionary,
                        id = x.id,
                        name = x.name,
                        options = x.options,
                        required = x.required,
                        restrictions = x.restrictions,
                        type = x.type,
                        unit = x.unit
                    }).Where(x=> !ids.Contains(x.id)).ToList());

                    gvParametersAllegro.DataSource = ap.OrderBy(x=>x.source).ToList();
                    gvParametersAllegro.DataBind();

                    //gvParemtersProductAllegro.DataSource = parameters.parameters;
                    //gvParemtersProductAllegro.DataBind();


                    //parameters = ;
                    //gvParemtersProductAllegro.DataSource = parameters.parameters;
                    //gvParemtersProductAllegro.DataBind();
                    break;
                case Dal.Helper.ShopType.eMag:
                    parametersEmag = Bll.EmagRESTHelper.GetCategory(ucShopCategoryControl.GetShopCategoryId());

                    gvParametersEmag.DataSource = parametersEmag.characteristics.OrderByDescending(x=>x.is_mandatory).ToList();
                    gvParametersEmag.DataBind();
                    break;
            }
        }
        protected void gvParametersAllegro_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gvDictionary = e.Row.FindControl("gvDictionary") as GridView;

                //Bll.AllegroRESTHelper.Parameters.Parameter parameter = e.Row.DataItem as Bll.AllegroRESTHelper.Parameters.Parameter;
                AllegroParameters parameter = e.Row.DataItem as AllegroParameters;

                gvDictionary.DataSource = parameter.dictionary;
                gvDictionary.DataBind();


                Label lblType = e.Row.FindControl("lblType") as Label; 
                Label lblRestrictions = e.Row.FindControl("lblRestrictions") as Label;

                lblType.Text = parameter.type;

                if (parameter.restrictions!=null)
                lblRestrictions.Text = String.Format("min {0}, max {1}<br>dł min {2}, dł max {3}<br>wielokrotny {4}<br>wariant: {5}", 
                    parameter.restrictions.min, parameter.restrictions.max, parameter.restrictions.minLength, parameter.restrictions.maxLength, parameter.restrictions.multipleChoices,
                    parameter.options?.variantsAllowed);
           
            }
        }
        protected void gvParametersEmag_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gvDictionary = e.Row.FindControl("gvDictionary") as GridView;
                CheckBox chbIsRequired = e.Row.FindControl("chbIsRequired") as CheckBox;

                Bll.EmagRESTHelper.Characteristic emag = e.Row.DataItem as Bll.EmagRESTHelper.Characteristic;

                gvDictionary.DataSource = emag.values;
                gvDictionary.DataBind();

                chbIsRequired.Checked = emag.is_mandatory == 1;

                Label lblType = e.Row.FindControl("lblType") as Label; 
                Label lblRestrictions = e.Row.FindControl("lblRestrictions") as Label;

                lblType.Text = emag.type_id.ToString();

 
                //if (parameter.restrictions != null)
                    //lblRestrictions.Text = String.Format("min {0}, max {1}<br>dł min {2}, dł max {3}<br>wielokrotny {4}<br>wariant: {5}",
                    //    parameter.restrictions.min, parameter.restrictions.max, parameter.restrictions.minLength, parameter.restrictions.maxLength, parameter.restrictions.multipleChoices,
                    //    parameter.options.variantsAllowed);

            }
        }
        protected void ddlShopType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucShopCategoryControl.SetCategoryId((Dal.Helper.ShopType)(Convert.ToInt32(ddlShopType.SelectedValue)), 0);
            //BindCategories();
        }
        //protected void ddlCategory_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    gvParameters.DataSource = null;
        //    gvParameters.DataBind();
        //    BindFields();

        //}
        protected void gvShopCategoryFields_OnDataBound(object sender, GridViewRowEventArgs e)
        {
            Dal.ShopCategoryField acf = e.Row.DataItem as Dal.ShopCategoryField;
            if(e.Row.RowType == DataControlRowType.Header)
            {
                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
                attributeGroups = pch.GetProductCatalogAttributeGroups();
                attributes = pch.GetProductCatalogAttributes();
                catalogFields = pch.GetProductCatalogFieldsToAllegroParameters(0);

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chbUseDefaultValue = e.Row.FindControl("chbUseDefaultValue") as CheckBox;
                if (e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Normal)
                {
                    Label lblFieldTypeId = e.Row.FindControl("lblFieldTypeId") as Label;
                    Label lbbUseDefaultValue = e.Row.FindControl("lblUseDefaultValue") as Label;
                    Label lblAttributeGroup = e.Row.FindControl("lblAttributeGroup") as Label;
                    Label lblAttribute = e.Row.FindControl("lblAttribute") as Label;
                    Label lblFieldName = e.Row.FindControl("lblFieldName") as Label;

                    lblFieldTypeId.Text = ddlFieldTypeId.Items.FindByValue( acf.FieldType.ToString()).Text;
                    chbUseDefaultValue.Checked = acf.UseDefaultValue;
                    if (acf.AttributeId.HasValue) lblAttribute.Text = attributes.Where(x => x.AttributeId == acf.AttributeId.Value)
                        .Select(x => String.Format("{0}.{1}", x.ProductCatalogAttributeGroup.Name, x.Name))
                        .FirstOrDefault();
                    if (acf.AttributeGroupId.HasValue) lblAttributeGroup.Text = attributeGroups.Where(x => x.AttributeGroupId == acf.AttributeGroupId.Value)
                            .Select(x => x.Name)
                            .FirstOrDefault();

                    if (acf.SystemFieldId.HasValue) lblFieldName.Text = catalogFields.Where(x => x.SystemFieldId == acf.SystemFieldId.Value)
                            .Select(x => x.FieldName)
                            .FirstOrDefault();

                    switch (acf.FieldType)
                    {
                        case 1: if (acf.IntValue.HasValue) lbbUseDefaultValue.Text = String.Format("({0})", acf.IntValue); break;
                        case 2: if (acf.StringValue!=null) lbbUseDefaultValue.Text = String.Format("({0})", acf.StringValue); break;
                        case 3: if (acf.FloatValue.HasValue) lbbUseDefaultValue.Text = String.Format("({0})", acf.FloatValue); break;
                    }
                    (e.Row.Cells[0].Controls[2] as LinkButton).OnClientClick = "return confirm('Czy usunąć dane pole?');";


                    if (acf.AttributeGroupId == null && acf.AttributeId == null && acf.SystemFieldId == null && acf.UseDefaultValue == false)
                        e.Row.BackColor = System.Drawing.Color.Silver;

                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlFieldTypeId = e.Row.FindControl("ddlFieldTypeId") as DropDownList;
                    DropDownList ddlAttributeGroup = e.Row.FindControl("ddlAttributeGroup") as DropDownList;
                    DropDownList ddlAttribute = e.Row.FindControl("ddlAttribute") as DropDownList;
                    DropDownList ddlFieldNames = e.Row.FindControl("ddlFieldNames") as DropDownList;

                    TextBox txbUseDefaultValue = e.Row.FindControl("txbUseDefaultValue") as TextBox;
                    ddlFieldTypeId.SelectedValue = acf.FieldType.ToString();
                    chbUseDefaultValue.Checked = acf.UseDefaultValue;
                    switch (acf.FieldType)
                    {
                        case 1: if (acf.IntValue.HasValue) txbUseDefaultValue.Text = String.Format("{0}", acf.IntValue); break;
                        case 2: if (acf.StringValue != null) txbUseDefaultValue.Text = String.Format("{0}", acf.StringValue); break;
                        case 3: if (acf.FloatValue.HasValue) txbUseDefaultValue.Text = String.Format("{0}", acf.FloatValue); break;
                    }
                    (e.Row.Cells[3].Controls[0] as TextBox).Width = 100;


                    ddlFieldNames.DataSource = catalogFields;
                    ddlFieldNames.DataBind();

                    ddlAttributeGroup.DataSource = attributeGroups.Where(x => x.AttributeGroupTypeId == 1 || x.AttributeGroupTypeId == 2).OrderBy(x=>x.Name).ToList();
                    ddlAttribute.DataSource = attributes.Where(x => x.ProductCatalogAttributeGroup.AttributeGroupTypeId == 3)
                        .Select(x => new
                        {
                            Name = String.Format("{0}.{1}", x.ProductCatalogAttributeGroup.Name, x.Name),
                            AttributeId = x.AttributeId
                        })
                        .OrderBy(x=>x.Name)
                        .ToList();
                    ddlAttributeGroup.DataBind();
                    ddlAttribute.DataBind();

                    if (acf.AttributeGroupId.HasValue)
                        ddlAttributeGroup.SelectedIndex = ddlAttributeGroup.Items.IndexOf(ddlAttributeGroup.Items.FindByValue(acf.AttributeGroupId.ToString()));

                    if (acf.AttributeId.HasValue)
                        ddlAttribute.SelectedIndex = ddlAttribute.Items.IndexOf(ddlAttribute.Items.FindByValue(acf.AttributeId.ToString()));
                    if (acf.SystemFieldId.HasValue)
                        ddlFieldNames.SelectedIndex = ddlFieldNames.Items.IndexOf(ddlFieldNames.Items.FindByValue(acf.SystemFieldId.ToString()));
                }
            }

        }

        protected void gvShopCategoryFields_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvShopCategoryFields.EditIndex = -1;
            BindFields();
        }
        protected void gvShopCategoryFields_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvShopCategoryFields.EditIndex = e.NewEditIndex;
            BindFields();

        }
        protected void gvShopCategoryFields_OnRowDeleting( object sender, GridViewDeleteEventArgs e)
        {
            int fieldId = Convert.ToInt32(gvShopCategoryFields.DataKeys[e.RowIndex][0]);

            Dal.OrderHelper oh = new Dal.OrderHelper();

            //oh.SetShopCategoryFieldDelete(Convert.ToInt32(ddlCategory.SelectedValue), fieldId);

            BindFields();

        }
        protected void gvShopCategoryFields_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvShopCategoryFields.Rows[e.RowIndex];
            TextBox txbQuantity = row.FindControl("txbQuantity") as TextBox;
            CheckBox chbUseDefaultValue1 = row.FindControl("chbUseDefaultValue") as CheckBox;
            CheckBox chbUpdateParameter = row.FindControl("chbUpdateParameter") as CheckBox;
            CheckBox chbUseDefaultAttribute = row.FindControl("chbUseDefaultAttribute") as CheckBox;
            TextBox txbUseDefaultValue = row.FindControl("txbUseDefaultValue") as TextBox;
            DropDownList ddlAttributeGroup = row.FindControl("ddlAttributeGroup") as DropDownList;
            DropDownList ddlAttribute = row.FindControl("ddlAttribute") as DropDownList;
            DropDownList ddlFieldNames = row.FindControl("ddlFieldNames") as DropDownList;

            int id = Convert.ToInt32(gvShopCategoryFields.DataKeys[e.RowIndex][0]);
            string categoryFieldId = (row.Cells[1].Controls[0] as TextBox).Text;
            int fieldType = Convert.ToInt32((row.Cells[2].Controls[1] as DropDownList).SelectedValue);
            string defaultValue = txbUseDefaultValue.Text.Trim() == ""? null:txbUseDefaultValue.Text.Trim();
            int? attributeGroupId = null;
            if (ddlAttributeGroup.SelectedIndex != 0)
                attributeGroupId = Convert.ToInt32(ddlAttributeGroup.SelectedValue);
            int? attributeId = null;
            int? systemFieldId = null;
            if (ddlAttribute.SelectedIndex != 0)
                attributeId = Convert.ToInt32(ddlAttribute.SelectedValue);
            if (ddlFieldNames.SelectedIndex != 0)
                systemFieldId = Convert.ToInt32(ddlFieldNames.SelectedValue);

            Dal.ShopCategoryField field = new Dal.ShopCategoryField()
            {
                //CategoryId = Convert.ToInt32(ddlCategory.SelectedValue),
                Description = (row.Cells[3].Controls[0] as TextBox).Text,
                //FieldId = fieldId,
                FieldType = fieldType,
                PassToShop = (row.Cells[8].Controls[0] as CheckBox).Checked,
                UpdateParameter = (row.Cells[9].Controls[0] as CheckBox).Checked,
                AttributeGroupId = attributeGroupId,
                AttributeId = attributeId,
                IsRequired = (row.Cells[10].Controls[0] as CheckBox).Checked,
                UseDefaultAttribute = (row.Cells[11].Controls[0] as CheckBox).Checked,
                SystemFieldId = systemFieldId,
                CategoryFieldId= categoryFieldId,
                Id=id
            };
            field.UseDefaultValue = chbUseDefaultValue1.Checked; 
            switch (fieldType)
            {
                case 1:
                    if (defaultValue == null)
                        field.IntValue = null;
                    else
                        field.IntValue = Convert.ToInt32(defaultValue);
                    break;
                case 2: field.StringValue = defaultValue; break;
                case 3:
                    if (defaultValue == null)
                        field.FloatValue = null;
                    else
                        field.FloatValue = Convert.ToDecimal(defaultValue);
                    break;
            }

            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetShopCategoryFieldUpdate(field);

            gvShopCategoryFields.EditIndex = -1;
            BindFields();

        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Dal.ShopCategoryField field = new Dal.ShopCategoryField()
            {
                //CategoryId = Convert.ToInt32(ddlCategory.SelectedValue),
                Description = txbDescription.Text.Trim(),
                //FieldId = Convert.ToInt32(txbFieldId.Text.Trim()),
                FieldType = Convert.ToInt32(ddlFieldTypeId.SelectedValue),
                UseDefaultValue = chbUseDefaultValue.Checked,
                PassToShop = chbPassToAllegro.Checked,
                UpdateParameter = chbUpdateParameter.Checked,
                IsRequired = chbIsRequired.Checked,
                UseDefaultAttribute = chbUseDefaultAttribute.Checked,
                CategoryFieldId= txbCategoryFieldId.Text,
                CategoryId=ucShopCategoryControl.GetCategoryId().Value,
                ShopTypeId = Int32.Parse(ddlShopType.SelectedValue)
                

            };


            string defaultValue = txbUseDefaultValue.Text.Trim() == "" ? null : txbUseDefaultValue.Text.Trim();
             
            switch (field.FieldType)
            {
                case 1:
                    if (defaultValue == null)
                        field.IntValue = null;
                    else
                        field.IntValue = Convert.ToInt32(defaultValue);
                    break;
                case 2: field.StringValue = defaultValue; break;
                case 3:
                    if (defaultValue == null)
                        field.FloatValue = null;
                    else
                        field.FloatValue = Convert.ToDecimal(defaultValue);
                    break;
            }

            Dal.OrderHelper oh = new Dal.OrderHelper();

            try
            {
                oh.SetShopCategoryField(field);
            }catch(Exception ex)
            {
                DisplayMessage(String.Format("Błąd dodawania nowego pola.<br><br>{0}", ex.Message));

            }
            BindFields();

        }
        private void BindFields()
        {

            Dal.ShopHelper oh = new Dal.ShopHelper();

            string catid = ucShopCategoryControl.GetShopCategoryId();
            List<Dal.ShopCategoryField> categoryFields = oh.GetShopCategoryFieldsByCategoryId( ucShopCategoryControl.GetShopType().Value, ucShopCategoryControl.GetShopCategoryId(), null);

            gvShopCategoryFields.DataSource = categoryFields;
            gvShopCategoryFields.DataBind();
        }

        protected void btnShowFields_Click(object sender, EventArgs e)
        {
            BindFields();

            gvParametersEmag.DataSource = null;
            gvParametersEmag.DataBind();
            gvParametersAllegro.DataSource = null;
            gvParametersAllegro.DataBind();
        }

        //private void BindCategories()
        //{

        //    Dal.ShopHelper sh = new Dal.ShopHelper();

        //    ddlCategory.DataSource = sh.GetShopCategories((Dal.Helper.ShopType)(Convert.ToInt32(ddlShopType.SelectedValue)))
        //        //.Where(x => x.CanCreateAuction)
        //        .OrderBy(x => x.Name).ToList();
        //    ddlCategory.DataBind();
        //}

    }
}