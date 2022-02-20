<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductCatalog.Attributes.View.aspx.cs" Inherits="LajtIt.Web.ProductCatalog_Attributes_View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Specyfikacja pliku importowego</h1>
    <asp:GridView runat="server" ID="gvFileSpec" AutoGenerateColumns="false">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="FieldName"   DataTextField="FieldName" DataNavigateUrlFormatString="#{0}"    HeaderText="Nazwa pola w pliku" />
            <asp:BoundField DataField="FieldType"      HeaderText="Typ pola" />
            <asp:BoundField DataField="AttributeGroupName"      HeaderText="Grupa atrybutów" />
            <asp:BoundField DataField="AttributeName"      HeaderText="Atrybut" /> 
            <asp:CheckBoxField DataField="IsRequired"  HeaderText="Wymagany"/>
        </Columns>

    </asp:GridView>
    <h1>Atrybuty produktów</h1>
    <asp:Repeater runat="server" ID="rpGroups" OnItemDataBound="rpGroups_ItemDataBound">

        <HeaderTemplate>
            <table>
                <tr style="background-color: gray; color: white">
                    <td>Grupa atrybutów</td>
                    <td style="width: 200px;">Nazwa kolumny w pliku</td>
                    <td style="width: 200px;">Atrybut</td>
                    <td style="width: 200px;">Kod</td>
                </tr>
        </HeaderTemplate>
        <FooterTemplate></table></FooterTemplate>
        <ItemTemplate>
            <tr runat="server" id="trRow">
                <td style="width: 200px;">
                    <asp:Label runat="server" ID="lblGroup"></asp:Label></td>
                <td style="width: 200px;">
                    <b>
                        <%--<a name='<%# Eval("CategoryName")%>'></a>--%></b></td>
                <td style="width: 400px" colspan="2">
                    <asp:Repeater runat="server" ID="rpAttributes" OnItemDataBound="rpAttributes_ItemDataBound">
                        <HeaderTemplate>
                            <table style="width: 700px;">
                        </HeaderTemplate>
                        <FooterTemplate></table></FooterTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width: 200px;">
                                    <a name='<%# Eval("AttributeFieldName")%>'></a><asp:HyperLink runat="server" ID="hlAttribute" Target="_blank" NavigateUrl="/ProductCatalog.Attribute.aspx?id={0}"></asp:HyperLink><asp:Label runat="server" ID="lblExcelColumnName"></asp:Label></td>
                                <td style="width: 200px;">
                                    <asp:Label runat="server" ID="lblCode"></asp:Label></td>
                            </tr>
                        </ItemTemplate>

                    </asp:Repeater>

                </td>
            </tr>
        </ItemTemplate>

    </asp:Repeater>


</asp:Content>
