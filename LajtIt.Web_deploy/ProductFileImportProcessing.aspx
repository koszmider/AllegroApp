<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductFileImportProcessing.aspx.cs" Inherits="LajtIt.Web.ProductFileImportProcessing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="upSearch">
        <ContentTemplate>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100px;">Produkt</td>
                    <td style="width: 100px;">Nazwa/kod/linia</td>
                    <td style="width: 100px;">Błędy</td>
                    <td></td>
                    <td style="text-align: right;"><a href="ProductFileImport.aspx">Powrót do listy</a></td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlProductExists">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>istnieje</asp:ListItem>
                            <asp:ListItem>nieistnieje</asp:ListItem>
                        </asp:DropDownList></td>
                    <td>
                        <asp:TextBox runat="server" ID="txbName"></asp:TextBox></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlValidationErrors">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>bez błędów</asp:ListItem>
                            <asp:ListItem>z błędami</asp:ListItem>
                        </asp:DropDownList></td>

                    <td>
                        <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Pokaż" />
                        <span style="position: absolute;">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upSearch">
                                <ProgressTemplate>
                                    <img src="Images/progress.gif" style="height: 20px" alt="" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </span></td>
                
                    </tr>
                <tr><td colspan="5"><asp:CheckBox runat="server" ID="chbPaging" Text="Stronnicuj wyniki" Checked="true" />
                    <asp:CheckBox runat="server" ID="chbImages" Text="Pokazuj zdjęcia" visible="false"/></td></tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="upStatus">
        <ContentTemplate>
            <table style="width:100%">
                <tr>
                    <td style="text-align: right">
                        <asp:DropDownList runat="server" ID="ddlStatus" ValidationGroup="status">
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="1">Nowy</asp:ListItem>
                            <asp:ListItem Value="0">Usunięty</asp:ListItem>
                            <asp:ListItem Value="6">Gotowy do importu</asp:ListItem>

                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ValidationGroup="status" Text="*" ControlToValidate="ddlStatus"></asp:RequiredFieldValidator>
                        <asp:LinkButton runat="server" Text="Zmień status" ValidationGroup="status" OnClick="lbtnStatusChange_Click"></asp:LinkButton><br />
                        <asp:HyperLink runat="server" ID="hlProductCatalog" Target="_blank" NavigateUrl="~/ProductCatalogForDb.aspx?FileImportId={0}&SupplierId={1}">Wyszukaj w katalogu</asp:HyperLink><span style="position: absolute;">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upStatus">
                                <ProgressTemplate>
                                    <img src="Images/progress.gif" style="height: 20px" alt="" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </span></td>
                </tr>

            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="upJoin">
        <ContentTemplate>
            <br />
            <br />
            <table style="width:100%">
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="chbAddNew" Text="Dodaj nowe" />
                        <asp:CheckBox runat="server" ID="chbUpdateExisting" Text="Aktualizuj istniejące" />
                        <asp:CheckBox runat="server" ID="chbDuplicates" Text="Sprawdzaj duplikaty" Checked="true" />
                        <asp:Button runat="server" ID="btnAddUpdate" OnClick="btnAddUpdate_Click" Text="Zapisz" OnClientClick="return confirm('Czy wykonać akcję?');" />

                    </td>    <td>Liczba rekordów: <asp:Label runat="server" ID="lblCount"></asp:Label><br />
                        Liczba połączonych: <asp:Label runat="server" ID="lblCountFound"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>Łącz po kolumnie: </td>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="rblJoinColumn" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True">Kod</asp:ListItem>
                                        <asp:ListItem>Ean</asp:ListItem>
                                        <asp:ListItem>IdZewnetrzne</asp:ListItem>
                                    </asp:RadioButtonList></td>
                                <td>
                                    <asp:LinkButton runat="server" ID="btnJoinColumnSave" OnClick="btnJoinColumnSave_Click" Text="Zapisz" /><span style="position: absolute;">
                                        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upJoin">
                                            <ProgressTemplate>
                                                <img src="Images/progress.gif" style="height: 20px" alt="" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </span></td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:GridView runat="server" ID="gvFiles" OnRowDataBound="gvFiles_RowDataBound" AutoGenerateColumns="false" Font-Size="8pt" DataKeyNames="FileDataId" BackColor="White" AllowPaging="true" OnPageIndexChanging="gvFiles_PageIndexChanging" PageSize="50">
        <Columns>
            <asp:TemplateField>
                <ItemStyle Width="30" HorizontalAlign="Right" />
                <ItemTemplate>
                    <asp:Literal ID="Literal1" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>.<asp:Literal runat="server" ID="LitId"></asp:Literal>
                    <asp:CheckBox runat="server" ID="cbOrder" />
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Literal runat="server" ID="litTotal"></asp:Literal>
                </FooterTemplate>
                <HeaderTemplate>
                    <asp:CheckBox runat="server" ID="cbOrder" onclick="javascript:SelectAllCheckboxes(this, 'cbOrder');" />
                    <asp:CheckBox runat="server" ID="cbFields" onclick="javascript:SelectAllCheckboxes(this, 'chbf');" />
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Katalog">
                <ItemTemplate>
                    <asp:HyperLink runat="server" Target="_blank" ID="hlProductCatalog" NavigateUrl="#"> 
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Comment" HeaderText="Uwagi" />
            <asp:BoundField DataField="ProductImportStatusName" HeaderText="Status" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblLinia" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfLinia" runat="server" />Linia          
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblNazwa" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfNazwa" runat="server" />Nazwa          
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblKod" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfKod" runat="server" />Kod            
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblEan" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfEan" runat="server" />Ean            
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblIdZewnetrzne" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfIdZewnetrzne" runat="server" />IdZewnetrzne            
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfStatus" runat="server" />Status          
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblStanMagazynowy" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfStanMagazynowy" runat="server" />StanMagazynowy            
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblCenaSprzedazyBrutto" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfCenaSprzedazyBrutto" runat="server" />CenaSprzedazyBrutto  
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblCenaPromocyjnaSprzedazyBrutto" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfCenaPromocyjnaSprzedazyBrutto" runat="server" />CenaPromocyjnaSprzedazyBrutto  
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblPromocjaKoniecData" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfPromocjaKoniecData" runat="server" />PromocjaKoniecData  
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblCenaZakupuNetto" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfCenaZakupuNetto" runat="server" />CenaZakupuNetto  
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblOpis" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfOpis" runat="server" />Opis           
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblKolor" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfKolor" runat="server" />Kolor           
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblKolorOpis" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfKolorOpis" runat="server" />KolorOpis           
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblKolorOprawy" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfKolorOprawy" runat="server" />Kolor           
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblTypLampy" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfTypLampy" runat="server" />TypLampy       
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZastosowanie" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZastosowanie" runat="server" />Zastosowanie       
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaTyp" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaTyp" runat="server" />ZarowkaTyp     
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaIlosc" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaIlosc" runat="server" />ZarowkaIlosc   
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaMocW" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaMocW" runat="server" />ZarowkaMocW     
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaMocLm" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaMocLm" runat="server" />ZarowkaMocLm     
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaBarwa" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaBarwa" runat="server" />ZarowkaBarwa     
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaBarwaK" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaBarwaK" runat="server" />ZarowkaBarwaK     
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaWZestawie" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaWZestawie" runat="server" />ZarowkaWZestawie     
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblMaterial" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfMaterial" runat="server" />Material       
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblMaterialOpis" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfMaterialOpis" runat="server" />MaterialOpis       
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblWymiary" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfWymiary" runat="server" />Wymiary       
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblWysokosc" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfWysokosc" runat="server" />Wysokosc       
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblSzerokosc" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfSzerokosc" runat="server" />Szerokosc      
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblDlugosc" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfDlugosc" runat="server" />Dlugosc        
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblSrednica" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfSrednica" runat="server" />Srednica        
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblWaga" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfWaga" runat="server" />Waga        
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblKlasaOchronnosci" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfKlasaOchronnosci" runat="server" />KlasaOchronnosci
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblKlasaEnergetyczna" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfKlasaEnergetyczna" runat="server" />KlasaEnergetyczna
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblInformacje" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfInformacje" runat="server" />Informacje
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblLampaLed" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfLampaLed" runat="server" />LampaLed
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblSciemniacz" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfSciemniacz" runat="server" />Sciemniacz
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblStyl" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfStyl" runat="server" />Styl
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaKatSwiecenia" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaKatSwiecenia" runat="server" />ZarowkaKatSwiecenia
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaStart" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaStart" runat="server" />ZarowkaStart
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaIloscCykli" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaIloscCykli" runat="server" />ZarowkaIloscCykli
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaRA" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaRA" runat="server" />ZarowkaRA
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaZywotnosc" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaZywotnosc" runat="server" />ZarowkaZywotnosc
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblWlacznik" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfWlacznik" runat="server" />Wlacznik
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaKsztalt" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaKsztalt" runat="server" />ZarowkaKsztalt
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZarowkaKolorSzkla" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZarowkaKolorSzkla" runat="server" />ZarowkaKolorSzkla
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblRuchomeZrodloSwiatla" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfRuchomeZrodloSwiatla" runat="server" />RuchomeZrodloSwiatla
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblRegulacjaWysokosci" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfRegulacjaWysokosci" runat="server" />RegulacjaWysokosci
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblPilot" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfPilot" runat="server" />Pilot
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblCzujnikRuchu" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfCzujnikRuchu" runat="server" />CzujnikRuchu
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblCecha" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfCecha" runat="server" />Cecha
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblAkcesoria" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfAkcesoria" runat="server" />Akcesoria
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblWiFi" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfWiFi" runat="server" />WiFi
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZasilanie" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZasilanie" runat="server" />Zasilanie
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblMontaz" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfMontaz" runat="server" />Montaz
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZdjecie1" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZdjecie1" runat="server" />Zdjecie1
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZdjecie2" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZdjecie2" runat="server" />Zdjecie2
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZdjecie3" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZdjecie3" runat="server" />Zdjecie3
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZdjecie4" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZdjecie4" runat="server" />Zdjecie4
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblZdjecie5" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfZdjecie5" runat="server" />Zdjecie5
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblPaczkaDlugosc" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfPaczkaDlugosc" runat="server" />PaczkaDlugosc
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblPaczkaSzerokosc" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfPaczkaSzerokosc" runat="server" />PaczkaSzerokosc
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblPaczkaWysokosc" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chbfPaczkaWysokosc" runat="server" />PaczkaWysokosc
                </HeaderTemplate>
            </asp:TemplateField>


        </Columns>

    </asp:GridView>

</asp:Content>
