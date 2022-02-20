<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ExportForShipping.aspx.cs" Inherits="LajtIt.Web.ExportForShipping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr valign="top">
            <td>Dostawca:<br />
                <asp:DropDownList runat="server" ID="ddlShippingCompany" DataTextField="Name" DataValueField="ShippingCompanyId">
                </asp:DropDownList>
                <asp:Button runat="server" ID="btnSearch" Text="Szukaj" OnClick="btnSearch_Click" />
            </td>
            <td>Batch<br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lbtnExportPath" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:DropDownList runat="server" ID="lsbOrderBatch" DataTextField="BatchName" DataValueField="OrderExportBatchId"
                            Rows="4">
                        </asp:DropDownList>
                        <asp:LinkButton runat="server" ID="lbtnExportPath" Text="Pobierz plik" OnClick="lbtnExportPath_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="gvExport" EmptyDataText="Brak zamówień do exportu"
        DataKeyNames="OrderId" AutoGenerateColumns="false" OnRowDataBound="gvExport_OnRowDataBound">
        <Columns>
            <asp:TemplateField>
                <ItemStyle Width="60" HorizontalAlign="Right" />
                <ItemTemplate>
                    <asp:Literal runat="server" ID="LitId"></asp:Literal>
                    <asp:CheckBox runat="server" ID="chbOrder" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField DataNavigateUrlFields="OrderId" DataTextField="OrderId" DataNavigateUrlFormatString="Order.aspx?id={0}"
                HeaderText="Id" />
            <asp:BoundField DataField="Client" HeaderText="Klient" HtmlEncode="false" />
            <asp:BoundField DataField="ShippingType" HeaderText="Rodzaj przesyłki" />
            <asp:TemplateField HeaderText="Kwota pobrania">
                <ItemTemplate>
                    <asp:Literal ID="litAmountToPay" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ShippingData" HeaderText="Parametry przesyłki" />
            <asp:BoundField DataField="CreateDate" HeaderText="Data zamówienia" DataFormatString="{0:yy-MM-dd HH:mm}" />
            <asp:BoundField DataField="StatusDate" HeaderText="Data statusu" DataFormatString="{0:yy-MM-dd HH:mm}" />
        </Columns>
    </asp:GridView>
    <asp:Button runat="server" Visible="false" ID="btnExport" Text="Export" OnClick="btnExport_Click"
        OnClientClick="return confirm('Wyeksportować?');" />
</asp:Content>
