<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OrdersProcessing.aspx.cs" Inherits="LajtIt.Web.OrdersProcessing" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Timer runat="server" ID="tmInterval" OnTick="tmInterval_Tick" Interval="15000"  ></asp:Timer>
    <div style="font-size: large;">
        <table>
            <tr valign="top">
                <td style="width: 800px">
                    Zamówienia wysłane: <asp:Label runat="server" ID="lblOrdersSent"></asp:Label><br />
                    <asp:Label runat="server" ID="lblOrderSentPerMinute"></asp:Label>

                </td>
                <td>
                    <table style="font-size: small; width: 520px" border="1">
                        <tr valign="top">
                            <td style="width: 120px"></td>
                            <td style="width: 100px">
                                <asp:CheckBox runat="server" ID="chbDpd" GroupName="shipping" Checked="true" AutoPostBack="true" OnCheckedChanged="rb_CheckedChanged" />
                                <img src="/Images/courier/1.png" width="70" /></td>
                            <td style="width: 100px">
                                <asp:CheckBox runat="server" ID="chbInpost" GroupName="shipping" OnCheckedChanged="rb_CheckedChanged" AutoPostBack="true" />
                                <img src="/Images/courier/4.png" width="70" /></td>
                       
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton runat="server" ID="rbExported" GroupName="status" Checked="true" Text="Do realizacji" OnCheckedChanged="rb_CheckedChanged" AutoPostBack="true" /></td>
                            <td style="font-weight: bold; text-align: center;">
                                <asp:Literal runat="server" ID="litExportedDpd"></asp:Literal>
                            </td>
                            <td style="font-weight: bold; text-align: center;">
                                <asp:Literal runat="server" ID="litExportedInpost"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton runat="server" ID="rbReady" GroupName="status" Text="Oczekujące" OnCheckedChanged="rb_CheckedChanged" AutoPostBack="true" />

                            </td>
                            <td style="text-align: center;" valign="top">
                                <asp:Literal runat="server" ID="litReadyDpd"></asp:Literal><br />

                                <asp:Button runat="server" ID="lbtnExport" Text="eksportuj" OnClick="lbtnExport_Click"
                                    OnClientClick="return confirm('Czy wyeksportować zamówienia do wysłania?');"></asp:Button>
 
                            </td>
                            <td style="text-align: center;">
                                <asp:Literal runat="server" ID="litReadyInpost"></asp:Literal><br />

                                <asp:Button runat="server" ID="Button1" Text="eksportuj" OnClick="lbtnExportInpost_Click"
                                    OnClientClick="return confirm('Czy wyeksportować zamówienia do wysłania?');"></asp:Button>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton runat="server" ID="rbSent" GroupName="status" Text="Wysłane" OnCheckedChanged="rb_CheckedChanged" AutoPostBack="true" />

                            </td>
                            <td style="text-align: center;">
                                <asp:TextBox runat="server" ID="txbSentDate" ></asp:TextBox>
                            </td>
                            <td style="text-align: center;">
                                <asp:Literal runat="server" ID="litSentInpost" Text="-"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="font-size: small;">
                    <asp:Panel runat="server" GroupingText="Inpost" Width="200" Visible="false">
                        <asp:UpdatePanel runat="server" ID="upCourier">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblInpostInfo" Text="Kurier Inpost niezamówiony"></asp:Label><br />
                                <asp:LinkButton runat="server" ID="lbtCallCourier" Text="Zamów kuriera" OnClick="lbtCallCourier_Click" CausesValidation="false" OnClientClick="return confirm('Czy zamówić kuriera po odbiór paczek?')" /><br />
                                <asp:LinkButton runat="server" ID="lbtCancelCourier" Text="Odwołaj kuriera" OnClick="lbtCancelCourier_Click" CausesValidation="false" OnClientClick="return confirm('Czy odwołać zamówionego kuriera?')" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <span style="position: absolute;">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upCourier">
                                <ProgressTemplate>
                                    <img src="Images/progress.gif" style="height: 20px" alt="" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </span>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView runat="server" ID="gvProducts" EnableViewState="true" AutoGenerateColumns="false" OnRowDataBound="gvProducts_RowDataBound" Style="width: 100%" DataKeyNames="OrderProductId">
                        <Columns>
                <asp:TemplateField HeaderText="Lp" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                    </HeaderTemplate>
                    <ItemTemplate> 
                           
                <asp:Literal runat="server" ID="LitId"></asp:Literal>
                        <asp:CheckBox runat="server" ID="chbOrder" />
                    </ItemTemplate>
                </asp:TemplateField>
                            <asp:HyperLinkField HeaderText="Id" ItemStyle-Width="40" DataNavigateUrlFields="OrderId" DataTextField="OrderId" DataNavigateUrlFormatString="OrderProcessing.aspx?id={0}"
                                Target="_blank" />
                            <asp:BoundField DataField="DeliveryDate" HeaderText="Data dekl." DataFormatString="{0:MM/dd}" ItemStyle-Width="60" />
                            <asp:BoundField DataField="Ean" HeaderText="Kod Ean" ItemStyle-Width="120" />
                            <asp:BoundField DataField="SupplierName" HeaderText="Producent" ItemStyle-Width="120" />
                            <asp:BoundField DataField="GroupName" HeaderText="Model" ItemStyle-Width="120" />
                            <asp:BoundField DataField="Code" HeaderText="Kod" ItemStyle-Width="120" />
                            <asp:BoundField DataField="Quantity" HeaderText="Ilość" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50" />
                            <asp:BoundField DataField="Comment" HeaderText="Uwagi" ItemStyle-Width="200" />
                        </Columns>
                    </asp:GridView>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            

                    <asp:LinkButton runat="server" Id="lbtnPrint" OnClick="lbtnPrint_Click" Text="Drukuj zaznaczone" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lbtnPrint" />
                        </Triggers>
                      </asp:UpdatePanel>
                </td>
            </tr>
        </table>

    </div>

</asp:Content>
