<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="Product.Status.aspx.cs" Inherits="LajtIt.Web.ProductStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<%@ Register Src="~/Controls/ProductCatalogGroup.ascx" TagName="ProductCatalogGroup" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Wyszukiwarka produktu</h1>

    <br />



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
    <asp:UpdatePanel runat="server" ID="upNewProduct">
        <ContentTemplate>
            Kod/Ean/Nazwa:
                    <asp:TextBox runat="server" ID="txbProductCode" MaxLength="50" Width="100%" onclick="this.select();"></asp:TextBox><br /><asp:Button ID="btnProductAdd"
                        runat="server" OnClick="btnProductAdd_Click" Text="Sprawdź produkt" />&nbsp;&nbsp;&nbsp;
    <asp:HiddenField ID="hfProductCatalogId" runat="server" />


            <span style="position: absolute; width: 10px;">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upNewProduct">
                    <ProgressTemplate>
                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </span>
            <br />
            <asp:Label runat="server" ID="lblProductNotExists" ForeColor="red" Visible="false" Text="Produkt nie istnieje w bazie">

            </asp:Label>
            <asp:Panel runat="server" ID="pnProduct"> <asp:HyperLink runat="server" ID="hlProduct" Target="_blank">
                                <asp:Image runat="server" ID="imgImage" Width="300" Visible="false" />
                            </asp:HyperLink>
                <table style="padding: 10px;">
                   

                    <tr>
                        <td>
                            <h2>Magazyn</h2>
                            <table style="padding: 5px;">
                                <tr style="text-align: center">
                                    <td style="padding: 5px; border: solid 1px silver;">Razem</td>
                                    <td style="padding: 5px; border: solid 1px silver;">Przewodnia</td>
                                    <td style="padding: 5px; border: solid 1px silver;">Pabianicka</td>
                                    <td style="padding: 5px; border: solid 1px silver;">Ekspozycja</td>
                                    <td style="padding: 5px; border: solid 1px silver;">
                                        <asp:Literal runat="server" ID="litTotalWh4" Text="Producent"></asp:Literal></td>
                                </tr>
                                <tr style="text-align: center">
                                    <td>
                                        <asp:Label ID="lblTotal" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTotalWh1" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTotalWh2" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTotalWh3" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTotalWh4" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h2>Gotowy i oczekujący na wysyłkę</h2>
                            <br />
                            <asp:GridView runat="server" AutoGenerateColumns="false" ID="gvReadyToSend" EmptyDataText="brak" OnRowDataBound="gvReadyToSend_RowDataBound">
                                <Columns>
                                    <asp:HyperLinkField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100" HeaderText="Zamówienie" DataTextField="OrderId" DataNavigateUrlFields="OrderId" DataNavigateUrlFormatString="/Order.aspx?id={0}" Target="_blank" />
                                    <asp:BoundField HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Center" HeaderText="Ilość" DataField="Quantity" />
                                    <asp:BoundField HeaderStyle-Width="250" HeaderText="Status zamówienia" DataField="StatusName" />
                                    <asp:BoundField HeaderStyle-Width="150" HeaderText="Wysyłka" DataField="ShippingCompanyName" />
                                    <asp:BoundField HeaderStyle-Width="150" HeaderText="Magazyn" DataField="WarehouseName" />
                                    <asp:BoundField HeaderStyle-Width="150" HeaderText="Data przyjęcia towaru" DataField="InsertDate" DataFormatString="{0:yyyy/MM/dd}" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h2>Oczekujący na inne produkty w zamówieniu</h2>
                            <br />
                            <asp:GridView runat="server" AutoGenerateColumns="false" ID="gvWaiting" EmptyDataText="brak" OnRowDataBound="gvReadyToSend_RowDataBound">
                                <Columns>
                                    <asp:HyperLinkField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100" HeaderText="Zamówienie" DataTextField="OrderId" DataNavigateUrlFields="OrderId" DataNavigateUrlFormatString="/Order.aspx?id={0}" Target="_blank" />
                                    <asp:BoundField HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Center" HeaderText="Ilość" DataField="Quantity" />
                                    <asp:BoundField HeaderStyle-Width="250" HeaderText="Status zamówienia" DataField="StatusName" />
                                    <asp:BoundField HeaderStyle-Width="150" HeaderText="Status produktu w zamówieniu" DataField="OrderProductStatusName" />
                                    <asp:BoundField HeaderStyle-Width="150" HeaderText="Magazyn" DataField="WarehouseName" />
                                    <asp:BoundField HeaderStyle-Width="150" HeaderText="Data przyjęcia towaru" DataField="InsertDate" DataFormatString="{0:yyyy/MM/dd}" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h2>Zwrot Italux</h2>
                            <br />
                            <asp:TextBox runat="server" ID="lblItalux"></asp:TextBox>
                       
                        </td>
                    </tr>
                </table>


            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
