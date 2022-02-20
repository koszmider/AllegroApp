<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ProductCatalog.Attribute.ShopCategories.aspx.cs" Inherits="LajtIt.Web.ProductAttributeShopCategories" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Controls/AttributeMenu.ascx" TagName="AttributeMenu" TagPrefix="uc" %>
<%@ Register Src="~/Controls/ShopCategoryControlJson.ascx" TagName="ShopCategoryControl" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <uc:AttributeMenu runat="server" SetTab="td4" ID="ucAttributeMenu"></uc:AttributeMenu>
    
    <asp:Panel ID="pnContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>


                        <asp:DropDownList runat="server" DataValueField="ShopTypeId" DataTextField="Name" ID="ddlShopType" AutoPostBack="true" Width="150"
                            OnSelectedIndexChanged="ddlShopType_SelectedIndexChanged">
                        </asp:DropDownList>


                    </td>
                </tr>
                <tr>
                    <td>
                        <uc:ShopCategoryControl runat="server" ID="ucShopCategoryControl"></uc:ShopCategoryControl>
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" ID="btnAttributeSave" OnClick="btnAttributeSave_Click" OnClientClick="return confirm('Zapisać zmiany?');" Text="Zapisz" /></td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlShopType" />
            <asp:PostBackTrigger ControlID="btnAttributeSave" />
        </Triggers>
    </asp:UpdatePanel>
        </asp:Panel>
</asp:Content>
