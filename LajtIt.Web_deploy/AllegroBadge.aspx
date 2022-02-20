<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllegroBadge.aspx.cs" 
    Inherits="LajtIt.Web.AllegroBadge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Kampanie Allegro</h1>
    <h2>
        <asp:Literal runat="server" ID="litName"></asp:Literal><asp:Literal runat="server" ID="litBadgeId"></asp:Literal></h2>
    <div style="text-align:right;">
        <a href="/AllegroBadges.aspx">wszystkie kampanie</a>
    </div>
    <table>
        <tr>
            <td>Aktywny</td>
            <td><asp:DropDownList runat="server" ID="ddlIsAvtive">
                <asp:ListItem Value="0">NIE</asp:ListItem>
                <asp:ListItem Value="1">TAK</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Zakres rabatu cenowego</td>
            <td><asp:TextBox runat="server" ID="txbRebateFrom"></asp:TextBox> - <asp:TextBox runat="server" ID="txbRebateTo"></asp:TextBox>
                <asp:CompareValidator runat="server" ControlToValidate="txbRebateFrom" ControlToCompare="txbRebateTo"
                     Type="Currency" Operator="LessThanEqual" ErrorMessage="Błędny zakres" Display="Dynamic"></asp:CompareValidator>
                <asp:CompareValidator runat="server" ControlToValidate="txbRebateTo" ValueToCompare="20"
                     Type="Currency" Operator="LessThanEqual" ErrorMessage="Max górny zakres to 20%"></asp:CompareValidator>

            </td>
        </tr>
        
        <tr>
            <td>Producenci</td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:ListBox runat="server" ID="lbxSuppliers" Rows="8" SelectionMode="Multiple"
                                DataTextField="Name" DataValueField="SupplierId" Width="320"></asp:ListBox></td>
                        <td>
                            <asp:Button runat="server" ID="btnSupplierAdd" OnClick="btnSupplierAdd_Click" Text=">>" /><br />
                            <asp:Button runat="server" ID="btnSupplierDel" OnClick="btnSupplierDel_Click" Text="<<" />
                        </td>
                        <td>
                            <asp:ListBox runat="server" ID="lbxSuppliersSelected" Rows="8" SelectionMode="Multiple"
                                DataTextField="Name" DataValueField="SupplierId" Width="320"></asp:ListBox></td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr><td colspan="2"><asp:Button runat="server" ID="btnSave" Text="Zapisz" OnClick="btnSave_Click" OnClientClick="if(Page_ClientValidate()) return confirm('Zapisać zmiany?');" /></td></tr>
    </table>
</asp:Content>
