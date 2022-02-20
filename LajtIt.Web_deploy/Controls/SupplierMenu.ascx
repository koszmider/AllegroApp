<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierMenu.ascx.cs"
    Inherits="LajtIt.Web.Controls.SupplierMenu" %>
<style>
    .tabSelected {
        background-color: white;
    }
</style>
<h2>
    <asp:Literal runat="server" ID="litProductCatalog"></asp:Literal></h2>
<div style="text-align:right"><a href="/Suppliers.aspx">wszyscy producenci</a></div>
<table style="border: solid 1px black; font-size: 12pt; background-color: Silver; margin: 10px; width: 100%;">
    <tr>
        <td runat="server" id="td1" style="width:200px; text-align:center;">
            <asp:HyperLink runat="server" ID="hl1" NavigateUrl="/Supplier.aspx?id={0}">Dane podstawowe</asp:HyperLink>
        </td>
        <td runat="server" id="td2" style="width:200px; text-align:center;">
            <asp:HyperLink runat="server" ID="hl2" NavigateUrl="/Supplier.Shops.aspx?id={0}">Sklepy</asp:HyperLink>
        </td>
        <td runat="server" id="td3" style="width:200px; text-align:center;">
            <asp:HyperLink runat="server" ID="hl3" NavigateUrl="/Supplier.Descriptions.aspx?id={0}">Opisy</asp:HyperLink>
        </td>
        <td></td>
    </tr>
</table>
