<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="lampa_Kronos_nocna.aspx.cs"
    Inherits="LajtIt.AllegroWeb.LampaNocnaKronosPage" %>

<%@ Register Src="~/Controls/KronosMKolory.ascx" TagName="KronosMKolory" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa nocna Astrid</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/kronos/www.lajtit.pl_lampka_nocna_astrid_biala.jpg"
                    alt="Lampa nocna Astrid" />
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/www.magic-lams.pl-lampki_nocne_Astrid_1.jpg"
                    alt="Lampa nocna Astrid" />
                <div class="offer_main_photo_comment">
                    Lampki nocne w aranżacji</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/kronos/www.lajtit.pl_modne_lampy_kronos_nocna_podlogowa_2.jpg"
                    alt="Lampa podłogowa Kronos" />
                <div class="offer_main_photo_comment">
                    <a href="http://allegro.pl/listing/user.php?string=Piękna+lampa+podłogowa&us_id=678165"
                        target="_blank">Lampa podłogowa Kronos</a> oraz <a href="http://allegro.pl/listing/user.php?string=nocne+lampki+astrid&us_id=678165"
                            target="_blank">nocna Astrid</a> w aranżacji</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/kronos/www.lajtit.pl_lampa_nocna_kronos_elegance_trzy_kolory.jpg"
                    alt="Lampa podłogowa Kronos" />
                <div class="offer_main_photo_comment">Polecamy również lampki nocne Kronos Elegance z podstawkami w kolorze białym, czarnym oraz niklowane
                    <a href="http://allegro.pl/listing/user/listing.php?us_id=678165&string=nocne+kronos+elegance&search_scope=userItems-678165"
                        target="_blank">Kliknij tutaj i sprawdź</a></div>
            </div>
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Wysokość:
                    </td>
                    <td>
                        60cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Średnica abażura:
                    </td>
                    <td>
                        23cm
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
                        stal proszkowana
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
        * Kolory abażurów:
        <uc:KronosMKolory ID="KronosMKolory1" runat="server" />
        <br />
    </div>
    <div class="regular_text">
        <div class="regular_text">
            Abażur lampy Kronos dostępny jest w wielu kolorach i w wielu układach kolorystycznych.
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
