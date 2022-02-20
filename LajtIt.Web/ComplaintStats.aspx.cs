using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace LajtIt.Web
{
    [Developer("63ade047-7971-4338-b1e3-5a7d880eaf79")]
    public partial class ComplaintStats : LajtitPage
    {
        int total = 0;
        int count = 0;
        decimal sum = 0;
        Dictionary<string, string> complaints;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { 
                BindOrderComplaintsByMonth();
            }
        }
        protected void gvOrderComplaintsByMonth_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {

                Dal.ComplaintsStats cs = e.Row.DataItem as Dal.ComplaintsStats;
                foreach (TableCell cell in e.Row.Cells)//.Controls.Cast<Control>().ToList())
                {
                    foreach (Control control in cell.Controls)
                        if (control.ID != null && control.ID.StartsWith("lbl"))
                        {
                            string field = control.ID.Replace("lbl", "");
                            
                                (control as Label).Text = complaints[field.ToString()];
                            
                             

                        }
                }
            }
                if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Dal.ComplaintsStats cs = e.Row.DataItem as Dal.ComplaintsStats;


                Label lblC1 = e.Row.FindControl("lblC1") as Label;
                Label lblC2 = e.Row.FindControl("lblC2") as Label;
                Label lblC3 = e.Row.FindControl("lblC3") as Label;
                Label lblC4 = e.Row.FindControl("lblC4") as Label;
                Label lblC5 = e.Row.FindControl("lblC5") as Label;
                Label lblC6 = e.Row.FindControl("lblC6") as Label;
                Label lblC7 = e.Row.FindControl("lblC7") as Label;
                Label lblC8 = e.Row.FindControl("lblC8") as Label;
                Label lblC9 = e.Row.FindControl("lblC9") as Label;
                Label lblC10 = e.Row.FindControl("lblC10") as Label;


                lblC1.Text = String.Format("{0}/{1:0.00}%", cs.C1, cs.C1*100.00/cs.Count);
                lblC2.Text = String.Format("{0}", cs.C2);
                lblC3.Text = String.Format("{0}", cs.C3);
                lblC4.Text = String.Format("{0}", cs.C4);
                lblC5.Text = String.Format("{0}", cs.C5);
                lblC6.Text = String.Format("{0}", cs.C6);
                lblC7.Text = String.Format("{0}", cs.C7);
                lblC8.Text = String.Format("{0}/{1:0.00}%", cs.C8, cs.C8 * 100.00 / cs.Count);
                lblC9.Text = String.Format("{0}", cs.C9);
                lblC10.Text = String.Format("{0}/{1:0.00}%", cs.C10, cs.C10 * 100.00 / cs.Count);



            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //e.Row.Cells[0].Text = "Razem";
                //e.Row.Cells[1].Text = String.Format("{0:C}", sum);
                //e.Row.Cells[3].Text = String.Format("{0}", count);
                //if(total!=0)
                //e.Row.Cells[4].Text = String.Format("{0:0.00}%", count*100.00/total);

            }
        }
   
        
     
        private void BindOrderComplaintsByMonth()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            complaints = new Dictionary<string, string>();
            foreach (Dal.OrderComplantType oct in oh.GetOrderComplaintTypes())
                complaints.Add(String.Format("C{0}", oct.OrderComplaintTypeId), oct.ComplaintType);

            gvOrderComplaintsByMonth.DataSource = oh.GetOrderComplaintsByMonth();
            gvOrderComplaintsByMonth.DataBind();
        }
      

        
    }
}