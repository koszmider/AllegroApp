<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShopCategory.aspx.cs" Inherits="LajtIt.Web.ShopCategory"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
window.onload = function () {
    var div = document.getElementById("dvScroll");
    var div_position = document.getElementById("div_position");
    var position = parseInt('<%=Request.Form["div_position"] %>');
    if (isNaN(position)) {
        position = 0;
    }
    div.scrollTop = position;
    div.onscroll = function () {
        div_position.value = div.scrollTop;
    };
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table style="width:100%">
        <tr valign="top">
            <td style="width:200px;">
                <asp:Button runat="server" ID="btnDeleteNotExisting" OnClick="btnDeleteNotExisting_Click" Text="Usuń nieistniejące kategorie" Visible="false" />

                <asp:TextBox runat="server" ID="txbSearch" AutoPostBack="true" OnTextChanged="txbSearch_TextChanged"></asp:TextBox>
                    <input type="hidden" id="div_position" name="div_position" />
                <div style="overflow-y: scroll; height: 760px; width: 300px" id="dvScroll">
                    <style>
                        .tv img {width:22px;}

                    </style>
                    <asp:TreeView runat="server" ID="tvCategory" CssClass="tv" ShowExpandCollapse="true" OnSelectedNodeChanged="tvCategory_SelectedNodeChanged" ExpandDepth="0" SelectedNodeStyle-Font-Bold="true">
                    </asp:TreeView>

                </div>


            </td>
            <td>
                <asp:Label runat="server" ID="lblInfo" Visible="false" Text="Kategoria nie istnieje"></asp:Label>
                <asp:Panel ID="pnCategory" runat="server" Visible="false">
                    <h2>Bieżące ustawienia</h2>
                    <table>
                        <tr>
                            <td>Nazwa</td>
                            <td>
                                <asp:Label runat="server" ID="lblName"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Url</td>
                            <td>
                                <asp:HyperLink runat="server" ID="hlLink" Target="_blank"></asp:HyperLink></td>
                        </tr>
                        <tr>
                            <td>Aktywny</td>
                            <td>
                                <asp:CheckBox runat="server" ID="chbIsActive" /></td>
                        </tr>
                    </table>
                    <h2>Konfiguracja</h2>
                    <table style="width:100%">
                        <tr>
                            <td>Nazwa</td>
                            <td><asp:TextBox runat="server" ID="txbName" style="width:100%"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Opis</td>
                            <td><asp:TextBox runat="server" ID="txbDesc" TextMode="MultiLine" Rows="10" style="width:100%"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Seo - tytuł</td>
                            <td><asp:TextBox runat="server" ID="txbSeoTitle" style="width:100%"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Seo - opis</td>
                            <td><asp:TextBox runat="server" ID="txbSeoDesc" style="width:100%"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Seo - keywords</td>
                            <td><asp:TextBox runat="server" ID="txbSeoKeywords" style="width:100%"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Url</td>
                            <td><asp:TextBox runat="server" ID="txbUrl" style="width:100%"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2"><asp:Image ImageUrl="~/Images/ok.jpg" Width="50" ID="imgOK"  runat="server"/><asp:Image runat="server" ImageUrl="~/Images/false.jpg" Width="50" ID="imgFalse" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button runat="server" ID="btnSave" Text="Zapisz zmiany" OnClick="btnSave_Click" OnClientClick="reurn confirm('Czy zapisać zmiany?');" />
                                <asp:Button runat="server" ID="btnPublish" Text="Publikuj zmiany" OnClick="btnPublish_Click" OnClientClick="reurn confirm('Czy opublikować zmiany?');" />
                                <asp:LinkButton runat="server" ID="lbtnDelete" Text="Usuń kategorię" OnClick="lbtnDelete_Click" ForeColor="Red" OnClientClick="reurn confirm('Czy usunąć kategorię?');"></asp:LinkButton></td>
                        </tr>
                    </table>
    </asp:Panel>
            </td>
        </tr>
    </table>


</asp:Content>


