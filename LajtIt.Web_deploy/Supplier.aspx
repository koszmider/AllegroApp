<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="Supplier.aspx.cs" Inherits="LajtIt.Web.Supplier" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/SupplierMenu.ascx" TagName="SupplierMenu" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc:SupplierMenu runat="server" SetTab="td1"></uc:SupplierMenu>
    <asp:UpdatePanel runat="server" ID="up">

        <ContentTemplate>
            <h1>
                <asp:Literal runat="server" ID="litSupplier"></asp:Literal></h1>
            <table class="mytable">
                
                <tr>
                    <td style="width: 300px;">Właściciel
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlSupplierOwner" DataTextField="Name" DataValueField="SupplierOwnerId" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px;">Kraj pochodzenia
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCountry" DataTextField="Name" DataValueField="CountryCode" AppendDataBoundItems="true">
                            <asp:ListItem Value="">-- brak -- </asp:ListItem>

                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Nazwa
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txbName" MaxLength="256"></asp:TextBox><asp:RequiredFieldValidator
                            runat="server" ControlToValidate="txbName" Text="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Aktywny
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chbIsActive" />
                    </td>
                </tr>
                <tr>
                    <td>Drop shipping
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chbIsDropShippingAvailable" />
                    </td>
                </tr>
                <tr>
                    <td>Rabat zakupowy
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txbRebate" MaxLength="10" Width="50" Style="text-align: right"></asp:TextBox>%
                <asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" ControlToValidate="txbRebate" Text="*"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                        runat="server" ControlToValidate="txbRebate" ValidationExpression="\d{1,2},\d{1,2}"
                        Text="Zły format"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>Marża sprzedażowa
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txbMargin" MaxLength="10" Width="50" Style="text-align: right"></asp:TextBox>%
                <asp:RequiredFieldValidator
                    ID="RequiredFieldValidator3" runat="server" ControlToValidate="txbMargin" Text="*"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                        ID="RegularExpressionValidator1" runat="server" ControlToValidate="txbMargin"
                        ValidationExpression="\d{1,3},\d{1,2}" Text="Zły format"></asp:RegularExpressionValidator>
                    </td>
                </tr>
        
                <tr>
                    <td>Czas dostawy 
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlShopDelivery" DataTextField="Name" DataValueField="DeliveryId" />
                    </td>
                </tr>
            </table>

            <h2>Zamówienia</h2>

            <table class="mytable">
                <tr>
                    <td style="width: 300px;">Sposób składania zamówień</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlOrderingType" DataTextField="Name" DataValueField="OrderingTypeId" /></td>
                </tr>
                <tr>
                    <td>B2b adres</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbB2bUrl" MaxLength="1000" Width="400"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>B2b kontakt email</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbB2bEmail" MaxLength="254" Width="400"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Dni składania zamówień<br />(nie oznaczaj jeśli codziennie)</td>
                    <td>
                        <asp:CheckBoxList runat="server" RepeatDirection="Horizontal" ID="chblOrderWeekDays">
                            <asp:ListItem Value="1">poniedziałek</asp:ListItem>
                            <asp:ListItem Value="2">wtorek</asp:ListItem>
                            <asp:ListItem Value="3">środa</asp:ListItem>
                            <asp:ListItem Value="4">czwartek</asp:ListItem>
                            <asp:ListItem Value="5">piątek</asp:ListItem>
                            <asp:ListItem Value="6">sobota</asp:ListItem>
                            <asp:ListItem Value="7">niedziela</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>

            <h2>Aktalizacja danych</h2>

            <table class="mytable">
                <tr>
                    <td style="width: 300px;">Typ aktualizacji
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlImportType" DataTextField="Name" DataValueField="ImportTypeId" Width="400"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Uwagi
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txbImportComment" TextMode="MultiLine" Width="400" Rows="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Url pliku aktualizującego
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txbImportUrl" Width="400"></asp:TextBox>
                        <asp:HyperLink runat="server" ID="hlUrl" Visible="false" Target="_blank">>>>>>></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>Śledź zmiany ilości
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chbIsQuantityTrackingAvailable" />
                    </td>
                </tr>
                <tr>
                    <td>Minimalna ilość 
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txbQuantityMin" TextMode="Number"></asp:TextBox> (zostaw puste by nie wyłączać aktywności produktu po osiągnięciu wymaganego poziomu)
                    </td>
                </tr>
                <tr>
                    <td>Automatyczne dostosowywanie cen
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlRoundPriceType" Width="400">
                            <asp:ListItem Value="0">Nie zmieniaj</asp:ListItem>
                            <asp:ListItem Value="1">Zaokrąglaj w dół do liczby całkowitej</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
        <tr valign="top">
            <td>Cennik dostawy Allegro
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlAllegroDeliveryType" DataTextField="Name"
                    DataValueField="DeliveryCostTypeId" AppendDataBoundItems="true">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList><br />
                lub gdy nie ma paczkomatu<br />
                <asp:DropDownList runat="server" ID="ddlAllegroAlternativeDeliveryType" DataTextField="Name"
                    DataValueField="DeliveryCostTypeId" AppendDataBoundItems="true">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
            
            </td>
        </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Zapisz" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right">
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

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
