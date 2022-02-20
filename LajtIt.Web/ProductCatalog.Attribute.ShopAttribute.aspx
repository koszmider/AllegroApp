<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ProductCatalog.Attribute.ShopAttribute.aspx.cs" Inherits="LajtIt.Web.ProductAttributeShopAttribute" %>

<%@ Register Src="~/Controls/AttributeMenu.ascx" TagName="AttributeMenu" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     
    <uc:AttributeMenu id="ucAttributeMenu" runat="server" SetTab="td1"></uc:AttributeMenu>
  
     <asp:Panel ID="pnContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td >
                        Sklep

                        <asp:DropDownList runat="server" DataValueField="ShopId" DataTextField="Name" ID="ddlShop" AutoPostBack="true" Width="150"
                            OnSelectedIndexChanged="ddlShop_SelectedIndexChanged">
                        </asp:DropDownList>


                    </td>
                </tr>
                <tr>
                    <td>Grupa atrybutów w sklepie
                        
                    </td>
                </tr>
                <tr>
                    <td >


                        <asp:DropDownList runat="server" DataValueField="ShopGroupingTypeId" DataTextField="Name" ID="ddlProductCatalogAttributeShopGroupingType" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">brak przypisania</asp:ListItem>
                        </asp:DropDownList>
                        

                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" ID="btnAttributeSave" OnClick="btnAttributeSave_Click" OnClientClick="return confirm('Zapisać zmiany?');" Text="Zapisz" /></td>
                </tr>
            </table>
        </ContentTemplate>
 
    </asp:UpdatePanel>
        </asp:Panel>

</asp:Content>
