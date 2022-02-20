<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="abazur_klosz_not.aspx.cs" Inherits="LajtIt.AllegroWeb.AbazurKloszNotPage  " %>
    
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace" TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
 
    <div id="offer">
        <h1 class="item_title color-r">
            Klosz plastikowy</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/klosz_plastikowy.png"
                    alt=" " />
            </div>
        </div>
        <div class="offer_spec">
            <table>
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
                        Szerokość:
                    </td>
                    <td>
                        28cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Średnica:
                    </td>
                    <td>
                        28cm
                    </td>
                </tr> 
                <tr>
                    <td>
                        Średnica otworu:
                    </td>
                    <td>
                       4,2cm (gwint E27)
                    </td>
                </tr>
                <tr>
                    <td>
                        Materiał:
                    </td>
                    <td>
                     tworzywo polipropylenowe
                    </td>
                </tr>
                <tr>
                    <td>
                        Max moc
                    </td>
                    <td>
                     100W
                    </td>
                </tr>
            </table>
        </div><div class="regular_text">
      Do klosza można <a href="http://allegro.pl/sklep/678165_lajt-it-design-you-like?id=678165&category=677326" target="_blank">dokupić zawieszkę</a>. Komplet stworzy ciekawą i tanią lampę.</div> 
    </div> 
      
   
    <a name="suggestions"></a>
    <div id="suggestions">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_produkty_uzupelniajace.png"
            alt="Produkty uzupełniające" />
            <uc:KronosProduktyUzupelniajace ID="KronosProduktyUzupelniajace1" runat="server" />
    </div>
 


    <a name="wysylka"></a>
    <div id="shipping">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_wysylka.png" alt="Wysyłka" />
        <div class="regular_text">
            Zamówiony towar wysyłamy kurierem. Czas dostawy
            przez Kuriera to 1-2 dni robocze. Zobacz pełen <a href="http://allegro.pl/my_page.php?uid=678165#jak_kupowac"
                target="_blank">proces realizacji zamówienia</a>.
        </div>
    </div>
</asp:Content>
