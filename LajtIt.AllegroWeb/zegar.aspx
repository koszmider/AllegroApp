<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="zegar.aspx.cs" Inherits="LajtIt.AllegroWeb.ZegarPage " %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
 
    <div id="offer">
        <h1 class="item_title color-r">
            Zegar Karlsson Cubic </h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/zegar/karlsson4.jpg"
                    alt="Zegar Kalsson" />
                    <div class="offer_main_photo_comment">Ułożenie kostek zależy tylko od Twojej wyobraźni</div>
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/zegar/karlsson5.jpg"
                    alt="Zegar Kalsson" />
            </div>
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/zegar/karlsson1.jpg"
                    alt="Zegar Kalsson" />
                    <div class="offer_main_photo_comment">Zegar w aranżacji</div>
            </div> 
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Wymiary wskazówek:

                    </td>
                    <td>
                        31cm i 24 cm 
                    </td>
                </tr>
                <tr>
                    <td>
                       Wymiary kostek:
                    </td>
                    <td>
                        6 cm x 6 cm x 6 cm
                    </td>
                </tr> 
                <tr>
                    <td>
                       Materiał:
                    </td>
                    <td>
                       stal, plastik
                    </td>
                </tr> 
                <tr>
                    <td>
                      Bateria:
                    </td>
                    <td>
                        1 x AA (nie ma w zestawie)
                    </td>
                </tr> 
            </table>
        </div>
    </div>
    

    <a name="wysylka"></a>
    <div id="shipping">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_wysylka.png" alt="Wysyłka" />
        <div class="regular_text">
            Zamówiony towar wysyłamy   Pocztą Polską.   Poczta Polska dostarcza towar w ciągu 2-4 dni
            roboczych. 
        </div>
    </div>
</asp:Content>
