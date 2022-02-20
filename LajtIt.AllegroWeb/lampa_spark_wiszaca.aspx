<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_spark_wiszaca.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaWiszacaSparkPage" %>

<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa wisząca Spark</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/spark.jpg" alt="Lampa wisząca Spark" />
                    <div class="offer_main_photo_comment">Biała lampa</div>
            </div> 
<%--            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/spark/www.lajtit.pl_modna_lampa_wiszaca_spark_aranzacja.jpg" alt="Lampa wisząca Spark" />
                    <div class="offer_main_photo_comment">Białe lampki Spark w aranżacji</div>
            </div> --%>
        </div>
        <div class="offer_spec">
       <%-- <div class="color-r" style="font-size:12px; text-align:center;">
        Lampa oferowana w dwóch rozmiarach.<br />Cena ta sama!</div>--%>
        
       <%-- <h2 class="  color-r">
             Spark M</h2>
            <table>
                <tr>
                    <td>
                        Średnica klosza: 
                    </td>
                    <td>
                        14cm (dół) / 9cm (góra)
                    </td>
                </tr>
                <tr>
                    <td>
                        Wysokość klosza:
                    </td>
                    <td>
                        15cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Liczba uchwytów na żarówki:
                    </td>
                    <td>
                        1 x E14
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
                        40W
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
                       70% poliester, 30% bawełna
                    </td>
                </tr>
                <tr>
                    <td>
                        Długość zawieszki:
                    </td>
                    <td>
                        max. 70cm (regulowana długość)
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
            </table>--%>
<%--        <h2 class="  color-r">
             Spark L</h2>--%>
            <table>
                <tr>
                    <td>
                        Średnica klosza: 
                    </td>
                    <td>
                        23cm (dół) / 13cm (góra)
                    </td>
                </tr>
                <tr>
                    <td>
                        Wysokość klosza:
                    </td>
                    <td>
                        22cm
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
                       70% poliester, 30% bawełna
                    </td>
                </tr>
                <tr>
                    <td>
                        Długość zawieszki:
                    </td>
                    <td>
                        max. 100cm (regulowana długość)
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
