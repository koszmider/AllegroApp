<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InvoiceCorrection.aspx.cs" Inherits="LajtIt.Web.InvoiceCorrection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Korekta faktury </h1>
    <div style="text-align: right">
        <asp:HyperLink runat="server" ID="hlOrder" Text="Wróć do zamówienia"></asp:HyperLink></div>
    <br />
    <asp:gridview runat="server" id="gvUserOrders" autogeneratecolumns="false" allowsorting="false"
        datakeynames="CorrectionInvoiceProductId" style="width: 100%" showfooter="true"
        onrowdatabound="gvUserOrders_OnRowDataBound">
        <rowstyle horizontalalign="Center" verticalalign="Top" />
        <columns>
    
 



        <asp:TemplateField HeaderText="Produkt" ItemStyle-Width="460px">
            <ItemTemplate>
                <asp:Literal ID="litProductNameItem" runat="server" Text='<%# Eval("Name") %>'></asp:Literal><br />
                <asp:TextBox ID="txbProductName" runat="server" Text='<%# Eval("CorrectionName") %>' width="420"></asp:TextBox>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
            
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Szt." ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60">
            <ItemTemplate>

                 
                <asp:Literal ID="litQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal><br />
                <asp:TextBox ID="txbQuantity" runat="server" style="text-align:right" Text='<%# Eval("CorrectionQuantity") %>' width="30"></asp:TextBox>
              



            </ItemTemplate> 
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Rabat" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60">
            <ItemTemplate> 
                <asp:Literal ID="litRebate" runat="server" Text='<%# Eval("Rebate") %>'></asp:Literal><br />
                <asp:TextBox ID="txbRebate" runat="server" style="text-align:right" Text='<%# Eval("CorrectionRebate") %>' width="30"></asp:TextBox>
            </ItemTemplate> 
        </asp:TemplateField>
        <asp:TemplateField HeaderText="VAT" ItemStyle-HorizontalAlign="Right" itemstyle-width="60">
            <ItemTemplate> 
                <asp:Literal ID="litVAT" runat="server" Text='<%# Eval("VatRate") %>'></asp:Literal><br />
                <asp:TextBox ID="txbVAT" runat="server" style="text-align:right" Text='<%# Eval("CorrectionVatRate") %>' width="30"></asp:TextBox>
            </ItemTemplate> 
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Cena" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="90">
            <ItemTemplate> 
                <asp:Literal ID="litPrice" runat="server" Text='<%# Eval("PriceBrutto") %>'  ></asp:Literal><br />
                <asp:TextBox ID="txbPrice" runat="server"  style="text-align:right" width="90" Text='<%# Eval("CorrectionPriceBrutto") %>'></asp:TextBox>
            </ItemTemplate> 
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Razem" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="90">
            <ItemTemplate>
                <asp:Literal ID="litTotal" runat="server" Text='<%# Eval("CalculatedTotalBrutto") %>'  ></asp:Literal><br />
                <asp:Literal ID="litTotalCorrection" runat="server" Text='<%# Eval("CorrectionCalculatedTotalBrutto") %>'  ></asp:Literal>
            </ItemTemplate> 
        </asp:TemplateField> 
    </columns>
    </asp:gridview>

    <script>


        function setVal(t) {
           
            $("#<%=txbComment.ClientID %>").val(t);
        }

      
    </script>
    <table>
        <tr>
            <td>Uwagi na fakturze<br /><asp:TextBox runat="server" ID="txbComment" TextMode="MultiLine" Columns="60" Rows="5"></asp:TextBox></td>
            <td>Wstaw opis lub wpiszę ręcznie<br />
                <asp:LinkButton  runat="server" CausesValidation="false"  OnClientClick="setVal(this.text);return false;" Text="Zwrot towaru w ramach odstąpienia od umowy"></asp:LinkButton><br />
                <asp:LinkButton  runat="server" CausesValidation="false"  OnClientClick="setVal(this.text);return false;" Text="Reklamacja towaru"></asp:LinkButton><br />
            </td>
        </tr>
    </table>

    <asp:button runat="server" id="btnSave" text="Zapisz" onclick="btnSave_Click" />
    <asp:LinkButton runat="server" id="btnInvoiceCorrection" text="Pobierz/podgląd" onclick="btnInvoiceCorrection_Click"
 />


    <div style="text-align:right">

    <asp:button runat="server" id="btnLock" text="Zablokuj fakturę" onclick="btnLock_Click" OnClientClick="return confirm('Utworzyć i zablokować korektę?');" />
    </div>
    <asp:updatepanel id="UpdatePanel1" runat="server">
        <triggers>
                                       
                                        <asp:PostBackTrigger ControlID="btnInvoiceCorrection" />
                                    </triggers>
    </asp:updatepanel>
</asp:Content>
