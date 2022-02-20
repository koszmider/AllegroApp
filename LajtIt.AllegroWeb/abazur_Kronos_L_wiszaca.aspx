<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="abazur_Kronos_L_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.AbazurWiszacaKronosLPage " %>
    
<%@ Register Src="~/Controls/KronosLKolory.ascx" TagName="KronosLKolory" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace" TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
 
    <div id="offer">
        <h1 class="item_title color-r">
            Abażur Kronos</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/astrid_kronos_glowna.jpg"
                    alt="Lampa wisząca Kronos" />
                    <div class="offer_main_photo_comment">Biała lampa w aranżacji</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/kronos/www.lajtit.pl_nowoczesne_lampy_kronos.jpg"
                    alt="Lampa wisząca Kronos" />
                    <div class="offer_main_photo_comment">Lampa Kronos w aranżacji wraz z lampą podłogową <a href="http://allegro.pl/Shop.php/Show?category=&country=1&buy=0&shop=0&order=&price_from=&price_to=&listing=0&listing_sel=&listing_interval=&pay=0&redir_to_search=1&string=kronos+due&id=678165" target="_blank">Kronos Due</a></div>
            </div>
            <div class="offer_main_photo_frame" style="text-align: center">
                <table id="Table_01" width="400" height="400" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2">
                            <img src="http://www.lajtit.pl/public/assets/allegro/forma_dostawy_kronos_01.jpg"
                                width="400" height="210" alt="">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a href="http://www.youtube.com/watch?feature=player_embedded&v=H1fjpzGk4M0" target="_blank">
                                <img src="http://www.lajtit.pl/public/assets/allegro/forma_dostawy_kronos_02.jpg"
                                    width="213" height="190" border="0" alt=""></a>
                        </td>
                        <td>
                            <img src="http://www.lajtit.pl/public/assets/allegro/forma_dostawy_kronos_03.jpg"
                                width="187" height="190" alt="">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Rozmiar:
                    </td>
                    <td>
                        L
                    </td>
                </tr>
                <tr>
                    <td>
                        Wysokość:
                    </td>
                    <td>
                        35cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Średnica:
                    </td>
                    <td>
                        35cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Obwód:
                    </td>
                    <td>
                        110cm
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
                        Rodzaj używanej żarówki:
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
                        Materiał abażura:
                    </td>
                    <td>
                        tworzywo sztuczne
                    </td>
                </tr> 
                <tr title="Wymaga składania: Tak (wybierając przesyłkę: list polecony), Nie (każdy inny rodzaj przesyłki)">
                    <td>
                        Abażur wymaga składania:
                    </td>
                    <td>
                        Tak/Nie **
                    </td>
                </tr>
                <tr>
                    <td>
                        Zestaw obejmuje:
                    </td>
                    <td>
                        Abażur 
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="offer_spec_comments">
        * Kolory abażurów: <uc:KronosLKolory ID="KronosLKolory1" runat="server" /><br />
        ** Wymaga składania: Tak (wybierając przesyłkę: list polecony), Nie (każdy inny
        rodzaj przesyłki)
    </div>
 
    <div class="regular_text">
      <b>W tej aukcji oferujemy sam abażur</b>, jeśli chcą Państwo nabyć komplet: abażur z kablem zasilającym (zawieszką) <a href="http://allegro.pl/Shop.php/Show?category=677338&country=1&buy=0&shop=0&order=&price_from=&price_to=&listing=0&listing_sel=&listing_interval=&pay=0&redir_to_search=1&string=kronos&id=678165" target="_blank" style="text-decoration: underline;">zapraszamy na pozostałe nasze aukcje</a>.
        <div class="regular_text">
            Abażur Kronos dostępny jest w wielu kolorach i w wielu układach kolorystycznych.
            Poniżej prezentujemy najbardziej popularne zestawy. Na Państwa specjalne życzenie
            możemy przygotować dowolny układ kolorów spośród dostępnych (dostępność danego koloru
            opisana jest powyżej).
        </div>
        <uc:Kronos runat="server" KronosType="L" Header="Abażury" />
    </div>
    <a name="more"></a>
    <div id="more">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_czy_wiesz_ze.png" alt="Czy wiesz, że ... ?" />
        <ul>
            <li>Można zamówić jedno, dwu lub wielokolorowy abażur</li>   
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
            Zamówiony towar wysyłamy kurierem (lampy złożone) lub listem poleconym (lampy do samodzielnego złożenia). Czas dostawy przez Kuriera to 1-2 dni robocze. Poczta Polska dostarcza towar w 2-4 dni robocze.
            Zobacz pełen <a href="http://allegro.pl/my_page.php?uid=678165#jak_kupowac" target="_blank">
                proces realizacji zamówienia</a>.
        </div>
    </div>
</asp:Content>
