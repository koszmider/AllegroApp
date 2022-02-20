<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrdersProducts.aspx.cs" Inherits="LajtIt.Web.OrdersProducts" EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Src="~/Controls/NewDelivery.ascx" TagName="NewDelivery" TagPrefix="uc" %>
<%@ Register Src="~/Controls/OrderProductsEmail.ascx" TagName="OrderProductsEmail" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Wykaz produktów w statusie "Oczekiwanie na dostawę"</h2>
    <div>
        <asp:UpdatePanel runat="server" ID="upSuppliers">
            <ContentTemplate>
                <table>
                    <tr>
                        <td><b>Dostawca:</b></td>
                        <td colspan="3">

                            <asp:RadioButtonList ID="chbSupplierOwners" runat="server" DataValueField="SupplierOwnerId" DataTextField="Name" RepeatColumns="8"
                                RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chbSupplierOwners_SelectedIndexChanged">
                            </asp:RadioButtonList> </td>
                    </tr>
                    <tr><td></td>
                        <td colspan="3"><asp:CheckBoxList AutoPostBack="true" OnSelectedIndexChanged="chblSuppliers_OnSelectedIndexChanged" runat="server" ID="chblSupplier" RepeatDirection="Horizontal" DataTextField="Name" DataValueField="SupplierId"></asp:CheckBoxList></td>
                    </tr>
                    <tr>
                        <td><b>Status:</b></td>
                        <td>
                            <asp:CheckBoxList ID="chbOps" runat="server" DataValueField="OrderProductStatusId"
                                DataTextField="StatusName"
                                RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chblSuppliers_OnSelectedIndexChanged">
                            </asp:CheckBoxList></td>

                        <td><b>Magazyn:</b></td>
                        <td>
                            <asp:CheckBoxList ID="chbWarehouse" runat="server" DataValueField="WarehouseId"
                                DataTextField="Name"
                                RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chblSuppliers_OnSelectedIndexChanged">
                            </asp:CheckBoxList>
                       

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox runat="server" ID="chbDeliveryDateAhead" Text="Pokaż zamówienia do realizacji w ciągu nast. 2 dni" AutoPostBack="true" OnCheckedChanged="chblSuppliers_OnSelectedIndexChanged" />
                            <asp:CheckBox runat="server" ID="chbNotReady" Text="Pokaż zamówienia nowe i nieopłacone" AutoPostBack="true" OnCheckedChanged="chblSuppliers_OnSelectedIndexChanged" /></td>
                 
                        <td><b>Wysyłka:</b></td>
                        <td>
                            <asp:RadioButtonList runat="server" id="rblSendFromOurWarehouse" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chbSupplierOwners_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">dowolna</asp:ListItem>
                                <asp:ListItem Value="1">od nas</asp:ListItem>
                                <asp:ListItem Value="2">z zew. mag</asp:ListItem>
                            </asp:RadioButtonList></td></tr>
                </table>


            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Timer runat="server" ID="tmRunOnce" Enabled="false" Interval="100" OnTick="chblSuppliers_OnSelectedIndexChanged"></asp:Timer>
        <div style="position: absolute;">
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upSuppliers">
                <ProgressTemplate>
                    <img src="Images/progress.gif" style="height: 20px" alt="" />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>
    <br />
    <div style="text-align:right"><asp:LinkButton runat="server" ID="lbtn" Text="generuj raport" OnClick="lbtn_Click"></asp:LinkButton></div>
    <uc:OrderProductsEmail runat="server" ID="ucOrderProductsEmail" Visible="false" />
    <br />
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <asp:GridView runat="server" ID="gvProductCatalog" DataKeyNames="OrderProductId,OrderType" OnRowDataBound="gvProductCatalog_OnRowDataBound" AllowSorting="true"
                OnSorting="gvProductCatalog_Sorting"
                AllowPaging="true"
                PageSize="100" ShowFooter="true"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField SortExpression="product">
                        <ItemStyle Width="20" HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="liId"></asp:Literal>
                            <asp:Literal runat="server" ID="LitId"></asp:Literal>
                            <asp:CheckBox runat="server" ID="chbOrder" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="litTotal"></asp:Literal>
                        </FooterTemplate>
                        <HeaderTemplate>
                            <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                        </HeaderTemplate>
                    </asp:TemplateField>

                    <asp:ImageField DataImageUrlField="ImageFullName" ControlStyle-Width="50" DataImageUrlFormatString="/images/productcatalog/{0}" SortExpression="product"
                        HeaderText="Zdjęcie" />
                    <asp:HyperLinkField DataNavigateUrlFormatString="/ProductCatalog.Preview.aspx?id={0}" Target="_blank" SortExpression="product"
                        DataTextField="Name" DataNavigateUrlFields="ProductCatalogId" HeaderText="Produkt" />
                    <asp:TemplateField HeaderText="Zamówienie" SortExpression="order" >
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlOrder" NavigateUrl="~/Order.aspx?id={0}" Target="_blank"></asp:HyperLink><br />
                            <asp:Label runat="server" ID="lblWarehouse" Text="Zamówienie wew."></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Code" HeaderText="Kod" SortExpression="code" />
                    <asp:TemplateField HeaderText="Dost.">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" Enabled="false" ID="chbIsAvailable" /><br />
                            <asp:Label runat="server" ID="lblQuantity" ToolTip="Na stanie | Dostępność u producenta"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SupplierName" HeaderText="Dostawca" SortExpression="supplier" />
                    <asp:TemplateField HeaderText="Status produktu" SortExpression="status" ItemStyle-Width="110">
                        <ItemTemplate> 
                            <asp:Label runat="server" ID="lblStatus"  ></asp:Label><br />
                            <asp:LinkButton runat="server" ID="lbtnOrderBatch" Visible="true" OnClick="lbtnOrderBatch_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Quantity" HeaderText="L.szt." ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="DeliveryDate" HeaderText="Dekl.data" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center" SortExpression="date" />
                    <asp:BoundField DataField="OrderDate" HeaderText="Data.zam" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center" SortExpression="dateOrder" />
                    <asp:BoundField DataField="Comment" HeaderText="Komentarz" />
     
                </Columns>
            </asp:GridView>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlOrderProductStatus" DataTextField="StatusName" DataValueField="OrderProductStatusId"></asp:DropDownList>
                        <asp:Button runat="server" ID="btnChangeStatus" OnClick="btnChangeStatus_Click" OnClientClick="return confirm('Czy chcesz zamienić status wybranych produktów?');" Text="Zmień status produktów" />

                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <img src="Images/progress.gif" style="height: 20px" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                    
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:LinkButton runat="server" ID="lbtnOrderProducts" Text="Pobierz tekst zamówienia" OnClick="lbtnOrderProducts_Click"></asp:LinkButton><br />
    
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="lbtnOrderProductsToFile" />
        </Triggers>
        <ContentTemplate>
    <asp:LinkButton runat="server" ID="lbtnOrderProductsToFile" Text="Pobierz do pliku" OnClick="lbtnOrderProductsToFile_Click"></asp:LinkButton><br />
     
    &nbsp;&nbsp;&nbsp;
    <asp:LinkButton runat="server"  ID="lbtnOrderProductsEmail" Text="Wyślij mail z zamówieniem" OnClick="lbtnOrderProductsEmail_Click" Visible="false"></asp:LinkButton>
   <asp:Label runat="server" ID="lblOK"></asp:Label>
 
            <asp:ModalPopupExtender ID="mpeFiles" runat="server"
                TargetControlID="lblOK"
                PopupControlID="pnShopProducts"
                BackgroundCssClass="modalBackground"
                DropShadow="true"
                CancelControlID="imbCancel"
                PopupDragHandleControlID="Panel1" />

            <asp:Panel runat="server" ID="pnShopProducts" GroupingText="Pliki zamówień" BackColor="White"
                Style="width: 900px; background-color: white; height: 450px; padding: 10px">

                <div style="text-align: right;">
                    <asp:ImageButton runat="server" ID="imbCancel" ImageUrl="~/Images/cancel.png" Width="20" /></div>
                <table style="padding:20px;">
                    <tr>
                        <td style="padding:20px;"><img src="https://loremflickr.com/320/240" /></td>
                        <td style="padding:20px;">
                            <asp:HyperLink runat="server" ID="hlFile1" Target="_blank">Pobierz do pliku</asp:HyperLink><br />
                            <asp:HyperLink runat="server" ID="hlFile2" Target="_blank" Visible="false">Zamówienie regularne</asp:HyperLink><br />
                        </td>
                    </tr>
                </table>


            </asp:Panel>
    
     <asp:Label runat="server" ID="Label1"></asp:Label>
 
            <asp:ModalPopupExtender ID="mpeProductOrders" runat="server"
                TargetControlID="Label1"
                PopupControlID="pnProductOrders"
                BackgroundCssClass="modalBackground"
                DropShadow="true"
                CancelControlID="ImageButton1"
                PopupDragHandleControlID="pnProductOrders" />

            <asp:Panel runat="server" ID="pnProductOrders"   BackColor="White"
                Style="width: 900px; background-color: white; height: 450px; padding: 10px">
                <div style="text-align: right;">
                    <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="~/Images/cancel.png" Width="20" /></div>
                <h2>Zamówione produkty</h2>
                <div style="overflow:scroll; height:380px;">
                <asp:GridView runat="server" ID="gvProductOrder" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="ProductName" ItemStyle-Width="300" />
                        <asp:TemplateField HeaderText="Kod produktu">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txbCode" Text='<%# Eval("Code") %>' OnClick="this.select();"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ean" ItemStyle-Width="60">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txbEan" Text='<%# Eval("Ean") %>' Width="100" OnClick="this.select();"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Quantity" HeaderText="Zamówiono" />
                        <asp:BoundField DataField="QuantityDelivered" HeaderText="Dostarczono" />
                        <asp:BoundField DataField="DeliveryDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data dostawy" />
                        <asp:BoundField DataField="Document" HeaderText="Dokument dostawy" />


                    </Columns>
                </asp:GridView>
</div>
            </asp:Panel>    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
