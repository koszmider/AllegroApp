<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrdersProductsDelivery.aspx.cs" Inherits="LajtIt.Web.OrdersProductsDelivery" EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Src="~/Controls/NewDelivery.ascx" TagName="NewDelivery" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div id="divPos"></div>
    <h2>Przyjmowanie dostaw</h2>
    <div>
        <asp:UpdatePanel runat="server" ID="upSuppliers">
            <ContentTemplate>
                <table>
                    <tr>
                        <td><b>Dostawca:</b></td>
                        <td colspan="3">

                            <asp:RadioButtonList ID="rblSupplierOwners" runat="server" DataValueField="SupplierOwnerId" DataTextField="Name" RepeatColumns="11"
                                RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chbSupplierOwners_SelectedIndexChanged">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>


                        <td><b>Magazyn:</b></td>
                        <td>
                            <asp:CheckBoxList ID="chbWarehouse" runat="server" DataValueField="WarehouseId"
                                DataTextField="Name"
                                RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chblSuppliers_OnSelectedIndexChanged">
                            </asp:CheckBoxList></td>
                    </tr>

                </table>


            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <br />
    <br />
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>

            <asp:Panel runat="server" ID="pnDelivery" Visible="false" GroupingText="Dokument przyjęcia towaru">
                <table>
                    <tr>
                        <td style="width: 300px">Dostawa dla</td>
                        <td>Nazwa dokumentu dostawy</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlCompany" DataValueField="CompanyId" DataTextField="Name" ValidationGroup="del"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox TextMode="MultiLine" Rows="3" Columns="80" runat="server" ID="txbDeliveryDocument" ValidationGroup="del"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" ValidationGroup="del" Text="Wybierz firmę" ControlToValidate="ddlCompany"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator runat="server" ValidationGroup="del" Text="Wprowadź numer dokumentu" ControlToValidate="txbDeliveryDocument"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button runat="server" ID="btnAddDelivery" ValidationGroup="del" OnClick="btnAddDelivery_Click" Visible="false" Text="Zapisz dostawy" />
                            <asp:LinkButton runat="server" ID="lbtnNewDelivery" Text="Anuluj przypisanie dostawy" Visible="false" OnClick="lbtnNewDelivery_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td></td>
                                    <td style="background-color: lightgray">dostawa pabianicka</td>
                                    <td style="background-color: orange">wysyłka z mag zew.</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </asp:Panel>

            <asp:GridView runat="server" ID="gvProductCatalog" DataKeyNames="OrderProductId,OrderType" OnRowDataBound="gvProductCatalog_OnRowDataBound" AllowSorting="true"
                OnSorting="gvProductCatalog_Sorting"
                AllowPaging="true"
                PageSize="200" ShowFooter="true"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField SortExpression="product">
                        <ItemStyle Width="20" HorizontalAlign="Center" />
                        <ItemTemplate>
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
                    <asp:BoundField DataField="Quantity" HeaderText="L.szt." ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Code" HeaderText="Kod" SortExpression="code" ItemStyle-Width="100" />
                    <asp:BoundField DataField="Comment" HeaderText="Komentarz" ItemStyle-Width="200" />
                    <asp:HyperLinkField DataNavigateUrlFormatString="/ProductCatalog.Preview.aspx?id={0}" Target="_blank" SortExpression="product"
                        DataTextField="Name" DataNavigateUrlFields="ProductCatalogId" HeaderText="Produkt" />
                    <asp:TemplateField HeaderText="Zamówienie" SortExpression="order">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlOrder" NavigateUrl="~/Order.aspx?id={0}" Target="_blank"></asp:HyperLink><br />
                            <asp:Label runat="server" ID="lblWarehouse" Text="Zamówienie wew."></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:BoundField DataField="SupplierName" HeaderText="Dostawca" SortExpression="supplier" />
                    <asp:BoundField DataField="DeliveryDate" HeaderText="Dekl.data" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center" SortExpression="date" />
                    <asp:TemplateField HeaderText="Akcje">
                        <ItemTemplate>
                            <uc:NewDelivery runat="server" ID="ucNewDelivery"></uc:NewDelivery>
                        </ItemTemplate>

                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <table style="width: 100%">
                <tr>

                    <td style="text-align: right">
                        <asp:Button runat="server" ID="btnNewDelivery" OnClick="btnNewDelivery_Click" Text="Wybierz produkty do dostawy" Visible="false" />
                    </td>
                </tr>
            </table>
            
            <table style="width: 100%">
            <tr>
                <td>
                    Data dostawy:
                </td>
                <td>
                    Wybierz miesiące (od - do (niezależnie od podanego dnia miesiąca raporty generowane będą od 1go dnia miesiąca (od) do ostatniego dnia miesiąca (do))):
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txbDeliveryDate" TextMode="Date"></asp:TextBox>
                    <asp:Button runat="server" ID="btnChangeDate" Text="Pokaż" OnClick="btnChangeDate_Click" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="TextBox13" TextMode="Date"></asp:TextBox>
                    <asp:TextBox runat="server" ID="TextBox23" TextMode="Date"></asp:TextBox>
                    <asp:LinkButton runat="server" ID="Button13" Text="Generuj miesięczne raporty" OnClick="btn13ChangeDate_Click"></asp:LinkButton>
                </td>
            </tr>
            </table>
            <asp:Panel runat="server" ID="pnOrdersToSend" GroupingText="Produkty do wysyłki">
                <asp:GridView runat="server" ID="gvOrdersToSend" AutoGenerateColumns="false">
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFormatString="/ProductCatalog.Preview.aspx?id={0}" Target="_blank" DataNavigateUrlFields="ProductCatalogId" DataTextField="ProductName" HeaderText="Nazwa produktu" />
                        <asp:BoundField ItemStyle-Width="50" DataField="Quantity" HeaderText="Liczba szt." />
                        <asp:BoundField ItemStyle-Width="100" DataField="SupplierName" HeaderText="Dostawca" />
                        <asp:BoundField ItemStyle-Width="100" DataField="Code" HeaderText="Kod produktu" />
                        <asp:HyperLinkField ItemStyle-Width="100" DataNavigateUrlFormatString="/OrderProcessing.aspx?id={0}" Target="_blank" DataNavigateUrlFields="OrderId" DataTextField="StatusName" HeaderText="Status zamówienia" />
                        <asp:BoundField ItemStyle-Width="100" DataField="InsertDate" HeaderText="Data dost." />
                        <asp:BoundField ItemStyle-Width="100" DataField="InsertUser" HeaderText="Dostawę przyjął" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnOrdersToPabianicka" GroupingText="Produkty na Pabianicką">

                <asp:GridView runat="server" ID="gvOrdersToPabianicka" AutoGenerateColumns="false">
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFormatString="/ProductCatalog.Preview.aspx?id={0}" Target="_blank" DataNavigateUrlFields="ProductCatalogId" DataTextField="ProductName" HeaderText="Nazwa produktu" />
                        <asp:BoundField ItemStyle-Width="50" DataField="Quantity" HeaderText="Liczba szt." />
                        <asp:BoundField ItemStyle-Width="100" DataField="SupplierName" HeaderText="Dostawca" />
                        <asp:BoundField ItemStyle-Width="100" DataField="Code" HeaderText="Kod produktu" />
                        <asp:HyperLinkField ItemStyle-Width="100" DataNavigateUrlFormatString="/OrderProcessing.aspx?id={0}" Target="_blank" DataNavigateUrlFields="OrderId" DataTextField="StatusName" HeaderText="Status zamówienia" />
                        <asp:BoundField ItemStyle-Width="100" DataField="InsertDate" HeaderText="Data dost." />
                        <asp:BoundField ItemStyle-Width="100" DataField="InsertUser" HeaderText="Dostawę przyjął" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnOrdersToWarehouse" GroupingText="Produkty do magazynu">

                <asp:GridView runat="server" ID="gvWarehouse" AutoGenerateColumns="false">
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFormatString="/ProductCatalog.Preview.aspx?id={0}" Target="_blank" DataNavigateUrlFields="ProductCatalogId" DataTextField="ProductName" HeaderText="Nazwa produktu" />
                        <asp:BoundField ItemStyle-Width="50" DataField="Quantity" HeaderText="Liczba szt." />
                        <asp:BoundField ItemStyle-Width="100" DataField="SupplierName" HeaderText="Dostawca" />
                        <asp:BoundField ItemStyle-Width="100" DataField="Code" HeaderText="Kod produktu" />

                        <asp:BoundField ItemStyle-Width="100" DataField="InsertDate" HeaderText="Data dost." />
                        <asp:BoundField ItemStyle-Width="100" DataField="InsertUser" HeaderText="Dostawę przyjął" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
