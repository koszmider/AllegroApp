using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web.Controls
{
    public partial class ShippingTypesControl : System.Web.UI.UserControl
    {
        private bool editExtraData = true;

        public bool EditExtraData { set { editExtraData = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void Bind(Dal.Helper.ShippingCompany shippingCompany)
        {
            switch(shippingCompany)
            {
                case Dal.Helper.ShippingCompany.Dpd:
                    txbDpdParcels.Enabled = true;
                    break;
                case Dal.Helper.ShippingCompany.InPost:
                    ddlPaczkomat.Enabled = true;
                    ddlPaczkomatGabaryt.Enabled = true;
                    break;
            }

        }

        internal string GetShippingData()
        {
            string shippingData = null;
            if (pDpd.Visible)
            {
                if (!lblWeight.Visible)
                    shippingData = String.Format("{0}", txbDpdParcels.Text.Trim());
                else
                    shippingData = String.Format("{0}|{1}", txbDpdParcels.Text.Trim(), rblDpdWeight.SelectedValue);
              
                pDpd.Visible = false;
            }
            if (pInPost.Visible)
            {
                shippingData = String.Format("{0}|{1}", ddlPaczkomat.SelectedValue, ddlPaczkomatGabaryt.SelectedValue);
                pInPost.Visible = false;
            }
            return shippingData;
        }

        internal void SetShippingExtraData(Dal.Order order, Dal.Helper.ShippingCompany shippingCompany, string shippingData)
        {
            pDpd.Visible = pInPost.Visible = false;

            switch (shippingCompany)
            {  
                case Dal.Helper.ShippingCompany.Dpd:
                    pDpd.Visible = true;

                    if(order.ShipmentCountryCode=="PL")
                    {
                        txbDpdParcels.Text = shippingData;
                        break;
                    }

                    lblWeight.Visible = true;
                    rblDpdWeight.Visible = true;

                    string[] sd = new string[] { };
                    if (!String.IsNullOrEmpty(shippingData))
                        sd = shippingData.Split(new char[] { '|' });
                    if (sd.Length == 2)
                    {
                        txbDpdParcels.Text = sd[0];
                        rblDpdWeight.SelectedIndex = rblDpdWeight.Items.IndexOf(rblDpdWeight.Items.FindByValue(sd[1]));
                    }
             
                    break; 
                case  Dal.Helper.ShippingCompany.InPost:
                    pInPost.Visible = true;
                    ddlPaczkomat.Items.Clear();
                    ddlPaczkomat.Items.Add("");
                    Dal.PaczkomatyHelper ph = new Dal.PaczkomatyHelper();
                    ddlPaczkomat.DataSource = ph.GetPaczkomaty();
                    ddlPaczkomat.DataBind();
                    ddlPaczkomat.Enabled = editExtraData;

                    if (shippingData != null)
                    {
                        string[] w = shippingData.Split('|');
                        ddlPaczkomat.SelectedIndex = ddlPaczkomat.Items.IndexOf(ddlPaczkomat.Items.FindByValue(w[0]));
                        ddlPaczkomatGabaryt.SelectedIndex = ddlPaczkomatGabaryt.Items.IndexOf(ddlPaczkomatGabaryt.Items.FindByValue(w[1]));
                    }
                    break;
            }
        }

        internal void SetEnabled(bool edit)
        {
            pDpd.Enabled = pInPost.Enabled = edit;
        }
    }
}