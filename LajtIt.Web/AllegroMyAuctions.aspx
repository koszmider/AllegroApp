<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllegroMyAuctions.aspx.cs" Inherits="LajtIt.Web.AllegroMyAuctions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView runat="server" ID="gvAuctions" AutoGenerateColumns="false"  OnRowDataBound="gvAuctions_OnRowDataBound">
        <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlPreview" Target="_blank" NavigateUrl="/ProductCatalogPreview.aspx?idProduct={0}&idSupplier={1}&preview=0">
                                <asp:Image runat="server" ID="imgImage" Width="100" /></asp:HyperLink></ItemTemplate>
                    </asp:TemplateField>
            <asp:TemplateField HeaderText="Produkt">
                <ItemStyle Width="300" />
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlProduct" NavigateUrl="Product.aspx?id={0}"></asp:HyperLink><br />
                    <asp:HyperLink runat="server" ID="hlProductAllegro" Target="_blank" NavigateUrl="http://allegro.pl/show_item.php?item={0}"></asp:HyperLink><br />
                    <asp:HyperLink runat="server" ID="hlAllegroItem" Target="_blank" NavigateUrl="http://allegro.pl/show_item.php?item={0}"></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kod/<br>Kod dostawcy">
                        <ItemStyle HorizontalAlign="Right" Width="100" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCode"></asp:Label> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Wystawiono/<br>w magazynie">
                        <ItemStyle HorizontalAlign="Right" Width="80" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblQuantity"></asp:Label>/
                            <asp:Label runat="server" ID="lblLeftQuantity"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
               
                    <asp:TemplateField HeaderText="Aktualna cena:<br>na Allegro/w magazynie">
                        <ItemStyle HorizontalAlign="Right"  Width="120"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblAllegroPrice"></asp:Label>/
                            <asp:Label runat="server" ID="lblSellPrice"></asp:Label><br />
                         Ofert: <asp:Label runat="server" ID="lblBids"></asp:Label>,  <asp:Label runat="server" ID="lblHighBidder"></asp:Label><br />
                             
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Zysk/strata">
                        <ItemStyle HorizontalAlign="Right" Width="120" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblIncome"></asp:Label>
                            <br />
                            <div style="color: gray; font-size: 80%;">koszt:
                                <asp:Label runat="server" ID="lblAllegroCost"></asp:Label><br />cena zakupu:
                                <asp:Label runat="server" ID="lblProductCost"></asp:Label></div>
                        </ItemTemplate>
                    </asp:TemplateField>
            <asp:TemplateField HeaderText="Koniec">
                        <ItemStyle HorizontalAlign="Right"  Width="120"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblEndDateTime" style="font-size:10pt;"></asp:Label>
                            <asp:Label runat="server" ID="lblEndHours"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
        </Columns>
    </asp:GridView>
</asp:Content>
 