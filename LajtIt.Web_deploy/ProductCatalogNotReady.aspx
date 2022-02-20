<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    CodeBehind="ProductCatalogNotReady.aspx.cs" Inherits="LajtIt.Web.ProductCatalogNotReady" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <h1>Produkty do konfiguracji</h1>
    <br />
    <asp:GridView runat="server" ID="gvSuppliers" AutoGenerateColumns="false">
        <Columns>
            <asp:HyperLinkField HeaderText="Dostawca" DataTextField="Name"  
                DataNavigateUrlFields="SupplierId" Target="_blank" DataNavigateUrlFormatString="/ProductCatalogForDb.aspx?SupplierId={0}&action=notready" ItemStyle-Width="200" />
            <asp:BoundField DataField="ProductCount" HeaderText="Dostępne" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="200"/>
            <asp:BoundField DataField="IsNotReadyCount" HeaderText="Nieskonfigurowane" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="200"/>
            <asp:BoundField DataField="ImportType" HeaderText="Typ integracji" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="200"/>
            <asp:BoundField DataField="LastImportDate" HeaderText="Ostatnia aktualizacja"  ItemStyle-Width="200" DataFormatString="{0:yyyy/MM/dd HH:mm}" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>

</asp:Content>
