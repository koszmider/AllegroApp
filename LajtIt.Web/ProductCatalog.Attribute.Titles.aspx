<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ProductCatalog.Attribute.Titles.aspx.cs" Inherits="LajtIt.Web.ProductAttributeTitles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Controls/AttributeMenu.ascx" TagName="AttributeMenu" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:AttributeMenu runat="server" ID="ucAttributeMenu" SetTab="td2"></uc:AttributeMenu>
    <asp:Panel ID="pnContent" runat="server">
        <table>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvShop" AutoGenerateColumns="false" ShowHeader="false" OnRowDataBound="gvShop_RowDataBound" DataKeyNames="ShopId">

                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle CssClass="td123" />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblShopName"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle CssClass="td123" />
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txbTemplate" Width="680" ValidationGroup="temp"></asp:TextBox>
                                    <asp:RegularExpressionValidator runat="server" ID="regV" ControlToValidate="txbTemplate" Visible="false" ValidationGroup="temp" ValidationExpression="^(\(?\[.*?\](\|\[.*?\]*)\)?)+" Text="zły format"></asp:RegularExpressionValidator>
                                    <asp:TextBoxWatermarkExtender ID="tbwWater" runat="server"
                                        TargetControlID="txbTemplate"
                                        WatermarkText="[SUPPLIER] [LAMP_TYPE] [LINE]  [COLOR]  [CODE]"
                                        WatermarkCssClass="watermarked" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                    stosuj nawias () by wyświetlać jeden ze wskazanych atrybutów np:<br />
                    [LAMPTYPE]<b><span style="color: red;">(</span>[KOLOR][ZASTOSOW]<span style="color: red;">)</span></b>[EAN]<b><span style="color: red;">(</span>[CODE][CODE2]<span style="color: red;">)</span></b>[LINIA]
                </td>
            </tr>


            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnAttributeSave" OnClick="btnAttributeSave_Click" OnClientClick="if(Page_ClientValidate('temp')) return confirm('Zapisać zmiany?');" Text="Zapisz" />
                    <asp:LinkButton runat="server" ID="lbtnCreateNames" OnClick="lbtnCreateNames_Click" OnClientClick="if(Page_ClientValidate('temp')) return confirm('Czy utworzyć nazwy produktów?');" Text="Twórz nazwy" /></td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
