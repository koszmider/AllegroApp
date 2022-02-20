<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Costs.aspx.cs" Inherits="LajtIt.Web.Costs" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Src="~/Controls/CostControl.ascx" TagName="CostControl" TagPrefix="uc" %>
<%@ Register Src="~/Controls/CostsControl.ascx" TagName="CostsControl" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Koszty</h1>
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>
    <table>
        <tr>
            <td>Miesiąc
            </td>
            <td>Typ:
            </td>
            <td>Faktura dla
            </td>
            <td>Faktura od
            </td>
            <td>Uwagi
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="ddlMonth">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCostTypeSearch" DataValueField="CostTypeId"
                    DataTextField="Name" />
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCompanyOwner" DataValueField="CompanyId" AppendDataBoundItems="true"
                    DataTextField="Name">
                    <asp:ListItem Value="0">-- wszystkie --</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCompany" DataValueField="CompanyId" AppendDataBoundItems="true"
                    DataTextField="Name">
                    <asp:ListItem Value="0">-- wszystkie --</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" ID="txbComment" ></asp:TextBox></td>
            <td>
                <asp:CheckBox runat="server" ID="chbIsForAccouting" Text="Niesprawdzone" />
            </td>
            <td>
                <asp:Button ID="btnSearch" OnClick="btnSearch_Click" Text="Szukaj" runat="server" />
            </td>
        </tr>
        <tr><td><asp:CheckBox runat="server" ID="chbToPay" Text="Gotowe do przelewu" /></td>
            <td>
                <asp:DropDownList runat="server" ID="ddlBatch" DataValueField="BatchId" DataTextField="BatchDate" AppendDataBoundItems="false">
                    

                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCostDocumentType" Width="150">
                    <asp:ListItem Value="0">-- pokaż wszystkie --</asp:ListItem>
                    <asp:ListItem Value="1">faktury</asp:ListItem>
                    <asp:ListItem Value="2">korekty</asp:ListItem>
                    <asp:ListItem Value="3">korekty opłacone</asp:ListItem>
                    <asp:ListItem Value="4">korekty nieopłacone</asp:ListItem>
                   <%-- <asp:ListItem Value="3">faktury z korektami</asp:ListItem>--%>
                    
                </asp:DropDownList>
            </td>
        </tr>
    </table>

    <asp:UpdatePanel runat="server" ID="upElixir">
        <ContentTemplate>
    <asp:LinkButton runat="server" ID="lbtnElixir" Text="Pobierz plik przelewów" OnClick="lbtnElixir_Click"></asp:LinkButton>
            </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lbtnElixir" />
        </Triggers>
    </asp:UpdatePanel>
 <asp:Panel runat="server" Visible="false">
    <asp:Panel runat="server" Visible="false" id="pChart">
    <asp:RadioButtonList id="rbtnlMonths" runat="server" RepeatDirection="Horizontal" >
        <asp:ListItem Value="0">wszystkie</asp:ListItem>
        <asp:ListItem Value="12">12 miesięcy</asp:ListItem>
        <asp:ListItem Value="24" Selected="True">24 miesięcy</asp:ListItem>
        <asp:ListItem Value="36">36 miesięcy</asp:ListItem>
    </asp:RadioButtonList>
    <asp:Chart ID="Chart1" runat="server" Width="1000">
        <Series>
            <asp:Series Name="Series1">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
                <AxisX IntervalAutoMode="VariableCount">
                </AxisX>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart></asp:Panel>
     </asp:Panel>
    <uc:CostsControl ID="ucCostsControl" runat="server"></uc:CostsControl>
</asp:Content>
