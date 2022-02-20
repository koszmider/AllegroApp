<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ComplaintStats.aspx.cs" Inherits="LajtIt.Web.ComplaintStats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <h1>Statystyka reklamacji</h1>
   <asp:GridView runat="server" ID="gvOrderComplaintsByMonth" AutoGenerateColumns="false" Width="100%"
        AllowPaging="true" PageSize="30" ShowFooter="true" OnRowDataBound="gvOrderComplaintsByMonth_OnRowDataBound">
        <FooterStyle HorizontalAlign="Right" Font-Bold="true" />
       <HeaderStyle Font-Size="Small" />
        <Columns>
            <asp:BoundField DataField="Month" HeaderText="Miesiąc" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="Count" HeaderText="L.zam."  ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="70"/>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Center" />
                
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblC1"></asp:Label>
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lblC1"></asp:Label>
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lblC8"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblC8"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lblC10"></asp:Label>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblC10"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lblC2"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblC2"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lblC3"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblC3"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lblC4"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblC4"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lblC5"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblC5"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lblC6"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblC6"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lblC7"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblC7"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lblC9"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblC9"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>


        </Columns>
    </asp:GridView>
 
</asp:Content>
