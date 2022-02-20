<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderShipping.ascx.cs"
    Inherits="LajtIt.Web.Controls.OrderShipping111" %>
 
<%@ Register Src="~/Controls/OrderShippingSimple.ascx" TagName="OrderShippingSimple" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script>
    function MainFunction() {

        $(document).ready(function () {
            $("#<%=txbServicePoint.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("/AutoComplete.asmx/GetPaczkomat") %>',
                        data: "{ 'searchText': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('-')[1],
                                    val: item.split('-')[0]
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
                    $("#<%=hfServicePoint.ClientID %>").val(i.item.val);
                },
                minLength: 1
            });
        });
    }
    function ClientValidate(source, arguments) {
<%--        alert($("#<%=txbServicePoint.ClientID %>").val());
        if ($("#ctl00_MainContent_ucOrderShipping_rbServiceModel_1").filter(":checked").val() == "2"
            && arguments.Value == "") {
            alert('blad');

            arguments.IsValid = false;
        }
        else {
            arguments.IsValid = false;
        }--%>

        arguments.IsValid = false;
    }

</script>
<asp:UpdatePanel runat="server" ID="UpdatePanel1">
    <Triggers>
        <asp:PostBackTrigger ControlID="imgbPrint" />
    </Triggers>
    <ContentTemplate>
        <asp:Label runat="server" ID="lblShippingMode"></asp:Label>
        <asp:Label runat="server" ID="lblShippingCompany"></asp:Label>
        <asp:Label runat="server" ID="lblServicePoint"></asp:Label><br />
        <asp:Label runat="server" ID="lblPayOnDelivery" Visible="false" Text="płatnośc przy odbiorze"></asp:Label>
        <asp:Label runat="server" ID="lblSendFromExternalWerehouse" Visible="false" Text="z mag. zew."></asp:Label>
        <asp:ImageButton runat="server" ID="imgbPrint" Visible="false" ImageUrl="~/Images/print.png" OnClick="imgbPrint_Click" Width="20" CausesValidation="false" />
        <asp:HyperLink runat="server" ID="hlTracking" Target="_blank"></asp:HyperLink>
        <br />
    </ContentTemplate>
</asp:UpdatePanel><br />

<asp:LinkButton runat="server" ID="lbtnOrderShippings" Text="zmień/dodaj metodę dostawy"></asp:LinkButton>
<asp:ModalPopupExtender ID="mpeAttributesEdit" runat="server"
    TargetControlID="lbtnOrderShippings"
    PopupControlID="pnAttributes2"
    BackgroundCssClass="modalBackground"
    DropShadow="true"
    CancelControlID="imbCancel1111"
    PopupDragHandleControlID="Panel2" />

