<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ProductCatalog.Delivery.aspx.cs"
    Inherits="LajtIt.Web.ProductDelivery" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<%@ Register Src="~/Controls/NewDeliveryMove.ascx" TagName="NewDeliveryMove" TagPrefix="uc" %>
<%@ Register Src="~/Controls/ProductOrderControl.ascx" TagName="ProductOrderControl" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <uc:ProductMenu runat="server" SetTab="td3"></uc:ProductMenu>
    <asp:Panel ID="Panel1" runat="server" GroupingText="Dostawy">

        <asp:Panel runat="server" ID="pnlControlQuantities">
            <table>

           
                <tr>
               
                    
                    <td>
                        <table style="padding: 5px;">
                            <tr style="text-align: center">
                                <td style="padding: 5px; border: solid 1px silver;"><b>Na stanie</b></td>
                                <td style="padding: 5px; border: solid 1px silver;">Sprzedano</td>
                                <td style="padding: 5px; border: solid 1px silver;">Przyjęto</td>
                                <td style="padding: 5px; border: solid 1px silver;">Zablokowano</td>  
                                <td style="padding: 5px; border: solid 1px silver;">Przewodnia</td>
                                <td style="padding: 5px; border: solid 1px silver;">Pabianicka</td>
                                <td style="padding: 5px; border: solid 1px silver;">Ekspozycja</td>
                                <td style="padding: 5px; border: solid 1px silver;">
                                    <asp:Literal runat="server" ID="litTotalWh4" Text="U producenta"></asp:Literal></td>
                            </tr>
                            <tr style="text-align: center">
                                <td>
                                    <b><asp:Label ID="lblLeftQuantity" runat="server"></asp:Label></b></td>
                                <td>
                                    <asp:Label ID="lblSold" runat="server"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblTotalQuantity" runat="server"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblBlocked" runat="server"></asp:Label></td> 
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
            </table>

            <uc:ProductOrderControl ID="ucProductOrderControl" runat="server"></uc:ProductOrderControl>
            <uc:NewDeliveryMove ID="ucNewDeliveryMove" runat="server"></uc:NewDeliveryMove>
            <div style="text-align: right;">
                <asp:LinkButton runat="server" id="lbtnNewDelivery" Text="dodaj nową dostawę" OnClick="lbtnNewDelivery_Click"></asp:LinkButton>
            </div>
            <asp:Label runat="server" ID="lblOK"></asp:Label>
            <asp:ModalPopupExtender ID="mpeDelivery" runat="server"
                TargetControlID="lblOK"
                PopupControlID="pnShopProducts"
                BackgroundCssClass="modalBackground"
                DropShadow="true"
                CancelControlID="btnCancel"
                PopupDragHandleControlID="Panel1" />

            <asp:Panel runat="server" ID="pnShopProducts" BackColor="White" Style="width: 700px; background-color: white; height: 450px; padding: 10px">
                <asp:Panel runat="server" ID="Panel2">
                </asp:Panel>
                <div style="width: 700px; height: 450px; overflow: scroll;">

                     
                        <asp:Panel runat="server" GroupingText="Dostawa">
                            <table>
                                <%--<tr>
                                    <td>Import
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlProductImport" AppendDataBoundItems="false"
                                            DataValueField="ImportId" DataTextField="Name">
                                            <asp:ListItem Value="0">-- brak --</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>Magazyn:</td>
                                    <td>
                                        <asp:RadioButtonList runat="server" ID="rblWarehouse1" DataTextField="Name"
                                            DataValueField="WarehouseId" RepeatDirection="Horizontal"></asp:RadioButtonList> </td>
                                </tr>
                                
                                <tr>
                                    <td>Liczba produktów
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txbQuantity" Width="350"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Liczba produktów zablokowanych
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txbQuantityBlocked" Width="350"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Cena zakupu (netto)
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txbPrice" Width="350"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Uwagi
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txbComment" TextMode="MultiLine" Rows="4" Width="350"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Zamówienie
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txbOrderId" Width="350"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Faktura</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txbInvoiceNumber" MaxLength="50" Width="350" ValidationGroup="add"></asp:TextBox>


                                        <asp:HiddenField ID="hfInvoiceNumber" runat="server" />
                                        <script type="text/javascript">

                                            function goAutoCompl() {

                                                $("#<%=txbInvoiceNumber.ClientID %>").autocomplete({
                                                    source: function (request, response) {
                                                        $.ajax({
                                                            url: '<%=ResolveUrl("~/AutoComplete.asmx/GetInvoices") %>',
                                                   data: "{ 'query': '0|" + request.term + "'}",
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
                                                        $("#<%=hfInvoiceNumber.ClientID %>").val(i.item.val);
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
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button runat="server" ID="btnDeliveryAdd" OnClick="btnDeliveryAdd_Click" Text="Zapisz dostawę"
                                            OnClientClick="return confirm('Czy zapisać nową dostawę?');" />
                    <asp:LinkButton runat="server" ID="btnCancel" Text="Anuluj" />
                                         
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                
                    <asp:Button runat="server" ID="btnOK" Text="OK" Visible="false" />
                </div>
            </asp:Panel>
        </asp:Panel>

        <asp:GridView runat="server" ID="gvDeliveries" AutoGenerateColumns="false" OnRowEditing="gvDeliveries_OnRowEditing" Width="100%"
            OnRowDataBound="gvDeliveries_RowDataBound"
            DataKeyNames="DeliveryId" OnRowCancelingEdit="gvDeliveries_OnRowCancelingEdit" OnRowUpdating="gvDeliveries_OnRowUpdating">
            <Columns>
                <asp:TemplateField HeaderText="Edytuj" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">

                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnEdit" Text="edytuj" OnClick="btnEdit_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DeliveryId" Visible="false" />
                <asp:BoundField DataField="Quantity" HeaderText="Liczba<br>produktów" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80" />
                <asp:BoundField DataField="QuantityBlocked" HeaderText="Produkty<br>wycofane" HtmlEncode="false" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="80"/>

                <asp:TemplateField HeaderText="Cena zakupu<br>(netto)" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txbPrice" Width="80"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPrice"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ImportName" HeaderText="Nazwa importu" ReadOnly="true" Visible="false" />
                <asp:BoundField DataField="InsertUser" HeaderText="Dodał" ReadOnly="true" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="InsertDate" HeaderText="Data" ReadOnly="true" DataFormatString="{0:yyyy/MM/dd HH:mm}" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center"/>
                <asp:TemplateField HeaderText="Magazyn" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:RadioButtonList runat="server" ID="rblWarehouse" DataTextField="Name" DataValueField="WarehouseId" RepeatDirection="Horizontal"></asp:RadioButtonList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblWarehouse"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Comment" HeaderText="Uwagi" />
                <asp:TemplateField HeaderText="Zam." ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" Target="_blank" ID="hlOrder" Visible="false" NavigateUrl="~/Order.aspx?id={0}"></asp:HyperLink>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txbOrderId" Width="80"></asp:TextBox>
                    </EditItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Faktura" ItemStyle-Width="200" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" Target="_blank" ID="hlInvoice"   NavigateUrl="~/Deliveries.aspx?costId={0}"></asp:HyperLink>
                    </ItemTemplate>

                </asp:TemplateField>
            </Columns>
        </asp:GridView>

 


        <asp:Panel runat="server" ID="pnlDoNotControlQuantities">
            <table>
                <tr>
                    <td>Cena zakupu (netto)
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txbFixedPrice" CssClass="price_textbox" OnTextChanged="CalculateBruto" AutoPostBack="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Cena zakupu (brutto)
                    </td>
                    <td style="text-align: right;">
                        <asp:Literal runat="server" ID="litFixedPriceBrutto"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" ID="btnFixedPriceSave" Text="Zapisz" OnClick="btnFixedPriceSave_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
