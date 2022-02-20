<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true" CodeBehind="ProductCatalog.Allegro.aspx.cs"
    Inherits="LajtIt.Web.ProductAllegro" %>

<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function changeValue(f) {
            f.select();

        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <uc:ProductMenu runat="server" SetTab="td2"></uc:ProductMenu>
    <asp:Panel runat="server" GroupingText="Informacje na potrzeby wystawiania aukcji">
        <asp:UpdatePanel runat="server" ID="upAllegro">
            <ContentTemplate>
                <table>
                    <colgroup>
                        <col width="150" />
                        <col width="750" />
                        <col />
                    </colgroup>

                    <tr>
                        <td></td>
                        <td>
                            <asp:CheckBox ID="chbAutoAssignProduct" runat="server" Text="Automatycznie przypisuj" />
                        </td>
                    </tr> 
                    <tr>
                        <td>Cena katalogowa (brutto)
                        </td>
                        <td>

                            <b>
                                <asp:Literal runat="server" ID="litPriceBrutto"></asp:Literal></b>
                        </td>
                        <tr>
                            <td>Ilość
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="litAllegroQuantity"></asp:Literal>
                            </td>
                        </tr>
                       <%-- <tr>
                            <td>Specyfikacja
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txbAllegroSpecification" TextMode="MultiLine" Rows="10"
                                    ValidationGroup="allegro" Width="500"></asp:TextBox>
                            </td>
                            <td>Wpisuj dane w układzie: Etykieta: opis
                            </td>
                        </tr>
                        <tr>
                            <td>Opis
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txbAllegroDescription" TextMode="MultiLine" Rows="4"
                                    Width="500" ValidationGroup="allegro"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Krótki opis
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txbShortDescription" TextMode="MultiLine" Rows="5"
                                    Width="500" ValidationGroup="allegro"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                        runat="server" ValidationGroup="allegro" Text="*" ErrorMessage="Krótki opis"
                                        ControlToValidate="txbShortDescription"></asp:RequiredFieldValidator>
                            </td>
                        </tr>--%>

                     
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btnAllegro" Text="Zapisz" OnClick="btnAllegro_Click"
                                    ValidationGroup="allegro" /><asp:ValidationSummary HeaderText="Następujące pola są wymagane:"
                                        runat="server" ValidationGroup="allegro" DisplayMode="BulletList" ShowMessageBox="true"
                                        ShowSummary="false" />
                                <span style="position: absolute;">
                                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upAllegro">
                                        <ProgressTemplate>
                                            <img src="Images/progress.gif" style="height: 20px" alt="" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:HyperLink runat="server" Text="Podgląd" ID="hlPreview" Target="_blank"></asp:HyperLink>
                            </td>
                            <td></td>
                            <td style="text-align: right;">
                                <asp:LinkButton runat="server" ID="btnAllegroUpdate" Text="Aktualizuj Allegro" OnClientClick="return confirm('Wykonać aktualizację wszystkich danych?');" OnClick="btnAllegroUpdate_Click" />
                            </td>
                        </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlCreateAuction" GroupingText="Wystawianie aukcji">
        <asp:UpdatePanel runat="server" ID="upRecommended">
            <ContentTemplate>
                <asp:Literal runat="server" ID="litScheduled"></asp:Literal>
                <table>
                    <tr valign="top">

                        <td>
                            <asp:Panel runat="server" GroupingText="Historia wystawianych przedmiotów" Style="max-height: 400px; overflow: scroll;">
                                <b>Trwające</b>
                                <asp:GridView runat="server" EmptyDataText="Brak ofert" 
                                    OnRowDataBound="gvAllegroHistory_OnRowDataBound" ShowHeader="true" AutoGenerateColumns="false"
                                    ID="gvAllegroHistory">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Cena">
                                            <ItemStyle HorizontalAlign="Right" Width="80"/>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbUserName"></asp:Label><br />
                                                <asp:HyperLink runat="server" ID="hlItem" NavigateUrl="http://allegro.pl/show_item.php?item={0}" Target="_blank"></asp:HyperLink>


                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                        <asp:BoundField HeaderText="Status" DataField="ItemStatus" />
                                        <asp:BoundField HeaderText="Tytuł" DataField="AllegroName" />
                                        <asp:BoundField HeaderText="Dodano" DataField="InsertDate" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                        <asp:BoundField HeaderText="Akt." DataField="LastUpdateDate" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                        <asp:BoundField HeaderText="Koniec" DataField="EndingDate" DataFormatString="{0:yyyy/MM/dd HH:mm}"  ItemStyle-Width="120"/>
                                        <asp:TemplateField HeaderText="Cena">
                                            <ItemStyle HorizontalAlign="Right" Width="80"/>
                                            <ItemTemplate>
                                                <asp:Image ImageUrl="/Images/licytacja.jpg" ID="imgIsAuction" ToolTip="Licytacja"
                                                    Visible="false" Width="20" runat="server" />&nbsp;<asp:Label ID="lblPrice" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Uwagi"> 
                                            <ItemTemplate>
                                                
                                                <asp:Label runat="server" ID="lblComment"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Sprzedanych" DataField="QuantityOrdered" ItemStyle-Width="80"/>
                                    </Columns>
                                </asp:GridView> 
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

</asp:Content>
