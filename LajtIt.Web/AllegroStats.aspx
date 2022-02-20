<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllegroStats.aspx.cs" Inherits="LajtIt.Web.AllegroStats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width: 200px; float: left;">
        <table>
           
            <tr>
                <td>
                    Miesiąc:<br />
                    <asp:ListBox runat="server" ID="lsbxMonths" SelectionMode="Single" Style="width: 100%;"
                        ValidationGroup="stat"></asp:ListBox>
                    <br />
                    <asp:RequiredFieldValidator runat="server" ValidationGroup="stat" ID="RequiredFieldValidator1"
                        ControlToValidate="lsbxMonths"><span style="color:Red;">wymagane</span></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Wartość min:<br />
                    <asp:TextBox runat="server" ID="txbItemsValue" Text="500" ValidationGroup="stat"
                        MaxLength="10" /><asp:RequiredFieldValidator runat="server" Display="Dynamic" ValidationGroup="stat"
                            ID="RequiredFieldValidator2" ControlToValidate="txbItemsValue"><span style="color:Red;">wymagane</span></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                runat="server" ValidationGroup="stat" ValidationExpression="[0-9]{1,10}" ControlToValidate="txbItemsValue"><span style="color:Red;">podaj liczbę</span></asp:RegularExpressionValidator>
                </td>
            </tr>
           
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnShow" OnClick="btnShow_Click" Text="Pokaż" ValidationGroup="stat" />
                                <span style="position: absolute;">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" >
                                        <ProgressTemplate>
                                            <img src="Images/progress.gif" style="height: 20px" alt="" /></ProgressTemplate>
                                    </asp:UpdateProgress>
                                </span>
                </td>
            </tr>
        </table>
    </div>
    <div style="float: left;">
        <asp:GridView runat="server" ID="gvStats" AutoGenerateColumns="false" AllowSorting="true"
            EmptyDataText="Brak danych. Zmień kryteria wyszukiwania i spróbuj ponownie."
            OnSorting="gvStats_OnSorting" ShowFooter="true" OnRowDataBound="gvStats_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Lp." ItemStyle-Width="15">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="litId" /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mies." ItemStyle-Width="15">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="litMonth" Text='<%# Eval("Year") +"/" +Eval("Month") %>' /></ItemTemplate>
                </asp:TemplateField>

                    <asp:TemplateField HeaderText="Użytkownik" SortExpression="ItemsOrdered" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlUserName" Target="_blank" />
                    </ItemTemplate>
                </asp:TemplateField>

                 
                <asp:TemplateField HeaderText="Razem" SortExpression="ItemsOrdered" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Literal ID="l1" runat="server" Text='<%# Eval("ItemsOrdered") %>' />/<b><asp:Literal
                            ID="l2" runat="server" Text='<%# Eval("ItemsValue") %>' /></b>
                    </ItemTemplate>
                </asp:TemplateField>
               
                <asp:TemplateField HeaderText="Akcje">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" BorderWidth="0" ID="hlViewAuctions" Target="_blank"><img src="Images/view.png" style="border:0px;" alt="Pokaż aukcje użytkownika w danym okresie" /></asp:HyperLink>
                        <asp:ImageButton runat="server"
                         ID="imbByDay" OnClick="imbByDay_Click" ImageUrl="images/view.png" CommandArgument='<%# Eval("UserId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div>
        <asp:GridView runat="server" ID="gvStatsByDay" AutoGenerateColumns="false" AllowSorting="false" Visible="false" EnableViewState="false"
            EmptyDataText="Brak danych. " ShowFooter="true" OnRowDataBound="gvStatsByDay_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Day" HeaderText="Dzień" />
                <asp:BoundField DataField="ItemsOrdered" HeaderText="Liczba" SortExpression="ItemsOrdered" /> 
                <asp:TemplateField HeaderText="Wartość">
                <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:HyperLink runat="server" BorderWidth="0" ID="hlViewAuctions" Target="_blank"><img src="Images/view.png" style="border:0px;" alt="Pokaż aukcje użytkownika w danym okresie" /></asp:HyperLink></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
