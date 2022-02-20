<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Order.aspx.cs" Inherits="LajtIt.Web.AllegroOrder" %>

<%@ Register Src="~/Controls/ItemOrderGrid.ascx" TagName="ItemOrderGrid" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Products.ascx" TagName="Products" TagPrefix="uc" %>
<%@ Register Src="~/Controls/OrderPayment.ascx" TagName="OrderPayment" TagPrefix="uc" %>
<%@ Register Src="~/Controls/OrderComplaint.ascx" TagName="OrderComplaint" TagPrefix="uc" %>
<%@ Register Src="~/Controls/OrderLog.ascx" TagName="OrderLog" TagPrefix="uc" %>
<%@ Register Src="~/Controls/OrderShipping.ascx" TagName="OrderShipping" TagPrefix="uc" %>
<%@ Register Src="~/Controls/OrderReceipt.ascx" TagName="OrderReceipt" TagPrefix="uc" %>
<%@ Register Src="~/Controls/OrderStatusHistory.ascx" TagName="OrderStatusHistory"
    TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function info(c) {
            if (!c.checked)
                return confirm('Odznaczenie opcji faktura spowoduje usunięcie zapisanych danych. Kontynuować?');

            return true;
        }

    </script>
    <div>
        <a name="order"></a>
        <h1>
            <asp:Literal runat="server" ID="litEmail" />&nbsp;&nbsp;&nbsp;<asp:HyperLink runat="server" ID="hlOrders" Target="_blank" Visible="false"></asp:HyperLink></h1>
        <div style="text-align: right;">
            <asp:Literal runat="server" ID="litDate"></asp:Literal>&nbsp;<asp:LinkButton runat="server"
                ID="lbtnChangeDate" OnClick="lbtnChangeDate_Click" Text="Zmień"></asp:LinkButton>
            <asp:Panel runat="server" ID="pChangeDate" GroupingText="Zmiana daty i źródła zamówienia"
                Visible="false">
                <asp:TextBox MaxLength="20" runat="server" ID="txbChangeDate" />
                <asp:DropDownList runat="server" ID="ddlShop" DataValueField="ShopId"
                    DataTextField="Name" />
                <br />
                <asp:LinkButton runat="server" ID="lbtnChangeDateCancel" OnClick="lbtnChangeDateCancel_Click"
                    Text="Anuluj"></asp:LinkButton>
                <asp:Button runat="server" ID="lbtnChangeDateSave" OnClick="lbtnChangeDateSave_Click"
                    Text="Zapisz"></asp:Button>
            </asp:Panel>
        </div>
        <table style="width:100%">
            <tr>
                <td >
                    <table style="border: 1px solid black;" border="1">
                        <tr style="background-color: Silver;">
                            <td></td>

                            <td>Dane do wysyłki
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chbInvoice" Text="Faktura" OnCheckedChanged="chbInvoice_Changed"
                                    AutoPostBack="true" /><asp:DropDownList runat="server" ID="ddlCompany" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"  >
                                        <asp:ListItem Value="1">Oświetlenie Lajtit</asp:ListItem>
                                        <asp:ListItem Value="78">M-Form sp. z o.o.</asp:ListItem>
                                    </asp:DropDownList>
                                <asp:CheckBox runat="server" ID="chbParagon" Text="Paragon" OnCheckedChanged="chbParagon_Changed"
                                    AutoPostBack="true" />
                                <asp:LinkButton runat="server" ID="btnParagonGet" Text=">>>" OnClick="btnParagonGet_Click"
                                    OnClientClick="return confirm('Czy chcesz wydrukować paragon?');" />
                            </td>
                            <td colspan="2">Dodatkowe ustawienia</td>
                        </tr>
                        <tr>
                            <td>Imię i nazwisko
                            </td>
                            <%--             <td>
                    <asp:TextBox runat="server" ID="txbFirsLastName" />
                </td>--%>
                            <td>
                                <asp:TextBox runat="server" ID="txbShipmentFirstName" Enabled="false" Width="140" 
                                    ValidationGroup="add"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txbShipmentLastName" Enabled="false" Width="140" 
                                    ValidationGroup="add"></asp:TextBox>
                            </td>
                            <td>NIP: <asp:TextBox runat="server" ID="txbInvoiceCountryCode" Enabled="false" Width="20"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txbInvoiceNIP" Enabled="false" Width="133" MaxLength="20"></asp:TextBox>
                                 <asp:LinkButton runat="server" ID="lbtnAccoutingType" Text="rozlicz fv" />
                               
            <asp:ModalPopupExtender ID="mpeAccoutingType" runat="server"
                TargetControlID="lbtnAccoutingType"
                PopupControlID="Panel1"
                BackgroundCssClass="modalBackground"
                DropShadow="true" 
                PopupDragHandleControlID="Panel1" />
 
            <asp:Panel runat="server" ID="Panel1" GroupingText="Paragon" BackColor="White" Style="width: 400px; background-color: white; height: 250px; padding: 10px">
             
                <asp:RadioButtonList ID="rblAccountingType" runat="server">
