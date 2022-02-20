<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OrderProcessing.aspx.cs" Inherits="LajtIt.Web.OrderProcessing" %>


<%@ Register Src="~/Controls/OrderShippingSimple.ascx" TagName="OrderShippingSimple" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="upPrint">
        <ContentTemplate>

            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <pre style="font-size: xx-large; color: Red;">
<asp:Label runat="server" ID="lblClientName" /><asp:Image runat="server" ID="imgCountryCode" Width="25" /></pre>
                    </td>
                    <td style="text-align:left">
                        <asp:Label runat="server" ID="lblShopName" Font-Bold="True" Font-Size="XX-Large" ForeColor="#33CC33" />
                    </td>
                    <td style="text-align:right">
                        <asp:Image runat="server" ID="imgCourier" Height="80" />
                    </td>
                    <td style="text-align: right;">
                        <asp:Timer runat="server" ID="tmInterval" OnTick="tmInterval_Tick" Interval="10000" Enabled="true"></asp:Timer>

                        <asp:Label runat="server" ID="lblShipment" /><asp:HyperLink runat="server" ID="hlOrder" ImageUrl="~/Images/view.png" NavigateUrl="/Order.aspx?id={0}"></asp:HyperLink><br />
                        <asp:HyperLink runat="server" ID="hlTracking" Target="_blank"></asp:HyperLink><br />
                        <asp:Label runat="server" ID="lblInpost" Font-Size="Larger"></asp:Label>

                        <asp:Panel runat="server" ID="pnGenerating" Height="20">
                            <img src="Images/progress.gif" style="height: 20px" alt="" />Generowanie etykiety. Czekaj...
                        </asp:Panel>
                    </td>
                    <td style="width: 60px; text-align: right;">

                        <asp:ImageButton runat="server" ID="imgbPrint" ImageUrl="~/Images/print.png" OnClick="imgbPrint_Click" Width="40" CausesValidation="false" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgbPrint" />
        </Triggers>
    </asp:UpdatePanel>

    <div style="clear: left">
    </div>



    <span style="font-size: large; color: black;"><a name="products"></a>
        <asp:GridView runat="server" ID="gvUserOrders" AutoGenerateColumns="false" AllowSorting="false"
            DataKeyNames="OrderProductId" Style="width: 100%" ShowFooter="true" OnRowDataBound="gvUserOrders_RowDataBound">
            <RowStyle HorizontalAlign="left" />
            <Columns>
                <asp:TemplateField HeaderText="Lp" ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:Literal ID="Literal1" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>.
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Produkt" ItemStyle-Width="460px">
                    <ItemTemplate>

                        <asp:Label ID="lblProductCatalogName" runat="server" Target="_blank"></asp:Label><br />
                        Kod:
                        <asp:Label ID="lblProductCatalogCode" runat="server" Target="_blank"></asp:Label><br />
                        <div style="text-align: right; width: 450px; font-size: medium; color: blue;">
                            <asp:Label ID="lblSubProducts" runat="server" Target="_blank"></asp:Label>
                        </div>
                        <asp:Panel
                            runat="server" ID="pnlComponents" GroupingText="Informacje o produkcie" Style="display: none;">
                            <br />
                            <asp:HyperLink Visible="false" runat="server" ID="hlImage" Target="_blank">
                                <asp:Image ID="imgImage" runat="server" Width="200" />
                            </asp:HyperLink>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Szt." ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Literal ID="Literal2" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal>
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <FooterTemplate>
                        <asp:Literal runat="server" ID="litQuantityFooter" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Szt." ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ValidationGroup="test" ID="ddlQuantity" Font-Size="X-Large"></asp:DropDownList>
                        <asp:RequiredFieldValidator Display="static"
                            ID="rvQuantity"
                            runat="server" ControlToValidate="ddlQuantity" ValidationGroup="test" Text="*"></asp:RequiredFieldValidator>
                        <asp:CompareValidator runat="server" ControlToValidate="ddlQuantity" ValueToCompare='<%# Eval("Quantity") %>' Text="*" ValidationGroup="test"
                            Operator="Equal" ID="cvQuantity"></asp:CompareValidator>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Data dostawy produktu" DataField="ProductDeliveryDate" ItemStyle-Width="150"
                    DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                <asp:TemplateField HeaderText="Komenatarz" ItemStyle-HorizontalAlign="left">
                    <ItemTemplate>
                        <asp:Literal ID="Literal3" runat="server" Text='<%#   Eval("Comment") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ValidationSummary runat="server" ValidationGroup="test" DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" />
    </span>
    <h2>Status</h2>
    <pre style="font-size: x-large; color: Red;">
<asp:Button runat="server" ID="btnSent" OnClick="btnSent_Click"   ValidationGroup="test" Width="100%"
    Text="Oznacz jako wysłane" /><asp:Label runat="server" ID="lblOrderStatus"></asp:Label></pre>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <asp:Panel runat="server" ID="pShippngChanges" GroupingText="Edycja parametrów przesyłki">
                <div>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Button Text="Zmień parametry paczki" Visible="true"  ID="lbtnShippingChange" runat="server" CausesValidation="false"
                                    OnClick="lbtnShippingChange_Click"></asp:Button> 
                            </td>
                            <td style="text-align:right;"><asp:Button Text="Zmień InPost na DPD" Visible="true"  ID="lbtnShippingModeChange" runat="server" CausesValidation="false"
                                    OnClientClick="return confirm('Czy napewno chcesz zmienić przesyłkę paczkomatową InPost na wysyłkę kurierską DPD?');"
                                    OnClick="lbtnShippingModeChange_Click"></asp:Button></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel runat="server" ID="pShipping" Visible="false">
                                    <table>
                                        <tr>
                                            <td style="width: 520px">
                                                <uc:OrderShippingSimple ID="ucOrderShippingSimple" runat="server" ValidateParcels="true"></uc:OrderShippingSimple>
                                                <asp:Button runat="server" ID="btnCourierSave" OnClick="btnCourierSave_Click"
                            OnClientClick=" if(Page_ClientValidate('os'))  return confirm('Zapisać zmiany?');" ValidationGroup="oc" Text="Utwórz nową przesyłkę" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>

            </asp:Panel>
        </ContentTemplate> 
    </asp:UpdatePanel>

</asp:Content>
