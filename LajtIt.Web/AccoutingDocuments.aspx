<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccoutingDocuments.aspx.cs" Inherits="LajtIt.Web.AccoutingDocuments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Dokumenty księgowe</h1>
    <br />
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="lbtnLajtit" />
            <asp:PostBackTrigger ControlID="lbtnAllegro" />
            <asp:PostBackTrigger ControlID="lbtnDpd" />
            <asp:PostBackTrigger ControlID="lbtnCeneo" />
            <asp:PostBackTrigger ControlID="lbtnMorele" />
            <asp:PostBackTrigger ControlID="lbtnEvidence" />
            <asp:PostBackTrigger ControlID="lbtnInvoices" />
            <asp:PostBackTrigger ControlID="lbtnInvoicesCorrections" />
            <asp:PostBackTrigger ControlID="lbtnInvoicesSummary" />
            <asp:PostBackTrigger ControlID="lbtnJPK" />
            <asp:PostBackTrigger ControlID="lbtnFirstData" />
            <asp:PostBackTrigger ControlID="lbtnJPKMAG" />
            <asp:PostBackTrigger ControlID="lbtnWarehouse" />
            <asp:PostBackTrigger ControlID="lbtnPayBack2" />
            <asp:PostBackTrigger ControlID="lbtnPayBack21" />
            <asp:PostBackTrigger ControlID="lbtnPayBack22" />
            <asp:PostBackTrigger ControlID="lbtnWarehouseEndMonthList" />
            <asp:PostBackTrigger ControlID="lbtnPz" />
            <asp:PostBackTrigger ControlID="lbtnWz" />
        </Triggers>
    </asp:UpdatePanel>
    Miesiąc:                
    <asp:DropDownList runat="server" ID="ddlMonth">
    </asp:DropDownList>
    <br />
    <br />
    <asp:Panel runat="server" GroupingText="Płatności">
        <table>
            <tr>
                <td style="width: 300px">Przelewy24</td>
                <td style="width: 300px">
                    <asp:LinkButton runat="server" ID="lbtnLajtit" OnClick="lbtnLajtit_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 300px">Allegro</td>
                <td style="width: 300px">
                    <asp:LinkButton runat="server" ID="lbtnAllegro" OnClick="lbtnAllegro_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 300px">DPD</td>
                <td style="width: 300px">
                    <asp:LinkButton runat="server" ID="lbtnDpd" OnClick="lbtnDpd_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 300px">Ceneo</td>
                <td style="width: 300px">
                    <asp:LinkButton runat="server" ID="lbtnCeneo" OnClick="lbtnCeneo_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 300px">Morele</td>
                <td style="width: 300px">
                    <asp:LinkButton runat="server" ID="lbtnMorele" OnClick="lbtnMorele_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 300px">First Data</td>
                <td style="width: 300px">
                    <asp:LinkButton runat="server" ID="lbtnFirstData" OnClick="lbtnFirstData_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" GroupingText="Faktury">
        <table>
            <tr>
                <td style="width: 500px">Ewidencja sprzedażowa</td>
                <td style="width: 500px">
                    <asp:LinkButton runat="server" ID="lbtnEvidence" OnClick="lbtnEvidence_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 500px">Faktury sprzedażowe</td>
                <td style="width: 500px">
                    <asp:LinkButton runat="server" ID="lbtnInvoices" OnClick="lbtnInvoices_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 500px">Korekty</td>
                <td style="width: 500px">
                    <asp:LinkButton runat="server" ID="lbtnInvoicesCorrections" OnClick="lbtnInvoicesCorrections_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 500px">Podsumowanie faktur</td>
                <td style="width: 500px">
                    <asp:LinkButton runat="server" ID="lbtnInvoicesSummary" OnClick="lbtnInvoicesSummary_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <%--        <tr>
            <td style="width: 500px">Ewidencja (wpłaty i zwroty)</td>
            <td style="width: 500px">
                <asp:LinkButton runat="server" ID="lbtnPayBack" OnClick="lbtnPayBack_Click" Text="Pobierz"></asp:LinkButton></td>
        </tr> --%>
            <tr>
                <td style="width: 500px">Ewidencja (zwroty [fiskalne i niefiskalne])</td>
                <td style="width: 500px">
                    <asp:LinkButton runat="server" ID="lbtnPayBack2" OnClick="lbtnPayBack2_Click" Text="Pobierz"></asp:LinkButton>
                    &nbsp; 
                    [<asp:LinkButton runat="server" ID="lbtnPayBack21" OnClick="lbtnPayBack21_Click" Text="Pobierz"></asp:LinkButton>
                    ,
                    <asp:LinkButton runat="server" ID="lbtnPayBack22" OnClick="lbtnPayBack22_Click" Text="Pobierz"></asp:LinkButton>
                    ]
                </td>
            </tr>
            <%--        <tr>
            <td style="width: 500px">Ewidencja (wpłaty)</td>
            <td style="width: 500px">
                <asp:LinkButton runat="server" ID="lbtnPayBack3" OnClick="lbtnPayBack3_Click" Text="Pobierz"></asp:LinkButton></td>
        </tr> --%>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" GroupingText="Magazyn">
        <table>
            <tr>
                <td style="width: 500px">Pz - przyjęcia</td>
                <td style="width: 500px">
                    <asp:LinkButton runat="server" ID="lbtnPz" OnClick="lbtnPz_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 500px">Wz - wydania</td>
                <td style="width: 500px">
                    <asp:LinkButton runat="server" ID="lbtnWz" OnClick="lbtnWz_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" GroupingText="Pozostałe">
        <table>
            <tr>
                <td style="width: 300px">Plik JPK_FA</td>
                <td style="width: 300px">
                    <asp:LinkButton runat="server" ID="lbtnJPK" OnClick="lbtnJPK_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 300px">Plik JPK_MAG</td>
                <td style="width: 300px">
                    <asp:LinkButton runat="server" ID="lbtnJPKMAG" OnClick="lbtnJPKMAG_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 300px">Magazyn (przyjęte)</td>
                <td style="width: 300px">
                    <asp:LinkButton runat="server" ID="lbtnWarehouse" OnClick="lbtnWarehouse_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 300px">Magazyn (stan na koniec miesiąca)</td>
                <td style="width: 300px">
                    <asp:LinkButton runat="server" ID="lbtnWarehouseEndMonthList" OnClick="lbtnWarehouseEndMonthList_Click" Text="Pobierz"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2">Faktury do wydruku:
                    <ul>
                        <li>Oświetlenie -> M-Form</li>
                        <li>Allegro</li>
                        <li>Amazon</li>
                        <li>Ceneo</li>
                        <li>Wypłaty gotówki</li>
                    </ul>
                </td>
            </tr>
        </table>
    </asp:Panel>


</asp:Content>
