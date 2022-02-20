<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductsCosts.aspx.cs" Inherits="LajtIt.Web.ProductsCosts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Analiza cen</h1>
    <table border="0">
        <tr>
            <td>Dostawca</td>
            <td>Sklepy</td>
            <td>Marża</td>
        </tr>
        <tr valign="top">
            <td width="200" rowspan="5">
                <asp:ListBox Rows="8" runat="server" ID="lbxSearchSupplier" SelectionMode="Multiple" Width="200" DataTextField="DisplayName" DataValueField="SupplierId"></asp:ListBox>

            </td>
            <td width="200" rowspan="5">
                <asp:ListBox Rows="8" runat="server" ID="lbxShops" SelectionMode="Multiple" DataValueField="ShopId" Width="200" DataTextField="Name"></asp:ListBox>
            </td>
            <td>od
                <asp:TextBox Width="120" runat="server" ID="txbMarzaFrom"></asp:TextBox>
                do
                <asp:TextBox Width="120" runat="server" ID="txbMarzaTo"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Narzut</td>
        </tr>
        <tr valign="top">

            <td>od
                <asp:TextBox Width="120" runat="server" ID="txbNarzutFrom"></asp:TextBox>
                do
                <asp:TextBox Width="120" runat="server" ID="txbNarzutTo"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Data dostawy</td>
        </tr>
        <tr>
            <td>
                od <asp:TextBox runat="server" ID="txbDateFrom" Width="120"></asp:TextBox><asp:CalendarExtender
                    ID="calDateFrom" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateFrom"></asp:CalendarExtender>
                do <asp:TextBox runat="server" ID="txbDateTo" Width="120"></asp:TextBox><asp:CalendarExtender
                    ID="calDateTo" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateTo"></asp:CalendarExtender>
            </td>
            <td>
                <asp:Button runat="server" ID="btnSearch" Text="Szukaj" OnClick="btnSearch_Click" /></td>
        </tr>
    </table>
    <asp:Panel runat="server" GroupingText="Wyniki" ID="pnResults" Visible="false">
        Średnia marża: <asp:Label runat="server" ID="lblMarza"></asp:Label><br />
        Średni narzut: <asp:Label runat="server" ID="lblNarzut"></asp:Label>
    </asp:Panel>
    <asp:GridView runat="server" Width="100%" ID="gvProducts" AutoGenerateColumns="false" OnRowDataBound="gvProducts_RowDataBound" OnSorting="gvProducts_Sorting" AllowSorting="true">
        <Columns>
            <asp:BoundField DataField="InsertDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data dost." ReadOnly="true" />
            <asp:HyperLinkField ItemStyle-Width="200" Target="_blank" DataNavigateUrlFields="ProductCatalogId" DataNavigateUrlFormatString="/ProductCatalog.Delivery.aspx?id={0}" HeaderText="Produkt" DataTextField="Name" />
            <asp:HyperLinkField ItemStyle-Width="60" Target="_blank" DataNavigateUrlFields="OrderId" DataNavigateUrlFormatString="/Order.aspx?id={0}" HeaderText="Zamówienie" DataTextField="OrderId" />
            <asp:BoundField HtmlEncode="false" DataField="SupplierName" HeaderText="Dostawca" ReadOnly="true" />
            <asp:BoundField HtmlEncode="false" DataField="ShopName" HeaderText="Sklep" ReadOnly="true" />
            <asp:BoundField   HtmlEncode="false" ItemStyle-HorizontalAlign="Right" DataField="PurchasePriceNetto" DataFormatString="{0:C}" HeaderText="Cena<br>zakupu<br>netto" ReadOnly="true" />
            <asp:BoundField   HtmlEncode="false" ItemStyle-HorizontalAlign="Right" DataField="SellPriceNetto" DataFormatString="{0:C}" HeaderText="Cena<br>sprzedaży<br>netto" ReadOnly="true" />
            <asp:BoundField HtmlEncode="false" ItemStyle-HorizontalAlign="Center" DataField="Quantity" HeaderText="L.szt." ReadOnly="true" />
            <asp:BoundField Visible="false" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" DataField="TotalPurchaseNetto" DataFormatString="{0:C}" HeaderText="Razem<br>zakup<br>netto" ReadOnly="true" />
            <asp:BoundField Visible="false" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" DataField="TotalSellNetto" DataFormatString="{0:C}" HeaderText="Razem<br>sprzedaż<br>netto" ReadOnly="true" />
            <asp:BoundField Visible="false" SortExpression="Profit" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" DataField="Profit" DataFormatString="{0:C}" HeaderText="Zysk" ReadOnly="true" />
            <asp:BoundField SortExpression="Marza" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" DataField="Marza" DataFormatString="{0:0.00}%" HeaderText="Marża" ReadOnly="true" />
            <asp:BoundField SortExpression="Narzut" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" DataField="Narzut" DataFormatString="{0:0.00}%" HeaderText="Narzut" ReadOnly="true" />
        </Columns>

    </asp:GridView>
</asp:Content>
