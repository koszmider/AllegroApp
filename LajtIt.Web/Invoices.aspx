<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Invoices.aspx.cs"
    Inherits="LajtIt.Web.Invoices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Faktury</h1><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
  
        <ContentTemplate>
            Miesiąc:
            <asp:DropDownList runat="server" ID="ddlMonths">
            </asp:DropDownList>
            Firma:
            <asp:DropDownList runat="server" ID="ddlCompany"> 
                <asp:ListItem Value="78">M-Form sp. z o.o.</asp:ListItem>
                <asp:ListItem Value="1">Oświetlenie Lajtit</asp:ListItem>
            </asp:DropDownList><br />
            <asp:CheckBox runat="server" Id="chbNotLocked" Text="faktury niezamknięte" />
            <asp:CheckBox runat="server" Id="chbInvoiceWithReceipt" Text="faktury z paragonem" />
            <asp:CheckBox runat="server" Id="chbInvoiceWithReceiptNotPrinted" Text="faktury z paragonem niewydrukowanym" />
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
                        <asp:Button Visible="false" runat="server" ID="btnPrintInvoices" Text="Drukuj faktury" OnClick="btnPrintInvoices_Click" /><br />
                        <asp:Button Visible="false" runat="server" ID="btnPrintInvoiceSummary" Text="Drukuj podsumowanie faktur" OnClick="btnPrintInvoiceSummary_Click" /><br />
                                </td>
                </tr>
            </table>

            <asp:GridView EnableViewState="false" ID="gvInvoices" runat="server" AutoGenerateColumns="false" 
                ShowFooter="true" OnRowDataBound="gvInvoices_OnRowDataBound">
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFormatString="~/Order.aspx?id={0}" DataNavigateUrlFields="OrderId"
                        DataTextField="InvoiceNumber" Target="_blank" />
                    <asp:HyperLinkField DataNavigateUrlFormatString="~/Files/Invoices/{0}" DataNavigateUrlFields="InvoiceFileName"
                        DataTextField="InvoiceDate" Target="_blank" DataTextFormatString="{0:yyyy/MM/dd}"
                        HeaderText="Data wystawienia" />
                    <asp:BoundField DataField="InvoiceSellDate" HeaderText="Data sprzedaży" DataFormatString="{0:yyyy/MM/dd}" />
                    <asp:BoundField DataField="CompanyName" HeaderText="Nazwa firmy" />
                    <asp:BoundField DataField="Nip" HeaderText="Nip" />
                    <asp:BoundField DataField="TotalPrice" ItemStyle-HorizontalAlign="right" HeaderText="Razem Brutto" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="AmountPaid" ItemStyle-HorizontalAlign="right" HeaderText="Zapłacono" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="AccountingType" HeaderText="Rozliczone" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
