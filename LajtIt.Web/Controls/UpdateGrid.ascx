<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateGrid.ascx.cs"
    Inherits="LajtIt.Web.Controls.UpdateGrid" %>
 

<asp:GridView runat="server" ID="gvPromos" AutoGenerateColumns="false" AllowSorting="false" EmptyDataText=""
    DataKeyNames="UpdateId" Style="width: 85%" ShowFooter="false"
    OnRowDataBound="gvPromos_RowDataBound" OnRowCommand="gvPromos_OnRowCommand">
    <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
    <Columns>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="" ItemStyle-Width="5%">
            <ItemTemplate>
                <asp:Button runat="server" ID="btnDel" CommandArgument='<%# Eval("UpdateId") %>' CommandName="IdDelete" Visible="true" Text="Usuń" OnClick="btnDel_Click" OnClientClick="return confirm('Czy na pewno usunąć to zaplanowane zadanie?');" />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Opis aktualizacji" ItemStyle-Width="25%">
            <ItemTemplate>
                <asp:Literal ID="litDesc" runat="server" Text='<%# Eval("Description") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Uruchomienie" ItemStyle-Width="20%">
            <ItemTemplate>
                <asp:Literal ID="litStartDate" runat="server" Text='<%# Eval("StartDate") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Czy aktywna" ItemStyle-Width="10%">
            <ItemTemplate>
                <asp:Literal ID="litIsActive" runat="server" Text='<%# Eval("IsActive") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="FileId" ItemStyle-Width="10%">
            <ItemTemplate>
                <asp:Literal ID="litFileId" runat="server" Text='<%# Eval("FileId") %>'></asp:Literal><br />
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>
