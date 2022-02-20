<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductCatalogVerification.aspx.cs" Inherits="LajtIt.Web.ProductCatalogVerification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Wystawianie ofert Allegro - błędy</h1>
    <asp:GridView runat="server" ID="gvProducts" OnRowDataBound="gvProducts_RowDataBound" AllowPaging="true" PageSize="50" AutoGenerateColumns="false">
        <Columns>
<%--            <asp:TemplateField>
                <ItemStyle Width="50" HorizontalAlign="Right" />
                <ItemTemplate>
                    <itemtemplate><asp:Literal runat="server" ID="liId"></asp:Literal></itemtemplate>
                    <asp:Literal runat="server" ID="LitId"></asp:Literal>
                    <asp:CheckBox runat="server" ID="chbOrder" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox runat="server" ID="CheckBox1" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                </HeaderTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField>
                <ItemStyle Width="120" />
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlPreview" Target="_blank" NavigateUrl="/ProductCatalogPreview.aspx?idProduct={0}&idSupplier={1}">
                        <asp:Image runat="server" ID="imgImage" Width="100" />
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Produkt">
                <ItemTemplate>
                    <div style="width: calc(100% - 70px); float: left;">
                        <asp:HyperLink runat="server" ID="hlProduct" NavigateUrl="ProductCatalog.Specification.aspx?id={0}"></asp:HyperLink><br />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Kod<br>(kod EAN)">
                <ItemStyle HorizontalAlign="Right" Width="100" />
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblCode"></asp:Label><br />
                    <asp:Label runat="server" ID="lblCodeSupplier"></asp:Label><br />
                    <asp:Label runat="server" ID="lblExternalId"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CommentSimple" />
            <asp:BoundField DataField="LastUpdateDateTime" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
        <asp:TemplateField>
            <ItemStyle Width="70" />
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="chbIsFixed" Visible="false" />
                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Naprawione?');" CommandArgument='<%# Eval("Id") %>' Text="Naprawione" OnClick="lbtnChecked_Click" />
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
