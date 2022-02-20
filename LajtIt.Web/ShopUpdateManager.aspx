<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShopUpdateManager.aspx.cs" Inherits="LajtIt.Web.ShopUpdateManager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Zarządzanie konfiguracją aktualizacji sklepów</h1>
    <table>
        <tr>
            <td>
                Rodzaj silnika sklepu:<br />
                <asp:DropDownList runat="server" ID="ddlShopType" DataValueField="ShopEngineTypeId" DataTextField="Name" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlShopType_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr style="text-align:center">
                        <td>Pole do aktualizacji:<br />
                            <asp:DropDownList runat="server" ID="ddlShopColumnType" DataValueField="ShopColumnTypeId" DataTextField="ColumnName"></asp:DropDownList>
                        </td>
                        <td>Kolumna aktualizująca:<br />
                            <asp:DropDownList runat="server" ID="ddlColumnName" DataValueField="ColumnName" DataTextField="ColumnName"></asp:DropDownList>
                        </td>
                        <td>Typ aktualizacji:<br />
                            <asp:DropDownList runat="server" ID="ddlUpdateType" DataValueField="UpdateTypeId" DataTextField="Name"></asp:DropDownList>
                        </td>
                        <td><asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Dodaj" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView runat="server" ID="gvColumnType" AutoGenerateColumns="false" EmptyDataText="Brak danych">
                    <Columns>
                        <asp:BoundField DataField="ShopColumnType" HeaderText="Pole do aktualizacji" />
                        <asp:BoundField DataField="ColumnName" HeaderText="Kolumna aktualizująca" />
                        <asp:BoundField DataField="UpdateName" HeaderText="Typ aktualizacji" />
                    </Columns>
                </asp:GridView>

            </td>
        </tr>
    </table>

</asp:Content>
