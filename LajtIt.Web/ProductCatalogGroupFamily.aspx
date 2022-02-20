<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductCatalogGroupFamily.aspx.cs" Inherits="LajtIt.Web.ProductCatalogGroupFamily" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Rodziny i grupy produktów</h1>
    <table>
        <tr>
            <td>Dostawca</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlSuppliers" AppendDataBoundItems="true" DataValueField="SupplierId" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlSuppliers_SelectedIndexChanged">
                    <asp:ListItem></asp:ListItem>

                </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Rodzina produktów</td>
            <td>
                <asp:ListBox runat="server" ID="lbxFamilies" Rows="10" Width="500" DataValueField="FamilyId" SelectionMode="Multiple"
                    DataTextField="FamilyName" AutoPostBack="true" OnSelectedIndexChanged="lbxFamilies_SelectedIndexChanged"></asp:ListBox></td>
            <td>
                <asp:Panel runat="server" ID="pFamilyJoin" Visible="false" GroupingText="Połącz rodziny">
                    <asp:Button runat="server" ID="btnFamilyJoin" OnClientClick="return confirm('Czy połączyć zaznaczone?')"
                        OnClick="btnFamilyJoin_Click" Text="Połącz zaznaczone rodziny" Width="300" />
                </asp:Panel>
                <asp:Panel runat="server" ID="pFamilyEdit" GroupingText="Dodaj/edytuj rodzinę">
                    <asp:TextBox runat="server" ID="txbFamilyEdit" ValidationGroup="editf" Width="300"></asp:TextBox><br />
                    <asp:DropDownList runat="server" ID="ddlFamilyTypeEdit" ValidationGroup="move" Width="300" DataValueField="FamilyTypeId" DataTextField="FamilyTypeName"></asp:DropDownList><br />
                    <asp:Button runat="server" ID="btnFamily" OnClientClick="return confirm('Czy zapisać dane?')" Visible="false"
                        OnClick="btnFamily_Click" Text="Zapisz zmiany" ValidationGroup="editf" Width="300" />
                    <asp:Button runat="server" ID="btnFamilyAdd" OnClientClick="return confirm('Czy zapisać dane?')"
                        OnClick="btnFamilyAdd_Click" Text="Dodaj nowy" ValidationGroup="editf" Width="300" />
                    <br />
                    <asp:LinkButton runat="server" ID="lbtnFamilyNew" Text="dodaj nowy" OnClick="lbtnFamilyNew_Click"></asp:LinkButton><br /><br />
                 <asp:Button runat="server" ID="btnFamilyFromGroups" OnClientClick="return confirm('Czy utworzyć rodziny z wybranych grup?')"
                        OnClick="btnFamilyFromGroups_Click" Text="Twórz rodziny z grup" ValidationGroup="editf" Width="300" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>Grupy produktów</td>
            <td>
                <asp:ListBox runat="server" ID="lbxGroups" SelectionMode="Multiple" Rows="30" Width="500" DataValueField="ProductCatalogGroupId" DataTextField="GroupName" AutoPostBack="true"
                    OnSelectedIndexChanged="lbxGroups_SelectedIndexChanged"></asp:ListBox></td>

            <td>
                <asp:Panel runat="server" ID="pnGroupEdit" Visible="false" GroupingText="Edycja">
                    <asp:TextBox runat="server" ID="txbGroupName" ValidationGroup="editg" Width="300"></asp:TextBox><br />
                    <asp:Button runat="server" ID="btnGroupsEdit" OnClientClick="return confirm('Czy zapisać dane?')"
                        OnClick="btnGroupsEdit_Click" Text="Zapisz" ValidationGroup="editg" Width="300" /><br />
                    <br />
                    <br />
                    <asp:HyperLink runat="server" ID="hlProductCatalog" Target="_blank">pokaż produkty</asp:HyperLink>
                </asp:Panel>
                <asp:Panel runat="server" ID="pFamilyAssign" Visible="false" GroupingText="Przenoszenie">
                    Przypisz rodzinę:<br />
                    <asp:DropDownList runat="server" ID="ddlFamilyAssign" DataValueField="FamilyId" DataTextField="FamilyName" ValidationGroup="move" Width="300">
                        <asp:ListItem></asp:ListItem>

                    </asp:DropDownList>
                    <br />
                    lub utwórz nową<br />
                    <asp:TextBox runat="server" ID="txbFamily" ValidationGroup="move" Width="300"></asp:TextBox>
                    <br />
                    <asp:CheckBox runat="server" ID="chbJump" Text="Dodaj i przejdź do rodziny" />
                    <hr />
                    <asp:Button runat="server" ID="btnGroupsMove" OnClientClick="return confirm('Czy przenieść wybrane grupy do nowej rodziny?')" Width="300" OnClick="btnGroupsMove_Click" Text="Przenieś" ValidationGroup="move" />
                </asp:Panel>
                <asp:Panel runat="server" ID="pnGroupAdd" Visible="false" GroupingText="Nowa grupa">
                    <asp:TextBox runat="server" ID="txbGroupNew" ValidationGroup="newg" Width="300"></asp:TextBox><br />
                    <asp:Button runat="server" ID="btnGroupNew" OnClientClick="return confirm('Czy zapisać dane?')"
                        OnClick="btnGroupNew_Click" Text="Dodaj nową grupę" ValidationGroup="newg" Width="300" />
                </asp:Panel>
                <asp:Panel runat="server" ID="pnGroupAddMerge" Visible="false" GroupingText="Nowa grupa">
                    <asp:TextBox runat="server" ID="txbGroupMerge" ValidationGroup="newm" Width="300"></asp:TextBox><br />
                    <asp:Button runat="server" ID="btnGroupMerge" OnClientClick="return confirm('Czy połączyć i utwozyć nową grupę?')"
                        OnClick="btnGroupMerge_Click" Text="Połącz i utwórz nową grupę" ValidationGroup="newm" Width="300" />
                </asp:Panel>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
        </tr>
    </table>
</asp:Content>
