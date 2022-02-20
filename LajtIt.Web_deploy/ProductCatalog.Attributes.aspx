<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ProductCatalog.Attributes.aspx.cs" Inherits="LajtIt.Web.ProductAttributes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/ShopCategoryControlJson.ascx" TagName="ShopCategoryControl" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Konfigurator atrybutów</h1>


    <asp:DropDownList runat="server" ID="ddlAttributes" DataValueField="AttributeGroupId"
        AppendDataBoundItems="true" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlAttributes_SelectedIndexChanged">
        <asp:ListItem></asp:ListItem>
    </asp:DropDownList>

    <div style="text-align: right;">
        <asp:LinkButton runat="server" ID="lbtnAttributeGroupNew" CausesValidation="false"
            Text="Dodaj nową grupę" OnClick="lbtnAttributeGroupNew_Click"></asp:LinkButton>
    </div>
    <asp:Panel runat="server" ID="pnAttributeGroup" Visible="false">
        <table style="width: 100%" class="mytable">
            <tr valign="top">
                <td style="width: 300px;">Nazwa
                </td>
                <td>
                    <asp:TextBox ID="txbName" Width="250" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Kod grupy
                </td>
                <td>
                    <asp:TextBox ID="txbGroupCode" Width="250" MaxLength="10" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Typ
                </td>
                <td>
                    <asp:DropDownList runat="server" DataValueField="AttributeGroupTypeId" Enabled="false" DataTextField="GroupName" ID="ddlAttributeGroupType"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Eksportuj do sklepu
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="chbExportToShop" Text="" />

                </td>
            </tr>

            <tr>
                <td>Kolejność w Sklepie
                </td>
                <td>
                    <asp:TextBox ID="txbAllegroOrder" MaxLength="2" Width="50" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rfv" ControlToValidate="txbAllegroOrder" ErrorMessage="*"></asp:RequiredFieldValidator>
                </td>
            </tr>

        </table>
    </asp:Panel>
    <table>

        <tr>
            <td colspan="3">
                <style>
                    table.t tr td {
                        padding: 5px;
                    }
                </style>
                    <asp:LinkButton runat="server" ID="lbtnAttributeNew" CausesValidation="false" Text="Dodaj nowy" Visible="false"
        OnClick="lbtnAttributeNew_Click"></asp:LinkButton><br /><br />
                <asp:GridView runat="server" ID="gvAttributes" CssClass="t" DataKeyNames="AttributeId" EmptyDataText="Brak atrybutów"
                    AutoGenerateColumns="false" Style="width: 100%" OnRowDataBound="gvAttributes_RowDataBound">


                    <Columns>
                        <asp:BoundField DataField="AttributeId" HeaderText="Id" />
                        <asp:TemplateField HeaderText="Nazwa">
                            <ItemStyle Width="500" />
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlName" NavigateUrl="~/ProductCatalog.Attribute.aspx?id={0}" Target="_blank"></asp:HyperLink>

                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="height: 70px"></td>
        </tr>
    </table>
    <div style="bottom: 0px; background-color: silver; width: 940px; padding: 20px; left: auto; position: fixed; z-index: 5;">
        <table style="width: 100%">
            <tr>
                <td>

                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>

                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Zapisz" OnClientClick="if(Page_ClientValidate()) return confirm('Zapisać?');" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                        </Triggers>
                    </asp:UpdatePanel>


                    &nbsp;&nbsp;&nbsp;&nbsp;<a href="#">na górę</a>
                </td>
            </tr>
        </table>
    </div>

    <asp:Label runat="server" ID="lblOK"></asp:Label>
 
    <asp:ModalPopupExtender ID="mpeAttribute" runat="server"
    TargetControlID="lblOK"
    PopupControlID="pnAttribute"
    BackgroundCssClass="modalBackground"
    DropShadow="true"
    CancelControlID="imbCancel"
    PopupDragHandleControlID="Panel1" />

<asp:Panel runat="server" ID="pnAttribute" GroupingText="Nowy atrybut" BackColor="White" 
    Style="width: 400px; background-color: white; height: 350px; padding: 10px">
    <div style="text-align:right; "><asp:ImageButton runat="server" ID="imbCancel" ImageUrl="~/Images/cancel.png" Width="20" /></div>
    <table>
        <tr>
            <td>Nazwa</td>
            <td><asp:TextBox runat="server" ID="txbAttributeName" MaxLength="254" ValidationGroup="new"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button runat="server" ID="btnAttributeAdd" Text="Dodaj atrybut" OnClientClick="return confirm('Dodać nowy atrybut?');" ValidationGroup="new"
                OnClick="btnAttributeAdd_Click" />
            </td>
        </tr>
    </table>

</asp:Panel>
</asp:Content>
