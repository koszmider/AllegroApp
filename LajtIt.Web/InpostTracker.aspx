<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InpostTracker.aspx.cs" Inherits="LajtIt.Web.InpostTracker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:TextBox runat="server" ID="txbTrackingNumber"></asp:TextBox><asp:Button runat="server" ID="btnGo" OnClick="btnGo_Click" />
    <br />
    <asp:TextBox runat="server" TextMode="MultiLine" ID="txbResult" Width="700" Height="500"></asp:TextBox>
</asp:Content>
