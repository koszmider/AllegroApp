<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemOrderGrid.ascx.cs"
    Inherits="LajtIt.Web.Controls.ItemOrderGrid" %>
<asp:Literal runat="server" ID="lit"></asp:Literal>
    <script type="text/javascript">

        function goAutoComplProduct(t, s, h) {
        
            $("#" + t).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/AutoComplete.asmx/GetProductsBySupplier") %>',
                                                data: "{ 'supplierId': '" + $("#" + s).val() + "', 'query': '" + request.term + "'}",
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
                                            $("#" + h).val(i.item.val);
                                        },
                                        minLength: 1
                                    });

        }

        $(document).ready(function () {
            goAutoComplProduct();
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            goAutoComplProduct();
        }

    </script>
<asp:GridView runat="server" ID="gvUserOrders" AutoGenerateColumns="false" AllowSorting="false" EmptyDataText="Brak produktów. Odśwież stronę."
    DataKeyNames="OrderProductId" OnRowEditing="gvUserOrders_OnRowEditing" OnRowCancelingEdit="gvUserOrders_OnRowCancelingEdit"
    OnRowUpdating="gvUserOrders_OnRowUpdating" Style="width: 100%" ShowFooter="true"
    OnRowDataBound="gvUserOrders_RowDataBound">
    <RowStyle HorizontalAlign="Center" VerticalAlign="Top" />
    <Columns>
        <asp:CommandField ItemStyle-Width="70" ShowCancelButton="true" ShowEditButton="true" ValidationGroup="pc"
            ButtonType="Image" EditImageUrl="~/Images/edit.jpg" UpdateImageUrl="~/Images/save.jpg"
            CancelImageUrl="~/Images/cancel.jpg" />
        <asp:TemplateField HeaderText="Lp" ItemStyle-Width="20px">
            <HeaderTemplate>
                <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Literal ID="litCounter" runat="server"></asp:Literal>
                <itemtemplate><asp:Literal runat="server" ID="liId"></asp:Literal></itemtemplate>
                <asp:Literal runat="server" ID="LitId"></asp:Literal>
                <asp:CheckBox runat="server" ID="chbOrder" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Produkt">
            <ItemTemplate>
                <asp:Literal ID="litProductNameItem" runat="server" Text='<%# Eval("ProductName") %>'></asp:Literal><br />
                <asp:HyperLink ID="hlProductCatalogName" runat="server" NavigateUrl="/ProductCatalog.Preview.aspx?id={0}" Target="_blank"></asp:HyperLink>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
            <EditItemTemplate>
                <asp:UpdatePanel runat="server" ID="upSupplier">
                    <ContentTemplate>
                        <b>
                            <asp:Literal ID="litProductNameEdit" runat="server" Text='<%# Eval("ProductName") %>'></asp:Literal></b>
                        <asp:Panel runat="server" ID="pnStatus" Visible="false" GroupingText="Produkt z katalogu">
                        

                            <table>
                                <tr>
                                    <td>

                                        <asp:DropDownList Width="350" runat="server" ID="ddlSupplier" DataValueField="SupplierId" DataTextField="DisplayName">
                                        </asp:DropDownList>
                                        <span style="position: absolute;">
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upSupplier">
                                                <ProgressTemplate>
                                                    <img src="Images/progress.gif" style="height: 20px" alt="" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </span>

                                    </td>
                                </tr>
                                <tr>
                                    <td>

                                        <asp:TextBox runat="server" ID="txbProductCode" ValidationGroup="pc" MaxLength="50" Width="350"></asp:TextBox>
                                        <asp:HiddenField ID="hfProductCatalogId" runat="server" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvProductCode" Text="wymagane"
                                            ValidationGroup="pc" ControlToValidate="txbProductCode"></asp:RequiredFieldValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Label runat="server" ID="lblItemCodeFromAuction"></asp:Label></td>
                                </tr>


                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>



            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Ext Id">
            <ItemStyle Width="80" />
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="hlExternalProduct" Target="_blank"></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Szt.">
            <ItemTemplate>


                <asp:UpdatePanel runat="server" ID="upQ">
                    <ContentTemplate>
                        <asp:LinkButton ID="lbtnQuantity" ToolTip="Kliknij i sprawdź stany magazynowe" runat="server" CommandArgument='<%# Eval("ProductCatalogId") %>' Text='<%# Eval("Quantity") %>' OnClick="lbtnQuantity_Click"></asp:LinkButton>

                        <span style="position: absolute;">
                            <asp:UpdateProgress ID="upQ1" runat="server" AssociatedUpdatePanelID="upQ">
                                <ProgressTemplate>
                                    <img src="Images/progress.gif" style="height: 20px" alt="" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </span>
                    </ContentTemplate>
                </asp:UpdatePanel>



            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txbQuantity" MaxLength="3" Width="20" runat="server" Text='<%# Eval("Quantity") %>'></asp:TextBox><asp:RegularExpressionValidator
                    runat="server" ControlToValidate="txbQuantity" ValidationExpression="^[0-9]{1,}$"
                    Text="*" />
            </EditItemTemplate>
            <FooterTemplate>
                <asp:Literal runat="server" ID="litQuantityFooter" />
            </FooterTemplate>
            <FooterStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Rabat" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60">
            <ItemTemplate>
                <asp:Literal ID="litRebate" runat="server" Text='<%# String.Format("{0}%", Eval("Rebate")) %>'></asp:Literal>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txbRebate" Style="text-align: right;" MaxLength="10" Width="40"
                    runat="server" Text='<%# Eval("Rebate") %>'></asp:TextBox><asp:RegularExpressionValidator
                        ID="revRebate" runat="server" ControlToValidate="txbRebate" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                        Text="*" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="VAT" ItemStyle-HorizontalAlign="Right">
            <ItemTemplate>
                <asp:Literal ID="litVAT" runat="server"></asp:Literal>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlVAT" runat="server">
                    <asp:ListItem Value="-1">-</asp:ListItem>
                    <asp:ListItem Value="0">0%</asp:ListItem>
                    <asp:ListItem Value="0,22" Enabled="false">22%</asp:ListItem>
                    <asp:ListItem Value="0,23">23%</asp:ListItem>
                </asp:DropDownList>
                <asp:CompareValidator runat="server" ControlToValidate="ddlVAT" ValueToCompare="-1"
                    Text="*" Operator="NotEqual"></asp:CompareValidator>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Cena" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100">
            <ItemTemplate>
                <asp:Literal ID="litPrice" runat="server" Text='<%# String.Format("{0:0.00} PLN", Eval("Price")) %>'></asp:Literal><br />
                <asp:Label runat="server" ID="lbPriceAmount"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txbPrice" Style="text-align: right;" MaxLength="10" Width="50" runat="server"
                    Text='<%# Eval("Price") %>'></asp:TextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                        runat="server" ControlToValidate="txbPrice" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                        Text="*" /><br />
                <asp:Label runat="server" ID="lbPriceAmount"></asp:Label>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Razem" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100">
            <ItemTemplate>
                <asp:Literal ID="litTotal" runat="server"></asp:Literal><br />
                <asp:Label runat="server" ID="lbPriceAmountTotal"></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Literal runat="server" ID="litTotalFooter" />
            </FooterTemplate>
            <FooterStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Komenatarz" ItemStyle-HorizontalAlign="left" ItemStyle-Width="160">
            <ItemTemplate>
                <asp:Literal ID="Literal3" runat="server" Text='<%#   Eval("Comment") %>'></asp:Literal>
            </ItemTemplate>

            <EditItemTemplate>
                <asp:TextBox ID="txbComment" Width="120" TextMode="MultiLine" Height="60" runat="server"
                    Text='<%# Eval("Comment") %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="left" ItemStyle-Width="160">
            <ItemStyle HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Literal ID="litStatusName" runat="server" Text='<%#   Eval("StatusName") %>'></asp:Literal>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Literal ID="litStatusName" runat="server" Visible="false" ></asp:Literal>
                <asp:DropDownList ID="ddlOrderProductStatus" runat="server" DataValueField="OrderProductStatusId"
                    DataTextField="StatusName">

                    <asp:ListItem Value="1">Nowy</asp:ListItem>
                    <asp:ListItem Value="4">Zamowiony</asp:ListItem>
                    <asp:ListItem Value="2">Wydany</asp:ListItem>
                    <asp:ListItem Value="3">Do wydania</asp:ListItem>
                    <asp:ListItem Value="0">Usunięty</asp:ListItem>
                </asp:DropDownList>

            </EditItemTemplate>
            <HeaderTemplate>
                <asp:DropDownList ID="ddlStatusName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusName_SelectedIndexChanged">
                    <asp:ListItem Value="-1">Status</asp:ListItem>
                    <asp:ListItem Value="1">Nowy</asp:ListItem>
                    <asp:ListItem Value="4">Zamowiony</asp:ListItem>
                    <asp:ListItem Value="2">Wydany</asp:ListItem>
                    <asp:ListItem Value="3">Do wydania</asp:ListItem>
                    <asp:ListItem Value="0">Usunięty</asp:ListItem>
                </asp:DropDownList>
            </HeaderTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
