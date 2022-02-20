<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="lampa_Kronos_podlogowa.aspx.cs"
    Inherits="LajtIt.AllegroWeb.LampaPodlogowaKronosPage " %>

<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <%--    <div class="price">
        <div class="color-r">
            5,40 zł</div>
    </div>--%>
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa podłogowa Kronos</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/userdata/gfx/04d1710b4c258245329f17107f25f113.jpg"
                    style="width: 400px;" alt="Lampa podłogowa Kronos" />
                <div class="offer_main_photo_comment">
                    Biała lampa w aranżacji</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/kronos/www.lajtit.pl_modne_lampy_kronos_nocna_podlogowa.jpg"
                    alt="Lampa podłogowa Kronos" />
                <div class="offer_main_photo_comment">
                    Lampa podłogowa Kronos oraz <a href="http://allegro.pl/listing/user.php?string=nocne+lampki+astrid&us_id=678165"
                        target="_blank">nocna Astrid</a> w aranżacji</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/kronos/www.lajtit.pl_modne_lampy_kronos_nocna_podlogowa_2.jpg"
                    alt="Lampa podłogowa Kronos" />
                <div class="offer_main_photo_comment">
                    Lampa podłogowa Kronos oraz <a href="http://allegro.pl/listing/user.php?string=nocne+lampki+astrid&us_id=678165"
                        target="_blank">nocna Astrid</a> w aranżacji</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/www.lajtit.pl_lampa_podlogowa_kronos.gif"
                    alt="Lampa podłogowa Kronos" />
                <div class="offer_main_photo_comment">
                    Możliwość regulowania wysokości</div>
            </div>
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Wysokość:
                    </td>
                    <td>
                        max 196cm; pięć możliwych wysokości: 62cm, 96cm, 130cm, 163cm, 196cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Średnica abażura:
                    </td>
                    <td>
                        35cm
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
                        3m
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
                        Średnica podstawy:
                    </td>
                    <td>
                        26 cm
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
        * Kolory abażurów: biały, biały półprzezroczysty, niebieski, żółty, czarny, czerwony,
        czerwony półprzezroczysty, zielony, srebrny, fioletowy, limonkowy, różowy<br />
    </div>
    <div class="regular_text">
        <div class="regular_text">
            Abażury lampy Kronos dostępne są w wielu kolorach i w wielu układach kolorystycznych.
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
    <div>
        <a href="http://allegro.pl/sklep/678165_lajt-it-design-you-like?category=850702&amp;id=678165"
            target="_blank">
            <img src="http://static.lajtit.pl/lajtit_exclusive_banner.jpg" style="border: 0"></a>
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
