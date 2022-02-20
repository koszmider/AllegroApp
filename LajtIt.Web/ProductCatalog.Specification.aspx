<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductCatalog.Specification.aspx.cs" Inherits="LajtIt.Web.ProductCatalogSpecification" %>

<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<%@ Register Src="~/Controls/ProductAttributes.ascx" TagName="ProductAttributes" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Product.Compare.ascx" TagName="ProductCompare" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc:ProductMenu ID="ProductMenu1" runat="server" SetTab="td4"></uc:ProductMenu>
    <div style="text-align: right">
        <asp:CheckBox runat="server" ID="chbIsReady" Text="Produkt skonfigurowany" />
        &nbsp;&nbsp;<asp:LinkButton runat="server" OnClientClick="return confirm('Zmienić?');" OnClick="Unnamed_Click" Text="zmień"></asp:LinkButton><br />

      

        <asp:LinkButton runat="server" ID="imgbCompare" Text="porównaj"            OnClick="imgbCompare_Click" Width="100"></asp:LinkButton>
        <asp:Label runat="server" ID="lblOK"></asp:Label>
        <asp:ModalPopupExtender ID="mpeCompare" runat="server"
            TargetControlID="lblOK"
            PopupControlID="pnShopProducts"
            BackgroundCssClass="modalBackground"
            DropShadow="true"
            PopupDragHandleControlID="Panel1" />

        <asp:Panel runat="server" ID="pnShopProducts"  
            BackColor="White" Style="width: 1000px; background-color: white; max-height: 750px; padding: 10px; overflow:scroll;">

            <uc:ProductCompare ID="ucProductCompare" runat="server" />
            <div style="text-align:left">
            <asp:LinkButton runat="server" ID="btnCancel" Text="Anuluj" /></div>
        </asp:Panel>

    </div>

    <uc:ProductAttributes ID="ucProductAttributes" runat="server"></uc:ProductAttributes>
    <div style="text-align: right; height: 120px;">


        <asp:TextBox runat="server" ID="txbAttributeGroupNew" MaxLength="30"></asp:TextBox><br />

        <asp:DropDownList runat="server" DataValueField="AttributeGroupTypeId" DataTextField="GroupName" ID="ddlAttributeGroupType"></asp:DropDownList><br />
        <asp:LinkButton runat="server" ID="lbtnAttributeGroupNewAdd" Text="Dodaj" OnClick="lbtnAttributeGroupNewAdd_Click" ValidationGroup="new"
            OnClientClick="return confirm('Czy chcesz dodać nową grupę atrybutów?');">
        </asp:LinkButton>
    </div>
    <div style="bottom: 0px; background-color: silver; width: 940px; padding: 20px; left: auto; position: fixed; z-index: 5;">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnAttributesSave" Text="Zapisz" ValidationGroup="new" OnClick="btnAttributesSave_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;<a href="#">na górę</a>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
