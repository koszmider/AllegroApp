<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebApiTestPage.aspx.cs" Inherits="LajtIt.Web.WebApiTestPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:DropDownList runat="server" ID="ddlUserId"><asp:ListItem Value="678165">JacekStawicki</asp:ListItem><asp:ListItem Value="28277822">CzerwoneJablko</asp:ListItem></asp:DropDownList><br />
ItemId: <asp:TextBox runat="server" ID="txbItemId"></asp:TextBox><asp:Button runat="server" ID="btnShow" OnClick="btnShow_Click" Text="Pokaż" />
<br /><br />

<b>doGetSiteJournal</b>
<asp:GridView runat="server" ID="gvGetSiteJournal" /><br />
<b>GetSiteJournalDeal</b>
<asp:GridView runat="server" ID="gvGetSiteJournalDeal" /><br />
<b>doGetPostBuyFormsDataForSellers</b>
<asp:GridView runat="server" ID="gvPostBuyFormDataStruct" />
</asp:Content>
