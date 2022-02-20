<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductCatalogBatches.aspx.cs" Inherits="LajtIt.Web.ProductCatalogBatches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Timer ID="tmTimer" runat="server" Interval="5000" OnTick="tmTimer_OnTick">
    </asp:Timer>
    <asp:Panel GroupingText="Wyszukiwanie" runat="server">
        <table>
            <tr valign="top">
                <td style="width: 300px">
                    <asp:Panel runat="server" GroupingText="Wyszukiwanie batchy" ID="Panel2">
                        <asp:ListBox SelectionMode="Multiple" runat="server" DataValueField="BatchStatusId"
                            Rows="6" DataTextField="Name" ID="lbxBatchStatus" Width="200"></asp:ListBox>
                        <br /> 
                        <asp:Button runat="server" ID="btnBatchSearch" OnClick="btnBatchSearch_Click" Text="Pokaż batche"
                            CausesValidation="false" />
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel runat="server" GroupingText="" ID="pnBatch" Width="600">
                        <asp:DropDownList runat="server" DataValueField="BatchId" DataTextField="Name" ID="ddlBatch"
                            Width="200" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch_OnSelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;<asp:Literal runat="server" EnableViewState="false" ID="litBatchInfo"></asp:Literal>
                        <br />
                        <asp:Button runat="server" ID="btnBatchDelete" OnClick="btnBatchDelete_Click" Visible="false"
                            Text="Usuń" CausesValidation="false" OnClientClick="return confirm('Usunąć?');" />
                        <asp:Button runat="server" ID="btnBatchPause" OnClick="btnBatchPause_Click" Visible="false"
                            Text="Zatrzymaj" CausesValidation="false" OnClientClick="return confirm('Zatrzymać?');" />
                        <asp:Image runat="server" ID="imgVerifying" Visible="false" ImageUrl="/Images/progress.gif" /><br />
                        <asp:Literal runat="server" ID="litSummary" EnableViewState="false"></asp:Literal>
                    </asp:Panel>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pCreateItems" GroupingText="Wystaw aukcje" runat="server">
        <table>
            <tr>
                <td>
                    Liczba dni
                </td>
                <td>
                    Promowanie
                </td>
                <td>
                    Konto
                </td>
                <td>Aukcja</td>
                <td>Stan</td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlDays" runat="server">
                        <asp:ListItem Value="0">Allegro - 3 dni</asp:ListItem>
                        <asp:ListItem Value="1">Allegro - 5 dni</asp:ListItem>
                        <asp:ListItem Value="2">Allegro - 7 dni</asp:ListItem>
                        <asp:ListItem Value="3">Allegro - 10 dni</asp:ListItem>
                        <asp:ListItem Value="4">Allegro - 14 dni</asp:ListItem>
                        <asp:ListItem Value="6">Allegro - 20 dni</asp:ListItem>
                        <asp:ListItem Value="5">Sklep - 30 dni</asp:ListItem>
                        <asp:ListItem Value="99" Selected="True">Sklep - do wyczerpania</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPromotion">
                        <asp:ListItem Value="false">Niepromuj</asp:ListItem>
                        <asp:ListItem Value="true">Promuj</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlAllegroUser">
                        <asp:ListItem Value="0">-- konto domyślne ---</asp:ListItem>
                        <asp:ListItem Value="678165">JacekStawicki</asp:ListItem>
                        <asp:ListItem Value="28277822">CzerwoneJablko</asp:ListItem>
                        <asp:ListItem Value="44282528">Oswietlenie_Lodz</asp:ListItem>
                        <asp:ListItem Value="55501013">sklep_italux</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                <asp:RadioButton runat="server" GroupName="Auction" ID="rbtnAuctionNo" Checked="true" Text="Nie" />
                <asp:RadioButton runat="server" GroupName="Auction" ID="rbtnAuctionYes" Text="Tak" />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlStatus">
                        <asp:ListItem Value="1">Nowy</asp:ListItem>
                        <asp:ListItem Value="2">Używany</asp:ListItem>
                        <asp:ListItem Value="238058">Powystawowy</asp:ListItem>
                        <asp:ListItem Value="238062">Po zwrocie</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr> 
            <tr> 
                 
            </tr>
            <tr>
                <td colspan="4">
                    <asp:CheckBox runat="server" ID="chbPlanning" Text="Planowane wystawienie" OnCheckedChanged="chbPlanning_OnCheckedChanged"
                        AutoPostBack="true" /><asp:DropDownList ID="ddlDay" runat="server" Enabled="false">
                        </asp:DropDownList>
                    <asp:DropDownList ID="ddlHour" runat="server" Enabled="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btnSent" Text="Wystaw aukcje" OnClick="btnSent_Click"
                                    OnClientClick="return confirm('Czy chcesz wystawić aukcje?');" Enabled="false" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnPreview" Text="Zweryfikuj" OnClick="btnPreview_Click" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSave" Text="Zapisz dane" OnClick="btnSave_Click" />
                            </td>
                            <td>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <img src="/Images/progress.gif" /></ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:GridView runat="server" ID="gvItems" AutoGenerateColumns="false" DataKeyNames="Id"
        OnRowDataBound="gvItems_OnDataBound" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="Nazwa produktu/Tytuł aukcji">
                <ItemStyle Width="350" />
                <ItemTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:Image runat="server" ID="imgImage" Width="100" />
                            </td>
                            <td>
                                Dostawca:
                                <asp:Literal runat="server" ID="litSupplier"></asp:Literal><br />
                                <b>
                                    <asp:HyperLink runat="server" ID="hlName" Target="_blank"></asp:HyperLink></b><br />
                                Ilość: <b>
                                    <asp:Literal runat="server" ID="litQuantity"></asp:Literal></b><br />
                                Cena: <b>
                                    <asp:Literal runat="server" ID="litPrice"></asp:Literal></b><br />
                                Data dodania:
                                <asp:Literal runat="server" ID="litInsertDate"></asp:Literal></b><br />
                            Konto:<b>
                                    <asp:Literal runat="server" ID="litAllegroUserName"></asp:Literal></b><br />
                         <%--       Typ: <b>
                                    <asp:Literal runat="server" ID="litLampType"></asp:Literal><br /> --%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Wynik walidacji">
                <ItemStyle Width="150" />
                <ItemTemplate>
                    <asp:Image runat="server" ID="imgOK" Visible="false" Width="50" ImageUrl="/Images/ok.jpg" /><asp:Image
                        runat="server" ID="imgError" Visible="false" Width="50" ImageUrl="/Images/false.jpg" /><br />
                    <asp:Literal runat="server" ID="litMsg"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Panel runat="server" ID="pnlCreated" GroupingText="Aukcja utworzona">
                        <table>
                            <tr>
                                <td>
                                    Konto
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblAllegroUserCreated"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nazwa
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblAllegroName"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Id
                                </td>
                                <td>
                                    <asp:HyperLink runat="server" ID="hlAllegroItemId" Target="_blank"></asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Data utworzenia/wystawienia
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblAllegroCreateDate"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cena
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblAllegroCreatePrice"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Ilość
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblQuantity"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlCreate">
                        <table>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" ID="hlPreview" Target="_blank"><img src="Images/view.png" /></asp:HyperLink>
                                    <asp:LinkButton runat="server" CausesValidation="false" CommandArgument='<%# Eval("Id") %>'
                                        ID="lbtnDeleteItemFromQueue" CommandName="delete" OnClick="lbtnDeleteItemFromQueue_Click"
                                        OnClientClick="return confirm('Czy usunąć z kolejki?');"><asp:Image runat="server" ImageUrl="~/Images/cancel.jpg" /></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
