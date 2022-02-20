<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_stiga_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.StigaWiszacaPage" %>
    
<%@ Register Src="~/Controls/Stiga.ascx" TagName="Stiga" TagPrefix="uc" %>
<%@ Register Src="~/Controls/StigaKolory.ascx" TagName="StigaKolory" TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa wisząca Stiga</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/Stiga/www.lajtit.pl_lampa_wiszaca_stiga_zielona.jpg"
                    alt="Lampa wisząca Stiga" />
                <div class="offer_main_photo_comment">
                    Lampa biało - zielona</div>
            </div> 
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/Stiga/www.lajtit.pl_lampa_wiszaca_stiga_fioletowa.jpg"
                    alt="Lampa wisząca Stiga" />
                <div class="offer_main_photo_comment">
                    Lampa biało - fioletowa</div>
            </div>
        </div>
        <div class="offer_spec">
            <table>
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
                        Szerokość:
                    </td>
                    <td>
                        60cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Obwód:
                    </td>
                    <td>
                        190cm
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
                <tr>
                    <td>
                        Wymaga składania:
                    </td>
                    <td>
                        Tak ***
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
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
        <div style="clear: left; height: 10px;">
        </div>
    </div>
    <div class="offer_spec_comments">
        * Kolory abażurów:
        <uc:StigaKolory runat="server" />
        <br />
        ** Kolory zawieszek: biała, czerwona, zielona, srebrna, niebieska, złota, czarna
        <br />
        *** Wymaga składania: Tak, ok 5 minut wg załączonej instrukcji</div>
    <div class="regular_text">
        Lampa Stiga dostępna jest w wielu kolorach i w wielu układach kolorystycznych. Poniżej
        prezentujemy najbardziej popularne zestawy. Na Państwa specjalne życzenie możemy
        przygotować dowolny układ kolorów spośród dostępnych.
    </div>
    <div class="regular_text">
    <uc:Stiga runat="server" />
    </div>
    <a name="suggestions"></a>
    <div id="suggestions">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_produkty_uzupelniajace.png"
            alt="Produkty uzupełniające" />
        <a href="http://allegro.pl/listing/user.php?string=stiga&us_id=678165" target="_blank">
            <img src="http://www.lajtit.pl/public/assets/allegro/Stiga/www.lajtit.pl_produkty_uzupelniajace_Stiga.png"
                alt="Strona otworzy się w nowym oknie" /></a>
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
