<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ExportBatches.aspx.cs" Inherits="LajtIt.Web.ExportBatches" %>

<%@ Register Src="~/Controls/ItemOrderGrid.ascx" TagName="ItemOrderGrid" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table>
            <tr valign="top">
                <td>
                    BatchId<br />
                    <asp:ListBox runat="server" ID="lsbOrderBatch" SelectionMode="Multiple" DataTextField="BatchName"
                        DataValueField="OrderExportBatchId" Rows="4"></asp:ListBox>
                </td>
                <td>
                    Dostawa:<br />
                    <asp:CheckBoxList RepeatColumns="3" runat="server" ID="chblShippingCompany" DataTextField="Name"
                        DataValueField="ShippingCompanyId">
                    </asp:CheckBoxList>
                    <asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnShowAll" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:Button runat="server" ID="btnShowAll" OnClick="btnShowAll_Click" Text="Pokaż wszystkie" />
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel runat="server" ID="pActions"   Visible="false">
        <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPrintParagons" />
                <asp:PostBackTrigger ControlID="btnPrintInvoices" />
                <asp:PostBackTrigger ControlID="btnPrintOrders" />
            </Triggers>
        </asp:UpdatePanel>
        <table style="width:100%">
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnShowProducts" OnClick="btnShowProducts_Click" Text="Pokaż produkty" /><asp:CheckBox
                        runat="server" ID="chbShowSent" Text="Pokaż również wysłane" />
                </td>
                <td style="text-align: right;">
                    <asp:Button runat="server" ID="btnPrintInvoices" OnClick="btnPrintInvoices_Click"
                        Text="Drukuj faktury" OnClientClick="return confirm('Czy chcesz wydrukować faktury dla wybranych batch id?');" />
                    <asp:Button runat="server" ID="btnPrintParagons" OnClick="btnPrintParagons_Click"
                        Text="Drukuj paragony" OnClientClick="return confirm('Czy chcesz wydrukować paragony dla wybranych batch id?');" />
                    <asp:Button runat="server" ID="btnPrintOrders" OnClick="btnPrintOrders_Click"
                        Text="Drukuj zamówienia" OnClientClick="return confirm('Czy chcesz wydrukować paragony dla wybranych batch id?');" />
                    <asp:Button runat="server" ID="btnShowInvoices" OnClick="btnShowInvoices_Click" Text="Pokaż faktury" />
                    <asp:Button runat="server" ID="Button1" OnClick="btnShowOrderDetails_Click" Text="Pokaż zamówienia" />
                </td>
            </tr>
        </table>
    </asp:Panel>
 
    <asp:Panel ID="pProducts" runat="server" Visible="false" >
        <asp:DropDownList runat="server" ID="ddlSupplier" AutoPostBack="true" OnSelectedIndexChanged="ddlSupplier_OnSelectedIndexChanged" Visible="false">
            <asp:ListItem Value="0">-- wszyscy dostawcy --</asp:ListItem>
            <asp:ListItem Value="3">produkty pudełkowe</asp:ListItem>
            <asp:ListItem Value="1">produkty Lajt it</asp:ListItem>
        </asp:DropDownList> 
        <table style="width: 100%; ">
            <tr>
                <td style="text-align: right;"><span>
                    <asp:GridView runat="server" ID="gvInvoices" EnableViewState="false" AutoGenerateColumns="false" Width="500">
                        <Columns>
                            <asp:HyperLinkField HeaderText="Id" ItemStyle-Width="40" DataNavigateUrlFields="OrderId"
                                DataTextField="OrderId" DataNavigateUrlFormatString="Order.aspx?id={0}" Target="_blank" />
                            <asp:BoundField DataField="Name" HeaderText="Klient" />
                            <asp:BoundField DataField="CompanyName" HeaderText="Firma" />
                        </Columns>
                    </asp:GridView></span>
                </td>
            </tr>
        </table>
    <asp:GridView runat="server" ID="gvProducts" EnableViewState="false" AutoGenerateColumns="false" OnRowDataBound="gvProducts_RowDataBound">
        <Columns>
            <asp:HyperLinkField HeaderText="Id" ItemStyle-Width="40"  DataNavigateUrlFields="OrderId" DataTextField="OrderId" DataNavigateUrlFormatString="OrderProcessing.aspx?id={0}#products"
                Target="_blank" /> 
            <asp:BoundField DataField="Name" HeaderText="Nazwa"  ItemStyle-Width="250"/>
            <asp:BoundField DataField="Quantity" HeaderText="Ilość" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="50"/>
            <asp:BoundField DataField="Comment" HeaderText="Uwagi"  ItemStyle-Width="200"/>
            <asp:BoundField DataField="CompanyName" HeaderText="Faktura"  ItemStyle-Width="150"/> 
        </Columns>
    </asp:GridView></asp:Panel>
    <br />
    <asp:GridView runat="server" ID="gvOrderExportView" EnableViewState="false" AutoGenerateColumns="false">
        <Columns>
        <asp:TemplateField><ItemTemplate><asp:CheckBox runat="server" /></ItemTemplate></asp:TemplateField>
            <asp:HyperLinkField HeaderText="Id" ItemStyle-Width="40"  DataNavigateUrlFields="OrderId" DataTextField="OrderId" DataNavigateUrlFormatString="Order.aspx?id={0}#products"
                Target="_blank" />
            <asp:BoundField DataField="UserName" HeaderText="Klient"  ItemStyle-Width="150" HtmlEncode="false"/>
            <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Width="100" />
            <asp:BoundField DataField="Phone" HeaderText="Telefon" ItemStyle-Width="100" />
            <asp:BoundField DataField="ShippingData" HeaderText="Dane" ItemStyle-Width="100" />
            <asp:BoundField DataField="ShippingType" HeaderText="Dostawa" ItemStyle-Width="200" />
        </Columns>
    </asp:GridView> 
    <br />
    <br />
    <div>
        <asp:GridView runat="server" ID="gvBatches" AutoGenerateColumns="false" PageSize="20"
            DataKeyNames="OrderExportBatchId" Width="100%" OnRowDataBound="gvOrders_OnRowDataBound"
            AllowPaging="true" OnPageIndexChanging="gvOrders_OnPageIndexChanging">
            <Columns>
                <asp:TemplateField>
                    <ItemStyle Width="60" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LitId" Visible="false"></asp:Literal>
                        <asp:CheckBox runat="server" ID="chbOrder" />
                    </ItemTemplate>
                    <HeaderTemplate>
                        <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this,'chbOrder');" /></HeaderTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="OrderExportBatchId" HeaderText="BatchId" HtmlEncode="false"
                    ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="InsertDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Data"
                    HtmlEncode="false" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Comment" HeaderText="Komentarz" />
                <asp:BoundField DataField="AmountToPay" HeaderText="Do zapłacenia" ItemStyle-HorizontalAlign="Right" Visible="false"
                    ItemStyle-Width="60" DataFormatString="{0:C}" />
                <asp:BoundField DataField="AmountPaid" HeaderText="Zapłacono" ItemStyle-HorizontalAlign="Right" Visible="false"
                    ItemStyle-Width="60" DataFormatString="{0:C}" />
              <%--  <asp:BoundField DataField="NumberOfInvoices" HeaderText="Faktury" ItemStyle-HorizontalAlign="center" />--%>
   <%--             <asp:BoundField DataField="ListPoleconyEkonomiczny" HeaderText="Ekon." ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="ListPoleconyPriorytetowy" HeaderText="Prior." ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="PrzesylkaKurierska" HeaderText="Kurier" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="PrzesylkaKuierskaPobraniowa" HeaderText="Pobr." ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Inne" HeaderText="Inne" ItemStyle-HorizontalAlign="center" />--%>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
