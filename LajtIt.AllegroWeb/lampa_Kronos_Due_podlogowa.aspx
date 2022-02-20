<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="lampa_Kronos_Due_podlogowa.aspx.cs"
    Inherits="LajtIt.AllegroWeb.LampaPodlogowaKronosDuePage" %>

<%@ Register Src="~/Controls/KronosMKolory.ascx" TagName="KronosLKolory" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa podłogowa Kronos Due</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/userdata/gfx/89bc5fd72a679fa1aba708a104c43593.jpg"
                    alt="Lampa podłogowa Kronos Due" />
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/kronos/www.lajtit.pl_nowoczesne_lampy_kronos.jpg"
                    alt="Lampa wisząca Kronos" />
                <div class="offer_main_photo_comment">
                    Lampa <a href="http://allegro.pl/Shop.php/Show?category=677338&country=1&buy=0&shop=0&order=&price_from=&price_to=&listing=0&listing_sel=&listing_interval=&pay=0&redir_to_search=1&string=kronos&id=678165"
                        target="_blank">wisząca Kronos</a> w aranżacji wraz z lampą podłogową <a href="http://allegro.pl/Shop.php/Show?category=&country=1&buy=0&shop=0&order=&price_from=&price_to=&listing=0&listing_sel=&listing_interval=&pay=0&redir_to_search=1&string=kronos+due&id=678165"
                            target="_blank">Kronos Due</a></div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/kronos/www.lajtit.pl_lampa_podlogowa_kronos_due_biala.jpg"
                    alt="Lampa wisząca Kronos" />
                <div class="offer_main_photo_comment">
                    Lampa Kronos Due - biała</div>
            </div>
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td style="width: 100px">
                        Wysokość:
                    </td>
                    <td>
                        max 200cm (możliwość regulacji w 4 zakresach: 200cm, 167cm, 134cm, 100cm)
                    </td>
                </tr>
                <tr>
                    <td>
                        Średnica abażurów:
                    </td>
                    <td>
                        23cm i 35cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Liczba uchwytów na żarówki:
                    </td>
                    <td>
                        1 x E27, 1 x E14
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
                        * dowolny spośród dostępnych (<a href="#kolor">zobacz</a>)
                    </td>
                </tr>
                <tr>
                    <td>
                        Kolor podstawki:
                    </td>
                    <td>
                        biały lub czarny
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
                        osobny do obu kloszy
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
                        2x abażur, podstawa
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="offer_spec_comments">
        <a name="kolor"></a>* Kolory abażurów:
        <uc:KronosLKolory ID="KronosLKolory1" runat="server" />
        <br />
        ** Kolory podstawek: biała, czarna<br />
    </div>
    <div class="regular_text">
        <div class="regular_text">
            Abażury lampy Kronos Due dostępne są w wielu kolorach i w wielu układach kolorystycznych.
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
