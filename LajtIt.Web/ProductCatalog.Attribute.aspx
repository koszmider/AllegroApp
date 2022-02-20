<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ProductCatalog.Attribute.aspx.cs" Inherits="LajtIt.Web.ProductAttribute" %>

<%@ Register Src="~/Controls/AttributeMenu.ascx" TagName="AttributeMenu" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     
    <uc:AttributeMenu id="ucAttributeMenu" runat="server" SetTab="td1"></uc:AttributeMenu>
    <asp:Panel runat="server" ID="pnContent">
    <table>
        <tr>
            <td>Nazwa</td>
            <td><asp:TextBox runat="server" ID="txbName" Width="300"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Rodzaj </td>
            <td><asp:RadioButton runat="server" GroupName="sex" ID="rbSex" Text="Nieokreślony" />
                <asp:RadioButton runat="server" GroupName="sex" ID="rbSex1" Text="Męski" />
                <asp:RadioButton runat="server" GroupName="sex" ID="rbSex0" Text="Żeński" />
                <asp:RadioButton runat="server" GroupName="sex" ID="rbSex2" Text="Nijaki" />
            </td>
        </tr>
        <tr>
            <td>Tytuł - przyjemna nazwa (r.męski)</td>
            <td><asp:TextBox runat="server" ID="txbFriendlyNameM" Width="300" MaxLength="1024"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Tytuł - przyjemna nazwa (r.żeński)</td>
            <td><asp:TextBox runat="server" ID="txbFriendlyNameF" Width="300" MaxLength="1024"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Tytuł - przyjemna nazwa (r.nijaki)</td>
            <td><asp:TextBox runat="server" ID="txbFriendlyNameN" Width="300" MaxLength="1024"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Opis - przyjemna nazwa (r.męski)</td>
            <td><asp:TextBox runat="server" ID="txbFriendlyDescriptionM" Width="300"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Opis - przyjemna nazwa (r.żeński)</td>
            <td><asp:TextBox runat="server" ID="txbFriendlyDescriptionF" Width="300"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Opis - przyjemna nazwa (r.nijaki)</td>
            <td><asp:TextBox runat="server" ID="txbFriendlyDescriptionN" Width="300"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Kod importu z pliku</td>
            <td><asp:TextBox runat="server" ID="txbCode" Width="300" MaxLength="20"></asp:TextBox></td>
        </tr>

        <tr>
            <td>Rodzaj pola</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlAttributeType">
                    <asp:ListItem Value="0">nie wybrano</asp:ListItem>
                    <asp:ListItem Value="1">wartość liczbowa</asp:ListItem>
                    <asp:ListItem Value="2">tekst</asp:ListItem>
                </asp:DropDownList>

            </td>
        </tr>

        <tr>
            <td>Szablon pola</td>
            <td><asp:TextBox runat="server" ID="txbFieldTemplate" Width="300"></asp:TextBox></td>
        </tr>
         

        <tr>
            <td colspan="2"><asp:Button runat="server" ID="btnAttributeSave" OnClick="btnAttributeSave_Click" OnClientClick="return confirm('Zapisać zmiany?');" Text="Zapisz" /></td>
        </tr>
    </table>
        </asp:Panel>
</asp:Content>
