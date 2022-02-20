<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="LajtIt.Web.Inventory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Inwentaryzacja</h1>
    <asp:Panel runat="server" GroupingText="Szukaj">

        <table>
            <tr>
                <td>  <asp:ListBox Rows="4" runat="server" ID="lbxSearchSupplier" SelectionMode="Multiple" Width="200" DataTextField="DisplayName" DataValueField="SupplierId"></asp:ListBox></td>
                <td>  <asp:ListBox Rows="4" runat="server" ID="lbxSearchWarehouse" SelectionMode="Multiple" Width="200" DataTextField="Name" DataValueField="WarehouseId"></asp:ListBox></td>
                <td><asp:Button runat="server" ID="btnSearch" Text="Szukaj" OnClick="btnSearch_Click" /></td>
            </tr>
        </table>
        </asp:Panel>
    <asp:Panel runat="server" GroupingText="Dodaj">
    <table>
        <tr>
            <td>Produkt</td>
            <td>Ilość</td>
            <td>Magazyn</td>
            <td>Uwagi</td>
        </tr>
        <tr>
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
                <asp:UpdatePanel runat="server" ID="upNewProduct">
                    <ContentTemplate>
                        Kod/Ean/Nazwa:
                    <asp:TextBox runat="server" ID="txbProductCode" MaxLength="50" Width="350" onfocus="this.select();"></asp:TextBox>
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
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbQuantity" Text="1"  TextMode="Number" Width="100"></asp:TextBox></td>
            <td>
                <asp:DropDownList runat="server" ID="ddlWarehouse" DataValueField="WarehouseId" DataTextField="Name"></asp:DropDownList></td>
            <td>
                <asp:TextBox runat="server" ID="txbComment" Width="100"></asp:TextBox></td>
            <td>
                <asp:Button runat="server" ID="btnAdd" Text="Dodaj" OnClick="btnAdd_Click" /></td>
        </tr>
    </table>
        </asp:Panel>

    <h2>Spis</h2>

    <asp:GridView runat="server" ID="gvProducts" AutoGenerateColumns="false" Width="100%" OnRowDeleting ="gvProducts_RowDeleting" DataKeyNames="Id">
        <Columns>

            <asp:HyperLinkField DataNavigateUrlFields="ProductCatalogId" DataTextField="Name" DataNavigateUrlFormatString="ProductCatalog.Preview.aspx?id={0}" Target="_blank" />
            <asp:BoundField DataField="Code" HeaderText="Kod" />
            <asp:BoundField DataField="SupplierName" HeaderText="Producent" />            
            <asp:BoundField DataField="Quantity" HeaderText="Ilość" />
            <asp:BoundField DataField="WarehouseAddress" HeaderText="Magazyn" />
            <asp:BoundField DataField="Comment" HeaderText="Uwagi" />
            <asp:BoundField DataField="InsertDate" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="Data" ItemStyle-Width="100" />
            <asp:CommandField ShowDeleteButton="true" DeleteText="usuń"  ItemStyle-Width="40" />
        </Columns>

    </asp:GridView>


    <h2>Porównanie</h2>
    <table>
        <tr>
            <td><asp:CheckBox runat="server" ID="chbNotMatch" Text="Pokaż niezgodne stany" AutoPostBack="true" OnCheckedChanged="btnSearch_Click" /></td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="gvInventorySummary" AutoGenerateColumns="false" Width="100%" OnRowDeleting ="gvProducts_RowDeleting" DataKeyNames="ProductCatalogId" OnRowDataBound="gvInventorySummary_RowDataBound">
        <Columns>

            <asp:HyperLinkField DataNavigateUrlFields="ProductCatalogId" DataTextField="Name" DataNavigateUrlFormatString="http://192.168.0.107/ProductCatalog.Delivery.aspx?id={0}" Target="_blank" />
            <asp:BoundField DataField="Code" HeaderText="Kod" />
            <asp:BoundField DataField="SupplierName" HeaderText="Producent" />            
            <asp:BoundField DataField="Quantity" HeaderText="Ilość wg spisu" />
            <asp:BoundField DataField="LeftQuantity" HeaderText="Ilość wg systemu" />
            <asp:BoundField DataField="QuantityForClient" HeaderText="Dla klienta" />
        </Columns>

    </asp:GridView>
</asp:Content>
