<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllegroUsers.aspx.cs" Inherits="LajtIt.Web.AllegroUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width: 200px; float: left;">
        <table>
            <tr>
                <td>
                    Uzytkownik Allegro :<br />
                    <asp:TextBox runat="server" ID="txbUserName" MaxLength="50" />
                </td>
            </tr>
            <tr>
                <td>
                    Aukcje:<br />
                    <asp:DropDownList runat="server" ID="ddlAuctionType">
                        <asp:ListItem Value="0">Wszystkie</asp:ListItem>
                        <asp:ListItem Selected="True" Value="1">Aktywne</asp:ListItem>
                        <asp:ListItem Value="2">Nieaktywne</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Sprzedaż:<br />
                    <asp:DropDownList ID="ddlSell" runat="server">
                        <asp:ListItem Value="0">Wszystkie</asp:ListItem>
                        <asp:ListItem Value="1" Selected="True">Tak</asp:ListItem>
                        <asp:ListItem Value="2">Nie</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr> 
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnShow" OnClick="btnShow_Click" Text="Pokaż" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:CheckBox runat="server" ID="chbDateRangeFilter" Visible="false" AutoPostBack="true"
            OnCheckedChanged="chbDateRangeFilter_OnCheckedChanged" />
        <asp:GridView runat="server" ID="gvUserAuctions" AutoGenerateColumns="false" AllowSorting="false"
            ShowFooter="true" OnRowDataBound="gvUserAuctions_RowDataBound">
            <Columns>
                <asp:ImageField DataImageUrlField="ImageUrl">
                </asp:ImageField>
                <asp:BoundField DataField="UserName" HeaderText="Użytkownik" SortExpression="UserName" />
                <asp:TemplateField HeaderText="Kategoria">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="litCategory" /><br />
                        <asp:Label runat="server" ID="litPromoted" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFields="ItemId" DataNavigateUrlFormatString="http://allegro.pl/show_item.php?item={0}"
                    DataTextField="Name" HeaderText="Nazwa aukcji" SortExpression="Name" Target="_blank" />
                <asp:TemplateField HeaderText="O/P/H">
                    <ItemTemplate>
                        <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("BidCount")%>'></asp:Literal>/<asp:Literal
                            ID="Literal2" runat="server" Text='<%# Eval("ItemsOrdered")%>'></asp:Literal>/<asp:Literal
                                ID="Literal3" runat="server" Text='<%# Eval("HitCount")%>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BuyNowPrice" HeaderText="Cena szt." SortExpression="BuyNowPrice" />
                <asp:BoundField DataField="ItemsValue" HeaderText="Wartość" SortExpression="ItemsValue" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="litStatus" /></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="LastUpdateDate" HeaderText="Ost. aktualizacja" SortExpression="LastUpdate" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
