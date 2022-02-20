<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Suppliers.aspx.cs" Inherits="LajtIt.Web.Suppliers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">
        function OpenProgressWindow(reqid) {
            window.showModalDialog("ResultPage.aspx?RequestId=" + reqid
                , "Progress  Window", "dialogHeight:200px; dialogWidth:380px");
        }
    </script>
    <h1>Dostawcy</h1>
    <div style="text-align: right"><a href="Supplier.New.aspx">nowy dostawca</a></div>
    <div style="text-align: right">
        <asp:DropDownList runat="server" ID="ddlShop" DataValueField="ShopId" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlShop_SelectedIndexChanged" AppendDataBoundItems="true">
            <asp:ListItem></asp:ListItem>
        </asp:DropDownList>

    </div>

    <div style="font-size: smaller">
        <asp:GridView runat="server" ID="gvSuppliers" AutoGenerateColumns="false" DataKeyNames="SupplierId" OnRowDataBound="gvSuppliers_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Lp" ItemStyle-Width="50px">
                    <HeaderTemplate>
                        <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("SupplierId") %>'></asp:Literal>.
                           
                <asp:Literal runat="server" ID="LitId"></asp:Literal>
                        <asp:CheckBox runat="server" ID="chbOrder" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Dostawca" ItemStyle-Width="200">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" id="hlSupplier" NavigateUrl="/Supplier.aspx?id={0}" ></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:BoundField DataField="DeliveryFixedId" HeaderText="Dekl.czas wysyłki (robocze)" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="EstimatedDeliveryTime" HeaderText="Akt.czas dost od prod (kalendarzowe)" ItemStyle-HorizontalAlign="Center" />

                <asp:CheckBoxField DataField="IsActive" HeaderText="Aktywny" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="AllegroDelivery" HeaderText="Cennik Allegro" ItemStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="ImportName" HeaderText="Aktualizacja" ItemStyle-HorizontalAlign="Center" />
                <asp:HyperLinkField DataTextField="LastImportDate" HeaderText="Akt.data" DataNavigateUrlFields="ImportUrl" Target="_blank"
                    DataTextFormatString="{0:yyyy/MM/dd HH:mm}" ItemStyle-HorizontalAlign="Center" />
                <asp:HyperLinkField DataTextField="B2bUrl" HeaderText="B2B" DataNavigateUrlFields="B2bUrl" Target="_blank" ItemStyle-Width="100"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="B2bEmail" DataFormatString="<a href=mailto:{0}>{0}</a>" HtmlEncodeFormatString="false" HeaderText="B2B kontakt" />
                <asp:TemplateField HeaderText="Szablon nazwy produktu">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txbTemplate" Width="400" ToolTip="np: Super promocja [SUPPLIER] na produkt [PRODUCT]"></asp:TextBox><asp:LinkButton runat="server" Text="kopiuj do pozostałych" ID="lbtnCopy" OnClick="lbtnCopy_Click"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
        <div style="text-align: right">
            <asp:Button runat="server" ID="btnSave" Text="Zapisz" OnClick="btnSave_Click" OnClientClick="return confirm('Zapisać?');" />

        </div>
        Akcja:
    <asp:DropDownList runat="server" ID="ddlAction" AutoPostBack="true" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged">
        <asp:ListItem Value="-1">-- wybierz --</asp:ListItem>
        <asp:ListItem Value="1">Nazwy</asp:ListItem>
        <asp:ListItem Value="2">Dostawy Allegro</asp:ListItem>
    </asp:DropDownList>
        <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional"
            ID="upAction">
            <ContentTemplate>
                <asp:Panel runat="server" GroupingText="Akcje" Visible="false" ID="pAction" ClientIDMode="Static">
                    <table>


                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="pnNames" runat="server" Visible="false">
                                    <table>
                                        <tr>
                                            <td>Zmień nazwy produktów<br />

                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnDeliveries" runat="server" Visible="false">
                                    <table>
                                        <tr>
                                            <td>Zmień nazwy cenniki dostaw Allegro<br />
                                                                   
                                               
                <asp:DropDownList runat="server" ID="ddlAllegroDeliveryType" DataTextField="Name"
                    DataValueField="DeliveryCostTypeId" AppendDataBoundItems="true">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList><br />
                                                lub gdy nie ma paczkomatu<br />
                                                <asp:DropDownList runat="server" ID="ddlAllegroAlternativeDeliveryType" DataTextField="Name"
                                                    DataValueField="DeliveryCostTypeId" AppendDataBoundItems="true">
                                                    <asp:ListItem></asp:ListItem>
                                                </asp:DropDownList>

                                            </td>
                                        </tr>

                                    </table>
                                </asp:Panel>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnAction" />
                                    </Triggers>

                                </asp:UpdatePanel>
                                <asp:Button runat="server" ID="btnAction" OnClick="btnAction_Click" OnClientClick="return confirm('Wykonać daną akcję?');"
                                    Text="Zapisz" />
                                <span style="position: absolute;">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upAction">
                                        <ProgressTemplate>
                                            <img src="Images/progress.gif" style="height: 20px" alt="" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </span>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
