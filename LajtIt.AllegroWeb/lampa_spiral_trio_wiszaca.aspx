<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_spiral_trio_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaWiszacaSpiralTrioPage" %>
    
<%@ Register Src="~/Controls/LampyOfertaKolory.ascx" TagName="LampyOfertaKolory"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa wisząca Spiral Trio</h1>
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
                <img src="http://www.lajtit.pl/public/assets/spiral/www.lajtit.pl_lampa_wiszaca_spiral_trio.jpg"
                    alt="Lampa wisząca Spiral" />
                <div class="offer_main_photo_comment">
                    Przykładowe kolory abażurów, możliwść wyboru dowolnego koloru spośród dostępnych</div>
            </div>
        </div>
        <div class="offer_spec">
            <table>
            <colgroup>
            <col width="100" />
            <col />
            </colgroup>
                <tr>
                    <td>
                        Wysokość:
                    </td>
                    <td>
                        max 1m, min 70cm
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
                        Obwód abażura:
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
                        3 x E27
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
                        dowolny spośród dostępnych <a href="#abazur_kolor">zobacz</a>*
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
                        Plafon:
                    </td>
                    <td>
                        metalowy, <a href="#plafon_kolor">kolor srebrny</a> **, średnica 34cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Zawieszka:
                    </td>
                    <td>
                        max 70cm, półprzezroczysty, regulowana długość każdego kabla osobno
                    </td>
                </tr> 
                <tr>
                    <td>
                        Zestaw obejmuje:
                    </td>
                    <td>
                          3 x abażur, plafon, okablowanie
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="offer_spec_comments">
        <a name="abazur_kolor"></a>* Kolory abażurów: biały, czarny, żółty, czerwony, zielony<br /><br />   
          <a name="plafon_kolor"></a>** Plafon kolor: stadnardowo srebrny. Na specjalne zamówienie istnieje możliwość przygotowania innego koloru. Prosimy o kontakt by uzyskać więcej informacji.
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
