<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Payments.aspx.cs" Inherits="LajtIt.Web.Payments" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Zarządzanie płatnościami</h1>
    <table style="width:100%">
        <tr>
            <td style="width: 170px">Miesiąc</td>
            <td style="width: 170px">Data</td>
            <td style="width: 120px">Konto firmy </td>
            <td style="width: 220px"></td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="ddlMonth" Width="100%">
                </asp:DropDownList>
            </td>
            <td >
                   <asp:TextBox runat="server" ID="txbDateFrom" Width="70"></asp:TextBox><asp:CalendarExtender
                        ID="calDateFrom" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateFrom">
                    </asp:CalendarExtender>
                    -<asp:TextBox runat="server" ID="txbDateTo" Width="70"></asp:TextBox><asp:CalendarExtender
                        ID="calDateTo" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateTo">
                    </asp:CalendarExtender>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCompany" DataValueField="CompanyId" AppendDataBoundItems="true"
                    OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true"
                    DataTextField="Name">
                </asp:DropDownList>
            </td>
            <td>
               

                <asp:Button ID="btnSearch" OnClick="btnSearch_Click" Text="Szukaj" runat="server" />
            </td>
            <td></td>
            </tr>
         <tr>
             <td colspan="5">
                 <b>Szukaj:</b><br />
                  <asp:CheckBox runat="server" ID="chbNotAssigned" Text="Bez przypisanego sposobu rozliczenia" />
                  <asp:CheckBox runat="server" ID="chbReceiptReady" Text="Do wystawienia paragonu" /><br />
                 
                  <asp:CheckBox runat="server" ID="chbReceipt1" Text="Z paragonem" />
                  <asp:CheckBox runat="server" ID="chbReceipt0" Text="Bez paragonu" />
             </td>
         </tr>
        <tr>
            <td colspan="5"><b>Rodzaj płatności: </b>
                <asp:CheckBoxList runat="server" RepeatDirection="Horizontal" ID="chbOrderPaymentType" DataTextField="Name" DataValueField="PaymentTypeId"></asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td colspan="5"><b>Rodzaj rozliczenia płatności:</b>
                <asp:CheckBoxList runat="server" RepeatDirection="Horizontal" ID="chbAccountingType"
                    DataTextField="Name" DataValueField="AccountingTypeId">
                </asp:CheckBoxList> 
                <b>Szukaj FV:</b><br />
                <asp:CheckBox runat="server" ID="chbInvoice1" Text="Faktura TAK" />
                (<asp:CheckBox runat="server" ID="chbInvoiceNip1" Text="nip tak" />
                <asp:CheckBox runat="server" ID="chbInvoiceNip0" Text="nip nie" />)
                <asp:CheckBox runat="server" ID="chbInvoice0" Text="Faktura NIE" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <style>
        table.pad tr td {
            padding: 3px;
        }
    </style>
    <table style="width: 100%">
        <tr valign="top">
            <td style="width: 1100px">
                <asp:GridView runat="server" CssClass="pad" ID="gvPayments" DataKeyNames="PaymentId" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvPayments_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Width="60" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LitId"></asp:Literal>
                                <asp:CheckBox runat="server" ID="chbPOrder" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chbPOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbPOrder');" />
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField DataNavigateUrlFields="OrderId" DataTextField="OrderId" DataNavigateUrlFormatString="/Order.aspx?id={0}" Target="_blank" />
                        <asp:BoundField DataField="InsertDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data wpłaty" />
                        <asp:BoundField DataField="Amount" DataFormatString="{0:C}" HeaderText="Kwota" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="AmountBalance" DataFormatString="{0:C}" HeaderText="Wartość zam." ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="PaymentName" DataFormatString="{0:C}" HeaderText="Rodzaj wpłaty" />
                        <asp:BoundField DataField="InvoiceNumber" HeaderText="Numer faktury" />
                        <asp:BoundField DataField="InvoiceDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data wystawienia" />
                        <asp:BoundField DataField="InvoiceSellDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data sprzedaży" />
                         
                        <asp:BoundField DataField="AccountingName" HeaderText="Rodzaj rozliczenia" />

                    </Columns>
                </asp:GridView>
            </td>


            <td>
                <h2>Bieżący wynik</h2>

                <table>
                    <tr>

                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblCurrent"></asp:Label></td>
                    </tr>
                </table>
                <asp:Panel runat="server" ID="pnSummary" Visible="false">
                <h2>System</h2>
                <table>
                    <tr>
                        <td>Razem:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblTotalAccount"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Ewidencja:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblEwidencja"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Faktury:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblFaktury"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Kasa fiskalna:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblKasa"></asp:Label></td>
                    </tr>
                </table>
                <h2>Bank</h2>
                <table>
                    <tr>
                        <td>Wpływy:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblCredit"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Wydatki:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblDebit"></asp:Label></td>
                    </tr>
                </table>
                    </asp:Panel>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <h2>Akcje</h2>
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton runat="server" ID="lbtnOrderReceipt" Text="Drukuj paragony" CausesValidation="false" OnClientClick="return confirm('Czy wydrukować paragony dla zaznaczonych płatności?');" OnClick="lbtnOrderReceipt_Click"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton  Visible="false" runat="server" ID="lbtnInvoiceSummary" Text="Podsumowanie faktur" OnClick="lbtnInvoiceSummary_Click"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton Visible="false" runat="server" ID="lbtnEvidence" Text="Ewidencja" OnClick="lbtnEvidence_Click"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton Visible="false" runat="server" ID="lbtnInvoices" Text="Faktury" OnClick="lbtnInvoices_Click"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton Visible="false" runat="server" ID="lbtnJPK" Text="JPK_FA" OnClick="lbtnJPK_Click"></asp:LinkButton></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lbtnInvoiceSummary" />
                        <asp:PostBackTrigger ControlID="lbtnEvidence" />
                        <asp:PostBackTrigger ControlID="lbtnInvoices" />
                        <asp:PostBackTrigger ControlID="lbtnJPK" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>


    Zmień rodzaj rozliczenia dla zaznaczonych:
    <asp:DropDownList runat="server" ID="ddlAccountingType" DataTextField="Name" DataValueField="AccountingTypeId" AppendDataBoundItems="true">
        <asp:ListItem></asp:ListItem>

    </asp:DropDownList><asp:Button runat="server" ID="btnChange" OnClick="btnChange_Click" OnClientClick="return confirm('Zmienić?')" Text="Zmień" />






</asp:Content>

