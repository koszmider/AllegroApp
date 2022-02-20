<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_Kronos_Trio_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaWiszacaKronosTrioPage" %>
    
<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosMKolory.ascx" TagName="KronosMKolory" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace" TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <%--<div class="price">
        <div class="color-r">
            5,40 zł</div>
    </div>--%>
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa wisząca Kronos Trio</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/kronos/www.lajtit.pl_lampa_kronos_trio.jpg"
                    alt="Lampa wisząca Kronos Trio" />
                    
                    <div class="offer_main_photo_comment">Abażury w kolorze białym, po podświetleniu nieznacznie zmieniają barwę</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/kronos/www.lajtit.pl_lampa_Kronos_trio_kolory.jpg"
                    alt="Lampa wisząca Kronos Trio" />
                    
                    <div class="offer_main_photo_comment">Lampy z plafonami w kolorach srebrnym, czarnym i białym (inne kolory dostępne na zamówienie)</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/kronos_trio/www.lajtit.pl_lampa_wiszaca_kronos_trio_od_dolu.jpg"
                    alt="Lampa wisząca Kronos Trio" />
                    
                    <div class="offer_main_photo_comment">Widok od dołu</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/kronos_trio/www.lajtit.pl_lampa_wiszaca_kronos_trio_od_dolu_z_boku.jpg"
                    alt="Lampa wisząca Kronos Trio" />
                    
                    <div class="offer_main_photo_comment"></div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/kronos_trio/www.lajtit.pl_lampa_wiszaca_kronos_trio_aranzacja.jpg"
                    alt="Lampa wisząca Kronos Trio" />
                    
                    <div class="offer_main_photo_comment">Wysokość zawieszenia każdego z abażurów można regulować oddzielnie</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/kronos_trio/www.lajtit.pl_lampa_wiszaca_kronos_trio2.jpg"
                    alt="Lampa wisząca Kronos Trio" />
                    
                    <div class="offer_main_photo_comment">Polecamy również: <a href="http://allegro.pl/listing/user.php?string=kronos+trio&us_id=678165" target="_blank">Kronos Trio w wersji z podłużnym plafonem. Kliknij tutaj i dowiedz się więcej</a>.<br />Ta lampa oferowana jest na innych naszych aukcjach.</div>
            </div>
        </div>
        <div class="offer_spec">
            <table> 
                <tr>
                    <td>
                        Abażur średnica:
                    </td>
                    <td>
                        23cm
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
                        metalowy, średnica 34cm, kolor: srebrny, biały, czarny (inne na zamówienie) **
                    </td>
                </tr>
                <tr>
                    <td>
                        Wysokość lampy:
                    </td>
                    <td>
                        max 120cm, regulowana długość każdego zwisu osobno
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
        <a name="abazur_kolor"></a>* Kolory abażurów: <uc:KronosMKolory ID="KronosMKolory1" runat="server" /> <br />
        <a name="plafon_kolor"></a>** Plafon kolor: stadnardowo srebrny. Na specjalne zamówienie istnieje możliwość przygotowania innego koloru. Prosimy o kontakt by uzyskać więcej informacji.  </div>
 
    <div class="regular_text">
      
        <div class="regular_text">
            Abażury Kronos dostępne są w wielu kolorach i w wielu układach kolorystycznych.
            Poniżej prezentujemy najbardziej popularne zestawy. Na Państwa specjalne życzenie
            możemy przygotować dowolny układ kolorów spośród dostępnych (dostępność danego koloru
            opisana jest powyżej).
        </div>
        <uc:Kronos ID="Kronos1" runat="server" KronosType="L" Header="Abażury" />
    </div>
   
    <a name="more"></a>
    <div id="more">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_czy_wiesz_ze.png" alt="Czy wiesz, że ... ?" />
        <ul>
            <li>Można zamówić jedno, dwu lub wielokolorowy abażur</li>
            <li>Można zamówić sam abażur, zobacz Nasze pozostałe aukcje</li> 
            <li>Na specjalne zamówienie, możemy przygotować dowolnej długości kable oraz kolor plafona</li>
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
<a href="http://allegro.pl/sklep/678165_lajt-it-design-you-like?category=850702&amp;id=678165" target="_blank"><img src="http://static.lajtit.pl/lajtit_exclusive_banner.jpg" style="border:0"></a>

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
