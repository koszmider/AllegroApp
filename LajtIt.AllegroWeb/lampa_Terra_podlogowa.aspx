<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_Terra_podlogowa.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaTerraKronosPage" %>
    
<%@ Register Src="~/Controls/KronosMKolory.ascx" TagName="KronosMKolory" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace" TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="price">
        <div class="color-r">
            26,60 zł</div>
    </div>
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa podłogowa Terra</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/userdata/gfx/9985ef794b385f5cc0ccbbfe3dd0d34b.jpg"
                    alt="Lampa podłogowa Terra" />
            </div>
        </div>
        <div class="offer_spec">
            <table> 
                <tr>
                    <td>
                        Wysokość:
                    </td>
                    <td>
                        220cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Średnica abażura:
                    </td>
                    <td>
                          45cm
                    </td>
                </tr> 
                <tr>
                    <td>
                        Liczba uchwytów na żarówki:
                    </td>
                    <td>
                        1 x E27 
                    </td>
                </tr>
                <tr>
                    <td>
                        Żarówki w komplecie:
                    </td>
                    <td>
                        Nie
                    </td>
                </tr>
                <tr>
                    <td>
                        Rodzaj żarówki:
                    </td>
                    <td>
                        Świetlówka energooszczędna
                    </td>
                </tr>
                <tr>
                    <td>
                        Max. moc:
                    </td>
                    <td>
                        45W (odpowiednik 120W dla żarówki tradycyjnej)
                    </td>
                </tr>
                <tr>
                    <td>
                        Kolor abażura:
                    </td>
                    <td>
                        dowolny spośród dostępnych *
                    </td>
                </tr>
                <tr>
                    <td>
                        Kolor podstawki:
                    </td>
                    <td>
                        czarny 
                    </td>
                </tr>
                <tr>
                    <td>
                        Materiał abażura:
                    </td>
                    <td>
                        tworzywo sztuczne
                    </td>
                </tr>
                <tr>
                    <td>
                        Materiał podstawki:
                    </td>
                    <td>
                        stal, powłoka proszkowa
                    </td>
                </tr>
                <tr>
                    <td>
                        Długość kabla:
                    </td>
                    <td>
                        6m
                    </td>
                </tr>
                <tr>
                    <td>
                        Włącznik:
                    </td>
                    <td>
                        na kablu
                    </td>
                </tr>
                <tr>
                    <td>
                        Abażur wymaga składania:
                    </td>
                    <td>
                         Nie
                    </td>
                </tr>
                <tr>
                    <td>
                        Zestaw obejmuje:
                    </td>
                    <td>
                        abażur, podstawa
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="offer_spec_comments">
        * Kolory abażurów: <uc:KronosMKolory ID="KronosMKolory1" runat="server" /><br />
 
     
    </div>
    <div class="regular_text">
      
      <div style="float:left"><img src="http://www.lajtit.pl/userdata/gfx/3e5e403027184e23a953b901407c0c60.jpg" style="height:400px; margin: 0 20px 0 0" alt="" /></div>
        <div class="regular_text">
            Abażury lampy Terra dostępne są w wielu kolorach i w wielu układach kolorystycznych.
            Prezentujemy najbardziej popularne zestawy. Na Państwa specjalne życzenie
            możemy przygotować dowolny układ kolorów spośród dostępnych (dostępność danego koloru
            opisana jest powyżej).
        </div> 
    </div>
    <a name="more"></a>
    <div id="more">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_czy_wiesz_ze.png" alt="Czy wiesz, że ... ?" />
        <ul>
            <li>Można zamówić jedno, dwu lub wielokolorowy abażur</li>
            <li>Można zamówić sam abażur, zobacz Nasze pozostałe aukcje</li>  
            <li>Do kompletu można zakupić: lampki nocne, stołowe, podłogowe a także kinkiety. Zobacz
                Naszą pełną ofertę</li>
        </ul>
    </div>
    <a name="suggestions"></a>
    <div id="suggestions">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_produkty_uzupelniajace.png"
            alt="Produkty uzupełniające" />
            <uc:KronosProduktyUzupelniajace runat="server" />
    </div>
    <a name="wysylka"></a>
    <div id="shipping">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_wysylka.png" alt="Wysyłka" />
        <div class="regular_text">
            Zamówiony towar wysyłamy kurierem. Czas dostawy przez Kuriera to 1-2 dni robocze. 
            Zobacz pełen <a href="http://allegro.pl/my_page.php?uid=678165#jak_kupowac" target="_blank">
                proces realizacji zamówienia</a>.
        </div>
    </div>
</asp:Content>
