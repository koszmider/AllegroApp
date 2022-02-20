<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="lampa_szklana_nocna.aspx.cs" Inherits="LajtIt.AllegroWeb.LampaNocnaSzklanaPage" %>
    
<%@ Register Src="~/Controls/KronosMKolory.ascx" TagName="KronosMKolory" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Kronos.ascx" TagName="Kronos" TagPrefix="uc" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace" TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
     
    <div id="offer">
        <h1 class="item_title color-r">
            Lampa stołowa Lajt</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/lampka_nocna_szklana1.jpg"
                    alt="Lampa stołowa Lajt" style="width:300px" />
            </div><div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/lampka_nocna_szklana2.jpg"
                    alt="Lampa stołowa Lajt" />
            </div>
        </div>
        <div class="offer_spec">
            <table> 
                <tr>
                    <td>
                        Wysokość lampki:
                    </td>
                    <td>
                        45cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Wysokość klosza:
                    </td>
                    <td>
                        16cm  
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
                        45W (odpowiednik 120W dla żarówki tradycyjnej)
                    </td>
                </tr> 
                <tr>
                    <td>
                        Kolor podstawki:
                    </td>
                    <td>
                        czarny
                    </td>
                </tr>
                <tr>
                    <td>
                        Materiał abażura:
                    </td>
                    <td>
                        szkło (do połowy matowe, od połowy z połyskiem)
                    </td>
                </tr>
                <tr>
                    <td>
                        Materiał podstawki:
                    </td>
                    <td>
                        stal proszkowana
                    </td>
                </tr>    
                <tr>
                    <td>
                        Zestaw obejmuje:
                    </td>
                    <td>
                         abażur, podstawa
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
            Zamówiony towar wysyłamy kurierem. Czas dostawy przez Kuriera to 1-2 dni robocze. 
            Zobacz pełen <a href="http://allegro.pl/my_page.php?uid=678165#jak_kupowac" target="_blank">
                proces realizacji zamówienia</a>.
        </div>
    </div>
</asp:Content>
