<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductCatalog.Calculator.aspx.cs" Inherits="LajtIt.Web.ProductCalculator" %>
    
<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"> 
    <uc:ProductMenu ID="ProductMenu1" runat="server" SetTab="td5"></uc:ProductMenu>
    <table>
        <tr>
            <td>
                Cena brutto
            </td>
            <td>
                <asp:TextBox Text="0" runat="server" ID="txbBrutto" AutoPostBack="true" MaxLength="10"
                    OnTextChanged="Calulcate"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Cena netto
            </td>
            <td>
                <asp:Label runat="server" ID="lblNetto"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Koszt materiał
            </td>
            <td>
                <asp:TextBox Text="0" runat="server" ID="txbMaterial" AutoPostBack="true" MaxLength="10"
                    OnTextChanged="Calulcate"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Koszt dodatków
            </td>
            <td>
                <asp:TextBox Text="0" runat="server" ID="txbAdditions" AutoPostBack="true" MaxLength="10"
                    OnTextChanged="Calulcate"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Koszt robocizna
            </td>
            <td>
                <asp:TextBox Text="0" runat="server" ID="txbWork" AutoPostBack="true" MaxLength="10"
                    OnTextChanged="Calulcate"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                Allegro
            </td>
            <td>
                Sklep
            </td>
        </tr>
        <tr>
            <td>
                Koszt wystawienia
            </td>
            <td>
                <asp:TextBox Text="0" runat="server" ID="txbCreateCostAllegro" AutoPostBack="true"
                    MaxLength="10" OnTextChanged="Calulcate"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox Text="0" runat="server" ID="txbCreateCostShop" AutoPostBack="true" MaxLength="10"
                    OnTextChanged="Calulcate"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Sprzedany przedmiotów
            </td>
            <td>
                <asp:TextBox Text="0" runat="server" ID="txbItemsSoldAllegro" AutoPostBack="true"
                    MaxLength="10" OnTextChanged="Calulcate"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox Text="0" runat="server" ID="txbItemsSoldShop" AutoPostBack="true" MaxLength="10"
                    OnTextChanged="Calulcate"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Prowizja sprzedaż
            </td>
            <td>
                <asp:Label runat="server" ID="lblCommissionAllegro"></asp:Label>
            </td>
            <td>
                <asp:Label runat="server" ID="lblCommissionShop"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Razem koszt
            </td>
            <td>
                <asp:Label ID="lblTotalAllegro" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTotalShop" runat="server"></asp:Label>
            </td>
        </tr>
        <tr style="font-weight: bold;">
            <td>
                Zysk netto zł
            </td>
            <td>
                <asp:Label ID="lblTotalIncomeAllegro" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTotalIncomeShop" runat="server"></asp:Label>
            </td>
        </tr>
        <tr style="font-weight: bold;">
            <td>
                Zysk %
            </td>
            <td>
                <asp:Label ID="lblTotalIncomePercAllegro" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTotalIncomePercShop" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Button runat="server" ID="btnSaveCalculation" OnClientClick="return confirm('Zapisać kalkulację?');"
                    Text="Zapisz" OnClick="btnSaveCalculation_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
