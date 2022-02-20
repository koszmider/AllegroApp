<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="Product.aspx.cs" Inherits="LajtIt.Web.Product" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<%@ Register Src="~/Controls/ProductCatalogGroup.ascx" TagName="ProductCatalogGroup" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function changeValue(f) {
            f.select();

        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <uc:ProductMenu runat="server" SetTab="td1"></uc:ProductMenu>
    <asp:Panel ID="Panel1" runat="server" GroupingText="Konfiguracja produktu">

        <table style="width: 100%; margin:0;padding:0">
            <colgroup>
                <col width="300" />
                <col width="" />
                <col width="100" />
            </colgroup>

            <tr>
                <td  colspan="2">
                    
                <uc:ProductCatalogGroup runat="server" ID="ucProductCatalogGroup" />

                </td>
                <td>
                    <div style="text-align: right;">
                        <asp:CheckBox runat="server" ID="chbIsFollowed" OnCheckedChanged="chbIsFollowed_CheckedChanged" AutoPostBack="true" Text="Obserwuj produkt" /></div>
                </td>
            </tr>
            <tr>
                <td>Nazwa
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txbName" AutoPostBack="true" MaxLength="100" Width="500"
                        ValidationGroup="client"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Kod / kod dostawcy
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="upCode">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txbAllegroCode" MaxLength="254" ValidationGroup="client"
                                AutoPostBack="true" OnTextChanged="txbAllegroCode_OnTextChanged" Width="500"></asp:TextBox>
                            <span style="position: absolute;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upCode">
                                    <ProgressTemplate>
                                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </span>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>Kod2 / kod2 dostawcy
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txbCode2" MaxLength="254" Width="500"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Kod EAN
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="upEan">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txbEan" MaxLength="254" ValidationGroup="client"
                                AutoPostBack="true" OnTextChanged="txbEan_OnTextChanged" Width="500"></asp:TextBox>
                            <span style="position: absolute;">
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upEan">
                                    <ProgressTemplate>
                                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </span>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </td>
            </tr>
            <tr>
                <td>Id zewnętrzne
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="upExt">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txbExternalId" MaxLength="254" ValidationGroup="client"
                                AutoPostBack="true" OnTextChanged="txbExternalId_OnTextChanged" Width="500"></asp:TextBox>
                            <span style="position: absolute;">
                                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upExt">
                                    <ProgressTemplate>
                                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </span>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        <tr>
            <td>Czas dostawy 
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlShopDelivery" DataTextField="Name" DataValueField="DeliveryId" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">-- ustalony na poziomie producenta -- </asp:ListItem>
                </asp:DropDownList>&nbsp;<asp:Label runat="server" ID="lbSupplierDeliveryTime"></asp:Label>
            </td>
        </tr>
               <tr>
                <td>Outlet/wyprzedaż</td>
                <td>
                    <asp:CheckBox runat="server" ID="chbIsOutlet" Text="Produkt wyprzedażowy lub outlet" /></td>
            </tr>
            <tr>
                <td>Wysyłka paczkomatem</td>
                <td>
                    <asp:RadioButton runat="server" ID="rbPaczkomat1" GroupName="paczkomat" Text="TAK" />
                    <asp:RadioButton runat="server" ID="rbPaczkomat0" GroupName="paczkomat" Text="NIE" />
                    <asp:RadioButton runat="server" ID="rbPaczkomat" GroupName="paczkomat" Text="nieokreślone" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr title="Produkt dostępny w listach wyboru">
                <td>Aktywny
                </td>
                <td>
                    <asp:CheckBox ID="chbIsActive" runat="server" Enabled="false" /><b>[AKT]</b> Aktywność dla wszystkich źródeł. Flaga ustawiana automatycznie na podstawie:
                    <table>
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;-
                                <asp:CheckBox ID="chbHasProductType" runat="server" Enabled="false" Text="Określony typ produktu" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;-
                                <asp:CheckBox ID="chbPrice" runat="server" Enabled="false" Text="Cena >0" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;-
                                <asp:CheckBox ID="chbImage" runat="server" Enabled="false" Text="Zdjęcie" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;-
                                <asp:CheckBox ID="chbCode" runat="server" Enabled="false" Text="Ustalony kod produktu" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;-
                                <asp:CheckBox ID="chbIsReady" runat="server" Enabled="false" Text="Skonfigurowany" />
                            </td>
                        </tr>
                    </table>
                    oraz
                    <table>
                        <tr title="Produkt dostępny w listach wyboru">
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-
                                <asp:CheckBox ID="chbIsOnStock" Enabled="false" runat="server" />Jest na stanie (wyznaczany automatycznie na podstawie stanów magazynowych)
                            </td>
                        </tr>
                    </table>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;lub
                    <table>
                        <tr title="Produkt dostępny w listach wyboru">
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-
                                <asp:CheckBox ID="chbIsAvailable" runat="server" />Kontroluj aktywność wszystkich źródeł ustawiając tę flagę
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i nie </td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-
                                <asp:CheckBox ID="chbIsDiscontinued" runat="server" Text="Wycofany" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-
                                <asp:CheckBox ID="chbIsHidden" runat="server" Text="Ukryty (obecny w systemie ale niepokazywany na żadnych listach)" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pAdmin" runat="server" GroupingText="Ceny ">
        <table style="width: 800px">
            <colgroup>
                <col width="350" />
                <col width="650" />
            </colgroup>
        
            <tr>
                <td>Typ produktu
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlProductType" DataTextField="ProductType" DataValueField="ProductTypeId">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Producent
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlSuppliers" DataTextField="DisplayName" DataValueField="SupplierId" AutoPostBack="true" OnSelectedIndexChanged="ddlSuppliers_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        <tr>
            <td>Blokuj rabaty koszykowe
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlLockRebates">
                    <asp:ListItem Value="-1">-- ustalony na poziomie producenta -- </asp:ListItem>
                    <asp:ListItem Value="1">TAK</asp:ListItem>
                    <asp:ListItem Value="0">NIE</asp:ListItem>
                </asp:DropDownList>&nbsp;<asp:Label runat="server" ID="Label1"></asp:Label>
            </td>
        </tr>
            <tr>
                <td>Cena katalogowa (brutto)
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txbPrice" AutoPostBack="true" MaxLength="10"
                        Width="200" ValidationGroup="client"></asp:TextBox><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator1" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                            ValidationGroup="client" ControlToValidate="txbPrice" Text="*" runat="server"></asp:RegularExpressionValidator><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator1" ValidationGroup="client" ControlToValidate="txbPrice"
                                Text="*" runat="server"></asp:RequiredFieldValidator><br />
                </td>
            </tr>
            <tr>
                <td>Cena promocyjna (brutto)
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txbPricePromo" AutoPostBack="true" MaxLength="10"
                        Width="200" ValidationGroup="client"></asp:TextBox><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator2" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                            ValidationGroup="client" ControlToValidate="txbPricePromo" Text="*" runat="server"></asp:RegularExpressionValidator>
                    do dnia:
                                <asp:TextBox runat="server" ID="txbPricePromoDate" Width="80"></asp:TextBox><asp:CalendarExtender
                                    ID="calDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txbPricePromoDate">
                                </asp:CalendarExtender>
                </td>
            </tr>
           
            <tr>
                <td>Cena zakupu (netto)
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txbPurchasePrice" AutoPostBack="true" MaxLength="10"
                        Width="200" ValidationGroup="client"></asp:TextBox><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator3" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                            ValidationGroup="client" ControlToValidate="txbPurchasePrice" Text="*" runat="server"></asp:RegularExpressionValidator>
                   
                </td>
            </tr>
           
        </table>
    </asp:Panel>
    <table style="width: 800px">
        <colgroup>
            <col width="350" />
            <col width="650" />
        </colgroup>

        <tr valign="top">
            <td>
                <asp:UpdatePanel runat="server" ID="upSave">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnClientDataSave" Text="Zapisz" OnClick="btnClientDataSave_Click"
                            ValidationGroup="client" /><span style="position: absolute;">
                                <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="upSave">
                                    <ProgressTemplate>
                                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </span>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="text-align: right">
                <asp:Button runat="server" ID="Button1" OnClick="btnDuplicate_Click" Text="Duplikuj produkt"
                    OnClientClick="return confirm('Zduplikować produkt? ');" /><br />
            </td>
        </tr>
        <tr>
            <td style="text-align: right" colspan="2">
                <div style="position: relative;">
                    <asp:UpdatePanel runat="server" ID="upAction1">
                        <ContentTemplate>
                            <asp:LinkButton runat="server" ID="lbtnCreateNames" OnClick="lbtnCreateNames_Click"
                                OnClientClick="return confirm('Czy zaktualizować nazwy wszystkich produktów na podstawie konfiguracji?');" CausesValidation="false" Text="Twórz nazwy" />
                            <span style="position: relative;">
                                <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="upAction1">
                                    <ProgressTemplate>
                                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </span>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
