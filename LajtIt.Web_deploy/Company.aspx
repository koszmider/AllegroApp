<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Company.aspx.cs" Inherits="LajtIt.Web.Company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Firma - edycja</h1>
    <div style="text-align:right"><a href="CompanyInfo.aspx">zobacz wszystkie</a></div>
    <asp:Panel runat="server" ID="pnAdmin">
        <table>
            <tr>
                <td>Aktywny</td>
                <td>
                    <asp:CheckBox runat="server" ID="chbIsActive" /></td>
            </tr>
            <tr>
                <td>Moja firma</td>
                <td>
                    <asp:CheckBox runat="server" ID="chbIsMyCompany" /></td>
            </tr>
            <tr>
                <td>Nazwa</td>
                <td>
                    <asp:TextBox runat="server" ID="txbName" Width="200" ></asp:TextBox><asp:RequiredFieldValidator runat="server" ControlToValidate="txbName" Text="*"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td>Adres</td>
                <td>
                    <asp:TextBox runat="server" ID="txbAddress" Width="200" ></asp:TextBox>
                    numer<asp:TextBox runat="server" ID="txbAddressNo" Width="100" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>Kod pocztowy</td>
                <td>
                    <asp:TextBox runat="server" ID="txbPostalCode" Width="200" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>Miasto</td>
                <td>
                    <asp:TextBox runat="server" ID="txbCity" Width="200" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>NIP</td>
                <td>
                    <asp:TextBox runat="server" ID="txbTaxId" Width="200" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>Regon</td>
                <td>
                    <asp:TextBox runat="server" ID="txbRegon" Width="200" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>KRS</td>
                <td>
                    <asp:TextBox runat="server" ID="txbKRS" Width="200" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>BDO</td>
                <td>
                    <asp:TextBox runat="server" ID="txbBDO" Width="200" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>Właściciel</td>
                <td>
                    <asp:TextBox runat="server" ID="txbOwner" Width="200" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>DPDNumcat</td>
                <td>
                    <asp:TextBox runat="server" ID="txbDPDNumcat" Width="200" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>Liczba dni płatności</td>
                <td>
                    <asp:TextBox runat="server" ID="txbPaymentDays" TextMode="Number" Width="200" ></asp:TextBox></td>
            </tr>
            <tr><td colspan="2"><hr /></td></tr>          
            <tr>
                <td></td>
                <td>
                    <asp:CheckBox runat="server" ID="chbCanSendToBank" Text="Pozwól na dodawanie do paczki przelewów" /></td>
            </tr>
            <tr>
                <td>Bank - nazwa firmy</td>
                <td>
                    <asp:TextBox runat="server" ID="txbBandCompanyName" Width="200" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>Bank - numer konta</td>
                <td>
                    <asp:TextBox runat="server" ID="txbBankAccountNumber" Width="200" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>Bank - numer rozliczeniowy</td>
                <td>
                    <asp:TextBox runat="server" ID="txbBankAccountNumber2" Width="200" Text="0" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>Bank - adres firmy</td>
                <td>
                    <asp:TextBox runat="server" ID="txbCompanyAddress" Width="200" ></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnSave" Text="Zapisz"  OnClick="btnSave_Click" /></td>
            </tr>
        </table>

    </asp:Panel>
     

</asp:Content>
