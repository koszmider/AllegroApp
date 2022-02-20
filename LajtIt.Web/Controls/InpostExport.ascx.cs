using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class InpostExport : LajtitControl
    { 

        private int OrderId { get { return Convert.ToInt32(Request.QueryString["id"].ToString()); } }

        public delegate void SavedEventHandler(bool enabled);
        public event SavedEventHandler RefreshOrder;

        public string GroupingText { set { pnInpost.GroupingText = value; } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Bind();
        }

        private void Bind()
        {
            string[] codes = new string[] { "LOD35N", "LOD07N", "LOD46N", "LOD08B", "LOD53N", "LOD35A", "LOD22N", "LOD078" };

            ddlInpost.Items.Clear();
            Dal.PaczkomatyHelper ph = new Dal.PaczkomatyHelper();
            ddlInpost.DataSource = ph.GetPaczkomaty().Where(x => codes.Contains(x.Name)).ToList();
            ddlInpost.DataBind();
            ddlInpost.Items.Insert(0, new ListItem("", "0"));
            ddlInpost.Items.Insert(1, new ListItem("--ZAMÓW KURIERA--", ""));

        }



        protected void lbtnExport_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            Bll.OrderHelper oh = new Bll.OrderHelper();


            bool result = false;


            Dal.OrderHelper oh1 = new Dal.OrderHelper();
            oh1.SetOrderTrackingNumberClear(OrderId, ddlInpost.SelectedValue, UserName);

            //result = oh.ExportForInPost(ddlInpost.SelectedValue, orderId, UserName);
            result = true;
            if (result)
            {
                DisplayMessage(String.Format("Nowa etykieta została utworzona. Będzie gotowa do pobrania w ciągu kilkunastu sekund"));
              
                if (RefreshOrder != null)
                    RefreshOrder(true);
            }
            else
                DisplayMessage(String.Format("Błąd generowania etykiet"));
        } 
    }
}
