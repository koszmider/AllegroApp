<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_Kronos_M_stal_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaWiszacaKronosMStalPage" %>
    
<%@ Register Src="~/Controls/KronosMKolory.ascx" TagName="KronosMKolory" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="price">
        <div class="color-r">
            7,60 zł</div>
    </div>
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa wisząca Kronos (zawieszka stalowa)</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/astrid_kronos_glowna.jpg"
                    alt="Lampa wisząca Kronos" />
            </div>
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Rozmiar:
                    </td>
                    <td>
                        M
                    </td>
                </tr>
                <tr>
                    <td>
                        Wysokość:
                    </td>
                    <td>
                        23cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Średnica:
                    </td>
                    <td>
                        23cm
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
                        Zawieszka:
                    </td>
                    <td>
                        stalowa, model do wyboru **
                    </td>
                </tr>
                <tr>
                    <td>
                        Abażur wymaga składania:
                    </td>
                    <td>
                        Tak/Nie ***
                    </td>
                </tr>
                <tr>
                    <td>
                        Zestaw obejmuje:
                    </td>
                    <td>
                        Abażur, zawieszka
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="offer_spec_comments">
        * Kolory abażurów: <uc:KronosMKolory runat="server" /><br />
        ** Zawieszka: Model I (70cm), Model II (130cm, linka regulacyjna)<br />
        *** Wymaga składania: Tak (wybierając przesyłkę: list polecony), Nie (każdy inny
        rodzaj przesyłki)
    </div>
    <div class="regular_text">
        <div style="float: left;">
            <img src="http://www.lajtit.pl/public/assets/allegro/zawieszki_metalowe.jpg" alt=""
                style="margin: 20px; float: left" /></div>
        <div class="regular_text">
            Do lampy Kronos mogą Państwo wybrać jedną z dwóch stalowych zawieszek:<br /><br />
            - Model I - 70cm długości, <br />
              - Model II - 130cm długości z linką<br /><br />
            Po żłożeniu zamówienia prosimy o podanie modelu zawieszki jaki mamy dołączyć.
        </div>
    </div>
    <div style="clear: both;">
    </div>
    <div class="regular_text">
        Lampa Kronos dostępna jest w wielu kolorach i w wielu układach kolorystycznych.
        Poniżej prezentujemy najbardziej popularne zestawy. Na Państwa specjalne życzenie
        możemy przygotować dowolny układ kolorów spośród dostępnych (dostępność danego koloru
        opisana jest powyżej).
    </div>
    <div class="regular_text">
        <uc:Kronos runat="server" KronosType="XL" Header="Lampy" />
    </div>
    <a name="more"></a>
    <div id="more">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_czy_wiesz_ze.png" alt="Czy wiesz, że ... ?" />
        <ul>
            <li>Można zamówić jedno, dwu lub wielokolorowy abażur</li>
            <li>Można zamówić sam abażur, zobacz Nasze pozostałe aukcje</li>
            <li>Za dopłatą można zamówić stalową zawieszkę do abażura</li>
            <li>Na specjalne zamówienie, możemy przygotować dowolnej długości zawieszkę plastikową</li>
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
            Zamówiony towar wysyłamy kurierem (lampy złożone) lub listem poleconym (lampy do
            samodzielnego złożenia). Czas dostawy przez Kuriera to 1-2 dni robocze. Poczta Polska
            dostarcza towar w 2-4 dni robocze. Zobacz pełen <a href="http://allegro.pl/my_page.php?uid=678165#jak_kupowac"
                target="_blank">proces realizacji zamówienia</a>.
        </div>
    </div>
</asp:Content>
