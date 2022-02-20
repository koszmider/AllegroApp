<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductsTracker.aspx.cs" Inherits="LajtIt.Web.ProductsTracker" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Monitoring produktów </h1>

    <table>
        <tr>
            <td>Dostawca</td>
            <td>Produkt</td>
            <td>Max liczba produktów</td>
            <td>Promocja</td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="ddlSuppliers" DataValueField="SupplierId" DataTextField="Name"></asp:DropDownList>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbSearchName" MaxLength="50" Width="200"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
                    TargetControlID="txbSearchName"
                    WatermarkText="Nazwa/Tytuł/Kod produktu/Ean"
                    WatermarkCssClass="watermarked" /></td>
            <td>
                <asp:TextBox runat="server" ID="txbQuantity" MaxLength="2" Text="10" TextMode="Number"></asp:TextBox>
            </td>
            <td><asp:DropDownList runat="server" ID="ddlPromotion">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>TAK</asp:ListItem>
                <asp:ListItem>NIE</asp:ListItem>
                </asp:DropDownList></td>
            <td>
                <asp:Button runat="server" Text="Szukaj" OnClick="btnSearch_Click" ID="btnSearch" /></td>
        </tr>
    </table>


    <asp:GridView runat="server" ID="gvProductCatalog" DataKeyNames="ProductCatalogId" Width="100%" BackColor="white"
        AllowPaging="true" OnPageIndexChanging="gvProductCatalog_OnPageIndexChanged"
        PageSize="50" ShowFooter="true" OnRowDataBound="gvProductCatalog_OnRowDataBound"
        AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField>
                <ItemStyle Width="50" HorizontalAlign="Right" />
                <ItemTemplate>
                    <itemtemplate><asp:Literal runat="server" ID="liId"></asp:Literal></itemtemplate>
                    <asp:Literal runat="server" ID="LitId"></asp:Literal>
                    <asp:CheckBox runat="server" ID="chbOrder" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle Width="120" />
                <ItemTemplate>
                    <asp:Image runat="server" ID="imgImage" Width="100" />

                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Produkt">
                <ItemTemplate>
                    <div style="width: calc(100% - 70px); float: left;">
                        <asp:HyperLink runat="server" ID="hlProduct" NavigateUrl="Product.aspx?id={0}"></asp:HyperLink>
                    </div>


                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Kod<br>(kod EAN)">
                <ItemStyle HorizontalAlign="Right" Width="100" />
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblCode"></asp:Label><br />
                    <asp:Label runat="server" ID="lblCode2"></asp:Label><br />
                    <asp:Label runat="server" ID="lblCodeSupplier"></asp:Label><br />
                    <asp:Label runat="server" ID="lblExternalId"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ilość u Producenta">
                <ItemStyle HorizontalAlign="Center" Width="100" />
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblSupplierQuantity" Text="{0}" ToolTip="Dostępność u producenta" Visible="false"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ilość u magazynie">
                <ItemStyle HorizontalAlign="Center" Width="100" />
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblQuantity"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Cena">
                <ItemStyle HorizontalAlign="Right" Width="100" />
                <ItemTemplate>

                    <table style="width: 100px;">
                        <tr>
                            <td style="width: 30px;">
                                <asp:Image ToolTip="Cena katalogowa" runat="server" ID="imgCat" ImageUrl="~/Images/k.png" Width="30" /></td>
                            <td style="width: 70px; text-align: right;">
                                <asp:Label runat="server" ID="lblSellPrice"></asp:Label></td>
                        </tr>
                         
                    </table>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
