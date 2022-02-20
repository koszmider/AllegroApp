<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllegroTokens.aspx.cs" Inherits="LajtIt.Web.AllegroTokens" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Tokeny Allegro</h1>
    <table>
        <tr>
            <td><asp:DropDownList runat="server" ID="ddlAllegroUsers" DataValueField="UserId" DataTextField="UserName"></asp:DropDownList></td>
            <td></td>
        </tr>
        <tr>
            <td>Kod</td>
            <td><asp:TextBox runat="server" ID="txbCode"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button runat="server" ID="btnGetToken" OnClick="btnGetToken_Click" Text="Pobierz token" /></td>
        </tr>
    </table>

    <table>
        <tr>
            <td>
    <asp:GridView runat="server" ID="gvUsers" AutoGenerateColumns="false" DataKeyNames="UserId" OnRowCommand="gvUsers_RowCommand">
        <Columns>
            <asp:BoundField DataField="UserName" HeaderText="Konto" />
            <asp:BoundField DataField="TokenCreateDate" HeaderText="Data utworzenia" />
            <asp:BoundField DataField="TokenEndDate" HeaderText="Data ważności" />
            <asp:HyperLinkField Text="Nowy token" DataNavigateUrlFormatString="https://allegro.pl/auth/oauth/authorize?response_type=code&client_id={0}&redirect_uri=https://lajtit.pl" Target="_blank" DataNavigateUrlFields="ClientId"/>
            <asp:ButtonField ButtonType="Link" Text="Odśwież" CommandName="refresh" />
        </Columns>

    </asp:GridView>
                </td>
            <td>
                Jak odświeżyć token?
                <ul>
                    <li>Wyloguj się z allegro</li>
                    <li>Kliknij <b>Nowy token</b> i zaloguj się do allegro</li>
                    <li>Po przekierowaniu na lajtit.pl z adresu https://lajtit.pl/?code=G1hnvk29dv0NQDWCMvmVcbDref4HM8R0 skopiuj <b>G1hnvk29dv0NQDWCMvmVcbDref4HM8R0</b></li>
                    <li>Wklej w okienku <b>Kod</b>, wybierz z listy login na który się logowano i kliknij <b>Pobierz token</b></li>
                    <li>Po poprawnie wykonanej czynności <b>Data ważności</b> odświeży się</li>
                </ul>
            </td>
        </tr>
    </table>
</asp:Content>
