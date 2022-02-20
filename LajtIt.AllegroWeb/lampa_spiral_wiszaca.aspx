<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_spiral_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaWiszacaSpiralPage" %>
    
<%@ Register Src="~/Controls/LampyOfertaKolory.ascx" TagName="LampyOfertaKolory"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa wisząca Spiral</h1>
        <div class="offer_main_photo">
            <asp:Panel runat="server" ID="pPhotoColor" Visible="false">
                <div class="offer_main_photo_frame">
                    <asp:Image runat="server" ID="imgColor" />
                </div>
                <div class="offer_main_photo_frame" runat="server" id="divPhoto2" visible="false">
                    <asp:Image runat="server" ID="imgColor2" />
                </div>
            </asp:Panel>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/www.magic-lamps.pl_Spiral_aranzacja.jpg"
                    alt="Lampa wisząca Spiral" />
                    <div class="offer_main_photo_comment">Lampa Spiral na pojedyńczym zwisie, dostępna w tej aukcji</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/spiral/www.lajtit.pl_lampa_wiszaca_spiral_trio.jpg"
                    alt="Lampa wisząca Spiral Trio" />
                    <div class="offer_main_photo_comment">Lampa Spiral Trio, różne kolory, <a href="http://allegro.pl/listing/user.php?string=spiral+trio&us_id=678165" target="_blank">kliknij tutaj i zobacz więcej</a>. Lampa Spiral Trio dostępna jest na naszych osobnych aukcjach.</div>
            </div>
        </div>
        <div class="offer_spec">
            <table>
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
                        max 1m, regulowana długość
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
                <tr>
                    <td>
                        Wymaga składania:
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
                        Abażur, zawieszka (oprawka, kabel, podsufitka)
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="offer_spec_comments">
        * Kolory abażurów: biały, czarny, żółty, czerwony, zielony<br />
        ** Kolory zawieszek: biała, czerwona, zielona, srebrna, niebieska, złota, czarna
    </div>
    <uc:LampyOfertaKolory ID="LampyOfertaKolory1" runat="server" />
    <div class="regular_text">
        Lampa Spiral dostępna jest w wielu kolorach i w wielu układach kolorystycznych.
        Poniżej prezentujemy najbardziej popularne zestawy. Na Państwa specjalne życzenie
        możemy przygotować dowolny układ kolorów spośród dostępnych (dostępność danego koloru
        opisana jest powyżej).<br />
        <br />
        <div style="text-align: center;">
            <img src="http://www.lajtit.pl/public/assets/allegro/www.magic-lamps.pl_Spiral_wszystkie.jpg"
                alt="Lampy wiszące Spiral" />
        </div>
    </div>
    <a name="suggestions"></a>
    <div id="suggestions">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_produkty_uzupelniajace.png"
            alt="Produkty uzupełniające" />
        <a href="http://allegro.pl/listing/user.php?string=Spiral&us_id=678165" target="_blank">
            <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_produkty_uzupelniajace_Spiral.png"
                alt="Strona otworzy się w nowym oknie" /></a>
    </div>
    <a name="more"></a>
    <div id="more">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_czy_wiesz_ze.png" alt="Czy wiesz, że ... ?" />
        <ul>
            <li>Na specjalne zamówienie, możemy przygotować dowolnej długości zawieszkę</li>
            <li>Możesz skomponować wielokolorowy abażur</li>
        </ul>
    </div>
    <a name="wysylka"></a>
    <div id="shipping">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_wysylka.png" alt="Wysyłka" />
        <div class="regular_text">
            Zamówiony towar wysyłamy kurierem.Czas dostawy przez Kuriera to 1-2 dni robocze.
            Zobacz pełen <a href="http://allegro.pl/my_page.php?uid=678165#jak_kupowac" target="_blank">
                proces realizacji zamówienia</a>.
        </div>
    </div>
</asp:Content>
