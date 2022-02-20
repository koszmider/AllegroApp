<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shop.aspx.cs"
    Inherits="LajtIt.Web.Shop" %>

<%@ Register Src="~/Controls/ShopMenu.ascx" TagName="ShopMenu" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc:ShopMenu runat="server" SetTab="td1"></uc:ShopMenu>
    <table>
        <tr>
            <td>Nazwa</td>
            <td>
                <asp:TextBox runat="server" ID="txbShop" MaxLength="100" Width="500"></asp:TextBox></td>
        </tr>
        <tr>
            <td>ClientSecret</td>
            <td>
                <asp:TextBox runat="server" ID="txbClientSecret" MaxLength="254" Width="500"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Szablon nazwy</td>
            <td>
                <asp:TextBox runat="server" ID="txbTemplate" MaxLength="100" Width="500"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Min. cena</td>
            <td>  <asp:TextBox runat="server" style="text-align:right" ID="txbMinPrice" MaxLength="10" Width="70"></asp:TextBox><asp:RegularExpressionValidator
                     Display="Dynamic"            runat="server" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ControlToValidate="txbMinPrice"
                                Text="zły format" /></td>
        </tr>
        <tr>
            <td>Max. cena</td>
            <td> <asp:TextBox runat="server" style="text-align:right" ID="txbMaxPrice" MaxLength="10" Width="70"></asp:TextBox><asp:RegularExpressionValidator
                     Display="Dynamic"            runat="server" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ControlToValidate="txbMaxPrice"
                                Text="zły format" /></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" OnClientClick="return confirm('Zapisać?');" Text="Zapisz" /></td>
            <td style="text-align: right;">
                <asp:CheckBox runat="server" ID="chbCreateNew" Text="Nadpisz istniejące" />
                <asp:Button runat="server" ID="btnName" OnClick="btnName_Click" OnClientClick="return confirm('Utworzyć nazwy produktów?');" Text="Twórz nazwy" /></td>

        </tr>
    </table>
</asp:Content>
