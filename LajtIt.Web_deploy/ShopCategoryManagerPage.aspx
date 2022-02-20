<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="ShopCategoryManagerPage.aspx.cs" Inherits="LajtIt.Web.ShopCategoryManagerPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/ShopCategoryControlJson.ascx" TagPrefix="uc1" TagName="ShopCategoryControlJson" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: right;">
        <asp:HyperLink runat="server" ID="hlPromotions" NavigateUrl="/ShopCategoryManager.aspx" Text="Zobacz wszystkie konfiguracje"></asp:HyperLink>
    </div>
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShopsAdd" />
            <asp:PostBackTrigger ControlID="btnShopsDel" />
            <asp:PostBackTrigger ControlID="chbConditonPromotion" />
            <asp:PostBackTrigger ControlID="chbConditonPromotionRate" />
            <asp:PostBackTrigger ControlID="chbPriceFrom" />
            <asp:PostBackTrigger ControlID="chbPriceTo" />
            <asp:PostBackTrigger ControlID="btnAttributeAdd" />
            <asp:PostBackTrigger ControlID="btnAttributeDel" />
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>

        <ContentTemplate>
            <table>
                <tr>
                    <td style="width: 250px">Sklep</td>
                    <td>
                        <asp:Label ID="lblShop" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 250px">Nazwa</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbName" MaxLength="256" Width="500"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h2>Konfiguracja</h2>
                    </td>
                </tr>
                <tr>
                    <td style="width: 250px">Aktywna</td>
                    <td>
                        <asp:CheckBox runat="server" ID="chbIsActive" /></td>
                </tr>
                <tr valign="top">
                    <td style="width: 250px">Kategoria główna</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbMainCategoryPriority" TextMode="Number" MaxLength="2"></asp:TextBox><br />pozostaw puste by nie nadpisywać kategorii głównej (wynikającej z przypisania rodzaju produktu) lub wstaw wartość. 
                        Im wyższa wartość tym bardziej produkt otrzyma tę kategorię jako główną. Jeśli produkt zakwalifikuje się do kilku kategorii dodatkowych, to pole zdecyduje, która kategoria będzie główną.<br /><br /></td>
                </tr>

                <tr>
                    <td style="width: 250px">Wybrane produkty dołącz do kategorii</td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <uc1:ShopCategoryControlJson runat="server" ID="ucShopCategoryControl" />
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnShopsAdd" OnClick="btnShopsAdd_Click" Text=">>" /><br />
                                    <asp:Button runat="server" ID="btnShopsDel" OnClick="btnShopsDel_Click" Text="<<" />
                                </td>
                                <td>
                                    <asp:ListBox runat="server" ID="lbxShopsSelected" Rows="8" SelectionMode="Multiple" 
                                        DataTextField="CategoryPath" DataValueField="CategoryId" Width="320"></asp:ListBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td style="width: 250px">
                                    <asp:CheckBox runat="server" ID="chbCountry" OnCheckedChanged="chbShopCategoryManagerPage_CheckedChanged" AutoPostBack="true" Text="Kraj dostawcy" /></td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCountry" DataValueField="CountryCode" DataTextField="Name" AppendDataBoundItems="true">
                                        <asp:ListItem Value="">-</asp:ListItem> 
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 250px">
                                    <asp:CheckBox runat="server" ID="chbAvailability" OnCheckedChanged="chbShopCategoryManagerPage_CheckedChanged" AutoPostBack="true" Text="Dostępność" /></td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlAvailability">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="0">na zamówienie</asp:ListItem>
                                        <asp:ListItem Value="1">od ręki</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 250px">
                                    <asp:CheckBox runat="server" ID="chbOutlet" OnCheckedChanged="chbShopCategoryManagerPage_CheckedChanged" AutoPostBack="true" Text="Outlet" /></td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlOutlet">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="0">nie</asp:ListItem>
                                        <asp:ListItem Value="1">tak</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 250px">
                                    <asp:CheckBox runat="server" ID="chbConditonPromotion" OnCheckedChanged="chbShopCategoryManagerPage_CheckedChanged" AutoPostBack="true" Text="Promocja" /></td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlConditonPromotion">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="0">produkty bez promocji</asp:ListItem>
                                        <asp:ListItem Value="1">produkty w promocji</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbConditonPromotionRate" OnCheckedChanged="chbShopCategoryManagerPage_CheckedChanged" AutoPostBack="true" Text="Promocja x% i więcej" /></td>
                                <td>
                                    <asp:TextBox runat="server" TextMode="Number" ID="txbPromotionRate" Value="50"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbPriceFrom" OnCheckedChanged="chbShopCategoryManagerPage_CheckedChanged" AutoPostBack="true" Text="Cena od" /></td>
                                <td>
                                    <asp:TextBox runat="server" TextMode="Number" ID="txbPriceFrom" Value="0"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbPriceTo" OnCheckedChanged="chbShopCategoryManagerPage_CheckedChanged" AutoPostBack="true" Text="Cena do" /></td>
                                <td>
                                    <asp:TextBox runat="server" TextMode="Number" ID="txbPriceTo" Value="0"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Producenci</td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:ListBox runat="server" ID="lbxSuppliers" Rows="10" SelectionMode="Multiple" 
                                                    DataTextField="Name" DataValueField="SupplierId" Width="320"></asp:ListBox></td>
                                            <td>
                                                <asp:Button runat="server" ID="btnSupplierAdd" OnClick="btnSupplierAdd_Click" Text=">>" /><br />
                                                <asp:Button runat="server" ID="btnSupplierDel" OnClick="btnSupplierDel_Click" Text="<<" />
                                            </td>
                                            <td>
                                                <asp:ListBox runat="server" ID="lbxSuppliersSelected" Rows="10" SelectionMode="Multiple" 
                                                    DataTextField="Name" DataValueField="SupplierId" Width="320"></asp:ListBox></td>
                                        </tr>
                                    </table>
                                 </td>
                            </tr>
                            <tr>
                                <td>Atrybuty</td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:ListBox runat="server" ID="lbxAttributes" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="AttributeId" Width="320"></asp:ListBox></td>
                                            <td>
                                                <asp:Button runat="server" ID="btnAttributeAdd" OnClick="btnAttributeAdd_Click" Text=">>" /><br />
                                                <asp:Button runat="server" ID="btnAttributeDel" OnClick="btnAttributeDel_Click" Text="<<" />
                                            </td>
                                            <td>
                                                <asp:ListBox runat="server" ID="lbxAttributesSelected" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="AttributeId" Width="320"></asp:ListBox></td>
                                        </tr>
                                    </table>
                                    Logika działania na atrybutach:<br />
                                    - atrybuty z tej samej grupy: przynajmniej jeden<br />
                                    - pomiędzy grupami: każda grupa musi być przypisana do produktu<br />
                                    <br />
                                    np wybrano: [Barwa światła].(Ciepła)[Barwa światła].(Zima)[Gwint żarówki].(E27)[Gwint żarówki].(E14) oznacza, że wybrane są produkty które mają Barwę światła ciepłą lub zimną <b>oraz </b>gwint E27 lub E14
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </table>



            <table>

                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" ID="btnSave" Text="Zapisz" OnClick="btnSave_Click" OnClientClick="return confirm('Zapisać zmiany?');" /></td>
                </tr>

            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
