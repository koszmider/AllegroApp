<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Promotions.aspx.cs" Inherits="LajtIt.Web.Promotions" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Promocje</h1>
    <div style="text-align: right;">
        <asp:LinkButton runat="server" ID="lbtnPromotionAdd" Text="Dodaj nową promocję" OnClick="lbtnPromotionAdd_Click" OnClientClick="return confirm('Dodać nową promocję?');"></asp:LinkButton>
    </div>
    <asp:GridView runat="server" ID="gvPromotions" AutoGenerateColumns="false" DataKeyNames="PromotionId">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="PromotionId" DataNavigateUrlFormatString="Promotion.aspx?id={0}" DataTextField="Name" HeaderText="Nazwa" />
            <asp:BoundField DataField="StartDate" HeaderText="Od" DataFormatString="{0:yyyy/MM/dd}" />
            <asp:BoundField DataField="EndDate" HeaderText="Do" DataFormatString="{0:yyyy/MM/dd}" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Aktywny" />
        </Columns>
    </asp:GridView>
</asp:Content>
