<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllegroItemPage.aspx.cs" Inherits="LajtIt.Web.AllegroItemPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        <asp:Label runat="server" ID="lbName"></asp:Label></h1>
    <table>
        <tr>
            <td>
                <asp:CheckBox runat="server" ID="chbAutoBidEnabled" />
            </td>
            <td>
                Automatyczne tworzenie zakupów
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Zapisz zmiany" />
            </td>
        </tr>
    </table>

    <asp:Panel runat="server" GroupingText="Lista ofert">
    
    <asp:GridView runat="server" ID="gvAllegroItemOrders" AutoGenerateColumns="false" OnRowDataBound="gvAllegroItemOrders_OnRowDataBound">
        <Columns>
            <asp:BoundField DataField="OrderDateTime" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
            <asp:HyperLinkField DataNavigateUrlFields="UserId" DataNavigateUrlFormatString="http://allegro.pl/listing/user/listing.php?us_id={0}" Target="_blank" DataTextField="UserName" />
            <asp:BoundField DataField="ItemPrice" DataFormatString="{0:C}" HeaderText="Cena" />
            <asp:BoundField DataField="ItemsOrdered" HeaderText="Szt." />
            <asp:CheckBoxField DataField="IsMyBid" HeaderText="Zakup własny" />
            <asp:TemplateField HeaderText="">
            <ItemTemplate>
            <asp:LinkButton runat= "server" CausesValidation="false" Text="Pokaż zamówienie" ID="lbtnShowOrder"  OnClick="lbtnShowOrder_Click" ></asp:LinkButton>
            </ItemTemplate>
            </asp:TemplateField> 
        </Columns>
    </asp:GridView>
    
    
    </asp:Panel>
</asp:Content>
