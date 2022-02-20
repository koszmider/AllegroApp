<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllegroMyUsers.aspx.cs" Inherits="LajtIt.Web.AllegroMyUsers" %>

<%@ Register Src="~/Controls/UploadImageAllegroUser.ascx" TagName="UploadImage" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Moje konta Allegro</h1>

    <asp:DropDownList runat="server" ID="ddlUsers" DataTextField="UserName" DataValueField="UserId" AppendDataBoundItems="true"
        AutoPostBack="true" OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged">
        <asp:ListItem></asp:ListItem>

    </asp:DropDownList>


    <asp:Panel runat="server" ID="pConfig" Visible="false">
        <uc:UploadImage ID="ucUploadImage" runat="server"></uc:UploadImage>
        <asp:Image runat="server" ID="imgHader" Visible ="false" style="max-width:800px" />
        <asp:LinkButton runat="server" OnClientClick="return confirm('Usunąć wybrane zdjęcie?')" Text="Usuń" ID="lbtnDelete" OnClick="lbtnDelete_Click" Visible="false"></asp:LinkButton>
    </asp:Panel>
</asp:Content>
