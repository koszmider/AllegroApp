<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShopMainPageCreator.aspx.cs" Inherits="LajtIt.Web.ShopMainPageCreator" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var DropDownList1 = $('#<%= ddlMainPageGroup.ClientID %>');
        //reset onchange to avoid postback
        DropDownList1.removeAttr("onchange");
        DropDownList1.change(function (e) {
            i = DropDownList1.prop('selectedIndex');
            //alert(i);
            if (confirm('are you sure?')) {
                //copy onchange from page source
                setTimeout("__doPostBack('<%= ddlMainPageGroup %>','something')", 0);
            }
            else {
                //return false;
                DropDownList1.prop('selectedIndex', i);
            }
        });
    });
    </script>

    <h2>Zarządzanie główną stroną sklepu</h2>
    <table style="width: 100%;">
        <tr>
            <td>Schemat strony</td>
        </tr>
        <tr valign="top">
            <td style="width: 500px;">
                <asp:DropDownList runat="server" ID="ddlMainPageGroup" DataTextField="Name" DataValueField="ShopMainPageGroupId"
                    Style="font-family: 'Courier New' , Courier, Fixed, monospace; width: 500px;" OnSelectedIndexChanged="btnShow_Click">
                </asp:DropDownList></td>
            <td>
                <asp:Button runat="server" ID="btnShow" OnClick="btnShow_Click" Text="Wczytaj schemat"
                    OnClientClick="return confirm('Porzucić niezapisane zmiany i wczytać wybrany schemat?');" />
                <asp:LinkButton runat="server" ID="btnActive" OnClick="btnActive_Click" Text="Ustaw aktywny"
                    OnClientClick="return confirm('Czy ustawić aktualny schemat jako aktywny?');" />
               </td>
            <td></td>
            </tr>
    </table>

    <table style="width: 100%;">
        <tr>
            <td style="text-align:right">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        <img src="Images/progress.gif" style="height: 20px" alt="" /></ProgressTemplate>
                </asp:UpdateProgress></td>
            <td style="text-align: right;"><asp:TextBox runat="server" ID="txbShopMainPageGroupName" ValidationGroup="new"></asp:TextBox><asp:RequiredFieldValidator SetFocusOnError="true"
                    ControlToValidate="txbShopMainPageGroupName" ValidationGroup="new" runat="server" Text="*"></asp:RequiredFieldValidator> 
                <asp:LinkButton runat="server" ID="lbtnSaveNew" OnClick="lbtnSaveNew_Click" ValidationGroup="new" Text="Zapisz nowy"
                    OnClientClick="return confirm('Zapisać schemat pod nową nazwa?');"></asp:LinkButton><asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
                        TargetControlID="txbShopMainPageGroupName"
                        WatermarkText="tytuł shematu"
                        WatermarkCssClass="watermarked" />
            </td>
            <td style="width: 100px;text-align: right;">
                <asp:LinkButton runat="server" ID="lbtnReadShop" OnClick="lbtnReadShop_Click" CausesValidation="false" Text="Wczytaj sklep"
                    OnClientClick="return confirm('Czy wczytać aktualne ustawienia bezpośrednio ze sklepu?');"></asp:LinkButton></td>
        </tr>

    </table>
    <style>
        .rl ul {
            list-style-type: decimal;
        }
    </style>
    <table>
        <tr valign="top">
            <td>
    <asp:ReorderList ID="rlProducts" runat="server"
        OnItemDataBound="rlProducts_OnItemDataBound"
        OnItemReorder="rlProducts_ItemReorder"
        OnDeleteCommand="rlProducts_DeleteCommand"
        DragHandleAlignment="Right"
        ItemInsertLocation="Beginning"
        DataKeyField="Id"
        SortOrderField="Priority"
        Width="500" CssClass="rl"
        ViewStateMode="Enabled"
        PostBackOnReorder="true"
        ShowInsertItem="false"
        BorderWidth="1"
        AllowReorder="true">

        <ItemTemplate>
            <div style="background-color: silver; padding: 3px; width: 90%;">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 120px;">
                            <asp:Image runat="server" ID="imgImage" Width="100" /></td>
                        <td>
                            <asp:HyperLink runat="server" Target="_blank" ID="hlProduct">
                                <asp:Label runat="server" ID="lblProductName"></asp:Label>
                            </asp:HyperLink><br />
                            <asp:Label runat="server" ID="lblPriceBrutto"></asp:Label><br />
                        </td>
                        <td style="width: 50px;">
                            <asp:HyperLink runat="server" Target="_blank" ID="hlShop">
                                <asp:Image runat="server" ImageUrl="~/Images/shop.png" Width="20"  ToolTip="Zobacz w sklepie"/>
                            </asp:HyperLink>
                            <asp:ImageButton ID="btnDeleteEvent" runat="server" CommandName="Delete" OnClientClick="return confirm('Usunąć wybrany produkt?');"
                                CommandArgument='<%# Eval("Id") %>' ImageUrl="~/Images/cancel.png"  Width="20" ToolTip="Usuń z listy"/></td>
                    </tr>
                </table>

            </div>
        </ItemTemplate>
        <ReorderTemplate>
            <asp:Label runat="server" ID="lblProductName"></asp:Label>
        </ReorderTemplate>
        <DragHandleTemplate>
            <asp:Image runat="server" ImageUrl="~/Images/updown.jpg" />
        </DragHandleTemplate>
        <InsertItemTemplate>
        </InsertItemTemplate>
    </asp:ReorderList>
    </td>
            <td>
                <asp:Panel runat="server" ID="pnMissingProducts" Visible="false" EnableViewState="false" GroupingText="Produkty spoza katalogu">
                    <asp:GridView ID="gvMissingProducts" AutoGenerateColumns="false" runat="server" ShowHeader="false" Width="100">
                        <Columns>
                            <asp:HyperLinkField DataTextField="Id" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="http://lajtit.pl/pl/p/p/{0}" Target="_blank" ItemStyle-HorizontalAlign="Center" />

                        </Columns>

                    </asp:GridView>

                </asp:Panel>

            </td>
        </tr>
    </table>
    <asp:UpdatePanel runat="server" ID="upNewProduct">
        <ContentTemplate>
            Kod:
                    <asp:TextBox runat="server" ID="txbProductCode" MaxLength="20" Width="60"></asp:TextBox><asp:Button
                        runat="server" OnClick="btnProductAdd_Click" Text="Dodaj" />&nbsp;&nbsp;&nbsp;
                    
                    <span style="position: absolute; width: 10px;">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upNewProduct">
                            <ProgressTemplate>
                                <img src="Images/progress.gif" style="height: 20px" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </span>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Zapisz" Visible="false" />
</asp:Content>
