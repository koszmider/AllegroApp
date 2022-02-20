<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductCatalogOnAllegroNoSellControl.ascx.cs" Inherits="LajtIt.Web.Controls.ProductCatalogOnAllegroNoSellControl" %>
<asp:GridView runat="server" AutoGenerateColumns="false" ID="gvProducts">
    <Columns>
        <asp:HyperLinkField DataNavigateUrlFields="ProductCatalogId" DataNavigateUrlFormatString="/ProductCatalog.Allegro.aspx?id={0}"
            DataTextField="Name" HeaderText="Produkt" />
        <asp:BoundField DataField="LastSold" HeaderText="Ostatnio sprzedane X aukcji temu" ItemStyle-HorizontalAlign="Center" />
    </Columns>
</asp:GridView>
