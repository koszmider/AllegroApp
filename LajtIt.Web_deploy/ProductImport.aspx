<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductImport.aspx.cs" Inherits="LajtIt.Web.ProductImport" %>
    
<%@ Register Src="~/Controls/CostsControl.ascx" TagName="CostsControl" TagPrefix="uc" %>
<%@ Register Src="~/Controls/CostControl.ascx" TagName="CostControl" TagPrefix="uc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
    <colgroup>
    <col width="200" />
    <col width="700" /></colgroup>
        <tr>
            <td>
                Nazwa
            </td>
            <td>
                <asp:Literal runat="server" ID="litName"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                Data importu
            </td>
            <td>
                <asp:Literal runat="server" ID="litDate"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                Uwagi
            </td>
            <td>
                <asp:Literal runat="server" ID="litComment"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                Liczba produktów
            </td>
            <td>
                <asp:Literal runat="server" ID="litTotalQuantityOrdered"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                Sprzedanych produktów
            </td>
            <td>
                <asp:Literal runat="server" ID="litTotalQuantitySell"></asp:Literal> (<asp:Literal runat="server" ID="litTotalQuantitySellPerc"></asp:Literal>% )
            </td>
        </tr>
        <tr>
            <td>
                Koszt zakupu (netto)
            </td>
            <td>
                <asp:Literal runat="server" ID="litCost"></asp:Literal> 
            </td>
        </tr>

        <tr>
            <td>
                Wartość sprzedaży  (netto)
            </td>
            <td>
                <asp:Literal runat="server" ID="litTotalSell"></asp:Literal> ( <asp:Literal runat="server" ID="litTotalSellPerc"></asp:Literal>% )
            </td>
        </tr>
        

        <tr>
            <td>
                Koszt Allegro  (netto)
            </td>
            <td>
                <asp:Literal runat="server" ID="litAllegroCost"></asp:Literal> 
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel runat="server" ID="pnCosts" GroupingText="Koszty">
                    <div style="text-align: right;">
                        <asp:LinkButton runat="server" ID="lbtnCostNew" CausesValidation="false" Text="Dodaj nowy"
                            OnClick="lbtnCostNew_Click"></asp:LinkButton></div>
                    <asp:Panel GroupingText="Dodaj nowy koszt" runat="server" Visible="false" ID="pCostNew">
                        <uc:CostControl id="ucCostControl" runat="server">
                        </uc:CostControl>
                    </asp:Panel>
                    <uc:CostsControl runat="server" ID="ucCostsControl" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
