<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shop.Suppliers.aspx.cs"
    Inherits="LajtIt.Web.ShopSuppliers" %>
<%@ Register Src="~/Controls/ShopMenu.ascx" TagName="ShopMenu" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/ShopCategoryControlJson.ascx" TagName="ShopCategoryControlJson" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc:ShopMenu runat="server" SetTab="td1"></uc:ShopMenu>
 
    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" id="rblOnlyActive" OnSelectedIndexChanged="chbOnlyActive_CheckedChanged" AutoPostBack="true">
        <asp:ListItem Selected="false">wszystkie</asp:ListItem>
        <asp:ListItem Selected="True">aktywne</asp:ListItem>
        <asp:ListItem Selected="false">nieaktywne</asp:ListItem>

    </asp:RadioButtonList> <br /> 
    
    <asp:GridView runat="server" ID="gvSuppliers" DataKeyNames="Id" AutoGenerateColumns="false"
        OnRowDataBound="gvSuppliers_RowDataBound">
        <Columns>
             <asp:TemplateField HeaderText="Dostawca">
                <ItemStyle Width="20" HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlSupplier" NavigateUrl="/Supplier.aspx?id={0}" Target="_blank"></asp:HyperLink>
                </ItemTemplate>            </asp:TemplateField>
            <asp:TemplateField HeaderText="Aktywny">
                <ItemStyle Width="20" HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="chbIsActive" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nazwa w opisie">
                <ItemStyle Width="20" HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="chbShowSupplierNameIdDescription" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Min. cena">
                <ItemStyle Width="70" />
                <ItemTemplate>
                    <asp:TextBox runat="server" style="text-align:right" ID="txbMinPrice" MaxLength="10" Width="70"></asp:TextBox><asp:RegularExpressionValidator
                     Display="Dynamic"            runat="server" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ControlToValidate="txbMinPrice"
                                Text="zły format" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Max.l. prod.">
                <ItemStyle Width="70" />
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txbMaxNumberOfProductsInOffer" MaxLength="3" Width="70"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Kategoria domyślna">
                <ItemStyle Width="70" HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Button runat="server" ID="btnCategory" OnClick="btnCategory_Click" />
                </ItemTemplate>
            </asp:TemplateField>
     
            <asp:TemplateField HeaderText="Szablon">
                <ItemStyle Width="400" />
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txbTemplate" MaxLength="256" Width="400"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Blokada rabatów">
                <ItemStyle Width="20" HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="chbLockRebates" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Do ceny regularnej +/-%">
                <ItemStyle Width="70" />
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txbSellDiscount" MaxLength="6" Width="50" Style="text-align: center"></asp:TextBox>%
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Do ceny regularnej +/-zł">
                <ItemStyle Width="70" />
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txbSellDiscountValue" MaxLength="6" Width="50" Style="text-align: center"></asp:TextBox>zł
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Do ceny promocyjnej +/-%">
                <ItemStyle Width="70" />
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txbSellCommision" MaxLength="6" Width="50" Style="text-align: center"></asp:TextBox>%
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br /><br />
    <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" OnClientClick="if(Page_ClientValidate()) return confirm('Zapisać zmiany?');" Text="Zapisz" />

     <asp:Label runat="server" ID="lblOK"></asp:Label>
    <asp:Label runat="server" ID="Label1"></asp:Label>


    <asp:ModalPopupExtender ID="mpeCategory" runat="server"
        TargetControlID="lblOK"
        PopupControlID="pnCategories"
        BackgroundCssClass="modalBackground"
        DropShadow="true"
        CancelControlID="btnCancel"
        PopupDragHandleControlID="Panel1" />

    <asp:Panel runat="server" ID="pnCategories" BackColor="White" Style="width: 400px; background-color: white; height: 400px; padding: 10px">
        <asp:Panel runat="server" ID="Panel1">
            Wybór kategorii w sklepie
        </asp:Panel>
        <div style="width: 400px; height: 350px">   
            <uc:ShopCategoryControlJson runat="server" ID="ucShopCategoryControlJson" />


        </div>
        <asp:Button runat="server" ID="btnOK" Text="Zapisz" OnClientClick="return confirm ('Zapisać zmiany?');" OnClick="btnOK_Click" CausesValidation="false" />
        <asp:LinkButton runat="server" ID="btnCancel" Text="Anuluj" />
    </asp:Panel>
</asp:Content>
