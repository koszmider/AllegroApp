<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SettingsEditor.aspx.cs" Inherits="LajtIt.Web.SettingsEditor"  EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Edytor ustawień</h1>
    <table>
        <tr valign="top">
            <td>
                <asp:ListBox runat="server" ID="lsbxSettings" Rows="10" Width="200" DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="lsbxSettings_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>

                <table>
                    <tr>
                        <td>
                            <asp:LinkButton runat="server" Text="Dodaj nowy" OnClick="btnNew_Click" OnClientClick="return confirm('Dodać nowy?');" /></td>
                        <td>
                            <asp:Button runat="server" Text="Zapisz" OnClick="btnSave_Click" OnClientClick="return confirm('Zapisać zmiany?');" /></td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:Panel runat="server" ID="pnSettings" Visible="false">
                    <table>
                        <tr>

                            <td>Nazwa:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbName" Width="400"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Kod:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbCode"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Wartość całkowita</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbInt"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Wartość liczbowa</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbDecimal"></asp:TextBox></td>
                        </tr>
                        <tr valign="top">
                            <td>Wartość tekstowa</td>
                            <td>
                                <asp:TextBox runat="server" TextMode="MultiLine" Rows="40" Columns="80" ID="txbString"></asp:TextBox></td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
