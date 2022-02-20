<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Promotion.aspx.cs" Inherits="LajtIt.Web.Promotion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: right;">
        <asp:HyperLink runat="server" ID="hlPromotions" NavigateUrl="/Promotions.aspx" Text="Zobacz wszystkie promocje"></asp:HyperLink>
    </div>

    <asp:TabContainer runat="server">
        <asp:TabPanel runat="server"
            HeaderText="Konfiguracja">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>Nazwa</td>
                        <td>
                            <asp:TextBox runat="server" ID="txbName" MaxLength="256" Width="500"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <h2>Konfiguracja</h2>
                        </td>
                    </tr>
                    <tr>
                        <td>Aktywna</td>
                        <td>
                            <asp:CheckBox runat="server" ID="chbIsActive" /></td>
                    </tr>
                    <tr>
                        <td>Czas trwania</td>
                        <td>
                            <asp:TextBox runat="server" ID="txbDateFrom" Width="70"></asp:TextBox><asp:CalendarExtender
                                ID="calDateFrom" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateFrom">
                            </asp:CalendarExtender>
                            -<asp:TextBox runat="server" ID="txbDateTo" Width="70"></asp:TextBox><asp:CalendarExtender
                                ID="calDateTo" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateTo">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server"
            HeaderText="Warunki">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>Zastosuj dla wybranych producentów</td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:ListBox runat="server" ID="lbxSuppliers" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="SupplierId" Width="220"></asp:ListBox></td>
                                    <td>
                                        <asp:Button runat="server" ID="btnSuppliersAdd" OnClick="btnSuppliersAdd_Click" Text=">>" /><br />
                                        <asp:Button runat="server" ID="btnSuppliersDel" OnClick="btnSuppliersDel_Click" Text="<<" />
                                    </td>
                                    <td>
                                        <asp:ListBox runat="server" ID="lbxSuppliersSelected" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="SupplierId" Width="220"></asp:ListBox></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>Zastosuj w wybranych sklepach</td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:ListBox runat="server" ID="lbxShops" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="ShopId" Width="220"></asp:ListBox></td>
                                    <td>
                                        <asp:Button runat="server" ID="btnShopsAdd" OnClick="btnShopsAdd_Click" Text=">>" /><br />
                                        <asp:Button runat="server" ID="btnShopsDel" OnClick="btnShopsDel_Click" Text="<<" />
                                    </td>
                                    <td>
                                        <asp:ListBox runat="server" ID="lbxShopsSelected" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="ShopId" Width="220"></asp:ListBox></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>Warunki</td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chbPromotion" OnCheckedChanged="chbPromotion_CheckedChanged" AutoPostBack="true" /></td>
                                    <td>Promocja</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPromotion">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="0">produkty bez promocji</asp:ListItem>
                                            <asp:ListItem Value="1">produkty w promocji</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chbPriceFrom" OnCheckedChanged="chbPromotion_CheckedChanged" AutoPostBack="true"/></td>
                                    <td>Cena od</td>
                                    <td>
                                        <asp:TextBox runat="server" TextMode="Number" ID="txbPriceFrom" Value="0"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chbPriceTo" OnCheckedChanged="chbPromotion_CheckedChanged" AutoPostBack="true"/></td>
                                    <td>Cena do</td>
                                    <td>
                                        <asp:TextBox runat="server" TextMode="Number" ID="txbPriceTo"  Value="0"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>Atrybuty</td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:ListBox runat="server" ID="lbxAttributes" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="AttributeId" Width="320"></asp:ListBox></td>
                                                <td>
                                                    <asp:Button runat="server" ID="btnAttributeAdd" OnClick="btnAttributeAdd_Click" Text=">>" /><br />
                                                    <asp:Button runat="server" ID="btnAttributeDel" OnClick="btnAttributeDel_Click" Text="<<" />
                                                </td>
                                                <td>
                                                    <asp:ListBox runat="server" ID="lbxAttributesSelected" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="AttributeId" Width="320"></asp:ListBox></td>
                                            </tr>
                                        </table>
                                        Logika działania na atrybutach:<br />
                                        - atrybuty z tej samej grupy: przynajmniej jeden<br />
                                        - pomiędzy grupami: każda grupa musi być przypisana do produktu<br />
                                        <br />
                                        np wybrano: [Barwa światła].(Ciepła)[Barwa światła].(Zima)[Gwint żarówki].(E27)[Gwint żarówki].(E14) oznacza, że wybrane są produkty które mają Barwę światła ciepłą lub zimną <b>oraz </b>gwint E27 lub E14
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server"
            HeaderText="Działania">
            <ContentTemplate>
                <table>
                  <%--  <tr>
                        <td>Znak wodny na zdjęciu</td>
                        <td>
                            <asp:CheckBox runat="server" ID="chbIsWatekmarkActive" /><asp:HyperLink runat="server" ID="hlWatermark" Text="zobacz" Target="_blank">
                                <asp:Image runat="server" Width="50" ID="imgWatermark" />
                            </asp:HyperLink><br />
                            <table>
                                <tr>
                                    <td>
                                        <table style="width: 100%;">
                                            <tr valign="top">

                                                <td>
                                                    <asp:UpdatePanel runat="server">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnImgAdd" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <input type="file" id="myfile" name="myfile" runat="server" size="100" />&nbsp;&nbsp;&nbsp;
                                     
                                    <asp:Button ID="btnImgAdd" runat="server" Text="Zapisz pliki" OnClick="btnImgAdd_Click" CausesValidation="false" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>


                        </td>
                    </tr>--%>

                    <tr>
                        <td>Dodatkowy tekst</td>
                        <td>
                            <asp:TextBox runat="server" ID="txbDescription" TextMode="MultiLine" Rows="10" Columns="100"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Produkty</td>
                        <td>
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
                            <asp:GridView runat="server" ID="gvPromotionProducts" AutoGenerateColumns="false" AllowSorting="false" ShowHeader="true"
                                DataKeyNames="Id" Style="width: 100%" ShowFooter="false" OnRowDataBound="gvPromotionProducts_RowDataBound">
                                <RowStyle HorizontalAlign="left" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Lp" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Literal ID="Literal1" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>.
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Produkt" ItemStyle-Width="160px">
                                        <ItemTemplate>
                                            <asp:HyperLink runat="server" ID="hlImage" NavigateUrl="~/ProductCatalog.Preview.aspx?id={0}" Target="_blank">
                                                <asp:Image ID="imgImage" runat="server" />
                                            </asp:HyperLink>


                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Produkt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductCatalogSupplier" runat="server"></asp:Label>
                                            /
                                <asp:Label ID="lblProductCatalogCode" runat="server"></asp:Label><br />
                                            <asp:Label ID="lblProductCatalogName" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Ilość">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductCatalogPrice" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ilość">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txbOfferQuantity" Width="150" Style="text-align: right;" onclick="this.select();"></asp:TextBox>szt.</td>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Usuń">
                                        <ItemTemplate>
                                            <div style="text-align: right">
                                                <asp:ImageButton runat="server" ID="ibtnDelete" ImageUrl="~/Images/false.jpg" Width="20" ToolTip="Usuń" OnClick="ibtnDelete_Click" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server"
            HeaderText="Wybrane produkty">
            <ContentTemplate>
                <div style="text-align: right;">
                    <asp:HyperLink runat="server" ID="hlProductCatalog" Target="_blank" NavigateUrl="/ProductCatalog.aspx?PromotionId={0}" Text="Zobacz produkty"></asp:HyperLink>
                </div>
                Aktualna liczba produktów w promocji (z bieżących ustawień): <b><asp:Label runat="server" ID="lblProductsCurrentCount"></asp:Label></b><br />
                Dotychczasowa liczba produktów w promocji: <b><asp:Label runat="server" ID="lblProductsCount"></asp:Label></b><br /><br /><br />
                <asp:Button runat="server" ID="btnPromotionProductsSelect" OnClick="btnPromotionProductsSelect_Click" Text="Utwórz/odśwież listę produktów w promocji" OnClientClick="return confirm('Czy utworzyć listę produktów?');" />
            </ContentTemplate>
        </asp:TabPanel>

    </asp:TabContainer>



    <table>

        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnSave" Text="Zapisz" OnClick="btnSave_Click" OnClientClick="return confirm('Zapisać zmiany?');" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="Button1" Text="text zdjecia" OnClick="Button1_Click" Visible="false" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="Button2" Text="Twórz warianty" OnClick="Button2_Click" Visible="false"/></td>
        </tr>
    </table>
</asp:Content>
