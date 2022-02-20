<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllegroPerformance.aspx.cs" Inherits="LajtIt.Web.AllegroPerformance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Allegro - jakość</h1>
    <br /><br />
    <asp:GridView runat="server" ID="gvAllegro" AutoGenerateColumns="false" OnRowDataBound="gvAllegro_RowDataBound">
        <Columns>
            <asp:BoundField HeaderText="Konto" DataField="UserName" />
            <asp:TemplateField HeaderText="Polecam">
                <ItemStyle HorizontalAlign="Right" />
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblRecommend"></asp:Label> / <b><asp:Label runat="server" ID="lblRecommendUnique"></asp:Label> </b>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nie polecam">
                <ItemStyle HorizontalAlign="Right" />
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblNotRecommend"></asp:Label> / <b><asp:Label runat="server" ID="lblNotRecommendUnique"></asp:Label> </b>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Średnia">
                <ItemStyle HorizontalAlign="Right" />
                <ItemTemplate> 
                    <asp:Label runat="server" ID="lblAverage"></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Zgodność z opisem">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate> 
                    <asp:Label runat="server" ID="lblRateDesc"></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Obsługa kupującego">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate> 
                    <asp:Label runat="server" ID="lblRateService"></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Koszt wysyłki">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate> 
                    <asp:Label runat="server" ID="lblRateDelivery"></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Liczba producentów<br>w sklepie">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate> 
                    <asp:Label runat="server" ID="lblShopSuppliers"></asp:Label> 
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>
    * Kolorem czerwonym oznaczone wartości poniżej 98% (próg przyznania SMART)
</asp:Content>
