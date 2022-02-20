<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true" CodeBehind="ProductCatalog.Marketing.aspx.cs"
    Inherits="LajtIt.Web.ProductMarketing" %>

<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upShop" runat="server">
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <uc:ProductMenu runat="server" SetTab="td11"></uc:ProductMenu>
    <asp:Panel runat="server">
        <asp:UpdatePanel runat="server" ID="upAllegro">
            <ContentTemplate> 
                <asp:GridView runat="server" CssClass="mytable" ID="gvShops" AutoGenerateColumns="false" OnRowDataBound="gvShops_RowDataBound" DataKeyNames="ShopId">
                    <Columns>
                        <asp:BoundField HeaderText="Nazwa Sklepu" DataField="ShopName" />
                        <asp:BoundField HeaderText="Id prod." DataField="ShopProductId" />
                        <asp:TemplateField HeaderText="Nazwa produktu">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>Szablon (dla sklepu):</td>
                                        <td><asp:Label runat="server" ID="lblShopTemplate" Text="brak szablonu"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Szablon (<asp:HyperLink runat="server" ID="hlProductType" Target="_blank"></asp:HyperLink>):</td>
                                        <td><asp:Label runat="server" ID="lblTemplate" Text="brak szablonu"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Nazwa dynamiczna:</td>
                                        <td><asp:Label runat="server" ID="lblName"  ></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Nazwa przypisana:</td>
                                        <td><asp:TextBox runat="server" ID="txbShopProductName" Width="400"></asp:TextBox></td></tr>
                                    <tr>
                                        <td>Blokuj nazwę:</td>
                                        <td><asp:CheckBox runat="server" ID="chbIsNameLocked" /></td></tr>
                                </table> 
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Zapisz" OnClientClick="return confirm('Zapisać zmiany?')" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
