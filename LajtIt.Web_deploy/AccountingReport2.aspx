<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountingReport2.aspx.cs" Inherits="LajtIt.Web.AccoutingReporting2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrintInvoices" />
            <asp:PostBackTrigger ControlID="btnEwidence" />
            <asp:PostBackTrigger ControlID="btnPrintInvoiceSummary" />
            <asp:PostBackTrigger ControlID="btnJPK_FA" />
        </Triggers>
        <ContentTemplate>
            Miesiąc:
            <asp:DropDownList runat="server" ID="ddlMonths">
            </asp:DropDownList>
            Firma:
            <asp:DropDownList runat="server" ID="ddlCompany"> 
                <asp:ListItem Value="78">M-Form sp. z o.o.</asp:ListItem>
                <asp:ListItem Value="1">Oświetlenie Lajtit</asp:ListItem>
            </asp:DropDownList>
            <asp:CheckBox runat="server" Id="chbNotLocked" Text="faktury niezamknięte" />
            <asp:Button runat="server" ID="btnShow" Text="Pokaż" OnClick="btnShow_Click" />
            <span style="position: absolute;">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </span>
            <br />
            <br />
            <table>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="gvResults" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvResults_OnRowDataBound">
                            <Columns>
                                <asp:BoundField DataField="CompanyName" HeaderText="Firma" />
                                <asp:BoundField DataField="Name" HeaderText="Rodzaj wpływu" />
                                <asp:BoundField DataField="SystemAll" HeaderText="Wpływy w systemie" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="ForEwidence" HeaderText="Do ewidencji" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="RealPayment" HeaderText="Wpływy na konto" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                            </Columns>


                        </asp:GridView>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnPrintInvoices" Text="Drukuj faktury" OnClick="btnPrintInvoices_Click" /><br />
                        <asp:Button runat="server" ID="btnPrintInvoiceSummary" Text="Drukuj podsumowanie faktur" OnClick="btnPrintInvoiceSummary_Click" /><br />
                        <asp:Button runat="server" ID="btnEwidence" Text="Drukuj ewidencje" OnClick="btnEwidence_Click" /><br />
                        <asp:Button runat="server" ID="btnJPK_FA" Text="Pobierz JPK_FA" OnClick="btnJPK_FA_Click" />
                    </td>
                </tr>
            </table>

            <asp:GridView EnableViewState="false" ID="gvInvoices" runat="server" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvInvoices_OnRowDataBound">
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFormatString="~/Order.aspx?id={0}" DataNavigateUrlFields="OrderId"
                        DataTextField="InvoiceNumber" Target="_blank" />
                    <asp:HyperLinkField DataNavigateUrlFormatString="~/Files/Invoices/{0}" DataNavigateUrlFields="InvoiceFileName"
                        DataTextField="InvoiceDate" Target="_blank" DataTextFormatString="{0:yyyy/MM/dd}"
                        HeaderText="Data wystawienia" />
                    <asp:BoundField DataField="InvoiceSellDate" HeaderText="Data sprzedaży" DataFormatString="{0:yyyy/MM/dd}" />
                    <asp:BoundField DataField="CompanyName" HeaderText="Nazwa firmy" />
                    <asp:BoundField DataField="CompanyAddress" HeaderText="Adres" />
                    <asp:BoundField DataField="Nip" HeaderText="Nip" />
                    <asp:BoundField DataField="CalculatedTotalBrutto" HeaderText="Razem Brutto" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="CalculatedTotalNetto" HeaderText="Razem Netto" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="CalculatedTotalVat" HeaderText="Razem VAT" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="CalculatedTotalNetto000" HeaderText="Netto dla VAT [0%]"
                        DataFormatString="{0:C}" />
                    <asp:BoundField DataField="CalculatedTotalVat000" HeaderText=" VAT [0%]" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="CalculatedTotalNetto023" HeaderText="Netto dla VAT [23%]"
                        DataFormatString="{0:C}" />
                    <asp:BoundField DataField="CalculatedTotalVat023" HeaderText=" VAT [23%]" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="AmountPaid" HeaderText="Zapłacono" DataFormatString="{0:C}" />
                    <asp:CheckBoxField Visible="false" DataField="ExcludeFromInvoiceReport" HeaderText="Rozliczone paragonem" />
                    <asp:BoundField DataField="AccountingType" HeaderText="Rozliczone" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
