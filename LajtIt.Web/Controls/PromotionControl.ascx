<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromotionControl.ascx.cs" Inherits="LajtIt.Web.Controls.PromotionControl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table>
    <tr>
        <td>Nazwa</td>
        <td>
            <asp:TextBox runat="server" ID="txbName" MaxLength="100"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Start</td>
        <td>
            <asp:TextBox runat="server" ID="txbStartDate" MaxLength="10" ValidationGroup="add" Style="text-align: center;"
                Width="70"></asp:TextBox><asp:RegularExpressionValidator runat="server" ControlToValidate="txbStartDate" Enabled="false"
                    ID="RegularExpressionValidator1" ValidationExpression="201[\d]{1}-[\d]{2}-[\d]{2}"
                    ValidationGroup="add" Text="*" /><asp:CalendarExtender
                        ID="calStartDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txbStartDate">
                    </asp:CalendarExtender>
        </td>
    </tr>
    <tr>
        <td>Koniec</td>
        <td>
            <asp:TextBox runat="server" ID="txbEndDate" MaxLength="10" ValidationGroup="add" Style="text-align: center;"
                Width="70"></asp:TextBox><asp:RegularExpressionValidator runat="server" ControlToValidate="txbEndDate" Enabled="false"
                    ID="RegularExpressionValidator3" ValidationExpression="201[\d]{1}-[\d]{2}-[\d]{2}"
                    ValidationGroup="add" Text="*" /><asp:CalendarExtender
                        ID="calEndDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txbEndDate">
                    </asp:CalendarExtender>
        </td>
    </tr>
    <tr>
        <td>Aktywny</td>
        <td>
            <asp:CheckBox runat="server" ID="chbIsActive"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="Zapisz" /></td>
    </tr>
</table>
