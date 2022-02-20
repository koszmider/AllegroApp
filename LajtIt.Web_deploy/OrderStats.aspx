<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OrderStats.aspx.cs" Inherits="LajtIt.Web.OrderStats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        td.columnAmount
        {
            text-align: right;
            width: 120px;
        }
    </style>
    <table style="width: 100%; font-size: 80%;">
    <tr><td colspan="2">
                Podane kwoty netto. <b>Zamówienia</b> - Wartość szacunkowa zamówień (z dostawą)
                wg daty zamówienia, <b>...opłacone</b> - wartość zamówień na podstawie wpłat wg
                daty zamówienia, <b>Allegro</b> - wartość zamówień Allegro, <b>Wpłaty</b> - w danym
                miesiącu, mogą dotyczyć zamówień z minionych miesięcy, <b>Zwroty</b> -  wykonane
                w danym miesiącu, <b>Koszty</b> - poniesione w danym miesiącu, <b>Dochód</b> - suma
                wpłat i zwrotów minus koszty, <b>VAT</b> - należny, do zapłaty<br /><br /></td></tr>
        <tr valign="top">
            <td>
                <asp:GridView runat="server" ID="gvOrderStats" AutoGenerateColumns="false" OnRowDataBound="gvOrderStats_OnRowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Date" HeaderText="Miesiąc" />
                        <asp:BoundField DataField="OrdersCount" HeaderText="L.zam" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Rate" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"
                            HeaderText="Średnia" />
                        <asp:BoundField DataField="AmountToPay" HeaderText="Zamówienia" ItemStyle-CssClass="columnAmount"
                            DataFormatString="{0:C}" />
                        <asp:BoundField DataField="AmountPaid" HeaderText="..opłacone" ItemStyle-CssClass="columnAmount"
                            DataFormatString="{0:C}" />
                        <asp:BoundField DataField="AllegroItemsValue" HeaderText="Allegro" DataFormatString="{0:C}"
                            ItemStyle-CssClass="columnAmount" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            Wpłaty
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlAccount" OnSelectedIndexChanged="ddlAccount_Changed"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="-1">all</asp:ListItem>
                                                <asp:ListItem Value="1">RTP</asp:ListItem>
                                                <asp:ListItem Value="2">La Luz</asp:ListItem>
                                                <asp:ListItem Value="0">reszta</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPayment"></asp:Label></ItemTemplate>
                            <ItemStyle CssClass="columnAmount" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="PaymentNetto" HeaderText="Wpłaty" DataFormatString="{0:C}"
                            Visible="false" ItemStyle-CssClass="columnAmount" />
                        <asp:BoundField DataField="ReturnNetto" HeaderText="Zwroty" DataFormatString="{0:C}"
                            ItemStyle-CssClass="columnAmount" />
                        <asp:BoundField DataField="CostNetto" HeaderText="Koszty" DataFormatString="{0:C}"
                            ItemStyle-CssClass="columnAmount" />
                        <asp:BoundField DataField="TotalNetto" HeaderText="Dochód" DataFormatString="{0:C}"
                            ItemStyle-Font-Bold="true" ItemStyle-CssClass="columnAmount" />
                        <asp:BoundField DataField="TotalVAT" HeaderText="Vat" DataFormatString="{0:C}" ItemStyle-Font-Bold="true"
                            ItemStyle-CssClass="columnAmount" />
                    </Columns>
                </asp:GridView>
            </td>
            <td style="text-align: right"> 
                <asp:GridView runat="server" ID="gvOrdersByDay" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="Date" DataFormatString="{0:MM-dd}" HeaderText="Dzień"
                            ItemStyle-Width="50" />
                        <asp:BoundField DataField="OrdersCount" ItemStyle-HorizontalAlign="Center" HeaderText="L.zam." />
                        <asp:BoundField DataField="AmountToPay" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Width="80" HeaderText="Wartość" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
