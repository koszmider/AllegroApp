<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OrdersStatisticsIncome.aspx.cs" Inherits="LajtIt.Web.OrdersStatisticsIncome" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Statystyka zyskowności</h1>
    <asp:UpdatePanel runat="server">

        <ContentTemplate>
            <table>
            
                <tr>
                    <td>Zakres danych wg daty wysyłki:<br />
                        <asp:TextBox runat="server" ID="txbDateFrom" Width="80"></asp:TextBox><asp:CalendarExtender
                            ID="calDateFrom" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateFrom"></asp:CalendarExtender>
                        <asp:TextBox runat="server" ID="txbDateTo" Width="80"></asp:TextBox><asp:CalendarExtender
                            ID="calDateTo" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateTo"></asp:CalendarExtender>
                        <asp:Button runat="server" OnClick="btnSearch_Click" ID="btnSearch" Text="Pokaż" />
                    </td>
               
                </tr>


            </table>

            <asp:GridView ID="gvOrders" AutoGenerateColumns="false" runat="server" OnSorting="gvOrders_Sorting" AllowSorting="true">
                <Columns>
                    <asp:BoundField ItemStyle-Width="250" SortExpression="Name" DataField="Name" HeaderText="Sklep" />
                    <asp:BoundField ItemStyle-Width="100" SortExpression="Total" ItemStyle-HorizontalAlign="right" DataField="Total" HeaderText="Saldo" DataFormatString="{0:C}" />
                    <asp:BoundField ItemStyle-Width="100" SortExpression="TotalIncome" ItemStyle-HorizontalAlign="right" DataField="TotalIncome" HeaderText="Wpływy" DataFormatString="{0:C}" />
                    <asp:BoundField ItemStyle-Width="100" SortExpression="TotalOutcome" ItemStyle-HorizontalAlign="right" DataField="TotalOutcome" HeaderText="Koszty" DataFormatString="{0:C}" /> 
                    <asp:BoundField ItemStyle-Width="100" SortExpression="Marza" ItemStyle-HorizontalAlign="right" DataField="Marza" HeaderText="Marża" DataFormatString="{0:0.00}%" />
                    <asp:BoundField ItemStyle-Width="100" SortExpression="Narzut" ItemStyle-HorizontalAlign="right" DataField="Narzut" HeaderText="Narzut" DataFormatString="{0:0.00}%" />
                </Columns>
            </asp:GridView>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
