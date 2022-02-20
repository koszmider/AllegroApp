<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShippingTypesControl.ascx.cs" Inherits="LajtIt.Web.Controls.ShippingTypesControl" %>

<asp:Panel runat="server" ID="pDpd" Visible="false">
    <table>
        <tr>
            <td>Liczba paczek
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbDpdParcels" MaxLength="2" Width="50" Text="0" ValidationGroup="shipping" /><asp:RegularExpressionValidator
                    ID="RegularExpressionValidator6" runat="server" ControlToValidate="txbDpdParcels"
                    Text="*" ValidationGroup="shipping" ValidationExpression="\d{1,2}" />
            </td>
            <td><asp:Label runat="server" ID="lblWeight" Text="Max waga rzeczywista lub gabarytowa" Visible="false"></asp:Label>
                <asp:RadioButtonList runat="server" ID="rblDpdWeight" RepeatDirection="Horizontal" Visible="false">
                    <asp:ListItem Value="3">3kg</asp:ListItem>
                    <asp:ListItem Value="10">10kg</asp:ListItem>
                    <asp:ListItem Value="20">20kg</asp:ListItem>
                    <asp:ListItem Value="31,5">31,5kg</asp:ListItem>
                </asp:RadioButtonList>

            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel runat="server" ID="pInPost" Visible="false">
    <table>
        <tr>
            <td>Gabaryt:
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlPaczkomatGabaryt" ValidationGroup="shipping">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem>A</asp:ListItem>
                    <asp:ListItem>B</asp:ListItem>
                    <asp:ListItem>C</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlPaczkomatGabaryt"
                    Text="*" ValidationGroup="shipping" ErrorMessage="Nie podano gabarytu" />
            </td>
        </tr>
        <tr>
            <td>Paczkomat:
            </td>
            <td>
                <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddlPaczkomat" DataTextField="Description" Width="300"
                    DataValueField="Name" ValidationGroup="shipping">
                    <asp:ListItem Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlPaczkomat"
                    Text="*" ValidationGroup="shipping" ErrorMessage="Nie wybrano paczkomatu" />
            </td>
        </tr>
    </table>
</asp:Panel>
