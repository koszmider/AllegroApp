<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="LajtIt.Web.Account.ChangePassword" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>
        Zmiana hasła
    </h1>
    <table>
        <tr>
            <td>
                Stare hasło
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbPasswordOld" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Nowe hasło
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbPassword" TextMode="Password"></asp:TextBox> 
            </td>
        </tr>
        <tr>
            <td>
                Nowe hasło (powtórz)
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbPassword1" TextMode="Password"></asp:TextBox><asp:CompareValidator
                    runat="server" ControlToCompare="txbPassword1" ControlToValidate="txbPassword"
                    Text="Powtórzenie nowego hasła jest niepoprawne" Type="String"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnPasswordChange" OnClick="btnPasswordChange_Click"
                    Text="Zmień hasło" />
            </td>
        </tr>
    </table>
</asp:Content>
