using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using System.Security.Cryptography;

namespace LajtIt.Web
{
    [Developer("afd4997e-82da-4fc7-ab2b-bc2e61b3be51")]
    public partial class WebApiTestPage : LajtitPage
    {
        protected void Page_Load(object se, EventArgs e)
        {
            using (SHA256 sha256 = new SHA256Managed())
            {
                byte[] passwordHash = sha256.ComputeHash(StrToByteArray("ddd"));
                string encodedPassword = Convert.ToBase64String(passwordHash);
            }
        }
        private static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
        //    try
        //    {
        //        long itemId = Convert.ToInt64(txbItemId.Text.Trim());

        //        Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.JacekStawicki.ToString());
        //        Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.CzerwoneJablko.ToString());


        //        AllegroHelper ah = new AllegroHelper();

        //        Bll.AllegroNewWCF.SiteJournal[] journals = ah.doGetSiteJournal(0, ddlUserId.SelectedItem.Text).ToArray();
        //        journals = journals.Where(x => x.itemId == itemId).ToArray();
        //        gvGetSiteJournal.DataSource = journals;
        //        gvGetSiteJournal.DataBind();



        //         Bll.AllegroNewWCF.SiteJournalDealsStruct[] journalDeals = ah.doGetSiteJournalDeals(0, ddlUserId.SelectedItem.Text);

        //        journalDeals = journalDeals.Where(x => x.dealItemId == itemId).ToArray();
        //        gvGetSiteJournalDeal.DataSource = journalDeals;
        //        gvGetSiteJournalDeal.DataBind();


        //        foreach (Bll.AllegroNewWCF.SiteJournalDealsStruct journalDeal in journalDeals)
        //        {

        //            switch (journalDeal.dealEventType)
        //            {
        //                case 2:
        //                    List<Bll.AllegroNewWCF.PostBuyFormDataStruct> buys = ah.GetItemTransactionsForTransactionsIDs(ddlUserId.SelectedItem.Text,
        //                    new long[] { journalDeal.dealTransactionId });


        //                    gvPostBuyFormDataStruct.DataSource = buys;
        //                    gvPostBuyFormDataStruct.DataBind();


        //                    break;
        //                case 4:
        //                    //     updateDeal = GetPayment(journalDeal);
        //                    break;
        //            }

        //        }

        //        DisplayMessage("Koniec");

        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayMessage(ex.Message);
        //    }

        }
    }
}