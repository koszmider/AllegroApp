<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Complaints.aspx.cs" Inherits="LajtIt.Web.Complaints" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table>
        <tr>
            <td>Status reklamacji</td>
            <td>Powód reklamacji</td>
            <td>Numer zamówienia</td>
            <td>Nazwa klienta</td>
            <td></td>
        </tr>
        <tr>
            <td rowspan="4">
                <asp:ListBox runat="server" ID="ddlComplaintStatus" DataValueField="ComplaintStatusId" DataTextField="Name"  Rows="5" SelectionMode="Multiple" ></asp:ListBox></td>
            <td rowspan="4">
                <asp:ListBox runat="server" ID="ddlComplaintType" DataValueField="OrderComplaintTypeId" DataTextField="ComplaintType"  Rows="5" SelectionMode="Multiple" ></asp:ListBox></td>
            <td>
                <asp:TextBox runat="server" ID="txbOrderId"></asp:TextBox></td>
            <td>
                <asp:TextBox runat="server" ID="txbClientName"></asp:TextBox></td>
            </tr>
        <tr>
            <td>Producent</td>
            <td>Produkt</td>

        </tr>
        <tr>
            <td>
              <asp:DropDownList runat="server" ID="ddlSuppliers" AppendDataBoundItems="true" DataTextField="Name"
                    DataValueField="SupplierId">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList></td>
                <td>    <div style="text-align: right;">



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
            <asp:UpdatePanel runat="server" ID="upNewProduct">
                <ContentTemplate>
              
                    <asp:TextBox runat="server" ID="txbProductCode" MaxLength="50" Width="150"></asp:TextBox>
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
        </div></td>
            </tr>
        <tr>
            <td><asp:CheckBox runat="server" ID="chbInvoiceCorrectionExpected" Text="Z korektą" /></td>
            <td>
                <asp:Button runat="server" Text="Szukaj" ID="btnSearch" OnClick="btnSearch_Click" /></td>
        
        </tr>
    </table>
    <br /><br />
    <asp:GridView runat="server" ID="gvComplaints" AllowSorting="true" AutoGenerateColumns="false"
        DataKeyNames="Id" Width="100%" OnSorting="gvComplaints_Sorting">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="Id" ItemStyle-Width="70" DataTextField="Id" DataNavigateUrlFormatString="Complaint.aspx?id={0}"
                
                HeaderText="Id" />
            <asp:BoundField DataField="InsertUser" HeaderText="Zgłosił" ItemStyle-Width="70" />
            <asp:BoundField DataField="InsertDate" HeaderText="Data zgłoszenia" ItemStyle-Width="70" DataFormatString="{0:yy-MM-dd}" SortExpression="InsertDate"/>
            <asp:BoundField DataField="LastUpdateDate" HeaderText="Data aktualizacji" ItemStyle-Width="70" DataFormatString="{0:yy-MM-dd HH:mm}" SortExpression="LastUpdateDate" />
            <asp:BoundField DataField="ClientName" HeaderText="Klient" />
            <asp:BoundField DataField="OrderComplaintType" HeaderText="Powód reklamacji" ItemStyle-Width="150" />
            <asp:BoundField DataField="OrderComplaintStatus" HeaderText="Status" ItemStyle-Width="150" />
            <asp:BoundField DataField="Comment" HeaderText="Uwagi" />
        </Columns>
    </asp:GridView>
</asp:Content>
