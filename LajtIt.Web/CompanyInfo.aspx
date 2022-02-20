<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CompanyInfo.aspx.cs" Inherits="LajtIt.Web.CompanyInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel runat="server" ID="pnAdmin">
        <table>
            <tr>
                <td>Nazwa</td>
                <td>Moja firma</td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txbSearch"></asp:TextBox></td>
                <td><asp:CheckBox runat="server" ID="chbIsMyCompany" Text="Tylko moja firma" /></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Szukaj" OnClick="btnSearch_Click" />
                    <asp:LinkButton runat="server" ID="lbtnClear" Text="wyczyść" OnClick="lbtnClear_Click"></asp:LinkButton></td>
            </tr>
        </table>

    </asp:Panel>
    <div style="text-align: right"><a href="Company.aspx">nowa firma</a></div>

    <asp:GridView runat="server" ID="gvCompany" AutoGenerateColumns="false" OnRowDataBound="gvCompany_RowDataBound">
        <Columns>

            <asp:BoundField DataField="CompanyId" HeaderText="Id" />
            <asp:TemplateField HeaderText="Dane firmowe">
                <ItemTemplate>
                    <asp:TextBox runat="server" Rows="6" ReadOnly="true" Columns="40" TextMode="MultiLine" ID="txbCompanyInfo"
                        OnClick="this.select();"> </asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Konto">
                <ItemTemplate>
                    <asp:TextBox runat="server" Width="200" ReadOnly="true" Text='<%# Eval("BankAccountNumber") %>' OnClick="this.select();"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="DpdNumcat" HeaderText="Dpd" />
            <asp:HyperLinkField DataNavigateUrlFields="CompanyId" DataNavigateUrlFormatString="Company.aspx?id={0}" Text="edycja" />
        </Columns>

    </asp:GridView>

</asp:Content>
