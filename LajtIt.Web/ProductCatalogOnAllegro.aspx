<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductCatalogOnAllegro.aspx.cs" Inherits="LajtIt.Web.ProductCatalogOnAllegro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:PostBackTrigger ControlID="gvProductCatalog" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel runat="server" ID="Panel1" GroupingText="Wyszukiwanie">
                <table>
                    <tr>
                        <td>
                            Dostawca
                        </td>
                        <td>
                            Nazwa/Tytuł/Kod produktu
                        </td> 
                        <td>
                            Dostawa
                        </td>
                        <td>
                            Promowane
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSearchSupplier" DataTextField="Name" DataValueField="SupplierId"
                                AppendDataBoundItems="true">
                                <asp:ListItem Value="0">-- wszyscy --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txbSearchName" MaxLength="50"></asp:TextBox>
                        </td> 
                        <td>
                            <asp:DropDownList runat="server" ID="ddlImport" DataTextField="Name" DataValueField="ImportId"
                                AppendDataBoundItems="true">
                                <asp:ListItem Value="0">-- wszystkie --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlPromotion" DataTextField="Name" DataValueField="ImportId"
                                AppendDataBoundItems="true">
                                <asp:ListItem Value="-1">-- wszystkie --</asp:ListItem>
                                <asp:ListItem Value="true">TAK</asp:ListItem>
                                <asp:ListItem Value="false">NIE</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chbReady" Text="Gotowy do wysłania" Checked="true" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Szukaj" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox runat="server" ID="chbAllegroNotCreated" Text="Pokaż produkty nie wystawione na Allegro" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chbUnlimited" Text="Do wyczerpania zap." />
                        </td>

                        <td>
                            <asp:DropDownList runat="server" ID="ddlSoldItems">
                                <asp:ListItem>--- sprzedaż ---</asp:ListItem>
                                <asp:ListItem>Sprzedaż - Tak</asp:ListItem>
                                <asp:ListItem>Sprzedaż - Nie</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
         <div style="background-color:White; font-size: 8pt;">
            <asp:GridView runat="server" ID="gvProductCatalog" DataKeyNames="ProductCatalogId" style="background-color:White; font-size: 8pt;"
                OnRowDataBound="gvProductCatalog_OnRowDataBound" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlPreview" Target="_blank" NavigateUrl="/ProductCatalogPreview.aspx?idProduct={0}&idSupplier={1}">
                                <asp:Image runat="server" ID="imgImage" Width="100" /></asp:HyperLink></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Produkt">
                        <ItemStyle Width="300" />
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlProduct" NavigateUrl="Product.aspx?id={0}"></asp:HyperLink><br />
                            (<asp:HyperLink runat="server" ID="hlProductAllegro" NavigateUrl="ProductCatalog.Allegro.aspx?id={0}"></asp:HyperLink>)
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kod<br>(kod dostawcy)" Visible="false">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCode"></asp:Label> 
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:BoundField DataField="D99" HeaderText="do wyczerp." ItemStyle-BackColor="LightGreen" />
                    <asp:BoundField DataField="D00" HeaderText="0" />
                    <asp:BoundField DataField="D01" HeaderText="1" />
                    <asp:BoundField DataField="D02" HeaderText="2" />
                    <asp:BoundField DataField="D03" HeaderText="3" />
                    <asp:BoundField DataField="D04" HeaderText="4" />
                    <asp:BoundField DataField="D05" HeaderText="5" />
                    <asp:BoundField DataField="D06" HeaderText="6" />
                    <asp:BoundField DataField="D07" HeaderText="7" />
                    <asp:BoundField DataField="D08" HeaderText="8" />
                    <asp:BoundField DataField="D09" HeaderText="9" />
                    <asp:BoundField DataField="D10" HeaderText="10" />
                    <asp:BoundField DataField="D11" HeaderText="11" />
                    <asp:BoundField DataField="D12" HeaderText="12" />
                    <asp:BoundField DataField="D13" HeaderText="13" />
                    <asp:BoundField DataField="D14" HeaderText="14" />
                    <asp:BoundField DataField="D15" HeaderText="15" />
                    <asp:BoundField DataField="D16" HeaderText="16" />
                    <asp:BoundField DataField="D17" HeaderText="17" />
                    <asp:BoundField DataField="D18" HeaderText="18" />
                    <asp:BoundField DataField="D19" HeaderText="19" />
                    <asp:BoundField DataField="D20" HeaderText="20" />
                    <asp:BoundField DataField="D21" HeaderText="21" />
                    <asp:BoundField DataField="D22" HeaderText="22" />
                    <asp:BoundField DataField="D23" HeaderText="23" />
                    <asp:BoundField DataField="D24" HeaderText="24" />
                    <asp:BoundField DataField="D25" HeaderText="25" />
                    <asp:BoundField DataField="D26" HeaderText="26" />
                    <asp:BoundField DataField="D27" HeaderText="27" />
                    <asp:BoundField DataField="D28" HeaderText="28" />
                    <asp:BoundField DataField="D29" HeaderText="29" />
                </Columns>
            </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
