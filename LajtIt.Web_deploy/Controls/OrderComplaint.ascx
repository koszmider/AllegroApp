<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderComplaint.ascx.cs"
    Inherits="LajtIt.Web.Controls.OrderComplaint" %>
<asp:GridView runat="server" ID="gvOrderCompaints" AutoGenerateColumns="false" OnRowEditing="gvOrderCompaints_OnRowEditing"
    OnRowDataBound="gvOrderCompaints_OnRowDataBound" Width="100%"
    OnRowCancelingEdit="gvOrderCompaints_OnRowCancelingEdit" DataKeyNames="ComplaintId"
    OnRowUpdating="gvOrderCompaints_OnRowUpdating">
    <Columns>
        <asp:HyperLinkField DataNavigateUrlFields="ComplaintId" DataTextField="ComplaintId" DataNavigateUrlFormatString="/Complaint.aspx?id={0}" ItemStyle-Width="30" HeaderText="Id" />
        <asp:BoundField DataField="InsertDate" HeaderText="Data" DataFormatString="{0:yy/MM/dd HH:mm}" ItemStyle-Width="60"
            ReadOnly="true" />
        <asp:TemplateField HeaderText="Powód reklamacji" ItemStyle-Width="100" >
            <ItemTemplate >
                <asp:Label runat="server" Text='<%# Eval("ComplaintType") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlOrderComplaintType" DataValueField="OrderComplaintTypeId"
                    DataTextField="ComplaintType">
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Comment" HeaderText="Uwagi" ItemStyle-Width="200" />
    </Columns>
</asp:GridView>
