<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shops.aspx.cs" Inherits="LajtIt.Web.Shops" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Zarządzanie sklepami</h1><br />
    <asp:GridView runat="server" ID="gvShops" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField HeaderText="Lp" ItemStyle-Width="50px">
                <HeaderTemplate>
                    <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("ShopId") %>'></asp:Literal>.
                           
                <asp:Literal runat="server" ID="LitId"></asp:Literal>
                    <asp:CheckBox runat="server" ID="chbOrder" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField HeaderText="Nazwa sklepu" DataNavigateUrlFormatString="/Shop.aspx?id={0}" DataNavigateUrlFields="ShopId" 
                DataTextField="Name" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Aktywny" />
        </Columns>
    </asp:GridView>
</asp:Content>
