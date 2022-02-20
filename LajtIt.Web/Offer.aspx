<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Offer.aspx.cs" Inherits="LajtIt.Web.Offer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div><h1>Kreator ofert</h1></div>
    <div style="text-align: right; width:200px; float:right;">
       <a href="Offers.aspx">Zobacz inne oferty</a></div><br /><br />

    <table style="width: 100%">
        <tr>
            <td>Nazwa oferty</td>
            <td>
                <asp:TextBox runat="server" ID="txbName"></asp:TextBox>
            </td>
            <td style="text-align: right;">
                <asp:DropDownList runat="server" ID="ddlOfferVersion" DataValueField="OfferVersionId" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlOfferVersion_SelectedIndexChanged">
                    <asp:ListItem Value="0">--- nowa wersja ---</asp:ListItem>
                </asp:DropDownList><br />
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:LinkButton runat="server" ID="btnDownload" Text="Pobierz do pliku" OnClick="btnDownload_Click"></asp:LinkButton>

                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnDownload" />
                    </Triggers>
                </asp:UpdatePanel>

            </td>
        </tr>
        <tr>
            <td>Status oferty</td>
            <td>
                <asp:DropDownList ID="ddlOfferStatus" DataTextField="Name" DataValueField="OfferStatusId" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Klient</td>
             
            <td>
                <asp:TextBox runat="server" ID="txbContactName" MaxLength="256"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Kontakt</td>
            <td>
                <asp:TextBox runat="server" ID="txbEmail" MaxLength="256"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                    TargetControlID="txbEmail"
                    WatermarkText="adres email"
                    WatermarkCssClass="watermarked" />
                <asp:TextBox runat="server" ID="txbPhone" MaxLength="256"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
                    TargetControlID="txbPhone"
                    WatermarkText="numer telefonu"
                    WatermarkCssClass="watermarked" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:CheckBox runat="server" ID="chbShowCode" Text="Pokaż kod produktu" />
                <asp:CheckBox runat="server" ID="chbShowSupplier" Text="Pokaż dostawcę" />
                <asp:CheckBox runat="server" ID="chbOfferHeader" Checked="true" Text="Pokaż nagłówek i stopkę oferty" />
 <asp:CheckBox runat="server" ID="chbOfferFooter" Checked="true" Text="Dołącz stopkę z warunkami oferty" />


            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button runat="server" ID="btnAdd" OnClick="btnSave_Click" Text="Zapisz" /></td>
        </tr>
    </table>

    <script type="text/javascript">

        function goAutoCompl() {
            $("#<%=txbProductCode.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/AutoComplete.asmx/GetProducts") %>',
                        data: "{ 'query': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('|')[0],
                                    val: item.split('|')[1]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hfProductCatalogId.ClientID %>").val(i.item.val);
                },
                minLength: 1
            });

            }

            $(document).ready(function () {
                goAutoCompl();
            });
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                goAutoCompl();
            }

    </script>
    <div style="text-align: right">
        <asp:UpdatePanel runat="server" ID="upNewProduct">
            <ContentTemplate>
                Kod/Ean/Nazwa:
                    <asp:TextBox runat="server" ID="txbProductCode" MaxLength="50" Width="150"></asp:TextBox><asp:Button ID="btnProductAdd"
                        runat="server" OnClick="btnProductAdd_Click" Text="Dodaj produkt" />&nbsp;&nbsp;&nbsp;
    <asp:HiddenField ID="hfProductCatalogId" runat="server" />


                <span style="position: absolute; width: 10px;">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upNewProduct">
                        <ProgressTemplate>
                            <img src="Images/progress.gif" style="height: 20px" alt="" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </span>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <br />
    <br />
    <asp:GridView runat="server" ID="gvOfferProducts" AutoGenerateColumns="false" AllowSorting="false" ShowHeader="false"
        DataKeyNames="Id" Style="width: 100%" ShowFooter="true" OnRowDataBound="gvOfferProducts_RowDataBound">
        <RowStyle HorizontalAlign="left" />
        <Columns>
            <asp:TemplateField HeaderText="Lp" ItemStyle-Width="20px">
                <ItemTemplate>
                    <asp:Literal ID="Literal1" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>.
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Produkt" ItemStyle-Width="160px">
                <ItemTemplate>
                    <asp:HyperLink runat="server" id="hlImage" NavigateUrl="~/ProductCatalog.Preview.aspx?id={0}" Target="_blank"><asp:Image ID="imgImage" runat="server" /></asp:HyperLink>


                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Produkt" >
                <ItemTemplate>
                    <asp:HiddenField runat="server" ID="hidOfferProductId" />
                    <table style="width: 100%">
                        <tr>
                            <td colspan="3" style="text-align: center; font-weight: bold">
                                <asp:Label ID="lblProductCatalogSupplier" runat="server"></asp:Label>
                                /
                                <asp:Label ID="lblProductCatalogCode" runat="server"></asp:Label><br />
                                <asp:Label ID="lblProductCatalogName" runat="server"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td style="width:80px">Nazwa</td>
                            <td style="width:80px"></td>
                            <td>
                                <asp:TextBox runat="server" ID="txbOfferName" Width="350" onclick="this.select();"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
                                    TargetControlID="txbOfferName"
                                    WatermarkText="pozostaw domyślną"
                                    WatermarkCssClass="watermarked" />
                            </td>
                        </tr>
                        <tr>
                            <td>Cena</td>
                            <td>
                                <asp:Label ID="lblProductCatalogPrice" runat="server"></asp:Label></td>
                            <td>
                                <asp:TextBox runat="server" ID="txbOfferPrice" Width="350" Style="text-align: right;" onclick="this.select();"></asp:TextBox>zł<asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server"
                                    TargetControlID="txbOfferPrice"
                                    WatermarkText="pozostaw domyślną"
                                    WatermarkCssClass="watermarked" />
                            </td>
                        </tr>
                        <tr>
                            <td>L.szt.</td>
                            <td style="text-align:center">-</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbOfferQuantity" Width="350" Style="text-align: right;" onclick="this.select();"></asp:TextBox>szt.</td>
                        </tr>
                        <tr>
                            <td>Rabat</td>
                            <td style="text-align:center">-</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbOfferRebate" Width="350" Style="text-align: right;" onclick="this.select();"></asp:TextBox>%</td>
                        </tr>
                        <tr>
                            <td>Uwagi</td>
                            <td style="text-align:center">-</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbComment" Width="350" Style="text-align: right;" onclick="this.select();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td><asp:Label runat="server" ID="lblAdditionlInfo"></asp:Label> </td>
                        </tr>
                    </table>

                    <div style="text-align: right">
                        <asp:ImageButton runat="server" ID="ibtnDelete" ImageUrl="~/Images/false.jpg" Width="20" ToolTip="Usuń" OnClick="ibtnDelete_Click" /></div>
                </ItemTemplate>
                <FooterStyle HorizontalAlign="Right" Font-Size="Large" />
                <FooterTemplate>
                    <asp:Label runat="server" ID="litTotalFooter"></asp:Label>
                </FooterTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView><br /><br />
    <div>
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Button runat="server" OnClientClick="return confirm('Czy zapisać wprowadzone zmiany?');" ID="btnSaveOfferProducts" Text="Zapisz produkty" OnClick="btnSaveOfferProducts_Click" />
                    <asp:Button runat="server" OnClientClick="return confirm('Utworzyć nową wersję oferty?');" ID="btnNewOfferVersion" Text="Dodaj nową wersję oferty" OnClick="btnNewOfferVersion_Click" /></td>
                <td style="text-align: right;">
                    <asp:Button runat="server" OnClientClick="return confirm('Zapisać i zablokować ofertę?');" ID="btnLock" Text="Zapisz i zablokuj ofertę" ForeColor="Red" OnClick="btnLock_Click" />
                    <asp:Button runat="server" OnClientClick="return confirm('Utworzyć nową wersję oferty z bieżącej?');" ID="btnDuplicate" Text="Stwórz nową wersję oferty z bieżącej" ForeColor="green" OnClick="btnDuplicate_Click" /><br /><br />
                    
                    <asp:Button runat="server" OnClientClick="return confirm('Utworzyć zamówienie na podstawie bieżącej oferty?');" ID="btnOrder" Text="Stwórz zamówienie z oferty" ForeColor="green" OnClick="btnOrder_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
