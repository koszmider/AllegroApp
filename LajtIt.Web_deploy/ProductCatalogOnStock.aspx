<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductCatalogOnStock.aspx.cs" Inherits="LajtIt.Web.ProductCatalogOnStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .promo { text-decoration:line-through;color:silver;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"><p>Produkty w magazynie</p>
    <table>
        <tr>
            
            <td>Dostawca:
                <asp:CheckBoxList ID="chblSuppliers" runat="server" DataValueField="SupplierId" DataTextField="SupplierName" RepeatColumns="8"
                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chblSuppliers_OnSelectedIndexChanged">
                </asp:CheckBoxList>
            </td>
            <td>
                       <asp:TextBox runat="server" ID="txbPriceFrom" MaxLength="50" Width="93" OnTextChanged="chblSuppliers_OnSelectedIndexChanged"  AutoPostBack="true"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                            TargetControlID="txbPriceFrom"
                            WatermarkText="cena od"
                            WatermarkCssClass="watermarked" />
                        <asp:TextBox runat="server" ID="txbPriceTo" MaxLength="50" Width="93" OnTextChanged="chblSuppliers_OnSelectedIndexChanged"  AutoPostBack="true"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server"
                            TargetControlID="txbPriceTo"
                            WatermarkText="cena do"
                            WatermarkCssClass="watermarked" /></td>
        </tr>
        <tr>
            
            <td>Typ lampy:
                <asp:CheckBoxList ID="chbLampType" runat="server" DataValueField="AttributeId" DataTextField="Name" RepeatColumns="5"
                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chblSuppliers_OnSelectedIndexChanged">
                </asp:CheckBoxList>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>

            <asp:Repeater runat="server" ID="rpProducts" OnItemDataBound="rpProducts_ItemDataBound">
                <ItemTemplate>
                    <div style="width:170px;  height:290px; border: solid 1px gray; float:left;">
                        <div style="width:150px;  height:150px; overflow:hidden;"><asp:HyperLink runat="server" ID="hlImage" Target="_blank"><asp:Image runat="server" ID="imgImage" Width="150" /></asp:HyperLink> </div>
                        <div style="text-align:center; font-weight:bold;">
                        <asp:HyperLink runat="server" ID="hlProduct" NavigateUrl="/ProductCatalog.Preview.aspx?id={0}&idSupplier={1}" Target="_blank"></asp:HyperLink><br />
                        <asp:Label runat="server" ID="lbPrice"></asp:Label><br />
                        <asp:Label runat="server" ID="lblQuantity"></asp:Label></div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
