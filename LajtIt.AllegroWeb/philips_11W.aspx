<%@ Page  Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="philips_11W.aspx.cs" Inherits="LajtIt.AllegroWeb.Philips11WPage" %>
<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace" TagPrefix="uc" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
        <h1 class="item_title color-r">
            Świetlówka kompaktowa GENIE Philips 11W</h1>
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/userdata/gfx/fc1210e033c365107f88096610bb022d.jpg"
                    alt="Świetlówka Philips 11W" />
            </div>
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Moc: 
                    </td>
                    <td>
                        11W = 60W
                    </td>
                </tr>
                <tr>
                    <td>
                        Strumień świetlny: 
                    </td>
                    <td>
                        600 lumenów
                    </td>
                </tr> 
                <tr>
                    <td>
                        Barwa: 
                    </td>
                    <td>
                        Ciepłobiała 2700 K
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
                        Trwałość: 
                    </td>
                    <td>
                        10000 h 
                    </td>
                </tr>
                <tr>
                    <td>
                        Klasa wydajności: 
                    </td>
                    <td>
                        "A"
                    </td>
                </tr>
                <tr>
                    <td>
                        Długość:
                    </td>
                    <td>
                        117 mm
                    </td>
                </tr>
                <tr>
                    <td>
                        Średnica: 
                    </td>
                    <td>
                        44.4 mm
                    </td>
                </tr>
                 
            </table>
        </div>
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
            Zamówiony towar wysyłamy kurierem lub Pocztą Polską. Czas dostawy przez Kuriera to 1-2 dni robocze. Poczta Polska dostarcza towar w ciągu 2-4 dni roboczych.
            Zobacz pełen <a href="http://allegro.pl/my_page.php?uid=678165#jak_kupowac" target="_blank">
                proces realizacji zamówienia</a>.
        </div>
    </div>
</asp:Content>
