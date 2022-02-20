<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllegroActions.aspx.cs" Inherits="LajtIt.Web.AllegroActions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" GroupingText="Wyszukiwanie">
                <table>
                    <tr>
                        <td>
                            Status
                        </td>
                        <td>
                            <asp:DropDownList runat="server" Id="ddlStatus">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>aktywny</asp:ListItem>
                            <asp:ListItem>nieaktywny</asp:ListItem>
                            </asp:DropDownList>
                        </td> 
                        <td><asp:Button runat="server" ID="btnSearch" OnClick="tmTimer_OnTick" Text="Szukaj" />
                <asp:LinkButton runat="server" ID="btnTimer" OnClick="btnTimer_Click" Text="Włącz odświeżanie" /><asp:Image
                    runat="server" ID="imgVerifying" Visible="false" ImageUrl="/Images/progress.gif" />
                        </td>
                    </tr>
                    
                </table>
            </asp:Panel>
           
            <asp:Timer ID="tmTimer" runat="server" Interval="5000" OnTick="tmTimer_OnTick" Enabled="false">
            </asp:Timer>
            <asp:GridView runat="server" ID="gvAllegroItems" DataKeyNames="ActionId" PageSize="50"
                Style="font-size: 9pt" ShowFooter="true" OnRowDataBound="gvAllegroItems_OnRowDataBound"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="30" HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LitId"></asp:Literal><br />
                            <asp:CheckBox runat="server" ID="chbOrder" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" /></HeaderTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlPreview" Target="_blank" NavigateUrl="/ProductCatalog.Allegro.aspx?idProduct={0}"></asp:HyperLink><br />
                             <asp:HyperLink runat="server" ID="hlAllegroItem" Target="_blank" NavigateUrl="http://allegro.pl/show_item.php?item={0}"></asp:HyperLink></ItemTemplate> 
                    </asp:TemplateField> 
                    <asp:BoundField DataField="ActionType" HeaderText="Akcja"   />
           
                    
                    <asp:BoundField DataField="InsertDate" HeaderText="Data dodania" DataFormatString="{0:yyyy/MM/dd<br>HH:mm}"
                        HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <asp:CheckBoxField DataField="IsProcessed" HeaderText="Wykonany"   ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="ProcessedDate" HeaderText="Data wykonania" DataFormatString="{0:yyyy/MM/dd<br>HH:mm}"
                        HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                        
                    <asp:BoundField DataField="Comment" HeaderText="Uwagi"   />
                </Columns>
            </asp:GridView>
            <asp:Button runat="server" ID="btnDelete" OnClick="btnDelete_Click" Text="Usuń" OnClientClick="return cofirm('Usunąć zaznaczone?');" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
