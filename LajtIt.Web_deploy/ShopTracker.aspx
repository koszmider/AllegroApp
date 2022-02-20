<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ShopTracker.aspx.cs" Inherits="LajtIt.Web.ShopTracker" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Kasa sklepowa</h1>
    <asp:TextBox runat="server" ID="txbDate" Width="80"></asp:TextBox><asp:CalendarExtender
        ID="calDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDate">
    </asp:CalendarExtender><asp:LinkButton runat="server" ID="lbtnChangeDate" OnClick="lbtnChangeDate_Click" CausesValidation="false" Text="Pokaż"></asp:LinkButton>
    <div style="text-align:right">
                     <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:LinkButton runat="server" ID="lbtnReport" Text="Raport wpłat" OnClick="lbtnReport_Click" Visible="false"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lbtnReport" />
                        </Triggers>
                    </asp:UpdatePanel>
    </div>
    <asp:Panel runat="server" GroupingText="Kasa" ID="pnAddPayment">
        <table>
            <tr>
                <td>Typ wpłaty
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPaymentType" DataTextField="Name" DataValueField="ShopPaymentTypeId">
                    </asp:DropDownList>
                </td>
                <td style="width: 500px; text-align: right">Zamknięcie dnia (z raportu kasowego)</td>
                <td style="width: 200px; text-align:right;">
       
                </td>
            </tr>
            <tr>
                <td>
                    Wartość
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txbAmount" MaxLength="8" ValidationGroup="payment" Text="0"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" Text="*" ControlToValidate="txbAmount" ValidationGroup="payment"
                        Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="rev1" runat="server" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="payment"
                        ControlToValidate="txbAmount" Text="Niepoprawna wartość" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
                <td style="width:500px; text-align:right">
                    <asp:TextBox runat="server" ID="txbDailyReport" MaxLength="8" ValidationGroup="daily"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="*" ControlToValidate="txbDailyReport"  ValidationGroup="daily"
                        Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"  ValidationGroup="daily"
                        ControlToValidate="txbDailyReport" Text="Niepoprawna wartość" Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvReceiptSum" runat="server" ValidationGroup="daily" Type="Double"
                        ControlToValidate="txbDailyReport" Text="Suma paragonów i kasy się nie zgadza"  Display="Dynamic"></asp:CompareValidator>
                    
                </td>
            </tr>
            <tr>
                <td>
                    Uwagi (np. kto odbiera wpłatę)
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txbComment" MaxLength="254" ValidationGroup="payment"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ControlToValidate="txbComment" ValidationGroup="payment"
                        Display="Dynamic"></asp:RequiredFieldValidator> 
                </td>
                <td style="text-align:right;">Suma paragonów w tym dniu: <asp:Label runat="server" ID="lblReceiptSum"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    Osoba wprowadzająca
                </td>
                <td>
                    <asp:Label runat="server" ID="lblUserName"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnPaymentAdd" OnClick="btnPaymentAdd_Click" Text="Dodaj" ValidationGroup="payment"
                        OnClientClick="return confirm('Zapisać wprowadzane informacje?');" />
                    <script>

//---javascript -----
function ConfirmSubmit()
{
   Page_ClientValidate();
   if(Page_IsValid) {
       return confirm('Dodać raport dobowy?');
    }
 return Page_IsValid;
}

                    </script>
                 
                </td>
                <td   style="width:500px; text-align:right" >
                    <asp:Button runat="server" ID="btnDailyReport" OnClick="btnDailyReport_Click" Text="Dodaj raport dnia"  ValidationGroup="daily"
                        OnClientClick="ConfirmSubmit()" />
                 
                </td>
            </tr>
        </table>
        <p>
        </p>
        <p>
        </p>
    </asp:Panel>
    <table>
    <tr>
    <td style="width:700px">
    <asp:GridView runat="server" ID="gvShopTrackerReport" AutoGenerateColumns="false" OnRowDataBound="gvShopTrackerReport_OnDataBound">
        <Columns>
            <asp:HyperLinkField HeaderText="Zamówienie" DataTextField="OrderId" DataNavigateUrlFields="OrderId"  Target="_blank" DataNavigateUrlFormatString="/Order.aspx?id={0}"/>
            <asp:BoundField HeaderText="Data" DataField="InsertDate" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
            <asp:BoundField HeaderText="Osoba" DataField="UserName" />
            <asp:BoundField HeaderText="Typ" DataField="PaymentName" />
            <asp:BoundField HeaderText="Kwota" DataField="Amount" DataFormatString="{0:C}"  ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField HeaderText="Uwagi" DataField="Comment" />
            <asp:CheckBoxField HeaderText="Paragon" DataField="ReceiptCreated" />
        <asp:TemplateField>
            <ItemStyle Width="70" />
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Wystawić paragon?');" CommandArgument='<%# Eval("OrderPaymentId") %>' Text="Wystaw paragon" OnClick="lbtnChecked_Click" />
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
    </asp:GridView></td>
    <td><div style="color:Green; font-size: x-large";>W kasie: <asp:Label runat="server" ID="lblTotal"></asp:Label></div>
    <div style="color:Green; font-size: large";>Płatność kartą: <asp:Label runat="server" ID="lblCardTotal"></asp:Label></div></td>
    </tr>
    
    </table>
</asp:Content>
