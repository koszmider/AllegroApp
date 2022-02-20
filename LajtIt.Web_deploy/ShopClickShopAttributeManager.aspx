<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShopClickShopAttributeManager.aspx.cs" Inherits="LajtIt.Web.ShopClickShopAttributeManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <asp:UpdatePanel runat="server" ID="upSuppliers">
            <ContentTemplate>
                <div>
                    <asp:DropDownList runat="server" ID="ddlShop" DataValueField="ShopId" DataTextField="Name"  AppendDataBoundItems="true" OnSelectedIndexChanged="ddlShop_SelectedIndexChanged">
                        <asp:ListItem Value="0">-- wybierz sklep --</asp:ListItem>
                    </asp:DropDownList>
    <asp:Button runat="server" ID="btnGetGroups" Text="Pobież grupy" OnClick="btnGetGroups_Click" />
    <asp:CheckBox runat="server" ID="chbAssignedOnly" Text="Tylko przypisane" Checked="true" />
                    <div style="position: absolute;">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upSuppliers">
                            <ProgressTemplate>
                                <img src="Images/progress.gif" style="height: 20px" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                </div>
                 



            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
<br />
    <br />
    <asp:GridView runat="server" ID="gvAttributeGroups" AutoGenerateColumns="false" OnRowDataBound="gvAttributeGroups_RowDataBound" AutoGenerateEditButton="true"
        OnRowEditing="gvAttributeGroups_RowEditing" OnRowUpdating="gvAttributeGroups_RowUpdating" DataKeyNames="attribute_group_id" OnRowCancelingEdit="gvAttributeGroups_RowCancelingEdit">
        <Columns>
            <asp:TemplateField HeaderText="Id">
                <ItemStyle Width="50" HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Label ID="lblExternalShopAttributeGroupId" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nazwa grupy w sklepie">
                <ItemTemplate>
                    <asp:Label ID="lblExternalShopAttributeGroupName" runat="server"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txbExternalShopAttributeGroupName" runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nazwa grupy w Systemie">
                <ItemTemplate>
                    <asp:Label ID="lblAttributeGroup" runat="server"></asp:Label>
                </itemtemplate>
                <edititemtemplate>
                    <asp:Label ID="lblAttributeGroup" runat="server"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlAttributeGroups" DataValueField="AttributeGroupId" DataTextField="Name"></asp:DropDownList>

                </EditItemTemplate>

            </asp:TemplateField>
            <asp:TemplateField HeaderText="Aktywna">
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="chbIsActive" Enabled="false" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBox runat="server" ID="chbIsActive" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Użyj w opcjach przeglądania">
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="chbIsSearchable" Enabled="false" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBox runat="server" ID="chbIsSearchable" />
                </EditItemTemplate>
            </asp:TemplateField>
            <%-- <asp:BoundField HeaderText="Użyj w opcjach przeglądania" DataField="IsSearchable" />--%>
        </Columns>

    </asp:GridView>

    <asp:Button runat="server" ID="btnCreate" Text="Utwórz kolejne 10 grup" OnClientClick="return confirm('Utworzyć?');" OnClick="btnCreate_Click" />

</asp:Content>
