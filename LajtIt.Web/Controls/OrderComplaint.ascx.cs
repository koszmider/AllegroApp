using System;
using System.Linq;
using System.Web.UI;
using LajtIt.Bll;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;

namespace LajtIt.Web.Controls
{
    public partial class OrderComplaint : LajtitControl
    {
        public delegate void SavedEventHandler(object sender, EventArgs e);
        public event SavedEventHandler Saved;

        private int OrderId
        {
            set { ViewState["OrderId"] = value; }
            get { return Convert.ToInt32(ViewState["OrderId"]); }
        }
        private bool AllowEdit
        {
            get
            {
                return Convert.ToBoolean(ViewState["AllowEdit"]);
            }
            set
            {
                ViewState["AllowEdit"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void gvOrderCompaints_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowState == DataControlRowState.Edit)
            {
                DropDownList ddlOrderComplaintType = e.Row.FindControl("ddlOrderComplaintType") as DropDownList;

                Dal.OrderHelper oh = new Dal.OrderHelper();
                ddlOrderComplaintType.DataSource = oh.GetOrderComplaintTypes();
                ddlOrderComplaintType.DataBind();

                var o = e.Row.DataItem;
                Type t = o.GetType();
                PropertyInfo pi = t.GetProperty("OrderComplaintTypeId");
                object id = pi.GetValue(o, null);

                ddlOrderComplaintType.SelectedIndex = ddlOrderComplaintType.Items.IndexOf(ddlOrderComplaintType.Items.FindByValue(id.ToString()));
            }

        }
        protected void gvOrderCompaints_OnRowEditing(object sender, GridViewEditEventArgs e )
        {
            gvOrderCompaints.EditIndex = e.NewEditIndex;
            BindOrderComplaints(OrderId, AllowEdit);
        }
        protected void gvOrderCompaints_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvOrderCompaints.EditIndex = -1;
            BindOrderComplaints(OrderId, AllowEdit);
        }
        protected void gvOrderCompaints_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvOrderCompaints.Rows[e.RowIndex];

            Dal.OrderComplaint oc = new Dal.OrderComplaint()
            {
                ComplaintPerson = (row.Cells[4].Controls[0] as TextBox).Text,
                Cost = Convert.ToDecimal((row.Cells[5].Controls[0] as TextBox).Text),
                Comment = (row.Cells[3].Controls[0] as TextBox).Text,
                OrderId = OrderId,
                OrderComplaintTypeId = Convert.ToInt32((row.FindControl("ddlOrderComplaintType") as DropDownList).SelectedValue),
                Id = Convert.ToInt32(gvOrderCompaints.DataKeys[0][row.RowIndex])
            };
            Dal.OrderHelper oh = new Dal.OrderHelper();
            oh.SetOrderComplaintUpdate(oc, UserName);
            gvOrderCompaints.EditIndex = -1;
            BindOrderComplaints(OrderId, AllowEdit);
            if (Saved != null)
                Saved(this, e);
        }
   
        public void BindOrderComplaints(int orderId, bool allowEdit)
        {
            OrderId = orderId;
            AllowEdit = allowEdit;

            Dal.OrderHelper oh = new Dal.OrderHelper();

            var o = oh.GetOrderComplaints(orderId)
                .Select(x => new
                {
                    InsertDate = x.InsertDate,
                    ComplaintType = x.OrderComplantType.ComplaintType,
                    ComplaintPerson = x.ComplaintPerson,
                    Comment = x.Comment,
                    Cost = x.Cost,
                    ComplaintId = x.Id,
                    OrderComplaintTypeId = x.OrderComplaintTypeId
                }).ToList();

            gvOrderCompaints.DataSource = o;
            gvOrderCompaints.DataBind();
        }
    }
}