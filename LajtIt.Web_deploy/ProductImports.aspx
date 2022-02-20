<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductImports.aspx.cs"
    Inherits="LajtIt.Web.ProductImports" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Importy</h1>
    <asp:GridView ID="gvImports" AutoGenerateColumns="false" runat="server" >
        <Columns>
            <asp:BoundField DataField="ImportId"  ReadOnly="true"/>
            <asp:HyperLinkField DataNavigateUrlFields="ImportId" DataTextField="Name" DataNavigateUrlFormatString="ProductImport.aspx?id={0}" HeaderText="Opis" />

             

            <asp:BoundField DataField="ImportDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data zamówienia" />
            <asp:BoundField DataField="ImportDeliveryDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data dostawy" />
            <asp:CheckBoxField DataField="IsActive"  HeaderText="Aktywny" />
        </Columns>
    </asp:GridView>
</asp:Content>
