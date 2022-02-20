<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductTypeSchema.aspx.cs" Inherits="LajtIt.Web.ProductTypeSchema" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Typ produktu - edytor schematu danych</h1>
    
    <table style="width: 100%">
        <tr>
            <td style="width:150px">Nazwa schematu</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlProductTypeSchema" DataValueField="ProductTypeSchemaId" DataTextField="SchemaName" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlProductTypeSchema_SelectedIndexChanged">
                </asp:DropDownList>
                    <asp:TextBox runat="server" ID="txbName" ValidationGroup="add"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
                        TargetControlID="txbName"
                        WatermarkText="Nazwy schemat"
                        WatermarkCssClass="watermarked" />
                    <asp:RequiredFieldValidator runat="server" Text="*" ControlToValidate="txbName" ValidationGroup="add"></asp:RequiredFieldValidator>
                <asp:LinkButton runat="server" ValidationGroup="add" Text="dodaj" OnClick="lbtnAdd_Click" ID="lbtnAdd"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Panel runat="server" ID="pnShops" Visible="false">
                    <table>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ListBox runat="server" ID="lbxShops" SelectionMode="Multiple" Rows="5" Width="300" DataValueField="ShopId" DataTextField="Name"></asp:ListBox></td>
                                        <td>
                                            <asp:Button runat="server" ID="btnAdd1" OnClick="btnAdd1_Click" Text=">>>>" CausesValidation="false" /><br />
                                            <asp:Button runat="server" ID="btnDel" OnClick="btnDel_Click" Text="<<<<" CausesValidation="false" />
                                        </td>
                                        <td>
                                            <asp:ListBox runat="server" ID="lbxShopsOn" SelectionMode="Multiple" Rows="5" Width="300" DataValueField="ShopId" DataTextField="Name"></asp:ListBox></td>
                                    </tr>
                                </table>
                                Lista sklepów, do których eksportowane będą dane z bieżącego schematu<br />
                                <br />
                            </td>

                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>Typ produktu</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlProductCatalogProductTypeAttribute" DataValueField="AttributeId" DataTextField="Name" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlProductCatalogProductTypeAttribute_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table style="width: 100%">
                    <tr><td><h2>Dostępne grupy atrubutów</h2></td>
                        <td><h2>Wybrane atrybuty</h2></td>
                    </tr>
                    <tr valign="top">
                        <td style="width: 50%">
                            <asp:ListBox runat="server" ID="lbxAttributeGroups" DataValueField="AttributeGroupId" Width="300" DataTextField="Name" Rows="20" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                        <td style="width: 50%">
                            <asp:GridView runat="server" ID="gvAttributeGroups" AutoGenerateColumns="false" DataKeyNames="AttributeGroupId" Width="300" EmptyDataText="Nie wybrano">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Nazwa atrubutu" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txbOrder" TextMode="Number" Text='<%# Eval("Order") %>' Width="50" MaxLength="2"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chbAttributeGroup" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            Lista atrybutów, które będą eksportowane do sklepu dla danego typu produktu.<br />
                            <asp:CheckBox runat="server" /> Zaznacz by usunąć z listy
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnAdd" Text="Dodaj" OnClick="btnAdd_Click" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnSaveAttributeGroups" Text="Zapisz" OnClick="btnSaveAttributeGroups_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>Atrybut</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlAtributeGroups" DataValueField="AttributeGroupId" DataTextField="Name" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlAtributeGroups_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <h2>Typy produktów</h2>
                        </td>
                        <td>
                            <h2>Wybrane typy produktów</h2>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td style="width: 50%">
                            <asp:ListBox runat="server" ID="lbxProductTypes" DataValueField="AttributeId" Width="300" DataTextField="Name" Rows="20" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                        <td style="width: 50%">
                            <asp:GridView runat="server" ID="gvProductTypesSelected" AutoGenerateColumns="false" DataKeyNames="AttributeId" Width="300" EmptyDataText="Nie wybrano">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Nazwa atrubutu" />
                                   
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chbAttribute" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView> 
                            <asp:CheckBox runat="server" /> Zaznacz by usunąć z listy
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnProductsAdd" Text="Dodaj" OnClick="btnProductsAdd_Click" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnProductsDelete" Text="Zapisz" OnClick="btnProductsDelete_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
