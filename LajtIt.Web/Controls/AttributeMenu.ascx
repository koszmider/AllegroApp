<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttributeMenu.ascx.cs"
    Inherits="LajtIt.Web.Controls.AttributeMenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<style>
    .tabSelected {
        background-color: white;
    }
</style>
    <h1>Konfigurator atrybutu </h1>

<h2> 
    <asp:HyperLink runat="server" ID="hlAttributeGroup" NavigateUrl="/productcatalog.attributes.aspx?idg={0}"></asp:HyperLink>.[<asp:Literal runat="server" ID="litAttribute"></asp:Literal>]</h2>
<table style="border: solid 1px black; font-size: 12pt; background-color: Silver; margin: 10px; width: 100%;">
    <tr>
        <td runat="server" id="td1" style="width:200px; text-align:center">
            <asp:HyperLink runat="server" ID="hl1" NavigateUrl="/ProductCatalog.Attribute.aspx?id={0}">Dane podstawowe</asp:HyperLink>
        </td>
        <td runat="server" id="td2" style="width:200px; text-align:center">
            <asp:HyperLink runat="server" ID="hl2" NavigateUrl="/ProductCatalog.Attribute.Titles.aspx?id={0}">Tytuły</asp:HyperLink>
        </td>
        <td runat="server" id="td3" style="width:200px; text-align:center">
            <asp:HyperLink runat="server" ID="hl3" NavigateUrl="/ProductCatalog.Attribute.ShopNames.aspx?id={0}">Nazwa w sklepie</asp:HyperLink>
        </td>
        <td runat="server" id="td4" style="width:200px; text-align:center">
            <asp:HyperLink runat="server" ID="hl4" NavigateUrl="/ProductCatalog.Attribute.ShopCategories.aspx?id={0}">Sklep kategoria</asp:HyperLink>
        </td>
        <td runat="server" id="td6" style="width:200px; text-align:center">
            <asp:HyperLink runat="server" ID="hl6" NavigateUrl="/ProductCatalog.Attribute.ShopAttribute.aspx?id={0}">Sklep atrybut</asp:HyperLink>
        </td>
        <td runat="server" id="td5" style="width:200px; text-align:center">
            <asp:HyperLink runat="server" ID="hl5" NavigateUrl="/ProductCatalog.Attribute.AllegroMapping.aspx?id={0}">Mapowanie Allegro</asp:HyperLink>
        </td>
        <td style="text-align:right">
            <asp:LinkButton runat="server" ID="lbtnAttrbiuteDelete" ForeColor="Red" Text="Usuń" OnClick="lbtnAttrbiuteDelete_Click"></asp:LinkButton>

        </td>
    </tr>
</table>
<asp:Panel runat="server" ID="pnNotAvailable" Visible="false">
    Zawartość niedostępna, nie jest konfigurowalna na tym poziomie.

</asp:Panel>
<asp:Panel runat="server" ID="pnNotExists" Visible="false">
    Atrybut nie istnieje lub został usunięty. <a href="/ProductCatalog.Attributes.aspx">Wróć do listy atrybutów</a>

</asp:Panel>

 <asp:Label runat="server" ID="lblOK"></asp:Label>
 
    <asp:ModalPopupExtender ID="mpeAttribute" runat="server"
    TargetControlID="lblOK"
    PopupControlID="pnAttribute"
    BackgroundCssClass="modalBackground"
    DropShadow="true"
    CancelControlID="imbCancel"
    PopupDragHandleControlID="Panel1" />

<asp:Panel runat="server" ID="pnAttribute" GroupingText="Usuwanie atrybutu" BackColor="White" 
    Style="width: 700px; background-color: white; height: 350px; padding: 10px">
    <div style="text-align:right; "><asp:ImageButton runat="server" ID="imbCancel" ImageUrl="~/Images/cancel.png" Width="20" /></div>

    <asp:Label runat="server" ID="lblInfo"></asp:Label><br /><br />
    <asp:Button runat="server" CausesValidation="false" OnClientClick="return confirm('Usunąć atrybut?');" 
        Text="Usuń" OnClick="btnDelete_Click" ID="btnDelete" />

</asp:Panel>