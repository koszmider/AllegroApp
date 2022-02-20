<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllegroSellMatrixControl.ascx.cs"
    Inherits="LajtIt.Web.Controls.AllegroSellMatrixControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table>
    <tr>
        <td>
            Data końcowa
        </td>
        <td>
            Liczba tygodni
        </td>
        <td>
            Promowanie
        </td>
        <td>
            Sprzedawca
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox runat="server" ID="txbEndDate" Width="80"></asp:TextBox><asp:CalendarExtender
                ID="calEndDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txbEndDate">
            </asp:CalendarExtender>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txbNumberOfWeeks" Text="2" Width="80"></asp:TextBox>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlIsPromoted">
                <asp:ListItem Value="-1">--</asp:ListItem>
                <asp:ListItem Value="1">Tak</asp:ListItem>
                <asp:ListItem Value="0">Nie</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlSeller">
                <asp:ListItem Value="-1">wszyscy</asp:ListItem>
                <asp:ListItem Value="0">Moje</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td><asp:Button runat="server" ID="btnShow" Text="Pokaż" OnClick="btnShow_Click" /></td>
    </tr>
</table>
<asp:GridView runat="server" ID="gvAllegroSellMatrix" AutoGenerateColumns="false"
    OnRowDataBound="gvAllegroSellMatrix_OnRowDataBound" ShowFooter="true"> 
     <HeaderStyle Width="180px" BackColor="Silver" />
     <FooterStyle ForeColor="White" />
    <Columns >
        <asp:BoundField DataField="Monday" HeaderText="Pon." ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="Tuesday" HeaderText="Wto." ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="Wednesday" HeaderText="Śr." ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="Thursday" HeaderText="Czw." ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="Friday" HeaderText="Pt." ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="Saturday" HeaderText="Sob." ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="Sunday" HeaderText="Nd." ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="TotalByHour" HeaderText="Razem" ItemStyle-Width="40" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField   HeaderText="%" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center"  ItemStyle-ForeColor="White"/>
 
        <asp:BoundField DataField="Hour" DataFormatString="{0}:00" HeaderText="Godzina"  ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center"/>
    </Columns>
</asp:GridView>
