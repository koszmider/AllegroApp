<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Payments.Marketplace.aspx.cs" Inherits="LajtIt.Web.PaymentsBankAccountMarketplace" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Wpłaty/wypłaty/zwroty sklepów</h1>
    <table>
        <tr>
            <td style="width: 120px">Miesiąc</td>
            <td style="width: 120px">Konto firmy </td>
            <td style="width: 220px"></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="ddlMonth">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCompany" DataValueField="CompanyId" AppendDataBoundItems="true"
                    DataTextField="Name">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlShops" DataValueField="ShopId" DataTextField="Name" > 
                </asp:DropDownList>
            </td>

            <td>
                <asp:DropDownList runat="server" ID="ddlPaymentType">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="1">Wpłata</asp:ListItem>
                    <asp:ListItem Value="2">Zwrot</asp:ListItem>
                    <asp:ListItem Value="3">Wypłata</asp:ListItem>
             
                </asp:DropDownList> 
            </td> 
            <td>
                <asp:CheckBox runat="server" ID="chbNotAssigned" Text="Nieprzypisane" />
            </td>
            <td>
                <asp:Button ID="btnSearch" OnClick="btnSearch_Click" Text="Szukaj" runat="server" />
            </td>
            <td></td>
        </tr> 
    </table>
    <br />
    <br />
    <style>
        table.pad {
        }

            table.pad tr td {
                padding: 3px;
            }

                table.pad tr td.comment {
                    overflow-x: auto;
                    word-break: break-all;
                    word-wrap: break-word;
                    overflow-y: hidden;
                    width: 200px;
                }
    </style>
    <table style="width: 100%">
        <tr valign="top">
            <td style="width: 1100px">
                <asp:GridView runat="server" CssClass="pad" ID="gvPayments"  DataKeyNames="Id" AutoGenerateColumns="false" ShowFooter="true"
                    OnRowDataBound="gvPayments_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Width="100" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LitId"></asp:Literal>
                                <asp:CheckBox runat="server" ID="chbPOrder" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chbPOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbPOrder');" />
                            </HeaderTemplate>
                        </asp:TemplateField>

                        <asp:BoundField ItemStyle-Width="100" DataField="PaymentDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data wpłaty" />
                        <asp:BoundField ItemStyle-Width="120" DataField="Amount" DataFormatString="{0:C}" HeaderText="Kwota" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField ItemStyle-Width="120" DataField="TotalAmount" DataFormatString="{0:C}" HeaderText="Saldo" ItemStyle-HorizontalAlign="Right" />
                        
                        <asp:BoundField ItemStyle-Width="150" DataField="ShopPaymentType.Name" HeaderText="Typ płatności" />
                        
                        <asp:BoundField ItemStyle-Width="150" DataField="ClientName" HeaderText="Dane" />
                        <asp:BoundField ItemStyle-Width="150" DataField="Title" HeaderText="Tytuł" />
                        <asp:BoundField ItemStyle-Width="200" ItemStyle-CssClass="comment" DataField="OrderPayment.OrderPaymentAccountingType.Name" HeaderText="Rodzaj" />
                        <asp:TemplateField HeaderText="Zamówienie">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID ="hlOrder" NavigateUrl="/Order.aspx?id={0}" Target="_blank"></asp:HyperLink>
                                <asp:ImageButton runat="server" ID="imgOrderEdit" ImageUrl="~/Images/add.png" OnClick="imgOrderEdit_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:LinkButton runat="server" ID="lbtnAccoutingType"   />

                <asp:ModalPopupExtender ID="mpeBankAccount" runat="server"
                    TargetControlID="lbtnAccoutingType"
                    PopupControlID="Panel1"
                    BackgroundCssClass="modalBackground"
                    DropShadow="true"
                    PopupDragHandleControlID="Panel1"
                     CancelControlID="btnCancel"
                    />

                <asp:Panel runat="server" ID="Panel1" GroupingText="Paragon" BackColor="White" Style="width: 400px; background-color: white; height: 250px; padding: 10px">
                    <asp:Label runat="server" ID="lblBankAccount"></asp:Label><br />
                    <br />
                    <asp:DropDownList runat="server" ID="ddlOrderPayments" DataValueField="PaymentId" DataTextField="Text" ValidationGroup="receipt">
                    </asp:DropDownList>
                    <br />
                    <br />
                    <asp:Button runat="server" ID="btnOrderPaymentAssign" Text="Przypisz wpłatę w banku do zamówienia" UseSubmitBehavior="true"
                        OnClick="btnOrderPaymentAssign_Click" ValidationGroup="receipt"
                        OnClientClick="if(Page_ClientValidate()) return confirm('Zapisać?');" />
                    <asp:LinkButton runat="server" ID="btnCancel" Text="Anuluj" />
                </asp:Panel>
            </td>


            <td>
                <h2>Bieżący wynik</h2>

                <table>
                    <tr>

                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblCurrent"></asp:Label></td>
                    </tr>
                </table>
                <h2>System</h2>
                <table>
           
                    <tr>
                        <td>Wpłaty:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblIncome"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Wypłaty:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblOutcome"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Zwroty:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblRefund"></asp:Label></td>
                    </tr>
                </table> 
                <h2>Zarządzanie</h2>
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton runat="server" CausesValidation="false" ID="lbtnAutoAssign" OnClick="lbtnAutoAssign_Click"
                                Text="Dopasuj automatycznie" OnClientClick="return confirm('Przypisać automatycznie zapisy w banku do wpłat w systemie?');"></asp:LinkButton></td>
                    </tr>

                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>

                                    <asp:LinkButton runat="server" CausesValidation="false" ID="lbtnBankStatement" OnClick="lbtnBankStatement_Click"
                                        Text="Pobierz wyciąg"></asp:LinkButton>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lbtnBankStatement" />
                                </Triggers>
                            </asp:UpdatePanel>

                        </td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Panel ID="pnReport" GroupingText="Wczytaj raport płatności" runat="server">
                                <asp:DropDownList runat="server" ID="ddlFile">
                                    <asp:ListItem Value="0">DPD (Excel)</asp:ListItem>
                                    <asp:ListItem Value="1">Przelewy24 (XML)</asp:ListItem>
                                    <asp:ListItem Value="6">Ceneo (Excel)</asp:ListItem>
                                    <asp:ListItem Value="">Empik</asp:ListItem>
                                    <asp:ListItem Value="21">Morele (Excel)</asp:ListItem>
                                    <asp:ListItem Value="13">Polcard - terminal (Excel)</asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                <asp:UpdatePanel runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnRaportUpload" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <input type="file" id="myfile" accept="document/*" name="myfile" runat="server" size="100" />&nbsp;&nbsp;&nbsp;
                                     <br />
                                        <asp:Button ID="btnRaportUpload" runat="server" Text="Wgraj plik" OnClick="btnRaportUpload_Click" CausesValidation="false" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
    </table>

 

</asp:Content>

