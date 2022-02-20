<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="kinkiet_tromle.aspx.cs" Inherits="LajtIt.AllegroWeb.KinkietTromlePage " %>
    
<%@ Register Src="~/Controls/TromleLKolory.ascx" TagName="TromleLKolory" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Tromle.ascx" TagName="Tromle" TagPrefix="uc" %>
<%@ Register Src="~/Controls/TromleProduktyUzupelniajace.ascx" TagName="TromleProduktyUzupelniajace" TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="price">
        <div class="color-r">
            11,40 zł</div>
    </div>
    <div id="offer">
        <h1 class="item_title color-r">
            Kinkiet Tromle</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/tromle/www.lajtit.pl_modne_lampy_tromle_kinkiet_bialy.jpg" style="width:300px"
                    alt="Kinkiet Tromle" /> 
            </div> 
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/tromle/www.lajtit.pl_modne_lampy_tromle_kinkiet_czarny.jpg" style="width:300px"
                    alt="Kinkiet Tromle" /> 
            </div> 
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/tromle/www.lajtit.pl_modne_lampy_tromle_kinkiet.jpg" style="width:300px"
                    alt="Kinkiet Tromle" /> 
                    <div class="offer_main_photo_comment">Wygląd samego kinkieta</div>
            </div> 
        </div>
        <div class="offer_spec">
            <table> 
               
                <tr>
                    <td>
                        Średnica abażura:
                    </td>
                    <td>
                        23cm  
                    </td>
                </tr> 
                <tr>
                    <td>
                        Wysokość abażura:
                    </td>
                    <td>
                        20cm  
                    </td>
                </tr> 
                <tr>
                    <td>
                        Wysokość kinkieta z abażurem:
                    </td>
                    <td>
                        38cm  
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
                        Kolor kinkieta:
                    </td>
                    <td>
                        stalowy
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
                        Materiał kinkieta:
                    </td>
                    <td>
                        stal niklowana
                    </td>
                </tr>   
                <tr>
                    <td>
                        Abażur wymaga składania:
                    </td>
                    <td>
                         Tak 
                    </td>
                </tr>
                <tr>
                <td>Kinkiet:</td>
                <td>Długość kabla 3m, włącznik na kablu. Można przystosować do podłączenia bezpośrednio w ścianie.</td>
                </tr>
                <tr>
                    <td>
                        Zestaw obejmuje:
                    </td>
                    <td>
                         abażur, kinkiet
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="offer_spec_comments">
        * Kolory abażurów: <uc:TromleLKolory ID="ucTromleLKolory" runat="server" /><br />
    
     
    </div>
    <div class="regular_text">
      
        <div class="regular_text">
            Abażur dostępny jest w wielu kolorach i w wielu układach kolorystycznych.
            Poniżej prezentujemy najbardziej popularne zestawy. Na Państwa specjalne życzenie
            możemy przygotować dowolny układ kolorów spośród dostępnych (dostępność danego koloru
            opisana jest powyżej).
        </div>
        <uc:Tromle KronosType="M" runat="server"  />
    </div>
    <a name="more"></a>
    <div id="more">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_czy_wiesz_ze.png" alt="Czy wiesz, że ... ?" />
        <ul>
            <li>Można zamówić jedno, dwu lub wielokolorowy abażur</li>
            <li>Można zamówić sam abażur, zobacz Nasze pozostałe aukcje</li>  
            <li>Do kompletu można zakupić: lampki nocne, stołowe, podłogowe a także kinkiety. Zobacz
                Naszą pełną ofertę</li>
        </ul>
    </div>
    <a name="suggestions"></a>
    <div id="suggestions">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_produkty_uzupelniajace.png"
            alt="Produkty uzupełniające" />
            <uc:TromleProduktyUzupelniajace runat="server" />
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
