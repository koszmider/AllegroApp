<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductOptions.ascx.cs" Inherits="LajtIt.Web.Controls.ProductOptions" %>
<asp:GridView runat="server" ID="gvProducts" AutoGenerateColumns="false" OnRowDataBound="gvProducts_RowDataBound" DataKeyNames="ProductCatalogOptionId">
    <Columns>
        <asp:TemplateField>
            <ItemStyle Width="50" HorizontalAlign="Right" />
            <ItemTemplate> 
                            <itemtemplate><asp:Literal runat="server" ID="liId"></asp:Literal></itemtemplate>
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
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="hlProduct" Target="_blank">
                    <asp:Image runat="server" ID="imgProduct" Width="50" /></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Name" HeaderText="Nazwa produktu" />
        <asp:BoundField DataField="PriceBrutto" DataFormatString="{0:C}" HeaderText="Cena" />
        <asp:CheckBoxField DataField="ShopInclude" HeaderText="Aktywny" />
    </Columns>
</asp:GridView>
