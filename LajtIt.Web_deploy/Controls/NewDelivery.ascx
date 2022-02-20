<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewDelivery.ascx.cs" Inherits="LajtIt.Web.Controls.NewDelivery" %>
<table>
    <tr>
        <td>Magazyn:</td>
        <td><asp:DropDownList runat="server" ID="ddlWarehouse" DataValueField="WarehouseId" DataTextField="Name">
</asp:DropDownList> </td>
    </tr>
    <tr>
        <td>Dodaj do zamówienia:</td>
        <td><asp:TextBox runat="server" ID="txbCount" MaxLength="4" Width="40"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Dodaj na magazyn:</td>
        <td><asp:TextBox runat="server" ID="txbCountWarehouse" MaxLength="4" Width="40"></asp:TextBox></td>
    </tr>
</table>



