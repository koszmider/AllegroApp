<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllegroBadges.aspx.cs" Inherits="LajtIt.Web.AllegroBadges" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Kampanie Allegro</h1>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       
        <ContentTemplate>
          <asp:GridView runat="server" ID="gvBadges" AutoGenerateColumns="false">
              <Columns>
                  <asp:HyperLinkField DataNavigateUrlFields="id" DataNavigateUrlFormatString="AllegroBadge.aspx?id={0}" HeaderText="Nazwa kampanii" DataTextField="name" />
                  <asp:BoundField DataField="type" HeaderText="Typ kampanii" />
                  <asp:HyperLinkField Target="_blank" DataNavigateUrlFields="regulationsLink" DataTextFormatString="{0}" DataTextField="regulationsLink" />
                  <asp:BoundField DataField="publication.from" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Od" />
                  <asp:BoundField DataField="publication.to" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Do" />
              </Columns>
          </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
