<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="Complaint.aspx.cs" Inherits="LajtIt.Web.Complaint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table style="width:100%">
        
        <tr>
            <td colspan="2" style="text-align: right;"><a href="Complaints.aspx">powrót do zgłoszeń</a></td>
        </tr>
        <tr>
            <td style="width:150px">Data zgłoszenia</td>
            <td>
                <asp:Label runat="server" ID="lblInsertDate"></asp:Label></td>
        </tr>
        <tr>
            <td>Zamówienie</td>
            <td>
                <asp:HyperLink runat="server" NavigateUrl="Order.aspx?id={0}" Target="_blank" ID="hlOrder"></asp:HyperLink></td>
        </tr>

        <tr>
            <td>Powód zgłoszenia</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlOrderCompaintType" DataValueField="OrderComplaintTypeId" DataTextField="ComplaintType"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Opis</td>
            <td>
                <asp:Label runat="server" ID="lblComment"></asp:Label></td>
        </tr>
        <tr>
            <td>Dodatkowe uwagi</td>
            <td>
                <asp:TextBox runat="server" Width="400" Rows="6" TextMode="MultiLine" ID="txbComment" ></asp:TextBox></td>
        </tr>
        <tr>
            <td>Wymagana korekta</td>
            <td>
                <asp:CheckBox runat="server" ID="chbInvoiceCorrectionExpected" Text="TAK" /></td>
        </tr>
        <tr>
            <td>Status reklamacji</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlComplaintStatus" DataValueField="ComplaintStatusId" DataTextField="Name"></asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Zapisz" OnClientClick="return confirm('Zapisać?');" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView runat="server" ID="gvComplaintStatusHistory" AutoGenerateColumns="false" Width="100%">
                    <Columns>
                        <asp:BoundField ItemStyle-Width="120" DataField="InsertDate" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="Data" />
                        <asp:BoundField ItemStyle-Width="120" DataField="InsertUser" HeaderText="Dodał" />
                        <asp:BoundField ItemStyle-Width="120" DataField="OrderComplaintStatus" HeaderText="Status" />
                        <asp:BoundField DataField="Comment" HeaderText="Uwagi" HtmlEncode="false" />

                    </Columns>
                </asp:GridView>

            </td>
        </tr>
    </table>
</asp:Content>
