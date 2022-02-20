<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="zawieszki.aspx.cs" Inherits="LajtIt.AllegroWeb.ZawieszkiPage" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
        <h1 class="item_title color-r">
            Zawieszki, zawiesia, zwisy do lamp</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit.pl_kolorowe_zawieszki_zawiesia_zwisy.jpg"
                    alt="Zawieszki" />
            </div>
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Materiał: 
                    </td>
                    <td>
                        kabel w otulinie, podsufitka plastik
                    </td>
                </tr>
                <tr>
                    <td>
                        Długość
                    </td>
                    <td>
                        max 1m (na zamówienie: dowolna długość)
                    </td>
                </tr> 
                <tr>
                    <td>
                        Kolory: 
                    </td>
                    <td>
                        biały, czerwony, zielony, srebrny, niebieski, złoty, czarny
                    </td>
                </tr>
                <tr>
                    <td>
                        Trzonek: 
                    </td>
                    <td>
                        E 27
                    </td>
                </tr> 
                <tr>
                    <td>
                        Uwagi: 
                    </td>
                    <td>
                        kupując lampy na naszych aukcjach, zawieszka w dowolnym kolorze w cenie!
                    </td>
                </tr> 
                 
            </table>
        </div>
    </div>
  
    <a name="wysylka"></a>
    <div id="shipping">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_wysylka.png" alt="Wysyłka" />
        <div class="regular_text">
            Zamówiony towar wysyłamy kurierem lub Pocztą Polską. Czas dostawy przez Kuriera to 1-2 dni robocze. Poczta Polska dostarcza towar w ciągu 2-4 dni roboczych.
            Zobacz pełen <a href="http://allegro.pl/my_page.php?uid=678165#jak_kupowac" target="_blank">
                proces realizacji zamówienia</a>.
        </div>
    </div>
</asp:Content>
