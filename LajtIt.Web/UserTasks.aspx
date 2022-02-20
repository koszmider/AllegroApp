<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserTasks.aspx.cs" Inherits="LajtIt.Web.UserTasks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Zadania</h2>
    <asp:Timer runat="server" ID="tmInterval" OnTick="tmInterval_Tick" Interval="1000000"  ></asp:Timer>

    <table style="width: 100%;">
        <tr>
            <td style="width: 210px;">Pracownik</td>
            <td style="width: 210px">Status zadania</td>
            <td>Kategoria</td>
            <td style="width: 200px"></td>

        </tr>
        <tr valign="top">
            <td>

                <asp:DropDownList runat="server" ID="ddlUserName" Width="200" AppendDataBoundItems="true" DataTextField="UserName" DataValueField="UserId">
                    <asp:ListItem>-- wszyscy --</asp:ListItem> 
                </asp:DropDownList>
            </td>
            <td>
                <asp:ListBox runat="server" ID="lbxUserTaskStatus" DataTextField="Name" DataValueField="TaskTrackerStatusId" SelectionMode="Multiple" Width="200"></asp:ListBox>
            </td>
            <td>
                <asp:ListBox runat="server" ID="lbxTaskTypes" DataTextField="Name" DataValueField="TaskTypeId" SelectionMode="Multiple" Width="200"></asp:ListBox>
            </td>
            <td>
                <asp:Button runat="server" ID="btnShow" OnClick="btnShow_Click" Text="Pokaż" /></td>
            <td style="text-align: right;"><a href="UserTask.aspx">Dodaj nowe zadanie</a></td>
        </tr>

    </table>
    <style>
        .row {
            padding: 10px;
            margin: 10px;
        }
    </style>
    <asp:GridView runat="server" ID="gvTasks" AutoGenerateColumns="false" Width="100%" EmptyDataText="Brak zadań" DataKeyNames="TaskId" OnRowDataBound="gvTasks_RowDataBound">
        <Columns>
            <asp:BoundField DataField="TaskId" HeaderText="Id" ItemStyle-Width="20" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="row" HeaderStyle-CssClass="row" />
            <asp:BoundField ItemStyle-Width="70" DataField="TaskTrackerType" HeaderText="Kategoria" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="row" />
            <asp:TemplateField HeaderText="Nazwa" >
                <ItemStyle HorizontalAlign="left" />
                <ItemTemplate>
                    <asp:Image runat="server" ID="imgReccuringTask" ImageUrl="~/Images/reload.ico"  Width="20" ImageAlign="AbsMiddle" ToolTip="Zadanie cykliczne"/>
                    <asp:HyperLink runat="server" ID="hlUserTask" NavigateUrl="UserTask.aspx?id={0}"></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="70" DataField="TaskStatusName" HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="row" />
            <asp:BoundField ItemStyle-Width="70" DataField="UserAssigned" HeaderText="Przypisany do..." ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="row" />
            <asp:BoundField ItemStyle-Width="70" DataField="InsertDate" HeaderText="Data dodania" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="row" />
            <asp:BoundField ItemStyle-Width="70" DataField="FinishDate" HeaderText="Data zakończenia" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="row" />

            <asp:TemplateField HeaderText="Priorytet" ItemStyle-Width="70">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txbPriority" TextMode="Number" MaxLength="3" Width="50" Text="0" Style="text-align: center"></asp:TextBox>
                    <asp:Label runat="server" ID="lblPriority" Text="0"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>
    <div style="text-align:right">
        <asp:Button runat="server" ID="btnSaveTasks" Text="Zapisz" OnClick="btnSaveTasks_Click" />

    </div>
</asp:Content>
