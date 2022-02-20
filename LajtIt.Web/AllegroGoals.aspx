<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="AllegroGoals.aspx.cs" Inherits="LajtIt.Web.AllegroGoals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGoalNew" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ID="hidGoalId" />
    Grupa: <asp:DropDownList runat="server" ID="ddlProductGroupsSearch" DataTextField="GroupName"
        AutoPostBack="true" OnSelectedIndexChanged="ddlProductGroupsSearch_OnSelectedIndexChanged"
        DataValueField="ProductCatalogGroupId" AppendDataBoundItems="true">
        <asp:ListItem Value="0">-- wszystkie --</asp:ListItem>
    </asp:DropDownList> <asp:CheckBox runat="server" ID="cbhIsActive" Checked="true"  AutoPostBack="true" Text="Tylko aktywne" OnCheckedChanged="ddlProductGroupsSearch_OnSelectedIndexChanged"/>
    <div style="font-size: 6pt;">
        <asp:Repeater runat="server" ID="rpWeekdays" OnItemDataBound="rpWeekdays_OnItemDataBound">
            <HeaderTemplate>
                <h1>
                    Harmonogram</h1>
                <table style="width: 100%; border: solid 1px black;">
                    <tr>
            </HeaderTemplate>
            <FooterTemplate>
                </tr></table></FooterTemplate>
            <ItemTemplate>
                <td style="width: 14.3%">
                    <h1>
                        <asp:Label runat="server" Text='<%# Eval("Weekday") %>' ToolTip='<%# Eval("WeekdayId") %>'
                            ID="lblWeekday"></asp:Label></h1>
                    <asp:GridView runat="server" ID="gvGoalSchedule" AutoGenerateColumns="false" Width="100%"
                        OnRowDataBound="gvGoalSchedule_OnRowDataBound" OnRowDeleting="gvGoalSchedule_OnRowDeleting"
                        DataKeyNames="Hour" ShowHeader="false">
                        <Columns>
                            <asp:BoundField DataField="DateTimeValue" ReadOnly="true" ItemStyle-Width="20" />
                            <asp:BoundField DataField="ScheduleId" ReadOnly="true" Visible="false" />
                            <asp:BoundField DataField="Schedule" />
                            <asp:TemplateField>
                                <ItemStyle Width="40" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnbDeleteSchedule" CommandArgument='<%#Eval("ScheduleId") %>'
                                        OnClick="lnbDeleteSchedule_Click" Text="Usuń" OnClientClick="return confirm('Usunąć?');"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div style="text-align: right">
        <asp:LinkButton runat="server" CausesValidation="false" Text="Dodaj nowy" OnClick="lnbGoalNew_Click"></asp:LinkButton></div>
    <asp:Panel runat="server" GroupingText="Nowy cel" Visible="false" ID="pGoalNew">
        Nazwa:<br />
        <asp:TextBox runat="server" ID="txbGoalNewName" MaxLength="100" ValidationGroup="new"></asp:TextBox><asp:RequiredFieldValidator
            runat="server" ValidationGroup="new" Text="*" ControlToValidate="txbGoalNewName" /><br />
        Produkt:<br />
        <asp:DropDownList runat="server" ID="ddlProductGroups" DataTextField="GroupName" AutoPostBack="true" OnSelectedIndexChanged="ddlProductGroups_OnSelectedIndexChanged"
            DataValueField="ProductCatalogGroupId">
        </asp:DropDownList>
        <asp:DropDownList runat="server" ID="ddlProducts" DataTextField="Name" DataValueField="ProductCatalogId"
            ValidationGroup="new" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="new"
            Text="*" ControlToValidate="ddlProducts" />
        <br />
        Item Id:<br />
        <asp:TextBox runat="server" ID="txbGoalNewItemId" MaxLength="20" ValidationGroup="new"></asp:TextBox><asp:RequiredFieldValidator
            ID="RequiredFieldValidator2" runat="server" ValidationGroup="new" Text="*" ControlToValidate="txbGoalNewItemId" /><br />
        Kolor:<br />
        <asp:TextBox runat="server" ID="txbColor" MaxLength="6" ValidationGroup="new"></asp:TextBox><asp:RequiredFieldValidator
            ID="RequiredFieldValidator3" runat="server" ValidationGroup="new" Text="*" ControlToValidate="txbColor" /><asp:RegularExpressionValidator
                runat="server" ValidationGroup="new" Text="*" ControlToValidate="txbColor" ValidationExpression="[A-Fa-f\d]{6}"></asp:RegularExpressionValidator><br />
        Aktywny:<br />
        <asp:CheckBox runat="server" ID="chbGoalNewIsActive" /><br />
        <div style="text-align: right">
            <asp:LinkButton ID="lnbGoalNewCancel" runat="server" CausesValidation="false" Text="Anuluj"
                OnClick="lnbGoalNewCancel_Click"></asp:LinkButton>
            <asp:Button ID="btnGoalNew" runat="server" Text="Zapisz" OnClick="btnGoalNew_Click"
                ValidationGroup="new"></asp:Button></div>
        <asp:Panel ID="pNewSchedule" runat="server" GroupingText="Nowy harmonogram">
            <div style="text-align: right;">
                Dzień:
                <asp:DropDownList runat="server" ID="ddlWeekday" ValidationGroup="newSchedule">
                    <asp:ListItem Value="0">--- wybierz ---</asp:ListItem>
                    <asp:ListItem Value="1">Poniedziałek</asp:ListItem>
                    <asp:ListItem Value="2">Wtorek</asp:ListItem>
                    <asp:ListItem Value="3">Środa</asp:ListItem>
                    <asp:ListItem Value="4">Czwartek</asp:ListItem>
                    <asp:ListItem Value="5">Piątek</asp:ListItem>
                    <asp:ListItem Value="6">Sobota</asp:ListItem>
                    <asp:ListItem Value="7">Niedziela</asp:ListItem>
                </asp:DropDownList>
                <asp:CompareValidator ValidationGroup="newSchedule" runat="server" ValueToCompare="0"
                    ControlToValidate="ddlWeekday" Operator="Equal"></asp:CompareValidator>
                Minuta:
                <asp:TextBox runat="server" Text="1" MaxLength="2" Width="20" ID="txbHour" ValidationGroup="newSchedule" />:<asp:TextBox
                    runat="server" Text="1" MaxLength="2" Width="20" ID="txbMinute" ValidationGroup="newSchedule" /><asp:RequiredFieldValidator
                        ValidationGroup="newSchedule" runat="server" ControlToValidate="txbHour" Text="*"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                            ValidationGroup="newSchedule" runat="server" ControlToValidate="txbMinute" Text="*"></asp:RequiredFieldValidator><asp:Button
                                runat="server" ID="btnNewScheduleAdd" Text="Dodaj" OnClick="btnNewScheduleAdd_Click"
                                ValidationGroup="newSchedule" OnClientClick="return confirm('Zapisać?');" />
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:GridView runat="server" ID="gvGoals" DataKeyNames="GoalId" AutoGenerateColumns="false"
        OnRowDataBound="gvGoals_OnRowDataBound" OnRowCommand="gvGoals_OnRowCommand">
        <Columns>
            <asp:CheckBoxField DataField="IsActive" />
            <asp:ButtonField ButtonType="Link" CausesValidation="false" CommandName="editGoal"
                DataTextField="Name" HeaderText="Cel" ItemStyle-Width="200px" />
                <asp:HyperLinkField DataNavigateUrlFields="GoalId" DataNavigateUrlFormatString="AllegroGoal.aspx?id={0}"
                    Text="Raport" ItemStyle-Width="20" />
            <asp:TemplateField HeaderText="Produkt" ItemStyle-Width="460px">
                <ItemTemplate>
                    <asp:Image runat="server" ImageUrl='<%# Eval("ImageUrl") %>' Width="60" style="float:left;" />
                    <asp:Literal ID="litProductNameItem" runat="server" Text='<%# Eval("ProductCatalog") %>'></asp:Literal><br />
                    (<asp:HyperLink ID="hlProductAllegroName" Target="_blank" runat="server"></asp:HyperLink>)</ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="LastUpdateDate" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}"
                HeaderText="Ostatnia akt." />
        </Columns>
    </asp:GridView>
</asp:Content>
