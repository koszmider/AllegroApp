<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminUserStats.aspx.cs" Inherits="LajtIt.Web.AdminUserStats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Raport obecności pracownika</h1><br /><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
            Miesiąc:
            <asp:DropDownList runat="server" ID="ddlMonths">
            </asp:DropDownList>

            Pracownik:
 
                <asp:DropDownList runat="server" ID="ddlUserName" Width="200" AppendDataBoundItems="true" DataTextField="UserName" DataValueField="UserName">
               
                </asp:DropDownList>

            <asp:Button runat="server" ID="btnShow" Text="Pokaż" OnClick="btnShow_Click" />
            <span style="position: absolute;">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </span>
            <br />
            <br />
            <table>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="gvResults" AutoGenerateColumns="false" OnRowDataBound="gvResults_OnRowDataBound" EmptyDataText="Brak danych">
                            <Columns>
                                <asp:TemplateField HeaderText="Lp.">
                                    <ItemStyle Width="20" HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LitId"></asp:Literal>

                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:BoundField DataField="Day" HeaderText="Dzień" />
                                <asp:BoundField DataField="Min" HeaderText="Początek" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                <asp:BoundField DataField="Max" HeaderText="Koniec" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                <asp:TemplateField HeaderText="Dzień">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDay" runat="server"></asp:Label></ItemTemplate>

                                </asp:TemplateField>
<asp:TemplateField HeaderText="Minuty">
    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblMin" runat="server"></asp:Label></ItemTemplate>

                                </asp:TemplateField>

                            </Columns>


                        </asp:GridView>
                        <br /><br />
                        Razem: <asp:Label runat="server" ID="lblMinutes"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
