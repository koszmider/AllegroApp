<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemMenu.ascx.cs" Inherits="LajtIt.Web.Controls.SystemMenu" %>
<table>
    <tr valign="top">
        <td>
            <asp:Label runat="server" ID="lblMenu" Visible="true" CssClass="menu"></asp:Label></td>
        <td>
            <asp:Repeater runat="server" ID="rpLinks" OnItemDataBound="rpLinks_ItemDataBound">
                <HeaderTemplate>
                    <ul id="menu2">
                        <li><a target="_blank" href="http://www.lajtit.pl">www.lajtit.pl</a></li>
                        <li><a target="_blank" href="https://www.facebook.com/Lajtit.Oswietlenie">fb lajtit</a></li>
                        <li><a target="_blank" href="https://www.facebook.com/oswietlenie.lajtit.lodz/">fb salon</a></li>
                        <li><a href="#">allegro</a><ul>
                </HeaderTemplate>
                <FooterTemplate></ul></li></ul></FooterTemplate>
                <ItemTemplate>
                    <li><asp:HyperLink runat="server" ID="hlAllegro" NavigateUrl="http://allegro.pl/listing/user.php?us_id={0}" Target="_blank"></asp:HyperLink></li>
                </ItemTemplate>
            </asp:Repeater>
        </td>
    </tr>
</table>




