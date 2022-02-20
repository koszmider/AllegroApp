<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true" CodeBehind="ProductCatalog.Shop.aspx.cs"
    Inherits="LajtIt.Web.ProductShop" %>

<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<%@ Register Src="~/Controls/ShopProduct.ascx" TagName="ShopProduct" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upShop" runat="server">
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <uc:ProductMenu runat="server" SetTab="td8"></uc:ProductMenu>
    <asp:Panel runat="server">
        <asp:UpdatePanel runat="server" ID="upAllegro">
            <ContentTemplate>

                
    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" id="rblOnlyActive" OnSelectedIndexChanged="chbOnlyActiveShops_CheckedChanged" AutoPostBack="true">
        <asp:ListItem Selected="false">wszystkie</asp:ListItem>
        <asp:ListItem Selected="True">aktywne</asp:ListItem>
        <asp:ListItem Selected="false">nieaktywne</asp:ListItem>

    </asp:RadioButtonList> <br /> 
                
                <uc:ShopProduct runat="server" ID="ucShopProduct"></uc:ShopProduct>

                <table>
                    <tr valign="top">
                        <td>

                            <asp:Button
                                runat="server" ID="btnUpdateImages" Text="Zaktualizuj zdjęcia" OnClick="btnUpdateImages_Click" Visible="false"
                                OnClientClick="return confirm('Zaktualizować zdjęcia w sklepie?');" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                                    runat="server" ID="btnCreate" Text="Utwórz produkt w sklepie" OnClick="btnCreate_Click"
                                    OnClientClick="return confirm('Utworzyć produkt w sklepie?');" Visible="false" />&nbsp;&nbsp;<asp:Button
                                        runat="server" ID="btnUpdate" Text="Aktualizuj produkt w sklepie" OnClick="btnUpdate_Click"
                                        OnClientClick="return confirm('Zaktualizować produkt w sklepie?');" Visible="false" />

                            <span style="position: absolute;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upAllegro">
                                    <ProgressTemplate>
                                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </span>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
