<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderPayment.ascx.cs"
    Inherits="LajtIt.Web.Controls.OrderPayment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<div style="text-align: right;">
    <asp:LinkButton runat="server" ID="lbtnPaymentAdd" OnClick="lbtnPaymentAdd_Click"
        Text="Dodaj płatność" />
</div>
<asp:Panel ID="pPayments" runat="server" GroupingText="Płatność" Visible="false">
    <div style="text-align: right; position: relative; height: 220px;">
        <div style="width: 405px; position: absolute; right: 0px;">
            <table style="text-align: left; width: 400px;">
                <tr>
                    <td>Typ płatności:<br />
                        <asp:DropDownList runat="server" ID="ddlCompany" Visible="false" Enabled="false" DataValueField="CompanyId" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_OnSelectedIndexChanged"></asp:DropDownList>
                        <asp:DropDownList runat="server" ID="ddlOrderPaymentTypes" DataTextField="Name" Width="200" DataValueField="PaymentTypeId"
                            ValidationGroup="payment">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvPaymentType" Text="wybierz" ControlToValidate="ddlOrderPaymentTypes"
                            ValidationGroup="payment" />
                    </td>
                    <td>Data:<br />
                        <asp:TextBox runat="server" ID="txbDateFrom" Width="80"></asp:TextBox><asp:CalendarExtender FirstDayOfWeek="Monday"
                            ID="calDateFrom" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateFrom">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>Ext ID:<br />
                        <asp:TextBox runat="server" Text="0" MaxLength="15" Width="80" Style="text-align: right;"
                            ID="txbExternalId" />
                        <asp:RequiredFieldValidator runat="server" ID="tfvAmount" Text="*" ControlToValidate="txbExternalId"
                            ValidationGroup="payment" /></td>
                    <td>Kwota:<br />
                        <asp:TextBox runat="server" Text="" MaxLength="10" Width="80" Style="text-align: right;" onchange="strip(this);"
                            ID="txbAmount" />
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" Text="*"
                            ControlToValidate="txbAmount" ValidationGroup="payment" /><asp:RegularExpressionValidator
                                runat="server" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ControlToValidate="txbAmount"
                                ValidationGroup="payment" Text="*" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="chbUpdateVAT" Text="Aktualizuj VAT dla produktów" />
                    </td>
                </tr>
            </table>
            <asp:Button runat="server" ID="btnProductSave" Text="Dodaj" OnClick="btnProductSave_Click" OnClientClick="return confirm('Zapisać dane? Czy data przelewu jest poprawna?');"
                ValidationGroup="payment" />&nbsp;<asp:LinkButton runat="server" ID="lnbPaymentCancel"
                    Text="Anuluj" OnClick="lnbPaymentCancel_Click" CausesValidation="false" />
        </div>
    </div>
</asp:Panel>
<asp:GridView runat="server" ID="gvOrderPayments" AutoGenerateColumns="false" OnRowDataBound="gvOrderPayments_OnRowDataBound" OnRowEditing="gvOrderPayments_OnRowEditing" OnRowCancelingEdit="gvOrderPayments_OnRowCancelingEdit" DataKeyNames="PaymentId"
  Width="100%"   OnRowUpdating="gvOrderPayments_OnRowUpdating">

    <Columns>
        <asp:CommandField ItemStyle-Width="65" ShowCancelButton="true" ShowEditButton="true"
            ButtonType="Image" EditImageUrl="~/Images/edit.jpg" UpdateImageUrl="~/Images/save.jpg"
            CancelImageUrl="~/Images/cancel.jpg" />
        <asp:BoundField DataField="OrderPaymentType.Name" HeaderText="Rodzaj płatności" ReadOnly="true" ItemStyle-Width="120" />
        <asp:BoundField DataField="InsertDate" HeaderText="Data" DataFormatString="{0:yyyy-MM-dd HH:mm}" ApplyFormatInEditMode="true"  ItemStyle-Width="200"/>
        <asp:BoundField DataField="InsertUser" HeaderText="Dodał" ReadOnly="true"  ItemStyle-Width="80"/>
        <asp:BoundField DataField="Amount" DataFormatString="{0:C}" HeaderText="Kwota" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="200"/>
        <asp:BoundField DataField="Comment" HeaderText="Uwagi"/> 
        <asp:TemplateField HeaderText="Rozliczono" ItemStyle-Width="150" >
            <EditItemTemplate>
                
               <asp:DropDownList runat="server" ID="ddlAccoutingType" AppendDataBoundItems="true" DataValueField="AccountingTypeId" DataTextField="Name">
                   <asp:ListItem></asp:ListItem>
               </asp:DropDownList>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label runat="server" ID="lblAccoutingType"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="PaymentId" HeaderText="ID" ReadOnly="true" />
        <asp:TemplateField HeaderText="Akcja" ItemStyle-Width="100" >
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbtnMoveOut" OnClick="lbtnMoveOut_Click" Text="Wycofaj wpłatę" OnClientClick="return confirm('Czy chcesz wycofać wpłatę? Nastąpi wyzerowanie wartości i wyksięgowanie z kasy');"></asp:LinkButton>
       
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

