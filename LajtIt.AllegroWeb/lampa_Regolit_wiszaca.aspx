<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_Regolit_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaWiszacaRegolitPage" %>

<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa wisząca Regolit</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/regolit.jpg" alt="Lampa wisząca Regolit" />
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/regolit1.jpg" alt="Lampa wisząca Regolit" />
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/regolit2.jpg" alt="Lampa wisząca Regolit" />
            </div>
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Średnica:
                    </td>
                    <td>
                        73cm
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
                        Max. moc:
                    </td>
                    <td>
                        60W
                    </td>
                </tr>
                <tr>
                    <td>
                        Kolor abażura:
                    </td>
                    <td>
                        biały
                    </td>
                </tr>
                <tr>
                    <td>
                        Materiał abażura:
                    </td>
                    <td>
                        papier ryżowy
                    </td>
                </tr>
                <tr>
                    <td>
                        Długość zawieszki:
                    </td>
                    <td>
                        1m
                    </td>
                </tr>
                <tr>
                    <td>
                        Kolor zawieszki:
                    </td>
                    <td>
                        biały
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
            Zamówiony towar wysyłamy kurierem. Czas dostawy przez Kuriera to 1-2 dni robocze.  Zobacz pełen <a href="http://allegro.pl/my_page.php?uid=678165#jak_kupowac"
                target="_blank">proces realizacji zamówienia</a>.
        </div>
    </div>
</asp:Content>
