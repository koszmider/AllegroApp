<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ProductCatalog.Attribute.ShopNames.aspx.cs" Inherits="LajtIt.Web.ProductAttributeShopNames" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Controls/AttributeMenu.ascx" TagName="AttributeMenu" TagPrefix="uc" %>
<%@ Register Src="~/Controls/ShopCategoryControlJson.ascx" TagName="ShopCategoryControl" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <uc:AttributeMenu runat="server" SetTab="td3" ID="ucAttributeMenu"></uc:AttributeMenu>
    
    <asp:Panel ID="pnContent" runat="server">
    
        </asp:Panel>
</asp:Content>
