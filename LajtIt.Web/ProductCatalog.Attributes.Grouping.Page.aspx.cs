using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;

namespace LajtIt.Web
{
    [Developer("1ee818a8-3310-419e-afd1-dcbeb33af8d2")]
    public partial class ProductCatalogAttributesGroupingPage : LajtitPage
    {
        private int AttributeGroupingId { get { return Convert.ToInt32(Request.QueryString["id"].ToString()); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrouping();
                 
                 
            }
        }
        private void BindGrouping()
        {
            Dal.ProductCatalogHelper ph = new Dal.ProductCatalogHelper();

            Dal.ProductCatalogAttributeGrouping scm = ph.GetProductCatalogAttributeGrouping(AttributeGroupingId);
            
            txbName.Text = scm.Name;
            //txbDescription.Text = promotion.Description;

            if(scm.GroupingTypeId==1)
            {
                lbxAttributeGroups.Enabled = lbxAttributeGroupsSelected.Enabled = btnAttributeGroupAdd.Enabled = btnAttributeGroupDel.Enabled = false;
                txbAttribute.Enabled = btnAttributeAdd2.Enabled= false;
            }

            List<Dal.ProductCatalogAttribute> attributesSelected = ph.GetProductCatalogAttributeGroupingAttributes(AttributeGroupingId);
            List<Dal.ProductCatalogAttributeGroup> attributeGroupsSelected = ph.GetProductCatalogAttributeGroupingAttributeGroups(AttributeGroupingId);
            int[] attributesSelectedIds = attributesSelected.Select(x => x.AttributeId).ToArray();
            int[] attributeGroupsSelectedIds = attributeGroupsSelected.Select(x => x.AttributeGroupId).ToArray();





            List<Dal.ProductCatalogAttribute> attributes = ph.GetProductCatalogAttributes()               
                //.Where(x => x.ProductCatalogAttributeGroup.AttributeGroupTypeId==1 || x.ProductCatalogAttributeGroup.AttributeGroupTypeId == 2)
                .ToList() ;

            if (scm.GroupingTypeId == 1)
                attributes = attributes.Where(x => x.AttributeGroupId == 6).ToList();

            var groups = attributes.Select(x => x.ProductCatalogAttributeGroup).Distinct().OrderBy(x => x.Name).ToList();

            lbxAttributes.Items.Clear();
            foreach(Dal.ProductCatalogAttributeGroup group in groups)
            {
                lbxAttributes.Items.AddRange(
                    attributes.Where(x => x.AttributeGroupId == group.AttributeGroupId && !attributesSelectedIds.Contains(x.AttributeId)
                    )
                    .Select(x => new ListItem(String.Format("[{0}].({1})", group.Name, x.Name), x.AttributeId.ToString()))
                    .ToArray()
                    );
            }

            ListItem[] itemsSelected = attributes.Where(x => attributesSelectedIds.Contains(x.AttributeId))
                    .Select(x => new ListItem(String.Format("[{0}].({1})", x.ProductCatalogAttributeGroup.Name, x.Name), x.AttributeId.ToString()))
                    .ToArray();


            //lbxAttributes.Items.Cast<ListItem>().Where(x => attributesSelectedIds.Contains(Convert.ToInt32(x.Value))).ToArray();
            lbxAttributesSelected.Items.Clear();
            lbxAttributesSelected.Items.AddRange(itemsSelected);





            List<Dal.ProductCatalogAttributeGroup> attributeGroups = ph.GetProductCatalogAttributeGroups()
                .OrderBy(x => x.Name)
                .ToList();
             
             

            lbxAttributeGroups.Items.Clear();
            foreach (Dal.ProductCatalogAttributeGroup group in attributeGroups)
            {
                lbxAttributeGroups.Items.AddRange(
                    attributeGroups.Where(x => x.AttributeGroupId == group.AttributeGroupId && !attributeGroupsSelectedIds.Contains(x.AttributeGroupId)
                    )
                    .Select(x => new ListItem(String.Format("[{0}]", group.Name), x.AttributeGroupId.ToString()))
                    .ToArray()
                    );
            }

            ListItem[] itemGroupsSelected = attributeGroups.Where(x => attributeGroupsSelectedIds.Contains(x.AttributeGroupId))
                    .Select(x => new ListItem(String.Format("[{0}]", x.Name), x.AttributeGroupId.ToString()))
                    .ToArray();


            //lbxAttributes.Items.Cast<ListItem>().Where(x => attributesSelectedIds.Contains(Convert.ToInt32(x.Value))).ToArray();
            lbxAttributeGroupsSelected.Items.Clear();
            lbxAttributeGroupsSelected.Items.AddRange(itemGroupsSelected);


        }

          

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Dal.ProductCatalogAttributeGrouping scm = new Dal.ProductCatalogAttributeGrouping()
                { 
                    Name = txbName.Text.Trim() ,
                    AttributeGroupingId = AttributeGroupingId
                };
                 
