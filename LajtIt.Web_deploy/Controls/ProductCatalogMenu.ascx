<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductCatalogMenu.ascx.cs"
    Inherits="LajtIt.Web.Controls.ProductCatalogMenu" %>
<style>
    .tabSelected
    {
        background-color: white;
    }
</style>
    <h2>
        <asp:Literal runat="server" ID="litProductCatalog"></asp:Literal></h2>
<table style=" border: solid 1px black;   font-size:12pt; background-color:Silver; margin:10px; width:100%;">
    <tr>
        <td runat="server" id="td1"><asp:HyperLink runat="server" ID="hl1" NavigateUrl="/Product.aspx?id={0}">Dane podstawowe</asp:HyperLink>
        </td>
        <td runat="server" id="td2"><asp:HyperLink runat="server" ID="hl2" NavigateUrl="/ProductCatalog.Allegro.aspx?id={0}">Allegro</asp:HyperLink>
        </td>
         <td runat="server" id="td8"><asp:HyperLink runat="server" ID="hl8" NavigateUrl="/ProductCatalog.Shop.aspx?id={0}">Sklepy</asp:HyperLink>
        </td>
        <td runat="server" id="td14"><asp:HyperLink runat="server" ID="hl14" NavigateUrl="/ProductCatalog.Descriptions.aspx?id={0}">Opisy</asp:HyperLink>
        </td>
        <td runat="server" id="td9"><asp:HyperLink runat="server" ID="hl9" NavigateUrl="/ProductCatalog.Images.aspx?id={0}">Zdjęcia</asp:HyperLink>
        </td>
       <td runat="server" id="td3"><asp:HyperLink runat="server" ID="hl3" NavigateUrl="/ProductCatalog.Delivery.aspx?id={0}">Dostawy</asp:HyperLink>
        </td>
       <td runat="server" id="td15"><asp:HyperLink runat="server" ID="hl15" NavigateUrl="/ProductCatalog.Packing.aspx?id={0}" >Paczki</asp:HyperLink>
        </td>
        <td runat="server" id="td4"><asp:HyperLink runat="server" ID="hl4" NavigateUrl="/ProductCatalog.Specification.aspx?id={0}">Specyfikacja</asp:HyperLink>
        </td>
       <td runat="server" id="td11"><asp:HyperLink runat="server" ID="hl11" NavigateUrl="/ProductCatalog.Marketing.aspx?id={0}" >Marketing</asp:HyperLink>
        </td>
       <td runat="server" id="td12"><asp:HyperLink runat="server" ID="hl12" NavigateUrl="/ProductCatalog.Grouping.aspx?id={0}" >Powiązania</asp:HyperLink>
        </td>
       <td runat="server" id="td13"><asp:HyperLink runat="server" ID="hl13" NavigateUrl="/ProductCatalog.Combo.aspx?id={0}" >Zestawy</asp:HyperLink>
        </td>
       <td runat="server" id="td10"><asp:HyperLink runat="server" ID="hl10" NavigateUrl="/ProductCatalog.History.aspx?id={0}" >Historia</asp:HyperLink>
        </td>
     <%--   <td runat="server" id="td5"><asp:HyperLink runat="server" ID="hl5" NavigateUrl="/ProductCatalog.Calculator.aspx?id={0}">Kalkulator</asp:HyperLink>
        </td>
<%--        <td runat="server" id="td7"><asp:HyperLink runat="server" ID="hl7" NavigateUrl="/ProductCatalogForAllegroBatchProduct.aspx?id={0}"  >Harmonogram </asp:HyperLink>
        </td>--%>
      <%--  <td runat="server" id="td8"><asp:HyperLink runat="server" ID="hl8" NavigateUrl="/ProductCatalogSubProducts.aspx?id={0}">Podprodukty</asp:HyperLink>
        </td>--%> 
        <td runat="server" id="td6" style="text-align:right"><asp:HyperLink runat="server" ID="hl6" NavigateUrl="/Orders.aspx?idProduct={0}" Target="_blank">Zamówienia</asp:HyperLink>
        </td>
        <td style="text-align:right">
    <asp:HyperLink runat="server" Text="Podgląd" ID="hlPreview" NavigateUrl="/ProductCatalog.Preview.aspx?id={0}" Target="_blank"></asp:HyperLink></td>
    </tr>
</table>
