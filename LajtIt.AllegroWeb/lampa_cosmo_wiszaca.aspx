<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_cosmo_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaWiszacaCosmoPage" %>
    
<%@ Register Src="~/Controls/LampyOfertaKolory.ascx" TagName="LampyOfertaKolory"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa wisząca Cosmo</h1>
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
                <img src="http://www.lajtit.pl/public/assets/allegro/www.magic-lamps.pl_lampy_salon_cosmo.jpg"
                    alt="Lampa wisząca Cosmo" />
            </div>
      <%--      <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/cosmo/www.lajtit.pl_lampa_wiszaca_cosmo_trio.jpg"
                    alt="Lampa wisząca Kronos" />
                    <div class="offer_main_photo_comment">Polecamy również lampę Cosmo Trio (wiele kolorów abażurów) dostępną na naszych osobnych aukcjach. <a href="http://allegro.pl/listing/user/listing.php?us_id=678165&string=cosmo+trio&search_scope=userItems-678165" target="_blank">Kliknij tutaj i sprawdź</a></div>
            </div> --%>
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Wysokość:
                    </td>
                    <td>
                        25cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Szerokość:
                    </td>
                    <td>
                        25cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Obwód:
                    </td>
                    <td>
                        75cm
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
                        Tak
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
    </div>
    <div class="offer_spec_comments">
        * Kolory abażurów: biały, biały półprzezroczysty, czerwony, zielony, fioletowy, limonkowy (brak), 
        niebieski, czarny, żółty, granatowy, różowy (brak), pomarańczowy, srebrny<br />
        ** Kolory zawieszek: biała, czerwona, zielona, srebrna, niebieska, złota, czarna
    </div>
    <uc:LampyOfertaKolory ID="LampyOfertaKolory1" runat="server" />
    <div class="regular_text">
        Lampa Cosmo dostępna jest w wielu kolorach i w wielu układach kolorystycznych. Poniżej
        prezentujemy najbardziej popularne zestawy. Na Państwa specjalne życzenie możemy
        przygotować dowolny układ kolorów spośród dostępnych (dostępność danego koloru opisana
        jest powyżej).<br />
        <br />
        <table style="width:100%;">
            <tr valign="top">
                <td style="text-align:left;">
                    <img src="http://www.lajtit.pl/public/assets/allegro/www.magic-lamps.pl_lampy_kolory_cosmo.jpg"
                        alt="Lampy wiszące Cosmo" />
                </td>
                <td style="text-align:right;">
                    <table class="border">
                        <tr>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_biale.jpg" alt="Lampy wiszące Cosmo"
                                    width="120px" />
                            </td>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_czarne.jpg" alt="Lampy wiszące Cosmo"
                                    width="120px" />
                            </td>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_fioletowe.jpg"
                                    alt="Lampy wiszące Cosmo" width="120px" />
                            </td>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_niebieskie.jpg"
                                    alt="Lampy wiszące Cosmo" width="120px" />
                            </td>
                        </tr>
                        <tr style="text-align: center;">
                            <td>
                                Biały
                            </td>
                            <td>
                                Czarny
                            </td>
                            <td>
                                Fioletowy
                            </td>
                            <td>
                                Niebieski
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_zolte.jpg" alt="Lampy wiszące Cosmo"
                                    width="120px" />
                            </td>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_granatowy.jpg" alt="Lampy wiszące Cosmo"
                                    width="120px" />
                            </td>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_czerwone.jpg" alt="Lampy wiszące Cosmo"
                                    width="120px" />
                            </td>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_zielone.jpg" alt="Lampy wiszące Cosmo"
                                    width="120px" />
                            </td>
                        </tr>
                        <tr style="text-align: center;">
                            <td>
                                Żółty
                            </td>
                            <td>
                                Granatowy
                            </td>
                            <td>
                                Czerwony
                            </td>
                            <td>
                                Zielony
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_srebrne.jpg" alt="Lampy wiszące Cosmo"
                                    width="120px" />
                                 
                            </td>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_rozowe.jpg" alt="Lampy wiszące Cosmo"
                                    width="120px" />
                            </td>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_pomaranczowe.jpg"
                                    alt="Lampy wiszące Cosmo" width="120px" />
                            </td>
                            <td>
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_cosmo_limonkowe.jpg"
                                    alt="Lampy wiszące Cosmo" width="120px" />
                                 
                            </td>
                        </tr>
                        <tr style="text-align: center;">
                            <td>Srebrny 
                            </td>
                            <td>
                                Różowy (brak)
                            </td>
                            <td>
                                Pomarańczowy
                            </td>
                            <td>
                                Limonkowy (brak)
                            </td>
                        </tr>
                    </table>
                </td>
        </table><br /><br />
        <table style="width: 100%">
            <tr style="text-align: left;">
                <td style="width: 50%">
                    <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_aranzacja_zolty.jpg"
                        alt="Lampy wiszące Cosmo" width="430px" />
                </td>
                <td style="text-align: right;">
                    <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_aranzacja_zielony.jpg"
                        alt="Lampy wiszące Cosmo" width="430px" />
                </td>
            </tr>
        </table>
    </div>

    

    <a name="suggestions"></a>
    <div id="suggestions">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_produkty_uzupelniajace.png" alt="Produkty uzupełniające" />
        
     <a href="http://allegro.pl/listing/user.php?string=cosmo&us_id=678165" target="_blank"><img src="http://www.lajtit.pl/public/assets/allegro/lajtit_produkty_uzupelniajace_cosmo.png" alt="Strona otworzy się w nowym oknie" /></a>
    </div>


    <a name="more"></a>
    <div id="more">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_czy_wiesz_ze.png" alt="Czy wiesz, że ... ?" />
        
     <ul>
     <li>Na specjalne zamówienie, możemy przygotować dowolnej długości zawieszkę</li>
     <li>Za dopłatą, dostępne są metalowe zawieszki</li>
     <li>Na naszych aukcjach możesz nabyć sam abażur (bez kabla zasilającego)</li>
     <li>Możesz skomponować wielokolorowy abażur</li>
     </ul>
    </div>


    <a name="wysylka"></a>
    <div id="shipping">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_wysylka.png" alt="Wysyłka" />
        <div class="regular_text">
            Zamówiony towar wysyłamy kurierem lub Pocztą Polską (listy polecone). Czas dostawy
            przez Kuriera to 1-2 dni robocze. Poczta Polska dostarcza towar w ciągu 2-4 dni
            roboczych. Zobacz pełen <a href="http://allegro.pl/my_page.php?uid=678165#jak_kupowac"
                target="_blank">proces realizacji zamówienia</a>.
        </div>
    </div>
</asp:Content>
