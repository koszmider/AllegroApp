<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Product.Compare.ascx.cs" Inherits="LajtIt.Web.Controls.Products" %>
<asp:Panel runat="server">
    <script type="text/javascript">

        function goAutoCompl() {
            $("#<%=txbProductCode.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/AutoComplete.asmx/GetProducts") %>',
                                data: "{ 'query': '" + request.term + "'}",
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    response($.map(data.d, function (item) {
                                        return {
                                            label: item.split('|')[0],
                                            val: item.split('|')[1]
                                        }
                                    }))
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                },
                select: function (e, i) {
                    $("#<%=hfProductCatalogId.ClientID %>").val(i.item.val);
                },
                minLength: 1
            });

        }

        $(document).ready(function () {
            goAutoCompl();
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            goAutoCompl();
        }

    </script>

    <asp:UpdatePanel runat="server" ID="upProducts">
        <ContentTemplate>
            <div style="text-align: right">
                Kod/Ean/Nazwa:
                    <asp:TextBox runat="server" ID="txbProductCode" MaxLength="50" Width="150"></asp:TextBox><asp:Button
                        ID="btnProductCompare"
                        runat="server" OnClick="btnProductCompare_Click" Text="Porównaj" />&nbsp;&nbsp;&nbsp;
                <asp:HiddenField ID="hfProductCatalogId" runat="server" />
            </div>
            <asp:Label runat="server" ID="lblPreview"></asp:Label>
            <asp:LinkButton runat="server" ID="lbtnCopy" Visible="false"
                OnClientClick="return confirm ('Czy zastąpić wszystkie atrybuty?')" OnClick="lbtnCopy_Click"
                Text="Skopiuj atrybuty do produktu"></asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
