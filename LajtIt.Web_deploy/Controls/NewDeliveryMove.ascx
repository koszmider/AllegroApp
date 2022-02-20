<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewDeliveryMove.ascx.cs" Inherits="LajtIt.Web.Controls.NewDeliveryMove" %>
<asp:Panel runat="server" ID="pnMove" GroupingText="Przenoszenie produktów między magazynami">
    <table style="width: 550px">
        <tr>
            <td style="width: 250px;">Z magazynu:</td>
            <td></td>
            <td>Do magazynu:</td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList Style="width: 250px;" runat="server" ID="ddlWarehouseFrom" DataValueField="WarehouseId" DataTextField="Name" OnSelectedIndexChanged="ddlWarehouseFrom_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </td>
            <td style="width: 50px;">
                <asp:TextBox runat="server" ID="txbQuantity" MaxLength="4" Width="40" ValidationGroup="move"></asp:TextBox></td>
            <td>
                <asp:DropDownList Style="width: 250px;" runat="server" ID="ddlWarehouseTo" DataValueField="WarehouseId" DataTextField="Name">
                </asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td colspan="3" style="text-align: center;">
                <asp:Button runat="server" ID="btnMove" OnClick="btnMove_Click" Text="Przenieś" ValidationGroup="move" /><br />
                <asp:Label runat="server" ID="lbInfo" ForeColor="Red" Text="Niewystarczająca liczba produktów do przeniesienia. Zmień magazyn"></asp:Label>
                <asp:RangeValidator ID="rvQuantity" runat="server" ControlToValidate="txbQuantity"  
                    MinimumValue="0" ValidationGroup="move" Type="Integer"></asp:RangeValidator>
            </td>
        </tr>
    </table>



</asp:Panel>
