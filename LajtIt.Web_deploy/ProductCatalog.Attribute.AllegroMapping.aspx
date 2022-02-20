<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ProductCatalog.Attribute.AllegroMapping.aspx.cs" Inherits="LajtIt.Web.ProductAttributeAllegroMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Controls/AttributeMenu.ascx" TagName="AttributeMenu" TagPrefix="uc" %>
<%@ Register Src="~/Controls/ShopCategoryControlJson.ascx" TagName="ShopCategoryControl" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <uc:AttributeMenu runat="server" SetTab="td5"></uc:AttributeMenu>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>Kategoria Allegro</td>
                    <td>Wartość parametru</td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList runat="server" DataValueField="ShopCategoryId" DataTextField="Name" ID="ddlShopCategory" AutoPostBack="true" Width="150"
                            OnSelectedIndexChanged="ddlExternalSourceAllegroCategory_SelectedIndexChanged" ValidationGroup="s">
                        </asp:DropDownList></td>
                    <td>
                        <asp:DropDownList AppendDataBoundItems="true" runat="server" ID="ddlAllegroCategoryParameters" ValidationGroup="s"
                            DataTextField="value" DataValueField="key">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfv" ControlToValidate="ddlAllegroCategoryParameters" Text="Wartość wymagana" ValidationGroup="s"></asp:RequiredFieldValidator>


                    </td>
                </tr>
            </table>



        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:Button runat="server" ID="btnAttributeSave" OnClick="btnAttributeSave_Click" OnClientClick="if(Page_ClientValidate()) return confirm('Zapisać zmiany?');" Text="Zapisz" ValidationGroup="s" />


</asp:Content>
