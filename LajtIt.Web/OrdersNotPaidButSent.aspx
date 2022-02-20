<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OrdersNotPaidButSent.aspx.cs" Inherits="LajtIt.Web.OrdersNotPaidButSent" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Zaległe pobraniówki</h1>
    <table>
        <tr><td>Kurier</td>
        <td>Numery listów przewozowych</td></tr>
        <tr valign="top">
        <td><asp:DropDownList ID="ddlShippingCompany" 
        OnSelectedIndexChanged="ddlShippingCompany_OnSelectedIndexChanged" AutoPostBack="true"
        runat="server" DataTextField="Name" DataValueField ="ShippingCompanyId" ></asp:DropDownList></td>
            <td>
                <asp:TextBox runat="server" ID="txbNumbers" TextMode="MultiLine" Rows="4" Columns="30"></asp:TextBox>
            </td>
            <td>
                <asp:Panel runat="server" ID="pNumbers" Visible="false">
                    <table>
                        <tr>
                            <td>
                                Liczba zamówień:
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="litCount"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Wartość:
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="litAmount"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Data wpływu:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txbDate" Width="80"></asp:TextBox><asp:CalendarExtender
                                    ID="calDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDate">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Źródło wpływu:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlOrderPaymentTypes" DataTextField="Name" DataValueField="PaymentTypeId">
                                <asp:ListItem Value="">wybierz</asp:ListItem>
                                <%--<asp:ListItem Value="">--------- RTP----------</asp:ListItem>
                                <asp:ListItem Value="2">Główne - mBiznes - VAT 23%</asp:ListItem> --%>
                                <asp:ListItem Value="13">Główne - ING - VAT 23%</asp:ListItem> 
                                <asp:ListItem Value="21">Główne - ING M-Form sp z o.o. - VAT 23%</asp:ListItem> 
                                <%--<asp:ListItem Value="">-------------------</asp:ListItem>
                                <asp:ListItem Value="">--------- Mag----------</asp:ListItem>
                                <asp:ListItem Value="">-------------------</asp:ListItem>
                                <asp:ListItem Value="16">ING - VAT 23%</asp:ListItem>
                                <asp:ListItem Value="3">eMax - VAT 0%</asp:ListItem> 
                                <asp:ListItem Value="">-------------------</asp:ListItem>
                                <asp:ListItem Value="">--------- BAB----------</asp:ListItem>
                                <asp:ListItem Value="">-------------------</asp:ListItem>
                                <asp:ListItem Value="17">eKonto - VAT 0%</asp:ListItem> --%>

                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rfvPaymentType" Text="wybierz" ControlToValidate="ddlOrderPaymentTypes" />
                            </td>
                        </tr>
                        <tr>
                        <td colspan="2"><asp:Button runat="server" ID="btnAddPayments" OnClick="btnAddPayments_Click" Text="Zapisz płatności" OnClientClick="return confirm('Czy liczba zamówień i kwota wpływu zagadza się? Jeśli tak, zapisz. Jeśli nie, sprawdź ponownie i ew. dodaj płatności ręcznie');" />&nbsp;&nbsp;<asp:LinkButton runat="server" CausesValidation="false" Text="Anuluj" OnClick="lnbCancel_Click"></asp:LinkButton></td></tr>
                    </table>
                </asp:Panel>
            </td>
            <td><asp:Label runat="server" ID="lblOrdersNotFound" EnableViewState="false"></asp:Label> </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" Text="Sprawdź" OnClick="txbCheck_Click" CausesValidation="false" />
            </td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="gvOrders" AutoGenerateColumns="false" Width="100%" ShowFooter=true
        OnRowDataBound="gvOrders_OnRowDataBound">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="OrderId" DataTextField="OrderId" DataNavigateUrlFormatString="Order.aspx?id={0}"
                ItemStyle-Width="60" ItemStyle-HorizontalAlign="center" HeaderText="Id" />
             <asp:TemplateField HeaderText="L. dni od wysłania">
                <ItemStyle Width="80" HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlUrl" Target="_blank"></asp:HyperLink>


                </ItemTemplate>
            </asp:TemplateField>
               <asp:BoundField DataField="ClientName" HeaderText="Klient" HtmlEncode="false" />
            <asp:BoundField DataField="AmountToPay" HeaderText="Do zapłacenia" ItemStyle-HorizontalAlign="Right"
                ItemStyle-Width="60" DataFormatString="{0:C}" />
            <asp:BoundField DataField="AmountPaid" HeaderText="Zapłacono" ItemStyle-HorizontalAlign="Right"
                ItemStyle-Width="60" DataFormatString="{0:C}" />
            <asp:BoundField DataField="AmountBalance" HeaderText="Balans" ItemStyle-HorizontalAlign="Right"
                ItemStyle-Width="80" DataFormatString="{0:C}" />
            <asp:BoundField DataField="CreateDate" HeaderText="Data<br>zamówienia" ItemStyle-Width="80"
                HtmlEncode="false" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy-MM-dd<br>HH:mm}" />
            <%--    <asp:BoundField DataField="LastStatusChangeDate" HeaderText="Data<br>zmiany statusu" ItemStyle-Width="80" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy-MM-dd<br>HH:mm}" />--%>
            <asp:BoundField DataField="SentDate" HeaderText="Data<br>wysłania" ItemStyle-Width="80"
                HtmlEncode="false" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy-MM-dd<br>HH:mm}" />
            <asp:TemplateField HeaderText="L. dni od wysłania">
                <ItemStyle Width="80" HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Literal runat="server" ID="litDays"></asp:Literal></ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
