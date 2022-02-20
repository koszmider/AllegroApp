<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_szklana_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaWiszacaSzklanaPage" %>

<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
        <h1 class="item_title color-r">
            Klasyczna lampa wisząca</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/lampa_szklana_1.jpg" alt="Lampa wisząca" />
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/lampa_szklana_2.jpg" alt="Lampa wisząca " />
            </div> 
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Średnica:
                    </td>
                    <td>
                        30cm (<a href="http://allegro.pl/Shop.php/Show?category=&country=1&buy=0&shop=0&order=&price_from=&price_to=&listing=0&listing_sel=&listing_interval=&pay=0&redir_to_search=1&string=mro%C5%BCone&id=678165" target="_blank">dostępne są też lampy o mniejszej średnicy 23cm, sprawdź tutaj</a>)
                    </td>
                </tr>
                <tr>
                    <td>
                        Wysokość:
                    </td>
                    <td>
                        12cm
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
                        Materiał klosz:
                    </td>
                    <td>
                       ręcznie formowane szkło
                    </td>
                </tr>
                <tr>
                    <td>
                        Długość zawieszki:
                    </td>
                    <td>
                        max. 1m (regulowana długość)
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
                        Klosz, zawieszka (oprawka, kabel, podsufitka)
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <a name="suggestions"></a>
    <div id="suggestions">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_produkty_uzupelniajace.png"
            alt="Produkty uzupełniające" />
                 <div class="regular_text">Polecamy również:<br /><br />
                <table style="border: solid 1px black;">
                    <tr>
                        <td>
                            <a href="http://allegro.pl/Shop.php/Show?category=&country=1&buy=0&shop=0&order=&price_from=&price_to=&listing=0&listing_sel=&listing_interval=&pay=0&redir_to_search=1&string=spark&id=678165"
                                target="_blank">
                                <img src="http://www.lajtit.pl/public/assets/allegro/spark.jpg" style="border: 0px;
                                    width: 150px" /></a>
                        </td>
                        <td>Materiałowa<br />
                            Cena: 13,99 zł
                        </td>
                      
                        <td>
                            <a href="http://allegro.pl/listing/user/listing.php?us_id=678165&string=lampa+28&search_scope=userItems-678165"
                                target="_blank">
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit.pl_lampa_plastikowa.jpg" style="border: 0px;
                                    width: 150px" /></a>
                        </td>
                        <td>Plastikowa<br />Średnica 28cm<br />
                            Cena: 13,99 zł
                        </td>
                      
                        <td>
                            <a href="http://allegro.pl/listing/user/listing.php?us_id=678165&string=szklana&search_scope=userItems-678165&price_from=14&price_to=15&price_enabled=1"
                                target="_blank">
                                <img src="http://www.lajtit.pl/public/assets/allegro/lampa_szklana_mala_1.jpg" style="border: 0px;
                                    width: 150px" /></a>
                        </td>
                        <td>Szklana<br />Średnica 23cm<br />
                            Cena: 14,99 zł
                        </td>
                      
                    </tr>
                </table>
                </div>

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
