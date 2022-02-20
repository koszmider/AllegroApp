<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Supplier.New.aspx.cs" Inherits="LajtIt.Web.SupplierNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Kreator nowego dostawcy</h1>
    <table class="mytable">

        <tr>
            <td style="width: 300px;">Właściciel
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlSupplierOwner" DataTextField="Name" DataValueField="SupplierOwnerId" AppendDataBoundItems="true">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList><asp:RequiredFieldValidator
                    runat="server" ControlToValidate="ddlSupplierOwner" Text="*"></asp:RequiredFieldValidator>
                <asp:TextBox runat="server" ID="txbSupplierOwner" MaxLength="256" ValidationGroup="owner"></asp:TextBox><asp:RequiredFieldValidator
                    ValidationGroup="owner"
                    runat="server" ControlToValidate="txbSupplierOwner" Text="*"></asp:RequiredFieldValidator><asp:LinkButton ValidationGroup="owner" runat="server" ID="lbtnSupplierOwnerAdd" OnClientClick="return cofirm('Dodać?')"
                        Text="Dodaj nowego" OnClick="lbtnSupplierOwnerAdd_Click"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>Nazwa
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbName" MaxLength="256"></asp:TextBox><asp:RequiredFieldValidator
                    runat="server" ControlToValidate="txbName" Text="*"></asp:RequiredFieldValidator>
            </td>
        </tr>

        <tr>
            <td style="width: 300px;">Czas dostawy 
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlShopDelivery" DataTextField="Name" DataValueField="DeliveryId" /><asp:RequiredFieldValidator
                    runat="server" ControlToValidate="ddlShopDelivery" Text="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <h2>Allegro</h2>
    <table class="mytable">

        <tr valign="top">
            <td style="width: 300px;">Cennik dostawy
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlAllegroDeliveryType" DataTextField="Name"
                    DataValueField="DeliveryCostTypeId" AppendDataBoundItems="true">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList><asp:RequiredFieldValidator
                    runat="server" ControlToValidate="ddlAllegroDeliveryType" Text="*"></asp:RequiredFieldValidator>

            </td>
        </tr>


        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Zapisz" /></td>
        </tr>

    </table>
</asp:Content>
