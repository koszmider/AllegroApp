<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="LajtIt.Web.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>
        Logowanie do systemu</h1>
    <table>
        <tr>
            <td>
                Login
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbLoginName1" autocomplete="Off"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Hasło
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbPassword1" TextMode="Password" autocomplete="Off"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="Wejdź do systemu" />
            </td>
        </tr>
    </table>
</asp:Content>
