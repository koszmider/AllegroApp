using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("de008a99-826b-4383-bba1-8c49f9f1d3c7")]
    public partial class Complaints : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindStatuses();
                BindComplaintTypes();
                BindComplaints("InsertDate"  , SortDirection.Ascending);
            }
        }

        private void BindComplaintTypes()
        {
            ddlSuppliers.DataSource = Dal.DbHelper.ProductCatalog.GetSuppliers().OrderByDescending(x => x.SupplierId).OrderBy(x=>x.Name).ToList();
            ddlSuppliers.DataBind();
            Dal.OrderHelper oh = new Dal.OrderHelper();
            ddlComplaintType.DataSource = oh.GetOrderComplaintTypes();
            ddlComplaintType.DataBind();
        }

        private void BindStatuses()
        {
            Dal.OrderHelper oh = new Dal.OrderHelper();
            List<Dal.ComplaintStatus> statuses = oh.GetComplaintStatuses().Where(x => x.ChangesStatus).ToList() ;
            
            foreach(Dal.ComplaintStatus status in statuses)
            {
                ListItem li = new ListItem(status.Name, status.ComplaintStatusId.ToString());
                li.Selected = status.IsStatusOpen;
                ddlComplaintStatus.Items.Add(li);
            }



        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
       


            BindComplaints("InsertDate",  SortDirection.Ascending);
        }
        private void BindComplaints(string sort, SortDirection sortDirection)
        {
            string sproductCatalogId = Request.Form[hfProductCatalogId.UniqueID];
            int productCatalogId = 0;

            if ( txbProductCode.Text != "" && !Int32.TryParse(sproductCatalogId, out productCatalogId))
            {
                DisplayMessage("Produkt nie istnieje");
                return;
            }

            if (sort != "")
                ViewState["sort"] = sort;
            else
    if (ViewState["sort"] != null)
                sort = ViewState["sort"].ToString();


            Dal.OrderHelper oh = new Dal.OrderHelper();

            int[] complaintTypeIds = ddlComplaintType.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => Int32.Parse(x.Value)).ToArray();

            var c = oh.GetComplaints(
                ddlComplaintStatus.Items.Cast<ListItem>().Where(x=>x.Selected).Select(x=>Int32.Parse(x.Value)).ToArray(),
                txbOrderId.Text,
                txbClientName.Text 
                )
                .Select(x =>
                new
                {
                    x.Comment,
                    OrderComplaintStatus = x.ComplaintStatus.Name,
                    x.ComplaintStatusId,
                    x.Cost,
                    x.Id,
                    x.InsertDate,
                    x.InsertUser,
                    x.OrderId,
                    ClientName = String.Format("{0} {1} {2}", x.Order.ShipmentCompanyName, x.Order.ShipmentFirstName, x.Order.ShipmentLastName),
                    OrderComplaintType = x.OrderComplantType.ComplaintType,
                    x.OrderComplaintTypeId,
                    x.InvoiceCorrectionExpected,
                    x.LastUpdateDate
                }
                )
                .ToList();
            if (complaintTypeIds.Count() > 0)
                c = c.Where(x => complaintTypeIds.Contains(x.OrderComplaintTypeId)).ToList();

            if (productCatalogId != 0)
            {
                int[] selectedOrderIds = c.Where(x=>x.OrderId.HasValue).Select(x => x.OrderId.Value).ToArray();
                int[] complaintIds = Dal.DbHelper.Orders.GetOrderComplaintsWithProduct(selectedOrderIds, productCatalogId);
                c = c.Where(x => complaintIds.Contains(x.Id)).ToList();

            }
            if (ddlSuppliers.SelectedIndex != 0)
            {
                int[] selectedOrderIds = c.Where(x => x.OrderId.HasValue).Select(x => x.OrderId.Value).ToArray();
                int[] complaintIds = Dal.DbHelper.Orders.GetOrderComplaintsWithSupplier(selectedOrderIds, Int32.Parse(ddlSuppliers.SelectedValue));
                c = c.Where(x => complaintIds.Contains(x.Id)).ToList();

            }
            if(chbInvoiceCorrectionExpected.Checked)
                c = c.Where(x => x.InvoiceCorrectionExpected).ToList();



            switch (sort)
            {
                case "InsertDate": gvComplaints.DataSource = c.OrderByDescending(x => x.InsertDate).ToList(); break;
                case "LastUpdateDate": gvComplaints.DataSource = c.OrderBy(x => x.LastUpdateDate).ToList(); break;
         
                default: gvComplaints.DataSource = c.OrderBy(x => x.InsertDate).ToList(); break;

            }


            
            gvComplaints.DataBind();
        }

        protected void gvComplaints_Sorting(object sender, GridViewSortEventArgs e)
        {
            BindComplaints(e.SortExpression, e.SortDirection);
        }
    }
}