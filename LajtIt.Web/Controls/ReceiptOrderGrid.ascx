<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReceiptOrderGrid.ascx.cs"
    Inherits="LajtIt.Web.Controls.ReceiptOrderGrid" %>
 
<asp:GridView runat="server" ID="gvUserOrders" AutoGenerateColumns="false" AllowSorting="false" EmptyDataText=""
    DataKeyNames="ReceiptId" Style="width: 100%" ShowFooter="false"
    OnRowDataBound="gvOrderReceipts_RowDataBound">
    <RowStyle HorizontalAlign="Center" VerticalAlign="Top" />
    <Columns>

        <asp:TemplateField HeaderText="Rodzaj paragonu" ItemStyle-Width="100">
            <ItemTemplate>
                <asp:Literal ID="litParType" runat="server" Text='<%# Eval("OrderReceiptTypeName") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Kwota" ItemStyle-Width="100">
            <ItemTemplate>
                <asp:Literal ID="litParAmount" runat="server" Text='<%# Eval("ReceiptAmount") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Data wystawienia" ItemStyle-Width="100">
            <ItemTemplate>
                <asp:Literal ID="litParDate" runat="server" Text='<%# Eval("InsertDate") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>
