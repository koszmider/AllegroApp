<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CostsControl.ascx.cs"
    Inherits="LajtIt.Web.Controls.CostsControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/CostControl.ascx" TagName="CostControl" TagPrefix="uc" %>

<asp:Label runat="server" ID="lblOK"></asp:Label>
<div style="text-align: right;">
    <asp:LinkButton runat="server" ID="lbtnCostNew" CausesValidation="false" Text="Dodaj nowy"
        OnClick="lbtnCostNew_Click"></asp:LinkButton>
</div>


<asp:ModalPopupExtender ID="mpeCostReceipt" runat="server"
    TargetControlID="lblOK"
    PopupControlID="pnShopProducts"
    BackgroundCssClass="modalBackground"
    DropShadow="true"
    CancelControlID="imbCancel"
    PopupDragHandleControlID="Panel1" />

<asp:Panel runat="server" ID="pnShopProducts" GroupingText="Koszt" BackColor="White" 
    Style="width: 900px; background-color: white; height: 450px; padding: 10px">
    <div style="text-align:right; "><asp:ImageButton runat="server" ID="imbCancel" ImageUrl="~/Images/cancel.png" Width="20" /></div>
    <uc:CostControl ID="ucCostControl" runat="server"></uc:CostControl> 

</asp:Panel>


<asp:GridView runat="server" ID="gvCosts" DataKeyNames="CostId" AutoGenerateColumns="false"
    ShowFooter="true" AllowPaging="true" PageSize="200" OnPageIndexChanging="gvCosts_OnPageIndexChanging"
    OnRowEditing="gvCosts_OnRowEditing" OnRowCancelingEdit="gvCosts_OnRowCancelingEdit"
    OnRowUpdating="gvCosts_OnRowUpdating" OnRowDataBound="gvCost_OnRowDataBound">
    <Columns>
        <asp:TemplateField HeaderText="Lp" ItemStyle-Width="20px">
            <HeaderTemplate>
                <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="chbOrder" />
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField HeaderText="Edytuj" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">

            <ItemTemplate>
                <asp:Button runat="server" ID="btnEdit" Text="edytuj" OnClick="btnEdit_Click" />
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField HeaderText="Faktura dla">
            <ItemStyle Width="200" />
            <ItemTemplate>
                <asp:Label runat="server" ID="lblCompanyOwner" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlCompanyOwner" DataValueField="CompanyId" DataTextField="Name"
                    Width="200" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Typ kosztu">
            <ItemStyle Width="200" />
            <ItemTemplate>
                <asp:Label runat="server" ID="lblCostType" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlCostType" DataValueField="CostTypeId" DataTextField="Name"
                    Width="200" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Faktura od">
            <ItemStyle Width="200" />
            <ItemTemplate>
                <asp:HyperLink NavigateUrl="/Company.aspx?id={0}" ID="hlCompany" Target="_blank" runat="server"></asp:HyperLink>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlCompany" DataValueField="CompanyId" DataTextField="Name"
                    Width="200" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:CheckBoxField DataField="IsForAccounting" HeaderText="Ks" ReadOnly="true" Visible="false" />
        <asp:TemplateField HeaderText="Data wystawienia">
            <ItemStyle Width="70" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Label runat="server" ID="lblDate" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txbDate" MaxLength="10" Width="70" ValidationGroup="ed"></asp:TextBox><asp:RegularExpressionValidator
                    runat="server" ControlToValidate="txbDate" ID="reg1" ValidationExpression="201[\d]{1}-[\d]{2}-[\d]{2}"
                    ValidationGroup="ed" Text="*" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Data płatności">
            <ItemStyle Width="70" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Label runat="server" ID="lblPaidDate" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txbPaidDate" MaxLength="10" Width="70" ValidationGroup="ed"></asp:TextBox><asp:RegularExpressionValidator
                    runat="server" ControlToValidate="txbPaidDate" ID="reg222" ValidationExpression="201[\d]{1}-[\d]{2}-[\d]{2}"
                    ValidationGroup="ed" Text="*" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="VAT">
            <ItemStyle Width="70" HorizontalAlign="Right" />
            <ItemTemplate>
                <asp:Label runat="server" ID="lblVat" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txbVAT" MaxLength="10" ValidationGroup="ed" Width="50"></asp:TextBox><asp:RegularExpressionValidator
                    ControlToValidate="txbVAT" runat="server" ID="reg2" ValidationExpression="[0-9]\{1,2}"
                    ValidationGroup="ed" Text="*" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="VAT" Visible="false">
            <ItemStyle Width="70" HorizontalAlign="Right" />
            <ItemTemplate>
                <asp:Label runat="server" ID="lblVATValue" />
            </ItemTemplate>
            <FooterStyle HorizontalAlign="Right" />
            <FooterTemplate>
                <asp:Label runat="server" ID="lblVATValue" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Kwota netto" Visible="false">
            <ItemStyle Width="100" HorizontalAlign="Right" />
            <ItemTemplate>
                <asp:Label runat="server" ID="lblAmountNetto" />
            </ItemTemplate>
            <FooterStyle HorizontalAlign="Right" />
            <FooterTemplate>
                <asp:Label runat="server" ID="lblAmountNetto" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Kwota brutto">
            <FooterStyle HorizontalAlign="Right" />
            <ItemStyle Width="100" HorizontalAlign="Right" />
            <ItemTemplate>
                <asp:Label runat="server" ID="lblAmount" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txbAmount" MaxLength="10" ValidationGroup="ed" Width="50"></asp:TextBox><asp:RegularExpressionValidator
                    ControlToValidate="txbAmount" runat="server" ID="reg3" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                    ValidationGroup="ed" Text="*" />
            </EditItemTemplate>
            <FooterTemplate>
                <asp:Label runat="server" ID="lblAmountBrutto" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Numer faktury">
            <ItemStyle Width="120" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Label runat="server" ID="lblInvoiceNumber" Width="120" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txbInvoiceNumber" MaxLength="100" Width="120" ValidationGroup="ed"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Uwagi">
            <ItemStyle Width="200" />
            <ItemTemplate>
                <asp:Label runat="server" ID="lblComment" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txbComment" MaxLength="250" ValidationGroup="ed"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" ValidationGroup="ed" Text="*" ControlToValidate="txbComment"></asp:RequiredFieldValidator>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Przelew">
            <ItemStyle Width="70" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Image runat="server" ID="imgBatch" ImageUrl="~/Images/calculator.png" Width="20" ToolTip="Paczka przelewów" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:RadioButton runat="server" ID="rbToPay1" GroupName="topay" Text="T" />
                <asp:RadioButton runat="server" ID="rbToPay0" GroupName="topay" Text="N" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemStyle Width="70" />
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="chbIsChecked" />
                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Zmienić status?');" CommandArgument='<%# Eval("CostId") %>' Text="Zmień" OnClick="lbtnChecked_Click" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
