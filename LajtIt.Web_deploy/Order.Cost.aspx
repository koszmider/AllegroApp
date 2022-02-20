<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Order.Cost.aspx.cs" Inherits="LajtIt.Web.OrderCost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Rachunek kosztów</h1>
    <div style="text-align: right">
        <asp:HyperLink ID="hlOrder" NavigateUrl="/Order.aspx?id={0}" runat="server">Powrót do zamówienia</asp:HyperLink>
    </div>

    <table>
        <tr valign="top">
            <td>
    <asp:GridView runat="server" Id="gvOrderCost" AutoGenerateColumns="false" OnRowDataBound="gvOrderCost_RowDataBound" >
        <Columns>
            <asp:BoundField DataField="ItemType" HeaderText="" />
            <asp:BoundField DataField="Quantity" HeaderText="Liczba" />
            <asp:BoundField DataField="PriceTotal" HeaderText="Cena całkowita" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
        </Columns>
    </asp:GridView></td>
            <td>
                <table style="padding: 5px;">
                    <tr>
                        <td>Wpływy</td>
                        <td style="text-align:right"><asp:Label runat="server" ID="lblGain" ForeColor="Green"></asp:Label> </td>
                    </tr>
                    <tr>
                        <td>Koszty</td>
                        <td style="text-align:right"><asp:Label runat="server" ID="lblLoss" ForeColor="Red"></asp:Label> </td>
                    </tr>
                    <tr>
                        <td>Zysk (koszt/wpływ)</td>
                        <td style="text-align:right"><asp:Label runat="server" ID="lblGainRate"></asp:Label> </td>
                    </tr> 
<%--                    <tr>
                        <td>Zysk % (wpływ/koszt)</td>
                        <td style="text-align:right"><asp:Label runat="server" ID="lblGainRatePerc"></asp:Label> </td>
                    </tr>  --%>                  <tr>
                        <td>Marża (zysk/cena sprzedazy)</td>
                        <td style="text-align:right"><asp:Label runat="server" ID="lblMarza"></asp:Label> </td>
                    </tr>                    <tr>
                        <td>Narzut (zysk/koszt)</td>
                        <td style="text-align:right"><asp:Label runat="server" ID="lblNarzut"></asp:Label> </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>
