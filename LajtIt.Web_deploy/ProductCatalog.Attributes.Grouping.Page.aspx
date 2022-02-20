<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="ProductCatalog.Attributes.Grouping.Page.aspx.cs" Inherits="LajtIt.Web.ProductCatalogAttributesGroupingPage" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: right;">
        <asp:HyperLink runat="server" ID="hlPromotions" NavigateUrl="/ProductCatalog.Attributes.Grouping.aspx" Text="Zobacz wszystkie konfiguracje"></asp:HyperLink>
    </div>
    <script type="text/javascript">

                function goAutoCompl() {
                    $("#<%=txbAttribute.ClientID %>").autocomplete({
                                source: function (request, response) {
                                    $.ajax({
                                        url: '<%=ResolveUrl("~/AutoComplete.asmx/GetAttribute") %>',
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
                                    $("#<%=hfAttribute.ClientID %>").val(i.item.val);
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
    <asp:UpdatePanel runat="server">
    

        <ContentTemplate>
            <table>
                <tr>
                    <td >
                        <h2>Konfiguracja</h2>
                    </td>
                </tr>
                <tr> 
                    <td style="text-align:left;">Nazwa<br />
                        <asp:TextBox runat="server" ID="txbName" MaxLength="256" Width="500"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr> 
                    <td >
                        <table>
                            <tr>
                                <td>
                                <td colspan="2"> <asp:TextBox runat="server" ID="txbAttribute" Width="326"></asp:TextBox></td>
                                                   <asp:HiddenField ID="hfAttribute" runat="server" /></td>
                    <td><asp:Button runat="server" ID="btnAttributeAdd2" OnClick="btnAttributeAdd2_Click" Text=">>" /></td>
                            </tr>
                            <tr>
                                <td>Atrybuty</td>

                                <td>
                                    <asp:ListBox runat="server" ID="lbxAttributes" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="AttributeId" Width="320"></asp:ListBox></td>
                                <td>
                                    <asp:Button runat="server" ID="btnAttributeAdd" OnClick="btnAttributeAdd_Click" Text=">>" /><br />
                                    <asp:Button runat="server" ID="btnAttributeDel" OnClick="btnAttributeDel_Click" Text="<<" />
                                </td>
                                <td>
                                    <asp:ListBox runat="server" ID="lbxAttributesSelected" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="AttributeId" Width="320"></asp:ListBox></td>
                                <td>Lista atrybutów, które będą widoczne dla danej konfiguracji</td>
                            </tr>
                            <tr>
                                <td>Grupy</td>
                                <td>
                                    <asp:ListBox runat="server" ID="lbxAttributeGroups" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="AttributeGroupId" Width="320"></asp:ListBox></td>
                                <td>
                                    <asp:Button runat="server" ID="btnAttributeGroupAdd" OnClick="btnAttributeGroupAdd_Click" Text=">>" /><br />
                                    <asp:Button runat="server" ID="btnAttributeGroupDel" OnClick="btnAttributeGroupDel_Click" Text="<<" />
                                </td>
                                <td>
                                    <asp:ListBox runat="server" ID="lbxAttributeGroupsSelected" Rows="10" SelectionMode="Multiple" DataTextField="Name" DataValueField="AttributeGroupId" Width="320"></asp:ListBox></td>
                                <td>Lista grup atrybutów, które będą widoczne dla danej konfiguracji</td>
                            </tr>
                        </table>
                      </td>
                </tr>

            </table>




            <table>

                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" ID="btnSave" Text="Zapisz" OnClick="btnSave_Click" OnClientClick="return confirm('Zapisać zmiany?');" /></td>
                </tr>

            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
