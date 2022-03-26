<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromoGrid.ascx.cs"
    Inherits="LajtIt.Web.Controls.PromoGrid" %>
 

<asp:GridView runat="server" ID="gvPromos" AutoGenerateColumns="false" AllowSorting="false" EmptyDataText=""
    DataKeyNames="PromotionId" Style="width: 85%" ShowFooter="false"
    OnRowDataBound="gvPromos_RowDataBound" OnRowCommand="gvPromos_OnRowCommand">
    <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
    <Columns>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="" ItemStyle-Width="5%">
            <ItemTemplate>
                <asp:Button runat="server" ID="btnDel" CommandArgument='<%# Eval("PromotionId") %>' CommandName="IdDelete" Visible="true" Text="Usuń" OnClick="btnDel_Click" OnClientClick="return confirm('Czy na pewno usunąć tą promocję?');" />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Opis promocji" ItemStyle-Width="25%">
            <ItemTemplate>
                <asp:Literal ID="litDesc" runat="server" Text='<%# Eval("Description") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Wartość [%]" ItemStyle-Width="10%">
            <ItemTemplate>
                <asp:Literal ID="litPercentValue" runat="server" Text='<%# Eval("PercentValue") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Początek" ItemStyle-Width="20%">
            <ItemTemplate>
                <asp:Literal ID="litStartDate" runat="server" Text='<%# Eval("StartDate") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Koniec" ItemStyle-Width="20%">
            <ItemTemplate>
                <asp:Literal ID="litEndDate" runat="server" Text='<%# Eval("EndDate") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Czy aktywna" ItemStyle-Width="10%">
            <ItemTemplate>
                <asp:Literal ID="litIsActive" runat="server" Text='<%# Eval("IsActive") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Czy trwa" ItemStyle-Width="10%">
            <ItemTemplate>
                <asp:Literal ID="litIsGoingOn" runat="server" Text='<%# Eval("IsGoingOn") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>
