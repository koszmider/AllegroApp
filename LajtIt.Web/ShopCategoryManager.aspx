<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="ShopCategoryManager.aspx.cs" Inherits="LajtIt.Web.ShopCategoryManager" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Zarządzanie specjalnymi kategoriami</h1>
    <h3>Skonfiguruj przypisywanie produktow do kategorii wg niestandardowych parametrów.</h3>
    <div style="width: 100%;">
        <asp:UpdatePanel runat="server" ID="upSuppliers">
            <ContentTemplate>

                <div style="text-align: right;">
                    <asp:LinkButton runat="server" ID="lbtnShopCategoryAdd" Text="Dodaj nową konfigurację kategorii" OnClick="lbtnShopCategoryAdd_Click" OnClientClick="return confirm('Dodać ?');"></asp:LinkButton>
                    <br />
                    <asp:LinkButton runat="server" ID="lbtnRefreshCategory" Text="Odśwież drzewo kategorii" OnClick="lbtnRefreshCategory_Click" OnClientClick="return confirm('Czy odświeżyć drzewo kategorii ?');"></asp:LinkButton>
                    <div style="width: 100%; text-align: right; "><div>
                        <div style="position: absolute;">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upSuppliers">
                                <ProgressTemplate>
                                    <img src="Images/progress.gif" style="height: 20px" alt="" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div></div>
                    </div>
                </div>
                <asp:DropDownList runat="server" ID="ddlShop" DataValueField="ShopId" DataTextField="Name" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlShop_SelectedIndexChanged">
                    <asp:ListItem Value="0">-- wybierz sklep --</asp:ListItem>
                </asp:DropDownList>



            </ContentTemplate>
        </asp:UpdatePanel>




        <asp:GridView runat="server" ID="gvShopCategorys" AutoGenerateColumns="false" DataKeyNames="ShopCategoryManagerId" Width="500" EmptyDataText="Brak danych">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="ShopCategoryManagerId" DataNavigateUrlFormatString="ShopCategoryManagerPage.aspx?id={0}"
                    DataTextField="Name" HeaderText="Nazwa" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="Aktywny" ItemStyle-HorizontalAlign="Center" />
            </Columns>
        </asp:GridView>
</asp:Content>
