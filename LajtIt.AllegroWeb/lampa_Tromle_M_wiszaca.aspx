<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_Tromle_M_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaWiszacaTromleMPage" %>
    
<%@ Register Src="~/Controls/TromleLKolory.ascx" TagName="TromleLKolory" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Tromle.ascx" TagName="Tromle" TagPrefix="uc" %>
<%@ Register Src="~/Controls/TromleProduktyUzupelniajace.ascx" TagName="TromleProduktyUzupelniajace" TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="price">
        <div class="color-r">
            4,50 zł</div>
    </div>
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa wisząca Tromle </h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                 <img src="http://www.lajtit.pl/public/assets/tromle/www.lajtit.pl_modne_lampy_tromle_m_biala.jpg"
                    alt="Lampa wisząca Tromle" /> 
                    <div class="offer_main_photo_comment">Biała lampa</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/tromle/www.lajtit.pl_modne_lampy_tromle_m_fioletowa.jpg"
                    alt="Lampa wisząca Tromle" />
                    <div class="offer_main_photo_comment">Fioletowa lampa</div>
            </div>
             
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Rozmiar:
                    </td>
                    <td>
                        M
                    </td>
                </tr>
                <tr>
                    <td>
                        Wysokość:
                    </td>
                    <td>
                        20cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Średnica:
                    </td>
                    <td>
                        23cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Obwód:
                    </td>
                    <td>
                        80cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Liczba uchwytów na żarówki:
                    </td>
                    <td>
                        1 x E14
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
                        Świetlówka energooszczędna, LED
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
                        Materiał abażura:
                    </td>
                    <td>
                        tworzywo sztuczne
                    </td>
                </tr>
                <tr>
                    <td>
                        Długość zawieszki:
                    </td>
                    <td>
                        max. 1m
                    </td>
                </tr>
                <tr>
                    <td>
                        Kolor zawieszki:
                    </td>
                    <td>
                        dowolny spośród dostępnych **
                    </td>
                </tr>
                <tr  >
                    <td>
                        Abażur wymaga składania:
                    </td>
                    <td>
                        Tak 
                    </td>
                </tr>
                <tr>
                    <td>
                        Zestaw obejmuje:
                    </td>
                    <td>
                        Abażur, zawieszka (oprawka, kabel, podsufitka)
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="offer_spec_comments">
        * Kolory abażurów: <uc:TromleLKolory runat="server" /><br />
        ** Kolory zawieszek: biała, czerwona, zielona, srebrna, niebieska, złota, czarna. Lampy z zawieszkami stalowymi dostępne są na <a href="http://allegro.pl/Shop.php/Show?category=677337&country=1&buy=0&shop=0&order=&price_from=&price_to=&listing=0&listing_sel=&listing_interval=&pay=0&redir_to_search=1&string=tromle+stalowa&id=678165" target="_blank">Naszych osobnych aukcjach</a><br />
        *** Wymaga składania: Tak (około 15 minut wg załączonej instrukcji)
    </div>
    <div class="regular_text color-r">
    W tej aukcji oferujemy lampę z abażurem o średnicy 23cm. Jeśli szukasz większego rozmiaru lampy Tromle <a href="http://allegro.pl/Shop.php/Show?category=&country=1&buy=0&shop=0&order=&price_from=&price_to=&listing=0&listing_sel=&listing_interval=&pay=0&redir_to_search=1&string=tromle+L&id=678165" target="_blank">sprawdź tutaj</a>.
    </div>
    <div class="regular_text">
        <div class="regular_text">
            Lampa Tromle dostępna jest w wielu kolorach i w wielu układach kolorystycznych.
            Poniżej prezentujemy najbardziej popularne zestawy. Na Państwa specjalne życzenie
            możemy przygotować dowolny układ kolorów spośród dostępnych (dostępność danego koloru
            opisana jest powyżej).
        </div>
        <uc:Tromle runat="server" KronosType="M" Header="Lampy" />
    </div>
    <a name="more"></a>
    <div id="more">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_czy_wiesz_ze.png" alt="Czy wiesz, że ... ?" />
        <ul>
            <li>Można zamówić jedno, dwu lub wielokolorowy abażur</li>
            <li>Można zamówić sam abażur, zobacz Nasze pozostałe aukcje</li>
            <li>Za dopłatą można zamówić stalową zawieszkę do abażura</li>
            <li>Na specjalne zamówienie, możemy przygotować dowolnej długości zawieszkę plastikową</li>
            <li>Do kompletu można zakupić: lampki nocne, stołowe, podłogowe a także kinkiety. Zobacz
                Naszą pełną ofertę</li>
        </ul>
    </div>
    <a name="suggestions"></a>
    <div id="suggestions">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_produkty_uzupelniajace.png"
            alt="Produkty uzupełniające" />
            <uc:TromleProduktyUzupelniajace runat="server" />
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
