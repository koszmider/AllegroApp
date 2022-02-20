<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    CodeBehind="ProductCatalog.aspx.cs" Inherits="LajtIt.Web.ProductCatalog" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/UploadImage.ascx" TagName="UploadImage" TagPrefix="uc" %> 
<%@ Register Src="~/Controls/ProductImages.ascx" TagName="ProductImages" TagPrefix="uc" %>
<%@ Register Src="~/Controls/ShopProduct.ascx" TagName="ShopProduct" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
            <%-- <asp:PostBackTrigger ControlID="gvProductCatalog" />--%>
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0">
                <tr valign="top">
                    <td width="200">
                        <asp:ListBox Rows="8" runat="server" ID="lsbxStatus" SelectionMode="Multiple" Width="200">
                            <asp:ListItem Value="1" Selected="True">Aktywny - gotowy</asp:ListItem>
                            <asp:ListItem Value="8">Aktywny - niegotowy</asp:ListItem>
                            <asp:ListItem Value="9">Dostępny u producenta</asp:ListItem>
                            <asp:ListItem Value="10">Niedostępny u producenta</asp:ListItem>
                            <asp:ListItem Value="0">Nieaktywny</asp:ListItem>
                            <%-- <asp:ListItem Value="6">Sklep - aktywny</asp:ListItem>
                            <asp:ListItem Value="2">Allegro - aktywny</asp:ListItem>
                            <asp:ListItem Value="4">Allegro - gotowy</asp:ListItem>
                            <asp:ListItem Value="5">Allegro - niegotowy</asp:ListItem>--%>
                            <asp:ListItem Value="11">Skonfigurowany</asp:ListItem>
                            <asp:ListItem Value="12">Nieskonfigurowany</asp:ListItem>
                            <asp:ListItem Value="-1">Wycofany</asp:ListItem>
                            <asp:ListItem Value="-2">Niewycofany</asp:ListItem>
                            <asp:ListItem Value="-3">Ukryty</asp:ListItem>
                            <asp:ListItem Value="-4">Nieukryty</asp:ListItem>
                        

                        </asp:ListBox>

                    </td>
                    <td width="200">
                        <asp:ListBox Rows="8" runat="server" ID="lbxSearchSupplier" SelectionMode="Multiple" Width="200" DataTextField="DisplayName" DataValueField="SupplierId"></asp:ListBox>

                    </td>
                    <td width="200">
                        <asp:TextBox runat="server" ID="txbSearchName" MaxLength="50" Width="200"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
                            TargetControlID="txbSearchName"
                            WatermarkText="Nazwa/Tytuł/Kod produktu/Ean"
                            WatermarkCssClass="watermarked" />
                        <br />
                         
                        <table>
                           
                            <tr><td colspan="2">

                               
                        <asp:TextBox runat="server" ID="txbPriceFrom" MaxLength="50" Width="90"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                            TargetControlID="txbPriceFrom"
                            WatermarkText="cena od"
                            WatermarkCssClass="watermarked" />
                        <asp:TextBox runat="server" ID="txbPriceTo" MaxLength="50" Width="90"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server"
                            TargetControlID="txbPriceTo"
                            WatermarkText="cena do"
                            WatermarkCssClass="watermarked" />
                                </td></tr>
                            <tr> 
                                <td colspan="2">
                                    <asp:CheckBoxList runat="server" Font-Size="9pt" ID="chblWarehouse" DataTextField="Name" DataValueField="WarehouseId" RepeatDirection="Horizontal" RepeatColumns="2"></asp:CheckBoxList></td>
                            </tr>
                        </table>
                    </td>

                    <td>


                        <table>
                             <tr>
                                <td style="font-weight: bold;">Promocja:</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbPromo0" Text="NIE" /><asp:CheckBox runat="server" ID="chbPromo1" Text="TAK" /></td>
                                <td style="font-weight: bold;">Obserw.:</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbIsFollowed" Text="TAK" /></td>
                            </tr>
                            <tr>
                                 <td style="font-weight: bold;">Outlet:</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbIsOutlet" Text="TAK" /></td>
                            </tr>
                          
                          
                        </table>


                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" colspan="4">
                        <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Szukaj" Width="100%" /></td>
                </tr>

            </table>

            <asp:GridView runat="server" ID="gvProductCatalog" DataKeyNames="ProductCatalogId" Width="100%" BackColor="white" PagerSettings-Position="TopAndBottom" PagerSettings-Mode="NumericFirstLast"
                AllowPaging="true" OnPageIndexChanging="gvProductCatalog_OnPageIndexChanged" OnDataBound="gvProductCatalog_DataBound"
                OnRowCommand="gvProductCatalog_RowCommand"
                PageSize="25" ShowFooter="true" OnRowDataBound="gvProductCatalog_OnRowDataBound"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="50" HorizontalAlign="Right" />
                        <ItemTemplate>
                            <itemtemplate><asp:Literal runat="server" ID="liId"></asp:Literal></itemtemplate>
                            <asp:Literal runat="server" ID="LitId"></asp:Literal>
                            <asp:CheckBox runat="server" ID="chbOrder" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                        </HeaderTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Width="150" />
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlPreview" Target="_blank" NavigateUrl="/ProductCatalog.Preview.aspx?id={0}&idSupplier={1}">
                                <asp:Image runat="server" ID="imgImage" Width="150" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Produkt">
                        <ItemTemplate>
                            <div style="width: calc(100% - 70px); float: left;">
                                <asp:HyperLink runat="server" ID="hlProduct" NavigateUrl="Product.aspx?id={0}"></asp:HyperLink><%--<br />
                                (<asp:HyperLink runat="server" ID="hlProductAllegro" NavigateUrl="ProductCatalog.Allegro.aspx?id={0}"></asp:HyperLink>)<br />
                                <asp:Label runat="server" ID="lblNewName"></asp:Label>--%>
                            </div>
                           
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="litTotal"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kod<br>(kod EAN)">
                        <ItemStyle HorizontalAlign="Right" Width="100" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCode"></asp:Label><br />
                            <asp:Label runat="server" ID="lblCode2"></asp:Label><br />
                            <asp:Label runat="server" ID="lblCodeSupplier"></asp:Label><br />
                            <asp:Label runat="server" ID="lblExternalId"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ilość">
                        <ItemStyle HorizontalAlign="Center" Width="100" />
                        <ItemTemplate>
                            <div style="height: 20px;"></div>

                            <div style="height: 50px;">
                                <asp:Label runat="server" ID="lblQuantity"></asp:Label></div>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label runat="server" ID="lblSupplierQuantity" Text="({0})" ToolTip="Dostępność u producenta" Visible="false"></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cena">
                        <ItemStyle HorizontalAlign="Right" Width="100" />
                        <ItemTemplate>

                            <table style="width: 100px;">
                                <tr>
                                    <td style="width: 30px;">
                                        <asp:ImageButton OnClick="imgCat_Click" ToolTip="Cena katalogowa" runat="server" ID="imgCat" ImageUrl="~/Images/k.png" Width="30" /></td>
                                    <td style="width: 70px; text-align: right;">
                                        <asp:Label runat="server" ID="lblSellPrice"></asp:Label></td>
                                </tr>
    
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Akcje" Visible="false">
                        <ItemStyle HorizontalAlign="Right" Width="312" />
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txbSpec" TextMode="MultiLine" Columns="40" Rows="15"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
            </asp:GridView>
            <div style="text-align:right">
                Na stronie: <asp:TextBox runat="server" ID="txbPageSize" Text="15" Width="35" TextMode="Number"></asp:TextBox>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    Akcja:
    <asp:DropDownList runat="server" ID="ddlAction" AutoPostBack="true" OnSelectedIndexChanged="ddlAction_OnSelectedIndexChanged">
        <asp:ListItem Value="-1">-- wybierz --</asp:ListItem>
        <asp:ListItem Value="1">Aktywowanie/deaktywowanie produktów</asp:ListItem>
        <asp:ListItem Value="2">Aktualizacja produktów sklep/Allegro</asp:ListItem>
        <asp:ListItem Value="11">Allegro - Wykonaj akcję na aukcjach Allegro</asp:ListItem>
        <asp:ListItem Value="8">Allegro - Zmień cennik dostawy</asp:ListItem>
        <asp:ListItem Value="20">Katalog - Aktualizacja ustawień</asp:ListItem>
        <asp:ListItem Value="23">Katalog - Czasy dostaw</asp:ListItem>
        <asp:ListItem Value="7">Katalog - Przypisz/zmień dostawców</asp:ListItem> 
        <asp:ListItem Value="14">Katalog - Przypisywanie produktów do grup</asp:ListItem>
        <asp:ListItem Value="15">Katalog - Wiązanie wzajemne produktów</asp:ListItem>
        <asp:ListItem Value="17">Katalog - Tworzenie nazw produktów</asp:ListItem>
        <asp:ListItem Value="19">Katalog - Wgrywanie zdjęć</asp:ListItem>
        <asp:ListItem Value="21">Katalog - eksport do pliku</asp:ListItem>
        <asp:ListItem Value="16">Sklep - tworzenie/aktualizacja produktów</asp:ListItem>
        <asp:ListItem Value="13">Sklep - aktualizacja zdjęć w sklepie</asp:ListItem>
        <asp:ListItem Value="22">Sklep - tworzenie opisów</asp:ListItem>
    </asp:DropDownList>
    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional"
        ID="upAction">
        <ContentTemplate>
            <asp:Panel runat="server" GroupingText="Akcje" Visible="false" ID="pAction" ClientIDMode="Static">
                <table>
                    <tr>
                        <td>Zastosuj do
                        </td>
                        <td>&nbsp;<asp:RadioButton runat="server" GroupName="batch" ID="rbtnAddToBatchAll" Text="wszystkich wyszukanych produktów" />
                            <asp:RadioButton runat="server" GroupName="batch" ID="rbtnAddToBatchSelected" Checked="true" Text="wybranych produktów" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="pnDelivery" runat="server" Visible="false">
                                Ustaw niestandardowe czasy dostaw dla produktów
                <asp:DropDownList runat="server" ID="ddlShopDelivery" DataTextField="Name" DataValueField="DeliveryId" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">-- ustalony na poziomie producenta -- </asp:ListItem>
                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel ID="pnDescriptions" runat="server" Visible="false">
                                Wygeneruj opis i wyślij automatycznie do sklepu:
                                <asp:CheckBoxList runat="server" ID="chblShops" DataValueField="ShopId" DataTextField="Name"></asp:CheckBoxList>
                            </asp:Panel>
                            <asp:Panel ID="pnSettings" runat="server" Visible="false">
                                Aktywuj/deaktywuj ustawienia.
                                 
                                <table>
                                    <tr valign="top">
                                        <td>
                                            <asp:ListBox runat="server" ID="lsbxSettings" SelectionMode="Multiple" Rows="2">
                                                <asp:ListItem Value="1">Paczkomaty</asp:ListItem>
                                                <asp:ListItem Value="0">Outlet</asp:ListItem>
                                            </asp:ListBox></td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlSettingsStatus">
                                                <asp:ListItem Value="0">--- nie zmieniaj nic ---</asp:ListItem>
                                                <asp:ListItem Value="1">aktywny</asp:ListItem>
                                                <asp:ListItem Value="2">nieaktywny</asp:ListItem>

                                            </asp:DropDownList></td>

                                    </tr>
                                    <tr>
                                        <td>Cena promocyjna (brutto)
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txbPricePromo" AutoPostBack="true" MaxLength="10"
                                                Width="60"></asp:TextBox>%<asp:RegularExpressionValidator
                                                    ID="RegularExpressionValidator2" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                                    ControlToValidate="txbPricePromo" Text="*" runat="server"></asp:RegularExpressionValidator>
                                            do dnia:
                                <asp:TextBox runat="server" ID="txbPricePromoDate" Width="80"></asp:TextBox><asp:CalendarExtender
                                    ID="calDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txbPricePromoDate"></asp:CalendarExtender> (wstaw 0 by usunąć promocję lub nie wypełniaj by nie zmieniać)
                                        </td>
                                    </tr>
        <tr>
            <td>Blokuj rabaty koszykowe
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlLockRebates">
                    <asp:ListItem Value="-2">-- nie zmieniaj ustawień -- </asp:ListItem>
                    <asp:ListItem Value="-1">-- ustalony na poziomie producenta -- </asp:ListItem>
                    <asp:ListItem Value="1">TAK</asp:ListItem>
                    <asp:ListItem Value="0">NIE</asp:ListItem>
                </asp:DropDownList>&nbsp;<asp:Label runat="server" ID="Label1"></asp:Label>
            </td>
        </tr>
                                     
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnStatus" runat="server" Visible="false">
                                Aktywuj/deaktywuj produkty.
                                 
                                <table>
                                    <tr valign="top">
                                        <td>
                                            <asp:ListBox runat="server" ID="lsbxSource" SelectionMode="Multiple" Rows="6">
                                                <asp:ListItem Value="1">Katalog produktów - aktywny</asp:ListItem>
                                                <asp:ListItem Value="5">Katalog produktów - skonfigurowany</asp:ListItem>
                                             
                                                <asp:ListItem Value="4">Katalog produktów - ukryty</asp:ListItem>
                                                <asp:ListItem Value="-1">Katalog produktów - wycofany</asp:ListItem>

                                            </asp:ListBox></td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlStatus">
                                                <asp:ListItem Value="0">--- nie zmieniaj nic ---</asp:ListItem>
                                                <asp:ListItem Value="1">aktywny</asp:ListItem>
                                                <asp:ListItem Value="2">nieaktywny</asp:ListItem>

                                            </asp:DropDownList></td>

                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnProductUpdate" runat="server" Visible="false">
                                Aktualizacja produktów                                 
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ListBox runat="server" ID="lsbxShops" SelectionMode="Multiple" DataValueField="ShopId" DataTextField="Name"> 
                                            </asp:ListBox></td>
                                        <td>
                                            <asp:CheckBoxList runat="server" ID="chblShopColumnType" RepeatDirection="Horizontal" DataValueField="ShopColumnTypeId" DataTextField="ColumnName"> 
                                            </asp:CheckBoxList></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnCreateAuctions" runat="server" Visible="false">
                                Dodaj prodkuty do wystawienia na Allegro
                                <asp:RadioButton runat="server" GroupName="newBatch" ID="rbBatchNew" Checked="true"
                                    Text="Nowy batch" />
                                <asp:TextBox runat="server" ID="txbBatchName" MaxLength="50"></asp:TextBox>
                                <asp:RadioButton runat="server" GroupName="newBatch" ID="rbBatchExisting" Text="Istniejący batch" />
                                <asp:DropDownList runat="server" ID="ddlBatch" DataValueField="BatchId" DataTextField="Name">
                                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel ID="pnScheduleCreate" runat="server" Visible="false">
                                Dodaj do harmonogramu wystawienia na Allegro 
                                <asp:DropDownList runat="server" ID="ddlSchedule" DataValueField="ProductCatalogForAllegroId" DataTextField="Name">
                                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel ID="pnSupplier" runat="server" Visible="false">
                                Przypisz produkty do dostawcy
                                <asp:DropDownList runat="server" ID="ddlSupplierUpdate" DataValueField="SupplierId" DataTextField="DisplayName">
                                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel ID="pnDeliveryCostType" runat="server" Visible="false">
                                Przypisz opcję dostawy do produktów
                                <asp:DropDownList runat="server" ID="ddlDeliveryCostType" DataValueField="DeliveryCostTypeId" DataTextField="Name">
                                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel ID="pnAllegroAuction" runat="server" Visible="false">
                                Wybierz akcję do zastosowania dla aukcji powiązanych z produktem
                                <asp:DropDownList runat="server" ID="ddlAllegroAuction" DataValueField="TypeId" DataTextField="Name">
                                </asp:DropDownList>
                                <asp:CheckBox runat="server" ID="chbDoNotReActive" Text="Nie wznawiaj aukcji w przyszłości" Checked="false" />
                            </asp:Panel>
                            
                            <asp:Panel ID="pnUpdateShopImages" runat="server" Visible="false">
                                Aktualizuj zdjęcia w sklepie
                                
                            </asp:Panel>
                            <asp:Panel ID="pnCreateNames" runat="server" Visible="false">
                                Aktualizuj nazwy produktów 
                              

                            </asp:Panel>

                            <asp:Panel ID="pnProductCatalogGroup" runat="server" Visible="false">
                                Przypisz grupę do produktów
                                <br />
                                <table>
                                    <tr>
                                        <td>Producent</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlProductCatalogGroupSupplier" Width="300">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Rodzina</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlProductCatalogGroupFamily" Width="300">
                                            </asp:DropDownList>
                                            <asp:TextBox runat="server" ID="txbFamilyName" Width="200"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server"
                            TargetControlID="txbFamilyName"
                            WatermarkText="Nowa rodzina"
                            WatermarkCssClass="watermarked" />
                                            

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Grupa</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlProductCatalogGroup" Width="300">
                                            </asp:DropDownList>
                                            <asp:TextBox runat="server" ID="txbGroupName" Width="200"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server"
                            TargetControlID="txbGroupName"
                            WatermarkText="Nowa grupa"
                            WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr> 
                                </table>


                                <asp:CascadingDropDown
                                    ID="ccdSuppliers"
                                    runat="server"
                                    ServicePath="AutoComplete.asmx"
                                    ServiceMethod="GetSuppliers"
                                    TargetControlID="ddlProductCatalogGroupSupplier"
                                    Category="Supplier" />



                                <asp:CascadingDropDown
                                    ID="ccdCountries"
                                    runat="server"
                                    ServicePath="AutoComplete.asmx"
                                    ServiceMethod="GetFamilies"
                                    TargetControlID="ddlProductCatalogGroupFamily"
                                    ParentControlID="ddlProductCatalogGroupSupplier"
                                    Category="Family"
                                    EmptyText="Brak rodzin" />


                                <asp:CascadingDropDown
                                    ID="ccdLeagues"
                                    runat="server"
                                    ServicePath="AutoComplete.asmx"
                                    ServiceMethod="GetGroups"
                                    TargetControlID="ddlProductCatalogGroup"
                                    ParentControlID="ddlProductCatalogGroupFamily"
                                    Category="Group"
                                    EmptyText="Brak grup" />


                                

                            </asp:Panel>
                            <asp:Panel ID="pnShopCreateUpdateProducts" runat="server" Visible="false">
                                <p>
                                    Utwórz/zaktualizuj wybrane produkty w sklepie lajtit.pl
                                </p>
                            </asp:Panel>
                            <asp:Panel ID="pnImages" runat="server" Visible="false">
                                <p>
                                    Utwórz/zaktualizuj wybrane produkty w sklepie lajtit.pl
                                </p>

                                <uc:UploadImage ID="ucUploadImage" runat="server"></uc:UploadImage>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnAction" />
                                </Triggers>

                            </asp:UpdatePanel>
                            <asp:Button runat="server" ID="btnAction" OnClick="btnAction_Click" OnClientClick="return confirm('Wykonać daną akcję?');"
                                Text="Zapisz" />
                            <span style="position: absolute;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upAction">
                                    <ProgressTemplate>
                                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </span>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Label runat="server" ID="lblOK"></asp:Label>
            <asp:ModalPopupExtender ID="mpeShop" runat="server"
                TargetControlID="lblOK"
                PopupControlID="pnShopProducts"
                BackgroundCssClass="modalBackground"
                DropShadow="true"
                CancelControlID="btnCancel"
                PopupDragHandleControlID="Panel1" />

            <asp:Panel runat="server" ID="pnShopProducts" BackColor="White" Style="width: 800px;  background-color: white; height: 550px; padding: 10px">
                <asp:Panel runat="server" ID="Panel1"> 
                </asp:Panel>
                <div style="width: 800px; height: 530px;overflow:scroll;">
                    <uc:ShopProduct runat="server" ID="ucShopProduct" OnlyActiveShops="true" />


                </div>
                <asp:Button runat="server" ID="btnOK" Text="OK" Visible="false" />
                <asp:LinkButton runat="server" ID="btnCancel" Text="Anuluj" />
            </asp:Panel>



        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="text-align: right; margin: 10px;">
                <a href="Product.New.aspx">Dodaj nowy produkt</a>
            </div>
</asp:Content>
