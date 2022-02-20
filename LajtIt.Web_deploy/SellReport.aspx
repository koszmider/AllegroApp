<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SellReport.aspx.cs" Inherits="LajtIt.Web.SellReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>
                Sprzedawca:
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlUserName">
                    <asp:ListItem>agata</asp:ListItem>
                    <asp:ListItem>ania</asp:ListItem>
                    <asp:ListItem>magda</asp:ListItem>
                    <asp:ListItem>jacek</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Miesiąc:
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlMonth">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Prowizja sprzedażowa
            </td>
            <td>
                <asp:Label runat="server" ID="lblSellCommision"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnShow" OnClick="btnShow_Click" Text="Pokaż" />
            </td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="gvReport" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvReport_OnRowDataBound">
        <Columns>
            <asp:HyperLinkField DataTextField="OrderId" DataNavigateUrlFields="OrderId" HeaderText="Id" Target="_blank" DataNavigateUrlFormatString="/Order.aspx?id={0}" />
            <asp:BoundField DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data zamówienia" DataField="CreateDate" />
            <asp:BoundField DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data finalizacji" DataField="ReadyDate" />
            <asp:BoundField DataFormatString="{0:C}" HeaderText="Wartość zamówienia" DataField="Amount" ItemStyle-HorizontalAlign="right"  />
            <asp:BoundField DataFormatString="{0:C}" HeaderText="Prowizja sprzedażowa" DataField="CommisionValue" ItemStyle-HorizontalAlign="right"/>
        </Columns>
    </asp:GridView>
</asp:Content>
