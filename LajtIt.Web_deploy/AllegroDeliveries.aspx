<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllegroDeliveries.aspx.cs" Inherits="LajtIt.Web.AllegroDeliveries" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Cenniki dostaw Allegro</h1>
<div style="text-align:right"><a href="AllegroDeliveryCost.aspx">Dodaj nowy</a></div>
    <asp:GridView runat="server" ID="gvDeliveries" AutoGenerateColumns="false">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="DeliveryCostTypeId" DataTextField="Name"
                DataNavigateUrlFormatString="AllegroDeliveryCost.aspx?id={0}" HeaderText="Cennik dostawy" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Aktywny" ItemStyle-HorizontalAlign="Center" />
            <asp:CheckBoxField DataField="IsPaczkomatAvailable" HeaderText="Paczkomat"  ItemStyle-HorizontalAlign="Center"/>
            <asp:BoundField DataField="Number" ItemStyle-HorizontalAlign="Center"
                HeaderText="L.dostawców z cennikiem" /> 
        </Columns>
    </asp:GridView>
</asp:Content>
