using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("33cc670e-5c97-4d76-8d44-147c37c39a82")]
    public partial class Offers : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindOfferStatuses();
                BindOffers();
            }
        }

        private void BindOfferStatuses()
        {
            Dal.OfferHelper oh = new Dal.OfferHelper();
        

            chblOfferStatus.DataSource = oh.GetOfferStatuses();
            chblOfferStatus.DataBind();

            chblOfferStatus.SelectedValue = "1";
        }

        private void BindOffers()
        {
            Dal.OfferHelper oh = new Dal.OfferHelper();
            int[] statuses = chblOfferStatus.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToArray();
            gvOffers.DataSource = oh.GetOffers(txbSearch.Text.Trim(), statuses)
                .Select(x=> new  {
                    OfferId=x.OfferId,
                    Name=x.Name,
                    ContactName=x.ContactName,
                    Email=x.Email,
                    Phone=x.Phone,
                    InsertDate=x.InsertDate,
                    InsertUser=x.InsertUser,
                    StatusName = x.OfferStatus.Name
                })
                .ToList();
            gvOffers.DataBind();
             
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Dal.OfferHelper oh = new Dal.OfferHelper();
            int offerId = oh.SetOfferNew(txbName.Text.Trim(), UserName);

            Response.Redirect(String.Format("offer.aspx?id={0}", offerId));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Dal.OfferHelper oh = new Dal.OfferHelper();

            if(txbSearchNumber.Text.Trim()!="")
            {
                Dal.OfferVersion ov = oh.GetOfferVersionByNumber(txbSearchNumber.Text.Trim());

                if (ov != null)
                {
                    DisplayMessage(String.Format("Znaleziono ofertę numer <b>{0}</b>. <a href='offer.aspx?id={1}&v={0}'>Kliknij tutaj aby ją przejrzeć</a>",
                        txbSearchNumber.Text.Trim(), ov.OfferId));
                }
                else
                    DisplayMessage(String.Format("Nie znaleziono żadnej oferty o numerze <b>{0}</b>", txbSearchNumber.Text));
            }

            else

            BindOffers();

        }
    }
}