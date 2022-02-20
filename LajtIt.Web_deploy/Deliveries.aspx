<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Deliveries.aspx.cs"
    Inherits="LajtIt.Web.Deliveries" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Dostawy</h1>

    <table>
        <tr>
            <td>Data od - do</td>
            <td>Dostawca</td>
            <td>FV przypisana</td>
            <td>Cena</td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" ID="txbDateFrom" Width="70"></asp:TextBox><asp:CalendarExtender
                    ID="calDateFrom" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateFrom"></asp:CalendarExtender>
                -<asp:TextBox runat="server" ID="txbDateTo" Width="70"></asp:TextBox><asp:CalendarExtender
                    ID="calDateTo" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateTo"></asp:CalendarExtender>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlSupplierOwner" DataValueField="SupplierOwnerId" DataTextField="Name" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">wszyscy</asp:ListItem>
                </asp:DropDownList></td>
            <td>
                <asp:DropDownList runat="server" ID="ddlInvoice" Width="100">
                    <asp:ListItem>-</asp:ListItem>
                    <asp:ListItem>TAK</asp:ListItem>
                    <asp:ListItem>NIE</asp:ListItem>
                </asp:DropDownList></td>
            <td>
                <asp:DropDownList runat="server" ID="ddlPrice" Width="100">
                    <asp:ListItem Value="-1">-</asp:ListItem>
                    <asp:ListItem Value="1">TAK</asp:ListItem>
                    <asp:ListItem Value="0">NIE</asp:ListItem>
                </asp:DropDownList></td>
        
            <td>
                <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Szukaj" /></td>
        </tr>
        <tr>
            <td>Dokument dostawy</td>
            <td>Faktura</td>
            <td>Akceptowalna różnica</td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" ID="txbDocumentName"></asp:TextBox></td>
            <td>
                <asp:TextBox runat="server" ID="txbInvoiceSearch"></asp:TextBox></td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="txbPriceDiff" Text="0,1" Width="100" style="text-align:right"></asp:TextBox>
            
                <asp:CheckBox runat="server" ID="chbPriceDiff" Text="Pokaż różnice cenowe" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:GridView runat="server" ID="gvDeliveries" DataKeyNames="DeliveryId" AutoGenerateColumns="false" PageSize="1000"
        ShowFooter="true" OnRowDataBound="gvDeliveries_RowDataBound"
         OnRowEditing="gvDeliveries_RowEditing" OnRowCancelingEdit="gvDeliveries_RowCancelingEdit"
        OnRowUpdating="gvDeliveries_RowUpdating" >
        <Columns>
            <asp:TemplateField HeaderText="Lp" ItemStyle-Width="20px">
                <HeaderTemplate>
                    <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Literal ID="litCounter" runat="server"></asp:Literal>
                    <asp:CheckBox runat="server" ID="chbOrder" />
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:CommandField ItemStyle-Width="100" ShowCancelButton="true"  ShowEditButton="true" EditText="" CancelText="Anuluj" UpdateText="Zapisz" />

            <asp:TemplateField HeaderText="Cena zakupu<br>dostawa/katalog" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblPrice"></asp:Label><br />
                    
                    <asp:Label runat="server" ID="lblPriceCatalog"></asp:Label>
                </ItemTemplate> 
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txbPrice" onclick="this.select();" onfocusout="this.value=this.value.replace('.', ',');" style="text-align:right;" Width="70"></asp:TextBox><br />
                    
                    <asp:Label runat="server" ID="lblPriceCatalog"></asp:Label>
                </EditItemTemplate>
            </asp:TemplateField>
             
            <asp:TemplateField HeaderText="Ilość" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblQuantity"></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label runat="server" ID="lblQuantity"></asp:Label></FooterTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txbQuantity" onclick="this.select();" style="text-align:right;" Width="30"></asp:TextBox></EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Razem netto" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblPriceTotal"></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label runat="server" ID="lblPriceTotal"></asp:Label></FooterTemplate>
            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dostawa">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblDocumentName"></asp:Label><br />
                    <asp:Label runat="server" ID="lblInsertUser"></asp:Label><br />
                    <asp:Label runat="server" ID="lblInsertDate"></asp:Label>
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Produkt" ItemStyle-Width="400">
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlProductName" Target="_blank" NavigateUrl="/ProductCatalog.Preview.aspx?id={0}"></asp:HyperLink><br />
                    <asp:Label runat="server" ID="lblCode"></asp:Label><br />
                    <asp:Label runat="server" ID="lblEan"></asp:Label>
                </ItemTemplate> 
            </asp:TemplateField>  
            <asp:BoundField DataField="SupplierName" HeaderText="Producent" ReadOnly="true"  ItemStyle-Width="100"/>
            <asp:TemplateField HeaderText="Faktura" ItemStyle-Width="150">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblInvoice"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <table>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="ddlAction" Visible="false" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">-- wybierz akcję --</asp:ListItem>
                    <asp:ListItem Value="1">Przypisz fakturę</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td>
                <asp:Panel runat="server" GroupingText="Akcje" Visible="false" ID="pAction" ClientIDMode="Static">
                    <table>
                        <tr>
                            <td>Zastosuj do
                            </td>
                            <td>&nbsp;<asp:RadioButton runat="server" Enabled="false" GroupName="batch" ID="rbtnAddToBatchAll" Text="wszystkich wyszukanych produktów" />
                                <asp:RadioButton runat="server" GroupName="batch" ID="rbtnAddToBatchSelected" Checked="true" Text="wybranych produktów" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button runat="server" ID="btnAction" Text="Wykonaj" OnClick="btnAction_Click" /></td>
                        </tr>
                    </table>
                </asp:Panel>
                  <script type="text/javascript">

                      function goAutoCompl() {

                          $("#<%=txbInvoiceNumber.ClientID %>").autocomplete({
                                     source: function (request, response) {
                                         $.ajax({
                                             url: '<%=ResolveUrl("~/AutoComplete.asmx/GetInvoices") %>',
                                    data: "{ 'query': '<%=ddlSupplierOwner.SelectedValue%>|" + request.term + "'}",
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
                                $("#<%=hfInvoiceNumber.ClientID %>").val(i.item.val);
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
                <asp:Panel runat="server" ID="pnIvoice" Visible="false">
                    Wyszukaj numer faktury
                     <div>
                       
                         <asp:UpdatePanel runat="server" ID="upNewProduct">
                             <ContentTemplate>

                                 <asp:TextBox runat="server" ID="txbInvoiceNumber" MaxLength="50" Width="350" ValidationGroup="add"></asp:TextBox>


                                 <asp:HiddenField ID="hfInvoiceNumber" runat="server" />


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

                </asp:Panel>


            </td>
        </tr>
    </table>

</asp:Content>
