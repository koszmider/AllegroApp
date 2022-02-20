<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmailTestPage.aspx.cs" Inherits="LajtIt.Web.EmailTestPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

Do: <asp:TextBox runat="server" ID="txbTo" Text="jstawik@gazeta.pl"></asp:TextBox><br />
SMTP: <asp:TextBox runat="server" ID="txbSMTP"></asp:TextBox><br />
Port: <asp:TextBox runat="server" ID="txbPort"></asp:TextBox><br />
<asp:Button runat="server" OnClick="btnSend_Click" Text="Wyślij" />

</asp:Content>
