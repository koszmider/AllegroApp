<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SystemAccessControl.aspx.cs"
    Inherits="LajtIt.Web.SystemAccessControl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table>
        <tr valign="top">
            <td>Strona</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlSystemPage" DataValueField="PageId" DataTextField="PageName" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="btnPageAddEdit_Click">
                    <asp:ListItem Value="0">--- dodaj nową stronę ---</asp:ListItem>

                </asp:DropDownList>
                <asp:Button runat="server" ID="btnPageAddEdit" Text=">>>" OnClick="btnPageAddEdit_Click" />
            </td>
            <td>
                <asp:Panel runat="server" ID="pnPage" Visible="false">
                    <table>
                        <tr>

                            <td>Nazwa</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbName" MaxLength="254" Width="300"></asp:TextBox></td>
                        </tr>
                        <tr>

                            <td>Url</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbUrl" MaxLength="254" Width="300"></asp:TextBox></td>
                        </tr>
                        <tr>

                            <td>Guid</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbGuidId" MaxLength="36" Width="300"></asp:TextBox></td>
                        </tr>
                        <tr>

                            <td>Aktywny</td>
                            <td>
                                <asp:CheckBox runat="server" ID="chbIsActive" /></td>
                        </tr>
                        <tr>

                            <td>Użyj w menu</td>
                            <td>
                                <asp:CheckBox runat="server" ID="chbCanUseInMenu" /></td>
                        </tr>
                        <tr>

                            <td>Wymaga uwierzytelnienia</td>
                            <td>
                                <asp:CheckBox runat="server" ID="chbRequiresAuthentication" /></td>
                        </tr>
                        <tr>
                            <td>Role</td>
                            <td>
                                <asp:CheckBoxList runat="server" ID="chblRoles" DataValueField="RoleId" RepeatColumns="2" OnDataBound="chblRoles_DataBound" DataTextField="RoleName"></asp:CheckBoxList></td>
                        </tr>
                        <tr><td colspan="2"><b>Akcje</b></td></tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:TextBox runat="server" ID="txbPageAction" MaxLength="254" Width="300"></asp:TextBox>
                                <asp:LinkButton runat="server" ID="lbtnPageAction" Text="Dodaj nową akcję" OnClick="lbtnPageAction_Click"></asp:LinkButton>
                            </td> 

                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView runat="server" ID="gvPageActions" AutoGenerateColumns="false" OnRowDataBound="gvPageActions_RowDataBound" DataKeyNames="PageActionId">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="Akcja" />
                                        <asp:BoundField DataField="GuidId" HeaderText="Id" />
                                        <asp:TemplateField>

                                            <ItemTemplate>
                                                <asp:CheckBoxList runat="server" Id="chbPageActionRole" RepeatColumns="3"></asp:CheckBoxList>

                                            </ItemTemplate>

                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </td>

                        </tr>
                        <tr>

                            <td colspan="2">
                                <asp:Button runat="server" ID="btnPageSave" Text="Zapisz" OnClick="btnPageSave_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lbtnPageSaveCancel" OnClick="lbtnPageSaveCancel_Click" CausesValidation="false" Text="Anuluj"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>

                </asp:Panel>
            </td>
        </tr>

    </table>

    <table>
        <tr valign="top">
            <td>Grupa</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlSystemGroup" DataValueField="GroupId" DataTextField="GroupName" AutoPostBack="true"
                    AppendDataBoundItems="true" OnSelectedIndexChanged="btnGroupAddEdit_Click">
                    <asp:ListItem Value="0">--- dodaj nową grupę ---</asp:ListItem>

                </asp:DropDownList>
                <asp:Button runat="server" ID="btnGroupAddEdit" Text=">>>" OnClick="btnGroupAddEdit_Click" />
            </td>
            <td>
                <asp:Panel runat="server" ID="pnGroup" Visible="false">
                    <table>
                        <tr>

                            <td>Nazwa</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbGroupName" MaxLength="254" Width="300"></asp:TextBox></td>
                        </tr>

                        <tr>

                            <td>Grupa nadrzędna</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSystemGroupParent" DataValueField="GroupId" DataTextField="GroupName"
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">--- główna grupa ---</asp:ListItem>

                                </asp:DropDownList></td>

                        </tr>
                        <tr>

                            <td>Aktywny</td>
                            <td>
                                <asp:CheckBox runat="server" ID="chbIsActiveGroup" /></td>
                        </tr>
                        <tr>

                            <td>Kolejność wyświetlenia</td>
                            <td>
                                <asp:TextBox runat="server" ID="txbGroupOrder" TextMode="Number" MaxLength="254" Width="300"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Strony</td>
                            <td>

                                <asp:DropDownList runat="server" ID="ddlPages" DataValueField="PageId" DataTextField="PageName">
                                </asp:DropDownList><asp:LinkButton runat="server" ID="lbtnPageAdd" Text="Dodaj stronę" OnClick="lbtnPageAdd_Click"></asp:LinkButton>
                                <asp:ReorderList ID="rlPages" runat="server"
                                    OnItemDataBound="rlPages_ItemDataBound"
                                    OnItemReorder="rlPages_ItemReorder"
                                    OnDeleteCommand="rlPages_DeleteCommand"
                                    DragHandleAlignment="Right"
                                    ItemInsertLocation="Beginning"
                                    DataKeyField="PageId"
                                    SortOrderField="Priority"
                                    Width="500"  LayoutType="Table"
                                    
                                    ViewStateMode="Enabled"
                                    PostBackOnReorder="true"
                                    ShowInsertItem="false"
                                    BorderWidth="1"
                                    AllowReorder="true">

                                    <ItemTemplate>
                                        <div style=" padding: 3px; width: 90%;">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <asp:HyperLink runat="server" Target="_blank" ID="hlPage">
                                                            <asp:Label runat="server" ID="lblPage"></asp:Label>
                                                        </asp:HyperLink>
                                                    </td>
                                                    <td style="width: 50px;">
                                                        <asp:ImageButton ID="btnDeleteEvent" runat="server" CommandName="Delete" OnClientClick="return confirm('Usunąć wybrany produkt?');"
                                                            CommandArgument='<%# Eval("PageId") %>' ImageUrl="~/Images/cancel.png" Width="20" ToolTip="Usuń z listy" /></td>
                                                </tr>
                                            </table>

                                        </div>
                                    </ItemTemplate>
                                    <ReorderTemplate>
                                        <asp:Label runat="server" ID="lblProductName"></asp:Label>
                                    </ReorderTemplate>
                                    <DragHandleTemplate>
                                        <asp:Image runat="server" ImageUrl="~/Images/updown.jpg" />
                                    </DragHandleTemplate>
                                    <InsertItemTemplate>
                                    </InsertItemTemplate>
                                </asp:ReorderList>

                            </td>
                        </tr>
                        <tr>

                            <td colspan="2">
                                <asp:Button runat="server" ID="btnGroupSave" Text="Zapisz" OnClick="btnGroupSave_Click" />
                                <asp:LinkButton runat="server" ID="lbtnGroupDelete" OnClick="lbtnGroupDelete_Click" OnClientClick="return confirm('Usunąć grupę i wszystkie podgrupy?');" CausesValidation="false" Text="Usuń" ForeColor="Red"></asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lbtnGroupSaveCancel" OnClick="lbtnGroupSaveCancel_Click" CausesValidation="false" Text="Anuluj"></asp:LinkButton>
                            </td>
                        </tr>

                    </table>

                </asp:Panel>
            </td>
        </tr>

    </table>
</asp:Content>
