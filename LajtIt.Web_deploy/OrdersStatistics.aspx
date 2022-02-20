<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OrdersStatistics.aspx.cs" Inherits="LajtIt.Web.OrdersStatistics" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Statystyka źródeł sprzedaży</h1>
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:RadioButton runat="server" ID="rbGroupSources0" GroupName="gr" Text="Nie grupuj źródeł zamówień" /></td>
                    <td>
                        <asp:RadioButton runat="server" ID="rbGroupSources1" GroupName="gr" Text="Grupuj źródła zamówień" Checked="true" /></td>
                    <td></td>
                </tr>
                <tr>
                    <td>

                        <asp:ListBox runat="server" ID="lbxGroupSources0" DataValueField="ShopId" DataTextField="Name" SelectionMode="Multiple"
                            Rows="6"                            ></asp:ListBox>

                    </td>
                    <td>

                        <asp:ListBox runat="server" ID="lbxGroupSources1" DataValueField="ShopTypeId" DataTextField="Name" SelectionMode="Multiple" Rows="6"></asp:ListBox>
                    </td>

                    <td> <asp:CheckBox runat="server" ID="chbShowLabels" Checked="false" Text="Pokaż wartości" />
                        <asp:Button runat="server" OnClick="btnSearch_Click" ID="btnSearch" Text="Pokaż" />
                    </td>
                </tr>
                <tr>
                    <td>Zakres danych:<br />
                        <asp:TextBox runat="server" ID="txbDateFrom" Width="80"></asp:TextBox><asp:CalendarExtender
                            ID="calDateFrom" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateFrom"></asp:CalendarExtender>
                        <asp:TextBox runat="server" ID="txbDateTo" Width="80"></asp:TextBox><asp:CalendarExtender
                            ID="calDateTo" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateTo"></asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:RadioButton runat="server" ID="rbByCount" GroupName="Show" Text="wg liczby zamówień"  /> 
                        <asp:RadioButton runat="server" ID="rbByAmout" GroupName="Show" Text="wg wartości zamówień" Checked="true"/><br />
                        <asp:RadioButton runat="server" ID="rbFinished" GroupName="fin" Text="sfinalizowane" Checked="true" /> 
                   
                        <asp:RadioButton runat="server" ID="rbAll" GroupName="fin" Text="wszystkie" /></td>
                </tr>
           
        
            </table>
            <asp:Chart ID="Chart1" runat="server" Width="1000" Height="400">
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
