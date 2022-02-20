<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ProductCatalog.Synonims.aspx.cs"
    Inherits="LajtIt.Web.ProductSynonims" %>

<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <uc:ProductMenu runat="server" SetTab="td4"></uc:ProductMenu>
    <asp:Panel ID="Panel1" runat="server" GroupingText="Synonimy nazw">
        <asp:TextBox runat="server" ID="txbSynonim" MaxLength="50" /><asp:Button runat="server"
            ID="btnSynonimAdd" Text="Dodaj" OnClick="btnSynonimAdd_Click" />
        <asp:GridView runat="server" ID="gvProductCatalogSynonims" DataKeyNames="ProductCatalogSynonimId"  
        OnRowDataBound="gvProductCatalogSynonims_OnRowDataBound" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Nazwa" />
                <asp:TemplateField HeaderText="Przypisywanie produktów">
                <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:UpdatePanel runat="server" ID="upAssign">
                            <ContentTemplate>
                                <asp:CheckBox runat="server" ID="chbAssign" AutoPostBack="true" OnCheckedChanged="chbAssign_OnCheckedChanged" />
                                <span style="position: absolute;">
                                    <asp:UpdateProgress ID="uprgAssign" runat="server" AssociatedUpdatePanelID="upAssign">
                                        <ProgressTemplate>
                                            <img src="Images/progress.gif" style="height: 20px" alt="" /></ProgressTemplate>
                                    </asp:UpdateProgress>
                                </span>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tworzenie aukcji">
                <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:UpdatePanel runat="server" ID="upCreate">
                            <ContentTemplate>
                                <asp:CheckBox runat="server" ID="chbCreate" AutoPostBack="true" OnCheckedChanged="chbCreate_OnCheckedChanged" />
                                <span style="position: absolute;">
                                    <asp:UpdateProgress ID="uprgCreate" runat="server" AssociatedUpdatePanelID="upCreate">
                                        <ProgressTemplate>
                                            <img src="Images/progress.gif" style="height: 20px" alt="" /></ProgressTemplate>
                                    </asp:UpdateProgress>
                                </span>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ItemTemplate>
                </asp:TemplateField> 
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