<asp:Panel runat="server" ID="pnAttributes2" BackColor="White" Style="width: 1200px; background-color: white; overflow:scroll; max-height: 600px; padding: 10px">
    <asp:Panel runat="server" ID="Panel2">
        <div style="text-align: right">
            <asp:ImageButton runat="server" ImageUrl="~/Images/cancel.png" Width="20" ID="imbCancel1111" />
        </div>

    </asp:Panel>
    <div style="width: 1200px; height: 500px">
        <table>
            <tr valign="top">
                <td>
                    <asp:UpdatePanel runat="server" ID="upM">
                        <ContentTemplate>

                            <asp:Panel runat="server" ID="pnOrderShipping" GroupingText="Kreator przesyłki" Visible="false">
                                <table>
                                    <tr>
                                        <td>Status przesyłki</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlOrderShippingStatus" DataTextField="Name" DataValueField="OrderShippingStatusId" ValidationGroup="os">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ValidationGroup="os" runat="server" ControlToValidate="ddlOrderShippingStatus"
                                                ValueToCompare="2" Operator="NotEqual" Text="nie możesz ustawić tego statusu" Display="Dynamic"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Rodzaj przesyłki</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlShippingServiceType" ValidationGroup="os">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Value="1">Przesyłka do zamówienia</asp:ListItem>
                                                <asp:ListItem Value="2">Przesyłka reklamacyjna</asp:ListItem>
                                            </asp:DropDownList><asp:RequiredFieldValidator runat="server" ID="rfvShippingServiceType"
                                                ControlToValidate="ddlShippingServiceType" ValidationGroup="os" Text="wymagane"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Typ usługi</td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList runat="server" ID="rbServiceModel" ValidationGroup="os" RepeatDirection="Horizontal">

                                                            <asp:ListItem Value="1">kurier</asp:ListItem>
                                                            <asp:ListItem Value="2">dostawa do punktu</asp:ListItem>
                                                            <asp:ListItem Value="3">odbiór w salonie</asp:ListItem>
                                                        </asp:RadioButtonList></td>
                                                    <td>
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="rbServiceModel" ValidationGroup="os" Text="wymagane"></asp:RequiredFieldValidator></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Przewoźnik</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlCourier" DataValueField="ShippingCompanyId" DataTextField="Name" AppendDataBoundItems="true">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList><asp:RequiredFieldValidator runat="server" ID="rfvShippingCompany" ControlToValidate="ddlCourier" ValidationGroup="os" Text="wymagane"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Punkt odbioru</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txbServicePoint"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hfServicePoint" />
                                            <asp:CustomValidator ID="CustomValidator1"
                                                ControlToValidate="txbServicePoint"
                                                ClientValidationFunction="ClientValidate"
                                                Display="Static"
                                                Text="Not an even number!"
                                                ValidationGroup="os"
                                                ForeColor="green"
                                                Font-Name="verdana"
                                                Font-Size="10pt"
                                                Visible="false"
                                                runat="server" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox runat="server" ID="chbCOD" Text="Płatność przy odbiorze" /></td>
                                    </tr>
                                    <tr>
                                        <td>Paczki</td>
                                        <td>
                                            <asp:GridView runat="server" ID="gvOrderShippingParcel" DataKeyNames="Id" ShowHeaderWhenEmpty="true"
                                                AutoGenerateColumns="false" OnRowDataBound="gvOrderShippingParcel_RowDataBound">
                                                <Columns>

                                                    <asp:TemplateField>
                                                        <HeaderStyle Width="28" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="ibtnDelete" ImageUrl="~/Images/cancel.png" Width="20"
                                                                CausesValidation="false"
                                                                OnClientClick="return confirm('Usunąć');" OnClick="ibtnDelete_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Gabaryt">
                                                        <HeaderStyle Width="150" />
                                                        <ItemTemplate>
                                                            <asp:DropDownList runat="server" ID="ddlSize" ValidationGroup="os">
                                                                <asp:ListItem Value="0">-- nie wybrano --</asp:ListItem>
                                                                <asp:ListItem Value="1">A</asp:ListItem>
                                                                <asp:ListItem Value="2">B</asp:ListItem>
                                                                <asp:ListItem Value="3">C</asp:ListItem>
                                                            </asp:DropDownList><br />
                                                            <asp:CompareValidator ValidationGroup="os" runat="server" ControlToValidate="ddlSize"
                                                                ValueToCompare="0" Operator="NotEqual" Text="wymagane" Display="Dynamic"></asp:CompareValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Waga">
                                                        <HeaderStyle Width="150" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txbWeight" Width="100" MaxLength="50"></asp:TextBox><br />
                                                            <asp:RequiredFieldValidator
                                                                ValidationGroup="os" runat="server" ControlToValidate="txbWeight"
                                                                Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Wysokość">
                                                        <HeaderStyle Width="150" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txbHeight" Width="100" MaxLength="50"></asp:TextBox><br />
                                                            <asp:RequiredFieldValidator
                                                                ValidationGroup="os" runat="server" ControlToValidate="txbHeight"
                                                                Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Szerokość">
                                                        <HeaderStyle Width="150" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txbWidth" Width="100" MaxLength="50"></asp:TextBox><br />
                                                            <asp:RequiredFieldValidator
                                                                ValidationGroup="os" runat="server" ControlToValidate="txbWidth"
                                                                Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Długość">
                                                        <HeaderStyle Width="150" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txbLength" Width="100" MaxLength="50"></asp:TextBox><br />
                                                            <asp:RequiredFieldValidator
                                                                ValidationGroup="os" runat="server" ControlToValidate="txbLength"
                                                                Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <table style="border: solid 1px black">
                                                <tr>
                                                    <th style="width: 20px;"></th>
                                                    <th style="width: 150px;"></th>
                                                    <th style="width: 150px;"></th>
                                                    <th style="width: 150px;"></th>
                                                    <th style="width: 150px;"></th>
                                                    <th style="width: 150px;"></th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton runat="server" ID="ibtnAdd"
                                                            OnClientClick="if((Page_ClientValidate('os') && Page_ClientValidate('os1')) == true) return confirm('Dodać nową paczkę do przesyłki?');" ImageUrl="~/Images/add.png" Width="20" OnClick="lbtnAddParcel_Click" /></td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlSize" ValidationGroup="os1">
                                                            <asp:ListItem Value="0">-- nie wybrano --</asp:ListItem>
                                                            <asp:ListItem Value="1">A</asp:ListItem>
                                                            <asp:ListItem Value="2">B</asp:ListItem>
                                                            <asp:ListItem Value="3">C</asp:ListItem>
                                                        </asp:DropDownList><br />
                                                        <asp:CompareValidator ValidationGroup="os1" runat="server" ControlToValidate="ddlSize"
                                                            ValueToCompare="0" Operator="NotEqual" Text="wymagane" Display="Dynamic"></asp:CompareValidator>

                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txbWeight" MaxLength="3" TextMode="Number" Width="100" ValidationGroup="os1"></asp:TextBox><br />
                                                        <asp:RequiredFieldValidator
                                                            ValidationGroup="os1" runat="server" ControlToValidate="txbWeight"
                                                            Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txbHeight" MaxLength="3" TextMode="Number" Width="100" ValidationGroup="os1"></asp:TextBox><br />
                                                        <asp:RequiredFieldValidator
                                                            ValidationGroup="os1" runat="server" ControlToValidate="txbHeight"
                                                            Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txbWidth" MaxLength="3" TextMode="Number" Width="100" ValidationGroup="os1"></asp:TextBox><br />
                                                        <asp:RequiredFieldValidator
                                                            ValidationGroup="os1" runat="server" ControlToValidate="txbWidth"
                                                            Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txbLength" MaxLength="3" TextMode="Number" Width="100" ValidationGroup="os1"></asp:TextBox><br />
                                                        <asp:RequiredFieldValidator
                                                            ValidationGroup="os1" runat="server" ControlToValidate="txbLength"
                                                            Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox runat="server" ID="chbSendExternal" Text="Wysyłka z magazynu zewnętrznego" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button runat="server" ID="btnSaveShipping"
                                                ValidationGroup="os"
                                                Text="Zapisz" OnClientClick="alert('Nie dodano żadnej paczki do przesyłki'); return false;" OnClick="btnSaveShipping_Click" />

                                            <asp:LinkButton runat="server" ID="lbtnCancel" Text="Anuluj" OnClick="lbtnCancel_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:LinkButton runat="server" ID="lbtnOrderShippingNew" Text="dodaj nową przesyłkę" OnClick="lbtnOrderShippingNew_Click"></asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>

                    <asp:GridView runat="server" ID="gvOrderShippings" AutoGenerateColumns="false" OnRowDataBound="gvOrderShippings_RowDataBound" CellPadding="5">
                        <Columns>
                            <asp:TemplateField HeaderText="Data utworzenia">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lbtnOrderShipping" Text="{0:yy/MM/dd HH:mm}" OnClick="lbtnOrderShipping_Click"></asp:LinkButton>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:BoundField DataField="ShippingCompany" HeaderText="Kurier" />
                            <asp:BoundField DataField="ShippingServiceType" HeaderText="Rodzaj przesyłki" />
                            <asp:BoundField DataField="ParcelCount" HeaderText="Liczba paczek" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Numer śledzenia">
                                <ItemTemplate>
                                    <asp:UpdatePanel runat="server" ID="up">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="imgbPrint1" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:ImageButton runat="server" ID="imgbPrint1" ImageUrl="~/Images/print.png" OnClick="imgbPrint_Click" Width="20" CausesValidation="false" />
                                            <asp:HyperLink runat="server" ID="hlTracking1" Target="_blank"></asp:HyperLink>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="OrderShippingStatus" HeaderText="Status przesyłki" />
                            <asp:BoundField DataField="COD" HeaderText="COD" DataFormatString="{0:C}" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
