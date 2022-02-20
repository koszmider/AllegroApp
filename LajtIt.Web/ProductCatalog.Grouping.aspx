<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true" CodeBehind="ProductCatalog.Grouping.aspx.cs"
    Inherits="LajtIt.Web.ProductGrouping" %>

<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upShop" runat="server">
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <uc:ProductMenu runat="server" SetTab="td12"></uc:ProductMenu>
    <asp:Panel runat="server">
        <asp:UpdatePanel runat="server" ID="upAllegro">
            <ContentTemplate> 
                <asp:GridView runat="server" CssClass="mytable" ID="gvGrouping" AutoGenerateColumns="false"
                    DataKeyNames="ProductCatalogGroupId"
                    OnRowDataBound="gvGrouping_RowDataBound" OnRowDeleting="gvGrouping_RowDeleting" EmptyDataText="Brak grup">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="~/Images/cancel.jpg" />
                         <asp:BoundField DataField="FamilyTypeName" HeaderText="Typ" />
                         <asp:BoundField DataField="FamilyName" HeaderText="Rodzina" />
                         <asp:BoundField DataField="GroupName" HeaderText="Grupa" />
                         <asp:BoundField DataField="FamilyTypeName" HeaderText="Typ" />
                         <asp:BoundField DataField="FamilySupplierName" HeaderText="Producent" />
                    </Columns>

                </asp:GridView> 
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
