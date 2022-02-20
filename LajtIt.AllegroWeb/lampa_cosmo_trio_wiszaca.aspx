<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_cosmo_trio_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaWiszacaCosmoTrioPage" %>
    
<%@ Register Src="~/Controls/LampyOfertaKolory.ascx" TagName="LampyOfertaKolory"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa wisząca Cosmo Trio</h1>
        <div class="offer_main_photo">
      
            <div class="offer_main_photo_frame">
                <img src="http://static.lajtit.pl/ProductCatalog/a9c66949-2d82-4394-86a6-03ffcaa9a04b.jpg"
                    alt="Lampa wisząca Cosmo Trio" />
                    <div class="offer_main_photo_comment">Cosmo Trio - białe abażury i biały plafon</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://static.lajtit.pl/ProductCatalog/a5a351a1-39e7-4a49-bcf4-53035882dd80.jpg"
                    alt="Lampa wisząca Cosmo Trio" />
                    <div class="offer_main_photo_comment">Cosmo Trio - srebrne abażury i srebrny plafon</div>
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
                    <td style="width:100px;">
                        Abażur średnica:
                    </td>
                    <td>
                        25cm
                    </td>
                </tr> 
                <tr>
                    <td>
                        Liczba uchwytów na żarówki :
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
                        metalowy, kolor srebrny, biały, czarny lub złoty (inne kolory na zamówienie), średnica 34cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Wysokość lampy
                    </td>
                    <td>
                        max 110cm, regulowana długość każdego kabla osobno
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
                <tr><td>Abażur wymaga składania</td><td>Tak, wg załączonej instrukcji</td></tr>
            </table>
        </div>
    </div>
    <div class="offer_spec_comments">
        <a name="abazur_kolor"></a>* Kolory abażurów: biały, czerwony, zielony, fioletowy, limonkowy, 
        niebieski, czarny, żółty, granatowy, różowy, pomarańczowy, srebrny<br />
        ** Kolory plafonów: srebrny, biały, czarny, złoty (inne kolory na zamówienie, prosimy o kontakt mailowy w celu potwierdzenia)
    </div>
    <uc:LampyOfertaKolory ID="LampyOfertaKolory1" runat="server" />
    <div class="regular_text">
        Lampa Cosmo Trio dostępna jest w wielu kolorach i w wielu układach kolorystycznych. Poniżej
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
                                Różowy 
                            </td>
                            <td>
                                Pomarańczowy
                            </td>
                            <td>
                                Limonkowy 
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

    <div>
<a href="http://allegro.pl/sklep/678165_lajt-it-design-you-like?category=850702&amp;id=678165" target="_blank"><img src="http://static.lajtit.pl/lajtit_exclusive_banner.jpg" style="border:0"></a>

</div>
    <a name="more"></a>
    <div id="more">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_czy_wiesz_ze.png" alt="Czy wiesz, że ... ?" />
        
     <ul>
     <li>Na specjalne zamówienie, możemy przygotować dowolnej długości zawieszkę i kolor plafonu</li>
 
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
