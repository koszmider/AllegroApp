using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using LajtIt.Bll;
using System.Linq;
using System.Web.UI;
using System.Drawing;

namespace LajtIt.Web.Controls
{
    public partial class UpdateGrid : LajtitControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void BindUpdates()
        {
            Dal.PromoHelper ph = new Dal.PromoHelper();
            List<Dal.Update> updates = ph.GetAllUpdates();

            gvPromos.DataSource = updates;
            gvPromos.DataBind();
        }


        protected void gvPromos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litDesc = e.Row.FindControl("litDesc") as Literal;
                Literal litStartDate = e.Row.FindControl("litStartDate") as Literal;
                Literal litIsActive = e.Row.FindControl("litIsActive") as Literal;
                Literal litFileId = e.Row.FindControl("litFileId") as Literal;

                Dal.Update upd = e.Row.DataItem as Dal.Update;

                litDesc.Text = upd.Description;
                litStartDate.Text = upd.StartDate.ToString();
                litFileId.Text = upd.FileId.ToString();
                if (upd.IsActive)
                    litIsActive.Text = "Tak";
                else
                    litIsActive.Text = "Nie";
            }

        }

        protected void gvPromos_OnRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("IdDelete"))
            {
                int updateId = Convert.ToInt32(e.CommandArgument);

                Bll.PromoHelper phbll = new Bll.PromoHelper();
                Dal.PromoHelper ph = new Dal.PromoHelper();
                
                Dal.Update upd = ph.GetUpdate(updateId);
                ph.DeleteUpdate(upd);
                Response.Redirect("Promotions.aspx");
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {

        }
    }
}