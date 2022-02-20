<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Offers.aspx.cs" Inherits="LajtIt.Web.Offers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h1>Kreator ofert</h1>
    </div>
    <div>
        <table style="text-align: right; width: 100%;">
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txbName"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
                        TargetControlID="txbName"
                        WatermarkText="Nazwa oferty"
                        WatermarkCssClass="watermarked" />
                    <asp:RequiredFieldValidator runat="server" Text="*" ControlToValidate="txbName"></asp:RequiredFieldValidator>

                    <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="Dodaj nową ofertę" />

                </td>
            </tr>

        </table>
    </div>
    <table>
        <tr>
            <td>
                <asp:TextBox runat="server" ID="txbSearch" Width="250" ValidationGroup="search"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                    TargetControlID="txbSearch"
                    WatermarkText="Nazwa oferty, klient, email, telefon"
                    WatermarkCssClass="watermarked" />
                <asp:TextBox runat="server" ID="txbSearchNumber" ValidationGroup="search"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server"
                    TargetControlID="txbSearchNumber"
                    WatermarkText="Numer oferty (1908...)"
                    WatermarkCssClass="watermarked" />
            </td>
            <td>
                <asp:CheckBoxList ID="chblOfferStatus" DataTextField="Name" RepeatDirection="Horizontal" DataValueField="OfferStatusId" runat="server"></asp:CheckBoxList>
            </td>
            <td>
                <asp:Button runat="server" ID="btnSearch" Text="Szukaj" OnClick="btnSearch_Click" ValidationGroup="search" />
            </td>

        </tr>

    </table>
    <br />
    <br />
    <asp:GridView runat="server" ID="gvOffers" AutoGenerateColumns="false" Width="100%">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="OfferId" DataTextField="Name" DataNavigateUrlFormatString="offer.aspx?id={0}" HeaderText="Nazwa" />
            <asp:BoundField DataField="ContactName" HeaderText="Klient" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Phone" HeaderText="Telefon" />
            <asp:BoundField DataField="InsertDate" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="Data dodania" />
            <asp:BoundField DataField="InsertUser" HeaderText="Autor" />
            <asp:BoundField DataField="StatusName" HeaderText="Status oferty" />

        </Columns>

    </asp:GridView>







</asp:Content>
