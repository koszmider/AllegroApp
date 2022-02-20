<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"   ValidateRequest="false"
    CodeBehind="Supplier.Descriptions.aspx.cs" Inherits="LajtIt.Web.SupplierDescriptions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/SupplierMenu.ascx" TagName="SupplierMenu" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc:SupplierMenu runat="server" SetTab="td3"></uc:SupplierMenu>

    <asp:DropDownList runat="server" ID="ddlShops" DataValueField="ShopId" DataTextField="Name"></asp:DropDownList>
    <asp:Button runat="server" ID="btnShopShow" CausesValidation="false" OnClick="btnShopShow_Click" Text="Pokaż" />

    <asp:Panel runat="server" ID="pnDescription">
        <asp:CheckBox runat="server" ID="chbIsActive" Text="Aktywny" /><br /><br /><br />
        <asp:Panel runat="server" ID="pnTags">Dozwolone tagi allegro:  &lt;h1>, &lt;h2>, &lt;p>, &lt;ul>, &lt;ol>, &lt;li> oraz &lt;b> oraz ich domknięcia.
          <asp:RegularExpressionValidator runat="server" ID="revTags"
              ValidationExpression="^([^<]|<h1>|<h2>|<p>|<ul>|<ol>|<li>|<b>|</h1>|</h2>|</p>|</ul>|</ol>|</li>|</b>|a z|A Z|1 9|(.\.))*$" Text="Niewłaściwe tagi html"
                                  

                                ControlToValidate="txtLongDescription" ErrorMessage="Niewłaściwe tagi html"></asp:RegularExpressionValidator>

            </asp:Panel> 
        <asp:TextBox
            ID="txtLongDescription"
            TextMode="MultiLine"
            Columns="180"
            Rows="30"
            runat="server" />
        <asp:HtmlEditorExtender EnableSanitization="false"
            ID="htmlLong"
             Enabled="false"
            
            DisplayPreviewTab="true"
            DisplaySourceTab="true"
            TargetControlID="txtLongDescription"
            runat="server" />
        <br />
        <asp:Button runat="server" ID="btnSave" CausesValidation="false" OnClick="btnSave_Click" Text="Zapisz" />
    </asp:Panel>
</asp:Content>