<asp:ListItem Value="0">domyślnie</asp:ListItem>
<asp:ListItem Value="1">ewidencja</asp:ListItem>
<asp:ListItem Value="3">kasa fiskalna</asp:ListItem>

                </asp:RadioButtonList>
                <asp:Button runat="server" ID="btnAccountingType" Text="Wystaw paragon" UseSubmitBehavior="true" 
                    OnClick="btnAccountingType_Click"  ValidationGroup="receipt" 
                    OnClientClick="return confirm('Zmienić sposób rozliczenia faktury?');" />
                <asp:LinkButton runat="server" ID="LinkButton1" onClick="btnCancel_Click" Text="Anuluj" />
                </asp:Panel> 






                            </td>
                            <td>Data&nbsp;dostawy</td>
                            <td><asp:TextBox runat="server" Id="txbDeliveryDate" TextMode="Date"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Nazwa firmy
                            </td>
                            
                            <td>
                                <asp:TextBox runat="server" ID="txbShipmentCompanyName" Enabled="false" Width="300" 
                                    ValidationGroup="add"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txbInvoiceCompanyName" Enabled="false" Width="300"></asp:TextBox>
                            </td>
                            <td colspan="2" style="text-align:center">Termin: <asp:Label runat="server" ID="lblDeliveryInfo"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Adres
                            </td> 
                            <td>
                                <asp:TextBox runat="server" ID="txbShipmentAddress" Enabled="false" Width="300" ValidationGroup="add"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txbInvoiceAddress" Enabled="false" Width="300"></asp:TextBox>
                            </td>
                            <td colspan="2" ><asp:CheckBox runat="server" ID="chbPriority" Text="Realizuj priorytetowo"   /></td>
                        </tr>
                        <tr>
                            <td>Kod/miasto/kraj
                            </td> 
                            <td>
                                <asp:TextBox runat="server" ID="txbShipmentPostCode" Enabled="false" Width="50" ValidationGroup="add" ></asp:TextBox><asp:RegularExpressionValidator
                                        ID="revShipmentPostCode" runat="server" ControlToValidate="txbShipmentPostCode"  
                                        ValidationExpression="^\d{2}\-\d{3}$"
                                        Text="*" ValidationGroup="add" />
                                <asp:TextBox runat="server" ID="txbShipmentCity" Enabled="false" Width="150" ValidationGroup="add"></asp:TextBox>
                                <asp:DropDownList runat="server" id="ddlShipmentCountry"  Enabled="false" DataValueField="CountryCode" DataTextField="CountryCode"></asp:DropDownList>
                                <asp:Image runat="server" ID="imgCountryCode" Width="20" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txbInvoicePostCode" Enabled="false" Width="50"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txbInvoiceCity" Enabled="false" Width="235"></asp:TextBox>
                            </td>
                            <td colspan="2" style="text-align:right"><asp:Button runat="server" ID="btnSaveOptions" Text="Zapisz" OnClick="btnSaveOptions_Click" /></td>
                        </tr>
                        <tr>
                            <td>Adres pełny
                            </td>
                            <%--  <td>
                    <asp:TextBox runat="server" ID="txbAddressFull" />
                </td>--%>
                            <td>
                                <asp:TextBox runat="server" ID="txbShipmentAddressFull" TextMode="MultiLine" Rows="4"  onclick="this.select();"
                                    Columns="30" Width="300"  BackColor="LightGray" ReadOnly="true" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txbInvoiceAddressFull" TextMode="MultiLine" Rows="4"
                                    Columns="30" Width="300" Enabled="false" />
                            </td>
                            <td colspan="2" style="text-align:center;"><asp:Label runat="server" ID="lblShopInfo"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Telefon
                            </td>
                            <%--  <td>
                    <asp:TextBox runat="server" ID="txbPhone" />
                </td>--%>
                            <td>
                                <asp:TextBox runat="server" ID="txbShipmentPhone" Enabled="false" Width="140" ValidationGroup="add" /><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator2" runat="server" Text="*" ValidationGroup="add" ControlToValidate="txbShipmentPhone" />
                                <asp:TextBox runat="server" ID="txbShipmentPhone2" Enabled="false" Width="140" ValidationGroup="add" />
                            </td>
                            <td rowspan="2">
                                <asp:TextBox runat="server" ID="txbInvoiceComment" TextMode="MultiLine" Rows="2"
                                    Columns="30" Width="300" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td>Email
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txbEmail" Enabled="false" Width="300" ValidationGroup="add" /><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator1" runat="server" Text="*" ValidationGroup="add" ControlToValidate="txbEmail" /><asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator1" runat="server" ControlToValidate="txbEmail"
                                        ValidationExpression="^([0-9a-zA-Z\+]([-\.\w]*[0-9a-zA-Z\+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                        Text="*" ValidationGroup="add" />
                            </td>
                            <td colspan="2" rowspan="2"> 
                        -
                        <asp:LinkButton runat="server" ID="btnPrePayment" Text="Dokument zaliczkowy" OnClick="btnPrePayment_Click"
                            OnClientClick="return confirm('Czy chcesz wydrukować dokument zaliczkowy?');" /><br />
                        -
                        <asp:LinkButton runat="server" ID="btnOrderPrint" Text="Drukuj zamówienie" OnClick="btnOrderPrint_Click"
                            OnClientClick="" /><br />
                        -
                        <asp:LinkButton runat="server" Text="Przenieś do nowego zam." ID="lbtnOrderNew" OnClick="lbtnOrderNew_Click"
                            CausesValidation="false" OnClientClick="return confirm('Czy chcesz przenieść wybrane produkty do nowego zamówienia? Tylko produkty dotąd niewydane mogą być przenoszone');" /><br />
                       
                 
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button runat="server" ID="btnEditShipmentAddress" Text="Edytuj" OnClick="btnEditShipmentAddress_Click" />
                                <asp:Button runat="server" ID="btnEditShipmentSave" Text="Zapisz" OnClientClick="if(!confirm('Zapisać zmiany?')) return false;"
                                    OnClick="btnEditShipmentSave_Click" Visible="false" ValidationGroup="add" />
                                <asp:Button runat="server" ID="btnEditShipmentCancel" Text="Anuluj" OnClick="btnEditShipmentCancel_Click"
                                    Visible="false" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnInvoiceCreate" Text="Twórz FVAT z danych do wysyłki"
                                    OnClick="btnInvoiceCreate_Click" />
                                <asp:Button runat="server" ID="btnEditInvoiceSave" Text="Zapisz" OnClientClick="return confirm('Zapisać zmiany?');"
                                    OnClick="btnEditInvoiceSave_Click" Visible="false" />
                                <asp:LinkButton runat="server" ID="btnEditInvoiceCancel" Text="Anuluj" OnClick="btnEditInvoiceCancel_Click"
                                    Visible="false" />
                                <asp:Button runat="server" ID="btnInvoiceGet" Text="Faktura" OnClick="btnInvoiceGet_Click"
                                    OnClientClick="return confirm('Czy chcesz wydrukować fakturę?');" />
                                <asp:Button runat="server" ID="btnInvoiceCorrection" Text="Korekta" OnClick="btnInvoiceCorrection_Click" Visible="false"
                                    OnClientClick="return confirm('Czy chcesz wydrukować korektę fakturę?');" />
                                <asp:LinkButton runat="server" ID="btnEditInvoiceAddress" Text="Edytuj" OnClick="btnEditInvoiceAddress_Click"
                                    Visible="false" />
                                <asp:Button runat="server" ID="btnInvoiceLock" Text="Zablokuj" OnClick="btnInvoiceLock_Click"
                                    OnClientClick="return confirm('Czy chcesz ostatecznie zakończyć edycję faktury? Jej ponowne otwarcie nie będzie możliwe.');" />
                                <asp:LinkButton runat="server" ID="lbtnUnlock" Text="Odblokuj" OnClick="lbtnUnlock_Click"
                                    OnClientClick="return confirm('Czy chcesz odblokować fakturę? ');" />
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnInvoiceGet" />
                                        <asp:PostBackTrigger ControlID="btnParagonGet" />
                                        <asp:PostBackTrigger ControlID="btnPrePayment" />
                                        <asp:PostBackTrigger ControlID="btnOrderPrint" />
                                        <asp:PostBackTrigger ControlID="btnInvoiceCorrection" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td> 
            </tr>
        </table>
        <div style="width: 500px; float: left;">
            <asp:UpdatePanel runat="server" ID="upSuppliers">
                <ContentTemplate>
                    <asp:CheckBoxList ID="chblSuppliers" runat="server" DataValueField="SupplierId" DataTextField="Name"
                        RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chblSuppliers_OnSelectedIndexChanged">
                    </asp:CheckBoxList>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="position: absolute;">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upSuppliers">
                    <ProgressTemplate>
                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </div>
        <div style="text-align: right;">



            <script type="text/javascript">

                function goAutoCompl() {
                    $("#<%=txbProductCode.ClientID %>").autocomplete({
                                source: function (request, response) {
                                    $.ajax({
                                        url: '<%=ResolveUrl("~/AutoComplete.asmx/GetProducts") %>',
                                        data: "{ 'query': '" + request.term + "'}",
                                        dataType: "json",
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        success: function (data) {
                                            response($.map(data.d, function (item) {
                                                return {
                                                    label: item.split('|')[0],
                                                    val: item.split('|')[1]
                                                }
                                            }))
                                        },
                                        error: function (response) {
                                            alert(response.responseText);
                                        },
                                        failure: function (response) {
                                            alert(response.responseText);
                                        }
                                    });
                                },
                                select: function (e, i) {
                                    $("#<%=hfProductCatalogId.ClientID %>").val(i.item.val);
                                },
                                minLength: 1
                            });

                            }

                            $(document).ready(function () {
                                goAutoCompl();
                            });
                            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                            function EndRequestHandler(sender, args) {
                                goAutoCompl();
                            }

            </script>
            <asp:UpdatePanel runat="server" ID="upNewProduct">
                <ContentTemplate>
                    Kod/Ean/Nazwa:
                    <asp:TextBox runat="server" ID="txbProductCode" MaxLength="50" Width="150"></asp:TextBox><asp:Button ID="btnProductAdd"
                        runat="server" OnClick="btnProductAdd_Click" Text="Dodaj" />&nbsp;&nbsp;&nbsp;
    <asp:HiddenField ID="hfProductCatalogId" runat="server" />


                    <span style="position: absolute; width: 10px;">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upNewProduct">
                            <ProgressTemplate>
                                <img src="Images/progress.gif" style="height: 20px" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </span>
                </ContentTemplate>
            </asp:UpdatePanel> 
        </div>
        <uc:ItemOrderGrid runat="server" ID="ucItemOrderGrid" />
        <asp:Label runat="server" ID="lblLockInfo" ForeColor="Red" Text="Dodawanie produktów oraz modyfikowanie wartości jest zablokowane. By je odblokować należy dokonać korekty płatności" Visible="false"></asp:Label>
        <table style="width: 100%;">
            <tr style="vertical-align: top;">
                <td style="width: 60%">
                    <uc:OrderShipping runat="server" ID="ucOrderShipping" />
                </td>
                <td style="width: 40%; text-align: right;">
                    <h3 style="text-align: right">Do zapłaty:
                        <asp:Label runat="server" ID="lblAmountToPay"></asp:Label><br />
                        <asp:Label runat="server" ID="lblAmountToPayCurrency"></asp:Label><br />
                        Zapłacono:
                        <asp:Label runat="server" ID="lblAmountPaid"></asp:Label><br />
                        <asp:Label runat="server" ID="lblAmountBalance"></asp:Label></h3>
                </td>
                <td>

                    <asp:ImageButton runat="server" ID="imgbCash" ImageUrl="~/Images/cash.png"   
                        OnClick="imgbCash_Click" Width="100" />
                                <asp:Label runat="server" ID="lblOK"></asp:Label>
            <asp:ModalPopupExtender ID="mpeReceipt" runat="server"
                TargetControlID="lblOK"
                PopupControlID="pnShopProducts"
                BackgroundCssClass="modalBackground"
                DropShadow="true"
                
                PopupDragHandleControlID="Panel1" />
 
            <asp:Panel runat="server" ID="pnShopProducts" GroupingText="Paragon" BackColor="White" Style="width: 700px; background-color: white; height: 550px; padding: 10px">
            <uc:OrderReceipt runat="server" ID="ucOrderReceipt" ValidationGroup='<%= btnSaveReceipt.ValidationGroup %>' />
                
                <asp:Button runat="server" ID="btnSaveReceipt" Text="Wystaw paragon" UseSubmitBehavior="true" 
                    OnClick="btnSaveReceipt_Click"  ValidationGroup="receipt" 
                    OnClientClick="return confirm('Wystawić paragon?');" />
                <asp:LinkButton runat="server" ID="btnCancel" onClick="btnCancel_Click" Text="Anuluj" />
                </asp:Panel> 
                </td> 
            </tr>
        </table>

        <uc:OrderStatusHistory runat="server" ID="ucOrderStatusHistory" />
        <asp:Panel runat="server" GroupingText="Płatności">
            <uc:OrderPayment runat="server" ID="ucOrderPayment" />
        </asp:Panel>
        <asp:Panel runat="server" GroupingText="Reklamacje">
            <uc:OrderComplaint runat="server" ID="ucOrderComplaint" />
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" GroupingText="Log zmian">
            <asp:UpdatePanel runat="server" ID="upOrderLog">
                <ContentTemplate>
            <asp:LinkButton runat="server" ID="lbtnOrderLog" Text="Pokaż historię zmian" CausesValidation="false" OnClick="lbtnOrderLog_Click"></asp:LinkButton>
            <div style="position: absolute;">
                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upOrderLog">
                    <ProgressTemplate>
                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div style="text-align: right;">
                <uc:OrderLog runat="server" ID="OrderLog" />
            </div></ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <div style="text-align: right;">
            <asp:Button runat="server" ID="btnOrderNew" Text="Nowe zamówienie" OnClick="btnOrderNew_Click"
                ValidationGroup="new" OnClientClick="return confirm('Czy chcesz utworzyć nowe zamówienie na podstawie bieżącego?');" /><br /><br />
              <asp:HyperLink ID="hlOrder" NavigateUrl="/Order.Cost.aspx?id={0}" Target="_blank" runat="server">Rachunek kosztów</asp:HyperLink></div>

    </div>
</asp:Content>
