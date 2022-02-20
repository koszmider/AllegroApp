using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;

namespace LajtIt.Web.Controls
{
    public partial class AttributeMenu : LajtitControl 
    {

        public string SetTab
        {
            set
            {
                ViewState["tab"] = value;
            }
            get
            {
                return ViewState["tab"].ToString();
            }
        }
        private int AttributeId
        {
            get { return Convert.ToInt32(Request.QueryString["id"]); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindHeader();
            BindSelection(); 
        }

        private void BindHeader()
        {
            Dal.ProductCatalogAttribute attribute = Dal.DbHelper.ProductCatalog.GetProductCatalogAttribute(AttributeId);

            if (attribute == null)
                return;

            litAttribute.Text = String.Format("{0}", attribute.Name);
            hlAttributeGroup.NavigateUrl = String.Format(hlAttributeGroup.NavigateUrl, attribute.AttributeGroupId);
            hlAttributeGroup.Text = String.Format("[{0}]", attribute.ProductCatalogAttributeGroup.Name); ;
        }

        internal   void NotAvailable()
        {
            pnNotAvailable.Visible = true;
        }
        public void NotExists()
        {
            pnNotExists.Visible = true;

        }

        private void BindSelection()
        {
            switch (SetTab)
            {

                case "td1": td1.Attributes.Add("class", "tabSelected"); break;
                case "td2": td2.Attributes.Add("class", "tabSelected"); break;
                case "td3": td3.Attributes.Add("class", "tabSelected"); break;
                case "td4": td4.Attributes.Add("class", "tabSelected"); break;
                case "td5": td5.Attributes.Add("class", "tabSelected"); break;
                case "td6": td6.Attributes.Add("class", "tabSelected"); break;
            }
            hl1.NavigateUrl = String.Format(hl1.NavigateUrl, AttributeId);
            hl2.NavigateUrl = String.Format(hl2.NavigateUrl, AttributeId);
            hl3.NavigateUrl = String.Format(hl3.NavigateUrl, AttributeId);
            hl4.NavigateUrl = String.Format(hl4.NavigateUrl, AttributeId);
            hl5.NavigateUrl = String.Format(hl5.NavigateUrl, AttributeId);
            hl6.NavigateUrl = String.Format(hl6.NavigateUrl, AttributeId);
        }

        protected void lbtnAttrbiuteDelete_Click(object sender, EventArgs e)
        {
            mpeAttribute.Show();
            List<string> msg;
            bool canDeleteAttribute = 
                Dal.DbHelper.ProductCatalog.GetProductCatalogAttributeCanDelete(AttributeId, out msg);


            if (!canDeleteAttribute)
                lblInfo.Text = String.Format("Wykryto kilka problemów, które należy wziąć pod uwagę przed usunięciem atrybutu.<ul><li>{0}</ul>[KRYTYCZNY] - oznacza, że przed usunięciem należy usunąć ręcznie przypisania.<br>Bez tego oznaczenia, relacje będą usunięte automatycznie.", String.Join("<li>", msg.ToArray()));
            else
                lblInfo.Text = "Możesz bezpiecznie usunąć atrybut";


            if(msg.Where(x=>x.Contains("[KRYTYCZNY]")).Count()>0)
                    btnDelete.Visible = false;
            else
                btnDelete.Visible = true;


        }

    

        protected void btnDelete_Click(object sender, EventArgs e)
        { List<string> msg;
            bool canDeleteAttribute =
               Dal.DbHelper.ProductCatalog.GetProductCatalogAttributeCanDelete(AttributeId, out msg);

            if (canDeleteAttribute || msg.Where(x => x.Contains("[KRYTYCZNY]")).Count() == 0)

                if (Dal.DbHelper.ProductCatalog.SetProductCatalogAttributeDelete(AttributeId))
                    DisplayMessage("Atrybut został usunięty. <a href='/ProductCatalog.Attributes.aspx'>Wróć do listy atrybutów</a>");
                else
                    DisplayMessage("Nie udało się usunąć atrybutu. Prawdopodobnie istnieją przypisania do niego.");
            else
                DisplayMessage("Nie udało się usunąć atrybutu. Prawdopodobnie istnieją przypisania do niego.");

        }
    }
}