                int[] attributes = lbxAttributesSelected.Items.Cast<ListItem>().Select(x => Convert.ToInt32(x.Value)).ToArray();
                int[] groups = lbxAttributeGroupsSelected.Items.Cast<ListItem>().Select(x => Convert.ToInt32(x.Value)).ToArray();
                Dal.ProductCatalogHelper pch = new ProductCatalogHelper();
                pch.SetProductCatalogAttributeGrouping(scm, attributes, groups);

                DisplayMessage("Zmiany zostały zapisane");
                BindGrouping();
            }
            catch (Exception ex)
            {
                DisplayMessage(String.Format("Błąd zapisu: {0}", ex.Message));

            }
        }

        private int GetValue(TextBox txb)
        {
            if (String.IsNullOrEmpty(txb.Text))
                return 0;
            else
                return Int32.Parse(txb.Text);
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




        protected void btnAttributeDel_Click(object sender, EventArgs e)
        {
            MoveItems(lbxAttributesSelected, lbxAttributes);

        }

        protected void btnAttributeAdd_Click(object sender, EventArgs e)
        {
            MoveItems(lbxAttributes, lbxAttributesSelected);
        }
        protected void btnAttributeGroupDel_Click(object sender, EventArgs e)
        {
            MoveItems(lbxAttributeGroupsSelected, lbxAttributeGroups);

        }

        protected void btnAttributeGroupAdd_Click(object sender, EventArgs e)
        {
            MoveItems(lbxAttributeGroups, lbxAttributeGroupsSelected);
        }

        protected void btnAttributeAdd2_Click(object sender, EventArgs e)
        {
            try
            {
                string[] ids = hfAttribute.Value.Split(new char[] { '-' });

                string groupOrAttribute = ids[0];

                Dal.ProductCatalogHelper pch = new ProductCatalogHelper();
                 

                if (groupOrAttribute == "A")
                {

                    if (lbxAttributesSelected.Items.FindByValue(ids[2]) != null)
                        return;
                    Dal.ProductCatalogAttribute a = Dal.DbHelper.ProductCatalog.GetProductCatalogAttribute(Int32.Parse(ids[2]));

                    lbxAttributesSelected.Items.Add(new ListItem(String.Format("[{0}].({1})", a.ProductCatalogAttributeGroup.Name, a.Name), ids[2]));
                    lbxAttributes.Items.RemoveAt(lbxAttributes.Items.IndexOf(lbxAttributes.Items.FindByValue(ids[2])));



                    //Dal.ProductCatalogAttributeGroupingAttribute att = new Dal.ProductCatalogAttributeGroupingAttribute()
                    //{
                    //    AttributeGroupingId = AttributeGroupingId,
                    //    AttributeId= Int32.Parse(ids[2])
                    //};
                    //pch.SetProductCatalogAttributeGroupingAttribute(att);
                }
                else
                {

                    if (lbxAttributeGroupsSelected.Items.FindByValue(ids[1]) != null)
                        return;

                    Dal.ProductCatalogAttributeGroup g = pch.GetProductCatalogAttributeGroup(Int32.Parse(ids[1]));
                    lbxAttributeGroupsSelected.Items.Add(new ListItem(String.Format("[{0}]", g.Name), ids[1]));
                    lbxAttributeGroups.Items.RemoveAt(lbxAttributeGroups.Items.IndexOf(lbxAttributeGroups.Items.FindByValue(ids[1])));

                    //Dal.ProductCatalogAttributeGroupingAttributeGroup att = new Dal.ProductCatalogAttributeGroupingAttributeGroup()
                    //{
                    //    AttributeGroupingId = AttributeGroupingId,
                    //    AttributeGroupId = Int32.Parse(ids[1])
                    //};
                    //pch.SetProductCatalogAttributeGroupingAttributeGroup(att);
                }

                //BindGrouping();
            }
            catch (Exception ex)
            {
                DisplayMessage(String.Format("Błąd zapisu: {0}", ex.Message));

            }
        }
    }
}