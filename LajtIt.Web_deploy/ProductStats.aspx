<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductStats.aspx.cs" Inherits="LajtIt.Web.ProductStats" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr valign="top">
            <td rowspan="4">
                <asp:RadioButtonList ID="rbtnDateOption" runat="server">
                    <asp:ListItem Selected="True">Wybrany zakres dat</asp:ListItem>
                    <asp:ListItem>Ten miesiąc</asp:ListItem>
                    <%--<asp:ListItem>Ten tydzień</asp:ListItem>
            <asp:ListItem>Poprzedni tydzień</asp:ListItem>--%>
                    <asp:ListItem>Poprzedni miesiąc</asp:ListItem>
                    <asp:ListItem>Ostatnie 30 dni</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>od</td>
            <td><asp:TextBox runat="server" ID="txbDateFrom" Width="80"></asp:TextBox><asp:CalendarExtender
                    ID="calFrom" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateFrom">
                </asp:CalendarExtender></td>
            
        </tr>
        <tr>
        <td>do</td>
        <td><asp:TextBox runat="server" ID="txbDateTo" Width="80"></asp:TextBox><asp:CalendarExtender
                    ID="calTo" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateTo">
                </asp:CalendarExtender></td>
        </tr>
        <tr>
        <td>Dostawca</td>
        <td><asp:DropDownList runat="server" ID="ddlSuppliers" AppendDataBoundItems="true" DataTextField="Name" DataValueField="SupplierId">
                <asp:ListItem></asp:ListItem>
                
                </asp:DropDownList></td></tr>
    </table>
    <asp:Button runat="server" OnClick="btnSearch_Click" ID="btnSearch" Text="Pokaż" />
    <br />
    <br />
    <asp:Label runat="server" ID="lblDates"></asp:Label>
    <table>
        <tr valign="top">
            <td>
                <asp:GridView runat="server" ID="gvStats" AutoGenerateColumns="false" AllowSorting="true"
                    OnSorting="gvStats_Sorting" OnRowDataBound="gvStats_OnRowDataBound" ShowFooter="true">
                    <Columns>
                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Produkt" ItemStyle-Width="300" />
                        <asp:BoundField DataField="OrdersCount" SortExpression="OrdersCount" HeaderText="Liczba zamówień"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="Liczba przedmiotów"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Amount" SortExpression="Amount" DataFormatString="{0:C}"
                            HeaderText="Wartość" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-HorizontalAlign="Right" />
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;
            </td>
            <td>
                <asp:GridView runat="server" ID="gvProductGroup" AutoGenerateColumns="false" AllowSorting="true"
                    OnSorting="gvProductGroup_Sorting" OnRowDataBound="gvProductGroup_OnRowDataBound"
                    ShowFooter="true">
                    <Columns>
                        <asp:BoundField DataField="GroupName" SortExpression="GroupName" HeaderText="Grupa produktów"
                            ItemStyle-Width="200" />
                        <asp:BoundField DataField="OrdersCount" SortExpression="OrdersCount" HeaderText="L.zam."
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TotalOrdersCount" SortExpression="TotalOrdersCount" HeaderText="%"
                            DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="L.prz."
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TotalQuantity" SortExpression="TotalQuantity" HeaderText="%"
                            DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Amount" SortExpression="Amount" DataFormatString="{0:C}"
                            HeaderText="Wartość" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right"
                            FooterStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="TotalAmount" SortExpression="TotalAmount" DataFormatString="{0:0.00}"
                            HeaderText="%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
