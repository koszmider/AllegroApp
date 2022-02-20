<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CostControl.ascx.cs" Inherits="LajtIt.Web.Controls.CostControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table style="width: 100%">
    <tr>
        <td colspan="4">
            <table>
                <tr>
                    <td>Faktura dla:</td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblCompanyFor" ValidationGroup="add" RepeatDirection="Horizontal">
                            <asp:ListItem Value="0" Selected="True">nie wybrano</asp:ListItem>
                            <asp:ListItem Value="1">Oświetlenie Lajtit</asp:ListItem>
                            <asp:ListItem Value="78">M-Form sp. z o.o.</asp:ListItem>
                        </asp:RadioButtonList></td>
                    <td>
                        <asp:CompareValidator runat="server" ID="cfv" ControlToValidate="rblCompanyFor" ValueToCompare="0" ValidationGroup="add"
                            Operator="GreaterThan" Type="Integer"
                            ErrorMessage="Wybierz odbiorcę faktury"></asp:CompareValidator></td>
                </tr>
            </table>




        </td>
    </tr>
    <tr>

        <td colspan="4">
            <asp:RadioButtonList runat="server" ID="rblCostDocumentType" ValidationGroup="add"
                AutoPostBack="true" OnSelectedIndexChanged="rblCostDocumentType_SelectedIndexChanged"
                RepeatDirection="Horizontal">
                <asp:ListItem Value="1" Selected="True">Faktura</asp:ListItem>
                <asp:ListItem Value="2">Korekta</asp:ListItem>
            </asp:RadioButtonList>

        </td>
    </tr>
    <tr>
        <td>Firma</td>
        <td>
            <div>
                <script type="text/javascript">

                    function goAutoCompl() {

                        $("#<%=txbCompany.ClientID %>").autocomplete({
                            source: function (request, response) {
                                $.ajax({
                                    url: '<%=ResolveUrl("~/AutoComplete.asmx/GetCompanies") %>',
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
                                $("#<%=hfCompanyId.ClientID %>").val(i.item.val);
                            },
                            minLength: 3
                        });

                    }

                    function goAutoComplInv() {



                        $("#<%=txbInvoiceCorrectionNumber.ClientID %>").autocomplete({
                            source: function (request, response) {
                                $.ajax({
                                    url: '<%=ResolveUrl("~/AutoComplete.asmx/GetInvoiceByCompanyId") %>',
                                    data: "{ 'query': '" + $('#<%=hfCompanyId.ClientID %>').val() + "|" + request.term + "'}",
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
                                $("#<%=hfInvoiceCorrection.ClientID %>").val(i.item.val);
                            },
                            minLength: 3
                        });

                    }
                    $(document).ready(function () {
                        goAutoCompl();
                        goAutoComplInv();
                    });
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                    function EndRequestHandler(sender, args) {
                        goAutoCompl();
                        goAutoComplInv();
                    }

                </script>
                <asp:UpdatePanel runat="server" ID="upNewProduct">
                    <ContentTemplate>

                        <asp:TextBox runat="server" ID="txbCompany" MaxLength="50" Width="250" ValidationGroup="add"></asp:TextBox>

                        <asp:HiddenField ID="hfCompanyId" runat="server" />


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

        </td>
        <td width="150">Numer faktury</td>
        <td>
            <asp:TextBox runat="server" ID="txbInvoiceNumber" MaxLength="100" ValidationGroup="add"></asp:TextBox>

        </td>
    </tr>
    <tr>
        <td><a href="Company.aspx" target="_blank">dodaj nową firmę</a></td>
        <td>
            <asp:RequiredFieldValidator runat="server" ValidationGroup="add" Text="wybierz" ControlToValidate="txbCompany"></asp:RequiredFieldValidator></td>
        <td></td>
        <td>
            <asp:RequiredFieldValidator runat="server" ID="rfvInvoiceNumber" ControlToValidate="txbInvoiceNumber" ValidationGroup="add" Text="podaj numer faktury" Enabled="false"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>

        <td>Rodzaj kosztu</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCostType" DataValueField="CostTypeId" DataTextField="Name" /></td>
        <td>Data płatności</td>
        <td>
            <asp:TextBox runat="server" ID="txbPaidDate" MaxLength="10" ValidationGroup="add" Style="text-align: center;"
                Width="70"></asp:TextBox><asp:RegularExpressionValidator runat="server" ControlToValidate="txbPaidDate" Enabled="false"
                    ID="RegularExpressionValidator4" ValidationExpression="201[\d]{1}-[\d]{2}-[\d]{2}"
                    ValidationGroup="add" Text="*" /><asp:CalendarExtender
                        ID="calPaidDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txbPaidDate"></asp:CalendarExtender>
        </td>
    </tr>
    <tr>
        <td>Data wystawienia</td>
        <td>
            <asp:TextBox runat="server" ID="txbDate" MaxLength="10" ValidationGroup="add" Style="text-align: center;"
                Width="70"></asp:TextBox><asp:RegularExpressionValidator runat="server" ControlToValidate="txbDate" Enabled="false"
                    ID="RegularExpressionValidator3" ValidationExpression="201[\d]{1}-[\d]{2}-[\d]{2}"
                    ValidationGroup="add" Text="*" /><asp:CalendarExtender
                        ID="calDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDate"></asp:CalendarExtender>
        </td>
        <td>VAT</td>
        <td>
            <asp:DropDownList ID="ddlVAT" runat="server">
                <asp:ListItem>23</asp:ListItem>
                <asp:ListItem>0</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td>Kwota</td>
        <td>
            <asp:TextBox runat="server" ID="txbAmount" MaxLength="10" ValidationGroup="add" Width="60" onchange="strip(this);"
                Style="text-align: right;"></asp:TextBox>
            <asp:RegularExpressionValidator ControlToValidate="txbAmount" Display="Dynamic"
                runat="server" ID="rfvAmout" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                ValidationGroup="add" Text="oczekiwana wartość dodatnia" />
            <asp:RequiredFieldValidator runat="server" ValidationGroup="add" Text="*" ControlToValidate="txbAmount"></asp:RequiredFieldValidator></td>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="chbToPay" Text="Dodaj do paczki przelewów" /></td>
    </tr>
    <tr>
        <td>Uwagi</td>
        <td>
            <asp:TextBox runat="server" ID="txbComment" MaxLength="250" ValidationGroup="add"></asp:TextBox></td>
        <td>
            <asp:Label runat="server" ID="lblInvoiceCorrection" Text="Korekta faktury" Enabled="false"></asp:Label></td>
        <td>
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <asp:TextBox runat="server" ID="txbInvoiceCorrectionNumber" Enabled="false" MaxLength="50" Width="150" ValidationGroup="add"></asp:TextBox>
                    <asp:HiddenField ID="hfInvoiceCorrection" runat="server" />
                    <span style="position: absolute; width: 10px;">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upNewProduct">
                            <ProgressTemplate>
                                <img src="Images/progress.gif" style="height: 20px" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </span>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>Do zamówienia:</td>
        <td><asp:TextBox runat="server" ID="txbOrder" MaxLength="250" ValidationGroup="add"></asp:TextBox>
            <asp:RegularExpressionValidator ControlToValidate="txbOrder" Display="Dynamic"
                runat="server" ID="RegularExpressionValidator1" ValidationExpression="\d{1,6}"
                ValidationGroup="add" Text="oczekiwano liczby" /></td>
        <td>
            <asp:CheckBox runat="server" ID="chbInvoiceCorrectionPaid" Enabled="false" Text="Zapłacona" />
        </td>
        <td>
            <asp:RequiredFieldValidator runat="server" ID="rfvInvoiceCorrectionNumber"
                ControlToValidate="txbInvoiceCorrectionNumber" ValidationGroup="add" Text="podaj numer faktury"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td colspan="4">

            <asp:Button runat="server" ValidationGroup="add"
                Text="Zapisz" ID="btnCostAdd" OnClick="btnCostAdd_Click" />
            <asp:LinkButton runat="server" CausesValidation="false" Text="Anuluj" ID="lbtnCostCancel"
                OnClick="lbtnCostCancel_Click" />
        </td>
    </tr>
</table>

