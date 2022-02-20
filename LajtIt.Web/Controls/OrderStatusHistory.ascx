<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderStatusHistory.ascx.cs"
    Inherits="LajtIt.Web.Controls.OrderComment" %>
<div style="text-align: right;">
    <h2>Bieżący status:
        <asp:Label runat="server" ID="lblOrderStatus" ForeColor="Green" /></h2>
</div>
<table style="width: 100%">
    <tr valign="top">
        <td>
            <asp:GridView runat="server" ID="gvOrderStatusHistory" AutoGenerateColumns="false"
                Width="900" EmptyDataText="Brak">
                <Columns>
                    <asp:TemplateField HeaderText="Data" ItemStyle-Width="100">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("InsertDate", "{0:yy/MM/dd<br>HH:mm}") %>'></asp:Label><br />
                            <br />
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("InsertUser") %>'></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="100" />
                    <asp:TemplateField HeaderText="Komentarz">
                        <ItemTemplate>
                            <div style="max-height: 100px; overflow: auto; width: 700px;" onclick='this.style.maxHeight="";'>
                                <asp:Label runat="server" Text='<%# Eval("Comment") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
        <td>
            <asp:UpdatePanel runat="server" ID="upChangeStatus">
                <ContentTemplate>
                    Komentarz:<br />
                    <asp:TextBox runat="server" ID="txbComment" TextMode="MultiLine" Width="250" Height="100" /><br />
                    <br />
                    Zmień status na:<br />
                    <table style="width: 350px;">
                        <tr valign="top">
                            <td>
                                <asp:DropDownList runat="server" ID="ddlStatus" DataValueField="OrderStatusId" DataTextField="StatusName" Width="180"
                                    ValidationGroup="st" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_OnSelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rfv" ControlToValidate="ddlStatus"
                                    Text="*" ValidationGroup="st" />
                            </td>
                            <td>
                                <asp:Button runat="server" ValidationGroup="st" Text="Zapisz status" ID="btnChangeStatus"
                                    OnClick="btnChangeStatus_Click" OnClientClick="if(! confirm('Czy zmienić status zamówienia?')) return false;" /> 
                            </td>
                            <td style="width: 60px">
                                <asp:Panel runat="server" ID="pnlEmail" Visible="false" Style="text-align: right;">
                                    <asp:CheckBox runat="server" ID="chbSendEmail" Checked="true" ToolTip="Wyślij maila do klienta z powiadomieniem" />
                                    <asp:ImageButton runat="server" ID="lnbShowEmail" ImageUrl="~/Images/view.png" ToolTip="Pokaż maila" OnClick="lnbShowEmail_Click"></asp:ImageButton>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span style="position: absolute;">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upChangeStatus">
                                        <ProgressTemplate>
                                            <img src="Images/progress.gif" style="height: 20px" alt="" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </span>

                                <asp:Panel runat="server" ID="pEmpik" Visible="false">
                                    Zamówienie z Empik. Wybierz produkty, które możesz zrealizować a następnie ustaw status oczekiwanie na wpłatę.   
                         Musisz wybrać przynajmniej jeden.  Jeśli żadnego z produktów nie możemy zrealizować, wybierz status Usunięty.

                         <br />
                                    <br />
                                    <br />

                                </asp:Panel>
                                <asp:Panel runat="server" ID="pEmpikDelete" Visible="false">
                                    Zamówienie z Empik. Wszystkie produkty z zamówienia zostaną usunięte a całe zamówienie odrzucone.
                         Klient zostanie automatycznie powiadomiony przez Empik o usunięciu zamówienia.

                         <br />
                                    <br />
                                    <br />

                                </asp:Panel>
                                <asp:Panel runat="server" ID="pOrderComplaint" Visible="false">

                                    <asp:Panel runat="server" GroupingText="Opis problemu">
                                        Powód:<br />
                                        <asp:DropDownList runat="server" ID="ddlOrderComplaintType" DataValueField="OrderComplaintTypeId"
                                            DataTextField="ComplaintType">
                                        </asp:DropDownList><br />
                                        <asp:CheckBox runat="server" ID="chbCloseComplaint" Text="Zamknij reklamację" /><br />
                                        <asp:CheckBox runat="server" ID="chbClearOrder" Text="Wyzeruj zamówienie" /><br />
                                    </asp:Panel>

                                </asp:Panel>
                                <asp:Panel runat="server" ID="pOrderNotification" Visible="false">
                                    Wybierz z listy
                        <asp:CheckBoxList runat="server" ID="chblOrderNotificationList" DataValueField="NotificationTypeId"
                            DataTextField="Name">
                        </asp:CheckBoxList>
                                    lub wpisz powód (do klienta):<br />
                                    <asp:TextBox runat="server" ID="txbOrderNotificationComment" TextMode="MultiLine" Rows="10" Columns="40"></asp:TextBox>
                                </asp:Panel>

                            </td>
                        </tr>
                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
