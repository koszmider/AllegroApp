<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DescriptionMixer.aspx.cs" Inherits="LajtIt.Web.DescriptionMixer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Mikser opisów - konfiguracja</h1>
    <br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            
            <asp:DropDownList runat="server" ID="ddlShopType" DataValueField="ShopTypeId"
                AppendDataBoundItems="true" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlAttributeGroups_OnSelectedIndexChanged">
          
            </asp:DropDownList><br />
            <asp:DropDownList runat="server" ID="ddlAttributeGroups" DataValueField="AttributeGroupId"
                AppendDataBoundItems="true" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlAttributeGroups_OnSelectedIndexChanged">
                <asp:ListItem></asp:ListItem>
            </asp:DropDownList>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlAttributes" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Panel runat="server" ID="pnAttributeGroup" Visible="false">
        <table style="width: 100%;">
            <tr>
                <td style="width: 33%;">Wersja męska</td>
                <td style="width: 33%;">Wersja żeńska</td>
                <td style="width: 33%;">Wersja nijaka</td>
            </tr>
            <tr>
                <td style="width: 33%;">
                    <asp:TextBox runat="server" ID="txbTemplateM" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox>
                </td>
                <td style="width: 33%;">
                    <asp:TextBox runat="server" ID="txbTemplateF" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox>
                </td>
                <td style="width: 33%;">
                    <asp:TextBox runat="server" ID="txbTemplateN" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3">Dodatkowe tagi: [PRODUCENT][KOD][KOD2][LINIA][EAN]</td>
            </tr>
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox runat="server" ID="cbxIsActive" Text="Aktywny" /></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>Zastosuj do:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlAttributes" DataValueField="AttributeId" DataTextField="Name" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">wszystkich atrybutów</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlProductAttributeGroupings" DataValueField="AttributeGroupingId" DataTextField="Name" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">wszystkich rodzajów produktów</asp:ListItem>
                                </asp:DropDownList></td>
                            <td>
                                <asp:HyperLink runat="server" NavigateUrl="~/ProductCatalog.Attributes.Grouping.aspx">dodaj nową grupę</asp:HyperLink></td>
                        </tr>
                    </table>

                </td>
                <td style="text-align: right;">
                    <asp:UpdatePanel runat="server" ID="up1">
                        <ContentTemplate>

                            <asp:Button runat="server" ID="btnSaveNew" Text="Dodaj" OnClick="btnSaveNew_Click" OnClientClick="return confirm('Czy dodać nowy wpis?');" />
                            <asp:Button runat="server" ID="btnSaveEdit" Text="Zapisz zmiany" Visible="false" OnClick="btnSaveEdit_Click" OnClientClick="return confirm('Zapisać zmiany?');" />
                            <asp:LinkButton runat="server" ID="lbtnCancel" Text="Anuluj" OnClick="lbtnCancel_Click"></asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div style="position: absolute;">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="up1">
                            <ProgressTemplate>
                                <img src="Images/progress.gif" style="height: 20px" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <style>
            .tabGroup tr td {
                padding: 10px
            }

            .tag {
                color: red;
                font-weight: bold;
            }

            .tagText {
                color: green;
                font-weight: bold;
            }
        </style>
        <asp:GridView runat="server" ID="gvAttributeGroup" AutoGenerateColumns="false" OnSelectedIndexChanging="gvAttributeGroup_SelectedIndexChanging"
            DataKeyNames="Id" Width="100%" CssClass="tabGroup" OnRowDataBound="gvAttributeGroup_RowDataBound">
            <SelectedRowStyle BackColor="silver" />
            <Columns>
                <asp:CommandField ShowSelectButton="true" SelectText="edytuj" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="Aktywny" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ProductCatalogAttribute.Name" HeaderText="Zastosuj do" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ProductCatalogAttributeGrouping.Name" HeaderText="Grupa produktów" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Wersja męska" ItemStyle-Width="30%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTemplateM"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Wersja żeńska" ItemStyle-Width="30%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTemplateF"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Wersja nijaka" ItemStyle-Width="30%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTemplateN"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
        <h2>Atrubuty</h2>
        <asp:GridView runat="server" ID="gvAttribute" AutoGenerateColumns="false" OnRowCancelingEdit="gvAttribute_RowCancelingEdit"
            OnRowEditing="gvAttribute_RowEditing" OnRowUpdating="gvAttribute_RowUpdating"
            DataKeyNames="AttributeId" Width="100%" CssClass="tabGroup" OnRowDataBound="gvAttribute_RowDataBound">

            <Columns>
                <asp:CommandField ShowEditButton="true" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100" />
                <asp:BoundField DataField="Name" ReadOnly="true" HeaderText="Atrybut" />
                <asp:TemplateField HeaderText="Wersja męska" ItemStyle-Width="25%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTemplateM"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txbTemplateM" TextMode="MultiLine" Rows="4" Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Wersja żeńska" ItemStyle-Width="25%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTemplateF"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txbTemplateF" TextMode="MultiLine" Rows="4" Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Wersja nijaka" ItemStyle-Width="25%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTemplateN"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txbTemplateN" TextMode="MultiLine" Rows="4" Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
