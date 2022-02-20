<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Payments.BankAccount.aspx.cs" Inherits="LajtIt.Web.PaymentsBankAccount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Konto bankowe</h1>
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
                <asp:DropDownList runat="server" ID="ddlAccountId" >
                    <asp:ListItem Value="1">ING</asp:ListItem>
                    <asp:ListItem Value="2">Przelewy24</asp:ListItem>
                </asp:DropDownList>
            </td>

            <td>
                <asp:DropDownList runat="server" ID="ddlPaymentType">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="DBIT">Obiążenie</asp:ListItem>
                    <asp:ListItem Value="CRDT">Uznanie</asp:ListItem>
                </asp:DropDownList>
                <asp:CheckBox runat="server" ID="chbNotAssigned" Text="Bez przypisania typu przelewu" />
                <asp:CheckBox runat="server" ID="chbOrderNotAssigned" Text="Bez przypisania zamówienia" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbSearch"></asp:TextBox></td>
            <td>
                <asp:Button ID="btnSearch" OnClick="btnSearch_Click" Text="Szukaj" runat="server" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:CheckBoxList RepeatDirection="Horizontal" runat="server" ID="chblBankAccountType" DataValueField="BankAccountTypeId" DataTextField="Name"></asp:CheckBoxList>
            </td>
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
                <asp:GridView runat="server" CssClass="pad" ID="gvPayments" DataKeyNames="Id" AutoGenerateColumns="false" ShowFooter="true"
                    OnRowDataBound="gvPayments_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Width="70" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LitId"></asp:Literal>
                                <asp:CheckBox runat="server" ID="chbPOrder" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chbPOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbPOrder');" />
                            </HeaderTemplate>
                        </asp:TemplateField>

                        <asp:BoundField ItemStyle-Width="50" DataField="TransferType" HeaderText="Typ przelewu" />
                        <asp:BoundField ItemStyle-Width="100" DataField="PaymentDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data wpłaty" />
                        <asp:BoundField ItemStyle-Width="120" DataField="Amount" DataFormatString="{0:C}" HeaderText="Kwota" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField ItemStyle-Width="250" DataField="ClientName" HeaderText="Klient" />
                        <asp:BoundField ItemStyle-Width="200" ItemStyle-CssClass="comment" DataField="Comment" HeaderText="Tytuł" />
                        <asp:BoundField ItemStyle-Width="200" ItemStyle-CssClass="comment" DataField="BankAccountTypeName" HeaderText="Rodzaj" />
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
                        <td>Razem:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblTotalAccount"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Ewidencja:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblEwidencja"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Faktury:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblFaktury"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Kasa fiskalna:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblKasa"></asp:Label></td>
                    </tr>
                </table>
                <h2>Bank</h2>
                <table>
                    <tr>
                        <td>Wpływy:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblCredit"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Wydatki:</td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblDebit"></asp:Label></td>
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
                </table>
            </td>
        </tr>
    </table>


    Zmień rodzaj rozliczenia dla zaznaczonych:
    <asp:DropDownList runat="server" ID="ddlBankAccountType" DataTextField="Name" DataValueField="BankAccountTypeId" AppendDataBoundItems="true">
        <asp:ListItem></asp:ListItem>

    </asp:DropDownList><asp:Button runat="server" ID="btnChange" OnClick="btnChange_Click" OnClientClick="return confirm('Zmienić?')" Text="Zmień" />


    <br />
    <br />
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>
            <input type="file" id="myfile" name="myfile" runat="server" size="100" />&nbsp;&nbsp;&nbsp;
                                     
                                    <asp:Button ID="btnSave" runat="server" Text="Zapisz pliki" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="save" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

