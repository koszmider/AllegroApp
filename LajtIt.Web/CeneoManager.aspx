<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CeneoManager.aspx.cs" Inherits="LajtIt.Web.CeneoManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table style="width: 700px">
        <tr>
            <td colspan="2" style="text-align: center; width: 300px; background-color:pink;">Ceneo</td>
            <td colspan="4" style="text-align: center; background-color:silver" >Sklep</td>
        </tr>
        <tr>
            <td style="text-align: center; background-color:pink; width:100px;"> </td>
            <td  style="text-align: center; background-color:pink;">
                <asp:DropDownList runat="server" ID="ddlShop" DataValueField="ShopId" DataTextField="Name"></asp:DropDownList></td>
            <td colspan="4" style="text-align: center; background-color:silver"></td>
        </tr>
        <tr>
            <td style="text-align: center; background-color:pink; width:100px;">Produkty</td>
            <td style="text-align: center; background-color:pink; width:200px;">
                <asp:DropDownList runat="server" ID="ddlProductAuction">
                    <asp:ListItem Value="-1">--</asp:ListItem>
                    <asp:ListItem Value="1">licytowane</asp:ListItem>
                    <asp:ListItem Value="0">nielicytowane</asp:ListItem>
                </asp:DropDownList></td>
            <td style="text-align: center; background-color:silver">Promocja</td>
            <td style="text-align: center; background-color:silver">
                <asp:DropDownList runat="server" ID="ddlShopPromo">
                    <asp:ListItem Value="-1">--</asp:ListItem>
                    <asp:ListItem Value="1">TAK</asp:ListItem>
                    <asp:ListItem Value="0">NIE</asp:ListItem>
                </asp:DropDownList></td>
            <td style="text-align: center; background-color:silver">Dostawcy</td>
            <td style="text-align: center; background-color:silver">
                <asp:ListBox runat="server" ID="lbxSuppliers" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="SupplierId" Width="220"></asp:ListBox></td>

        </tr>
        <tr><td style="text-align: center; background-color:pink;"></td><td style="text-align: center; background-color:pink;"></td>
            <td style="text-align: center; background-color:silver;">Cena</td>
            <td colspan="4" style="text-align: center; background-color:silver;">od <asp:TextBox runat="server" id="txbPriceFrom" Text="0" Width="50"></asp:TextBox> do <asp:TextBox runat="server" id="txbPriceTo" Text="100000" Width="50"></asp:TextBox></td>
            </tr>
        <tr>
            <td style="width:200px;text-align: center; background-color:pink;">Stawka licytacji</td>
            <td  style="text-align: center; background-color:pink;">od <asp:TextBox runat="server" id="txbBidFrom" Text="0" Width="50"></asp:TextBox> do <asp:TextBox runat="server" id="txbBidTo" Text="10" Width="50"></asp:TextBox></td>
            <td colspan="4" style="text-align: center; background-color:silver"></td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnSearch" Text="Szukaj" OnClick="btnSearch_Click" /></td>
            <td>
                <asp:Label runat="server" ID="lblResults"></asp:Label></td>
        </tr>
    </table>
    <asp:Panel ID="pnActions" GroupingText="Akcje" runat="server">
     
        <table>
            <tr>
                <td><asp:Button runat="server" ID="btnClearPromotions" Text="Wyzeruj licytowane" OnClientClick="return confirm('Wyzerować?');" OnClick="btnClearPromotions_Click" />
</td>
                <td></td>
            </tr>
            <tr>
                <td><asp:Button runat="server" ID="btnMaxBid" Text="Ustaw stawkę licytacji" ValidationGroup="max"
                    OnClientClick="return confirm('Ustawić cenę?');" OnClick="btnMaxBid_Click" />
</td>
                <td><asp:TextBox ID="txbMaxBid" Text="5" ValidationGroup="max" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
        </table>
        
    </asp:Panel>
</asp:Content>
