using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("4eb66b40-70e3-440c-ad09-8828802035d2")]
    public partial class AllegroDeliveryCost : LajtitPage
    {
        private int? DeliveryCostTypeId
        {
            get
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                    return Convert.ToInt32(Request.QueryString["id"].ToString());
                else
                    return null;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindDeliveryCost();
        }


        protected void txbSave_Click(object sender, EventArgs e)
        {
            List<Dal.AllegroDeliveryCost> deliveries = new List<Dal.AllegroDeliveryCost>();
            Dal.AllegroDeliveryCostType deliveryCostType = new Dal.AllegroDeliveryCostType();


            foreach (GridViewRow row in gvDeliveries.Rows)
            {
                if (((CheckBox)row.FindControl("chbIsActive")).Checked)
                {

                    Dal.AllegroDeliveryCost d = new Dal.AllegroDeliveryCost()
                        {
                            BaseCost = Convert.ToDecimal(((TextBox)row.FindControl("txbBaseCost")).Text),
                            Name = row.Cells[0].Text,
                            NextItemCost = Convert.ToDecimal(((TextBox)row.FindControl("txbNextItemCost")).Text),
                            Quantity = Convert.ToInt32(((TextBox)row.FindControl("txbQunatity")).Text),
                            FieldId = Convert.ToInt32(gvDeliveries.DataKeys[row.RowIndex][0])
                        };
                     
                    if (((DropDownList)row.FindControl("ddlAllegroDeliveryMethod")).SelectedIndex!=0)
                        d.AllegroDeliveryMethodId = Guid.Parse(((DropDownList)row.FindControl("ddlAllegroDeliveryMethod")).SelectedValue);

                    deliveries.Add(d);
                }
            }





            Dal.OrderHelper oh = new Dal.OrderHelper();
           
            try
            {
                oh.SetAllegroDeliveries(DeliveryCostTypeId, txbName.Text.Trim(), chbIsActive.Checked, chbIsPaczkomatAvailable.Checked, deliveries);

                DisplayMessage("Zmiany zostały zapisane");
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
            }
        }


        List<Bll.AllegroRESTHelper.DeliveryMethod> allegroDeliveryMethods;
        private void BindDeliveryCost()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            if (DeliveryCostTypeId.HasValue)
            {
                Dal.AllegroDeliveryCostType ct = oh.GetAllegroDeliveryCostTypes()
                    .Where(x => x.DeliveryCostTypeId == DeliveryCostTypeId.Value).FirstOrDefault();

                txbName.Text = ct.Name.Trim();
                chbIsActive.Checked = ct.IsActive;
                chbIsPaczkomatAvailable.Checked = ct.IsPaczkomatAvailable;
                imgOK.Visible = ct.IsPublished.HasValue && ct.IsPublished.Value;
                imgFalse.Visible = !imgOK.Visible;

                btnPublish.Visible = true;

                gvAllegroUsers.DataSource = oh.GetAllegroDeliveryCostTypeForAllegro(DeliveryCostTypeId.Value);
                gvAllegroUsers.DataBind();
            }
            List<Dal.AllegroDeliveryCostsByIdResult> deliveries = oh.GetAllegroDeliveryCosts(DeliveryCostTypeId);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            allegroDeliveryMethods = Bll.AllegroRESTHelper.GetDeliveryMethods().OrderBy(x=>x.name).ToList();

            gvDeliveries.DataSource = deliveries;
            gvDeliveries.DataBind();

        }

        protected void gvDeliveries_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.AllegroDeliveryCostsByIdResult adc = e.Row.DataItem as Dal.AllegroDeliveryCostsByIdResult;

                ((TextBox)e.Row.FindControl("txbBaseCost")).Text = String.Format("{0:0.00}", adc.BaseCost);
                ((TextBox)e.Row.FindControl("txbQunatity")).Text = String.Format("{0}", adc.Quantity);
                ((TextBox)e.Row.FindControl("txbNextItemCost")).Text = String.Format("{0:0.00}", adc.NextItemCost);

                ((CheckBox)e.Row.FindControl("chbIsActive")).Checked = adc.DeliveryCostTypeId != 0;


                DropDownList ddlAllegroDeliveryMethod = e.Row.FindControl("ddlAllegroDeliveryMethod") as DropDownList;

                ddlAllegroDeliveryMethod.DataSource = allegroDeliveryMethods;
                ddlAllegroDeliveryMethod.DataBind();

                if (adc.AllegroDeliveryMethodId.HasValue)
                    ddlAllegroDeliveryMethod.SelectedValue = adc.AllegroDeliveryMethodId.Value.ToString();


            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {

            bool result = Bll.AllegroRESTHelper.SetDeliveryMethod(DeliveryCostTypeId.Value);


            DisplayMessage("Zmiany zostały opublikowane");
            BindDeliveryCost();
        }
    }
}