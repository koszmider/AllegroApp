<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopProducer.ascx.cs" Inherits="LajtIt.Web.Controls.ShopProducerControl" %>
<div style="text-align: right;">
    <script type="text/javascript">
        function goAutoCompl() {
            $("#<%=txbProducer.ClientID %>").autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/AutoComplete.asmx/GetShopProducers") %>',
                                data: "{ 'query': '" + $("#<%=hfShopId.ClientID %>").val() + "|" + request.term + "'}",
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
                            $("#<%=hfProducerId.ClientID %>").val(i.item.val);
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
    <asp:UpdatePanel runat="server" ID="upNewProduct">
        <ContentTemplate>

            <table style="width:100%; text-align:left">
                <tr>
                    <td style="width:220px">Wyszukaj producenta</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbProducer" MaxLength="50" Width="150"></asp:TextBox> 
    <asp:HiddenField ID="hfProducerId" runat="server" />
    <asp:HiddenField ID="hfShopId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Utwórz nowego producenta</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbProducerNew" MaxLength="50" Width="150"></asp:TextBox><asp:RequiredFieldValidator
                            runat="server" ID="rfv" ControlToValidate="txbProducerNew" ValidationGroup="create" Text="*"></asp:RequiredFieldValidator>
                        <asp:LinkButton runat="server" ID="lbtnProducerCreate" ValidationGroup="create" Text="utwórz" OnClick="lbtnProducerCreate_Click" OnClientClick="return confirm('Utworzyć?');"></asp:LinkButton>

                    </td>
                </tr>
            </table>




            <span style="position: absolute; width: 10px;">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upNewProduct">
                    <ProgressTemplate>
                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </span>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

