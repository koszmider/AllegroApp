<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true" CodeBehind="ProductCatalog.Packing.aspx.cs"
    Inherits="LajtIt.Web.ProductPacking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upShop" runat="server">
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <uc:ProductMenu runat="server" SetTab="td15"></uc:ProductMenu>
    <asp:Label runat="server" ID="lblOK"></asp:Label>
    <div style="text-align: right;">
        <asp:LinkButton runat="server" ID="lbtnPackingNew" CausesValidation="false" Text="Dodaj"
            OnClick="lbtnPackingNew_Click"></asp:LinkButton>
    </div>

    <asp:GridView runat="server" ID="gvPacking" AutoGenerateColumns="false" OnRowDataBound="gvPacking_RowDataBound" EmptyDataText="Brak zdefiniowanych gabarytów">
        <Columns>
            <asp:TemplateField HeaderText="Edytuj" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">

                <ItemTemplate>
                    <asp:Button runat="server" ID="btnEdit" Text="edytuj" OnClick="btnEdit_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Paczkomat" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center">

                <ItemTemplate>
                    <asp:Label runat="server" ID="lblSize"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField> 
            <asp:CheckBoxField HeaderText="Ponadgabaryt?" DataField="IsOversize" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField ItemStyle-Width="120" ItemStyle-HorizontalAlign="center" DataField="Weight" HeaderText="Waga" />
            <asp:BoundField ItemStyle-Width="120" ItemStyle-HorizontalAlign="center" DataField="Width" HeaderText="Szerokość" />
            <asp:BoundField ItemStyle-Width="120" ItemStyle-HorizontalAlign="center" DataField="Height" HeaderText="Wysokość" />
            <asp:BoundField ItemStyle-Width="120" ItemStyle-HorizontalAlign="center" DataField="Length" HeaderText="Długość" />
            <asp:BoundField ItemStyle-Width="120" ItemStyle-HorizontalAlign="center" DataField="WeightCalculated" HeaderText="Waga wyliczona" />
        </Columns>

    </asp:GridView>
    <asp:ModalPopupExtender ID="mpePacking" runat="server"
        TargetControlID="lblOK"
        PopupControlID="pnShopProducts"
        BackgroundCssClass="modalBackground"
        DropShadow="true"
        CancelControlID="imbCancel"
        PopupDragHandleControlID="Panel1">
    </asp:ModalPopupExtender>

    <asp:Panel runat="server" ID="pnShopProducts" GroupingText="Paczki" BackColor="White"
        Style="width: 900px; background-color: white; height: 250px; padding: 10px">
        <div style="text-align: right;">
            <asp:ImageButton runat="server" ID="imbCancel" ImageUrl="~/Images/cancel.png" Width="20" />
        </div>

        <table>
            <tr>
                <td>Paczkomat</td>
                <td>Ponadgabaryt?</td>
                <td>Waga</td>
                <td>Szerokość</td>
                <td>Wysokość</td>
                <td>Długość</td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList runat="server" ID="ddlSize" ValidationGroup="os" Width="200">
                        <asp:ListItem Value="-1">-- nie wybrano --</asp:ListItem>
                        <asp:ListItem Value="0">niemożliwy</asp:ListItem>
                        <asp:ListItem Value="1">A</asp:ListItem>
                        <asp:ListItem Value="2">B</asp:ListItem>
                        <asp:ListItem Value="3">C</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlIsOversize" ValidationGroup="os" Width="200">
                        <asp:ListItem Value="0">-- nie wybrano --</asp:ListItem>
                        <asp:ListItem Value="1">TAK</asp:ListItem>
                        <asp:ListItem Value="2">NIE</asp:ListItem>
                    </asp:DropDownList>

                    </td>
                <td>
                    <asp:TextBox runat="server" Width="100"   ValidationGroup="os" ID="txbWeight"></asp:TextBox></td>
                <td>                                         
                    <asp:TextBox runat="server" Width="100"   ValidationGroup="os" ID="txbWidth"></asp:TextBox>
                </td>                                        
                <td>                                         
                    <asp:TextBox runat="server" Width="100"   ValidationGroup="os" ID="txbHeight"></asp:TextBox></td>
                <td>                                         
                    <asp:TextBox runat="server" Width="100"   ValidationGroup="os" ID="txbLength"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="5"><br /><br /><br />
                    <asp:Button runat="server" ID="btnSave" ValidationGroup="os" Text="Zapisz" OnClick="btnSave_Click" />
                    <asp:LinkButton runat="server" ID="btnDelete" CausesValidation="false"  OnClientClick="return confirm('Usunąć?')" Text="Usuń" ForeColor="Red" OnClick="btnDelete_Click" />
                </td>
            </tr>
        </table>

    </asp:Panel>
</asp:Content>
