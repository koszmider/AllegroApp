<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Payments.Dpd.aspx.cs" Inherits="LajtIt.Web.PaymentsDpd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Wpłaty DPD</h1>
    <table>
        <tr>
            <td style="width: 120px">Miesiąc</td>
            <td style="width: 120px">Konto firmy</td>
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
                <asp:CheckBox runat="server" ID="chbNotAssigned" Text="Bez przypisania" />
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

                        <asp:BoundField ItemStyle-Width="50" DataField="TrackingNumber" HeaderText="Typ przelewu" />
                        <asp:BoundField ItemStyle-Width="100" DataField="PaymentDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data wpłaty" />
                        <asp:BoundField ItemStyle-Width="120" DataField="DpdAmount" DataFormatString="{0:C}" HeaderText="Kwota" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField ItemStyle-Width="120" DataField="DpdTotalAmount" DataFormatString="{0:C}" HeaderText="Kwota" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField ItemStyle-Width="70" DataField="BatchNumber" HeaderText="Numer przelewu" /> 
                        <asp:BoundField ItemStyle-Width="200" ItemStyle-CssClass="comment" DataField="AccountingName" HeaderText="Rodzaj" />
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
                    <asp:Button runat="server" ID="btnOrderPaymentAssign" Text="Przypisz wpłatę DPD do zamówienia" UseSubmitBehavior="true"
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
               
                <h2>Zarządzanie</h2>
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton runat="server" CausesValidation="false" ID="lbtnAutoAssign" OnClick="lbtnAutoAssign_Click"
                                Text="Dopasuj automatycznie" OnClientClick="return confirm('Przypisać automatycznie przelewy DPD do wpłat w systemie?');"></asp:LinkButton></td>
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
     

</asp:Content>

