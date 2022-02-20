<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductCatalog.Preview.aspx.cs" Inherits="LajtIt.Web.ProductCatalog_Preview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/Styles/Site.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="">

            <table style="background-color: white; width: 1000px; display: block; margin-left: auto; margin-right: auto">
                <tr>
                    <td colspan="2">
                        <h1>
                            <asp:Label runat="server" ID="lblName"></asp:Label></h1>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right;">
                        <asp:DropDownList runat="server" ID="ddlShop" DataValueField="ShopId" DataTextField="Name" AutoPostBack="true" 
                            OnSelectedIndexChanged="ddlShop_SelectedIndexChanged"></asp:DropDownList>


                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right;">
                        <asp:HyperLink runat="server" ID="hlProduct" NavigateUrl="/Product.aspx?id={0}">Pokaż kartę produktu</asp:HyperLink></td>
                </tr>
                <tr valign="top">
                    <td style="width: 600px;">
                        <asp:Repeater runat="server" ID="rpImages" OnItemDataBound="rpImages_ItemDataBound">
                            <HeaderTemplate>
                                <table>
                            </HeaderTemplate>
                            <FooterTemplate></table></FooterTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Image runat="server" ID="imgImage" Style="max-width: 400px" />

                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div style="padding:30px;">
                            <asp:Label runat="server" ID="lblDescriptionInfo"  ForeColor="Red"></asp:Label><br />
                        <asp:Label runat="server" ID="lblDescription"></asp:Label><br /></div>
                        
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblSpecification"></asp:Label><br />
                        <br />
                        <asp:Label runat="server" ID="lblPreview" ForeColor="Red" Text="Wyszarzone opisy nie są przekazywane do Allegro"></asp:Label>
                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
