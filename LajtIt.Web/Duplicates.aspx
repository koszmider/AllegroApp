<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Duplicates.aspx.cs" Inherits="LajtIt.Web.Duplicates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Duplikaty Ean</h1>
    <asp:DropDownList runat="server" ID="ddlDuplicates" OnSelectedIndexChanged="ddlDuplicates_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <asp:Button runat="server" ID="btnSearch" Text="Pokaż" OnClick="ddlDuplicates_SelectedIndexChanged" />

    <asp:GridView runat="server" ID="gvDuplicates" AutoGenerateColumns="false" DataKeyNames="ProductCatalogId"
        OnRowDataBound="gvDuplicates_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="Wybór">
                <ItemTemplate>
                    <asp:RadioButton ID="rbRowSelector" runat="server" /></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle Width="120" />
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlPreview" Target="_blank" NavigateUrl="/ProductCatalog.Preview.aspx?id={0}&idSupplier={1}">
                        <asp:Image runat="server" ID="imgImage" Width="100" />
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField DataNavigateUrlFields="ProductCatalogId" DataNavigateUrlFormatString="/Product.aspx?id={0}" Target="_blank"
                DataTextField="Name" HeaderText="Produkt" />
            <asp:BoundField DataField="Ean" HeaderText="Ean" />
            <asp:BoundField DataField="Code" HeaderText="Kod" />
            <asp:BoundField DataField="ExternalId" HeaderText="ExtId" />
            <asp:BoundField DataField="PriceBruttoFixed" HeaderText="Cena" DataFormatString="{0:C}" />
            <asp:BoundField DataField="SupplierQuantity" HeaderText="Qty" />
            <asp:BoundField DataField="SupplierName" HeaderText="Dostawca" />
            <asp:HyperLinkField DataNavigateUrlFields="ShopProductId" DataNavigateUrlFormatString="http://lajtit.pl/pl/p/p/{0}" Target="_blank"
                DataTextField="ShopProductId" HeaderText="Sklep" />
            <asp:BoundField DataField="AllegroCount" HeaderText="Allegro" />
            <asp:BoundField DataField="OrdersCount" HeaderText="Zamówienia" />
            <asp:BoundField DataField="InvoiceCount" HeaderText="Faktura" />
            <asp:BoundField DataField="IsDiscontinued" HeaderText="Wycofany" />
            <asp:BoundField DataField="IsHidden" HeaderText="Ukryty" />
            <asp:BoundField DataField="IsAvailable" HeaderText="Aktywny" />

        </Columns>

    </asp:GridView>
    <asp:Button Visible="false" runat="server" ID="btnDelete" OnClick="btnDelete_Click" Text="Zapisz i usuń duplikaty" OnClientClick="return confirm('Usuń duplikaty');" />

</asp:Content>
