<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllegroItemsUpdate.aspx.cs" Inherits="LajtIt.Web.AllegroItemsUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" GroupingText="Wyszukiwanie">
                <table>
                    <tr>
                        <td>
                            Sprzedaż
                        </td>
                        <td>
                            Nazwa/kod
                        </td>
                        <td>
                            Promowane
                        </td>
                        <td>
                            Stany magazynowe
                        </td>
                        <td>
                            Konto
                        </td>
                        <td>
                            Dostawca
                        </td>
                        <td>
                            Import
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSell">
                                <asp:ListItem Value="-1">--</asp:ListItem>
                                <asp:ListItem Value="true" Selected="True">Tak</asp:ListItem>
                                <asp:ListItem Value="false">Nie</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txbSearch"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlIsPromoted">
                                <asp:ListItem Value="-1">--</asp:ListItem>
                                <asp:ListItem Value="true">Tak</asp:ListItem>
                                <asp:ListItem Value="false">Nie</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlQuantities">
                                <asp:ListItem Value="-1">--</asp:ListItem>
                                <asp:ListItem Value="true">Zgodne</asp:ListItem>
                                <asp:ListItem Value="false">Niezgodne</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlUserName" AppendDataBoundItems="true">
                                <asp:ListItem Value="-1">--</asp:ListItem> 
                        <asp:ListItem Value="678165">JacekStawicki</asp:ListItem>
                        <asp:ListItem Value="28277822">CzerwoneJablko</asp:ListItem>
                        <asp:ListItem Value="44282528">Oswietlenie_Lodz</asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSuppliers" DataTextField="DisplayName" DataValueField="SupplierId" AppendDataBoundItems="true">
                                <asp:ListItem Value="-1">--</asp:ListItem> 
                            </asp:DropDownList></td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlImport" DataTextField="Name" DataValueField="ImportId"
                                AppendDataBoundItems="true">
                                <asp:ListItem Value="0">-- wszystkie --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button runat="server" OnClick="tmTimer_OnTick" Text="Szukaj" />
                        </td>
                    </tr> 
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" GroupingText="Akcje">
                <asp:CheckBoxList runat="server" ID="chblUpdate" RepeatDirection="Horizontal">
                    <asp:ListItem>Wszystko</asp:ListItem>
                    <asp:ListItem>Cennik dostawy</asp:ListItem>
                    <asp:ListItem>Ilość</asp:ListItem>
                    <asp:ListItem>Cena</asp:ListItem>
                    <asp:ListItem>Tytuł</asp:ListItem>
                    <asp:ListItem>Opis</asp:ListItem>    
                    <asp:ListItem>Zdjęcia</asp:ListItem>    
                    <asp:ListItem>Parametry</asp:ListItem>     
                                                <asp:ListItem>Status</asp:ListItem>
                                                <asp:ListItem>Kategoria</asp:ListItem>
                                                <asp:ListItem>Ean</asp:ListItem>       
            </asp:CheckBoxList><asp:CheckBox runat="server" ID="chbSearchAll" Text="Zastosuj do wszystkich" />
                <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="Dodaj do kolejki"
                    OnClientClick="return confirm('Dodać do kolejki?');" />
                <asp:Button runat="server" ID="btnRun" OnClick="btnRun_Click" Text="Uruchom aktualizowanie"
                    OnClientClick="return confirm('Uruchomić aktualizowanie?');" />
                <asp:LinkButton runat="server" ID="btnStop" OnClick="btnStop_Click" Text="Zatrzymaj aktualizowanie"
                    OnClientClick="return confirm('Zatrzymać aktualizowanie?');" />
                <asp:LinkButton runat="server" ID="btnDelete" OnClick="btnDelete_Click" Text="Usuń aktualizowanie"
                    ForeColor="Red" OnClientClick="return confirm('Usunąć z kolejki?');" />
                <asp:LinkButton runat="server" ID="btnTimer" OnClick="btnTimer_Click" Text="Włącz odświeżanie" /><asp:Image
                    runat="server" ID="imgVerifying" Visible="false" ImageUrl="/Images/progress.gif" />
            </asp:Panel>
            <asp:Timer ID="tmTimer" runat="server" Interval="5000" OnTick="tmTimer_OnTick" Enabled="false">
            </asp:Timer>
            <asp:GridView runat="server" ID="gvAllegroItems" DataKeyNames="Id" PageSize="50" AllowPaging="true"
                Style="font-size: 9pt" ShowFooter="true" OnRowDataBound="gvAllegroItems_OnRowDataBound"
                AutoGenerateColumns="false" OnPageIndexChanging="gvAllegroItems_OnPageIndexChanging">
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="30" HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LitId"></asp:Literal><br />
                            <asp:CheckBox runat="server" ID="chbOrder" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" /></HeaderTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="UserName" HeaderText="Konto" /> 
                    <asp:TemplateField HeaderText="Produkt">
                        <ItemStyle Width="300" />
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlProduct" NavigateUrl="Product.aspx?id={0}"></asp:HyperLink><br />
                            <asp:HyperLink runat="server" ID="hlProductAllegro" Target="_blank" NavigateUrl="ProductCatalog.Allegro.aspx?id={0}"></asp:HyperLink><br />
                            <asp:HyperLink runat="server" ID="hlAllegroItem" Target="_blank" NavigateUrl="http://allegro.pl/show_item.php?item={0}"></asp:HyperLink>
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kod/<br>Kod dostawcy">
                        <ItemStyle HorizontalAlign="Right" Width="100" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCode"></asp:Label> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Wystawiono/<br>w magazynie">
                        <ItemStyle HorizontalAlign="Right" Width="120" />
                        <ItemTemplate>
                        <asp:Label runat="server" ID="lblBidCount"></asp:Label><br />
                        Wystawiono:    <asp:Label runat="server" ID="lblQuantity"></asp:Label><br />
                        W magazynie:    <asp:Label runat="server" ID="lblLeftQuantity"></asp:Label><br />
                            <asp:Label runat="server" ID="lblAllegroPrice"></asp:Label>/
                            <asp:Label runat="server" ID="lblSellPrice"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Status aktualizacji">
                        <ItemStyle Width="150" />
                        <ItemTemplate>
                            <asp:Image runat="server" ID="imgOK" Visible="false" Width="50" ImageUrl="/Images/ok.jpg" /><asp:Image
                                runat="server" ID="imgError" Visible="false" Width="50" ImageUrl="/Images/false.jpg" /><br />
                            <asp:Literal runat="server" ID="litMsg"></asp:Literal>
                            <asp:Image runat="server" ID="imgVerifying" Visible="false" Width="50" ImageUrl="/Images/progress.gif" />
                            <asp:Image runat="server" ID="imgPause" Width="50" Visible="false" ImageUrl="/Images/pause.png" /></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
