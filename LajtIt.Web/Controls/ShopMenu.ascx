<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopMenu.ascx.cs"
    Inherits="LajtIt.Web.Controls.ShopMenu" %>
<style>
    .tabSelected {
        background-color: white;
    }
</style>
<h2>
    <asp:Literal runat="server" ID="litProductCatalog"></asp:Literal></h2>    <div style="text-align:right"><a href="/shops.aspx">wróć do listy</a></div>


<table style="border: solid 1px black; font-size: 12pt; background-color: Silver; margin: 10px; width: 100%;">
    <tr>
        <td runat="server" id="td1" style="width: 200px; text-align: center;">
            <asp:HyperLink runat="server" ID="hl1" NavigateUrl="/Shop.aspx?id={0}">Dane podstawowe</asp:HyperLink>
        </td>
        <td runat="server" id="td2" style="width: 200px; text-align: center;">
            <asp:HyperLink runat="server" ID="hl2" NavigateUrl="/Shop.Suppliers.aspx?id={0}">Dostawcy</asp:HyperLink>
        </td>

        <td></td>
    </tr>
</table>
