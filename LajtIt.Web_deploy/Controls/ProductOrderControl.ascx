<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductOrderControl.ascx.cs" Inherits="LajtIt.Web.Controls.ProductOrderControl" %>

<asp:Panel runat="server" GroupingText="Zamawianie produktu">
    <table>
        <tr style="vertical-align:top">
            <td>Liczba zamawianych produktów</td>
            <td>
                <asp:TextBox runat="server" ID="txbQuantity" MaxLength="3" ValidationGroup="order" Text="1" TextMode="Number"></asp:TextBox></td>
            <td rowspan="2">Bieżące zamówienia<br />
                <asp:GridView runat="server" ID="gvProductOrder" AutoGenerateColumns="false" EmptyDataText="Brak zamówień w trakcie" DataKeyNames="Id"
                    OnRowDataBound="gvProductOrder_RowDataBound" OnRowDeleting="gvProductOrder_RowDeleting">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="true" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Quantity" HeaderText="Ilość"  ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="InsertDate" HeaderText="Data zamówienia" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                        <asp:BoundField DataField="InsertUser" HeaderText="Zamawiający" />
                        <asp:BoundField DataField="Comment" HeaderText="Uwagi" />

                    </Columns>

                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>Uwagi</td>
            <td>
                <asp:TextBox runat="server" ID="txbComment" ValidationGroup="order" TextMode="MultiLine" Rows="3"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ValidationGroup="order" Text="Zamów produkty"
                    OnClientClick="return confirm('Czy chcesz zamówić wskazaną liczbę produktów?');"
                    ID="btnProductOrder" OnClick="btnProductOrder_Click" />
            </td>
        </tr>

    </table>


</asp:Panel>
