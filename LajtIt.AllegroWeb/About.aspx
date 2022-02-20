<%@ Page Title="About Us" Language="C#" AutoEventWireup="true" CodeBehind="About.aspx.cs"
    Inherits="LajtIt.AllegroWeb.About" %>

<%@ Register Src="~/Controls/Styles.ascx" TagName="Styles" TagPrefix="uc" %>
<form runat="server">
<uc:Styles runat="server" />
<div id="container">
    <div id="page">
        <div id="header">
            <div id="header_flags">
                <table id="table_flags" style="width: 310; height: 50; border: 0;" cellpadding="0"
                    cellspacing="0">
                    <tr>
                        <td>
                            <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_flagi_01.png" width="44"
                                height="50" alt="" />
                        </td>
                        <td>
                            <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_flagi_02.png" width="45"
                                height="50" alt="" />
                        </td>
                        <td>
                            <a href="http://static.lajtit.pl/translate.htm?l=en" target="_blank">
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_flagi_03.png" width="44"
                                    style="border: 0;" height="50" alt="" /></a>
                        </td>
                        <td>
                            <a href="http://static.lajtit.pl/translate.htm?l=cs" target="_blank">
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_flagi_04.png" width="44"
                                    style="border: 0;" height="50" alt="" /></a>
                        </td>
                        <td>
                            <a href="http://static.lajtit.pl/translate.htm?l=sk" target="_blank">
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_flagi_05.png" width="44"
                                    style="border: 0;" height="50" alt="" /></a>
                        </td>
                        <td>
                            <a href="http://static.lajtit.pl/translate.htm?l=ru" target="_blank">
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_flagi_06.png" width="45"
                                    style="border: 0;" height="50" alt="" /></a>
                        </td>
                        <td>
                            <a href="http://static.lajtit.pl/translate.htm?l=de" target="_blank">
                                <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_flagi_07.png" width="44"
                                    style="border: 0;" height="50" alt="" /></a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="contact" class="color-it bold">
            Email: <a href="mailto:kontakt@LajtIt.pl">kontakt@<span class="color-lajt bold">Lajt</span><span
                class="color-it bold">it</span>.pl</a> Telefon: +48 604 688 227 GG:
            <img src="http://status.gadu-gadu.pl/users/status.asp?id=3459943&styl=1" style="border: 0px"><a
                href="gg:3459943">3459943</a> Skype:
            <img src="http://download.skype.com/share/skypebuttons/buttons/call_blue_transparent_34x34.png"
                alt="Skype me!" style="border: 0px; width: 20px; vertical-align: top;" /><a href="skype:jacek-stawicki">jacek-stawicki</a>
        </div>
        <a name="poznaj_nas"></a>
        <div id="about_us">
            <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_poznaj_nas.png" alt="Poznaj Nas!" />
            <div class="regular_text">
                Jesteśmy młodym, pozytywnie nastawionym do życia zespołem zafascynowanym designem
                i oświetleniem. Naszą pracę doskonale oddaje maksyma „rób to co lubisz, a nie przepracujesz
                ani jednego dnia w życiu”. Tworzymy wyjątkowe przedmioty, które ocieplają przestrzeń
                i nadają domowym wnętrzom niepowtarzalny charakter. Cały czas szukamy inspiracji
                i jesteśmy otwarci na wszelkie pomysły, dlatego jeśli chcecie się czymś z nami podzielić,
                piszcie :)</div>
            <div class="regular_text">
                Zapraszamy na Nasz profil na FaceBook gdzie możesz śledzić Nasze codzienne aktualizacje
                w Naszej ofercie oraz nowinki ze świata designu i aranżacji wnętrz. Bądź z Nami
                w kontakcie!
                <br />
                <br />
                <div style="text-align: center; width: 100%">
                    <a href="http://www.facebook.com/lajtit" target="_blank">
                        <img src="http://www.lajtit.pl/public/assets/allegro/facebook.png" style="border: 0px;
                            width: 150px" alt="Facebbok" /></a></div>
                <br />
                <br />
                Odwiedź Naszego bloga, poczytaj o tym co Nam w głowach "siedzi" :)<br />
                <br />
                Ostatnio na Naszym blogu:<br />
                <br />
                <div style="text-align: center; width: 100%">
                    <a href="http://lajtit.wordpress.com/" target="_blank">
                        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_blog.jpg" alt="Blog"
                            class="color-lajt" /></a></div>
            </div>
        </div>
      <%-- <a name="wygraj_bon"></a>
        <div id="take_photo">
        <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_wysylka.png"
                alt="" />
            <div class="regular_text">
            W dniach 4-11 stycznia przebywamy na urlopie. W związku z tym, wszystkie zakupione przedmioty w okresie 4-11 stycznia zostaną dostarczone po 11 stycznia. Przepraszamy za opóźnienie w dostawie i jednocześnie liczymy na wyrozumiałość :)
            

            </div>
            <div class="regular_text">
            Dla wszystkich klientów, którzy zakupią w tym okresie towary na naszych aukcjach oferujemy darmową wysyłkę. Warunkiem skorzystania z tej opcji jest:
            <ul><li>W aukcjach gdzie już oferowana darmowa wysyłka nic się nie zmienia. Opcja darmowa wciąż jest darmowa, opcja płatna jest płatna.</li>
            <li>W aukcjach gdzie w cenniku dostawy nie ma wyszczególnionej darmowej dostawy oferujemy darmową wysyłkę w przypadku wyboru <b>listu poleconego ekonomicznego</b> lub <b>przesyłki kurierskiej</b>. Oznacza to iż należy opłacić towar przed wysłaniem.</li></ul>
            <b>Oferta darmowej wysyłki trwa do końca urlopu (11 stycznia) bądź do jej wcześniejszego odwołania.</b><br /><br />
            W przypadku dodatkowych pytań prosimy o kontakt mailowy.
            </div>
 
            <div class="regular_text">W okresie urlopu wciąż pozostajemy w kontakcie telefonicznym i mailowym, zapraszamy więc do kontaktowania się z Nami!</div>       
        </div>
        <a name="wygraj_bon"></a>
        <div id="take_photo">
            <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_kalendarz_dostaw.png"
                alt="Kalendarz dostaw" />
            <div class="regular_text">
                Święta Bożego Narodzenia przed Nami. Aby zdążyć z prezentami przed
                świętami   proszę sprawdzić w tabelce poniżej do kiedy najpóźniej należy zakupić i opłacić (lub wybrać przesyłkę pobraniową) zamówienie aby otrzymać przesyłkę do 24 grudnia *.<br /><br />
                <table style="border: solid 1px black;">
                    <tr class="black">
                        <td>
                            Dzień
                        </td>
                        <td>
                            17.12 (pn)
                        </td> 
                        <td>
                            18.12 (wt)
                        </td>
                        <td>
                            19.12 (śr)
                        </td>
                        <td>
                            20.12 (czw)
                        </td>
                        <td>
                            21.12 (pt)
                        </td>
                        <td>
                            22.12 (sb)
                        </td>
                        <td>
                            23.12 (nd)
                        </td>
                        <td>
                            24.12 (pn)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Przesyłka kurierska
                        </td>
                        <td class="green">&nbsp;</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">do 16:00</td>
                    </tr>
                    <tr>
                        <td>
                            Poczta Polska - list priorytetowy
                        </td>
                        <td class="green">&nbsp;</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">do 13:00</td>
                    </tr>
                    <tr>
                        <td>
                            Poczta Polska - list ekonomiczny</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">do 13:00</td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Odbiór osobisty (Łódź)
                        </td>
                        <td class="green">&nbsp;</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">&nbsp;</td>
                        <td class="green">do 12:00</td>
                    </tr>
                </table><br /><br />
                * Podane czasy mają charakter orientacyjny i nie stanowią gwarancji doręczenia w wybranym terminie. 
                <br /><br />
                Zamówienia, które nie zostaną dostarczone przed świętami, będą dostarczane w dniach 27.12(czw), 28.12(pt), 31.12(pn).
            </div>
        </div>--%>
        <a name="jak_kupowac"></a>
        <div id="order_process">
            <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_realizacja_zamowienia.png"
                alt="Jak złożyć zamówienie?" />
            <div class="regular_text">
                W celu jak najszybszej realizacji zamówienia prosimy:
                <ul>
                    <li>Po zakupie przedmiotu prosimy wypełnić formularz dostawy określając w nim rodzaj
                        przesyłki oraz adres dostawy</li>
                    <li>Prosimy dokonać wpłaty za towar i przesyłkę poprzez PayU lub przelew bankowy (nr
                        konta przesyłamy w wiadomości tuż po zakupie na Allegro)</li>
                    <li>Koniecznie prosimy podać kolor lamp w aukcjach gdzie są dostępne lampy w różnych
                        kolorach</li>
                    <li>Przesyłki wysyłamy w ciągu 48 godzin od: daty wpłaty lub daty wyboru przesyłki pobraniowej
                        oraz podania wszystkich niezbędnych parametrów zamówionego towaru bez których jego
                        realizacja nie jest możliwa (np. kolor lampy).</li>
                    <li>Przesyłki wysyłamy:
                        <ul>
                            <li>Pocztą Polską - tylko listy polecone. Czas dostawy przez PP 2-4 dni robocze</li>
                            <li>Kurierem - paczki oraz przesyłki pobraniowe. Czas dostawy przez Kuriera: na drugi
                                dzień roboczy.</li>
                        </ul>
                        Rodzaj dostępnej przesyłki podany jest w Aukcji. </li>
                    <li>Po nadaniu przez Nas przesyłki kurierskiej Kupujący otrzyma informację email z numerem
                        nadania oraz linkiem do śledzenia przesyłki. Przy realizacji zamówienia przez Pocztę
                        Polską, nie wysyłamy numeru nadania ale istnieje możliwość zapytania Nas o jego
                        numer i sprawdzenia listu na stronie <a href="http://sledzenie.poczta-polska.pl/"
                            target="_blank">sledzenie.poczta-polska.pl</a></li>
                </ul>
            </div>
        </div>
        <a name="regulamin"></a>
        <div id="rules">
            <img src="http://www.lajtit.pl/public/assets/allegro/lajtit_regulamin_zakupow.png"
                alt="Regulamin zakupów" />
            <div class="regular_text">
                Prosimy zapoznać się z regulaminem zakupów na Naszych aukcjach.
                <ul>
                    <li>Właścicielem konta Allegro <b>jacekstawicki</b>
                        <img src="http://static.allegrostatic.pl/site_images/1/0/company_icon.gif" />
                        jest: RTP Development Jacek Stawicki. z siedzibą w Łódź, Treflowa 29 , NIP: 7272491257,
                        tel +48604688227, adres email: kontakt<b>@</b>lajtit.pl.</li>
                    <li>Zakup przedmiotu oznacza akceptację regulaminu.</li>
                    <li>Podane ceny zawierają podatek VAT.</li>
                    <li>Na specjalne życzenie wystawiamy faktury VAT.</li>
                    <li>Zakupiony towar (nieuszkodzony) można zwrócić w ciągu 10 dni od daty jego otrzymania.
                        Koszt zwrotu towaru, w oryginalnym opakowaniu, ponosi Kupujący. Towar należy odesłać
                        przesyłką rejestrowaną oraz niepobraniową na adres siedziby firmy (podany wyżej).
                        Zwrot kwoty zakupu towaru dokonujemy na wskazany nr konta bankowego w ciągu 7 dni
                        od daty jego otrzymania. W przypadku aukcji z darmową wysyłką od kwoty zwrotu potrącany
                        jest faktyczny koszt wysyłki towaru do Kupującego.</li>
                    <li>Nie wysyłamy towaru przesyłką pobraniową do Kupujących, którzy nie mają pełnej aktywacji
                        konta lub posiadają negatywne komentarze dotyczące problemów z odbiorem przesyłki.</li>
                </ul>
            </div>
        </div>
    </div>
</div>
</form>
