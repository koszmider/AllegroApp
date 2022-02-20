<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShopProducts.aspx.cs" Inherits="LajtIt.Web.ShopProducts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Produkty w sklepie, nie w systemie</h2>

    <asp:Button runat="server" ID="btnShopProducts" Text="Produkty w sklepie, nie w systemie" OnClick="btnShopProducts_Click"/>
    <asp:GridView runat="server" ID="gvProductsInShop"></asp:GridView>
    <asp:Button runat="server" ID="btnShopProductsDeleteNotExisingInSystem" Text="Usuń" OnClick="btnShopProductsDeleteNotExisingInSystem_Click"/>

    
    <h2>Produkty w systemie, nie w sklepie</h2>
</asp:Content>
