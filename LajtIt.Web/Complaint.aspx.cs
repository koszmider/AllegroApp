using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("e6716bf5-accd-47d5-a0ea-f663843c6835")]
    public partial class Complaint : LajtitPage
    {
        private int Id { get { return Convert.ToInt32(Request.QueryString["id"].ToString()); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindStatuses();
                BindComplaints();
            }
        }

        private void BindStatuses()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            ddlComplaintStatus.DataSource = oh.GetComplaintStatuses().ToList();
            ddlComplaintStatus.DataBind();
            ddlOrderCompaintType.DataSource = oh.GetOrderComplaintTypes().ToList();
            ddlOrderCompaintType.DataBind();


        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindComplaints();
        }
        private void BindComplaints()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();

            Dal.OrderComplaint oc = oh.GetOrderComplaint(Id);

            lblComment.Text = oc.Comment;
            lblInsertDate.Text = oc.InsertDate.ToLongDateString();
            ddlComplaintStatus.SelectedValue = oc.ComplaintStatusId.ToString();
            ddlOrderCompaintType.SelectedValue = oc.OrderComplaintTypeId.ToString();
            hlOrder.Text = oc.OrderId.ToString();
            hlOrder.NavigateUrl = String.Format(hlOrder.NavigateUrl, oc.OrderId);
            chbInvoiceCorrectionExpected.Checked = oc.InvoiceCorrectionExpected;

            gvComplaintStatusHistory.DataSource = oh.GetComplaintHistory(Id)
                .Select(x =>
                new
                {
                   
                    OrderComplaintStatus = x.ComplaintStatus.Name,
                    x.ComplaintStatusId,
                    x.Id,
                    Comment = GetComment(x.Comment),
                    x.InsertDate,
                    x.InsertUser,                }
                )
                .ToList();

            gvComplaintStatusHistory.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dal.ComplaintStatusHistory csh = new Dal.ComplaintStatusHistory()
            {
                Comment = txbComment.Text.Trim(),
                ComplaintStatusId = Int32.Parse(ddlComplaintStatus.SelectedValue),
                InsertDate = DateTime.Now,
                InsertUser = UserName,
                OrderComplaintId = Id,
                
            };
            Dal.OrderComplaint oc = new Dal.OrderComplaint()
            {
                Id = Id,
                LastUpdateDate = DateTime.Now,
                InvoiceCorrectionExpected = chbInvoiceCorrectionExpected.Checked,
                OrderComplaintTypeId=Int32.Parse(ddlOrderCompaintType.SelectedValue)
            };


            Dal.OrderHelper oh = new Dal.OrderHelper();

            oh.SetOrderComplaint(csh, oc);

            BindComplaints();


            DisplayMessage("Zapisano");
        }
        private string GetComment(string comment)
        {
            if (comment == null)
                return "";
            else
                return comment.Replace("\n", "<br>");
        }
    }
}