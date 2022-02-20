<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopCategoryControl.ascx.cs"
    Inherits="LajtIt.Web.Controls.ShopCategoryControl" %>
<asp:ListBox   Font-Names="Courier" runat="server" Rows="6" ID="lsbCategories"
    DataValueField="CategoryId" DataTextField="Name"></asp:ListBox>
<asp:HiddenField runat="server" ID="hidSelected" />