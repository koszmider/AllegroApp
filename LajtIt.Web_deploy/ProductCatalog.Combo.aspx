<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true" CodeBehind="ProductCatalog.Combo.aspx.cs"
    Inherits="LajtIt.Web.ProductCombo" %>

<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Produkt kombinowany</h1>
    
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
    <uc:ProductMenu runat="server" SetTab="td13"></uc:ProductMenu>
    <asp:Panel runat="server" ID="pnCombo">
        <table>
            <tr>
                <td>Wyznaczanie ceny</td>
                <td><asp:RadioButtonList runat="server" ID="rblPrice" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Selected="true">Cena stała</asp:ListItem>
                    <asp:ListItem Value="2">Cena obliczana</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
        </table>
        <div style="text-align: right">
            <asp:UpdatePanel runat="server" ID="upNewProduct">
                <ContentTemplate>
                    Kod/Ean/Nazwa:
                    <asp:TextBox runat="server" ID="txbProductCode" MaxLength="50" Width="150"></asp:TextBox><asp:Button ID="btnProductAdd"
                        runat="server" OnClick="btnProductAdd_Click" Text="Dodaj produkt" />&nbsp;&nbsp;&nbsp;
    <asp:HiddenField ID="hfProductCatalogId" runat="server" />


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

        <br />
        <br />
        <asp:GridView runat="server" ID="gvProducts" AutoGenerateColumns="false" OnRowDataBound="gvProducts_OnRowDataBound" DataKeyNames="Id">

            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemStyle Width="20" HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="liId"></asp:Literal>
                        <asp:Literal runat="server" ID="LitId"></asp:Literal>
                        <asp:CheckBox runat="server" ID="chbOrder" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Literal runat="server" ID="litTotal"></asp:Literal>
                    </FooterTemplate>
                    <HeaderTemplate>
                        <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Width="150" />
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlPreview" Target="_blank" NavigateUrl="/ProductCatalog.Preview.aspx?id={0}">
                            <asp:Image runat="server" ID="imgImage" Width="150" />
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Kod">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSupplierName"></asp:Label><br />
                        <asp:Label runat="server" ID="lblCode"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFormatString="/ProductCatalog.Preview.aspx?id={0}" Target="_blank" ItemStyle-Width="300"
                    DataTextField="Name" DataNavigateUrlFields="ProductCatalogRefId" HeaderText="Produkt" />
                
                <asp:CheckBoxField HeaderText="Aktywny" DataField="IsActive" />
                <asp:BoundField HeaderText="Cena" DataFormatString="{0:C}" DataField="PriceBruttoFixed" />
                <asp:TemplateField HeaderText="Ilość">
                    <ItemStyle Width="60" />
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txbQuantity" Width="60" Text="1"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rabat">
                    <ItemStyle Width="60" />
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txbRebate" Width="60" Text="0"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Usuń">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chbDelete" ForeColor="Red" Text="" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <br />
        <br />
        <asp:Button runat="server" ID="btnSave" Text="Zapisz" OnClick="btnSave_Click" OnClientClick="return confirm('Zapisać zmiany?');" />
    </asp:Panel>
    <asp:Label runat="server" ID="lblInfo" Text="Produkt nie jest kombinowany z kilku podproduktów." Visible="false"></asp:Label>


</asp:Content>
