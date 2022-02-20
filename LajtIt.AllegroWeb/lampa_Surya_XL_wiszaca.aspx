<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_Surya_XL_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaWiszacaSuryaXLPage" %>
    
<%@ Register Src="~/Controls/LampyOfertaKolory.ascx" TagName="LampyOfertaKolory"
    TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosLKolory.ascx" TagName="KronosMKolory" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<%--<div class="price">
        <div class="color-r">
            11,40 zł</div>
    </div>--%>

    <div id="offer">
        <h1 class="item_title color-r">
            Lampa wisząca Surya</h1>
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
                <img src="http://www.lajtit.pl/public/assets/allegro/surya/www.lajtit.pl_modna_lampa_surya_XXL_13.jpg"
                    alt="Lampa wisząca Kronos" />
                <div class="offer_main_photo_comment">
                    Biała lampa w aranżacji</div>
            </div>
            <div class="offer_main_photo_frame">
                <div style="float: left; padding:2px;">
                    <img style="width: 180px" src="http://www.lajtit.pl/public/assets/allegro/surya/www.lajtit.pl_modna_lampa_surya_XXL_06.jpg"
                        alt="Lampa wisząca Kronos" />
                    <div class="offer_main_photo_comment">
                        Biała lampa w aranżacji</div>
                </div>
                <div style="float: left;">
                    <img style="width: 180px; padding:2px;" src="http://www.lajtit.pl/public/assets/allegro/surya/www.lajtit.pl_modna_lampa_surya_XXL_01.jpg"
                        alt="Lampa wisząca Kronos" />
                    <div class="offer_main_photo_comment">
                        Biała lampa w aranżacji</div>
                </div>
                <div>
                    <img style="width: 180px; padding:2px;" src="http://www.lajtit.pl/public/assets/allegro/surya/www.lajtit.pl_modna_lampa_surya_XXL_07.jpg"
                        alt="Lampa wisząca Kronos" />
                    <div class="offer_main_photo_comment">
                        Biała lampa w aranżacji</div>
                </div>
        </div>
    </div>
    <div class="offer_spec">
        <table>
            <tr>
                <td>
                    Rozmiar:
                </td>
                <td>
                    XL
                </td>
            </tr>
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
                    Średnica:
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
                    Abażur, zawieszka (oprawka, kabel, podsufitka)
                </td>
            </tr>
        </table>
    </div>
    </div>
    <div class="offer_spec_comments">
        * Kolory abażurów: <uc:KronosMKolory runat="server" /><br />
        ** Kolory zawieszek: biała, czerwona, zielona, srebrna, niebieska, złota, czarna<br />
    
    </div>
    <uc:LampyOfertaKolory ID="LampyOfertaKolory1" runat="server" />
    <div class="regular_text">
        <div style="float: left; padding: 10px">
            <img src="http://www.lajtit.pl/public/assets/allegro/magic_lamps_duza_piekna_kula_lampy_surya.jpg"
                alt="" /></div>
        <div class="regular_text">
            Lampa Surya dostępna jest w wielu kolorach i w wielu układach kolorystycznych. Poniżej
            prezentujemy najbardziej popularne zestawy. Na Państwa specjalne życzenie możemy
            przygotować dowolny układ kolorów spośród dostępnych (dostępność danego koloru opisana
            jest powyżej).
        </div>
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
            Zamówiony towar wysyłamy kurierem. Czas dostawy przez Kuriera to 1-2 dni robocze. Zobacz pełen <a href="http://allegro.pl/my_page.php?uid=678165#jak_kupowac"
                target="_blank">proces realizacji zamówienia</a>.
        </div>
    </div>
</asp:Content>
