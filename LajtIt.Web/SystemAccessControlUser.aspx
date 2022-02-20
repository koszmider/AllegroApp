<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SystemAccessControlUser.aspx.cs" 
    Inherits="LajtIt.Web.SystemAccessControlUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table>
        <tr valign="top">
            <td>Użytkownik</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlAdminUser" DataValueField="UserId" DataTextField="UserName" AutoPostBack="true"  OnSelectedIndexChanged="btnUserAddEdit_Click">
       
                </asp:DropDownList>
                <asp:Button runat="server" ID="btnUserAddEdit" Text=">>>" OnClick="btnUserAddEdit_Click" />
            </td>
            <td>
                <asp:Panel runat="server" ID="pnUser" Visible="false">
                <table>
                    <tr>

                        <td>Nazwa</td>
                        <td>
                            <asp:TextBox runat="server" ID="txbUserName" MaxLength="254" Width="300"></asp:TextBox></td>
                    </tr>
                    <tr>

                        <td>Hasło</td>
                        <td>
                            <asp:TextBox runat="server" ID="txbPassword" TextMode="Password" MaxLength="254"    Width="300"></asp:TextBox> (wpisz by zmienić, zostaw puste by nie zmieniać)</td>
                    </tr>
                    <tr>

                        <td>Email</td>
                        <td>
                            <asp:TextBox runat="server" ID="txbEmail" TextMode="Email"  Width="300"></asp:TextBox></td>
                    </tr>
                    <tr>

                        <td>Prowizja sprzedażowa</td>
                        <td>
                            <asp:TextBox runat="server" ID="txbCommision" MaxLength="254" Width="300"></asp:TextBox></td>
                    </tr>
                    <tr>

                        <td>Aktywny</td>
                        <td>
                            <asp:CheckBox runat="server" ID="chbIsActive"  /></td>
                    </tr> 
                    <tr>
                        <td>Role</td>
                        <td><asp:CheckBoxList runat="server" ID="chblRoles" DataValueField="RoleId"  RepeatColumns="2" OnDataBound="chblRoles_DataBound"   DataTextField="RoleName"></asp:CheckBoxList></td>
                    </tr>
                    <tr>

                        <td colspan="2">
                            <asp:Button runat="server" ID="btnUserSave" Text="Zapisz" OnClick="btnUserSave_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lbtnUserSaveCancel" OnClick="lbtnUserSaveCancel_Click" CausesValidation="false" Text="Anuluj" ></asp:LinkButton>
                        </td>
                    </tr>

                </table>

                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <h2>Role</h2>

                <asp:GridView runat="server" ID="gvRoles" AutoGenerateColumns="false" OnRowDataBound="gvRoles_RowDataBound" ShowHeader="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td width="100">
                                            <b>
                                                <asp:Label runat="server" ID="lblRole"></asp:Label></b></td>
                                        <td>

                                            <asp:GridView runat="server" ID="gvUsers" AutoGenerateColumns="false" ShowHeader="false" EmptyDataText="-" BorderWidth="0">
                                                <Columns>
                                                    <asp:BoundField DataField="UserName" />

                                                </Columns>

                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>

                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </td>

        </tr>
    </table>


</asp:Content>
