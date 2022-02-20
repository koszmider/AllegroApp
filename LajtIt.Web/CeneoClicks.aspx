<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CeneoClicks.aspx.cs" Inherits="LajtIt.Web.CeneoClicks" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Raport Ceneo</h1>
    <table width="100%">

        <tr>
            <td style="width:200px">Data</td>
            <td style="width:200px">Koszt przekliku</td>
           
            <td rowspan="2" style="text-align:right">
                <table style="width:100%"> 
                    <tr>
                        <td style="text-align:right">
                            <asp:Panel ID="pnReport"   runat="server">

                                <asp:UpdatePanel runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnReportUpload" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <input type="file" id="myfile" accept="document/*" name="myfile" runat="server" size="100" />&nbsp;&nbsp;&nbsp;
                                     
                                        <asp:Button ID="btnReportUpload" runat="server" Text="Wgraj raport (XLS)" OnClick="btnReportUpload_Click" CausesValidation="false" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr> 
            <td>
                <asp:TextBox runat="server" ID="txbDateFrom" Width="80"></asp:TextBox><asp:CalendarExtender
                    ID="calFrom" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateFrom"></asp:CalendarExtender>
                -
            
                <asp:TextBox runat="server" ID="txbDateTo" Width="80"></asp:TextBox><asp:CalendarExtender
                    ID="calTo" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateTo"></asp:CalendarExtender>
            </td>
            <td>

                <asp:TextBox runat="server" Width="80" ID="txbFrom" TextMode="Number"></asp:TextBox>-
                
                <asp:TextBox runat="server" Width="80" ID="txbTo" TextMode="Number"></asp:TextBox></td>

            
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSearch" OnClick="btnSearch_Click" Text="Szukaj" runat="server" /></td>
            <td colspan="3" style="text-align:right;">Ostatnie dane Ceneo: <asp:Label runat="server" ID="lblCeneo"></asp:Label></td>
        </tr>
    </table>
    <br /><br />
    <asp:GridView runat="server" ID="gvCeneo" AutoGenerateColumns="false" AllowSorting="true" OnSorting="gvCeneo_Sorting" ShowFooter="true"
    OnRowDataBound="gvCeneo_RowDataBound">
        <Columns>
            <asp:BoundField ItemStyle-Width="150"                                   SortExpression="SupplierName"  DataField="SupplierName" HeaderText="Dostawca" />
            <asp:BoundField ItemStyle-Width="150" FooterStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" SortExpression="TotalCost"     DataField="TotalCost"       DataFormatString="{0:C}" HeaderText="Koszt przeklików" />
            <asp:BoundField ItemStyle-Width="150" FooterStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" SortExpression="TotalSell"     DataField="TotalSell"       DataFormatString="{0:C}" HeaderText="Sprzedaż" />
            <asp:BoundField ItemStyle-Width="150" FooterStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" SortExpression="Rate"          DataField="Rate"            DataFormatString="{0:0.00}%" HeaderText="Marża" /> 
            <asp:BoundField ItemStyle-Width="150" FooterStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" SortExpression="TotalCostRate" DataField="TotalCostRate" DataFormatString="{0:0.00}%" HeaderText="Koszt udział" /> 
            <asp:BoundField ItemStyle-Width="150" FooterStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" SortExpression="TotalSellRate" DataField="TotalSellRate" DataFormatString="{0:0.00}%" HeaderText="Sprzedaż udział" /> 
        </Columns>

    </asp:GridView>

</asp:Content>
