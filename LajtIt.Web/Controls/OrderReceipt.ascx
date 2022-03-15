<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderReceipt.ascx.cs"
    Inherits="LajtIt.Web.Controls.OrderReceiptControl" %>
<%@ Register Src="~/Controls/ReceiptOrderGrid.ascx" TagName="ReceiptOrderGrid" TagPrefix="uc" %>
<style>
    .tabSelected {
        background-color: white;
    }
</style>

<asp:Panel runat="server" ID="pnOrderReceipt">

    <table>
        <tr>
            <td><b>Lokalizacja drukarki</b></td>
            <td><asp:DropDownList runat="server" ID="ddlCashRegister" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlCashRegister_SelectedIndexChanged" DataTextField="Name"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Typ paragonu
            </td>
            <td>
                <asp:RadioButtonList runat="server" ID="rbReceiptType" AutoPostBack="true" ValidationGroup="receipt"
                    OnSelectedIndexChanged="ddlReceiptType_SelectedIndexChanged">
                    <asp:ListItem Value="1">Paragon na całość zamówienia</asp:ListItem>
                    <asp:ListItem Value="2">Paragon zaliczkowy</asp:ListItem>
                    <asp:ListItem Value="3">Paragon rozliczenie zaliczki</asp:ListItem>

                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="rfv" runat="server" ValidationGroup="receipt" 
                    ControlToValidate="rbReceiptType"   Text="Wybierz rodzaj paragonu"></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td>Wysokość zaliczki</td>
            <td>
                <asp:TextBox runat="server" ID="txbPrePayment" Enabled="false" ValidationGroup="receipt"></asp:TextBox>
<%--                <asp:CompareValidator ID="cvPrePayment" runat="server" ValidationGroup="receipt"  Display="Dynamic"
                    ControlToValidate="txbPrePayment" Operator="LessThan" Type="Double" Text="Wartość zaliczki zbyt wysoka"></asp:CompareValidator>--%>
 <asp:Label runat="server" ID="lblPrePayment" ForeColor="Red" Visible="false" Text="Wartość wymagana"></asp:Label>

            </td>
        </tr>
        <tr>
            <td>Numer NIP
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbNip" ValidationGroup="receipt"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Wartość zamówienia</td>
            <td>
                <asp:Label runat="server" ID="lblTotal" ValidationGroup="receipt"></asp:Label></td>
        </tr>
        <tr>
            <td>Wartość płatności</td>
            <td>
                <asp:Label runat="server" ID="lblPayments" ValidationGroup="receipt"></asp:Label></td>
        </tr>
        <tr>
            <td>Wartość wystawionych paragonów</td>
            <td>
                <asp:Label runat="server" ID="lblReceiptTotal"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="2">
                <uc:ReceiptOrderGrid runat="server" ID="ucReceiptOrderGrid" />
            </td>
        </tr>
        <tr>
            <td colspan="2"><br />Status drukarki fiskalnej:<br />
                <asp:Label runat="server" ID="lblStatus" Text="sprawdzam...."></asp:Label>
                <asp:UpdatePanel runat="server" ID="upTimer">
                    <ContentTemplate>
                        <asp:UpdateProgress AssociatedUpdatePanelID="upTimer" runat="server" DynamicLayout="false">
                            <ProgressTemplate>
                                <asp:Image runat="server" ImageUrl="/Images/progress.gif" Style="height: 20px" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Timer ID="tmTimer" runat="server" Interval="10000" OnTick="tmTimer_Tick" Enabled="false" >
                        </asp:Timer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <br /> 
 

</asp:Panel>
