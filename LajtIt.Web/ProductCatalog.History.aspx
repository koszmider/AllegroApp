<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true" CodeBehind="ProductCatalog.History.aspx.cs"
    Inherits="LajtIt.Web.ProductHistory" %>

<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Historia zmian</h1>
    <asp:UpdatePanel ID="upShop" runat="server">
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <uc:ProductMenu runat="server" SetTab="td10"></uc:ProductMenu>
     <asp:DropDownList runat="server" ID="ddlColumnName" DataTextField="ColumnName" DataValueField="ColumnName" AutoPostBack="true"
         OnSelectedIndexChanged="ddlColumnName_SelectedIndexChanged">
            
     </asp:DropDownList><br /><br />

    <asp:GridView runat="server" ID="gvHistory" AutoGenerateColumns="false" OnRowDataBound="gvHistory_RowDataBound">
        <Columns>
            <asp:BoundField HeaderText="Data zmiany" DataField="InsertDate" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
            <asp:BoundField HeaderText="Zmiany dokonał" DataField="InsertUser" />
            <asp:BoundField HeaderText="Pole" DataField="ColumnName" />
            <asp:TemplateField HeaderText="Wartość" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblValue"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Uwagi" DataField="Comment" />

        </Columns>

    </asp:GridView>
</asp:Content>
