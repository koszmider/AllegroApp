<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="lampa_klosz_not.aspx.cs"
    Inherits="LajtIt.AllegroWeb.LampaNotPage  " %>

<%@ Register Src="~/Controls/KronosProduktyUzupelniajace.ascx" TagName="KronosProduktyUzupelniajace"
    TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="offer">
      
        <div class="offer_main_photo">
            <div class="offer_main_photo_frame">
                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit.pl_lampa_plastikowa.jpg"
                    alt=" " />
            </div>
             
        </div>
        <div class="offer_spec">
            <table>
                <tr>
                    <td>
                        Wysokość klosza:
                    </td>
                    <td>
                        12cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Szerokość klosza:
                    </td>
                    <td>
                        28cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Średnica klosza:
                    </td>
                    <td>
                        28cm
                    </td>
                </tr>
                <tr>
                    <td>
                        Zawieszka/kabel:
                    </td>
                    <td>
                        max 1m, biała
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
                        60W
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="clear: both;">
    </div>
   
</asp:Content>
