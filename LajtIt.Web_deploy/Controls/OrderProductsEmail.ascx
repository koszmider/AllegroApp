<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderProductsEmail.ascx.cs" Inherits="LajtIt.Web.Controls.OrderProductsEmail" %>

<table>
    <tr>
        <td style="width:150px;">Email</td>
        <td><asp:TextBox runat="server" ID="txbEmail" Width="300"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Temat</td>
        <td>
            <asp:TextBox runat="server" ID="txbTitle" Width="300"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Dostawa do:</td>
        <td>
            <asp:RadioButtonList ID="rblWarehouse" runat="server" DataValueField="WarehouseId"
                DataTextField="Name"
                RepeatDirection="Horizontal">
            </asp:RadioButtonList></td>
    </tr>
    <tr>
        <td>Treść</td>
        <td>
            <asp:TextBox runat="server" ID="txbBody" Columns="80" Rows="20" TextMode="MultiLine"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button runat="server" ID="btnSend" Text="Wyślij" OnClick="btnSend_Click" OnClientClick="return confirm('Zapisać zamówienie i wysłać maila?')" />
            <asp:LinkButton runat="server" ID="btnCancel" Text="Anuluj" OnClick="btnCancel_Click" /></td>
    </tr>

</table>
