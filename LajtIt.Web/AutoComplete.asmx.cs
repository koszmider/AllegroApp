using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using LajtIt.Bll.Extensions;

namespace LajtIt.Web.AutoCompleteExample
{
  


    public class ReceiptCommand
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public string Xml { get; set; } 
    }

    ///<summary>
    /// Summary description for Service
    ///</summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService] 
    public class AutoComplete : System.Web.Services.WebService
    {
        public class PhotoDetailC
        {
            public int Id { get; set; }
            public int Order { get; set; }
            public string Caption { get; set; }
        }
        [WebMethod]
        public bool PhotoDetail(List<PhotoDetailC> photos)
        {

            int[] imageIdInOrder = photos.OrderBy(x =>x.Order).Select(x => x.Id).ToArray();

            if (imageIdInOrder.Length == 0)
                return false;

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
              pch.SetProductCatalogImagesOrder(imageIdInOrder);


            return true;
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetPaczkomat(string searchText)
        {
            Dal.PaczkomatyHelper ph = new Dal.PaczkomatyHelper();

            return ph.GetPaczkomaty(searchText).Select(x=>String.Format("{0}-{1}", x.Name,x.Description)).Take(20).ToArray();             
             
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethodAttribute]
        public void SetOrderReceiptCommandResult(int id, int statusId, string errorMsg)
        {
            try
            {
                Dal.DbHelper.Orders.SetReceiptCommandResult(id, statusId, errorMsg);


             //   Bll.ErrorHandler.SendEmail(String.Format("Wysłano polecenie do drukarki. Id: {0}, Status: {1}, <Br><Br>Błąd: {2}",                    id, statusId, errorMsg));
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Wysłano polecenie do drukarki. Id: {0}, Status: {1}, <Br><Br>Błąd: {2}",
                    id, statusId, errorMsg));
            }
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethodAttribute]
        public ReceiptCommand[] GetOrderReceiptCommands(int id, string ip, bool isError, string errorMsg)
        {

            ReceiptCommand[] commands = Dal.DbHelper.Orders.GetReceiptCommands()
                .Where(x=>x.OrderReceipt.CashRegisterId==id)
                .Select(x => new ReceiptCommand()
                {
                    Id = x.Id,
                    ReceiptId = x.ReceiptId,
                    Xml = x.XmlCmd
                }).ToArray();

            Dal.DbHelper.Orders.SetCashRegister(id, ip, isError, errorMsg);
            return commands;
        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethodAttribute]
        public List<string> GetInvoiceByCompanyId(string query)
        {

            string[] s = query.Split(new char[] { '|' });
            string cId = s[0];

            if (cId == "")
                return null;

            int companyId = Int32.Parse(cId);

            string invoiceNumber = s[1];

            List<Dal.Cost> costs = Dal.DbHelper.Accounting.GetCostsByCompanyId(companyId, invoiceNumber);

            List<string> attr = costs.Select(x => String.Format("{1}-{2}|{0}", x.CostId, x.InvoiceNumber, x.Company.Name)).ToList();
                

            return attr;
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethodAttribute]
        public List<string> GetInvoices(string query)
        {

            string[] s = query.Split(new char[] { '|' });
            int supplierOwnerId = Int32.Parse(s[0]);
            string invoiceNumber = s[1];

            List<Dal.Cost> costs = Dal.DbHelper.Accounting.GetCosts(supplierOwnerId, invoiceNumber);

            List<string> attr = costs.Select(x => String.Format("{1}-{2}|{0}", x.CostId, x.InvoiceNumber, x.Company.Name)).ToList();


            return attr;
        }
        [WebMethod]
        [System.Web.Script.Services.ScriptMethodAttribute]
        public List<string> GetAttribute(string query)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Dal.ProductCatalogAttribute> attributes = pch.GetProductCatalogAttributes().Where(x => x.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            List<Dal.ProductCatalogAttributeGroup> attributeGroups = pch.GetProductCatalogAttributeGroups()
                .Where(x => x.AttributeGroupTypeId == 1 || x.AttributeGroupTypeId == 2)
                .Where(x => x.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();


            List<string> attr = attributes.Select(x => String.Format("[{0}].[{1}]|0-A-{2}-{3}", x.ProductCatalogAttributeGroup.Name, x.Name, x.AttributeGroupId, x.AttributeId)).ToList();
            attr.AddRange(attributeGroups.Select(x => String.Format("[{0}]|0-G-{1}", x.Name, x.AttributeGroupId)).ToList());


            return attr;
        }
        [WebMethod]
        [System.Web.Script.Services.ScriptMethodAttribute]
        public List<string> GetShopProducers(string query)
        {
            Dal.ShopHelper pch = new Dal.ShopHelper();
            string[] s = query.Split(new char[] { '|' });
            int shopId = Int32.Parse(s[0]);

            List<Dal.ShopProducer> producers = pch.GetShopProducers(shopId)
                .Where(x=>x.Name.ToLower().Contains(s[1].ToLower()))
                .ToList();

            var c = producers.Select(x => String.Format("{0}|{1}", x.Name, x.ShopProducerId)).ToList();

           

            return c.ToList();
        }
        [WebMethod]
        [System.Web.Script.Services.ScriptMethodAttribute]
        public List<string> GetCompanies(string query)
        { 

            List<Dal.Company> companies = Dal.DbHelper.Accounting.GetCompanies(query, 100);

            var c= companies.Select(x => String.Format("{0}|{1}", x.Name, x.CompanyId)).ToList();

            c.Insert(0, "--- bez przypisania ---|0");

            return c.ToList();
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethodAttribute]
        public List<string> GetProducts(string query)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Dal.ProductCatalogView> products = pch.GetProductCatalogForSearch(query, 100);

            return products.Select(x => String.Format("{0} - {1:C} {2}|{3}", x.SupplierName, x.PriceBruttoFixed, x.Name, x.ProductCatalogId)).ToList(); ;
        }
        [WebMethod]
        [System.Web.Script.Services.ScriptMethodAttribute]
        public List<string> GetProductsBySupplier(string supplierId, string query)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Dal.ProductCatalogView> products = pch.GetProductCatalogForSearch(Int32.Parse(supplierId), query, 100);

            return products.Select(x => String.Format("{2} - {0}|{1}", x.Name, x.ProductCatalogId, x.Code)).ToList(); ;
        }
        [WebMethod]
        [System.Web.Script.Services.ScriptMethodAttribute]
        public List<System.Web.UI.WebControls.ListItem> GetShopCategory(int shopTypeId, string shopCategoryId)
        { 
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ShopCategoryFnResult> categories = pch.GetShopCategories(shopTypeId, shopCategoryId);

            var c = categories
                .OrderBy(x=>x.Name)
                
                .Select(x => new System.Web.UI.WebControls.ListItem(x.Name, x.ShopCategoryId.ToString())).ToList();// String.Format("- {0} |{1}", x.Name, x.ShopCategoryId)).ToList(); ;
            return c;
        }

        public AutoComplete()
        {
            //Uncomment the following line if using designed components

            //InitializeComponent();



            FillLists();

        }

        List<Supplier> suppliers = new List<Supplier>();
        List<Family> families = new List<Family>();
        List<Group> groups = new List<Group>();

        private void FillLists()

        { 
            var s = Dal.DbHelper.ProductCatalog.GetSuppliers();
            Dal.ProductCatalogGroupHelper pch = new Dal.ProductCatalogGroupHelper();
            var f = pch.GetProductCatalogFamilies();
            var g = pch.GetProductCatalogGroups();


            suppliers.AddRange(
                s
                .OrderBy(x=>x.Name)
                .Select(x => new Supplier()
                {
                    SupplierId = x.SupplierId,
                    SupplierName = x.Name
                }
                ));
            //suppliers.Insert(0, new Supplier() { SupplierId = 0, SupplierName = "-- bez producenta --" });
            families.AddRange(
                f.Select(x => new Family()
                {
                    FamilyId = x.FamilyId,
                    SupplierId = x.SupplierId,
                    FamilyName = x.FamilyName
                }));
            groups.AddRange(
                g.Select(x => new Group()
                {
                    FamilyId = x.FamilyId,
                    GroupId = x.ProductCatalogGroupId,
                    GroupName = x.GroupName
                }
                ));
        }


        [WebMethod]

        public CascadingDropDownNameValue[] GetSuppliers(string knownCategoryValues, string category)

        {

            List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

            var x = from c in suppliers select c;
            l.Add(new CascadingDropDownNameValue("--- brak ---", "0"));
            foreach (Supplier supplier in x)

            {

                l.Add(new CascadingDropDownNameValue(supplier.SupplierName, supplier.SupplierId.ToString()));

            }

            return l.ToArray();

        }


        [WebMethod]

        public CascadingDropDownNameValue[] GetFamilies(string knownCategoryValues, string category)

        {
            int supplierId = 0;

            System.Collections.Specialized.StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            if (!kv.ContainsKey("Supplier") || !Int32.TryParse(kv["Supplier"], out supplierId))

            {

                throw new ArgumentException("Couldn't find Supplier.");

            };

            List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();
            IEnumerable<Family> fam;// = new IEnumerable<Family>();
            if (supplierId == 0)
                fam = from c in families where c.SupplierId == null select c;
            else
                fam = from c in families where c.SupplierId == supplierId select c;


            foreach (Family family in fam)

            {

                l.Add(new CascadingDropDownNameValue(family.FamilyName, family.FamilyId.ToString()));

            }

            return l.ToArray();

        }



        [WebMethod]

        public CascadingDropDownNameValue[] GetGroups(string knownCategoryValues, string category)
        {
            int familyId;

            System.Collections.Specialized.StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            if (!kv.ContainsKey("Family") || !Int32.TryParse(kv["Family"], out familyId))

            {

                throw new ArgumentException("Couldn't find Family.");

            };

            List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

            var x = from le in groups where le.FamilyId == familyId select le;

            foreach (Group group in x)
            {
                l.Add(new CascadingDropDownNameValue(group.GroupName, group.GroupId.ToString()));
            }

            return l.ToArray();

        }



        #region Classes

        public class Supplier
        {

            public int? SupplierId { get; set; }

            public string SupplierName { get; set; }

        }
        public class Family
        {

            public int FamilyId { get; set; }
            public int? SupplierId { get; set; }
            public string FamilyName { get; set; }

        }

        public class Group
        {
            public int GroupId { get; set; }

            public string GroupName { get; set; }

            public int FamilyId { get; set; }

        }


        #endregion





        List<AttributeGroup> attributeGroups = new List<AttributeGroup>();
        List<Attribute> attributes = new List<Attribute>();








        #region ProductCatalog
        public class AttributeGroup
        {

            public int AttributeGroupId { get; set; }

            public string Name { get; set; }

        }
        public class Attribute
        {

            public int AttributeGroupId { get; set; }
            public int AttributeId { get; set; }
            public string Name { get; set; }

        }
        [WebMethod]

        public CascadingDropDownNameValue[] GetAttributeGroups(string knownCategoryValues, string category)

        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();


            attributeGroups.AddRange(
                pch.GetProductCatalogAttributeGroups().OrderBy(xy=>xy.Name).Select(xy => new AttributeGroup()
                {
                    AttributeGroupId = xy.AttributeGroupId,
                    Name = xy.Name
                }
                ));
            List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

            var x = from c in attributeGroups select c;
            foreach (AttributeGroup group in x)

            {

                l.Add(new CascadingDropDownNameValue(group.Name, group.AttributeGroupId.ToString()));

            }

            return l.ToArray();

        }


        [WebMethod]

        public CascadingDropDownNameValue[] GetAttributes(string knownCategoryValues, string category)

        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
 
            attributes.AddRange(
                pch.GetProductCatalogAttributes().OrderBy(x=>x.Name).Select(x => new Attribute()
                {
                    AttributeGroupId = x.AttributeGroupId,
                    AttributeId = x.AttributeId,
                    Name = x.Name
                }));


            int attributeGroupId = 0;

            System.Collections.Specialized.StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            if (!kv.ContainsKey("AttributeGroup") || !Int32.TryParse(kv["AttributeGroup"], out attributeGroupId))

            {

                throw new ArgumentException("Couldn't find AttributeGroup.");

            };

            List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();
            IEnumerable<Attribute> att = from c in attributes where c.AttributeGroupId == attributeGroupId select c;


            foreach (Attribute a in att)

            {

                l.Add(new CascadingDropDownNameValue(a.Name, a.AttributeId.ToString()));

            }

            return l.ToArray();

        }

        #endregion
    }
}
