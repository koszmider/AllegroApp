<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OrderNew.aspx.cs" Inherits="LajtIt.Web.OrderNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    Podaj email/nazwę klienta w celu wyszukania danych:<br />
    <asp:TextBox runat="server" ID="txbName" MaxLength="50" ValidationGroup="search"></asp:TextBox><asp:RequiredFieldValidator
        runat="server" ID="rfv" ControlToValidate="txbName" Text="*" ValidationGroup="search" /><asp:Button
            runat="server" ID="btnSearch" Text="Szukaj" ValidationGroup="search" OnClick="btnSearch_Click" />
    <asp:GridView runat="server" ID="gvClients" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lbtnCreateOrder" CommandArgument='<%# Eval("OrderId") %>'
                        CommandName="createOrder" Text="Użyj" OnClick="lbtnCreateOrder_Click"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Client" HeaderText="Dane klienta" HtmlEncode="false" /> 
            <asp:BoundField DataField="CreateDate" DataFormatString="{0:yy-MM-dd}" HeaderText="Data utworzenia" />
        </Columns>
    </asp:GridView>
    <asp:Panel runat="server" ID="pOrderNew" Visible="false"><br /><br />
        Możesz użyć danych klienta znalezionego powyżej lub utwórz nowe, puste zamówienie:<br />
        <asp:Button runat="server" ID="btnOrderNew" Text="Nowe zamówienie" OnClick="btnOrderNew_Click" /></asp:Panel>
</asp:Content>
